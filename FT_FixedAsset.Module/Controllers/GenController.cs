using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo.DB.Helpers;
using FT_FixedAsset.Module.BusinessObjects;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GenController : ViewController
    {
        public GenController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        public void MsgBox(InformationType infoType, string caption, string PopUpMsg)
        {
            try
            {
                MessageOptions options = new MessageOptions();
                options.Duration = 7000;
                options.Message = string.Format(PopUpMsg);
                options.Type = infoType;
                options.Web.Position = InformationPosition.Right;
                options.Win.Caption = caption;
                options.Win.Type = WinMessageType.Flyout;
                Application.ShowViewStrategy.ShowMessage(options);
            }
            catch
            {
                throw new Exception(PopUpMsg);
            }
        }

        public string getConnectionString()
        {
            string connectionString = "";

            ConnectionStringParser helper = new ConnectionStringParser(Application.ConnectionString);
            helper.RemovePartByName("xpodatastorepool");
            connectionString = string.Format(helper.GetConnectionString());

            return connectionString;
        }

        public bool TransactionNotification(string query)
        {
            #region Connect to xaf DB

            bool returnedResult = false;
            string connectionString = getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand TransactionNotification = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region Transaction Notification
            

            try
            {
                conn.Open();

                TransactionNotification.CommandText = query;
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(TransactionNotification);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                #endregion

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        goto CatchResult;
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MsgBox(InformationType.Error, "Error", ex.Message);
                conn.Close();
                goto CatchResult;
            }
            return returnedResult;

        CatchResult:
            return returnedResult = true;
            #endregion
        }

        public void PostTransactionNotice(string objType, string transType, int docEntry)
        {
            #region Connect to xaf DB

            string PopUpMsg = "";
            string connectionString = getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand TransactionNotification = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region Post Transaction Notification


            try
            {
                conn.Open();

                TransactionNotification.CommandText = "EXEC SBO_SP_PostTransactionNotice '" + objType + "', '" + transType + "', 1, 'DocEntry' , '" + docEntry + "'";
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(TransactionNotification);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                #endregion

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() != "0")
                        {
                            PopUpMsg = ("(-" + row[0].ToString() + ") " + row[1].ToString());
                            MsgBox(InformationType.Error, "Error", PopUpMsg);
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                PopUpMsg = ex.Message;
                MsgBox(InformationType.Error, "Error", PopUpMsg);
                conn.Close();
            }
            #endregion
        }

        public void RecordChangeLog(string objType, int docEntry)
        {
            #region Connect to xaf DB

            string PopUpMsg = "";
            string connectionString = getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand TransactionNotification = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region RecordChangeLog


            try
            {
                conn.Open();

                TransactionNotification.CommandText = "EXEC sp_FTS11_RecordChangeLog '" + objType + "', 1, 'DocEntry', '" + docEntry + "' , 1";
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(TransactionNotification);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                #endregion

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        if (row[0].ToString() != "0")
                        {
                            PopUpMsg = ("(-" + row[0].ToString() + ") " + row[1].ToString());
                            MsgBox(InformationType.Error, "Error", PopUpMsg);
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                PopUpMsg = ex.Message;
                MsgBox(InformationType.Error, "Error", PopUpMsg);
                conn.Close();
            }
            #endregion
        }

        public void GenerateDeprn(DateTime DateFrm, DateTime DateTo, int FAKey, string UserCode)
        {
            #region Connect to xaf DB

            string PopUpMsg = "";
            string connectionString = getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand TransactionNotification = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region Generate Depreciation
            try
            {
                conn.Open();

                TransactionNotification.CommandText = "EXEC sp_FTS11_GenerateDeprn '" + DateFrm.ToString("yyyyMMdd") + "', '" + DateTo.ToString("yyyyMMdd") + 
                    "', '" + FAKey + "', '" + UserCode + "', ''";
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(TransactionNotification);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                #endregion

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        if (row[0].ToString() != "0")
                        {
                            PopUpMsg = ("(-" + row[0].ToString() + ") " + row[1].ToString());
                            MsgBox(InformationType.Error, "Error", PopUpMsg);
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                PopUpMsg = ex.Message;
                MsgBox(InformationType.Error, "Error", PopUpMsg);
                conn.Close();
            }
            #endregion
        }

        public void prepareJEPreview(PostWidget JEPreview, FTS11OADM OADM)
        {
            #region Connect to xaf DB

            string PopUpMsg = "";
            string connectionString = getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand TransactionNotification = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region Prepare JE Preview
            try
            {
                conn.Open();

                TransactionNotification.CommandText = "EXEC sp_FTS11_PrepareJEPreview '" +
                    (JEPreview.DateFrm.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : JEPreview.DateFrm.ToString("yyyy-MM-dd")) + "', '" +
                    (JEPreview.DateTo.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : JEPreview.DateTo.ToString("yyyy-MM-dd")) + "', '" +
                    (JEPreview.FAClsFrm == null ? "" : JEPreview.FAClsFrm.Code) + "', '" +
                    (JEPreview.FAClsTo == null ? "" : JEPreview.FAClsTo.Code) + "', '" +
                    (JEPreview.FAGrpFrm == null ? "" : JEPreview.FAGrpFrm.U_FAGrpCod) + "', '" +
                    (JEPreview.FAGrpTo == null ? "" : JEPreview.FAGrpTo.U_FAGrpCod) + "', '" +
                    (JEPreview.FACodeFrm == null ? "" : JEPreview.FACodeFrm.U_FACode) + "', '" +
                    (JEPreview.FACodeTo == null ? "" : JEPreview.FACodeTo.U_FACode) + "', '" +
                    (OADM.U_SplitBy == null ? "" : OADM.U_SplitBy.Code) + "', '" +
                    (JEPreview.DocDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : JEPreview.DocDate.ToString("yyyy-MM-dd")) + "', '" +
                    (string.IsNullOrEmpty(JEPreview.U_CreateBy) ? "" : JEPreview.U_CreateBy.ToString()) + "', '" +
                    (JEPreview.Series == null ? "" : JEPreview.Series.Series) + "', '" +
                    (string.IsNullOrEmpty(JEPreview.Ref1) ? "" : JEPreview.Ref1.ToString()) + "', '" +
                    (string.IsNullOrEmpty(JEPreview.Ref2) ? "" : JEPreview.Ref2.ToString()) + "', '" +
                    (string.IsNullOrEmpty(JEPreview.JEMemo) ? "" : JEPreview.JEMemo.ToString()) + "', '" +
                    (JEPreview.PostTo == null ? "" : JEPreview.PostTo.Code) + "', '" +
                    (JEPreview.FileType == null ? "" : JEPreview.FileType.Code) + "'";
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(TransactionNotification);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                #endregion

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        if (row[0].ToString() != "0")
                        {
                            PopUpMsg = ("(-" + row[0].ToString() + ") " + row[1].ToString());
                            MsgBox(InformationType.Error, "Error", PopUpMsg);
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                PopUpMsg = ex.Message;
                MsgBox(InformationType.Error, "Error", PopUpMsg);
                conn.Close();
            }
            #endregion
        }

        public void RegenerateView()
        {
            try
            {
                string connectionString = getConnectionString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("", conn);

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Open();

                    cmd.CommandText = "EXEC sp_FTS11_RegenerateView";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MsgBox(InformationType.Error, "Error", ex.Message);
            }
        }

        public void UpdateIsFieldEditable(int FAKey)
        {
            try
            {
                string connectionString = getConnectionString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("", conn);

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Open();

                    cmd.CommandText = "EXEC sp_FTS11_SetIsFieldEditable ";
                    if (FAKey == 0)
                        cmd.CommandText += "''";
                    else
                        cmd.CommandText += FAKey;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MsgBox(InformationType.Error, "Error", ex.Message);
            }
        }

        public void AutoNumbering(FTS11OADM OADM, FTS11NNM1 code,FTS11OFAM OFAM, IObjectSpace os)
        {
            string prefix = "";
            string numbering = "";
            string suffix = "";

            if (code.Month == false && code.Year == false)
            {
                prefix = string.IsNullOrEmpty(code.Prefix) ? "" : code.Prefix.ToString();
                numbering = String.Format("{0:D" + code.Length + "}", int.Parse(code.Seq.ToString()));
                suffix = string.IsNullOrEmpty(code.Suffix) ? "" : code.Suffix.ToString();

                code.Seq++;
            }
            else if (OADM.U_FACodeDt.Code == "AcqDate" && code.Month == true && code.Year == false)
            {
                //Prefix
                if (!string.IsNullOrEmpty(code.Prefix))
                {
                    if (code.Prefix.Contains("[") && code.Prefix.Contains("]"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_AcqDate.ToString("MM") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else
                        prefix = code.Prefix;
                }
                else
                {
                    prefix = "";
                }

                //Numbering
                FTS11NNM2 NNM2Code = os.FindObject<FTS11NNM2>(CriteriaOperator.Parse("DocEntry=? AND DocMonth=? AND DocYear=?", code.DocEntry, OFAM.U_AcqDate.ToString("MM"), 0));

                if (NNM2Code == null)
                {
                    FTS11NNM2 NNM2 = os.CreateObject<FTS11NNM2>();
                    NNM2.DocEntry = code;
                    NNM2.DocYear = 0;
                    NNM2.DocMonth = int.Parse(OFAM.U_AcqDate.ToString("MM"));
                    NNM2.Seq = 1;
                    NNM2.Length = code.Length;
                    code.FTS11NNM2.Add(NNM2);

                    numbering = String.Format("{0:D" + code.Length + "}", 1);

                    NNM2.Seq++;
                }
                else
                {
                    numbering = String.Format("{0:D" + NNM2Code.Length + "}", int.Parse(NNM2Code.Seq.ToString()));

                    NNM2Code.Seq++;
                }

                //Suffix
                if (!string.IsNullOrEmpty(code.Suffix))
                {
                    if (code.Suffix.Contains("[") && code.Suffix.Contains("]"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_AcqDate.ToString("MM") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else
                        suffix = code.Suffix;
                }
                else
                {
                    suffix = "";
                }
            }
            else if (OADM.U_FACodeDt.Code == "AcqDate" && code.Month == false && code.Year == true)
            {
                //Prefix
                if (!string.IsNullOrEmpty(code.Prefix))
                {
                    string yearformat = "";
                    yearformat = code.Prefix.Substring(code.Prefix.IndexOf('[') + 1, code.Prefix.IndexOf(']') - code.Prefix.IndexOf('[') - 1);

                    if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YYYY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yyyy") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yy") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else
                        prefix = code.Prefix;
                }
                else
                {
                    prefix = "";
                }

                //Numbering
                FTS11NNM2 NNM2Code = os.FindObject<FTS11NNM2>(CriteriaOperator.Parse("DocEntry=? AND DocMonth=? AND DocYear=?", code.DocEntry, 0, OFAM.U_AcqDate.ToString("yyyy")));

                if (NNM2Code == null)
                {
                    FTS11NNM2 NNM2 = os.CreateObject<FTS11NNM2>();
                    NNM2.DocEntry = code;
                    NNM2.DocYear = int.Parse(OFAM.U_AcqDate.ToString("yyyy"));
                    NNM2.DocMonth = 0;
                    NNM2.Seq = 1;
                    NNM2.Length = code.Length;
                    code.FTS11NNM2.Add(NNM2);

                    numbering = String.Format("{0:D" + code.Length + "}", 1);

                    NNM2.Seq++;
                }
                else
                {
                    numbering = String.Format("{0:D" + NNM2Code.Length + "}", int.Parse(NNM2Code.Seq.ToString()));

                    NNM2Code.Seq++;
                }

                //Suffix
                if (!string.IsNullOrEmpty(code.Suffix))
                {
                    string yearformat = "";
                    yearformat = code.Suffix.Substring(code.Suffix.IndexOf('[') + 1, code.Suffix.IndexOf(']') - code.Suffix.IndexOf('[') - 1);

                    if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YYYY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yyyy") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yy") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else
                        suffix = code.Suffix;
                }
                else
                {
                    suffix = "";
                }

            }
            else if (OADM.U_FACodeDt.Code == "AcqDate" && code.Month == true && code.Year == true)
            {
                //Prefix
                if (!string.IsNullOrEmpty(code.Prefix))
                {
                    string yearformat = "";
                    yearformat = code.Prefix.Substring(code.Prefix.IndexOf('[') + 1, code.Prefix.IndexOf(']') - code.Prefix.IndexOf('[') - 1);

                    if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YYYY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yyyyMM") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yyMM") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else
                        prefix = code.Prefix;
                }
                else
                {
                    prefix = "";
                }

                //Numbering
                FTS11NNM2 NNM2Code = os.FindObject<FTS11NNM2>(CriteriaOperator.Parse("DocEntry=? AND DocMonth=? AND DocYear=?", code.DocEntry, OFAM.U_AcqDate.ToString("MM"), OFAM.U_AcqDate.ToString("yyyy")));

                if (NNM2Code == null)
                {
                    FTS11NNM2 NNM2 = os.CreateObject<FTS11NNM2>();
                    NNM2.DocEntry = code;
                    NNM2.DocYear = int.Parse(OFAM.U_AcqDate.ToString("yyyy"));
                    NNM2.DocMonth = int.Parse(OFAM.U_AcqDate.ToString("MM"));
                    NNM2.Seq = 1;
                    NNM2.Length = code.Length;
                    code.FTS11NNM2.Add(NNM2);

                    numbering = String.Format("{0:D" + code.Length + "}", 1);

                    NNM2.Seq++;
                }
                else
                {
                    numbering = String.Format("{0:D" + NNM2Code.Length + "}", int.Parse(NNM2Code.Seq.ToString()));

                    NNM2Code.Seq++;
                }

                //Suffix
                if (!string.IsNullOrEmpty(code.Suffix))
                {
                    string yearformat = "";
                    yearformat = code.Suffix.Substring(code.Suffix.IndexOf('[') + 1, code.Suffix.IndexOf(']') - code.Suffix.IndexOf('[') - 1);

                    if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YYYY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yyyyMM") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_AcqDate.ToString("yyMM") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else
                        suffix = code.Suffix;
                }
                else
                {
                    suffix = "";
                }
            }
            else if (OADM.U_FACodeDt.Code == "EffDate" && code.Month == true && code.Year == false)
            {
                //Prefix
                if (!string.IsNullOrEmpty(code.Prefix))
                {
                    if (code.Prefix.Contains("[") && code.Prefix.Contains("]"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_EffDate.ToString("MM") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else
                        prefix = code.Prefix;
                }
                else
                {
                    prefix = "";
                }

                //Numbering
                FTS11NNM2 NNM2Code = os.FindObject<FTS11NNM2>(CriteriaOperator.Parse("DocEntry=? AND DocMonth=? AND DocYear=?", code.DocEntry, OFAM.U_EffDate.ToString("MM"), 0));

                if (NNM2Code == null)
                {
                    FTS11NNM2 NNM2 = os.CreateObject<FTS11NNM2>();
                    NNM2.DocEntry = code;
                    NNM2.DocMonth = int.Parse(OFAM.U_EffDate.ToString("MM"));
                    NNM2.Seq = 1;
                    NNM2.Length = code.Length;
                    code.FTS11NNM2.Add(NNM2);

                    numbering = String.Format("{0:D" + code.Length + "}", 1);

                    NNM2.Seq++;
                }
                else
                {
                    numbering = String.Format("{0:D" + NNM2Code.Length + "}", int.Parse(NNM2Code.Seq.ToString()));

                    NNM2Code.Seq++;
                }

                //Suffix
                if (!string.IsNullOrEmpty(code.Suffix))
                {
                    if (code.Suffix.Contains("[") && code.Suffix.Contains("]"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_EffDate.ToString("MM") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else
                        suffix = code.Suffix;
                }
                else
                {
                    suffix = "";
                }
            }
            else if (OADM.U_FACodeDt.Code == "EffDate" && code.Month == false && code.Year == true)
            {
                //Prefix
                if (!string.IsNullOrEmpty(code.Prefix))
                {
                    string yearformat = "";
                    yearformat = code.Prefix.Substring(code.Prefix.IndexOf('[') + 1, code.Prefix.IndexOf(']') - code.Prefix.IndexOf('[') - 1);

                    if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YYYY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_EffDate.ToString("yyyy") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_EffDate.ToString("yy") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else
                        prefix = code.Prefix;
                }
                else
                {
                    prefix = "";
                }

                //Numbering
                FTS11NNM2 NNM2Code = os.FindObject<FTS11NNM2>(CriteriaOperator.Parse("DocEntry=? AND DocMonth=? AND DocYear=?", code.DocEntry, 0, OFAM.U_EffDate.ToString("yyyy")));

                if (NNM2Code == null)
                {
                    FTS11NNM2 NNM2 = os.CreateObject<FTS11NNM2>();
                    NNM2.DocEntry = code;
                    NNM2.DocYear = int.Parse(OFAM.U_EffDate.ToString("yyyy"));
                    NNM2.DocMonth = 0;
                    NNM2.Seq = 1;
                    NNM2.Length = code.Length;
                    code.FTS11NNM2.Add(NNM2);

                    numbering = String.Format("{0:D" + code.Length + "}", 1);

                    NNM2.Seq++;
                }
                else
                {
                    numbering = String.Format("{0:D" + NNM2Code.Length + "}", int.Parse(NNM2Code.Seq.ToString()));

                    NNM2Code.Seq++;
                }

                //Suffix
                if (!string.IsNullOrEmpty(code.Suffix))
                {
                    string yearformat = "";
                    yearformat = code.Suffix.Substring(code.Suffix.IndexOf('[') + 1, code.Suffix.IndexOf(']') - code.Suffix.IndexOf('[') - 1);

                    if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YYYY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_EffDate.ToString("yyyy") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_EffDate.ToString("yy") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else
                        suffix = code.Suffix;
                }
                else
                {
                    suffix = "";
                }

            }
            else if (OADM.U_FACodeDt.Code == "EffDate" && code.Month == true && code.Year == true)
            {
                //Prefix
                if (!string.IsNullOrEmpty(code.Prefix))
                {
                    string yearformat = "";
                    yearformat = code.Prefix.Substring(code.Prefix.IndexOf('[') + 1, code.Prefix.IndexOf(']') - code.Prefix.IndexOf('[') - 1);

                    if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YYYY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_EffDate.ToString("yyyyMM") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else if (code.Prefix.Contains("[") && code.Prefix.Contains("]") && yearformat.Contains("YY"))
                        prefix = code.Prefix.Substring(0, code.Prefix.IndexOf('[')) + OFAM.U_EffDate.ToString("yyMM") + code.Prefix.Substring(code.Prefix.LastIndexOf(']') + 1);
                    else
                        prefix = code.Prefix;
                }
                else
                {
                    prefix = "";
                }

                //Numbering
                FTS11NNM2 NNM2Code = os.FindObject<FTS11NNM2>(CriteriaOperator.Parse("DocEntry=? AND DocMonth=? AND DocYear=?", code.DocEntry, OFAM.U_EffDate.ToString("MM"), OFAM.U_EffDate.ToString("yyyy")));

                if (NNM2Code == null)
                {
                    FTS11NNM2 NNM2 = os.CreateObject<FTS11NNM2>();
                    NNM2.DocEntry = code;
                    NNM2.DocYear = int.Parse(OFAM.U_EffDate.ToString("yyyy"));
                    NNM2.DocMonth = int.Parse(OFAM.U_EffDate.ToString("MM"));
                    NNM2.Seq = 1;
                    NNM2.Length = code.Length;
                    code.FTS11NNM2.Add(NNM2);

                    numbering = String.Format("{0:D" + code.Length + "}", 1);

                    NNM2.Seq++;
                }
                else
                {
                    numbering = String.Format("{0:D" + NNM2Code.Length + "}", int.Parse(NNM2Code.Seq.ToString()));

                    NNM2Code.Seq++;
                }

                //Suffix
                if (!string.IsNullOrEmpty(code.Suffix))
                {
                    string yearformat = "";
                    yearformat = code.Suffix.Substring(code.Suffix.IndexOf('[') + 1, code.Suffix.IndexOf(']') - code.Suffix.IndexOf('[') - 1);

                    if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YYYY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_EffDate.ToString("yyyyMM") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else if (code.Suffix.Contains("[") && code.Suffix.Contains("]") && yearformat.Contains("YY"))
                        suffix = code.Suffix.Substring(0, code.Suffix.IndexOf('[')) + OFAM.U_EffDate.ToString("yyMM") + code.Suffix.Substring(code.Suffix.LastIndexOf(']') + 1);
                    else
                        suffix = code.Suffix;
                }
                else
                {
                    suffix = "";
                }
            }

            OFAM.U_FACode = prefix + numbering + suffix;
        }

        public void getSAPConfiguration(SAPbobsCOM.Company oCom, FTS11OADM OADM)
        {
            //Connect to SAP
            string SvrType = OADM.U_SvrTyp.Code;

            switch (SvrType)
            {
                case "MSSQL2005":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;
                    break;
                case "MSSQL2008":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                    break;
                case "MSSQL2012":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    break;
                case "MSSQL2014":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    break;
                case "MSSQL2016":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                    break;
                case "MSSQL2017":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                    break;
                case "MSSQL2019":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
                    break;
                case "HANADB":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    break;
            }
            oCom.Server = OADM.U_SqlSvr;
            oCom.DbUserName = OADM.U_SqlUID;
            oCom.DbPassword = OADM.U_SqlPWD;
            oCom.language = SAPbobsCOM.BoSuppLangs.ln_English;
            //oCom.LicenseServer = ConfigurationManager.AppSettings.Get("LicenseServer").ToString();
            oCom.CompanyDB = OADM.U_SboDB;
            oCom.UserName = OADM.U_SboUID;
            oCom.Password = OADM.U_SboPWD;

            if (oCom.Connect() != 0)
            {
                string message = oCom.GetLastErrorDescription().ToString().Replace("'", "");
                MsgBox(InformationType.Error, "Error", message);
            }
        }

        public void executeNonQuery(string query)
        {
            #region Connect to xaf DB

            string PopUpMsg = "";
            string connectionString = getConnectionString();

            SqlConnection conn = new SqlConnection(connectionString);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region ExecuteNonQuery
            try
            {
                conn.Open();

                SqlCommand cmdsql = new SqlCommand(query, conn);
                cmdsql.ExecuteNonQuery();
                cmdsql.Dispose();

                conn.Close();
            }
            catch (Exception ex)
            {
                PopUpMsg = ex.Message;
                MsgBox(InformationType.Error, "Error", PopUpMsg);
                conn.Close();
            }
            #endregion
        }
    }
}
