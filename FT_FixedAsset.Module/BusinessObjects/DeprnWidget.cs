using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using System;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Depreciation Widget")]
    [ImageName("Design_16x16")]
    [NavigationItem("Widgets")]
    [NonPersistent]
    [Appearance("DisableReset", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&Close", AppearanceItemType.Action, "True", TargetItems = "SaveAndClose", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableCancel", AppearanceItemType.Action, "True", TargetItems = "Cancel", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class DeprnWidget : BaseObject
    {
        public DeprnWidget(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateFrm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrm.AddMonths(1).AddDays(-1);
        }

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
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Date To")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DateTo
        {
            get { return _DateTo; }
            set { SetPropertyValue("DateTo", ref _DateTo, value); }
        }

        private FTS11OFAM _FACodeFrm;
        [XafDisplayName("F/A Code From"), ToolTip("F/A Code From")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAM FACodeFrm
        {
            get { return _FACodeFrm; }
            set { SetPropertyValue("FACodeFrm", ref _FACodeFrm, value); }
        }

        private FTS11OFAM _FACodeTo;
        [XafDisplayName("F/A Code To"), ToolTip("F/A Code To")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAM FACodeTo
        {
            get { return _FACodeTo; }
            set { SetPropertyValue("FACodeTo", ref _FACodeTo, value); }
        }

        private FTS11OFAG _FAGrpFrm;
        [XafDisplayName("F/A Group From"), ToolTip("F/A Group From")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAG FAGrpFrm
        {
            get { return _FAGrpFrm; }
            set { SetPropertyValue("FAGrpFrm", ref _FAGrpFrm, value); }
        }

        private FTS11OFAG _FAGrpTo;
        [XafDisplayName("F/A Group To"), ToolTip("F/A Group To")]
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAG FAGrpTo
        {
            get { return _FAGrpTo; }
            set { SetPropertyValue("FAGrpTo", ref _FAGrpTo, value); }
        }

        private FTS11OFAC _FAClsFrm;
        [XafDisplayName("F/A Class From"), ToolTip("F/A Class From")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAC FAClsFrm
        {
            get { return _FAClsFrm; }
            set { SetPropertyValue("FAClsFrm", ref _FAClsFrm, value); }
        }

        private FTS11OFAC _FAClsTo;
        [XafDisplayName("F/A Class To"), ToolTip("F/A Class To")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAC FAClsTo
        {
            get { return _FAClsTo; }
            set { SetPropertyValue("FAClsTo", ref _FAClsTo, value); }
        }
    }
}
