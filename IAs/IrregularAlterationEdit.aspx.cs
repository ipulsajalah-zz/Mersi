using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Utils;
using Ebiz.Tids.CV.Repositories;
using DevExpress.Web;
using DevExpress.WebUtils;
using System.Data;
using System.Collections;
using System.IO;
using DevExpress.Internal;
using System.Data.SqlClient;
using DevExpress.Utils;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using System.Data.Common;
using System.Text;
using System.Drawing;

namespace Ebiz.WebForm.custom.IAs
{
    public partial class IrregularAlterationEdit : System.Web.UI.Page
    {
        protected class FileInfo
        {
            public string FileName;
            public string FilePath;
        };
        List<IAAttachments> AttachFileModelList = new List<IAAttachments>();
        IAAttachmentsModels ListAttachment = new IAAttachmentsModels();
        public const string _ConStr = "AppDb";
        public DataSet ds = null;
        public ASPxUploadControl ucAttachmentReq;
        public ASPxUploadControl ucAttachmentApproval;
        public ASPxTextBox txtReport;
        public DataTable dtTemp_Attachment;
        protected ASPxDateEdit[] dates;

        protected static string TEMPORARY_DIR = AppConfiguration.TEMPORARY_DIR + "/upload";
        protected static string UPLOAD_DIR = AppConfiguration.UPLOAD_DIR + "/IrregularAlteration";
        protected static int MAX_DRAFT = 2;
        protected void Page_Load(object sender, EventArgs e)
        {

            dates = new ASPxDateEdit[12];
            dates[0] = Januari;
            dates[1] = Februari;
            dates[2] = Maret;
            dates[3] = April;
            dates[4] = Mei;
            dates[5] = Juni;
            dates[6] = Juli;
            dates[7] = Agustus;
            dates[8] = September;
            dates[9] = Oktober;
            dates[10] = November;
            dates[11] = Desember;

            string connstr = ConnectionHelper.CONNECTION_STRING;
            sdsIAHeader.ConnectionString = connstr;
            sdsIAStation.ConnectionString = connstr;
            sdsIAPart.ConnectionString = connstr;
            sdsIAModel.ConnectionString = connstr;
            sdsAttachment.ConnectionString = connstr;
            sdsIA.ConnectionString = connstr;
            sdsIATask.ConnectionString = connstr;
            sdsIATaskStatusLookup.ConnectionString = connstr;
            int assemblyTypeId = Session["assemblyType"].ToInt32(0);
            string user = ((User)Session["user"]).UserName; //Session["user"] as User;
            int userId = ((User)Session["user"]).Id;
            var GetIdIA = Request.QueryString["id"].ToInt32(0);
            var GetassignId = IrregAltRepository.getAssignId(GetIdIA, userId);
            var GetdeleagteId = IrregAltRepository.getDelegateId(GetIdIA, userId);
            int getauthor = IrregAltRepository.getauthorbyId(GetIdIA);
            //string getApprovalManager = IrregAltRepository.GetApprovalManager();
            //string getEmailApprvManager = IrregAltRepository.GetEmailApprvManager(getApprovalManager);
            //var GetApprovalManagerId = IrregAltRepository.getApprovalManagerId(getEmailApprvManager, getApprovalManager);
            int getOrgIdFromUser = Convert.ToInt32(((User)Session["user"]).OrganizationId);
            int GetApprovalManagerId = IrregAltRepository.GetApprovalManager(getOrgIdFromUser);

            if (user == null)
                userId = 0;

            //ignore if session is not found
            if (userId == 0 || assemblyTypeId == 0)
            {
                return;
            }



            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32(0);
                if (id > 0)
                {

                    if (userId == GetassignId || userId == GetdeleagteId)
                    {
                        lblHeader.Text = "Irregular Alteration - Edit";
                        ReadOnlyForm();
                        GenerateCheckBoxIaOrg(id, true);

                        LoadData();
                    }
                    else if (userId == getauthor)
                    {
                        lblHeader.Text = "Irregular Alteration - Edit";
                        ReadWriteForm();
                        GenerateCheckBoxIaOrg(id, false);


                        LoadData();
                    }
                    else if (userId == GetApprovalManagerId)
                    {
                        lblHeader.Text = "Irregular Alteration - Edit";
                        ReadWriteForm();
                        GenerateCheckBoxIaOrg(id, false);


                        LoadData();
                    }
                    else
                    {
                        lblHeader.Text = "Irregular Alteration - View";
                        ReadOnlyForm();
                        LoadData();
                        GenerateCheckBoxIaOrg(id, true);


                    }

                }
                else
                {

                    //create draft
                    id = CreateDraft(assemblyTypeId, userId);
                    hfIaID.Value = id.ToString();
                    lblHeader.Text = "Irregular Alteration - Create";
                    //CreateDumy(id);
                    LoadData();
                    GenerateCheckBoxIaOrg(id, false);

                    btnSave.Text = "Save";
                }


                //store the id
                lblId.Text = id.ToString();
                sdsIAHeader.SelectParameters["Id"].DefaultValue = id.ToString();
                sdsIAStation.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsIAPart.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsIAModel.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsRemarksInfo.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsRemarksInfoDelegate.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsRemarksInfoDelegate.SelectParameters["DelegateUserId"].DefaultValue = userId.ToString();
                sdsRemarksInfoAssigned.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsRemarksInfoAssigned.SelectParameters["AssignedUserId"].DefaultValue = userId.ToString();
                sdsAttachment.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsIATask.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsIAPartDisplay.SelectParameters["IAId"].DefaultValue = id.ToString();
                //filter model lookup based on current assembly type
                sdsModelLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
                sdsModelLookup.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsModelLookupEdit.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsStationLookup.SelectParameters["IAId"].DefaultValue = id.ToString();
                sdsTypeLookup.SelectParameters["AssemblyTypeId"].DefaultValue = assemblyTypeId.ToString();
                sdsOrganizationLookupEdit.SelectParameters["IAId"].DefaultValue = id.ToString();
                frmLayoutIrregAlt.DataSource = sdsIAHeader;
                frmLayoutIrregAlt.DataBind();
                htDescription.Html = txtDescription.Text;


                //show the corresponding buttons for action
                bool isApproved = IrregAltRepository.getisApproved(lblId.Text.ToInt32(0));
                bool isReadyForApproval = IrregAltRepository.getisReadyForApproval(lblId.Text.ToInt32(0));
                int? status = IrregAltRepository.getStatusIA(lblId.Text.ToInt32(0));
            }
            else
            {
                int id = Request.QueryString["id"].ToInt32(0);
                if (id > 0)
                {

                    if (userId == GetassignId || userId == GetdeleagteId)
                    {
                        GenerateCheckBoxIaOrg(id, true);
                    }
                    else if (userId == getauthor)
                    {
                        GenerateCheckBoxIaOrg(id, false);
                    }
                    else if (userId == GetApprovalManagerId)
                    {
                        GenerateCheckBoxIaOrg(id, false);
                    }

                }
                else
                {
                    id = hfIaID.Value.ToInt32(0); ;
                    GenerateCheckBoxIaOrg(id, false);
                }
            }


            gvAffectedDepart.DataBind();
            GVDataAttachment.DataSource = Session["Attachment"];
            GVDataAttachment.DataBind();

            //insertDraftAttachment
            int sIAId = lblId.Text.ToInt32(0);
            var SumRowData = GVDataAttachment.VisibleRowCount;
            List<IAAttachments> ListAttachFile = new List<IAAttachments>();
            for (int i = 0; i < SumRowData; i++)
            {
                IAAttachments AttachFile = new IAAttachments();
                AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FilePath").ToString();

                ListAttachFile.Add(AttachFile);
            }
            for (int x = 0; x < ListAttachFile.Count(); x++)
            {
                IrregAltRepository.InsertAttachMentIA(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, sIAId);
            }

            //rebind data attachment
            int HeaderId = lblId.Text.ToInt32(0);
            if (HeaderId != 0)
            {
                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(HeaderId);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
            }
            else
            {
                var getHeaderId = lblId.Text;
                int HeaderId_ = Convert.ToInt32(getHeaderId);
                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(HeaderId_);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
            }
            if (cmbIAType.Value == null)
            {
                cmbIAType.Text = "-Select-";
            }
            if (cmbModels.Value == null)
            {
                cmbModels.Text = "-Select-";
            }
            if (cmbArea.Value == null)
            {
                cmbArea.Text = "-Select-";
            }
            if (cmbStation.Value == null)
            {
                cmbStation.Text = "-Select-";
            }
            string getAreaId = IrregAltRepository.GetAreaId(cmbArea.Text);
            cmbStation.DataSourceID = null;
            sdsStation.SelectParameters["AssemblySectionId"].DefaultValue = getAreaId;
            cmbStation.DataSource = sdsStation;
            cmbStation.DataBind();
        }
        protected void ReadOnlyForm()
        {
            //NOTE: we can just user frmLayoutIrregAlt.Enabled=false. But the resulting look is not neat
            var GetIdIA = Request.QueryString["id"].ToInt32(0);
            Session["Attachment"] = null;
            btnGeneratePart.Enabled = false;
            btnGeneratePart.Visible = false;
            lblgeneratepart.Visible = false;
            cmbIAType.Enabled = false;
            cmbModels.Enabled = false;
            cmbArea.Enabled = false;
            cmbStation.Enabled = false;
            txtNumber.ReadOnly = true;
            txtInfoNumber.ReadOnly = true;
            txtDescription.ReadOnly = true;
            txtPart.ReadOnly = true;
            DescriptionPart.ReadOnly = true;
            dtValidFrom.ReadOnly = true;
            btnSendToApproved.Visible = false;
            dtValidTo.ReadOnly = true;
            // chkIsDynamicCheck.ReadOnly = true;
            //cmbApproverUser.ReadOnly = true;
            ImplementationDate.ReadOnly = true;
            //btnApproval.Visible = false;
            //btnSendApproval.Visible = false;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
            btnSendBack.Visible = false;
            SendNotif.Visible = false;
            btnEditIA.Visible = false;
            BtnSaveVpn.Visible = false;
            Januari.ReadOnly = true;
            Februari.ReadOnly = true;
            Maret.ReadOnly = true;
            April.ReadOnly = true;
            Mei.ReadOnly = true;
            Juni.ReadOnly = true;
            Juli.ReadOnly = true;
            Agustus.ReadOnly = true;
            September.ReadOnly = true;
            Oktober.ReadOnly = true;
            November.ReadOnly = true;
            Desember.ReadOnly = true;
            htDescription.Enabled = false;
            htDescription.Settings.AllowHtmlView = false;
            htDescription.Settings.AllowDesignView = false;
            htDescription.SettingsHtmlEditing.AllowFormElements = false;

            int HeaderId = GetIdIA;
            if (HeaderId != 0)
            {

                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(HeaderId);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
            }
            else
            {
                var getHeaderId = lblId.Text;
                int HeaderId_ = Convert.ToInt32(getHeaderId);
                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(HeaderId_);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
            }


            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowEditButton = false;

            ShowAttachment.Enabled = false;
            btnSave.Visible = false;
        }
        protected void ReadWriteForm()
        {
            //NOTE: we can just user frmLayoutIrregAlt.Enabled=false. But the resulting look is not neat
            var GetIdIA = Request.QueryString["id"].ToInt32(0);
            Session["Attachment"] = null;
            btnGeneratePart.Enabled = true;
            btnGeneratePart.Visible = false;
            lblgeneratepart.Visible = true;
            cmbIAType.Enabled = true;
            txtNumber.ReadOnly = false;
            txtInfoNumber.ReadOnly = false;
            //txtInfoFrom.ReadOnly = false;
            //memoSubject.ReadOnly = false;
            txtDescription.ReadOnly = false;
            dtValidFrom.ReadOnly = false;
            btnSendToApproved.Visible = false;
            dtValidTo.ReadOnly = false;
            //chkIsDynamicCheck.ReadOnly = false;
            //cmbApproverUser.ReadOnly = false;
            ImplementationDate.ReadOnly = false;
            htDescription.Enabled = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
            btnSendBack.Visible = false;
            SendNotif.Visible = false;
            BtnSaveVpn.Visible = true;
            Januari.ReadOnly = false;
            Februari.ReadOnly = false;
            Maret.ReadOnly = false;
            April.ReadOnly = false;
            Mei.ReadOnly = false;
            Juni.ReadOnly = false;
            Juli.ReadOnly = false;
            Agustus.ReadOnly = false;
            September.ReadOnly = false;
            Oktober.ReadOnly = false;
            November.ReadOnly = false;
            Desember.ReadOnly = false;
            txtPart.ReadOnly = false;
            DescriptionPart.ReadOnly = true;
            cmbModels.Enabled = true;
            cmbArea.Enabled = true;
            cmbStation.Enabled = true;
            htDescription.Settings.AllowHtmlView = true;
            htDescription.Settings.AllowDesignView = false;

            int HeaderId = GetIdIA;
            if (HeaderId != 0)
            {

                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(HeaderId);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
            }
            else
            {
                var getHeaderId = lblId.Text;
                int HeaderId_ = Convert.ToInt32(getHeaderId);
                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(HeaderId_);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
            }

            ShowAttachment.Enabled = true;
            btnSave.Visible = true;

            btnEditIA.Visible = false;
            //btnDelete.Visible = false;
            if (GetIdIA != null)
            {
                btnSave.Text = "Update";
            }
            else
            {
                btnSave.Text = "Save";
            }
            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowEditButton = true;
        }
        protected void LoadData()
        {

            int GetIdIA = Request.QueryString["id"].ToInt32(0);
            var IAid = IrregAltRepository.RetrieveIAHeadersById(GetIdIA);


            var getDataIAPart = IrregAltRepository.RetrieveIAPartByIAId(GetIdIA);
            var getDataIAModel = IrregAltRepository.RetrieveIAModelByIAId(GetIdIA);
            var getDataIAStation = IrregAltRepository.RetrieveIAStationByIAId(GetIdIA);

            var GetPVM = IrregAltRepository.RetrieveIAPackingMonthById(GetIdIA);

            //GetData Approver Manager
            int getOrgIdFromUser = Convert.ToInt32(((User)Session["user"]).OrganizationId);
            string EmailManager = lblEmailManager.Text;
            int GetApprovalManagerId = IrregAltRepository.GetApprovalManager(getOrgIdFromUser);
            int GetSecondManagerId = IrregAltRepository.GetManager(getOrgIdFromUser);

            //string getApprovalManager = IrregAltRepository.GetApprovalManager();
            //string getApprovalManagerName = IrregAltRepository.GetApprovalManagerbyUserName(getApprovalManager);
            //string getEmailApprvManager = IrregAltRepository.GetEmailApprvManager(getApprovalManager);
            //var GetApprovalManagerId = IrregAltRepository.getApprovalManagerId(getEmailApprvManager, getApprovalManager);

            //GetData Author
            int getauthor = IrregAltRepository.getauthorbyId(GetIdIA);
            string getauthorname = IrregAltRepository.getauthorName(getauthor);
            string getauthoremail = IrregAltRepository.getauthorEmail(getauthor);
            DataTable dt = getListVpm(GetIdIA);
            List<IATask> IATaskData = IrregAltRepository.checkIATaskForNotify(GetIdIA.ToInt32(0));
            var getSectionId = IrregAltRepository.getidsectionId(GetIdIA); //GetAssemblySectionData
            var getSectionName = IrregAltRepository.getidsectionById(getSectionId.ToInt32(0));

            int userId = ((User)Session["user"]).Id; //SessionUser for name & email Approver
            var GetassignId = IrregAltRepository.getAssignId(GetIdIA, userId);
            var GetdeleagteId = IrregAltRepository.getDelegateId(GetIdIA, userId);
            var GetNameAssign = IrregAltRepository.getauthorName(GetassignId);
            string getAssignemail = IrregAltRepository.getauthorEmail(GetassignId);
            var GetNameDelegate = IrregAltRepository.getauthorName(GetdeleagteId);
            string getDelegateEmail = IrregAltRepository.getauthorEmail(GetdeleagteId);

            var getAssignUserIdEicEd = IrregAltRepository.GetAssignedUserIdEicED();
            string getIAorgNameEicEd = IrregAltRepository.getauthorName(getAssignUserIdEicEd.ToInt32(0));
            var getAssignUserIdEicEl = IrregAltRepository.GetAssignedUserIdEicEL();
            string getIAorgNameEicEl = IrregAltRepository.getauthorName(getAssignUserIdEicEl.ToInt32(0));
            var getAssignUserIdAGC = IrregAltRepository.GetAssignedUserIdAGC();
            string getIAorgNameAGC = IrregAltRepository.getauthorName(getAssignUserIdAGC.ToInt32(0));
            var getAssignUserIdQEP_R = IrregAltRepository.GetAssignedUserIdQEP_R();
            string getIAorgNameQEP_R = IrregAltRepository.getauthorName(getAssignUserIdQEP_R.ToInt32(0));
            var getAssignUserIdQEP = IrregAltRepository.GetAssignedUserIdQEP();
            string getIAorgNameQEP = IrregAltRepository.getauthorName(getAssignUserIdQEP.ToInt32(0));
            var getAssignUserIdPLG = IrregAltRepository.GetAssignedUserIdPLG();
            string getIAorgNamePLG = IrregAltRepository.getauthorName(getAssignUserIdPLG.ToInt32(0));
            var getAssignUserIdACV = IrregAltRepository.GetAssignedUserIdACV();
            string getIAorgNameACV = IrregAltRepository.getauthorName(getAssignUserIdACV.ToInt32(0));
            var getAssignUserIdSCCPPC = IrregAltRepository.GetAssignedUserIdSCCPPC();
            string getIAorgNameSCCPPC = IrregAltRepository.getauthorName(getAssignUserIdSCCPPC.ToInt32(0));
            var getAssignUserIdQA = IrregAltRepository.GetAssignedUserIdQA();
            string getIAorgNameQA = IrregAltRepository.getauthorName(getAssignUserIdQA.ToInt32(0));
            var getAssignUserIdIT = IrregAltRepository.GetAssignedUserIdIT();
            string getIAorgNameIT = IrregAltRepository.getauthorName(getAssignUserIdIT.ToInt32(0));
            var getAssignUserIdVPC = IrregAltRepository.GetAssignedUserIdVPC();
            string getIAorgNameVPC = IrregAltRepository.getauthorName(getAssignUserIdVPC.ToInt32(0));

            //GetLastStatusIA
            string getlastStatus = GetLastStatusIA(GetIdIA);
            if (getlastStatus != "")
            {
                LastStatusIA.Visible = true;
                StatusIA.Visible = false;
                LastStatusIA.Text = getlastStatus;
            }
            else
            {
                StatusIA.Text = "";
            }
            if (IAid != null)
            {
                if (IAid.StatusId.ToInt32() == 0)
                {
                    StatusIA.Text = "";
                }
                else if (IAid.StatusId.ToInt32() == 1)
                {
                    StatusIA.Text = "Draft";
                }
                else if (IAid.StatusId.ToInt32() == 2)
                {
                    StatusIA.Text = "Open";
                }
                else if (IAid.StatusId.ToInt32() == 3)
                {
                    StatusIA.Text = "Close";
                }
                else if (IAid.StatusId.ToInt32() == 4)
                {
                    StatusIA.Text = "Cancel";
                }
                else
                {
                    StatusIA.Text = "";
                }
            }
            else
            {
                StatusIA.Text = "";
            }


            //mapping Date
            if (GetPVM != null)
            {
                string[] strArr = new string[dt.Rows.Count];
                int i = 0;
                foreach (DataRow r in dt.Rows)
                {
                    Nullable<DateTime> d = r["DateVpm"] as Nullable<DateTime>;
                    if (d != null)
                    {
                        dates[d.Value.Month - 1].Date = d.Value;
                    }
                    strArr[i] = r["DateVpm"].ToString("MM");
                    i++;
                }
            }

            //data information
            if (getDataIAPart != null)
            {
                txtPart.Text = getDataIAPart.PartNumber;
                DescriptionPart.Text = getDataIAPart.Description;
            }
            else
            {
                txtPart.Text = "";
                DescriptionPart.Text = "";
            }
            ///
            if (IAid != null)
            {
                cmbIAType.Value = IAid.IATypeId;

                lblheader2.Value = string.Format("{0}", IrregAltRepository.getTypeDescription(IAid.IATypeId));

            }

            if (getDataIAModel != null)
            {
                cmbModels.Value = getDataIAModel.CatalogModelId;
            }
            else
            {
                cmbModels.Value = "";
            }
            if (getDataIAStation != null)
            {
                cmbArea.Value = getDataIAStation.AssemblySectionId;
                cmbStation.Value = getDataIAStation.StationId;
            }
            else
            {
                cmbModels.Value = "";
                cmbStation.Value = "";
            }
            List<vIAOrganizations> iaorg = IrregAltRepository.getIAOrganizations();
            foreach (vIAOrganizations item in iaorg)
            {
                lblManagerName.Text += item.OrganizationName + " - " + item.FullName + ";  ";
            }
            //lblApproverManager.Text = getApprovalManagerName;
            //lblEmailManager.Text = getEmailApprvManager;
            Namelogin.Text = getauthorname;
            lblUsername_.Text = ((User)Session["user"]).UserName;
            emailuser.Text = getauthoremail;
            if (IAid != null)
            {
                createddate.Text = IAid.CreatedDate.ToString("dd-MM-yyyy");

                DateTime dat = DateTime.Parse(IAid.DistributionDate.ToString());
                dtDistributionDate.Text = dat.ToString("dd-MM-yyyy");
            }
            else
            {
                createddate.Text = "";
                dtDistributionDate.Text = "";
            }



            //Counting data approver on progress bar
            var getAllApprover = IrregAltRepository.getAllApprover(GetIdIA);
            var getAllIsApproved = IrregAltRepository.getAllIsApproved(GetIdIA);
            ProgresBar.Maximum = 100;
            ProgresBar.Minimum = 0;
            if (getAllApprover != 0 && getAllIsApproved != 0)
            {
                decimal? CountProgress = ((getAllIsApproved.ToDecimal(0) / getAllApprover.ToDecimal(0)) * 100);
                ProgresBar.Value = CountProgress;
            }
            else if (getAllIsApproved == 0)
            {
                ProgresBar.Value = 0;
            }
            else
            {
                ProgresBar.Value = 0;
            }

            if (lblHeader.Text == "Irregular Alteration - Edit")
            {
                int GetManagerUserId = 0;
                if (GetApprovalManagerId != 0)
                {
                    GetManagerUserId = GetApprovalManagerId;
                }
                else
                {
                    GetManagerUserId = GetSecondManagerId;
                }

                if (userId == GetassignId || userId == GetdeleagteId || userId == GetManagerUserId || userId == getauthor)
                {
                    if (userId == GetManagerUserId && userId == GetassignId)
                    {
                        string getnameapprover = ((User)Session["user"]).FullName;
                        string getemailapprover = ((User)Session["user"]).Email;
                        NameApprover.Text = GetNameDelegate;
                        EmailApprover.Text = getemailapprover;

                        //Remarks Summary
                        string remarks = "";
                        DataTable dtIATaskStatus = getReamksSummaryForAuthorUser(GetIdIA);
                        for (int i = 0; i < dtIATaskStatus.Rows.Count; i++)
                        {
                            DataRow row = dtIATaskStatus.Rows[i];
                            string orgz = row["OrganizationName"].ToString();
                            string email = row["EmailDelegate"].ToString();
                            string dates = row["ReportDate"].ToString();
                            if (orgz != "" && email != "" && dates != "")
                            {

                                remarks += row["OrganizationName"] + " " + "by" + " " + row["EmailDelegate"] + " " + "at" + " " + row["ReportDate"] + "<br />";
                                remarks += " " + " " + row["Report"] + "<br /><br />";
                                remarks += row["Reminder"] + "<hr />" + "<br />";
                                lblremaks.Text = remarks;

                            }
                        }

                        if (StatusIA.Text == "Draft") //Status Draft
                        {
                            ReadOnlyForm();
                            if (IAid.IsReadyForApproval == true && IAid.IsApproved == false)
                            {
                                btnSubmit.Visible = true;
                                btnSendBack.Visible = true;
                                btnCancel.Visible = true;
                                btnEditIA.Visible = true;
                            }
                            else
                            {
                                btnSubmit.Visible = false;
                                btnSendBack.Visible = false;
                            }
                            SendNotif.Visible = false;
                            BtnSaveVpn.Visible = false;
                            btnSendToApproved.Visible = false;

                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Open") //Status open
                        {
                            ReadOnlyForm();
                            btnCancel.Visible = true;
                            SendNotif.Visible = true;

                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowEditButton = true;
                        }
                        else if (StatusIA.Text == "Close") //Status Close
                        {
                            ReadOnlyForm();
                        }
                        else if (StatusIA.Text == "Cancel") //Status Cancel
                        {
                            ReadOnlyForm();
                        }
                        else
                        {
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                        }
                    }
                    else if (userId == GetassignId) //userId = GetassignId
                    {
                        NameApprover.Text = GetNameAssign;
                        EmailApprover.Text = getAssignemail;

                        //remarks Sumary
                        string remarks = "";
                        DataTable dtIATaskStatus = getReamksSummaryForAssignUser(GetIdIA, GetassignId);
                        for (int i = 0; i < dtIATaskStatus.Rows.Count; i++)
                        {
                            DataRow row = dtIATaskStatus.Rows[i];
                            string orgz = row["OrganizationName"].ToString();
                            string email = row["EmailDelegate"].ToString();
                            string dates = row["ReportDate"].ToString();
                            if (orgz != "" && email != "" && dates != "")
                            {

                                remarks += row["OrganizationName"] + " " + "by" + " " + row["EmailDelegate"] + " " + "at" + " " + row["ReportDate"] + "<br />";
                                remarks += " " + " " + row["Report"] + "<br /><br />";
                                remarks += row["Reminder"] + "<hr />" + "<br />";
                                lblremaks.Text = remarks;

                            }
                        }

                        if (StatusIA.Text == "Draft") //Status Draft
                        {
                            ReadOnlyForm();
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Open") //Status Open
                        {
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowEditButton = true;
                        }
                        else if (StatusIA.Text == "Close") //Status Close
                        {
                            ReadOnlyForm();
                        }
                        else if (StatusIA.Text == "Cancel") //Status Cancel
                        {
                            ReadOnlyForm();
                        }

                    }
                    else if (userId == GetdeleagteId) //userId = GetdeleagteId
                    {
                        NameApprover.Text = GetNameDelegate;
                        EmailApprover.Text = getDelegateEmail;

                        //remarks Sumary
                        string remarks = "";
                        DataTable dtIATaskStatus = getReamksSummaryForDelegateUser(GetIdIA, GetdeleagteId);
                        for (int i = 0; i < dtIATaskStatus.Rows.Count; i++)
                        {
                            DataRow row = dtIATaskStatus.Rows[i];
                            string orgz = row["OrganizationName"].ToString();
                            string email = row["EmailDelegate"].ToString();
                            string dates = row["ReportDate"].ToString();
                            if (orgz != "" && email != "" && dates != "")
                            {

                                remarks += row["OrganizationName"] + " " + "by" + " " + row["EmailDelegate"] + " " + "at" + " " + row["ReportDate"] + "<br />";
                                remarks += " " + " " + row["Report"] + "<br /><br />";
                                remarks += row["Reminder"] + "<hr />" + "<br />";
                                lblremaks.Text = remarks;

                            }
                        }

                        if (StatusIA.Text == "Draft") //Status Draft
                        {
                            ReadOnlyForm();
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Open") //Status Open
                        {
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Close") //Status Close
                        {
                            ReadOnlyForm();
                        }
                        else if (StatusIA.Text == "Cancel") //Status Cancel
                        {
                            ReadOnlyForm();
                        }
                    }
                    else if (userId == GetManagerUserId) //userId = GetApprovalManagerId
                    {
                        string getnameapprover = ((User)Session["user"]).FullName;
                        string getemailapprover = ((User)Session["user"]).Email;
                        NameApprover.Text = GetNameDelegate;
                        EmailApprover.Text = getemailapprover;

                        //Remarks Summary
                        string remarks = "";
                        DataTable dtIATaskStatus = getReamksSummaryForAuthorUser(GetIdIA);
                        for (int i = 0; i < dtIATaskStatus.Rows.Count; i++)
                        {
                            DataRow row = dtIATaskStatus.Rows[i];
                            string orgz = row["OrganizationName"].ToString();
                            string email = row["EmailDelegate"].ToString();
                            string dates = row["ReportDate"].ToString();
                            if (orgz != "" && email != "" && dates != "")
                            {

                                remarks += row["OrganizationName"] + " " + "by" + " " + row["EmailDelegate"] + " " + "at" + " " + row["ReportDate"] + "<br />";
                                remarks += " " + " " + row["Report"] + "<br /><br />";
                                remarks += row["Reminder"] + "<hr />" + "<br />";
                                lblremaks.Text = remarks;

                            }
                        }

                        if (StatusIA.Text == "Draft") //Status draft
                        {
                            ReadOnlyForm();
                            if (IAid.IsReadyForApproval == true && IAid.IsApproved == false)
                            {
                                btnSubmit.Visible = true;
                                btnSendBack.Visible = true;
                                btnCancel.Visible = true;
                                btnEditIA.Visible = true;
                            }
                            else
                            {
                                btnSubmit.Visible = false;
                                btnSendBack.Visible = false;
                            }
                            SendNotif.Visible = false;
                            BtnSaveVpn.Visible = false;
                            btnSendToApproved.Visible = false;

                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Open") //Status Open
                        {
                            ReadOnlyForm();
                            btnCancel.Visible = true;
                            SendNotif.Visible = true;

                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Close") //Status Close
                        {
                            ReadOnlyForm();
                        }
                        else if (StatusIA.Text == "Cancel") //Status Cancel
                        {
                            ReadOnlyForm();
                        }
                        else
                        {
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                        }
                    }
                    else if (userId == getauthor) //Userid = getauthor
                    {
                        //Remaks Summary
                        string remarks = "";
                        DataTable dtIATaskStatus = getReamksSummaryForAuthorUser(GetIdIA);
                        for (int i = 0; i < dtIATaskStatus.Rows.Count; i++)
                        {
                            DataRow row = dtIATaskStatus.Rows[i];
                            string orgz = row["OrganizationName"].ToString();
                            string email = row["EmailDelegate"].ToString();
                            string dates = row["ReportDate"].ToString();
                            if (orgz != "" && email != "" && dates != "")
                            {

                                remarks += row["OrganizationName"] + " " + "by" + " " + row["EmailDelegate"] + " " + "at" + " " + row["ReportDate"] + "<br />";
                                remarks += " " + " " + row["Report"] + "<br /><br />";
                                remarks += row["Reminder"] + "<hr />" + "<br />";
                                lblremaks.Text = remarks;

                            }
                        }

                        if (StatusIA.Text == "Draft") //Status Draft
                        {
                            ReadOnlyForm();
                            btnEditIA.Visible = true;
                            btnSave.Visible = false;
                            //btnSave.Text = "Update";
                            SendNotif.Visible = false;
                            btnSubmit.Visible = false;
                            btnSendBack.Visible = false;
                            BtnSaveVpn.Visible = false;
                            btnCancel.Visible = true;
                            if (IAid.IsReadyForApproval == true || IAid.IsApproved == true)
                            {
                                btnSendToApproved.Visible = true;
                                btnSendToApproved.Text = "Waiting For Approval";
                                btnSendToApproved.Enabled = false;
                                btnEditIA.Visible = false;
                                btnCancel.Visible = false;
                                if (IAid.IsApproved == true)
                                {
                                    btnSendToApproved.Text = "Is Approve";
                                    btnSendToApproved.Enabled = true;
                                }
                            }
                            else
                            {
                                btnSendToApproved.Visible = true;
                            }
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Open") //Status Open
                        {
                            ReadOnlyForm();
                            btnCancel.Visible = true;
                            SendNotif.Visible = true;

                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                            (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
                        }
                        else if (StatusIA.Text == "Close") //Status Close
                        {
                            ReadOnlyForm();
                        }
                        else if (StatusIA.Text == "Cancel") //Status Cancel
                        {
                            ReadOnlyForm();
                        }
                    }
                }
                else if (userId != GetassignId || userId != GetdeleagteId || userId != GetApprovalManagerId || userId != getauthor)
                {
                    ReadOnlyForm();
                }
            }
            else if (lblHeader.Text == "Irregular Alteration - View")
            {
                ReadOnlyForm();
                //Remarks Summary
                string remarks = "";
                DataTable dtIATaskStatus = getReamksSummaryForAuthorUser(GetIdIA);
                for (int i = 0; i < dtIATaskStatus.Rows.Count; i++)
                {
                    DataRow row = dtIATaskStatus.Rows[i];
                    string orgz = row["OrganizationName"].ToString();
                    string email = row["EmailDelegate"].ToString();
                    string dates = row["ReportDate"].ToString();
                    if (orgz != "" && email != "" && dates != "")
                    {

                        remarks += row["OrganizationName"] + " " + "by" + " " + row["EmailDelegate"] + " " + "at" + " " + row["ReportDate"] + "<br />";
                        remarks += " " + " " + row["Report"] + "<br /><br />";
                        remarks += row["Reminder"] + "<hr />" + "<br />";
                        lblremaks.Text = remarks;

                    }
                }
            }
            else //Create New
            {
                Namelogin.Text = ((User)Session["user"]).FirstName + ' ' + ((User)Session["user"]).LastName; //Session Author
                emailuser.Text = ((User)Session["user"]).Email;
                createddate.Text = "";
                StatusIA.Visible = true;
                StatusIA.Text = "Draft";
                btnSubmit.Visible = false;
                SendNotif.Visible = false;
                DescriptionPart.ReadOnly = true;
                var GetFileAttachment = IrregAltRepository.GetAttachmentViewEditIA(GetIdIA);
                Session["Attachment"] = GetFileAttachment;
                GVDataAttachment.DataSource = GetFileAttachment;
                GVDataAttachment.DataBind();
                (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
                (gvAffectedDepart.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            }
        }
        protected void GenerateCheckBoxIaOrg(int IdIa, bool flag)
        {
            List<vIAOrganizations> iaorg = IrregAltRepository.getIAOrganizations();

            List<int> OrgIaTask = IrregAltRepository.getOrganizationId(IdIa);

            StringBuilder builder = new StringBuilder();
            int idx = 1;
            foreach (vIAOrganizations temp in iaorg)
            {
                int idIaTask = IrregAltRepository.getTaskId(IdIa, temp.OrganizationId);

                string templateheader = string.Format(@" <div class='col-md-3 form-group'>
                                                            <label class='control-label col-sm-3' >{0}</label>
                                                            <div class='col-sm-9'>
                                                             ", temp.OrganizationName);
                //                string templatefoot = string.Format(@"<dx:ASPxLabel runat='server' ID='lbl{0}'>{1}</dx:ASPxLabel>
                //                                                      <asp:HiddenField ID='hf{2}' runat='server' val='{3}' />
                //                                                            </div>
                //                                                    </div>", temp.OrganizationName, temp.FullName,temp.Id,idIaTask);


                string templatefoot = string.Format(@"   </div>
                                                    </div>");

                string templateclearfix = string.Format(@"<div class='clearfix'> 
                                                    </div>");

                Literal temlit = new Literal();
                temlit.Text = templateheader;

                ASPxCheckBox cb1 = new ASPxCheckBox();

                cb1.ID = "cb" + temp.OrganizationId;
                cb1.ClientInstanceName = "cb" + temp.OrganizationId;
                cb1.AutoPostBack = false;
                var command = string.Format("cbIAOrg_OnChanged");
                cb1.JSProperties["cpID"] = temp.Id;
                cb1.JSProperties["cpManagerUserId"] = temp.ManagerUserId;
                cb1.JSProperties["cpOrganizationId"] = temp.OrganizationId;
                cb1.JSProperties["cpIaId"] = IdIa;
                cb1.JSProperties["cpidIaTask"] = idIaTask;
                cb1.ClientSideEvents.CheckedChanged = command;
                cb1.ReadOnly = flag;

                TextBox tb = new TextBox();
                tb.ID = "tbx" + temp.Id;
                tb.Text = idIaTask.ToString();
                tb.Attributes.Add("class", "hiddentb");


                bool isInList = OrgIaTask.IndexOf(temp.OrganizationId) != -1;
                if (isInList)
                {
                    cb1.Checked = true;
                }

                Literal temlit2 = new Literal();
                temlit2.Text = templatefoot;

                divDataView.Controls.Add(temlit);
                divDataView.Controls.Add(cb1);
                divDataView.Controls.Add(tb);

                divDataView.Controls.Add(temlit2);


                //untuk clearfix bootstrap
                if (idx % 4 == 0)
                {
                    Literal temlit3 = new Literal();
                    temlit3.Text = templateclearfix;
                    divDataView.Controls.Add(temlit3);
                }
                idx++;



            }
        }
        protected void CreateDumy(int IAId)
        {
            // CreateDumyIAParts(IAId);
            //CreateDumyIAModel(IAId);
            //CreateDumyIAStationArea(IAId);
        }
        protected void InsertIAParts(int Id, string part, string descriptionPart)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "insert into IAParts (IAId, PartNumber, Description, PartNumberMigration, PartDescriptionMigration)"
                                        + "values ('" + Id + "' ,'" + part + "','" + descriptionPart + "', null , null)";
                string sqlCmdExecute = string.Format(sqlCmd, Id, part, descriptionPart);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected void InsertIAModel(int Id, int cmbModel)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "insert into IAModels (IAId, CatalogModelId, AssemblySectionId, StationId, ModelMigration) "
                                        + "values ('" + Id + "' ,'" + cmbModel + "' , 0, null, null)";
                string sqlCmdExecute = string.Format(sqlCmd, Id, cmbModel);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected void InsertIAStation(int IAId, int areaid, int stationid)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "insert into IAStations (IAId, AssemblySectionId, StationId, IncomingArea, ReworkArea) "
                                        + "values ('" + IAId + "' ,'" + areaid + "' ,'" + stationid + "', null, null )";
                string sqlCmdExecute = string.Format(sqlCmd, IAId);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected DataTable getListVpm(int GetIdIA)
        {
            DataTable PVM;
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();
                string sqlcmd = "select a.DateVpm from IAPackingMonths a where a.IAId = {0}";
                string sqlCmdExct = string.Format(sqlcmd, GetIdIA);
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExct;
                    cmd.CommandType = CommandType.Text;
                    var datareader = cmd.ExecuteReader();
                    var datatable = new DataTable();
                    datatable.Load(datareader);
                    PVM = datatable;
                    LoggerHelper.Info("Get List Pvm Success");
                }
                return PVM;
            }
        }
        protected DataTable getListOrg(int GetIdIA)
        {
            DataTable ORG;
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();
                string sqlcmd = "select a.OrganizationId from IATasks a where a.IAId = {0}";
                string sqlCmdExct = string.Format(sqlcmd, GetIdIA);
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExct;
                    cmd.CommandType = CommandType.Text;
                    var datareader = cmd.ExecuteReader();
                    var datatable = new DataTable();
                    datatable.Load(datareader);
                    ORG = datatable;
                    LoggerHelper.Info("Get List ORG Success");
                }
                return ORG;
            }
        }
        protected DataTable getReamksSummaryForAuthorUser(int GetIdIA)
        {
            DataTable Task;
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();
                string sqlcmd = "select t1.Id, t1.IAId,	t1.DelegateUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.AssignedUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.DelegateDate, t1.OrganizationId, t4.Name as OrganizationName, t1.IATaskStatusId as StatusIATask, t2.ReportDate, t2.Report, t2.Reminder, t3.Email as EmailDelegate, t5.Email as EmailAssignment from IATasks t1 "
                                + "left join IATaskReports t2 on t2.IATaskId = t1.Id "
                                + "left join Users t3 on t1.DelegateUserId = t3.Id "
                                + "left join Users t5 on t1.AssignedUserId = t5.Id "
                                + "left join Organizations t4 on t1.OrganizationId = t4.Id "
                                + "where t1.IAId = {0}";
                string sqlCmdExct = string.Format(sqlcmd, GetIdIA);
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExct;
                    cmd.CommandType = CommandType.Text;
                    var datareader = cmd.ExecuteReader();
                    var datatable = new DataTable();
                    datatable.Load(datareader);
                    Task = datatable;
                    LoggerHelper.Info("Get List IATasks Success");
                }
                return Task;
            }
        }
        protected DataTable getReamksSummaryForAssignUser(int GetIdIA, int AssignUserId)
        {
            DataTable Task;
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                //con.Open();
                //string sqlcmd = "select t1.Id, t1.IAId,	t1.DelegateUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.AssignedUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.DelegateDate, t1.OrganizationId, t4.Name as OrganizationName, t1.IATaskStatusId as StatusIATask, t2.ReportDate, t2.Report, t2.Reminder, t3.Email as EmailDelegate, t5.Email as EmailAssignment from IATasks t1 "
                //                + "left join IATaskReports t2 on t2.IATaskId = t1.Id "
                //                + "left join Users t3 on t1.DelegateUserId = t3.Id "
                //                + "left join Users t5 on t1.AssignedUserId = t5.Id "
                //                + "left join Organizations t4 on t1.OrganizationId = t4.Id "
                //                + "where t1.IAId = {0} and t1.AssignedUserId = {1}";
                //string sqlCmdExct = string.Format(sqlcmd, GetIdIA, AssignUserId);
                //using (DbCommand cmd = con.CreateCommand())
                //{
                //    cmd.Connection = con;
                //    cmd.CommandText = sqlCmdExct;
                //    cmd.CommandType = CommandType.Text;
                //    var datareader = cmd.ExecuteReader();
                //    var datatable = new DataTable();
                //    datatable.Load(datareader);
                //    Task = datatable;
                //    LoggerHelper.Info("Get List IATasks Success");
                //}
                //return Task;
                con.Open();
                string sqlcmd = "select t1.Id, t1.IAId,	t1.DelegateUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.AssignedUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.DelegateDate, t1.OrganizationId, t4.Name as OrganizationName, t1.IATaskStatusId as StatusIATask, t2.ReportDate, t2.Report, t2.Reminder, t3.Email as EmailDelegate, t5.Email as EmailAssignment from IATasks t1 "
                                + "left join IATaskReports t2 on t2.IATaskId = t1.Id "
                                + "left join Users t3 on t1.DelegateUserId = t3.Id "
                                + "left join Users t5 on t1.AssignedUserId = t5.Id "
                                + "left join Organizations t4 on t1.OrganizationId = t4.Id "
                                + "where t1.IAId = {0}";
                string sqlCmdExct = string.Format(sqlcmd, GetIdIA);
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExct;
                    cmd.CommandType = CommandType.Text;
                    var datareader = cmd.ExecuteReader();
                    var datatable = new DataTable();
                    datatable.Load(datareader);
                    Task = datatable;
                    LoggerHelper.Info("Get List IATasks Success");
                }
                return Task;
            }
        }
        protected DataTable getReamksSummaryForDelegateUser(int GetIdIA, int DelegateUserId)
        {
            DataTable Task;
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                //con.Open();
                //string sqlcmd = "select t1.Id, t1.IAId,	t1.DelegateUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.AssignedUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.DelegateDate, t1.OrganizationId, t4.Name as OrganizationName, t1.IATaskStatusId as StatusIATask, t2.ReportDate, t2.Report, t2.Reminder, t3.Email as EmailDelegate, t5.Email as EmailAssignment from IATasks t1 "
                //                + "left join IATaskReports t2 on t2.IATaskId = t1.Id "
                //                + "left join Users t3 on t1.DelegateUserId = t3.Id "
                //                + "left join Users t5 on t1.AssignedUserId = t5.Id "
                //                + "left join Organizations t4 on t1.OrganizationId = t4.Id "
                //                + "where t1.IAId = {0} and t1.DelegateUserId = {1}";
                //string sqlCmdExct = string.Format(sqlcmd, GetIdIA, DelegateUserId);
                //using (DbCommand cmd = con.CreateCommand())
                //{
                //    cmd.Connection = con;
                //    cmd.CommandText = sqlCmdExct;
                //    cmd.CommandType = CommandType.Text;
                //    var datareader = cmd.ExecuteReader();
                //    var datatable = new DataTable();
                //    datatable.Load(datareader);
                //    Task = datatable;
                //    LoggerHelper.Info("Get List IATasks Success");
                //}
                //return Task;
                con.Open();
                string sqlcmd = "select t1.Id, t1.IAId,	t1.DelegateUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.AssignedUserId, t3.FirstName +' '+ t3.LastName as FullName, t1.DelegateDate, t1.OrganizationId, t4.Name as OrganizationName, t1.IATaskStatusId as StatusIATask, t2.ReportDate, t2.Report, t2.Reminder, t3.Email as EmailDelegate, t5.Email as EmailAssignment from IATasks t1 "
                                + "left join IATaskReports t2 on t2.IATaskId = t1.Id "
                                + "left join Users t3 on t1.DelegateUserId = t3.Id "
                                + "left join Users t5 on t1.AssignedUserId = t5.Id "
                                + "left join Organizations t4 on t1.OrganizationId = t4.Id "
                                + "where t1.IAId = {0}";
                string sqlCmdExct = string.Format(sqlcmd, GetIdIA);
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExct;
                    cmd.CommandType = CommandType.Text;
                    var datareader = cmd.ExecuteReader();
                    var datatable = new DataTable();
                    datatable.Load(datareader);
                    Task = datatable;
                    LoggerHelper.Info("Get List IATasks Success");
                }
                return Task;
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            //Notes: Page_Unload is called when the page has been completely rendered
            //       Dont delete the draft here!
        }
        protected void ASPxCallback_Callback(object source, CallbackEventArgs e)
        {
            //int id = lblId.Text.ToInt32(0);

            //if (!string.IsNullOrEmpty(txtNumber.Text))//(id > 0)
            //{
            //    DeleteDraft(txtNumber.Text);
            //}
        }

        #region Action Buttons
        protected void btnSendApproval_Click(object sender, EventArgs e)
        {
            //if (gvAffectedModels.VisibleRowCount == 0 || gvAffectedPart.VisibleRowCount == 0 || gvStations.VisibleRowCount == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to send Approval. Station, Type and Model cannot be empty !')", true);
            //}
            //else if (string.IsNullOrEmpty(txtInfoNumber.Text) ||
            //       string.IsNullOrEmpty(htDescription.Html.ToString()) || dtValidFrom.Value == null || dtValidTo.Value == null)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to send Approval. Info Number, Info From, Subject, Description, Valid From and Valid To cannot blank !')", true);
            //}
            //else
            //{
            //    int id = lblId.Text.ToInt32(0);
            //    if (SendIAForApproval(id))
            //    {
            //        try
            //        {
            //            // send Email to Approver
            //            string link;
            //            if (Request.QueryString.Count > 0)
            //                link = HttpContext.Current.Request.Url.AbsoluteUri;
            //            else
            //                link = HttpContext.Current.Request.Url.AbsoluteUri + "?id=" + id;

            //            // string penerima = UserRepository.RetrieveEmailByFullName(cmbApproverUser.Text);
            //            // DotWeb.Utils.EmailNotification.NotificationIA(penerima, link, id, "Open");
            //            //Ebiz.Tids.CV.Utils.EmailNotification.NotificationIA(penerima, link, id, "Open");

            //            //btnSendApproval.Visible = false;
            //            btnSubmit.Visible = true;
            //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Send approval successfully !');", true);
            //        }
            //        catch (Exception ex)
            //        {
            //            LoggerHelper.LogError(ex);
            //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('" + ex.Message + "')", true);
            //        }
            //    }
            //}

            //ReadOnlyForm();
            //StatusIA.Text = "Waiting for Approval";
            //btnSave.Visible = false;
            //btnEditIA.Visible = true;
        }
        protected void btnApproval_Click(object sender, EventArgs e)
        {
            string linkIA;
            string user = ((User)Session["user"]).UserName; //Session["user"] as User;
            string message = "Irregular Alteration " + txtNumber.Text + " has been released.";
            string link = HttpContext.Current.Request.Url.AbsoluteUri;

            try
            {
                int id = lblId.Text.ToInt32(0);
                if (ApproveIA(id))
                {
                    //Send Email to AssignManager
                    for (int i = 0; i < gvAffectedDepart.VisibleRowCount; i++)
                    {
                        string penerima = UserRepository.RetrieveEmailByUserId(Convert.ToInt32(gvAffectedDepart.GetRowValues(i, "AssignedUserId").ToString()));
                        //DotWeb.Utils.EmailNotification.NotificationIA(penerima, link, id, "Assigned");
                    }

                    //string approver = cmbApproverUser.Text;
                    string author = UserRepository.RetrieveEmailByUserId(IrregAltRepository.getAuthorIdByIA(id));
                    //DotWeb.Utils.EmailNotification.NotificationforAuthor(author, link, lblId.Text.ToInt32(0), approver, "IAApproved");

                    //dtDistributionDate.Value = DateTime.Now;
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data approved !');", true);

                    //make the form read only
                    // btnApproval.Visible = false;
                    ReadOnlyForm();
                    StatusIA.Text = "Open";

                    //TODO: send notification to related users
                    linkIA = "/custom/IrregAlt/IAEdit.aspx?Id=" + lblId.Text;
                    //NotificationAppRepository.SaveForNotification(user, message, linkIA, "");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                //List<IATask> data = IrregAltRepository.checkIATaskStatus(lblId.Text.ToInt32(0)); //Check data if has open task
                //if (data.Count != 0)
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please make sure all task already close !')", true);
                //else
                //{
                //    sdsIAHeader.UpdateCommand = "Update IAs set StatusId = 2 where Id = " + lblId.Text; // set close IA
                //    sdsIAHeader.Update();

                //    //define email to Author
                //    //string approver = cmbApproverUser.Text;
                //    string author = UserRepository.RetrieveEmailByUserId(IrregAltRepository.getAuthorIdByIA(lblId.Text.ToInt32(0)));
                //    string link = HttpContext.Current.Request.Url.AbsoluteUri;

                //    //DotWeb.Utils.EmailNotification.NotificationforAuthor(author, link, lblId.Text.ToInt32(0), approver, "IA");

                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Irregular Alteration has closed !')", true);
                //    //btnClose.Visible = false;
                //    StatusIA.Text = "Close";
                //}
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // string Sectionname = AssemblySectionId.Text;
                if (cmbIAType.Value.ToString() == "-Select-")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Select Type !');", true);
                    return;
                }
                if (cmbModels.Value.ToString() == "-Select-")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Select Model !');", true);
                    return;
                }
                //if (gvStations.VisibleRowCount == 0)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed to save data, Station cannot be empty !')", true);
                //}
                if (cmbArea.Value.ToString() == "-Select-")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Select Area !');", true);
                    return;
                } if (cmbStation.Value.ToString() == "-Select-")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Please Select Station !');", true);
                    return;
                }
                else
                {
                    //DeleteDraft(txtNumber.Text);
                    //sdsIAHeader.UpdateParameters["Id"].DefaultValue = lblId.Text;
                    //sdsIAHeader.Update();

                    int cmbIAType_ = cmbIAType.SelectedItem == null ? 0 : int.Parse(cmbIAType.SelectedItem.Value.ToString());
                    int Id = lblId.Text.ToInt32(0);
                    string htDescription_ = htDescription.Html;
                    string validfrom = dtValidFrom.Text;
                    string validto = dtValidTo.Text;

                    //insert into Ias
                    IrregAltRepository.UpdateIAs(
                        Id,
                        cmbIAType_,
                        txtNumber.Text,
                        txtInfoNumber.Text,
                        htDescription_,
                        validfrom,
                        validto,
                        ImplementationDate.Text
                        );

                    //insert into iaparts
                    var GetIdIA = Request.QueryString["id"].ToInt32(0);
                    var getDataIAPart = IrregAltRepository.RetrieveIAPartByIAId(GetIdIA);
                    var getDataIAModel = IrregAltRepository.RetrieveIAModelByIAId(GetIdIA);
                    var getDataIAStation = IrregAltRepository.RetrieveIAStationByIAId(GetIdIA);

                    if (getDataIAPart != null)
                    {
                        IrregAltRepository.UpdateIAParts(
                           Id,
                           txtPart.Text,
                           DescriptionPart.Text
                           );
                    }
                    else
                    {
                        InsertIAParts(Id, txtPart.Text, DescriptionPart.Text);
                    }

                    //insert into iamodels

                    int cmbModels_ = cmbModels.Value.ToInt32(0);
                    if (getDataIAModel != null)
                    {
                        IrregAltRepository.UpdateIAModels(
                             Id,
                             cmbModels_
                             );
                    }
                    else
                    {
                        InsertIAModel(Id, cmbModels_);
                    }

                    //insert area and station

                    int cmbArea_ = cmbArea.Value.ToInt32(0);
                    int cmbStation_ = cmbStation.Value.ToInt32(0);
                    if (getDataIAStation != null)
                    {
                        IrregAltRepository.UpdateIAStation(
                           Id,
                           cmbArea_,
                           cmbStation_
                           );
                    }
                    else
                    {
                        InsertIAStation(Id, cmbArea_, cmbStation_);
                    }

                    var IAId = lblId.Text;

                    //***** SaveFileAttachmentIA ****//
                    int id = Request.QueryString["id"].ToInt32(0);
                    if (id < 1)
                    {
                        int sIAId = Convert.ToInt32(IAId);
                        var SumRowData = GVDataAttachment.VisibleRowCount;
                        List<IAAttachments> ListAttachFile = new List<IAAttachments>();
                        for (int i = 0; i < SumRowData; i++)
                        {
                            IAAttachments AttachFile = new IAAttachments();
                            AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                            AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FilePath").ToString();

                            ListAttachFile.Add(AttachFile);
                        }
                        for (int x = 0; x < ListAttachFile.Count(); x++)
                        {
                            IrregAltRepository.InsertAttachMentIA(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, sIAId);
                        }
                    }
                    else
                    {
                        int sIAId = Convert.ToInt32(IAId);
                        var SumRowData = GVDataAttachment.VisibleRowCount;
                        List<IAAttachments> ListAttachFile = new List<IAAttachments>();
                        for (int i = 0; i < SumRowData; i++)
                        {
                            IAAttachments AttachFile = new IAAttachments();
                            AttachFile.FileName = GVDataAttachment.GetRowValues(i, "FileName").ToString();
                            AttachFile.FilePath = GVDataAttachment.GetRowValues(i, "FilePath").ToString();
                            ListAttachFile.Add(AttachFile);
                        }
                        for (int x = 0; x < ListAttachFile.Count(); x++)
                        {
                            IrregAltRepository.InsertAttachMentIA(ListAttachFile[x].FileName, ListAttachFile[x].FilePath, sIAId);
                        }
                    }
                    // ***** EndProcess ***** //
                    // ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data " + btnSave.Text + "d successfully !')", true);

                    ReadOnlyForm();
                    btnEditIA.Visible = true;
                    int idheader = Request.QueryString["id"].ToInt32(0);
                    if (idheader > 0)
                    {
                        LoadData();
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data " + btnSave.Text + "d successfully !')", true);

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data " + btnSave.Text + "d successfully !')", true);
                        Page.Response.Redirect("/custom/IAs/IrregularAlterationPage.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                errorMessageLabel.Text = ex.Message;
                errorMessageLabel.Visible = true;
            }
        }
        protected void btnEditIA_Click(object sender, EventArgs e)
        {
            try
            {
                lblHeader.Text = "Irregular Alteration - Edit";
                ReadWriteForm();
                cmbIAType.Enabled = false;
                txtNumber.Enabled = false;

                bool isApproved = IrregAltRepository.getisApproved(lblId.Text.ToInt32(0));
                bool isReadyForApproval = IrregAltRepository.getisReadyForApproval(lblId.Text.ToInt32(0));
                int userId = ((User)Session["user"]).Id;
                int authorId = UserRepository.RetrieveUserIdByUserName(lblUsername_.Text);// txtAuthorUserId.Text.ToInt32(0);            

            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //DeleteDraft(lblId.Text.ToInt32(0));
            Page.Response.Redirect("/custom/IAs/IrregularAlterationPage.aspx");
        } // Back to View IA page
        protected void btnNotify_Click(object sender, EventArgs e)
        {
            try
            {
                string penerima;
                string link = HttpContext.Current.Request.Url.AbsoluteUri;

                for (int i = 0; i < gvAffectedDepart.VisibleRowCount; i++)
                {
                    // send email to assign manager
                    penerima = UserRepository.RetrieveEmailByUserId(Convert.ToInt32(gvAffectedDepart.GetRowValues(i, "AssignedUserId").ToString()));
                    if (!string.IsNullOrEmpty(penerima)) { }
                    //DotWeb.Utils.EmailNotification.Notify(penerima, link, lblId.Text.ToInt32(0));

                    // send email to delegate user
                    int delegateUserId = gvAffectedDepart.GetRowValues(i, "DelegateUserId").ToInt32(0);
                    if (delegateUserId != 0)
                    {
                        penerima = UserRepository.RetrieveEmailByUserId(Convert.ToInt32(gvAffectedDepart.GetRowValues(i, "DelegateUserId").ToString()));
                        //DotWeb.Utils.EmailNotification.Notify(penerima, link, lblId.Text.ToInt32(0));
                    }
                }

                // send email to Approver
                //int approverId = cmbApproverUser.Value.ToInt32(0);
                //if (approverId != 0)
                //{
                //    penerima = UserRepository.RetrieveEmailByUserId(approverId);
                //    //DotWeb.Utils.EmailNotification.Notify(penerima, link, lblId.Text.ToInt32(0));
                //}

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Notify data changed successfully !');", true);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnCloseTask_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;
            try
            {
                int ValueId = gvAffectedDepart.GetRowValues(index, "Id") == null ? 0 : int.Parse(gvAffectedDepart.GetRowValues(index, "Id").ToString());
                Session["IdTask"] = ValueId;
                int userId = ((User)Session["user"]).Id;
                int IATaskId = IrregAltRepository.getIATaskIdbyAssignManage(ValueId, userId);
                string status = IrregAltRepository.getLastStatusIATaksReport(ValueId, userId);
                int CekUserDeleget = IrregAltRepository.getDelegateID(ValueId);
                int getUserAssign = IrregAltRepository.getUserAssignID(ValueId);
                //if (CekUserDeleget == 0 || CekUserDeleget == null)
                //{
                //try
                //{
                //    sdsIATask.UpdateCommand = "Update IATasks set IATaskStatusId = 2, CloseDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where Id = " + IATaskId;
                //    sdsIATask.Update();
                //    int id = lblId.Text.ToInt32(0);
                //    int getauthorId = IrregAltRepository.getauthorbyId(id);
                //    string getEmailApprvManager = txtEmailManager.Text;
                //    string getApprovalManager = IrregAltRepository.GetApprovalManager();
                //    var GetApprovalManagerId = IrregAltRepository.getApprovalManagerId(getEmailApprvManager, getApprovalManager);
                //    //Send Email feedback to author
                //    IrregAltRepository.SendNotifForAuthor(getUserAssign, getauthorId, id);
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Task Irregular Alteration closed !')", true);
                //    LoadData();

                //    List<IATask> data = IrregAltRepository.checkIATaskStatus(lblId.Text.ToInt32(0)); //Check data if has open task
                //    if (data.Count == 0)
                //    {
                //        //untuk update status jadi 3 (Close di IA)
                //        sdsIAHeader.UpdateCommand = "Update IAs set StatusId = 3 where Id = " + lblId.Text;
                //        sdsIAHeader.Update();

                //        //Send Email feedback to author
                //        IrregAltRepository.SendNotifAllTaskCloseForAuthor(getauthorId, getEmailApprvManager.ToString(), id);
                //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Task Irregular Alteration closed !')", true);
                //        LoadData();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    LoggerHelper.LogError(ex);
                //}

                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Task Irregular Alteration has closed !')", true);

                //}
                //else
                //{
                if (status == "Close" || status == "2")
                {
                    try
                    {
                        sdsIATask.UpdateCommand = "Update IATasks set IATaskStatusId = 2, CloseDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where Id = " + IATaskId;
                        sdsIATask.Update();
                        //Cek The Last close task
                        List<IATask> data = IrregAltRepository.checkIATaskStatus(lblId.Text.ToInt32(0)); //Check data if has open task
                        if (data.Count == 0)
                        {
                            //sdsIAHeader.UpdateCommand = "Update IAs set StatusId = 3, __IsDraft = 0, IsApproved=1 where Id = " + lblId.Text;
                            //sdsIAHeader.Update();

                            int id = lblId.Text.ToInt32(0);
                            ReleaseIA(id); //Update Ias to release
                            int getauthorId = IrregAltRepository.getauthorbyId(id);
                            string getEmailApprvManager = lblEmailManager.Text;
                            string getApprovalManager = IrregAltRepository.GetApprovalManager();
                            var GetApprovalManagerId = IrregAltRepository.getApprovalManagerId(getEmailApprvManager, getApprovalManager);

                            //Send Email feedback to author
                            IrregAltRepository.SendNotifForAuthor(getUserAssign, getauthorId, id);
                            IrregAltRepository.SendNotifAllTaskCloseForAuthor(getauthorId, getEmailApprvManager, id);
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Task Irregular Alteration closed !')", true);
                            LoadData();
                        }

                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Task Irregular Alteration has closed !')", true);
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.LogError(ex);
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please check user delegate can't empty and make sure all task already close !')", true);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }

        }
        protected void btnOpenTask_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            var index = container.VisibleIndex;

            try
            {
                int ValueId = gvAffectedDepart.GetRowValues(index, "Id") == null ? 0 : int.Parse(gvAffectedDepart.GetRowValues(index, "Id").ToString());
                Session["IdTask"] = ValueId;
                int userId = ((User)Session["user"]).Id;
                int IATaskId = IrregAltRepository.getIATaskIdbyAssignManage(ValueId, userId);

                sdsIATask.UpdateCommand = "Update IATasks set IATaskStatusId = 1, CloseDate = null where Id = " + IATaskId;
                sdsIATask.Update();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Task Irregular Alteration has opened !')", true);
                LoadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void SendNotif_Click(object sender, EventArgs e)
        {
            try
            {
                List<IATask> data = IrregAltRepository.checkIATaskForNotify(lblId.Text.ToInt32(0)); //Check data if has open task
                if (data.Count > 0)
                {
                    int id = lblId.Text.ToInt32(0);
                    //Send EmailNotify to AssignManager
                    for (int i = 0; i < gvAffectedDepart.VisibleRowCount; i++)
                    {
                        IrregAltRepository.SendNotifForNotify(Convert.ToInt32(gvAffectedDepart.GetRowValues(i, "AssignedUserId").ToString()), id);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Notify Success !');", true);
                    LoadData();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Notify Failed');", true);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnSendToApproved_Click(object sender, EventArgs e)
        {
            int getManagerIdforApproval = 0;
            try
            {
                int getOrgIdFromUser = Convert.ToInt32(((User)Session["user"]).OrganizationId);
                int id = lblId.Text.ToInt32(0);
                //string EmailManager = lblEmailManager.Text;
                int getApprovalManager = IrregAltRepository.GetApprovalManager(getOrgIdFromUser);
                int getSecondManager = IrregAltRepository.GetManager(getOrgIdFromUser);
                //var GetApprovalManagerId = IrregAltRepository.getApprovalManagerId(EmailManager, getApprovalManager);

                //sdsIAHeader.UpdateCommand = "Update IAs set IsReadyForApproval = 1 where Id = " + lblId.Text;
                //sdsIAHeader.Update();
                if (getApprovalManager != 0)
                {
                    getManagerIdforApproval = getApprovalManager;
                }
                else
                {
                    getManagerIdforApproval = getSecondManager;
                }
                UpdateIsReadyForApprovalSend(id); //Update IsReadyForApproval on table IAs
                IrregAltRepository.SendNotifForApproverManager(getManagerIdforApproval, id);
                btnSendToApproved.Text = "Waiting Approver";
                btnSendToApproved.Enabled = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            try
            {
                int? status = IrregAltRepository.getStatusIA(lblId.Text.ToInt32(0));
                if (status != null && status == 1)
                {
                    //sdsIAHeader.UpdateCommand = "Update IAs set StatusId = 2, __IsDraft = 0, IsApproved = 1 where Id = " + lblId.Text;
                    //sdsIAHeader.Update();
                    int id = lblId.Text.ToInt32(0);
                    UpdateIAStatusApproveAndSend(id); //Update Status IA to Inprogress

                    //Send Email to AssignManager ()
                    for (int i = 0; i < gvAffectedDepart.VisibleRowCount; i++)
                    {
                        IrregAltRepository.SendNotifForAsssigned(Convert.ToInt32(gvAffectedDepart.GetRowValues(i, "AssignedUserId").ToString()), id);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data Irregular Alteration has Submitted !');", true);
                    LoadData();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Sorry data has been submitted.');", true);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void BtnSaveVpn_Click1(object sender, EventArgs e)
        {
            var getHeaderId = lblId.Text;
            int GetIdIA = getHeaderId.ToInt32(0);

            IrregAltRepository.DeletePackingMonth(getHeaderId);
            IrregAltRepository.SavePackingMonth(getHeaderId, dates);


        }
        #endregion
        protected void IAHeaders_Modifying(object sender, SqlDataSourceCommandEventArgs e)
        {
            //Utils.AssertNotReadOnly();
        }

        #region GV Stations
        protected void gvStations_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = false;
            //TODO: anything to perform here before inserting to database?
        }

        protected void gvStations_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //pass the reference key
            e.NewValues["IAId"] = lblId.Text;
            e.Cancel = false;
        }

        protected void gvStations_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //TODO: validate entry before submitting

            //foreach (GridViewColumn column in grid.Columns)
            //{
            //    GridViewDataColumn dataColumn = column as GridViewDataColumn;
            //    if (dataColumn == null) continue;
            //    if (e.NewValues[dataColumn.FieldName] == null)
            //    {
            //        e.Errors[dataColumn] = "Value can't be null.";
            //    }
            //}
            //if (e.Errors.Count > 0) e.RowError = "Please, fill all fields.";
            //if (e.NewValues["FirstName"] != null && e.NewValues["FirstName"].ToString().Length < 2)
            //{
            //    AddError(e.Errors, grid.Columns["FirstName"], "First Name must be at least two characters long.");
            //}
            //if (e.NewValues["LastName"] != null && e.NewValues["LastName"].ToString().Length < 2)
            //{
            //    AddError(e.Errors, grid.Columns["LastName"], "Last Name must be at least two characters long.");
            //}
            //if (e.NewValues["Email"] != null && !e.NewValues["Email"].ToString().Contains("@"))
            //{
            //    AddError(e.Errors, grid.Columns["Email"], "Invalid e-mail.");
            //}

            //int age = e.NewValues["Age"] != null ? (int)e.NewValues["Age"] : 0;
            //if (age < 18)
            //{
            //    AddError(e.Errors, grid.Columns["Age"], "Age must be greater than or equal 18.");
            //}
            //DateTime arrival = DateTime.MinValue;
            //DateTime.TryParse(e.NewValues["ArrivalDate"] == null ? string.Empty : e.NewValues["ArrivalDate"].ToString(), out arrival);
            //if (DateTime.Today.Year != arrival.Year || DateTime.Today.Month != arrival.Month)
            //{
            //    AddError(e.Errors, grid.Columns["ArrivalDate"], "Arrival date is required and must belong to the current month.");
            //}

            //if (string.IsNullOrEmpty(e.RowError) && e.Errors.Count > 0) e.RowError = "Please, correct all errors.";
        }

        protected void gvStations_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (!grid.IsNewRowEditing)
            {
                grid.DoRowValidation();
            }
        }

        protected void gvStations_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            string getData = IrregAltRepository.getStationAndArea(lblId.Text.ToInt32(0));

            if (e.Column.FieldName == "StationId")
            {
                var cmb = (ASPxComboBox)e.Editor;
                if (getData == null || getData == "")
                {
                    cmb.DataSourceID = "";
                    cmb.DataSource = sdsStationLookup1;
                    cmb.DataBind();
                }
                else
                {
                    cmb.DataSourceID = "";
                    cmb.DataSource = sdsStationLookup;
                    cmb.Callback += new CallbackEventHandlerBase(cmbStation_Callback);

                    var grid = e.Column.Grid;
                    if (!cmb.IsCallback && !grid.IsNewRowEditing)
                    {
                        //entering edit mode
                        var orgId = -1;
                        //orgId = (int)grid.GetRowValues(e.VisibleIndex, "AssemblySectionId");
                        int a = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "AssemblySectionId"));
                        orgId = a;
                        //set the filter
                        SqlDataSource ds = cmb.DataSource as SqlDataSource;
                        if (ds == null)
                        {
                            ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
                        }
                        ds.SelectParameters["AssemblySectionId"].DefaultValue = orgId.ToString();

                        cmb.DataBind();
                    }
                }
            }

        }
        #endregion

        #region GV Affected Parts
        protected void gvAffectedPart_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = false;
            //TODO: anything to perform here before inserting to database?
        }

        protected void gvAffectedPart_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //pass the reference key
            e.NewValues["IAId"] = lblId.Text;
            e.Cancel = false;

            string AlreadyPartNumber = e.NewValues["PartNumber"] as string;
            string AlreadyDescription = e.NewValues["Description"] as string;

            ASPxGridView grid = (ASPxGridView)sender;
            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                string NewPartNumber = grid.GetRowValues(i, "PartNumber") as string;
                string NewDescription = grid.GetRowValues(i, "Description") as string;
                if (NewPartNumber == AlreadyPartNumber && NewDescription == AlreadyDescription)
                {
                    throw new Exception("Part Number and Description must be unique");
                }
            }

        }

        protected void gvAffectedPart_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
        }

        protected void gvAffectedPart_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (!grid.IsNewRowEditing)
            {
                grid.DoRowValidation();
            }
        }
        #endregion GV Affected Parts

        #region GV Affected Models
        protected void gvAffectedModel_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = false;
            //TODO: anything to perform here before inserting to database?
        }

        protected void gvAffectedModel_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //pass the reference key
            e.NewValues["IAId"] = lblId.Text;
            e.Cancel = false;

            //ASPxGridView grid = sender as ASPxGridView;
            //ASPxComboBox txtProcessNo = grid.FindEditFormEditor("ModelName") as ASPxComboBox;
            //var cekModel = IrregAltRepository.cekModel(lblId.Text.ToInt32(0), txtProcessNo.ToInt32(0));
            //if (cekModel != 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Catalog Model Already Axist !')", true);
            //}

        }

        protected void gvAffectedModel_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
        }

        protected void gvAffectedModel_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (!grid.IsNewRowEditing)
            {
                grid.DoRowValidation();
            }
        }

        protected void gvAffectedModel_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            string getDataIAModels = IrregAltRepository.getIAModels(lblId.Text.ToInt32(0));

            if (e.Column.FieldName == "CatalogModelId")
            {
                var cmb = (ASPxComboBox)e.Editor;
                if (getDataIAModels == null || getDataIAModels == "")
                {
                    cmb.DataSourceID = null;
                    cmb.DataSource = sdsModelLookup;
                    cmb.DataBind();
                }
                else
                {
                    cmb.DataSourceID = "";
                    cmb.DataSource = sdsModelLookupEdit;
                    cmb.DataBind();
                }
            }

            //if (e.Column.FieldName == "ModelId")
            //{
            //    var cmb = (ASPxComboBox)e.Editor;
            //    cmb.DataSourceID = "";
            //    //cmb.DataSource = sdsModelLookupByTypeId;
            //    cmb.Callback += new CallbackEventHandlerBase(cmbModel_Callback);

            //    var grid = e.Column.Grid;
            //    if (!cmb.IsCallback && !grid.IsNewRowEditing)
            //    {
            //        //entering edit mode
            //        var typeId = -1;
            //        //if (!grid.IsNewRowEditing)
            //        //{
            //        typeId = (int)grid.GetRowValues(e.VisibleIndex, "TypeId");

            //        //set the filter
            //        SqlDataSource ds = cmb.DataSource as SqlDataSource;
            //        if (ds == null)
            //        {
            //            ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            //        }
            //        ds.SelectParameters["TypeId"].DefaultValue = typeId.ToString();
            //        //}

            //        //rebind the combobox
            //        cmb.DataBind();
            //    }

            //}
            //if (e.Column.FieldName == "VariantId")
            //{
            //    var cmb = (ASPxComboBox)e.Editor;
            //    cmb.DataSourceID = "";
            //    cmb.DataSource = sdsVariantLookupByModelId;
            //    cmb.Callback += new CallbackEventHandlerBase(cmbVariant_Callback);

            //    //if (!cmb.IsCallback)
            //    //{
            //    //    cmb.DataBind();
            //    //}

            //    var grid = e.Column.Grid;
            //    if (!cmb.IsCallback && !grid.IsNewRowEditing)
            //    {
            //        //entering edit mode
            //        var modelId = -1;
            //        //if (!grid.IsNewRowEditing)
            //        //{
            //        modelId = (int)grid.GetRowValues(e.VisibleIndex, "ModelId");

            //        //set the filter
            //        SqlDataSource ds = cmb.DataSource as SqlDataSource;
            //        if (ds == null)
            //        {
            //            ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            //        }
            //        ds.SelectParameters["ModelId"].DefaultValue = modelId.ToString();
            //        //}

            //        //rebind the combobox
            //        cmb.DataBind();
            //    }
            //    //else
            //    //{
            //    //    //TODO
            //    //    cmb.DataBind();
            //    //}
            //}
        }

        private void cmbVariant_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxComboBox cmb = sender as ASPxComboBox;

            int modelId = e.Parameter.ToInt32(0);

            //set the filter
            SqlDataSource ds = cmb.DataSource as SqlDataSource;
            if (ds == null)
            {
                ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            }
            ds.SelectParameters["ModelId"].DefaultValue = modelId.ToString();

            //rebind the combobox
            if (modelId == 0)
            {
                cmb.DataSourceID = null;
                cmb.DataSource = sdsVariantLookup;
                cmb.DataBind();
            }
            else
                cmb.DataBind();
        }
        private void cmbModel_Callback(object sender, CallbackEventArgsBase e)
        {
            //ASPxComboBox cmb = sender as ASPxComboBox;

            //int typeId = e.Parameter.ToInt32(0);

            ////set the filter
            //SqlDataSource ds = cmb.DataSource as SqlDataSource;
            //if (ds == null)
            //{
            //    ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            //}
            //ds.SelectParameters["TypeId"].DefaultValue = typeId.ToString();

            ////rebind the combobox
            //if (typeId == 0)
            //{
            //    cmb.DataSourceID = null;
            //    cmb.DataSource = sdsModelLookup;
            //    cmb.DataBind();
            //}
            //else
            //    cmb.DataBind();
        }
        #endregion GV Affected Models

        #region GV Attachments
        protected void gvAttachment_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = false;
            //TODO: anything to perform here before inserting to database?
        }
        protected string SaveToUploadFolder(string filepath, string filename)
        {
            string tempdir = Server.MapPath(UPLOAD_DIR);
            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            string tempfile = String.Format("{0}/{1}_{2}_{3}", tempdir, lblId.Text, DateTime.Now.ToString("yyyyMMddhhmmss"), filename);
            if (File.Exists(tempfile))
            {
                //somehow, file already exist
                LoggerHelper.Info("File exist: " + tempfile);
                return null;
            }

            try
            {
                File.Move(filepath, tempfile);
            }
            catch (Exception e)
            {
                //ignore
                LoggerHelper.LogError(e);
                return null;
            }

            return tempfile.Replace(HttpContext.Current.Server.MapPath("~/"), "~/").Replace(@"\", "/");
        }


        protected void gvAttachment_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
        }

        protected void gvAttachment_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (!grid.IsNewRowEditing)
            {
                grid.DoRowValidation();
            }
        }

        protected void gvAttachment_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //TODO
        }

        protected void ucAttachment_Init(object sender, EventArgs e)
        {
            //ASPxUploadControl uploader = (ASPxUploadControl)sender;
            //ucAttachmentReq = (ASPxUploadControl)sender;
            ////uploader.FileUploadComplete += new EventHandler<FileUploadCompleteEventArgs>(ucAttachment_FileUploadComplete);
            //ucAttachmentReq.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE;
            //ucAttachmentReq.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE.BytesToString();
            //ucAttachmentReq.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");
        }

        protected void ucAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.IsValid) return;

            FileInfo file = new FileInfo();
            file.FilePath = SaveToTemporaryFolder(e.UploadedFile);
            file.FileName = e.UploadedFile.FileName;

            Session[lblId2.Text + "_Attachment"] = file;

            e.CallbackData = e.UploadedFile.FileName;
        }
        #endregion GV Attachments

        #region GV Affected Departments
        protected void gvAffectedDepart_DataBound(object sender, EventArgs e)
        {
            User user = Session["user"] as User;
            int userId = 0;
            if (user != null)
                userId = user.Id;

            ASPxGridView Grid = ((ASPxGridView)sender);
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                GridViewColumn dtColTask = Grid.Columns["AssignedUserId"];
                DataRowView row = Grid.GetRow(i) as DataRowView;
                int assignedUserId = row["AssignedUserId"].ToInt32(0);
                int delegateUserId = row["DelegateUserId"].ToInt32(0);
                int iaid = Request.QueryString["id"].ToInt32(0);


                if (iaid == 0)
                {
                    Grid.ExpandRow(i);
                }
                else if (i != 0)
                {
                    if (userId == assignedUserId || userId == delegateUserId)
                    {
                        Grid.ExpandRow(i);
                        //Grid.DetailRows.ExpandRow(i);
                    }
                }
            }
        }
        protected void gvAffectedDepart_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            User user = Session["user"] as User;
            int userId = 0;
            if (user != null)
                userId = user.Id;

            ASPxGridView Grid = ((ASPxGridView)sender);
            for (int i = 1; i < Grid.VisibleRowCount; i++)
            {

                DataRowView row = Grid.GetRow(i) as DataRowView;
                int assignedUserId = row["AssignedUserId"].ToInt32(0);
                int delegateUserId = row["DelegateUserId"].ToInt32(0);


                if (e.DataColumn.FieldName == "CloseDate")
                {
                    ASPxButton textBox_ = gvAffectedDepart.FindRowCellTemplateControl(i, null, "lnkTest") as ASPxButton;
                    ASPxButton textBox2_ = gvAffectedDepart.FindRowCellTemplateControl(i, null, "lnkTest2") as ASPxButton;
                    string date = "";
                    //Convert object to string 
                    if (e.CellValue.ToString() == string.Empty)
                    {
                        date = string.Empty;
                    }
                    else
                    {
                        date = ((DateTime)e.CellValue).ToString("dd/MM/yyyy");
                    }

                    //condition
                    if (e.CellValue.ToString() == string.Empty && textBox_ != null && textBox2_ != null)
                    {
                        if (StatusIA.Text == "Draft")
                        {
                            if (userId == assignedUserId)
                            {
                                textBox_.Visible = false;
                                textBox2_.Visible = false;
                            }
                            else if (userId == delegateUserId)
                            {
                                textBox_.Visible = false;
                                textBox2_.Visible = false;
                            }
                        }
                        else if (StatusIA.Text == "Open")
                        {
                            if (userId == assignedUserId)
                            {
                                textBox_.Visible = false;
                                textBox2_.Visible = true;
                            }
                            else if (userId == delegateUserId)
                            {
                                textBox_.Visible = true;
                                textBox_.CssClass = "blink";
                                textBox_.Enabled = true;
                                textBox2_.Visible = true;
                            }

                        }
                        else if (StatusIA.Text == "Close")
                        {
                            if (userId == assignedUserId)
                            {
                                textBox_.Visible = false;
                                textBox2_.Visible = false;
                            }
                            else if (userId == delegateUserId)
                            {
                                textBox_.Visible = false;
                                textBox2_.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (textBox_ != null && textBox2_ != null)
                        {
                            string date_ = date + " | View Report";
                            textBox2_.Text = date_;
                            textBox2_.Visible = true;
                            textBox_.Visible = false;
                        }
                    }
                }
            }  //else if (e.DataColumn.FieldName == "FlagReport")
            //{
            //    ASPxButton textBox = gvAffectedDepart.FindRowCellTemplateControl(i, null, "lnkTest") as ASPxButton;
            //    ASPxButton textBox2 = gvAffectedDepart.FindRowCellTemplateControl(i, null, "lnkTest2") as ASPxButton;


            //    if (StatusIA.Text == "Draft")
            //    {
            //        textBox.Visible = false;
            //        textBox2.Visible = false;
            //    }
            //    else if (StatusIA.Text == "Open")
            //    {
            //        if (userId == delegateUserId)
            //        {
            //            textBox.Visible = true;
            //            textBox.CssClass = "blink";
            //            textBox.Enabled = true;
            //            textBox2.Visible = true;
            //        }
            //        else
            //        {
            //            textBox.Visible = false;
            //            textBox2.Visible = true;
            //        }
            //    }
            //    else if (StatusIA.Text == "Close")
            //    {
            //        textBox.Visible = false;
            //        textBox2.Visible = true;
            //    }

            //}

        }
        protected void gvAffectedDepart_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;

            int userId = ((User)Session["user"]).Id;
            int IAid = lblId.Text.ToInt32(0);

            try
            {
                for (int i = 1; i < grid.VisibleRowCount; i++)
                {
                    GridViewColumn dtColTask = grid.Columns["Task"];
                    DataRowView row = grid.GetRow(i) as DataRowView;
                    int assignedUserId = row["AssignedUserId"].ToInt32(0);
                    int delegateUserId = row["DelegateUserId"].ToInt32(0);
                    int id = row["Id"].ToInt32(0);
                    DateTime? reportdate = row["CloseDate"].ToDateTime();
                  
                    string GetIAReportStatus = IrregAltRepository.getLastStatusIATaksReport(id, assignedUserId);
                    string statusTaskReport = "";
                    if (GetIAReportStatus.ToInt32(0) == 2)
                    {
                        statusTaskReport = "Close";
                    }

                    if (e.RowType != GridViewRowType.Data) return;

                    ASPxButton btnclose = grid.FindRowCellTemplateControl(i, null, "btnCloseTask") as ASPxButton;
                    //ASPxButton btnopen = grid.FindRowCellTemplateControl(i, null, "btnOpenTask") as ASPxButton;
                    ASPxButton StatusOpen = grid.FindRowCellTemplateControl(i, null, "BtnStatusOpen") as ASPxButton;
                    ASPxButton StatusClose = grid.FindRowCellTemplateControl(i, null, "BtnStatusClose") as ASPxButton;

                    if (userId == assignedUserId && reportdate == null && StatusIA.Text == "Open")
                    {
                        if (statusTaskReport == "Close")
                        {
                            btnclose.Visible = true;
                            btnclose.Enabled = true;
                            btnclose.CssClass = "btn btn-danger btn-sm blink";
                            btnclose.Native = true;
                            StatusOpen.Visible = false;
                            StatusClose.Visible = false;
                        }
                        else
                        {
                            btnclose.Visible = false;
                            StatusOpen.Visible = true;
                        }

                    }
                    else if (userId == assignedUserId && reportdate != null && StatusIA.Text == "Open")
                    {
                        StatusClose.Visible = true;
                    }
                    else if (userId == assignedUserId && reportdate != null && StatusIA.Text == "Close")
                    {
                        StatusClose.Visible = true;
                    }
                    else if (userId != assignedUserId && reportdate == null && StatusIA.Text == "Open")
                    {
                        btnclose.Visible = false;
                        StatusOpen.Visible = true;
                    }
                    else if (userId != assignedUserId && reportdate != null && StatusIA.Text == "Close")
                    {
                        btnclose.Visible = false;
                        StatusClose.Visible = true;
                    }
                    else if (userId != assignedUserId && reportdate != null && StatusIA.Text == "Open")
                    {
                        btnclose.Visible = false;
                        StatusClose.Visible = true;
                    }

                    else
                    {
                        btnclose.Visible = false;
                        StatusClose.Visible = false;
                        StatusOpen.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void gvAffectedDepart_Init(object sender, EventArgs e)
        {
            User user = Session["user"] as User;
            int userId = 0;
            int authorId;
            if (user != null)
                userId = user.Id;

            if (Request.QueryString.Count > 0)
            {
                int IAid = Request.QueryString["id"].ToInt32(0);
                authorId = IrregAltRepository.getAuthorIdByIA(IAid);

                if (userId == authorId)
                {
                    gvAffectedDepart.DataColumns["OrganizationId"].EditFormSettings.Visible = DefaultBoolean.True;
                    gvAffectedDepart.DataColumns["AssignedUserId"].EditFormSettings.Visible = DefaultBoolean.True;
                    gvAffectedDepart.DataColumns["Task"].EditFormSettings.Visible = DefaultBoolean.True;
                }
            }
            else
            {
                gvAffectedDepart.DataColumns["OrganizationId"].EditFormSettings.Visible = DefaultBoolean.True;
                gvAffectedDepart.DataColumns["AssignedUserId"].EditFormSettings.Visible = DefaultBoolean.True;
                gvAffectedDepart.DataColumns["Task"].EditFormSettings.Visible = DefaultBoolean.True;
            }
        }

        protected void gvAffectedDepart_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            e.Cancel = false;
            //e.NewValues["DelegateUserId"] = "";
            //TODO: anything to perform here before inserting to database?
            string DelegateId = e.NewValues["DelegateUserId"].ToString();
            if (DelegateId == null || DelegateId == "")
                return;

            if (!string.IsNullOrEmpty(DelegateId))
            {
                e.NewValues["DelegateDate"] = DateTime.Now;

                //Send Email to Delegate
                int id = lblId.Text.ToInt32(0);
                try
                {
                    IrregAltRepository.SendNotifForDelegate(DelegateId.ToInt32(0), id);
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogError(ex);
                }
            }
        }

        protected void gvAffectedDepart_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //pass the reference key
            e.NewValues["IAId"] = lblId.Text;
            e.Cancel = false;
        }

        protected void gvAffectedDepart_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
        }

        protected void gvAffectedDepart_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (!grid.IsNewRowEditing)
            {
                grid.DoRowValidation();
            }
        }

        protected void gvAffectedDepart_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            User user = Session["user"] as User;
            int userId = 0;
            int authorId = UserRepository.RetrieveUserIdByUserName(lblUsername_.Text);
            if (user != null)
            {
                userId = user.Id;
            }

            if (e.Column.FieldName == "OrganizationId")
            {

                var cmb = (ASPxComboBox)e.Editor;
                cmb.DataSourceID = "";
                cmb.DataSource = sdsOrganizationLookup; //sdsOrganizationLookupEdit;
                cmb.ReadOnly = true;
            }
            else if (e.Column.FieldName == "AssignedUserId")
            {
                var cmb = (ASPxComboBox)e.Editor;
                cmb.DataSourceID = "";
                cmb.DataSource = sdsUserLookupByOrganizationId;
                cmb.Callback += new CallbackEventHandlerBase(cmbAssignedUser_Callback);

                var grid = e.Column.Grid;
                if (!cmb.IsCallback && !grid.IsNewRowEditing)
                {
                    //entering edit mode
                    var orgId = -1;
                    orgId = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "OrganizationId"));

                    //set the filter
                    SqlDataSource ds = cmb.DataSource as SqlDataSource;
                    if (ds == null)
                    {
                        ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
                    }
                    ds.SelectParameters["OrganizationId"].DefaultValue = orgId.ToString();

                    cmb.DataBind();
                }

                int id = Request.QueryString["id"].ToInt32(0);
                if (id > 0)
                {
                    int AssignId = IrregAltRepository.getAssignId(lblId.Text.ToInt32(0), userId);
                    if (AssignId == userId)
                        cmb.Enabled = false;
                    else if (userId == authorId)
                        cmb.Visible = true;
                    else
                        cmb.Enabled = true;
                }
                else
                    cmb.Enabled = true;
            }

            else if (e.Column.FieldName == "DelegateUserId")
            {
                var cmb = (ASPxComboBox)e.Editor;
                cmb.DataSourceID = "";
                cmb.DataSource = sdsUserDelegateLookupByOrganizationId;
                cmb.Callback += new CallbackEventHandlerBase(cmbDelegateUser_Callback);

                var grid = e.Column.Grid;

                if (grid.IsNewRowEditing)
                {
                    //e.Editor.Caption = "";
                    e.Editor.ClientVisible = true;
                }

                if (!cmb.IsCallback && !grid.IsNewRowEditing)
                {
                    //entering edit mode
                    var orgId = -1;
                    orgId = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "OrganizationId"));

                    //set the filter
                    SqlDataSource ds = cmb.DataSource as SqlDataSource;
                    if (ds == null)
                    {
                        ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
                    }
                    ds.SelectParameters["OrganizationId"].DefaultValue = orgId.ToString();

                    cmb.DataBind();
                }

                int id = Request.QueryString["id"].ToInt32(0);
                if (id > 0)
                {
                    int AssignId = IrregAltRepository.getAssignId(lblId.Text.ToInt32(0), userId);
                    if (AssignId == userId)
                        cmb.Enabled = true;
                    else
                        cmb.Enabled = false;
                }
                else
                    cmb.Visible = false;

            }

        }

        private void cmbAssignedUser_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxComboBox cmb = sender as ASPxComboBox;

            int orgId = e.Parameter.ToInt32(0);

            //set the filter
            SqlDataSource ds = cmb.DataSource as SqlDataSource;
            if (ds == null)
            {
                ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            }
            ds.SelectParameters["OrganizationId"].DefaultValue = orgId.ToString();

            //rebind the combobox
            cmb.DataBind();
            if (cmb.Items.Count > 0)
            {
                cmb.SelectedIndex = 0;
            }
        }

        private void cmbDelegateUser_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxComboBox cmb = sender as ASPxComboBox;

            int orgId = e.Parameter.ToInt32(0);

            //set the filter
            SqlDataSource ds = cmb.DataSource as SqlDataSource;
            if (ds == null)
            {
                ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            }
            ds.SelectParameters["OrganizationId"].DefaultValue = orgId.ToString();

            //rebind the combobox
            cmb.DataBind();
        }

        protected void gvAffectedDepart_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            User user = Session["user"] as User;
            int userId = 0;
            int authorId = IrregAltRepository.getAuthorIdByIA(lblId.Text.ToInt32(0));
            if (user != null)
                userId = user.Id;

            try
            {

                if (e.VisibleIndex == -1) return;

                switch (e.ButtonType)
                {
                    case ColumnCommandButtonType.Edit:
                        e.Visible = IATask_EditButtonVisibleCriteria((ASPxGridView)sender, e.VisibleIndex);
                        if (userId != authorId)
                        {
                            e.Text = "Delegate";
                            e.Styles.Style.CssClass = "blink";
                        }
                        else
                            if (StatusIA.Text == "Open")
                            {
                                e.Text = "Task";
                            }
                            else
                            {
                                e.Text = "Edit";
                            }
                        break;

                    case ColumnCommandButtonType.Update:
                        e.Visible = IATask_EditButtonVisibleCriteria((ASPxGridView)sender, e.VisibleIndex);
                        if (userId != authorId)
                            e.Text = "Send";
                        else
                            e.Text = "Update";
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }

        }

        protected bool IATask_EditButtonVisibleCriteria(ASPxGridView Grid, int VisibleIndex)
        {
            User user = Session["user"] as User;
            int userId = 0;
            int authorId = UserRepository.RetrieveUserIdByUserName(lblUsername_.Text);
            // int approverId = 0;


            if (user != null)
                userId = user.Id;
            DataRowView row = Grid.GetRow(VisibleIndex) as DataRowView;
            int assignedUserId = row["AssignedUserId"].ToInt32(0);
            int delegateUserId = row["DelegateUserId"].ToInt32(0);

            if (StatusIA.Text == "Open")
            {
                if (userId == assignedUserId && delegateUserId == 0) // Author can edit
                    return true;
                else
                    return false;
            }
            else if (StatusIA.Text == "Draft" || StatusIA.Text == "Open")
            {
                if (userId == authorId)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
            //else
            //    return false;
        }

        #region Task Reports
        protected void gvTaskReport_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex == -1)
            {
                switch (e.ButtonType)
                {
                    case ColumnCommandButtonType.New:
                        e.Text = "Report";
                        e.Styles.Style.CssClass = "blink";
                        break;
                }
            }
            else
            {
                switch (e.ButtonType)
                {
                    case ColumnCommandButtonType.Update:
                        e.Text = "Send";
                        break;
                }
            }
        }
        protected void gvTaskReport_DataBound(object sender, EventArgs e)
        {
            ASPxGridView Grid = ((ASPxGridView)sender);
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                DataRowView row = Grid.GetRow(i) as DataRowView;
                string FileName = row["FileName"].ToString();

                if (string.IsNullOrEmpty(FileName))
                    (Grid.Columns["FilePath"] as GridViewDataColumn).ReadOnly = true;
            }
        }

        protected void gvTaskReport_BeforePerformDataSelect(object sender, EventArgs e)
        {
            //use by datasource sdsIATaskReport
            //Session["IATaskId"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        }

        protected void gvTaskReport_Init(object sender, EventArgs e)
        {
            //ASPxGridView grid = (sender as ASPxGridView);
            //User user = Session["user"] as User;
            //int userId = 0;
            //if (user != null)
            //    userId = user.Id;

            //int authorId = UserRepository.RetrieveUserIdByUserName(lblUsername_.Text);
            //int taskId = (sender as ASPxGridView).GetMasterRowKeyValue().ToInt32(0);
            //int delegateUserId = gvAffectedDepart.GetRowValuesByKeyValue(taskId, "DelegateUserId").ToInt32(0);
            //int assigneduserId = gvAffectedDepart.GetRowValuesByKeyValue(taskId, "AssignedUserId").ToInt32(0);
            //var GetIdIA = Request.QueryString["id"].ToInt32(0);
            //string status = IrregAltRepository.getLastStatusReport(GetIdIA, assigneduserId); //check last report status

            //if (dtDistributionDate.Value == null)//not approved yet.
            //{
            //    (grid.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            //    (grid.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            //    //(grid.Columns["Command"] as GridViewCommandColumn).ShowEditButton = false;
            //}
            //else
            //{
            //    if (status == "Close" || status == "2") // Status close can't do new item
            //    {
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowEditButton = false;
            //    }

            //    else if (userId == delegateUserId) // just can do new item
            //    {
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = true;
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowEditButton = true;
            //    }

            //    else if (userId == authorId) // author 
            //    {
            //        if (btnEditIA.Visible == true) // cant edit
            //        {
            //            (grid.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            //            (grid.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            //            (grid.Columns["Command"] as GridViewCommandColumn).ShowEditButton = false;
            //        }
            //        else
            //        {
            //            (grid.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            //            (grid.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            //            (grid.Columns["Command"] as GridViewCommandColumn).ShowEditButton = true;
            //        }
            //    }

            //    else
            //    {
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowNewButtonInHeader = false;
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowDeleteButton = false;
            //        (grid.Columns["Command"] as GridViewCommandColumn).ShowEditButton = false;
            //    }
            //}
        }

        protected void gvTaskReport_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ASPxGridView grid = (sender as ASPxGridView);
                //pass the reference key
                e.NewValues["IATaskId"] = (sender as ASPxGridView).GetMasterRowKeyValue();
                e.NewValues["ReportDate"] = DateTime.Now;
                object ReportDate = e.NewValues["ReportDate"];
                object FileName = e.NewValues["FileName"];
                object TaskStatus = e.NewValues["IATaskStatusId"];
                object Report = e.NewValues["Report"];
                object Remainder = e.NewValues["Remainder"];

                //get the file info from session
                if (FileName != null)
                {
                    FileInfo file = Session[lblId.Text + "_TaskAttachment"] as FileInfo;
                    if (file != null && file.FileName != null)
                    {
                        //move the file to the upload page
                        e.NewValues["FilePath"] = SaveTaskToUploadFolder(e.NewValues["IATaskId"].ToInt32(0), file.FilePath, file.FileName);
                    }
                }

                //send email to assignuser
                int id = lblId.Text.ToInt32(0);
                int userDelegateId = ((User)Session["user"]).Id;
                int assignId = IrregAltRepository.getAssignIdByDelegateId(lblId.Text.ToInt32(0), userDelegateId);
                IrregAltRepository.SendNotifForAsssignedFromDelegate(assignId, id);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex.Message);
            }

            e.Cancel = false;
        }

        protected void gvTaskReport_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = false;
            //TODO: delete the old file
        }

        protected void gvTaskReport_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = false;
            //TODO: delete the old file
        }

        protected void ucTaskAttachment_Init(object sender, EventArgs e)
        {
            //ASPxUploadControl uploader = (ASPxUploadControl)sender;
            //uploader.FileUploadComplete += new EventHandler<FileUploadCompleteEventArgs>(ucAttachment_FileUploadComplete);
        }

        protected void ucTaskAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.IsValid) return;

            FileInfo file = new FileInfo();
            file.FilePath = SaveToTemporaryFolder(e.UploadedFile);
            file.FileName = e.UploadedFile.FileName;

            Session[lblId.Text + "_TaskAttachment"] = file;

            e.CallbackData = e.UploadedFile.FileName;

            //ASPxUploadControl uploader = (ASPxUploadControl)sender;
            //Control control = uploader.Parent;
            //ASPxGridView grid = null;

            //while (control != null)
            //{
            //    grid = control as ASPxGridView;
            //    if (grid != null)
            //    {
            //        break;
            //    }
            //    control = control.Parent;
            //}

            //if (grid != null)
            //{
            //    GridViewDataTextColumn col = grid.Columns["FileName"] as GridViewDataTextColumn;


            //    var txt = grid.FindEditRowCellTemplateControl(grid.Columns["FileName"] as GridViewDataColumn, "txtTaskFileName");
            //    if (txt != null)
            //    {
            //        //txt.Text = file.FileName;
            //    }
            //}
        }

        #endregion Task Reports

        #endregion GV Affected Departements
        protected string GetLastStatusIA(int IAId)
        {
            string value = "";

            if (IAId == null || IAId == 0)
            {
                return value;
            }
            else
            {
                using (DbConnection con = ConnectionHelper.GetConnection())
                {
                    con.Open();

                    string sqlCmd = @"SELECT *,case when TextStatus = 'Close' Then 'Close' else COALESCE (aa.LastProgress, aa.TextStatus) end as [LastStatus]
                               FROM   (SELECT DISTINCT IA.Id,
                               (SELECT TOP (1) t5.Name + ' -' + ' ' + CONVERT(varchar(25), t1.CloseDate) AS LastProgress
                               FROM    IATasks t1 INNER JOIN
                                            IATaskReports t2 ON t2.IATaskId = t1.Id INNER JOIN
                                            Organizations t5 ON t5.Id = t1.OrganizationId
                               WHERE IAId = ia.Id and t1.IATaskStatusId = 2
                               ORDER BY t2.ReportDate DESC) AS LastProgress,
							   CASE WHEN (ia.StatusId = 1) THEN 'Draft' ELSE CASE WHEN (ia.StatusId  = 2) THEN 'Open' ELSE CASE WHEN (ia.StatusId  = 3) THEN 'Close' ELSE CASE WHEN (ia.StatusId  = 4) 
                               THEN 'Cancel' ELSE '' END END END END AS TextStatus
                               FROM    IAs ia) aa where aa.Id =" + IAId;
                    //string sqlCmdExecute = string.Format(sqlCmd, IAId);

                    using (DbCommand cmd = con.CreateCommand())
                    {
                        LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                        cmd.Connection = con;
                        cmd.CommandText = sqlCmd;
                        cmd.CommandType = CommandType.Text;
                        IDataReader datareader = null;
                        datareader = cmd.ExecuteReader();
                        while (datareader.Read())
                        {
                            value = datareader["LastStatus"].ToString();
                        }
                    }
                }
                return value;
            }
        }
        void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }
        protected string SaveToTemporaryFolder(UploadedFile file)
        {
            string tempdir = Server.MapPath(TEMPORARY_DIR);
            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            string tempfile = String.Format("{0}/{1}_{2}", tempdir, DateTime.Now.ToString("yyyyMMddhhmmss"), file.FileName);
            if (File.Exists(tempfile))
            {
                //somehow, file already exist
                LoggerHelper.Info("File exist: " + tempfile);
                return null;
            }

            try
            {
                file.SaveAs(tempfile);
            }
            catch (Exception e)
            {
                //ignore
                LoggerHelper.LogError(e);
                return null;
            }

            return tempfile;
        }
        protected string SaveTaskToUploadFolder(int taskid, string filepath, string filename)
        {
            string tempdir = Server.MapPath(UPLOAD_DIR);
            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            string tempfile = String.Format("{0}/{1}_{2}_{3}_{4}", tempdir, lblId.Text, taskid, DateTime.Now.ToString("yyyyMMddhhmmss"), filename);
            if (File.Exists(tempfile))
            {
                //somehow, file already exist
                LoggerHelper.Info("File exist: " + tempfile);
                return null;
            }

            try
            {
                File.Move(filepath, tempfile);
            }
            catch (Exception e)
            {
                //ignore
                LoggerHelper.LogError(e);
                return null;
            }

            return tempfile.Replace(HttpContext.Current.Server.MapPath("~/"), "~/").Replace(@"\", "/");
        }
        protected int CreateDraft(int AssemblyTypeId, int UserId)
        {
            int headerId = 0;
            string EpcNumber = GetInternalEpcNumber(AssemblyTypeId, 1);

            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "insert into IAs (AssemblyTypeId, IATypeId, StatusId, InternalEpcNumber, AuthorUserId, __IsDraft, ValidPeriodTo) "
                                        + "values ('" + AssemblyTypeId + "' ,(select top 1 [Id] from [dbo].[IATypes]),'1', '" + EpcNumber + "' , '" + UserId + "', '1', '2099-12-31 00:00:00.000')";
                string sqlCmdExecute = string.Format(sqlCmd, 1, EpcNumber, UserId);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;

                    cmd.ExecuteScalar();

                    cmd.CommandText = "Select SCOPE_IDENTITY() ID";
                    DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        headerId = Convert.ToInt32(reader["Id"]);
                    }
                    reader.Close();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return headerId;
        }
        protected void DeleteDraft(int IAId)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "delete from IAs where __IsDraft=1 and Id=" + IAId;

                string sqlCmdExecute = string.Format(sqlCmd, IAId);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;

                    cmd.ExecuteNonQuery();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }

        }
        protected void UpdateIsReadyForApprovalSend(int Id)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "Update IAs set IsReadyForApproval = 1 where Id=" + Id;
                string sqlCmdExecute = string.Format(sqlCmd, Id);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected void UpdateIsReadyForApprovalSendBack(int Id)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "Update IAs set IsReadyForApproval = 0 where Id=" + Id;
                string sqlCmdExecute = string.Format(sqlCmd, Id);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected void UpdateIAStatusApproveAndSend(int Id)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "Update IAs set StatusId = 2, __IsDraft = 0, IsApproved = 1 where Id = " + lblId.Text;
                string sqlCmdExecute = string.Format(sqlCmd, Id);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected void ReleaseIA(int Id)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "Update IAs set StatusId = 3, __IsDraft = 0 where Id = " + lblId.Text;
                string sqlCmdExecute = string.Format(sqlCmd, Id);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteScalar();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return;
        }
        protected bool ApproveIA(int HeaderId)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = "update IAs set IsApproved=1, DistributionDate=sysdatetime() where Id = @HeaderId = {0}";

                string sqlCmdExecute = string.Format(sqlCmd, HeaderId);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;

                    cmd.ExecuteNonQuery();

                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return true;
        }
        //protected bool UnapproveIA(int HeaderId)
        //{
        //    using (SqlConnection conn = AppConnection.GetConnection())
        //    {
        //        conn.Open();

        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = conn;
        //        cmd.CommandTimeout = 0;

        //        try
        //        {
        //            cmd.CommandText = "update IAHeaders set IsApproved=0, DistributionDate=NULL where Id=" + HeaderId;
        //            cmd.ExecuteScalar();
        //        }
        //        catch (Exception e)
        //        {
        //            LoggerHelper.LogError(e);
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        protected User getUser(int UserId)
        {
            User user = null;
            using (ScaffoldDb dbContext = new ScaffoldDb())
            {
                user = dbContext.Users.Where(u => u.Id == UserId).FirstOrDefault();
            }
            return user;
        }
        protected void UcAtthmentTask_Init(object sender, EventArgs e)
        {
            ucAttachmentApproval = (ASPxUploadControl)sender;

            ucAttachmentApproval.ValidationSettings.MaxFileSize = AppConfiguration.UPLOAD_MAXFILESIZE_BYTES;
            ucAttachmentApproval.ValidationSettings.MaxFileSizeErrorText = "File size must be <= " + AppConfiguration.UPLOAD_MAXFILESIZE_BYTES.BytesToString();
            ucAttachmentApproval.ValidationSettings.AllowedFileExtensions = AppConfiguration.UPLOAD_FILE_EXTENSIONS.Split(",");

            ucAttachmentApproval.FileUploadComplete += new EventHandler<FileUploadCompleteEventArgs>(ucAttachmentAssignee_FileUploadComplete);
        }
        protected void txtReport_Init(object sender, EventArgs e)
        {
            txtReport = (ASPxTextBox)sender;
        }
        protected void ucAttachmentAssignee_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            //detailGvDepartment = (ASPxGridView)sender;
            try
            {
                //if (e.IsValid == true)
                //    e.CallbackData = this.UploadAttachment(e.UploadedFile, UserRepository.RetrieveUserNameById(int.Parse(Session["UserId"].ToString())), "Task");

            }
            catch (Exception ex)
            {
                e.IsValid = false;
                e.ErrorText = ex.Message;
            }
        }
        protected void cmbIAType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int assemblyTypeId = Session["assemblyType"].ToInt32(0);

            txtNumber.Text = GetInternalEpcNumber(assemblyTypeId, cmbIAType.SelectedItem.Value.ToInt32(0), lblId.Text.ToInt32(0));
        }
        protected string GetInternalEpcNumber(int AssemblyTypeId, int IATypeId, int IAId = 0)
        {
            string EpcNumber = "";
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                con.Open();

                string sqlCmd = " DECLARE @EpcNumber varchar(25) EXEC [usp_GetIAInternalEpcNumber] @AssemblyTypeId = {0} , @IATypeId = {1}, @IAId = {2}, @EpcNumber = @EpcNumber OUTPUT SELECT @EpcNumber as N'@EpcNumber'";

                string sqlCmdExecute = string.Format(sqlCmd, AssemblyTypeId, IATypeId, IAId, EpcNumber);

                using (DbCommand cmd = con.CreateCommand())
                {
                    LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                    cmd.Connection = con;
                    cmd.CommandText = sqlCmdExecute;
                    cmd.CommandType = CommandType.Text;
                    string a = cmd.ExecuteScalar().ToString();



                    //cmd.ExecuteNonQuery();

                    EpcNumber = a;
                    LoggerHelper.Info("Finish SQL Command Process...");
                }
            }
            return EpcNumber;
        }

        // *** UploadFileAttachmentIA *** //
        protected void ShowAttachment_Click(object sender, EventArgs e)
        {
            PopUpAttachMent.ShowOnPageLoad = true;
        }
        protected void PopUpAttachMent_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            PopUpAttachMent.ShowOnPageLoad = false;
        }
        protected int GetIdAttachment()
        {
            int IdData = 0;
            var Id = (ListAttachment.AttachmentModels == null ? 0 : ListAttachment.AttachmentModels.Count()) + 1;
            IdData = Id;
            return IdData;
        }
        static DataTable ConvertListToDataTable(List<IAAttachments> list)
        {
            // New table.
            DataTable table = new DataTable();
            // Get max columns.
            int columns = list.Count();
            // Add columns.
            table.Columns.Add("Id");
            table.Columns.Add("FileName");
            table.Columns.Add("FilePath");
            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array.Id, array.FileName, array.FilePath);
            }
            return table;
        }
        protected void UploadAttachment_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            int id = Request.QueryString["id"].ToInt32(0);
            if (id < 1)
            {
                string PathName = AppConfiguration.UPLOAD_DIR + "/IrregularAlteration/";
                if (!Directory.Exists(Server.MapPath(PathName)))
                {
                    Directory.CreateDirectory(Server.MapPath(PathName));
                }
                string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
                string resultFileName = e.UploadedFile.FileName;
                string resultFileUrl = PathName + "/" + resultFileName;
                string resultFilePath = MapPath(resultFileUrl);
                e.UploadedFile.SaveAs(resultFilePath);

                string name = e.UploadedFile.FileName;
                string url = ResolveClientUrl(resultFileUrl);
                long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
                string sizeText = sizeInKilobytes.ToString() + " KB";
                e.CallbackData = name + "|" + url + "|" + sizeText;

                IAAttachments AttachFile = new IAAttachments();
                AttachFile.Id = GetIdAttachment();
                AttachFile.FileName = resultFileName;
                AttachFile.FilePath = resultFileUrl;
                AttachFileModelList.Add(AttachFile);

                DataTable table = ConvertListToDataTable(AttachFileModelList);
                Session["dtAttachment"] = table;
                Session["Attachment"] = AttachFileModelList;

                PopUpAttachMent.ShowOnPageLoad = false;
                GVDataAttachment.DataSource = Session["Attachment"];
                GVDataAttachment.DataBind();
            }
            else
            {
                string PathName = AppConfiguration.UPLOAD_DIR + "/IrregularAlteration/";
                if (!Directory.Exists(Server.MapPath(PathName)))
                {
                    Directory.CreateDirectory(Server.MapPath(PathName));
                }
                string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
                string resultFileName = e.UploadedFile.FileName;
                string resultFileUrl = PathName + "/" + resultFileName;
                string resultFilePath = MapPath(resultFileUrl);
                e.UploadedFile.SaveAs(resultFilePath);

                string name = e.UploadedFile.FileName;
                string url = ResolveClientUrl(resultFileUrl);
                long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
                string sizeText = sizeInKilobytes.ToString() + " KB";
                e.CallbackData = name + "|" + url + "|" + sizeText;
                var Data = Session["Attachmentx"] as List<IAAttachments>;
                var oldattachImage = Session["Attachmentx"] as List<IAAttachments>;

                if (Data == null)
                {
                    IAAttachments AttachFile = new IAAttachments();
                    AttachFile.Id = GetIdAttachment();
                    AttachFile.FileName = resultFileName;
                    AttachFile.FilePath = resultFileUrl;
                    AttachFileModelList.Add(AttachFile);
                    Session["Attachment"] = AttachFileModelList;
                }
                else
                {
                    IAAttachments AttachFile = new IAAttachments();
                    for (int i = 0; i < oldattachImage.Count; i++)
                    {
                        AttachFile.Id = oldattachImage[i].Id;
                        AttachFile.FileName = oldattachImage[i].FileName;
                        AttachFile.FilePath = oldattachImage[i].FilePath;
                        AttachFileModelList.Add(AttachFile);
                        Session["Attachmentx"] = null;
                    }
                    IAAttachments AttachFilex = new IAAttachments();
                    AttachFilex.Id = GetIdAttachment();
                    AttachFilex.FileName = resultFileName;
                    AttachFilex.FilePath = resultFileUrl;
                    AttachFileModelList.Add(AttachFilex);
                    Session["Attachment"] = AttachFileModelList;
                }
                var DataView = Session["Attachment"] as List<IAAttachments>;
                DataTable table = ConvertListToDataTable(DataView);
                Session["dtAttachment"] = table;
                GVDataAttachment.DataSource = DataView;
                GVDataAttachment.DataBind();
            }
        }
        protected void IdBtndelete_Click(object sender, EventArgs e)
        {
            if (btnSave.Visible == true) // View form first 
            {
                ASPxButton btn = sender as ASPxButton;
                GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
                var Rowindex = container.VisibleIndex;
                int ValueRowId = int.Parse(GVDataAttachment.GetRowValues(Rowindex, "IAId").ToString());
                string ValueRowId2 = GVDataAttachment.GetRowValues(Rowindex, "FileName").ToString();
                var oldattachImage = Session["Attachment"] as List<IAAttachments>;

                if (ValueRowId != null && ValueRowId.ToString() != "0")
                {
                    for (int i = 0; i < oldattachImage.Count; i++)
                    {
                        if (oldattachImage[i].IAId == ValueRowId)
                        {
                            oldattachImage.Remove(oldattachImage[i]);
                            IrregAltRepository.DeleteAttachment(ValueRowId);
                            Session["Attachment"] = oldattachImage;
                        }
                    }
                    GVDataAttachment.DataSource = Session["Attachment"];
                    GVDataAttachment.DataBind();
                }
                else
                {
                    for (int i = 0; i < oldattachImage.Count; i++)
                    {
                        if (oldattachImage[i].FileName == ValueRowId2)
                        {
                            oldattachImage.Remove(oldattachImage[i]);
                            Session["Attachment"] = oldattachImage;
                        }
                    }
                    GVDataAttachment.DataSource = Session["Attachment"];
                    GVDataAttachment.DataBind();
                }
            }
            else
            {
                GVDataAttachment.DataSource = Session["Attachment"];
                GVDataAttachment.DataBind();
            }
        }
        protected void GVDataAttachment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GVDataAttachment.DataSource = Session["Attachment"];
            GVDataAttachment.DataBind();
        }
        private void cmbStation_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxComboBox cmb = sender as ASPxComboBox;

            int SecId = e.Parameter.ToInt32(0);

            //set the filter
            SqlDataSource ds = cmb.DataSource as SqlDataSource;
            if (ds == null)
            {
                ds = Page.FindControl(cmb.DataSourceID) as SqlDataSource;
            }
            ds.SelectParameters["AssemblySectionId"].DefaultValue = SecId.ToString();

            //rebind the combobox
            cmb.DataBind();
        }
        protected void GVDataAttachment_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

            ASPxGridView grid = (ASPxGridView)sender;
            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                DataRowView row = grid.GetRow(i) as DataRowView;
                if (e.RowType != GridViewRowType.Data) return;
                ASPxButton delete = grid.FindRowCellTemplateControl(i, null, "IdBtndelete") as ASPxButton;

                if (lblHeader.Text == "Irregular Alteration - View")
                {
                    delete.Visible = false;
                }
                else if (lblHeader.Text == "Irregular Alteration - Edit" && btnSave.Visible == false)
                {
                    delete.Visible = false;
                }
                else
                {
                    delete.Visible = true;
                }
            }
        }
        protected void callback_Callback(object sender, CallbackEventArgsBase e)
        {
            //string[] parm = e.Parameter.Split(';');

            //if(parm.Length > 1)
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string statuscheckbox = parm[0];
            //    string cpID = parm[1];
            //    string cpManagerUserId = parm[2];
            //    string cpOrganizationId = parm[3];

            //    if(statuscheckbox == "Checked")
            //    {
            //        IrregAltRepository.InsertIaTask(IAId, cpOrganizationId, cpManagerUserId);

            //    }
            //    else
            //    {
            //         IrregAltRepository.DeleteIaTask(IAId, cpOrganizationId);

            //    }
            //    gvAffectedDepart.DataBind();
            //    return;
            //}

            //if (e.Parameter == "cbEIC_ED") //EIC_ED
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserId = IrregAltRepository.GetAssignedUserIdEicED();
            //    string getOrgnizationId = IrregAltRepository.GetOrgnizationIdEicED();

            //    if (cbEIC_ED.Checked == true)
            ////    {
            //        IrregAltRepository.InsertEicED(IAId, getOrgnizationId, getAssignedUserId);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbEIC_ED.Checked == false)
            ////    {
            //       IrregAltRepository.DeleteEicED(IAId, getOrgnizationId);
            ////        gvAffectedDepart.DataBind();
            ////        return;
            //    }
            //    else
            //    {
            //        // cbEIC_ED.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbEIC_EL") //Eic_EL
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdEicEL = IrregAltRepository.GetAssignedUserIdEicEL();
            //    string getOrgnizationIdEicEl = IrregAltRepository.GetOrgnizationIdEicEL();

            //    if (cbEIC_EL.Checked == true)
            //    {
            //        IrregAltRepository.InsertEicEL(IAId, getOrgnizationIdEicEl, getAssignedUserIdEicEL);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbEIC_EL.Checked == false)
            //    {
            //        IrregAltRepository.DeleteEicEL(IAId, getOrgnizationIdEicEl);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbEIC_EL.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbAGC")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdAGC = IrregAltRepository.GetAssignedUserIdAGC();
            //    string getOrgnizationIdAGC = IrregAltRepository.GetOrgnizationIdAGC();

            //    if (cbAGC.Checked == true)
            //    {
            //        IrregAltRepository.InsertAGC(IAId, getOrgnizationIdAGC, getAssignedUserIdAGC);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbAGC.Checked == false)
            //    {
            //        IrregAltRepository.DeleteAGC(IAId, getOrgnizationIdAGC);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbAGC.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbQEP_R")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdQEP_R = IrregAltRepository.GetAssignedUserIdQEP_R();
            //    string getOrgnizationIdQEP_R = IrregAltRepository.GetOrgnizationIdQEP_R();

            //    if (cbQEP_R.Checked == true)
            //    {
            //        IrregAltRepository.InsertQEP_R(IAId, getOrgnizationIdQEP_R, getAssignedUserIdQEP_R);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbQEP_R.Checked == false)
            //    {
            //        IrregAltRepository.DeleteQEP_R(IAId, getOrgnizationIdQEP_R);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbQEP_R.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbQEP")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdQEP = IrregAltRepository.GetAssignedUserIdQEP();
            //    string getOrgnizationIdQEP = IrregAltRepository.GetOrgnizationIdQEP();

            //    if (cbQEP.Checked == true)
            //    {
            //        IrregAltRepository.InsertQEP(IAId, getOrgnizationIdQEP, getAssignedUserIdQEP);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbQEP.Checked == false)
            //    {
            //        IrregAltRepository.DeleteQEP(IAId, getOrgnizationIdQEP);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbQEP.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbPLG")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdPLG = IrregAltRepository.GetAssignedUserIdPLG();
            //    string getOrgnizationIdPLG = IrregAltRepository.GetOrgnizationIdPLG();

            //    if (cbPLG.Checked == true)
            //    {
            //        IrregAltRepository.InsertPLG(IAId, getOrgnizationIdPLG, getAssignedUserIdPLG);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbPLG.Checked == false)
            //    {
            //        IrregAltRepository.DeletePLG(IAId, getOrgnizationIdPLG);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbPLG.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbACV")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdACV = IrregAltRepository.GetAssignedUserIdACV();
            //    string getOrgnizationIdACV = IrregAltRepository.GetOrgnizationIdACV();

            //    if (cbACV.Checked == true)
            //    {
            //        IrregAltRepository.InsertcbACV(IAId, getOrgnizationIdACV, getAssignedUserIdACV);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbACV.Checked == false)
            //    {
            //        IrregAltRepository.DeletecbACV(IAId, getOrgnizationIdACV);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbACV.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbSCCPPC")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdSCCPPC = IrregAltRepository.GetAssignedUserIdSCCPPC();
            //    string getOrgnizationIdSCCPPC = IrregAltRepository.GetOrgnizationIdSCCPPC();

            //    if (cbSCCPPC.Checked == true)
            //    {
            //        IrregAltRepository.InsertSCCPPC(IAId, getOrgnizationIdSCCPPC, getAssignedUserIdSCCPPC);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbSCCPPC.Checked == false)
            //    {
            //        IrregAltRepository.DeleteSCCPPC(IAId, getOrgnizationIdSCCPPC);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        //cbSCCPPC.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbQA")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdQA = IrregAltRepository.GetAssignedUserIdQA();
            //    string getOrgnizationIdQA = IrregAltRepository.GetOrgnizationIdQA();

            //    if (cbQA.Checked == true)
            //    {
            //        IrregAltRepository.InsertQA(IAId, getOrgnizationIdQA, getAssignedUserIdQA);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbQA.Checked == false)
            //    {
            //        IrregAltRepository.DeleteQA(IAId, getOrgnizationIdQA);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbQA.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbIT")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdIT = IrregAltRepository.GetAssignedUserIdIT();
            //    string getOrgnizationIdIT = IrregAltRepository.GetOrgnizationIdIT();

            //    if (cbIT.Checked == true)
            //    {
            //        IrregAltRepository.InsertIT(IAId, getOrgnizationIdIT, getAssignedUserIdIT);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbIT.Checked == false)
            //    {
            //        IrregAltRepository.DeleteIT(IAId, getOrgnizationIdIT);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        //cbIT.Checked = false;
            //    }
            //}
            //else if (e.Parameter == "cbVPC")
            //{
            //    int IAId = lblId.Text.ToInt32(0);
            //    string getAssignedUserIdVPC = IrregAltRepository.GetAssignedUserIdVPC();
            //    string getOrgnizationIdVPC = IrregAltRepository.GetOrgnizationIdVPC();

            //    if (cbVPC.Checked == true)
            //    {
            //        IrregAltRepository.InsertVPC(IAId, getOrgnizationIdVPC, getAssignedUserIdVPC);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else if (cbVPC.Checked == false)
            //    {
            //        IrregAltRepository.DeleteVPC(IAId, getOrgnizationIdVPC);
            //        gvAffectedDepart.DataBind();
            //        return;
            //    }
            //    else
            //    {
            //        // cbVPC.Checked = false;
            //    }
            //}
            //else
            //if (e.Parameter == "BtnSaveVpn")
            //    {
            //        var getHeaderId = lblId.Text;
            //        int GetIdIA = getHeaderId.ToInt32(0);

            //        IrregAltRepository.DeletePackingMonth(getHeaderId);
            //        IrregAltRepository.SavePackingMonth(getHeaderId, dates);
            //        return;
            //    }
        }
        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            try
            {
                int id = lblId.Text.ToInt32(0);
                string EmailAuthor = emailuser.Text;
                var GetAuthorId = IrregAltRepository.getauthorbyId(id);

                //sdsIAHeader.UpdateCommand = "Update IAs set IsReadyForApproval = 1 where Id = " + lblId.Text;
                //sdsIAHeader.Update();

                UpdateIsReadyForApprovalSendBack(id); //Update IsReadyForApproval on table IAs
                IrregAltRepository.SendNotifForAuthorSendBack(GetAuthorId, id);
                LoadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int Id = lblId.Text.ToInt32(0);

            try
            {
                using (DbConnection con = ConnectionHelper.GetConnection())
                {
                    con.Open();
                    string sqlCmd = "UPDATE IAs set StatusId = 4 where Id =" + Id;

                    using (DbCommand cmd = con.CreateCommand())
                    {
                        LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                        cmd.Connection = con;
                        cmd.CommandText = sqlCmd;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteScalar();

                        LoggerHelper.Info("Finish SQL Command Process...");
                    }
                }
                LoadData();
                return;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnGeneratePart_Click(object sender, EventArgs e)
        {
            string parts = txtPart.Text;
            if (parts != "")
            {
                string descript = getPartDescription(parts);
                DescriptionPart.Text = descript;
            }
            else
            {
                DescriptionPart.Text = "";
            }
        }
        protected string getPartDescription(string parts)
        {
            string value = "";
            string[] splitparts = parts.Split('/');
            foreach (string part in splitparts)
            {
                value = string.Format("{0},'{1}'", value, part);
            }
            value = value.Substring(1);

            if (parts == null || parts == "")
            {
                return value;
            }
            else
            {
                using (DbConnection con = ConnectionHelper.GetConnection())
                {
                    con.Open();

                    string sqlCmd = @";with rawDatas as (
                                       SELECT distinct c.PartNo,c.[Description]
                                       FROM AssyCatalogs c
                                       WHERE c.PartNo in (" + value + @"))
                                       select STUFF(
		                               (SELECT CONCAT(', ',x.[Description]) 
			                           FROM rawDatas x 
			                           order by x.PartNo
			                           FOR XML PATH('')
		                               ),1,1,'')[PartDescription]";

                    //string sqlCmdExecute = string.Format(sqlCmd, IAId);

                    using (DbCommand cmd = con.CreateCommand())
                    {
                        LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                        cmd.Connection = con;
                        cmd.CommandText = sqlCmd;
                        cmd.CommandType = CommandType.Text;
                        IDataReader datareader = null;
                        datareader = cmd.ExecuteReader();
                        while (datareader.Read())
                        {
                            value = datareader["PartDescription"].ToString();
                        }
                        splitparts = value.Split(',');
                        value = string.Empty;

                        foreach (string part in splitparts)
                        {
                            value = string.Format("{0} \n {1}", value, part);
                        }
                    }
                }
                return value;
            }
        }
        protected void LinkAttachment_Click(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            string custID = lb.CommandArgument;
            lblTempIdIATask.Text = custID;
            PopUpAttachmentDepartment.ShowOnPageLoad = true;
        }
        protected void ucTaskAttachments_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.IsValid) return;

            FileInfo file = new FileInfo();
            file.FilePath = SaveToTemporaryFolder(e.UploadedFile);
            file.FileName = e.UploadedFile.FileName;

            Session[lblId.Text + "_TaskAttachment"] = file;
            Session["FilePathAttachment"] = file.FilePath;

            e.CallbackData = e.UploadedFile.FileName;
        }
        protected void SaveDelegationAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                int iataskid = 0;
                string desciptonAttachment = "";
                string filename = "";
                string filepath = "";
                string FilePath = "";
                iataskid = TempGetIATaskId2["hidden_value"].ToInt32(0);
                desciptonAttachment = HtmlDesciptonAttachment.Html;
                filename = FileNameAttachment.Text;
                filepath = Session["FilePathAttachment"].ToString();
                FilePath = SaveTaskToUploadFolder(iataskid, filepath, filename);
                int status = 0;
                string statusattachment = "";
                if (rbOpen.Checked == true)
                {
                    statusattachment = "This ticket still open";
                    status = 1;
                }
                else if (rbCLose.Checked == true)
                {
                    status = 2;
                }
                else
                {
                    statusattachment = "Plase close this ticket !!!";
                    status = 0;
                }

                bool result = InsertDelegationAttachment(iataskid, desciptonAttachment, filename, FilePath, statusattachment, status);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Data " + SaveDelegationAttachment.Text + "Successfully !')", true);
                HtmlDesciptonAttachment.Html = "";
                FileNameAttachment.Text = "";
                rbCLose.Checked = false;
                rbOpen.Checked = false;
                PopUpAttachmentDepartment.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed Insert Data : " + ex.Message + "');", true);
                LoggerHelper.LogError(ex.Message);
                return;
            }
        }
        protected bool InsertDelegationAttachment(int iataskid, string desciptonAttachment, string filename, string FilePath, string statusattachment, int status)
        {
            using (DbConnection con = ConnectionHelper.GetConnection())
            {
                try
                {
                    con.Open();

                    string sqlCmd = "insert into IATaskReports (IATaskId, Report, FileName, FilePath, Reminder, IATaskStatusId, ReportDate, CreatedDate)"
                                            + "values ('" + iataskid + "' ,'" + desciptonAttachment + "','" + filename + "', '" + FilePath + "' , '" + statusattachment + "','" + status + "',GETDATE(),GETDATE())";
                    string sqlCmdExecute = string.Format(sqlCmd, iataskid, desciptonAttachment, filename, FilePath, statusattachment, status);

                    using (DbCommand cmd = con.CreateCommand())
                    {
                        LoggerHelper.Info("SQL Command Process: " + sqlCmd);

                        cmd.Connection = con;
                        cmd.CommandText = sqlCmdExecute;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteScalar();

                        LoggerHelper.Info("Finish SQL Command Process to Insert IATaskReports ...");
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogError(ex);
                }
            }
            return true;
        }
    }
}