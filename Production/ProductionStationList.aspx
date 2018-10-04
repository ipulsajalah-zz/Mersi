<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductionStationList.aspx.cs" Inherits="Ebiz.WebForm.custom.Production.ProductionStationList" %>

<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //
        //This script is used to handle client side action of command buntton
        //

        //function OnCustomButtonClick(s, e) {
        //    if (e.buttonID == "Copy") {
        //        var value = "Copy_" + e.visibleIndex;
        //        s.PerformCallback(value);
        //    }
        //}

    </script>

    <div style="text-align: center; padding-bottom: 10px">
        <dx:ASPxLabel ID="lblPosition" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
    <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>



</asp:Content>