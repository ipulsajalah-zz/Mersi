using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DotWeb;
using DotWeb.Repositories;
using DotWeb.Models;
using DotWeb.Utils;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Configuration;
using System.Globalization;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;

namespace DotMercy.custom
{
    public partial class VehicleDeliveryOrder_SEND : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        protected User currentUser;
        DataTable dt = new DataTable();

        class ReadOnlyTemplate : ITemplate
        {
            public void InstantiateIn(Control _container)
            {
                GridViewEditItemTemplateContainer container = _container as GridViewEditItemTemplateContainer;

                ASPxLabel lbl = new ASPxLabel();
                lbl.ID = "lbl";

                container.Controls.Add(lbl);
                lbl.Text = string.IsNullOrEmpty(container.Text) ? "_" : container.Text;
            }
        }
        protected void GetYear()
        {
            cmbYear.DataSource = VehicleOrderRepository.Year();
            cmbYear.TextField = "Y";
            cmbYear.ValueField = "nom";
            cmbYear.Value = "-Select-";
            cmbYear.DataBind();
        }
        protected void GetMonth()
        {
            cmbMonth.DataSource = VehicleOrderRepository.Month();
            cmbMonth.TextField = "MonthName";
            cmbMonth.ValueField = "MonthNumber";
            cmbMonth.Value = "-Select-";
            cmbMonth.DataBind();
        }
        protected void GetDay(DataTable dt)
        {
            cmbDay.DataSource = dt;// Day(System.DateTime.Now);
            cmbDay.TextField = "Day";
            cmbDay.ValueField = "Id";
            cmbDay.Value = "-Select-";
            cmbDay.DataBind();
        }
        protected ControlPlanProcess UpdateItem(OrderedDictionary keys, OrderedDictionary newValues)
        {
            AppDb context = new AppDb();

            var id = Convert.ToInt32(keys["Id"]);
            var item = context.ControlPlanProcesses.First(i => i.Id == id);

            VehicleOrderRepository.LoadNewValues(item, newValues);
            context.SaveChanges();
            return item;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblGetAssemblyTypeId.Value = Session["assemblyType"].ToInt32(0);
            int assemblyTypeId = Session["assemblyType"].ToInt32(0);
            string month = DateTime.Today.Month.ToString();
            if (month.Count() == 1)
                month = "0" + month;

            string earlymonth = DateTime.Today.Year.ToString() + "-" + month + "-01";
            if (!IsCallback && !IsPostBack)
            {
                int start = grid.PageIndex * grid.SettingsPager.PageSize;
                int end = (grid.PageIndex + 1) * grid.SettingsPager.PageSize;
                GridViewDataColumn column1 = grid.Columns["CategoryName"] as GridViewDataColumn;
                GridViewDataColumn column2 = grid.Columns["Description"] as GridViewDataColumn;
                for (int i = start; i < end; i++)
                {
                    ASPxTextBox txtBox1 = (ASPxTextBox)grid.FindRowCellTemplateControl(i, column1, "txtBox");
                    ASPxTextBox txtBox2 = (ASPxTextBox)grid.FindRowCellTemplateControl(i, column2, "txtBox");
                    if (txtBox1 == null || txtBox2 == null)
                        continue;
                    int id = Convert.ToInt32(grid.GetRowValues(i, grid.KeyFieldName));
                }

                GetYear();
                GetMonth();

                Session["OriSqlCommand"] = SqlDataSource1.SelectCommand;
                BindGrid(" Where A.ProdOutDate BETWEEN '" + earlymonth + "' and EOMONTH('" +
                    earlymonth + "') and A.AssemblyTypeId = " + assemblyTypeId + " and A.isCKD = 1 order by ProdOutDate asc");
            }
            else
            {
                if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text == "-Select-" && cmbMonth.Text != "-Select-" && lblGetAssemblyTypeId.Text != null) // search from month
                {
                    DataSet ds = VehicleOrderRepository.GetVPCStorageByMonth(Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                    grid.DataSource = null;
                    grid.DataSourceID = null;
                    grid.DataSource = ds.Tables[0];
                    Session["GridData"] = ds.Tables[0];
                    grid.DataBind();
                }
                else if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text != "-Select-" && cmbMonth.Text != "-Select-" && lblGetAssemblyTypeId.Text != null) // search from month and year
                {
                    DataSet ds = VehicleOrderRepository.GetVPCStorageByMonthYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                    grid.DataSource = null;
                    grid.DataSourceID = null;
                    grid.DataSource = ds.Tables[0];
                    Session["GridData"] = ds.Tables[0];
                    grid.DataBind();
                }
                else if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text != "-Select-" && cmbMonth.Text == "-Select-" && lblGetAssemblyTypeId.Text != null) // search from year
                {
                    DataSet ds = VehicleOrderRepository.GetVPCStorageByYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(lblGetAssemblyTypeId.Value));

                    grid.DataSource = null;
                    grid.DataSourceID = null;
                    grid.DataSource = ds.Tables[0];
                    Session["GridData"] = ds.Tables[0];
                    grid.DataBind();
                }
                else if (!string.IsNullOrEmpty(cmbDay.Text) || cmbDay.Text != "-Select-" && lblGetAssemblyTypeId.Text != null)   // search from date
                {
                    if (cmbDay.Text == "")
                    {
                        DataSet ds = VehicleOrderRepository.GetVPCStorageForLoadGrid(Convert.ToInt32(lblGetAssemblyTypeId.Value));
                        grid.DataSource = null;
                        grid.DataSourceID = null;
                        grid.DataSource = ds.Tables[0];
                        Session["GridData"] = ds.Tables[0];
                        grid.DataBind();
                    }
                    else
                    {
                        if (cmbYear.Text == "-Select-" && lblGetAssemblyTypeId.Text != null)
                        {

                            DataSet ds = VehicleOrderRepository.GetVPCStorageByDay(cmbDay.Text, Convert.ToInt32(lblGetAssemblyTypeId.Value));
                            grid.DataSource = null;
                            grid.DataSourceID = null;
                            grid.DataSource = ds.Tables[0];
                            Session["GridData"] = ds.Tables[0];
                            grid.DataBind();

                        }
                        else
                        {
                            DataSet ds = VehicleOrderRepository.GetVPCStorageByDayYear(Convert.ToInt32(cmbYear.Text), cmbDay.Text, Convert.ToInt32(lblGetAssemblyTypeId.Value));
                            grid.DataSource = null;
                            grid.DataSourceID = null;
                            grid.DataSource = ds.Tables[0];
                            Session["GridData"] = ds.Tables[0];
                            grid.DataBind();

                        }

                    }
                }
                else
                {
                    Session["OriSqlCommand"] = SqlDataSource1.SelectCommand;
                    BindGrid(" Where A.ProdOutDate BETWEEN '" + earlymonth + "' and EOMONTH('" +
                        earlymonth + "') and A.AssemblyTypeId = " + assemblyTypeId + " and A.isCKD = 1 order by ProdOutDate asc");
                }

            }

        }

        private void BindGrid(string condition)
        {
            string sql = Session["OriSqlCommand"].ToString();
            sql = sql + condition;
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            Session["GridData"] = SqlDataSource1;

            grid.DataSourceID = null;
            grid.DataSource = Session["GridData"];
            grid.DataBind();
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //    if (grid.IsEditing)
            //    {
            //        if (e.Column.FieldName == "TrolleyId")
            //        {

            //        }
            //    }
        }
        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                e.NewValues["original_Id"] = e.NewValues["Id"];
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        protected void grid_Init(object sender, EventArgs e)
        {

        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "post")
            {

            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            foreach (var args in e.UpdateValues)
                UpdateItem(args.Keys, args.NewValues);

            e.Handled = true;
        }
        protected void grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            ASPxGridView detailGrid = ((ASPxGridView)sender);
            string FinNo = Convert.ToString(grid.GetRowValues(e.VisibleIndex, "VehicleNumber"));

            string queryStringStd = "?FINNumber=" + FinNo;

            if (e.ButtonID == "btnPrintNIK")
            {
                ASPxGridView.RedirectOnCallback("~/custom/Report/ReportProduction.aspx?" + queryStringStd);
            }
        }
        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                ASPxGridView grv = (sender as ASPxGridView);

                //ASPxLabel hplStation =
                //    (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblPrintNIK");

                //ASPxButton btnPrintNIK =
                //    (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnPrintNIK");

                int countIA = 10;

                if (countIA == 0)
                {
                    //hplStation.Visible = false;                    
                    ////btnPrintNIK.Visible = false;
                    //btnIrreg.Visible = false;
                }
                else
                {
                    //hplStation.Visible = true;
                    //btnPrintNIK.Visible = true;
                    //btnIrreg.Visible = true;
                }

            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                ASPxGridView grv = (sender as ASPxGridView);
                if (e.VisibleIndex > -1)
                {
                    if (e.DataColumn.FieldName == "SendToMarketing")
                    {
                        DataRowView row = grv.GetRow(e.VisibleIndex) as DataRowView;
                        string bpk = row["BPK"].ToString();

                        ASPxButton btnSend = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnSend") as ASPxButton;
                        ASPxLabel lblSend = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblSend") as ASPxLabel;

                        string caption = e.CellValue.ToString();// Convert.ToString(e.CellValue);
                        if (caption == string.Empty && !string.IsNullOrEmpty(bpk))
                        {
                            btnSend.Visible = true;
                            lblSend.Visible = false;
                        }
                        else
                        {
                            lblSend.Text = caption;
                            btnSend.Visible = false;
                            lblSend.Visible = true;
                        }
                    }

                    if (e.DataColumn.FieldName == "PrintNik")
                    {
                        ASPxButton btnPrintNik = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnPrintNIK") as ASPxButton;
                        ASPxLabel lblPrintNik = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblPrintNIK") as ASPxLabel;
                        string caption = e.CellValue.ToString("yyyy-MM-dd");// Convert.ToString(e.CellValue);
                        if (caption != string.Empty)
                        {
                            lblPrintNik.Text = caption;
                            btnPrintNik.Visible = false;
                            lblPrintNik.Visible = true;
                        }
                        else
                        {
                            btnPrintNik.Visible = false;
                            lblPrintNik.Visible = false;
                        }
                    }

                    if (e.DataColumn.FieldName == "ReceivedByMarketing")
                    {
                        ASPxButton btnRCV = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnRCV") as ASPxButton;
                        ASPxLabel lblRCV = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblRCV") as ASPxLabel;

                        string caption = e.CellValue.ToString();// Convert.ToString(e.CellValue);
                        if (caption != string.Empty)
                        {
                            lblRCV.Text = caption;
                            btnRCV.Visible = false;
                            lblRCV.Visible = true;
                        }
                        else
                        {
                            btnRCV.Visible = false;
                            lblRCV.Visible = false;
                        }
                    }

                    if (e.DataColumn.FieldName == "BPK")
                    {
                        ASPxButton btnBPK = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnBPK") as ASPxButton;
                        ASPxLabel lblBPK = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblBPK") as ASPxLabel;

                        string caption = e.CellValue.ToString();// Convert.ToString(e.CellValue);
                        if (caption != string.Empty)
                        {
                            lblBPK.Text = caption;
                            btnBPK.Visible = false;
                            lblBPK.Visible = true;
                        }
                        else
                        {
                            btnBPK.Visible = false;
                            lblBPK.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        protected void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbYear.Text != "-Select-" && lblGetAssemblyTypeId.Text != null)
            {
                DataTable dtable = new DataTable();
                dtable = VehicleOrderRepository.GetDay(Convert.ToInt32(cmbMonth.Value), cmbYear.Text);
                GetDay(dtable); // bind day

                DataSet ds = VehicleOrderRepository.GetVPCStorageByMonthYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else
            {
                DataTable dtable = new DataTable();
                dtable = VehicleOrderRepository.GetDay(Convert.ToInt32(cmbMonth.Value), cmbYear.Text);
                GetDay(dtable); // bind day

                DataSet ds = VehicleOrderRepository.GetVPCStorageByMonth(Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }

            cmbDay.Enabled = true;
        }
        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                //int id = Convert.ToInt32(btn.CommandArgument);
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                //int id = int.Parse(grid.GetRowValues(Rowindex, "FINNumber").ToString());
                int i = int.Parse(grid.GetRowValues(Rowindex, "Id").ToString());

                if (i != 0 || i != null)
                {
                    string query = @"UPDATE VPCStorage SET SendToMarketing = GETDATE() WHERE Id=" + Convert.ToString(i);

                    AppDb context = new AppDb();
                    DataSet dsData = new DataSet();

                    using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = query;
                            cmd.CommandTimeout = 7000;
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    string user = ((User)Session["user"]).UserName;
                    SqlDataSource1.UpdateCommand = "UPDATE VPCStorage SET SendToMarketingBy ='" + user + "' WHERE Id =" + i;
                    SqlDataSource1.Update();
                    LoadData();
                    //Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        protected void btnRCV_OnClick(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                int i = Convert.ToInt32(btn.CommandArgument);

                if (i != 0 || i != null)
                {
                    string query = @"UPDATE VPCStorage SET ReceivedByMarketing = GETDATE() WHERE Id=" + Convert.ToString(i);

                    AppDb context = new AppDb();
                    DataSet dsData = new DataSet();
                    using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = query;
                            cmd.CommandTimeout = 7000;
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadData();
                   // Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        protected void btnPrintNIK_OnClick(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                //int id = Convert.ToInt32(btn.CommandArgument);
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                //int id = int.Parse(grid.GetRowValues(Rowindex, "FINNumber").ToString());
                int id = int.Parse(grid.GetRowValues(Rowindex, "Id").ToString());

                if (id != 0 || id != null)
                {
                    string query = @"UPDATE VPCStorage SET PrintNIK = GETDATE() WHERE Id=" + Convert.ToString(id);

                    AppDb context = new AppDb();
                    DataSet dsData = new DataSet();
                    using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = query;
                            cmd.CommandTimeout = 7000;
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    string nikNumber = VehicleOrderRepository.GetRunningNumber();
                    string user = ((User)Session["user"]).UserName;
                    SqlDataSource1.UpdateCommand = "UPDATE VPCStorage SET PrintNIKBy ='" + user + "', NIKNumber='" + nikNumber + "' WHERE Id =" + id;
                    SqlDataSource1.Update();

                    string FinNo = VehicleOrderRepository.GetFINNo(id);
                    string queryStringStd = "?FINNumber=" + FinNo;

                    Page.Response.Redirect("~/custom/Report/ReportNIK.aspx" + queryStringStd);
                    LoadData();
                    //Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }
        protected void btnBPK_OnClick(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                int id = Convert.ToInt32(btn.CommandArgument);

                if (id != 0 || id != null)
                {
                    string query = @"UPDATE VPCStorage SET BPK = GETDATE() WHERE Id=" + Convert.ToString(id);

                    AppDb context = new AppDb();
                    DataSet dsData = new DataSet();
                    using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = query;
                            cmd.CommandTimeout = 7000;
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadData();
                    //Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            //if (grid.Selection.Count == 0)
            //{
            //exportGrid.ExportedRowType = GridViewExportedRowType.Selected;
            //}
            grid.DataSource = Session["GridData"];
            grid.DataBind();
            exportGrid.FileName = "VehicleDeliveryOrder_" + DateTime.Now.ToShortDateString();
            exportGrid.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
            //exportGrid.WriteXlsxToResponse("VehicleDeliveryOrder_" + DateTime.Now.ToShortDateString());

        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text == "-Select-" && cmbMonth.Text != "-Select-" && lblGetAssemblyTypeId.Text != null) // search from month
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageByMonth(Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text != "-Select-" && cmbMonth.Text != "-Select-" && lblGetAssemblyTypeId.Text != null) // search from month and year
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageByMonthYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text != "-Select-" && cmbMonth.Text == "-Select-" && lblGetAssemblyTypeId.Text != null) // search from year
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageByYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(lblGetAssemblyTypeId.Value));

                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else if (!string.IsNullOrEmpty(cmbDay.Text) || cmbDay.Text != "-Select-" && lblGetAssemblyTypeId.Text != null)   // search from date
            {
                if (cmbDay.Text == "")
                {
                    grid.DataSourceID = null;
                    grid.DataSource = SqlDataSource1;
                    Session["GridData"] = SqlDataSource1;
                    grid.DataBind();
                }
                else
                {
                    if (cmbYear.Text == "-Select-" && lblGetAssemblyTypeId.Text != null)
                    {

                        DataSet ds = VehicleOrderRepository.GetVPCStorageByDay(cmbDay.Text, Convert.ToInt32(lblGetAssemblyTypeId.Value));
                        grid.DataSource = null;
                        grid.DataSourceID = null;
                        grid.DataSource = ds.Tables[0];
                        Session["GridData"] = ds.Tables[0];
                        grid.DataBind();

                    }
                    else
                    {
                        DataSet ds = VehicleOrderRepository.GetVPCStorageByDayYear(Convert.ToInt32(cmbYear.Text), cmbDay.Text, Convert.ToInt32(lblGetAssemblyTypeId.Value));
                        grid.DataSource = null;
                        grid.DataSourceID = null;
                        grid.DataSource = ds.Tables[0];
                        Session["GridData"] = ds.Tables[0];
                        grid.DataBind();

                    }

                }
            }
            else
            {
                grid.DataSourceID = null;
                grid.DataSource = SqlDataSource1;
                Session["GridData"] = SqlDataSource1;
                grid.DataBind();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            cmbDay.Text = "";
         

            if (lblGetAssemblyTypeId != null)
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageForLoadGrid(Convert.ToInt32(lblGetAssemblyTypeId.Value));

                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();

            }            
        }
        private void LoadData()
        {
            if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text == "-Select-" && cmbMonth.Text != "-Select-" && lblGetAssemblyTypeId.Text != null) // search from month
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageByMonth(Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text != "-Select-" && cmbMonth.Text != "-Select-" && lblGetAssemblyTypeId.Text != null) // search from month and year
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageByMonthYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(cmbMonth.Value), Convert.ToInt32(lblGetAssemblyTypeId.Value));
                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else if ((cmbDay.Text == "" || cmbDay.Text == "-Select-") && cmbYear.Text != "-Select-" && cmbMonth.Text == "-Select-" && lblGetAssemblyTypeId.Text != null) // search from year
            {
                DataSet ds = VehicleOrderRepository.GetVPCStorageByYear(Convert.ToInt32(cmbYear.Text), Convert.ToInt32(lblGetAssemblyTypeId.Value));

                grid.DataSource = null;
                grid.DataSourceID = null;
                grid.DataSource = ds.Tables[0];
                Session["GridData"] = ds.Tables[0];
                grid.DataBind();
            }
            else if (!string.IsNullOrEmpty(cmbDay.Text) || cmbDay.Text != "-Select-" && lblGetAssemblyTypeId.Text != null)   // search from date
            {
                if (cmbDay.Text == "")
                {
                    DataSet ds = VehicleOrderRepository.GetVPCStorageForLoadGrid(Convert.ToInt32(lblGetAssemblyTypeId.Value));
                    grid.DataSource = null;
                    grid.DataSourceID = null;
                    grid.DataSource = ds.Tables[0];
                    Session["GridData"] = ds.Tables[0];
                    grid.DataBind();
                }
                else
                {
                    if (cmbYear.Text == "-Select-" && lblGetAssemblyTypeId.Text != null)
                    {

                        DataSet ds = VehicleOrderRepository.GetVPCStorageByDay(cmbDay.Text, Convert.ToInt32(lblGetAssemblyTypeId.Value));
                        grid.DataSource = null;
                        grid.DataSourceID = null;
                        grid.DataSource = ds.Tables[0];
                        Session["GridData"] = ds.Tables[0];
                        grid.DataBind();

                    }
                    else
                    {
                        DataSet ds = VehicleOrderRepository.GetVPCStorageByDayYear(Convert.ToInt32(cmbYear.Text), cmbDay.Text, Convert.ToInt32(lblGetAssemblyTypeId.Value));
                        grid.DataSource = null;
                        grid.DataSourceID = null;
                        grid.DataSource = ds.Tables[0];
                        Session["GridData"] = ds.Tables[0];
                        grid.DataBind();

                    }
                }
            }
            else
            {
                grid.DataSourceID = null;
                grid.DataSource = SqlDataSource1;
                Session["GridData"] = SqlDataSource1;
                grid.DataBind();
            }

            if (Session["cmbMonth"] != null)
                cmbMonth.Value = Session["cmbMonth"].ToString();

            if (Session["cmbYear"] != null)
                cmbYear.Text = Session["cmbYear"].ToString();

            if (Session["cmbDay"] != null)
                cmbDay.Text = Session["cmbDay"].ToString();
        }

      
    }
}