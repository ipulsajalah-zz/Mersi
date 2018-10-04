using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
//using System.Linq;
using DotWeb;
using DotWeb.Utils;
using DotWeb.Models;
using DotWeb.Repositories;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using System.Drawing;
//using DevExpress.XtraGauges.Core.Drawing;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Models;

namespace DotMercy.custom.ToolManagement
{
    public partial class ToolCalibration : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetDDLToolNumber();

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
                ComboBox_ToolNumber.DataBind();
            }
            catch (Exception ex)
            {

                throw;
            }
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
        
        protected void ShowGv(object sender, EventArgs e)
        {

            DataTable dtCalibration = new DataTable();

            ToolCalibrationRepository repoIrregAlt = new ToolCalibrationRepository();
            var ParameterProductionLine = ComboBox_ProductionLine.SelectedItem == null ? "0" : ComboBox_ProductionLine.SelectedItem.Value;
            var ParameterStation = ComboBox_Station.SelectedItem == null ? "0" : ComboBox_Station.SelectedItem.Value.ToString();
            var ParameterInventoryNumber = ComboBox_InventoryNumber.SelectedItem == null ? "0" : ComboBox_InventoryNumber.SelectedItem.Value.ToString();
            var ParameterToolNumber = ComboBox_ToolNumber.SelectedItem == null ? "0" : ComboBox_ToolNumber.SelectedItem.Value.ToString();
            dtCalibration = repoIrregAlt.RetrieveGridData("usp_Tool_GetPendingCalibrations", ParameterToolNumber.ToString(), ParameterProductionLine.ToString(), ParameterStation.ToString(), ParameterInventoryNumber.ToString());
            dtCalibration.PrimaryKey = new DataColumn[] { dtCalibration.Columns["Id"] };
            Session["dtCalibration"] = dtCalibration;
            GridviewToolVerification.DataSource = dtCalibration;
            GridviewToolVerification.KeyFieldName = "Id";

            GridviewToolVerification.DataBind();

        }
        protected void SelectedChangedToolNumber(object sender, EventArgs e)
        {
            ComboBox_InventoryNumber.DataSource = AssignToolRepository.RetrieveDDLInventoryNumber((ComboBox_ToolNumber.SelectedItem.Text).ToString());
            ComboBox_InventoryNumber.TextField = "InventoryNumber";
            ComboBox_InventoryNumber.Value = "-Select-";
            ComboBox_InventoryNumber.DataBind();
        }
        protected void DetailChangedTes(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            try
            {
                ASPxGridView some = (ASPxGridView)sender;
                var colapse = some.DetailRows.VisibleCount;
                if (colapse == 1)
                {

                    IList<ToolVerification> result = new List<ToolVerification>();
                    var rowIndex = e.VisibleIndex;

                    // var ValueId = GridviewToolVerification.GetRowValues(i, "ToolSetupId");
                    var ValueId = GridviewToolVerification.GetRowValues(rowIndex, "ToolId");
                    var ValueInventory = GridviewToolVerification.GetRowValues(rowIndex, "Id");
                    Session["VisibleIndex"] = ValueInventory.ToString();
                    var NextDayGet = GridviewToolVerification.GetRowValues(rowIndex, "NextDay");
                    var ToolNumber = GridviewToolVerification.GetRowValues(rowIndex, "ToolNumber");
                    DateTime today = DateTime.Now;
                    var MinNMGet = GridviewToolVerification.GetRowValues(rowIndex, "MinNM");
                    var MaxNMGet = GridviewToolVerification.GetRowValues(rowIndex, "MaxNM");
                    var ValSetNm = GridviewToolVerification.GetRowValues(rowIndex, "SetNM");
                    var ValueToolSetupId = GridviewToolVerification.GetRowValues(rowIndex, "Id");
                    var ValueNextCalibrationDate = GridviewToolVerification.GetRowValues(rowIndex, "NextCalibrationDate");
                    var CheckValue = ValueNextCalibrationDate.ToString() == "" ? 0 : 1;
                    HiddenCheckValue.Value = CheckValue.ToString();

                    var x = GridviewToolVerification.FindDetailRowTemplateControl(rowIndex, "LayoutDetails") as ASPxFormLayout;

                    var GetDataCalibrationDate = x.FindControl("lblCalibrationDate") as ASPxLabel;
                    GetDataCalibrationDate.Text = ValueNextCalibrationDate.ToString() == "" ? (DateTime.Now).ToString() : ValueNextCalibrationDate.ToString();
                    var GetNextDay = x.FindControl("DateDetails") as ASPxTextBox;
                    GetNextDay.Text = NextDayGet.ToString() == "" ? "0" : NextDayGet.ToString();
                    //DateTime today = DateTime.Now;
                    var GetNextDate = x.FindControl("DateDetailsCalender") as ASPxTextBox;
                    GetNextDate.Text = (today.AddDays(int.Parse(NextDayGet.ToString() == "" ? "0" : NextDayGet.ToString()))).ToString();

                    var GetDataInventoryNumber = x.FindControl("lblInventoryNumber") as ASPxLabel;
                    GetDataInventoryNumber.Text = ToolNumber.ToString();
                    var GetDataNmRange = x.FindControl("lblNmRange") as ASPxLabel;
                    GetDataNmRange.Text = MinNMGet.ToString() + '-' + MaxNMGet.ToString() + " Nm";
                    var ButtonMarks = x.FindControl("Btn_mark") as ASPxButton;
                    var BtnSave = x.FindControl("BtnSave") as ASPxButton;

                    var Btn_Verify = x.FindControl("Btn_Verify") as ASPxButton;

                    var lbl20 = x.FindControl("lbl20") as ASPxLabel;
                    lbl20.Text = "20% (" + Convert.ToDouble(MaxNMGet) * 0.2 + " Nm)";
                    var lbl40 = x.FindControl("lbl40") as ASPxLabel;
                    lbl40.Text = "40% (" + Convert.ToDouble(MaxNMGet) * 0.4 + " Nm)";
                    var lbl60 = x.FindControl("lbl60") as ASPxLabel;
                    lbl60.Text = "60% (" + Convert.ToDouble(MaxNMGet) * 0.6 + " Nm)";
                    var lbl80 = x.FindControl("lbl80") as ASPxLabel;
                    lbl80.Text = "80% (" + Convert.ToDouble(MaxNMGet) * 0.8 + " Nm)";
                    var lbl100 = x.FindControl("lbl100") as ASPxLabel;
                    lbl100.Text = "100% (" + Convert.ToDouble(MaxNMGet) * 1 + " Nm)";

                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        public IEnumerable<DateTime> GetDisabledDates()
        {
            List<DateTime> disabledDates = new List<DateTime>();

            disabledDates.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 7));
            disabledDates.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 11));
            disabledDates.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 13));
            disabledDates.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 17));
            disabledDates.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 19));

            return disabledDates;
        }
        #region edit
        //        protected void MarkSaveColaboration(object sender, EventArgs e)
        //        {
        //            //var te = hiddenvalue.Value;
        //            var SumRowData = GridviewToolVerification.VisibleRowCount;
        //            for (int i = 0; i < SumRowData; i++)
        //            {
        //                var ValueId = GridviewToolVerification.GetCurrentPageRowValues("Id")[i];
        //                var ValueToolSetupId = GridviewToolVerification.GetCurrentPageRowValues("ToolSetupId")[i];
        //                var ValueInventory = GridviewToolVerification.GetCurrentPageRowValues("InventoryNumber")[i];
        //                var iList = ToolCalibrationRepository.RetrieveDataToolCalibration(ValueId.ToString(), ValueInventory.ToString());
        //                var varGetData = GridviewToolVerification.FindDetailRowTemplateControl(i, "LayoutDetails") as ASPxFormLayout;
        //                if (varGetData != null)
        //                {
        //                    var GetVerDate = varGetData.FindControl("lblVerificationDate") as ASPxLabel;
        //                    var GetToolSetupId = varGetData.FindControl("lblInventoryNumber") as ASPxLabel;
        //                    var GetText_NM = varGetData.FindControl("Text_NM") as ASPxTextBox;
        //                    var GetText_Min = iList[0].Min;
        //                    var GetText_Max = iList[0].Max;
        //                    var GetCollaboration1 = varGetData.FindControl("Text_VerValue1") as ASPxTextBox;
        //                    var GetCollaboration2 = varGetData.FindControl("Text_VerValue2") as ASPxTextBox;
        //                    var GetCollaboration3 = varGetData.FindControl("Text_VerValue3") as ASPxTextBox;
        //                    var GetCollaboration4 = varGetData.FindControl("Text_VerValue4") as ASPxTextBox;
        //                    var GetCollaboration5 = varGetData.FindControl("Text_VerValue5") as ASPxTextBox;
        //                    var GetRemarks = varGetData.FindControl("RemarksDetail") as ASPxMemo;
        //                    var GetCallDate = DateTime.Now;
        //                    var GetDateNext = varGetData.FindControl("DateDetails") as ASPxDateEdit;
        //                    var ValText_VerDate = iList[0].LastVerificationDate.ToString();
        //                    var ValCalNumber = "1";
        //                    var ValTollSetupId = ValueToolSetupId.ToString();
        //                    var ValTollInv = ValueInventory.ToString();
        //                    var ValText_NM = GetText_NM.Text;
        //                    var ValText_Min = GetText_Min.ToString();
        //                    var ValText_Max = GetText_Max.ToString();
        //                    var ValText_Calibration1 = GetCollaboration1.Text;
        //                    var ValText_Calibration2 = GetCollaboration2.Text;
        //                    var ValText_Calibration3 = GetCollaboration3.Text;
        //                    var ValText_Calibration4 = GetCollaboration4.Text;
        //                    var ValText_Calibration5 = GetCollaboration5.Text;
        //                    var ValMemo_Remarks = GetRemarks.Text;
        //                    var NextVerDate = GetDateNext.Text;
        //                    ToolCalibrationRepository.SaveDataToolVerificationMark(ValText_VerDate, ValCalNumber, ValTollSetupId, ValTollInv, ValText_NM, ValText_Min, ValText_Max, ValText_Calibration1, ValText_Calibration2, ValText_Calibration3, ValText_Calibration4, ValText_Calibration5, NextVerDate, ValMemo_Remarks);
        //                }

        //            }




        //            //ASPxGridView grid = new ASPxGridView();
        //            //var rowIndex = grid.DetailRows.VisibleCount;

        //            Response.Redirect(Request.RawUrl);
        //        }


        #endregion
        protected void DateDetails(object sender, CalendarDayCellPreparedEventArgs e)
        {

            if (e.Date.DayOfWeek == DayOfWeek.Sunday || e.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                e.Cell.Attributes["style"] = "pointer-events: none; background-color: gray";
            }

        }
        protected void MarkAsCrapSave(object sender, EventArgs e)
        {
            //var te = hiddenvalue.Value;
            var SumRowData = GridviewToolVerification.VisibleRowCount;
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Mark As Scrap Success !');", true);
            for (int i = 0; i < SumRowData; i++)
            {
                var varGetData = GridviewToolVerification.FindDetailRowTemplateControl(i, "LayoutDetails") as ASPxFormLayout;
                if (varGetData != null)
                {
                    var ValueInventory = GridviewToolVerification.GetRowValues(i, "Id");
                    var InvString = ValueInventory.ToString();
                    var ValueId = GridviewToolVerification.GetRowValues(i, "ToolId");
                    //  var Attempt = varGetData.FindControl("lbl_Attempt") as ASPxTextBox;
                    DateTime today = DateTime.Now;
                    var MinNMGet = GridviewToolVerification.GetRowValues(i, "MinNM");
                    var MaxNMGet = GridviewToolVerification.GetRowValues(i, "MaxNM");
                    var ValSetNm = GridviewToolVerification.GetRowValues(i, "SetNM");
                    //var iList = ToolCalibrationRepository.RetrieveDataStaticSave("usp_ToolCalibration_GetDataStatic", ValueId.ToString(), ValueInventory.ToString());

                    var ValAttempt = lbl_Attempt.Text == "0" ? 1 : int.Parse(lbl_Attempt.Text) + 1;
                    var GetVerDate = varGetData.FindControl("lblVerificationDate") as ASPxLabel;
                    var GetToolSetupId = varGetData.FindControl("lblInventoryNumber") as ASPxLabel;
                    var GetText_NM = ValSetNm.ToString();
                    var GetText_Min = MinNMGet.ToString();
                    var GetText_Max = MaxNMGet.ToString();

                    if (ValAttempt > 3)
                    {
                        ValAttempt = 3;

                    }
                    for (int u = 0; u < ValAttempt; u++)
                    {
                        var VerifNumb = u + 1;
                        var GetCollaboration1 = varGetData.FindControl("Text_VerValue_" + VerifNumb + "_" + "1") as ASPxTextBox;
                        var GetCollaboration2 = varGetData.FindControl("Text_VerValue_" + VerifNumb + "_" + "2") as ASPxTextBox;
                        var GetCollaboration3 = varGetData.FindControl("Text_VerValue_" + VerifNumb + "_" + "3") as ASPxTextBox;
                        var GetCollaboration4 = varGetData.FindControl("Text_VerValue_" + VerifNumb + "_" + "4") as ASPxTextBox;
                        var GetCollaboration5 = varGetData.FindControl("Text_VerValue_" + VerifNumb + "_" + "5") as ASPxTextBox;
                        var GetRemarks = varGetData.FindControl("RemarksDetail") as ASPxMemo;
                        var GetCallDate = DateTime.Now;
                        var GetDateNext = varGetData.FindControl("DateDetails") as ASPxTextBox;
                        var DayPlus = int.Parse(GetDateNext.Text);
                        DateTime ValueNext = today.AddDays(DayPlus);

                        //var ValText_VerDate = iList[0].LastVerificationDate.ToString();
                        var ValCalNumber = "1";
                        var ValTollSetupId = ValueId.ToString();
                        var ValTollInv = ValueInventory.ToString();
                        var ValText_NM = GetText_NM.ToString();
                        var ValText_Min = GetText_Min.ToString();
                        var ValText_Max = GetText_Max.ToString();
                        var ValText_Calibration1 = GetCollaboration1 == null ? "0" : GetCollaboration1.Text;
                        var ValText_Calibration2 = GetCollaboration2 == null ? "0" : GetCollaboration2.Text;
                        var ValText_Calibration3 = GetCollaboration3 == null ? "0" : GetCollaboration3.Text;
                        var ValText_Calibration4 = GetCollaboration4 == null ? "0" : GetCollaboration4.Text;
                        var ValText_Calibration5 = GetCollaboration5 == null ? "0" : GetCollaboration5.Text;
                        var ValMemo_Remarks = GetRemarks == null ? "" : GetRemarks.Text;
                        var NextVerDate = ValueNext == null ? "" : ValueNext.ToString();
                        User user = (User)Session["user"];
                        var CreatedBy = user.UserName;
                        ToolCalibrationRepository.SaveDataToolVerificationMark(InvString, ValCalNumber, ValTollSetupId, ValTollInv, ValText_NM, ValText_Min, ValText_Max, ValText_Calibration1, ValText_Calibration2, ValText_Calibration3, ValText_Calibration4, ValText_Calibration5, NextVerDate, ValMemo_Remarks, CreatedBy);

                    }

                }


            }
            Response.Write(@"
            <script>
            alert('Mark as Scrap Success');
            setTimeout(function(){            
            window.location = '" + Request.RawUrl + @"';
            }, 2000);
            </script>");
        }

        protected void SaveCalibration(object sender, EventArgs e)
        {
            var SumRowData = GridviewToolVerification.VisibleRowCount;
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Calibration Success !');", true);
            for (int i = 0; i < SumRowData; i++)
            {

                //GridviewToolVerification.GetCurrentPageRowValues("ToolSetupId")[i];


                var varGetData = GridviewToolVerification.FindDetailRowTemplateControl(i, "LayoutDetails") as ASPxFormLayout;

                if (varGetData != null)
                {
                    var ValueInventory = GridviewToolVerification.GetRowValues(i, "Id");
                    var InvString = ValueInventory.ToString();
                    var ValueId = GridviewToolVerification.GetRowValues(i, "ToolId");
                    var ValAttempt = lbl_Attempt.Text == "0" ? 1 : int.Parse(lbl_Attempt.Text) + 1;
                    var GetVerDate = varGetData.FindControl("lblVerificationDate") as ASPxLabel;
                    var GetToolSetupId = varGetData.FindControl("lblInventoryNumber") as ASPxLabel;
                    var MinNMGet = GridviewToolVerification.GetRowValues(i, "MinNM");
                    var MaxNMGet = GridviewToolVerification.GetRowValues(i, "MaxNM");
                    var ValSetNm = GridviewToolVerification.GetRowValues(i, "SetNM");
                    for (int u = 0; u < ValAttempt; u++)
                    {

                        var GetCollaboration1 = varGetData.FindControl("Text_VerValue_" + ValAttempt + "_" + "1") as ASPxTextBox;
                        var GetCollaboration2 = varGetData.FindControl("Text_VerValue_" + ValAttempt + "_" + "2") as ASPxTextBox;
                        var GetCollaboration3 = varGetData.FindControl("Text_VerValue_" + ValAttempt + "_" + "3") as ASPxTextBox;
                        var GetCollaboration4 = varGetData.FindControl("Text_VerValue_" + ValAttempt + "_" + "4") as ASPxTextBox;
                        var GetCollaboration5 = varGetData.FindControl("Text_VerValue_" + ValAttempt + "_" + "5") as ASPxTextBox;
                        var GetRemarks = varGetData.FindControl("RemarksDetail") as ASPxMemo;
                        var GetCallDate = DateTime.Now;
                        var GetDateNext = varGetData.FindControl("DateDetails") as ASPxTextBox;
                        DateTime today = DateTime.Now;
                        var DayPlus = int.Parse(GetDateNext.Text);
                        DateTime ValueNext = today.AddDays(DayPlus);
                        // var ValText_VerDate = iList[0].LastVerificationDate.ToString();
                        var ValCalNumber = "1";
                        var ValTollSetupId = ValueId.ToString();
                        var ValTollInv = ValueInventory.ToString();
                        var ValText_NM = ValSetNm.ToString();
                        var ValText_Min = MinNMGet.ToString();
                        var ValText_Max = MaxNMGet.ToString();
                        var ValText_Calibration1 = GetCollaboration1 == null ? "0" : GetCollaboration1.Text;
                        var ValText_Calibration2 = GetCollaboration2 == null ? "0" : GetCollaboration2.Text;
                        var ValText_Calibration3 = GetCollaboration3 == null ? "0" : GetCollaboration3.Text;
                        var ValText_Calibration4 = GetCollaboration4 == null ? "0" : GetCollaboration4.Text;
                        var ValText_Calibration5 = GetCollaboration5 == null ? "0" : GetCollaboration5.Text;
                        var ValMemo_Remarks = GetRemarks.Text;
                        var NextVerDate = ValueNext.ToString();
                        User user = (User)Session["user"];
                        var CreatedBy = user.UserName;
                        ToolCalibrationRepository.SaveDataToolCalibration(InvString, ValCalNumber, ValTollSetupId, ValTollInv, ValText_NM, ValText_Min, ValText_Max, ValText_Calibration1, ValText_Calibration2, ValText_Calibration3, ValText_Calibration4, ValText_Calibration5, NextVerDate, ValMemo_Remarks, CreatedBy);
                        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Mark As Scrap successfully !');", true);
                    }
                }
            }
            Response.Write(@"
                <script>
                alert('Record successful.');
                setTimeout(function(){            
                window.location = '" + Request.RawUrl + @"';   
                }, 2000);
                </script>");
            //Response.Redirect(Request.RawUrl);
        }

        protected void GridviewToolVerification_DataBinding(object sender, EventArgs e)
        {
            GridviewToolVerification.DataSource = Session["dtCalibration"];
        }

        protected void ExportExcel_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });

        }

        protected void btnToolBarcode_Click(object sender, EventArgs e)
        {
            DateTime ValueNext = DateTime.Now;
            try
            {
                var SumRowData = GridviewToolVerification.VisibleRowCount;
                for (int i = 0; i < SumRowData; i++)
                {

                    var varGetData = GridviewToolVerification.FindDetailRowTemplateControl(i, "LayoutDetails") as ASPxFormLayout;

                    if (varGetData != null)
                    {
                        Session["SetNM"] = GridviewToolVerification.GetRowValues(i, "SetNM");
                        DateTime today = DateTime.Now;

                        var ValAttempt = lbl_Attempt.Text == "0" ? 1 : int.Parse(lbl_Attempt.Text) + 1;

                        if (ValAttempt > 3)
                        {
                            ValAttempt = 3;
                        }
                        for (int u = 0; u < ValAttempt; u++)
                        {

                            var GetCallDate = DateTime.Now;
                            var GetDateNext = varGetData.FindControl("DateDetails") as ASPxTextBox;
                            var DayPlus = int.Parse(GetDateNext.Text);
                            ValueNext = today.AddDays(DayPlus);


                        }
                        var BtnSave = varGetData.FindControl("BtnSave") as ASPxButton;
                    }


                }
                int cpId = int.Parse(Session["VisibleIndex"].ToString());
                string NextVerificationDate = ValueNext.ToString("yyyyMMdd");
                string DateNow = DateTime.Now.ToString("yyyyMMdd");
                User user = (User)Session["user"];
                //int SetNm = int.Parse(Session["SetNM"].ToString());
                string CreatedBy = user.UserName;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
                  "window.open('/custom/Report/ToolCalibrationBarcode.aspx?inventoryId=" + cpId + "&NextVerificationDate=" + NextVerificationDate + "&User=" + CreatedBy + "','_newtab');",
                  true);

                // Response.Redirect("/custom/Report/ToolCalibrationBarcode.aspx?inventoryId=" + cpId + "&NextVerificationDate=" + NextVerificationDate +"&User="+CreatedBy);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void GridviewToolVerification_HtmlDataRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grv = (sender as ASPxGridView);
            if (grv.GetRowValues(e.VisibleIndex, "NextCalibrationDate") != null)
            {
                if (grv.GetRowValues(e.VisibleIndex, "NextCalibrationDate").ToString() != "")
                {
                    DateTime DateData = Convert.ToDateTime(grv.GetRowValues(e.VisibleIndex, "NextCalibrationDate"));
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