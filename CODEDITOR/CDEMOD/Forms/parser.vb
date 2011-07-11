Imports System.IO
Public Class parser

    Public Opener As Form
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TX_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TX.TextChanged

    End Sub

    Private Sub cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancel.Click
        Me.Close()
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim f As Main
        f = CType(Me.Owner, Main)
        f.cmt_tb.Text = TX.Text
        Me.Close()
    End Sub

    Private Sub clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear.Click
        TX.Text = Nothing
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim b1 As String() = TX.Text.Split(CChar(vbLf))
        Dim i As Integer = 0
        Dim g As Integer = 0
        Dim code As Integer = 0
        Dim wikiend As Integer = 0
        Dim jane2ch As Integer = 0
        Dim b2 As String = Nothing
        Dim cwcar As String = Nothing
        Dim rmmode As Integer = 0

        For Each s As String In b1
            If s.Length >= 2 Then
                If s.Substring(0, 2) = "_S" Or s.Substring(0, 2) = "_C" Then
                    g = 1
                End If

                If s.Length >= 7 Then
                    Dim t As New System.Text.RegularExpressions.Regex( _
            "トラックバック.[0-9]+.+リンク元.[0-9]+", _
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim w As System.Text.RegularExpressions.Match = t.Match(s)
                    If w.Success Then
                        b2 &= "_CW cwcwiki用,最後にある場合は消さないでください" & vbCrLf
                        s = ""
                        'g = 0
                        wikiend = 1
                    ElseIf s.Substring(0, 6) = "抽出レス数：" Then
                        b2 &= "_CJ JANE2CH用,最後にある場合は消さないでください" & vbCrLf
                        jane2ch = 1
                    End If
                End If
            If g = 1 Then
                If s.Length >= 2 Then
                    If s.Substring(0, 2) = "_C" Then
                        s = s.PadRight(3)
                        If i = 0 Then
                            If s.Substring(2, 1) = "!" Then
                                rmmode = 1
                            End If
                            b2 &= s.Trim & vbCrLf
                            code = 0
                        ElseIf s.Substring(2, 1) = "D" Then
                            code = 0
                        ElseIf s.Substring(2, 1) = "J" Then
                            b2 &= s.Trim & vbCrLf
                            jane2ch = 1
                            code = 0
                        ElseIf s.Substring(2, 1) = "W" Then
                            b2 &= s.Trim & vbCrLf
                            wikiend = 1
                            code = 0
                        ElseIf s.Substring(2, 1) = "!" Then
                            rmmode = 1
                        ElseIf s.Substring(2, 1) = "#" Then
                            rmmode = 0
                        Else
                            b2 &= s.Trim & vbCrLf
                            code = 0
                        End If
                        i += 1

                        '_L 0x12345678 0x12345678
                    ElseIf s.Substring(0, 2) = "_L" Or s.Substring(0, 2) = "_M" Or s.Substring(0, 2) = "_N" Then
                        cwcar = s.Substring(0, 3)
                        s = s.Replace(vbCr, "")
                        s = s.PadRight(24, "0"c)
                        If s.Substring(3, 2) = "0x" And s.Substring(14, 2) = "0x" Then

                            s = System.Text.RegularExpressions.Regex.Replace( _
                    s, "[g-zG-Z]", "A")
                            s = s.ToUpper
                            s = s.Replace("_A ", cwcar)
                            s = s.Replace(" 0A", " 0x")
                            b2 &= s.Substring(0, 24) & vbCrLf
                            code = 1
                            'popdb
                        ElseIf s.Substring(2, 1) = " " And s.Substring(11, 1) = " " Then
                            s = System.Text.RegularExpressions.Regex.Replace( _
                    s, "[g-zG-Z]", "A")
                            s = s.ToUpper
                            s = s.Replace("_A ", "_L ")
                            b2 &= s.Substring(0, 16) & vbCrLf
                            code = 1
                        End If
                    ElseIf rmmode = 1 Then
                        s = s.Replace("#", "")
                        b2 &= s.Trim & vbCrLf
                    ElseIf code = 1 Then
                        '661 名前：名無しさん＠お腹いっぱい。[sage] 投稿日：2011/06/28(火) 19:45:22.31 ID:Nl1EJEAd
                        Dim r As New System.Text.RegularExpressions.Regex( _
                "[0-9]+ 名前.+投稿日.+ID.+", _
                System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        Dim m As System.Text.RegularExpressions.Match = r.Match(s)

                        Dim p As New System.Text.RegularExpressions.Regex( _
                "-{60}", _
                System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        Dim l As System.Text.RegularExpressions.Match = p.Match(s)
                        If m.Success Then
                            b2 &= s.Trim & vbCrLf
                        ElseIf l.Success Then

                        Else
                            s = s.Replace("#", "")
                            b2 &= "#" & s.Trim & vbCrLf
                        End If

                    End If
                End If
            End If
            End If
        Next
        If jane2ch = 0 And wikiend = 0 Then
            b2 &= "_CD パーサー用ダミー,最後にある場合は消さないでください" & vbCrLf
        End If
        TX.Text = b2
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim b1 As String() = TX.Text.Split(CChar(vbLf))
        Dim b2 As String = Nothing
        Dim bc2 As String = Nothing

        For Each s As String In b1
            If s.Length > 12 Then
                Dim p As New System.Text.RegularExpressions.Regex( _
    "[0-9A-Fa-f]{8} [0-9A-Fa-f?]{4}", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim mp As System.Text.RegularExpressions.Match = p.Match(s)
                Dim cf As New System.Text.RegularExpressions.Regex( _
    "[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim mc As System.Text.RegularExpressions.Match = cf.Match(s)
                Dim ar As New System.Text.RegularExpressions.Regex( _
    "0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim ma As System.Text.RegularExpressions.Match = ar.Match(s)

                bc2 = s.Substring(0, 2)
                s = s.Trim
                If mc.Success And bc2 <> "_M" And bc2 <> "_N" Then
                    s = mc.Value
                    s = s.Replace("_L ", "")
                    s = s.Insert(0, "0x")
                    s = s.Insert(11, "0x")
                    s = "_L " & s & vbCrLf
                ElseIf mp.Success Then
                    s = mp.Value
                    s = s.Replace("?", "A")
                    s = s.Replace("_L ", "")
                    s = "_L " & s & vbCrLf
                ElseIf ma.Success And bc2 <> "_L" And bc2 <> "_N" Then
                    s = ma.Value
                    s = s.Replace("_M ", "")
                    s = "_M " & s & vbCrLf
                End If
            End If
            If s.Length >= 2 Then
                b2 &= s.Trim & vbCrLf
            End If
        Next
        TX.Text = b2
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim b1 As String() = TX.Text.Split(CChar(vbLf))
        Dim b2 As String = Nothing
        Dim bc1 As String = Nothing
        Dim bc2 As String = Nothing
        Dim bc3 As String = Nothing
        Dim rmmode As Integer

        For Each s As String In b1
            If s.Length >= 2 Then
                s = s.PadRight(3)
                bc1 = s.Substring(0, 1)
                bc2 = s.Substring(0, 2)
                bc3 = s.Substring(0, 3)

                If bc3 = "_CR" Then
                    rmmode = 1
                ElseIf bc3 = "_CM" Then
                    rmmode = 0
                End If

                '661 名前：名無しさん＠お腹いっぱい。[sage] 投稿日：2011/06/28(火) 19:45:22.31 ID:Nl1EJEAd
                Dim r As New System.Text.RegularExpressions.Regex( _
        "[0-9]+ 名前.+投稿日.+ID.+", _
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(s)
                Dim p As New System.Text.RegularExpressions.Regex( _
        "-{60}", _
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim l As System.Text.RegularExpressions.Match = p.Match(s)

                If m.Success Or bc1 = "#" Or bc2 = "_L" Or bc2 = "_M" Or bc2 = "_N" Then
                    s = s.Trim & vbCrLf
                ElseIf bc3 = "_CR" Or bc3 = "_CM" Or l.Success Then
                    s = ""
                ElseIf bc3 = "_CJ" Or bc3 = "_CW" Or bc3 = "_CD" Or bc3 = "_C#" Or bc3 = "_C!" Then

                ElseIf rmmode = 1 Then
                    s = s.Replace("_C0 ", "")
                    s = s.Replace("_C1 ", "")
                ElseIf bc3 = "_C0" Or bc3 = "_C1" Or bc2 = "_S" Or bc2 = "_G" Then
                Else
                    s = s.Replace("_C0", "")
                    s = s.Replace("_C1 ", "")
                    s = "_C0 " & s.Trim & vbCrLf
                End If
                b2 &= s.Trim & vbCrLf
            End If
        Next
        TX.Text = b2
    End Sub
End Class