using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.WebForm.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.Production
{
    public partial class ConsumtionMaterialDirect : ListPageBase //System.Web.UI.Page
    {
        protected static string tableName = "ConsumptionMaterialsDirect";

        private DataTable dataTable;
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
            return true;
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }

        protected void btnDirect_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/custom/Production/ConsumtionMaterialDirect.aspx");
        }

        protected void btnInDirect_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/custom/Production/ConsumtionMaterialInDirect.aspx");
        }

        protected void btnConsumptionMaterialUsages_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/custom/Production/ConsumtionMaterialUsages.aspx");
        }

    }
}