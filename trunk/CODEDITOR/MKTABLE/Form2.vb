Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form2


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        TextBox1.Text = My.Settings.tablest
        TextBox2.Text = My.Settings.tableen
        TextBox3.Text = My.Settings.tablerowst
        TextBox4.Text = My.Settings.tablerowen
        TextBox5.Text = My.Settings.tableheaderst
        TextBox6.Text = My.Settings.tableheaderen
        TextBox7.Text = My.Settings.tabledatast
        TextBox8.Text = My.Settings.tabledataen
        TextBox9.Text = My.Settings.thexval
        ComboBox1.SelectedIndex = My.Settings.htmltblmode
        CheckBox1.Checked = My.Settings.mstable

    End Sub


    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        My.Settings.tablest = TextBox1.Text
        My.Settings.tableen = TextBox2.Text
        My.Settings.tablerowst = TextBox3.Text
        My.Settings.tablerowen = TextBox4.Text
        My.Settings.tableheaderst = TextBox5.Text
        My.Settings.tableheaderen = TextBox6.Text
        My.Settings.tabledatast = TextBox7.Text
        My.Settings.tabledataen = TextBox8.Text
        My.Settings.thexval = TextBox9.Text

        My.Settings.htmltblmode = ComboBox1.SelectedIndex
        My.Settings.mstable = CheckBox1.Checked
        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim cc As New Class1
        cc.converthtml(ComboBox1.SelectedIndex, CheckBox1.Checked)

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged

    End Sub
End Class