using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom
{
    public partial class DashboardAchievementDetail : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                string StationAct = Request.QueryString["StationAct"].ToString();
                int ProductionLineId = Convert.ToInt32(Request.QueryString["ProductionLineId"]);
                string Date = Request.QueryString["Date1"].ToString();
                //DateTime enteredDate = DateTime.Parse(Date);
            DateTime dt;
            if (DateTime.TryParseExact(Date, "yyyyMMdd",
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out dt))
            //{
                captionHeaderTitle.Text = StationAct + " Line " + ProductionLineId + " On " + dt.ToShortDateString();
                fillGridView(StationAct, ProductionLineId,Date);
            //}
            
        }

        private void fillGridView(string StationAct, int ProductionLineId, string Date)
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter da = new SqlDataAdapter("usp_GetProductionAchievement", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //first paramenter: parameter name, second parameter: parameter value of object type
            //using this way you can add more parameters
            da.SelectCommand.Parameters.AddWithValue("@Date1", Date.ToString());
            da.SelectCommand.Parameters.AddWithValue("@ProductionLineId", ProductionLineId);
            da.SelectCommand.Parameters.AddWithValue("@StationAct", StationAct);
            da.SelectCommand.Parameters.AddWithValue("@AssemblyTypeId", assemblyTypeId);
            DataSet ds = new DataSet();
            da.Fill(ds);
            grvDashboardArchievementDetail.DataSource = ds;
            grvDashboardArchievementDetail.DataBind();
        }
     }
}