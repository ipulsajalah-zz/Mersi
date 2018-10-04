using DevExpress.Web;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Spa.Models;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Ebiz.WebForm.custom.STA
{
    public partial class AlterationSummary : ListPageBase
    {
        protected static string tableName = "AlterationSummary";
        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            List<AlterationSummaryLeftMenu> list = RicRepository.getAlterationSummaryLeftMenu();
            string temp = hdnfldVariable.Value;

            string PackingMonth = Request.QueryString["PackingMonth"];

            if (PackingMonth != null)
            {
                if (PackingMonth != "All")
                {
                    tableMeta.AdditionalFilters = string.Format("PackingMonth = '{0}'", PackingMonth);
                }
                else
                {
                    tableMeta.AdditionalFilters = "";
                    Session["AlterationSummaryTV"] = null;
              
                }
            }


            if (Session["AlterationSummaryTV"] == null)
            {


                int idx = 1;
                string year = "";
                string leftmenu = @"
                                <div class='container' style='padding:0; margin:0'>
                                    <div class='row row-offcanvas row-offcanvas-left'>
                                        <div class='col-xs-6 col-sm-2 sidebar-offcanvas' id='sidebar' role='navigation'>
                                        <nav>
                                            <ul class='nav'>";
                foreach (var item in list)
                {
                    if (year != item.Year)
                    {
                        if (idx > 1)
                        {
                            leftmenu += @"  </ul>
                                      </li>";
                        }
                        leftmenu += string.Format(@"  
                                                <li><a href='#' id='btn-{1}' class='navahref' data-toggle='collapse' data-target='#submenu{1}' aria-expanded='false'>{1}</a>
                                                    <ul class='nav collapse' id='submenu{1}' role='menu' aria-labelledby='btn-{1}'>
                                                        <li class='lichild'><a href='/custom/STA/AlterationSummary.aspx?PackingMonth={2}'>{3}</a></li>
                                               ", idx, item.Year, item.PackingMonth, item.MonthName);
                        idx += 1;
                    }
                    else
                    {
                        leftmenu += string.Format(@"<li class='lichild'><a href='/custom/STA/AlterationSummary.aspx?PackingMonth={0}'>{1}</a></li>
                                                ", item.PackingMonth, item.MonthName);
                    }
                    year = item.Year;


                }

                leftmenu += @"</nav>
                </div>
                <div class='col-xs-12 col-sm-10'  style='padding-right:0; margin:0'>
                     <div id='MainContainer' runat='server'>";

                masterPage.MainContent.Controls.Add(new LiteralControl(leftmenu));
                Session["AlterationSummaryTV"] = leftmenu;
         
            }
            else
            {
                if (PackingMonth != null)
                {
                    string year = Request.QueryString["PackingMonth"].Substring(0, 4);
                    string leftmenu = Session["AlterationSummaryTV"].ToString();

                    leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse' id='submenu{0}'", year), string.Format("<ul class='nav collapse in' id='submenu{0}'", year));
                    Session["AlterationSummaryTV"] = leftmenu;
                }
                masterPage.MainContent.Controls.Add(new LiteralControl(Session["AlterationSummaryTV"].ToString()));
              
            }
              if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }
         
            return true;
        }

        //
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string GetCollapsedTreeView(string id, string flag)
        {

            String value = (String)HttpContext.Current.Session["AlterationSummaryTV"];

            if (value != null)
            {

                string year = id.Substring(4, 4);
                string leftmenu = value;

                if (flag == "true")
                {
                    leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse in' id='submenu{0}'", year), string.Format("<ul class='nav collapse' id='submenu{0}'", year));
                }
                else
                {
                    leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse' id='submenu{0}'", year), string.Format("<ul class='nav collapse in' id='submenu{0}'", year));
                }
                HttpContext.Current.Session["AlterationSummaryTV"] = leftmenu;

            }
            return "";
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }

        protected void btnlinktochecklist_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/custom/Checklist/Alteration.aspx");
            
        }

       
    }
}