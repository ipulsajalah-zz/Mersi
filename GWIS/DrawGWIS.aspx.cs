using Ebiz.Scaffolding.Utils;
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

namespace Ebiz.WebForm.custom.GWIS
{
    public partial class DrawGWIS : System.Web.UI.Page
    {
        DataTable dt = new DataTable();

        //SqlConnection lcon = new SqlConnection(ConfigurationManager.ConnectionStrings.ToString());
        //Point Point = new Point(new Size(20, 100));
        //SqlDataAdapter adp;
        //Byte[] byteOfImage;
        //SqlDataReader dr;
        //SqlCommand com;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrWhiteSpace(Request.QueryString["ImageID"]))
                {
                    getEditImage(Request.QueryString["ImageID"]);
                    setRepeaterAnnot(Request.QueryString["ImageID"]);
                }
            }
        }

        public void CreateMessageAlert(string type, string message)
        {
            Guid guidKey = System.Guid.NewGuid();
            Page pg = HttpContext.Current.Handler as Page;

            String strScript = "showNotification({" +
                                        "type : \"" + type + "\"," +
                                        "message: \"" + message + "\"," +
                                        "autoClose: true," +
                                        "duration: 3" +
                                    "});";
            pg.ClientScript.RegisterStartupScript(pg.GetType(), guidKey.ToString(), strScript, true);
        }

        public Byte[] BmpToBytes_MemStream(Bitmap bmp)
        {
            MemoryStream MS = null;

            //Save to memory using the Jpeg format
            bmp.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);

            //read to end
            Byte[] bmpBytes = MS.GetBuffer();
            bmp.Dispose();
            MS.Close();

            return bmpBytes;
        }

        private void setRepeaterAnnot(string ImageId)
        {
            using (DbConnection conn = ConnectionHelper.GetConnection())
            {
                String query = "SELECT id, type, color, pos_left, pos_top, modifieddate as edit_date, modifiedby as editor, remarks, (pos_left + width) AS x2, (pos_top + height) AS y2, CASE type WHEN '1' THEN 'Arrow' WHEN '2' THEN 'Rectangle' WHEN '3' THEN 'Ellipse' END AS strType FROM GWISAnnotations WHERE PictureId = '" + ImageId + "' ORDER BY type";

                DataAdapter adp = conn.CreateDataAdapter(query);
                try
                {
                    conn.Open();

                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    Repeater_Annot.DataSource = ds.Tables[0];
                    Repeater_Annot.DataBind();
                }
                catch(Exception ex)
                {
                    CreateMessageAlert("error", "An error occured when setting repeater annot. Loc : setRepeaterAnnot. Exception : " + ex.Message);
                }

            }
        }

        private void getEditImage(string ImageId)
        {
            Image_Edit.ImageUrl = "DisplayCGIS.aspx?ImageID=" + ImageId + "&state=" + Request.QueryString["state"] + "&timestamp=" + Guid.NewGuid().ToString();
        }

        protected void Button_Save_Click(object sender, EventArgs e)
        {
            string[] arr1 = TextBox_Start.Text.Split(",");
            string[] arr2 = TextBox_End.Text.Split(",");

            int pos_top, pos_left, width, height;

            pos_top = arr1[1].ToInt32(0);
            pos_left = arr1[0].ToInt32(0);
            height = arr2[1].ToInt32(0) - pos_top;
            width = arr2[0].ToInt32(0) - pos_left;

            using (DbConnection conn = ConnectionHelper.GetConnection())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO GWISAnnotations(PictureId, pos_top, pos_left, width, height, createddate, createdby, modifieddate, modifiedby, remarks, type, color) VALUES(@id_image, @pos_top, @pos_left, @width, @height, @upload_date, @uploader, @edit_date, @editor, @remarks, @type, @color)";

                cmd.AddInParameter("@id_image", DbType.Int32, 4).Value = (Request.QueryString["ImageID"].ToInt32(0));
                cmd.AddInParameter("@pos_top", DbType.Int32, 4).Value = (pos_top);
                cmd.AddInParameter("@pos_left", DbType.Int32, 4).Value = (pos_left);
                cmd.AddInParameter("@width", DbType.Int32, 4).Value = (width);
                cmd.AddInParameter("@height", DbType.Int32, 4).Value = (height);
                cmd.AddInParameter("@upload_date", DbType.Date).Value = (DateTime.Today);
                cmd.AddInParameter("@uploader", DbType.String, 50).Value = (HttpContext.Current.User.Identity.Name);
                cmd.AddInParameter("@edit_Date", DbType.Date).Value = (DateTime.Today);
                cmd.AddInParameter("@editor", DbType.String, 50).Value = (HttpContext.Current.User.Identity.Name);
                cmd.AddInParameter("@remarks", DbType.String, 50).Value = (TextBox_Remarks.Text);
                cmd.AddInParameter("@type", DbType.Int32, 4).Value = (DropDownList_Type.SelectedValue);
                cmd.AddInParameter("@color", DbType.String, 50).Value = (DropDownList_Color.SelectedValue);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    CreateMessageAlert("Success", "Save data success.");
                    setRepeaterAnnot(Request.QueryString["ImageID"]);
                    getEditImage(Request.QueryString["ImageID"]);
                    TextBox_Start.Text = "";
                    TextBox_End.Text = "";
                }
                catch (Exception ex)
                {
                    CreateMessageAlert("Error", "Save data failed. Loc : Button_Save_Click. Exception : " + ex.Message);
                }

            }
        }

        protected void Button_Edit_Click(object sender, EventArgs e)
        {
            string[] arr1 = TextBox_Start_Edit.Text.Split(",");
            string[] arr2 = TextBox_End_Edit.Text.Split(",");

            int pos_top, pos_left, width, height;

            pos_top = arr1[1].ToInt32(0);
            pos_left = arr1[0].ToInt32(0);
            height = arr2[1].ToInt32(0) - pos_top;
            width = arr2[0].ToInt32(0) - pos_left;

            using (DbConnection conn = ConnectionHelper.GetConnection())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE GWISAnnotations SET color = @color, pos_left = @pos_left, pos_top = @pos_top, width = @width, height = @height, remarks = @remarks, modifieddate = @edit_date, modifiedby = @editor WHERE id = @id";

                cmd.AddInParameter("@pos_top", DbType.Int32, 4).Value = (pos_top);
                cmd.AddInParameter("@pos_left", DbType.Int32, 4).Value = (pos_left);
                cmd.AddInParameter("@width", DbType.Int32, 4).Value = (width);
                cmd.AddInParameter("@height", DbType.Int32, 4).Value = (height);
                cmd.AddInParameter("@edit_date", DbType.Date).Value = (DateTime.Today);
                cmd.AddInParameter("@editor", DbType.String, 50).Value = (HttpContext.Current.User.Identity.Name);
                cmd.AddInParameter("@remarks", DbType.String, 50).Value = (TextBox_Remarks_Edit.Text);
                cmd.AddInParameter("@type", DbType.Int32, 4).Value = (DropDownList_Type.SelectedValue);
                cmd.AddInParameter("@color", DbType.String, 50).Value = (DropDownList_Color_Edit.SelectedValue);
                cmd.AddInParameter("@id", DbType.Int32, 4).Value = (HiddenField_Id_Edit.Value);

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    CreateMessageAlert("Success", "Edit data success.");
                    setRepeaterAnnot(Request.QueryString["ImageID"]);
                    getEditImage(Request.QueryString["ImageID"]);
                    TextBox_Start.Text = "";
                    TextBox_End.Text = "";
                }
                catch (Exception ex)
                {
                    CreateMessageAlert("Error", "Edit data failed. Loc : Button_Edit_Click. Exception : " + ex.Message);
                }

            }
        }

        protected void Button_Delete_Data_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            var array = btn.CommandArgument;
            var value = array.Split(",");
            int Id = value[0].ToInt32(0);
            string LastEdited = value[1];

            using (DbConnection conn = ConnectionHelper.GetConnection())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM GWISAnnotations WHERE id = @id";
                cmd.AddInParameter("@id", DbType.Int32, 4).Value = Id;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    CreateMessageAlert("Success", "Delete data success.");
                    setRepeaterAnnot(Request.QueryString["ImageID"]);
                    getEditImage(Request.QueryString["ImageID"]);
                }
                catch (Exception ex)
                {
                    CreateMessageAlert("Error","Delete data failed. Loc : Button_Delete_Data_Click. Exception : " + ex.Message);
                }
            }
        }

    }
}
