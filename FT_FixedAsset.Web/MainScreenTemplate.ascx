<%@ Control Language="C#" CodeBehind="MainScreenTemplate.ascx.cs" ClassName="MainScreenTemplate" Inherits="FT_FixedAsset.Web.MainScreenTemplate" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v19.2, Version=19.2.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="xaf" %>

<div class="VerticalTemplate BodyBackColor">
    <xaf:XafUpdatePanel ID="UPPopupWindowControl" runat="server">
        <xaf:XafPopupWindowControl runat="server" ID="PopupWindowControl" />
    </xaf:XafUpdatePanel>
    <dx:ASPxGlobalEvents ID="GE" ClientSideEvents-EndCallback="AdjustSize" runat="server" />
    <table id="MT" border="0" width="100%" cellpadding="0" cellspacing="0" class="dxsplControl_<%= BaseXafPage.CurrentTheme %>">
        <tbody>
            <tr>
                <td style="vertical-align: top; height: 10px;" class="dxsplPane_<%= BaseXafPage.CurrentTheme %>">
                    <div id="VerticalTemplateHeader" class="VerticalTemplateHeader">
                        <table cellpadding="0" cellspacing="0" border="0" class="Top" width="100%">
                            <tr>
                                <td class="Logo">
                                    <asp:HyperLink runat="server" ID="LogoLink">
                                        <xaf:ThemedImageControl ID="TIC" DefaultThemeImageLocation="App_Themes/{0}/Xaf" ImageName="Logo.png"
                                            BorderWidth="0px" runat="server" />
                                    </asp:HyperLink>
                                </td>
                                <td class="Security">
                                    <xaf:XafUpdatePanel ID="UPSAC" UpdatePanelForASPxGridListCallback="False" runat="server">
                                        <xaf:ActionContainerHolder runat="server" ID="SAC" CssClass="Security"
                                            ContainerStyle="Links" ShowSeparators="True">
                                            <ActionContainers>
                                                <xaf:WebActionContainer ContainerId="Notifications" IsDropDown="false" />
                                                <xaf:WebActionContainer ContainerId="Security" IsDropDown="false" />
                                            </ActionContainers>
                                        </xaf:ActionContainerHolder>
                                    </xaf:XafUpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="ACPanel">
                            <tr class="Content">
                                <td class="Company Name" align="left">
                                    <h2><%= Company%></h2>
                                </td>
                                <td class="Content WithPaddings" align="right">
                                    <xaf:XafUpdatePanel ID="UPSHC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                        <xaf:ActionContainerHolder ID="SHC" runat="server" ContainerStyle="Links" CssClass="TabsContainer">
                                            <ActionContainers>
                                                <xaf:WebActionContainer ContainerId="RootObjectsCreation" IsDropDown="false" />
                                                <xaf:WebActionContainer ContainerId="Appearance" IsDropDown="false" />
                                                <xaf:WebActionContainer ContainerId="Search" IsDropDown="false" />
                                                <xaf:WebActionContainer ContainerId="FullTextSearch" IsDropDown="false" />
                                            </ActionContainers>
                                        </xaf:ActionContainerHolder>
                                    </xaf:XafUpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <table id="MRC" style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="LPcell" style="min-width: 200px; width: 200px; vertical-align: top">
                                <div id="LP" class="LeftPane">
                                    <xaf:XafUpdatePanel ID="UPNC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                        <xaf:NavigationActionContainer ID="NC" runat="server" CssClass="xafNavigationBarActionContainer"
                                            ContainerId="ViewsNavigation" Width="100%" />
                                    </xaf:XafUpdatePanel>
                                    <xaf:XafUpdatePanel ID="UPTP" runat="server">
                                        <div id="TP" runat="server" class="ToolsActionContainerPanel">
                                            <dx:ASPxRoundPanel ID="TRP" runat="server" HeaderText="Tools" CssClass="TRP" Width="170px">
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <xaf:ActionContainerHolder ID="VTC" runat="server" Menu-Width="100%"
                                                            Orientation="Vertical" ContainerStyle="Links" ShowSeparators="False">
                                                            <ActionContainers>
                                                                <xaf:WebActionContainer ContainerId="Tools" IsDropDown="false" />
                                                            </ActionContainers>
                                                        </xaf:ActionContainerHolder>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxRoundPanel>
                                            <xaf:ActionContainerHolder ID="DAC" runat="server" Orientation="Vertical"
                                                BorderWidth="0px" ContainerStyle="Links" ShowSeparators="False">
                                                <ActionContainers>
                                                    <xaf:WebActionContainer ContainerId="Diagnostic" IsDropDown="false" />
                                                </ActionContainers>
                                            </xaf:ActionContainerHolder>
                                            <br />
                                        </div>
                                    </xaf:XafUpdatePanel>
                                </div>
                            </td>
                            <td id="separatorCell" style="width: 6px; border-bottom-style: none; border-top-style: none"
                                class="dxsplVSeparator_<%= BaseXafPage.CurrentTheme %> dxsplPane_<%= BaseXafPage.CurrentTheme %>">
                                <div id="separatorButton" class="dxsplVSeparatorButton_<%= BaseXafPage.CurrentTheme %>" onmouseover="OnMouseEnter('separatorButton')"
                                    onmouseout="OnMouseLeave('separatorButton')" onclick="OnClick('LPcell','separatorImage',true)">
                                    <div id="separatorImage" style="width: 6px;" class="dxWeb_splVCollapseBackwardButton_<%= BaseXafPage.CurrentTheme %>">
                                    </div>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <xaf:XafUpdatePanel ID="UPTB" runat="server">
                                                    <xaf:ActionContainerHolder CssClass="ACH MainToolbar" runat="server" ID="TB" ContainerStyle="ToolBar" Orientation="Horizontal">
                                                        <Menu Width="100%" ItemAutoWidth="False" ClientInstanceName="mainMenu">
                                                            <BorderTop BorderStyle="None" />
                                                            <BorderLeft BorderStyle="None" />
                                                            <BorderRight BorderStyle="None" />
                                                        </Menu>
                                                        <ActionContainers>
                                                            <xaf:WebActionContainer ContainerId="ObjectsCreation" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="Edit" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="RecordEdit" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="View" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="Export" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="Reports" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="Filters" IsDropDown="false" />
                                                        </ActionContainers>
                                                    </xaf:ActionContainerHolder>
                                                </xaf:XafUpdatePanel>
                                                <xaf:XafUpdatePanel ID="UPVH" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                    <table id="VH" border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
                                                        <tr>
                                                            <td class="ViewHeader">
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="ViewHeader">
                                                                    <tr>
                                                                        <td class="ViewImage">
                                                                            <xaf:ViewImageControl ID="VIC" runat="server" />
                                                                        </td>
                                                                        <td class="ViewCaption">
                                                                            <h1>
                                                                                <xaf:ViewCaptionControl ID="VCC" runat="server" />
                                                                            </h1>
                                                                            <xaf:NavigationHistoryActionContainer ID="VHC" runat="server" CssClass="NavigationHistoryLinks"
                                                                                ContainerId="ViewsHistoryNavigation" Delimiter=" / " />
                                                                        </td>
                                                                        <td align="right">
                                                                            <xaf:ActionContainerHolder runat="server" ID="RNC" ContainerStyle="Links" Orientation="Horizontal"
                                                                                UseLargeImage="True" CssClass="RecordsNavigationContainer">
                                                                                <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                                                                <ActionContainers>
                                                                                    <xaf:WebActionContainer ContainerId="RecordsNavigation" IsDropDown="false" />
                                                                                </ActionContainers>
                                                                            </xaf:ActionContainerHolder>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </xaf:XafUpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <xaf:XafUpdatePanel ID="UPEMA" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                    <xaf:ActionContainerHolder runat="server" ID="EMA" ContainerStyle="Links" Orientation="Horizontal" CssClass="EditModeActions">
                                                        <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                                        <ActionContainers>
                                                            <xaf:WebActionContainer ContainerId="Save" IsDropDown="false" />
                                                            <xaf:WebActionContainer ContainerId="UndoRedo" IsDropDown="false" />
                                                        </ActionContainers>
                                                    </xaf:ActionContainerHolder>
                                                </xaf:XafUpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="CP" style="overflow: auto; width: 100%;">
                                                    <table border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
                                                        <tr class="Content">
                                                            <td class="Content">
                                                                <xaf:XafUpdatePanel ID="UPEI" runat="server" UpdatePanelForASPxGridListCallback="True">
                                                                    <xaf:ErrorInfoControl ID="ErrorInfo" Style="margin: 10px 0px 10px 0px" runat="server" />
                                                                </xaf:XafUpdatePanel>
                                                                <xaf:XafUpdatePanel ID="UPVSC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <xaf:ViewSiteControl ID="VSC" runat="server" />
                                                                    <xaf:ActionContainerHolder runat="server" ID="EditModeActions2" ContainerStyle="Links"
                                                                        Orientation="Horizontal" CssClass="EditModeActions">
                                                                        <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" Paddings-PaddingTop="15px" />
                                                                        <ActionContainers>
                                                                            <xaf:WebActionContainer ContainerId="Save" IsDropDown="false" />
                                                                            <xaf:WebActionContainer ContainerId="UndoRedo" IsDropDown="false" />
                                                                        </ActionContainers>
                                                                    </xaf:ActionContainerHolder>
                                                                </xaf:XafUpdatePanel>
                                                                <div id="Spacer" class="Spacer">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <xaf:XafUpdatePanel ID="UPQC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <xaf:QuickAccessNavigationActionContainer CssClass="Links NavigationLinks" ID="QC" runat="server"
                                                                        ContainerId="ViewsNavigation" PaintStyle="Caption" ShowSeparators="True" />
                                                                </xaf:XafUpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px; vertical-align: bottom" class="BodyBackColor">
                    <xaf:XafUpdatePanel ID="UPIMP" runat="server" UpdatePanelForASPxGridListCallback="False">
                        <asp:Literal ID="InfoMessagesPanel" runat="server" Text="" Visible="False"></asp:Literal>
                    </xaf:XafUpdatePanel>
                    <div id="Footer" class="Footer">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <div class="FooterCopyright">
                                        <xaf:AboutInfoControl ID="AIC" runat="server">Copyright text</xaf:AboutInfoControl>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

</div>
