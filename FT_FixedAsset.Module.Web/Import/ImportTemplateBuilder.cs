using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;

namespace FT_FixedAsset.Module.Web.Import
{
    public static class ImportTemplateBuilder
    {
        private class Field { public IMemberInfo Member; public string Level; }

        public enum TemplatePart { All, Header, Lines }

        public static string FileName(ImportDefinition def)
        {
            return def.HasLines ? def.Caption + " Import Template.zip" : def.Caption + " Import Template.xlsx";
        }

        /// <summary>Masters: one .xlsx. Transactions / masters with lines: a .zip with the Header file and the Lines file.</summary>
        public static byte[] Download(XafApplication app, ImportDefinition def)
        {
            if (!def.HasLines) return Build(app, def);
            using (var ms = new MemoryStream())
            {
                using (var zip = new System.IO.Compression.ZipArchive(ms, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    if (def.LinesOptional)
                    {
                        AddZipEntry(zip, def.Caption + ".xlsx", Build(app, def, TemplatePart.Header));
                        AddZipEntry(zip, def.Caption + " - " + CaptionOfType(app, def.LineType) + "s (optional).xlsx", Build(app, def, TemplatePart.Lines));
                    }
                    else
                    {
                        AddZipEntry(zip, def.Caption + " - Header.xlsx", Build(app, def, TemplatePart.Header));
                        AddZipEntry(zip, def.Caption + " - Lines.xlsx", Build(app, def, TemplatePart.Lines));
                    }
                }
                return ms.ToArray();
            }
        }

        private static void AddZipEntry(System.IO.Compression.ZipArchive zip, string name, byte[] content)
        {
            using (Stream es = zip.CreateEntry(name).Open())
                es.Write(content, 0, content.Length);
        }

        public static byte[] Build(XafApplication app, ImportDefinition def, TemplatePart part = TemplatePart.All)
        {
            using (var wb = new Workbook())
            {
                Worksheet ws;
                if (def.DtwTemplate != null && part != TemplatePart.Lines)
                {
                    // existing DTW template, sample data removed
                    using (Stream s = typeof(ImportTemplateBuilder).Assembly.GetManifestResourceStream("ImportTemplates." + def.DtwTemplate))
                        wb.LoadDocument(s, DocumentFormat.Xlsx);
                    ws = wb.Worksheets.Contains("Sheet1") ? wb.Worksheets["Sheet1"] : wb.Worksheets[0];
                    int last = ws.GetUsedRange().BottomRowIndex;
                    if (last >= 2) ws.Rows.Remove(2, last - 1);
                    for (int i = wb.Worksheets.Count - 1; i >= 0; i--)
                        if (wb.Worksheets[i] != ws && IsEmpty(wb.Worksheets[i])) wb.Worksheets.RemoveAt(i);

                    // B/F depreciation count is needed when migrating existing assets
                    if (def.HeaderType == typeof(BusinessObjects.FTS11OFAM) && Codes(ws).All(c => !c.Equals("U_BFDepr", StringComparison.OrdinalIgnoreCase)))
                    {
                        int col = ws.GetUsedRange().RightColumnIndex + 1;
                        ws.Cells[0, col].Value = "B/F Depr. Count";
                        ws.Cells[1, col].Value = "U_BFDepr";
                        ws.Cells[0, col].CopyFrom(ws.Cells[0, col - 1], PasteSpecial.Formats);
                        ws.Cells[1, col].CopyFrom(ws.Cells[1, col - 1], PasteSpecial.Formats);
                    }

                    // optional lines (Kossan: Referenced Documents): Import Doc No. links the rows of the lines file
                    if (def.LinesOptional && Codes(ws).All(c => !c.Equals(ImportDefinitions.DocNoCode, StringComparison.OrdinalIgnoreCase)))
                    {
                        int col = ws.GetUsedRange().RightColumnIndex + 1;
                        ws.Cells[0, col].Value = "Import Doc No.";
                        ws.Cells[1, col].Value = ImportDefinitions.DocNoCode;
                        ws.Cells[0, col].CopyFrom(ws.Cells[0, col - 1], PasteSpecial.Formats);
                        ws.Cells[1, col].CopyFrom(ws.Cells[1, col - 1], PasteSpecial.Formats);
                    }
                }
                else
                {
                    ws = wb.Worksheets[0];
                    ws.Name = "Sheet1";
                    var fields = TemplateFields(app, def)
                        .Where(f => part == TemplatePart.All || (part == TemplatePart.Header ? f.Level == "Header" : f.Level == "Line")).ToList();
                    ws.Cells[0, 0].Value = "Import Doc No.";
                    ws.Cells[1, 0].Value = ImportDefinitions.DocNoCode;
                    for (int i = 0; i < fields.Count; i++)
                    {
                        ws.Cells[0, i + 1].Value = Caption(app, fields[i].Member);
                        ws.Cells[1, i + 1].Value = fields[i].Member.Name;
                    }
                    int lastCol = fields.Count;
                    CellRange r1 = ws.Range.FromLTRB(0, 0, lastCol, 0);
                    r1.Font.Bold = true;
                    r1.Fill.BackgroundColor = Color.FromArgb(221, 235, 247);
                    CellRange r2 = ws.Range.FromLTRB(0, 1, lastCol, 1);
                    r2.Font.Color = Color.Gray;
                    r2.Font.Italic = true;
                    ws.Columns.AutoFit(0, lastCol);
                    ws.FreezeRows(1);
                }

                // date columns: show as dd/MM/yyyy
                var codes = Codes(ws);
                for (int c = 0; c < codes.Count; c++)
                {
                    IMemberInfo m = Member(app, def, codes[c]);
                    if (m != null && (Nullable.GetUnderlyingType(m.MemberType) ?? m.MemberType) == typeof(DateTime))
                        ws.Range.FromLTRB(c, 2, c, 1001).NumberFormat = "dd/MM/yyyy";
                }

                AddInstructions(app, def, wb, codes, part);
                wb.Worksheets.ActiveWorksheet = ws;

                using (var ms = new MemoryStream())
                {
                    wb.SaveDocument(ms, DocumentFormat.Xlsx);
                    return ms.ToArray();
                }
            }
        }

        private static bool IsEmpty(Worksheet ws)
        {
            CellRange u = ws.GetUsedRange();
            return u == null || (u.RowCount == 1 && u.ColumnCount == 1 && ws.Cells[u.TopRowIndex, u.LeftColumnIndex].Value.IsEmpty);
        }

        private static List<string> Codes(Worksheet ws)
        {
            var list = new List<string>();
            int last = ws.GetUsedRange().RightColumnIndex;
            for (int c = 0; c <= last; c++) list.Add(ws.Cells[1, c].Value.IsEmpty ? "" : ws.Cells[1, c].Value.ToString().Trim());
            return list;
        }

        /// <summary>Editable fields shown on the detail screen (header then lines), in screen order.</summary>
        private static List<Field> TemplateFields(XafApplication app, ImportDefinition def)
        {
            var list = new List<Field>();
            list.AddRange(Editable(app, def.HeaderType).Select(m => new Field { Member = m, Level = "Header" }));
            if (def.HasLines)
                list.AddRange(Editable(app, def.LineType)
                    .Where(m => m.Name != def.LineParent)   // link to the header, set by the import
                    .Select(m => new Field { Member = m, Level = "Line" }));
            return list;
        }

        private static IEnumerable<IMemberInfo> Editable(XafApplication app, Type t)
        {
            ITypeInfo ti = app.TypesInfo.FindTypeInfo(t);
            return ti.OwnMembers
                .Where(m => m.IsPublic && m.IsPersistent && !m.IsReadOnly && !m.IsList && !m.IsKey)
                .Where(m => !ImportDefinitions.SystemFields.Contains(m.Name))
                .Where(m => !(m.Name == "U_FAName" || m.Name.StartsWith("U_AccNam") || m.Name.StartsWith("U_AccCod")))   // filled from the F/A Code / account
                .Where(m =>
                {
                    var v = m.FindAttribute<VisibleInDetailViewAttribute>();
                    if (v != null && !(bool)v.Value) return false;
                    var b = m.FindAttribute<System.ComponentModel.BrowsableAttribute>();
                    if (b != null && !b.Browsable) return false;
                    // permanently disabled on screen (no criteria) = calculated field
                    if (m.FindAttributes<AppearanceAttribute>().Any(a => a.Enabled == false && string.IsNullOrEmpty(a.Criteria))) return false;
                    return true;
                })
                .OrderBy(m => { var i = m.FindAttribute<IndexAttribute>(); return i == null ? int.MaxValue : i.Index; })
                .ToList();
        }

        private static IMemberInfo Member(XafApplication app, ImportDefinition def, string code)
        {
            if (string.IsNullOrEmpty(code)) return null;
            string c = code.Contains(".") ? code.Substring(code.IndexOf('.') + 1) : code;
            foreach (Type t in new[] { def.HeaderType, def.LineType })
            {
                if (t == null) continue;
                IMemberInfo m = app.TypesInfo.FindTypeInfo(t).Members.FirstOrDefault(x => string.Equals(x.Name, c, StringComparison.OrdinalIgnoreCase));
                if (m != null) return m;
            }
            return null;
        }

        private static string CaptionOfType(XafApplication app, Type t)
        {
            var cls = t == null ? null : app.Model.BOModel.GetClass(t);
            return cls != null && !string.IsNullOrEmpty(cls.Caption) ? cls.Caption : (t == null ? "" : t.Name);
        }

        private static string Caption(XafApplication app, IMemberInfo m)
        {
            var cls = app.Model.BOModel.GetClass(m.Owner.Type);
            var mm = cls == null ? null : cls.FindMember(m.Name);
            return mm != null && !string.IsNullOrEmpty(mm.Caption) ? mm.Caption : m.Name;
        }

        private static void AddInstructions(XafApplication app, ImportDefinition def, Workbook wb, List<string> codes, TemplatePart part)
        {
            Worksheet ws = wb.Worksheets.Add("Instructions");
            var rows = new List<string[]>
            {
                new[] { def.Caption + " - import template" + (part == TemplatePart.Header ? " (HEADER file)" : part == TemplatePart.Lines ? " (LINES file)" : "") },
                new[] { "Fill data on Sheet1 from row 3. Row 1 = captions, row 2 = field codes (do not change row 2). Extra columns with unknown codes are ignored." },
                new[] { "Save as .xlsx / .xls, or as Text (Tab delimited) / CSV with the same layout. Blank cells keep the default value." },
                new[] { "The whole file is checked first; if any row has an error nothing is saved. Tick 'Run Simulation' to check without saving." },
            };
            if (def.LinesOptional && part == TemplatePart.Header)
            {
                rows.Add(new[] { "ONE row per asset. 'Import Doc No.' is only needed for assets that have " + CaptionOfType(app, def.LineType) + "s: give each such asset its own key (e.g. A1, A2)." });
                rows.Add(new[] { "Import this file alone, or together with the " + CaptionOfType(app, def.LineType) + "s file: choose this file as 'Header File' and that file as 'Lines File'." });
            }
            else if (def.LinesOptional && part == TemplatePart.Lines)
            {
                rows.Add(new[] { CaptionOfType(app, def.LineType) + "s file (optional): one row per document. 'Import Doc No.' must match the asset's 'Import Doc No.' in the asset file." });
                rows.Add(new[] { "Import it together with the asset file: choose the asset file as 'Header File' and this file as 'Lines File'." });
            }
            else if (part == TemplatePart.Header)
            {
                rows.Add(new[] { "HEADER file: ONE row per document. 'Import Doc No.' is your own key (e.g. WO1, WO2) and must be unique in this file." });
                rows.Add(new[] { "Import it together with the LINES file: on the Import Data screen choose this file as 'Header File' and the lines file as 'Lines File'." });
            }
            else if (part == TemplatePart.Lines)
            {
                rows.Add(new[] { "LINES file: one row per asset line. 'Import Doc No.' must match a row in the HEADER file - lines with the same Import Doc No. go into that document." });
                rows.Add(new[] { "Import it together with the HEADER file: on the Import Data screen choose the header file as 'Header File' and this file as 'Lines File'." });
            }
            else if (def.IsTransaction)
                rows.Add(new[] { "One row per asset line. Rows with the same 'Import Doc No.' become one document (header columns must be the same on those rows). Leave it blank for one document per row." });
            if (def.HeaderType == typeof(BusinessObjects.FTS11OFAM) && part != TemplatePart.Lines)
                rows.Add(new[] { "Leave F/A Code (U_FACode) blank to use the automatic numbering, same as on screen." });
            rows.Add(new string[0]);
            rows.Add(new[] { "Field Code", "Caption", "Level", "Type / how to fill", "Required" });

            int headerRow = rows.Count - 1;
            int[] decimals = LoadDecimals(app);   // { quantity, amount } from General Settings
            foreach (string code in codes.Where(c => c != ""))
            {
                if (code.Equals(ImportDefinitions.DocNoCode, StringComparison.OrdinalIgnoreCase))
                {
                    rows.Add(new[] { code, "Import Doc No.", "Grouping", def.LinesOptional ? "Any text/number - links the asset and its " + CaptionOfType(app, def.LineType) + "s" : "Any text/number - groups lines into one document", "" });
                    continue;
                }
                IMemberInfo m = Member(app, def, code);
                if (m == null) { rows.Add(new[] { code, "", "", "(not a field - ignored)", "" }); continue; }
                bool line = def.HasLines && m.Owner.Type == def.LineType;
                bool req = m.FindAttribute<RuleRequiredFieldAttribute>() != null;
                rows.Add(new[] { code, Caption(app, m), line ? "Line" : "Header", Describe(app, m, decimals), req ? "Yes" : "" });
            }

            for (int r = 0; r < rows.Count; r++)
                for (int c = 0; c < rows[r].Length; c++)
                    ws.Cells[r, c].Value = rows[r][c];

            ws.Cells[0, 0].Font.Bold = true;
            ws.Cells[0, 0].Font.Size = 13;
            ws.Range.FromLTRB(0, headerRow, 4, headerRow).Font.Bold = true;
            ws.Range.FromLTRB(0, headerRow, 4, headerRow).Fill.BackgroundColor = Color.FromArgb(221, 235, 247);
            ws.Columns[0].WidthInCharacters = 18;
            ws.Columns[1].WidthInCharacters = 30;
            ws.Columns[2].WidthInCharacters = 9;
            ws.Columns[3].WidthInCharacters = 70;
            ws.Columns[4].WidthInCharacters = 9;
        }

        private static int[] LoadDecimals(XafApplication app)
        {
            try
            {
                using (IObjectSpace os = app.CreateObjectSpace(typeof(BusinessObjects.FTS11OADM)))
                {
                    var o = os.FindObject<BusinessObjects.FTS11OADM>(DevExpress.Data.Filtering.CriteriaOperator.Parse("DocEntry=1"));
                    if (o != null) return new[] { o.U_QtyDec, o.U_SumDec };
                }
            }
            catch { }
            return new[] { FT_FixedAsset.Module.Controllers.DecimalPlaces.DefaultQtyDec, FT_FixedAsset.Module.Controllers.DecimalPlaces.DefaultSumDec };
        }

        private static string Describe(XafApplication app, IMemberInfo m, int[] decimals)
        {
            Type t = Nullable.GetUnderlyingType(m.MemberType) ?? m.MemberType;
            if (t == typeof(DateTime)) return "Date (dd/MM/yyyy)";
            if (t == typeof(bool)) return "TRUE / FALSE (or Y / N)";
            if (t == typeof(int) || t == typeof(long) || t == typeof(short)) return "Whole number";
            if (t == typeof(double) || t == typeof(decimal) || t == typeof(float))
            {
                var kind = FT_FixedAsset.Module.Controllers.DecimalPlaces.KindOf(m.Owner.Type, m.Name, m.MemberType);
                int places = FT_FixedAsset.Module.Controllers.DecimalPlaces.Places(kind, decimals[0], decimals[1]);
                if (kind == FT_FixedAsset.Module.Controllers.NumberKind.Quantity)
                    return places == 0 ? "Quantity - whole number (General Settings: Quantity Decimals = 0)" : "Quantity - up to " + places + " decimal place(s) (General Settings: Quantity Decimals)";
                if (kind == FT_FixedAsset.Module.Controllers.NumberKind.Amount)
                    return "Amount - rounded to " + places + " decimal place(s) (General Settings: Amount Decimals)";
                return "Number";
            }
            if (t == typeof(string)) return "Text";
            if (typeof(XPBaseObject).IsAssignableFrom(t))
            {
                ITypeInfo ti = app.TypesInfo.FindTypeInfo(t);
                var cls = app.Model.BOModel.GetClass(t);
                string by = ti.DefaultMember != null ? ti.DefaultMember.Name : "code";
                if (ti.FindMember("FormatCode") != null) by = "G/L account code";
                return "Code of " + (cls != null ? cls.Caption : t.Name) + " (" + by + ")";
            }
            return t.Name;
        }
    }
}
