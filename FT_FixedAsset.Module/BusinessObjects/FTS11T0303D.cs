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
    [XafDisplayName("Adjust Acc. Depr Details")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code != 'O'", TargetItems = "SwitchToEditMode; Edit; InlineEdit", Enabled = false, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code = 'P'", TargetItems = "Delete", Enabled = false, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11T0303D : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11T0303D(Session session)
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
        private FTS11T0303 _DocEntry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FTS11T0303-FTS11T0303D", typeof(FTS11T0303))]
        public FTS11T0303 DocEntry
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
                    }
                    else
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, "");
                        SetPropertyValue("U_FAName", ref _U_FAName, "");
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

        private double _U_AmtLC;
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Accu. Depr Adj (LC)"), ToolTip("Accu. Depr Adj (LC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ImmediatePostData]
        [DbType("numeric(19, 6)")]
        public double U_AmtLC
        {
            get { return _U_AmtLC; }
            set
            {
                SetPropertyValue("U_AmtLC", ref _U_AmtLC, value);
                if (!IsLoading)
                {
                    if (value > 0)
                        SetPropertyValue("U_AmtSC", ref _U_AmtSC, value);
                    else
                        SetPropertyValue("U_AmtSC", ref _U_AmtSC, 0);
                }
            }
        }

        private double _U_AmtSC;
        [Index(21), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [XafDisplayName("Accu. Depr Adj (SC)"), ToolTip("Accu. Depr Adj (SC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19, 6)")]
        public double U_AmtSC
        {
            get { return _U_AmtSC; }
            set { SetPropertyValue("U_AmtSC", ref _U_AmtSC, value); }
        }

        private int _U_UseLifeD;
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Depr Count Adj."), ToolTip("Depr Count Adj.")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("numeric(19,6)")]
        public int U_UseLifeD
        {
            get { return _U_UseLifeD; }
            set { SetPropertyValue("U_UseLifeD", ref _U_UseLifeD, value); }
        }

        private string _U_AccCodG;
        [XafDisplayName("Adj. Gain/Loss A/C Code"), ToolTip("Revalue Surplus A/C Code")]
        [Index(40), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodG
        {
            get { return _U_AccCodG; }
            set { SetPropertyValue("U_AccCodG", ref _U_AccCodG, value); }
        }

        private FTS11_vw_OACT _U_AccFrmG;
        [XafDisplayName("Adj. Gain/Loss A/C"), ToolTip("Adj. Gain/Loss A/C")]
        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [XafDisplayName("Adj. Gain/Loss A/C Name"), ToolTip("Adj. Gain/Loss A/C Name")]
        [Index(42), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamG
        {
            get { return _U_AccNamG; }
            set { SetPropertyValue("U_AccNamG", ref _U_AccNamG, value); }
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