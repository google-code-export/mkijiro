Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports System.Net.Sockets

Public Class Form1
    Friend error_fnt As String = ""

    Private Sub form1load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        drivelettter.Text = My.Settings.drivepath
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles INSTALL.Click

        Dim trans As String = ""
        Dim installpath As String = findpsp()
        Dim meinst() As String = {"ja_recovery.txt", "ja_vshmenu.txt", "ja_ftable.bin"}
        Dim proinst() As String = {"recovery_jp.txt", "satelite_jp.txt", "font_recovery.txt", "CP932_HIRA.pf", "CP932_KANA.pf"}
        Dim mode As String = "\CFWME用\"
        Dim hiragana As String = "ひらがなPSP\"
        If CFWME.Checked = False Then
            mode = "\CFWPRO用\"
        End If
        If KANA.Checked = True Then
            hiragana = "カタカナPSP\"
        End If

        If CFWPROGO.Checked = True Then
            hiragana = hiragana.Insert(7, "GO")
        End If

        If cfont.Checked = True AndAlso File.Exists(My.Settings.cfont) = False Then
            System.Media.SystemSounds.Beep.Play()
            MessageBox.Show("カスタムフォントが存在しません", "エラー")
        End If


        If installpath <> "" Then

            If My.Computer.FileSystem.DirectoryExists(installpath & "seplugins") = False Then
                My.Computer.FileSystem.CreateDirectory(installpath & "seplugins")
            End If

            If CFWME.Checked = True Then
                For i = 0 To 1
                    File.Copy(Application.StartupPath & mode & meinst(i), installpath & "seplugins\" & meinst(i), True)
                Next
                If cfont.Checked = True Then
                    File.Copy(My.Settings.cfont, installpath & "seplugins\" & meinst(2), True)
                Else
                    File.Copy(Application.StartupPath & mode & hiragana & meinst(2), installpath & "seplugins\" & meinst(2), True)
                End If
            Else
                For i = 0 To 1
                    File.Copy(Application.StartupPath & mode & proinst(i), installpath & "seplugins\" & proinst(i), True)
                Next

                If cfont.Checked = True Then
                    Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\tmp.txt", False, System.Text.Encoding.GetEncoding(0))
                    Dim psp As String = "ms0:/"
                    If CFWPROGO.Checked = True Then
                        psp = "ef0:/"
                    End If
                    sw.Write(psp & "seplugins/fonts/" & Path.GetFileNameWithoutExtension(My.Settings.cfont) & ".pf")
                    sw.Close()
                    File.Copy(Application.StartupPath & "\tmp.txt", installpath & "seplugins\" & proinst(2), True)
                Else
                    File.Copy(Application.StartupPath & mode & hiragana & proinst(2), installpath & "seplugins\" & proinst(2), True)
                End If

                If My.Computer.FileSystem.DirectoryExists(installpath & "seplugins\fonts") = False Then
                    My.Computer.FileSystem.CreateDirectory(installpath & "seplugins\fonts")
                End If

                If cfont.Checked = True Then
                    File.Copy(My.Settings.cfont, installpath & "seplugins\fonts\" & Path.GetFileNameWithoutExtension(My.Settings.cfont) & ".pf", True)
                ElseIf HIRA.Checked = True Then
                    File.Copy(Application.StartupPath & mode & proinst(3), installpath & "seplugins\fonts\" & proinst(3), True)
                Else
                    File.Copy(Application.StartupPath & mode & proinst(4), installpath & "seplugins\fonts\" & proinst(4), True)
                End If

            End If
            '"インストールが完了しました"
            If My.Application.Culture.Name = "ja-JP" Then
                trans = My.Resources.s5
            Else
                trans = My.Resources.s5_e
            End If
            System.Media.SystemSounds.Asterisk.Play()
            MessageBox.Show(trans, "インストール完了")
        Else
            '"メモリースティックフォーマット時自動生成されるPSPフォルダとMEMSTICK.INDが見つかりませんでした"
            If My.Application.Culture.Name = "ja-JP" Then
                trans = My.Resources.s6
            Else
                trans = My.Resources.s6_e
            End If

            '"隠しファイルMEMSTICK.INDがない場合はメモリースティックのルートに作成してください"
            If My.Application.Culture.Name = "ja-JP" Then
                trans &= My.Resources.s7
            Else
                trans &= My.Resources.s7_e
            End If
            System.Media.SystemSounds.Beep.Play()
            MessageBox.Show(trans, "エラー")
        End If
    End Sub


    Function findpsp() As String
        Dim PSP As String = " :\PSP"
        Dim driveletter As Integer = &H44 'drivepath D～Z
        Dim i As Integer
        For i = 0 To 22
            PSP = PSP.Remove(0, 1)
            PSP = PSP.Insert(0, Chr(driveletter))
            driveletter += 1
            If lockdriveletter.Checked = True Then
                PSP = drivelettter.Text & "\PSP"
            Else
                drivelettter.Text = PSP.Substring(0, 2)
            End If
            If My.Computer.FileSystem.DirectoryExists(PSP) AndAlso File.Exists(PSP.Substring(0, 2) & "\MEMSTICK.IND") Then
                PSP = PSP.Substring(0, 2)
                Dim prxpath As String = "seplugins"
                If Not File.Exists(PSP & prxpath) Then
                    System.IO.Directory.CreateDirectory(PSP & prxpath)
                End If

                My.Settings.drivepath = PSP
                Return PSP
            End If
        Next
        Return ""
    End Function


    Private Sub TextBox1_KeyPress(ByVal sender As Object, _
  ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack And e.KeyChar <> "."c Then
            e.Handled = True
        End If
    End Sub


    Private Sub CFWME_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CFWME.CheckedChanged, CFWPRO.CheckedChanged, CFWPROGO.CheckedChanged, CFWME.Click
        GroupBox2.Enabled = True
    End Sub

    Private Sub HIRA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HIRA.CheckedChanged, KANA.CheckedChanged
        GroupBox3.Enabled = True
    End Sub

    Private Sub cf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cfont.CheckedChanged
        If HIRA.Checked = False AndAlso KANA.Checked = False AndAlso cfont.Checked = True AndAlso File.Exists(My.Settings.cfont) = False Then
            System.Media.SystemSounds.Beep.Play()
            MessageBox.Show("指定されたフォントファイルが存在しません", "ファイルエラー")
            GroupBox3.Enabled = False
        End If
        If My.Settings.cfont = error_fnt Then
            System.Media.SystemSounds.Beep.Play()
            MessageBox.Show("指定されたフォントファイルは使用できません", "ファイルエラー")
            GroupBox3.Enabled = False
        End If
    End Sub

    Private Sub AUTOPSP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AUTOPSP.CheckedChanged, lockdriveletter.CheckedChanged
        INSTALL.Enabled = True
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ofd As New OpenFileDialog
        GroupBox3.Enabled = False
        ofd.Filter = "すべてのファイル|*.*"
        ofd.Title = "開くファイルを選択してください"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.cfont = ofd.FileName
            Dim fs As New System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            'ファイルを読み込むバイト型配列を作成する
            Dim bs(2048 + 18) As Byte
            fs.Read(bs, 0, bs.Length)
            fs.Close()
            If (BitConverter.ToInt32(bs, 0) = &H544E4F46) Then
                If bs(16) = 0 AndAlso bs(14) = 8 AndAlso bs(15) = 8 Then
                    Dim ws As New System.IO.FileStream("tmp.fnt", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    ws.Write(bs, 17, 2048)
                    ws.Close()
                    My.Settings.cfont = "tmp.fnt"
                    GroupBox3.Enabled = True
                ElseIf bs(16) > 0 Then
                    MessageBox.Show("全角FONTXは使用できません")
                    error_fnt = ofd.FileName
                Else
                    MessageBox.Show("8x8以外は使用できません")
                    error_fnt = ofd.FileName
                End If
            Else
                GroupBox3.Enabled = True
            End If
        End If

    End Sub
End Class
