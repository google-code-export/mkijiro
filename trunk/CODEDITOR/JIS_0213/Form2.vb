Imports System.IO
Imports System.Text     'Encoding用
Imports System.Text.RegularExpressions

Public Class form2

    Private Sub ini(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SELECT_CP.SelectedIndex = My.Settings.usercpsel
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim cp As New Regex("^\d+", RegexOptions.ECMAScript)
        Dim cpm As Match = cp.Match(SELECT_CP.Text)
        Dim cpg As Integer = 0
        If cpm.Success Then
            cpg = CInt(cpm.Value)
            If checkcp(cpg) = True Then
                My.Settings.mscodepage = cpg
                My.Settings.usercp = cpg
                My.Settings.usercpsel = SELECT_CP.SelectedIndex
                My.Settings.cpstr = SELECT_CP.Text
                Me.Close()
            Else
                MessageBox.Show("CP" & cpm.Value & "は対応してないコードページです", "エラー")
            End If
        Else
            Me.Close()
        End If

    End Sub

    Private Function checkcp(ByVal cp As Integer) As Boolean
        Dim len As Integer = mscp.Length - 1
        For i = 0 To len
            If cp = mscp(i) Then
                SELECT_CP.SelectedIndex = i
                Return True
            End If
        Next
        Return False

    End Function

    Dim mscp As Integer() = {0, 37, 437, 500, 708, 709, 710, 720, 737, 775, 850, 852, 855, 857, 858, 860, 861, 862, 863, 864, 865, 866, 869, 870, 874, 875, 932, 936, 949, 950, 1026, 1047, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149, 1200, 1201, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1361, 10000, 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10008, 10010, 10017, 10021, 10029, 10079, 10081, 10082, 12000, 12001, 20000, 20001, 20002, 20003, 20004, 20005, 20105, 20106, 20107, 20108, 20127, 20261, 20269, 20273, 20277, 20278, 20280, 20284, 20285, 20290, 20297, 20420, 20423, 20424, 20833, 20838, 20866, 20871, 20880, 20905, 20924, 20932, 20936, 20949, 21025, 21027, 21866, 28591, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28603, 28605, 29001, 38598, 50220, 50221, 50222, 50225, 50227, 50229, 50930, 50931, 50933, 50935, 50936, 50937, 50939, 51932, 51936, 51949, 51950, 52936, 54936, 57002, 57003, 57004, 57005, 57006, 57007, 57008, 57009, 57010, 57011, 65000, 65001}

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles SELECT_CP.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack Then
            e.Handled = True
            Beep()
        End If
    End Sub

End Class