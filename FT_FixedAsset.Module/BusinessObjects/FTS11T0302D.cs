using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Revalue Cost Details")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code != 'O'", TargetItems = "SwitchToEditMode; Edit; InlineEdit", Enabled = false, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code = 'P'", TargetItems = "Delete", Enabled = false, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]


    public class FTS11T0302D : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11T0302D(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            LineStatus = Session.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'O'"));
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int Oid { get; set; }

        #region Parent Table
        private FTS11T0302 _DocEntry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FTS11T0302-FTS11T0302D", typeof(FTS11T0302))]
        public FTS11T0302 DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
        }
        #endregion

        private FTS11OFAM _U_FAKey;
        [DataSourceCriteria("SQuantity > 0")]
        [XafDisplayName("F/A Code"), ToolTip("F/A Code")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OFAM U_FAKey
        {
            get { return _U_FAKey; }
            set {
                SetPropertyValue("U_FAKey", ref _U_FAKey, value);

                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, value.U_FACode);
                        SetPropertyValue("U_FAName", ref _U_FAName, value.U_FAName);
                        SetPropertyValue("U_UseLifeN", ref _U_UseLifeN, value.U_UseLife);
                        SetPropertyValue("U_TCostLCO", ref _U_TCostLCO, value.U_TCostLC);
                        SetPropertyValue("U_TCostSCO", ref _U_TCostSCO, value.U_TCostSC);
                        SetPropertyValue("U_TCostSCO", ref _U_TCostFCO, value.U_TCostFC);
                    }
                    else
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, "");
                        SetPropertyValue("U_FAName", ref _U_FAName, "");
                        SetPropertyValue("U_UseLifeN", ref _U_UseLifeN, 0);
                        SetPropertyValue("U_TCostLCO", ref _U_TCostLCO, 0);
                        SetPropertyValue("U_TCostSCO", ref _U_TCostSCO, 0);
                        SetPropertyValue("U_TCostSCO", ref _U_TCostFCO, 0);
                    }
                }
            }
        }

        private string _U_FACode;
        [XafDisplayName("FACode"), ToolTip("FACode")]
        [Index(11), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_FACode
        {
            get { return _U_FACode; }
            set { SetPropertyValue("U_FACode", ref _U_FACode, value); }
        }

        private string _U_FAName;
        [XafDisplayName("F/A Name"), ToolTip("F/A Name")]
        [Index(12), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string U_FAName
        {
            get { return _U_FAName; }
            set { SetPropertyValue("U_FAName", ref _U_FAName, value); }
        }

        private double _U_UseLifeN;
        [Index(20), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("New Useful Life"), ToolTip("New Useful Life")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("numeric(19,6)")]
        public double U_UseLifeN
        {
            get { return _U_UseLifeN; }
            set { SetPropertyValue("U_UseLifeN", ref _U_UseLifeN, value); }
        }

        private double _U_TCostLCO;
        [XafDisplayName("Before Revalue (LC)"), ToolTip("Before Revalue (LC)")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [Appearance("TCostLCO", Enabled = false)]
        public double U_TCostLCO
        {
            get { return _U_TCostLCO; }
            set { SetPropertyValue("U_TCostLCO", ref _U_TCostLCO, value); }
        }

        private double _U_TCostLCD;
        [XafDisplayName("Adjustment (LC)"), ToolTip("Adjustment (LC)")]
        [Index(31), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [ImmediatePostData]
        public double U_TCostLCD
        {
            get { return _U_TCostLCD; }
            set
            {
                SetPropertyValue("U_TCostLCD", ref _U_TCostLCD, value);
                if(!IsLoading)
                {
                    U_TCostLCN = U_TCostLCO + U_TCostLCD;
                    U_TCostSCD = U_TCostLCD;
                }
            }
        }

        private double _U_TCostLCN;
        [XafDisplayName("After Revalue (LC)"), ToolTip("After Revalue (LC)")]
        [Index(32), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [Appearance("TCostLCN", Enabled = false)]
        public double U_TCostLCN
        {
            get { return _U_TCostLCN; }
            set { SetPropertyValue("U_TCostLCN", ref _U_TCostLCN, value); }
        }

        private double _U_CostLCD;
        [XafDisplayName("Adj/Unit (LC)"), ToolTip("Adj/Unit (LC)")]
        [Index(33), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_CostLCD
        {
            get { return _U_CostLCD; }
            set { SetPropertyValue("U_CostLCD", ref _U_CostLCD, value); }
        }

        private double _U_CostSCD;
        [XafDisplayName("Adj/Unit (SC)"), ToolTip("Adj/Unit (SC)")]
        [Index(50), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_CostSCD
        {
            get { return _U_CostSCD; }
            set { SetPropertyValue("U_CostSCD", ref _U_CostSCD, value); }
        }

        private double _U_TCostSCO;
        [XafDisplayName("Before Revalue (SC)"), ToolTip("Before Revalue (SC)")]
        [Index(51), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [Appearance("TCostSCO", Enabled = false)]
        [ImmediatePostData]
        public double U_TCostSCO
        {
            get { return _U_TCostSCO; }
            set
            {
                SetPropertyValue("U_TCostSCO", ref _U_TCostSCO, value);
                if (!IsLoading)
                {
                    U_TCostSCN = U_TCostSCO + U_TCostSCD;
                }
            }
        }

        private double _U_TCostSCD;
        [XafDisplayName("Adjustment (SC)"), ToolTip("Adjustment (SC)")]
        [Index(52), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_TCostSCD
        {
            get { return _U_TCostSCD; }
            set { SetPropertyValue("U_TCostSCD", ref _U_TCostSCD, value); }
        }

        private double _U_TCostSCN;
        [XafDisplayName("After Revalue (SC)"), ToolTip("After Revalue (SC)")]
        [Index(53), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [Appearance("TCostSCN", Enabled = false)]
        public double U_TCostSCN
        {
            get { return _U_TCostSCN; }
            set { SetPropertyValue("U_TCostSCN", ref _U_TCostSCN, value); }
        }

        private double _U_TCostFCO;
        [XafDisplayName("Before Revalue (FC)"), ToolTip("Before Revalue (FC)")]
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [Appearance("TCostFCO", Enabled = false)]
        public double U_TCostFCO
        {
            get { return _U_TCostFCO; }
            set { SetPropertyValue("U_TCostFCO", ref _U_TCostFCO, value); }
        }

        private double _U_TCostFCD;
        [XafDisplayName("Adjustment (FC)"), ToolTip("Adjustment (FC)")]
        [Index(61), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [ImmediatePostData]
        public double U_TCostFCD
        {
            get { return _U_TCostFCD; }
            set
            {
                SetPropertyValue("U_TCostFCD", ref _U_TCostFCD, value);
                if (!IsLoading)
                {
                    U_TCostFCN = U_TCostFCO + U_TCostFCD;
                }
            }
        }

        private double _U_TCostFCN;
        [XafDisplayName("After Revalue (FC)"), ToolTip("After Revalue (FC)")]
        [Index(62), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        [Appearance("TCostFCN", Enabled = false)]
        public double U_TCostFCN
        {
            get { return _U_TCostFCN; }
            set { SetPropertyValue("U_TCostFCN", ref _U_TCostFCN, value); }
        }

        private double _U_CostFCD;
        [XafDisplayName("Adj/Unit (FC)"), ToolTip("Adj/Unit (FC)")]
        [Index(63), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_CostFCD
        {
            get { return _U_CostFCD; }
            set { SetPropertyValue("U_CostFCD", ref _U_CostFCD, value); }
        }

        private string _U_AccCodG;
        [XafDisplayName("Revalue Surplus A/C Code"), ToolTip("Revalue Surplus A/C Code")]
        [Index(70), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodG
        {
            get { return _U_AccCodG; }
            set { SetPropertyValue("U_AccCodG", ref _U_AccCodG, value); }
        }

        private FTS11_vw_OACT _U_AccFrmG;
        [XafDisplayName("Revalue Surplus A/C"), ToolTip("Revalue Surplus A/C")]
        [Index(71), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmG
        {
            get { return _U_AccFrmG; }
            set
            {
                SetPropertyValue("U_AccFrmG", ref _U_AccFrmG, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodG", ref _U_AccCodG, value.AcctCode);
                        SetPropertyValue("U_AccNamG", ref _U_AccNamG, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodG", ref _U_AccCodG, "");
                        SetPropertyValue("U_AccNamG", ref _U_AccNamG, "");
                    }
                }
            }
        }

        private string _U_AccNamG;
        [XafDisplayName("Revalue Surplus A/C Name"), ToolTip("Revalue Surplus A/C Name")]
        [Index(72), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamG
        {
            get { return _U_AccNamG; }
            set { SetPropertyValue("U_AccNamG", ref _U_AccNamG, value); }
        }

        private string _U_AccCodL;
        [XafDisplayName("Impairement Loss A/C Code"), ToolTip("Impairement Loss A/C Code")]
        [Index(80), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodL
        {
            get { return _U_AccCodL; }
            set { SetPropertyValue("U_AccCodL", ref _U_AccCodL, value); }
        }

        private FTS11_vw_OACT _U_AccFrmL;
        [XafDisplayName("Impairement Loss A/C"), ToolTip("Impairement Loss A/C")]
        [Index(81), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmL
        {
            get { return _U_AccFrmL; }
            set
            {
                SetPropertyValue("U_AccFrmL", ref _U_AccFrmL, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodL", ref _U_AccCodL, value.AcctCode);
                        SetPropertyValue("U_AccNamL", ref _U_AccNamL, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodL", ref _U_AccCodL, "");
                        SetPropertyValue("U_AccNamL", ref _U_AccNamL, "");
                    }
                }
            }
        }

        private string _U_AccNamL;
        [XafDisplayName("Impairement Loss A/C Name"), ToolTip("Impairemet Loss A/C Name")]
        [Index(82), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamL
        {
            get { return _U_AccNamL; }
            set { SetPropertyValue("U_AccNamL", ref _U_AccNamL, value); }
        }

        private string _U_AccCodC;
        [XafDisplayName("Accu. Impair Loss A/C Code"), ToolTip("Accu. Impair Loss A/C Code")]
        [Index(90), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodC
        {
            get { return _U_AccCodC; }
            set { SetPropertyValue("U_AccCodC", ref _U_AccCodC, value); }
        }

        private FTS11_vw_OACT _U_AccFrmC;
        [XafDisplayName("Accu. Impair Loss A/C"), ToolTip("Accu. Impair Loss A/C")]
        [Index(91), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmC
        {
            get { return _U_AccFrmC; }
            set
            {
                SetPropertyValue("U_AccFrmC", ref _U_AccFrmC, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodC", ref _U_AccCodC, value.AcctCode);
                        SetPropertyValue("U_AccNamC", ref _U_AccNamC, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodC", ref _U_AccCodC, "");
                        SetPropertyValue("U_AccNamC", ref _U_AccNamC, "");
                    }
                }
            }
        }

        private string _U_AccNamC;
        [XafDisplayName("Accu. Impair Loss A/C Name"), ToolTip("Accu. Impair Loss A/C Name")]
        [Index(92), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamC
        {
            get { return _U_AccNamC; }
            set { SetPropertyValue("U_AccNamC", ref _U_AccNamC, value); }
        }

        private SE_FAStatus _LineStatus;
        [XafDisplayName("Line Status"), ToolTip("Line Status")]
        [NoForeignKey]
        [Index(999), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("LineStatus", Enabled = false)]
        public SE_FAStatus LineStatus
        {
            get { return _LineStatus; }
            set { SetPropertyValue("LineStatus", ref _LineStatus, value); }
        }

        private int _LogInst;
        [Index(1000), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("LogInst"), ToolTip("LogInst")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public int LogInst
        {
            get { return _LogInst; }
            set { SetPropertyValue("LogInst", ref _LogInst, value); }
        }
    }
}