<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ConsumtionMaterialUsages.aspx.cs" Inherits="Ebiz.WebForm.Tids.CV.Production.ConsumtionMaterialUsages" %>

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
    <script>
        //$(document).ready(function () {
        //    $('.navahref').click(function () {
        //        var tempid = $(this).attr('id');


        //        var submenuid = "#submenu" + tempid.substring(4, 8);
        //        $.ajax({
        //            type: "POST",
        //            url: "AlterationSummary.aspx/GetCollapsedTreeView",
        //            data: "{'id':'" + $(this).attr('id') + "', 'flag':'" + $(submenuid).is(":visible") + "'}",
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            failure: function (response) {
        //                alert(response.d);   
        //            }
        //        });
        //    });
        //});
        function getValuePm(s, e) {
            txtTempValue.Set('hidden_value', cmbPackingMonth.GetValue());
        }
        function Direct(s,e) {
            window.location.href = "/custom/Production/ConsumtionMaterialDirect.aspx";
        }
    </script>

    <h3 class="grid-header" style="font-family: CorpoS; align-content: center; text-align: center; align-items: center; color: black;">Consumption Material Usages</h3>
    <h5 class="grid-header" style="font-family: CorpoS; margin-top: -15px; align-content: center; text-align: center; align-items: center; color: black;">Manage consumption material manage to certain Process in certain Packing Month and Station</h5>

    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <div class="container" style="padding: 0;">
        <div class="row row-offcanvas row-offcanvas-left">
        </div>
    </div>

    <div class='row row-offcanvas row-offcanvas-left'>
        <div class='col-xs-6 col-sm-12 sidebar-offcanvas' id='sidebar' role='navigation'>
            <dx:ASPxLabel ID="lblpackingMonth" Style="font-family: CorpoS;" Font-Bold="false" Text="Select Packing Month :" runat="server"></dx:ASPxLabel>
            <br />
            <dx:ASPxComboBox runat="server" ID="cmbPackingMonth" AutoPostBack="true" DataSourceID="sdsGetPackingMonth" ValueField="PM" TextField="PM" CssClass="dropdown-header" ClientInstanceName="cmbPackingMonth">
                <ClientSideEvents SelectedIndexChanged="getValuePm" />
            </dx:ASPxComboBox>
            <dx:ASPxButton runat="server" ID="LinkConsumptionMaterialListDirect" Text="Consumption Material">
                <ClientSideEvents Click="function (s, e) { e.processOnServer = false; window.location='/custom/Production/ConsumtionMaterialDirect.aspx'; }" />
            </dx:ASPxButton>
            <dx:ASPxButton runat="server" ID="ASPxButton1" Text="Consumption Material Usages" Enabled="false"></dx:ASPxButton>
            <dx:ASPxHiddenField runat="server" ClientInstanceName="txtTempValue" ID="txtTempValue"></dx:ASPxHiddenField>

            <div class="col-sm-2">
            </div>      
        </div>
        <div class='col-xs-6 col-sm-12 sidebar-offcanvas' id='sidebar' role='navigation'>

            <dx:ASPxLabel ID="lblCaption" runat="server" Style="font-family: CorpoS; font-size:larger; margin-top: -15px; align-content: center; text-align: center; align-items: center; color: black;"></dx:ASPxLabel>
            <div class="col-sm-2">
            </div>
        </div>
    </div>

    <asp:SqlDataSource ID="sdsGetPackingMonth" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT convert(VARCHAR(6),VALIDFROM,112)[PM] FROM GWISProcesses WHERE [Status] < 4 AND DATEDIFF(M,ValidFrom,GETDATE()) > 0 group by convert(VARCHAR(6),VALIDFROM,112) order by convert(VARCHAR(6),VALIDFROM,112) desc"></asp:SqlDataSource>
</asp:Content>
