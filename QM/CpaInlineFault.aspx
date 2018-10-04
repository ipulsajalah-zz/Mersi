<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="CpaInlineFault.aspx.cs" Inherits="Ebiz.WebForm.custom.QM.CpaInlineFault" %>



<%--<%@ Register Assembly="DevExpress.Web.v14.2" Namespace="DevExpress.Web" TagPrefix="dx" %>
--%>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Expires" content="0" />
        <style>
            .image {
                width: 1950px;
                height: 500px;
                z-index: 3;
                background-size: 100% 100%;
                background-image: url("../../Content/Images/draw2.png");
                background-repeat: repeat-y;
                background-attachment: inherit;
                -webkit-background-size: 100% 100%;
                -moz-background-size: 100% 100%;
                -o-background-size: 100% 100%;
                -ms-background-size: 100% 100%;
            }
        </style>
        <script type="text/javascript">
           function onPopupShown(s, e) {
                var windowInnerWidth = window.innerWidth - 40;
                if (s.GetWidth() != windowInnerWidth) {
                    s.SetWidth(windowInnerWidth);
                    s.UpdatePosition();
                }

                var windowInnerHeight = window.innerHeight - 20;
                if (s.GetHeight() != windowInnerHeight) {
                    s.SetHeight(windowInnerHeight);
                    s.UpdatePosition();
                }
            }

            function CloseUploadControl() {
                PopUpAttachMent.Hide();
                GvContainmentAction.PerformCallback();
            }

            function CloseUploadControlRootCauseAnalysis() {
                PopUpAttachMentRootCauseAnalysis.Hide();
                GvRootCauseAnalysis.PerformCallback();
               
            }

            function CloseUploadControlCorrectiveAction() {
                PopUpAttachMentCorrectiveAction.Hide();
                GvCorrectiveAction.PerformCallback();

            }

            function CloseUploadControlMonitoringEffectiveness() {
                PopUpAttachMentMonitoringEffectiveness.Hide();
                GvMonitoringEffectiveness.PerformCallback();

            }


            function callpop(s, e) {
                PopUpAttachMent.Show();
            }

            function buildDamageCode(grid, column, value) {
                var editor = grid.GetEditor(column);
                if (editor == null)
                    return;

                
                var editorFaultConstructionGroupId = grid.GetEditor("FaultConstructionGroupId");
                var editorFaultLocationId = grid.GetEditor("FaultLocationId");
                var editorFaultTypeId = grid.GetEditor("FaultTypeId");
                var editorFaultDamageTypeId = grid.GetEditor("FaultDamageTypeId");
                
                var editordamagacode = grid.GetEditor("DamageCode");
                var editorFaultClassId = grid.GetEditor("FaultClassId");
                var editorWeightage = grid.GetEditor("Weightage");
                $.ajax({
                    type: "POST",
                    url: "CpaInlineFault.aspx/SetDamageCode",
                    data: "{'FaultConstructionGroupId':'" + editorFaultConstructionGroupId.GetText() + "', 'FaultLocationId':'" + editorFaultLocationId.GetText() + "', 'FaultTypeId':'" + editorFaultTypeId.GetText() + "', 'FaultDamageTypeId':'" + editorFaultDamageTypeId.GetText() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                      
                        var tmp = JSON.parse(data["d"])
                        editordamagacode.SetValue(String(tmp.damagecode));
                        editorFaultClassId.SetValue(parseInt(tmp.classes));
                        editorWeightage.SetValue(parseInt(tmp.weight));


                        //editordamagacode.SetValue("s");
                        //editorFaultClassId.SetValue(1);
                        //editorWeightage.SetValue(1);


                    },
                    failure: function (response) {
            //        alert(response.d);
                }
            });
     
            }

            function CallPopUP(sender, visibleIndex, buttonId) {
                var grid = eval(sender);
                grid.GetRowValues(visibleIndex, "Id", CallPopUP_OnGetRowValues);
            }

            function CallPopUP_OnGetRowValues(value) {
                console.log(value);
                popupReport.SetContentUrl("/Reports/ProductAuditReport.aspx?Id=" + value);
                popupReport.SetHeaderText("Product Audit Report");
                popupReport.Show();
            }


        </script>

    </head>
    <body>
        <dx:ASPxPopupControl ID="popupReport" ScrollBars="Auto" ClientInstanceName="popupReport" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="DETAIL" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="40" MinHeight="20" OnWindowCallback="popupReport_WindowCallback">
            <ContentStyle Paddings-Padding="0">
                <Paddings Padding="0px"></Paddings>
            </ContentStyle>
            <ClientSideEvents Shown="onPopupShown" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxPageControl ID="DetailTabPage" Width="100%" runat="server" CssClass="dxtcFixed" ActiveTabIndex="1" EnableHierarchyRecreation="True">
                        <TabPages>
                            <dx:TabPage Text="Action">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl1" runat="server">
                                        <dx:ASPxFormLayout ID="frmLayoutCheckingCode" DataSourceID="dsAction" runat="server" ColCount="2">
                                            <Items>

                                                <dx:LayoutItem Caption="" FieldName="FaultAuditRecordsId">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<asp:HiddenField ID="TbFaultAuditRecordsId" Value='<%# Eval("Id") %>' runat="server" />--%>
                                                            <br />
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Containment Action" FieldName="ContainmentAction">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxMemo ID="TbContainmentAction" Rows="10" Width="350px" runat="server"></dx:ASPxMemo>
                                                            <br />
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxButton runat="server" ID="TbContainmentActionAtt" ClientInstanceName="TbContainmentActionAtt" Text="Add Attachment" OnClick="TbContainmentActionAtt_Click"></dx:ASPxButton>--%>

                                                            <dx:ASPxButton ID="TbContainmentActionAtt" runat="server" Text="Add Attachment" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                                <ClientSideEvents Click="function(s, e) {callpop(s,e); }" />
                                                            </dx:ASPxButton>
                                                            <br />
                                                            <br />
                                                            <dx:ASPxGridView ID="GvContainmentAction" ClientInstanceName="GvContainmentAction" SettingsBehavior-ConfirmDelete="true" OnRowDeleting="GvContainmentAction_RowDeleting" OnLoad="GvContainmentAction_Load" OnInit="GvContainmentAction_Init"  runat="server" AutoGenerateColumns="False" KeyFieldName="Id">

                                                              <%--  <EditFormLayoutProperties ColCount="1"></EditFormLayoutProperties>--%>
                                                                <Columns>
                                                                    <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-TextField="FileName" PropertiesHyperLinkEdit-Target="_blank" Caption="FileName" VisibleIndex="0">
                                                                        <PropertiesHyperLinkEdit></PropertiesHyperLinkEdit>
                                                                    </dx:GridViewDataHyperLinkColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="Id" Visible="false" ReadOnly="True" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewCommandColumn ShowDeleteButton="True" VisibleIndex="2">
                                                                    </dx:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                                                                <SettingsText ConfirmDelete="Are you sure want to delete this data?" />
<%--                                                                <SettingsAdaptivity>
                                                                    <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                                                                </SettingsAdaptivity>--%>

                                                                <SettingsPager Position="Bottom" PageSize="3">
                                                                    <PageSizeItemSettings Items="2,5" Visible="True" />
                                                                </SettingsPager>
                                                            </dx:ASPxGridView>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Root Cause Analysis" FieldName="RootCauseAnalysis">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxMemo ID="TbRootCauseAnalysis" Rows="10" Width="350px" runat="server"></dx:ASPxMemo>
                                                            <br />
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxButton runat="server" ID="RootCauseAnalysisAtt" Text="Add Attachment"></dx:ASPxButton>--%>
                                                              <dx:ASPxButton ID="RootCauseAnalysisAtt" runat="server" Text="Add Attachment" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                                <ClientSideEvents Click="function(s, e) { PopUpAttachMentRootCauseAnalysis.Show(); }" />
                                                            </dx:ASPxButton>
                                                            <br />
                                                            <br />
                                                            <dx:ASPxGridView ID="GvRootCauseAnalysis" PageSize="3" ClientInstanceName="GvRootCauseAnalysis" runat="server" SettingsBehavior-ConfirmDelete="true" AutoGenerateColumns="False" KeyFieldName="Id" OnInit="GvRootCauseAnalysis_Init" OnLoad="GvRootCauseAnalysis_Load" OnRowDeleting="GvRootCauseAnalysis_RowDeleting">
                                                                <Columns>
                                                                    <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-TextField="FileName" PropertiesHyperLinkEdit-Target="_blank" Caption="FileName" VisibleIndex="0">
                                                                        <PropertiesHyperLinkEdit></PropertiesHyperLinkEdit>
                                                                    </dx:GridViewDataHyperLinkColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="Id" Visible="false" ReadOnly="True" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewCommandColumn ShowDeleteButton="True" VisibleIndex="2">
                                                                    </dx:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                                                                <SettingsText ConfirmDelete="Are you sure want to delete this data?" />
                                                                <SettingsPager Position="Bottom" PageSize="2">
                                                                    <PageSizeItemSettings Items="2,5" Visible="True" />
                                                                </SettingsPager>
                                                            </dx:ASPxGridView>
                                                            <br />
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Corrective Action" FieldName="CorrectiveAction">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxMemo ID="TbCorrectiveAction" Rows="10" Width="350px" runat="server"></dx:ASPxMemo>
                                                            <br />
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxButton runat="server" ID="TbCorrectiveActionAtt" Text="Add Attachment"></dx:ASPxButton>--%>
                                                            <dx:ASPxButton ID="TbCorrectiveActionAtt" runat="server" Text="Add Attachment" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                                <ClientSideEvents Click="function(s, e) { PopUpAttachMentCorrectiveAction.Show(); }" />
                                                            </dx:ASPxButton>

                                                            <br />
                                                            <br />
                                                            <dx:ASPxGridView ID="GvCorrectiveAction" ClientInstanceName="GvCorrectiveAction" runat="server" SettingsBehavior-ConfirmDelete="true" AutoGenerateColumns="False" KeyFieldName="Id" OnInit="GvCorrectiveAction_Init" OnLoad="GvCorrectiveAction_Load" OnRowDeleting="GvCorrectiveAction_RowDeleting">
                                                                <Columns>
                                                                    <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-TextField="FileName" PropertiesHyperLinkEdit-Target="_blank" Caption="FileName" VisibleIndex="0">
                                                                        <PropertiesHyperLinkEdit></PropertiesHyperLinkEdit>
                                                                    </dx:GridViewDataHyperLinkColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="Id" Visible="false" ReadOnly="True" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewCommandColumn ShowDeleteButton="True" VisibleIndex="2">
                                                                    </dx:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                                                                <SettingsText ConfirmDelete="Are you sure want to delete this data?" />
                                                                <SettingsPager Position="Bottom" PageSize="2">
                                                                    <PageSizeItemSettings Items="2,5" Visible="True" />
                                                                </SettingsPager>
                                                            </dx:ASPxGridView>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Monitoring Effectiveness" FieldName="MonitoringEffectiveness">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxMemo ID="TbMonitoringEffectiveness" Rows="10" Width="350px" runat="server"></dx:ASPxMemo>
                                                            <br />
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxButton runat="server" ID="TbMonitoringEffectivenessAtt" Text="Add Attachment"></dx:ASPxButton>--%>
                                                            <dx:ASPxButton ID="TbMonitoringEffectivenessAtt" runat="server" Text="Add Attachment" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                                <ClientSideEvents Click="function(s, e) { PopUpAttachMentMonitoringEffectiveness.Show(); }" />
                                                            </dx:ASPxButton>
                                                            <br />
                                                            <br />
                                                            <dx:ASPxGridView ID="GvMonitoringEffectiveness" ClientInstanceName="GvMonitoringEffectiveness" runat="server" SettingsBehavior-ConfirmDelete="true" AutoGenerateColumns="False" KeyFieldName="Id" OnInit="GvMonitoringEffectiveness_Init" OnLoad="GvMonitoringEffectiveness_Load" OnRowDeleting="GvMonitoringEffectiveness_RowDeleting">
                                                                <Columns>
                                                                    <dx:GridViewDataHyperLinkColumn FieldName="FileLocation" PropertiesHyperLinkEdit-TextField="FileName" PropertiesHyperLinkEdit-Target="_blank" Caption="FileName" VisibleIndex="0">
                                                                        <PropertiesHyperLinkEdit></PropertiesHyperLinkEdit>
                                                                    </dx:GridViewDataHyperLinkColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="Id" Visible="false" ReadOnly="True" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewCommandColumn ShowDeleteButton="True" VisibleIndex="2">
                                                                    </dx:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                                                                <SettingsText ConfirmDelete="Are you sure want to delete this data?" />
                                                                <SettingsPager Position="Bottom" PageSize="2">
                                                                    <PageSizeItemSettings Items="2,5" Visible="True" />
                                                                </SettingsPager>
                                                            </dx:ASPxGridView>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="" CaptionCellStyle-Paddings-PaddingTop="20">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxButton runat="server" ID="btnSaveAction" OnClick="btnSaveAction_Click" Text="Save"></dx:ASPxButton>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionCellStyle>
                                                        <Paddings PaddingTop="20px"></Paddings>
                                                    </CaptionCellStyle>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:ASPxFormLayout>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="RCA">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl" runat="server">
                                        <div class="image">
                                            <br>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMan1" runat="server" Style="margin-left: 270px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMachine1" runat="server" Style="margin-left: 265px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMaterial1" runat="server" Style="margin-left: 227px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMan2" runat="server" Style="margin-left: 65px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMachine2" runat="server" Style="margin-left: 260px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMaterial2" runat="server" Style="margin-left: 242px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMan3" runat="server" Style="margin-left: 345px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMachine3" runat="server" Style="margin-left: 270px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMaterial3" runat="server" Style="margin-left: 245px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMan4" runat="server" Style="margin-left: 170px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMachine4" runat="server" Style="margin-left: 270px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMaterial4" runat="server" Style="margin-left: 230px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMan5" runat="server" Style="margin-left: 433px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMachine5" runat="server" Style="margin-left: 278px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TbMaterial5" runat="server" Style="margin-left: 230px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TextBox16" runat="server" Style="margin-left: 1630px; width: 200px;"></asp:TextBox>
                                            <br>
                                            
                                            <asp:TextBox ID="TbMethod1" runat="server" Style="margin-left: 630px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TextBox18" ReadOnly="true" runat="server" Style="margin-left: 300px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMethod2" Text='<%# Eval("Method2") %>' runat="server" Style="margin-left: 360px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TextBox20" ReadOnly="true" runat="server" Style="margin-left: 288px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMethod3" Text='<%# Eval("Method3") %>' runat="server" Style="margin-left: 560px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TextBox22" ReadOnly="true" runat="server" Style="margin-left: 305px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMethod4" Text='<%# Eval("Method4") %>' runat="server" Style="margin-left: 310px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TextBox24" ReadOnly="true" runat="server" Style="margin-left: 305px; width: 200px;"></asp:TextBox>
                                            <br>
                                            <br>
                                            <asp:TextBox ID="TbMethod5" Text='<%# Eval("Method5") %>' runat="server" Style="margin-left: 500px; width: 200px;"></asp:TextBox>
                                            <asp:TextBox ID="TextBox26" ReadOnly="true" runat="server" Style="margin-left: 305px; width: 200px;"></asp:TextBox>
                                        </div>
                                        <dx:ASPxButton runat="server" ID="btnSaveRca" OnClick="btnSaveRca_Click" Text="Save"></dx:ASPxButton>

                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="Issue Update">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl3" runat="server">
                                        <dx:ASPxGridView ID="gvIssue" runat="server" Width="100%" ClientInstanceName="gvIssue" KeyFieldName="Id" AutoGenerateColumns="False" OnRowUpdating="gvIssue_RowUpdating" OnRowInserting="gvIssue_RowInserting" DataSourceID="dsIssue">
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowDeleteButton="True" Width="5%" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
                                                </dx:GridViewCommandColumn>

                                                <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="false" VisibleIndex="0">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="FaultAuditRecordsId" Visible="false" VisibleIndex="1">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="Date" VisibleIndex="2">
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataMemoColumn FieldName="Remarks" Caption="Remarks" PropertiesMemoEdit-Height="100px" Width="350px">
                                                    <EditFormSettings RowSpan="2" ColumnSpan="2" Visible="True" VisibleIndex="3" />
                                                    <PropertiesMemoEdit Style-Wrap="True"></PropertiesMemoEdit>
                                                </dx:GridViewDataMemoColumn>
                                                <dx:GridViewDataDateColumn FieldName="CreatedDate" VisibleIndex="4">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="CreatedBy" VisibleIndex="5">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="ModifiedDate" VisibleIndex="6">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="ModifiedBy" VisibleIndex="7">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataCheckColumn FieldName="IsDeleted" Visible="false" VisibleIndex="8">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataCheckColumn>
                                            </Columns>
                                            <Settings ShowGroupPanel="true" />
                                            <SettingsBehavior ConfirmDelete="True" EnableCustomizationWindow="true" />
                                            <SettingsText ConfirmDelete="Are you sure want to delete this data?" />
                                            <SettingsSearchPanel Visible="True" />
                                            <SettingsPager Position="Bottom">
                                                <PageSizeItemSettings Items="10, 20, 50,100" Visible="True" />
                                            </SettingsPager>
                                        </dx:ASPxGridView>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                    </dx:ASPxPageControl>


                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


        <%--popup--%>

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
                                        <dx:ASPxUploadControl ID="UploadAttachment" runat="server" Width="300px" FileUploadMode="OnPageLoad" ClientInstanceName="UploadControl"
                                            NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="True" ShowProgressPanel="True"
                                            OnFileUploadComplete="UploadAttachment_FileUploadComplete" ClientSideEvents-FilesUploadComplete="CloseUploadControl">
                                            <ClientSideEvents FilesUploadComplete="CloseUploadControl"></ClientSideEvents>
                                            <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                        </dx:ASPxUploadControl>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        
        <dx:ASPxPopupControl ID="PopUpAttachMentRootCauseAnalysis" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpAttachMentRootCauseAnalysis"
            HeaderText="Attachment Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade"  OnWindowCallback="PopUpAttachMentRootCauseAnalysis_WindowCallback">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="450px">
                        <Items>
                            <dx:LayoutItem Caption="Document File">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxUploadControl ID="UploadRootCauseAnalysis" runat="server" Width="300px" FileUploadMode="OnPageLoad" ClientInstanceName="UploadRootCauseAnalysis"
                                            NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="True" ShowProgressPanel="True"
                                            OnFileUploadComplete="UploadRootCauseAnalysis_FileUploadComplete" ClientSideEvents-FilesUploadComplete="CloseUploadControlRootCauseAnalysis">
                                            <ClientSideEvents FilesUploadComplete="CloseUploadControlRootCauseAnalysis"></ClientSideEvents>
                                            <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                        </dx:ASPxUploadControl>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


         <dx:ASPxPopupControl ID="PopUpAttachMentCorrectiveAction" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpAttachMentCorrectiveAction"
            HeaderText="Attachment Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade"  OnWindowCallback="PopUpAttachMentCorrectiveAction_WindowCallback">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Width="450px">
                        <Items>
                            <dx:LayoutItem Caption="Document File">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxUploadControl ID="UploadPopUpAttachMentCorrectiveAction" runat="server" Width="300px" FileUploadMode="OnPageLoad" ClientInstanceName="UploadPopUpAttachMentCorrectiveAction"
                                            NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="True" ShowProgressPanel="True"
                                            OnFileUploadComplete="UploadPopUpAttachMentCorrectiveAction_FileUploadComplete" ClientSideEvents-FilesUploadComplete="CloseUploadControlCorrectiveAction">
                                            <ClientSideEvents FilesUploadComplete="CloseUploadControlCorrectiveAction"></ClientSideEvents>
                                            <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                        </dx:ASPxUploadControl>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


         <dx:ASPxPopupControl ID="PopUpAttachMentMonitoringEffectiveness" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpAttachMentMonitoringEffectiveness"
            HeaderText="Attachment Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade"  OnWindowCallback="PopUpAttachMentMonitoringEffectiveness_WindowCallback">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server" Width="450px">
                        <Items>
                            <dx:LayoutItem Caption="Document File">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxUploadControl ID="UploadPopUpAttachMentMonitoringEffectiveness" runat="server" Width="300px" FileUploadMode="OnPageLoad" ClientInstanceName="UploadPopUpAttachMentMonitoringEffectiveness"
                                            NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="True" ShowProgressPanel="True" OnFileUploadComplete="UploadPopUpAttachMentMonitoringEffectiveness_FileUploadComplete1"
                                             ClientSideEvents-FilesUploadComplete="CloseUploadControlMonitoringEffectiveness">
                                            <ClientSideEvents FilesUploadComplete="CloseUploadControlMonitoringEffectiveness"></ClientSideEvents>
                                            <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                        </dx:ASPxUploadControl>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <asp:SqlDataSource ID="dsAction" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>"
            SelectCommand="SELECT * FROM [FaultAuditRecords] WHERE [Id] = @Id ">
            <SelectParameters>
                <asp:SessionParameter SessionField="FaultAuditRecordsId" Name="Id" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="dsIssue" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>"
            DeleteCommand="DELETE FROM [FaultAuditIssued] WHERE [Id] = @Id"
            InsertCommand="INSERT INTO [FaultAuditIssued] ([FaultAuditRecordsId], [Date], [Remarks], [CreatedDate], [CreatedBy],[IsDeleted]) VALUES (@FaultAuditRecordsId, @Date, @Remarks, GETDATE(), @CreatedBy,0 )"
            SelectCommand="SELECT * FROM [FaultAuditIssued] WHERE FaultAuditRecordsId IS NOT NULL AND  FaultAuditRecordsId = @FaultAuditRecordsId "
            UpdateCommand="UPDATE [FaultAuditIssued] SET [FaultAuditRecordsId] = @FaultAuditRecordsId, [Date] = @Date, [Remarks] = @Remarks, [ModifiedDate] = GETDATE(), [ModifiedBy] = @ModifiedBy WHERE [Id] = @Id">
            <SelectParameters>
                <asp:SessionParameter SessionField="FaultAuditRecordsId" Name="FaultAuditRecordsId" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:SessionParameter SessionField="FaultAuditRecordsId" Name="FaultAuditRecordsId" Type="Int32" />
                <asp:Parameter Name="Date" Type="DateTime" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:Parameter Name="CreatedBy" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:SessionParameter SessionField="FaultAuditRecordsId" Name="FaultAuditRecordsId" Type="Int32" />
                <asp:Parameter Name="Date" Type="DateTime" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:Parameter Name="ModifiedBy" Type="String" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <br />
        <br />
    </body>
    </html>

</asp:Content>
