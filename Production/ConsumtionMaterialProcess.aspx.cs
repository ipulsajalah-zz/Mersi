using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Uploaders;
using Ebiz.Scaffolding.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.Production
{
    public partial class ConsumtionMaterialProcess : ListPageBase
    {
        protected static string tableName = "vConsumptionMaterialProcess";

        private DataTable dataTable;
        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            int ProcessId = Request.QueryString["gwisprocessid"].ToInt32(0);
            if (ProcessId != null)
            {
                if (ProcessId > 0)
                {
                    tableName = "vConsumptionMaterialProcess";
                    tableMeta.AdditionalFilters = string.Format("GWISProcessId = '{0}'", ProcessId);
                }
                else
                {
                    //
                }
            }
            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            //set key master
            if (ProcessId > 0)
               SetMasterKey("GwisProcessId", ProcessId);

               Session["TableName"] = tableName;
            return true;
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }

        protected void LinkConsumptionMaterialListDirect_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/custom/Production/ConsumtionMaterialInDirect.aspx");
        }

        protected void btnConsumptionMaterialUsages_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/custom/Production/ConsumtionMaterialUsages.aspx");
        }
    }
}