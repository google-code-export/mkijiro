Public Class Form3


    Private Sub form1load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://code.google.com/p/mkijiro/source/browse/#svn%2Ftrunk%2FCODEDITOR%2Ftempinstaller")
    End Sub

End Class