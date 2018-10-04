<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ConsumtionMaterialProcess.aspx.cs" Inherits="Ebiz.WebForm.custom.Production.ConsumtionMaterialProcess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h3 class="grid-header" style=" font-family:CorpoS; align-content: center; text-align: center; align-items: center; color: black;">Consumption Material Usages</h3>
    <h5 class="grid-header" style=" font-family:CorpoS; margin-top:-15px; align-content: center; text-align: center; align-items: center; color: black;">Manage consumption material manage to certain Process in certain Packing Month and Station</h5>
     <div class="container-fluid" style="width: 100%;  padding: 10px 0 10px 0 !important; text-align: left; font-weight: bolder">
        <dx:ASPxButton runat="server" ID="LinkConsumptionMaterialListDirect" Text="Consumption Material List" OnClick="LinkConsumptionMaterialListDirect_Click"></dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="ASPxButton1" Text="Consumption Material Usages" Enabled="false"></dx:ASPxButton>
        <dx:ASPxButton runat="server" ID="btnConsumptionMaterialUsages" ClientInstanceName="btnConsumptionMaterialUsages" Text="Back To Process List"  AutoPostBack="false" OnClick="btnConsumptionMaterialUsages_Click">
        </dx:ASPxButton>
    </div>
</asp:Content>
