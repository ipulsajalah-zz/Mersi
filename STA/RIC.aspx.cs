using System;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Web;
using System.Web.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Scaffolding.Utils;
using System.Linq;
using Ebiz.Tids.CV.Repositories;

namespace DotMercy.custom.Report
{
    public partial class RIC : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        Ebiz.Tids.CV.Models.RIC ric = null;

        protected new void Page_Load(object sender, EventArgs e)
        {
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
                masterPage.PageTitle.Controls.Add(new LiteralControl("Record Implementation Control"));

                return;
            }

            byte[] reportb = ReportRepository.GetReportBinary("Ric");
            if (reportb != null)
            {

                using (var stream = new MemoryStream(reportb))
                {
                    XtraReport report = XtraReport.FromStream(stream, true);

                    report.Parameters["parameter1"].Value = ric.Id;
                    report.Parameters["parameter1"].Visible = false;

                    ASPxDocumentViewer1.Report = report;
                    ASPxDocumentViewer1.DataBind();
                }
            }
            else
            {

                string filePath = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/rptRic.repx");
                DevExpress.XtraReports.UI.XtraReport report = DevExpress.XtraReports.UI.XtraReport.FromFile(filePath, true);


                report.Parameters["parameter1"].Value = ric.Id;
                report.Parameters["parameter1"].Visible = false;

                ASPxDocumentViewer1.Report = report;
                ASPxDocumentViewer1.DataBind();

            }

        }

        //public static int GetRicId(int packingmth, int model, int variant)
        //{
        //    return RicRepository.GetRicId(packingmth, model, variant);
        //}

        protected void BtnDesign_Click(object sender, EventArgs e)
        {
            string url = string.Format("~/custom/Report/Designer.aspx?ricid={0}", ric.Id);
            Response.Redirect(url);
        }


    }
}