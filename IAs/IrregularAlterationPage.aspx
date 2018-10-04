<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="IrregularAlterationPage.aspx.cs" Inherits="Ebiz.WebForm.custom.IAs.IrregularAlterationPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Irregular Alteration
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        @font-face {
            font-family: CorpoS;
            src: url(../../Content/Font/c063003t.ttf);
        }

        div {
            font-family: CorpoS;
        }

        .nav > li > a {
            position: relative;
            display: block;
            padding: 8px 8px;
        }

        .col-xs-12 {
            background-color: #fff;
        }

        #sidebar {
            height: 100%;
            padding-right: 0;
            padding-top: 10px;
            /*margin-left: -35px;*/
        }

            #sidebar .nav {
                width: 95%;
            }

            #sidebar li {
                border: 0 #f2f2f2 solid;
                border-bottom-width: 1px;
            }

        #submenu1 {
            padding-left: 5px;
        }

        .lichild {
            margin-left: 10px;
            font-size: 12px;
            font-family: Segoe UI;
        }
        /* collapsed sidebar styles */
        @media screen and (max-width: 767px) {
            .row-offcanvas {
                position: relative;
                -webkit-transition: all 0.25s ease-out;
                -moz-transition: all 0.25s ease-out;
                transition: all 0.25s ease-out;
            }

            .row-offcanvas-right .sidebar-offcanvas {
                right: -41.6%;
            }

            .row-offcanvas-left .sidebar-offcanvas {
                left: -41.6%;
            }

            .row-offcanvas-right.active {
                right: 41.6%;
            }

            .row-offcanvas-left.active {
                left: 41.6%;
            }

            .sidebar-offcanvas {
                position: absolute;
                top: 0;
                width: 41.6%;
            }

            #sidebar {
                padding-top: 0;
            }
        }
    </style>
    <script type="text/javascript">

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

        function imgGWIS_Click(sender, visibleIndex, buttonId) {
            var grid = eval(sender);
            grid.GetRowValues(visibleIndex, "Id", imgGWIS_OnGetRowValues);
        }


        function CallPopUP(sender, visibleIndex, buttonId) {
            var grid = eval(sender);
            grid.GetRowValues(visibleIndex, "Id", CallPopUP_OnGetRowValues);
        }

        function CallPopUP_OnGetRowValues(value) {
            console.log(value);
            popupReport.SetContentUrl("/Reports/ReportIAEdit.aspx?Id=" + value);
            popupReport.SetHeaderText("Report IA");
            popupReport.Show();
        }



        function ConfirmCreateDoc() {
            var mhl = document.getElementById("=lblFINNumber.ClientID").innerHTML;
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm_value.value = "Yes") {
                window.open = "IrregularAlterationEdit.aspx";

            } else {
                window.open = "IrregularAlterationPage.aspx";
            }
            document.forms[0].appendChild(confirm_value);


        }
    </script>
    <div class="container" style="padding: 0;">
        <div class="row row-offcanvas row-offcanvas-left">

            <!-- sidebar -->


            <!-- main area -->
            <%--<div class="col-xs-12 col-sm-10">--%>
            <h2>Irregular Alteration</h2>
            <%--<div id="MainContainer" runat="server">--%>
            <dx:ASPxLabel runat="server" ID="lblHeader" CssClass="headerText" />
            <dx:ASPxButton runat="server" Text="New" Width="100px" ID="btnNew" OnClick="btnNew_Click"></dx:ASPxButton>
            <br />
            <dx:ASPxGridView ID="gvIrregAltvw" Visible="false" runat="server" DataSourceID="SqlDataSourceIA" Width="100%" AutoGenerateColumns="true" KeyFieldName="Id"
                ClientInstanceName="gvIrregAltvw" OnHtmlDataCellPrepared="gvIrregAltvw_HtmlDataCellPrepared">
                <SettingsDetail ShowDetailRow="false" />
                <SettingsBehavior AutoExpandAllGroups="true" />

                <Columns>
                    <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="1" ReadOnly="True" Visible="false">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="PPD" Caption="PPD PA PRE-INFO" VisibleIndex="2" Width="100" ShowInCustomizationForm="True" ExportCellStyle-BorderSides="None">
                        <ExportCellStyle BorderSides="None"></ExportCellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Year" Caption="Year" VisibleIndex="2" Width="100" ShowInCustomizationForm="True" ExportCellStyle-BorderSides="None">
                        <ExportCellStyle BorderSides="None"></ExportCellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="InternalEpcNumber" Caption="Task Type" VisibleIndex="2" Width="100" ShowInCustomizationForm="True" ExportCellStyle-BorderSides="None">
                        <ExportCellStyle BorderSides="None"></ExportCellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="TextStatus" Caption="Status" VisibleIndex="3" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ValidPeriodFrom" Caption="Valid From" VisibleIndex="4" ShowInCustomizationForm="True" PropertiesTextEdit-DisplayFormatString="MM/dd/yyyy" HeaderStyle-Wrap="True">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                        </PropertiesTextEdit>
                        <HeaderStyle Wrap="True"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ValidPeriodTo" Caption="Valid To" VisibleIndex="5" ShowInCustomizationForm="True" PropertiesTextEdit-DisplayFormatString="MM/dd/yyyy" HeaderStyle-Wrap="True">
                        <PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
                        </PropertiesTextEdit>
                        <HeaderStyle Wrap="True"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="InfoNumber" Caption="Info No" VisibleIndex="6" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PartNumber" Caption="Part Number" VisibleIndex="7" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PartDescription" Caption="Description" VisibleIndex="8" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Type" VisibleIndex="9" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ImplementationDate" VisibleIndex="10" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>


                    <dx:GridViewDataTextColumn VisibleIndex="13" Caption="Print">
                        <DataItemTemplate>
                            <a onclick="CallPopUP(<%# Eval("Id") %>);">[Print]</a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="0" Width="50">
                        <HeaderCaptionTemplate>
                            <dx:ASPxHyperLink ID="btnAdd" runat="server" Text="New">
                                <ClientSideEvents Click="function (s, e) { window.open ('IrregularAlterationEdit.aspx', '_blank');}" />
                            </dx:ASPxHyperLink>
                        </HeaderCaptionTemplate>
                        <DataItemTemplate>
                            <a class="btn btn-primary btn-xs" onclick="window.open ('IrregularAlterationEdit.aspx?id=<%# Eval("Id") %>', '_blank');"><i class="glyphicon glyphicon-eye-open"></i>View</a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="True" />
                <SettingsPager AlwaysShowPager="True" PageSize="20">
                    <PageSizeItemSettings Visible="True">
                    </PageSizeItemSettings>
                </SettingsPager>
                <Settings ShowGroupPanel="True" />
                <SettingsText ConfirmDelete="Are you wanto to delete this data ?" />
                <SettingsSearchPanel Visible="True" ShowApplyButton="True" ColumnNames="Internal Number;Status" />
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="exportGrid" runat="server" GridViewID="gvIrregAltvw">
            </dx:ASPxGridViewExporter>



            <dx:ASPxPopupControl ID="popupReport" ClientInstanceName="popupReport" runat="server" AllowDragging="false" AutoUpdatePosition="True"
                CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" HeaderText="ReportIA" Modal="True"
                PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="40" MinHeight="20">
                <ContentStyle Paddings-Padding="0" />
                <ClientSideEvents Shown="onPopupShown" />
            </dx:ASPxPopupControl>

            <asp:SqlDataSource ID="SqlDataSourceIA" runat="server"
                ConnectionString="<%$ ConnectionStrings:AppDb %>"
                SelectCommand="
SELECT distinct  
     CASE WHEN LEFT(IA.InternalEpcNumber, 3) = 'PI' THEN 'PRE-INFORMATION'
     WHEN LEFT(IA.InternalEpcNumber, 3) = 'PPD' THEN 'CV PRODUCTION PART DISCREPANCIES'
	 WHEN LEFT(IA.InternalEpcNumber, 3) = 'PA' THEN 'PREVENTIVE ACTION'
	 WHEN LEFT(IA.InternalEpcNumber, 3) = 'TI' THEN 'TICKET'
	 ELSE '' END 'PPD',
     ISNULL(YEAR(isnull(IA.ValidPeriodTo,ia.ValidPeriodFrom)),2099) [Year],
      ia.*,	
	  case when (StatusID = 1) then 'Open'
	  else case when (StatusID = 3) then 'InProgress'
	  else case when (StatusID = 2) then 'Close'
	  else case when (StatusID = 1) then 'Cancel'
	  else ''
	  end end end end as TextStatus,

                               (SELECT distinct STUFF ((SELECT '; ' + (b.Description)
                                from IAs a 
                                inner join IAParts b on a.Id = b.IAId  
                                where IAId = ia.Id
                                FOR XML PATH('')), 1, 1, '') as PartDescription)[PartDescription],
								 (SELECT distinct STUFF ((SELECT '; ' + (b.PartNumber)
                                from IAs a 
                                inner join IAParts b on a.Id = b.IAId  
                                where IAId = ia.Id
                                FOR XML PATH('')), 1, 1, '') as PartNumber)[PartNumber],
							  	(SELECT distinct STUFF ((SELECT '; ' + (c.Model)
                                from IAs a 
                                inner join IAModels b on a.Id = b.IAId  
								inner join CatalogModels c on b.CatalogModelId = c.Id
                                where IAId = ia.Id
                                FOR XML PATH('')), 1, 1, '') as Type)[Type]
                                from IAs ia
                                left join IAStations on ia.Id = IAStations.IAId
								left join IAParts on ia.Id = IAParts.IAId
								left join IAModels on ia.Id = IAModels.IAId
								left join CatalogModels on IAModels.CatalogModelId = CatalogModels.Id
						
"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sdsCboYear" runat="server"
                ConnectionString="<%$ ConnectionStrings:AppDb %>"
                SelectCommand="Select distinct Cast('20'+Left(substring(InternalEpcNumber, CHARINDEX(' ', InternalEpcNumber)+1, 
                               len(InternalEpcNumber)-(CHARINDEX(' ', InternalEpcNumber)-1)),2) as nvarchar) Year from IAs"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sdsStatus" runat="server"
                ConnectionString="<%$ ConnectionStrings:AppDb %>"
                SelectCommand="select distinct
case when(a.StatusId = 1) then 'Draft'
else case when(a.StatusId = 2) then 'Open'
else case when(a.StatusId = 3) then 'Close'
else 'All'
end end end as StatusIA,
                  a.StatusId
from IAs as a"></asp:SqlDataSource>


            <%--</div>
                <!-- /.col-xs-12 main -->
            </div>--%>
            <!--/.row-->
        </div>

    </div>
</asp:Content>
