using DevExpress.Web;
using DevExpress.Web.Data;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public partial class LinkToProcess : ListPageBase
    {
        protected static string tableName = "uspToolProcessNew";//"vToolProcessAssignment";
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

            sdsProductionLineLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
            sdsProductionLineLookup.DataBind();

            ASPxGridView1.DataSource = SqlDataSource1;

            return true;
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxGridView1.DataBind();
        }
        protected void popupLinkToProcess_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

            string[] parameters = e.Parameter.Split(';');
            int line = 1; //cmbProductionLines.SelectedItem.Value.ToInt32(0);
            int ProcessId = Convert.ToInt32(parameters[0]);
            int stationid = Convert.ToInt32(parameters[1]);
            int PartId = Convert.ToInt32(parameters[2]);

            if (ProcessId != null && stationid != null)
            {
                GetGVToolInventories1(ProcessId, line, stationid, PartId);
                Session["ProcessId"] = ProcessId;
                Session["StationId"] = stationid;
                Session["PartId"] = PartId;
                Session["ProdLine"] = line;
                int assemblyTypeId = HttpContext.Current.Session["assemblyType"].ToInt32(0);
            }
        }
        protected void GetGVToolInventories1(int gwisprocessid, int prodline, int StationId, int gwispartid)
        {
            SqlDataSource ds = ASPxGridView1.DataSource as SqlDataSource;
            ds.SelectParameters["gwisprocessid"].DefaultValue = gwisprocessid.ToString();
            ds.SelectParameters["prodline"].DefaultValue = prodline.ToString();
            ds.SelectParameters["StationId"].DefaultValue = StationId.ToString();
            ds.SelectParameters["gwispartid"].DefaultValue = gwispartid.ToString();
            ASPxGridView1.DataBind();
        }
        protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int prodLineId = 1; //e.Parameters.ToInt32(0);
            int ProcessId = Session["ProcessId"].ToInt32(0);
            int stationId = Session["StationId"].ToInt32(0);
            int partid = Session["PartId"].ToInt32(0);
            GetGVToolInventories1(ProcessId, stationId, prodLineId, partid);
        }
        protected void ASPxGridView1_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Number")
            {
                e.Value = string.Format("{0}", e.ListSourceRowIndex + 1);
            }
        }
        protected void ASPxGridView1_PageIndexChanged(object sender, EventArgs e)
        {
            int line = Session["ProdLine"].ToInt32(0);
            int ProcessId = Session["ProcessId"].ToInt32(0);
            int stationid = Session["StationId"].ToInt32(0);
            int PartId = Session["PartId"].ToInt32(0);

            GetGVToolInventories1(ProcessId, line, stationid, PartId);
        }
        protected void ASPxGridView1_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            int line = Session["ProdLine"].ToInt32(0);
            int ProcessId = Session["ProcessId"].ToInt32(0);
            int stationid = Session["StationId"].ToInt32(0);
            int PartId = Session["PartId"].ToInt32(0);

            GetGVToolInventories1(ProcessId, line, stationid, PartId);
        }
        protected void ASPxGridView1_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            int ProcessId = Session["ProcessId"].ToInt32(0);
            int PartId = Session["PartId"].ToInt32(0);
            string sequence = e.NewValues["Sequence"].ToString();
            string inventoryid = e.NewValues["ToolInventoryId"].ToString();
            string assignmentProcessId = "0";
            if (e.NewValues["AssignmentProcessId"] != null)
            {
                assignmentProcessId = e.NewValues["AssignmentProcessId"].ToString();
            }
            SqlDataSource1.UpdateParameters["Sequence"].DefaultValue = sequence.ToString();
            SqlDataSource1.UpdateParameters["GWISPartId"].DefaultValue = PartId.ToString();
            SqlDataSource1.UpdateParameters["GWISProcessId"].DefaultValue = ProcessId.ToString();
            SqlDataSource1.UpdateParameters["ToolInventoryId"].DefaultValue = inventoryid.ToString();
            SqlDataSource1.UpdateParameters["ToolAssignmentProcessId"].DefaultValue = assignmentProcessId.ToString();
            SqlDataSource1.Update();
        }
        protected void ASPxGridView1_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

            switch (e.ButtonType)
            {
                case ColumnCommandButtonType.Update:

                    e.Text = "Assign";
                    //e.Styles.Style.Paddings.PaddingLeft = ;
                    break;

                case ColumnCommandButtonType.Cancel:

                    e.Text = "Cancel";
                    break;

            }
        }

        protected void ASPxGridView1_Load(object sender, EventArgs e)
        {
            int line = Session["ProdLine"].ToInt32(0);
            int ProcessId = Session["ProcessId"].ToInt32(0);
            int stationid = Session["StationId"].ToInt32(0);
            int PartId = Session["PartId"].ToInt32(0);

            GetGVToolInventories1(ProcessId, line, stationid, PartId);
        }
    }
}