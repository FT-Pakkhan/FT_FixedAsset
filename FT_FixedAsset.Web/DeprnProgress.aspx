<%@ Page Language="C#" EnableSessionState="ReadOnly" EnableViewState="false" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="FT_FixedAsset.Module.Controllers" %>
<script runat="server">
    // Run Depreciation progress window (opened in an overlay by DeprnProgressController).
    //   ?run=N            the window
    //   ?run=N&status=1   progress / result as JSON (polled every second)
    //   ?active=1         {"run": id of the run in progress, or 0}
    //   ?run=N&stop=1     Stop (POST) - the run stops after the current asset
    //   ?run=N&csv=1      all error lines as CSV
    // Only for a logged-on user (forms authentication) and only this session's company database.

    private string Conn { get { return Session[DeprnRunner.SessionConnKey] as string; } }

    private int RunId
    {
        get { int id; return int.TryParse(Request.QueryString["run"], out id) ? id : 0; }
    }

    private static string J(string s) { return "\"" + HttpUtility.JavaScriptStringEncode(s ?? "") + "\""; }

    private void Json(string body)
    {
        Response.Clear();
        Response.ContentType = "application/json";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(body);
        Response.End();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string cs = Conn;

        if (Request.QueryString["active"] == "1")
        {
            int active = 0;
            try { if (!string.IsNullOrEmpty(cs)) active = DeprnRunner.ActiveRunId(cs); } catch { }
            Json("{\"run\":" + active + "}");
            return;
        }

        if (string.IsNullOrEmpty(cs) || RunId <= 0)
        {
            if (Request.QueryString["status"] == "1") Json("{\"error\":\"Session expired - please open the Depreciation Widget again.\"}");
            return;   // the page shows its own message
        }

        if (Request.QueryString["stop"] == "1" && Request.HttpMethod == "POST")
        {
            DeprnRunner.RequestStop(cs, RunId);
            Json("{\"ok\":true}");
            return;
        }

        if (Request.QueryString["csv"] == "1")
        {
            var sb = new StringBuilder("F/A Code,Error\r\n");
            foreach (string[] l in DeprnRunner.AllErrors(cs, RunId))
                sb.Append("\"" + l[0].Replace("\"", "\"\"") + "\",\"" + l[1].Replace("\"", "\"\"") + "\"\r\n");
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=Depreciation_Errors_Run" + RunId + ".csv");
            Response.ContentEncoding = Encoding.UTF8;
            Response.BinaryWrite(Encoding.UTF8.GetPreamble());
            Response.Write(sb.ToString());
            Response.End();
            return;
        }

        if (Request.QueryString["status"] == "1")
        {
            // all error lines once the run has ended (search / sort happen in the window)
            DeprnRunner.Status s = DeprnRunner.GetStatus(cs, RunId, 50000);
            if (s == null) { Json("{\"error\":\"Run not found.\"}"); return; }
            var sb = new StringBuilder("{");
            sb.Append("\"run\":").Append(s.Run)
              .Append(",\"state\":").Append(J(s.State))
              .Append(",\"total\":").Append(s.Total)
              .Append(",\"done\":").Append(s.Done)
              .Append(",\"generated\":").Append(s.Generated)
              .Append(",\"skipped\":").Append(s.Skipped)
              .Append(",\"errors\":").Append(s.Errors)
              .Append(",\"elapsed\":").Append(s.ElapsedSec)
              .Append(",\"period\":").Append(J(s.Period))
              .Append(",\"startBy\":").Append(J(s.StartBy))
              .Append(",\"started\":").Append(J(s.Started))
              .Append(",\"note\":").Append(J(s.Note))
              .Append(",\"stopRequested\":").Append(s.StopRequested ? "true" : "false")
              .Append(",\"errorLines\":[");
            for (int i = 0; i < s.ErrorLines.Count; i++)
                sb.Append(i == 0 ? "" : ",").Append("[").Append(J(s.ErrorLines[i][0])).Append(",").Append(J(s.ErrorLines[i][1])).Append("]");
            sb.Append("]}");
            Json(sb.ToString());
        }
    }
</script>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8" />
<title>Run Depreciation</title>
<style>
  html, body { margin: 0; height: 100%; font-family: Tahoma, Segoe UI, Arial, sans-serif; font-size: 13px; color: #000; background: #fff; }
  .wrap { display: flex; flex-direction: column; height: 100%; }
  .head { background: #d4e1f2; border-bottom: 1px solid #a9bfd6; padding: 8px 12px; font-weight: bold; display: flex; align-items: center; gap: 8px; }
  .body { flex: 1; overflow: auto; padding: 14px 18px; }
  .foot { border-top: 1px solid #c5d3e4; background: #eef3fa; padding: 8px 12px; text-align: right; }
  .bar { height: 18px; background: #e6ebf2; border: 1px solid #b7c6da; border-radius: 2px; overflow: hidden; margin: 8px 0 4px; }
  .bar div { height: 100%; width: 0; background: #5b9bd5; transition: width .4s; }
  .bar.done div { background: #5aa95a; }
  .bar.err div { background: #d9822b; }
  table.kv { border-collapse: collapse; margin-top: 6px; }
  table.kv td { padding: 2px 14px 2px 0; }
  table.kv td.v { text-align: right; font-weight: bold; min-width: 80px; }
  .muted { color: #555; }
  .note { margin-top: 8px; color: #8a4b00; }
  .errs { margin-top: 12px; }
  .errs .list { max-height: calc(100vh - 360px); min-height: 90px; overflow: auto; border: 1px solid #c5d3e4; }
  .errs table { border-collapse: collapse; width: 100%; }
  .errs th, .errs td { text-align: left; padding: 3px 6px; border-bottom: 1px solid #e3e9f1; vertical-align: top; }
  .errs th { background: #eef3fa; position: sticky; top: 0; cursor: pointer; user-select: none; white-space: nowrap; }
  .errs th:hover { background: #dde7f4; }
  .errs th .arrow { color: #1f5fa8; font-size: 10px; margin-left: 4px; }
  .errtools { display: flex; align-items: center; gap: 8px; margin: 6px 0; flex-wrap: wrap; }
  .errtools input { flex: 1; min-width: 160px; font: inherit; padding: 3px 6px; border: 1px solid #8ba0bc; border-radius: 2px; }
  .errs td.none { color: #555; text-align: center; padding: 10px; }
  mark { background: #ffe58a; padding: 0; }
  button { font: inherit; padding: 4px 16px; margin-left: 6px; border: 1px solid #8ba0bc; background: #f4f7fb; border-radius: 2px; cursor: pointer; }
  button.primary { background: #ffe8a0; border-color: #c9a227; }
  button:disabled { opacity: .5; cursor: default; }
  a { color: #1f5fa8; }
</style>
</head>
<body>
<div class="wrap">
  <div class="head" id="title">Run Depreciation</div>
  <div class="body">
    <div id="msg" class="muted">Loading...</div>
    <div id="main" style="display:none">
      <div>Period <b id="period"></b> &nbsp;<span class="muted" id="who"></span></div>
      <div class="bar" id="bar"><div></div></div>
      <div id="count"></div>
      <table class="kv" id="kv"></table>
      <div class="note" id="note"></div>
      <div class="errs" id="errs" style="display:none">
        <b>Error Details</b> &nbsp;<span id="errcsv"></span>
        <div class="errtools">
          <input id="errsearch" type="search" placeholder="Search F/A code or error (e.g. ITC2024, 2024.05)" autocomplete="off" />
          <span class="muted" id="errinfo"></span>
        </div>
        <div class="list"><table><thead><tr>
          <th style="width:150px" data-col="0" title="Click to sort">F/A Code<span class="arrow"></span></th>
          <th data-col="1" title="Click to sort">Error<span class="arrow"></span></th>
        </tr></thead><tbody id="errbody"></tbody></table></div>
      </div>
    </div>
  </div>
  <div class="foot">
    <button id="stop" style="display:none">Stop</button>
    <button id="hide" style="display:none" title="The run continues; open the Depreciation Widget again to see it">Run in Background</button>
    <button id="ok" class="primary" style="display:none">OK</button>
  </div>
</div>
<script>
(function () {
  var run = <%= RunId %>, base = location.pathname, timer = null, stopArmed = false;
  function $(id) { return document.getElementById(id); }
  function n(v) { return (v || 0).toLocaleString('en-US'); }
  function esc(s) { return String(s).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }
  function dur(sec) { sec = Math.max(0, Math.round(sec)); var h = Math.floor(sec / 3600), m = Math.floor(sec % 3600 / 60), s = sec % 60;
                      return (h ? h + ' h ' : '') + (h || m ? m + ' min ' : '') + s + ' s'; }
  function close() { try { window.top.FT_CloseDeprnProgress(); } catch (e) { } }
  $('ok').onclick = close;
  $('hide').onclick = close;
  $('stop').onclick = function () {
    if (!stopArmed) { stopArmed = true; $('stop').textContent = 'Click again to stop'; return; }
    $('stop').disabled = true; $('stop').textContent = 'Stopping...';
    var x = new XMLHttpRequest(); x.open('POST', base + '?run=' + run + '&stop=1', true); x.send();
  };
  function row(label, value) { return '<tr><td>' + label + '</td><td class="v">' + value + '</td></tr>'; }

  function show(s) {
    if (s.error) { $('msg').textContent = s.error; $('main').style.display = 'none'; $('ok').style.display = ''; return; }
    $('msg').style.display = 'none'; $('main').style.display = '';
    $('period').textContent = s.period;
    $('who').textContent = '(started by ' + s.startBy + ', ' + s.started + ')';
    var pct = s.total > 0 ? Math.floor(s.done * 100 / s.total) : 0;
    $('bar').firstChild.style.width = (s.state === 'running' ? pct : 100) + '%';
    if (s.state === 'running') {
      $('title').textContent = 'Running Depreciation...';
      $('count').innerHTML = '<b>' + n(s.done) + '</b> / ' + n(s.total) + ' assets (' + pct + '%)';
      var left = s.done > 0 ? s.elapsed / s.done * (s.total - s.done) : null;
      $('kv').innerHTML = row('Errors so far', n(s.errors)) + row('Elapsed', dur(s.elapsed)) + row('Time left (estimate)', left === null ? '-' : '~ ' + dur(left));
      $('note').textContent = s.stopRequested ? 'Stopping after the current asset...' : 'You may close this window - the run continues on the server.';
      $('stop').style.display = s.stopRequested ? 'none' : ''; $('hide').style.display = ''; $('ok').style.display = 'none';
      return true;
    }
    // finished / stopped / interrupted: summary
    var titles = { finished: s.errors > 0 ? 'Depreciation Completed with Errors' : 'Depreciation Completed', stopped: 'Depreciation Stopped', interrupted: 'Depreciation Interrupted' };
    $('title').textContent = titles[s.state] || 'Depreciation';
    $('bar').className = 'bar ' + (s.state === 'finished' && s.errors === 0 ? 'done' : 'err');
    $('count').innerHTML = '<b>' + n(s.done) + '</b> / ' + n(s.total) + ' assets processed';
    $('kv').innerHTML = row('Assets in Run', n(s.total)) + row('Generated', n(s.generated)) +
                        row('Skipped', n(s.skipped) + '') + row('Errors', n(s.errors)) + row('Time Taken', dur(s.elapsed));
    $('note').textContent = s.note || '';
    if (s.errors > 0) {
      $('errs').style.display = '';
      errLines = s.errorLines;
      $('errcsv').innerHTML = '<a href="' + base + '?run=' + run + '&csv=1">Download all errors (CSV)</a>';
      renderErrors();
    }
    $('stop').style.display = 'none'; $('hide').style.display = 'none'; $('ok').style.display = '';
    return false;
  }

  // ---- Error Details: search (F/A code or error text) and sort (click a column header) ----
  var errLines = [], sortCol = 0, sortAsc = true, MAX_ROWS = 500, searchTimer = null;
  function hi(text, words) {   // highlight the searched words
    var h = esc(text);
    for (var i = 0; i < words.length; i++) {
      var w = esc(words[i]).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
      if (w) h = h.replace(new RegExp('(' + w + ')(?![^<]*>)', 'ig'), '<mark>$1</mark>');
    }
    return h;
  }
  function renderErrors() {
    var q = $('errsearch').value.trim().toLowerCase();
    var words = q ? q.split(/\s+/) : [];
    var rows = [];
    for (var i = 0; i < errLines.length; i++) {
      var hay = (errLines[i][0] + ' ' + errLines[i][1]).toLowerCase(), ok = true;
      for (var j = 0; j < words.length && ok; j++) ok = hay.indexOf(words[j]) >= 0;   // every word must match
      if (ok) rows.push(errLines[i]);
    }
    rows.sort(function (a, b) {
      var x = a[sortCol].toLowerCase(), y = b[sortCol].toLowerCase();
      if (x === y) { x = a[1 - sortCol].toLowerCase(); y = b[1 - sortCol].toLowerCase(); }
      return (x < y ? -1 : x > y ? 1 : 0) * (sortAsc ? 1 : -1);
    });
    var html = '', shown = Math.min(rows.length, MAX_ROWS);
    for (var k = 0; k < shown; k++) html += '<tr><td>' + hi(rows[k][0], words) + '</td><td>' + hi(rows[k][1], words) + '</td></tr>';
    if (!rows.length) html = '<tr><td class="none" colspan="2">No error matches "' + esc(q) + '".</td></tr>';
    $('errbody').innerHTML = html;
    var info = q ? n(rows.length) + ' of ' + n(errLines.length) + ' match' : n(errLines.length) + ' error(s)';
    if (rows.length > MAX_ROWS) info += ' - showing first ' + n(MAX_ROWS) + ', search to narrow down';
    $('errinfo').textContent = info;
    var ths = document.querySelectorAll('.errs th');
    for (var t = 0; t < ths.length; t++)
      ths[t].querySelector('.arrow').textContent = (+ths[t].getAttribute('data-col') === sortCol) ? (sortAsc ? '\u25B2' : '\u25BC') : '';
  }
  $('errsearch').oninput = function () { clearTimeout(searchTimer); searchTimer = setTimeout(renderErrors, 150); };
  (function () {
    var ths = document.querySelectorAll('.errs th');
    for (var t = 0; t < ths.length; t++) ths[t].onclick = function () {
      var c = +this.getAttribute('data-col');
      if (c === sortCol) sortAsc = !sortAsc; else { sortCol = c; sortAsc = true; }
      renderErrors();
    };
  })();

  function poll() {
    var x = new XMLHttpRequest();
    x.open('GET', base + '?run=' + run + '&status=1&t=' + new Date().getTime(), true);
    x.onload = function () {
      var keep = false;
      try { keep = show(JSON.parse(x.responseText)); } catch (e) { $('msg').textContent = 'Could not read the progress.'; keep = true; }
      if (keep) timer = setTimeout(poll, 1000);
    };
    x.onerror = function () { timer = setTimeout(poll, 3000); };
    x.send();
  }
  if (run > 0) poll(); else { $('msg').textContent = 'Session expired - please open the Depreciation Widget again.'; $('ok').style.display = ''; }
})();
</script>
</body>
</html>
