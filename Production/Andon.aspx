<%@ Page Title="Andon" Language="C#" MasterPageFile="~/Andon.master" EnableViewState="false" AutoEventWireup="true" CodeBehind="Andon.aspx.cs" Inherits="DotMercy.custom.Production.Andon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    PCD
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="Server">

    <style>
        .andonContainer {
            position: absolute;
            top: 65px;
            right: 0;
            bottom: 0;
            left: 0;
            border: 2px solid white;
            background-color: black;
        }

        .andonContainerBlink {
            position: absolute;
            top: 65px;
            right: 0;
            bottom: 0;
            left: 0;
            border: 2px solid white;
            animation: blinker 1s steps(2, start) infinite;
            -webkit-animation: blinker 1s steps(2, start) infinite;
        }
        #t1{
            position: relative;
        }

        #t2{
            position: relative;
        }

        #t3{
            position: relative;
        }

        #t4{
            position: relative;
            height: 100%;
        }

        @keyframes blinker {
            0% {
                background-color: black;
            }

            100% {
                background-color: #b21c08;
            }
        }

        @-webkit-keyframes blinker {
            0% {
                background-color: black;
            }

            100% {
                background-color: #b21c08;
            }
        }

        .andonTable {
            border: 2px solid white;
            padding: 10px;
        }
        
        #ctl00_ctl00_ASPxSplitter1_1 {
            background-color: black !important;
        }

        .marquee {
            width: 100%;
            overflow: hidden;
            white-space: nowrap;
            float: right;
            box-sizing: border-box;
        }

            .marquee:hover {
                animation-play-state: paused;
            }

        @keyframes marquee {
            0% {
                text-indent: 100%;
            }

            100% {
                text-indent: -100%;
            }
        }

        .panel-station {
            width: 19.68%;
            margin-right: 5px;
            border: solid 1px white;
            float: left;
            margin-bottom: 10px;
        }

        .panelText{
            font-size: 4.5vw;
        }

        .panelText2{
            font-size: 1.3vw;
        }
        .textX{
            font-size: 4.5vw;
            color: red;
        }
        .textX2{
            font-size: 5vw;
            color: red;
        }
        .textX3{
            font-size: 3vw;
            color: white;
        }
        .actPlan{
            width: 30%; text-align: center; display: flex; flex-direction: column; justify-content: center;
        }
        .div-header-station {
            border-bottom: solid 1px white;
            text-align: center;
            height: auto;
        }

        .div-body-station {
            text-align: center;
        }

        .div-footer-station {
            word-break: break-all;
            border-top: solid 1px white;
            text-align: center;
        }
    </style>

    <script src="/Content/jquery.marquee.min.js" type="text/javascript"></script>
    <script src="/Content/moment.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var interval = <%= POLL_INTERVAL %>;
        var andonAlertJson = <%=AndonAlertJson%>; 
        var schedss =  <%=Scheduless%>;   
        var IsPause = false;
        var countdown = 1000;

        function ExecuteLocalTime() {
            //setInterval(function () { PageMethods.GetCurrentTime(OnSuccess); }, countdown);
            setInterval(function () { ShowDateTime(); }, countdown);
        }

        function OnSuccess(response, userContext, methodName) {
            lblTime.SetText(response);
        }

        function ShowDateTime()
        {
            lblTime.SetText(moment().format('dddd, DD MMMM YYYY, h:mm:ss A'));
        }

        //traffict lights
        function ExecuteCondition(s, e) {
            console.log("refresh condition execute!");
            setTimeout(function () { CallbackPanelCondition.PerformCallback(); }, interval)
        }

        function ExecuteCurrentTime(s, e) {
            console.log("refresh current execute!");
        
   

            if (fields.Get("cp_action") == undefined) {
                CallbackPanelCurrent.PerformCallback("start");
                return;
            }

           

            var action = "reset";
            if (fields.Get("cp_action") != undefined) {
                action = fields.Get("cp_action");
            }

          
           

            //start counting again
            if (action == "reset" || action == "resume")
            {
                var t = 0;
                if (fields.Get("cp_countdown") != undefined) {
                    t = fields.Get("cp_countdown");
                }

                var seed = 0;
                if (fields.Get("cp_seed") != undefined) {
                    seed = fields.Get("cp_seed");
                }

                if (t != 0)
                    setTimeout(function () { doCountdown(seed, t - (countdown / 1000)); }, countdown);
            }

            setTimeout(function () { CallbackPanelCurrent.PerformCallback("check"); }, interval)
        }

        function doCountdown(seed, t) {
            var datetime = moment().format('HH:mm'); 
            schedss =  <%=Scheduless%>; 

            if (IsPause == false)
            {

                var origSeed = 0;
                if (fields.Get("cp_seed") != undefined) {
                    origSeed = fields.Get("cp_seed");
                }

                if (seed == origSeed)
                {
                    var seconds = Math.floor((t) % 60);
                    var minutes = Math.floor((t / 60) % 60);
                    var hours = Math.floor((t / (60 * 60)) % 24);


                    var time  = ('0' + hours).slice(-2) + ":" + ('0' + minutes).slice(-2) + ":" + ('0' + seconds).slice(-2);

                    var actual = lblActual.GetText();
                    var plan = lblPlanning.GetText();
                    if (plan > actual)
                    {
                        document.getElementById('audiotag').play();
                    }
                    else
                    {
                        for(var i = 0; i < andonAlertJson.length; i++) {
                            var obj = andonAlertJson[i];
                            if(time >= obj.AlertTimeEnd && time <= obj.AlertTimeStart)
                            {
                                document.getElementById('audiotag').play();
                            }
                        }
                    }
           
                    lblCountdown.SetText(time);


                    //continue counting
                    if (t == 0)
                        setTimeout(function () { doCountdown(seed, seed); }, countdown);
                    else
                        setTimeout(function () { doCountdown(seed, t - (countdown / 1000)); }, countdown);
                }
                else {
                    console.log("stopping current counter!");
                }

                fields.Set("cp_countdown", t);

                if (t == 0)
                {
                    var planned = 0;
                    if (fields.Get("cp_planned") != undefined) {
                        planned = fields.Get("cp_planned");
                    }
                    //increment planned value
                    planned += 1;
                    fields.Set("cp_planned", planned);
                    lblPlanning.SetText(planned);

               

                    //callback in case any need for server side action
                    CallbackPanelCurrent.PerformCallback("finish");
                }
            }

            //break time
            for(var i = 0; i < schedss.length; i++) {
                var obj = schedss[i];
                if(obj.BreakStart >=  datetime && obj.BreakStart <= datetime)
                {  
                    IsPause = true;
                   // doCountdown(seed, t); 
                    break;
                }
                else
                {
                    IsPause = false;
                   // doCountdown(seed, t);
                }
            }
           
            if (IsPause == true)
            {
                setTimeout(function () { doCountdown(seed, seed); }, countdown);
            }
        }

        //running text
        function ExecuteFault(s, e) {
          //  console.log("refresh fault execute!");

            var elementHeights = $('.div-footer-station').map(function() {
                return $(this).height();
            }).get();
            var maxHeight = Math.max.apply(null, elementHeights);
            $('.div-footer-station').height(maxHeight);


            if (s.cp_test != undefined && s.cp_test.length > 0) {

                if (s.cp_test =="havePro")
                {
                    $('.andonContainer').addClass('andonContainerBlink');
                    $('.whiteText2').css('color', 'white');
                }
                else
                {
                    $('.andonContainer').removeClass('andonContainerBlink');
                    $('.whiteText2').css('color', 'red');
                }
                /// $('.andonContainer').Class('andonContainerBlink');
                //alert(s.cp_test);
                delete(s.cp_test);
            }

            //update text from server
            if (s.cp_runningtext != undefined && s.cp_runningtext.length > 0) {
                $('.marquee').text(s.cp_runningtext);
                delete(s.cp_runningtext);
            }

            if ($('.marquee').text().length > 0) {
                console.log($('.marquee').text());
                //start the marquee
                $('.marquee')
                    .bind('finished', function () {
                        //reset the text first to avoid blinking
                        $(this).text(" ");
                        //Change text to something else after first loop finishes
                        $(this).marquee('destroy');
                        //Load new content using Ajax 
                        CallbackPanelFault.PerformCallback();
                    })
                    .marquee();
            }
            else {
                //callback again later
                setTimeout(function () { CallbackPanelFault.PerformCallback(); }, interval)
            }
        }

        function StartAll() {
            ExecuteLocalTime();
            ExecuteCurrentTime(CallbackPanelCurrent);
            ExecuteCondition();
            ExecuteFault(CallbackPanelFault);
        }

        //$(document).ready(function () {
        //    StartAll();
        //});

        window.onload = StartAll;

        window.onkeydown = disableF5;

        // slight update to account for browsers not supporting e.which
        function disableF5(e) { if ((e.which || e.keyCode) == 116) e.preventDefault(); };

        // in jquery 3.x, andSelf() is replaced with addBack()
        $.fn.andSelf = function () {
            return this.addBack.apply(this, arguments);
        }



       
    </script>

    <div>
        <audio id="audiotag" src="../../Content/audio/Alarm.mp3"></audio>
       <%-- <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>
        <asp:ScriptManager runat="server" ID="ScriptManager" EnablePageMethods="true">
        </asp:ScriptManager>
        <div id="andonCntr" class="andonContainer" runat="server">
            <div id="t1" runat="server" class="andonTable" style="color: white;">
                <div class="row">
                    <div class="col-sm-4">
                        <dx:ASPxLabel ID="lblLine" runat="server" ClientInstanceName="lblLine" Font-Size="5em" Style="float: left"></dx:ASPxLabel>
                    </div>
                    <div class="col-sm-8">
                        <dx:ASPxLabel ID="lblTime" runat="server" ClientInstanceName="lblTime" Font-Size="5em" Style="float: right"></dx:ASPxLabel>
                    </div>
                </div>
            </div>

            <div id="t2" runat="server" class="andonTable" style="color: red; min-height: 90px">
                <dx:ASPxCallbackPanel runat="server" ID="CallbackPanelFault" ClientInstanceName="CallbackPanelFault" RenderMode="Div" OnCallback="CallbackPanelFault_Callback">
                    <ClientSideEvents EndCallback="ExecuteFault"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <div class="marquee whiteText2" id="runningText" runat="server" style="font-size: 5em;" data-duration='10000' data-gap='10' data-startvisible="false" width="100%">
                                Starting up... Have a good day!
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>

            <div id="t3" class="andonTable">
                <dx:ASPxCallbackPanel runat="server" ID="CallbackPanelCurrent" ClientInstanceName="CallbackPanelCurrent" RenderMode="Div" OnCallback="CallbackPanelCurrent_Callback">
                    <ClientSideEvents EndCallback="ExecuteCurrentTime"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContentPlan" runat="server">
                            <dx:ASPxHiddenField ID="fields" ClientInstanceName="fields" runat="server" />
                            <div style="width: 100%; display: inline-flex;">
                                <div style="width: 40%; text-align: center">
                                    <dx:ASPxLabel Text="DownTime" runat="server" CssClass="textX3"></dx:ASPxLabel> <br />
                                    <dx:ASPxLabel runat="server" ID="lblCountdown" ClientInstanceName="lblCountdown" CssClass="whiteText2 textX2"></dx:ASPxLabel>
                                </div>
                                <div class="actPlan">
                                    <dx:ASPxLabel Text="Planning" runat="server" CssClass="textX3"></dx:ASPxLabel> <br />
                                    <dx:ASPxLabel ID="lblPlanning" ClientInstanceName="lblPlanning" Text="" runat="server" CssClass="whiteText2 textX"></dx:ASPxLabel>
                                </div>
                                <div class="actPlan">
                                    <dx:ASPxLabel Text="Actual" runat="server" CssClass="textX3"></dx:ASPxLabel> <br />
                                    <dx:ASPxLabel ID="lblActual" ClientInstanceName="lblActual" Text="" runat="server" CssClass="whiteText2 textX"></dx:ASPxLabel>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>

            <div id="t4" class="andonTable" style="color: white;">
                <dx:ASPxCallbackPanel runat="server" ID="CallbackPanelCondition" ClientInstanceName="CallbackPanelCondition" RenderMode="Div">
                    <ClientSideEvents EndCallback="ExecuteCondition"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContentCondition" runat="server">
                            <div id="divPanelCondition" runat="server" style="padding: 5px; display: inline-flex; flex-flow: row wrap; width: 100%; align-content: center"></div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>
        </div>
        <asp:HiddenField ID="hfFaults" runat="server" />
        <asp:HiddenField ID="hfPlanning" runat="server" />
        <asp:HiddenField ID="hfActual" runat="server" />
    </div>
</asp:Content>
