using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ebiz.WebForm.custom.Catalog
{
    public partial class FMEA : System.Web.UI.Page
    {

        string pm = "", pm2 = "", pm3 = "", pm4 = "", pm5 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (cmbTypeModel.SelectedItem == null) { pm = ""; }
                else { pm = cmbTypeModel.SelectedItem.ToString(); }

                if (cmbPM.SelectedItem == null) { pm2 = ""; }
                else { pm2 = cmbPM.SelectedItem.ToString(); }

                if (cmbAssemblyModel.SelectedItem == null) { pm3 = ""; }
                else { pm3 = cmbAssemblyModel.SelectedItem.ToString(); }

                if (cmbStations.SelectedItem == null) { pm4 = ""; }
                else { pm4 = cmbStations.SelectedItem.ToString(); }

                if (cmbGWISProcesses.SelectedItem == null) { pm5 = "0"; }
                else { pm5 = cmbGWISProcesses.SelectedItem.Value.ToString(); }

                if (cmbGWISProcesses.SelectedItem != null) { LoadDataFMEA(); }
            }
           
           

            if (pm != null || pm != "") { TypeModel(); }
            else { TypeModelNull(); }

            if (pm2 != null || pm2 != "") { PM(); }
            else { PMNull(); }

            if (pm3 != null || pm3 != "") { AssemblyModel(); }
            else { AssemblyModelNull(); }

            if (pm4 != null || pm4 != "") { StationName(); }
            else { StationNameNull(); }

        }

        protected DataTable getDataFMEA(String GetIdGWISProcesses)
        {
            if (GetIdGWISProcesses != "" || GetIdGWISProcesses != null)
            {
                DataTable Task;
                using (DbConnection con = ConnectionHelper.GetConnection())
                {
               
                        con.Open();
                        string sqlcmd = "select t1.ProcessId as Id,t1.PartNo as PartNo,t1.PartDescription as PartDescription ,t1.HSUE as HSUE,t1.Qty_Actual as  Qty_Actual,t1.SecondHand as SecondHand from GWISParts t1 where t1.ProcessId = " + GetIdGWISProcesses;
                        string sqlCmdExct = string.Format(sqlcmd);
                        using (DbCommand cmd = con.CreateCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = sqlCmdExct;
                            cmd.CommandType = CommandType.Text;
                            var datareader = cmd.ExecuteReader();
                            var datatable = new DataTable();
                            datatable.Load(datareader);
                            Task = datatable;
                            LoggerHelper.Info("Get List FMEA Success");
                        }
                        return Task;
                 }
            }
            return null;
        }
        private void TypeModel()
        {
            sdsGetPM.SelectParameters["ModelType"].DefaultValue = pm.ToString();
            cmbPM.DataBind();
        }
        private void TypeModelNull()
        {
            sdsGetPM.SelectParameters["ModelType"].DefaultValue = "";
            cmbPM.DataBind();
        }
        private void PM()
        {
            sdsGetAssemblyModel.SelectParameters["PM"].DefaultValue = pm2.ToString();
            cmbAssemblyModel.DataBind();
        }
        private void PMNull()
        {
            sdsGetAssemblyModel.SelectParameters["PM"].DefaultValue = "";
            cmbAssemblyModel.DataBind();
        }
        private void AssemblyModel()
        {
            sdsGetStations.SelectParameters["Stations"].DefaultValue = pm3.ToString();
            cmbStations.DataBind();
        }
        private void AssemblyModelNull()
        {
            sdsGetStations.SelectParameters["Stations"].DefaultValue = "";
            cmbStations.DataBind();
        }
        private void StationName()
        {
            sdsGetGWISProcesses.SelectParameters["StationName"].DefaultValue = pm4.ToString();
            cmbGWISProcesses.DataBind();
        }
        private void StationNameNull()
        {
            sdsGetGWISProcesses.SelectParameters["StationName"].DefaultValue = "";
            cmbGWISProcesses.DataBind();
        }
        private void LoadDataFMEA()
        {
            DataTable dtFMEA = getDataFMEA(pm5);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table class=\"table\">");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th></th>");
            sb.AppendLine("<th>Part No.</th>");
            sb.AppendLine("<th>Description</th>");
            sb.AppendLine("<th>HSUE</th>");
            sb.AppendLine("<th>Qty</th>");
            sb.AppendLine("<th>Second Hand</th>");
            sb.AppendLine("<th></th>");
            sb.AppendLine("</tr>");
            for (int i = 0; i < dtFMEA.Rows.Count; i++)
            {
                int h;
                h = i + 1;
                DataRow row = dtFMEA.Rows[i];
                string nopart = row["PartNo"].ToString();
                string description = row["PartDescription"].ToString();
                string hsu = row["HSUE"].ToString();
                string qty = row["Qty_Actual"].ToString();
                string secoundhand = row["SecondHand"].ToString();
                
                sb.AppendLine("<tr style=\"background: #f3f3f3;\">");
                sb.AppendLine("<td>");
                sb.AppendLine(h.ToString());
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine(nopart);
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine(description);
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine(hsu);
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine(qty);
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                sb.AppendLine(secoundhand);
                sb.AppendLine("</td>");
                sb.AppendLine("<td>");
                //sb.AppendLine("<dx:ASPxButton AutoPostBack=\"false\" ID=\"lnkTest\" runat=\"server\" RenderMode=\"Link\" Text=\"Report\" OnClick='<%# string.Format(\"javascript:return CallPopUPAddFMEA(\"{0}\")\", Eval(\"Id\")) %> '></dx:ASPxButton>");
                //buton untuk popup
                sb.AppendLine("<button id=\"btnAddFMEA\" onclick='CallPopUPAddFMEA()'>+ Add FMEA</button>");
                // sb.AppendLine("<a href=\"#myModal\" data-toggle=\"modal\" style=\"color: #0a61ad; background: #f3f3f3; padding: 5px; border: 1px solid grey; text-decoration: none;\">+ Add FMEA </ a >");
                sb.AppendLine("</td>");
                
            }
            sb.AppendLine("</table>");
            divFMEA.InnerHtml = sb.ToString();
        }

        protected void btnRPN_Click(object sender, EventArgs e)
        {
        }

    }
}