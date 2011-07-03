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

        For Each s As String In b1
            If s.Length >= 2 Then
                If s.Substring(0, 2) = "_S" Or s.Substring(0, 2) = "_C" Then
                    g = 1
                End If

                If s.Length >= 7 Then
                    If s.Substring(0, 7) = "トラックバック" Then
                        b2 &= "_CW cwcwiki用,最後にある場合は消さないでください" & vbCrLf
                        'g = 0
                        wikiend = 1
                    End If
                    If s.Substring(0, 6) = "抽出レス数：" Then
                        b2 &= "_CJ JANE2CH用,最後にある場合は消さないでください" & vbCrLf
                        jane2ch = 1
                    End If
                End If
                    If g = 1 Then
                    If s.Length >= 2 Then
                        If s.Substring(0, 2) = "_C" Then
                            If i = 0 Then
                                b2 &= s.Trim & vbCrLf
                            ElseIf s.Substring(2, 1) = "D" Then
                            ElseIf s.Substring(2, 1) = "J" Then
                                b2 &= s.Trim & vbCrLf
                                jane2ch = 1
                            ElseIf s.Substring(2, 1) = "W" Then
                                b2 &= s.Trim & vbCrLf
                                wikiend = 1
                            Else
                                b2 &= s.Trim & vbCrLf
                            End If
                            code = 0
                            i += 1
                        End If
                        If s.Substring(0, 2) = "_L" Then
                            s = s.PadRight(24)
                            If s.Substring(3, 2) = "0x" And s.Substring(14, 2) = "0x" Then

                                s = System.Text.RegularExpressions.Regex.Replace( _
                        s, "[g-zG-Z]", "A")
                                s = s.ToUpper
                                s = System.Text.RegularExpressions.Regex.Replace( _
                        s, "_A ", "_L ")
                                s = s.Replace(" 0A", " 0x")
                                b2 &= s.Substring(0, 24) & vbCrLf
                                code = 1
                            End If
                        ElseIf code = 1 Then
                            '661 名前：名無しさん＠お腹いっぱい。[sage] 投稿日：2011/06/28(火) 19:45:22.31 ID:Nl1EJEAd
                            Dim r As New System.Text.RegularExpressions.Regex( _
                    "[0-9]+ 名前.+投稿日.+ID.+", _
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            Dim m As System.Text.RegularExpressions.Match = r.Match(s)
                            If m.Success Then
                                b2 &= s.Trim & vbCrLf
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

    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    Dim calet As Integer = TX.SelectionStart
    '    Dim b1 As String = TX.Text.Substring(0, calet)
    '    TX.Text = b1 & "_C0 " & TX.Text.Substring(calet)
    '    'TX.SelectionStart = calet + 3

    'End Sub

    'Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
    '    Dim calet As Integer = TX.SelectionStart
    '    '        Dim r As New System.Text.RegularExpressions.Regex( _
    '    '"#.+\n_L.+\n", _
    '    'System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '    Dim b1 As String = TX.Text
    '    Dim b2 As String() = b1.Split(CChar(vbLf))
    '    Dim strl As Integer = 0
    '    Dim cm As Integer = 0
    '    Dim cml As Integer = 0
    '    Dim back As Integer = 0
    '    For Each s As String In b2
    '        If strl > calet Then
    '            If s.Length >= 2 Then
    '                If s.Substring(0, 1) = "#" Then
    '                    cm = 1
    '                    back = s.Length
    '                ElseIf cm = 1 And s.Substring(0, 2) = "_L" Then
    '                    cml = 1
    '                    Exit For
    '                Else
    '                    cm = 0
    '                End If
    '            End If
    '        End If
    '        strl += s.Length
    '    Next
    '    TX.SelectionStart = strl
    'End Sub
End Class