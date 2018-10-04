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

namespace Ebiz.WebForm.custom.PPC
{
    public partial class ProductionSequenceDetail : ListPageBase
    {
        protected static string tableName = "ProductionSequenceDetail";

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
    }
}