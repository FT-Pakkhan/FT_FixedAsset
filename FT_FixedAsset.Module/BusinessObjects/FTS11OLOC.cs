using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Location")]
    [NavigationItem("Setup")]
    [ImageName("LoadPageSetup_16x16")]
    [DefaultProperty("Code")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11OLOC : XPLiteObject
    { 
        public FTS11OLOC(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId != null && SecuritySystem.CurrentUserId.ToString() != "")
            {
                U_CreateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
                U_UpdateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
            }
            CreateDate = DateTime.Today;
            UpdateDate = DateTime.Today;
            Object = "FTS11OLOC";
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private string _Code;
        [XafDisplayName("Code"), ToolTip("Code")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save), RuleUniqueValue(DefaultContexts.Save)]
        [Size(8)]
        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue("Code", ref _Code, value); }
        }

        private string _Name;
        [XafDisplayName("Name"), ToolTip("Name")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save), RuleUniqueValue(DefaultContexts.Save)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }

        private FTS11_vw_OPRJ _U_Project;
        [XafDisplayName("Project"), ToolTip("Project")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public FTS11_vw_OPRJ U_Project
        {
            get { return _U_Project; }
            set { SetPropertyValue("U_Project", ref _U_Project, value); }
        }

        private FTS11_vw_OPRC _U_ProfitCode;
        [XafDisplayName("Cost Centre D1"), ToolTip("Cost Centre D1")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '1'")]
        public FTS11_vw_OPRC U_ProfitCode
        {
            get { return _U_ProfitCode; }
            set { SetPropertyValue("U_ProfitCode", ref _U_ProfitCode, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode2;
        [XafDisplayName("Cost Centre D2"), ToolTip("Cost Centre D2")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '2'")]
        public FTS11_vw_OPRC U_OcrCode2
        {
            get { return _U_OcrCode2; }
            set { SetPropertyValue("U_OcrCode2", ref _U_OcrCode2, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode3;
        [XafDisplayName("Cost Centre D3"), ToolTip("Cost Centre D3")]
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '3'")]
        public FTS11_vw_OPRC U_OcrCode3
        {
            get { return _U_OcrCode3; }
            set { SetPropertyValue("U_OcrCode3", ref _U_OcrCode3, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode4;
        [XafDisplayName("Cost Centre D4"), ToolTip("Cost Centre D4")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '4'")]
        public FTS11_vw_OPRC U_OcrCode4
        {
            get { return _U_OcrCode4; }
            set { SetPropertyValue("U_OcrCode4", ref _U_OcrCode4, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode5;
        [XafDisplayName("Cost Centre D5"), ToolTip("Cost Centre D5")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '5'")]
        public FTS11_vw_OPRC U_OcrCode5
        {
            get { return _U_OcrCode5; }
            set { SetPropertyValue("U_OcrCode5", ref _U_OcrCode5, value); }
        }

        private FTS11OIND _U_IndKey1;
        [XafDisplayName("Indicator 1"), ToolTip("Indicator 1")]
        [Index(90), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DataSourceCriteria("U_IndDim = '1'")]
        public FTS11OIND U_IndKey1
        {
            get { return _U_IndKey1; }
            set
            {
                SetPropertyValue("U_IndKey1", ref _U_IndKey1, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_IndCod1", ref _U_IndCod1, value.U_IndCode);
                }
            }
        }

        private string _U_IndCod1;
        [XafDisplayName("Indicator Code 1"), ToolTip("Indicator Code 1")]
        [Index(91), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod1
        {
            get { return _U_IndCod1; }
            set { SetPropertyValue("U_IndCod1", ref _U_IndCod1, value); }
        }

        private FTS11OIND _U_IndKey2;
        [XafDisplayName("Indicator 2"), ToolTip("Indicator 2")]
        [Index(100), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DataSourceCriteria("U_IndDim = '2'")]
        public FTS11OIND U_IndKey2
        {
            get { return _U_IndKey2; }
            set
            {
                SetPropertyValue("U_IndKey2", ref _U_IndKey2, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_IndCod2", ref _U_IndCod2, value.U_IndCode);
                }
            }
        }

        private string _U_IndCod2;
        [XafDisplayName("Indicator Code 2"), ToolTip("Indicator Code 2")]
        [Index(101), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod2
        {
            get { return _U_IndCod2; }
            set { SetPropertyValue("U_IndCod2", ref _U_IndCod2, value); }
        }


        private FTS11OIND _U_IndKey3;
        [XafDisplayName("Indicator 3"), ToolTip("Indicator 3")]
        [Index(110), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DataSourceCriteria("U_IndDim = '3'")]
        public FTS11OIND U_IndKey3
        {
            get { return _U_IndKey3; }
            set
            {
                SetPropertyValue("U_IndKey3", ref _U_IndKey3, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_IndCod3", ref _U_IndCod3, value.U_IndCode);
                }
            }
        }

        private string _U_IndCod3;
        [XafDisplayName("Indicator Code 3"), ToolTip("Indicator Code 3")]
        [Index(111), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod3
        {
            get { return _U_IndCod3; }
            set { SetPropertyValue("U_IndCod3", ref _U_IndCod3, value); }
        }

        private FTS11OIND _U_IndKey4;
        [XafDisplayName("Indicator 4"), ToolTip("Indicator 4")]
        [Index(120), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DataSourceCriteria("U_IndDim = '4'")]
        public FTS11OIND U_IndKey4
        {
            get { return _U_IndKey4; }
            set
            {
                SetPropertyValue("U_IndKey4", ref _U_IndKey4, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_IndCod4", ref _U_IndCod4, value.U_IndCode);
                }
            }
        }

        private string _U_IndCod4;
        [XafDisplayName("Indicator Code 4"), ToolTip("Indicator Code 4")]
        [Index(121), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod4
        {
            get { return _U_IndCod4; }
            set { SetPropertyValue("U_IndCod4", ref _U_IndCod4, value); }
        }

        private FTS11OIND _U_IndKey5;
        [XafDisplayName("Indicator 5"), ToolTip("Indicator 5")]
        [Index(130), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DataSourceCriteria("U_IndDim = '5'")]
        public FTS11OIND U_IndKey5
        {
            get { return _U_IndKey5; }
            set
            {
                SetPropertyValue("U_IndKey5", ref _U_IndKey5, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_IndCod5", ref _U_IndCod5, value.U_IndCode);
                }
            }
        }

        private string _U_IndCod5;
        [XafDisplayName("Indicator Code 5"), ToolTip("Indicator Code 5")]
        [Index(131), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod5
        {
            get { return _U_IndCod5; }
            set { SetPropertyValue("U_IndCod5", ref _U_IndCod5, value); }
        }

        private FTS11OIND _U_IndKey6;
        [XafDisplayName("Indicator 6"), ToolTip("Indicator 6")]
        [Index(140), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [DataSourceCriteria("U_IndDim = '6'")]
        public FTS11OIND U_IndKey6
        {
            get { return _U_IndKey6; }
            set
            {
                SetPropertyValue("U_IndKey6", ref _U_IndKey6, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_IndCod6", ref _U_IndCod6, value.U_IndCode);
                }
            }
        }

        private string _U_IndCod6;
        [XafDisplayName("Indicator Code 6"), ToolTip("Indicator Code 6")]
        [Index(141), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod6
        {
            get { return _U_IndCod6; }
            set { SetPropertyValue("U_IndCod6", ref _U_IndCod6, value); }
        }

        private string _U_CreateBy;
        [XafDisplayName("Create By"), ToolTip("Create By")]
        [Index(993), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_CreateBy
        {
            get { return _U_CreateBy; }
            set { SetPropertyValue("U_CreateBy", ref _U_CreateBy, value); }
        }

        private string _U_UpdateBy;
        [XafDisplayName("Update By"), ToolTip("Update By")]
        [Index(994), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_UpdateBy
        {
            get { return _U_UpdateBy; }
            set { SetPropertyValue("U_UpdateBy", ref _U_UpdateBy, value); }
        }

        private DateTime _CreateDate;
        [Index(996), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private DateTime _UpdateDate;
        [Index(997), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [XafDisplayName("Update Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { SetPropertyValue("UpdateDate", ref _UpdateDate, value); }
        }

        private string _Object;
        [XafDisplayName("Object"), ToolTip("Object")]
        [Index(999), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }

        private int _LogInst;
        [Index(1000), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [XafDisplayName("LogInst"), ToolTip("LogInst")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public int LogInst
        {
            get { return _LogInst; }
            set { SetPropertyValue("LogInst", ref _LogInst, value); }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get { return Session.IsNewObject(this); }
        }
    }
}