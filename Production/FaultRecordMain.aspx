<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="FaultRecordMain.aspx.cs" Inherits="DotMercy.custom.Production.FaultRecordMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Fault Record Main
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent"   runat="server">
    <style type="text/css">
        .inl {
            display: inline-flex;
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
                filebyte = fileData[3];
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
        function PartProc() {
            var value = PUcbPartProces.GetSelectedIndex();
            pcInteruptFault.PerformCallback('PartDescription$' + value);
        }
        function PartProcDesc() {
            var value = PUcbPartProcesDesc.GetSelectedIndex();
            pcInteruptFault.PerformCallback('PUcbPartProcesDesc$' + value);
        }
        function cbFaultDescx() {
            var value = cbFaultDesc.GetSelectedIndex();
            pcInteruptFault.PerformCallback('cbFaultDesc$' + value);
        }
        function cbFaultDescDescx() {
            var value = cbFaultDescDesc.GetSelectedIndex();
            pcInteruptFault.PerformCallback('cbFaultDescDesc$' + value);
        }
        function GetType() {
            var s = cbFINNumber.GetText().split('/')[1].trim();
            PageMethods.GetType(s, ONgetType);
        }
        function GetFinNumber() {
            var s = CSTypeValue.GetText();
            cbFINNumber.data
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
        function OnGetPlant(response, userContext, methodName) {
            TxPlant.SetText(response)
        }
        function OnGetMileage(response, userContext, methodName) {
            TxMileage.SetText(response)
        }
        function GetSection() {
            var s = cbStation.GetValue();
            PageMethods.GetSection(s, OnGetSection);
        }
        //function GetAuditor1() {
        //    var s = Auditor1.GetValue();
        //    PageMethods.GetAuditor1(s, OnGetAuditor1);
        //}  
        function GetAuditor1() {
            var y = Auditor1.GetText();
        PageMethods.GetAuditor1(y, OnGetLine);
        }
        function GetAuditor2() {
            var s = Auditor2.GetText();
            PageMethods.GetAuditor2(s, OnGetLine);
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
        function CollapsingDetailRow() {
            //  Btn_mark.SetEnabled(true);
        }
        function ExpandingDetailRow() {
            //   Btn_mark.SetEnabled(true);
        }
    </script>
    
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <dx:ASPxHiddenField runat="server" ID="HiddenText" ClientInstanceName="HiddenText"></dx:ASPxHiddenField>
    <br />
     <div style="text-align: center; padding-bottom: 10px">
        <dx:ASPxLabel ID="lblPosition" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
    <div style="text-align: center; padding-bottom: 10px">
        <dx:ASPxLabel ID="lblPosition2" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
    <br />
    <dx:ASPxButton runat="server" ID="btnFault" AutoPostBack="true" OnClick="btnFault_Click" Text="New Record"></dx:ASPxButton>
    <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>
    <br />
    <dx:ASPxLabel runat="server" ID="labelNotif" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    <dx:ASPxGridView ID="GvMainFaultRecord" OnDetailRowExpandedChanged="GvMainFaultRecord_DetailRowExpandedChanged" OnCustomButtonInitialize="GvMainFaultRecord_CustomButtonInitialize"
        runat="server" KeyFieldName="Id" AutoGenerateColumns="False"
        ClientInstanceName="GvMainFaultRecord" Width="100%">
        <ClientSideEvents DetailRowCollapsing="CollapsingDetailRow" DetailRowExpanding="ExpandingDetailRow" />
        <Columns>
            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="Action">
                <DataItemTemplate>
                    <dx:ASPxButton AutoPostBack="true" Text="Edit" RenderMode="Link" ID="EditHeader" runat="server" OnClick="EditHeader_Click" ClientInstanceName="EditDetails">
                    </dx:ASPxButton>
                    <dx:ASPxButton AutoPostBack="false" Text="Delete" RenderMode="Link" ID="DeleteHeader" runat="server" OnClick="DeleteHeader_Click" ClientInstanceName="DeleteHeader">
                    </dx:ASPxButton>
                </DataItemTemplate>

            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn VisibleIndex="1" Caption="View Report">
                <DataItemTemplate>
                     <dx:ASPxButton AutoPostBack="false" Text="Report" RenderMode="Link" ID="ASPxButton1" runat="server" OnClick="btnReport_Click" ClientInstanceName="EditDetails">
                    </dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Id" Caption="No" VisibleIndex="2" Visible="true">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ModelName" VisibleIndex="3" Visible="true">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="cs" VisibleIndex="4" Visible="true" Caption="CS" GroupIndex="0" SortIndex="0" SortOrder="Descending">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AuditDate" VisibleIndex="5" Visible="true" SortOrder="Descending">
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
            <%--   <dx:GridViewDataTextColumn VisibleIndex="10">
                            <DataItemTemplate>
                                <dx:ASPxButton AutoPostBack="false" Text="Report" RenderMode="Link" ID="EditHeader" runat="server" OnClick="btnReport_Click" ClientInstanceName="EditDetails">
                                </dx:ASPxButton>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>--%>
            <%--  <dx:GridViewDataTextColumn VisibleIndex="10">
                            <DataItemTemplate>
                                <dx:ASPxButton AutoPostBack="false" Text="Report" RenderMode="Link" ID="btnReport" runat="server" OnClick="btnReport_Click"">
                                </dx:ASPxButton>
                            </DataItemTemplate>
             </dx:GridViewDataTextColumn>--%>
        </Columns>
        <SettingsPager AlwaysShowPager="True" PageSize="20">
            <PageSizeItemSettings Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <Settings ShowGroupPanel="True" />
        <SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
        <SettingsSearchPanel Visible="True" />
        <Templates>
            <DetailRow>
                <dx:ASPxButton runat="server" ID="btnFaultDetail" OnClick="btnFaultDetail_Click" Text="New Record"></dx:ASPxButton>
                <dx:ASPxGridView ID="GvDetailsFaultRecord" OnDataBinding="GvDetailsFaultRecord_DataBinding" OnCustomButtonInitialize="GvDetailsFaultRecord_CustomButtonInitialize"
                    runat="server" KeyFieldName="Id" AutoGenerateColumns="False"
                    ClientInstanceName="GvDetailsFaultRecord" Width="100%">
                    <ClientSideEvents DetailRowCollapsing="CollapsingDetailRow" DetailRowExpanding="ExpandingDetailRow" />
                    <Columns>
                        <dx:GridViewDataTextColumn VisibleIndex="0">
                            <DataItemTemplate>
                                <dx:ASPxButton AutoPostBack="false" Text="Edit" RenderMode="Link" ID="EditDetails" runat="server" OnClick="EditDetails_Click" ClientInstanceName="EditDetails">
                                </dx:ASPxButton>
                                  <dx:ASPxButton AutoPostBack="false" Text="Delete" RenderMode="Link" ID="DeleteDetails" runat="server" OnClick="DeleteDetails_Click" ClientInstanceName="DeleteHeader">
                    </dx:ASPxButton>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>

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

    <dx:ASPxPopupControl ID="pcInteruptFaultRecord" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None" OnDataBinding="pcInteruptFaultRecord_DataBinding"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFault" Width="800px"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcInteruptFaultRecord_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout4" runat="server" Width="100%" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="Date">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit ID="AuditDates" runat="server" Width="100%"
                                        EditFormatString="yyyy-MM-dd" DisplayFormatString="yyyy-MM-dd">
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="VIN/FIN">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFINNumber" runat="server" ClientInstanceName="cbFINNumber" Width="100%" TextFormatString="{0} / {1}" ValueField="Id" ValueType="System.Int32">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetLine(); GetType(); GetColor(); GetBauMaster();}" />
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="VINNumber" Width="200px" />
                                            <dx:ListBoxColumn FieldName="FINNumber" Width="200px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CS">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox AutoPostBack="true" ID="CSTypeValue" ClientInstanceName="CSTypeValue" runat="server" OnSelectedIndexChanged="CSTypeValue_SelectedIndexChanged"
                                        Width="100%"
                                        NullText="-Select-">
                                        <Items>
                                            <dx:ListEditItem Text="VoCA" Value="4" />
                                            <dx:ListEditItem Text="PA" Value="3" />
                                        </Items>
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
                                      OnSelectedIndexChanged="Auditor1_SelectedIndexChanged" ClientInstanceName="Auditor1"
                                        TextField="UserName" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Auditor2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="Auditor2" runat="server" AutoPostBack="false" Width="100%"
                                       OnSelectedIndexChanged="Auditor2_SelectedIndexChanged" ClientInstanceName="Auditor2"
                                        TextField="UserName" ValueField="UserId" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
                    <dx:ASPxButton ID="BtnMainSaveCloseNew" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="BtnSaveMain_Click">
                    </dx:ASPxButton>
                  <%--  <dx:ASPxButton ID="BtnResetMain" runat="server" Text="Reset" AutoPostBack="false">
                        <ClientSideEvents Click="function(s, e) {Reset()}" />
                    </dx:ASPxButton>--%>
                    <dx:ASPxButton ID="BtnMainSaveClose" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="BtnMainSaveClose_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="BtnMainSaveCloseEdit" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="BtnMainSaveCloseEdit_Click" Visible="false">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="BtnMainClose" runat="server" Text="Close" AutoPostBack="false" OnClick="BtnMainClose_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="pcFaultDesc" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcFaultDesc" Width="800px"
        HeaderText="Add New Part Description" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="pcFaultDescForm" runat="server" Width="100%" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="Code">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="pcFaultDescCode" runat="server" Width="100%" ClientInstanceName="pcFaultDescCode" MaxLength="50">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Description">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="pcFaultDescDescription" runat="server" Width="100%" ClientInstanceName="pcFaultDescCode" MaxLength="100">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
`                   <dx:ASPxButton ID="pcFaultDescSave" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="pcFaultDescSave_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="pcFaultDescCancel" runat="server" Text="Close" AutoPostBack="false" OnClick="pcFaultDescCancel_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="pcFaultPart" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcFaultPart" Width="800px"
        HeaderText="Add New Part" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="pcFaultPartForm" runat="server" Width="100%" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="Code">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="pcFaultPartCode" runat="server" Width="100%" ClientInstanceName="pcFaultPartCode" MaxLength="50">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Description">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="pcFaultPartDescription" runat="server" Width="100%" ClientInstanceName="pcFaultPartDescription" MaxLength="100">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
`                   <dx:ASPxButton ID="pcFaultPartSave" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="pcFaultPartSave_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="pcFaultPartCancel" runat="server" Text="Close" AutoPostBack="false" OnClick="pcFaultPartCancel_Click">
                    </dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="pcFaulRecordDetail" runat="server" ShowCloseButton="false" CloseOnEscape="false" CloseAction="None" OnDataBinding="pcFaulRecordDetail_DataBinding"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcInteruptFault" Width="1200px" Height="500px"
        HeaderText="Fault Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade" EnableViewState="false" OnWindowCallback="pcFaulRecordDetail_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="100%" ColCount="2">
                    <Items>
                        <dx:LayoutItem Caption="Id" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="TextId" runat="server" AutoPostBack="false" Visible="false" Width="400px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Audit Id" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="IdFaultAudit" runat="server" AutoPostBack="false" Visible="false" Width="400px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Fault Description">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <div style="display: inline-flex">
                                        <dx:ASPxComboBox ID="cbFaultDesc" runat="server" ClientInstanceName="cbFaultDesc" DropDownStyle="DropDown" TextFormatString="{0} - {1}" ValueField="FaultDescriptionId" ValueType="System.Int32" Width="400px">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {cbFaultDescx()}" />
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                                <dx:ListBoxColumn FieldName="Description" Width="300px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                        <dx:ASPxComboBox ID="cbFaultDescDesc" runat="server" ClientInstanceName="cbFaultDescDesc"  DropDownStyle="DropDown" ValueField="FaultDescriptionId" ValueType="System.Int32" Width="200px" Visible="false">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {cbFaultDescDescx()}" />
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Code" Visible="False" Width="100px" />
                                                <dx:ListBoxColumn FieldName="Description" Width="300px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </div>
                                    <dx:ASPxButton ID="btnFaultAdd" runat="server" ClientInstanceName="btnAddPart" CssClass="btn btn-primary btn-xs" OnClick="btnAddFaultDesc_Click" RenderMode="Link" Text="New">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Assembly Section" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbAssemblySection" Width="100%" ClientInstanceName="tbAssemblySection" runat="server"
                                        ClientEnabled="false">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Description Details" Visible="False">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txDescDetail" runat="server" ClientInstanceName="txDescDetail" Width="400px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Classification" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbClassification" runat="server" Width="400px" ClientInstanceName="tbClassification" ClientEnabled="False" NullText="NoData">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Fault Related" Visible="false">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbFaultRelated" runat="server" AutoPostBack="false" Width="400px" Visible="false"
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
                        <dx:LayoutItem Caption="Reponsible">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbReponsibleLine" runat="server" AutoPostBack="false" Width="100%"
                                        DataSourceID="dataReponsible"
                                        ValueField="Id" ValueType="System.Int32">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Id" Width="100px" Visible="false" />
                                            <dx:ListBoxColumn FieldName="Description" Width="100px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Part" RowSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                   <div style="display: inline-flex;">
                                        <dx:ASPxComboBox ID="PUcbPartProces" ClientInstanceName="PUcbPartProces" TextFormatString="{0} - {1}" DropDownStyle="DropDown" runat="server" AutoPostBack="false"  Width="400px"
                                            ValueField="Id" ValueType="System.Int32">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Code" Width="100px" />
                                                <dx:ListBoxColumn FieldName="Description" Width="300px" />
                                            </Columns>
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {PartProc()}" />
                                        </dx:ASPxComboBox>
                                        <dx:ASPxComboBox ID="PUcbPartProcesDesc" ClientInstanceName="PUcbPartProcesDesc" AutoPostBack="false" DropDownStyle="DropDown" runat="server" Width="200px" Visible="false"
                                            ValueField="Id" ValueType="System.Int32">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Code" Width="100px" Visible="false" />
                                                <dx:ListBoxColumn FieldName="Description" Width="300px" />
                                            </Columns>
                                              <ClientSideEvents SelectedIndexChanged="function(s, e) {PartProcDesc()}" />
                                        </dx:ASPxComboBox>
                                    </div>
                                    <dx:ASPxButton AutoPostBack="true" ID="btnAddPart" runat="server" ClientInstanceName="btnAddPart" RenderMode="Link" Text="New" OnClick="btnAddPart_Click" CssClass="btn btn-primary btn-xs">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Analysis">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbAnalysis" runat="server" AutoPostBack="false" Width="100%">
                                        <Items>
                                            <dx:ListEditItem Text="Yes" Value="1" />
                                            <dx:ListEditItem Text="No" Value="0" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Rework">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbRework" runat="server" AutoPostBack="false" Width="100%">
                                        <Items>
                                            <dx:ListEditItem Text="Yes" Value="1" />
                                            <dx:ListEditItem Text="No" Value="0" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Area">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmb_Area" runat="server" ClientInstanceName="cmb_Area" DataSourceID="DataArea" TextField="Description" ValueField="Id" ValueType="System.Int32" Width="400px">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Remarks">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txRemarks" runat="server" Width="100%" ClientInstanceName="txRemarks">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Prio">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbPriority" runat="server" AutoPostBack="false" Width="400px">
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
                        <dx:LayoutItem Caption="Default Image">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbDefaultImage" ValueField="Id" TextField="FileName" runat="server" AutoPostBack="false" Width="400">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Originator">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cbStation" ClientInstanceName="cbStation" runat="server"
                                        AutoPostBack="false" Width="400px" DataSourceID="sdsStation"
                                        TextField="StationName" ValueField="Id" ValueType="System.Int32"
                                        IncrementalFilteringMode="Contains">
                                        <%-- <ClientSideEvents SelectedIndexChanged="function(s, e) {e.processOnServer = false; GetSection();}" />--%>
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
                <dx:ASPxFormLayout ID="Attachment" runat="server">
                    <Items>
                        <dx:LayoutItem Caption="Attachment File">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxGridView runat="server" ViewStateMode="Enabled" ID="GVDataAttachment" OnDataBinding="GVDataAttachment_DataBinding" OnHtmlRowPrepared="GVDataAttachment_HtmlRowPrepared" ClientInstanceName="GVDataAttachment" KeyFieldName="Id" Width="100%">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileName" VisibleIndex="1">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileLocation" VisibleIndex="6">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileType" VisibleIndex="3" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="FileBinnary" VisibleIndex="4" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataCheckColumn VisibleIndex="5" Caption="Default" Visible="false">
                                                <DataItemTemplate>
                                                    <dx:ASPxCheckBox ID="CheckBoxDefault" runat="server" ClientInstanceName="CheckBoxDefault" Visible="false">
                                                    </dx:ASPxCheckBox>
                                                </DataItemTemplate>
                                            </dx:GridViewDataCheckColumn>
                                            <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-Text="Download" PropertiesHyperLinkEdit-Target="_blank" Caption="Action" VisibleIndex="3">
                                                <PropertiesHyperLinkEdit Text="Download"></PropertiesHyperLinkEdit>
                                            </dx:GridViewDataHyperLinkColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="6">
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
                    <dx:ASPxButton ID="BtnDetailFaultSaveNew" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="BtnDetailFaultSaveNew_Click">
                    </dx:ASPxButton>
                    <%--   <dx:ASPxButton ID="BtnDetailFaultSaveNewEdit" runat="server" Text="Save & Record New" AutoPostBack="false" OnClick="BtnDetailFaultSaveNewEdit_Click">
                    </dx:ASPxButton>--%>
                   <%-- <dx:ASPxButton ID="BtnResetFaultDetail" runat="server" Text="Reset" AutoPostBack="false">
                        <ClientSideEvents Click="function(s, e) {Reset()}" />
                    </dx:ASPxButton>--%>
                    <dx:ASPxButton ID="BtnFaultDetailSaveClose" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="BtnFaultDetailSaveClose_Click">
                    </dx:ASPxButton>
                    <%--  <dx:ASPxButton ID="BtnFaultDetailSaveCloseEdit" runat="server" Text="Save & Close" AutoPostBack="false" OnClick="BtnFaultDetailSaveCloseEdit_Click">
                    </dx:ASPxButton>--%>
                    <dx:ASPxButton ID="BtnFaultDetailClose" runat="server" Text="Close" AutoPostBack="false" OnClick="BtnFaultDetailClose_Click">
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
                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Width="450px">
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
    <asp:SqlDataSource ID="DataGvMainFault" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select A.Id,a.CS,a.AuditDate,a.VINNumber,a.FINNumber,a.Plant,a.Mileage,b.UserName User1,c.UserName User2 from FaultAudit a 
inner join Users b on b.Id = a.Auditor1UserId
inner join Users c on c.Id = a.Auditor2UserId"></asp:SqlDataSource>

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
        SelectCommand="select Id,Name StationName from FaultAuditOriginator  where IsActive=1"></asp:SqlDataSource>

    <asp:SqlDataSource ID="dataReponsible" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Description from faultauditresponsible where IsActive=1"></asp:SqlDataSource>


    <asp:SqlDataSource ID="sdsFINNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from ProductionHistories"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFINNumberVPC" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, VINNumber, FINNumber from VPCStorage where FINNumber is not null"></asp:SqlDataSource>

    <asp:SqlDataSource ID="gvDetailData" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select a.Id,b.Description Responsible,c.Description PartProcess,d.Description faultDescription,e.Description faultArea,f.StationName,a.Analysis,a.CreatedDate,a.CreateBy,a.Prio,a.Rework from FaultAuditRecord a 
inner join FaultAuditResponsible b on a.FaultAuditResponsibleId = b.Id
inner join FaultPartProcess c on a.MaterialId = c.FaultPartProcessId
inner join FaultDescription d on a.FaultDescriptionId = d.FaultDescriptionId
inner join FaultArea e on a.FaultAreaId = e.id
inner join Stations f on f.Id = a.StationId"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFaultPartProcess" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Code, Description from FaultAuditParts where IsActive=1"></asp:SqlDataSource>

    <asp:SqlDataSource ID="DataArea" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id,Code, Description from FaultArea where IsActive=1"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFaultDescription" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select FaultDescriptionId, Code, Description from FaultDescription where IsActive=1"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsFaultRelatedType" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select FaultRelatedTypeId, Code, Description from FaultRelatedType where IsActive=1"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsAssemblySection" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, AssemblySectionName from AssemblySection"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUserVOCA" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="DECLARE @AuditorListType VARCHAR(100)
SELECT @AuditorListType = 'VOCA Auditor'
SELECT us.Id as UserId, us.FirstName + ' ' + us.LastName AS UserName 
FROM Users us INNER JOIN Configs co ON co.[Key] = us.UserName 
WHERE co.[Group] = @AuditorListType OR @AuditorListType IS NULL
"></asp:SqlDataSource>
     <asp:SqlDataSource ID="sdsUserPA" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="DECLARE @AuditorListType VARCHAR(100)
SELECT @AuditorListType = 'PA Auditor'
SELECT us.Id as UserId, us.FirstName + ' ' + us.LastName AS UserName 
FROM Users us INNER JOIN Configs co ON co.[Key] = us.UserName 
WHERE co.[Group] = @AuditorListType OR @AuditorListType IS NULL
"></asp:SqlDataSource>
     <asp:SqlDataSource ID="sdsUserALL" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="DECLARE @AuditorListType VARCHAR(100)
SELECT Distinct us.Id as UserId, us.FirstName + ' ' + us.LastName AS UserName 
FROM Users us INNER JOIN Configs co ON co.[Key] = us.UserName 
WHERE co.[Group] = @AuditorListType OR @AuditorListType IS NULL
"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsStampNumber" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[UserId],[StampNumber] FROM [dbo].[EmployeeStampNumber]"></asp:SqlDataSource>
   

</asp:Content>
