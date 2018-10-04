<%@ Page Title="Production Dashboard" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductionDashboard.aspx.cs" Inherits="Ebiz.WebForm.custom.Production.ProductionDashboard" %>
 <%--EnableViewState="false"--%>
<%--<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Production Dashboard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        table div.dxb {
            padding: 0px !important;
        }

        div.div-header {
            text-align: center;
            border: 1px solid black;
            line-height: 27px;
            background: black;
        }

        .btn-new-line {
            float: left;
            height: 28px;
            width: 50px;
            line-height: 18px;
        }

        .label-line {
            font-weight: bold;
            color: white;
        }

        .label-section {
            font-weight: bold;
        }

        .dataview-master {
            margin: 5px 0;
            height: 42px;
            width: 42px;
            padding: 5px;
        }

        .dxdvItem_DevEx.dx-al {
            padding: 5px !important;
            height: 164px;
        }

        .dataview-master .dataview-item {
            height: 42px;
            width: 42px;
            padding: 5px;
        }

        .div-SubAssy {
            float: left;
            margin-right: 20px;
        }

        .div-section {
            border: 1px solid black;
            padding: 5px;
            margin-bottom: 10px;
        }

            .div-section.sa {
                display: inline-block;
                margin-bottom: 10px;
                width: 100%;
            }

        .btn-new-sa {
            margin-right: 5px;
            font-weight: bold;
            width: 20px;
            font-size: 1.2em;
        }

        .div-station-name {
            background: grey;
            text-align: center;
            height: 30px;
            color: white;
            font-weight: bold;
        }

        .span-station-name .label-station {
            vertical-align: middle;
        }

        .div-body, .div-footer {
            height: 15px;
        }

        .div-footer {
            text-align: center;
        }

        .img-notification {
            float: right;
            height: 15px;
            width: 15px;
        }

        .btn-details {
            padding: 0;
            height: 90px;
            max-height: 90px;
            width: 90px;
            max-width: 90px;
        }

            .btn-details img {
                width: 90px;
                max-height: 90px;
            }

            .btn-details span {
                width: 90px;
                max-height: 90px;
                font-size: 6em;
            }
    </style>
    <script type="text/javascript">
        function ExecuteRefresh(s, e) {
            setTimeout(function () { CallbackPanel.PerformCallback(); }, 30000) //30second
        };
        function BtnNew() {

        }
        // slight update to account for browsers not supporting e.which
        function disableF5(e) { if ((e.which || e.keyCode) == 116) e.preventDefault(); };

        window.onload = ExecuteRefresh;
        window.onkeydown = disableF5;

    </script>

    <dx:ASPxCallbackPanel runat="server" ID="cbpDashboard" ClientInstanceName="CallbackPanel" RenderMode="Div" OnCallback="cbpDashboard_Callback">
        <ClientSideEvents EndCallback="ExecuteRefresh"></ClientSideEvents>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent" runat="server">
                <table class="tableHeader" style="width: 100%;">
                    <tr>
                        <td>
                            <div style="padding-bottom: 15px; width: 300px; text-align: left">
                                <label style="font-weight: bold; font-size: small; vertical-align: text-top;">
                                    Overall Status
                                </label>
                                <br />
                                <dx:ASPxLabel ID="lblOverall" runat="server" Border-BorderStyle="Solid"
                                    Border-BorderWidth="1px" Font-Bold="True" Font-Size="2em">
                                </dx:ASPxLabel>
                            </div>
                        </td>
                        <td>
                            <div style="padding-bottom: 15px; width: 345px; text-align: center">
                                <label style="font-weight: bold; font-size: medium; vertical-align: text-top;">
                                    Production Dashboard
                                </label>
                                <br />
                                <label style="font-weight: bold; text-size-adjust: 75%">
                                    PT. Mercedez Benz Indonesia
                                </label>
                            </div>
                        </td>
                        <td>
                            <div style="padding-bottom: 15px; text-align: center">
                                <label style="font-weight: bold; font-size: small; vertical-align: text-top;">
                                    Daily Target
                                </label>
                                <br />
                                <dx:ASPxLabel ID="lblDailyTarget" runat="server" Border-BorderStyle="Solid"
                                    Border-BorderWidth="1px" Font-Bold="True" Font-Size="2em">
                                </dx:ASPxLabel>
                            </div>
                        </td>
                        <td>
                            <div style="padding-bottom: 15px; text-align: center">
                                <label style="font-weight: bold; font-size: small; vertical-align: text-top;">
                                    Monthly Target
                                </label>
                                <br />
                                <dx:ASPxLabel ID="lblMonthlyTarget" runat="server" Border-BorderStyle="Solid"
                                    Border-BorderWidth="1px" Font-Bold="True" Font-Size="2em">
                                </dx:ASPxLabel>
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="tableBody" style="width: 100%;">
                    <tbody>
                        <tr>
                            <td style="padding: 0px; margin: 10px;">
                                <div id="divMasterDataView" runat="server"></div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table class="tableFooter" style="text-align: center; border: solid 1px black">
                    <tr>
                        <td style="border: solid 1px black; width: 20%">
                            <div style="height: 15px; background: grey;">
                                <label style="vertical-align: middle; color: white">
                                    Offline
                                </label>
                            </div>
                            <div>
                                <dx:ASPxButton ID="btnOffline" runat="server" OnClick="btnOffline_Click"
                                    CommandArgument="isOffline" Width="100px" Height="100px" Font-Size="7em">
                                </dx:ASPxButton>
                            </div>
                        </td>
                        <td style="border: solid 1px black; width: 20%">
                            <div style="height: 15px; background: grey;">
                                <label style="vertical-align: middle; color: white">
                                    Rectification
                                </label>
                            </div>
                            <div>
                                <dx:ASPxButton ID="btnRectification" runat="server" OnClick="btnRectification_Click"
                                    CommandArgument="isRecti" Width="100px" Height="100px" Font-Size="7em">
                                </dx:ASPxButton>
                            </div>
                        </td>
                        <td style="border: solid 1px black; width: 20%">
                            <div style="height: 15px; background: grey;">
                                <dx:ASPxLabel runat="server" style="vertical-align: middle; color: white" id="lblVoca">
                                </dx:ASPxLabel>
                            </div>
                            <div>
                                <dx:ASPxButton ID="btnVoca" runat="server" OnClick="btnVoca_Click"
                                    CommandArgument="isVoca" Width="100px" Height="100px" Font-Size="7em">
                                </dx:ASPxButton>
                            </div>
                        </td>
                         <td id="tes" style="border: solid 1px black; width: 20%">
                            <div style="height: 15px; background: grey;">
                                 <dx:ASPxLabel runat="server" style="vertical-align: middle; color: white" id="lblPa">
                                </dx:ASPxLabel>
                            </div>
                            <div>
                                <dx:ASPxButton ID="btnPA" runat="server" OnClick="btnPA_Click"
                                    CommandArgument="isPA" Width="100px" Height="100px" Font-Size="7em">
                                </dx:ASPxButton>
                            </div>
                        </td>
                          <td id="pdi0" style="border: solid 1px black; width: 20%">
                            <div style="height: 15px; background: grey;">
                               <label style="vertical-align: middle; color: white">
                                    PDI-0
                                </label>
                            </div>
                          <div>
                                <dx:ASPxButton ID="btnPDI0" runat="server" OnClick="btnPDI0_Click"
                                    CommandArgument="isPDI0" Width="100px" Height="100px" Font-Size="7em">
                                </dx:ASPxButton>
                            </div>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
