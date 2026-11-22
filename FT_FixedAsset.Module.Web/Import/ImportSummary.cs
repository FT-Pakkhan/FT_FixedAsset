using System;
using System.Collections.Generic;
using System.Linq;

namespace FT_FixedAsset.Module.Web.Import
{
    /// <summary>
    /// Result of one Import / Run Simulation, kept in the user's session and shown by ImportResult.aspx
    /// (same window style as the Run Depreciation summary).
    /// </summary>
    [Serializable]
    public class ImportSummary
    {
        public const string SessionKey = "FT_ImportSummary";

        public string Caption;        // e.g. "Location"
        public bool Simulation;
        public bool Success;
        public bool Committed;
        public string FileName;
        public string LinesFileName;
        public string StartBy;
        public DateTime Started;
        public double Seconds;
        public int Records;           // records valid (simulation) / imported
        public int Errors;            // error lines
        public string Note;           // info messages from the engine
        public List<string[]> Lines = new List<string[]>();   // Row, Import Doc No., Code, Status, Message

        public string Title
        {
            get
            {
                if (Simulation) return Success ? "Simulation Passed" : "Simulation Failed";
                return Committed ? "Import Completed" : "Import Failed";
            }
        }

        public static ImportSummary From(ImportEngine engine, ImportDefinition def, bool simulation,
                                         string fileName, string linesFileName, string startBy, DateTime started, double seconds)
        {
            var s = new ImportSummary
            {
                Caption = def.Caption,
                Simulation = simulation,
                Success = engine.Success,
                Committed = engine.Committed,
                FileName = fileName,
                LinesFileName = linesFileName,
                StartBy = startBy,
                Started = started,
                Seconds = seconds,
                Errors = engine.Results.Count(r => r.Status == "Error"),
                Records = engine.Results.Count(r => r.Status == "OK" || r.Status == "Valid"),
                Note = string.Join(" ", engine.Results.Where(r => r.Status == "Info").Select(r => r.Message))
            };
            foreach (ImportResultLine r in engine.Results.Where(r => r.Status != "Info").OrderBy(r => r.Row).ThenBy(r => r.Id))
                s.Lines.Add(new[] { r.Row > 0 ? r.Row.ToString() : "", r.DocNo ?? "", r.Code ?? "", r.Status ?? "", r.Message ?? "" });
            return s;
        }
    }
}
