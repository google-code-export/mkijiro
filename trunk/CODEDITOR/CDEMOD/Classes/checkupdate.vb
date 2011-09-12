﻿Imports System.Text.RegularExpressions
Imports System.IO

Public Class checkupdate


    Public Sub CDEupater(ByVal mode As String)

        If My.Computer.Network.IsAvailable Then
            Dim tx As String = "http://mkijiro.googlecode.com/svn/trunk/CODEDITOR/CDEMOD/bin/Release/version"
            Dim exe As String = "http://unzu127xp.pa.land.to/mogura/writelog.php?dl=http://unzu127xp.pa.land.to/data/IJIRO/CDEMOD/bin/Release/CDE_CP932_FM4.exe"
            '保存先のファイル名
            Dim fileName As String = "CDEMOD.exe"
            Dim check As New checkupdate
            Dim f As Form2 = Form2
            Dim b1 As String = check.getweb(fileName, tx, 0)
            Dim dd As String = f.Label2.Text.Substring(6, 10)
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
                        If Not f.Label2.Text.Contains("-") Then
                            exeupdate = True
                        ElseIf f.Label2.Text.Contains("-") Then
                            Dim subnum As String = f.Label2.Text.Substring(17, 1)
                            Dim textsubnum As String = b1.Substring(b1.Length - 1, 1)
                            If String.Compare(subnum, subnum, True) < 0 Then
                                exeupdate = True
                            End If
                        End If
                    End If

                    If exeupdate = True Then
                        Dim result As DialogResult = MessageBox.Show("最新版が見つかりました、ダウンロードしますか？", "更新の確認", _
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        If result = DialogResult.Yes Then
                            Dim save As String = fileName.Insert(fileName.LastIndexOf("."), rr)
                            save = save.Replace(":", "")
                            save = save.Replace("/", "")
                            Dim ok As String = check.getweb(save, exe, 1)
                            If ok = "OK" Then
                                MessageBox.Show(save & vbCrLf & "BUID:" & m.Value & "のダウンロードが完了しました" _
                                 & vbCrLf & "設定を引き継ぎたい場合は起動しているEXE名と同じ名前にしてください", "ダウンロード完了")
                            Else
                                MessageBox.Show("ダウンロードに失敗しました", "ダウンロード失敗")
                            End If
                        End If

                    ElseIf mode = "help" Then
                        MessageBox.Show("最新版です", "更新の確認")
                    End If

                ElseIf mode = "help" Then
                    MessageBox.Show("最新版です", "更新の確認")
                End If

            ElseIf mode = "help" Then
                MessageBox.Show("更新情報の取得に失敗しました", "サーバーエラー")
            End If

        ElseIf mode = "help" Then
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
    System.Text.Encoding.GetEncoding(65001)
        '応答データを受信するためのStreamを取得
        Dim strm As System.IO.Stream = webres.GetResponseStream()
        If webmode = 0 Then
            Dim sr As New System.IO.StreamReader(strm, enc)
            '受信して表示
            Dim html As String = sr.ReadLine

            sr.Close()
            strm.Close()
            Return html

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