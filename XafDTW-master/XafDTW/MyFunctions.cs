using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FT_FixedAsset.Module.BusinessObjects;
namespace XafDTW
{
    public class MyFunctions
    {
        public static string getKeyOperatorString(PropertyInfo property)
        {
            string opcriteria = "Oid=?";
            #region your key property by type
            if (typeof(FTS11OADM) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11STRN) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11OFAC) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11OFAG) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11OLOC) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11OIND) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11OFAM) == property.PropertyType) opcriteria = "DocEntry=?";
            if (typeof(FTS11_vw_NNM1) == property.PropertyType) opcriteria = "Series=?";
            if (typeof(FTS11_vw_OACT) == property.PropertyType) opcriteria = "FormatCode=?";
            if (typeof(FTS11_vw_OACT_OCRD) == property.PropertyType) opcriteria = "FormatCode=?";
            if (typeof(FTS11_vw_OCRN) == property.PropertyType) opcriteria = "CurrCode=?";
            if (typeof(FTS11_vw_OPRC) == property.PropertyType) opcriteria = "PrcCode=?";
            if (typeof(FTS11_vw_OPRJ) == property.PropertyType) opcriteria = "PrjCode=?";
            if (typeof(SE_FAStatus) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FADeprnBy) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FASvrTyp) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FADeprnTyp) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FAProrate) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FAIndicator) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FARateYr) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FACodeBy) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FACodeDt) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FASysCalc) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FAPostTo) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FAFileType) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FASplitBy) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FAPostOpt) == property.PropertyType) opcriteria = "Code=?";
            if (typeof(SE_FARptGrpOpt) == property.PropertyType) opcriteria = "Code=?";
            #endregion

            return opcriteria;
        }

        public static void RegisterEntities()
        {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();

            #region Xaf Business Object
            XafTypesInfo.Instance.RegisterEntity(typeof(FTS11OADM));
            XafTypesInfo.Instance.RegisterEntity(typeof(FTS11OFAC));
            XafTypesInfo.Instance.RegisterEntity(typeof(FTS11OFAG));
            XafTypesInfo.Instance.RegisterEntity(typeof(FTS11OIND));
            XafTypesInfo.Instance.RegisterEntity(typeof(FTS11OLOC));
            XafTypesInfo.Instance.RegisterEntity(typeof(FTS11OFAM));
            #endregion

            XafTypesInfo.Instance.RegisterEntity(typeof(SystemUsers));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }

    }
}
