using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FileSystemData.BusinessObjects;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Attachment")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11FAM3 : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11FAM3(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        #region Parent Table
        private FTS11OFAM _U_FAKey;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11OFAM-FTS11FAM3", typeof(FTS11OFAM))]
        public FTS11OFAM U_FAKey
        {
            get { return _U_FAKey; }
            set { SetPropertyValue("U_FAKey", ref _U_FAKey, value); }
        }
        #endregion

        [DevExpress.ExpressApp.DC.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        [Index(575), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Attachment"), ToolTip("Attachment")]
        public FileSystemStoreObject U_FilePath
        {
            get { return GetPropertyValue<FileSystemStoreObject>("U_FilePath"); }
            set { SetPropertyValue<FileSystemStoreObject>("U_FilePath", value); }
        }
    }
}