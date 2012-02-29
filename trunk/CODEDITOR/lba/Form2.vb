Imports System.IO
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class ver

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://code.google.com/p/mkijiro/")
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim check As New checkupdate
        check.CDEupater("help")
    End Sub
End Class