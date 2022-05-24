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

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Transaction Journal")]
    public class FTS11OJDT : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11OJDT(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _DocEntry;
        [Key]
        [XafDisplayName("DocEntry"), ToolTip("DocEntry")]
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public int DocEntry
        {
            get { return _DocEntry; }
            set { SetPropertyValue("DocEntry", ref _DocEntry, value); }
        }

        private DateTime _DocDate;
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Document Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set { SetPropertyValue("DocDate", ref _DocDate, value); }
        }

        private DateTime _CreateDate;
        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private string _CreateBy;
        [XafDisplayName("Create By"), ToolTip("Create By")]
        [Index(31), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string CreateBy
        {
            get { return _CreateBy; }
            set { SetPropertyValue("CreateBy", ref _CreateBy, value); }
        }

        private DateTime _PostDate;
        [Index(40), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Post Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime PostDate
        {
            get { return _PostDate; }
            set { SetPropertyValue("PostDate", ref _PostDate, value); }
        }

        private string _PostBy;
        [XafDisplayName("Post By"), ToolTip("Post By")]
        [Index(41), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string PostBy
        {
            get { return _PostBy; }
            set { SetPropertyValue("PostBy", ref _PostBy, value); }
        }

        private string _TransStatus;
        [XafDisplayName("TransStatus"), ToolTip("Trans Status")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string TransStatus
        {
            get { return _TransStatus; }
            set { SetPropertyValue("TransStatus", ref _TransStatus, value); }
        }

        private int _TransID;
        [Index(51), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Trans ID"), ToolTip("Trans ID")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("int")]
        public int TransID
        {
            get { return _TransID; }
            set { SetPropertyValue("TransID", ref _TransID, value); }
        }

        private int _Series;
        [Index(60), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Series"), ToolTip("Series")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("int")]
        public int Series
        {
            get { return _Series; }
            set { SetPropertyValue("Series", ref _Series, value); }
        }

        private string _Ref1;
        [XafDisplayName("Ref1"), ToolTip("Ref1")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string Ref1
        {
            get { return _Ref1; }
            set { SetPropertyValue("Ref1", ref _Ref1, value); }
        }

        private string _Ref2;
        [XafDisplayName("Ref2"), ToolTip("Ref2")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string Ref2
        {
            get { return _Ref2; }
            set { SetPropertyValue("Ref2", ref _Ref2, value); }
        }

        private string _Memo;
        [XafDisplayName("Memo"), ToolTip("Memo")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DbType("nvarchar(50)")]
        public string Memo
        {
            get { return _Memo; }
            set { SetPropertyValue("Memo", ref _Memo, value); }
        }

        private string _PostTo;
        [XafDisplayName("PostTo"), ToolTip("PostTo")]
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DbType("nvarchar(50)")]
        public string PostTo
        {
            get { return _PostTo; }
            set { SetPropertyValue("PostTo", ref _PostTo, value); }
        }

        private string _FileType;
        [XafDisplayName("FileType"), ToolTip("FileType")]
        [Index(110), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DbType("nvarchar(50)")]
        public string FileType
        {
            get { return _FileType; }
            set { SetPropertyValue("FileType", ref _FileType, value); }
        }
    }
}