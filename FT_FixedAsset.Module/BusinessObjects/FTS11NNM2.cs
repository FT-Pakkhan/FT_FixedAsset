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
    [DomainComponent]
    [XafDisplayName("Numbering by Date")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11NNM2 : XPLiteObject
    {
        public FTS11NNM2(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int Oid { get; set; }

        #region Parent Table
        private FTS11NNM1 _DocEntry;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11NNM1-FTS11NNM2", typeof(FTS11NNM1))]
        public FTS11NNM1 DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
        }
        #endregion

        private int _DocYear;
        [XafDisplayName("Year"), ToolTip("Year")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:d}")]
        [RuleRequiredField(DefaultContexts.Save)]
        public int DocYear
        {
            get { return _DocYear; }
            set { SetPropertyValue("DocYear", ref _DocYear, value); }
        }

        private int _DocMonth;
        [XafDisplayName("Month"), ToolTip("Month")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:d}")]
        [RuleRequiredField(DefaultContexts.Save)]
        public int DocMonth
        {
            get { return _DocMonth; }
            set { SetPropertyValue("DocMonth", ref _DocMonth, value); }
        }

        private int _Seq;
        [XafDisplayName("Seq"), ToolTip("Seq")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public int Seq
        {
            get { return _Seq; }
            set { SetPropertyValue("Seq", ref _Seq, value); }
        }

        private int _Length;
        [XafDisplayName("Length"), ToolTip("Length")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public int Length
        {
            get { return _Length; }
            set { SetPropertyValue("Length", ref _Length, value); }
        }
    }
}