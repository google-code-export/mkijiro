Imports System.Text.RegularExpressions

Public Class Form5

    Private Sub ff5load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Me.Text = "description" Then
            TextBox1.Text = My.Settings.descpattern

        ElseIf Me.Text = "romname" Then
            TextBox1.Text = My.Settings.romnamepattern
        Else
            TextBox1.Text = My.Settings.namepattern
        End If
        TextBox1.SelectionStart = 0
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        If Me.Text = "description" Then
            My.Settings.descpattern = TextBox1.Text

        ElseIf Me.Text = "romname" Then
            My.Settings.romnamepattern = TextBox1.Text
        Else
            My.Settings.namepattern = TextBox1.Text
        End If
        Me.Close()
    End Sub


    Private Sub neverKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        Dim mask As New Regex("[<>\|\*\?:/\\]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        If m.Success Then
            e.Handled = True
            Beep()
        End If
    End Sub

End Class