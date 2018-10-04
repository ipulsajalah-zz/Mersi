using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotWeb.Models;
using DotWeb.Repositories;
using DotWeb.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.Web;
using System.Drawing;
using Ebiz.Scaffolding.Utils;

namespace DotMercy.custom
{
    public partial class VINNumberChecker : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        private static string _lineId;
        private static int _productionLineId;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                _lineId = Request.QueryString["LineId"];
            }

            if (_lineId == null)
            {
                Response.Redirect("../Production/ProductionDashboard.aspx");
            }

            //I assume LineId here is actually line number for the given AssemblyTypeId
            _productionLineId = ProductionRepository.GetProductionLineId(_lineId.ToInt32(0), assemblyTypeId);
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            ASPxButton btn = (sender as ASPxButton);

            var conditionalBtn = btn.CommandArgument;

            var userauthkey = ProductionRepository.CheckAuth(tbLogin.Text);

            // authkey


            tbLogin.Text = "";

            if (userauthkey == "" || userauthkey == null)
            {

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
                return;
            }
            //string line = _lineId;
            //var jcSerial = txtSerialNumberJC.Text;
            var prodSerial = txtSerialNumberProd.Text;

            try
            {
                if (ProductionRepository.CheckModelInSequence(prodSerial) == null)
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Model not found, please check Production Sequence !');", true);

                else if (ProductionRepository.CheckVariantInSequence(prodSerial) == null)
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Variant not found, please check Production Sequence !');", true);

                else if (string.IsNullOrWhiteSpace(ProductionRepository.CheckSerialNumberInSequence(prodSerial)))
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Serial Number/ VIN/ Order No : " + prodSerial + " Not Listed in Production Sequence!');", true);
                else
                {

                    var stationId = ProductionRepository.GetFirstStation(assemblyTypeId);
                    //Check routing exist
                    //var CheckDataInFirstStation = ProductionRepository.GetVehicleCountInStation(stationId, _productionLineId);
                    var notif = ProductionRepository.IsExistProductionRoute(prodSerial, assemblyTypeId);


                    if (notif)
                    {
                        //TODO: Confirm what is the purpose for this
                        //TODO: Is it to make sure that if vehicle count in first station > 0, it will always fail
                        //if (CheckDataInFirstStation == 0)
                        //{

                        var CheckFinNumber = ProductionRepository.GetFinNumber(prodSerial, assemblyTypeId);
                        var prod = ProductionRepository.GetProductionHistoryInMainLines(CheckFinNumber, assemblyTypeId);

                        if (prod == null)
                        {
                            if (!ProductionRepository.GetCapacityCountInStation(stationId, _productionLineId))
                            {
                                throw new Exception("Next Station Max Capacity");
                            }
                            else{
                            //Insert new productionHistory. if it already exist in subassy, replace it with the current line
                            var histId = ProductionRepository.InsertProductionHistory(_productionLineId, assemblyTypeId, prodSerial, "");
                            var detail = ProductionRepository.InsertProductionHistoryDetail(histId, stationId, false, false, "");
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Check VIN Number data successfully !');window.location ='../Production/ProductionDashboard.aspx'", true);
                        }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('VIN Number is already used !');", true);
                        }

                        //}
                        //else
                        //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('" + notif + "!');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", "alert('Production route is not found!');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed. " + ex.Message + " !');", true);
            }
        }

        private void ShowPopUp()
        {
            PopupLogin.ShowOnPageLoad = true;
            PopupLogin.Modal = true;
            PopupLogin.ModalBackgroundStyle.Opacity = 80;
            PopupLogin.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
            PopupLogin.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
            PopupLogin.AutoUpdatePosition = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ASPxButton btn = (sender as ASPxButton);

            var conditionalBtn = btn.CommandArgument;

            var userauthkey = ProductionRepository.CheckAuth(tbLogin.Text);

            // authkey


            tbLogin.Text = "";

            if (userauthkey == "" || userauthkey == null)
            {

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage2", @"alert('Your Auth Key not valid');", true);
                return;
            }
            if (VINNumberRepository.CheckUser(txtPassword.Text) != "")
            {
                try
                {
                    //string penerima = "yuhu@daimler.com";
                    //string subjectemail = "Testing Email TIDS";
                    //string email = "Halllooo Tes, blablabla";
                    //EmailNotification.GenerateEmail(penerima, subjectemail, email);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Check VIN Number data successfully !');", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed " + ex.Message + " !');", true);
                }
                PopupLogin.ShowOnPageLoad = false;
                PopupLogin.Modal = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Invalid authorization key !');", true);
                PopupLogin.ShowOnPageLoad = false;
                PopupLogin.Modal = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupLogin.ShowOnPageLoad = false;
            PopupLogin.Modal = false;
        }
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Production/ProductionDashboard.aspx");
        }
    }
}