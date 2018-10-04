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

namespace Ebiz.WebForm.custom.DPR
{
    public partial class DPRUpload : ListPageBase
    {
        //Table Name
        protected static string tableName = "DailyProductionRequirements";

        private DataTable dataTable;

        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            ////header info
            //string productionMonth = Request.QueryString["vpm"];

            ////Set master key
            //if (!string.IsNullOrEmpty(productionMonth))
            //    SetMasterKey("ProductionMonth", productionMonth);

            //create header
            Session["TableName"] = tableName;
            return true;
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }

        #region Upload Document

        protected void popupDocUpload_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "upload")
            {
                string filePath = Session["pl_uploaded_file"] as string;
                string fileName = Path.GetFileName(filePath);
                //int userId = 0;

                var user = HttpContext.Current.Session["user"] as User;
                int userId = 0;
                if (user != null)
                {
                    userId = user.Id;
                }

                DPRUploader uploader = new DPRUploader();

                //set username for column createdby and modifiedby
                uploader.Username = PermissionHelper.GetAuthenticatedUserName();

                //parameter
                DateTime value = (DateTime)dtPM.Value;
                int productionMonth = value.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture).ToInt32(0);

                //parse the file
                string message = "";
                int rowCount = 0; // uploader.Parse(filePath, productionMonth);

                //UploadeingFile
                var upload = DPRUploader.UploadFile(productionMonth.ToString(), fileName, filePath, userId);

               // if (rowCount > 0)
                if (upload)
                {
                    lblSuccess.Text = "Successfully upload and process data " + Path.GetFileName(filePath) + ", " + message;
                    this.masterGrid.DataBindSafe();

                    //e.CallbackData = lblSuccess.Text;
                    //e.IsValid = true;
                    //e.ErrorText = "";
                }
                else
                {
                    if(uploader.ErrorMessage.Length > 0)
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

            //try
            //{
            //    string[] parameters = e.Parameter.Split(';');
            //    Session["Id"] = parameters[0];
            //    CheckListInstanceStep oStep =
            //        CheckListInstanceRepository.RetrieveCurrentInstanceStep(Convert.ToInt32(parameters[0]));
            //    DotWeb.Models.CheckListInstanceInfo oInfo =
            //        CheckListInstanceRepository.RetrieveCurrentInstanceInfoById(
            //            oStep.CheckListInstanceInfoId.GetValueOrDefault());
            //    int ass = Convert.ToInt32(Session["assemblyType"]);
            //    int modelId = ModelRepository.RetrieveModelIdByName(oInfo.Model);
            //    int vpm = Convert.ToInt32(oInfo.PackingMonth);
            //    int variantId = ModelRepository.RetrieveVariantIdByModelIdAndVariantName(modelId, oInfo.Variant);
            //    if (oStep != null)
            //    {
            //        string fileTypeName = FileTypeRepository.RetrieveFileTypeNameById(
            //                                                          oStep.FileTypeId);
            //        popupDocUpload.JSProperties["cpHeaderText"] = "File Upload - " + fileTypeName;

            //        UploaderBase uploader = UploaderHelper.GetUploader(oStep.FileTypeId, ass);
            //        if (uploader.HasExistingData(vpm, modelId, variantId) != 0)
            //        {
            //            lblexist.Text = "File Already exist";
            //            lblFile.Text = uploader.FileName;
            //        }

            //        Session["FileTypeName"] = fileTypeName;
            //        Session["FileTypeId"] = oStep.FileTypeId;
            //        Session["ModelName"] = oInfo.Model;
            //        Session["VariantName"] = oInfo.Variant;
            //        Session["PackingMonth"] = oInfo.PackingMonth;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }

        protected void fileUploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                //string fileType = Session["FileTypeName"].ToString();
                //int fileTypeId = Session["FileTypeId"].ToInt32(0);

                string filePath = "";
                if (e.IsValid)
                {
                    filePath = SavePostedFile(e.UploadedFile, "DPRUpload");
                }

                Session["pl_uploaded_file"] = filePath;
            }
            catch(Exception ex)
            {
                e.ErrorText = ex.Message + " " + ex.StackTrace;
                e.IsValid = false;
                e.CallbackData = "Upload file failed. " + ex.Message;
                LoggerHelper.LogError(ex);
                LoggerHelper.LogError(e.ErrorText);
            }
        }

        private string SavePostedFile(UploadedFile eUploadedFile, string fileTypeName)
        {
            if (!eUploadedFile.IsValid) return String.Empty;

            //=========cek folder Production Month
            string path = ConfigurationHelper.UPLOAD_DIR + "/" + fileTypeName + "/" + Session["ModelName"];
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }

            //=========cek folder Model
            string pathModel = path + "/" + Session["ProductionMonth"];
            if(!Directory.Exists(Server.MapPath(pathModel)))
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

        #endregion
    }
}