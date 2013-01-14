Public Class vita


    Private Sub ff(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.vitamask
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.vitamask = TextBox1.Text
        Me.Close()
    End Sub
End Class