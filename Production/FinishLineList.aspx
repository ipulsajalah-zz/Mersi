<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="FinishLineList.aspx.cs" Inherits="DotMercy.custom.Production.FinishLineList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Finish line List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div style="text-align: center; padding-bottom: 10px">
        <dx:ASPxLabel ID="lblPosition" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
      <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>
    <dx:ASPxGridView ID="gridCondition" ClientInstanceName="gridPosition" runat="server" AutoGenerateColumns="False" KeyFieldName="ItemId" Width="100%">
        <Columns>
            <%--<dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Caption="Select">
            </dx:GridViewCommandColumn>--%>
            <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ItemId">
              <%--  <DataItemTemplate>
                    <dx:ASPxButton ID="Btn_ItemId" runat="server" RenderMode="Link" Text='<%#Eval("ItemId") %>' OnClick="Btn_ItemId_Click">
                    </dx:ASPxButton>
                </DataItemTemplate>--%>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ItemId" ReadOnly="True" Visible="False" VisibleIndex="2"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ModelName" VisibleIndex="3"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VariantName" VisibleIndex="4"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FINNumber" VisibleIndex="1">
                 <DataItemTemplate>
                    <dx:ASPxButton ID="Btn_ItemId" runat="server" RenderMode="Link" Text='<%#Eval("FINNumber") %>' OnClick="Btn_ItemId_Click">
                    </dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VINNumber" VisibleIndex="5">
                
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssemblySectionName" VisibleIndex="6"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="7"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StationId" ReadOnly="true" Visible="false" VisibleIndex="8"></dx:GridViewDataTextColumn>
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
    <dx:ASPxButton ID="btnExportExcel" runat="server" Text="Export Excel" OnClick="ExportExcel_Click"></dx:ASPxButton>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="gridCondition" FileName="FinishLineList"></dx:ASPxGridViewExporter>
  <%--  <div style="text-align: right; margin-top: 10px">
        <dx:ASPxButton ID="btnRectification" runat="server" Text="Send to Rectification" Visible="false"
            OnClick="btnRectification_Click">
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_Click" Visible="false"></dx:ASPxButton>
    </div>--%>
</asp:Content>
