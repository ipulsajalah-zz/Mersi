<%@ Page Title="Production Release Fault" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductionReleaseFault.aspx.cs" Inherits="DotMercy.custom.Production.ProductionReleaseFault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Production Release Fault
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
        function onFileUploadComplete(s, e) {
            if (e.callbackData) {
                var fileData = e.callbackData.split('|');
                var fileName = fileData[0],
                    fileUrl = fileData[1],
                    fileSize = fileData[2];
                //DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
            }
        }
        function GetCSNumber() {
            var y = CSTypeValue.GetText();
            PageMethods.GetCSNumber(y, OnGetLine);
        }
        function GetLine() {
            var s = cbFINNumber.GetValue();
            HiddenText.Set("CSType", CSTypeValue.GetValue());
            HiddenText.Set("FINNumber", cbFINNumber.GetText());
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
    </script>
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
     <h2>Production Release Fault</h2>
    <dx:ASPxHiddenField runat="server" ID="HiddenText" ClientInstanceName="HiddenText"></dx:ASPxHiddenField>
    <dx:ASPxButton runat="server" ID="btnFault" OnClick="btnFault_Click" Text="New"></dx:ASPxButton>
 
    <dx:ASPxGridView ID="gridFault" runat="server" KeyFieldName="FaultRecordId" Width="100%" OnHtmlRowPrepared="gridFault_HtmlRowPrepared" OnCustomButtonInitialize="gridFault_CustomButtonInitialize"
        OnRowUpdating="gridFault_RowUpdating" OnDataBinding="gridFault_DataBinding">
        <Columns>
            <%--<dx:GridViewCommandColumn ShowEditButton="true" VisibleIndex="0" FixedStyle="Left" Width="40px" />--%>
            <dx:GridViewDataTextColumn VisibleIndex="0">
                <DataItemTemplate>
                    <dx:ASPxButton ID="Btn_Edit" runat="server" RenderMode="Link" Text="Edit" OnClick="Btn_Edit_Click">
                    </dx:ASPxButton>
                     <dx:ASPxButton ID="Btn_Delete" runat="server" RenderMode="Link" Text="Delete" OnClick="Btn_Delete_Click">
                    </dx:ASPxButton>
                </DataItemTemplate>

            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn VisibleIndex="1" Visible="false">
                <DataItemTemplate>
                    <dx:ASPxButton ID="Btn_ShowAttach" runat="server" RenderMode="Link" Text="View Attachment" OnClick="Btn_ShowAttach_Click">
                    </dx:ASPxButton>
                </DataItemTemplate>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultRecordId" VisibleIndex="2" Visible="false" ReadOnly="true">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProductionHistoryId" VisibleIndex="3" Visible="false" ReadOnly="true">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="FaultStatus" VisibleIndex="4" Width="80px">
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn FieldName="FINNumber" Caption="FIN Number" VisibleIndex="5" Width="135px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CSType" VisibleIndex="6" Width="55px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultNumber" VisibleIndex="7" Width="90px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProductionLineId" VisibleIndex="8" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProductionLineName" Caption="Line Name" VisibleIndex="9" Width="70px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssemblySectionId" VisibleIndex="10" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssemblySectionName" Caption="Section Name" VisibleIndex="11" Width="90px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StationId" VisibleIndex="12" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StationName" VisibleIndex="13" Width="100px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="QualityGateId" VisibleIndex="14" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="QualityGateName" Caption="Quality Gate" VisibleIndex="15" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultPartProcessId" VisibleIndex="16" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultPartProcessDesc" Caption="Fault Part Process" VisibleIndex="17" Width="170px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultRelatedTypeId" VisibleIndex="18" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultRelatedTypeDesc" Caption="Fault Related Type" VisibleIndex="19" Width="120px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultDescriptionId" VisibleIndex="20" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaultDescriptionText" Caption="Fault Description" VisibleIndex="21" Width="170px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CGISId" VisibleIndex="22" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CGISNo" VisibleIndex="23" Width="120px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="RF" VisibleIndex="24" Width="25px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="25" Width="70px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="NCP" VisibleIndex="26" Width="95px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="RecordDate" VisibleIndex="27" Width="150px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="InspectorUserId" VisibleIndex="28" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="InspectorName" VisibleIndex="29" Width="120px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StamperUserId" VisibleIndex="30" Visible="false" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StamperName" VisibleIndex="31" Width="120px" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Remarks" VisibleIndex="32" Width="120px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IsSentToRectification" VisibleIndex="33" Width="140px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

        </Columns>
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Items="10,20,50" Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <SettingsEditing Mode="EditFormAndDisplayRow" />
        <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" />
        <SettingsSearchPanel Visible="True" />
        <Styles>
            <%--<Cell HorizontalAlign="Center"></Cell>--%>
            <Header HorizontalAlign="Center" />
        </Styles>
    </dx:ASPxGridView>

     <dx:ASPxPopupControl ID="pcInteruptFault" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None" OnDataBinding="pcInteruptFault_DataBinding"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFault" Width="800px"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcInteruptFault_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout4" runat="server" Width="100%" ColCount="2">
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
                                    <dx:ASPxComboBox ID="cbFINNumber" ClientInstanceName="cbFINNumber" runat="server"
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
                                        ValueField="FaultDescriptionId" ValueType="System.Int32" TextFormatString="{0} - {1}" DropDownStyle = "DropDown">
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
                        <dx:LayoutItem Caption="Fault Status">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="faultStatus" runat="server" AutoPostBack="false" Width="170px">
                                       <Items>
                                            <dx:ListEditItem Text="Close" Value="1" />
                                            <dx:ListEditItem Text="On Progress" Value="0" />
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

                <dx:ASPxFormLayout ID="ASPxFormLayout5" runat="server">
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
                                            <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-Target="_blank" PropertiesHyperLinkEdit-Text="Download" Caption="Action" VisibleIndex="3">
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

    <dx:ASPxPopupControl ID="pcInteruptFaultx" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None" OnDataBinding="pcInteruptFault_DataBinding"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFaultx"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcInteruptFault_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="layoutFault" runat="server" Width="450px" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="CS Type">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="CSTypeValuex" runat="server" AutoPostBack="false" NullText="-Select-">
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
                                    <dx:ASPxComboBox ID="cbFaultDescx" ClientInstanceName="cbFaultDescx" runat="server" Width="100%"
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
                                    <dx:ASPxComboBox ID="cbFaultRelatedx" runat="server" AutoPostBack="false" Width="100%"
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
                        <%--<dx:LayoutItem Caption="Document File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="ShowAttachment" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
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
                    <dx:ASPxButton ID="btnClosex" runat="server" Text="Close" AutoPostBack="false" OnClick="btnClose_Click">
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
                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="450px">
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


    <dx:ASPxPopupControl ID="PopUpViewDocument" runat="server"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpViewDocument"
        HeaderText="Attachment Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Width="450px">
                    <Items>
                        <dx:LayoutItem Caption="Attachment File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxGridView runat="server" ViewStateMode="Enabled" ID="GvAttachMentView" ClientInstanceName="GVDataAttachment" KeyFieldName="Id">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileName" VisibleIndex="1">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileLocation" VisibleIndex="2">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-Text="Download" VisibleIndex="3">
                                                <PropertiesHyperLinkEdit Text="Download"></PropertiesHyperLinkEdit>
                                            </dx:GridViewDataHyperLinkColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                    </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:SqlDataSource ID="sdsCgis" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
         SELECT cpp.Id, cpp.ProcessNo, cpp.ProcessName, cpp.Rf, cpp.CgisNo, cpp.PartNumber, cpp.PartDescription
		FROM ControlPlanProcesses cpp
	    WHERE cpp.StationId = @StationId AND cpp.ControlPlanId IN (
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

    <asp:SqlDataSource ID="sdsQualityGate" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from FaultQualityGate"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsStation" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from Stations"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFINNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from ProductionHistories"></asp:SqlDataSource>

     <asp:SqlDataSource ID="sdsFINNumberVPC" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from VPCStorage where FINNumber is not null"></asp:SqlDataSource>


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

    <asp:SqlDataSource ID="sdsStampNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[UserId],[StampNumber] FROM [dbo].[EmployeeStampNumber]"></asp:SqlDataSource>

</asp:Content>

