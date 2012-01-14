Public Class Form2

    Private Sub LinkLabel2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("http://python.org/download/releases/2.7.2/")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://code.google.com/p/procfw/downloads/detail?name=ciso.py&can=2&q=")
    End Sub
End Class