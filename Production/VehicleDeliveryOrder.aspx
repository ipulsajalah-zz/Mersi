<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="VehicleDeliveryOrder.aspx.cs" Inherits="DotMercy.custom.VehicleDeliveryOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Vehicle Order Delivery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .categoryTable {
            width: 100%;
        }

            .categoryTable .imageCell {
                padding: 2px;
            }

            .categoryTable .textCell {
                padding-left: 20px;
                width: 100%;
            }

        .textCell .label {
            color: #969696;
        }

        .textCell .description {
            font-size: 13px;
            width: 230px;
        }

        .btnInline {
            display: inline-table;
        }

        .txtbox {
            margin-right: 10px;
        }

        .lbl {
            margin: 5px 0px;
        }
    </style>
    <script>
        function OnBeginCallback(s, e) {
            console.log('Begin callback: ' + e.command);
            s.isError = false;
            command = e.command;
        }
        function OnEndCallback(s, e) {
            if (!!s.cpalertmessage) {
                alert(s.cpalertmessage);
            }
        }
    </script>
    <span style="font-size: 18px; float: left; font-weight: bolder; padding-bottom: 15px;">Vehicle Delivery Order</span>
    <div style="width: 100%; display: inline-flex; padding-bottom: 15px;">
        <dx:ASPxLabel runat="server" Text="Year:" CssClass="lbl"></dx:ASPxLabel>
         <dx:ASPxLabel ID="lblGetAssemblyTypeId" Visible="false" runat="server"></dx:ASPxLabel>
             
        <dx:ASPxComboBox ID="cmbYear" runat="server" ValueType="System.String" Paddings-Padding="5px" CssClass="txtbox">
        </dx:ASPxComboBox>
        <dx:ASPxLabel runat="server" Text="Month:" CssClass="lbl"></dx:ASPxLabel>
        <dx:ASPxComboBox ID="cmbMonth" runat="server" ValueType="System.String" OnSelectedIndexChanged="cmbMonth_SelectedIndexChanged" AutoPostBack="true" NullText="Month" Paddings-Padding="5px" CssClass="txtbox">
        </dx:ASPxComboBox>
        <dx:ASPxComboBox ID="cmbDay" Enabled="false" runat="server" ValueType="System.String" NullText="Day" Paddings-Padding="5px" CssClass="txtbox">
        </dx:ASPxComboBox>
        <dx:ASPxButton ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="txtbox" Text="Clear">
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" CssClass="txtbox" Text="Search">
        </dx:ASPxButton>
        <div style="padding-left: 330px">
            <dx:ASPxButton ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" Text="Export To Excel">
            </dx:ASPxButton>
        </div>
    </div>
    <div style="width: 100%; display: inline-flex; padding-bottom: 10px;">
        <dx:ASPxLabel runat="server" Text="Production No" Width="100"></dx:ASPxLabel>
        <dx:ASPxTextBox ID="txtReprint" runat="server"></dx:ASPxTextBox>
        <div style="padding-left: 10px">
            <dx:ASPxButton ID="btnReprint" runat="server" OnClick="btnReprint_Click" Text="Reprint">
            </dx:ASPxButton>
        </div>
    </div>

    <dx:ASPxGridView ID="grid" runat="server"  ClientInstanceName="gridclient" Width="100%" AutoGenerateColumns="False" OnInit="grid_Init"
        OnCustomCallback="grid_CustomCallback" OnCustomButtonCallback="grid_CustomButtonCallback" OnHtmlRowPrepared="grid_HtmlRowPrepared"
        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" >
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Id" Visible="false" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CatalogueName" VisibleIndex="1" Caption="Type" Width="80">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CommNos" VisibleIndex="2" Caption="Commnos No">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FINNumber" VisibleIndex="3" Caption="Production No">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="VINNumber" VisibleIndex="4" Caption="Chassis No">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="EngineNo" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProdOutDate" VisibleIndex="6" Width="70" CellStyle-Wrap="True" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="dd/MM/yyyy" SortIndex="0" SortOrder="Descending">
<PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>

<CellStyle HorizontalAlign="Center" Wrap="True"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrderNo" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BuyOutDate" Caption="Buy Off Date" VisibleIndex="8" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="dd/MM/yyyy">
<PropertiesTextEdit DisplayFormatString="dd/MM/yyyy - HH:mm:ss " ></PropertiesTextEdit>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="BPK" Caption="Confirm BPK" VisibleIndex="9" Width="90" HeaderStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="{0}">
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnBPK" Visible="false" runat="server" RenderMode="Link" Text="Confirm" OnClick="btnBPK_OnClick">
                        <Image Height="12px" Url="~/Content/Images/glyphicons-145-folder-open.png" Width="16px">
                        </Image>
                    </dx:ASPxButton>
                    <dx:ASPxLabel ID="lblBPK" Visible="false" runat="server" RenderMode="Link" Text="Confirm">
                    </dx:ASPxLabel>
                </DataItemTemplate>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PrintNik" Caption="Print NIK" VisibleIndex="10" Width="150" HeaderStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="{0:dd/MMMM/yyyy}" > 
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnPrintNIK" Visible="false" runat="server" CommandArgument='<%# Eval("Id") %>' RenderMode="Link" Text="Print" OnClick="btnPrintNIK_OnClick">
                        <Image Height="12px" Url="~/Content/Images/glyphicons-145-folder-open.png" Width="16px">
                        </Image>
                    </dx:ASPxButton>
                    <dx:ASPxLabel ID="lblPrintNIK" Visible="false" runat="server" CommandArgument='<%# Eval("Id") %>' RenderMode="Link" Text="Print">
                    </dx:ASPxLabel>
                </DataItemTemplate>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="GRDate" Caption="Good Received" VisibleIndex="11" Width="80" HeaderStyle-Wrap="True" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy - HH:mm:ss " >
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Send To Marketing" FieldName="SendToMarketing" VisibleIndex="12" Width="90" HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center">
                <PropertiesTextEdit DisplayFormatString="{0:dd/MMMM/yyyy}">
                </PropertiesTextEdit>
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnSend" Visible="false" runat="server" CommandArgument='<%# Eval("Id") %>' RenderMode="Link" Text="Send" OnClick="btnSend_OnClick">
                        <Image Height="12px" Url="~/Content/Images/glyphicons-145-folder-open.png" Width="16px">
                        </Image>
                    </dx:ASPxButton>
                    <dx:ASPxLabel ID="lblSend" Visible="false" runat="server" CommandArgument='<%# Eval("Id") %>' RenderMode="Link" Text="Confirm">
                    </dx:ASPxLabel>
                </DataItemTemplate>

<HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ReceivedByMarketing" VisibleIndex="13" Width="90" Caption="Received By Marketing" HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center">
                <DataItemTemplate>
                    <dx:ASPxButton ID="btnRCV" Visible="false" runat="server" RenderMode="Link" Text="Received" CommandArgument='<%# Eval("Id") %>' OnClick="btnRCV_OnClick">
                        <Image Height="12px" Width="16px" Url="~/Content/Images/glyphicons-145-folder-open.png">
                        </Image>
                    </dx:ASPxButton>
                    <dx:ASPxLabel ID="lblRCV" Visible="false" runat="server" CommandArgument='<%# Eval("Id") %>' RenderMode="Link" Text="Confirm">
                    </dx:ASPxLabel>
                </DataItemTemplate>

<HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager AlwaysShowPager="True" PageSize="20">
        </SettingsPager>
        <SettingsEditing Mode="Batch">
        </SettingsEditing>
        <Settings ShowStatusBar="Hidden" ShowGroupPanel="true" />
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        <SettingsSearchPanel Visible="true" />
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="exportGrid" runat="server" GridViewID="grid">
    </dx:ASPxGridViewExporter>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand=" SELECT
                        A.[CatalogueName],
                        A.[FINNumber], 
	                    A.[VINNumber], 
	                    A.[CommNos], 
	                    A.[EngineNo], 
	                    A.[GRDate], 
	                    A.[BuyOutDate], 
	                    A.[ProdOutDate], 
	                    A.[Remark], 
	                    (A.NIKNumber+' '+Convert(nvarchar(30), A.PrintNik,103)+' '+ RIGHT(A.PrintNIK,7) +' by '+A.PrintNIKBy)[PrintNik], 
	                    (Convert(nvarchar(30),A.SendToMarketing,103)+' by '+A.SendToMarketingBy)[SendToMarketing], 
	                    (Convert(nvarchar(30),A.ReceivedByMarketing,103)+' by '+A.ReceivedByMarketingBy)[ReceivedByMarketing], 
	                    A.[Id], 
	                    (Convert(nvarchar(30),A.BPK,103)+' by ' +A.BPKBy)[BPK],
	                    B.OrderNo
                    FROM VPCStorage A
                    LEFT JOIN ProductionSequenceDetail B ON A.FINNumber = B.SerialNumber
                    ">

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsModel" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id, ModelName FROM Models ORDER BY ModelName"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsType" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id, Type FROM Types ORDER BY Type"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsVPCLocation" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id, BuildingName FROM VPCLocation"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsVarian0" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT Id, Variant FROM Variants"></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlVPCMovementStatus" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id], [MovementStatus] FROM [VPCMovementStatus]"></asp:SqlDataSource>

</asp:Content>
