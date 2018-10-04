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

namespace Ebiz.WebForm.Tids.CV.Catalog
{
    public partial class AssyCatalogs : ListPageBase
    {
        //Default table name
        //protected static string tableName = "AssyCatalogs";
        protected static string tableName = "vAssyCatalogLookup2";

        private DataTable dataTable;
        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            List<Ebiz.Tids.CV.Repositories.AssyCatalogsRepository.AssyCatalogsLeftMenu> list = AssyCatalogsRepository.getAssyCatalogLeftMenu();

            string temp = hdnfldVariable.Value;

            string PackingMonth = Request.QueryString["PackingMonth"];
            string ModelId = Request.QueryString["Model"];
            int KGUs = Request.QueryString["KGUs"].ToInt32(0);
            int KguId = Request.QueryString["KguId"].ToInt32(0);
            string groups = Request.QueryString["Groups"];
            string modeAll = Request.QueryString["Mode"];

            tableMeta.AdditionalFilters = "";
            Session["AssyCatalogsTreeView"] = null;

            int AssyCatalogHeaderId = AssyCatalogsRepository.assycatalogheaderId(PackingMonth, ModelId);
            int getmodelid = AssyCatalogsRepository.getModelId(ModelId);
            //int getkguid = AssyCatalogsRepository.getKguId(AssyCatalogHeaderId, getmodelid);


            //Menu Master KGU and KGU Sub Group
            if (KGUs == 1)
            {
                tableName = "KGUs";
            }
            else
            {
                tableName = "vAssyCatalogLookup2";
            }


            //Additional filter tablemeta
            if (PackingMonth != null)
            {
                if (PackingMonth != "All" || PackingMonth != null)
                {
                    tableMeta.AdditionalFilters = string.Format("AssyCatalogsHeaderId = '{0}' AND CatalogModelId = '{1}' AND Page = '{3}'", AssyCatalogHeaderId, getmodelid, KguId, groups);
                }
                else
                {
                    tableMeta.AdditionalFilters = "";
                    Session["AssyCatalogsTreeView"] = null;
                }
            }
            else if (modeAll != null)
            {
                tableMeta.AdditionalFilters = "";
            }

            //set data tree view on session
            if (Session["AssyCatalogsTreeView"] == null)
            {


                int idx = 1;
                string model = string.Empty;
                string year = string.Empty;
                string packingmonth = string.Empty;
                string ConstructionGroup = string.Empty;
                int idgroup = 1;
                int idsubgroup = 1;

                string leftmenu = @"
                                <div class='container' style='padding:0; margin:0'>
                                    <div class='row row-offcanvas row-offcanvas-left'>
                                        <div class='col-xs-6 col-sm-2 sidebar-offcanvas' id='sidebar' role='navigation'>
                                        <nav>
                                            <ul class='nav'>";
                foreach (var item in list)
                {
                    if (model != item.Model)
                    {
                        if (idx > 1)
                        {
                            leftmenu += @"</ul></li></ul></li></ul></li></ul></li>";
                        }


                        leftmenu += string.Format(@" 
                                                <li>
                                                    <a href='#' id='btn-{0}' class='navahref' style='padding-left:10px;' data-toggle='collapse' data-target='#submenu{0}' aria-expanded='false'>{0}</a>

                                                    <ul class='nav collapse' style='padding-left:10px;' id='submenu{0}' role='menu' aria-labelledby='btn-{0}'>
                                                    <li class='lichild1'><a href='#' id='btn-{11}' class='navahref' data-toggle='collapse' data-target='#submenu{11}' aria-expanded='false'>{1}</a></li>

                                                    <ul class='nav collapse' id='submenu{11}' role='menu' aria-labelledby='btn-{11}'>
                                                    <li class='lichild2'><a href='#'  id='btn-{12}' class='navahref' data-toggle='collapse' data-target='#submenu{12}' aria-expanded='false'>{2}</a>

                                                    <ul class='nav collapse' id='submenu{12}' role='menu' aria-labelledby='btn-{12}'>
                                                    <li class='lichild3'><a href='#'  id='btn-{4}' class='navahref' data-toggle='collapse' data-target='#submenu{4}' aria-expanded='false'>{3}</a>

                                                    <ul class='nav collapse' id='submenu{4}' role='menu' aria-labelledby='btn-{4}'>
                                                    <li class='lichild4'><a href='/custom/Catalog/AssyCatalogs.aspx?PackingMonth={2}&Model={0}&KguId={7}&Groups={9}&PageName={5}&HeaderId={10}'  id='btn-{6}'  data-toggle='collapse' >{5}</a></li>
                                               ", item.Model, item.Year, item.PackingMonth, item.ConstructionGroup, string.Format("ConstructionGroup{0}", idgroup), item.ConstructionSubGroup, string.Format("ConstructionSubGroup{0}", idsubgroup), item.KguId, item.SubKguId, item.Groups, item.AssyCatalogsHeaderId, string.Format("year{0}", idgroup), string.Format("pm{0}", idgroup));
                        idx += 1;
                        idgroup += 1;
                    }
                    else
                    {
                        if (year != item.Year)
                        {
                            leftmenu += @"</ul></li></ul></li></ul></li>";
                            leftmenu += string.Format(@"<li class='lichild1'>
                                                    <a href='#' id='btn-{10}' class='navahref' data-toggle='collapse' data-target='#submenu{10}' aria-expanded='false'>{0}</a>
                                                    
                                                    <ul class='nav collapse' id='submenu{10}' role='menu' aria-labelledby='btn-{10}'>
                                                    <li class='lichild2'><a href='#'  id='btn-{11}' class='navahref' data-toggle='collapse' data-target='#submenu{11}' aria-expanded='false'>{1}</a>

                                                    <ul class='nav collapse' id='submenu{11}' role='menu' aria-labelledby='btn-{11}'>
                                                    <li class='lichild3'><a href='#'  id='btn-{3}' class='navahref' data-toggle='collapse' data-target='#submenu{3}' aria-expanded='false'>{2}</a>

                                                    <ul class='nav collapse' id='submenu{3}' role='menu' aria-labelledby='btn-{3}'>
                                                    <li class='lichild4'><a href='/custom/Catalog/AssyCatalogs.aspx?PackingMonth={1}&Model={6}&KguId={7}&Groups={8}&PageName={4}&HeaderId={9}' id='btn-{5}' data-toggle='collapse'>{4}</a></li>
                                                ", item.Year, item.PackingMonth, item.ConstructionGroup, string.Format("ConstructionGroup{0}", idgroup), item.ConstructionSubGroup, string.Format("ConstructionSubGroup{0}", idsubgroup), item.Model, item.KguId, item.Groups, item.AssyCatalogsHeaderId, string.Format("year{0}", idgroup), string.Format("pm{0}", idgroup));
                            idgroup += 1;
                        }
                        else
                        {
                            if (packingmonth != item.PackingMonth)
                            {
                                leftmenu += @"</ul></li></ul></li>";
                                leftmenu += string.Format(@"<li class='lichild2'>
                                                            <a href='#' id='btn-{9}' class='navahref' data-toggle='collapse' data-target='#submenu{9}' aria-expanded='false'>{0}</a>

                                                            <ul class='nav collapse' id='submenu{9}' role='menu' aria-labelledby='btn-{9}'>
                                                            <li class='lichild3'><a href='#'  id='btn-{2}' class='navahref' data-toggle='collapse' data-target='#submenu{2}' aria-expanded='false'>{1}</a>

                                                            <ul class='nav collapse' id='submenu{2}' role='menu' aria-labelledby='btn-{2}'>
                                                            <li class='lichild4'><a href='/custom/Catalog/AssyCatalogs.aspx?PackingMonth={0}&Model={5}&KguId={6}&Groups={7}&PageName={3}&HeaderId={8}'  id='btn-{4}'>{3}</a></li>
                                                ", item.PackingMonth, item.ConstructionGroup, string.Format("ConstructionGroup{0}", idgroup), item.ConstructionSubGroup, string.Format("ConstructionSubGroup{0}", idsubgroup), item.Model, item.KguId, item.Groups, item.AssyCatalogsHeaderId, string.Format("pm{0}", idgroup));
                                idgroup += 1;
                            }
                            else
                            {
                                if (ConstructionGroup != item.ConstructionGroup)
                                {
                                    leftmenu += @"</ul></li>";
                                    leftmenu += string.Format(@"<li class='lichild3'>
                                                                <a href='#' id='btn-{1}' class='navahref' data-toggle='collapse' data-target='#submenu{1}' aria-expanded='false'>{0}</a>

                                                                <ul class='nav collapse' id='submenu{1}' role='menu' aria-labelledby='btn-{1}'>
                                                                <li class='lichild4'><a href='/custom/Catalog/AssyCatalogs.aspx?PackingMonth={4}&Model={5}&KguId={6}&Groups={7}&PageName={2}&HeaderId={8}'  id='btn-{3}' class='navahref'>{2}</a>
                                                    ", item.ConstructionGroup, string.Format("ConstructionGroup{0}", idgroup), item.ConstructionSubGroup, string.Format("ConstructionSubGroup{0}", idsubgroup), item.PackingMonth, item.Model, item.KguId, item.Groups, item.AssyCatalogsHeaderId);
                                    idgroup += 1;
                                }
                                else
                                {
                                    leftmenu += string.Format(@"<li class='lichild4'><a href='/custom/Catalog/AssyCatalogs.aspx?PackingMonth={2}&Model={3}&KguId={4}&Groups={5}&PageName={0}&HeaderId={6}' id='btn-{1}'>{0}</a>", item.ConstructionSubGroup, string.Format("ConstructionSubGroup{0}", idsubgroup), item.PackingMonth, item.Model, item.KguId, item.Groups, item.AssyCatalogsHeaderId);
                                }
                            }
                        }
                    }
                    year = item.Year;
                    model = item.Model;
                    packingmonth = item.PackingMonth;
                    ConstructionGroup = item.ConstructionGroup;
                    idsubgroup += 1;

                }

                leftmenu += @"</ul></li></ul></li></ul></li></ul></li>";
                leftmenu += @"<li><a href='/custom/Catalog/AssyCatalogs.aspx?Mode=All' id='btn-3' data-toggle='collapse' data-target='#submenu3' aria-expanded='false'>All Model</a>
                           
                            </li>";

                leftmenu += @"<li><a href='#' id='btn-3' data-toggle='collapse' data-target='#submenu4' aria-expanded='false'>KGU MASTER</a>
                            <ul class='nav collapse' id='submenu4' role='menu' aria-labelledby='btn-3'>
                                <li class='lichild'><a href='/KGUs/list' target='_blank'>KGUs</a></li>
                            </ul>
                            </li>";
                leftmenu += @"</ul>";
                leftmenu += @"</nav>
                </div>
                <div class='col-xs-12 col-sm-10'  style='padding-right:0; margin:0'>
                    <h2 id='packingmonth'></h2>
                     <div id='MainContainer' runat='server'>";
                masterPage.MainContent.Controls.Add(new LiteralControl(leftmenu));
                Session["AssyCatalogsTreeView"] = leftmenu;

            }
            else
            {
                if (PackingMonth != null)
                {
                    string year = Request.QueryString["PackingMonth"].Substring(0, 4);
                    string model = Request.QueryString["Model"].ToString();
                    string kguid = Request.QueryString["KguId"].ToString();

                    string leftmenu = Session["AssyCatalogsTreeView"].ToString();

                    leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse' id='submenu{1}'", year, model, kguid), string.Format("<ul class='nav collapse in' id='submenu{1}'", year, model, kguid));
                    Session["AssyCatalogsTreeView"] = leftmenu;
                }
                masterPage.MainContent.Controls.Add(new LiteralControl(Session["AssyCatalogsTreeView"].ToString()));

            }
            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            ////header info
            //int modelId = Request.QueryString["model"].ToInt32(0);
            //string packingMonth = Request.QueryString["vpm"];

            //Set master key
            if (AssyCatalogHeaderId > 0)
                SetMasterKey("AssyCatalogsHeaderId2", AssyCatalogHeaderId);
            if (getmodelid > 0)
                SetMasterKey("CatalogModelId2", getmodelid);
            if (groups != null)
                SetMasterKey("Page2", groups);
            //create header
            //string modelName = RicRepository.GetModelName(modelId);
            Session["TableName"] = tableName;
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

            //String value = (String)HttpContext.Current.Session["AssyCatalogsTreeView"];

            //if (value != null)
            //{

            //    string year = id;//.Substring(4, 4);
            //    string leftmenu = value;

            //    if (flag == "true")
            //    {
            //        leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse in' id='submenu{0}'", year), string.Format("<ul class='nav collapse' id='submenu{0}'", year));
            //    }
            //    else
            //    {
            //        leftmenu = leftmenu.Replace(string.Format("<ul class='nav collapse' id='submenu{0}'", year), string.Format("<ul class='nav collapse in' id='submenu{0}'", year));
            //    }
            //    HttpContext.Current.Session["AssyCatalogsTreeView"] = leftmenu;

            //}
            return "";
        }

        #region Upload Document

        protected void popupDocUpload_OnWindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "upload")
            {
                string filePath = Session["pl_uploaded_file"] as string;

                AssyCatalogsUploader uploader = new AssyCatalogsUploader();

                //set username for column createdby and modifiedby
                uploader.Username = PermissionHelper.GetAuthenticatedUserName();

                //parameter
                DateTime value = DateTime.Now;
                int packingMonth = value.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture).ToInt32(0);
                int modelId = cmbModels.Value.ToInt32(0);

                //parse the file
                string message = "";
                int rowCount = uploader.Parse(filePath, packingMonth, modelId);
                if (rowCount > 0)
                {
                    lblSuccess.Text = "Successfully upload and process data " + Path.GetFileName(filePath) + ", " + rowCount + " rows inserted, " + message;
                    this.masterGrid.DataBindSafe();
                }
                else
                {
                    if (uploader.ErrorMessage.Length > 0)
                    {
                        lblSuccess.Text = uploader.ErrorMessage;
                    }
                    else
                    {
                        lblSuccess.Text = "Upload file " + Path.GetFileName(filePath) + " failed!";
                    }
                    //delete the file
                    File.Delete(filePath);

                }
            }
        }

        protected void fileUploadControl_OnFileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                string filePath = "";
                if (e.IsValid)
                {
                    filePath = SavePostedFile(e.UploadedFile, "AssyCatalogs");
                }

                Session["pl_uploaded_file"] = filePath;
            }
            catch (Exception ex)
            {
                e.ErrorText = ex.Message + " " + ex.StackTrace;
                e.IsValid = false;
                e.CallbackData = "Upload file failed.  " + ex.Message;
                LoggerHelper.LogError(ex);
                LoggerHelper.LogError(e.ErrorText);
            }
        }

        private string SavePostedFile(UploadedFile eUploadedFile, string fileTypeName)
        {
            if (!eUploadedFile.IsValid) return String.Empty;

            //=========cek folder Packing Month
            string path = ConfigurationHelper.UPLOAD_DIR + "/" + fileTypeName + "/" + Session["ModelName"];
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }

            //=========cek folder Model
            string pathModel = path + "/" + Session["PackingMonth"];
            if (!Directory.Exists(Server.MapPath(pathModel)))
            {
                Directory.CreateDirectory(Server.MapPath(pathModel));
            }

            String UploadDir = pathModel + "/";

            FileInfo fileInfo = new FileInfo(eUploadedFile.FileName);
            String fileName = eUploadedFile.FileName.ToString().Replace(" ", "_");
            String ext = Path.GetExtension(eUploadedFile.FileName);
            String fileType = eUploadedFile.ContentType.ToString();

            String resFileName = "";
            if (!File.Exists(UploadDir + fileName))
            {
                resFileName = Server.MapPath(UploadDir + fileName);
                eUploadedFile.SaveAs(resFileName);
            }

            return resFileName;
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            try
            {
                popupDocUpload.ShowOnPageLoad = false;

            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                popupDocUpload.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string day = DateTime.Now.Day.ToString();
            String filename = String.Format("AssyCatalogs_{0}_{1}", date, day);

            Export(this.masterGrid, this.tableMeta.Name, filename);
        }

    }

}