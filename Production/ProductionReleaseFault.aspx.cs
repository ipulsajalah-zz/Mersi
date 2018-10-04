using DevExpress.Utils;
using DevExpress.Web;
//using DevExpress.XtraGauges.Core.Drawing;
using DotWeb.Models;
using DotWeb.Repositories;
using DotWeb.Utils;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{
    public partial class ProductionReleaseFault : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        List<ProductionFaultReleaseAttachmentModel> AttachFileModelList = new List<ProductionFaultReleaseAttachmentModel>();
        ProductionReleaseFaultModel ListAttachment = new ProductionReleaseFaultModel();
        // List<DocumentAttachment> AttachFileModelList = new List<DocumentAttachment>();

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadAttachment.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE_BYTES;
            UploadAttachment.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            UploadAttachment.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");

            if (!IsPostBack)
            {
                var faults = FaultRepository.GetReleaseFaultRecord();
                gridFault.DataSource = faults;
                gridFault.DataBind();
                //if (Session["dtAttachment"] != null)
                //{
                //    GVDataAttachment.DataSource = Session["dtAttachment"];
                //    GVDataAttachment.DataBind();
                //}

            }
            PopUpAttachMent.ShowOnPageLoad = false;

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
            senderGridView.DataSource = FaultRepository.GetReleaseFaultRecord();
        }

        protected void gridFault_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView faultGridView = (ASPxGridView)sender;
            var faults = FaultRepository.GetReleaseFaultRecord();
            faultGridView.DataSource = faults;

            (faultGridView.Columns["FaultStatus"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = Enum.GetValues(typeof(EFaultStatus));
        }
        protected void btnFault_Click(object sender, EventArgs e)
        {
            Session["FINNumber"] = "-Select-";
            Session["CSType"] = "-Select-";
            HiddenText["CSType"] = "-Select-";
            HiddenText["FINNumber"] = "-Select-";
            pcInteruptFault.ShowOnPageLoad = true;
            loadFaultDefault();
        }
        protected void pcInteruptFault_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            // loadFaultDefault();
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
            PopUpAttachMent.ShowOnPageLoad = false;
        }
        //binding & set default form Fault
        private void loadFaultDefault()
        {
            HiddenText["Edit"] = "0";
            Session["DtAttachmentEdit"] = null;
            Session["dtAttachment"] = null;
            GVDataAttachment.DataSource = null;
            GVDataAttachment.DataBind();
            currentdate.Date = DateTime.Now;
            cbStation.SelectedIndex = 0;
            cbQG.SelectedIndex = 0;
            cbFINNumber.Value = 1;
            CSTypeValue.NullText = "-Select-";

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
            //HiddenText["FINNumber"] = cbFINNumber.Text == "1" ? "-Select-" : cbFINNumber.Text;
            if (Session["FINNumber"] != null)
            {
                cbFINNumber.Text = Session["FINNumber"].ToString();
            }
            if (Session["CSType"] != null)
            {
                CSTypeValue.Value = Session["CSType"].ToString();
            }
            GVDataAttachment.DataSource = "";
            //CSTypeValue.Text = "-Select-";
        }


        //binding & set default form Fault
        private void loadFaultDefaultEdit(int id)
        {
            Session["DtAttachmentEdit"] = null;
            HiddenText["Edit"] = "1";
            HiddenText["IdFault"] = id;
            var DataLoad = FaultRepository.GetFaultRecordById(id);
            currentdate.Date = DateTime.Now;
            //GVDataAttachment.DataSource = Session["dtAttachment"];
            //GVDataAttachment.DataBind();
            CSTypeValue.Text = DataLoad[0].CSType.ToString();
            cbStation.Text = DataLoad[0].StationName.ToString();
            cbStation.ReadOnly = true;
            cbStamp.Text = DataLoad[0].StamperName;
            tbRemark.Text = DataLoad[0].Remarks;
            cbQG.Text = DataLoad[0].QualityGateName.ToString();
            cbQG.ReadOnly = true;
            cbFINNumber.Text = DataLoad[0].FINNumber.ToString();
            cbFINNumber.ReadOnly = true;

            cbPartProces.Text = DataLoad[0].FaultPartProcessDesc.ToString();
            cbFaultDesc.Text = DataLoad[0].FaultDescriptionText;
            cbFaultRelated.Text = DataLoad[0].FaultRelatedTypeDesc;
            cbPriority.Text = DataLoad[0].Priority.ToString();
            cbInspector.Text = DataLoad[0].InspectorName;
            cbStamp.Text = DataLoad[0].StamperName;
            cbRecti.Text = DataLoad[0].IsSentToRectification.ToString();
            tbNCP.Text = DataLoad[0].NCP;
            tbRemark.Text = DataLoad[0].Remarks;

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
            var faults = FaultRepository.GetAttachMentViewEdit(id);
            Session["DtAttachmentEdit"] = faults;
            Session["dtAttachment"] = ConvertListToDataTable(faults);
            GVDataAttachment.DataSource = faults;
            GVDataAttachment.DataBind();
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

        protected void cbCGIS_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["StationId"] = cbStation.Value;
            if ((cbFINNumber.Value).ToString() == "-Select-")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Fill VIN/FIN');", true);
            }
            else
            {
                Session["HistoryId"] = cbFINNumber.Value;

                cbCGIS.DataSource = sdsCgis;
                cbCGIS.DataBind();
                cbCGIS.SelectedIndex = 0;
            }
        }

        protected void btnRecordNew_Click(object sender, EventArgs e)
        {
            //Session["FINNumber"] = "-Select-";
            //Session["CSType"] = "-Select-";
            //HiddenText["CSType"] = "-Select-";
            //HiddenText["FINNumber"] = "-Select-";
            //var fault = faultRecord();

            //is exist in fault record
            if (HiddenText["Edit"].ToString() == "0")
            {
                var fault = faultRecord();

                if (fault.FaultRecordModels.Remarks != "MandatoryEmpty")
                {

                    if (fault.FaultRecordModels.Remarks != "MandatoryEmpty2")
                    {
                        var GetIdFault = FaultRepository.InsertFaultRecordReleaseDynamic(fault.FaultRecordModels);

                        for (int x = 0; x < fault.AttachmentModels.Count(); x++)
                        {
                            var id = cbFINNumber.Text.Split('/')[1].Trim();
                            FaultRepository.InsertAttachMent(fault.AttachmentModels[x].FileName, fault.AttachmentModels[x].FilePath, id, GetIdFault);
                        }
                        //add to fault record

                        //close popUpControl Fault
                        // pcInteruptFault.ShowOnPageLoad = false;
                        //binding again grid Fault
                        gridFault.DataBind();
                        // detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
                        //}
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Fill FinNumber');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage4", @"alert('Please Fill CSType');", true);
                }

                Response.Redirect("../Production/ProductionReleaseFault.aspx");
            }
            else
            {
                var model = new FaultRecord();
                int IdFault = int.Parse(HiddenText["IdFault"].ToString());
                model.FaultRecordId = IdFault;
                model.FaultStatus = faultStatus.Text == "" ? 0 : int.Parse(faultStatus.SelectedItem.Value.ToString());
                model.Remarks = tbRemark.Text.ToString();
                model.Priority = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.Value.ToString());
                model.FaultDescriptionId = cbFaultDesc.SelectedItem == null ? 0 : int.Parse(cbFaultDesc.Value.ToString());
                model.FaultRelatedTypeId = cbFaultRelated.SelectedItem == null ? 0 : int.Parse(cbFaultRelated.Value.ToString());
                FaultRepository.UpdateFaultRecord(model);
                FaultRepository.Delete(IdFault);
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
                    var Fin = cbFINNumber.Text;
                    FaultRepository.InsertAttachMent(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, Fin, IdFault);
                }
                //pcInteruptFault.ShowOnPageLoad = false;
                //pcInteruptFaultx.ShowOnPageLoad = false;
                Response.Redirect("../Production/ProductionReleaseFault.aspx");
            }
            loadFaultDefault();
            //cbFINNumber.SelectedItem.Text = "-Select-";
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (HiddenText["Edit"].ToString() == "0")
            {
                var fault = faultRecord();

                if (fault.FaultRecordModels.Remarks != "MandatoryEmpty")
                {

                    if (fault.FaultRecordModels.Remarks != "MandatoryEmpty2")
                    {
                        var GetIdFault = FaultRepository.InsertFaultRecordReleaseDynamic(fault.FaultRecordModels);

                        for (int x = 0; x < fault.AttachmentModels.Count(); x++)
                        {
                            var id = cbFINNumber.Text.Split('/')[1].Trim();
                            FaultRepository.InsertAttachMent(fault.AttachmentModels[x].FileName, fault.AttachmentModels[x].FilePath, id, GetIdFault);
                        }
                        //add to fault record

                        //close popUpControl Fault
                        pcInteruptFault.ShowOnPageLoad = false;
                        //binding again grid Fault
                        gridFault.DataBind();
                        // detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
                        //}
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Fill FinNumber');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage4", @"alert('Please Fill CSType');", true);
                }

                Response.Redirect("../Production/ProductionReleaseFault.aspx");
            }
            else
            {
                var model = new FaultRecord();
                int IdFault = int.Parse(HiddenText["IdFault"].ToString());
                model.FaultRecordId = IdFault;
                model.FaultStatus = faultStatus.Text == "" ? 0 : int.Parse(faultStatus.SelectedItem.Value.ToString());
                model.Remarks = tbRemark.Text.ToString();
                model.Priority = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.Value.ToString());
                model.FaultDescriptionId = cbFaultDesc.SelectedItem == null ? 0 : int.Parse(cbFaultDesc.Value.ToString());
                model.FaultRelatedTypeId = cbFaultRelated.SelectedItem == null ? 0 : int.Parse(cbFaultRelated.Value.ToString());
                FaultRepository.UpdateFaultRecord(model);
                FaultRepository.Delete(IdFault);
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
                    var Fin = cbFINNumber.Text;
                    FaultRepository.InsertAttachMent(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, Fin, IdFault);
                }
                pcInteruptFault.ShowOnPageLoad = false;
                pcInteruptFaultx.ShowOnPageLoad = false;
                Response.Redirect("../Production/ProductionReleaseFault.aspx");
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = false;
            pcInteruptFaultx.ShowOnPageLoad = false;
            gridFault.DataBind();
            //detailCondition.ImageUrl = FaultRepository.IsHaveFault(_historyId) ? urlImgNotifRed : urlImgNotifGreen;
        }


        public ProductionReleaseFaultModel faultRecord()
        {
            var finnumber = "";
            if (CSTypeValue.Text.ToString() != "-Select-")
            {
                int csType = CSTypeValue.Text.ToString() == "" ? 0 : int.Parse(CSTypeValue.SelectedItem.Value.ToString());
            }
            else
            {
                FaultRecord FaultInput = new FaultRecord();
                //FaultInput.CSType = CSTypeValue.Text == "" ? 0 : int.Parse(CSTypeValue.SelectedItem.Value.ToString());
                FaultInput.Remarks = "MandatoryEmpty";
                ProductionReleaseFaultModel InsertFaultRecordFault = new ProductionReleaseFaultModel();
                {
                    InsertFaultRecordFault.FaultRecordModels = FaultInput;
                };
                return InsertFaultRecordFault;
            }
            var ncp = tbNCP.Text;
            // int productionHistoriesId = cbFINNumber.Value == null ? 0 : Convert.ToInt32(cbFINNumber.SelectedItem.Text);
            int productionHistoriesId = 0;
            var faultNumber = "32054";
            int QGate = Convert.ToInt32(cbQG.Value);
            int station = Convert.ToInt32(cbStation.Value);
            ListEditItem selectedItem = cbFINNumber.SelectedItem;

            if (cbFINNumber.Text.ToString() == "-Select-")
            {
                FaultRecord FaultInputfailed = new FaultRecord();
                FaultInputfailed.CSType = CSTypeValue.Text == "" ? 0 : int.Parse(CSTypeValue.SelectedItem.Value.ToString());
                FaultInputfailed.Remarks = "MandatoryEmpty2";
                ProductionReleaseFaultModel InsertFaultRecordFaultfailed = new ProductionReleaseFaultModel();
                {
                    InsertFaultRecordFaultfailed.FaultRecordModels = FaultInputfailed;
                };
                return InsertFaultRecordFaultfailed;
            }
            else
            {
                // finnumber = cbFINNumber.Text == "1" ? Session["FINNumber"].ToString() : cbFINNumber.Text;
                finnumber = selectedItem.GetValue("FINNumber").ToString();
            }

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
            // var stampName = cbStamp.SelectedItem;
            var stampName = "";
            var remark = tbRemark.Text;
            bool recti = Convert.ToBoolean(cbRecti.Value.ToInt32());

            FaultRecord DataFault = new FaultRecord();
            DataFault.CSType = CSTypeValue.Text == "-Select-" ? 0 : int.Parse(CSTypeValue.SelectedItem.Value.ToString());
            DataFault.NCP = ncp;
            DataFault.ProductionHistoriesId = productionHistoriesId;
            DataFault.FaultNumber = faultNumber;
            DataFault.QualityGateId = QGate;
            DataFault.AssemblySectionId = assemblySectionId;
            DataFault.ProductionLinesId = productionLineId;
            DataFault.StationId = station;
            DataFault.FINNumber = finnumber.ToString();
            DataFault.CGISId = cgisId;
            DataFault.Priority = priority;
            DataFault.FaultPartProcessId = partProces;
            DataFault.FaultRelatedTypeId = faultRelated;
            DataFault.FaultDescriptionId = faultDescId;
            DataFault.FaultDescriptionText = faultDescText;
            DataFault.InspectorUserId = inspectorId == null ? 0 : inspectorId;
            DataFault.InspectorName = inspectorName.ToString();
            DataFault.StamperUserID = stampId;
            DataFault.StamperName = stampName.ToString();
            DataFault.Remarks = remark;
            DataFault.IsSentToRectification = recti;
            DataFault.UserAuth = "";


            var SumRowData = GVDataAttachment.VisibleRowCount;
            List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
            for (int i = 0; i < SumRowData; i++)
            {
                DocumentAttachment AttachFile = new DocumentAttachment();
                AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                ListAttachFile.Add(AttachFile);
            }

            ProductionReleaseFaultModel InsertFaultRecord = new ProductionReleaseFaultModel();
            {
                InsertFaultRecord.FaultRecordModels = DataFault;
                InsertFaultRecord.AttachmentModels = ListAttachFile;
            };
            return InsertFaultRecord;










        }
        protected int GetIdAttachment()
        {
            int IdData = 0;
            var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
            IdData = Id;
            return IdData;
        }
        protected void uplDocumentWI_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (HiddenText["Edit"].ToString() == "1")
            {
                string HiddenCstype = CSTypeValuex.Text;
                string x = cbFINNumber.Text;
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
                if (Session["DtAttachmentEdit"] != null)
                {
                    var oldattachImage = Session["DtAttachmentEdit"] as List<ProductionFaultReleaseAttachmentModel>;
                    if (oldattachImage.Count > 0)
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
                            Session["DtAttachmentEdit"] = null;
                        }
                    }
                }
                ProductionFaultReleaseAttachmentModel NewData = new ProductionFaultReleaseAttachmentModel();
                NewData.Id = GetIdAttachment();
                NewData.FileName = resultFileName;
                NewData.FileLocation = resultFileUrl;
                NewData.FileType = resultExtension;
                AttachFileModelList.Add(NewData);


                DataTable table = ConvertListToDataTable(AttachFileModelList);
                Session["dtAttachment"] = table;
                //Session["DtAttachmentEdit"] = AttachFileModelList;
                //cbFINNumber.Text = HiddenText["FINNumber"].ToString();
            }
            else
            {
                string HiddenCstype = HiddenText["CSType"].ToString();
                string x = HiddenText["FINNumber"].ToString().Split('/')[1].Trim();
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

                ProductionFaultReleaseAttachmentModel AttachFile = new ProductionFaultReleaseAttachmentModel();
                AttachFile.Id = GetIdAttachment();
                AttachFile.FileName = resultFileName;
                AttachFile.FileLocation = resultFileUrl;
                AttachFile.FileType = resultExtension;
                AttachFileModelList.Add(AttachFile);
                ListAttachment.ProductionFaultReleaseAttachmentModelList = AttachFileModelList;
                DataTable table = ConvertListToDataTable(AttachFileModelList);
                Session["dtAttachment"] = table;
                cbFINNumber.Text = HiddenText["FINNumber"].ToString();
            }


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

        protected void ShowAttachment_Click(object sender, EventArgs e)
        {
            if (HiddenText["Edit"].ToString() == "0")
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
            else
            {
                PopUpAttachMent.ShowOnPageLoad = true;
            }
        }

        protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            pcInteruptFault.ShowOnPageLoad = false;

            PopUpAttachMent.ShowOnPageLoad = false;
            pcInteruptFault.ShowOnPageLoad = true;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();

        }

        protected void Btn_ShowAttach_Click(object sender, EventArgs e)
        {
            PopUpViewDocument.ShowOnPageLoad = true;
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            var FinNumber = gridFault.GetRowValues(index, "FINNumber");
            string FinParam = FinNumber.ToString();
            var faults = FaultRepository.GetAttachMentView(FinParam);
            GvAttachMentView.DataSource = faults;
            GvAttachMentView.DataBind();
        }

        protected void gridFault_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //ASPxGridView grv = (sender as ASPxGridView);
            //if (grv.GetRowValues(e.VisibleIndex, "FaultStatus").ToString() == "Open")
            //{
            //    Color tlr = ARGBColorTranslator.FromHtml("#ff8080");
            //    e.Row.BackColor = tlr;

            //}
            //else
            //{
            //    Color tlr = ARGBColorTranslator.FromHtml("#44C169");
            //    e.Row.BackColor = tlr;
            //}
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

        protected void btnSaveCloseEdit_Click(object sender, EventArgs e)
        {
            var model = new FaultRecord();
            model.FaultRecordId = int.Parse(HiddenText["IdFault"].ToString());
            model.FaultStatus = FaultStatusx.Text == "" ? 0 : int.Parse(FaultStatusx.SelectedItem.Value.ToString());
            model.Remarks = tbRemarkx.ToString();
            model.Priority = cbPriorityx.SelectedItem == null ? 0 : int.Parse(cbPriorityx.Value.ToString());
            model.FaultDescriptionId = cbFaultDescx.SelectedItem == null ? 0 : int.Parse(cbFaultDescx.Value.ToString());
            model.FaultRelatedTypeId = cbFaultRelatedx.SelectedItem == null ? 0 : int.Parse(cbFaultRelatedx.Value.ToString());
            FaultRepository.UpdateFaultRecord(model);


            pcInteruptFault.ShowOnPageLoad = false;
            pcInteruptFaultx.ShowOnPageLoad = false;
            Response.Redirect("../Production/ProductionReleaseFault.aspx");
        }

        protected void pcInteruptFault_DataBinding(object sender, EventArgs e)
        {
            if (Session["FINNumber"] != null)
            {
                cbFINNumber.Text = HiddenText["FINNumber"].ToString();
            }
        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            pcInteruptFaultx.ShowOnPageLoad = true;
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            int ValueId = int.Parse(gridFault.GetRowValues(index, "FaultRecordId").ToString());
            try
            {
                FaultRepository.DeleteFault(ValueId);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void gridFault_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "Btn_Delete")
            {
                if (!permissions.Contains(Ebiz.Scaffolding.Models.EPermissionType.Delete))
                {
                    e.Visible = DefaultBoolean.False;
                }
            }
            else if (e.ButtonID == "EditDetails")
            {
                if (!permissions.Contains(EPermissionType.Update))
                {
                    e.Visible = DefaultBoolean.False;
                }
            }
        }

        protected void UploadAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

        }

        protected void IdBtndelete_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var Rowindex = container.VisibleIndex;
            int ValueRowId = int.Parse(GVDataAttachment.GetRowValues(Rowindex, "Id").ToString());
            //var Data = Session["DataDetail"];
            var oldattachImage = Session["dtAttachment"] as List<ProductionFaultReleaseAttachmentModel>;
            DataTable Datax = Session["dtAttachment"] as DataTable;
            //List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
            //List<DataRow> list = Datax.AsEnumerable().ToList();
            //int Id = int.Parse(Datax.Rows[0].ItemArray[0].ToString());
            for (int i = 0; i < Datax.Rows.Count; i++)
            {
                int Id = int.Parse(Datax.Rows[i].ItemArray[0].ToString());
                if (Id == ValueRowId)
                {
                    DataRow dr = Datax.Rows[i];
                    dr.Delete();
                    // dr.AcceptChanges();
                }
            }


            DataTable DataNew = new DataTable();
            DataNew = Datax;
            GVDataAttachment.DataSource = DataNew;
            Session["dtAttachment"] = DataNew;
            GVDataAttachment.DataBind();

        }



    }
}