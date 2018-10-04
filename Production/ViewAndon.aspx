<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAndon.aspx.cs" MasterPageFile="~/Andon.master" Inherits="DotMercy.custom.Production.ViewAndon" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    PCD
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .div-header {
            padding: 10px;
            text-align: center;
        }

        .lbl_line {
            font-size: 16px;
            padding: 0 10px 0 0;
        }

        .dxeRoot_DevEx {
            margin: 0 auto;
        }
        .overrideDate{
            display: inline-flex;
        }
        .overrideDateLbl{
            margin: 5px;
        }
    </style>

    <table class="tableBody" style="margin: 0 auto">
        <tbody>

            <tr>
                <td style="padding: 0px; margin: 10px;">
                    <div id="divMasterDataView" runat="server"></div>
                </td>
            </tr>
        </tbody>
    </table>
    <script>
        function goto(line, stationAct) {
            
            window.location = "../Production/Andon.aspx?stationAct=" + stationAct + "&Line=" + line + "";
        }
        function goto2() {
            var date = document.getElementById("ctl00_ctl00_ASPxSplitter1_Content_ContentSplitter_MainContent_dat_I").value;
            __doPostBack('btnSave', date);
        }
    </script>

</asp:Content>
