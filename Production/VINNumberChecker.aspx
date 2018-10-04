<%@ Page Title="VIN Number Checker" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="VINNumberChecker.aspx.cs" Inherits="DotMercy.custom.VINNumberChecker" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
    Submit New CKD
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function ShowLoginWindow(parm1) {
            if (parm1 == 'Fault') {
                pcLogin2.Show();
            }
            else if (parm1 == 'Rectivication') {
                pcLoginRecti.Show();
            }
            else if (parm1 == 'NextStation') {
                pcLogin.Show();
            } else if (parm1 == 'Offline') {
                pcLoginOffiline.Show();
            } else if (parm1 == 'PA') {
                PcLoginPA.Show();
            } else if (parm1 == 'Release') {
                PcLoginRelease.Show();
            }

        }
    </script>
    <div class="content">
        <h2>VIN Number Checker</h2>
        <dx:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="btnVerify">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <dx:ASPxFormLayout ID="layoutVINChecker" runat="server">
                        <Items>
                            <dx:LayoutItem Caption="Serial Number Production">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="txtSerialNumberProd" runat="server" Width="170px">
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                    <div style="padding-left: 145px">
                        <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>
                        <dx:ASPxButton ID="btnVerify" runat="server" Text="Verify" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) { ShowLoginWindow('NextStation'); }" />
                        </dx:ASPxButton>

                        <asp:HiddenField ID="hdnLine" runat="server" />
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>

        <dx:ASPxPopupControl ID="PopupLogin" runat="server" Height="100px" Width="200px" Modal="True">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxFormLayout ID="layoutLogin" runat="server">
                        <Items>
                            <dx:LayoutItem Caption="Authorization Key">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="txtPassword" runat="server" Width="170px" Password="True">
                                        </dx:ASPxTextBox>
                                        <br />
                                        <dx:ASPxButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click">
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="pcLogin" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcLogin"
            HeaderText="AuthKey" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
            <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbLogin.Focus(); }" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                        <PanelCollection>
                            <dx:PanelContent runat="server">
                                <table>
                                    <tr>
                                        <td rowspan="4">
                                            <div class="pcmSideSpacer">
                                            </div>
                                        </td>
                                        <td class="pcmCellCaption">
                                            <dx:ASPxLabel ID="lblUsername1" runat="server" Width="100px" Text="AuthKey : " AssociatedControlID="tbLogin">
                                            </dx:ASPxLabel>
                                        </td>

                                        <td>
                                            <div class="pcmCellText">
                                                <dx:ASPxTextBox ID="tbLogin" runat="server" Password="true" Width="150px" ClientInstanceName="tbLogin">
                                                </dx:ASPxTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="pcmButton">
                                                <span>&nbsp;</span>
                                                <dx:ASPxButton ID="btOK" runat="server" Text="OK" Width="70px" AutoPostBack="False" OnClick="btnVerify_Click">
                                                </dx:ASPxButton>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="pcmButton">
                                                <span>&nbsp;</span>
                                                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel" Width="50px" AutoPostBack="False">
                                                    <ClientSideEvents Click="function(s, e) { pcLogin.Hide(); }" />
                                                </dx:ASPxButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>

                </dx:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle>
                <Paddings PaddingBottom="5px" />
            </ContentStyle>
        </dx:ASPxPopupControl>

    </div>
</asp:Content>
