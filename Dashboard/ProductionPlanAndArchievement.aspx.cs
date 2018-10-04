using DevExpress.Web;
using DotWeb;
using DotWeb.Models;
using DotWeb.Utils;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Dashboard
{
    public partial class ProductionPlanAndArchievement : ListPageBase
    {
        protected TableMeta tableMeta = null;
        protected ASPxGridView masterGrid = null;
        protected ASPxFormLayout filterPanel = null;
        protected string connectionString = "";
        protected List<EPermissionType> permissions = null;

        protected ASPxGridViewExporter gridExporter = null;

        protected static string tableName = "ProductionPlanAndArchievement";

        /// <summary>
        /// Page Init event; creates all required controls in a page.
        /// </summary>
        /// <param name="sender">The page sending the event.</param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            var user = HttpContext.Current.Session["user"] as User;
            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }
            int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToString().ToInt32(0);

            //if (userId == 0 || assemblyTypeId == 0)
            //{
            //    invalid state. forced to logout
            //    TODO: User RolePermissionHelper.Logout
            //    RolePermissionHelper.RedirectToLogin();
            //    Response.Redirect("/Account/Logout.aspx");
            //}

            var masterPage = this.Controls[0] as IMainMaster;
            if (masterPage == null)
                Response.Write("<p>Your master page must implement IMainMaster interface.</p>");

            //Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                var page = this.Controls[0] as IMainMaster;
                page.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return;
            }

            //header info
            string StationAct = Request.QueryString["StationAct"].ToString();
            int ProductionLineId = Convert.ToInt32(Request.QueryString["ProductionLineId"]);
            string Date1 = Request.QueryString["Date1"].ToString();
            //DateTime Date = DateTime.Parse(dt);

            string header = "";
            //parameter to the stored procedure
            //if it is a stored procedure, use comma as separator for paramater/filter. do not user AND/OR logical operation
            string queryFilter = null;
            if (tableMeta.TableType == ETableType.StoredProcedure)
            {
                queryFilter = string.Format("Date1={0}{1}ProductionLineId={2}{3}StationAct={4}", Date1, AppConfiguration.SP_FILTER_SEPARATOR, ProductionLineId, AppConfiguration.SP_FILTER_SEPARATOR, StationAct);
            }
            else
            {

                if (StationAct !="")
                {
                    queryFilter = "StationAct=" + StationAct;
                    if (ProductionLineId > 0)
                    {
                        queryFilter += " AND ProductionLineId=" + ProductionLineId;
                    }
                    if (Date1 !=null)
                    {
                        queryFilter += " AND Date=" + Date1;
                    }
                }
            }

            //create the page
            var gridCreator = new MasterGrid(this, this.tableMeta);
            masterGrid = gridCreator.Render(userId, assemblyTypeId, queryFilter);

            //create filter panel
            var filterPanelCreator = new FilterPanel(this, this.tableMeta);
            filterPanel = filterPanelCreator.Render(userId, assemblyTypeId);

            //create hidden field to store temporary data
            var field = new ASPxHiddenField();
            field.ID = HIDDEN_FIELD_ID;
            field.ClientInstanceName = HIDDEN_FIELD_ID;

            //var scriptManager = new ASPxScriptManager();

            var panel = new System.Web.UI.WebControls.Panel();
            panel.CssClass = "mainContent";
            panel.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>{0}</h2>", tableMeta.Caption)));
            panel.Controls.Add(new LiteralControl(string.Format("<p>"+ StationAct + " Line " +ProductionLineId+" On "+ Date1 + "</p>")));

            if (filterPanel != null)
                panel.Controls.Add(filterPanel);
            panel.Controls.Add(masterGrid);
            if (masterGrid.Width.Value > 0)
                panel.Width = masterGrid.Width;

            panel.Controls.Add(field);

            if (tableMeta.ShowExportButton)
            {
                var buttonPanel = new System.Web.UI.WebControls.Panel();
                buttonPanel.CssClass = "grid-button-panel";
                buttonPanel.Width = new Unit("100%");
                //buttonPanel.BorderWidth = new Unit("1px");

                //add export button
                ASPxButton btnExport = new ASPxButton();
                btnExport.ID = "btnExportGrid";
                btnExport.Text = "Export to Excel";
                btnExport.Click += btnExport_click;
                btnExport.CssClass = "btn-export-xls";

                //btnExport.Width = new Unit("100%");

#if _BTN_NATIVE_
                btnExport.CssClass = "btn btn-export btn-xs";
                btnExport.Native = true;
#endif

                buttonPanel.Controls.Add(btnExport);
                panel.Controls.Add(buttonPanel);

                //add exporter grid
                gridExporter = new ASPxGridViewExporter();
                gridExporter.GridViewID = masterGrid.ID; //string.Concat(table, "GridView");
                gridExporter.ID = "GridExport";
                gridExporter.FileName = tableMeta.Name + " (" + header + ")";
                gridExporter.ExportedRowType = GridViewExportedRowType.All;

                panel.Controls.Add(gridExporter);

                ////add script manager
                //panel.Controls.Add(scriptManager);
            }

            masterPage.MainContent.Controls.Add(panel);
            masterPage.PageTitle.Controls.Add(new LiteralControl(tableMeta.Caption));

            ////re-apply the filtering if necessary
            //if (tableMeta.ShowFilterPanel)
            //{
            //    GridViewHelper.ApplyGridViewFilter(masterGrid, filterPanel, tableMeta);
            //}
            //else
            //{
            //    masterGrid.DataBind();
            //}

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (tableMeta == null || masterGrid == null) return;

            //re-apply the filtering if necessary
            if (tableMeta == null)
                return;

            if (tableMeta.ShowFilterPanel)
            {
                GridViewHelper.ApplyGridViewFilter(masterGrid, filterPanel, tableMeta);
            }
            else
            {
                masterGrid.DataBind();
            }
        }
        void btnExport_click(object sender, EventArgs e)
        {
            gridExporter.WriteXlsxToResponse();

            //ASPxButton button = (ASPxButton)sender;
            //foreach (Control item in button.Parent.Controls)
            //{
            //    ASPxGridViewExporter exp = item as ASPxGridViewExporter;
            //    if (exp != null)
            //    {
            //        exp.WriteXlsxToResponse();
            //        break;
            //    }
            //}
        }

        public override NameValueCollection QueryString
        {
            get { return Request.QueryString; }
        }
    }
}