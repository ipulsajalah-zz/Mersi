using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DotMercy.custom.Production
{
    public partial class ProductionTimeSchedule : Ebiz.Scaffolding.WebForm.UI.CustomPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        
        }
        protected void gridView_ParseValue(object sender, DevExpress.Web.Data.ASPxParseValueEventArgs e)
        {
            //ASPxGridView gridDetail = sender as ASPxGridView;
            //if (gridDetail.IsEditing)
            //{
            //    if (e.FieldName == "InTime" || e.FieldName == "StartTime" || e.FieldName == "BreakStart" || e.FieldName == "BreakEnd" || e.FieldName == "StopTime" || e.FieldName == "EndTime")
            //    {
            //        //DateTime? myDate = DateTime.Parse(e.Value.ToString());
            //        //string sqlFormattedDate = myDate.Value.ToString("HH:mm");
            //        e.Value = "10:00";

            //    }
            //}
        }
        protected void gridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //ASPxGridView gridDetail = sender as ASPxGridView;
            //if (gridDetail.IsEditing)
            //{
            //    if (e.Column.FieldName == "InTime")
            //    {
            //        if (e.KeyValue == DBNull.Value || e.KeyValue == null) return;

            //        DateTime? myDate = DateTime.Parse(e.Value.ToString());
            //        string sqlFormattedDate = myDate.Value.ToString("HH:mm");
                   
            //    }
            //} 
            
        }


        protected void gridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //{
        //    DateTime? myDate = DateTime.Today;

        //    if (e.Column.FieldName == "InTime")
        //    {
        //        if (e.Value != null)
        //        {
        //            if (e.Value.ToString() != string.Empty)
        //                e.Value = myDate.Value.AddHours(1);
        //        }
        //    }
        }

        //protected void ASPxGridView1_RowUpdated(object sender,aspxgr)
        ////protected void gridView_CellEditorInitialize(object sende e)
        ////{
        ////    if (e.Column.FieldName == "Time")
        ////    {
        ////        e.Editor.Value = DateTime.Parse(e.Value.ToString());
        ////    }
        ////}

    }
}