using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XafDTW.Helpers
{
    public class Logs
    {
        public static string getConfigurationAppSettingValue(string name)
        {
            return ConfigurationManager.AppSettings.Get(name);
        }
        public static string logPath { get; set; }
        public static string UserID { get; set; }
        public static void WriteLog(string lvl, string PortalUserID, string str)
        {
            if (string.IsNullOrWhiteSpace(PortalUserID)) PortalUserID = UserID;
            FileStream fileStream = null;
            string filePath = "";
            if (string.IsNullOrWhiteSpace(logPath))
                filePath = Logs.getConfigurationAppSettingValue("LogPath");
            else
                filePath = logPath;
            filePath = filePath + "[" + "FA_DTW" + "] Log_" + System.DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

            FileInfo fileInfo = new FileInfo(filePath);
            DirectoryInfo dirInfo = new DirectoryInfo(fileInfo.DirectoryName);
            if (!dirInfo.Exists) dirInfo.Create();

            if (!fileInfo.Exists)
            {
                fileStream = fileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(filePath, FileMode.Append);
            }

            StreamWriter log = new StreamWriter(fileStream);
            string status = lvl;//.ToString().Replace("[Log]", "");

            //For SAP_Integration_Log
            log.WriteLine("[{0}-{3}][{1}]{2}", status, PortalUserID, str.ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            //For E-Mail
            //createEmailLog(status + str.ToString());
            //setEmailContet(lvl.ToString(), str.ToString());

            log.Close();

        }

        public static void JsonToXML(string json, string type, bool success, string docnum, string errmsg)
        {
            try
            {
                string filePath = "";
                if (string.IsNullOrWhiteSpace(logPath))
                    filePath = "C:\\SAP_API_Integration_Log\\";
                else
                    filePath = logPath;

                filePath += "XML\\";
                filePath = filePath + "[" + type + "] Log_" + System.DateTime.Today.ToString("yyyyMMdd") + "." + "xml";

                //FileStream fileStream = null;
                FileInfo fileInfo = new FileInfo(filePath);
                DirectoryInfo dirInfo = new DirectoryInfo(fileInfo.DirectoryName);
                if (!dirInfo.Exists) dirInfo.Create();

                XmlDocument doc = new XmlDocument();
                if (!fileInfo.Exists)
                {
                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);
                    XmlElement body = doc.CreateElement("Body");
                    doc.AppendChild(body);
                    doc.Save(filePath);
                }
                doc.Load(filePath);

                string logid = System.Guid.NewGuid().ToString();
                XmlElement newElement = doc.CreateElement("Row");

                XmlNode note = doc.CreateElement("LogID");
                note.InnerText = logid;
                newElement.AppendChild(note);

                note = doc.CreateElement("LogResult");
                note.InnerText = success ? "success" : "fail";
                newElement.AppendChild(note);

                note = doc.CreateElement("LogTime");
                //note.InnerText = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                note.InnerText = DateTime.Now.ToUniversalTime().ToString("o");
                newElement.AppendChild(note);

                note = doc.CreateElement("LogMessage");
                note.InnerText = errmsg;
                newElement.AppendChild(note);

                note = doc.CreateElement("DocNum");
                note.InnerText = string.IsNullOrWhiteSpace(docnum) ? "0" : docnum;
                newElement.AppendChild(note);

                AssignElement(json, "Body", doc, newElement, logid, true);
                XmlNode nodes = doc.SelectSingleNode("Body");
                nodes.AppendChild(newElement);

                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ ex.Message }\n{ ex.StackTrace }");
            }
        }
        private static void AssignElement(string json, string type, XmlDocument doc, XmlElement newElement, string parentlogid, bool root)
        {
            using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max))
            {
                XmlNode nodes = doc.SelectSingleNode(type);

                XmlNode note = null;
                if (!root)
                {
                    if (!string.IsNullOrWhiteSpace(parentlogid))
                    {
                        note = doc.CreateElement("ParentID");
                        note.InnerText = parentlogid;
                        newElement.AppendChild(note);

                        note = doc.CreateElement("LineID");
                        parentlogid = System.Guid.NewGuid().ToString();
                        note.InnerText = parentlogid;
                        newElement.AppendChild(note);
                    }
                }
                var xMembers = from members in XElement.Load(reader).Elements() select members;
                foreach (XElement x in xMembers)
                {
                    JObject jobj = JObject.Parse(json);
                    if (jobj[x.Name.ToString()].GetType() == typeof(JArray))
                    {
                        JArray jarray = (JArray)jobj[x.Name.ToString()];
                        string linejson = "";
                        foreach (JObject Jdtl in jarray.Children())
                        {
                            XmlElement linenode = doc.CreateElement(x.Name.ToString());
                            linejson = JsonConvert.SerializeObject(Jdtl);
                            AssignElement(linejson, type + "/" + newElement.Name, doc, linenode, parentlogid, false);
                            newElement.AppendChild(linenode);
                        }
                    }
                    else
                    {
                        note = doc.CreateElement(x.Name.ToString());
                        note.InnerText = x.Value;
                        newElement.AppendChild(note);
                    }
                }

            }

        }
    }
}
