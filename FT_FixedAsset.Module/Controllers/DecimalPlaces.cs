using FT_FixedAsset.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace FT_FixedAsset.Module.Controllers
{
    public enum NumberKind
    {
        /// <summary>Keeps the format defined on the field itself (exchange rate, percentage, count, settings...).</summary>
        Own,
        /// <summary>General Settings - Quantity Decimals.</summary>
        Quantity,
        /// <summary>General Settings - Amount Decimals.</summary>
        Amount,
        /// <summary>Always 2 decimals (useful life / B/F depreciation count).</summary>
        Fixed2
    }

    /// <summary>
    /// One rule for which number fields follow "Quantity Decimals" / "Amount Decimals" (General Settings).
    /// Used by DynamicDecimalController (detail + list views) and by Data Import.
    /// </summary>
    public static class DecimalPlaces
    {
        private static readonly HashSet<string> QuantityFields = new HashSet<string>(StringComparer.Ordinal)
        {
            "U_Quantity", "SQuantity", "QtyFrom", "QtyTo", "Quantity"
        };

        private static readonly HashSet<string> Fixed2Fields = new HashSet<string>(StringComparer.Ordinal)
        {
            "U_UseLife", "U_BFDepr"
        };

        // not quantities or amounts - keep their own format
        private static readonly HashSet<string> OwnFormatFields = new HashSet<string>(StringComparer.Ordinal)
        {
            "U_ExSC", "U_ExFC",                              // exchange rates (n6)
            "U_DeprnCnt", "DeprnCnt",                        // depreciation count
            "U_DeprPrcnt", "U_DeprPrcntN", "DeprPrcnt",      // percentages
            "U_UseLifeN", "UseLife"                          // useful life (months)
        };

        // screens whose numbers are settings, not amounts
        private static readonly HashSet<Type> OwnFormatTypes = new HashSet<Type>
        {
            typeof(FTS11OADM), typeof(FTS11OPPA)
        };

        public static NumberKind KindOf(Type objectType, string memberName, Type memberType)
        {
            Type t = Nullable.GetUnderlyingType(memberType) ?? memberType;
            if (t != typeof(double) && t != typeof(decimal) && t != typeof(float)) return NumberKind.Own;
            if (objectType != null && OwnFormatTypes.Contains(objectType)) return NumberKind.Own;
            if (QuantityFields.Contains(memberName)) return NumberKind.Quantity;
            if (Fixed2Fields.Contains(memberName)) return NumberKind.Fixed2;
            if (OwnFormatFields.Contains(memberName)) return NumberKind.Own;
            return NumberKind.Amount;
        }

        public const int DefaultQtyDec = 0, DefaultSumDec = 2;   // FTS11OADM defaults

        /// <summary>Decimal places for the kind, or -1 for NumberKind.Own.</summary>
        public static int Places(NumberKind kind, int qtyDec, int sumDec)
        {
            switch (kind)
            {
                case NumberKind.Quantity: return Math.Max(0, qtyDec);
                case NumberKind.Amount: return Math.Max(0, sumDec);
                case NumberKind.Fixed2: return 2;
                default: return -1;
            }
        }

        public static int Places(NumberKind kind, FTS11OADM oadm)
        {
            return oadm == null ? Places(kind, DefaultQtyDec, DefaultSumDec) : Places(kind, oadm.U_QtyDec, oadm.U_SumDec);
        }

        /// <summary>
        /// Import / API values: amounts are rounded to Amount Decimals (as the screen would show and store them);
        /// a quantity with more decimals than Quantity Decimals is an error (the screen would not accept it either).
        /// Returns the value to store, or null with an error message.
        /// </summary>
        public static double? Apply(NumberKind kind, int places, double value, out string error)
        {
            error = null;
            if (kind == NumberKind.Own || places < 0) return value;
            double rounded = Math.Round(value, places, MidpointRounding.AwayFromZero);
            bool extraDecimals = Math.Abs(value - rounded) > 1e-9 * Math.Max(1.0, Math.Abs(value));   // ignore Excel floating noise
            if (kind == NumberKind.Quantity && extraDecimals)
            {
                error = "'" + value.ToString("0.##########", System.Globalization.CultureInfo.InvariantCulture) + "' has too many decimals - " +
                        (places == 0 ? "quantity must be a whole number" : "quantity allows " + places + " decimal place(s)") +
                        " (General Settings - Quantity Decimals).";
                return null;
            }
            return rounded;
        }

        public static string Format(int places)
        {
            return "{0:n" + places + "}";
        }
    }
}
