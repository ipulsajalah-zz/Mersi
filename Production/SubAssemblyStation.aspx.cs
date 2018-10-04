using DevExpress.Web;
using DotWeb.Repositories;
using DotWeb.Utils;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{
    public partial class SubAssemblyStation : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        private static int _sectionId;
        private static int _historyId;

        private static int _lineId;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString.Count > 0)
            {
                _sectionId = Convert.ToInt32(Request.QueryString["SectId"]);
                if (_sectionId > 0)
                {
                    var station = ProductionRepository.GetAssemblySections(assemblyTypeId).Where(x => x.Id == _sectionId)
                        .Select(x => x.AssemblySectionName).SingleOrDefault();
                    lblStation.Text = station.ToString();

                    var subAssy = ProductionRepository.GetProductionAssemblyBySection(_sectionId, assemblyTypeId);
                    gridSubAssembly.DataSource = subAssy;
                    gridSubAssembly.DataBind();
                }
            }
            else
            {
                Response.Redirect("../Production/ProductionDashboard.aspx");
            }

            _lineId = ProductionRepository.GetProductionLineId(AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER, assemblyTypeId);
        }

        protected void btnVehicle_Click(object sender, EventArgs e)
        {
            ASPxButton btn = (sender as ASPxButton);
            GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
            int stationId = (int)container.Grid.GetRowValues(container.VisibleIndex, new string[] { "StationId" });
            string status = (string)container.Grid.GetRowValues(container.VisibleIndex, new string[] { "Status" });
            bool IsInStock = (bool)container.Grid.GetRowValues(container.VisibleIndex, new string[] { "IsInStock" });
            if (status == enumProductionStatus.WorkInProgress.ToString() || IsInStock == true)
            {
                Response.Redirect("../Production/ProductionDashboardDetail.aspx?Type=" + 0 + "&Hist=" + btn.CommandArgument + "&Stat=" + stationId + "&Sect=" + _sectionId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Material Not In Stock');", true);
            }
        }

        protected void btnAddSubAssy_Click(object sender, EventArgs e)
        {
            var _finNumber = tbAddSubAssy.Text;
            var redirectWithError = string.Empty;

            //TODO: check for authkey

            //is exist in main line
            if (string.IsNullOrWhiteSpace(_finNumber))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Serial number cannot be empty !');", true);
                return;
            }
            var CheckFinNumber = ProductionRepository.GetFinNumber(_finNumber, assemblyTypeId);
            if (string.IsNullOrWhiteSpace(ProductionRepository.CheckSerialNumberInSequence(CheckFinNumber)))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Serial number " + _finNumber + " not in Production !');", true);
                return;
            }

            //is exist in material assy production 
            if (ProductionRepository.IsExistProductionMaterialHistory(CheckFinNumber, _sectionId, assemblyTypeId))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Sub Assembly for " + _finNumber + " has already exist !');", true);
                return;
            }

            //NOTE: No need to check for ProductionRoute. It is already checked in VINNumberChecker
            //var notif = ProductionRepository.IsExistProductionRoute(_finNumber, assemblyTypeId);
            //if (notif == "exist")
            //{
            //TODO: confirm LineId for SubAssy is 20 (or is it 0?)
            int histId = 0;

            var prod = ProductionRepository.GetProductionHistoryInMainLines(CheckFinNumber, assemblyTypeId);
            if (prod != null) histId = prod.Id;

            if (histId == 0)
            {
                //not exist in main lines, create one
                //Note: if it already exist in SubAssy, it will replace the line to represent the current line
                histId = ProductionRepository.InsertProductionHistory(_lineId, assemblyTypeId, CheckFinNumber, "");
            }

            if (histId == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Failed add " + CheckFinNumber + " to this Sub Assy, Production Sequence with Serial Number " + CheckFinNumber + " is not found. !');", true);
                return;
            }

            //add to productionmaterial assembly
            var startStationId = ProductionRepository.GetFirstStationInAssemblySection(_sectionId);
            int prodMatHist = ProductionRepository.InsertProductionMaterialHistory(CheckFinNumber, startStationId, assemblyTypeId);
            if (prodMatHist > 0)
            {
                //add to details
                if (0 != ProductionRepository.InsertProductionHistoryDetail(histId, startStationId, false, false, ""))
                {
                    //set historyId and _stationid
                    _historyId = histId;
                    tbAddSubAssy.Text = string.Empty;
                    pcAddSubAssy.ShowOnPageLoad = false;
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('" + CheckFinNumber + " add successfully to this Sub Assy !');window.location ='../Production/ProductionDashboardDetail.aspx?Type=" + 0 + "&Hist=" + histId + "&Stat=" + startStationId + "&Sect=" + _sectionId + "'", true);
                }
                else
                {
                    //failed to save detail?then delete material from gravity
                    ProductionRepository.DeleteProductionHistoryMaterial(CheckFinNumber, startStationId);
                    redirectWithError = "Failed add " + CheckFinNumber + " to this Sub Assy !";
                }
            }
            else
                //failed to add new material
                redirectWithError = "Failed add " + CheckFinNumber + " to this Sub Assy !";
            //}
            //}

            if (!string.IsNullOrWhiteSpace(redirectWithError))
            {
                pcAddSubAssy.ShowOnPageLoad = false;
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('" + redirectWithError + "')", true);
            }
        }

        protected void pcAddSubAssy_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            gridSubAssembly.DataBind();
        }
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Production/ProductionDashboard.aspx");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                string ValueRowId = gridSubAssembly.GetRowValues(Rowindex, "ChassisNumber").ToString();
                int StationId = int.Parse(gridSubAssembly.GetRowValues(Rowindex, "StationId").ToString());
                ProductionRepository.DeleteSubAssembly(ValueRowId, StationId);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }
        }
    }
}