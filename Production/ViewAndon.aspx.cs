using DevExpress.Web;
using DotWeb.Repositories;
using DotWeb.Utils;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{
    public partial class ViewAndon : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        
        //}

        protected void Page_Init(object sender, EventArgs e)
        { 
            loadInit();
            string __EVENTTARGET = Request["__EVENTTARGET"];
            if (__EVENTTARGET == "btnSave")
            {
                DateTime parameter = Request["__EVENTARGUMENT"].ToDateTime((DateTime.Now));
                save(parameter);
            }
        }

        public void loadInit()
        {
            var ProductionLine = ProductionRepository.GetProductionLine().Where(c=> c.LineNumber < 20);
            var AssemblySectionGroup = ProductionRepository.GetAssemblySectionGroup();
        
            bool IsAndoMode = true;
            HtmlGenericControl div = new HtmlGenericControl("div");
         
                

            divMasterDataView.Controls.Clear();
           
          
            foreach (var pl in ProductionLine)
            {
                HtmlGenericControl divHeader = new HtmlGenericControl("div");
                divHeader.Attributes.Add("class", "div-header");

                

                if (IsAndoMode)
                {

                    HtmlGenericControl overrideDate = new HtmlGenericControl("div");
                    overrideDate.Attributes.Add("class", "overrideDate");

                    ASPxLabel lbl = new ASPxLabel();
                    lbl.ID = "Date";
                    lbl.Text = "Override Date";
                    lbl.CssClass = "lbl_line overrideDateLbl";

                   
                    ASPxDateEdit dat = new ASPxDateEdit();
                    dat.ID = "dat";
                    dat.CssClass = "lbl_line overrideDateLbl";
                    dat.Value = ProductionRepository.GetAndonOverideDate();


                    ASPxButton btnNew2 = new ASPxButton();
                    btnNew2.ID = Guid.NewGuid().ToString("N");
                    btnNew2.Text = "Save";
                    btnNew2.AutoPostBack = false;
                    btnNew2.CssClass = "overrideDateLbl";
                   
                    var command = "function (s, e) {goto2();}";
                    btnNew2.ClientSideEvents.Click = command;


                    overrideDate.Controls.Add(lbl);
                    overrideDate.Controls.Add(dat);
                    overrideDate.Controls.Add(btnNew2);
                    divHeader.Controls.Add(overrideDate);
                    divMasterDataView.Controls.Add(divHeader);
                    
                    IsAndoMode = false;

                }
                divHeader = new HtmlGenericControl("div");
                divHeader.Attributes.Add("class", "div-header");


                    ASPxLabel lblLine = new ASPxLabel();
                    lblLine.ID = pl.Id.ToString();
                    lblLine.Text = pl.LineName;
                    lblLine.CssClass = "lbl_line";

                    divHeader.Controls.Add(lblLine);
                    divMasterDataView.Controls.Add(divHeader);

                   

                 

                    divMasterDataView.Controls.Add(divHeader);
               
                    foreach (var sc in AssemblySectionGroup)
                    {
                        ASPxLabel lblTrimMech = new ASPxLabel();
                        lblTrimMech.Text = "    ";
                        lblTrimMech.CssClass = "label-section";
                        

                        ASPxButton btnNew = new ASPxButton();
                        btnNew.ID = sc.Name.ToString() + pl.Id.ToString() + Guid.NewGuid().ToString("N"); ;
                        btnNew.Text = sc.Name;
                        btnNew.AutoPostBack = false;
                        btnNew.Paddings.Padding = 5;
                       

                        btnNew.CssClass = string.Format("btn-new-line {0}", pl.Id.ToString());



                        var command = "function (s, e) {goto(" + pl.Id + "," + "'" + sc.Name.ToString() + "'" + ");}";
                        btnNew.ClientSideEvents.Click = command;



                        divHeader.Controls.Add(lblTrimMech);
                        divHeader.Controls.Add(btnNew);


                    }

                    divMasterDataView.Controls.Add(divHeader);

                
            }
        }


        public void save(DateTime date)
        {
            try
            {

                ProductionRepository.UpdateAndonOverideDate(date);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                //alert process has move to next station
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Failed, please contact your administrator');", true);
                AppLogger.LogError(ex.Message);
            }
        }
    }
}