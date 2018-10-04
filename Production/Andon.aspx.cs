using DevExpress.Web;
//using DevExpress.Web.ASPxGauges;
//using DevExpress.Web.ASPxGauges.Gauges;
//using DevExpress.Web.ASPxGauges.Gauges.State;
//using DevExpress.XtraGauges.Core.Model;
using DotWeb.Models;
using DotWeb.Repositories;
using DotWeb.Utils;
using Ebiz.Scaffolding.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotMercy.custom.Production
{
    public partial class Andon : System.Web.UI.Page
    {
        private static string IMG_STATUS_RED = "/Content/Images/status-red.png";
        private static string IMG_STATUS_GREEN = "/Content/Images/status-green.png";
        private static string DEFAULT_STARTTIME = "07:45";
        private static string DEFAULT_ENDTIME = "16:00";

        private int assySectionId = 0;
        private int lineId = 0;
        private int day = 0;
        private string stationAct = "";
        protected int POLL_INTERVAL = AppConfiguration.ANDON_POLL_INTERVAL_MSEC;

        public List<AndonAlertStartEnd> ListAndonAlert = null;
        
        public string AndonAlertJson;
        public string Scheduless;

        protected void Page_Init(object sender, EventArgs e)
        {
            ListAndonAlert = ProductionRepository.GetAndonAlert();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            AndonAlertJson = oSerializer.Serialize(ListAndonAlert);

            DateTime date = DateTime.Now;
            DateTime dateCustom;
            dateCustom = ProductionRepository.GetAndonOverideDate();
            var dayCode = 1;

            if (DateTime.Today == dateCustom)
            {
                dayCode = 3;//custom
            }
            else
            {
                dayCode = date.DayOfWeek == DayOfWeek.Friday ? 2 : 1; //1 normal day, 2 jumuah

            }

            List<TimeSchedule> sched = ProductionRepository.GetAndonDailySchedules(dayCode);

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer2 = new System.Web.Script.Serialization.JavaScriptSerializer();
            Scheduless = oSerializer2.Serialize(sched);

          //  


            int assemblyTypeId = Session["assemblyType"].ToInt32(1);

            //set the poll interval
            int POOL_INTERVAL = AppConfiguration.ANDON_POLL_INTERVAL_MSEC;

            //get line-id and stationAct


            //int LineNumber = Request.QueryString["LineNumber"].ToInt32(1);
            //lineId = ProductionRepository.GetProductionLine(assemblyTypeId).Where(c => c.LineNumber == LineNumber).Select(c => c.Id).ToInt32(1);

            lineId = Request.QueryString["Line"].ToInt32(1);
      
            stationAct = Request.QueryString["stationAct"].ToString(string.Empty);

            //if (stationAct == string.Empty & lineId == 0)
            //{

            //    Response.Redirect("../Production/ViewAndon.aspx");
            //}
            //DateTime dt = DateTime.Parse("09:00:00");

            ////get sched for the day. we don't handle of weekend,(saturday or sunday)
            //var dayCode = dt.DayOfWeek == DayOfWeek.Friday ? 2 : 1; //1 normal day, 2 jumuah
            //List<TimeSchedule> sched = ProductionRepository.GetTimeScheduleDaily(dayCode);

            //ETimeOfDayType timeType = GetTimeType(dt);

            if (!IsPostBack)
            {
                var line = ProductionRepository.GetLines(assemblyTypeId).Where(x => x.Id == lineId).FirstOrDefault();
                var section = ProductionRepository.GetAssemblySections(assemblyTypeId).Where(x => x.Id == assySectionId).FirstOrDefault();

                var pattern = "([a-z?])[_ ]?([A-Z])";
                var output = Regex.Replace(stationAct, pattern, "$1 $2");

                lblLine.Text = output + " " + line.LineName;

                //loadRunningText(lineId, assySectionId, dt, sched);
                //loadCountdown(lineId, assySectionId, dt, sched);
                loadStationStatus(lineId, assySectionId, stationAct);
            }

            //list of stations are built dynamically by code-behind. so it has to be rebuilt everytime!
            if (CallbackPanelCondition.IsCallback)
                loadStationStatus(lineId, assySectionId, stationAct);

            //if (CallbackPanelCurrent.IsCallback)
            //    loadCountdown(lineId, assySectionId, dt, sched);

            //if (CallbackPanelFault.IsCallback)
            //    loadRunningText(lineId, assySectionId, dt, sched);
        }

        public void loadStationStatus(int LineId, int AssySectionId, string StationAct)
        {
            var ProdItem = ProductionRepository.GetAndonStationStatus(LineId, AssySectionId, DateTime.Now, StationAct);
           


            int i = 0;
            foreach (ProductionStationStatus item in ProdItem)
            {
                string name = "Status" + i;

                //Generate new panelStation
                ASPxPanel panelStation = new ASPxPanel();
                HtmlGenericControl divHeader = new HtmlGenericControl("div");
                HtmlGenericControl divBody = new HtmlGenericControl("div");
                HtmlGenericControl divFooter = new HtmlGenericControl("div");
                HtmlGenericControl divFooterFin = new HtmlGenericControl("div");
                HtmlGenericControl br = new HtmlGenericControl("br");


                //Generate new labelStation in divHeader
                ASPxLabel lblStation = new ASPxLabel();
                //lblStation.Font.Size = new FontUnit("small");
                lblStation.ID = name + "Header";
                lblStation.ClientInstanceName = lblStation.ID;
                //lblStation.Text = item.StationName;
                lblStation.Text = item.StationInitial; 
                lblStation.CssClass = "panelText";

                if (item.FaultCount > 0)
                {

                    divHeader.Style.Add("background-color", "red");
                    lblStation.Style.Add("color", "white");
                }


                divHeader.Controls.Add(lblStation);
                divHeader.Attributes.Add("class", "div-header-station");

                //Generate new imgStatus in divBody
                ASPxImage imgStatus = new ASPxImage();
                imgStatus.ID = name + "Status";
                imgStatus.ClientInstanceName = imgStatus.ID;
                imgStatus.Width = 90;
                imgStatus.Height = 90;

                if (item.FINNumber != string.Empty)
                    imgStatus.ImageUrl = IMG_STATUS_GREEN;
                else
                    imgStatus.ImageUrl = IMG_STATUS_RED;

                divBody.Controls.Add(imgStatus);
                divBody.Attributes.Add("class", "div-body-station");

                //Generate new lblModelVariant in divFooter
                ASPxLabel lblModelVariant = new ASPxLabel();
                //lblModelVariant.Font.Size = new FontUnit("small");
                lblModelVariant.ID = name + "Footer";
                lblModelVariant.ClientInstanceName = lblModelVariant.ID;
                lblModelVariant.Text = item.ModelName + " " + item.VariantName;
                lblModelVariant.CssClass = "panelText2";
                //lblModelVariant.Text = item.VINNumber;




                //endtes
                divFooter.Controls.Add(lblModelVariant);
                divFooter.Controls.Add(br);
                divFooter.Attributes.Add("class", "div-footer-station");

                if (item.FINNumber.Length > 0)
                {
                    // finnumber
                    int len = item.FINNumber.Length - 6;
                    ASPxLabel FIN = new ASPxLabel();
                    //FIN.Font.Size = new FontUnit("small");
                    FIN.ID = item.FINNumber + "Footer";
                    FIN.ClientInstanceName = item.FINNumber;
                    FIN.CssClass = "panelText2";

                    FIN.Text = item.FINNumber.Substring(len, 6);

                    divFooter.Controls.Add(FIN);
                }



                panelStation.Controls.Add(divHeader);
                panelStation.Controls.Add(divBody);
                panelStation.Controls.Add(divFooter);
                panelStation.CssClass = "panel-station";

                divPanelCondition.Controls.Add(panelStation);
                //   Response.Redirect(Request.RawUrl);
            }
        }

        public void loadRunningText(int LineId, int AssySectionId, DateTime dt, List<TimeSchedule> sched)
        {
        }

        public void loadCountdown(int LineId, int AssySectionId, DateTime date, List<TimeSchedule> sched)
        {
        }

        //public void loadFault()
        //{
        //    HtmlGenericControl divRunningText = new HtmlGenericControl();
        //    divRunningText.Style.Add("font-size", "50px");

        //    if (_isWorkingTime)
        //    {
        //        if (_tempFaults != hfFaults.Value)
        //        {
        //            hfFaults.Value = _tempFaults;
        //            divRunningText.InnerHtml = _tempFaults;
        //        }
        //        else
        //        {
        //            divRunningText.InnerHtml = _tempFaults;
        //        }
        //    }
        //    else if (_isBreakTime)
        //    {
        //        hfFaults.Value = "Istirahat woiii";
        //        _tempFaults = "Istirahat woiii";
        //        divRunningText.InnerHtml = "Istirahat woiii";
        //    }
        //    else
        //    {
        //        hfFaults.Value = "Standing by...";
        //        _tempFaults = "Standing by...";
        //        divRunningText.InnerHtml = "Standing by...";
        //    }

        //    runningText.Controls.Add(divRunningText);

        //}

        [WebMethod]
        public static string GetCurrentTime()
        {
            return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        public static string FormatDate(int seconds)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (seconds / (60 * 60)) % 24, seconds / 60, seconds % 60);
        }

        public static int GetElapsedTimeInSeconds(DateTime Date, List<TimeSchedule> sched)
        {
            if (sched == null || sched.Count == 0)
                return 0;

            DateTime startTime = DateTime.Parse(sched[0].StartTime);
            DateTime endTime = DateTime.Parse(sched[0].EndTime);

            TimeSpan ts2 = Date.TimeOfDay;
            DateTime actualTime = Date;

            //break time
            foreach (var ld in sched)
            {
                //break
                if (ts2 >= DateTime.Parse(ld.BreakStart).TimeOfDay && ts2 <= DateTime.Parse(ld.BreakEnd).TimeOfDay)
                {
                    actualTime = DateTime.Parse(ld.BreakStart);
                }
            }



            if (actualTime > endTime)
                actualTime = endTime;

            int elapsedTime = (int)(actualTime - startTime).TotalSeconds;
            //if current date is before start-time, we have not started working!
            if (elapsedTime <= 0)
                return 0;

            int elapsedBreakTime = 0;
            List<TimeSchedule> breaks = sched.Where(s => DateTime.Parse(s.BreakEnd) < Date).ToList();
            foreach (TimeSchedule ts in breaks)
            {
                elapsedBreakTime += (int)(DateTime.Parse(ts.BreakEnd) - DateTime.Parse(ts.BreakStart)).TotalSeconds;
            }

            //effective elapsedtime
            elapsedTime -= elapsedBreakTime;

            return elapsedTime;
        }

        public static int GetTactTimeInSeconds(int productionTarget, List<TimeSchedule> sched)
        {
            if (sched == null || sched.Count == 0 || productionTarget <= 0)
                return 0;

            int totalWorkingTime = (int)(DateTime.Parse(sched[0].EndTime) - DateTime.Parse(sched[0].StartTime)).TotalSeconds;

            int totalBreakTime = 0;
            foreach (TimeSchedule ts in sched)
            {
                totalBreakTime += (int)(DateTime.Parse(ts.BreakEnd) - DateTime.Parse(ts.BreakStart)).TotalSeconds;
            }

            return (totalWorkingTime - totalBreakTime) / productionTarget;
        }

        //public static int GetCountDownTimeInSeconds(int LineId, int AssySectionId, DateTime Date, int target, List<TimeSchedule> sched)
        //{
        //    if (sched == null || sched.Count == 0 || target <= 0)
        //        return 0;

        //    int countdown = 0;

        //    int tackTime = GetTactTimeInSeconds(target, sched);

        //    DateTime startTime = DateTime.Parse(sched[0].StartTime);
        //    DateTime actualStartTime = ProductionRepository.GetAndonLastItemCompletionDate(LineId, AssySectionId, Date);
        //    if (startTime > actualStartTime)
        //        actualStartTime = startTime;

        //    int elapsedTime = (int)(Date - actualStartTime).TotalSeconds;
        //    //if current date is before start-time, we have not started working!
        //    if (elapsedTime < 0)
        //        elapsedTime = 0;

        //    int elapsedBreakTime = 0;
        //    List<TimeSchedule> breaks = sched.Where(s => DateTime.Parse(s.BreakEnd) < Date).ToList();
        //    foreach (TimeSchedule ts in breaks)
        //    {
        //        elapsedBreakTime += (int)(DateTime.Parse(ts.BreakEnd) - DateTime.Parse(ts.BreakStart)).TotalSeconds;
        //    }

        //    //effective elapsedtime
        //    elapsedTime -= elapsedBreakTime;

        //    //calculate countdown
        //    if (tackTime > 0)
        //        countdown = (tackTime) - (elapsedTime);

        //    return countdown;
        //}

        //public static int GetCountDownTimeV2InSeconds(int lineId, int assySectionId, DateTime date, int remainingTarget, List<TimeSchedule> sched)
        //{
        //    if (sched == null || sched.Count == 0 || remainingTarget <= 0)
        //        return 0;

        //    DateTime startTime = DateTime.Parse(sched[0].StartTime);
        //    DateTime endTime = DateTime.Parse(sched[0].EndTime);

        //    DateTime actualStartTime = ProductionRepository.GetAndonLastItemCompletionDate(lineId, assySectionId, date);
        //    if (startTime > actualStartTime)
        //        actualStartTime = startTime;

        //    int totalWorkingTime = (int)(endTime - actualStartTime).TotalSeconds;

        //    int totalBreakTime = 0;
        //    foreach (TimeSchedule ts in sched)
        //    {
        //        DateTime startBreak = DateTime.Parse(ts.BreakStart);
        //        DateTime endBreak = DateTime.Parse(ts.BreakEnd);

        //        if (endBreak < actualStartTime) continue;

        //        DateTime actualStartBreak = actualStartTime;
        //        if (startBreak > actualStartTime)
        //            actualStartBreak = startBreak;

        //        totalBreakTime += (int)(endBreak - actualStartBreak).TotalSeconds;
        //    }

        //    int tackTime = (totalWorkingTime - totalBreakTime) / remainingTarget;

        //    int elapsedTime = (int)(date - actualStartTime).TotalSeconds;
        //    //if current date is before start-time, we have not started working!
        //    if (elapsedTime < 0)
        //        elapsedTime = 0;

        //    int elapsedBreakTime = 0;
        //    List<TimeSchedule> breaks = sched.Where(s => DateTime.Parse(s.BreakEnd) < date).ToList();
        //    foreach (TimeSchedule ts in breaks)
        //    {
        //        elapsedBreakTime += (int)(DateTime.Parse(ts.BreakEnd) - DateTime.Parse(ts.BreakStart)).TotalSeconds;
        //    }

        //    //effective elapsedtime
        //    elapsedTime -= elapsedBreakTime;

        //    //calculate countdown
        //    int countdown = 0;
        //    if (tackTime > 0)
        //        countdown = (tackTime) - elapsedTime;

        //    return countdown;
        //}

        public static ETimeOfDayType GetTimeType(DateTime date, List<TimeSchedule> sched)
        {
            //if (sched == null)
            //{
            //    //we don't handle of weekend,(saturday or sunday)
            //    var dayCode = date.DayOfWeek == DayOfWeek.Friday ? 2 : 1; //1 normal day, 2 jumuah
            //    sched = ProductionRepository.GetAndonDailySchedules(dayCode);
            //}

            if (sched == null || sched.Count == 0)
                return ETimeOfDayType.OffTime;

            TimeSpan ts = date.TimeOfDay;

            //off time
            if (ts < DateTime.Parse(sched[0].InTime).TimeOfDay || ts > DateTime.Parse(sched[0].EndTime).TimeOfDay)
            {
                return ETimeOfDayType.OffTime;
            }

            //ready time
            if (ts >= DateTime.Parse(sched[0].InTime).TimeOfDay && ts < DateTime.Parse(sched[0].StartTime).TimeOfDay)
            {
                return ETimeOfDayType.ReadyTime;
            }

            //break time
            foreach (var ld in sched)
            {
                //break
                if (ts >= DateTime.Parse(ld.BreakStart).TimeOfDay && ts <= DateTime.Parse(ld.BreakEnd).TimeOfDay)
                {
                    return ETimeOfDayType.BreakTime;
                }
            }

            //else: working time
            return ETimeOfDayType.WorkingTime;
        }

        protected void CallbackPanelFault_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

            DateTime date = DateTime.Now;

            DateTime dateCustom;
            dateCustom = ProductionRepository.GetAndonOverideDate();
            var dayCode = 1;

            if (DateTime.Today == dateCustom)
            {
                dayCode = 3;//custom
            }
            else
            {
                dayCode = date.DayOfWeek == DayOfWeek.Friday ? 2 : 1; //1 normal day, 2 jumuah

            }
            List<TimeSchedule> sched = ProductionRepository.GetAndonDailySchedules(dayCode);

            string text = "";

            ETimeOfDayType timeType = GetTimeType(date, sched);
            if (timeType == ETimeOfDayType.BreakTime)
            {
                text = AppConfiguration.ANDON_MSG_BREAKTIME;
                //store current fault id in session
                Session["AndonFaultId"] = -1;
            }
            else if (timeType == ETimeOfDayType.ReadyTime)
            {
                text = AppConfiguration.ANDON_MSG_READYTIME;
                //store current fault id in session
                Session["AndonFaultId"] = -1;
            }
            else if (timeType == ETimeOfDayType.OffTime)
            {
                text = AppConfiguration.ANDON_MSG_OFFTIME;
                //store current fault id in session
                Session["AndonFaultId"] = -1;
            }
            else
            {
                //text
                var textlist = ProductionRepository.GetAndonProductionProblem(lineId, DateTime.Now, stationAct, 0);

                foreach (var item in textlist)
                {
                    text += string.Format(" {0} : {1} ", item.StationName, item.ProblemName);

                }

                if (textlist.Count() > 0)
                {
                    CallbackPanelFault.JSProperties["cp_test"] = "havePro";

                }
                else
                {
                    CallbackPanelFault.JSProperties["cp_test"] = "noPro";

                }
                //set to empty string to avoid flicker
                runningText.InnerText = "";

                //pass text to client
                CallbackPanelFault.JSProperties["cp_runningtext"] = text;




                //working time



                var faults = ProductionRepository.GetAndonProductionFaults(lineId, assySectionId, DateTime.Now);

                if (faults.Count() == 0)
                {
                    runningText.InnerText = "";
                    //store current fault id in session
                    Session["AndonFaultId"] = -1;
                    return;
                }

                int faultId = 0;
                if (Session["AndonFaultId"] != null)
                    faultId = Session["AndonFaultId"].ToInt32(0);

                //get index of current running fault
                int faultIdx = 0;
                if (faultId > 0)
                {
                    for (int i = 0; i < faults.Count(); i++)
                    {
                        if (faults[i].Id == faultId)
                        {
                            faultIdx = i;
                            break;
                        }
                    }

                    //get the next fault to show
                    if (faultIdx >= 0 && faultIdx < faults.Count - 1)
                        faultIdx++;
                    else
                        faultIdx = 0;
                }

                ProductionFault fau = faults[faultIdx];
                // text = string.Format("{0} {1} : {2}", fau.StationName, fau.Classification, fau.PartDescription);



                //store current fault id in session
                Session["AndonFaultId"] = fau.Id;
            }

            //set to empty string to avoid flicker
            runningText.InnerText = "";

            //pass text to client
            CallbackPanelFault.JSProperties["cp_runningtext"] = text;
        }

        protected void CallbackPanelCurrent_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime date = DateTime.Now;
    

            DateTime dateCustom;
            dateCustom = ProductionRepository.GetAndonOverideDate();
            var dayCode = 1;

            if (DateTime.Today == dateCustom)
            {
                dayCode = 3;//custom
            }
            else
            {
                dayCode = date.DayOfWeek == DayOfWeek.Friday ? 2 : 1; //1 normal day, 2 jumuah

            }


           

            List<TimeSchedule> sched = ProductionRepository.GetAndonDailySchedules(dayCode);


            System.Web.Script.Serialization.JavaScriptSerializer oSerializer2 = new System.Web.Script.Serialization.JavaScriptSerializer();
            Scheduless = oSerializer2.Serialize(sched);



            ETimeOfDayType timeType = GetTimeType(date, sched);

            int target = ProductionRepository.GetAndonProductionTarget(lineId, assySectionId, date, stationAct);
            int tactTime = GetTactTimeInSeconds(target, sched);
            //if (tactTime == 0) tactTime = 1;

            int elapsedTime = GetElapsedTimeInSeconds(date, sched);
            
            int plan = 0;
            if (tactTime > 0) plan = elapsedTime / tactTime;
            lblPlanning.Text = plan.ToString();

            int actual = ProductionRepository.GetAndonProductionActual(lineId, date, stationAct);
            lblActual.Text = actual.ToString();

            int countdown = 0;





        
            if (timeType == ETimeOfDayType.OffTime || timeType == ETimeOfDayType.ReadyTime)
            {
                Session["AndonTactTime"] = tactTime;
                Session["AndonAchievement"] = actual;
                //dont countdown
                fields["cp_action"] = "stop";
                countdown = tactTime;
            }
            else if (timeType == ETimeOfDayType.BreakTime)
            {
                //Session["AndonTactTime"] = tactTime;
                //Session["AndonAchievement"] = actual;
                ////pause counting
                fields["cp_action"] = "pause";
                //if (fields.Contains("cp_countdown"))
                //{
                //    countdown = fields["cp_countdown"].ToInt32(0);
                //}

                Session["AndonTactTime"] = tactTime;
                Session["AndonAchievement"] = actual;
                //start counting
                if (tactTime == 0)
                    countdown = 0;
                else
                {
                    countdown = elapsedTime % tactTime;
                    countdown = tactTime - countdown;
                }
                //fields["cp_action"] = "reset";
                fields["cp_countdown"] = countdown;
                fields["cp_seed"] = tactTime;
                fields["cp_planned"] = plan;


                lblCountdown.Text = FormatDate(countdown);
            }
            else
            {
                if (e.Parameter == "start")
                {
                    Session["AndonTactTime"] = tactTime;
                    Session["AndonAchievement"] = actual;
                    //start counting
                    if (tactTime == 0)
                        countdown = 0;
                    else
                    {
                        countdown = elapsedTime % tactTime;
                        countdown = tactTime - countdown;
                    }
                    fields["cp_action"] = "reset";
                    fields["cp_countdown"] = countdown;
                    fields["cp_seed"] = tactTime;
                    fields["cp_planned"] = plan;
                }
                else if (e.Parameter == "check")
                {
                    int prevTact = Session["AndonTactTime"].ToInt32(0);
                    if (tactTime == prevTact)
                    {
                        Session["AndonAchievement"] = actual;
                        //keep counting
                        if (fields.Contains("cp_action") && fields["cp_action"].ToString() == "pause")
                            fields["cp_action"] = "resume";
                        else
                            fields["cp_action"] = "count";
                        if (fields.Contains("cp_countdown"))
                        {
                            countdown = fields["cp_countdown"].ToInt32(0);
                        }
                    }
                    else
                    {
                        Session["AndonTactTime"] = tactTime;
                        Session["AndonAchievement"] = actual;
                        //reset counting
                        countdown = elapsedTime % tactTime;
                        fields["cp_action"] = "reset";
                        fields["cp_countdown"] = countdown;
                        fields["cp_seed"] = tactTime;
                        fields["cp_planned"] = plan;
                    }
                }
                else //e.Parameter = "finish"
                {
                    //TODO

                    //keep counting down
                    fields["cp_action"] = "count";
                    if (fields.Contains("cp_countdown"))
                    {
                        countdown = fields["cp_countdown"].ToInt32(0);
                    }
                }
            }
           // lblCountdown.Text = FormatDate(countdown);

            //if (e.Parameter == "start")
            //{
            //    //store current fault id in session
            //    Session["AndonAchievement"] = null;
            //}

            //int achievement = -1;
            //if (Session["AndonAchievement"] != null)
            //    achievement = Session["AndonAchievement"].ToInt32(0);

            //int countdown = 0;
            //if (actual == achievement && fields.Contains("cp_countdown"))
            //{
            //    countdown = fields["cp_countdown"].ToInt32(0);
            //}
            //else
            //{
            //    countdown = GetCountDownTimeInSeconds(lineId, assySectionId, date, target, sched);
            //    //countdown = GetCountDownTimeV2InSeconds(lineId, assySectionId, date, (target - actual), sched);
            //    fields["cp_countdown"] = countdown;
            //    //fields["cp_seed"] = countdown;
            //}
            //lblCountdown.Text = FormatDate(countdown);

            //if (e.Parameter == "finish")
            //{
            //    //TODO: sound alarm?

            //    //keep counting down
            //    fields["cp_action"] = "count";
            //}
            //else //if (e.Parameter == "check")
            //{
            //    if (timeType == ETimeOfDayType.OffTime || timeType == ETimeOfDayType.ReadyTime)
            //    {
            //        //start of day
            //        Session["AndonAchievement"] = 0;

            //        //dont countdown
            //        fields["cp_action"] = "stop";
            //    }
            //    else if (timeType == ETimeOfDayType.BreakTime)
            //    {
            //        fields["cp_action"] = "pause";
            //    }
            //    else  //ETimeOfDayType.WorkingTime
            //    {
            //        if (actual == achievement)
            //        {
            //            if (fields.Contains("cp_action") && fields["cp_action"].ToString() == "pause")
            //                fields["cp_action"] = "resume";
            //            else
            //                fields["cp_action"] = "count";
            //        }
            //        else
            //        {
            //            //new actual -> reset
            //            fields["cp_action"] = "reset";
            //            fields["cp_seed"] = actual;

            //            Session["AndonAchievement"] = actual;
            //        }
            //    }
            //}   //if (e.Parameter == "finish")
        }
    }
}