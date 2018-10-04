<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="STAViewSingle.aspx.cs" Inherits="Ebiz.WebForm.custom.STA.STAViewSingle" %>

<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function OnOrganization1Changed(cboOrganization1) {
            cmbApproved1.PerformCallback(cboOrganization1.GetSelectedItem().value.toString());
        }

        function OnOrganization2Changed(cboOrganization2) {
            cmbApproved2.PerformCallback(cboOrganization2.GetSelectedItem().value.toString());
        }

        function OnOrganization3Changed(cboOrganization3) {
            cmbApproved3.PerformCallback(cboOrganization3.GetSelectedItem().value.toString());
        }

        function OnOrganization4Changed(cboOrganization4) {
            cmbApproved4.PerformCallback(cboOrganization4.GetSelectedItem().value.toString());
        }

        function OnOrganization5Changed(cboOrganization5) {
            cmbApproved5.PerformCallback(cboOrganization5.GetSelectedItem().value.toString());
        }
    </script>

    <script type="text/javascript">
        //
        //This script is used to handle client side action of command buntton
        //
        function OnCustomButtonClick(s, e) {
            if (e.buttonID == "Copy") {
                var value = "Copy_" + e.visibleIndex;
                s.PerformCallback(value);
            }
            //if (e.buttonID == "btnDeviation") {
            //    var keyValue = grvCpPerStation.GetRowKey(e.visibleIndex);
            //    var cgisNo = grvCpPerStation.GetRowValues(e.visibleIndex, 'CgisNo');
            //    popupDeviation.Show();
            //    popupDeviation.SetHeaderText = 'Deviation - ' + cgisNo;
            //    popupDeviation.PerformCallback(keyValue + ';' + e.buttonID);
            //} else if (e.buttonID == "btnMove") {
            //    var keyValue = grvCpPerStation.GetRowKey(e.visibleIndex);
            //    var cgisNo = grvCpPerStation.GetRowValues(e.visibleIndex, 'CgisNo');
            //    popupMoveStation.Show();
            //    popupMoveStation.SetHeaderText = 'Move Process - ' + cgisNo;
            //    popupMoveStation.PerformCallback(keyValue + ';' + e.buttonID);
            //}
        }

    </script>

    <script type="text/javascript">
        //
        //This scripts are used for client side batch-editing using cascading combobox
        //

        //IMPORTANT: Not yet working properly

        var currentEditableVisibleIndex = -1;

        function OnBatchEditStartEditing(s, e) {
            currentEditableVisibleIndex = e.visibleIndex;

            var col = e.focusedColumn; //ASPxClientGridViewColumn
            var fieldName = col.fieldName;
            var name = col.name;

            var refFieldName = s.cpCascadingColumn[fieldName];
            if (refFieldName = undefined)
                return;

            //var currentCountryID = s.batchEditApi.GetCellValue(currentEditableVisibleIndex, "CountryID");
            //var currentCityID = s.batchEditApi.GetCellValue(currentEditableVisibleIndex, "CityID")
            //lastCityID = currentCityID;
            //if (lastCountryID == currentCountryID)
            //    CityID.SetSelectedItem(CityID.FindItemByValue(lastCityID));
            //else {
            //    if (currentCountryID == null) {
            //        CityID.SetSelectedIndex(-1);
            //        return;
            //    }
            //    lastCountryID = currentCountryID;
            //    PageMethods.GetCities(lastCountryID, CitiesCombo_OnSuccessGetCities);
            //}
        }

        function OnBatchEditEndEditing(s, e) {
            currentEditableVisibleIndex = -1;
        }

    </script>

    <script type="text/javascript">
        //
        //these scripts are used for dropdown list in filter panel
        //
        var textSeparator = "; ";
        var valueSeparator = ";";

        function OnListBoxSelectionChanged(name, cascade) {
            var checkComboBox = eval("dd" + name);
            var checkListBox = eval("chk" + name);
            var panel = null;
            if (cascade != null && cascade !== undefined)
                panel = eval("panel" + cascade);

            //start processing
            var selectedItems = checkListBox.GetSelectedItems();
            //update the dropdown text
            checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            //notify the callback panel for cascading edit
            if (panel != null)
                panel.PerformCallback(GetSelectedItemsValue(selectedItems));
            //store value in hidden field
            Hidden.Set(name, GetSelectedItemsValue(selectedItems));
            //Hidden[name] = GetSelectedItemsValue(selectedItems);
        }

        function SynchronizeListBoxValues(name, cascade) {
            var checkComboBox = eval("dd" + name);
            var checkListBox = eval("chk" + name);
            var panel = null;
            if (cascade != null && cascade !== undefined)
                panel = eval("panel" + cascade);

            //Assumption: user manually type into the dropdown text (or clear the text)
            //update the listbox selection. remove non-existing item in the list
            checkListBox.UnselectAll();
            var texts = checkComboBox.GetText().split(textSeparator);
            var values = GetValuesByTexts(checkListBox, texts); // for remove non-existing texts
            checkListBox.SelectValues(values);
            //reset the dropdown text
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            //notify the callback panel for cascading edit
            if (panel != null)
                panel.PerformCallback(GetSelectedItemsValue(selectedItems));
            //store value in hidden field
            Hidden.Set(name, GetSelectedItemsValue(selectedItems));
        }

        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                //if(items[i].index != 0)
                texts.push(items[i].text);
            return texts.join(textSeparator);
        }

        function GetSelectedItemsValue(items) {
            var values = [];
            for (var i = 0; i < items.length; i++)
                //if(items[i].index != 0)
                values.push(items[i].value);
            return values.join(valueSeparator);
        }

        function GetValuesByTexts(checkListBox, texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }

        function btnUpdate_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            callback.PerformCallback("update");
        }

        function callback_EndCallback(s, e) {
            if (s.cpAlert != null && s.cpAlert.length > 0)
                alert(s.cpAlert);
        }

        function btnApproved1_click(s, e) {
            callback.PerformCallback("approve1");
        }
        function btnApproved2_click(s, e) {
            callback.PerformCallback("approve2");
        }
        function btnApproved3_click(s, e) {
            callback.PerformCallback("approve3");
        }
        function btnApproved4_click(s, e) {
            callback.PerformCallback("approve4");
        }
        function btnApproved5_click(s, e) {
            callback.PerformCallback("approve5");
        }
        function Btn_Submit_click(s, e) {
            if (confirm('Are you sure want to submit this data?'))
                callback.PerformCallback("submit");
        }
    </script>

    <style type="text/css">
        .headerBorder {
            border-right: 2px solid black;
        }

        .staTextlabel {
            border: none;
        }

        .wrapText {
            word-break: break-all;
        }


        .printbtn {
            vertical-align: middle;
            padding: 4.5px 14px;
        }

        a.printbtn:hover {
            text-decoration: none;
        }
    </style>

    <div style="padding-bottom: 10px; padding-left: 0px" runat="server" id="divHeader">
        <table style="width: 100%;">
            <tr>
                <td style="width: 33.33%;">
                    <div>
                        <dx:ASPxLabel ID="ASPxLabel1" Style="text-align: left; text-size-adjust: 100%; font-weight: bold; font-size: large;"
                            runat="server" Text="View Alteration by RicNr">
                        </dx:ASPxLabel>
                        <dx:ASPxTextBox ID="lblIssuer" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px">
                        </dx:ASPxTextBox>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <dx:ASPxCallbackPanel ID="callback" runat="server" ClientInstanceName="callback" OnCallback="callback_Callback">
            <ClientSideEvents EndCallback="callback_EndCallback" />
            <PanelCollection>
                <dx:PanelContent>
                    <table border="1" id="HeaderTop" style="width: 100%">
                        <tr>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Ric Number">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="lbl_valueRicNumber" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Vehicle">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="lblModelName" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization1" DataSourceID="sdsOrganization1"
                                    TextField="Value" ValueField="Id" ValueType="System.Int32" SelectedIndex="0" ClientEnabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization1Changed(s); }"></ClientSideEvents>
                                </dx:ASPxComboBox>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="display: inline-flex;">
                                    <dx:ASPxComboBox runat="server" ID="cmbApprovalOne" Width="110" ClientInstanceName="cmbApproved1" OnCallback="cmbApprovalOne_Callback"
                                        DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxButton Text="Approve" ID="btnApproved1" Visible="false" AutoPostBack="false" runat="server" CssClass="blink" Style="margin-left: 10px;">
                                        <ClientSideEvents Click="btnApproved1_click" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                            <td style="width: 250px; text-align: center">
                                <div style="display: inline-flex">
                                    <dx:ASPxLabel runat="server" ID="lblApprove1"></dx:ASPxLabel>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Packing Month">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="lblHeaderPackingMonth" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Source">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="Source" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization2" DataSourceID="sdsOrganization2"
                                    TextField="Value" ValueField="Id" ValueType="System.Int32" SelectedIndex="0" ClientEnabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization2Changed(s); }"></ClientSideEvents>
                                </dx:ASPxComboBox>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="display: inline-flex">

                                    <dx:ASPxComboBox runat="server" ID="cmbApprovalTwo" Width="110" ClientInstanceName="cmbApproved2" OnCallback="cmbApprovalTwo_Callback"
                                        DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxButton Text="Approve" ID="btnApproved2" AutoPostBack="false" Visible="false" runat="server" CssClass="blink" Style="margin-left: 10px;">
                                        <ClientSideEvents Click="btnApproved2_click" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                            <td style="width: 250px; text-align: center">
                                <div style="display: inline-flex">
                                    <dx:ASPxLabel runat="server" ID="lblApprove2"></dx:ASPxLabel>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Prod. Month">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="lblHeaderProdMonth" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Reason">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="txtReasonOfAlteration" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization3" DataSourceID="sdsOrganization3"
                                    TextField="Value" ValueField="Id" ValueType="System.Int32" SelectedIndex="0" ClientEnabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization3Changed(s); }"></ClientSideEvents>
                                </dx:ASPxComboBox>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="display: inline-flex">

                                    <dx:ASPxComboBox runat="server" ID="cmbApprovalThree" Width="110" ClientInstanceName="cmbApproved3" OnCallback="cmbApprovalThree_Callback"
                                        DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxButton Text="Approve" ID="btnApproved3" AutoPostBack="false" Visible="false" runat="server" CssClass="blink" Style="margin-left: 10px;">
                                        <ClientSideEvents Click="btnApproved3_click" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                            <td style="width: 250px; text-align: center">
                                <div style="display: inline-flex">
                                    <dx:ASPxLabel runat="server" ID="lblApprove3"></dx:ASPxLabel>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Comnos">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="lbl_CommnosFrom" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Issued On">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="lblIssuedOn" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization4" DataSourceID="sdsOrganization4"
                                    TextField="Value" ValueField="Id" ValueType="System.Int32" SelectedIndex="0" ClientEnabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization4Changed(s); }"></ClientSideEvents>
                                </dx:ASPxComboBox>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="display: inline-flex">
                                    <dx:ASPxComboBox runat="server" ID="cmbApprovalFour" Width="110" ClientInstanceName="cmbApproved4" OnCallback="cmbApprovalFour_Callback"
                                        DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxButton Text="Approve" ID="btnApproved4" AutoPostBack="false" Visible="false" runat="server" CssClass="blink" Style="margin-left: 10px;">
                                        <ClientSideEvents Click="btnApproved4_click" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                            <td style="width: 250px; text-align: center">
                                <div style="display: inline-flex">
                                    <dx:ASPxLabel runat="server" ID="lblApprove4"></dx:ASPxLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="LOT No">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="LotNo" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Ric Status">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel29" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="display: inline-flex">
                                    <dx:ASPxComboBox runat="server" ID="cmbRicStatus" Width="110" ClientInstanceName="cmbRicStatus"
                                        DataSourceID="sdsRicStatus" TextField="StatusName" ValueField="Id">
                                    </dx:ASPxComboBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization5" DataSourceID="sdsOrganization5"
                                    TextField="Value" ValueField="Id" ValueType="System.Int32" SelectedIndex="0" ClientEnabled="false">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization5Changed(s); }"></ClientSideEvents>
                                </dx:ASPxComboBox>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel32" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="display: inline-flex">
                                    <dx:ASPxComboBox runat="server" ID="cmbApprovalFive" Width="110" ClientInstanceName="cmbApproved5" OnCallback="cmbApprovalFive_Callback"
                                        DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                            <RequiredField IsRequired="True" ErrorText="Required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxButton Text="Approve" ID="btnApproved5" AutoPostBack="false" Visible="false" runat="server" CssClass="blink" Style="margin-left: 10px;">
                                        <ClientSideEvents Click="btnApproved5_click" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                            <td style="width: 250px; text-align: center">
                                <div style="display: inline-flex">
                                    <dx:ASPxLabel runat="server" ID="lblApprove5"></dx:ASPxLabel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Chasis No">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel34" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center">
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="value_lblChassis" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="OPT. Code/Remarks">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel36" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxMemo ID="txtCodes" runat="server" Height="70px" Width="100%">
                                    </dx:ASPxMemo>
                                </div>
                            </td>
                            <td rowspan="1" style="width: 100px; text-align: center;">
                                <dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Status Header">
                                </dx:ASPxLabel>
                            </td>
                            <td style="text-align: center; width: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel38" runat="server" Text=":">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <div style="width: 100%;">
                                    <dx:ASPxTextBox ID="StatusHeader" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </div>
                                <br />
                                <dx:ASPxButton Text="Update" ID="btnUpdate" runat="server" AutoPostBack="false">
                                    <ClientSideEvents Click="btnUpdate_Click" />
                                </dx:ASPxButton>

                                <dx:ASPxButton runat="server" Style="text-align: center" ID="Btn_Submit" Text="Submit" AutoPostBack="false" ClientInstanceName="Btn_Submit">
                                    <ClientSideEvents Click="Btn_Submit_click" />
                                </dx:ASPxButton>
                                <td style="width: 220px; text-align: center">
                                    <div style="display: inline-flex">
                                        <dx:ASPxLabel runat="server" ID="ASPxLabel9" Visible="true"></dx:ASPxLabel>
                                    </div>
                                </td>
                        </tr>

                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
        <br />
        <table border="0" id="btn" style="width: 100%">
            <tr>

                <td style="width: 33.33%;">
                    <div style="align-content: center">
                        <div style="text-align: center; vertical-align: middle;">
                            <dx:ASPxHyperLink Text="Print Summary Alteration" ID="hlPrint" runat="server" Target="_blank" CssClass="dxbButton_DevEx printbtn" ForeColor="Black">
                            </dx:ASPxHyperLink>

                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <br />
    </div>

    <asp:SqlDataSource ID="sdsRICStatus" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, StatusName from RICStatus Order by StatusName"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsOrganization1" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, [Key], [Value] from Configs where [Value] = 'ACV' Order by [Value]"></asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sdsOrganization2" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, [Key], [Value] from Configs where [Value] = 'AGC' Order by [Value]"></asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sdsOrganization3" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
       SelectCommand="select Id, [Key], [Value] from Configs where [Value] = 'EIC' Order by [Value]"></asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sdsOrganization4" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, [Key], [Value] from Configs where [Value] = 'IT' Order by [Value]"></asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sdsOrganization5" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select Id, [Key], [Value] from Configs where [Value] = 'QA' Order by [Value]"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUser" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT u.Id as UserId, u.FirstName + ' ' + isnull(u.LastName, '') AS FullName FROM dbo.Users u
        left join dbo.UserOrganizations uo on uo.UserId = u.Id
        where (uo.OrganizationId = @organizationId or u.OrganizationId = @organizationId) ORDER BY FullName">
        <SelectParameters>
            <asp:Parameter Name="organizationId" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUsers" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id as UserId, FirstName + ' ' + LastName AS FullName FROM Users where
        LastName is not null ORDER BY FirstName"></asp:SqlDataSource>

</asp:Content>
