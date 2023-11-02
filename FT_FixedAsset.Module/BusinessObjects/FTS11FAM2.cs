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
    [XafDisplayName("FA Master Advanced")]
    [Appearance("DisableLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class FTS11FAM2 : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public FTS11FAM2(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Object = "FTS11FAM2";
        }

        [Key(AutoGenerate = true), Browsable(false)]
        public int DocEntry { get; set; }

        #region Parent Table
        private FTS11OFAM _U_FAKey;
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Association("FTS11OFAM-FTS11FAM2", typeof(FTS11OFAM))]
        public FTS11OFAM U_FAKey
        {
            get { return _U_FAKey; }
            set { SetPropertyValue("U_FAKey", ref _U_FAKey, value); }
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
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_Location", ref _U_Location, value.Code);
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
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_AccCodA", ref _U_AccCodA, value.AcctCode);
                    SetPropertyValue("U_AccNamA", ref _U_AccNamA, value.AcctName);
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
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_AccCodD", ref _U_AccCodD, value.AcctCode);
                    SetPropertyValue("U_AccNamD", ref _U_AccNamD, value.AcctName);
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
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_AccCodC", ref _U_AccCodC, value.AcctCode);
                    SetPropertyValue("U_AccNamC", ref _U_AccNamC, value.AcctName);
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
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_AccCodW", ref _U_AccCodW, value.AcctCode);
                    SetPropertyValue("U_AccNamW", ref _U_AccNamW, value.AcctName);
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
                if (!IsLoading & value != null)
                {
                    SetPropertyValue("U_AccCodQ", ref _U_AccCodQ, value.AcctCode);
                    SetPropertyValue("U_AccNamQ", ref _U_AccNamQ, value.AcctName);
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

        private FTS11_vw_OPRJ _U_Project;
        [XafDisplayName("Project"), ToolTip("Project")]
        [Index(160), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public FTS11_vw_OPRJ U_Project
        {
            get { return _U_Project; }
            set { SetPropertyValue("U_Project", ref _U_Project, value); }
        }

        private FTS11_vw_OPRC _U_ProfitCode;
        [XafDisplayName("Cost Centre D1"), ToolTip("Cost Centre D1")]
        [Index(170), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '1'")]
        public FTS11_vw_OPRC U_ProfitCode
        {
            get { return _U_ProfitCode; }
            set { SetPropertyValue("U_ProfitCode", ref _U_ProfitCode, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode2;
        [XafDisplayName("Cost Centre D2"), ToolTip("Cost Centre D2")]
        [Index(171), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '2'")]
        public FTS11_vw_OPRC U_OcrCode2
        {
            get { return _U_OcrCode2; }
            set { SetPropertyValue("U_OcrCode2", ref _U_OcrCode2, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode3;
        [XafDisplayName("Cost Centre D3"), ToolTip("Cost Centre D3")]
        [Index(172), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '3'")]
        public FTS11_vw_OPRC U_OcrCode3
        {
            get { return _U_OcrCode3; }
            set { SetPropertyValue("U_OcrCode3", ref _U_OcrCode3, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode4;
        [XafDisplayName("Cost Centre D4"), ToolTip("Cost Centre D4")]
        [Index(173), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '4'")]
        public FTS11_vw_OPRC U_OcrCode4
        {
            get { return _U_OcrCode4; }
            set { SetPropertyValue("U_OcrCode4", ref _U_OcrCode4, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode5;
        [XafDisplayName("Cost Centre D5"), ToolTip("Cost Centre D5")]
        [Index(174), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '5'")]
        public FTS11_vw_OPRC U_OcrCode5
        {
            get { return _U_OcrCode5; }
            set { SetPropertyValue("U_OcrCode5", ref _U_OcrCode5, value); }
        }

        private FTS11OIND _U_IndKey1;
        [XafDisplayName("Indicator 1"), ToolTip("Indicator 1")]
        [Index(180), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(181), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod1
        {
            get { return _U_IndCod1; }
            set { SetPropertyValue("U_IndCod1", ref _U_IndCod1, value); }
        }

        private FTS11OIND _U_IndKey2;
        [XafDisplayName("Indicator 2"), ToolTip("Indicator 2")]
        [Index(190), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(191), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod2
        {
            get { return _U_IndCod2; }
            set { SetPropertyValue("U_IndCod2", ref _U_IndCod2, value); }
        }


        private FTS11OIND _U_IndKey3;
        [XafDisplayName("Indicator 3"), ToolTip("Indicator 3")]
        [Index(200), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(201), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod3
        {
            get { return _U_IndCod3; }
            set { SetPropertyValue("U_IndCod3", ref _U_IndCod3, value); }
        }

        private FTS11OIND _U_IndKey4;
        [XafDisplayName("Indicator 4"), ToolTip("Indicator 4")]
        [Index(210), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(211), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod4
        {
            get { return _U_IndCod4; }
            set { SetPropertyValue("U_IndCod4", ref _U_IndCod4, value); }
        }

        private FTS11OIND _U_IndKey5;
        [XafDisplayName("Indicator 5"), ToolTip("Indicator 5")]
        [Index(220), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(221), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod5
        {
            get { return _U_IndCod5; }
            set { SetPropertyValue("U_IndCod5", ref _U_IndCod5, value); }
        }

        private FTS11OIND _U_IndKey6;
        [XafDisplayName("Indicator 6"), ToolTip("Indicator 6")]
        [Index(230), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
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
        [Index(231), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string U_IndCod6
        {
            get { return _U_IndCod6; }
            set { SetPropertyValue("U_IndCod6", ref _U_IndCod6, value); }
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