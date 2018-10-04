using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotWeb.Repositories;
using DotWeb.Utils;
using DevExpress.Web;
using DotWeb.Models;
using Ebiz.Scaffolding.Utils;

namespace DotMercy.custom.Production
{
   
    public partial class ProductionEndOffLineList : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        public static string _condition;
        private static int _stationId;
        private static int _historyId;
        private static int _requestType;
        private static int _sectId;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
                _condition = Convert.ToString(Request.QueryString["Condition"]);
            else
                Response.Redirect("../Production/ProductionDashboard.aspx");

            LoadData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                _requestType = Convert.ToInt32(Request.QueryString["Type"]);
                _stationId = Convert.ToInt32(Request.QueryString["Stat"]);
                _historyId = Convert.ToInt32(Request.QueryString["Hist"]);
                _sectId = Convert.ToInt32(Request.QueryString["Sect"]);
 
            }
            if (!IsPostBack)
                LoadData();
        }
        protected void LoadData()
        {
            var productionItems = new List<ProductionItem>();

         
                lblPosition.Text = "End Of Line";
                btnProcess.Text = "Release Vehicle";
                //btnRectification.Visible = true;
                _stationId = Convert.ToInt32(Request.QueryString["Stat"]);
                productionItems = ProductionRepository.GetProductionEndOfflIneByStation(_stationId);
          
            gridCondition.DataSource = productionItems;
            gridCondition.DataBind();
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            var items = gridCondition.GetSelectedFieldValues(new string[] { "ItemId", "StationId" });

            if (items.Any())
            {
                foreach (var item in items)
                {
                    //convert object to readable type
                    IEnumerable<object> prodItem = (IEnumerable<object>)item;
                    var _itemId = Convert.ToInt32(prodItem.FirstOrDefault().ToString());
                    var _stationId = Convert.ToInt32(prodItem.LastOrDefault().ToString());

                    //get history item
                    var histItem = ProductionRepository.GetProductionItemByHistoryStation(_stationId, _itemId);

                    if (_condition == "isVoca")
                    {
                        //check if this car still have fault to solve
                        if (FaultRepository.IsHaveFault(_itemId))
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('There are still unfinished fault, please solve this first.');", true);
                        else
                            if (ProductionRepository.UpdateProductionHistoryVOCA(_itemId, false))
                            {
                                // insert new inventory
                                if (ProductionRepository.InsertProductionInventory(_itemId, Session["assemblyType"].ToInt32(0)) > 0)
                                {
                                    //update this last item production history detail
                                    if (ProductionRepository.UpdateProductionHistoryDetail(_itemId, _stationId,
                                        histItem.IsOffline, histItem.IsRectification, histItem.IsOffline, histItem.IsRectification, true))
                                    {

                                        //update production history status to complete
                                        if (ProductionRepository.UpdateProductionHistoryComplete(_itemId))
                                        {
                                            //update all existing assembly for this part
                                            ProductionRepository.UpdateAllSubAssyBySerialNumber(histItem.FINNumber);
                                            //load data
                                            LoadData();
                                        }
                                        else
                                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Update Production History, please contact your administrator');", true);
                                    }
                                    else
                                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Update Production History Details, please contact your administrator');", true);
                                }
                                else
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Insert New Inventory, please contact your administrator');", true);
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Insert New Inventory, please contact your administrator');", true);
                    }
                    else
                    {
                        var newStationId = _condition == "isRecti" ? ProductionRepository.GetFirstEndOfLineStation(assemblyTypeId) : ProductionRepository.GetFirstStation(assemblyTypeId);

                        //insert new station detail to start eol
                        if (0 != ProductionRepository.InsertProductionHistoryDetail(histItem.ItemId, newStationId,
                            _condition == "isRecti" ? histItem.IsOffline : false, _condition == "isRecti" ? false : histItem.IsRectification,""))
                        {
                            //update last un done history detail
                            if (ProductionRepository.UpdateProductionHistoryDetail(histItem.ItemId, histItem.StationId,
                                histItem.IsOffline, histItem.IsRectification, histItem.IsOffline, true, true))
                            {

                                if (_condition == "isRecti")
                                    ProductionRepository.UpdateAllSubAssyBySerialNumber(histItem.FINNumber);
                                //todo success
                                LoadData();
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot update last history details.');", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot send "
                                + histItem.FINNumber + " to " + _condition == "isRecti" ? "End of Line" : "Trimming Line" + ".');", true);
                    }
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('No Rows Selected.');", true);
        }

        protected void btnRectification_Click(object sender, EventArgs e)
        {
            var items = gridCondition.GetSelectedFieldValues(new string[] { "ItemId", "StationId" });

            if (items.Any())
            {
                foreach (var item in items)
                {
                    //getcurrentStaion and history
                    //convert object to readable type
                    IEnumerable<object> prodItem = (IEnumerable<object>)item;
                    int currentHistoryId = Convert.ToInt32(prodItem.FirstOrDefault().ToString());
                    int currentStationId = Convert.ToInt32(prodItem.LastOrDefault().ToString());

                    try
                    {
                        //back to unvoca
                        if (ProductionRepository.UpdateProductionHistoryVOCA(currentHistoryId, false))
                        {
                            //current history
                            var currentProdItem = ProductionRepository.GetProductionItemByHistoryStation(currentStationId, currentHistoryId);

                            //getnextposition            
                            bool oldIsRectify = currentProdItem.IsRectification;
                            bool newIsRectify = true;

                            //update process in this station
                            if (ProductionRepository.UpdateProductionHistoryDetail(currentHistoryId, currentStationId, currentProdItem.IsOffline, oldIsRectify, currentProdItem.IsOffline, newIsRectify, false))
                            {
                                //todo success
                                LoadData();
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Update Current History');", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        //alert process has move to next station
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
                        AppLogger.LogError(ex.Message);
                    }
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('No Rows Selected.');", true);
        }

        private void AddDataItems(ASPxDataView dvMaster, List<ProductionItem> items, int sectionId)
        {
            List<DotWeb.Models.Station> stations = new List<DotWeb.Models.Station>();

            stations = ProductionRepository.GetStationsByAssemblySection(sectionId);

            if (stations.Any())
            {
                foreach (var station in stations)
                {
                    DataViewItem dvItem = new DataViewItem();

                    var historyId = string.Empty;
                    var model = string.Empty;
                    var variant = string.Empty;
                    var finNo = string.Empty;
                    var statusId = string.Empty;
                    bool isSubAssy = ProductionRepository.IsSubAssemblyStation(station.Id);
                    var subAssyCount = ProductionRepository.CountSubAssyPerSection(sectionId);

                    var prodItem = items.Where(x => x.StationId == station.Id).FirstOrDefault();
                    if (prodItem != null)
                    {
                        historyId = prodItem.ItemId.ToString();
                        model = prodItem.ModelName;
                        variant = prodItem.VariantName;
                        finNo = prodItem.FINNumber;
                        statusId = prodItem.StatusId.ToString();
                    }

                    dvItem.DataItem = new
                    {
                        HistoryId = historyId,
                        ModelName = model,
                        VariantName = variant,
                        FINNumber = finNo,
                        StatusId = statusId,
                        StationId = station.Id,
                        StationName = station.StationName,
                        IsQGate = station.IsQualityGate,

                        SectionId = sectionId,
                        IsSubAssy = isSubAssy,
                        NumSubAssy = subAssyCount
                    };

                    dvMaster.Items.Add(dvItem);
                }

                dvMaster.SettingsTableLayout.ColumnCount = stations.Count;
            }
            else
            {
                foreach (var item in items)
                {
                    DataViewItem dvItem = new DataViewItem();

                    dvItem.DataItem = new
                    {
                        HistoryId = item.ItemId,
                        ModelName = item.ModelName,
                        VariantName = item.VariantName,
                        FINNumber = item.FINNumber,
                        StatusId = item.StatusId,
                        StationId = item.StationId,
                        StationName = item.StationName,
                        IsQGate = false,

                        SectionId = sectionId,
                        IsSubAssy = false,
                        NumSubAssy = 0
                    };

                    dvMaster.Items.Add(dvItem);
                }

                dvMaster.SettingsTableLayout.ColumnCount = items.Count > 0 ? items.Count : 1;
            }
        }

        protected void Btn_ItemId_Click(object sender, EventArgs e)
        {
            _requestType = Convert.ToInt32(Request.QueryString["Type"]);
            _stationId = Convert.ToInt32(Request.QueryString["Stat"]);
            _historyId = Convert.ToInt32(Request.QueryString["Hist"]);
            _sectId = Convert.ToInt32(Request.QueryString["Sect"]);
            //var stations = ProductionRepository.GetStationsByAssemblySection(_sectId);
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            var StationId = gridCondition.GetRowValues(index, "StationId");
            var HisId = gridCondition.GetRowValues(index, "ItemId");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
            //      "window.open('../Production/ProductionDashboardDetail.aspx?Type=" + _requestType + "&Hist=" + HisId + "&Stat=" + StationId + "&Sect=0" + "','_newtab');",
            //      true);
            Response.Redirect("../Production/ProductionDashboardDetail.aspx?Type=" + _requestType + "&Hist=" + HisId + "&Stat=" + StationId + "&Sect=0");
        }

        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Production/ProductionDashboard.aspx");
        }
    }
}