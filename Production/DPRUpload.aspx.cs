using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DotMercy.custom.Uploader;
using DotWeb.Models;
using DotWeb.Repositories;
using OfficeOpenXml;
using DotWeb.Utils;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.Models;

namespace DotMercy.custom
{
    public partial class DPRUpload : System.Web.UI.Page
    {
        private static string filePath = string.Empty;
        private static string fileName = string.Empty;
        private static string folderDPR = "DPR"; //sould put this as config
        private static string prodMonth = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            FileDPRBrowse.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE_BYTES;
            FileDPRBrowse.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            FileDPRBrowse.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");

            ASPxFileManager1.Settings.RootFolder = AppConfiguration.UPLOAD_DIR + "/DPR/";
            ASPxFileManager1.Settings.InitialFolder = AppConfiguration.UPLOAD_DIR + "/DPR/";
            ASPxFileManager1.Settings.ThumbnailFolder = AppConfiguration.TEMPORARY_DIR + "/thumb/";
        }

        protected void FileDPRBrowse_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            filePath = string.Empty;
            fileName = string.Empty;
            FileLabel.Text = string.Empty;
            prodMonth = hdnProdMonth.Value;

            try
            {
                if (e.IsValid)
                {
                    //directory check
                    string path = AppConfiguration.UPLOAD_DIR + "/" + folderDPR + "/" + prodMonth;

                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    string UploadDir = path + "/"; 

                    FileInfo fileInfo = new FileInfo(e.UploadedFile.FileName);
                    String fileNameOri = e.UploadedFile.FileName.ToString().Replace(" ", "_");
                    String ext = System.IO.Path.GetExtension(e.UploadedFile.FileName);

                    if ((fileNameOri.Length - ext.Length) > 45)
                    {
                        fileNameOri = fileNameOri + ext;
                        fileNameOri = fileNameOri.Substring(0, 16).ToLower() + ext;
                    }

                    fileName = fileNameOri;

                    //SaveFile
                    if (!File.Exists(UploadDir + fileName))
                    {
                        filePath = Server.MapPath(UploadDir + fileName);
                        e.UploadedFile.SaveAs(filePath);
                    }

                    e.CallbackData = fileName;
                }
                else
                {
                    throw new Exception("No file to upload");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured : {0}", ex.Message);

                filePath = string.Empty;
                fileName = string.Empty;
                e.CallbackData = string.Empty;
            }
        }

        protected void FileDPRUpload_Click(object sender, EventArgs e)
        {
            FileLabel.Text = string.Empty;
            prodMonth = string.Empty;

            //check month selected or not
            if (!string.IsNullOrWhiteSpace(hdnProdMonth.Value))
            {
                prodMonth = hdnProdMonth.Value;
            }
            else
            {
                //TODO if Production Month Empty!!
                fileName = string.Empty;
                filePath = string.Empty;
                FileLabel.Text = "Please select Production Month.";
                return;
            }

            if (!string.IsNullOrWhiteSpace(filePath) && !string.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    var user = HttpContext.Current.Session["user"] as User;
                    int userId = 0;
                    if (user != null)
                    {
                        userId = user.Id;
                    }
                    //uploadingfile
                    var upload = DPRUploader.UploadFile(prodMonth, fileName, filePath, userId);

                    fileName = string.Empty;
                    filePath = string.Empty;

                    if (upload)
                        FileLabel.Text = "File Uploaded Successfully";
                    else
                        FileLabel.Text = "Error processing uploaded file. Please check your file format.";
                }
                catch (Exception ex)
                {
                    FileLabel.Text = "Error upload file : " + ex.Message;
                }
            }
            else
            {
                FileLabel.Text = "File Cannot be upload, check your file format.";
                return;
            }
        }

        protected void FileDelete_Click(object sender, EventArgs e)
        {
            string StrFilename = "";
            string strPhysicalFolder = Server.MapPath("..\\");

            string strFileFullPath = strPhysicalFolder + StrFilename;

            if (System.IO.File.Exists(strFileFullPath))
            {
                System.IO.File.Delete(strFileFullPath);
            }
        }


        #region creating select period month using DropDownEdit
        protected void dde_Init(object sender, EventArgs e)
        {
            dde_pmonth.DropDownWindowTemplate = new DateTemplate();
        }
        public class DateTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                container.Controls.Add(new LiteralControl("<table  class='tab'"));
                container.Controls.Add(new LiteralControl("<tr>"));

                container.Controls.Add(new LiteralControl("<td class='cell' align='left'>"));
                ASPxButton btPrev = new ASPxButton();
                btPrev.ID = "btPrev";
                container.Controls.Add(btPrev);
                btPrev.RenderMode = ButtonRenderMode.Link;
                btPrev.CssClass = "buttonMonth";
                btPrev.Width = 10;
                btPrev.ClientSideEvents.Click = "OnPrevClick";
                btPrev.AutoPostBack = false;
                btPrev.Text = "<";
                container.Controls.Add(new LiteralControl("</td>"));

                container.Controls.Add(new LiteralControl("<td class='cell' style='text-align: center'>"));
                ASPxLabel label = new ASPxLabel();
                label.ID = "YearLabel";
                container.Controls.Add(label);
                label.Text = DateTime.Now.Year.ToString();
                label.ClientInstanceName = "lblYear";
                container.Controls.Add(new LiteralControl("</td>"));

                container.Controls.Add(new LiteralControl("<td class='cell' align='right'>"));
                ASPxButton btNext = new ASPxButton();
                btNext.ID = "btNext";
                container.Controls.Add(btNext);
                btNext.AutoPostBack = false;
                btNext.RenderMode = ButtonRenderMode.Link;
                btNext.CssClass = "buttonMonth";
                btNext.Width = 10;
                btNext.Text = ">";
                btNext.ClientSideEvents.Click = "OnNextClick";
                container.Controls.Add(new LiteralControl("</td>"));

                container.Controls.Add(new LiteralControl("</tr>"));
                container.Controls.Add(new LiteralControl("</table>"));

                container.Controls.Add(new LiteralControl("<table  class='tab'>"));
                int k = 1;
                for (int i = 0; i < 3; i++)
                {
                    container.Controls.Add(new LiteralControl("<tr>"));
                    for (int j = 0; j < 4; j++)
                    {
                        container.Controls.Add(new LiteralControl("<td class='cell'>"));
                        ASPxButton button = new ASPxButton();
                        button.ID = "btn#" + k;
                        container.Controls.Add(button);
                        button.AutoPostBack = false;
                        button.RenderMode = ButtonRenderMode.Link;
                        button.Width = 50;
                        button.CssClass = "buttonMonth";
                        button.FocusRectBorder.BorderWidth = 1;
                        button.ClientSideEvents.Click = "OnClick";
                        button.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(k);
                        container.Controls.Add(new LiteralControl("</td>"));
                        k++;
                    }
                    container.Controls.Add(new LiteralControl("</tr>"));
                }
                container.Controls.Add(new LiteralControl("</table>"));
            }
        }
        #endregion

        protected void ASPxFileManager1_ItemDeleting(object source, FileManagerItemDeleteEventArgs e)
        {
            //Delete here
            if (File.Exists(e.Item.FullName))
            {
                File.Delete(e.Item.FullName);
            }
        }


    }
}