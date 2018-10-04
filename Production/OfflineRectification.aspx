<%@ Page Title="Production Dashboard Detail" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="OfflineRectification.aspx.cs" Inherits="DotMercy.custom.Production.OfflineRectification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Offline Rectification
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="text-align: center; padding-bottom: 10px">
        <dx:ASPxLabel ID="lblPosition" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
    <div style="margin-bottom:10px">
        <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>
    </div>
    <dx:ASPxGridView ID="gridCondition" ClientInstanceName="gridPosition" runat="server" AutoGenerateColumns="False" KeyFieldName="ItemId" Width="100%">
        <Columns>
            <%--<dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Caption="Select">
            </dx:GridViewCommandColumn>--%>
            
            <dx:GridViewDataTextColumn FieldName="ItemId" ReadOnly="True" Visible="False" VisibleIndex="14"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ModelName" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VariantName" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FINNumber" VisibleIndex="0">
                 <DataItemTemplate>
                    <dx:ASPxButton ID="Btn_ItemId" runat="server" RenderMode="Link" Text='<%#Eval("FINNumber") %>' OnClick="Btn_ItemId_Click">
                    </dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VINNumber" VisibleIndex="1">
           
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="LineId" Caption="Production Line" VisibleIndex="5" GroupIndex="0" SortIndex="0" SortOrder="Ascending"></dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn FieldName="StationName" VisibleIndex="6"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssemblySectionName" VisibleIndex="7"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="8"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StationId" ReadOnly="true" Visible="false" VisibleIndex="9"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="InTimeRework" VisibleIndex="10"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OutTimeRework" VisibleIndex="11"></dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Items="10,20,50" Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <Settings ShowGroupPanel="True" />
        <SettingsSearchPanel Visible="True" />
        <Styles>
            <Header HorizontalAlign="Center" />
        </Styles>
    </dx:ASPxGridView>
    <div style="text-align: right; margin-top: 10px">
        <dx:ASPxButton ID="btnRectification" runat="server" Text="Send to Rectification" Visible="false"
            OnClick="btnRectification_Click">
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_Click" Visible="false"></dx:ASPxButton>
    </div>
</asp:Content>
