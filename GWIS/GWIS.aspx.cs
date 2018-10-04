using DevExpress.Web;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.GWIS
{
    public partial class GWIS : ListPageBase
    {
        //Default table name
        protected static string tableName = "GWISProcesses";
        protected static string tablePart = "GWISParts";

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
            Session["TableName"] = tableName;

            TableMeta tblParts = schemaInfo.Tables.Where(s => s.Name.Equals(tablePart, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            tblParts.CustomPopupInsert = "popupPartSelection";

            gvPartSelections.DataSource = sdsPartSelections;

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            return true;
        }

        protected void popupPartSelection_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int processId = e.Parameter.ToInt32(0);
            if (processId == 0)
                return;

            //store in session for further use
            Session["GWISProcessId"] = processId;

            int catModelId = Session["GWISCatalogModelId"].ToInt32(0);
            if (catModelId > 0)
            {
                ListEditItem item = cmbCatalogModel.Items.FindByValue(catModelId.ToString());
                if (item != null)
                {
                    cmbCatalogModel.SelectedItem = item;
                    cmbCatalogModel.ClientEnabled = false;
                }
            }
            else
            {
                cmbCatalogModel.ClientEnabled = true;
            }

            if (cmbCatalogModel.Value != null)
            {
                sdsPageLookup.SelectParameters["CatalogModelId"].DefaultValue = cmbCatalogModel.Value.ToString();
                cmbPage.DataSourceID = "";
                cmbPage.DataSource = sdsPageLookup;
                cmbPage.DataBind();
            }
        }

        protected void popupAddPart_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (e.Parameter == "save")
            {
                int processId = Session["GWISProcessId"].ToInt32(0);
                int assyCatalogId = txtEditorId.Text.ToInt32(0);

                if (processId == 0 || assyCatalogId == 0)
                {
                    popupAddPart.JSProperties["cpResult"] = "Invalid GWISProcessId or AssyCatalogId";
                    return;
                }

                using (AppDb ctx = new AppDb())
                {
                    AssyCatalog cat = ctx.AssyCatalogs.Where(x => x.Id == assyCatalogId).FirstOrDefault();
                    if (cat == null)
                        return;

                    GWISPart part = new GWISPart();
                    part.ProcessId = processId;
                    //part.AssyCatalogId = assyCatalogId;
                    part.CatalogPage = txtEditorPage.Text;
                    part.PartNo = txtEditorPartNo.Text;
                    part.PartDescription = txtEditorDescription.Text;
                    part.Qty_Actual = txtEditorQtyActual.Text.ToInt32(0);
                    part.Pos_Actual = txtEditorPosActual.Text.ToInt32(0);
                    part.Pos2_Actual = txtEditorPos2Actual.Text.ToInt32(0);

                    part.SA = cat.SA;
                    
                    part.CatalogModelId = cmbCatalogModel.Value.ToInt32(0);

                    part.CreatedDate = DateTime.Now;
                    part.CreatedBy = PermissionHelper.GetAuthenticatedUserName();

                    ctx.GWISParts.Add(part);

                    try
                    {
                        ctx.SaveChanges();

                        popupAddPart.JSProperties["cpResult"] = "OK";
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.LogError(ex);

                        popupAddPart.JSProperties["cpResult"] = ex.Message;
                    }
                }
            }
        }

        protected void gvPartSelections_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if(e.Column.FieldName == "QtyRemain")
            {
                int remain = e.GetListSourceFieldValue("Qty").ToInt32(0) - e.GetListSourceFieldValue("QtyUsed").ToInt32(0);
                e.Value = remain;
            }
        }

        protected void gvPartSelections_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] arr = e.Parameters.Split(";");
            if (arr.Length != 2)
                return;

            int catModelId = arr[0].ToInt32(0);
            string page = arr[1].Trim();

            //using (SqlConnection conn = new SqlConnection(ConnectionHelper.CONNECTION_STRING))
            //{
                SqlDataSource mySqlDataSource = new SqlDataSource();
                mySqlDataSource.ConnectionString = ConnectionHelper.CONNECTION_STRING;
                mySqlDataSource.SelectCommandType = SqlDataSourceCommandType.Text;
                mySqlDataSource.SelectCommand = "exec dbo.usp_GWIS_PartSelection @CatalogModelId=" + catModelId.ToString() + ", @Page='" + page + "', @PackingMonth=NULL";

                DataView dv = ((DataView)mySqlDataSource.Select(DataSourceSelectArguments.Empty));
                DataTable dt2 = dv.Table;
            //}

            //sdsPartSelections.SelectParameters["CatalogModelId"].DefaultValue = catModelId.ToString();
            //sdsPartSelections.SelectParameters["Page"].DefaultValue = page;

            //DateTime? dt = Session["PackingMonth"].ToDateTime();
            //if (dt != null && dt.Value != DateTime.MinValue)
            //    sdsPartSelections.SelectParameters["PackingMonth"].DefaultValue = dt.ToString("yyyyMM");

            //sdsPartSelections.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            //sdsPartSelections.SelectCommand = "exec dbo.usp_GWIS_PartSelection @CatalogModelId=" + catModelId.ToString() + ", @Page='" + page + "', @PackingMonth=NULL";
            //sdsPartSelections.SelectCommandType = SqlDataSourceCommandType.Text;

            //dv = ((DataView)sdsPartSelections.Select(DataSourceSelectArguments.Empty));
            //if (dv != null)
            //    dt2 = dv.Table;

            SqlDataSource ds = gvPartSelections.DataSource as SqlDataSource;

            ds.SelectParameters["CatalogModelId"].DefaultValue = catModelId.ToString();
            ds.SelectParameters["Page"].DefaultValue = page;

            DateTime? dt = Session["PackingMonth"].ToDateTime();
            if (dt != null && dt.Value != DateTime.MinValue)
                ds.SelectParameters["PackingMonth"].DefaultValue = dt.ToString("yyyyMM");

            dv = ((DataView)ds.Select(DataSourceSelectArguments.Empty));
            if (dv != null)
                dt2 = dv.Table;

            //gvPartSelections.DataSource = sdsPartSelections;
            //gvPartSelections.DataSource = ds;
            gvPartSelections.DataSource = mySqlDataSource;
            gvPartSelections.DataBind();
        }

        protected void cmbPage_Callback(object sender, CallbackEventArgsBase e)
        {
            int catModelId = e.Parameter.ToInt32(0);
            if (catModelId == 0)
                return;

            sdsPageLookup.SelectParameters["CatalogModelId"].DefaultValue = catModelId.ToString();
            cmbPage.DataSourceID = "";
            cmbPage.DataSource = sdsPageLookup;
            cmbPage.DataBind();
        }

        protected void gvPartSelections_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            int startIndex = grid.PageIndex * grid.SettingsPager.PageSize;
            int end = Math.Min(grid.VisibleRowCount, startIndex + grid.SettingsPager.PageSize);

            object[] PartNo = new object[end - startIndex], Description = new object[end - startIndex]
                , Pos = new object[end - startIndex], Pos2 = new object[end - startIndex]
                , Qty = new object[end - startIndex], QtyUsed = new object[end - startIndex], QtyRemain = new object[end - startIndex];
            for (int n = startIndex; n < end; n++)
            {
                PartNo[n - startIndex] = grid.GetRowValues(n, "PartNo");
                Description[n - startIndex] = grid.GetRowValues(n, "Description");
                Pos[n - startIndex] = grid.GetRowValues(n, "Pos");
                Pos2[n - startIndex] = grid.GetRowValues(n, "Pos2");
                Qty[n - startIndex] = grid.GetRowValues(n, "Qty");
                QtyUsed[n - startIndex] = grid.GetRowValues(n, "QtyUsed");
                QtyRemain[n - startIndex] = grid.GetRowValues(n, "QtyRemain");
                //titles[n - startIndex] = grid.GetRowValues(n, "title");
            }
            e.Properties["cpPartNo"] = PartNo;
            e.Properties["cpDescription"] = Description;
            e.Properties["cpPos"] = Pos;
            e.Properties["cpPos2"] = Pos2;
            e.Properties["cpQty"] = Qty;
            e.Properties["cpQtyUsed"] = QtyUsed;
            e.Properties["cpQtyRemain"] = QtyRemain;
            //e.Properties["cpDescription"] = Description;
        }

        //protected void popupGWIS_WindowCallback1(object source, PopupWindowCallbackArgs e)
        //{
        //    ///name;visibleindex;buttonId
        //    string[] arr = e.Parameter.Split(GridViewHelper.SEPARATOR_VALUES);

        //    ASPxGridView grid = this.masterGrid;
        //    int id = grid.GetRowValues(arr[1].ToInt32(-1), "Id").ToInt32(0);

        //    string imageId = RicRepository.GetImgId(id);

        //    popupGWIS.ContentUrl = "/custom/GWIS/DrawGWIS.aspx?ImageID=" + imageId;

        //    ////Session["ImageID"] = a;

        //    //if (a != null)
        //    //{
        //    ////GWISPicture gwisPict = RicRepository.GetGwisPictId(ab);

        //    //    getEditImage(a);
        //    //    setRepeaterAnnot(a);

        //    //}
        //}

    }
}