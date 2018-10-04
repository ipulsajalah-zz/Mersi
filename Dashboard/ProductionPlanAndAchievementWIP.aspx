<%@ Page Language="C#" MasterPageFile="~/Andon.master" AutoEventWireup="true" CodeBehind="ProductionPlanAndAchievementWIP.aspx.cs" Inherits="DotMercy.custom.Dashboard.ProductionPlanAndAchievementWIP" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Data.Linq" Assembly="DevExpress.Web.v18.1, Version=18.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent"  AutoGenerateColumns="true" runat="server">
    <div>
        <style>
            .dxgvControl_DevEx{
                margin:auto;
            }
        </style>
    <span style="font-size: 18px; float: left; font-weight: bolder; padding-left: 30px;padding-bottom:10px;padding-top:10px">DashBoard Archievement Detail</span>
    <div style="width: 100%; float: left; padding-left: 30px; display: inline-flex;white-space: nowrap; padding-bottom: 10px;">
        <dx:ASPxLabel runat="server" ID="captionHeaderTitle" Text="" Width="100" Font-Size="Small"></dx:ASPxLabel>
    </div>
    <dx:ASPxGridView ID="grvDashboardArchievementDetail" Width="97%" runat="server" Settings-HorizontalScrollBarMode="Auto">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ProductionType" Visible="True" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Type" Visible="True" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ModelName" Visible="True" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Variant" Visible="True" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VINNumber" Visible="True" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FINNumber" Visible="True" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CommnosNumber" Visible="True" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EngineNo" Visible="True" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProductionLineId" Caption="Production Line" Visible="True" VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StationAct" Visible="True" VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IsRectification" Visible="false" VisibleIndex="10">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior  ColumnResizeMode="Control"/>
       <SettingsPager AlwaysShowPager="True" PageSize="20">
            <PageSizeItemSettings Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <SettingsEditing Mode="Batch">
        </SettingsEditing>
        <Settings ShowStatusBar="Hidden" ShowGroupPanel="true" />
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        <SettingsSearchPanel Visible="true"/>
    </dx:ASPxGridView>
        </div>
    </asp:Content>
