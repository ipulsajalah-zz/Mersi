<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="RIC.aspx.cs" Inherits="DotMercy.custom.Report.RIC" %>
<%@ Register assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server"> RIC 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <dx:ASPxButton ID="BtnDesign" runat="server" Text="Design" OnClick="BtnDesign_Click"></dx:ASPxButton>
    
        <br />
    <br />
    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" SettingsReportViewer-PrintUsingAdobePlugIn="false" runat="server">
    </dx:ASPxDocumentViewer>
</asp:Content>
