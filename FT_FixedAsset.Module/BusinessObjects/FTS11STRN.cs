using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Transaction Types")]
    [NavigationItem("Setup")]
    [ImageName("LoadPageSetup_16x16")]
    [DefaultProperty("Name")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11STRN : XPLiteObject
    { 
        public FTS11STRN(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Object = "FTS11STRN";
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private string _Code;
        [XafDisplayName("Code"), ToolTip("Code")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Code", Enabled = false)]
        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        private string _Name;
        [XafDisplayName("Name"), ToolTip("Name")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Name", Enabled = false)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private string _U_TrnsGrp;
        [XafDisplayName("Group"), ToolTip("Group")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("U_TrnsGrp", Enabled = false)]
        public string U_TrnsGrp
        {
            get { return _U_TrnsGrp; }
            set { SetPropertyValue("U_TrnsGrp", ref _U_TrnsGrp, value); }
        }

        private bool _U_Post;
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Posting")]
        public bool U_Post
        {
            get { return _U_Post; }
            set { SetPropertyValue("U_Post", ref _U_Post, value); }
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