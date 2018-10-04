using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.WebForm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.QM
{
    public partial class CpaChecklist : ListPageBase
    {
        //Default table name
        protected string tableName = "ChecklistHeader";

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

            //create header
            var panel = new System.Web.UI.WebControls.Panel();
            panel.CssClass = "mainContent";
            panel.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>{0}</h2>", tableMeta.Caption)));
            masterPage.MainContent.Controls.Add(panel);

            return true;
        }


    }
}