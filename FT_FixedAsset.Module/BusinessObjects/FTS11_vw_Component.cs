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
    [XafDisplayName("Component")]
    [DefaultProperty("DocEntry")]
    [Appearance("DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11_vw_Component : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11_vw_Component(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Parent Table
        private FTS11OFAM _FAKey;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11OFAM-FTS11_vw_Component", typeof(FTS11OFAM))]
        public FTS11OFAM FAKey
        {
            get { return _FAKey; }
            set { SetPropertyValue("FAKey", ref _FAKey, value); }
        }
        #endregion

        private string _DocEntry;
        [Key]
        [XafDisplayName("DocEntry"), ToolTip("DocEntry")]
        [Index(10), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        //[ModelDefault("DisplayFormat", "{0:n0}")]
        //[ModelDefault("EditMask", "{0:n0}")]
        public string DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
        }

        private int _ADocEntry;
        [XafDisplayName("ADocEntry"), ToolTip("ADocEntry")]
        [Index(10), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int ADocEntry
        {
            get { return _ADocEntry; }
            set { SetPropertyValue("ADocEntry", ref _ADocEntry, value); }
        }

        private int _Order;
        [Index(11), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Order"), ToolTip("Order")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int Order
        {
            get { return _Order; }
            set { SetPropertyValue("Order", ref _Order, value); }
        }

        private string _Type;
        [XafDisplayName("Type"), ToolTip("Type")]
        [Index(12), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string Type
        {
            get { return _Type; }
            set { SetPropertyValue("Type", ref _Type, value); }
        }

        private string _U_FACode;
        [XafDisplayName("F/A Code"), ToolTip("F/A Code")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_FACode
        {
            get { return _U_FACode; }
            set { SetPropertyValue("U_FACode", ref _U_FACode, value); }
        }

        private string _U_FAName;
        [XafDisplayName("F/A Name"), ToolTip("F/A Name")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_FAName
        {
            get { return _U_FAName; }
            set { SetPropertyValue("U_FAName", ref _U_FAName, value); }
        }

        private int _U_Quantity;
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Quantity"), ToolTip("Quantity")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int U_Quantity
        {
            get { return _U_Quantity; }
            set { SetPropertyValue("U_Quantity", ref _U_Quantity, value); }
        }

        private double _U_CostLC;
        [XafDisplayName("Cost"), ToolTip("Cost")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double U_CostLC
        {
            get { return _U_CostLC; }
            set { SetPropertyValue("U_CostLC", ref _U_CostLC, value); }
        }

        private double _U_TCostLC;
        [XafDisplayName("Total Cost"), ToolTip("Total Cost")]
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double U_TCostLC
        {
            get { return _U_TCostLC; }
            set { SetPropertyValue("U_TCostLC", ref _U_TCostLC, value); }
        }

        private int _CFQty;
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("C/F Quantity"), ToolTip("C/F Quantity")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int CFQty
        {
            get { return _CFQty; }
            set { SetPropertyValue("CFQty", ref _CFQty, value); }
        }

        private double _CostLC;
        [XafDisplayName("C/F Cost"), ToolTip("C/F Cost")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double CostLC
        {
            get { return _CostLC; }
            set { SetPropertyValue("CostLC", ref _CostLC, value); }
        }

        private double _AccuLC;
        [XafDisplayName("Accu. Depr"), ToolTip("Accu. Depr")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double AccuLC
        {
            get { return _AccuLC; }
            set { SetPropertyValue("AccuLC", ref _AccuLC, value); }
        }

        private double _Nbv;
        [XafDisplayName("NBV"), ToolTip("NBV")]
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double Nbv
        {
            get { return _Nbv; }
            set { SetPropertyValue("Nbv", ref _Nbv, value); }
        }
    }
}