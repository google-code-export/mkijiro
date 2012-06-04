Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Dim p As String() = {"sjis.txt", "euc-jp.txt", "gbk.txt", "utf16le.txt", "utf16be.txt", "table\euc-jp.dat", "table\shift-jis.dat", "table\gbk.dat", "unicode.txt", "", "", "", "", "", ""}
    Dim pp As String() = {"sjisvsgbk", "sjisvsutf8", "gbkvssjis", "gbkvsutf8", "eucvsgbk", "eucvsutf8"}

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ENCODE.KeyPress
        e.Handled = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Dim output As String = p(ENCODE.SelectedIndex)
            Dim mm As New Regex("CP\d+", RegexOptions.ECMAScript)
            Dim m As Match = mm.Match(ENCODE.Text)
            If m.Success Then
                Dim enc As Integer = CInt(m.Value.Remove(0, 2))
                If (ENCODE.SelectedIndex >= 5 AndAlso ENCODE.SelectedIndex <= 7) Then
                    Dim ucs2 As New System.IO.FileStream(output, System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Dim bs(65535 * 2) As Byte
                    Dim bb(1) As Byte
                    Dim s As String = ""
                    Dim bbb As Byte() = Nothing
                    If sp.Checked Then
                        Array.Resize(bb, 4)
                        For i = 0 To &H2FFFF
                            bb(0) = CByte(i And &HFF)
                            bb(1) = CByte((i >> 8) And &HFF)
                            bb(2) = CByte((i >> 16) And &HFF)
                            bb(3) = CByte((i >> 24) And &HFF)
                            s = System.Text.Encoding.GetEncoding(12000).GetString(bb)
                            bbb = System.Text.Encoding.GetEncoding(enc).GetBytes(s)
                            Array.Resize(bbb, 2)
                            ucs2.Write(bbb, 0, 2)
                        Next
                        For i = &HE0000 To &HE0FFF
                            bb(0) = CByte(i And &HFF)
                            bb(1) = CByte((i >> 8) And &HFF)
                            bb(2) = CByte((i >> 16) And &HFF)
                            bb(3) = CByte((i >> 24) And &HFF)
                            s = System.Text.Encoding.GetEncoding(12000).GetString(bb)
                            bbb = System.Text.Encoding.GetEncoding(enc).GetBytes(s)
                            Array.Resize(bbb, 2)
                            ucs2.Write(bbb, 0, 2)
                        Next

                    Else
                        For i = 0 To &HFFFF
                            bb(0) = CByte(i And &HFF)
                            bb(1) = CByte(i >> 8)
                            s = System.Text.Encoding.GetEncoding(1200).GetString(bb)
                            bbb = System.Text.Encoding.GetEncoding(enc).GetBytes(s)
                            Array.Resize(bbb, 2)
                            ucs2.Write(bbb, 0, 2)
                        Next
                    End If
                    ucs2.Close()
                    Beep()
                    'SJIS,GBK,EUC VS テーブル,フォントロード使い回し
                ElseIf ENCODE.SelectedIndex >= 10 Then
                    Dim twotwo As New System.IO.FileStream("table\" & pp((ENCODE.SelectedIndex - 10) * 2), System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Dim twofour As New System.IO.FileStream("table\" & pp((ENCODE.SelectedIndex - 10) * 2 + 1), System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Dim bb(1) As Byte
                    Dim bs(65535 * 2) As Byte
                    Dim bss(65535 * 4) As Byte
                    Dim s As String = ""
                    Dim c1 As Integer = 0
                    Dim c2 As Integer = 0
                    Dim bbb As Byte() = Nothing
                    Dim bbbb As Byte() = Nothing
                    Dim dest As Integer = 0
                    Dim len As Integer = 0
                    ' SJIS VS
                    If ENCODE.SelectedIndex = 10 Then
                        For i = &H8140 To &HFCFF
                            bb(1) = CByte(i And &HFF)
                            bb(0) = CByte(i >> 8)
                            c1 = i >> 8
                            c2 = i And &HFF
                            If c2 >= &H40 AndAlso (((c1 Xor &H20) + &H5F) And &HFF) < &H3C Then
                                s = System.Text.Encoding.GetEncoding(932).GetString(bb)
                                bbb = System.Text.Encoding.GetEncoding(936).GetBytes(s)
                                bbbb = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                                Array.Resize(bbb, 2)
                                Array.Resize(bbbb, 4)
                                dest = ((c1 Xor &H20) - &HA1) * 192 + c2 - &H40
                                Array.Copy(bbb, 0, bs, dest * 2, 2)
                                Array.Copy(bbbb, 0, bss, dest * 4, 4)
                            End If
                        Next
                        len = ((c1 Xor &H20) - &HA1 + 1) * 192
                    End If
                    'GBK VS
                    If ENCODE.SelectedIndex = 11 Then
                        For i = &H8140 To &HFEFF
                            bb(1) = CByte(i And &HFF)
                            bb(0) = CByte(i >> 8)
                            c1 = i >> 8
                            c2 = i And &HFF
                            If c2 >= &H40 Then 'AndAlso ((c1 + &H7F) And &HFF) < &H7E Then
                                s = System.Text.Encoding.GetEncoding(936).GetString(bb)
                                bbb = System.Text.Encoding.GetEncoding(932).GetBytes(s)
                                bbbb = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                                Array.Resize(bbb, 2)
                                Array.Resize(bbbb, 4)
                                dest = (c1 - &H81) * 192 + c2 - &H40
                                Array.Copy(bbb, 0, bs, dest * 2, 2)
                                Array.Copy(bbbb, 0, bss, dest * 4, 4)
                            End If
                        Next
                        len = (c1 - &H81 + 1) * 192
                    End If
                    'EUC VS
                    If ENCODE.SelectedIndex = 12 Then
                        For i = &HA1A1 To &HFCFF
                            bb(1) = CByte(i And &HFF)
                            bb(0) = CByte(i >> 8)
                            c1 = i >> 8
                            c2 = i And &HFF
                            If c2 >= &HA1 AndAlso c1 >= &HA1 Then
                                s = System.Text.Encoding.GetEncoding(51932).GetString(bb)
                                bbb = System.Text.Encoding.GetEncoding(936).GetBytes(s)
                                bbbb = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                                Array.Resize(bbb, 2)
                                Array.Resize(bbbb, 4)
                                dest = (c1 - &HA1) * 96 + c2 - &HA1
                                Array.Copy(bbb, 0, bs, dest * 2, 2)
                                Array.Copy(bbbb, 0, bss, dest * 4, 4)
                            End If
                        Next
                        len = (c1 - &HA1 + 1) * 96
                    End If


                    twotwo.Write(bs, 0, len * 2)
                    twofour.Write(bss, 0, len * 4)
                    twotwo.Close()
                    twofour.Close()
                    Beep()

                    'てきすとからいっぱい
                    ElseIf ENCODE.SelectedIndex = 8 Then
                        Dim ofd As New OpenFileDialog()
                        ofd.Filter = _
                            "TXTファイル(*.txt)|*.txt"
                        ofd.Title = "開くファイルを選択してください"
                        If ofd.ShowDialog() = DialogResult.OK Then
                            output = ofd.FileName
                            Dim sr As New System.IO.StreamReader(output, System.Text.Encoding.GetEncoding(65001))
                            Dim fsjis As New System.IO.FileStream("txt_table\sjis", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim fjis As New System.IO.FileStream("txt_table\jis", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim feuc As New System.IO.FileStream("txt_table\euc-jp", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim fube As New System.IO.FileStream("txt_table\utf16be", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim fule As New System.IO.FileStream("txt_table\utf16le", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim futf8 As New System.IO.FileStream("txt_table\utf8", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim usj As New System.IO.FileStream("txt_table\utf16_sjis", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim ue As New System.IO.FileStream("txt_table\utf16_euc", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim uj As New System.IO.FileStream("txt_table\utf16_jis", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim s As String = ""
                            Dim r As New Regex("0x[0-9A-F]{4}\t0x[0-9A-F]{4}\t0x[0-9A-F]{4}", RegexOptions.ECMAScript)
                            Dim hexm As Match
                            Dim sjis(1) As Byte
                            Dim jis(1) As Byte
                            Dim euc(1) As Byte
                            Dim bs1(65535 * 2) As Byte
                            Dim bs2(65535 * 2) As Byte
                            Dim bs3(65535 * 2) As Byte
                            Dim bs4(65535 * 2) As Byte
                            Dim utf16be(1) As Byte
                            Dim utf16le(1) As Byte
                            Dim null(4) As Byte
                            Dim utf8 As Byte()
                            Dim srg As Boolean = False
                            Dim x As UInt16 = 0
                            Dim y As UInt16 = 0
                            Dim z As UInt16 = 0
                            While sr.Peek() > -1
                                s = sr.ReadLine()
                                hexm = r.Match(s)
                                If hexm.Success AndAlso s.Contains("//") = False Then
                                    s = hexm.Value
                                    x = Convert.ToUInt16(s.Substring(0, 6), 16)
                                    y = Convert.ToUInt16(s.Substring(7, 6), 16)
                                    z = Convert.ToUInt16(s.Substring(14, 6), 16)
                                    sjis(1) = CByte(x And &HFF)
                                    sjis(0) = CByte(x >> 8)
                                    jis(1) = CByte(y And &HFF)
                                    jis(0) = CByte(y >> 8)
                                    If jis(0) = 0 Then
                                        sjis(0) = CByte(x And &HFF)
                                        sjis(1) = CByte(x >> 8)
                                        jis(0) = CByte(y And &HFF)
                                        jis(1) = CByte(y >> 8)
                                        If ((jis(0) + &H5F) And &HFF) < &H40 Then
                                            euc(0) = &H8E
                                            euc(1) = jis(0)
                                        Else
                                            euc(0) = jis(0)
                                            euc(1) = jis(1)
                                        End If
                                    Else
                                        euc(1) = CByte((y Or &H80) And &HFF)
                                        euc(0) = CByte((y >> 8) Or &H80)
                                    End If
                                    utf16be(1) = CByte(z And &HFF)
                                    utf16be(0) = CByte(z >> 8)
                                    utf16le(1) = utf16be(0)
                                    utf16le(0) = utf16be(1)
                                    s = Encoding.GetEncoding(1201).GetString(utf16be)
                                    utf8 = Encoding.GetEncoding(65001).GetBytes(s)
                                    Array.Resize(utf8, 4)
                                    fsjis.Write(sjis, 0, 2)
                                    fjis.Write(jis, 0, 2)
                                    feuc.Write(euc, 0, 2)
                                    fube.Write(utf16be, 0, 2)
                                    fule.Write(utf16le, 0, 2)
                                    futf8.Write(utf8, 0, 4)
                                    If (z < 65536) Then
                                        Array.Copy(jis, 0, bs1, z * 2, 2)
                                        Array.Copy(sjis, 0, bs2, z * 2, 2)
                                        Array.Copy(euc, 0, bs3, z * 2, 2)
                                    ElseIf (z < 65536 * 2) Then
                                        'sjis2004
                                        Array.Copy(sjis, 0, bs4, (z - &H10000) * 2, 2)
                                        srg = True
                                    End If
                                End If
                            End While

                            uj.Write(bs1, 0, 65535 * 2)
                            usj.Write(bs2, 0, 65535 * 2)
                            If srg = True Then
                                usj.Write(bs4, 0, 65535 * 2)
                            End If
                            ue.Write(bs3, 0, 65535 * 2)

                            fsjis.Write(null, 0, 2)
                            fjis.Write(null, 0, 2)
                            feuc.Write(null, 0, 2)
                            fube.Write(null, 0, 2)
                            fule.Write(null, 0, 2)
                            futf8.Write(null, 0, 4)
                            sr.Close()
                            fsjis.Close()
                            fjis.Close()
                            fule.Close()
                            fube.Close()
                            futf8.Close()
                            feuc.Close()
                            Beep()
                        End If
                        'utf32
                    ElseIf ENCODE.SelectedIndex = 9 Then
                        Dim ofd As New OpenFileDialog()
                        ofd.Filter = _
                            "TXTファイル(*.txt)|*.txt"
                        ofd.Title = "開くファイルを選択してください"
                        If ofd.ShowDialog() = DialogResult.OK Then
                            output = ofd.FileName
                            Dim sr As New System.IO.StreamReader(output, System.Text.Encoding.GetEncoding(65001))
                            Dim fjis As New System.IO.FileStream("txt_table\custom_utf32", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                            Dim s As String = ""
                            Dim r As New Regex("0x[0-9A-F]{2,6}\tU\+[0-9A-F]{4,6}\t", RegexOptions.ECMAScript)
                            Dim hexm As Match
                            Dim btbl(4 * (&H30FFF)) As Byte
                            Dim ss As String()
                            Dim bbb As Byte()
                            Dim sw(3) As Byte
                            Dim x As UInt32 = 0
                            Dim y As UInt32 = 0
                            Dim z As Integer = 0
                            While sr.Peek() > -1
                                s = sr.ReadLine()
                                hexm = r.Match(s)
                                If hexm.Success AndAlso s.Contains("//") = False Then
                                    s = hexm.Value.Replace("U+", "")
                                    ss = s.Split(CChar(vbTab))
                                    x = Convert.ToUInt32(ss(0), 16)
                                    y = Convert.ToUInt32(ss(1), 16)
                                    If x > &HFFFFFF Then
                                    ElseIf x > &HFFFF Then
                                        x = x << 8
                                    ElseIf x > &HFF Then
                                        x = x << 16
                                    Else
                                        x = x << 24
                                    End If
                                    bbb = BitConverter.GetBytes(x)
                                    Array.Resize(sw, bbb.Length)
                                    z = bbb.Length - 1
                                    For i = 0 To z
                                        sw(i) = bbb(z - i)
                                    Next
                                    If y > &H30000 Then
                                        y = CUInt(y - &HB0000)
                                    End If
                                    Array.Copy(sw, 0, btbl, y * 4, bbb.Length)
                                End If
                            End While
                            fjis.Write(btbl, 0, btbl.Length)
                            sr.Close()
                            fjis.Close()
                            Beep()
                        End If
                    ElseIf File.Exists(output) Then
                        Dim sr As New System.IO.FileStream(output, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                        Dim fs As New System.IO.FileStream("table\" & Path.GetFileNameWithoutExtension(output), System.IO.FileMode.Create, System.IO.FileAccess.Write)
                        Dim fss As New System.IO.FileStream("table\utf8", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                        Dim bs(CInt(sr.Length) - 1) As Byte
                        sr.Read(bs, 0, bs.Length)
                        Dim ss As String = System.Text.Encoding.GetEncoding(enc).GetString(bs)
                        Dim sss As String() = ss.Split(CChar(vbLf))
                        Dim i As Integer = 0
                        Dim bb As Byte() = Nothing
                        Dim bbb As Byte() = Nothing
                        If sp.Checked = False Then
                            For Each s As String In sss
                                s = s.Replace(vbCr, "")
                                bb = System.Text.Encoding.GetEncoding(enc).GetBytes(s)
                                bbb = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                                Array.Resize(bb, 2)
                                Array.Resize(bbb, 4)
                                fs.Write(bb, 0, 2)
                                fss.Write(bbb, 0, 4)
                            Next
                        End If

                        If EX.Checked = True Then
                            Dim s As String = ""
                            Dim st As String = ""
                            Dim sth As Integer = 0
                            Dim sth2 As Integer = 0
                            Dim sth3 As UInt64 = 0
                            Dim swap(3) As Byte
                            Dim ssss As String()
                            Dim z As Integer = 0
                            Dim len As Integer
                            Dim len2 As Integer
                            Dim uni As New Regex("(^0x[0-9A-fa-f]+|^&#x[0-9a-fA-F]+|&#[0-9]+|^u\+?[0-9A-fa-f]+|^U\+?[0-9A-fa-f]+)")
                            Dim unim As Match
                            Dim ssr As New System.IO.StreamReader("extra.txt", System.Text.Encoding.GetEncoding(65001))
                            While ssr.Peek() > -1
                                s = ssr.ReadLine()
                                ssss = s.Split(CChar(vbTab))
                                unim = uni.Match(ssss(0))
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
                                                MessageBox.Show(st & "はサロゲートペアではありません")
                                                sth = &H3F
                                            End If
                                        Else
                                            MessageBox.Show(st & "はサロゲートペアではありません")
                                            sth = &H3F
                                        End If
                                    End If
                                    bbb = BitConverter.GetBytes(sth)
                                    st = System.Text.Encoding.GetEncoding(12000).GetString(bbb)

                                    bbb = System.Text.Encoding.GetEncoding(65001).GetBytes(st)
                                Else
                                    bbb = System.Text.Encoding.GetEncoding(65001).GetBytes(ssss(0))
                                End If

                                unim = uni.Match(ssss(1))
                                If unim.Success Then
                                    st = unim.Value
                                    If st.Contains("x") Then
                                        st = st.Replace("&#", "0")
                                        sth3 = Convert.ToUInt64(st, 16)
                                    ElseIf st.Contains("u") Then
                                        st = st.Remove(0, 1)
                                        st = st.Replace("+", "")
                                        sth3 = Convert.ToUInt64(st, 16)
                                    Else
                                        st = st.Remove(0, 2)
                                        sth3 = Convert.ToUInt64(st)
                                    End If

                                    bb = BitConverter.GetBytes(sth3)
                                    Array.Resize(bb, 4)
                                    z = bb.Length - 1
                                    For i = 0 To z
                                        swap(i) = bb(z - i)
                                    Next

                                    If sth3 > &HFFFFFF Then
                                        bb(0) = swap(0)
                                        bb(1) = swap(1)
                                        bb(2) = swap(2)
                                        bb(3) = swap(3)
                                    ElseIf sth3 > &HFFFF Then
                                        Array.Resize(bb, 3)
                                        bb(0) = swap(1)
                                        bb(1) = swap(2)
                                        bb(2) = swap(3)
                                    ElseIf sth3 > &HFF Then
                                        Array.Resize(bb, 2)
                                        bb(0) = swap(2)
                                        bb(1) = swap(3)
                                    Else
                                        Array.Resize(bb, 2)
                                        bb(0) = swap(3)
                                        bb(1) = 0
                                    End If
                                Else
                                    bb = System.Text.Encoding.GetEncoding(enc).GetBytes(ssss(1))
                                End If
                                len = bb.Length
                                If len <= 2 Then
                                    Array.Resize(bb, 2)
                                    Array.Resize(bbb, 4)
                                    If BitConverter.ToUInt16(bb, 0) <> 0 AndAlso BitConverter.ToUInt32(bbb, 0) <> 0 Then
                                        fs.Write(bb, 0, 2)
                                        fss.Write(bbb, 0, 4)
                                    End If
                                Else
                                    len2 = len
                                    If (len And 1) = 1 Then
                                        len += 1
                                    End If
                                    Array.Resize(bb, len)
                                    fs.Write(bb, 0, len)
                                    Array.Resize(bbb, 4)
                                    fss.Write(bbb, 0, 4)
                                    Array.Clear(bbb, 0, 4)
                                    bbb(0) = CByte(len2)
                                    len = len >> 1
                                    For i = 0 To len - 2
                                        fss.Write(bbb, 0, 4)
                                    Next
                                End If
                            End While
                            If len <= 2 Then
                                Array.Resize(bbb, 4)
                                Array.Clear(bbb, 0, 4)
                                fs.Write(bbb, 0, 2)
                                bbb(0) = CByte(len)
                                fss.Write(bbb, 0, 4)
                            End If
                            ssr.Close()
                        End If

                        Array.Clear(bbb, 0, 4)
                        fs.Write(bbb, 0, 2)
                        fss.Write(bbb, 0, 4)
                        sr.Close()
                        fs.Close()
                        fss.Close()
                        Beep()
                    Else
                        MessageBox.Show(output & ",変換対象テキストがありません")
                    End If
                Else
                    MessageBox.Show("文字コードが指定できません")
                End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Dim mm As New Regex("CP\d+", RegexOptions.ECMAScript)
            Dim m As Match = mm.Match(ENCODE.Text)
            If m.Success Then
                Dim enc As Integer = CInt(m.Value.Remove(0, 2))
                Dim output As String = p(ENCODE.SelectedIndex)
                If ENCODE.SelectedIndex >= 6 Then
                    '                   Dim s As String = TextBox1.Text
                    '                   Dim fs As New System.IO.FileStream(output, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    '                   Dim bs(CInt(fs.Length - 1)) As Byte
                    '                   fs.Read(bs, 0, bs.Length)
                    '                   fs.Close()
                    '                   Dim utf8st As Byte() = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                    '                   Dim stm(256) As Byte
                    '                   Dim i As Integer = 0
                    '                   Dim k As Integer = 0
                    '                   Dim c As Integer = 0
                    '                   Dim r As Integer = 0
                    '                   Dim seek As Integer = 0
                    '                   Dim dummy As Byte() = {0, 32}
                    '                   If (textbox4.Text <> "") Then
                    '                       dummy = System.Text.Encoding.GetEncoding(enc).GetBytes(textbox4.Text)
                    '                   End If

                    '                   While i < utf8st.Length
                    '                       c = utf8st(i)

                    '                       If (c < &H80) Then
                    '                           stm(i) = 0
                    '                           stm(i + 1) = c
                    '                           r = 1
                    '                       ElseIf (c < &HC2) Then
                    '                           r = -1
                    '                       ElseIf (c < &HE0) Then
                    '                           If (n < 2) Then
                    '                               r = (-2 - (0))
                    '	if (!((s[1] ^ 0x80) < 0x40))
                    '                                   r = -1
                    '	*pwc = ((ucs4_t) (c & 0x1f) << 6)
                    '		| (ucs4_t) (s[1] ^ 0x80);
                    '                                   r = 2
                    'else if (c < 0xf0) {
                    '                                   If (n < 3) Then
                    '                                       r = (-2 - (0))
                    '	if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
                    '		&& (c >= 0xe1 || s[1] >= 0xa0)))
                    '                                           r = -1
                    '	*pwc = ((ucs4_t) (c & 0x0f) << 12)
                    '		| ((ucs4_t) (s[1] ^ 0x80) << 6)
                    '		| (ucs4_t) (s[2] ^ 0x80);
                    '                                           r = 3
                    '} else if (c < 0xf8 && sizeof(ucs4_t)*8 >= 32) {
                    '                                           If (n < 4) Then
                    '                                               r = (-2 - (0))
                    '	if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
                    '		&& (s[3] ^ 0x80) < 0x40
                    '		&& (c >= 0xf1 || s[1] >= 0x90)))
                    '                                                   r = -1
                    '	*pwc = ((ucs4_t) (c & 0x07) << 18)
                    '		| ((ucs4_t) (s[1] ^ 0x80) << 12)
                    '		| ((ucs4_t) (s[2] ^ 0x80) << 6)
                    '		| (ucs4_t) (s[3] ^ 0x80);
                    '                                                   r = 4
                    '} else if (c < 0xfc && sizeof(ucs4_t)*8 >= 32) {
                    '                                                   If (n < 5) Then
                    '                                                       r = (-2 - (0))
                    '	if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
                    '		&& (s[3] ^ 0x80) < 0x40 && (s[4] ^ 0x80) < 0x40
                    '		&& (c >= 0xf9 || s[1] >= 0x88)))
                    '                                                           r = -1
                    '	*pwc = ((ucs4_t) (c & 0x03) << 24)
                    '		| ((ucs4_t) (s[1] ^ 0x80) << 18)
                    '		| ((ucs4_t) (s[2] ^ 0x80) << 12)
                    '		| ((ucs4_t) (s[3] ^ 0x80) << 6)
                    '		| (ucs4_t) (s[4] ^ 0x80);
                    '                                                           r = 5
                    ' else if (c < 0xfe && sizeof(ucs4_t)*8 >= 32) then
                    '                                                           If (n < 6) Then
                    '                                                               r = (-2 - (0))
                    '	if (!((s[1] ^ 0x80) < 0x40 && (s[2] ^ 0x80) < 0x40
                    '		&& (s[3] ^ 0x80) < 0x40 && (s[4] ^ 0x80) < 0x40
                    '		&& (s[5] ^ 0x80) < 0x40
                    '		&& (c >= 0xfd || s[1] >= 0x84)))
                    '                                                                   r = -1
                    '	*pwc = ((ucs4_t) (c & 0x01) << 30)
                    '		| ((ucs4_t) (s[1] ^ 0x80) << 24)
                    '		| ((ucs4_t) (s[2] ^ 0x80) << 18)
                    '		| ((ucs4_t) (s[3] ^ 0x80) << 12)
                    '		| ((ucs4_t) (s[4] ^ 0x80) << 6)
                    '		| (ucs4_t) (s[5] ^ 0x80);
                    '                                                                   r = 6
                    '                                                               Else
                    '                                                                   r = -1
                    '                                                               End If
                    '                   End While


                    '                   TextBox2.Text = System.Text.Encoding.GetEncoding(enc).GetString(stm)
                    '                   s = ""
                    '                   For i = 0 To stm.Length - 1
                    '                       If enc < 1200 AndAlso enc > 1201 Then
                    '                           If stm(i) = 0 Then
                    '                               Exit For
                    '                           End If
                    '                       Else
                    '                           If (i And 1) = 0 AndAlso stm(i) = 0 AndAlso stm(i + 1) = 0 Then
                    '                               Exit For
                    '                           End If
                    '                       End If
                    '                       s &= stm(i).ToString("X2")
                    '                   Next

                    '                   TextBox3.Text = s

                Else
                    Dim enctable As String = "table\" & Path.GetFileNameWithoutExtension(output)
                    If File.Exists("table\utf8") AndAlso File.Exists(enctable) Then
                        Dim s As String = TextBox1.Text
                        Dim bb As Byte() = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                        Dim bs(2047) As Byte
                        Dim stm(256) As Byte
                        Dim dummy As Byte() = {32, 0}
                        Dim skiplen As Integer = 0
                        If (textbox4.Text <> "") Then
                            dummy = System.Text.Encoding.GetEncoding(enc).GetBytes(textbox4.Text)
                        End If
                        Dim seek As UInteger
                        Dim i As Integer = 0
                        Dim k As Integer = 0
                        Dim kk As Integer = 0
                        Dim tm As Integer = 0
                        Dim big As UInteger = 0
                        Dim fail As Boolean = False
                        Dim tofu As Boolean = False

                        While i < bb.Length
                            If bb(i) < &H80 Then
                                If enc = 1200 Then
                                    stm(k) = bb(i)
                                    stm(k + 1) = 0
                                    k += 2
                                ElseIf enc = 1201 Then
                                    stm(k + 1) = bb(i)
                                    stm(k) = 0
                                    k += 2
                                Else
                                    stm(k) = bb(i)
                                    k += 1
                                End If
                                i += 1
                            ElseIf bb(i) < &HC2 Then
                                Exit While
                            ElseIf bb(i) < &HF8 Then
                                If (bb(i) < &HE0) Then
                                    seek = CUInt(bb(i) + (bb(i + 1) * 256))
                                ElseIf (bb(i) < &HF0) Then
                                    seek = CUInt(bb(i) + (bb(i + 1) * 256) + (bb(i + 2) * 65536))
                                Else
                                    seek = BitConverter.ToUInt32(bb, i)
                                End If
                                kk = 0
                                fail = False
                                Dim fs As New System.IO.FileStream("table\utf8", System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                While True
                                    Dim readSize As Integer = fs.Read(bs, 0, bs.Length)
                                    For j = 0 To 512 - 1
                                        big = BitConverter.ToUInt32(bs, 4 * j)
                                        If seek = big Then
                                            kk += j
                                            skiplen = bs(4 * j + 4)
                                            Exit While
                                        End If
                                        If big = 0 Then
                                            'kk += j
                                            fail = True
                                            Exit While
                                        End If
                                    Next
                                    kk += 512
                                    If readSize = 0 Then
                                        fail = True
                                        Exit While
                                    End If
                                End While
                                fs.Close()
                                If fail = False Then
                                    If skiplen < 16 Then
                                        Dim fss As New System.IO.FileStream(enctable, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                        fss.Seek(2 * kk, IO.SeekOrigin.Begin)
                                        fss.Read(bs, 0, skiplen)
                                        fss.Close()
                                        Array.Copy(bs, 0, stm, k, skiplen)
                                        k += skiplen
                                    Else
                                        Dim fss As New System.IO.FileStream(enctable, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                        fss.Seek(2 * kk, IO.SeekOrigin.Begin)
                                        fss.Read(bs, 0, 2)
                                        fss.Close()
                                        'CP1201全角
                                        If bs(1) = 0 AndAlso enc = 1201 Then
                                            Array.Copy(bs, 0, stm, k, 2)
                                            k += 2
                                            '半角カナ
                                        ElseIf bs(1) = 0 Then
                                            Array.Copy(bs, 0, stm, k, 1)
                                            k += 1
                                            '全角
                                        Else
                                            Array.Copy(bs, 0, stm, k, 2)
                                            k += 2
                                        End If
                                        End If
                                    Else
                                        '失敗
                                        Array.Copy(dummy, 0, stm, k, 2)
                                        k += 2
                                    End If
                                If (bb(i) < &HE0) Then
                                    i += 2
                                ElseIf bb(i) < &HF0 Then
                                    i += 3
                                Else
                                    i += 4
                                End If
                            ElseIf bb(i) < &HFC Then
                                i += 5
                            ElseIf bb(i) < &HFE Then
                                i += 6
                            Else
                                'BOM
                                i += 3
                            End If
                        End While

                        TextBox2.Text = System.Text.Encoding.GetEncoding(enc).GetString(stm)
                        s = ""
                        For i = 0 To stm.Length - 1
                            If enc < 1200 AndAlso enc > 1201 Then
                                If stm(i) = 0 Then
                                    Exit For
                                End If
                            Else
                                If (i And 1) = 0 AndAlso stm(i) = 0 AndAlso stm(i + 1) = 0 Then
                                    Exit For
                                End If
                            End If
                            s &= stm(i).ToString("X2")
                        Next

                        TextBox3.Text = s

                    Else
                        MessageBox.Show(output & enctable & ",文字テーブルファイルがありません")
                    End If
                End If
            Else
                MessageBox.Show("文字コードが指定できません")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

End Class
