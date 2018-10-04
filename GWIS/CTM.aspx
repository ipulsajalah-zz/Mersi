<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="CTM.aspx.cs" Inherits="Ebiz.WebForm.custom.GWIS.GWISClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    CTM
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!DOCTYPE html>
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <link rel="stylesheet" href="../../Content/GWIS/Contents/select2.min.css">
        <link rel="stylesheet" href="../../Content/GWIS/Contents/bootstrap-datetimepicker.min.css">
        <link rel="stylesheet" href="../../Content/GWIS/Contents/dataTables.bootstrap.min.css">
        <link rel="stylesheet" href="../../Content/GWIS/Contents/jquery.dataTables.min.css">
        <link rel="stylesheet" href="../../Content/GWIS/Contents/trumbowyg.min.css">
        <title>TIDSCV-CTM</title>
        <style>
            body {
                margin-top: 20px;
                min-height: 100vh;
                font-size: 12px !important;
                font-family: "Segoe UI",Helvetica,Arial,sans-serif;
            }

            .dropdown-filter, content-type-dropdown {
                width: 100%;
            }

            ul.nav-tabs.nav-justified > li.active > a {
                background-color: #428bca;
                color: #ffffff;
                font-size: 14px;
            }

            ul.nav-tabs.nav-justified > li > a {
                color: #333;
                font-size: 14px;
            }

            .datepicker {
                background-color: #fff;
                color: #333;
            }

            .full-width {
                width: 100%;
            }

            .nopadding {
                padding: 0 !important;
            }

            .noborder {
                border: 0 !important;
                -webkit-box-shadow: inset 0 0 0 rgba(0,0,0,0) !important;
            }

            .thumb {
                width: 100%;
                height: 100%;
                border: 1px solid #000;
                margin: 10px 5px 0 0;
            }

            .thumb-add {
                width: 100px;
                height: 100px;
                border: 1px solid #000;
                margin: 10px 5px 0 0;
            }

            .panel-default > .panel-heading {
                color: #333;
                background-color: #fff;
                border-color: #e4e5e7;
                padding: 0;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
            }

                .panel-default > .panel-heading a {
                    display: block;
                    padding: 10px 15px;
                }

                    .panel-default > .panel-heading a:after {
                        content: "";
                        position: relative;
                        top: 1px;
                        display: inline-block;
                        font-family: 'Glyphicons Halflings';
                        font-style: normal;
                        font-weight: 400;
                        line-height: 1;
                        -webkit-font-smoothing: antialiased;
                        -moz-osx-font-smoothing: grayscale;
                        float: right;
                        transition: transform .25s linear;
                        -webkit-transition: -webkit-transform .25s linear;
                    }

                    .panel-default > .panel-heading a[aria-expanded="true"] {
                        background-color: #eee;
                    }

                        .panel-default > .panel-heading a[aria-expanded="true"]:after {
                            content: "\2212";
                            -webkit-transform: rotate(180deg);
                            transform: rotate(180deg);
                        }

                        .panel-default > .panel-heading a[aria-expanded="true"]:before {
                            content: "";
                            -webkit-transform: rotate(180deg);
                            transform: rotate(180deg);
                        }

                    .panel-default > .panel-heading a[aria-expanded="false"]:after {
                        content: "\002b";
                        -webkit-transform: rotate(90deg);
                        transform: rotate(90deg);
                    }

                    .panel-default > .panel-heading a[aria-expanded="false"]:before {
                        content: "";
                        -webkit-transform: rotate(90deg);
                        transform: rotate(90deg);
                    }

            .accordion-option {
                width: 100%;
                float: left;
                clear: both;
                margin: 15px 0;
            }

                .accordion-option .title {
                    font-size: 20px;
                    font-weight: bold;
                    float: left;
                    padding: 0;
                    margin: 0;
                }

                .accordion-option .toggle-accordion {
                    float: right;
                    font-size: 16px;
                    color: #6a6c6f;
                }

                    .accordion-option .toggle-accordion:before {
                        content: "Expand All";
                    }

                    .accordion-option .toggle-accordion.active:before {
                        content: "Collapse All";
                    }

            /*Icon and event button*/
            .stat-draft {
                color: red;
            }

                .stat-draft:hover {
                }

            .stat-evaluation {
                color: #d0ca05;
            }

            .stat-released {
                color: green;
            }

            .stat-used {
                color: blue;
            }

            .treeviewScroll {
                width: 90%;
                overflow-x: auto;
                white-space: nowrap;
                overflow-y: scroll;
                max-height: 400px;
                border: 1px solid #ddd;
            }

            .list-group {
                min-width: 313px;
                width: auto;
                display: inline-block;
            }

            .modalLoading {
                display: none;
                position: fixed;
                z-index: 1000;
                top: 0;
                left: 0;
                height: 100%;
                width: 100%;
                background: rgba( 255, 255, 255, .8 ) url('../../Content/Images/pIkfp.gif') 50% 50% no-repeat;
            }

            /* When the body has the loading class, we turn
               the scrollbar off with overflow:hidden */
            body.loading {
                overflow: hidden;
            }

                /* Anytime the body has the loading class, our
               modal element will be visible */
                body.loading .modalLoading {
                    display: block;
                }

            #radioBtn .notActive {
                color: #3276b1 !important;
                background-color: #fff !important;
            }

            .headertitle {
                text-align: left;
                color: #405778;
                font-size: 13px;
                font-weight: bold;
                letter-spacing: -0.5px;
                margin-left: 30px;
                font-style: normal;
            }

            .tabledata {
                border-spacing: 0px;
                border-collapse: collapse;
            }

            .rowhighlight:hover {
                background-color: orange !important;
            }

            .tabledata .fmea td,
            .tabledata .fmea th,
            .tabledata tfoot .total,
            .tabledata .fmeafoot td {
                padding-top: 5px;
                padding-bottom: 5px;
                border: 1px solid #bbb;
                text-align: center;
            }

            .tabledata .fmea th {
                font-weight: bold;
                color: #405778;
                background-color: #ddd;
                float: none;
            }

            .tabledata tfoot .total {
                font-weight: bold;
                font-size: 9pt;
                color: #405778;
            }

            .tabeldata thead .trans .tabletitleleft {
                text-align: left;
                font-weight: bold;
                font-size: 14px;
            }

            .tabledata thead .trans th {
                /*border-bottom: 1px solid #ccc;*/
                color: #808080;
                height: 28px;
                background-color: transparent;
                font-size: 15px;
            }

            .tabledata tbody tr td {
                width: auto;
                text-align: left;
                color: #222;
                padding: 5px 1em 5px 5px;
                border: 1px solid #bbb;
            }

            .remarks {
                max-width: 70px;
                word-wrap: break-word;
            }

            .tabledata tbody tr:nth-child(odd) {
                background: #EEE;
            }

            .inputan {
                font-family: 'Segoe UI';
                font-size: 9pt;
                margin-top: 1px;
            }

            .button {
                background-color: #f3f3f3;
                border: 1px solid #ccc;
                font-weight: bold;
                color: #405778;
                cursor: pointer;
                padding: 3px 5px;
            }

            tfoot {
                display: table-footer-group;
                vertical-align: middle;
                border-color: inherit;
            }

            .auto-style1 {
                width: 218px;
            }

            #modal, #modal1, #LP {
                display: none;
                background-color: #fff;
                padding: 10px 1em;
                border: 1px solid #ddd;
                min-width: 250px;
            }

                #modal .head, #modal1 .head, #LP .head {
                    float: left;
                    font-size: 12pt;
                    color: #405778;
                    font-weight: bold;
                }

            .modal-content-edit {
                margin-left: -160px;
                width: 1210px;
                margin-top: 150px;
            }

            .input-group {
                position: relative;
                display: table;
                border-collapse: collapse;
                top: 0px;
                left: 0px;
            }

            .BorderButton {
                border: none !important;
                background-color: white;
            }

            /*.dxsplLCC {
                overflow: hidden !important;
            }*/
        </style>
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
        </script>

    </head>
    <body>
        <dx:ASPxPopupControl ID="popupCTM" ClientInstanceName="popupCTM" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="CTM" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="100%" MinHeight="100%">
            <ContentStyle Paddings-Padding="0" />
            <ClientSideEvents Shown="onPopupShown" />
        </dx:ASPxPopupControl>
        <p></p>
        <div class="wrapper">
            <div class="content">
                <div class="row">
                    <div class="col-md-3" style="margin-left: -20px;">
                        <h6><b>Control Plan Data</b></h6>
                        <b>Packing Month</b>
                        <div class="input-group date" id="packingMonth" style="width: 90%;">
                           <select id="packingMonthVal"  class="form-control dropdown-month" style="width: 100%"></select>
                             <%--<input type="text" name="TextpackingMonth" id="packingMonthVal" class="form-control dropdown-filter" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>--%>
                        </div>
                        <br />
                        <div id="treeview" class="treeviewScroll"></div>
                        <br />
                    </div>

                    <table>
                        <tbody>
                            <tr>
                                <td rowspan="2" colspan="1">
                                    <img src="../../Content/Images/clock.png" style="width: 170%" /></td>
                                </td>
                                    <td rowspan="1" colspan="1"><cite class="headertitle">CYCLE TIME</cite></td>
                            </tr>
                            <tr>
                                <td rowspan="1" colspan="1">
                                    <cite style="font-size: 9px; font-weight: normal; color: dimgrey; margin-left: 30px;">Manage cycle time and number of manpower of each process in production
                                    </cite>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <div class="col-md-9 rightTab hidden" style="margin-left: -35px;">
                        <br />
                        <br />
                        <table>
                            <thead>
                                <tr class="trans">
                                    <th colspan="16" class="tabletitleleft" style="padding-left: 0px; font-size: 14px; padding-bottom: 5px;">
                                        <span id="tittleProject"></span>
                                    </th>
                                </tr>
                            </thead>
                        </table>

                        <table id="tblCycleTime" class="tabledata" cellspacing="0">
                            <thead>
                                <tr class="fmea">
                                    <th rowspan="2">Nr.</th>
                                    <th rowspan="2">Process Step</th>
                                    <th rowspan="2">MP</th>
                                    <th rowspan="2">POS</th>
                                    <th rowspan="2">Part Number</th>
                                    <th rowspan="2">CP</th>
                                    <th rowspan="2">Motion</th>
                                    <th colspan="3">Time Data Sample</th>
                                    <th colspan="3">Average</th>
                                    <th rowspan="2" style="max-width: 150px;">Remarks</th>
                                </tr>
                                <tr class="fmea">
                                    <th>1</th>
                                    <th>2</th>
                                    <th>3</th>
                                    <th>VA</th>
                                    <th>NVA</th>
                                    <th>VE</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="8" class=""></td>
                                    <td colspan="2" style="font-weight: bold; color: #405778; font-size: 9pt;" class="">TOTAL : </td>
                                    <td class="total" style="height: 20px">
                                        <span id="MainContent_Repeater_Data_Label_Total_VA">0</span>
                                    </td>
                                    <td class="total" style="height: 20px">
                                        <span id="MainContent_Repeater_Data_Label_Total_NVA">0</span>
                                    </td>
                                    <td class="total" style="height: 20px">
                                        <span id="MainContent_Repeater_Data_Label_Total_VE">0</span>
                                    </td>
                                    <td class="total" style="padding: 0px; background-color: green; height: 20px;">
                                        <span id="MainContent_Repeater_Data_Label_Total_All" style="color: white;">0</span>
                                    </td>
                                </tr>
                                <tr class="">
                                    <td class="auto-style1">
                                        <span id="MainContent_RequiredFieldValidator_TextBox_Nr" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_Nr" name="ctl00$MainContent$TextBox_Nr" class="inputan" style="width: 30px;" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RequiredFieldValidator_TextBox_ProcessStep" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_ProcessStep" name="ctl00$MainContent$TextBox_ProcessStep" class="inputan" style="width: 200px; margin-right: 2px; margin-left: 2px;" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RequiredFieldValidator_TextBox_Manpower" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_Manpower" name="ctl00$MainContent$TextBox_Manpower" class="inputan" style="width: 30px;" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RequiredFieldValidator_Dropdownlist_POS" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <select name="ctl00$MainContent$Dropdownlist_POS" id="MainContent_Dropdownlist_POS" class="inputan" onchange="SetValuePartNumber()" style="width: 40px; margin-left: 2px;">
                                            <option value="">-Select POS-</option>
                                        </select>
                                    </td>
                                    <td>
                                        <span id="MainContent_RequiredFieldValidator_TextBox_PartNumber" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_PartNumber" name="ctl00$MainContent$TextBox_PartNumber" class="inputan" style="width: 120px; margin-left: 3px; margin-right: 2px;" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RequiredFieldValidator_Dropdownlist_CP" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <select name="ctl00$MainContent$Dropdownlist_CP" id="MainContent_Dropdownlist_CP" class="inputan" onchange="ChangeCP()" style="width: 40px;">
                                            <option value="" selected="selected">-Select CP-</option>
                                            <option value="A">A-Setup tool/Jig (VE)</option>
                                            <option value="B">B-Preparation (NVA)</option>
                                            <option value="C">C-Moving (NVA)</option>
                                            <option value="D">D-Process (VA)</option>
                                            <option value="E">E-Checking (NVA)</option>
                                            <option value="K">K-Kitting (VA)</option>
                                        </select>
                                    </td>
                                    <td>
                                        <input type="text" id="MainContent_Textbox_Motion" name="ctl00$MainContent$Textbox_Motion" class="inputan" style="width: 45px; margin-top: 13px; margin-left: 3px;" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RegularExpressionValidator_TextBox_1" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_1" name="ctl00$MainContent$TextBox_1" class="inputan" style="width: 40px; margin-left: 1px;" onkeyup="" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RegularExpressionValidator_TextBox_2" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_2" name="ctl00$MainContent$TextBox_2" class="inputan" style="width: 40px; margin-left: 1px;" onkeyup="" />
                                    </td>
                                    <td>
                                        <span id="MainContent_RegularExpressionValidator_TextBox_3" style="color: red; display: none;">This field cannot be empty !</span>
                                        <br />
                                        <input type="text" id="MainContent_TextBox_3" name="ctl00$MainContent$TextBox_3" class="inputan" style="width: 40px; margin-left: 1px;" onkeyup="" />
                                    </td>
                                    <td>
                                        <input type="text" id="MainContent_TextBox_VA" name="ctl00$MainContent$TextBox_VA" class="" disabled="disabled" style="width: 40px; margin-top: 13px; margin-left: 2px;" />
                                    </td>
                                    <td>
                                        <input type="text" id="MainContent_TextBox_NVA" name="ctl00$MainContent$TextBox_NVA" class="" disabled="disabled" style="width: 40px; margin-top: 13px; margin-left: 2px;" />
                                    </td>
                                    <td>
                                        <input type="text" id="MainContent_TextBox_VE" name="ctl00$MainContent$TextBox_EA" class="" disabled="disabled" style="width: 40px; margin-top: 13px; margin-left: 2px;" />
                                    </td>
                                    <td>
                                        <textarea name="ctl00$MainContent$TextBox_Remarks" rows="2" cols="20" id="MainContent_TextBox_Remarks" class="inputan" style="margin-top: 2px; margin-left: 7px;"></textarea>
                                    </td>
                                    <td colspan="2">
                                        <input type="button" name="ctl00$MainContent$Button_Save" value="Save" onclick="" id="MainContent_Button_Save" class="inputan button" style="margin-left: 5px;" />
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal pop up update -->
        <div class="modal fade" tabindex="-1" role="dialog" id="modale-default">
            <div id="modale-type" class="modal-dialog modal-lg" role="document">
                <div class="modal-content modal-content-edit">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modale-title">Modal title</h4>
                    </div>
                    <div class="content">
                        <div class="modal-body" id="modale-body">
                            <table id="table2" class="tabledata" cellspacing="0">
                                <thead>
                                    <tr class="fmea">
                                        <th rowspan="2">Nr.</th>
                                        <th rowspan="2">Process Step</th>
                                        <th rowspan="2">MP</th>
                                        <th rowspan="2">POS</th>
                                        <th rowspan="2">Part Number</th>
                                        <th rowspan="2">CP</th>
                                        <th rowspan="2">Motion</th>
                                        <th colspan="3">Time Data Sample</th>
                                        <th colspan="3">Average</th>
                                        <th rowspan="2">Remarks</th>
                                    </tr>
                                    <tr class="fmea">
                                        <th>1</th>
                                        <th>2</th>
                                        <th>3</th>
                                        <th>VA</th>
                                        <th>NVA</th>
                                        <th>VE</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="">
                                        <td class="auto-style1">
                                            <span id="MainContent_RequiredFieldValidator_TextBox_Nr_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="hidden" id="MainContent_TextBox_id_Edit">
                                            <input type="text" id="MainContent_TextBox_Nr_Edit" name="ctl00$MainContent$TextBox_Nr_Edit" class="inputan" style="width: 30px;" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RequiredFieldValidator_TextBox_ProcessStep_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="text" id="MainContent_TextBox_ProcessStep_Edit" name="ctl00$MainContent$TextBox_ProcessStep_Edit" class="inputan" style="width: 200px; margin-right: 2px; margin-left: 2px;" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RequiredFieldValidator_TextBox_Manpower_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="text" id="MainContent_TextBox_Manpower_Edit" name="ctl00$MainContent$TextBox_Manpower_Edit" class="inputan" style="width: 30px;" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RequiredFieldValidator_Dropdownlist_POS_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <select name="ctl00$MainContent$Dropdownlist_POS_Edit" id="MainContent_Dropdownlist_POS_Edit" class="inputan" onchange="SetValuePartNumberEdit()" style="width: 40px; margin-left: 2px;">
                                                <option value="">-Select POS-</option>
                                            </select>
                                        </td>
                                        <td>
                                            <span id="MainContent_RequiredFieldValidator_TextBox_PartNumber_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="text" id="MainContent_TextBox_PartNumber_Edit" name="ctl00$MainContent$TextBox_PartNumber_Edit" class="inputan" style="width: 120px; margin-left: 3px; margin-right: 2px;" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RequiredFieldValidator_Dropdownlist_CP_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <select name="ctl00$MainContent$Dropdownlist_CP_Edit" id="MainContent_Dropdownlist_CP_Edit" class="inputan" onchange="ChangeCPEdit()" style="width: 40px;">
                                                <option value="" selected="selected">-Select CP-</option>
                                                <option value="A">A-Setup tool/Jig (VE)</option>
                                                <option value="B">B-Preparation (NVA)</option>
                                                <option value="C">C-Moving (NVA)</option>
                                                <option value="D">D-Process (VA)</option>
                                                <option value="E">E-Checking (NVA)</option>
                                                <option value="K">K-Kitting (VA)</option>
                                            </select>
                                        </td>
                                        <td>
                                            <input type="text" id="MainContent_Textbox_Motion_Edit" name="ctl00$MainContent$Textbox_Motion_Edit" class="inputan" style="width: 45px; margin-top: 13px; margin-left: 3px;" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RegularExpressionValidator_TextBox_1_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="text" id="MainContent_TextBox_1_Edit" name="ctl00$MainContent$TextBox_1_Edit" class="inputan" style="width: 40px; margin-left: 1px;" onkeyup="" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RegularExpressionValidator_TextBox_2_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="text" id="MainContent_TextBox_2_Edit" name="ctl00$MainContent$TextBox_2_Edit" class="inputan" style="width: 40px; margin-left: 1px;" onkeyup="" />
                                        </td>
                                        <td>
                                            <span id="MainContent_RegularExpressionValidator_TextBox_3_Edit" style="color: red; display: none;">This field cannot be empty !</span>
                                            <br />
                                            <input type="text" id="MainContent_TextBox_3_Edit" name="ctl00$MainContent$TextBox_3_Edit" class="inputan" style="width: 40px; margin-left: 1px;" onkeyup="" />
                                        </td>
                                        <td>
                                            <input type="text" id="MainContent_TextBox_VA_Edit" name="ctl00$MainContent$TextBox_VA_Edit" class="" disabled="disabled" style="width: 40px; margin-top: 13px; margin-left: 2px;" />
                                        </td>
                                        <td>
                                            <input type="text" id="MainContent_TextBox_NVA_Edit" name="ctl00$MainContent$TextBox_NVA_Edit" class="" disabled="disabled" style="width: 40px; margin-top: 13px; margin-left: 2px;" />
                                        </td>
                                        <td>
                                            <input type="text" id="MainContent_TextBox_VE_Edit" name="ctl00$MainContent$TextBox_EA_Edit" class="" disabled="disabled" style="width: 40px; margin-top: 13px; margin-left: 2px;" />
                                        </td>
                                        <td>
                                            <textarea name="ctl00$MainContent$TextBox_Remarks_Edit" rows="2" cols="20" id="MainContent_TextBox_Remarks_Edit" class="inputan" style="margin-top: 2px; margin-left: 7px;"></textarea>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer" style="text-align: center;">
                        <input type="button" name="ctl00$MainContent$Button_Save_Edit" value="Save" onclick="" id="MainContent_Button_Save_Edit" class="inputan button" style="margin-left: 5px;" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <!-- modal pop up Delete -->
        <div class="modal fade" tabindex="-1" role="dialog" id="modale-default-delete">
            <div id="modale-type-delete" class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modale-title-delete">Modal title</h4>
                    </div>
                    <div class="content">
                        <input type="hidden" id="MainContent_TextBox_id_Delete" />
                        <div class="modal-body" id="modale-body-delete">
                            
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="modale-left-button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                        <button type="button" id="modale-right-button" class="btn btn-primary" data-dismiss="modal">OK</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <!-- modal pop up notif -->
        <div class="modal fade" tabindex="-1" role="dialog" id="modale-default-notif">
            <div id="modale-type-notif" class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modale-title-notif"></h4>
                    </div>
                    <div class="content">
                        <div class="modal-body" id="modale-body-notif">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="modale-right-button-notif" class="btn btn-primary" data-dismiss="modal">OK</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <div class="modalLoading"></div>

        <script src="../../Content/GWIS/Scripts/bootstrap-treeview.js"></script>
        <script src="../../Content/GWIS/Scripts/moment-with-locales.js"></script>
        <script src="../../Content/GWIS/Scripts/moment-with-locales.min.js"></script>
        <script src="../../Content/GWIS/Scripts/jquery.dataTables.min.js"></script>
        <script src="../../Content/GWIS/Scripts/dataTables.bootstrap.min.js"></script>
        <script src="../../Content/GWIS/Scripts/select2.min.js"></script>
        <script src="../../Content/GWIS/Scripts/bootstrap-datetimepicker.min.js"></script>
        <script src="../../Content/GWIS/Scripts/trumbowyg.min.js"></script>

        <script type="text/javascript">
            var jsonData, selectedDate, tblAssyCat;
            var uriPath = "/api";
            var titleProject = [];
            var vRows = "";
            var AssemblySection = [];
            var ProcessIdTree = "";
            Array.prototype.unique = function () {
                return this.filter(function (value, index, self) {
                    return self.indexOf(value) === index;
                });
            }

            $body = $("body");
            $(document).on({
                ajaxStart: function () { $body.addClass("loading"); },
                ajaxStop: function () { $body.removeClass("loading"); }
            });

            function getAsyncAPI(url) {
                return $.ajax({
                    url: uriPath.concat(url),
                    type: "GET",
                    dataType: 'json',
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); } //set tokenString before send
                });
            }

            function getInitDataFromServer() {
                var def = $.Deferred();
                var responses = new Object();
                getAsyncAPI("/list/view/spGWISGetHeader")
                    .then(function (ret) {
                        responses.trees = ret && ret.rows;
                        return getAsyncAPI("/list/view/spPackingMonth");
                    }, function (err) {
                        console.log("error on getting headerTreeview. Err : ", err);
                        return getAsyncAPI("/list/view/spPackingMonth");
                    })
                    .then(function (ret) {
                        responses.packingmonth = ret && ret.rows;
                    }, function (err) {
                        console.log("error on getting station lookup. Err : ", err);
                    })
                    .always(function () {
                        def.resolve(responses);
                    });
                return def.promise();
            }

            function parseDataFromServer() {
                getInitDataFromServer()
                    .done(function (arr) {
                        jsonData = arr.trees;

                        var arrEach = [], defaultCaption = null, idField = '', captionField = '';

                        $(".dropdown-month").each(function (i, elm) {
                            switch ($(elm).attr("id").toLowerCase()) {
                                case "packingmonthval":
                                    defaultCaption = "-Select Packing Month";
                                    arrEach = arr.packingmonth || [];//[{ id: "76", name: "Left" }, { id: "82", name: "Right" }, { id: "78", name: "Neutral" }];
                                    idField = "pm";
                                    captionField = "pm";
                                    break;
                                default:
                                    defaultCaption = null;
                                    break;
                            }
                            if (defaultCaption) {
                                $(elm).empty();
                                if ($(elm).attr('id').toLowerCase() != 'packingmonthval') {
                                    $(elm).append($("<option></option>").val('default').html(defaultCaption));
                                }
                                arrEach.forEach(function (row, rowNum) {
                                    $(elm).append($("<option></option>").val(row[idField]).html(row[captionField]));
                                })
                                if ($(elm).attr('id').toLowerCase() == 'packingmonthval') {
                                    selectedDate = moment($("#packingMonthVal").val().concat("01"), "YYYYMMDD");
                                }
                            }
                        });

                        loadTreeview();
                    })
                    .fail(function (err) {
                        console.log("deferred call err : ", err);
                    })
            }

            function getDetailProcess(processId) {
                $("#tblCycleTime tbody").html("");
                getAsyncAPI("/list/view/vGetCycleTime?ProsessId=".concat(processId))
                    .then(function (ctm) {
                        if (ctm && ctm.rows) {
                            dttCycleTime(ctm.rows);
                        }
                        return getAsyncAPI("/list/view/vGetCycleTimePartNo?ProcessId=".concat(processId));
                    }, function (err) {
                        console.log("Error getting CTM Process; Err : ", err);
                        return getAsyncAPI("/list/view/vGetCycleTimePartNo?ProcessId=".concat(processId));
                    })
                .then(function (partNo) {
                    if (partNo && partNo.rows) {
                        GetValuePosPartNo(partNo.rows);
                    }
                }, function (err) {
                    console.log("Error getting Value Option; Err : ", err);
                });
                sumAverageOption();
                ClearTextEditor();
            }

            function loadTreeview() {
                var tvs = jsonData.filter(function (obj) {
                    //return selectedDate.isBetween(moment(obj.validFrom), moment(obj.validTo), "[]")
                    return selectedDate.isBetween(moment(obj.validFrom), moment(obj.validTo), "days", "[]")
                })
                tvData = [];
                if (tvs.length && tvs[0]) {
                    tvs.forEach(function (row, rowNum) {

                        var gwis, sect, stat;

                        var findGwis = $.grep(tvData, function (obj) {
                            return obj.text === row.model;
                        });
                        if (findGwis.length < 1) {
                            gwis = new Object({ "text": row.model, "selectable": false, "tags": ["gwisTypeId", row.gwisTypeId], "nodes": [] });
                            tvData.push(gwis);
                            gwis = tvData[tvData.length - 1];
                        }
                        else
                            gwis = findGwis[0];

                        var findSect = $.grep(gwis.nodes, function (obj) {
                            return obj.text === row.assemblySectionName;
                        });

                        if (findSect.length < 1) {
                            sect = new Object({ "text": row.assemblySectionName, "selectable": false, "tags": ["assemblySectionId", row.assemblySectionId], "nodes": [] });
                            gwis.nodes.push(sect);
                            sect = gwis.nodes[gwis.nodes.length - 1];
                        }
                        else
                            sect = findSect[0];

                        var findStat = $.grep(sect.nodes, function (obj) {
                            return obj.text === row.stationName;
                        })
                        if (findStat.length < 1) {
                            stat = new Object({ "text": row.stationName, "selectable": false, "tags": ["stationId", row.stationId], "nodes": [] });
                            sect.nodes.push(stat);
                            stat = sect.nodes[sect.nodes.length - 1];
                        }
                        else
                            stat = findStat[0];

                        var findProc = $.grep(stat.nodes, function (obj) {
                            return obj.text === row.processNo;
                        })

                        if (findProc.length < 1) {
                            var icon = "<span class='glyphicon glyphicon-asterisk ";
                            if (row.status == 1) {
                                icon = icon + " stat-draft'></span>";
                            } else if (row.status == 2) {
                                icon = icon + " stat-evaluation'></span>";
                            } else if (row.status == 3) {
                                icon = icon + " stat-released' ></span>";
                            } else {
                                icon = icon + " stat-used' ></span>";
                            }
                            stat.nodes.push(new Object({ "text": icon.concat('&nbsp;', row.processNo, ':\r\n', row.processName), "tags": ['processId', row.id] }))
                        }
                    });
                }
                $("#treeview").treeview({ data: tvData, highlightSearchResults: false });
                $("#treeview").treeview('collapseAll', { silent: true });
                $("#treeview").on("nodeSelected", function (event, node) {
                    if (node.tags && node.tags.length === 2) {
                        ProcessIdTree = node.tags[1];
                        //var tittleSelect = $("<div>").html(node.text).text();

                        var vDate = moment($("#packingMonthVal").val());
                        var titleShow = "";

                        //titleShow = titleShow.concat(vDate.format("YYYY"), vDate.format("MM"), " / ");

                        titleShow = titleShow.concat(vDate.format("YYYY"), " / ");

                        titleProject.forEach(function (vall) {
                            titleShow = titleShow.concat(vall, " / ")
                        });

                        titleShow = titleShow.concat($("li[data-nodeId='".concat(node.nodeId, "']")).text().trim());

                        $("#tittleProject").html(titleShow);
                        $(".rightTab").removeClass('hidden');
                        getDetailProcess(node.tags[1]);
                    }
                })
                $("#treeview").on("nodeExpanded", function (event, node) {
                    var sectionNodes = $('#treeview').treeview('search', ["(.*?)", {
                        ignoreCase: true,     // case insensitive
                        exactMatch: false,    // like or equals
                        revealResults: false,  // dont matching nodes
                    }]).filter(function (obj) { return obj.parentId == node.parentId && obj.nodeId !== node.nodeId })

                    var IndexTitleProject = $("li[data-nodeId='".concat(node.nodeId, "']")).children('span.indent').length;
                    titleProject[IndexTitleProject] = node.text;

                    $('#treeview').treeview('collapseNode', [sectionNodes, { silent: true, ignoreChildren: false }]);

                    if (node.tags[0] === 'stationId') {

                        $('#treeview li[data-nodeid='.concat(node.nodeId, ']')).nextUntil($('li span.expand-icon').parent('li'), 'li').each(function (i, elm) {
                            const origin = $(elm).text();
                            const index = origin.indexOf("[");
                            const text = origin.split("] ")[1];
                            const status = origin.split("]")[0].substr(1, 1);
                            $(elm).text().replace(origin, text);
                        })
                    }
                })
            }

            //$("#packingMonth").on("dp.hide", function (e) {
            //    if (moment("1-".concat($("#packingMonthVal").val()), "D-MMMM-YYYY").isValid()) {
            //        selectedDate = moment("1-".concat($("#packingMonthVal").val()), "D-MMMM-YYYY");
            //        if (jsonData && jsonData.length > 0)
            //            loadTreeview();
            //    }
            //});

            $("#packingMonth").on("change", function (e) {
                //if (moment("1-".concat($("#packingMonthVal").val()), "D-MMMM-YYYY").isValid()) {
                selectedDate = moment($("#packingMonthVal").val().concat("01"), "YYYYMMDD");
                //if ($('#listGwisType a.active').length > 0)
                if (jsonData && jsonData.length > 0)
                    loadTreeview();
                //}
            });

            $(function () {
                $(".control-type-dropdown").select2();
                $(".content-type-dropdown").select2();
                
                parseDataFromServer();
                $(".dropdown-month").select2();
                //$('#packingMonth').datetimepicker({
                //    viewMode: 'months',
                //    format: 'MMMM-YYYY'
                //});
                $(".date-time-picker").datetimepicker({
                    viewMode: 'days',
                    format: 'YYYY-MM-DD'
                });
                //$('#packingMonthVal').val(moment().format("MMMM-YYYY"));
                //$('#packingMonth').trigger("dp.hide");
                //$("#packingMonth").show();
            })

            function dttCycleTime(BindDataTable) {
                for (var i = 0; i < BindDataTable.length; i++) {
                    if (i % 2 == 0) {
                        vRows = "<tr class='rowhighlight even'>";
                    } else {
                        vRows = "<tr class='rowhighlight odd'>";
                    }
                    vRows += "<td class='NoTable'>".concat(i + 1, "</td>");

                    $.each(BindDataTable[i], function (name, value) {
                        if (name == "prosessId" || name == "id") {
                            vRows += "<td class='hidden ".concat(name.trim(), "'>", value, "</td>");
                        } else {
                            vRows += "<td class='".concat(name.trim(), "'>", value, "</td>");
                        }
                    });
                    vRows += "<td class='BorderButton'><input type='button' id='Button_Edit_Data' class = 'inputan button' onClick='GetDataEdit(this)' value='Edit'></td><td class='BorderButton'><input type='button' id='Button_Delete_Data' class = 'inputan button action-delete' onclick='DeleteDataCTM(this)' value='Delete' style='margin-left:-7px;'></td></tr>";
                    $("#tblCycleTime > tbody").append(vRows);
                }
                sumAverageOption();
            }

            function ChangeCP() {
                if ($("#MainContent_Dropdownlist_CP").val() == "D" || $("#MainContent_Dropdownlist_CP").val() == "K") {
                    $("#MainContent_TextBox_VA").prop("disabled", false);
                    $("#MainContent_TextBox_NVA").prop("disabled", true);
                    $("#MainContent_TextBox_VE").prop("disabled", true);
                    $("#MainContent_TextBox_NVA").val("");
                    $("#MainContent_TextBox_VE").val("");
                } else if ($("#MainContent_Dropdownlist_CP").val() == "B" || $("#MainContent_Dropdownlist_CP").val() == "C" || $("#MainContent_Dropdownlist_CP").val() == "E") {
                    $("#MainContent_TextBox_NVA").prop("disabled", false);
                    $("#MainContent_TextBox_VA").prop("disabled", true);
                    $("#MainContent_TextBox_VE").prop("disabled", true);
                    $("#MainContent_TextBox_VA").val("");
                    $("#MainContent_TextBox_VE").val("");
                } else if ($("#MainContent_Dropdownlist_CP").val() == "A") {
                    $("#MainContent_TextBox_NVA").prop("disabled", true);
                    $("#MainContent_TextBox_VA").prop("disabled", true);
                    $("#MainContent_TextBox_VE").prop("disabled", false);
                    $("#MainContent_TextBox_NVA").val("");
                    $("#MainContent_TextBox_VA").val("");
                } else {
                    $("#MainContent_TextBox_NVA").prop("disabled", true);
                    $("#MainContent_TextBox_VA").prop("disabled", true);
                    $("#MainContent_TextBox_VE").prop("disabled", true);
                    $("#MainContent_TextBox_NVA").val("");
                    $("#MainContent_TextBox_VA").val("");
                    $("#MainContent_TextBox_VE").val("");
                }
            }

            function ChangeCPEdit() {
                if ($("#MainContent_Dropdownlist_CP_Edit").val() == "D" || $("#MainContent_Dropdownlist_CP_Edit").val() == "K") {
                    $("#MainContent_TextBox_VA_Edit").prop("disabled", false);
                    $("#MainContent_TextBox_NVA_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VE_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_NVA_Edit").val("");
                    $("#MainContent_TextBox_VE_Edit").val("");
                } else if ($("#MainContent_Dropdownlist_CP_Edit").val() == "B" || $("#MainContent_Dropdownlist_CP_Edit").val() == "C" || $("#MainContent_Dropdownlist_CP_Edit").val() == "E") {
                    $("#MainContent_TextBox_NVA_Edit").prop("disabled", false);
                    $("#MainContent_TextBox_VA_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VE_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VA_Edit").val("");
                    $("#MainContent_TextBox_VE_Edit").val("");
                } else if ($("#MainContent_Dropdownlist_CP_Edit").val() == "A") {
                    $("#MainContent_TextBox_NVA_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VA_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VE_Edit").prop("disabled", false);
                    $("#MainContent_TextBox_NVA_Edit").val("");
                    $("#MainContent_TextBox_VA_Edit").val("");
                } else {
                    $("#MainContent_TextBox_NVA_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VA_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_VE_Edit").prop("disabled", true);
                    $("#MainContent_TextBox_NVA_Edit").val("");
                    $("#MainContent_TextBox_VA_Edit").val("");
                    $("#MainContent_TextBox_VE_Edit").val("");
                }
            }

            function GetDataEdit(currents) {
                $('#modale-default').modal('show');
                $("#modale-title").html("Edit Cycle Time");

                var parent = $(currents).closest("tr");
                $("#MainContent_TextBox_id_Edit").val(parent.children("td.id").text());
                $("#MainContent_TextBox_Nr_Edit").val(parent.children("td.NoTable").text());
                $("#MainContent_TextBox_ProcessStep_Edit").val(parent.children("td.prosessStep").text());
                $("#MainContent_TextBox_Manpower_Edit").val(parent.children("td.mp").text());
                $("#MainContent_Dropdownlist_POS_Edit").val(parent.children("td.pos").text());
                $("#MainContent_TextBox_PartNumber_Edit").val(parent.children('td.partNo').text());
                $("#MainContent_Dropdownlist_CP_Edit").val(parent.children("td.cp").text());
                $("#MainContent_Textbox_Motion_Edit").val(parent.children("td.motion").text());
                $("#MainContent_TextBox_1_Edit").val(parent.children("td.tdS_1").text());
                $("#MainContent_TextBox_2_Edit").val(parent.children("td.tdS_2").text());
                $("#MainContent_TextBox_3_Edit").val(parent.children("td.tdS_3").text());
                $("#MainContent_TextBox_VA_Edit").val(parent.children("td.averageVA").text());
                $("#MainContent_TextBox_NVA_Edit").val(parent.children("td.averageNVA").text());
                $("#MainContent_TextBox_VE_Edit").val(parent.children("td.averageVE").text());
                $("#MainContent_TextBox_Remarks_Edit").val(parent.children("td.remarks").text());

                ChangeCPEdit();
            }

            function DeleteDataCTM(currents) {
                if ($("#Button_Delete_Data").hasClass('action-delete')) {
                    $('#modale-default-delete').modal('show');
                    $('#modale-right-button').prop('disabled', false);
                    //$('.modal-footer > .btn-danger').attr('object-id', $(this).attr('object-id'));
                    $('#modale-title-delete').html('Confirm delete');
                    $('#modale-body-delete').append('Are you sure want to delete this data ?');
                    $('#modale-left-button').html('Close');
                    $('#modale-right-button').html('Delete');
                    var parent = $(currents).closest("tr");
                    $("#MainContent_TextBox_id_Delete").val(parent.children("td.id").text());
                    //$('#modale-default').attr("data-sender", "parts-delete-data").modal('show');
                }
            }

            $("#MainContent_Button_Save").on("click", function (e) {
                if ($("#MainContent_TextBox_Nr").val() == "" || $("#MainContent_TextBox_ProcessStep").val() == "" || $("#MainContent_TextBox_Manpower").val() == "" || $("#MainContent_Dropdownlist_POS").val() == "" || $("#MainContent_TextBox_PartNumber").val() == "" || $("#MainContent_Dropdownlist_CP").val() == "") {
                    $("#MainContent_RequiredFieldValidator_TextBox_Nr").show();
                    $("#MainContent_RequiredFieldValidator_TextBox_ProcessStep").show();
                    $("#MainContent_RequiredFieldValidator_TextBox_Manpower").show();
                    $("#MainContent_RequiredFieldValidator_Dropdownlist_POS").show();
                    $("#MainContent_RequiredFieldValidator_TextBox_PartNumber").show();
                    $("#MainContent_RequiredFieldValidator_Dropdownlist_CP").show();
                } else {
                    var obj = new Object();
                    obj.ProsessId = ProcessIdTree;
                    obj.ProsessStep = $("#MainContent_TextBox_ProcessStep").val();
                    obj.MP = $("#MainContent_TextBox_Manpower").val();
                    obj.POS = $("#MainContent_Dropdownlist_POS").val();
                    obj.CP = $("#MainContent_Dropdownlist_CP").val();
                    obj.Motion = $("#MainContent_Textbox_Motion").val();
                    obj.TDS_1 = $("#MainContent_TextBox_1").val();
                    obj.TDS_2 = $("#MainContent_TextBox_2").val();
                    obj.TDS_3 = $("#MainContent_TextBox_3").val();
                    obj.AverageVA = $("#MainContent_TextBox_VA").val();
                    obj.AverageNVA = $("#MainContent_TextBox_NVA").val();
                    obj.AverageVE = $("#MainContent_TextBox_VE").val();
                    obj.PartNo = $("#MainContent_TextBox_PartNumber").val();
                    obj.Remarks = $("#MainContent_TextBox_Remarks").val();
                    obj.IsDeleted = false;

                    $.ajax({
                        url: '/api/list/insert/CycleTime',
                        type: "PUT",
                        data: obj,
                        beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                        success: function (data, status, xhr) {
                            $("#tblCycleTime tbody").html("");
                            ClearTextEditor();

                            $('#modale-default-notif').modal('show');
                            $("#modale-body-notif").html("Data Has Been Save !");
                            $("#modale-title-notif").html("Notification");

                            if (ProcessIdTree != "") {
                                getDetailProcess(ProcessIdTree);
                            }

                            $("#MainContent_RequiredFieldValidator_TextBox_Nr").hide();
                            $("#MainContent_RequiredFieldValidator_TextBox_ProcessStep").hide();
                            $("#MainContent_RequiredFieldValidator_TextBox_Manpower").hide();
                            $("#MainContent_RequiredFieldValidator_Dropdownlist_POS").hide();
                            $("#MainContent_RequiredFieldValidator_TextBox_PartNumber").hide();
                            $("#MainContent_RequiredFieldValidator_Dropdownlist_CP").hide();
                            $("#MainContent_TextBox_NVA").prop("disabled", true);
                            $("#MainContent_TextBox_VA").prop("disabled", true);
                            $("#MainContent_TextBox_VE").prop("disabled", true);
                        },
                        error: function (xhr) {
                            console.log(xhr);
                        }
                    });
                }
            });

            $("#MainContent_Button_Save_Edit").on("click", function (e) {
                if ($("#MainContent_TextBox_Nr_Edit").val() == "" || $("#MainContent_TextBox_ProcessStep_Edit").val() == "" || $("#MainContent_TextBox_Manpower_Edit").val() == "" || $("#MainContent_Dropdownlist_POS_Edit").val() == "" || $("#MainContent_TextBox_PartNumber_Edit").val() == "" || $("#MainContent_Dropdownlist_CP_Edit").val() == "") {
                    $("#MainContent_RequiredFieldValidator_TextBox_Nr_Edit").show();
                    $("#MainContent_RequiredFieldValidator_TextBox_ProcessStep_Edit").show();
                    $("#MainContent_RequiredFieldValidator_TextBox_Manpower_Edit").show();
                    $("#MainContent_RequiredFieldValidator_Dropdownlist_POS_Edit").show();
                    $("#MainContent_RequiredFieldValidator_TextBox_PartNumber_Edit").show();
                    $("#MainContent_RequiredFieldValidator_Dropdownlist_CP_Edit").show();
                } else {
                    $('#modale-default').modal('hide');
                    var CTMEditId = $("#MainContent_TextBox_id_Edit").val();
                    var obj = new Object();
                    obj.ProsessId = ProcessIdTree;
                    obj.ProsessStep = $("#MainContent_TextBox_ProcessStep_Edit").val();
                    obj.MP = $("#MainContent_TextBox_Manpower_Edit").val();
                    obj.POS = $("#MainContent_Dropdownlist_POS_Edit").val();
                    obj.CP = $("#MainContent_Dropdownlist_CP_Edit").val();
                    obj.Motion = $("#MainContent_Textbox_Motion_Edit").val();
                    obj.TDS_1 = $("#MainContent_TextBox_1_Edit").val();
                    obj.TDS_2 = $("#MainContent_TextBox_2_Edit").val();
                    obj.TDS_3 = $("#MainContent_TextBox_3_Edit").val();
                    obj.AverageVA = $("#MainContent_TextBox_VA_Edit").val();
                    obj.AverageNVA = $("#MainContent_TextBox_NVA_Edit").val();
                    obj.AverageVE = $("#MainContent_TextBox_VE_Edit").val();
                    obj.PartNo = $("#MainContent_TextBox_PartNumber_Edit").val();
                    obj.Remarks = $("#MainContent_TextBox_Remarks_Edit").val();
                    obj.IsDeleted = false;

                    $.ajax({
                        url: '/api/list/update/CycleTime/'.concat(CTMEditId),
                        type: "POST",
                        data: obj,
                        beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                        success: function (data, status, xhr) {
                            $("#tblCycleTime tbody").html("");
                            ClearTextEditor();

                            $('#modale-default-notif').modal('show');
                            $("#modale-body-notif").html("Data Has Been Update !");
                            $("#modale-title-notif").html("Notification");

                            if (ProcessIdTree != "") {
                                getDetailProcess(ProcessIdTree);
                            }

                            $("#MainContent_RequiredFieldValidator_TextBox_Nr_Edit").hide();
                            $("#MainContent_RequiredFieldValidator_TextBox_ProcessStep_Edit").hide();
                            $("#MainContent_RequiredFieldValidator_TextBox_Manpower_Edit").hide();
                            $("#MainContent_RequiredFieldValidator_Dropdownlist_POS_Edit").hide();
                            $("#MainContent_RequiredFieldValidator_TextBox_PartNumber_Edit").hide();
                            $("#MainContent_RequiredFieldValidator_Dropdownlist_CP_Edit").hide();
                        },
                        error: function (xhr) {
                            console.log(xhr);
                        }
                    });
                }
            });

            $("#modale-right-button").on("click", function (e) {
                var CTMDeletedId = $("#MainContent_TextBox_id_Delete").val();
                var obj = new Object();
                obj.IsDeleted = true;

                $.ajax({
                    url: '/api/list/update/CycleTime/'.concat(CTMDeletedId),
                    type: "POST",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        $("#tblCycleTime tbody").html("");

                        $('#modale-default-notif').modal('show');
                        $("#modale-body-notif").html("Data Has Been Delete !");
                        $("#modale-title-notif").html("Notification");
                        $('#modale-body-delete').empty();

                        if (ProcessIdTree != "") {
                            getDetailProcess(ProcessIdTree);
                        }
                    },
                    error: function (xhr) {
                        console.log(xhr);
                    }
                });
            });

            $("#modale-left-button").on("click", function (e) {
                $('#modale-body-delete').empty();
            });

            function SetValuePartNumber() {
                var ValuePos = $("#MainContent_Dropdownlist_POS option:selected").text();
                var arr1 = ValuePos.split("|");
                var arr2 = arr1[0].split(".")[1].trim();
                $("#MainContent_TextBox_PartNumber").val(arr2);
            }

            function SetValuePartNumberEdit() {
                var ValuePos = $("#MainContent_Dropdownlist_POS_Edit option:selected").text();
                var arr1 = ValuePos.split("|");
                var arr2 = arr1[0].split(".")[1].trim();
                $("#MainContent_TextBox_PartNumber_Edit").val(arr2);
            }

            function ClearTextEditor() {
                $("#MainContent_TextBox_Nr").val("");
                $("#MainContent_TextBox_ProcessStep").val("");
                $("#MainContent_TextBox_Manpower").val("");
                $("#MainContent_Dropdownlist_POS").val("");
                $("#MainContent_Dropdownlist_CP").val("");
                $("#MainContent_Textbox_Motion").val("");
                $("#MainContent_TextBox_1").val("");
                $("#MainContent_TextBox_2").val("");
                $("#MainContent_TextBox_3").val("");
                $("#MainContent_TextBox_VA").val("");
                $("#MainContent_TextBox_NVA").val("");
                $("#MainContent_TextBox_VE").val("");
                $("#MainContent_TextBox_PartNumber").val("");
                $("#MainContent_TextBox_Remarks").val("");

                $("#MainContent_TextBox_Nr_Edit").val("");
                $("#MainContent_TextBox_ProcessStep_Edit").val("");
                $("#MainContent_TextBox_Manpower_Edit").val("");
                $("#MainContent_Dropdownlist_POS_Edit").val("");
                $("#MainContent_Dropdownlist_CP_Edit").val("");
                $("#MainContent_Textbox_Motion_Edit").val("");
                $("#MainContent_TextBox_1_Edit").val("");
                $("#MainContent_TextBox_2_Edit").val("");
                $("#MainContent_TextBox_3_Edit").val("");
                $("#MainContent_TextBox_VA_Edit").val("");
                $("#MainContent_TextBox_NVA_Edit").val("");
                $("#MainContent_TextBox_VE_Edit").val("");
                $("#MainContent_TextBox_PartNumber_Edit").val("");
                $("#MainContent_TextBox_Remarks_Edit").val("");
            }

            function GetValuePosPartNo(BindDataOption) {
                var arrEach = [], defaultCaption = null, idField = '', captionField = '';
                var name = "";
                BindDataOption.forEach(function (row, rowNum) {
                    name = name.concat(row.pos_Actual, ". ", row.partNo, " | ", row.partDescription);
                    arrEach[rowNum] = { id: row.pos_Actual, name: name }
                });

                $("#MainContent_Dropdownlist_POS, #MainContent_Dropdownlist_POS_Edit").each(function (i, elm) {
                    $(elm).empty();
                    $(elm).append($("<option></option>").val('default').html("-Select Pos-"));
                    arrEach.forEach(function (row, rowNum) {
                        $(elm).append($("<option></option>").val(row.id).html(row.name));
                    })
                });
            }

            function sumAverageOption() {
                var SumAverageVA = 0;
                var SumAverageNVA = 0;
                var SumAverageVE = 0;
                var SumAverageTotal = 0;
                $("#tblCycleTime .averageVA").each(function () {
                    SumAverageVA += parseInt($(this).text())
                });

                $("#tblCycleTime .averageNVA").each(function () {
                    SumAverageNVA += parseInt($(this).text())
                });

                $("#tblCycleTime .averageVE").each(function () {
                    SumAverageVE += parseInt($(this).text())
                });

                $("#MainContent_Repeater_Data_Label_Total_VA").html(SumAverageVA);
                $("#MainContent_Repeater_Data_Label_Total_NVA").html(SumAverageNVA);
                $("#MainContent_Repeater_Data_Label_Total_VE").html(SumAverageVE);

                SumAverageTotal = SumAverageVA + SumAverageNVA + SumAverageVE;

                $("#MainContent_Repeater_Data_Label_Total_All").html(SumAverageTotal);
            }

        </script>
    </body>
</asp:Content>