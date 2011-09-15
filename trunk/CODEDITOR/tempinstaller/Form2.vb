Public Class Form2


    Private Sub form1load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.usbpath.Replace("\", "/")
        TextBox2.Text = My.Settings.ftppath.Replace("\", "/")
        Me.FormBorderStyle = FormBorderStyle.FixedToolWindow

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Settings.usbpath = TextBox1.Text.Replace("/", "\")
        My.Settings.ftppath = TextBox2.Text.Replace("/", "\")
        Me.Close()

    End Sub

End Class