<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AlterationSummary.aspx.cs" Inherits="Ebiz.WebForm.custom.STA.AlterationSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
          $(document).ready(function () {
              $('.navahref').click(function () {
                  var tempid = $(this).attr('id');
                  var submenuid = "#submenu" + tempid.substring(4, 8);
                  $.ajax({
                      type: "POST",
                      url: "AlterationSummary.aspx/GetCollapsedTreeView",
                      data: "{'id':'" + $(this).attr('id') + "', 'flag':'" + $(submenuid).is(":visible") + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      failure: function (response) {
                      alert(response.d);
                    }
                    });
              });
          });

        
    </script>
    <h2>Alteration Summary</h2>
    <br />
    <dx:ASPxButton runat="server" ID="btnlinktochecklist" Text="New Alteration" OnClick="btnlinktochecklist_Click"></dx:ASPxButton>
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <div class="container" style="padding: 0;">
        <div class="row row-offcanvas row-offcanvas-left">
        </div>
    </div>
  
</asp:Content>
