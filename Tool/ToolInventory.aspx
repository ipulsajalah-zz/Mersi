<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ToolInventory.aspx.cs" Inherits="Ebiz.WebForm.Tids.CV.custom.Tool.ToolInventory" %>
<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //
        //This script is used to handle client side action of command buntton
        //

        //function OnCustomButtonClick(s, e) {
        //    if (e.buttonID == "Copy") {
        //        var value = "Copy_" + e.visibleIndex;
        //        s.PerformCallback(value);
        //    } else if (e.buttonID == "Col_ActionAssignStation") {
        //        var keyValue = masterGrid.GetRowKey(e.visibleIndex);
        //        //var cgisNo = masterGrid.GetRowValues(e.visibleIndex, 'Cgis No');
        //        //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');
        //        popupToolAssign.Show();
        //        popupToolAssign.SetHeaderText = 'Assign Tool to Station';
        //        popupToolAssign.PerformCallback("Tool;" + keyValue);
        //    } else if (e.buttonID == "Col_ActionReturn") {
        //        var keyValue = masterGrid.GetRowKey(e.visibleIndex);
        //        //var cgisNo = masterGrid.GetRowValues(e.visibleIndex, 'Cgis No');
        //        //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');
        //        popupToolReturn.Show();
        //        popupToolReturn.SetHeaderText = 'Return to Tool Room';
        //        popupToolReturn.PerformCallback("Return;" + keyValue);
        //    }
        //}

        function buildInventoryNumber(grid, column, value) {
            //var editor2 = grid.GetEditor(1);
            //var editor3 = eval(grid.name + "ProcessNoEditor");

            var editor = grid.GetEditor("Number");
            if (editor == null)
                return;

            var editor1 = grid.GetEditor("ToolId");
            var str1 = editor1.GetText();

            var editor2 = grid.GetEditor("RunningNumber");
            var str2 = editor2.GetText();

            var str = str1 + '-' + str2;
            editor.SetValue(str);

            if (column == 'ToolId')
            {
                //TODO: update the Calibrated checkbox based on value from table Tools (using ToolId)
                //grid.PerformCallback("Cb_Ext_;OnToolIdChanged;" + value);
            }
        }

        function ActionAssignStation(grid, visibleIndex, columnName) {
            //var grid = window[gridName];
            //if (typeof grid == 'undefined' || grid == null)
            //    return;

            var keyValue = grid.GetRowKey(visibleIndex);
            //var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
            //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

            popupToolAssign.Show();
            popupToolAssign.SetHeaderText = 'Assign Tool to Station';
            popupToolAssign.PerformCallback("Tool;" + keyValue);
        }

        function ActionReturn(grid, visibleIndex, columnName) {
            //var grid = window[gridName];
            //if (typeof grid == 'undefined' || grid == null)
            //    return;

            var keyValue = grid.GetRowKey(visibleIndex);
            //var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
            //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

            popupToolReturn.Show();
            popupToolReturn.SetHeaderText = 'Return to Tool Room';
            popupToolReturn.PerformCallback("Return;" + keyValue);
        }

        function OnAssemblyTypeChanged(s, e)
        {
            cmbProductionLines.PerformCallback(cmbAssemblyTypes.GetSelectedItem().value.toString())
        }

        function OnProductionLineChanged(s, e) {
            cmbAssemblySections.PerformCallback(cmbProductionLines.GetSelectedItem().value.toString())
        }

        function OnAssemblySectionChanged(s, e) {
            cmbStations.PerformCallback(cmbAssemblySections.GetSelectedItem().value.toString())
        }

        function btnAssignToolExit_Click(s, e) {
            popupToolAssign.Hide();
        }

        function btnReturnToolExit_Click(s, e) {
            popupToolReturn.Hide();
        }

    </script>

    <dx:ASPxPopupControl ID="popupToolAssign" ClientInstanceName="popupToolAssign" runat="server" HeaderText="Tool Assignment" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupToolAssign_WindowCallback" ShowFooter="true">

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="frmToolAssign" runat="server" ColCount="1">

                    <Items>
                        <dx:LayoutItem Caption="Assembly Type">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbAssemblyTypes" runat="server" TextField="Name" ValueField="Id"
                                        ClientInstanceName="cmbAssemblyTypes">
                                        <ClientSideEvents SelectedIndexChanged="OnAssemblyTypeChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

<%--                        <dx:LayoutItem Caption="Production Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Production Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbProductionLines" runat="server" TextField="LineName" ValueField="Id"
                                        ClientInstanceName="cmbProductionLines">
                                        <ClientSideEvents SelectedIndexChanged="OnProductionLineChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
<%--                       <dx:LayoutItem Caption="Assembly Section">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Assembly Section">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbAssemblySections" runat="server" TextField="SectionName" ValueField="Id"
                                        ClientInstanceName="cmbAssemblySections" OnCallback="cmbAssemblySections_Callback">
                                        <ClientSideEvents SelectedIndexChanged="OnAssemblySectionChanged"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
<%--                       <dx:LayoutItem Caption="Station Name">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Station Name">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbStations" runat="server" TextField="StationName" ValueField="Id"
                                        ClientInstanceName="cmbStations" OnCallback="cmbStations_Callback">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
<%--                        <dx:LayoutItem Caption="Set Value">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Set Value">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxSpinEdit ID="txtSetNM" ClientInstanceName="txtSetNM" runat="server" Width="170px">
                                    </dx:ASPxSpinEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
<%--                       <dx:LayoutItem Caption="GWIS Model">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxCheckBoxList ID="chkGWISTypes" runat="server" DataSourceID="sdsTypeLookup" TextField="Model" ValueField="Id" 
                                        RepeatColumns="4" RepeatDirection="Horizontal">
                                    </dx:ASPxCheckBoxList>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
<%--                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btnAssignTool" runat="server" OnClick="btnAssignTool_Click" Text="Assign" AutoPostBack="false">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                     </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <table>
                <tr>
                    <td style="width: 100%">
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnAssignTool" runat="server" OnClick="btnAssignTool_Click" Text="Assign Tool" AutoPostBack="false">
                        </dx:ASPxButton>
                    </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                        <dx:ASPxButton runat="server" ID="btnAssignToolExit" ClientInstanceName="btnAssignToolExit" Text="Close" AutoPostBack="false">
                               <ClientSideEvents Click="btnAssignToolExit_Click" />
                        </dx:ASPxButton>
                  </td>
                </tr>
            </table>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupToolReturn" ClientInstanceName="popupToolReturn" runat="server" HeaderText="Return to Tool Room" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupToolReturn_WindowCallback" ShowFooter="true">

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="fromToolReturn" runat="server" ColCount="2">

                    <Items>
                        <dx:LayoutItem Caption="Rack">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbRacks" runat="server" TextField="RackName" ValueField="Id" DataSourceID="sdsToolRackLookup"
                                        ClientInstanceName="cmbRacks">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                       <dx:LayoutItem Caption="Fault?">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxCheckBox ID="chkStatus" runat="server"></dx:ASPxCheckBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
<%--                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btnReturnTool" runat="server" OnClick="btnReturnTool_Click" Text="Return to Tool Room" AutoPostBack="false">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                     </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>

        <FooterContentTemplate>
            <table>
                <tr>
                    <td style="width: 100%">
                    </td>
                    <td>
                        <dx:ASPxButton ID="btnReturnTool" runat="server" OnClick="btnReturnTool_Click" Text="Return to Tool Room" AutoPostBack="false">
                        </dx:ASPxButton>
                    </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                        <dx:ASPxButton runat="server" ID="btnReturnToolExit" ClientInstanceName="btnReturnToolExit" Text="Close" AutoPostBack="false">
                               <ClientSideEvents Click="btnReturnToolExit_Click" />
                        </dx:ASPxButton>
                  </td>
                </tr>
            </table>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <asp:SqlDataSource ID="sdsAssemblyTypeLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, Name from [dbo].[AssemblyTypes] r">
    </asp:SqlDataSource>


    <asp:SqlDataSource ID="sdsProductionLineLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, LineName from [dbo].[ProductionLines] pl 
                        WHERE pl.AssemblyTypeId=@AssemblyTypeId">

        <SelectParameters>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0"/>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsToolRackLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, RackName from [dbo].[ToolRacks] r">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsStationLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select st.Id, st.StationName from [dbo].[Stations] st
                        WHERE st.AssemblySectionId=@AssemblySectionId">

        <SelectParameters>
            <asp:Parameter Name="AssemblySectionId" Type="Int32" DefaultValue="0"/>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsAssemblySectionLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select s.Id, s.AssemblySectionName as SectionName from [dbo].[AssemblySections] s
                        WHERE s.AssemblySectionType=@AssemblySectionType and s.AssemblyTypeId=@AssemblyTypeId">

        <SelectParameters>
            <asp:Parameter Name="AssemblySectionType" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0"/>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsTypeLookup" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" 
        SelectCommand="select Id,Model from CatalogModels ORDER BY Model">
    </asp:SqlDataSource>

</asp:Content>
    
