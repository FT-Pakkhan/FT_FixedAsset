<%@ Page Language="C#" EnableSessionState="ReadOnly" EnableViewState="false" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="FT_FixedAsset.Module.Web.Import" %>
<script runat="server">
    // Data Import result window (opened in an overlay by ImportController after Import / Run Simulation).
    // Same look as the Run Depreciation summary (DeprnProgress.aspx).
    //   (no query)  the window, built from this session's last import
    //   ?csv=1      all lines as CSV (or only the errors with &errors=1)
    // Only for a logged-on user (forms authentication); the data never leaves the user's own session.

    protected ImportSummary S { get { return Session[ImportSummary.SessionKey] as ImportSummary; } }

    private static string J(string s) { return "\"" + HttpUtility.JavaScriptStringEncode(s ?? "") + "\""; }
    private static string Csv(string s) { return "\"" + (s ?? "").Replace("\"", "\"\"") + "\""; }

    protected string DataJson()
    {
        ImportSummary s = S;
        if (s == null) return "null";
        var sb = new StringBuilder("{");
        sb.Append("\"title\":").Append(J(s.Title))
          .Append(",\"caption\":").Append(J(s.Caption))
          .Append(",\"simulation\":").Append(s.Simulation ? "true" : "false")
          .Append(",\"success\":").Append(s.Success ? "true" : "false")
          .Append(",\"committed\":").Append(s.Committed ? "true" : "false")
          .Append(",\"file\":").Append(J(s.FileName))
          .Append(",\"linesFile\":").Append(J(s.LinesFileName))
          .Append(",\"startBy\":").Append(J(s.StartBy))
          .Append(",\"started\":").Append(J(s.Started.ToString("dd/MM/yyyy hh:mm tt")))
          .Append(",\"seconds\":").Append(s.Seconds.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture))
          .Append(",\"records\":").Append(s.Records)
          .Append(",\"errors\":").Append(s.Errors)
          .Append(",\"note\":").Append(J(s.Note))
          .Append(",\"lines\":[");
        for (int i = 0; i < s.Lines.Count; i++)
        {
            string[] l = s.Lines[i];
            sb.Append(i == 0 ? "" : ",").Append("[").Append(J(l[0])).Append(",").Append(J(l[1])).Append(",")
              .Append(J(l[2])).Append(",").Append(J(l[3])).Append(",").Append(J(l[4])).Append("]");
        }
        sb.Append("]}");
        return sb.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        ImportSummary s = S;
        if (Request.QueryString["csv"] == "1" && s != null)
        {
            bool errorsOnly = Request.QueryString["errors"] == "1";
            var sb = new StringBuilder("Row,Import Doc No.,Code / Doc No.,Status,Message\r\n");
            foreach (string[] l in s.Lines.Where(x => !errorsOnly || x[3] == "Error"))
                sb.Append(Csv(l[0])).Append(",").Append(Csv(l[1])).Append(",").Append(Csv(l[2])).Append(",")
                  .Append(Csv(l[3])).Append(",").Append(Csv(l[4])).Append("\r\n");
            string name = "Import_" + new string(s.Caption.Where(char.IsLetterOrDigit).ToArray())
                        + (s.Simulation ? "_Simulation" : "") + (errorsOnly ? "_Errors" : "") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            Response.ContentEncoding = Encoding.UTF8;
            Response.BinaryWrite(Encoding.UTF8.GetPreamble());
            Response.Write(sb.ToString());
            Response.End();
        }
    }
</script>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8" />
<title>Import Result</title>
<style>
  html, body { margin: 0; height: 100%; font-family: Tahoma, Segoe UI, Arial, sans-serif; font-size: 13px; color: #000; background: #fff; }
  .wrap { display: flex; flex-direction: column; height: 100%; }
  .head { background: #d4e1f2; border-bottom: 1px solid #a9bfd6; padding: 8px 12px; font-weight: bold; }
  .body { flex: 1; overflow: auto; padding: 14px 18px; }
  .foot { border-top: 1px solid #c5d3e4; background: #eef3fa; padding: 8px 12px; text-align: right; }
  .bar { height: 18px; background: #e6ebf2; border: 1px solid #b7c6da; border-radius: 2px; overflow: hidden; margin: 8px 0 4px; }
  .bar div { height: 100%; width: 100%; background: #5aa95a; }
  .bar.err div { background: #d9822b; }
  table.kv { border-collapse: collapse; margin-top: 6px; }
  table.kv td { padding: 2px 14px 2px 0; }
  table.kv td.v { text-align: right; font-weight: bold; min-width: 80px; }
  .muted { color: #555; }
  .note { margin-top: 8px; color: #8a4b00; max-height: 72px; overflow: auto; }
  .note.ok { color: #2e6b2e; }
  .errs { margin-top: 12px; }
  .errs .list { max-height: calc(100vh - 380px); min-height: 90px; overflow: auto; border: 1px solid #c5d3e4; }
  .errs table { border-collapse: collapse; width: 100%; }
  .errs th, .errs td { text-align: left; padding: 3px 6px; border-bottom: 1px solid #e3e9f1; vertical-align: top; }
  .errs td.num { text-align: right; }
  .errs th { background: #eef3fa; position: sticky; top: 0; cursor: pointer; user-select: none; white-space: nowrap; }
  .errs th:hover { background: #dde7f4; }
  .errs th .arrow { color: #1f5fa8; font-size: 10px; margin-left: 4px; }
  .errs td.st-Error { color: #b02a2a; font-weight: bold; }
  .errtools { display: flex; align-items: center; gap: 10px; margin: 6px 0; flex-wrap: wrap; }
  .errtools input[type=search] { flex: 1; min-width: 160px; font: inherit; padding: 3px 6px; border: 1px solid #8ba0bc; border-radius: 2px; }
  .errs td.none { color: #555; text-align: center; padding: 10px; }
  mark { background: #ffe58a; padding: 0; }
  button { font: inherit; padding: 4px 16px; margin-left: 6px; border: 1px solid #8ba0bc; background: #f4f7fb; border-radius: 2px; cursor: pointer; }
  button.primary { background: #ffe8a0; border-color: #c9a227; }
  a { color: #1f5fa8; }
</style>
</head>
<body>
<div class="wrap">
  <div class="head" id="title">Import Result</div>
  <div class="body">
    <div id="msg" class="muted" style="display:none">No import result found - please import again.</div>
    <div id="main" style="display:none">
      <div><b id="caption"></b> &nbsp;<span class="muted" id="who"></span></div>
      <div class="bar" id="bar"><div></div></div>
      <table class="kv" id="kv"></table>
      <div class="note" id="note"></div>
      <div class="errs" id="errs" style="display:none">
        <b>Details</b> &nbsp;<span id="csv"></span>
        <div class="errtools">
          <input id="search" type="search" placeholder="Search row, code or message (e.g. LOC01, not found)" autocomplete="off" />
          <label id="onlylbl"><input id="only" type="checkbox" /> Errors only</label>
          <span class="muted" id="info"></span>
        </div>
        <div class="list"><table><thead><tr id="hdr"></tr></thead><tbody id="rows"></tbody></table></div>
      </div>
    </div>
  </div>
  <div class="foot"><button id="ok" class="primary">OK</button></div>
</div>
<script>
(function () {
  var s = <%= DataJson() %>, base = location.pathname;
  function $(id) { return document.getElementById(id); }
  function n(v) { return (v || 0).toLocaleString('en-US'); }
  function esc(t) { return String(t).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }
  function dur(sec) { if (sec < 60) return (Math.round(sec * 10) / 10) + ' s';
                      sec = Math.round(sec); var m = Math.floor(sec / 60); return m + ' min ' + (sec % 60) + ' s'; }
  function row(label, value) { return '<tr><td>' + label + '</td><td class="v">' + value + '</td></tr>'; }
  $('ok').onclick = function () { try { window.top.FT_CloseImportResult(); } catch (e) { } };
  if (!s) { $('msg').style.display = ''; return; }

  $('title').textContent = s.title + ' - ' + s.caption;
  $('main').style.display = '';
  $('caption').textContent = 'File: ' + s.file + (s.linesFile ? ' + ' + s.linesFile : '');
  $('who').textContent = '(by ' + s.startBy + ', ' + s.started + ')';
  $('bar').className = 'bar' + (s.success ? '' : ' err');
  $('kv').innerHTML =
      row(s.simulation ? 'Records Valid' : (s.committed ? 'Records Imported' : 'Records Valid'), n(s.records)) +
      row('Errors', n(s.errors)) +
      row('Saved', s.committed ? 'Yes' : 'No' + (s.simulation ? ' (simulation)' : '')) +
      row('Time Taken', dur(s.seconds));
  $('note').textContent = s.note || '';
  $('note').className = 'note' + (s.success ? ' ok' : '');

  // ---- Details: search, sort (click a column header), errors only ----
  var hasDoc = s.lines.some(function (l) { return l[1]; });
  var cols = [ { i: 0, name: 'Row', w: '50px', num: true }, { i: 1, name: 'Import Doc No.', w: '110px' },
               { i: 2, name: 'Code / Doc No.', w: '130px' }, { i: 3, name: 'Status', w: '60px' }, { i: 4, name: 'Message' } ]
             .filter(function (c) { return c.i !== 1 || hasDoc; });
  var sortCol = 0, sortAsc = true, MAX_ROWS = 500, timer = null;
  if (s.lines.length) {
    $('errs').style.display = '';
    $('only').checked = s.errors > 0;
    $('onlylbl').style.display = s.errors > 0 && s.errors < s.lines.length ? '' : 'none';
    $('csv').innerHTML = '<a href="' + base + '?csv=1">Download all (CSV)</a>' +
                         (s.errors > 0 ? ' &nbsp;<a href="' + base + '?csv=1&errors=1">Download errors (CSV)</a>' : '');
    $('hdr').innerHTML = cols.map(function (c) {
      return '<th data-col="' + c.i + '"' + (c.w ? ' style="width:' + c.w + '"' : '') + ' title="Click to sort">' + c.name + '<span class="arrow"></span></th>';
    }).join('');
  }
  function hi(text, words) {
    var h = esc(text);
    for (var i = 0; i < words.length; i++) {
      var w = esc(words[i]).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
      if (w) h = h.replace(new RegExp('(' + w + ')(?![^<]*>)', 'ig'), '<mark>$1</mark>');
    }
    return h;
  }
  function key(l, c) { return c === 0 ? (parseInt(l[0], 10) || 0) : String(l[c]).toLowerCase(); }
  function render() {
    var q = $('search').value.trim().toLowerCase(), words = q ? q.split(/\s+/) : [], only = $('only').checked;
    var list = s.lines.filter(function (l) {
      if (only && l[3] !== 'Error') return false;
      var hay = l.join(' ').toLowerCase();
      for (var j = 0; j < words.length; j++) if (hay.indexOf(words[j]) < 0) return false;
      return true;
    });
    list.sort(function (a, b) {
      var x = key(a, sortCol), y = key(b, sortCol);
      if (x === y) { x = key(a, 0); y = key(b, 0); }
      return (x < y ? -1 : x > y ? 1 : 0) * (sortAsc ? 1 : -1);
    });
    var html = '', shown = Math.min(list.length, MAX_ROWS);
    for (var k = 0; k < shown; k++) {
      html += '<tr>' + cols.map(function (c) {
        return '<td class="' + (c.num ? 'num' : '') + (c.i === 3 ? ' st-' + esc(list[k][3]) : '') + '">' + hi(list[k][c.i], words) + '</td>';
      }).join('') + '</tr>';
    }
    if (!list.length) html = '<tr><td class="none" colspan="' + cols.length + '">Nothing matches' + (q ? ' "' + esc(q) + '"' : '') + '.</td></tr>';
    $('rows').innerHTML = html;
    var info = (q || only) ? n(list.length) + ' of ' + n(s.lines.length) + ' line(s)' : n(s.lines.length) + ' line(s)';
    if (list.length > MAX_ROWS) info += ' - showing first ' + n(MAX_ROWS) + ', search to narrow down';
    $('info').textContent = info;
    var ths = document.querySelectorAll('.errs th');
    for (var t = 0; t < ths.length; t++)
      ths[t].querySelector('.arrow').textContent = (+ths[t].getAttribute('data-col') === sortCol) ? String.fromCharCode(sortAsc ? 9650 : 9660) : '';   // up / down triangle (kept ASCII: the file has no BOM)
  }
  $('search').oninput = function () { clearTimeout(timer); timer = setTimeout(render, 150); };
  $('only').onchange = render;
  var ths = document.querySelectorAll('.errs th');
  for (var t = 0; t < ths.length; t++) ths[t].onclick = function () {
    var c = +this.getAttribute('data-col');
    if (c === sortCol) sortAsc = !sortAsc; else { sortCol = c; sortAsc = true; }
    render();
  };
  if (s.lines.length) render();
})();
</script>
</body>
</html>
