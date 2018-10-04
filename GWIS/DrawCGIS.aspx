<%@ Page Title="Draw CGIS" Language="VB" UnobtrusiveValidationMode="None" AutoEventWireup="false" CodeFile="DrawCGIS.aspx.vb" Inherits="report_drawCGIS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Expires" content="0" />
    <title></title>
    <script type="text/javascript" src="../Scripts/popup.js"></script>
    <link rel="Stylesheet" type="text/css" href="../css/cmptheme.css" />

    <script type="text/javascript" src="/Content/Scripts/jquery_notification_v.1.js"></script>
    <link href="/Content/css/notification_style.css" type="text/css" rel="stylesheet"/>

<%--    <script type="text/javascript" src="../Scripts/jquery-1.7.1.js"></script>    
    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>
    <link rel="stylesheet" type="text/css" href="../Styles/jquery-ui.css" />

    <link rel="Stylesheet" type="text/css" href="../Scripts/jquery-ui-1.10.3.custom/css/ui-lightness/jquery-ui-1.10.3.custom.min.css" />
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../Scripts/jquery.imgareaselect-0.9.10/css/imgareaselect-animated.css" />
    <script type="text/javascript" src="../Scripts/jquery.imgareaselect-0.9.10/scripts/jquery.imgareaselect.pack.js"></script>--%>

    <link rel="stylesheet" type="text/css" href="/Content/css/imgareaselect-default.css" />
    <script type="text/javascript" src="/Content/scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/Content/scripts/jquery.imgareaselect.pack.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= Image_Edit.ClientID %>").imgAreaSelect({
                fadeSpeed: 400,
                instance: true,
                handles: false,
                // x1: 42, y1: 96, x2: 57, y2: 123,
                keys: { arrows: 15, ctrl: 5, shift: 'resize' },
                onSelectStart: preview,
                onSelectChange: preview,
                onSelectEnd:
                function (img, selection) {
                    if (document.getElementById("<%=HiddenField_Edit.ClientID%>").value == "0") {
                        $("#dialog").dialog({
                            title: "Entry New Shape",
                            autoOpen: false,
                            minHeight: 20,
                            minWidth: 20,
                            width: 300,
                            show: {
                                effect: "fade",
                                duration: 350
                            },
                            hide: {
                                effect: "blind",
                                duration: 200
                            },
                            buttons: {
                                Submit: function () {
                                    $("#<%= Button_Save.ClientID %>").click();
                                },
                                Close: function () {
                                    $(this).dialog('close');
                                }
                            },
                            open: function (type, data) {
                                $(this).parent().appendTo("form");

                                $(".ui-dialog-title").hide();
                                $(".ui-dialog-titlebar-close").hide();
                                $(".ui-dialog-titlebar").hide();
                            },
                            close: function (type, data) {
                                ($(this).parent().replaceWith(""));
                            }
                        });

                        if (selection.width > 5 && selection.height > 5) {
                            $("#dialog").dialog("option", "position", [selection.x2 + 20, selection.y1 + 20]);
                            $("#dialog").dialog("open");
                            $(".ui-effects-wrapper").css("z-index", -1);
                        } else {
                            $("#dialog").dialog("close");
                        }
                    }
                }
            });

        });

        function getValueForEdit(currents) {

            document.getElementById("edit_table").style.display = "block";
            document.getElementById("<%=HiddenField_Edit.ClientID%>").value = "1";
            document.getElementById("<%=HiddenField_Id_Edit.clientid %>").value = currents.name;
            document.getElementById("<%=Label_ID.clientid %>").innerHTML = currents.name;
            var editParams = currents.title.split("|");
            SetSelectedIndex("<%=DropDownList_Color_Edit.clientid %>", editParams[0]);
            document.getElementById("<%=TextBox_Start_Edit.clientid %>").value = editParams[1] + ", " + editParams[2];
            document.getElementById("<%=TextBox_End_Edit.ClientID%>").value = editParams[3] + ", " + editParams[4];
            document.getElementById("<%=TextBox_Remarks_Edit.clientid %>").value = editParams[5];

            if (editParams[6] == "1") {
                document.getElementById("<%= DropDownList_ArrowDirection_Edit.ClientID%>").style.display = '';
                document.getElementById("<%= DropDownList_ArrowDirection2_Edit.ClientID%>").style.display = '';
                document.getElementById("<%= Label_ArrowDirection_Edit.ClientID%>").style.display = '';
                document.getElementById("<%= Label_ArrowDirection2_Edit.ClientID%>").style.display = '';
                SetSelectedIndex("<%= DropDownList_ArrowDirection_Edit.ClientID%>", "Right");
                SetSelectedIndex("<%= DropDownList_ArrowDirection2_Edit.ClientID%>", "Down");
            } else {
                document.getElementById("<%= DropDownList_ArrowDirection_Edit.ClientID%>").style.display = 'none';
                document.getElementById("<%= DropDownList_ArrowDirection2_Edit.ClientID%>").style.display = 'none';
                document.getElementById("<%= Label_ArrowDirection_Edit.ClientID%>").style.display = 'none';
                document.getElementById("<%= Label_ArrowDirection2_Edit.ClientID%>").style.display = 'none';
            }

        }

        function preview(img, selection) {
            SetSelectedIndex("<%= DropDownList_ArrowDirection.ClientID%>", "Right");
            SetSelectedIndex("<%= DropDownList_ArrowDirection2.ClientID%>", "Down");

            var picwidth = $("#<%= Image_Edit.ClientID %>").width();
            var picheight = $("#<%= Image_Edit.ClientID %>").height();
            var picdev = picwidth / picheight;

            if (!selection.width || !selection.height)
                return;

            var scaleX = 100 / selection.width;
            var scaleY = 100 / selection.height;

            $('#preview img').css({
                width: Math.round(scaleX * picwidth),
                height: Math.round(scaleY * picheight),
                marginLeft: -Math.round(scaleX * selection.x1),
                marginTop: -Math.round(scaleY * selection.y1)
            });
            
            document.getElementById("<%= TextBox_Start.ClientID%>").value = selection.x1 + "," + selection.y1;
            document.getElementById("<%= TextBox_End.ClientID%>").value = selection.x2 + "," + selection.y2;
            document.getElementById("<%= TextBox_Start_Edit.ClientID%>").value = selection.x1 + "," + selection.y1;
            document.getElementById("<%= TextBox_End_Edit.ClientID%>").value = selection.x2 + "," + selection.y2;
            document.getElementById("<%= HiddenField_Start.ClientID%>").value = selection.x1 + "," + selection.y1;
            document.getElementById("<%= HiddenField_End.ClientID%>").value = selection.x2 + "," + selection.y2;
            document.getElementById("<%= HiddenField_Start_Edit.ClientID%>").value = selection.x1 + "," + selection.y1;
            document.getElementById("<%= HiddenField_End_Edit.ClientID%>").value = selection.x2 + "," + selection.y2;
        };

        function gotoSomething() {
            $('html,body').animate({ scrollTop: $("#edit_table").offset().top }, 'slow');
        }

        function SetSelectedIndex(dropdownlist, sVal) {
            var a = document.getElementById(dropdownlist);
            for (i = 0; i < a.length; i++) {
                if (a.options[i].value == sVal) {
                    a.selectedIndex = i;
                }
            }
        }

        function canceledit() {
            document.getElementById("<%=HiddenField_Edit.ClientID%>").value = "0";
            $('html,body').animate({ scrollTop: $("body").offset().top }, 'slow');
            document.getElementById("edit_table").style.display = "none";
        }

        $(document).ready(function () {
            $(".controlanchor").click(function () {
                $("#changeimage").toggle("blind", { direction: "up" }, 200);
            });

            $("#<%= TextBox_Start.ClientID %>").click(function () {
                $("#<%= HiddenField_C.ClientID %>").val("0");
                $("#<%= TextBox_Start.ClientID %>").select();
            });
            $("#<%= TextBox_End.ClientID %>").click(function () {
                $("#<%= HiddenField_C.ClientID %>").val("1");
                $("#<%= TextBox_End.ClientID %>").select();
            });
            $("#<%= TextBox_Start_Edit.ClientID %>").click(function () {
                $("#<%= HiddenField_C.ClientID %>").val("0");
                $("#<%= TextBox_Start_Edit.ClientID %>").select();
            });
            $("#<%= TextBox_End_Edit.ClientID %>").click(function () {
                $("#<%= HiddenField_C.ClientID %>").val("1");
                $("#<%= TextBox_End_Edit.ClientID %>").select();
            });
        });

        function checkType() {
            var control = document.getElementById("<%= DropDownList_Type.ClientID %>");
            if (control.options[control.selectedIndex].value == "1") {
                document.getElementById("<%= DropDownList_ArrowDirection.ClientID%>").style.display = '';
                document.getElementById("<%= DropDownList_ArrowDirection2.ClientID%>").style.display = '';
                SetSelectedIndex("<%= DropDownList_ArrowDirection.ClientID%>", "Right");
                SetSelectedIndex("<%= DropDownList_ArrowDirection2.ClientID%>", "Down");
            } else {
                document.getElementById("<%= DropDownList_ArrowDirection.ClientID%>").style.display = 'none';
                document.getElementById("<%= DropDownList_ArrowDirection2.ClientID%>").style.display = 'none';
            }
        };

        function setArrowDirection() {
            var coStart = document.getElementById("<%= HiddenField_Start.ClientID%>").value.split(",");
            var coEnd = document.getElementById("<%= HiddenField_End.ClientID%>").value.split(",");
            var x1 = coStart[0];
            var y1 = coStart[1];
            var x2 = coEnd[0];
            var y2 = coEnd[1];

            var e = document.getElementById("<%= DropDownList_ArrowDirection.ClientID%>");
            var a = document.getElementById("<%= DropDownList_ArrowDirection2.ClientID%>");
            var hor = e.options[e.selectedIndex].value;
            var ver = a.options[a.selectedIndex].value;

            if ((hor == "Right") && (ver == "Down")) {
                document.getElementById("<%= TextBox_Start.ClientID%>").value = x1 + "," + y1;
                document.getElementById("<%= TextBox_End.ClientID%>").value = x2 + "," + y2;
            } else if ((hor == "Right") && (ver == "Up")) {
                document.getElementById("<%= TextBox_Start.ClientID%>").value = x1 + "," + y2;
                document.getElementById("<%= TextBox_End.ClientID%>").value = x2 + "," + y1;
            } else if ((hor == "Left") && (ver == "Down")) {
                document.getElementById("<%= TextBox_Start.ClientID%>").value = x2 + "," + y1;
                document.getElementById("<%= TextBox_End.ClientID%>").value = x1 + "," + y2;
            } else if ((hor == "Left") && (ver == "Up")) {
                document.getElementById("<%= TextBox_Start.ClientID%>").value = x2 + "," + y2;
                document.getElementById("<%= TextBox_End.ClientID%>").value = x1 + "," + y1;
            }
        };

        function setArrowDirection_Edit() {
            var coStart = document.getElementById("<%= HiddenField_Start_Edit.ClientID%>").value.split(",");
            var coEnd = document.getElementById("<%= HiddenField_End_Edit.ClientID%>").value.split(",");

            if ((coStart != "" && coEnd != "")) {
                var x1 = coStart[0];
                var y1 = coStart[1];
                var x2 = coEnd[0];
                var y2 = coEnd[1];

                var e = document.getElementById("<%= DropDownList_ArrowDirection_Edit.ClientID%>");
                var a = document.getElementById("<%= DropDownList_ArrowDirection2_Edit.ClientID%>");
                var hor = e.options[e.selectedIndex].value;
                var ver = a.options[a.selectedIndex].value;

                if ((hor == "Right") && (ver == "Down")) {
                    document.getElementById("<%= TextBox_Start_Edit.ClientID%>").value = x1 + "," + y1;
                    document.getElementById("<%= TextBox_End_Edit.ClientID%>").value = x2 + "," + y2;
                } else if ((hor == "Right") && (ver == "Up")) {
                    document.getElementById("<%= TextBox_Start_Edit.ClientID%>").value = x1 + "," + y2;
                    document.getElementById("<%= TextBox_End_Edit.ClientID%>").value = x2 + "," + y1;
                } else if ((hor == "Left") && (ver == "Down")) {
                    document.getElementById("<%= TextBox_Start_Edit.ClientID%>").value = x2 + "," + y1;
                    document.getElementById("<%= TextBox_End_Edit.ClientID%>").value = x1 + "," + y2;
                } else if ((hor == "Left") && (ver == "Up")) {
                    document.getElementById("<%= TextBox_Start_Edit.ClientID%>").value = x2 + "," + y2;
                    document.getElementById("<%= TextBox_End_Edit.ClientID%>").value = x1 + "," + y1;
                }
            }
            
    };
            
    </script>
    <style type="text/css">
        .area{
	         position:relative;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
    <asp:HiddenField ID="HiddenField_C" runat="server" Value="0"/>
    <asp:HiddenField ID="HiddenField_Edit" runat="server" Value="0"/>

        <table>
        <tr>
        <td style="vertical-align: top;">
        
            <div id="dialog" style="display: none; padding-left: 5px; padding-top: 0;">
                <asp:HiddenField ID="HiddenField_Start" runat="server" />
                <asp:HiddenField ID="HiddenField_End" runat="server" />
                <table class="tableinput" id="input_table" style="margin-top: 0; padding-top: 0; padding-bottom: 0;">
                    <thead>
                        <tr>
                            <th>Draw New Shape</th>
                        </tr>
                    </thead>
                    <tbody> 
                    <tr>
                        <td class="ttl">Type<br />
                            <asp:DropDownList ID="DropDownList_Type" runat="server" CssClass="inputan" OnChange="checkType();">
                                <asp:ListItem Value="1">Arrow</asp:ListItem>
                                <asp:ListItem Value="2">Rectangle</asp:ListItem>
                                <asp:ListItem Value="3">Ellipse</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList_ArrowDirection" runat="server" CssClass="inputan" OnChange="setArrowDirection()">
                               <asp:ListItem Value="Right">Right</asp:ListItem>
                                <asp:ListItem Value="Left">Left</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList_ArrowDirection2" runat="server" CssClass="inputan" OnChange="setArrowDirection()">
                               <asp:ListItem Value="Up">Up</asp:ListItem>
                                <asp:ListItem Value="Down">Down</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="ttl">Color<br />
                            <asp:DropDownList ID="DropDownList_Color" runat="server" CssClass="inputan">
                                <asp:ListItem>Black</asp:ListItem>
                                <asp:ListItem>Red</asp:ListItem>
                                <asp:ListItem>Blue</asp:ListItem>
                                <asp:ListItem value="Lime">Green</asp:ListItem>
                                <asp:ListItem>Yellow</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="ttl">Start Point<br />
                        <asp:TextBox ID="TextBox_Start" runat="server" Width="50px" CssClass="inputan" Style="border: solid 1px #4698ca;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_StartPoint" runat="server" ErrorMessage="<br /> Must Not Empty" ValidationGroup="drawGroup" ControlToValidate="TextBox_Start" Display="Dynamic" Style="color: red;"></asp:RequiredFieldValidator>
                        </td>
                        <td class="ttl">End Point<br />
                        <asp:TextBox ID="TextBox_End" runat="server" Width="50px" CssClass="inputan" Style="border: solid 1px #4698ca;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_EndPoint" runat="server" ErrorMessage="<br /> Must Not Empty" ValidationGroup="drawGroup" ControlToValidate="TextBox_End" Display="Dynamic" Style="color: red;"></asp:RequiredFieldValidator>
                        </td>
                        <td class="ttl">Remarks<br />
                        <asp:TextBox ID="TextBox_Remarks" runat="server" CssClass="inputan"></asp:TextBox></td>
                        <td class="ttl"><asp:Button ID="Button_Save" runat="server" Text="Draw" style="display: none;" CssClass="inputan button" ValidationGroup="drawGroup" /></td>
                    </tr>
                    </tbody> 
                </table>
            </div> 
        </td>
        <td style="vertical-align: top" rowspan="3">
    <asp:Repeater ID="Repeater_Annot" runat="server">
    <HeaderTemplate>
        <table class="tabeldata" style="margin-left: 2em;">
            <thead>
                <tr class="trans">
                    <th>ID</th>
                    <th>Type</th>
                    <th>Color</th>
                    <th>Start Point</th>
                    <th>End Point</th>
                    <th>Remarks</th>
                    <th>Last Edited</th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
                <tr class="rowhighlight normal" bgcolor="<%# iif(Container.ItemIndex Mod 2 = 0, "", "#f2f2f2") %>">
                    <td><%# Eval("id")%></td>
                    <td><%# Eval("strType")%></td>
                    <td><%# Eval("Color")%></td>
                    <td><%# Eval("pos_left") & ", " & Eval("pos_top") %></td>
                    <td><%# Eval("x2") & ", " & Eval("y2") %></td>
                    <td><%# Eval("remarks") %></td>
                    <td><%# Eval("edit_date") & " by " & Eval("editor") %></td>
                    <td><input id="Button_Edit_Data" class="inputan button" type="button" value="Edit" name='<%#Eval("id") %>' title='<%# Eval("color") & "|" & Eval("pos_left") & "|" & Eval("pos_top") & "|" & Eval("x2") & "|" & Eval("y2") & "|" & Eval("remarks") & "|" & Eval("type") %>' onclick="getValueForEdit(this); gotoSomething();" /></td>
                    <td>
                        <asp:Button ID="Button_Delete_Data" runat="server" CssClass="inputan button" Text="Delete" CommandArgument='<%#Eval("id") %>' CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this data ?');" /><br />
                    </td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
    </asp:Repeater>
    </td> 
    </tr> 
    <tr>
        <td style="vertical-align: top;">
            <table class="tableinput" id="edit_table" style="display: none; width: auto;">
                <thead>
                    <tr>
                        <th colspan="8" class="style1">Edit Shape
                            <asp:HiddenField ID="HiddenField_Start_Edit" runat="server" />
                            <asp:HiddenField ID="HiddenField_End_Edit" runat="server" />
                        </th>
                    </tr> 
                </thead>
                <tbody>
                <tr>
                    <td class="ttl">ID<br />
                    <asp:Label ID="Label_ID" runat="server" Text="" Style="color: #405778; font-weight: bold;"></asp:Label></td>
                    <td class="ttl">Color<br />
                        <asp:DropDownList ID="DropDownList_Color_Edit" runat="server" CssClass="inputan">
                            <asp:ListItem>Black</asp:ListItem>
                            <asp:ListItem>Red</asp:ListItem>
                            <asp:ListItem>Blue</asp:ListItem>
                            <asp:ListItem value="Lime">Green</asp:ListItem>
                            <asp:ListItem>Yellow</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="ttl"><asp:Label ID="Label_ArrowDirection_Edit" runat="server" Text="Horizontal"></asp:Label><br />
                        <asp:DropDownList ID="DropDownList_ArrowDirection_Edit" runat="server" CssClass="inputan" OnChange="setArrowDirection_Edit()">
                            <asp:ListItem Value="Right">Right</asp:ListItem>
                            <asp:ListItem Value="Left">Left</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="ttl"><asp:Label ID="Label_ArrowDirection2_Edit" runat="server" Text="Vertical"></asp:Label><br />
                        <asp:DropDownList ID="DropDownList_ArrowDirection2_Edit" runat="server" CssClass="inputan" OnChange="setArrowDirection_Edit()">
                            <asp:ListItem Value="Up">Up</asp:ListItem>
                            <asp:ListItem Value="Down">Down</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="ttl">Start Point<br />
                    <asp:TextBox ID="TextBox_Start_Edit" runat="server" Width="50px" CssClass="inputan" Style="border: solid 1px #4698ca;"></asp:TextBox></td>
                    <td class="ttl">End Point<br />
                    <asp:TextBox ID="TextBox_End_Edit" runat="server" Width="50px" CssClass="inputan" Style="border: solid 1px #4698ca;"></asp:TextBox></td>
                </tr> 
                <tr>
                    <td class="ttl">Remarks<br />
                    <asp:TextBox ID="TextBox_Remarks_Edit" runat="server" CssClass="inputan"></asp:TextBox></td>
                    <td class="ttl"><br />
                    <asp:HiddenField ID="HiddenField_Id_Edit" runat="server" />
                    <asp:Button ID="Button_Edit" runat="server" Text="Edit" CssClass="inputan button" /></td>
                    <td class="ttl"><br />
                    <input id="Button_Cancel" class="inputan button" type="button" value="Cancel" onclick="canceledit()" style="border: 1px solid red" /></td>
                </tr>
                </tbody> 
            </table>

            <div class="area">
                <asp:Image ID="Image_Edit" runat="server" ImageUrl="" />
            </div> 
        </td>
    </tr>
    <tr>
        <td>
            <a href="#" class="controlanchor">Change Image</a><br />
            <div id="changeimage" style="display: none;">
                <asp:FileUpload ID="FileUpload_ChangeImage" runat="server" />
                <asp:Button ID="Button_Change" runat="server" Text="Change" class="inputan button" />
            </div> 
        </td>
    </tr>
    </table> 

    </div>
    </form>
</body>
</html>

    
    
        