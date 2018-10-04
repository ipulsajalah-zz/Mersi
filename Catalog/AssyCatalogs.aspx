<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AssyCatalogs.aspx.cs" Inherits="Ebiz.WebForm.Tids.CV.Catalog.AssyCatalogs" %>

<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
        <style>
        .nav > li > a {
            position: relative;
            display: block;
            padding: 8px 8px;
        }

        .col-xs-12 {
            background-color: #fff;
        }

        #sidebar {
            height: 100%;
            padding-right: 0;
            padding-top: 10px;
            background-color: #cceaf0;
            height: 620px;
            overflow: auto;
            /*margin-left: -35px;*/
        }

            #sidebar .nav {
                width: 95%;
            }

            #sidebar li {
                border: 0 #f2f2f2 solid;
                border-bottom-width: 1px;
            }

        #submenu1 {
            padding-left: 5px;
        }

        .lichild {
            margin-left: 10px;
            font-size: 12px;
            font-family: Segoe UI;
        } 
        .lichild2 {
            margin-left: 10px;
            font-size: 12px;
            font-family: Segoe UI;
            padding-left:0px;
        }
        .lichild3 {
            margin-left: 10px;
            font-size: 12px;
            font-family: Segoe UI;
            padding-left:1px;
        }
        .lichild4 {
            margin-left: 10px;
            font-size: 12px;
            font-family: Segoe UI;
            padding-left:2px;
        }
        /* collapsed sidebar styles */
        @media screen and (max-width: 767px) {
            .row-offcanvas {
                position: relative;
                -webkit-transition: all 0.25s ease-out;
                -moz-transition: all 0.25s ease-out;
                transition: all 0.25s ease-out;
            }

            .row-offcanvas-right .sidebar-offcanvas {
                right: -41.6%;
            }

            .row-offcanvas-left .sidebar-offcanvas {
                left: -41.6%;
            }

            .row-offcanvas-right.active {
                right: 41.6%;
            }

            .row-offcanvas-left.active {
                left: 41.6%;
            }

            .sidebar-offcanvas {
                position: absolute;
                top: 0;
                width: 41.6%;
            }

            #sidebar {
                padding-top: 0;
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.navahref').click(function () {
                var tempid = $(this).attr('id');
                var submenuid = "#submenu" + tempid;
                $.ajax({
                    type: "POST",
                    url: "AssyCatalogs.aspx/GetCollapsedTreeView",
                    data: "{'id':'" + $(this).attr('id') + "', 'flag':'" + $(submenuid).is(":visible") + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            });
        });

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

        function btnUpload_Click(s, e) {
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

        function fileUploadControl_UploadComplete(s, e) {
            btnSave.SetText("Upload");
            btnExit.SetEnabled(true);
            lblSuccess.SetText(e);

            //call function to process the file
            popupDocUpload.PerformCallback("upload");
        }

        function fileUploadControl_TextChanged(s, e) {
            lblSuccess.SetText("");
        }

        function DisabledButton(flag) {
            //btnShowData.SetVisible(false);  //todo: cari tau cara get CommandArgument
            //btnClose.SetVisible(flag);
            //btnExit.SetVisible(flag);
        }


    </script>
    <%--<ul class='nav collapse'  id='submenu{0}' role='menu' aria-labelledby='btn-{0}'>
   --%>                                                 
    <h2 class="grid-header" style="align-content:center; text-align:center; align-items:center; color:black;">Assembly Catalogue</h2>
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
     <div class="container" style="padding: 0;">
        <div class="row row-offcanvas row-offcanvas-left">
        </div>
    </div>
    <div class="container-fluid" style="width: 100%; padding: 10px 0 10px 0 !important; text-align: left; font-weight: bolder">
        <dx:ASPxButton runat="server" ID="btnUpload" EnableTheming="false" ClientInstanceName="btnUpload" Text="| Upload Assy Catalogue" AutoPostBack="false">
            <ClientSideEvents Click="btnUpload_Click" />
            <Image Url="../../Content/Images/glyphicons-145-folder-open.png"></Image>
        </dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="btnExport" Text="Export To Excel" OnClick="btnExport_Click"></dx:ASPxButton>
    </div>

    <dx:ASPxPopupControl ID="popupDocUpload" ClientInstanceName="popupDocUpload" runat="server" AllowDragging="True" AutoUpdatePosition="True"
        CloseAction="None" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="File Upload" Modal="True"
        PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="400" MinHeight="200"
        OnWindowCallback="popupDocUpload_OnWindowCallback" ShowFooter="true">
        <ClientSideEvents EndCallback="function(s,e){if (!!s.cpHeaderText){ s.SetHeaderText(s.cpHeaderText); }}"></ClientSideEvents>

        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div style="width: 100%; overflow: hidden;">
                   <%-- <div>
                        <dx:ASPxLabel runat="server" Text="Date Upload :" Font-Bold="true"></dx:ASPxLabel>
                        <br />
                        <dx:ASPxDateEdit ID="dtPM" Visible="true" runat="server" NullText="Packing Month" DisplayFormatString="yyyyMM">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                    </div>--%>
                    <div>
                        <dx:ASPxLabel runat="server" Text="Model :" Font-Bold="true"></dx:ASPxLabel>
                        <br />
                        <dx:ASPxComboBox ID="cmbModels" runat="server" NullText="Model" TextField="Model" ValueField="Id" DataSourceID="sdsModelLookup"
                            ClientInstanceName="cmbModels">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True" ValidationGroup="valgroup1">
                                <RequiredField IsRequired="True" ErrorText="Required" />
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </div>
                    <div>
                        <dx:ASPxLabel runat="server" Text="Select File:" Font-Bold="true"></dx:ASPxLabel>
                        <br />
                        <dx:ASPxUploadControl ID="fileUploadControl" runat="server" UploadMode="Standard" Width="280px" ShowProgressPanel="True"
                            ClientInstanceName="fileUploadControl" NullText="Click here to browse files..." OnFileUploadComplete="fileUploadControl_OnFileUploadComplete" ShowUploadButton="False">
                            <ClientSideEvents FileUploadComplete="fileUploadControl_UploadComplete"
                                Init="function(s,e){ DisabledButton(false); }" TextChanged="fileUploadControl_TextChanged" />
                        </dx:ASPxUploadControl>
                    </div>
                </div>
                <div>
                    <dx:ASPxLabel ID="lblSuccess" ClientInstanceName="lblSuccess" Text="" runat="server"></dx:ASPxLabel>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <hr />
            <table>
                <tr>
                    <td style="width: 100%"></td>
                    <td>
                        <dx:ASPxButton runat="server" ID="btnSave" ClientInstanceName="btnSave" Text="Upload" AutoPostBack="false">
                            <ClientSideEvents Click="btnSave_Click" />
                        </dx:ASPxButton>
                    </td>
                    <td style="width: 50px"></td>
                    <td style="padding-left: 2px;">
                        <dx:ASPxButton runat="server" ID="btnExit" ClientInstanceName="btnExit" OnClick="btnExit_Click" Text="Close" AutoPostBack="false">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <asp:SqlDataSource ID="sdsModelLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select cm.Id as Id, cm.Model as Model from [dbo].[CatalogModels] cm WHERE cm.IsDeleted = 0 ORDER by cm.Model"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsRicTypeLookup" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select t.Id as Id, t.RicType as RicType from RICTypes t WHERE t.IsDeleted = 0 ORDER BY t.RicType"></asp:SqlDataSource>



</asp:Content>