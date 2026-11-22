using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Spreadsheet;

namespace FT_FixedAsset.Module.Web.Import
{
    public class ImportRow
    {
        public int RowNumber;      // 1-based row number as the user sees it in the file (lines file when two files are used)
        public int HeaderRowNumber;// row in the header file (two-file transaction import), 0 otherwise
        public object[] Values;    // string, double, DateTime or bool (null = empty)
    }

    public class ImportTable
    {
        public List<string> Codes = new List<string>();
        public List<string> Captions = new List<string>();
        public List<ImportRow> Rows = new List<ImportRow>();

        public int IndexOf(string code)
        {
            return Codes.FindIndex(c => string.Equals(c, code, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Reads the DTW layout: row 1 = captions, row 2 = field codes, data from row 3.
    /// If row 1 already holds the field codes, data starts on row 2.
    /// </summary>
    public static class ImportFileReader
    {
        public static ImportTable Read(string fileName, byte[] content, Func<string, bool> isKnownCode)
        {
            if (content == null || content.Length == 0)
                throw new InvalidOperationException("The file is empty.");

            string ext = (Path.GetExtension(fileName) ?? "").ToLowerInvariant();
            List<ImportRow> raw;
            switch (ext)
            {
                case ".xlsx":
                case ".xlsm":
                case ".xls":
                    raw = ReadExcel(content, ext);
                    break;
                case ".txt":
                case ".csv":
                    raw = ReadText(content);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported file type '" + ext + "'. Use .xlsx, .xls, .txt or .csv.");
            }

            return BuildTable(raw, isKnownCode);
        }

        private static ImportTable BuildTable(List<ImportRow> raw, Func<string, bool> isKnownCode)
        {
            Func<ImportRow, int> score = r => r == null ? 0 : r.Values.Count(v => v != null && isKnownCode(Convert.ToString(v).Trim()));
            ImportRow row1 = raw.FirstOrDefault(r => r.RowNumber == 1);
            ImportRow row2 = raw.FirstOrDefault(r => r.RowNumber == 2);

            ImportRow codeRow, captionRow = null;
            if (score(row2) > 0 && score(row2) >= score(row1)) { codeRow = row2; captionRow = row1; }
            else if (score(row1) > 0) { codeRow = row1; }
            else throw new InvalidOperationException("No field codes found. Row 2 of the file must contain the field codes (e.g. U_FACode, U_FAGrpKey) - download the Import Template for the correct layout.");

            var table = new ImportTable();
            for (int c = 0; c < codeRow.Values.Length; c++)
            {
                table.Codes.Add(Convert.ToString(codeRow.Values[c] ?? "").Trim());
                table.Captions.Add(captionRow != null && c < captionRow.Values.Length ? Convert.ToString(captionRow.Values[c] ?? "").Trim() : "");
            }

            foreach (ImportRow r in raw.Where(r => r.RowNumber > codeRow.RowNumber))
            {
                if (r.Values.All(v => v == null || (v is string && ((string)v).Trim() == ""))) continue;
                var vals = new object[table.Codes.Count];
                Array.Copy(r.Values, vals, Math.Min(r.Values.Length, vals.Length));
                table.Rows.Add(new ImportRow { RowNumber = r.RowNumber, Values = vals });
            }

            if (table.Rows.Count == 0)
                throw new InvalidOperationException("The file has no data rows (data must start below the field-code row).");
            return table;
        }

        private static List<ImportRow> ReadExcel(byte[] content, string ext)
        {
            var rows = new List<ImportRow>();
            using (var wb = new Workbook())
            using (var ms = new MemoryStream(content))
            {
                wb.LoadDocument(ms, ext == ".xls" ? DocumentFormat.Xls : (ext == ".xlsm" ? DocumentFormat.Xlsm : DocumentFormat.Xlsx));
                Worksheet ws = wb.Worksheets.Contains("Sheet1") ? wb.Worksheets["Sheet1"] : wb.Worksheets[0];
                CellRange used = ws.GetUsedRange();
                int lastRow = used.BottomRowIndex, lastCol = used.RightColumnIndex;

                for (int r = 0; r <= lastRow; r++)
                {
                    var vals = new object[lastCol + 1];
                    for (int c = 0; c <= lastCol; c++)
                    {
                        CellValue v = ws.Cells[r, c].Value;
                        if (v == null || v.IsEmpty) continue;
                        if (v.IsDateTime) vals[c] = v.DateTimeValue;
                        else if (v.IsBoolean) vals[c] = v.BooleanValue;
                        else if (v.IsNumeric) vals[c] = v.NumericValue;
                        else if (v.IsText) vals[c] = v.TextValue;
                        else if (v.IsError) vals[c] = "#ERROR: " + v.ErrorValue.Name;
                        else vals[c] = v.ToString();
                    }
                    rows.Add(new ImportRow { RowNumber = r + 1, Values = vals });
                }
            }
            return rows;
        }

        private static List<ImportRow> ReadText(byte[] content)
        {
            string text = DecodeText(content);
            var lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            // delimiter: tab, else ';' or ',' whichever is more frequent in the first lines
            string sample = string.Join("\n", lines.Take(3));
            char delim = sample.Contains('\t') ? '\t'
                       : (sample.Count(ch => ch == ';') > sample.Count(ch => ch == ',') ? ';' : ',');

            var rows = new List<ImportRow>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == lines.Length - 1 && lines[i] == "") break;
                List<string> cells = SplitLine(lines, ref i, delim, out int startLine);
                rows.Add(new ImportRow
                {
                    RowNumber = startLine + 1,
                    Values = cells.Select(c => string.IsNullOrWhiteSpace(c) ? null : (object)c).ToArray()
                });
            }
            return rows;
        }

        /// <summary>Splits one record, honouring "quoted" fields (which may contain the delimiter or line breaks).</summary>
        private static List<string> SplitLine(string[] lines, ref int i, char delim, out int startLine)
        {
            startLine = i;
            var cells = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            string line = lines[i];
            int p = 0;
            while (true)
            {
                if (p >= line.Length)
                {
                    if (inQuotes && i + 1 < lines.Length) { sb.Append('\n'); i++; line = lines[i]; p = 0; continue; }
                    break;
                }
                char ch = line[p];
                if (inQuotes)
                {
                    if (ch == '"' && p + 1 < line.Length && line[p + 1] == '"') { sb.Append('"'); p += 2; continue; }
                    if (ch == '"') { inQuotes = false; p++; continue; }
                    sb.Append(ch); p++;
                }
                else
                {
                    if (ch == '"' && sb.Length == 0) { inQuotes = true; p++; continue; }
                    if (ch == delim) { cells.Add(sb.ToString()); sb.Clear(); p++; continue; }
                    sb.Append(ch); p++;
                }
            }
            cells.Add(sb.ToString());
            return cells;
        }

        private static string DecodeText(byte[] content)
        {
            // BOM (UTF-8 / UTF-16 "Unicode Text" from Excel)
            if (content.Length >= 2 && content[0] == 0xFF && content[1] == 0xFE) return Encoding.Unicode.GetString(content, 2, content.Length - 2);
            if (content.Length >= 2 && content[0] == 0xFE && content[1] == 0xFF) return Encoding.BigEndianUnicode.GetString(content, 2, content.Length - 2);
            if (content.Length >= 3 && content[0] == 0xEF && content[1] == 0xBB && content[2] == 0xBF) return Encoding.UTF8.GetString(content, 3, content.Length - 3);
            try
            {
                return new UTF8Encoding(false, true).GetString(content);   // strict UTF-8
            }
            catch (DecoderFallbackException)
            {
                return Encoding.Default.GetString(content);                 // ANSI "Text (Tab delimited)"
            }
        }
    }
}
