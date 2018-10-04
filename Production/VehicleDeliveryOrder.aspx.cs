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
using Ebiz.SAP;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;

namespace DotMercy.custom
{
    public partial class VehicleDeliveryOrder : Ebiz.Scaffolding.WebForm.UI.CustomPage
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

                if (e.DataColumn.FieldName == "PrintNik")
                {
                    ASPxButton btnPrintNik = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnPrintNIK") as ASPxButton;
                    ASPxLabel lblPrintNik = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblPrintNIK") as ASPxLabel;

                    string caption = e.CellValue.ToString();// Convert.ToString(e.CellValue);
                    if (caption != string.Empty)
                    {
                        lblPrintNik.Text = caption;
                        btnPrintNik.Visible = false;
                        lblPrintNik.Visible = true;
                    }
                    else
                    {
                        btnPrintNik.Visible = true;
                        lblPrintNik.Visible = false;
                    }
                }

                if (e.DataColumn.FieldName == "SendToMarketing")
                {
                    ASPxButton btnSend = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "btnSend") as ASPxButton;
                    ASPxLabel lblSend = grv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "lblSend") as ASPxLabel;

                    string caption = e.CellValue.ToString();// Convert.ToString(e.CellValue);
                    if (caption != string.Empty)
                    {
                        lblSend.Text = caption;
                        btnSend.Visible = false;
                        lblSend.Visible = true;
                    }
                    else
                    {
                        btnSend.Visible = false;
                        lblSend.Visible = false;
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
                        btnBPK.Visible = true;
                        lblBPK.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
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
                ASPxButton btn = (sender as ASPxButton);
                int i = Convert.ToInt32(btn.CommandArgument);

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
                int i = btn.CommandArgument.ToInt32(0);

                if (i != 0)
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
                    //Response.Redirect(Request.RawUrl);
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
                ASPxButton btn = (sender as ASPxButton);
                //int id = Convert.ToInt32(btn.CommandArgument);
                LoadData();
                //ASPxButton btn = sender as ASPxButton;
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

                    NIK nik = VehicleOrderRepository.getNIKNumber();
                    string message = nik.Message;
                    string nikNumber = nik.NIKNumber;
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        //grid.JSProperties["cpalertmessage"] = message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + message + "');", true);
                    }
                    else
                    {
                        string user = ((User)Session["user"]).UserName;
                        SqlDataSource1.UpdateCommand = "UPDATE VPCStorage SET PrintNIKBy ='" + user + "', NIKNumber='" + nikNumber + "' WHERE Id =" + id;
                        SqlDataSource1.Update();

                        string FinNo = VehicleOrderRepository.GetFINNo(id);
                        string queryStringStd = "?FINNumber=" + FinNo;

                        Page.Response.Redirect("~/custom/Report/ReportNIK.aspx" + queryStringStd);
                    }
                    //Response.Redirect(Request.RawUrl);
                    LoadData();
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
                LoadData();

                //int id = Convert.ToInt32(btn.CommandArgument);
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                //int id = int.Parse(grid.GetRowValues(Rowindex, "FINNumber").ToString());
                string Id = grid.GetRowValues(Rowindex, "Id").ToString();
                if (Id != null)
                {
                    string user = ((User)Session["user"]).UserName;

                    //Update data date and user already confirm
                    string query = @"UPDATE VPCStorage SET BPK = GETDATE(), BPKBy ='" + user + "'  WHERE Id=" + Id;
                    using (SqlConnection conn = AppConnection.GetConnection())
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

                    // SAP
                    DataTable dt = GetOrderNoVPCStorage(Id.ToInt32(0));

                    //DataTable GrInfo
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        //dtBPK = Convert.ToDateTime(dr["BPK"]);
                        string tOrderNo = dr["OrderNo"].ToString();

                        DataTable GrInfoDt = new DataTable();
                        using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString))
                        using (var cmd = new SqlCommand("usp_GetGRInfoToSAPbyProdOrder", con))
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@OrderNo", tOrderNo));
                            da.Fill(GrInfoDt);
                        }

                        if (dr["BuyOutDate"] != DBNull.Value)
                        {
                            string tSAPMessage = "";
                            string tSAPMessageGR = "";
                            Ebiz.SAP.UserUtil SendToSAP = new Ebiz.SAP.UserUtil();

                            bool resultFromSAP = SendToSAP.ConfirmOrderPKW(SAPConfiguration.SAP_CONFIG_NAME, tOrderNo, "TIDS", "0010", out tSAPMessage);

                            System.Threading.Thread.Sleep(2000);
                            DataTable resultFromSAPGR = SendToSAP.GoodReceiptAutomatically(SAPConfiguration.SAP_CONFIG_NAME, tOrderNo, out tSAPMessageGR);

                            if (resultFromSAP == true && string.IsNullOrEmpty(tSAPMessageGR))
                            {
                                SendToSAP.UpdateVDC(SAPConfiguration.SAP_CONFIG_NAME, tOrderNo, GrInfoDt);
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Check SAP Order Number data successfully');", true);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(tSAPMessageGR))
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Error: " + tSAPMessageGR + "');", true);
                                else
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Check SAP Order Number:" + tSAPMessage + "');", true);
                            }
                        }

                        //string tSAPMessage = "";
                        //string tSAPMessageGR = "";
                        //Ebiz.SAP.UserUtil SendToSAP = new Ebiz.SAP.UserUtil();

                        //bool resultFromSAP = SendToSAP.ConfirmOrderPKW(SAPConfiguration.SAP_CONFIG_NAME, tOrderNo, "TIDS", "0010", out tSAPMessage);
                        //if (resultFromSAP == true)
                        //{
                        //    //Update data date and user already confirm
                        //    string query = @"UPDATE VPCStorage SET BPK = GETDATE() WHERE Id=" + Id;
                        //    using (SqlConnection conn = AppConnection.GetConnection())
                        //    {
                        //        if (conn.State == ConnectionState.Closed) conn.Open();

                        //        using (SqlCommand cmd = new SqlCommand())
                        //        {
                        //            cmd.CommandText = query;
                        //            cmd.CommandTimeout = 7000;
                        //            cmd.CommandType = CommandType.Text;
                        //            cmd.Connection = conn;
                        //            cmd.ExecuteNonQuery();
                        //        }
                        //    }

                        //    string user = ((User)Session["user"]).UserName;
                        //    SqlDataSource1.UpdateCommand = "UPDATE VPCStorage SET BPKBy ='" + user + "' WHERE Id =" + Id;
                        //    SqlDataSource1.Update();

                        //    if (dr["BuyOutDate"] == DBNull.Value)
                        //    {
                        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Confirm BPK is successful');", true);
                        //    }
                        //    else
                        //    {
                        //        System.Threading.Thread.Sleep(2000);
                        //        DataTable resultFromSAPGR = SendToSAP.GoodReceiptAutomatically(SAPConfiguration.SAP_CONFIG_NAME, tOrderNo, out tSAPMessageGR);

                        //        if (string.IsNullOrEmpty(tSAPMessageGR))
                        //        {
                        //            SendToSAP.UpdateVDC(SAPConfiguration.SAP_CONFIG_NAME, tOrderNo, GrInfoDt);

                        //            //Update data date and user already confirm
                        //            query = @"UPDATE VPCStorage SET GRDATE = GETDATE() WHERE Id=" + Id;
                        //            using (SqlConnection conn = AppConnection.GetConnection())
                        //            {
                        //                if (conn.State == ConnectionState.Closed) conn.Open();

                        //                using (SqlCommand cmd = new SqlCommand())
                        //                {
                        //                    cmd.CommandText = query;
                        //                    cmd.CommandTimeout = 7000;
                        //                    cmd.CommandType = CommandType.Text;
                        //                    cmd.Connection = conn;
                        //                    cmd.ExecuteNonQuery();
                        //                }
                        //            }
                        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Confirm BPK and SAP GR is successful');", true);

                        //        }
                        //        else
                        //        {
                        //            if (!string.IsNullOrEmpty(tSAPMessageGR))
                        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Error: " + tSAPMessageGR + "');", true);
                        //            else
                        //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('SAP GR failed:" + tSAPMessage + "');", true);
                        //        }
                        //    }   //if (dr["BuyOutDate"] != DBNull.Value)
                        //}   
                        //else
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Confirm BPK failed:" + tSAPMessage + "');", true);
                        //}
                    }

                    //Response.Redirect(Request.RawUrl);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
        }

    

        protected DataTable GetOrderNoVPCStorage(int tID)
        {
            AppDb context = new AppDb();

            string sql = @"SELECT  
	                        A.[CatalogueName], 
	                        A.[VINNumber], 
	                        A.[CommNos], 
	                        A.[EngineNo], 
	                        A.[GRDate], 
	                        A.[BuyOutDate], 
	                        A.[GiDate], 
	                        A.[Remark], 
	                        A.[PrintNik], 
	                        A.[SendToMarketing], 
	                        A.[ReceivedByMarketing], 
	                        A.[Id], 
	                        A.[BPK],
	                        B.OrderNo
                        FROM VPCStorage A
                        LEFT JOIN ProductionSequenceDetail B ON A.FINNumber = B.SerialNumber
                        WHERE A.Id=" + tID;

            DataTable dtData = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 7000;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtData);
                    }
                }
                return dtData;
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex);
            }
            return null;
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
        }

        protected void btnReprint_Click(object sender, EventArgs e)
        {
            string FinNo = VehicleOrderRepository.CheckFINNo(txtReprint.Text.Trim());
            if (FinNo != null)
            {
                string queryStringStd = "?FINNumber=" + FinNo;
                Page.Response.Redirect("~/custom/Report/ReportNIKReprint.aspx" + queryStringStd);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Production No : " + txtReprint.Text.Trim() + " never print before !')", true);
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
