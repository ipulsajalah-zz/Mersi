using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Uploaders;
using Ebiz.Scaffolding.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Tids.CV.Uploaders;

namespace Ebiz.WebForm.Tids.CV.Checklist
{
    public partial class STAChecklist : ChecklistPageBase
    {
        //Default table name
        protected static string checklistName = "STA";

        private DataTable dataTable;

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            checklist = schemaInfo.Checklists.Where(s => s.Name.Equals(checklistName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (checklist == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            //TODO: move to configuration
            //tableMeta.PreviewColumnName = "Torque";

            //header info
            int modelId = Request.QueryString["model"].ToInt32(0);
            string packingMonth = Request.QueryString["vpm"];

            //Set master key
            if (modelId > 0)
                SetMasterKey("ModelId", modelId);
            if (!string.IsNullOrEmpty(packingMonth))
                SetMasterKey("PackingMonth", packingMonth);

            //create header
            string modelName = RicRepository.GetModelName(modelId);

            //lblPackingMonth.Text = packingMonth;
            //lblModel.Text = modelName;

            //var panel = new System.Web.UI.WebControls.Panel();
            //panel.CssClass = "mainContent";
            //panel.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>{0} ({1} {2} - {3})</h2>", tableMeta.Caption, ModelName, VariantName, cp.PackingMonth)));
            //masterPage.MainContent.Controls.Add(panel);

             Session["ChecklistName"] = checklistName;

            //cmbProductionLines.DataSource = sdsProductionLineLookup;
            //sdsProductionLineLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
            //sdsProductionLineLookup.DataBind();

            cmbPrevPM.DataSource = sdsPrevPMLookup;

            return true; 
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            //if (masterGrid != null)
            //{
            //}

            return true;
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
                int packingMonth = Session["PM"].ToInt32(0);
                int modelId = Session["ModelId"].ToInt32(0);
                int ricType = cmbRicType.Value.ToInt32(0);

                //parse the file
                string message = "";
                int rowCount = uploader.Parse(filePath, packingMonth, modelId, ricType);
                if (rowCount > 0)
                {
                    lblSuccess.Text = "Successfully upload and process data " + Path.GetFileName(filePath) + ", " + rowCount + " rows inserted, " + message;

                    //this.masterGrid.DataBindSafe();

                    int stepId = Session["StepId"].ToInt32(0);
                    ChecklistHelper.ReleaseStep(stepId);

                    string gridName = Session["StepGrid"].ToString();
                    ASPxGridView grid = this.Page.FindNestedControl2(gridName) as ASPxGridView;
                    if (grid != null)
                    {
                        grid.DataBindSafe();
                    }
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

                    //e.CallbackData = lblSuccess.Text;

                    //e.IsValid = false;
                    //e.ErrorText = lblSuccess.Text;
                }
            }
            else
            {
                string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);
                if (arr.Length < 1)
                    return;

                int instanceStepId = arr[0].ToInt32(0);
                string gridName = arr[1];

                ChecklistInstanceStep step = ChecklistHelper.GetChecklistStep(instanceStepId);
                if (step == null || step.ChecklistInstance == null)
                    return;

                // get parameter
                DateTime dt = DateTime.Parse(ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "PackingMonth"));
                int packingMonth = dt.ToString("yyyyMM").ToInt32(0);
                int modelId = ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "CatalogModelId").ToInt32(0);

                Session["PM"] = packingMonth;
                Session["ModelId"] = modelId;
                Session["StepId"] = instanceStepId;
                Session["StepGrid"] = gridName;

                lblSuccess.Text = "";
                //txtPM.Value = packingMonth.ToString();
                //txtModelId.Value = modelId;
                ////cmbRicType.Value = 0;
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
            string path = ConfigurationHelper.UPLOAD_DIR + "/" + fileTypeName  + "/" + Session["ModelName"];
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
                int modelId = Session["ModelId"].ToInt32(0);
                int packingMonth = txtActPM.ToInt32(0);
                int prevPackingMonth = cmbPrevPM.Value.ToInt32(0);
                int ricType = cmbRicType.Value.ToInt32(0);

                bool status = RicRepository.ComparePackingList(modelId, ricType, packingMonth, prevPackingMonth);

                if (status)
                {
                    lblSuccess_Compare.Text = "Successfully compare packing month " + packingMonth + " with packing month " + prevPackingMonth;
                }
            }
            else
            {
                string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);
                if (arr.Length < 1)
                    return;

                int instanceStepId = arr[0].ToInt32(0);

                ChecklistInstanceStep step = ChecklistHelper.GetChecklistStep(instanceStepId);
                if (step == null || step.ChecklistInstance == null)
                    return;

                // get parameter
                DateTime dt = DateTime.Parse(ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "PackingMonth"));
                int packingMonth = dt.ToString("yyyyMM").ToInt32(0);
                int modelId = ChecklistHelper.GetChecklistParameterValue(step.ChecklistInstanceId, "CatalogModelId").ToInt32(0);

                txtActPM.Value = packingMonth.ToString();
                //txtModelId_Compare.Value = modelId.ToString();

                Session["ModelId"] = modelId;

                SqlDataSource sds = cmbPrevPM.DataSource as SqlDataSource;
                sds.SelectParameters["ModelId"].DefaultValue = modelId.ToString();
                sds.SelectParameters["PackingMonth"].DefaultValue = packingMonth.ToString();
                cmbPrevPM.DataBind();

                cmbPrevPM.SelectedIndex = 0;
            }
        }
    }

}