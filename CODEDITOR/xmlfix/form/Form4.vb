Imports System.Text.RegularExpressions
Imports System.Text
Imports System.IO
Imports System

Public Class Form4

    Private Sub ff4load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If File.Exists(TextBox2.Text) Then
            Dim sr As New System.IO.StreamReader(TextBox2.Text, System.Text.Encoding.GetEncoding(65001))
            TextBox1.Text = sr.ReadToEnd
            sr.Close()
            TextBox1.SelectionStart = 0
        End If
        TextBox1.AcceptsTab = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If File.Exists(TextBox2.Text) Then
            Dim nb As New UTF8Encoding
            Dim sw As New System.IO.StreamWriter(TextBox2.Text, False, nb)
            sw.Write(TextBox1.Text)
            sw.Close()
        End If
        Me.Close()
    End Sub

    Private Sub neverKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        Dim mask As New Regex("[<>\|\*\?:/\\\t]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        If m.Success Then
            e.Handled = True
            Beep()
        End If
    End Sub
End Class