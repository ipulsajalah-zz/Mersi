<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ToolVerification.aspx.cs" Inherits="DotMercy.custom.ToolManagement.ToolVerification" %>

<%--Copy Dari Sini--%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Tool Verification

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
        function ClickMarkValidasi() {
            var GetValueNM = Text_NM.GetText();
            var GetValueVer1 = text_ver1_1.GetText();
            var GetValueVer2 = text_ver1_2.GetText();
            var GetValueVer3 = text_ver1_3.GetText();
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
            var GetValueVer1 = text_ver1_1.GetText();
            var GetValueVer2 = text_ver1_2.GetText();
            var GetValueVer3 = text_ver1_3.GetText();
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
            if (text_ver1_1.GetMainElement().style.backgroundColor == "#ebccd1") {
                alert('Verification One Failed Please Verify again')
                return false;
            }
            if (text_ver1_2.GetMainElement().style.backgroundColor == "#ebccd1") {
                alert('Verification Two Failed Please Verify again')
                return false;
            }
            if (text_ver1_3.GetMainElement().style.backgroundColor == "#ebccd1") {
                alert('Verification three Failed Please Verify again')
                return false;
            }
        }
        function CollapsingDetailRow() {
            //  Btn_mark.SetEnabled(true);
        }
        function ExpandingDetailRow() {
            //   Btn_mark.SetEnabled(true);
            //text_ver2_1.SetVisible = false;
            //text_ver2_2.SetVisible = false;
            //text_ver2_3.SetVisible = false;

        }
        function functionMaxMin() {
            Btn_mark.SetEnabled(true);
            var GetValueNM = Text_NM.GetText();
            if (GetValueNM < 10) {
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
        function Validasi() {
            text_ver1_1.SetEnabled = true;
            Btn_mark.SetEnabled(true);
            var valueattempt = '';
            var GetValueMax = Text_Max.GetText().replace(',', '.');
            var GetValueMin = Text_Min.GetText().replace(',', '.');
            var Valuetext_ver1_1 = text_ver1_1.GetText().replace(',', '.');
            var Valuetext_ver1_2 = text_ver1_2.GetText().replace(',', '.');
            var Valuetext_ver1_3 = text_ver1_3.GetText().replace(',', '.');
            if (parseFloat(Valuetext_ver1_1) <= parseFloat(GetValueMin)) {
                text_ver1_1.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver1_1.GetInputElement().style.backgroundColor = "#ebccd1";
                // valueattempt += 1;
                //Btn_mark.SetEnabled = true;

            }
            if (parseFloat(Valuetext_ver1_1) >= parseFloat(GetValueMax)) {
                text_ver1_1.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver1_1.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled = true;
                ResultVerifyPart1.SetText("FAILED");
            }
            if (parseFloat(Valuetext_ver1_2) <= parseFloat(GetValueMin)) {
                text_ver1_2.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver1_2.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled();

            }
            if (parseFloat(Valuetext_ver1_2) >= parseFloat(GetValueMax)) {
                text_ver1_2.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver1_2.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled();
            }
            if (parseFloat(Valuetext_ver1_3) <= parseFloat(GetValueMin)) {
                text_ver1_3.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver1_3.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled();

            }
            if (parseFloat(Valuetext_ver1_3) >= parseFloat(GetValueMax)) {
                text_ver1_3.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver1_3.GetInputElement().style.backgroundColor = "#ebccd1";
                // valueattempt += 1;


            }
            if (parseFloat(Valuetext_ver1_1) >= parseFloat(GetValueMin) && parseFloat(Valuetext_ver1_1) <= parseFloat(GetValueMax)) {
                text_ver1_1.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver1_1.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            if (parseFloat(Valuetext_ver1_2) >= parseFloat(GetValueMin) && parseFloat(Valuetext_ver1_2) <= parseFloat(GetValueMax)) {
                text_ver1_2.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver1_2.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            if (parseFloat(Valuetext_ver1_3) >= parseFloat(GetValueMin) && parseFloat(Valuetext_ver1_3) <= parseFloat(GetValueMax)) {
                text_ver1_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver1_3.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            //if (valueattempt >= 1) {
            //    var getvalue = lbl_Attempt.GetText();
            //    if (getvalue == "") {
            //        lbl_Attempt.SetText(0);
            //    }
            //    var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
            //    lbl_Attempt.SetText(valueattemptUI);

            //}
            //if (parseInt(lbl_Attempt.GetText()) >= 3) {
            //    //Btn_mark.SetEnabled();
            //    Btn_mark.SetEnabled(true);
            //    Btn_Save.SetEnabled();
            //}
            if (text_ver1_1.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && text_ver1_2.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && text_ver1_3.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)") {
                ResultVerifyPart1.SetText("Success");
                ResultVerifyPart1.GetMainElement().style.backgroundColor = "#d6e9c6";
                ResultVerifyPart1.GetMainElement().style.padding = "5px";
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                VerifyPart1.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
            }
            if (text_ver1_1.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || text_ver1_2.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || text_ver1_3.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)") {
                ResultVerifyPart1.SetText("Failed");
                ResultVerifyPart1.GetMainElement().style.backgroundColor = "#ebccd1";
                ResultVerifyPart1.GetMainElement().style.padding = "5px";
                text_ver2_1.SetClientVisible = true;
                text_ver2_2.SetClientVisible = true;
                text_ver2_3.SetClientVisible = true;
                text_ver2_1.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                text_ver2_2.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                text_ver2_3.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification2Result";
                text_ver1_1.clientEnabled = false;
                text_ver1_2.clientEnabled = false;
                text_ver1_3.clientEnabled = false;
                VerifyPart1.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                document.getElementById("text_ver1_2").disabled = true;
                Btn_VerificationPart2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                lbl_Verify2_result.GetMainElement().className = "dxeBase_DevEx Verification2Result";
                lbl_Attempt.SetText(1);
                //Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                //Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
            }
        }
        function Validasi2() {
            Btn_mark.SetEnabled(true);
            var valueattempt = '';
            var GetValueMax = Text_Max.GetText().replace(',', '.');
            var GetValueMin = Text_Min.GetText().replace(',', '.');
            var text_ver2_1val = text_ver2_1.GetText().replace(',', '.');
            var text_ver2_2val = text_ver2_2.GetText().replace(',', '.');
            var text_ver2_3val = text_ver2_3.GetText().replace(',', '.');
            if (parseFloat(text_ver2_1val) <= parseFloat(GetValueMin)) {
                text_ver2_1.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver2_1.GetInputElement().style.backgroundColor = "#ebccd1";
                // valueattempt += 1;
                //Btn_mark.SetEnabled();

            }
            if (parseFloat(text_ver2_1val) >= parseFloat(GetValueMax)) {
                text_ver2_1.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver2_1.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled();
                //Lbl_Verify.SetText("FAILED");
            }
            if (parseFloat(text_ver2_2val) <= parseFloat(GetValueMin)) {
                text_ver2_2.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver2_2.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled();

            }
            if (parseFloat(text_ver2_2val) >= parseFloat(GetValueMax)) {
                text_ver2_2.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver2_2.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //Btn_mark.SetEnabled();
            }
            if (parseFloat(text_ver2_3val) <= parseFloat(GetValueMin)) {
                text_ver2_3.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver2_3.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //  Btn_mark.SetEnabled();

            }
            if (parseFloat(text_ver2_3val) >= parseFloat(GetValueMax)) {
                text_ver2_3.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver2_3.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;


            }
            if (parseFloat(text_ver2_1val) >= parseFloat(GetValueMin) && parseFloat(text_ver2_1val) <= parseFloat(GetValueMax)) {
                text_ver2_1.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver2_1.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            if (parseFloat(text_ver2_2val) >= parseFloat(GetValueMin) && parseFloat(text_ver2_2val) <= parseFloat(GetValueMax)) {
                text_ver2_2.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver2_2.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            if (parseFloat(text_ver2_3val) >= parseFloat(GetValueMin) && parseFloat(text_ver2_3val) <= parseFloat(GetValueMax)) {
                text_ver2_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver2_3.GetInputElement().style.backgroundColor = "#d6e9c6";

            }

            if (text_ver2_1.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && text_ver2_2.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && text_ver2_3.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)") {
                lbl_Verify2_result.SetText("Success");
                lbl_Verify2_result.GetMainElement().style.backgroundColor = "#d6e9c6";
                lbl_Verify2_result.GetMainElement().style.padding = "5px";
                Btn_mark.SetClientVisible = false;
                Btn_Save.SetClientVisible = true;
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
            }
            if (text_ver2_1.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || text_ver2_2.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || text_ver2_3.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)") {
                lbl_Verify2_result.SetText("Failed");
                lbl_Verify2_result.GetMainElement().style.backgroundColor = "#ebccd1";
                lbl_Verify2_result.GetMainElement().style.padding = "5px";
                text_ver3_1.GetMainElement().className = "Verification2Result"
                text_ver3_1.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification3Result";
                text_ver3_2.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification3Result";
                text_ver3_3.GetMainElement().className = "dxeTextBoxSys dxeTextBox_DevEx Verification3Result";
                text_ver2_1.clientEnabled = false;
                text_ver2_2.clientEnabled = false;
                text_ver2_3.clientEnabled = false;
                Btn_VerificationPart2.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Verify3.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3Result";
                lbl_verify_result_3.GetMainElement().className = "dxeBase_DevEx Verification3Result";
                var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
                lbl_Attempt.SetText(valueattemptUI);
                //Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                //Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
            }
        }

        function Validasi3() {
            Btn_mark.SetEnabled(true);
            var valueattempt = '';
            var GetValueMax = Text_Max.GetText().replace(',', '.');;
            var GetValueMin = Text_Min.GetText().replace(',', '.');;
            var text_ver3_1val = text_ver3_1.GetText().replace(',', '.');;
            var text_ver3_2val = text_ver3_2.GetText().replace(',', '.');;
            var text_ver3_3val = text_ver3_3.GetText().replace(',', '.');;
            if (parseFloat(text_ver3_1val) < parseFloat(GetValueMin)) {
                text_ver3_1.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver3_1.GetInputElement().style.backgroundColor = "#ebccd1";
                // valueattempt += 1;
                // Btn_mark.SetEnabled();

            }
            if (parseFloat(text_ver3_1val) >= parseFloat(GetValueMax)) {
                text_ver3_1.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver3_1.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                // Btn_mark.SetEnabled();
                //  Lbl_Verify.SetText("FAILED");
            }
            if (parseFloat(text_ver3_2val) <= parseFloat(GetValueMin)) {
                text_ver3_2.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver3_2.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                //  Btn_mark.SetEnabled();

            }
            if (parseFloat(text_ver3_2val) >= parseFloat(GetValueMax)) {
                text_ver3_2.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver3_2.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                // Btn_mark.SetEnabled();
            }
            if (parseFloat(text_ver3_3val) <= parseFloat(GetValueMin)) {
                text_ver3_3.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver3_3.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;
                // Btn_mark.SetEnabled();

            }
            if (parseFloat(text_ver3_3val) >= parseFloat(GetValueMax)) {
                text_ver3_3.GetMainElement().style.backgroundColor = "#ebccd1";
                text_ver3_3.GetInputElement().style.backgroundColor = "#ebccd1";
                //valueattempt += 1;


            }
            if (parseFloat(text_ver3_1val) >= parseFloat(GetValueMin) && parseFloat(text_ver3_1val) <= parseFloat(GetValueMax)) {
                text_ver3_1.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver3_1.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            if (parseFloat(text_ver3_2val) >= parseFloat(GetValueMin) && parseFloat(text_ver3_2val) <= parseFloat(GetValueMax)) {
                text_ver3_2.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver3_2.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            if (parseFloat(text_ver3_3val) >= parseFloat(GetValueMin) && parseFloat(text_ver3_3val) <= parseFloat(GetValueMax)) {
                text_ver3_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                text_ver3_3.GetInputElement().style.backgroundColor = "#d6e9c6";

            }
            //if (valueattempt >= 1) {
            //    var getvalue = lbl_Attempt.GetText();
            //    if (getvalue == "") {
            //        lbl_Attempt.SetText(0);
            //    }
            //    var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
            //    lbl_Attempt.SetText(valueattemptUI);

            //}
            //if (parseInt(lbl_Attempt.GetText()) >= 3) {
            //    //Btn_mark.SetEnabled();
            //    Btn_mark.SetEnabled(true);
            //    Btn_Save.SetEnabled();
            //}
            if (text_ver3_1.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && text_ver3_2.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)" && text_ver3_3.GetMainElement().style.backgroundColor == "rgb(214, 233, 198)") {
                lbl_verify_result_3.SetText("Success");
                lbl_verify_result_3.GetMainElement().style.backgroundColor = "#d6e9c6";
                lbl_verify_result_3.GetMainElement().style.padding = "5px";
                Btn_mark.SetClientVisible = false;
                Btn_Save.SetClientVisible = true;
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
            }
            if (text_ver3_1.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || text_ver3_2.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)" || text_ver3_3.GetMainElement().style.backgroundColor == "rgb(235, 204, 209)") {
                lbl_verify_result_3.SetText("Failed");
                lbl_verify_result_3.GetMainElement().style.backgroundColor = "#ebccd1";
                lbl_verify_result_3.GetMainElement().style.padding = "5px";
                var valueattemptUI = parseInt(lbl_Attempt.GetText()) + 1;
                lbl_Attempt.SetText(valueattemptUI);
                text_ver3_1.clientEnabled = false;
                text_ver3_2.clientEnabled = false;
                text_ver3_3.clientEnabled = false;
                Btn_Verify3.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification3";
                Btn_mark.SetClientVisible = true;
                Btn_Save.SetClientVisible = false;
                Btn_mark.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2Result";
                Btn_Save.GetMainElement().className = "dxbButton_DevEx dxbButtonSys dxbTSys dxbButtonHover_DevEx Verification2";
            }
        }
    </script>
    <style>
    </style>
    <div class="content">
        <h2>Tool Verification</h2>
        <dx:ASPxHiddenField ID="Gv_second" runat="server"></dx:ASPxHiddenField>
        <asp:HiddenField ID="hiddenvalue" runat="server" EnableViewState="true" />
        <asp:HiddenField ID="hiddenSetNm" runat="server" />
        <asp:HiddenField ID="HiddenMin" runat="server" />
        <asp:HiddenField ID="HiddenMax" runat="server" />
        <dx:ASPxFormLayout ID="LayOutAssign" runat="server" Style="margin-top: 0px; width: 100%">

            <Items>

                <dx:LayoutGroup Caption="">
                    <Items>
                        <dx:LayoutItem Caption="Tool Number">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ComboBox_ToolNumber" runat="server" AutoPostBack="True" Height="25px" OnSelectedIndexChanged="SelectedChangedToolNumber">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <%--<dx:LayoutItem Caption="">
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
                        <%-- <dx:LayoutItem Caption="Model">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ComboBox_Model" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectedChangedModelVariant">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
                        <dx:LayoutItem Caption="Production Line">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="ComboBox_ProductionLine" runat="server">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <%--<dx:LayoutItem Caption="Model">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="Cmb_Model" runat="server" AutoPostBack="True" TextField="ModelName" ValueField="Id" Height="25px" DataSourceID="ModelsSource">
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>--%>
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
                                   
                                  <dx:ASPxGridView ID="GridviewToolVerification" runat="server" KeyFieldName="Id" AutoGenerateColumns="False" 
            OnDetailRowExpandedChanged="GridviewToolVerification_DetailChanged" ClientInstanceName="GridviewToolVerification" 
            OnDataBinding="GridviewToolVerification_DataBinding" Width="100%" OnHtmlRowPrepared="GridviewToolVerification_HtmlDataRowPrepared">
            <ClientSideEvents DetailRowCollapsing="CollapsingDetailRow" DetailRowExpanding="ExpandingDetailRow"/>
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="1">
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="ToolId" ReadOnly="True" VisibleIndex="-1" Visible="false">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="VerDay" ReadOnly="True" VisibleIndex="0" Visible="false">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="2" Visible="false">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ToolNumber" ReadOnly="True" VisibleIndex="3">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="4" Visible="true">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SetNM" ReadOnly="True" VisibleIndex="5">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                 <dx:GridViewDataTextColumn FieldName="MaxNM" ReadOnly="True" VisibleIndex="7">
                <EditFormSettings Visible="true" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MinNM" ReadOnly="True" VisibleIndex="6">
                <EditFormSettings Visible="true" />
            </dx:GridViewDataTextColumn>
                <%-- <dx:GridViewDataCheckColumn FieldName="Status" VisibleIndex="5">
            </dx:GridViewDataCheckColumn>--%>
                 <dx:GridViewDataTextColumn FieldName="LastVerificationDate" VisibleIndex="9" Name="Last Verification Date" Caption="Last Verification">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="NextVerificationDate" VisibleIndex="10" Name="Next Verification Date" Caption="Next Verification">
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
                            <dx:LayoutItem Caption="ToolNumber">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxLabel ID="lblInventoryNumber" runat="server" ClientInstanceName="lblInventoryNumber">
                                        </dx:ASPxLabel>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Last Verification Date">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxLabel ID="lblVerificationDate" runat="server" ClientInstanceName="lblVerificationDate" ViewStateMode="Enabled">
                                        </dx:ASPxLabel>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="SetNM">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxLabel ID="SetNM" runat="server" ClientInstanceName="SetNM">
                                        </dx:ASPxLabel>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutGroup Caption="" ColCount="5">
                                <Items>
                                    <dx:LayoutItem Caption="NM">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="Text_NM" CssClass="TextNmCSS" runat="server" ClientInstanceName="Text_NM" Width="170px">
                                                    <ClientSideEvents KeyUp="functionMaxMin" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Min">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="Text_Min" runat="server" Width="170px" ClientInstanceName="Text_Min">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Max" ColSpan="3">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="Text_Max" runat="server" Width="170px" ClientInstanceName="Text_Max">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Verification 1">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver1_1" runat="server" Width="170px" ClientInstanceName="text_ver1_1">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver1_2" runat="server" ClientInstanceName="text_ver1_2" Width="170px">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver1_3" runat="server" ClientInstanceName="text_ver1_3" Width="170px">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxButton ID="Btn_Verify" runat="server" AutoPostBack="False" ClientInstanceName="VerifyPart1" Text="Verify">
                                                    <ClientSideEvents Click="Validasi" />
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxLabel ID="ResultVerifyPart1" runat="server" ClientInstanceName="ResultVerifyPart1">
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Verification 2">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver2_1" runat="server" ClientInstanceName="text_ver2_1" Width="170px" CssClass="Verification2">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver2_2" runat="server" ClientInstanceName="text_ver2_2" Width="170px" CssClass="Verification2">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver2_3" runat="server" ClientInstanceName="text_ver2_3" Width="170px" CssClass="Verification2">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxButton CssClass="Verification2" ID="Btn_VerificationPart2" runat="server" ClientInstanceName="Btn_VerificationPart2" Text="Verify" AutoPostBack="False">
                                                    <ClientSideEvents Click="Validasi2" />
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxLabel ID="lbl_Verify2_result" runat="server" ClientInstanceName="lbl_Verify2_result" CssClass="Verification2">
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Verification 3">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver3_1" runat="server" CssClass="Verification3" ClientInstanceName="text_ver3_1" Width="170px">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver3_2" runat="server" ClientInstanceName="text_ver3_2" Width="170px" CssClass="Verification3">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="text_ver3_3" runat="server" ClientInstanceName="text_ver3_3" Width="170px" CssClass="Verification3">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxButton ID="Btn_Verify3" runat="server" Text="Verify" AutoPostBack="false" CssClass="Verification3">
                                                    <ClientSideEvents Click="Validasi3" />
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxLabel ID="lbl_verify_result_3" CssClass="Verification3" runat="server" ClientInstanceName="lbl_verify_result_3">
                                                </dx:ASPxLabel>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="" ClientVisible="False">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxTextBox ID="lbl_Attempt" runat="server" ClientInstanceName="lbl_Attempt" Text="ASPxLabel">
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            <dx:LayoutGroup Border-BorderStyle="None" Border-BorderWidth="0px" BorderLeft-BorderStyle="None" Caption="" ColCount="2">
                                <BorderLeft BorderStyle="None" />
                                <Border BorderStyle="None" BorderWidth="0px" />
                                <Items>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">
                                                <dx:ASPxButton ID="Btn_mark" runat="server" Style="float: right;" Text="Request Calibration" ClientInstanceName="Btn_mark" OnClick="Btn_mark_Click" CssClass="Verification2">
                                                    <ClientSideEvents Click="ClickMarkValidasi" />
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="" Width="10px">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server" Style="width: 10px;">
                                                <dx:ASPxButton ID="BtnSave" ClientInstanceName="Btn_Save" AutoPostBack="false"
                                                    runat="server" Style="width: 10px; float: right" Text="Save" OnClick="Btn_Save" CssClass="Verification2">
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
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

        
     
   <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="GridviewToolVerification" FileName="ToolVerification"></dx:ASPxGridViewExporter>
 
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommand="
	select b.Id,convert(varchar(25),tl.Number) + ' - ' + convert(varchar(25),b.InventoryNumber) ToolNumber,st.StationName,tl.MaxNM,tl.MinNM,tl.Number,tl.Calibrated,g.ValDay,g.ClassName,max(d.NextVerificationDate) NextVerificationDate,tl.Description,b.ToolSetupId,b.Status,b.SetNm SetNM from 
	(select distinct a.Id,a.Number,a.Calibrated,a.MaxNM,a.MinNM,a.Description from tools a) tl left join ToolInventories b on tl.Id = b.ToolSetupId
	inner join (select distinct ToolInventoryId,StationId from ToolAssignmentStation) c on c.ToolInventoryId= b.Id
	left join ToolVerifications d on d.ToolSetupInv = b.Id

	left join ControlPlanTools e on e.ToolId = tl.id
	left join ControlPlanProcesses f on f.id = e.ControlPlanProcessId
		left join ToolClass g on g.ClassName = f.Class
	left join Stations st on st.Id =c.StationId 
	where tl.Calibrated = 1 and b.InventoryNumber IS NOT NULL or d.NextVerificationDate is null or d.NextVerificationDate BETWEEN (CONVERT(char(10), GetDate() -1,126)) AND (CONVERT(char(10), GetDate(),126))
	group by b.InventoryNumber,tl.Number,b.Id, g.ClassName,g.ClassName,tl.Calibrated,st.StationName,tl.MaxNM,tl.MinNM,tl.Description,b.ToolSetupId,b.Status,b.SetNm,g.ValDay
"></asp:SqlDataSource>
    <!--select DISTINCT  c.NextCalibrationDate,c.CalibratedDate,b.SetNm,a.Number,b.status Status,a.Id,a.Description,b.InventoryNumber,a.Number from tools a 
left join ToolInventories b on a.Id = b.ToolSetupId
left join ToolCalibrations c on c.ToolSetupId = a.Id
Where b.InventoryNumber !='' and c.NextCalibrationDate In(select TOP 1 a.NextCalibrationDate from ToolCalibrations a order by a.CalibratedDate)-->
    <asp:SqlDataSource ID="ModelsSource" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommand="select ModelName,Id from Models ORDER BY ModelName"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DataProductionLine" runat="server" ConnectionString="<%$ ConnectionStrings:AppDb %>" SelectCommand="select * from ProductionLines order by LineName"></asp:SqlDataSource>


</asp:Content>
