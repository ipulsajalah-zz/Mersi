<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="FaultAuditRecord.aspx.cs" Inherits="DotMercy.custom.Production.FaultAuditRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Fault Audit Record
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
        function GetType() {
            var s = cbFINNumber.GetText().split('/')[1].trim();
            PageMethods.GetType(s, ONgetType);
        }
        function GetColor() {
            var s = cbFINNumber.GetText().split('/')[1].trim();
            PageMethods.GetColor(s, OnGetColor);
        }
        function GetBauMaster() {
            var s = cbFINNumber.GetText().split('/')[1].trim();
            PageMethods.GetBauMaster(s, OnGetBaumaster);
        }
        function OnGetBaumaster(response, userContext, methodName) {
            TxBaumaster.SetText(response)
        }
        function ONgetType(response, userContext, methodName) {
            TxType.SetText(response)
        }
        function OnGetColor(response, userContext, methodName) {
            TxColor.SetText(response)
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
        <dx:ASPxHiddenField runat="server" ID="HiddenText" ClientInstanceName="HiddenText"></dx:ASPxHiddenField>
   <dx:ASPxFormLayout ID="Layout" runat="server" Width="100%" ColCount="2">
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
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
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
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetLine(); GetType(); GetColor(); GetBauMaster();}" />
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="VINNumber" Width="200px" />
                                            <dx:ListBoxColumn FieldName="FINNumber" Width="200px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         
                       <%-- <dx:LayoutItem Caption="NCP">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbNCP" runat="server" AutoPostBack="false" Width="100%"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbLine" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="tbLine">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Type">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TxType" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="TxType">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Color">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TxColor" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="TxColor">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Baumaster">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TxBaumaster" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="TxBaumaster">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Mileage">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TxMileage" runat="server" Width="100%" ClientInstanceName="TxMileage">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Plant">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TxPlant" runat="server" Width="100%" ClientInstanceName="TxPlant">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Auditor1">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                     <dx:ASPxComboBox ID="Auditor1" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsUser"
                                        TextField="UserName" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Auditor2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                     <dx:ASPxComboBox ID="Auditor2" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsUser"
                                        TextField="UserName" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Material">
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
                        <%--<dx:LayoutItem Caption="Quality Gate">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbQG" runat="server" AutosdsQualityGatePostBack="false" Width="100%"
                                        DataSourceID="sdsQualityGate"
                                        TextField="StationName" ValueField="Id" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Station Name">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbStation" ClientInstanceName="cbStation" runat="server"
                                        AutoPostBack="false" Width="100%" DataSourceID="sdsStation"
                                        TextField="StationName" ValueField="Id" ValueType="System.Int32"
                                        IncrementalFilteringMode="Contains">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetSection();}" />
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
                      <%--  <dx:LayoutItem Caption="Inspector">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbInspector" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="sdsUser"
                                        TextField="UserName" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                       
                      <%--  <dx:LayoutItem Caption="CGIS No.">
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
                        </dx:LayoutItem>--%>
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
                        <dx:LayoutItem Caption="Fault Classification" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbClassification" ClientInstanceName="tbClassification" Width="100%" runat="server"     NullText="NoData"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                         <dx:LayoutItem Caption="Area">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmb_Area" ClientInstanceName="cmb_Area" runat="server"
                                        AutoPostBack="false" Width="100%" DataSourceID="sdsStation"
                                        TextField="StationName" ValueField="Id" ValueType="System.Int32"
                                        IncrementalFilteringMode="Contains">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetSection();}" />
                                    </dx:ASPxComboBox>
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
                                             <dx:ListEditItem Text="5" Value="5" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Related" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultRelated" runat="server" AutoPostBack="false" Width="100%" Visible="false"
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
                         <dx:LayoutItem Caption="Reponsible Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbReponsibleLine" runat="server" AutoPostBack="false" Width="100%" 
                                        DataSourceID="dataReponsible"
                                        ValueField="Id" ValueType="System.Int32" TextFormatString="{0} - {1}">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Id" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Description" Width="100px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                       <dx:LayoutItem Caption="Analysis">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txAnalysis" runat="server" Width="100%" ClientInstanceName="txAnalysis">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Rework">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TxRework" runat="server" Width="100%" ClientInstanceName="TxRework">
                                    </dx:ASPxTextBox>
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
                       <%-- <dx:LayoutItem Caption="Remark">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbRemark" runat="server" AutoPostBack="false" Width="100%"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>

                        <%--<dx:LayoutItem Caption="Document File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="ShowAttachment" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                    </Items>

                </dx:ASPxFormLayout>

                <dx:ASPxFormLayout ID="Attachment" runat="server">
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
                    <dx:ASPxButton ID="btnRecordNew" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="btnRecordNew_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="BtnReset" runat="server" Text="Reset" AutoPostBack="false">
                        <ClientSideEvents Click="function(s, e) {Reset()}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnSaveClose" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="btnSaveClose_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnClose" runat="server" Text="Close" AutoPostBack="false" OnClick="btnClose_Click">
                    </dx:ASPxButton>
                </div>

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

    <asp:SqlDataSource ID="sdsQualityGate" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from FaultQualityGate"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsStation" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StationName from Stations"></asp:SqlDataSource>

     <asp:SqlDataSource ID="dataReponsible" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Description from faultauditresponsible"></asp:SqlDataSource>


    <asp:SqlDataSource ID="sdsFINNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from ProductionHistories"></asp:SqlDataSource>

     <asp:SqlDataSource ID="sdsFINNumberVPC" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from VPCStorage where FINNumber is not null"></asp:SqlDataSource>


    <asp:SqlDataSource ID="sdsFaultPartProcess" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select FaultPartProcessId, Code, Description from FaultPartProcess"></asp:SqlDataSource>

     <asp:SqlDataSource ID="DataArea" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id,Code, Description from FaultArea"></asp:SqlDataSource>

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
