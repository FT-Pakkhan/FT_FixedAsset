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
    [XafDisplayName("Transaction")]
    [DefaultProperty("DocEntry")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11TJDT : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11TJDT(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Child Table
        [Association("FTS11TJDT-FTS11TJDTD", typeof(FTS11TJDTD))]
        public XPCollection FTS11TJDTD
        {
            get { return GetCollection("FTS11TJDTD"); }
        }
        #endregion

        private int _DocEntry;
        [Key]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Document No."), ToolTip("Document No,")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [Appearance("DocEntry", Enabled = false)]
        [DbType("int")]
        public int DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
        }

        private FTS11OFAM _FAKey;
        [XafDisplayName("F/A Code"), ToolTip("F/A Code")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OFAM FAKey
        {
            get { return _FAKey; }
            set { SetPropertyValue("FAKey", ref _FAKey, value); }
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

        private DateTime _DocDate;
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Document Date"), ToolTip("Document Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set { SetPropertyValue("DocDate", ref _DocDate, value); }
        }

        private string _TransType;
        [XafDisplayName("Trans Type"), ToolTip("Trans Type")]
        [Index(80), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string TransType
        {
            get { return _TransType; }
            set { SetPropertyValue("TransType", ref _TransType, value); }
        }

        private string _Remark;
        [XafDisplayName("Remark"), ToolTip("Remark")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue("Remark", ref _Remark, value); }
        }

        private SE_FAStatus _Status;
        [XafDisplayName("Status"), ToolTip("Status")]
        [NoForeignKey]
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Status", Enabled = false)]
        public SE_FAStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }
    }
}