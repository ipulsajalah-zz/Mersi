using DevExpress.Web;
using DotWeb;
using DotWeb.Models;
using DotWeb.Repositories;
using DotWeb.Utils;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DotMercy.custom.Production
{
    public partial class ProductionPDI0List : ListPageBase
    {
         protected static string tableName = "ProductionPDI0List";

    
         
        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            string org = ControlPlanRepository.GetUserOrganizationQM(assemblyTypeId, user.Id).ToUpper();
            if (org == "QM/VPC")
            {
                //engineering > edit mode
                tableName = "ProductionPDI0ListQM";
            }
            else if (org == "QM/QEP")
            {
                //qm
                tableName = "ProductionPDI0ListQM";
            }
            else if (org == "QM/QA")
            {
                //qm
                tableName = "ProductionPDI0List";
            }
            else if (org == "PROD/APC")
            {
                //logistic
                tableName = "ProductionPDI0List";
            }
            else
            {
                tableName = "ProductionPDI0List";
            }
            //}

            //override if mode is specified 
            if (Request.QueryString["mode"] != null)
            {
                int mode = Request.QueryString["mode"].ToInt32(0);
                if (mode == 1)
                {
                    //qm
                    tableName = "ProductionPDI0ListQM";
                }
                else if (mode == 2)
                {
                    //engineering > edit mode
                    tableName = "ProductionPDI0ListQM";
                }
                else if (mode == 3)
                {
                    //engineering > edit mode
                    tableName = "ProductionPDI0List";
                }
                else if (mode == 4)
                {
                    //logistic
                    tableName = "ProductionPDI0List";
                }
                else
                {
                    tableName = "ProductionPDI0List";
                }
            }

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(tableName, user.Id, assemblyTypeId);
            if (permissions.Count() == 0)
            {
                //set redirect = 1 to prevent AccessDenied.aspx to try to re-check the permission
                Session["redirect"] = 1;
                //redirect
                Response.Redirect(AppConfiguration.PAGE_ACCESSDENIED + "?path=" + HttpContext.Current.Request.RawUrl);
            }


            int stationId = Request.QueryString["Stat"].ToInt32(0);

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

        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/custom/Production/ProductionDashboard.aspx");
        }

    
    }
}