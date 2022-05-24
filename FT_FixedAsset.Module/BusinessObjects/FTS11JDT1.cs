//using System;
//using System.Linq;
//using System.Text;
//using DevExpress.Xpo;
//using DevExpress.ExpressApp;
//using System.ComponentModel;
//using DevExpress.ExpressApp.DC;
//using DevExpress.Data.Filtering;
//using DevExpress.Persistent.Base;
//using System.Collections.Generic;
//using DevExpress.ExpressApp.Model;
//using DevExpress.Persistent.BaseImpl;
//using DevExpress.Persistent.Validation;
//using DevExpress.ExpressApp.ConditionalAppearance;
//using DevExpress.ExpressApp.Editors;

//namespace FT_FixedAsset.Module.BusinessObjects
//{
//    [XafDisplayName("Transaction Log")]
//    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableInlineEdit", AppearanceItemType.Action, "True", TargetItems = "InlineEdit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
//    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]

//    public class FTS11JDT1 : XPLiteObject
//    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
//        public FTS11JDT1(Session session)
//            : base(session)
//        {
//        }
//        public override void AfterConstruction()
//        {
//            base.AfterConstruction();
//            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
//        }

//        public int DocEntry { get; set; }

//        private int _LineID;
//        [Index(10), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Line ID"), ToolTip("Line ID")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int LineID
//        {
//            get { return _LineID; }
//            set { SetPropertyValue("LineID", ref _LineID, value); }
//        }

//        #region Parent Table
//        private FTS11OFAM _FAKey;
//        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
//        [Association("FTS11OFAM-FTS11JDT1", typeof(FTS11OFAM))]
//        public FTS11OFAM FAKey
//        {
//            get { return _FAKey; }
//            set { SetPropertyValue("SupplierForm", ref _FAKey, value); }
//        }
//        #endregion

//        private DateTime _DocDate;
//        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Document Date")]
//        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
//        [DbType("datetime")]
//        public DateTime DocDate
//        {
//            get { return _DocDate; }
//            set { SetPropertyValue("DocDate", ref _DocDate, value); }
//        }

//        private DateTime _CreateDate;
//        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Create Date")]
//        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
//        [DbType("datetime")]
//        public DateTime CreateDate
//        {
//            get { return _CreateDate; }
//            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
//        }

//        private string _CreateBy;
//        [XafDisplayName("Create By"), ToolTip("Create By")]
//        [Index(31), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string CreateBy
//        {
//            get { return _CreateBy; }
//            set { SetPropertyValue("CreateBy", ref _CreateBy, value); }
//        }

//        private DateTime _CancelDate;
//        [Index(40), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Cancel Date")]
//        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
//        [DbType("datetime")]
//        public DateTime CancelDate
//        {
//            get { return _CancelDate; }
//            set { SetPropertyValue("CancelDate", ref _CancelDate, value); }
//        }

//        private string _CancelBy;
//        [XafDisplayName("Cancel By"), ToolTip("Cancel By")]
//        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string CancelBy
//        {
//            get { return _CancelBy; }
//            set { SetPropertyValue("CancelBy", ref _CancelBy, value); }
//        }

//        private DateTime _PostDate;
//        [Index(50), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Post Date")]
//        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
//        [DbType("datetime")]
//        public DateTime PostDate
//        {
//            get { return _PostDate; }
//            set { SetPropertyValue("PostDate", ref _PostDate, value); }
//        }

//        private string _PostBy;
//        [XafDisplayName("Post By"), ToolTip("Post By")]
//        [Index(51), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string PostBy
//        {
//            get { return _PostBy; }
//            set { SetPropertyValue("PostBy", ref _PostBy, value); }
//        }

//        private DateTime _RvrsDate;
//        [Index(60), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Rvrs Date")]
//        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
//        [DbType("datetime")]
//        public DateTime RvrsDate
//        {
//            get { return _RvrsDate; }
//            set { SetPropertyValue("RvrsDate", ref _RvrsDate, value); }
//        }

//        private string _RvrsBy;
//        [XafDisplayName("Rvrs By"), ToolTip("Rvrs By")]
//        [Index(61), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string RvrsBy
//        {
//            get { return _RvrsBy; }
//            set { SetPropertyValue("RvrsBy", ref _RvrsBy, value); }
//        }

//        private string _PostType;
//        [XafDisplayName("Post Type"), ToolTip("Post Type")]
//        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string PostType
//        {
//            get { return _PostType; }
//            set { SetPropertyValue("Type", ref _PostType, value); }
//        }

//        private int _TransID;
//        [Index(80), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Trans ID"), ToolTip("Trans ID")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int TransID
//        {
//            get { return _TransID; }
//            set { SetPropertyValue("TransID", ref _TransID, value); }
//        }

//        private int _ReconKey;
//        [Index(90), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Recon Key"), ToolTip("Recon Key")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int ReconKey
//        {
//            get { return _ReconKey; }
//            set { SetPropertyValue("ReconKey", ref _ReconKey, value); }
//        }

//        private string _TransType;
//        [XafDisplayName("Trans Type"), ToolTip("Trans Type")]
//        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string TransType
//        {
//            get { return _TransType; }
//            set { SetPropertyValue("Type", ref _TransType, value); }
//        }

//        private string _TransStatus;
//        [XafDisplayName("TransStatus"), ToolTip("Trans Status")]
//        [Index(110), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string TransStatus
//        {
//            get { return _TransStatus; }
//            set { SetPropertyValue("TransStatus", ref _TransStatus, value); }
//        }

//        private string _BaseType;
//        [XafDisplayName("Base Type"), ToolTip("Base Type")]
//        [Index(120), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string BaseType
//        {
//            get { return _BaseType; }
//            set { SetPropertyValue("BaseType", ref _BaseType, value); }
//        }

//        private int _BaseEntry;
//        [Index(130), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("Base Entry"), ToolTip("Base Entry")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int BaseEntry
//        {
//            get { return _BaseEntry; }
//            set { SetPropertyValue("BaseEntry", ref _BaseEntry, value); }
//        }

//        private string _AcctCat;
//        [XafDisplayName("AcctCat"), ToolTip("AcctCat")]
//        [Index(140), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string AcctCat
//        {
//            get { return _AcctCat; }
//            set { SetPropertyValue("AcctCat", ref _AcctCat, value); }
//        }

//        private string _AcctFrm;
//        [XafDisplayName("AcctFrm"), ToolTip("AcctFrm")]
//        [Index(141), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string AcctFrm
//        {
//            get { return _AcctFrm; }
//            set { SetPropertyValue("AcctFrm", ref _AcctFrm, value); }
//        }

//        private string _AcctCode;
//        [XafDisplayName("AcctCode"), ToolTip("AcctCode")]
//        [Index(142), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string AcctCode
//        {
//            get { return _AcctCode; }
//            set { SetPropertyValue("AcctCode", ref _AcctCode, value); }
//        }

//        private string _Project;
//        [XafDisplayName("Project"), ToolTip("Project")]
//        [Index(150), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string Project
//        {
//            get { return _Project; }
//            set { SetPropertyValue("Project", ref _Project, value); }
//        }

//        private string _ProfitCo;
//        [XafDisplayName("ProfitCo"), ToolTip("ProfitCo")]
//        [Index(151), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string ProfitCo
//        {
//            get { return _ProfitCo; }
//            set { SetPropertyValue("ProfitCo", ref _ProfitCo, value); }
//        }

//        private string _OcrCode2;
//        [XafDisplayName("OcrCode2"), ToolTip("OcrCode2")]
//        [Index(152), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string OcrCode2
//        {
//            get { return _OcrCode2; }
//            set { SetPropertyValue("OcrCode2", ref _OcrCode2, value); }
//        }

//        private string _OcrCode3;
//        [XafDisplayName("OcrCode3"), ToolTip("OcrCode3")]
//        [Index(153), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string OcrCode3
//        {
//            get { return _OcrCode3; }
//            set { SetPropertyValue("OcrCode3", ref _OcrCode3, value); }
//        }

//        private string _OcrCode4;
//        [XafDisplayName("OcrCode4"), ToolTip("OcrCode4")]
//        [Index(154), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string OcrCode4
//        {
//            get { return _OcrCode4; }
//            set { SetPropertyValue("OcrCode4", ref _OcrCode4, value); }
//        }

//        private string _OcrCode5;
//        [XafDisplayName("OcrCode5"), ToolTip("OcrCode5")]
//        [Index(155), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string OcrCode5
//        {
//            get { return _OcrCode5; }
//            set { SetPropertyValue("OcrCode5", ref _OcrCode5, value); }
//        }

//        private double _AmtFC;
//        [Index(160), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double AmtFC
//        {
//            get { return _AmtFC; }
//            set { SetPropertyValue("AmtFC", ref _AmtFC, value); }
//        }

//        private double _AmtLC;
//        [Index(161), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double AmtLC
//        {
//            get { return _AmtLC; }
//            set { SetPropertyValue("AmtLC", ref _AmtLC, value); }
//        }

//        private double _AmtSC;
//        [Index(162), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double AmtSC
//        {
//            get { return _AmtSC; }
//            set { SetPropertyValue("AmtSC", ref _AmtSC, value); }
//        }

//        private int _DeprnCnt;
//        [Index(170), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("DeprnCnt"), ToolTip("DeprnCnt")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int DeprnCnt
//        {
//            get { return _DeprnCnt; }
//            set { SetPropertyValue("DeprnCnt", ref _DeprnCnt, value); }
//        }

//        private double _Quantity;
//        [Index(180), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double Quantity
//        {
//            get { return _Quantity; }
//            set { SetPropertyValue("Quantity", ref _Quantity, value); }
//        }

//        private int _UseLife;
//        [Index(190), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("UseLife"), ToolTip("UseLife")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int UseLife
//        {
//            get { return _UseLife; }
//            set { SetPropertyValue("UseLife", ref _UseLife, value); }
//        }

//        private double _UAmtFC;
//        [Index(200), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double UAmtFC
//        {
//            get { return _UAmtFC; }
//            set { SetPropertyValue("UAmtFC", ref _UAmtFC, value); }
//        }

//        private double _UAmtLC;
//        [Index(201), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double UAmtLC
//        {
//            get { return _UAmtLC; }
//            set { SetPropertyValue("UAmtLC", ref _UAmtLC, value); }
//        }

//        private double _UAmtSC;
//        [Index(202), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double UAmtSC
//        {
//            get { return _UAmtSC; }
//            set { SetPropertyValue("UAmtSC", ref _UAmtSC, value); }
//        }

//        private double _RAmtLC;
//        [Index(211), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double RAmtLC
//        {
//            get { return _RAmtLC; }
//            set { SetPropertyValue("RAmtLC", ref _RAmtLC, value); }
//        }

//        private double _RAmtSC;
//        [Index(212), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double RAmtSC
//        {
//            get { return _RAmtSC; }
//            set { SetPropertyValue("RAmtSC", ref _RAmtSC, value); }
//        }

//        private int _RUseLife;
//        [Index(213), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("RUseLife"), ToolTip("RUseLife")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int RUseLife
//        {
//            get { return _RUseLife; }
//            set { SetPropertyValue("RUseLife", ref _RUseLife, value); }
//        }

//        private string _Location;
//        [XafDisplayName("Location"), ToolTip("Location")]
//        [Index(220), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        public string Location
//        {
//            get { return _Location; }
//            set { SetPropertyValue("Location", ref _Location, value); }
//        }

//        private int _OJDTKey;
//        [Index(230), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
//        [XafDisplayName("OJDTKey"), ToolTip("OJDTKey")]
//        [ModelDefault("DisplayFormat", "{0:n0}")]
//        [ModelDefault("EditMask", "{0:n0}")]
//        [DbType("int")]
//        public int OJDTKey
//        {
//            get { return _OJDTKey; }
//            set { SetPropertyValue("OJDTKey", ref _OJDTKey, value); }
//        }

//        private double _Revalue;
//        [Index(241), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double Revalue
//        {
//            get { return _Revalue; }
//            set { SetPropertyValue("Revalue", ref _Revalue, value); }
//        }

//        private double _RevalueSC;
//        [Index(242), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
//        [ModelDefault("DisplayFormat", "{0:n2}")]
//        [DbType("numeric(19, 6)")]
//        public double RevalueSC
//        {
//            get { return _RevalueSC; }
//            set { SetPropertyValue("RevalueSC", ref _RevalueSC, value); }
//        }
//    }
//}