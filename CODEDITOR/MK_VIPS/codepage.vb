Imports System.IO
Imports System.Text     'Encoding用
Imports System.Text.RegularExpressions

Public Class codepage

    Private Sub ini(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim m As Form1
        m = CType(Me.Owner, Form1)
        ComboBox1.SelectedIndex = m.sel

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim cp As New Regex("^\d+", RegexOptions.ECMAScript)
        Dim cpm As Match = cp.Match(ComboBox1.Text)
        Dim m As New Form1
        m = CType(Me.Owner, Form1)
        If cpm.Success Then
            m.cpg = CInt(cpm.Value)
            m.sel = ComboBox1.SelectedIndex
        End If
        Me.Close()

    End Sub

End Class