<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ReportGWIS.aspx.cs" Inherits="Ebiz.WebForm.custom.GWIS.ReportGWIS" %>
<%@ Register assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server"> Irregular Alteration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" SettingsReportViewer-PrintUsingAdobePlugIn="false" runat="server" ReportTypeName="Ebiz.WebForm.Reports.GWISReport">
    </dx:ASPxDocumentViewer>
</asp:Content>
