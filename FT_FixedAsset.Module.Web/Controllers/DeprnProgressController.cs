using System.Web;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using FT_FixedAsset.Module.BusinessObjects;
using FT_FixedAsset.Module.Controllers;

namespace FT_FixedAsset.Module.Web.Controllers
{
    /// <summary>
    /// Progress window of Run Depreciation (DeprnProgress.aspx in a centred overlay).
    /// - On the Depreciation Widget page: defines the overlay and opens it whenever a run is in progress
    ///   (also when the user comes back to the page during a run).
    /// - Run Depreciation (after "Yes") opens it at once through DeprnRunner.ShowProgress.
    /// </summary>
    public class DeprnProgressController : ObjectViewController<DetailView, DeprnWidget>
    {
        static DeprnProgressController()
        {
            DeprnRunner.ShowProgress = runId =>
            {
                if (WebWindow.CurrentRequestWindow == null) return;
                // the Yes button runs inside the confirmation popup (an iframe): open the window in the main page
                WebWindow.CurrentRequestWindow.RegisterStartupScript("FTDeprnShow" + runId,
                    "try{ (window.top.FT_ShowDeprnProgress || function(){})(" + runId + "); }catch(e){}", true);
            };
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            // the progress page reads this session's company database
            GenController genCon = Frame.GetController<GenController>();
            if (genCon != null && HttpContext.Current != null && HttpContext.Current.Session != null)
                HttpContext.Current.Session[DeprnRunner.SessionConnKey] = genCon.getConnectionString();

            if (WebWindow.CurrentRequestWindow != null)
                WebWindow.CurrentRequestWindow.RegisterStartupScript("FTDeprnProgress", OverlayScript(VirtualPathUtility.ToAbsolute("~/DeprnProgress.aspx")), true);
        }

        private static string OverlayScript(string url)
        {
            return @"(function(w){
  w.FT_DeprnUrl = '" + HttpUtility.JavaScriptStringEncode(url) + @"';
  w.FT_ShowDeprnProgress = function(run){
    var d = w.document;
    if (d.getElementById('ftDeprnOverlay')) return;
    w.FT_DeprnShownRun = run;
    var o = d.createElement('div'); o.id = 'ftDeprnOverlay';
    o.style.cssText = 'position:fixed;left:0;top:0;right:0;bottom:0;background:rgba(0,0,0,.35);z-index:100000;';
    var f = d.createElement('iframe'); f.title = 'Run Depreciation';
    f.src = w.FT_DeprnUrl + '?run=' + run + '&t=' + new Date().getTime();
    f.style.cssText = 'position:absolute;left:50%;top:50%;width:640px;height:520px;max-width:calc(100% - 32px);max-height:calc(100% - 32px);' +
                      'transform:translate(-50%,-50%);border:1px solid #8ba0bc;background:#fff;box-shadow:0 4px 18px rgba(0,0,0,.3);';
    o.appendChild(f); d.body.appendChild(o);
  };
  // closed by the user (Run in Background / OK): do not pop that run up again on this page
  w.FT_CloseDeprnProgress = function(){ var o = w.document.getElementById('ftDeprnOverlay'); if (o) o.parentNode.removeChild(o); w.FT_DeprnClosedRun = w.FT_DeprnShownRun; };
  function check(){
    if (w.location.pathname.indexOf('DeprnWidget_DetailView') < 0 || w.document.getElementById('ftDeprnOverlay')) return;
    var x = new XMLHttpRequest();
    x.open('GET', w.FT_DeprnUrl + '?active=1&t=' + new Date().getTime(), true);
    x.onload = function(){ try { var r = JSON.parse(x.responseText); if (r.run > 0 && r.run !== w.FT_DeprnClosedRun) w.FT_ShowDeprnProgress(r.run); } catch(e){} };
    x.send();
  }
  check();
  if (!w.FT_DeprnPoll) w.FT_DeprnPoll = setInterval(check, 3000);
})(window.top);";
        }
    }
}
