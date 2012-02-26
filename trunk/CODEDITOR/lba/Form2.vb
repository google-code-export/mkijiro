Imports System.IO
Imports System.Windows.Forms


Public Class ver

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://code.google.com/p/mkijiro/")
    End Sub
End Class