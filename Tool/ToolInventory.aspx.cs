using DevExpress.Web;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ebiz.Tids.CV.Utils;

namespace Ebiz.WebForm.Tids.CV.custom.Tool
{
    public partial class ToolInventory : ListPageBase
    {
        //Default table name
        protected static string tableName = "ToolInventories";

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            sdsProductionLineLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
            cmbProductionLines.DataSource = sdsProductionLineLookup;

            sdsAssemblySectionLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
            cmbAssemblySections.DataSource = sdsAssemblySectionLookup;
            cmbStations.DataSource = sdsStationLookup;

            cmbAssemblyTypes.DataSource = sdsAssemblyTypeLookup;

            //sdsTypeLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();

            //sdsToolRackLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            var panel = new System.Web.UI.WebControls.Panel();
            panel.CssClass = "mainContent";
            panel.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>{0}</h2>", tableMeta.Caption)));
            masterPage.MainContent.Controls.Add(panel);

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (MasterGrid == null)
                return true;

            #region MasterGrid

            //enable row tracking
            MasterGrid.SettingsBehavior.EnableRowHotTrack = true;

            #endregion

            return true;
        }

        protected void popupToolReturn_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            string[] str = e.Parameter.Split(';');
            if (str.Length != 2) return;

            int toolInvId = str[1].ToInt32(0);
            if (toolInvId == 0) return;

            //store in sesion
            Session["ToolInvId"] = toolInvId;

            LoadToolRackPopup(toolInvId);
        }

        protected void btnReturnTool_Click(object sender, EventArgs e)
        {
            int toolInvId = Session["ToolInvId"].ToInt32(0);

            int rackId = cmbRacks.Value.ToInt32(0);
            bool toolStatus = !chkStatus.Checked;

            bool status = ToolRepository.ReturnTool(rackId, toolStatus, toolInvId, User.UserName);

            if (status)
                popupToolReturn.ShowOnPageLoad = false;

            masterGrid.DataBind();
        }

        protected void popupToolAssign_Load(object sender, EventArgs e)
        {
            //IMPORTANT: dont do anything here since it is reloaded when clicking Save
            ////get from session
            //int toolInvId = Session["ToolInvId"].ToInt32(0);

            //LoadToolAssignmentPopup(toolInvId);
        }

        protected void popupToolAssign_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            string[] str = e.Parameter.Split(';');
            if (str.Length != 2) return;

            int toolInvId = str[1].ToInt32(0);
            if (toolInvId == 0) return;

            //store in sesion
            Session["ToolInvId"] = toolInvId;

            LoadToolAssignmentPopup(toolInvId);
        }

        protected void cmbStations_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            GetDDLStations(e.Parameter.ToInt32(0));
        }

        protected void cmbAssemblySections_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            GetDDLAssemblySections(e.Parameter.ToInt32(0));
        }

        protected void btnAssignTool_Click(object sender, EventArgs e)
        {
            int toolInvId = Session["ToolInvId"].ToInt32(0);

            int assemblyTypeId = cmbAssemblyTypes.Value.ToInt32(0);
            int prodLineId = cmbProductionLines.Value.ToInt32(0);
            int assemblySectionId = cmbAssemblySections.Value.ToInt32(0);
            int stationId = cmbStations.Value.ToInt32(0);
            if (prodLineId == 0 || assemblySectionId == 0 || stationId == 0)
                return;
            decimal setNm = txtSetNM.Text.Trim().ToDecimal(0);

            string modelIdList = "";
            for (int i = 0; i < chkGWISTypes.SelectedValues.Count; i++)
            {
                if (modelIdList.Length == 0)
                    modelIdList = chkGWISTypes.SelectedValues[i].ToString();
                else
                    modelIdList = String.Format("{0},{1}", modelIdList, chkGWISTypes.SelectedValues[i].ToString());
            }

            bool status = ToolRepository.AssignToolStation(assemblyTypeId, prodLineId, assemblySectionId, stationId, setNm, toolInvId, User.UserName);
            if (status)
                status = ToolRepository.AssignToolModels(assemblyTypeId, modelIdList, toolInvId, User.UserName);

            if (status)
                popupToolAssign.ShowOnPageLoad = false;

            masterGrid.DataBind();
        }

        private void LoadToolRackPopup(int toolInvId)
        {
            //get current station assignment
            Ebiz.Tids.CV.Models.ToolInventory ti = GetToolInventory(toolInvId);
            Session["ToolInventory"] = ti;

            if (ti != null && ti.RackId != null)
            {
                GetDDLToolRack(ti.RackId.Value);
            }
            else
            {
                GetDDLToolRack(0);
            }
        }

        private void GetDDLToolRack(int RackId)
        {
            cmbRacks.DataBind();

            if (RackId > 0)
            {
                ListEditItem item = cmbRacks.Items.FindByValue(RackId.ToString());
                cmbProductionLines.SelectedItem = item;
            }
            else
            {
                cmbRacks.SelectedItem = null;
            }
        }

        //private ToolAssignmentStation GetStationAssignment(int toolInvId)
        //{
        //    using (AppDb ctx = new AppDb())
        //    {
        //        return ctx.ToolAssignmentStations.Where(x => x.ToolInventoryId == toolInvId && x.HasDeviation == true).FirstOrDefault();
        //    }
        //}

        private AssemblySection GetAssemblySection(int assySectionId)
        {
            using (AppDb ctx = new AppDb())
            {
                return ctx.AssemblySections.Where(x => x.Id == assySectionId).FirstOrDefault();
            }
        }

        private Ebiz.Tids.CV.Models.ToolInventory GetToolInventory(int toolInvId)
        {
            using (AppDb ctx = new AppDb())
            {
                return ctx.ToolInventories.Where(x => x.Id == toolInvId).FirstOrDefault();
            }
        }

        private ProductionLine GetProductionLine(int prodLineId)
        {
            using (AppDb ctx = new AppDb())
            {
                return ctx.ProductionLines.Where(x => x.Id == prodLineId).FirstOrDefault();
            }
        }

        private void LoadToolAssignmentPopup(int toolInvId)
        {
            //reset assignment
            cmbProductionLines.SelectedItem = null;
            cmbAssemblySections.SelectedItem = null;
            cmbStations.SelectedItem = null;

            //cmbProductionLines.DataBind();

            ////get current station assignment
            //ToolAssignmentStation tap = GetStationAssignment(toolInvId);
            //Session["ToolAssignmentStation"] = tap;

            Ebiz.Tids.CV.Models.ToolInventory ti = GetToolInventory(toolInvId);

            if (ti.AssemblyTypeId != null && ti.AssemblyTypeId.Value > 0 
                    && ti.ProductionLineId != null && ti.ProductionLineId.Value > 0 
                    && ti.AssemblySectionId != null && ti.AssemblySectionId.Value > 0
                    && ti.StationId != null && ti.StationId.Value > 0)
            {
                GetDDLAssemblyTypes(ti.AssemblyTypeId.Value);
                GetDDLProductionLines(cmbAssemblyTypes.Value.ToInt32(0), ti.ProductionLineId.Value);
                GetDDLAssemblySections(cmbProductionLines.Value.ToInt32(0), ti.AssemblySectionId.Value);
                GetDDLStations(cmbAssemblySections.Value.ToInt32(0), ti.StationId.Value);

                txtSetNM.Value = ti.SetNM;
                GetCHKTypes(ti.Id);
            }
            else
            {
                GetDDLAssemblyTypes(PermissionHelper.GetCurrentAssemblyTypeId());
                GetDDLProductionLines(cmbAssemblyTypes.Value.ToInt32(0));
                GetDDLAssemblySections(cmbProductionLines.Value.ToInt32(0));
                GetDDLStations(cmbAssemblySections.Value.ToInt32(0));

                txtSetNM.Value = "";
                GetCHKTypes(0);
            }

        }

        private void GetCHKTypes(int ToolInvId)
        {
            chkGWISTypes.DataBind();
          
            //get current value
            if (ToolInvId == 0)
            {
                foreach (ListEditItem item in chkGWISTypes.Items)
                {
                    item.Selected = false;
                }

            }
            else
            {
                string typeIdList = ToolRepository.GetCurrentAssignedModels(ToolInvId);
                if (typeIdList == null) return;

                string[] typeIds = typeIdList.Split(',');
                foreach (string typeId in typeIds)
                {
                    foreach (ListEditItem item in chkGWISTypes.Items)
                    {
                        if (item.Value.Equals(typeId))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }

            }
        }

        private void GetDDLAssemblyTypes(int defaultValue = 0)
        {
            cmbAssemblyTypes.DataBind();

            if (defaultValue > 0)
            {
                ListEditItem item = cmbAssemblyTypes.Items.FindByValue(defaultValue.ToString());
                cmbAssemblyTypes.SelectedItem = item;
            }
        }

        private void GetDDLProductionLines(int assemblyTypeId, int defaultValue = 0)
        {
            if (assemblyTypeId == 0)
            {
                cmbProductionLines.Items.Clear();
                cmbProductionLines.SelectedItem = null;
                return;
            }

            SqlDataSource ds = cmbProductionLines.DataSource as SqlDataSource;
            ds.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
            cmbProductionLines.DataBind();

            if (defaultValue > 0)
            {
                ListEditItem item = cmbProductionLines.Items.FindByValue(defaultValue.ToString());
                cmbProductionLines.SelectedItem = item;
            }
        }

        private void GetDDLAssemblySections(int prodLineId, int defaultValue = 0)
        {
            if (prodLineId == 0)
            {
                cmbAssemblySections.Items.Clear();
                cmbAssemblySections.SelectedItem = null;
                return;
            }

            int assemblySectionType = 0;

            ProductionLine line = GetProductionLine(prodLineId);
            if (line == null) return;

            if (line.LineNumber < AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER)
            {
                assemblySectionType = (int)EAssemblySectionType.MainLine;
            }
            else if (line.LineNumber == AppConfiguration.PRODUCTION_SUBASSY_LINENUMBER)
            {
                assemblySectionType = (int)EAssemblySectionType.SubAssemblyLine;
            }
            else if (line.LineNumber == AppConfiguration.PRODUCTION_ENDOFLINE_LINENUMBER)
            {
                assemblySectionType = (int)EAssemblySectionType.EndOfLine;
            }
            else if (line.LineNumber == AppConfiguration.PRODUCTION_FINISHLINE_LINENUMBER)
            {
                assemblySectionType = (int)EAssemblySectionType.FinishLine;
            }
            else
            {
                return;
            }

            SqlDataSource ds = cmbAssemblySections.DataSource as SqlDataSource;
            ds.SelectParameters["AssemblySectionType"].DefaultValue = assemblySectionType.ToString();
            cmbAssemblySections.DataBind();
            //if (ds.SelectParameters["AssemblySectionType"].DefaultValue != assemblySectionType.ToString())
            //{
            //    ds.SelectParameters["AssemblySectionType"].DefaultValue = assemblySectionType.ToString();
            //    cmbAssemblySections.DataBind();
            //}

            if (defaultValue > 0)
            {
                ListEditItem item = cmbAssemblySections.Items.FindByValue(defaultValue.ToString());
                cmbAssemblySections.SelectedItem = item;
            }

        }

        private void GetDDLStations(int assemblySectionId, int defaultValue = 0)
        {
            if (assemblySectionId == 0)
            {
                cmbStations.Items.Clear();
                cmbStations.SelectedItem = null;
                return;
            }

            SqlDataSource ds = cmbStations.DataSource as SqlDataSource;
            ds.SelectParameters["AssemblySectionId"].DefaultValue = assemblySectionId.ToString();
            cmbStations.DataBind();

            if (defaultValue > 0)
            {
                ListEditItem item = cmbStations.Items.FindByValue(defaultValue.ToString());
                cmbStations.SelectedItem = item;
            }
        }
    }
}