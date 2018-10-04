<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ToolCalibration.aspx.cs" Inherits="DotMercy.custom.ToolManagement.ToolCalibration" %>

<%--Copy Dari Sini--%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Tool Calibration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .Verification2 {
            visibility: hidden;
        }

        .Verification2Result {
            visibility: visible;
        }

        .Verification3 {
            visibility: hidden;
        }

        .Verification3Result {
            visibility: visible;
        }
    </style>
    <script type="text/javascript">
        function PickDate(s, e) {
            //Btn_Verify2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3Result";
            //if (ASPxClientUtils.GetKeyCode(e.htmlEvent) != ASPxKey.Enter && ASPxClientUtils.GetKeyCode(e.htmlEvent) != ASPxKey.Tab) {
            //    return;
            //}

            //var CheckDateNotNull = DateDetails.GetText();
            //if (CheckDateNotNull == "") {
            //    DateDetails.SetText(0);
            //}
            var dateFormat = function () {
                var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
                    timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
                    timezoneClip = /[^-+\dA-Z]/g,
                    pad = function (val, len) {
                        val = String(val);
                        len = len || 2;
                        while (val.length < len) val = "0" + val;
                        return val;
                    };

                // Regexes and supporting functions are cached through closure
                return function (date, mask, utc) {
                    var dF = dateFormat;

                    // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
                    if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                        mask = date;
                        date = undefined;
                    }

                    // Passing date through Date applies Date.parse, if necessary
                    date = date ? new Date(date) : new Date;
                    if (isNaN(date)) throw SyntaxError("invalid date");  // throw SyntaxError("invalid date");

                    mask = String(dF.masks[mask] || mask || dF.masks["default"]);

                    // Allow setting the utc argument via the mask
                    if (mask.slice(0, 4) == "UTC:") {
                        mask = mask.slice(4);
                        utc = true;
                    }

                    var _ = utc ? "getUTC" : "get",
                        d = date[_ + "Date"](),
                        D = date[_ + "Day"](),
                        m = date[_ + "Month"](),
                        y = date[_ + "FullYear"](),
                        H = date[_ + "Hours"](),
                        M = date[_ + "Minutes"](),
                        s = date[_ + "Seconds"](),
                        L = date[_ + "Milliseconds"](),
                        o = utc ? 0 : date.getTimezoneOffset(),
                        flags = {
                            d: d,
                            dd: pad(d),
                            ddd: dF.i18n.dayNames[D],
                            dddd: dF.i18n.dayNames[D + 7],
                            m: m + 1,
                            mm: pad(m + 1),
                            mmm: dF.i18n.monthNames[m],
                            mmmm: dF.i18n.monthNames[m + 12],
                            yy: String(y).slice(2),
                            yyyy: y,
                            h: H % 12 || 12,
                            hh: pad(H % 12 || 12),
                            H: H,
                            HH: pad(H),
                            M: M,
                            MM: pad(M),
                            s: s,
                            ss: pad(s),
                            l: pad(L, 3),
                            L: pad(L > 99 ? Math.round(L / 10) : L),
                            t: H < 12 ? "a" : "p",
                            tt: H < 12 ? "am" : "pm",
                            T: H < 12 ? "A" : "P",
                            TT: H < 12 ? "AM" : "PM",
                            Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                            o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                            S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                        };

                    return mask.replace(token, function ($0) {
                        return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
                    });
                };
            }();
            // Some common format strings
            dateFormat.masks = {
                "default": "ddd mmm dd yyyy HH:MM:ss",
                shortDate: "m/d/yy",
                mediumDate: "mmm d, yyyy",
                longDate: "mmmm d, yyyy",
                fullDate: "dddd, mmmm d, yyyy",
                shortTime: "h:MM TT",
                mediumTime: "h:MM:ss TT",
                longTime: "h:MM:ss TT Z",
                isoDate: "yyyy-mm-dd",
                isoTime: "HH:MM:ss",
                isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
                isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
            };

            // Internationalization strings
            dateFormat.i18n = {
                dayNames: [
                    "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
                    "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
                ],
                monthNames: [
                    "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
                    "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
                ]
            };

            // For convenience...
            Date.prototype.format = function (mask, utc) {
                return dateFormat(this, mask, utc);
            };
            ////$('#datepicker').datepicker({ minDate: 0 });
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth();
            var yyyy = today.getFullYear();
            var date = new Date(yyyy, mm, dd);
            var newdate = new Date(date);
            var Sum = DateDetails.GetText();
            newdate.setDate(newdate.getDate() + parseInt(Sum));
            var nd = new Date(newdate);
            var y = dateFormat(nd, "dddd, mmmm dS, yyyy, h:MM:ss TT");
            DateDetailsCalender.SetText(y);

            var CheckDay = DateDetailsCalender.GetText().split(",")[0];
            if (CheckDay == "Sunday") {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth();
                var yyyy = today.getFullYear();
                var date = new Date(yyyy, mm, dd);
                var newdate = new Date(date);
                var ValueNow = DateDetails.GetText();
                DateDetails.SetText(ValueNow - 2);
                var Sum = DateDetails.GetText();
                newdate.setDate(newdate.getDate() + parseInt(Sum));
                var nd = new Date(newdate);
                var y = dateFormat(nd, "dddd, mmmm dS, yyyy, h:MM:ss TT");
                DateDetailsCalender.SetText(y);
            }
            if (CheckDay == "Saturday") {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth();
                var yyyy = today.getFullYear();
                var date = new Date(yyyy, mm, dd);
                var newdate = new Date(date);
                var ValueNow = DateDetails.GetText();
                DateDetails.SetText(ValueNow - 1);
                var Sum = DateDetails.GetText();
                newdate.setDate(newdate.getDate() + parseInt(Sum));
                var nd = new Date(newdate);
                var y = dateFormat(nd, "dddd, mmmm dS, yyyy, h:MM:ss TT");
                DateDetailsCalender.SetText(y);
            }

        }
        function ClickMarkValidasi() {
            var GetValueNM = Text_NM.GetText();
            var GetValueVer1 = Text_VerValue1.GetText();
            var GetValueVer2 = Text_VerValue2.GetText();
            var GetValueVer3 = Text_VerValue3.GetText();
            if (GetValueNM == '') {
                alert('Please Fill Nm')
                return false;
            }
            if (GetValueVer1 == '') {
                alert('Please Fill Verification one')
                return false;
            }
            if (GetValueVer2 == '') {
                alert('Please Fill Verification two')
                return false;
            }
            if (GetValueVer3 == '') {
                alert('Please Fill Verification three')
                return false;
            }

        }
        function ClickSaveValidasi() {
            var GetValueNM = Text_NM.GetText();
            var GetValueVer1 = Text_VerValue1.GetText();
            var GetValueVer2 = Text_VerValue2.GetText();
            var GetValueVer3 = Text_VerValue3.GetText();
            if (GetValueNM == '') {
                alert('Please Fill Nm')
                return false;
            }
            if (GetValueVer1 == '') {
                alert('Please Fill Verification one')
                return false;
            }
            if (GetValueVer2 == '') {
                alert('Please Fill Verification two')
                return false;
            }
            if (GetValueVer3 == '') {
                alert('Please Fill Verification three')
                return false;
            }
            if (Text_VerValue1.GetMainElement().style.backgroundColor == "#ebccd1") {
                alert('Verification One Failed Please Verify again')
                return false;
            }
            if (Text_VerValue2.GetMainElement().style.backgroundColor == "#ebccd1") {
                alert('Verification Two Failed Please Verify again')
                return false;
            }
            if (Text_VerValue3.GetMainElement().style.backgroundColor == "#ebccd1") {
                alert('Verification three Failed Please Verify again')
                return false;
            }
        }
        function CollapsingDetailRow() {
            //  Btn_mark.SetEnabled(true);
        }
        function ExpandingDetailRow() {
            //   Btn_mark.SetEnabled(true);
        }
        function functionMaxMin() {
            Btn_mark.SetEnabled(false);
            var GetValueNM = Text_NM.GetText();
            if (GetValueNM > 10) {
                var Persentase = GetValueNM * (4 / 100);
                var NilMax = parseInt(GetValueNM) + Persentase;
                var NilMin = parseInt(GetValueNM) - Persentase;
                Text_Max.SetText(NilMax);
                Text_Min.SetText(NilMin);
            } else {
                var Persentase = GetValueNM * (6 / 100);
                var NilMax = parseInt(GetValueNM) + Persentase;
                var NilMin = parseInt(GetValueNM) - Persentase;
                Text_Max.SetText(NilMax);
                Text_Min.SetText(NilMin);
            }

        };
        function Validasi1() {

            var valueattempt = '';
            var GetValueRange = lblNmRange.GetText();
            var GetValueMax = parseFloat(GetValueRange.split('-')[1].split(' ')[0]);
            var PersenMax = GetValueMax * 0.01;
            // Start validasi1
            //20%
            var ValueText_VerValue1 = Text_VerValue_1_1.GetText();
            var ValuePersen1 = GetValueMax * (20 / 100);
            //40%
            var ValueText_VerValue2 = Text_VerValue_1_2.GetText();
            var ValuePersen2 = GetValueMax * (40 / 100);
            //60%
            var ValueText_VerValue3 = Text_VerValue_1_3.GetText();
            var ValuePersen3 = GetValueMax * (60 / 100);
            //80%
            var ValueText_VerValue4 = Text_VerValue_1_4.GetText();
            var ValuePersen4 = GetValueMax * (80 / 100);
            //100%
            var ValueText_VerValue5 = Text_VerValue_1_5.GetText();
            var ValuePersen5 = GetValueMax * (100 / 100);
            //End Validasi1
            //validasi 20%  10 < 9
            var GetRangePersen20 = ValuePersen1 - ValueText_VerValue1;
            if (Math.abs(GetRangePersen20) > PersenMax) {
                Text_VerValue_1_1.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_1_1.GetInputElement().style.backgroundColor = "#ebccd1";
                valueattempt += 1;
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
            }
            if (Math.abs(GetRangePersen20) <= PersenMax) {
                Text_VerValue_1_1.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_1_1.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi 40%
            var GetRangePersen40 = ValuePersen2 - ValueText_VerValue2;
            if (Math.abs(GetRangePersen40) > PersenMax) {
                Text_VerValue_1_2.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_1_2.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen40) <= PersenMax) {
                Text_VerValue_1_2.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_1_2.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi 60%
            var GetRangePersen60 = ValuePersen3 - ValueText_VerValue3;
            if (Math.abs(GetRangePersen60) > PersenMax) {
                Text_VerValue_1_3.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_1_3.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen60) <= PersenMax) {
                Text_VerValue_1_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_1_3.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);

            }
            //validasi80%
            var GetRangePersen80 = ValuePersen4 - ValueText_VerValue4;
            if (Math.abs(GetRangePersen80) > PersenMax) {
                Text_VerValue_1_4.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_1_4.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                //valueattempt += 1;
            }
            if (Math.abs(GetRangePersen80) <= PersenMax) {
                Text_VerValue_1_4.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_1_4.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi100%
            var GetRangePersen100 = ValuePersen5 - ValueText_VerValue5;
            if (Math.abs(GetRangePersen100) > PersenMax) {
                Text_VerValue_1_5.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_1_5.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen100) <= PersenMax) {
                Text_VerValue_1_5.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_1_5.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            if (parseFloat(valueattempt) >= PersenMax) {
                var getvalue = lbl_Attempt.GetText();



            }
            //if (parseInt(lbl_Attempt.GetText()) >= 3) {
            //    //Btn_mark.SetEnabled();
            //    Btn_mark.SetEnabled(true);
            //    Btn_Save.SetEnabled(false);
            //}
            if (Text_VerValue_1_1.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_1_2.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_1_3.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_1_4.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_1_5.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)") {
                Lbl_Verify.SetText("Success");
                Lbl_Verify.GetMainElement().style.backgroundColor = "rgb(214, 233, 198)";
                Lbl_Verify.GetMainElement().style.padding = "5px";
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                btn_barcode.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";

            }
            if (Text_VerValue_1_1.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_1_2.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_1_3.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_1_4.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_1_5.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)") {
                Lbl_Verify.SetText("Failed");
                Lbl_Verify.GetMainElement().style.backgroundColor = "rgb(235, 204, 209)";
                Lbl_Verify.GetMainElement().style.padding = "5px";
            }
            //  Btn_mark.SetEnabled(true);
            if (Lbl_Verify.GetText() == "Failed") {
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                Text_VerValue_2_1.SetClientVisible = true;
                Text_VerValue_2_1.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_2_2.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_2_3.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_2_4.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_2_5.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Btn_Verify2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                //Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                //Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Verify.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3";
                Btn_Verify2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3Result";
                //enable
                Text_VerValue_1_1.clientEnabled = false;
                Text_VerValue_1_2.clientEnabled = false;
                Text_VerValue_1_3.clientEnabled = false;
                Text_VerValue_1_4.clientEnabled = false;
                Text_VerValue_1_5.clientEnabled = false;
                lbl_Attempt.SetText(1);

            }
            if (parseInt(lbl_Attempt.GetText()) >= 3) {
                //Btn_mark.SetEnabled();
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
            }
        }
        function Validasi2() {

            var valueattempt = '';
            var GetValueRange = lblNmRange.GetText();
            var GetValueMax = parseFloat(GetValueRange.split('-')[1].split(' ')[0]);
            var PersenMax = GetValueMax * 0.01;
            // Start validasi1
            //20%
            var ValueText_VerValue1 = Text_VerValue_2_1.GetText();
            var ValuePersen1 = GetValueMax * (20 / 100);
            //40%
            var ValueText_VerValue2 = Text_VerValue_2_2.GetText();
            var ValuePersen2 = GetValueMax * (40 / 100);
            //60%
            var ValueText_VerValue3 = Text_VerValue_2_3.GetText();
            var ValuePersen3 = GetValueMax * (60 / 100);
            //80%
            var ValueText_VerValue4 = Text_VerValue_2_4.GetText();
            var ValuePersen4 = GetValueMax * (80 / 100);
            //100%
            var ValueText_VerValue5 = Text_VerValue_2_5.GetText();
            var ValuePersen5 = GetValueMax * (100 / 100);
            //End Validasi1
            //validasi 20%  10 < 9
            var GetRangePersen20 = ValuePersen1 - ValueText_VerValue1;
            if (Math.abs(GetRangePersen20) > PersenMax) {
                Text_VerValue_2_1.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_2_1.GetInputElement().style.backgroundColor = "#ebccd1";
                valueattempt += 1;
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
            }
            if (Math.abs(GetRangePersen20) <= PersenMax) {
                Text_VerValue_2_1.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_2_1.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi 40%
            var GetRangePersen40 = ValuePersen2 - ValueText_VerValue2;
            if (Math.abs(GetRangePersen40) > PersenMax) {
                Text_VerValue_2_2.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_2_2.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen40) <= PersenMax) {
                Text_VerValue_2_2.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_2_2.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi 60%
            var GetRangePersen60 = ValuePersen3 - ValueText_VerValue3;
            if (Math.abs(GetRangePersen60) > PersenMax) {
                Text_VerValue_2_3.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_2_3.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen60) <= PersenMax) {
                Text_VerValue_2_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_2_3.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);

            }
            //validasi80%
            var GetRangePersen80 = ValuePersen4 - ValueText_VerValue4;
            if (Math.abs(GetRangePersen80) > PersenMax) {
                Text_VerValue_2_4.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_2_4.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                //valueattempt += 1;
            }
            if (Math.abs(GetRangePersen80) <= PersenMax) {
                Text_VerValue_2_4.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_2_4.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi100%
            var GetRangePersen100 = ValuePersen5 - ValueText_VerValue5;
            if (Math.abs(GetRangePersen100) > PersenMax) {
                Text_VerValue_2_5.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_2_5.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen100) <= PersenMax) {
                Text_VerValue_2_5.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_2_5.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            if (parseFloat(valueattempt) >= PersenMax) {
                //var getvalue = lbl_Attempt.GetText();
                //if (getvalue == "") {
                //    lbl_Attempt.SetText(0);
                //}
                //var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
                //lbl_Attempt.SetText(valueattemptUI);


            }
            //if (parseInt(lbl_Attempt.GetText()) >= 3) {
            //    //Btn_mark.SetEnabled();
            //    Btn_mark.SetEnabled(true);
            //    Btn_Save.SetEnabled(false);
            //}
            if (Text_VerValue_2_1.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_2_2.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_2_3.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_2_4.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_2_5.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)") {
                Lbl_Verify2.SetText("Success");
                Lbl_Verify2.GetMainElement().style.backgroundColor = "rgb(214, 233, 198)";
                Lbl_Verify2.GetMainElement().style.padding = "5px";
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                btn_barcode.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";

            }
            if (Text_VerValue_2_1.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_2_2.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_2_3.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_2_4.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_2_5.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)") {
                Lbl_Verify2.SetText("Failed");
                Lbl_Verify2.GetMainElement().style.backgroundColor = "rgb(235, 204, 209)";
                Lbl_Verify2.GetMainElement().style.padding = "5px";
            }
            //  Btn_mark.SetEnabled(true);
            if (Lbl_Verify2.GetText() == "Failed") {
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                Text_VerValue_3_1.SetClientVisible = true;
                Text_VerValue_3_1.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_3_2.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_3_3.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_3_4.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Text_VerValue_3_5.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                Btn_Verify3.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                Btn_Verify2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                //Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                //Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                //enable
                Text_VerValue_2_1.clientEnabled = false;
                Text_VerValue_2_2.clientEnabled = false;
                Text_VerValue_2_3.clientEnabled = false;
                Text_VerValue_2_4.clientEnabled = false;
                Text_VerValue_2_5.clientEnabled = false;
                var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
                lbl_Attempt.SetText(valueattemptUI);
                //Btn_Verify2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3";               
                //Btn_Verify3.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3Result";
            }
            if (parseInt(lbl_Attempt.GetText()) >= 3) {
                //Btn_mark.SetEnabled();
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
            }
        }
        function Validasi3() {

            var valueattempt = '';
            var GetValueRange = lblNmRange.GetText();
            var GetValueMax = parseFloat(GetValueRange.split('-')[1].split(' ')[0]);
            var PersenMax = GetValueMax * 0.01;
            // Start validasi1
            //20%
            var ValueText_VerValue1 = Text_VerValue_3_1.GetText();
            var ValuePersen1 = GetValueMax * (20 / 100);
            //40%
            var ValueText_VerValue2 = Text_VerValue_3_2.GetText();
            var ValuePersen2 = GetValueMax * (40 / 100);
            //60%
            var ValueText_VerValue3 = Text_VerValue_3_3.GetText();
            var ValuePersen3 = GetValueMax * (60 / 100);
            //80%
            var ValueText_VerValue4 = Text_VerValue_3_4.GetText();
            var ValuePersen4 = GetValueMax * (80 / 100);
            //100%
            var ValueText_VerValue5 = Text_VerValue_3_5.GetText();
            var ValuePersen5 = GetValueMax * (100 / 100);
            //End Validasi1
            //validasi 20%  10 < 9
            var GetRangePersen20 = ValuePersen1 - ValueText_VerValue1;
            if (Math.abs(GetRangePersen20) > PersenMax) {
                Text_VerValue_3_1.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_3_1.GetInputElement().style.backgroundColor = "#ebccd1";
                valueattempt += 1;
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
            }
            if (Math.abs(GetRangePersen20) <= PersenMax) {
                Text_VerValue_3_1.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_3_1.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi 40%
            var GetRangePersen40 = ValuePersen2 - ValueText_VerValue2;
            if (Math.abs(GetRangePersen40) > PersenMax) {
                Text_VerValue_3_2.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_3_2.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen40) <= PersenMax) {
                Text_VerValue_3_2.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_3_2.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi 60%
            var GetRangePersen60 = ValuePersen3 - ValueText_VerValue3;
            if (Math.abs(GetRangePersen60) > PersenMax) {
                Text_VerValue_3_3.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_3_3.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen60) <= PersenMax) {
                Text_VerValue_3_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_3_3.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);

            }
            //validasi80%
            var GetRangePersen80 = ValuePersen4 - ValueText_VerValue4;
            if (Math.abs(GetRangePersen80) > PersenMax) {
                Text_VerValue_3_4.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_3_4.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                //valueattempt += 1;
            }
            if (Math.abs(GetRangePersen80) <= PersenMax) {
                Text_VerValue_3_4.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_3_4.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            //validasi100%
            var GetRangePersen100 = ValuePersen5 - ValueText_VerValue5;
            if (Math.abs(GetRangePersen100) > PersenMax) {
                Text_VerValue_3_5.GetMainElement().style.backgroundColor = "#ebccd1";
                Text_VerValue_3_5.GetInputElement().style.backgroundColor = "#ebccd1";
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                // valueattempt += 1;
            }
            if (Math.abs(GetRangePersen100) <= PersenMax) {
                Text_VerValue_3_5.GetMainElement().style.backgroundColor = "#d6e9c6";
                Text_VerValue_3_5.GetInputElement().style.backgroundColor = "#d6e9c6";
                Btn_mark.SetEnabled(false);
                Btn_Save.SetEnabled(true);
            }
            if (parseFloat(valueattempt) >= PersenMax) {
                var getvalue = lbl_Attempt.GetText();
                if (getvalue == "") {

                }



            }
            //if (parseInt(lbl_Attempt.GetText()) >= 3) {
            //    //Btn_mark.SetEnabled();
            //    Btn_mark.SetEnabled(true);
            //    Btn_Save.SetEnabled(false);
            //}
            if (Text_VerValue_3_1.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_3_2.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_3_3.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_3_4.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && Text_VerValue_3_5.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)") {
                Lbl_Verify3.SetText("Success");
                Lbl_Verify3.GetMainElement().style.backgroundColor = "rgb(214, 233, 198)";
                Lbl_Verify3.GetMainElement().style.padding = "5px";
                Text_VerValue_3_1.clientEnabled = false;
                Text_VerValue_3_2.clientEnabled = false;
                Text_VerValue_3_3.clientEnabled = false;
                Text_VerValue_3_4.clientEnabled = false;
                Text_VerValue_3_5.clientEnabled = false;
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                btn_barcode.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
            }
            if (Text_VerValue_3_1.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_3_2.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_3_3.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_3_4.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || Text_VerValue_3_5.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)") {
                Lbl_Verify3.SetText("Failed");
                Lbl_Verify3.GetMainElement().style.backgroundColor = "rgb(235, 204, 209)";
                Lbl_Verify3.GetMainElement().style.padding = "5px";
            }
            //  Btn_mark.SetEnabled(true);
            if (Lbl_Verify3.GetText() == "Failed") {
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
                //enable
                Text_VerValue_3_1.clientEnabled = false;
                Text_VerValue_3_2.clientEnabled = false;
                Text_VerValue_3_3.clientEnabled = false;
                Text_VerValue_3_4.clientEnabled = false;
                Text_VerValue_3_5.clientEnabled = false;
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Verify3.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                DateDetails.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                DateDetailsCalender.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                ClickDate.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
            }
            if (parseInt(lbl_Attempt.GetText()) >= 3) {
                //Btn_mark.SetEnabled();
                Btn_mark.SetEnabled(true);
                Btn_Save.SetEnabled(false);
            }
            var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
            lbl_Attempt.SetText(valueattemptUI);
        }
    </script>
    <h2>Tool Calibration</h2>
    <asp:HiddenField runat="server" ID="HiddenCheckValue" />
    <dx:ASPxFormLayout ID="LayOutAssign" runat="server" Style="margin-top: 0px; width: 100%">

        <Items>

            <dx:LayoutGroup Caption="" ColCount="2">
                <Items>
                    <dx:LayoutItem Caption="Tool Number">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboBox_ToolNumber" runat="server" AutoPostBack="True" Height="25px" OnSelectedIndexChanged="SelectedChangedToolNumber">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <%-- <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxDateEdit runat="server" ID="dtPackingMonth" DisplayFormatString="yyyyMM" ClientVisible="false"></dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                    <dx:LayoutItem Caption="Inventory Number">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboBox_InventoryNumber" runat="server">
                                </dx:ASPxComboBox>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Model">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="Cmb_Model" runat="server" AutoPostBack="True" TextField="ModelName" ValueField="Id" Height="25px" DataSourceID="ModelsSource">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Production Line">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboBox_ProductionLine" runat="server" DataSourceID="DataProductionLine" TextField="LineName" ValueField="Id">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Station">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxComboBox ID="ComboBox_Station" runat="server">
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxButton ID="Btn_ShowGv" runat="server" ClientInstanceName="Btn_ShowGv" OnClick="ShowGv" Text="Show">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="" ColCount="2">
                <Items>
                    <dx:LayoutItem Caption="" ColSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">

                                <dx:ASPxGridView ID="GridviewToolVerification" OnHtmlRowPrepared="GridviewToolVerification_HtmlDataRowPrepared"
                                    runat="server" KeyFieldName="Id" AutoGenerateColumns="False" OnDetailRowExpandedChanged="DetailChangedTes"
                                    ClientInstanceName="GridviewToolVerification" OnDataBinding="GridviewToolVerification_DataBinding" Width="100%">
                                    <ClientSideEvents DetailRowCollapsing="CollapsingDetailRow" DetailRowExpanding="ExpandingDetailRow" />
                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="2">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="ToolId" ReadOnly="True" VisibleIndex="-1" Visible="false">
                                            <EditFormSettings Visible="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ResultCalibration" ReadOnly="True" VisibleIndex="0" Visible="false">
                                            <EditFormSettings Visible="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="NextDay" ReadOnly="True" VisibleIndex="1" Visible="false">
                                            <EditFormSettings Visible="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="3" Visible="false">
                                            <EditFormSettings Visible="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ToolNumber" ReadOnly="True" VisibleIndex="4">
                                            <EditFormSettings Visible="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="5" Visible="true">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="SetNM" ReadOnly="True" VisibleIndex="6">
                                            <EditFormSettings Visible="true" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="MaxNM" ReadOnly="True" VisibleIndex="8">
                                            <EditFormSettings Visible="true" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="MinNM" ReadOnly="True" VisibleIndex="7">
                                            <EditFormSettings Visible="true" />
                                        </dx:GridViewDataTextColumn>
                                        <%-- <dx:GridViewDataCheckColumn FieldName="Status" VisibleIndex="5">
            </dx:GridViewDataCheckColumn>
                                        --%>
                                        <dx:GridViewDataTextColumn FieldName="LastCalibrationDate" VisibleIndex="9" Caption="Last Calibration">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="NextCalibrationDate" VisibleIndex="10" Caption="Next Calibration">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ClassName" VisibleIndex="11" Name="Class" Caption="Class">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="StationName" VisibleIndex="12" Name="Class" Caption="Station">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowGroupPanel="True" />
                                    <SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
                                    <SettingsSearchPanel Visible="True" />
                                    <Templates>
                                        <DetailRow>
                                            <dx:ASPxFormLayout ID="LayoutDetails" runat="server" ClientInstanceName="LayoutDetails" ClientIDMode="Static" ViewStateMode="Enabled">
                                                <Items>
                                                    <dx:LayoutItem Caption="Last Calibration Date">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxLabel ID="lblCalibrationDate" runat="server" ClientInstanceName="lblCalibrationDate">
                                                                </dx:ASPxLabel>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <%-- <dx:LayoutItem Caption="Last Verification Date">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="lblVerificationDate" runat="server" ClientInstanceName="lblVerificationDate" ViewStateMode="Enabled">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                                                    <%-- <dx:LayoutItem Caption="Tool Number">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxLabel ID="lblToolNumber" runat="server" ClientInstanceName="lblToolNumber" ViewStateMode="Enabled">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                                                    <dx:LayoutItem Caption="Inventory Number">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxLabel ID="lblInventoryNumber" runat="server" ClientInstanceName="lblInventoryNumber">
                                                                </dx:ASPxLabel>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutItem Caption="Nm Range">
                                                        <LayoutItemNestedControlCollection>
                                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                                <dx:ASPxLabel ID="lblNmRange" runat="server" ClientInstanceName="lblNmRange">
                                                                </dx:ASPxLabel>
                                                            </dx:LayoutItemNestedControlContainer>
                                                        </LayoutItemNestedControlCollection>
                                                    </dx:LayoutItem>
                                                    <dx:LayoutGroup Caption="" ColCount="9">
                                                        <Items>
                                                            <dx:LayoutItem Caption="">
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="lbl20" runat="server">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="lbl40" runat="server">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="lbl60" runat="server">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="lbl80" runat="server">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="lbl100" runat="server">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="" ColSpan="3">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="LayoutDetails_E7" runat="server" Text="">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="Calibration 1">
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_1_1" runat="server" ClientInstanceName="Text_VerValue_1_1" Width="60px">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_1_2" runat="server" ClientInstanceName="Text_VerValue_1_2" Width="60px">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_1_3" runat="server" ClientInstanceName="Text_VerValue_1_3" Width="60px">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_1_4" runat="server" ClientInstanceName="Text_VerValue_1_4" Width="60px">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_1_5" runat="server" ClientInstanceName="Text_VerValue_1_5" Width="60px">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="" ColSpan="2">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxButton ID="Btn_Verify" runat="server" AutoPostBack="False" Text="Verify">
                                                                            <ClientSideEvents Click="Validasi1" />
                                                                        </dx:ASPxButton>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="Lbl_Verify" runat="server" ClientInstanceName="Lbl_Verify">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="Calibration 2">
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_2_1" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_2_2" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_2_3" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_2_4" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_2_5" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="" ColSpan="2">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxButton ID="Btn_Verify2" runat="server" Text="Verify" AutoPostBack="false" CssClass="Verification2">
                                                                            <ClientSideEvents Click="Validasi2" />
                                                                        </dx:ASPxButton>

                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="Lbl_Verify2" runat="server" ClientInstanceName="Lbl_Verify2">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="Calibration 3">
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_3_1" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_3_2" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_3_3" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_3_4" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="Text_VerValue_3_5" runat="server" Width="60px" CssClass="Verification2">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="" ColSpan="2">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxButton ID="Btn_Verify3" runat="server" Text="Verify" AutoPostBack="false" CssClass="Verification2">
                                                                            <ClientSideEvents Click="Validasi3" />
                                                                        </dx:ASPxButton>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxLabel ID="Lbl_Verify3" runat="server" ClientInstanceName="Lbl_Verify3">
                                                                        </dx:ASPxLabel>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <%-- <dx:ASPxTextBox ID="lbl_Attempt" runat="server" ClientInstanceName="lbl_Attempt" ClientVisible="false">
                                            </dx:ASPxTextBox>--%>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                        </Items>
                                                    </dx:LayoutGroup>
                                                    <dx:LayoutGroup Caption="" ColCount="2" RowSpan="2">
                                                        <CellStyle>
                                                            <Border BorderColor="White" BorderStyle="None" />
                                                        </CellStyle>
                                                        <Items>
                                                            <dx:LayoutItem Caption="Remarks" Name="Memo_Remarks" ColSpan="2">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxMemo ID="RemarksDetail" runat="server" Height="71px" Width="628px">
                                                                        </dx:ASPxMemo>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="Plan Next Calibration (Day)" Name="DateNextCalibration">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="DateDetails" runat="server" Text="0" ClientInstanceName="DateDetails">
                                                                        </dx:ASPxTextBox>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxButton ID="ClickDate" ClientInstanceName="ClickDate" Text="Calculate Date" runat="server" Style="float: right" AutoPostBack="false">
                                                                            <ClientSideEvents Click="PickDate" />
                                                                        </dx:ASPxButton>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>

                                                            <dx:LayoutItem Caption="Date" Name="DateNextCalibration">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxTextBox ID="DateDetailsCalender" runat="server" ClientInstanceName="DateDetailsCalender"></dx:ASPxTextBox>
                                                                        <clientsideevents keyup="PickDate" />
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxButton ID="Btn_mark" runat="server" Style="float: right" Text="Mark As Scrap" OnClick="MarkAsCrapSave" ClientInstanceName="Btn_mark" CssClass="Verification2" AutoPostBack="false">
                                                                        </dx:ASPxButton>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                            <dx:LayoutItem Caption="">
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                                        <dx:ASPxButton ID="BtnSave" AutoPostBack="false" runat="server" Style="float: right" Text="Save" OnClick="SaveCalibration" ClientInstanceName="Btn_Save" CssClass="Verification2">
                                                                        </dx:ASPxButton>
                                                                        <dx:ASPxButton ID="btnToolBarcode" runat="server" Text="Print Barcode" ClientInstanceName="btn_barcode" OnClick="btnToolBarcode_Click" CssClass="Verification2" AutoPostBack="false"></dx:ASPxButton>
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                            </dx:LayoutItem>
                                                        </Items>
                                                    </dx:LayoutGroup>
                                                    <%--<dx:LayoutGroup Border-BorderStyle="None" Border-BorderWidth="0px" BorderLeft-BorderStyle="None" Caption="" ColCount="2">
                            <BorderLeft BorderStyle="None" />
                            <Border BorderStyle="None" BorderWidth="0px" />
                            <Items>
                                <dx:LayoutItem Caption="">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server">
                                            <dx:ASPxButton ID="Btn_mark" runat="server" Text="Mark As Scrap" OnClick="MarkAsCrapSave" ClientInstanceName="Btn_mark" CssClass="Verification2" AutoPostBack="false">
                                            </dx:ASPxButton>    
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="" Width="10px">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" style="width:10px;">
                                            <dx:ASPxButton ID="BtnSave" AutoPostBack="false"  runat="server" style="width:10px;float:right" Text="Save" OnClick="SaveCalibration" ClientInstanceName="Btn_Save" CssClass="Verification2">
                                            </dx:ASPxButton>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>--%>
                                                </Items>
                                            </dx:ASPxFormLayout>
                                        </DetailRow>
                                    </Templates>

                                </dx:ASPxGridView>
                                <dx:ASPxButton ID="btnExportExcel" runat="server" Text="Export Excel" OnClick="ExportExcel_Click"></dx:ASPxButton>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
    </dx:ASPxFormLayout>

    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="GridviewToolVerification" FileName="ToolCalibration"></dx:ASPxGridViewExporter>

    <dx:ASPxTextBox ID="lbl_Attempt" runat="server" ClientInstanceName="lbl_Attempt" Text="0" ClientVisible="false">
    </dx:ASPxTextBox>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommand="select b.Id,convert(varchar(25),tl.Number) + ' - ' + convert(varchar(25),b.InventoryNumber) ToolNumber,st.StationName,tl.MaxNM,tl.MinNM,tl.Number,tl.Calibrated, f.Class ClassName,max(d.NextCalibrationDate) NextCalibrationDate,tl.Description,b.ToolSetupId,b.Status,b.SetNm SetNM from 
	(select distinct a.Id,a.Number,a.Calibrated,a.MaxNM,a.MinNM,a.Description from tools a) tl left join ToolInventories b on tl.Id = b.ToolSetupId
	inner join (select distinct ToolInventoryId,StationId from ToolAssignmentStation) c on c.ToolInventoryId= b.Id
	left join ToolCalibrations d on d.ToolInvId = b.Id
	
	left join ControlPlanTools e on e.ToolId = tl.id
	left join ControlPlanProcesses f on f.id = e.ControlPlanProcessId
	left join ToolClass g on g.ClassName = f.Class
	left join Stations st on st.Id =c.StationId 
	where tl.Calibrated = 1 and b.InventoryNumber IS NOT NULL or d.NextCalibrationDate is null or d.NextCalibrationDate BETWEEN (CONVERT(char(10), GetDate() -1,126)) AND (CONVERT(char(10), GetDate(),126))
	group by b.InventoryNumber,tl.Number,b.Id,f.Class,g.ClassName,tl.Calibrated,st.StationName,tl.MaxNM,tl.MinNM,tl.Description,b.ToolSetupId,b.Status,b.SetNm
"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ModelsSource" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommand="select ModelName,Id from Models ORDER BY ModelName"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DataProductionLine" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommand="select * from ProductionLines order by LineName"></asp:SqlDataSource>
</asp:Content>
