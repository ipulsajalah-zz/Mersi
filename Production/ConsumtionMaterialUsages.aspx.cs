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

namespace Ebiz.WebForm.Tids.CV.Production
{
    public partial class ConsumtionMaterialUsages : ListPageBase //System.Web.UI.Page
    {
        //Default table name
        //protected static string tableName = "AssyCatalogs";
        protected static string tableName = "vConsumptionMaterialListProcess2";//"uspConsumptionMaterialListProcess"; //
        
        private DataTable dataTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pm = string.Empty;
            string PM = Request.QueryString["PM"];
            string Types = Request.QueryString["Types"];
            string AssyDescrip = Request.QueryString["AssyDescrip"];
            string Station = Request.QueryString["Station"];

            if (PM != null && Types != null && AssyDescrip != null && Station != null)
            {
                lblCaption.Text = PM + ' ' + '/' + ' ' + Types + ' ' + '/' + ' ' + AssyDescrip + ' ' + '/' + ' ' + Station;
            }
            else 
            {
                lblCaption.Text = "";
            }

            if (IsPostBack)
            {
                if (cmbPackingMonth.SelectedItem == null)
                {
                    Session["pm"] = pm;
                }
                else
                {
                    string a = cmbPackingMonth.SelectedItem.ToString();
                    Response.Redirect("/custom/Production/ConsumtionMaterialUsages.aspx?PM=" + a);
                }
            }
            else
            {
                if (PM != null)
                {
                    cmbPackingMonth.Value = PM;
                }
            }
        }
        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            string PM = Request.QueryString["PM"];
            int ProcessId = Request.QueryString["Pid"].ToInt32(0);

            if (ProcessId != null)
            {
                if (ProcessId > 0)
                {
                    tableName = "vConsumptionMaterialListProcess2";
                    tableMeta.AdditionalFilters = string.Format("GwisProcessId = '{0}'", ProcessId);
                }
                else
                {
                    //tableName = "";
                }
            }

            string pm = string.Empty;
            if (PM != null)
            {
                pm = PM;
            }
            else 
            {
                if (Session["pm"] == null || Session["pm"] == string.Empty)
                {
                    return false;
                }
            }

            List<Ebiz.Tids.CV.Repositories.ConsumptionMaterialRepository.ConsumptionFilterLeftMenu> list = ConsumptionMaterialRepository.getConsumptionFilterLeftMenu(pm);

            string temp = hdnfldVariable.Value;

            string Types = Request.QueryString["Types"];

            if (Types == null)
            {
               
                    //tableMeta.AdditionalFilters = "";
                    Session["AssyCatalogsTreeView2"] = null;

            }


            if (Session["AssyCatalogsTreeView2"] == null)
            {


                int idx = 1;
                string types = string.Empty;
                string assydescrip = string.Empty;
                string station = string.Empty;
                string ConstructionGroup = string.Empty;
                int iddata = 1;
                int idsubgroup = 1;

                string leftmenu = @"
                                <div class='container' style='padding:0; margin:0'>
                                    <div class='row row-offcanvas row-offcanvas-left'>
                                        <div class='col-xs-6 col-sm-2 sidebar-offcanvas' id='sidebar' role='navigation'>
                                        <nav>
                                            <ul class='nav'>";
                foreach (var item in list)
                {

                    if (types != item.Types)
                    {
                        if (idx > 1)
                        {
                            leftmenu += @"</ul></li></ul></li>";
                        }


                        leftmenu += string.Format(@" 
                                                <li>
                                                    <a href='#' id='btn-{0}' class='navahref' style='padding-left:10px;' data-toggle='collapse' data-target='#submenu{0}' aria-expanded='false'>{1}</a>

                                                    <ul class='nav collapse' id='submenu{0}' style='padding-left:10px;' role='menu' aria-labelledby='btn-{0}'>
                                                    <li class='lichild1'><a href='#' id='btn-{2}' class='navahref' data-toggle='collapse' data-target='#submenu{2}' aria-expanded='false'>{3}</a></li>

                                                    <ul class='nav collapse' id='submenu{2}' role='menu' aria-labelledby='btn-{2}'>
                                                    <li class='lichild2'><li class='lichild4'><a href='/custom/Production/ConsumtionMaterialUsages.aspx?Types={1}&AssyDescrip={3}&Station={5}&Pid={6}&PM={7}' id='btn-{4}'>{5}</a></li>
                                               ", string.Concat("type-", iddata.ToString()), item.Types, string.Concat("desc-", iddata.ToString()), item.Description, string.Concat("station-", iddata.ToString()), item.StationName, item.gwisprocessid, item.PM);
                        idx += 1;
                        //idgroup += 1;
                    }
                    else
                    {
                        if (assydescrip != item.Description)
                        {
                            leftmenu += @"</ul></li>";
                            leftmenu += string.Format(@"<li class='lichild1'>
                                                    <a href='#' id='btn-{0}' class='navahref' data-toggle='collapse' data-target='#submenu{0}' aria-expanded='false'>{1}</a>
                                                    
                                                    <ul class='nav collapse' id='submenu{0}' role='menu' aria-labelledby='btn-{0}'>
                                                    <li class='lichild2'><a href='/custom/Production/ConsumtionMaterialUsages.aspx?Types={4}&AssyDescrip={1}&Station={3}&Pid={5}&PM={6}' id='btn-{3}'>{3}</a></li>
                                                ", string.Concat("desc-", iddata.ToString()), item.Description, string.Concat("station-", iddata.ToString()), item.StationName, item.Types, item.gwisprocessid, item.PM);
                            //idgroup += 1;
                        }
                        else
                        {
                                if (station != item.StationName)
                                {
                                    leftmenu += string.Format(@"<li class='lichild4'><a href='/custom/Production/ConsumtionMaterialUsages.aspx?Types={1}&AssyDescrip={3}&Station={5}&Pid={6}&PM={7}' id='btn-{4}' class='navahref'>{5}</a>
                                                    ", string.Concat("type-", iddata.ToString()), item.Types, string.Concat("desc-", iddata.ToString()), item.Description, string.Concat("station-", iddata.ToString()), item.StationName, item.gwisprocessid, item.PM);
                                   //idgroup += 1;
                                }
                                //else
                                //{
                                //    leftmenu += string.Format(@"<li class='lichild4'><a href='/custom/Production/ConsumtionMaterialUsages.aspx?Types={1}&AssyDescrip={3}&Station={5}' id='btn-{0}'>{5}</a>", item.Types.Replace(" ", ""), item.Types, item.Description.Replace(" ", ""), item.Description, item.StationName.Replace(" ", ""), item.StationName);
                                //}  
                            
                        }
                    }
                    types = item.Types;
                    assydescrip = item.Description;
                    station = item.StationName;
                    idsubgroup += 1;
                    iddata += 1;
                }

                leftmenu += @"</ul></li></ul></li>";
//                leftmenu += @"<li><a href='#' id='btn-3' data-toggle='collapse' data-target='#submenu3' aria-expanded='false'>KGU MASTER</a>
//                            <ul class='nav collapse' id='submenu3' role='menu' aria-labelledby='btn-3'>
//                                <li class='lichild'><a href='/KGUs/list' target='_blank'>KGUs</a></li>
//                            </ul>
//                            </li>";
                leftmenu += @"</ul>";
                leftmenu += @"</nav>
                </div>
                <div class='col-xs-12 col-sm-10'  style='padding-right:0; margin:0'>
                    <h2 id='types'></h2>
                     <div id='MainContainer' runat='server'>";
                masterPage.MainContent.Controls.Add(new LiteralControl(leftmenu));
                Session["AssyCatalogsTreeView2"] = leftmenu;

            }
            else
            {
                //if (PackingMonth != null)
                //{
                //    string year = Request.QueryString["PackingMonth"].Substring(0, 4);
                //    string model = Request.QueryString["Model"].ToString();
                //    string kguid = Request.QueryString["KguId"].ToString();

                //    string leftmenu = Session["AssyCatalogsTreeView"].ToString();

                //    leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse' id='submenu{1}'", year, model, kguid), string.Format("<ul class='nav collapse in' id='submenu{1}'", year, model, kguid));
                //    Session["AssyCatalogsTreeView"] = leftmenu;
                //}
                masterPage.MainContent.Controls.Add(new LiteralControl(Session["AssyCatalogsTreeView2"].ToString()));

            }

            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }
            
            //set key master
            if (ProcessId > 0) 
            { 
                SetMasterKey("GwisProcessId", ProcessId);
                Session["TableName"] = tableName;
            }
            else 
            {
                Session["TableName"] = string.Empty;
                return false;
            }
            return true;
        }
        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string GetCollapsedTreeView(string id, string flag)
        {

            String value = (String)HttpContext.Current.Session["AssyCatalogsTreeView2a"];

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
                HttpContext.Current.Session["AlterationSummaryTV1"] = leftmenu;

            }
            return "";
        }


    }
}