using DevExpress.Web;
using DotWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{
    public partial class FaultAuditRecord : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRecordNew_Click(object sender, EventArgs e)
        {

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ShowAttachment_Click(object sender, EventArgs e)
        {

        }

        [WebMethod]
        //get Line by HistoryId
        public static string GetLine(int values)
        {

            var tempLine = ProductionRepository.GetLineByProductionId(values);
            return tempLine.LineName;
        }

        
        [WebMethod]
        //get AssemblySection by StationId
        public static string GetSection(int values)
        {
            var tempSection = StationRepository.RetrieveAsemblySectionByStId(values);
            return tempSection;
        }

        [WebMethod]
        //get Fault Classification by DescriptionId
        public static string GetClassification(int values)
        {
            var tempClassification = FaultRepository.GetFaultClassification(values);
            return tempClassification.Description;
        }

        [WebMethod]
        public static string GetType(string values)
        {
            var tempType = FaultRepository.GetType(values);
            return tempType;
        }

        [WebMethod]
        public static string GetColor(string values)
        {
            var tempType = FaultRepository.GetColor(values);
            return tempType;
        }

        [WebMethod]
        public static string GetBauMaster(string values)
        {
            var tempType = FaultRepository.GetBauMaster(values);
            return tempType;
        }
        //protected void cbCGIS_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    Session["StationId"] = cbStation.Value;
        //    Session["HistoryId"] = cbFINNumber.Value;
        //    cbCGIS.DataSource = sdsCgis;
        //    cbCGIS.DataBind();
        //    cbCGIS.SelectedIndex = 0;
        //}

    }
}