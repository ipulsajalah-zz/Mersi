using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.STA
{
    public partial class STAPopup : ListPageBase
    {
        //Default table name
        protected static string tableName = "STAs";

        //private DataTable dataTable;

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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

            //TODO: move to configuration
            //tableMeta.PreviewColumnName = "Torque";

            //header info
            int modelId = Request.QueryString["modelid"].ToInt32(0);
            string packingMonth = Request.QueryString["vpm"];
            int rictype = Request.QueryString["rictype"].ToInt32(0);

            int HeaderId = Request.QueryString["id"].ToInt32(0);

            if (HeaderId != 0)
            {
                tableMeta.AdditionalFilters = string.Format("Id = {0}", HeaderId);
            }
            else
            {
                tableMeta.AdditionalFilters = string.Empty;
            }

            //Set master key
            //if (modelId > 0)
            //    SetMasterKey("CatalogModelId", modelId);
            //if (!string.IsNullOrEmpty(packingMonth))
            //    SetMasterKey("PackingMonth", packingMonth);
            //if (rictype > 0)
            //    SetMasterKey("RICTypeId", rictype);

            //create header
            //string modelName = RicRepository.GetModelName(modelId);

            Session["TableName"] = tableName;

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (this.masterGrid != null)
            {
                masterGrid.DetailRows.ExpandAllRows();
                masterGrid.SettingsSearchPanel.Visible = false;
            }

            return true;
        }

    }
}