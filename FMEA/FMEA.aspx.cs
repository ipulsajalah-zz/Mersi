using DevExpress.Web;
using Ebiz.Scaffolding.Generator;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using Ebiz.Scaffolding.WebForm.UI;
using Ebiz.Tids.CV.Models;
using Ebiz.Tids.CV.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace Ebiz.WebForm.custom.FMEA
{
    public partial class FMEA : ListPageBase
    {       
        //Default table name
        protected static string tableName = "FMEAs";
        
        protected override bool OnInit(object sender, EventArgs e)
        {
            //get TableMeta form Schema. Schema is loaded during login
            var schemaInfo = Application["SchemaInfo"] as SchemaInfo;
            tableMeta = schemaInfo.Tables.Where(s => s.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if(tableMeta == null)
            {
                masterPage.MainContent.Controls.Add(new LiteralControl(string.Format("<h2>{0}</2>", "Invalid Page")));
                return false;
            }
            Session["TableName"] = tableName;

            BindType();
            BindArea();
            BindStation();
            BindProcess();
            BindPart();
            BindTypeEdit();
            BindAreaEdit();
            BindStationEdit();
            BindProcessEdit();
            BindPartEdit();

            return true;
        }

        protected override bool OnLoad(object sender, EventArgs e)
        {
            
            return true;
        }

       

        protected void PopUpAddFMEA_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int key = e.Parameter.ToInt32(0);
            if (key == 0)
                return;

            Session["FMEANo"] = key;

            //get data from db
            using(AppDb fmeas = new AppDb())
            {
                FMEAs fmea = new FMEAs();

                //populate controls
                fmea.Id = Convert.ToInt32(hddId.Value);
                fmea.FMEANo = txtFMEANo.Text;
                fmea.CatalogModelId = Convert.ToInt32(ddlType.SelectedValue);
                fmea.AssemblyModelId = Convert.ToInt32(ddlAssemblyModel.SelectedValue);
                fmea.AssemblySectionId = Convert.ToInt32(ddlArea.SelectedValue);
                fmea.StationId = Convert.ToInt32(ddlStation.SelectedValue);
                fmea.ProcessId = Convert.ToInt32(ddlProcessNo.SelectedValue);
                fmea.PartId = Convert.ToInt32(ddlPartNo.SelectedValue);
                fmea.Potential_Failure_Effect = txtPotentialFailureEffect.Text;
                fmea.Potential_Failure_Mode = txtPotentialFailureMode.Text;
                fmea.Potential_Causes = txtPotentialCauses.Text;
                fmea.Controls_Prevention = txtControlsPrevention.Text;
                fmea.Controls_Detection = txtControlsDetection.Text;
                fmea.ValidFrom = Convert.ToDateTime(txtValfrom.Text);
                fmea.ValidTo = Convert.ToDateTime(txtValto.Text);
            }                                           
        }

        public static string Constr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;

        public static string GetConnectionString()
        {
            return Constr;
        }

        protected void btnSaveFMEA_Click(object sender, EventArgs e)
        {
            String query = @"Insert into FMEAs(Potential_Failure_Effects, Potential_Failure_Mode, Potential_Causes, Controls_Prevention, Controls_Detection, ValidFrom, ValidTo, SEV, HSUE, OCC, DET, FMEANo, CatalogModeId, AssemblyModelId, AssemblySectionId, StationId, ProcessId, PartId)
                             values (@PFE, @PFM, @PC, @CP, @CD, @VF, @VT, @SEV, @HSUE, @OCC, @DET, @FMEANo, @CatalogMode, @AssemblyModel, @AssemblySection, @Station, @Process, @Part)";
            
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("@PFE", SqlDbType.VarChar).Value = txtPotentialFailureEffect.Text;
                        cmd.Parameters.Add("@PFM", SqlDbType.VarChar).Value = txtPotentialFailureMode.Text;
                        cmd.Parameters.Add("@PC", SqlDbType.VarChar).Value = txtPotentialCauses.Text;
                        cmd.Parameters.Add("@CP", SqlDbType.VarChar).Value = txtControlsPrevention.Text;
                        cmd.Parameters.Add("@CD", SqlDbType.VarChar).Value = txtControlsDetection.Text;
                        cmd.Parameters.Add("@VF", SqlDbType.Date).Value = txtValfrom.Text;
                        cmd.Parameters.Add("@VT", SqlDbType.Date).Value = txtValto.Text;
                        cmd.Parameters.Add("@SEV", SqlDbType.Int).Value = ddlSEV.SelectedItem.Text;
                        cmd.Parameters.Add("@HSUE", SqlDbType.Char).Value = ddlClass.SelectedItem.Text;
                        cmd.Parameters.Add("@OCC", SqlDbType.Int).Value = ddlOCC.SelectedItem.Text;
                        cmd.Parameters.Add("@DET", SqlDbType.Int).Value = ddlDET.SelectedItem.Text;
                        cmd.Parameters.Add("@FMEANo", SqlDbType.VarChar).Value = txtFMEANo.Text;
                        cmd.Parameters.Add("@CatalogMode", SqlDbType.Int).Value = ddlType.SelectedValue; 
                        cmd.Parameters.Add("@AssemblyModel", SqlDbType.Int).Value = ddlAssemblyModel.SelectedValue;
                        cmd.Parameters.Add("@AssemblySection", SqlDbType.Int).Value = ddlArea.SelectedValue;
                        cmd.Parameters.Add("@Station", SqlDbType.Int).Value = ddlStation.SelectedValue;
                        cmd.Parameters.Add("@Process", SqlDbType.Int).Value = ddlProcessNo.SelectedValue;
                        cmd.Parameters.Add("@Part", SqlDbType.Int).Value = ddlPartNo.SelectedValue;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }               
        }

        protected void BindType()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, Model from CatalogModels";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlType.DataSource = dt;
            ddlType.DataBind();
            ddlType.DataTextField = "Model";
            ddlType.DataValueField = "ID";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindArea()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, AssemblySectionName from AssemblySections";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlArea.DataSource = dt;
            ddlArea.DataBind();
            ddlArea.DataTextField = "AssemblySectionName";
            ddlArea.DataValueField = "ID";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindStation()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, StationName from Stations";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlStation.DataSource = dt;
            ddlStation.DataBind();
            ddlStation.DataTextField = "StationName";
            ddlStation.DataValueField = "ID";
            ddlStation.DataBind();
            ddlStation.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindProcess()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, ProcessNo from GWISProcesses";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlProcessNo.DataSource = dt;
            ddlProcessNo.DataBind();
            ddlProcessNo.DataTextField = "ProcessNo";
            ddlProcessNo.DataValueField = "ID";
            ddlProcessNo.DataBind();
            ddlProcessNo.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindPart()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, PartNo from GWISParts";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlPartNo.DataSource = dt;
            ddlPartNo.DataBind();
            ddlPartNo.DataTextField = "PartNo";
            ddlPartNo.DataValueField = "ID";
            ddlPartNo.DataBind();
            ddlPartNo.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void PopUpEditFMEA_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int key = e.Parameter.ToInt32(0);
            if (key == 0)
                return;

            Session["FMEANo"] = key;

            //get data from db
            using (AppDb fmeas = new AppDb())
            {
                FMEAs fmea = new FMEAs();

                //populate controls
                fmea.Id = Convert.ToInt32(hddIdEdit.Value);
                fmea.FMEANo = txtFMEANoEdit.Text;
                fmea.CatalogModelId = Convert.ToInt32(ddlTypeEdit.SelectedValue);
                fmea.AssemblyModelId = Convert.ToInt32(ddlAssemblyModelEdit.SelectedValue);
                fmea.AssemblySectionId = Convert.ToInt32(ddlAreaEdit.SelectedValue);
                fmea.StationId = Convert.ToInt32(ddlStationEdit.SelectedValue);
                fmea.ProcessId = Convert.ToInt32(ddlProcessNoEdit.SelectedValue);
                fmea.PartId = Convert.ToInt32(ddlPartNoEdit.SelectedValue);
                fmea.Potential_Failure_Effect = txtPotentialFailureEffectEdit.Text;
                fmea.Potential_Failure_Mode = txtPotentialFailureModeEdit.Text;
                fmea.Potential_Causes = txtPotentialCausesEdit.Text;
                fmea.Controls_Prevention = txtControlsPreventionEdit.Text;
                fmea.Controls_Detection = txtControlDetectionEdit.Text;
                fmea.ValidFrom = Convert.ToDateTime(txtValfromEdit.Text);
                fmea.ValidTo = Convert.ToDateTime(txtValtoEdit.Text);
            }
        }

        protected void BindTypeEdit()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, Model from CatalogModels";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlTypeEdit.DataSource = dt;
            ddlTypeEdit.DataBind();
            ddlTypeEdit.DataTextField = "Model";
            ddlTypeEdit.DataValueField = "ID";
            ddlTypeEdit.DataBind();
            ddlTypeEdit.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindAreaEdit()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, AssemblySectionName from AssemblySections";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlAreaEdit.DataSource = dt;
            ddlAreaEdit.DataBind();
            ddlAreaEdit.DataTextField = "AssemblySectionName";
            ddlAreaEdit.DataValueField = "ID";
            ddlAreaEdit.DataBind();
            ddlAreaEdit.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindStationEdit()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, StationName from Stations";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlStationEdit.DataSource = dt;
            ddlStationEdit.DataBind();
            ddlStationEdit.DataTextField = "StationName";
            ddlStationEdit.DataValueField = "ID";
            ddlStationEdit.DataBind();
            ddlStationEdit.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindProcessEdit()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, ProcessNo from GWISProcesses";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlProcessNoEdit.DataSource = dt;
            ddlProcessNoEdit.DataBind();
            ddlProcessNoEdit.DataTextField = "ProcessNo";
            ddlProcessNoEdit.DataValueField = "ID";
            ddlProcessNoEdit.DataBind();
            ddlProcessNoEdit.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void BindPartEdit()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            string com = "select distinct Id, PartNo from GWISParts";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlPartNoEdit.DataSource = dt;
            ddlPartNoEdit.DataBind();
            ddlPartNoEdit.DataTextField = "PartNo";
            ddlPartNoEdit.DataValueField = "ID";
            ddlPartNoEdit.DataBind();
            ddlPartNoEdit.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        protected void btnSaveFMEAEdit_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            
        }
 
    }
}