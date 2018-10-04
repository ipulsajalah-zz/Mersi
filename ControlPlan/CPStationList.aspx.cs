using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using DevExpress.Data.Linq;
using DevExpress.Web;
using System.Drawing;
//using DevExpress.XtraGauges.Core.Drawing;
using Ebiz.Scaffolding.Utils;
using Ebiz.Tids.CV.Models;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Repositories;
using Ebiz.Scaffolding.Generator;

namespace Ebiz.WebForm.Tids.CV.ControlPlans
{
    public partial class CPStationList : ListPageBase
    {
        //Default table name
        protected string tableName = "ControlPlanStationList";
        ControlPlan cp = null;

        //int DataCount = 0;
        //List<Controlplan> DataStationDeviation = new List<Controlplan>();

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


            //header info
            //int vpm = Request.QueryString["vpm"].ToInt32(0);
            //int modelId = Request.QueryString["model"].ToInt32(0);
            //int variantId = Request.QueryString["variant"].ToInt32(0);
            int cpId = Request.QueryString["CpId"].ToInt32(0);
            //int stationId = Request.QueryString["StationId"].ToInt32(0);
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
            //else if (stationId == 0)
            //{
            //    var panel1 = new System.Web.UI.WebControls.Panel();
            //    panel1.CssClass = "mainContent";
            //    panel1.Controls.Clear();
            //    panel1.Controls.Add(new LiteralControl(string.Format("<h2 class='grid-header'>Invalid Station for CP Station Detail</h2>")));

            //    masterPage.MainContent.Controls.Clear();
            //    masterPage.MainContent.Controls.Add(panel1);
            //    masterPage.PageTitle.Controls.Add(new LiteralControl(tableMeta.Caption));
            //    return false;
            //}

            //Set master key
            SetMasterKey("ControlPlanId", cp.Id);

            //Store the CP object so that it is accessible to other classes
            keyValues.Add("CP", cp);
            keyValues.Add("ControlPlanId", cp.Id);

            hfDocControl["PackingMonth"] = cp.PackingMonth;
            hfDocControl["ModelId"] = cp.AssemblyModelId;

            //create header
            string ModelName = ControlPlanRepository.RetrieveAssemblyModelNameById(cp.AssemblyModelId);
            //string StationName = ControlPlanRepository.RetrieveStationNameById(stationId);

            //lblStationName.Text = StationName;
            lblPackingMonth.Text = cp.PackingMonth;
            lblModel.Text = ModelName;

            //if (!IsPostBack && !IsCallback)
            //{
            //    if (Request.QueryString.Count > 0)
            //    {
            //        hfDocControl["ControlPlanId"] = cpId;
            //        if (cp != null)
            //        {
            //            hfDocControl["PackingMonth"] = cp.PackingMonth;
            //            hfDocControl["ModelId"] = cp.ModelId;
            //            hfDocControl["VariantId"] = cp.VariantId;

            //            lblPackingMonth.Text = cp.PackingMonth;
            //            lblModel.Text = ModelRepository.RetrieveModelNameById(cp.ModelId) + " " +
            //                            ModelRepository.RetrieveVariantNameByModelIdAndVariantId(
            //                                cp.ModelId, cp.VariantId);
            //        }
            //    }
            //}
            //else
            //{
            //    if (Session["StationId"] != null)
            //    {
            //        if (popupDocControl.HeaderText.ToString() == "Document Control - Working Instruction")
            //        {
            //            grvDocControlDetail.DataSourceID = null;
            //            grvDocControlDetail.DataSource = ControlPlanRepository.RetrieveDocumentByDocTypeAndStationId("CPWI",
            //            Convert.ToInt32(Session["StationId"].ToString()), cp.PackingMonth, Convert.ToInt32(cp.ModelId), Convert.ToInt32(cp.VariantId));
            //            grvDocControlDetail.DataBind();
            //        }
            //        else
            //        {
            //            grvDocControlDetail.DataSourceID = null;
            //            grvDocControlDetail.DataSource = ControlPlanRepository.RetrieveDocumentByDocTypeAndStationIdII("CPII",
            //            Convert.ToInt32(Session["StationId"].ToString()), cp.PackingMonth, Convert.ToInt32(cp.ModelId), Convert.ToInt32(cp.VariantId));
            //            grvDocControlDetail.DataBind();
            //        }

            //    }
            //}

            return true;
        }

        protected void grvDocControl_OnHtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //TODO

            //try
            //{
            //    if (e.RowType == GridViewRowType.Data)
            //    {
            //        ASPxGridView grv = (sender as ASPxGridView);
            //        ASPxHyperLink hplStation =
            //            (ASPxHyperLink)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "hplStation");
            //        int stationId = Convert.ToInt32(grv.GetRowValues(e.VisibleIndex, "StationId"));
            //        //hplStation.NavigateUrl = "CpDetail.aspx?StationId=" + stationId + "&vpm=" +
            //        //                         hfDocControl["PackingMonth"] +
            //        //                         "&model=" + hfDocControl["ModelId"] + "&variant=" +
            //        //                         hfDocControl["VariantId"];
            //        hplStation.NavigateUrl = "CPStationDetail.aspx?StationId=" + stationId + "&vpm=" +
            //                                 hfDocControl["PackingMonth"] +
            //                                 "&model=" + hfDocControl["ModelId"] + "&variant=" +
            //                                 hfDocControl["VariantId"];

            //        ASPxButton btnIrreg =
            //            (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnIrregularAlt");

            //        int countIA =
            //            IrregAltRepository.CountIAByModelIdAndStationId(Convert.ToInt32(hfDocControl["ModelId"]), Convert.ToInt32(hfDocControl["VariantId"]),
            //                Convert.ToInt32(btnIrreg.CommandArgument), Convert.ToInt32(hfDocControl["PackingMonth"]));
            //        if (countIA == 0)
            //            btnIrreg.Visible = false;
            //        else
            //            btnIrreg.Visible = true;

            //        ////FMEA
            //        ASPxButton btnFmea = (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnFMEA");
            //        int countFMEA = ControlPlanRepository.CountFmeaAttachment(Convert.ToInt32(btnFmea.CommandArgument),
            //            hfDocControl["PackingMonth"].ToString(), Convert.ToInt32(hfDocControl["ModelId"]));
            //        if (countFMEA == 0)
            //            btnFmea.Visible = false;
            //        else
            //            btnFmea.Visible = true;
            //        ////AlterationDoc
            //        ASPxButton btnAlterationDoc = (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnAlterationDoc");
            //        int countAlterationDoc = ControlPlanRepository.CountAlterationAttachment(Convert.ToInt32(btnAlterationDoc.CommandArgument),
            //            hfDocControl["PackingMonth"].ToString(), Convert.ToInt32(hfDocControl["ModelId"]));
            //        if (countAlterationDoc == 0)
            //            btnAlterationDoc.Visible = false;
            //        else
            //            btnAlterationDoc.Visible = true;

            //        //WI
            //        ASPxButton btnWi = (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnWI");
            //        int countWi = ControlPlanRepository.CountCpDoc("CPWI", Convert.ToInt32(hfDocControl["ModelId"]),
            //            Convert.ToInt32(btnWi.CommandArgument), hfDocControl["PackingMonth"].ToString(), Convert.ToInt32(hfDocControl["VariantId"]));
            //        if (countWi == 0)
            //            btnWi.Visible = false;
            //        else
            //            btnWi.Visible = true;

            //        //II
            //        ASPxButton btnIi = (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnII");
            //        int countIi = ControlPlanRepository.CountCpDocII("CPII", Convert.ToInt32(hfDocControl["ModelId"]),
            //            Convert.ToInt32(btnIi.CommandArgument), hfDocControl["PackingMonth"].ToString(), Convert.ToInt32(hfDocControl["VariantId"]));
            //        if (countIi == 0)
            //            btnIi.Visible = false;
            //        else
            //            btnIi.Visible = true;

            //        //ConsumptionMaterial
            //        ASPxButton btnCm = (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnCM");
            //        int countCm =
            //            ConsumptionMaterialRepository.CountConsumptionMaterialUsage(
            //                Convert.ToInt32(hfDocControl["ModelId"]), Convert.ToInt32(btnCm.CommandArgument));
            //        if (countCm == 0)
            //            btnCm.Visible = false;
            //        else
            //            btnCm.Visible = true;

            //        //Calibrated Tool
            //        ASPxButton btnCalTools =
            //            (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnCalTools");
            //        int countCalTool =
            //            ToolRepository.CountToolByStationId(hfDocControl["ControlPlanId"].ToInt32(0),
            //                Convert.ToInt32(btnCalTools.CommandArgument), true);
            //        if (countCalTool == 0)
            //            btnCalTools.Visible = false;
            //        else
            //            btnCalTools.Visible = true;

            //        //Non Calibrated Tool
            //        ASPxButton btnNonCalTools =
            //            (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnNonCalTools");
            //        int countNonCalTool =
            //            ToolRepository.CountToolByStationId(hfDocControl["ControlPlanId"].ToInt32(0),
            //                Convert.ToInt32(btnCalTools.CommandArgument), false);
            //        if (countNonCalTool == 0)
            //            btnNonCalTools.Visible = false;
            //        else
            //            btnNonCalTools.Visible = true;

            //        //color
            //        //var x = new List<Controlplan>();
            //        if (DataCount < 1)
            //        {
            //            var x =
            //                ControlPlanRepository.GetStationDeviation(int.Parse(hfDocControl["ControlPlanId"].ToString()));
            //            DataCount = x.Count;
            //            DataStationDeviation = x;
            //        }

            //        List<ControlPlanProcess> ListData = new List<ControlPlanProcess>();
            //        for (int i = 0; i < DataStationDeviation.Count; i++)
            //        {
            //            if (stationId == DataStationDeviation[i].StationId)
            //            {
            //                Color tlr = System.Drawing.ColorTranslator.FromHtml("#44C169");
            //                e.Row.BackColor = tlr;
            //            }
            //        }


            //        //if (grv.GetRowValues(e.VisibleIndex, "HasDeviation") != null &&
            //        //  Convert.ToBoolean(grv.GetRowValues(e.VisibleIndex, "HasDeviation")))
            //        //{
            //        //    Color tlr = ARGBColorTranslator.FromHtml("#44C169");
            //        //    e.Row.BackColor = tlr;
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }
        protected void grvIA_OnCustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "No")
                {
                    e.Value = string.Format("{0}", e.ListSourceRowIndex + 1);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void grvIA_OnHtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                ASPxGridView grv = (sender as ASPxGridView);
                ASPxButton btnIAInfo = (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnIAInfo");
                if (grv.GetRowValues(e.VisibleIndex, "Info") != null && grv.GetRowValues(e.VisibleIndex, "Info").ToString().Trim().Length > 0)
                {
                    IA ia = IrregAltRepository.RetrieveIAHeadersById(Convert.ToInt32(btnIAInfo.CommandArgument));
                    btnIAInfo.Text = (grv.GetRowValues(e.VisibleIndex, "Info").ToString()) + System.Environment.NewLine + "(" + ia.InfoNumber + ")";
                }

                ASPxLabel lblPart = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblPart");
                if (lblPart != null)
                {
                    int id = Convert.ToInt32(grv.GetRowValues(e.VisibleIndex, "Id"));
                    lblPart.Text = IrregAltRepository.RetrieveIrregAlterationPartsById(id);
                }

                ASPxLabel lblModel = (ASPxLabel)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "lblModelName");
                if (lblModel != null)
                {
                    string ModelName = ControlPlanRepository.RetrieveAssemblyModelNameById(Convert.ToInt32(hfDocControl["ModelId"]));
                    lblModel.Text = ModelName;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void grvDocControlDetail_OnHtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.VisibleIndex == -1) return;

                ASPxGridView grv = (sender as ASPxGridView);
                ASPxButton btnDownload =
                    (ASPxButton)grv.FindRowCellTemplateControl(e.VisibleIndex, null, "btnDownload");
                if (grv.GetRowValues(e.VisibleIndex, "File") != null) //|| grv.GetRowValues(e.VisibleIndex, "FileLocation").ToString() != string.Empty)
                    btnDownload.Text = Path.GetFileName(grv.GetRowValues(e.VisibleIndex, "File").ToString());
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }

        protected void btnStationName_OnClick(object sender, EventArgs e)
        {
            try
            {
                int stationId = Convert.ToInt32((sender as ASPxButton).CommandArgument);
                string urlName = "CpDetail.aspx?StationId=" + stationId + "&vpm=" + hfDocControl["PackingMonth"] +
                                 "&model=" + hfDocControl["ModelId"] + "&variant=" + hfDocControl["VariantId"];
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenNewWindow",
                    "window.open('" + urlName + "','_newtab');", true);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnCM_OnClick(object sender, EventArgs e)
        {
            int modelId = hfDocControl["ModelId"].ToInt32(0);
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
                    "window.open('../Materials/CMListByStation.aspx?stationId=" + btn.CommandArgument + "&ModelId=" + modelId + "','_newtab');",
                    true);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnIrregularAlt_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    grvIA.DataSource = null;
            //    grvIA.DataSource = IrregAltRepository.RetrieveIrregAlterationBy(Convert.ToInt32(hfDocControl["ModelId"]), Convert.ToInt32(hfDocControl["VariantId"]),Convert.ToInt32(btn.CommandArgument), Convert.ToInt32(hfDocControl["PackingMonth"]));
            //    grvIA.DataBind();
            //    popupIA.ShowOnPageLoad = true;
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }
        protected void btnIAInfo_OnClick(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
                    "window.open('../IrregAlt/IAEdit.aspx?Id=" + btn.CommandArgument + "','_newtab');",
                    true);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnWI_OnClick(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                ControlPlan cp =
                        ControlPlanRepository.RetrieveControlPlanById(Convert.ToInt32(Request.QueryString["CpId"]));

                hfDocCtrlDetail["docType"] = "CPWI";

                grvDocControlDetail.DataSourceID = null;
                grvDocControlDetail.DataSource = ControlPlanRepository.RetrieveDocumentByDocTypeAndStationId("CPWI",
                    Convert.ToInt32(btn.CommandArgument), cp.PackingMonth, Convert.ToInt32(cp.AssemblyModelId));
                grvDocControlDetail.DataBind();
                Session["StationId"] = Convert.ToInt32(btn.CommandArgument);

                popupDocControl.HeaderText = "Document Control - Working Instruction";
                popupDocControl.ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnII_OnClick(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                ControlPlan cp =
                        ControlPlanRepository.RetrieveControlPlanById(Convert.ToInt32(Request.QueryString["CpId"]));

                hfDocCtrlDetail["docType"] = "CPII";
                grvDocControlDetail.DataSource = null;
                grvDocControlDetail.DataSource = ControlPlanRepository.RetrieveDocumentByDocTypeAndStationIdII("CPII",
                    Convert.ToInt32(btn.CommandArgument), cp.PackingMonth, Convert.ToInt32(cp.AssemblyModelId));
                grvDocControlDetail.DataBind();
                Session["StationId"] = Convert.ToInt32(btn.CommandArgument);

                popupDocControl.HeaderText = "Document Control - Inspection Instruction";
                popupDocControl.ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnFMEA_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    int model = Convert.ToInt32( hfDocControl["ModelId"]);
            //    ASPxButton btn = (ASPxButton)sender;
            //    ControlPlanFmeaAttachment cpfa =
            //        ControlPlanRepository.RetrieveControlPlanFmeaAttachmentByStationIdAndIsActive(Convert.ToInt32(btn.CommandArgument), hfDocControl["PackingMonth"].ToString(), model);
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
            //        "window.open('../CommonWorks/CustomDocumentViewer.aspx?stationId=" + btn.CommandArgument +
            //        "&docType=CPFMEA&id=" + cpfa.Id + " ','_blank');",
            //        true);
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }
        protected void btnAlterationDoc_Click(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    int model = Convert.ToInt32(hfDocControl["ModelId"]);
            //    ASPxButton btn = (ASPxButton)sender;
            //    AlterationDoc cpfa =
            //        ControlPlanRepository.RetrieveControlPlanAlterationAttachmentByStationIdAndIsActive(Convert.ToInt32(btn.CommandArgument), hfDocControl["PackingMonth"].ToString(), model);
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
            //    "window.open('../CommonWorks/CustomDocumentViewer.aspx?stationId=" + btn.CommandArgument +
            //    "&docType=CPALTERATIONDOC&id=" + cpfa.Id + " ','_blank');",
            //    true);
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }
        protected void btnProcessNo_OnClick(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (sender as ASPxButton);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
                    "window.open('/ControlPlanAttachmentsWI/list','_blank');",
                    true);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
            }
        }
        protected void btnNonCalTools_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    hfTools["IsCalibrated"] = false;
            //    grvTools.DataSource = null;
            //    grvTools.DataSource =
            //        ToolRepository.RetrieveToolAssignByStationIdAndCalibratedType(
            //            false, Convert.ToInt32(btn.CommandArgument), hfDocControl["ControlPlanId"].ToInt32(0));
            //    grvTools.DataBind();
                
            //    popupTools.HeaderText = "Tools - Non Calibrated";
            //    popupTools.ShowOnPageLoad = true;
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }
        protected void btnCalTools_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    hfTools["IsCalibrated"] = true;
            //    grvTools.DataSource = null;
            //    grvTools.DataSource =
            //        ToolRepository.RetrieveToolAssignByStationIdAndCalibratedType(
            //            true, Convert.ToInt32(btn.CommandArgument), hfDocControl["ControlPlanId"].ToInt32(0));
            //    grvTools.DataBind();
            //    Session["StationId"] = Convert.ToInt32(btn.CommandArgument);

            //    popupTools.HeaderText = "Tools - Calibrated";
            //    popupTools.ShowOnPageLoad = true;
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }
         protected void grvTools_PageIndexChanged(object sender, EventArgs e)
        {
            //TODO

            //if (Session["StationId"] != null)
            // {
            //     hfTools["IsCalibrated"] = true;
            //     grvTools.DataSource = null;
            //     grvTools.DataSource =
            //         ToolRepository.RetrieveToolAssignByStationIdAndCalibratedType(
            //             Convert.ToBoolean(hfTools["IsCalibrated"]), Convert.ToInt32(Session["StationId"].ToString()), hfDocControl["ControlPlanId"].ToInt32(0));
            //     grvTools.DataBind();
            //     popupTools.HeaderText = "Tools - Calibrated";
            //     popupTools.ShowOnPageLoad = true;
            // }
            //else
            // {
            //     grvTools.DataSource = null;
            //     popupTools.HeaderText = "Tools - Calibrated";
            //     popupTools.ShowOnPageLoad = true;
            // }
        }
        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            //TODO

            //try
            //{
            //    ASPxButton btn = (sender as ASPxButton);
            //    int docId = Convert.ToInt32(btn.CommandArgument);
            //    var selectedValues = grvDocControlDetail.GetCurrentPageRowValues("Id", "DocTypeId", "File");
            //    string filePath = GetFileFromRowValues(docId, selectedValues);
            //    string docType = GetDocTypeFromRowValues(docId, selectedValues);

            //    if (Path.GetExtension(filePath) == ".pdf")
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow",
            //        "window.open('../CommonWorks/CustomDocumentViewer.aspx?id=" + btn.CommandArgument + "&docType=" + docType + "','_blank');",
            //        true);
            //    }
            //    else
            //    {
            //        Response.Redirect("../../custom/DownloadFile.aspx?type=" + hfDocCtrlDetail["docType"] + "&Id=" +
            //                          btn.CommandArgument);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LoggerHelper.LogError(ex);
            //}
        }

       

       

    }
}