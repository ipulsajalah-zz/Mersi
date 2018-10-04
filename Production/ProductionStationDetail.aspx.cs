using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Globalization;
using System.Drawing;
//using DevExpress.XtraGauges.Core.Drawing;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Generator;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;

namespace Ebiz.WebForm.custom.Production
{
    public partial class ProductionStationDetail : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        ////List<FaultAuditAttachmentModel> AttachFileModelList = new List<FaultAuditAttachmentModel>();
        //List<ProductionFaultReleaseAttachment> AttachFileModelList = new List<ProductionFaultReleaseAttachmentModel>();
        //ProductionReleaseFaultModel ListAttachment = new ProductionReleaseFaultModel();

        private const string urlImgDefaultCar = "~/content/images/production/car/default.png";
        private const string urlImgNotifGreen = "~/content/images/production/notification/green.gif";
        private const string urlImgNotifRed = "~/content/images/production/notification/red.gif";

        //private static int _stationId;
        //private static int _historyId;

        private static int _requestType;
        private static int _sectId;
        private static bool _isNotNull;
        private static bool _isSubAssy;
        private static bool _isMechLine;
        private static bool _isFirstStation;
        private static bool _isLastStation;
        private static int _lineId;

        private static string CPStationDetailTableName = "CPStationDetail";
        private static string VehicleFaultTableName = "VehicleFaults";
        private static string StationAlterationTableName = "StationAlterations";
        private static string VehicleSubAssyTableName = "VehicleSubAssys";

        private ASPxGridView gvCpStationDetail = null;
        private ASPxGridView gvVehicleFaults = null;
        private ASPxGridView gvStationAlterations = null;
        private ASPxGridView gvVehicleSubAssys = null;

        private TableMeta tableMeta = null;
        private Station station = null;
        private ProductionHistory prodHist = null;

        protected override bool OnInit(object sender, EventArgs e)
        {
            UploadAttachment.ValidationSettings.MaxFileSize = ConfigurationHelper.UPLOAD_MAXFILESIZE_BYTES;
            UploadAttachment.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + ConfigurationHelper.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            UploadAttachment.ValidationSettings.AllowedFileExtensions = ConfigurationHelper.UPLOAD_FILE_EXTENSIONS.Split(",");

            if (Request.QueryString.Count == 0)
                Response.Redirect("../Production/ProductionDashboard.aspx");

            int _stationId = Request.QueryString["Stat"].ToInt32(0);
            int _historyId = Request.QueryString["Hist"].ToInt32(0);

            station = ProductionRepository.GetProductionStation(_stationId);
            if (station == null)
            {
                masterPage.MainContent.Controls.Clear();
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Station")));
                return false;
            }

            prodHist = ProductionRepository.GetProductionHistory(_historyId);
            //if (prodHist == null)
            //{
            //    masterPage.MainContent.Controls.Clear();
            //    masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Production History")));
            //    return false;
            //}

            _requestType = station.IsQualityGate ? 1 : 0;
            _sectId = station.AssemblySectionId;
            _isNotNull = _stationId != 0 && _historyId != 0 ? true : false;

            _isSubAssy = ProductionRepository.IsSubAssemblyStation(_stationId);
            _isFirstStation = ProductionRepository.IsFirstStation(_stationId, assemblyTypeId);
            _isLastStation = ProductionRepository.IsLastStation(_stationId, assemblyTypeId);
            _isMechLine = false; // ProductionRepository.IsMechLineStation(_stationId);


            if (!IsPostBack)
            {
                loadDetail();
            }

            showCPStationDetail();
            showVehicleFault();
            showStationIA();
            showVehicleSubAssy();

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (gvCpStationDetail != null)
            {
                //masterGrid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                //masterGrid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                //masterGrid.Settings.VerticalScrollableHeight = 600;

                gvCpStationDetail.Templates.PreviewRow = new TemplatePreviewRow();

                for (int i = 0; i < gvCpStationDetail.Columns.Count; i++)
                {
                    GridViewDataColumn col = gvCpStationDetail.Columns[i] as GridViewDataColumn;
                    if (col == null) continue;

                    if (col.Name == "VDoc" || col.Name == "SecondHand" || col.Name == "DS" || col.Name == "CS2" || col.Name == "CS3"
                        || col.Name == "JCStamp" || col.Name == "JCBarcode")
                    {
                        GridViewDataCheckColumn checkCol = col as GridViewDataCheckColumn;
                        checkCol.PropertiesCheckEdit.DisplayTextChecked = "X";
                        checkCol.PropertiesCheckEdit.DisplayTextUnchecked = "";
                        checkCol.PropertiesCheckEdit.DisplayTextGrayed = "";
                        checkCol.PropertiesCheckEdit.UseDisplayImages = false;
                        checkCol.CellStyle.Font.Bold = true;
                        checkCol.CellStyle.Font.Size = new FontUnit(FontSize.Medium);
                        checkCol.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (col.Name == "FBS" || col.Name == "DRT")
                    {
                        col.DataItemTemplate = new TemplateCpStationDetailDataItem(this, col.Name);
                    }
                    else if (col.Name == "II" || col.Name == "WI")
                    {
                        col.DataItemTemplate = new TemplateCpStationDetailDataItem(this, col.Name);

                        col.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                        col.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        col.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                        col.FilterCellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                        col.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

                        //< Settings AllowGroup = "False" />
                        // < EditFormSettings Visible = "False" />
                        //  < FilterCellStyle Wrap = "True" >
                        //   </ FilterCellStyle >
                        //   < HeaderStyle Wrap = "True" ></ HeaderStyle >
                    }
                }

                gvCpStationDetail.DataBind();
            }

            return true;
        }

        private void showCPStationDetail()
        {
            //TabPage tabPage = new TabPage("Control Plan");
            //tabDetail.TabPages.Add(tabPage);

            TabPage tabPage = tabDetail.TabPages.FindByName("tabProcess");
            if (tabPage == null)
                return;

            tabPage.Controls.Clear();

            if (station.IsQualityGate)
            {
                tabPage.Visible = false;
                return;
            }

            //if no production item, dont bother
            if (prodHist == null)
                return;

            //get CP info
            int cpId = 0;
            using (AppDb ctx = new AppDb())
            {
                //get control plan
                ControlPlan cp = ctx.ControlPlans.Where(x => x.CatalogModelId == prodHist.CatalogModelId && x.PackingMonth == prodHist.PackingMonth).FirstOrDefault();
                if (cp == null)
                    return;

                if (cp.Status == EControlPlanStatus.Draft)
                {
                    LiteralControl control = new LiteralControl(string.Format("<h2>Control Plan is not released yet.</h2>"));
                    tabPage.Controls.Add(control);
                    return;
                }

                cpId = cp.Id;

                //Set master key
                SetMasterKey("ControlPlanId", cp.Id);

                //set key value
                keyValues.Add("CP", cp);
            }

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(CPStationDetailTableName, user.Id, assemblyTypeId);
            if (permissions.Count() == 0)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>No permission.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(CPStationDetailTableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>Invalid Page.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            //TODO: move to configuration
            //tableMeta.PreviewColumnName = "Torque";

            //if CP status is released, create grid
            string filter = String.Format("ControlPlanId={0} AND StationId={1}", cpId, station.Id);

            var gridCreator = new MasterGrid(this, tableMeta);
            gvCpStationDetail = gridCreator.Render(user.Id, assemblyTypeId, filter);

            //Important: Have to set the width this way because of a quirk/bug in aspxgridview
            gvCpStationDetail.ClientSideEvents.Init = "function(s, e) { s.SetWidth(" + tabDetail.ClientInstanceName + ".GetWidth() - 40); }"; //"OnDetailInit";
            
            tabPage.Controls.Add(gvCpStationDetail);
        }

        private void showVehicleFault()
        {
            TabPage tabPage = tabDetail.TabPages.FindByName("tabFaults");
            if (tabPage == null)
                return;

            tabPage.Controls.Clear();

            //if no production item, dont bother
            if (prodHist == null)
                return;

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(CPStationDetailTableName, user.Id, assemblyTypeId);
            if (permissions.Count() == 0)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>No permission.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(CPStationDetailTableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>Invalid Page.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            string filter = String.Format("ProductionHistoryId={0}", prodHist.Id);

            var gridCreator = new MasterGrid(this, tableMeta);
            gvVehicleFaults = gridCreator.Render(user.Id, assemblyTypeId, filter);

            //Important: Have to set the width this way because of a quirk/bug in aspxgridview
            gvVehicleFaults.ClientSideEvents.Init = "function(s, e) { s.SetWidth(" + tabDetail.ClientInstanceName + ".GetWidth() - 40); }"; //"OnDetailInit";

            tabPage.Controls.Add(gvVehicleFaults);
        }

        private void showStationIA()
        {
            TabPage tabPage = tabDetail.TabPages.FindByName("tabAlteration");
            if (tabPage == null)
                return;

            tabPage.Controls.Clear();

            if (station.IsQualityGate)
            {
                tabPage.Visible = false;
                return;
            }

            //if no production item, dont bother
            if (prodHist == null)
                return;

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(CPStationDetailTableName, user.Id, assemblyTypeId);
            if (permissions.Count() == 0)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>No permission.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(CPStationDetailTableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>Invalid Page.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            string filter = String.Format("ProductionHistoryId={0} and StationId={1}", prodHist.Id, station.Id);

            var gridCreator = new MasterGrid(this, tableMeta);
            gvStationAlterations = gridCreator.Render(user.Id, assemblyTypeId, filter);

            //Important: Have to set the width this way because of a quirk/bug in aspxgridview
            gvStationAlterations.ClientSideEvents.Init = "function(s, e) { s.SetWidth(" + tabDetail.ClientInstanceName + ".GetWidth() - 40); }"; //"OnDetailInit";

            tabPage.Controls.Add(gvStationAlterations);
        }

        private void showVehicleSubAssy()
        {
            TabPage tabPage = tabDetail.TabPages.FindByName("tabSubAssy");
            if (tabPage == null)
                return;

            tabPage.Controls.Clear();

            if (station.IsQualityGate)
            {
                tabPage.Visible = false;
                return;
            }

            //if no production item, dont bother
            if (prodHist == null)
                return;

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(CPStationDetailTableName, user.Id, assemblyTypeId);
            if (permissions.Count() == 0)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>No permission.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(CPStationDetailTableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                LiteralControl control = new LiteralControl(string.Format("<h2>Invalid Page.</h2>"));
                tabPage.Controls.Add(control);
                return;
            }

            string filter = String.Format("ProductionHistoryId={0} and StationId={1}", prodHist.Id, station.Id);

            var gridCreator = new MasterGrid(this, tableMeta);
            gvVehicleSubAssys = gridCreator.Render(user.Id, assemblyTypeId, filter);

            //Important: Have to set the width this way because of a quirk/bug in aspxgridview
            gvVehicleSubAssys.ClientSideEvents.Init = "function(s, e) { s.SetWidth(" + tabDetail.ClientInstanceName + ".GetWidth() - 40); }"; //"OnDetailInit";

            tabPage.Controls.Add(gvVehicleSubAssys);
        }

        //load Detail Header
        private void loadDetail()
        {
            if (_isNotNull == false)
                Response.Redirect("../Production/ProductionDashboard.aspx");

            //default setting
            detailCondition.Visible = false;

            if (station == null || prodHist == null || prodHist.ProductionSequenceDetail == null)
                return;

            //ProductionSequenceDetail prodSeqDetail = ProductionRepository.GetProductionSequenceDetail(prodHist.SerialNumber);
            //if (prodSeqDetail == null)
            //    return;

            //StationCondition stationConditions = ProductionRepository.GetStationCondition(station.Id, prodHist.Id);

            //car image
            string UrlImageCar = ProductionHistoryHelper.GetImageCar(prodHist.Id);
            if (UrlImageCar == "" || UrlImageCar == null)
            {
                detailImg.ImageUrl = urlImgDefaultCar;
            }
            else
            {
                detailImg.ImageUrl = UrlImageCar;
            }
            detailImg.Enabled = false;
            detailImg.CssClass = "img-detail";

            //button offline
            if (station.AllowOffline == true)
                btnMoveOffline.Enabled = true;
            else
                btnMoveOffline.Enabled = false;

            //button online
            if (station.AllowRectification == true)
                btnMoveRecti.Enabled = true;
            else
                btnMoveRecti.Enabled = false;

            //notification
            detailCondition.Enabled = false;
            detailCondition.CssClass = "img-notification";
            detailCondition.ImageUrl =
                    FaultRepository.hasFault(prodHist.Id) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", station.Id).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;

            //station info
            lblStation.Text = station.StationName + (_isSubAssy ? string.Empty : " [" + prodHist.ProductionLine.LineNumber + "]");

            //vehicle info
            lblVINNumber.Text = prodHist.VINNumber;
            lblFINNumber.Text = prodHist.SerialNumber;
            lblModelVariant.Text = prodHist.CatalogModel.Model;
            lblPackingMonth.Text = prodHist.PackingMonth;

            //detail vehicle info
            lblEngineNo.Text = prodHist.ProductionSequenceDetail.EngineNo;
            lblCommnos.Text = prodHist.ProductionSequenceDetail.Commnos;

            //TODO
            //lblFDoc.Text = stationConditions.FDokProdNo;
            //moOptionCode.Text = stationConditions.OptionCodes;
            //lblPaintCode.Text = stationConditions.PaintCode == 0 ? "" : stationConditions.PaintCode.ToString();
            //lblInteriorCode.Text = stationConditions.InteriorCode == 0 ? "" : stationConditions.InteriorCode.ToString();
            //lblLocalProdNo.Text = stationConditions.LocalProdNo;

            // if station is sub assy station
            if (_isSubAssy)
            {
                btnNextProcess.Text = "Set Sub Assembly Done";
                //btnMoveOffline.Enabled = false;
                //btnMoveRecti.Enabled = false;
                btnProblem.ClientVisible = false;
                btnProblem.Visible = false;
                //btnMoveRecti.Enabled = false;

                //if it is completed button next process disable
                if (ProductionRepository.IsAssemblyCompleted(prodHist.Id, station.Id))
                    btnNextProcess.Enabled = false;
            }

            if (ProductionRepository.IsEndOfLineStation(station.Id))
            {
                //btnMoveRecti.Enabled = true;
            }

            //if station is member of Last Station
            if (_isLastStation)
            {
                btnProblem.ClientVisible = false;
                btnProblem.Visible = false;
                //btnMoveRecti.Enabled = true;
                //if station is last station of end of line
                //TODO: confirm what it really wants to do (double check of IsLastStation())

                btnNextProcess.Visible = false;
                BtnMovePA.Visible = true;
                BtnMoveVOCA.Visible = true;
                //btnMoveOffline.Visible = true;
                btnMoveOffline.Text = "Send to VOCA";
                if (ConfigurationHelper.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
                {
                    btnMoveOffline.Text = "Send to TearDown";
                    BtnMovePA.Visible = false;
                    BtnMoveVOCA.Visible = false;
                }
                else
                {
                    btnMoveOffline.Text = "Send to Offline";
                }
            }
            else
            {
                //btnMoveOffline.Enabled = false;
                btnRelease.Enabled = false;
                if (_isFirstStation) //if its the very first station
                    btnRelease.Text = "Cancel Vehicle Input";
                else
                    btnRelease.Enabled = false;
            }
        }

        //#region headerButton
        ////action btnNextProcess
        //protected void btnNextProcess_Click(object sender, EventArgs e)
        //{
        //    var user = ProductionRepository.GetUser(tbLogin.Text.Trim());
        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbLogin.Text = "";
        //        return;
        //    }
        //    tbLogin.Text = "";



        //    ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
        //        return;
        //    }

        //    Station station = ProductionRepository.GetProductionStation(_stationId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
        //        return;
        //    }

        //    //TODO: Implement using StationPermission
        //    //Only the Quality can move the car on station GATE 1 and GATE 2

        //    string GetOrgUser = ProductionRepository.getOrg(user.Id);
        //    if (_stationId.ToString() == "39" || _stationId.ToString() == "41")
        //    {
        //        if (GetOrgUser.ToString() != "3")
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Only the Quality can move the car on this station ... ');", true);
        //            return; 
        //        }
        //    }


        //    if (_isSubAssy)
        //    {
        //        //In subassy, this is Complete Assemby button
        //        try
        //        {
        //            bool status = prodHist.SubAssyMoveToNextStation(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station (SubAssy), please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station (SubAssy): " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            bool status = prodHist.MoveToNextStation(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }
        //    }

        //    if (_isSubAssy)
        //        Response.Redirect("../Production/SubAssemblyStation.aspx?SectId=" + _sectId);
        //    else
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully Move to Next Station !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //Response.Redirect("../Production/ProductionDashboard.aspx");

        //}

        ////action btnMoveRectification
        //protected void btnMoveRecti_Click(object sender, EventArgs e)
        //{
        //    var user = ProductionRepository.GetUser(tbLoginRecti.Text.Trim());
        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbLoginRecti.Text = "";
        //        return;
        //    }
        //    tbLoginRecti.Text = "";

        //    ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
        //        return;
        //    }

        //    DotWeb.Models.Station station = ProductionRepository.GetProductionStation(_stationId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
        //        return;
        //    }

        //    try
        //    {
        //        bool status = prodHist.MoveToRectification(station, user.UserName);
        //        if (!status)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to RECTIFICATION, please contact your administrator');", true);
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to RECTIFICATION: " + ex.Message + "');", true);
        //        LoggerHelper.LogError(ex.Message);
        //        return;
        //    }

        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully sent to Rectification !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //Response.Redirect("../Production/ProductionDashboard.aspx");
        //}

        ////action btnMoveOffline
        //protected void btnMoveOffline_Click(object sender, EventArgs e)
        //{
        //    var user = ProductionRepository.GetUserQM(tbLoginOffline.Text.Trim());
        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbLoginOffline.Text = "";
        //        return;
        //    }
        //    tbLoginOffline.Text = "";

        //    ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
        //        return;
        //    }

        //    Station station = ProductionRepository.GetProductionStation(_stationId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
        //        return;
        //    }

        //    if (_isLastStation)
        //    {
        //        //In last station, this button is Move To VOCA
        //        try
        //        {
        //            bool status = false;
        //            if (ConfigurationHelper.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
        //            {
        //                status = prodHist.MoveToTearDown(station, user.UserName);
        //            }
        //            else
        //            {
        //                status = prodHist.MoveToVOCA(station, user.UserName);
        //            }

        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to VOCA, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to VOCA: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }

        //    }
        //    else
        //    {
        //        try
        //        {
        //            bool status = prodHist.MoveToOffline(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to OFFLINE, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to OFFLINE: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }
        //    }

        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully " + btnMoveOffline.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //Response.Redirect("../Production/ProductionDashboard.aspx");
        //}

        ////action btnFault
        //protected void btnFault_Click(object sender, EventArgs e)
        //{
        //    //if (ProductionRepository.IsEndOfLineStation(_stationId)) //**On End Of Line stations, not only QM user that can input fault.*/
        //    //{
        //    //    var user = ProductionRepository.GetUser(tbLogin.Text.Trim());
        //    //}
        //    //else
        //    //{
        //    //    user = ProductionRepository.GetUserQM(tbLogin2.Text.Trim());
        //    //}

        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbLogin2.Text = "";
        //        return;
        //    }
        //    tbLogin2.Text = "";

        //    Session["hdnUserAuth"] = user.AuthKey;

        //    pcInteruptFault.ShowOnPageLoad = true;
        //    loadFaultDefault();
        //    //var GetValueInspector = ProductionRepository.GetUserId(tbLogin2.Text);
        //    //cbInspector.Value = GetValueInspector == null ? 0 : GetValueInspector;
        //    cbInspector.Value = user.Id;
        //    pcLogin2.ShowOnPageLoad = false;
        //}

        ////action btnRelease
        //protected void btnRelease_Click(object sender, EventArgs e)
        //{
        //    var user = ProductionRepository.GetUserQM(tbloginRelease.Text.Trim());

        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbloginRelease.Text = "";
        //        return;
        //    }
        //    tbloginRelease.Text = "";

        //    ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
        //        return;
        //    }

        //    Station station = ProductionRepository.GetProductionStation(_stationId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
        //        return;
        //    }

        //    if (_isFirstStation)
        //    {
        //        //in first station, this is Release button
        //        try
        //        {
        //            bool status = prodHist.CancelProduction(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to Cancel Production, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to Cancel Production: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }

        //    }
        //    else
        //    {
        //        try
        //        {
        //            bool status = prodHist.ReleaseProduction(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to Release Production, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to Release Production: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }
        //    }

        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully released !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //Response.Redirect("../Production/ProductionDashboard.aspx");
        //}

        //protected void btnMoveVOCA_Click(object sender, EventArgs e)
        //{
        //    var user = ProductionRepository.GetUserQM(tbLoginVOCA.Text.Trim());
        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbLoginVOCA.Text = "";
        //        return;
        //    }
        //    tbLoginVOCA.Text = "";

        //    ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
        //        return;
        //    }

        //    Station station = ProductionRepository.GetProductionStation(_stationId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
        //        return;
        //    }

        //    string confirmValue = Request.Form["confirm_value"];
        //    if (confirmValue == "Yes")
        //    {
        //        try
        //        {
        //            bool status = prodHist.MoveToVOCA(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to VOCA, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to VOCA: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully " + BtnMoveVOCA.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);

        //    }
        //    else
        //    {
        //        // ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cancel move to VOCA !');;window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cancel move to VOCA !');window.location.assign('../Production/ProductionDashboard.aspx');", true);

        //        return;
        //    }


        //    // ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully " + btnMoveOffline.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //Response.Redirect("../Production/ProductionDashboard.aspx");
        //}

        //protected void BtnMovePA_Click(object sender, EventArgs e)
        //{
        //    var user = ProductionRepository.GetUserQM(tbLoginPA.Text.Trim());
        //    if (user == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        tbLoginPA.Text = "";
        //        return;
        //    }
        //    tbLoginPA.Text = "";

        //    ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
        //        return;
        //    }

        //    Station station = ProductionRepository.GetProductionStation(_stationId);
        //    if (prodHist == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
        //        return;
        //    }

        //    //    try
        //    //    {
        //    //        bool status = prodHist.MoveToPA(station, user.UserName);
        //    //        if (!status)
        //    //        {
        //    //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to PA, please contact your administrator');", true);
        //    //            return;
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to PA: " + ex.Message + "');", true);
        //    //        LoggerHelper.LogError(ex.Message);
        //    //        return;
        //    //    }

        //    //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully " + BtnMovePA.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //    //Response.Redirect("../Production/ProductionDashboard.aspx");

        //    string confirmValue = Request.Form["confirm_value"];
        //    if (confirmValue == "Yes")
        //    {
        //        try
        //        {
        //            bool status = prodHist.MoveToPA(station, user.UserName);
        //            if (!status)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to PA, please contact your administrator');", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to PA: " + ex.Message + "');", true);
        //            LoggerHelper.LogError(ex.Message);
        //            return;
        //        }
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Succesfully ? " + BtnMovePA.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cancel move to PA !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //        return;
        //    }
        //    // ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Succesfully ? " + BtnMovePA.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
        //    //Response.Redirect("../Production/ProductionDashboard.aspx");
        //}
        //#endregion

        //#region grid

        //////binding gridControlPlan
        ////protected void cpPerStationGrid_DataBinding(object sender, EventArgs e)
        ////{
        ////    ASPxGridView cpPerStationGridView = (ASPxGridView)sender;
        ////    var controlplan = ProductionRepository.GetProductionProcessByStationHistory(_stationId, _historyId);
        ////    cpPerStationGridView.DataSource = controlplan;

        ////    cpPerStationGridView.HtmlRowPrepared += CpPerStationGridView_HtmlRowPrepared;
        ////}

        ////binding gridFault
        //protected void gridFault_DataBinding(object sender, EventArgs e)
        //{
        //    ASPxGridView faultGridView = (ASPxGridView)sender;
        //    var faults = FaultRepository.GetFaultRecord(_historyId);
        //    faultGridView.DataSource = faults;

        //    (faultGridView.Columns["FaultStatus"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = Enum.GetValues(typeof(EFaultStatus));
        //}

        //protected void gridFault_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    var model = new FaultRecord();
        //    model.FaultRecordId = (int)e.Keys[0];
        //    model.FaultStatus = e.NewValues["FaultStatus"].ToString() == EFaultStatus.Open.ToString() ?
        //        (int)EFaultStatus.Open : (int)EFaultStatus.Close;
        //    model.Remarks = (string)e.NewValues["Remarks"];

        //    FaultRepository.UpdateFaultRecord(model);

        //    ASPxGridView senderGridView = (ASPxGridView)sender;
        //    e.Cancel = true;
        //    senderGridView.CancelEdit();
        //    senderGridView.DataSource = FaultRepository.GetFaultRecord(_historyId);
        //}

        ////binding gridIrregular
        //protected void gridIrregular_DataBinding(object sender, EventArgs e)
        //{
        //    ASPxGridView irregularGridView = (ASPxGridView)sender;
        //    var irregular = string.Empty;
        //    irregularGridView.DataSource = ProductionRepository.RetrieveIrregAlterationBy(_stationId, _historyId, lblPackingMonth.Text);
        //}

        ////binding gridIrregular
        //protected void gridSubAssy_DataBinding(object sender, EventArgs e)
        //{
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    var irregular = string.Empty;
        //    grid.DataSource = ProductionRepository.GetSubAssemblyStatus(_historyId, _stationId);
        //}

        //////set prepared gridControlPlan
        ////private static void CpPerStationGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (e.VisibleIndex >= 0)
        ////        {
        ////            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
        ////                e.Row.Height = Unit.Pixel(30);

        ////            ASPxGridView grv = (sender as ASPxGridView);
        ////            ASPxLabel lblVDoc = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblVDoc");
        ////            var stations = ProductionRepository.GetStationCondition(_stationId, _historyId);

        ////            if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "VDoc")) == true)
        ////            {
        ////                lblVDoc.Visible = true;
        ////                lblVDoc.Text = "X";
        ////            }

        ////            ASPxLabel lbl2Hand = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lbl2Hand");

        ////            if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "SecondHand")) == true)
        ////            {
        ////                lbl2Hand.Visible = true;
        ////                lbl2Hand.Text = "X";
        ////            }

        ////            ASPxLabel lblDS = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblDS");

        ////            if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "DS")) == true)
        ////            {
        ////                lblDS.Visible = true;
        ////                lblDS.Text = "X";
        ////            }

        ////            ASPxLabel lblCS2 = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblCS2");

        ////            if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "CS2")) == true)
        ////            {
        ////                lblCS2.Visible = true;
        ////                lblCS2.Text = "X";
        ////            }

        ////            ASPxLabel lblCS3 = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblCS3");

        ////            if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "CS3")) == true)
        ////            {
        ////                lblCS3.Visible = true;
        ////                lblCS3.Text = "X";
        ////            }
        ////            ASPxLabel lblDRT = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblDRT");
        ////            if (Convert.ToString(grv.GetRowValues(e.VisibleIndex, "DRT")) == "N" || Convert.ToString(grv.GetRowValues(e.VisibleIndex, "DRT")) == "J" || Convert.ToString(grv.GetRowValues(e.VisibleIndex, "DRT")) == "1")
        ////            {
        ////                lblDRT.Visible = true;
        ////                lblDRT.Text = "X";
        ////            }
        ////            else
        ////            {
        ////                lblDRT.Visible = true;
        ////                lblDRT.Text = "";
        ////            }

        ////            ASPxLabel lblFBS = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblFBS");
        ////            if (Convert.ToString(grv.GetRowValues(e.VisibleIndex, "FBS")) == "N" || Convert.ToString(grv.GetRowValues(e.VisibleIndex, "FBS")) == "J" || Convert.ToString(grv.GetRowValues(e.VisibleIndex, "FBS")) == "1")
        ////            {
        ////                lblFBS.Visible = true;
        ////                lblFBS.Text = "X";
        ////            }
        ////            else
        ////            {
        ////                lblFBS.Visible = true;
        ////                lblFBS.Text = "";
        ////            }


        ////            //gridview di WI/II
        ////            string processNo = grv.GetRowValues(e.VisibleIndex, "CgisNo").ToString();
        ////            ASPxGridView grvWIDocs = (ASPxGridView)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "grvWIDocs");
        ////            int countAttWI = ControlPlanRepository.CountAttachment("CPWI", processNo);

        ////            if (countAttWI > 0)
        ////            {
        ////                grvWIDocs.Visible = true;
        ////                grvWIDocs.DataSource = ControlPlanRepository.RetrieveControlPlanAttachmentByProcessAndDocType(processNo, "CPWI", stations.Current.ModelId);
        ////                grvWIDocs.DataBind();
        ////            }
        ////            else
        ////            {
        ////                grvWIDocs.Visible = false;
        ////            }

        ////            ASPxGridView grvIIDocs = (ASPxGridView)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "grvIIDocs");
        ////            int countAttII = ControlPlanRepository.CountAttachment("CPII", processNo);

        ////            if (countAttII > 0)
        ////            {
        ////                grvIIDocs.Visible = true;
        ////                grvIIDocs.DataSource =
        ////                    ControlPlanRepository.RetrieveControlPlanAttachmentByProcessAndDocType(processNo, "CPII", stations.Current.ModelId);
        ////                grvIIDocs.DataBind();
        ////            }
        ////            else
        ////            {
        ////                grvIIDocs.Visible = false;
        ////            }

        ////            ASPxLabel lblDialog = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblDialog");
        ////            if (grv.GetRowValues(e.VisibleIndex, "DialogAddress") != null)
        ////            {
        ////                lblDialog.Text = grv.GetRowValues(e.VisibleIndex, "DialogAddress").ToString();
        ////            }

        ////            ASPxLabel lblRf = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblRf");
        ////            if (!string.IsNullOrEmpty(grv.GetRowValues(e.VisibleIndex, "Rf").ToString()))
        ////                lblRf.Text = String.Format("{0:0.0#}", Convert.ToDecimal(grv.GetRowValues(e.VisibleIndex, "Rf").ToString()));
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        LoggerHelper.LogError(ex);
        ////    }
        ////}

        ////action btnDownloadWII
        //protected void btnDownloadWI_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxButton btn = (sender as ASPxButton);
        //        Response.Redirect("../DownloadFile.aspx?type=CPWI&Id=" + btn.CommandArgument);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }
        //}

        ////action btnDownloadII
        //protected void btnDownloadII_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxButton btn = (sender as ASPxButton);
        //        Response.Redirect("../DownloadFile.aspx?type=CPII&Id=" + btn.CommandArgument);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }
        //}

        ////binding btnCGIS
        //protected void btnCgisNo_DataBinding(object sender, EventArgs e)
        //{
        //    //ASPxButton btn = (sender as ASPxButton);
        //    //var command = "function (s, e) {window.open('../ControlPlans/CpCgisSheet.aspx?cpProcessId=" + btn.CommandArgument + "', '_blank')}";
        //    //btn.ClientSideEvents.Click = command;

        //    ASPxButton btn = (sender as ASPxButton);
        //    var data = ControlPlanRepository.RetrieveControlPlanProcessById(Convert.ToInt32(btn.CommandArgument));
        //    int cpid = (int)data.ControlPlanId;
        //    string cgisno = data.CgisNo;
        //    //Type oType = TypeRepository.RetrieveTypeByControlPlanId(cpid);
        //    ControlPlanPerId cp = ControlPlanRepository.RetrieveControlPlanByCpId(cpid);

        //    //DateTime dt = DateTime.ParseExact(cp.Vpm + "01", "yyyyMMdd", CultureInfo.InvariantCulture);
        //    List<LastCgisManual> cgismanual = CgisSheetManualRepository.GetListLastCgisManual((int)cp.ModelId, (int)cp.VariantId, cgisno, cp.Vpm.ToInt32(201601));
        //    var singleresult = (from a in cgismanual select a).OrderByDescending(a => a.ValidFrom).FirstOrDefault();
        //    if (cgismanual.Count > 0)
        //    {
        //        if (singleresult.FileType == "application/pdf")
        //        {
        //            btn.OnClientClick = "function(s,e){window.open('../CommonWorks/CustomDocumentViewer.aspx?Id=" + singleresult.Id + "&docType=CGISManual" + "','_blank')}";
        //        }
        //        else
        //        {
        //            btn.OnClientClick = "function(s,e){window.open('../DownloadFile.aspx?type=CGISManual&Id=" + singleresult.Id + "')}";
        //            //Response.Redirect("../DownloadFile.aspx?type=CGISManual&Id=" + singleresult.Id);
        //        }
        //    }
        //    else
        //    {
        //        btn.OnClientClick = "function(s,e){window.open('../ControlPlans/CpCgisSheet.aspx?CGISStation=" + cgisno +
        //                        "&CGISModel=" + oType.CGISModel + "&vpm=" + cp.Vpm + "&model=" + cp.ModelId + "&variant=" + cp.VariantId + "','_blank')}";
        //    }
        //}
        //#endregion

        //#region fault
        ////action btnSave&New
        //protected void btnRecordNew_Click(object sender, EventArgs e)
        //{
        //    //is exist in fault record
        //    if (HiddenText["Edit"].ToString() == "0")
        //    {
        //        var fault = faultRecord();

        //        //is exist in fault record
        //        //if (FaultRepository.IsFaultSame(fault.ProductionHistoriesId, fault.CGISId))
        //        //{
        //        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Check Again, Fault is already in database !!!');", true);
        //        //    loadFaultDefault();
        //        //}
        //        //else
        //        //{
        //        int GetOrgUser = Convert.ToInt32(ProductionRepository.getOrg(user.Id));
        //        if (CSTypeValue.SelectedItem.ToString() == "2" && GetOrgUser.ToString() == "6")
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Only the Qm can input CS 2 !!! ');", true);
        //            loadFaultDefault();
        //            return;
        //        }
        //        loadFaultDefault();
        //        //add to fault record
        //        int Id = FaultRepository.InsertFaultRecord(fault);
        //        //close popUpControl Fault

        //        //binding again grid Fault
        //        gridFault.DataBind();
        //        //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
        //        var SumRowData = GVDataAttachment.VisibleRowCount;
        //        List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
        //        for (int i = 0; i < SumRowData; i++)
        //        {
        //            DocumentAttachment AttachFile = new DocumentAttachment();
        //            AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
        //            AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
        //            ListAttachFile.Add(AttachFile);
        //        }
        //        for (int x = 0; x < ListAttachFile.Count(); x++)
        //        {
        //            var cbFINNUmber = cbFINNumber.Text.Split('/')[1].Trim();
        //            FaultRepository.InsertAttachMentFromDasboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, cbFINNUmber, Id);
        //        }
        //        detailCondition.ImageUrl =
        //            FaultRepository.IsHaveFault(_historyId) ||
        //            (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
        //            ? urlImgNotifRed : urlImgNotifGreen;
        //    }
        //    else
        //    {
        //        var model = new FaultRecord();
        //        model.CSType = CSTypeValue.Value == null ? 0 : int.Parse(CSTypeValue.Value.ToString());
        //        model.FaultRecordId = int.Parse(HiddenText["IdFault"].ToString());
        //        model.FaultStatus = cbFaultStatus.Text == "" ? 0 : int.Parse(cbFaultStatus.SelectedItem.Value.ToString());
        //        model.Remarks = tbRemark.Text.ToString();
        //        model.Priority = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.Value.ToString());
        //        model.FaultRelatedTypeId = cbFaultRelated.SelectedItem == null ? 0 : int.Parse(cbFaultRelated.Value.ToString());
        //        model.IsSentToRectification = Convert.ToBoolean(cbRecti.Value.ToInt32());
        //        model.IsPdi0 = Convert.ToBoolean(cbIsPDI0.Value.ToInt32());
        //        model.FaultDescriptionId = cbFaultDesc.SelectedItem == null ? 0 : int.Parse(cbFaultDesc.Value.ToString());
        //        model.FaultDescriptionText = cbFaultDesc.Text.ToString();
        //        model.FaultPartProcessId = cbPartProces.SelectedItem == null ? 0 : int.Parse(cbPartProces.Value.ToString());
        //        model.StamperUserID = cbStamp.SelectedItem == null ? 0 : int.Parse(cbStamp.Value.ToString());
        //        model.StamperName = cbStamp.Text.ToString();
        //        model.QualityGateId = cbQG.SelectedItem == null ? 0 : int.Parse(cbQG.SelectedItem.Value.ToString());
        //        model.NCP = tbNCP.Text.ToString();
        //        model.InspectorUserId = cbInspector.SelectedItem == null ? 0 : int.Parse(cbInspector.Value.ToString());
        //        model.InspectorName = cbInspector.Text.ToString();
        //        model.RecordDate = DateTime.Parse(currentdate.Value.ToString("yyy/MM/dd"));
        //        FaultRepository.UpdateFaultRecord(model);
        //        //binding again grid Fault
        //        gridFault.DataBind();
        //        //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
        //        var SumRowData = GVDataAttachment.VisibleRowCount;
        //        List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
        //        for (int i = 0; i < SumRowData; i++)
        //        {
        //            DocumentAttachment AttachFile = new DocumentAttachment();
        //            AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
        //            AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
        //            ListAttachFile.Add(AttachFile);
        //        }
        //        FaultRepository.Delete(int.Parse(Session["IdFault"].ToString()));
        //        for (int x = 0; x < ListAttachFile.Count(); x++)
        //        {
        //            var id = cbFINNumber.Text;
        //            FaultRepository.InsertAttachMentFaultDashboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, id, int.Parse(Session["IdFault"].ToString()));
        //        }
        //        detailCondition.ImageUrl =
        //            FaultRepository.IsHaveFault(_historyId) ||
        //            (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
        //            ? urlImgNotifRed : urlImgNotifGreen;
        //    }
        //    loadFaultDefault();
        //}

        ////action btnSave&Close
        //protected void btnSaveClose_Click(object sender, EventArgs e)
        //{
        //    if (HiddenText["Edit"].ToString() == "0")
        //    {
        //        var fault = faultRecord();
        //        //is exist in fault record
        //        //if (FaultRepository.IsFaultSame(fault.ProductionHistoriesId, fault.CGISId))
        //        //{
        //        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Check Again, Fault is already in database !!!');", true);
        //        //    loadFaultDefault();
        //        //}
        //        //else
        //        //{
        //        //  loadFaultDefault();
        //        //add to fault record  int GetOrgUser = Convert.ToInt32(ProductionRepository.getOrg(user.Id));

        //        int GetOrgUser = Convert.ToInt32(ProductionRepository.getOrg(user.Id));
        //        if (CSTypeValue.SelectedItem.ToString() == "2" && GetOrgUser.ToString() == "6")
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Only the Qm can input CS 2 !!! ');", true);
        //                loadFaultDefault();
        //                return;
        //            }
        //        int Id = FaultRepository.InsertFaultRecord(fault);
        //        //close popUpControl Fault
        //        pcInteruptFault.ShowOnPageLoad = false;
        //        pcLogin2.ShowOnPageLoad = false;
        //        //binding again grid Fault
        //        gridFault.DataBind();
        //        //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
        //        var SumRowData = GVDataAttachment.VisibleRowCount;
        //        List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
        //        for (int i = 0; i < SumRowData; i++)
        //        {
        //            DocumentAttachment AttachFile = new DocumentAttachment();
        //            AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
        //            AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
        //            ListAttachFile.Add(AttachFile);
        //        }
        //        for (int x = 0; x < ListAttachFile.Count(); x++)
        //        {
        //            var cbFINNUmber = cbFINNumber.Text.Split('/')[1].Trim();
        //            FaultRepository.InsertAttachMentFromDasboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, cbFINNUmber, Id);
        //        }
        //        detailCondition.ImageUrl =
        //            FaultRepository.IsHaveFault(_historyId) ||
        //            (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
        //            ? urlImgNotifRed : urlImgNotifGreen;
        //    }
        //    else
        //    {
        //        var model = new FaultRecord();
        //        model.CSType = CSTypeValue.Value == null ? 0 : int.Parse(CSTypeValue.Value.ToString());
        //        model.FaultRecordId = int.Parse(HiddenText["IdFault"].ToString());
        //        //model.FaultStatus = cbFaultStatus.Text == "" ? 0 : int.Parse(cbFaultStatus.Value.ToString());
        //        model.Remarks = tbRemark.Text.ToString();
        //        model.Priority = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.Value.ToString());
        //        model.FaultRelatedTypeId = cbFaultRelated.SelectedItem == null ? 0 : int.Parse(cbFaultRelated.Value.ToString());
        //        model.IsSentToRectification = Convert.ToBoolean(cbRecti.Value.ToInt32());
        //        model.IsPdi0 = Convert.ToBoolean(cbIsPDI0.Value.ToInt32());
        //        model.FaultDescriptionId = cbFaultDesc.SelectedItem == null ? 0 : int.Parse(cbFaultDesc.Value.ToString());
        //        model.FaultDescriptionText = cbFaultDesc.Text.ToString();
        //        model.FaultPartProcessId = cbPartProces.SelectedItem == null ? 0 : int.Parse(cbPartProces.Value.ToString());
        //        model.StamperUserID = cbStamp.SelectedItem == null ? 0 : int.Parse(cbStamp.Value.ToString());
        //        model.StamperName = cbStamp.Text.ToString();
        //        model.QualityGateId = cbQG.SelectedItem == null ? 0 : int.Parse(cbQG.SelectedItem.Value.ToString());
        //        model.NCP = tbNCP.Text.ToString();
        //        model.InspectorUserId = cbInspector.SelectedItem == null ? 0 : int.Parse(cbInspector.Value.ToString());
        //        model.InspectorName = cbInspector.Text.ToString();
        //        model.RecordDate = DateTime.Parse(currentdate.Value.ToString("yyy/MM/dd"));
        //        model.FaultStatus = cbFaultStatus.SelectedItem == null ? 0 : int.Parse(cbFaultStatus.Value.ToString());

        //        FaultRepository.UpdateFaultRecord(model);
        //        pcInteruptFault.ShowOnPageLoad = false;
        //        pcLogin2.ShowOnPageLoad = false;
        //        //binding again grid Fault
        //        gridFault.DataBind();
        //        //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
        //        var SumRowData = GVDataAttachment.VisibleRowCount;
        //        List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
        //        for (int i = 0; i < SumRowData; i++)
        //        {
        //            DocumentAttachment AttachFile = new DocumentAttachment();
        //            AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
        //            AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
        //            ListAttachFile.Add(AttachFile);
        //        }
        //        FaultRepository.Delete(int.Parse(Session["IdFault"].ToString()));
        //        for (int x = 0; x < ListAttachFile.Count(); x++)
        //        {
        //            var id = cbFINNumber.Text;
        //            FaultRepository.InsertAttachMentFaultDashboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, id, int.Parse(Session["IdFault"].ToString()));
        //        }
        //        detailCondition.ImageUrl =
        //            FaultRepository.IsHaveFault(_historyId) ||
        //            (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
        //            ? urlImgNotifRed : urlImgNotifGreen;
        //    }

        //    //  }
        //}

        //protected void btnClose_Click(object sender, EventArgs e)
        //{
        //    pcInteruptFault.ShowOnPageLoad = false;
        //    pcLogin2.ShowOnPageLoad = false;

        //    gridFault.DataBind();
        //    // detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;

        //    detailCondition.ImageUrl =
        //            FaultRepository.hasFault(_historyId) ||
        //            (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
        //            ? urlImgNotifRed : urlImgNotifGreen;

        //}

        //protected void pcInteruptFault_WindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    //loadFaultDefault();
        //    //GVDataAttachment.DataSource = Session["dtAttachment"];
        //    GVDataAttachment.DataSource = Session["Attachment"];
        //    GVDataAttachment.DataBind();
        //}

        ////binding FaultRecord
        //public FaultRecord faultRecord()
        //{
        //    int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToInt32(1);

        //    int csType = CSTypeValue.Value == null ? 0 : int.Parse(CSTypeValue.Value.ToString());
        //    var ncp = tbNCP.Text;
        //    int productionHistoriesId = Convert.ToInt32(cbFINNumber.Value);
        //    var faultNumber = "32054";
        //    int QGate = Convert.ToInt32(cbQG.Value);
        //    int station = Convert.ToInt32(cbStation.Value);
        //    ListEditItem selectedItem = cbFINNumber.SelectedItem;
        //    var finnumber = selectedItem.GetValue("FINNumber").ToString();

        //    var tempAssemblySection = ProductionRepository.GetAssemblySection(station);
        //    int assemblySectionId = tempAssemblySection.Id;

        //    var tempProductionLine = ProductionRepository.GetLineByProductionId(productionHistoriesId);
        //    int productionLineId = tempProductionLine.Id;

        //    var cgisId = Convert.ToInt32(cbCGIS.Value);
        //    int priority = Convert.ToInt32(cbPriority.Value);
        //    int partProces = Convert.ToInt32(cbPartProces.Value);
        //    int faultRelated = Convert.ToInt32(cbFaultRelated.Value);
        //    int faultDescId;
        //    if (int.TryParse(cbFaultDesc.Value.ToString(), out faultDescId))
        //    {
        //        faultDescId = Convert.ToInt32(cbFaultDesc.Value);
        //    }
        //    else
        //    {
        //        faultDescId = ProductionRepository.InputNonStandard(cbFaultDesc.Value.ToString());
        //    }
        //    var faultDescText = cbFaultDesc.Text;

        //    int inspectorId = Convert.ToInt32(cbInspector.Value);

        //    var inspectorName = cbInspector.SelectedItem == null ? "" : cbInspector.SelectedItem.Text;
        //    int stampId = cbStamp.Value == null ? 0 : Convert.ToInt32(cbStamp.Value);
        //    // var stampName = cbStamp.SelectedItem;
        //    var remark = tbRemark.Text.ToString();
        //    bool recti = Convert.ToBoolean(cbRecti.Value.ToInt32());
        //    bool pdi0 = Convert.ToBoolean(cbIsPDI0.Value.ToInt32());

        //    string userauth = Session["hdnUserAuth"].ToString();


        //    FaultRecord fault = new FaultRecord()
        //    {
        //        CSType = csType,
        //        NCP = ncp,
        //        ProductionHistoriesId = productionHistoriesId,
        //        FaultNumber = faultNumber,
        //        QualityGateId = QGate,
        //        AssemblySectionId = assemblySectionId,
        //        ProductionLinesId = productionLineId,
        //        StationId = station,
        //        FINNumber = finnumber.ToString(),
        //        CGISId = cgisId,
        //        Priority = priority,
        //        FaultPartProcessId = partProces,
        //        FaultRelatedTypeId = faultRelated,
        //        FaultDescriptionId = faultDescId,

        //        FaultDescriptionText = faultDescText,
        //        InspectorUserId = inspectorId,
        //        InspectorName = inspectorName.ToString(),
        //        StamperUserID = stampId,
        //        StamperName = "",
        //        Remarks = remark,
        //        IsSentToRectification = recti,
        //        IsPdi0 = pdi0,
        //        UserAuth = userauth
        //    };

        //    return fault;
        //}

        ////binding & set default form Fault
        //private void loadFaultDefault()
        //{
        //    try
        //    {
        //        Session["Attachment"] = null;
        //        GVDataAttachment.DataSource = null;
        //        GVDataAttachment.DataBind();
        //        currentdate.Date = DateTime.Now;
        //        HiddenText["Edit"] = "0";
        //        cbStation.SelectedIndex = 0;
        //        cbQG.SelectedIndex = 0;
        //        cbFINNumber.Value = _historyId;
        //        HiddenText["FINNumber"] = cbFINNumber.Text;
        //        cbPartProces.SelectedIndex = 0;
        //        cbFaultDesc.SelectedIndex = 0;
        //        cbFaultRelated.SelectedIndex = 0;
        //        cbPriority.SelectedIndex = 0;
        //        cbInspector.SelectedIndex = 0;
        //        cbStamp.SelectedIndex = 0;
        //        cbRecti.SelectedIndex = 0;
        //        cbIsPDI0.SelectedIndex = 0;
        //        tbNCP.Text = "";
        //        tbRemark.Text = "";
        //        cbFaultStatus.Value = "0";
        //        //set TextBox Line
        //        string line = GetLine(Convert.ToInt32(cbFINNumber.Value));
        //        tbLine.Text = line;

        //        //set textBox Assembly Section
        //        string section = GetSection(Convert.ToInt32(cbStation.Value));
        //        tbAssemblySection.Text = section;

        //        //set textBox Fault Classification
        //        string classification = GetClassification(Convert.ToInt32(cbFaultDesc.Value));
        //        tbClassification.Text = classification;

        //        //set comboBox CGIS
        //        Session["StationId"] = cbStation.Value;
        //        Session["HistoryId"] = cbFINNumber.Value;
        //        cbCGIS.DataSource = sdsCgis;
        //        cbCGIS.DataBind();
        //        cbCGIS.SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex.Message);
        //    }

        //}

        ////callback comboBox CGIS from comboBox StationId & HistoryId
        //protected void cbCGIS_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    Session["StationId"] = cbStation.Value;
        //    Session["HistoryId"] = _historyId;
        //    cbCGIS.DataSource = sdsCgis;
        //    cbCGIS.DataBind();
        //    cbCGIS.SelectedIndex = 0;
        //}

        //[WebMethod]
        ////get Line by HistoryId
        //public static string GetLine(int values)
        //{
        //    var tempLine = ProductionRepository.GetLineByProductionId(values);
        //    return tempLine.LineName;
        //}

        //[WebMethod]
        ////get AssemblySection by StationId
        //public static string GetSection(int values)
        //{
        //    var tempSection = StationRepository.RetrieveAsemblySectionByStId(values);
        //    return tempSection;
        //}

        //[WebMethod]
        ////get Fault Classification by DescriptionId
        //public static string GetClassification(int values)
        //{
        //    var tempClassification = FaultRepository.GetFaultClassification(values);
        //    return tempClassification.Description;
        //}
        //#endregion

        //#region Problem

        //protected void btnProblem_Click(object sender, EventArgs e)
        //{
        //    ASPxButton btn = (sender as ASPxButton);

        //    var conditionalBtn = btn.CommandArgument;

        //    var userauthkey = ProductionRepository.CheckAuth(AuthProblem.Text);

        //    // authkey




        //    if (userauthkey == "" || userauthkey == null)
        //    {

        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        PcProblemAuth.ShowOnPageLoad = false;
        //        return;
        //    }
        //    string problem = ProductionRepository.GetProblemId(_stationId, _lineId).ToString();
        //    if (problem != "-1")
        //    {
        //        for (int i = 0; i < rbProblem.Items.Count; i++)
        //        {
        //            if (rbProblem.Items[i].Value.ToString() == problem)
        //            {
        //                rbProblem.SelectedIndex = i;
        //                break;
        //            }
        //        }
        //    }
        //    else
        //        rbProblem.SelectedIndex = -1;

        //    PcProblemAuth.ShowOnPageLoad = false;
        //    pcProblem.ShowOnPageLoad = true;
        //}
        //protected void btnAddProblem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int problem = Convert.ToInt32(rbProblem.Value);
        //        ProductionRepository.InsertAndonProblemHistory(problem, _stationId, DateTime.Now, _lineId);
        //        pcProblem.ShowOnPageLoad = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        //alert process has move to next station
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Insert Andon Problem, please contact your administrator');", true);
        //        LoggerHelper.LogError(ex.Message);
        //    }


        //}
        //protected void btnCloseProblem_Click(object sender, EventArgs e)
        //{
        //    ASPxButton btn = (sender as ASPxButton);

        //    var conditionalBtn = btn.CommandArgument;

        //    var userauthkey = ProductionRepository.CheckAuth(tbLogin.Text);

        //    // authkey


        //    tbLogin.Text = "";

        //    if (userauthkey == "" || userauthkey == null)
        //    {

        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
        //        return;
        //    }
        //    pcProblem.ShowOnPageLoad = false;
        //    gridFault.DataBind();
        //}

        //protected void btnClearProblem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int problem = Convert.ToInt32(rbProblem.Value);
        //        ProductionRepository.UpdateAndonProblemHistory(problem, _stationId, _lineId);
        //        rbProblem.SelectedIndex = -1;


        //        pcProblem.ShowOnPageLoad = false;
        //        gridFault.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        //alert process has move to next station
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Insert Andon Problem, please contact your administrator');", true);
        //        LoggerHelper.LogError(ex.Message);
        //    }




        //}


        //#endregion

        //#region EditFault

        //protected void Btn_Back_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("../Production/ProductionDashboard.aspx");
        //}

        //protected void Btn_Edit_Click(object sender, EventArgs e)
        //{
        //    pcInteruptFault.ShowOnPageLoad = true;
        //    ASPxButton btn = sender as ASPxButton;
        //    GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
        //    var index = container.VisibleIndex;
        //    try
        //    {
        //        int ValueId = gridFault.GetRowValues(index, "FaultRecordId") == null ? 0 : int.Parse(gridFault.GetRowValues(index, "FaultRecordId").ToString());
        //        Session["IdFault"] = ValueId;
        //        if (ValueId != 0)
        //        {
        //            loadFaultDefaultEdit(ValueId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }

        //}

        ////binding & set default form Fault
        //private void loadFaultDefaultEdit(int id)
        //{
        //    HiddenText["Edit"] = "1";
        //    HiddenText["IdFault"] = id;
        //    Session["Attachment"] = null;
        //    Session["Attachmentx"] = null;
        //    var DataLoad = FaultRepository.GetFaultRecordById(id);
        //    //GVDataAttachment.DataSource = Session["dtAttachment"];
        //    //GVDataAttachment.DataBind();
        //    CSTypeValue.Text = DataLoad[0].CSType.ToString();
        //    cbStation.Text = DataLoad[0].StationName.ToString();
        //    cbStation.ReadOnly = true;
        //    cbStamp.Text = DataLoad[0].StamperName;
        //    tbRemark.Text = DataLoad[0].Remarks;
        //    cbQG.Text = DataLoad[0].QualityGateName.ToString();
        //    //cbQG.ReadOnly = true;
        //    cbFaultStatus.Text = DataLoad[0].FaultStatus.ToString();
        //    cbFINNumber.Text = DataLoad[0].FINNumber.ToString() + '/';
        //    Session["FinNumber"] = DataLoad[0].FINNumber.ToString();
        //    cbFINNumber.ReadOnly = true;
        //    tbLine.Text = DataLoad[0].ProductionLineId.ToString();
        //    tbLine.ReadOnly = true;
        //    tbNCP.Text = DataLoad[0].NCP.ToString();
        //    tbAssemblySection.Text = DataLoad[0].AssemblySectionName;
        //    cbStamp.Text = DataLoad[0].StamperName;
        //    cbPartProces.Text = DataLoad[0].FaultPartProcessDesc.ToString();
        //    cbFaultDesc.Text = DataLoad[0].FaultDescriptionText;
        //    cbFaultRelated.Text = DataLoad[0].FaultRelatedTypeDesc;
        //    cbPriority.Text = DataLoad[0].Priority.ToString();
        //    cbInspector.Text = DataLoad[0].InspectorName;
        //    cbStamp.Text = DataLoad[0].StamperName;
        //    cbRecti.Text = DataLoad[0].IsSentToRectification.ToString();
        //    cbIsPDI0.Text = DataLoad[0].IsPdi0.ToString();
        //    tbNCP.Text = DataLoad[0].NCP;
        //    tbRemark.Text = DataLoad[0].Remarks;
        //    currentdate.Value = DataLoad[0].RecordDate;
        //    //set TextBox Line
        //    //string line = GetLine(Convert.ToInt32(cbFINNumber.Value));
        //    tbLine.Text = DataLoad[0].ProductionLineName;

        //    //set textBox Assembly Section
        //    //string section = GetSection(Convert.ToInt32(cbStation.Value));
        //    tbAssemblySection.Text = DataLoad[0].AssemblySectionName;

        //    //set textBox Fault Classification
        //    //string classification = GetClassification(Convert.ToInt32(cbFaultDesc.Value));
        //    tbClassification.Text = DataLoad[0].FaultClassDescription;

        //    //set comboBox CGIS
        //    cbCGIS.Text = DataLoad[0].CGISNo;
        //    HiddenText["FINNumber"] = cbFINNumber.Text == "1" ? "-Select-" : cbFINNumber.Text;
        //    if (cbFINNumber.Text == "1")
        //    {
        //        cbFINNumber.Text = "-Select-";
        //    }
        //    else
        //    {
        //        cbFINNumber.Text = DataLoad[0].FINNumber;
        //    }
        //    var faults = FaultRepository.GetAttachMentViewEdit(id);
        //    Session["Attachment"] = faults;

        //    Session["Attachmentx"] = faults;
        //    GVDataAttachment.DataSource = faults;
        //    GVDataAttachment.DataBind();
        //}



        //protected void ShowAttachment_Click(object sender, EventArgs e)
        //{
        //    PopUpAttachMent.ShowOnPageLoad = true;
        //}

        //protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    pcInteruptFault.ShowOnPageLoad = false;
        //    PopUpAttachMent.ShowOnPageLoad = false;
        //    pcInteruptFault.ShowOnPageLoad = true;
        //    GVDataAttachment.DataSource = Session["Attachment"];
        //    GVDataAttachment.DataBind();
        //    //GVDataAttachmentx.DataSource = Session["dtAttachment"];
        //    //GVDataAttachmentx.DataBind();
        //}

        //protected void UploadAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        //{
        //    if (HiddenText["Edit"].ToString() == "0")
        //    {
        //        string x = "";
        //        string HiddenCstype = "2";
        //        if (HiddenText["FINNumber"].ToString() != "")
        //        {
        //            x = HiddenText["FINNumber"].ToString().Split('/')[1].Trim();
        //        }
        //        else
        //        {
        //            x = cbFINNumber.Text;
        //        }
        //        Session["FINNumber"] = x;
        //        Session["CSType"] = HiddenCstype;
        //        string PathName = ConfigurationHelper.UPLOAD_DIR + "/VPCReleaseFault/" + x;
        //        if (!Directory.Exists(Server.MapPath(PathName)))
        //        {
        //            Directory.CreateDirectory(Server.MapPath(PathName));
        //        }
        //        string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
        //        //string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
        //        string resultFileName = e.UploadedFile.FileName;
        //        string resultFileUrl = PathName + "/" + resultFileName;
        //        string resultFilePath = MapPath(resultFileUrl);
        //        e.UploadedFile.SaveAs(resultFilePath);

        //        // UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

        //        string name = e.UploadedFile.FileName;
        //        string url = ResolveClientUrl(resultFileUrl);
        //        long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
        //        string sizeText = sizeInKilobytes.ToString() + " KB";
        //        e.CallbackData = name + "|" + url + "|" + sizeText;
        //        // var oldattachImage = FaultRepository.GetAttachMentView(Session["FinNumber"].ToString());

        //        ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
        //        AttachFile.Id = GetIdAttachment();
        //        AttachFile.FileName = resultFileName;
        //        AttachFile.FileType = resultExtension;
        //        AttachFile.FileLocation = resultFileUrl;
        //        AttachFileModelList.Add(AttachFile);
        //        //ListAttachment.attachmentAuditModels = AttachFileModelList;
        //        // }


        //        DataTable table = ConvertListToDataTable(AttachFileModelList);
        //        Session["dtAttachment"] = table;
        //        Session["Attachment"] = AttachFileModelList;
        //        cbFINNumber.Text = HiddenText["FINNumber"].ToString();
        //        GVDataAttachment.DataSource = Session["Attachment"];
        //        GVDataAttachment.DataBind();
        //        PopUpAttachMent.ShowOnPageLoad = false;
        //        // PopUpAttachMentx.ShowOnPageLoad = false;
        //    }
        //    else
        //    {
        //        string HiddenCstype = "2";
        //        string x = Session["FinNumber"].ToString();
        //        Session["FINNumber"] = x;
        //        Session["CSType"] = HiddenCstype;
        //        string PathName = ConfigurationHelper.UPLOAD_DIR + "/VPCReleaseFault/" + x;
        //        if (!Directory.Exists(Server.MapPath(PathName)))
        //        {
        //            Directory.CreateDirectory(Server.MapPath(PathName));
        //        }
        //        string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
        //        //string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
        //        string resultFileName = e.UploadedFile.FileName;
        //        string resultFileUrl = PathName + "/" + resultFileName;
        //        string resultFilePath = MapPath(resultFileUrl);
        //        e.UploadedFile.SaveAs(resultFilePath);

        //        // UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

        //        string name = e.UploadedFile.FileName;
        //        string url = ResolveClientUrl(resultFileUrl);
        //        long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
        //        string sizeText = sizeInKilobytes.ToString() + " KB";
        //        e.CallbackData = name + "|" + url + "|" + sizeText;
        //        var Data = Session["Attachmentx"] as List<ProductionFaultReleaseAttachmentModel>;
        //        var oldattachImage = Session["Attachmentx"] as List<ProductionFaultReleaseAttachmentModel>;

        //        if (Data == null)
        //        {
        //            ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
        //            AttachFile.Id = GetIdAttachment();
        //            AttachFile.FileName = resultFileName;
        //            AttachFile.FileLocation = resultFileUrl;
        //            AttachFile.FileType = resultExtension;

        //            AttachFileModelList.Add(AttachFile);
        //            Session["Attachment"] = AttachFileModelList;
        //            //ListAttachment.attachmentAuditModels = AttachFileModelList;
        //        }
        //        else
        //        {
        //            ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
        //            for (int i = 0; i < oldattachImage.Count; i++)
        //            {
        //                AttachFile.Id = oldattachImage[i].Id;
        //                AttachFile.FileName = oldattachImage[i].FileName;
        //                AttachFile.FileType = oldattachImage[i].FileType;
        //                AttachFile.FileLocation = oldattachImage[i].FileLocation;
        //                AttachFileModelList.Add(AttachFile);
        //                //ListAttachment.attachmentAuditModels = AttachFileModelList;
        //                Session["Attachmentx"] = null;
        //            }
        //            ProductionFaultReleaseAttachmentModel AttachFilex = new ProductionFaultReleaseAttachmentModel();
        //            AttachFilex.Id = GetIdAttachment();
        //            AttachFilex.FileName = resultFileName;
        //            AttachFilex.FileLocation = resultFileUrl;
        //            AttachFilex.FileType = resultExtension;
        //            AttachFileModelList.Add(AttachFilex);
        //            //ListAttachment.AttachmentModels = AttachFileModelList;
        //            Session["Attachment"] = AttachFileModelList;
        //        }
        //        var DataView = Session["Attachment"] as List<ProductionFaultReleaseAttachmentModel>;
        //        DataTable table = ConvertListToDataTable(DataView);
        //        Session["dtAttachment"] = table;
        //        cbFINNumber.Text = HiddenText["FINNumber"].ToString();
        //        //GVDataAttachmentx.DataSource = Session["dtAttachment"];
        //        GVDataAttachment.DataSource = DataView;
        //        GVDataAttachment.DataBind();
        //    }

        //}

        //protected int GetIdAttachment()
        //{
        //    int IdData = 0;
        //    var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
        //    IdData = Id;
        //    return IdData;
        //}

        //static DataTable ConvertListToDataTable(List<ProductionFaultReleaseAttachmentModel> list)
        //{
        //    // New table.
        //    DataTable table = new DataTable();

        //    // Get max columns.
        //    int columns = list.Count();


        //    // Add columns.

        //    table.Columns.Add("Id");
        //    table.Columns.Add("FileName");
        //    table.Columns.Add("FileLocation");


        //    // Add rows.
        //    foreach (var array in list)
        //    {
        //        table.Rows.Add(array.Id, array.FileName, array.FileLocation);
        //    }

        //    return table;
        //}

        //protected void pcInteruptFault_DataBinding(object sender, EventArgs e)
        //{

        //}

        ////protected void cpPerStationGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        ////{
        ////    ASPxGridView grv = (sender as ASPxGridView);
        ////    if (grv.GetRowValues(e.VisibleIndex, "HasDeviation") != null)
        ////    {
        ////        if (grv.GetRowValues(e.VisibleIndex, "HasDeviation") != null &&
        ////        Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "HasDeviation")))
        ////        {
        ////            Color tlr = System.Drawing.ColorTranslator.FromHtml("#44C169");
        ////            e.Row.BackColor = tlr;
        ////        }
        ////    }

        ////}

        //protected void IdBtndelete_Click(object sender, EventArgs e)
        //{
        //    ASPxButton btn = sender as ASPxButton;
        //    GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
        //    var Rowindex = container.VisibleIndex;
        //    int ValueRowId = int.Parse(GVDataAttachment.GetRowValues(Rowindex, "Id").ToString());
        //    //var Data = Session["dtAttachment"];
        //    //var oldattachImage = Session["ByteImage"] as List<FaultAuditAttachmentModel>;
        //    // DataTable Datax = Session["dtAttachment"] as DataTable;
        //    //List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
        //    //List<DataRow> list = Datax.AsEnumerable().ToList();
        //    ////int Id = int.Parse(Datax.Rows[0].ItemArray[0].ToString());
        //    //for (int i = 0; i < Datax.Rows.Count; i++)
        //    //{
        //    //    int Id = int.Parse(Datax.Rows[i].ItemArray[0].ToString());
        //    //    if (Id == ValueRowId)
        //    //    {
        //    //        DataRow dr = Datax.Rows[i];
        //    //        dr.Delete();
        //    //        dr.AcceptChanges();
        //    //    }
        //    //}
        //    var oldattachImage = Session["Attachment"] as List<ProductionFaultReleaseAttachmentModel>;
        //    for (int i = 0; i < oldattachImage.Count; i++)
        //    {
        //        if (oldattachImage[i].Id == ValueRowId)
        //        {
        //            oldattachImage.Remove(oldattachImage[i]);
        //            Session["Attachment"] = oldattachImage;
        //        }
        //    }

        //    GVDataAttachment.DataSource = Session["Attachment"];
        //    GVDataAttachment.DataBind();

        //}

        //protected void Btn_Delete_Click(object sender, EventArgs e)
        //{
        //    int index = int.Parse(Session["IndexDelete"].ToString());

        //    try
        //    {
        //        FaultRepository.DeleteFault(index);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }
        //    FormConfirmDelete.ShowOnPageLoad = false;
        //    Response.Redirect(Request.RawUrl);
        //}

        //protected void BtnSaveIndex(object sender, EventArgs e)
        //{
        //    ASPxButton btn = sender as ASPxButton;
        //    GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

        //    var index = container.VisibleIndex;
        //    int ValueId = int.Parse(gridFault.GetRowValues(index, "FaultRecordId").ToString());
        //    Session["IndexDelete"] = ValueId;
        //    int GetOrgQEPSupervisor = Convert.ToInt32(ProductionRepository.getOrg(user.Id));
        //    string GetCreatedBy = Convert.ToString(ProductionRepository.getCreatedBy(ValueId));
        //    string GetUserId = Convert.ToString(ProductionRepository.getUserId(GetCreatedBy));
        //    if (GetUserId == user.Id.ToString())
        //    {
        //        FormConfirmDelete.ShowOnPageLoad = true;
        //    }
        //    else if(GetOrgQEPSupervisor.ToString() == "15")
        //    {
        //        FormConfirmDelete.ShowOnPageLoad = true;
        //    }
        //    else 
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Cannot Delete Fault ! ... ');", true);
        //        return;
        //    }



        //}

        //#endregion

        //#region GridDS

        ///// <summary>
        ///// Get Fault Record by History ID
        ///// </summary>
        ///// <param name="historyId"></param>
        ///// <returns></returns>
        //public static List<FaultItems> GetFaultRecord(int historyId)
        //{
        //    var listFault = new List<FaultItems>();

        //    SqlConnection con = new SqlConnection(ConString);
        //    SqlCommand cmd = new SqlCommand("usp_GetProductionFault", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlHelper.AddInParameter(cmd, "@ProductionHistoriesId", historyId);
        //    IDataReader datareader = null;

        //    try
        //    {
        //        con.Open();
        //        datareader = cmd.ExecuteReader();
        //        listFault = MappingFaultRecord(datareader);
        //    }
        //    catch (Exception e)
        //    {
        //        AppLogger.LogError(e);
        //    }
        //    finally
        //    {
        //        datareader.Close();
        //        datareader.Dispose();
        //        con.Close();
        //        con.Dispose();
        //    }

        //    return listFault;
        //}

        ///// <summary>
        ///// Update Table FaultRecord with model Fault Record
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static Boolean UpdateFaultRecord(FaultRecord model)
        //{
        //    using (AppDb context = new AppDb())
        //    using (DbContextTransaction transaction = context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var fault = context.FaultRecord.Where(f => f.FaultRecordId == model.FaultRecordId).FirstOrDefault();

        //            if (fault != null)
        //            {
        //                fault.CSType = model.CSType;
        //                fault.FaultStatus = model.FaultStatus == null ? fault.FaultStatus : model.FaultStatus;
        //                fault.Remarks = model.Remarks;
        //                fault.ModifiedDate = DateTime.Now;
        //                fault.Priority = model.Priority;
        //                fault.IsSentToRectification = model.IsSentToRectification == null ? fault.IsSentToRectification : model.IsSentToRectification;
        //                fault.IsPdi0 = model.IsPdi0 == null ? fault.IsPdi0 : model.IsPdi0;
        //                fault.FaultRelatedTypeId = model.FaultRelatedTypeId == 0 ? fault.FaultRelatedTypeId : model.FaultRelatedTypeId;
        //                fault.FaultDescriptionId = model.FaultDescriptionId == 0 ? fault.FaultDescriptionId : model.FaultDescriptionId;
        //                fault.FaultDescriptionText = model.FaultDescriptionText;
        //                fault.FaultPartProcessId = model.FaultPartProcessId == 0 ? fault.FaultPartProcessId : model.FaultPartProcessId;
        //                fault.StamperUserID = model.StamperUserID == 0 ? fault.StamperUserID : model.StamperUserID;
        //                fault.StamperName = model.StamperName;
        //                fault.QualityGateId = model.QualityGateId == 0 ? fault.QualityGateId : model.QualityGateId;
        //                fault.NCP = model.NCP;
        //                fault.InspectorUserId = model.InspectorUserId == 0 ? fault.InspectorUserId : model.InspectorUserId;
        //                fault.InspectorName = model.InspectorName;
        //                fault.RecordDate = model.RecordDate;

        //                var user = HttpContext.Current.Session["user"] as User;
        //                var userName = string.Empty;
        //                if (user != null)
        //                {
        //                    userName = user.UserName;
        //                }
        //                fault.ModifiedBy = userName;

        //                context.SaveChanges();
        //                transaction.Commit();

        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            AppLogger.LogError(ex.Message);
        //        }
        //    }

        //    return false;
        //}

        //public static IList RetrieveIrregAlterationBy(int StationId, int _history, string packingmonth)
        //{
        //    IList iaList;
        //    using (AppDb context = new AppDb())
        //    {

        //        var ModelId = (from a in context.ProductionHistories where a.Id == _history select a.ModelId).FirstOrDefault();
        //        var VariantId = (from a in context.ProductionHistories where a.Id == _history select a.VariantId).FirstOrDefault();

        //        int? IAmodelId = IrregAltRepository.RetrieveModelorAllModel(ModelId, packingmonth);
        //        int? IAVariantId = IrregAltRepository.RetrieveVariantorAllVariant(ModelId, VariantId, packingmonth.ToString());

        //        if (IAVariantId != null && IAmodelId != null)
        //            iaList =
        //            context.vw_IrregDocControl.Where(p => p.ModelId == IAmodelId && p.VariantId == IAVariantId && p.StationId == StationId).ToList();

        //        else if (IAVariantId != null && IAmodelId == null)
        //            iaList =
        //            context.vw_IrregDocControl.Where(p => p.ModelId == ModelId && p.VariantId == IAVariantId && p.StationId == StationId).ToList();

        //        else
        //            iaList =
        //                context.vw_IrregDocControl.Where(p => p.ModelId == IAmodelId && p.StationId == StationId).ToList();

        //    }
        //    return iaList;
        //}

        ///// <summary>
        ///// Validate current station need material from sub Assembly line to approve to next process
        ///// </summary>
        ///// <param name="stationId"></param>
        ///// <param name="modelId"></param>
        ///// <param name="variantId"></param>
        ///// <returns></returns>
        //public static List<SubAssyStatus> GetSubAssemblyStatus(int prodHistId, int stationId)
        //{
        //    List<SubAssyStatus> result = new List<SubAssyStatus>();

        //    ProductionHistory prod = GetProductionHistory(prodHistId);
        //    Station station = GetProductionStation(stationId);

        //    using (AppDb context = new AppDb())
        //    {
        //        var route = context.ProductionRoute.Where(x => x.ModelId == prod.ModelId && x.VariantId == prod.VariantId && x.LineId == prod.ProductionLineId).FirstOrDefault();
        //        if (route != null)
        //        {
        //            List<ProductionRoutePath> routePaths = new List<ProductionRoutePath>();
        //            if (station.Id == GetFirstStationInAssemblySection(station.AssemblySectionId))
        //            {
        //                routePaths = context.ProductionRoutePath.Where(x => x.ProductionRouteId == route.Id && (x.FeedOutStationId == station.Id || (x.FeedOutStationId == null && x.FeedOutAssemblySectionId == station.AssemblySectionId))).ToList();
        //            }
        //            else
        //            {
        //                routePaths = context.ProductionRoutePath.Where(x => x.ProductionRouteId == route.Id && (x.FeedOutStationId == station.Id)).ToList();
        //            }

        //            foreach (ProductionRoutePath path in routePaths)
        //            {
        //                Station feedInStation = context.Stations.Where(x => x.AssemblySectionId == path.AssemblySectionId).OrderByDescending(o => o.Sequence).FirstOrDefault();
        //                if (feedInStation == null) continue;

        //                AssemblySection feedInSection = context.AssemblySections.Where(x => x.Id == path.AssemblySectionId).FirstOrDefault();
        //                if (feedInSection == null || feedInSection.AssemblySectionType != (int)EAssemblySectionType.SubAssemblyLine) continue;

        //                SubAssyStatus subassy = new SubAssyStatus()
        //                {
        //                    Name = feedInSection.AssemblySectionName,
        //                    ConsumerStationName = station.StationName
        //                };

        //                ProductionHistoryDetail detail = context.ProductionHistoryDetails.Where(x => x.StationId == feedInStation.Id && x.ProductionHistoryId == prodHistId).OrderByDescending(x => x.StartTime).FirstOrDefault();
        //                if (detail == null)
        //                {
        //                    subassy.Status = "NotStarted";
        //                    subassy.StartTime = null;
        //                    subassy.EndTime = null;
        //                }
        //                else
        //                {
        //                    subassy.Status = ((enumProductionDetailsStatus)detail.StatusId).ToString();
        //                    subassy.StartTime = detail.StartTime;
        //                    subassy.EndTime = detail.EndTime;
        //                }

        //                result.Add(subassy);
        //            }

        //            return result;
        //        }
        //    }

        //    return result;
        //}

        //#endregion

    }

    class TemplatePreviewRow : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            GridViewPreviewRowTemplateContainer con = container as GridViewPreviewRowTemplateContainer;

            string txt = DataBinder.Eval(con.DataItem, "Torque").ToString();
            if (String.IsNullOrWhiteSpace(txt))
                return;

            ASPxLabel lbl = new ASPxLabel();
            lbl.ID = "lblPreview";
            lbl.Text = txt;
            con.Controls.Add(lbl);

            con.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp"));

            ASPxImage img = new ASPxImage();
            img.ID = "imgTools";
            img.ImageUrl = "/Content/Images/glyphicons-375-claw-hammer.png";
            img.Width = new Unit("16px");
            con.Controls.Add(img);

            con.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp"));

            ASPxLabel lbl2 = new ASPxLabel();
            lbl2.ID = "lblAssignTools";
            con.Controls.Add(lbl2);

            //string dialogAddr = DataBinder.Eval(con.DataItem, "DialogAddress").ToString();
            //string partNo = DataBinder.Eval(con.DataItem, "PartNumber").ToString();
            //string cgisNo = DataBinder.Eval(con.DataItem, "CgisNo").ToString();

            //string toolDesc = ToolRepository.GetAssignedToolToControlPlanProcess(dialogAddr, partNo, cgisNo);
            //if (toolDesc == "")
            //{
            //    img.Visible = false;
            //    lbl2.Visible = false;
            //}
            //else
            //{
            //    lbl2.Text = toolDesc;
            //}
        }
    }

    public class TemplateCpStationDetailDataItem : ITemplate
    {
        string name;
        PageBase parent;

        public TemplateCpStationDetailDataItem(PageBase Parent, string Name)
        {
            this.parent = Parent;
            this.name = Name;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            GridViewDataItemTemplateContainer template = container as GridViewDataItemTemplateContainer;
            ASPxGridView grid = template.Grid;

            if (name == "FBS")
            {
                ASPxLabel lbl = new ASPxLabel();
                lbl.ID = "lblFBS";
                lbl.Font.Bold = true;
                lbl.Font.Size = new FontUnit(FontSize.Medium);

                container.Controls.Add(lbl);
            }
            else if (name == "DRT")
            {
                ASPxLabel lbl = new ASPxLabel();
                lbl.ID = "lblDRT";
                lbl.Font.Bold = true;
                lbl.Font.Size = new FontUnit(FontSize.Medium);

                container.Controls.Add(lbl);
            }
            else if (name == "WI")
            {
                ASPxGridView grd = new ASPxGridView();
                grd.ID = "grvWIDocs";
                grd.KeyFieldName = "Id";
                grd.CssClass = "innerGrid";
                grd.Settings.ShowColumnHeaders = false;
                grd.Settings.GridLines = GridLines.None;

                GridViewDataColumn col = new GridViewDataColumn();
                col.FieldName = "Title";
                col.DataItemTemplate = new TemplateCpStationDetailDataItem(parent, "WIDoc");
                grd.Columns.Add(col);

                col = new GridViewDataColumn();
                col.FieldName = "RunningNumber";
                col.VisibleIndex = 0;
                col.Visible = false;
                grd.Columns.Add(col);

                grd.SettingsPager.Visible = false;
                grd.SettingsDataSecurity.AllowDelete = false;
                grd.SettingsDataSecurity.AllowEdit = false;
                grd.SettingsDataSecurity.AllowInsert = false;

                container.Controls.Add(grd);

                //< dx:ASPxGridView ID = "grvWIDocs" runat = "server" AutoGenerateColumns = "False" KeyFieldName = "Id" CssClass = "innerGrid" >
                //             < Settings ShowColumnHeaders = "False" GridLines = "None" ></ Settings >
                //                < Columns >
                //                    < dx:GridViewDataTextColumn VisibleIndex = "0" FieldName = "Title" >
                //                           < DataItemTemplate >
                //                               < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadWI" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadWI_OnClick" >
                //                </ dx:ASPxButton >
                //             </ DataItemTemplate >
                //         </ dx:GridViewDataTextColumn >
                //      </ Columns >
                //      < SettingsPager Visible = "False" >
                //       </ SettingsPager >
                //       < SettingsDataSecurity AllowDelete = "False" AllowEdit = "False" AllowInsert = "False" />
                //        </ dx:ASPxGridView >

            }
            else if (name == "II")
            {
                ASPxGridView grd = new ASPxGridView();
                grd.ID = "grvIIDocs";
                grd.KeyFieldName = "Id";
                grd.CssClass = "innerGrid";
                grd.Settings.ShowColumnHeaders = false;
                grd.Settings.GridLines = GridLines.None;

                GridViewDataColumn col = new GridViewDataColumn();
                col.FieldName = "Title";
                col.VisibleIndex = 0;
                col.DataItemTemplate = new TemplateCpStationDetailDataItem(parent, "IIDoc");
                grd.Columns.Add(col);

                col = new GridViewDataColumn();
                col.FieldName = "RunningNumber";
                col.VisibleIndex = 0;
                col.Visible = false;
                grd.Columns.Add(col);

                grd.SettingsPager.Visible = false;
                grd.SettingsDataSecurity.AllowDelete = false;
                grd.SettingsDataSecurity.AllowEdit = false;
                grd.SettingsDataSecurity.AllowInsert = false;

                container.Controls.Add(grd);

                //< dx:ASPxGridView ID = "grvIIDocs" runat = "server" AutoGenerateColumns = "False" CssClass = "innerGrid" >
                //           < Settings ShowColumnHeaders = "False" GridLines = "None" ></ Settings >
                //              < Columns >
                //                  < dx:GridViewDataTextColumn VisibleIndex = "0" >
                //                       < DataItemTemplate >
                //                           < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadII" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadII_OnClick" >
                //                </ dx:ASPxButton >
                //             </ DataItemC:\Workspaces\MBINA-LPS\Dotsystem Scaffold\NewTIDS\appmercy\DotMercy\DotWeb\UI\Template >
                //         </ dx:GridViewDataTextColumn >
                //      </ Columns >
                //      < SettingsPager Visible = "False" >
                //       </ SettingsPager >
                //       < SettingsDataSecurity AllowDelete = "False" AllowEdit = "False" AllowInsert = "False" />
                //        </ dx:ASPxGridView >
            }
            else if (name == "WIDoc")
            {
                //string text = HttpUtility.HtmlDecode(template.Text).Trim();
                string number = grid.GetRowValues(template.VisibleIndex, "DocID").ToString();
                int id = grid.GetRowValues(template.VisibleIndex, "Id").ToInt32(0);
                string title = grid.GetRowValues(template.VisibleIndex, "Title").ToString();

                ASPxButton btn = new ASPxButton();
                btn.ID = "btnDownloadWI";
                btn.RenderMode = ButtonRenderMode.Link;
                btn.Text = number;
                btn.CommandArgument = id.ToString();
                btn.ToolTip = title;
                btn.Click += btnDownloadWI_OnClick;

                //CPStationDetail page = parent as CPStationDetail;
                //if (page != null)
                //    btn.Click += page.btnDownloadWI_OnClick;

                container.Controls.Add(btn);

                //                               < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadWI" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadWI_OnClick" >
                //                </ dx:ASPxButton >

            }
            else if (name == "IIDoc")
            {
                //string text = HttpUtility.HtmlDecode(template.Text).Trim();
                string number = grid.GetRowValues(template.VisibleIndex, "DocID").ToString();
                int id = grid.GetRowValues(template.VisibleIndex, "Id").ToInt32(0);
                string title = grid.GetRowValues(template.VisibleIndex, "Title").ToString();

                ASPxButton btn = new ASPxButton();
                btn.ID = "btnDownloadII";
                btn.RenderMode = ButtonRenderMode.Link;
                btn.Text = number;
                btn.CommandArgument = id.ToString();
                btn.ToolTip = title;
                btn.Click += btnDownloadII_OnClick;

                //CPStationDetail page = parent as CPStationDetail;
                //if (page != null)
                //    btn.Click += page.btnDownloadII_OnClick;

                container.Controls.Add(btn);

                //                           < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadII" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadII_OnClick" >
                //                </ dx:ASPxButton >
            }
        }

        private void btnDownloadWI_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    int id = Convert.ToInt32(btn.CommandArgument);
            //    ControlPlanAttachmentWI type = RetriveControlPlanWIAttachmentId(id);
            //    if (type.FileType == "application/pdf")
            //    {
            //        parent.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "window.open('../CommonWorks/CustomDocumentViewer.aspx?Id=" + id + "&docType=CPWI" + "','_newtab');",
            //        true);
            //    }
            //    else
            //    {
            //        //Response.Redirect("./custom/DownloadFile.aspx?type=CPWI" + "&Id=" + id);
            //        parent.Response.Redirect("../DownloadFile.aspx?type=CPWI&Id=" + id);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }

        private void btnDownloadII_OnClick(object sender, EventArgs e)
        {
            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    int id = Convert.ToInt32(btn.CommandArgument);
            //    ControlPlanAttachmentII type = RetriveControlPlanIIAttachmentbyId(id);
            //    if (type.FileType == "application/pdf")
            //    {
            //        parent.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "window.open('../CommonWorks/CustomDocumentViewer.aspx?Id=" + id + "&docType=CPII" + "','_newtab');",
            //        true);
            //    }
            //    else
            //    {
            //        //Response.Redirect("./custom/DownloadFile.aspx?type=CPWI" + "&Id=" + id);
            //        parent.Response.Redirect("../DownloadFile.aspx?type=CPII&Id=" + id);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }

        //public static ControlPlanAttachmentWI RetriveControlPlanWIAttachmentId(int id)
        //{
        //    using (AppDb context = new AppDb())
        //    {
        //        var data = context.ControlPlanAttachments.Where(a => a.Id == id && a.DocTypeId == 1).FirstOrDefault();
        //        return data;
        //    }
        //}

        //public static ControlPlanAttachmentII RetriveControlPlanIIAttachmentbyId(int id)
        //{
        //    using (AppDb context = new AppDb())
        //    {
        //        var data = context.ControlPlanIIAttachments.Where(a => a.Id == id && a.DocTypeId == 2).FirstOrDefault();
        //        return data;
        //    }
        //}
    }

}