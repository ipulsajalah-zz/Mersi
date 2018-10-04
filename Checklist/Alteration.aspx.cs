using DevExpress.Web;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Tids.CV.Uploaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.Checklist
{
    public partial class Alteration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cmbPrevPM.DataSource = sdsPrevPMLookup;
            int ModelId = Request.QueryString["CatalogModelId"].ToInt32(0);
            int PackingMonth = Request.QueryString["PackingMonth"].ToInt32(0);
            int RictypeId = Request.QueryString["RICTypeId"].ToInt32(0);

            var model = RicRepository.getModelNamebyModelId(ModelId);
            var RicType = RicRepository.getRicTypebyRicId(RictypeId);

            if (ModelId != null || ModelId != 0)
            {
                if (model != "")
                {
                    string tahun = PackingMonth.ToString().Substring(0, 4);
                    string bulan = PackingMonth.ToString().Substring(4, 2);
                    string vpm = "01" + '/' + bulan + '/' + tahun;
                    cmbModels.Value = model;
                    dtPackingMonth.Value = vpm;
                    cmbRicType.Value = RicType;

                    RefreshPage(ModelId, PackingMonth, RictypeId);
                }
                else
                {
                    cmbModels.Value = "";
                    dtPackingMonth.Value = "";
                }
            }
            else
            {
                cmbModels.Value = "";
                dtPackingMonth.Value = "";
            }
        }

        #region Upload Document

        protected void popupDocUpload_OnWindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "upload")
            {
                string filePath = Session["pl_uploaded_file"] as string;

                PackingListUploader uploader = new PackingListUploader();

                //set username for column createdby and modifiedby
                uploader.Username = PermissionHelper.GetAuthenticatedUserName();

                //parameter
                //DateTime value = (DateTime) dtPM.Value;
                int packingMonth = dtPackingMonth.Date.ToString("yyyyMM").ToInt32(0);
                int modelId = cmbModels.Value.ToInt32(0);
                int ricType = cmbRicType.Value.ToInt32(0);

                //parse the file
                string message = "";
                int rowCount = uploader.Parse(filePath, packingMonth, modelId, ricType);
                if (rowCount > 0)
                {
                    lblSuccess.Text = "Successfully upload and process data " + Path.GetFileName(filePath) + ", " + rowCount + " rows inserted, " + message;

                    popupDocUpload.JSProperties["cpResult"] = "1";

                    //this.masterGrid.DataBindSafe();

                    //int stepId = Session["StepId"].ToInt32(0);
                    //ChecklistHelper.ReleaseStep(stepId);

                    //string gridName = Session["StepGrid"].ToString();
                    //ASPxGridView grid = this.Page.FindNestedControl2(gridName) as ASPxGridView;
                    //if (grid != null)
                    //{
                    //    grid.DataBindSafe();
                    //}
                }
                else
                {
                    if (uploader.ErrorMessage.Length > 0)
                    {
                        lblSuccess.Text = uploader.ErrorMessage;
                    }
                    else
                    {
                        lblSuccess.Text = "Upload file " + Path.GetFileName(filePath) + " failed!";
                    }
                    //delete the file
                    File.Delete(filePath);

                    popupDocUpload.JSProperties["cpResult"] = "-1";

                    //e.CallbackData = lblSuccess.Text;

                    //e.IsValid = false;
                    //e.ErrorText = lblSuccess.Text;
                }
            }
            else
            {
                //string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);
                //if (arr.Length < 1)
                //    return;

                //int instanceStepId = arr[0].ToInt32(0);
                //string gridName = arr[1];

                //ChecklistInstanceStep step = ChecklistHelper.GetChecklistStep(instanceStepId);
                //if (step == null || step.ChecklistInstance == null)
                //    return;

                //// get parameter
                //DateTime dt = DateTime.Parse(ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "PackingMonth"));
                //int packingMonth = dt.ToString("yyyyMM").ToInt32(0);
                //int modelId = ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "CatalogModelId").ToInt32(0);

                //Session["PM"] = packingMonth;
                //Session["ModelId"] = modelId;
                //Session["StepId"] = instanceStepId;
                //Session["StepGrid"] = gridName;

                lblSuccess.Text = "";
                //txtPM.Value = packingMonth.ToString();
                //txtModelId.Value = modelId;
                ////cmbRicType.Value = 0;

                popupDocUpload.JSProperties["cpResult"] = "0";

            }

        }

        protected void fileUploadControl_OnFileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                //string fileType = Session["FileTypeName"].ToString();
                //int fileTypeId = Session["FileTypeId"].ToInt32(0);

                string filePath = "";
                if (e.IsValid)
                {
                    filePath = SavePostedFile(e.UploadedFile, "PackingLists");
                }

                Session["pl_uploaded_file"] = filePath;
            }
            catch (Exception ex)
            {
                e.ErrorText = ex.Message + " " + ex.StackTrace;
                e.IsValid = false;
                e.CallbackData = "Upload file failed.  " + ex.Message;
                LoggerHelper.LogError(ex);
                LoggerHelper.LogError(e.ErrorText);
            }
        }

        private string SavePostedFile(UploadedFile eUploadedFile, string fileTypeName)
        {
            if (!eUploadedFile.IsValid) return String.Empty;

            //=========cek folder Packing Month
            string path = ConfigurationHelper.UPLOAD_DIR + "/" + fileTypeName + "/" + Session["ModelName"];
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }

            //=========cek folder Model
            string pathModel = path + "/" + Session["PackingMonth"];
            if (!Directory.Exists(Server.MapPath(pathModel)))
            {
                Directory.CreateDirectory(Server.MapPath(pathModel));
            }

            String UploadDir = pathModel + "/";

            FileInfo fileInfo = new FileInfo(eUploadedFile.FileName);
            String fileName = eUploadedFile.FileName.ToString().Replace(" ", "_");
            String ext = Path.GetExtension(eUploadedFile.FileName);
            String fileType = eUploadedFile.ContentType.ToString();

            String resFileName = "";
            if (!File.Exists(UploadDir + fileName))
            {
                resFileName = Server.MapPath(UploadDir + fileName);
                eUploadedFile.SaveAs(resFileName);
            }

            return resFileName;
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            try
            {
                popupDocUpload.ShowOnPageLoad = false;

                //CheckListInstanceRepository.SetStatusCheckListStep(Convert.ToInt32(Session["Id"]), CheckListStatus.Complete);
                //grvInstanceInfo.DataBind();

                //Page page = HttpContext.Current.CurrentHandler as Page;
                //// Checks if the handler is a Page and that the script isn't allready on the Page
                //if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("refreshGrid"))
                //{
                //    page.ClientScript.RegisterClientScriptBlock(typeof(CheckListInstanceInfo), "refreshGrid", "RefreshGrid();");
                //}
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                //TODO

                //popupDocUpload.ShowOnPageLoad = false;

                //CheckListInstanceRepository.SetStatusCheckListStep(Convert.ToInt32(Session["Id"]), CheckListStatus.Complete);
                //grvInstanceInfo.DataBind();

                //Page page = HttpContext.Current.CurrentHandler as Page;
                //// Checks if the handler is a Page and that the script isn't allready on the Page
                //if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("refreshGrid"))
                //{
                //    page.ClientScript.RegisterClientScriptBlock(typeof(CheckListInstanceInfo), "refreshGrid", "RefreshGrid();");
                //}
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnExit_OnClick(object sender, EventArgs e)
        {
            try
            {
                //TODO

                popupDocUpload.ShowOnPageLoad = false;

                //CheckListInstanceRepository.SetStatusCheckListStep(Convert.ToInt32(Session["Id"]), CheckListStatus.Complete);
                //grvInstanceInfo.DataBind();

                //Page page = HttpContext.Current.CurrentHandler as Page;
                //// Checks if the handler is a Page and that the script isn't allready on the Page
                //if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("refreshGrid"))
                //{
                //    page.ClientScript.RegisterClientScriptBlock(typeof(CheckListInstanceInfo), "refreshGrid", "RefreshGrid();");
                //}
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        #endregion

        protected void popupComparePL_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "compare")
            {
                int packingMonth = dtPackingMonth.Date.ToString("yyyyMM").ToInt32(0);
                int modelId = cmbModels.Value.ToInt32(0);
                int ricType = cmbRicType.Value.ToInt32(0);

                int prevPackingMonth = cmbPrevPM.Value.ToInt32(0);

                try
                {
                    bool status = RicRepository.ComparePackingList(modelId, ricType, packingMonth, prevPackingMonth);

                    if (status)
                    {
                        lblSuccess_Compare.Text = "Successfully compare packing month " + packingMonth + " with packing month " + prevPackingMonth;
                        popupComparePL.JSProperties["cpResult"] = "1";
                    }
                }
                catch (Exception ex)
                {
                    lblSuccess_Compare.Text = ex.Message;
                    popupComparePL.JSProperties["cpResult"] = "-1";
                }
            }
            else
            {
                //string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);
                //if (arr.Length < 1)
                //    return;

                //int instanceStepId = arr[0].ToInt32(0);

                //ChecklistInstanceStep step = ChecklistHelper.GetChecklistStep(instanceStepId);
                //if (step == null || step.ChecklistInstance == null)
                //    return;

                //// get parameter
                //DateTime dt = DateTime.Parse(ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "PackingMonth"));
                //int packingMonth = dt.ToString("yyyyMM").ToInt32(0);
                //int modelId = ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "CatalogModelId").ToInt32(0);

                int modelId = cmbModels.Value.ToInt32(0);
                int packingMonth = dtPackingMonth.Date.ToString("yyyyMM").ToInt32(0);

                txtActPM.Value = packingMonth.ToString();
                //txtModelId_Compare.Value = modelId.ToString();

                //Session["ModelId"] = modelId;

                SqlDataSource sds = cmbPrevPM.DataSource as SqlDataSource;
                sds.SelectParameters["ModelId"].DefaultValue = modelId.ToString();
                sds.SelectParameters["PackingMonth"].DefaultValue = packingMonth.ToString();
                cmbPrevPM.DataBind();

                cmbPrevPM.SelectedIndex = 0;

                popupComparePL.JSProperties["cpResult"] = "0";
            }
        }

        protected void callback_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] str = e.Parameter.Split(";");
            if (str.Length < 4)
                return;

            if (str[0] == "release")
            {
                int modelid = str[1].ToInt32(0);
                int vpm = str[2].ToInt32(0);
                int rictype = str[3].ToInt32(0);

                try
                {
                    RicRepository.ReleaseRIC(modelid, vpm, rictype);
                    callback.JSProperties["cpResult"] = "RIC successfully released";
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpResult"] = ex.Message;
                }

                PackingList pl = RicRepository.GetPackingList(modelid, vpm, rictype);
                if (pl != null)
                {
                    imgUploadPL.ImageUrl = "/Content/Images/status-green.png";
                    imgUploadPL.Visible = true;
                    lblPL.Text = "Filename: " + pl.FileName + "; Lot: " + pl.LotNos + "; Upload Date: " + pl.CreatedDate;
                }
                else
                {
                    imgUploadPL.ImageUrl = "/Content/Images/status-red.png";
                    imgUploadPL.Visible = true;
                    lblPL.Text = "No packing list uploaded.";
                }

                RIC ric = RicRepository.GetRIC(modelid, vpm, rictype);
                if (ric != null && ric.CompareDate != null && ric.CompareDate.Value > pl.CreatedDate)
                {
                    imgComparePL.ImageUrl = "/Content/Images/status-green.png";
                    imgComparePL.Visible = true;
                    lblComparePL.Text = "RICNr: " + ric.RICNr + "; Compare Date: " + ric.CompareDate;

                    if (ric.StatusId != EStatus.Draft)
                    {
                        imgReleaseRic.ImageUrl = "/Content/Images/status-green.png";
                        imgReleaseRic.Visible = true;
                        lblReleaseRIC.Text = "RICNr: " + ric.RICNr + "; Released Date: " + ric.IssuedDate;
                    }
                    else
                    {
                        imgReleaseRic.ImageUrl = "/Content/Images/status-red.png";
                        imgReleaseRic.Visible = true;
                        lblReleaseRIC.Text = "No RIC";
                    }
                }
                else
                {
                    imgComparePL.ImageUrl = "/Content/Images/status-red.png";
                    imgComparePL.Visible = true;
                    lblComparePL.Text = "No RIC";

                    imgReleaseRic.ImageUrl = "/Content/Images/status-red.png";
                    imgReleaseRic.Visible = true;
                    lblReleaseRIC.Text = "No RIC";
                }

                if (ric == null)
                    hdField["status"] = 0;
                else
                    hdField["status"] = (int) ric.StatusId;
            }
            else if (str[0] == "refresh")
            {
                int modelid = str[1].ToInt32(0);
                int vpm = str[2].ToInt32(0);
                int rictype = str[3].ToInt32(0);
                RefreshPage(modelid, vpm, rictype);
            }

        }
        protected void RefreshPage(int modelid, int vpm, int rictype) 
        {
            PackingList pl = RicRepository.GetPackingList(modelid, vpm, rictype);
            if (pl != null)
            {
                imgUploadPL.ImageUrl = "/Content/Images/status-green.png";
                imgUploadPL.Visible = true;
                lblPL.Text = "Filename: " + pl.FileName + "; Lot: " + pl.LotNos + "; Upload Date: " + pl.CreatedDate;
            }
            else
            {
                imgUploadPL.ImageUrl = "/Content/Images/status-red.png";
                imgUploadPL.Visible = true;
                lblPL.Text = "No packing list uploaded.";
            }

            RIC ric = RicRepository.GetRIC(modelid, vpm, rictype);
            if (ric != null && ric.CompareDate != null && ric.CompareDate.Value > pl.CreatedDate)
            {
                imgComparePL.ImageUrl = "/Content/Images/status-green.png";
                imgComparePL.Visible = true;
                lblComparePL.Text = "RICNr: " + ric.RICNr + "; Compare Date: " + ric.CompareDate;

                if (ric.StatusId != EStatus.Draft)
                {
                    imgReleaseRic.ImageUrl = "/Content/Images/status-green.png";
                    imgReleaseRic.Visible = true;
                    lblReleaseRIC.Text = "RICNr: " + ric.RICNr + "; Released Date: " + ric.IssuedDate;
                }
                else
                {
                    imgReleaseRic.ImageUrl = "/Content/Images/status-red.png";
                    imgReleaseRic.Visible = true;
                    lblReleaseRIC.Text = "No RIC";
                }
            }
            else
            {
                imgComparePL.ImageUrl = "/Content/Images/status-red.png";
                imgComparePL.Visible = true;
                lblComparePL.Text = "No RIC";

                imgReleaseRic.ImageUrl = "/Content/Images/status-red.png";
                imgReleaseRic.Visible = true;
                lblReleaseRIC.Text = "No RIC";
            }

            callback.JSProperties["cpResult"] = "";

            if (ric == null)
                hdField["status"] = 0;
            else
                hdField["status"] = (int)ric.StatusId;

            cmbModels.Enabled = false;
            dtPackingMonth.Enabled = false;
            cmbRicType.Enabled = false;
        }
    }
}
