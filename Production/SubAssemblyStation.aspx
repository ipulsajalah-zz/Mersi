<%@ Page Title="SubAssemblyStation" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SubAssemblyStation.aspx.cs" Inherits="DotMercy.custom.Production.SubAssemblyStation" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageTitle" runat="server">
    Sub Assembly Information
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="text-align: center">
        <dx:ASPxLabel ID="lblStation" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    </div>
    <div style="margin-bottom: 10px">
        <dx:ASPxButton ID="Btn_Back" runat="server" OnClick="Btn_Back_Click" Text="Back To DashBoard"></dx:ASPxButton>
        <dx:ASPxButton ID="btnNew" runat="server" Text="New" AutoPostBack="false">
            <ClientSideEvents Click="function (s, e) {pcAddSubAssy.Show();}" />
        </dx:ASPxButton>
    </div>
    <dx:ASPxGridView ID="gridSubAssembly" ClientInstanceName="gridSubAssembly" runat="server"
        AutoGenerateColumns="False" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ItemId" ReadOnly="false" Visible="false" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ChassisNumber" VisibleIndex="2">
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnVehicle" runat="server" RenderMode="Link" Text='<%#Eval("ChassisNumber") %>'
                        CommandArgument='<%#Eval("ItemId")%>' AutoPostBack="False" OnClick="btnVehicle_Click">
                    </dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Action" Width="80px" VisibleIndex="14">
                <DataItemTemplate>
                    <dx:ASPxButton AutoPostBack="false" HorizontalAlign="Center" Text="Cancel" ID="Cancel" runat="server" OnClick="Cancel_Click" ClientInstanceName="Cancel">
                  
                    </dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VehicleNumber" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PackingMonth" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Model" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Variant" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StationId" Visible="false" VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EngineNo" Visible="true" VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrderNo" Visible="true" VisibleIndex="11">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IsInStock" Visible="true" VisibleIndex="12">
            </dx:GridViewDataTextColumn>
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

    <dx:ASPxPopupControl runat="server" ID="pcAddSubAssy" ClientInstanceName="pcAddSubAssy"
        HeaderText="Add New SubAssembly" Modal="true" PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" OnWindowCallback="pcAddSubAssy_WindowCallback">
        <ClientSideEvents CloseButtonClick="function(s, e) { s.PerformCallback(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxFormLayout ID="layoutAddSubAssy" runat="server">
                    <Items>
                        <dx:LayoutItem Caption="Create Part for Serial Number">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox runat="server" ID="tbAddSubAssy" Width="250px" AutoPostBack="false"></dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div style="text-align: right">
                    <dx:ASPxButton runat="server" AutoPostBack="false" ID="btnAddSubAssy" Text="Submit" OnClick="btnAddSubAssy_Click"></dx:ASPxButton>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
