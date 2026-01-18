using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Review Useful Life Details")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code != 'O'", TargetItems = "SwitchToEditMode; Edit; InlineEdit", Enabled = false, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code = 'P'", TargetItems = "Delete", Enabled = false, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [RuleCriteria("T0301D_UseLifeN", DefaultContexts.Save, "U_UseLifeN > 0", "\"Useful Life\" must more than zero.")]

    public class FTS11T0301D : XPLiteObject
    { 
        public FTS11T0301D(Session session)
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
        private FTS11T0301 _DocEntry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FTS11T0301-FTS11T0301D", typeof(FTS11T0301))]
        public FTS11T0301 DocEntry
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
                        SetPropertyValue("U_DeprPrcntN", ref _U_DeprPrcntN, value.U_DeprPrcnt);
                    }
                    else
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, "");
                        SetPropertyValue("U_FAName", ref _U_FAName, "");
                        SetPropertyValue("U_UseLifeN", ref _U_UseLifeN, 0);
                        SetPropertyValue("U_DeprPrcntN", ref _U_DeprPrcntN, 0);
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
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("New Useful Life"), ToolTip("New Useful Life")]
        [Appearance("UseLife", Enabled = false, Criteria = "[U_FAKey].[U_DeprnBy].[Code] <> 'U'")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public double U_UseLifeN
        {
            get { return _U_UseLifeN; }
            set
            {
                SetPropertyValue("U_UseLifeN", ref _U_UseLifeN, value);
                if (!IsLoading && U_FAKey.U_DeprnBy.Code == "U")
                {
                    if (value != 0)
                        U_DeprPrcntN = Math.Round(12 / U_UseLifeN * 100, 6);
                    else
                        U_DeprPrcntN = 0;
                }
            }
        }

        private double _U_DeprPrcntN;
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("New Percentage"), ToolTip("New Percentage")]
        [Appearance("DeprPrcntN", Enabled = false, Criteria = "[U_FAKey].[U_DeprnBy].[Code] <> 'P'")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public double U_DeprPrcntN
        {
            get { return _U_DeprPrcntN; }
            set
            {
                SetPropertyValue("U_DeprPrcntN", ref _U_DeprPrcntN, value);
                if (!IsLoading && U_FAKey.U_DeprnBy.Code == "P")
                {
                    if (value != 0)
                        U_UseLifeN = Math.Round(12 / U_DeprPrcntN * 100, 6);
                    else
                        U_UseLifeN = 0;
                }
            }
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