Imports System
Imports System.Text
Imports System.IO

Public Class Form1


    Private Sub ff(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If My.Settings.Setting = True Then
            CheckBox1.Checked = True
        End If
        If Directory.Exists(My.Settings.last) = False Then
            My.Settings.last = Application.StartupPath
        End If
        ComboBox1.SelectedIndex = My.Settings.c
        ComboBox2.SelectedIndex = My.Settings.a
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If pro_python() = -1 Then
            Dim f As New Form2
            f.ShowDialog()
            f.Dispose()
            Exit Sub
        End If

        Dim ofd As New OpenFileDialog()
        ofd.Title = "ファイルを選んで下さい"
        ofd.InitialDirectory = My.Settings.last
        ofd.Filter = "ISOファイル(*.iso)|*iso"
        If ofd.ShowDialog() = DialogResult.OK Then
            My.Settings.last = Path.GetDirectoryName(ofd.FileName)
            Dim procso As New System.Diagnostics.ProcessStartInfo()
            procso.FileName = "ciso.py"
            procso.Arguments = "-c" & (ComboBox1.SelectedIndex + 1).ToString & " -a " & ComboBox2.SelectedIndex.ToString & " """ & ofd.FileName & """ """ & Path.GetFileNameWithoutExtension(ofd.FileName) & ".cso"""
            If CheckBox1.Checked = True Then
                procso.Arguments = procso.Arguments.Insert(0, "-m ")
            End If
            System.Diagnostics.Process.Start(procso)
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If pro_python() = -1 Then
            Dim f As New Form2
            f.ShowDialog()
            f.Dispose()
            Exit Sub
        End If

        Dim ofd As New OpenFileDialog()
        ofd.Title = "ファイルを選んで下さい"
        ofd.InitialDirectory = My.Settings.last
        ofd.Filter = "CSOファイル(*.cso)|*cso"
        If ofd.ShowDialog() = DialogResult.OK Then
            My.Settings.last = Path.GetDirectoryName(ofd.FileName)
            Dim procso As New System.Diagnostics.ProcessStartInfo()
            procso.FileName = "ciso.py"
            procso.Arguments = "-c0 """ & ofd.FileName & """ """ & Path.GetFileNameWithoutExtension(ofd.FileName) & ".iso"""
            If CheckBox1.Checked = True Then
                procso.Arguments = procso.Arguments.Insert(0, "-m ")
            End If
            System.Diagnostics.Process.Start(procso)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        My.Settings.Setting = CheckBox1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        My.Settings.c = ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        My.Settings.a = ComboBox2.SelectedIndex
    End Sub

    Function pro_python() As Integer
        If File.Exists("ciso.py") = False Then
            Return -1
        End If
        Return 0
    End Function
End Class
