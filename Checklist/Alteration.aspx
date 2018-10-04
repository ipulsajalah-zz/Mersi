<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Alteration.aspx.cs" Inherits="Ebiz.WebForm.custom.Checklist.Alteration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">Alteration Checklist
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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
        //    }
        //}

        //function ActionDeviation(gridName, visibleIndex, columnName) {
        //    var grid = window[gridName];
        //    if (typeof grid == 'undefined' || grid == null)
        //        return;

        //    var keyValue = grid.GetRowKey(visibleIndex);
        //    var cgisNo = grid.GetRowValues(visibleIndex, 'CgisNo');
        //    //var StationId = masterGrid.GetRowValues(e.visibleIndex, 'Station Id');

        //    popupDeviation.Show();
        //    popupDeviation.SetHeaderText = 'Deviation - ' + cgisNo;
        //    popupDeviation.PerformCallback(keyValue + ';' + e.buttonID);
        //}

        function lnkUploadPL_Click(s, e)
        {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            //var status = hdField.Get("status");

            if (hdField.Get("status") == 1)
            {
                alert("RIC is already released.");
                return;
            }

            fileUploadControl.ClearText();
            fileUploadControl.UpdateErrorMessageCell(0, '', true);
            lblSuccess.SetText('');

            popupDocUpload.Show();
        }

        function lnkVerifyPL_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            //TODO
            //window.open("/PackingLists/list", "popup", 'width=600,height=600,scrollbars=no,resizable=no');

            //$('#popupModal').on('show', function () {
            //    $('iframe').attr("src", "/PackingLists/list");
            //});
            //$('#popupModal').modal({ show: true });

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var str = dt.toISOString();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            popupPackinList.SetContentUrl("/custom/STA/PackingListPopup.aspx?modelid=" + modelid + "&vpm=" + vpm + "&rictype=" + rictype);
            popupPackinList.SetHeaderText("Packing Lists");
            popupPackinList.Show();
        }

        function lnkComparePL_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            if (hdField.Get("status") == 1) {
                alert("RIC is already released.");
                return;
            }

            popupComparePL.PerformCallback();
            popupComparePL.Show();
        }

        function lnkVerifyDeviation_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            //TODO
            //window.open("/STAs/list", "popup", 'width=100%,height=600,scrollbars=no,resizable=no');
            popupAlteration.SetContentUrl("/custom/STA/STADeviationPopup.aspx?modelid=" + modelid + "&vpm=" + vpm + "&rictype=" + rictype);
            popupAlteration.SetHeaderText("Deviations");
            popupAlteration.Show();
        }

        function lnkVerifyAlt_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            //for get id from url
            //var urlparams = new URLSearchParams(window.location.search);
            //var HeaderId = urlparams.get('id');

            //var url_string = window.location.href;
            //var url = new URL(url_string);
            var HeaderId = getUrlParameter("id");
           // console.log(c);

            //TODO
            //window.open("/STAs/list", "popup", 'width=100%,height=600,scrollbars=no,resizable=no');
            popupAlteration.SetContentUrl("/custom/STA/STAPopup.aspx?id=" + HeaderId + "&modelid=" + modelid + "&vpm=" + vpm + "&rictype=" + rictype);
            popupAlteration.SetHeaderText("Alterations");
            popupAlteration.Show();
        }
        

        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };
        function lnkVerifyRic_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            //TODO
            //window.open("/custom/STA/STAViewSingle.aspx", "popup", 'width=600,height=600,scrollbars=no,resizable=no');
            popupRIC.SetContentUrl("/custom/STA/STAViewPopup.aspx?modelid=" + modelid + "&vpm=" + vpm + "&rictype=" + rictype);
            popupRIC.SetHeaderText("Summary Alterations");
            popupRIC.Show();
        }
        
        function lnkReleaseRic_Click(s, e) {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

            if (hdField.Get("status") == 1) {
                alert("RIC is already released.");
                return;
            }

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();
           
            var message = 'Do you want to release RIC for Model: ' + cmbModels.GetText() + ', Packing Month: ' + vpm + ', Type: ' + cmbRicType.GetText() + '?';
            if (confirm(message))
                callback.PerformCallback("release;" + modelid + ";" + vpm + ";" + rictype)
        } 

        function btnSave_Click(s, e) {
            if (btnSave.GetText() == "Upload")
                doUpload();

            else if (btnSave.GetText() == "Cancel")
                cancelUpload();
        }

        function doUpload() {
            if (fileUploadControl.GetText() != "") {
                //lblCompleteMessage.SetVisible(false);
                //pbUpload.SetPosition(0);
                btnExit.SetEnabled(false);
                fileUploadControl.Upload();
                //btnUpload.SetEnabled(false);
                //pnlProgress.SetVisible(true);
                btnSave.SetText("Cancel");
            }
        }

        function cancelUpload() {
            fileUploadControl.Cancel();
            //btnUpload.SetEnabled(true);
            //pnlProgress.SetVisible(false);
            btnSave.SetText("Upload");
            btnExit.SetEnabled(true);
        }

        function fileUploadControl_UploadComplete(s, e)
        {
            btnSave.SetText("Upload");
            btnExit.SetEnabled(true);
            lblSuccess.SetText(e);

            //call function to process the file
            popupDocUpload.PerformCallback("upload");
        }

        function fileUploadControl_TextChanged(s, e)
        {
            lblSuccess.SetText("");
        }

        function btnExit_Click(s, e)
        {
            popupDocUpload.Hide();
        }

        function btnCompare_Click(s, e) {
            //call function to process the file
            popupComparePL.PerformCallback("compare");
        }

        function btnExitCompare_Click(s, e) {
            popupComparePL.Hide();
        }


        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth - 40;
            if (s.GetWidth() != windowInnerWidth) {
                s.SetWidth(windowInnerWidth);
                s.UpdatePosition();
            }

            var windowInnerHeight = window.innerHeight - 40;
            if (s.GetHeight() != windowInnerHeight) {
                s.SetHeight(windowInnerHeight);
                s.UpdatePosition();
            }

        }

        function callback_EndCallback(s, e)
        {
            if (s.cpResult != null && s.cpResult.length > 0)
                alert(s.cpResult);
        }

        //function btnExport_Click(s, e) {
        //    popupExport.PerformCallback("export");
        //}

        ////popupUploadDoc
        ///*----Upload Part by PopUp----*/
        //function fileUploadControl_OnFileUploadComplete(args) {

        //    if (args.isValid && args.callbackData != "") {

        //        src = args.callbackData;
        //        arrSplit = src.split("|");
        //        lblError.SetText(args.callbackData);

        //        //if ($("span:contains(Fail to parse)")) {
        //        //    $("[id$='btnClose']").remove();
        //        //}
        //        var a = $("[id$='btnClose']");
        //        var b = $("[id$='btnExit']");

        //        $("span:contains('Fail to bulk copy to server')").css("color", "Red");
        //        $("span:contains('Invalid file extension')").css("color", "Red");
        //        $("span:contains('Successfully upload')").css("color", "Green");
        //        $("span:contains('Fail to parse')").css("color", "Red");
        //        $("span:contains('Model and variant not found for')").css("color", "Green");
        //        if ($("span:contains('Fail to bulk copy to server')").length > 0) {
        //            a.detach();
        //        }
        //        if ($("span:contains('Invalid file extension')").length > 0) {
        //            a.detach();
        //        }
        //        if ($("span:contains('Fail to parse')").length > 0) {
        //            a.detach();
        //        }
        //        if ($("span:contains('Successfully upload')").length > 0) {
        //            b.detach();
        //        }
        //        if (args.isValid && args.errorText == "") {
        //            DisabledButton(true);

        //        }
        //    }
        //}

        function DisabledButton(flag) {
            //btnShowData.SetVisible(false);  //todo: cari tau cara get CommandArgument
            //btnClose.SetVisible(flag);
            //btnExit.SetVisible(flag);
        }
        
        function cmbModels_SelectedIndexChanged(s, e)
        {
            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            if (modelid == null || modelid == 0 || vpm == null || vpm == 0 || rictype == null || rictype == 0)
                return;

            callback.PerformCallback("refresh;" + modelid + ";" + vpm + ";" + rictype)
        }

        function dtPackingMonth_ValueChanged(s, e) {
            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            if (modelid == null || modelid == 0 || vpm == null || vpm == 0 || rictype == null || rictype == 0)
                return;

            callback.PerformCallback("refresh;" + modelid + ";" + vpm + ";" + rictype)
        }

        function cmbRicType_SelectedIndexChanged(s, e) {
            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            if (modelid == null || modelid == 0 || vpm == null || vpm == 0 || rictype == null || rictype == 0)
                return;

            callback.PerformCallback("refresh;" + modelid + ";" + vpm + ";" + rictype)
        }

        function popupDocUpload_EndCallback(s, e)
        {
            if (s.cpResult == null || s.cpResult != '1')
                return;

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            if (modelid == null || modelid == 0 || vpm == null || vpm == 0 || rictype == null || rictype == 0)
                return;

            callback.PerformCallback("refresh;" + modelid + ";" + vpm + ";" + rictype)
        }

        function popupComparePL_EndCallback(s, e)
        {
            if (s.cpResult == null || s.cpResult != '1')
                return;

            var modelid = cmbModels.GetValue();

            var dt = dtPackingMonth.GetDate();
            if (dt == null)
                dt = new Date();
            //var vpm = dt.toISOString().slice(0, 7).replace(/-/g, "");
            var vpm = (dt.getFullYear() * 100) + dt.getMonth() + 1;

            var rictype = cmbRicType.GetValue();

            if (modelid == null || modelid == 0 || vpm == null || vpm == 0 || rictype == null || rictype == 0)
                return;

            callback.PerformCallback("refresh;" + modelid + ";" + vpm + ";" + rictype)
        }

    </script>

    <h2 class="grid-header">Alteration Checklists</h2>

    <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
    <dx:ASPxFormLayout ID="frmLayout" runat="server" ColCount="1">
        <Items>
            <dx:LayoutItem Caption="Model">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxComboBox ID="cmbModels" runat="server" NullText="Model" TextField="Model" ValueField="Id" DataSourceID="sdsModelLookup"
                            ClientInstanceName="cmbModels">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>      
                            <ClientSideEvents SelectedIndexChanged="cmbModels_SelectedIndexChanged" />                                  
                        </dx:ASPxComboBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Packing Month">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxDateEdit ID="dtPackingMonth" ClientInstanceName="dtPackingMonth" Visible="true" runat="server" NullText="Packing Month" DisplayFormatString="yyyyMM">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                            <ClientSideEvents ValueChanged="dtPackingMonth_ValueChanged" />                                  
                        </dx:ASPxDateEdit>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Packing List Type">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxComboBox ID="cmbRicType" runat="server" NullText="PL Type" TextField="RicType" ValueField="Id" DataSourceID="sdsRicTypeLookup"
                            ClientInstanceName="cmbRicType">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                            <ClientSideEvents SelectedIndexChanged="cmbRicType_SelectedIndexChanged" />                                  
                        </dx:ASPxComboBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
        </Items>
    </dx:ASPxFormLayout>

    </div>

    <dx:ASPxCallbackPanel ID="callback" ClientInstanceName="callback" runat="server" OnCallback="callback_Callback">
        <ClientSideEvents EndCallback="callback_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxHiddenField ID="hdField" ClientInstanceName="hdField" runat="server" ></dx:ASPxHiddenField>
                <table border="0">
                    <tr style="padding-bottom: 20px;">
                        <td style="width: 50px; text-align:center;">
                            <dx:ASPxImage ID="imgUploadPL" ClientInstanceName="imgUploadPL" runat="server" Height="20px" Width="20px" ImageUrl="/Content/Images/status-none.png"></dx:ASPxImage>
                        </td>
                        <td>
                            <div style="width: 100%; overflow: hidden; font-size: 15px; ">
                                <dx:ASPxHyperLink ID="lnkUploadPL" runat="server" Text="Upload Packing List" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkUploadPL_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td>
                            <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
                            <dx:ASPxLabel ID="lblPL" ClientInstanceName="lblPL" runat="server"></dx:ASPxLabel>
                            </div>
                        </td>
                    </tr>
                    <tr style="padding-bottom: 20px;">
                        <td></td>
                        <td>
                            <div style="width: 100%; overflow: hidden; font-size: 15px; padding-bottom: 20px;">
                                <dx:ASPxHyperLink ID="lnkVerifyPL" runat="server" Text="Check Packing List" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkVerifyPL_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr style="padding-bottom: 20px;">
                        <td style="width: 50px; text-align:center;">
                            <dx:ASPxImage ID="imgComparePL" ClientInstanceName="imgComparePL" runat="server" Height="20px" Width="20px" ImageUrl="/Content/Images/status-none.png"></dx:ASPxImage>
                        </td>
                        <td>
                            <div style="width: 100%; overflow: hidden; font-size: 15px; ">
                                <dx:ASPxHyperLink ID="lnkComparePL" runat="server" Text="Compare Packing List" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkComparePL_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td>
                            <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
                            <dx:ASPxLabel ID="lblComparePL" ClientInstanceName="lblComparePL" runat="server"></dx:ASPxLabel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
                                <dx:ASPxHyperLink ID="lnkVerifyDeviation" runat="server" Text="Check Deviation" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkVerifyDeviation_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
                                <dx:ASPxHyperLink ID="lnkVerifyAlt" runat="server" Text="Check Alteration" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkVerifyAlt_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
                                <dx:ASPxHyperLink ID="lnkVerifyRic" runat="server" Text="Check RIC" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkVerifyRic_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr style="padding-bottom: 20px;">
                        <td style="width: 50px; text-align:center;">
                            <dx:ASPxImage ID="imgReleaseRic" ClientInstanceName="imgReleaseRic" runat="server" Height="20px" Width="20px" ImageUrl="/Content/Images/status-none.png"></dx:ASPxImage>
                        </td>
                        <td>
                            <div style="width: 100%; overflow: hidden; font-size: 15px; ">
                                <dx:ASPxHyperLink ID="lnkReleaseRic" runat="server" Text="Release RIC" Cursor="pointer" Font-Size="15px" >
                                    <ClientSideEvents Click="lnkReleaseRic_Click" />
                                </dx:ASPxHyperLink>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td>
                            <div style="width: 100%; overflow: hidden; padding-bottom: 20px; font-size: 15px; ">
                            <dx:ASPxLabel ID="lblReleaseRIC" ClientInstanceName="lblReleaseRIC" runat="server"></dx:ASPxLabel>
                                </div>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:ASPxPopupControl ID="popupDocUpload" ClientInstanceName="popupDocUpload" runat="server" AllowDragging="True" AutoUpdatePosition="True"
        CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="Upload Packing List" Modal="True"
        PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="400" MinHeight="200"
        OnWindowCallback="popupDocUpload_OnWindowCallback" ShowFooter="true">
        <ClientSideEvents EndCallback="popupDocUpload_EndCallback">
        </ClientSideEvents>

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="width: 100%; overflow: hidden;">
<%--                    <div>
                        <dx:ASPxComboBox ID="cmbModels_PL" runat="server" Caption="Model" NullText="Model" TextField="Model" ValueField="Id" DataSourceID="sdsModelLookup"
                            ClientInstanceName="cmbModels_PL" ReadOnly="true">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>
                     </div>
                    <div>
                        <dx:ASPxDateEdit ID="dtPM" Visible="true" runat="server" Caption="Packing Month" NullText="Packing Month" DisplayFormatString="yyyyMM">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxDateEdit>
                    </div>
                    <div>
                        <dx:ASPxComboBox ID="cmbRicType" runat="server" Caption="Packing List Type" NullText="PL Type" TextField="RicType" ValueField="Id" DataSourceID="sdsRicTypeLookup"
                            ClientInstanceName="cmbRicType">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>
                    </div>--%>
                   <dx:ASPxUploadControl ID="fileUploadControl" runat="server" UploadMode="Standard" Width="280px" ShowProgressPanel="True"
                        ClientInstanceName="fileUploadControl" NullText="Click here to browse files..." OnFileUploadComplete="fileUploadControl_OnFileUploadComplete" ShowUploadButton="False">
                        <ClientSideEvents FileUploadComplete="fileUploadControl_UploadComplete"
                            Init="function(s,e){ DisabledButton(false); }" TextChanged="fileUploadControl_TextChanged"
                         />
                    </dx:ASPxUploadControl>
                </div>
                <div>
                    <dx:ASPxLabel ID="lblSuccess" ClientInstanceName="lblSuccess" Text="" runat="server"></dx:ASPxLabel>
                </div>
                <div style="padding-top: 20px;">
<%--                    <dx:ASPxLabel ID="lblTemp" runat="server" ClientInstanceName="lblTemp">
                    </dx:ASPxLabel>--%>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <table>
                <tr>
                    <td style="width: 100%">
                    </td>
                    <td>
                        <dx:ASPxButton runat="server" ID="btnSave" ClientInstanceName="btnSave" Text="Upload" AutoPostBack="false">
                               <ClientSideEvents Click="btnSave_Click" />
                        </dx:ASPxButton>
                      </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                      <dx:ASPxButton runat="server" ID="btnExit" ClientInstanceName="btnExit" Text="Close" AutoPostBack="false">
                               <ClientSideEvents Click="btnExit_Click" />
                        </dx:ASPxButton>
                  </td>
                </tr>
            </table>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupComparePL" ClientInstanceName="popupComparePL" runat="server" AllowDragging="True" AutoUpdatePosition="True"
        CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="Compare Packing List" Modal="True"
        PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="400" MinHeight="200"
        OnWindowCallback="popupComparePL_WindowCallback" ShowFooter="true">
        <ClientSideEvents EndCallback="popupComparePL_EndCallback">
        </ClientSideEvents>

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="width: 100%; overflow: hidden;">
<%--                    <div>
                        <dx:ASPxTextBox ID="txtModelId_Compare" ClientInstanceName="txtModelId_Compare" Visible ="false" runat="server" Caption="Model" ReadOnly="true">
                           <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxTextBox>
                     </div>--%>
                    <div>
                        <dx:ASPxTextBox ID="txtActPM" ClientInstanceName="txtActPM" Visible ="true" runat="server" Caption="Packing Month" ReadOnly="true">
                           <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxTextBox>
                    </div>
                    <div>
                        <dx:ASPxComboBox ID="cmbPrevPM" runat="server" Caption="Prev. Packing Month" NullText="Prev. Packing Month" TextField="PackingMonth" ValueField="PackingMonth"
                            ClientInstanceName="cmbPrevPM">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>
                    </div>
<%--                    <div>
                        <dx:ASPxComboBox ID="cmbRicType_Compare" runat="server" Caption="Packing List Type" NullText="PL Type" TextField="RicType" ValueField="Id" DataSourceID="sdsRicTypeLookup"
                            ClientInstanceName="cmbRicType_Compare">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>
                    </div>--%>
                </div>
                <div>
                    <dx:ASPxLabel ID="lblSuccess_Compare" ClientInstanceName="lblSuccess_Compare" Text="" runat="server"></dx:ASPxLabel>
                </div>
<%--                <div style="padding-top: 20px;">
                </div>--%>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <table>
                <tr>
                    <td style="width: 100%">
                    </td>
                    <td>
                        <dx:ASPxButton runat="server" ID="btnCompare" ClientInstanceName="btnCompare" Text="Compare" AutoPostBack="false">
                               <ClientSideEvents Click="btnCompare_Click" />
                        </dx:ASPxButton>
                      </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                      <dx:ASPxButton runat="server" ID="btnExitCompare" ClientInstanceName="btnExitCompare" Text="Close" AutoPostBack="false">
                               <ClientSideEvents Click="btnExitCompare_Click" />
                        </dx:ASPxButton>
                  </td>
                </tr>
            </table>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupPackinList" ClientInstanceName="popupPackinList" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton" 
        EnableViewState="False" PopupElementID="popupArea" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowOnPageLoad="false" 
        FooterText="Try to resize the control using the resize grip or the control's edges"
        HeaderText="Feedback form" EnableHierarchyRecreation="True" FooterStyle-Wrap="True" Modal="true">
        <ContentStyle Paddings-Padding="0" />
        <ClientSideEvents Shown="onPopupShown"/>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupAlteration" ClientInstanceName="popupAlteration" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton" 
        EnableViewState="False" PopupElementID="popupArea" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowOnPageLoad="false" 
        FooterText="Try to resize the control using the resize grip or the control's edges"
        HeaderText="Feedback form" EnableHierarchyRecreation="True" FooterStyle-Wrap="True" Modal="true">
        <ContentStyle Paddings-Padding="0" />
        <ClientSideEvents Shown="onPopupShown"/>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupRIC" ClientInstanceName="popupRIC" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton" 
        EnableViewState="False" PopupElementID="popupArea" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowOnPageLoad="false" 
        FooterText="Try to resize the control using the resize grip or the control's edges"
        HeaderText="Feedback form" EnableHierarchyRecreation="True" FooterStyle-Wrap="True" Modal="true">
        <ContentStyle Paddings-Padding="0" />
        <ClientSideEvents Shown="onPopupShown"/>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupDeviation" ClientInstanceName="popupDeviation" runat="server" AllowDragging="True" AllowResize="True"
        CloseAction="CloseButton" 
        EnableViewState="False" PopupElementID="popupArea" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowOnPageLoad="false" 
        FooterText="Try to resize the control using the resize grip or the control's edges"
        HeaderText="Feedback form" EnableHierarchyRecreation="True" FooterStyle-Wrap="True" Modal="true">
        <ContentStyle Paddings-Padding="0" />
        <ClientSideEvents Shown="onPopupShown"/>
    </dx:ASPxPopupControl>


    <div id="popupModal" class="modal hide fade" tabindex="-1" role="dialog">
	    <div class="modal-header">
		    <button type="button" class="close" data-dismiss="modal">×</button>
			    <h3>Title</h3>
	    </div>
	    <div class="modal-body">
          <iframe src="" style="zoom:0.60" frameborder="0" height="50" width="99.6%"></iframe>
	    </div>
	    <div class="modal-footer">
		    <button class="btn" data-dismiss="modal">OK</button>
	    </div>
    </div>

    <asp:SqlDataSource ID="sdsModelLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select cm.Id as Id, cm.Model as Model from [dbo].[CatalogModels] cm WHERE cm.IsDeleted = 0 ORDER by cm.Model">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsRicTypeLookup" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" 
        SelectCommand="select t.Id as Id, t.RicType as RicType from RICTypes t WHERE t.IsDeleted = 0 ORDER BY t.RicType">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsPrevPMLookup" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" 
        SelectCommand="select distinct t.PackingMonth from RICs t WHERE t.IsDeleted = 0 and CatalogModelId = @ModelId and t.PackingMonth < @PackingMonth ORDER BY t.PackingMonth desc">
        <SelectParameters>
            <asp:Parameter Name="ModelId" DbType="Int32" DefaultValue="0" Direction="Input" />
            <asp:Parameter Name="PackingMonth" DbType="Int32" DefaultValue="0" Direction="Input" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
