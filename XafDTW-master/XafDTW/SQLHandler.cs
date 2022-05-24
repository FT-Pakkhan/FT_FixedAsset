using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafDTW
{
    class SQLHandler
    {
        static public void ExecuteNonQuery(string command)
        {
            /*
             * SELECT data from 3rd party SQL Server Database to SAP database
             * 
             * */
            if (command == "") return;
            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ConnectionString);

                try
                {
                    myConnection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                SqlCommand myCommand = new SqlCommand(command, myConnection)
                {
                    CommandTimeout = 90000
                };
                myCommand.ExecuteNonQuery();

                try
                {
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        static public DataTable GetDataTable(string command)
        {
            /*
             * SELECT data from 3rd party SQL Server Database to SAP database
             * 
             * */
            DataTable myDataTable = new DataTable();
            if (command == "") return myDataTable;

            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ConnectionString);

                try
                {
                    myConnection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                SqlCommand myCommand = new SqlCommand(command, myConnection)
                {
                    CommandTimeout = 90000
                };
                using (SqlDataAdapter adapter = new SqlDataAdapter(myCommand))
                {
                    adapter.Fill(myDataTable);
                }

                try
                {
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return myDataTable;
        }


        static public List<string> GetSqlDataReaderInString(string command)
        {
            /*
             * SELECT data from 3rd party SQL Server Database to SAP database
             * 
             * */
            SqlDataReader myReader = null;
            List<string> result = new List<string>();
            if (command == "") return result;

            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ConnectionString);

                try
                {
                    myConnection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                SqlCommand myCommand = new SqlCommand(command, myConnection)
                {
                    CommandTimeout = 90000
                };

                try
                {
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        result.Add(myReader[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                try
                {
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        static public int GetSqlDataReaderSuccessCount(string command)
        {
            /*
             * SELECT data from 3rd party SQL Server Database to SAP database
             * 
             * */
            SqlDataReader myReader = null;
            int successCnt = 0;
            if (command == "") return successCnt;

            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ConnectionString);

                try
                {
                    myConnection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                SqlCommand myCommand = new SqlCommand(command, myConnection)
                {
                    CommandTimeout = 90000
                };

                try
                {
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        if (myReader[0].ToString().ToUpper() == "SUCCESS") successCnt++;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                try
                {
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return successCnt;
        }
    }
}
