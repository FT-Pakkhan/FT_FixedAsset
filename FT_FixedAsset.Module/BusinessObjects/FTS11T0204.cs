using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Transferred Out")]
    [ImageName("ListBullets_16x16")]
    [DefaultProperty("DocNum")]
    [NavigationItem("Fixed Asset")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", Criteria = "Status.Code != 'O'", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableCancelTrans", AppearanceItemType.Action, "True", Criteria = "Status.Code != 'O' and Status.Code != 'P'", TargetItems = "CancelTrans", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11T0204 : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11T0204(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            Status = Session.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'O'"));
            U_DocDate = DateTime.Today;
            U_RefDate = DateTime.Today;
            if (SecuritySystem.CurrentUserId != null)
            {
                U_CreateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
                U_UpdateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
            }
            CreateDate = DateTime.Today;
            UpdateDate = DateTime.Today;
            Object = "FTS11T0204";
        }

        #region Child Table
        [Association("FTS11T0204-FTS11T0204D", typeof(FTS11T0204D))]
        public XPCollection FTS11T0204D
        {
            get { return GetCollection("FTS11T0204D"); }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private SE_FAStatus _Status;
        [XafDisplayName("Status"), ToolTip("Status")]
        [NoForeignKey]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Status", Enabled = false)]
        public SE_FAStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }

        private int _DocNum;
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Document No."), ToolTip("Document No,")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [Appearance("DocNum", Enabled = false)]
        [DbType("int")]
        public int DocNum
        {
            get { return DocEntry; }
            set { SetPropertyValue("DocNum", ref _DocNum, value); }
        }

        private string _U_CreateBy;
        [XafDisplayName("Created By"), ToolTip("Created By")]
        [Index(20), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_CreateBy
        {
            get { return _U_CreateBy; }
            set { SetPropertyValue("U_CreateBy", ref _U_CreateBy, value); }
        }

        private string _U_UpdateBy;
        [XafDisplayName("Updated By"), ToolTip("Updated By")]
        [Index(21), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_UpdateBy
        {
            get { return _U_UpdateBy; }
            set { SetPropertyValue("U_UpdateBy", ref _U_UpdateBy, value); }
        }

        private DateTime _U_CnclDate;
        [Index(30), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Cancel Date"), ToolTip("Cancel Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime U_CnclDate
        {
            get { return _U_CnclDate; }
            set { SetPropertyValue("U_CnclDate", ref _U_CnclDate, value); }
        }

        private string _U_CnclBy;
        [XafDisplayName("Cancel By"), ToolTip("Cancel By")]
        [Index(31), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_CnclBy
        {
            get { return _U_CnclBy; }
            set { SetPropertyValue("U_CnclBy", ref _U_CnclBy, value); }
        }

        private DateTime _U_RvrsDate;
        [Index(40), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Reverse Date"), ToolTip("Reverse Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime U_RvrsDate
        {
            get { return _U_RvrsDate; }
            set { SetPropertyValue("U_RvrsDate", ref _U_RvrsDate, value); }
        }

        private string _U_RvrsBy;
        [XafDisplayName("Reverse By"), ToolTip("Reverse By")]
        [Index(41), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_RvrsBy
        {
            get { return _U_RvrsBy; }
            set { SetPropertyValue("U_RvrsBy", ref _U_RvrsBy, value); }
        }

        private DateTime _U_DocDate;
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Document Date"), ToolTip("Document Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime U_DocDate
        {
            get { return _U_DocDate; }
            set { SetPropertyValue("U_DocDate", ref _U_DocDate, value); }
        }

        private DateTime _U_RefDate;
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Ref Date"), ToolTip("Ref Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime U_RefDate
        {
            get { return _U_RefDate; }
            set { SetPropertyValue("U_RefDate", ref _U_RefDate, value); }
        }

        private string _U_Ref1;
        [XafDisplayName("Ref 1"), ToolTip("Ref 1")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ref1
        {
            get { return _U_Ref1; }
            set { SetPropertyValue("U_Ref1", ref _U_Ref1, value); }
        }

        private string _U_Ref2;
        [XafDisplayName("Ref 2"), ToolTip("Ref 2")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Ref2
        {
            get { return _U_Ref2; }
            set { SetPropertyValue("U_Ref2", ref _U_Ref2, value); }
        }

        private string _U_Remark1;
        [XafDisplayName("Remark"), ToolTip("Remark")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_Remark1
        {
            get { return _U_Remark1; }
            set { SetPropertyValue("U_Remark1", ref _U_Remark1, value); }
        }

        private DateTime _CreateDate;
        [Index(997), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private DateTime _UpdateDate;
        [Index(998), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
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