<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="DailyProductionRequirements.aspx.cs" Inherits="DotMercy.custom.DailyProductionRequirements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Daily Production Requirements
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <h2 id="lblHeader" runat="server">View Edit Production Daily </h2>
    <dx:ASPxGridView ID="masterGrid" runat="server" AutoGenerateColumns="false" ClientInstanceName="dprGrid" KeyFieldName="Id" CssClass="gridView">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="1" ReadOnly="true">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProductionMonth" VisibleIndex="2" ReadOnly="true"
                Settings-AllowHeaderFilter="True"
                Settings-HeaderFilterMode="CheckedList" />
            <dx:GridViewDataTextColumn FieldName="FileName" VisibleIndex="3" ReadOnly="true" />
            <dx:GridViewDataDateColumn FieldName="UploadDate" VisibleIndex="4" ReadOnly="true">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
            </dx:GridViewDataDateColumn>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Height="200" />
        </SettingsPopup>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="dprdsGrid" runat="server" KeyFieldName="Id" Width="100%"
                    OnDataBinding="dprdsGrid_DataBinding">
                    <Columns>
                        <dx:GridViewCommandColumn ShowEditButton="true" VisibleIndex="0" />
                        <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="1" ReadOnly="true" Visible="false">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Production Date" FieldName="ProductionDate" VisibleIndex="2" ReadOnly="true"
                            Settings-AllowHeaderFilter="True"
                            Settings-HeaderFilterMode="CheckedList">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Production Line" FieldName="ProductionLineId" VisibleIndex="3" ReadOnly="true"
                            Settings-AllowHeaderFilter="True"
                            Settings-HeaderFilterMode="CheckedList" />
                        <dx:GridViewDataTextColumn Caption="Station Action" FieldName="StationAct" VisibleIndex="4" ReadOnly="true" />
                        <dx:GridViewDataTextColumn Caption="Target" FieldName="Target" VisibleIndex="5" />
                        <dx:GridViewDataTextColumn Caption="Target Accumulation" FieldName="TargetAccumulation" VisibleIndex="6" />
                        <dx:GridViewDataTextColumn Caption="Achieve" FieldName="Achieve" VisibleIndex="7" />
                        <dx:GridViewDataTextColumn Caption="Achieve Accumulation" FieldName="AchieveAccumulation" VisibleIndex="8" />
                        <dx:GridViewDataTextColumn Caption="Surplus" FieldName="Surplus" VisibleIndex="9" />
                    </Columns>
                    <SettingsDetail ShowDetailRow="false" ShowDetailButtons="false" />
                    <Settings ShowFooter="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsPopup>
                        <EditForm Width="600" />
                    </SettingsPopup>
                    <SettingsPager Position="TopAndBottom">
                        <PageSizeItemSettings Items="10,20,50" Visible="true" />
                    </SettingsPager>
                </dx:ASPxGridView>
                </div>
            </DetailRow>
        </Templates>
        <Paddings Padding="0px" />
        <Border BorderWidth="0px" />
        <BorderBottom BorderWidth="1px" />
        <Settings ShowGroupPanel="True" />
        <SettingsDetail ShowDetailRow="true" />
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Items="10,20,50" Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <SettingsSearchPanel Visible="True" />
        <Styles>
            <Header HorizontalAlign="Center" />
        </Styles>
    </dx:ASPxGridView>
</asp:Content>

