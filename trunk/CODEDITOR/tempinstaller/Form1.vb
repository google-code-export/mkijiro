Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Function getweb(ByVal filename3 As String, ByVal url As String) As Boolean

        'WebRequestの作成
        Dim webreq As System.Net.HttpWebRequest = _
            CType(System.Net.WebRequest.Create(url),  _
                System.Net.HttpWebRequest)

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim webres As System.Net.HttpWebResponse = _
            CType(webreq.GetResponse(), System.Net.HttpWebResponse)

        '応答データを受信するためのStreamを取得
        Dim strm As System.IO.Stream = webres.GetResponseStream()

        'ファイルに書き込むためのFileStreamを作成
        Dim fs As New System.IO.FileStream(filename3, _
            System.IO.FileMode.Create, System.IO.FileAccess.Write)

        '応答データをファイルに書き込む
        Dim b As Integer
        While True
            b = strm.ReadByte()
            If b = -1 Then Exit While
            fs.WriteByte(Convert.ToByte(b))
        End While

        '閉じる
        fs.Close()
        strm.Close()

        Return True
    End Function

    Function unzip(ByVal zippath As String)
        '展開先のフォルダのパス
        Dim extractDir As String = Application.StartupPath

        'ZIP書庫を読み込む 
        Dim fs As New System.IO.FileStream( _
            zipPath, _
            System.IO.FileMode.Open, _
            System.IO.FileAccess.Read)
        'ZipInputStreamオブジェクトの作成 
        Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(fs)

        'パスワードが設定されているときは指定する 
        'zis.Password = "pass"

        'ZIP内のエントリを列挙 
        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
        While True
            'ZipEntryを取得
            ze = zis.GetNextEntry()
            If ze Is Nothing Then
                Exit While
            End If
            If Not ze.IsDirectory Then
                '展開先のファイル名を決定 
                Dim fileName As String = System.IO.Path.GetFileName(ze.Name)
                '展開先のフォルダを決定 
                Dim destDir As String = System.IO.Path.Combine( _
                    extractDir, System.IO.Path.GetDirectoryName(ze.Name))
                System.IO.Directory.CreateDirectory(destDir)
                '展開先のファイルのフルパスを決定 
                Dim destPath As String = System.IO.Path.Combine(destDir, fileName)

                '書き込み先のファイルを開く
                Dim writer As New System.IO.FileStream(destPath, _
                    System.IO.FileMode.Create, System.IO.FileAccess.Write)
                '展開するファイルを読み込む
                Dim buffer As Byte() = New Byte(2047) {}
                Dim len As Integer
                While True
                    len = zis.Read(buffer, 0, buffer.Length)
                    If len = 0 Then
                        Exit While
                    End If
                    'ファイルに書き込む
                    writer.Write(buffer, 0, len)
                End While
                '閉じる 
                writer.Close()
            Else
                'フォルダのとき 
                '作成するフォルダのフルパスを決定 
                Dim dirPath As String = System.IO.Path.Combine( _
                    extractDir, System.IO.Path.GetDirectoryName(ze.Name))
                'フォルダを作成 
                System.IO.Directory.CreateDirectory(dirPath)
            End If
        End While

        '閉じる 
        zis.Close()
        fs.Close()
        Return True
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = ""

        If My.Computer.Network.IsAvailable Then
            Dim installpath As String = findpsp()
            Dim temp() As String = {"\seplugins\Tempar\tempar.prx", "\seplugins\Tempar\tempar_lite.prx", "\seplugins\Tempar\tempar_autooff.prx"}
            If installpath <> "" Then
                TextBox1.Text = "PSPが見つかりました,temparのダウンロードを開始します" & vbCrLf
                Dim fileName As String = "tmp.zip"
                getweb(fileName, My.Settings.tempar)
                TextBox1.Text &= "TEMPARをPSPにコピーしています..." & vbCrLf
                unzip(fileName)
                For i = 0 To 1
                    File.Copy(Application.StartupPath & temp(i), installpath.Substring(0, 3) & temp(i), True)
                Next
                If My.Settings.tempar.Contains("1.62") Then
                    File.Copy(Application.StartupPath & temp(2), installpath.Substring(0, 3) & temp(2), True)
                End If
                If My.Settings.tempar.Contains("1.63") AndAlso CheckBox1.Checked = True Then
                    TextBox1.Text &= "ランゲージファイルをコピーしています" & vbCrLf
                    Dim temptxt As String = "\seplugins\Tempar\languages\language"
                    For k = 1 To 2
                        File.Copy(Application.StartupPath & temptxt & k.ToString & ".bin", installpath.Substring(0, 3) & temptxt & k.ToString & ".bin", True)
                    Next
                End If
                TextBox1.Text &= "インストールが完了しました"
            Else
                TextBox1.Text = "PSPが接続されてません"
            End If

            Else
            TextBox1.Text = "インターネットに接続されてません"
            End If
    End Sub

    Function findpsp() As String
        Dim PSP As String = " :\PSP"
        Dim drive() As String = {"C", "D", "E", "F", "G", "H", "I", "J", "K", "L"}
        Dim i As Integer
        For i = 0 To 9
            PSP = PSP.Remove(0, 1)
            PSP = PSP.Insert(0, drive(i))
            If My.Computer.FileSystem.DirectoryExists(PSP) Then
                Return PSP
            End If
        Next
        Return ""
    End Function

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        My.Settings.tempar = "http://raing3.gshi.org/psp-utilities/files/psp/tempar/tempar-1.62-3.zip"
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        My.Settings.tempar = "http://raing3.gshi.org/psp-utilities/files/psp/tempar/tempar-1.63.zip"
    End Sub
End Class
