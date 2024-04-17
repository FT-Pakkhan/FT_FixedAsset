using DevExpress.Data.Filtering;
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
    [XafDisplayName("Fixed Asset Group")]
    [NavigationItem("Setup")]
    [ImageName("LoadPageSetup_16x16")]
    [DefaultProperty("U_FAGrpCod")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableValidate", AppearanceItemType.Action, "True", TargetItems = "ShowAllContexts", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11OFAG : XPLiteObject
    {
        public FTS11OFAG(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            U_DeprnYN = true;
            U_Status = Session.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'O'"));
            U_DeprnBy = Session.FindObject<SE_FADeprnBy>(CriteriaOperator.Parse("Code = 'U'"));
            U_DeprnTyp = Session.FindObject<SE_FADeprnTyp>(CriteriaOperator.Parse("Code = 'S'"));
            U_Prorate = Session.FindObject<SE_FAProrate>(CriteriaOperator.Parse("Code = 'N'"));
            if (SecuritySystem.CurrentUserId != null && SecuritySystem.CurrentUserId.ToString() != "")
            {
                U_CreateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
                U_UpdateBy = Session.GetObjectByKey<SystemUsers>(SecuritySystem.CurrentUserId).UserName;
            }
            CreateDate = DateTime.Today;
            UpdateDate = DateTime.Today;
            Object = "FTS11OFAG";
        }

        #region Child Table
        [Association("FTS11OFAG-FTS11FAG1", typeof(FTS11FAG1))]
        public XPCollection FTS11FAG1
        {
            get { return GetCollection("FTS11FAG1"); }
        }
        #endregion

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        private SE_FAStatus _U_Status;
        [XafDisplayName("Status"), ToolTip("Status")]
        [NoForeignKey]
        [Index(0), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Status", Enabled = false)]
        public SE_FAStatus U_Status
        {
            get { return _U_Status; }
            set { SetPropertyValue("U_Status", ref _U_Status, value); }
        }

        private string _U_FAGrpCod;
        [XafDisplayName("F/A Group Code"), ToolTip("F/A Group Code")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save), RuleUniqueValue(DefaultContexts.Save)]
        public string U_FAGrpCod
        {
            get { return _U_FAGrpCod; }
            set { SetPropertyValue("U_FAGrpCod", ref _U_FAGrpCod, value); }
        }

        private string _U_FAGrpNam;
        [XafDisplayName("F/A Group Name"), ToolTip("F/A Group Name")]
        [Index(11), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save), RuleUniqueValue(DefaultContexts.Save)]
        public string U_FAGrpNam
        {
            get { return _U_FAGrpNam; }
            set { SetPropertyValue("U_FAGrpNam", ref _U_FAGrpNam, value); }
        }

        private FTS11OFAC _U_FAClsKey;
        [XafDisplayName("F/A Class"), ToolTip("F/A Class")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OFAC U_FAClsKey
        {
            get { return _U_FAClsKey; }
            set {
                SetPropertyValue("U_FAClsKey", ref _U_FAClsKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("U_FAClsCod", ref _U_FAClsCod, value.Code);
                    else
                        SetPropertyValue("U_FAClsCod", ref _U_FAClsCod, "");
                }
            }
        }

        private string _U_FAClsCod;
        [XafDisplayName("F/A Class Code"), ToolTip("F/A Class Code")]
        [Index(21), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("FAClsCod", Enabled = false)]
        public string U_FAClsCod
        {
            get { return _U_FAClsCod; }
            set { SetPropertyValue("U_FAClsCod", ref _U_FAClsCod, value); }
        }

        private bool _U_DeprnYN;
        [Index(30), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Depreciation")]
        [ImmediatePostData]
        public bool U_DeprnYN
        {
            get { return _U_DeprnYN; }
            set { SetPropertyValue("U_DeprnYN", ref _U_DeprnYN, value); }
        }

        private SE_FADeprnBy _U_DeprnBy;
        [XafDisplayName("Depreciate By"), ToolTip("Depreciate By")]
        [NoForeignKey]
        [ImmediatePostData]
        [Index(40), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FADeprnBy U_DeprnBy
        {
            get { return _U_DeprnBy; }
            set {
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
        [Index(50), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FADeprnTyp U_DeprnTyp
        {
            get { return _U_DeprnTyp; }
            set { SetPropertyValue("U_DeprnTyp", ref _U_DeprnTyp, value); }
        }

        private SE_FAProrate _U_Prorate;
        [XafDisplayName("Prorate"), ToolTip("Prorate")]
        [NoForeignKey]
        [Appearance("Prorate", Enabled = false, Criteria = "[U_DeprnTyp].[Code] <> 'S'")]
        [Index(60), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public SE_FAProrate U_Prorate
        {
            get { return _U_Prorate; }
            set { SetPropertyValue("U_Prorate", ref _U_Prorate, value); }
        }

        private double _U_UseLife;
        [Index(70), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Useful Life (Months)"), ToolTip("Useful Life (Months)")]
        [Appearance("UseLife", Enabled = false, Criteria = "[U_DeprnBy].[Code] <> 'U'")]
        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_UseLife
        {
            get { return _U_UseLife; }
            set { 
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
        [Index(80), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Percentage %"), ToolTip("Precentage %")]
        [Appearance("DeprPrcnt", Enabled = false, Criteria = "[U_DeprnBy].[Code] <> 'P'")]
        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_DeprPrcnt
        {
            get { return _U_DeprPrcnt; }
            set { 
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
        [Index(90), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Min. NBV (LC)"), ToolTip("Min. NBV (LC)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_MBaseLC
        {
            get { return _U_MBaseLC; }
            set { SetPropertyValue("U_MBaseLC", ref _U_MBaseLC, value); }
        }

        private double _U_MBaseSC;
        [XafDisplayName("Min NBV (SC)"), ToolTip("Min NBV (SC)")]
        [Index(100), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [DbType("numeric(19,6)")]
        public double U_MBaseSC
        {
            get { return _U_MBaseSC; }
            set { SetPropertyValue("U_MBaseSC", ref _U_MBaseSC, value); }
        }

        private string _U_AccCodA;
        [XafDisplayName("Asset A/C"), ToolTip("Asset A/C")]
        [Index(110), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodA
        {
            get { return _U_AccCodA; }
            set { SetPropertyValue("U_AccCodA", ref _U_AccCodA, value); }
        }

        private FTS11_vw_OACT _U_AccFrmA;
        [XafDisplayName("Asset A/C Code"), ToolTip("Asset A/C Code")]
        [Index(111), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
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
        [Index(112), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamA
        {
            get { return _U_AccNamA; }
            set { SetPropertyValue("U_AccNamA", ref _U_AccNamA, value); }
        }

        private string _U_AccCodD;
        [XafDisplayName("Depreciation A/C"), ToolTip("Depreciation A/C")]
        [Index(120), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodD
        {
            get { return _U_AccCodD; }
            set { SetPropertyValue("U_AccCodD", ref _U_AccCodD, value); }
        }

        private FTS11_vw_OACT _U_AccFrmD;
        [XafDisplayName("Depreciation A/C Code"), ToolTip("Depreciation A/C Code")]
        [Index(121), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "U_DeprnYN = true")]
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
        [Index(122), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamD
        {
            get { return _U_AccNamD; }
            set { SetPropertyValue("U_AccNamD", ref _U_AccNamD, value); }
        }

        private string _U_AccCodC;
        [XafDisplayName("Acc. Deprn A/C"), ToolTip("Acc. Deprn A/C")]
        [Index(130), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodC
        {
            get { return _U_AccCodC; }
            set { SetPropertyValue("U_AccCodC", ref _U_AccCodC, value); }
        }

        private FTS11_vw_OACT _U_AccFrmC;
        [XafDisplayName("Acc. Deprn A/C Code"), ToolTip("Acc. Deprn A/C Code")]
        [Index(131), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "U_DeprnYN = true")]
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
        [Index(132), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamC
        {
            get { return _U_AccNamC; }
            set { SetPropertyValue("U_AccNamC", ref _U_AccNamC, value); }
        }

        private string _U_AccCodW;
        [XafDisplayName("Disposal A/C"), ToolTip("Disposal A/C")]
        [Index(140), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodW
        {
            get { return _U_AccCodW; }
            set { SetPropertyValue("U_AccCodW", ref _U_AccCodW, value); }
        }

        private FTS11_vw_OACT _U_AccFrmW;
        [XafDisplayName("Disposal A/C Code"), ToolTip("Disposal A/C Code")]
        [Index(141), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(142), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamW
        {
            get { return _U_AccNamW; }
            set { SetPropertyValue("U_AccNamW", ref _U_AccNamW, value); }
        }

        private string _U_AccCodQ;
        [XafDisplayName("Asset Offset A/C"), ToolTip("Asset Offset A/C")]
        [Index(150), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_AccCodQ
        {
            get { return _U_AccCodQ; }
            set { SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, value); }
        }

        private FTS11_vw_OACT _U_AccFrmQ;
        [XafDisplayName("Asset Offset A/C Code"), ToolTip("Asset Offset A/C Code")]
        [Index(151), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(152), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamQ
        {
            get { return _U_AccNamQ; }
            set { SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, value); }
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