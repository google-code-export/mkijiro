Imports System
Imports System.Text
Imports System.IO

Public Class Form1

    Private Sub INI(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        TextBox1.Font = My.Settings.font
        If My.Settings.out = True Then
            outTX.Checked = True
        End If

    End Sub

    Dim parsetest As String() = {"sjis2004test.txt", "eucjis2004.txt", "extra.txt", "out.txt"}
    Dim vstable As String() = {"table\sjisvsutf8", "table\eucvsutf8", "table\extra"}
    Dim unitable As String() = {"table\custom_utf32", "table\custom_utf32_2", "table\custom_utf32_3"}


    Private Function sel_num(ByVal sel As Integer, ByVal mode As Integer) As Integer
        sel = 0
        If EUC.Checked = True Then
            sel = 1
        ElseIf EX.Checked = True Then
            sel = 2
        End If
        If mode = 0 AndAlso outTX.Checked = True Then
            sel = 3
        End If

        Return sel
    End Function

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles READ.Click
        Dim sel As Integer = 0
        sel = sel_num(sel, 0)

        If File.Exists(parsetest(sel)) Then

            Dim fs As New FileStream(parsetest(sel), FileMode.Open, FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            Dim bss(CInt(fs.Length - 1) * 2) As Byte
            fs.Read(bs, 0, bs.Length)
            fs.Close()

            Dim codepage As Integer = GetCode(bs)
            Label1.Text = codepage.ToString

            If outTX.Checked = True Then
                sel = sel_num(sel, 1)
            End If
            If File.Exists(vstable(sel)) Then
                Dim tfs As New FileStream(vstable(sel), FileMode.Open, FileAccess.Read)
                Dim tbl(CInt(tfs.Length - 1)) As Byte
                tfs.Read(tbl, 0, tbl.Length)
                tfs.Close()
                Dim tofu As Byte() = {&HE2, &H96, &HA1}
                Dim maru As Byte() = {&HE3, &H82, &H9A}
                Dim ac As Byte() = {&HCC, &H80}
                Dim ac2 As Byte() = {&HCC, &H81}
                Dim gousei As Byte() = {&HCB, &HA9, &HCB, &HA5}
                Dim jis201 As Byte() = {0, 0, 0}
                Dim c1 As Integer = 0
                Dim c2 As Integer = 0
                Dim c3 As Integer = 0
                Dim ct As Integer = 0
                Dim i As Integer = 0
                Dim k As Integer = 0
                Dim pos As Integer = 0
                If SJIS.Checked = True Then
                    'SJIS
                    While i < bs.Length

                        c1 = bs(i)
                        If c1 < 128 Then
                            '制御コード排除
                            If (c1 < 32 AndAlso c1 <> &H9 AndAlso c1 <> &HD AndAlso c1 <> &HA) Or c1 = &H7F Then
                                c1 = 32
                            End If
                            bss(k) = c1
                            '改行CRLF化
                            If c1 = &HA Then
                                If k > 0 Then
                                    c2 = bss(k - 1)
                                Else
                                    c2 = 0
                                End If
                                If c2 <> &HD Then
                                    bss(k) = &HD
                                    bss(k + 1) = c1
                                    k += 1
                                End If
                            End If
                            k += 1
                            i += 1
                        ElseIf ((c1 + &H5F) And &HFF) < &H3F Then
                            c1 = c1 + &HFF60 - &HA0
                            'uuuu zzzz yyyy xxxx
                            'E1..EF 1110uuuu
                            '80..BF 10zzzzyy
                            '80..BF 10yyxxxx
                            jis201(0) = CByte((c1 >> 12) Or &HE0)
                            jis201(1) = CByte(((c1 >> 6) And &H3F) Or &H80)
                            jis201(2) = CByte((c1 And &H3F) Or &H80)
                            Array.Copy(jis201, 0, bss, k, 3)
                            k += 3
                            i += 1
                        ElseIf ((((c1 Xor &H20) + &H5F) And &HFF) < &H3C) Then
                            c2 = bs(i + 1)
                            If c2 >= &H40 AndAlso c2 <= &HFC AndAlso c2 <> &H7F Then
                                pos = ((c1 Xor &H20) - &HA1) * 192 + c2 - &H40
                                If pos * 4 < tbl.Length Then
                                    Array.Copy(tbl, pos * 4, bss, k, 4)
                                    ct = bss(k)
                                    If ct = 0 Then
                                        Array.Copy(tofu, 0, bss, k, 3)
                                        k += 3
                                    Else
                                        If ct < &H80 Then
                                            k += 1
                                        ElseIf ct < &HC2 Then
                                        ElseIf ct < &HE0 Then
                                            k += 2
                                            If c1 = &H86 AndAlso (c2 And 1) = 1 AndAlso c2 >= &H67 AndAlso c2 <= &H6D Then
                                                Array.Copy(ac, 0, bss, k, 2)
                                                k += 2
                                            ElseIf c1 = &H86 AndAlso (c2 And 1) = 0 AndAlso c2 >= &H68 AndAlso c2 <= &H6E Then
                                                Array.Copy(ac2, 0, bss, k, 2)
                                                k += 2
                                            ElseIf c1 = &H86 AndAlso (c2 = &H85 Or c2 = &H86) Then
                                                Array.Copy(gousei, (c2 And 1) * 2, bss, k, 2)
                                                k += 2
                                            ElseIf c1 = &H86 AndAlso c2 = &H63 Then
                                                Array.Copy(ac, 0, bss, k, 2)
                                                k += 2
                                            End If
                                        ElseIf ct < &HF0 Then
                                            k += 3
                                            If c1 = &H82 AndAlso c2 >= &HF5 AndAlso c2 <= &HF9 Then
                                                Array.Copy(maru, 0, bss, k, 3)
                                                k += 3
                                            ElseIf c1 = &H83 AndAlso c2 >= &H97 AndAlso c2 <= &H9E Then
                                                Array.Copy(maru, 0, bss, k, 3)
                                                k += 3
                                            ElseIf c1 = &H83 AndAlso c2 = &HF6 Then
                                                Array.Copy(maru, 0, bss, k, 3)
                                                k += 3
                                            End If


                                        ElseIf ct < &HF8 Then
                                            k += 4
                                        End If
                                    End If
                                End If
                            Else
                                Array.Copy(tofu, 0, bss, k, 3)
                                k += 3
                            End If
                            i += 2
                        Else
                            i += 1
                        End If

                    End While

                    'EUC
                ElseIf EUC.Checked = True Then
                    While i < bs.Length
                        c1 = bs(i)
                        If c1 < 128 Then
                            '制御コード排除
                            If (c1 < 32 AndAlso c1 <> &H9 AndAlso c1 <> &HD AndAlso c1 <> &HA) Or c1 = &H7F Then
                                c1 = 32
                            End If
                            bss(k) = c1
                            '改行CRLF化
                            If c1 = &HA Then
                                If k > 0 Then
                                    c2 = bss(k - 1)
                                Else
                                    c2 = 0
                                End If
                                If c2 <> &HD Then
                                    bss(k) = &HD
                                    bss(k + 1) = c1
                                    k += 1
                                End If
                            End If
                            k += 1
                            i += 1
                        ElseIf c1 = &H8E Then
                            c2 = bs(i + 1)
                            If ((c2 + &H5F) And &HFF) < &H3F Then
                                c2 = c2 + &HFF60 - &HA0
                                'uuuu zzzz yyyy xxxx
                                'E1..EF 1110uuuu
                                '80..BF 10zzzzyy
                                '80..BF 10yyxxxx
                                jis201(0) = CByte((c2 >> 12) Or &HE0)
                                jis201(1) = CByte(((c2 >> 6) And &H3F) Or &H80)
                                jis201(2) = CByte((c2 And &H3F) Or &H80)
                                Array.Copy(jis201, 0, bss, k, 3)
                                k += 3
                            End If
                            i += 2

                        ElseIf (((c1 + &H5F) And &HFF) < &H5E) Or c1 = &H8F Then
                            c2 = bs(i + 1)
                            c3 = 0

                            If c1 = &H8F Then
                                c3 = 1
                                c1 = c2
                                c2 = bs(i + 2)
                            End If

                            If c2 >= &HA1 AndAlso c2 <= &HFE Then
                                pos = (c3 * 94 * 94) + (c1 - &HA1) * 94 + c2 - &HA1
                                If pos * 4 < tbl.Length Then
                                    Array.Copy(tbl, pos * 4, bss, k, 4)
                                    ct = bss(k)
                                    If ct = 0 Then
                                        Array.Copy(tofu, 0, bss, k, 3)
                                        k += 3
                                    Else
                                        If ct < &H80 Then
                                            k += 1
                                        ElseIf ct < &HC2 Then
                                        ElseIf ct < &HE0 Then
                                            k += 2
                                            If c3 = 0 Then
                                                If c1 = &HAB AndAlso (c2 And 1) = 0 AndAlso c2 >= &HC8 AndAlso c2 <= &HCF Then
                                                    Array.Copy(ac, 0, bss, k, 2)
                                                    k += 2
                                                ElseIf c1 = &HAB AndAlso (c2 And 1) = 1 AndAlso c2 >= &HC8 AndAlso c2 <= &HCF Then
                                                    Array.Copy(ac2, 0, bss, k, 2)
                                                    k += 2
                                                ElseIf c1 = &HAB AndAlso (c2 = &HE5 Or c2 = &HE6) Then
                                                    Array.Copy(gousei, (c2 And 1) * 2, bss, k, 2)
                                                    k += 2
                                                ElseIf c1 = &HAB AndAlso c2 = &HC4 Then
                                                    Array.Copy(ac, 0, bss, k, 2)
                                                    k += 2
                                                End If
                                            End If

                                        ElseIf ct < &HF0 Then
                                            k += 3
                                            If c3 = 0 Then
                                                If c1 = &HA4 AndAlso c2 >= &HF7 AndAlso c2 <= &HFB Then
                                                    Array.Copy(maru, 0, bss, k, 3)
                                                    k += 3
                                                ElseIf c1 = &HA5 AndAlso c2 >= &HF7 AndAlso c2 <= &HFE Then
                                                    Array.Copy(maru, 0, bss, k, 3)
                                                    k += 3
                                                ElseIf c1 = &HA6 AndAlso c2 = &HF8 Then
                                                    Array.Copy(maru, 0, bss, k, 3)
                                                    k += 3
                                                End If
                                            End If

                                        ElseIf ct < &HF8 Then
                                            k += 4
                                        End If
                                    End If
                                End If
                            Else
                                Array.Copy(tofu, 0, bss, k, 3)
                                k += 3
                            End If


                            If c3 = 1 Then
                                i += 3
                            Else
                                i += 2

                            End If

                        Else
                            i += 1
                        End If

                    End While

                    'extra 'GBK/BIG5
                ElseIf EX.Checked = True Then
                    While i < bs.Length
                        c1 = bs(i)
                        If c1 < 128 Then
                            '制御コード排除
                            If (c1 < 32 AndAlso c1 <> &H9 AndAlso c1 <> &HD AndAlso c1 <> &HA) Or c1 = &H7F Then
                                c1 = 32
                            End If
                            bss(k) = c1
                            '改行CRLF化
                            If c1 = &HA Then
                                If k > 0 Then
                                    c2 = bss(k - 1)
                                Else
                                    c2 = 0
                                End If
                                If c2 <> &HD Then
                                    bss(k) = &HD
                                    bss(k + 1) = c1
                                    k += 1
                                End If
                            End If
                            k += 1
                            i += 1
                            'BIG5/GBK
                        ElseIf (((c1 + &H7F) And &HFF) < &H7E) Then
                            c2 = bs(i + 1)
                            If c2 >= &H40 AndAlso c2 <= &HFE Then
                                pos = (c1 - &H81) * 192 + c2 - &H40
                                If pos * 4 < tbl.Length Then
                                    Array.Copy(tbl, pos * 4, bss, k, 4)
                                    ct = bss(k)
                                    If ct = 0 Then
                                        Array.Copy(tofu, 0, bss, k, 3)
                                        k += 3
                                    Else
                                        If ct < &H80 Then
                                            k += 1
                                        ElseIf ct < &HC2 Then
                                        ElseIf ct < &HE0 Then
                                            k += 2
                                        ElseIf ct < &HF0 Then
                                            k += 3
                                        ElseIf ct < &HF8 Then
                                            k += 4
                                        End If
                                    End If
                                End If
                            Else
                                Array.Copy(tofu, 0, bss, k, 3)
                                k += 3
                            End If
                            i += 2
                        Else
                            i += 1
                        End If

                    End While
                End If

                Array.Resize(bss, k)

                TextBox1.Text = Encoding.GetEncoding(65001).GetString(bss)
            Else
                MessageBox.Show(vstable(sel) & "がありません")
            End If
        Else
            MessageBox.Show(parsetest(sel) & "がありません")
        End If

    End Sub

    Function gouseimaru(ByVal pos As Integer) As Integer
        Dim maru As Integer() = {&H304B, &H304D, &H304F, &H3051, &H3053, &H30AB, &H30AD, &H30AF, &H30B1, &H30B3, &H30BB, &H30C4, &H30C8, &H31F7}
        For i = 0 To maru.Length - 1
            If pos = maru(i) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Function gouseiac(ByVal pos As Integer) As Integer
        Dim maru As Integer() = {&H254, &H28C, &H259, &H25A}
        For i = 0 To maru.Length - 1
            If pos = maru(i) Then
                Return i
            End If
        Next
        Return -1
    End Function


    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles SAVE.Click

        Dim sel As Integer = 0
        sel = sel_num(sel, 1)
        If File.Exists(unitable(sel)) = True Then
            Dim fs As New FileStream("out.txt", FileMode.Create, FileAccess.Write)
            Dim bs As Byte() = Encoding.GetEncoding(1200).GetBytes(TextBox1.Text)
            Dim bss(CInt(bs.Length - 1)) As Byte

            Dim tfs As New FileStream(unitable(sel), FileMode.Open, FileAccess.Read)
            Dim tbl(CInt(tfs.Length - 1)) As Byte
            tfs.Read(tbl, 0, tbl.Length)
            tfs.Close()

            Dim tofusjis As Byte() = {&H81, &HA0}
            Dim tofueuc As Byte() = {&HA2, &HA1}
            Dim tofugb As Byte() = {&HA1, &HF5} 'GBK 0xa1f5 ,BIG5 0xa1bc
            Dim tofuhk As Byte() = {&HA1, &HBC} '
            Dim maru As Byte() = {&HE3, &H82, &H9A}
            Dim ac As Byte() = {&HCC, &H80}
            Dim gousei As Byte() = {&HCB, &HA9, &HCB, &HA5}

            Dim hmaru As Integer() = {&HF582, &H9783, &HF683}
            Dim ha As Integer = &H6386
            Dim hac As Integer = &H6786
            Dim koe As Integer = &H8586


            Dim hmaru_euc As Integer() = {&HF7A4, &HF7A5, &HF8A6}
            Dim ha_euc As Integer = &HC4AB
            Dim hac_euc As Integer = &HC8AB
            Dim koe_euc As Integer = &HE5AB

            Dim code As Byte()

            Dim jis201 As Byte() = {0, 0, 0}
            Dim c1 As Integer = 0
            Dim c2 As Integer = 0
            Dim c3 As Integer = 0
            Dim ct As Integer = 0
            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim l As Integer = 0
            Dim m As Integer = 0
            Dim hex As UInt16 = 0
            Dim hex2 As UInt16 = 0
            Dim pos As Int32 = 0
            'SJIS2004
            If SJIS.Checked = True Then
                While i < bs.Length
                    hex = BitConverter.ToUInt16(bs, i)
                    If hex >= &HD800 AndAlso hex <= &HDBFF Then
                        i += 2
                        hex2 = BitConverter.ToUInt16(bs, i)
                        If hex2 >= &HDC00 AndAlso hex2 <= &HDFFF Then
                            pos = (hex And &H3FF) * 1024 + (hex2 And &H3FF)
                            pos += &H10000
                        Else
                            pos = tbl.Length
                        End If
                    Else
                        pos = Convert.ToInt32(hex)
                    End If
                    If pos * 4 < tbl.Length Then
                        Array.Copy(tbl, pos * 4, bss, k, 4)
                        c1 = bss(k)
                        c2 = bss(k + 1)
                        c3 = bss(k + 2)
                        If i + 4 <= bs.Length Then
                            hex2 = BitConverter.ToUInt16(bs, i + 2)
                        Else
                            hex2 = 0
                        End If

                        If hex2 = &H309A AndAlso gouseimaru(pos) >= 0 Then
                            m = gouseimaru(pos)
                            l = 0
                            If m > 12 Then
                                l = 2
                                m = 0
                            ElseIf m > 4 Then
                                l = 1
                                m -= 5
                            End If
                            code = BitConverter.GetBytes(hmaru(l) + &H100 * m)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H300 AndAlso pos = &HE6 Then
                            code = BitConverter.GetBytes(ha)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H300 AndAlso gouseiac(pos) >= 0 Then
                            code = BitConverter.GetBytes(hac + (2 * gouseiac(pos) * &H100))
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H301 AndAlso gouseiac(pos) >= 0 Then
                            code = BitConverter.GetBytes(hac + ((2 * gouseiac(pos) + 1) * &H100))
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H2E9 AndAlso pos = &H2E5 Then
                            code = BitConverter.GetBytes(koe + &H100)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H2E5 AndAlso pos = &H2E9 Then
                            code = BitConverter.GetBytes(koe)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2
                        Else
                            If pos < 128 Then
                                If pos < 32 AndAlso pos <> &HA AndAlso pos <> &HD AndAlso pos <> &H9 Then
                                    pos = 32
                                End If
                                bss(k) = pos
                                k += 1
                            ElseIf c1 = 0 AndAlso c2 = 0 Then
                                Array.Copy(tofusjis, 0, bss, k, 2)
                                k += 2
                            ElseIf c2 = 0 AndAlso c1 >= &HA1 AndAlso c1 <= &HDF Then
                                k += 1
                            ElseIf (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso c2 >= &H40 Then
                                k += 2
                            Else
                                Array.Copy(tofusjis, 0, bss, k, 2)
                                k += 2
                            End If
                        End If
                    End If
                    i += 2
                End While

                'EUC2004
            ElseIf EUC.Checked = True Then
                While i < bs.Length
                    hex = BitConverter.ToUInt16(bs, i)
                    If hex >= &HD800 AndAlso hex <= &HDBFF Then
                        i += 2
                        hex2 = BitConverter.ToUInt16(bs, i)
                        If hex2 >= &HDC00 AndAlso hex2 <= &HDFFF Then
                            pos = (hex And &H3FF) * 1024 + (hex2 And &H3FF)
                            pos += &H10000
                        Else
                            pos = tbl.Length
                        End If
                    Else
                        pos = Convert.ToInt32(hex)
                    End If
                    If pos * 4 < tbl.Length Then
                        Array.Copy(tbl, pos * 4, bss, k, 4)
                        c1 = bss(k)
                        c2 = bss(k + 1)
                        c3 = bss(k + 2)

                        If i + 4 <= bs.Length Then
                            hex2 = BitConverter.ToUInt16(bs, i + 2)
                        Else
                            hex2 = 0
                        End If

                        If hex2 = &H309A AndAlso gouseimaru(pos) >= 0 Then
                            m = gouseimaru(pos)
                            l = 0
                            If m > 12 Then
                                l = 2
                                m = 0
                            ElseIf m > 4 Then
                                l = 1
                                m -= 5
                            End If
                            code = BitConverter.GetBytes(hmaru_euc(l) + &H100 * m)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H300 AndAlso pos = &HE6 Then
                            code = BitConverter.GetBytes(ha_euc)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H300 AndAlso gouseiac(pos) >= 0 Then
                            code = BitConverter.GetBytes(hac_euc + ((2 * gouseiac(pos)) * &H100))
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H301 AndAlso gouseiac(pos) >= 0 Then
                            code = BitConverter.GetBytes(hac_euc + ((2 * gouseiac(pos) + 1) * &H100))
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H2E9 AndAlso pos = &H2E5 Then
                            code = BitConverter.GetBytes(koe_euc + &H100)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2

                        ElseIf hex2 = &H2E5 AndAlso pos = &H2E9 Then
                            code = BitConverter.GetBytes(koe_euc)
                            Array.Copy(code, 0, bss, k, 4)
                            k += 2
                            i += 2
                        Else
                            If pos < 128 Then
                                If pos < 32 AndAlso pos <> &HA AndAlso pos <> &HD AndAlso pos <> &H9 Then
                                    pos = 32
                                End If
                                bss(k) = pos
                                k += 1
                            ElseIf c1 = 0 AndAlso c2 = 0 Then
                                Array.Copy(tofueuc, 0, bss, k, 2)
                                k += 2
                            ElseIf c1 = &H8E Then
                                k += 2
                            ElseIf c1 = &H8F Then
                                k += 3
                            ElseIf ((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= &HA1 Then
                                k += 2
                            Else
                                Array.Copy(tofueuc, 0, bss, k, 2)
                                k += 2
                            End If
                        End If
                    End If
                    i += 2
                End While
                'BIG5/GBK
            ElseIf EX.Checked = True Then
                While i < bs.Length
                    hex = BitConverter.ToUInt16(bs, i)
                    If hex >= &HD800 AndAlso hex <= &HDBFF Then
                        i += 2
                        hex2 = BitConverter.ToUInt16(bs, i)
                        If hex2 >= &HDC00 AndAlso hex2 <= &HDFFF Then
                            pos = (hex And &H3FF) * 1024 + (hex2 And &H3FF)
                            pos += &H10000
                        Else
                            pos = tbl.Length
                        End If
                    Else
                        pos = Convert.ToInt32(hex)
                    End If
                    If pos * 4 < tbl.Length Then
                        Array.Copy(tbl, pos * 4, bss, k, 4)
                        c1 = bss(k)
                        c2 = bss(k + 1)
                        c3 = bss(k + 2)

                        If pos < 128 Then
                            If pos < 32 AndAlso pos <> &HA AndAlso pos <> &HD AndAlso pos <> &H9 Then
                                pos = 32
                            End If
                            bss(k) = pos
                            k += 1
                        ElseIf c1 = 0 AndAlso c2 = 0 Then
                            Array.Copy(tofuhk, 0, bss, k, 2)
                            k += 2
                            'skip
                        ElseIf ((c1 + &H7F) And &HFF) < &H7E AndAlso c2 >= &H40 Then
                            k += 2
                        Else
                            Array.Copy(tofuhk, 0, bss, k, 2)
                            k += 2
                        End If
                    End If
                    i += 2
                End While
            End If



            fs.Write(bss, 0, k)
            fs.Close()
            Beep()
        Else
            MessageBox.Show(unitable(sel) & "がありません")
        End If

    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles CLEAR.Click
        TextBox1.Text = ""
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles FONTs.Click
        Dim fd As New FontDialog()

        '初期のフォントを設定
        fd.Font = TextBox1.Font
        '初期の色を設定
        fd.Color = TextBox1.ForeColor
        '存在しないフォントやスタイルをユーザーが選択すると
        'エラーメッセージを表示する
        fd.FontMustExist = True
        '横書きフォントだけを表示する
        fd.AllowVerticalFonts = False
        '色を選択できるようにする
        fd.ShowColor = True
        '取り消し線、下線、テキストの色などのオプションを指定可能にする
        'デフォルトがTrueのため必要はない
        fd.ShowEffects = True
        '固定ピッチフォント以外も表示する
        'デフォルトがFalseのため必要はない
        fd.FixedPitchOnly = False
        'ベクタ フォントを選択できるようにする
        'デフォルトがTrueのため必要はない
        fd.AllowVectorFonts = True

        'ダイアログを表示する
        If fd.ShowDialog() <> DialogResult.Cancel Then
            'TextBox1のフォントと色を変える
            TextBox1.Font = fd.Font
            My.Settings.font = fd.Font
            TextBox1.ForeColor = fd.Color
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.KeyPress, TextBox1.KeyUp, TextBox1.KeyDown, TextBox1.TextChanged, TextBox1.Click
        Dim iCaret As Integer = Me.TextBox1.SelectionStart
        Dim s As String = ""
        If iCaret < TextBox1.Text.Length - 1 Then
            Dim bb As Byte() = Encoding.GetEncoding(1200).GetBytes(TextBox1.Text.Substring(iCaret, 2))
            Dim sarrogate As UInteger = BitConverter.ToUInt32(bb, 0) And &HFFFF
            Dim sarrogate2 As UInteger = BitConverter.ToUInt32(bb, 0) >> 16
            If sarrogate < &HD800 Or sarrogate > &HDFFF Then
                Array.Clear(bb, 2, 2)
                unihex.Text = "UTF-32:U+" & sarrogate.ToString("X")
                unihex.Text &= " UTF-16:U+" & sarrogate.ToString("X")
                unihex.Text &= " UTF-8:" & getutf8(bb)
            ElseIf sarrogate2 >= &HDC00 AndAlso sarrogate2 < &HE000 Then
                Dim utf32 As UInt32 = &H10000 + ((sarrogate And &H3FF) << 10) Or (sarrogate2 And &H3FF)
                unihex.Text = "UTF-32:U+" & utf32.ToString("X")
                unihex.Text &= " UTF-16:U+" & sarrogate.ToString("X")
                unihex.Text &= "+" & sarrogate2.ToString("X")
                bb = BitConverter.GetBytes(utf32)
                unihex.Text &= " UTF-8:" & getutf8(bb)
            Else
                unihex.Text = "U+" & sarrogate.ToString("X")
                unihex.Text &= "+" & sarrogate2.ToString("X")
                unihex.Text &= "は不正なサロゲートペアです"
            End If
        ElseIf iCaret = TextBox1.Text.Length - 1 Then
            Dim bb As Byte() = Encoding.GetEncoding(1200).GetBytes(TextBox1.Text.Substring(iCaret, 1))
            Dim sarrogate As UInteger = BitConverter.ToUInt16(bb, 0)
            Array.Resize(bb, 4)
            unihex.Text = "UTF-32:U+" & sarrogate.ToString("X")
            unihex.Text &= " UTF-16:U+" & sarrogate.ToString("X")
            unihex.Text &= " UTF-8:" & getutf8(bb)
        Else
            unihex.Text = "[EOF]"
        End If

    End Sub


    ' UTF32からUTF-8に変形
    Private Function getutf8(ByVal bb As Byte()) As String
        Dim wc As Integer = 0
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim s As String = ""
        While i < bb.Length
            wc = BitConverter.ToInt32(bb, i)
            n += utf32toutf8(bb, wc, n)
            i += 4
        End While
        Array.Resize(bb, n)
        For i = 0 To n - 1
            s &= bb(i).ToString("X")
        Next
        Return s

    End Function

    ',libconvのVB用
    Private Function utf32toutf8(r As Byte(), wc As Integer, n As Integer) As Integer
        '        Static int
        'utf8_wctomb (conv_t conv, unsigned char *r, ucs4_t wc, int n) /* n == 0 is acceptable */
        '{
        '  int count;
        '  if (wc < 0x80)
        '    count = 1;
        '  else if (wc < 0x800)
        '    count = 2;
        '  else if (wc < 0x10000)
        '    count = 3;
        '  else if (wc < 0x200000)
        '    count = 4;
        '  else if (wc < 0x4000000)
        '    count = 5;
        '  else if (wc <= 0x7fffffff)
        '    count = 6;
        '        Else
        '    return RET_ILUNI;
        '            If (n < count)
        '    return RET_TOOSMALL;
        '  switch (count) { /* note: code falls through cases! */
        '    case 6: r[5] = 0x80 | (wc & 0x3f); wc = wc >> 6; wc |= 0x4000000;
        '    case 5: r[4] = 0x80 | (wc & 0x3f); wc = wc >> 6; wc |= 0x200000;
        '    case 4: r[3] = 0x80 | (wc & 0x3f); wc = wc >> 6; wc |= 0x10000;
        '    case 3: r[2] = 0x80 | (wc & 0x3f); wc = wc >> 6; wc |= 0x800;
        '    case 2: r[1] = 0x80 | (wc & 0x3f); wc = wc >> 6; wc |= 0xc0;
        '    case 1: r[0] = wc;
        '  }
        '  return count;
        '}
        Dim count As Integer = 0
        Dim countbk As Integer = 0
        If wc < &H80 Then
            count = 1
        ElseIf wc < &H800 Then
            count = 2
        ElseIf wc < &H10000 Then
            count = 3
        ElseIf wc < &H200000 Then
            count = 4
            'ElseIf wc < &H4000000 Then
            '    count = 5
            'ElseIf wc <= &H7FFFFFFF Then
            '    count = 6
        Else
            'Return -1
            Return 0
        End If

        'If n < count Then
        '    Return -2
        'End If
        countbk = count

        'If count = 6 Then
        '    r(n + 5) = CByte(&H80 Or (wc And &H3F))
        '    wc = wc >> 6
        '    wc = wc Or &H4000000
        '    count -= 1
        'End If
        'If count = 5 Then
        '    r(n + 4) = CByte(&H80 Or (wc And &H3F))
        '    wc = wc >> 6
        '    wc = wc Or &H200000
        '    count -= 1
        'End If
        If count = 4 Then
            r(n + 3) = CByte(&H80 Or (wc And &H3F))
            wc = wc >> 6
            wc = wc Or &H10000
            count -= 1
        End If
        If count = 3 Then
            r(n + 2) = CByte(&H80 Or (wc And &H3F))
            wc = wc >> 6
            wc = wc Or &H800
            count -= 1
        End If
        If count = 2 Then
            r(n + 1) = CByte(&H80 Or (wc And &H3F))
            wc = wc >> 6
            wc = wc Or &HC0
            count -= 1
        End If
        If count = 1 Then
            r(n) = CByte(wc)
        End If


        Return countbk

    End Function

    ''' <summary>
    ''' 文字コードを判別する
    ''' </summary>
    ''' <remarks>
    ''' Jcode.pmのgetcodeメソッドを移植したものです。
    ''' Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
    ''' Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
    ''' </remarks>
    ''' <param name="bytes">文字コードを調べるデータ</param>
    ''' <returns>適当と思われるEncodingオブジェクト。
    ''' 判断できなかった時はnull。</returns>
    Public Shared Function GetCode(ByVal bytes As Byte()) As Integer
        Const bEscape As Byte = &H1B
        Const bAt As Byte = &H40
        Const bDollar As Byte = &H24
        Const bAnd As Byte = &H26
        Const bOpen As Byte = &H28 ''('
        Const bB As Byte = &H42
        Const bD As Byte = &H44
        Const bJ As Byte = &H4A
        Const bI As Byte = &H49

        Dim len As Integer = bytes.Length
        Dim b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte

        'Encode::is_utf8 は無視

        Dim isBinary As Boolean = False
        Dim i As Integer
        For i = 0 To len - 1
            b1 = bytes(i)
            If b1 <= &H6 OrElse b1 = &H7F OrElse b1 = &HFF Then
                ''binary'
                isBinary = True
                If b1 = &H0 AndAlso i < len - 1 AndAlso bytes(i + 1) <= &H7F Then
                    'smells like raw unicode
                    Return 1200
                End If
            End If
        Next
        If isBinary Then
            Return Nothing
        End If

        'not Japanese
        Dim notJapanese As Boolean = True
        For i = 0 To len - 1
            b1 = bytes(i)
            If b1 = bEscape OrElse &H80 <= b1 Then
                notJapanese = False
                Exit For
            End If
        Next
        If notJapanese Then
            Return 20127
        End If

        For i = 0 To len - 3
            b1 = bytes(i)
            b2 = bytes(i + 1)
            b3 = bytes(i + 2)

            If b1 = bEscape Then
                If b2 = bDollar AndAlso b3 = bAt Then
                    'JIS_0208 1978
                    'JIS
                    Return 50220
                ElseIf b2 = bDollar AndAlso b3 = bB Then
                    'JIS_0208 1983
                    'JIS
                    Return 50220
                ElseIf b2 = bOpen AndAlso (b3 = bB OrElse b3 = bJ) Then
                    'JIS_ASC
                    'JIS
                    Return 50220
                ElseIf b2 = bOpen AndAlso b3 = bI Then
                    'JIS_KANA
                    'JIS
                    Return 50220
                End If
                If i < len - 3 Then
                    b4 = bytes(i + 3)
                    If b2 = bDollar AndAlso b3 = bOpen AndAlso b4 = bD Then
                        'JIS_0212
                        'JIS
                        Return 50220
                    End If
                    If i < len - 5 AndAlso _
                        b2 = bAnd AndAlso b3 = bAt AndAlso b4 = bEscape AndAlso _
                        bytes(i + 4) = bDollar AndAlso bytes(i + 5) = bB Then
                        'JIS_0208 1990
                        'JIS
                        Return 50220
                    End If
                End If
            End If
        Next

        'should be euc|sjis|utf8
        'use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
        Dim enc As Integer() = {0, 0, 0, 0, 0}
        Dim encode As String() = {"sjis", "euc", "utf8", "gbk", "big5"}

        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H81 <= b1 AndAlso b1 <= &H9F) OrElse _
                (&HE0 <= b1 AndAlso b1 <= &HFC)) AndAlso _
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse _
                 (&H80 <= b2 AndAlso b2 <= &HFC)) Then
                'SJIS_C
                enc(0) += 2
                i += 1
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&HA1 <= b1 AndAlso b1 <= &HFE) AndAlso _
                (&HA1 <= b2 AndAlso b2 <= &HFE)) OrElse _
                (b1 = &H8E AndAlso (&HA1 <= b2 AndAlso b2 <= &HDF)) Then
                'EUC_C
                'EUC_KANA
                enc(1) += 2
                i += 1
            ElseIf i < len - 2 Then
                b3 = bytes(i + 2)
                If b1 = &H8F AndAlso (&HA1 <= b2 AndAlso b2 <= &HFE) AndAlso _
                    (&HA1 <= b3 AndAlso b3 <= &HFE) Then
                    'EUC_0212
                    enc(1) += 3
                    i += 2
                End If
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If (&HC0 <= b1 AndAlso b1 <= &HDF) AndAlso _
                (&H80 <= b2 AndAlso b2 <= &HBF) Then
                'UTF8
                enc(2) += 2
                i += 1
            ElseIf i < len - 2 Then
                b3 = bytes(i + 2)
                If (&HE0 <= b1 AndAlso b1 <= &HEF) AndAlso _
                    (&H80 <= b2 AndAlso b2 <= &HBF) AndAlso _
                    (&H80 <= b3 AndAlso b3 <= &HBF) Then
                    'UTF8
                    enc(2) += 3
                    i += 2
                End If
            ElseIf i < len - 3 Then
                b3 = bytes(i + 2)
                b4 = bytes(i + 3)
                If (&HF0 <= b1 AndAlso b1 <= &HF7) AndAlso _
                    (&H80 <= b2 AndAlso b2 <= &HBF) AndAlso _
                    (&H80 <= b3 AndAlso b3 <= &HBF) AndAlso _
                    (&H80 <= b4 AndAlso b4 <= &HBF) Then
                    'UTF8
                    enc(2) += 4
                    i += 3
                End If
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H81 <= b1 AndAlso b1 <= &HFE) AndAlso _
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse _
                 (&H80 <= b2 AndAlso b2 <= &HFE))) Then
                'GBK
                enc(3) += 2
                i += 1
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H88 <= b1 AndAlso b1 <= &HFE) AndAlso _
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse _
                 (&HA1 <= b2 AndAlso b2 <= &HFE))) Then
                'HK
                enc(4) += 2
                i += 1
            End If
        Next
        'M. Takahashi's suggestion
        'utf8 += utf8 / 2;

        Array.Sort(enc, encode)

        If encode(4) = "euc" Then
            'EUC
            Return 512132004
        ElseIf encode(4) = "sjis" Or encode(4) = "gbk" Or encode(4) = "big5" Then

            Dim enc2 As Integer() = {0, 0, 0}
            Dim encode2 As String() = {"sjis", "gbk", "big5"}

            Dim s As String = Encoding.GetEncoding(936).GetString(bytes)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(1) += 1
                End If
            Next
            s = Encoding.GetEncoding(950).GetString(bytes)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(2) += 1
                End If
            Next
            s = Encoding.GetEncoding(932).GetString(bytes)
            For i = 0 To s.Length - 1
                If s(i) = "?" Then
                    enc2(0) += 1
                End If
            Next

            Array.Sort(enc2, encode2)
            If encode2(0) = "gbk" Then
                'GBK
                Return 936
            End If

            'SJIS
            If encode2(0) = "sjis" Then
                Return 2132004
            End If

            Return 950

            ElseIf encode(4) = "utf8" Then
                'UTF8
                Return 65001
            End If

            Return 0
    End Function

    Private Sub EUC_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles EUC.CheckedChanged
        My.Settings.sel = 512132004
    End Sub

    Private Sub EX_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles EX.CheckedChanged
        My.Settings.sel = 951
    End Sub

    Private Sub SJIS_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles SJIS.CheckedChanged
        My.Settings.sel = 2132004
    End Sub

    Private Sub outTX_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles outTX.CheckedChanged
        My.Settings.out = outTX.Checked
    End Sub
End Class
