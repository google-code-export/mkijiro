Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Class1

    Public Function converthtml(ByVal mode As Integer) As Boolean

        Dim bb As Byte() = Nothing
        Dim unicp(65535) As UInt16
        Dim unicpst(65535) As String
        Dim uni(65535) As String
        Dim enc As Integer = 932
        Dim ii As UInt16 = 0
        Dim th As String = My.Settings.tablest
        Dim tf = My.Settings.tableen
        Dim tst = My.Settings.tablerowst
        Dim tend = My.Settings.tablerowen
        Dim th1 = My.Settings.tableheaderst
        Dim th2 = My.Settings.tableheaderen
        Dim td1 = My.Settings.tabledatast
        Dim td2 = My.Settings.tabledataen

        Dim c1 As Integer = 0
        Dim c2 As Integer = 0
        Dim jis As Byte() = {&H1B, &H24, &H42}

        Dim unicodeview As Boolean = My.Settings.mstable
        Dim tk As String = My.Settings.tuni


        Dim sb As New StringBuilder

        If mode = 0 Then
            tk.Replace("JIS", "SJIS")
        ElseIf mode = 1 Then
            enc = 51932
            tk.Replace("JIS", "EUC")
        ElseIf mode = 2 Then
            enc = 50220
        ElseIf mode = 3 Then
            enc = 936
            tk.Replace("JIS", "GBK")
        End If

        For ii = 0 To 65534
            bb = BitConverter.GetBytes(ii)
            If ii < 256 Then
                bb(0) = CByte(ii And &HFF)
                uni(ii) = Encoding.GetEncoding(enc).GetString(bb)

                bb = Encoding.GetEncoding(12000).GetBytes(uni(ii))
                unicp(ii) = BitConverter.ToUInt16(bb, 0)
                If ii <> &H3F AndAlso unicp(ii) = &H3F Then
                    uni(ii) = ""
                    unicpst(ii) = ""
                ElseIf uni(ii) <> "" Then
                    unicpst(ii) = "<br>U+" & unicp(ii).ToString("X")
                    If unicp(ii) = &H30FB Then
                        uni(ii) = ""
                        unicpst(ii) = ""
                    End If

                End If
            ElseIf bb.Length = 2 Then
                bb(0) = CByte(ii >> 8)
                bb(1) = CByte(ii And &HFF)
                If mode = 2 Then
                    Array.Resize(bb, 5)
                    Array.Copy(bb, 0, bb, 3, 2)
                    Array.Copy(jis, 0, bb, 0, 3)
                End If

                uni(ii) = Encoding.GetEncoding(enc).GetString(bb)

                bb = Encoding.GetEncoding(12000).GetBytes(uni(ii))
                unicp(ii) = BitConverter.ToUInt16(bb, 0)

                If ii <> &H3F AndAlso unicp(ii) = &H3F Then
                    uni(ii) = ""
                    unicpst(ii) = ""
                ElseIf uni(ii) <> "" Then

                    unicpst(ii) = "<br>U+" & unicp(ii).ToString("X")
                    If unicp(ii) = &H30FB Then
                        uni(ii) = ""
                        unicpst(ii) = ""
                    End If

                End If
            End If
        Next

        sb.AppendLine(th)
        sb.AppendLine(tk)

        If mode = 0 Then
            For ii = 0 To &HFFFE
                c1 = ii >> 8
                c2 = ii And &HFF

                If (ii < 256 Or (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso c2 >= &H40 AndAlso c2 <= &HFF AndAlso c2 <> &H7F) Then

                    If ((ii And 15) = 0) Then
                        sb.AppendLine(tst)
                        sb.Append(th1)
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)
                    Else

                       If ii = &H8145 Then
                            uni(ii) = "・"
                            unicpst(ii) = "<br>U+30FB"
                        End If
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)

                        If ((ii And 15) = 15) Then
                            sb.AppendLine(tend)
                        End If
                    End If
                End If
            Next

        ElseIf mode = 1 Then
            For ii = 0 To &HFFFE
                c1 = ii >> 8
                c2 = ii And &HFF

                If (ii < 256 Or ((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= &HA0 AndAlso c2 <= &HFF) Then

                    If ((ii And 15) = 0) Then
                        sb.AppendLine(tst)
                        sb.Append(th1)
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)
                    Else
                        If ii = &HA1A6 Then
                            uni(ii) = "・"
                            unicpst(ii) = "<br>U+30FB"
                        End If
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)

                        If ((ii And 15) = 15) Then
                            sb.AppendLine(tend)
                        End If
                    End If
                End If
            Next


        ElseIf mode = 2 Then
            For ii = 0 To &HFFFE
                c1 = ii >> 8
                c2 = ii And &HFF

                If (ii < 256 Or (c1 >= &H21 AndAlso c1 <= &H92 AndAlso c2 >= &H20 AndAlso c2 <= &H7F)) Then

                    If ((ii And 15) = 0) Then
                        sb.AppendLine(tst)
                        sb.Append(th1)
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        'If ((ii And 255) = &H20) Then
                        '    uni(ii) = ""
                        'End If
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)
                    Else

                     If ii = &H2126 Then
                            uni(ii) = "・"
                            unicpst(ii) = "<br>U+30FB"
                        End If
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)

                        If ((ii And 15) = 15) Then
                            sb.AppendLine(tend)
                        End If
                    End If
                End If
            Next

        ElseIf mode = 3 Then
            For ii = 0 To &HFFFE
                c1 = ii >> 8
                c2 = ii And &HFF

                If (ii < 256 Or ((c1 + &H7F) And &HFF) < &H7E AndAlso c2 >= &H40 AndAlso c2 <= &HFF AndAlso c2 <> &H7F) Then

                    If ((ii And 15) = 0) Then
                        sb.AppendLine(tst)
                        sb.Append(th1)
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)
                    Else

                        If ii = &HA1A6 Then
                            uni(ii) = "・"
                            unicpst(ii) = "<br>U+30FB"
                        End If
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)

                        If ((ii And 15) = 15) Then
                            sb.AppendLine(tend)
                        End If
                    End If
                End If
            Next
        End If

        sb.AppendLine(tf)

        Dim sw As New System.IO.StreamWriter("test.html", False, System.Text.Encoding.GetEncoding(65001))
        sw.Write(sb.ToString)
        sw.Close()
        Process.Start("test.html")
        Return True
    End Function

End Class
