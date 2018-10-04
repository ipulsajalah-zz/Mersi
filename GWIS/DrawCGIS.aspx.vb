' ****** Draw CGIS ******
' ****** Author : Nastio Diaz ******
' ****** diaz.cmp@gmail.com ******
' ****** 18/06/2013 ******

Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.IO
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Partial Class report_drawCGIS
    Inherits System.Web.UI.Page
    Dim lcon As New SqlConnection(ConfigurationManager.ConnectionStrings("AppDb").ToString)
    Dim Point As Point = New Size(20, 100)
    Dim adp As SqlDataAdapter
    Dim byteOfImage As Byte()
    Dim dt As New DataTable
    Dim dr As SqlDataReader
    Dim com As SqlCommand

    Protected Sub report_drawCGIS_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        lcon = CType(Ebiz.Scaffolding.Utils.ConnectionHelper.GetConnection(), SqlConnection)

        If Not Page.IsPostBack Then
            'setRepeaterImage()
            If Not String.IsNullOrEmpty(Request.QueryString("ImageID")) Then
                getEditImage(Request.QueryString("ImageID"))
                setRepeaterAnnot(Request.QueryString("ImageID"))
            End If
        End If
    End Sub

    'Private Sub setRepeaterImage()
    '    adp = New SqlDataAdapter("SELECT * FROM Tbl_LKW_Picture", lcon)
    '    Try
    '        adp.Fill(dt)
    '        Repeater_Image.DataSource = dt
    '        Repeater_Image.DataBind()
    '    Catch ex As Exception
    '        CreateMessageAlert("error", "An error occured when setting repeater image. Loc : setRepeaterImage")
    '    End Try
    'End Sub

    Private Sub setRepeaterAnnot(ByVal ImageID As String)
        adp = New SqlDataAdapter("SELECT id, type, color, pos_left, pos_top, edit_date, editor, remarks, (pos_left + width) AS x2, (pos_top + height) AS y2, CASE type WHEN '1' THEN 'Arrow' WHEN '2' THEN 'Rectangle' WHEN '3' THEN 'Ellipse' END AS strType FROM GWISAnnotations WHERE PictureId = '" & ImageID & "' ORDER BY type", lcon)
        Try
            dt.Clear()
            adp.Fill(dt)
            Repeater_Annot.DataSource = dt
            Repeater_Annot.DataBind()
        Catch ex As Exception
            CreateMessageAlert("error", "An error occured when setting repeater annot. Loc : setRepeaterAnnot")
        End Try
    End Sub

    Private Sub getEditImage(ByVal ImageID As String)
        Image_Edit.ImageUrl = "DisplayCGIS.aspx?ImageID=" & ImageID & "&state=" & Request.QueryString("state") & "&timestamp=" & Guid.NewGuid.ToString()
    End Sub

    Public Function BmpToBytes_MemStream(ByVal bmp As Bitmap) As Byte()

        Dim MS As New MemoryStream
        ' Save to memory using the Jpeg format
        bmp.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg)

        'read to end
        Dim bmpBytes As Byte() = MS.GetBuffer()
        bmp.Dispose()
        MS.Close()

        Return bmpBytes
    End Function

    Protected Sub Button_Save_Click(sender As Object, e As System.EventArgs) Handles Button_Save.Click
        Dim arr1() As String = Split(TextBox_Start.Text, ",")
        Dim arr2() As String = Split(TextBox_End.Text, ",")
        Dim pos_top, pos_left, width, height As Integer
        pos_top = arr1(1)
        pos_left = arr1(0)
        height = arr2(1) - arr1(1)
        width = arr2(0) - arr1(0)

        com = New SqlCommand("INSERT INTO GWISAnnotations(PictureId, pos_top, pos_left, width, height, upload_date, uploader, edit_date, editor, remarks, type, color) VALUES(@id_image, @pos_top, @pos_left, @width, @height, @upload_date, @uploader, @edit_date, @editor, @remarks, @type, @color)", lcon)
        com.Parameters.Add("@id_image", SqlDbType.Int).Value = Request.QueryString("ImageID")
        com.Parameters.Add("@pos_top", SqlDbType.Int).Value = pos_top
        com.Parameters.Add("@pos_left", SqlDbType.Int).Value = pos_left
        com.Parameters.Add("@width", SqlDbType.Int).Value = width
        com.Parameters.Add("@height", SqlDbType.Int).Value = height
        com.Parameters.Add("@upload_date", SqlDbType.Date).Value = Date.Today
        com.Parameters.Add("@uploader", SqlDbType.VarChar, 50).Value = HttpContext.Current.User.Identity.Name
        com.Parameters.Add("@edit_Date", SqlDbType.Date).Value = Date.Today
        com.Parameters.Add("@editor", SqlDbType.VarChar, 50).Value = HttpContext.Current.User.Identity.Name
        com.Parameters.Add("@remarks", SqlDbType.VarChar, 50).Value = TextBox_Remarks.Text
        com.Parameters.Add("@type", SqlDbType.Int).Value = DropDownList_Type.SelectedValue
        com.Parameters.Add("@color", SqlDbType.VarChar, 50).Value = DropDownList_Color.SelectedValue
        Try
            lcon.Open()
            com.ExecuteNonQuery()
            CreateMessageAlert("Success", "Save data success.")
            setRepeaterAnnot(Request.QueryString("ImageID"))
            getEditImage(Request.QueryString("ImageID"))
            TextBox_Start.Text = String.Empty
            TextBox_End.Text = String.Empty
        Catch ex As Exception
            CreateMessageAlert("Error", "Save data failed. Loc : Button_Save_Click")
        Finally
            lcon.Close()
        End Try

    End Sub

    Public Shared Sub CreateMessageAlert(ByVal type As String, ByVal message As String)
        Dim guidKey As Guid = Guid.NewGuid()
        Dim pg As Page = HttpContext.Current.Handler
        Dim strScript As String = "showNotification({" &
                                        "type : """ & type & """," &
                                        "message: """ & message & """," &
                                        "autoClose: true," &
                                        "duration: 3" &
                                    "});"
        pg.ClientScript.RegisterStartupScript(pg.GetType(), guidKey.ToString(), strScript, True)
    End Sub

    'Protected Sub Button_Upload_Click(sender As Object, e As System.EventArgs) Handles Button_Upload.Click
    '    If FileUpload_Image.HasFile Then
    '        Dim imgbyte(FileUpload_Image.PostedFile.ContentLength) As Byte
    '        FileUpload_Image.PostedFile.InputStream.Read(imgbyte, 0, FileUpload_Image.PostedFile.ContentLength)
    '        com = New SqlCommand("INSERT INTO WI_Image(image, upload_date, uploader, edit_date, editor, active, remarks) VALUES(@image, @upload_date, @uploader, @edit_Date, @editor, @active, @remarks)", lcon)
    '        com.Parameters.Add("@image", SqlDbType.Image).Value = imgbyte
    '        com.Parameters.Add("@upload_date", SqlDbType.Date).Value = Date.Today
    '        com.Parameters.Add("@uploader", SqlDbType.VarChar, 50).Value = HttpContext.Current.User.Identity.Name
    '        com.Parameters.Add("@edit_Date", SqlDbType.Date).Value = Date.Today
    '        com.Parameters.Add("@editor", SqlDbType.VarChar, 50).Value = HttpContext.Current.User.Identity.Name
    '        com.Parameters.Add("@active", SqlDbType.Int).Value = 1
    '        com.Parameters.Add("@remarks", SqlDbType.VarChar, 200).Value = TextBox_Remarks_Image.Text
    '        Try
    '            lcon.Open()
    '            com.ExecuteNonQuery()
    '            lcon.Close()
    '            CreateMessageAlert("success", "Upload image success.")
    '            setRepeaterImage()
    '        Catch ex As Exception
    '            CreateMessageAlert("error", "Error uploading image. Loc : Button_Upload_Click")
    '        End Try
    '    Else
    '        CreateMessageAlert("error", "Please select a file. Loc : Button_Upload_Click")
    '    End If
    'End Sub

    Protected Sub Repeater_Annot_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Repeater_Annot.ItemCommand
        If e.CommandName.ToString = "delete" And e.CommandArgument.ToString <> "" Then
            com = New SqlCommand("DELETE FROM GWISAnnotations WHERE id = " & e.CommandArgument & "", lcon)
            Try
                lcon.Open()
                com.ExecuteNonQuery()
                CreateMessageAlert("Success", "Delete data success !")
            Catch ex As Exception
                CreateMessageAlert("Error", "An error occured while deleting Repeater Data. Loc: Protected Sub Repeater_Annot_ItemCommand")
            Finally
                lcon.Close()
                com.Dispose()
            End Try
            setRepeaterAnnot(Request.QueryString("ImageID"))
            getEditImage(Request.QueryString("ImageID"))
        End If
    End Sub

    Protected Sub Button_Edit_Click(sender As Object, e As System.EventArgs) Handles Button_Edit.Click
        Dim arr1() As String = Split(TextBox_Start_Edit.Text, ",")
        Dim arr2() As String = Split(TextBox_End_Edit.Text, ",")
        Dim pos_top, pos_left, width, height As Integer
        pos_top = arr1(1)
        pos_left = arr1(0)
        height = arr2(1) - arr1(1)
        width = arr2(0) - arr1(0)

        com = New SqlCommand("UPDATE GWISAnnotations SET color = @color, pos_left = @pos_left, pos_top = @pos_top, width = @width, height = @height, remarks = @remarks, edit_date = @edit_date, editor = @editor WHERE id = @id", lcon)
        com.Parameters.Add("@pos_top", SqlDbType.Int).Value = pos_top
        com.Parameters.Add("@pos_left", SqlDbType.Int).Value = pos_left
        com.Parameters.Add("@width", SqlDbType.Int).Value = width
        com.Parameters.Add("@height", SqlDbType.Int).Value = height
        com.Parameters.Add("@edit_Date", SqlDbType.Date).Value = Date.Today
        com.Parameters.Add("@editor", SqlDbType.VarChar, 50).Value = HttpContext.Current.User.Identity.Name
        com.Parameters.Add("@remarks", SqlDbType.VarChar, 50).Value = TextBox_Remarks_Edit.Text
        com.Parameters.Add("@color", SqlDbType.VarChar, 50).Value = DropDownList_Color_Edit.SelectedValue
        com.Parameters.Add("@id", SqlDbType.Int).Value = HiddenField_Id_Edit.Value
        Try
            lcon.Open()
            com.ExecuteNonQuery()
            CreateMessageAlert("Success", "Edit data success.")
            setRepeaterAnnot(Request.QueryString("ImageID"))
            getEditImage(Request.QueryString("ImageID"))
        Catch ex As Exception
            CreateMessageAlert("Error", "Edit data failed. Loc : Button_Edit_Click")
        Finally
            lcon.Close()
        End Try
    End Sub

    'Protected Sub Repeater_Image_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Repeater_Image.ItemCommand
    '    If e.CommandName.ToString = "delete" And e.CommandArgument.ToString <> "" Then
    '        com = New SqlCommand("DELETE FROM WI_Image WHERE id = " & e.CommandArgument & "", lcon)
    '        Try
    '            lcon.Open()
    '            com.ExecuteNonQuery()
    '            CreateMessageAlert("success", "Delete data success !")
    '        Catch ex As Exception
    '            CreateMessageAlert("error", "An error occured while deleting Repeater Data. Loc: Protected Sub Repeater_Annot_ItemCommand")
    '        Finally
    '            lcon.Close()
    '            com.Dispose()
    '        End Try
    '        setRepeaterImage()
    '    End If
    'End Sub

    Protected Sub Button_Change_Click(sender As Object, e As System.EventArgs) Handles Button_Change.Click
        If FileUpload_ChangeImage.HasFile Then
            Dim imgbyte(FileUpload_ChangeImage.PostedFile.ContentLength) As Byte
            FileUpload_ChangeImage.PostedFile.InputStream.Read(imgbyte, 0, FileUpload_ChangeImage.PostedFile.ContentLength)
            com = New SqlCommand("UPDATE GWISPictures SET Image = @picture WHERE Id = @picture_id", lcon)
            com.Parameters.Add("@picture", SqlDbType.Image).Value = imgbyte
            com.Parameters.Add("@picture_id", SqlDbType.Int).Value = Request.QueryString("ImageID")
            Try
                lcon.Open()
                com.ExecuteNonQuery()
            Catch ex As Exception
                CreateMessageAlert("Error", "Error changing image. Loc : Button_Change_Click")
                Exit Sub
            Finally
                com.Dispose()
                lcon.Close()
            End Try

            com = New SqlCommand("DELETE FROM GWISAnnotations WHERE PictureId = @id_image", lcon)
            com.Parameters.Add("@id_image", SqlDbType.Int).Value = Request.QueryString("ImageID")
            Try
                lcon.Open()
                com.ExecuteNonQuery()
                CreateMessageAlert("success", "Change data success.")
                setRepeaterAnnot(Request.QueryString("ImageID"))
                getEditImage(Request.QueryString("ImageID"))
            Catch ex As Exception
                CreateMessageAlert("Error", "Error deleting related annot. Loc : Button_Change_Click")
                Exit Sub
            Finally
                com.Dispose()
                lcon.Close()
            End Try

        End If
    End Sub

End Class
