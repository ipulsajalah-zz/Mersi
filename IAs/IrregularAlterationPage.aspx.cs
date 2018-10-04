using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using DevExpress.Data.Helpers;
using DevExpress.Data.Linq.Helpers;
//using DevExpress.Data.WcfLinq.Helpers;
using Ebiz.Scaffolding.Models;
using System.Configuration;
using System.Globalization;
using Ebiz.Scaffolding.Utils;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Tids.CV.Models;
using Ebiz.Scaffolding.WebForm.UI;
using DevExpress.Web;
using System.Drawing;
using Ebiz.Scaffolding.Generator;
using System.Web.UI;

namespace Ebiz.WebForm.custom.IAs
{


    public partial class IrregularAlterationPage : ListPageBase
    {
        protected static string tableName = "IrregularAlteration";
        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {


            string TextStatus = Request.QueryString["TextStatus"];
            int Month = Request.QueryString["Month"].ToInt32(0);
            int Email = Request.QueryString["Email"].ToInt32(0);
            if (TextStatus != null || Month != 0)
            {
                tableName = "IrregularAlteration";
            }
            else if (Email == 1)
            {
                tableName = "IAOrganizations";
            }
            else if (Email == 2)
            {
                tableName = "IACcEmail";
            }


            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();


            if (TextStatus != null)
            {
                if (TextStatus != "All")
                {
                    tableMeta.AdditionalFilters = string.Format("TextStatus = '{0}'", TextStatus);
                }
                else
                {
                    tableMeta.AdditionalFilters = "";
                }

                TableMeta tbl = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                tbl.DefaultGrouping = "PPD";
                tbl.ApplyColumnGroupingAndSorting();
                ColumnMeta filtercol = tbl.Columns.Where(x => x.Name == "Years").FirstOrDefault();
                filtercol.DisplayInGrid = false;
            }
            else if (Month != 0)
            {
                tableMeta.AdditionalFilters = string.Format(" '{0}'  between month(ValidPeriodFrom) and	month(isnull(ValidPeriodTo,12))  ", Month);


                TableMeta tbl = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                tbl.DefaultGrouping = "Years,PPD";
                tbl.ApplyColumnGroupingAndSorting();
                ColumnMeta filtercol = tbl.Columns.Where(x => x.Name == "PPD").FirstOrDefault();
                filtercol.DisplayInGrid = false;
            }
            else if (Email == 0 && Month == 0 && TextStatus == null)
            {
                tableMeta.AdditionalFilters = "";
                TableMeta tbl = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                tbl.DefaultGrouping = "PPD";
                tbl.ApplyColumnGroupingAndSorting();
                ColumnMeta filtercol = tbl.Columns.Where(x => x.Name == "Years").FirstOrDefault();
                filtercol.DisplayInGrid = false;
            }










            masterPage.MainContent.Controls.Add(new LiteralControl(
                 @"
        <div class='container' style='padding:0; margin:0'>
            <div class='row row-offcanvas row-offcanvas-left'>
                <div class='col-xs-6 col-sm-2 sidebar-offcanvas' id='sidebar' role='navigation'>
                <nav>
                    <ul class='nav'>
                        <li><a href='#' id='btn-1' data-toggle='collapse' data-target='#submenu1' aria-expanded='false'>(PPD) PA PRE-INFO</a>
                            <ul class='nav collapse in' id='submenu1' role='menu' aria-labelledby='btn-1'>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=All'>ALL INFO DOCUMENT</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=Close'>TASK STATUS CLOSE</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=Draft'>TASK STATUS DRAFT</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?TextStatus=Open'>TASK STATUS OPEN</a></li>
                            </ul>
                        </li>
                        <li><a href='#' id='btn-2' data-toggle='collapse' data-target='#submenu2' aria-expanded='false'>PACKING MONTH</a>
                            <ul class='nav collapse in' id='submenu2' role='menu' aria-labelledby='btn-2'>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=1'>JANUARY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=2'>FEBRUARY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=3'>MARCH</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=4'>APRIL</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=5'>MAY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=6'>JUNE</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=7'>JULY</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=8'>AGUSTUS</a></li>

                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=9'>SEPTEMBER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=10'>OCTOBER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=11'>NOVEMBER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Month=12'>DECEMBER</a></li>

                            </ul>
                        </li>
                        <li><a href='#' id='btn-3' data-toggle='collapse' data-target='#submenu3' aria-expanded='false'>USER PROFILE</a>
                            <ul class='nav collapse in' id='submenu3' role='menu' aria-labelledby='btn-3'>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Email=1'>EMAIL MANAGER</a></li>
                                <li class='lichild'><a href='/custom/IAs/IrregularAlterationPage.aspx?Email=2'>EMAIL CC DIRECTURE</a></li>
                            </ul>
                        </li>
                    </ul>
                </nav>
                </div>
                <div class='col-xs-12 col-sm-10'  style='padding-right:0; margin:0'>
                  
                     <div id='MainContainer' runat='server'>"
                ));

            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            //TODO: move to configuration
            //tableMeta.PreviewColumnName = "Torque";

            // header info
            int modelId = Request.QueryString["model"].ToInt32(0);
            string packingMonth = Request.QueryString["vpm"];

            //Set master key
            if (modelId > 0)
                SetMasterKey("ModelId", modelId);
            if (!string.IsNullOrEmpty(packingMonth))
                SetMasterKey("PackingMonth", packingMonth);

            ////create header
            //string modelName = RicRepository.GetModelName(modelId);

            Session["TableName"] = tableName;


            return true;
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            //var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            //TableMeta tbl = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            //tbl.DefaultGrouping = "Page";


            masterPage.MainContent.Controls.Add(new LiteralControl(string.Format(" </div></div>  </div></div>")));
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            //if (masterGrid != null)
            //{
            //}
            //var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            //TableMeta tbl = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            //tbl.DefaultGrouping = "Page";

            return true;
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    string packingMonth, searchpkm, year, yearAndPkm;
        //    int userId = ((User)Session["user"]).Id;

        //    List<Ebiz.Tids.CV.Models.IA> checkUserAssigneorDelegate = IrregAltRepository.checkUserAssignOrDelegate(userId);


        //    if (!IsPostBack)
        //    {
        //        Session["OriSqlCommand"] = SqlDataSourceIA.SelectCommand;
        //    }


        //    int Status = Request.QueryString["Status"].ToInt32(0);
        //    int Month = Request.QueryString["Month"].ToInt32(0);

        //    int Email = Request.QueryString["Email"].ToInt32(0);
        //    if (Email > 0)
        //    {
        //        gvIrregAltvw.Visible = false;
        //        if (Email == 1)
        //        {
        //            gvManager.Visible = true;
        //        }
        //        else if (Email == 2)
        //        {
        //            gvDelegate.Visible = true;
        //        }
        //        else
        //        {
        //            gvCcDirecture.Visible = true;
        //        }
        //    }
        //    else if (Month > 0)
        //    {
        //        gvIrregAltvw.Visible = true;
        //        gvManager.Visible = false;
        //        gvDelegate.Visible = false;
        //        gvCcDirecture.Visible = false;

        //        searchpkm = string.Format(" WHERE   '{0}'  between month(ia.ValidPeriodFrom) and	month(isnull(ia.ValidPeriodTo,12))  and ia.__IsDraft !=1 order by ia.Id desc", Month);
        //        BindGrid(searchpkm);
        //        gvIrregAltvw.GroupBy(gvIrregAltvw.Columns["Year"]);
        //        gvIrregAltvw.Columns["PPD"].Visible = false;
        //    }
        //    else
        //    {
        //        gvIrregAltvw.Visible = true;
        //        gvManager.Visible = false;
        //        gvDelegate.Visible = false;
        //        gvCcDirecture.Visible = false;
        //        if (Status > 0)
        //        {
        //            searchpkm = string.Format(" WHERE ia.StatusId = '{0}' and ia.__IsDraft !=1 order by ia.Id desc", Status);
        //            BindGrid(searchpkm);
        //            gvIrregAltvw.GroupBy(gvIrregAltvw.Columns["PPD"]);
        //            gvIrregAltvw.Columns["Year"].Visible = false;
        //        }
        //        else
        //        {
        //            searchpkm = string.Format(" WHERE ia.__IsDraft !=1 order by ia.Id desc", Status);
        //            BindGrid(searchpkm);
        //            gvIrregAltvw.GroupBy(gvIrregAltvw.Columns["PPD"]);
        //            gvIrregAltvw.Columns["Year"].Visible = false;
        //        }
        //    }
        //}



        private void BindGrid(string condition)
        {
            string sql = Session["OriSqlCommand"].ToString();
            sql = sql + condition;
            SqlDataSourceIA.SelectCommand = sql;
            SqlDataSourceIA.DataBind();
            gvIrregAltvw.DataBind();

        }

        //protected void gvIrregAltvw_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        //{W

        //}
        //protected void gvIrregAltvw_DataBound(object sender, EventArgs e)
        //{
        //    if (!permissions.Contains(EPermissionType.Update))
        //    {
        //        gvIrregAltvw.Columns[0].Visible = false;

        //    }
        //    if (!permissions.Contains(EPermissionType.Insert))
        //    {
        //        //btnAdd.visible = false;
        //        (gvIrregAltvw.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
        //    }
        //}
        protected void gvIrregAltvw_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //if (e.DataColumn.FieldName != "TextStatus") return;
            //    List<IA> dataStatus = IrregAltRepository.getStatusForListPageIA();

            // if (dataStatus.Count != 0)
            //{
            //    foreach (var item in dataStatus)
            //    {
            //        if (e.CellValue.ToString() == "Open")
            //        {
            //            Color tlr = System.Drawing.ColorTranslator.FromHtml("#ff0000");
            //            e.Cell.BackColor = tlr;
            //        }
            //        else if (e.CellValue.ToString() == "InProgress")
            //        {
            //            Color tlr = System.Drawing.ColorTranslator.FromHtml("#ffff00");
            //            e.Cell.BackColor = tlr;
            //        }
            //        else if (e.CellValue.ToString() == "Close")
            //        {
            //            Color tlr = System.Drawing.ColorTranslator.FromHtml("#3cb371");
            //            e.Cell.BackColor = tlr;
            //        }
            //    }
            //}

            if (e.DataColumn.FieldName != "InternalEpcNumber" && e.DataColumn.FieldName != "TextStatus") return;

            int userId = ((User)Session["user"]).Id;
            List<IA> data = IrregAltRepository.getInternalEPCNumber(userId);
            List<IA> dataApprover = IrregAltRepository.getTaskCloseToApprover(userId);
            List<IA> dataStatus = IrregAltRepository.getStatusForListPageIA();

            if (e.DataColumn.FieldName == "InternalEpcNumber")
            {
                if (data.Count != 0)
                {
                    foreach (var item in data)
                    {
                        if (e.CellValue.ToString() == item.InternalEpcNumber)
                        {
                            e.Cell.CssClass = "blinktext";
                        }
                    }
                }

                else if (dataApprover.Count != 0)
                {
                    foreach (var item in dataApprover)
                    {
                        if (e.CellValue.ToString() == item.InternalEpcNumber)
                        {
                            e.Cell.CssClass = "blinktext";
                        }
                    }
                }
            }
            else if (e.DataColumn.FieldName == "TextStatus")
            {
                if (dataStatus.Count != 0)
                {
                    foreach (var item in dataStatus)
                    {
                        if (e.CellValue.ToString() == "Open")
                        {
                            Color tlr = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                            e.Cell.BackColor = tlr;
                        }
                        else if (e.CellValue.ToString() == "InProgress")
                        {
                            Color tlr = System.Drawing.ColorTranslator.FromHtml("#ffff00");
                            e.Cell.BackColor = tlr;
                        }
                        else if (e.CellValue.ToString() == "Close")
                        {
                            Color tlr = System.Drawing.ColorTranslator.FromHtml("#3cb371");
                            e.Cell.BackColor = tlr;
                        }
                    }
                }
            }
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string packingMonth, searchpkm, year, yearAndPkm;
        //    packingMonth = dtPkm.Date.ToString("yyyy-MM-dd");

        //    if (dtPkm.Value != null && string.IsNullOrEmpty(cboYear.Text))
        //    {
        //        searchpkm = " WHERE '" + packingMonth + "' (between ia.ValidPeriodFrom and ia.ValidPeriodTo) and ia.__IsDraft !=1 order by ia.Id desc";
        //        BindGrid(searchpkm);
        //    }
        //    else if (dtPkm.Value == null && !string.IsNullOrEmpty(cboYear.Text))
        //    {
        //        year = " WHERE Cast(ia.ValidPeriodFrom as nvarchar) is not null and Cast('20'+Left(substring(ia.InternalEpcNumber, CHARINDEX(' ', ia.InternalEpcNumber)+1, len(ia.InternalEpcNumber)-(CHARINDEX(' ', ia.InternalEpcNumber)-1)),2) as nvarchar) = "
        //            + cboYear.Text + " and ia.__IsDraft !=1 order by ia.Id desc";
        //        BindGrid(year);
        //    }
        //    else if (dtPkm.Value != null && cboYear.Text != null)
        //    {
        //        yearAndPkm = " WHERE ('" + packingMonth + "' between ia.ValidPeriodFrom and ia.ValidPeriodTo) and Cast('20'+Left(substring(ia.InternalEpcNumber, CHARINDEX(' ', InternalEpcNumber)+1, len(ia.InternalEpcNumber)-(CHARINDEX(' ', ia.InternalEpcNumber)-1)),2) as nvarchar) = "
        //            + cboYear.Text + " and Cast(ia.ValidPeriodFrom as nvarchar) is not null and ia.__IsDraft !=1 order by ia.Id desc";
        //        BindGrid(yearAndPkm);
        //    }
        //    else
        //        BindGrid(" where ia.__IsDraft !=1 order by ia.Id desc");
        //}
        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    cboYear.Text = "";
        //    dtPkm.Text = "";
        //}
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            gvIrregAltvw.DataBind();
            exportGrid.FileName = "IrregularAlteration_" + DateTime.Now.ToShortDateString();
            //exportGrid.WriteXlsxToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {

            Response.Redirect("/custom/IAs/IrregularAlterationEdit.aspx");
        }

    }
}