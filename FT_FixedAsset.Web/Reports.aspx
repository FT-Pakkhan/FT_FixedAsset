<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="FT_FixedAsset.Web.Reports" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" ToolPanelWidth="100%" Width="100%" Height="100%" EnableParameterPrompt="False" ReuseParameterValuesOnRefresh="True"/>
        </div>
        <script type="text/javascript">
            // Fix: report moves right a little on every page change when Windows display scaling is not 100%.
            // At 125% the viewer's 1px borders become 0.8px; Crystal's bobj.setOuterSize reads them with parseInt
            // (0.8 -> 0), so each layout makes the left panel wider. Same function, but reading the real (decimal) sizes.
            if (window.bobj && bobj.setOuterSize) {
                bobj.setOuterSize = function (f, b, e, c) {
                    var k = null; var a = f.style;
                    if (a.display == "none") { k = { visibility: a.visibility, position: a.position, display: "none" }; a.visibility = "hidden"; a.position = "absolute"; a.display = ""; }
                    function g(l) { var m = MochiKit.DOM.getStyle(f, l); return bobj.isString(m) ? (parseFloat(m) || 0) : 0; }
                    if (bobj.isNumber(b)) {
                        if (!bobj.isBorderBoxModel()) { b -= g("border-left-width"); b -= g("border-right-width"); b -= g("padding-left"); b -= g("padding-right"); if (c) { b -= g("margin-left"); b -= g("margin-right"); } }
                        a.width = Math.max(0, b) + "px";
                    }
                    if (bobj.isNumber(e)) {
                        if (!bobj.isBorderBoxModel()) { if (c) { e -= g("margin-top"); e -= g("margin-bottom"); } e -= g("border-top-width"); e -= g("border-bottom-width"); e -= g("padding-top"); e -= g("padding-bottom"); }
                        a.height = Math.max(0, e) + "px";
                    }
                    if (k) { a.display = k.display; a.position = k.position; a.visibility = k.visibility; }
                };
            }
        </script>
    </form>
</body>
</html>
