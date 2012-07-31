Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form2

    Private Sub ff(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim m As New Form1
        TextBox1.Text = "0x" & m.tmp.ToString("X")
        TextBox2.Text = "0x" & m.cmf.ToString("X")

    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim hex As New Regex("0x8[0-9A-Fa-f]+")
        Dim hexm As Match = hex.Match(TextBox1.Text)
        Dim k As Integer = 0
        Dim t As Integer = 0
        If hexm.Success Then
            k = Convert.ToInt32(hexm.Value, 16)
        End If
        hexm = hex.Match(TextBox2.Text)
        If hexm.Success Then
            t = Convert.ToInt32(hexm.Value, 16)
            If vaid(k) = True AndAlso vaid(t) = True Then
                CType(Me.Owner, Form1).tmp = k
                CType(Me.Owner, Form1).cmf = t
            Else
                MessageBox.Show("サブルーチンの仮アドレスは0x8000000～0x84000000のカーネルメモリ範囲内に設置して下さい")
                Exit Sub
            End If
        End If

        Me.Close()
    End Sub

    Private Function vaid(ByVal k As Integer) As Boolean
        If k >= &H8000000 AndAlso k < &H8400000 Then
            Return True
        End If
        Return False
    End Function
End Class