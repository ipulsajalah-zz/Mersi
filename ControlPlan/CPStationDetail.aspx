<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="CPStationDetail.aspx.cs" Inherits="Ebiz.WebForm.Tids.CV.ControlPlans.CPStationDetail" %>

<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //
        //This script is used to handle client side action of command buntton
        //

        //function OnAddNew(s, e) {
        //    popupnewButton.Show();
        //}

        //function OnCustomButtonClick(s, e) {
        //    if (e.buttonID == "Copy") {
        //        var value = "Copy_" + e.visibleIndex;
        //        s.PerformCallback(value);
        //    }
        //    else if (e.buttonID == "Col_ActionDeviation") {
        //        var keyValue = masterGrid.GetRowKey(e.visibleIndex);
        //        var cgisNo = masterGrid.GetRowValues(e.visibleIndex, 'CgisNo');
        //        popupDeviation.Show();
        //        popupDeviation.SetHeaderText = 'Deviation - ' + cgisNo;
        //        popupDeviation.PerformCallback(keyValue + ';' + e.buttonID);
        //    } else if (e.buttonID == "Col_ActionMove") {
        //        var keyValue = masterGrid.GetRowKey(e.visibleIndex);
        //        var cgisNo = masterGrid.GetRowValues(e.visibleIndex, 'CgisNo');
        //        popupMoveStation.Show();
        //        popupMoveStation.SetHeaderText = 'Move Process - ' + cgisNo;
        //        popupMoveStation.PerformCallback("Move;" + keyValue);
        //    } else if (e.buttonID == "Col_ActionTool") {
        //        var keyValue = masterGrid.GetRowKey(e.visibleIndex);
        //        //var cgisNo = masterGrid.GetRowValues(e.visibleIndex, 'Cgis No');
        //        //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');
        //        popupToolAssign.Show();
        //        popupToolAssign.SetHeaderText = 'Assign Tool - ' + cgisNo;
        //        popupToolAssign.PerformCallback("Tool;" + keyValue);
        //    } else if (e.buttonID == 'Col_ActionCheckCS2') {
        //        var keyValue = grvCpPerStation.GetRowKey(e.visibleIndex);
        //        popupCheckingCode.Show();
        //        popupCheckingCode.SetHeaderText = 'Checking Code';
        //        popupCheckingCode.PerformCallback('Check;' + e.keyValue);
        //    }
        //}

        function ActionCheckCS2(gridName, visibleIndex, columnName) {
            var grid = window[gridName];
            if (typeof grid == 'undefined' || grid == null)
                return;

            var keyValue = grid.GetRowKey(visibleIndex);
            //var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
            //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

            popupCheckingCode.Show();
            popupCheckingCode.SetHeaderText = 'Checking Code';
            popupCheckingCode.PerformCallback('Check;' + e.keyValue);
        }

        function ActionDeviation(gridName, visibleIndex, columnName) {
            var grid = window[gridName];
            if (typeof grid == 'undefined' || grid == null)
                return;

            var keyValue = grid.GetRowKey(visibleIndex);
            var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
            //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

            popupDeviation.Show();
            popupDeviation.SetHeaderText = 'Deviation - ' + cgisNo;
            popupDeviation.PerformCallback(keyValue + ';' + e.buttonID);
        }

        function ActionMove(gridName, visibleIndex, columnName) {
            var grid = window[gridName];
            if (typeof grid == 'undefined' || grid == null)
                return;

            var keyValue = grid.GetRowKey(visibleIndex);
            var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
            //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

            popupMoveStation.Show();
            popupMoveStation.SetHeaderText = 'Move Process - ' + cgisNo;
            popupMoveStation.PerformCallback("Move;" + keyValue);
        }

        function ActionTool(gridName, visibleIndex, columnName) {
            var grid = window[gridName];
            if (typeof grid == 'undefined' || grid == null)
                return;

            var keyValue = grid.GetRowKey(visibleIndex);
            var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
            //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

            popupToolAssign.Show();
            popupToolAssign.SetHeaderText = 'Assign Tool - ' + cgisNo;
            popupToolAssign.PerformCallback("Tool;" + keyValue);
        }

        function CheckingCode(s, e)
        {
            alert(s.name);
            popupCheckingCode.Show();
            popupCheckingCode.SetHeaderText = 'Checking Code';
            popupCheckingCode.PerformCallback('Check;' + e.keyValue);
        }

        function OnAsemblySectionChanged(s, e) {
            //var x = cmbAsemblySection.GetValue();
            //popupMoveStation.PerformCallback(x + ';' + "Stations");
            cmbStations.PerformCallback("Section;" + cmbAssemblySections.GetSelectedItem().value.toString())
        }

        function OnStationChanged(s, e) {
            cmbStationProcesses.PerformCallback("Station;" + cmbStations.GetSelectedItem().value.toString())
        }


        function OnOrderingOptionChanged(s, e) {
            var x = cmbOrderingOptions.GetValue();
            if (x == 9) {
                cmbStationProcesses.SetEnabled(true);
            }
            else {
                cmbStationProcesses.SetEnabled(false);
            }
        }

        function OnProductionLineChanged(s, e) {
            //cmbToolInventories.PerformCallback(cmbProductionLines.GetSelectedItem().value.toString());
            gvToolAssign.PerformCallback(cmbProductionLines.GetSelectedItem().value.toString());
        }

        function OnModelChanged(cmbModel) {
            cboVariant.PerformCallback(cboModel.GetSelectedItem().value.toString());
        }

        function OnModelNewChanged(cmbModelNew) {
            cboVariantNew.PerformCallback(cboModelNew.GetSelectedItem().value.toString());
        }

    </script>

    <div class="container-fluid" style="width: 100%; padding: 0 0 20px 0 !important;">
        <div style="text-align: center; font-weight: bolder">
            <span style="font-size: 18px;">Control Plan<br />
                <dx:ASPxLabel Style="font-weight: bolder; font-size: large;" CssClass="h2" runat="server" ID="lblStationName" /> <br />
            </span>
            <span style="font-size: 16px;">
                <strong>               
                    PM:&nbsp;<dx:ASPxLabel runat="server" ID="lblPackingMonth" Style="font-size: 16px; font-weight: 700" />
                    &nbsp;&nbsp;&nbsp; Model:&nbsp;<dx:ASPxLabel runat="server" ID="lblModel" Style="font-weight: 700; font-size: 16px" />
                </strong>
            </span>
        </div>
    </div>

<%--    <dx:ASPxPopupControl ID="popupDeviation" ClientInstanceName="popupDeviation" runat="server" HeaderText="Deviation" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupDeviation_OnWindowCallback">

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxMemo ID="memoDeviation" runat="server" Height="100%" Width="100%">
                </dx:ASPxMemo>
            </dx:PopupControlContentControl>
        </ContentCollection>

    </dx:ASPxPopupControl>--%>

    <dx:ASPxPopupControl ID="popupCheckingCode" runat="server" HeaderText="Checking Code" MinHeight="250px" MinWidth="800px"
        AllowDragging="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" CommandArgument='<%#Eval("Id") %>' ClientInstanceName="popupCheckingCode" OnWindowCallback="popupCheckingCode_WindowCallback">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">

                <dx:ASPxFormLayout ID="frmLayoutCheckingCode" runat="server" ColCount="1">
                    <Items>                       
                        <dx:LayoutItem Caption="Code">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxCheckBoxList ID="chkListCodes" 
                                        runat="server" RepeatColumns="4" TextField="Code" ValueField="Id" CheckBoxFocusedStyle-Wrap="False">
                                    </dx:ASPxCheckBoxList>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="" CaptionCellStyle-Paddings-PaddingTop="20">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton runat="server" ID="btnSaveCheckingCode" Text="Save" OnClick="btnSaveCheckingCode_Click"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupMoveStation" ClientInstanceName="popupMoveStation" runat="server" HeaderText="Move Process" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupMoveStation_OnWindowCallback" OnLoad="popupMoveStation_Load">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="frmLayoutMp" runat="server" ColCount="3">

                    <Items>
                        <dx:LayoutItem Caption="Move">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cboMoveAllorPos" runat="server" NullText="-Select">
                                        <Items>
                                            <dx:ListEditItem Text="-All Rf-" Value="0" />
                                            <dx:ListEditItem Text="-This Rf Only-" Value="1" />
                                        </Items>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="frmLayoutMp_E2" runat="server" Text="From">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="frmLayoutMp_E1" runat="server" Text="To">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Assembly Section">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="oldLabelAssemblySection" runat="server">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbAsemblySection" runat="server" ClientInstanceName="cmbAssemblySections" TextField="AssemblySectionName" ValueField="Id" ClientSideEvents-SelectedIndexChanged="OnAsemblySectionChanged">
                                        <ClientSideEvents SelectedIndexChanged="OnAsemblySectionChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Station">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="lblOriginalStation" runat="server">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cboNewStation" runat="server" TextField="StationName" ValueField="StationId"
                                         ClientInstanceName="cmbStations" OnCallback="cmbStations_Callback">
                                        <ClientSideEvents SelectedIndexChanged="OnStationChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Position">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbOrderingOptions" runat="server" TextField="Text" ValueField="Value" 
                                        ClientInstanceName="cmbOrderingOptions">
                                        <ClientSideEvents SelectedIndexChanged="OnOrderingOptionChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbStationProcesses" runat="server" TextField="ProcessDescription" ValueField="ProcessNo" 
                                        ClientInstanceName="cmbStationProcesses" ClientEnabled="false" OnCallback="cmbStationProcesses_Callback">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <dx:ASPxLabel runat="server" ID="lblErrorLog" Visible="False" />
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupToolAssign" ClientInstanceName="popupToolAssign" runat="server" HeaderText="Tool Assignment" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupToolAssign_WindowCallback">

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="frmToolAssign" runat="server" ColCount="2">

                    <Items>
                        <dx:LayoutItem Caption=" Production Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbProductionLines" runat="server" 
                                        ClientInstanceName="cmbProductionLines">
                                        <ClientSideEvents SelectedIndexChanged="OnProductionLineChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                       <dx:LayoutItem Caption="" ColSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                <dx:ASPxGridView runat="server" ID="gvToolAssign" ClientInstanceName="gvToolAssign" Visible="true" Width="600" EnableViewState="false"
                    OnCustomUnboundColumnData="gvToolAssign_CustomUnboundColumnData" OnCustomCallback="gvToolAssign_CustomCallback" KeyFieldName="Id">
                    <Columns>
                        <dx:GridViewDataTextColumn VisibleIndex="0" Width="30" Caption="No." FieldName="Number" UnboundType="String">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="CPProcessId" VisibleIndex="10" Visible="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="1" FieldName="Sequence" Name="Sequnce" CellStyle-HorizontalAlign="Center" Width="100px">
                            <DataItemTemplate>
                               <dx:ASPxTextBox ID="Sequence" HorizontalAlign="Center" RootStyle-VerticalAlign="Middle" runat="server" Width="50px" Text='<%# Bind("Sequence") %>'>
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Sequence" Name="Old Sequence" VisibleIndex="5" Visible="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="InventoryId" VisibleIndex="8" Visible="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="9" Visible="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2" Width="200px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="3" Width="200px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Class" VisibleIndex="4" CellStyle-HorizontalAlign="Center" Width="50px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="SetNM" VisibleIndex="5" CellStyle-HorizontalAlign="Center" Width="50px">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ColumnResizeMode="Control" />
                    <SettingsPager Mode="ShowAllRecords" NumericButtonCount="100">
                        <PageSizeItemSettings ShowAllItem="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings VerticalScrollableHeight="500" VerticalScrollBarMode="Visible" />
                </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btnAssignTool" runat="server" OnClick="btnAssignTool_Click" Text="Assign">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                     </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>

    </dx:ASPxPopupControl>

    <asp:SqlDataSource ID="sdsToolAssignment" runat="server" CancelSelectOnNullParameter="false"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT ti.Id, concat(t.Number, '-', ti.InventoryNumber) as Name, t.Description, tc.ClassName as Class,
                                t.MinNM, t.MaxNM, ti.SetNM, tap.Sequence
                       from [dbo].[ToolAssignmentStation] tas
                       inner join [dbo].[ToolInventories] ti on ti.Id = tas.ToolInventoryId 
                       inner join [dbo].[ToolClass] tc on tc.Id = ti.ClassId 
                       inner join [dbo].[Tools] t on t.Id = ti.ToolId           
                       left join dbo.ToolAssignmentProcess tap on tap.ToolInventoryId = tas.ToolInventoryId and tap.HasDeviation=1 and tap.CpProcessId=@CpProcessId 
                       WHERE tas.HasDeviation=1 and tas.ProdLineId=@ProdLineId and tas.StationId=@StationId
                                and ti.IsDeleted=0 and ti.Status=1 
                                and t.IsDeleted=0
                       order by isnull(tap.Sequence, 999), t.Number, ti.InventoryNumber">

         <SelectParameters>
            <asp:Parameter Name="CpProcessId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="ProdLineId" Type="Int32" DefaultValue="1" />
            <asp:Parameter Name="StationId" Type="Int32" DefaultValue="4" />
            <asp:Parameter Name="ToolClass" Type="String" DefaultValue="" ConvertEmptyStringToNull="true"/>
        </SelectParameters>
   </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsProductionLineLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, LineName from [dbo].[ProductionLines] pl 
                        WHERE pl.AssemblyTypeId=@AssemblyTypeId">

        <SelectParameters>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0"/>
        </SelectParameters>
    </asp:SqlDataSource>

<%--    <asp:SqlDataSource ID="sdsCheckingCode" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" 
    SelectCommand="SELECT a.Code as Id, a.Code+' '+a.Description as Code FROM Defects a
                    INNER JOIN DefectGroups b on a.DefectGroupId = b.Id
                    Where b.Name = 'Checklist CS2' "></asp:SqlDataSource>--%>

</asp:Content>