Imports System.IO
Imports System.Text     'Encoding用
Imports System.Text.RegularExpressions

Public Class codepage

    Private Sub ini(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim m As Form1
        m = CType(Me.Owner, Form1)
        ComboBox1.SelectedIndex = checkcp(m.cpg)

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim cp As New Regex("^\d+", RegexOptions.ECMAScript)
        Dim cpm As Match = cp.Match(ComboBox1.Text)
        Dim m As New Form1
        m = CType(Me.Owner, Form1)
        If cpm.Success Then
            m.cpg = CInt(cpm.Value)
            m.sel = checkcp(CInt(cpm.Value))
            Me.Close()
        Else
            MessageBox.Show("コードページを指定して下さい。例;　932 SJIS")
        End If

    End Sub

    Dim mscp As Integer() = {0, 37, 437, 500, 708, 709, 710, 720, 737, 775, 850, 852, 855, 857, 858, 860, 861, 862, 863, 864, 865, 866, 869, 870, 874, 875, 932, 936, 949, 950, 1026, 1047, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149, 1200, 1201, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1361, 10000, 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10008, 10010, 10017, 10021, 10029, 10079, 10081, 10082, 12000, 12001, 20000, 20001, 20002, 20003, 20004, 20005, 20105, 20106, 20107, 20108, 20127, 20261, 20269, 20273, 20277, 20278, 20280, 20284, 20285, 20290, 20297, 20420, 20423, 20424, 20833, 20838, 20866, 20871, 20880, 20905, 20924, 20932, 20936, 20949, 21025, 21027, 21866, 28591, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28603, 28605, 29001, 38598, 50220, 50221, 50222, 50225, 50227, 50229, 50930, 50931, 50933, 50935, 50936, 50937, 50939, 51932, 51936, 51949, 51950, 52936, 54936, 57002, 57003, 57004, 57005, 57006, 57007, 57008, 57009, 57010, 57011, 65000, 65001}

    Function checkcp(ByVal cp As Integer) As Integer
        Dim len As Integer = mscp.Length - 1
        For i = 0 To len
            If cp = mscp(i) Then
                Return i
            End If
        Next

        Return 0

    End Function

End Class