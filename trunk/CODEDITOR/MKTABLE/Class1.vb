Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Class1

    'htmltable作成
    Public Function txt_converthtml(ByVal mode As Integer, ByVal unic As Boolean, ByVal cpuni As Boolean) As Boolean
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
            Dim sblink As New StringBuilder

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
            Dim unicc As New Regex("(^0x[0-9A-fa-f]+|^&#x[0-9a-fA-F]+|&#[0-9]+|^U\+[0-9A-fa-f]+|^u\+[0-9A-fa-f]+|^\d\-[0-9A-fa-f]+)")
            Dim unicd As New Regex("(^\+[0-9A-fa-f]+)")
            Dim unim As Match
            Dim unidm As Match

            Dim jis As Boolean = My.Settings.jis
            Dim selmode As Integer = My.Settings.seljis
            Dim fakejis As Integer = 0
            Dim jisesp As Integer = 0

            Dim ofd As New OpenFileDialog()
            ofd.Filter = _
                "TXTファイル(*.txt)|*.txt"
            ofd.Title = "開くファイルを選択してください"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim ssr As New System.IO.StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding(65001))

                While ssr.Peek() > -1
                    s = ssr.ReadLine()
                    ssss = s.Split(CChar(vbTab))


                    If cpuni = True AndAlso ssss.Length >= 2 Then
                        st = ssss(1)
                        ssss(1) = ssss(0)
                        ssss(0) = st
                    End If


                    If ssss.Length > 1 Then
                        unim = unicc.Match(ssss(1))
                        If unim.Success Then
                            st = unim.Value
                            If st.Contains("x") Then
                                st = st.Replace("&#", "0")
                                sth = Convert.ToInt32(st, 16)
                            ElseIf st.Contains("-") Then
                                jisesp = Convert.ToInt32(st.Substring(0, 1), 16)
                                st = st.Remove(0, 2)
                                If jisesp = 4 Then
                                    jisesp = &H8F0000
                                End If
                                sth = Convert.ToInt32(st, 16) + jisesp
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


                        If jis = True Then
                            c2 = sth And &HFF
                            c1 = (sth >> 8) And &HFF
                            c3 = (sth >> 16) And &HFF
                            sjis = ucp

                            'sjis
                            If selmode = 0 Then
                                If (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso ((c2 + &HC0) And &HFF) < 189 AndAlso c2 <> &H7F Then
                                    If c1 >= &HF0 Then
                                        euc = 0
                                    End If
                                    c1 = ((c1 Xor &H20) << 1) - &H141 + &H20
                                    If (c2 >= &H9F) Then
                                        c1 += 1
                                    End If

                                    If ((c1 And 1) <> 0) Then
                                        If ((c2 And &H80) = 0) Then
                                            c2 += 1
                                        End If
                                        c2 -= &H20
                                    Else
                                        c2 -= &H7E
                                    End If
                                    If fakejis < c1 Then
                                        fakejis = c1
                                    End If

                                ElseIf c1 = 0 AndAlso c2 <= &H7F Then
                                    euc = 0

                                Else
                                    euc = 0
                                    c1 = 0
                                    c2 = 0
                                End If


                                'sjis2004
                            ElseIf selmode = 1 Then
                                If (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso ((c2 + &HC0) And &HFF) < 189 AndAlso c2 <> &H7F Then
                                    sjis = (c1 << 8) + c2
                                    hantei = 0

                                    If c1 >= &HF0 Then
                                        euc = &H8F
                                    End If

                                    'jisPLANE1
                                    If (sjis >= &H8140 AndAlso sjis <= &HEFFC) Then
                                        c1 = ((c1 Xor &H20) << 1) - &H141 + &H20

                                        If (c2 >= &H9F) Then
                                            c1 += 1
                                        End If

                                        If ((c1 And 1) <> 0) Then
                                            If ((c2 And &H80) = 0) Then
                                                c2 += 1
                                            End If
                                            c2 -= &H20
                                        Else
                                            c2 -= &H7E
                                        End If

                                        c3 = 0

                                        '第1バイトは (区番号 + 0x1DF) ÷ 2 － (区番号 ÷ 8) × 3
                                        '第1バイトは (区番号 + 0x19B) ÷ 2 
                                        '//int[] jjis = new int[] { 1, 8, 3, 4, 5, 12, 13, 14 ,15};
                                        'jisPLANE2
                                    ElseIf (sjis >= &HF040 AndAlso sjis <= &HF49E) Then

                                        c3 = 1
                                        hantei = ((c1 And &HF)) << 1
                                        If (c2 >= &H9F) Then
                                            hantei += 1
                                        End If

                                        c1 = ((hantei) And &HF) + 1

                                        If (c1 = 2) Then
                                            c1 = 8
                                        ElseIf (c1 >= 6) Then
                                            c1 += 6
                                        End If


                                        If ((c1 And 1) <> 0) Then

                                            If ((c2 And &H80) = 0) Then

                                                c2 += 1

                                            End If


                                            c2 -= &H20

                                        Else

                                            c2 -= &H7E

                                        End If

                                        c1 += &H20

                                        'jisPLANE2
                                    ElseIf (sjis >= &HF49F) Then
                                        euc = &H8F
                                        eucparse = True

                                        c1 = (2 * c1) - &H19B + &H20

                                        If (c2 >= &H9F) Then
                                            c1 += 1
                                        End If


                                        If ((c1 And 1) <> 0) Then

                                            If ((c2 And &H80) = 0) Then

                                                c2 += 1

                                            End If


                                            c2 -= &H20

                                        Else

                                            c2 -= &H7E

                                        End If

                                    Else
                                        c1 = 0
                                        c2 = 0
                                        euc = 0
                                    End If

                                ElseIf c1 = 0 AndAlso c2 <= &H7F Then
                                    euc = 0
                                Else
                                    c1 = 0
                                    c2 = 0
                                    euc = 0

                                End If

                                'fake euc cp20932
                            ElseIf selmode = 3 Then

                                If c1 >= &HA1 AndAlso c2 >= &H21 AndAlso c2 <= &H7E Then
                                    euc = &H8F
                                    eucparse = True
                                    c1 = c1 And &H7F
                                    c2 = c2 And &H7F

                                ElseIf c1 >= &HA1 AndAlso c2 >= &HA1 AndAlso c2 <= &HFE Then

                                    euc = 0
                                    c1 = c1 And &H7F
                                    c2 = c2 And &H7F
                                ElseIf c1 = 0 AndAlso c2 <= &H7F Then
                                    euc = 0
                                Else
                                    euc = 0
                                    c1 = 0
                                    c2 = 0
                                End If
                                'euc
                            ElseIf selmode = 2 Then

                                If c3 = &H8F Then
                                    euc = &H8F
                                    eucparse = True
                                    c1 = c1 And &H7F
                                    c2 = c2 And &H7F

                                ElseIf c1 >= &HA1 AndAlso c2 >= &HA1 Then

                                    euc = 0
                                    c1 = c1 And &H7F
                                    c2 = c2 And &H7F
                                ElseIf c1 = 0 AndAlso c2 <= &H7F Then
                                    euc = 0
                                Else
                                    euc = 0
                                    c1 = 0
                                    c2 = 0
                                End If


                            End If

                            ucp = c2 + (c1 << 8)

                        End If


                        unim = unicc.Match(ssss(0))
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

                    sblink.Append("<a href=""#ASCII"">ASCII</a> ")
                    sblink.Append("<a href=""#JISP1"">JISPLANE1</a> ")

                    Dim jislimit As Integer = &H7E
                    If fakejis > &H7E Then
                        jislimit = fakejis
                    End If

                    For ii = 0 To &HFFFE
                        c1 = ii >> 8
                        c2 = ii And &HFF

                        If ii = 129 Then
                            sb.AppendLine(tst)
                            sb.Append(th1)
                            sb.Append("<a name=""JISP1""></a>")
                            sb.Append("JIS PLANE1")
                            sb.AppendLine(th2)
                            sb.AppendLine(tend)
                        End If

                        If (ii < 128 Or (c1 >= &H21 AndAlso c1 <= jislimit AndAlso c2 >= &H20 AndAlso c2 <= &H7F)) Then

                            If ((ii And 15) = 0) Then
                                sb.AppendLine(tst)
                                sb.Append(th1)
                                If ii = 0 Then
                                    sb.Append("<a name=""ASCII""></a>")
                                End If
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

                    If eucparse = True Then
                        sblink.Append("<a href=""#JISP2"">JISPLANE2</a> ")

                        sb.AppendLine(tst)
                        sb.Append(th1)
                        sb.Append("<a name=""JISP2""></a>")
                        sb.Append("JIS PLANE2")
                        sb.AppendLine(th2)
                        sb.AppendLine(tend)

                        For ii = 0 To &HFFFE
                            c1 = ii >> 8
                            c2 = ii And &HFF

                            If ((c1 >= &H21 AndAlso c1 <= &H7E AndAlso c2 >= &H20 AndAlso c2 <= &H7F)) Then

                                If ((ii And 15) = 0) Then
                                    sb.AppendLine(tst)
                                    sb.Append(th1)
                                    sb.Append(ii.ToString("X"))
                                    sb.AppendLine(th2)
                                    sb.Append(td1)
                                    sb.Append(unip2(ii))
                                    If unicodeview = True Then
                                        sb.Append(unicpstp2(ii))
                                    End If
                                    sb.AppendLine(td2)
                                Else
                                    sb.Append(td1)
                                    sb.Append(unip2(ii))
                                    If unicodeview = True Then
                                        sb.Append(unicpstp2(ii))
                                    End If
                                    sb.AppendLine(td2)

                                    If ((ii And 15) = 15) Then
                                        sb.AppendLine(tend)
                                    End If
                                End If
                            End If
                        Next
                    End If
                    sb.Insert(0, sblink.ToString)

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
    Public Function converthtml(ByVal mode As Integer, ByVal unic As Boolean, ByVal enc As Integer) As Boolean

        Dim bb As Byte() = Nothing
        Dim unicp(65535) As UInt16
        Dim unicpst(65535) As String
        Dim uni(65535) As String
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
        Dim unicodeview As Boolean = unic
        Dim tk As String = My.Settings.thexval

        'Dim ascii As Byte() = {&H1B, &H28, &H40}
        Dim jis201roma As Byte() = {&H1B, &H28, &H4A}
        Dim jis201kana As Byte() = {&H1B, &H28, &H49}
        Dim jis73 As Byte() = {&H1B, &H24, &H40}

        Dim jis83 As Byte() = {&H1B, &H24, &H42}

        'Dim unicpstas(255) As String
        'Dim unias(255) As String

        Dim unicpstjisroma(255) As String
        Dim unijisroma(255) As String

        Dim unicpstjiskana(255) As String
        Dim unijiskana(255) As String

        Dim unicpstjis73(10496) As String
        Dim unijis73(10496) As String

        Dim tmp As Integer = 0
        Dim z As Integer = 0


        Dim sb As New StringBuilder
        Dim sb1 As New StringBuilder
        Dim sb2 As New StringBuilder
        Dim sb3 As New StringBuilder

        If mode = 0 Then
            tk.Replace("JIS", "SJIS")
        ElseIf mode = 1 Then
            tk.Replace("JIS", "EUC")
        ElseIf mode = 2 Then

        ElseIf mode = 3 Then
            tk.Replace("JIS", "GBK")
        End If

        For ii = 0 To 65534
            bb = BitConverter.GetBytes(ii)
            If ii < 256 Then
                bb(0) = CByte(ii And &HFF)

                uni(ii) = Encoding.GetEncoding(enc).GetString(bb)

                bb = Encoding.GetEncoding(12000).GetBytes(uni(ii))
                unicp(ii) = BitConverter.ToUInt16(bb, 0)


                If ii < &H20 Then
                    uni(ii) = ""
                    unicpst(ii) = "<br>U+" & ii.ToString("X")
                ElseIf ii <> &H3F AndAlso unicp(ii) = &H3F Then
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

                    If ii >= &H2120 AndAlso ii <= &H2821 Then
                        Array.Copy(jis73, 0, bb, 0, 3)
                        unijis73(ii) = Encoding.GetEncoding(enc).GetString(bb)
                        bb = Encoding.GetEncoding(12000).GetBytes(unijis73(ii))
                        tmp = BitConverter.ToUInt16(bb, 0)
                        If ii <> &H3F AndAlso tmp = &H3F Then
                            unijis73(ii) = ""
                            unicpstjis73(ii) = ""
                        ElseIf uni(ii) <> "" Then
                            unicpstjis73(ii) = "<br>U+" & tmp.ToString("X")
                            If tmp = &H30FB Then
                                unijis73(ii) = ""
                                unicpstjis73(ii) = ""
                            End If
                        End If
                    End If

                    Array.Resize(bb, 5)
                    bb(3) = CByte(ii >> 8)
                    bb(4) = CByte(ii And &HFF)
                    Array.Copy(jis83, 0, bb, 0, 3)

                End If

                uni(ii) = Encoding.GetEncoding(enc).GetString(bb)

                bb = Encoding.GetEncoding(12000).GetBytes(uni(ii))
                If bb.Length = 4 Then
                    unicp(ii) = BitConverter.ToUInt16(bb, 0)
                Else
                    unicp(ii) = &H3F
                End If

                If (ii <> &H3F AndAlso unicp(ii) = &H3F) Then
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
            Dim euc2b As Integer = &HA0

            If enc = 20932 Then
                euc2b = 32
            End If

            For ii = 0 To &HFFFE
                c1 = ii >> 8
                c2 = ii And &HFF

                If (ii < 256 Or (ii >= &H8EA0 AndAlso ii <= &H8EDF) Or ((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= euc2b AndAlso c2 <= &HFF) Then

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

                If (ii < 256) Then

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
                        sb.Append(td1)
                        sb.Append(uni(ii))
                        If unicodeview = True Then
                            sb.Append(unicpst(ii))
                        End If
                        sb.AppendLine(td2)

                        If ((ii And 15) = 15) Then
                            sb.AppendLine(tend)
                        End If


                        If ii = 255 Then
                            sb.AppendLine(tst)
                            sb3.AppendLine(th1)
                            sb3.AppendLine("//ESC $ @<br>") ' C 6226-1978	ESC 2/4 4/0	ESC $ @<br>")
                            sb3.AppendLine(th2)
                            sb3.AppendLine(tend)
                            z = sb.Length
                            sb.AppendLine(tst)
                            sb.AppendLine(th1)
                            sb.AppendLine("//ESC $ B<br>") ' X 0208-1983	ESC 2/4 4/2	ESC $ B<br>")
                            sb.AppendLine(th2)
                            sb.AppendLine(tend)
                        End If
                    End If

                ElseIf ((c1 >= &H21 AndAlso c1 <= &H97 AndAlso c2 >= &H20 AndAlso c2 <= &H7F)) Then

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

                        If ii >= &H2120 AndAlso ii <= &H287F Then
                            sb3.AppendLine(tst)
                            sb3.Append(th1)
                            sb3.Append(ii.ToString("X"))
                            sb3.AppendLine(th2)
                            sb3.Append(td1)
                            sb3.Append(unijis73(ii))
                            If unicodeview = True Then
                                sb3.Append(unicpstjis73(ii))
                            End If
                            sb3.AppendLine(td2)
                        End If

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

                        If ii >= &H2120 AndAlso ii <= &H287F Then
                            sb3.Append(td1)
                            sb3.Append(unijis73(ii))
                            If unicodeview = True Then
                                sb3.Append(unicpstjis73(ii))
                            End If
                            sb3.AppendLine(td2)

                            If ((ii And 15) = 15) Then
                                sb3.AppendLine(tend)
                            End If

                            If ii = &H287F Then
                                sb.Insert(z, sb3.ToString)
                            End If

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

            If enc = 54936 Then
                Dim stpos As Integer = 0
                Dim c4 As Integer = 0
                Dim cp As Integer = 0
                Dim kst As Integer = &H81308130
                Dim kmax As Integer = &H8339FE3F

                While (kst < &HFE39FE39)

                    For k = kst To kmax
                        c1 = (k >> 8) And &HFF
                        c2 = (k >> 16) And &HFF
                        c3 = k And &HF0

                        If (((c1 + &H7F) And &HFF) < &H7E) AndAlso (c2 >= &H30) AndAlso (c2 <= &H39) AndAlso (c3 = &H30) Then

                            Array.Resize(bb, 4)
                            bb(0) = CByte((k >> 24) And &HFF)
                            bb(1) = CByte((k >> 16) And &HFF)
                            bb(2) = CByte((k >> 8) And &HFF)
                            bb(3) = CByte(k And &HFF)

                            uni(stpos) = Encoding.GetEncoding(enc).GetString(bb)

                            bb = Encoding.GetEncoding(12000).GetBytes(uni(stpos))
                            If bb.Length = 4 Then
                                cp = BitConverter.ToInt32(bb, 0)

                            ElseIf bb.Length = 8 Then
                                cp = &H10000 + (BitConverter.ToInt32(bb, 0) And &H3FF) << 10 + (BitConverter.ToInt32(bb, 4) And &H3FF)
                            Else
                                cp = &H3F
                            End If

                            If cp = &H3F Then
                                uni(stpos) = ""
                                unicpst(stpos) = ""
                            ElseIf uni(stpos) <> "" Then

                                unicpst(stpos) = "<br>U+" & cp.ToString("X")
                                If cp = &H30FB Then
                                    uni(stpos) = ""
                                    unicpst(stpos) = ""
                                End If

                            End If

                            If ((k And 15) = 0) Then
                                sb.AppendLine(tst)
                                sb.Append(th1)
                                sb.Append(k.ToString("X"))
                                sb.AppendLine(th2)
                                sb.Append(td1)
                                sb.Append(uni(stpos))
                                If unicodeview = True Then
                                    sb.Append(unicpst(stpos))
                                End If
                                sb.AppendLine(td2)
                            Else
                                sb.Append(td1)
                                sb.Append(uni(stpos))
                                If unicodeview = True Then
                                    sb.Append(unicpst(stpos))
                                End If
                                sb.AppendLine(td2)

                                If ((k And 15) = 15) Then
                                    sb.AppendLine(tend)
                                End If
                            End If
                        End If
                    Next
                    kst += &H3000000
                    kmax += &H3000000

                    If kmax > &HFE39FE3F Then
                        kmax = &HFE39FE3F
                    End If
                End While
            End If

            End If

            sb.AppendLine(tf)

            Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\test.html", False, System.Text.Encoding.GetEncoding(65001))
        sw.Write(sb.ToString.Replace(vbNullChar, ""))
        sw.Close()

        If enc = 54936 Then
            If MessageBox.Show("GB18030はUNICODEBMPを含んでいるため表示に時間がかかります(目安クロームで１～2分）。このまま表示しますか？", "GB18030ベンチマーク警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Cancel Then
                Return True
            End If
        End If

            Process.Start(Application.StartupPath & "\test.html")

            Return True
    End Function


End Class
