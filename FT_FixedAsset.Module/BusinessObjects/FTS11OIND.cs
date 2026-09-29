using DevExpress.ExpressApp;
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
    [XafDisplayName("Indicator")]
    [NavigationItem("Setup")]
    [ImageName("LoadPageSetup_16x16")]
    [DefaultProperty("U_IndCode")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11OIND : XPLiteObject
    { 
        public FTS11OIND(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            if (SecuritySystem.CurrentUserId != null && SecuritySystem.CurrentUserId.ToString() != "")
            {
                U_CreateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
                U_UpdateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
            }
            CreateDate = DateTime.Today;
            UpdateDate = DateTime.Today;
            Object = "FTS11OIND";
        }

        #region Dummy field
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy01", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy01
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy02", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy02
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy03", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy03
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy04", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy04
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy05", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy05
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy06", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy06
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy07", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy07
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy08", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy08
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy09", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy09
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy10", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy10
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy11", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy11
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy12", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy12
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy13", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy13
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy14", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy14
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy15", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy15
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy16", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy16
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy17", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy17
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy18", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy18
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy19", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy19
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy20", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy20
        {
            get { return ""; }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private string _U_IndCode;
        [XafDisplayName("Code"), ToolTip("Code")]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save), RuleUniqueValue(DefaultContexts.Save)]
        public string U_IndCode
        {
            get { return _U_IndCode; }
            set { SetPropertyValue("U_IndCode", ref _U_IndCode, value); }
        }

        private string _U_IndName;
        [XafDisplayName("Name"), ToolTip("Name")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string U_IndName
        {
            get { return _U_IndName; }
            set { SetPropertyValue("U_IndName", ref _U_IndName, value); }
        }

        private SE_FAIndicator _U_IndDim;
        [XafDisplayName("Indicator"), ToolTip("Indicator")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FAIndicator U_IndDim
        {
            get { return _U_IndDim; }
            set { SetPropertyValue("U_IndDim", ref _U_IndDim, value); }
        }

        private string _U_CreateBy;
        [XafDisplayName("Create By"), ToolTip("Create By")]
        [Index(993), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_CreateBy
        {
            get { return _U_CreateBy; }
            set { SetPropertyValue("U_CreateBy", ref _U_CreateBy, value); }
        }

        private string _U_UpdateBy;
        [XafDisplayName("Update By"), ToolTip("Update By")]
        [Index(994), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_UpdateBy
        {
            get { return _U_UpdateBy; }
            set { SetPropertyValue("U_UpdateBy", ref _U_UpdateBy, value); }
        }

        private DateTime _CreateDate;
        [Index(996), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private DateTime _UpdateDate;
        [Index(997), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Update Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { SetPropertyValue("UpdateDate", ref _UpdateDate, value); }
        }

        private string _Object;
        [XafDisplayName("Object"), ToolTip("Object")]
        [Index(999), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
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

        [Browsable(false)]
        public bool IsNew
        {
            get { return Session.IsNewObject(this); }
        }
    }
}