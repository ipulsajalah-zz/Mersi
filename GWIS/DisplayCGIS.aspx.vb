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

Partial Class report_displayImage
    Inherits System.Web.UI.Page
    Dim dr As SqlDataReader
    Dim com As SqlCommand
    Dim lcon As New SqlConnection(ConfigurationManager.ConnectionStrings("AppDb").ToString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lcon = CType(Ebiz.Scaffolding.Utils.ConnectionHelper.GetConnection(), SqlConnection)

        If Request.QueryString("ImageID") IsNot Nothing Then
            Dim bmp As Bitmap
            Dim newImage As Byte()

            Dim strQuery As String = "select * from GWISPictures where Id=@id"
            Dim cmd As SqlCommand = New SqlCommand(strQuery)
            cmd.Parameters.Add("@id", SqlDbType.Int).Value() = Convert.ToInt32(Request.QueryString("ImageID"))
            Dim dt As DataTable = GetData(cmd)

            If dt IsNot Nothing Then
                Dim maxHeight As Integer = 500
                Dim maxWidth As Integer = 500
                Dim oriStream As New System.IO.MemoryStream(CType(dt.Rows(0)("Image"), Byte()))
                Dim imageToBeResized As System.Drawing.Image = System.Drawing.Image.FromStream(oriStream)

                ' Scaling & resizing to fit the proportion
                Dim imageHeight As Integer = imageToBeResized.Height
                Dim imageWidth As Integer = imageToBeResized.Width
                imageHeight = (imageHeight * maxWidth) / imageWidth
                imageWidth = maxWidth

                If imageHeight > maxHeight Then
                    imageWidth = (imageWidth * maxHeight) / imageHeight
                    imageHeight = maxHeight
                End If

                bmp = New Bitmap(imageToBeResized, imageWidth, imageHeight)
                newImage = BmpToBytes_MemStream(bmp)

                'Dim bytes() As Byte = DrawOnImage(CType(dt.Rows(0)("img_eng_op"), Byte()), Convert.ToInt32(Request.QueryString("ImageID")))
                Dim bytes() As Byte = DrawOnImage(newImage, Convert.ToInt32(Request.QueryString("ImageID")))

                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.BinaryWrite(bytes)
                Response.Flush()
                Response.End()
            End If
        End If
    End Sub

    Public Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As New DataTable
        Dim lcon As New SqlConnection(ConfigurationManager.ConnectionStrings("AppDb").ToString)
        Dim sda As New SqlDataAdapter
        cmd.CommandType = CommandType.Text
        cmd.Connection = lcon
        Try
            lcon.Open()
            sda.SelectCommand = cmd
            sda.Fill(dt)
            Return dt
        Catch ex As Exception
            Response.Write(ex.Message)
            Return Nothing
        Finally
            lcon.Close()
            sda.Dispose()
            lcon.Dispose()
        End Try
    End Function

    Private Function DrawOnImage(ByVal OldImage As Byte(), ByVal ImageID As Integer) As Byte()
        Dim TmpSize As System.Drawing.Size
        Dim ImageByte As Byte()
        Dim factor As Double = 1
        Dim divider As Double = 1

        Dim imgMemoryStream As MemoryStream = New MemoryStream(OldImage)
        Dim Image As System.Drawing.Image = Drawing.Image.FromStream(imgMemoryStream)


        'Read Image Dimensions
        TmpSize.Height = Image.Height
        TmpSize.Width = Image.Width

        'Create a new Bitmap Object
        Dim NewBitmap As New System.Drawing.Bitmap(Image)
        'Create a new Graphic Object
        Dim Graphic As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(NewBitmap)

        'Picture processing
        com = New SqlCommand("SELECT * FROM GWISAnnotations WHERE PictureId = " & ImageID, lcon)
        lcon.Open()
        dr = com.ExecuteReader

        If dr.HasRows Then
            While dr.Read
                If dr("type") = 1 Then ' Arrow
                    Dim bigArrow As New AdjustableArrowCap(4, 6)
                    Dim Pen As New Pen(Drawing.Color.FromName(dr("color")), 3)
                    Pen.CustomEndCap = bigArrow
                    Dim Brush As New SolidBrush(Drawing.Color.FromName(dr("color")))
                    Graphic.DrawLine(Pen, CSng(dr("pos_left")), CSng(dr("pos_top")), CSng(dr("pos_left") + dr("width")), CSng(dr("pos_top") + dr("height")))
                    If Request.QueryString("state") = "yes" Then
                        Graphic.DrawString(dr("id"), New Font("Arial", 12), Brush, New Point(CSng(dr("pos_left") - 12 + dr("width") / 2), CSng(dr("pos_top") - 10 + dr("height") / 2)))
                    End If
                ElseIf dr("type") = 2 Then ' Rectangle
                    Dim Pen As New Pen(Drawing.Color.FromName(dr("color")), 1)
                    Dim Brush As New SolidBrush(Drawing.Color.FromName(dr("color")))
                    Dim rect As Rectangle
                    rect = New Rectangle(dr("pos_left"), dr("pos_top"), dr("width"), dr("height"))
                    Graphic.FillRectangle(New SolidBrush(Color.White), rect)
                    Graphic.DrawString(dr("remarks"), New Font("Arial", 13), Brush, rect)
                    Graphic.DrawRectangle(Pen, rect)
                    If Request.QueryString("state") = "yes" Then
                        Graphic.DrawString(dr("id"), New Font("Arial", 9), Brush, New Point(CSng(dr("pos_left") - 30 + dr("width")), CSng(dr("pos_top") + 30)))
                    End If
                ElseIf dr("type") = 3 Then ' Ellipse
                    Dim Pen As New Pen(Drawing.Color.FromName(dr("color")), 3)
                    Dim Brush As New SolidBrush(Drawing.Color.FromName(dr("color")))
                    Dim rect As Rectangle
                    rect = New Rectangle(dr("pos_left"), dr("pos_top"), dr("width"), dr("height"))
                    Graphic.DrawEllipse(Pen, rect)
                    If Request.QueryString("state") = "yes" Then
                        Graphic.DrawString(dr("id"), New Font("Arial", 12), Brush, New Point(CSng(dr("pos_left") - 12 + dr("width") / 2), CSng(dr("pos_top") + dr("height") / 2)))
                    End If
                    End If
            End While
        End If
        dr.Close()
        lcon.Close()

        'Save new Image
        ImageByte = BmpToBytes_MemStream(NewBitmap)

        Graphic.Dispose()
        NewBitmap.Dispose()

        Return ImageByte
    End Function

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
End Class
