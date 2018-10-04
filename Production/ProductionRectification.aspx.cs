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
using System.Drawing;
//using DevExpress.XtraGauges.Core.Drawing;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.Models;

namespace DotMercy.custom.Production
{
    public partial class ProductionRectification : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        private const string urlImgDefaultCar = "~/content/images/production/car/default.png"; 
        private const string urlImgNotifGreen = "~/content/images/production/notification/green.gif";
        private const string urlImgNotifRed = "~/content/images/production/notification/red.gif";
        List<ProductionFaultReleaseAttachmentModel> AttachFileModelList = new List<ProductionFaultReleaseAttachmentModel>();
        ProductionReleaseFaultModel ListAttachment = new ProductionReleaseFaultModel();
        private static int _stationId;
        private static int _historyId;
        private static int _requestType;
        private static int _sectId;
        private static bool _isNotNull;
        private static bool _isSubAssy;
        private static bool _isFirstStation;
        private static int _lineId;

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadAttachment.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE_BYTES;
            UploadAttachment.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            UploadAttachment.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");

            if (Request.QueryString.Count > 0)
            {
                _requestType = 0;

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

            _lineId = stations.Current.LineId;
            if (stations != null)
            {
                //car image
                detailImg.ImageUrl = urlImgDefaultCar;
                detailImg.Enabled = false;
                detailImg.CssClass = "img-detail";

                //notification
                detailCondition.Enabled = false;
                detailCondition.CssClass = "img-notification";

                if (_historyId == 0)
                    detailCondition.Visible = false;
                else
                    detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;

                //station info
                lblStation.Text = "[RECTIFICATION]" + stations.Current.StationName + (_isSubAssy ? string.Empty : " [" + stations.Current.LineId + "]");
                lblVINNumber.Text = stations.Current.VINNumber;
                lblFINNumber.Text = stations.Current.FINNumber;
                lblModelVariant.Text = stations.Current.ModelName + " " + stations.Current.VariantName;

                lblEngineNo.Text = stations.EngineNo;
                lblCommnos.Text = stations.CommnosNo;
                lblPackingMonth.Text = stations.PackingMonth.ToString();
                lblFDoc.Text = stations.FDokProdNo;
                moOptionCode.Text = stations.OptionCodes;
                lblPaintCode.Text = stations.PaintCode.ToString();
                lblInteriorCode.Text = stations.InteriorCode.ToString();
                lblLocalProdNo.Text = stations.LocalProdNo;

                //binding fault
                gridFault.DataBind();

                if (_requestType == 0) //process and alteration only bind for stations not Q - gate
                {
                    cpPerStationGrid.DataBind();
                    gridIrregular.DataBind();
                }
                else
                {
                    tabDetail.TabPages.FindByName("tabProcess").Visible = false;
                    tabDetail.TabPages.FindByName("tabAlteration").Visible = false;
                }

            }
        }

        #region headerButton
        //action btnNextProcess
        //protected void btnNextProcess_Click(object sender, EventArgs e)
        //{
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
        //            //int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToInt32(1);

        //            //getnextstationid
        //            var nextStation = ProductionRepository.GetRouteNextStation(currentStationId, currentProdItem.ModelId, currentProdItem.VariantId, _lineId);

        //            if (nextStation == null)
        //            {
        //                //insert new
        //                if (0 != ProductionRepository.InsertProductionHistoryDetail(currentHistoryId, nextStation.Id, isOffline, isRectify,""))
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

        ////action btnMoveRectification
        //protected void btnMoveRecti_Click(object sender, EventArgs e)
        //{
        //    ToOfflineRecti(false);
        //}

        //action btnMoveOffline
        protected void btnMoveOffline_Click(object sender, EventArgs e)
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

            //var _StationPick = IdcmbEndOffLine.SelectedItem.Value;
            //var _StationPickName = IdcmbEndOffLine.SelectedItem.Text;
            //var _Sequence = ProductionRepository.GetStationCode(_StationPick.ToString());
            ////convert object to readable type
            //var _stationId = Convert.ToInt32(Request.QueryString["Stat"]);
            //var _historyId = Convert.ToInt32(Request.QueryString["Hist"]);
            ////get history item
            //var histItem = ProductionRepository.GetProductionItemByHistoryStation(_stationId, _historyId);

            string userName = AppConfiguration.DEFAULT_USERNAME;
            var user = Session["ProductionUser"] as User;
            if (user != null)
            {
                userName = user.UserName;
            }

            ProductionHistory prodHist = ProductionRepository.GetProductionHistory(_historyId);
            if (prodHist == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid production data');", true);
                return;
            }

            DotWeb.Models.Station targetStation = ProductionRepository.GetProductionStation(IdcmbEndOffLine.SelectedItem.Value.ToInt32(0));
            if (prodHist == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Invalid station data');", true);
                return;
            }

            try
            {
                bool status = prodHist.FinishRectification(targetStation, userName);
                if (!status)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to End of Line, please contact your administrator');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Move to End of Line: " + ex.Message + "');", true);
                AppLogger.LogError(ex.Message);
                return;
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Successfully Move to End of Line !');window.location.assign('../Production/ProductionDashboard.aspx');", true);
            //Response.Redirect("../Production/ProductionDashboard.aspx");
        }

        ////set Station to Offline & Rectification
        //private void ToOfflineRecti(bool updateOffline)
        //{
        //    //getcurrentStaion and history
        //    int currentStationId = _stationId;
        //    int currentHistoryId = _historyId;

        //    try
        //    {
        //        //current history
        //        var currentProdItem = ProductionRepository.GetProductionItemByHistoryStation(currentStationId, currentHistoryId);

        //        //getnextstatus            
        //        int statusId = currentProdItem.StatusId;

        //        //getnextposition            
        //        bool oldIsOffline = currentProdItem.IsOffline;
        //        bool oldIsRectify = currentProdItem.IsRectification;
        //        bool newIsOffline = updateOffline ? true : currentProdItem.IsOffline;
        //        bool newIsRectify = updateOffline ? currentProdItem.IsRectification : true;

        //        //update process in this station
        //        if (ProductionRepository.UpdateProductionHistoryDetail(currentHistoryId, currentStationId, oldIsOffline, oldIsRectify, newIsOffline, newIsRectify, false))
        //        {
        //            //back to dashboard
        //            Response.Redirect("../Production/ProductionDashboard.aspx");
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

        //action btnFault
        protected void btnFault_Click(object sender, EventArgs e)
        {
            var user = ProductionRepository.GetUserQM(tbLogin2.Text.Trim());
            if (user == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
                tbLogin2.Text = "";
                return;
            }
            tbLogin2.Text = "";

            Session["hdnUserAuth"] = user.AuthKey;

            pcInteruptFault.ShowOnPageLoad = true;
            loadFaultDefault();
            //var GetValueInspector = ProductionRepository.GetUserId(tbLogin2.Text);
            //cbInspector.Value = GetValueInspector == null ? 0 : GetValueInspector;
            cbInspector.Value = user.Id;
            pcLogin2.ShowOnPageLoad = false;
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
          //  var controlplan = ProductionRepository.GetProductionProcessByStationHistoryAll();
           // cpPerStationGridView.DataSource = controlplan;

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
            //is exist in fault record
            if (HiddenText["Edit"].ToString() == "0")
            {
                var fault = faultRecord();

                //is exist in fault record
                //if (FaultRepository.IsFaultSame(fault.ProductionHistoriesId, fault.CGISId))
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Check Again, Fault is already in database !!!');", true);
                //    loadFaultDefault();
                //}
                //else
                //{
                loadFaultDefault();
                //add to fault record
                int Id = FaultRepository.InsertFaultRecord(fault);
                //close popUpControl Fault

                //binding again grid Fault
                gridFault.DataBind();
                //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
                var SumRowData = GVDataAttachment.VisibleRowCount;
                List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                for (int i = 0; i < SumRowData; i++)
                {
                    DocumentAttachment AttachFile = new DocumentAttachment();
                    AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                    AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                    ListAttachFile.Add(AttachFile);
                }
                for (int x = 0; x < ListAttachFile.Count(); x++)
                {
                    var cbFINNUmber = cbFINNumber.Text.Split('/')[1].Trim();
                    FaultRepository.InsertAttachMentFromDasboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, cbFINNUmber, Id);
                }
                detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;
            }
            else
            {
                var model = new FaultRecord();
                model.CSType = CSTypeValue.Value == null ? 0 : int.Parse(CSTypeValue.Value.ToString());
                model.FaultRecordId = int.Parse(HiddenText["IdFault"].ToString());
                model.FaultStatus = cbFaultStatus.Text == "" ? 0 : int.Parse(cbFaultStatus.SelectedItem.Value.ToString());
                model.Remarks = tbRemark.Text.ToString();
                model.Priority = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.Value.ToString());
                model.FaultRelatedTypeId = cbFaultRelated.SelectedItem == null ? 0 : int.Parse(cbFaultRelated.Value.ToString());
                model.IsSentToRectification = Convert.ToBoolean(cbRecti.Value.ToInt32());
                model.FaultDescriptionId = cbFaultDesc.SelectedItem == null ? 0 : int.Parse(cbFaultDesc.Value.ToString());
                model.FaultDescriptionText = cbFaultDesc.Text.ToString();
                model.FaultPartProcessId = cbPartProces.SelectedItem == null ? 0 : int.Parse(cbPartProces.Value.ToString());
                model.StamperUserID = cbStamp.SelectedItem == null ? 0 : int.Parse(cbStamp.Value.ToString());
                model.StamperName = cbStamp.Text.ToString();
                model.QualityGateId = cbQG.SelectedItem == null ? 0 : int.Parse(cbQG.SelectedItem.Value.ToString());
                model.NCP = tbNCP.Text.ToString();
                model.InspectorUserId = cbInspector.SelectedItem == null ? 0 : int.Parse(cbInspector.Value.ToString());
                model.InspectorName = cbInspector.Text.ToString();
                model.RecordDate = DateTime.Parse(currentdate.Value.ToString("yyy/MM/dd"));
                FaultRepository.UpdateFaultRecord(model);
                //binding again grid Fault
                gridFault.DataBind();
                //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
                var SumRowData = GVDataAttachment.VisibleRowCount;
                List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                for (int i = 0; i < SumRowData; i++)
                {
                    DocumentAttachment AttachFile = new DocumentAttachment();
                    AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                    AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                    ListAttachFile.Add(AttachFile);
                }
                FaultRepository.Delete(int.Parse(Session["IdFault"].ToString()));
                for (int x = 0; x < ListAttachFile.Count(); x++)
                {
                    var id = cbFINNumber.Text;
                    FaultRepository.InsertAttachMentFaultDashboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, id, int.Parse(Session["IdFault"].ToString()));
                }
                detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;
            }
            loadFaultDefault();
        }

        //action btnSave&Close
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (HiddenText["Edit"].ToString() == "0")
            {
                var fault = faultRecord();

                //is exist in fault record
                //if (FaultRepository.IsFaultSame(fault.ProductionHistoriesId, fault.CGISId))
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Check Again, Fault is already in database !!!');", true);
                //    loadFaultDefault();
                //}
                //else
                //{
                //  loadFaultDefault();
                //add to fault record
                int Id = FaultRepository.InsertFaultRecord(fault);
                //close popUpControl Fault
                pcInteruptFault.ShowOnPageLoad = false;
                pcLogin2.ShowOnPageLoad = false;
                //binding again grid Fault
                gridFault.DataBind();
                //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
                var SumRowData = GVDataAttachment.VisibleRowCount;
                List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                for (int i = 0; i < SumRowData; i++)
                {
                    DocumentAttachment AttachFile = new DocumentAttachment();
                    AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                    AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                    ListAttachFile.Add(AttachFile);
                }
                for (int x = 0; x < ListAttachFile.Count(); x++)
                {
                    var cbFINNUmber = cbFINNumber.Text.Split('/')[1].Trim();
                    FaultRepository.InsertAttachMentFromDasboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, cbFINNUmber, Id);
                }
                detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;
            }
            else
            {
                var model = new FaultRecord();
                model.CSType = CSTypeValue.Value == null ? 0 : int.Parse(CSTypeValue.Value.ToString());
                model.FaultRecordId = int.Parse(HiddenText["IdFault"].ToString());
                model.FaultStatus = cbFaultStatus.Text == "" ? 0 : int.Parse(cbFaultStatus.Value.ToString());
                model.Remarks = tbRemark.Text.ToString();
                model.Priority = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.Value.ToString());
                model.FaultRelatedTypeId = cbFaultRelated.SelectedItem == null ? 0 : int.Parse(cbFaultRelated.Value.ToString());
                model.IsSentToRectification = Convert.ToBoolean(cbRecti.Value.ToInt32());
                model.FaultDescriptionId = cbFaultDesc.SelectedItem == null ? 0 : int.Parse(cbFaultDesc.Value.ToString());
                model.FaultDescriptionText = cbFaultDesc.Text.ToString();
                model.FaultPartProcessId = cbPartProces.SelectedItem == null ? 0 : int.Parse(cbPartProces.Value.ToString());
                model.StamperUserID = cbStamp.SelectedItem == null ? 0 : int.Parse(cbStamp.Value.ToString());
                model.StamperName = cbStamp.Text.ToString();
                model.QualityGateId = cbQG.SelectedItem == null ? 0 : int.Parse(cbQG.SelectedItem.Value.ToString());
                model.NCP = tbNCP.Text.ToString();
                model.InspectorUserId = cbInspector.SelectedItem == null ? 0 : int.Parse(cbInspector.Value.ToString());
                model.InspectorName = cbInspector.Text.ToString();
                model.RecordDate = DateTime.Parse(currentdate.Value.ToString("yyy/MM/dd"));
              
                FaultRepository.UpdateFaultRecord(model);
                pcInteruptFault.ShowOnPageLoad = false;
                pcLogin2.ShowOnPageLoad = false;
                //binding again grid Fault
                gridFault.DataBind();
                //   detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
                var SumRowData = GVDataAttachment.VisibleRowCount;
                List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                for (int i = 0; i < SumRowData; i++)
                {
                    DocumentAttachment AttachFile = new DocumentAttachment();
                    AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                    AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                    ListAttachFile.Add(AttachFile);
                }
                FaultRepository.Delete(int.Parse(Session["IdFault"].ToString()));
                for (int x = 0; x < ListAttachFile.Count(); x++)
                {
                    var id = cbFINNumber.Text;
                    FaultRepository.InsertAttachMentFaultDashboard(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, id, int.Parse(Session["IdFault"].ToString()));
                }
                detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;
            }

            //  }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = false;
            pcLogin2.ShowOnPageLoad = false;

            gridFault.DataBind();
            // detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;

            detailCondition.ImageUrl =
                    FaultRepository.IsHaveFault(_historyId) ||
                    (ProductionRepository.GetAndonProductionProblem(_lineId, DateTime.Now, "", _stationId).Count() > 0)
                    ? urlImgNotifRed : urlImgNotifGreen;

        }

        protected void pcInteruptFault_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            //loadFaultDefault();
            //GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataSource = Session["Attachment"];
            GVDataAttachment.DataBind();
        }

        //binding FaultRecord
        public FaultRecord faultRecord()
        {
            int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToInt32(1);

            int csType = CSTypeValue.Value == null ? 0 : int.Parse(CSTypeValue.Value.ToString());
            var ncp = tbNCP.Text;
            int productionHistoriesId = Convert.ToInt32(cbFINNumber.Value);
            var faultNumber = "32054";
            int QGate = Convert.ToInt32(cbQG.Value);
            int station = Convert.ToInt32(cbStation.Value);
            ListEditItem selectedItem = cbFINNumber.SelectedItem;
            var finnumber = selectedItem.GetValue("FINNumber").ToString();

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

            var inspectorName = cbInspector.SelectedItem == null ? "" : cbInspector.SelectedItem.Text;
            int stampId = cbStamp.Value == null ? 0 : Convert.ToInt32(cbStamp.Value);
            // var stampName = cbStamp.SelectedItem;
            var remark = tbRemark.Text.ToString();
            bool recti = Convert.ToBoolean(cbRecti.Value.ToInt32());

            string userauth = Session["hdnUserAuth"].ToString();


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
                StamperName = "",
                Remarks = remark,
                IsSentToRectification = recti,
                UserAuth = userauth
            };

            return fault;
        }

        //binding & set default form Fault
        private void loadFaultDefault()
        {
            try
            {
                Session["Attachment"] = null;
                GVDataAttachment.DataSource = null;
                GVDataAttachment.DataBind();
                currentdate.Date = DateTime.Now;
                HiddenText["Edit"] = "0";
                cbStation.SelectedIndex = 0;
                cbQG.SelectedIndex = 0;
                cbFINNumber.Value = _historyId;
                HiddenText["FINNumber"] = cbFINNumber.Text;
                cbPartProces.SelectedIndex = 0;
                cbFaultDesc.SelectedIndex = 0;
                cbFaultRelated.SelectedIndex = 0;
                cbPriority.SelectedIndex = 0;
                cbInspector.SelectedIndex = 0;
                cbStamp.SelectedIndex = 0;
                cbRecti.SelectedIndex = 0;
                tbNCP.Text = "";
                tbRemark.Text = "";
                cbFaultStatus.Value = "0";
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
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
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

        protected void BtnEndOffLine_Click(object sender, EventArgs e)
        {
            User user = ProductionRepository.GetUserQM(tbLogin.Text.Trim());
            if (user == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
                tbLogin.Text = "";
                return;
            }
            tbLogin.Text = "";

            //store in session for later retrieval
            Session["ProductionUser"] = user;

            PopUpEndOffLine.ShowOnPageLoad = true;
        }

        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Production/ProductionDashboard.aspx");
        }

        protected void Btn_Edit_Click(object sender, EventArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = true;
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            try
            {
                int ValueId = gridFault.GetRowValues(index, "FaultRecordId") == null ? 0 : int.Parse(gridFault.GetRowValues(index, "FaultRecordId").ToString());
                Session["IdFault"] = ValueId;
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
        //binding & set default form Fault
        private void loadFaultDefaultEdit(int id)
        {
            HiddenText["Edit"] = "1";
            HiddenText["IdFault"] = id;
            Session["Attachment"] = null;
            Session["Attachmentx"] = null;
            var DataLoad = FaultRepository.GetFaultRecordById(id);
            //GVDataAttachment.DataSource = Session["dtAttachment"];
            //GVDataAttachment.DataBind();
            CSTypeValue.Text = DataLoad[0].CSType.ToString();
            cbStation.Text = DataLoad[0].StationName.ToString();
            cbStation.ReadOnly = true;
            cbStamp.Text = DataLoad[0].StamperName;
            tbRemark.Text = DataLoad[0].Remarks;
            cbQG.Text = DataLoad[0].QualityGateName.ToString();
            //cbQG.ReadOnly = true;
            cbFaultStatus.Text = DataLoad[0].FaultStatus.ToString();
            cbFINNumber.Text = DataLoad[0].FINNumber.ToString() + '/';
            Session["FinNumber"] = DataLoad[0].FINNumber.ToString();
            cbFINNumber.ReadOnly = true;
            tbLine.Text = DataLoad[0].ProductionLineId.ToString();
            tbLine.ReadOnly = true;
            tbNCP.Text = DataLoad[0].NCP.ToString();
            tbAssemblySection.Text = DataLoad[0].AssemblySectionName;
            cbStamp.Text = DataLoad[0].StamperName;
            cbPartProces.Text = DataLoad[0].FaultPartProcessDesc.ToString();
            cbFaultDesc.Text = DataLoad[0].FaultDescriptionText;
            cbFaultRelated.Text = DataLoad[0].FaultRelatedTypeDesc;
            cbPriority.Text = DataLoad[0].Priority.ToString();
            cbInspector.Text = DataLoad[0].InspectorName;
            cbStamp.Text = DataLoad[0].StamperName;
            cbRecti.Text = DataLoad[0].IsSentToRectification.ToString();
            tbNCP.Text = DataLoad[0].NCP;
            tbRemark.Text = DataLoad[0].Remarks;
            currentdate.Value = DataLoad[0].RecordDate;
            //set TextBox Line
            //string line = GetLine(Convert.ToInt32(cbFINNumber.Value));
            tbLine.Text = DataLoad[0].ProductionLineName;

            //set textBox Assembly Section
            //string section = GetSection(Convert.ToInt32(cbStation.Value));
            tbAssemblySection.Text = DataLoad[0].AssemblySectionName;

            //set textBox Fault Classification
            //string classification = GetClassification(Convert.ToInt32(cbFaultDesc.Value));
            tbClassification.Text = DataLoad[0].FaultClassDescription;

            //set comboBox CGIS
            cbCGIS.Text = DataLoad[0].CGISNo;
            HiddenText["FINNumber"] = cbFINNumber.Text == "1" ? "-Select-" : cbFINNumber.Text;
            if (cbFINNumber.Text == "1")
            {
                cbFINNumber.Text = "-Select-";
            }
            else
            {
                cbFINNumber.Text = DataLoad[0].FINNumber;
            }
            var faults = FaultRepository.GetAttachMentViewEdit(id);
            Session["Attachment"] = faults;

            Session["Attachmentx"] = faults;
            GVDataAttachment.DataSource = faults;
            GVDataAttachment.DataBind();
        }



        protected void ShowAttachment_Click(object sender, EventArgs e)
        {
            PopUpAttachMent.ShowOnPageLoad = true;
        }

        protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = false;
            PopUpAttachMent.ShowOnPageLoad = false;
            pcInteruptFault.ShowOnPageLoad = true;
            GVDataAttachment.DataSource = Session["Attachment"];
            GVDataAttachment.DataBind();
            //GVDataAttachmentx.DataSource = Session["dtAttachment"];
            //GVDataAttachmentx.DataBind();
        }

        protected void UploadAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (HiddenText["Edit"].ToString() == "0")
            {
                string x = "";
                string HiddenCstype = "2";
                if (HiddenText["FINNumber"].ToString() != "")
                {
                    x = HiddenText["FINNumber"].ToString().Split('/')[1].Trim();
                }
                else
                {
                    x = cbFINNumber.Text;
                }
                Session["FINNumber"] = x;
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
                // var oldattachImage = FaultRepository.GetAttachMentView(Session["FinNumber"].ToString());

                ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
                AttachFile.Id = GetIdAttachment();
                AttachFile.FileName = resultFileName;
                AttachFile.FileType = resultExtension;
                AttachFile.FileLocation = resultFileUrl;
                AttachFileModelList.Add(AttachFile);
                //ListAttachment.attachmentAuditModels = AttachFileModelList;
                // }


                DataTable table = ConvertListToDataTable(AttachFileModelList);
                Session["dtAttachment"] = table;
                Session["Attachment"] = AttachFileModelList;
                cbFINNumber.Text = HiddenText["FINNumber"].ToString();
                GVDataAttachment.DataSource = Session["Attachment"];
                GVDataAttachment.DataBind();
                PopUpAttachMent.ShowOnPageLoad = false;
                // PopUpAttachMentx.ShowOnPageLoad = false;
            }
            else
            {
                string HiddenCstype = "2";
                string x = Session["FinNumber"].ToString();
                Session["FINNumber"] = x;
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
                var Data = Session["Attachmentx"] as List<ProductionFaultReleaseAttachmentModel>;
                var oldattachImage = Session["Attachmentx"] as List<ProductionFaultReleaseAttachmentModel>;

                if (Data == null)
                {
                    ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
                    AttachFile.Id = GetIdAttachment();
                    AttachFile.FileName = resultFileName;
                    AttachFile.FileLocation = resultFileUrl;
                    AttachFile.FileType = resultExtension;

                    AttachFileModelList.Add(AttachFile);
                    Session["Attachment"] = AttachFileModelList;
                    //ListAttachment.attachmentAuditModels = AttachFileModelList;
                }
                else
                {
                    ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
                    for (int i = 0; i < oldattachImage.Count; i++)
                    {
                        AttachFile.Id = oldattachImage[i].Id;
                        AttachFile.FileName = oldattachImage[i].FileName;
                        AttachFile.FileType = oldattachImage[i].FileType;
                        AttachFile.FileLocation = oldattachImage[i].FileLocation;
                        AttachFileModelList.Add(AttachFile);
                        //ListAttachment.attachmentAuditModels = AttachFileModelList;
                        Session["Attachmentx"] = null;
                    }
                    ProductionFaultReleaseAttachmentModel AttachFilex = new ProductionFaultReleaseAttachmentModel();
                    AttachFilex.Id = GetIdAttachment();
                    AttachFilex.FileName = resultFileName;
                    AttachFilex.FileLocation = resultFileUrl;
                    AttachFilex.FileType = resultExtension;
                    AttachFileModelList.Add(AttachFilex);
                    //ListAttachment.AttachmentModels = AttachFileModelList;
                    Session["Attachment"] = AttachFileModelList;
                }
                var DataView = Session["Attachment"] as List<ProductionFaultReleaseAttachmentModel>;
                DataTable table = ConvertListToDataTable(DataView);
                Session["dtAttachment"] = table;
                cbFINNumber.Text = HiddenText["FINNumber"].ToString();
                //GVDataAttachmentx.DataSource = Session["dtAttachment"];
                GVDataAttachment.DataSource = DataView;
                GVDataAttachment.DataBind();
            }

        }
        protected int GetIdAttachment()
        {
            int IdData = 0;
            var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
            IdData = Id;
            return IdData;
        }
        static DataTable ConvertListToDataTable(List<ProductionFaultReleaseAttachmentModel> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = list.Count();


            // Add columns.

            table.Columns.Add("Id");
            table.Columns.Add("FileName");
            table.Columns.Add("FileLocation");


            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array.Id, array.FileName, array.FileLocation);
            }

            return table;
        }

        protected void pcInteruptFault_DataBinding(object sender, EventArgs e)
        {

        }







        protected void cpPerStationGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grv = (sender as ASPxGridView);
            if (grv.GetRowValues(e.VisibleIndex, "HasDeviation") != null)
            {
                if (grv.GetRowValues(e.VisibleIndex, "HasDeviation") != null &&
                Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "HasDeviation")))
                {
                    Color tlr = System.Drawing.ColorTranslator.FromHtml("#44C169");
                    e.Row.BackColor = tlr;
                }
            }

        }

        protected void IdBtndelete_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var Rowindex = container.VisibleIndex;
            int ValueRowId = int.Parse(GVDataAttachment.GetRowValues(Rowindex, "Id").ToString());
            //var Data = Session["dtAttachment"];
            //var oldattachImage = Session["ByteImage"] as List<FaultAuditAttachmentModel>;
            // DataTable Datax = Session["dtAttachment"] as DataTable;
            //List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
            //List<DataRow> list = Datax.AsEnumerable().ToList();
            ////int Id = int.Parse(Datax.Rows[0].ItemArray[0].ToString());
            //for (int i = 0; i < Datax.Rows.Count; i++)
            //{
            //    int Id = int.Parse(Datax.Rows[i].ItemArray[0].ToString());
            //    if (Id == ValueRowId)
            //    {
            //        DataRow dr = Datax.Rows[i];
            //        dr.Delete();
            //        dr.AcceptChanges();
            //    }
            //}
            var oldattachImage = Session["Attachment"] as List<ProductionFaultReleaseAttachmentModel>;
            for (int i = 0; i < oldattachImage.Count; i++)
            {
                if (oldattachImage[i].Id == ValueRowId)
                {
                    oldattachImage.Remove(oldattachImage[i]);
                    Session["Attachment"] = oldattachImage;
                }
            }

            GVDataAttachment.DataSource = Session["Attachment"];
            GVDataAttachment.DataBind();

        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            int index = int.Parse(Session["IndexDelete"].ToString());

            try
            {
                FaultRepository.DeleteFault(index);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
            FormConfirmDelete.ShowOnPageLoad = false;
            Response.Redirect(Request.RawUrl);
        }

        protected void BtnSaveIndex(object sender, EventArgs e)
        {
            //pcInteruptFault.ShowOnPageLoad = true;
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            var index = container.VisibleIndex;
            int ValueId = int.Parse(gridFault.GetRowValues(index, "FaultRecordId").ToString());
            Session["IndexDelete"] = ValueId;
            FormConfirmDelete.ShowOnPageLoad = true;
        }
    }
}