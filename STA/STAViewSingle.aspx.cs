using DevExpress.Web;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Scaffolding.Utils;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.STA
{
    public partial class STAViewSingle : ListPageBase
    {
        //Default table name
        protected string tableName = "STASummary";

        RIC ric = null;

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            //string str = txtReasonOfAlteration.Text;

            //Dynamic permission
            //TODO

            ////determine if we should use QM/LOG/ENG configuration
            //string org = ControlPlanRepository.GetUserOrganization(assemblyTypeId, user.UserId).ToUpper();
            //if (org == "ENG")
            //{
            //    //engineering > edit mode
            //    tableName = "STAViewSingleENG";
            //}
            //else
            //{
            //    tableName = "STAViewSingle";
            //}

            ////override if mode is specified 
            //if (Request.QueryString["mode"] != null)
            //{
            //    int mode = Request.QueryString["mode"].ToInt32(0);
            //    if (mode == 2)
            //    {
            //        //engineering > edit mode
            //        tableName = "STAViewSingleENG";
            //    }
            //    else
            //    {
            //        tableName = "STAViewSingle";
            //    }
            //}

            //since we determine the table dynamically, check for permission explicitly
            permissions = PermissionHelper.GetTablePermissions(tableName, user.UserId, assemblyTypeId);
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

            //header info
            int vpm = Request.QueryString["vpm"].ToInt32(0);
            int modelId = Request.QueryString["modelid"].ToInt32(0);
            int rictype = Request.QueryString["rictype"].ToInt32(0);
            int ricId = Request.QueryString["id"].ToInt32(0);

            using (AppDb ctx = new AppDb())
            {
                if (ricId > 0)
                {
                    ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                }
                else
                {
                    ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                }
            }

            if (ric == null)
            {
                var panel1 = new System.Web.UI.WebControls.Panel();
                panel1.CssClass = "mainContent";
                panel1.Controls.Clear();
                panel1.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>Invalid Model/Variant/PackingMonth for RIC</h2>")));

                masterPage.MainContent.Controls.Add(panel1);
                masterPage.PageTitle.Controls.Add(new LiteralControl(tableMeta.Caption));

                divHeader.Visible = false;

                return false;
            }

            //set master key
            SetMasterKey("RICId", ric.Id);

            //link direct to show report sta
            hlPrint.NavigateUrl = "~/Reports/ReportSTA.aspx?Id=" + ric.Id;

            //set datasources
            SqlDataSource sds = new SqlDataSource();
            sds.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            sds.SelectCommand = "SELECT uo.UserId, uo.FullName FROM vUserOrganizations uo where (uo.OrganizationId = @organizationId or uo.UserId = @userId) ORDER BY FullName";
            sds.SelectParameters.Add("organizationId", "");
            sds.SelectParameters.Add("userId", "");
            cmbApprovalOne.DataSourceID = "";
            cmbApprovalOne.DataSource = sds;

            sds = new SqlDataSource();
            sds.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            sds.SelectCommand = "SELECT uo.UserId, uo.FullName FROM vUserOrganizations uo where (uo.OrganizationId = @organizationId or uo.UserId = @userId) ORDER BY FullName";
            sds.SelectParameters.Add("organizationId", "");
            sds.SelectParameters.Add("userId", "");
            cmbApprovalTwo.DataSourceID = "";
            cmbApprovalTwo.DataSource = sds;

            sds = new SqlDataSource();
            sds.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            sds.SelectCommand = "SELECT uo.UserId, uo.FullName FROM vUserOrganizations uo where (uo.OrganizationId = @organizationId or uo.UserId = @userId) ORDER BY FullName";
            sds.SelectParameters.Add("organizationId", "");
            sds.SelectParameters.Add("userId", "");
            cmbApprovalThree.DataSourceID = "";
            cmbApprovalThree.DataSource = sds;

            sds = new SqlDataSource();
            sds.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            sds.SelectCommand = "SELECT uo.UserId, uo.FullName FROM vUserOrganizations uo where (uo.OrganizationId = @organizationId or uo.UserId = @userId) ORDER BY FullName";
            sds.SelectParameters.Add("organizationId", "");
            sds.SelectParameters.Add("userId", "");
            cmbApprovalFour.DataSourceID = "";
            cmbApprovalFour.DataSource = sds;

            sds = new SqlDataSource();
            sds.ConnectionString = ConnectionHelper.CONNECTION_STRING;
            sds.SelectCommand = "SELECT uo.UserId, uo.FullName FROM vUserOrganizations uo where (uo.OrganizationId = @organizationId or uo.UserId = @userId) ORDER BY FullName";
            sds.SelectParameters.Add("organizationId", "");
            sds.SelectParameters.Add("userId", "");
            cmbApprovalFive.DataSourceID = "";
            cmbApprovalFive.DataSource = sds;

            //create header
            //if (ric != null)
                ShowSTAHeader(ric.Id);

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {            
            ////create header
            //if (ric != null)
            //    ShowSTAHeader(ric.Id);

            //string str = txtReasonOfAlteration.Text;

            if (masterGrid != null)
            {
                //configure column wrapping
                for (int i = 0; i < masterGrid.Columns.Count; i++)
                {
                    GridViewDataColumn col = masterGrid.Columns[i] as GridViewDataColumn;
                    if (col == null) continue;

                    if (col.Name == "OldCode")
                    {
                        col.CellStyle.BorderRight.BorderColor = System.Drawing.Color.Black;
                        col.CellStyle.BorderRight.BorderWidth = new Unit("2px");
                        col.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                        col.CellStyle.CssClass = "wrapText";
                    }
                    else if (col.Name == "NewCode")
                    {
                        col.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                        col.CellStyle.CssClass = "wrapText";
                    }
                    else if (col.Name == "ModelId" || col.Name == "VariantId" || col.Name == "PackingMonth")
                    {
                        col.Visible = false;
                    }
                }
            }

            return true;
        }

        private void ShowSTAHeader(int ricid)
        {
            RIC ric = RicRepository.GetRIC(ricid);

            if (ric == null)
                return;

            //int ricId = ric.Id;
            string model = RicRepository.GetModelName(ric.CatalogModelId);
            lblHeaderPackingMonth.Text = ric.PackingMonth;

            #region STAHeader
            lblHeaderProdMonth.Text = ric.ProductionMonth;
            lblModelName.Text = model;
            lbl_valueRicNumber.Text = ric.RICNr;
            lbl_CommnosFrom.Text = ric.Commnos;
            value_lblChassis.Text = ric.ChassisNR;
            txtReasonOfAlteration.Text = ric.ReasonOfAlteration;
            Source.Text = ric.Source;
            LotNo.Text = ric.LotNo;
            txtCodes.Text = ric.Codes;
            lblIssuedOn.Text = ric.IssuedDate.ToString();
            var statusValue = ric.RICStatusId.ToString();
            cmbRicStatus.Text = statusValue;
            lblApprove1.Text = " Approved at : " + ric.ApprovedBy1Date.ToString();
            lblApprove2.Text = " Approved at :" + ric.ApprovedBy2Date.ToString();
            lblApprove3.Text = " Approved at :" + ric.ApprovedBy3Date.ToString();
            lblApprove4.Text = " Approved at :" + ric.ApprovedBy4Date.ToString();
            lblApprove5.Text = " Approved at :" + ric.ApprovedBy5Date.ToString();

            if (!IsPostBack && !IsCallback)
            {
                SqlDataSource sds = cmbApprovalOne.DataSource as SqlDataSource;
                sds.SelectParameters["organizationId"].DefaultValue = RicRepository.GetOrgId(
                    ConfigurationHelper.GetConfiguration("STA", "ApproverOrg1", PermissionHelper.GetCurrentAssemblyTypeId())).ToString();
                sds.SelectParameters["userId"].DefaultValue = ric.ApprovedBy1.ToString();
                cmbApprovalOne.DataBind();
                cmbApprovalOne.Text = RicRepository.GetUserFullName(ric.ApprovedBy1.Value);

                //cmbOrganization2.DataBind();
                sds = cmbApprovalTwo.DataSource as SqlDataSource;
                sds.SelectParameters["organizationId"].DefaultValue = RicRepository.GetOrgId(
                    ConfigurationHelper.GetConfiguration("STA", "ApproverOrg2", PermissionHelper.GetCurrentAssemblyTypeId())).ToString();
                sds.SelectParameters["userId"].DefaultValue = ric.ApprovedBy2.ToString();
                cmbApprovalTwo.DataBind();
                cmbApprovalTwo.Text = RicRepository.GetUserFullName(ric.ApprovedBy2.Value);

                sds = cmbApprovalThree.DataSource as SqlDataSource;
                sds.SelectParameters["organizationId"].DefaultValue = RicRepository.GetOrgId(
                    ConfigurationHelper.GetConfiguration("STA", "ApproverOrg3", PermissionHelper.GetCurrentAssemblyTypeId())).ToString();
                sds.SelectParameters["userId"].DefaultValue = ric.ApprovedBy3.ToString();
                cmbApprovalThree.DataBind();
                cmbApprovalThree.Text = RicRepository.GetUserFullName(ric.ApprovedBy3.Value);

                sds = cmbApprovalFour.DataSource as SqlDataSource;
                sds.SelectParameters["organizationId"].DefaultValue = RicRepository.GetOrgId(
                    ConfigurationHelper.GetConfiguration("STA", "ApproverOrg4", PermissionHelper.GetCurrentAssemblyTypeId())).ToString();
                sds.SelectParameters["userId"].DefaultValue = ric.ApprovedBy4.ToString();
                cmbApprovalFour.DataBind();
                cmbApprovalFour.Text = RicRepository.GetUserFullName(ric.ApprovedBy4.Value);

                sds = cmbApprovalFive.DataSource as SqlDataSource;
                sds.SelectParameters["organizationId"].DefaultValue = RicRepository.GetOrgId(
                    ConfigurationHelper.GetConfiguration("STA", "ApproverOrg5", PermissionHelper.GetCurrentAssemblyTypeId())).ToString();
                sds.SelectParameters["userId"].DefaultValue = ric.ApprovedBy5.ToString();
                cmbApprovalFive.DataBind();
                cmbApprovalFive.Text = RicRepository.GetUserFullName(ric.ApprovedBy5.Value);
            }

            //--GetValueStatus
            if (cmbRicStatus != null)
            {
                if (cmbRicStatus.Text == "White")
                {
                    StatusHeader.Text = "Draft";
                }
                else if (cmbRicStatus.Text == "Yellow")
                {
                    StatusHeader.Text = "On Progress";
                }
                else if (cmbRicStatus.Text == "Red")
                {
                    StatusHeader.Text = "Release";
                }
            }
            else
            {
                StatusHeader.Text = "";
            }

            //Btn_Submit.ClientSideEvents.Click = "function(s, e) { e.processOnServer = confirm('Are you sure want to submit this data?');}";

            #endregion

            #region Bind Approval And Button

            int Id = ((User)Session["user"]).Id;



            #region Bind Approval 1
            if (ric.ApprovedBy1 != null)
            {
                ////var cmbOrganization1 = Page.FindControl("cmbOrganization1") as ASPxComboBox;
                //int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy1.Value);
                //if (organizationId > 0)
                //    cmbOrganization1.Text = RicRepository.GetOrganizationName(organizationId);

                //////var cmbApprovalOne = Page.FindControl("cmbApprovalOne") as ASPxComboBox;
                ////cmbApprovalOne.DataSourceID = null;

                ////sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                ////cmbApprovalOne.DataSource = sdsUser;
                ////cmbApprovalOne.DataBind();

                //cmbApprovalOne.Text = RicRepository.GetUserFullName(ric.ApprovedBy1.Value);


                if (ric.RICStatusId == ERICStatus.White)
                {
                    cmbApprovalOne.Enabled = true;
                    cmbOrganization1.Enabled = true;
                    btnApproved1.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy1 && ric.ApprovedBy1Date == null)
                {
                    btnApproved1.Visible = true; // Has Submitted and Can Approved by Approver 1
                    cmbApprovalOne.Enabled = true;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow)
                {
                    cmbApprovalOne.Enabled = true;
                    // cmbOrganization1.Enabled = true;
                    btnApproved1.Visible = false;
                }
                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy1Date == DateTime.MinValue || ric.ApprovedBy1Date == null) // not yet approved still can edit
                    {
                        cmbApprovalOne.Enabled = true;
                        cmbOrganization1.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalOne.Enabled = false;
                        cmbOrganization1.Enabled = false;
                    }
                    btnApproved1.Visible = false;
                }
                else
                {
                    cmbApprovalOne.Enabled = false;
                    cmbOrganization1.Enabled = false;
                    btnApproved1.Visible = false;
                }
            }
            #endregion

            #region Bind Approval 2
            if (ric.ApprovedBy2 != null)
            {
                ////var cmbOrganization2 = Page.FindControl("cmbOrganization2") as ASPxComboBox;
                //int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy2.Value);
                //if (organizationId > 0)
                //    cmbOrganization2.Text = RicRepository.GetOrganizationName(organizationId);

                //////var cmbApprovalTwo = Page.FindControl("cmbApprovalTwo") as ASPxComboBox;
                ////cmbApprovalTwo.DataSourceID = null;

                ////sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                ////cmbApprovalTwo.DataSource = sdsUser;
                ////cmbApprovalTwo.DataBind();

                //cmbApprovalTwo.Text = RicRepository.GetUserFullName(ric.ApprovedBy2.Value);

                if (ric.RICStatusId == ERICStatus.White)
                {
                    cmbApprovalTwo.Enabled = true;
                    //cmbOrganization2.Enabled = true;
                    btnApproved2.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy2 && ric.ApprovedBy2Date == null)
                    btnApproved2.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 2

                else if (ric.RICStatusId == ERICStatus.Yellow)
                {
                    cmbApprovalTwo.Enabled = true;
                    //cmbOrganization2.Enabled = true;
                    btnApproved2.Visible = false;
                }

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy2Date == DateTime.MinValue || ric.ApprovedBy2Date == null) // not yet approved still can edit
                    {
                        cmbApprovalTwo.Enabled = true;
                        // cmbOrganization2.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalTwo.Enabled = false;
                        //cmbOrganization2.Enabled = false;
                    }

                    btnApproved2.Visible = false;
                }
                else
                {
                    cmbApprovalTwo.Enabled = false;
                    //cmbOrganization2.Enabled = false;
                    btnApproved2.Visible = false;
                }
            }
            #endregion

            #region Bind Approval 3
            if (ric.ApprovedBy3 != null)
            {
                //int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy3.Value);
                //if (organizationId > 0)
                //    cmbOrganization3.Text = RicRepository.GetOrganizationName(organizationId);

                ////var cmbApprovalThree = Page.FindControl("cmbApprovalThree") as ASPxComboBox;
                //cmbApprovalThree.DataSourceID = null;

                //sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                //cmbApprovalThree.DataSource = sdsUser;
                //cmbApprovalThree.DataBind();

                //cmbApprovalThree.Text = RicRepository.GetUserFullName(ric.ApprovedBy3.Value);

                if (ric.RICStatusId == ERICStatus.White)
                {
                    cmbApprovalThree.Enabled = true;
                    //cmbOrganization3.Enabled = true;
                    btnApproved3.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy3 && ric.ApprovedBy3Date == null)
                    btnApproved3.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 3

                else if (ric.RICStatusId == ERICStatus.Yellow)
                {
                    cmbApprovalThree.Enabled = true;
                    //cmbOrganization3.Enabled = true;
                    btnApproved3.Visible = false;
                }

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy3Date == DateTime.MinValue || ric.ApprovedBy3Date == null) // not yet approved still can edit
                    {
                        cmbApprovalThree.Enabled = true;
                        //cmbOrganization3.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalThree.Enabled = false;
                        //cmbOrganization3.Enabled = false;
                    }
                    btnApproved3.Visible = false;
                }
                else
                {
                    cmbApprovalThree.Enabled = false;
                    //cmbOrganization3.Enabled = false;
                    btnApproved3.Visible = false;
                }
            }
            #endregion

            #region Bind Approval 4
            if (ric.ApprovedBy4 != null)
            {
                //int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy4.Value);
                //if (organizationId > 0)
                //    cmbOrganization4.Text = RicRepository.GetOrganizationName(organizationId);

                ////var cmbApprovalFour = Page.FindControl("cmbApprovalFour") as ASPxComboBox;
                //cmbApprovalFour.DataSourceID = null;

                //sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                //cmbApprovalFour.DataSource = sdsUser;
                //cmbApprovalFour.DataBind();

                //cmbApprovalFour.Text = RicRepository.GetUserFullName(ric.ApprovedBy4.Value);

                if (ric.RICStatusId == ERICStatus.White)
                {
                    cmbApprovalFour.Enabled = true;
                    //cmbOrganization4.Enabled = true;
                    btnApproved4.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy4 && ric.ApprovedBy4Date == null)
                    btnApproved4.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 4

                else if (ric.RICStatusId == ERICStatus.Yellow)
                {
                    cmbApprovalFour.Enabled = true;
                    //cmbOrganization4.Enabled = true;
                    btnApproved4.Visible = false;
                }

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy4Date == DateTime.MinValue || ric.ApprovedBy4Date == null) // not yet approved still can edit
                    {
                        cmbApprovalFour.Enabled = true;
                        //cmbOrganization4.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalFour.Enabled = false;
                        // cmbOrganization4.Enabled = false;
                    }
                    btnApproved4.Visible = false;
                }
                else
                {
                    cmbApprovalFour.Enabled = false;
                    //cmbOrganization4.Enabled = false;
                    btnApproved4.Visible = false;
                }
            }

            #endregion
            
            #region Bind Approval 5
            if (ric.ApprovedBy5 != null)
            {
                //int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy5.Value);
                //if (organizationId > 0)
                //    cmbOrganization5.Text = RicRepository.GetOrganizationName(organizationId);

                ////var cmbApprovalFour = Page.FindControl("cmbApprovalFour") as ASPxComboBox;
                //cmbApprovalFive.DataSourceID = null;

                //sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                //cmbApprovalFive.DataSource = sdsUser;
                //cmbApprovalFive.DataBind();

                //cmbApprovalFive.Text = RicRepository.GetUserFullName(ric.ApprovedBy5.Value);

                if (ric.RICStatusId == ERICStatus.White)
                {
                    cmbApprovalFive.Enabled = true;
                    //cmbOrganization4.Enabled = true;
                    btnApproved5.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy5 && ric.ApprovedBy5Date == null)
                    btnApproved5.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 4

                else if (ric.RICStatusId == ERICStatus.Yellow)
                {
                    cmbApprovalFive.Enabled = true;
                    //cmbOrganization4.Enabled = true;
                    btnApproved5.Visible = false;
                }

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy5Date == DateTime.MinValue || ric.ApprovedBy5Date == null) // not yet approved still can edit
                    {
                        cmbApprovalFive.Enabled = true;
                        //cmbOrganization4.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalFive.Enabled = false;
                        // cmbOrganization4.Enabled = false;
                    }
                    btnApproved5.Visible = false;
                }
                else
                {
                    cmbApprovalFive.Enabled = false;
                    //cmbOrganization4.Enabled = false;
                    btnApproved5.Visible = false;
                }
            }
            #endregion

            if (ric.RICStatusId == ERICStatus.White) //Submit Changes
            {
                Btn_Submit.Visible = true;
                LoadWriteData();
                lblApprove1.Visible = true;
                lblApprove2.Visible = true;
                lblApprove3.Visible = true;
                lblApprove4.Visible = true;
                lblApprove5.Visible = true;
            }
            else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer) //Issuer always can update before status red/4
            {
                btnUpdate.Visible = true;
                if (ric.RICStatusId != ERICStatus.White)
                    Btn_Submit.Visible = false;
                LoadViewData();
                lblApprove1.Visible = true;
                lblApprove2.Visible = true;
                lblApprove3.Visible = true;
                lblApprove4.Visible = true;
                lblApprove5.Visible = true;
            }
            else if (ric.RICStatusId == ERICStatus.Yellow)
            {
                Btn_Submit.Visible = false; //Already submit
                btnUpdate.Visible = true;
                LoadWriteData();
                lblApprove1.Visible = true;
                lblApprove2.Visible = true;
                lblApprove3.Visible = true;
                lblApprove4.Visible = true;
                lblApprove5.Visible = true;

            }
            else
            {
                Btn_Submit.Visible = false; //Already submit
                btnUpdate.Visible = false;
                LoadViewData();
                lblApprove1.Visible = true;
                lblApprove2.Visible = true;
                lblApprove3.Visible = true;
                lblApprove4.Visible = true;
                lblApprove5.Visible = true;
            }
            #endregion

            if (!permissions.Contains(EPermissionType.Update))
            {
                btnUpdate.Visible = false;
            }

            if (!permissions.Contains(EPermissionType.Insert))
            {
                btnUpdate.Visible = false;
                Btn_Submit.Visible = false;
            }
            if (ric.StatusId == EStatus.Released)
            {
                btnUpdate.Visible = false;
            }
        }
        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    //#region check data
        //    //double num;
        //    //int Approver1 = cmbApprovalOne.Text.ToInt32(0);
        //    //int Approver2 = cmbApprovalTwo.Text.ToInt32(0);
        //    // int   Approver3 = cmbApprovalThree.Text.ToInt32(0);
        //    // int Approver4 = cmbApprovalFour.Text.ToInt32(0);
        //    // int Approver5 = cmbApprovalFive.Text.ToInt32(0);

        //    //#endregion
        //    int Approver1 = cmbApprovalOne.SelectedItem == null ? 0 : int.Parse(cmbApprovalOne.SelectedItem.Value.ToString());
        //    int Approver2 = cmbApprovalTwo.SelectedItem == null ? 0 : int.Parse(cmbApprovalTwo.SelectedItem.Value.ToString());
        //    int Approver3 = cmbApprovalThree.SelectedItem == null ? 0 : int.Parse(cmbApprovalThree.SelectedItem.Value.ToString());
        //    int Approver4 = cmbApprovalFour.SelectedItem == null ? 0 : int.Parse(cmbApprovalFour.SelectedItem.Value.ToString());
        //    int Approver5 = cmbApprovalFive.SelectedItem == null ? 0 : int.Parse(cmbApprovalFive.SelectedItem.Value.ToString());

        //    //int Approver1 = cmbApprovalOne.Value.ToInt32(0);
        //    //int Approver2 = cmbApprovalTwo.Value.ToInt32(0);
        //    //int Approver3 = cmbApprovalThree.Value.ToInt32(0);
        //    //int Approver4 = cmbApprovalFour.Value.ToInt32(0);
        //    //int Approver5 = cmbApprovalFive.Value.ToInt32(0);

        //    try
        //    {
        //        //if (cmbApprovalOne.Value == null || cmbApprovalTwo.Value == null || cmbApprovalThree.Value == null || cmbApprovalFour.Value == null || cmbApprovalFive.Value == null)
        //        //{
        //        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to update data. Reason of Alteration, Codes and Approver cannot blank  !');", true);
        //        //    ShowSTAHeader(ric.Id);
        //        //}
        //        //else
        //        //{
        //            //Update without fill oldUntil                    
        //            RicRepository.UpdateSTAHeader(
        //                ric.Id,
        //                lblHeaderProdMonth.Text,
        //                lbl_CommnosFrom.Text,
        //                LotNo.Text,
        //                value_lblChassis.Text,
        //                Source.Text,
        //                txtReasonOfAlteration.Text,
        //                txtCodes.Text,
        //                Approver1, Approver2, Approver3, Approver4, Approver5);

        //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data updated successfully !');", true);
        //            ShowSTAHeader(ric.Id);
        //        }
        //  //  }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to update data ! " + ex.Message + "');", true);
        //        //ShowSTAHeader();
        //    }
        //}

        protected void cmbApprovalOne_Callback(object sender, CallbackEventArgsBase e)
        {
            int organizationId = e.Parameter.ToInt32(0);
            if (organizationId == 0)
                return;

            SqlDataSource sds = cmbApprovalOne.DataSource as SqlDataSource;
            if (sds == null)
                return;

            sds.SelectParameters["organizationId"].DefaultValue = organizationId.ToString();

            //save current selected user
            object userId = cmbApprovalOne.Value;
            if (userId != null)
                sds.SelectParameters["userId"].DefaultValue = userId.ToString();

            cmbApprovalOne.DataBind();

            //restore prev selected user
            if (userId != null)
                cmbApprovalOne.Text = RicRepository.GetUserFullName(userId.ToInt32(0));

        }
        protected void cmbApprovalTwo_Callback(object sender, CallbackEventArgsBase e)
        {
            int organizationId = e.Parameter.ToInt32(0);
            if (organizationId == 0)
                return;

            SqlDataSource sds = cmbApprovalTwo.DataSource as SqlDataSource;
            if (sds == null)
                return;

            sds.SelectParameters["organizationId"].DefaultValue = organizationId.ToString();
            cmbApprovalTwo.DataBind();
        }
        protected void cmbApprovalThree_Callback(object sender, CallbackEventArgsBase e)
        {

            int organizationId = e.Parameter.ToInt32(0);
            if (organizationId == 0)
                return;

            SqlDataSource sds = cmbApprovalThree.DataSource as SqlDataSource;
            if (sds == null)
                return;

            sds.SelectParameters["organizationId"].DefaultValue = organizationId.ToString();
            //sds.SelectParameters[""]
            cmbApprovalThree.DataBind();
        }
        protected void cmbApprovalFour_Callback(object sender, CallbackEventArgsBase e)
        {

            int organizationId = e.Parameter.ToInt32(0);
            if (organizationId == 0)
                return;

            SqlDataSource sds = cmbApprovalFour.DataSource as SqlDataSource;
            if (sds == null)
                return;

            sds.SelectParameters["organizationId"].DefaultValue = organizationId.ToString();
            cmbApprovalFour.DataBind();
        }
        protected void cmbApprovalFive_Callback(object sender, CallbackEventArgsBase e)
        {


            int organizationId = e.Parameter.ToInt32(0);
            if (organizationId == 0)
                return;

            SqlDataSource sds = cmbApprovalFive.DataSource as SqlDataSource;
            if (sds == null)
                return;

            sds.SelectParameters["organizationId"].DefaultValue = organizationId.ToString();
            cmbApprovalFive.DataBind();
        }
        //protected void cmbRicStatus_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    FillUserCombo(int.Parse(e.Parameter));
        //}
        protected void FillUserCombo(int organizationId)
        {
            if (organizationId == 0)
            {
                cmbApprovalOne.DataSource = sdsUsers;
                cmbApprovalOne.DataBind();
                cmbApprovalTwo.DataSource = sdsUsers;
                cmbApprovalTwo.DataBind();
                cmbApprovalThree.DataSource = sdsUsers;
                cmbApprovalThree.DataBind();
                cmbApprovalFour.DataSource = sdsUsers;
                cmbApprovalFour.DataBind();
                cmbApprovalFive.DataSource = sdsUsers;
                cmbApprovalFive.DataBind();
            }
            else
            {
                sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                cmbApprovalOne.DataSourceID = null;
                cmbApprovalOne.DataSource = sdsUser;
                cmbApprovalOne.DataBind();

                cmbApprovalTwo.DataSourceID = null;
                cmbApprovalTwo.DataSource = sdsUser;
                cmbApprovalTwo.DataBind();

                cmbApprovalThree.DataSourceID = null;
                cmbApprovalThree.DataSource = sdsUser;
                cmbApprovalThree.DataBind();

                cmbApprovalFour.DataSourceID = null;
                cmbApprovalFour.DataSource = sdsUser;
                cmbApprovalFour.DataBind();

                cmbApprovalFive.DataSourceID = null;
                cmbApprovalFive.DataSource = sdsUser;
                cmbApprovalFive.DataBind();
            }
        }

        private string GetData(List<object> selectedValues)
        {
            string vpm = "";
            foreach (object[] oValue in selectedValues)
            {

                vpm = oValue[1].ToString();
            }
            return vpm;
        }
        private void LoadViewData()
        {
            lbl_valueRicNumber.Enabled = false;
            lblModelName.Enabled = false;
            lblHeaderPackingMonth.Enabled = false;
            Source.Enabled = false;
            lblHeaderProdMonth.Enabled = false;
            txtReasonOfAlteration.Enabled = false;
            lbl_CommnosFrom.Enabled = false;
            lblIssuedOn.Enabled = false;
            LotNo.Enabled = false;
            cmbRicStatus.Enabled = false;
            value_lblChassis.Enabled = false;
            txtCodes.Enabled = false;
            StatusHeader.Enabled = false;
            cmbOrganization1.Enabled = false;
            cmbOrganization2.Enabled = false;
            cmbOrganization3.Enabled = false;
            cmbOrganization4.Enabled = false;
            cmbOrganization5.Enabled = false;
        }
        private void LoadWriteData()
        {
            lbl_valueRicNumber.Enabled = false;
            lblModelName.Enabled = false;
            lblHeaderPackingMonth.Enabled = false;
            Source.Enabled = true;
            lblHeaderProdMonth.Enabled = true;
            txtReasonOfAlteration.Enabled = true;
            lbl_CommnosFrom.Enabled = true;
            lblIssuedOn.Enabled = false;
            LotNo.Enabled = true;
            cmbRicStatus.Enabled = false;
            value_lblChassis.Enabled = true;
            txtCodes.Enabled = true;
            StatusHeader.Enabled = false;
            cmbOrganization1.Enabled = true;
            cmbOrganization2.Enabled = true;
            cmbOrganization3.Enabled = true;
            cmbOrganization4.Enabled = true;
            cmbOrganization5.Enabled = true;
        }

        protected void callback_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "update")
            {
                //#endregion
                int Approver1 = cmbApprovalOne.Value.ToInt32(0);
                int Approver2 = cmbApprovalTwo.Value.ToInt32(0);
                int Approver3 = cmbApprovalThree.Value.ToInt32(0);
                int Approver4 = cmbApprovalFour.Value.ToInt32(0);
                int Approver5 = cmbApprovalFive.Value.ToInt32(0);

                try
                {
                    //if (txtReasonOfAlteration.Value == null || txtCodes.Value == null || cmbApprovalOne.Value == null || cmbApprovalTwo.Value == null || cmbApprovalThree.Value == null || cmbApprovalFour.Value == null || cmbApprovalFive.Value == null)
                    //{
                    //    callback.JSProperties["cpAlert"] = "Failed to update data. Reason of Alteration, Codes and Approver cannot blank  !";
                    //}
                    //else
                    //{
                        //Update without fill oldUntil                    
                        RicRepository.UpdateSTAHeader(
                            ric.Id,
                            lblHeaderProdMonth.Text,
                            lbl_CommnosFrom.Text,
                            LotNo.Text,
                            value_lblChassis.Text,
                            Source.Text,
                            txtReasonOfAlteration.Text,
                            txtCodes.Text,
                            Approver1, Approver2, Approver3, Approver4, Approver5);

                        //header info
                        int vpm = Request.QueryString["vpm"].ToInt32(0);
                        int modelId = Request.QueryString["modelid"].ToInt32(0);
                        int rictype = Request.QueryString["rictype"].ToInt32(0);
                        int ricId = Request.QueryString["id"].ToInt32(0);

                        ShowSTAHeader(ric.Id);
                        callback.JSProperties["cpAlert"] = "Data updated successfully !";
                    }
                //}
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to update data ! " + ex.Message + "'";
                }
            }
            else if (e.Parameter == "submit")
            {
                callback.JSProperties["cpAlert"] = "";

                string userName = ((User)Session["user"]).UserName;

                //header info
                int vpm = Request.QueryString["vpm"].ToInt32(0);
                int modelId = Request.QueryString["modelid"].ToInt32(0);
                int rictype = Request.QueryString["rictype"].ToInt32(0);
                int ricId = Request.QueryString["id"].ToInt32(0);

                using (AppDb ctx = new AppDb())
                {
                    if (ricId > 0)
                    {
                        ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                    }
                    else
                    {
                        ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                    }
                }

                if (ric == null)
                {
                    callback.JSProperties["cpAlert"] = "Invalid RIC number.";
                    return;
                }

                try
                {
                    //RicRepository.SaveSubmit(ric.Id); // Changes Status White
                    RicRepository.ReleaseRIC(ric.Id); //Release RIC

                    using (AppDb ctx = new AppDb())
                    {
                        if (ricId > 0)
                        {
                            ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                        }
                        else
                        {
                            ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                        }
                    }

                    callback.JSProperties["cpAlert"] = "RIC is submit";
                    ShowSTAHeader(ric.Id);
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to approve RIC! " + ex.Message + "'";
                }
            }

            else if (e.Parameter == "approve1")
            {
                callback.JSProperties["cpAlert"] = "";

                string userName = ((User)Session["user"]).UserName;

                //header info
                int vpm = Request.QueryString["vpm"].ToInt32(0);
                int modelId = Request.QueryString["modelid"].ToInt32(0);
                int rictype = Request.QueryString["rictype"].ToInt32(0);
                int ricId = Request.QueryString["id"].ToInt32(0);

                using (AppDb ctx = new AppDb())
                {
                    if (ricId > 0)
                    {
                        ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                    }
                    else
                    {
                        ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                    }
                }
                if (ric.RICStatusId != ERICStatus.Yellow)
                {
                    callback.JSProperties["cpAlert"] = "Approved Failed.";
                    return;

                }
                if (ric == null)
                {
                    callback.JSProperties["cpAlert"] = "Invalid RIC number.";
                    return;
                }

                if (ric.ApprovedBy1 == null || ric.ApprovedBy1.Value != PermissionHelper.GetAuthenticatedUserId())
                {
                    callback.JSProperties["cpAlert"] = "Not the approval. RIC is not approved.";
                    return;     //not the approval
                }

                //string modelName = RicRepository.GetModelName(ric.CatalogModelId);
                //string subject = String.Format("Summary Technical Alteration for PackingMonth {0}, CatalogModel {1} has been approved by {2}.", ric.PackingMonth, modelName, userName);
                //string message = subject;
                //string url = String.Format("/custom/STA/STAViewSingle.aspx?id={0}", ric.Id);

                try
                {
                    //approve
                    RicRepository.ApprovedOne(ric.Id);

                    //send notification to creator
                    //NotificationHelper.SendNotification(ric.Issuer, subject, message, url);

                    callback.JSProperties["cpAlert"] = "RIC is approved";
                    ShowSTAHeader(ric.Id);
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to approve RIC! " + ex.Message + "'";
                }

            }

            else if (e.Parameter == "approve2")
            {
                callback.JSProperties["cpAlert"] = "";

                string userName = ((User)Session["user"]).UserName;


                //header info
                int vpm = Request.QueryString["vpm"].ToInt32(0);
                int modelId = Request.QueryString["modelid"].ToInt32(0);
                int rictype = Request.QueryString["rictype"].ToInt32(0);
                int ricId = Request.QueryString["id"].ToInt32(0);

                using (AppDb ctx = new AppDb())
                {
                    if (ricId > 0)
                    {
                        ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                    }
                    else
                    {
                        ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                    }
                }
                if (ric.RICStatusId != ERICStatus.Yellow)
                {
                    callback.JSProperties["cpAlert"] = "Approved Failed.";
                    return;

                }
                if (ric == null)
                {
                    callback.JSProperties["cpAlert"] = "Invalid RIC number.";
                    return;
                }
                if (ric.ApprovedBy2 == null || ric.ApprovedBy2.Value != PermissionHelper.GetAuthenticatedUserId())
                {
                    callback.JSProperties["cpAlert"] = "Not the approval. RIC is not approved.";
                    return;     //not the approval
                }

                //string modelName = RicRepository.GetModelName(ric.CatalogModelId);
                //string subject = String.Format("Summary Technical Alteration for PackingMonth {0}, CatalogModel {1} has been approved by {2}.", ric.PackingMonth, modelName, userName);
                //string message = subject;
                //string url = String.Format("/custom/STA/STAViewSingle.aspx?id={0}", ric.Id);

                try
                {
                    //approve
                    RicRepository.ApprovedTwo(ric.Id);

                    //send notification to creator
                    //NotificationHelper.SendNotification(ric.Issuer, subject, message, url);

                    callback.JSProperties["cpAlert"] = "RIC is approved";
                    ShowSTAHeader(ric.Id);
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to approve RIC! " + ex.Message + "'";
                }

            }
            else if (e.Parameter == "approve3")
            {
                callback.JSProperties["cpAlert"] = "";

                string userName = ((User)Session["user"]).UserName;

                //header info
                int vpm = Request.QueryString["vpm"].ToInt32(0);
                int modelId = Request.QueryString["modelid"].ToInt32(0);
                int rictype = Request.QueryString["rictype"].ToInt32(0);
                int ricId = Request.QueryString["id"].ToInt32(0);

                using (AppDb ctx = new AppDb())
                {
                    if (ricId > 0)
                    {
                        ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                    }
                    else
                    {
                        ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                    }
                }
                if (ric.RICStatusId != ERICStatus.Yellow)
                {
                    callback.JSProperties["cpAlert"] = "Approved Failed.";
                    return;

                }
                if (ric == null)
                {
                    callback.JSProperties["cpAlert"] = "Invalid RIC number.";
                    return;
                }
                if (ric.ApprovedBy3 == null || ric.ApprovedBy3.Value != PermissionHelper.GetAuthenticatedUserId())
                {
                    callback.JSProperties["cpAlert"] = "Not the approval. RIC is not approved.";
                    return;     //not the approval
                }
                //string modelName = RicRepository.GetModelName(ric.CatalogModelId);
                //string subject = String.Format("Summary Technical Alteration for PackingMonth {0}, CatalogModel {1} has been approved by {2}.", ric.PackingMonth, modelName, userName);
                //string message = subject;
                //string url = String.Format("/custom/STA/STAViewSingle.aspx?id={0}", ric.Id);

                try
                {
                    //approve
                    RicRepository.ApprovedThree(ric.Id);

                    //send notification to creator
                    //NotificationHelper.SendNotification(ric.Issuer, subject, message, url);

                    callback.JSProperties["cpAlert"] = "RIC is approved";
                    ShowSTAHeader(ric.Id);
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to approve RIC! " + ex.Message + "'";
                }

            }
            else if (e.Parameter == "approve4")
            {
                callback.JSProperties["cpAlert"] = "";

                string userName = ((User)Session["user"]).UserName;

                //header info
                int vpm = Request.QueryString["vpm"].ToInt32(0);
                int modelId = Request.QueryString["modelid"].ToInt32(0);
                int rictype = Request.QueryString["rictype"].ToInt32(0);
                int ricId = Request.QueryString["id"].ToInt32(0);

                using (AppDb ctx = new AppDb())
                {
                    if (ricId > 0)
                    {
                        ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                    }
                    else
                    {
                        ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                    }
                }
                if (ric.RICStatusId != ERICStatus.Yellow)
                {
                    callback.JSProperties["cpAlert"] = "Approved Failed.";
                    return;

                }
                if (ric == null)
                {
                    callback.JSProperties["cpAlert"] = "Invalid RIC number.";
                    return;
                }
                if (ric.ApprovedBy4 == null || ric.ApprovedBy4.Value != PermissionHelper.GetAuthenticatedUserId())
                {
                    callback.JSProperties["cpAlert"] = "Not the approval. RIC is not approved.";
                    return;     //not the approval
                }

                //string modelName = RicRepository.GetModelName(ric.CatalogModelId);
                //string subject = String.Format("Summary Technical Alteration for PackingMonth {0}, CatalogModel {1} has been approved by {2}.", ric.PackingMonth, modelName, userName);
                //string message = subject;
                //string url = String.Format("/custom/STA/STAViewSingle.aspx?id={0}", ric.Id);

                try
                {
                    //approve
                    RicRepository.ApprovedFour(ric.Id);

                    //send notification to creator
                    //NotificationHelper.SendNotification(ric.Issuer, subject, message, url);

                    callback.JSProperties["cpAlert"] = "RIC is approved";
                    ShowSTAHeader(ric.Id);
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to approve RIC! " + ex.Message + "'";
                }

            }
            else if (e.Parameter == "approve5")
            {
                callback.JSProperties["cpAlert"] = "";

                string userName = ((User)Session["user"]).UserName;

                //header info
                int vpm = Request.QueryString["vpm"].ToInt32(0);
                int modelId = Request.QueryString["modelid"].ToInt32(0);
                int rictype = Request.QueryString["rictype"].ToInt32(0);
                int ricId = Request.QueryString["id"].ToInt32(0);

                using (AppDb ctx = new AppDb())
                {
                    if (ricId > 0)
                    {
                        ric = ctx.RICs.Where(x => x.Id == ricId).FirstOrDefault();
                    }
                    else
                    {
                        ric = ctx.RICs.Where(x => x.CatalogModelId == modelId && x.RICTypeId == rictype && x.PackingMonth == vpm.ToString()).FirstOrDefault();
                    }
                }

                if (ric.RICStatusId != ERICStatus.Yellow)
                {
                    callback.JSProperties["cpAlert"] = "Approved Failed.";
                    return;

                }
                if (ric == null)
                {
                    callback.JSProperties["cpAlert"] = "Invalid RIC number.";
                    return;
                }
                if (ric.ApprovedBy5 == null || ric.ApprovedBy5.Value != PermissionHelper.GetAuthenticatedUserId())
                {
                    callback.JSProperties["cpAlert"] = "Not the approval. RIC is not approved.";
                    return;     //not the approval
                }

                //string modelName = RicRepository.GetModelName(ric.CatalogModelId);
                //string subject = String.Format("Summary Technical Alteration for PackingMonth {0}, CatalogModel {1} has been approved by {2}.", ric.PackingMonth, modelName, userName);
                //string message = subject;
                //string url = String.Format("/custom/STA/STAViewSingle.aspx?id={0}", ric.Id);

                try
                {
                    //approve
                    RicRepository.ApprovedFive(ric.Id);

                    //send notification to creator
                    //NotificationHelper.SendNotification(ric.Issuer, subject, message, url);

                    //NotificationHelper.SendNotification(ric.ApprovedBy1.Value, subject, message, url);

                    callback.JSProperties["cpAlert"] = "RIC is approved";
                    ShowSTAHeader(ric.Id);
                }
                catch (Exception ex)
                {
                    callback.JSProperties["cpAlert"] = "Failed to approve RIC! " + ex.Message + "'";
                }

            }
        }
    }
}