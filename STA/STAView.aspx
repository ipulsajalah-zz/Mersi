<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="STAViewSingle.aspx.cs" Inherits="Ebiz.WebForm.Tids.CV.Custom.STAViewSingle" %>

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
            for(var i = 0; i < items.length; i++)
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
            for(var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if(item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
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
                        <dx:ASPxLabel ID="ASPxLabel1" Style="text-align: left; text-size-adjust: 75%; font-weight: bold;"
                            runat="server" Text="Mercedes-Benz Indonesia">
                        </dx:ASPxLabel>
                    </div>
                </td>
                <td style="width: 25%;">
                    <div style="text-align: center; font-weight: bold; font-size: medium; vertical-align: text-top;">
                        IMPLEMENTATION CONTROL FOR ALTERATION
                        <dx:ASPxLabel ID="lblTop" runat="server" Style="text-align: center; font-weight: bold; font-size: medium; vertical-align: text-top;" Text="CV">
                        </dx:ASPxLabel>
                    </div>
                    <div style="text-align: center; font-weight: bold; font-size: medium; vertical-align: text-top;">
                        <dx:ASPxLabel ID="lblHeaderModelVariant" runat="server" Style="text-align: center; font-weight: bold; font-size: medium; vertical-align: text-top;" Text="">
                        </dx:ASPxLabel> -  
                        <dx:ASPxLabel ID="lblHeaderPackingMonth" runat="server" Style="text-align: center; font-weight: bold; font-size: medium; vertical-align: text-top;" Text="">
                        </dx:ASPxLabel>
                    </div>
                    <br />
                </td>
                <td style="width: 33.33%;">
                    <div style="float: right">
                        <div style="text-align: center; vertical-align: middle;">
                            <dx:ASPxHyperLink Text="Print RIC" ID="hlPrint" runat="server" Target="_blank" CssClass="dxbButton_DevEx printbtn" ForeColor="Black">
                            </dx:ASPxHyperLink>
                            <dx:ASPxButton Text="Update" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click">
                            </dx:ASPxButton>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table border="1" id="HeaderTop" style="width: 100%">
            <tr>
                <td rowspan="1">
                    <div style="display: inline-flex">
                        <dx:ASPxLabel ID="lbl_Issuer" Style="text-align: left;" runat="server" Text="Issuer :">
                        </dx:ASPxLabel>
                        <div>
                            <dx:ASPxTextBox ID="lblIssuer" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px">
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </td>
                <td rowspan="2" colspan="2" style="text-align: center" id="TitleMiddleHeader">IMPLEMENTATION CONTROL FOR ALTERATION
                    <dx:ASPxLabel ID="lblTop2" runat="server" Text="PC">
                    </dx:ASPxLabel>
                </td>
                <td colspan="2" style="text-align: center">
                    <dx:ASPxLabel ID="lbl_IssuedOn" runat="server" Text="Issued on :">
                    </dx:ASPxLabel>
                    <dx:ASPxTextBox ID="lblIssuedOn" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" HorizontalAlign="Center" Width="100%">
                    </dx:ASPxTextBox>
                    <div style="float: right">
                        <dx:ASPxButton runat="server" Style="text-align: center" ID="Btn_Submit" Text="Submit" OnClick="SubmitChanges">
                        </dx:ASPxButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="2">
                    <div style="display: inline-flex">
                        <dx:ASPxTextBox ID="lblModelName" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="30%">
                        </dx:ASPxTextBox>
                        <div style="display: inline-flex; float: right">
                            <dx:ASPxTextBox ID="lblbaumuster" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px">
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </td>
                <td colspan="2">
                    <div style="text-align: center;">
                        <div style="display: inline-flex">
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Status :">
                            </dx:ASPxLabel>
                            <dx:ASPxTextBox ID="lblStatus" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" HorizontalAlign="Center" Width="80px">
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 100px;">
                    <dx:ASPxLabel ID="lbl_TechAteration" Style="text-align: center;" runat="server" Text="Technical Alteration :">
                    </dx:ASPxLabel>
                </td>
                <td rowspan="1" style="text-align: center;" id="Title_headerMid">Reason Of Alteration
                </td>
                <td rowspan="1" colspan="2" style="text-align: center; width: 150px" id="TitleAppBy">APPROVED BY
                </td>
            </tr>
            <tr>
                <td rowspan="1" style="width: 275px;">
                    <dx:ASPxLabel ID="lbl_RicNumber" runat="server" Text="RIC NUMBER :">
                    </dx:ASPxLabel>
                </td>
                <td>
                    <div style="width: 100%;">
                        <dx:ASPxTextBox ID="txtReasonOfAlteration" runat="server" Width="100%">
                        </dx:ASPxTextBox>
                    </div>
                </td>
                <td rowspan="2" style="text-align: center; width: 150px">
                    <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization1" DataSourceID="sdsOrganization"
                        TextField="Name" ValueField="Id" ValueType="System.Int32">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization1Changed(s); }">
                        </ClientSideEvents>
                    </dx:ASPxComboBox>
                </td>
                <td rowspan="2" style="text-align: center; width: 100px">
                    <div style="display: inline-flex">
                        <dx:ASPxComboBox runat="server" ID="cmbApprovalOne" ClientInstanceName="cmbApproved1" OnCallback="cmbApprovalOne_Callback"
                            DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                        </dx:ASPxComboBox>
                        <dx:ASPxButton Text="Approve" ID="btnApproved1" Visible="false" runat="server" CssClass="blink" OnClick="ApprovedOne" Style="margin-left: 10px;">
                        </dx:ASPxButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxTextBox ID="lbl_valueRicNumber" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px">
                    </dx:ASPxTextBox>
                </td>
                <td>
                    <dx:ASPxTextBox ID="lblTechnicalAlteration" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="100%" HorizontalAlign="Center">
                    </dx:ASPxTextBox>
                </td>
                <td rowspan="2" style="text-align: center; width: 150px">Cumulative Figure: 
                    <div>
                        <dx:ASPxTextBox ID="txtCumulativeFigure" ReadOnly="false" runat="server" Width="100%">
                        </dx:ASPxTextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <div style="display: inline-flex">
                        <dx:ASPxLabel runat="server" ID="lbl_Commnos" Text="COMMNOS   :">
                        </dx:ASPxLabel>
                        <dx:ASPxLabel runat="server" ID="lblCommnosCountry" Text="831">
                        </dx:ASPxLabel>
                        <dx:ASPxTextBox ID="lbl_CommnosFrom" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="40px">
                        </dx:ASPxTextBox>
                        <dx:ASPxTextBox ID="lbl_ComnmnosTo" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="80px">
                        </dx:ASPxTextBox>
                        <dx:ASPxTextBox ID="lbl_Unit" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="80px">
                        </dx:ASPxTextBox>
                    </div>
                </td>
                <td colspan="1" rowspan="4">&nbsp;</td>
                <td rowspan="1" colspan="2" style="text-align: center; width: 150px">CONFIRMED BY
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dx:ASPxLabel runat="server" ID="lblPCorEngine">
                    </dx:ASPxLabel>
                    :
                    <dx:ASPxLabel runat="server" ID="lblNumPCorEngineMin">
                    </dx:ASPxLabel>
                    -
                    <dx:ASPxLabel runat="server" ID="lblNumPCorEngineMax">
                    </dx:ASPxLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: inline-flex">
                        <dx:ASPxLabel runat="server" ID="lbl_PackingMonth" Text="PACKING MONTH   :">
                        </dx:ASPxLabel>
                        <dx:ASPxTextBox ID="lblPackingMonth" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="50px">
                        </dx:ASPxTextBox>
                    </div>
                </td>
                <td colspan="1" rowspan="2" style="text-align: center; width: 150px">Impl. Date MBIna  :
                    <div>
                        <dx:ASPxDateEdit ID="txtImpleDate" HorizontalAlign="Center" Width="100%" runat="server">
                        </dx:ASPxDateEdit>
                    </div>
                </td>
                <td rowspan="2" style="text-align: center; width: 100px">
                    <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization2" DataSourceID="sdsOrganization"
                        TextField="Name" ValueField="Id" ValueType="System.Int32">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization2Changed(s); }">
                        </ClientSideEvents>
                    </dx:ASPxComboBox>
                </td>
                <td rowspan="2" style="text-align: center; width: 150px">
                    <div style="display: inline-flex">
                        <dx:ASPxComboBox runat="server" ID="cmbApprovalTwo" ClientInstanceName="cmbApproved2" OnCallback="cmbApprovalTwo_Callback"
                            DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                        </dx:ASPxComboBox>
                        <dx:ASPxButton Text="Approve" ID="btnApproved2" Visible="false" runat="server" CssClass="blink" OnClick="ApprovedTwo" Style="margin-left: 10px;">
                        </dx:ASPxButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: inline-flex">
                        <dx:ASPxLabel runat="server" ID="lbl_ChassisNR" Text="CHASSIS NR :">
                        </dx:ASPxLabel>
                        <dx:ASPxTextBox ID="value_lblChassis" ReadOnly="true" runat="server" CssClass="staTextlabel" Paddings-Padding="0px" Width="280px">
                        </dx:ASPxTextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="2">
                    <dx:ASPxLabel Text="Remarks :" runat="server" ID="lbl_Remarks">
                    </dx:ASPxLabel>
                    <div style="float: right">
                        <dx:ASPxMemo ID="txtRemarks" runat="server" Height="70px" Width="400px">
                        </dx:ASPxMemo>
                    </div>
                </td>
                <td colspan="2" rowspan="2" style="vertical-align: bottom">
                    <dx:ASPxMemo ID="txtCodes" runat="server" Height="70px" Width="100%">
                    </dx:ASPxMemo>
                </td>
                <td style="text-align: center; width: 100px">
                    <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization3" DataSourceID="sdsOrganization"
                        TextField="Name" ValueField="Id" ValueType="System.Int32">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization3Changed(s); }">
                        </ClientSideEvents>
                    </dx:ASPxComboBox>
                </td>
                <td style="text-align: center; width: 150px">
                    <div style="display: inline-flex">
                        <dx:ASPxComboBox runat="server" ID="cmbApprovalThree" ClientInstanceName="cmbApproved3" OnCallback="cmbApprovalThree_Callback"
                            DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                        </dx:ASPxComboBox>
                        <dx:ASPxButton Text="Approve" ID="btnApproved3" Visible="false" runat="server" CssClass="blink" OnClick="ApprovedThree" Style="margin-left: 10px;">
                        </dx:ASPxButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100px">
                    <dx:ASPxComboBox runat="server" HorizontalAlign="Center" Width="150" ID="cmbOrganization4" DataSourceID="sdsOrganization"
                        TextField="Name" ValueField="Id" ValueType="System.Int32">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOrganization4Changed(s); }">
                        </ClientSideEvents>
                    </dx:ASPxComboBox>
                </td>
                <td style="text-align: center; width: 150px">
                    <div style="display: inline-flex">
                        <dx:ASPxComboBox runat="server" ID="cmbApprovalFour" ClientInstanceName="cmbApproved4" OnCallback="cmbApprovalFour_Callback"
                            DataSourceID="sdsUser" TextField="FullName" ValueField="UserId">
                        </dx:ASPxComboBox>
                        <dx:ASPxButton Text="Approve" ID="btnApproved4" Visible="false" runat="server" CssClass="blink" OnClick="ApprovedFour" Style="margin-left: 10px;">
                        </dx:ASPxButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <div style="display: inline-flex">
                        <span style="width: 100%">OLD UNTIL&nbsp;</span>
                        <dx:ASPxTextBox ID="txtOldUntil" runat="server" Width="50px">
                        </dx:ASPxTextBox>
                    </div>
                </td>
                <td colspan="2" style="width: 300px;">
                    <div style="display: inline-flex">
                        <span style="width: 100%">NEW FROM</span>
                        <dx:ASPxTextBox ID="lblNewFrom" runat="server" Width="50px">
                        </dx:ASPxTextBox>
                    </div>
                </td>
                <td colspan="2"></td>
            </tr>
        </table>
    </div>

    <asp:SqlDataSource ID="sdsRICStatus" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Name from RICStatus Order by Name">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsOrganization" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Name from Organizations Order by Name">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsUser" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id as UserId,  FirstName + ' ' + LastName AS FullName FROM Users 
        where (OrganizationId = @organizationId) and 
        LastName is not null ORDER BY FullName">
        <SelectParameters>
            <asp:Parameter Name="organizationId" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsUsers" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id as UserId, FirstName + ' ' + LastName AS FullName FROM Users where
        LastName is not null ORDER BY FirstName"></asp:SqlDataSource>

</asp:Content>