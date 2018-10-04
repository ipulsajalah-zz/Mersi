<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="CpaChecklist.aspx.cs" Inherits="Ebiz.WebForm.custom.QM.CpaChecklist" %>

<asp:Content ID="pageTitle" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
          <script type="text/javascript">
              var idx = '';
            function onPopupShown(s, e) {
                var windowInnerWidth = window.innerWidth - 40;
                if (s.GetWidth() != windowInnerWidth) {
                    s.SetWidth(windowInnerWidth);
                    s.UpdatePosition();
                }

                var windowInnerHeight = window.innerHeight - 20;
                if (s.GetHeight() != windowInnerHeight) {
                    s.SetHeight(windowInnerHeight);
                    s.UpdatePosition();
                }
            }

            function CallAddCPA(sender, visibleIndex, buttonId) {
                //alert(visibleIndex);
                //alert(buttonId);
               // var idtr = $('#' + buttonId).closest('tr').attr('id');
                //alert(idtr);
               // idx =
               // $('#'+ idtr + ' td:eq(3)').html('kkkkk');
                var grid = eval(sender);
                grid.GetRowValues(visibleIndex, "Id", CallAddCPA_OnGetRowValues);
            }

            function CallAddCPA_OnGetRowValues(value) {
                popupAddCPA.SetContentUrl("/custom/QM/ExistingCpa.aspx?Id=" + value);
                popupAddCPA.SetHeaderText("Add CPA");
                popupAddCPA.Show();
            }

    


            function CallExistingCPA(sender, visibleIndex, buttonId) {
                var grid = eval(sender);
                grid.GetRowValues(visibleIndex, "Id", CallExistingCPA_OnGetRowValues);
            }

            function CallExistingCPA_OnGetRowValues(value) {
                popupAddCPA.SetContentUrl("/custom/QM/ExistingCpa.aspx?Id=" + value);
                popupExistingCPA.SetHeaderText("Existing CPA");
                popupExistingCPA.Show();
            }
    
        </script>
    
        <dx:ASPxPopupControl ID="popupAddCPA" ClientInstanceName="popupAddCPA" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="40" MinHeight="20">
            <ContentStyle Paddings-Padding="0" />
            <ClientSideEvents Shown="onPopupShown" CloseUp="function(s, e) { ChecklistDetailDetailGrid.Refresh(); }" />
             
        </dx:ASPxPopupControl>

    
        <dx:ASPxPopupControl ID="popupExistingCPA" ClientInstanceName="popupExistingCPA" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="40" MinHeight="20">
            <ContentStyle Paddings-Padding="0" />
              <ClientSideEvents Shown="onPopupShown" CloseUp="function(s, e) { ChecklistDetailDetailGrid.Refresh(); }" />
        </dx:ASPxPopupControl>
</asp:Content>