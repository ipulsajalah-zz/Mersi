<%@ Page Title="Production List PDI-0" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProductionPDI0List.aspx.cs" Inherits="DotMercy.custom.Production.ProductionPDI0List" %>
<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">

    <div style="text-align: center; padding-bottom: 10px">
        <dx:ASPxLabel ID="lblPosition" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
      <div class="container-fluid" style="width: 100%; padding: 0 0 20px 0 !important;">
        <div style="text-align: center; font-weight: bolder">
            <span style="font-size: 18px;">Production PDI-0<br />
<%--                <dx:ASPxLabel Style="font-weight: bolder; font-size: large;" CssClass="h2" runat="server" ID="lblStationName" /> <br />--%>
            </span>
           
        </div>
    </div>
    <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>



</asp:Content>