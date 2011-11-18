Imports System.Text.RegularExpressions

Public Class Form2

    Friend Shared backpath As String = My.Settings.usbpath
    Friend Shared backpath2 As String = My.Settings.ftppath

    Private Sub form1load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.usbpath.Replace("\", "/")
        TextBox2.Text = My.Settings.ftppath.Replace("\", "/")

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Settings.usbpath = TextBox1.Text.Replace("/", "\")
        My.Settings.ftppath = TextBox2.Text.Replace("/", "\")
        Dim ftpdirlevel As String = My.Settings.ftppath.Substring(1, My.Settings.ftppath.Length - 1)

        Dim trans As String = ""
        'FTPdir警告
        If My.Application.Culture.Name = "ja-JP" Then
            trans = My.Resources.s14
        Else
            trans = My.Resources.s14_e
        End If

        If ftpdirlevel.Contains("\") AndAlso MessageBox.Show(Me, trans, "FTP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Me.Close()

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If Regex.IsMatch(TextBox1.Text, _
    "^/.*", System.Text.RegularExpressions.RegexOptions.ECMAScript) Then
            backpath = TextBox1.Text
        Else
            TextBox1.Text = backpath
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If Regex.IsMatch(TextBox2.Text, _
 "^/.*", System.Text.RegularExpressions.RegexOptions.ECMAScript) Then
            backpath2 = TextBox2.Text
        Else
            TextBox2.Text = backpath2
        End If
    End Sub
End Class