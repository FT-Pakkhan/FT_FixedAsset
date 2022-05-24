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
    [XafDisplayName("Reclassed Out Details")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code != 'O'", TargetItems = "SwitchToEditMode; Edit; InlineEdit", Enabled = false, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code = 'P'", TargetItems = "Delete", Enabled = false, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [RuleCriteria("T0205D_Quantity", DefaultContexts.Save, "U_Quantity > 0", "\"Quantity\" must more than zero.")]

    public class FTS11T0205D : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11T0205D(Session session)
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
        private FTS11T0205 _DocEntry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FTS11T0205-FTS11T0205D", typeof(FTS11T0205))]
        public FTS11T0205 DocEntry
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
            set
            {
                SetPropertyValue("U_FAKey", ref _U_FAKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, value.U_FACode);
                        SetPropertyValue("U_FAName", ref _U_FAName, value.U_FAName);
                        SetPropertyValue("U_Quantity", ref _U_Quantity, value.SQuantity);
                        SetPropertyValue("U_LocationKey", ref _U_LocationKey, Session.FindObject<FTS11OLOC>(CriteriaOperator.Parse("Code = ?", value.U_LastLoc)));
                        SetPropertyValue("U_Location", ref _U_Location, value.U_LastLoc);
                    }
                    else
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, "");
                        SetPropertyValue("U_FAName", ref _U_FAName, "");
                        SetPropertyValue("U_Quantity", ref _U_Quantity, 0);
                        SetPropertyValue("U_LocationKey", ref _U_LocationKey, null);
                        SetPropertyValue("U_Location", ref _U_Location, "");
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

        private int _U_Quantity;
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Quantity"), ToolTip("Quantity")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("numeric(19,6)")]
        public int U_Quantity
        {
            get { return _U_Quantity; }
            set { SetPropertyValue("U_Quantity", ref _U_Quantity, value); }
        }

        private FTS11OLOC _U_LocationKey;
        [XafDisplayName("Location"), ToolTip("Location")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OLOC U_LocationKey
        {
            get { return _U_LocationKey; }
            set
            {
                SetPropertyValue("U_LocationKey", ref _U_LocationKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_Location", ref _U_Location, value.Code);
                    }
                    else
                    {
                        SetPropertyValue("U_Location", ref _U_Location, "");
                    }
                }
            }
        }

        private string _U_Location;
        [XafDisplayName("Location Code"), ToolTip("Location Code")]
        [Index(31), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_Location
        {
            get { return _U_Location; }
            set { SetPropertyValue("U_Location", ref _U_Location, value); }
        }

        private string _U_AccCodW;
        [XafDisplayName("Write Off A/C Code"), ToolTip("Write Off A/C Code")]
        [Index(40), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodW
        {
            get { return _U_AccCodW; }
            set { SetPropertyValue("U_AccCodW", ref _U_AccCodW, value); }
        }

        private FTS11_vw_OACT _U_AccFrmW;
        [XafDisplayName("Write Off Clearing A/C"), ToolTip("Write Off Clearing A/C")]
        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmW
        {
            get { return _U_AccFrmW; }
            set
            {
                SetPropertyValue("U_AccFrmW", ref _U_AccFrmW, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_AccCodW", ref _U_AccCodW, value.AcctCode);
                    SetPropertyValue("U_AccNamW", ref _U_AccNamW, value.AcctName);
                }
            }
        }

        private string _U_AccNamW;
        [XafDisplayName("Write Off Clearing A/C Name"), ToolTip("Write Off Clearing A/C Name")]
        [Index(42), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamW
        {
            get { return _U_AccNamW; }
            set { SetPropertyValue("U_AccNamW", ref _U_AccNamW, value); }
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