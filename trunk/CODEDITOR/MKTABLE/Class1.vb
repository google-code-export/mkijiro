Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Class1

    'htmltable作成
    Public Function txt_converthtml(ByVal mode As Integer, ByVal unic As Boolean) As Boolean
        Try

            Dim bb As Byte() = Nothing
            Dim unicpst(65536) As String
            Dim uni(65536) As String
            Dim unicpstp2(65536) As String
            Dim unip2(65536) As String
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

            Dim s As String = ""

            Dim sjis As Integer = 0
            Dim hantei As Integer = 0
            Dim wc As Integer = 0
            Dim i As Integer = 0
            Dim n As Integer = 0
            Dim bbb As Byte() = Nothing
            Dim bbbb As Byte() = Nothing
            Dim dest As Integer = 0
            Dim llen As Integer = 0
            Dim over As Boolean = False

            Dim c1 As Integer = 0
            Dim c2 As Integer = 0
            Dim c3 As Integer = 0

            Dim unicodeview As Boolean = unic
            Dim tk As String = My.Settings.thexval

            Dim sb As New StringBuilder

            Dim st As String = ""
            Dim sth As Integer = 0
            Dim ucp As Integer = 0
            Dim euc As Integer = 0
            Dim eucparse As Boolean = False
            Dim sth2 As Integer = 0
            Dim sth3 As UInt64 = 0
            Dim swap(3) As Byte
            Dim ssss As String()
            Dim z As Integer = 0
            Dim len As Integer = 0
            Dim unicc As New Regex("(^0x[0-9A-fa-f]+|^&#x[0-9a-fA-F]+|&#[0-9]+|^U\+[0-9A-fa-f]+|^u\+[0-9A-fa-f]+)")
            Dim unicd As New Regex("(^\+[0-9A-fa-f]+)")
            Dim unim As Match
            Dim unidm As Match


            Dim ofd As New OpenFileDialog()
            ofd.Filter = _
                "TXTファイル(*.txt)|*.txt"
            ofd.Title = "開くファイルを選択してください"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim ssr As New System.IO.StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding(65001))

                While ssr.Peek() > -1
                    s = ssr.ReadLine()
                    ssss = s.Split(CChar(vbTab))

                    If ssss.Length > 1 Then
                        unim = unicc.Match(ssss(0))
                        If unim.Success Then
                            st = unim.Value
                            If st.Contains("x") Then
                                st = st.Replace("&#", "0")
                                sth = Convert.ToInt32(st, 16)
                            ElseIf st.Contains("u") Or st.Contains("U") Then
                                st = st.Remove(0, 1)
                                st = st.Replace("+", "")
                                sth = Convert.ToInt32(st, 16)
                            Else
                                st = st.Remove(0, 2)
                                sth = Convert.ToInt32(st)
                            End If

                            ucp = sth And &HFFFF
                            euc = sth >> 16
                        Else
                            ucp = -1
                            euc = 0
                        End If

                        unim = unicc.Match(ssss(1))
                        If unim.Success AndAlso ucp >= 0 Then
                            st = unim.Value
                            If st.Contains("x") Then
                                st = st.Replace("&#", "0")
                                sth = Convert.ToInt32(st, 16)
                            ElseIf st.Contains("u") Or st.Contains("U") Then
                                st = st.Remove(0, 1)
                                st = st.Replace("+", "")
                                sth = Convert.ToInt32(st, 16)
                            Else
                                st = st.Remove(0, 2)
                                sth = Convert.ToInt32(st)
                            End If

                            'サロゲートペア
                            If sth >= &HD800 And sth <= &HDBFF Then
                                unim = unim.NextMatch
                                If unim.Success Then
                                    st = unim.Value
                                    If st.Contains("x") Then
                                        st = st.Replace("&#", "0")
                                        sth2 = Convert.ToInt32(st, 16)
                                    ElseIf st.Contains("u") Or st.Contains("U") Then
                                        st = st.Remove(0, 1)
                                        st = st.Replace("+", "")
                                        sth2 = Convert.ToInt32(st, 16)
                                    Else
                                        st = st.Remove(0, 2)
                                        sth2 = Convert.ToInt32(st)
                                    End If
                                    If sth2 >= &HDC00 And sth2 <= &HDFFF Then
                                        sth = sth And &H3FF
                                        sth2 = sth2 And &H3FF
                                        sth = (sth << 10) + sth2 + &H10000
                                    Else
                                        ucp = &HFFFF
                                        sth = &H3F
                                    End If
                                Else
                                    ucp = &HFFFF
                                    sth = &H3F
                                End If
                            End If

                            bbb = BitConverter.GetBytes(sth)
                            If euc = &H8F Then
                                eucparse = True
                                unip2(ucp) = Encoding.GetEncoding(12000).GetString(bbb)
                                unicpstp2(ucp) = "<br>U+" & sth.ToString("X")
                            ElseIf euc = 0 Then
                                uni(ucp) = Encoding.GetEncoding(12000).GetString(bbb)
                                unicpst(ucp) = "<br>U+" & sth.ToString("X")
                            End If

                            unidm = unicd.Match(ssss(1).Replace(unim.Value, ""))
                            While unidm.Success
                                st = unidm.Value.Remove(0, 1)
                                sth = Convert.ToInt32(st, 16)
                                bbb = BitConverter.GetBytes(sth)
                                If euc = &H8F Then
                                    unip2(ucp) = Encoding.GetEncoding(12000).GetString(bbb)
                                    unicpstp2(ucp) = "<br>U+" & sth.ToString("X")
                                ElseIf euc = 0 Then
                                    uni(ucp) &= Encoding.GetEncoding(12000).GetString(bbb)
                                    unicpst(ucp) &= "<br>+" & sth.ToString("X")
                                End If
                                unidm = unidm.NextMatch
                            End While

                        End If
                    End If

                End While

                ssr.Close()
                sb.AppendLine(th)
                sb.AppendLine(tk)

                'sjis
                If mode = 0 Then
                    For ii = 0 To &HFFFE
                        c1 = ii >> 8
                        c2 = ii And &HFF

                        If (ii < 256 Or (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso c2 >= &H40 AndAlso c2 <= &HFF AndAlso c2 <> &H7F) Then

                            If ((ii And 15) = 0) Then
                                sb.AppendLine(tst)
                                sb.Append(th1)
                                If ((((ii >> 8) Xor &H20) + &H5F) And &HFF) < &H3C Then
                                    sb.Append("<a name=""")
                                    sb.Append((ii >> 8).ToString("X"))
                                    sb.Append("""></a>")
                                End If
                                sb.Append(ii.ToString("X"))
                                sb.AppendLine(th2)
                                sb.Append(td1)
                                If (((ii Xor &H20) + &H5F) And &HFF) < &H3C AndAlso ii < 256 Then
                                    sb.Append("<a href=""#")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append(""">")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append("</a>")
                                End If
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

                                If (((ii Xor &H20) + &H5F) And &HFF) < &H3C AndAlso ii < 256 Then
                                    sb.Append("<a href=""#")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append(""">")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append("</a>")
                                End If

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
                    'euc
                ElseIf mode = 1 Then
                    For ii = 0 To &HFFFE
                        c1 = ii >> 8
                        c2 = ii And &HFF

                        If (ii < 256 Or (ii >= &H8EA0 AndAlso ii <= &H8EDF) Or ((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= &HA0 AndAlso c2 <= &HFF) Then

                            If ((ii And 15) = 0) Then
                                sb.AppendLine(tst)
                                sb.Append(th1)
                                If ii = &H8EA0 Then
                                    sb.AppendLine("<a name=""8E""></a>")
                                ElseIf (ii >> 8) >= &HA1 Then
                                    sb.Append("<a name=""")
                                    sb.Append((ii >> 8).ToString("X"))
                                    sb.Append("""></a>")
                                End If
                                sb.Append(ii.ToString("X"))
                                sb.AppendLine(th2)
                                sb.Append(td1)
                                If ii >= &HA1 AndAlso ii <= &HFE Then
                                    sb.Append("<a href=""#")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append(""">")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append("</a>")
                                End If
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
                                If ii = &H8E Then
                                    sb.AppendLine("<a href=""#8E"">8E</a>")
                                ElseIf ii = &H8F And eucparse = True Then
                                    sb.AppendLine("<a href=""#8F"">8F</a>")
                                ElseIf ii >= &HA1 AndAlso ii <= &HFE Then
                                    sb.Append("<a href=""#")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append(""">")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append("</a>")
                                End If

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

                    If eucparse = True Then

                        Dim iii As Integer = 0
                        For iii = &H8FA1A0 To &H8FFEFF
                            c1 = (iii >> 8) And &HFF
                            c2 = iii And &HFF
                            c3 = iii And &HFFFF

                            If (((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= &HA0 AndAlso c2 <= &HFF) Then

                                If ((iii And 15) = 0) Then
                                    sb.AppendLine(tst)
                                    sb.Append(th1)
                                    If iii = &H8FA1A0 Then
                                        sb.Append("<a NAME=""8F""></a>")
                                    End If
                                    sb.Append(iii.ToString("X"))
                                    sb.AppendLine(th2)
                                    sb.Append(td1)
                                    sb.Append(unip2(c3))
                                    If unicodeview = True Then
                                        sb.Append(unicpstp2(c3))
                                    End If
                                    sb.AppendLine(td2)
                                Else
                                    sb.Append(td1)
                                    sb.Append(unip2(c3))
                                    If unicodeview = True Then
                                        sb.Append(unicpstp2(c3))
                                    End If
                                    sb.AppendLine(td2)

                                    If ((iii And 15) = 15) Then
                                        sb.AppendLine(tend)
                                    End If
                                End If
                            End If
                        Next
                    End If
                    'jis
                ElseIf mode = 2 Then
                    For ii = 0 To &HFFFE
                        c1 = ii >> 8
                        c2 = ii And &HFF

                        If (ii < 256 Or (c1 >= &H21 AndAlso c1 <= &H7E AndAlso c2 >= &H20 AndAlso c2 <= &H7F)) Then

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

                    'gbk/big5
                ElseIf mode = 3 Then
                    For ii = 0 To &HFFFE
                        c1 = ii >> 8
                        c2 = ii And &HFF

                        If (ii < 256 Or ((c1 + &H7F) And &HFF) < &H7E AndAlso c2 >= &H40 AndAlso c2 <= &HFF AndAlso c2 <> &H7F) Then

                            If ((ii And 15) = 0) Then
                                sb.AppendLine(tst)
                                sb.Append(th1)
                                If (ii >> 8) >= &H81 Then
                                    sb.Append("<a name=""")
                                    sb.Append((ii >> 8).ToString("X"))
                                    sb.Append("""></a>")
                                End If
                                sb.Append(ii.ToString("X"))
                                sb.AppendLine(th2)
                                sb.Append(td1)
                                If ii >= &H81 AndAlso ii <= &HFE Then
                                    sb.Append("<a href=""#")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append(""">")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append("</a>")
                                End If
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
                                If ii >= &H81 AndAlso ii <= &HFE Then
                                    sb.Append("<a href=""#")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append(""">")
                                    sb.Append((ii).ToString("X"))
                                    sb.Append("</a>")
                                End If
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

                Dim sw As New System.IO.StreamWriter("cvt.html", False, System.Text.Encoding.GetEncoding(65001))
                sw.Write(sb.ToString)
                sw.Close()
                Process.Start("cvt.html")
            End If


        Catch ex As Exception

        End Try

        Return True
    End Function

    'さんぷる
    Public Function converthtml(ByVal mode As Integer, ByVal unic As Boolean) As Boolean

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
        Dim c3 As Integer = 0
        Dim jis As Byte() = {&H1B, &H24, &H42}

        Dim unicodeview As Boolean = unic
        Dim tk As String = My.Settings.thexval


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
                If bb.Length = 4 Then
                    unicp(ii) = BitConverter.ToUInt16(bb, 0)
                Else
                    unicp(ii) = &H3F
                End If

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
                        If ((((ii >> 8) Xor &H20) + &H5F) And &HFF) < &H3C Then
                            sb.Append("<a name=""")
                            sb.Append((ii >> 8).ToString("X"))
                            sb.Append("""></a>")
                        End If
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        If (((ii Xor &H20) + &H5F) And &HFF) < &H3C AndAlso ii < 256 Then
                            sb.Append("<a href=""#")
                            sb.Append((ii).ToString("X"))
                            sb.Append(""">")
                            sb.Append((ii).ToString("X"))
                            sb.Append("</a>")
                        End If
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

                        If (((ii Xor &H20) + &H5F) And &HFF) < &H3C AndAlso ii < 256 Then
                            sb.Append("<a href=""#")
                            sb.Append((ii).ToString("X"))
                            sb.Append(""">")
                            sb.Append((ii).ToString("X"))
                            sb.Append("</a>")
                        End If

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

                If (ii < 256 Or (ii >= &H8EA0 AndAlso ii <= &H8EDF) Or ((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= &HA0 AndAlso c2 <= &HFF) Then

                    If ((ii And 15) = 0) Then
                        sb.AppendLine(tst)
                        sb.Append(th1)
                        If ii = &H8EA0 Then
                            sb.AppendLine("<a name=""8E""></a>")
                        ElseIf (ii >> 8) >= &HA1 Then
                            sb.Append("<a name=""")
                            sb.Append((ii >> 8).ToString("X"))
                            sb.Append("""></a>")
                        End If
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        If ii >= &HA1 AndAlso ii <= &HFE Then
                            sb.Append("<a href=""#")
                            sb.Append((ii).ToString("X"))
                            sb.Append(""">")
                            sb.Append((ii).ToString("X"))
                            sb.Append("</a>")
                        End If
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
                        If ii = &H8E Then
                            sb.AppendLine("<a href=""#8E"">8E</a>")
                        ElseIf ii >= &HA1 AndAlso ii <= &HFE Then
                            sb.Append("<a href=""#")
                            sb.Append((ii).ToString("X"))
                            sb.Append(""">")
                            sb.Append((ii).ToString("X"))
                            sb.Append("</a>")
                        End If

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
                        If (ii >> 8) >= &H81 Then
                            sb.Append("<a name=""")
                            sb.Append((ii >> 8).ToString("X"))
                            sb.Append("""></a>")
                        End If
                        sb.Append(ii.ToString("X"))
                        sb.AppendLine(th2)
                        sb.Append(td1)
                        If ii >= &H81 AndAlso ii <= &HFE Then
                            sb.Append("<a href=""#")
                            sb.Append((ii).ToString("X"))
                            sb.Append(""">")
                            sb.Append((ii).ToString("X"))
                            sb.Append("</a>")
                        End If
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
                        If ii >= &H81 AndAlso ii <= &HFE Then
                            sb.Append("<a href=""#")
                            sb.Append((ii).ToString("X"))
                            sb.Append(""">")
                            sb.Append((ii).ToString("X"))
                            sb.Append("</a>")
                        End If
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

        Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\test.html", False, System.Text.Encoding.GetEncoding(65001))
        sw.Write(sb.ToString)
        sw.Close()

        Process.Start(Application.StartupPath & "\test.html")

        Return True
    End Function

End Class
