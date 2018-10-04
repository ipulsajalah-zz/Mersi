<%@ Page Title="" UnobtrusiveValidationMode="None" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="GWIS.aspx.cs" Inherits="Ebiz.WebForm.custom.GWIS.GWIS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Expires" content="0" />

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

            function imgGWIS_Click(sender, visibleIndex, buttonId) {
                var grid = eval(sender);
                grid.GetRowValues(visibleIndex, "Id", imgGWIS_OnGetRowValues);
            }

            function imgGWIS_OnGetRowValues(value) {
                popupGWIS.SetContentUrl("/custom/GWIS/DrawGWIS.aspx?ImageID=" + value);
                popupGWIS.SetHeaderText("Edit GWIS Image");
                popupGWIS.Show();
            }

            function buildProcessNo(grid, column, value)
            {
                //var editor2 = grid.GetEditor(1);
                //var editor3 = eval(grid.name + "ProcessNoEditor");
                
                var editor = grid.GetEditor("ProcessNo");
                if (editor == null)
                    return;

                var str = editor.GetValue();

                var arr = str.split('.');
                if (arr.length < 4)
                {
                    var arr2 = new String[4];
                    for(var i=0; i<arr.length; i++)
                    {
                        arr2[i] = arr[i]
                    }

                    arr = arr2;
                }

                if (column == 'AssemblySectionId')
                {
                    var editor2 = grid.GetEditor("AssemblySectionId");
                    str = editor2.GetText();

                    var arr2 = str.split("-");
                    var code = arr2[0].trim();

                    arr[0] = code;

                }
                else if (column == 'StationId')
                {
                    var editor2 = grid.GetEditor("StationId");
                    str = editor2.GetText();

                    var arr2 = str.split("-");
                    var code = arr2[0].trim();

                    arr[1] = code;

                }
                else if (column == 'ProcessNo_Position')
                {
                    var code = String.fromCharCode(value);
                    //var code = value.toString().substr(1, 1);

                    arr[2] = code;
                }
                else if (column == 'ProcessNo_Number')
                {
                    var code = pad(value, 4, "0");

                    arr[3] = code;
                }

                str = arr.join('.');
                editor.SetValue(str);
            }

            function pad(n, width, z) {
                z = z || '0';
                n = n + '';
                return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
            }

            function gvPartSelectionsCustomButtonClick(s, e) 
            {
                if (e.buttonID == 'btnAddPart')
                {
                    var keyValue = s.GetRowKey(e.visibleIndex);

                    txtEditorId.SetText(keyValue.toString());
                    txtEditorPage.SetText(cmbPage.GetText());
                    //s.GetRowValues(e.visibleIndex, 'PartNo', txtEditorPartNo_SetValue);
                    //s.GetRowValues(e.visibleIndex, 'Description', txtEditorDescription_SetValue);
                    //s.GetRowValues(e.visibleIndex, 'QtyRemain', txtEditorQtyRemaining_SetValue);
                    //s.GetRowValues(e.visibleIndex, 'Pos', txtEditorPos_SetValue);
                    //s.GetRowValues(e.visibleIndex, 'Pos2', txtEditorPos2_SetValue);

                    txtEditorPartNo.SetText(gvPartSelections.cpPartNo[e.visibleIndex - gvPartSelections.visibleStartIndex])
                    txtEditorDescription.SetText(gvPartSelections.cpDescription[e.visibleIndex - gvPartSelections.visibleStartIndex])
                    txtEditorQtyRemaining.SetText(gvPartSelections.cpQtyRemain[e.visibleIndex - gvPartSelections.visibleStartIndex])
                    txtEditorPos.SetText(gvPartSelections.cpPos[e.visibleIndex - gvPartSelections.visibleStartIndex])
                    txtEditorPosActual.SetText(gvPartSelections.cpPos[e.visibleIndex - gvPartSelections.visibleStartIndex])
                    txtEditorPos2.SetText(gvPartSelections.cpPos2[e.visibleIndex - gvPartSelections.visibleStartIndex])
                    txtEditorPos2Actual.SetText(gvPartSelections.cpPos2[e.visibleIndex - gvPartSelections.visibleStartIndex])

                    popupAddPart.Show();
                    //popupAddPart.PerformCallback(keyValue);
                }
            }

            function txtEditorPartNo_SetValue(value)
            {
                txtEditorPartNo.SetText(value);
            }

            function txtEditorDescription_SetValue(value)
            {
                txtEditorDescription.SetText(value);
            }

            function txtEditorQtyRemaining_SetValue(value)
            {
                txtEditorQtyRemaining.SetText(value);
            }

            function txtEditorPos_SetValue(value)
            {
                txtEditorPos.SetText(value);
                txtEditorPosActual.SetText(value);
            }

            function txtEditorPos2_SetValue(value)
            {
                txtEditorPos2.SetText(value);
                txtEditorPos2Actual.SetText(value);
            }

            function btnSavePart_Click(s, e)
            {
                //save
                popupAddPart.PerformCallback("save");
            }

            function btnCancelPart_Click(s, e)
            {
                popupAddPart.Hide();
            }

            function popupAddPart_EndCallback(s, e)
            {
                if (s.cpResult != null && s.cpResult.length > 0)
                {
                    if (s.cpResult == "OK") {
                        //refresh the grid
                        GWISProcessesGrid.PerformCallback("filter");
                        popupAddPart.Hide();
                    }
                    else
                        //popup the error
                        alert(s.cpResult);
                }                 
            }

        </script>

    </head>
    <body>

        <dx:ASPxPopupControl ID="popupGWIS" ClientInstanceName="popupGWIS" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="GWIS" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="90" MinHeight="90">
            <ContentStyle Paddings-Padding="0" />
            <ClientSideEvents Shown="onPopupShown" />
        </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupAddPart" ClientInstanceName="popupAddPart" runat="server" HeaderText="Add Part" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupAddPart_WindowCallback" ShowFooter="true">

        <ClientSideEvents EndCallback="popupAddPart_EndCallback" />

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                 <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" ColCount="2">

                    <Items>
                        <dx:LayoutItem Caption="Id">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorId" ClientInstanceName="txtEditorId" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Page">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorPage" ClientInstanceName="txtEditorPage" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="PartNo">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorPartNo" ClientInstanceName="txtEditorPartNo" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Description">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorDescription" ClientInstanceName="txtEditorDescription" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Remaining" FieldName="QtyRemain">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorQtyRemaining" ClientInstanceName="txtEditorQtyRemaining" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Quantity (Actual)" FieldName="QtyUsed">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorQtyActual" ClientInstanceName="txtEditorQtyActual" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Position" FieldName="Pos">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorPos" ClientInstanceName="txtEditorPos" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Position (Actual)" FieldName="PosActual">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorPosActual" ClientInstanceName="txtEditorPosActual" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Position2" FieldName="Pos2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorPos2" ClientInstanceName="txtEditorPos2" ReadOnly="true" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                         <dx:LayoutItem Caption="Position2 (Actual)" FieldName="Pos2Actual">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtEditorPos2Actual" ClientInstanceName="txtEditorPos2Actual" runat="server"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
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
                        <dx:ASPxButton runat="server" ID="btnSavePart" ClientInstanceName="btnSavePart" Text="Add" AutoPostBack="false">
                               <ClientSideEvents Click="btnSavePart_Click" />
                        </dx:ASPxButton>
                      </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                      <dx:ASPxButton runat="server" ID="btnCancelPart" ClientInstanceName="btnCancelPart" Text="Cancel" AutoPostBack="false">
                               <ClientSideEvents Click="btnCancelPart_Click" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </FooterContentTemplate>
   </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupPartSelection" ClientInstanceName="popupPartSelection" runat="server" HeaderText="Select Parts" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" MinHeight="200px" MinWidth="400px" AllowDragging="True" CloseAction="CloseButton" Modal="True" PopupAnimationType="Fade"
        OnWindowCallback="popupPartSelection_WindowCallback">

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="frmPartSelection" runat="server" ColCount="2">

                    <Items>
                        <dx:LayoutItem Caption="Assembly Catalog" ColSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbCatalogModel" runat="server" 
                                        ClientInstanceName="cmbCatalogModel" DataSourceID="sdsCatalogModelLookup" ValueField="Id" TextField="Model">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e) { cmbPage.PerformCallback(s.GetValue()); }"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Catalog Page">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="cmbPage" runat="server" 
                                        ClientInstanceName="cmbPage" DataSourceID="sdsPageLookup" ValueField="Page" TextField="Page" OnCallback="cmbPage_Callback">
                                        <ClientSideEvents ValueChanged="function(s,e) { gvPartSelections.PerformCallback(cmbCatalogModel.GetValue().toString() + ';' + s.GetValue().toString()); } " >

                                        </ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>                                        
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                       <dx:LayoutItem Caption="" ColSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                <dx:ASPxGridView runat="server" ID="gvPartSelections" ClientInstanceName="gvPartSelections" Visible="true" Width="900" EnableViewState="false"
                    OnCustomUnboundColumnData="gvPartSelections_CustomUnboundColumnData" OnCustomCallback="gvPartSelections_CustomCallback" KeyFieldName="Id" 
                    OnCustomJSProperties="gvPartSelections_CustomJSProperties">
                    <Columns>
                        <dx:GridViewCommandColumn ShowNewButton="false" ShowUpdateButton="false" ShowEditButton="false" ShowDeleteButton="false"> 
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="btnAddPart" Text="Add"></dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="PartNo" Name="PartNo" Caption="Part No" VisibleIndex="1" Visible="true" Width="180px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Description" Name="Description" Caption="Description" VisibleIndex="2" Width="200px" Visible="true">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="Pos" Name="Pos" Caption="Pos" VisibleIndex="3" Width="50px">
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="Pos2" Name="Pos2" Caption="Pos2" VisibleIndex="4" Width="50px">
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="Qty" Name="Qty" Caption="Total Qty" VisibleIndex="5" Width="80px">
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="QtyUsed" Name="QtyUsed" Caption="Used Qty" VisibleIndex="6" Width="80px">
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="QtyRemain" Name="QtyRemain" Caption="Remain Qty" VisibleIndex="7" UnboundType="Integer" Width="80px">
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                    <SettingsBehavior ColumnResizeMode="Control" />
                    <SettingsPager Mode="ShowAllRecords" NumericButtonCount="100">
                        <PageSizeItemSettings ShowAllItem="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings VerticalScrollableHeight="500" VerticalScrollBarMode="Visible" />
                    <ClientSideEvents 
                        CustomButtonClick="gvPartSelectionsCustomButtonClick">

                    </ClientSideEvents> 

                </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>

    </dx:ASPxPopupControl>

    <asp:SqlDataSource ID="sdsCatalogModelLookup" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" 
        SelectCommand="select Id, Model from dbo.CatalogModels where IsDeleted=0 ORDER BY Model"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsPageLookup" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" 
        SelectCommand="select distinct Page from dbo.AssyCatalogs where (CatalogModelId=@CatalogModelId) and IsDeleted=0 ORDER BY Page">

        <SelectParameters>
            <asp:Parameter Name="CatalogModelId" Direction="Input" Type="Int32" DefaultValue="0" />
        </SelectParameters>

    </asp:SqlDataSource>

<%--    <asp:SqlDataSource ID="sdsPartSelections" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommandType="StoredProcedure"
        SelectCommand="dbo.usp_GWIS_PartSelection">

        <SelectParameters>
            <asp:Parameter Name="CatalogModelId" Direction="Input" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="Page" Direction="Input" Type="String" DefaultValue="" />
            <asp:Parameter Name="PackingMonth" Direction="Input" Type="String" DefaultValue="" />
        </SelectParameters>

    </asp:SqlDataSource>--%>

    <asp:SqlDataSource ID="sdsPartSelections" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommandType="Text"
        SelectCommand="exec dbo.usp_GWIS_PartSelection @CatalogModelId=@CatalogModelId, @Page=@Page, @PackingMonth=NULL">

        <SelectParameters>
            <asp:Parameter Name="CatalogModelId" Direction="Input" Type="Int32" DefaultValue="2" />
            <asp:Parameter Name="Page" Direction="Input" Type="String" DefaultValue="03.443" />
            <asp:Parameter Name="PackingMonth" Direction="Input" Type="String" DefaultValue="" ConvertEmptyStringToNull="true" />
        </SelectParameters>

    </asp:SqlDataSource>

    </body>
    </html>
</asp:Content>
