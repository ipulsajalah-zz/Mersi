<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="GWISClient.aspx.cs" Inherits="Ebiz.WebForm.custom.GWIS.GWISClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    GWIS
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
        <title>TIDSCV-GWIS</title>
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
                width: 300px;
                height: 200px;
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
            .stat-draft:hover{

            }
            .stat-evaluation{
                color: #d0ca05; 
            }
            .stat-released{
                color: green; 
            }
            .stat-used{
                color:blue; 
            }
            .treeviewScroll{
                width: 100%;
                overflow-x: auto;
                white-space: nowrap;
                overflow-y: scroll;
                max-height: 400px;
                border: 1px solid #ddd;
            }
            .list-group{
                min-width: 313px;
                width: auto;
                display: inline-block;
            }

            .modalLoading {
                display:    none;
                position:   fixed;
                z-index:    1000;
                top:        0;
                left:       0;
                height:     100%;
                width:      100%;
                background: rgba( 255, 255, 255, .8 )
                            url('../../Content/Images/pIkfp.gif')
                            50% 50%
                            no-repeat;
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
            #radioBtn .notActive{
                color: #3276b1 !important;
                background-color: #fff !important;
            }
            .center-text {
                text-align:center;
            }
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
        <dx:ASPxPopupControl ID="popupGWIS" ClientInstanceName="popupGWIS" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="GWIS" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="100%" MinHeight="100%">
            <ContentStyle Paddings-Padding="0" />
            <ClientSideEvents Shown="onPopupShown" />
        </dx:ASPxPopupControl>
        <p></p>
        <div class="wrapper">
            <div class="content">
                <div class="row">
                    <div class="col-md-3">
                        <b>Packing Month</b>
                        <div class="input-group date" id="packingMonth" style="width:100%">
                            <select id="packingMonthVal"  class="form-control dropdown-month" style="width: 100%"></select>

                            <%--<input type="text" name="packingMonth" id="packingMonthVal" class="form-control dropdown-filter" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>--%>
                        </div>
                        <br />
                        <b>GWIS Processes</b>
                        <%--<div class="treeviewScroll">--%>
                        <div id="treeview" class="treeviewScroll"></div>
                        <%--</div>--%>
                        <br />
                        <b>Search Part :</b>
                         <div class="panel panel-primary" style="border-color:#ddd" >
                            <div class="panel-body">
                             Part No.<br />
                                <input type="text" id="partNoSearch"  class="form-control"/><br />
                                <br />
                            Page Catalog <br />
                                <input type="text" id="PageCatSearch" class="form-control" />
                                <br /><br />
                                <button type="button" class="btn btn-info"  id="searchPart">Search</button>
                            </div>
                         </div>
                        <br />
                        <b>Search Alteration :</b>
                         <div class="panel panel-primary" style="border-color:#ddd" >
                            <div class="panel-body">
                             Page Catalog.<br />
                                <select id="PageCatalogSearch"  class="content-type-dropdown" style="width: 100%"></select>
                                <br /><br />
                                <button type="button" class="btn btn-info"  id="searchbyModel">Search</button>
                            </div>
                         </div>
                        <br>
                        <button type="button" class="btn btn-success btn-lg" style="width: 100%" id="btnNewGWISPartsInfo"><span class="glyphicon glyphicon-plus"></span>New GWIS Process</button>
                        <br>
                        <br>
                        <%--<b>GWIS Type</b>
                        <div class="list-group" id="listGwisType">
                        </div>
                        <br>
                        <br>--%>
                    </div>
                    <div class="col-md-9 rightTab hidden">

                        <h3 class="text-center"><a href="#" target="_blank" id="tittleProject"></a></h3>
                        <ul class="nav nav-tabs nav-justified">
                            <li class="active"><a data-toggle="tab" href="#nav-infos">Master</a></li>
                            <li><a data-toggle="tab" href="#nav-parts">Parts</a></li>
                            <li><a data-toggle="tab" href="#nav-pictures">Pictures</a></li>
                            <li><a data-toggle="tab" href="#nav-materials">Consumption Materials</a></li>
                            <li><a data-toggle="tab" href="#nav-tools">Tools</a></li>
                            <li><a data-toggle="tab" href="#nav-history">History</a></li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane fade in active" id="nav-infos">
                                <br />
                                <br />
                                <div class="panel panel-primary" id="nav-infos-content">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>GWIS Type</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <select id="gwis-type" class=" allcontent-info content-type-dropdown process-number-generator" style="width: 100%" disabled="disabled"></select>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Assembly Area</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-right">
                                                <select id="assy-section" class=" allcontent-info content-type-dropdown process-number-generator" style="width: 100%" disabled="disabled"></select>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Station</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <select id="station" class=" allcontent-info content-type-dropdown process-number-generator" style="width: 100%" disabled="disabled"></select>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Position</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <select id="position" class=" allcontent-info content-type-dropdown process-number-generator" style="width: 100%" disabled="disabled"></select>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Number</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <input id="number" type="number" min="0" max="9999" step="1" class="form-control  allcontent-info content-type-text process-number-generator" value="0" disabled="disabled" />
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Process No</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <h5 id="process-no" class=" allcontent-info content-type-label"></h5>
                                                <%--<input type="hidden" id="Valprocess-no" />--%>
                                            </div>                                            
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Process Name</b>
                                            </div>
                                            <div class="col-md-10 nav-infos-content content-value-right">
                                                <input id="process-name" class="form-control  allcontent-info content-type-text" type="text" />
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Valid From</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <div class="input-group date-time-picker date" id="valid-from">
                                                    <input type="text" id="valid-from-val" class="form-control  allcontent-info content-type-date" placeholder="YYYY-MM-DD" />
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Valid To</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-right">
                                                <div class="input-group date-time-picker date" id="valid-to">
                                                    <input type="text" id="valid-to-val" class="form-control  allcontent-info content-type-date" placeholder="YYYY-MM-DD" />
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Reference</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <input id="reference" class="form-control  allcontent-info content-type-text" type="text" />
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Status</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-left">
                                                <%--<input id="valid-type" class="form-control  allcontent-info content-type-text" type="text" />--%>
                                                <div id="btnStatus" class="btn-group" role="group" aria-label="Basic example">
                                                  <button type="button" class="btn" data-values ="1" data-toggle="statusChange">Draft</button>
                                                  <button type="button" class="btn" data-values ="3" data-toggle="statusChange">Release</button>
                                                  <button type="button" class="btn" data-values ="4" data-toggle="statusChange">Delete</button>
                                                </div>
                                                <input type="hidden" name="statusChange" id="statusChange"  class="form-control content-type-text">
                                                <%--<div class="input-group">
    				                                <div id="radioBtn" class="btn-group">
    					                                <a class="btn btn-primary btn-sm active" data-toggle="fun" data-title="Y">YES</a>
                                                        <a class="btn btn-primary btn-sm notActive" data-toggle="fun" data-title="X">I don't know</a>
    					                                <a class="btn btn-primary btn-sm notActive" data-toggle="fun" data-title="N">NO</a>
    				                                </div>
    				                                <input type="hidden" name="fun" id="fun">
    			                                </div>--%>
                                            </div>
                                            <%--<div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>History</b>
                                            </div>
                                            <div class="col-md-6 nav-infos-content content-value-right">
                                                <input id="history" class="form-control  allcontent-info content-type-text" type="text" />
                                            </div>--%>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                                <%--<div class="panel-group accordion hidden" id="accordion-engineer" role="tablist" aria-multiselectable="false">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading-engineer">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion-engineer" href="#collapse-engineer" aria-expanded="false" aria-controls="collapse-engineer">Engineer Operation</a>
                                            </h4>
                                            <div id="collapse-engineer" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading-engineer">
                                                <div class="panel-body">
                                                    <div class="panel panel-primary" id="nav-infos-engineer">

                                                        <br />
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-md-12 nav-infos-content content-caption-left">
                                                                    <b>Engineer Operation </b>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-value-left">
                                                                    <div id="engineer-operation" class="form-control content-type-area"></div>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-caption-left">
                                                                    <br />
                                                                    <b>Img Eng Op </b>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-value-left">
                                                                    <br />
                                                                    <input type="file" class="content-type-file" id="filesEngineer" name="files[]" />
                                                                    <output id="listEngineer" class="content-type-image"></output>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>

<%--                                <div class="panel-group" id="accordion-quality" role="tablist" aria-multiselectable="false">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading-quality">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion-quality" href="#collapse-quality" aria-expanded="true" aria-controls="collapse-quality">Quality Operation</a>
                                            </h4>
                                            <div id="collapse-quality" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading-engineer">
                                                <div class="panel-body">

                                                    <div class="panel panel-primary" id="nav-infos-quality">
                                                        <br />
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-md-12 nav-infos-content content-caption-right">
                                                                    <b>Quality Operation </b>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-value-left">
                                                                    <div id="quality-operation" class="form-control  allcontent-info content-type-area"></div>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-caption-left">
                                                                    <br />
                                                                    <b>Img II </b>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-value-left">
                                                                    <br />
                                                                    <input type="file" class=" allcontent-info content-type-file" id="filesQuality" name="files[]" />
                                                                    <output id="listQuality" class="allcontent-info content-type-image"></output>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="panel-heading text-right">
                                    <input type="button" id="cancel-save" class="btn-danger hidden" value="Cancel" />
                                    <input type="button" id="save-data" class="btn btn-primary footer-type-button disabled" value="Save Changes" />
                                </div>
                            </div>
                            <div class="tab-pane fade" id="nav-parts">
                                <br />
                                <button type="button" class="btn btn-success btn-lg btnAddGWIS" id="btnNewGWISParts"><span class="glyphicon glyphicon-plus"></span>New GWIS Parts</button>
                                <br />
                                <br />
                                <div class="panel-group" class="accordionParts" id="accordion-engineerParts" role="tablist" aria-multiselectable="false">
                                    <div class="panel panel-default">
                                        <div class="panel-heading" role="tab" id="heading-engineerParts">
                                            <h4 class="panel-title">
                                                <a role="button" data-toggle="collapse" data-parent="#accordion-engineerParts" href="#collapse-engineerParts" aria-expanded="false" aria-controls="collapse-engineer">Engineer Operation</a>
                                            </h4>
                                            <div id="collapse-engineerParts" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="heading-engineer">
                                                <div class="panel-body">
                                                    <div class="panel panel-primary" id="nav-infos-engineerParts">

                                                        <br />
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-md-12 nav-infos-content content-caption-left">
                                                                    <b>Engineer Operation </b>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-value-left">
                                                                    <div id="engineer-operationParts" class="form-control content-type-area"></div>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-caption-left imageParts">
                                                                    <br />
                                                                    <b>Img Eng Op </b>
                                                                </div>
                                                                <div class="col-md-12 nav-infos-content content-value-left">
                                                                    <br />
                                                                    <%--<input type="file" class="content-type-file" id="filesEngineerParts" name="files[]" />--%>
                                                                    <output id="listEngineerParts" class="content-type-image"></output>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <table id="tblGWISParts" class="display table-bordered" cellspacing="0" width="100%"></table>
                                <br />
                                
                                <div id="panelParts" class="hidden">
                                    <br />
                                    <br />
                                    <div class="col-md-6 nav-infos-content content-value-left">
                                        <b>Model :</b><select id="cat-model" class="content-type-dropdown content-parts" style="width: 100%"></select>
                                        <b>Page :</b><select id="cat-page" class="content-type-dropdown content-parts" style="width: 100%"></select>
                                        <b>SA :</b><select id="cat-sa" class="content-type-dropdown content-parts" style="width: 100%"></select>
                                    </div>
                                    <div class="col-md-12" id="catParent">
                                        <table id="tblPartsCatalog" class="display table-bordered" cellspacing="0" width="100%"></table>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="nav-materials">
                                <br />
                                <br />
                                <table id="tblGWISMaterials" class="display table-bordered" cellspacing="0" width="100%"></table>
                            </div>
                            <div class="tab-pane fade" id="nav-tools">
                                <br />
                                <br />
                                <table id="tblGWISTools" class="display table-bordered" cellspacing="0" width="100%"></table>
                            </div>
                            <div class="tab-pane fade" id="nav-fmea">
                                <br />
                                <button type="button" class="btn btn-success btn-lg" id="btnNewGWISFMEA"><span class="glyphicon glyphicon-plus"></span>New GWIS FMEA    </button>
                                <br />
                                <br />
                                <table id="tblGWISFMEA" class="display table-bordered" cellspacing="0" width="100%"></table>
                            </div>
                            <div class="tab-pane fade" id="nav-pictures">
                                <br />
                                <button type="button" class="btn btn-success btn-lg btnAddGWIS" id="btnNewGWISPicture"><span class="glyphicon glyphicon-plus"></span>New GWIS Picture</button>
                                <br />
                                <br />
                                <div class="col-md-12" id="GWISPictureView">                                        
                                
                                </div>
                            </div>
                            <div class="tab-pane fade" id="nav-history">
                                <br />
                                <div id ="tabHistory">
                                <div class="panel panel-primary" id="nav-his-content">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-5 nav-infos-content text-center">
                                                <h3><b>Current</b></h3>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-5 nav-infos-content text-center">
                                                <h3><b>Old</b></h3>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>GWIS Type</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="gwis-type-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>GWIS Type</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="gwis-type-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Assembly Area</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="assy-section-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Assembly Area</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="assy-section-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Station</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="station-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Station</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="station-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Position</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="position-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Position</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="position-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Number</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="number-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Number</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="number-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Status GWIS</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="status-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Status GWIS</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="status-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Process No</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="process-no-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Process No</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="process-no-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Process Name</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="process-name-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Process Name</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="process-name-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Valid From</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="valid-from-val-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Valid From</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="valid-from-val-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Valid To</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="valid-to-val-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-right">
                                                <b>Valid To</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-right">
                                                <label id="valid-to-val-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Reference</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="reference-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Reference</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="reference-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Valid Type</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="valid-type-Currhist"></label>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-2 nav-infos-content content-caption-left">
                                                <b>Valid Type</b>
                                            </div>
                                            <div class="col-md-3 nav-infos-content content-value-left">
                                                <label id="valid-type-his"></label>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                                <br />
                                <div class="panel panel-primary" id="parts-his-content">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-5 nav-infos-content text-center">
                                                <h3><b>Current</b></h3>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-5 nav-infos-content text-center">
                                                <h3><b>Old</b></h3>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-5 nav-infos-content text-center">
                                                <table id="tblGWISPartsCurrent" class="display table-bordered" cellspacing="0" width="100%"></table>
                                            </div>
                                            <div class="col-md-2 nav-infos-content">
                                            </div>
                                            <div class="col-md-5 nav-infos-content text-center">
                                                <table id="tblGWISPartsOld" class="display table-bordered" cellspacing="0" width="100%"></table>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" tabindex="-1" role="dialog" id="modale-default">
                <div id="modale-type" class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="modale-title">Modal title</h4>
                        </div>
                        <div class="content">
                            <div class="modal-body" id="modale-body">
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
        </div>
        <div class="modalLoading"></div>


        <script src="../../Content/GWIS/Scripts/bootstrap-treeview.js"></script>
        <script src="../../Content/GWIS/Scripts/moment-with-locales.js"></script>
        <script src="../../Content/GWIS/Scripts/jquery.dataTables.min.js"></script>
        <script src="../../Content/GWIS/Scripts/dataTables.bootstrap.min.js"></script>
        <script src="../../Content/GWIS/Scripts/select2.min.js"></script>
        <script src="../../Content/GWIS/Scripts/bootstrap-datetimepicker.min.js"></script>
        <script src="../../Content/GWIS/Scripts/trumbowyg.min.js"></script>
        <script src="../../Content/GWIS/Scripts/tab-info.js?00002"></script>
        <script src="../../Content/GWIS/Scripts/tab-parts.js?00002"></script>
        <script src="../../Content/GWIS/Scripts/tab-picture.js?00002"></script>
        <script src="../../Content/GWIS/Scripts/tab-tools.js?00002"></script>
        <script src="../../Content/GWIS/Scripts/tab-materials.js?00002"></script>
        <script src="../../Content/GWIS/Scripts/tab-fmea.js?00001"></script>
        <script src="../../Content/GWIS/Scripts/tab-history.js?00001"></script>
        <script type="text/javascript">
            var jsonData, selectedDate, tblAssyCat;
            var uriPath = "/api";
            var AssemblySection = [];
            //var authDatas = JSON.parse('{"username":"Admin", "password":"123456", "grant_type":"password" }');
            var ProcessIdTree = "";
            var nodeID = "";
            Array.prototype.unique = function () {
                return this.filter(function (value, index, self) {
                    return self.indexOf(value) === index;
                });
            }
            //function generateNewToken() {
            //    return $.ajax({
            //        type: 'POST',
            //        url: uriPath.concat("/token"),
            //        data: authDatas
            //    });
            //}

            $body = $("body");
            $(document).on({
                ajaxStart: function () { $body.addClass("loading"); },
                ajaxStop: function () { $body.removeClass("loading"); }
            });
               
            function isContentChange() {
                var ret = true;
                $("[class*=content-type-]").each(function (i, elm) {

                    if ($(elm).hasClass("content-type-dropdown") && $(elm).attr('data-default') != undefined)
                        ret = $(elm).select2('val') == $(elm).attr("data-default");
                    if (($(elm).hasClass("content-type-text") || $(elm).hasClass("content-type-date")) && $(elm).attr('data-default') != undefined)
                        ret = $(elm).val() == $(elm).attr('data-default');
                    if (($(elm).hasClass("content-type-area") || $(elm).hasClass("content-type-label")) && $(elm).attr('data-default') != undefined)
                        ret = $(elm).html() == $(elm).attr('data-default');
                    if (!ret)
                        return false;
                })
                return !ret;
            }
            function setContentDefaultData() {
                $("[class*=content-type-]").each(function (i, elm) {
                    if ($(elm).hasClass("content-type-dropdown"))
                        $(elm).val($(elm).attr("data-default")).trigger("change");
                    if ($(elm).hasClass("content-type-text") || $(elm).hasClass("content-type-date"))
                        $(elm).val($(elm).attr('data-default'))
                    if ($(elm).hasClass("content-type-area") || $(elm).hasClass("content-type-label"))
                        $(elm).html($(elm).attr('data-default'))
                    if ($(elm).hasClass("content-type-image")) {
                        if ($(elm).attr('data-default'))
                            $(elm).html('<span><img src="'.concat(($(elm).attr('data-default').indexOf("data:image") > -1 ? '' : 'data:image/jpg;base64\, '), $(elm).attr('data-default'), '" class="thumb"></img></span>'))
                        else
                            $(elm).html('');
                    }
                })

                $("#save-data").prop("disabled", "disabled");
            }
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
                        return getAsyncAPI("/list/view/GWISTypesLookup");
                    }, function (err) {
                        console.log("error on getting headerTreeview. Err : ", err);
                        return getAsyncAPI("/list/view/GWISTypesLookup");
                    })
                    .then(function (ret) {
                        responses.gwis = ret && ret.rows;
                        return getAsyncAPI("/list/view/GWISAssemblyAreas");
                    }, function (err) {
                        console.log("error on getting gwis type. Err : ", err);
                        return getAsyncAPI("/list/view/GWISAssemblyAreas");
                    })
                    .then(function (ret) {
                        responses.sections = ret && ret.rows;
                        AssemblySection = responses.sections;
                        return getAsyncAPI("/list/view/vGWISStationLookup");
                    }, function (err) {
                        console.log("error on getting assembly area. Err : ", err);
                        return getAsyncAPI("/list/view/vGWISStationLookup");
                    })
                    .then(function (ret) {
                        responses.stations = ret && ret.rows;
                        return getAsyncAPI("/list/view/spPackingMonth");
                    }, function (err) {
                        console.log("error on getting station lookup. Err : ", err);
                        return getAsyncAPI("/list/view/spPackingMonth");
                    })
                    .then(function (ret) {
                        responses.packingmonth = ret && ret.rows;
                        return getAsyncAPI("/list/view/CatalogModels");
                    }, function (err) {
                        console.log("error on getting station lookup. Err : ", err);
                        return getAsyncAPI("/list/view/CatalogModels");
                    })
                    .then(function (ret) {
                        responses.models = ret && ret.rows;
                        responses.positions = [{ id: "76", name: "Left" }, { id: "82", name: "Right" }, { id: "78", name: "Neutral" }];
                    }, function (err) { console.log("error on getting catalog models. Err : ", err); })
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

                        $(".control-type-dropdown,.content-type-dropdown,.dropdown-month").each(function (i, elm) {
                            switch ($(elm).attr("id").toLowerCase()) {
                                //case "gwistype":
                                //    defaultCaption = "-Select GWIS Type-";
                                //    arrEach = arr.gwis || [];//arr[1] && arr[1][0].rows || [];
                                //    idField = "id";
                                //    captionField = "model";
                                //    break;
                                case "cat-model":
                                    defaultCaption = "-Select Model Type-";
                                    arrEach = arr.models || []; //arr[4] && arr[4][0].rows || [];
                                    idField = "id";
                                    captionField = "model";
                                    break;
                                case "pagecatalogsearch":
                                    defaultCaption = "-Select Model Type-";
                                    arrEach = arr.models || []; //arr[4] && arr[4][0].rows || [];
                                    idField = "id";
                                    captionField = "model";
                                    break;
                                case "gwis-type":
                                    defaultCaption = "-Select GWIS Type-";
                                    arrEach = arr.gwis || [];// arr[1] && arr[1][0].rows || [];
                                    idField = "id";
                                    captionField = "model";
                                    break;
                                case "assy-section":
                                    defaultCaption = "-Select Section-";
                                    arrEach = arr.sections || [];//arr[2] && arr[2][0].rows || [];
                                    idField = "id";
                                    captionField = "description";
                                    break;
                                case "station":
                                    defaultCaption = "-Select Station-";
                                    arrEach = arr.stations || []; //arr[3] && arr[3][0].rows || [];
                                    idField = "id";
                                    captionField = "stationName";
                                    break;
                                case "position":
                                    defaultCaption = "-Select Position-";
                                    arrEach = arr.positions || [];//[{ id: "76", name: "Left" }, { id: "82", name: "Right" }, { id: "78", name: "Neutral" }];
                                    idField = "id";
                                    captionField = "name";
                                    break;
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


                        $('#listGwisType').empty();
                        (arr.gwis || []).forEach(function (row, rowNum) {
                            $('#listGwisType').append($("<a></a>").attr("id", row.id).attr("href", "#").addClass("list-group-item").html(row.model));
                        })
                    })
                    .fail(function (err) {
                        console.log("deferred call err : ", err);
                    })
            }
            function getDetailProcess(processId) {

                getAsyncAPI("/list/view/GWISProcesses/".concat(processId))
                    .then(function (gwis) {
                        if (gwis && gwis.rows) {
                            bindControl(gwis.rows[0]);
                            bindHistoryData(gwis.rows[0]);
                        }
                        return getAsyncAPI("/list/view/GWISGetParts?ProcessId=".concat(processId));
                    }, function (err) {
                        console.log("Error getting GWIS Process; Err : ", err);
                        return getAsyncAPI("/list/view/GWISGetParts?ProcessId=".concat(processId));
                    })
                    .then(function (parts) {
                        if (parts && parts.rows) {
                            bindPartsHistory(parts.rows, "current");
                            bindTableParts(parts.rows);
                        }
                        return getAsyncAPI("/list/view/GWISGetPictures?ProcessId=".concat(processId));
                    }, function (err) {
                        dttGWISParts({
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        },'error');
                        dtPartsCurrent({
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        });
                        console.log("Error getting GWIS Parts; Err : ", err);
                        return getAsyncAPI("/list/view/GWISGetPictures?ProcessId=".concat(processId));
                    })
                    .then(function (pics) {
                        if (pics && pics.rows)
                            bindTablePics(pics.rows);
                        return getAsyncAPI("/list/view/GWISGetConsumptionMaterial?gwisProcessId=".concat(processId));
                    }, function (err) {
                        dttGWISPartsPicture({
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        }, "error");
                        console.log("Error getting GWIS Pics; Err : ", err);
                        return getAsyncAPI("/list/view/GWISGetConsumptionMaterial?gwisProcessId=".concat(processId));
                    })
                    .then(function (material) {
                        if (material && material.rows)
                            bindTableMaterials(material.rows);
                        return getAsyncAPI("/list/view/GWISGetToolAssigment?gwisProcessId=".concat(processId));
                    }, function (err) {
                        dttGWISMaterials({
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        },"err");
                        console.log("Error getting GWIS material; Err : ", err);
                        return getAsyncAPI("/list/view/GWISGetToolAssigment?gwisProcessId=".concat(processId));
                    })
                    .then(function (tools) {
                        if (tools && tools.rows)
                            bindTableTools(tools.rows);
                        //return getAsyncAPI("/list/view/vAssyCatalogs/Model/'".concat(model, "'"));
                    }, function (err) {
                        dttGWISTools({
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        },"err");
                        console.log("Error getting GWIS material; Err : ", err);
                        //return getAsyncAPI("/list/view/vAssyCatalogs/Model/'".concat(model, "'"));
                    })
                //.then(function (cat) {
                //    if (cat && cat.rows)
                //        bindTablePartsCat(cat.rows);
                //}, function (err) { console.log("Error getting Assy Catalog; Err : ", err); });

                showNavTab("nav-infos-content");
            }
            function showNavTab(navContentId) {
                $("#".concat(navContentId)).prop("hidden", false);
            }

            function loadTreeview() {
                var tvs = jsonData.filter(function (obj) {
                    //return obj.gwisTypeId == $("#gwisType").select2('val') && selectedDate.isBetween(moment(obj.validFrom), moment(obj.validTo), "[]")
                    return selectedDate.isBetween(moment(obj.validFrom), moment(obj.validTo),"days","[]")
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
                            stat.nodes.push(new Object({ "text": icon.concat('&nbsp;',row.processNo, ':\r\n', row.processName), "tags": ['processId', row.id] }))
                        }
                    });
                }
                $("#treeview").treeview({ data: tvData, highlightSearchResults: false });
                $("#treeview").treeview('collapseAll', { silent: true });
                $("#treeview").on("nodeSelected", function (event, node) {
                    if (node.tags && node.tags.length === 2) {
                        //var parentNodes = $('#treeview').treeview('search', ["(.*?)", {
                        //    ignoreCase: true,     // case insensitive
                        //    exactMatch: false,    // like or equals
                        //    revealResults: false,  // dont matching nodes
                        //}]).filter(function (obj) { return obj.parentId == null })
                        //var elm = $('li span.glyphicon-minus').closest('li')[0];
                        //var models = elm && $(elm).text();
                        //var model = models && models.substring(models.indexOf(' ') + 1, models.length);
                        nodeID = node.nodeId;
                        ProcessIdTree = node.tags[1];
                        var url = document.location.href.replace(document.location.pathname,'').concat('/Reports/GWISReports.aspx?id=', ProcessIdTree);
                        $("#tittleProject").attr('href', url);
                        var tittleSelect = $("<div>").html(node.text).text();
                        $("#tittleProject").html(tittleSelect);
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

                    $('#treeview').treeview('collapseNode', [sectionNodes, { silent: true, ignoreChildren: false }]);

                    if (node.tags[0] === 'stationId') {

                        $('#treeview li[data-nodeid='.concat(node.nodeId, ']')).nextUntil($('li span.expand-icon').parent('li'), 'li').each(function (i, elm) {
                            const origin = $(elm).text();
                            const index = origin.indexOf("[");
                            const text = origin.split("] ")[1];
                            const status = origin.split("]")[0].substr(1, 1);
                            $(elm).text().replace(origin, text);
                            //$($('li span.glyphicon-minus').parent('li')[2]).nextUntil($('li span.expand-icon').parent('li'), 'li').each(function (i, elm) {
                        })
                    }

                })
            }
            //$("#packingMonth").on("dp.hide", function (e) {
            //    if (moment("1-".concat($("#packingMonthVal").val()), "D-MMMM-YYYY").isValid()) {
            //        selectedDate = moment("1-".concat($("#packingMonthVal").val()), "D-MMMM-YYYY");
            //        //if ($('#listGwisType a.active').length > 0)
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
            $(".footer-type-button").on("click", function () {
                switch ($(this).attr("id")) {
                    //case "reset-data":
                    //    resetModal();
                    //    $("#modale-title").html("Confirm Action");
                    //    if (!$('#save-data').prop('disabled')) {
                    //        //$("#modale-body").append("<h3 class='text-warning'></h3>").html("You have unsaved changes!!");
                    //        $("#modale-body").append("<h4 class='text-info'></h4>").html("Do you still want to discard all the changes ?");
                    //        $("#modale-left-button").removeClass().addClass("btn btn-info").html("No");
                    //        $("#modale-right-button").removeClass().addClass("btn btn-primary").html("Yes");
                    //        $("#modale-default").attr("data-sender", "reset-data").modal("show");
                    //    }
                    //    //else {
                    //    //    $("#modale-body").append("<h4 class='text-info'></h4>").html("This will reset data to its default value");
                    //    //    $("#modale-left-button").removeClass().addClass("hidden");
                    //    //    $("#modale-right-button").removeClass().addClass("btn btn-primary").html("Proceed");
                    //    //}

                    //    break;
                    case "save-data":
                        resetModal();
                        $("#modale-title").html("Confirm Action");
                        if (!$('#save-data').prop('disabled') && $("#btnNewGWISPartsInfo").val() != "addNew") {
                            $("#modale-body").append($("<h4 class='text-warning'></h4>").html("Are you sure want to Save?"));
                            //.append($("<h4 class='text-info'></h4>").html("Do you still want to discard all the changes ?"));
                            $("#modale-left-button").removeClass().addClass("btn btn-default").html("No");
                            $("#modale-right-button").removeClass().addClass("btn btn-success").html("Yes");
                            $("#modale-default").attr("data-sender", "update-data").modal("show");
                        }
                        else {
                            $("#modale-body").append($("<h4 class='text-warning'></h4>").html("Are you sure want to Save?"));
                            //.append($("<h4 class='text-info'></h4>").html("Do you still want to discard all the changes ?"));
                            $("#modale-left-button").removeClass().addClass("btn btn-default").html("No");
                            $("#modale-right-button").removeClass().addClass("btn btn-success").html("Yes");
                            $("#modale-default").attr("data-sender", "addNew-data").modal("show");
                        };
                        //else {
                        //    $("#modale-body").append("<h4 class='text-info'></h4>").html("This will reset data to its default value");
                        //    $("#modale-left-button").removeClass().addClass("hidden");
                        //    $("#modale-right-button").removeClass().addClass("btn btn-primary").html("Proceed");
                        //}
                        break;
                    default:
                        break;

                }
            })
            $("#modale-default").on("hide.bs.modal", function () {
                if ($(document.activeElement).is("#modale-right-button")) {
                    switch ($("#modale-default").attr("data-sender")) {
                        //case "reset-data":
                        //    setContentDefaultData();
                        //    break;
                        case "update-data":
                            infoUpdate();
                            break;
                        case "addNew-data":
                            infoAddNew();
                            break;
                        case "picture-addNew":
                            PictureAddNew();
                            break;
                        case "picture-edit":
                            PictureUpdate();
                            break;
                        case "picture-delete-data":
                            PictureDelete();
                            break;
                        case "parts-cat-addNew":
                            PartsCatalogSaveNew();
                            break;
                        case "parts-edit-data":
                            PartsSaveEdit();
                            break;
                        case "parts-delete-data":
                            deleteParts();
                            break;
                        default:
                            break;
                    }
                }
            })
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                switch ($(e.target) && $(e.target).length > 0 && $(e.target)[0].hash) {
                    case "#nav-infos":
                        if ($.fn.DataTable.isDataTable('#tblSearchResult')) {
                            $('#tblSearchResult').DataTable().columns.adjust();
                        }
                        break;
                    case "#nav-parts":
                        if ($.fn.DataTable.isDataTable('#tblGWISParts')) {
                            $('#tblGWISParts').DataTable().columns.adjust();
                        }
                        break;
                    //case "#nav-pictures":
                    //    if ($.fn.DataTable.isDataTable('#tblGWISPicture')) {
                    //        $('#tblGWISPicture').DataTable().columns.adjust();
                    //    }
                    //    break;
                    case "#nav-materials":
                        if ($.fn.DataTable.isDataTable('#tblGWISMaterials')) {
                            $('#tblGWISMaterials').DataTable().columns.adjust();
                        }
                        break;
                    case "#nav-tools":
                        if ($.fn.DataTable.isDataTable('#tblGWISTools')) {
                            $('#tblGWISTools').DataTable().columns.adjust();
                        }
                        break;
                    case "#nav-history":
                        if ($.fn.DataTable.isDataTable('#tblGWISPartsCurrent')) {
                            $('#tblGWISPartsCurrent').DataTable().columns.adjust();
                        }
                        if ($.fn.DataTable.isDataTable('#tblGWISPartsOld')) {
                            $('#tblGWISPartsOld').DataTable().columns.adjust();
                        }                        
                        break;
                    default:
                        break;
                }
            });
            function infoAddNew() {
                var obj = new Object();
                obj.GWISTypeId = $("#gwis-type").val();
                obj.GWISStationId = $("#station").val();
                obj.StationId = $("#station").val();
                obj.GWISAssemblyAreaId = $("#assy-section").val();
                obj.AssemblySectionId = AssemblySection.filter(function (val) { return val.id == obj.GWISAssemblyAreaId })[0].assemblySectionId;
                obj.ProcessNo_Position = $("#position").val();
                obj.ProcessNo_Number = $("#number").val();
                obj.ProcessNo = $("#process-no").text();
                obj.ValidFrom = $("#valid-from-val").val();
                obj.ValidTo = $("#valid-to-val").val();
                obj.Status = $("#statusChange").val();//$("#status").val();
                obj.ProcessName = $("#process-name").val();
                obj.Reference = $("#reference").val();
                //obj.history = $("#history").val();
                //obj.valid_type = $("#valid-type").val();
                //obj.Eng_op = $("#engineer-operation").html();
                //var arrImg = $("#listEngineer > span  > img").attr("src").split(',');
                //obj.img_eng_op = arrImg[1];
                //obj.Qua_op = $("#quality-operation").html().replace(/"/g,'\'');
                //if ($("#listQuality > span  > img").attr("src") != undefined) {
                //    arrImg = $("#listQuality > span  > img").attr("src").split(',');
                //    obj.img_ii = arrImg[1];
                //}                

                $.ajax({
                    url: '/api/list/insert/GWISProcesses',
                    type: "PUT",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        //getDetailProcess(ProcessIdTree);
                        resetNewGWISPArtsInfo();
                        parseDataFromServer()
                        //if (ProcessIdTree != "") {
                        //    getDetailProcess(ProcessIdTree);
                        //}
                        //update datatable tblGWISParts
                    },
                    error: function (xhr) {
                        console.log(xhr);
                        ///alert('error');
                    }
                });
            }
            function infoUpdate() {
                var obj = new Object();
                obj.ValidFrom = $("#valid-from-val").val();
                obj.ValidTo = $("#valid-to-val").val();
                obj.Status = $("#statusChange").val();//$("#status").val();
                obj.ProcessName = $("#process-name").val();
                obj.Reference = $("#reference").val();
                //obj.history = $("#history").val();
                obj.valid_type = $("#valid-type").val();
                //obj.Eng_op = $("#engineer-operation").html();
                //var arrImg = $("#listEngineer > span  > img").attr("src").split(',');
                //obj.img_eng_op = arrImg[1];
                //obj.Qua_op = $("#quality-operation").html();
                //if ($("#listQuality > span  > img").attr("src") != undefined) {
                //    arrImg = $("#listQuality > span  > img").attr("src").split(',');
                //    obj.img_ii = arrImg[1];
                //}
                var statusOld = $("#statusChange").attr('data-default')
                var statusNew = $("#statusChange").val();
                $.ajax({
                    url: '/api/list/update/GWISProcesses/'.concat(ProcessIdTree),
                    type: "POST",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        if (statusOld != statusNew) {
                            if(statusNew == 4){
                                parseDataFromServer();
                                $(".rightTab").addClass('hidden');
                            } else if (statusNew == 3) {                                
                                $("li[data-nodeid='".concat(nodeID, "'] > .glyphicon.glyphicon-asterisk")).removeClass("stat-draft");
                                $("li[data-nodeid='".concat(nodeID, "'] > .glyphicon.glyphicon-asterisk")).addClass("stat-released");
                                getDetailProcess(ProcessIdTree);
                            }
                        } else {
                            getDetailProcess(ProcessIdTree);
                        }
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            };
            function PartsCatalogSaveNew() {
                var obj = new Object();
                obj.ProcessId = ProcessIdTree;
                obj.PartNo = $("#lblPartNoCat").html();
                if ($("#lblPartNoCat").html() == "A 100 000 00 00") {
                    obj.PartDescription = $("#txtDescCat").val();
                } else {
                    obj.PartDescription = $("#lblDescCat").html();
                }
                obj.Pos = $("#lblPosCat").html();
                obj.Pos2 = $("#lblPos2Cat").html();
                obj.Qty = $("#lblQtyCat").html();
                obj.Qty_Actual = $("#ActQtyCat").val();
                obj.Pos_Actual = $("#ActPosCat").val();
                obj.takt_time = $("#cycleTime").val();
                obj.AssyCatalogId = $("#Id").val();
                obj.Page_Catalog = $("#Page").val();
                obj.CatalogModelId = $("#CatalogModelId").val();
                obj.SA = $("#SA").val();
                obj.KGU = $("#KGU").val();
                if ($("#cat-model").val() != "default") {
                    obj.CatalogModeId = $("#cat-model").val();
                }
                obj.Page_Catalog = $("#cat-page").val() == "default" ? "-" : $("#cat-page").val();
                obj.SA = $("#cat-sa").val() == "default" ? "" : $("#cat-sa").val();
                $.ajax({
                    url: '/api/list/insert/GWISParts',
                    type: "PUT",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        getDetailProcess(ProcessIdTree);

                        //update datatable tblGWISParts
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            };
            function PartsSaveEdit() {
                var partsID = $("#IdParts").val();
                var obj = new Object();
                if ($("#lblPartNoParts").html() == "A 100 000 00 00") {
                    obj.PartDescription = $("#txtDescParts").val();
                }
                obj.Qty = $("#QtyParts").val();
                obj.Pos = $("#PosParts").val();
                obj.Pos2 = $("#Pos2Parts").val();
                $.ajax({
                    url: '/api/list/update/GWISParts/'.concat(partsID),
                    type: "POST",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        getDetailProcess(ProcessIdTree);
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            };
            function PartsDelete() {
                var partsID = $("#IdParts").val();
                var obj = new Object();
                obj.IsDeleted = true;
                $.ajax({
                    url: '/api/list/update/GWISParts/'.concat(partsID),
                    type: "POST",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        getDetailProcess(ProcessIdTree);
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            };
            function PictureAddNew() {
                var obj = new Object();
                obj.ProcessId = ProcessIdTree;
                //obj.ImageName = $("#ImageNameAdd").val();
                obj.Rf = $("#PosAdd").val();
                if ($("#files").attr('object-image') != undefined) {
                    var valblob = $("#files").attr('object-image').split(',');
                    obj.Image = valblob[1];
                }
               
                $.ajax({
                    url: '/api/list/insert/GWISPictures',
                    type: "PUT",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        getDetailProcess(ProcessIdTree);
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            };
            function PictureUpdate() {
                var obj = new Object();
                obj.ProcessId = ProcessIdTree;
                //obj.ImageName = $("#ImageNameEdit").val();
                obj.Rf = $("#posEdit").val();
                if ($("#files").attr('object-image') != undefined) {
                    var valblob = $("#files").attr('object-image').split(',');
                    obj.Image = valblob[1];
                }
                var idPicture = $("#IdPicture").val();
                $.ajax({
                    url: '/api/list/update/GWISPictures/'.concat(idPicture),
                    type: "POST",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        getDetailProcess(ProcessIdTree);
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            };
            function PictureDelete() {
                var obj = new Object();
                obj.IsDeleted = true;
                var idPicture = $("#IdPicture").val();
                $.ajax({
                    url: '/api/list/update/GWISPictures/'.concat(idPicture),
                    type: "POST",
                    data: obj,
                    beforeSend: function (xhr, settings) { xhr.setRequestHeader('Authorization', localStorage.getItem('token')); },
                    success: function (data, status, xhr) {
                        getDetailProcess(ProcessIdTree);
                    },
                    error: function (xhr) {
                        ///alert('error');
                    }
                });
            }
            function resetModal() {
                $('#modale-title').html('Modal Tittle');
                $('#modale-left-button').html('');
                $('#modale-right-button').html('');
                document.getElementById("modale-default").removeAttribute("data-sender");
                $('#modale-type').removeClass('modal-lg');
                $('#modale-type').removeClass('modal-sm');
                $('#modale-body').html("");
                $('#modale-left-button').show();
                $('#modale-right-button').show();
                $('#modale-left-button').addClass('btn');
                $('#modale-right-button').addClass('btn');
            }

            function handleFileSelect(evt, filesid, onevent) {
                var files = evt.target.files; // FileList object

                // Loop through the FileList and render image files as thumbnails.
                for (var i = 0, f; f = files[i]; i++) {
                    // Only process image files.
                    if (!f.type.match('image.*')) {
                        var attrFiles = $(filesid).attr('object-image');
                        if (typeof attrFiles !== typeof undefined && attrFiles !== false) {
                            $(filesid).removeAttr("object-image");
                        }
                        if (onevent.toLowerCase() == "add") {
                            $("#modale-right-button").prop("disabled", true);
                        }
                        continue;
                    }

                    var reader = new FileReader();
                    // Closure to capture the file information.
                    reader.onload = (function (theFile) {
                        return function (e) {
                            // Render thumbnail.
                            //var span = document.createElement('span');
                            //span.innerHTML = ['<img class="thumb" src="', e.target.result,
                            //    '" title="', escape(theFile.name), '"/>'].join('');
                            //$(lisId).html(span);
                            $(filesid).attr('object-image', e.target.result)
                            //console.log(span);
                        };
                    })(f);
                    if (onevent.toLowerCase() == "add") {
                        $("#modale-right-button").prop("disabled", false);
                    }
                    // Read in the image file as a data URL.
                    reader.readAsDataURL(f);
                }
            }
            function b64toBlob(b64Data, contentType, sliceSize) {
                contentType = contentType || '';
                sliceSize = sliceSize || 512;

                var byteCharacters = atob(b64Data);
                var byteArrays = [];

                for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                    var slice = byteCharacters.slice(offset, offset + sliceSize);

                    var byteNumbers = new Array(slice.length);
                    for (var i = 0; i < slice.length; i++) {
                        byteNumbers[i] = slice.charCodeAt(i);
                    }

                    var byteArray = new Uint8Array(byteNumbers);

                    byteArrays.push(byteArray);
                }

                var blob = new Blob(byteArrays, { type: contentType });
                return blob;
            }
           

            //===============================================================================EVENT FOR SEARCH===================================================================
            //$("#modale-default").on('shown.bs.modal', function () {
            //    $('#tblSearchResult').DataTable().columns.adjust();
            //});

            $("#searchPart").on('click', function () {
                resetModal();

                var partNo = $("#partNoSearch").val();
                var PageCat = $("#PageCatSearch").val();
                $('#modale-right-button').removeClass('btn');

                $('#modale-right-button').hide();
                if (partNo == "") {
                    $('#modale-title').html('Warning');
                    $('#modale-body').html('<h5>Part No. must be fill<h5>');
                    $('#modale-default').modal('show');
                    $('#modale-left-button').html('Close');
                    return false;
                }
                $('#modale-left-button').removeClass('btn');
                $('#modale-left-button').hide();


                var contentBody = [];
                contentBody
                    .push('<table id="tblSearchResult" class="display table-bordered" cellspacing="0" width="100%"></table>'
                    );

                $('#modale-right-button').prop('disabled', true);
                $('#modale-title').html('Search Result');
                $('#modale-type').addClass('modal-lg');
                $('#modale-body').html(contentBody.join(''));              

                getAsyncAPI("/list/view/GWISSearchParts?PartNo='".concat(partNo,"'"))
                    .then(function (result) {
                        if (result && result.rows) {
                            if (PageCat != "" ) {
                                var arrCat = $.grep(result.rows, function (element, index) {
                                    return element.page_Catalog == PageCat.toString();
                                });
                                bindSearchParts("#tblSearchResult", arrCat, "PartsNo");
                            } else {
                                bindSearchParts("#tblSearchResult", result.rows, "PartsNo");
                            }
                        }
                    }, function (err) {
                        console.log("Error getting Search Parts; Err : ", err);
                        dttSearchParts("#tblSearchResult",{
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        },'error');
                    })

                
                $('#modale-default').modal('show');
                
            });

            $("#searchbyModel").on('click', function () {
                resetModal();

                var PageCat = $("#PageCatalogSearch").val();
                $('#modale-right-button').removeClass('btn');

                $('#modale-right-button').hide();
                if (PageCat == "default") {
                    $('#modale-title').html('Warning');
                    $('#modale-body').html('<h5>you must choose Page Catalog<h5>');
                    $('#modale-default').modal('show');
                    $('#modale-left-button').html('Close');
                    return false;
                }
                $('#modale-left-button').removeClass('btn');
                $('#modale-left-button').hide();


                var contentBody = [];
                contentBody
                    .push('<table id="tblSearchResultModel" class="display table-bordered" cellspacing="0" width="100%"></table>'
                    );

                $('#modale-right-button').prop('disabled', true);
                $('#modale-title').html('Search Result');
                $('#modale-type').addClass('modal-lg');
                $('#modale-body').html(contentBody.join(''));

                getAsyncAPI("/list/view/GWISSearchbyModel?id=".concat(PageCat))
                    .then(function (result) {
                        if (result && result.rows) {
                            bindSearchParts("#tblSearchResultModel", result.rows, "model");
                        }
                    }, function (err) {
                        console.log("Error getting Search By Model ; Err : ", err);
                        dttSearchParts("#tblSearchResultModel", {
                            rows: [["Data Not Found"]],
                            cols: [{ title: "ERROR", orderable: false }]
                        }, "error", "model");

                    })



            });

            function bindSearchParts(nameTable, datas, searchBy) {
                var cols = [];
                Object.keys(datas[0]).forEach(function (data) {
                    var col = { 'title': data, 'orderable': false, 'className': data.trim() }
                    cols.push(col);
                })
                cols.splice(0, 0, { 'title': 'Action', 'orderable': false, 'className': 'Action' });
                var rows = datas.map(function (obj) {
                    var buttons = '<button type="button" object-id="'.concat(obj.processId, '" title="View Details" class="btn btn-primary btn-xs btn-action-datatable-search">View Details</button>')
                    var newObj = $.extend({ 'button': buttons }, obj)
                    return Object.keys(newObj).map(function (e) {
                        return newObj[e];
                    })
                });

                var dtParts = { cols: cols, rows: rows };

                dttSearchParts(nameTable, dtParts, '', searchBy);
            }

            function dttSearchParts(nameTable, datas, status,searchBy) {
                if ($.fn.dataTable.isDataTable(nameTable)) {
                    $(nameTable).DataTable().destroy();
                }
                $(nameTable).empty();
                var dttResult;
                if (status == "error") {
                    $(nameTable).DataTable({
                        "columns": datas.cols,
                        "scrollX": false,
                        "paging": false,
                        "bFilter": false,
                        "bInfo": false,
                        "bSortable": false,
                        "autoWidth": true,
                        "data": datas.rows,
                        "fnDrawCallback": function () {
                            $("#modale-default").on('shown.bs.modal', function () {
                                if ($.fn.DataTable.isDataTable(nameTable)) {
                                    $(nameTable).DataTable().columns.adjust();
                                }                                
                            });
                        }
                    });
                    
                } else {
                    if (searchBy == "model") {
                        $(nameTable).DataTable({
                            "columns": datas.cols,
                            "scrollX": true,
                            "paging": true,
                            "bFilter": true,
                            "bInfo": false,
                            "bSortable": false,
                            "autoWidth": true,
                            "data": datas.rows,
                            "columnDefs": [
                            {
                                "targets": [1],
                                "visible": false,
                                "searchable": false
                            },
                            {
                                "targets": [2],
                                "visible": false,
                                "searchable": false
                            }],
                            "fnDrawCallback": function () {
                                $("#modale-default").on('shown.bs.modal', function () {
                                    if ($.fn.DataTable.isDataTable(nameTable)) {
                                        $(nameTable).DataTable().columns.adjust();
                                    }
                                });
                            }
                        });
                        $('.btn-action-datatable-search').on('click', function () {
                            $('#modale-default').modal('hide');
                            var parentParts = $(this).closest('tr');
                            var processId = $(this).attr('object-id');
                            var tittle = parentParts.children('td.processNo').html().concat(' : ', parentParts.children('td.processName').html());
                            $("#tittleProject").html(tittle);
                            var url = document.location.href.replace(document.location.pathname, '').concat('/Reports/GWISReports.aspx?id=', $(this).attr('object-id'));
                            $("#tittleProject").attr('href', url);
                            $(".rightTab").removeClass('hidden');
                            getDetailProcess(processId);
                        });
                        
                    } else {
                        $(nameTable).DataTable({
                            "columns": datas.cols,
                            "scrollX": true,
                            "paging": true,
                            "bFilter": true,
                            "bInfo": false,
                            "bSortable": false,
                            "autoWidth": true,
                            "data": datas.rows,
                            "columnDefs": [
                            {
                                "targets": [1],
                                "visible": false,
                                "searchable": false
                            }],
                            "fnDrawCallback": function () {
                                $("#modale-default").on('shown.bs.modal', function () {
                                    if ($.fn.DataTable.isDataTable(nameTable)) {
                                        $(nameTable).DataTable().columns.adjust();
                                    }
                                });
                            }
                        });
                        $('.btn-action-datatable-search').on('click', function () {
                            $('#modale-default').modal('hide');
                            var parentParts = $(this).closest('tr');
                            var processId = $(this).attr('object-id');
                            var tittle = parentParts.children('td.processNo').html().concat(' : ', parentParts.children('td.processName').html());
                            $("#tittleProject").html(tittle);
                            var url = document.location.href.replace(document.location.pathname, '').concat('/Reports/GWISReports.aspx?id=', $(this).attr('object-id'));
                            $("#tittleProject").attr('href', url);
                            $(".rightTab").removeClass('hidden');
                            getDetailProcess(processId);
                        });
                        
                    }                    
                }
                $('#modale-default').modal('show');

            }

            $('#btnStatus button').on('click', function (e) {
                var sel = $(this).attr('data-values');
                var tog = $(this).attr('data-toggle');
                var changed = false;
                var dataDefault = $('#' + tog).attr('data-default');
                if (sel == "1" && dataDefault <= 1) {
                    $('#' + tog).val('1');
                    if ($('#btnStatus button').hasClass('btn-primary'))
                        $('#btnStatus button').removeClass('btn-primary')
                    if ($('#btnStatus button').hasClass('btn-success'))
                        $('#btnStatus button').removeClass('btn-success')
                    $(this).addClass('btn-danger')
                    if (isContentChange()) {
                        $("#save-data").prop("disabled", false);//$("#save-data").removeClass("disabled");
                        $("#save-data").removeClass("disabled");
                    }
                    else
                        $("#save-data").prop("disabled", "disabled");//("#save-data").removeclass("disabled").addclass("disabled");
                    
                } else if (sel == "3" && dataDefault <= 3) {
                    $('#' + tog).val('3');
                    if ($('#btnStatus button').hasClass('btn-primary'))
                        $('#btnStatus button').removeClass('btn-primary')
                    if ($('#btnStatus button').hasClass('btn-danger'))
                        $('#btnStatus button').removeClass('btn-danger')
                    $(this).addClass('btn-success');
                    if (isContentChange()) {
                        $("#save-data").prop("disabled", false);//$("#save-data").removeClass("disabled");
                        $("#save-data").removeClass("disabled");
                    }
                    else
                        $("#save-data").prop("disabled", "disabled");//("#save-data").removeclass("disabled").addclass("disabled");
                } else if (sel == "4") {
                    $('#' + tog).val('4');
                    if ($('#btnStatus button').hasClass('btn-success'))
                        $('#btnStatus button').removeClass('btn-success')
                    if ($('#btnStatus button').hasClass('btn-danger'))
                        $('#btnStatus button').removeClass('btn-danger')
                    $(this).addClass('btn-primary');
                    if (isContentChange()) {
                        $("#save-data").prop("disabled", false);//$("#save-data").removeClass("disabled");
                        $("#save-data").removeClass("disabled");
                    }
                    else
                        $("#save-data").prop("disabled", "disabled");//("#save-data").removeclass("disabled").addclass("disabled");
                }
                e.stopPropagation();
                e.cancelBubble;

            })

            $(function () {
                $('#accordion-engineer .panel-collapse.in').collapse('hide');
                $('#accordion-engineerParts .panel-collapse.in').collapse('hide');
                $('#accordion-quality .panel-collapse.in').collapse('hide');
                parseDataFromServer();
                //$("#engineer-operation").trumbowyg();
                $("#engineer-operationParts").trumbowyg();
                //$("#quality-operation").trumbowyg();
                $(".control-type-dropdown").select2();
                $(".content-type-dropdown").select2();
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
            })
        </script>
    </body>
</asp:Content>
