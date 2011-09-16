Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports System.Net.Sockets

Public Class Form1


    Private Sub form1load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim trans As String = ""
        '前回インストールしたｐｒｘ
        If My.Application.Culture.Name = "ja-JP" Then
            trans = My.Resources.s1
        Else
            trans = My.Resources.s1_e
        End If
        If My.Settings.tempar.Contains("1.62") Then
            RadioButton1.Checked = True
        Else
            RadioButton2.Checked = True
        End If

        TextBox2.Text = My.Settings.pspipaddress
        TextBox1.Text = trans & vbCrLf & My.Settings.lastprx
        DomainUpDown1.Text = My.Settings.drivepath
        CheckBox1.Checked = My.Settings.lang
        CheckBox2.Checked = My.Settings.drivelock
        CheckBox3.Checked = My.Settings.useftp

        Me.FormBorderStyle = FormBorderStyle.FixedToolWindow

    End Sub


    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        My.Settings.pspipaddress = TextBox2.Text
        My.Settings.drivepath = DomainUpDown1.Text

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

        ProgressBar1.Value = 0
        '応答データをファイルに書き込む
        Dim readData(5095) As Byte
        Dim readSize As Integer = 0
        Dim i As Integer
        While True
            readSize = strm.Read(readData, 0, readData.Length)
            If readSize = 0 Then
                ProgressBar1.Value = 100
                Exit While
            End If
            fs.Write(readData, 0, readSize)
            i += 1
            ProgressBar1.Value = i
            If i = 100 Then
                i = 0
            End If
        End While

        '閉じる
        fs.Close()
        strm.Close()

        Return True
    End Function

    Function unzip(ByVal zippath As String) As Boolean
        '展開先のフォルダのパス
        Dim extractDir As String = Application.StartupPath

        'ZIP書庫を読み込む 
        Dim fs As New System.IO.FileStream( _
            zippath, _
            System.IO.FileMode.Open, _
            System.IO.FileAccess.Read)
        'ZipInputStreamオブジェクトの作成 
        Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(fs)

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

    Function getdate(ByVal path As String) As String
        Dim prx As New System.IO.FileStream(path, _
System.IO.FileMode.Open, _
System.IO.FileAccess.Read)
        Dim bs(CInt(prx.Length - 1)) As Byte
        prx.Read(bs, 0, bs.Length)

        Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()

        'ハッシュ値を計算する 
        Dim temp As Byte() = md5.ComputeHash(bs)

        'byte型配列を16進数の文字列に変換 
        Dim result As New System.Text.StringBuilder()
        For Each b As Byte In temp
            result.Append(b.ToString("x2"))
        Next
        Dim md5hash As String = result.ToString

        prx.Close()
        Dim str(48) As Byte
        '42 75 69 6C 64
        Dim i As Integer = 0
        Dim k As Integer = 0
        While i < bs.Length - 5
            If bs(i) = &H42 AndAlso bs(i + 1) = &H75 AndAlso bs(i + 2) = &H69 AndAlso bs(i + 3) = &H6C AndAlso bs(i + 4) = &H64 Then
                While i + k < bs.Length
                    If bs(i + k) = &H0 Then
                        Exit While
                    End If
                    k += 1
                End While
                Array.ConstrainedCopy(bs, i, str, 0, k)
                Exit While
            End If
            i += 1
        End While
        Dim builddate As String = System.Text.Encoding.GetEncoding(0).GetString(str)
        Return builddate.Replace(vbNullChar, "") & vbCrLf & "MD5;" & md5hash
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = ""
        Dim trans As String = ""
        If My.Computer.Network.IsAvailable Then
            Dim installpath As String = findpsp()
            Dim temp() As String = {"\seplugins\TempAR\tempar.prx", "\seplugins\TempAR\tempar_lite.prx", "\seplugins\TempAR\tempar_autooff.prx"}
            Dim builddate As String = Nothing

            'サーバーのホスト名とポート番号
            Dim host As String = TextBox2.Text
            Dim port As Integer = 21

            If CheckBox3.Checked = True Then
                    'http://dobon.net/vb/dotnet/internet/receivepop3mail.html
                    'TcpClientを作成し、サーバーと接続する
                    Dim tcp As New TcpClient(host, port)
                    Dim ns As NetworkStream = tcp.GetStream
                    'NetworkStreamを取得する
                    Dim ftpdir As String = My.Settings.ftppath.Replace("\", "/")
                    '待ち時間
                    Thread.Sleep(100)

                    If ns.DataAvailable = True Then
                        Dim fileName As String = "tmp.zip"
                        getweb(fileName, My.Settings.tempar)
                        unzip(fileName)
                        builddate = getdate(Application.StartupPath & temp(0))
                        '"から応答がありました、送信を開始します"
                        If My.Application.Culture.Name = "ja-JP" Then
                            trans = My.Resources.s10
                        Else
                            trans = My.Resources.s10_e
                        End If
                        TextBox1.Text &= TextBox2.Text & trans & vbCrLf

                        'をPSPにコピーしています...
                        If My.Application.Culture.Name = "ja-JP" Then
                            trans = My.Resources.s3
                        Else
                            trans = My.Resources.s3_e
                        End If

                        TextBox1.Text &= "TEMPAR " & builddate & trans & vbCrLf


                        TextBox1.Text &= ReceiveData(ns)
                        SendData(ns, "PWD" & vbCrLf)
                        TextBox1.Text &= ReceiveData(ns)
                        SendData(ns, "USER " & "anonymous" & vbCrLf)
                        TextBox1.Text &= ReceiveData(ns)
                        SendData(ns, "PASS " & "anonymous" & vbCrLf)
                        TextBox1.Text &= ReceiveData(ns)
                        SendData(ns, "CWD " & ftpdir & vbCrLf)
                        TextBox1.Text &= ReceiveData(ns)
                        SendData(ns, "PWD" & vbCrLf)
                        TextBox1.Text &= ReceiveData(ns)
                        SendData(ns, "TYPE I" & vbCrLf)
                        TextBox1.Text &= ReceiveData(ns)

                        upload(ns, temp(0))
                        upload(ns, temp(1))
                        If RadioButton1.Checked = True Then
                            upload(ns, temp(2))
                        End If

                        SendData(ns, "QUIT" + vbCrLf)
                        ns.Close()
                        tcp.Close()
                        My.Settings.lastprx = builddate

                        '"アップロードが完了しました"
                        If My.Application.Culture.Name = "ja-JP" Then
                            trans = My.Resources.s11
                        Else
                            trans = My.Resources.s11_e
                        End If
                        TextBox1.Text &= trans & vbCrLf
                        System.Media.SystemSounds.Asterisk.Play()
                    Else
                        ns.Close()
                        tcp.Close()
                        'デーモンじゃない
                        If My.Application.Culture.Name = "ja-JP" Then
                            trans = My.Resources.s12
                        Else
                            trans = My.Resources.s12_e
                        End If
                        TextBox1.Text = TextBox2.Text & trans & vbCrLf
                        System.Media.SystemSounds.Exclamation.Play()
                    End If

            ElseIf installpath <> "" Then
                'PSPが見つかりました,temparのダウンロードを開始します
                If My.Application.Culture.Name = "ja-JP" Then
                    trans = My.Resources.s2
                Else
                    trans = My.Resources.s2_e
                End If
                TextBox1.Text = trans & vbCrLf
                Dim fileName As String = "tmp.zip"
                getweb(fileName, My.Settings.tempar)
                unzip(fileName)
                builddate = getdate(Application.StartupPath & temp(0))

                'をPSPにコピーしています...
                If My.Application.Culture.Name = "ja-JP" Then
                    trans = My.Resources.s3
                Else
                    trans = My.Resources.s3_e
                End If
                TextBox1.Text &= "TEMPAR " & builddate & trans & vbCrLf
                For i = 0 To 1
                    File.Copy(Application.StartupPath & temp(i), installpath & My.Settings.usbpath & temp(i).Replace("\seplugins\TempAR", ""), True)
                Next
                If My.Settings.tempar.Contains("1.62") Then
                    File.Copy(Application.StartupPath & temp(2), installpath & My.Settings.usbpath & temp(2).Replace("\seplugins\TempAR", ""), True)
                End If
                If My.Settings.tempar.Contains("1.63") AndAlso CheckBox1.Checked = True Then
                    '"ランゲージファイルをコピーしています"
                    If My.Application.Culture.Name = "ja-JP" Then
                        trans = My.Resources.s4
                    Else
                        trans = My.Resources.s4_e
                    End If
                    TextBox1.Text &= trans & vbCrLf
                    Dim temptxt As String = My.Settings.usbpath & "\languages\language"
                    temptxt = temptxt.Replace("/", "\")
                    For k = 1 To 2
                        File.Copy(Application.StartupPath & "\seplugins\TempAR\languages\language" & k.ToString & ".bin", installpath & temptxt & k.ToString & ".bin", True)
                    Next
                End If
                '"インストールが完了しました"
                If My.Application.Culture.Name = "ja-JP" Then
                    trans = My.Resources.s5
                Else
                    trans = My.Resources.s5_e
                End If
                TextBox1.Text &= trans
                My.Settings.lastprx = builddate
                System.Media.SystemSounds.Asterisk.Play()
            Else
                '"メモリースティックフォーマット時自動生成されるPSPフォルダとMEMSTICK.INDが見つかりませんでした"
                If My.Application.Culture.Name = "ja-JP" Then
                    trans = My.Resources.s6
                Else
                    trans = My.Resources.s6_e
                End If
                TextBox1.Text = trans & vbCrLf

                '"隠しファイルMEMSTICK.INDがない場合はメモリースティックのルートに作成してください"
                If My.Application.Culture.Name = "ja-JP" Then
                    trans = My.Resources.s7
                Else
                    trans = My.Resources.s7_e
                End If
                TextBox1.Text &= trans
                ProgressBar1.Value = 0
                System.Media.SystemSounds.Exclamation.Play()
            End If
            Else
                '"インターネットに接続されてません"
                If My.Application.Culture.Name = "ja-JP" Then
                    trans = My.Resources.s8
                Else
                    trans = My.Resources.s8_e
                End If
                TextBox1.Text = trans
                ProgressBar1.Value = 0

                System.Media.SystemSounds.Exclamation.Play()
            End If
    End Sub

    Function upload(ByVal ns As NetworkStream, ByVal tmpath As String) As Boolean
        Dim sendp As String = ""
        Dim host As String = TextBox2.Text
        Dim p As Integer = 0
        Dim q As Integer = 0
        Dim s As String = ""
        Dim inst As Integer = tmpath.LastIndexOf("\") + 1
        Dim prxname As String = tmpath.Substring(inst, tmpath.Length - inst)
        Dim UpLoadStream As NetworkStream

        SendData(ns, "PASV" & vbCrLf)
        'pasvのぽーとげと、あくてぃぶはPORTにかえるだけ
        'http://www.java2s.com/Tutorial/VB/0400__Socket-Network/FtpClientinVBnet.htm
        sendp = ReceiveData(ns).Replace(")", "")
        TextBox1.Text &= sendp.Replace("(", "")
        p = sendp.LastIndexOf(",")
        s = sendp.Substring(p + 1, sendp.Length - p - 1)
        sendp = sendp.Remove(p)
        p = CInt(s)
        q = sendp.LastIndexOf(",")
        s = sendp.Substring(q + 1, sendp.Length - q - 1)
        q = CInt(s) * 256
        'ftpdのしようのためseplugins/tempar　は使えないっぽいのでるーと+TEMPARしかないっぽい
        Dim prx As New FileStream(Application.StartupPath & tmpath, FileMode.Open, FileAccess.Read)
        Dim bs(CInt(prx.Length - 1)) As Byte
        prx.Read(bs, 0, bs.Length)
        prx.Close()
        Dim data As New TcpClient()
        data.Connect(host, p + q)
        UpLoadStream = data.GetStream
        SendData(ns, "STOR " & prxname & vbCrLf)
        TextBox1.Text &= ReceiveData(ns)
        UpLoadStream.Write(bs, 0, bs.Length)
        UpLoadStream.Close()
        data.Close()
        TextBox1.Text &= ReceiveData(ns)

        Return True
    End Function

    Private Overloads Shared Function ReceiveData( _
        ByVal stream As NetworkStream, _
        ByVal multiLines As Boolean, _
        ByVal bufferSize As Integer, _
        ByVal enc As Encoding) As String
        Dim data(bufferSize - 1) As Byte
        Dim len As Integer = 0
        Dim msg As String = ""
        Dim ms As New System.IO.MemoryStream

        'すべて受信する
        '(無限ループに陥る恐れあり)
        Do
            '受信
            len = stream.Read(data, 0, data.Length)
            ms.Write(data, 0, len)
            '文字列に変換する
            msg = enc.GetString(ms.ToArray())
        Loop While stream.DataAvailable OrElse _
            ((Not multiLines OrElse msg.StartsWith("-ERR")) AndAlso _
                Not msg.EndsWith(vbCrLf)) OrElse _
            (multiLines AndAlso Not msg.EndsWith(vbCrLf + "." + vbCrLf))

        ms.Close()

        '"-ERR"を受け取った時は例外をスロー
        If msg.StartsWith("-ERR") Then
            Throw New ApplicationException("Received Error")
        End If

        Return msg
    End Function
    Private Overloads Shared Function ReceiveData( _
            ByVal stream As NetworkStream, _
            ByVal multiLines As Boolean, _
            ByVal bufferSize As Integer) As String
        Return ReceiveData(stream, multiLines, bufferSize, _
            Encoding.GetEncoding(0))
    End Function
    Private Overloads Shared Function ReceiveData( _
            ByVal stream As NetworkStream, _
            ByVal multiLines As Boolean) As String
        Return ReceiveData(stream, multiLines, 256)
    End Function

    Private Overloads Shared Function ReceiveData( _
            ByVal stream As NetworkStream) As String
        Return ReceiveData(stream, False)
    End Function

    Private Overloads Shared Sub SendData( _
        ByVal stream As NetworkStream, _
        ByVal msg As String, _
        ByVal enc As Encoding)
        'byte型配列に変換
        Dim data As Byte() = enc.GetBytes(msg)
        '送信
        stream.Write(data, 0, data.Length)
    End Sub

    Private Overloads Shared Sub SendData( _
            ByVal stream As NetworkStream, _
            ByVal msg As String)
        SendData(stream, msg, Encoding.ASCII)
    End Sub

    Function findpsp() As String
        Dim PSP As String = " :\PSP"
        Dim driveletter As Integer = &H44 'drivepath D～Z
        Dim i As Integer
        For i = 0 To 22
            PSP = PSP.Remove(0, 1)
            PSP = PSP.Insert(0, Chr(driveletter))
            driveletter += 1
            If CheckBox2.Checked = True Then
                PSP = DomainUpDown1.Text & "\PSP"
            Else
                DomainUpDown1.Text = PSP.Substring(0, 2)
            End If
            If My.Computer.FileSystem.DirectoryExists(PSP) AndAlso File.Exists(PSP.Substring(0, 2) & "MEMSTICK.IND") Then
                PSP = PSP.Substring(0, 2)
                Dim prxpath As String = My.Settings.usbpath.Replace("/", "\")
                Dim langpath As String = prxpath & "\languages"
                If Not File.Exists(PSP & prxpath) Then
                    System.IO.Directory.CreateDirectory(PSP & prxpath)
                End If
                If CheckBox1.Checked = True AndAlso Not File.Exists(PSP & langpath) Then
                    System.IO.Directory.CreateDirectory(PSP & langpath)
                End If

                My.Settings.drivepath = PSP
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = "TEMPAR"
        If RadioButton1.Checked = True Then
            TextBox1.Text &= "1.62-3 "
        Else
            TextBox1.Text &= "1.63 "
        End If
        Dim fileName As String = "tmp.zip"
        getweb(fileName, My.Settings.tempar)
        unzip(fileName)
        Dim trans As String
        '" がリリースされてます"
        If My.Application.Culture.Name = "ja-JP" Then
            trans = My.Resources.s9
        Else
            trans = My.Resources.s9_e
        End If
        Dim s As String = getdate(Application.StartupPath & "\seplugins\TempAR\tempar.prx")
        TextBox1.Text &= s & trans
        System.Media.SystemSounds.Asterisk.Play()
    End Sub

    Private Sub DomainUpDown1_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DomainUpDown1.SelectedItemChanged
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged

        If CheckBox2.Checked = True Then
            DomainUpDown1.ReadOnly = True
            My.Settings.drivelock = True
        Else
            DomainUpDown1.ReadOnly = False
            My.Settings.drivelock = False
        End If


    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged

        If CheckBox3.Checked = True Then
            My.Settings.useftp = True
        Else
            My.Settings.useftp = False
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox3.Checked = True Then
            My.Settings.lang = True
        Else
            My.Settings.lang = False
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        Dim trans As String = ""

        Dim r As New Regex("^1(0|72|92)\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$")
        Dim m As Match = r.Match(TextBox2.Text)
        If m.Success Then

        Else
            'IP
            If My.Application.Culture.Name = "ja-JP" Then
                trans = My.Resources.s13
            Else
                trans = My.Resources.s13_e
            End If
            TextBox1.Text = trans
        End If
    End Sub

    Private Sub カスタマイズCToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles customdir.Click
        Dim c As New Form2
        c.ShowDialog()
        c.Dispose()
    End Sub

    Private Sub verinfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles verinfo.Click
        Dim v As New Form3
        v.ShowDialog()
        v.Dispose()
    End Sub

End Class
