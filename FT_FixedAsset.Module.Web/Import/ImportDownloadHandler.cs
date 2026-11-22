using System;
using System.Web;
using System.Web.SessionState;
using DevExpress.ExpressApp.Web;

namespace FT_FixedAsset.Module.Web.Import
{
    /// <summary>
    /// Serves a file prepared by an action (e.g. Download Template) through ~/ImportDownload.ashx?id=...
    /// Writing a file straight into an XAF callback leaves the page on "Loading", so the action stores the
    /// bytes in the session and the browser fetches them here (same idea as Reports.aspx for the reports).
    /// </summary>
    public class ImportDownloadHandler : IHttpHandler, IRequiresSessionState
    {
        private const string Prefix = "ImportDownload_";

        private class PendingFile
        {
            public string FileName;
            public string ContentType;
            public byte[] Content;
        }

        /// <summary>Keeps the file for this user and makes the browser download it.</summary>
        public static void Send(string fileName, string contentType, byte[] content)
        {
            string id = Guid.NewGuid().ToString("N");
            HttpContext.Current.Session[Prefix + id] = new PendingFile { FileName = fileName, ContentType = contentType, Content = content };

            string url = VirtualPathUtility.ToAbsolute("~/ImportDownload.ashx") + "?id=" + id;
            WebWindow.CurrentRequestWindow.RegisterStartupScript("ImportDownload_" + id,
                "(function(){var f=document.createElement('iframe');f.style.display='none';f.src='" + url + "';document.body.appendChild(f);})();",
                true);
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request.QueryString["id"] ?? "";
            var file = context.Session == null ? null : context.Session[Prefix + id] as PendingFile;
            if (file == null)
            {
                context.Response.StatusCode = 404;
                context.Response.Write("The file is no longer available. Please click the button again.");
                return;
            }
            context.Session.Remove(Prefix + id);   // one-time download

            context.Response.Clear();
            context.Response.ContentType = file.ContentType;
            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.FileName.Replace("\"", "") + "\"");
            context.Response.AddHeader("Content-Length", file.Content.Length.ToString());
            context.Response.BinaryWrite(file.Content);
            context.Response.Flush();
            context.ApplicationInstance.CompleteRequest();
        }
    }
}
