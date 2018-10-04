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

namespace Ebiz.WebForm.Tids.CV.ControlPlans
{
    public partial class CPStationDetail : ListPageBase
    {
        //Default table name
        protected string tableName = "ControlPlanStationDetail";

        private DataTable dataTable;

        ControlPlan cp = null;
        ControlPlanProcess cpp = null;

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            ////if we have permission to edit/delete, use the QM/ENG/LOG configuration
            //string org = ControlPlanRepository.GetUserOrganization(assemblyTypeId, user.Id).ToUpper();
            //if (org == "QM")
            //{
            //    //qm
            //    tableName = "CPStationDetailQM";
            //}
            //else if (org == "QEP/SUPERVISOR")
            //{
            //    //engineering > edit mode
            //    tableName = "CPStationDetailQM";
            //}
            //else if (org == "ENG")
            //{
            //    //engineering > edit mode
            //    tableName = "CPStationDetailENG";
            //}
            //else if (org == "LOG")
            //{
            //    //logistic
            //    tableName = "CPStationDetailLOG";
            //}
            //else
            //{
            //    tableName = "CPStationDetail";
            //}

            ////override if mode is specified 
            //if (Request.QueryString["mode"] != null)
            //{
            //    int mode = Request.QueryString["mode"].ToInt32(0);
            //    if (mode == 1)
            //    {
            //        //qm
            //        tableName = "CPStationDetailQM";
            //    }
            //    else if (mode == 2)
            //    {
            //        //engineering > edit mode
            //        tableName = "CPStationDetailQM";
            //    }
            //    else if (mode == 3)
            //    {
            //        //engineering > edit mode
            //        tableName = "CPStationDetailENG";
            //    }
            //    else if (mode == 4)
            //    {
            //        //logistic
            //        tableName = "CPStationDetailLOG";
            //    }
            //    else
            //    {
            //        tableName = "CPStationDetail";
            //    }
            //}

            ////since we determine the table dynamically, check for permission explicitly
            //permissions = PermissionHelper.GetTablePermissions(tableName, user.Id, assemblyTypeId);
            //if (permissions.Count() == 0)
            //{
            //    //set redirect = 1 to prevent AccessDenied.aspx to try to re-check the permission
            //    Session["redirect"] = 1;
            //    //redirect
            //    Response.Redirect(ConfigurationHelper.PAGE_ACCESSDENIED + "?path=" + HttpContext.Current.Request.RawUrl);
            //}

            ////get TableMeta from Schema. Schema is loaded during login
            //var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            //tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            //if (tableMeta == null)
            //{
            //    masterPage.MainContent.Controls.Clear();
            //    masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
            //    return false;
            //}

            //TODO: move to configuration
            //tableMeta.PreviewColumnName = "Torque";
            //tableMeta.Height = 600;

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            //header info
            //int vpm = Request.QueryString["vpm"].ToInt32(0);
            //int modelId = Request.QueryString["model"].ToInt32(0);
            //int variantId = Request.QueryString["variant"].ToInt32(0);
            int cpId = Request.QueryString["CpId"].ToInt32(0);
            int stationId = Request.QueryString["StationId"].ToInt32(0);
            //if (stationId == 0)
            //    stationId = Request.QueryString["station"].ToInt32(0);

            using (AppDb ctx = new AppDb())
            {
                if (cpId > 0)
                {
                    cp = ctx.ControlPlans.Where(x => x.Id == cpId).FirstOrDefault();
                }
                //else
                //{
                //    cp = ctx.ControlPlans.Where(x => x.ModelId == modelId && x.VariantId == variantId && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                //}
            }

            if (cp == null)
            {
                var panel1 = new System.Web.UI.WebControls.Panel();
                panel1.CssClass = "mainContent";
                panel1.Controls.Clear();
                panel1.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>Invalid Model/Variant/PackingMonth for CP Station Detail</h2>")));

                masterPage.MainContent.Controls.Clear();
                masterPage.MainContent.Controls.Add(panel1);
                masterPage.PageTitle.Controls.Add(new LiteralControl(tableMeta.Caption));
                return false;
            }
            else if (stationId == 0)
            {
                var panel1 = new System.Web.UI.WebControls.Panel();
                panel1.CssClass = "mainContent";
                panel1.Controls.Clear();
                panel1.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>Invalid Station for CP Station Detail</h2>")));

                masterPage.MainContent.Controls.Clear();
                masterPage.MainContent.Controls.Add(panel1);
                masterPage.PageTitle.Controls.Add(new LiteralControl(tableMeta.Caption));
                return false;
            }

            //Set master key
            SetMasterKey("ControlPlanId", cp.Id);

            //additional filter
            queryFilter = string.Format("StationId={0}", stationId);

            //Store the CP object so that it is accessible to other classes
            keyValues.Add("CP", cp);
            keyValues.Add("ControlPlanId", cp.Id);
            keyValues.Add("StationId", stationId);

            //create header
            string ModelName = ControlPlanRepository.RetrieveAssemblyModelNameById(cp.AssemblyModelId);
            string StationName = ControlPlanRepository.RetrieveStationNameById(stationId);

            lblStationName.Text = StationName;
            lblPackingMonth.Text = cp.PackingMonth;
            lblModel.Text = ModelName;

            //var panel = new System.Web.UI.WebControls.Panel();
            //panel.CssClass = "mainContent";
            //panel.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>{0} ({1} {2} - {3})</h2>", tableMeta.Caption, ModelName, VariantName, cp.PackingMonth)));
            //masterPage.MainContent.Controls.Add(panel);

            //populate combobox
            //GetDDLAssemblySections();
            //GetDDLOrderingOptions();

            Session["CpId"] = cp.Id;

            //cmbProductionLines.DataSource = sdsProductionLineLookup;
            //sdsProductionLineLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
            //sdsProductionLineLookup.DataBind();

            //cmbToolInventories.DataSource = sdsToolInventoryLookup;
            gvToolAssign.DataSource = sdsToolAssignment;

            return true; 
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (masterGrid != null)
            {
                //masterGrid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                //masterGrid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                //masterGrid.Settings.VerticalScrollableHeight = 600;

                masterGrid.Templates.PreviewRow = new TemplatePreviewRow();

                for (int i = 0; i < masterGrid.Columns.Count; i++)
                {
                    GridViewDataColumn col = masterGrid.Columns[i] as GridViewDataColumn;
                    if (col == null) continue;

                    if (col.Name == "VDoc" || col.Name == "SecondHand" || col.Name == "DS" || col.Name == "CS2" || col.Name == "CS3"
                        || col.Name == "JCStamp" || col.Name == "JCBarcode" || col.Name == "FBS" || col.Name == "DRT")
                    {
                        GridViewDataCheckColumn checkCol = col as GridViewDataCheckColumn;
                        if (checkCol != null)
                        {
                            checkCol.PropertiesCheckEdit.DisplayTextChecked = "X";
                            checkCol.PropertiesCheckEdit.DisplayTextUnchecked = "";
                            checkCol.PropertiesCheckEdit.DisplayTextGrayed = "";
                            checkCol.PropertiesCheckEdit.UseDisplayImages = false;
                            checkCol.CellStyle.Font.Bold = true;
                            checkCol.CellStyle.Font.Size = new FontUnit(FontSize.Medium);
                            checkCol.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (col.Name == "FBS" || col.Name == "DRT")
                    {
                        col.DataItemTemplate = new TemplateCpStationDetailDataItem(this, col.Name);
                    }
                    else if (col.Name == "II" || col.Name == "WI")
                    {
                        col.DataItemTemplate = new TemplateCpStationDetailDataItem(this, col.Name);

                        col.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                        col.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        col.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                        col.FilterCellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                        col.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

                        //< Settings AllowGroup = "False" />
                        // < EditFormSettings Visible = "False" />
                        //  < FilterCellStyle Wrap = "True" >
                        //   </ FilterCellStyle >
                        //   < HeaderStyle Wrap = "True" ></ HeaderStyle >
                     }
                }
            }

            return true;
        }

        protected void GetDDLStation(string AssemblySectionId)
        {
            DataTable dt = SqlHelper.ExecuteSqlCommand("select Id, StationName from dbo.Stations where AssemblySectionId=" + AssemblySectionId);
            if (dt == null) return;

            try
            {
                cboNewStation.DataSource = dt;
                //cboNewStation.Items.Add("-Select-", 0);
                cboNewStation.TextField = "StationName";
                cboNewStation.ValueField = "Id";
                //cboNewStation.Value = "-Select-";
                cboNewStation.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void GetDDLAssemblySections()
        {
            DataTable dt = SqlHelper.ExecuteSqlCommand("select Id, AssemblySectionName from dbo.AssemblySections");
            if (dt == null) return;

            //dt.Rows.Add(new object[] { 0, "-No Change-" });
            //dt.DefaultView.Sort = "AssemblySectionName ASC";

            try
            {
                cmbAsemblySection.DataSource = dt;
                cmbAsemblySection.TextField = "AssemblySectionName";
                cmbAsemblySection.ValueField = "Id";
                //cmbAsemblySection.Items.Add("-No Change-", 0);
                //cmbAsemblySection.Value = "-No Change-";
                cmbAsemblySection.DataBind();

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GetDDLOrderingOptions()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");

            dt.Rows.Add(new object[] { 0, "-Default (By CGISNo)-" });
            dt.Rows.Add(new object[] { 1, "First Process" });
            dt.Rows.Add(new object[] { 2, "Last Process" });
            dt.Rows.Add(new object[] { 9, "After Specified Process" });

            try
            {
                cmbOrderingOptions.DataSource = dt;
                cmbOrderingOptions.ValueField = "Value";
                cmbOrderingOptions.TextField = "Text";
                cmbOrderingOptions.Value = "-Default (By CGISNo)-";
                cmbOrderingOptions.DataBind();

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GetDDLStationProcesses(int ControlPlanId, int StationId)
        {
            DataTable dt = SqlHelper.ExecuteSqlCommand("select min(Id) as Id, concat(CgisNo, ' ', min(ProcessName)) as ProcessDescription, min(OrderNoInStation) as OrderNoInStation from dbo.ControlPlanProcesses where ControlPlanId=" + ControlPlanId + " and StationId=" + StationId + " group by CgisNo");
            if (dt == null) return;

            dt.DefaultView.Sort = "OrderNoInStation ASC";

            using (AppDb ctx = new AppDb())
            {
                cmbStationProcesses.DataSource = dt;
                cmbStationProcesses.ValueField = "Id";
                cmbStationProcesses.TextField = "ProcessDescription";
                cmbStationProcesses.DataBind();
            }

            //TODO

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Value");
            //dt.Columns.Add("Text");

            //dt.Rows.Add(new object[] { 0, "-Select-" });
            //dt.Rows.Add(new object[] { 1, "First Process" });
            //dt.Rows.Add(new object[] { 2, "Last Process" });
            //dt.Rows.Add(new object[] { 9, "After Specified Process" });

            //try
            //{
            //    cmbOrderingOptions.ValueField = "Value";
            //    cmbOrderingOptions.TextField = "Text";
            //    cmbOrderingOptions.DataBind();

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        protected void popupMoveStation_OnWindowCallback(object source, PopupWindowCallbackArgs e)
        {
            string[] arr = e.Parameter.Split(';');
            if (arr.Length != 2) return;

            if (arr[0] == "Move")
            {
                int cppId = arr[1].ToInt32(0);

                int cpId = Session["CpId"].ToInt32(0);

                ControlPlanProcess cpp = GetControlPlanProcess(cpId, cppId);
                if (cpp != null)
                {
                    Station station = GetStation(cpp.StationId);
                    AssemblySection section = null;
                    if (station != null)
                        section = GetAssemblySection(station.AssemblySectionId);

                    //lblOriginalStation.Text = 
                    //    StationRepository.RetrieveStationNameById(cpp.StationId.GetValueOrDefault());
                    //oldLabelAssemblySection.Text = StationRepository.RetrieveAsemblySectionByStId(cpp.StationId.GetValueOrDefault());
                    lblOriginalStation.Text = station != null ? station.StationName : "";
                    oldLabelAssemblySection.Text = section != null ? section.AssemblySectionName : "";
                    Session["OriCppId"] = cppId;

                    if (section != null)
                        GetDDLStation(section.Id.ToString());
                    cmbAsemblySection.Value = oldLabelAssemblySection.Text;
                    cboNewStation.Value = lblOriginalStation.Text;

                    if (station != null)
                        GetDDLStationProcesses(cpId, station.Id);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) // save Move Station
        {
            try
            {
                lblErrorLog.Visible = false;

                int cpId = Session["CpId"].ToInt32(0);

                int cppId = Session["OriCppId"].ToInt32(0);
                //cpp = ControlPlanRepository.RetrieveControlPlanProcessById(cppId);

                //if (cpp == null)
                //{
                //    lblErrorLog.Text = "No process selected.";
                //    lblErrorLog.ForeColor = Color.DarkRed;
                //    lblErrorLog.Visible = true;
                //    return;
                //}

                //if (cboMoveAllorPos.Value == null)
                //{
                //    lblErrorLog.Text = "Please select option Move.";
                //    lblErrorLog.ForeColor = Color.DarkRed;
                //    lblErrorLog.Visible = true;
                //    return;
                //}

                //if (cmbAsemblySection.Value == null)
                //{
                //    lblErrorLog.Text = "Please select New Section";
                //    lblErrorLog.ForeColor = Color.DarkRed;
                //    lblErrorLog.Visible = true;
                //    return;
                //}

                //if (cboNewStation.Value == null)
                //{
                //    lblErrorLog.Text = "Please select New Station.";
                //    lblErrorLog.ForeColor = Color.DarkRed;
                //    lblErrorLog.Visible = true;
                //    return;
                //}

                //if (cmbOrderingOptions.Value == null)
                //{
                //    lblErrorLog.Text = "Please select Position.";
                //    lblErrorLog.ForeColor = Color.DarkRed;
                //    lblErrorLog.Visible = true;
                //    return;
                //}

                if (cmbOrderingOptions.Value.ToInt32(0) == 9 && cmbStationProcesses.Value == null)
                {
                    lblErrorLog.Text = "Please select Position.";
                    lblErrorLog.ForeColor = Color.DarkRed;
                    lblErrorLog.Visible = true;
                    return;
                }

                bool isRfOnly = true;
                if (Convert.ToInt32(cboMoveAllorPos.Value) <= 0)
                {
                    isRfOnly = false;
                }

                int AfterCpProcessId = cmbOrderingOptions.Value.ToInt32(0);
                if (AfterCpProcessId == 1)
                {
                    AfterCpProcessId = 1;
                }
                else if(AfterCpProcessId == 2)
                {
                    AfterCpProcessId = -1;
                }
                else if (AfterCpProcessId == 9)
                {
                    AfterCpProcessId = cmbStationProcesses.Value.ToInt32(0);
                }

                ControlPlanRepository.MoveProcesses(cpId, cppId, cboNewStation.Value.ToInt32(0), AfterCpProcessId, this.User.UserName, isRfOnly);

                cmbAsemblySection.Value = "-No Change-";
                cboNewStation.Text = "";
                cmbOrderingOptions.Value = "-Default (By CGISNo)-";

                cpp = null;
                Session["OriCppId"] = 0;

                popupMoveStation.ShowOnPageLoad = false;

                masterGrid.DataBind();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        //protected void popupDeviation_OnWindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    try
        //    {
        //        string[] parameters = e.Parameter.Split(';');
        //        int id = Convert.ToInt32(parameters[0]);
        //        ControlPlanProcess cpp = ControlPlanRepository.RetrieveControlPlanProcessById(id);
        //        if (cpp != null)
        //        {
        //            memoDeviation.Text = cpp.Deviation;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }
        //}

        //#region Add to grid new Popup
        //protected void grvAdd_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        //{
        //    e.Editor.ReadOnly = false;
        //}
        //protected void grvAdd_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        //{
        //    CustomDataSource.Rows.Add(e.NewValues["Id"], e.NewValues["PartNumber"],
        //        e.NewValues["PartDescription"], e.NewValues["Dialog"], e.NewValues["Qty"], e.NewValues["Pos"], e.NewValues["TextJC"]);

        //    ASPxGridView grid = sender as ASPxGridView;
        //    grid.CancelEdit();
        //    e.Cancel = true;
        //}
        //protected void grvAdd_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        //{
        //    //CustomDataSource.Clear();
        //    int Id = CustomDataSource.Rows.Count;
        //    CustomDataSource.Rows.Add(e.NewValues["Id"] = Id, e.NewValues["PartNumber"],
        //        e.NewValues["PartDescription"], e.NewValues["Dialog"], e.NewValues["Qty"], e.NewValues["Pos"],
        //        e.NewValues["Class"], e.NewValues["VDoc"], e.NewValues["SecondHand"],
        //        e.NewValues["FBS"], e.NewValues["DS"], e.NewValues["CS2"]
        //        , e.NewValues["DRT"], e.NewValues["CodeRule"], e.NewValues["Torque"]
        //        , e.NewValues["ValidFrom"], e.NewValues["ValidTo"], e.NewValues["TextJC"]);

        //    ASPxGridView grid = sender as ASPxGridView;
        //    grid.CancelEdit();
        //    e.Cancel = true;
        //}
        //protected void grvAdd_DataBinding(object sender, EventArgs e)
        //{
        //    (sender as ASPxGridView).DataSource = CustomDataSource;
        //}
        //protected void grvAdd_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        //{
        //    ASPxGridView g = sender as ASPxGridView;

        //    int id = (int)e.Keys[g.KeyFieldName];
        //    DataRow dr = CustomDataSource.Rows.Find(id);
        //    CustomDataSource.Rows.Remove(dr);

        //    UpdateData(g);
        //    e.Cancel = true;
        //}
        //public DataTable CustomDataSource
        //{
        //    get
        //    {
        //        if (Session["CustomTable"] == null)
        //            Session["CustomTable"] = CreateDataSource();
        //        return (DataTable)Session["CustomTable"];
        //    }
        //    set
        //    {
        //        Session["CustomTable"] = value;
        //    }
        //}
        //private DataTable CreateDataSource()
        //{
        //    DataTable dataTable = new DataTable("DataTable");
        //    dataTable.Columns.Add("Id", typeof(Int32));
        //    dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };
        //    dataTable.Columns.Add("PartNumber", typeof(string));
        //    dataTable.Columns.Add("PartDescription", typeof(string));
        //    dataTable.Columns.Add("Dialog", typeof(string));
        //    dataTable.Columns.Add("Qty", typeof(string));
        //    dataTable.Columns.Add("Pos", typeof(string));
        //    dataTable.Columns.Add("Class", typeof(string));
        //    dataTable.Columns.Add("VDoc", typeof(bool));
        //    dataTable.Columns.Add("SecondHand", typeof(bool));
        //    dataTable.Columns.Add("FBS", typeof(string));
        //    dataTable.Columns.Add("DS", typeof(bool));
        //    dataTable.Columns.Add("CS2", typeof(bool));
        //    dataTable.Columns.Add("DRT", typeof(string));
        //    dataTable.Columns.Add("CodeRule", typeof(string));
        //    dataTable.Columns.Add("Torque", typeof(string));
        //    dataTable.Columns.Add("ValidFrom", typeof(DateTime));
        //    dataTable.Columns.Add("ValidTo", typeof(DateTime));
        //    dataTable.Columns.Add("TextJC", typeof(string));

        //    for (int i = 0; i < dataTable.Rows.Count; i++)
        //    {
        //        dataTable.Rows.Add(i, "Id" + i.ToString());
        //        dataTable.Rows.Add(i, "PartNumber" + i.ToString());
        //        dataTable.Rows.Add(i, "PartDescription" + i.ToString());
        //        dataTable.Rows.Add(i, "Dialog" + i.ToString());
        //        dataTable.Rows.Add(i, "Qty" + i.ToString());
        //        dataTable.Rows.Add(i, "Pos" + i.ToString());
        //        dataTable.Rows.Add("Class" + i.ToString());
        //        dataTable.Rows.Add("VDoc" + i.ToString());
        //        dataTable.Rows.Add("SecondHand" + i.ToString());
        //        dataTable.Rows.Add("FBS" + i.ToString());
        //        dataTable.Rows.Add("DS" + i.ToString());
        //        dataTable.Rows.Add("CS2" + i.ToString());
        //        dataTable.Rows.Add("DRT" + i.ToString());
        //        dataTable.Rows.Add("CodeRule" + i.ToString());
        //        dataTable.Rows.Add("Torque" + i.ToString());
        //        dataTable.Rows.Add("ValidFrom" + i.ToString());
        //        dataTable.Rows.Add("ValidTo" + i.ToString());
        //        dataTable.Rows.Add("TextJC", typeof(string));
        //    }
        //    return dataTable;
        //}

        //private void UpdateData(ASPxGridView g)
        //{
        //    ViewState["CustomTable"] = dataTable;
        //    g.DataBind();
        //}

        //protected void btnNewProcessSave_Click(object sender, EventArgs e)
        //{
        //    if (ControlPlanRepository.RetrieveControlPlanByVpmModelVariant(cp.PackingMonth.ToInt32(0),
        //                cp.ModelId, cp.VariantId) != null)
        //    {
        //        if (CustomDataSource.Rows.Count != 0)
        //        {
        //            foreach (DataRow item in CustomDataSource.Rows)
        //            {
        //                string strDevTepl = "[" + DateTime.Now + "] New Process/Part added by user [{0}]";

        //                ControlPlanProcess cpProcessNew = new ControlPlanProcess();

        //                cpProcessNew.ControlPlanId = ControlPlanRepository.RetrieveControlPlanByVpmModelVariant(cp.PackingMonth.ToInt32(0),
        //                   cp.ModelId, cp.VariantId).Id;
        //                cpProcessNew.ProcessName = txtProcessName.Text;
        //                cpProcessNew.StationId = Convert.ToInt32(cboStationAdd.Value);
        //                cpProcessNew.CgisNo = txtCgisNo.Text;
        //                cpProcessNew.OriCgisNo = txtCgisNo.Text;
        //                cpProcessNew.PartNumber = item["PartNumber"].ToString();
        //                cpProcessNew.PartDescription = item["PartDescription"].ToString();
        //                cpProcessNew.DialogAddress = item["Dialog"].ToString();
        //                cpProcessNew.Qty = Convert.ToInt32(item["Qty"]);
        //                cpProcessNew.Rf = Convert.ToInt32(item["Pos"]);
        //                cpProcessNew.HasDeviation = true;
        //                cpProcessNew.Deviation = string.Format(strDevTepl,
        //                    (User)Session["user"] == null ? "" : ((User)Session["user"]).FullName);
        //                cpProcessNew.Class = item["Class"].ToString();
        //                cpProcessNew.FBS = item["FBS"].ToString();
        //                cpProcessNew.DRT = item["DRT"].ToString();
        //                cpProcessNew.DS = item["DS"].ToBoolean(false); //Convert.ToBoolean(item["DS"] ?? "false");
        //                cpProcessNew.VDoc = item["Vdoc"].ToBoolean(false); //Convert.ToBoolean(item["VDoc"]);
        //                cpProcessNew.SecondHand = item["SecondHand"].ToBoolean(false); //Convert.ToBoolean(item["SecondHand"] ?? "false");
        //                cpProcessNew.CS2 = item["CS2"].ToBoolean(false); //Convert.ToBoolean(item["CS2"] ?? "false");
        //                cpProcessNew.CS3 = item["CS2"].ToBoolean(false); //Convert.ToBoolean(item["CS3"] ?? "false");
        //                cpProcessNew.CodeRule = item["CodeRule"].ToString().Equals("") ? ";" : item["CodeRule"].ToString();
        //                cpProcessNew.Torque = item["Torque"].ToString();
        //                cpProcessNew.ValidFrom = Convert.ToDateTime(item["ValidFrom"]);//new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        //                cpProcessNew.ValidTo = Convert.ToDateTime(item["ValidTo"]);//new DateTime(2099, 12, 31));
        //                cpProcessNew.TextJC = item["TextJC"].ToString();

        //                ControlPlanRepository.SaveNewControlPlanProcess(cpProcessNew);
        //            }

        //            #region add document
        //            //ControlPlanAttachment cpaNew = new ControlPlanAttachment();
        //            //if (uplPopupAdd.UploadedFiles.Length > 0)
        //            //{
        //            //    if (uplPopupAdd.UploadedFiles[0].ContentLength > 0)
        //            //    {
        //            //        cpaNew.DocTypeId = DocTypeRepository.RetrieveDocTypeIdByDescription("CPWI");
        //            //        cpaNew.ProcessNo = txtCgisNo.Text;
        //            //        cpaNew.ModelId = Convert.ToInt32(hfData["ModelId"]);
        //            //        cpaNew.Title = txtTitle.Text;
        //            //        cpaNew.ValidFrom = Convert.ToDateTime(txtValidFrom.Value);
        //            //        cpaNew.ValidTo = Convert.ToDateTime(txtValidTo.Value);
        //            //        cpaNew.CreatedDate = DateTime.Now;
        //            //        cpaNew.CreatedBy = (User)Session["user"] == null ? "" : ((User)Session["user"]).FullName;
        //            //        cpaNew.ModifiedDate = DateTime.Now;

        //            //        SavePostedFile(uplPopupAdd.UploadedFiles[0]);
        //            //        cpaNew.FileLocation = Session["FileLocation"].ToString();
        //            //        cpaNew.FileType = Session["FileType"].ToString();

        //            //        cpaNew.CreatedBy = Session["user"] == null ? "" : ((User)Session["user"]).UserName;
        //            //        cpaNew.CreatedDate = DateTime.Now;
        //            //        cpaNew.ModifiedDate = DateTime.Now;

        //            //        ControlPlanRepository.SaveNewDocument(cpaNew);
        //            //    }
        //            //}

        //            #endregion

        //            txtProcessName.Text = "";
        //            txtCgisNo.Text = "";
        //            //dtPkmNew.Text = "";
        //            //cboModelNew.Text = "";
        //            //cboVariantNew.Text = "";
        //            cboStationAdd.Text = "";
        //            lblError.Text = "";
        //            CustomDataSource.Clear();
        //            Session["CustomTable"] = null;
        //            grvAdd.DataBind();

        //            popupnewButton.ShowOnPageLoad = false;
        //        }
        //        else
        //        {
        //            lblError.Text = "Please create new item first !!!";
        //            lblError.ForeColor = Color.Red;
        //        }
        //    }
        //    else
        //    {
        //        lblError.Text = "Packing Month = " + cp.PackingMonth + ", Model = " + cp.ModelId + " and Variant = " + cp.VariantId + " does not exist in Control Plan !!!";
        //        lblError.ForeColor = Color.Red;
        //    }
        //}

        //#endregion

        //#region WI/II
        //protected void grvDocControlManagementWI_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        //{
        //    e.NewValues["DocTypeId"] = DocTypeRepository.RetrieveDocTypeIdByDescription("CPWI");
        //    e.NewValues["Title"] = Session["FileName"].ToString();
        //    e.NewValues["RunningNumber"] = null;
        //    e.NewValues["FileLocation"] = Session["FileLocation"].ToString();
        //    e.NewValues["FileType"] = Session["FileType"].ToString();
        //    //e.NewValues["ModelId"] = 0;
        //    e.NewValues["CreatedDate"] = DateTime.Now;
        //    e.NewValues["CreatedBy"] = Session["user"] == null ? "" : ((User)Session["user"]).UserName;
        //    e.NewValues["ModifiedDate"] = DateTime.Now;
        //    e.NewValues["ModifiedBy"] = Session["user"] == null ? "" : ((User)Session["user"]).UserName;

        //    e.Cancel = false;
        //}
        //protected void grvDocControlManagementWI_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        //{
        //    int docTypeId = DocTypeRepository.RetrieveDocTypeIdByDescription("CPWI");
        //    e.NewValues["DocTypeId"] = docTypeId;
        //    e.NewValues["Title"] = Session["FileName"].ToString();
        //    e.NewValues["RunningNumber"] = ControlPlanRepository.CekRunningNumber(docTypeId);
        //    e.NewValues["FileLocation"] = Session["FileLocation"].ToString();
        //    e.NewValues["FileType"] = Session["FileType"].ToString();
        //    //e.NewValues["ModelId"] = 28;
        //    e.NewValues["CreatedDate"] = DateTime.Now;
        //    e.NewValues["CreatedBy"] = Session["user"] == null ? "" : ((User)Session["user"]).UserName;
        //    e.NewValues["ModifiedDate"] = DateTime.Now;
        //    e.NewValues["ModifiedBy"] = Session["user"] == null ? "" : ((User)Session["user"]).UserName;

        //    e.Cancel = false;
        //}

        //protected void grvDocControlManagementWI_Init(object sender, EventArgs e)
        //{
        //    grvDocControlManagementWI.DataSourceID = null;
        //    grvDocControlManagementWI.DataSource = sdsDocControlWI;
        //    grvDocControlManagementWI.DataBind();
        //}

        //protected void ucTaskAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        //{
        //    if (!e.IsValid) return;

        //    SavePostedFile(e.UploadedFile);
        //    Session["FileName"] = e.UploadedFile.FileName;

        //    e.CallbackData = e.UploadedFile.FileName;
        //}

        //private string SavePostedFile(UploadedFile eUploadedFile)
        //{
        //    String resFileName = "";
        //    try
        //    {
        //        if (!eUploadedFile.IsValid) return String.Empty;

        //        //=========cek folder Packing Month
        //        string path = ConfigurationHelper.UPLOAD_DIR + "/" + Session["PackingMonth"];
        //        if (!Directory.Exists(Server.MapPath(path)))
        //        {
        //            Directory.CreateDirectory(Server.MapPath(path));
        //        }

        //        //=========cek folder Model
        //        string pathModel = path + "/" + Session["ModelName"];
        //        if (!Directory.Exists(Server.MapPath(pathModel)))
        //        {
        //            Directory.CreateDirectory(Server.MapPath(pathModel));
        //        }

        //        //=========cek folder Varian
        //        string pathVarian = pathModel + "/" + Session["VariantName"];
        //        if (!Directory.Exists(Server.MapPath(pathVarian)))
        //        {
        //            Directory.CreateDirectory(Server.MapPath(pathVarian));
        //        }

        //        String UploadDir = pathVarian + "/"; 


        //        //FileInfo fileInfo = new FileInfo(eUploadedFile.FileName);
        //        String fileNameOri = eUploadedFile.FileName.ToString().Replace(" ", "_");
        //        String ext = Path.GetExtension(eUploadedFile.FileName);
        //        String fileType = eUploadedFile.ContentType.ToString();

        //        String fileName = fileNameOri;

        //        //String resFileName = "";
        //        if (!File.Exists(UploadDir + fileName))
        //        {
        //            resFileName = Server.MapPath(UploadDir + fileName);
        //            eUploadedFile.SaveAs(resFileName);
        //        }

        //        Session["FileLocation"] = resFileName;
        //        Session["FileType"] = fileType;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }

        //    return resFileName;
        //}

        //#endregion

        public void btnDownloadWI_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    int id = Convert.ToInt32(btn.CommandArgument);
            //    ControlPlanAttachment type = ControlPlanRepository.RetriveControlPlanAttachmentId(id);

            //    string filepath = HttpContext.Current.Server.MapPath(type.FileLocation);
            //    if (!File.Exists(filepath))
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "alert('File not found');", true);
            //        return;
            //    }

            //    if (type.FileType == "application/pdf")
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "window.open('/custom/CommonWorks/CustomDocumentViewer.aspx?Id=" + id + "&docType=CPWI" + "','_newtab');",
            //        true);
            //    }
            //    else
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "window.open('/custom/DownloadFile.aspx?type=CPWI&Id=" + id + "','_self');",
            //        true);
            //        //Response.Redirect("./custom/DownloadFile.aspx?type=CPWI" + "&Id=" + id);
            //        //Response.Redirect("../DownloadFile.aspx?type=CPWI&Id=" + id);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}

        }

        public void btnDownloadII_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    int id = Convert.ToInt32(btn.CommandArgument);
            //    ControlPlanAttachmentII type = ControlPlanRepository.RetriveControlPlanIIAttachmentbyId(id);

            //    string filepath = HttpContext.Current.Server.MapPath(type.FileLocation);
            //    if (!File.Exists(filepath))
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "alert('File not found');", true);
            //        return;
            //    }

            //    if (type.FileType == "application/pdf")
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "window.open('/custom/CommonWorks/CustomDocumentViewer.aspx?Id=" + id + "&docType=CPII" + "','_newtab');",
            //        true);
            //    }
            //    else
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
            //        "window.open('/custom/DownloadFile.aspx?type=CPII&Id=" + id + "','_self');",
            //        true);
            //        //Response.Redirect("./custom/DownloadFile.aspx?type=CPWI" + "&Id=" + id);
            //        //Response.Redirect("../DownloadFile.aspx?type=CPWI&Id=" + id);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}

        }

        protected void cmbStationProcesses_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] arr = e.Parameter.Split(";");
            if (arr.Length != 2) return;

            int stationId = arr[1].ToInt32(0);

            GetDDLStationProcesses(cp.Id, stationId);
        }

        protected void cmbStations_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] arr = e.Parameter.Split(";");
            if (arr.Length != 2) return;

            int sectionId = arr[1].ToInt32(0);

            GetDDLStation(sectionId.ToString());
        }

        public ControlPlanProcess GetControlPlanProcess(int cpId, int cppId)
        {
            using (AppDb context = new AppDb())
            {
                return context.ControlPlanProcesses.Where(p => p.Id == cppId && p.ControlPlanId == cpId).FirstOrDefault();
            }
        }

        public Station GetStation(int stationId)
        {
            using (AppDb context = new AppDb())
            {
                return context.Stations.Where(p => p.Id == stationId).FirstOrDefault();
            }
        }

        public AssemblySection GetAssemblySection(int sectionId)
        {
            using (AppDb context = new AppDb())
            {
                return context.AssemblySections.Where(p => p.Id == sectionId).FirstOrDefault();
            }
        }

        protected void popupMoveStation_Load(object sender, EventArgs e)
        {
            cmbStationProcesses.SelectedItem = null;
            if (cmbOrderingOptions.Value.ToInt32(0) == 9)
            {
                cmbStationProcesses.ClientEnabled = true;
            }
            else
            {
                cmbStationProcesses.ClientEnabled = false;
            }
        }

        protected void btnAssignTool_Click(object sender, EventArgs e)
        {
            int cppId = Session["OriCppId"].ToInt32(0);
            int prodLineId = cmbProductionLines.Value.ToInt32(0);
            //int toolInvId = cmbToolInventories.Value.ToInt32(0);
            if (cppId == 0 || prodLineId == 0)
                return;

            ToolRepository.UnassignToolProcess(prodLineId, cppId, User.UserName);

            GridViewDataTextColumn col = gvToolAssign.Columns[2] as GridViewDataTextColumn;
            for (int i = 0; i < gvToolAssign.VisibleRowCount; i++)
            {
                ASPxTextBox txtSequence = gvToolAssign.FindRowCellTemplateControl(i, col, "Sequence") as ASPxTextBox;
                int sequence = txtSequence.Text.ToInt32(0);
                int toolInvId = gvToolAssign.GetCurrentPageRowValues("Id")[i].ToInt32(0);

                //((IPostBackDataHandler)txtSequence).LoadPostData("", Request.Form);
                if (toolInvId > 0 && sequence > 0)
                {
                    ToolRepository.AssignToolProcess(prodLineId, cppId, toolInvId, sequence, User.UserName);
                }
            }

            //bool status = ToolRepository.AssignToolProcess(prodLineId, cppId, toolInvId, User.UserName);

            //if (status)

            gvToolAssign.DataBind();

            popupToolAssign.ShowOnPageLoad = false;
        }

        protected void popupToolAssign_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    int id = Session["OriCppId"].ToInt32(0);
            //    ControlPlanProcess cpp = ControlPlanRepository.RetrieveControlPlanProcessById(id);
            //    if (cpp != null)
            //    {
            //        GetDDLProductionLines(cpp.StationId);

            //        GetDDLToolInventories(cmbProductionLines.Value.ToInt32(0), id, cpp.StationId, cpp.Class.Trim());

            //        Session["OriCppId"] = id;
            //        Session["StationId"] = cpp.StationId;
            //        Session["ToolClass"] = cpp.Class.Trim();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //    throw ex;
            //}

        }

        protected void popupToolAssign_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            try
            {
                string[] parameters = e.Parameter.Split(';');
                if (parameters.Length != 2 && parameters[0] != "Tool")
                    return;

                int id = Convert.ToInt32(parameters[1]);
                ControlPlanProcess cpp = ControlPlanRepository.RetrieveControlPlanProcessById(id);
                if (cpp != null)
                {
                    GetDDLProductionLines(cpp.StationId);

                    //GetDDLToolInventories(cmbProductionLines.Value.ToInt32(0), id, cpp.StationId, cpp.Class.Trim());

                    GetGVToolInventories(cmbProductionLines.Value.ToInt32(0), id, cpp.StationId, cpp.HSUE.Trim());

                    Session["OriCppId"] = id;
                    Session["StationId"] = cpp.StationId;
                    Session["ToolClass"] = cpp.HSUE.Trim();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw ex;
            }

        }

        protected void GetDDLProductionLines(int StationId)
        {
            //TODO

            ////get assembly section first
            //DataTable dt = SqlHelper.ExecuteSqlCommand("select a.AssemblySectionType from dbo.Stations s "
            //                                            + "inner join dbo.AssemblySection a on a.Id = s.AssemblySectionId "
            //                                            + "where s.Id=" + StationId);
            //if (dt == null || dt.Rows.Count == 0) return;

            //DataTable dt2 = null;
            //if (dt.Rows[0][0].ToInt32(0) == (int)EAssemblySectionType.MainLine)
            //{
            //    dt2 = SqlHelper.ExecuteSqlCommand("select Id, LineName from [dbo].[ProductionLines] pl where pl.AssemblyTypeId=" + assemblyTypeId + " and pl.LineNumber < " + ConfigurationHelper.PRODUCTION_SUBASSY_LINENUMBER);
            //}
            //else if (dt.Rows[0][0].ToInt32(0) == (int)EAssemblySectionType.SubAssemblyLine)
            //{
            //    dt2 = SqlHelper.ExecuteSqlCommand("select Id, LineName from [dbo].[ProductionLines] pl where pl.AssemblyTypeId=" + assemblyTypeId + " and pl.LineNumber = " + ConfigurationHelper.PRODUCTION_SUBASSY_LINENUMBER);
            //}
            //else if (dt.Rows[0][0].ToInt32(0) == (int)EAssemblySectionType.EndOfLine)
            //{
            //    dt2 = SqlHelper.ExecuteSqlCommand("select Id, LineName from [dbo].[ProductionLines] pl where pl.AssemblyTypeId=" + assemblyTypeId + " and pl.LineNumber = " + ConfigurationHelper.PRODUCTION_ENDOFLINE_LINENUMBER);
            //}
            //else if (dt.Rows[0][0].ToInt32(0) == (int)EAssemblySectionType.FinishLine)
            //{
            //    dt2 = SqlHelper.ExecuteSqlCommand("select Id, LineName from [dbo].[ProductionLines] pl where pl.AssemblyTypeId=" + assemblyTypeId + " and pl.LineNumber = " + ConfigurationHelper.PRODUCTION_FINISHLINE_LINENUMBER);
            //}
            //if (dt2 == null || dt2.Rows.Count == 0) return;

            //try
            //{
            //    cmbProductionLines.DataSource = dt2;
            //    cmbProductionLines.TextField = "LineName";
            //    cmbProductionLines.ValueField = "Id";
            //    cmbProductionLines.DataBind();

            //    //set default to first entry
            //    if (cmbProductionLines.Items.Count > 0)
            //    {
            //        cmbProductionLines.SelectedIndex = 0;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        protected void gvToolAssign_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int prodLineId = e.Parameters.ToInt32(0);
            int cppId = Session["OriCppId"].ToInt32(0);
            int stationId = Session["StationId"].ToInt32(0);
            string toolClass = Session["ToolClass"] == null ? "" : Session["ToolClass"].ToString();

            GetGVToolInventories(prodLineId, cppId, stationId, toolClass);
        }

        protected void GetGVToolInventories(int ProdLineId, int CpProcessId, int StationId, string Class)
        {
            SqlDataSource ds = gvToolAssign.DataSource as SqlDataSource;
            ds.SelectParameters["CpProcessId"].DefaultValue = CpProcessId.ToString();
            ds.SelectParameters["ProdLineId"].DefaultValue = ProdLineId.ToString();
            ds.SelectParameters["StationId"].DefaultValue = StationId.ToString();
            ds.SelectParameters["ToolClass"].DefaultValue = Class;

            gvToolAssign.DataBind();

            ////set default to first entry
            //if (cmbToolInventories.Items.Count > 0)
            //{
            //    cmbToolInventories.SelectedIndex = 0;
            //}

            //int toolInvId = ToolRepository.GetCurrentAssignedTool(ProdLineId, StationId, CpProcessId);
            //if (toolInvId == 0)
            //{
            //    cmbToolInventories.SelectedItem = null;
            //    return;
            //}

            ////cmbToolInventories.SelectedItem = null;
            ////cmbToolInventories.Value = toolInvId;
            //ListEditItem item = cmbToolInventories.Items.FindByValue(toolInvId.ToString());
            //cmbToolInventories.SelectedItem = item;
        }

        protected void popupCheckingCode_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            try
            {
                string[] parameters = e.Parameter.Split(';');
                int id = Convert.ToInt32(parameters[0]);

                ControlPlanProcess cpp = ControlPlanRepository.RetrieveControlPlanProcessById(id);
                string[] checkingCodeId = cpp.CheckingCode.ToString().Split(';');

                Session["CheckingCodeId"] = id;

                foreach (string item in checkingCodeId)
                {
                    for (int i = 0; i < chkListCodes.Items.Count; i++)
                    {
                        if (item.Trim().Equals(chkListCodes.Items[i].Value.ToString()))
                        {
                            chkListCodes.Items[i].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnSaveCheckingCode_Click(object sender, EventArgs e)
        {
            try
            {
                int cppId = Convert.ToInt32(Session["CheckingCodeId"]);

                if (chkListCodes.SelectedValues.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var value in chkListCodes.SelectedValues)
                    {
                        sb.Append(value + "; ");
                    }

                    ControlPlanRepository.UpdateCheckingCode(sb.ToString(), cppId);
                }

                chkListCodes.UnselectAll();
                popupCheckingCode.ShowOnPageLoad = false;

                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void gvToolAssign_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Number")
            {
                e.Value = string.Format("{0}", e.ListSourceRowIndex + 1);
            }
        }

    }

    public class TemplateCpStationDetailDataItem : ITemplate
    {
        string name;
        PageBase parent;

        public TemplateCpStationDetailDataItem(PageBase Parent, string Name)
        {
            this.parent = Parent;
            this.name = Name;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            GridViewDataItemTemplateContainer template = container as GridViewDataItemTemplateContainer;
            ASPxGridView grid = template.Grid;

            if (name == "FBS")
            {
                ASPxLabel lbl = new ASPxLabel();
                lbl.ID = "lblFBS";
                lbl.Font.Bold = true;
                lbl.Font.Size = new FontUnit(FontSize.Medium);

                container.Controls.Add(lbl);
            }
            else if (name == "DRT")
            {
                ASPxLabel lbl = new ASPxLabel();
                lbl.ID = "lblDRT";
                lbl.Font.Bold = true;
                lbl.Font.Size = new FontUnit(FontSize.Medium);

                container.Controls.Add(lbl);
            }
            else if (name == "WI")
            {
                ASPxGridView grd = new ASPxGridView();
                grd.ID = "grvWIDocs";
                grd.KeyFieldName = "Id";
                grd.CssClass = "innerGrid";
                grd.Settings.ShowColumnHeaders = false;
                grd.Settings.GridLines = GridLines.None;

                GridViewDataColumn col = new GridViewDataColumn();
                col.FieldName = "Title";
                col.DataItemTemplate = new TemplateCpStationDetailDataItem(parent, "WIDoc");
                grd.Columns.Add(col);

                col = new GridViewDataColumn();
                col.FieldName = "RunningNumber";
                col.VisibleIndex = 0;
                col.Visible = false;
                grd.Columns.Add(col);

                grd.SettingsPager.Visible = false;
                grd.SettingsDataSecurity.AllowDelete = false;
                grd.SettingsDataSecurity.AllowEdit = false;
                grd.SettingsDataSecurity.AllowInsert = false;

                container.Controls.Add(grd);

                //< dx:ASPxGridView ID = "grvWIDocs" runat = "server" AutoGenerateColumns = "False" KeyFieldName = "Id" CssClass = "innerGrid" >
                //             < Settings ShowColumnHeaders = "False" GridLines = "None" ></ Settings >
                //                < Columns >
                //                    < dx:GridViewDataTextColumn VisibleIndex = "0" FieldName = "Title" >
                //                           < DataItemTemplate >
                //                               < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadWI" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadWI_OnClick" >
                //                </ dx:ASPxButton >
                //             </ DataItemTemplate >
                //         </ dx:GridViewDataTextColumn >
                //      </ Columns >
                //      < SettingsPager Visible = "False" >
                //       </ SettingsPager >
                //       < SettingsDataSecurity AllowDelete = "False" AllowEdit = "False" AllowInsert = "False" />
                //        </ dx:ASPxGridView >

            }
            else if (name == "II")
            {
                ASPxGridView grd = new ASPxGridView();
                grd.ID = "grvIIDocs";
                grd.KeyFieldName = "Id";
                grd.CssClass = "innerGrid";
                grd.Settings.ShowColumnHeaders = false;
                grd.Settings.GridLines = GridLines.None;

                GridViewDataColumn col = new GridViewDataColumn();
                col.FieldName = "Title";
                col.VisibleIndex = 0;
                col.DataItemTemplate = new TemplateCpStationDetailDataItem(parent, "IIDoc");
                grd.Columns.Add(col);

                col = new GridViewDataColumn();
                col.FieldName = "RunningNumber";
                col.VisibleIndex = 0;
                col.Visible = false;
                grd.Columns.Add(col);

                grd.SettingsPager.Visible = false;
                grd.SettingsDataSecurity.AllowDelete = false;
                grd.SettingsDataSecurity.AllowEdit = false;
                grd.SettingsDataSecurity.AllowInsert = false;

                container.Controls.Add(grd);

                //< dx:ASPxGridView ID = "grvIIDocs" runat = "server" AutoGenerateColumns = "False" CssClass = "innerGrid" >
                //           < Settings ShowColumnHeaders = "False" GridLines = "None" ></ Settings >
                //              < Columns >
                //                  < dx:GridViewDataTextColumn VisibleIndex = "0" >
                //                       < DataItemTemplate >
                //                           < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadII" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadII_OnClick" >
                //                </ dx:ASPxButton >
                //             </ DataItemC:\Workspaces\MBINA-LPS\Dotsystem Scaffold\NewTIDS\appmercy\DotMercy\DotWeb\UI\Template >
                //         </ dx:GridViewDataTextColumn >
                //      </ Columns >
                //      < SettingsPager Visible = "False" >
                //       </ SettingsPager >
                //       < SettingsDataSecurity AllowDelete = "False" AllowEdit = "False" AllowInsert = "False" />
                //        </ dx:ASPxGridView >
            }
            else if (name == "WIDoc")
            {
                //string text = HttpUtility.HtmlDecode(template.Text).Trim();
                string number = grid.GetRowValues(template.VisibleIndex, "DocID").ToString();
                int id = grid.GetRowValues(template.VisibleIndex, "Id").ToInt32(0);
                string title = grid.GetRowValues(template.VisibleIndex, "Title").ToString();

                ASPxButton btn = new ASPxButton();
                btn.ID = "btnDownloadWI";
                btn.RenderMode = ButtonRenderMode.Link;
                btn.Text = number;
                btn.CommandArgument = id.ToString();
                btn.ToolTip = title;
                //btn.Click += btnDownloadWI_OnClick;

                CPStationDetail page = parent as CPStationDetail;
                if (page != null)
                    btn.Click += page.btnDownloadWI_OnClick;

                container.Controls.Add(btn);

                //                               < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadWI" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadWI_OnClick" >
                //                </ dx:ASPxButton >

            }
            else if (name == "IIDoc")
            {
                //string text = HttpUtility.HtmlDecode(template.Text).Trim();
                string number = grid.GetRowValues(template.VisibleIndex, "DocID").ToString();
                int id = grid.GetRowValues(template.VisibleIndex, "Id").ToInt32(0);
                string title = grid.GetRowValues(template.VisibleIndex, "Title").ToString();

                ASPxButton btn = new ASPxButton();
                btn.ID = "btnDownloadII";
                btn.RenderMode = ButtonRenderMode.Link;
                btn.Text = number;
                btn.CommandArgument = id.ToString();
                btn.ToolTip = title;
                //btn.Click += btnDownloadII_OnClick;

                CPStationDetail page = parent as CPStationDetail;
                if (page != null)
                    btn.Click += page.btnDownloadII_OnClick;

                container.Controls.Add(btn);

                //                           < dx:ASPxButton runat = "server" RenderMode = "Link" Text = '<%#Eval("RunningNumber") %>'
                //                    CommandArgument = '<%#Eval("Id") %>' ID = "btnDownloadII" ToolTip = '<%#Eval("Title") %>'
                //                    OnClick = "btnDownloadII_OnClick" >
                //                </ dx:ASPxButton >
            }
        }

        //private void btnDownloadWI_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxButton btn = (sender as ASPxButton);
        //        int id = Convert.ToInt32(btn.CommandArgument);
        //        ControlPlanAttachment type = ControlPlanRepository.RetriveControlPlanAttachmentId(id);
        //        if (type.FileType == "application/pdf")
        //        {
        //            parent.Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
        //            "window.open('../CommonWorks/CustomDocumentViewer.aspx?Id=" + id + "&docType=CPWI" + "','_newtab');",
        //            true);
        //        }
        //        else
        //        {
        //            //Response.Redirect("./custom/DownloadFile.aspx?type=CPWI" + "&Id=" + id);
        //            parent.Response.Redirect("../DownloadFile.aspx?type=CPWI&Id=" + id);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }
        //}

        //private void btnDownloadII_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxButton btn = (sender as ASPxButton);
        //        int id = Convert.ToInt32(btn.CommandArgument);
        //        ControlPlanAttachmentII type = ControlPlanRepository.RetriveControlPlanIIAttachmentbyId(id);
        //        if (type.FileType == "application/pdf")
        //        {
        //            parent.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" + DateTime.Now.ToString("yyyyMMddhhmmss"),
        //            "window.open('../CommonWorks/CustomDocumentViewer.aspx?Id=" + id + "&docType=CPII" + "','_newtab');",
        //            true);
        //        }
        //        else
        //        {
        //            //Response.Redirect("./custom/DownloadFile.aspx?type=CPWI" + "&Id=" + id);
        //            parent.Response.Redirect("../DownloadFile.aspx?type=CPII&Id=" + id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogError(ex);
        //    }
        //}
    }

    class TemplatePreviewRow : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            GridViewPreviewRowTemplateContainer con = container as GridViewPreviewRowTemplateContainer;

            string txt = DataBinder.Eval(con.DataItem, "Torque").ToString();
            if (String.IsNullOrWhiteSpace(txt))
                return;

            ASPxLabel lbl = new ASPxLabel();
            lbl.ID = "lblPreview";
            lbl.Text = txt;
            con.Controls.Add(lbl);

            con.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp"));

            ASPxImage img = new ASPxImage();
            img.ID = "imgTools";
            img.ImageUrl = "/Content/Images/glyphicons-375-claw-hammer.png";
            img.Width = new Unit("16px");
            con.Controls.Add(img);

            con.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp"));

            ASPxLabel lbl2 = new ASPxLabel();
            lbl2.ID = "lblAssignTools";
            con.Controls.Add(lbl2);

            //string dialogAddr = DataBinder.Eval(con.DataItem, "DialogAddress").ToString();
            //string partNo = DataBinder.Eval(con.DataItem, "PartNumber").ToString();
            //string cgisNo = DataBinder.Eval(con.DataItem, "CgisNo").ToString();

            //string toolDesc = ToolRepository.GetAssignedToolToControlPlanProcess(dialogAddr, partNo, cgisNo);
            //if (toolDesc == "")
            //{
            //    img.Visible = false;
            //    lbl2.Visible = false;
            //}
            //else
            //{
            //    lbl2.Text = toolDesc;
            //}
        }
    }

}