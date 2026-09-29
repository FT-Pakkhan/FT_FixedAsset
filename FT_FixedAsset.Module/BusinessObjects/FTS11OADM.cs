using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Data;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("General Settings")]
    [NavigationItem("System Initialization")]
    [ImageName("ModelEditor_Settings")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&Close", AppearanceItemType.Action, "True", TargetItems = "SaveAndClose", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11OADM : XPLiteObject
    { 
        public FTS11OADM(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            

            U_DeprBy = Session.FindObject<SE_FADeprnBy>(CriteriaOperator.Parse("Code = 'U'"));
            U_FACodeBy = Session.FindObject<SE_FACodeBy>(CriteriaOperator.Parse("Code = 'FAGrpCod'"));
            U_FACodeDt = Session.FindObject<SE_FACodeDt>(CriteriaOperator.Parse("Code = 'AcqDate'"));
            U_SysCalc = Session.FindObject<SE_FASysCalc>(CriteriaOperator.Parse("Code = 'M'"));
            U_PostTo = Session.FindObject<SE_FAPostTo>(CriteriaOperator.Parse("Code = 'SBO'"));
            U_FileType = Session.FindObject<SE_FAFileType>(CriteriaOperator.Parse("Code = 'JV'"));
            U_SplitBy = Session.FindObject<SE_FASplitBy>(CriteriaOperator.Parse("Code = 'N'"));

            U_Ocr1Capt = "Cost Centre D1";
            U_Ocr2Capt = "Cost Centre D2";
            U_Ocr3Capt = "Cost Centre D3";
            U_Ocr4Capt = "Cost Centre D4";
            U_Ocr5Capt = "Cost Centre D5";
            U_Ind1Capt = "Indicator 1";
            U_Ind2Capt = "Indicator 2";
            U_Ind3Capt = "Indicator 3";
            U_Ind4Capt = "Indicator 4";
            U_Ind5Capt = "Indicator 5";
            U_Ind6Capt = "Indicator 6";
            U_MainCur = "MYR";
            U_SysCur = "MYR";
            U_QtyDec = 0;
            U_SumDec = 2;
            U_JEMemo = "Posting From Fixed Asset System";
            Object = "FTS11OADM";

            U_DeprSalv = true;
            U_DiToBs = true;

        }

        protected override void OnSaving()
        {
            if (!(Session is NestedUnitOfWork) && (Session.DataLayer != null) && (Session.ObjectLayer is SimpleObjectLayer))
            {
            }

            if (!string.IsNullOrEmpty(U_TypeSqlPassword))
                U_SqlPWD = U_TypeSqlPassword;

            if (!string.IsNullOrEmpty(U_TypeSboPassword))
                U_SboPWD = U_TypeSboPassword;

            base.OnSaving();

        }

        #region Dummy field
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy01", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy01
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy02", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy02
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy03", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy03
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy04", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy04
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy05", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy05
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy06", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy06
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy07", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy07
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy08", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy08
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy09", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy09
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy10", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy10
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy11", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy11
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy12", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy12
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy13", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy13
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy14", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy14
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy15", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy15
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy16", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy16
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy17", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy17
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy18", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy18
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy19", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy19
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy20", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy20
        {
            get { return ""; }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private string _CompnyName;
        [XafDisplayName("Company Name"), ToolTip("Company Name")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string CompnyName
        {
            get { return _CompnyName; }
            set { SetPropertyValue("CompnyName", ref _CompnyName, value); }
        }

        private SE_FACodeBy _U_FACodeBy;
        [XafDisplayName("Generate Code By"), ToolTip("Generate Code By")]
        [Index(11), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FACodeBy U_FACodeBy
        {
            get { return _U_FACodeBy; }
            set { SetPropertyValue("U_FACodeBy", ref _U_FACodeBy, value); }
        }

        private SE_FACodeDt _U_FACodeDt;
        [XafDisplayName("Coding Date By"), ToolTip("Coding Date By")]
        [Index(12), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FACodeDt U_FACodeDt
        {
            get { return _U_FACodeDt; }
            set { SetPropertyValue("U_FACodeDt", ref _U_FACodeDt, value); }
        }

        private bool _U_DisFACode;
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Disable F/A Code")]
        public bool U_DisFACode
        {
            get { return _U_DisFACode; }
            set { SetPropertyValue("U_DisFACode", ref _U_DisFACode, value); }
        }

        private SE_FADeprnBy _U_DeprBy;
        [XafDisplayName("Deprn. Calc. By"), ToolTip("Deprn. Calc. By")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FADeprnBy U_DeprBy
        {
            get { return _U_DeprBy; }
            set { SetPropertyValue("U_DeprBy", ref _U_DeprBy, value); }
        }

        private bool _U_RndYr;
        [Index(21), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Annual Adjustment")]
        public bool U_RndYr
        {
            get { return _U_RndYr; }
            set { SetPropertyValue("U_RndYr", ref _U_RndYr, value); }
        }

        private bool _U_DeprSalv;
        [Index(22), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Depr. Consider Salvage")]
        public bool U_DeprSalv
        {
            get { return _U_DeprSalv; }
            set { SetPropertyValue("U_DeprSalv", ref _U_DeprSalv, value); }
        }

        private string _U_Ind1Capt;
        [XafDisplayName("Indicator 1"), ToolTip("Indicator 1")]
        [Index(31), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ind1Capt
        {
            get { return _U_Ind1Capt; }
            set { SetPropertyValue("U_Ind1Capt", ref _U_Ind1Capt, value); }
        }

        private string _U_Ind2Capt;
        [XafDisplayName("Indicator 2"), ToolTip("Indicator 2")]
        [Index(32), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ind2Capt
        {
            get { return _U_Ind2Capt; }
            set { SetPropertyValue("U_Ind2Capt", ref _U_Ind2Capt, value); }
        }

        private string _U_Ind3Capt;
        [XafDisplayName("Indicator 3"), ToolTip("Indicator 3")]
        [Index(33), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ind3Capt
        {
            get { return _U_Ind3Capt; }
            set { SetPropertyValue("U_Ind3Capt", ref _U_Ind3Capt, value); }
        }

        private string _U_Ind4Capt;
        [XafDisplayName("Indicator 4"), ToolTip("Indicator 4")]
        [Index(34), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ind4Capt
        {
            get { return _U_Ind4Capt; }
            set { SetPropertyValue("U_Ind4Capt", ref _U_Ind4Capt, value); }
        }

        private string _U_Ind5Capt;
        [XafDisplayName("Indicator 5"), ToolTip("Indicator 5")]
        [Index(35), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ind5Capt
        {
            get { return _U_Ind5Capt; }
            set { SetPropertyValue("U_Ind5Capt", ref _U_Ind5Capt, value); }
        }

        private string _U_Ind6Capt;
        [XafDisplayName("Indicator 6"), ToolTip("Indicator 6")]
        [Index(36), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ind6Capt
        {
            get { return _U_Ind6Capt; }
            set { SetPropertyValue("U_Ind6Capt", ref _U_Ind6Capt, value); }
        }

        private string _U_Ocr1Capt;
        [XafDisplayName("Cost Centre D1"), ToolTip("Cost Centre D1")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ocr1Capt
        {
            get { return _U_Ocr1Capt; }
            set { SetPropertyValue("U_Ocr1Capt", ref _U_Ocr1Capt, value); }
        }

        private string _U_Ocr2Capt;
        [XafDisplayName("Cost Centre D2"), ToolTip("Cost Centre D2")]
        [Index(81), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ocr2Capt
        {
            get { return _U_Ocr2Capt; }
            set { SetPropertyValue("U_Ocr2Capt", ref _U_Ocr2Capt, value); }
        }

        private string _U_Ocr3Capt;
        [XafDisplayName("Cost Centre D3"), ToolTip("Cost Centre D3")]
        [Index(82), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ocr3Capt
        {
            get { return _U_Ocr3Capt; }
            set { SetPropertyValue("U_Ocr3Capt", ref _U_Ocr3Capt, value); }
        }

        private string _U_Ocr4Capt;
        [XafDisplayName("Cost Centre D4"), ToolTip("Cost Centre D4")]
        [Index(83), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ocr4Capt
        {
            get { return _U_Ocr4Capt; }
            set { SetPropertyValue("U_Ocr4Capt", ref _U_Ocr4Capt, value); }
        }

        private string _U_Ocr5Capt;
        [XafDisplayName("Cost Centre D5"), ToolTip("Cost Centre D5")]
        [Index(84), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ocr5Capt
        {
            get { return _U_Ocr5Capt; }
            set { SetPropertyValue("U_Ocr5Capt", ref _U_Ocr5Capt, value); }
        }

        private string _U_MainCur;
        [XafDisplayName("Local Currency"), ToolTip("Local Currency")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_MainCur
        {
            get { return _U_MainCur; }
            set { SetPropertyValue("U_MainCur", ref _U_MainCur, value); }
        }

        private string _U_SysCur;
        [XafDisplayName("System Currency"), ToolTip("System Currency")]
        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_SysCur
        {
            get { return _U_SysCur; }
            set { SetPropertyValue("U_SysCur", ref _U_SysCur, value); }
        }

        private SE_FASysCalc _U_SysCalc;
        [XafDisplayName("SC Amt. Calculation"), ToolTip("SC Amt. Calculation")]
        [Index(42), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FASysCalc U_SysCalc
        {
            get { return _U_SysCalc; }
            set { SetPropertyValue("U_SysCalc", ref _U_SysCalc, value); }
        }

        private int _U_SumDec;
        [XafDisplayName("Amount Decimals"), ToolTip("Amount Decimals")]
        [Index(43), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("numeric(19,6)")]
        public int U_SumDec
        {
            get { return _U_SumDec; }
            set { SetPropertyValue("U_SumDec", ref _U_SumDec, value); }
        }

        private FTS11OLOC _U_Location;
        [XafDisplayName("Default Location"), ToolTip("Default Location")]
        [Index(44), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OLOC U_Location
        {
            get { return _U_Location; }
            set { SetPropertyValue("U_Location", ref _U_Location, value); }
        }

        private string _U_CRPath;
        [XafDisplayName("Alternate CR Path"), ToolTip("Alternate CR Path")]
        [Index(45), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_CRPath
        {
            get { return _U_CRPath; }
            set { SetPropertyValue("U_CRPath", ref _U_CRPath, value); }
        }

        private int _U_QtyDec;
        [XafDisplayName("Quantity Decimals"), ToolTip("Quantity Decimals")]
        [Index(46), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("numeric(19,6)")]
        public int U_QtyDec
        {
            get { return _U_QtyDec; }
            set { SetPropertyValue("U_QtyDec", ref _U_QtyDec, value); }
        }

        private SE_FASvrTyp _U_SvrTyp;
        [XafDisplayName("Server Type"), ToolTip("Server Type")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public SE_FASvrTyp U_SvrTyp
        {
            get { return _U_SvrTyp; }
            set { SetPropertyValue("U_SvrTyp", ref _U_SvrTyp, value); }
        }

        private string _U_SqlSvr;
        [XafDisplayName("Server Name"), ToolTip("Server Name")]
        [Index(51), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_SqlSvr
        {
            get { return _U_SqlSvr; }
            set { SetPropertyValue("U_SqlSvr", ref _U_SqlSvr, value); }
        }

        private string _U_SqlUID;
        [XafDisplayName("Login"), ToolTip("Login")]
        [Index(52), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_SqlUID
        {
            get { return _U_SqlUID; }
            set { SetPropertyValue("U_SqlUID", ref _U_SqlUID, value); }
        }

        private string _U_SqlPWD;
        [XafDisplayName("Password"), ToolTip("U_Password")]
        [Index(53)]
        [Browsable(false)]
        public string U_SqlPWD
        {
            get { return _U_SqlPWD; }
            set { SetPropertyValue("U_SqlPWD", ref _U_SqlPWD, value); }
        }

        private string _U_TypeSqlPassword;
        [XafDisplayName("Password")]
        [NonPersistent]
        [PasswordPropertyText(true)]
        [Index(54), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_TypeSqlPassword
        {
            get { return _U_TypeSqlPassword; }
            set { SetPropertyValue("U_TypeSqlPassword", ref _U_TypeSqlPassword, value); }
        }

        private string _U_LicSvr;
        [XafDisplayName("License Server"), ToolTip("License Server")]
        [Index(55), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_LicSvr
        {
            get { return _U_LicSvr; }
            set { SetPropertyValue("U_LicSvr", ref _U_LicSvr, value); }
        }

        private string _U_SboDB;
        [XafDisplayName("Database Name"), ToolTip("Database Name")]
        [Index(56), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_SboDB
        {
            get { return _U_SboDB; }
            set { SetPropertyValue("U_SboDB", ref _U_SboDB, value); }
        }

        private string _U_SboUID;
        [XafDisplayName("SAP User ID"), ToolTip("SAP User ID")]
        [Index(57), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_SboUID
        {
            get { return _U_SboUID; }
            set { SetPropertyValue("U_SboUID", ref _U_SboUID, value); }
        }

        private string _U_SboPWD;
        [XafDisplayName("SAP Password"), ToolTip("SAP Password")]
        [Index(58)]
        [Browsable(false)]
        public string U_SboPWD
        {
            get { return _U_SboPWD; }
            set { SetPropertyValue("U_SboPWD", ref _U_SboPWD, value); }
        }

        private string _U_TypeSboPassword;
        [XafDisplayName("SAP Password")]
        [NonPersistent]
        [PasswordPropertyText(true)]
        [Index(59), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_TypeSboPassword
        {
            get { return _U_TypeSboPassword; }
            set { SetPropertyValue("U_TypeSboPassword", ref _U_TypeSboPassword, value); }
        }

        private SE_FAPostTo _U_PostTo;
        [XafDisplayName("Post To"), ToolTip("Post To")]
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FAPostTo U_PostTo
        {
            get { return _U_PostTo; }
            set { SetPropertyValue("U_PostTo", ref _U_PostTo, value); }
        }

        private SE_FAFileType _U_FileType;
        [XafDisplayName("File Type"), ToolTip("File Type")]
        [Index(61), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FAFileType U_FileType
        {
            get { return _U_FileType; }
            set { SetPropertyValue("U_FileType", ref _U_FileType, value); }
        }

        private FTS11_vw_NNM1 _U_Series;
        [XafDisplayName("Journal Series"), ToolTip("Journal Series")]
        [Index(62), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public FTS11_vw_NNM1 U_Series
        {
            get { return _U_Series; }
            set { SetPropertyValue("U_Series", ref _U_Series, value); }
        }

        private string _U_JEMemo;
        [XafDisplayName("Journal Remark"), ToolTip("Journal Remark")]
        [Index(63), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DbType("nvarchar(254)")]
        public string U_JEMemo
        {
            get { return _U_JEMemo; }
            set { SetPropertyValue("U_JEMemo", ref _U_JEMemo, value); }
        }

        private string _U_FilePath;
        [XafDisplayName("Attachment Path"), ToolTip("Attachment Path")]
        [Index(64), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_FilePath
        {
            get { return _U_FilePath; }
            set { SetPropertyValue("U_FilePath", ref _U_FilePath, value); }
        }

        private SE_FASplitBy _U_SplitBy;
        [XafDisplayName("SplitBy"), ToolTip("SplitBy")]
        [Index(65), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FASplitBy U_SplitBy
        {
            get { return _U_SplitBy; }
            set { SetPropertyValue("U_SplitBy", ref _U_SplitBy, value); }
        }

        private bool _U_DiToBs;
        [Index(66), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Post Dimension to Balance Sheet")]
        public bool U_DiToBs
        {
            get { return _U_DiToBs; }
            set { SetPropertyValue("U_DiToBs", ref _U_DiToBs, value); }
        }

        private bool _U_DisposeWoQty;
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Write Off W/O Affect Qty")]
        public bool U_DisposeWoQty
        {
            get { return _U_DisposeWoQty; }
            set { SetPropertyValue("U_DisposeWoQty", ref _U_DisposeWoQty, value); }
        }

        private bool _U_DisposeCost;
        [Index(71), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Amend Write Off cost")]
        public bool U_DisposeCost
        {
            get { return _U_DisposeCost; }
            set { SetPropertyValue("U_DisposeCost", ref _U_DisposeCost, value); }
        }

        private bool _U_ApportDepr;
        [Index(72), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Apportion 1st Period Depr")]
        public bool U_ApportDepr
        {
            get { return _U_ApportDepr; }
            set { SetPropertyValue("U_ApportDepr", ref _U_ApportDepr, value); }
        }

        private bool _U_BFByNBV;
        [Index(73), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("B/F Asset Depr by B/F NBV")]
        public bool U_BFByNBV
        {
            get { return _U_BFByNBV; }
            set { SetPropertyValue("U_BFByNBV", ref _U_BFByNBV, value); }
        }

        private SE_FARateYr _U_RateYr;
        [XafDisplayName("Rounding Piority"), ToolTip("Rounding Piority")]
        [Index(67), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FARateYr U_RateYr
        {
            get { return _U_RateYr; }
            set { SetPropertyValue("U_RateYr", ref _U_RateYr, value); }
        }

        private string _U_Ref1;
        [XafDisplayName("Ref. 1"), ToolTip("Ref. 1")]
        [Index(68), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DbType("nvarchar(100)")]
        public string U_Ref1
        {
            get { return _U_Ref1; }
            set { SetPropertyValue("U_Ref1", ref _U_Ref1, value); }
        }

        private string _U_Ref2;
        [XafDisplayName("Ref. 2"), ToolTip("Ref. 2")]
        [Index(69), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DbType("nvarchar(100)")]
        public string U_Ref2
        {
            get { return _U_Ref2; }
            set { SetPropertyValue("U_Ref2", ref _U_Ref2, value); }
        }

        private string _Object;
        [XafDisplayName("Object"), ToolTip("Object")]
        [Index(999), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }
    }
}