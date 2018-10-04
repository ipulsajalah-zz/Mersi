using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Dashboard
{
    public partial class ProductionPlanAndAchievementWIP : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            string StationAct = Request.QueryString["StationAct"].ToString();
            int ProductionLineId = Convert.ToInt32(Request.QueryString["ProductionLineId"]);
            string Date = Request.QueryString["Date"].ToString();
            int AssemblyTypeId = Convert.ToInt32(Request.QueryString["AssemblyTypeId"]);
            string StationAct2 = Request.QueryString["StationAct2"].ToString();
            DateTime dt;
            if (DateTime.TryParseExact(Date, "yyyyMMdd",
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dt))
                captionHeaderTitle.Text = StationAct + " Line " + ProductionLineId + " On " + dt.ToShortDateString();
            string StoreProcedurename = "";
            if (Request.QueryString["Type"] == "TestingLine")
                StoreProcedurename += "usp_GetWorkInProgressTestLineDetail";
            else if(Request.QueryString["Type"] == "VOCAPA")
                StoreProcedurename += "usp_dashboard_VocaPA";
            else
                StoreProcedurename += "usp_GetWorkInProgressDetail";
            fillGridView(StoreProcedurename,StationAct, ProductionLineId, Date, AssemblyTypeId, StationAct2);

        }

        private void fillGridView(string StoreProcedurename,string StationAct, int ProductionLineId, string Date,int AssemblyTypeId,string StationAct2=null)
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter(StoreProcedurename, con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //first paramenter: parameter name, second parameter: parameter value of object type
            //using this way you can add more parameters
            da.SelectCommand.Parameters.AddWithValue("@Date", Date.ToString());
            da.SelectCommand.Parameters.AddWithValue("@ProductionLineId", ProductionLineId);
            da.SelectCommand.Parameters.AddWithValue("@StationAct", StationAct);
            da.SelectCommand.Parameters.AddWithValue("@StationAct2", StationAct2);
            da.SelectCommand.Parameters.AddWithValue("@AssemblyTypeId", AssemblyTypeId);
            DataSet ds = new DataSet();
            da.Fill(ds);
            grvDashboardArchievementDetail.DataSource = ds;
            grvDashboardArchievementDetail.DataBind();
        }
    }
}