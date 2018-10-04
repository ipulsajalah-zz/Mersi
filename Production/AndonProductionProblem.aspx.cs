using DotWeb.Models;
using DotWeb.Utils;
using Ebiz.Scaffolding.Models;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{
    public partial class AndonProductionProblem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var user = HttpContext.Current.Session["user"] as User;
            if (user != null)
                e.NewValues[AppConfiguration.CREATEDBY_COLUMN] = user.UserName;
            else
                e.NewValues[AppConfiguration.CREATEDBY_COLUMN] = AppConfiguration.DEFAULT_USERNAME;
            e.NewValues[AppConfiguration.CREATEDDATE_COLUMN] = DateTime.Now;
        }

        protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var user = HttpContext.Current.Session["user"] as User;
            if (user != null)
                e.NewValues[AppConfiguration.MODIFIEDBY_COLUMN] = user.UserName;
            else
                e.NewValues[AppConfiguration.MODIFIEDBY_COLUMN] = AppConfiguration.DEFAULT_USERNAME;
            e.NewValues[AppConfiguration.MODIFIEDDATE_COLUMN] = DateTime.Now;
        }
    }
}