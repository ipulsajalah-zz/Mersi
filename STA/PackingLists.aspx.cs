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

namespace Ebiz.WebForm.Tids.CV.STA
{
    public partial class PackingLists : ListPageBase
    {
        //Default table name
        protected static string tableName = "PackingLists";

        //private DataTable dataTable;

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
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
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

            Session["TableName"] = tableName;

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
                DateTime value = (DateTime) dtPM.Value;
                int packingMonth = value.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture).ToInt32(0);
                int modelId = cmbModels.Value.ToInt32(0);
                int ricType = cmbRicType.Value.ToInt32(0);

                //make sure RIC is not released yet. If it is already released, can not upload packing list
                RIC ric = RicRepository.GetRIC(modelId, packingMonth, ricType);
                if (ric != null && ric.StatusId == EStatus.Released)
                {
                    lblSuccess.Text = "RIC is already released. Upload file " + Path.GetFileName(filePath) + " failed!";
                    return;
                }

                //parse the file
                string message = "";
                int rowCount = uploader.Parse(filePath, packingMonth, modelId, ricType);
                if (rowCount > 0)
                {
                    lblSuccess.Text = "Successfully upload and process data " + Path.GetFileName(filePath) + ", " + rowCount + " rows inserted, " + message;

                    this.masterGrid.DataBindSafe();
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
                }
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

        #endregion
    }

}