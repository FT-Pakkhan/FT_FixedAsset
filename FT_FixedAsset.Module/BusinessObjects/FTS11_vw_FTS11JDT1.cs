using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Trans. Log")]
    [DefaultProperty("DocKey")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11_vw_FTS11JDT1 : XPLiteObject
    {
        public FTS11_vw_FTS11JDT1(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region Parent Table
        private FTS11OFAM _FAKey;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11OFAM-FTS11_vw_FTS11JDT1", typeof(FTS11OFAM))]
        public FTS11OFAM FAKey
        {
            get { return _FAKey; }
            set { SetPropertyValue("FAKey", ref _FAKey, value); }
        }
        #endregion

        private string _DocKey;
        [Key]
        [XafDisplayName("DocKey"), ToolTip("DocKey")]
        [Index(10), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string DocKey
        {
            get { return _DocKey; }
            set { SetPropertyValue("DocKey", ref _DocKey, value); }
        }

        private DateTime _DocDate;
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Trans. Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set { SetPropertyValue("DocDate", ref _DocDate, value); }
        }

        private DateTime _CreateDate;
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Create. Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private string _TransStatus;
        [XafDisplayName("Status"), ToolTip("Status")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string TransStatus
        {
            get { return _TransStatus; }
            set { SetPropertyValue("TransStatus", ref _TransStatus, value); }
        }

        private string _TransType;
        [XafDisplayName("Trans. Type"), ToolTip("Trans. Type")]
        [Index(50), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string TransType
        {
            get { return _TransType; }
            set { SetPropertyValue("TransType", ref _TransType, value); }
        }

        private int _DocEntry;
        [Index(60), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("DocEntry"), ToolTip("DocEntry")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
        }

        private int _TransID;
        [Index(70), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("TransId"), ToolTip("TransId")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int TransID
        {
            get { return _TransID; }
            set { SetPropertyValue("TransID", ref _TransID, value); }
        }

        private int _LineId;
        [Index(71), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("LineId"), ToolTip("LineId")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int LineId
        {
            get { return _LineId; }
            set { SetPropertyValue("LineId", ref _LineId, value); }
        }

        private int _BaseEntry;
        [Index(80), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("BaseEntry"), ToolTip("BaseEntry")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int BaseEntry
        {
            get { return _BaseEntry; }
            set { SetPropertyValue("BaseEntry", ref _BaseEntry, value); }
        }

        private string _Name;
        [XafDisplayName("Trans. Name"), ToolTip("Trans. Name")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private double _Quantity;
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Quantity"), ToolTip("Quantity")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double Quantity
        {
            get { return _Quantity; }
            set { SetPropertyValue("Quantity", ref _Quantity, value); }
        }

        private string _Location;
        [XafDisplayName("Location"), ToolTip("Location")]
        [Index(110), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Location
        {
            get { return _Location; }
            set { SetPropertyValue("Location", ref _Location, value); }
        }

        private double _CostFC;
        [XafDisplayName("Cost (FC)"), ToolTip("Cost (FC)")]
        [Index(120), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double CostFC
        {
            get { return _CostFC; }
            set { SetPropertyValue("CostFC", ref _CostFC, value); }
        }

        private double _CostLC;
        [XafDisplayName("Cost (LC)"), ToolTip("Cost (LC)")]
        [Index(121), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double CostLC
        {
            get { return _CostLC; }
            set { SetPropertyValue("CostLC", ref _CostLC, value); }
        }

        private double _AccuLC;
        [XafDisplayName("Deprn (LC)"), ToolTip("Deprn (LC)")]
        [Index(130), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double AccuLC
        {
            get { return _AccuLC; }
            set { SetPropertyValue("AccuLC", ref _AccuLC, value); }
        }

        private double _DeprnCnt;
        [Index(140), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Deprn Cnt"), ToolTip("Deprn Cnt")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double DeprnCnt
        {
            get { return _DeprnCnt; }
            set { SetPropertyValue("DeprnCnt", ref _DeprnCnt, value); }
        }

        private double _UseLife;
        [Index(150), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Useful Life"), ToolTip("Useful Life")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double UseLife
        {
            get { return _UseLife; }
            set { SetPropertyValue("UseLife", ref _UseLife, value); }
        }

        private double _DeprPrcnt;
        [Index(151), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Percentage"), ToolTip("Percentage")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double DeprPrcnt
        {
            get { return _DeprPrcnt; }
            set { SetPropertyValue("DeprPrcnt", ref _DeprPrcnt, value); }
        }

        private double _Revalue;
        [XafDisplayName("Residual Value (LC)"), ToolTip("Residual Value (LC)")]
        [Index(160), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double Revalue
        {
            get { return _Revalue; }
            set { SetPropertyValue("Revalue", ref _Revalue, value); }
        }

        private double _RevalueSC;
        [XafDisplayName("Residual Value (SC)"), ToolTip("Residual Value (SC)")]
        [Index(161), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double RevalueSC
        {
            get { return _RevalueSC; }
            set { SetPropertyValue("RevalueSC", ref _RevalueSC, value); }
        }

    }
}

