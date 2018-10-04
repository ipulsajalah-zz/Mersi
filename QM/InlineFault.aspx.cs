using DevExpress.Web;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Tids.CV.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.QM
{
    public partial class InlineFault : ListPageBase
    {
        protected static string UploadDirectory = AppConfiguration.UPLOAD_DIR + "/Qm";
        QmAttachmentsModels ListAttachment = new QmAttachmentsModels();
        List<FaultAuditRecordAttachment> AttachFileModelList = new List<FaultAuditRecordAttachment>();
        List<FaultAuditRecordAttachment> AttachFileModelListRC = new List<FaultAuditRecordAttachment>();
        List<FaultAuditRecordAttachment> AttachFileModelListCA = new List<FaultAuditRecordAttachment>();
        List<FaultAuditRecordAttachment> AttachFileModelListME = new List<FaultAuditRecordAttachment>();

        protected static string tableName = "FaultAuditsInlineFault";

        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta form Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            tableMeta.AdditionalFilters = string.Format("IsCpa = 'False'");
        
            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</2>", "Invalid Page")));
                return false;
                      
            }
            Session["TableName"] = tableName;

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {

            return true;
        }

        protected void popupReport_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            var temp = e.Parameter.ToString().Split(";");
            int id = temp[1].ToInt32(0);

            if (id == 0)
                return;

            Session["FaultAuditRecordsId"] = id;
            frmLayoutCheckingCode.DataBind();
            gvIssue.DataBind();

            QmRcaModels rc = new QmRcaModels();
            rc = QmRepository.GetRca(id);

            TbMan1.Text = rc.Man1;
            TbMan2.Text = rc.Man2;
            TbMan3.Text = rc.Man3;
            TbMan4.Text = rc.Man4;
            TbMan5.Text = rc.Man5;

            TbMachine1.Text = rc.Machine1;
            TbMachine2.Text = rc.Machine2;
            TbMachine3.Text = rc.Machine3;
            TbMachine4.Text = rc.Machine4;
            TbMachine5.Text = rc.Machine5;

            TbMaterial1.Text = rc.Material1;
            TbMaterial2.Text = rc.Material2;
            TbMaterial3.Text = rc.Material3;
            TbMaterial4.Text = rc.Material4;
            TbMaterial5.Text = rc.Material5;

            TbMethod1.Text = rc.Method1;
            TbMethod2.Text = rc.Method2;
            TbMethod3.Text = rc.Method3;
            TbMethod4.Text = rc.Method4;
            TbMethod5.Text = rc.Method5;

            //containment
            AttachFileModelList = QmRepository.GetAttachmentViewEditIA(id, 1);
            Session["Attachment"] = AttachFileModelList;
            GvContainmentAction.DataSource = AttachFileModelList;
            GvContainmentAction.DataBind();
           // AttachFileModelList = GetFileAttachment;


            //GvRootCauseAnalysis
            AttachFileModelListRC = QmRepository.GetAttachmentViewEditIA(id, 2);
            Session["AttachmentRC"] = AttachFileModelListRC;
            GvRootCauseAnalysis.DataSource = AttachFileModelListRC;
            GvRootCauseAnalysis.DataBind();

            //GvCorrectiveAction
            var AttachFileModelListCA = QmRepository.GetAttachmentViewEditIA(id, 3);
            Session["AttachmentCA"] = AttachFileModelListCA;
            GvCorrectiveAction.DataSource = AttachFileModelListCA;
            GvCorrectiveAction.DataBind();


            //GvMonitoringEffectiveness
            AttachFileModelListME = QmRepository.GetAttachmentViewEditIA(id, 4);
            Session["AttachmentME"] = AttachFileModelListME;
            GvMonitoringEffectiveness.DataSource = AttachFileModelListME;
            GvMonitoringEffectiveness.DataBind();
        }
        protected void gvIssue_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var user = HttpContext.Current.Session["user"] as User;
            e.NewValues["ModifiedBy"] = user.UserName;
        }

        protected void gvIssue_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var user = HttpContext.Current.Session["user"] as User;
            e.NewValues["CreatedBy"] = user.UserName;
        }
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {

            try
            {
                ASPxButton btn = (ASPxButton)sender;
                var user = HttpContext.Current.Session["user"] as User;

                ASPxFormLayout frm = (ASPxFormLayout)btn.NamingContainer;
                int FaultAuditRecordsId = Session["FaultAuditRecordsId"].ToInt32(0);
                string ContainmentAction = frm.GetNestedControlValueByFieldName("ContainmentAction").ToString("");
                string RootCauseAnalysis = frm.GetNestedControlValueByFieldName("RootCauseAnalysis").ToString("");
                string CorrectiveAction = frm.GetNestedControlValueByFieldName("CorrectiveAction").ToString("");
                string MonitoringEffectiveness = frm.GetNestedControlValueByFieldName("MonitoringEffectiveness").ToString("");

                QmRepository.UpdateAction(FaultAuditRecordsId, ContainmentAction, RootCauseAnalysis, CorrectiveAction, MonitoringEffectiveness,"", user.UserName);
                
                var AttachFileModelList = (List<FaultAuditRecordAttachment>)Session["Attachment"];
                var AttachFileModelListRC= (List<FaultAuditRecordAttachment>)Session["AttachmentRC"];
                var AttachFileModelListCA = (List<FaultAuditRecordAttachment>)Session["AttachmentCA"];
                var AttachFileModelListME = (List<FaultAuditRecordAttachment>)Session["AttachmentME"];


                for (int x = 0; x < AttachFileModelList.Count(); x++)
                {
                    QmRepository.InsertAttachMent(AttachFileModelList[x].FileName, AttachFileModelList[x].FileLocation, 1, FaultAuditRecordsId,user.UserName);
                }

                for (int x = 0; x < AttachFileModelListRC.Count(); x++)
                {
                    QmRepository.InsertAttachMent(AttachFileModelListRC[x].FileName, AttachFileModelListRC[x].FileLocation, 2, FaultAuditRecordsId, user.UserName);
                }

                for (int x = 0; x < AttachFileModelListCA.Count(); x++)
                {
                    QmRepository.InsertAttachMent(AttachFileModelListCA[x].FileName, AttachFileModelListCA[x].FileLocation, 3, FaultAuditRecordsId, user.UserName);
                }

                for (int x = 0; x < AttachFileModelListME.Count(); x++)
                {
                    QmRepository.InsertAttachMent(AttachFileModelListME[x].FileName, AttachFileModelListME[x].FileLocation, 4, FaultAuditRecordsId, user.UserName);
                }


            }
            catch (Exception ex)
            {
                //errorMessageLabel.Text = ex.Message;
                //errorMessageLabel.Visible = true;
            }
        }

        protected void dsAction_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
           // DevExpress.XtraSplashScreen.Utils.AssertNotReadOnly();
        }

        protected void btnSaveRca_Click(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;

            GridViewDataRowTemplateContainer container = btn.NamingContainer as GridViewDataRowTemplateContainer;
            ASPxPageControl ASPxPageControl = btn.NamingContainer as ASPxPageControl;
            var FaultAuditRecordsId = Session["FaultAuditRecordsId"].ToInt32(0);
            var user = HttpContext.Current.Session["user"] as User;

            TextBox TbMan1 = ASPxPageControl.FindControl("TbMan1") as TextBox;
            TextBox TbMan2 = ASPxPageControl.FindControl("TbMan2") as TextBox;
            TextBox TbMan3 = ASPxPageControl.FindControl("TbMan3") as TextBox;
            TextBox TbMan4 = ASPxPageControl.FindControl("TbMan4") as TextBox;
            TextBox TbMan5 = ASPxPageControl.FindControl("TbMan5") as TextBox;

            TextBox TbMachine1 = ASPxPageControl.FindControl("TbMachine1") as TextBox;
            TextBox TbMachine2 = ASPxPageControl.FindControl("TbMachine2") as TextBox;
            TextBox TbMachine3 = ASPxPageControl.FindControl("TbMachine3") as TextBox;
            TextBox TbMachine4 = ASPxPageControl.FindControl("TbMachine4") as TextBox;
            TextBox TbMachine5 = ASPxPageControl.FindControl("TbMachine5") as TextBox;

            TextBox TbMaterial1 = ASPxPageControl.FindControl("TbMaterial1") as TextBox;
            TextBox TbMaterial2 = ASPxPageControl.FindControl("TbMaterial2") as TextBox;
            TextBox TbMaterial3 = ASPxPageControl.FindControl("TbMaterial3") as TextBox;
            TextBox TbMaterial4 = ASPxPageControl.FindControl("TbMaterial4") as TextBox;
            TextBox TbMaterial5 = ASPxPageControl.FindControl("TbMaterial5") as TextBox;

            TextBox TbMethod1 = ASPxPageControl.FindControl("TbMethod1") as TextBox;
            TextBox TbMethod2 = ASPxPageControl.FindControl("TbMethod2") as TextBox;
            TextBox TbMethod3 = ASPxPageControl.FindControl("TbMethod3") as TextBox;
            TextBox TbMethod4 = ASPxPageControl.FindControl("TbMethod4") as TextBox;
            TextBox TbMethod5 = ASPxPageControl.FindControl("TbMethod5") as TextBox;


            QmRepository.UpdateRca(FaultAuditRecordsId.ToInt32(0), TbMan1.Text, TbMan2.Text, TbMan3.Text, TbMan4.Text, TbMan5.Text,
                                                               TbMachine1.Text, TbMachine2.Text, TbMachine3.Text, TbMachine4.Text, TbMachine5.Text,
                                                               TbMaterial1.Text, TbMaterial2.Text, TbMaterial3.Text, TbMaterial4.Text, TbMaterial5.Text,
                                                               TbMethod1.Text, TbMethod2.Text, TbMethod3.Text, TbMethod4.Text, TbMethod5.Text, user.UserName);
        }

        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json)]  
        [System.Web.Services.WebMethod]

        public static string SetDamageCode(string FaultConstructionGroupId, string FaultLocationId, string FaultTypeId, string FaultDamageTypeId)
        {

            string _FaultConstructionGroupId = "";
            string _FaultLocationId = "";
            string _FaultTypeId = "";
            string _FaultDamageTypeId = "";

            if (!string.IsNullOrEmpty(FaultConstructionGroupId))
            {
              _FaultConstructionGroupId =  FaultConstructionGroupId.Split("-")[0].ToString("");
            }

            if (!string.IsNullOrEmpty(FaultLocationId))
            {
                _FaultLocationId = FaultLocationId.Split("-")[0].ToString("");
            }
            if (!string.IsNullOrEmpty(FaultTypeId))
            {
               _FaultTypeId = FaultTypeId.Split("-")[0].ToString("");
            }
            if (!string.IsNullOrEmpty(FaultDamageTypeId))
            {
                 _FaultDamageTypeId = QmRepository.GetDamageCode(FaultDamageTypeId.Split("-")[0]);
            }
           

            Object o = new
            {
                damagecode = string.Format("Q{0}{1}{2}{3}",_FaultConstructionGroupId,_FaultLocationId,_FaultTypeId,_FaultDamageTypeId),
                classes = "1",
                weight = "1"
            };


            return new JavaScriptSerializer().Serialize(o);  
        }

        #region uploadContainmentAction
        protected string SaveToUploadFolder(int FaultAuditRecordId, string filepath, string filename)
        {
            string tempdir = Server.MapPath(UploadDirectory);
            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            string tempfile = String.Format("{0}/{1}_{2}_{3}", tempdir, FaultAuditRecordId, DateTime.Now.ToString("yyyyMMddhhmmss"), filename);
            if (File.Exists(tempfile))
            {
                //somehow, file already exist
                LoggerHelper.Info("File exist: " + tempfile);
                return null;
            }

            try
            {
                File.Move(filepath, tempfile);
            }
            catch (Exception e)
            {
                //ignore
                LoggerHelper.LogError(e);
                return null;
            }

            return tempfile.Replace(HttpContext.Current.Server.MapPath("~/"), "~/").Replace(@"\", "/");
        }

        static DataTable ConvertListToDataTable(List<FaultAuditRecordAttachment> list)
        {
            // New table.
            DataTable table = new DataTable();
            // Get max columns.
            int columns = list.Count();
            // Add columns.
            table.Columns.Add("Id");
            table.Columns.Add("FileName");
            table.Columns.Add("FileLocation");
            table.Columns.Add("FaultAuditRecordsId");
            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array.Id, array.FileName, array.FileLocation, array.FaultAuditRecordId);
            }
            return table;
        }

        protected int GetIdAttachment()
        {
            int IdData = 0;
            var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
            IdData = Id;
            return IdData;
        }

        protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            PopUpAttachMent.ShowOnPageLoad = false;
        }
        protected void UploadAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            int id = Session["FaultAuditRecordsId"].ToInt32(0);

            string PathName = AppConfiguration.UPLOAD_DIR + "/Qm/" + id.ToString("") + "/";
            if (!Directory.Exists(Server.MapPath(PathName)))
            {
                Directory.CreateDirectory(Server.MapPath(PathName));
            }
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = e.UploadedFile.FileName;
            string resultFileUrl = PathName + "/" + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);

            string name = e.UploadedFile.FileName;
            string url = ResolveClientUrl(resultFileUrl);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;


            //if (Session["Attachment"] == null)
            //{

            //var tem = Session["Attachment"];
            AttachFileModelList = (List<FaultAuditRecordAttachment>)Session["Attachment"];

            FaultAuditRecordAttachment AttachFile = new FaultAuditRecordAttachment();
            AttachFile.Id = GetIdAttachment();
            AttachFile.FileName = resultFileName;
            AttachFile.FileLocation = resultFileUrl;
            AttachFile.FaultAuditRecordId = id;
            AttachFileModelList.Add(AttachFile);

            DataTable table = ConvertListToDataTable(AttachFileModelList);
            Session["dtAttachment"] = table;
            Session["Attachment"] = AttachFileModelList;

            PopUpAttachMent.ShowOnPageLoad = false;


        }

        protected void GvContainmentAction_Load(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.DataSource = Session["Attachment"];
            grid.DataBind();
        }

        protected void GvContainmentAction_Init(object sender, EventArgs e)
        {
            ASPxGridView detgrid = sender as ASPxGridView;
            if (detgrid == null) return;

            detgrid.DataSource = Session["Attachment"];
            detgrid.DataBind();

        }

        protected void GvContainmentAction_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                int id = e.Keys[0].ToInt32(0);
                int FaultAuditRecordsId = Session["FaultAuditRecordsId"].ToInt32(0);
         
                QmRepository.DeleteAttachMent(id);

                AttachFileModelList = QmRepository.GetAttachmentViewEditIA(FaultAuditRecordsId, 1);
                Session["Attachment"] = AttachFileModelList;
                GvContainmentAction.DataSource = AttachFileModelList;
                GvContainmentAction.DataBind();
                
            }
            catch (Exception ex)
            {
                //PanelMensajes.ShowOnPageLoad = true;
                //LblMensaje.Text = ex.Message;
            }
            finally
            {
                e.Cancel = true;
            }
        }

        #endregion uploadContainmentAction
        
        #region uploadRootCauseAnalysis

        protected void PopUpAttachMentRootCauseAnalysis_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

        }

        protected void UploadRootCauseAnalysis_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            int id = Session["FaultAuditRecordsId"].ToInt32(0);

            string PathName = AppConfiguration.UPLOAD_DIR + "/Qm/" + id.ToString("") + "/";
            if (!Directory.Exists(Server.MapPath(PathName)))
            {
                Directory.CreateDirectory(Server.MapPath(PathName));
            }
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = e.UploadedFile.FileName;
            string resultFileUrl = PathName + "/" + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);

            string name = e.UploadedFile.FileName;
            string url = ResolveClientUrl(resultFileUrl);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;


            AttachFileModelListRC = (List<FaultAuditRecordAttachment>)Session["AttachmentRC"];

            FaultAuditRecordAttachment AttachFile = new FaultAuditRecordAttachment();
            AttachFile.Id = GetIdAttachment();
            AttachFile.FileName = resultFileName;
            AttachFile.FileLocation = resultFileUrl;
            AttachFile.FaultAuditRecordId = id;
            AttachFileModelListRC.Add(AttachFile);

            DataTable table = ConvertListToDataTable(AttachFileModelListRC);
            Session["dtAttachmentRC"] = table;
            Session["AttachmentRC"] = AttachFileModelListRC;

            PopUpAttachMentRootCauseAnalysis.ShowOnPageLoad = false;
            //Popup

        }


        protected void GvRootCauseAnalysis_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.DataSource = Session["AttachmentRC"];
            grid.DataBind();
        }

        protected void GvRootCauseAnalysis_Load(object sender, EventArgs e)
        {
            ASPxGridView detgrid = sender as ASPxGridView;
            detgrid.DataSource = Session["AttachmentRC"];
            detgrid.DataBind();

        }


        protected void GvRootCauseAnalysis_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                int id = e.Keys[0].ToInt32(0);
                int FaultAuditRecordsId = Session["FaultAuditRecordsId"].ToInt32(0);

                QmRepository.DeleteAttachMent(id);

                AttachFileModelList = QmRepository.GetAttachmentViewEditIA(FaultAuditRecordsId, 2);
                Session["AttachmentRC"] = AttachFileModelList;
                GvContainmentAction.DataSource = AttachFileModelList;
                GvContainmentAction.DataBind();

            }
            catch (Exception ex)
            {
                //PanelMensajes.ShowOnPageLoad = true;
                //LblMensaje.Text = ex.Message;
            }
            finally
            {
                e.Cancel = true;
            }
        }

        #endregion uploadRootCauseAnalysis

        #region uploadCorrectiveAction

        protected void UploadPopUpAttachMentCorrectiveAction_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            int id = Session["FaultAuditRecordsId"].ToInt32(0);

            string PathName = AppConfiguration.UPLOAD_DIR + "/Qm/" + id.ToString("") + "/";
            if (!Directory.Exists(Server.MapPath(PathName)))
            {
                Directory.CreateDirectory(Server.MapPath(PathName));
            }
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = e.UploadedFile.FileName;
            string resultFileUrl = PathName + "/" + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);

            string name = e.UploadedFile.FileName;
            string url = ResolveClientUrl(resultFileUrl);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;


            AttachFileModelListCA = (List<FaultAuditRecordAttachment>)Session["AttachmentCA"];

            FaultAuditRecordAttachment AttachFile = new FaultAuditRecordAttachment();
            AttachFile.Id = GetIdAttachment();
            AttachFile.FileName = resultFileName;
            AttachFile.FileLocation = resultFileUrl;
            AttachFile.FaultAuditRecordId = id;
            AttachFileModelListCA.Add(AttachFile);

            DataTable table = ConvertListToDataTable(AttachFileModelListCA);
            Session["dtAttachmentCA"] = table;
            Session["AttachmentCA"] = AttachFileModelListCA;

            PopUpAttachMentCorrectiveAction.ShowOnPageLoad = false;
        }

        protected void PopUpAttachMentCorrectiveAction_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
        }

        protected void GvCorrectiveAction_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.DataSource = Session["AttachmentCA"];
            grid.DataBind();
        }

        protected void GvCorrectiveAction_Load(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.DataSource = Session["AttachmentCA"];
            grid.DataBind();
        }
        protected void GvCorrectiveAction_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                int id = e.Keys[0].ToInt32(0);
                int FaultAuditRecordsId = Session["FaultAuditRecordsId"].ToInt32(0);

                QmRepository.DeleteAttachMent(id);

                AttachFileModelList = QmRepository.GetAttachmentViewEditIA(FaultAuditRecordsId, 3);
                Session["AttachmentCA"] = AttachFileModelList;
                GvContainmentAction.DataSource = AttachFileModelList;
                GvContainmentAction.DataBind();

            }
            catch (Exception ex)
            {
                //PanelMensajes.ShowOnPageLoad = true;
                //LblMensaje.Text = ex.Message;
            }
            finally
            {
                e.Cancel = true;
            }
        }


        #endregion uploadCorrectiveAction

        #region uploadMonitoringEffectiveness

        protected void PopUpAttachMentMonitoringEffectiveness_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

        }

        //protected void UploadPopUpAttachMentMonitoringEffectiveness_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        //{
         
        //}

        protected void UploadPopUpAttachMentMonitoringEffectiveness_FileUploadComplete1(object sender, FileUploadCompleteEventArgs e)
        {
            int id = Session["FaultAuditRecordsId"].ToInt32(0);

            string PathName = AppConfiguration.UPLOAD_DIR + "/Qm/" + id.ToString("") + "/";
            if (!Directory.Exists(Server.MapPath(PathName)))
            {
                Directory.CreateDirectory(Server.MapPath(PathName));
            }
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = e.UploadedFile.FileName;
            string resultFileUrl = PathName + "/" + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);

            string name = e.UploadedFile.FileName;
            string url = ResolveClientUrl(resultFileUrl);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;


            AttachFileModelListME = (List<FaultAuditRecordAttachment>)Session["AttachmentME"];

            FaultAuditRecordAttachment AttachFile = new FaultAuditRecordAttachment();
            AttachFile.Id = GetIdAttachment();
            AttachFile.FileName = resultFileName;
            AttachFile.FileLocation = resultFileUrl;
            AttachFile.FaultAuditRecordId = id;
            AttachFileModelListME.Add(AttachFile);

            DataTable table = ConvertListToDataTable(AttachFileModelListME);
            Session["dtAttachmentME"] = table;
            Session["AttachmentME"] = AttachFileModelListME;

            PopUpAttachMentMonitoringEffectiveness.ShowOnPageLoad = false;
        }

        protected void GvMonitoringEffectiveness_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.DataSource = Session["AttachmentME"];
            grid.DataBind();
        }

        protected void GvMonitoringEffectiveness_Load(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.DataSource = Session["AttachmentME"];
            grid.DataBind();
        }

        protected void GvMonitoringEffectiveness_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                int id = e.Keys[0].ToInt32(0);
                int FaultAuditRecordsId = Session["FaultAuditRecordsId"].ToInt32(0);

                QmRepository.DeleteAttachMent(id);

                AttachFileModelList = QmRepository.GetAttachmentViewEditIA(FaultAuditRecordsId, 4);
                Session["AttachmentME"] = AttachFileModelList;
                GvContainmentAction.DataSource = AttachFileModelList;
                GvContainmentAction.DataBind();

            }
            catch (Exception ex)
            {
                //PanelMensajes.ShowOnPageLoad = true;
                //LblMensaje.Text = ex.Message;
            }
            finally
            {
                e.Cancel = true;
            }
        }
    
        #endregion uploadMonitoringEffectiveness


    }
}
    