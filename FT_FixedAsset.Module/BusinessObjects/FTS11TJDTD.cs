using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Transaction Details")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11TJDTD : XPLiteObject
    { 
        public FTS11TJDTD(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
        }

        #region Parent Table
        private FTS11TJDT _DocEntry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FTS11TJDT-FTS11TJDTD", typeof(FTS11TJDT))]
        public FTS11TJDT DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
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

        private int _LineID;
        [Index(20), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Line ID"), ToolTip("Line ID")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public int LineID
        {
            get { return _LineID; }
            set { SetPropertyValue("LineID", ref _LineID, value); }
        }

        private string _AcctFrm;
        [XafDisplayName("Account"), ToolTip("Account")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string AcctFrm
        {
            get { return _AcctFrm; }
            set { SetPropertyValue("AcctFrm", ref _AcctFrm, value); }
        }

        private double _DebitLC;
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Debit (LC)"), ToolTip("Debit (LC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double DebitLC
        {
            get { return _DebitLC; }
            set { SetPropertyValue("DebitLC", ref _DebitLC, value); }
        }

        private double _CreditLC;
        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Credit (LC)"), ToolTip("Credit (LC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double CreditLC
        {
            get { return _CreditLC; }
            set { SetPropertyValue("CreditLC", ref _CreditLC, value); }
        }

        private double _DebitFC;
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Debit (FC)"), ToolTip("Debit (FC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double DebitFC
        {
            get { return _DebitFC; }
            set { SetPropertyValue("DebitFC", ref _DebitFC, value); }
        }

        private double _CreditFC;
        [Index(51), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Credit (FC)"), ToolTip("Credit (FC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double CreditFC
        {
            get { return _CreditFC; }
            set { SetPropertyValue("CreditFC", ref _CreditFC, value); }
        }

        private double _DebitSC;
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Debit (SC)"), ToolTip("Debit (SC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double DebitSC
        {
            get { return _DebitSC; }
            set { SetPropertyValue("DebitSC", ref _DebitSC, value); }
        }

        private double _CreditSC;
        [Index(61), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Credit (SC)"), ToolTip("Credit (SC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double CreditSC
        {
            get { return _CreditSC; }
            set { SetPropertyValue("CreditSC", ref _CreditSC, value); }
        }

        private string _Project;
        [XafDisplayName("Project"), ToolTip("Project")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string Project
        {
            get { return _Project; }
            set { SetPropertyValue("Project", ref _Project, value); }
        }

        private string _ProfitCo;
        [XafDisplayName("D1"), ToolTip("Cost Center 1")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string ProfitCo
        {
            get { return _ProfitCo; }
            set { SetPropertyValue("ProfitCo", ref _ProfitCo, value); }
        }

        private string _OcrCode2;
        [XafDisplayName("D2"), ToolTip("Cost Center 2")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string OcrCode2
        {
            get { return _OcrCode2; }
            set { SetPropertyValue("OcrCode2", ref _OcrCode2, value); }
        }

        private string _OcrCode3;
        [XafDisplayName("D3"), ToolTip("Cost Center 3")]
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string OcrCode3
        {
            get { return _OcrCode3; }
            set { SetPropertyValue("OcrCode3", ref _OcrCode3, value); }
        }

        private string _OcrCode4;
        [XafDisplayName("D4"), ToolTip("Cost Center 4")]
        [Index(110), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string OcrCode4
        {
            get { return _OcrCode4; }
            set { SetPropertyValue("OcrCode4", ref _OcrCode4, value); }
        }

        private string _OcrCode5;
        [XafDisplayName("D5"), ToolTip("Cost Center 5")]
        [Index(120), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string OcrCode5
        {
            get { return _OcrCode5; }
            set { SetPropertyValue("OcrCode5", ref _OcrCode5, value); }
        }
    }
}