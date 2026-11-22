using System;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [ImageName("BO_Person")]
    [XafDisplayName("Staff")]
    [DefaultClassOptions]
    [DefaultProperty("FullName")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class SystemUsers : PermissionPolicyUser
    { 
        public SystemUsers(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            
        }

        private string _FullName;
        [XafDisplayName("Full Name"), ToolTip("Full Name")]
        [Index(0)]
        public string FullName
        {
            get { return _FullName; }
            set { SetPropertyValue("_FullName", ref _FullName, value); }
        }

        #region Password Administration (FTS11OPPA)
        private DateTime _LastPwdChgOn;
        [XafDisplayName("Last Password Changed On"), ToolTip("Last Password Changed On")]
        [Index(60), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("LastPwdChgOn", Enabled = false)]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy hh:mm:ss tt")]
        [ModelDefault("EditMask", "dd/MM/yyyy hh:mm:ss tt")]
        public DateTime LastPwdChgOn
        {
            get { return _LastPwdChgOn; }
            set { SetPropertyValue("LastPwdChgOn", ref _LastPwdChgOn, value); }
        }

        private bool _IsLocked;
        [XafDisplayName("Locked"), ToolTip("Untick to unlock the account")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public bool IsLocked
        {
            get { return _IsLocked; }
            set { SetPropertyValue("IsLocked", ref _IsLocked, value); }
        }

        private DateTime _LockedDate;
        [XafDisplayName("Locked Date"), ToolTip("Locked Date")]
        [Index(71), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("LockedDate", Enabled = false)]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy hh:mm:ss tt")]
        [ModelDefault("EditMask", "dd/MM/yyyy hh:mm:ss tt")]
        public DateTime LockedDate
        {
            get { return _LockedDate; }
            set { SetPropertyValue("LockedDate", ref _LockedDate, value); }
        }

        private int _FailedLogonCount;
        [XafDisplayName("Failed Logon Attempts"), ToolTip("Failed Logon Attempts")]
        [Index(72), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("FailedLogonCount", Enabled = false)]
        public int FailedLogonCount
        {
            get { return _FailedLogonCount; }
            set { SetPropertyValue("FailedLogonCount", ref _FailedLogonCount, value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (IsLoading || IsSaving || propertyName != nameof(IsLocked)) return;

            if (IsLocked)
            {
                LockedDate = DateTime.Now;
            }
            else
            {
                // unlocked (by the administrator or after the lock duration) - start counting again
                LockedDate = DateTime.MinValue;
                FailedLogonCount = 0;
            }
        }
        #endregion
    }
}