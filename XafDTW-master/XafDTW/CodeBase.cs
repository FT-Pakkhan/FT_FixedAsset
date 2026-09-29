using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using XafDTW.Helpers;

namespace XafDTW
{
    public abstract class CodeBase
    {
        protected DataTable Dt { get; set; }
        protected string ExcelFullPath { get; set; }
        protected IDictionary<string, Action> dict;
        protected SecurityStrategyComplex Security { get; set; }
        protected IObjectSpaceProvider ObjectSpaceProvider { get; set; }
        public string ConnString { get; set; }

        public void selectFunction(string excelFullPath, string typeCode)
        {
            try
            {
                ExcelFullPath = excelFullPath;
                string excelconn = ExcelHelper.getConnection(Logs.getConfigurationAppSettingValue("ExcelExtension"), excelFullPath);
                if (string.IsNullOrWhiteSpace(excelconn))
                {
                    Application.Exit();
                    return;
                }
                string query = String.Format("select * from [{0}${1}:{2}]", Logs.getConfigurationAppSettingValue("ExcelSheet"), Logs.getConfigurationAppSettingValue("ExcelStart"), Logs.getConfigurationAppSettingValue("ExcelEnd"));

                OleDbConnection con = new OleDbConnection(excelconn);
                con.Open();
                OleDbCommand cmd = new OleDbCommand(query, con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                Dt = new DataTable();
                da.Fill(Dt);

                foreach (KeyValuePair<string, Action> entry in dict)
                {
                    if (typeCode == entry.Key)
                    {
                        Action action = entry.Value;
                        action.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

        protected void CopyPropertyValueByDataRow(object tobj, DataRow dr)
        {
            try
            {
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    string tcolumnname = dc.ColumnName;
                    var svalue = dr[tcolumnname];
                    if (svalue == null || Convert.ToString(svalue) == "") continue;
                    var tproperty = tobj.GetType().GetProperty(tcolumnname);

                    //Replace with continue, so that user can create extra column in excel for their vlookup view purpose
                    //if (tproperty == null) throw new MyException($"{tcolumnname} not found in {tobj.GetType().ToString()}");
                    if (tproperty == null) continue;

                    if (typeof(DevExpress.Xpo.XPBaseObject).IsAssignableFrom(tproperty.PropertyType))
                    {
                        string opcriteria = MyFunctions.getKeyOperatorString(tproperty);

                        if (opcriteria == "") throw new MyException("Business Object's Key property is not valid.");

                        GeneralHelper.SetPropertyValueCriteriaOperator(tobj, tcolumnname, DevExpress.Data.Filtering.CriteriaOperator.Parse(opcriteria, Convert.ToString(svalue)));
                        continue;
                    }
                    else if (tproperty.PropertyType == typeof(string))
                    {
                        GeneralHelper.SetPropertyValue(tobj, tcolumnname, Convert.ToString(svalue));
                        continue;
                    }
                    else if (tproperty.PropertyType == typeof(bool))
                    {
                        GeneralHelper.SetPropertyValue(tobj, tcolumnname, Convert.ToBoolean(svalue));
                        continue;
                    }
                    else if (tproperty.PropertyType == typeof(int))
                    {
                        GeneralHelper.SetPropertyValue(tobj, tcolumnname, Convert.ToInt32(svalue));
                        continue;
                    }
                    else if (tproperty.PropertyType == typeof(double))
                    {
                        GeneralHelper.SetPropertyValue(tobj, tcolumnname, Convert.ToDouble(svalue));
                        continue;
                    }
                    else if (tproperty.PropertyType == typeof(decimal))
                    {
                        GeneralHelper.SetPropertyValue(tobj, tcolumnname, Convert.ToDecimal(svalue));
                        continue;
                    }
                    else if (tproperty.PropertyType == typeof(DateTime))
                    {
                        GeneralHelper.SetPropertyValue(tobj, tcolumnname, Convert.ToDateTime(svalue));
                        continue;
                    }
                    GeneralHelper.SetPropertyValue(tobj, tcolumnname, svalue);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new InvalidOperationException(ex.InnerException.Message);
                else
                    throw new InvalidOperationException(ex.Message);
            }
        }

    }
}
