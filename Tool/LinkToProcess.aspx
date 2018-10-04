<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="LinkToProcess.aspx.cs" Inherits="Ebiz.WebForm.custom.Tool.LinkToProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Tool Assign Process</h4>
    <script type="text/javascript">
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth - 80;
            if (s.GetWidth() != windowInnerWidth) {
                s.SetWidth(windowInnerWidth);
                s.UpdatePosition();
            }
            var windowInnerHeight = window.innerHeight - 80;
            if (s.GetHeight() != windowInnerHeight) {
                s.SetHeight(windowInnerHeight);
                s.UpdatePosition();
            }

        }
        function LinkToProcessGWIS_Click(sender, visibleIndex, buttonId) {
            console.log('test');

            var grid = eval(sender);
            grid.GetRowValues(visibleIndex, 'GwisProcessId;StationId;GwisPartId', LinkToProcessGWIS_OnGetRowValues);
        }

        function LinkToProcessGWIS_OnGetRowValues(values) {
            popupLinkToProcess.Show();
            console.log(values[1]);
            popupLinkToProcess.PerformCallback(values[0] + ";" + values[1] + ";" + values[2]);
        }
        function OnProductionLineChanged(s, e) {
            gvToolAssign.PerformCallback(cmbProductionLines.GetSelectedItem().value.toString());
        }
    </script>

    <dx:ASPxPopupControl ID="popupLinkToProcess" ClientInstanceName="popupLinkToProcess" runat="server" AllowDragging="false" AutoUpdatePosition="True"
        CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="Tool Process" Modal="True"
        PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="90" MinHeight="90" OnWindowCallback="popupLinkToProcess_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="frmToolAssign" runat="server">
                    <Items>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbProductionLines" Visible="false" runat="server" ClientInstanceName="cmbProductionLines" DataSourceID="sdsProductionLineLookup" ValueField="Id" TextField="LineName" SelectedIndex="0">
                                        <ClientSideEvents SelectedIndexChanged="OnProductionLineChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="" HorizontalAlign="Center">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <br />
                                    <dx:ASPxGridView OnPageIndexChanged="ASPxGridView1_PageIndexChanged" OnLoad="ASPxGridView1_Load" OnCustomUnboundColumnData="ASPxGridView1_CustomUnboundColumnData" OnCustomCallback="ASPxGridView1_CustomCallback" ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" OnRowUpdating="ASPxGridView1_RowUpdating" OnRowUpdated="ASPxGridView1_RowUpdated" OnCommandButtonInitialize="ASPxGridView1_CommandButtonInitialize">
                                        <Columns>
                                            <dx:GridViewDataTextColumn VisibleIndex="0" Width="30" Caption="No." FieldName="Number" UnboundType="String">
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="AssignmentProcessId" Caption="AssignmentProcessId" VisibleIndex="10" ReadOnly="true" Visible="true" Width="-20">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="2" FieldName="Sequence" ShowInCustomizationForm="True" Name="Sequence" CellStyle-HorizontalAlign="Center" Caption="Sequence" Width="90">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="3" Caption="Tool No" FieldName="ToolNo" ReadOnly="true" Visible="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="4" Caption="GWISProcessId" FieldName="GWISProcessId" ReadOnly="true" Name="GWISProcessId" Visible="false">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="5" Caption="GWISPartId" FieldName="GWISPartId" ReadOnly="true" Visible="false">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="6" Caption="ToolInventoryId" FieldName="ToolInventoryId"  ReadOnly="true" Visible="false">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SettingTorque" Caption="Setting Torque" ReadOnly="true" CellStyle-HorizontalAlign="Center" VisibleIndex="4" Visible="true">
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="StationName" Caption="Station" VisibleIndex="7" Visible="true" ReadOnly="true">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="UsageModels" Caption="Usage Models" VisibleIndex="8" Visible="true"  ReadOnly="true">
                                            </dx:GridViewDataTextColumn>

                                        </Columns>
                                        <Styles>
                                            <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                        </Styles>
                                        <SettingsCommandButton>
                                            <UpdateButton Styles-Style-HorizontalAlign="Center" Styles-FocusRectStyle-HorizontalAlign="Center"></UpdateButton>
                                        </SettingsCommandButton>
                                        <SettingsSearchPanel Visible="true" />
                                        <SettingsBehavior ColumnResizeMode="Control" />
                                        <SettingsEditing Mode="Batch"></SettingsEditing>
                                        <SettingsPager Position="Bottom">
                                        <PageSizeItemSettings Items="15, 50, 100" />
                                        </SettingsPager>
                                    </dx:ASPxGridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>"
                                        DeleteCommand="DELETE FROM [ToolAssignmentProcesses] WHERE [Id] = @Id"
                                        InsertCommand="INSERT INTO [ToolAssignmentProcesses] ([ToolInventoryId], [GWISProcessId], [GWISPartId], [Sequence], [ValidFrom], [ValidTo]) VALUES (@ToolInventoryId, @GWISProcessId, @GWISPartId, @Sequence, @ValidFrom, @ValidTo)"
                                        SelectCommand="select ROW_NUMBER() over (order by aa.gwispartid) Id, * from (select distinct
    ti.Id as ToolInventoryId,
	tap.[Sequence],
    tap.Id as AssignmentProcessId,
	ti.Number as ToolNo,
	ti.SetNM as SettingTorque,
	s.StationName,
	gp.Id as GWISProcessId,
	gs.Id as GWISPartId,
	gp.ValidFrom,
	gp.ValidTo,
	STUFF
		((SELECT ', ' + at.Name + ' ' + cm.Model
		FROM    dbo.ToolAssignmentModels tam 
		LEFT JOIN dbo.AssemblyTypes at on at.Id = tam.AssemblyTypeId
		LEFT JOIN dbo.CatalogModels cm on cm.Id = tam.CatalogModelId
		WHERE tam.ToolInventoryId = ti.Id FOR XML PATH('')), 1, 2, '') as UsageModels
		
from dbo.ToolInventories ti
LEFT JOIN Stations s on s.Id = ti.StationId
left join GWISProcesses gp on gp.Id = @gwisprocessid
left join GWISParts gs on gs.ProcessId = gp.Id
LEFT JOIN ToolAssignmentProcesses tap on tap.ToolInventoryId = ti.Id and tap.GwisProcessId = gp.Id 
where 
ti.StationId = @StationId and
ti.ProductionLineId = @prodline and gs.Id = @gwispartid ) aa"
                                        UpdateCommand="usp_Tool_AssignProcess" UpdateCommandType="StoredProcedure">
                                        <DeleteParameters>
                                            <asp:Parameter Name="Id" Type="Int32" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="ToolInventoryId" Type="Int32" />
                                            <asp:Parameter Name="GWISProcessId" Type="Int32" />
                                            <asp:Parameter Name="GWISPartId" Type="Int32" />
                                            <asp:Parameter Name="Sequence" Type="Int32" />
                                            <asp:Parameter Name="ValidFrom" Type="DateTime" />
                                            <asp:Parameter Name="ValidTo" Type="DateTime" />
                                            <%--<asp:Parameter Name="CreatedDate" Type="DateTime" />
                                            <asp:Parameter Name="CreatedBy" Type="String" />
                                            <asp:Parameter Name="ModifiedDate" Type="DateTime" />
                                            <asp:Parameter Name="ModifiedBy" Type="String" />
                                            <asp:Parameter Name="IsDeleted" Type="Boolean" />--%>
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="GWISPartId" Type="Int32" />
                                            <asp:Parameter Name="GWISProcessId" Type="Int32" />
                                            <asp:Parameter Name="ToolInventoryId" Type="Int32" />
                                            <asp:Parameter Name="Sequence" Type="Int32" />
<%--                                            <asp:Parameter Name="UserName" Type="String" />--%>
                                            <asp:Parameter Name="ToolAssignmentProcessId" Type="Int32" />
                                            <asp:Parameter Name="Id" Type="Int32" />
                                        </UpdateParameters>
                                        <SelectParameters>
                                            <asp:Parameter Name="gwisprocessid" Type="Int32" DefaultValue="0" />
                                            <asp:Parameter Name="StationId" Type="Int32" DefaultValue="0" />
                                            <asp:Parameter Name="prodline" Type="Int32" DefaultValue="0" />
                                            <asp:Parameter Name="gwispartid" Type="Int32" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <br />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle Paddings-Padding="0">
            <Paddings Padding="0px"></Paddings>
        </ContentStyle>
        <ClientSideEvents Shown="onPopupShown" />
    </dx:ASPxPopupControl>

    <asp:SqlDataSource ID="sdsProductionLineLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select pl.Id, pl.LineName from [dbo].[ProductionLines] pl where pl.LineNumber < 20 and pl.AssemblyTypeId = @AssemblyTypeId">
        <SelectParameters>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
