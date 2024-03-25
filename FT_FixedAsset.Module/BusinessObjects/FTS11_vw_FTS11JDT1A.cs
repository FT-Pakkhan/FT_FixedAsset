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
    [DomainComponent]
    [NonPersistent]
    [XafDisplayName("Trans. Log C")]
    public class FTS11_vw_FTS11JDT1C : XPObject
    {
        public FTS11_vw_FTS11JDT1C(Session session)
            : base(session)
        {
        }

        public IList<FTS11_vw_FTS11JDT1A> AddLog(XPCollection<FTS11_vw_FTS11JDT1> mylist)
        {
            IList<FTS11_vw_FTS11JDT1A> rtnlist = new List<FTS11_vw_FTS11JDT1A>();

            foreach (FTS11_vw_FTS11JDT1 dtl in mylist.OrderBy(pp => pp.DocDate).ThenBy(pp => pp.TransType).ThenBy(pp => pp.DocEntry).ThenBy(pp => pp.TransID))
            {
                FTS11_vw_FTS11JDT1A obj = new FTS11_vw_FTS11JDT1A(Session);
                obj.DocKey = dtl.DocKey;
                obj.DocDate = dtl.DocDate;
                obj.CreateDate = dtl.CreateDate;
                obj.TransStatus = dtl.TransStatus;
                obj.DocEntry = dtl.DocEntry;
                obj.TransType = dtl.TransType;
                obj.BaseEntry = dtl.BaseEntry;
                obj.Name = dtl.Name;
                obj.Quantity = dtl.Quantity;
                obj.Location = dtl.Location;
                obj.CostFC = dtl.CostFC;
                obj.CostLC = dtl.CostLC;
                obj.AccuLC = dtl.AccuLC;
                obj.DeprnCnt = dtl.DeprnCnt;
                obj.UseLife = dtl.UseLife;
                obj.DeprPrcnt = dtl.DeprPrcnt;
                obj.Revalue = dtl.Revalue;
                obj.RevalueSC = dtl.RevalueSC;
                rtnlist.Add(obj);
            }

            return rtnlist;
        }
    }

    [DomainComponent]
    [XafDisplayName("Trans. Log A")]
    [NonPersistent]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11_vw_FTS11JDT1A : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11_vw_FTS11JDT1A(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        //private string _DocKey;
        [Key]
        [XafDisplayName("DocKey"), ToolTip("DocKey")]
        [Index(10), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string DocKey { get; set; }

        //private DateTime _DocDate;
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Trans. Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DocDate { get; set; }

        //private DateTime _CreateDate;
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Create. Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime CreateDate { get; set; }

        //private string _TransStatus;
        [XafDisplayName("Status"), ToolTip("Status")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string TransStatus { get; set; }

        //private int _DocEntry;
        [Index(60), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("DocEntry"), ToolTip("DocEntry")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int DocEntry { get; set; }

        //private string _TransType;
        [XafDisplayName("Trans. Type"), ToolTip("Trans. Type")]
        [Index(50), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string TransType { get; set; }

        //private int _BaseEntry;
        [Index(80), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("BaseEntry"), ToolTip("BaseEntry")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int BaseEntry { get; set; }

        //private string _Name;
        [XafDisplayName("Trans. Name"), ToolTip("Trans. Name")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Name { get; set; }

        //private int _Quantity;
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Quantity"), ToolTip("Quantity")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double Quantity { get; set; }

        //Private string _Location;
        [XafDisplayName("Location"), ToolTip("Location")]
        [Index(110), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Location { get; set; }

        //Private double _CostFC;
        [XafDisplayName("Cost (FC)"), ToolTip("Cost (FC)")]
        [Index(120), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double CostFC { get; set; }

        //Private double _CostLC;
        [XafDisplayName("Cost (LC)"), ToolTip("Cost (LC)")]
        [Index(121), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double CostLC { get; set; }

        //Private double _AccuLC;
        [XafDisplayName("Deprn (LC)"), ToolTip("Deprn (LC)")]
        [Index(130), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double AccuLC { get; set; }

        //Private int _UseLife;
        [Index(140), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Useful Life"), ToolTip("Useful Life")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double UseLife { get; set; }

        //Private int _DeprPrcnt;
        [Index(141), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Percentage"), ToolTip("Percentage")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double DeprPrcnt { get; set; }

        //Private int _DeprnCnt;
        [Index(150), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Deprn Cnt"), ToolTip("Deprn Cnt")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        public double DeprnCnt { get; set; }

        //Private double _Revalue;
        [XafDisplayName("Residual Value (LC)"), ToolTip("Residual Value (LC)")]
        [Index(160), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double Revalue { get; set; }

        //Private double _RevalueSC;
        [XafDisplayName("Residual Value (SC)"), ToolTip("Residual Value (SC)")]
        [Index(161), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double RevalueSC { get; set; }

    }
}