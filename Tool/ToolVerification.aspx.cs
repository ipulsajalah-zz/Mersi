using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using DevExpress.Web;
using DotWeb.Repositories;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotWeb;
using DotWeb.Utils;
using DotWeb.Models;
using DevExpress.XtraPrinting;
using DevExpress.Export;
//using DevExpress.XtraGauges.Core.Drawing;
using System.Drawing;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;

namespace DotMercy.custom.ToolManagement
{
    public partial class ToolVerification : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDDLToolNumber();
                GetDDLProductionLine();
                GetDDLStation();
            }
           
        }
        protected void GetDDLToolNumber()
        {
            try
            {
                ComboBox_ToolNumber.DataSource = AssignToolRepository.RetrieveDDLToolNumber();
                ComboBox_ToolNumber.TextField = "ToolNumber";
                ComboBox_ToolNumber.ValueField = "ToolSetupId";
                ComboBox_ToolNumber.Value = "-Select-";
                //ComboBox_ToolNumber.Value = "ToolSetupId";
                //ComboBox_ToolNumber.ID = "ToolSetupId";
                ComboBox_ToolNumber.DataBind();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected void GetDDLProductionLine()
        {
            try
            {
                ComboBox_ProductionLine.DataSource = AssignToolRepository.RetrieveProdLine();
                ComboBox_ProductionLine.TextField = "ProductionLine";
                ComboBox_ProductionLine.ValueField = "Id";
                ComboBox_ProductionLine.Value = "-Select-";
                ComboBox_ProductionLine.DataBind();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected void SelectedChangedToolNumber(object sender, EventArgs e)
        {
            ComboBox_InventoryNumber.DataSource = AssignToolRepository.RetrieveDDLInventoryNumber((ComboBox_ToolNumber.SelectedItem.Text).ToString());
            ComboBox_InventoryNumber.TextField = "InventoryNumber";
            ComboBox_InventoryNumber.Value = "-Select-";
            ComboBox_InventoryNumber.DataBind();
        }
        protected void ShowGv(object sender, EventArgs e)
        {


            DataTable dtVerification = new DataTable();

            ToolCalibrationRepository RepoCalibration = new ToolCalibrationRepository();
            var ParameterProductionLine = ComboBox_ProductionLine.SelectedItem == null ? "0" : ComboBox_ProductionLine.SelectedItem.Value;
            var ParameterStation = ComboBox_Station.SelectedItem == null ? "0" : ComboBox_Station.SelectedItem.Value.ToString();
            var ParameterInventoryNumber = ComboBox_InventoryNumber.SelectedItem == null ? "0" : ComboBox_InventoryNumber.SelectedItem.Value.ToString();
            var ParameterToolNumber = ComboBox_ToolNumber.SelectedItem == null ? "0" : ComboBox_ToolNumber.SelectedItem.Value.ToString();
            dtVerification = RepoCalibration.RetrieveGridData("usp_Tool_GetPendingVerifications",ParameterToolNumber.ToString(), ParameterProductionLine.ToString(), ParameterStation.ToString(), ParameterInventoryNumber.ToString());
            dtVerification.PrimaryKey = new DataColumn[] { dtVerification.Columns["Id"] };

            //store datasource in session
            Session["dtVerification"] = dtVerification;
            GridviewToolVerification.DataBind();
        }
        protected void GetDDLStation()
        {
            try
            {
                ComboBox_Station.DataSource = AssignToolRepository.RetrieveStation();
                ComboBox_Station.TextField = "StationName";
                ComboBox_Station.Value = "-Select-";
                ComboBox_Station.ValueField = "IdTool";
                ComboBox_Station.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void detailGrid_DataSelect(object sender, EventArgs e)
        {
            //Session["Id"] = ASPxGridView1.GetCurrentPageRowValues("Id");
            //Session["InventoryNumber"] = (sender as ASPxGridView).GetMasterRowKeyValue();
            //ToolVerificationRepository.RetrieveDataToolVerification(1, 1);

        }

        protected void GridviewToolVerification_DetailChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            double NilMax = 0;
            double NilMin = 0;
            ASPxGridView some = (ASPxGridView)sender;
            var colapse = some.DetailRows.VisibleCount;
            if (colapse == 1)
            {
                IList<ToolVerification> result = new List<ToolVerification>();
                var rowIndex = e.VisibleIndex;
                var ValueId = GridviewToolVerification.GetRowValues(rowIndex,"ToolId");
                var ValueToolNumber = GridviewToolVerification.GetRowValues(rowIndex,"ToolNumber");
                var ValueInventory = GridviewToolVerification.GetRowValues(rowIndex, "Id");
                var ValSetNm = GridviewToolVerification.GetRowValues(rowIndex, "SetNM");
                var ValueToolSetupId = GridviewToolVerification.GetRowValues(rowIndex, "Id");
                var ValNextDay = GridviewToolVerification.GetRowValues(rowIndex,"NextVerificationDate");
                var ValueNextVerificationDate = GridviewToolVerification.GetCurrentPageRowValues("Id")[rowIndex];
                //Perhitungan SetNM
                var convertIntValSetNM = ValSetNm == null ? "" : ValSetNm.ToString();
                if (float.Parse(convertIntValSetNM.Split(',')[0]) < 10)
                {
                    var Persentase = float.Parse(convertIntValSetNM.Split('.')[0]) * 0.02;
                    NilMax = float.Parse(convertIntValSetNM.Split('.')[0]) + Persentase;
                    NilMin = float.Parse(convertIntValSetNM.Split('.')[0]) - Persentase;
                    HiddenMax.Value = NilMax.ToString();
                    HiddenMin.Value = NilMin.ToString();
                }
                else
                {
                    var Persentase = float.Parse(convertIntValSetNM.Split('.')[0]) * 0.03;
                    NilMax = float.Parse(convertIntValSetNM.Split('.')[0]) + Persentase;
                    NilMin = float.Parse(convertIntValSetNM.Split('.')[0]) - Persentase;
                    HiddenMax.Value = NilMax.ToString();
                    HiddenMin.Value = NilMin.ToString();
                }

                var x = GridviewToolVerification.FindDetailRowTemplateControl(rowIndex, "LayoutDetails") as ASPxFormLayout;
                var GetDataCalibrationDate = x.FindControl("lblCalibrationDate") as ASPxLabel;
                var GetDataSetNM = x.FindControl("SetNM") as ASPxLabel;
                GetDataSetNM.Text = ValSetNm.ToString();
                var GetDataLastVerificationDate = x.FindControl("lblVerificationDate") as ASPxLabel;
                GetDataLastVerificationDate.Text = ValNextDay.ToString() == "" ? (DateTime.Now).ToString() :ValNextDay.ToString() ;
                var GetDataInventoryNumber = x.FindControl("lblInventoryNumber") as ASPxLabel;
                GetDataInventoryNumber.Text = ValueToolNumber.ToString();
                var GetSetNm = x.FindControl("Text_NM") as ASPxTextBox;
                var GetSetMin = x.FindControl("Text_Min") as ASPxTextBox;
                var GetSetMax = x.FindControl("Text_Max") as ASPxTextBox;
                GetSetNm.Text = ValSetNm.ToString();
                GetSetMax.Text = NilMax.ToString();
                GetSetMin.Text = NilMin.ToString();

                var ButtonMarks = x.FindControl("Btn_mark") as ASPxButton;
                hiddenvalue.Value = (0).ToString();
                var BtnSave = x.FindControl("BtnSave") as ASPxButton;
                var Btn_Verify = x.FindControl("Btn_Verify") as ASPxButton;
            }
        }

        protected void Btn_mark_Click(object sender, EventArgs e)
        {
            double NilMax = 0;
            double NilMin = 0;
            var SumRowData = GridviewToolVerification.VisibleRowCount;
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Request Calibration Success !');", true);
            for (int i = 0; i < SumRowData; i++)
            {
                var varGetData = GridviewToolVerification.FindDetailRowTemplateControl(i, "LayoutDetails") as ASPxFormLayout;
                if (varGetData != null){
                    var ValSetNm = GridviewToolVerification.GetRowValues(i, "SetNM");
                    var convertIntValSetNM = ValSetNm == null ? "" : ValSetNm.ToString();
                    if (float.Parse(convertIntValSetNM.Split(',')[0]) < 10)
                    {
                        var Persentase = float.Parse(convertIntValSetNM.Split('.')[0]) * 0.02;
                        NilMax = float.Parse(convertIntValSetNM.Split('.')[0]) + Persentase;
                        NilMin = float.Parse(convertIntValSetNM.Split('.')[0]) - Persentase;
                        HiddenMax.Value = NilMax.ToString();
                        HiddenMin.Value = NilMin.ToString();
                    }
                    else
                    {
                        var Persentase = float.Parse(convertIntValSetNM.Split('.')[0]) * 0.03;
                        NilMax = float.Parse(convertIntValSetNM.Split('.')[0]) + Persentase;
                        NilMin = float.Parse(convertIntValSetNM.Split('.')[0]) - Persentase;
                        HiddenMax.Value = NilMax.ToString();
                        HiddenMin.Value = NilMin.ToString();
                    }
                    var GetSetNm = varGetData.FindControl("Text_NM") as ASPxTextBox;
                    var GetSetMin = varGetData.FindControl("Text_Min") as ASPxTextBox;
                    var GetSetMax = varGetData.FindControl("Text_Max") as ASPxTextBox;
                    GetSetNm.Text = ValSetNm.ToString();
                    GetSetMax.Text = NilMax.ToString();
                    GetSetMin.Text = NilMin.ToString();
                    var ValueId = GridviewToolVerification.GetRowValues(i,"ToolId");
                    var ValueInventory = GridviewToolVerification.GetRowValues(i,"Id");
                    //var ValSetNm = GridviewToolVerification.GetRowValues(i,"SetNM");
                    var ValueToolSetupId = GridviewToolVerification.GetRowValues(i,"Id");
                    var ValueNextVerificationDate = GridviewToolVerification.GetRowValues(i,"Id");
                    //var iList = ToolVerificationRepository.RetrieveDataToolVerification("ToolVerification_GetData", ValueId.ToString(), ValueInventory.ToString());
                    var Attempt = varGetData.FindControl("lbl_Attempt") as ASPxTextBox;
                    var ValAttempt = Attempt.Text == "ASPxLabel" ? 1 : int.Parse(Attempt.Text) + 1;
                    var InvString = ValueInventory.ToString();
                    var NextDayGet = GridviewToolVerification.GetRowValues(i, "VerDay");
                    DateTime today = DateTime.Now;
                    ToolVerificationRepository.SaveChangeNextVerificationDate(ValueInventory.ToString());
                    if (ValAttempt >= 4)
                    {
                        ValAttempt = 3;
                    }
                    for (int u = 0; u < ValAttempt; u++)
                    {
                        var VerifNumb = u + 1;
                        var GetVerDate = varGetData.FindControl("lblVerificationDate") as ASPxLabel;
                        var GetToolSetupId = varGetData.FindControl("lblInventoryNumber") as ASPxLabel;
                        var GetText_NM = varGetData.FindControl("Text_NM") as ASPxTextBox;
                        var GetText_Min = varGetData.FindControl("Text_Min") as ASPxTextBox;
                        var GetText_Max = varGetData.FindControl("Text_Max") as ASPxTextBox;
                        var GetVerification1 = varGetData.FindControl("text_ver" + VerifNumb + "_" + "1") as ASPxTextBox;
                        var GetVerification2 = varGetData.FindControl("text_ver" + VerifNumb + "_" + "2") as ASPxTextBox;
                        var GetVerification3 = varGetData.FindControl("text_ver" + VerifNumb + "_" + "3") as ASPxTextBox;
                        var ValCalNumber = "1";
                        var ValTollSetupId = ValueToolSetupId.ToString();
                        var ValTollInv = ValueInventory.ToString();
                        var ValText_NM = GetText_NM.Text;
                        var ValText_Min = GetText_Min.Text;
                        var ValText_Max = GetText_Max.Text;
                        var ValText_Verification1 = GetVerification1.Text;
                        var ValText_Verification2 = GetVerification2.Text;
                        var ValText_Verification3 = GetVerification3.Text;
                        var VerifNumberString = VerifNumb.ToString();
                        DateTime ValueNext = today.AddDays(int.Parse(NextDayGet.ToString()));
                        User user = (User)Session["user"];
                        var CreatedBy = user.UserName;
                        ToolVerificationRepository.SaveDataToolVerificationMark(ValueNext, VerifNumberString, InvString, "ToolVerificationInsert", ValCalNumber, ValueId.ToString(), ValTollInv, ValText_NM, ValText_Min, ValText_Max, ValText_Verification1, ValText_Verification2, ValText_Verification3, "0", "", ValTollInv, "0",CreatedBy);
                   
                    }

                    
                    
                }

            }
            Response.Write(@"
                <script>
                alert('Request Calibration Success');
                setTimeout(function(){            
                window.location = '" + Request.RawUrl + @"';
                }, 2000);
                </script>");            
            //Response.Redirect(Request.RawUrl);
        }

        protected void Btn_Save(object sender, EventArgs e)
        {
            try
            {
            double NilMax = 0;
            double NilMin = 0;
            var SumRowData = GridviewToolVerification.VisibleRowCount;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Verification Success !');", true);
            for (int i = 0; i < SumRowData; i++)
            {
                //var rowIndex = e.VisibleIndex;
               
                var varGetData = GridviewToolVerification.FindDetailRowTemplateControl(i, "LayoutDetails") as ASPxFormLayout;
                if (varGetData != null)
                {

                    var ValSetNm = GridviewToolVerification.GetRowValues(i, "SetNM");
                    var convertIntValSetNM = ValSetNm == null ? "" : ValSetNm.ToString();
                    if (float.Parse(convertIntValSetNM.Split(',')[0]) < 10)
                    {
                        var Persentase = float.Parse(convertIntValSetNM.Split('.')[0]) * 0.02;
                        NilMax = float.Parse(convertIntValSetNM.Split('.')[0]) + Persentase;
                        NilMin = float.Parse(convertIntValSetNM.Split('.')[0]) - Persentase;
                        HiddenMax.Value = NilMax.ToString();
                        HiddenMin.Value = NilMin.ToString();
                    }
                    else
                    {
                        var Persentase = float.Parse(convertIntValSetNM.Split('.')[0]) * 0.03;
                        NilMax = float.Parse(convertIntValSetNM.Split('.')[0]) + Persentase;
                        NilMin = float.Parse(convertIntValSetNM.Split('.')[0]) - Persentase;
                        HiddenMax.Value = NilMax.ToString();
                        HiddenMin.Value = NilMin.ToString();
                    }
                    var GetSetNm = varGetData.FindControl("Text_NM") as ASPxTextBox;
                    var GetSetMin = varGetData.FindControl("Text_Min") as ASPxTextBox;
                    var GetSetMax = varGetData.FindControl("Text_Max") as ASPxTextBox;
                    GetSetNm.Text = ValSetNm.ToString();
                    GetSetMax.Text = NilMax.ToString();
                    GetSetMin.Text = NilMin.ToString();
                        var ValueId = GridviewToolVerification.GetRowValues(i, "ToolId");
                        var ValueInventory = GridviewToolVerification.GetRowValues(i, "Id");
                    var NextDayGet = GridviewToolVerification.GetRowValues(i, "VerDay");
                    DateTime today = DateTime.Now;
               
                        var ValueToolSetupId = GridviewToolVerification.GetRowValues(i, "Id");
                        var ValueNextVerificationDate = GridviewToolVerification.GetRowValues(i, "Id");
                    //var iList = ToolVerificationRepository.RetrieveDataToolVerification("ToolVerification_GetData", ValueId.ToString(), ValueInventory.ToString());
                    var Attempt = varGetData.FindControl("lbl_Attempt") as ASPxTextBox;
                        var ValAttempt = Attempt.Text == "ASPxLabel" ? 1 : int.Parse(Attempt.Text) + 1;
                    var InvString = ValueInventory.ToString();
                    if (ValAttempt > 3)
                    {
                        ValAttempt = 3;
                    }
                    for (int u = 0; u < ValAttempt; u++)
                    {
                        var VerifNumb = u + 1;
                        var GetVerDate = varGetData.FindControl("lblVerificationDate") as ASPxLabel;
                        var GetToolSetupId = varGetData.FindControl("lblInventoryNumber") as ASPxLabel;
                        var GetText_Min = varGetData.FindControl("Text_Min") as ASPxTextBox;
                        var GetText_Max = varGetData.FindControl("Text_Max") as ASPxTextBox;
                        var GetVerification1 = varGetData.FindControl("text_ver" + VerifNumb + "_" + "1") as ASPxTextBox;
                        var GetVerification2 = varGetData.FindControl("text_ver" + VerifNumb + "_" + "2") as ASPxTextBox;
                        var GetVerification3 = varGetData.FindControl("text_ver" + VerifNumb + "_" + "3") as ASPxTextBox;
                        //var ValText_VerDate = iList[0].LastVerificationDate.ToString();
                        var ValCalNumber = "1";
                        var ValTollSetupId = ValueToolSetupId.ToString();
                        var ValTollInv = ValueInventory.ToString();
                        var ValText_NM = ValSetNm.ToString();
                        var ValText_Min = GetText_Min.Text;
                        var ValText_Max = GetText_Max.Text;
                        var ValText_Verification1 = GetVerification1.Text;
                        var ValText_Verification2 = GetVerification2.Text;
                        var ValText_Verification3 = GetVerification3.Text;
                        var VerifNumberString = VerifNumb.ToString();
                        DateTime ValueNext = today.AddDays(int.Parse(NextDayGet.ToString() == "" ? "0" : NextDayGet.ToString()));
                        User user = (User)Session["user"];
                        var CreatedBy = user.UserName;
                        ToolVerificationRepository.SaveDataToolVerification(ValueNext, VerifNumberString, InvString, ValCalNumber, ValueId.ToString(), ValTollInv, ValText_NM, ValText_Min, ValText_Max, ValText_Verification1, ValText_Verification2, ValText_Verification3, ValTollInv, CreatedBy);    
                    }  
                }
            }
            Response.Write(@"
                <script>
                alert('Save Success');
                setTimeout(function(){            
                window.location = '" + Request.RawUrl + @"';
                }, 2000);
                </script>");   
            //Response.Redirect(Request.RawUrl);
        }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }

        protected void GridviewToolVerification_DataBinding(object sender, EventArgs e)
        {
            GridviewToolVerification.DataSource = Session["dtVerification"];
        }

        protected void ExportExcel_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });

        }

        protected void GridviewToolVerification_HtmlDataRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grv = (sender as ASPxGridView);
            if (grv.GetRowValues(e.VisibleIndex, "NextVerificationDate") != null)
            {
                if (grv.GetRowValues(e.VisibleIndex, "NextVerificationDate").ToString() != "")
                {
                    DateTime DateData = Convert.ToDateTime(grv.GetRowValues(e.VisibleIndex, "NextVerificationDate"));
                    var BeetWeenDay = (DateTime.Now - DateData).TotalDays;
                    if (BeetWeenDay > 1)
                    {
                        Color tlr = System.Drawing.ColorTranslator.FromHtml("#ff1a1a");
                        e.Row.BackColor = tlr;
                    }
                }
            }
        }

       
    }
}