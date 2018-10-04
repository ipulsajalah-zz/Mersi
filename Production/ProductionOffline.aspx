<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductionOffline.aspx.cs" Inherits="DotMercy.custom.Production.ProductionOffline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Production Rectification
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .img-detail {
            max-height: 90px;
            width: 120px;
        }

        .img-notification {
            float: left;
            height: 20px;
            width: 20px;
        }

        table#table-header td {
            padding: 0 5px;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function CloseUploadControl() {
            PopUpAttachMent.Hide();

            PopUpAttachMent.PerformCallback();

            pcInteruptFault.PerformCallback();
            cbFINNumber.SetText = cbFINNumber.GetText();
            //GVDataAttachment.Refresh();
        }
        function GetLine() {
            var s = cbFINNumber.GetValue();
            PageMethods.GetLine(s, OnGetLine);
        }
        function OnGetLine(response, userContext, methodName) {
            tbLine.SetText(response)
        }
        function GetSection() {
            var s = cbStation.GetValue();
            PageMethods.GetSection(s, OnGetSection);
        }
        function OnGetSection(response, userContext, methodName) {
            tbAssemblySection.SetText(response)
        }
        function GetClassification() {
            var s = cbFaultDesc.GetValue();
            PageMethods.GetClassification(s, OnGetClassification);
        }
        function OnGetClassification(response, userContext, methodName) {
            tbClassification.SetText(response)
        }
        function GetCgis() {
            cbCGIS.PerformCallback();
        }
        function Reset(s, e) {
            pcInteruptFault.PerformCallback();
        }
        function ShowLoginWindow(parm1) {
            if (parm1 == 'Fault') {
                pcLogin2.Show();
            }
            else if (parm1 == 'Trim') {
                TrimPopUp.Show();
            }
            else if (parm1 == 'Mech') {
                MechValidasi.Show();
            }
            else if (parm1 == 'Rectivication') {
                pcLoginRecti.Show();
            }
            else if (parm1 == 'NextStation') {
                pcLogin.Show();
            } else if (parm1 == 'Offline') {
                pcLoginOffiline.Show();//
            } else if (parm1 == 'PA') {
                PcLoginPA.Show();
            } else if (parm1 == 'Release') {
                PcLoginRelease.Show();
            } else if (parm1 == 'Problem') {
                PcProblemAuth.Show();
            } else if (parm1 == 'DeleteFault') {
                // FormConfirmDelete.Show();
            } else if (parm1 == 'SendStation') {
                ValidasiBackStation.Show();
            }

        }
    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <dx:ASPxHiddenField runat="server" ID="HiddenText" ClientInstanceName="HiddenText"></dx:ASPxHiddenField>
    <dx:ASPxPanel ID="PanelHeader" runat="server">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <table style="width: 100%; margin-bottom: 10px" id="table-header">
                    <tr>
                        <td colspan="10" style="text-align: center; padding-bottom: 10px">
                            <dx:ASPxLabel ID="lblStation" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr style="border: solid 1px">
                        <td>
                            <dx:ASPxLabel Text="Chassis No." runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="Vehicle No." runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="CommNos No." runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="Packing Month" runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="Engine No." runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="Local Prod. No." runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="Model" runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="FDoc" runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel Text="Paint" runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <td style="border-right: solid 1px">
                            <dx:ASPxLabel Text="Interior" runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>
                        <%--<td>
                            <dx:ASPxLabel Text="Option Code" runat="server" Font-Bold="true"></dx:ASPxLabel>
                        </td>--%>
                    </tr>
                    <tr style="border: solid 1px">
                        <td>
                            <dx:ASPxLabel ID="lblVINNumber" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblFINNumber" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblCommnos" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblPackingMonth" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblEngineNo" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblLocalProdNo" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblModelVariant" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblFDoc" runat="server"></dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="lblPaintCode" runat="server"></dx:ASPxLabel>
                        </td>
                        <td style="border-right: solid 1px">
                            <dx:ASPxLabel ID="lblInteriorCode" runat="server"></dx:ASPxLabel>
                        </td>
                        <%--<td>
                            <dx:ASPxMemo ID="moOptionCode" AutoPostBack="false" runat="server" Enabled="false" Width="100%" Height="30px"></dx:ASPxMemo>
                        </td>--%>
                    </tr>
                    <tr style="border: solid 1px">
                        <td colspan="10" style="border-right: solid 1px; padding: 0">
                            <dx:ASPxMemo ID="moOptionCode" AutoPostBack="false" runat="server" Enabled="false" Width="100%" Height="30px"></dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr style="border: solid 1px">
                        <td rowspan="3">
                            <asp:ImageButton ID="detailCondition" runat="server" />
                            <asp:ImageButton ID="detailImg" runat="server" />
                        </td>
                        <td rowspan="3" colspan="10">
                            <div>
                                <div style="text-align: center">
                                    <dx:ASPxLabel Text="Control Panel" runat="server" Font-Size="Small" Font-Bold="true"></dx:ASPxLabel>
                                </div>
                                <%-- <div style="display:inline-block; padding:5px">
                                    <dx:ASPxButton ID="btnNextProcess" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Move to Next Station" Font-Bold="true" Font-Size="13px" OnClick="btnNextProcess_Click">
                                    </dx:ASPxButton>
                                </div>--%>
                                <div style="display: inline-block; padding: 5px">
                                    <%-- <dx:ASPxButton ID="btnSendToTrim" runat="server" Visible="false" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Send to Trim" Font-Bold="true" Font-Size="13px">
                                         <ClientSideEvents Click="function(s, e) { ShowLoginWindow('Trim'); }" />
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnSendToMechHeader" Visible="false" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Send to Mechanical" Font-Bold="true" Font-Size="13px">
                                         <ClientSideEvents Click="function(s, e) { ShowLoginWindow('Mech'); }" />
                                    </dx:ASPxButton>--%>
                                    <dx:ASPxButton ID="btnSendToStation" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Send to Station" Font-Bold="true" Font-Size="13px">
                                        <ClientSideEvents Click="function(s, e) { ShowLoginWindow('SendStation'); }" />
                                    </dx:ASPxButton>
                                </div>
                                <%--<div style="display:inline-block; padding:5px">
                                </div>                                
                                <div style="display:inline-block; padding:5px">
                                    <dx:ASPxButton ID="btnRelease" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Release Vehicle" Font-Bold="true" Font-Size="13px" OnClick="btnRelease_Click">
                                    </dx:ASPxButton>
                                </div>--%>
                                <div style="display: inline-block; padding: 5px">
                                    <dx:ASPxButton ID="btnFault" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Add Fault Record" Font-Bold="true" Font-Size="13px">
                                        <ClientSideEvents Click="function(s, e) { ShowLoginWindow('Fault'); }" />
                                    </dx:ASPxButton>
                                    <%-- <dx:ASPxButton ID="BtnEndOffLine" runat="server" AutoPostBack="False" Font-Bold="True" Font-Size="13px" Height="40px" OnClick="BtnEndOffLine_Click" Text="Send To End of Line" Visible="False" Width="180px">
                                    </dx:ASPxButton>--%>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>

    <dx:ASPxPageControl ID="tabDetail" runat="server" ActiveTabIndex="0" EnableHierarchyRecreation="True" EnableCallBacks="false" Width="100%" Height="400px">
        <TabPages>
            <dx:TabPage Text="Control Plan" Name="tabProcess">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxGridView ID="cpPerStationGrid" ClientInstanceName="cpPerStationGrid" runat="server" KeyFieldName="Id"
                            AutoGenerateColumns="False" SettingsBehavior-AllowSort="false" Width="100%" OnDataBinding="cpPerStationGrid_DataBinding">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="2">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ControlPlanId" Visible="False" VisibleIndex="3">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProcessNo" Visible="False" VisibleIndex="4">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StationId" Visible="False" VisibleIndex="5">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProcessName" VisibleIndex="6" FixedStyle="Left" Width="150px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="CgisNo" VisibleIndex="7">
                                    <DataItemTemplate>
                                        <dx:ASPxButton ID="btnCgisNo" runat="server" RenderMode="Link" Text='<%#Eval("CgisNo") %>' CommandArgument='<%#Eval("Id") %>'
                                            AutoPostBack="False" OnDataBinding="btnCgisNo_DataBinding">
                                        </dx:ASPxButton>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Working Instruction" VisibleIndex="8" FieldName="WI" Width="140px">
                                    <Settings AllowGroup="False" />
                                    <EditFormSettings Visible="False" />
                                    <FilterCellStyle Wrap="True">
                                    </FilterCellStyle>
                                    <DataItemTemplate>
                                        <dx:ASPxGridView ID="grvWIDocs" runat="server" AutoPostBack="false" AutoGenerateColumns="False" KeyFieldName="Id" CssClass="innerGrid">
                                            <Settings ShowColumnHeaders="False" GridLines="None"></Settings>
                                            <Columns>
                                                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="Title">
                                                    <DataItemTemplate>
                                                        <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false" Text='<%#Eval("Title") %>'
                                                            CommandArgument='<%#Eval("Id") %>' ID="btnDownloadWI"
                                                            OnClick="btnDownloadWI_Click">
                                                        </dx:ASPxButton>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsPager Visible="False">
                                            </SettingsPager>
                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                        </dx:ASPxGridView>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Inspection Instruction" VisibleIndex="9" FieldName="II" Width="140px">
                                    <Settings AllowGroup="False" />
                                    <EditFormSettings Visible="False" />
                                    <FilterCellStyle Wrap="True">
                                    </FilterCellStyle>
                                    <DataItemTemplate>
                                        <dx:ASPxGridView ID="grvIIDocs" runat="server" AutoPostBack="false" AutoGenerateColumns="False" CssClass="innerGrid">
                                            <Settings ShowColumnHeaders="False" GridLines="None"></Settings>
                                            <Columns>
                                                <dx:GridViewDataTextColumn VisibleIndex="0">
                                                    <DataItemTemplate>
                                                        <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false" Text='<%#Eval("Title") %>'
                                                            CommandArgument='<%#Eval("Id") %>' ID="btnDownloadII"
                                                            OnClick="btnDownloadII_Click">
                                                        </dx:ASPxButton>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsPager Visible="False">
                                            </SettingsPager>
                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                        </dx:ASPxGridView>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Rf" VisibleIndex="10" Caption="RF" Width="30px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="DialogAddress" VisibleIndex="11" Width="165px">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel runat="server" ID="lblDialog" />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="PartNumber" VisibleIndex="12" Width="110px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="PartDescription" VisibleIndex="13" Width="150px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Qty" VisibleIndex="14" Width="35px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Class" VisibleIndex="15">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn FieldName="VDoc" VisibleIndex="16">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel ID="lblVDoc" runat="server" Font-Bold="True" Font-Size="Medium">
                                        </dx:ASPxLabel>
                                    </DataItemTemplate>
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewDataCheckColumn FieldName="SecondHand" VisibleIndex="17" Caption="2 Hand">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel ID="lbl2Hand" runat="server" Font-Bold="True" Font-Size="Medium">
                                        </dx:ASPxLabel>
                                    </DataItemTemplate>
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewDataTextColumn FieldName="FBS" VisibleIndex="18">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn FieldName="DS" VisibleIndex="19">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel ID="lblDS" runat="server" Font-Bold="True" Font-Size="Medium">
                                        </dx:ASPxLabel>
                                    </DataItemTemplate>
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewDataCheckColumn FieldName="CS2" VisibleIndex="20">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel ID="lblCS2" runat="server" Font-Bold="True" Font-Size="Medium">
                                        </dx:ASPxLabel>
                                    </DataItemTemplate>
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewDataCheckColumn FieldName="CS3" VisibleIndex="21">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel ID="lblCS3" runat="server" Font-Bold="True" Font-Size="Medium">
                                        </dx:ASPxLabel>
                                    </DataItemTemplate>
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewDataTextColumn FieldName="DRT" VisibleIndex="22">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StampNo" VisibleIndex="23" Caption="Stamp Nr">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Rack" FieldName="Rk" VisibleIndex="24">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="TrolleyId" VisibleIndex="25">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Items="10,20,50" Visible="True">
                                </PageSizeItemSettings>
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" VerticalScrollBarMode="Visible" VerticalScrollableHeight="295" VerticalScrollBarStyle="Standard" />
                            <SettingsSearchPanel Visible="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" />
                            </Styles>
                        </dx:ASPxGridView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Fault Records" Name="tabFaults">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxGridView ID="gridFault" runat="server" KeyFieldName="FaultRecordId" Width="100%"
                            OnRowUpdating="gridFault_RowUpdating" OnDataBinding="gridFault_DataBinding">
                            <Columns>
                                <dx:GridViewDataTextColumn VisibleIndex="0">
                                    <DataItemTemplate>
                                        <dx:ASPxButton ID="Btn_Edit" runat="server" RenderMode="Link" Text="Edit" OnClick="Btn_Edit_Click">
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="Btn_Delete" runat="server" RenderMode="Link" Text="Delete" OnClick="BtnSaveIndex">
                                            <ClientSideEvents Click="function(s, e) { ShowLoginWindow('DeleteFault'); }" />
                                        </dx:ASPxButton>
                                    </DataItemTemplate>

                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultRecordId" VisibleIndex="1" Visible="false" ReadOnly="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProductionHistoryId" VisibleIndex="2" Visible="false" ReadOnly="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataComboBoxColumn FieldName="FaultStatus" VisibleIndex="3" Width="80px">
                                </dx:GridViewDataComboBoxColumn>
                                <dx:GridViewDataTextColumn FieldName="FINNumber" Caption="FIN Number" VisibleIndex="4" Width="135px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="CSType" VisibleIndex="5" Width="55px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultNumber" VisibleIndex="6" Width="90px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProductionLineId" VisibleIndex="7" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProductionLineName" Caption="Line Name" VisibleIndex="8" Width="70px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AssemblySectionId" VisibleIndex="9" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AssemblySectionName" Caption="Section Name" VisibleIndex="10" Width="90px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StationId" VisibleIndex="11" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StationName" VisibleIndex="12" Width="100px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="QualityGateId" VisibleIndex="13" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="QualityGateName" Caption="Quality Gate" VisibleIndex="14" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultPartProcessId" VisibleIndex="15" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultPartProcessDesc" Caption="Fault Part Process" VisibleIndex="16" Width="170px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultRelatedTypeId" VisibleIndex="17" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultRelatedTypeDesc" Caption="Fault Related Type" VisibleIndex="18" Width="120px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultDescriptionId" VisibleIndex="19" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FaultDescriptionText" Caption="Fault Description" VisibleIndex="20" Width="170px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="CGISId" VisibleIndex="21" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="CGISNo" VisibleIndex="22" Width="120px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="RF" VisibleIndex="23" Width="25px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="24" Width="70px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="NCP" VisibleIndex="25" Width="95px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="RecordDate" VisibleIndex="26" Width="150px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="InspectorUserId" VisibleIndex="27" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="InspectorName" VisibleIndex="28" Width="120px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StamperUserId" VisibleIndex="29" Visible="false" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StamperName" VisibleIndex="30" Width="120px" ReadOnly="true">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Remarks" VisibleIndex="31" Width="120px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="IsSentToRectification" VisibleIndex="32" Width="140px">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Items="10,20,50" Visible="True">
                                </PageSizeItemSettings>
                            </SettingsPager>
                            <SettingsEditing Mode="EditFormAndDisplayRow" />
                            <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" VerticalScrollBarMode="Visible" VerticalScrollableHeight="295" VerticalScrollBarStyle="Standard" />
                            <SettingsSearchPanel Visible="True" />
                            <Styles>
                                <%--<Cell HorizontalAlign="Center"></Cell>--%>
                                <Header HorizontalAlign="Center" />
                            </Styles>
                        </dx:ASPxGridView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Irregular Alteration" Name="tabAlteration">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxGridView ID="gridIrregular" runat="server" AutoPostBack="false" KeyFieldName="Id" Width="100%" OnDataBinding="gridIrregular_DataBinding">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="0" Visible="false"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ProductionHistoryId" VisibleIndex="1"></dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Items="10,20,50" Visible="True">
                                </PageSizeItemSettings>
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" VerticalScrollBarMode="Visible" VerticalScrollableHeight="295" VerticalScrollBarStyle="Standard" />
                            <SettingsSearchPanel Visible="True" />
                            <Styles>
                                <Header HorizontalAlign="Center" />
                            </Styles>
                        </dx:ASPxGridView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
    <dx:ASPxPopupControl ID="pcLogin2" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcLogin2"
        HeaderText="AuthKey" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbLogin2.Focus(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel3" runat="server" DefaultButton="btOK2">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
                                        </div>
                                    </td>
                                    <td class="pcmCellCaption">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Width="100px" Text="AuthKey : " Style="float: left; margin-right: 8px;" AssociatedControlID="tbLogin">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="pcmCellText">
                                        <dx:ASPxTextBox ID="tbLogin2" runat="server" Password="true" Width="150px" ClientInstanceName="tbLogin2">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="btOK2" runat="server" Text="OK" Width="70px" AutoPostBack="False" Style="float: left; margin-right: 8px;" OnClick="btnFault_Click">
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="btCancel2" runat="server" Text="Cancel" Width="50px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                <ClientSideEvents Click="function(s, e) { pcLogin2.Hide(); }" />
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
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


    <dx:ASPxPopupControl ID="ValidasiBackStation" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="ValidasiBackStation"
        HeaderText="AuthKey" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbLogin2.Focus(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel4" runat="server" DefaultButton="btnToStation">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
                                        </div>
                                    </td>
                                    <td class="pcmCellCaption">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Width="100px" Text="AuthKey : " Style="float: left; margin-right: 8px;" AssociatedControlID="tbLogin">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="pcmCellText">
                                        <dx:ASPxTextBox ID="TxAuthKeyBackStation" runat="server" Password="true" Width="150px" ClientInstanceName="tbLogin2">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="btnToStation" runat="server" Text="OK" Width="70px" AutoPostBack="False" OnClick="BtnSendToStation">
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel" Width="50px" AutoPostBack="False">
                                                <ClientSideEvents Click="function(s, e) { ValidasiBackStation.Hide(); }" />
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
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

    <dx:ASPxPopupControl ID="PopUpAttachMent" runat="server"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpAttachMent"
        HeaderText="Attachment Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" OnWindowCallback="PopUpAttachMent_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout4" runat="server" Width="450px">
                    <Items>
                        <dx:LayoutItem Caption="Document File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxUploadControl ID="UploadAttachment" runat="server" Width="300px" ClientInstanceName="UploadControl"
                                        NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="True" ShowProgressPanel="True"
                                        OnFileUploadComplete="UploadAttachment_FileUploadComplete" ClientSideEvents-FilesUploadComplete="CloseUploadControl">
                                        <ClientSideEvents FilesUploadComplete="CloseUploadControl"></ClientSideEvents>

                                        <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                        <%-- <ClientSideEvents FileUploadStart="function(s, e) { DXUploadedFilesContainer.Clear(); }"
                                            FileUploadComplete="onFileUploadComplete" />--%>
                                    </dx:ASPxUploadControl>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                    </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="pcInteruptFault" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None" OnDataBinding="pcInteruptFault_DataBinding"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFault" Width="800px"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcInteruptFault_WindowCallback">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbRemark.Focus(); }" />
        <ContentCollection>

            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout5" runat="server" Width="100%" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="Date">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="currentdate" runat="server" AutoPostBack="false" Width="100%"
                                        EditFormatString="yyyy-MM-dd" DisplayFormatString="yyyy-MM-dd">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CS">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="CSTypeValue" runat="server" ClientInstanceName="CSTypeValue" Width="100%" NullText="-Select-">
                                        <Items>
                                            <%--  <dx:ListEditItem Text="VPC" Value="0" />--%>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <%--            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />--%>
                                            <%--<dx:ListEditItem Text="5" Value="5" />--%>
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="VIN/FIN">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFINNumber" ClientInstanceName="cbFINNumber" runat="server" ReadOnly="true"
                                        AutoPostBack="false" Width="100%" DataSourceID="sdsFINNumber" ValueField="Id" ValueType="System.Int32"
                                        IncrementalFilteringMode="Contains" TextFormatString="{0} / {1}">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetLine(); GetCgis();}" />
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="VINNumber" Width="200px" />
                                            <dx:ListBoxColumn FieldName="FINNumber" Width="200px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="NCP">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbNCP" runat="server" AutoPostBack="false" Width="100%"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbLine" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="tbLine">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Quality Gate">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbQG" runat="server" AutosdsQualityGatePostBack="false" Width="100%"
                                        DataSourceID="sdsQualityGate"
                                        TextField="StationName" ValueField="Id" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Station Name">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbStation" ClientInstanceName="cbStation" runat="server"
                                        AutoPostBack="false" Width="100%" DataSourceID="sdsStation"
                                        TextField="StationName" ValueField="Id" ValueType="System.Int32"
                                        IncrementalFilteringMode="Contains">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetSection(); GetCgis();}" />
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Assembly Section">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbAssemblySection" Width="100%" ClientInstanceName="tbAssemblySection" runat="server"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Inspector">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbInspector" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsUser"
                                        TextField="UserName" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Part / Process">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbPartProces" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsFaultPartProcess"
                                        ValueField="FaultPartProcessId" ValueType="System.Int32" TextFormatString="{0} ({1})">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Description" Width="300px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CGIS No.">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbCGIS" ClientInstanceName="cbCGIS" runat="server" AutoPostBack="false" Width="100%"
                                        ValueField="Id" ValueType="System.Int32" TextFormatString="{0} : {1} ({2}) - {3}- {4}" NullText="NoData"
                                        OnCallback="cbCGIS_Callback">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="ProcessName" Width="250px" />
                                            <dx:ListBoxColumn FieldName="CgisNo" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Rf" Width="35px" />
                                            <dx:ListBoxColumn FieldName="PartNumber" Width="110px" />
                                            <dx:ListBoxColumn FieldName="PartDescription" Width="300px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Description">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultDesc" ClientInstanceName="cbFaultDesc" runat="server" Width="100%"
                                        AutoPostBack="false" DataSourceID="sdsFaultDescription" IncrementalFilteringMode="Contains"
                                        ValueField="FaultDescriptionId" ValueType="System.Int32" TextFormatString="{0} - {1}" DropDownStyle="DropDown">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Description" Width="120px" />
                                        </Columns>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetClassification()}" />
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Classification" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbClassification" ClientInstanceName="tbClassification" Width="100%" runat="server" Visible="false" NullText="NoData"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Prio">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbPriority" runat="server" AutoPostBack="false" Width="100%">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Related">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultRelated" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsFaultRelatedType"
                                        ValueField="FaultRelatedTypeId" ValueType="System.Int32" TextFormatString="{0} - {1}">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Description" Width="100px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Send Rectification">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbRecti" runat="server" AutoPostBack="false" Width="100%">
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0" />
                                            <dx:ListEditItem Text="Yes" Value="1" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Stamp">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">

                                    <dx:ASPxComboBox ID="cbStamp" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsStampNumber"
                                        TextField="StampNumber" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Document File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="ShowAttachment" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Remark">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxMemo ID="tbRemark" runat="server" ClientInstanceName="tbRemark" Width="100%" Height="50px"></dx:ASPxMemo>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Status">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultStatus" runat="server" AutoPostBack="false" Width="170px">
                                        <Items>
                                            <dx:ListEditItem Text="Close" Value="1" />
                                            <dx:ListEditItem Text="Open" Value="0" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <%--<dx:LayoutItem Caption="Document File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="ShowAttachment" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                    </Items>

                </dx:ASPxFormLayout>

                <dx:ASPxFormLayout ID="ASPxFormLayout6" runat="server">
                    <Items>
                        <dx:LayoutItem Caption="Attachment File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxGridView runat="server" ViewStateMode="Enabled" ID="GVDataAttachment" ClientInstanceName="GVDataAttachment" KeyFieldName="Id" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileName" VisibleIndex="1">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileLocation" VisibleIndex="2" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-Text="Download" PropertiesHyperLinkEdit-Target="_blank" Caption="Action" VisibleIndex="3">
                                                <PropertiesHyperLinkEdit Text="Download"></PropertiesHyperLinkEdit>
                                            </dx:GridViewDataHyperLinkColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="4">
                                                <DataItemTemplate>
                                                    <dx:ASPxButton AutoPostBack="false" ID="IdBtndelete" runat="server" RenderMode="Link" Text="Delete" OnClick="IdBtndelete_Click">
                                                    </dx:ASPxButton>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowGroupPanel="True" VerticalScrollBarMode="Visible" VerticalScrollableHeight="100" VerticalScrollBarStyle="Standard" />

                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
                    <dx:ASPxButton ID="ASPxButton12" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="btnRecordNew_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="ASPxButton13" runat="server" Text="Reset" AutoPostBack="false">
                        <ClientSideEvents Click="function(s, e) {Reset()}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="ASPxButton14" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="btnSaveClose_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="ASPxButton15" runat="server" Text="Close" AutoPostBack="false" OnClick="btnClose_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="FormConfirmDelete" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="FormConfirmDelete"
        HeaderText="Are You Sure Delete This Fault ?" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
        <%--  <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); AuthProblem.Focus(); }" />--%>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="FormDelete" runat="server" DefaultButton="BtnDelete">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table>
                                <tr style="padding-top: 10px;">
                                    <div class="pcmButton">
                                        <dx:ASPxButton ID="ASPxButton11" runat="server" Text="Yes" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px;" OnClick="Btn_Delete_Click">
                                        </dx:ASPxButton>
                                    </div>
                                </tr>
                                <tr style="padding-top: 10px;">
                                    <div class="pcmButton">
                                        <dx:ASPxButton ID="ASPxButton18" runat="server" Text="No" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                            <ClientSideEvents Click="function(s, e) { FormConfirmDelete.Hide(); }" />
                                        </dx:ASPxButton>


                                    </div>
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
                                    <td class="pcmCellText">
                                        <dx:ASPxTextBox ID="tbLogin" runat="server" Password="true" Width="150px" ClientInstanceName="tbLogin">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="btOK" runat="server" Text="OK" Width="70px" AutoPostBack="False" OnClick="btnNextProcess_Click">
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" Width="50px" AutoPostBack="False">
                                                <ClientSideEvents Click="function(s, e) { pcLogin.Hide(); }" />
                                            </dx:ASPxButton>
                                            <input type="hidden" id="hdntxt" name="hdntxt" value="">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
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

    <dx:ASPxPopupControl ID="TrimPopUp" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="TrimPopUp"
        HeaderText="AuthKey" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbTrim.Focus(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="BtnTrim">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
                                        </div>
                                    </td>
                                    <td class="pcmCellCaption">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Width="100px" Text="AuthKey : " AssociatedControlID="tbTrim">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="pcmCellText">
                                        <dx:ASPxTextBox ID="tbTrim" runat="server" Password="true" Width="150px" ClientInstanceName="tbTrim">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="BtnTrim" runat="server" Text="OK" Width="70px" AutoPostBack="False" OnClick="BtnEndOffLine_Click">
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="BtnCancelTrim" runat="server" Text="Cancel" Width="50px" AutoPostBack="False">
                                                <ClientSideEvents Click="function(s, e) { TrimPopUp.Hide(); }" />
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
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

    <dx:ASPxPopupControl ID="MechValidasi" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="MechValidasi"
        HeaderText="AuthKey" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbMechValidasi.Focus(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
                                        </div>
                                    </td>
                                    <td class="pcmCellCaption">
                                        <dx:ASPxLabel ID="tbMech" runat="server" Width="100px" Text="AuthKey : " AssociatedControlID="tbMechValidasi">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td class="pcmCellText">
                                        <dx:ASPxTextBox ID="tbMechText" runat="server" Password="true" Width="150px" ClientInstanceName="tbMechValidasi">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="BtnValidasiMech" runat="server" Text="OK" Width="70px" AutoPostBack="False" OnClick="BtnValidasiMech_Click">
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="BtnCancelMech" runat="server" Text="Cancel" Width="50px" AutoPostBack="False">
                                                <ClientSideEvents Click="function(s, e) { tbMechValidasi.Hide(); }" />
                                            </dx:ASPxButton>
                                            <%--<input type="hidden" id="hdntxt" name="hdntxt" value="">--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <div class="pcmSideSpacer">
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

    <dx:ASPxPopupControl ID="PopUpEndOffLine" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpEndOffLine"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcInteruptFault_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="450px">
                    <Items>
                        <dx:LayoutItem Caption="Select Station">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="IdcmbEndOffLine" runat="server" AutoPostBack="false" Width="170px"
                                        DataSourceID="DataCmbEndOfflIne" NullText="-Select-"
                                        ValueField="Id" ValueType="System.Int32" TextField="StationName">
                                    </dx:ASPxComboBox>
                                    <dx:ASPxButton ID="SubmitButton" runat="server" AutoPostBack="false" Width="180px" Visible="true" Height="40px"
                                        Text="Send To Trim" Font-Bold="true" Font-Size="13px" OnClick="btnSendToTrim_Click">
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnSendMech" runat="server" AutoPostBack="false" Width="180px" Visible="false" Height="40px"
                                        Text="Send To Mechanical" Font-Bold="true" Font-Size="13px" OnClick="btnSendMech_Click">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="PopUpSendToMech" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpSendToMech"
        HeaderText="Send To Mech" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="PopUpSendToMech_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Width="450px">
                    <Items>
                        <dx:LayoutItem Caption="Select Station">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmb_MechStation" runat="server" AutoPostBack="false" Width="170px"
                                        DataSourceID="DataCmbMech" NullText="-Select-"
                                        ValueField="Id" ValueType="System.Int32" TextField="StationName">
                                    </dx:ASPxComboBox>

                                    <dx:ASPxButton ID="btnPopSendMech" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Send To Mechanical" Font-Bold="true" Font-Size="13px" OnClick="btnSendMech_Click">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:SqlDataSource ID="sdsQualityGate" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from FaultQualityGate"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DataCmbEndOfflIne" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select a.*,b.AssemblySectionName from stations a inner join AssemblySection b on a.AssemblySectionId = b.Id where b.AssemblySectionName = 'Trimming Line'"></asp:SqlDataSource>

    <asp:SqlDataSource ID="DataCmbMech" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select a.Id,a.StationName,b.AssemblySectionName from stations a inner join AssemblySection b on a.AssemblySectionId = b.Id where b.IsMechLine=1"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsStation" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from Stations"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFINNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from ProductionHistories"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFaultPartProcess" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select FaultPartProcessId, Code, Description from FaultPartProcess"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFaultDescription" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select FaultDescriptionId, Code, Description from FaultDescription"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFaultRelatedType" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select FaultRelatedTypeId, Code, Description from FaultRelatedType"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsAssemblySection" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, AssemblySectionName from AssemblySection"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUser" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id as UserId, FirstName + ' ' + LastName AS UserName from Users Where OrganizationId = 3"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsCgis" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
        SELECT cpp.Id, cpp.ProcessNo, cpp.ProcessName, cpp.Rf, cpp.CgisNo, cpp.PartNumber, cpp.PartDescription
		FROM ControlPlanProcesses cpp
	    WHERE cpp.StationId = @StationId AND cpp.ControlPlanId = (
		    SELECT cp.Id FROM ControlPlans cp join ( 
			    SELECT vod.PackingMonth, vod.ModelId, vod.VariantId FROM VehicleOrderDetails vod
			    WHERE vod.VehicleNumber = (
				    SELECT ph.VINNumber FROM ProductionHistories ph
				    WHERE ph.Id = @HistoryId
				)
		    ) q
		ON cp.PackingMonth = q.PackingMonth AND cp.ModelId = q.ModelId AND cp.VariantId = q.VariantId)">
        <SelectParameters>
            <asp:SessionParameter Name="StationId" SessionField="StationId" Type="Int32" />
            <asp:SessionParameter Name="HistoryId" SessionField="HistoryId" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsStampNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[UserId],[StampNumber] FROM [dbo].[EmployeeStampNumber]"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsProblem" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[Name]FROM [dbo].[AndonProductionProblem] where [IsActive] = 1"></asp:SqlDataSource>
</asp:Content>
