<%@ Page Title="" UnobtrusiveValidationMode="None" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ToolVerifications.aspx.cs" Inherits="Ebiz.WebForm.custom.Tool.ToolVerifications" %>

<%--<%@ Register assembly="DevExpress.XtraCharts.v17.2.Web, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="dx" %>--%>

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

            function showHistory(s, e)
            {
                s.GetRowValues(e, 'Id;Number', OnGetRowValues);
            }

            function OnGetRowValues(values) {
                popupHistory.SetHeaderText('Verification History - Last 10 (' + values[1] + ')');
                popupHistory.PerformCallback(values[1] + ';' + values[0]);
                popupHistory.Show();
            }

        </script>

    </head>
    <body>

        <h2 class='grid-header'>Tool Verifications (TODAY)</h2>

        <dx:ASPxPopupControl ID="popupHistory" ClientInstanceName="popupHistory" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            MinWidth="100" MinHeight="100"
            OnWindowCallback="popupHistory_WindowCallback" OnPopupWindowCommand="popupHistory_PopupWindowCommand" 
            HeaderText="Verification History">
            <ContentStyle Paddings-Padding="20" />
<%--            <ClientSideEvents Shown="onPopupShown"/>--%>

<%--            <HeaderTemplate>
                <div class="head">
                    Verification History <br /> 
                    Tool Number : <asp:Label ID="Label_ToolNumber_Add" runat="server"></asp:Label> <br />
                </div> 
            </HeaderTemplate>
--%>
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">

                <dx:WebChartControl ID="chartHistory" runat="server" Height="300px"
                    Width="700px" ClientInstanceName="chartHistory"
                    OnCustomCallback="chartHistory_CustomCallback"
                    CrosshairEnabled="False" ToolTipEnabled="False"
                    SeriesDataMember = "Verification#"
                >
                    <Legend AlignmentHorizontal="Center" AlignmentVertical="BottomOutside" Direction="LeftToRight">
                    </Legend>
<%--                    <Titles>
                        <dx:ChartTitle Text="Tool Number"></dx:ChartTitle>
                    </Titles>--%>
                    <SeriesTemplate ArgumentScaleType="DateTime" ArgumentDataMember="VerifiedDate" ValueDataMembersSerializable="Value" LabelsVisibility="True">
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
                            <AxisY Interlaced="True" Title-Text="Verification Value" Title-Visibility="True" VisibleInPanesSerializable="-1">
                            </AxisY>
                        </dx:XYDiagram>
                    </DiagramSerializable>
                    <BorderOptions Visibility="False" />
                </dx:WebChartControl>

                </dx:PopupControlContentControl>
            </ContentCollection>

        </dx:ASPxPopupControl>

<%--    <asp:SqlDataSource ID="sdsHistory" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="
            select u.VerifiedDate, u.Verification#, u.Value
            from 
            (
	            select TOP 10 ver.VerifiedDate, ver.Verification1, ver.Verification2, ver.Verification3 
	            from [dbo].[ToolVerifications] ver
	            where ver.ToolInventoryId = @ToolInventoryId
	            order by ver.VerifiedDate DESC
            ) v
            unpivot
            (
              Value
              for Verification# in (Verification1, Verification2, Verification3)
            ) u;        
        "
    >
        <SelectParameters>
            <asp:Parameter Name="ToolInventoryId" DbType="Int32" DefaultValue="3" />
        </SelectParameters>
    </asp:SqlDataSource>
--%>

        <dx:ASPxPopupControl ID="popupVerification" ClientInstanceName="popupVerification" runat="server" AllowDragging="false" AutoUpdatePosition="True"
            CloseAction="CloseButton" CloseAnimationType="Fade" CloseOnEscape="True" Modal="True"
            PopupAnimationType="Fade" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="100" MinHeight="100"
            OnWindowCallback="popupVerification_WindowCallback" OnPopupWindowCommand="popupVerification_PopupWindowCommand">
            <ContentStyle Paddings-Padding="0" />
            <ClientSideEvents Shown="onPopupShown"/>
            <HeaderTemplate>
                <div class="head">
                    INSERT NEW VERIFICATION <br /> 
                    Tool Number : <asp:Label ID="Label_ToolNumber_Add" runat="server"></asp:Label> <br />
                    Verification Number : <asp:Label ID="Label_VerificationNo_Add" runat="server"></asp:Label>
                </div> 

            </HeaderTemplate>
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                <table id="Tabel_Insert_Verification" class="tabeldata" cellspacing="0" align="left" style="margin-top: 10px;">
                    <thead>
                        <tr class="trans">
                            <th rowspan="2" class="normalhead">Min</th>
                            <th rowspan="2" class="normalhead">Setting<br />Value</th>
                            <th rowspan="2" class="normalhead">Max</th>
                            <th colspan="3" class="normalhead">Checking Value</th>
                            <th rowspan="2" class="normalhead">Verification Date</th>
                            <th rowspan="2" class="normalhead">Next Verification<br />Date</th>
                            <th rowspan="2" class="normalhead">Status</th>
                            <th rowspan="2" class="normalhead">Remarks</th>
                        </tr>
                        <tr class="trans">
                            <th class="normalhead checkvalue">1</th>
                            <th class="normalhead checkvalue">2</th>
                            <th class="normalhead checkvalue">3</th>
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
                            <td class="normalrow"><%= DateTime.Now.ToString("yyyy-MM-dd") %></td>
                            <td class="normalrow">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_NextVerification" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="This field cannot be empty ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_NextVerification" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_TextBox_NextVerification" runat="server" ValidationExpression="^[0-9]*$" 
                                        ErrorMessage="RegularExpressionValidator" ValidationGroup="ValGroup1" 
                                        SetFocusOnError="True" Text="Only numeric and <br />dot are allowed ! <br />" 
                                        Display="Dynamic" ControlToValidate="TextBox_NextVerification" ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="TextBox_NextVerification" runat="server" CssClass="inputan" Width="20" MaxLength="3" ValidationGroup="ValGroup1"></asp:TextBox> Days Later
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
                        List of 10 latest verification data
                    </div>
                    <asp:Repeater ID="Repeater_Verification" runat="server">
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
                                    <th colspan="3" class="normalhead">Checking Value</th>
                                    <th rowspan="2" class="normalhead">Verification Date</th>
                                    <th rowspan="2" class="normalhead">Next Verification<br />Date</th>
                                    <th rowspan="2" class="normalhead"></th>
                                </tr>
                                <tr class="trans">
                                    <th class="normalhead checkvalue">1</th>
                                    <th class="normalhead checkvalue">2</th>
                                    <th class="normalhead checkvalue">3</th>
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
                                    <td class="normalrow"><%#Eval("tanggal", "{0: yyyy-MM-dd}")%></td>
                                    <td class="normalrow"><%#Eval("next_tanggal", "{0: yyyy-MM-dd}")%></td>
                                    <td>
                                        <asp:Button ID="Button_Delete_Data" runat="server" CssClass="inputan button" Text="Delete" CommandArgument='<%#Eval("id")%>' CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this data ?');" />
                                    </td>
                                </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                                <tr>
                                    <td colspan="14">
                                        <input id="Button_Insert_Data" class="inputan button" type="button" value="+ Insert New Verification" name='' onclick="clearAddControl(); Popup.showModal('modal'); return false;" />
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
