<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="IrregularAlterationEdit.aspx.cs" Inherits="Ebiz.WebForm.custom.IAs.IrregularAlterationEdit" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Irregular Alteration Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .hiddentb {
            display: none;
        }
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

        .container {
            width: 100%;
        }

        .lichild {
            margin-left: 10px;
            font-size: 12px;
            font-family: Segoe UI;
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

        body .dxhePreviewArea_MetropolisBlue.dxheViewArea_MetropolisBlue {
            border: 1px solid;
        }

        .dxeEditArea_MetropolisBlue {
            background-color: #f3f3f3 !important;
        }

        .dxeDisabled_MetropolisBlue, .dxeDisabled_MetropolisBlue td.dxe {
            background-color: #f3f3f3 !important;
        }

        .dxeReadOnly_MetropolisBlue {
            background-color: #f3f3f3 !important;
        }
    </style>
    <script type="text/javascript">
        function OnNewClick(s, e) {
            gvTaskReport2.UpdateEdit();
        }
        function CallPopUPReport(buttonId) {
            TempGetIATaskId.SetText(buttonId)
            TempGetIATaskId2.Set('hidden_value', buttonId)
            //gvTaskReport2.Refresh();
            PopUpAttachmentDepartment.Show();
            //gvTaskReport2.AddNewRow();
            //gvTaskReport2.PerformCallback();
        }
        function CallPopUPViewReport(buttonId) {
            TempGetIATaskId.SetText(buttonId)
            gvTaskReport.Refresh();
            PopUpViewAttachmentDepartment.Show();
            //gvTaskReport.PerformCallback();
        }

        function ALertMessage(message, boolFlag) {
            alert(message);
            if (boolFlag == 'true') {
                window.location = "IrregularAlterationPage.aspx";
            }
        }

        function OnModelChanged(cmbModel) {
            CmbVariant.PerformCallback(cmbModel.GetSelectedItem().value.toString());
        }

        var lastModel = null;
        function OnEndCallback(s, e) {
            if (lastModel) {
                gvAffectedModels.GetEditor("Variant").PerformCallback(lastModel);
                lastModel = null;
            }
        }

        function OnDepartmentChanged(ddlDepartment) {
            DdlManager.PerformCallback(ddlDepartment.GetSelectedItem().value.toString());
        }

        function ucAttachment_UploadCompleted(s, e) {
            //var txtbox = gvAttachments.GetEditor("FileName");
            gvAttachments.GetEditor('FileName').SetText(e.callbackData);
        }

        function ucTaskAttachment_UploadCompleted(s, e) {
            //var txtbox = gvAttachments.GetEditor("FileName");
            //gvTaskReport.GetEditor('FileName').SetText(e.callbackData);
            gvTaskReport_FileName.SetText(e.callbackData);
        }
        function ucTaskAttachment_UploadCompleted2(s, e) {
            //var txtbox = gvAttachments.GetEditor("FileName");
            //gvTaskReport.GetEditor('FileName').SetText(e.callbackData);
            gvTaskReport_FileName2.SetText(e.callbackData);
        }
        //function rbOpenClick(s, e)
        //{
        //    if (s.GetCheckState == "Checked")
        //    {

        //        alert("Meytri :*")

        //    }
        //}
        //function rbCloseClick(s, e)
        //{
        //    if (s.GetCheckState == "Checked")
        //    {
        //        //rbOpen.SetChecked(false);
        //        rbOpenClick(e.boolFlag == false)
        //    }
        //}
        window.onbeforeunload = function (e) {
            //TODO: this is still not working
            //cb.PerformCallback('Unload');
            //jQuery.ajax({ url: "~/custom/IrregAlt/IAEdit.aspx?delete=" + lblId.Text, async: false });
        };
        function CloseUploadControl() {
            PopUpAttachMent.Hide();
            GVDataAttachment.PerformCallback();
        }
        function OnDropDownJan(s, e) {
            var maxDayFormated = new Date(2017, 0, 1);
            s.GetCalendar().SetVisibleDate(maxDayFormated);
        }
        //CheckedChanged
        function cbIAOrg_OnChanged(s, e) {
            var obj = new Object();
            obj.IAId = s.cpIaId;
            obj.OrganizationId = s.cpOrganizationId;
            obj.Task = "";
            obj.IATaskStatusId = 1;
            obj.AssignedUserId = s.cpManagerUserId;


            if (s.GetCheckState() == "Checked") {
                $.ajax({
                    url: '/api/list/insert/IATasks',
                    type: "PUT",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        $('#ctl00_ctl00_ASPxSplitter1_Content_MainContent_frmLayoutIrregAlt_tbx'.concat(s.cpID)).val(data.status);
                        gvAffectedDepart.Refresh();
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });

            }
            else {
                var tempid = $('#ctl00_ctl00_ASPxSplitter1_Content_MainContent_frmLayoutIrregAlt_tbx'.concat(s.cpID)).val();

                $.ajax({
                    url: '/api/list/delete/IATasks/'.concat(tempid),
                    type: 'DELETE',
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                }).done(function () {
                    gvAffectedDepart.Refresh();
                }).fail(function (msg) {
                    console.log(msg);
                    //console.log('FAIL');
                }).always(function (msg) {
                    gvAffectedDepart.Refresh();
                });
            }

        }


    </script>

    <style type="text/css">
        .wrapText {
            word-break: break-all;
        }
    </style>
    <div class='container' style='padding: 0; margin: 0'>
        <div class='row row-offcanvas row-offcanvas-left'>
            <div class='col-xs-6 col-sm-2 sidebar-offcanvas' id='sidebar' role='navigation'>
                <nav>
                    <ul class='nav'>
                        <li><a href='#' id='btn-1' data-toggle='collapse' data-target='#submenu1' aria-expanded='false'>(PPD) PA PRE-INFO</a>
                            <ul class='nav collapse in' id='submenu1' role='menu' aria-labelledby='btn-1'>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=All'>ALL INFO DOCUMENT</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=Close'>TASK STATUS CLOSE</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=Open'>TASK STATUS OPEN</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=Draft'>TASK STATUS DRAFT</a></li>
                            </ul>
                        </li>
                        <li><a href='#' id='btn-2' data-toggle='collapse' data-target='#submenu2' aria-expanded='false'>PACKING MONTH</a>
                            <ul class='nav collapse in' id='submenu2' role='menu' aria-labelledby='btn-2'>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=1'>JANUARY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=2'>FEBRUARY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=3'>MARCH</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=4'>APRIL</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=5'>MAY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=6'>JUNE</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=7'>JULY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=8'>AGUSTUS</a></li>

                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=9'>SEPTEMBER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=10'>OCTOBER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=11'>NOVEMBER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=12'>DECEMBER</a></li>

                            </ul>
                        </li>
                        <li><a href='#' id='btn-3' data-toggle='collapse' data-target='#submenu3' aria-expanded='false'>USER PROFILE</a>
                            <ul class='nav collapse in' id='submenu3' role='menu' aria-labelledby='btn-3'>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Email=1'>EMAIL MANAGER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Email=2'>EMAIL CC DIRECTURE</a></li>
                            </ul>
                        </li>
                    </ul>
                </nav>
            </div>
            <div class='col-xs-12 col-sm-10' style='padding-right: 0; margin: 0; padding-left: 0'>

                <div id="MainContainer" runat="server">
                    <asp:HiddenField ID="hfIaID" runat="server" />

                    <br />
                    &nbsp; 
                    <dx:ASPxLabel runat="server" ID="lblHeader" CssClass="headerText" Visible="false" Font-Size="Large" Font-Bold="true" />
                    <dx:ASPxLabel runat="server" ID="lblTempIdIATask" ClientInstanceName="lblTempIdIATask" Visible="true" />

                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                        <dx:ASPxLabel runat="server" ID="lblheader2" CssClass="headerText" Text="Irregular Alteration" Font-Size="Large" Font-Bold="true" />
                    </div>
                    <br />
                    <br />
                    <dx:ASPxHiddenField runat="server" ClientInstanceName="hfAnswers" ID="hfAnswers">
                    </dx:ASPxHiddenField>

                    <dx:ASPxCallback ID="ASPxCallback" runat="server" OnCallback="ASPxCallback_Callback" ClientInstanceName="cb">
                    </dx:ASPxCallback>

                    <div style="padding-top: 10px;">
                        <dx:ASPxLabel runat="server" ID="lblMessage" Visible="false"></dx:ASPxLabel>
                    </div>

                    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Style="margin-left: 0px" Width="100%" ColCount="2">
                        <Items>
                            <dx:LayoutGroup Caption="">
                                <Items>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <%-- <dx:ASPxButton runat="server" Width="100px" ID="btnApproveManager" Text="Send To Approver Manager"></dx:ASPxButton>--%>
                                                <dx:ASPxLabel runat="server" ID="lblApproverManager" Visible="false"></dx:ASPxLabel>
                                                <dx:ASPxLabel runat="server" ID="lblEmailManager" Visible="false"></dx:ASPxLabel>
                                                <dx:ASPxButton runat="server" Text="Approve & Send" Width="100px" ID="btnSubmit" OnClick="btnSubmit_Click1" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to send this data?');}"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Send Back" Width="100px" ID="btnSendBack" OnClick="btnSendBack_Click" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to send back this data?');}"></ClientSideEvents>
                                                </dx:ASPxButton>

                                                <dx:ASPxButton runat="server" Text="Cancel" Width="100px" ID="btnCancel" OnClick="btnCancel_Click" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to cancel this data?');}"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Send To Approver Manager" ID="btnSendToApproved" OnClick="btnSendToApproved_Click" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to Send this data to approver manager ?');}"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Send Notify" ID="SendNotif" OnClick="SendNotif_Click" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to Send Notify for user assignment ?');}"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click"></dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Edit" ID="btnEditIA" OnClick="btnEditIA_Click" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to edit this data?');}"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Notify" ID="btnNotify" OnClick="btnNotify_Click" Visible="false">
                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to notify Assign Manager?');}"></ClientSideEvents>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton runat="server" Text="Back" ID="btnBack" OnClick="btnBack_Click"></dx:ASPxButton>
                                                <hr />
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Status">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxProgressBar runat="server" ID="ProgresBar" Width="100%"></dx:ASPxProgressBar>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>

                            <dx:LayoutGroup Caption="Vpn" ColSpan="2">
                                <Items>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <div class="container">
                                                    <div class="row row-centered">
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="January"></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="February">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="March">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="April">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="May">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="June">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Januari" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 0, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Februari" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 1, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Maret" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 2, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="April" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 3, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Mei" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 4, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Juni" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 5, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="clearfix"></div>

                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="July"></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="August">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="September">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="October">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="November">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="December">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Juli" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 6, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Agustus" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 7, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="September" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 8, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Oktober" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 9, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="November" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 10, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-xs-2 col-centered">
                                                            <dx:ASPxDateEdit ID="Desember" runat="server" Width="100%">
                                                                <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 11, 1)); }"></ClientSideEvents>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>
                                                </div>




                                                <br />
                                                <table border="0" style="width: 100%; padding: 2px;">

                                                    <tr>
                                                        <td rowspan="1" style="width: 100%; text-align: center;">
                                                            <dx:ASPxButton runat="server" Text="Save Vpn" ID="BtnSaveVpn" ClientInstanceName="BtnSaveVpn" OnClick="BtnSaveVpn_Click1">
                                                                <%--<ClientSideEvents Click="BtnSaveVpn_OnChanges" />--%>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td rowspan="1" style="width: 100%; text-align: center;">
                                                            <dx:ASPxLabel runat="server" ID="lbljanuari" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblfebruari" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblmaret" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblapril" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblmei" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lbljuni" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lbljuli" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblagustus" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblseptember" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lbloktober" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lblnovember" Visible="false"></dx:ASPxLabel>
                                                            <dx:ASPxLabel runat="server" ID="lbldesember" Visible="false"></dx:ASPxLabel>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>

                    <dx:ASPxFormLayout ID="frmLayoutIrregAlt" runat="server" Style="margin-left: 0px" Width="100%" ColCount="2">
                        <Items>
                            <dx:LayoutGroup Caption="Author" ColSpan="2">
                                <Items>
                                    <dx:LayoutItem Caption="" FieldName="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <div class="container">
                                                    <div class="form-horizontal">
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4" for="TbStatus">Status</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="StatusIA" runat="server" Visible="false" Width="300px" Enabled="false" ReadOnly="true" DisplayFormatString=""></dx:ASPxTextBox>
                                                            </div>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="LastStatusIA" runat="server" Width="300px" Visible="false" Enabled="false" ReadOnly="true" DisplayFormatString=""></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4" for="TbAuthor">Author</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="Namelogin" runat="server" Width="300px" Enabled="false" DisplayFormatString=""></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4" for="TbEmail">Email</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="emailuser" runat="server" Width="300px" Enabled="false"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4" for="TbDate">Date</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="createddate" runat="server" Width="300px" Enabled="false" ReadOnly="true"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4" for="TbType">Type</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxComboBox ID="cmbIAType" runat="server" Width="300px" ValueField="Id" DataSourceID="sdsIATypeLookup" TextField="IAType" OnSelectedIndexChanged="cmbIAType_SelectedIndexChanged" ValueType="System.Int32" AutoPostBack="true" ClientInstanceName="cmbIAType" NullText="-Ticket-">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            <dx:LayoutGroup Caption="Approved" ColSpan="2">
                                <Items>
                                    <dx:LayoutItem Caption="" FieldName="InternalEpcNumber">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <div class="container">
                                                    <div class="form-horizontal">
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4">Internal EPC Number</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="txtNumber" runat="server" Width="300px" Enabled="false"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4">Name</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="NameApprover" runat="server" Width="300px" Enabled="false" ReadOnly="true"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix"></div>

                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4">Email</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="EmailApprover" runat="server" Width="300px" Enabled="false" ReadOnly="true"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4">Info Number</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="txtInfoNumber" runat="server" Width="300px"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                        <div class="clearfix"></div>

                                                        <div class="col-md-6 form-group">
                                                            <label class="control-label col-sm-4">Distribution Date</label>
                                                            <div class="col-sm-8">
                                                                <dx:ASPxTextBox ID="dtDistributionDate" runat="server" Width="300px" ReadOnly="true"></dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            <dx:LayoutGroup Caption="Department">
                                <Items>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <div class="container">
                                                    <div class="form-horizontal">
                                                        <div id="divDataView" runat="server"></div>
                                                    </div>
                                                </div>
                                                <dx:ASPxLabel runat="server" Text="Distribute To : " Font-Bold="true"></dx:ASPxLabel>
                                                <dx:ASPxLabel runat="server" ID="lblManagerName"></dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                </Items>

                            </dx:LayoutGroup>

                            <dx:LayoutGroup Caption="Data Information" ColSpan="2">
                                <Items>
                                    <dx:LayoutItem Caption="Part">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="txtPart" runat="server" Width="600px"></dx:ASPxTextBox>
                                                <br />
                                                <dx:ASPxButton ID="btnGeneratePart" runat="server" OnClick="btnGeneratePart_Click" Enabled="true" Text="Generate Part" BackColor="#999999" Font-Bold="true"></dx:ASPxButton>
                                                <dx:ASPxLabel runat="server" Text="*(GENERATE PART) - for get part description" ID="lblgeneratepart" Font-Bold="true"></dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Description">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxMemo runat="server" ID="DescriptionPart" Width="600px" Height="80px"></dx:ASPxMemo>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Models">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxComboBox runat="server" ID="cmbModels" DataSourceID="sdsModelLookup" Width="300px" ValueField="Id" TextField="ModelName" ValueType="System.Int32" AutoPostBack="false" ClientInstanceName="cmbModels" NullText="-Select-"></dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Valid From" FieldName="ValidPeriodFrom">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxDateEdit ID="dtValidFrom" runat="server" Width="139px" DisplayFormatString="yyyy/MM/01">
                                                    <%--      <ClientSideEvents DropDown="function(s,e) { s.GetCalendar().SetVisibleDate(new Date((new Date()).getFullYear(), 6, 1)); }"></ClientSideEvents>
                                                    --%>
                                                </dx:ASPxDateEdit>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Valid To" FieldName="ValidPeriodTo">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxDateEdit ID="dtValidTo" runat="server" Width="139px" DisplayFormatString="yyyy/MM/dd"></dx:ASPxDateEdit>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Implemention" FieldName="ImplementationDate">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="ImplementationDate" runat="server" Width="300px"></dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Area">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxComboBox runat="server" ID="cmbArea" DataSourceID="sdsAreaLookup" Width="300px" ValueField="Id" TextField="Description" ValueType="System.Int32" AutoPostBack="true" ClientInstanceName="cmbArea" NullText="-Select-"></dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Station">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxComboBox runat="server" ID="cmbStation" DataSourceID="sdsStation" Width="300px" ValueField="Id" TextField="StationName" ValueType="System.Int32" AutoPostBack="false" ClientInstanceName="cmbStation" NullText="-Select-"></dx:ASPxComboBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Description Of Problem" FieldName="Description">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                <dx:ASPxMemo ID="txtDescription" runat="server" Height="71px" Width="300px" Visible="false"></dx:ASPxMemo>
                                                <dx:ASPxHtmlEditor ID="htDescription" ClientInstanceName="htDescription" runat="server" Height="350px" Width="100%" SettingsImageUpload-UploadFolder="~\custom\FileUpload\IAPictures">
                                                    <%--<Settings AllowHtmlView="False" AllowPreview="False" />--%>
                                                    <settingsimageupload uploadimagefolder="~\custom\FileUpload\IAPictures/" uploadfolder="~\custom\FileUpload\IAPictures/"></settingsimageupload>
                                                </dx:ASPxHtmlEditor>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Department">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxGridView OnHtmlDataCellPrepared="gvAffectedDepart_HtmlDataCellPrepared" ID="gvAffectedDepart" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvAffectedDepart" KeyFieldName="Id" DataSourceID="sdsIATask" EnableRowsCache="false" OnInit="gvAffectedDepart_Init"
                                                    OnRowInserting="gvAffectedDepart_RowInserting" OnRowUpdating="gvAffectedDepart_RowUpdating" OnRowValidating="gvAffectedDepart_RowValidating" OnStartRowEditing="gvAffectedDepart_StartRowEditing" OnDataBound="gvAffectedDepart_DataBound"
                                                    OnCellEditorInitialize="gvAffectedDepart_CellEditorInitialize" OnCommandButtonInitialize="gvAffectedDepart_CommandButtonInitialize" OnHtmlRowPrepared="gvAffectedDepart_HtmlRowPrepared">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="1" ReadOnly="True" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataMemoColumn FieldName="Task" VisibleIndex="1" Width="300" GroupIndex="1">
                                                            <EditFormSettings Visible="False" VisibleIndex="1" ColumnSpan="2" />
                                                            <PropertiesMemoEdit>
                                                                <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                                    <RequiredField IsRequired="True"></RequiredField>
                                                                </ValidationSettings>
                                                            </PropertiesMemoEdit>
                                                        </dx:GridViewDataMemoColumn>
                                                        <dx:GridViewDataComboBoxColumn FieldName="OrganizationId" VisibleIndex="2" Caption="Department">
                                                            <PropertiesComboBox TextField="Name" ValueField="Id" DataSourceID="sdsOrganizationLookup" Width="300">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { gvAffectedDepart.GetEditor('AssignedUserId').PerformCallback(s.GetValue()); gvAffectedDepart.GetEditor('DelegateUserId').PerformCallback(s.GetValue()); }" />
                                                            </PropertiesComboBox>
                                                            <EditFormSettings Visible="False" VisibleIndex="2" ColumnSpan="2" />
                                                        </dx:GridViewDataComboBoxColumn>
                                                        <dx:GridViewDataComboBoxColumn Name="AssignedUserId" FieldName="AssignedUserId" VisibleIndex="3" Caption="Assigned To">
                                                            <PropertiesComboBox TextField="FullName" ValueField="UserId" DataSourceID="sdsUserLookup">
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>
                                                        <dx:GridViewDataDateColumn FieldName="AssignedDate" VisibleIndex="4" PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataComboBoxColumn Name="DelegateUserId" FieldName="DelegateUserId" Visible="false" VisibleIndex="5" Caption="Delegated To">
                                                            <EditFormSettings Visible="True" VisibleIndex="5" />
                                                            <PropertiesComboBox TextField="FullName" ValueField="UserId" DataSourceID="sdsUserLookup1">
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>
                                                        <dx:GridViewDataDateColumn FieldName="DelegateDate" VisibleIndex="6" Visible="false" PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy">
                                                            <EditFormSettings Visible="false" />
                                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataTextColumn FieldName="DelegateTo" VisibleIndex="7" Caption="Delegate To">
                                                            <EditFormSettings Visible="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataDateColumn FieldName="CloseDate" VisibleIndex="8" PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                            <DataItemTemplate>
                                                                <dx:ASPxButton AutoPostBack="false" ID="lnkTest_" runat="server" Font-Underline="false" RenderMode="Link" Text="Report" OnClick='<%# string.Format("javascript:return CallPopUPReport(\"{0}\")", Eval("Id")) %>'>
                                                                </dx:ASPxButton>
                                                                <br />
                                                                <dx:ASPxButton AutoPostBack="false" ID="lnkTest2_" runat="server" Font-Underline="false" RenderMode="Link" Text="View Report" OnClick='<%# string.Format("javascript:return CallPopUPViewReport(\"{0}\")", Eval("Id")) %>'>
                                                                </dx:ASPxButton>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataTextColumn VisibleIndex="13" Visible="false" Caption="Attachment" FieldName="FlagReport">
                                                            <EditFormSettings Visible="False" />
                                                            <DataItemTemplate>
                                                                <dx:ASPxButton AutoPostBack="false" ID="lnkTest" runat="server" RenderMode="Link" Text="Report" OnClick='<%# string.Format("javascript:return CallPopUPReport(\"{0}\")", Eval("Id")) %>'>
                                                                </dx:ASPxButton>
                                                                <br />
                                                                <dx:ASPxButton AutoPostBack="false" ID="lnkTest2" runat="server" RenderMode="Link" Text="View Report" ClientInstanceName="lnkTest2" OnClick='<%# string.Format("javascript:return CallPopUPViewReport(\"{0}\")", Eval("Id")) %>'>
                                                                </dx:ASPxButton>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataComboBoxColumn FieldName="IATaskStatusId" VisibleIndex="9" Caption="Status" Visible="false">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesComboBox TextField="Status" ValueField="Id" DataSourceID="sdsIATaskStatusLookup">
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>
                                                        <dx:GridViewDataTextColumn VisibleIndex="10" Caption="Status">
                                                            <EditFormSettings Visible="False" />
                                                            <DataItemTemplate>
                                                                <dx:ASPxButton runat="server" Text="Close Task" ID="btnCloseTask" OnClick="btnCloseTask_Click" Visible="false">
                                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to close this task?');}"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton runat="server" Text="Open Task" ID="btnOpenTask" OnClick="btnOpenTask_Click" Visible="false">
                                                                    <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to open this task?');}"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton runat="server" Text="Open" ID="BtnStatusOpen" Visible="false" Enabled="false" BackColor="Red">
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton runat="server" Text="Close" ID="BtnStatusClose" Visible="false" Enabled="false" BackColor="Blue">
                                                                </dx:ASPxButton>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewCommandColumn Name="Command" VisibleIndex="0" ShowDeleteButton="true" ShowNewButtonInHeader="true" ShowEditButton="true">
                                                        </dx:GridViewCommandColumn>
                                                    </Columns>
                                                    <Settings ShowGroupPanel="true" />
                                                    <SettingsBehavior ConfirmDelete="True" />
                                                    <SettingsText ConfirmDelete="Are you sure want to delete this data?" />
                                                    <SettingsDetail ShowDetailRow="true" />
                                                </dx:ASPxGridView>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Document File">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxButton ID="ShowAttachment" Text="Attachment" runat="server" OnClick="ShowAttachment_Click"></dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Attachment File">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxGridView runat="server" ViewStateMode="Enabled" ID="GVDataAttachment" ClientInstanceName="GVDataAttachment" OnCustomCallback="GVDataAttachment_CustomCallback" OnHtmlRowPrepared="GVDataAttachment_HtmlRowPrepared" KeyFieldName="Id" Width="100%">
                                                    <Columns>
                                                        <dx:GridViewDataColumn FieldName="Id" VisibleIndex="0" Visible="false">
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataColumn FieldName="FilePath" VisibleIndex="2" Visible="false">
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataHyperLinkColumn FieldName="FilePath" PropertiesHyperLinkEdit-TextField="FileName" PropertiesHyperLinkEdit-Target="_blank" Caption="FileName" VisibleIndex="3">
                                                            <PropertiesHyperLinkEdit></PropertiesHyperLinkEdit>
                                                        </dx:GridViewDataHyperLinkColumn>
                                                        <dx:GridViewDataTextColumn VisibleIndex="4" Caption="Action">
                                                            <DataItemTemplate>
                                                                <dx:ASPxButton AutoPostBack="true" ID="IdBtndelete" runat="server" RenderMode="Link" Text="Delete" OnClick="IdBtndelete_Click">
                                                                </dx:ASPxButton>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="100" VerticalScrollBarStyle="Standard" />

                                                </dx:ASPxGridView>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Remarks">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <br />
                                                <table style="border: 1px solid grey; width: 70%; align-content: center;">
                                                    <tr>
                                                        <th rowspan="1" style="width: 100px; border: 1px solid grey; text-align: center; background-color: #f5f5f5; height: 25px;">
                                                            <dx:ASPxLabel ID="headerIaTask" runat="server" Text="Remarks">
                                                            </dx:ASPxLabel>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 10px; padding-top: 5px;">
                                                            <asp:Label ID="lblremaks" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>

                    <dx:ASPxPopupControl ID="PopUpAttachmentDepartment" runat="server"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpAttachmentDepartment"
                        HeaderText="Attachment Form" AllowDragging="True" Modal="True" Width="800" Height="650" PopupAnimationType="Fade">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <dx:ASPxFormLayout ID="frmPartSelection" runat="server" Width="100%">
                                    <Items>
                                        <dx:LayoutItem Caption="">
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Description" CaptionSettings-VerticalAlign="Middle">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxMemo ID="txtDesciptonAttachment" runat="server" Width="100%" Visible="false"></dx:ASPxMemo>
                                                    <dx:ASPxHtmlEditor ID="HtmlDesciptonAttachment" ClientInstanceName="HtmlDesciptonAttachment" runat="server" Height="350px" Width="100%" SettingsImageUpload-UploadFolder="~\custom\FileUpload\IAPictures">
                                                        <%--<Settings AllowHtmlView="False" AllowPreview="False" />--%>
                                                        <settingsimageupload uploadimagefolder="~\custom\FileUpload\IAPictures/" uploadfolder="~\custom\FileUpload\IAPictures/"></settingsimageupload>
                                                    </dx:ASPxHtmlEditor>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Select File" FieldName="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxUploadControl ID="ucTaskAttachments" runat="server" UploadMode="Advanced" ShowUploadButton="false" ShowProgressPanel="true" AutoStartUpload="true"
                                                        ClientInstanceName="ucTaskAttachments" OnFileUploadComplete="ucTaskAttachments_FileUploadComplete">
                                                        <AdvancedModeSettings EnableDragAndDrop="True" />
                                                        <ValidationSettings MaxFileSize="25194304" AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png" />
                                                        <ClientSideEvents  FileUploadComplete="ucTaskAttachment_UploadCompleted2" />
                                                    </dx:ASPxUploadControl>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="File Name" FieldName="FileName">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="FileNameAttachment" runat="server" ClientInstanceName="gvTaskReport_FileName2" ReadOnly="true" ValidationSettings-RequiredField-IsRequired="true">
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Status" FieldName="" CaptionSettings-VerticalAlign="Middle">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxRadioButton ID="rbOpen" ClientInstanceName="rbOpen"  Text="Open" GroupName="bbb" runat="server">
                                                        <ClientSideEvents  />
                                                    </dx:ASPxRadioButton>
                                                    <dx:ASPxRadioButton ID="rbCLose" ClientInstanceName="rbClose" Text="Close" GroupName="bbb" runat="server">
                                                        <ClientSideEvents />
                                                    </dx:ASPxRadioButton>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                        </dx:LayoutItem>

                                        <dx:LayoutItem Caption="" HorizontalAlign="Center">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxButton ID="SaveDelegationAttachment" ClientInstanceName="SaveDelegationAttachment" OnClick="SaveDelegationAttachment_Click" HorizontalAlign="Center" runat="server" Text="Save">
                                                        <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are you sure want to Save this data?');}" />
                                                    </dx:ASPxButton>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:ASPxFormLayout>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ID="PopUpViewAttachmentDepartment" runat="server"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpViewAttachmentDepartment"
                        HeaderText="Attachment Form" AllowDragging="True" Modal="True" PopupAnimationType="Fade">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server" Width="450px">
                                    <Items>
                                        <dx:LayoutItem Caption="" HorizontalAlign="Center">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">

                                                    <dx:ASPxGridView ID="gvTaskReport" runat="server" Width="100%" ClientInstanceName="gvTaskReport" KeyFieldName="Id" AutoGenerateColumns="False" DataSourceID="sdsIATaskReport2"
                                                        OnRowInserting="gvTaskReport_RowInserting" OnRowDeleting="gvTaskReport_RowDeleting" OnRowUpdating="gvTaskReport_RowUpdating" OnDataBound="gvTaskReport_DataBound"
                                                        OnBeforePerformDataSelect="gvTaskReport_BeforePerformDataSelect" OnInit="gvTaskReport_Init" OnCommandButtonInitialize="gvTaskReport_CommandButtonInitialize">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="1" ReadOnly="True" Visible="false">
                                                                <EditFormSettings Visible="false" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataMemoColumn FieldName="Report" VisibleIndex="2" Caption="Report" Width="400">
                                                                <CellStyle Wrap="True" CssClass="wrapText"></CellStyle>
                                                                <EditFormSettings Visible="True" VisibleIndex="2" ColumnSpan="2" />
                                                                <PropertiesMemoEdit>
                                                                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                                        <RequiredField IsRequired="True"></RequiredField>
                                                                    </ValidationSettings>
                                                                </PropertiesMemoEdit>
                                                            </dx:GridViewDataMemoColumn>
                                                            <dx:GridViewDataTextColumn Name="FileName" FieldName="FileName" VisibleIndex="3" Caption="File Name" Visible="true" ReadOnly="True" Width="200">
                                                                <EditFormSettings Visible="True" VisibleIndex="4" Caption="" />
                                                                <PropertiesTextEdit ClientInstanceName="gvTaskReport_FileName" />
                                                                <CellStyle Wrap="True" CssClass="wrapText"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="FilePath" VisibleIndex="4" Caption="" ReadOnly="True">
                                                                <EditFormSettings Visible="false" />
                                                                <DataItemTemplate>
                                                                    <dx:ASPxHyperLink NavigateUrl='<%# Eval("FilePath") %>' Target="_blank" Text="View File" runat="server">
                                                                    </dx:ASPxHyperLink>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataComboBoxColumn FieldName="IATaskStatusId" VisibleIndex="6" Caption="Status">
                                                                <PropertiesComboBox TextField="Status" ValueField="Id" DataSourceID="sdsIATaskStatusLookup1">
                                                                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                                                                        <RequiredField IsRequired="True"></RequiredField>
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents SelectedIndexChanged="SelectedChangesStatus" />
                                                                </PropertiesComboBox>
                                                            </dx:GridViewDataComboBoxColumn>
                                                        </Columns>
                                                        <Settings ShowGroupPanel="false" />
                                                        <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                                                    </dx:ASPxGridView>

                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:ASPxFormLayout>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

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
                                                    <dx:ASPxUploadControl ID="UploadAttachment" runat="server" Width="300px" ClientInstanceName="UploadControl"
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

                    <dx:ASPxTextBox ID="TempGetIATaskId" ClientInstanceName="TempGetIATaskId" runat="server" Width="170px"></dx:ASPxTextBox>
                    <dx:ASPxHiddenField ID="TempGetIATaskId2" ClientInstanceName="TempGetIATaskId2" runat="server"></dx:ASPxHiddenField>
                    <dx:ASPxLabel runat="server" ID="lblId" Visible="false" />
                    <dx:ASPxLabel runat="server" ID="lblUsername_" Visible="false" />
                    <dx:ASPxLabel runat="server" ID="lblId2" Visible="false" />
                    <dx:ASPxLabel runat="server" ID="errorMessageLabel" Visible="false" Style="padding-left: 153px;" ForeColor="Red" EnableViewState="false" EncodeHtml="false" />
                </div>
            </div>
        </div>
    </div>

    <asp:SqlDataSource ID="sdsIATaskStatusLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Status from IATaskStatus Order by Id"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIATaskStatusLookup1" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id, Status from IATaskStatus where Id in('1','2') Order by Id"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUserLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id as UserId, FirstName +' '+ LastName as FullName from Users Order by FirstName"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUserLookup1" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id as UserId, FirstName +' '+ LastName as FullName from Users Order by FirstName"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUserLookupByOrganizationId" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select
                            s.Id,
                            s.OrganizationId,
                            s.ManagerUserId,
                            a.Id as UserId,
                            a.FirstName +' '+ a.LastName as FullName
                            from IACcEmail s 
							inner join Users a on a.Id = s.ManagerUserId
                            where s.OrganizationId = @OrganizationId
							union
							Select
                            s.Id,
                            s.OrganizationId,
                            s.ManagerUserId,
                            a.Id as UserId,
                            a.FirstName +' '+ a.LastName as FullName
                            from IAOrganizations s 
							inner join Users a on a.Id = s.ManagerUserId
                            where s.OrganizationId = @OrganizationId
							">
        <SelectParameters>
            <asp:Parameter Name="OrganizationId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsUserDelegateLookupByOrganizationId" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id as UserId, FirstName +' '+ LastName as FullName from Users where OrganizationId=@OrganizationId and LastName is not null  Order by FirstName">
        <SelectParameters>
            <asp:Parameter Name="OrganizationId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsOrganizationLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select o.Id, o.Name from Organizations o"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsOrganizationLookupEdit" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select o.Id, o.Name from Organizations o where o.Id not in (
select ia.OrganizationId from IATasks ia where ia.IAId = @IAId)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIATypeLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select Id as Id, Description as IAType from IATypes order by Description"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsStationLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="   Select s.Id,
                            s.StationName,
                            s.AssemblyTypeId,
                            case when s.AssemblyTypeId = 1 then 'Chassis' 
						    when s.AssemblyTypeId = 2 then 'Aggregate'
						    else 'Cabin' end as AssemblyType,
							s.AssemblySectionId
                            from Stations s 
							inner join AssemblySections a on a.Id = s.AssemblySectionId
                            where s.AssemblySectionId = @AssemblySectionId and s.Id Not In(
                            select distinct a.StationId from IAStations a where a.IAId = @IAId
                            )
							order by s.StationName">
        <SelectParameters>
            <asp:Parameter Name="AssemblySectionId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsStationLookup1" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select s.Id,
                            s.StationName,
                            s.AssemblyTypeId,
                            case when s.AssemblyTypeId = 1 then 'Chassis' 
						    when s.AssemblyTypeId = 2 then 'Aggregate'
						    else 'Cabin' end as AssemblyType,
							s.AssemblySectionId
                            from Stations s ">
        <SelectParameters>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsGetAssemblySection" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select a.Id, a.AssemblySectionName
                            from AssemblySections a
							order by a.AssemblySectionName"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsTypeLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select * from ( select 0 as id, '-All Types-' as Type
                       union Select t.Id as Id, t.Type as Type from Types t 
                       WHERE t.AssemblyTypeId = @AssemblyTypeId)T order by T.Type">
        <SelectParameters>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsAreaLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select distinct a.Id, a.Description, a.AssemblySectionId from [GWISAssemblyAreas] a"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsStation" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select distinct a.Id, a.StationName, a.AssemblySectionId from Stations a
                       where a.AssemblySectionId = @AssemblySectionId">
        <SelectParameters>
            <asp:Parameter Name="AssemblySectionId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsModelLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select distinct a.Id, a.Id as CatalogModelId,a.Model as ModelName from CatalogModels a">
        <SelectParameters>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsModelLookupEdit" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select distinct a.Id, a.Id as CatalogModelId,a.Model as ModelName from CatalogModels a where Id not in(
SELECT distinct [CatalogModelId] FROM [IAModels] WHERE [IAId] = @IAId)">
        <SelectParameters>
            <asp:Parameter Name="AssemblyTypeId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsModelLookupByTypeId" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select  * from( select 0 as id, '-All Models-' as ModelName
                       union Select m.Id as Id, m.ModelName as ModelName from Models m 
                       inner join Types t on (t.Id = m.TypeId) WHERE m.TypeId = @TypeId)m order by m.ModelName">
        <SelectParameters>
            <asp:Parameter Name="TypeId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsVariantLookup" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select * from( select 0 as id, '-All Variant-' as VariantName
                       union Select Id, Variant as VariantName from Variants a)a order by a.VariantName">
        <SelectParameters>
            <asp:Parameter Name="ModelId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsVariantLookupByModelId" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="Select * from( select 0 as id, '-All Variant-' as VariantName
                       union Select Id, Variant as VariantName from Variants a WHERE CatalogModelId = @CatalogModelId)a order by a.VariantName">
        <SelectParameters>
            <asp:Parameter Name="CatalogModelId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsVPN" runat="server" OnUpdating="IAHeaders_Modifying"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [IATypeId],[InternalEpcNumber],[InfoNumber],[Description],[ValidPeriodFrom],[ValidPeriodTo],
                                [ImplementationDate],[DistributionDate],[IsDynamicCheck],[IsReadyForApproval],[IsApproved],
                                [AuthorUserId],u1.UserName as AuthorUserName,[ApproverUserId],u2.UserName as ApproverUserName,u1.OrganizationId as OrganizationId 
                        FROM [IAs] ia
                            LEFT JOIN [Users] u1 on (u1.Id = ia.AuthorUserId)
                            LEFT JOIN [Users] u2 on (u2.Id = ia.ApproverUserId)
                        WHERE ia.[Id] = @Id"
        UpdateCommand="UPDATE [IAs] SET [IATypeId]=@IATypeId,[InternalEpcNumber]=@InternalEpcNumber,[InfoNumber]=@InfoNumber,[Description]=@Description,
                                [ValidPeriodFrom]=@ValidPeriodFrom,[ValidPeriodTo]=@ValidPeriodTo,[ImplementationDate]=@ImplementationDate,
                                [__IsDraft]=0,[StatusID]=1 WHERE [Id] = @Id"
        DeleteCommand="Delete ia from IAs ia		                
						left join IAAttachments on ia.Id = IAAttachments.IAId
						left join IAModels on ia.Id = IAModels.IAId
						left join IAParts on ia.Id = IAParts.IAId           
	                    left join IAStations on ia.Id = IAStations.IAId
	                    left join IATasks on ia.Id = IATasks.IAId
	                    left join IATaskReports on IATasks.Id = IATaskReports.IATaskId
                        WHERE ia.Id = @Id">
        <SelectParameters>
            <asp:Parameter Name="Id" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <UpdateParameters>
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$cmbIAType" Name="IATypeId" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$txtNumber" Name="InternalEpcNumber" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$txtInfoNumber" Name="InfoNumber" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$htDescription" Name="Description" PropertyName="Html" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$dtValidFrom" Name="ValidPeriodFrom" PropertyName="Value" Type="DateTime" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$dtValidTo" Name="ValidPeriodTo" PropertyName="Value" Type="DateTime" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$ImplementationDate" Name="ImplementationDate" PropertyName="Value" Type="String" />
            <%--<asp:ControlParameter ControlID="frmLayoutIrregAlt$chkIsDynamicCheck" Name="IsDynamicCheck" PropertyName="Value" Type="Boolean" />--%>
            <%-- <asp:ControlParameter ControlID="frmLayoutIrregAlt$cmbApproverUser" Name="ApproverUserId" PropertyName="Value" Type="Int32" />
            --%>
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIAHeader" runat="server" OnUpdating="IAHeaders_Modifying"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [IATypeId],[InternalEpcNumber],[InfoNumber],[Description],[ValidPeriodFrom],[ValidPeriodTo],
                                [ImplementationDate],[DistributionDate],[IsDynamicCheck],[IsReadyForApproval],[IsApproved],
                                [AuthorUserId],u1.UserName as AuthorUserName,[ApproverUserId],u2.UserName as ApproverUserName,u1.OrganizationId as OrganizationId 
                        FROM [IAs] ia
                            LEFT JOIN [Users] u1 on (u1.Id = ia.AuthorUserId)
                            LEFT JOIN [Users] u2 on (u2.Id = ia.ApproverUserId)
                        WHERE ia.[Id] = @Id"
        UpdateCommand="UPDATE [IAs] SET [DistributionDate]=GETDATE(), [IATypeId]=@IATypeId,[InternalEpcNumber]=@InternalEpcNumber,[InfoNumber]=@InfoNumber,[Description]=@Description,
                                [ValidPeriodFrom]=@ValidPeriodFrom,[ValidPeriodTo]=@ValidPeriodTo,[ImplementationDate]=@ImplementationDate,
                                [__IsDraft]=0,[StatusID]=1 WHERE [Id] = @Id"
        DeleteCommand="Delete ia from IAs ia		                
						left join IAAttachments on ia.Id = IAAttachments.IAId
						left join IAModels on ia.Id = IAModels.IAId
						left join IAParts on ia.Id = IAParts.IAId           
	                    left join IAStations on ia.Id = IAStations.IAId
	                    left join IATasks on ia.Id = IATasks.IAId
	                    left join IATaskReports on IATasks.Id = IATaskReports.IATaskId
                        WHERE ia.Id = @Id">
        <SelectParameters>
            <asp:Parameter Name="Id" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <UpdateParameters>
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$dtDistributionDate" Name="DistributionDate" PropertyName="Value" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$cmbIAType" Name="IATypeId" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$txtNumber" Name="InternalEpcNumber" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$txtInfoNumber" Name="InfoNumber" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$htDescription" Name="Description" PropertyName="Html" Type="String" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$dtValidFrom" Name="ValidPeriodFrom" PropertyName="Value" Type="DateTime" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$dtValidTo" Name="ValidPeriodTo" PropertyName="Value" Type="DateTime" />
            <asp:ControlParameter ControlID="frmLayoutIrregAlt$ImplementationDate" Name="ImplementationDate" PropertyName="Value" Type="String" />
            <%--<asp:ControlParameter ControlID="frmLayoutIrregAlt$chkIsDynamicCheck" Name="IsDynamicCheck" PropertyName="Value" Type="Boolean" />--%>
            <%-- <asp:ControlParameter ControlID="frmLayoutIrregAlt$cmbApproverUser" Name="ApproverUserId" PropertyName="Value" Type="Int32" />
            --%>
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIAStation" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT 
        [Id],
        [IAId],
        [StationId],
        [AssemblySectionId]
         FROM [IAStations] 
        WHERE [IAId] = @IAId"
        InsertCommand="INSERT INTO IAStations(IAId,StationId,AssemblySectionId) VALUES (@IAId, @StationId, @AssemblySectionId)"
        DeleteCommand="DELETE FROM IAStations WHERE (Id = @Id)"
        UpdateCommand="UPDATE IAStations SET StationId = @StationId, AssemblySectionId = @AssemblySectionId WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
            <asp:Parameter Name="StationId" Type="Int32" />
            <asp:Parameter Name="AssemblySectionId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="AssemblySectionId" Type="Int32" />
            <asp:Parameter Name="StationId" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsInsertVPN" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        InsertCommand="Insert into IAPackingMonths (IAId, PackingMonth, CreatedDate, CreatedBy, ModifiedDate,ModifiedBy,IsDeleted)
                       values (@IAId,Concat((YEAR(@CreatedDate)),'',(MONTH(@CreatedDate))),@CreatedDate,@CreatedBy,@MD,@MB,@IsDeleted)"
        UpdateCommand="UPDATE IAPackingMonths SET IAId = @IAId, CreatedDate = @CreatedDate, ModifiedDate = @MD,ModifiedBy = @MB,IsDeleted=@IsDeleted WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIAPart" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT distinct [Id],[IAId],[PartNumber],[Description] FROM [IAParts] WHERE [IAId] = @IAId"
        InsertCommand="INSERT INTO IAParts(IAId, PartNumber, Description) VALUES (@IAId, @PartNumber, @Description)"
        DeleteCommand="DELETE FROM IAParts WHERE (Id = @Id)"
        UpdateCommand="UPDATE IAParts SET PartNumber = @PartNumber, Description = @Description WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
            <asp:Parameter Name="PartNumber" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="PartNumber" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIAPartDisplay" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT distinct [IAId],[PartNumber],[Description] FROM [IAParts] WHERE [IAId] = @IAId"
        InsertCommand="INSERT INTO IAParts(IAId, PartNumber, Description) VALUES (@IAId, @PartNumber, @Description)"
        DeleteCommand="DELETE FROM IAParts WHERE (Id = @Id)"
        UpdateCommand="UPDATE IAParts SET PartNumber = @PartNumber, Description = @Description WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
            <asp:Parameter Name="PartNumber" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="PartNumber" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIA" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select * from IAs WHERE [IAId] = @IAId"
        UpdateCommand="UPDATE StatusId SET StatusId = @StatusId WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="StatusId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="StatusId" Type="Int32" DefaultValue="0" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIAModel" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
SELECT A.[Id],A.[CatalogModelId], C.Model as ModelName FROM [IAModels] A
LEFT JOIN CatalogModels C ON C.Id = A.CatalogModelId
WHERE [IAId] = @IAId"
        InsertCommand="INSERT INTO IAModels(IAId, CatalogModelId) VALUES (@IAId, @CatalogModelId)"
        DeleteCommand="DELETE FROM IAModels WHERE (Id = @Id)"
        UpdateCommand="UPDATE IAModels SET CatalogModelId = @CatalogModelId WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
            <asp:Parameter Name="CatalogModelId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="CatalogModelId" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsAttachment" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[IAId],[FileName],[FilePath] FROM [IAAttachments] WHERE [IAId] = @IAId"
        InsertCommand="INSERT INTO IAAttachments(IAId, FileName, FilePath) VALUES (@IAId, @FileName, @FilePath)"
        DeleteCommand="DELETE FROM IAAttachments WHERE (Id = @Id)"
        UpdateCommand="UPDATE IAAttachments SET FileName = @FileName, FilePath = @FilePath WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
            <asp:Parameter Name="FileName" Type="String" />
            <asp:Parameter Name="FilePath" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="FileName" Type="String" />
            <asp:Parameter Name="FilePath" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIATask" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT 
        O.Name,
        O.OrderNo,
        A.[Id],
        [IAId],
        A.[OrganizationId] as OrganizationId,
		[AssignedUserId],
		[AssignedDate],
		[DelegateUserId],
		[DelegateDate],
		[CloseDate],
		[Task],
		[IATaskStatusId],
        convert(bit,1) FlagReport,
        convert(bit,1) FlagViewReport,
		u.FirstName + ' ' + u.LastName + ' (' + CONVERT(varchar(20), DelegateDate)+ ')' as DelegateTo
        FROM [IATasks] A 
        inner join Organizations O on A.OrganizationId = O.Id
        inner join IATaskStatus t on a.IATaskStatusId = t.Id
		left join Users u on a.DelegateUserId = u.Id
        WHERE [IAId] = @IAId
        ORDER BY O.OrderNo
        "
        InsertCommand="INSERT INTO IATasks(IAId, OrganizationId, AssignedUserId, DelegateUserId, DelegateDate, Task,IATaskStatusId) 
                       VALUES (@IAId, @OrganizationId, @AssignedUserId, @DelegateUserId, @DelegateDate, @Task,1)"
        DeleteCommand="DELETE FROM IATasks WHERE (Id = @Id)"
        UpdateCommand="UPDATE IATasks SET OrganizationId = @OrganizationId, AssignedUserId = @AssignedUserId,
                       DelegateUserId = @DelegateUserId, DelegateDate = @DelegateDate, Task = @Task WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IAId" Type="Int32" />
            <asp:Parameter Name="OrganizationId" Type="Int32" />
            <asp:Parameter Name="AssignedUserId" Type="Int32" />
            <asp:Parameter Name="DelegateUserId" Type="Int32" />
            <asp:Parameter Name="DelegateDate" Type="DateTime" />
            <asp:Parameter Name="Task" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="AssignedUserId" Type="Int32" />
            <asp:Parameter Name="DelegateDate" Type="DateTime" />
            <asp:Parameter Name="OrganizationId" Type="Int32" />
            <asp:Parameter Name="DelegateUserId" Type="Int32" />
            <asp:Parameter Name="Task" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIATaskReport" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT
            case when ([IATaskStatusId] = 2) then convert(bit,1) else convert(bit,0) end as chkClose,
            case when ([IATaskStatusId] = 1) then convert(bit,1) else convert(bit,0) end as chkOpen,
            [Id],[Reminder],[Report],[FileName],[FilePath],[IATaskStatusId],[ReportDate] 
            FROM [IATaskReports] WHERE [IATaskId] = @IATaskId"
        InsertCommand="INSERT INTO IATaskReports(IATaskStatusId, IATaskId, Reminder, Report, FileName, FilePath, ReportDate) VALUES (@IATaskStatusId, @IATaskId, @Reminder, @Report, @FileName, @FilePath, @ReportDate)"
        DeleteCommand="DELETE FROM IATaskReports WHERE (Id = @Id)"
        UpdateCommand="UPDATE IATaskReports SET IATaskStatusId = @IATaskStatusId, Reminder = @Reminder, Report = @Report, FileName = @FileName, FilePath = @FilePath WHERE (Id = @Id)">
        <SelectParameters>
            <asp:ControlParameter ControlID="TempGetIATaskId" Name="IATaskId" PropertyName="Text" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:ControlParameter ControlID="TempGetIATaskId" Name="IATaskId" PropertyName="Text" Type="Int32" />
            <asp:Parameter Name="Reminder" Type="String" />
            <asp:Parameter Name="Report" Type="String" />
            <asp:Parameter Name="FileName" Type="String" />
            <asp:Parameter Name="FilePath" Type="String" />
            <asp:Parameter Name="ReportDate" Type="DateTime" />
            <asp:Parameter Name="IATaskStatusId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Reminder" Type="String" />
            <asp:Parameter Name="Report" Type="String" />
            <asp:Parameter Name="FileName" Type="String" />
            <asp:Parameter Name="FilePath" Type="String" />
            <asp:Parameter Name="IATaskStatusId" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsIATaskReport2" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[Reminder],[Report],[FileName],[FilePath],[IATaskStatusId],[ReportDate] FROM [IATaskReports] WHERE [IATaskId] = @IATaskId"
        InsertCommand="INSERT INTO IATaskReports(IATaskStatusId, IATaskId, Reminder, Report, FileName, FilePath, ReportDate) VALUES (@IATaskStatusId, @IATaskId, @Reminder, @Report, @FileName, @FilePath, @ReportDate)"
        DeleteCommand="DELETE FROM IATaskReports WHERE (Id = @Id)"
        UpdateCommand="UPDATE IATaskReports SET IATaskStatusId = @IATaskStatusId, Reminder = @Reminder, Report = @Report, FileName = @FileName, FilePath = @FilePath WHERE (Id = @Id)">
        <SelectParameters>
            <%-- <asp:SessionParameter Name="IATaskId" Type="Int32" DefaultValue="0" SessionField="IATaskId" />--%>
            <asp:ControlParameter ControlID="TempGetIATaskId" Name="IATaskId" PropertyName="Text" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="IATaskId" Type="Int32" />
            <asp:Parameter Name="Reminder" Type="String" />
            <asp:Parameter Name="Report" Type="String" />
            <asp:Parameter Name="FileName" Type="String" />
            <asp:Parameter Name="FilePath" Type="String" />
            <asp:Parameter Name="ReportDate" Type="DateTime" />
            <asp:Parameter Name="IATaskStatusId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Reminder" Type="String" />
            <asp:Parameter Name="Report" Type="String" />
            <asp:Parameter Name="FileName" Type="String" />
            <asp:Parameter Name="FilePath" Type="String" />
            <asp:Parameter Name="IATaskStatusId" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsRemarksInfo" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
select 
t1.Id,
t1.IAId,	
t1.DelegateUserId,
t3.FirstName +' '+ t3.LastName as FullName,
t1.AssignedUserId,
t3.FirstName +' '+ t3.LastName as FullName,
t1.DelegateDate,
t1.OrganizationId,
t4.Name as OrganizationName,
t1.IATaskStatusId as StatusIATask,
t2.ReportDate,
t2.Report,
t2.Reminder
from IATasks t1
left join IATaskReports t2 on t2.IATaskId = t1.Id
left join Users t3 on t1.DelegateUserId = t3.Id 
left join Users t5 on t1.AssignedUserId = t5.Id 
left join Organizations t4 on t1.OrganizationId = t4.Id
WHERE t1.[IAId] = @IAId">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsRemarksInfoDelegate" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
select 
t1.Id,
t1.IAId,	
t1.DelegateUserId,
t3.FirstName +' '+ t3.LastName as FullName,
t1.AssignedUserId,
t3.FirstName +' '+ t3.LastName as FullName,
t1.DelegateDate,
t1.OrganizationId,
t4.Name as OrganizationName,
t1.IATaskStatusId as StatusIATask,
t2.ReportDate,
t2.Report,
t2.Reminder
from IATasks t1
left join IATaskReports t2 on t2.IATaskId = t1.Id
left join Users t3 on t1.DelegateUserId = t3.Id 
left join Users t5 on t1.AssignedUserId = t5.Id 
left join Organizations t4 on t1.OrganizationId = t4.Id
WHERE t1.[IAId] = @IAId and t1.DelegateUserId = @DelegateUserId">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="DelegateUserId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsRemarksInfoAssigned" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
select 
t1.Id,
t1.IAId,	
t1.DelegateUserId,
t3.FirstName +' '+ t3.LastName as FullName,
t1.AssignedUserId,
t3.FirstName +' '+ t3.LastName as FullName,
t1.DelegateDate,
t1.OrganizationId,
t4.Name as OrganizationName,
t1.IATaskStatusId as StatusIATask,
t2.ReportDate,
t2.Report,
t2.Reminder
from IATasks t1
left join IATaskReports t2 on t2.IATaskId = t1.Id
left join Users t3 on t1.DelegateUserId = t3.Id 
left join Users t5 on t1.AssignedUserId = t5.Id 
left join Organizations t4 on t1.OrganizationId = t4.Id
WHERE t1.[IAId] = @IAId and t1.AssignedUserId = @AssignedUserId">
        <SelectParameters>
            <asp:Parameter Name="IAId" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="AssignedUserId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
