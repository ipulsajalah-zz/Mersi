<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="DPRUpload.aspx.cs" Inherits="DotMercy.custom.DPRUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Daily Production Requirement
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var defaultMonth = "Jan";
        var currentMonthElement = null;
        function OnClick(s, e) {
            currentMonthElement = s;
            var month = !!currentMonthElement ? currentMonthElement.GetText() : defaultMonth;
            var period = lblYear.GetText() + getMonthFromString(month);

            var myHidden = document.getElementById('<%= hdnProdMonth.ClientID %>');
            if (myHidden)//checking whether it is found on DOM, but not necessary
            {
                myHidden.value = period;
            }
            dde_pmonth.SetText(period);
            dde_pmonth.HideDropDown();

        }

        function OnPrevClick(s, e) {
            lblYear.SetText(parseInt(lblYear.GetText()) - 1);
            currentMonthElement = null;
        }
        function OnNextClick(s, e) {
            lblYear.SetText(parseInt(lblYear.GetText()) + 1);
            currentMonthElement = null;
        }
        function getMonthFromString(mon) {
            return ("0" + (new Date(Date.parse(mon + " 1, 2012")).getMonth() + 1)).slice(-2);
        }
        ///production Month

        function onFileUploadComplete(s, e) {
            var callbackData = e.callbackData;
            console.log(callbackData);

            if (callbackData != '') {
                FileLabel.SetText("File to be upload : " + callbackData);
                FileDPRUpload.SetEnabled(true);
            } else {
                FileDPRUpload.SetEnabled(false);
            }
        }

    </script>
    <style type="text/css">
        .cell .buttonMonth {
            border-width: 1px;
            width: 50px;
            text-align: center;
            text-decoration: none;
            color: inherit;
        }

        .tab {
            width: 100%;
            border-color: #600;
            border-width: 0 0 1px 1px;
            border-style: solid;
        }

        .cell {
            border-color: #600;
            border-width: 1px 1px 0 0;
            border-style: solid;
            margin: 0;
            padding: 4px;
            background-color: #eee6a3;
        }
    </style>
    <h2 id="lblHeader" runat="server">Upload Production Planning</h2>

    <dx:ASPxFormLayout ID="frmLayout" runat="server">
        <Items>
            <dx:LayoutItem Caption="Production Month">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxDropDownEdit ID="dde_pmonth" OnInit="dde_Init" runat="server" ClientInstanceName="dde_pmonth">
                        </dx:ASPxDropDownEdit>
                        <asp:HiddenField ID="hdnProdMonth" runat="server"></asp:HiddenField>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Select Files">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxUploadControl ID="FileDPRBrowse" runat="server" ClientInstanceName="FileDPRBrowse"
                            AutoStartUpload="true" ShowProgressPanel="true" FileUploadMode="OnPageLoad" NullText="Click here to upload file.."
                            OnFileUploadComplete="FileDPRBrowse_FileUploadComplete">
                            <ClientSideEvents FileUploadComplete="onFileUploadComplete" />
                            <ValidationSettings AllowedFileExtensions=".xlsx, .xls" />
                        </dx:ASPxUploadControl>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxButton ID="FileDPRUpload" runat="server" AutoPostBack="false" ClientInstanceName="FileDPRUpload" ClientEnabled="false"
                            Text="Upload" OnClick="FileDPRUpload_Click">
                        </dx:ASPxButton>
                        
                        <dx:ASPxButton ID="FileDelete" runat="server" AutoPostBack="false" ClientInstanceName="FileDPRDelete" ClientEnabled="false"
                            Text="Delete" OnClick="FileDelete_Click" Visible="False">
                        </dx:ASPxButton>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
             <dx:LayoutItem Caption="">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">                       
                        <dx:ASPxLabel runat="server" ID="FileLabel" ClientInstanceName="FileLabel"></dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
        </Items>
    </dx:ASPxFormLayout>
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="100%">
        <Items>
            <dx:LayoutItem Caption="">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">                       
                        <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" OnItemDeleting="ASPxFileManager1_ItemDeleting" Width="100%">
                            <Settings EnableMultiSelect="True" RootFolder="~\upload\DPR\" ThumbnailFolder="~\Thumb\" InitialFolder="~\upload\DPR\" AllowedFileExtensions=".xls,.xlsx" />
                            <SettingsEditing AllowDelete="True" />
                            <SettingsUpload Enabled="False">
                            </SettingsUpload>
                        </dx:ASPxFileManager>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
        </Items>
    </dx:ASPxFormLayout>
</asp:Content>
