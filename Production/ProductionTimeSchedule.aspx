<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionTimeSchedule.aspx.cs" MasterPageFile="~/Main.master" Inherits="DotMercy.custom.Production.ProductionTimeSchedule" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Production Time Schedule
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
      <h2> Production Time Schedule</h2>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" OnCustomColumnDisplayText="gridView_CustomColumnDisplayText" OnCellEditorInitialize="gridView_CellEditorInitialize" OnParseValue="gridView_ParseValue" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="id">
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="false" VisibleIndex="1">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="DayCode" Caption="Day" VisibleIndex="2">
                <PropertiesComboBox>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    <Items>
                        <dx:ListEditItem Text="Mon – Tue" Value="1" />
                        <dx:ListEditItem Text="Friday" Value="2" />
                        <dx:ListEditItem Text="Overide" Value="3" />
                    </Items>
                </PropertiesComboBox>

            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="AssemblyTypeID" Caption="Assembly" VisibleIndex="3">
                <PropertiesComboBox DataSourceID="sdsAssembly" TextField="Name" ValueField="Id">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTimeEditColumn FieldName="InTime" VisibleIndex="4">
                <PropertiesTimeEdit DisplayFormatInEditMode="True" DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTimeEdit>
            </dx:GridViewDataTimeEditColumn>
            <dx:GridViewDataTimeEditColumn FieldName="StartTime" VisibleIndex="5">
                <PropertiesTimeEdit DisplayFormatInEditMode="True" DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTimeEdit>
            </dx:GridViewDataTimeEditColumn>

            <dx:GridViewDataComboBoxColumn FieldName="BreakSequence" VisibleIndex="6">
                <PropertiesComboBox>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    <Items>
                        <dx:ListEditItem Text="1" Value="1" />
                        <dx:ListEditItem Text="2" Value="2" />
                        <dx:ListEditItem Text="3" Value="3" />
                    </Items>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTimeEditColumn FieldName="BreakStart" VisibleIndex="7">
                <PropertiesTimeEdit DisplayFormatInEditMode="True" DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTimeEdit>
            </dx:GridViewDataTimeEditColumn>
            <dx:GridViewDataTimeEditColumn FieldName="BreakEnd" VisibleIndex="8">
                <PropertiesTimeEdit DisplayFormatInEditMode="True" DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTimeEdit>
            </dx:GridViewDataTimeEditColumn>
            <dx:GridViewDataTimeEditColumn FieldName="StopTime" VisibleIndex="9">
                <PropertiesTimeEdit DisplayFormatInEditMode="True" DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTimeEdit>
            </dx:GridViewDataTimeEditColumn>
            <dx:GridViewDataTimeEditColumn FieldName="EndTime" VisibleIndex="10">
                <PropertiesTimeEdit DisplayFormatInEditMode="True" DisplayFormatString="HH:mm" EditFormat="Time" EditFormatString="HH:mm">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTimeEdit>
            </dx:GridViewDataTimeEditColumn>
                 
        </Columns>
          <Settings ShowGroupPanel="True" />
              <SettingsSearchPanel Visible="True" />
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DotWebDb %>"
        DeleteCommand="DELETE FROM [ProductionTimeSchedule] WHERE [id] = @id"
        InsertCommand="INSERT INTO [ProductionTimeSchedule] ([DayCode], [AssemblyTypeID], [InTime],[StartTime], [BreakSequence], [BreakStart], [BreakEnd], [StopTime], [EndTime]) 
        VALUES (@DayCode, @AssemblyTypeID,cast(@InTime as time) ,cast(@StartTime as time) , @BreakSequence,cast(@BreakStart as time) ,cast(@BreakEnd as time) , cast(@StopTime as time),cast(@EndTime as time) )"
        SelectCommand="SELECT [id], [DayCode], [AssemblyTypeID],
                        IIF(ISDATE([InTime]) = 1,convert(datetime,[InTime]),'') [InTime],
                        IIF(ISDATE([StartTime]) = 1,convert(datetime,[StartTime]),'') [StartTime] ,
                        [BreakSequence],
                        IIF(ISDATE([BreakStart]) = 1,convert(datetime,[BreakStart]),'') [BreakStart],
                        IIF(ISDATE([BreakEnd]) = 1,convert(datetime,[BreakEnd]),'') [BreakEnd] ,
                        IIF(ISDATE([StopTime]) = 1,convert(datetime,[StopTime]),'') [StopTime],
                        IIF(ISDATE([EndTime]) = 1,convert(datetime,[EndTime]),'') [EndTime]
                        FROM [ProductionTimeSchedule]
                        "
        UpdateCommand="UPDATE [ProductionTimeSchedule] SET [DayCode] = @DayCode, [AssemblyTypeID] = @AssemblyTypeID, [InTime] = cast(@InTime as time), [StartTime] = cast(@StartTime as time), [BreakSequence] = @BreakSequence, [BreakStart] = cast(@BreakStart as time), [BreakEnd] = cast(@BreakEnd as time), [StopTime] = cast(@StopTime as time), [EndTime] =  cast(@EndTime as time) WHERE [id] = @id">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="DayCode" Type="Int32" />
            <asp:Parameter Name="AssemblyTypeID" Type="Int32" />
            <asp:Parameter Name="InTime" Type="String" />
            <asp:Parameter Name="StartTime" Type="String" />
            <asp:Parameter Name="BreakSequence" Type="Int32" />
            <asp:Parameter Name="BreakStart" Type="String" />
            <asp:Parameter Name="BreakEnd" Type="String" />
            <asp:Parameter Name="StopTime" Type="String" />
            <asp:Parameter Name="EndTime" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="DayCode" Type="Int32" />
            <asp:Parameter Name="AssemblyTypeID" Type="Int32" />
            <asp:Parameter Name="InTime" Type="String" />
            <asp:Parameter Name="StartTime" Type="String" />
            <asp:Parameter Name="BreakSequence" Type="Int32" />
            <asp:Parameter Name="BreakStart" Type="String" />
            <asp:Parameter Name="BreakEnd" Type="String" />
            <asp:Parameter Name="StopTime" Type="String" />
            <asp:Parameter Name="EndTime" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sdsAssembly" runat="server"
        ConnectionString="<%$ ConnectionStrings:AppDb %>"
        SelectCommand="SELECT [Id],[Name] FROM [dbo].[AssemblyTypes]"></asp:SqlDataSource>
</asp:Content>

