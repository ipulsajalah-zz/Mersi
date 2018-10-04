using Ebiz.WebForm.Reports;
using Ebiz.WebForm.Reports.GWIS_Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.GWIS
{
    public partial class ReportGWIS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsCallback && !IsPostBack)
            {

                GWISReport rpt = new GWISReport();

                int id = Convert.ToInt32(Request.QueryString["Id"]);

                rpt.Parameters["parameter1"].Value = id;
                rpt.Parameters["parameter1"].Visible = false;

                ASPxDocumentViewer1.Report = rpt;
            }
        }
    }
}