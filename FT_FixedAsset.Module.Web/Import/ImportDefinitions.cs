using System;
using System.Collections.Generic;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Web.Import
{
    public class ImportDefinition
    {
        public Type HeaderType;
        public Type LineType;              // null = master data (one row = one record)
        public string LinesCollection;     // header collection property holding the lines
        public string Caption;             // used for the template file name
        public string DtwTemplate;         // embedded DTW template (existing layout), null = generated
        public bool CheckLocationQuantity; // Write Off / Disposal / Correction / Transfer / Reclass

        public bool IsTransaction { get { return LineType != null; } }
    }

    public static class ImportDefinitions
    {
        public const string DocNoCode = "ImportDocNo";

        private static readonly Dictionary<Type, ImportDefinition> map = new Dictionary<Type, ImportDefinition>();

        static ImportDefinitions()
        {
            Master(typeof(FTS11OFAC), "Fixed Asset Class", "FTS11OFAC.xlsx");
            Master(typeof(FTS11OFAG), "Fixed Asset Group", "FTS11OFAG.xlsx");
            Master(typeof(FTS11OIND), "Indicator", "FTS11OIND.xlsx");
            Master(typeof(FTS11OLOC), "Location", "FTS11OLOC.xlsx");
            Master(typeof(FTS11OFAM), "Fixed Asset Master", "FTS11OFAM.xlsx");

            Trans(typeof(FTS11T0201), typeof(FTS11T0201D), "Write Off", true);
            Trans(typeof(FTS11T0202), typeof(FTS11T0202D), "Disposal", true);
            Trans(typeof(FTS11T0203), typeof(FTS11T0203D), "Correction Out", true);
            Trans(typeof(FTS11T0204), typeof(FTS11T0204D), "Transferred Out", true);
            Trans(typeof(FTS11T0205), typeof(FTS11T0205D), "Reclassed Out", true);
            Trans(typeof(FTS11T0301), typeof(FTS11T0301D), "Review Useful Life", false);
            Trans(typeof(FTS11T0302), typeof(FTS11T0302D), "Revalue Cost", false);
            Trans(typeof(FTS11T0303), typeof(FTS11T0303D), "Adjust Acc. Depr", false);
            Trans(typeof(FTS11T0304), typeof(FTS11T0304D), "Move", false);
            Trans(typeof(FTS11T0305), typeof(FTS11T0305D), "Revalue Residual Value", false);
        }

        private static void Master(Type t, string caption, string dtw)
        {
            map[t] = new ImportDefinition { HeaderType = t, Caption = caption, DtwTemplate = dtw };
        }

        private static void Trans(Type h, Type l, string caption, bool qty)
        {
            map[h] = new ImportDefinition { HeaderType = h, LineType = l, LinesCollection = l.Name, Caption = caption, CheckLocationQuantity = qty };
        }

        public static ImportDefinition Find(Type t)
        {
            ImportDefinition d;
            return t != null && map.TryGetValue(t, out d) ? d : null;
        }

        public static IEnumerable<Type> SupportedTypes { get { return map.Keys; } }

        public static ImportDefinition Find(ImportObject obj)
        {
            switch (obj)
            {
                case ImportObject.FixedAssetClass: return Find(typeof(FTS11OFAC));
                case ImportObject.FixedAssetGroup: return Find(typeof(FTS11OFAG));
                case ImportObject.Indicator: return Find(typeof(FTS11OIND));
                case ImportObject.Location: return Find(typeof(FTS11OLOC));
                case ImportObject.FixedAssetMaster: return Find(typeof(FTS11OFAM));
                case ImportObject.WriteOff: return Find(typeof(FTS11T0201));
                case ImportObject.Disposal: return Find(typeof(FTS11T0202));
                case ImportObject.CorrectionOut: return Find(typeof(FTS11T0203));
                case ImportObject.TransferredOut: return Find(typeof(FTS11T0204));
                case ImportObject.ReclassedOut: return Find(typeof(FTS11T0205));
                case ImportObject.ReviewUsefulLife: return Find(typeof(FTS11T0301));
                case ImportObject.RevalueCost: return Find(typeof(FTS11T0302));
                case ImportObject.AdjustAccDepr: return Find(typeof(FTS11T0303));
                case ImportObject.Move: return Find(typeof(FTS11T0304));
                case ImportObject.RevalueResidualValue: return Find(typeof(FTS11T0305));
            }
            return null;
        }

        /// <summary>Fields maintained by the system - never taken from the file.</summary>
        public static readonly HashSet<string> SystemFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "DocEntry", "Oid", "Object", "LogInst", "IsFieldEditable", "Status", "LineStatus", "DocNum",
            "CreateDate", "UpdateDate", "U_CreateBy", "U_UpdateBy", "U_CnclBy", "U_CnclDate", "U_RvrsBy", "U_RvrsDate",
            "SQuantity", "STotalCostLC", "SDeprLC", "SNBVLC", "IsNew"
        };
    }
}
