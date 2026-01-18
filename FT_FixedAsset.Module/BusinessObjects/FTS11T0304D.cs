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
    [XafDisplayName("Move Details")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code != 'O'", TargetItems = "SwitchToEditMode; Edit; InlineEdit", Enabled = false, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", Criteria = "LineStatus.Code = 'P'", TargetItems = "Delete", Enabled = false, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [RuleCriteria("T0304D_Quantity", DefaultContexts.Save, "U_Quantity > 0", "\"Quantity\" must more than zero.")]

    public class FTS11T0304D : XPLiteObject
    { 
        public FTS11T0304D(Session session)
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
        private FTS11T0304 _DocEntry;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Association("FTS11T0304-FTS11T0304D", typeof(FTS11T0304))]
        public FTS11T0304 DocEntry
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
                        SetPropertyValue("U_Quantity", ref _U_Quantity, value.SQuantity);
                        SetPropertyValue("U_FrLocKey", ref _U_FrLocKey, Session.FindObject<FTS11OLOC>(CriteriaOperator.Parse("Code = ?", value.U_LastLoc)));
                        SetPropertyValue("U_FrLoc", ref _U_FrLoc, value.U_LastLoc);
                    }
                    else
                    {
                        SetPropertyValue("U_FACode", ref _U_FACode, "");
                        SetPropertyValue("U_FAName", ref _U_FAName, "");
                        SetPropertyValue("U_Quantity", ref _U_Quantity, 0);
                        SetPropertyValue("U_FrLocKey", ref _U_FrLocKey, null);
                        SetPropertyValue("U_FrLoc", ref _U_FrLoc, "");
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

        private double _U_Quantity;
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Quantity"), ToolTip("Quantity")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [Appearance("Quantity", Enabled = false)]
        [DbType("numeric(19,6)")]
        public double U_Quantity
        {
            get { return _U_Quantity; }
            set { SetPropertyValue("U_Quantity", ref _U_Quantity, value); }
        }

        private FTS11OLOC _U_FrLocKey;
        [XafDisplayName("Location From"), ToolTip("Location From")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("U_FrLocKey", Enabled = false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public FTS11OLOC U_FrLocKey
        {
            get { return _U_FrLocKey; }
            set
            {
                SetPropertyValue("U_FrLocKey", ref _U_FrLocKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_FrLoc", ref _U_FrLoc, value.Code);
                    }
                    else
                    {
                        SetPropertyValue("U_FrLoc", ref _U_FrLoc, "");
                    }
                }
            }
        }

        private string _U_FrLoc;
        [XafDisplayName("Location From Code"), ToolTip("Location From Code")]
        [Index(31), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_FrLoc
        {
            get { return _U_FrLoc; }
            set { SetPropertyValue("U_FrLoc", ref _U_FrLoc, value); }
        }

        private FTS11OLOC _U_ToLocKey;
        [XafDisplayName("Location To"), ToolTip("Location To")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public FTS11OLOC U_ToLocKey
        {
            get { return _U_ToLocKey; }
            set
            {
                SetPropertyValue("U_ToLocKey", ref _U_ToLocKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_ToLoc", ref _U_ToLoc, value.Code);
                    }
                    else
                    {
                        SetPropertyValue("U_ToLoc", ref _U_ToLoc, "");
                    }
                }
            }
        }

        private string _U_ToLoc;
        [XafDisplayName("Location To Code"), ToolTip("Location To Code")]
        [Index(41), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_ToLoc
        {
            get { return _U_ToLoc; }
            set { SetPropertyValue("U_ToLoc", ref _U_ToLoc, value); }
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