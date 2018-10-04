<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonProductionProblem.aspx.cs" MasterPageFile="~/Main.master" Inherits="DotMercy.custom.Production.AndonProductionProblem" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
   Andon Production Problem
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
      <h2> Andon Production Problem</h2>
          <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="Id" OnRowInserting="ASPxGridView1_RowInserting" OnRowUpdating="ASPxGridView1_RowUpdating">
              <Columns>
                  <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
                  </dx:GridViewCommandColumn>
                  <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="1">
                      <EditFormSettings Visible="False" />
                  </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2">
                  </dx:GridViewDataTextColumn>
                  <dx:GridViewDataCheckColumn FieldName="IsActive" VisibleIndex="3">
                  </dx:GridViewDataCheckColumn>
                  <dx:GridViewDataDateColumn FieldName="CreatedDate" VisibleIndex="4" >
                         <EditFormSettings Visible="False" />
                  </dx:GridViewDataDateColumn>
                  <dx:GridViewDataTextColumn FieldName="CreatedBy" VisibleIndex="5">
                         <EditFormSettings Visible="False" />
                  </dx:GridViewDataTextColumn>
                  <dx:GridViewDataDateColumn FieldName="ModifiedDate" VisibleIndex="6" >
                         <EditFormSettings Visible="False" />
                  </dx:GridViewDataDateColumn>
                  <dx:GridViewDataTextColumn FieldName="ModifiedBy" VisibleIndex="7">
                         <EditFormSettings Visible="False" />
                  </dx:GridViewDataTextColumn>
              </Columns>
              <Settings ShowGroupPanel="True" />
              <SettingsSearchPanel Visible="True" />
          </dx:ASPxGridView>
          <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DotWebDb %>" DeleteCommand="DELETE FROM [AndonProductionProblem] WHERE [Id] = @Id" 
              InsertCommand="INSERT INTO [AndonProductionProblem] ([Name], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (@Name, @IsActive, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy)" SelectCommand="SELECT [Id], [Name], [IsActive], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy] FROM [AndonProductionProblem]" UpdateCommand="UPDATE [AndonProductionProblem] SET [Name] = @Name, [IsActive] = @IsActive, [CreatedDate] = @CreatedDate, [CreatedBy] = @CreatedBy, [ModifiedDate] = @ModifiedDate, [ModifiedBy] = @ModifiedBy WHERE [Id] = @Id">
              <DeleteParameters>
                  <asp:Parameter Name="Id" Type="Int32" />
              </DeleteParameters>
              <InsertParameters>
                  <asp:Parameter Name="Name" Type="String" />
                  <asp:Parameter Name="IsActive" Type="Boolean" />
                  <asp:Parameter Name="CreatedDate" Type="DateTime" />
                  <asp:Parameter Name="CreatedBy" Type="String" />
                  <asp:Parameter Name="ModifiedDate" Type="DateTime" />
                  <asp:Parameter Name="ModifiedBy" Type="String" />
              </InsertParameters>
              <UpdateParameters>
                  <asp:Parameter Name="Name" Type="String" />
                  <asp:Parameter Name="IsActive" Type="Boolean" />
                  <asp:Parameter Name="CreatedDate" Type="DateTime" />
                  <asp:Parameter Name="CreatedBy" Type="String" />
                  <asp:Parameter Name="ModifiedDate" Type="DateTime" />
                  <asp:Parameter Name="ModifiedBy" Type="String" />
                  <asp:Parameter Name="Id" Type="Int32" />
              </UpdateParameters>
          </asp:SqlDataSource>
    

   </asp:Content>
