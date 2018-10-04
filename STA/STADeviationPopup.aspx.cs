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
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Tids.CV.Uploaders;

namespace Ebiz.WebForm.custom.STA
{
    public partial class STADeviationPopup : ListPageBase
    {
        //Default table name
        protected static string tableName = "STADeviations";

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

            //TODO: move to configurations
            //tableMeta.ShowFilterPanel = false;

            //header info
            int modelId = Request.QueryString["modelid"].ToInt32(0);
            string packingMonth = Request.QueryString["vpm"];
            int rictype = Request.QueryString["rictype"].ToInt32(0);

            //Set master key
            if (modelId > 0)
                SetMasterKey("CatalogModelId", modelId);
            if (!string.IsNullOrEmpty(packingMonth))
                SetMasterKey("PackingMonth", packingMonth);
            if (rictype > 0)
                SetMasterKey("RICTypeId", rictype);

            //create header
            //string modelName = RicRepository.GetModelName(modelId);

            Session["TableName"] = tableName;

            string LotNos = RicRepository.GetPackingListLotNos(modelId, packingMonth.ToInt32(0), rictype);

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (this.masterGrid != null)
            {
                masterGrid.DetailRows.ExpandAllRows();
                masterGrid.SettingsSearchPanel.Visible = false;
            }

            if (this.FilterPanel != null)
            {
                filterPanel.Visible = false;
            }

            return true;
        }

    }
}