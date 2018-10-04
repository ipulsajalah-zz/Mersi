using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DotWeb.Models;
using DotWeb.Repositories;
using DotWeb.Utils;
using System.IO;
using System.Data;
using Ebiz.Scaffolding.Utils;

namespace DotMercy.custom.Production
{
    public partial class ProductionVoca : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        private const string urlImgDefaultCar = "~/content/images/production/car/default.png"; 
        private const string urlImgNotifGreen = "~/content/images/production/notification/green.gif";
        private const string urlImgNotifRed = "~/content/images/production/notification/red.gif";
        ProductionReleaseFaultModel ListAttachment = new ProductionReleaseFaultModel();
        List<DocumentAttachment> AttachFileModelList = new List<DocumentAttachment>();
        private static string _condition;
        private static int _stationId;
        private static int _historyId;
        private static string FinNumberEdit;
        private static int _requestType;
        private static int _sectId;
        private static int _lineId;
        private static bool _isNotNull;
        private static bool _isSubAssy;
        private static bool _isFirstStation;

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadAttachment.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE_BYTES;
            UploadAttachment.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            UploadAttachment.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");

            if (Request.QueryString.Count > 0)
            {
                _requestType = 0;
                _condition = Request.QueryString["Condition"].ToString();
                _stationId = Convert.ToInt32(Request.QueryString["Stat"]);
                _historyId = Convert.ToInt32(Request.QueryString["Hist"]);
                _sectId = 0;
                _isNotNull = _stationId != 0 && _historyId != 0 ? true : false;
            }
            else
            {
                Response.Redirect("../Production/ProductionDashboard.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["dtAttachment"] != null)
                {
                    GVDataAttachment.DataSource = Session["dtAttachment"];
                    GVDataAttachment.DataBind();
                }
                if (_stationId != 0) //if trigger from sub assy station
                {
                    _isSubAssy = ProductionRepository.IsSubAssemblyStation(_stationId);
                    _isFirstStation = ProductionRepository.IsFirstStation(_stationId, assemblyTypeId);
                }

                loadDetail();
            }
        }
        private void loadDetail()
        {
            if (_isNotNull == false)
                Response.Redirect("../Production/ProductionDashboard.aspx");

            var stations = ProductionRepository.GetStationCondition(_stationId, _historyId);

            if (stations != null)
            {
                //car image
                detailImg.ImageUrl = urlImgDefaultCar;
                detailImg.Enabled = false;
                detailImg.CssClass = "img-detail";

                //notification
                detailCondition.Enabled = false;
                detailCondition.CssClass = "img-notification";
                _lineId = stations.Current.LineId;
                if (_historyId == 0)
                    detailCondition.Visible = false;
                else
                    detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;

                //station info
                if (_condition == "isPA")
                {
                    lblStation.Text = "[PA]" + stations.Current.StationName + (_isSubAssy ? string.Empty : " [" + stations.Current.LineId + "]"); ;
                    BtnVoca.Visible = true;
                }
                else
                {

                    if (AppConfiguration.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
                    {
                        lblStation.Text = "[TearDown Audit]" + stations.Current.StationName + (_isSubAssy ? string.Empty : " [" + stations.Current.LineId + "]"); ;
                    }
                    else
                    {
                        lblStation.Text = "[VoCA]" + stations.Current.StationName + (_isSubAssy ? string.Empty : " [" + stations.Current.LineId + "]"); ;
                    }

                }
                lblVINNumber.Text = stations.Current.VINNumber;
                //lblFINNumber.Text = stations.Current.FINNumber;
                if (AppConfiguration.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
                {
                    lblFINNumber.Text = stations.EngineNo;
                }
                else
                {
                    lblFINNumber.Text = stations.Current.FINNumber;
                }
                lblModelVariant.Text = stations.Current.ModelName + " " + stations.Current.VariantName;

                if (AppConfiguration.DEFAULT_ASSEMBLYTYPEID != assemblyTypeId)
                {
                    lblEngineNo.Text = stations.Current.FINNumber; ;
                }
                else
                {
                    lblEngineNo.Text = stations.EngineNo;
                }
                lblCommnos.Text = stations.CommnosNo;
                lblPackingMonth.Text = stations.PackingMonth.ToString();
                lblFDoc.Text = stations.FDokProdNo;
                moOptionCode.Text = stations.OptionCodes;
                lblPaintCode.Text = stations.PaintCode.ToString();
                lblInteriorCode.Text = stations.InteriorCode.ToString();
                lblLocalProdNo.Text = stations.LocalProdNo;

                //binding fault
                gridFault.DataBind();
                GvDetailsFaultRecord.DataBind();




            }
        }

        #region headerButton

        //action ToFinalStation
        protected void BtnReleaseFinal_Click(object sender, EventArgs e)
        {
            //TODO: confirm whther to ask for authkey
            //var user = ProductionRepository.GetUserQM(tbLoginRecti.Text.Trim());
            //if (user == null)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
            //    tbLoginRecti.Text = "";
            //    return;
            //}
            //tbLoginRecti.Text = "";

            string userName = AppConfiguration.DEFAULT_USERNAME;
            //var user = Session["ProductionUser"] as User;
            //if (user != null)
            //{
            //    userName = user.UserName;
            //}

            ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
            if (prodHist == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
                return;
            }

            //Stations targetStation = ProductionRepository.GetProductionStation(IdcmbEndOffLine.SelectedItem.Value.ToInt32(0));
            //if (prodHist == null)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
            //    return;
            //}

            try
            {
                bool status = prodHist.FinishVocaPa(userName);
                if (!status)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Finish Line from VOCA/PA, please contact your administrator');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Finish Line from VOCA/PA: " + ex.Message + "');", true);
                AppLogger.LogError(ex.Message);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully " + BtnReleaseFinal.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
            //Response.Redirect("../Production/ProductionDashboard.aspx");

        }
        //action btnNextProcess
        //protected void btnNextProcess_Click(object sender, EventArgs e)
        //{
        //    int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToInt32(1);

        //    //getcurrentStaion and history
        //    int currentStationId = _stationId;
        //    int currentHistoryId = _historyId;

        //    try
        //    {
        //        //current history by station
        //        var currentProdItem = ProductionRepository.GetProductionItemByHistoryStation(currentStationId, currentHistoryId);

        //        //getnextposition            
        //        bool isOffline = currentProdItem.IsOffline;
        //        bool isRectify = currentProdItem.IsRectification;
        //        //bool isVoca = currentProdItem.IsVoca;
        //        //not sub assy
        //        if (!_isSubAssy)
        //        {
        //            //not end of line
        //            if (!ProductionRepository.IsEndOfLineStation(_stationId))
        //            {
        //                //check this station need assembly product or not.
        //                //TODO: this includes dependency between main line. should be excluded
        //                var needStationIds = ProductionRepository.CheckCurrentStationNeedMaterialFromSubAssembly(_stationId, currentProdItem.ModelId, currentProdItem.VariantId, _lineId);

        //                if (needStationIds.Any())
        //                {
        //                    foreach (var needStationId in needStationIds)
        //                    {
        //                        //check assembly for this station done or not
        //                        if (ProductionRepository.IsAssemblyCompleted(currentProdItem.ItemId, needStationId))
        //                        {
        //                            //update to historyMaterial quantity
        //                            ProductionRepository.UpdateQuantityInProductionMaterialHistory(currentProdItem.FINNumber, ProductionRepository.GetAssemblySection(needStationId).Id);
        //                        }
        //                        else
        //                        {
        //                            //notify to wait or assembly done.
        //                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Sub Assembly " +
        //                                ProductionRepository.GetAssemblySection(needStationId).AssemblySectionName + " for " + currentProdItem.FINNumber + " on this station is undone.');", true);
        //                            return;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //update process in this station
        //        if (ProductionRepository.UpdateProductionHistoryDetail(currentHistoryId, currentStationId, isOffline, isRectify, isOffline, isRectify, true))
        //        {
        //            //getnextstationid
        //            var nextStation = ProductionRepository.GetRouteNextStation(currentStationId, currentProdItem.ModelId, currentProdItem.VariantId, _lineId);

        //            if (nextStation == null)
        //            {
        //                //insert new
        //                if (0 != ProductionRepository.InsertProductionHistoryDetail(currentHistoryId, nextStation.Id, isOffline, isRectify, ""))
        //                {
        //                    if (_isSubAssy)//update station on material history
        //                        ProductionRepository.UpdateConsumerInProductionMaterialHistory(currentProdItem.FINNumber,
        //                            ProductionRepository.GetAssemblySection(currentStationId).Id, currentStationId, nextStation.Id);

        //                    //back to dashboard
        //                    if (_isSubAssy)
        //                        Response.Redirect("../Production/SubAssemblyStation.aspx?SectId=" + _sectId);
        //                    else
        //                        Response.Redirect("../Production/ProductionDashboard.aspx");
        //                }
        //                else
        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Create new history');", true);
        //            }
        //            else
        //            {
        //                if (_isSubAssy)
        //                    Response.Redirect("../Production/SubAssemblyStation.aspx?SectId=" + _sectId);
        //                else
        //                    Response.Redirect("../Production/ProductionDashboard.aspx");
        //            }
        //        }
        //        else
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Update Current History');", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        //alert process has move to next station
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
        //        AppLogger.LogError(ex.Message);
        //    }
        //}


        ////action btnMoveOffline
        //protected void btnMoveOffline_Click(object sender, EventArgs e)
        //{

        //    var _StationPick = IdcmbEndOffLine.SelectedItem.Value;
        //    var _StationPickName = IdcmbEndOffLine.SelectedItem.Text;
        //    var _Sequence = ProductionRepository.GetStationCode(_StationPick.ToString());
        //    //convert object to readable type
        //    var _stationId = Convert.ToInt32(Request.QueryString["Stat"]);
        //    var _historyId = Convert.ToInt32(Request.QueryString["Hist"]);
        //    //get history item
        //    var histItem = ProductionRepository.GetProductionItemByHistoryStation(_stationId, _historyId);


        //    var newStationId = ProductionRepository.GetFirstEndOfLineStation(assemblyTypeId);

        //    //insert new station detail to start eol
        //    if (0 != ProductionRepository.InsertProductionHistoryDetail(histItem.ItemId, newStationId,
        //         false, false, ""))
        //    {
        //        //update last un done history detail
        //        if (ProductionRepository.UpdateProductionHistoryDetail(histItem.ItemId, histItem.StationId,
        //            histItem.IsOffline, histItem.IsRectification, histItem.IsOffline, true, true))
        //        {
        //            ProductionRepository.UpdateAllSubAssyBySerialNumber(histItem.FINNumber);
        //            ProductionRepository.UpdateStationPick(_historyId.ToString(), _StationPick.ToString(), _StationPickName.ToString(), _Sequence.ToString());
        //            //todo success
        //            // LoadData();
        //            Response.Redirect("../Production/ProductionDashboard.aspx");
        //        }
        //        else
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot update last history details.');", true);
        //    }
        //    else
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot send "
        //            + histItem.FINNumber + " to End of Line" + ".');", true);



        //    ////if station is last station of end of line
        //    //if (ProductionRepository.CheckLastEndofLineStation(_stationId))
        //    //{
        //    //    //getcurrentStaion and history
        //    //    int currentStationId = _stationId;
        //    //    int currentHistoryId = _historyId;

        //    //    try
        //    //    {
        //    //        //update process in this station
        //    //        if (ProductionRepository.UpdateProductionHistoryVOCA(currentHistoryId, true))
        //    //        {
        //    //            //back to dashboard
        //    //            Response.Redirect("../Production/ProductionDashboard.aspx");
        //    //        }
        //    //        else
        //    //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Update Current History');", true);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        //alert process has move to next station
        //    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
        //    //        AppLogger.LogError(ex.Message);
        //    //    }
        //    //}
        //    //else
        //    //    ToOfflineRecti(true);
        //}

        //set Station to Offline & Rectification
        private void ToOfflineRecti(bool updateOffline)
        {
            //getcurrentStaion and history
            int currentStationId = _stationId;
            int currentHistoryId = _historyId;

            try
            {
                //current history
                var currentProdItem = ProductionRepository.GetProductionItemByHistoryStation(currentStationId, currentHistoryId);

                //getnextstatus            
                int statusId = currentProdItem.StatusId;

                //getnextposition            
                bool oldIsOffline = currentProdItem.IsOffline;
                bool oldIsRectify = currentProdItem.IsRectification;
                bool newIsOffline = updateOffline ? true : currentProdItem.IsOffline;
                bool newIsRectify = updateOffline ? currentProdItem.IsRectification : true;

                //update process in this station
                if (ProductionRepository.UpdateProductionHistoryDetail(currentHistoryId, currentStationId, oldIsOffline, oldIsRectify, newIsOffline, newIsRectify, false))
                {
                    //back to dashboard
                    Response.Redirect("../Production/ProductionDashboard.aspx");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Update Current History');", true);
            }
            catch (Exception ex)
            {
                //alert process has move to next station
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
                AppLogger.LogError(ex.Message);
            }
        }

        //action btnFault
        protected void btnFault_Click(object sender, EventArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = true;
            loadFaultDefault();
        }

        ////action btnRelease
        //protected void btnRelease_Click(object sender, EventArgs e)
        //{
        //    //get productionItem
        //    var prodItem = ProductionRepository.GetProductionItemByHistoryStation(_stationId, _historyId);
        //    try
        //    {
        //        //can it be cancel / remove?
        //        if (_isFirstStation)
        //        {
        //            if (ProductionRepository.CheckHasBeenExistOnOtherStation(_historyId, assemblyTypeId))
        //                //Vehicle has story in other station. This is not the first time of this vehicle 
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('This vehicle exist in another station, you can not remove it');", true);
        //            else
        //                if (ProductionRepository.UpdateProductionHistoryCancel(_historyId)) //remove / cancel vehicle
        //                    //done delete
        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Delete or remove vehicle successfully!');window.location ='../Production/ProductionDashboard.aspx'", true);
        //                else
        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Can not delete or remove vehicle, please contact your administrator.');", true);
        //        }
        //        else
        //        {
        //            //check if this car still have fault to solve
        //            if (FaultRepository.IsHaveFault(_historyId))
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('There are still unfinished fault, please solve this first.');", true);
        //            else
        //                // insert new inventory
        //                if (ProductionRepository.InsertProductionInventory(_historyId, Session["assemblyType"].ToInt32(0)) > 0)
        //                {
        //                    //update this last item production history detail
        //                    if (ProductionRepository.UpdateProductionHistoryDetail(_historyId, _stationId,
        //                        prodItem.IsOffline, prodItem.IsRectification, prodItem.IsOffline, prodItem.IsRectification, true))
        //                    {
        //                        //update production history status to complete
        //                        if (ProductionRepository.UpdateProductionHistoryComplete(_historyId))
        //                        {
        //                            //update all existing assembly for this part
        //                            ProductionRepository.UpdateAllSubAssyBySerialNumber(prodItem.FINNumber);
        //                            //back to dashboard
        //                            Response.Redirect("../Production/ProductionDashboard.aspx");
        //                        }
        //                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Update Production History, please contact your administrator');", true);
        //                    }
        //                    else
        //                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Update Production History Details, please contact your administrator');", true);
        //                }
        //                else
        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Insert New Inventory, please contact your administrator');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppLogger.LogError(ex.Message);
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
        //    }
        //}
        #endregion

        #region grid
        //binding gridControlPlan
        protected void cpPerStationGrid_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView cpPerStationGridView = (ASPxGridView)sender;
            var controlplan = ProductionRepository.GetProductionProcessByStationHistory(_stationId, _historyId);
            cpPerStationGridView.DataSource = controlplan;

            cpPerStationGridView.HtmlRowPrepared += CpPerStationGridView_HtmlRowPrepared;
        }

        //binding gridFault
        protected void gridFault_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView faultGridView = (ASPxGridView)sender;
            var faults = FaultRepository.GetFaultRecord(_historyId);
            faultGridView.DataSource = faults;

            (faultGridView.Columns["FaultStatus"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = Enum.GetValues(typeof(EFaultStatus));
        }

        protected void gridFault_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var model = new FaultRecord();
            model.FaultRecordId = (int)e.Keys[0];
            model.FaultStatus = e.NewValues["FaultStatus"].ToString() == EFaultStatus.Open.ToString() ?
                (int)EFaultStatus.Open : (int)EFaultStatus.Close;
            model.Remarks = (string)e.NewValues["Remarks"];

            FaultRepository.UpdateFaultRecord(model);

            ASPxGridView senderGridView = (ASPxGridView)sender;
            e.Cancel = true;
            senderGridView.CancelEdit();
            senderGridView.DataSource = FaultRepository.GetFaultRecord(_historyId);
        }

        //binding gridIrregular
        protected void gridIrregular_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView irregularGridView = (ASPxGridView)sender;
            var irregular = string.Empty;
            irregularGridView.DataSource = irregular;
        }

        //set prepared gridControlPlan
        private static void CpPerStationGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.VisibleIndex >= 0)
                {
                    if (e.RowType == DevExpress.Web.GridViewRowType.Data)
                        e.Row.Height = Unit.Pixel(30);

                    ASPxGridView grv = (sender as ASPxGridView);
                    ASPxLabel lblVDoc = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblVDoc");
                    var stations = ProductionRepository.GetStationCondition(_stationId, _historyId);

                    if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "VDoc")) == true)
                    {
                        lblVDoc.Visible = true;
                        lblVDoc.Text = "X";
                    }

                    ASPxLabel lbl2Hand = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lbl2Hand");

                    if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "SecondHand")) == true)
                    {
                        lbl2Hand.Visible = true;
                        lbl2Hand.Text = "X";
                    }

                    ASPxLabel lblDS = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblDS");

                    if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "DS")) == true)
                    {
                        lblDS.Visible = true;
                        lblDS.Text = "X";
                    }

                    ASPxLabel lblCS2 = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblCS2");

                    if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "CS2")) == true)
                    {
                        lblCS2.Visible = true;
                        lblCS2.Text = "X";
                    }

                    ASPxLabel lblCS3 = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblCS3");

                    if (Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "CS3")) == true)
                    {
                        lblCS3.Visible = true;
                        lblCS3.Text = "X";
                    }

                    //gridview di WI/II
                    string processNo = grv.GetRowValues(e.VisibleIndex, "CgisNo").ToString();
                    ASPxGridView grvWIDocs = (ASPxGridView)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "grvWIDocs");
                    int countAttWI = ControlPlanRepository.CountAttachment("CPWI", processNo);

                    if (countAttWI > 0)
                    {
                        grvWIDocs.Visible = true;
                        grvWIDocs.DataSource = ControlPlanRepository.RetrieveControlPlanAttachmentByProcessAndDocType(processNo, "CPWI", stations.Current.ModelId);
                        grvWIDocs.DataBind();
                    }
                    else
                    {
                        grvWIDocs.Visible = false;
                    }

                    ASPxGridView grvIIDocs = (ASPxGridView)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "grvIIDocs");
                    int countAttII = ControlPlanRepository.CountAttachment("CPII", processNo);

                    if (countAttII > 0)
                    {
                        grvIIDocs.Visible = true;
                        grvIIDocs.DataSource =
                            ControlPlanRepository.RetrieveControlPlanAttachmentByProcessAndDocType(processNo, "CPII", stations.Current.ModelId);
                        grvIIDocs.DataBind();
                    }
                    else
                    {
                        grvIIDocs.Visible = false;
                    }

                    ASPxLabel lblDialog = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblDialog");
                    if (grv.GetRowValues(e.VisibleIndex, "DialogAddress") != null)
                    {
                        lblDialog.Text = grv.GetRowValues(e.VisibleIndex, "DialogAddress").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }

        //action btnDownloadWII
        protected void btnDownloadWI_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                Response.Redirect("../DownloadFile.aspx?type=CPWI&Id=" + btn.CommandArgument);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }

        //action btnDownloadII
        protected void btnDownloadII_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                Response.Redirect("../DownloadFile.aspx?type=CPII&Id=" + btn.CommandArgument);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }

        //binding btnCGIS
        protected void btnCgisNo_DataBinding(object sender, EventArgs e)
        {
            ASPxButton btn = (sender as ASPxButton);
            var command = "function (s, e) {window.open('../ControlPlans/CpCgisSheet.aspx?cpProcessId=" + btn.CommandArgument + "', '_blank')}";
            btn.ClientSideEvents.Click = command;
        }
        #endregion

        #region fault
        //action btnSave&New
        protected void btnRecordNew_Click(object sender, EventArgs e)
        {
            var fault = faultRecord();

            ////is exist in fault record
            //if (FaultRepository.IsFaultSame(fault.FaultRecordModels.ProductionHistoriesId, fault.FaultRecordModels.CGISId))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Check Again, Fault is already in database !!!');", true);
            //}
            //else
            //{
            //add to fault record
            FaultRepository.InsertFaultRecord(fault.FaultRecordModels);
            //}
            loadFaultDefault();
        }

        //action btnSave&Close
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            var fault = faultRecord();

            //is exist in fault record

            loadFaultDefault();

            if (_condition == "isPA")
            {
                //for (int x = 0; x < fault.AttachmentModels.Count(); x++)
                //{
                //    var id = cbFINNumber.Text.Split('/')[1].Trim();
                //    FaultRepository.InsertAttachMent(fault.AttachmentModels[x].FileName, fault.AttachmentModels[x].FilePath, id);
                //}
                //add to fault record
                FaultRepository.InsertFaultRecordReleaseCS3(fault.FaultRecordModels);
                //close popUpControl Fault
                pcInteruptFault.ShowOnPageLoad = false;
                //binding again grid Fault
                gridFault.DataBind();
                //add to fault record

                //close popUpControl Fault
                pcInteruptFault.ShowOnPageLoad = false;
                //binding again grid Fault
                gridFault.DataBind();
                detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
            }
            else
            {
                //for (int x = 0; x < fault.AttachmentModels.Count(); x++)
                //{
                //    var id = cbFINNumber.Text.Split('/')[1].Trim();
                //    FaultRepository.InsertAttachMent(fault.AttachmentModels[x].FileName, fault.AttachmentModels[x].FilePath, id);
                //}
                //add to fault record
                FaultRepository.InsertFaultRecordRelease(fault.FaultRecordModels);
                //close popUpControl Fault
                pcInteruptFault.ShowOnPageLoad = false;
                //binding again grid Fault
                gridFault.DataBind();
                detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = false;
            gridFault.DataBind();
            detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
        }

        protected void pcInteruptFault_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            //loadFaultDefault();
            pcInteruptFault.ShowOnPageLoad = false;
            pcInteruptFault.ShowOnPageLoad = true;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
            PopUpAttachMent.ShowOnPageLoad = false;
        }

        //binding FaultRecord
        public ProductionReleaseFaultModel faultRecord()
        {
            int csType = 0;
            if (_condition == "isPA")
            {
                csType = 3;
            }
            else
            {
                csType = 4;
            }

            var ncp = tbNCP.Text;
            int productionHistoriesId = Convert.ToInt32(cbFINNumber.Value);
            var faultNumber = "32054";
            int QGate = Convert.ToInt32(cbQG.Value);
            int station = Convert.ToInt32(cbStation.Value);
            ListEditItem selectedItem = cbFINNumber.SelectedItem;
            var finnumber = selectedItem.GetValue("FINNumber").ToString();
            Session["FINNumber"] = finnumber;
            var tempAssemblySection = ProductionRepository.GetAssemblySection(station);
            int assemblySectionId = tempAssemblySection.Id;

            var tempProductionLine = ProductionRepository.GetLineByProductionId(productionHistoriesId);
            int productionLineId = tempProductionLine.Id;

            var cgisId = Convert.ToInt32(cbCGIS.Value);
            int priority = Convert.ToInt32(cbPriority.Value);
            int partProces = Convert.ToInt32(cbPartProces.Value);
            int faultRelated = Convert.ToInt32(cbFaultRelated.Value);
            int faultDescId;
            if (int.TryParse(cbFaultDesc.Value.ToString(), out faultDescId))
            {
                faultDescId = Convert.ToInt32(cbFaultDesc.Value);
            }
            else
            {
                faultDescId = ProductionRepository.InputNonStandard(cbFaultDesc.Value.ToString());
            }
            var faultDescText = cbFaultDesc.Text;
            int inspectorId = Convert.ToInt32(cbInspector.Value);
            var inspectorName = cbInspector.SelectedItem;
            int stampId = Convert.ToInt32(cbStamp.Value);
            var stampName = cbStamp.SelectedItem;
            var remark = tbRemark.Text;
            bool recti = Convert.ToBoolean(cbRecti.Value.ToInt32());

            FaultRecord fault = new FaultRecord()
            {
                CSType = csType,
                NCP = ncp,
                ProductionHistoriesId = productionHistoriesId,
                FaultNumber = faultNumber,
                QualityGateId = QGate,
                AssemblySectionId = assemblySectionId,
                ProductionLinesId = productionLineId,
                StationId = station,
                FINNumber = finnumber.ToString(),
                CGISId = cgisId,
                Priority = priority,
                FaultPartProcessId = partProces,
                FaultRelatedTypeId = faultRelated,
                FaultDescriptionId = faultDescId,
                FaultDescriptionText = faultDescText,
                InspectorUserId = inspectorId,
                InspectorName = inspectorName.ToString(),
                StamperUserID = stampId,
                StamperName = stampName.ToString(),
                Remarks = remark,
                IsSentToRectification = recti
            };
            var SumRowData = GVDataAttachment.VisibleRowCount;
            List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
            for (int i = 0; i < SumRowData; i++)
            {
                DocumentAttachment AttachFile = new DocumentAttachment();
                AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "Url").ToString();
                ListAttachFile.Add(AttachFile);
            }
            ProductionReleaseFaultModel InsertFaultRecord = new ProductionReleaseFaultModel();
            {
                InsertFaultRecord.FaultRecordModels = fault;
                InsertFaultRecord.AttachmentModels = ListAttachFile;
            };
            return InsertFaultRecord;
        }

        //binding & set default form Fault
        private void loadFaultDefault()
        {
            currentdate.Date = DateTime.Now;

            cbStation.SelectedIndex = 0;
            cbQG.SelectedIndex = 0;
            cbFINNumber.Value = _historyId;
            FinNumberEdit = cbFINNumber.SelectedItem.Text;
            cbPartProces.SelectedIndex = 0;
            cbFaultDesc.SelectedIndex = 0;
            cbFaultRelated.SelectedIndex = 0;
            cbPriority.SelectedIndex = 0;
            cbInspector.SelectedIndex = 0;
            cbStamp.SelectedIndex = 0;
            cbRecti.SelectedIndex = 0;
            tbNCP.Text = "";
            tbRemark.Text = "";

            //set TextBox Line
            string line = GetLine(Convert.ToInt32(cbFINNumber.Value));
            tbLine.Text = line;

            //set textBox Assembly Section
            string section = GetSection(Convert.ToInt32(cbStation.Value));
            tbAssemblySection.Text = section;

            //set textBox Fault Classification
            string classification = GetClassification(Convert.ToInt32(cbFaultDesc.Value));
            tbClassification.Text = classification;

            //set comboBox CGIS
            Session["StationId"] = cbStation.Value;
            Session["HistoryId"] = cbFINNumber.Value;
            cbCGIS.DataSource = sdsCgis;
            cbCGIS.DataBind();
            cbCGIS.SelectedIndex = 0;
            if (_condition == "isPA")
            {
                CSTypeValue.Text = "3";
            }
            else
            {
                CSTypeValue.Text = "4";
            }


        }

        //callback comboBox CGIS from comboBox StationId & HistoryId
        protected void cbCGIS_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["StationId"] = cbStation.Value;
            Session["HistoryId"] = cbFINNumber.Value;
            cbCGIS.DataSource = sdsCgis;
            cbCGIS.DataBind();
            cbCGIS.SelectedIndex = 0;
        }

        [WebMethod]
        //get Line by HistoryId
        public static string GetLine(int values)
        {
            var tempLine = ProductionRepository.GetLineByProductionId(values);
            return tempLine.LineName;
        }

        [WebMethod]
        //get AssemblySection by StationId
        public static string GetSection(int values)
        {
            var tempSection = StationRepository.RetrieveAsemblySectionByStId(values);
            return tempSection;
        }

        [WebMethod]
        //get Fault Classification by DescriptionId
        public static string GetClassification(int values)
        {
            var tempClassification = FaultRepository.GetFaultClassification(values);
            return tempClassification.Description;
        }
        #endregion
        //protected void btnMoveRecti_Click(object sender, EventArgs e)
        //{
        //    ToOfflineRecti(false);
        //}
        //protected void BtnEndOffLine_Click(object sender, EventArgs e)
        //{
        //    PopUpEndOffLine.ShowOnPageLoad = true;
        //}

        //protected void btnSendToTrim_Click(object sender, EventArgs e)
        //{
        //    int currentHistoryId = _historyId;

        //    //TODO: Do we need to prompt for AuthKey?

        //    var newStationId = _condition == "isRecti" ? ProductionRepository.GetFirstEndOfLineStation(assemblyTypeId) : ProductionRepository.GetFirstStation(assemblyTypeId);

        //    var histItem = ProductionRepository.GetProductionItemByHistoryStation(_stationId, _historyId);

        //    //insert new station detail to start eol
        //    if (0 != ProductionRepository.InsertProductionHistoryDetail(histItem.ItemId, newStationId,
        //             _condition == "isRecti" ? histItem.IsOffline : false, _condition == "isRecti" ? false : histItem.IsRectification, ""))
        //    {
        //        if (ProductionRepository.UpdateProductionReworksOut(currentHistoryId, "VOCA", DateTime.Now, DateTime.Now))
        //        //back to dashboard
        //        {
        //            if (ProductionRepository.UpdateProductionHistoryToTrim(_historyId))
        //            {
        //                //update last un done history detail
        //                if (ProductionRepository.UpdateProductionHistoryDetail(histItem.ItemId, histItem.StationId,
        //                    histItem.IsOffline, histItem.IsRectification, histItem.IsOffline, true, true))
        //                {


        //                        if (_condition == "isRecti")
        //                            ProductionRepository.UpdateAllSubAssyBySerialNumber(histItem.FINNumber);
        //                        //todo success
        //                        Response.Redirect("../Production/ProductionDashboard.aspx");

        //                }

        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot update last history details.');", true);
        //                }
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot update last history details.');", true);
        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot update last history details Rework');", true);
        //        }
        //    }
        //    else
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot send "
        //            + histItem.FINNumber + " to " + _condition == "isRecti" ? "End of Line" : "Trimming Line" + ".');", true);
        //}
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Production/ProductionDashboard.aspx");
        }
        protected void btnSaveCloseEdit_Click(object sender, EventArgs e)
        {
            var model = new FaultRecord();
            model.FaultRecordId = int.Parse(HiddenText["IdFault"].ToString());
            model.FaultStatus = FaultStatusx.Text == "" ? 0 : int.Parse(FaultStatusx.SelectedItem.Value.ToString());
            model.Remarks = tbRemarkx.ToString();

            FaultRepository.UpdateFaultRecord(model);


            pcInteruptFaultx.ShowOnPageLoad = false;
            //pcLogin2.ShowOnPageLoad = false;

            gridFault.DataBind();
            // detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;

            detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;
        }
        private void loadFaultDefaultEdit(int id)
        {
            HiddenText["IdFault"] = id;
            var DataLoad = FaultRepository.GetFaultRecordById(id);
            currentdatex.Date = DateTime.Now;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
            CSTypeValuex.Text = DataLoad[0].CSType.ToString();
            cbStationx.Text = DataLoad[0].StationName.ToString();
            cbStationx.ReadOnly = true;
            cbStamp.Text = DataLoad[0].StamperName;
            tbRemark.Text = DataLoad[0].Remarks;
            cbQGx.Text = DataLoad[0].QualityGateName.ToString();
            cbQGx.ReadOnly = true;
            cbFINNumberx.Text = DataLoad[0].FINNumber.ToString();
            cbFINNumberx.ReadOnly = true;

            cbPartProcesx.Text = DataLoad[0].FaultPartProcessDesc.ToString();
            cbFaultDescx.Text = DataLoad[0].FaultDescriptionText;
            cbFaultRelatedx.Text = DataLoad[0].FaultRelatedTypeDesc;
            cbPriorityx.Text = DataLoad[0].Priority.ToString();
            cbInspectorx.Text = DataLoad[0].InspectorName;
            cbStamp.Text = DataLoad[0].StamperName;
            cbRectix.Text = DataLoad[0].IsSentToRectification.ToString();
            tbNCPx.Text = DataLoad[0].NCP;
            tbRemark.Text = DataLoad[0].Remarks;

            //set TextBox Line
            //string line = GetLine(Convert.ToInt32(cbFINNumber.Value));
            tbLinex.Text = DataLoad[0].ProductionLineName;

            //set textBox Assembly Section
            //string section = GetSection(Convert.ToInt32(cbStation.Value));
            tbAssemblySectionx.Text = DataLoad[0].AssemblySectionName;

            //set textBox Fault Classification
            //string classification = GetClassification(Convert.ToInt32(cbFaultDesc.Value));
            tbClassificationx.Text = DataLoad[0].FaultClassDescription;

            //set comboBox CGIS
            cbCGISx.Text = DataLoad[0].CGISNo;
            HiddenText["FINNumber"] = cbFINNumberx.Text == "1" ? "-Select-" : cbFINNumberx.Text;
            if (cbFINNumberx.Text == "1")
            {
                cbFINNumberx.Text = "-Select-";
            }
            else
            {
                cbFINNumberx.Text = DataLoad[0].FINNumber;
            }
            var faults = FaultRepository.GetAttachMentView(DataLoad[0].FINNumber.ToString());
            GVDataAttachmentx.DataSource = faults;
            GVDataAttachmentx.DataBind();
        }
        protected void Btn_Edit_Click(object sender, EventArgs e)
        {
            pcInteruptFaultx.ShowOnPageLoad = true;
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            try
            {
                int ValueId = gridFault.GetRowValues(index, "FaultRecordId") == null ? 0 : int.Parse(gridFault.GetRowValues(index, "FaultRecordId").ToString());
                if (ValueId != 0)
                {
                    loadFaultDefaultEdit(ValueId);
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }

        }
        protected void btnClosex_Click(object sender, EventArgs e)
        {
            pcInteruptFaultx.ShowOnPageLoad = false;
            //pcLogin2.ShowOnPageLoad = false;

            gridFault.DataBind();
            // detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;

            detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;

        }
        protected void ShowAttachment_Click(object sender, EventArgs e)
        {
            if (cbFINNumber.SelectedItem != null)
            {
                PopUpAttachMent.ShowOnPageLoad = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Fin Number !');", true);
            }
        }

        protected void uplDocumentWI_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            string HiddenCstype = "";
            if (_condition == "isPA")
            {
                HiddenCstype = "3";
            }
            else
            {
                HiddenCstype = "4";
            }

            string x = FinNumberEdit.Split('/')[1].Trim();

            Session["CSType"] = HiddenCstype;
            string PathName = AppConfiguration.UPLOAD_DIR + "/VPCReleaseFault/" + x;
            if (!Directory.Exists(Server.MapPath(PathName)))
            {
                Directory.CreateDirectory(Server.MapPath(PathName));
            }
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            //string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileName = e.UploadedFile.FileName;
            string resultFileUrl = PathName + "/" + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);

            // UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

            string name = e.UploadedFile.FileName;
            string url = ResolveClientUrl(resultFileUrl);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;
            DocumentAttachment AttachFile = new DocumentAttachment();
            AttachFile.Id = GetIdAttachment();
            AttachFile.FileName = resultFileName;
            AttachFile.FilePath = resultFileUrl;
            AttachFile.FileType = resultExtension;
            AttachFileModelList.Add(AttachFile);
            ListAttachment.AttachmentModels = AttachFileModelList;
            DataTable table = ConvertListToDataTable(AttachFileModelList);
            Session["dtAttachment"] = table;
            //cbFINNumber.Text = HiddenText["FINNumber"].ToString();

        }
        protected int GetIdAttachment()
        {
            int IdData = 0;
            var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
            IdData = Id;
            return IdData;
        }
        static DataTable ConvertListToDataTable(List<DocumentAttachment> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = list.Count();


            // Add columns.

            table.Columns.Add("Id");
            table.Columns.Add("FileName");
            table.Columns.Add("Url");


            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array.Id, array.FileName, array.FilePath);
            }

            return table;
        }
        protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

            pcInteruptFault.ShowOnPageLoad = false;
            pcInteruptFault.ShowOnPageLoad = true;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
        }
        protected void pcInteruptFault_DataBinding(object sender, EventArgs e)
        {
            if (Session["FINNumber"] != null)
            {
                cbFINNumber.Text = HiddenText["FINNumber"].ToString();
            }
            pcInteruptFault.ShowOnPageLoad = false;
            pcInteruptFault.ShowOnPageLoad = true;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
        }

        protected void BtnVoca_Click(object sender, EventArgs e)
        {
            ASPxButton btn = (sender as ASPxButton);

            var conditionalBtn = btn.CommandArgument;

            var userauthkey = ProductionRepository.CheckAuthOrg(tbLogin.Text);

            // authkey


            tbLogin.Text = "";

            if (userauthkey == "" || userauthkey == null)
            {

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
                return;
            }

            //getcurrentStaion and history
            int currentStationId = _stationId;
            int currentHistoryId = _historyId;

            try
            {
                //update process in this station
                if (ProductionRepository.UpdateProductionHistoryVOCA(currentHistoryId, true))
                {
                    if (ProductionRepository.InsertProductionReworksIn(currentHistoryId, "VOCA", DateTime.Now, DateTime.Now))
                    //back to dashboard
                    {
                        //back to dashboard
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully " + BtnVoca.Text + " !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
                        //Response.Redirect("../Production/ProductionDashboard.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Update Current History Reworks');", true);
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Cannot Update Current History');", true);
            }
            catch (Exception ex)
            {
                //alert process has move to next station
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to Next Station, please contact your administrator');", true);
                AppLogger.LogError(ex.Message);
            }

        }

        protected void GvDetailsFaultRecord_DataBinding(object sender, EventArgs e)
        {
            int Cstype = 0;
            ASPxGridView faultDetailGridView = (ASPxGridView)sender;
            if (_condition == "isPA")
            {
                Cstype = 3;
            }
            else
            {
                Cstype = 4;
            }
            var faults = FaultRepository.RetrieveGridDataFaultVoca(lblFINNumber.Text, "Usp_FaultAuditGetMainGv", Cstype);
            faultDetailGridView.DataSource = faults;

            // (faultDetailGridView.Columns["FaultStatus"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = Enum.GetValues(typeof(enumFaultStatus));
        }

        protected void GvDetailsFaultRecord_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //ASPxGridView faultDetailGridView = (ASPxGridView)sender;
            //var faults = FaultRepository.RetrieveGridDataFaultVoca(lblFINNumber.Text, "Usp_FaultProductionGetgvFaultvoca");
            //faultDetailGridView.DataSource = faults;
            int Cstype = 0;
            if (_condition == "isPA")
            {
                Cstype = 4;
            }
            else
            {
                Cstype = 3;
            }
            ASPxGridView senderGridView = (ASPxGridView)sender;
            e.Cancel = true;
            senderGridView.CancelEdit();
            // senderGridView.DataSource = FaultRepository.RetrieveGridDataFaultVoca(lblFINNumber.Text, "Usp_FaultProductionGetgvFaultvoca", Cstype);
            senderGridView.DataSource = FaultRepository.RetrieveGridDataFaultVoca(lblFINNumber.Text, "Usp_FaultAuditGetMainGv", Cstype);
        }


        protected void AddVocaPa_Click(object sender, EventArgs e)
        {
            var Id = FaultRepository.GetIdProductionHistories(lblFINNumber.Text);
            var cs = FaultRepository.GetIdVocaPA(lblFINNumber.Text);
            Response.Redirect("../Production/FaultRecordMain.aspx?FinNumber=" + lblFINNumber.Text + "&Id=" + Id + "&IdVoca=" + cs);
        }

        protected void GvMainFaultRecord_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            try
            {
                int Cstype = 0;
                int Index = e.VisibleIndex;
                Session["IndexDetails"] = Index;
                Session["IdFault"] = int.Parse(GvDetailsFaultRecord.GetRowValues(Index, "Id").ToString());
                Session["FinNumber"] = GvDetailsFaultRecord.GetRowValues(Index, "FINNumber").ToString();
                // Find Row Expanded
                ASPxGridView Data = (ASPxGridView)sender;
                var colapse = Data.DetailRows.VisibleCount;
                if (colapse == 1)
                {
                    var rowIndex = e.VisibleIndex;
                    Session["RowIndex"] = e.VisibleIndex;
                    var GvData = GvDetailsFaultRecord.FindDetailRowTemplateControl(rowIndex, "GvDetailsFaultRecord") as ASPxGridView;
                    int ValueId = int.Parse(GvDetailsFaultRecord.GetRowValues(Index, "Id").ToString());
                    Session["RowExpanddedId"] = ValueId;
                    if (_condition == "isPA")
                    {
                        Cstype = 4;
                    }
                    else
                    {
                        Cstype = 3;
                    }
                    GvData.DataSource = ProductionFaultAuditRepository.RetrieveGridDetailData(ValueId, "Usp_FaultAuditProductionGetGVDetail");
                    GvData.DataBind();
                    Session["GvDetailsBind"] = ProductionFaultAuditRepository.RetrieveGridDetailData(ValueId, "Usp_FaultAuditProductionGetGVDetail"); ;
                }

            }

            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;

            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            var id = int.Parse(GvDetailsFaultRecord.GetRowValues(index, "Id").ToString());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
                "window.open('/custom/Report/ReportVoCaPa.aspx?Id=" + id + "','_newtab');",
                true);
        }


    }
}