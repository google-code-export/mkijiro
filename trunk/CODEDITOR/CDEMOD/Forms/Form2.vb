Imports System.Text.RegularExpressions
Imports System.IO


Public Class Form2
    Public Sub form2_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Me.Location = New Point(m.Location.X + 500, m.Location.Y + 40)

        If m.fixedform.Checked = True Then
            Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Process.Start(m.browser, "http://code.google.com/p/mkijiro/source/browse/#svn%2Ftrunk%2FCODEDITOR%2FCDEMOD")
    End Sub

    Private Sub CDEupate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CDEupdate.Click

        If My.Computer.Network.IsAvailable Then
            Dim tx As String = "http://unzu127xp.pa.land.to/data/IJIRO/CDEMOD/bin/Release/CDE_GBK.txt"
            Dim exe As String = "http://unzu127xp.pa.land.to/mogura/writelog.php?dl=http://unzu127xp.pa.land.to/data/IJIRO/CDEMOD/bin/Release/CDE_CP932_FM4.exe"
            '保存先のファイル名
            Dim fileName As String = "APP\version"
            Dim fileName2 As String = "CDEMOD.exe"

            If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\APP") Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\APP")
            End If

                getweb(fileName, tx)
                Dim b1 As String
                Dim dd As String = Me.Label2.Text.Substring(6, 10)
                Dim txt As New FileStream(fileName, FileMode.Open, FileAccess.Read)
                Dim sr As New StreamReader(fileName, _
                                           System.Text.Encoding.GetEncoding(0))

                Do Until sr.EndOfStream = True ' Begin reading the file and stop when we reach the end
                    b1 = sr.ReadLine
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

                        If String.Compare(dd, rr, True) < 0 Then
                            Dim save As String = fileName2.Insert(fileName2.LastIndexOf("."), Now.ToString)
                            save = save.Replace(":", "")
                            save = save.Replace("/", "")
                            getweb(save, exe)
                            MessageBox.Show(save & vbCrLf & "BUID:" & m.Value & "のダウンロードが完了しました", "ダウンロード完了")
                        Else
                            MessageBox.Show("最新版です", "更新の確認")
                        End If

                        Exit Do
                    End If
                Loop
                sr.Close()
                txt.Close()

            Else
                MessageBox.Show("インターネットに接続されてません", "接続エラー")
            End If
    End Sub
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

End Class