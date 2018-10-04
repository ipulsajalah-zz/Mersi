using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Data.Linq;
using DevExpress.Utils;
using DevExpress.Web;
//using DevExpress.XtraGauges.Core.Drawing;
using System.Web;
using System.IO;
using Ebiz.Scaffolding.Utils;
using Ebiz.Tids.CV.Utils;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Tids.CV.Models;

namespace Ebiz.WebForm.custom.Production
{
    public partial class ProductionDashboard : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        private const string urlImgDefaultCar = "~/content/images/production/car/default.png";
        private const string urlImgDefaultEmpty = "~/content/images/production/wait.png";
        private const string urlImgNotifGreen = "~/content/images/production/notification/green.gif";
        private const string urlImgNotifRed = "~/content/images/production/notification/red.gif";

        private List<DataViewItem> dvStations = new List<DataViewItem>();

        protected override bool OnInit(object sender, EventArgs e)
        {
            int assemblyTypeId = Request.QueryString["AssemblyTypeId"].ToInt32(0);
            if (assemblyTypeId == 0)
                assemblyTypeId = PermissionHelper.GetCurrentAssemblyTypeId();

            if (!IsPostBack)
            {
                InitDisplay(assemblyTypeId);
            }

            return true;
        }

        public void DrawMainLine(AppDb ctx, ProductionLine line, AssemblySectionType sectionType, bool showNewButton = false)
        {
            //Generate new div header in div master
            HtmlGenericControl divLineHeader = new HtmlGenericControl("div");
            divLineHeader.Attributes.Add("class", "div-header");

            //create button New for the Line
            if (showNewButton)
            {
                ASPxButton btnNew = new ASPxButton();
                btnNew.ID = line.Id.ToString();
                btnNew.Text = "New";
                btnNew.AutoPostBack = false;
                btnNew.CssClass = "btn-new-line";
                var command = "function (s, e) {window.location = '../Production/VINNumberChecker.aspx?LineId=" + line.Id + "'}";
                btnNew.ClientSideEvents.Click = command;
                divLineHeader.Controls.Add(btnNew);
            }

            ASPxLabel lblMainLine = new ASPxLabel();
            if (sectionType.IsMultiLine)
                lblMainLine.Text = line.LineName + " " + sectionType.Name;
            else
                lblMainLine.Text = sectionType.Name;
            lblMainLine.CssClass = "label-line";

            divLineHeader.Controls.Add(lblMainLine);
            divMasterDataView.Controls.Add(divLineHeader);

            //Line content
            HtmlGenericControl divLineContent = new HtmlGenericControl("div");
            divLineContent.Attributes.Add("class", "div-section");

            divMasterDataView.Controls.Add(divLineContent);

            IQueryable<AssemblySection> mainsections = ctx.AssemblySections.Where(x => x.AssemblySectionTypeId == sectionType.Id).OrderBy(x => x.AreaNo);
            foreach (AssemblySection sect in mainsections)
            {
                IQueryable<Station> stations = ctx.Stations.Where(x => x.AssemblySectionId == sect.Id);
                if (stations.Count() == 0)
                    continue;

                //Assembly section label
                ASPxLabel lblSection = new ASPxLabel();
                lblSection.Text = sect.AssemblySectionName;
                lblSection.CssClass = "label-section";

                divLineContent.Controls.Add(lblSection);

                //Generate new dataView in Dashboard
                ASPxDataView dvMaster = new ASPxDataView();

                dvMaster.ID = "dv" + line.LineName + sect.AssemblySectionName;
                dvMaster.ItemTemplate = new StationDataViewTemplate();
                dvMaster.CssClass = "dataview-master";

                //dvMaster.ClientSideEvents.Init = "function(s, e) { setTimeout(function() { " + cbpDashboard.ClientInstanceName + ".PerformCallback('refresh; " + sect.Id + "'); }, " + AppConfiguration.ANDON_POLL_INTERVAL_MSEC + ") }; ";

                divLineContent.Controls.Add(dvMaster);

                //Station list
                foreach (Station sta in stations)
                {
                    DataViewItem dvItem = new DataViewItem();

                    StationInfo info = new StationInfo();
                    info.StationId = sta.Id;
                    info.StationName = sta.StationName;
                    info.SectionId = sect.Id;
                    info.LineId = line.Id;

                    info.SectionTypeId = sectionType.Id;
                    info.IsQGate = sta.IsQualityGate;

                    info.Capacity = sta.Capacity;

                    info.Items = ProductionRepository.GetProductionItems(info.LineId, info.StationId);

                    dvItem.DataItem = info;

                    dvMaster.Items.Add(dvItem);

                    //add the collections of dataviewitems for easy updates
                    dvStations.Add(dvItem);
                }

                dvMaster.SettingsTableLayout.ColumnCount = (stations.Count() <= 10 ? stations.Count() : 10);

            }

        }

        public void DrawSubAssyLine(AppDb ctx, AssemblySectionType sectionType)
        {
            //Generate new div header in div master
            HtmlGenericControl divSubAssyHeader = new HtmlGenericControl("div");
            divSubAssyHeader.Attributes.Add("class", "div-header");

            ASPxLabel lblSubAssy = new ASPxLabel();
            lblSubAssy.Text = "Sub Assembly";
            lblSubAssy.CssClass = "label-line";

            divSubAssyHeader.Controls.Add(lblSubAssy);
            divMasterDataView.Controls.Add(divSubAssyHeader);

            //SubAssy content
            HtmlGenericControl divSubAssyContent = new HtmlGenericControl("div");
            divSubAssyContent.Attributes.Add("class", "div-section");

            divMasterDataView.Controls.Add(divSubAssyContent);

            IQueryable<AssemblySection> subsections = ctx.AssemblySections.Where(x => x.AssemblySectionTypeId == sectionType.Id).OrderBy(x => x.AreaNo);
            foreach (AssemblySection sect in subsections)
            {
                IQueryable<Station> stations = ctx.Stations.Where(x => x.AssemblySectionId == sect.Id);
                if (stations.Count() == 0)
                    continue;

                //Assembly section label
                ASPxLabel lblSection = new ASPxLabel();
                lblSection.Text = sect.AssemblySectionName;
                lblSection.CssClass = "label-section";

                divSubAssyContent.Controls.Add(lblSection);

                //Generate new dataView in Dashboard
                ASPxDataView dvMaster = new ASPxDataView();

                dvMaster.ID = "dvSubAssy" + sect.AssemblySectionName;
                dvMaster.ItemTemplate = new StationDataViewTemplate();
                dvMaster.CssClass = "dataview-master";

                //dvMaster.ClientSideEvents.Init = "function(s, e) { setTimeout(function() { " + cbpDashboard.ClientInstanceName + ".PerformCallback('refresh; " + sect.Id + "'); }, " + AppConfiguration.ANDON_POLL_INTERVAL_MSEC + ") }; ";

                divSubAssyContent.Controls.Add(dvMaster);

                //Station list
                foreach (Station sta in stations)
                {
                    DataViewItem dvItem = new DataViewItem();

                    StationInfo info = new StationInfo();
                    info.StationId = sta.Id;
                    info.StationName = sta.StationName;
                    info.SectionId = sect.Id;
                    info.LineId = AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER;

                    info.SectionTypeId = sectionType.Id;
                    info.IsQGate = sta.IsQualityGate;

                    info.Capacity = sta.Capacity;

                    info.Items = ProductionRepository.GetProductionItems(info.LineId, info.StationId);

                    dvItem.DataItem = info;

                    dvMaster.Items.Add(dvItem);

                    //add the collections of dataviewitems for easy updates
                    dvStations.Add(dvItem);
                }

                dvMaster.SettingsTableLayout.ColumnCount = (stations.Count() <= 10 ? stations.Count() : 10);
            }
        }

        public void InitDisplay(int assemblyTypeId)
        {
            //clear to append new
            divMasterDataView.Controls.Clear();

            using (AppDb ctx = new AppDb())
            {
                IQueryable<AssemblySectionType> types = ctx.AssemblySectionTypes.Where(x => x.ShowInDashboard).OrderBy(x => x.DashboardOrderNo);
                IQueryable<ProductionLine> lines = ctx.ProductionLines.Where(l => l.AssemblyTypeId == assemblyTypeId && l.LineNumber < AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER);

                for(int i=0; i<types.Count(); i++)
                {
                    AssemblySectionType sectionType = types.ElementAt(i);

                    int count = ctx.vStations.Where(x => x.AssemblySectionTypeId == sectionType.Id).Count();
                    if (count == 0)
                        continue;

                    if (sectionType.IsSubAssy)
                    {
                        //subassy
                        DrawSubAssyLine(ctx, sectionType);
                    }
                    else
                    {
                        //Main line
                        foreach (ProductionLine line in lines)
                        {
                            DrawMainLine(ctx, line, sectionType, (i == 0));

                            if (!sectionType.IsMultiLine)
                                break;
                        }
                    }
                }
            }

            //Get data from Repository
            var dashboard = ProductionRepository.GetProductionDashboard(assemblyTypeId);

            //Set UI from Repository
            lblOverall.Text = dashboard.Overall + " %";
            lblDailyTarget.Text = dashboard.TargetAchieve + " / " + dashboard.TargetDPR;
            lblMonthlyTarget.Text = dashboard.MonthlyAchieve + " / " + dashboard.MonthlyDPR;
            btnOffline.Text = dashboard.OfflineCount.ToString();
            btnRectification.Text = dashboard.RectiCount.ToString();
            btnVoca.Text = dashboard.VocaCount.ToString();
            btnPDI0.Text = dashboard.PDI0Count.ToString();

            if (AppConfiguration.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
            {
                lblVoca.Text = "Tear Down Audit";
                btnPA.Visible = false;
                lblPa.Text = "";
                lblPa.Visible = false;
            }
            else
            {
                lblVoca.Text = "VocA";
                btnPA.Text = dashboard.PACount.ToString();
                lblPa.Text = "PA";
            }

        }


        ////Load dashboard
        //public void loadInit(int assemblyTypeId)
        //{
        //    //Get data from Repository
        //    var dashboard = ProductionRepository.GetProductionDashboard(assemblyTypeId);

        //    //Set UI from Repository
        //    lblOverall.Text = dashboard.Overall + " %";
        //    lblDailyTarget.Text = dashboard.TargetAchieve + " / " + dashboard.TargetDPR;
        //    lblMonthlyTarget.Text = dashboard.MonthlyAchieve + " / " + dashboard.MonthlyDPR;
        //    btnOffline.Text = dashboard.OfflineCount.ToString();
        //    btnRectification.Text = dashboard.RectiCount.ToString();
        //    btnVoca.Text = dashboard.VocaCount.ToString();
        //    btnPDI0.Text = dashboard.PDI0Count.ToString();

        //    if (AppConfiguration.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
        //    {
        //        lblVoca.Text = "Tear Down Audit";
        //        btnPA.Visible = false;
        //        lblPa.Text = "";
        //        lblPa.Visible = false;
        //    }
        //    else
        //    {
        //        lblVoca.Text = "VocA";
        //        btnPA.Text = dashboard.PACount.ToString();
        //        lblPa.Text = "PA";
        //    }

        //    //clear to append new
        //    divMasterDataView.Controls.Clear();

        //    //Loop count data line in Repository
        //    if (dashboard == null || dashboard.ProductionLineItems == null)
        //        return;

        //    foreach (var pl in dashboard.ProductionLineItems)
        //    {
        //        //Generate new div header in div master
        //        HtmlGenericControl divHeader = new HtmlGenericControl("div");
        //        divHeader.Attributes.Add("class", "div-header");

        //        //if MainLine, create button New for the Line
        //        if (pl.Id <= AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER)
        //        {
        //            ASPxButton btnNew = new ASPxButton();
        //            btnNew.ID = pl.Id.ToString();
        //            btnNew.Text = "New";
        //            btnNew.AutoPostBack = false;
        //            btnNew.CssClass = "btn-new-line";
        //            var command = "function (s, e) {window.location = '../Production/VINNumberChecker.aspx?LineId=" + pl.Id + "'}";
        //            btnNew.ClientSideEvents.Click = command;
        //            divHeader.Controls.Add(btnNew);
        //        }

        //        ASPxLabel lblLine = new ASPxLabel();
        //        lblLine.Text = pl.LineName;
        //        lblLine.CssClass = "label-line";

        //        divHeader.Controls.Add(lblLine);
        //        divMasterDataView.Controls.Add(divHeader);

        //        //Generate new div trimMech in div master
        //        HtmlGenericControl divTrimMech = new HtmlGenericControl("div");
        //        divTrimMech.Attributes.Add("class", "div-section");

        //        //Loop count data Assembly Section in Repository
        //        foreach (var sc in pl.Sections)
        //        {
        //            ASPxLabel lblTrimMech = new ASPxLabel();
        //            lblTrimMech.Text = sc.SectionName;
        //            lblTrimMech.CssClass = "label-section";

        //            //Generate new dataView in Dashboard
        //            ASPxDataView dvMaster = new ASPxDataView();
        //            dvMaster.ID = "dv" + pl.LineName + sc.SectionName;

        //            AddDataItems(dvMaster, pl.Id, sc.SectionId);

        //            dvMaster.ItemTemplate = new StationDataViewTemplate();
        //            dvMaster.CssClass = "dataview-master";

        //            dvMaster.ClientSideEvents.Init = "function(s, e) { setTimeout(function() { " + cbpDashboard.ClientInstanceName + ".PerformCallback('refresh; " + dvMaster.ID + "'); }, " + AppConfiguration.ANDON_POLL_INTERVAL_MSEC + ") }; ";

        //            //Generate new div SubAssy in div TrimMech
        //            HtmlGenericControl divSubAssy = new HtmlGenericControl("div");

        //            //if SubAssy, create button Count SubAssy
        //            if (pl.Id == AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER)
        //            {
        //                divTrimMech.Attributes.Add("class", "div-section sa");
        //                divSubAssy.Attributes.Add("class", "div-SubAssy");
        //            }
        //            divSubAssy.Controls.Add(lblTrimMech);
        //            divSubAssy.Controls.Add(dvMaster);
        //            divTrimMech.Controls.Add(divSubAssy);
        //        }

        //        divMasterDataView.Controls.Add(divTrimMech);
        //    }
        //}

        //Create dataView Template inherits ITemplate
        public class StationDataViewTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToInt32(0);

                var dvMasterContainer = container as DataViewItemTemplateContainer;
                StationInfo info = dvMasterContainer.DataItem as StationInfo;
                if (info == null)
                    return;

                var firstStationId = ProductionRepository.GetFirstStation(assemblyTypeId);

                ////set parameter
                //int _historyId = GetData(dataItem, "HistoryId").ToInt32(0);
                //string _model = GetData(dataItem, "ModelName").ToString(string.Empty);
                //string _variant = GetData(dataItem, "VariantName").ToString(string.Empty);
                //string _finNo = GetData(dataItem, "FINNumber").ToString(string.Empty);
                //string _statusId = GetData(dataItem, "StatusId").ToString(string.Empty);
                //string _stationName = GetData(dataItem, "StationName").ToString(string.Empty);
                //bool _isQGate = GetData(dataItem, "IsQGate") == "True" ? true : false;
                //string _stationId = GetData(dataItem, "StationId").ToString(string.Empty);
                //bool _isSubAssy = GetData(dataItem, "IsSubAssy") == "True" ? true : false;
                //bool _IsEndOfLine = GetData(dataItem, "IsEndOfLine") == "True" ? true : false;
                //string _sectionId = GetData(dataItem, "SectionId").ToString(string.Empty);
                //string _numSubAssy = GetData(dataItem, "NumSubAssy").ToString(string.Empty);
                //bool _isfinishLine = GetData(dataItem, "IsFinishLine") == "True" ? true : false;
                //bool isMechLine = GetData(dataItem, "IsMechLine") == "True" ? true : false;
                //bool isList = GetData(dataItem, "IsList") == "True" ? true : false;

                //string UrlImageCar = ProductionHistoryHelper.GetImageCar(_historyId);
                ////MemoryStream ms = new MemoryStream(ImageCar);
                ////System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                ////paramter get problem
                //int LineId = GetData(dataItem, "LineId").ToInt32(0);
                //int stationId = GetData(dataItem, "StationId").ToInt32(0);
                //if (firstStationId == stationId)
                //{
                //    var countCarHOP = ProductionRepository.GetVehicleCountInStation(stationId, LineId);
                //    _stationName = _stationName + '(' + countCarHOP + ')';
                //}

                //Generate new divTemplateDataView in Template dataView
                HtmlGenericControl div = new HtmlGenericControl("div");

                //Generate new divHeader in divTemplateDataView
                HtmlGenericControl divHeader = new HtmlGenericControl("div");
                divHeader.Attributes.Add("class", "div-station-name");

                ASPxLabel lblStationName = new ASPxLabel();
                lblStationName.CssClass = "lblStationName_" + info.LineId + "_" + info.StationId;
                divHeader.Controls.Add(lblStationName);

                //Generate new divBody in divTemplateDataView
                HtmlGenericControl divBody = new HtmlGenericControl("div");
                divBody.Attributes.Add("class", "div-body");

                ASPxLabel lblModel = new ASPxLabel();
                lblModel.ID = "lblModel_" + info.LineId + "_" + info.StationId;
                lblModel.Text = "";
                divBody.Controls.Add(lblModel);

                //Generate imageCondition                
                ImageButton imgCondition = new ImageButton();
                imgCondition.ID = "imgCondition_" + info.LineId + "_" + info.StationId;
                imgCondition.Enabled = false;
                imgCondition.CssClass = "img-notification";
                divBody.Controls.Add(imgCondition);

                //Generate new divImage in divTemplateDataView
                HtmlGenericControl divImage = new HtmlGenericControl("div");

                ASPxButton btnDetail = new ASPxButton();
                btnDetail.ID = "btnDetail_" + info.LineId + "_" + info.StationId; ;
                btnDetail.AutoPostBack = false;
                btnDetail.CssClass = "btn-details";
                divImage.Controls.Add(btnDetail);

                //Generate new divFooter in divTemplateDataView
                HtmlGenericControl divFooter = new HtmlGenericControl("div");
                divFooter.Attributes.Add("class", "div-footer");

                ASPxLabel lblFINNumber = new ASPxLabel();
                lblFINNumber.ID = "lblFINNumber_" + info.LineId + "_" + info.StationId;
                lblFINNumber.Text = "";
                divFooter.Controls.Add(lblFINNumber);

                div.Controls.Add(divHeader);
                div.Controls.Add(divBody);
                div.Controls.Add(divImage);
                div.Controls.Add(divFooter);

                container.Controls.Add(div);

                if (info.Capacity <= 1 || info.Items == null || info.Items.Count == 0)
                    lblStationName.Text = info.StationName;
                else
                    lblStationName.Text = String.Format("{0} ({1})", lblStationName, info.Items.Count);

                if (info.Capacity > 1)
                {
                    btnDetail.Text = info.Items.Count.ToString();

                    lblModel.Visible = false;
                    imgCondition.Visible = false;
                    lblFINNumber.Visible = false;
                }
                else
                {
                    if (info.Items.Count == 0)
                    {
                        lblModel.Text = "";
                        imgCondition.Visible = false;
                        lblFINNumber.Text = "";

                        btnDetail.ClientEnabled = false;
                        btnDetail.ImageUrl = urlImgDefaultEmpty;

                    }
                    else
                    {
                        lblModel.Text = info.Items[0].ModelName;

                        imgCondition.ImageUrl =
                            FaultRepository.hasFault(info.Items[0].Id) ||
                            (ProductionRepository.GetAndonProductionProblem(info.LineId, DateTime.Now, "", info.StationId).Count() > 0)
                            ? urlImgNotifRed : urlImgNotifGreen;

                        if (info.Items[0].SerialNumber.Length < 17)//Engine Data
                        {
                            // Permintaaan 8 Character
                            lblFINNumber.Text = string.IsNullOrWhiteSpace(info.Items[0].SerialNumber) ? string.Empty : info.Items[0].SerialNumber.Length < 14 ? string.Empty : info.Items[0].SerialNumber.Substring(6, 8);
                        }
                        else
                        {
                            lblFINNumber.Text = string.IsNullOrWhiteSpace(info.Items[0].SerialNumber) ? string.Empty : info.Items[0].SerialNumber.Length < 17 ? string.Empty : info.Items[0].SerialNumber.Substring(11, 6);
                        }

                        string UrlImageCar = ProductionHistoryHelper.GetImageCar(info.Items[0].Id);
                        if (UrlImageCar == "" || UrlImageCar == null)
                        {
                            btnDetail.ImageUrl = urlImgDefaultCar;
                        }
                        else
                        {
                            btnDetail.ImageUrl = UrlImageCar;
                        }

                    }
                }

                //if Subassy DataView
                if (info.Capacity > 1)
                {
                    btnDetail.ClientSideEvents.Click = "function (s, e) {window.location = '../Production/ProductionStationList.aspx?Stat=" + info.StationId + "'}";
                }
                else
                {
                    btnDetail.ClientSideEvents.Click = "function (s, e) {window.location = '../Production/ProductionDashboardDetail.aspx?Type=" + (info.IsQGate ? "1" : "0")
                   + "&Hist=" + 0 + "&Stat=" + info.StationId + "&Sect=0'}";
                }
            }
        }

        //Adding data to dataView
        private void AddDataItems(ASPxDataView dvMaster, int lineId, int sectionId)
        {
            AssemblySection section = ProductionRepository.GetAssemblySection(sectionId);

            List<Station> stations = new List<Station>();

            stations = ProductionRepository.GetStationsInAssemblySection(sectionId);

            if (stations.Any())
            {
                foreach (Station sta in stations)
                {
                    DataViewItem dvItem = new DataViewItem();

                    StationInfo info = new StationInfo();
                    info.StationId = sta.Id;
                    info.StationName = sta.StationName;
                    info.SectionId = sectionId;
                    info.LineId = lineId;

                    info.SectionTypeId = section.AssemblySectionTypeId;
                    info.IsQGate = sta.IsQualityGate;

                    info.Capacity = sta.Capacity;

                    info.Items = ProductionRepository.GetProductionItems(info.LineId, info.StationId);

                    dvItem.DataItem = info;

                    dvMaster.Items.Add(dvItem);

                }

                dvMaster.SettingsTableLayout.ColumnCount = stations.Count;
            }
        }

        //Get data by dataBinder
        public static string GetData(object dataObject, string fieldName)
        {
            var result = string.Empty;
            var temp = DataBinder.Eval(dataObject, fieldName);
            if (temp != null) result = temp.ToString();

            return result;
        }

        //Redirect action btnOffline
        protected void btnOffline_Click(object sender, EventArgs e)
        {
            ASPxButton btnOffline = (ASPxButton)sender;

            try
            {
                Response.Redirect("../Production/OfflineRectification.aspx?Condition=" + btnOffline.CommandArgument);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        //Redirect action btnRectification
        protected void btnRectification_Click(object sender, EventArgs e)
        {
            ASPxButton btnRecti = (ASPxButton)sender;

            try
            {
                Response.Redirect("../Production/OfflineRectification.aspx?Condition=" + btnRecti.CommandArgument);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnVoca_Click(object sender, EventArgs e)
        {
            ASPxButton btnVoca = (ASPxButton)sender;

            try
            {
                Response.Redirect("../Production/OfflineRectification.aspx?Condition=" + btnVoca.CommandArgument);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnPA_Click(object sender, EventArgs e)
        {
            ASPxButton btnVoca = (ASPxButton)sender;

            try
            {
                Response.Redirect("../Production/OfflineRectification.aspx?Condition=" + btnPA.CommandArgument);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnPDI0_Click(object sender, EventArgs e)
        {
            ASPxButton btnPDI0 = (ASPxButton)sender;

            try
            {
                Response.Redirect("../Production/ProductionPDI0List.aspx?Condition=" + btnPDI0.CommandArgument);
               
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void cbpDashboard_Callback(object sender, CallbackEventArgsBase e)
        {
            if (string.IsNullOrWhiteSpace(e.Parameter))
            {
                InitDisplay(PermissionHelper.GetCurrentAssemblyTypeId());
                //UpdateDisplay();
            }
        }

        protected void UpdateDisplay()
        {
            List<ProductionItem> items = ProductionRepository.GetAllProductionItems(PermissionHelper.GetCurrentAssemblyTypeId());

            //update all dvStations
            foreach (DataViewItem dvi in dvStations)
            {
                StationInfo info = dvi.DataItem as StationInfo;

                IEnumerable<ProductionItem> pi = items.Where(x => x.LineId == info.LineId && x.StationId == info.StationId);
                if (pi.Count() == 0)
                {
                    //no vehicle in the station > clear the station
                }
                else
                {
                    //there are vehicle in the station
                    if (info.Capacity == 1)
                    {
                        //show the vehicle info
                    }
                    else
                    {
                        //show the number of vehicle in station
                    }
                }
            }
        }
    }

}