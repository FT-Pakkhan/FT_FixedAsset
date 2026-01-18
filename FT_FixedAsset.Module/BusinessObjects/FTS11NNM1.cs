using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Numbering")]
    [NavigationItem("System Initialization")]
    [XafDefaultProperty("DocType")]
    [ImageName("ListNumbers_16x16")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11NNM1 : XPLiteObject
    {
        public FTS11NNM1(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Object = "FTS11NNM1";
        }

        #region Child Table
        [Association("FTS11NNM1-FTS11NNM2")]
        public XPCollection<FTS11NNM2> FTS11NNM2
        {
            get { return GetCollection<FTS11NNM2>("FTS11NNM2"); }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private string _DocType;
        [XafDisplayName("Document Type"), ToolTip("Document Type")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save), RuleUniqueValue(DefaultContexts.Save)]
        public string DocType
        {
            get { return _DocType; }
            set { SetPropertyValue("DocType", ref _DocType, value); }
        }

        private string _Prefix;
        [XafDisplayName("Prefix"), ToolTip("Prefix")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string Prefix
        {
            get { return _Prefix; }
            set { SetPropertyValue("Prefix", ref _Prefix, value); }
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

        private string _Suffix;
        [XafDisplayName("Suffix"), ToolTip("Suffix")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string Suffix
        {
            get { return _Suffix; }
            set { SetPropertyValue("Suffix", ref _Suffix, value); }
        }

        private int _Length;
        [XafDisplayName("Length"), ToolTip("Length")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public int Length
        {
            get { return _Length; }
            set { SetPropertyValue("Length", ref _Length, value); }
        }

        private bool _Month;
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Month")]
        public bool Month
        {
            get { return _Month; }
            set { SetPropertyValue("Month", ref _Month, value); }
        }

        private bool _Year;
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Year")]
        public bool Year
        {
            get { return _Year; }
            set { SetPropertyValue("Year", ref _Year, value); }
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