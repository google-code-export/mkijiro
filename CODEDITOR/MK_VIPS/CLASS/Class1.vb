Imports System.Text.RegularExpressions
Imports System.IO

Public Class checkupdate


    Public Sub upater(ByVal mode As String)

        If My.Computer.Network.IsAvailable Then
            Dim tx As String = "http://mkijiro.googlecode.com/svn/trunk/CODEDITOR/MK_VIPS/bin/Release/version.txt"
            Dim uptx As String = "http://mkijiro.googlecode.com/svn/trunk/CODEDITOR/MK_VIPS/bin/Release/updater.txt"
            Dim up As String = "http://mkijiro.googlecode.com/svn/trunk/CODEDITOR/updater/bin/Release/updater.exe"

            '保存先のファイル名
            Dim fileName As String = ""
            Dim check As New checkupdate
            Dim f As New version
            Dim b1 As String = check.getweb(fileName, tx, 0)
            Dim b2 As String = StrConv(f.Label3.Text, VbStrConv.Narrow)
            Dim dd As String = b2.Substring(6, 10)
            Dim textsubnum As String = ""
            Dim s As New Regex("\d\d\d\d/\d{1,2}/\d{1,2}", RegexOptions.IgnoreCase)
            Dim m As Match = s.Match(b1)
            If m.Success Then
                Dim rr As String = m.Value
                If rr.Substring(7, 1) <> "/" Then
                    rr = rr.Insert(5, "0")
                End If
                If rr.Length = 9 Then
                    rr = rr.Insert(8, "0")
                End If

                If String.Compare(dd, rr, True) <= 0 Then
                    Dim exeupdate As Boolean = False
                    If String.Compare(dd, rr, True) < 0 Then
                        exeupdate = True
                    ElseIf String.Compare(dd, rr, True) = 0 AndAlso b1.Contains("-") Then
                        If Not b2.Contains("-") Then
                            exeupdate = True
                        ElseIf b2.Contains("-") Then
                            Dim subnum As String = b2.Substring(17, 1)
                            textsubnum = b1.Substring(b1.Length - 1, 1)
                            subnum = StrConv(subnum, VbStrConv.Narrow)
                            textsubnum = StrConv(textsubnum, VbStrConv.Narrow)
                            If String.Compare(subnum, textsubnum, True) < 0 Then
                                exeupdate = True
                            End If
                        End If
                    End If

                    If exeupdate = True Then
                        Dim result As DialogResult = MessageBox.Show("最新版が見つかりました、ダウンロードしますか？", "更新の確認", _
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        If result = DialogResult.Yes Then
                            Dim txt As String = Application.StartupPath & "\up\updater.txt"
                            Dim boot As String = Application.StartupPath & "\up\updater.exe"
                            If My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\up") = False Then
                                System.IO.Directory.CreateDirectory(Application.StartupPath & "\up")
                            End If
                            check.getweb(txt, uptx, 1)
                            check.getweb(boot, up, 1)
                            Process.Start(boot)
                            Form1.Close()
                        End If

                    ElseIf mode = "help" Then
                        Beep()
                        MessageBox.Show("最新版です", "更新の確認")
                    End If

                ElseIf mode = "help" Then
                    Beep()
                    MessageBox.Show("最新版です", "更新の確認")
                End If

            ElseIf mode = "help" Then
                Beep()
                MessageBox.Show("更新情報の取得に失敗しました", "サーバーエラー")
            End If

        ElseIf mode = "help" Then
            Beep()
            MessageBox.Show("インターネットに接続されてません", "接続エラー")
        End If

    End Sub

    Function getweb(ByVal filename3 As String, ByVal url As String, ByVal webmode As Integer) As String

        'WebRequestの作成
        Dim webreq As System.Net.HttpWebRequest = _
            CType(System.Net.WebRequest.Create(url),  _
                System.Net.HttpWebRequest)

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim webres As System.Net.HttpWebResponse = Nothing
        Try
            'サーバーからの応答を受信するためのWebResponseを取得
            webres = CType(webreq.GetResponse(), System.Net.HttpWebResponse)

        Catch ex As System.Net.WebException
            'HTTPプロトコルエラーかどうか調べる
            If ex.Status = System.Net.WebExceptionStatus.ProtocolError Then
                'HttpWebResponseを取得
                Dim errres As System.Net.HttpWebResponse = _
                    CType(ex.Response, System.Net.HttpWebResponse)
            Else
            End If
        End Try

        If (webres Is Nothing) Then
            Return ""
        ElseIf webres.ResponseUri.AbsolutePath.Contains("404") Then
            Return ""
        End If

        Dim enc As System.Text.Encoding = _
    System.Text.Encoding.GetEncoding(932)
        '応答データを受信するためのStreamを取得
        Dim strm As System.IO.Stream = webres.GetResponseStream()
        If webmode = 0 Then
            Dim sr As New System.IO.StreamReader(strm, enc)
            '受信して表示
            Dim html As String = sr.ReadLine

            sr.Close()
            strm.Close()
            Return StrConv(html, VbStrConv.Narrow)

        ElseIf webmode = 1 Then
            'ファイルに書き込むためのFileStreamを作成
            Dim fs As New System.IO.FileStream(filename3, _
                System.IO.FileMode.Create, System.IO.FileAccess.Write)

            '応答データをファイルに書き込む
            Dim readData(1023) As Byte
            Dim readSize As Integer = 0
            While True
                readSize = strm.Read(readData, 0, readData.Length)
                If readSize = 0 Then
                    Exit While
                End If
                fs.Write(readData, 0, readSize)
            End While

            '閉じる
            fs.Close()
            strm.Close()
        End If

        Return "OK"
    End Function

End Class
