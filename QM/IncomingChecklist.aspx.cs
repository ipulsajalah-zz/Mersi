using DevExpress.Web;
using DevExpress.Web.Data;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Utils;
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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;

namespace Ebiz.WebForm.Tids.CV.QM
{
    public partial class IncomingChecklist : ListPageBase
    {
        //Default table name
        protected string tableName = "IncomingChecklists";

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            int userId = PermissionHelper.GetAuthenticatedUserId();

            //if we have permission to edit/delete, use the QM/ENG/LOG configuration
            if (IsOrganizationUser("QM", userId))
            {
                //qm
                tableName = "IncomingChecklists";
            }
            else if (IsOrganizationUser("LOG", userId))
            {
                tableName = "IncomingChecklistsLOG";
            }
            else if (IsOrganizationUser("PPC", userId))
            {
                tableName = "IncomingChecklistsPPC";
            }
            else
            {
                tableName = "";
            }

            //override if mode is specified 
            if (Request.QueryString["mode"] != null)
            {
                int mode = Request.QueryString["mode"].ToInt32(0);
                if (mode == 1)
                {
                    //qm
                    tableName = "IncomingChecklists";
                }
                else if (mode == 4)
                {
                    //logistic
                    tableName = "IncomingChecklistsLOG";
                }
                else if (mode == 5)
                {
                    //logistic
                    tableName = "IncomingChecklistsPPC";
                }
                else
                {
                    tableName = "IncomingChecklists";
                }
            }

            //check if we have valid tablename
            if (tableName == "")
            {
                //set redirect = 1 to prevent AccessDenied.aspx to try to re-check the permission
                Session["redirect"] = 1;
                //redirect
                Response.Redirect(ConfigurationHelper.PAGE_ACCESSDENIED + "?path=" + HttpContext.Current.Request.RawUrl);
            }

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(tableName, user.Id, assemblyTypeId);
            if (permissions.Count() == 0)
            {
                //set redirect = 1 to prevent AccessDenied.aspx to try to re-check the permission
                Session["redirect"] = 1;
                //redirect
                Response.Redirect(ConfigurationHelper.PAGE_ACCESSDENIED + "?path=" + HttpContext.Current.Request.RawUrl);
            }

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

        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }

        public bool IsOrganizationUser(string OrgName, int UserId)
        {
            string query = @"select count(*)
                             from dbo.UserOrganizations uo
                             inner join dbo.Configs c on c.[Group] = 'OrganizationCV' 
                                and c.[Key] = '" + OrgName + @"' 
                                and cast(c.Value as int) = uo.OrganizationId 
                             where uo.UserId = " + UserId;

            int count = SqlHelper.ExecuteSqlCommandScalar(query).ToInt32(0);

            return (count > 0);
        }

    }
}