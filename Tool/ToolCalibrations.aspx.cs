using DevExpress.Web;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.Tool
{
    public partial class ToolCalibrations : ListPageBase
    {
        //Default table name
        protected static string tableName = "ToolCalibrationsToday";

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

            //tableMeta.CustomPopupInsert = "popupCalibration";
            //tableMeta.CustomPopupEdit = "popupCalibration";

            Session["TableName"] = tableName;

            SqlDataSource ds = new SqlDataSource();
            ds.SelectCommand = @"
                select u.CalibratedDate, u.Calibration#, u.Value
                from 
                (
	                select TOP 10 ver.CalibratedDate, ver.Calibration1, ver.Calibration2, ver.Calibration3, ver.Calibration4, ver.Calibration5  
	                from [dbo].[ToolCalibrations] ver
	                where ver.ToolInventoryId = @ToolInventoryId
	                order by ver.CalibratedDate DESC
                ) v
                unpivot
                (
                  Value
                  for Calibration# in (Calibration1, Calibration2, Calibration3, Calibration4, Calibration5)
                ) u;                   
            ";
            ds.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            ds.SelectCommandType = SqlDataSourceCommandType.Text;
            ds.SelectParameters.Add("ToolInventoryId", DbType.Int32, "3");

            chartHistory.DataSourceID = null;
            chartHistory.DataSource = ds;

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (MasterGrid == null)
                return true;
            
            //enable row tracking
            MasterGrid.SettingsBehavior.EnableRowHotTrack = true;

            MasterGrid.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Row;
            MasterGrid.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
            MasterGrid.SettingsEditing.BatchEditSettings.AllowValidationOnEndEdit = false;

            //int idx = 0;

            //if (MasterGrid.Columns.Count == 0)
            //    return true;

            ////create band column (if not exist)
            //GridViewColumn dtCol = MasterGrid.Columns["Calibrations"];
            ////int idx = MasterGrid.Columns.IndexOf(dtCol);
            //if (dtCol == null)
            //{
            //    GridViewBandColumn col = new GridViewBandColumn("Calibrations");
            //    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            //    dtCol = masterGrid.Columns["Calibration1"];
            //    idx = MasterGrid.Columns.IndexOf(dtCol);
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["Calibration2"];
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["Calibration3"];
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["Calibration4"];
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["Calibration5"];
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["SetNM"];
            //    if (dtCol != null)
            //    {
            //        dtCol.CellStyle.ForeColor = Color.Blue;
            //        dtCol.CellStyle.Font.Bold = true;
            //    }

            //    dtCol = MasterGrid.Columns["MinNM"];
            //    if (dtCol != null)
            //    {
            //        dtCol.CellStyle.ForeColor = Color.Red;
            //        dtCol.CellStyle.Font.Bold = true;
            //    }

            //    dtCol = MasterGrid.Columns["MaxNM"];
            //    if (dtCol != null)
            //    {
            //        dtCol.CellStyle.ForeColor = Color.Red;
            //        dtCol.CellStyle.Font.Bold = true;
            //    }

            //    for (int i = 0; i < idx; i++)
            //    {
            //        dtCol = MasterGrid.Columns[i];
            //        dtCol.CellStyle.BorderLeft.BorderWidth = new Unit(0);
            //        dtCol.CellStyle.BorderRight.BorderWidth = new Unit(0);
            //    }

            //    if (dtCol != null)
            //        dtCol.CellStyle.BorderRight.BorderWidth = new Unit("1px");

            //    col.CellStyle.BorderLeft.BorderWidth = new Unit("1px");
            //    col.CellStyle.BorderRight.BorderWidth = new Unit("1px");
            //    col.VisibleIndex = idx;
            //    MasterGrid.Columns.Insert(idx, col);
            //}

            return true;
        }

        protected void popupHistory_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int key = 0;

            string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);
            if (arr.Length > 1)
                key = arr[1].ToInt32(0);

            if (key == 0)
                return;

            ////get tool information
            //ToolInventory ti = null;
            //using (AppDb ctx = new AppDb())
            //{
            //    ti = ctx.ToolInventories.Where(x => x.Id == key).FirstOrDefault();
            //}

            //if (ti == null)
            //    return;

            ////set chart title
            //if (chartHistory.Titles.Count > 0)
            //    chartHistory.Titles[0].Text = ti.Number;

            //load calibration history
            SqlDataSource ds = chartHistory.DataSource as SqlDataSource;
            if (ds != null)
            {
                ds.SelectParameters["ToolInventoryId"].DefaultValue = key.ToString();
                //ds.DataBind();
                chartHistory.DataSource = ds;
                chartHistory.DataBind();
            }
        }

        protected void popupHistory_PopupWindowCommand(object source, PopupControlCommandEventArgs e)
        {

        }

        protected void chartHistory_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //if (e.Parameter == "MarkerKind")
            //    PerformMarkerKindAction();
            //else if (e.Parameter == "MarkerSize")
            //    PerformMarkerSizeAction();
            //else if (e.Parameter == "ShowLabels")
            //    PerformShowLabelsAction();
        }

        protected void popupCalibration_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            string key = e.Parameter;

            //get tool information

            //load calibration history
        }

        protected void popupCalibration_PopupWindowCommand(object source, PopupControlCommandEventArgs e)
        {

        }

        //protected void popupGWIS_WindowCallback1(object source, PopupWindowCallbackArgs e)
        //{
        //    ///name;visibleindex;buttonId
        //    string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);

        //    ASPxGridView grid = this.masterGrid;
        //    int id = grid.GetRowValues(arr[1].ToInt32(-1), "Id").ToInt32(0);

        //    string imageId = RicRepository.GetImgId(id);

        //    popupGWIS.ContentUrl = "/custom/GWIS/DrawGWIS.aspx?ImageID=" + imageId;

        //    ////Session["ImageID"] = a;

        //    //if (a != null)
        //    //{
        //    ////GWISPicture gwisPict = RicRepository.GetGwisPictId(ab);

        //    //    getEditImage(a);
        //    //    setRepeaterAnnot(a);

        //    //}
        //}

    }
}