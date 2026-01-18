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
    [XafDisplayName("FA Group Advanced")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11FAG1 : XPLiteObject
    { 
        public FTS11FAG1(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Object = "FTS11FAG1";
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        #region Parent Table
        private FTS11OFAG _U_FAGrpKey;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11OFAG-FTS11FAG1", typeof(FTS11OFAG))]
        public FTS11OFAG U_FAGrpKey
        {
            get { return _U_FAGrpKey; }
            set { SetPropertyValue("U_FAGrpKey", ref _U_FAGrpKey, value); }
        }
        #endregion

        private FTS11OLOC _U_LocationKey;
        [XafDisplayName("Location"), ToolTip("Location")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public FTS11OLOC U_LocationKey
        {
            get { return _U_LocationKey; }
            set {
                SetPropertyValue("U_LocationKey", ref _U_LocationKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        SetPropertyValue("U_Location", ref _U_Location, value.Code);
                    }
                    else
                    {
                        SetPropertyValue("U_Location", ref _U_Location, "");
                    }
                }
            }
        }

        private string _U_Location;
        [XafDisplayName("Location Code"), ToolTip("Location Code")]
        [Index(11), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string U_Location
        {
            get { return _U_Location; }
            set { SetPropertyValue("U_Location", ref _U_Location, value); }
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
        [Index(111), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(112), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(121), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(122), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(131), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(132), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(141), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(142), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(151), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(152), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string U_AccNamQ
        {
            get { return _U_AccNamQ; }
            set { SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, value); }
        }

        private string _Object;
        [XafDisplayName("Object"), ToolTip("Object")]
        [Index(999), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Object
        {
            get { return _Object; }
            set { SetPropertyValue("Object", ref _Object, value); }
        }
    }
}