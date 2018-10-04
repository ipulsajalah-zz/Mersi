<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductionVoca.aspx.cs" Inherits="DotMercy.custom.Production.ProductionVoca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Production VOCA
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
            pcInteruptFault.Hide();
            pcInteruptFault.Show();
        }
        function onFileUploadComplete(s, e) {
            if (e.callbackData) {
                var fileData = e.callbackData.split('|');
                var fileName = fileData[0],
                    fileUrl = fileData[1],
                    fileSize = fileData[2];
                //DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
            }
        }
        function GetLine() {
            HiddenText.Set("CSType", CSTypeValue.GetValue());
            HiddenText.Set("FINNumber", cbFINNumber.GetText());
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
            else if (parm1 == 'Voca') {
                pcLogin.Show();
            }

        }
    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <dx:ASPxHiddenField runat="server" ID="HiddenText" ClientInstanceName="HiddenText"></dx:ASPxHiddenField>
    <dx:ASPxPanel ID="PanelHeader" runat="server">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>
                <table style="width: 100%; margin-bottom: 10px" id="table-header">
                    <tr>
                        <td colspan="10" style="text-align: center; padding-bottom: 10px">
                            <dx:ASPxLabel ID="lblStation" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr class="headerMenu" style="border: solid 1px">
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
                                <div style="display: inline-block; padding: 5px">
                                    <dx:ASPxButton ID="BtnReleaseFinal" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Send to Final Station/Gate" Font-Bold="true" Font-Size="13px" OnClick="BtnReleaseFinal_Click">
                                    </dx:ASPxButton>
                                </div>
                                <div style="display: inline-block; padding: 5px">
                                    <dx:ASPxButton ID="AddVocaPa" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Add Fault" Font-Bold="true" Font-Size="13px" OnClick="AddVocaPa_Click">
                                    </dx:ASPxButton>
                                </div>
                                <div style="display: inline-block; padding: 5px">
                                    <dx:ASPxButton ID="BtnVoca"  runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Send To Voca" Font-Bold="true" Font-Size="13px" Visible="false">
                                        <ClientSideEvents Click="function(s, e) { ShowLoginWindow('Voca'); }" />
                                    </dx:ASPxButton>
                                </div>
                                <div style="display: inline-block; padding: 5px">
                                    <dx:ASPxButton ID="btnFault" runat="server" AutoPostBack="false" Width="180px" Height="40px"
                                        Text="Add Fault Record" Visible="false" Font-Bold="true" Font-Size="13px" OnClick="btnFault_Click">
                                        <%--    <ClientSideEvents Click="function(s, e) { ShowLoginWindow('Fault'); }" />--%>
                                    </dx:ASPxButton>
                                </div>


                            </div>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>

    <dx:ASPxPopupControl ID="pcInteruptFaultx" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFaultx"
        HeaderText="Fault Form Edit" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Width="450px" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="CS Type">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="CSTypeValuex" runat="server" AutoPostBack="false" NullText="-Select-" Enabled="false">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Date">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="currentdatex" runat="server" AutoPostBack="false"
                                        EditFormatString="yyyy-MM-dd" DisplayFormatString="yyyy-MM-dd">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="NCP">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbNCPx" runat="server" NullText="NoData" AutoPostBack="false" Width="170px"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Chassis Number">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFINNumberx" ClientInstanceName="cbFINNumberx" runat="server"
                                        AutoPostBack="false" Width="170px"
                                        IncrementalFilteringMode="Contains" TextFormatString="{0} / {1}">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbLinex" NullText="NoData" runat="server" ClientEnabled="false" ClientInstanceName="tbLinex">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Quality Gate">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbQGx" runat="server" AutoPostBack="false" Width="170px">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Station Name">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbStationx" ClientInstanceName="cbStationx" runat="server"
                                        AutoPostBack="false" Width="170px"
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
                                    <dx:ASPxTextBox ID="tbAssemblySectionx" ClientInstanceName="tbAssemblySectionx" runat="server"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Inspector">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbInspectorx" runat="server" AutoPostBack="false" Width="170px"
                                        TextField="UserName" ValueField="UserId">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Part / Process">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbPartProcesx" runat="server" AutoPostBack="false" Width="170px"
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
                                    <dx:ASPxComboBox ID="cbCGISx" ClientInstanceName="cbCGISx" runat="server" AutoPostBack="false" Width="170px" ReadOnly="true"
                                        ValueField="Id" ValueType="System.Int32" TextFormatString="{0} : {1} ({2}) - {3}- {4}"
                                        OnCallback="cbCGIS_Callback">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Description">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultDescx" ClientInstanceName="cbFaultDescx" runat="server" Width="170px"
                                        AutoPostBack="false" ReadOnly="true" IncrementalFilteringMode="Contains"
                                        ValueField="FaultDescriptionId" ValueType="System.Int32" TextFormatString="{0} - {1}">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Classification">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbClassificationx" ClientInstanceName="tbClassificationx" runat="server"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Pri">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbPriorityx" runat="server" AutoPostBack="false" Width="170px">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Related">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultRelatedx" runat="server" AutoPostBack="false" Width="170px"
                                        ValueType="System.Int32" TextFormatString="{0} - {1}">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Description" Width="100px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Send Rectification" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbRectix" runat="server" AutoPostBack="false" Width="70px">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Stamp">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbStampx" runat="server" AutoPostBack="false" Width="170px"
                                        TextField="StampNumber" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <%--<dx:LayoutItem Caption="Document File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton Enabled="false" ID="ShowAttachment" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Remark">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbRemarkx" runat="server" AutoPostBack="false" Width="170px"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Fault Status">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="FaultStatusx" runat="server" AutoPostBack="false" Width="170px">
                                        <Items>
                                            <dx:ListEditItem Text="Close" Value="1" />
                                            <dx:ListEditItem Text="On Progress" Value="0" />
                                        </Items>
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
                    </Items>

                </dx:ASPxFormLayout>

                <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server">
                    <Items>
                        <dx:LayoutItem Caption="Attachment File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxGridView runat="server" ViewStateMode="Enabled" ID="GVDataAttachmentx" ClientInstanceName="GVDataAttachment" KeyFieldName="Id" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileName" VisibleIndex="1">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileLocation" VisibleIndex="2" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-Text="Download" Caption="Action" VisibleIndex="3">
                                                <PropertiesHyperLinkEdit Text="Download"></PropertiesHyperLinkEdit>
                                            </dx:GridViewDataHyperLinkColumn>
                                        </Columns>
                                        <Settings ShowGroupPanel="True" VerticalScrollBarMode="Visible" VerticalScrollableHeight="100" VerticalScrollBarStyle="Standard" />

                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
                    <%-- <dx:ASPxButton ID="btnRecordNew" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="btnRecordNew_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnReset" runat="server" Text="Reset" AutoPostBack="false">
                        <ClientSideEvents Click="function(s, e) {Reset()}" />
                    </dx:ASPxButton>--%>
                    <dx:ASPxButton ID="btnSaveCloseEdit" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="btnSaveCloseEdit_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnClosex" runat="server" Text="Close" AutoPostBack="false" OnClick="btnClosex_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>

        </ContentCollection>
    </dx:ASPxPopupControl>
    
    <dx:ASPxPageControl ID="tabDetail" runat="server" ActiveTabIndex="0" EnableHierarchyRecreation="True" Width="100%" Height="400px">
        <TabPages>
            <dx:TabPage Text="Fault Records" Enabled="false" Visible="false" Name="tabFaults">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxGridView ID="gridFault" runat="server" KeyFieldName="FaultRecordId" Width="100%"
                            OnDataBinding="gridFault_DataBinding">
                            <Columns>
                                <%--<dx:GridViewCommandColumn ShowEditButton="true" VisibleIndex="0" FixedStyle="Left" Width="40px" />--%>
                                <dx:GridViewDataTextColumn VisibleIndex="0">
                                    <DataItemTemplate>
                                        <dx:ASPxButton ID="Btn_Edit" runat="server" RenderMode="Link" Text="Edit" OnClick="Btn_Edit_Click">
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
            <dx:TabPage Text="Fault Records VoCA/PA" Name="tabFaults">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <dx:ASPxGridView ID="GvDetailsFaultRecord" OnDataBinding="GvDetailsFaultRecord_DataBinding"
                            runat="server" KeyFieldName="Id" AutoGenerateColumns="False" OnDetailRowExpandedChanged="GvMainFaultRecord_DetailRowExpandedChanged"
                            ClientInstanceName="GvDetailsFaultRecord" Width="100%">
                            <Columns>
                                <dx:GridViewDataTextColumn VisibleIndex="1" Caption="View Report">
                                    <DataItemTemplate>
                                        <dx:ASPxButton AutoPostBack="false" Text="Report" RenderMode="Link" ID="EditHeader" runat="server" OnClick="btnReport_Click" ClientInstanceName="EditDetails">
                                        </dx:ASPxButton>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Id" Caption="No" VisibleIndex="2" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ModelName" VisibleIndex="3" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="cs" VisibleIndex="4" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AuditDate" VisibleIndex="5" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="VINNumber" VisibleIndex="6" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="FINNumber" VisibleIndex="7" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Plant" VisibleIndex="8" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Mileage" VisibleIndex="9" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="User1" Caption="Auditor 1" VisibleIndex="10" Visible="true">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="User2" Caption="Auditor 2" VisibleIndex="11" Visible="true">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowGroupPanel="True" />
                            <SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
                            <SettingsSearchPanel Visible="True" />
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
                            <Templates>
                                <DetailRow>

                                    <dx:ASPxGridView ID="GvDetailsFaultRecord"
                                        runat="server" KeyFieldName="Id" AutoGenerateColumns="False"
                                        ClientInstanceName="GvDetailsFaultRecord" Width="100%">
                                        <ClientSideEvents DetailRowCollapsing="CollapsingDetailRow" />
                                        <Columns>


                                            <dx:GridViewDataTextColumn FieldName="Id" Caption="No" VisibleIndex="1" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="Responsible" Caption="Responsible" VisibleIndex="2" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="PartProcess" Caption="Part Process" VisibleIndex="3" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="faultDescription" Caption="Fault Description" VisibleIndex="4" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="faultArea" Caption="Fault Area" VisibleIndex="5" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="StationName" Caption="Station Name" VisibleIndex="6" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataCheckColumn FieldName="Analysis" VisibleIndex="7" Visible="true">
                                            </dx:GridViewDataCheckColumn>
                                            <dx:GridViewDataCheckColumn FieldName="Rework" VisibleIndex="7" Visible="true">
                                            </dx:GridViewDataCheckColumn>
                                            <dx:GridViewDataTextColumn FieldName="Prio" VisibleIndex="6" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="CreateBy" VisibleIndex="8" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="CreatedDate" VisibleIndex="9" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>

                                    </dx:ASPxGridView>
                                </DetailRow>

                            </Templates>
                        </dx:ASPxGridView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>

    <dx:ASPxPopupControl ID="pcInteruptFault" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFault" Width="800px" OnDataBinding="pcInteruptFault_DataBinding"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcInteruptFault_WindowCallback">
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
                                    <dx:ASPxComboBox ID="CSTypeValue" runat="server" ClientInstanceName="CSTypeValue" Width="100%" NullText="-Select-" Enabled="false">
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
                                        ValueField="FaultDescriptionId" ValueType="System.Int32" TextFormatString="{0} - {1}">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Description" Width="120px" />
                                        </Columns>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetClassification()}" />
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Classification">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbClassification" ClientInstanceName="tbClassification" Width="100%" runat="server" NullText="NoData"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Pri">
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
                        <dx:LayoutItem Caption="Send Rectification" Visible="false">
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
                                    <dx:ASPxButton ID="ASPxButton1" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Remark">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbRemark" runat="server" AutoPostBack="false" Width="100%"></dx:ASPxTextBox>
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
                                            <dx:GridViewDataColumn FieldName="Url" VisibleIndex="2" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataHyperLinkColumn FieldName="Url" PropertiesHyperLinkEdit-Text="Download" Caption="Action" VisibleIndex="3">
                                                <PropertiesHyperLinkEdit Text="Download"></PropertiesHyperLinkEdit>
                                            </dx:GridViewDataHyperLinkColumn>
                                        </Columns>
                                        <Settings ShowGroupPanel="True" VerticalScrollBarMode="Visible" VerticalScrollableHeight="100" VerticalScrollBarStyle="Standard" />

                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="btnRecordNew_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Reset" AutoPostBack="false">
                        <ClientSideEvents Click="function(s, e) {Reset()}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="btnSaveClose_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="Close" AutoPostBack="false" OnClick="btnClose_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
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
                                        OnFileUploadComplete="uplDocumentWI_FileUploadComplete" ClientSideEvents-FilesUploadComplete="CloseUploadControl">
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
                                            <dx:ASPxButton ID="btOK" runat="server" Text="OK" Width="70px" AutoPostBack="False" Style="float: left; margin-right: 8px;" OnClick="BtnVoca_Click">
                                            </dx:ASPxButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="pcmButton">
                                            <span>&nbsp</span>
                                            <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" Width="50px" AutoPostBack="False" Style="float: left; margin-right: 8px">
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
    </dx:ASPxPopupControl>

    <asp:SqlDataSource ID="sdsQualityGate" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from FaultQualityGate"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DataCmbEndOfflIne" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select a.*,b.AssemblySectionName from stations a inner join AssemblySection b on a.AssemblySectionId = b.Id where b.AssemblySectionName = 'End of Line'"></asp:SqlDataSource>

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
</asp:Content>
