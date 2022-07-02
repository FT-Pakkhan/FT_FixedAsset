using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Data.Filtering;

namespace FT_FixedAsset.Module.BusinessObjects
{
    [XafDisplayName("Fixed Asset Reports")]
    [ImageName("Action_Chart_ShowDesigner")]
    [NavigationItem("Widgets")]
    [NonPersistent]
    [DomainComponent]
    [Appearance("DisableReset", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave", AppearanceItemType.Action, "True", TargetItems = "Save", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&New", AppearanceItemType.Action, "True", TargetItems = "SaveAndNew", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableSave&Close", AppearanceItemType.Action, "True", TargetItems = "SaveAndClose", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("DisableCancel", AppearanceItemType.Action, "True", TargetItems = "Cancel", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class ReportViewer : BaseObject
    {
        public ReportViewer(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Report = Session.FindObject<FTS11SRPT>(CriteriaOperator.Parse("DocEntry = '1'"));
            Group1 = Session.FindObject<SE_FARptGrpOpt>(CriteriaOperator.Parse("Code = 'faclsnam'"));
            Group2 = Session.FindObject<SE_FARptGrpOpt>(CriteriaOperator.Parse("Code = 'fagrpnam'"));
            DateFrm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrm.AddMonths(1).AddDays(-1);
        }

        private FTS11SRPT _Report;
        [NonPersistent]
        [Index(0), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Reports"), ToolTip("Reports")]
        public FTS11SRPT Report
        {
            get { return _Report; }
            set { SetPropertyValue("Report", ref _Report, value); }
        }

        private SE_FARptGrpOpt _Group1;
        [XafDisplayName("Grouping 1"), ToolTip("Grouping 1")]
        [Index(1), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FARptGrpOpt Group1
        {
            get { return _Group1; }
            set { SetPropertyValue("Group1", ref _Group1, value); }
        }

        private SE_FARptGrpOpt _Group2;
        [XafDisplayName("Grouping 2"), ToolTip("Grouping 2")]
        [Index(2), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [NoForeignKey]
        public SE_FARptGrpOpt Group2
        {
            get { return _Group2; }
            set { SetPropertyValue("Group2", ref _Group2, value); }
        }

        private DateTime _DateFrm;
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Trans Date From")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DateFrm
        {
            get { return _DateFrm; }
            set { SetPropertyValue("DateFrm", ref _DateFrm, value); }
        }

        private DateTime _DateTo;
        [Index(11), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Trans Date To")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime DateTo
        {
            get { return _DateTo; }
            set { SetPropertyValue("DateTo", ref _DateTo, value); }
        }

        private DateTime _AcqDateFrm;
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Acquire Date From")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime AcqDateFrm
        {
            get { return _AcqDateFrm; }
            set { SetPropertyValue("AcqDateFrm", ref _AcqDateFrm, value); }
        }

        private DateTime _AcqDateTo;
        [Index(21), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Acquire Date To")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime AcqDateTo
        {
            get { return _AcqDateTo; }
            set { SetPropertyValue("AcqDateTo", ref _AcqDateTo, value); }
        }

        private DateTime _CreateDateFrm;
        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("Create Date From")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime CreateDateFrm
        {
            get { return _CreateDateFrm; }
            set { SetPropertyValue("CreateDateFrm", ref _CreateDateFrm, value); }
        }

        private DateTime _CreateDateTo;
        [Index(31), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        [XafDisplayName("CreateDate To")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime CreateDateTo
        {
            get { return _CreateDateTo; }
            set { SetPropertyValue("CreateDateTo", ref _CreateDateTo, value); }
        }

        private FTS11OFAM _FACodeFrmKey;
        [XafDisplayName("F/A Code From"), ToolTip("F/A Code From")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OFAM FACodeFrmKey
        {
            get { return _FACodeFrmKey; }
            set
            {
                SetPropertyValue("FACodeFrmKey", ref _FACodeFrmKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("FACodeFrm", ref _FACodeFrm, value.U_FACode);
                    else
                        SetPropertyValue("FACodeFrm", ref _FACodeFrm, "");
                }
            }
        }

        private string _FACodeFrm;
        [XafDisplayName("F/A Code From1"), ToolTip("F/A Code From1")]
        [Index(41), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string FACodeFrm
        {
            get { return _FACodeFrm; }
            set { SetPropertyValue("FACodeFrm", ref _FACodeFrm, value); }
        }

        private FTS11OFAM _FACodeToKey;
        [XafDisplayName("F/A Code To"), ToolTip("F/A Code To")]
        [Index(42), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OFAM FACodeToKey
        {
            get { return _FACodeToKey; }
            set
            {
                SetPropertyValue("FACodeToKey", ref _FACodeToKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("FACodeTo", ref _FACodeTo, value.U_FACode);
                    else
                        SetPropertyValue("FACodeTo", ref _FACodeTo, "");
                }
            }
        }

        private string _FACodeTo;
        [XafDisplayName("F/A Code To1"), ToolTip("F/A Code To1")]
        [Index(43), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string FACodeTo
        {
            get { return _FACodeTo; }
            set { SetPropertyValue("FACodeTo", ref _FACodeTo, value); }
        }

        private FTS11OFAG _FAGrpFrmKey;
        [XafDisplayName("F/A Group From"), ToolTip("F/A Group From")]
        [Index(50), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OFAG FAGrpFrmKey
        {
            get { return _FAGrpFrmKey; }
            set
            {
                SetPropertyValue("FAGrpFrmKey", ref _FAGrpFrmKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("FAGrpFrm", ref _FAGrpFrm, value.U_FAGrpCod);
                    else
                        SetPropertyValue("FAGrpFrm", ref _FAGrpFrm, "");
                }
            }
        }

        private string _FAGrpFrm;
        [XafDisplayName("F/A Group From1"), ToolTip("F/A Group From1")]
        [Index(51), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string FAGrpFrm
        {
            get { return _FAGrpFrm; }
            set { SetPropertyValue("FAGrpFrm", ref _FAGrpFrm, value); }
        }

        private FTS11OFAG _FAGrpToKey;
        [XafDisplayName("F/A Group To"), ToolTip("F/A Group To")]
        [Index(52), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OFAG FAGrpToKey
        {
            get { return _FAGrpToKey; }
            set
            {
                SetPropertyValue("FAGrpToKey", ref _FAGrpToKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("FAGrpTo", ref _FAGrpTo, value.U_FAGrpCod);
                    else
                        SetPropertyValue("FAGrpTo", ref _FAGrpTo, "");
                }
            }
        }

        private string _FAGrpTo;
        [XafDisplayName("F/A Group To1"), ToolTip("F/A Group To1")]
        [Index(53), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string FAGrpTo
        {
            get { return _FAGrpTo; }
            set { SetPropertyValue("FAGrpTo", ref _FAGrpTo, value); }
        }

        private FTS11OFAC _FAClsFrmKey;
        [XafDisplayName("F/A Class From"), ToolTip("F/A Class From")]
        [Index(60), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OFAC FAClsFrmKey
        {
            get { return _FAClsFrmKey; }
            set
            {
                SetPropertyValue("FAClsFrmKey", ref _FAClsFrmKey, value);
                if (!IsLoading & value != null)
                {
                    if (value != null)
                        SetPropertyValue("FAClsFrm", ref _FAClsFrm, value.Code);
                    else
                        SetPropertyValue("FAClsFrm", ref _FAClsFrm, "");
                }
            }
        }

        private string _FAClsFrm;
        [XafDisplayName("F/A Class From1"), ToolTip("F/A Class From1")]
        [Index(61), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string FAClsFrm
        {
            get { return _FAClsFrm; }
            set { SetPropertyValue("FAClsFrm", ref _FAClsFrm, value); }
        }

        private FTS11OFAC _FAClsToKey;
        [XafDisplayName("F/A Class To"), ToolTip("F/A Class To")]
        [Index(62), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OFAC FAClsToKey
        {
            get { return _FAClsToKey; }
            set
            {
                SetPropertyValue("FAClsToKey", ref _FAClsToKey, value);
                if (!IsLoading)
                {
                    if (value != null)
                        SetPropertyValue("FAClsTo", ref _FAClsTo, value.Code);
                    else
                        SetPropertyValue("FAClsTo", ref _FAClsTo, "");
                }
            }
        }

        private string _FAClsTo;
        [XafDisplayName("F/A Class To1"), ToolTip("F/A Class To1")]
        [Index(63), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string FAClsTo
        {
            get { return _FAClsTo; }
            set { SetPropertyValue("FAClsTo", ref _FAClsTo, value); }
        }

        private FTS11OLOC _U_LocFrmKey;
        [XafDisplayName("Location From"), ToolTip("Location From")]
        [Index(70), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OLOC U_LocFrmKey
        {
            get { return _U_LocFrmKey; }
            set { SetPropertyValue("U_LocFrmKey", ref _U_LocFrmKey, value); }
        }

        private FTS11OLOC _U_LocToKey;
        [XafDisplayName("Location To"), ToolTip("Location To")]
        [Index(80), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ImmediatePostData]
        public FTS11OLOC U_LocToKey
        {
            get { return _U_LocToKey; }
            set { SetPropertyValue("U_LocToKey", ref _U_LocToKey, value); }
        }

        private double _NBV_Frm;
        [XafDisplayName("NBV From"), ToolTip("NBV From")]
        [Index(90), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double NBV_Frm
        {
            get { return _NBV_Frm; }
            set { SetPropertyValue("NBV_Frm", ref _NBV_Frm, value); }
        }

        private double _NBV_To;
        [XafDisplayName("NBV To"), ToolTip("NBV To")]
        [Index(100), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public double NBV_To
        {
            get { return _NBV_To; }
            set { SetPropertyValue("NBV_To", ref _NBV_To, value); }
        }

        private int _QtyFrom;
        [Index(110), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Quantity From"), ToolTip("Quantity From")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int QtyFrom
        {
            get { return _QtyFrom; }
            set { SetPropertyValue("QtyFrom", ref _QtyFrom, value); }
        }

        private int _QtyTo;
        [Index(120), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [XafDisplayName("Quantity To"), ToolTip("Quantity To")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        [ModelDefault("EditMask", "{0:n0}")]
        public int QtyTo
        {
            get { return _QtyTo; }
            set { SetPropertyValue("QtyTo", ref _QtyTo, value); }
        }

        private FTS11_vw_OACT _U_AccFrm;
        [XafDisplayName("G/L Acc. From"), ToolTip("G/L Acc. From")]
        [Index(130), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccFrm
        {
            get { return _U_AccFrm; }
            set { SetPropertyValue("U_AccFrm", ref _U_AccFrm, value); }
        }

        private FTS11_vw_OACT _U_AccTo;
        [XafDisplayName("G/L Acc. To"), ToolTip("G/L Acc. To")]
        [Index(140), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [ImmediatePostData]
        public FTS11_vw_OACT U_AccTo
        {
            get { return _U_AccTo; }
            set { SetPropertyValue("U_AccTo", ref _U_AccTo, value); }
        }

        private FTS11_vw_OPRJ _U_ProjectFrm;
        //[XafDisplayName("Project From"), ToolTip("Project From")]
        [XafDisplayName("Plant From"), ToolTip("Plant From")]
        [Index(150), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public FTS11_vw_OPRJ U_ProjectFrm
        {
            get { return _U_ProjectFrm; }
            set { SetPropertyValue("U_ProjectFrm", ref _U_ProjectFrm, value); }
        }

        private FTS11_vw_OPRJ _U_ProjectTo;
        //[XafDisplayName("Project To"), ToolTip("Project To")]
        [XafDisplayName("Plant To"), ToolTip("Plant To")]
        [Index(151), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        public FTS11_vw_OPRJ U_ProjectTo
        {
            get { return _U_ProjectTo; }
            set { SetPropertyValue("U_ProjectTo", ref _U_ProjectTo, value); }
        }

        private FTS11_vw_OPRC _U_ProfitCodeFrm;
        //[XafDisplayName("C.Centre D1 From"), ToolTip("C.Centre D1 From")]
        [XafDisplayName("Project Code From"), ToolTip("Project Code From")]
        [Index(160), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '1'")]
        public FTS11_vw_OPRC U_ProfitCodeFrm
        {
            get { return _U_ProfitCodeFrm; }
            set { SetPropertyValue("U_ProfitCodeFrm", ref _U_ProfitCodeFrm, value); }
        }

        private FTS11_vw_OPRC _U_ProfitCodeTo;
        //[XafDisplayName("C.Centre D1 To"), ToolTip("C.Centre D1 To")]
        [XafDisplayName("Project Code To"), ToolTip("Project Code To")]
        [Index(161), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '1'")]
        public FTS11_vw_OPRC U_ProfitCodeTo
        {
            get { return _U_ProfitCodeTo; }
            set { SetPropertyValue("U_ProfitCodeTo", ref _U_ProfitCodeTo, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode2Frm;
        //[XafDisplayName("C.Centre D2 From"), ToolTip("C.Centre D2 From")]
        [XafDisplayName("Department From"), ToolTip("Department From")]
        [Index(170), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '2'")]
        public FTS11_vw_OPRC U_OcrCode2Frm
        {
            get { return _U_OcrCode2Frm; }
            set { SetPropertyValue("U_OcrCode2Frm", ref _U_OcrCode2Frm, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode2To;
        //[XafDisplayName("C.Centre D2 To"), ToolTip("C.Centre D2 To")]
        [XafDisplayName("Department To"), ToolTip("Department To")]
        [Index(171), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '2'")]
        public FTS11_vw_OPRC U_OcrCode2To
        {
            get { return _U_OcrCode2To; }
            set { SetPropertyValue("U_OcrCode2To", ref _U_OcrCode2To, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode3Frm;
        [XafDisplayName("C.Centre D3 From"), ToolTip("Cost Centre D3 From")]
        [Index(180), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '3'")]
        public FTS11_vw_OPRC U_OcrCode3Frm
        {
            get { return _U_OcrCode3Frm; }
            set { SetPropertyValue("U_OcrCode3Frm", ref _U_OcrCode3Frm, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode3To;
        [XafDisplayName("C.Centre D3 To"), ToolTip("Cost Centre D3 To")]
        [Index(181), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '3'")]
        public FTS11_vw_OPRC U_OcrCode3To
        {
            get { return _U_OcrCode3To; }
            set { SetPropertyValue("U_OcrCode3To", ref _U_OcrCode3To, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode4Frm;
        [XafDisplayName("C.Centre D4 From"), ToolTip("C.Centre D4 From")]
        [Index(190), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '4'")]
        public FTS11_vw_OPRC U_OcrCode4Frm
        {
            get { return _U_OcrCode4Frm; }
            set { SetPropertyValue("U_OcrCode4Frm", ref _U_OcrCode4Frm, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode4To;
        [XafDisplayName("C.Centre D4 To"), ToolTip("C.Centre D4 To")]
        [Index(191), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '4'")]
        public FTS11_vw_OPRC U_OcrCode4To
        {
            get { return _U_OcrCode4To; }
            set { SetPropertyValue("U_OcrCode4To", ref _U_OcrCode4To, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode5Frm;
        [XafDisplayName("C.Centre D5 From"), ToolTip("C.Centre D5 From")]
        [Index(200), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '5'")]
        public FTS11_vw_OPRC U_OcrCode5Frm
        {
            get { return _U_OcrCode5Frm; }
            set { SetPropertyValue("U_OcrCode5Frm", ref _U_OcrCode5Frm, value); }
        }

        private FTS11_vw_OPRC _U_OcrCode5To;
        [XafDisplayName("C.Centre D5 To"), ToolTip("C.Centre D5 To")]
        [Index(201), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [NoForeignKey]
        [DataSourceCriteria("DimCode = '5'")]
        public FTS11_vw_OPRC U_OcrCode5To
        {
            get { return _U_OcrCode5To; }
            set { SetPropertyValue("U_OcrCode5To", ref _U_OcrCode5To, value); }
        }

        private FTS11OIND _U_IndKey1Frm;
        //[XafDisplayName("Indicator 1 From"), ToolTip("Indicator 1 From")]
        [XafDisplayName("Area Code From"), ToolTip("Area Code From")]
        [Index(210), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '1'")]
        public FTS11OIND U_IndKey1Frm
        {
            get { return _U_IndKey1Frm; }
            set { SetPropertyValue("U_IndKey1Frm", ref _U_IndKey1Frm, value); }
        }

        private FTS11OIND _U_IndKey1To;
        //[XafDisplayName("Indicator 1 To"), ToolTip("Indicator 1 To")]
        [XafDisplayName("Area Code To"), ToolTip("Area Code To")]
        [Index(211), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '1'")]
        public FTS11OIND U_IndKey1To
        {
            get { return _U_IndKey1To; }
            set { SetPropertyValue("U_IndKey1To", ref _U_IndKey1To, value); }
        }

        private FTS11OIND _U_IndKey2Frm;
        [XafDisplayName("Indicator 2 From"), ToolTip("Indicator 2 From")]
        [Index(220), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '2'")]
        public FTS11OIND U_IndKey2Frm
        {
            get { return _U_IndKey2Frm; }
            set { SetPropertyValue("U_IndKey2Frm", ref _U_IndKey2Frm, value); }
        }

        private FTS11OIND _U_IndKey2To;
        [XafDisplayName("Indicator 2 To"), ToolTip("Indicator 2 To")]
        [Index(221), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '2'")]
        public FTS11OIND U_IndKey2To
        {
            get { return _U_IndKey2To; }
            set { SetPropertyValue("U_IndKey2To", ref _U_IndKey2To, value); }
        }

        private FTS11OIND _U_IndKey3Frm;
        [XafDisplayName("Indicator 3 From"), ToolTip("Indicator 3 From")]
        [Index(230), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '3'")]
        public FTS11OIND U_IndKey3Frm
        {
            get { return _U_IndKey3Frm; }
            set { SetPropertyValue("U_IndKey3Frm", ref _U_IndKey3Frm, value); }
        }

        private FTS11OIND _U_IndKey3To;
        [XafDisplayName("Indicator 3 To"), ToolTip("Indicator 3 To")]
        [Index(231), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '3'")]
        public FTS11OIND U_IndKey3To
        {
            get { return _U_IndKey3To; }
            set { SetPropertyValue("U_IndKey3To", ref _U_IndKey3To, value); }
        }

        private FTS11OIND _U_IndKey4Frm;
        [XafDisplayName("Indicator 4 From"), ToolTip("Indicator 4 From")]
        [Index(240), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '4'")]
        public FTS11OIND U_IndKey4Frm
        {
            get { return _U_IndKey4Frm; }
            set { SetPropertyValue("U_IndKey4Frm", ref _U_IndKey4Frm, value); }
        }

        private FTS11OIND _U_IndKey4To;
        [XafDisplayName("Indicator 4 To"), ToolTip("Indicator 4 To")]
        [Index(241), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '4'")]
        public FTS11OIND U_IndKey4To
        {
            get { return _U_IndKey4To; }
            set { SetPropertyValue("U_IndKey4To", ref _U_IndKey4To, value); }
        }

        private FTS11OIND _U_IndKey5Frm;
        [XafDisplayName("Indicator 5 From"), ToolTip("Indicator 5 From")]
        [Index(250), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '5'")]
        public FTS11OIND U_IndKey5Frm
        {
            get { return _U_IndKey5Frm; }
            set { SetPropertyValue("U_IndKey5Frm", ref _U_IndKey5Frm, value); }
        }

        private FTS11OIND _U_IndKey5To;
        [XafDisplayName("Indicator 5 To"), ToolTip("Indicator 5 To")]
        [Index(251), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '5'")]
        public FTS11OIND U_IndKey5To
        {
            get { return _U_IndKey5To; }
            set { SetPropertyValue("U_IndKey5To", ref _U_IndKey5To, value); }
        }

        private FTS11OIND _U_IndKey6Frm;
        [XafDisplayName("Indicator 6 From"), ToolTip("Indicator 6 From")]
        [Index(260), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '6'")]
        public FTS11OIND U_IndKey6Frm
        {
            get { return _U_IndKey6Frm; }
            set { SetPropertyValue("U_IndKey6Frm", ref _U_IndKey6Frm, value); }
        }

        private FTS11OIND _U_IndKey6To;
        [XafDisplayName("Indicator 6 To"), ToolTip("Indicator 6 To")]
        [Index(261), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [DataSourceCriteria("U_IndDim = '6'")]
        public FTS11OIND U_IndKey6To
        {
            get { return _U_IndKey6To; }
            set { SetPropertyValue("U_IndKey6To", ref _U_IndKey6To, value); }
        }
    }
}
