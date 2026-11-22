using System;
using System.ComponentModel;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FT_FixedAsset.Module.BusinessObjects
{
    /// <summary>
    /// Password Administration (one record, DocEntry = 1). A value of 0 switches that rule off.
    /// Ported from the Hiap Teck Portal (HT11OPPA).
    /// </summary>
    [XafDisplayName("Password Administration")]
    [NavigationItem("System Initialization")]
    [ImageName("BO_Security")]
    [DefaultProperty("DocEntry")]
    [Appearance("PPA_DisableNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableSave&Close", AppearanceItemType.Action, "True", TargetItems = "SaveAndClose", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("PPA_DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11OPPA : XPLiteObject
    {
        public FTS11OPPA(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            string userName = CurrentUserName();
            CreateBy = userName;
            UpdateBy = userName;
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            Object = "FTS11OPPA";
            AccLocDuration = 30;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!Session.IsNewObject(this))
            {
                UpdateBy = CurrentUserName();
                UpdateDate = DateTime.Now;
            }
        }

        private string CurrentUserName()
        {
            if (SecuritySystem.CurrentUserId == null || SecuritySystem.CurrentUserId.ToString() == "") return null;
            SystemUsers u = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId);
            return u == null ? null : u.UserName;
        }

        #region Dummy field
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy01", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy01
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy02", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy02
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy03", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy03
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy04", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy04
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy05", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy05
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy06", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy06
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy07", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy07
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy08", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy08
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy09", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy09
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy10", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy10
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy11", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy11
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy12", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy12
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy13", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy13
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy14", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy14
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Dummy15", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy15
        {
            get { return ""; }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private double _PwdExp;
        [XafDisplayName("Expiration After"), ToolTip("Days before a password must be changed (0 = never expires)")]
        [Index(20)]
        [ModelDefault("DisplayFormat", "{0:0 Days}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double PwdExp
        {
            get { return _PwdExp; }
            set { SetPropertyValue("PwdExp", ref _PwdExp, value); }
        }

        private double _PwdMinLen;
        [XafDisplayName("Minimum Length"), ToolTip("Minimum Length")]
        [Index(21)]
        [ModelDefault("DisplayFormat", "{0:0 Characters}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double PwdMinLen
        {
            get { return _PwdMinLen; }
            set { SetPropertyValue("PwdMinLen", ref _PwdMinLen, value); }
        }

        private double _MinUppers;
        [XafDisplayName("Minimum Number of Upper Characters"), ToolTip("Minimum Number of Upper Characters")]
        [Index(22)]
        [ModelDefault("DisplayFormat", "{0:0}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double MinUppers
        {
            get { return _MinUppers; }
            set { SetPropertyValue("MinUppers", ref _MinUppers, value); }
        }

        private double _MinLowCase;
        [XafDisplayName("Minimum Number of Lower Characters"), ToolTip("Minimum Number of Lower Characters")]
        [Index(23)]
        [ModelDefault("DisplayFormat", "{0:0}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double MinLowCase
        {
            get { return _MinLowCase; }
            set { SetPropertyValue("MinLowCase", ref _MinLowCase, value); }
        }

        private double _MinDigits;
        [XafDisplayName("Minimum Number of Digits"), ToolTip("Minimum Number of Digits")]
        [Index(24)]
        [ModelDefault("DisplayFormat", "{0:0}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double MinDigits
        {
            get { return _MinDigits; }
            set { SetPropertyValue("MinDigits", ref _MinDigits, value); }
        }

        private double _MinNonAlph;
        [XafDisplayName("Minimum Number of Non-Alphanumeric Characters"), ToolTip("Minimum Number of Non-Alphanumeric Characters")]
        [Index(25)]
        [ModelDefault("DisplayFormat", "{0:0}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double MinNonAlph
        {
            get { return _MinNonAlph; }
            set { SetPropertyValue("MinNonAlph", ref _MinNonAlph, value); }
        }

        private double _NumPrevPwd;
        [XafDisplayName("Password cannot match"), ToolTip("Number of previous passwords that cannot be reused (0 = off)")]
        [Index(26)]
        [ModelDefault("DisplayFormat", "{0:0 Previous Passwords}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double NumPrevPwd
        {
            get { return _NumPrevPwd; }
            set { SetPropertyValue("NumPrevPwd", ref _NumPrevPwd, value); }
        }

        private double _NumAuthLoc;
        [XafDisplayName("Authentication before Users Account Locked"), ToolTip("Failed logons before the account is locked (0 = never lock)")]
        [Index(27)]
        [ModelDefault("DisplayFormat", "{0:0}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double NumAuthLoc
        {
            get { return _NumAuthLoc; }
            set { SetPropertyValue("NumAuthLoc", ref _NumAuthLoc, value); }
        }

        private double _AccLocDuration;
        [XafDisplayName("Users Account Lock Duration"), ToolTip("Minutes before a locked account unlocks itself")]
        [Index(28)]
        [ModelDefault("DisplayFormat", "{0:0 Minutes}")]
        [ModelDefault("EditMask", "{0:0}")]
        [DbType("numeric(19,6)")]
        public double AccLocDuration
        {
            get { return _AccLocDuration; }
            set { SetPropertyValue("AccLocDuration", ref _AccLocDuration, value); }
        }

        private string _CreateBy;
        [XafDisplayName("Create By")]
        [Index(9994), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string CreateBy
        {
            get { return _CreateBy; }
            set { SetPropertyValue("CreateBy", ref _CreateBy, value); }
        }

        private DateTime _CreateDate;
        [XafDisplayName("Create Date")]
        [Index(9995), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [DbType("datetime")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private string _UpdateBy;
        [XafDisplayName("Update By")]
        [Index(9996), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string UpdateBy
        {
            get { return _UpdateBy; }
            set { SetPropertyValue("UpdateBy", ref _UpdateBy, value); }
        }

        private DateTime _UpdateDate;
        [XafDisplayName("Update Date")]
        [Index(9997), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [DbType("datetime")]
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { SetPropertyValue("UpdateDate", ref _UpdateDate, value); }
        }

        private string _Object;
        [XafDisplayName("Object")]
        [Index(9999), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }

        [Browsable(false), NonPersistent]
        public bool IsNew
        {
            get { return Session.IsNewObject(this); }
        }

        /// <summary>
        /// Reads the settings with plain SQL, so the rules apply to every user even when
        /// their role has no permission to read FTS11OPPA. Returns null when not set up.
        /// </summary>
        public static PasswordPolicy Load(string connectionString)
        {
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                "IF OBJECT_ID('dbo.FTS11OPPA') IS NOT NULL " +
                "SELECT TOP 1 PwdExp, PwdMinLen, MinUppers, MinLowCase, MinDigits, MinNonAlph, NumPrevPwd, NumAuthLoc, AccLocDuration " +
                "FROM dbo.FTS11OPPA ORDER BY DocEntry", cn))
            {
                cn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    Func<int, int> n = i => r.IsDBNull(i) ? 0 : (int)Convert.ToDouble(r.GetValue(i));
                    return new PasswordPolicy
                    {
                        PwdExp = n(0), PwdMinLen = n(1), MinUppers = n(2), MinLowCase = n(3), MinDigits = n(4),
                        MinNonAlph = n(5), NumPrevPwd = n(6), NumAuthLoc = n(7), AccLocDuration = n(8)
                    };
                }
            }
        }
    }

    /// <summary>Plain copy of the Password Administration values (0 = rule off).</summary>
    public class PasswordPolicy
    {
        public int PwdExp, PwdMinLen, MinUppers, MinLowCase, MinDigits, MinNonAlph, NumPrevPwd, NumAuthLoc, AccLocDuration;

        public static PasswordPolicy From(FTS11OPPA o)
        {
            if (o == null) return null;
            return new PasswordPolicy
            {
                PwdExp = (int)o.PwdExp, PwdMinLen = (int)o.PwdMinLen, MinUppers = (int)o.MinUppers, MinLowCase = (int)o.MinLowCase,
                MinDigits = (int)o.MinDigits, MinNonAlph = (int)o.MinNonAlph, NumPrevPwd = (int)o.NumPrevPwd,
                NumAuthLoc = (int)o.NumAuthLoc, AccLocDuration = (int)o.AccLocDuration
            };
        }

        public int LockMinutes { get { return AccLocDuration > 0 ? AccLocDuration : 30; } }

        /// <summary>Returns an error message, or null when the password meets the rules.</summary>
        public string CheckComplexity(string pwd)
        {
            pwd = pwd ?? "";
            int upper = 0, lower = 0, digit = 0, other = 0;
            foreach (char c in pwd)
            {
                if (char.IsUpper(c)) upper++;
                else if (char.IsLower(c)) lower++;
                else if (char.IsDigit(c)) digit++;
                else if (!char.IsLetterOrDigit(c)) other++;
            }
            if (pwd.Length >= PwdMinLen && upper >= MinUppers && lower >= MinLowCase && digit >= MinDigits && other >= MinNonAlph)
                return null;

            var parts = new System.Collections.Generic.List<string>();
            if (PwdMinLen > 0) parts.Add("at least " + PwdMinLen + " characters");
            if (MinUppers > 0) parts.Add(MinUppers + " uppercase letter(s)");
            if (MinLowCase > 0) parts.Add(MinLowCase + " lowercase letter(s)");
            if (MinDigits > 0) parts.Add(MinDigits + " digit(s)");
            if (MinNonAlph > 0) parts.Add(MinNonAlph + " non-alphanumeric character(s)");
            return "The password must contain " + string.Join(", ", parts) + ".";
        }
    }

    /// <summary>Password history, used for "Password cannot match N previous passwords" (salted hashes only).</summary>
    [Browsable(false)]
    [XafDisplayName("Password History")]
    public class FTS11OUPH : XPLiteObject
    {
        public FTS11OUPH(Session session) : base(session) { }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private Guid _UserOid;
        [Indexed]
        public Guid UserOid
        {
            get { return _UserOid; }
            set { SetPropertyValue("UserOid", ref _UserOid, value); }
        }

        private string _PrevPwd;
        [Size(200)]
        public string PrevPwd
        {
            get { return _PrevPwd; }
            set { SetPropertyValue("PrevPwd", ref _PrevPwd, value); }
        }

        private DateTime _ChangedOn;
        [DbType("datetime")]
        public DateTime ChangedOn
        {
            get { return _ChangedOn; }
            set { SetPropertyValue("ChangedOn", ref _ChangedOn, value); }
        }
    }
}
