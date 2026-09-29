<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>

<%@ Register Assembly="DevExpress.ExpressApp.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
    <style type="text/css">
        * {
            font-size: 13px !important;
            /* font-weight: bold !important;*/
            font-family: Calibri !important;
        }

        /*#region Gray out for disabled field */
        .dxeEditArea_SoftOrange.dxeEditAreaSys.dxeDisabled_SoftOrange {
            background-color: lightgray;
            color: black;
        }

        .dxeDisabled_SoftOrange td.dxic {
            background-color: lightgray;
        }

        .dxeEditArea_Office2010Blue.dxeEditAreaSys.dxeDisabled_Office2010Blue {
            background-color: lightgray;
            color: black;
        }

        .dxeDisabled_Office2010Blue td.dxic {
            background-color: lightgray;
        }
    </style>
</head>
<body class="VerticalTemplate">
    <form id="form2" runat="server">
        <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
        <div runat="server" id="Content" />
    </form>
</body>
</html>
