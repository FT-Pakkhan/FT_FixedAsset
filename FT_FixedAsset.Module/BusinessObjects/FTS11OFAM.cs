using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FileSystemData.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Fixed Asset Master")]
    [ImageName("ListBullets_16x16")]
    [DefaultProperty("U_FACode")]
    [NavigationItem("Fixed Asset")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    //[RuleCriteria("OFAM_Quantity", DefaultContexts.Save, "U_Quantity > 0", "\"Quantity\" must more than zero.")]
    [RuleCriteria("OFAM_ExSC", DefaultContexts.Save, "U_ExSC > 0", "\"Ex. Rate (SC)\" must more than zero.")]
    [RuleCriteria("OFAM_ExFC", DefaultContexts.Save, "U_ExFC > 0", "\"Ex. Rate (FC)\" must more than zero.")]
    [RuleCriteria("AcqEffDate", DefaultContexts.Save, "U_EffDate >= U_AcqDate", "\"Acquire Date\" must be earlier or equal to Effective Date")]

    // Disable to cater FOC Asset
    //[RuleCriteria("OFAM_TCostLC", DefaultContexts.Save, "U_TCostLC > 0", "\"Total Cost (LC)\" must more than zero.")]
    //[RuleCriteria("OFAM_TCostFC", DefaultContexts.Save, "U_TCostFC > 0", "\"Total Cost (FC)\" must more than zero.")]
    //[RuleCriteria("OFAM_TCostSC", DefaultContexts.Save, "U_TCostSC > 0", "\"Total Cost (SC)\" must more than zero.")]

    public class FTS11OFAM : XPLiteObject
    {
        public FTS11OFAM(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            U_Status = Session.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'O'"));
            U_AcqTypeKey = Session.FindObject<FTS11STRN>(CriteriaOperator.Parse("Code = 'T0101'"));
            U_Curr = Session.FindObject<FTS11_vw_OCRN>(CriteriaOperator.Parse("CurrCode = ?", Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_MainCur));
            U_LocationKey = Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_Location;
            U_Quantity = 1;
            U_UoM = "UNIT";
            U_ExSC = 1;
            U_ExFC = 1;

            U_FAGrpSyn = true;
            U_BarCodYN = true;

            if (SecuritySystem.CurrentUserId != null && SecuritySystem.CurrentUserId.ToString() != "")
            {
                U_CreateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
                U_UpdateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
            }
            CreateDate = DateTime.Today;
            UpdateDate = DateTime.Today;
            Object = "FTS11OFAM";
            IsFieldEditable = "Y";
            LogInst = 0;
        }

        #region Dummy field
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy01", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy01
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy02", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy02
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy03", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy03
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy04", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy04
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy05", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy05
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy06", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy06
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy07", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy07
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy08", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy08
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy09", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy09
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy10", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy10
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy11", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy11
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy12", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy12
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy13", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy13
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy14", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy14
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy15", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy15
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy16", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy16
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy17", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy17
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy18", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy18
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy19", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy19
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy20", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy20
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy21", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy21
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy22", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy22
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy23", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy23
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy24", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy24
        {
            get { return ""; }
        }
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("Dummy25", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public string Dummy25
        {
            get { return ""; }
        }
        #endregion

        #region Child Table
        [Browsable(false)]
        [NonPersistent]
        public FTS11_vw_FTS11JDT1C FTS11_vw_FTS11JDT1C { get; set; }
        public ICollection<FTS11_vw_FTS11JDT1A> FTS11_vw_FTS11JDT1A
        {
            get
            {
                if (FTS11_vw_FTS11JDT1C is null)
                    FTS11_vw_FTS11JDT1C = new FTS11_vw_FTS11JDT1C(Session);
                return FTS11_vw_FTS11JDT1C.AddLog(FTS11_vw_FTS11JDT1);
            }

        }

        [Association("FTS11OFAM-FTS11_vw_FTS11JDT1")]
        public XPCollection<FTS11_vw_FTS11JDT1> FTS11_vw_FTS11JDT1
        {
            get { return GetCollection<FTS11_vw_FTS11JDT1>("FTS11_vw_FTS11JDT1"); }
        }

        [Association("FTS11OFAM-FTS11_vw_Component")]
        public XPCollection<FTS11_vw_Component> FTS11_vw_Component
        {
            get { return GetCollection<FTS11_vw_Component>("FTS11_vw_Component"); }
        }

        [Association("FTS11OFAM-FTS11FAM2", typeof(FTS11FAM2))]
        [Appearance("FTS11FAM2", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        public XPCollection FTS11FAM2
        {
            get { return GetCollection("FTS11FAM2"); }
        }

        [Association("FTS11OFAM-FTS11FAM3", typeof(FTS11FAM3))]
        public XPCollection FTS11FAM3
        {
            get { return GetCollection("FTS11FAM3"); }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private SE_FAStatus _U_Status;
        [XafDisplayName("Status"), ToolTip("Status")]
        [NoForeignKey]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Status", Enabled = false)]
        public SE_FAStatus U_Status
        {
            get { return _U_Status; }
            set { SetPropertyValue("U_Status", ref _U_Status, value); }
        }

        private string _U_FACode;
        [XafDisplayName("F/A Code"), ToolTip("F/A Code")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("FACode", Enabled = false, Criteria = "IsFieldEditable = 'N' OR IsFACodeDisabled")]
        [RuleUniqueValue(DefaultContexts.Save)]
        public string U_FACode
        {
            get { return _U_FACode; }
            set { SetPropertyValue("U_FACode", ref _U_FACode, value); }
        }

        private string _U_FAName;
        [XafDisplayName("F/A Name"), ToolTip("F/A Name")]
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string U_FAName
        {
            get { return _U_FAName; }
            set { SetPropertyValue("U_FAName", ref _U_FAName, value); }
        }

        private FTS11OFAG _U_FAGrpKey;
        [XafDisplayName("F/A Group"), ToolTip("F/A Group")]
        [Index(40), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("U_FAGrpKey", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OFAG U_FAGrpKey
        {
            get { return _U_FAGrpKey; }
            set {
                SetPropertyValue("U_FAGrpKey", ref _U_FAGrpKey, value);
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_FAGrpCod", ref _U_FAGrpCod, value.U_FAGrpCod);
                }
                if (!IsLoading && U_FAGrpSyn != false)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_DeprnYN", ref _U_DeprnYN, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprnYN);
                        SetPropertyValue("U_DeprnBy", ref _U_DeprnBy, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprnBy);
                        SetPropertyValue("U_DeprnTyp", ref _U_DeprnTyp, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprnTyp);
                        SetPropertyValue("U_Prorate", ref _U_Prorate, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_Prorate);
                        SetPropertyValue("U_UseLife", ref _U_UseLife, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_UseLife);
                        SetPropertyValue("U_DeprPrcnt", ref _U_DeprPrcnt, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprPrcnt);
                        SetPropertyValue("U_MBaseLC", ref _U_MBaseLC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_MBaseLC);
                        SetPropertyValue("U_MBaseSC", ref _U_MBaseSC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_MBaseSC);
                        SetPropertyValue("U_AccCodA", ref _U_AccCodA, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodA);
                        SetPropertyValue("U_AccFrmA", ref _U_AccFrmA, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmA);
                        SetPropertyValue("U_AccNamA", ref _U_AccNamA, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamA);
                        SetPropertyValue("U_AccCodD", ref _U_AccCodD, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodD);
                        SetPropertyValue("U_AccFrmD", ref _U_AccFrmD, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmD);
                        SetPropertyValue("U_AccNamD", ref _U_AccNamD, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamD);
                        SetPropertyValue("U_AccCodC", ref _U_AccCodC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodC);
                        SetPropertyValue("U_AccFrmC", ref _U_AccFrmC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmC);
                        SetPropertyValue("U_AccNamC", ref _U_AccNamC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamC);
                        SetPropertyValue("U_AccCodW", ref _U_AccCodW, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodW);
                        SetPropertyValue("U_AccFrmW", ref _U_AccFrmW, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmW);
                        SetPropertyValue("U_AccNamW", ref _U_AccNamW, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamW);
                        SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodQ);
                        SetPropertyValue("U_AccFrmQ", ref _U_AccFrmQ, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmQ);
                        SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamQ);
                    }
                    else
                    {
                        SetPropertyValue("U_FAGrpCod", ref _U_FAGrpCod, "");
                    }
                }
            }
        }

        private string _U_FAGrpCod;
        [XafDisplayName("F/A Group Code"), ToolTip("F/A Group Code")]
        [Index(41), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_FAGrpCod
        {
            get { return _U_FAGrpCod; }
            set { SetPropertyValue("U_FAGrpCod", ref _U_FAGrpCod, value); }
        }

        private bool _U_FAGrpSyn;
        [Index(50), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Sync F/A Group Settings"), ToolTip("Sync F/A Group Settings")]
        [ImmediatePostData]
        public bool U_FAGrpSyn
        {
            get { return _U_FAGrpSyn; }
            set
            {
                SetPropertyValue("U_FAGrpSyn", ref _U_FAGrpSyn, value);
                if (!IsLoading && value != false && U_FAGrpKey != null)
                {
                    SetPropertyValue("U_DeprnYN", ref _U_DeprnYN, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprnYN);
                    SetPropertyValue("U_DeprnBy", ref _U_DeprnBy, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprnBy);
                    SetPropertyValue("U_DeprnTyp", ref _U_DeprnTyp, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprnTyp);
                    SetPropertyValue("U_Prorate", ref _U_Prorate, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_Prorate);
                    SetPropertyValue("U_UseLife", ref _U_UseLife, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_UseLife);
                    SetPropertyValue("U_DeprPrcnt", ref _U_DeprPrcnt, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_DeprPrcnt);
                    SetPropertyValue("U_MBaseLC", ref _U_MBaseLC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_MBaseLC);
                    SetPropertyValue("U_MBaseSC", ref _U_MBaseSC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_MBaseSC);
                    SetPropertyValue("U_AccCodA", ref _U_AccCodA, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodA);
                    SetPropertyValue("U_AccFrmA", ref _U_AccFrmA, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmA);
                    SetPropertyValue("U_AccNamA", ref _U_AccNamA, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamA);
                    SetPropertyValue("U_AccCodD", ref _U_AccCodD, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodD);
                    SetPropertyValue("U_AccFrmD", ref _U_AccFrmD, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmD);
                    SetPropertyValue("U_AccNamD", ref _U_AccNamD, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamD);
                    SetPropertyValue("U_AccCodC", ref _U_AccCodC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodC);
                    SetPropertyValue("U_AccFrmC", ref _U_AccFrmC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmC);
                    SetPropertyValue("U_AccNamC", ref _U_AccNamC, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamC);
                    SetPropertyValue("U_AccCodW", ref _U_AccCodW, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodW);
                    SetPropertyValue("U_AccFrmW", ref _U_AccFrmW, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmW);
                    SetPropertyValue("U_AccNamW", ref _U_AccNamW, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamW);
                    SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccCodQ);
                    SetPropertyValue("U_AccFrmQ", ref _U_AccFrmQ, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccFrmQ);
                    SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, Session.FindObject<FTS11OFAG>(CriteriaOperator.Parse("DocEntry=?", U_FAGrpKey.DocEntry)).U_AccNamQ);
                }
            }
        }

        private FTS11STRN _U_AcqTypeKey;
        [XafDisplayName("Acquisition Type"), ToolTip("Acquisition Type")]
        [Index(60), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("U_AcqTypeKey", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DataSourceCriteria("U_TrnsGrp = 'IN'")]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11STRN U_AcqTypeKey
        {
            get { return _U_AcqTypeKey; }
            set
            {
                SetPropertyValue("U_AcqTypeKey", ref _U_AcqTypeKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_AcqType", ref _U_AcqType, value.Code);
                    else
                        SetPropertyValue("U_AcqType", ref _U_AcqType, "");
                }
            }
        }

        private string _U_AcqType;
        [XafDisplayName("Acquisition Type Code"), ToolTip("Acquisition Type Code")]
        [Index(61), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_AcqType
        {
            get { return _U_AcqType; }
            set { SetPropertyValue("U_AcqType", ref _U_AcqType, value); }
        }

        private DateTime _U_AcqDate;
        [Index(70), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Acquire Date")]
        [ImmediatePostData]
        [Appearance("U_AcqDate", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_AcqDate
        {
            get { return _U_AcqDate; }
            set 
            { 
                SetPropertyValue("U_AcqDate", ref _U_AcqDate, value);
                SetPropertyValue("U_EffDate", ref _U_EffDate, value);
            }
        }

        private double _U_Quantity;
        [Index(80), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Quantity"), ToolTip("Quantity")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [Appearance("Quantity", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public double U_Quantity
        {
            get { return _U_Quantity; }
            set
            {
                SetPropertyValue("U_Quantity", ref _U_Quantity, value);
                if (!IsLoading)
                {
                    if (value != 0)
                    {
                        U_TCostLC = U_Quantity * U_CostLC;
                        U_TCostSC = U_Quantity * U_CostSC;
                        U_TCostFC = U_Quantity * U_CostFC;
                        U_TBaseLC = U_Quantity * U_BaseLC;
                        U_TBaseSC = U_Quantity * U_BaseSC;
                    }
                }
            }
        }

        private string _U_UoM;
        [XafDisplayName("UOM"), ToolTip("UoM")]
        [Index(90), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_UoM
        {
            get { return _U_UoM; }
            set { SetPropertyValue("U_UoM", ref _U_UoM, value); }
        }

        private FTS11OLOC _U_LocationKey;
        [XafDisplayName("Acq. Location"), ToolTip("Acq. Location")]
        [Index(100), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Location", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OLOC U_LocationKey
        {
            get { return _U_LocationKey; }
            set
            {
                SetPropertyValue("U_LocationKey", ref _U_LocationKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_Location", ref _U_Location, value.Code);
                        SetPropertyValue("U_LastLoc", ref _U_LastLoc, value.Code);
                        SetPropertyValue("U_Project", ref _U_Project, value.U_Project);
                        SetPropertyValue("U_ProfitCode", ref _U_ProfitCode, value.U_ProfitCode);
                        SetPropertyValue("U_OcrCode2", ref _U_OcrCode2, value.U_OcrCode2);
                        SetPropertyValue("U_OcrCode3", ref _U_OcrCode3, value.U_OcrCode3);
                        SetPropertyValue("U_OcrCode4", ref _U_OcrCode4, value.U_OcrCode4);
                        SetPropertyValue("U_OcrCode5", ref _U_OcrCode5, value.U_OcrCode5);
                        SetPropertyValue("U_IndKey1", ref _U_IndKey1, value.U_IndKey1);
                        SetPropertyValue("U_IndKey2", ref _U_IndKey2, value.U_IndKey2);
                        SetPropertyValue("U_IndKey3", ref _U_IndKey3, value.U_IndKey3);
                        SetPropertyValue("U_IndKey4", ref _U_IndKey4, value.U_IndKey4);
                        SetPropertyValue("U_IndKey5", ref _U_IndKey5, value.U_IndKey5);
                        SetPropertyValue("U_IndKey6", ref _U_IndKey6, value.U_IndKey6);
                    }
                    else
                    {
                        SetPropertyValue("U_Location", ref _U_Location, "");
                        SetPropertyValue("U_LastLoc", ref _U_LastLoc, "");
                        SetPropertyValue("U_Project", ref _U_Project, null);
                        SetPropertyValue("U_ProfitCode", ref _U_ProfitCode, null);
                        SetPropertyValue("U_OcrCode2", ref _U_OcrCode2, null);
                        SetPropertyValue("U_OcrCode3", ref _U_OcrCode3, null);
                        SetPropertyValue("U_OcrCode4", ref _U_OcrCode4, null);
                        SetPropertyValue("U_OcrCode5", ref _U_OcrCode5, null);
                        SetPropertyValue("U_IndKey1", ref _U_IndKey1, null);
                        SetPropertyValue("U_IndKey2", ref _U_IndKey2, null);
                        SetPropertyValue("U_IndKey3", ref _U_IndKey3, null);
                        SetPropertyValue("U_IndKey4", ref _U_IndKey4, null);
                        SetPropertyValue("U_IndKey5", ref _U_IndKey5, null);
                        SetPropertyValue("U_IndKey6", ref _U_IndKey6, null);
                    }
                }
            }
        }

        private string _U_Location;
        [XafDisplayName("Location Code"), ToolTip("Location Code")]
        [Index(101), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_Location
        {
            get { return _U_Location; }
            set { SetPropertyValue("U_Location", ref _U_Location, value); }
        }

        private FTS11_vw_OCRN _U_Curr;
        [XafDisplayName("Currency"), ToolTip("Currency")]
        [Index(110), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("Curr", Enabled = false)]
        [NoForeignKey]
        public FTS11_vw_OCRN U_Curr
        {
            get { return _U_Curr; }
            set { SetPropertyValue("U_Curr", ref _U_Curr, value); }
        }

        private double _U_OriCost;
        [XafDisplayName("Original Cost"), ToolTip("Original Cost")]
        [Index(120), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_OriCost
        {
            get { return _U_OriCost; }
            set { SetPropertyValue("U_OriCost", ref _U_OriCost, value); }
        }

        private double _U_OriCostSC;
        [XafDisplayName("Original Cost (SC)"), ToolTip("Original Cost (SC)")]
        [Index(121), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_OriCostSC
        {
            get { return _U_OriCostSC; }
            set { SetPropertyValue("U_OriCostSC", ref _U_OriCostSC, value); }
        }

        private double _U_Revalue;
        [XafDisplayName("Residual Value"), ToolTip("Residual Value")]
        [Index(122), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_Revalue", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_Revalue
        {
            get { return _U_Revalue; }
            set { SetPropertyValue("U_Revalue", ref _U_Revalue, value); }
        }

        private double _U_RevalueSC;
        [XafDisplayName("Residual Value (SC)"), ToolTip("Residual Value (SC)")]
        [Index(123), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_RevalueSC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_RevalueSC
        {
            get { return _U_RevalueSC; }
            set { SetPropertyValue("U_RevalueSC", ref _U_RevalueSC, value); }
        }

        private string _U_AccCodV;
        [XafDisplayName("Acquire Frm A/C"), ToolTip("Acquire Frm A/C")]
        [Index(130), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_AccCodV
        {
            get { return _U_AccCodV; }
            set { SetPropertyValue("U_AccCodV", ref _U_AccCodV, value); }
        }

        private FTS11_vw_OACT_OCRD _U_AccFrmV;
        [XafDisplayName("Acquire Frm A/C Code"), ToolTip("Acquire Frm A/C Code")]
        [Index(131), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT_OCRD U_AccFrmV
        {
            get { return _U_AccFrmV; }
            set
            {
                SetPropertyValue("U_AccFrmV", ref _U_AccFrmV, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodV", ref _U_AccCodV, value.AcctCode);
                        SetPropertyValue("U_AccNamV", ref _U_AccNamV, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodV", ref _U_AccCodV, "");
                        SetPropertyValue("U_AccNamV", ref _U_AccNamV, "");
                    }
                }
            }
        }

        private string _U_AccNamV;
        [XafDisplayName("Acquire Frm A/C Name"), ToolTip("Acquire Frm A/C Name")]
        [Index(132), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_AccNamV
        {
            get { return _U_AccNamV; }
            set { SetPropertyValue("U_AccNamV", ref _U_AccNamV, value); }
        }

        private double _U_CostLC;
        [XafDisplayName("Unit Cost (LC)"), ToolTip("Unit Cost (LC)")]
        [Index(140), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_CostLC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public double U_CostLC
        {
            get { return _U_CostLC; }
            set
            {
                SetPropertyValue("U_CostLC", ref _U_CostLC, value);
                if (!IsLoading)
                {
                    if (value != 0)
                    {
                        U_TCostLC = U_Quantity * U_CostLC;
                        U_CostFC = U_CostLC / double.Parse(U_ExFC.ToString());
                        U_CostSC = U_CostFC * double.Parse(U_ExSC.ToString());
                        U_BaseLC = U_CostLC;
                    }
                    else
                    {
                        U_TCostLC = 0;
                        U_CostFC = 0;
                        U_CostSC = 0;
                        U_BaseLC = 0;
                    }
                    
                }
            }
        }

        private double _U_TCostLC;
        [XafDisplayName("Total Cost (LC)"), ToolTip("Total Cost (LC)")]
        [Index(141), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_TCostLC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_TCostLC
        {
            get { return _U_TCostLC; }
            set { SetPropertyValue("U_TCostLC", ref _U_TCostLC, value); }
        }

        private double _U_CostSC;
        [XafDisplayName("Unit Cost (SC)"), ToolTip("Unit Cost (SC)")]
        [Index(150), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_CostSC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_CostSC
        {
            get { return _U_CostSC; }
            set
            {
                SetPropertyValue("U_CostSC", ref _U_CostSC, value);
                if (!IsLoading)
                {
                    if (value != 0)
                    {
                        U_TCostSC = U_Quantity * U_CostSC;
                        U_BaseSC = U_CostSC;
                    }
                    else
                    {
                        U_TCostSC = 0;
                        U_BaseSC = 0;
                    }
                }
            }
        }

        private double _U_TCostSC;
        [XafDisplayName("Total Cost (SC)"), ToolTip("Total Cost (SC)")]
        [Index(151), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_TCostSC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_TCostSC
        {
            get { return _U_TCostSC; }
            set { SetPropertyValue("U_TCostSC", ref _U_TCostSC, value); }
        }

        private double _U_CostFC;
        [XafDisplayName("Unit Cost (FC)"), ToolTip("Unit Cost (FC)")]
        [Index(150), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_CostFC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_CostFC
        {
            get { return _U_CostFC; }
            set
            {
                SetPropertyValue("U_CostFC", ref _U_CostFC, value);
                if (!IsLoading)
                {
                    if (value != 0)
                        U_TCostFC = U_Quantity * U_CostFC;
                    else
                        U_TCostFC = 0;
                }
            }
        }

        private double _U_TCostFC;
        [XafDisplayName("Total Cost (FC)"), ToolTip("Total Cost (FC)")]
        [Index(151), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_TCostFC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_TCostFC
        {
            get { return _U_TCostFC; }
            set { SetPropertyValue("U_TCostFC", ref _U_TCostFC, value); }
        }

        private decimal _U_ExSC;
        [XafDisplayName("Ex. Rate (SC)"), ToolTip("Ex. Rate (SC)")]
        [Index(160), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n6}")]
        [Appearance("U_ExSC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public decimal U_ExSC
        {
            get { return _U_ExSC; }
            set { SetPropertyValue("U_ExSC", ref _U_ExSC, value); }
        }

        private decimal _U_ExFC;
        [XafDisplayName("Ex. Rate (FC)"), ToolTip("Ex. Rate (SC)")]
        [Index(170), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n6}")]
        [Appearance("U_ExFC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public decimal U_ExFC
        {
            get { return _U_ExFC; }
            set { SetPropertyValue("U_ExFC", ref _U_ExFC, value); }
        }

        private double _U_BaseLC;
        [XafDisplayName("Unit B/F NBV (LC)"), ToolTip("Unit B/F NBV (LC)")]
        [Index(180), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ImmediatePostData]
        [Appearance("U_BaseLC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_BaseLC
        {
            get { return _U_BaseLC; }
            set
            {
                SetPropertyValue("U_BaseLC", ref _U_BaseLC, value);
                if (!IsLoading)
                {
                    if (value != 0)
                    {
                        U_TBaseLC = U_Quantity * U_BaseLC;
                        U_BaseSC = (U_BaseLC / double.Parse(U_ExFC.ToString())) * double.Parse(U_ExSC.ToString());


                        //It will prompt to redirect out when user didnt select FA group first due to FA Group settings useful life not sync, so cannot divide by 0
                        U_BFDepr = Math.Round(((U_TCostLC - U_TBaseLC) / (U_TCostLC / U_UseLife)), 0);
                    }
                    else
                    {
                        U_TBaseLC = 0;
                        U_BaseSC = 0;
                        U_BFDepr = 0;
                    }
                }
            }
        }

        private double _U_TBaseLC;
        [XafDisplayName("Total B/F NBV (LC)"), ToolTip("Total B/F NBV (LC)")]
        [Index(181), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_TBaseLC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_TBaseLC
        {
            get { return _U_TBaseLC; }
            set { SetPropertyValue("U_TBaseLC", ref _U_TBaseLC, value); }
        }

        private double _U_BaseSC;
        [XafDisplayName("Unit B/F NBV (SC)"), ToolTip("Unit B/F NBV (SC)")]
        [Index(180), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ImmediatePostData]
        [Appearance("U_BaseSC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_BaseSC
        {
            get { return _U_BaseSC; }
            set
            {
                SetPropertyValue("U_BaseSC", ref _U_BaseSC, value);
                if (!IsLoading)
                {
                    if (value != 0)
                        U_TBaseSC = U_Quantity * U_BaseSC;
                    else
                        U_TBaseSC = 0;
                }
            }
        }

        private double _U_TBaseSC;
        [XafDisplayName("Total B/F NBV (SC)"), ToolTip("Total B/F NBV (SC)")]
        [Index(181), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("U_TBaseSC", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        [DbType("numeric(19,6)")]
        public double U_TBaseSC
        {
            get { return _U_TBaseSC; }
            set { SetPropertyValue("U_TBaseSC", ref _U_TBaseSC, value); }
        }

        private double _U_BFDepr;
        [Index(190), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("B/F Depr Life"), ToolTip("B/F Depr Life")]
        [Appearance("BFDepr", Enabled = false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_BFDepr
        {
            get { return _U_BFDepr; }
            set { SetPropertyValue("U_BFDepr", ref _U_BFDepr, value); }
        }

        private double _U_DeprnCnt;
        [Index(191), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [XafDisplayName("Depr Count"), ToolTip("Depr Count")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        [DbType("numeric(19,6)")]
        public double U_DeprnCnt
        {
            get { return _U_DeprnCnt; }
            set { SetPropertyValue("U_DeprnCnt", ref _U_DeprnCnt, value); }
        }

        private DateTime _U_EffDate;
        [Index(200), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Effective Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [Appearance("U_EffDate", Enabled = false, Criteria = "IsFieldEditable = 'N'")]
        public DateTime U_EffDate
        {
            get { return _U_EffDate; }
            set { SetPropertyValue("U_EffDate", ref _U_EffDate, value); }
        }

        private bool _U_DeprnYN;
        [Index(210), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Depreciation")]
        [Appearance("DeprnYN", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public bool U_DeprnYN
        {
            get { return _U_DeprnYN; }
            set { SetPropertyValue("U_DeprnYN", ref _U_DeprnYN, value); }
        }

        private SE_FADeprnBy _U_DeprnBy;
        [XafDisplayName("Depreciate By"), ToolTip("Depreciate By")]
        [NoForeignKey]
        [ImmediatePostData]
        [Index(220), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("DeprnBy", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FADeprnBy U_DeprnBy
        {
            get { return _U_DeprnBy; }
            set
            {
                SetPropertyValue("U_DeprnBy", ref _U_DeprnBy, value);
                if (!IsLoading & value != null)
                {
                    if (value != Session.FindObject<SE_FADeprnBy>(CriteriaOperator.Parse("Code = 'U'")))
                    {
                        U_UseLife = 0;
                    }
                    else
                    {
                        U_DeprPrcnt = 0;
                    }
                }
            }
        }

        private SE_FADeprnTyp _U_DeprnTyp;
        [XafDisplayName("Depreciate Type"), ToolTip("Depreciate Type")]
        [NoForeignKey]
        [ImmediatePostData]
        [Index(240), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("DeprnTyp", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FADeprnTyp U_DeprnTyp
        {
            get { return _U_DeprnTyp; }
            set { SetPropertyValue("U_DeprnTyp", ref _U_DeprnTyp, value); }
        }

        private SE_FAProrate _U_Prorate;
        [XafDisplayName("Prorate"), ToolTip("Prorate")]
        [NoForeignKey]
        [Appearance("Prorate", Enabled = false, Criteria = "[U_DeprnTyp].[Code] <> 'S' OR U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [Index(250), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FAProrate U_Prorate
        {
            get { return _U_Prorate; }
            set { SetPropertyValue("U_Prorate", ref _U_Prorate, value); }
        }

        private double _U_UseLife;
        [Index(260), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Useful Life (Months)"), ToolTip("Useful Life (Months)")]
        [Appearance("UseLife", Enabled = false, Criteria = "[U_DeprnBy].[Code] <> 'U' OR U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public double U_UseLife
        {
            get { return _U_UseLife; }
            set
            {
                SetPropertyValue("U_UseLife", ref _U_UseLife, value);
                if (!IsLoading && U_DeprnBy.Code == "U")
                {
                    if (value != 0)
                        U_DeprPrcnt = Math.Round(12 / U_UseLife * 100, 6);
                    else
                        U_DeprPrcnt = 0;
                }
            }
        }

        private double _U_DeprPrcnt;
        [Index(270), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Percentage %"), ToolTip("Precentage %")]
        [Appearance("DeprPrcnt", Enabled = false, Criteria = "[U_DeprnBy].[Code] <> 'P' OR U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [ImmediatePostData]
        [DbType("numeric(19,6)")]
        public double U_DeprPrcnt
        {
            get { return _U_DeprPrcnt; }
            set
            {
                SetPropertyValue("U_DeprPrcnt", ref _U_DeprPrcnt, value);
                if (!IsLoading && U_DeprnBy.Code == "P")
                {
                    if (value != 0)
                        U_UseLife = Math.Round(12 / U_DeprPrcnt * 100, 6);
                    else
                        U_UseLife = 0;
                }
            }
        }

        private double _U_MBaseLC;
        [XafDisplayName("Min NBV (LC)"), ToolTip("Min NBV (LC)")]
        [Index(280), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("MBaseLC", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_MBaseLC
        {
            get { return _U_MBaseLC; }
            set { SetPropertyValue("U_MBaseLC", ref _U_MBaseLC, value); }
        }

        private double _U_MBaseSC;
        [XafDisplayName("Min NBV (SC)"), ToolTip("Min NBV (SC)")]
        [Index(290), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("MBaseSC", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_MBaseSC
        {
            get { return _U_MBaseSC; }
            set { SetPropertyValue("U_MBaseSC", ref _U_MBaseSC, value); }
        }

        private string _U_AccCodA;
        [XafDisplayName("Asset A/C"), ToolTip("Asset A/C")]
        [Index(300), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [Appearance("AccCodA", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccCodA
        {
            get { return _U_AccCodA; }
            set { SetPropertyValue("U_AccCodA", ref _U_AccCodA, value); }
        }

        private FTS11_vw_OACT _U_AccFrmA;
        [XafDisplayName("Asset A/C Code"), ToolTip("Asset A/C Code")]
        [Index(301), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Appearance("AccFrmA", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmA
        {
            get { return _U_AccFrmA; }
            set
            {
                SetPropertyValue("U_AccFrmA", ref _U_AccFrmA, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodA", ref _U_AccCodA, value.AcctCode);
                        SetPropertyValue("U_AccNamA", ref _U_AccNamA, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodA", ref _U_AccCodA, "");
                        SetPropertyValue("U_AccNamA", ref _U_AccNamA, "");
                    }
                }
            }
        }

        private string _U_AccNamA;
        [XafDisplayName("Asset A/C Name"), ToolTip("Asset A/C Name")]
        [Index(302), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccNamA", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccNamA
        {
            get { return _U_AccNamA; }
            set { SetPropertyValue("U_AccNamA", ref _U_AccNamA, value); }
        }

        private string _U_AccCodD;
        [XafDisplayName("Depreciation A/C"), ToolTip("Depreciation A/C")]
        [Index(310), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [Appearance("AccCodD", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccCodD
        {
            get { return _U_AccCodD; }
            set { SetPropertyValue("AccCodD", ref _U_AccCodD, value); }
        }

        private FTS11_vw_OACT _U_AccFrmD;
        [XafDisplayName("Depreciation A/C Code"), ToolTip("Depreciation A/C Code")]
        [Index(311), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "U_DeprnYN = true")]
        [Appearance("AccFrmD", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmD
        {
            get { return _U_AccFrmD; }
            set
            {
                SetPropertyValue("U_AccFrmD", ref _U_AccFrmD, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodD", ref _U_AccCodD, value.AcctCode);
                        SetPropertyValue("U_AccNamD", ref _U_AccNamD, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodD", ref _U_AccCodD, "");
                        SetPropertyValue("U_AccNamD", ref _U_AccNamD, "");
                    }
                }
            }
        }

        private string _U_AccNamD;
        [XafDisplayName("Depreciation A/C Name"), ToolTip("Depreciation A/C Name")]
        [Index(312), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccNamD", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccNamD
        {
            get { return _U_AccNamD; }
            set { SetPropertyValue("U_AccNamD", ref _U_AccNamD, value); }
        }

        private string _U_AccCodC;
        [XafDisplayName("Acc. Deprn A/C"), ToolTip("Acc. Deprn A/C")]
        [Index(320), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [Appearance("AccCodC", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccCodC
        {
            get { return _U_AccCodC; }
            set { SetPropertyValue("U_AccCodC", ref _U_AccCodC, value); }
        }

        private FTS11_vw_OACT _U_AccFrmC;
        [XafDisplayName("Acc. Deprn A/C Code"), ToolTip("Acc. Deprn A/C Code")]
        [Index(321), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "U_DeprnYN = true")]
        [Appearance("AccFrmC", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmC
        {
            get { return _U_AccFrmC; }
            set
            {
                SetPropertyValue("U_AccFrmC", ref _U_AccFrmC, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodC", ref _U_AccCodC, value.AcctCode);
                        SetPropertyValue("U_AccNamC", ref _U_AccNamC, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodC", ref _U_AccCodC, "");
                        SetPropertyValue("U_AccNamC", ref _U_AccNamC, "");
                    }
                }
            }
        }

        private string _U_AccNamC;
        [XafDisplayName("Acc. Deprn A/C Name"), ToolTip("Acc. Deprn A/C Name")]
        [Index(322), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccNamC", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccNamC
        {
            get { return _U_AccNamC; }
            set { SetPropertyValue("U_AccNamC", ref _U_AccNamC, value); }
        }

        private string _U_AccCodW;
        [XafDisplayName("Disposal A/C"), ToolTip("Disposal A/C")]
        [Index(330), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [Appearance("AccCodW", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccCodW
        {
            get { return _U_AccCodW; }
            set { SetPropertyValue("U_AccCodW", ref _U_AccCodW, value); }
        }

        private FTS11_vw_OACT _U_AccFrmW;
        [XafDisplayName("Disposal A/C Code"), ToolTip("Disposal A/C Code")]
        [Index(331), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccFrmW", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmW
        {
            get { return _U_AccFrmW; }
            set
            {
                SetPropertyValue("U_AccFrmW", ref _U_AccFrmW, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodW", ref _U_AccCodW, value.AcctCode);
                        SetPropertyValue("U_AccNamW", ref _U_AccNamW, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodW", ref _U_AccCodW, "");
                        SetPropertyValue("U_AccNamW", ref _U_AccNamW, "");
                    }
                }
            }
        }

        private string _U_AccNamW;
        [XafDisplayName("Disposal A/C Name"), ToolTip("Disposal A/C Name")]
        [Index(332), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccNamW", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccNamW
        {
            get { return _U_AccNamW; }
            set { SetPropertyValue("U_AccNamW", ref _U_AccNamW, value); }
        }

        private string _U_AccCodQ;
        [XafDisplayName("Asset Offset A/C"), ToolTip("Asset Offset A/C")]
        [Index(340), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [Appearance("AccCodQ", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccCodQ
        {
            get { return _U_AccCodQ; }
            set { SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, value); }
        }

        private FTS11_vw_OACT _U_AccFrmQ;
        [XafDisplayName("Asset Offset A/C Code"), ToolTip("Asset Offset A/C Code")]
        [Index(341), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccFrmQ", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrmQ
        {
            get { return _U_AccFrmQ; }
            set
            {
                SetPropertyValue("U_AccFrmQ", ref _U_AccFrmQ, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, value.AcctCode);
                        SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, value.AcctName);
                    }
                    else
                    {
                        SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, "");
                        SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, "");
                    }
                }
            }
        }

        private string _U_AccNamQ;
        [XafDisplayName("Asset Offset A/C Name"), ToolTip("Asset Offset A/C Name")]
        [Index(342), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("AccNamQ", Enabled = false, Criteria = "U_FAGrpSyn = true OR IsFieldEditable = 'N'")]
        public string U_AccNamQ
        {
            get { return _U_AccNamQ; }
            set { SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, value); }
        }

        private FTS11_vw_OPRJ _U_Project;
        [XafDisplayName("Project"), ToolTip("Project")]
        [Index(350), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public FTS11_vw_OPRJ U_Project
        {
            get { return _U_Project; }
            set { SetPropertyValue("U_Project", ref _U_Project, value); }
        }

        private FTS11_vw_OPRC _U_ProfitCode;
        [XafDisplayName("Cost Centre D1"), ToolTip("Cost Centre D1")]
        [Index(360), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '1'")]
        public FTS11_vw_OPRC U_ProfitCode
        {
            get { return _U_ProfitCode; }
            set { SetPropertyValue("U_ProfitCode", ref _U_ProfitCode, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode2;
        [XafDisplayName("Cost Centre D2"), ToolTip("Cost Centre D2")]
        [Index(370), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '2'")]
        public FTS11_vw_OPRC U_OcrCode2
        {
            get { return _U_OcrCode2; }
            set { SetPropertyValue("U_OcrCode2", ref _U_OcrCode2, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode3;
        [XafDisplayName("Cost Centre D3"), ToolTip("Cost Centre D3")]
        [Index(380), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '3'")]
        public FTS11_vw_OPRC U_OcrCode3
        {
            get { return _U_OcrCode3; }
            set { SetPropertyValue("U_OcrCode3", ref _U_OcrCode3, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode4;
        [XafDisplayName("Cost Centre D4"), ToolTip("Cost Centre D4")]
        [Index(390), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '4'")]
        public FTS11_vw_OPRC U_OcrCode4
        {
            get { return _U_OcrCode4; }
            set { SetPropertyValue("U_OcrCode4", ref _U_OcrCode4, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode5;
        [XafDisplayName("Cost Centre D5"), ToolTip("Cost Centre D5")]
        [Index(400), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '5'")]
        public FTS11_vw_OPRC U_OcrCode5
        {
            get { return _U_OcrCode5; }
            set { SetPropertyValue("U_OcrCode5", ref _U_OcrCode5, value); }
        }

        private FTS11OIND _U_IndKey1;
        [XafDisplayName("Indicator 1"), ToolTip("Indicator 1")]
        [Index(410), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '1'")]
        public FTS11OIND U_IndKey1
        {
            get { return _U_IndKey1; }
            set
            {
                SetPropertyValue("U_IndKey1", ref _U_IndKey1, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_IndCod1", ref _U_IndCod1, value.U_IndCode);
                    else
                        SetPropertyValue("U_IndCod1", ref _U_IndCod1, "");
                }
            }
        }

        private string _U_IndCod1;
        [XafDisplayName("Indicator Code 1"), ToolTip("Indicator Code 1")]
        [Index(411), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_IndCod1
        {
            get { return _U_IndCod1; }
            set { SetPropertyValue("U_IndCod1", ref _U_IndCod1, value); }
        }

        private FTS11OIND _U_IndKey2;
        [XafDisplayName("Indicator 2"), ToolTip("Indicator 2")]
        [Index(420), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '2'")]
        public FTS11OIND U_IndKey2
        {
            get { return _U_IndKey2; }
            set
            {
                SetPropertyValue("U_IndKey2", ref _U_IndKey2, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_IndCod2", ref _U_IndCod2, value.U_IndCode);
                    else
                        SetPropertyValue("U_IndCod2", ref _U_IndCod2, "");
                }
            }
        }

        private string _U_IndCod2;
        [XafDisplayName("Indicator Code 2"), ToolTip("Indicator Code 2")]
        [Index(421), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_IndCod2
        {
            get { return _U_IndCod2; }
            set { SetPropertyValue("U_IndCod2", ref _U_IndCod2, value); }
        }


        private FTS11OIND _U_IndKey3;
        [XafDisplayName("Indicator 3"), ToolTip("Indicator 3")]
        [Index(420), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '3'")]
        public FTS11OIND U_IndKey3
        {
            get { return _U_IndKey3; }
            set
            {
                SetPropertyValue("U_IndKey3", ref _U_IndKey3, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_IndCod3", ref _U_IndCod3, value.U_IndCode);
                    else
                        SetPropertyValue("U_IndCod3", ref _U_IndCod3, "");
                }
            }
        }

        private string _U_IndCod3;
        [XafDisplayName("Indicator Code 3"), ToolTip("Indicator Code 3")]
        [Index(421), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_IndCod3
        {
            get { return _U_IndCod3; }
            set { SetPropertyValue("U_IndCod3", ref _U_IndCod3, value); }
        }

        private FTS11OIND _U_IndKey4;
        [XafDisplayName("Indicator 4"), ToolTip("Indicator 4")]
        [Index(430), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '4'")]
        public FTS11OIND U_IndKey4
        {
            get { return _U_IndKey4; }
            set
            {
                SetPropertyValue("U_IndKey4", ref _U_IndKey4, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_IndCod4", ref _U_IndCod4, value.U_IndCode);
                    else
                        SetPropertyValue("U_IndCod4", ref _U_IndCod4, "");
                }
            }
        }

        private string _U_IndCod4;
        [XafDisplayName("Indicator Code 4"), ToolTip("Indicator Code 4")]
        [Index(431), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_IndCod4
        {
            get { return _U_IndCod4; }
            set { SetPropertyValue("U_IndCod4", ref _U_IndCod4, value); }
        }

        private FTS11OIND _U_IndKey5;
        [XafDisplayName("Indicator 5"), ToolTip("Indicator 5")]
        [Index(440), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '5'")]
        public FTS11OIND U_IndKey5
        {
            get { return _U_IndKey5; }
            set
            {
                SetPropertyValue("U_IndKey5", ref _U_IndKey5, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_IndCod5", ref _U_IndCod5, value.U_IndCode);
                    else
                        SetPropertyValue("U_IndCod5", ref _U_IndCod5, "");
                }
            }
        }

        private string _U_IndCod5;
        [XafDisplayName("Indicator Code 5"), ToolTip("Indicator Code 5")]
        [Index(441), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_IndCod5
        {
            get { return _U_IndCod5; }
            set { SetPropertyValue("U_IndCod5", ref _U_IndCod5, value); }
        }

        private FTS11OIND _U_IndKey6;
        [XafDisplayName("Indicator 6"), ToolTip("Indicator 6")]
        [Index(450), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '6'")]
        public FTS11OIND U_IndKey6
        {
            get { return _U_IndKey6; }
            set
            {
                SetPropertyValue("U_IndKey6", ref _U_IndKey6, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_IndCod6", ref _U_IndCod6, value.U_IndCode);
                    else
                        SetPropertyValue("U_IndCod6", ref _U_IndCod6, "");
                }
            }
        }

        private string _U_IndCod6;
        [XafDisplayName("Indicator Code 6"), ToolTip("Indicator Code 6")]
        [Index(451), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_IndCod6
        {
            get { return _U_IndCod6; }
            set { SetPropertyValue("U_IndCod6", ref _U_IndCod6, value); }
        }

        private string _U_BarCode;
        [XafDisplayName("Barcode"), ToolTip("Barcode")]
        [Index(460), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_BarCode
        {
            get { return _U_BarCode; }
            set { SetPropertyValue("U_IndCod6", ref _U_BarCode, value); }
        }

        private bool _U_BarCodYN;
        [Index(461), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Barcode Printing")]
        public bool U_BarCodYN
        {
            get { return _U_BarCodYN; }
            set { SetPropertyValue("U_BarCodYN", ref _U_BarCodYN, value); }
        }

        private string _U_TagNum;
        [XafDisplayName("Tag No."), ToolTip("Tag No.")]
        [Index(470), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_TagNum
        {
            get { return _U_TagNum; }
            set { SetPropertyValue("U_TagNum", ref _U_TagNum, value); }
        }

        private string _U_Legacy;
        [XafDisplayName("Legacy Code"), ToolTip("Legacy Code")]
        [Index(480), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Legacy
        {
            get { return _U_Legacy; }
            set { SetPropertyValue("U_Legacy", ref _U_Legacy, value); }
        }

        private string _U_Serial;
        [XafDisplayName("Serial"), ToolTip("Serial")]
        [Index(490), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Serial
        {
            get { return _U_Serial; }
            set { SetPropertyValue("U_Serial", ref _U_Serial, value); }
        }

        private string _U_Model;
        [XafDisplayName("Model"), ToolTip("Model")]
        [Index(500), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Model
        {
            get { return _U_Model; }
            set { SetPropertyValue("U_Model", ref _U_Model, value); }
        }

        private DateTime _U_WarExpr;
        [Index(510), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Warranty Expiry")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_WarExpr
        {
            get { return _U_WarExpr; }
            set { SetPropertyValue("U_WarExpr", ref _U_WarExpr, value); }
        }

        private string _U_InsNum;
        [XafDisplayName("Insurance Ref."), ToolTip("Insurance Ref.")]
        [Index(520), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_InsNum
        {
            get { return _U_InsNum; }
            set { SetPropertyValue("U_InsNum", ref _U_InsNum, value); }
        }

        private DateTime _U_InsDate;
        [Index(521), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Insurance Ref. Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_InsDate
        {
            get { return _U_InsDate; }
            set { SetPropertyValue("U_InsDate", ref _U_InsDate, value); }
        }

        private string _U_InsType;
        [XafDisplayName("Insurance Type"), ToolTip("Insurance Type")]
        [Index(522), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_InsType
        {
            get { return _U_InsType; }
            set { SetPropertyValue("U_InsType", ref _U_InsType, value); }
        }

        private string _U_InsComp;
        [XafDisplayName("Insurance Company"), ToolTip("Insurance Company")]
        [Index(523), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_InsComp
        {
            get { return _U_InsComp; }
            set { SetPropertyValue("U_InsComp", ref _U_InsComp, value); }
        }

        private double _U_InsCov;
        [XafDisplayName("Coverage"), ToolTip("Coverage")]
        [Index(524), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_InsCov
        {
            get { return _U_InsCov; }
            set { SetPropertyValue("U_InsCov", ref _U_InsCov, value); }
        }

        private double _U_InsVal;
        [XafDisplayName("Value"), ToolTip("Value")]
        [Index(525), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_InsVal
        {
            get { return _U_InsVal; }
            set { SetPropertyValue("U_InsVal", ref _U_InsVal, value); }
        }

        private string _U_LeasRef;
        [XafDisplayName("Lease Ref."), ToolTip("Lease Ref.")]
        [Index(530), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_LeasRef
        {
            get { return _U_LeasRef; }
            set { SetPropertyValue("U_LeasRef", ref _U_LeasRef, value); }
        }

        private DateTime _U_LeasDate;
        [Index(531), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Lease Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_LeasDate
        {
            get { return _U_LeasDate; }
            set { SetPropertyValue("U_LeasDate", ref _U_LeasDate, value); }
        }

        private string _U_RefDoc1;
        //[XafDisplayName("Ref. Doc. 1"), ToolTip("Ref Doc. 1")]
        [XafDisplayName("Invoice No."), ToolTip("Invoice No.")]
        [Index(540), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_RefDoc1
        {
            get { return _U_RefDoc1; }
            set { SetPropertyValue("U_RefDoc1", ref _U_RefDoc1, value); }
        }

        private DateTime _U_RefDate1;
        [Index(541), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        //[XafDisplayName("Ref. Date 1"), ToolTip("Ref. Date 1")]
        [XafDisplayName("Invoice Date"), ToolTip("Invoice Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_RefDate1
        {
            get { return _U_RefDate1; }
            set { SetPropertyValue("U_RefDate1", ref _U_RefDate1, value); }
        }

        private string _U_RefDoc2;
        [XafDisplayName("Ref. Doc. 2"), ToolTip("Ref Doc. 2")]
        [Index(550), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_RefDoc2
        {
            get { return _U_RefDoc2; }
            set { SetPropertyValue("U_RefDoc2", ref _U_RefDoc2, value); }
        }

        private DateTime _U_RefDate2;
        [Index(551), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Ref. Date 2")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_RefDate2
        {
            get { return _U_RefDate2; }
            set { SetPropertyValue("U_RefDate2", ref _U_RefDate2, value); }
        }

        private string _U_RefDoc3;
        [XafDisplayName("Ref. Doc. 3"), ToolTip("Ref Doc. 3")]
        [Index(560), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_RefDoc3
        {
            get { return _U_RefDoc3; }
            set { SetPropertyValue("U_RefDoc3", ref _U_RefDoc3, value); }
        }

        private DateTime _U_RefDate3;
        [Index(561), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Ref. Date 3")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime U_RefDate3
        {
            get { return _U_RefDate3; }
            set { SetPropertyValue("U_RefDate3", ref _U_RefDate3, value); }
        }

        private string _U_Remark1;
        //[XafDisplayName("Remark 1"), ToolTip("Remark 1")]
        [XafDisplayName("Asset Owner"), ToolTip("Asset Owner")]
        [Size(255)]
        [Index(570), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Remark1
        {
            get { return _U_Remark1; }
            set { SetPropertyValue("U_Remark1", ref _U_Remark1, value); }
        }

        private string _U_Remark2;
        [XafDisplayName("Remark 2"), ToolTip("Remark 2")]
        [Size(255)]
        [Index(571), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Remark2
        {
            get { return _U_Remark2; }
            set { SetPropertyValue("U_Remark2", ref _U_Remark2, value); }
        }

        private string _U_Remark3;
        [XafDisplayName("Remark 3"), ToolTip("Remark 3")]
        [Size(255)]
        [Index(572), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Remark3
        {
            get { return _U_Remark3; }
            set { SetPropertyValue("U_Remark3", ref _U_Remark3, value); }
        }

        private string _U_Remark4;
        [XafDisplayName("Remark 4"), ToolTip("Remark 4")]
        [Size(255)]
        [Index(573), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Remark4
        {
            get { return _U_Remark4; }
            set { SetPropertyValue("U_Remark4", ref _U_Remark4, value); }
        }

        private string _U_Text;
        [XafDisplayName("Note"), ToolTip("Note")]
        [Size(SizeAttribute.Unlimited)]
        [Index(574), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_Text
        {
            get { return _U_Text; }
            set { SetPropertyValue("U_Text", ref _U_Text, value); }
        }

        [DevExpress.ExpressApp.DC.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        [Index(575), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [XafDisplayName("Attachment"), ToolTip("Attachment")]
        public FileSystemStoreObject U_FilePath
        {
            get { return GetPropertyValue<FileSystemStoreObject>("U_FilePath"); }
            set { SetPropertyValue<FileSystemStoreObject>("U_FilePath", value); }
        }

        private FTS11OFAM _U_RootKey;
        [DataSourceCriteria("DocEntry != '@This.DocEntry'")]
        [XafDisplayName("Component Of"), ToolTip("Component Of")]
        [Index(576), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public FTS11OFAM U_RootKey
        {
            get { return _U_RootKey; }
            set { SetPropertyValue("U_RootKey", ref _U_RootKey, value); }
        }

        private double _SQuantity;
        [Index(800), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("C/F Quantity"), ToolTip("C/F Quantity")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [Appearance("SQuantity", Enabled = false)]
        [DbType("numeric(19,6)")]
        public double SQuantity
        {
            get { return _SQuantity; }
            set { SetPropertyValue("SQuantity", ref _SQuantity, value); }
        }

        private string _U_LastLoc;
        [XafDisplayName("Location"), ToolTip("Location")]
        [Appearance("SLocation", Enabled = false)]
        [Index(801), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string U_LastLoc
        {
            get { return _U_LastLoc; }
            set { SetPropertyValue("U_LastLoc", ref _U_LastLoc, value); }
        }

        private double _STotalCostLC;
        [XafDisplayName("C/F Cost (LC)"), ToolTip("C/F (LC)")]
        [Index(802), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("STotalCostLC", Enabled = false)]
        //[ModelDefault("DisplayFormat", "{0:n2}")]
        //[ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double STotalCostLC
        {
            get { return _STotalCostLC; }
            set { SetPropertyValue("STotalCostLC", ref _STotalCostLC, value); }
        }

        private double _SDeprLC;
        [XafDisplayName("Accu. Depreciation"), ToolTip("Accu. Depreciation")]
        [Index(803), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("SDeprLC", Enabled = false)]
        //[ModelDefault("DisplayFormat", "{0:n2}")]
        //[ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double SDeprLC
        {
            get { return _SDeprLC; }
            set { SetPropertyValue("SDeprLC", ref _SDeprLC, value); }
        }

        private double _SNBVLC;
        [XafDisplayName("NBV"), ToolTip("NBV")]
        [Index(804), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("SNBVLC", Enabled = false)]
        //[ModelDefault("DisplayFormat", "{0:n2}")]
        //[ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double SNBVLC
        {
            get { return _SNBVLC; }
            set { SetPropertyValue("SNBVLC", ref _SNBVLC, value); }
        }

        private string _U_CreateBy;
        [XafDisplayName("Create By"), ToolTip("Create By")]
        [Index(993), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_CreateBy
        {
            get { return _U_CreateBy; }
            set { SetPropertyValue("U_CreateBy", ref _U_CreateBy, value); }
        }

        private string _U_UpdateBy;
        [XafDisplayName("Update By"), ToolTip("Update By")]
        [Index(994), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_UpdateBy
        {
            get { return _U_UpdateBy; }
            set { SetPropertyValue("U_UpdateBy", ref _U_UpdateBy, value); }
        }

        private string _U_CnclBy;
        [XafDisplayName("Cancel By"), ToolTip("Cancel By")]
        [Index(995), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string U_CnclBy
        {
            get { return _U_CnclBy; }
            set { SetPropertyValue("U_CnclBy", ref _U_CnclBy, value); }
        }

        private DateTime _CreateDate;
        [Index(996), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { SetPropertyValue("CreateDate", ref _CreateDate, value); }
        }

        private DateTime _UpdateDate;
        [Index(997), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
        [XafDisplayName("Update Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { SetPropertyValue("UpdateDate", ref _UpdateDate, value); }
        }

        private DateTime _U_CnclDate;
        [Index(998), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
        [XafDisplayName("Cancel Date")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        [DbType("datetime")]
        public DateTime U_CnclDate
        {
            get { return _U_CnclDate; }
            set { SetPropertyValue("U_CnclDate", ref _U_CnclDate, value); }
        }

        private string _Object;
        [XafDisplayName("Object"), ToolTip("Object")]
        [Index(999), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }

        private int _LogInst;
        [Index(1000), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
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
        private string _IsFieldEditable;
        [Index(1010), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(true)]
        [DbType("nvarchar(1)")]
        public string IsFieldEditable
        {
            get { return _IsFieldEditable; }
            set { SetPropertyValue("IsFieldEditable", ref _IsFieldEditable, value); }
        }

        [Browsable(false)]
        public bool IsFACodeDisabled
        {
            get { return Session.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'")).U_DisFACode; }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get { return Session.IsNewObject(this); }
        }

    }
}