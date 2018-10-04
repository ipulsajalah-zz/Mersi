using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DotWeb.Models;
using DotWeb.Repositories;

namespace DotMercy.custom
{
    public partial class DailyProductionRequirements : System.Web.UI.Page
    {
        private object _masterKey;

        protected void Page_Load(object sender, EventArgs e)
        {
            masterGrid.DataSource = DailyProductionRequirementRepository.GetAllDPRActive();
            masterGrid.DataBind();
        }

        protected void dprdsGrid_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView detail = (ASPxGridView)sender;
            _masterKey = detail.GetMasterRowKeyValue();

            detail.DataSource = DailyProductionRequirementRepository.GetDPRDetailsById((int)_masterKey);

            //updating row
            detail.RowUpdating += new DevExpress.Web.Data.ASPxDataUpdatingEventHandler(detail_RowUpdating);
        }

        void detail_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //you have to create validation of inputed field. only integer allowed to insert
            var model = new DailyProductionRequirementDetail();
            model.Id = (int)e.Keys[0];
            model.Target = (int)e.NewValues["Target"];
            model.TargetAccumulation = (int)e.NewValues["TargetAccumulation"];
            model.Achieve = (int)e.NewValues["Achieve"];
            model.AchieveAccumulation = (int)e.NewValues["AchieveAccumulation"];
            model.Surplus = (int)e.NewValues["Surplus"];

            DailyProductionRequirementRepository.UpdateDPRDetail(model);

            ASPxGridView senderGridView = (ASPxGridView)sender;
            e.Cancel = true;
            senderGridView.CancelEdit();
            senderGridView.DataSource = DailyProductionRequirementRepository.GetDPRDetailsById((int)_masterKey);
        }
    }
}