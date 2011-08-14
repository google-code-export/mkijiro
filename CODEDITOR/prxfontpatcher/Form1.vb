Public Class Form1

    Friend fontpath As String

    Private Sub form_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fontpath = CStr(fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\telazorn_misakihira")
        font1.Checked = True
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim packedprx As Boolean = False
        Dim ofd As New OpenFileDialog()
        ofd.Filter = _
    "prxファイル(*.prx)|*.prx;"
        ofd.Title = "*.prxを選択してください"
        If ofd.ShowDialog() = DialogResult.OK Then
            Dim fs As New System.IO.FileStream(ofd.FileName, _
    System.IO.FileMode.Open, _
    System.IO.FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            fs.Read(bs, 0, bs.Length)
            Dim header(4) As Byte
            fs.Seek(0, IO.SeekOrigin.Begin)
            fs.Read(header, 0, 4)
            fs.Close()
            Dim str As String = System.Text.Encoding.GetEncoding(932).GetString(header)
            If str.Contains("~PSP") Then
                Dim gzip As New System.IO.FileStream(System.AppDomain.CurrentDomain.BaseDirectory & "font\fakegziphead", _
        System.IO.FileMode.Open, _
        System.IO.FileAccess.Read)
                Dim ghead(18) As Byte
                gzip.Read(ghead, 0, 19)
                gzip.Close()
                Array.ConstrainedCopy(ghead, 0, bs, 0, 19)
                Array.ConstrainedCopy(bs, 346, bs, 19, bs.Length - 346)
                Dim gzipprx As New System.IO.FileStream(System.AppDomain.CurrentDomain.BaseDirectory & "font\temp.gz", _
        System.IO.FileMode.Create, _
        System.IO.FileAccess.Write)
                'バイト型配列の内容をすべて書き込む 
                gzipprx.Write(bs, 0, bs.Length)
                '閉じる 
                gzipprx.Close()
                Dim gzipFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "font\temp.gz"
                '展開先のファイル名
                Dim outFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "font\temp.prx"
                '展開する書庫のFileStreamを作成する
                Dim gzipFileStrm As New System.IO.FileStream( _
                    gzipFile, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                '圧縮解除モードのGZipStreamを作成する
                Dim gzipStrm As New System.IO.Compression.GZipStream( _
                    gzipFileStrm, System.IO.Compression.CompressionMode.Decompress)
                '展開先のファイルのFileStreamを作成する
                Dim outFileStrm As New System.IO.FileStream( _
                    outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write)

                Dim buffer(1024) As Byte
                While True
                    '書庫から展開されたデータを読み込む
                    Dim readSize As Integer = gzipStrm.Read(buffer, 0, buffer.Length)
                    '最後まで読み込んだ時は、ループを抜ける
                    If readSize = 0 Then
                        Exit While
                    End If '展開先のファイルに書き込む
                    outFileStrm.Write(buffer, 0, readSize)
                End While
                '閉じる
                outFileStrm.Close()
                gzipStrm.Close()
                Dim unpack As New System.IO.FileStream(outFile, _
        System.IO.FileMode.Open, _
        System.IO.FileAccess.Read)
                Dim unpackprx(CInt(unpack.Length - 1)) As Byte
                unpack.Read(unpackprx, 0, CInt(unpack.Length))
                Array.Resize(bs, CInt(unpack.Length - 1))
                Array.Copy(unpackprx, 0, bs, 0, CInt(unpack.Length - 1))
                unpack.Close()
            End If
            Dim patchfont(2048) As Byte
            If System.IO.File.Exists(fontpath) Then
                Dim font As New System.IO.FileStream(fontpath, _
        System.IO.FileMode.Open, _
        System.IO.FileAccess.Read)
                font.Read(patchfont, 0, 2048)
                font.Close()
                Dim i As Integer = 0
                While i < bs.Length - 8
                    '3C 42 A5 81 A5 99 42 3C
                    If bs(i) = &H3C AndAlso bs(i + 1) = &H42 AndAlso bs(i + 2) = &HA5 AndAlso bs(i + 3) = &H81 AndAlso bs(i + 4) = &HA5 AndAlso bs(i + 5) = &H99 AndAlso bs(i + 6) = &H42 AndAlso bs(i + 7) = &H3C Then
                            Array.ConstrainedCopy(patchfont, 0, bs, i - 8, 2048)
                            Exit While
                        ElseIf bs(i) = &H7E AndAlso bs(i + 1) = &H81 AndAlso bs(i + 2) = &HA5 AndAlso bs(i + 3) = &H81 AndAlso bs(i + 4) = &HBD AndAlso bs(i + 5) = &H99 AndAlso bs(i + 6) = &H81 AndAlso bs(i + 7) = &H7E Then
                            Array.ConstrainedCopy(patchfont, 0, bs, i - 8, 2048)
                            Exit While
                        End If
                        i += 1
                End While
                Dim index As Integer = ofd.FileName.LastIndexOf(".")
                Dim newfile As String = ofd.FileName.Insert(index, "_patched")
                Dim save As New System.IO.FileStream(newfile, _
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

    Private Sub font5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles font5.CheckedChanged
        fontpath = System.AppDomain.CurrentDomain.BaseDirectory & "font\acorn_bold"
    End Sub
End Class
