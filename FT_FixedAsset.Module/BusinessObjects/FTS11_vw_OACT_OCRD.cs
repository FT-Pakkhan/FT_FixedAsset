using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Account/BP Master")]
    [DefaultProperty("FormatCode")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11_vw_OACT_OCRD : XPLiteObject
    { 
        public FTS11_vw_OACT_OCRD(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
        }

        private string _AcctCode;
        [XafDisplayName("AcctCode"), ToolTip("AcctCode")]
        [Index(10), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string AcctCode
        {
            get { return _AcctCode; }
            set { SetPropertyValue("AcctCode", ref _AcctCode, value); }
        }

        private string _FormatCode;
        [Key]
        [XafDisplayName("Account Code"), ToolTip("Account Code")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string FormatCode
        {
            get { return _FormatCode; }
            set { SetPropertyValue("FormatCode", ref _FormatCode, value); }
        }

        private string _AcctName;
        [XafDisplayName("Account Name"), ToolTip("Account Name")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string AcctName
        {
            get { return _AcctName; }
            set { SetPropertyValue("AcctName", ref _AcctName, value); }
        }
    }
}