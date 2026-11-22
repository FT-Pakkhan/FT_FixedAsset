using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using FT_FixedAsset.Module.BusinessObjects;
using FT_FixedAsset.Module.Controllers;

namespace FT_FixedAsset.Module.Web.Import
{
    /// <summary>
    /// All-or-nothing import. Every record goes through the same steps as saving on screen
    /// (property setters, F/A code numbering, validation rules, transaction checks,
    /// SBO_SP_PostTransactionNotice 'A', sp_FTS11_RecordChangeLog) inside ONE database transaction.
    /// Any error -> the whole transaction is rolled back and identity seeds are reset (no DocEntry gaps).
    /// </summary>
    public class ImportEngine
    {
        private class Col
        {
            public int Index;
            public string Code;
            public IMemberInfo Member;
            public bool OnLine;
            public int Order;
        }

        private class Record
        {
            public string DocNo;
            public List<ImportRow> Rows = new List<ImportRow>();
        }

        private readonly XafApplication app;
        private readonly GenController genCon;
        private readonly ImportDefinition def;
        private readonly ITypeInfo headerInfo, lineInfo;

        private UnitOfWork uow;
        private IObjectSpace os;
        private int qtyDec = DecimalPlaces.DefaultQtyDec, sumDec = DecimalPlaces.DefaultSumDec;
        private readonly Dictionary<string, object> refCache = new Dictionary<string, object>();

        public List<ImportResultLine> Results = new List<ImportResultLine>();
        public bool Success { get; private set; }
        public bool Committed { get; private set; }
        public int RecordCount { get; private set; }

        public ImportEngine(XafApplication app, GenController genCon, ImportDefinition def)
        {
            this.app = app;
            this.genCon = genCon;
            this.def = def;
            headerInfo = app.TypesInfo.FindTypeInfo(def.HeaderType);
            lineInfo = def.HasLines ? app.TypesInfo.FindTypeInfo(def.LineType) : null;
        }

        #region column mapping
        public bool IsKnownCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            if (string.Equals(code, ImportDefinitions.DocNoCode, StringComparison.OrdinalIgnoreCase)) return true;
            bool onLine;
            return FindMember(code, out onLine) != null;
        }

        private IMemberInfo FindMember(string code, out bool onLine)
        {
            onLine = false;
            string c = code.Trim();
            if (lineInfo != null && (c.StartsWith("D.", StringComparison.OrdinalIgnoreCase) || c.StartsWith("Line.", StringComparison.OrdinalIgnoreCase)))
            {
                onLine = true;
                return Member(lineInfo, c.Substring(c.IndexOf('.') + 1));
            }
            IMemberInfo m = Member(headerInfo, c);
            if (m == null && lineInfo != null)
            {
                m = Member(lineInfo, c);
                onLine = m != null;
            }
            return m;
        }

        private static IMemberInfo Member(ITypeInfo ti, string name)
        {
            return ti.Members.FirstOrDefault(m => m.IsPublic && string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        private List<Col> MapColumns(ImportTable table, out int docNoIndex)
        {
            docNoIndex = -1;
            var cols = new List<Col>();
            var ignored = new List<string>();
            for (int i = 0; i < table.Codes.Count; i++)
            {
                string code = table.Codes[i];
                if (string.IsNullOrWhiteSpace(code)) continue;
                if (string.Equals(code, ImportDefinitions.DocNoCode, StringComparison.OrdinalIgnoreCase)) { docNoIndex = i; continue; }

                bool onLine;
                IMemberInfo m = FindMember(code, out onLine);
                if (m == null) { ignored.Add(code + " (unknown field)"); continue; }
                if (ImportDefinitions.SystemFields.Contains(m.Name)) { ignored.Add(code + " (system field)"); continue; }
                if (m.IsReadOnly || m.IsList) { ignored.Add(code + " (read-only)"); continue; }

                var idx = m.FindAttribute<DevExpress.Persistent.Base.IndexAttribute>();
                cols.Add(new Col { Index = i, Code = code, Member = m, OnLine = onLine, Order = idx == null ? int.MaxValue : idx.Index });
            }
            if (ignored.Count > 0)
                Info(0, "", "Columns ignored: " + string.Join(", ", ignored));
            // apply in screen order: setters such as F/A Group / F/A Code fill dependent fields,
            // which the file may then override explicitly
            return cols.OrderBy(c => c.Order).ThenBy(c => c.Index).ToList();
        }
        #endregion

        public void Run(ImportTable table, bool commit)
        {
            int docNoIndex;
            List<Col> cols = MapColumns(table, out docNoIndex);
            List<Col> headerCols = cols.Where(c => !c.OnLine).ToList();
            List<Col> lineCols = cols.Where(c => c.OnLine).ToList();

            if (cols.Count == 0) { Error(0, "", "", "None of the column codes match fields of " + def.Caption + "."); return; }

            // assets / transactions post journal entries: not while a depreciation run is in progress (simulation is fine)
            if (commit && (def.IsTransaction || def.HeaderType == typeof(FTS11OFAM)))
            {
                string runMsg = DeprnRunLock.CheckAnyRun(genCon.getConnectionString());
                if (runMsg != null)
                {
                    Error(0, "", "", "Import blocked: " + runMsg);
                    Info(0, "", "Nothing was saved. You can still run Simulation now and import after the depreciation finishes.");
                    return;
                }
            }
            if (def.IsTransaction && !lineCols.Any(c => c.Member.Name == "U_FAKey"))
            { Error(0, "", "", "Column U_FAKey (F/A Code) is required for " + def.Caption + "."); return; }

            List<Record> records = Group(table, docNoIndex);
            RecordCount = records.Count;

            using (var conn = new SqlConnection(genCon.getConnectionString()))
            {
                conn.Open();
                Dictionary<string, Ident> identBefore = ReadIdentities(conn);

                var xpoInfo = XpoTypesInfoHelper.GetXpoTypeInfoSource();
                var store = new MSSqlConnectionProvider(conn, AutoCreateOption.None);
                var layer = new SimpleDataLayer(xpoInfo.XPDictionary, store);
                uow = new UnitOfWork(layer);
                os = new XPObjectSpace(app.TypesInfo, xpoInfo, () => uow);

                // quantity / amount decimals (General Settings), same rule as the screens
                FTS11OADM settings = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1"));
                qtyDec = settings != null ? settings.U_QtyDec : DecimalPlaces.DefaultQtyDec;
                sumDec = settings != null ? settings.U_SumDec : DecimalPlaces.DefaultSumDec;

                bool failed = preFailed;
                bool rolledBack = false;
                uow.ExplicitBeginTransaction();
                try
                {
                    foreach (Record rec in records)
                    {
                        if (!ProcessRecord(rec, headerCols, lineCols, failed))
                            failed = true;
                    }

                    if (!failed && commit)
                    {
                        uow.ExplicitCommitTransaction();
                        Committed = true;
                    }
                    else
                    {
                        uow.ExplicitRollbackTransaction();
                        rolledBack = true;
                    }
                }
                catch (Exception ex)
                {
                    failed = true;
                    Error(0, "", "", "Import stopped: " + Inner(ex).Message);
                    try { if (!rolledBack) { uow.ExplicitRollbackTransaction(); rolledBack = true; } } catch { }
                }

                if (rolledBack) ResetIdentities(conn, identBefore);
                Success = !failed;

                try { os.Dispose(); } catch { }
                try { uow.Dispose(); } catch { }
                try { layer.Dispose(); } catch { }
            }

            if (!Committed)
                foreach (ImportResultLine r in Results.Where(x => x.Status == "OK"))
                {
                    r.Status = "Valid";
                    r.Message = string.IsNullOrEmpty(r.Message) ? "Not saved" : r.Message + " - not saved";
                }

            if (Success && !Committed)
                Info(0, "", "Simulation passed - " + RecordCount + " record(s) can be imported. Nothing was saved (simulation).");
            else if (Committed)
                Info(0, "", RecordCount + " record(s) imported.");
            else
                Info(0, "", "Nothing was saved. Fix the errors above and import the file again.");
        }

        #region two-file transaction import (header file + lines file)
        private bool twoFiles;
        private bool preFailed;   // errors found before the records are processed (e.g. header/lines linking)
        private int HdrRow(ImportRow r) { return twoFiles && r.HeaderRowNumber > 0 ? r.HeaderRowNumber : r.RowNumber; }
        private string HdrTag { get { return twoFiles ? "[Header file] " : ""; } }
        private string LineTag { get { return twoFiles ? "[Lines file] " : ""; } }

        /// <summary>
        /// Transactions from two files linked by Import Doc No.: the header file has one row per document,
        /// the lines file one row per asset line. They are combined and then imported exactly like a single file.
        /// </summary>
        public void Run(ImportTable header, ImportTable lines, bool commit)
        {
            twoFiles = true;
            int hDoc = header.IndexOf(ImportDefinitions.DocNoCode), lDoc = lines.IndexOf(ImportDefinitions.DocNoCode);
            if (hDoc < 0) Error(0, "", "", "[Header file] Column " + ImportDefinitions.DocNoCode + " (Import Doc No.) is required.");
            if (lDoc < 0) Error(0, "", "", "[Lines file] Column " + ImportDefinitions.DocNoCode + " (Import Doc No.) is required.");
            if (hDoc < 0 || lDoc < 0) { Info(0, "", "Nothing was saved. Fix the errors above and import the files again."); return; }

            var headerByDoc = new Dictionary<string, ImportRow>(StringComparer.OrdinalIgnoreCase);
            var order = new List<string>();
            var noLineKeys = new HashSet<string>();   // optional lines: header rows without Import Doc No.
            foreach (ImportRow r in header.Rows)
            {
                string doc = Text(r.Values[hDoc]);
                if (doc == "" && def.LinesOptional) { string k = "\u0001" + r.RowNumber; headerByDoc[k] = r; order.Add(k); noLineKeys.Add(k); }
                else if (doc == "") Error(r.RowNumber, "", "", "[Header file] Import Doc No. is empty.");
                else if (headerByDoc.ContainsKey(doc)) Error(r.RowNumber, doc, "", "[Header file] Import Doc No. '" + doc + "' appears more than once (one row per document).");
                else { headerByDoc[doc] = r; order.Add(doc); }
            }

            var linesByDoc = new Dictionary<string, List<ImportRow>>(StringComparer.OrdinalIgnoreCase);
            foreach (ImportRow r in lines.Rows)
            {
                string doc = Text(r.Values[lDoc]);
                if (doc == "") { Error(r.RowNumber, "", "", "[Lines file] Import Doc No. is empty."); continue; }
                if (!headerByDoc.ContainsKey(doc)) { Error(r.RowNumber, doc, "", "[Lines file] Import Doc No. '" + doc + "' is not in the header file."); continue; }
                List<ImportRow> l;
                if (!linesByDoc.TryGetValue(doc, out l)) linesByDoc[doc] = l = new List<ImportRow>();
                l.Add(r);
            }
            if (!def.LinesOptional)
                foreach (string doc in order.Where(d => !linesByDoc.ContainsKey(d)))
                    Error(headerByDoc[doc].RowNumber, doc, "", "[Header file] Import Doc No. '" + doc + "' has no lines in the lines file.");

            // linking errors fail the import, but the documents that do link are still checked,
            // so every problem is reported in one go
            preFailed = Results.Any(r => r.Status == "Error");
            if (!def.LinesOptional) order = order.Where(d => linesByDoc.ContainsKey(d)).ToList();
            if (order.Count == 0) { Info(0, "", "Nothing was saved. Fix the errors above and import the files again."); return; }

            // combined layout: ImportDocNo | header columns | line columns
            var combined = new ImportTable();
            combined.Codes.Add(ImportDefinitions.DocNoCode);
            combined.Captions.Add("Import Doc No.");
            var hCols = Enumerable.Range(0, header.Codes.Count).Where(i => i != hDoc && header.Codes[i] != "").ToList();
            var lCols = Enumerable.Range(0, lines.Codes.Count).Where(i => i != lDoc && lines.Codes[i] != "").ToList();
            foreach (int i in hCols) { combined.Codes.Add(header.Codes[i]); combined.Captions.Add(header.Captions[i]); }
            foreach (int i in lCols) { combined.Codes.Add(lines.Codes[i]); combined.Captions.Add(lines.Captions[i]); }

            foreach (string doc in order)
            {
                ImportRow h = headerByDoc[doc];
                string docOut = noLineKeys.Contains(doc) ? "" : doc;
                List<ImportRow> docLines;
                if (!linesByDoc.TryGetValue(doc, out docLines))
                {
                    // optional lines: the header alone (empty line columns -> no line is created)
                    var only = new List<object> { docOut };
                    only.AddRange(hCols.Select(i => h.Values[i]));
                    only.AddRange(lCols.Select(i => (object)null));
                    combined.Rows.Add(new ImportRow { RowNumber = h.RowNumber, HeaderRowNumber = h.RowNumber, Values = only.ToArray() });
                    continue;
                }
                foreach (ImportRow l in docLines)
                {
                    var vals = new List<object> { docOut };
                    vals.AddRange(hCols.Select(i => h.Values[i]));
                    vals.AddRange(lCols.Select(i => l.Values[i]));
                    combined.Rows.Add(new ImportRow { RowNumber = l.RowNumber, HeaderRowNumber = h.RowNumber, Values = vals.ToArray() });
                }
            }
            Run(combined, commit);
        }
        #endregion

        private List<Record> Group(ImportTable table, int docNoIndex)
        {
            var list = new List<Record>();
            var byDoc = new Dictionary<string, Record>(StringComparer.OrdinalIgnoreCase);
            foreach (ImportRow row in table.Rows)
            {
                string docNo = docNoIndex >= 0 ? Text(row.Values[docNoIndex]) : "";
                Record rec;
                if (!def.HasLines || docNo == "" || !byDoc.TryGetValue(docNo, out rec))
                {
                    rec = new Record { DocNo = docNo };
                    list.Add(rec);
                    if (def.HasLines && docNo != "") byDoc[docNo] = rec;
                }
                rec.Rows.Add(row);
            }
            return list;
        }

        /// <returns>false when the record has errors</returns>
        private bool ProcessRecord(Record rec, List<Col> headerCols, List<Col> lineCols, bool alreadyFailed)
        {
            var errors = new List<KeyValuePair<int, string>>();
            ImportRow first = rec.Rows[0];
            XPBaseObject header = null;
            string code = "";
            try
            {
                header = (XPBaseObject)os.CreateObject(def.HeaderType);
                Apply(header, headerCols, first, errors, HdrRow(first), HdrTag);

                if (def.HasLines)
                {
                    // header columns must be identical on every row of the same document
                    foreach (Col c in headerCols)
                    {
                        var distinct = rec.Rows.Select(r => Text(r.Values[c.Index])).Where(s => s != "").Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                        if (distinct.Count > 1)
                            errors.Add(new KeyValuePair<int, string>(first.RowNumber, c.Code + ": different values within the same Import Doc No. (" + string.Join(" / ", distinct) + ")"));
                    }
                }

                // a wrong code / date / number is reported on its own - the follow-on
                // "must not be empty" messages it would cause are just noise
                bool headerValueErrors = errors.Count > 0;
                var badLines = new HashSet<int>();
                lineRows = new List<int>();   // file row index of each created line
                if (def.HasLines)
                {
                    for (int i = 0; i < rec.Rows.Count; i++)
                    {
                        // optional lines (asset + referenced documents): a row with all line cells empty has no line
                        if (def.LinesOptional && lineCols.All(c => Text(rec.Rows[i].Values[c.Index]) == "")) continue;
                        int before = errors.Count;
                        var line = (XPBaseObject)os.CreateObject(def.LineType);
                        line.SetMemberValue(def.LineParent, header);
                        Apply(line, lineCols, rec.Rows[i], errors, rec.Rows[i].RowNumber, LineTag);
                        if (errors.Count > before) badLines.Add(lineRows.Count);
                        lineRows.Add(i);
                    }
                }

                if (!headerValueErrors && def.HeaderType == typeof(FTS11OFAM))
                    NumberAsset((FTS11OFAM)header, first.RowNumber, errors);

                code = RecordCode(header);
                if (!headerValueErrors)
                    Validate(header, HdrRow(first), errors, HdrTag);
                if (def.HasLines)
                {
                    List<XPBaseObject> lineObjs = Lines(header).ToList();
                    for (int i = 0; i < lineObjs.Count && i < lineRows.Count; i++)
                        if (!badLines.Contains(i))
                            Validate(lineObjs[i], rec.Rows[lineRows[i]].RowNumber, errors, LineTag);
                    if (!headerValueErrors && def.IsTransaction)
                        TransactionRules(header, rec, errors, badLines);
                }

                // every clean record is written inside the import transaction, even after another record failed
                // (all of it is rolled back at the end then) - so later rows see it exactly as in a real import:
                // next F/A code, quantity already used, posting-procedure checks
                if (errors.Count == 0)
                {
                    os.CommitChanges();
                    int key = Convert.ToInt32(headerInfo.KeyMember.GetValue(header));
                    string obj = Convert.ToString(header.GetMemberValue("Object"));
                    foreach (string e in RunProc("EXEC SBO_SP_PostTransactionNotice " + Sql(obj) + ", 'A', 1, 'DocEntry', " + Sql(key.ToString())))
                        errors.Add(new KeyValuePair<int, string>(HdrRow(first), HdrTag + e));
                    foreach (string e in RunProc("EXEC sp_FTS11_RecordChangeLog " + Sql(obj) + ", 1, 'DocEntry', " + Sql(key.ToString()) + ", 1"))
                        errors.Add(new KeyValuePair<int, string>(HdrRow(first), HdrTag + e));
                    code = RecordCode(header);
                    if (def.IsTransaction) code = "Doc " + key;
                }
            }
            catch (Exception ex)
            {
                errors.Add(new KeyValuePair<int, string>(HdrRow(first), HdrTag + Inner(ex).Message));
            }

            if (errors.Count > 0)
            {
                Discard(header);   // keep the failed record out of the next write
                foreach (var e in errors.GroupBy(x => x.Key + "|" + x.Value).Select(g => g.First()))
                    Error(e.Key, rec.DocNo, code, e.Value);
                return false;
            }
            string msg = def.IsTransaction ? rec.Rows.Count + " " + def.LinesCaption
                       : def.HasLines && lineRows.Count > 0 ? lineRows.Count + " " + def.LinesCaption : "";
            Results.Add(new ImportResultLine { Id = Results.Count + 1, Row = HdrRow(first), DocNo = rec.DocNo, Code = code, Status = alreadyFailed ? "Valid" : "OK", Message = alreadyFailed ? "Row is valid (not saved because of other errors)" : msg });
            return true;
        }

        private List<int> lineRows = new List<int>();

        private void Discard(XPBaseObject header)
        {
            if (header == null) return;
            try
            {
                if (def.HasLines)
                    foreach (XPBaseObject line in Lines(header)) os.RemoveFromModifiedObjects(line);
                os.RemoveFromModifiedObjects(header);
            }
            catch { }
        }

        private IEnumerable<XPBaseObject> Lines(XPBaseObject header)
        {
            return ((IEnumerable)header.GetMemberValue(def.LinesCollection)).Cast<XPBaseObject>().ToList();
        }

        private string RecordCode(XPBaseObject obj)
        {
            IMemberInfo dm = headerInfo.DefaultMember;
            return dm == null || def.IsTransaction ? "" : Convert.ToString(dm.GetValue(obj));
        }

        #region values
        private void Apply(XPBaseObject obj, List<Col> cols, ImportRow row, List<KeyValuePair<int, string>> errors, int rowNo, string tag)
        {
            foreach (Col c in cols)
            {
                object raw = row.Values[c.Index];
                if (raw == null || (raw is string && ((string)raw).Trim() == "")) continue;
                try
                {
                    string err;
                    object v = ConvertValue(raw, c.Member, out err);
                    if (err != null) { errors.Add(new KeyValuePair<int, string>(rowNo, tag + c.Code + ": " + err)); continue; }
                    c.Member.SetValue(obj, v);
                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<int, string>(rowNo, tag + c.Code + ": " + Inner(ex).Message));
                }
            }
        }

        private object ConvertValue(object raw, IMemberInfo m, out string err)
        {
            err = null;
            Type t = Nullable.GetUnderlyingType(m.MemberType) ?? m.MemberType;
            string s = Text(raw);

            if (t == typeof(string)) return s;

            if (typeof(XPBaseObject).IsAssignableFrom(t) || (m.MemberTypeInfo != null && m.MemberTypeInfo.IsPersistent))
            {
                object o = Resolve(t, s);
                if (o == null) err = "'" + s + "' not found in " + CaptionOf(t) + ".";
                return o;
            }

            if (t == typeof(DateTime))
            {
                if (raw is DateTime) return (DateTime)raw;
                if (raw is double) return DateTime.FromOADate((double)raw);
                DateTime d;
                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy", "yyyy-MM-dd", "yyyyMMdd", "dd.MM.yyyy", "dd/MM/yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss" };
                if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out d)) return d;
                if (DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.None, out d)) return d;
                err = "'" + s + "' is not a valid date (use dd/MM/yyyy).";
                return null;
            }

            if (t == typeof(bool))
            {
                if (raw is bool) return (bool)raw;
                if (raw is double) return (double)raw != 0;
                switch (s.ToUpperInvariant())
                {
                    case "TRUE": case "Y": case "YES": case "1": return true;
                    case "FALSE": case "N": case "NO": case "0": return false;
                }
                err = "'" + s + "' is not Yes/No (use TRUE/FALSE or Y/N).";
                return null;
            }

            if (t.IsEnum)
            {
                try { return Enum.Parse(t, s, true); } catch { err = "'" + s + "' is not valid. Allowed: " + string.Join(", ", Enum.GetNames(t)); return null; }
            }

            if (t == typeof(int) || t == typeof(long) || t == typeof(short) || t == typeof(double) || t == typeof(decimal) || t == typeof(float))
            {
                double d;
                if (raw is double) d = (double)raw;
                else if (!double.TryParse(s.Replace(" ", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out d)
                      && !double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out d))
                { err = "'" + s + "' is not a number."; return null; }

                if (t == typeof(int) || t == typeof(long) || t == typeof(short))
                {
                    if (Math.Abs(d - Math.Round(d)) > 0.0000001) { err = "'" + s + "' must be a whole number."; return null; }
                    return Convert.ChangeType(Math.Round(d), t, CultureInfo.InvariantCulture);
                }

                // quantity / amount decimals from General Settings (same rule as the screens)
                NumberKind kind = DecimalPlaces.KindOf(m.Owner.Type, m.Name, m.MemberType);
                double? applied = DecimalPlaces.Apply(kind, DecimalPlaces.Places(kind, qtyDec, sumDec), d, out err);
                if (applied == null) return null;
                return Convert.ChangeType(applied.Value, t, CultureInfo.InvariantCulture);
            }

            return Convert.ChangeType(s, t, CultureInfo.InvariantCulture);
        }

        /// <summary>Lookup by code (default property, Code, FormatCode, ...) and, for numbers, by key (DTW layout).</summary>
        private object Resolve(Type t, string text)
        {
            string cacheKey = t.FullName + "|" + text;
            object found;
            if (refCache.TryGetValue(cacheKey, out found)) return found;

            ITypeInfo ti = app.TypesInfo.FindTypeInfo(t);
            var names = new List<string>();
            if (ti.DefaultMember != null) names.Add(ti.DefaultMember.Name);
            foreach (string n in new[] { "Code", "FormatCode", "AcctCode", "PrjCode", "PrcCode", "CurrCode", "CardCode", "U_FACode", "U_FAGrpCod", "U_IndCode", "DocType", "Series" })
                if (ti.FindMember(n) != null) names.Add(n);

            foreach (string n in names.Distinct())
            {
                IMemberInfo m = ti.FindMember(n);
                if (m == null || m.MemberType != typeof(string)) continue;
                found = os.FindObject(t, new BinaryOperator(n, text), true);
                if (found != null) break;
            }

            if (found == null && ti.KeyMember != null)
            {
                Type kt = Nullable.GetUnderlyingType(ti.KeyMember.MemberType) ?? ti.KeyMember.MemberType;
                try
                {
                    object key = kt == typeof(string) ? (object)text
                        : Convert.ChangeType(double.Parse(text, CultureInfo.InvariantCulture), kt, CultureInfo.InvariantCulture);
                    found = os.GetObjectByKey(t, key);
                }
                catch (FormatException) { }
                catch (InvalidCastException) { }
                catch (OverflowException) { }
            }

            refCache[cacheKey] = found;
            return found;
        }

        private string CaptionOf(Type t)
        {
            var cls = app.Model.BOModel.GetClass(t);
            return cls != null ? cls.Caption : t.Name;
        }

        private static string Text(object v)
        {
            if (v == null) return "";
            if (v is double) return ((double)v).ToString("0.##########", CultureInfo.InvariantCulture);
            if (v is DateTime) return ((DateTime)v).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return Convert.ToString(v, CultureInfo.InvariantCulture).Trim();
        }
        #endregion

        #region business rules
        private void NumberAsset(FTS11OFAM OFAM, int rowNo, List<KeyValuePair<int, string>> errors)
        {
            if (!string.IsNullOrEmpty(OFAM.U_FACode)) return;   // code given in file

            FTS11OADM OADM = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = 1"));
            string docType = GenController.GetFACodeDocType(OADM, OFAM);
            FTS11NNM1 code = os.FindObject<FTS11NNM1>(CriteriaOperator.Parse("DocType=?", docType));
            if (code == null)
            {
                errors.Add(new KeyValuePair<int, string>(rowNo, "F/A Code is empty and no numbering is set for " +
                    (OADM != null && OADM.U_FACodeBy != null ? OADM.U_FACodeBy.Name : "F/A Code") + " '" + docType + "'."));
                return;
            }
            genCon.AutoNumbering(OADM, code, OFAM, os);
            if (string.IsNullOrEmpty(OFAM.U_FACode))
                errors.Add(new KeyValuePair<int, string>(rowNo, "F/A Code could not be generated (check Acquisition / Effective Date)."));
        }

        private void Validate(object obj, int rowNo, List<KeyValuePair<int, string>> errors, string tag)
        {
            RuleSetValidationResult r = Validator.RuleSet.ValidateTarget(os, obj, DefaultContexts.Save);
            foreach (RuleSetValidationResultItem item in r.Results.Where(x => x.State == ValidationState.Invalid))
                errors.Add(new KeyValuePair<int, string>(rowNo, tag + item.ErrorMessage));
        }

        /// <summary>Same checks as WebModifyControllers.SaveAndClose for the transaction screens.</summary>
        private void TransactionRules(XPBaseObject header, Record rec, List<KeyValuePair<int, string>> errors, HashSet<int> badLines)
        {
            int rowNo = HdrRow(rec.Rows[0]);
            List<XPBaseObject> lines = Lines(header).ToList();
            if (lines.Count == 0) { errors.Add(new KeyValuePair<int, string>(rowNo, HdrTag + "No Asset selected.")); return; }

            // same asset twice in one document: reported on the repeated line
            var seen = new HashSet<int>();
            for (int i = 0; i < lines.Count; i++)
            {
                var f = lines[i].GetMemberValue("U_FAKey") as FTS11OFAM;
                if (f != null && !seen.Add(f.DocEntry))
                    errors.Add(new KeyValuePair<int, string>(rec.Rows[Math.Min(i, rec.Rows.Count - 1)].RowNumber,
                        LineTag + f.U_FACode + " - " + f.U_FAName + ": Duplicate Asset found in list."));
            }

            DateTime docDate = (DateTime)header.GetMemberValue("U_DocDate");
            for (int i = 0; i < lines.Count; i++)
            {
                if (badLines.Contains(i)) continue;   // already reported (wrong value in the file)
                XPBaseObject line = lines[i];
                int lineRow = rec.Rows[Math.Min(i, rec.Rows.Count - 1)].RowNumber;
                var fa = line.GetMemberValue("U_FAKey") as FTS11OFAM;
                if (fa == null) continue;
                string who = LineTag + fa.U_FACode + " - " + fa.U_FAName + ": ";

                if (def.CheckLocationQuantity)
                {
                    var loc = line.GetMemberValue("U_LocationKey") as FTS11OLOC;
                    double qty = Convert.ToDouble(line.GetMemberValue("U_Quantity"));
                    if (loc != null && Exists(
                        "SELECT T0.FAKey FROM FTS11JDT1 T0 WHERE T0.TransStatus <> 'C' AND T0.Location = " + Sql(loc.Code) +
                        " AND T0.FAKey = " + fa.DocEntry + " GROUP BY T0.FAKey, T0.Location HAVING SUM(T0.Quantity) < " + qty.ToString(CultureInfo.InvariantCulture)))
                        errors.Add(new KeyValuePair<int, string>(lineRow, who + "Insufficient Quantity in Location."));
                }

                if (Exists("SELECT TOP 1 1 FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                           "WHERE T0.TransStatus = 'P' AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' AND T0.FAKey = " + fa.DocEntry +
                           " AND T0.DocDate >= '" + new DateTime(docDate.Year, docDate.Month, 1).ToString("yyyyMMdd") + "'"))
                    errors.Add(new KeyValuePair<int, string>(lineRow, who + "Posted Depreciation found after Document Date."));

                if (docDate < fa.U_AcqDate)
                    errors.Add(new KeyValuePair<int, string>(lineRow, who + "Document Date earlier than Asset Acquisition Date."));
            }
        }
        #endregion

        #region sql helpers (all run on the import connection, inside the import transaction)
        private bool Exists(string sql)
        {
            SelectedData d = uow.ExecuteQuery(sql);
            return d.ResultSet.Length > 0 && d.ResultSet[0].Rows.Length > 0;
        }

        /// <summary>Runs a notice procedure; returns its error messages (first result set, code &lt;&gt; 0).</summary>
        private List<string> RunProc(string sql)
        {
            var errs = new List<string>();
            SelectedData d = uow.ExecuteQuery(sql);
            if (d.ResultSet.Length == 0) return errs;
            foreach (SelectStatementResultRow r in d.ResultSet[0].Rows)
            {
                if (r.Values.Length == 0 || r.Values[0] == null) continue;
                string c = Convert.ToString(r.Values[0]);
                if (c != "0" && c != "")
                    errs.Add("(-" + c + ") " + (r.Values.Length > 1 ? Convert.ToString(r.Values[1]) : ""));
            }
            return errs;
        }

        private static string Sql(string s)
        {
            return s == null ? "NULL" : "N'" + s.Replace("'", "''") + "'";
        }

        private class Ident { public string Table; public string Column; public long? Last; }

        private static Dictionary<string, Ident> ReadIdentities(SqlConnection conn)
        {
            var d = new Dictionary<string, Ident>();
            using (var cmd = new SqlCommand(
                "SELECT QUOTENAME(SCHEMA_NAME(o.schema_id)) + '.' + QUOTENAME(o.name), c.name, CONVERT(bigint, c.last_value) " +
                "FROM sys.identity_columns c JOIN sys.objects o ON o.object_id = c.object_id WHERE o.type = 'U'", conn))
            using (var rd = cmd.ExecuteReader())
                while (rd.Read())
                    d[rd.GetString(0)] = new Ident { Table = rd.GetString(0), Column = rd.GetString(1), Last = rd.IsDBNull(2) ? (long?)null : rd.GetInt64(2) };
            return d;
        }

        /// <summary>
        /// A rolled-back insert still consumes IDENTITY values; put every seed this import moved back
        /// (never below the highest committed key, so rows saved meanwhile by other users stay safe).
        /// </summary>
        private void ResetIdentities(SqlConnection conn, Dictionary<string, Ident> before)
        {
            try
            {
                foreach (Ident now in ReadIdentities(conn).Values)
                {
                    Ident was;
                    if (!before.TryGetValue(now.Table, out was) || was.Last == now.Last) continue;
                    long max;
                    using (var cmd = new SqlCommand("SELECT CONVERT(bigint, ISNULL(MAX(" + "[" + now.Column.Replace("]", "]]") + "]), 0)) FROM " + now.Table, conn))
                        max = Convert.ToInt64(cmd.ExecuteScalar());
                    long target = Math.Max(was.Last ?? 0, max);
                    using (var cmd = new SqlCommand("DBCC CHECKIDENT ('" + now.Table.Replace("'", "''") + "', RESEED, " + target + ") WITH NO_INFOMSGS", conn))
                        cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Info(0, "", "Note: document numbers used during validation could not be reset (" + Inner(ex).Message + ").");
            }
        }
        #endregion

        #region results
        private void Error(int row, string docNo, string code, string msg)
        {
            Results.Add(new ImportResultLine { Id = Results.Count + 1, Row = row, DocNo = docNo, Code = code, Status = "Error", Message = msg });
        }

        private void Info(int row, string docNo, string msg)
        {
            Results.Add(new ImportResultLine { Id = Results.Count + 1, Row = row, DocNo = docNo, Code = "", Status = "Info", Message = msg });
        }

        private static Exception Inner(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex;
        }
        #endregion
    }
}
