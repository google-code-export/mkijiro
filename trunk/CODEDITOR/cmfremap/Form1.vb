Imports System
Imports System.IO
Imports System.Text

Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        'int IsHzcode(int x, const char *msg)
        '{
        '        return ((( ( (unsigned char)msg[x] > 0xA0 && (unsigned char)msg[x] < 0xAA) 
        '|| ((unsigned char)msg[x] > 0xAF && (unsigned char)msg[x] < 0xF8)) && (unsigned char)msg[x + 1] > 0xA0)
        '                        || ((unsigned char)msg[x] > 0x80 && (unsigned char)msg[x] < 0xA1 && (unsigned char)msg[x + 1] > 0x3F)
        '                        || ((unsigned char)msg[x] > 0xA9 && (unsigned char)msg[x + 1] > 0x3F && (unsigned char)msg[x + 1] < 0xA1)
        '                );
        '}

        'static u8 * GetHz(int x, const char *msg)
        '{
        '                        if((unsigned char)msg[x] > 0xA0 && (unsigned char)msg[x] < 0xAA && (unsigned char)msg[x + 1] > 0xA0)
        '                                return _get_hzfont( (int)((unsigned char)msg[x] - 0xA1) * 94 + (int)((unsigned char)msg[x + 1] - 0xA1) );
        '        Else
        '                        if((unsigned char)msg[x] > 0xAF && (unsigned char)msg[x + 1] > 0xA0)
        '                                return _get_hzfont( 846 + (int)((unsigned char)msg[x] - 0xB0) * 94 + (int)((unsigned char)msg[x + 1] - 0xA1) );
        '                        else if((unsigned char)msg[x] > 0x80 && (unsigned char)msg[x] < 0xA1 && (unsigned char)msg[x + 1] > 0x3F)
        '                                return _get_hzfont( 7614 + (int)((unsigned char)msg[x] - 0x81) * 191 + (int)((unsigned char)msg[x + 1] - 0x40) );
        '            Else
        '                                return _get_hzfont( 13726 + (int)((unsigned char)msg[x] - 0xAA) * 97 + (int)((unsigned char)msg[x + 1] - 0x40) );
        '}

        Dim fs As New FileStream("font.dat", FileMode.Open, FileAccess.Read)
        Dim bs(fs.Length - 1) As Byte
        Dim bss(2 * (fs.Length - 1)) As Byte
        fs.Read(bs, 0, bs.Length)
        Dim fssize As Integer = fs.Length
        fs.Close()

        If File.Exists("font_base.dat") Then
            Dim ffs As New FileStream("font_base.dat", FileMode.Open, FileAccess.Read)
            ffs.Read(bss, 0, fssize)
            ffs.Close()
        Else
            Array.Copy(bs, 0, bss, 0, 2048)
        End If

        Dim c1 As Integer = 0
        Dim c2 As Integer = 0
        Dim pos As Integer = 0
        Dim pos2 As Integer = 0
        For i = &H8140 To &HFEFE
            c1 = i >> 8
            c2 = i And &HFF
            If ((((c1 > &HA0 AndAlso c1 < &HAA) Or (c1 > &HAF AndAlso c1 < &HF8)) AndAlso c2 > &HA0) Or (c1 > &H80 AndAlso c1 < &HA1 AndAlso c2 > &H3F) Or (c1 > &HA9 AndAlso c2 > &H3F AndAlso c2 < &HA1)) Then
                If (c2 >= 40 AndAlso c2 <= &HFE AndAlso c2 <> &H7F) Then
                    pos = 2048 + 18 * ((c1 - &H81) * 192 + c2 - &H40)
                    If c1 > &HA0 AndAlso c1 < &HAA AndAlso c2 > &HA0 Then
                        pos2 = 2048 + 18 * ((c1 - &HA1) * 94 + c2 - &HA1)
                        If pos2 + 18 <= fssize Then
                            Array.Copy(bs, pos2, bss, pos, 18)
                        End If
                    Else
                        If (c1 > &HAF AndAlso c2 > &HA0) Then
                            pos2 = 2048 + 18 * (846 + (c1 - &HB0) * 94 + c2 - &HA1) '3B7C+800=437C
                            If pos2 + 18 <= fssize Then
                                Array.Copy(bs, pos2, bss, pos, 18)
                            End If
                        ElseIf (c1 > &H80 AndAlso c1 < &HA1 AndAlso c2 > &H3F) Then
                            pos2 = 2048 + 18 * (7614 + (c1 - &H81) * 191 + c2 - &H40) '2175C+800=21F5C
                            If pos2 + 18 <= fssize Then
                                Array.Copy(bs, pos2, bss, pos, 18)
                            End If
                        Else
                            pos2 = 2048 + 18 * (13726 + (c1 - &HAA) * 97 + c2 - &H40) '3C51C+800=3CD1C
                            If pos2 + 18 <= fssize Then
                                Array.Copy(bs, pos2, bss, pos, 18)
                            End If
                        End If
                    End If
                End If
            End If

        Next


        Dim fss As New FileStream("font_gbk.dat", FileMode.Create, FileAccess.Write)
        fss.Write(bss, 0, 437504)
        fss.Close()
        Beep()

    End Sub
End Class
