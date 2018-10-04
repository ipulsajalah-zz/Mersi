<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.master" AutoEventWireup="true" CodeBehind="FMEA.aspx.cs" Inherits="Ebiz.WebForm.custom.FMEA.FMEA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function btnAdd_Click(s, e) {
            PopUpAddFMEA.Show();
        }

        function btnEdit_Click(s, e) {
            PopUpEditFMEA.Show();
        }

        function btnCancelFMEA_Click(s, e) {
            PopUpAddFMEA.Hide();
        }

        function btnCancelFMEAEdit_Click(s, e) {
            PopUpEditFMEA.Hide();
        }

    </script>
   
    <div align="center" id="wrapper">
    <div id="fullside">
    <h3 style="text-align: center;">FMEA</h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="False">
    </asp:ScriptManager>

        <dx:ASPxButton ID="btnAdd" runat="server" Text="Add" ClientInstanceName="btnAdd" AutoPostBack="false">
            <ClientSideEvents Click="btnAdd_Click" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="btnEdit" runat="server" Text="Edit" ClientInstanceName="btnEdit" AutoPostBack="false" OnClick="btnEdit_Click">
            <ClientSideEvents Click="btnEdit_Click" />
        </dx:ASPxButton>

    <!-- Modal Pop Up -->
    
   
   <!-- Modal Pop Up -->
    

        <!-- Modal Pop Up Add FMEA -->
        <dx:ASPxPopupControl ID="PopUpAddFMEA" ClientInstanceName="PopUpAddFMEA" runat="server" OnWindowCallback="PopUpAddFMEA_WindowCallback" ShowFooter="true" AllowDragging="true" >
          
          <ContentCollection>
              <dx:PopupControlContentControl>                                              
                          <div style="float: right"><a href="#" onclick="Popup.hide('LP');">close</a></div> 
                          <div style="clear:both;"></div>

                          <table style="background-color:#f3f3f3; margin-top: 2em; " cellspacing="0"cellpadding="4" border="0" width="100%">
                             <tr>
                                <th colspan="12" style="text-align: left; font-size:10pt; color: #405778;">
                                </th>
                             </tr>                             
                            <tr bgcolor="#f2f2f2" style="color: #202020;">
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">FMEA Number</th>                                
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Type</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Assembly Model</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Area</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Station</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Process No</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Part No</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Potential Failure Mode</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Potential Failure Effects</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">SEV</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Class</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Potential Causes</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">OCC</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Controls Prevention</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">DET</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Controls Detection</th>
                                <th bgcolor="#FF3300">RPN</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Valfrom</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Valto</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">&nbsp;</th>
                            </tr>
                            <tr style="text-align:center">                                
                                <td><asp:TextBox ID="txtFMEANo" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlType" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAssemblyModel" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlArea" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStation" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProcessNo" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPartNo" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtPotentialFailureMode" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtPotentialFailureEffect" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlSEV" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlClass" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt" >
                                        <asp:ListItem Value="">-</asp:ListItem>
                                        <asp:ListItem Enabled="True" Value="A">A</asp:ListItem>
                                        <asp:ListItem Enabled="True" Value="B">B</asp:ListItem>
                                        <asp:ListItem Enabled="True" Value="C">C</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtPotentialCauses" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlOCC" AutoPostBack="false" runat="server" 
                                        Font-Names="Arial" Font-Size="8pt" >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtControlsPrevention" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlDET" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt" >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtControlsDetection" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td style="text-align: center">
                                    <asp:Label ID="lblRPN" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="#405778"></asp:Label>
                                </td>
                                <td><asp:TextBox ID="txtValfrom" runat="server" Width="100px" Font-Names="Arial" 
                                    Font-Size="8pt" CssClass="tanggal"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtValto" runat="server" Width="100px" Font-Names="Arial" 
                                    Font-Size="8pt" CssClass="tanggal"></asp:TextBox></td>
                                <td>
                                    <asp:HiddenField ID="hddId" runat="server" />
                                </td>                   
                              </tr>
                        </table>
              </dx:PopupControlContentControl>
          </ContentCollection>
              
          <FooterContentTemplate>
            <table>
                <tr>
                    <td style="width: 100%">
                    </td>
                    <td>
                        <dx:ASPxButton runat="server" ID="btnSaveFMEA" ClientInstanceName="btnSaveFMEA" Text="Add" AutoPostBack="false" OnClick="btnSaveFMEA_Click">
                        </dx:ASPxButton>
                      </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                      <dx:ASPxButton runat="server" ID="btnCancelFMEA" ClientInstanceName="btnCancelFMEA" Text="Cancel" AutoPostBack="false">
                          <ClientSideEvents Click="btnCancelFMEA_Click" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </FooterContentTemplate>

        </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="PopUpEditFMEA" ClientInstanceName="PopUpEditFMEA" runat="server" OnWindowCallback="PopUpEditFMEA_WindowCallback" ShowFooter="true" AllowDragging="true" >
          
          <ContentCollection>
              <dx:PopupControlContentControl>                                              
                          <div style="float: right"><a href="#" onclick="Popup.hide('LP');">close</a></div> 
                          <div style="clear:both;"></div>

                          <table style="background-color:#f3f3f3; margin-top: 2em; " cellspacing="0"cellpadding="4" border="0" width="100%">
                             <tr>
                                <th colspan="12" style="text-align: left; font-size:10pt; color: #405778;">
                                </th>
                             </tr>                             
                            <tr bgcolor="#f2f2f2" style="color: #202020;">
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">FMEA Number</th>                                
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Type</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Assembly Model</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Area</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Station</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Process No</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Part No</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Potential Failure Mode</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Potential Failure Effects</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">SEV</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Class</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Potential Causes</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">OCC</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Controls Prevention</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">DET</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Controls Detection</th>
                                <th bgcolor="#FF3300">RPN</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Valfrom</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">Valto</th>
                                <th style="border-top: 1px solid #4698ca; border-bottom: 1px solid #4698ca">&nbsp;</th>
                            </tr>
                            <tr style="text-align:center">                                
                                <td><asp:TextBox ID="txtFMEANoEdit" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlTypeEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAssemblyModelEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAreaEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStationEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProcessNoEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPartNoEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  ></asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtPotentialFailureModeEdit" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtPotentialFailureEffectEdit" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlSEVEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt"  >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlClassEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt" >
                                        <asp:ListItem Value="">-</asp:ListItem>
                                        <asp:ListItem Enabled="True" Value="A">A</asp:ListItem>
                                        <asp:ListItem Enabled="True" Value="B">B</asp:ListItem>
                                        <asp:ListItem Enabled="True" Value="C">C</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtPotentialCausesEdit" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlOCCEdit" AutoPostBack="false" runat="server" 
                                        Font-Names="Arial" Font-Size="8pt" >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtControlsPreventionEdit" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlDETEdit" AutoPostBack="false" runat="server" Font-Names="Arial" Font-Size="8pt" >
                                        <asp:ListItem Value="0">-</asp:ListItem>
                                        <asp:ListItem Enabled="True">1</asp:ListItem>
                                        <asp:ListItem Enabled="True">2</asp:ListItem>
                                        <asp:ListItem Enabled="True">3</asp:ListItem>
                                        <asp:ListItem Enabled="True">4</asp:ListItem>
                                        <asp:ListItem Enabled="True">5</asp:ListItem>
                                        <asp:ListItem Enabled="True">6</asp:ListItem>
                                        <asp:ListItem Enabled="True">7</asp:ListItem>
                                        <asp:ListItem Enabled="True">8</asp:ListItem>
                                        <asp:ListItem Enabled="True">9</asp:ListItem>
                                        <asp:ListItem Enabled="True">10</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtControlDetectionEdit" runat="server" Width="135px" Font-Names="Arial" 
                                    Font-Size="8pt"></asp:TextBox></td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="#405778"></asp:Label>
                                </td>
                                <td><asp:TextBox ID="txtValfromEdit" runat="server" Width="100px" Font-Names="Arial" 
                                    Font-Size="8pt" CssClass="tanggal"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtValtoEdit" runat="server" Width="100px" Font-Names="Arial" 
                                    Font-Size="8pt" CssClass="tanggal"></asp:TextBox></td> 
                                <td>
                                    <asp:HiddenField ID="hddIdEdit" runat="server" />
                                </td>                  
                              </tr>
                        </table>
              </dx:PopupControlContentControl>
          </ContentCollection>
              
          <FooterContentTemplate>
            <table>
                <tr>
                    <td style="width: 100%">
                    </td>
                    <td>
                        <dx:ASPxButton runat="server" ID="btnSaveFMEAEdit" ClientInstanceName="btnSaveFMEAEdit" Text="Save" AutoPostBack="false" OnClick="btnSaveFMEAEdit_Click">
                        </dx:ASPxButton>
                      </td>
                    <td style="width: 50px">
                    </td>
                    <td style="padding-left: 2px;">
                      <dx:ASPxButton runat="server" ID="btnCancelFMEAEdit" ClientInstanceName="btnCancelFMEAEdit" Text="Cancel" AutoPostBack="false">
                          <ClientSideEvents Click="btnCancelFMEAEdit_Click" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </FooterContentTemplate>

        </dx:ASPxPopupControl>

        

    </div> 
</div>
</asp:Content>
