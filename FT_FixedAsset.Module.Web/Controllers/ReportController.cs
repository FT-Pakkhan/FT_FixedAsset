using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using FT_FixedAsset.Module.BusinessObjects;
using FT_FixedAsset.Module.Controllers;
using SAPbobsCOM;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace FT_FixedAsset.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ReportController : ViewController
    {
        GenController genCon;

        public ReportController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            #region ButtonActive
            Preview.Active.SetItemValue("Active", false);
            PostTrans.Active.SetItemValue("Active", false);

            if (View.ObjectTypeInfo.Type == typeof(PostWidget))
            {
                Preview.Active.SetItemValue("Active", true);
                Preview.Caption = "Preview";
            }

            if (View.ObjectTypeInfo.Type == typeof(ReportViewer))
            {
                Preview.Active.SetItemValue("Active", true);
                Preview.Caption = "Preview";
            }

            #endregion
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GenController>();
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void Preview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string browserPath = "";
            string HTTPSecure = "";

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["HTTPSecure"].ToString()))
                throw new InvalidOperationException("HTTPSecure Key cannot be found in webconfig.");



            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BrowserPath"].ToString()))
                browserPath = ConfigurationManager.AppSettings["BrowserPath"].ToString();
            else
            {
                if (HttpContext.Current.Request.ApplicationPath == "/")
                    browserPath = HttpContext.Current.Request.Url.Authority;
                else
                    browserPath = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
            }

            if (HttpContext.Current.Request.IsLocal && browserPath.Contains("localhost"))
                HTTPSecure = "http";
            else
                HTTPSecure = ConfigurationManager.AppSettings["HTTPSecure"].ToString();

            if (View.ObjectTypeInfo.Type == typeof(PostWidget))
            {
                PostWidget JEPreview = View.CurrentObject as PostWidget;
                HttpContext.Current.Session["PreviewObj"] = "PW";

                //Block if Date from more earlier than Date to
                if (JEPreview.DateFrm > JEPreview.DateTo)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Invalid Date From and Date To.");
                    return;
                }

                //Block if series not selected
                if (JEPreview.Series == null)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Please Select Series.");
                    return;
                }

                if (Preview.Caption == "Preview")
                {
                    PostTrans.Active.SetItemValue("Active", true);
                    Preview.Caption = "Back";

                    //Change to View Mode
                    ((DetailView)View).ViewEditMode = ViewEditMode.View;
                    View.BreakLinksToControls();
                    View.CreateControls();

                    HttpContext.Current.Session["Connection"] = string.Format(genCon.getConnectionString());
                    HttpContext.Current.Session["ReportName"] = "Journal Entry Preview";

                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11OADM OADM = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = 1"));

                    //Prepare JE Preview
                    genCon.prepareJEPreview(JEPreview, OADM);

                    if (!string.IsNullOrEmpty(OADM.U_CRPath))
                    {
                        HttpContext.Current.Session["ReportPath"] = OADM.U_CRPath + "fts11report_jepreview.rpt";
                    }
                    else
                    {
                        HttpContext.Current.Session["ReportPath"] = HttpContext.Current.Server.MapPath("~\\FAReports\\") + "fts11report_jepreview.rpt";
                    }

                    HttpContext.Current.Session["DateFrm"] = JEPreview.DateFrm.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : JEPreview.DateFrm.ToString("yyyy-MM-dd");
                    HttpContext.Current.Session["DateTo"] = JEPreview.DateTo.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : JEPreview.DateTo.ToString("yyyy-MM-dd");
                    HttpContext.Current.Session["FACodFrm"] = JEPreview.FACodeFrm == null ? "" : JEPreview.FACodeFrm.U_FACode;
                    HttpContext.Current.Session["FACodTo"] = JEPreview.FACodeTo == null ? "" : JEPreview.FACodeTo.U_FACode;
                    HttpContext.Current.Session["currency"] = "LC";
                    HttpContext.Current.Session["FAGrpFrm"] = JEPreview.FAGrpFrm == null ? "" : JEPreview.FAGrpFrm.U_FAGrpCod;
                    HttpContext.Current.Session["FAGrpTo"] = JEPreview.FAGrpTo == null ? "" : JEPreview.FAGrpTo.U_FAGrpCod;
                    HttpContext.Current.Session["FAClsFrm"] = JEPreview.FAClsFrm == null ? "" : JEPreview.FAClsFrm.Code;
                    HttpContext.Current.Session["FAClsTo"] = JEPreview.FAClsTo == null ? "" : JEPreview.FAClsTo.Code;
                    HttpContext.Current.Session["JESplitBy"] = OADM.U_SplitBy == null ? "" : OADM.U_SplitBy.Code;
                    HttpContext.Current.Session["Option"] = JEPreview.PostOpt == null ? "" : JEPreview.PostOpt.Code;
                    HttpContext.Current.Session["PostTo"] = JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code;
                    HttpContext.Current.Session["FileType"] = JEPreview.FileType == null ? "" : JEPreview.FileType.Code;
                    HttpContext.Current.Session["Series"] = JEPreview.Series == null ? "" : JEPreview.Series.Series;
                    HttpContext.Current.Session["RefDate"] = JEPreview.DocDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : JEPreview.DocDate.ToString("yyyy-MM-dd");
                    HttpContext.Current.Session["Ref1"] = string.IsNullOrEmpty(JEPreview.Ref1) ? "" : JEPreview.Ref1.ToString();
                    HttpContext.Current.Session["Ref2"] = string.IsNullOrEmpty(JEPreview.Ref2) ? "" : JEPreview.Ref2.ToString();
                    HttpContext.Current.Session["Memo"] = string.IsNullOrEmpty(JEPreview.JEMemo) ? "" : JEPreview.JEMemo.ToString();
                    HttpContext.Current.Session["UserID"] = string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString();

                    WebWindow.CurrentRequestWindow.RegisterStartupScript("Preview",
                        "window.open('" + HTTPSecure + "://" + browserPath + @"/Reports.aspx','_blank');", true);
                }
                else
                {
                    PostTrans.Active.SetItemValue("Active", false);
                    Preview.Caption = "Preview";

                    //Change to Edit Mode
                    ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                    View.BreakLinksToControls();
                    View.CreateControls();
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(ReportViewer))
            {
                ReportViewer reportViewer = View.CurrentObject as ReportViewer;
                HttpContext.Current.Session["PreviewObj"] = "RV";

                //Block if Date from more earlier than Date to
                if (reportViewer.DateFrm > reportViewer.DateTo)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Invalid Date From and Date To.");
                    return;
                }

                //Block if Create Date from more earlier than Create Date to
                if (reportViewer.CreateDateFrm > reportViewer.CreateDateTo)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Invalid Create Date From and Create Date To.");
                    return;
                }

                //Block if Acquire Date from more earlier than Acquire Date to
                if (reportViewer.AcqDateFrm > reportViewer.AcqDateTo)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Invalid Acq. Date From and Acq. Date To.");
                    return;
                }

                HttpContext.Current.Session["Connection"] = string.Format(genCon.getConnectionString());
                HttpContext.Current.Session["ReportName"] = reportViewer.Report.Name;

                IObjectSpace os = Application.CreateObjectSpace();
                FTS11OADM OADM = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = 1"));

                if (!string.IsNullOrEmpty(OADM.U_CRPath))
                {
                    HttpContext.Current.Session["ReportPath"] = OADM.U_CRPath + reportViewer.Report.DWName;
                }
                else
                {
                    HttpContext.Current.Session["ReportPath"] = HttpContext.Current.Server.MapPath("~\\FAReports\\") + reportViewer.Report.DWName;
                }

                HttpContext.Current.Session["Group1"] = reportViewer.Group1 == null ? "" : reportViewer.Group1.Code;
                HttpContext.Current.Session["Group2"] = reportViewer.Group2 == null ? "" : reportViewer.Group2.Code;
                HttpContext.Current.Session["TransDateFrm"] = reportViewer.DateFrm.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : reportViewer.DateFrm.ToString("yyyy-MM-dd");
                HttpContext.Current.Session["TransDateTo"] = reportViewer.DateTo.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : reportViewer.DateTo.ToString("yyyy-MM-dd");
                HttpContext.Current.Session["AcqDateFrm"] = reportViewer.AcqDateFrm.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : reportViewer.AcqDateFrm.ToString("yyyy-MM-dd");
                HttpContext.Current.Session["AcqDateTo"] = reportViewer.AcqDateTo.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : reportViewer.AcqDateTo.ToString("yyyy-MM-dd");
                HttpContext.Current.Session["CreateDateFrm"] = reportViewer.CreateDateFrm.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : reportViewer.CreateDateFrm.ToString("yyyy-MM-dd");
                HttpContext.Current.Session["CreateDateTo"] = reportViewer.CreateDateTo.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : reportViewer.CreateDateTo.ToString("yyyy-MM-dd");
                HttpContext.Current.Session["FACodeFrm"] = reportViewer.FACodeFrmKey == null ? "" : reportViewer.FACodeFrmKey.U_FACode;
                HttpContext.Current.Session["FACodeTo"] = reportViewer.FACodeToKey == null ? "" : reportViewer.FACodeToKey.U_FACode;
                HttpContext.Current.Session["currency"] = "LC";
                HttpContext.Current.Session["FAGrpFrm"] = reportViewer.FAGrpFrmKey == null ? "" : reportViewer.FAGrpFrmKey.U_FAGrpCod;
                HttpContext.Current.Session["FAGrpTo"] = reportViewer.FAGrpToKey == null ? "" : reportViewer.FAGrpToKey.U_FAGrpCod;
                HttpContext.Current.Session["FAClsFrm"] = reportViewer.FAClsFrmKey == null ? "" : reportViewer.FAClsFrmKey.Code;
                HttpContext.Current.Session["FAClsTo"] = reportViewer.FAClsToKey == null ? "" : reportViewer.FAClsToKey.Code;
                HttpContext.Current.Session["U_LocFrmKey"] = reportViewer.U_LocFrmKey == null ? "" : reportViewer.U_LocFrmKey.Code;
                HttpContext.Current.Session["U_LocToKey"] = reportViewer.U_LocToKey == null ? "" : reportViewer.U_LocToKey.Code;
                HttpContext.Current.Session["NBV_Frm"] = reportViewer.NBV_Frm == 0 ? -10000 : reportViewer.NBV_Frm;
                HttpContext.Current.Session["NBV_To"] = reportViewer.NBV_To == 0 ? -10000 : reportViewer.NBV_To;
                HttpContext.Current.Session["QtyFrom"] = reportViewer.QtyFrom == 0 ? -10000 : reportViewer.QtyFrom;
                HttpContext.Current.Session["QtyTo"] = reportViewer.QtyTo == 0 ? -10000 : reportViewer.QtyTo;
                HttpContext.Current.Session["U_AccFrm"] = reportViewer.U_AccFrm == null ? "" : reportViewer.U_AccFrm.AcctCode;
                HttpContext.Current.Session["U_AccTo"] = reportViewer.U_AccTo == null ? "" : reportViewer.U_AccTo.AcctCode;
                HttpContext.Current.Session["U_ProjectFrm"] = reportViewer.U_ProjectFrm == null ? "" : reportViewer.U_ProjectFrm.PrjCode;
                HttpContext.Current.Session["U_ProjectTo"] = reportViewer.U_ProjectTo == null ? "" : reportViewer.U_ProjectTo.PrjCode;
                HttpContext.Current.Session["U_ProfitCodeFrm"] = reportViewer.U_ProfitCodeFrm == null ? "" : reportViewer.U_ProfitCodeFrm.PrcCode;
                HttpContext.Current.Session["U_ProfitCodeTo"] = reportViewer.U_ProfitCodeTo == null ? "" : reportViewer.U_ProfitCodeTo.PrcCode;
                HttpContext.Current.Session["U_OcrCode2Frm"] = reportViewer.U_OcrCode2Frm == null ? "" : reportViewer.U_OcrCode2Frm.PrcCode;
                HttpContext.Current.Session["U_OcrCode2To"] = reportViewer.U_OcrCode2To == null ? "" : reportViewer.U_OcrCode2To.PrcCode;
                HttpContext.Current.Session["U_OcrCode3Frm"] = reportViewer.U_OcrCode3Frm == null ? "" : reportViewer.U_OcrCode3Frm.PrcCode;
                HttpContext.Current.Session["U_OcrCode3To"] = reportViewer.U_OcrCode3To == null ? "" : reportViewer.U_OcrCode3To.PrcCode;
                HttpContext.Current.Session["U_OcrCode4Frm"] = reportViewer.U_OcrCode4Frm == null ? "" : reportViewer.U_OcrCode4Frm.PrcCode;
                HttpContext.Current.Session["U_OcrCode4To"] = reportViewer.U_OcrCode4To == null ? "" : reportViewer.U_OcrCode4To.PrcCode;
                HttpContext.Current.Session["U_OcrCode5Frm"] = reportViewer.U_OcrCode5Frm == null ? "" : reportViewer.U_OcrCode5Frm.PrcCode;
                HttpContext.Current.Session["U_OcrCode5To"] = reportViewer.U_OcrCode5To == null ? "" : reportViewer.U_OcrCode5To.PrcCode;
                HttpContext.Current.Session["U_IndKey1Frm"] = reportViewer.U_IndKey1Frm == null ? "" : reportViewer.U_IndKey1Frm.U_IndCode;
                HttpContext.Current.Session["U_IndKey1To"] = reportViewer.U_IndKey1To == null ? "" : reportViewer.U_IndKey1To.U_IndCode;
                HttpContext.Current.Session["U_IndKey2Frm"] = reportViewer.U_IndKey2Frm == null ? "" : reportViewer.U_IndKey2Frm.U_IndCode;
                HttpContext.Current.Session["U_IndKey2To"] = reportViewer.U_IndKey2To == null ? "" : reportViewer.U_IndKey2To.U_IndCode;
                HttpContext.Current.Session["U_IndKey3Frm"] = reportViewer.U_IndKey3Frm == null ? "" : reportViewer.U_IndKey3Frm.U_IndCode;
                HttpContext.Current.Session["U_IndKey3To"] = reportViewer.U_IndKey3To == null ? "" : reportViewer.U_IndKey3To.U_IndCode;
                HttpContext.Current.Session["U_IndKey4Frm"] = reportViewer.U_IndKey4Frm == null ? "" : reportViewer.U_IndKey4Frm.U_IndCode;
                HttpContext.Current.Session["U_IndKey4To"] = reportViewer.U_IndKey4To == null ? "" : reportViewer.U_IndKey4To.U_IndCode;
                HttpContext.Current.Session["U_IndKey5Frm"] = reportViewer.U_IndKey5Frm == null ? "" : reportViewer.U_IndKey5Frm.U_IndCode;
                HttpContext.Current.Session["U_IndKey5To"] = reportViewer.U_IndKey5To == null ? "" : reportViewer.U_IndKey5To.U_IndCode;
                HttpContext.Current.Session["U_IndKey6Frm"] = reportViewer.U_IndKey6Frm == null ? "" : reportViewer.U_IndKey6Frm.U_IndCode;
                HttpContext.Current.Session["U_IndKey6To"] = reportViewer.U_IndKey6To == null ? "" : reportViewer.U_IndKey6To.U_IndCode;
                HttpContext.Current.Session["U_AccFrm"] = reportViewer.U_AccFrm == null ? "" : reportViewer.U_AccFrm.FormatCode;
                HttpContext.Current.Session["U_AccTo"] = reportViewer.U_AccTo == null ? "" : reportViewer.U_AccTo.FormatCode;

                WebWindow.CurrentRequestWindow.RegisterStartupScript("Preview",
                    "window.open('" + HTTPSecure + "://" + browserPath + @"/Reports.aspx','_blank');", true);
            }
        }

        private void PostTrans_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            PostWidget JEPreview = View.CurrentObject as PostWidget;

            IObjectSpace os = Application.CreateObjectSpace();
            FTS11OADM OADM = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = 1"));

            #region Connect to xaf DB

            string connectionString = genCon.getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmdsql = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region Post Document

            try
            {
                conn.Open();

                switch (OADM.U_SplitBy.ToString().ToUpper())
                {
                    case "FAGRPNAM":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAGrpNam + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAGrpNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit) <> 0 OR SUM(Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAGrpNam, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5 ";
                        break;

                    case "FACLSNAM":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5 ";
                        break;

                    case "FACLSNAM_PROFITCO":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + '-' + ProfitCo + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, ProfitCo, TransType, DebCred DESC, FormatCode, Project, OcrCode2, OcrCode3, OcrCode4, OcrCode5 ";
                        break;

                    case "FACLSNAM_OCRCODE2":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + '-' + OcrCode2 + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, OcrCode2, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode3, OcrCode4, OcrCode5 ";
                        break;

                    case "FACLSNAM_OCRCODE3":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + '-' + OcrCode3 + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, OcrCode3, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode2, OcrCode4, OcrCode5 ";
                        break;

                    case "FACLSNAM_OCRCODE4":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + '-' + OcrCode4 + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, OcrCode4, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode5 ";
                        break;

                    case "FACLSNAM_OCRCODE5":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + '-' + OcrCode5 + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, OcrCode5, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4 ";
                        break;

                    case "FACLSNAM_PROJECT":
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAClsNam + '-' + Project + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAClsNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAClsNam, Project, TransType, DebCred DESC, FormatCode, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5 ";
                        break;

                    default:
                        cmdsql.CommandText = "SELECT OJDTKey AS [Batch], '0' AS [Series], '' AS [Ref1], '' AS [Ref2],  '' AS [Memo], 'N' AS [AutoVat], " +
                            "LEFT(FAGrpNam + ' - ' + TransType,50) AS [LineMemo], AcctCode AS [ShortName], SUM(Debit - Credit) AS [Amount], 0 AS [BaseAmount], " +
                            "Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, '' AS [TaxCode]  " +
                            "FROM FTS11_vw_JDT1RPT WHERE TransStatus = 'O' AND OJDTKey <> 0 " +
                            "GROUP BY OJDTKey, FAGrpNam, DebCred, AcctCode, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5, TransType " +
                            "HAVING SUM(Debit - Credit) <> 0  " +
                            "ORDER BY OJDTKey, FAGrpNam, TransType, DebCred DESC, FormatCode, Project, ProfitCo, OcrCode2, OcrCode3, OcrCode4, OcrCode5";
                        break;
                }

                SqlDataAdapter da = new SqlDataAdapter(cmdsql);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();

                if (dt.Rows.Count > 0)
                {
                    createJE(dt, JEPreview, OADM, conn, cmdsql);
                }
                else
                {
                    genCon.MsgBox(InformationType.Error, "Error", "No data to be posted.");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                genCon.MsgBox(InformationType.Error, "Error", ex.Message.ToString().Replace("'", ""));
                conn.Close();
                return;
            }
            #endregion

            genCon.UpdateIsFieldEditable(0);
        }

        public void createJE(DataTable dt, PostWidget JEPreview, FTS11OADM OADM, SqlConnection conn, SqlCommand cmdsql)
        {
            string transId = "", batch = "", postTo = "", fileType = "";
            double amount = 0, basesum = 0;

            SAPbobsCOM.JournalVouchers oJV = null;
            SAPbobsCOM.JournalEntries oJE = null;

            //Connect to SAP
            SAPbobsCOM.Company oCom = new SAPbobsCOM.Company();
            try
            {
                genCon.getSAPConfiguration(oCom, OADM);

                postTo = JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code;
                fileType = JEPreview.FileType == null ? "" : JEPreview.FileType.Code;

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        // New journal
                        if (batch != row["Batch"].ToString())
                        {
                            #region Save earlier journal
                            if (batch != "")
                            {
                                if (postTo == "SBO")
                                {
                                    if (fileType == "JV")
                                    {
                                        if (oJV.Add() != 0)
                                        {
                                            genCon.MsgBox(InformationType.Error, "Error", oCom.GetLastErrorDescription());
                                        }
                                        else
                                        {
                                            genCon.MsgBox(InformationType.Info, "Infomation", "Journal Voucher created");

                                            cmdsql = new SqlCommand("EXEC [dbo].[sp_FTS11_SetOJDTPostedStatus] '" +
                                                batch + "', '" +
                                                transId + "', '" +
                                                (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                                                (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "', '" +
                                                (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "'", conn);
                                            cmdsql.ExecuteNonQuery();
                                            cmdsql.Dispose();

                                        }
                                    }
                                    else
                                    {
                                        if (oJE.Add() != 0)
                                        {
                                            genCon.MsgBox(InformationType.Error, "Error", oCom.GetLastErrorDescription());
                                        }
                                        else
                                        {
                                            genCon.MsgBox(InformationType.Info, "Infomation", "Journal Entry created");

                                            cmdsql = new SqlCommand("EXEC [dbo].[sp_FTS11_SetOJDTPostedStatus] '" +
                                                batch + "', '" +
                                                transId + "', '" +
                                                (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                                                (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "', '" +
                                                (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "'", conn);
                                            cmdsql.ExecuteNonQuery();
                                            cmdsql.Dispose();

                                        }
                                    }
                                }
                                else
                                {
                                    oJE.SaveXML(Path.GetTempPath() + "FTS00JW_OJDT.xml");

                                    if (fileType == "JV")
                                        genCon.MsgBox(InformationType.Info, "Infomation", "Journal Entry created");
                                    else
                                        genCon.MsgBox(InformationType.Info, "Infomation", "Journal Voucher created");

                                    cmdsql = new SqlCommand("EXEC [dbo].[sp_FTS11_SetOJDTPostedStatus] '" +
                                        batch + "', '" +
                                        transId + "', '" +
                                        (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                                        (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "', '" +
                                        (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "'", conn);
                                    cmdsql.ExecuteNonQuery();
                                    cmdsql.Dispose();
                                }
                            }
                            batch = row["Batch"].ToString();
                            #endregion

                            #region initiate journal
                            if (fileType == "JV")
                            {
                                oJV = (SAPbobsCOM.JournalVouchers)oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
                                oJE = oJV.JournalEntries;
                            }
                            else
                            {
                                oJE = (SAPbobsCOM.JournalEntries)oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                            }

                            Console.WriteLine("Preparing journal...");
                            if ((JEPreview.Series.Series == "0" ? int.Parse(row["Series"].ToString()) : int.Parse(JEPreview.Series.Series)) != 0) oJE.Series = (JEPreview.Series.Series == "0" ? int.Parse(row["Series"].ToString()) : int.Parse(JEPreview.Series.Series));
                            oJE.ReferenceDate = DateTime.Parse(JEPreview.DocDate.ToString("yyyy-MM-dd"));
                            oJE.DueDate = oJE.ReferenceDate;
                            oJE.TaxDate = oJE.ReferenceDate;

                            oJE.Reference = string.IsNullOrEmpty(JEPreview.Ref1) ? "" : JEPreview.Ref1.ToString();
                            oJE.Reference2 = string.IsNullOrEmpty(JEPreview.Ref2) ? "" : JEPreview.Ref2.ToString();
                            oJE.Memo = string.IsNullOrEmpty(JEPreview.JEMemo) ? "" : JEPreview.JEMemo.ToString();
                            oJE.AutoVAT = (row["AutoVat"].ToString() == "N" ? BoYesNoEnum.tNO : BoYesNoEnum.tYES);
                            #endregion
                        }

                        #region Insert row to journal
                        if (oJE.Lines.ShortName != "") oJE.Lines.Add();
                        oJE.Lines.ShortName = row["ShortName"].ToString();
                        if (row["Project"].ToString() != "") oJE.Lines.ProjectCode = row["Project"].ToString();
                        if (row["ProfitCo"].ToString() != "") oJE.Lines.CostingCode = row["ProfitCo"].ToString();
                        if (row["OcrCode2"].ToString() != "") oJE.Lines.CostingCode2 = row["OcrCode2"].ToString();
                        if (row["OcrCode3"].ToString() != "") oJE.Lines.CostingCode3 = row["OcrCode3"].ToString();
                        if (row["OcrCode4"].ToString() != "") oJE.Lines.CostingCode4 = row["OcrCode4"].ToString();
                        if (row["OcrCode5"].ToString() != "") oJE.Lines.CostingCode5 = row["OcrCode5"].ToString();
                        if (row["TaxCode"].ToString() != "") oJE.Lines.TaxGroup = row["TaxCode"].ToString();
                        if (row["LineMemo"].ToString() != "") oJE.Lines.LineMemo = row["LineMemo"].ToString();

                        amount = Double.Parse(row["Amount"].ToString());
                        basesum = Double.Parse(row["BaseAmount"].ToString());
                        if (amount > 0) oJE.Lines.Debit = amount;
                        if (amount < 0) oJE.Lines.Credit = -amount;
                        //oJE.Lines.GrossValue = grossValue;
                        if (basesum != 0) oJE.Lines.BaseSum = basesum;
                        #endregion
                    }

                    #region Save last journal
                    Console.WriteLine("Add journal...");
                    if (postTo == "SBO")
                    {
                        if (fileType == "JV")
                        {
                            if (oJV.Add() != 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", oCom.GetLastErrorDescription());
                            }
                            else
                            {
                                genCon.MsgBox(InformationType.Info, "Infomation", "Journal Voucher created");

                                cmdsql = new SqlCommand("EXEC [dbo].[sp_FTS11_SetOJDTPostedStatus] '" +
                                    batch + "', '" +
                                    transId + "', '" +
                                    (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                                    (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "', '" +
                                    (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "'", conn);
                                cmdsql.ExecuteNonQuery();
                                cmdsql.Dispose();
                            }
                        }
                        else
                        {
                            if (oJE.Add() != 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", oCom.GetLastErrorDescription());
                            }
                            else
                            {
                                genCon.MsgBox(InformationType.Info, "Infomation", "Journal Entry created");

                                cmdsql = new SqlCommand("EXEC [dbo].[sp_FTS11_SetOJDTPostedStatus] '" +
                                    batch + "', '" +
                                    transId + "', '" +
                                    (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                                    (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "', '" +
                                    (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "'", conn);
                                cmdsql.ExecuteNonQuery();
                                cmdsql.Dispose();
                            }
                        }
                    }
                    else
                    {
                        oJE.SaveXML(Path.GetTempPath() + "FTS00JW_OJDT.xml");

                        if (fileType == "JV")
                            genCon.MsgBox(InformationType.Info, "Infomation", "Journal Entry created");
                        else
                            genCon.MsgBox(InformationType.Info, "Infomation", "Journal Voucher created");

                        cmdsql = new SqlCommand("EXEC [dbo].[sp_FTS11_SetOJDTPostedStatus] '" +
                            batch + "', '" +
                            transId + "', '" +
                            (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                            (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "', '" +
                            (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "'", conn);
                        cmdsql.ExecuteNonQuery();
                        cmdsql.Dispose();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    genCon.MsgBox(InformationType.Error, "Error", ex.Message.ToString());
                }
                //oCom.Disconnect();
            }
            catch (Exception ex)
            {
                genCon.MsgBox(InformationType.Error, "Error", ex.Message.ToString());
                //oCom.Disconnect();
            }

            //oCom = null;
        }

    }
}
