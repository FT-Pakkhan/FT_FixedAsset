using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Reports")]
    [NavigationItem("Setup")]
    [ImageName("LoadPageSetup_16x16")]
    [DefaultProperty("Name")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11SRPT : XPLiteObject
    { 
        public FTS11SRPT(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Object = "FTS11SRPT";
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private string _Name;
        [XafDisplayName("Name"), ToolTip("Name")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private string _DWName;
        [XafDisplayName("CR Name"), ToolTip("CR Name")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string DWName
        {
            get { return _DWName; }
            set { SetPropertyValue("DWName", ref _DWName, value); }
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