<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ConsumtionMaterialInDirect.aspx.cs" Inherits="Ebiz.WebForm.custom.Production.ConsumtionMaterialInDirect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h3 class="grid-header" style=" font-family:CorpoS; align-content: center; text-align: center; align-items: center; color: black;">Consumption Material In-Direct</h3>
     <h5 class="grid-header" style=" font-family:CorpoS; align-content: center; margin-top:-15px; text-align: center; align-items: center; color: black;">Manage consumption material direct or in-direct</h5>
 
    <div class="container-fluid" style="width: 100%;  padding: 10px 0 10px 0 !important; text-align: left; font-weight: bolder">
        <dx:ASPxButton runat="server" ID="btnDirect" ClientInstanceName="btnDirect" Text="Direct" AutoPostBack="false" OnClick="btnDirect_Click">
        </dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="btnInDirect" Enabled="false" ClientInstanceName="btnInDirect" Text="InDirect" AutoPostBack="false" OnClick="btnInDirect_Click">
        </dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="btnConsumptionMaterialUsages" ClientInstanceName="btnConsumptionMaterialUsages" Text="Consumption Material Usages" AutoPostBack="false" OnClick="btnConsumptionMaterialUsages_Click">
        </dx:ASPxButton>
    </div>

</asp:Content>
