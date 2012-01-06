Imports System
Imports System.IO


Public Class extra
    Friend x As Single = 1.0F

    Private Sub extra_load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        PictureBox1.AllowDrop = True
        PictureBox2.AllowDrop = True
        PictureBox3.AllowDrop = True
    End Sub


    Private Sub Form1_Activated( _
        ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Activated
        Dim im As String = Application.StartupPath & "\imgs\user\" & Me.Text & "boxart.png"
        Dim um As String = Application.StartupPath & "\imgs\user\" & Me.Text & "umd_front.png"
        Dim um2 As String = Application.StartupPath & "\imgs\user\" & Me.Text & "umd_back.png"
        If File.Exists(im) Then
            bitmap_resize(PictureBox1, im, 790, 630)
        End If
        If File.Exists(um) Then
            bitmap_resize(PictureBox2, um, 280, 260)
        End If
        If File.Exists(um2) Then
            PictureBox3.Image = System.Drawing.Image.FromFile(um2)
        End If
    End Sub

    Public Function AutoGraphics(ByVal picSource As PictureBox) As Graphics

        If picSource.Image Is Nothing Then
            picSource.Image = New Bitmap(picSource.ClientRectangle.Width, picSource.ClientRectangle.Height)
        End If

        Return Graphics.FromImage(picSource.Image)

    End Function

    Private Sub drop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragEnter, PictureBox2.DragEnter, PictureBox3.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub picture1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragDrop
        Try
            'Dim m As umdisomanger
            'm = CType(Me.Owner, umdisomanger)
            Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            Dim picture As String = Application.StartupPath & "\imgs\user\" & Me.Text & "boxart.png"
            Me.Focus()
            If picture = fileName(0) Then

            ElseIf File.Exists(picture) = True Then
                'If MessageBox.Show(m.lang(39) & picture & m.lang(40), m.lang(14), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                If File.Exists(picture) = True AndAlso MessageBox.Show(picture & "が存在します。削除してもよろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    File.Delete(picture)
                    File.Copy(fileName(0), picture)
                End If
            Else
                File.Copy(fileName(0), picture)
            End If
            bitmap_resize(PictureBox1, picture, 790, 630)
            If picture <> fileName(0) Then
                Dim bmp As New Bitmap(fileName(0))
                bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                bmp.Dispose()
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, m.lang(7))
        End Try
    End Sub
    Private Sub picture2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox2.DragDrop
        Try
            Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            Dim picture As String = Application.StartupPath & "\imgs\user\" & Me.Text & "umd_front.png"

            'Dim m As New umdisomanger
            ''m = CType(Me.Owner, umdisomanger)
            Me.Focus()
            If picture = fileName(0) Then

                ' ElseIf File.Exists(picture) = True AndAlso MessageBox.Show(m.lang(39) & picture & m.lang(40), m.lang(14), _
                'MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
            ElseIf File.Exists(picture) = True AndAlso File.Exists(picture) = True AndAlso MessageBox.Show(picture & "が存在します。削除してもよろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

                File.Delete(picture)
                File.Copy(fileName(0), picture)
            Else
                File.Copy(fileName(0), picture)
            End If
            bitmap_resize(PictureBox2, picture, 280, 260)
            If picture <> fileName(0) Then
                Dim bmp As New Bitmap(fileName(0))
                bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                bmp.Dispose()
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, lang(7))
        End Try
    End Sub

    Private Sub picture3_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox3.DragDrop
        Try
            Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            Dim picture As String = Application.StartupPath & "\imgs\user\" & Me.Text & "umd_back.png"
            Dim m As umdisomanger
            m = CType(Me.Owner, umdisomanger)
            Me.Focus()
            If picture = fileName(0) Then

            ElseIf File.Exists(picture) = True AndAlso MessageBox.Show(m.lang(39) & picture & m.lang(40), m.lang(14), _
           MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                File.Delete(picture)
                File.Copy(fileName(0), picture)
            Else
                File.Copy(fileName(0), picture)
            End If
            bitmap_resize(PictureBox3, picture, 280, 260)
            If picture <> fileName(0) Then
                Dim bmp As New Bitmap(fileName(0))
                bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                bmp.Dispose()
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, lang(7))
        End Try
    End Sub



    Function bitmap_resize(ByRef pc As PictureBox, ByVal path As String, ByVal width As Integer, ByVal height As Integer) As Boolean

        Dim image = New Bitmap(path)
        'PictureBox1のGraphicsオブジェクトの作成

        pc.Image = Nothing
        Dim g As Graphics = AutoGraphics(pc) 'pc.CreateGraphics()
        '補間方法として最近傍補間を指定する
        g.InterpolationMode = _
            System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        g.Clear(Color.White)
        '画像を縮小表示
        g.DrawImage(image, 0, 0, width, height)
        '補間方法として高品質双三次補間を指定する
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

        'BitmapとGraphicsオブジェクトを破棄
        image.Dispose()
        g.Dispose()
        Return True
    End Function

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        PictureBox3.Image = Magnify(PictureBox3.Image, x * 1.2F)

    End Sub


    Private Function Magnify(ByVal Source As Image, ByVal Rate As Double, Optional ByVal Quality As Drawing2D.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic) As Image

        '▼引数のチェック

        If IsNothing(Source) Then
            Throw New NullReferenceException("Sourceに値が設定されていません。")
        End If

        If CInt(Source.Size.Width * Rate) <= 0 OrElse CInt(Source.Size.Height * Rate) <= 0 Then
            Throw New ArgumentException("処理後の画像のサイズが0以下になります。Rateには十分大きな値を指定してください。")
        End If

        '▼処理後の大きさの空の画像を作成

        Dim NewRect As Rectangle

        NewRect.Width = CInt(Source.Size.Width * Rate)
        NewRect.Height = CInt(Source.Size.Height * Rate)

        Dim DestImage As New Bitmap(NewRect.Width, NewRect.Height)

        '▼拡大・縮小実行

        Dim g As Graphics = Graphics.FromImage(DestImage)

        g.InterpolationMode = Quality
        g.DrawImage(Source, NewRect)

        Return DestImage

    End Function

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        PictureBox3.Image = Magnify(PictureBox3.Image, x * 0.8F)
    End Sub
End Class