Public Class Form2

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.mask
        TextBox2.Text = My.Settings.mask2
        If My.Settings.cmmask = False Then
            RadioButton2.Checked = True
        Else
            RadioButton1.Checked = True
        End If
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        My.Settings.mask = TextBox1.Text
        My.Settings.mask2 = TextBox2.Text
        Me.Close()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton2.CheckedChanged
        My.Settings.cmmask = False
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton1.CheckedChanged
        My.Settings.cmmask = True
    End Sub
End Class