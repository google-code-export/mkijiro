Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class editor

    Private Sub load1(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim ss As String() = My.Settings.instdir.Split(CChar(vbLf))

        dir.Items.Clear()
        For Each s As String In ss
            dir.Items.Add(s.Trim)
        Next

        dir.Text = dir.Text
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim m As umdisomanger
        m = CType(Me.Owner, umdisomanger)

        If gname.Text = "" Then
            Beep()
            '"管理名が空です",
            MessageBox.Show(Me, m.lang(34), m.lang(1))
        ElseIf gid.Text = "" Then
            Beep()
            '"ゲームIDが空です"
            MessageBox.Show(Me, m.lang(35), m.lang(1))
        ElseIf File.Exists(fpath.Text) = False Then
            Beep()
            '"指定したファイルが存在しません"
            MessageBox.Show(Me, m.lang(36), m.lang(1))
        Else
            If impath.Text = "" Then
                impath.Text = gname.Text
            End If
            Me.Text = "APPLY"
            Me.Close()
        End If
    End Sub

    Private Sub closef(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Text = ""
        Me.Close()
    End Sub


    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click

        Dim ofd As New OpenFileDialog()
        Dim m As umdisomanger
        m = CType(Me.Owner, umdisomanger)

        ofd.InitialDirectory = My.Settings.isobase
        ofd.Filter = m.lang(25)
        ofd.Title = m.lang(26)
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = DialogResult.OK Then
            Dim psf As New psf
            If psf.video(ofd.FileName) <> "" Then
                fpath.Text = ofd.FileName
                If gname.Text = "" Then
                    gname.Text = psf.GETNAME(ofd.FileName, "N")
                End If
                If gid.Text = "" Then
                    gid.Text = psf.GETID(ofd.FileName)
                End If
                If dir.Text = "" Then
                    If psf.video(ofd.FileName) = "PSP" Then
                        dir.Text = "X:\ISO\"
                    ElseIf psf.video(ofd.FileName) = "VIDEO" Then
                        dir.Text = "X:\ISO\VIDEO\"
                    Else
                        dir.Text = "X:\PSP\GAME\"
                    End If
                End If

            Else
                '"UMDRAWイメージ,PBPではありません"
                MessageBox.Show(Me, m.lang(37), m.lang(1))
            End If
            My.Settings.isobase = Path.GetDirectoryName(ofd.FileName)
        End If
    End Sub


    Private Sub TreeView1_drop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles fpath.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub


    Private Sub ListBox1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles fpath.DragDrop
        Dim m As umdisomanger
        m = CType(Me.Owner, umdisomanger)
        Try

            Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            Dim psf As New psf
            If psf.video(fileName(0)) <> "" Then
                fpath.Text = fileName(0)
                If gname.Text = "" Then
                    gname.Text = psf.GETNAME(fileName(0), "N")
                End If
                If gid.Text = "" Then
                    gid.Text = psf.GETID(fileName(0))
                End If
                If dir.Text = "" Then
                    If psf.video(fileName(0)) = "PSP" Then
                        dir.Text = "X:\ISO\"
                    ElseIf psf.video(fileName(0)) = "VIDEO" Then
                        dir.Text = "X:\ISO\VIDEO\"
                    Else
                        dir.Text = "X:\PSP\GAME\"
                    End If
                End If
            Else
                MessageBox.Show(Me, m.lang(37), m.lang(1))
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, m.lang(7))
        End Try
    End Sub


    Private Sub drivelettter_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles fpath.KeyPress
        Dim mask As New Regex("[:/\?<>\|\*""]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        e.KeyChar = Char.ToUpper(e.KeyChar)
        If m.Success Then
            e.Handled = True
        End If
    End Sub

    Private Sub idmask(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gid.KeyPress
        Dim mask As New Regex("[^0-9A-Za-z\x08\-]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        e.KeyChar = Char.ToUpper(e.KeyChar)
        If m.Success Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim psf As New psf
        Dim path As String = fpath.Text
        If File.Exists(path) Then
            note.Text &= psf.GETNAME(path, "A")
        End If
    End Sub

    Private Sub note_TextChanged(sender As System.Object, e As System.EventArgs) Handles note.TextChanged

    End Sub
End Class