
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafDTW.Helpers
{
    public class ExcelHelper
    {
        public static string getConnection(string extension, string filenamewithpath)
        {
            string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filenamewithpath + ";Extended Properties=Excel 12.0";
            return connection;
        }

        public static string getCellValue(OleDbConnection conn, string sheet, string cell)
        {
            try
            {
                string ExcelQuery = $"SELECT * FROM [{sheet}${cell}:{cell}]";
                OleDbCommand ExcelCommand = new OleDbCommand(ExcelQuery, conn);
                OleDbDataReader ExcelReader = ExcelCommand.ExecuteReader();
                string value = ExcelReader.GetName(0);

                return value;
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

        public static void movefile(string sourcefile, string targetPath)
        {
            try
            {
                System.IO.Directory.CreateDirectory(targetPath);
                string destinationFile = targetPath + "\\" + System.IO.Path.GetFileName(sourcefile);
                System.IO.File.Move(sourcefile, destinationFile);
                Logs.WriteLog("Log", "", $"{sourcefile} move to {destinationFile}.");
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }

    }
}
