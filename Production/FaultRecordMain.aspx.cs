
using DevExpress.Utils;
using DevExpress.Web;
using DotWeb.Models;
using DotWeb.Repositories;
using DotWeb.Utils;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{

    public partial class FaultRecordMain : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        public int IdFaultRecord { get; set; }
        ProductionReleaseFaultModel ListAttachment = new ProductionReleaseFaultModel();
        List<FaultAuditAttachmentModel> AttachFileModelList = new List<FaultAuditAttachmentModel>();
        private static string _FinNumber;
        private static string _Id;
        private static string _voca;
        private static string _IdVoca;

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadAttachment.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE_BYTES;
            UploadAttachment.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            UploadAttachment.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");

            if (Request.QueryString.Count > 0)
            {
                _Id = Request.QueryString["Id"];
                _FinNumber = Request.QueryString["FinNumber"];
                _voca = Request.QueryString["FinNumber"];
                _IdVoca = Request.QueryString["IdVoca"];
                GvMainFaultRecord.DataSource = ProductionFaultAuditRepository.RetrieveDataFromSerialNumber(_FinNumber, "Usp_FaultAuditGetMainGv");
                GvMainFaultRecord.DataBind();
                GvMainFaultRecord.SearchPanelFilter = _FinNumber;
                labelNotif.Text = "FinNumber :" + _FinNumber;

            }
            else
            {
                labelNotif.Text = "FinNumber : - All FinNumber -";
                GvMainFaultRecord.DataSource = ProductionFaultAuditRepository.RetrieveDataFromSerialNumber("0", "Usp_FaultAuditGetMainGv");
                GvMainFaultRecord.DataBind();
            }
            LoadDataUser();
            if (!IsPostBack)
            {
                Session["SaveByte"] = null;
                Session["ByteImage"] = null;
                Session["dtAttachment"] = null;
                Session["DataDetail"] = null;
                Session["IdVoca"] = "";
                Session["_IdVoca"] = "";

            }
            PopUpAttachMent.ShowOnPageLoad = false;
            txDescDetail.ClientVisible = false;
            LoadData();

        }
        private void LoadData()
        {
            if (Request.QueryString.Count == 0)
            {
                lblPosition.Text = "Fault Record Main (VOCA & PA)";
            }
            else if (_IdVoca == "4")
            {
                lblPosition.Text = "Fault Record Main (VOCA)";

            }
            else if (_IdVoca == "3")
            {
                lblPosition.Text = "Fault Record Main (PA)";

            }
        }
        private void LoadDataUser()
        {
            if (Request.QueryString.Count == 0)
            {
                if (CSTypeValue.Value != null)
                {
                    if (CSTypeValue.Value.ToString() == "4")
                    {
                        Auditor1.DataSourceID = null;
                        Auditor1.DataSource = sdsUserVOCA;
                        Auditor1.DataBind();
                        Auditor2.DataSourceID = null;
                        Auditor2.DataSource = sdsUserVOCA;
                        Auditor2.DataBind();
                    }
                    else
                    {
                        Auditor1.DataSourceID = null;
                        Auditor1.DataSource = sdsUserPA;
                        Auditor1.DataBind();
                        Auditor2.DataSourceID = null;
                        Auditor2.DataSource = sdsUserPA;
                        Auditor2.DataBind();
                    }
                }
            }
            else
            {
                if (_IdVoca == "4")
                {
                    Auditor1.DataSourceID = null;
                    Auditor1.DataSource = sdsUserVOCA;
                    Auditor1.DataBind();
                    Auditor2.DataSourceID = null;
                    Auditor2.DataSource = sdsUserVOCA;
                    Auditor2.DataBind();
                }
                else
                {
                    Auditor1.DataSourceID = null;
                    Auditor1.DataSource = sdsUserPA;
                    Auditor1.DataBind();
                    Auditor2.DataSourceID = null;
                    Auditor2.DataSource = sdsUserPA;
                    Auditor2.DataBind();
                }
            }
        }
        protected void Auditor1_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadDataUser();
            Auditor1.DataBind();
        }
        protected void Auditor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataUser();
            Auditor2.DataBind();
        }
        protected void CSTypeValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Id = int.Parse(CSTypeValue.Value.ToString()) == 3 ? 0 : 1;
            var data = FaultRepository.GetFinNumber(Id);

            cbFINNumber.DataSource = data;
            cbFINNumber.DataBind();
            if (CSTypeValue.Value == "4")
            {
                Auditor1.Enabled = true;
                Auditor1.DataSource = sdsUserVOCA;
                Auditor1.DataBind();
                Auditor2.Enabled = true;
                Auditor2.DataSource = sdsUserVOCA;
                Auditor2.DataBind();

            }
            else if (CSTypeValue.Value == "3")
            {
                Auditor1.Enabled = true;
                Auditor1.DataSource = sdsUserPA;
                Auditor1.DataBind();
                Auditor2.Enabled = true;
                Auditor2.DataSource = sdsUserPA;
                Auditor2.DataBind();
            }
            else
            {
                Auditor1.Enabled = false;

            }

        }
        protected void PUcbPartProces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PUcbPartProces.Value != null)
            {
                int MaterialId = int.Parse(PUcbPartProces.Value.ToString());
                PUcbPartProcesDesc.Value = MaterialId;
            }
        }
        protected void PUcbPartProcesDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PUcbPartProcesDesc.SelectedItem != null)
            {

                int MaterialId = int.Parse(PUcbPartProcesDesc.SelectedItem.Value.ToString());
                PUcbPartProces.Value = MaterialId;
            }
        }
        protected void GvMainFaultRecord_DetailRowExpandedChanged(object sender, DevExpress.Web.ASPxGridViewDetailRowEventArgs e)
        {

            try
            {
                int Index = e.VisibleIndex;
                Session["IndexDetails"] = Index;
                HiddenText["CsType"] = GvMainFaultRecord.GetRowValues(Index, "cs").ToString();
                Session["IdFault"] = int.Parse(GvMainFaultRecord.GetRowValues(Index, "Id").ToString());
                Session["CsType"] = GvMainFaultRecord.GetRowValues(Index, "cs").ToString();
                Session["FinNumber"] = GvMainFaultRecord.GetRowValues(Index, "FINNumber").ToString();

                // Find Row Expanded
                ASPxGridView Data = (ASPxGridView)sender;
                var colapse = Data.DetailRows.VisibleCount;
                if (colapse == 1)
                {
                    var rowIndex = e.VisibleIndex;
                    Session["RowIndex"] = e.VisibleIndex;
                    var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(rowIndex, "GvDetailsFaultRecord") as ASPxGridView;
                    int ValueId = int.Parse(GvMainFaultRecord.GetRowValues(Index, "Id").ToString());
                    Session["RowExpanddedId"] = ValueId;
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
        protected void GvFaultAuditRecord_DetailRowExpandedChanged(object sender, DevExpress.Web.ASPxGridViewDetailRowEventArgs e)
        {

        }
        //DataBinder binding
        protected void GvMainFaultRecord_DataBinding(object sender, EventArgs e)
        {

        }
        protected void pcInteruptFaultRecord_DataBinding(object sender, EventArgs e)
        {

        }
        protected void GvDetailsFaultRecord_DataBinding(object sender, EventArgs e)
        {

            try
            {
                if (Session["GvDetailsBind"] != null)
                {
                    var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(int.Parse(Session["RowIndex"].ToString()), "GvDetailsFaultRecord") as ASPxGridView;
                    GvData.DataSource = ProductionFaultAuditRepository.RetrieveGridDetailData(int.Parse(Session["RowExpanddedId"].ToString()), "Usp_FaultAuditProductionGetGVDetail");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void pcFaulRecordDetail_DataBinding(object sender, EventArgs e)
        {

        }
        protected void GVDataAttachment_DataBinding(object sender, EventArgs e)
        {
            //if (Session["DataDetail"] != null)
            //{
            //    //GVDataAttachment.DataSource = Session["dtAttachment"];
            //    DataTable Datax = Session["DataDetail"] as DataTable;
            //    if (Datax.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < Datax.Rows.Count; i++)
            //        {
            //            GridViewDataCheckColumn CC = GVDataAttachment.Columns[5] as GridViewDataCheckColumn;
            //            ASPxCheckBox CConvencional = GVDataAttachment.FindRowCellTemplateControl(i, CC, "CheckBoxDefault") as ASPxCheckBox;

            //            string Check = Datax.Rows[i].ItemArray[5].ToString();
            //            ((IPostBackDataHandler)CConvencional).LoadPostData("", Request.Form);
            //            if (Check == "True")
            //            {
            //                CConvencional.Checked = true;
            //            }
            //        }
            //    }
            //}
        }
        //End data binding
        protected void GvFaultAuditRecord_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void GVDataAttachment_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        //event click
        protected void ShowAttachment_Click(object sender, EventArgs e)
        {
            if (Session["FinNumber"].ToString() != "")
            {
                PopUpAttachMent.ShowOnPageLoad = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Fin Number !');", true);
            }
        }
        protected void btnReport_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;

            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            var id = int.Parse(GvMainFaultRecord.GetRowValues(index, "Id").ToString());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
                "window.open('/custom/Report/ReportVoCaPa.aspx?Id=" + id + "','_newtab');",
                true);
        }
        protected void DeleteHeader_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                int ValueRowId = int.Parse(GvMainFaultRecord.GetRowValues(Rowindex, "Id").ToString());
                ProductionFaultAuditRepository.DeleteFault(ValueRowId);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }
        }
        protected void EditHeader_Click(object sender, EventArgs e)
        {
            HiddenText["EditHeader"] = "1";
            pcInteruptFaultRecord.ShowOnPageLoad = true;
            try
            {
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;

                int ValueRowId = int.Parse(GvMainFaultRecord.GetRowValues(Rowindex, "Id").ToString());
                Session["IdHeader"] = ValueRowId;
                TextId.Text = ValueRowId.ToString();
                //var ParameterStation = ComboBox_Station.SelectedItem.Value;
                //var ValueId = GvDetailsFaultRecord.GetRowValues(index, "Id");
                var DtTbEdit = ProductionFaultAuditRepository.RetrieveGridHeaderData(ValueRowId, "Usp_FaultAuditProductionGetGVHeader");
                CSTypeValue.Value = DtTbEdit.Rows[0].ItemArray[1].ToString();

                var DataColordll = ProductionFaultAuditRepository.RetrieveDataGetBaumaster(DtTbEdit.Rows[0].ItemArray[4].ToString(), "Usp_FaultAuditGetTypeColorBaumaster");
                tbLine.Text = DataColordll.Rows[0].ItemArray[2].ToString();
                TxType.Text = DataColordll.Rows[0].ItemArray[3].ToString();
                TxBaumaster.Text = DataColordll.Rows[0].ItemArray[1].ToString();
                TxPlant.Text = DtTbEdit.Rows[0].ItemArray[5].ToString();
                TxMileage.Text = DtTbEdit.Rows[0].ItemArray[6].ToString();
                Auditor1.Text = DtTbEdit.Rows[0].ItemArray[7].ToString();
                Auditor2.Text = DtTbEdit.Rows[0].ItemArray[8].ToString();
                cbFINNumber.Text = DtTbEdit.Rows[0].ItemArray[3].ToString() + "/" + DtTbEdit.Rows[0].ItemArray[4].ToString();
                TxColor.Text = DataColordll.Rows[0].ItemArray[0].ToString();
                //int year = int.Parse(DtTbEdit.Rows[0].ItemArray[2].ToString("yyyy").ToString().Split('/')[2].Split(' ')[0]);
                //int Day = int.Parse(DtTbEdit.Rows[0].ItemArray[2].ToString("dd").ToString().Split('/')[0]);
                //int Month = int.Parse(DtTbEdit.Rows[0].ItemArray[2].ToString("MM").ToString().Split('/')[1]);
                //DateTime dt = new DateTime(year, Month, Day);
                LoadDataUser();
                DateTime? dateAudit = ProductionFaultAuditRepository.getDateFaultAudit(Convert.ToInt32(DtTbEdit.Rows[0].ItemArray[0]));
               
                AuditDates.Text = dateAudit.Value.ToString("yyyy-MM-dd");
                BtnMainSaveCloseNew.Visible = false;
                BtnMainSaveClose.Visible = false;
                BtnMainSaveCloseEdit.Visible = true;
                BtnMainClose.Visible = true;
            }

            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }


        }
        protected void BtnSaveMain_Click(object sender, EventArgs e)
        {
            string CsType = "";
            string VinNumber = "";
            string FinNumber = "";
            int Auditor1UserId = 0;
            int Auditor2UserId = 0;
            if (CSTypeValue.SelectedItem != null)
            {
                CsType = CSTypeValue.SelectedItem.Value.ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select CSType !');", true);
                return;
            }
            string AuditDate = AuditDates.Text.ToString();
            if (cbFINNumber.SelectedItem != null)
            {
                ListEditItem selectedItem = cbFINNumber.SelectedItem;
                VinNumber = selectedItem.GetValue("VINNumber").ToString();
                FinNumber = selectedItem.GetValue("FINNumber").ToString();
            }
            string Plant = TxPlant.Text;
            int Mileage = int.Parse(TxMileage.Text == "" ? "0" : TxMileage.Text);
            User user = (User)Session["user"];
            var CreatedBy = user.UserName;
            if (Auditor1.SelectedItem != null)
            {
                Auditor1UserId = int.Parse(Auditor1.SelectedItem.Value.ToString());


            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Auditor 1 !');", true);
                return;
            }
            if (Auditor2.SelectedItem != null)
            {
                Auditor2UserId = int.Parse(Auditor2.SelectedItem.Value.ToString());

            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Auditor 2 !');", true);
                //return;
            }

            ProductionFaultAuditRepository.SaveDataAuditHeader(CsType, AuditDate, VinNumber, FinNumber, Plant, Mileage, Auditor1UserId, Auditor2UserId, CreatedBy);
            loadFaultDefaultHeader();
            pcInteruptFaultRecord.ShowOnPageLoad = true;
            _FinNumber = Request.QueryString["FinNumber"];
            GvMainFaultRecord.DataSource = ProductionFaultAuditRepository.RetrieveDataFromSerialNumber(_FinNumber, "Usp_FaultAuditGetMainGv");
            GvMainFaultRecord.DataBind();
        }
        protected void btnFault_Click(object sender, EventArgs e)
        {
            HiddenText["EditHeader"] = "0";
            pcInteruptFaultRecord.ShowOnPageLoad = true;
            loadFaultDefaultHeader();


            if (Request.QueryString.Count > 0)
            {

                if (_voca == "4")
                {
                    CSTypeValue.Value = 4;
                    CSTypeValue.SelectedIndex = 1;

                }
                else
                {
                    CSTypeValue.Value = 3;
                    CSTypeValue.SelectedIndex = 2;

                }

                TxPlant.Text = "158";
                int Id = int.Parse(_Id);
                var data = FaultRepository.GetFinNumber(Id);
                cbFINNumber.DataSource = data;
                cbFINNumber.DataBind();
                cbFINNumber.Value = int.Parse(_Id);
                tbLine.Text = GetLineDashBoardNotNUll(int.Parse(_Id));
                tbAssemblySection.Text = GetSectioneDashBoardNotNUll(int.Parse(_Id));
                tbClassification.Text = GetClassificationDashBoardNotNUll(int.Parse(_Id));
                TxType.Text = GetTypeDashBoardNotNUll(_FinNumber);
                TxColor.Text = GetColorDashBoardNotNUll(_FinNumber);
                TxBaumaster.Text = GetBauMasterDashBoardNotNUll(_FinNumber);

            }
            TxPlant.Text = "158";
            cmbDefaultImage.DataSource = null;
            cmbDefaultImage.DataBind();
            LoadDataUser();
            BtnMainSaveCloseNew.Visible = true;
            BtnMainSaveClose.Visible = true;
            BtnMainSaveCloseEdit.Visible = false;
            BtnMainClose.Visible = true;
        }
        protected void BtnMainSaveClose_Click(object sender, EventArgs e)
        {
            if (HiddenText["EditHeader"].ToString() != "1")
            {
                string CsType = "";
                string VinNumber = "";
                string FinNumber = "";
                int Auditor1UserId = 0;
                int Auditor2UserId = 0;
                if (CSTypeValue.SelectedItem != null)
                {
                    CsType = CSTypeValue.SelectedItem.Value.ToString();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select CSType !');", true);
                    return;
                }
                string AuditDate = AuditDates.Text.ToString();
                if (cbFINNumber.SelectedItem != null)
                {
                    ListEditItem selectedItem = cbFINNumber.SelectedItem;
                    VinNumber = selectedItem.GetValue("VINNumber").ToString();
                    FinNumber = selectedItem.GetValue("FINNumber").ToString();
                }
                string Plant = TxPlant.Text;
                int Mileage = int.Parse(TxMileage.Text == "" ? "0" : TxMileage.Text);
                User user = (User)Session["user"];
                var CreatedBy = user.UserName;
                if (Auditor1.SelectedItem != null)
                {
                    Auditor1UserId = int.Parse(Auditor1.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Auditor 1 !');", true);
                    return;
                }
                if (Auditor2.SelectedItem != null)
                {
                    Auditor2UserId = int.Parse(Auditor2.SelectedItem.Value.ToString());


                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Auditor 2 !');", true);
                    //return;
                }
                ProductionFaultAuditRepository.SaveDataAuditHeader(CsType, AuditDate, VinNumber, FinNumber, Plant, Mileage, Auditor1UserId, Auditor2UserId, CreatedBy);
                pcInteruptFaultRecord.ShowOnPageLoad = false;
                _FinNumber = Request.QueryString["FinNumber"];
                GvMainFaultRecord.DataSource = ProductionFaultAuditRepository.RetrieveDataFromSerialNumber(_FinNumber, "Usp_FaultAuditGetMainGv");
                GvMainFaultRecord.DataBind();
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                try
                {
                    string CsType = CSTypeValue.SelectedItem == null ? "0" : CSTypeValue.SelectedItem.Value.ToString();
                    string AuditDate = AuditDates.Text.ToString();
                    ListEditItem selectedItem = cbFINNumber.SelectedItem;
                    string VinNumber = cbFINNumber.SelectedItem == null ? "0" : selectedItem.GetValue("VINNumber").ToString();
                    string FinNumber = cbFINNumber.SelectedItem == null ? "0" : selectedItem.GetValue("FINNumber").ToString();
                    string Plant = TxPlant.Text;
                    int Mileage = int.Parse(TxMileage.Text);
                    User user = (User)Session["user"];
                    var CreatedBy = user.UserName;

                    int Auditor1UserId = Auditor1.SelectedItem == null ? 0 : int.Parse(Auditor1.SelectedItem.Value.ToString());
                    int Auditor2UserId = Auditor2.SelectedItem == null ? 0 : int.Parse(Auditor2.SelectedItem.Value.ToString());
                    ProductionFaultAuditRepository.SaveDataAuditHeaderEdit(int.Parse(Session["IdHeader"].ToString()), CsType, AuditDate, VinNumber, FinNumber, Plant, Mileage, Auditor1UserId, Auditor2UserId, CreatedBy);
                    pcInteruptFaultRecord.ShowOnPageLoad = false;

                }
                catch (Exception ex)
                {
                    AppLogger.LogError(ex);
                }
                Response.Redirect(Request.RawUrl);
            }
        }
        protected void BtnMainClose_Click(object sender, EventArgs e)
        {
            pcInteruptFaultRecord.ShowOnPageLoad = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        protected void btnFaultDetail_Click(object sender, EventArgs e)
        {
            pcFaulRecordDetail.ShowOnPageLoad = true;
            IdFaultAudit.Text = Session["IdFault"].ToString();
            HiddenText["IdFault"] = Session["IdFault"].ToString();
            HiddenText["Edit"] = "0";
            loadFaultDefaultEdit();
            if (Session["CsType"].ToString() == "VoCA")
            {
                var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
                cbFaultDesc.DataSource = data;
                cbFaultDesc.DataBind();
                cbFaultDescDesc.DataSource = data;
                cbFaultDescDesc.DataBind();
            }
            else
            {
                var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
                cbFaultDesc.DataSource = data;
                cbFaultDesc.DataBind();
                cbFaultDescDesc.DataSource = data;
                cbFaultDescDesc.DataBind();
            }
            PUcbPartProces.DataSource = sdsFaultPartProcess;
            PUcbPartProces.DataBind();
            PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
            PUcbPartProcesDesc.DataBind();
        }
        protected void BtnDetailFaultSaveNew_Click(object sender, EventArgs e)
        {
            if (HiddenText["Edit"].ToString() == "0")
            {
                int FaultAuditReponsibleId = 0;
                int MaterialId = 0;
                int FaultDescriptionId = 0;
                int FaultAreaID = 0;
                int Prio = 0;
                int StationId = 0;
                bool Rework = false;
                bool Analysis = false;
                User user = (User)Session["user"];
                var CreatedBy = user.UserName;
                var SumRowData = GVDataAttachment.VisibleRowCount;
                List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                GridViewDataTextColumn CC = GVDataAttachment.Columns[5] as GridViewDataTextColumn;
                if (SumRowData > 0)
                {
                    for (int i = 0; i < SumRowData; i++)
                    {
                        DocumentAttachment AttachFile = new DocumentAttachment();
                        AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                        AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                        //AttachFile.FileType = GVDataAttachment.GetRowValues(i, "FileType").ToString();
                        ASPxCheckBox Check = GVDataAttachment.FindRowCellTemplateControl(i, CC, "CheckBoxDefault") as ASPxCheckBox;
                        // AttachFile.Checked = bool.Parse(Check.Value == null ? "false" : Check.Value.ToString());
                        AttachFile.Byte = Encoding.UTF8.GetBytes(GVDataAttachment.GetRowValues(i, "FileBinnary").ToString());
                        ListAttachFile.Add(AttachFile);
                    }
                }
                string IdDefaultAttachment = cmbDefaultImage.Text;
                int faultauditid = int.Parse(Session["IdFault"].ToString());
                ListEditItem selectedItemResponsible = cmbReponsibleLine.SelectedItem;
                //string FinNumber = selectedItem.GetValue("FINNumber").ToString();
                if (cmbReponsibleLine.SelectedItem != null)
                {
                    FaultAuditReponsibleId = int.Parse(cmbReponsibleLine.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Reponsible Line !');", true);
                    return;
                }

                //FaultAuditReponsibleId = int.Parse(cmbReponsibleLine.SelectedItem.Value.ToString());
                ListEditItem SelectedItemMaterial = PUcbPartProces.SelectedItem;
                int faultDescId;
                if (int.TryParse(PUcbPartProces.Value.ToString(), out faultDescId))
                {
                    MaterialId = int.Parse(PUcbPartProces.Value.ToString());
                }
                else
                {
                    var Code = PUcbPartProces.Text;
                    var Description = PUcbPartProcesDesc.Text;
                    MaterialId = ProductionFaultAuditRepository.SaveDataAuditParts(Code, Description);
                }

                MaterialId = int.Parse(MaterialId.ToString());



                if (int.TryParse(cbFaultDesc.Value.ToString(), out faultDescId))
                {
                    FaultDescriptionId = Convert.ToInt32(cbFaultDesc.Value);
                }
                else
                {
                    string csType = Session["CsType"].ToString();
                    string Code = cbFaultDesc.Text;
                    string Desc = cbFaultDescDesc.Text;
                    if (csType == "VoCA")
                    {
                        FaultDescriptionId = ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, true, false, false);
                    }
                    else
                    {
                        FaultDescriptionId = ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, false, true, false);
                    }

                }
                FaultDescriptionId = int.Parse(FaultDescriptionId.ToString());


                if (cmb_Area.SelectedItem != null)
                {
                    FaultAreaID = int.Parse(cmb_Area.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Area !');", true);
                    return;
                }
                FaultAreaID = int.Parse(cmb_Area.SelectedItem.Value.ToString());

                if (cbPriority.SelectedItem != null)
                {
                    Prio = int.Parse(cbPriority.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Pri !');", true);
                    return;
                }
                Prio = int.Parse(cbPriority.SelectedItem.Value.ToString());

                if (cbStation.SelectedItem != null)
                {
                    StationId = int.Parse(cbStation.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Station Name !');", true);
                    return;
                }
                StationId = int.Parse(cbStation.SelectedItem.Value.ToString());

                if (cmbAnalysis.SelectedItem != null)
                {
                    Analysis = cmbAnalysis.SelectedItem.Value.ToString() == "0" ? false : true;

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Analysis !');", true);
                    return;
                }
                Analysis = cmbAnalysis.SelectedItem.Text == "No" ? false : true;

                if (cmbRework.SelectedItem != null)
                {
                    Rework = cmbRework.SelectedItem.Value.ToString() == "0" ? false : true;

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Rework !');", true);
                    return;
                }
                Rework = cmbRework.SelectedItem.Text == "Yes" ? true : false;

                string Remarks = txRemarks.Text;
                var attachImage = Session["SaveByte"] as List<FaultAuditAttachmentModel>;
                //Check Count Checked Default
                int SumChecked = 0;
                string FaultDescDetails = txDescDetail.Text;
                for (int x = 0; x < ListAttachFile.Count(); x++)
                {
                    int Count = ListAttachFile[x].Checked == true ? 1 : 0;
                    SumChecked += Count;
                }
                //
                if (SumChecked <= 1)
                {
                    int IdFaultRecord = ProductionFaultAuditRepository.SaveDataAuditDetail(faultauditid, FaultAuditReponsibleId, MaterialId, FaultDescriptionId, FaultAreaID, Prio, StationId, Analysis, Rework, CreatedBy, Remarks, FaultDescDetails);
                    for (int x = 0; x < ListAttachFile.Count(); x++)
                    {
                        if (IdDefaultAttachment == ListAttachFile[x].FileName)
                        {
                            ProductionFaultAuditRepository.InsertAttachMentAudit(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, attachImage[x].FileBinnary, IdFaultRecord, attachImage[x].FileType, CreatedBy, true);
                        }
                        else
                        {
                            ProductionFaultAuditRepository.InsertAttachMentAudit(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, attachImage[x].FileBinnary, IdFaultRecord, attachImage[x].FileType, CreatedBy, false);
                        }

                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Default Checked More Than One');", true);
                    return;
                }
                Session["SaveByte"] = null;
                //pcFaulRecordDetail.ShowOnPageLoad = false;
                loadFaultDefaultEdit();
                pcFaulRecordDetail.ShowOnPageLoad = true;
                IdFaultAudit.Text = Session["IdFault"].ToString();
                HiddenText["IdFault"] = Session["IdFault"].ToString();
                HiddenText["Edit"] = "0";
                loadFaultDefaultEdit();
                if (Session["CsType"].ToString() == "VoCA")
                {
                    var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
                    cbFaultDesc.DataSource = data;
                    cbFaultDesc.DataBind();
                    cbFaultDescDesc.DataSource = data;
                    cbFaultDescDesc.DataBind();
                }
                else
                {
                    var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
                    cbFaultDesc.DataSource = data;
                    cbFaultDesc.DataBind();
                    cbFaultDescDesc.DataSource = data;
                    cbFaultDescDesc.DataBind();
                }
                PUcbPartProces.DataSource = sdsFaultPartProcess;
                PUcbPartProces.DataBind();
                PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
                PUcbPartProcesDesc.DataBind();
            }
            else
            {
                try
                {
                    int FaultAuditReponsibleId = 0;
                    int MaterialId = 0;
                    int FaultDescriptionId = 0;
                    User user = (User)Session["user"];
                    var CreatedBy = user.UserName;
                    var SumRowData = GVDataAttachment.VisibleRowCount;
                    List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                    GridViewDataTextColumn CC = GVDataAttachment.Columns[5] as GridViewDataTextColumn;
                    string IdDefaultAttachment = cmbDefaultImage.Text;
                    int id = int.Parse(HiddenText["Id"].ToString());
                    int faultauditid = int.Parse(Session["IdFault"].ToString());
                    ListEditItem selectedItemResponsible = cmbReponsibleLine.SelectedItem;
                    if (cmbReponsibleLine.SelectedItem != null)
                    {
                        FaultAuditReponsibleId = int.Parse(cmbReponsibleLine.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        FaultAuditReponsibleId = 0;
                    }
                    ListEditItem SelectedItemMaterial = PUcbPartProces.SelectedItem;
                    int faultDescId;
                    if (int.TryParse(PUcbPartProces.Value.ToString(), out faultDescId))
                    {
                        MaterialId = int.Parse(PUcbPartProces.Value.ToString());
                    }
                    else
                    {
                        MaterialId = 0;
                    }

                    MaterialId = int.Parse(MaterialId.ToString());



                    if (int.TryParse(cbFaultDesc.Value.ToString(), out faultDescId))
                    {
                        FaultDescriptionId = Convert.ToInt32(cbFaultDesc.Value);
                    }
                    else
                    {
                        FaultDescriptionId = 0;
                    }



                    int FaultAreaID = cmb_Area.SelectedItem == null ? 0 : int.Parse(cmb_Area.SelectedItem.Value.ToString());
                    int Prio = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.SelectedItem.Value.ToString());
                    int StationId = cbStation.SelectedItem == null ? 0 : int.Parse(cbStation.SelectedItem.Value.ToString());
                    int Analysis = cmbAnalysis.SelectedItem == null ? 3 : int.Parse(cmbAnalysis.SelectedItem.Text == "Yes" ? "1" : "0");
                    int Rework = cmbRework.SelectedItem == null ? 3 : int.Parse(cmbRework.SelectedItem.Text == "Yes" ? "1" : "0");

                    var attachImage = Session["SaveByte"] as List<FaultAuditAttachmentModel>;
                    if (attachImage == null)
                    {
                        attachImage = Session["ByteImage"] as List<FaultAuditAttachmentModel>;
                    }
                    DataTable Datax = Session["DataDetail"] as DataTable;
                    //Check Count Checked Default
                    int SumChecked = 0;
                    string Remarks = txRemarks.Text;
                    string DescDetail = txDescDetail.Text;
                    for (int x = 0; x < ListAttachFile.Count(); x++)
                    {
                        int Count = ListAttachFile[x].Checked == true ? 1 : 0;
                        SumChecked += Count;
                    }
                    //
                    if (SumChecked <= 1)
                    {

                        bool IdFaultRecord = ProductionFaultAuditRepository.UpdateAuditModel(id, FaultAuditReponsibleId, MaterialId, FaultDescriptionId, FaultAreaID, Prio, StationId, Analysis, Rework, CreatedBy, Remarks, DescDetail);
                        if (Datax.Rows.Count > 0)
                        {

                            ProductionFaultAuditRepository.DeleteAttachmentEdit(id);
                            for (int x = 0; x < Datax.Rows.Count; x++)
                            {

                                string FileName = Datax.Rows[x].ItemArray[1].ToString();
                                string FilePath = Datax.Rows[x].ItemArray[2].ToString();
                                //byte[] FileByte = Encoding.UTF8.GetBytes(Datax.Rows[x].ItemArray[3].ToString());
                                string FileType = Datax.Rows[x].ItemArray[4].ToString();
                                bool Checked = bool.Parse(Datax.Rows[x].ItemArray[5].ToString());
                                if (IdDefaultAttachment == FileName)
                                {
                                    ProductionFaultAuditRepository.InsertAttachMentAuditEdit(FileName, FilePath, attachImage[x].FileBinnary, id, FileType, CreatedBy, true);
                                }
                                else
                                {
                                    ProductionFaultAuditRepository.InsertAttachMentAuditEdit(FileName, FilePath, attachImage[x].FileBinnary, id, FileType, CreatedBy, false);
                                }

                            }

                        }
                        if (IdFaultRecord == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage3", @"alert('Save Successfully');", true);
                            var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(int.Parse(Session["RowIndex"].ToString()), "GvDetailsFaultRecord") as ASPxGridView;
                            GvData.DataSource = ProductionFaultAuditRepository.RetrieveGridDetailData(int.Parse(Session["RowExpanddedId"].ToString()), "Usp_FaultAuditProductionGetGVDetail");
                            GvData.DataBind();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Default Checked More Than One');", true);
                        return;
                    }
                    Session["SaveByte"] = null;
                    loadFaultDefaultEdit();
                    pcFaulRecordDetail.ShowOnPageLoad = true;
                    IdFaultAudit.Text = Session["IdFault"].ToString();
                    HiddenText["IdFault"] = Session["IdFault"].ToString();
                    HiddenText["Edit"] = "0";
                    loadFaultDefaultEdit();
                    if (Session["CsType"].ToString() == "VoCA")
                    {
                        var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
                        cbFaultDesc.DataSource = data;
                        cbFaultDesc.DataBind();
                        cbFaultDescDesc.DataSource = data;
                        cbFaultDescDesc.DataBind();
                    }
                    else
                    {
                        var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
                        cbFaultDesc.DataSource = data;
                        cbFaultDesc.DataBind();
                        cbFaultDescDesc.DataSource = data;
                        cbFaultDescDesc.DataBind();
                    }
                    PUcbPartProces.DataSource = sdsFaultPartProcess;
                    PUcbPartProces.DataBind();
                    PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
                    PUcbPartProcesDesc.DataBind();

                }
                catch (Exception ex)
                {
                    AppLogger.LogError(ex);

                }

            }

        }
        protected void BtnFaultDetailSaveClose_Click(object sender, EventArgs e)
        {

            if (HiddenText["Edit"].ToString() == "0")
            {
                int FaultAuditReponsibleId = 0;
                int MaterialId = 0;
                int FaultDescriptionId = 0;
                int FaultAreaID = 0;
                int Prio = 0;
                int StationId = 0;
                bool Rework = false;
                bool Analysis = false;
                User user = (User)Session["user"];
                var CreatedBy = user.UserName;
                var SumRowData = GVDataAttachment.VisibleRowCount;
                List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                GridViewDataTextColumn CC = GVDataAttachment.Columns[5] as GridViewDataTextColumn;
                if (SumRowData > 0)
                {
                    for (int i = 0; i < SumRowData; i++)
                    {
                        DocumentAttachment AttachFile = new DocumentAttachment();
                        AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                        AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FileLocation").ToString();
                        //AttachFile.FileType = GVDataAttachment.GetRowValues(i, "FileType").ToString();
                        ASPxCheckBox Check = GVDataAttachment.FindRowCellTemplateControl(i, CC, "CheckBoxDefault") as ASPxCheckBox;
                        // AttachFile.Checked = bool.Parse(Check.Value == null ? "false" : Check.Value.ToString());
                        AttachFile.Byte = Encoding.UTF8.GetBytes(GVDataAttachment.GetRowValues(i, "FileBinnary").ToString());
                        ListAttachFile.Add(AttachFile);
                    }
                }
                string IdDefaultAttachment = cmbDefaultImage.Text;
                int faultauditid = int.Parse(Session["IdFault"].ToString());
                ListEditItem selectedItemResponsible = cmbReponsibleLine.SelectedItem;
                //string FinNumber = selectedItem.GetValue("FINNumber").ToString();
                if (cmbReponsibleLine.SelectedItem != null)
                {
                    FaultAuditReponsibleId = int.Parse(cmbReponsibleLine.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Reponsible Line !');", true);
                    return;
                }

                //FaultAuditReponsibleId = int.Parse(cmbReponsibleLine.SelectedItem.Value.ToString());
                ListEditItem SelectedItemMaterial = PUcbPartProces.SelectedItem;
                int faultDescId;
                if (PUcbPartProces.Value != null)
                {
                    if (int.TryParse(PUcbPartProces.Value.ToString(), out faultDescId))
                    {
                        MaterialId = int.Parse(PUcbPartProces.Value.ToString());
                    }
                    else
                    {
                        var Code = PUcbPartProces.Text;
                        var Description = PUcbPartProcesDesc.Text;
                        MaterialId = ProductionFaultAuditRepository.SaveDataAuditParts(Code, Description);
                    }
                }

                MaterialId = int.Parse(MaterialId.ToString());


                if (cbFaultDesc.Value != null)
                {
                    if (int.TryParse(cbFaultDesc.Value.ToString(), out faultDescId))
                    {
                        FaultDescriptionId = Convert.ToInt32(cbFaultDesc.Value);
                    }
                    else
                    {
                        string csType = Session["CsType"].ToString();
                        string Code = cbFaultDesc.Text;
                        string Desc = cbFaultDescDesc.Text;
                        if (csType == "VoCA")
                        {
                            FaultDescriptionId = ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, true, false, false);
                        }
                        else
                        {
                            FaultDescriptionId = ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, false, true, false);
                        }

                    }
                    FaultDescriptionId = int.Parse(FaultDescriptionId.ToString());
                }

                if (cmb_Area.SelectedItem != null)
                {
                    FaultAreaID = int.Parse(cmb_Area.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Area !');", true);
                    return;
                }
                FaultAreaID = int.Parse(cmb_Area.SelectedItem.Value.ToString());

                if (cbPriority.SelectedItem != null)
                {
                    Prio = int.Parse(cbPriority.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Pri !');", true);
                    return;
                }
                Prio = int.Parse(cbPriority.SelectedItem.Value.ToString());

                if (cbStation.SelectedItem != null)
                {
                    StationId = int.Parse(cbStation.SelectedItem.Value.ToString());

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Station Name !');", true);
                    return;
                }
                StationId = int.Parse(cbStation.SelectedItem.Value.ToString());

                if (cmbAnalysis.SelectedItem != null)
                {
                    Analysis = cmbAnalysis.SelectedItem.Value.ToString() == "0" ? false : true;

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Analysis !');", true);
                    return;
                }
                Analysis = cmbAnalysis.SelectedItem.Text == "No" ? false : true;

                if (cmbRework.SelectedItem != null)
                {
                    Rework = cmbRework.SelectedItem.Value.ToString() == "0" ? false : true;

                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Select Rework !');", true);
                    return;
                }
                Rework = cmbRework.SelectedItem.Text == "Yes" ? true : false;

                string Remarks = txRemarks.Text;
                var attachImage = Session["SaveByte"] as List<FaultAuditAttachmentModel>;
                //Check Count Checked Default
                int SumChecked = 0;
                string FaultDescDetails = txDescDetail.Text;
                for (int x = 0; x < ListAttachFile.Count(); x++)
                {
                    int Count = ListAttachFile[x].Checked == true ? 1 : 0;
                    SumChecked += Count;
                }
                //
                if (SumChecked <= 1)
                {
                    int IdFaultRecord = ProductionFaultAuditRepository.SaveDataAuditDetail(faultauditid, FaultAuditReponsibleId, MaterialId, FaultDescriptionId, FaultAreaID, Prio, StationId, Analysis, Rework, CreatedBy, Remarks, FaultDescDetails);
                    for (int x = 0; x < ListAttachFile.Count(); x++)
                    {
                        if (IdDefaultAttachment == ListAttachFile[x].FileName)
                        {
                            ProductionFaultAuditRepository.InsertAttachMentAudit(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, attachImage[x].FileBinnary, IdFaultRecord, attachImage[x].FileType, CreatedBy, true);
                        }
                        else
                        {
                            ProductionFaultAuditRepository.InsertAttachMentAudit(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, attachImage[x].FileBinnary, IdFaultRecord, attachImage[x].FileType, CreatedBy, false);
                        }

                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Default Checked More Than One');", true);
                    return;
                }
                pcFaulRecordDetail.ShowOnPageLoad = false;
            }
            else
            {
                try
                {
                    int FaultAuditReponsibleId = 0;
                    int MaterialId = 0;
                    int FaultDescriptionId = 0;
                    User user = (User)Session["user"];
                    var CreatedBy = user.UserName;
                    var SumRowData = GVDataAttachment.VisibleRowCount;
                    List<DocumentAttachment> ListAttachFile = new List<DocumentAttachment>();
                    GridViewDataTextColumn CC = GVDataAttachment.Columns[5] as GridViewDataTextColumn;
                    string IdDefaultAttachment = cmbDefaultImage.Text;
                    int id = int.Parse(HiddenText["Id"].ToString());
                    int faultauditid = int.Parse(Session["IdFault"].ToString());
                    ListEditItem selectedItemResponsible = cmbReponsibleLine.SelectedItem;
                    if (cmbReponsibleLine.SelectedItem != null)
                    {
                        FaultAuditReponsibleId = int.Parse(cmbReponsibleLine.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        FaultAuditReponsibleId = 0;
                    }
                    ListEditItem SelectedItemMaterial = PUcbPartProces.SelectedItem;
                    int faultDescId;
                    if (int.TryParse(PUcbPartProces.Value.ToString(), out faultDescId))
                    {
                        MaterialId = int.Parse(PUcbPartProces.Value.ToString());
                    }
                    else
                    {
                        MaterialId = 0;
                    }

                    MaterialId = int.Parse(MaterialId.ToString());



                    if (int.TryParse(cbFaultDesc.Value.ToString(), out faultDescId))
                    {
                        FaultDescriptionId = Convert.ToInt32(cbFaultDesc.Value);
                    }
                    else
                    {
                        FaultDescriptionId = 0;
                    }



                    int FaultAreaID = cmb_Area.SelectedItem == null ? 0 : int.Parse(cmb_Area.SelectedItem.Value.ToString());
                    int Prio = cbPriority.SelectedItem == null ? 0 : int.Parse(cbPriority.SelectedItem.Value.ToString());
                    int StationId = cbStation.SelectedItem == null ? 0 : int.Parse(cbStation.SelectedItem.Value.ToString());
                    int Analysis = cmbAnalysis.SelectedItem == null ? 3 : int.Parse(cmbAnalysis.SelectedItem.Text == "Yes" ? "1" : "0");
                    int Rework = cmbRework.SelectedItem == null ? 3 : int.Parse(cmbRework.SelectedItem.Text == "Yes" ? "1" : "0");

                    var attachImage = Session["SaveByte"] as List<FaultAuditAttachmentModel>;
                    if (attachImage == null)
                    {
                        attachImage = Session["ByteImage"] as List<FaultAuditAttachmentModel>;
                    }
                    DataTable Datax = Session["DataDetail"] as DataTable;
                    //Check Count Checked Default
                    int SumChecked = 0;
                    string Remarks = txRemarks.Text;
                    string DescDetail = txDescDetail.Text;
                    for (int x = 0; x < ListAttachFile.Count(); x++)
                    {
                        int Count = ListAttachFile[x].Checked == true ? 1 : 0;
                        SumChecked += Count;
                    }
                    //
                    if (SumChecked <= 1)
                    {

                        bool IdFaultRecord = ProductionFaultAuditRepository.UpdateAuditModel(id, FaultAuditReponsibleId, MaterialId, FaultDescriptionId, FaultAreaID, Prio, StationId, Analysis, Rework, CreatedBy, Remarks, DescDetail);
                        if (Datax.Rows.Count > 0)
                        {

                            ProductionFaultAuditRepository.DeleteAttachmentEdit(id);
                            for (int x = 0; x < Datax.Rows.Count; x++)
                            {

                                string FileName = Datax.Rows[x].ItemArray[1].ToString();
                                string FilePath = Datax.Rows[x].ItemArray[2].ToString();
                                //byte[] FileByte = Encoding.UTF8.GetBytes(Datax.Rows[x].ItemArray[3].ToString());
                                string FileType = Datax.Rows[x].ItemArray[4].ToString();
                                bool Checked = bool.Parse(Datax.Rows[x].ItemArray[5].ToString());
                                if (IdDefaultAttachment == FileName)
                                {
                                    ProductionFaultAuditRepository.InsertAttachMentAuditEdit(FileName, FilePath, attachImage[x].FileBinnary, id, FileType, CreatedBy, true);
                                }
                                else
                                {
                                    ProductionFaultAuditRepository.InsertAttachMentAuditEdit(FileName, FilePath, attachImage[x].FileBinnary, id, FileType, CreatedBy, false);
                                }

                            }

                        }
                        if (IdFaultRecord == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage3", @"alert('Save Successfully');", true);
                            var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(int.Parse(Session["RowIndex"].ToString()), "GvDetailsFaultRecord") as ASPxGridView;
                            GvData.DataSource = ProductionFaultAuditRepository.RetrieveGridDetailData(int.Parse(Session["RowExpanddedId"].ToString()), "Usp_FaultAuditProductionGetGVDetail");
                            GvData.DataBind();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Default Checked More Than One');", true);
                        return;
                    }

                    pcFaulRecordDetail.ShowOnPageLoad = false;
                    var GvDataBind = GvMainFaultRecord.FindDetailRowTemplateControl(int.Parse(Session["RowIndex"].ToString()), "GvDetailsFaultRecord") as ASPxGridView;
                    GvDataBind.DataSource = ProductionFaultAuditRepository.RetrieveGridDetailData(int.Parse(Session["RowExpanddedId"].ToString()), "Usp_FaultAuditProductionGetGVDetail");
                    GvDataBind.DataBind();
                }
                catch (Exception ex)
                {
                    AppLogger.LogError(ex);

                }

            }
            Response.Redirect(Request.RawUrl);
        }
        protected void BtnFaultDetailClose_Click(object sender, EventArgs e)
        {
            pcFaulRecordDetail.ShowOnPageLoad = false;
        }
        protected void BtnDetailFaultSaveNewEdit_Click(object sender, EventArgs e)
        {

        }
        protected void DeleteDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                var Index = int.Parse(Session["IndexDetails"].ToString());
                var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(Index, "GvDetailsFaultRecord") as ASPxGridView;
                int ValueRowId = int.Parse(GvData.GetRowValues(Rowindex, "Id").ToString());
                ProductionFaultAuditRepository.DeleteFaultRecord(ValueRowId);
                int index = int.Parse(Session["RowIndex"].ToString());
                var GvDataBind = GvMainFaultRecord.FindDetailRowTemplateControl(index, "GvDetailsFaultRecord") as ASPxGridView;
                int ValueId = int.Parse(GvMainFaultRecord.GetRowValues(Index, "Id").ToString());
                GvDataBind.DataSource = ProductionFaultAuditRepository.RetrieveGridDetailData(ValueId, "Usp_FaultAuditProductionGetGVDetail");
                GvDataBind.DataBind();
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }
        }
        protected void EditDetails_Click(object sender, EventArgs e)
        {
            pcFaulRecordDetail.ShowOnPageLoad = true;

            HiddenText["Edit"] = "1";
            try
            {
                loadFaultDefaultEdit();
                //TextId.ClientVisible = false;
                IdFaultAudit.ClientVisible = true;
                tbAssemblySection.ClientVisible = false;
                // Find Row Expanded
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                var Index = int.Parse(Session["IndexDetails"].ToString());
                var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(Index, "GvDetailsFaultRecord") as ASPxGridView;
                int ValueRowId = int.Parse(GvData.GetRowValues(Rowindex, "Id").ToString());

                TextId.Text = ValueRowId.ToString();
                HiddenText["Id"] = ValueRowId.ToString();
                //var ParameterStation = ComboBox_Station.SelectedItem.Value;
                //var ValueId = GvDetailsFaultRecord.GetRowValues(index, "Id");
                var DtTbEdit = ProductionFaultAuditRepository.RetrieveGridDetailDataEdit(ValueRowId, "Usp_FaultAuditProductionGetGVDetailEdit");
                var DtTbEditAttachment = ProductionFaultAuditRepository.RetrieveGridDetailDataEditAttachment(ValueRowId, "Usp_FaultAuditProductionGetGVDetailEditAttachment");

                IdFaultAudit.Text = DtTbEdit.Rows[0].ItemArray[0].ToString();
                PUcbPartProces.Text = DtTbEdit.Rows[0].ItemArray[13].ToString();
                PUcbPartProcesDesc.Text = DtTbEdit.Rows[0].ItemArray[2].ToString();
                cbStation.Text = DtTbEdit.Rows[0].ItemArray[5].ToString();
                tbAssemblySection.Text = "";
                cbFaultDesc.Text = DtTbEdit.Rows[0].ItemArray[3].ToString();
                cbFaultDescDesc.Text = DtTbEdit.Rows[0].ItemArray[3].ToString();
                cmb_Area.Text = DtTbEdit.Rows[0].ItemArray[4].ToString();
                cbPriority.Text = DtTbEdit.Rows[0].ItemArray[9].ToString();
                cmbReponsibleLine.Text = DtTbEdit.Rows[0].ItemArray[1].ToString();
                cmbAnalysis.Text = DtTbEdit.Rows[0].ItemArray[6].ToString();
                cmbRework.Text = DtTbEdit.Rows[0].ItemArray[10].ToString();
                txDescDetail.Text = DtTbEdit.Rows[0].ItemArray[11].ToString();
                txRemarks.Text = DtTbEdit.Rows[0].ItemArray[12].ToString();
                Session["DataDetail"] = null;
                Session["dtAttachment"] = null;
                Session["ByteImage"] = null;
                PUcbPartProces.DataSource = sdsFaultPartProcess;
                PUcbPartProces.DataBind();
                PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
                PUcbPartProcesDesc.DataBind();
                if (Session["CsType"].ToString() == "VoCA")
                {
                    var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
                    cbFaultDesc.DataSource = data;
                    cbFaultDesc.DataBind();
                    cbFaultDescDesc.DataSource = data;
                    cbFaultDescDesc.DataBind();
                }
                else
                {
                    var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
                    cbFaultDesc.DataSource = data;
                    cbFaultDesc.DataBind();
                    cbFaultDescDesc.DataSource = data;
                    cbFaultDescDesc.DataBind();
                }

                PUcbPartProces.DataSource = sdsFaultPartProcess;
                PUcbPartProces.DataBind();
                PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
                PUcbPartProcesDesc.DataBind();
                GVDataAttachment.DataSource = DtTbEditAttachment;
                Session["DataDetail"] = DtTbEditAttachment;
                //Session["ByteImage"] = DtTbEditAttachment;
                GVDataAttachment.DataBind();
                for (int y = 0; y <= DtTbEditAttachment.Rows.Count - 1; y++)
                {
                    if (DtTbEditAttachment.Rows[y].ItemArray[5].ToString() == "True")
                    {
                        cmbDefaultImage.Text = DtTbEditAttachment.Rows[y].ItemArray[1].ToString();
                    }
                }

                cmbDefaultImage.DataSource = DtTbEditAttachment;
                cmbDefaultImage.DataBind();
                Session["ByteImage"] = ProductionFaultAuditRepository.GetEditAttachmentList(ValueRowId);
                if (Session["DataDetail"] != null)
                {
                    cmbDefaultImage.DataSource = Session["ByteImage"];
                    cmbDefaultImage.DataBind();
                }
                // Session["ByteImage"] = ProductionFaultAuditRepository.GetEditAttachmentList(ValueRowId);
            }

            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }
        }
        protected void BtnFaultDetailSaveCloseEdit_Click(object sender, EventArgs e)
        {

        }
        protected void BtnMainSaveCloseEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string CsType = CSTypeValue.SelectedItem == null ? "0" : CSTypeValue.SelectedItem.Value.ToString();
                string AuditDate = AuditDates.Text.ToString();
                ListEditItem selectedItem = cbFINNumber.SelectedItem;
                string VinNumber = cbFINNumber.SelectedItem == null ? "0" : selectedItem.GetValue("VINNumber").ToString();
                string FinNumber = cbFINNumber.SelectedItem == null ? "0" : selectedItem.GetValue("FINNumber").ToString();
                string Plant = TxPlant.Text;
                int Mileage = int.Parse(TxMileage.Text);
                User user = (User)Session["user"];
                var CreatedBy = user.UserName;

                int Auditor1UserId = Auditor1.SelectedItem == null ? 0 : int.Parse(Auditor1.SelectedItem.Value.ToString());
                int Auditor2UserId = Auditor2.SelectedItem == null ? 0 : int.Parse(Auditor2.SelectedItem.Value.ToString());
                ProductionFaultAuditRepository.SaveDataAuditHeaderEdit(int.Parse(Session["IdHeader"].ToString()), CsType, AuditDate, VinNumber, FinNumber, Plant, Mileage, Auditor1UserId, Auditor2UserId, CreatedBy);
                pcInteruptFaultRecord.ShowOnPageLoad = false;

            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
            Response.Redirect(Request.RawUrl);
        }
        protected void IdBtndelete_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var Rowindex = container.VisibleIndex;
            int ValueRowId = int.Parse(GVDataAttachment.GetRowValues(Rowindex, "Id").ToString());
            var Data = Session["DataDetail"];
            var oldattachImage = Session["ByteImage"] as List<FaultAuditAttachmentModel>;
            DataTable Datax = Session["DataDetail"] as DataTable;
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
                    dr.AcceptChanges();
                }
            }
            for (int i = 0; i < oldattachImage.Count; i++)
            {
                if (oldattachImage[i].Id == ValueRowId)
                {
                    oldattachImage.Remove(oldattachImage[i]);
                    Session["ByteImage"] = oldattachImage;
                    Session["SaveByte"] = oldattachImage;
                }
            }

            DataTable DataNew = new DataTable();
            DataNew = Datax;
            GVDataAttachment.DataSource = DataNew;
            Session["DataDetail"] = DataNew;
            GVDataAttachment.DataBind();
            cmbDefaultImage.DataSource = Session["DataDetail"];
            cmbDefaultImage.DataBind();
        }
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Production/ProductionDashboard.aspx");
        }
        protected void btnAddPart_Click(object sender, EventArgs e)
        {
            pcFaultPart.ShowOnPageLoad = true;

            ////var Code = PUcbPartProces.Text;
            ////var Description = PUcbPartProcesDesc.Text;
            ////ProductionFaultAuditRepository.SaveDataAuditParts(Code, Description);
            //PUcbPartProces.ClientEnabled = true;
            //PUcbPartProcesDesc.ClientEnabled = true;
            //PUcbPartProces.Text = null;
            //PUcbPartProcesDesc.Text = null;
            //PUcbPartProces.NullText = "Please Select";
            //PUcbPartProcesDesc.NullText = "Please Select";
            //PUcbPartProces.DataSource = sdsFaultPartProcess;
            //PUcbPartProces.DataBind();
            //PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
            //PUcbPartProcesDesc.DataBind();

        }
        protected void pcFaultDescSave_Click(object sender, EventArgs e)
        {
            //TODO
            string csType = Session["CsType"].ToString();
            string Code = pcFaultDescCode.Text;
            string Desc = pcFaultDescDescription.Text;
            if (csType == "VoCA")
            {
                ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, true, false, false);
            }
            else
            {
                ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, false, true, false);
            }

            if (Session["CsType"].ToString() == "VoCA")
            {
                var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
                cbFaultDesc.DataSource = data;
                cbFaultDesc.DataBind();
                cbFaultDescDesc.DataSource = data;
                cbFaultDescDesc.DataBind();
            }
            else
            {
                var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
                cbFaultDesc.DataSource = data;
                cbFaultDesc.DataBind();
                cbFaultDescDesc.DataSource = data;
                cbFaultDescDesc.DataBind();
            }

            pcFaultDesc.ShowOnPageLoad = false;
        }
        protected void pcFaultDescCancel_Click(object sender, EventArgs e)
        {
            //TODO
            pcFaultDesc.ShowOnPageLoad = false;
        }
        protected void pcFaultPartSave_Click(object sender, EventArgs e)
        {
            //TODO
            string Code = pcFaultPartCode.Text;
            string Desc = pcFaultPartDescription.Text;
            ProductionFaultAuditRepository.SaveDataAuditParts(Code, Desc);

            PUcbPartProces.DataSource = sdsFaultPartProcess;
            PUcbPartProces.DataBind();
            PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
            PUcbPartProcesDesc.DataBind();

            pcFaultPart.ShowOnPageLoad = false;
        }
        protected void pcFaultPartCancel_Click(object sender, EventArgs e)
        {
            //TODO
            pcFaultPart.ShowOnPageLoad = false;
        }
        protected void btnAddFaultDesc_Click(object sender, EventArgs e)
        {
            pcFaultDesc.ShowOnPageLoad = true;

            ////string csType = Session["CsType"].ToString();
            ////string Code = cbFaultDesc.Text;
            ////string Desc = cbFaultDescDesc.Text;
            ////if (csType == "VoCA")
            ////{
            ////    ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, true,false,false);
            ////}
            ////else
            ////{
            ////    ProductionFaultAuditRepository.SaveDataAuditDescription(Code, Desc, false, true, false);
            ////}
            //cbFaultDesc.ClientEnabled = true;
            //cbFaultDescDesc.ClientEnabled = true;
            //cbFaultDesc.Text = null;
            //cbFaultDescDesc.Text = null;
            //cbFaultDesc.NullText = "Please Select";
            //cbFaultDescDesc.NullText = "Please Select";
            //if (Session["CsType"].ToString() == "VoCA")
            //{
            //    var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
            //    cbFaultDesc.DataSource = data;
            //    cbFaultDesc.DataBind();
            //    cbFaultDescDesc.DataSource = data;
            //    cbFaultDescDesc.DataBind();
            //}
            //else
            //{
            //    var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
            //    cbFaultDesc.DataSource = data;
            //    cbFaultDesc.DataBind();
            //    cbFaultDescDesc.DataSource = data;
            //    cbFaultDescDesc.DataBind();
            //}

        }
        //End event click
        protected void uplDocumentWI_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            string FINNumber = Session["FinNumber"].ToString();
            string PathName = AppConfiguration.UPLOAD_DIR + "/VPCReleaseFault/" + FINNumber;
            if (!Directory.Exists(Server.MapPath(PathName)))
            {
                Directory.CreateDirectory(Server.MapPath(PathName));
            }

            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            //string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileName = e.UploadedFile.FileName;
            byte[] resultFileByte = e.UploadedFile.FileBytes;
            string resultFileUrl = PathName + "/" + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);

            e.UploadedFile.SaveAs(resultFilePath);

            // UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

            string name = e.UploadedFile.FileName;
            string url = ResolveClientUrl(resultFileUrl);
            long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";
            e.CallbackData = name + "|" + url + "|" + sizeText;
            var oldattachImage = Session["ByteImage"] as List<FaultAuditAttachmentModel>;

            if (oldattachImage == null)
            {
                FaultAuditAttachmentModel AttachFile = new FaultAuditAttachmentModel();
                AttachFile.Id = GetIdAttachment();
                AttachFile.FileName = resultFileName;
                AttachFile.FileLocation = resultFileUrl;
                AttachFile.FileType = resultExtension;
                AttachFile.FileBinnary = resultFileByte;
                AttachFileModelList.Add(AttachFile);
                ListAttachment.attachmentAuditModels = AttachFileModelList;
            }
            else
            {
                FaultAuditAttachmentModel AttachFile = new FaultAuditAttachmentModel();
                //for (int i = 0; i < oldattachImage.Count; i++)
                //{
                //    AttachFile.Id = oldattachImage[i].Id;
                //    AttachFile.FileName = oldattachImage[i].FileName;
                //    AttachFile.FileLocation = oldattachImage[i].FileLocation;
                //    AttachFile.FileType = oldattachImage[i].FileType;
                //    AttachFile.FileBinnary = oldattachImage[i].FileBinnary;
                //    AttachFileModelList.Add(AttachFile);
                ListAttachment.attachmentAuditModels = oldattachImage;
                Session["ByteImage"] = null;
                //}
                FaultAuditAttachmentModel AttachFile2 = new FaultAuditAttachmentModel();
                AttachFile2.Id = GetIdAttachment();
                AttachFile2.FileName = resultFileName;
                AttachFile2.FileLocation = resultFileUrl;
                AttachFile2.FileType = resultExtension;
                AttachFile2.FileBinnary = resultFileByte;
                ListAttachment.attachmentAuditModels.Add(AttachFile2);
                //ListAttachment.attachmentAuditModels = AttachFileModelList;
            }

            //Session["ByteImage"] = null;
            Session["SaveByte"] = ListAttachment.attachmentAuditModels;
            DataTable table = ConvertListToDataTable(ListAttachment.attachmentAuditModels);
            //if (Session["DataDetail"] != null)
            //{
            //    DataTable Datax = Session["DataDetail"] as DataTable;
            //    foreach (DataRow dr in Datax.Rows)
            //    {

            //        table.Rows.Add(dr.ItemArray);
            //        table.AcceptChanges();
            //    }
            //    Session["dtAttachment"] = null;
            //    Session["dtAttachment"] = table;
            //    Session["DataDetail"] = null;
            //    Session["DataDetail"] = table;
            //    //Session["ByteImage"] = table;
            //    cmbDefaultImage.DataSource = Session["DataDetail"];
            //    cmbDefaultImage.DataBind();
            //}
            // cbFINNumber.Text = HiddenText["FINNumber"].ToString();
            Session["dtAttachment"] = null;
            Session["dtAttachment"] = table;
            Session["DataDetail"] = null;
            Session["DataDetail"] = table;
            //Session["ByteImage"] = table;
            cmbDefaultImage.DataSource = Session["DataDetail"];
            cmbDefaultImage.DataBind();
        }
        static DataTable ConvertListToDataTable(List<FaultAuditAttachmentModel> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = list.Count();


            // Add columns.

            table.Columns.Add("Id");
            table.Columns.Add("FileName");
            table.Columns.Add("FileLocation");
            table.Columns.Add("FileBinnary");
            table.Columns.Add("FileType");
            table.Columns.Add("IsCheck");


            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array.Id, array.FileName, array.FileLocation, array.FileBinnary, array.FileType, bool.Parse("false"));
            }

            return table;
        }
        protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            pcFaulRecordDetail.ShowOnPageLoad = false;
            pcFaulRecordDetail.ShowOnPageLoad = true;
            PopUpAttachMent.ShowOnPageLoad = false;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
            cmbDefaultImage.DataSource = Session["dtAttachment"];
            cmbDefaultImage.DataBind();
        }
        protected void pcInteruptFaultRecord_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            pcFaulRecordDetail.ShowOnPageLoad = false;
            pcFaulRecordDetail.ShowOnPageLoad = true;
            PopUpAttachMent.ShowOnPageLoad = false;
            GVDataAttachment.DataSource = Session["dtAttachment"];
            GVDataAttachment.DataBind();
            cmbDefaultImage.DataSource = Session["dtAttachment"];
            cmbDefaultImage.DataBind();
        }
        protected void pcFaulRecordDetail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int faultDescId;
            if (e.Parameter.ToString().Split('$')[0].ToString() == "PartDescription")
            {
                var x = e;
                if (e.Parameter.ToString().Split('$')[1].ToString() != "-1")
                {
                    int IdDescFault = int.Parse(e.Parameter.ToString().Split('$')[1].ToString());
                    PUcbPartProcesDesc.Value = IdDescFault;
                    //PUcbPartProcesDesc.ReadOnly = true;
                    //PUcbPartProces.ReadOnly = true;
                }
                else
                {
                    if (PUcbPartProces.Text.ToString() == "")
                    {
                        // PUcbPartProcesDesc.ReadOnly = false;
                        PUcbPartProcesDesc.Text = null;
                        PUcbPartProcesDesc.NullText = "Please Select";
                    }
                }
            }
            if (e.Parameter.ToString().Split('$')[0].ToString() == "PUcbPartProcesDesc")
            {
                var x = e;
                if (e.Parameter.ToString().Split('$')[1].ToString() != "-1")
                {
                    int IdDescFault = int.Parse(e.Parameter.ToString().Split('$')[1].ToString());
                    PUcbPartProces.Value = IdDescFault;
                    //PUcbPartProcesDesc.ReadOnly = true;
                    //PUcbPartProces.ReadOnly = true;
                }
                else
                {
                    if (PUcbPartProcesDesc.Text.ToString() == "")
                    {
                        //PUcbPartProces.ReadOnly = false;
                        PUcbPartProces.Text = null;
                        PUcbPartProces.NullText = "Please Select";
                    }
                }
            }
            if (e.Parameter.ToString().Split('$')[0].ToString() == "cbFaultDesc")
            {
                var x = e;
                if (e.Parameter.ToString().Split('$')[1].ToString() != "-1")
                {
                    int IdDescFault = int.Parse(e.Parameter.ToString().Split('$')[1].ToString());
                    cbFaultDescDesc.Value = IdDescFault;
                    //cbFaultDescDesc.ReadOnly = true;
                    // cbFaultDesc.ReadOnly = true;
                }
                else
                {
                    if (cbFaultDesc.Text.ToString() == "")
                    {
                        //cbFaultDescDesc.ReadOnly = false;
                        cbFaultDescDesc.Text = null;
                        cbFaultDescDesc.NullText = "Please Select";

                    }
                }
            }
            if (e.Parameter.ToString().Split('$')[0].ToString() == "cbFaultDescDesc")
            {
                if (e.Parameter.ToString().Split('$')[1].ToString() != "-1")
                {
                    var x = e;
                    int IdDescFault = int.Parse(e.Parameter.ToString().Split('$')[1].ToString());
                    cbFaultDesc.Value = IdDescFault;
                    //  cbFaultDesc.ReadOnly = true;
                    // cbFaultDescDesc.ReadOnly = true;
                }
                else
                {
                    if (cbFaultDescDesc.Text.ToString() == "")
                    {
                        //  cbFaultDesc.ReadOnly = false;
                        //cbFaultDesc.Text = null;
                        cbFaultDesc.NullText = "Please Select";
                    }
                }
            }
            if (Session["CsType"].ToString() == "VoCA")
            {
                var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", true, false, false);
                cbFaultDesc.DataSource = data;
                cbFaultDesc.DataBind();
                cbFaultDescDesc.DataSource = data;
                cbFaultDescDesc.DataBind();
            }
            else
            {
                var data = ProductionFaultAuditRepository.RetrieveDataFaultDesc("Usp_FaultAuditGetFaultDescription", false, true, false);
                cbFaultDesc.DataSource = data;
                cbFaultDesc.DataBind();
                cbFaultDescDesc.DataSource = data;
                cbFaultDescDesc.DataBind();
            }

            PUcbPartProces.DataSource = sdsFaultPartProcess;
            PUcbPartProces.DataBind();
            PUcbPartProcesDesc.DataSource = sdsFaultPartProcess;
            PUcbPartProcesDesc.DataBind();
            pcFaulRecordDetail.ShowOnPageLoad = false;
            pcFaulRecordDetail.ShowOnPageLoad = true;
            PopUpAttachMent.ShowOnPageLoad = false;
            //GVDataAttachment.DataSource = Session["DataDetail"];
            //GVDataAttachment.DataBind();
            if (Session["dtAttachment"] != null)
            {
                cmbDefaultImage.DataSource = Session["dtAttachment"];
                cmbDefaultImage.DataBind();
                GVDataAttachment.DataSource = Session["dtAttachment"];
                GVDataAttachment.DataBind();
            }

            //if (Session["DataDetail"] != null)
            //{
            //    GVDataAttachment.DataSource = Session["DataDetail"];
            //    GVDataAttachment.DataBind();
            //}
        }
        protected int GetIdAttachment()
        {
            int IdData = 0;
            var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
            IdData = Id;
            return IdData;
        }

        //Action Header Main
        #region HeaderMainAction

        private void loadFaultDefaultHeader()
        {
            pcInteruptFaultRecord.ShowOnPageLoad = true;
            try
            {

                //var ParameterStation = ComboBox_Station.SelectedItem.Value;
                //var ValueId = GvDetailsFaultRecord.GetRowValues(index, "Id");

                CSTypeValue.Text = "";
                AuditDates.Value = "";
                cbFINNumber.Text = "";
                TxPlant.Text = "";
                TxMileage.Text = "";
                Auditor1.Text = "";
                Auditor2.Text = "";

            }

            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
            }
        }
        //End
        #endregion
        //Get Data Web Method
        #region WebMethod
        [WebMethod]
        //get Line by HistoryId
        public static string GetLine(int values)
        {

            var tempLine = ProductionRepository.GetLineByProductionId(values);
            return tempLine.LineName;
        }

        [WebMethod]
        //get Line by HistoryId
        public static List<ProdHistoryModel> GetFinNumber(int values)
        {
            var tempLine = FaultRepository.GetFinNumber(values);
            return tempLine;
        }

        public static string GetLineDashBoardNotNUll(int values)
        {

            var tempLine = ProductionRepository.GetLineByProductionId(values);
            return tempLine.LineName.ToString();
        }

        [WebMethod]
        //get AssemblySection by StationId
        public static string GetSection(int values)
        {
            var tempSection = StationRepository.RetrieveAsemblySectionByStId(values);
            return tempSection;
        }
        public static string GetSectioneDashBoardNotNUll(int values)
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
        public static string GetClassificationDashBoardNotNUll(int values)
        {
            var tempClassification = FaultRepository.GetFaultClassification(values);
            if (tempClassification == null) return "-";
            return tempClassification.Description;
        }
        [WebMethod]
        public static string GetType(string values)
        {
            var tempType = FaultRepository.GetType(values);
            return tempType;
        }
        public static string GetTypeDashBoardNotNUll(string values)
        {
            var tempType = FaultRepository.GetType(values);
            return tempType;
        }
        [WebMethod]
        public static string GetColor(string values)
        {
            var tempType = FaultRepository.GetColor(values);
            return tempType;
        }
        public static string GetColorDashBoardNotNUll(string values)
        {
            var tempType = FaultRepository.GetColor(values);
            return tempType;
        }
        [WebMethod]
        public static string GetBauMaster(string values)
        {
            var tempType = FaultRepository.GetBauMaster(values);
            return tempType;
        }
        public static string GetBauMasterDashBoardNotNUll(string values)
        {
            var tempType = FaultRepository.GetBauMaster(values);
            return tempType;
        }
        #endregion
        #region Detail

        private void loadFaultDefaultEdit()
        {
            // Find Row Expanded
            IdFaultAudit.ClientVisible = true;
            TextId.ClientVisible = true;
            tbAssemblySection.ClientVisible = false;
            var Index = int.Parse(Session["IndexDetails"].ToString());
            var GvData = GvMainFaultRecord.FindDetailRowTemplateControl(Index, "GvDetailsFaultRecord") as ASPxGridView;
            //var ParameterStation = ComboBox_Station.SelectedItem.Value;
            //var ValueId = GvDetailsFaultRecord.GetRowValues(index, "Id");
            PUcbPartProces.Text = "";
            //PUcbPartProces.Text = DtTbEdit.Rows[0].ItemArray[1].ToString();
            cbStation.Text = "";
            tbAssemblySection.Text = "";
            cbFaultDesc.Text = "";
            cbFaultDescDesc.Text = "";
            cbFaultDesc.NullText = "Please Select";
            cbFaultDescDesc.NullText = "PleaseSelet";

            PUcbPartProces.Text = "";
            PUcbPartProcesDesc.Text = "";
            PUcbPartProces.NullText = "Please Select";
            PUcbPartProcesDesc.NullText = "PleaseSelet";

            cmb_Area.Text = "";
            cbPriority.Text = "";
            cmbReponsibleLine.Text = "";
            cmbAnalysis.Text = "";
            cmbRework.Text = "";
            txRemarks.Text = "";
            txDescDetail.Text = "";
            cmbDefaultImage.Text = "";
            GVDataAttachment.DataSource = "";
            //Session["DataDetail"] = "";
            Session["dtAttachment"] = "";

            GVDataAttachment.DataSource = string.Empty;
            GVDataAttachment.DataBind();
            cmbDefaultImage.DataSource = string.Empty;
            cmbDefaultImage.DataBind();
        }



        #endregion

        protected void GvMainFaultRecord_DetailsChanged(object sender, EventArgs e)
        {

        }
        protected void PUcbPartProces_Callback(object sender, CallbackEventArgsBase e)
        {
            if (PUcbPartProces.Value != null)
            {
                int MaterialId = int.Parse(PUcbPartProces.Value.ToString());
                PUcbPartProcesDesc.Value = MaterialId;
            }
        }
        protected void GvMainFaultRecord_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "DeleteDetails")
            {
                if (!permissions.Contains(EPermissionType.Delete))
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
        protected void GvDetailsFaultRecord_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.ButtonID == "DeleteHeader")
            {
                if (!permissions.Contains(EPermissionType.Delete))
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
        //End GetData Web Method


    }
}