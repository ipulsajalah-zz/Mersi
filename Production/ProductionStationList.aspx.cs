using DevExpress.Web;
using DevExpress.Web.Data;
using Ebiz.Scaffolding.Generator;
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

namespace Ebiz.WebForm.custom.Production
{
    public partial class ProductionStationList : ListPageBase
    {
        //Default table name
        protected static string tableName = "ProductionStationList";

        private DataTable dataTable;

        /// <summary>
        /// Custom Page_Init function for inherited class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnInit(object sender, EventArgs e)
        {
            int stationId = Request.QueryString["Stat"].ToInt32(0);
            //int historyId = Request.QueryString["Hist"].ToInt32(0);

            Station station = ProductionRepository.GetProductionStation(stationId);
            if (station == null)
            {
                masterPage.MainContent.Controls.Clear();
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Station")));
                return false;
            }

            ////if it is sequential station, directly open the station detail
            //if (station.IsSequentialAssembly && historyId > 0)
            //{
            //    Response.Redirect("~/custom/Production/ProductionDashboardDetail.aspx?Type=" + (station.IsQualityGate ? "1" : "0")
            //           + "&Hist=" + historyId + "&Stat=" + station.Id + "&Sect=" + station.AssemblySectionId);
            //}

            //get TableMeta from Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (tableMeta == null)
            {
                masterPage.MainContent.Controls.Clear();
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</h2>", "Invalid Page")));
                return false;
            }

            lblPosition.Text = station.StationName;

            masterPage.PageTitle.Controls.Add(new LiteralControl(station.StationName));

            //Set master key
            SetMasterKey("StationId", station.Id);

            //Store the Station object so that it is accessible to other classes
            keyValues.Add("Station", station);
            keyValues.Add("StationId", station.Id);

            return true; 
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            if (masterGrid != null)
            {
                //masterGrid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                //masterGrid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                //masterGrid.Settings.VerticalScrollableHeight = 600;

            }

            return true;
        }

        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/custom/Production/ProductionDashboard.aspx");
        }

    }

}