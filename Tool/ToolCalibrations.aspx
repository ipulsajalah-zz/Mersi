<%@ Page Title="" UnobtrusiveValidationMode="None" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ToolCalibrations.aspx.cs" Inherits="Ebiz.WebForm.custom.Tool.ToolCalibrations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Expires" content="0" />

        <script type="text/javascript">

            function onPopupShown(s, e) {
                var windowInnerWidth = window.innerWidth - 40;
                if (s.GetWidth() != windowInnerWidth) {
                    s.SetWidth(windowInnerWidth);
                    s.UpdatePosition();
                }

                var windowInnerHeight = window.innerHeight - 200;
                if (s.GetHeight() != windowInnerHeight) {
                    s.SetHeight(windowInnerHeight);
                    s.UpdatePosition();
                }

            }

            function showHistory(s, e) {
                s.GetRowValues(e, 'Id;Number', OnGetRowValues);
            }

            function OnGetRowValues(values) {
                popupHistory.SetHeaderText('Calibration History - Last 10 (' + values[1] + ')');
                popupHistory.PerformCallback(values[1] + ';' + values[0]);
                popupHistory.Show();
            }

        </script>

    </head>
    <body>
        <h2 class='grid-header'>Tool Calibrations (TODAY)</h2>

        <dx:ASPxPopupControl ID="popupHistory" ClientInstanceName="popupHistory" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            MinWidth="100" MinHeight="100"
            OnWindowCallback="popupHistory_WindowCallback" OnPopupWindowCommand="popupHistory_PopupWindowCommand" 
            HeaderText="Calibration History">
            <ContentStyle Paddings-Padding="0" />
<%--            <ClientSideEvents Shown="onPopupShown"/>--%>

<%--            <HeaderTemplate>
                <div class="head">
                    Calibration History <br /> 
                    Tool Number : <asp:Label ID="Label_ToolNumber_Add" runat="server"></asp:Label> <br />
                </div> 
            </HeaderTemplate>--%>

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">

                <dx:WebChartControl ID="chartHistory" runat="server" Height="300px"
                    Width="700px" ClientInstanceName="chartHistory"
                    OnCustomCallback="chartHistory_CustomCallback"
                    CrosshairEnabled="False" ToolTipEnabled="False"
                    SeriesDataMember = "Calibration#"
                >
                    <Legend AlignmentHorizontal="Center" AlignmentVertical="BottomOutside" Direction="LeftToRight">
                    </Legend>
<%--                    <Titles>
                        <dx:ChartTitle Text="Tool Number"></dx:ChartTitle>
                    </Titles>--%>
                    <SeriesTemplate ArgumentScaleType="DateTime" ArgumentDataMember="CalibratedDate" ValueDataMembersSerializable="Value" LabelsVisibility="True">
                        <ViewSerializable>
                            <dx:LineSeriesView></dx:LineSeriesView>
                        </ViewSerializable>
                    </SeriesTemplate> 
                    <DiagramSerializable>
                        <dx:XYDiagram>
                            <AxisX Title-Text="Date" VisibleInPanesSerializable="-1" >
                                <DateTimeScaleOptions MeasureUnit="Day" />
                                <Label TextPattern="{V:yyyy-MM-dd}" />
                            </AxisX>
                            <AxisY Interlaced="True" Title-Text="Calibration Value" Title-Visibility="True" VisibleInPanesSerializable="-1">
                            </AxisY>
                        </dx:XYDiagram>
                    </DiagramSerializable>
                    <BorderOptions Visibility="False" />
                </dx:WebChartControl>

                </dx:PopupControlContentControl>
            </ContentCollection>

        </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="popupCalibration" ClientInstanceName="popupCalibration" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="100" MinHeight="100"
            OnWindowCallback="popupCalibration_WindowCallback" OnPopupWindowCommand="popupCalibration_PopupWindowCommand">
            <ContentStyle Paddings-Padding="0" />
            <ClientSideEvents Shown="onPopupShown"/>
            <HeaderTemplate>
                <div class="head">
                    INSERT NEW CALIBRATION <br /> 
                    Tool Number : <asp:Label ID="Label_ToolNumber_Add" runat="server"></asp:Label> <br />
                    Calibration Number : <asp:Label ID="Label_CalibrationNo_Add" runat="server"></asp:Label>
                </div> 

            </HeaderTemplate>
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table id="Tabel_Insert_Calibration" class="tabeldata" cellspacing="0" align="left" style="margin-top: 10px;">
                    <thead>
                        <tr class="trans">
                            <th rowspan="2" class="normalhead">Min</th>
                            <th rowspan="2" class="normalhead">Setting<br />Value</th>
                            <th rowspan="2" class="normalhead">Max</th>
                            <th colspan="5" class="normalhead">Checking Value</th>
                            <th rowspan="2" class="normalhead">Calibration Date</th>
                            <th rowspan="2" class="normalhead">Next Calibration<br />Date</th>
                            <th rowspan="2" class="normalhead">Status</th>
                            <th rowspan="2" class="normalhead">Remarks</th>
                        </tr>
                        <tr class="trans">
                            <th class="normalhead checkvalue">1</th>
                            <th class="normalhead checkvalue">2</th>
                            <th class="normalhead checkvalue">3</th>
                            <th class="normalhead checkvalue">4</th>
                            <th class="normalhead checkvalue">5</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="normal">
                            <td class="normalrow"><asp:Label ID="Label_MinValue_Add" runat="server" ForeColor="Red"></asp:Label></td>
                            <td class="normalrow"><asp:Label ID="Label_SettingValue_Add" runat="server" ForeColor="Blue"></asp:Label></td>
                            <td class="normalrow"><asp:Label ID="Label_MaxValue_Add" runat="server" ForeColor="Red"></asp:Label></td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Check1" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check1" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_Check1" runat="server" ValidationExpression="^[.0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check1" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_Check1" runat="server" CssClass="inputan" Width="40" ValidationGroup="ValGroup1"></asp:TextBox>
                            </td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Check2" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check2" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_Check2" runat="server" ValidationExpression="^[.0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check2" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_Check2" runat="server" CssClass="inputan" Width="40" ValidationGroup="ValGroup1"></asp:TextBox>
                            </td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Check3" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check3" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_Check3" runat="server" ValidationExpression="^[.0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check1" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_Check3" runat="server" CssClass="inputan" Width="40" ValidationGroup="ValGroup1"></asp:TextBox>
                            </td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Check4" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check4" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_Check4" runat="server" ValidationExpression="^[.0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check4" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_Check4" runat="server" CssClass="inputan" Width="40" ValidationGroup="ValGroup1"></asp:TextBox>
                            </td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Check5" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check5" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_Check5" runat="server" ValidationExpression="^[.0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_Check5" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_Check5" runat="server" CssClass="inputan" Width="40" ValidationGroup="ValGroup1"></asp:TextBox>
                            </td>
                            <td class="normalrow"><%= DateTime.Now.ToString("yyyy-MM-dd") %></td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_NextCalibration" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_NextCalibration" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_NextCalibration" runat="server" ValidationExpression="^[0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_NextCalibration" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_NextCalibration" runat="server" CssClass="inputan" Width="20" MaxLength="3" ValidationGroup="ValGroup1"></asp:TextBox> Days Later
                            </td>
                            <td class="normalrow">
                                <asp:DropDownList ID="DropDownList_Status" runat="server">
                                    <asp:ListItem>OK</asp:ListItem>
                                    <asp:ListItem>REPAIR</asp:ListItem>
                                    <asp:ListItem>DONE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="normalrow">
                                <asp:TextBox ID="TextBox_Remarks" runat="server" CssClass="inputan" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table> 

                    <div style="text-align: center;"><hr /><asp:Button ID="Button_Save" runat="server" class="inputan button" 
                                    Text="Save" ValidationGroup="ValGroup1" /></div> 

                    <div>
                        List of 10 latest calibration data
                    </div>
                    <asp:Repeater ID="Repeater_Calibration" runat="server">
                    <HeaderTemplate>
                        <table class="tabeldata" cellspacing="0" align="left">
                            <thead>
                                <tr class="trans">
                                    <th rowspan="2" class="normalhead">No.</th>
                                    <th rowspan="2" class="normalhead">Tool Number</th>
                                    <th rowspan="2" class="normalhead">Calibration No.</th>
                                    <th rowspan="2" class="normalhead">Min</th>
                                    <th rowspan="2" class="normalhead">Setting Value</th>
                                    <th rowspan="2" class="normalhead">Max</th>
                                    <th colspan="5" class="normalhead">Checking Value</th>
                                    <th rowspan="2" class="normalhead">Calibration Date</th>
                                    <th rowspan="2" class="normalhead">Next Calibration<br />Date</th>
                                    <th rowspan="2" class="normalhead"></th>
                                </tr>
                                <tr class="trans">
                                    <th class="normalhead checkvalue">1</th>
                                    <th class="normalhead checkvalue">2</th>
                                    <th class="normalhead checkvalue">3</th>
                                    <th class="normalhead checkvalue">4</th>
                                    <th class="normalhead checkvalue">5</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                                <tr class="rowhighlight normal" bgcolor="<%# (Container.ItemIndex % 2) == 0 ? "" : "#f2f2f2" %>">
                                    <td class="normalrow"><%#Container.ItemIndex + 1%></td>
                                    <td class="normalrow"><%#Eval("tool_number")%></td>
                                    <td class="normalrow"><%#Eval("calibration_no")%></td>
                                    <td class="normalrow" style="color: red;"><asp:Label ID="Label_MinValue" runat="server" Text=""></asp:Label></td>
                                    <td class="normalrow" style="color: blue;"><%#Eval("setting_value")%></td>
                                    <td class="normalrow" style="color: red;"><asp:Label ID="Label_MaxValue" runat="server" Text=""></asp:Label></td>
                                    <td class="checkvalue"><%#Eval("check1")%></td>
                                    <td class="checkvalue"><%#Eval("check2")%></td>
                                    <td class="checkvalue"><%#Eval("check3")%></td>
                                    <td class="checkvalue"><%#Eval("check4")%></td>
                                    <td class="checkvalue"><%#Eval("check5")%></td>
                                    <td class="normalrow"><%#Eval("tanggal", "{0: yyyy-MM-dd}")%></td>
                                    <td class="normalrow"><%#Eval("next_cal", "{0: yyyy-MM-dd}")%></td>
                                    <td>
                                        <asp:Button ID="Button_Delete_Data" runat="server" CssClass="inputan button" Text="Delete" CommandArgument='<%#Eval("id")%>' CommandName="" OnClientClick="return confirm('Are you sure you want to delete this data ?');" />
                                    </td>
                                </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                                <tr>
                                    <td colspan="14">
                                        <input id="Button_Insert_Data" class="inputan button" type="button" value="+ Insert New Calibration" name='' onclick="clearAddControl(); Popup.showModal('modal'); return false;" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </body>
    </html>
</asp:Content>
