Public Class Form1

    Friend fontpath As String

    Private Sub form_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fontpath = CStr(fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\telazon_misakihira")
        font1.Checked = True
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = _
    "prxファイル(*.prx)|*.prx;"
        ofd.Title = "cwcheat*.prxを選択してください"
        If ofd.ShowDialog() = DialogResult.OK Then
            Dim fs As New System.IO.FileStream(ofd.FileName, _
    System.IO.FileMode.Open, _
    System.IO.FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            fs.Read(bs, 0, bs.Length)
            fs.Close()
            Dim patchfont(2048) As Byte
            If System.IO.File.Exists(fontpath) Then
                Dim font As New System.IO.FileStream(fontpath, _
        System.IO.FileMode.Open, _
        System.IO.FileAccess.Read)
                font.Read(patchfont, 0, 2048)
                font.Close()
                Dim i As Integer = 0
                While i < bs.Length
                    If bs(i) = &H64 AndAlso bs(i + 1) = &H62 AndAlso bs(i + 2) = &H5F AndAlso bs(i + 3) = &H72 AndAlso bs(i + 4) = &H65 AndAlso bs(i + 5) = &H61 AndAlso bs(i + 6) = &H64 AndAlso bs(i + 7) = &H5F AndAlso bs(i + 8) = &H62 AndAlso bs(i + 9) = &H75 AndAlso bs(i + 10) = &H66 AndAlso bs(i + 11) = &H66 AndAlso bs(i + 12) = &H65 AndAlso bs(i + 13) = &H72 Then
                        Array.ConstrainedCopy(patchfont, 0, bs, i + 16, 2048)
                        Exit While
                    End If
                    i += 1
                End While
                Dim save As New System.IO.FileStream(System.AppDomain.CurrentDomain.BaseDirectory & "cwc_patched.prx", _
        System.IO.FileMode.Create, _
        System.IO.FileAccess.Write)
                'バイト型配列の内容をすべて書き込む 
                save.Write(bs, 0, bs.Length)
                '閉じる 
                save.Close()
            Else
                MessageBox.Show("フォントファイルがありません。")
            End If
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font1.CheckedChanged
        fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\telazorn_misakihira"
    End Sub

    Private Sub font2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font2.CheckedChanged
        fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\telazorn_misakikana"
    End Sub

    Private Sub font3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font3.CheckedChanged
        fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\misakihira"
    End Sub

    Private Sub font4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font4.CheckedChanged
        fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\misakikana"
    End Sub
End Class
