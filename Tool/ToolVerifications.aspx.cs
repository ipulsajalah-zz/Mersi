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
using DevExpress.XtraCharts;

namespace Ebiz.WebForm.custom.Tool
{
    public partial class ToolVerifications : ListPageBase
    {
        //Default table name
        protected static string tableName = "ToolVerificationsToday";

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
                select u.VerifiedDate, u.Verification#, u.Value
                from 
                (
	                select TOP 10 ver.VerifiedDate, ver.Verification1, ver.Verification2, ver.Verification3 
	                from [dbo].[ToolVerifications] ver
	                where ver.ToolInventoryId = @ToolInventoryId
	                order by ver.VerifiedDate DESC
                ) v
                unpivot
                (
                  Value
                  for Verification# in (Verification1, Verification2, Verification3)
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

            #region MasterGrid

            //enable row tracking
            MasterGrid.SettingsBehavior.EnableRowHotTrack = true;

            MasterGrid.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Row;
            MasterGrid.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
            MasterGrid.SettingsEditing.BatchEditSettings.AllowValidationOnEndEdit = false;

            //int idx = 0;

            ////create band column (if not exist)
            //GridViewColumn dtCol = MasterGrid.Columns["Verifications"];
            ////int idx = MasterGrid.Columns.IndexOf(dtCol);
            //if (dtCol == null)
            //{
            //    GridViewBandColumn col = new GridViewBandColumn("Verifications");
            //    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            //    dtCol = masterGrid.Columns["Verification1"];
            //    idx = MasterGrid.Columns.IndexOf(dtCol);
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["Verification2"];
            //    if (dtCol != null)
            //    {
            //        dtCol.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2f2f2");
            //        dtCol.HeaderStyle.ForeColor = Color.Blue;
            //        dtCol.HeaderStyle.Font.Bold = true;

            //        MasterGrid.Columns.Remove(dtCol);
            //        col.Columns.Add(dtCol);
            //    }

            //    dtCol = MasterGrid.Columns["Verification3"];
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
            //    dtCol.CellStyle.BorderRight.BorderWidth = new Unit("1px");

            //    col.CellStyle.BorderLeft.BorderWidth = new Unit("1px");
            //    col.CellStyle.BorderRight.BorderWidth = new Unit("1px");
            //    col.VisibleIndex = idx;
            //    MasterGrid.Columns.Insert(idx, col);
            
            //}

            #endregion

            
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

            //load verification history
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

        protected void popupVerification_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            string key = e.Parameter;

            //get tool information

            //load calibration history
        }

        protected void popupVerification_PopupWindowCommand(object source, PopupControlCommandEventArgs e)
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

        protected void chartHistory_CustomCallback(object sender, DevExpress.XtraCharts.Web.CustomCallbackEventArgs e)
        {
            //if (e.Parameter == "MarkerKind")
            //    PerformMarkerKindAction();
            //else if (e.Parameter == "MarkerSize")
            //    PerformMarkerSizeAction();
            //else if (e.Parameter == "ShowLabels")
            //    PerformShowLabelsAction();
        }

    }
}