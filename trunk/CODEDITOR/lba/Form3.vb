﻿Imports System
Imports System.Windows.Forms

Public Class Form3

    Private Sub ll(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        TextBox1.Text = CType(Me.Owner, Form1).fsbuf.Text
        TextBox2.Text = CType(Me.Owner, Form1).nodemax.Text
        TextBox3.Text = CType(Me.Owner, Form1).addlistmax.Text
        Dim st As String = CType(Me.Owner, Form1).SAVEMODE.Text

        If st = "S" Then
            SDIR.Checked = True
        End If
        If st = "F" Then
            FD.Checked = True
        End If
        Dim a As Integer = CInt(CType(Me.Owner, Form1).bool_exe.Text)
        If (a And 1) = 1 Then
            TXTBOOT.Checked = True
        End If
        If (a And 2) = 2 Then
            NOUMD.Checked = True
        End If

    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "保存先を選んで下さい"
        fbd.SelectedPath = CType(Me.Owner, Form1).sdir.Text
        fbd.ShowNewFolderButton = True
        If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
            CType(Me.Owner, Form1).sdir.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        CType(Me.Owner, Form1).fsbuf.Text = TextBox1.Text
        CType(Me.Owner, Form1).nodemax.Text = TextBox2.Text
        CType(Me.Owner, Form1).addlistmax.Text = TextBox3.Text
        CType(Me.Owner, Form1).vlistmax.Text = TextBox4.Text
        Dim a As Integer = 0
        If TXTBOOT.Checked = True Then
            a = 1
        End If
        If NOUMD.Checked = True Then
            a = a Or 2
        End If
        CType(Me.Owner, Form1).bool_exe.Text = a.ToString

        If APATH.Checked = True Then
            CType(Me.Owner, Form1).SAVEMODE.Text = "A"
        End If
        If SDIR.Checked = True Then
            CType(Me.Owner, Form1).SAVEMODE.Text = "S"
        End If
        If FD.Checked = True Then
            CType(Me.Owner, Form1).SAVEMODE.Text = "F"
        End If

        Me.Close()
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress, TextBox2.KeyPress, TextBox3.KeyPress, TextBox4.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) AndAlso e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Dim ofd As New OpenFileDialog
        ofd.Title = "起動するexeを選んで下さい"
        ofd.Filter = "EXEファイル(*.exe)|*.exe"

        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            CType(Me.Owner, Form1).exe.Text = ofd.FileName
        End If
    End Sub
End Class