<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="STAChecklist.aspx.cs" Inherits="Ebiz.WebForm.Tids.CV.Checklist.STAChecklist" %>

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
        //    }
        //}

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

        function btnUpload_Click(s, e)
        {
            fileUploadControl.ClearText();
            fileUploadControl.UpdateErrorMessageCell(0, '', true);
            popupDocUpload.Show();
        }

        function btnSave_Click(s, e) {
            if (btnSave.GetText() == "Upload")
                doUpload();

            else if (btnSave.GetText() == "Cancel")
                cancelUpload();
        }

        function doUpload() {
            if (!ASPxClientEdit.ValidateGroup('valgroup1')) {
                return;
            }

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


    </script>

    <h2 class="grid-header">Alteration Checklists</h2>

    <dx:ASPxPopupControl ID="popupDocUpload" ClientInstanceName="popupDocUpload" runat="server" AllowDragging="True" AutoUpdatePosition="True"
        CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="File Upload" Modal="True"
        PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="400" MinHeight="200"
        OnWindowCallback="popupDocUpload_OnWindowCallback" ShowFooter="true">
        <ClientSideEvents EndCallback="function(s,e){if (!!s.cpHeaderText){ s.SetHeaderText(s.cpHeaderText); }}">
        </ClientSideEvents>

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="width: 100%; overflow: hidden;">
                    <div>
                        <dx:ASPxTextBox ID="txtPM" ClientInstanceName="txtPM" Visible ="false" runat="server" Caption="Packing Month" ReadOnly="true">
                           <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxTextBox>
<%--                        <dx:ASPxDateEdit ID="dtPM" Visible="true" runat="server" Caption="Packing Month" NullText="Packing Month" DisplayFormatString="yyyyMM">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxDateEdit>--%>
                    </div>
                    <div>
                        <dx:ASPxTextBox ID="txtModelId" ClientInstanceName="txtModelId" Visible ="false" runat="server" Caption="Model" ReadOnly="true">
                           <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxTextBox>
<%--                        <dx:ASPxComboBox ID="cmbModels" runat="server" Caption="Model" NullText="Model" TextField="Model" ValueField="Id" DataSourceID="sdsModelLookup"
                            ClientInstanceName="cmbModels" ReadOnly="true">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>--%>
                     </div>
                    <div>
                        <dx:ASPxComboBox ID="cmbRicType" runat="server" Caption="Packing List Type" NullText="PL Type" TextField="RicType" ValueField="Id" DataSourceID="sdsRicTypeLookup"
                            ClientInstanceName="cmbRicType">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>
                    </div>
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
        CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="File Upload" Modal="True"
        PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="400" MinHeight="200"
        OnWindowCallback="popupComparePL_WindowCallback" ShowFooter="true">
        <ClientSideEvents EndCallback="function(s,e){if (!!s.cpHeaderText){ s.SetHeaderText(s.cpHeaderText); }}">
        </ClientSideEvents>

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="width: 100%; overflow: hidden;">
                    <div>
                        <dx:ASPxTextBox ID="txtModelId_Compare" ClientInstanceName="txtModelId_Compare" Visible ="false" runat="server" Caption="Model" ReadOnly="true">
                           <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxTextBox>
<%--                        <dx:ASPxComboBox ID="cmbModels" runat="server" Caption="Model" NullText="Model" TextField="Model" ValueField="Id" DataSourceID="sdsModelLookup"
                            ClientInstanceName="cmbModels" ReadOnly="true">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>--%>
                     </div>
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
                    <div>
                        <dx:ASPxComboBox ID="cmbRicType_Compare" runat="server" Caption="Packing List Type" NullText="PL Type" TextField="RicType" ValueField="Id" DataSourceID="sdsRicTypeLookup"
                            ClientInstanceName="cmbRicType_Compare">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>                                        
                        </dx:ASPxComboBox>
                    </div>
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