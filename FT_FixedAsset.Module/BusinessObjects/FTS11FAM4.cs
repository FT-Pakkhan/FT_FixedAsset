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
    [XafDisplayName("Referenced Document")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11FAM4 : XPLiteObject
    {
        public FTS11FAM4(Session session) 
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        #region Parent Table
        private FTS11OFAM _U_FAKey;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11OFAM-FTS11FAM4", typeof(FTS11OFAM))]
        public FTS11OFAM U_FAKey
        {
            get { return _U_FAKey; }
            set { SetPropertyValue("U_FAKey", ref _U_FAKey, value); }
        }
        #endregion

        private DateTime _U_DocDate;
        [XafDisplayName("Doc. Date"), ToolTip("Doc. Date")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_DocDate
        {
            get { return _U_DocDate; }
            set { SetPropertyValue("U_DocDate", ref _U_DocDate, value); }
        }

        private string _U_NumAtCard;
        //[XafDisplayName("Reference No."), ToolTip("Reference No.")]
        [XafDisplayName("VR No."), ToolTip("VR No.")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_NumAtCard
        {
            get { return _U_NumAtCard; }
            set { SetPropertyValue("U_NumAtCard", ref _U_NumAtCard, value); }
        }

        private string _U_NumAtCard2;
        //[XafDisplayName("Reference No. 2"), ToolTip("Reference No. 2")]
        [XafDisplayName("Invoice No."), ToolTip("Invoice No.")]
        [Index(21), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_NumAtCard2
        {
            get { return _U_NumAtCard2; }
            set { SetPropertyValue("U_NumAtCard2", ref _U_NumAtCard2, value); }
        }

        private string _U_Desc;
        [XafDisplayName("Description"), ToolTip("Description")]
        [Size(254)]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Desc
        {
            get { return _U_Desc; }
            set { SetPropertyValue("U_Desc", ref _U_Desc, value); }
        }

        private double _U_Amount;
        [XafDisplayName("Amount"), ToolTip("Amount")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_Amount
        {
            get { return _U_Amount; }
            set { SetPropertyValue("U_Amount", ref _U_Amount, value); }
        }

        private string _U_Reason;
        [XafDisplayName("Reason"), ToolTip("Reason")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Reason
        {
            get { return _U_Reason; }
            set { SetPropertyValue("U_Reason", ref _U_Reason, value); }
        }
    }
}