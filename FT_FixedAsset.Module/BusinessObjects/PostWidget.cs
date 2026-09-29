using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Posting Widget")]
    [ImageName("Design_16x16")]
    [NavigationItem("Widgets")]
    [NonPersistent]
    [Appearance("DisableReset", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&Close", AppearanceItemType.Action, "True", TargetItems = "SaveAndClose", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableCancel", AppearanceItemType.Action, "True", TargetItems = "Cancel", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class PostWidget : BaseObject
    {
        public PostWidget(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateFrm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrm.AddMonths(1).AddDays(-1);
            DocDate = DateFrm.AddMonths(1).AddDays(-1);
            PostOpt = Session.FindObject<SE_FAPostOpt>(CriteriaOperator.Parse("Code = 'S'"));
            PostTo = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_PostTo;
            FileType = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_FileType;
            Series = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_Series;
            JEMemo = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_JEMemo;
            Ref1 = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_Ref1;
            Ref2 = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_Ref2;
            if (SecuritySystem.CurrentUserId != null)
            {
                U_CreateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
            }
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

        private DateTime _DateFrm;
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Date From")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DateFrm
        {
            get { return _DateFrm; }
            set { SetPropertyValue("DateFrm", ref _DateFrm, value); }
        }

        private DateTime _DateTo;
        [Index(11), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Date To")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [ImmediatePostData]
        public DateTime DateTo
        {
            get { return _DateTo; }
            set
            {
                SetPropertyValue("DateTo", ref _DateTo, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("DocDate", ref _DocDate, value);
                }
            }
        }

        private FTS11OFAM _FACodeFrm;
        [XafDisplayName("F/A Code From"), ToolTip("F/A Code From")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAM FACodeFrm
        {
            get { return _FACodeFrm; }
            set { SetPropertyValue("FACodeFrm", ref _FACodeFrm, value); }
        }

        private FTS11OFAM _FACodeTo;
        [XafDisplayName("F/A Code To"), ToolTip("F/A Code To")]
        [Index(21), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAM FACodeTo
        {
            get { return _FACodeTo; }
            set { SetPropertyValue("FACodeTo", ref _FACodeTo, value); }
        }

        private FTS11OFAG _FAGrpFrm;
        [XafDisplayName("F/A Group From"), ToolTip("F/A Group From")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAG FAGrpFrm
        {
            get { return _FAGrpFrm; }
            set { SetPropertyValue("FAGrpFrm", ref _FAGrpFrm, value); }
        }

        private FTS11OFAG _FAGrpTo;
        [XafDisplayName("F/A Group To"), ToolTip("F/A Group To")]
        [Index(31), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAG FAGrpTo
        {
            get { return _FAGrpTo; }
            set { SetPropertyValue("FAGrpTo", ref _FAGrpTo, value); }
        }

        private FTS11OFAC _FAClsFrm;
        [XafDisplayName("F/A Class From"), ToolTip("F/A Class From")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAC FAClsFrm
        {
            get { return _FAClsFrm; }
            set { SetPropertyValue("FAClsFrm", ref _FAClsFrm, value); }
        }

        private FTS11OFAC _FAClsTo;
        [XafDisplayName("F/A Class To"), ToolTip("F/A Class To")]
        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAC FAClsTo
        {
            get { return _FAClsTo; }
            set { SetPropertyValue("FAClsTo", ref _FAClsTo, value); }
        }

        private SE_FAPostOpt _PostOpt;
        [XafDisplayName("Option"), ToolTip("Option")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public SE_FAPostOpt PostOpt
        {
            get { return _PostOpt; }
            set { SetPropertyValue("PostOpt", ref _PostOpt, value); }
        }

        private SE_FAPostTo _PostTo;
        [XafDisplayName("Post To"), ToolTip("Post To")]
        [Index(51), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public SE_FAPostTo PostTo
        {
            get { return _PostTo; }
            set { SetPropertyValue("PostTo", ref _PostTo, value); }
        }

        private SE_FAFileType _FileType;
        [XafDisplayName("As"), ToolTip("As")]
        [Index(52), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public SE_FAFileType FileType
        {
            get { return _FileType; }
            set { SetPropertyValue("FileType", ref _FileType, value); }
        }

        private FTS11_vw_NNM1 _Series;
        [XafDisplayName("Series"), ToolTip("Series")]
        [Index(53), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public FTS11_vw_NNM1 Series
        {
            get { return _Series; }
            set { SetPropertyValue("Series", ref _Series, value); }
        }

        private DateTime _DocDate;
        [Index(54), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Posting Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set { SetPropertyValue("DocDate", ref _DocDate, value); }
        }

        private string _Ref1;
        [XafDisplayName("Ref. 1"), ToolTip("Ref. 1")]
        [Index(55), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string Ref1
        {
            get { return _Ref1; }
            set { SetPropertyValue("Ref1", ref _Ref1, value); }
        }

        private string _Ref2;
        [XafDisplayName("Ref. 2"), ToolTip("Ref. 2")]
        [Index(56), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string Ref2
        {
            get { return _Ref2; }
            set { SetPropertyValue("Ref2", ref _Ref2, value); }
        }

        private string _JEMemo;
        [XafDisplayName("Journal Remarks"), ToolTip("Journal Remarks")]
        [Index(57), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Size(50)]
        public string JEMemo
        {
            get { return _JEMemo; }
            set { SetPropertyValue("JEMemo", ref _JEMemo, value); }
        }

        private string _U_CreateBy;
        [XafDisplayName("Created By"), ToolTip("Created By")]
        [Index(60), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_CreateBy
        {
            get { return _U_CreateBy; }
            set { SetPropertyValue("U_CreateBy", ref _U_CreateBy, value); }
        }
    }
}
