<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="FMEA.aspx.cs" Inherits="Ebiz.WebForm.custom.Catalog.FMEA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modal-lg {
            width: 1350px;
        }

        .tblstyle {
            padding: 3px;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function CallPopUPAddFMEA() {
            PopUpAddFMEA.Show();
            console.log("Masuk Boy");
        }
        function CallPopUPRatingFMEA() {
            PopUpRatingFMEA.Show();
            console.log("Masuk Boy");
        }
        function CallPopUPEditFMEA() {
            PopUpEditFMEA.Show();
            console.log("Masuk Boy");
        }
    </script>
    <div class="container">
        <div class="row">
            <div class="col-md-12 col-lg-12">
                <div class="col-md-offset-1">
                    <h4 style="color: #0a61ad;" class="col-md-offset-5">FMEA</h4>
                    <table>
                        <tr>
                            <td>
                                 <dx:ASPxComboBox runat="server" ID="cmbTypeModel" AutoPostBack="true" DataSourceID="sdsGetTypeModel" ValueField="ModelType" TextField="ModelType" CssClass="dropdown-header" ClientInstanceName="cmbTypeModel">
                                </dx:ASPxComboBox>
                                
                            </td>
                            <td>
                                  <dx:ASPxComboBox runat="server" ID="cmbPM" AutoPostBack="true" DataSourceID="sdsGetPM" ValueField="PM" TextField="PM" CssClass="dropdown-header" ClientInstanceName="cmbPM">
                                 </dx:ASPxComboBox>
                                 
                            </td>
                            <td>
                                   <dx:ASPxComboBox runat="server" ID="cmbAssemblyModel" AutoPostBack="true" DataSourceID="sdsGetAssemblyModel" ValueField="AssemblyModel" TextField="AssemblyModel" CssClass="dropdown-header" ClientInstanceName="cmbAssemblyModel">
                                   </dx:ASPxComboBox>
                                   
                            </td>
                            <td>
                                    <dx:ASPxComboBox runat="server" ID="cmbStations" AutoPostBack="true" DataSourceID="sdsGetStations" ValueField="StationName" TextField="StationName" CssClass="dropdown-header" ClientInstanceName="cmbStations">
                                    </dx:ASPxComboBox>

                            </td>
                            <td>
                                    <dx:ASPxComboBox runat="server" ID="cmbGWISProcesses" AutoPostBack="true" DataSourceID="sdsGetGWISProcesses" ValueField="Id" TextField="GP" CssClass="dropdown-header" ClientInstanceName="cmbGWISProcesses">
                                    </dx:ASPxComboBox>    
                            </td>
                            <td>
                                    <dx:ASPxTextBox ID="txtRPN" runat="server" Text="100" CssClass="dropdown-header">
                                    </dx:ASPxTextBox>
                            </td>
                            <td>
                                    <dx:ASPxButton ID="btnRPN" runat="server" Text="RPN" OnClick="btnRPN_Click">
                                    </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <dx:LayoutItemNestedControlContainer runat="server">
                 <br />
                   <div id="divFMEA" runat="server"></div>
                </dx:LayoutItemNestedControlContainer>
            </div>
        </div>

        <style>
            .table1 {
                background: #e8e8e8;
                color: #405778;
            }

            .table2 {
                background: #b0c4de;
            }

            table th {
                text-align: center;
            }
        </style>
        <div class="row">
            <table class="table table-bordered">
                <tr>
                    <th rowspan="2" class="table1">No.</th>
                    <th rowspan="2" class="table1">Process Function</th>
                    <th rowspan="2" class="table1">Potential Failuer Mode</th>
                    <th rowspan="2" class="table1">Potential Failure Effects</th>
                    <th rowspan="2" style="background: yellow; color: #405778;">SEV</th>
                    <th rowspan="2" style="background: white; color: #405778;">Class</th>
                    <th rowspan="2" class="table1">Potential Cause</th>
                    <th colspan="4" class="table1">Current Process</th>
                    <th rowspan="2" style="background: #dc143c;">RPN</th>
                    <th rowspan="2" class="table2">Recommended Action</th>
                    <th rowspan="2" class="table2">Responsibility</th>
                    <th colspan="5" class="table2">Action Results</th>
                </tr>
                <tr>
                    <th class="table1">Control Prevention</th>
                    <th style="background: yellow; color: #405778;">OCC</th>
                    <th class="table1">Control Detection</th>
                    <th style="background: yellow; color: #405778;">DET</th>
                    <th class="table2">Action Taken</th>
                    <th class="table2">SEV</th>
                    <th class="table2">OCC</th>
                    <th class="table2">DET</th>
                    <th style="background: #d2691e;">RPN</th>
                </tr>
                <tr>
                    <td>1</td>
                    <td>AL 0201 L.0010 ENGRAVING CHASSIS</td>
                    <td>test</td>
                    <td>test</td>
                    <td>1</td>
                    <td>A</td>
                    <td>test</td>
                    <td>test</td>
                    <td>1</td>
                    <td>test</td>
                    <td>1</td>
                    <td>1</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td><button id="btnEditFMEA" onclick='CallPopUPEditFMEA()'>Edit</button><br />
                        <asp:Button Text="Delete" runat="server" Style="color: #0a61ad; background: #f3f3f3; padding: 3px; border: 1px solid grey; text-decoration: none;" /></td>
                </tr>
            </table>
        </div>
        <br />
    </div>

    <!-- Modal Edit FMEA -->
    <dx:ASPxPopupControl ID="PopUpEditFMEA" runat="server"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpEditFMEA"
                        HeaderText="" AllowDragging="false" ScrollBars="Auto" Height="500" Width="1350" Modal="True" PopupAnimationType="Auto">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                               <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" Width="100%">
                                    <Items>
                                        <dx:LayoutItem Caption="">
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                   
                    <div class="row">
                        <div class="col-md-11">
                            <h4 class="modal-title" style="color: #0a61ad;">Edit FMEA<br />
                                <font size="2">AL.0201.L.0010 ENGRAVING CHASSIS</font>
                                <br />
                            </h4>
                        </div>
                        <div class="col-md-1">
                            <a href="#" data-dismiss="modal">close</a>
                        </div>
                    </div>
                <div class="modal-body">
                    <div style="background: #f3f3f3;">
                        <table class="tblmodal">
                            <thead>
                                <tr style="color: #0a61ad;">
                                    <th class="tblstyle">What do you see in the factory that tells you the cause has occurred?</th>
                                    <th class="tblstyle">What is the impact on the Key Output Variables (Customer Requirements)?</th>
                                    <th class="tblstyle">How severe is the effect to the cust. or next process?</th>
                                    <th class="tblstyle">Safety Classification</th>
                                    <th class="tblstyle">What are the potential ways can cause the failure mode?</th>
                                    <th class="tblstyle">How often does cause or failure modes occur?</th>
                                    <th class="tblstyle">What are the existing controls and procedures (Inspection and test) that preventive the cause or the Failure Mode?</label></th>
                                    <th class="tblstyle">How well can you detect cause?</th>
                                    <th class="tblstyle">What are the actions for reducing the occurance of the Cause, or improving detection?</th>
                                    <th colspan="3"><button id="btnRatingFMEA2" onclick='CallPopUPRatingFMEA()'>Open Rating Table</button></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr style="border-top: 1px solid #0a61ad; border-bottom: 1px solid #0a61ad;">
                                    <th class="tblstyle">Potential Failure Mode</th>
                                    <th class="tblstyle">Potential Failure Effects</th>
                                    <th class="tblstyle">SEV</th>
                                    <th class="tblstyle">Class</th>
                                    <th class="tblstyle">Potential Causes</th>
                                    <th class="tblstyle">OCC</th>
                                    <th class="tblstyle">Control Prevention</th>
                                    <th class="tblstyle">DET</th>
                                    <th class="tblstyle">Controls Detection</th>
                                    <th style="background: red;">RPN</th>
                                    <th class="tblstyle">Valfrom</th>
                                    <th class="tblstyle">Valto</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle"></td>
                                    <td class="tblstyle">
                                        <input type="text" size="15" /></td>
                                    <td class="tblstyle">
                                        <input type="text" size="15" /></td>
                                    <td width="5%">
                                        <a href="#" style="color: #0a61ad; background: #f3f3f3; padding: 3px; border: 1px solid grey; text-decoration: none;">
                                            <strong>Save</strong>
                                        </a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-offset-3">
                            <h2 style="margin: auto; width: 35%; color: #4e6381;"><b>Evaluation</b></h2>
                            <br />
                            <table class="tblmodal" style="background: #f3f3f3;">
                                <thead>
                                    <tr style="border-top: 1px solid #0a61ad; border-bottom: 1px solid #0a61ad;">
                                        <th class="tblstyle">Actions Recommended</th>
                                        <th class="tblstyle">Responsible</th>
                                        <th class="tblstyle">Due Date</th>
                                        <th class="tblstyle">Actions taken</th>
                                        <th class="tblstyle">SEC</th>
                                        <th class="tblstyle">OCC</th>
                                        <th class="tblstyle">DET</th>
                                        <th style="background: red;">RPN</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="tblstyle">
                                            <input type="text" value="sd" style="width: 100%;"></td>
                                        <td class="tblstyle">
                                            <input type="text" value="aasd" style="width: 100%;"></td>
                                        <td class="tblstyle">
                                            <input type="date" value="2018-04-03" style="width: 100%;"><br>
                                            <i>ex. 20120325 (YYYMMDD)</i></td>
                                        <td class="tblstyle">
                                            <input type="text" value="test" style="width: 100%;"></td>
                                        <td class="tblstyle">
                                            <select style="width: 100%;">
                                                <option>6</option>
                                            </select></td>
                                        <td class="tblstyle">
                                            <select style="width: 100%;">
                                                <option>2</option>
                                            </select></td>
                                        <td class="tblstyle">
                                            <select style="width: 100%;">
                                                <option>2</option>
                                            </select></td>
                                        <td class="tblstyle">24</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:ASPxFormLayout>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                        <dx:ASPxPopupControl ID="PopUpAddFMEA" runat="server"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpAddFMEA"
                        HeaderText="" AllowDragging="false" ScrollBars="Auto" Height="400" Width="1350" Modal="True" PopupAnimationType="Auto">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                               <dx:ASPxFormLayout ID="frmPartSelection" runat="server" Width="100%">
                                    <Items>
                                        <dx:LayoutItem Caption="">
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                   
                    <div class="row">
                        <div class="col-md-11">
                            <h4 class="modal-title" style="color: #0a61ad;">Add FMEA<br />
                                AL.0201.L.0010 ENGRAVING CHASSIS<br />
                                Part A : A 100 000 00 00 - DAMMY
                            </h4>
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <div style="background: #f3f3f3;">
                        <table class="tblmodal">
                            <thead>
                                <tr style="color: #0a61ad;">
                                    <th class="tblstyle">What do you see in the factory that tells you the cause has occurred?</th>
                                    <th class="tblstyle">What is the impact on the Key Output Variables (Customer Requirements)?</th>
                                    <th class="tblstyle">How severe is the effect to the cust. or next process?</th>
                                    <th class="tblstyle">Safety Classification</th>
                                    <th class="tblstyle">What are the potential ways can cause the failure mode?</th>
                                    <th class="tblstyle">How often does cause or failure modes occur?</th>
                                    <th class="tblstyle">What are the existing controls and procedures (Inspection and test) that preventive the cause or the Failure Mode?</label></th>
                                    <th class="tblstyle">How well can you detect cause?</th>
                                    <th class="tblstyle">What are the actions for reducing the occurance of the Cause, or improving detection?</th>
                                    <th colspan="3"><button id="btnRatingFMEA" onclick='CallPopUPRatingFMEA()'>Open Rating Table</button></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr style="border-top: 1px solid #0a61ad; border-bottom: 1px solid #0a61ad;">
                                    <th class="tblstyle">Potential Failure Mode</th>
                                    <th class="tblstyle">Potential Failure Effects</th>
                                    <th class="tblstyle">SEV</th>
                                    <th class="tblstyle">Class</th>
                                    <th class="tblstyle">Potential Causes</th>
                                    <th class="tblstyle">OCC</th>
                                    <th class="tblstyle">Control Prevention</th>
                                    <th class="tblstyle">DET</th>
                                    <th class="tblstyle">Controls Detection</th>
                                    <th style="background: red;">RPN</th>
                                    <th class="tblstyle">Valfrom</th>
                                    <th class="tblstyle">Valto</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td class="tblstyle">
                                          <dx:ASPxTextBox ID="txtPotentialFailureMode" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    </td>
                                    <td class="tblstyle">
                                        <dx:ASPxTextBox ID="txtPotentialFailureEffects" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    <td class="tblstyle">
                                         <dx:ASPxComboBox runat="server" ID="cmbSEV" AutoPostBack="true" ValueField="ModelType" TextField="ModelType" CssClass="dropdown-header">
                                              <Items>
                                                    <dx:ListEditItem Selected="True" Text="A" Value="1"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="B" Value="2"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="C" Value="3"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="D" Value="4"></dx:ListEditItem>
                                                </Items>
                                         </dx:ASPxComboBox>
                                    </td>
                                    <td class="tblstyle">
                                         <dx:ASPxComboBox runat="server" ID="cmbClass" AutoPostBack="true"  ValueField="ModelType" TextField="ModelType" CssClass="dropdown-header">
                                              <Items>
                                                    <dx:ListEditItem Selected="True" Text="A" Value="1"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="B" Value="2"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="C" Value="3"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="D" Value="4"></dx:ListEditItem>
                                                </Items>
                                         </dx:ASPxComboBox>
                                    </td>
                                    <td class="tblstyle">
                                       <dx:ASPxTextBox ID="txtPotentialCauses" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    <td class="tblstyle">
                                         <dx:ASPxComboBox runat="server" ID="cmbOCC" AutoPostBack="true" ValueField="ModelType" TextField="ModelType" CssClass="dropdown-header">
                                                <Items>
                                                    <dx:ListEditItem Selected="True" Text="A" Value="1"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="B" Value="2"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="C" Value="3"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="D" Value="4"></dx:ListEditItem>
                                                </Items>
                                         </dx:ASPxComboBox>
                                    </td>
                                    <td class="tblstyle">
                                        <dx:ASPxTextBox ID="txtControlPrevention" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    <td class="tblstyle">
                                         <dx:ASPxComboBox runat="server" ID="cmbDET" AutoPostBack="true" ValueField="ModelType" TextField="ModelType" CssClass="dropdown-header">
                                                <Items>
                                                    <dx:ListEditItem Selected="True" Text="A" Value="1"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="B" Value="2"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="C" Value="3"></dx:ListEditItem>
                                                    <dx:ListEditItem Text="D" Value="4"></dx:ListEditItem>
                                                </Items>
                                         </dx:ASPxComboBox>
                                    </td>
                                    <td class="tblstyle">
                                        <dx:ASPxTextBox ID="txtControlsDetection" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    <td class="tblstyle"></td>
                                    <td class="tblstyle">
                                        <dx:ASPxTextBox ID="txtValfrom" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    <td class="tblstyle">
                                        <dx:ASPxTextBox ID="txtValto" runat="server" Enabled="true" DisplayFormatString=""></dx:ASPxTextBox>
                                    <td width="10%">
                                        <dx:ASPxButton ID="btnSave" runat="server" Enabled="true" Text="Save"></dx:ASPxButton>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                                    </div>

                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:ASPxFormLayout>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
      <!-- Modal Add FMEA-->

    <%--ubah jadi popup--%>
    <div class="modal" id="myModal" role="dialog" style="overflow-y: auto; overflow-x: auto;">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <div class="col-md-11">
                            <h4 class="modal-title" style="color: #0a61ad;">Add FMEA<br />
                                AL.0201.L.0010 ENGRAVING CHASSIS<br />
                                Part A : A 100 000 00 00 - DAMMY
                            </h4>
                        </div>
                        <div class="col-md-1">
                            
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <div style="background: #f3f3f3;">
                        <table class="tblmodal">
                            <thead>
                                <tr style="color: #0a61ad;">
                                    <th class="tblstyle">What do you see in the factory that tells you the cause has occurred?</th>
                                    <th class="tblstyle">What is the impact on the Key Output Variables (Customer Requirements)?</th>
                                    <th class="tblstyle">How severe is the effect to the cust. or next process?</th>
                                    <th class="tblstyle">Safety Classification</th>
                                    <th class="tblstyle">What are the potential ways can cause the failure mode?</th>
                                    <th class="tblstyle">How often does cause or failure modes occur?</th>
                                    <th class="tblstyle">What are the existing controls and procedures (Inspection and test) that preventive the cause or the Failure Mode?</label></th>
                                    <th class="tblstyle">How well can you detect cause?</th>
                                    <th class="tblstyle">What are the actions for reducing the occurance of the Cause, or improving detection?</th>
                                    <th colspan="3"><button id="btnRatingFMEA3" onclick='CallPopUPRatingFMEA()'>Open Rating Table</button></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr style="border-top: 1px solid #0a61ad; border-bottom: 1px solid #0a61ad;">
                                    <th class="tblstyle">Potential Failure Mode</th>
                                    <th class="tblstyle">Potential Failure Effects</th>
                                    <th class="tblstyle">SEV</th>
                                    <th class="tblstyle">Class</th>
                                    <th class="tblstyle">Potential Causes</th>
                                    <th class="tblstyle">OCC</th>
                                    <th class="tblstyle">Control Prevention</th>
                                    <th class="tblstyle">DET</th>
                                    <th class="tblstyle">Controls Detection</th>
                                    <th style="background: red;">RPN</th>
                                    <th class="tblstyle">Valfrom</th>
                                    <th class="tblstyle">Valto</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle">
                                        <select>
                                            <option>-</option>
                                        </select>
                                    </td>
                                    <td class="tblstyle">
                                        <input type="text" /></td>
                                    <td class="tblstyle"></td>
                                    <td class="tblstyle">
                                        <input type="text" size="15" /></td>
                                    <td class="tblstyle">
                                        <input type="text" size="15" /></td>
                                    <td width="10%">
                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Enabled="true" Text="Save"></dx:ASPxButton>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- Modal Rating Table -->
     <dx:ASPxPopupControl ID="PopUpRatingFMEA" runat="server"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopUpRatingFMEA"
                        HeaderText="" AllowDragging="false" ScrollBars="Auto" Height="600" Width="1350" Modal="True" PopupAnimationType="Auto">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                               <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" Width="100%">
                                    <Items>
                                        <dx:LayoutItem Caption="">
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                   
            <!-- Modal content-->
           
                    <h4 class="modal-title" style="color: #405778;">Rating Table of Process-FMEA</h4>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr style="background: #405778; color: white;">
                            <th>RANK</th>
                            <th colspan="3">SEVERITY</th>
                            <th colspan="2">OCCURANCE</th>
                            <th colspan="2">DETECTION</th>
                        </tr>
                        <tr>
                            <td><b>Very High</b></td>
                            <td style="width: 15px;">10 9</td>
                            <td style="width: 455px;"><i><u>Product:</u></i><br />
                                safety risk, non-compliance with statutory provisions
                                <br />
                                - sudden death
                                <br />
                                - sudden death with warning</td>
                            <td>
                                <i><u>Process  (VDA 2. Ausg.):</u></i>
                                <br />
                                > very severe fault; wich affects safety and/or breaches the compliance with statutory provisions
              <td style="width: 15px;">10 9</td>
                                <td style="width: 200px;">fault cause appears very often,
                                    <br />
                                    useless, inapplicable process.</td>
                                <td style="width: 15px;">10 9</td>
                                <td>fault cause is unlikely to be detected, the fault cause can not be examined.
                                    <br />
                                    >no inspection</td>
                        </tr>
                        <tr>
                            <td><b>High</b></td>
                            <td style="width: 15px;">7 8</td>
                            <td><i><u>Product:</u></i><br />
                                functionality of vehicle severely limited; break down or immediate shop attendance
                                <br />
                                absolutely necessary, limited function of important sub-systems</td>
                            <td>
                                <i><u>Process  (VDA 2. Ausg.):</u></i>
                                <br />
                                > severely delayed delivery
                  <br />
                  <br />
                                > high tool wear/-damage
                  <br />
                                > high cost overrun
                  <br />
                                > scrapping contingent high
                            </td>
                            <td style="width: 15px;">7 8</td>
                            <td>fault cause appears repeatedly,
                  <br />
                                inaccurate process.

                            </td>
                            <td style="width: 15px;">7 8</td>
                            <td>fault cause is unlikely to be detected, probably hidden fault cause, unreliable examination
                  <br />
                                >visual inspection

                            </td>
                        </tr>
                        <tr>
                            <td><b>Medium</b></td>
                            <td style="width: 15px;">4 5 6</td>
                            <td><i><u>Product:</u></i><br />
                                functionality of vehicle limited; immediate shop attendance not necessary; limited
                                <br />
                                function of important control and comfort systems</td>
                            <td>
                                <i><u>Process  (VDA 2. Ausg.):</u></i>
                                <br />
                                > delayed delivery
                  <br />
                                > moderate ratio of rework, process interference
                  <br />
                                > moderate tool wear/-damage
                  <br />
                                > moderate cost overrun
                  <br />
                                > moderate scrapping contingent
                            </td>
                            <td style="width: 15px;">4 5 6</td>
                            <td>fault cause appears occasionaly,
                  <br />
                                lessincacurate process.

                            </td>
                            <td style="width: 15px;">4 5 6</td>
                            <td>fault cause appears occasionaly,
                  <br />
                                reliable.
                  <br />
                                sampling inspection
                            </td>
                        </tr>
                        <tr>
                            <td><b>LOW</b></td>
                            <td style="width: 15px;">2 3</td>
                            <td><i><u>Product:</u></i><br />
                                functionality of vehicle slightly limited reduce performance or appearance notify by most customer</td>
                            <td>
                                <i><u>Process  (VDA 2. Ausg.):</u></i>
                                <br />
                                > slight ratio of rework
                  <br />
                                > slight process interference
                  <br />
                                > severely delayed delivery
                  <br />
                                > slight cost overrun
                  <br />
                                > slight scrapping contingent
                            </td>
                            <td style="width: 15px;">2 3</td>
                            <td>minor appearance of fault cause,
                  <br />
                                accurate process

                            </td>
                            <td style="width: 15px;">2 3</td>
                            <td>fault cause is very likely to be detected, examanations are reliable, e.g various examinations independent of one another.
                  <br />
                                100% check
                            </td>
                        </tr>
                        <tr>
                            <td><b>VERY LOW</b></td>
                            <td style="width: 15px;">1</td>
                            <td><i><u>Product:</u></i><br />
                                functionality of vehicle slightly limited reduce performance or appearance notify only by qualified personnel / expert</td>
                            <td>
                                <i><u>Process  (VDA 2. Ausg.):</u></i>
                                <br />
                                > very low; acceptable cost overrun
                            </td>
                            <td style="width: 15px;">1</td>
                            <td>chance of fault cause appearance is unlikely

                            </td>
                            <td style="width: 15px;">1</td>
                            <td>fault cause is certainly going to be detected
                  <br />
                                Pokayoke/jidoka
                            </td>
                        </tr>
                    </table>
                                                    
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:ASPxFormLayout>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
    <script type="text/javascript">
        $('.modal-child').on('show.bs.modal', function () {
            var modalParent = $(this).attr('data-modal-parent');
            $(modalParent).css('opacity', 0);
        });

        $('.modal-child').on('hidden.bs.modal', function () {
            var modalParent = $(this).attr('data-modal-parent');
            $(modalParent).css('opacity', 1);
        });
    </script>



    <asp:SqlDataSource ID="sdsGetTypeModel" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select t3.Name + ' ' + t2.Model as ModelType 
 from AssemblyModels t1
inner join CatalogModels t2 on t1.CatalogModelId = t2.Id
inner join AssemblyTypes t3 on t1.AssemblyTypeId = t3.Id"></asp:SqlDataSource>

     <asp:SqlDataSource ID="sdsGetPM" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select distinct 
SUBSTRING(convert(varchar(6),t1.ValidFrom, 112), 1, 4)  + '-' + SUBSTRING(convert(varchar(6),t1.ValidFrom, 112), 5, 6)  as PM,
t4.Name + ' ' + t5.Model as ModelType
from GWISProcesses t1
inner join GWISParts t2 on t1.Id = t2.ProcessId
inner join AssemblySections t3 on t1.AssemblySectionId = t3.Id
inner join AssemblyTypes t4 on t1.AssemblyTypeId = t4.Id
inner join CatalogModels t5 on t2.CatalogModelId = t5.Id
where t4.Name + ' ' + t5.Model = @ModelType 
order by SUBSTRING(convert(varchar(6),t1.ValidFrom, 112), 1, 4)  + '-' + SUBSTRING(convert(varchar(6),t1.ValidFrom, 112), 5, 6) desc">
         <SelectParameters>
            <asp:Parameter Name="ModelType" Type="String"/>
         </SelectParameters>
     </asp:SqlDataSource>

     <asp:SqlDataSource ID="sdsGetAssemblyModel" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select distinct convert(varchar(10), t1.AssemblySectionName ) + ' ' + convert(varchar(10), t4.Model) as AssemblyModel
from AssemblySections t1
inner join GWISProcesses t2 on t1.Id = t2.AssemblySectionId
inner join GWISParts t3 on t2.Id = t3.ProcessId
inner join CatalogModels t4 on t3.CatalogModelId = t4.Id
where SUBSTRING(convert(varchar(6),t2.ValidFrom, 112), 1, 4)  + '-' + SUBSTRING(convert(varchar(6),t2.ValidFrom, 112), 5, 6) = @PM">
          <SelectParameters>
            <asp:Parameter Name="PM" Type="String"/>
         </SelectParameters>
     </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsGetStations" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select distinct t1.StationName from Stations t1 
inner join GWISProcesses t2 on t1.Id = t2.StationId
inner join GWISParts t3 on t2.Id = t3.ProcessId
inner join CatalogModels t4 on t3.CatalogModelId = t4.Id
inner join AssemblySections t5 on t5.Id = t2.AssemblySectionId
where convert(varchar(10), t5.AssemblySectionName ) + ' ' + convert(varchar(10), t4.Model) = @Stations 
order by t1.StationName asc">
         <SelectParameters>
            <asp:Parameter Name="Stations" Type="String"/>
         </SelectParameters>
    </asp:SqlDataSource>

     <asp:SqlDataSource ID="sdsGetGWISProcesses" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="select t1.Id as Id, t1.ProcessNo + '   ' + t1.ProcessName as GP from GWISProcesses t1
inner join  Stations t2 on t1.StationId = t2.Id where t2.StationName = @StationName">
         <SelectParameters>
            <asp:Parameter Name="StationName" Type="String"/>
         </SelectParameters>
     </asp:SqlDataSource>


</asp:Content>
