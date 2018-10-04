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

namespace Ebiz.WebForm.Tids.CV.Custom
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
            int modelId = Request.QueryString["model"].ToInt32(0);
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
            //if (tableMeta.TableType == ETableType.StoredProcedure)
            //{
            //    queryFilter = string.Format("RICId={0}", ric.Id);
            //}
            //else
            //{
            //    queryFilter = string.Format("RICId={0}", ric.Id);
            //}

            //create header
            ShowSTAHeader();

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
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

        private void ShowSTAHeader()
        {
            if (ric == null)
                return;

            int ricId = ric.Id;
            //var iList = StaViewRepository.RetrieveDataStaView("usp_GetSTAView", ricId);
            string model = RicRepository.GetModelName(ric.CatalogModelId);
            //string variant = RecordImplemControlRepository.RetrieveVariantNameById(iList[0].VariantId);
            //int vehicleId = RecordImplemControlRepository.RetrieveVehiclesIdByPM(Convert.ToInt32(iList[0].PackingMonth), iList[0].ModelId, iList[0].VariantId);
            //string assytype = Session["assemblyType"].ToString();

            //if (vehicleId == 0)
            //    return;

            //header
            lblTop.Text = "CV"; lblTop2.Text = "CV";
            lblPCorEngine.Text = "PROD NUMBER ";
            //if (assytype == "1")
            //{
            //    lblTop.Text = "PC"; lblTop2.Text = "PC";
            //    lblPCorEngine.Text = "PROD NUMBER ";
            //    //lblNumPCorEngineMin.Text = RecordImplemControlRepository.RetrieveProdNumMin(vehicleId, iList[0].ModelId, iList[0].VariantId).ToString();
            //    //lblNumPCorEngineMax.Text = RecordImplemControlRepository.RetrieveProdNumMax(vehicleId, iList[0].ModelId, iList[0].VariantId).ToString();
            //}
            //else if (assytype == "2")
            //{
            //    lblTop.Text = "CV"; lblTop2.Text = "CV";
            //}
            //else
            //{
            //    lblTop.Text = "ENGINE"; lblTop2.Text = "ENGINE";
            //    lblPCorEngine.Text = "ENGINE NUMBER ";
            //    //lblNumPCorEngineMin.Text = RecordImplemControlRepository.RetrieveEngineNumMin(vehicleId, iList[0].ModelId, iList[0].VariantId).ToString();
            //    //lblNumPCorEngineMax.Text = RecordImplemControlRepository.RetrieveEngineNumMax(vehicleId, iList[0].ModelId, iList[0].VariantId).ToString();
            //}

            lblHeaderModelVariant.Text = model;
            lblHeaderPackingMonth.Text = ric.PackingMonth;

            hlPrint.NavigateUrl = "~/custom/Report/RIC.aspx?ricid=" + ricId;

            #region STAHeader

            //var lblIssuer = Page.FindControl("lblIssuer") as ASPxTextBox;
            lblIssuer.Text = PermissionHelper.GetUserNameByUserId(ric.Issuer);

            //var lblModelName = Page.FindControl("lblModelName") as ASPxTextBox;
            lblModelName.Text = model;

            //var lblbaumuster = Page.FindControl("lblbaumuster") as ASPxTextBox;
            lblbaumuster.Text = " BM: " + RicRepository.GetModelBaumuster(ric.CatalogModelId);

            //var lbl_valueRicNumber = Page.FindControl("lbl_valueRicNumber") as ASPxTextBox;
            lbl_valueRicNumber.Text = ric.RICNr;

            //var lbl_CommnosFrom = Page.FindControl("lbl_CommnosFrom") as ASPxTextBox;
            lbl_CommnosFrom.Text = ric.Commnos;

            //var lbl_ComnmnosTo = Page.FindControl("lbl_ComnmnosTo") as ASPxTextBox;
            //lbl_ComnmnosTo.Text = " - " + iList[0].CommnosTo.ToString();

            //var lbl_Unit = Page.FindControl("lbl_Unit") as ASPxTextBox;
            //var SumUnit = (iList[0].CommnosTo - iList[0].CommnosFrom) + 1;
            //lbl_Unit.Text = "(" + SumUnit.ToString() + " Unit" + ")";

            //var lblPackingMonth = Page.FindControl("lblPackingMonth") as ASPxTextBox;
            lblPackingMonth.Text = ric.PackingMonth;
            lblPackingMonth.Text = lblPackingMonth.Text.Substring(4, 2) + "/" + lblPackingMonth.Text.Substring(2, 2);

            //var value_lblChassis = Page.FindControl("value_lblChassis") as ASPxTextBox;
            value_lblChassis.Text = ric.ChassisNR;

            //var lblTechnicalAlteration = Page.FindControl("lblTechnicalAlteration") as ASPxTextBox;
            lblTechnicalAlteration.Text = ric.PackingMonth;
            lblTechnicalAlteration.Text = lblTechnicalAlteration.Text.Substring(4, 2) + "/" + lblTechnicalAlteration.Text.Substring(2, 2);

            //var txtReasonOfAlteration = Page.FindControl("txtReasonOfAlteration") as ASPxTextBox;
            txtReasonOfAlteration.Text = ric.ReasonOfAlteration;

            //var txtImpleDate = Page.FindControl("txtImpleDate") as ASPxDateEdit;
            if (ric.ImplementationDate == DateTime.MinValue)
                txtImpleDate.Value = null;
            else
                txtImpleDate.Value = ric.ImplementationDate;

            //var txtCumulativeFigure = Page.FindControl("txtCumulativeFigure") as ASPxTextBox;
            //txtCumulativeFigure.Text = iList[0].CumulativeFigure;

            //var txtRemarks = Page.FindControl("txtRemarks") as ASPxMemo;
            txtRemarks.Text = ric.Remark;

            //var txtCodes = Page.FindControl("txtCodes") as ASPxMemo;
            txtCodes.Text = ric.Codes;

            //var lblIssuedOn = Page.FindControl("lblIssuedOn") as ASPxTextBox;
            lblIssuedOn.Text = ric.IssuedDate.ToString("dd/MM/yyyy");

            //////var OldUntil = Page.FindControl("lblOldUntil") as ASPxTextBox;
            ////var txtOldUntil = Page.FindControl("txtOldUntil") as ASPxTextBox;
            //if (iList[0].OldUntil == DateTime.MinValue)
            //{
            //    txtOldUntil.Text = "";
            //}
            //else
            //    txtOldUntil.Text = iList[0].OldUntil.ToString("MM/yy");

            ////var lblNewFrom = Page.FindControl("lblNewFrom") as ASPxTextBox;
            //if (iList[0].NewFrom == DateTime.MinValue)
            //    lblNewFrom.Text = "";
            //else
            //    lblNewFrom.Text = iList[0].NewFrom.ToString("MM/yy");

            var statusValue = ric.RICStatusId.ToString();
            //var lblStatus = Page.FindControl("lblStatus") as ASPxTextBox;

            //if (statusValue == 1)
            //    lblStatus.Text = "Draft";
            //else if (statusValue == 2)
            //    lblStatus.Text = "White";
            //else if (statusValue == 3)
            //    lblStatus.Text = "Yellow";
            //else if (statusValue == 4)
            //    lblStatus.Text = "Red";

            //var btnUpdate = Page.FindControl("btnUpdate") as ASPxButton;
            //var Btn_Submit = Page.FindControl("Btn_Submit") as ASPxButton;
            Btn_Submit.ClientSideEvents.Click = "function(s, e) { e.processOnServer = confirm('Are you sure want to submit this data?');}";

            #endregion

            #region Bind Approval And Button

            //var ValueApprovalOne = iList[0].ApprovedBy1;
            //var ValueApprovalTwo = iList[0].ApprovedBy2;
            //var ValueApprovalThree = iList[0].ApprovedBy3;
            //var ValueApprovalFour = iList[0].ApprovedBy4;

            //var btnApproved1 = Page.FindControl("btnApproved1") as ASPxButton;
            //var btnApproved2 = Page.FindControl("btnApproved2") as ASPxButton;
            //var btnApproved3 = Page.FindControl("btnApproved3") as ASPxButton;
            //var btnApproved4 = Page.FindControl("btnApproved4") as ASPxButton;
            int Id = ((User)Session["user"]).Id;

            #region Bind Approval 1
            if (ric.ApprovedBy1 > 0)
            {
                //var cmbOrganization1 = Page.FindControl("cmbOrganization1") as ASPxComboBox;
                int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy1);
                if (organizationId > 0)
                    cmbOrganization1.Text = RicRepository.GetOrganizationName(organizationId);

                //var cmbApprovalOne = Page.FindControl("cmbApprovalOne") as ASPxComboBox;
                cmbApprovalOne.DataSourceID = null;

                sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                cmbApprovalOne.DataSource = sdsUser;
                cmbApprovalOne.DataBind();

                cmbApprovalOne.Text = RicRepository.GetUserFullName(ric.ApprovedBy1);


                if (ric.RICStatusId == ERICStatus.Draft)
                {
                    cmbApprovalOne.Enabled = true;
                    cmbOrganization1.Enabled = true;
                    btnApproved1.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy1 && (ric.ApprovedBy1Date == DateTime.MinValue || ric.ApprovedBy1Date == null))
                    btnApproved1.Visible = true; // Has Submitted and Can Approved by Approver 1

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
            if (ric.ApprovedBy2 > 0)
            {
                //var cmbOrganization2 = Page.FindControl("cmbOrganization2") as ASPxComboBox;
                int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy2);
                if (organizationId > 0)
                    cmbOrganization1.Text = RicRepository.GetOrganizationName(organizationId);

                //var cmbApprovalTwo = Page.FindControl("cmbApprovalTwo") as ASPxComboBox;
                cmbApprovalTwo.DataSourceID = null;

                sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                cmbApprovalTwo.DataSource = sdsUser;
                cmbApprovalTwo.DataBind();

                cmbApprovalTwo.Text = RicRepository.GetUserFullName(ric.ApprovedBy2);

                if (ric.RICStatusId == ERICStatus.Draft)
                {
                    cmbApprovalTwo.Enabled = true;
                    cmbOrganization2.Enabled = true;
                    btnApproved2.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy2 && (ric.ApprovedBy2Date == DateTime.MinValue || ric.ApprovedBy2Date == null))
                    btnApproved2.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 2

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy2Date == DateTime.MinValue || ric.ApprovedBy2Date == null) // not yet approved still can edit
                    {
                        cmbApprovalTwo.Enabled = true;
                        cmbOrganization2.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalTwo.Enabled = false;
                        cmbOrganization2.Enabled = false;
                    }

                    btnApproved2.Visible = false;
                }
                else
                {
                    cmbApprovalTwo.Enabled = false;
                    cmbOrganization2.Enabled = false;
                    btnApproved2.Visible = false;
                }
            }
            #endregion

            #region Bind Approval 3
            if (ric.ApprovedBy3 > 0)
            {
                int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy3);
                if (organizationId > 0)
                    cmbOrganization1.Text = RicRepository.GetOrganizationName(organizationId);

                //var cmbApprovalThree = Page.FindControl("cmbApprovalThree") as ASPxComboBox;
                cmbApprovalThree.DataSourceID = null;

                sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                cmbApprovalThree.DataSource = sdsUser;
                cmbApprovalThree.DataBind();

                cmbApprovalThree.Text = RicRepository.GetUserFullName(ric.ApprovedBy3);

                if (ric.RICStatusId == ERICStatus.Draft)
                {
                    cmbApprovalThree.Enabled = true;
                    cmbOrganization3.Enabled = true;
                    btnApproved3.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy3 && (ric.ApprovedBy3Date == DateTime.MinValue || ric.ApprovedBy3Date == null))
                    btnApproved3.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 3

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy3Date == DateTime.MinValue || ric.ApprovedBy3Date == null) // not yet approved still can edit
                    {
                        cmbApprovalThree.Enabled = true;
                        cmbOrganization3.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalThree.Enabled = false;
                        cmbOrganization3.Enabled = false;
                    }
                    btnApproved3.Visible = false;
                }
                else
                {
                    cmbApprovalThree.Enabled = false;
                    cmbOrganization3.Enabled = false;
                    btnApproved3.Visible = false;
                }
            }
            #endregion

            #region Bind Approval 4
            if (ric.ApprovedBy4 != 0)
            {
                int organizationId = RicRepository.GetUserOrganizationId(ric.ApprovedBy4);
                if (organizationId > 0)
                    cmbOrganization1.Text = RicRepository.GetOrganizationName(organizationId);

                //var cmbApprovalFour = Page.FindControl("cmbApprovalFour") as ASPxComboBox;
                cmbApprovalFour.DataSourceID = null;

                sdsUser.SelectParameters[0].DefaultValue = organizationId.ToString();
                cmbApprovalFour.DataSource = sdsUser;
                cmbApprovalFour.DataBind();

                cmbApprovalFour.Text = RicRepository.GetUserFullName(ric.ApprovedBy4);

                if (ric.RICStatusId == ERICStatus.Draft)
                {
                    cmbApprovalFour.Enabled = true;
                    cmbOrganization4.Enabled = true;
                    btnApproved4.Visible = false;
                }
                else if (ric.RICStatusId == ERICStatus.Yellow && Id == ric.ApprovedBy4 && (ric.ApprovedBy4Date == DateTime.MinValue || ric.ApprovedBy4Date == null))
                    btnApproved4.Visible = true; // Approved by Approver 1 And Can Confirm by Approver 4

                else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer)
                {
                    if (ric.ApprovedBy4Date == DateTime.MinValue || ric.ApprovedBy4Date == null) // not yet approved still can edit
                    {
                        cmbApprovalFour.Enabled = true;
                        cmbOrganization4.Enabled = true;
                    }
                    else
                    {
                        cmbApprovalFour.Enabled = false;
                        cmbOrganization4.Enabled = false;
                    }
                    btnApproved4.Visible = false;
                }
                else
                {
                    cmbApprovalFour.Enabled = false;
                    cmbOrganization4.Enabled = false;
                    btnApproved4.Visible = false;
                }
            }
            #endregion

            if (ric.RICStatusId == ERICStatus.Draft) //Submit Changes
                Btn_Submit.Visible = true;

            else if (ric.RICStatusId != ERICStatus.Red && Id == ric.Issuer) //Issuer always can update before status red/4
            {
                btnUpdate.Visible = true;
                if (ric.RICStatusId != ERICStatus.Draft)
                    Btn_Submit.Visible = false;
            }
            else
            {
                Btn_Submit.Visible = false; //Already submit
                btnUpdate.Visible = false;
                txtOldUntil.Enabled = false;
                lblNewFrom.Enabled = false;
                txtImpleDate.Enabled = false;
                txtCumulativeFigure.Enabled = false;
                txtReasonOfAlteration.Enabled = false;
                txtRemarks.Enabled = false;
                txtCodes.Enabled = false;
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
        }

        protected void SubmitChanges(object sender, EventArgs e)
        {
            int checkListStepId = 0;

            //For execute ReleaseRIC
            //var iList = StaViewRepository.RetrieveDataStaView("usp_GetSTAView", ric.Id);
            //int packingmonth = int.Parse(iList[0].PackingMonth);
            //int model = iList[0].ModelId;
            //int variant = iList[0].VariantId;
            //int assemblyTypeid = Convert.ToInt32(Session["assemblyType"].ToString());

            ////For Notification
            //string linkSTA, linkCP, linkJB;
            //string userName = ((User)Session["user"]).UserName;
            //string modelName = RecordImplemControlRepository.RetrieveModelNameById(iList[0].ModelId);
            //string variantName = RecordImplemControlRepository.RetrieveVariantNameById(iList[0].VariantId);
            //string message = packingmonth + ", Model " + modelName + " and Variant " + variantName + " has been released.";

            string linkSTA = "";
            if (Request.QueryString.Count > 0)
            {
                linkSTA = HttpContext.Current.Request.Url.AbsoluteUri;
            }
            else
                linkSTA = HttpContext.Current.Request.Url + "?Id=" + ric.Id;

            if (lblIssuer != null)
            {
                RicRepository.UpdateStatus(ric.Id, ERICStatus.White);

                //Release RIC
                //TODO
                //RicRepository.ReleaseRIC(ric.Id); //Release RIC
                //ControlPlanRepository.ProcessControlPlan(packingmonth, model, variant); // Set Control Plan

                //Send email notification for STA approver
                //TODO
                //NotificationAppRepository.SaveForNotification(userName, "Summary Technical Alteration for PackingMonth " + message, linkSTA, ""); //Notification for STA

                //int cpId = ControlPlanRepository.RetrieveCpIdByPkmModelVariant(packingmonth.ToString(), model, variant);
                //linkCP = "/custom/ControlPlans/CpDocumentControl.aspx?CpId=" + cpId;
                //NotificationAppRepository.SaveForNotification(userName, "Control Plan for PackingMonth " + message, linkCP, ""); //Notification for Control Plan

                //linkJB = "/custom/JobCard.aspx?vpm=" + packingmonth + "&model=" + model + "&variant=" + variant + "";
                //NotificationAppRepository.SaveForNotification(userName, "Job Card for PackingMonth " + message, linkJB, ""); //Notification for JobCard

                //List<string> dataApprover = new List<string>();
                //dataApprover = RecordImplemControlRepository.GetValueApproverSTA();

                //for (int i = 0; i < dataApprover.Count(); i++)
                //{
                //    DotWeb.Utils.EmailNotification.SendNotifSTA(UserRepository.RetrieveUserIdByUserName(dataApprover[i]), linkSTA, ric.Id, "Approved");
                //}

                //Set release in checklist
                //TODO
                //if (Request.QueryString["checkListId"] != null)
                //    checkListStepId = Request.QueryString["checkListId"].ToInt32(0);
                //else
                //{
                //    if (assemblyTypeid == 3) // Check Engine or PC; 3 = Engine
                //        checkListStepId = CheckListInstanceRepository.getCheckListInstanceStepCheckListId(packingmonth.ToString(), modelName, variantName, "Submit STA for Release", "Summary Technical Alteration Checklist-Engine");
                //    else
                //        checkListStepId = CheckListInstanceRepository.getCheckListInstanceStepCheckListId(packingmonth.ToString(), modelName, variantName, "Submit STA for Release", "Summary Technical Alteration Checklist");
                //}

                //if (checkListStepId != 0) // cek CheckListIdInstanceInfo
                //{
                //    CheckListInstanceRepository.SetStatusCheckListStep(checkListStepId, CheckListStatus.Complete);

                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Submitted !')", true);
                //    ShowSTAHeader();
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Submitted but failed to set complete Submit STA for Release !')", true);
                //    ShowSTAHeader();
                //}

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Submitted !')", true);
            }
        }

        protected void ApprovedOne(object sender, EventArgs e)
        {
            if (ric == null)
                return;

            ////var iList = StaViewRepository.RetrieveDataStaView("usp_GetSTAView", ric.Id);
            //int packingmonth = int.Parse(iList[0].PackingMonth);
            //int model = iList[0].ModelId;
            //int variant = iList[0].VariantId;

            string linkSTA;
            if (Request.QueryString.Count > 0)
                linkSTA = HttpContext.Current.Request.Url.AbsoluteUri;
            else
                linkSTA = HttpContext.Current.Request.Url + "?id=" + ric.Id;

            if (lblIssuer != null)
            {
                RicRepository.ApprovedOne(ric.Id);
                //RicRepository.UpdateStatus(ric.Id, ERICStatus.Yellow); // Changes Status yellow

                //Send email notification for STA comfirmer
                //TODO

                //List<string> dataApprover = new List<string>();
                //dataApprover = RecordImplemControlRepository.GetValueConfirmerSTA();

                //for (int i = 0; i < dataApprover.Count(); i++)
                //{
                //    DotWeb.Utils.EmailNotification.SendNotifSTA(UserRepository.RetrieveUserIdByUserName(dataApprover[i]), linkSTA, ric.Id, "Confirmed");
                //}

                btnApproved1.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Approved !')", true);
                ShowSTAHeader();
            }
        }
        protected void ApprovedTwo(object sender, EventArgs e)
        {
 
            if (lblIssuer != null)
            {
                RicRepository.ApprovedTwo(ric.Id);

                btnApproved2.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Approved !')", true);
                ShowSTAHeader();
            }
        }
        protected void ApprovedThree(object sender, EventArgs e)
        {
            if (lblIssuer != null)
            {
                RicRepository.ApprovedThree(ric.Id);

                btnApproved3.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Approved !')", true);
                ShowSTAHeader();
            }
        }
        protected void ApprovedFour(object sender, EventArgs e)
        {
            if (lblIssuer != null)
            {
                RicRepository.ApprovedFour(ric.Id);
                //RicRepository.UpdateStatus(ric.Id, ERICStatus.Red); // Changes Status Red

                btnApproved4.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Approved !')", true);
                ShowSTAHeader();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            #region check data
            double num;
            int Approver1 = 0, Approver2 = 0, Approver3 = 0, Approver4 = 0;

            //if (double.TryParse(cmbApprovalOne.Text, out num) && double.TryParse(cmbApprovalTwo.Text, out num) &&
            //    double.TryParse(cmbApprovalThree.Text, out num) && double.TryParse(cmbApprovalFour.Text, out num))
            //{
            //    Approver1 = Convert.ToInt32(cmbApprovalOne.Text);
            //    Approver2 = Convert.ToInt32(cmbApprovalTwo.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalThree.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalOne.Text, out num) && double.TryParse(cmbApprovalTwo.Text, out num) &&
            //  double.TryParse(cmbApprovalThree.Text, out num))
            //{
            //    Approver1 = Convert.ToInt32(cmbApprovalOne.Text);
            //    Approver2 = Convert.ToInt32(cmbApprovalTwo.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalOne.Text, out num) && double.TryParse(cmbApprovalTwo.Text, out num))
            //{
            //    Approver1 = Convert.ToInt32(cmbApprovalOne.Text);
            //    Approver2 = Convert.ToInt32(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalOne.Text, out num) && double.TryParse(cmbApprovalThree.Text, out num))
            //{
            //    Approver1 = Convert.ToInt32(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalOne.Text, out num) && double.TryParse(cmbApprovalFour.Text, out num))
            //{
            //    Approver1 = Convert.ToInt32(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = Convert.ToInt32(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalTwo.Text, out num) && double.TryParse(cmbApprovalThree.Text, out num))
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver2 = Convert.ToInt32(cmbApprovalTwo.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalTwo.Text, out num) && double.TryParse(cmbApprovalFour.Text, out num))
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalOne.Text);
            //    Approver2 = Convert.ToInt32(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = Convert.ToInt32(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalThree.Text, out num) && double.TryParse(cmbApprovalFour.Text, out num))
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalThree.Text);
            //    Approver4 = Convert.ToInt32(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalOne.Text, out num))
            //{
            //    Approver1 = Convert.ToInt32(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalTwo.Text, out num))
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalOne.Text);
            //    Approver2 = Convert.ToInt32(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalThree.Text, out num))
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = Convert.ToInt32(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            //else if (double.TryParse(cmbApprovalFour.Text, out num))
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = Convert.ToInt32(cmbApprovalFour.Text);
            //}
            //else
            //{
            //    Approver1 = UserRepository.RetrieveUserIdByFullName(cmbApprovalOne.Text);
            //    Approver2 = UserRepository.RetrieveUserIdByFullName(cmbApprovalTwo.Text);
            //    Approver3 = UserRepository.RetrieveUserIdByFullName(cmbApprovalThree.Text);
            //    Approver4 = UserRepository.RetrieveUserIdByFullName(cmbApprovalFour.Text);
            //}
            #endregion

            Approver1 = cmbApprovalOne.Text.ToInt32(0);
            Approver2 = cmbApprovalTwo.Text.ToInt32(0);
            Approver3 = cmbApprovalThree.Text.ToInt32(0);
            Approver3 = cmbApprovalFour.Text.ToInt32(0);

            try
            {
                if (txtReasonOfAlteration.Value == null || txtCodes.Value == null || cmbApprovalOne.Value == null || cmbApprovalTwo.Value == null || cmbApprovalThree.Value == null || cmbApprovalFour.Value == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to update data. Reason of Alteration, Codes and Approver cannot blank  !');", true);
                    ShowSTAHeader();
                }
                else if (txtOldUntil.Value != null)
                {
                    int monthOU = int.Parse(txtOldUntil.Text.Substring(0, 2));

                    if (txtOldUntil.Value != null && txtOldUntil.Text.Length == 5 && txtOldUntil.Text.Contains("/") && monthOU < 13)
                    {
                        if (RicRepository.UpdateSTAHeader(ric.Id, txtReasonOfAlteration.Text, txtCumulativeFigure.Text, txtImpleDate.Text,
                            txtCodes.Text, txtRemarks.Text, txtOldUntil.Text, Approver1, Approver2, Approver3, Approver4) == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data updated successfully !');", true);
                            //masterGrid.ExpandRow(RowData);
                            ShowSTAHeader();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to update data !')", true);
                            ShowSTAHeader();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to update. OldUntil wrong format  !');", true);
                        ShowSTAHeader();
                    }
                }
                else
                {
                    //Update without fill oldUntil                    
                    RicRepository.UpdateSTAHeader(ric.Id, txtReasonOfAlteration.Text, txtCumulativeFigure.Text, txtImpleDate.Text,
                       txtCodes.Text, txtRemarks.Text, txtOldUntil.Text, Approver1, Approver2, Approver3, Approver4);

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data updated successfully !');", true);
                    ShowSTAHeader();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to update data ! " + ex.Message + "');", true);
                ShowSTAHeader();
            }
        }

        protected void cmbApprovalOne_Callback(object sender, CallbackEventArgsBase e)
        {
            FillUserCombo(int.Parse(e.Parameter));
        }
        protected void cmbApprovalTwo_Callback(object sender, CallbackEventArgsBase e)
        {
            FillUserCombo(int.Parse(e.Parameter));
        }
        protected void cmbApprovalThree_Callback(object sender, CallbackEventArgsBase e)
        {
            FillUserCombo(int.Parse(e.Parameter));
        }
        protected void cmbApprovalFour_Callback(object sender, CallbackEventArgsBase e)
        {
            FillUserCombo(int.Parse(e.Parameter));
        }
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

        //protected void btnCgisNo_OnDataBinding(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int modelId;
        //        string vpm;
        //        int variant;
        //        if (Request.QueryString.Count > 0)
        //        {
        //            modelId = Convert.ToInt32(Request.QueryString["model"]);
        //            vpm = Request.QueryString["vpm"];
        //            variant = Convert.ToInt32(Request.QueryString["variant"]);
        //        }
        //        else
        //        {
        //            int ricId = Convert.ToInt32(Session["sessiionRICId"]);
        //            RecordImplemControl ric = RecordImplemControlRepository.RetrieveRicHeaderById(ricId);
        //            modelId = ric.ModelId;
        //            vpm = ric.PackingMonth;
        //            variant = ric.VariantId;
        //        }
        //        DotWeb.Models.Type oType = TypeRepository.RetrieveTypesByModelId(modelId);
        //        ASPxButton btn = (sender as ASPxButton);
        //        DateTime dt = DateTime.ParseExact(vpm + "01", "yyyyMMdd", CultureInfo.InvariantCulture);
        //        List<LastCgisManual> cgismanual = CgisSheetManualRepository.GetListLastCgisManual(modelId, variant, btn.CommandArgument, dt);
        //        var singleresult = (from a in cgismanual select a).OrderByDescending(a => a.ValidFrom).FirstOrDefault();
        //        if (cgismanual.Count > 0)
        //        {
        //            if (singleresult.FileType == "application/pdf")
        //            {
        //                btn.OnClientClick = "function(s,e){window.open('/custom/CommonWorks/CustomDocumentViewer.aspx?Id=" + singleresult.Id + "&docType=CGISManual" + "','_blank')}";
        //            }
        //            else
        //            {
        //                btn.OnClientClick = "function(s,e){window.open('DownloadFile.aspx?type=CGISManual&Id=" + singleresult.Id + "')}";
        //                //Response.Redirect("../DownloadFile.aspx?type=CGISManual&Id=" + singleresult.Id);
        //            }
        //        }
        //        else
        //        {
        //            btn.OnClientClick = "function(s,e){window.open('/custom/ControlPlans/CpCgisSheet.aspx?CGISStation=" + btn.CommandArgument +
        //                            "&CGISModel=" + oType.CGISModel + "&vpm=" + vpm + "&model=" + modelId + "&variant=" + variant + "','_blank')}";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppLogger.LogError(ex);
        //    }
        //}
    }
}