using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Web.Controllers
{
 
    public partial class UpdateXafDisplayNameController : ViewController
    {
        public UpdateXafDisplayNameController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM) || View.ObjectTypeInfo.Type == typeof(FTS11FAM2) || 
                View.ObjectTypeInfo.Type == typeof(FTS11OLOC) || View.ObjectTypeInfo.Type == typeof(ReportViewer) ||
                View.ObjectTypeInfo.Type == typeof(DeprnWidget) || View.ObjectTypeInfo.Type == typeof(PostWidget))
            {
                FTS11OADM OADM = ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = 1"));

                if (View is ListView listView && listView.Editor != null)
                {
                    /* Project (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_PrjCapt))
                    {
                        IModelColumn U_Project = listView.Model.Columns["U_Project"];
                        IModelColumn U_ProjectFrm = listView.Model.Columns["U_ProjectFrm"];
                        IModelColumn U_ProjectTo = listView.Model.Columns["U_ProjectTo"];
                        if (U_Project != null) U_Project.Caption = OADM.U_PrjCapt;
                        if (U_ProjectFrm != null) U_ProjectFrm.Caption = OADM.U_PrjCapt + " From";
                        if (U_ProjectTo != null) U_ProjectTo.Caption = OADM.U_PrjCapt + " To";
                    }

                    /* Cost Centre D1 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr1Capt))
                    {
                        IModelColumn U_ProfitCode = listView.Model.Columns["U_ProfitCode"];
                        IModelColumn U_ProfitCodeFrm = listView.Model.Columns["U_ProfitCodeFrm"];
                        IModelColumn U_ProfitCodeTo = listView.Model.Columns["U_ProfitCodeTo"];
                        if (U_ProfitCode != null) U_ProfitCode.Caption = OADM.U_Ocr1Capt;
                        if (U_ProfitCodeFrm != null) U_ProfitCodeFrm.Caption = OADM.U_Ocr1Capt + " From";
                        if (U_ProfitCodeTo != null) U_ProfitCodeTo.Caption = OADM.U_Ocr1Capt + " To";
                    }

                    /* Cost Centre D2 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr2Capt))
                    {
                        IModelColumn U_OcrCode2 = listView.Model.Columns["U_OcrCode2"];
                        IModelColumn U_OcrCode2Frm = listView.Model.Columns["U_OcrCode2Frm"];
                        IModelColumn U_OcrCode2To = listView.Model.Columns["U_OcrCode2To"];
                        if (U_OcrCode2 != null) U_OcrCode2.Caption = OADM.U_Ocr2Capt;
                        if (U_OcrCode2Frm != null) U_OcrCode2Frm.Caption = OADM.U_Ocr2Capt + " From";
                        if (U_OcrCode2To != null) U_OcrCode2To.Caption = OADM.U_Ocr2Capt + " To";
                    }

                    /* Cost Centre D3 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr3Capt))
                    {
                        IModelColumn U_OcrCode3 = listView.Model.Columns["U_OcrCode3"];
                        IModelColumn U_OcrCode3Frm = listView.Model.Columns["U_OcrCode3Frm"];
                        IModelColumn U_OcrCode3To = listView.Model.Columns["U_OcrCode3To"];
                        if (U_OcrCode3 != null) U_OcrCode3.Caption = OADM.U_Ocr3Capt;
                        if (U_OcrCode3Frm != null) U_OcrCode3Frm.Caption = OADM.U_Ocr3Capt + " From";
                        if (U_OcrCode3To != null) U_OcrCode3To.Caption = OADM.U_Ocr3Capt + " To";
                    }

                    /* Cost Centre D4 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr4Capt))
                    {
                        IModelColumn U_OcrCode4 = listView.Model.Columns["U_OcrCode4"];
                        IModelColumn U_OcrCode4Frm = listView.Model.Columns["U_OcrCode4Frm"];
                        IModelColumn U_OcrCode4To = listView.Model.Columns["U_OcrCode4To"];
                        if (U_OcrCode4 != null) U_OcrCode4.Caption = OADM.U_Ocr4Capt;
                        if (U_OcrCode4Frm != null) U_OcrCode4Frm.Caption = OADM.U_Ocr4Capt + " From";
                        if (U_OcrCode4To != null) U_OcrCode4To.Caption = OADM.U_Ocr4Capt + " To";
                    }

                    /* Cost Centre D5 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr5Capt))
                    {
                        IModelColumn U_OcrCode5 = listView.Model.Columns["U_OcrCode5"];
                        IModelColumn U_OcrCode5Frm = listView.Model.Columns["U_OcrCode5Frm"];
                        IModelColumn U_OcrCode5To = listView.Model.Columns["U_OcrCode5To"];
                        if (U_OcrCode5 != null) U_OcrCode5.Caption = OADM.U_Ocr5Capt;
                        if (U_OcrCode5Frm != null) U_OcrCode5Frm.Caption = OADM.U_Ocr5Capt + " From";
                        if (U_OcrCode5To != null) U_OcrCode5To.Caption = OADM.U_Ocr5Capt + " To";
                    }

                    /* Indicator 1 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind1Capt))
                    {
                        IModelColumn U_IndKey1 = listView.Model.Columns["U_IndKey1"];
                        IModelColumn U_IndKey1Frm = listView.Model.Columns["U_IndKey1Frm"];
                        IModelColumn U_IndKey1To = listView.Model.Columns["U_IndKey1To"];
                        if (U_IndKey1 != null) U_IndKey1.Caption = OADM.U_Ind1Capt;
                        if (U_IndKey1Frm != null) U_IndKey1Frm.Caption = OADM.U_Ind1Capt + " From";
                        if (U_IndKey1To != null) U_IndKey1To.Caption = OADM.U_Ind1Capt + " To";
                    }

                    /* Indicator 2 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind2Capt))
                    {
                        IModelColumn U_IndKey2 = listView.Model.Columns["U_IndKey2"];
                        IModelColumn U_IndKey2Frm = listView.Model.Columns["U_IndKey2Frm"];
                        IModelColumn U_IndKey2To = listView.Model.Columns["U_IndKey2To"];
                        if (U_IndKey2 != null) U_IndKey2.Caption = OADM.U_Ind2Capt;
                        if (U_IndKey2Frm != null) U_IndKey2Frm.Caption = OADM.U_Ind2Capt + " From";
                        if (U_IndKey2To != null) U_IndKey2To.Caption = OADM.U_Ind2Capt + " To";
                    }

                    /* Indicator 3 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind3Capt))
                    {
                        IModelColumn U_IndKey3 = listView.Model.Columns["U_IndKey3"];
                        IModelColumn U_IndKey3Frm = listView.Model.Columns["U_IndKey3Frm"];
                        IModelColumn U_IndKey3To = listView.Model.Columns["U_IndKey3To"];
                        if (U_IndKey3 != null) U_IndKey3.Caption = OADM.U_Ind3Capt;
                        if (U_IndKey3Frm != null) U_IndKey3Frm.Caption = OADM.U_Ind3Capt + " From";
                        if (U_IndKey3To != null) U_IndKey3To.Caption = OADM.U_Ind3Capt + " To";
                    }

                    /* Indicator 4 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind4Capt))
                    {
                        IModelColumn U_IndKey4 = listView.Model.Columns["U_IndKey4"];
                        IModelColumn U_IndKey4Frm = listView.Model.Columns["U_IndKey4Frm"];
                        IModelColumn U_IndKey4To = listView.Model.Columns["U_IndKey4To"];
                        if (U_IndKey4 != null) U_IndKey4.Caption = OADM.U_Ind4Capt;
                        if (U_IndKey4Frm != null) U_IndKey4Frm.Caption = OADM.U_Ind4Capt + " From";
                        if (U_IndKey4To != null) U_IndKey4To.Caption = OADM.U_Ind4Capt + " To";
                    }

                    /* Indicator 5 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind5Capt))
                    {
                        IModelColumn U_IndKey5 = listView.Model.Columns["U_IndKey5"];
                        IModelColumn U_IndKey5Frm = listView.Model.Columns["U_IndKey5Frm"];
                        IModelColumn U_IndKey5To = listView.Model.Columns["U_IndKey5To"];
                        if (U_IndKey5 != null) U_IndKey5.Caption = OADM.U_Ind5Capt;
                        if (U_IndKey5Frm != null) U_IndKey5Frm.Caption = OADM.U_Ind5Capt + " From";
                        if (U_IndKey5To != null) U_IndKey5To.Caption = OADM.U_Ind5Capt + " To";
                    }

                    /* Indicator 6 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind6Capt))
                    {
                        IModelColumn U_IndKey6 = listView.Model.Columns["U_IndKey6"];
                        IModelColumn U_IndKey6Frm = listView.Model.Columns["U_IndKey6Frm"];
                        IModelColumn U_IndKey6To = listView.Model.Columns["U_IndKey6To"];
                        if (U_IndKey6 != null) U_IndKey6.Caption = OADM.U_Ind6Capt;
                        if (U_IndKey6Frm != null) U_IndKey6Frm.Caption = OADM.U_Ind6Capt + " From";
                        if (U_IndKey6To != null) U_IndKey6To.Caption = OADM.U_Ind6Capt + " To";
                    }
                }

                if (View is DetailView)
                {
                    DetailView detailView = (DetailView)View;

                    /* Project (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_PrjCapt))
                    {
                        PropertyEditor U_Project = detailView.FindItem("U_Project") as PropertyEditor;
                        PropertyEditor U_ProjectFrm = detailView.FindItem("U_ProjectFrm") as PropertyEditor;
                        PropertyEditor U_ProjectTo = detailView.FindItem("U_ProjectTo") as PropertyEditor;
                        if (U_Project != null) U_Project.Caption = OADM.U_PrjCapt;
                        if (U_ProjectFrm != null) U_ProjectFrm.Caption = OADM.U_PrjCapt + " From";
                        if (U_ProjectTo != null) U_ProjectTo.Caption = OADM.U_PrjCapt + " To";
                    }

                    /* Cost Centre D1 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr1Capt))
                    {
                        PropertyEditor U_ProfitCode = detailView.FindItem("U_ProfitCode") as PropertyEditor;
                        PropertyEditor U_ProfitCodeFrm = detailView.FindItem("U_ProfitCodeFrm") as PropertyEditor;
                        PropertyEditor U_ProfitCodeTo = detailView.FindItem("U_ProfitCodeTo") as PropertyEditor;
                        if (U_ProfitCode != null) U_ProfitCode.Caption = OADM.U_Ocr1Capt;
                        if (U_ProfitCodeFrm != null) U_ProfitCodeFrm.Caption = OADM.U_Ocr1Capt + " From";
                        if (U_ProfitCodeTo != null) U_ProfitCodeTo.Caption = OADM.U_Ocr1Capt + " To";
                    }

                    /* Cost Centre D2 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr2Capt))
                    {
                        PropertyEditor U_OcrCode2 = detailView.FindItem("U_OcrCode2") as PropertyEditor;
                        PropertyEditor U_OcrCode2Frm = detailView.FindItem("U_OcrCode2Frm") as PropertyEditor;
                        PropertyEditor U_OcrCode2To = detailView.FindItem("U_OcrCode2To") as PropertyEditor;
                        if (U_OcrCode2 != null) U_OcrCode2.Caption = OADM.U_Ocr2Capt;
                        if (U_OcrCode2Frm != null) U_OcrCode2Frm.Caption = OADM.U_Ocr2Capt + " From";
                        if (U_OcrCode2To != null) U_OcrCode2To.Caption = OADM.U_Ocr2Capt + " To";
                    }

                    /* Cost Centre D3 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr3Capt))
                    {
                        PropertyEditor U_OcrCode3 = detailView.FindItem("U_OcrCode3") as PropertyEditor;
                        PropertyEditor U_OcrCode3Frm = detailView.FindItem("U_OcrCode3Frm") as PropertyEditor;
                        PropertyEditor U_OcrCode3To = detailView.FindItem("U_OcrCode3To") as PropertyEditor;
                        if (U_OcrCode3 != null) U_OcrCode3.Caption = OADM.U_Ocr3Capt;
                        if (U_OcrCode3Frm != null) U_OcrCode3Frm.Caption = OADM.U_Ocr3Capt + " From";
                        if (U_OcrCode3To != null) U_OcrCode3To.Caption = OADM.U_Ocr3Capt + " To";
                    }

                    /* Cost Centre D4 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr4Capt))
                    {
                        PropertyEditor U_OcrCode4 = detailView.FindItem("U_OcrCode4") as PropertyEditor;
                        PropertyEditor U_OcrCode4Frm = detailView.FindItem("U_OcrCode4Frm") as PropertyEditor;
                        PropertyEditor U_OcrCode4To = detailView.FindItem("U_OcrCode4To") as PropertyEditor;
                        if (U_OcrCode4 != null) U_OcrCode4.Caption = OADM.U_Ocr4Capt;
                        if (U_OcrCode4Frm != null) U_OcrCode4Frm.Caption = OADM.U_Ocr4Capt + " From";
                        if (U_OcrCode4To != null) U_OcrCode4To.Caption = OADM.U_Ocr4Capt + " To";
                    }

                    /* Cost Centre D5 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ocr5Capt))
                    {
                        PropertyEditor U_OcrCode5 = detailView.FindItem("U_OcrCode5") as PropertyEditor;
                        PropertyEditor U_OcrCode5Frm = detailView.FindItem("U_OcrCode5Frm") as PropertyEditor;
                        PropertyEditor U_OcrCode5To = detailView.FindItem("U_OcrCode5To") as PropertyEditor;
                        if (U_OcrCode5 != null) U_OcrCode5.Caption = OADM.U_Ocr5Capt;
                        if (U_OcrCode5Frm != null) U_OcrCode5Frm.Caption = OADM.U_Ocr5Capt + " From";
                        if (U_OcrCode5To != null) U_OcrCode5To.Caption = OADM.U_Ocr5Capt + " To";
                    }

                    /* Indicator 1 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind1Capt))
                    {
                        PropertyEditor U_IndKey1 = detailView.FindItem("U_IndKey1") as PropertyEditor;
                        PropertyEditor U_IndKey1Frm = detailView.FindItem("U_IndKey1Frm") as PropertyEditor;
                        PropertyEditor U_IndKey1To = detailView.FindItem("U_IndKey1To") as PropertyEditor;
                        if (U_IndKey1 != null) U_IndKey1.Caption = OADM.U_Ind1Capt;
                        if (U_IndKey1Frm != null) U_IndKey1Frm.Caption = OADM.U_Ind1Capt + " From";
                        if (U_IndKey1To != null) U_IndKey1To.Caption = OADM.U_Ind1Capt + " To";
                    }

                    /* Indicator 2 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind2Capt))
                    {
                        PropertyEditor U_IndKey2 = detailView.FindItem("U_IndKey2") as PropertyEditor;
                        PropertyEditor U_IndKey2Frm = detailView.FindItem("U_IndKey2Frm") as PropertyEditor;
                        PropertyEditor U_IndKey2To = detailView.FindItem("U_IndKey2To") as PropertyEditor;
                        if (U_IndKey2 != null) U_IndKey2.Caption = OADM.U_Ind2Capt;
                        if (U_IndKey2Frm != null) U_IndKey2Frm.Caption = OADM.U_Ind2Capt + " From";
                        if (U_IndKey2To != null) U_IndKey2To.Caption = OADM.U_Ind2Capt + " To";
                    }

                    /* Indicator 3 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind3Capt))
                    {
                        PropertyEditor U_IndKey3 = detailView.FindItem("U_IndKey3") as PropertyEditor;
                        PropertyEditor U_IndKey3Frm = detailView.FindItem("U_IndKey3Frm") as PropertyEditor;
                        PropertyEditor U_IndKey3To = detailView.FindItem("U_IndKey3To") as PropertyEditor;
                        if (U_IndKey3 != null) U_IndKey3.Caption = OADM.U_Ind3Capt;
                        if (U_IndKey3Frm != null) U_IndKey3Frm.Caption = OADM.U_Ind3Capt + " From";
                        if (U_IndKey3To != null) U_IndKey3To.Caption = OADM.U_Ind3Capt + " To";
                    }

                    /* Indicator 4 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind4Capt))
                    {
                        PropertyEditor U_IndKey4 = detailView.FindItem("U_IndKey4") as PropertyEditor;
                        PropertyEditor U_IndKey4Frm = detailView.FindItem("U_IndKey4Frm") as PropertyEditor;
                        PropertyEditor U_IndKey4To = detailView.FindItem("U_IndKey4To") as PropertyEditor;
                        if (U_IndKey4 != null) U_IndKey4.Caption = OADM.U_Ind4Capt;
                        if (U_IndKey4Frm != null) U_IndKey4Frm.Caption = OADM.U_Ind4Capt + " From";
                        if (U_IndKey4To != null) U_IndKey4To.Caption = OADM.U_Ind4Capt + " To";
                    }

                    /* Indicator 5 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind5Capt))
                    {
                        PropertyEditor U_IndKey5 = detailView.FindItem("U_IndKey5") as PropertyEditor;
                        PropertyEditor U_IndKey5Frm = detailView.FindItem("U_IndKey5Frm") as PropertyEditor;
                        PropertyEditor U_IndKey5To = detailView.FindItem("U_IndKey5To") as PropertyEditor;
                        if (U_IndKey5 != null) U_IndKey5.Caption = OADM.U_Ind5Capt;
                        if (U_IndKey5Frm != null) U_IndKey5Frm.Caption = OADM.U_Ind5Capt + " From";
                        if (U_IndKey5To != null) U_IndKey5To.Caption = OADM.U_Ind5Capt + " To";
                    }

                    /* Indicator 6 (Company Setup caption) */
                    if (!string.IsNullOrEmpty(OADM.U_Ind6Capt))
                    {
                        PropertyEditor U_IndKey6 = detailView.FindItem("U_IndKey6") as PropertyEditor;
                        PropertyEditor U_IndKey6Frm = detailView.FindItem("U_IndKey6Frm") as PropertyEditor;
                        PropertyEditor U_IndKey6To = detailView.FindItem("U_IndKey6To") as PropertyEditor;
                        if (U_IndKey6 != null) U_IndKey6.Caption = OADM.U_Ind6Capt;
                        if (U_IndKey6Frm != null) U_IndKey6Frm.Caption = OADM.U_Ind6Capt + " From";
                        if (U_IndKey6To != null) U_IndKey6To.Caption = OADM.U_Ind6Capt + " To";
                    }
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
