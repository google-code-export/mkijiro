Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports System.Net.Sockets

Public Class Form1

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

        If installpath <> "" Then

            If My.Computer.FileSystem.DirectoryExists(installpath & "seplugins") = False Then
                My.Computer.FileSystem.CreateDirectory(installpath & "seplugins")
            End If

            If CFWME.Checked = True Then
                For i = 0 To 1
                    File.Copy(Application.StartupPath & mode & meinst(i), installpath & "seplugins\" & meinst(i), True)
                Next
                File.Copy(Application.StartupPath & mode & hiragana & meinst(2), installpath & "seplugins\" & meinst(2), True)
            Else
                For i = 0 To 1
                    File.Copy(Application.StartupPath & mode & proinst(i), installpath & "seplugins\" & proinst(i), True)
                Next
                File.Copy(Application.StartupPath & mode & hiragana & proinst(2), installpath & "seplugins\" & proinst(2), True)

                If My.Computer.FileSystem.DirectoryExists(installpath & "seplugins\fonts") = False Then
                    My.Computer.FileSystem.CreateDirectory(installpath & "seplugins\fonts")
                End If

                If HIRA.Checked = True Then
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

    Private Sub AUTOPSP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AUTOPSP.CheckedChanged, lockdriveletter.CheckedChanged
        INSTALL.Enabled = True
    End Sub
End Class
