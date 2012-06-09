Imports System
Imports System.Text
Imports System.IO

Public Class Form1

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles READ.Click

        If File.Exists("sjis2004test.txt") Then
            Dim fs As New FileStream("sjis2004test.txt", FileMode.Open, FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            Dim bss(CInt(fs.Length - 1) * 2) As Byte
            fs.Read(bs, 0, bs.Length)
            fs.Close()
            If File.Exists("table\sjisvsutf8") Then
                Dim tfs As New FileStream("table\sjisvsutf8", FileMode.Open, FileAccess.Read)
                Dim tbl(CInt(tfs.Length - 1)) As Byte
                tfs.Read(tbl, 0, tbl.Length)
                tfs.Close()
                Dim tofu As Byte() = {&HE2, &H96, &HA1}
                Dim maru As Byte() = {&HE3, &H82, &H9A}
                Dim ac As Byte() = {&HCC, &H80}
                Dim gousei As Byte() = {&HCB, &HA9, &HCB, &HA5}
                Dim jis201 As Byte() = {0, 0, 0}
                Dim c1 As Integer = 0
                Dim c2 As Integer = 0
                Dim ct As Integer = 0
                Dim i As Integer = 0
                Dim k As Integer = 0
                Dim pos As Integer = 0
                While i < bs.Length
                    c1 = bs(i)
                    If c1 < 128 Then
                        bss(k) = c1
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
                                            Array.Copy(ac, 0, bss, k, 2)
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
                TextBox1.Text = Encoding.GetEncoding(65001).GetString(bss)
            Else
                MessageBox.Show("table\sjisvsutf8がありません")
            End If
        Else
            MessageBox.Show("sjis2004test.txtがありません")
        End If

    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles SAVE.Click

        If File.Exists("table\custom_utf32") Then
            Dim fs As New FileStream("sjis2004_out.txt", FileMode.Create, FileAccess.Write)
            Dim bs As Byte() = Encoding.GetEncoding(1200).GetBytes(TextBox1.Text)
            Dim bss(CInt(bs.Length - 1)) As Byte

            Dim tfs As New FileStream("table\custom_utf32", FileMode.Open, FileAccess.Read)
            Dim tbl(CInt(tfs.Length - 1)) As Byte
            tfs.Read(tbl, 0, tbl.Length)
            tfs.Close()

            Dim tofu As Byte() = {&HE2, &H96, &HA1}
            Dim maru As Byte() = {&HE3, &H82, &H9A}
            Dim ac As Byte() = {&HCC, &H80}
            Dim gousei As Byte() = {&HCB, &HA9, &HCB, &HA5}
            Dim jis201 As Byte() = {0, 0, 0}
            Dim c1 As Integer = 0
            Dim c2 As Integer = 0
            Dim c3 As Integer = 0
            Dim ct As Integer = 0
            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim hex As UInt16 = 0
            Dim hex2 As UInt16 = 0
            Dim pos As Int32 = 0
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
                    ElseIf c1 = 0 AndAlso c2 = 2 Then
                        'skip
                    ElseIf c2 = 0 AndAlso c1 >= &HA1 AndAlso c1 <= &HDF Then
                        k += 1
                    ElseIf (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso c2 >= 40 Then
                        k += 2
                    End If
                End If
                i += 2
            End While
            fs.Write(bss, 0, k)
            fs.Close()
            Beep()
        Else
            MessageBox.Show("table\custom_utf32がありません")
        End If

    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles CLEAR.Click
        TextBox1.Text = ""
    End Sub
End Class
