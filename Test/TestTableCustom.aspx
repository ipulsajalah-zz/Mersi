<%@ Page Title="" UnobtrusiveValidationMode="None" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="TestTableCustom.aspx.cs" Inherits="Ebiz.WebForm.custom.Test.TestTableCustom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Expires" content="0" />

        <script type="text/javascript">
            function testAlert(s,e)
            {
                alert("Hello World: " + e + "!");
            }
        </script>


    </head>
    <body>

    </body>
    </html>
</asp:Content>
