using CrystalDecisions.CrystalReports.Engine;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FT_FixedAsset.Web
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["PreviewObj"].ToString() == "RV")
                RVLoadReport();
            else
                PWLoadReport();

        }

        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["PreviewObj"].ToString() == "RV")
                RVLoadReport();
            else
                PWLoadReport();
        }

        public void PWLoadReport()
        {
            ReportDocument doc = new ReportDocument();
            Session["ReportSource"] = doc;
            doc.Load(HttpContext.Current.Session["ReportPath"].ToString());
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(HttpContext.Current.Session["Connection"].ToString());
            doc.DataSourceConnections[0].SetConnection(builder.DataSource, builder.InitialCatalog, builder.UserID, builder.Password);

            ParameterFieldDefinitions crParameterdef;
            crParameterdef = doc.DataDefinition.ParameterFields;

            //Check if parameter exists only assign else have to filter by report to assign
            foreach (ParameterFieldDefinition param in crParameterdef)
            {
                if (param.Name.Equals("DateFrom"))
                    doc.SetParameterValue("DateFrom", DateTime.Parse(HttpContext.Current.Session["DateFrm"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("DateTo"))
                    doc.SetParameterValue("DateTo", DateTime.Parse(HttpContext.Current.Session["DateTo"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("FACodFrm"))
                    doc.SetParameterValue("FACodFrm", HttpContext.Current.Session["FACodFrm"].ToString());

                if (param.Name.Equals("FACodTo"))
                    doc.SetParameterValue("FACodTo", HttpContext.Current.Session["FACodTo"].ToString());

                if (param.Name.Equals("FAGrpFrm"))
                    doc.SetParameterValue("FAGrpFrm", HttpContext.Current.Session["FAGrpFrm"].ToString());

                if (param.Name.Equals("FAGrpTo"))
                    doc.SetParameterValue("FAGrpTo", HttpContext.Current.Session["FAGrpTo"].ToString());

                if (param.Name.Equals("FAClsFrm"))
                    doc.SetParameterValue("FAClsFrm", HttpContext.Current.Session["FAClsFrm"].ToString());

                if (param.Name.Equals("FAClsTo"))
                    doc.SetParameterValue("FAClsTo", HttpContext.Current.Session["FAClsTo"].ToString());

                if (param.Name.Equals("JESplitBy"))
                    doc.SetParameterValue("JESplitBy", HttpContext.Current.Session["JESplitBy"]);

                if (param.Name.Equals("Option"))
                    doc.SetParameterValue("Option", HttpContext.Current.Session["Option"]);

                if (param.Name.Equals("PostTo"))
                    doc.SetParameterValue("PostTo", HttpContext.Current.Session["PostTo"]);

                if (param.Name.Equals("FileType"))
                    doc.SetParameterValue("FileType", HttpContext.Current.Session["FileType"]);

                if (param.Name.Equals("Series"))
                    doc.SetParameterValue("Series", HttpContext.Current.Session["Series"]);

                if (param.Name.Equals("RefDate"))
                    doc.SetParameterValue("RefDate", HttpContext.Current.Session["RefDate"]);

                if (param.Name.Equals("Ref1"))
                    doc.SetParameterValue("Ref1", HttpContext.Current.Session["Ref1"]);

                if (param.Name.Equals("Ref2"))
                    doc.SetParameterValue("Memo", HttpContext.Current.Session["Ref2"]);

                if (param.Name.Equals("Memo"))
                    doc.SetParameterValue("Memo", HttpContext.Current.Session["Memo"]);

                if (param.Name.Equals("UserID"))
                    doc.SetParameterValue("UserID", HttpContext.Current.Session["UserID"]);
            }

            if (Session["ReportSource"] != null)
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["ReportSource"];
            else
                CrystalReportViewer1.ReportSource = doc;

            CrystalReportViewer1.ID = HttpContext.Current.Session["ReportName"].ToString();

        }

        public void RVLoadReport()
        {
            ReportDocument doc = new ReportDocument();
            Session["ReportSource"] = doc;
            doc.Load(HttpContext.Current.Session["ReportPath"].ToString());
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(HttpContext.Current.Session["Connection"].ToString());
            doc.DataSourceConnections[0].SetConnection(builder.DataSource, builder.InitialCatalog, builder.UserID, builder.Password);

            ParameterFieldDefinitions crParameterdef;
            crParameterdef = doc.DataDefinition.ParameterFields;

            //Check if parameter exists only assign else have to filter by report to assign
            foreach (ParameterFieldDefinition param in crParameterdef)
            {
                if (param.Name.Equals("Group1"))
                    doc.SetParameterValue("Group1", HttpContext.Current.Session["Group1"].ToString());

                if (param.Name.Equals("Group2"))
                    doc.SetParameterValue("Group2", HttpContext.Current.Session["Group2"].ToString());

                if (param.Name.Equals("currency"))
                    doc.SetParameterValue("currency", "");

                if (param.Name.Equals("datefrom"))
                    doc.SetParameterValue("datefrom", DateTime.Parse(HttpContext.Current.Session["TransDateFrm"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("dateto"))
                    doc.SetParameterValue("dateto", DateTime.Parse(HttpContext.Current.Session["TransDateto"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("acqdatefrom"))
                    doc.SetParameterValue("acqdatefrom", DateTime.Parse(HttpContext.Current.Session["AcqDateFrm"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("acqdateto"))
                    doc.SetParameterValue("acqdateto", DateTime.Parse(HttpContext.Current.Session["AcqDateto"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("createdatefrom"))
                    doc.SetParameterValue("createdatefrom", DateTime.Parse(HttpContext.Current.Session["CreateDateFrm"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("createdateto"))
                    doc.SetParameterValue("createdateto", DateTime.Parse(HttpContext.Current.Session["CreateDateto"].ToString()).ToString("yyyy-MM-dd"));

                if (param.Name.Equals("facodefrom"))
                    doc.SetParameterValue("facodefrom", HttpContext.Current.Session["FACodeFrm"].ToString());

                if (param.Name.Equals("facodeto"))
                    doc.SetParameterValue("facodeto", HttpContext.Current.Session["FACodeto"].ToString());

                if (param.Name.Equals("fagrpcodfrom"))
                    doc.SetParameterValue("fagrpcodfrom", HttpContext.Current.Session["FAGrpFrm"].ToString());

                if (param.Name.Equals("fagrpcodto"))
                    doc.SetParameterValue("fagrpcodto", HttpContext.Current.Session["FAGrpto"].ToString());

                if (param.Name.Equals("faclscodfrom"))
                    doc.SetParameterValue("faclscodfrom", HttpContext.Current.Session["FAClsFrm"].ToString());

                if (param.Name.Equals("faclscodto"))
                    doc.SetParameterValue("faclscodto", HttpContext.Current.Session["FAClsto"].ToString());

                if (param.Name.Equals("locationfrom"))
                    doc.SetParameterValue("locationfrom", HttpContext.Current.Session["U_LocFrmKey"].ToString());

                if (param.Name.Equals("locationto"))
                    doc.SetParameterValue("locationto", HttpContext.Current.Session["U_LocToKey"].ToString());

                if (param.Name.Equals("nbvfrom"))
                    doc.SetParameterValue("nbvfrom", HttpContext.Current.Session["NBV_Frm"].ToString());

                if (param.Name.Equals("nbvto"))
                    doc.SetParameterValue("nbvto", HttpContext.Current.Session["NBV_to"].ToString());

                if (param.Name.Equals("qtyfrom"))
                    doc.SetParameterValue("qtyfrom", HttpContext.Current.Session["Qtyfrom"].ToString());

                if (param.Name.Equals("qtyto"))
                    doc.SetParameterValue("qtyto", HttpContext.Current.Session["Qtyto"].ToString());

                if (param.Name.Equals("projectfrom"))
                    doc.SetParameterValue("projectfrom", HttpContext.Current.Session["U_ProjectFrm"].ToString());

                if (param.Name.Equals("projectto"))
                    doc.SetParameterValue("projectto", HttpContext.Current.Session["U_Projectto"].ToString());

                if (param.Name.Equals("profitcofrom"))
                    doc.SetParameterValue("profitcofrom", HttpContext.Current.Session["U_ProfitCodeFrm"].ToString());

                if (param.Name.Equals("profitcoto"))
                    doc.SetParameterValue("profitcoto", HttpContext.Current.Session["U_ProfitCodeto"].ToString());

                if (param.Name.Equals("ocrcode2from"))
                    doc.SetParameterValue("ocrcode2from", HttpContext.Current.Session["U_OcrCode2Frm"].ToString());

                if (param.Name.Equals("ocrcode2to"))
                    doc.SetParameterValue("ocrcode2to", HttpContext.Current.Session["U_OcrCode2to"].ToString());

                if (param.Name.Equals("ocrcode3from"))
                    doc.SetParameterValue("ocrcode3from", HttpContext.Current.Session["U_OcrCode3Frm"].ToString());

                if (param.Name.Equals("ocrcode3to"))
                    doc.SetParameterValue("ocrcode3to", HttpContext.Current.Session["U_OcrCode3to"].ToString());

                if (param.Name.Equals("ocrcode4from"))
                    doc.SetParameterValue("ocrcode4from", HttpContext.Current.Session["U_OcrCode4Frm"].ToString());

                if (param.Name.Equals("ocrcode4to"))
                    doc.SetParameterValue("ocrcode4to", HttpContext.Current.Session["U_OcrCode4to"].ToString());

                if (param.Name.Equals("ocrcode5from"))
                    doc.SetParameterValue("ocrcode5from", HttpContext.Current.Session["U_OcrCode5Frm"].ToString());

                if (param.Name.Equals("ocrcode5to"))
                    doc.SetParameterValue("ocrcode5to", HttpContext.Current.Session["U_OcrCode5to"].ToString());

                if (param.Name.Equals("ind1codefrom"))
                    doc.SetParameterValue("ind1codefrom", HttpContext.Current.Session["U_IndKey1Frm"].ToString());

                if (param.Name.Equals("ind1codeto"))
                    doc.SetParameterValue("ind1codeto", HttpContext.Current.Session["U_IndKey1to"].ToString());

                if (param.Name.Equals("ind2codefrom"))
                    doc.SetParameterValue("ind2codefrom", HttpContext.Current.Session["U_IndKey2Frm"].ToString());

                if (param.Name.Equals("ind2codeto"))
                    doc.SetParameterValue("ind2codeto", HttpContext.Current.Session["U_IndKey2to"].ToString());

                if (param.Name.Equals("ind3codefrom"))
                    doc.SetParameterValue("ind3codefrom", HttpContext.Current.Session["U_IndKey3Frm"].ToString());

                if (param.Name.Equals("ind3codeto"))
                    doc.SetParameterValue("ind3codeto", HttpContext.Current.Session["U_IndKey3to"].ToString());

                if (param.Name.Equals("ind4codefrom"))
                    doc.SetParameterValue("ind4codefrom", HttpContext.Current.Session["U_IndKey4Frm"].ToString());

                if (param.Name.Equals("ind4codeto"))
                    doc.SetParameterValue("ind4codeto", HttpContext.Current.Session["U_IndKey4to"].ToString());

                if (param.Name.Equals("ind5codefrom"))
                    doc.SetParameterValue("ind5codefrom", HttpContext.Current.Session["U_IndKey5Frm"].ToString());

                if (param.Name.Equals("ind5codeto"))
                    doc.SetParameterValue("ind5codeto", HttpContext.Current.Session["U_IndKey5to"].ToString());

                if (param.Name.Equals("ind6codefrom"))
                    doc.SetParameterValue("ind6codefrom", HttpContext.Current.Session["U_IndKey6Frm"].ToString());

                if (param.Name.Equals("ind6codeto"))
                    doc.SetParameterValue("ind6codeto", HttpContext.Current.Session["U_IndKey6to"].ToString());

                if (param.Name.Equals("formatcodefrom"))
                    doc.SetParameterValue("formatcodefrom", HttpContext.Current.Session["U_AccFrm"].ToString());

                if (param.Name.Equals("formatcodeto"))
                    doc.SetParameterValue("formatcodeto", HttpContext.Current.Session["U_AccTo"].ToString());
            }

            if (Session["ReportSource"] != null)
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["ReportSource"];
            else
                CrystalReportViewer1.ReportSource = doc;

            CrystalReportViewer1.ID = HttpContext.Current.Session["ReportName"].ToString();
        }
    }
}