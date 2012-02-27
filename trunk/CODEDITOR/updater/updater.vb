Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class updater

    Private Sub main_Load(ByVal sender As Object, _
        ByVal e As EventArgs) Handles MyBase.Load

        Dim uppath As String = Application.StartupPath & "\updater.txt"
        Dim boot As String = ""

        If File.Exists(uppath) Then
            Dim sr As New System.IO.StreamReader(uppath, _
        System.Text.Encoding.GetEncoding(65001))
            '内容をすべて読み込む
            Dim s As String = ""
            Dim url As String = ""
            Dim xxx As New Regex(""".*?""", RegexOptions.ECMAScript)
            Dim m As Match
            Dim ct As Boolean = False
            While sr.Peek() > -1
                s = sr.ReadLine()
                If s.Length > 3 Then
                    ct = False
                    If s.Contains("DL=") Then
                        s = s.Remove(0, 3)
                        m = xxx.Match(s)
                        While m.Success
                            If ct = False Then
                                url = m.Value.Replace("""", "")
                                ct = True
                            Else
                                s = m.Value.Replace("""", "")
                                getfile(s, url)
                                Exit While
                            End If
                            m = m.NextMatch
                        End While

                    ElseIf s.Contains("BOOT=") Then
                        s = s.Remove(0, 5)
                        m = xxx.Match(s)
                        While m.Success
                            If ct = False Then
                                url = m.Value.Replace("""", "")
                                ct = True
                            Else
                                boot = m.Value.Replace("""", "")
                                getfile(boot, url)
                                Exit While
                            End If
                            m = m.NextMatch
                        End While
                    End If
                End If
            End While
            '閉じる
            sr.Close()


            File.Delete(uppath)

            Process.Start(boot)
        Else
            MessageBox.Show("updater.txtが見つかりませんでした", "うｐ失敗")
        End If

        Me.Close()

    End Sub

    Function getfile(ByVal s As String, ByVal url As String) As Boolean

        '保存先のファイル名
        Dim fileName As String = s

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
        Dim fs As New System.IO.FileStream(fileName, _
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
    End Function

End Class
