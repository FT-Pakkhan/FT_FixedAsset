using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Validation;
using FT_FixedAsset.Module.BusinessObjects;
using FT_FixedAsset.Module.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using XafDTW.Helpers;

namespace XafDTW
{
    public class Code : CodeBase
    {
        public Code(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider, string connString)
        {
            Security = security;
            ObjectSpaceProvider = objectSpaceProvider;
            ConnString = connString;
            dict = new Dictionary<string, Action>();

            #region add your typecode and function to dictionary
            dict.Add("FTS11OFAC", dtwFTS11OFAC);
            dict.Add("FTS11OFAG", dtwFTS11OFAG);
            dict.Add("FTS11OLOC", dtwFTS11OLOC);
            dict.Add("FTS11OIND", dtwFTS11OIND);
            dict.Add("FTS11OFAM", dtwFTS11OFAM);
            #endregion
        }

        #region your functions
        private void dtwFTS11OFAC()
        {
            try
            {
                int count = 0;
                IObjectSpace securedObjectSpace = ObjectSpaceProvider.CreateObjectSpace();
                var list = new List<DisplayResult>();

                foreach (DataRow dr in Dt.Rows)
                {
                    FTS11OFAC OFAC = securedObjectSpace.CreateObject<FTS11OFAC>();
                    try
                    {
                        OFAC.U_CreateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        OFAC.U_UpdateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        CopyPropertyValueByDataRow(OFAC, dr);
                        Validator.RuleSet.Validate(securedObjectSpace, OFAC, DefaultContexts.Save);
                        securedObjectSpace.CommitChanges();
                        PostTransactionNotice(OFAC.Object, "A", OFAC.DocEntry);
                        RecordChangeLog(OFAC.Object, OFAC.DocEntry);
                        list.Add(new DisplayResult() { SuccessFail = "Success", Code = OFAC.Code, Message = "Successfully Imported!" });
                        count++;
                    }
                    catch (Exception ex)
                    {
                        securedObjectSpace.Rollback();
                        list.Add(new DisplayResult()
                        {
                            SuccessFail = "Failed",
                            Code = OFAC.Code,
                            Message = OFAC.Code + " - " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message)
                        });
                        Logs.WriteLog("Exception", "", OFAC.Code + " - " +
                            (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                }
                Result result = new Result(list);
                result.Show();
                MessageBox.Show(count.ToString() + " record(s) successfully imported!", "Completed");
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

        private void dtwFTS11OFAG()
        {
            try
            {
                int count = 0;
                IObjectSpace securedObjectSpace = ObjectSpaceProvider.CreateObjectSpace();
                var list = new List<DisplayResult>();

                foreach (DataRow dr in Dt.Rows)
                {
                    FTS11OFAG OFAG = securedObjectSpace.CreateObject<FTS11OFAG>();
                    try
                    {
                        OFAG.U_CreateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        OFAG.U_UpdateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        CopyPropertyValueByDataRow(OFAG, dr);
                        Validator.RuleSet.Validate(securedObjectSpace, OFAG, DefaultContexts.Save);
                        securedObjectSpace.CommitChanges();
                        PostTransactionNotice(OFAG.Object, "A", OFAG.DocEntry);
                        RecordChangeLog(OFAG.Object, OFAG.DocEntry);
                        list.Add(new DisplayResult() { SuccessFail = "Success", Code = OFAG.U_FAGrpCod, Message = "Successfully Imported!" });
                        count++;
                    }
                    catch (Exception ex)
                    {
                        securedObjectSpace.Rollback();
                        list.Add(new DisplayResult()
                        {
                            SuccessFail = "Failed",
                            Code = OFAG.U_FAGrpCod,
                            Message = OFAG.U_FAGrpCod + " - " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message)
                        });
                        Logs.WriteLog("Exception", "", OFAG.U_FAGrpCod + " - " +
                            (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                }
                Result result = new Result(list);
                result.Show();
                MessageBox.Show(count.ToString() + " record(s) successfully imported!", "Completed");
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

        private void dtwFTS11OLOC()
        {
            try
            {
                int count = 0;
                IObjectSpace securedObjectSpace = ObjectSpaceProvider.CreateObjectSpace();

                var list = new List<DisplayResult>();
                foreach (DataRow dr in Dt.Rows)
                {
                    FTS11OLOC OLOC = securedObjectSpace.CreateObject<FTS11OLOC>();
                    try
                    {
 
                        OLOC.U_CreateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        OLOC.U_UpdateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        CopyPropertyValueByDataRow(OLOC, dr);
                        Validator.RuleSet.Validate(securedObjectSpace, OLOC, DefaultContexts.Save);
                        securedObjectSpace.CommitChanges();
                        PostTransactionNotice(OLOC.Object, "A", OLOC.DocEntry);
                        RecordChangeLog(OLOC.Object, OLOC.DocEntry);
                        list.Add(new DisplayResult() {SuccessFail = "Success", Code = OLOC.Code, Message = "Successfully Imported!" });
                        count++;
                    }
                    catch (Exception ex)
                    {
                        securedObjectSpace.Rollback();
                        list.Add(new DisplayResult() 
                        { 
                            SuccessFail = "Failed", 
                            Code = OLOC.Code, 
                            Message = OLOC.Code + " - " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message) 
                        });
                        Logs.WriteLog("Exception", "", OLOC.Code + " - " +
                            (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                }
                Result result = new Result(list);
                result.Show();
                MessageBox.Show(count.ToString() + " record(s) successfully imported!", "Completed");
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

        private void dtwFTS11OIND()
        {
            try
            {
                int count = 0;
                IObjectSpace securedObjectSpace = ObjectSpaceProvider.CreateObjectSpace();

                var list = new List<DisplayResult>();
                foreach (DataRow dr in Dt.Rows)
                {
                    FTS11OIND OIND = securedObjectSpace.CreateObject<FTS11OIND>();
                    try
                    {

                        OIND.U_CreateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        OIND.U_UpdateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        CopyPropertyValueByDataRow(OIND, dr);
                        Validator.RuleSet.Validate(securedObjectSpace, OIND, DefaultContexts.Save);
                        securedObjectSpace.CommitChanges();
                        PostTransactionNotice(OIND.Object, "A", OIND.DocEntry);
                        RecordChangeLog(OIND.Object, OIND.DocEntry);
                        list.Add(new DisplayResult() { SuccessFail = "Success", Code = OIND.U_IndCode, Message = "Successfully Imported!" });
                        count++;
                    }
                    catch (Exception ex)
                    {
                        securedObjectSpace.Rollback();
                        list.Add(new DisplayResult()
                        {
                            SuccessFail = "Failed",
                            Code = OIND.U_IndCode,
                            Message = OIND.U_IndCode + " - " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message)
                        });
                        Logs.WriteLog("Exception", "", OIND.U_IndCode + " - " +
                            (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                }
                Result result = new Result(list);
                result.Show();
                MessageBox.Show(count.ToString() + " record(s) successfully imported!", "Completed");
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

        private void dtwFTS11OFAM()
        {
            try
            {
                int count = 0;
                IObjectSpace securedObjectSpace = ObjectSpaceProvider.CreateObjectSpace();

                var list = new List<DisplayResult>();
                foreach (DataRow dr in Dt.Rows)
                {
                    FTS11OFAM OFAM = securedObjectSpace.CreateObject<FTS11OFAM>();
                    try
                    {
                        OFAM.U_CreateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        OFAM.U_UpdateBy = securedObjectSpace.FindObject<SystemUsers>(CriteriaOperator.Parse("UserName=?", Security.UserName)).UserName;
                        CopyPropertyValueByDataRow(OFAM, dr);
                        Validator.RuleSet.Validate(securedObjectSpace, OFAM, DefaultContexts.Save);
                        securedObjectSpace.CommitChanges();
                        PostTransactionNotice(OFAM.Object, "A", OFAM.DocEntry);
                        RecordChangeLog(OFAM.Object, OFAM.DocEntry);
                        list.Add(new DisplayResult() { SuccessFail = "Success", Code = OFAM.U_FACode, Message = "Successfully Imported!" });
                        count++;
                    }
                    catch (Exception ex)
                    {
                        securedObjectSpace.Rollback();
                        list.Add(new DisplayResult()
                        {
                            SuccessFail = "Failed",
                            Code = OFAM.U_FACode,
                            Message = OFAM.U_FACode + " - " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message)
                        });
                        Logs.WriteLog("Exception", "", OFAM.U_FACode + " - " +
                            (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                }
                Result result = new Result(list);
                result.Show();
                MessageBox.Show(count.ToString() + " record(s) successfully imported!", "Completed");
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }
        #endregion

        public void PostTransactionNotice(string objType, string transType, int docEntry)
        {
            #region Connect to xaf DB
            string connectionString = ConnString;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand PostTransactionNotice = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region Post Transaction Notification
            try
            {
                conn.Open();

                PostTransactionNotice.CommandText = "EXEC SBO_SP_PostTransactionNotice '" + objType + "', '" + transType + "', 1, 'DocEntry' , '" + docEntry + "'";
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(PostTransactionNotice);
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
                            MessageBox.Show("(-" + row[0].ToString() + ") " + row[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            #endregion
        }

        public void RecordChangeLog(string objType, int docEntry)
        {
            #region Connect to xaf DB

            string connectionString = ConnString;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand RecordChangeLog = new SqlCommand("", conn);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            #endregion

            #region RecordChangeLog


            try
            {
                conn.Open();

                RecordChangeLog.CommandText = "EXEC sp_FTS11_RecordChangeLog '" + objType + "', 1, 'DocEntry', '" + docEntry + "' , 1";
                #region Check Rule Validation

                SqlDataAdapter da = new SqlDataAdapter(RecordChangeLog);
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
                            MessageBox.Show("(-" + row[0].ToString() + ") " + row[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            #endregion
        }
    }
}
