﻿Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Friend cpg As Integer = 932
    Friend sel As Integer = 0
    Public tmp As Integer = &H8200000
    Public cmf As Integer = &H8300000

    Private Sub ff(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try

            If File.Exists("conf") = True Then
                Dim sr As New System.IO.StreamReader("conf", System.Text.Encoding.GetEncoding(932))
                Dim s As String
                While sr.Peek() > -1
                    s = sr.ReadLine()
                    If s.Contains("ADDR") Then
                        ADDR.Text = s.Remove(0, 4)
                    ElseIf s.Contains("MODE") Then
                        MODE.Text = s.Remove(0, 4)
                    ElseIf s.Contains("ENC") Then
                        cpg = CInt(s.Remove(0, 3))
                    ElseIf s.Contains("SEL") Then
                        sel = CInt(s.Remove(0, 3))
                    ElseIf s.Contains("TMSB") Then
                        tmp = CInt(s.Remove(0, 4))
                    ElseIf s.Contains("CMSB") Then
                        cmf = CInt(s.Remove(0, 4))
                    ElseIf s.Contains("FONT") Then
                        Dim ss As String() = s.Split(CChar(","))
                        ASM.Font = New Font(iniparse(ss(0)), CSng(iniparse(ss(1))), FontStyle.Regular)
                        CODE.Font = ASM.Font
                    ElseIf s.Contains("RPN") Then
                        tmp = CInt(s.Remove(0, 3))
                        If (tmp And 1) = 1 Then
                            RPN.Checked = True
                        End If
                        If (tmp And 2) = 2 Then
                            STACKORDER.Checked = False
                            LOOKSORDER.Checked = True
                        Else
                            STACKORDER.Checked = True
                            LOOKSORDER.Checked = False
                        End If
                        If (tmp And 4) = 4 Then
                            CVTRPN.Checked = True
                        End If
                        If (tmp And 8) = 8 Then
                            cr.Checked = True
                        End If
                    End If
                End While
                sr.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Function iniparse(ByVal s As String) As String
        s = s.Remove(0, s.LastIndexOf("=") + 1)
        Return s
    End Function

    Private Function iniparse2(ByVal s As String) As Single
        s = s.Remove(0, s.LastIndexOf("=") + 1)
        Return CSng(s)
    End Function

    Private Sub fc(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Dim sr As New System.IO.StreamWriter("conf", False, System.Text.Encoding.GetEncoding(932))
        Dim s As String
        s = "ADDR" & ADDR.Text
        sr.WriteLine(s)
        s = "MODE" & MODE.Text
        sr.WriteLine(s)
        s = "ENC" & cpg.ToString
        sr.WriteLine(s)
        s = "SEL" & sel.ToString
        sr.WriteLine(s)
        s = "TMSB" & tmp.ToString
        sr.WriteLine(s)
        s = "CMSB" & cmf.ToString
        sr.WriteLine(s)
        s = "FONT" & ASM.Font.ToString
        sr.WriteLine(s)
        If RPN.Checked Then
            Dim k As Integer = 1
            If LOOKSORDER.Checked = True Then
                k += 2
            End If
            If CVTRPN.Checked = True Then
                k += 4
            End If
            If cr.Checked = True Then
                k += 8
            End If
            s = "RPN" & k.ToString
            sr.WriteLine(s)
        End If
        sr.Close()

    End Sub


    Private Sub ffe(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles ASM.DragEnter, CODE.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub


    Private Sub ffd(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles ASM.DragDrop
        Dim fn As String() = CType( _
               e.Data.GetData(DataFormats.FileDrop, False), _
               String())
        If File.Exists(fn(0)) Then
            Dim sr As New StreamReader(fn(0), Encoding.GetEncoding(cpg))
            ASM.Text = sr.ReadToEnd()
            sr.Close()
        End If
    End Sub


    Private Sub ffc(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles CODE.DragDrop
        Dim fn As String() = CType( _
               e.Data.GetData(DataFormats.FileDrop, False), _
               String())
        If File.Exists(fn(0)) Then
            Dim ext As String = Path.GetExtension(fn(0))
            If ext = ".bin" Or ext = ".pat" Then
                bin2txt(fn(0), ext)
            Else
                Dim sr As New StreamReader(fn(0), Encoding.GetEncoding(cpg))
                CODE.Text = sr.ReadToEnd()
                sr.Close()
            End If
        End If
    End Sub


    Function str2bin(ByVal temp As String) As Byte()
        temp = temp.Replace("0x", "")
        Dim num(3) As Integer
        Dim bytes(3) As Byte
        For i = 0 To 3
            num(i) = Convert.ToInt32(temp.Substring(6 - 2 * i, 2), 16)
            bytes(i) = Convert.ToByte(num(i))
        Next
        Return bytes
    End Function

    'IEE754単精度浮動小数点binary32を半精度浮動小数点binary16に変換 Cから移植、VB.NET
    Function converthalffloat(ByVal hf As String) As String
        Dim hex As Integer = Convert.ToInt32(hf, 16)
        Dim sign As Integer = (hex >> 31) And 1
        Dim exponent As Integer = (hex >> 23) And &HFF
        Dim fraction As Integer = (hex And &H7FFFFF)

        '        WebSVN()
        'psp - Rev 2457
        '        Subversion(Repositories)
        'Rev:
        '(root)/trunk/prxtool/disasm.C @ 2457
        'Rev 2206 - Blame - Compare with Previous - Last modification - View Log - RSS feed

        '/***************************************************************
        ' * PRXTool : Utility for PSP executables.
        ' * (c) TyRaNiD 2k6
        ' *
        ' * disasm.C - Implementation of a MIPS disassembler
        ' ***************************************************************/
        '/* VFPU 16-bit floating-point format. */ from psplinksource
        '#define VFPU_FLOAT16_EXP_MAX    0x1f
        '#define VFPU_SH_FLOAT16_SIGN    15
        '#define VFPU_MASK_FLOAT16_SIGN  0x1
        '#define VFPU_SH_FLOAT16_EXP     10
        '#define VFPU_MASK_FLOAT16_EXP   0x1f
        '#define VFPU_SH_FLOAT16_FRAC    0
        '#define VFPU_MASK_FLOAT16_FRAC  0x3ff
        '        /* Convert a VFPU 16-bit floating-point number to IEEE754. */
        '        unsigned int float2int=0;
        '        unsigned short float16 = addresscode & 0xFFFF;
        '        unsigned int sign = (float16 >> VFPU_SH_FLOAT16_SIGN) & VFPU_MASK_FLOAT16_SIGN;
        '        int exponent = (float16 >> VFPU_SH_FLOAT16_EXP) & VFPU_MASK_FLOAT16_EXP;
        '        unsigned int fraction = float16 & VFPU_MASK_FLOAT16_FRAC;
        '        float2int = (sign << 31) + ((exponent + 112) << 23) + (fraction << 13);
        exponent -= 112
        If exponent > 30 Then
            exponent = 31
        ElseIf exponent < 0 Then
            exponent = 0
            fraction = 0
        End If

        exponent <<= 10
        fraction >>= 13
        sign <<= 15
        hex = exponent + fraction
        hex = hex And (&H7FFF)
        If hex >= &H7C00 Then '無限
            hex = &H7C00
            'hex = &H7F80 '数字以外のなにか
        End If
        hex += sign
        hf = hex.ToString("X").PadLeft(4, "0"c)

        Return hf
    End Function

    'IEE754単精度浮動小数点binary32を半精度浮動小数点binary16に変換の逆 Cから移植、VB.NET
    Function converthalffloat2(ByVal s As String) As String
        Dim hex As Integer = Convert.ToInt32(s, 16)
        Dim sign As Integer = (hex >> 15) And 1
        Dim exponent As Integer = (hex >> 10) And &H1F
        Dim fraction As Integer = (hex And &H3FF)
        Dim hf As String

        'WebSVN()
        'psp - Rev 2457
        '        Subversion(Repositories)
        'Rev:
        '(root)/trunk/prxtool/disasm.C @ 2457
        'Rev 2206 - Blame - Compare with Previous - Last modification - View Log - RSS feed

        '/***************************************************************
        ' * PRXTool : Utility for PSP executables.
        ' * (c) TyRaNiD 2k6
        ' *
        ' * disasm.C - Implementation of a MIPS disassembler
        ' ***************************************************************/
        '/* VFPU 16-bit floating-point format. */ from psplinksource
        '#define VFPU_FLOAT16_EXP_MAX    0x1f
        '#define VFPU_SH_FLOAT16_SIGN    15
        '#define VFPU_MASK_FLOAT16_SIGN  0x1
        '#define VFPU_SH_FLOAT16_EXP     10
        '#define VFPU_MASK_FLOAT16_EXP   0x1f
        '#define VFPU_SH_FLOAT16_FRAC    0
        '#define VFPU_MASK_FLOAT16_FRAC  0x3ff
        '        /* Convert a VFPU 16-bit floating-point number to IEEE754. */
        '        unsigned int float2int=0;
        '        unsigned short float16 = addresscode & 0xFFFF;
        '        unsigned int sign = (float16 >> VFPU_SH_FLOAT16_SIGN) & VFPU_MASK_FLOAT16_SIGN;
        '        int exponent = (float16 >> VFPU_SH_FLOAT16_EXP) & VFPU_MASK_FLOAT16_EXP;
        '        unsigned int fraction = float16 & VFPU_MASK_FLOAT16_FRAC;
        '        float2int = (sign << 31) + ((exponent + 112) << 23) + (fraction << 13);
        If exponent = 0 AndAlso fraction = 0 Then
        Else
            exponent += 112
        End If
        exponent <<= 23
        fraction <<= 13
        sign <<= 31
        hex = exponent + fraction
        hex = hex And &H7FFFFFFF
        hex += sign
        hf = hex.ToString("X8")

        Return hf
    End Function

#Region "decoder配列"

    Dim decoder As String() = {
 "nop", "0x00000000", "0xFFFFFFFF", "",
"li", "0x24000000", "0xFFE00000", "%t,%i",
"li", "0x34000000", "0xFFE00000", "%t,%I",
"move", "0x00000021", "0xFC1F07FF", "%d,%s",
"move", "0x00000025", "0xFC1F07FF", "%d,%s",
"b", "0x10000000", "0xFFFF0000", "%O",
"b", "0x04010000", "0xFFFF0000", "%O",
"bal", "0x04110000", "0xFFFF0000", "%O",
"bnez", "0x14000000", "0xFC1F0000", "%s,%O",
"bnezl", "0x54000000", "0xFC1F0000", "%s,%O",
"beqz", "0x10000000", "0xFC1F0000", "%s,%O",
"beqzl", "0x50000000", "0xFC1F0000", "%s,%O",
"neg", "0x00000022", "0xFFE007FF", "%d,%t",
"negu", "0x00000023", "0xFFE007FF", "%d,%t",
"not", "0x00000027", "0xFC1F07FF", "%d,%s",
"jalr", "0x0000F809", "0xFC1FFFFF", "%J",
"add", "0x00000020", "0xFC0007FF", "%d,%s,%t",
"addi", "0x20000000", "0xFC000000", "%t,%s,%i",
"addiu", "0x24000000", "0xFC000000", "%t,%s,%i",
"addu", "0x00000021", "0xFC0007FF", "%d,%s,%t",
"and", "0x00000024", "0xFC0007FF", "%d,%s,%t",
"andi", "0x30000000", "0xFC000000", "%t,%s,%I",
"beq", "0x10000000", "0xFC000000", "%s,%t,%O",
"beql", "0x50000000", "0xFC000000", "%s,%t,%O",
"bgez", "0x04010000", "0xFC1F0000", "%s,%O",
"bgezal", "0x04110000", "0xFC1F0000", "%s,%O",
"bgezl", "0x04030000", "0xFC1F0000", "%s,%O",
"bgtz", "0x1C000000", "0xFC1F0000", "%s,%O",
"bgtzl", "0x5C000000", "0xFC1F0000", "%s,%O",
"bitrev", "0x7C000520", "0xFFE007FF", "%d,%t",
"blez", "0x18000000", "0xFC1F0000", "%s,%O",
"blezl", "0x58000000", "0xFC1F0000", "%s,%O",
"bltz", "0x04000000", "0xFC1F0000", "%s,%O",
"bltzl", "0x04020000", "0xFC1F0000", "%s,%O",
"bltzal", "0x04100000", "0xFC1F0000", "%s,%O",
"bltzall", "0x04120000", "0xFC1F0000", "%s,%O",
"bne", "0x14000000", "0xFC000000", "%s,%t,%O",
"bnel", "0x54000000", "0xFC000000", "%s,%t,%O",
"break", "0x0000000D", "0xFC00003F", "%c",
"cache", "0xbc000000", "0xfc000000", "%k,%o",
"cfc0", "0x40400000", "0xFFE007FF", "%t,%p",
"clo", "0x00000017", "0xFC1F07FF", "%d,%s",
"clz", "0x00000016", "0xFC1F07FF", "%d,%s",
"ctc0", "0x40C00000", "0xFFE007FF", "%t,%p",
"max", "0x0000002C", "0xFC0007FF", "%d,%s,%t",
"min", "0x0000002D", "0xFC0007FF", "%d,%s,%t",
"dbreak", "0x7000003F", "0xFFFFFFFF", "",
"div", "0x0000001A", "0xFC00FFFF", "%s,%t",
"divu", "0x0000001B", "0xFC00FFFF", "%s,%t",
"dret", "0x7000003E", "0xFFFFFFFF", "",
"eret", "0x42000018", "0xFFFFFFFF", "",
"ext", "0x7C000000", "0xFC00003F", "%t,%s,%a,%ne",
"ins", "0x7C000004", "0xFC00003F", "%t,%s,%a,%ni",
"j", "0x08000000", "0xFC000000", "%j",
"jr", "0x00000008", "0xFC1FFFFF", "%J",
"jalr", "0x00000009", "0xFC1F07FF", "%d,%J",
"jal", "0x0C000000", "0xFC000000", "%j",
"lb", "0x80000000", "0xFC000000", "%t,%o",
"lbu", "0x90000000", "0xFC000000", "%t,%o",
"lh", "0x84000000", "0xFC000000", "%t,%o",
"lhu", "0x94000000", "0xFC000000", "%t,%o",
"ll", "0xC0000000", "0xFC000000", "%t,%o",
"lui", "0x3C000000", "0xFFE00000", "%t,%I",
"lw", "0x8C000000", "0xFC000000", "%t,%o",
"lwl", "0x88000000", "0xFC000000", "%t,%o",
"lwr", "0x98000000", "0xFC000000", "%t,%o",
"madd", "0x0000001C", "0xFC00FFFF", "%s,%t",
"maddu", "0x0000001D", "0xFC00FFFF", "%s,%t",
"mfc0", "0x40000000", "0xFFE007FF", "%t,%0",
"mfdr", "0x7000003D", "0xFFE007FF", "%t,%r",
"mfhi", "0x00000010", "0xFFFF07FF", "%d",
"mfic", "0x70000024", "0xFFE007FF", "%t,%p",
"mflo", "0x00000012", "0xFFFF07FF", "%d",
"movn", "0x0000000B", "0xFC0007FF", "%d,%s,%t",
"movz", "0x0000000A", "0xFC0007FF", "%d,%s,%t",
"msub", "0x0000002e", "0xfc00ffff", "%s,%t",
"msubu", "0x0000002f", "0xfc00ffff", "%s,%t",
"mtc0", "0x40800000", "0xFFE007FF", "%t,%0",
"mtdr", "0x7080003D", "0xFFE007FF", "%t,%r",
"mtic", "0x70000026", "0xFFE007FF", "%t,%p",
"halt", "0x70000000", "0xFFFFFFFF", "",
"mthi", "0x00000011", "0xFC1FFFFF", "%s",
"mtlo", "0x00000013", "0xFC1FFFFF", "%s",
"mult", "0x00000018", "0xFC00FFFF", "%s,%t",
"multu", "0x00000019", "0xFC0007FF", "%s,%t",
"nor", "0x00000027", "0xFC0007FF", "%d,%s,%t",
"or", "0x00000025", "0xFC0007FF", "%d,%s,%t",
"ori", "0x34000000", "0xFC000000", "%t,%s,%I",
"rotr", "0x00200002", "0xFFE0003F", "%d,%t,%a",
"rotv", "0x00000046", "0xFC0007FF", "%d,%t,%s",
"seb", "0x7C000420", "0xFFE007FF", "%d,%t",
"seh", "0x7C000620", "0xFFE007FF", "%d,%t",
"sb", "0xA0000000", "0xFC000000", "%t,%o",
"sh", "0xA4000000", "0xFC000000", "%t,%o",
"sc", "0xE0000000", "0xFC000000", "%t, %o",
"sllv", "0x00000004", "0xFC0007FF", "%d,%t,%s",
"sll", "0x00000000", "0xFFE0003F", "%d,%t,%a",
"slt", "0x0000002A", "0xFC0007FF", "%d,%s,%t",
"slti", "0x28000000", "0xFC000000", "%t,%s,%i",
"sltiu", "0x2C000000", "0xFC000000", "%t,%s,%I",
"sltu", "0x0000002B", "0xFC0007FF", "%d,%s,%t",
"sra", "0x00000003", "0xFFE0003F", "%d,%t,%a",
"srav", "0x00000007", "0xFC0007FF", "%d,%t,%s",
"srlv", "0x00000006", "0xFC0007FF", "%d,%t,%s",
"srl", "0x00000002", "0xFFE0003F", "%d,%t,%a",
"sw", "0xAC000000", "0xFC000000", "%t,%o",
"swl", "0xA8000000", "0xFC000000", "%t,%o",
"swr", "0xB8000000", "0xFC000000", "%t,%o",
"sub", "0x00000022", "0xFC0007FF", "%d,%s,%t",
"subu", "0x00000023", "0xFC0007FF", "%d,%s,%t",
"sync", "0x0000000F", "0xFFFFFFFF", "",
"syscall", "0x0000000C", "0xFC00003F", "%C",
"xor", "0x00000026", "0xFC0007FF", "%d,%s,%t",
"xori", "0x38000000", "0xFC000000", "%t,%s,%I",
"wsbh", "0x7C0000A0", "0xFFE007FF", "%d,%t",
"wsbw", "0x7C0000E0", "0xFFE007FF", "%d,%t",
"abs.s", "0x46000005", "0xFFFF003F", "%D,%S",
"add.s", "0x46000000", "0xFFE0003F", "%D,%S,%T",
"bc1f", "0x45000000", "0xFFFF0000", "%O",
"bc1fl", "0x45020000", "0xFFFF0000", "%O",
"bc1t", "0x45010000", "0xFFFF0000", "%O",
"bc1tl", "0x45030000", "0xFFFF0000", "%O",
"c.f.s", "0x46000030", "0xFFE007FF", "%S,%T",
"c.un.s", "0x46000031", "0xFFE007FF", "%S,%T",
"c.eq.s", "0x46000032", "0xFFE007FF", "%S,%T",
"c.ueq.s", "0x46000033", "0xFFE007FF", "%S,%T",
"c.olt.s", "0x46000034", "0xFFE007FF", "%S,%T",
"c.ult.s", "0x46000035", "0xFFE007FF", "%S,%T",
"c.ole.s", "0x46000036", "0xFFE007FF", "%S,%T",
"c.ule.s", "0x46000037", "0xFFE007FF", "%S,%T",
"c.sf.s", "0x46000038", "0xFFE007FF", "%S,%T",
"c.ngle.s", "0x46000039", "0xFFE007FF", "%S,%T",
"c.seq.s", "0x4600003A", "0xFFE007FF", "%S,%T",
"c.ngl.s", "0x4600003B", "0xFFE007FF", "%S,%T",
"c.lt.s", "0x4600003C", "0xFFE007FF", "%S,%T",
"c.nge.s", "0x4600003D", "0xFFE007FF", "%S,%T",
"c.le.s", "0x4600003E", "0xFFE007FF", "%S,%T",
"c.ngt.s", "0x4600003F", "0xFFE007FF", "%S,%T",
"ceil.w.s", "0x4600000E", "0xFFFF003F", "%D,%S",
"cfc1", "0x44400000", "0xFFE007FF", "%t,%p",
"ctc1", "0x44c00000", "0xFFE007FF", "%t,%p",
"cvt.s.w", "0x46800020", "0xFFFF003F", "%D,%S",
"cvt.w.s", "0x46000024", "0xFFFF003F", "%D,%S",
"div.s", "0x46000003", "0xFFE0003F", "%D,%S,%T",
"floor.w.s", "0x4600000F", "0xFFFF003F", "%D,%S",
"lwc1", "0xc4000000", "0xFC000000", "%T,%o",
"mfc1", "0x44000000", "0xFFE007FF", "%t,%1",
"mov.s", "0x46000006", "0xFFFF003F", "%D,%S",
"mtc1", "0x44800000", "0xFFE007FF", "%t,%1",
"mul.s", "0x46000002", "0xFFE0003F", "%D,%S,%T",
"neg.s", "0x46000007", "0xFFFF003F", "%D,%S",
"round.w.s", "0x4600000C", "0xFFFF003F", "%D,%S",
"sqrt.s", "0x46000004", "0xFFFF003F", "%D,%S",
"sub.s", "0x46000001", "0xFFE0003F", "%D,%S,%T",
"swc1", "0xe4000000", "0xFC000000", "%T,%o",
"trunc.w.s", "0x4600000D", "0xFFFF003F", "%D,%S",
"bvf", "0x49000000", "0xFFE30000", "%Zc,%O",
"bvfl", "0x49020000", "0xFFE30000", "%Zc,%O",
"bvt", "0x49010000", "0xFFE30000", "%Zc,%O",
"bvtl", "0x49030000", "0xFFE30000", "%Zc,%O",
"lv.q", "0xD8000000", "0xFC000002", "%Xq,%Y",
"lv.s", "0xC8000000", "0xFC000000", "%Xs,%Y",
"lvl.q", "0xD4000000", "0xFC000002", "%Xq,%Y",
"lvr.q", "0xD4000002", "0xFC000002", "%Xq,%Y",
"mfv", "0x48600000", "0xFFE0FF80", "%t,%zs",
"mfvc", "0x48600000", "0xFFE0FF00", "%t,%2d",
"mtv", "0x48E00000", "0xFFE0FF80", "%t,%zs",
"mtvc", "0x48E00000", "0xFFE0FF00", "%t,%2d",
"sv.q", "0xF8000000", "0xFC000002", "%Xq,%Y",
"sv.s", "0xE8000000", "0xFC000000", "%Xs,%Y",
"svl.q", "0xF4000000", "0xFC000002", "%Xq,%Y",
"svr.q", "0xF4000002", "0xFC000002", "%Xq,%Y",
"vabs.p", "0xD0010080", "0xFFFF8080", "%zp,%yp",
"vabs.q", "0xD0018080", "0xFFFF8080", "%zq,%yq",
"vabs.s", "0xD0010000", "0xFFFF8080", "%zs,%ys",
"vabs.t", "0xD0018000", "0xFFFF8080", "%zt,%yt",
"vadd.p", "0x60000080", "0xFF808080", "%zp,%yp,%xp",
"vadd.q", "0x60008080", "0xFF808080", "%zq,%yq,%xq",
"vadd.s", "0x60000000", "0xFF808080", "%zs,%ys,%xs",
"vadd.t", "0x60008000", "0xFF808080", "%zt,%yt,%xt",
"vasin.p", "0xD0170080", "0xFFFF8080", "%zp,%yp",
"vasin.q", "0xD0178080", "0xFFFF8080", "%zq,%yq",
"vasin.s", "0xD0170000", "0xFFFF8080", "%zs,%ys",
"vasin.t", "0xD0178000", "0xFFFF8080", "%zt,%yt",
"vavg.s", "0xD0470000", "0xFFFF8080", "%zs,%ys",
"vavg.p", "0xD0470080", "0xFFFF8080", "%zs,%yp",
"vavg.q", "0xD0478080", "0xFFFF8080", "%zs,%yq",
"vavg.t", "0xD0478000", "0xFFFF8080", "%zs,%yt",
"vbfy1.p", "0xD0420080", "0xFFFF8080", "%zp,%yp",
"vbfy1.q", "0xD0428080", "0xFFFF8080", "%zq,%yq",
"vbfy2.q", "0xD0438080", "0xFFFF8080", "%zq,%yq",
"vcmovf.p", "0xD2A80080", "0xFFF88080", "%zp,%yp,%v3",
"vcmovf.q", "0xD2A88080", "0xFFF88080", "%zq,%yq,%v3",
"vcmovf.s", "0xD2A80000", "0xFFF88080", "%zs,%ys,%v3",
"vcmovf.t", "0xD2A88000", "0xFFF88080", "%zt,%yt,%v3",
"vcmovt.p", "0xD2A00080", "0xFFF88080", "%zp,%yp,%v3",
"vcmovt.q", "0xD2A08080", "0xFFF88080", "%zq,%yq,%v3",
"vcmovt.s", "0xD2A00000", "0xFFF88080", "%zs,%ys,%v3",
"vcmovt.t", "0xD2A08000", "0xFFF88080", "%zt,%yt,%v3",
"vcmp.p", "0x6C000080", "0xFF8080F0", "%Zn,%yp,%xp",
"vcmp.p", "0x6C000080", "0xFFFF80F0", "%Zn,%yp",
"vcmp.p", "0x6C000080", "0xFFFFFFF0", "%Zn",
"vcmp.q", "0x6C008080", "0xFF8080F0", "%Zn,%yq,%xq",
"vcmp.q", "0x6C008080", "0xFFFF80F0", "%Zn,%yq",
"vcmp.q", "0x6C008080", "0xFFFFFFF0", "%Zn",
"vcmp.s", "0x6C000000", "0xFF8080F0", "%Zn,%ys,%xs",
"vcmp.s", "0x6C000000", "0xFFFF80F0", "%Zn,%ys",
"vcmp.s", "0x6C000000", "0xFFFFFFF0", "%Zn",
"vcmp.t", "0x6C008000", "0xFF8080F0", "%Zn,%yt,%xt",
"vcmp.t", "0x6C008000", "0xFFFF80F0", "%Zn,%yt",
"vcmp.t", "0x6C008000", "0xFFFFFFF0", "%Zn",
"vcos.p", "0xD0130080", "0xFFFF8080", "%zp,%yp",
"vcos.q", "0xD0138080", "0xFFFF8080", "%zq,%yq",
"vcos.s", "0xD0130000", "0xFFFF8080", "%zs,%ys",
"vcos.t", "0xD0138000", "0xFFFF8080", "%zt,%yt",
"vcrs.t", "0x66808000", "0xFF808080", "%zt,%yt,%xt",
"vcrsp.t", "0xF2808000", "0xFF808080", "%zt,%yt,%xt",
"vcst.p", "0xD0600080", "0xFFE0FF80", "%zp,%vk",
"vcst.q", "0xD0608080", "0xFFE0FF80", "%zq,%vk",
"vcst.s", "0xD0600000", "0xFFE0FF80", "%zs,%vk",
"vcst.t", "0xD0608000", "0xFFE0FF80", "%zt,%vk",
"vdet.s", "0x67000000", "0xFF808080", "%zs,%ys,%xs",
"vdet.t", "0x67008000", "0xFF808080", "%zs,%yt,%xt",
"vdet.p", "0x67000080", "0xFF808080", "%zs,%yp,%xp",
"vdet.q", "0x67008080", "0xFF808080", "%zs,%yq,%xq",
"vdiv.p", "0x63800080", "0xFF808080", "%zp,%yp,%xp",
"vdiv.q", "0x63808080", "0xFF808080", "%zq,%yq,%xq",
"vdiv.s", "0x63800000", "0xFF808080", "%zs,%ys,%xs",
"vdiv.t", "0x63808000", "0xFF808080", "%zt,%yt,%xt",
"vdot.s", "0x64800000", "0xFF808080", "%zs,%ys,%xs",
"vdot.p", "0x64800080", "0xFF808080", "%zs,%yp,%xp",
"vdot.q", "0x64808080", "0xFF808080", "%zs,%yq,%xq",
"vdot.t", "0x64808000", "0xFF808080", "%zs,%yt,%xt",
"vexp2.p", "0xD0140080", "0xFFFF8080", "%zp,%yp",
"vexp2.q", "0xD0148080", "0xFFFF8080", "%zq,%yq",
"vexp2.s", "0xD0140000", "0xFFFF8080", "%zs,%ys",
"vexp2.t", "0xD0148000", "0xFFFF8080", "%zt,%yt",
"vf2h.p", "0xD0320080", "0xFFFF8080", "%zs,%yp",
"vf2h.q", "0xD0328080", "0xFFFF8080", "%zp,%yq",
"vf2h.s", "0xD0320000", "0xFFFF8080", "%zs, %ys",
"vf2h.t", "0xD0328000", "0xFFFF8080", "%zp, %yt",
"vf2id.p", "0xD2600080", "0xFFE08080", "%zp,%yp,%v5",
"vf2id.q", "0xD2608080", "0xFFE08080", "%zq,%yq,%v5",
"vf2id.s", "0xD2600000", "0xFFE08080", "%zs,%ys,%v5",
"vf2id.t", "0xD2608000", "0xFFE08080", "%zt,%yt,%v5",
"vf2in.p", "0xD2000080", "0xFFE08080", "%zp,%yp,%v5",
"vf2in.q", "0xD2008080", "0xFFE08080", "%zq,%yq,%v5",
"vf2in.s", "0xD2000000", "0xFFE08080", "%zs,%ys,%v5",
"vf2in.t", "0xD2008000", "0xFFE08080", "%zt,%yt,%v5",
"vf2iu.p", "0xD2400080", "0xFFE08080", "%zp,%yp,%v5",
"vf2iu.q", "0xD2408080", "0xFFE08080", "%zq,%yq,%v5",
"vf2iu.s", "0xD2400000", "0xFFE08080", "%zs,%ys,%v5",
"vf2iu.t", "0xD2408000", "0xFFE08080", "%zt,%yt,%v5",
"vf2iz.p", "0xD2200080", "0xFFE08080", "%zp,%yp,%v5",
"vf2iz.q", "0xD2208080", "0xFFE08080", "%zq,%yq,%v5",
"vf2iz.s", "0xD2200000", "0xFFE08080", "%zs,%ys,%v5",
"vf2iz.t", "0xD2208000", "0xFFE08080", "%zt,%yt,%v5",
"vfad.s", "0xD0460000", "0xFFFF8080", "%zs,%ys",
"vfad.p", "0xD0460080", "0xFFFF8080", "%zs,%yp",
"vfad.q", "0xD0468080", "0xFFFF8080", "%zs,%yq",
"vfad.t", "0xD0468000", "0xFFFF8080", "%zs,%yt",
"vfim.s", "0xDF800000", "0xFF800000", "%xs,%vh",
"vflush", "0xFFFF040D", "0xFFFFFFFF", "",
"vh2f.p", "0xD0330080", "0xFFFF8080", "%zq,%yp",
"vh2f.s", "0xD0330000", "0xFFFF8080", "%zp,%ys",
"vh2f.q", "0xD0338080", "0xFFFF8080", "%zq, %yp",
"vh2f.t", "0xD0330000", "0xFFFF8080", "%zq, %yp",
"vhdp.p", "0x66000080", "0xFF808080", "%zs,%yp,%xp",
"vhdp.q", "0x66008080", "0xFF808080", "%zs,%yq,%xq",
"vhdp.t", "0x66008000", "0xFF808080", "%zs,%yt,%xt",
"vhtfm2.p", "0xF0800000", "0xFF808080", "%zp,%ym,%xp",
"vhtfm3.t", "0xF1000080", "0xFF808080", "%zt,%yn,%xt",
"vhtfm4.q", "0xF1808000", "0xFF808080", "%zq,%yo,%xq",
"vi2c.q", "0xD03D8080", "0xFFFF8080", "%zs,%yq",
"vi2f.p", "0xD2800080", "0xFFE08080", "%zp,%yp,%v5",
"vi2f.q", "0xD2808080", "0xFFE08080", "%zq,%yq,%v5",
"vi2f.s", "0xD2800000", "0xFFE08080", "%zs,%ys,%v5",
"vi2f.t", "0xD2808000", "0xFFE08080", "%zt,%yt,%v5",
"vi2s.p", "0xD03F0080", "0xFFFF8080", "%zs,%yp",
"vi2s.q", "0xD03F8080", "0xFFFF8080", "%zp,%yq",
"vi2uc.q", "0xD03C8080", "0xFFFF8080", "%zs,%yq",
"vi2us.p", "0xD03E0080", "0xFFFF8080", "%zs,%yq",
"vi2us.q", "0xD03E8080", "0xFFFF8080", "%zp,%yq",
"vidt.p", "0xD0030080", "0xFFFFFF80", "%zp",
"vidt.q", "0xD0038080", "0xFFFFFF80", "%zq",
"viim.s", "0xDF000000", "0xFF800000", "%xs,%vi",
"vlgb.s", "0xD0370000", "0xFFFF8080", "%zs,%ys",
"vlog2.p", "0xD0150080", "0xFFFF8080", "%zp,%yp",
"vlog2.q", "0xD0158080", "0xFFFF8080", "%zq,%yq",
"vlog2.s", "0xD0150000", "0xFFFF8080", "%zs,%ys",
"vlog2.t", "0xD0158000", "0xFFFF8080", "%zt,%yt",
"vmax.p", "0x6D800080", "0xFF808080", "%zp,%yp,%xp",
"vmax.q", "0x6D808080", "0xFF808080", "%zq,%yq,%xq",
"vmax.s", "0x6D800000", "0xFF808080", "%zs,%ys,%xs",
"vmax.t", "0x6D808000", "0xFF808080", "%zt,%yt,%xt",
"vmfvc", "0xD0500000", "0xFFFF0080", "%zs,%2s",
"vmidt.p", "0xF3830080", "0xFFFFFF80", "%zm",
"vmidt.q", "0xF3838080", "0xFFFFFF80", "%zo",
"vmidt.t", "0xF3838000", "0xFFFFFF80", "%zn",
"vmin.p", "0x6D000080", "0xFF808080", "%zp,%yp,%xp",
"vmin.q", "0x6D008080", "0xFF808080", "%zq,%yq,%xq",
"vmin.s", "0x6D000000", "0xFF808080", "%zs,%ys,%xs",
"vmin.t", "0x6D008000", "0xFF808080", "%zt,%yt,%xt",
"vmmov.p", "0xF3800080", "0xFFFF8080", "%zm,%ym",
"vmmov.q", "0xF3808080", "0xFFFF8080", "%zo,%yo",
"vmmov.t", "0xF3808000", "0xFFFF8080", "%zn,%yn",
"vmmul.p", "0xF0000080", "0xFF808080", "%?%zm,%ym,%xm",
"vmmul.q", "0xF0008080", "0xFF808080", "%?%zo,%yo,%xo",
"vmmul.t", "0xF0008000", "0xFF808080", "%?%zn,%yn,%xn",
"vmone.p", "0xF3870080", "0xFFFFFF80", "%zm",
"vmone.q", "0xF3878080", "0xFFFFFF80", "%zo",
"vmone.t", "0xF3878000", "0xFFFFFF80", "%zn",
"vmov.p", "0xD0000080", "0xFFFF8080", "%zp,%yp",
"vmov.q", "0xD0008080", "0xFFFF8080", "%zq,%yq",
"vmov.s", "0xD0000000", "0xFFFF8080", "%zs,%ys",
"vmov.t", "0xD0008000", "0xFFFF8080", "%zt,%yt",
"vmscl.p", "0xF2000080", "0xFF808080", "%zm,%ym,%xs",
"vmscl.q", "0xF2008080", "0xFF808080", "%zo,%yo,%xs",
"vmscl.t", "0xF2008000", "0xFF808080", "%zn,%yn,%xs",
"vmtvc", "0xD0510000", "0xFFFF8000", "%2d,%ys",
"vmul.p", "0x64000080", "0xFF808080", "%zp,%yp,%xp",
"vmul.q", "0x64008080", "0xFF808080", "%zq,%yq,%xq",
"vmul.s", "0x64000000", "0xFF808080", "%zs,%ys,%xs",
"vmul.t", "0x64008000", "0xFF808080", "%zt,%yt,%xt",
"vmzero.p", "0xF3860080", "0xFFFFFF80", "%zm",
"vmzero.q", "0xF3868080", "0xFFFFFF80", "%zo",
"vmzero.t", "0xF3868000", "0xFFFFFF80", "%zn",
"vneg.p", "0xD0020080", "0xFFFF8080", "%zp,%yp",
"vneg.q", "0xD0028080", "0xFFFF8080", "%zq,%yq",
"vneg.s", "0xD0020000", "0xFFFF8080", "%zs,%ys",
"vneg.t", "0xD0028000", "0xFFFF8080", "%zt,%yt",
"vnop", "0xFFFF0000", "0xFFFFFFFF", "",
"vnrcp.p", "0xD0180080", "0xFFFF8080", "%zp,%yp",
"vnrcp.q", "0xD0188080", "0xFFFF8080", "%zq,%yq",
"vnrcp.s", "0xD0180000", "0xFFFF8080", "%zs,%ys",
"vnrcp.t", "0xD0188000", "0xFFFF8080", "%zt,%yt",
"vnsin.p", "0xD01A0080", "0xFFFF8080", "%zp,%yp",
"vnsin.q", "0xD01A8080", "0xFFFF8080", "%zq,%yq",
"vnsin.s", "0xD01A0000", "0xFFFF8080", "%zs,%ys",
"vnsin.t", "0xD01A8000", "0xFFFF8080", "%zt,%yt",
"vocp.p", "0xD0440080", "0xFFFF8080", "%zp,%yp",
"vocp.q", "0xD0448080", "0xFFFF8080", "%zq,%yq",
"vocp.s", "0xD0440000", "0xFFFF8080", "%zs,%ys",
"vocp.t", "0xD0448000", "0xFFFF8080", "%zt,%yt",
"vone.p", "0xD0070080", "0xFFFFFF80", "%zp",
"vone.q", "0xD0078080", "0xFFFFFF80", "%zq",
"vone.s", "0xD0070000", "0xFFFFFF80", "%zs",
"vone.t", "0xD0078000", "0xFFFFFF80", "%zt",
"vpfxd", "0xDE000000", "0xFF000000", "[%vp4,%vp5,%vp6,%vp7]",
"vpfxs", "0xDC000000", "0xFF000000", "[%vp0,%vp1,%vp2,%vp3]",
"vpfxt", "0xDD000000", "0xFF000000", "[%vp0,%vp1,%vp2,%vp3]",
"vqmul.q", "0xF2808080", "0xFF808080", "%zq,%yq,%xq",
"vrcp.p", "0xD0100080", "0xFFFF8080", "%zp,%yp",
"vrcp.q", "0xD0108080", "0xFFFF8080", "%zq,%yq",
"vrcp.s", "0xD0100000", "0xFFFF8080", "%zs,%ys",
"vrcp.t", "0xD0108000", "0xFFFF8080", "%zt,%yt",
"vrexp2.p", "0xD01C0080", "0xFFFF8080", "%zp,%yp",
"vrexp2.q", "0xD01C8080", "0xFFFF8080", "%zq,%yq",
"vrexp2.s", "0xD01C0000", "0xFFFF8080", "%zs,%ys",
"vrexp2.t", "0xD01C8000", "0xFFFF8080", "%zt,%yt",
"vrndf1.p", "0xD0220080", "0xFFFFFF80", "%zp",
"vrndf1.q", "0xD0228080", "0xFFFFFF80", "%zq",
"vrndf1.s", "0xD0220000", "0xFFFFFF80", "%zs",
"vrndf1.t", "0xD0228000", "0xFFFFFF80", "%zt",
"vrndf2.p", "0xD0230080", "0xFFFFFF80", "%zp",
"vrndf2.q", "0xD0238080", "0xFFFFFF80", "%zq",
"vrndf2.s", "0xD0230000", "0xFFFFFF80", "%zs",
"vrndf2.t", "0xD0238000", "0xFFFFFF80", "%zt",
"vrndi.p", "0xD0210080", "0xFFFFFF80", "%zp",
"vrndi.q", "0xD0218080", "0xFFFFFF80", "%zq",
"vrndi.s", "0xD0210000", "0xFFFFFF80", "%zs",
"vrndi.t", "0xD0218000", "0xFFFFFF80", "%zt",
"vrnds.s", "0xD0200000", "0xFFFF80FF", "%ys",
"vrot.s", "0xF3A00000", "0xFFE08080", "%zs,%ys,%vr",
"vrot.p", "0xF3A00080", "0xFFE08080", "%zp,%ys,%vr",
"vrot.q", "0xF3A08080", "0xFFE08080", "%zq,%ys,%vr",
"vrot.t", "0xF3A08000", "0xFFE08080", "%zt,%ys,%vr",
"vrsq.p", "0xD0110080", "0xFFFF8080", "%zp,%yp",
"vrsq.q", "0xD0118080", "0xFFFF8080", "%zq,%yq",
"vrsq.s", "0xD0110000", "0xFFFF8080", "%zs,%ys",
"vrsq.t", "0xD0118000", "0xFFFF8080", "%zt,%yt",
"vs2i.p", "0xD03B0080", "0xFFFF8080", "%zq,%yp",
"vs2i.s", "0xD03B0000", "0xFFFF8080", "%zp,%ys",
"vsat0.p", "0xD0040080", "0xFFFF8080", "%zp,%yp",
"vsat0.q", "0xD0048080", "0xFFFF8080", "%zq,%yq",
"vsat0.s", "0xD0040000", "0xFFFF8080", "%zs,%ys",
"vsat0.t", "0xD0048000", "0xFFFF8080", "%zt,%yt",
"vsat1.p", "0xD0050080", "0xFFFF8080", "%zp,%yp",
"vsat1.q", "0xD0058080", "0xFFFF8080", "%zq,%yq",
"vsat1.s", "0xD0050000", "0xFFFF8080", "%zs,%ys",
"vsat1.t", "0xD0058000", "0xFFFF8080", "%zt,%yt",
"vsbn.s", "0x61000000", "0xFF808080", "%zs,%ys,%xs",
"vsbz.s", "0xD0360000", "0xFFFF8080", "%zs,%ys",
"vscl.p", "0x65000080", "0xFF808080", "%zp,%yp,%xs",
"vscl.s", "0x65000000", "0xFF808080", "%zs,%ys,%xs",
"vscl.q", "0x65008080", "0xFF808080", "%zq,%yq,%xs",
"vscl.t", "0x65008000", "0xFF808080", "%zt,%yt,%xs",
"vscmp.p", "0x6E800080", "0xFF808080", "%zp,%yp,%xp",
"vscmp.q", "0x6E808080", "0xFF808080", "%zq,%yq,%xq",
"vscmp.s", "0x6E800000", "0xFF808080", "%zs,%ys,%xs",
"vscmp.t", "0x6E808000", "0xFF808080", "%zt,%yt,%xt",
"vsge.p", "0x6F000080", "0xFF808080", "%zp,%yp,%xp",
"vsge.q", "0x6F008080", "0xFF808080", "%zq,%yq,%xq",
"vsge.s", "0x6F000000", "0xFF808080", "%zs,%ys,%xs",
"vsge.t", "0x6F008000", "0xFF808080", "%zt,%yt,%xt",
"vsgn.p", "0xD04A0080", "0xFFFF8080", "%zp,%yp",
"vsgn.q", "0xD04A8080", "0xFFFF8080", "%zq,%yq",
"vsgn.s", "0xD04A0000", "0xFFFF8080", "%zs,%ys",
"vsgn.t", "0xD04A8000", "0xFFFF8080", "%zt,%yt",
"vsin.p", "0xD0120080", "0xFFFF8080", "%zp,%yp",
"vsin.q", "0xD0128080", "0xFFFF8080", "%zq,%yq",
"vsin.s", "0xD0120000", "0xFFFF8080", "%zs,%ys",
"vsin.t", "0xD0128000", "0xFFFF8080", "%zt,%yt",
"vslt.p", "0x6F800080", "0xFF808080", "%zp,%yp,%xp",
"vslt.q", "0x6F808080", "0xFF808080", "%zq,%yq,%xq",
"vslt.s", "0x6F800000", "0xFF808080", "%zs,%ys,%xs",
"vslt.t", "0x6F808000", "0xFF808080", "%zt,%yt,%xt",
"vsocp.p", "0xD0450080", "0xFFFF8080", "%zq,%yp",
"vsocp.s", "0xD0450000", "0xFFFF8080", "%zp,%ys",
"vsqrt.p", "0xD0160080", "0xFFFF8080", "%zp,%yp",
"vsqrt.q", "0xD0168080", "0xFFFF8080", "%zq,%yq",
"vsqrt.s", "0xD0160000", "0xFFFF8080", "%zs,%ys",
"vsqrt.t", "0xD0168000", "0xFFFF8080", "%zt,%yt",
"vsrt1.q", "0xD0408080", "0xFFFF8080", "%zq,%yq",
"vsrt2.q", "0xD0418080", "0xFFFF8080", "%zq,%yq",
"vsrt3.q", "0xD0488080", "0xFFFF8080", "%zq,%yq",
"vsrt4.q", "0xD0498080", "0xFFFF8080", "%zq,%yq",
"vsub.p", "0x60800080", "0xFF808080", "%zp,%yp,%xp",
"vsub.q", "0x60808080", "0xFF808080", "%zq,%yq,%xq",
"vsub.s", "0x60800000", "0xFF808080", "%zs,%ys,%xs",
"vsub.t", "0x60808000", "0xFF808080", "%zt,%yt,%xt",
"vsync", "0xFFFF0000", "0xFFFF0000", "%I",
"vsync", "0xFFFF0320", "0xFFFFFFFF", "",
"vt4444.q", "0xD0598080", "0xFFFF8080", "%zq,%yq",
"vt5551.q", "0xD05A8080", "0xFFFF8080", "%zq,%yq",
"vt5650.q", "0xD05B8080", "0xFFFF8080", "%zq,%yq",
"vtfm2.p", "0xF0800080", "0xFF808080", "%zp,%ym,%xp",
"vtfm3.t", "0xF1008000", "0xFF808080", "%zt,%yn,%xt",
"vtfm4.q", "0xF1808080", "0xFF808080", "%zq,%yo,%xq",
"vus2i.p", "0xD03A0080", "0xFFFF8080", "%zq,%yp",
"vus2i.s", "0xD03A0000", "0xFFFF8080", "%zp,%ys",
"vwb.q", "0xF8000002", "0xFC000002", "%Xq,%Y",
"vwbn.s", "0xD3000000", "0xFF008080", "%zs,%ys,%N",
"vzero.p", "0xD0060080", "0xFFFFFF80", "%zp",
"vzero.q", "0xD0068080", "0xFFFFFF80", "%zq",
"vzero.s", "0xD0060000", "0xFFFFFF80", "%zs",
"vzero.t", "0xD0068000", "0xFFFFFF80", "%zt",
"mfvme", "0x68000000", "0xFC000000", "%t, %i",
"mtvme", "0xb0000000", "0xFC000000", "%t, %i",
"vncos.s", "0x68000000", "0xFC000000", "%zs,%i",
"vncos.p", "0xD01b0080", "0xFFFF8080", "%zp,%yp",
"vncos.q", "0xD01b8080", "0xFFFF8080", "%zq,%yq",
"vncos.s", "0xD01b0000", "0xFFFF8080", "%zs,%ys",
"vncos.t", "0xD01b8000", "0xFFFF8080", "%zt,%yt",
"vnasin.p", "0xD01f0080", "0xFFFF8080", "%zp,%yp",
"vnasin.q", "0xD01f8080", "0xFFFF8080", "%zq,%yq",
"vnasin.s", "0xD01f0000", "0xFFFF8080", "%zs,%ys",
"vnasin.t", "0xD01f8000", "0xFFFF8080", "%zt,%yt",
"vncos.s", "0x68000000", "0xFC000000", "%zs,%i",
"vnlog2.p", "0xD01d0080", "0xFFFF8080", "%zp,%yp",
"vnlog2.q", "0xD01d8080", "0xFFFF8080", "%zq,%yq",
"vnlog2.s", "0xD01d0000", "0xFFFF8080", "%zs,%ys",
"vnlog2.t", "0xD01d8000", "0xFFFF8080", "%zt,%yt",
"vnsqrt.p", "0xD01e0080", "0xFFFF8080", "%zp,%yp",
"vnsqrt.q", "0xD01e8080", "0xFFFF8080", "%zq,%yq",
"vnsqrt.s", "0xD01e0000", "0xFFFF8080", "%zs,%ys",
"vnsqrt.t", "0xD01e8000", "0xFFFF8080", "%zt,%yt",
"vnrsq.p", "0xD0190080", "0xFFFF8080", "%zp,%yp",
"vnrsq.q", "0xD0198080", "0xFFFF8080", "%zq,%yq",
"vnrsq.s", "0xD0190000", "0xFFFF8080", "%zs,%ys",
"vnrsq.t", "0xD0198000", "0xFFFF8080", "%zt,%yt"
                              }
#End Region

    'decoder PRXTOOLの移植
#Region "decoder"
    Function decoders(ByVal hex As UInteger, ByVal l As Integer) As String
        Try
            Dim mask As UInteger = 0
            Dim mips As UInteger = 0
            Dim asm As String = ""


            Dim z As Integer = 0

            While z < decoder.Length
                mips = Convert.ToUInt32(decoder(z + 1), 16)
                mask = Convert.ToUInt32(decoder(z + 2), 16)
                If (hex And mask) = mips Then
                    asm = decoder(z) & vbTab & decoder(z + 3)
                    asm = decode_arg(asm, hex, l)
                    Exit While
                End If
                z += 4
            End While

            If asm = "" Then
                asm = ".word" & vbTab & "0x" & hex.ToString("X8")
            End If

            Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return "decode error"
        End Try
    End Function

    Function reg_dec(ByVal z As Integer) As String
        Dim ss As String() = {"zr", "at", "v0", "v1", "a0", "a1", "a2", "a3", "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "t8", "t9", "k0", "k1", "gp", "sp", "fp", "ra"}
        Return ss(z)
    End Function

    Function reg_dec_debug(ByVal z As Integer) As String
        Dim dr As String() = {"DRCNTL", "DEPC", "DDATA0", "DDATA1", "IBC", "DBC", "$06", "$07", _
    "IBA", "IBAM", "$10", "$11", "DBA", "DBAM", "DBD", "DBDM", ""}
        If z > 15 Then
            z = 16
            dr(16) = "$" & z.ToString
        End If
        Return dr(z)
    End Function

    Function reg_cop0(ByVal z As Integer) As String
        Dim cop0 As String() = {"INDEX", "RANDOM", "ENTRYLO0", "ENTRYLO1", "CONTEXT", "PAGEMASK", "WIRED", "$7", "BADVADDR", "COUNT", "ENTRYHI", "COMPARE", "STATUS", "CAUSE", "EPC", "PRID", "CONFIG", "LLADDR", "WATCHLO", "WATCHHI", "XCONTEXT", "$21", "$22", "DEBUG", "DEPC", "PERFCNT", "ERRCTL", "CACHEERR", "TAGLO", "TAGHI", "ERROREPC", "DESAVE"}
        Return cop0(z)
    End Function

    Function decode_arg(ByVal str As String, ByVal hex As UInteger, ByVal l As Integer) As String
        Dim ss(str.Length) As String
        Dim z As Integer = 0
        Dim vmmul As Boolean = False
        For Each s As String In str
            ss(z) = s
            z += 1
        Next

        For i = 0 To ss.Length - 1
            If (ss(i) = "%") Then
                i += 1
                Select Case ss(i)
                    Case "0"
                        str = str.Replace("%0", reg_cop0((CInt(hex >> 11) And &H1F)))
                    Case "1"
                        str = str.Replace("%1", "$f" & (CInt(hex >> 11) And &H1F).ToString)

                    Case "a"
                        str = str.Replace("%a", ((CInt(hex >> 6) And &H1F)).ToString)
                    Case "C"
                        str = str.Replace("%C", "$" & (CInt(hex >> 6) And &HFFFFF).ToString("X"))
                    Case "c"
                        str = str.Replace("%c", "$" & (CInt(hex >> 6) And &HFFFFF).ToString("X"))
                    Case "n"
                        Select Case ss(i + 1)
                            Case "e"
                                str = str.Replace("%ne", ((CInt(hex >> 11) And &H1F) + 1).ToString)
                            Case "i"
                                str = str.Replace("%ni", ((CInt(hex >> 11) And &H1F) - (CInt(hex >> 6) And &H1F) + 1).ToString)
                        End Select
                    Case "i"
                        Dim k As Integer = CInt(hex And &HFFFF)
                        Dim minus As String = ""
                        If k > &H7FFF Then
                            k = &H10000 - k
                            minus = "-"
                        End If
                        minus &= "0x" & k.ToString("X")
                        str = str.Replace("%i", minus)
                    Case "N"
                        str = str.Replace("%N", "0x" & (CInt(((hex >> 16) And &HFF)).ToString("X")))
                    Case "I"
                        str = str.Replace("%I", "0x" & (CInt((hex And &HFFFF)).ToString("X")))
                    Case "j"
                        str = str.Replace("%j", "0x" & (CInt((hex And &H3FFFFFF) << 2).ToString("X")))
                    Case "J"
                        str = str.Replace("%J", reg_dec((CInt(hex >> 21) And &H1F)))
                    Case "k"
                        str = str.Replace("%k", "$" & (CInt(hex >> 16) And &H1F).ToString)
                    Case "o"
                        Dim k As Integer = CInt(hex And &HFFFF)
                        Dim minus As String = ""
                        If k > &H7FFF Then
                            k = &H10000 - k
                            minus = "-"
                        End If
                        minus &= "0x" & k.ToString("X")
                        str = str.Replace("%o", minus & "(" & reg_dec((CInt(hex >> 21) And &H1F)) & ")")
                    Case "O"
                        Dim k As Integer = CInt(hex And &HFFFF)
                        Dim minus As String = ""
                        If k > &H7FFF Then
                            k -= &H10000
                        End If
                        k = (k << 2) + 4
                        k += l And &HFFFFFFF
                        If k < &H1800000 Then
                            k += &H8800000
                        End If
                        str = str.Replace("%O", "0x" & k.ToString("X"))
                    Case "p"
                        str = str.Replace("%p", (CInt(hex >> 11) And &H1F).ToString)
                    Case "r"
                        str = str.Replace("%r", reg_dec_debug(CInt(hex >> 11) And &H1F))
                    Case "s"
                        str = str.Replace("%s", reg_dec((CInt(hex >> 21) And &H1F)))
                    Case "t"
                        str = str.Replace("%t", reg_dec((CInt(hex >> 16) And &H1F)))
                    Case "d"
                        str = str.Replace("%d", reg_dec((CInt(hex >> 11) And &H1F)))
                    Case "S"
                        str = str.Replace("%S", "$f" & (CInt(hex >> 11) And &H1F).ToString)
                    Case "T"
                        str = str.Replace("%T", "$f" & (CInt(hex >> 16) And &H1F).ToString)
                    Case "D"
                        str = str.Replace("%D", "$f" & (CInt(hex >> 6) And &H1F).ToString)
                    Case "Z"
                        Select Case ss(i + 1)
                            Case "c"
                                str = str.Replace("%Zc", (CInt(hex >> 18) And &H7).ToString)
                            Case "n"
                                str = str.Replace("%Zn", vfpucond(CInt(hex >> 16) And &HF))
                        End Select
                    Case "x"
                        str = str.Replace("%x" & ss(i + 1), vfpureg(CInt(hex >> 16) And &H7F, ss(i + 1)))
                    Case "y"
                        Dim reg As Integer = CInt(hex >> 8) And &H7F
                        If vmmul = True Then
                            If (reg And &H20) <> 0 Then
                                reg = reg And &H5F
                            Else
                                reg = reg Or &H20
                            End If
                        End If
                        str = str.Replace("%y" & ss(i + 1), vfpureg(reg, ss(i + 1)))

                    Case "z"
                        str = str.Replace("%z" & ss(i + 1), vfpureg(CInt(hex And &H7F), ss(i + 1)))

                    Case "v"
                        '// [hlide] completed %v? (? is 3, 5, 8, k, i, h, r, p? (? is (0, 1, 2, 3, 4, 5, 6, 7) ) )
                        Select Case ss(i + 1)
                            Case "3"
                                str = str.Replace("%v" & ss(i + 1), (CInt((hex >> 16) And &H7).ToString))
                                'output = print_int(VI3(opcode), output); i++; 
                            Case "5"
                                str = str.Replace("%v" & ss(i + 1), (CInt((hex >> 16) And &H1F).ToString))
                                'output = print_int(VI5(opcode), output); i++; 
                            Case "8"
                                str = str.Replace("%v" & ss(i + 1), (CInt((hex >> 16) And &HFF).ToString))
                                'output = print_int(VI8(opcode), output); i++; 
                            Case "k"
                                str = str.Replace("%v" & ss(i + 1), print_vfpu_const(CInt((hex >> 16) And &H1F)))
                                'output = print_vfpu_const(VI5(opcode), output); i++; 
                            Case "i"
                                Dim k As Integer = CInt(hex And &HFFFF)
                                Dim minus As String = ""
                                If k > &H7FFF Then
                                    k = &H10000 - k
                                    minus = "-"
                                End If
                                minus &= "0x" & k.ToString("X")
                                str = str.Replace("%v" & ss(i + 1), minus)
                                'output = print_int(IMM(opcode), output); i++; 
                            Case "h"
                                Dim sss As String = (hex And &HFFFF).ToString("X4")
                                Dim bytes As Byte() = str2bin("0000" & sss)

                                If (bytes(1) And &H7F) < &H7C Then
                                    sss = converthalffloat2(sss)
                                    Dim bytes2 As Byte() = BitConverter.GetBytes(Convert.ToInt32(sss, 16))
                                    sss = Convert.ToDecimal(BitConverter.ToSingle(bytes2, 0)).ToString

                                ElseIf (bytes(1) And &H7F) < &H7F Then
                                    If (bytes(1) And &H80) = 0 Then
                                        sss = "+"
                                    Else
                                        sss = "-"
                                    End If
                                    sss &= "Inf"
                                Else
                                    If (bytes(1) And &H80) = 0 Then
                                        sss = "+"
                                    Else
                                        sss = "-"
                                    End If
                                    sss &= "NaN"
                                End If
                                str = str.Replace("%v" & ss(i + 1), sss)
                                'output = print_vfpu_halffloat(opcode, output); i++; 
                            Case "r"
                                str = str.Replace("%v" & ss(i + 1), print_vfpu_rotator(hex))
                                'output = print_vfpu_rotator(opcode, output); i++; 

                            Case "p"
                                str = str.Replace("%v" & ss(i + 1) & ss(i + 2), print_vfpu_prefix(hex, ss(i + 2)))
                                'if (fmt[i+2]) { output = print_vfpu_prefix(opcode, fmt[i+2], output); i += 2; }
                        End Select
                    Case "2"
                        ': // [hlide] added %2? (? is d, s)
                        Select Case ss(i + 1)
                            Case "d"
                                str = str.Replace("%2" & ss(i + 1), print_cop2(CInt(hex And &HFF)))
                                ' : output = print_cop2(VED(opcode), output); i++; break;
                            Case "s"
                                str = str.Replace("%2" & ss(i + 1), print_cop2(CInt((hex >> 8) And &HFF)))
                                ': output = print_cop2(VES(opcode), output); i++; break;
                        End Select

                    Case "X"
                        str = str.Replace("%X" & ss(i + 1), vfpureg((CInt(hex And 3) << 5) Or (CInt(hex >> 16) And &H1F), ss(i + 1)))

                    Case "Y"
                        str = str.Replace("%Y", "0x" & CInt(hex And &HFFFC).ToString("X") & "(" & reg_dec((CInt(hex >> 21) And &H1F)) & ")")
                        'output = print_ofs(IMM(opcode) & ~3, RS(opcode), output, realregs);

                    Case "?"
                        vmmul = True
                        str = str.Replace("%?", "")

                End Select
            End If
        Next
        Return str
    End Function

    Function print_cop2(ByVal reg As Integer) As String
        Dim vfpu_extra_regs As String() = {"VFPU_PFXS",
         "VFPU_PFXT",
         "VFPU_PFXD",
         "VFPU_CC ",
         "VFPU_INF4",
         "",
         "",
         "VFPU_REV",
         "VFPU_RCX0",
         "VFPU_RCX1",
         "VFPU_RCX2",
         "VFPU_RCX3",
         "VFPU_RCX4",
         "VFPU_RCX5",
         "VFPU_RCX6",
         "VFPU_RCX7"
        }
        Dim ss As String = ""

        If ((reg >= 128) AndAlso (reg < 128 + 16) AndAlso (vfpu_extra_regs(reg - 128)) <> "") Then
            'len = sprintf(output, "%s", vfpu_extra_regs(reg - 128));
            ss = vfpu_extra_regs(reg - 128)
        Else

            ss = "$" & reg.ToString
            'Len = sprintf(output, "$%d", reg)
        End If

        Return ss
    End Function

    Function print_vfpu_prefix(ByVal l As UInteger, ByVal pos As String) As String
        '/* VFPU prefix instruction operands.  The *_SH_* values really specify where
        '   the bitfield begins, as VFPU prefix instructions have four operands
        '   encoded within the immediate field. */
        Dim VFPU_SH_PFX_NEG As UInteger = 16
        Dim VFPU_MASK_PFX_NEG As UInteger = 1   '/* Negation. */
        Dim VFPU_SH_PFX_CST As UInteger = 12
        Dim VFPU_MASK_PFX_CST As UInteger = 1   '/* Constant. */
        Dim VFPU_SH_PFX_ABS_CSTHI As UInteger = 8
        Dim VFPU_MASK_PFX_ABS_CSTHI As UInteger = 1 '/* Abs/Constant (bit 2). */
        Dim VFPU_SH_PFX_SWZ_CSTLO As UInteger = 0
        Dim VFPU_MASK_PFX_SWZ_CSTLO As UInteger = 3 '/* Swizzle/Constant (bits 0-1). */
        Dim VFPU_SH_PFX_MASK As UInteger = 8
        Dim VFPU_MASK_PFX_MASK As UInteger = 1  '/* Mask. */
        Dim VFPU_SH_PFX_SAT As UInteger = 0
        Dim VFPU_MASK_PFX_SAT As UInteger = 3   '/* Saturation. */
        Dim ss As String = ""
        Dim poss As UInteger = CUInt(&H30 + Convert.ToUInt32(pos))
        Dim pfx_cst_names As String() = {"0", "1", "2", "1/2", "3", "1/3", "1/4", "1/6"}
        Dim pfx_swz_names As String() = {"x", "y", "z", "w"}
        Dim pfx_sat_names As String() = {"", "[0:1]", "", "[-1:1]"}

        Select Case pos
            Case "0", "1", "2", "3"

                Dim base As UInteger = CUInt(poss - 48)
                Dim negation As UInteger = (l >> CInt(base + VFPU_SH_PFX_NEG)) And VFPU_MASK_PFX_NEG
                Dim constant As UInteger = (l >> CInt(base + VFPU_SH_PFX_CST)) And VFPU_MASK_PFX_CST
                Dim abs_consthi As UInteger = (l >> CInt(base + VFPU_SH_PFX_ABS_CSTHI)) And VFPU_MASK_PFX_ABS_CSTHI
                Dim swz_constlo As UInteger = (l >> CInt(base << 1)) And VFPU_MASK_PFX_SWZ_CSTLO

                If (negation <> 0) Then
                    ss &= "-"
                    'Len = sprintf(output, "-")
                End If

                If (constant <> 0) Then
                    ss &= pfx_cst_names((CInt(abs_consthi << 2) Or CInt(swz_constlo)))
                    'len += sprintf(output+len, "%s", pfx_cst_names[(abs_consthi << 2) | swz_constlo]);

                Else
                    If (abs_consthi <> 0) Then
                        ss &= "|" & pfx_swz_names(CInt(swz_constlo)) & "|"
                        'len += sprintf(output+len, "|%s|", pfx_swz_names[swz_constlo]);

                    Else
                        ss &= pfx_swz_names(CInt(swz_constlo))
                        'len += sprintf(output+len, "%s", pfx_swz_names[swz_constlo]);

                    End If
                End If

            Case "4", "5", "6", "7"
                Dim base As UInteger = CUInt(poss - &H34)
                Dim mask As UInteger = (l >> CInt(base + VFPU_SH_PFX_MASK)) And VFPU_MASK_PFX_MASK
                Dim saturation As UInteger = (l >> CInt(base << 1)) And VFPU_MASK_PFX_SAT

                If (mask <> 0) Then
                    ss &= "m"
                    'len += sprintf(output, "m");
                Else
                    ss &= pfx_sat_names(CInt(saturation))
                    'len += sprintf(output, "%s", pfx_sat_names[saturation]);
                End If

        End Select

        Return ss

    End Function

    Function print_vfpu_rotator(ByVal l As UInteger) As String

        Dim elements(4) As String
        Dim ss As String
        Dim VFPU_MASK_OP_SIZE As UInteger = &H8080
        Dim VFPU_OP_SIZE_PAIR As UInteger = &H80
        Dim VFPU_OP_SIZE_TRIPLE As UInteger = &H8000
        Dim VFPU_OP_SIZE_QUAD As UInteger = &H8080
        Dim VFPU_SH_ROT_HI As UInteger = 2
        Dim VFPU_MASK_ROT_HI As UInteger = 3
        Dim VFPU_SH_ROT_LO As UInteger = 0
        Dim VFPU_MASK_ROT_LO As UInteger = 3
        Dim VFPU_SH_ROT_NEG As UInteger = 4
        Dim VFPU_MASK_ROT_NEG As UInteger = 1

        Dim opcode As UInteger = l And VFPU_MASK_OP_SIZE
        Dim rotators As UInteger = CUInt((l >> 16) And &H1F)
        Dim opsize, rothi, rotlo, negation, i As UInteger

        '/* Determine the operand size so we'll know how many elements to output. */
        If (opcode = VFPU_OP_SIZE_PAIR) Then
            opsize = 2
        ElseIf (opcode = VFPU_OP_SIZE_TRIPLE) Then
            opsize = 3
        ElseIf (opcode = VFPU_OP_SIZE_QUAD) Then
            opsize = 4
        Else
            opsize = 1
            'opsize = (opcode = VFPU_OP_SIZE_QUAD) * 4 
            ';/* Sanity check. */
        End If

        rothi = CUInt((rotators >> 2) And 3)
        rotlo = CUInt((rotators >> 0) And 3)
        negation = CUInt((rotators >> 4) And 1)

        If (rothi = rotlo) Then
            elements(0) = "s"
            elements(1) = "s"
            elements(2) = "s"
            elements(3) = "s"
        Else
        elements(0) = "0"
        elements(1) = "0"
        elements(2) = "0"
        elements(3) = "0"
        End If

        elements(CInt(rothi)) = "s"
        elements(CInt(rotlo)) = "c"

        ss = "["
        'len = sprintf(output, "[");

        'for (i = 0;;)
        opsize = CUInt(opsize - 1)
        For i = 0 To opsize
            If (negation <> 0 AndAlso elements(CInt(i)) = "s") Then
                ss &= "-"
            End If

            ss &= elements(CInt(i))
            'len += sprintf(output, "%s", elements[i++]);
            If (i >= opsize) Then
                Exit For
            End If
            ss &= " ,"
        Next

        ss &= "]"
        'len += sprintf(output, "]");

        Return ss
    End Function

    Function print_vfpu_const(ByVal k As Integer) As String
        Dim ss As String = ""
        Dim vfpu_const_names As String() = {
            "", "VFPU_HUGE", "VFPU_SQRT2", "VFPU_SQRT1_2", "VFPU_2_SQRTPI", "VFPU_2_PI", "VFPU_1_PI", "VFPU_PI_4", "VFPU_PI_2", "VFPU_PI",
            "VFPU_E", "VFPU_LOG2E", "VFPU_LOG10E", "VFPU_LN2", "VFPU_LN10", "VFPU_2PI", "VFPU_PI_6", "VFPU_LOG10TWO", "VFPU_LOG2TEN", "VFPU_SQRT3_2"}
        If ((k > 0) AndAlso (k < 20)) Then
            ss = vfpu_const_names(k)
        Else
            ss = k.ToString
        End If

        Return ss
    End Function

    Function print_vfpu_reg(ByVal reg As Integer, ByVal offset As Integer, ByVal one As String, ByVal two As String) As String
        Dim ss As String
        If (CInt(reg >> 5) And 1) <> 0 Then
            ss = two & CInt((reg >> 2) And 7).ToString & offset.ToString & (reg And 3).ToString
        Else
            ss = one & CInt((reg >> 2) And 7).ToString & (reg And 3).ToString & offset.ToString
        End If

        Return ss
    End Function

    Function vfpureg(ByVal reg As Integer, ByVal s As String) As String
        Dim ss As String = ""
        Select Case s
            Case "s"
                ss = "S" & (CInt(reg >> 2) And 7).ToString & (CInt(reg) And 3).ToString & (CInt(reg >> 5) And 3).ToString
                ' return print_vfpusingle(reg, output);
            Case "q"
                ' return print_vfpuquad(reg, output);
                ss = print_vfpu_reg(reg, 0, "C", "R")
            Case "p"
                ' return print_vfpupair(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 2, "C", "R")
                Else
                    ss = print_vfpu_reg(reg, 0, "C", "R")
                End If

            Case "t"
                ' return print_vfputriple(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 1, "C", "R")
                Else
                    ss = print_vfpu_reg(reg, 0, "C", "R")
                End If

            Case "m"
                ' return print_vfpumpair(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 2, "M", "E")
                Else
                    ss = print_vfpu_reg(reg, 0, "M", "E")
                End If

            Case "n"
                ' return print_vfpumtriple(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 1, "M", "E")
                Else
                    ss = print_vfpu_reg(reg, 0, "M", "E")
                End If

            Case "o"
                ' return print_vfpumatrix(reg, output);
                ss = print_vfpu_reg(reg, 0, "M", "E")

        End Select
        Return ss
    End Function

    Function vfpucond(ByVal k As Integer) As String
        Dim vfpucmp As String() = {"FL", "EQ", "LT", "LE", "TR", "NE", "GE", "GT", "EZ", "EN", "EI", "ES", "NZ", "NN", "NI", "NS", ""}
        If k > 15 Then
            k = 16
            vfpucmp(16) = k.ToString
        End If
        Return vfpucmp(k)
    End Function

#End Region

    'ASM INSERT
#Region "INSERT ASM"
    Function assembler(ByVal str As String, ByVal str2 As String) As String
        Try
            Dim hex As Integer = 0
            Dim hex2 As Integer = Convert.ToInt32(str2, 16) And &H9FFFFFFF
            Dim asm As String = ""
            Dim mips As String = ""

            Dim psdis As New Regex("(\t|\x20|　)?(#|;).+$")
            Dim psdism As Match = psdis.Match(str)
            If psdism.Success Then
                str = str.Substring(0, psdism.Index)
            End If
            Dim llb As New Regex("^.*?:+( |\t|　)+")
            Dim llbm As Match = llb.Match(str)
            If llbm.Success Then
                str = str.Remove(0, llbm.Length)
            End If
            str &= " "

            Dim valhex As New Regex("(\$|0x)[0-9A-Fa-f]{1,8}")
            Dim valhexm As Match = valhex.Match(str)
            If valhexm.Success Then
                str = str.Replace(valhexm.Value, valhexm.Value.ToUpper)
                str = str.Replace("0X", "0x")
            End If
            Dim ss As String() = str.ToLower.Split(CChar(","))
            Dim shead As New Regex("^[a-z0-9\.]+(\x20|\t)+")
            Dim sheadm As Match = shead.Match(str)

            If sheadm.Success Then
                mips = sheadm.Value.Replace(" ", "")
                mips = mips.Replace(vbTab, "")
                str = str.Trim
                ss(0) = ss(0).Replace(sheadm.Value, "")

                select case mips
                    Case "nop"
                    Case "syscall"
                        hex = 12
                        hex = hex Or valhex_syscall(str, hex)
                    Case "break"
                        hex = &HD '13
                        hex = hex Or valhex_syscall(str, hex)
                    Case "sync"
                        hex = 15
                    Case "sll"
                        hex = reg_boolean3(str, hex, 0)
                        hex = valdec_boolean_para(str, hex, 3)
                    Case "rotr"
                        hex = &H200002
                        hex = reg_boolean3(str, hex, 0)
                        hex = valdec_boolean_para(str, hex, 3)
                    Case "rotv"
                        hex = &H46
                        hex = reg_boolean3(str, hex, 0)
                    Case "srl"
                        hex = &H2
                        hex = reg_boolean3(str, hex, 0)
                        hex = valdec_boolean_para(str, hex, 3)
                    Case "sra"
                        hex = &H3
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = valdec_boolean_para(str, hex, 3)
                    Case "sllv"
                        hex = &H4
                        hex = reg_boolean3(str, hex, 0)
                    Case "srlv"
                        hex = &H6
                        hex = reg_boolean3(str, hex, 0)
                    Case "srav"
                        hex = &H7
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = reg_boolean_para(ss(2), hex, 0)
                    Case "jalr"
                        hex = &H9
                        If ss.Length = 1 Then
                            Array.Resize(ss, 2)
                            ss(1) = ss(0)
                            ss(0) = "ra"
                        End If
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                    Case "movz"
                        hex = &HA
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "movn"
                        hex = &HB
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "mfhi"
                        hex = &H10
                        hex = reg_boolean_para(ss(0), hex, 2)
                    Case "mthi"
                        hex = &H11
                        hex = reg_boolean_para(ss(0), hex, 0)
                    Case "mflo"
                        hex = &H12
                        hex = reg_boolean_para(ss(0), hex, 2)
                    Case "mtlo"
                        hex = &H13
                        hex = reg_boolean_para(ss(0), hex, 0)
                    Case "clz"
                        hex = &H16
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                    Case "clo"
                        hex = &H17
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                    Case "add"
                        hex = &H20
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "addu"
                        hex = &H21
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "move", "mov"
                        hex = &H21
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                    Case "sub"
                        hex = &H22
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "neg"
                        hex = &H22
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "subu"
                        hex = &H23
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "negu"
                        hex = &H23
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "and"
                        hex = &H24
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "or"
                        hex = &H25
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "xor"
                        hex = &H26
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "nor"
                        hex = &H27
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "not"
                        hex = &H27
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                    Case "slt"
                        hex = &H2A
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "sltu"
                        hex = &H2B
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "max"
                        hex = &H2C
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "min"
                        hex = &H2D
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = reg_boolean_para(ss(2), hex, 1)
                    Case "j"
                        hex = &H8000000
                        hex = offset_boolean(str, hex)
                    Case "jal"
                        hex = &HC000000
                        hex = offset_boolean(str, hex)
                    Case "jr"
                        hex = &H8
                        hex = reg_boolean_para(ss(0), hex, 0)
                    Case "mult"
                        hex = &H18
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "multu"
                        hex = &H19
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "div"
                        hex = &H1A
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "divu"
                        hex = &H1B
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "madd"
                        hex = &H1C
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "maddu"
                        hex = &H1D
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "msub"
                        hex = &H2E
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "msubu"
                        hex = &H2F
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "bltz"
                        hex = &H4000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bgez"
                        hex = &H4010000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bltzl"
                        hex = &H4020000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bgezl"
                        hex = &H4030000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bltzal"
                        hex = &H4100000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bgezal"
                        hex = &H4110000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bltzall"
                        hex = &H4120000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bgezall"
                        hex = &H4130000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "mtsab"
                        hex = &H4180000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = valhex_boolean(ss(1), hex)
                    Case "mtsah"
                        hex = &H4190000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = valhex_boolean(ss(1), hex)

                        '0x10 branch
                    Case "b"
                        hex = &H10000000
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bal"
                        hex = &H4110000
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bnez"
                        hex = &H14000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bnezl"
                        hex = &H54000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "beqz"
                        hex = &H10000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "beqzl"
                        hex = &H50000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "beq"
                        hex = &H10000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bne"
                        hex = &H14000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "blez"
                        hex = &H18000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bgtz"
                        hex = &H1C000000
                        hex = reg_boolean_para(str, hex, 0)
                        hex = offset_boolean2(str, hex, hex2)

                        '0x20 add/boolean
                    Case "addi"
                        hex = &H20000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "addiu"
                        hex = &H24000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "li"
                        hex = &H24000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = valhex_boolean(str, hex)
                    Case "slti"
                        hex = &H28000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "sltiu"
                        hex = &H2C000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "andi"
                        hex = &H30000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "ori"
                        hex = &H34000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "xori"
                        hex = &H38000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = valhex_boolean(str, hex)
                    Case "lui"
                        hex = &H3C000000
                        hex = reg_boolean_para(str, hex, 1)
                        hex = valhex_boolean(str, hex)

                        '0x40 FPU
                    Case "mfc0"
                        hex = &H40000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                    Case "mtc0"
                        hex = &H40800000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                    Case "cfc0"
                        hex = &H40400000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = hex Or (cop_sel(ss(1), "") << 11)
                    Case "ctc0"
                        hex = &H40C00000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = hex Or (cop_sel(ss(1), "") << 11)
                    Case "eret"
                        hex = &H42000018
                    Case "cfc1"
                        hex = &H44400000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = hex Or (cop_sel(ss(1), "") << 11)
                    Case "ctc1"
                        hex = &H44C00000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = hex Or (cop_sel(ss(1), "") << 11)
                    Case "mfc1"
                        hex = &H44000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = float_sel(ss(1), hex, 2)
                    Case "mtc1"
                        hex = &H44800000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = float_sel(ss(1), hex, 2)
                    Case "bc1f"
                        hex = &H45000000
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bc1t"
                        hex = &H45010000
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bc1tl"
                        hex = &H45020000
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bc1tl"
                        hex = &H45030000
                        hex = offset_boolean2(str, hex, hex2)
                    Case "add.s"
                        hex = &H46000000
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                        hex = float_sel(ss(2), hex, 1)
                    Case "sub.s"
                        hex = &H46000001
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                        hex = float_sel(ss(2), hex, 1)
                    Case "mul.s"
                        hex = &H46000002
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                        hex = float_sel(ss(2), hex, 1)
                    Case "div.s"
                        hex = &H46000003
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                        hex = float_sel(ss(2), hex, 1)
                    Case "sqrt.s"
                        hex = &H46000004
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "abs.s"
                        hex = &H46000005
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "mov.s"
                        hex = &H46000006
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "neg.s"
                        hex = &H46000007
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "round.w.s"
                        hex = &H4600000C
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "trunc.w.s"
                        hex = &H4600000D
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "ceil.w.s"
                        hex = &H4600000E
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "floor.w.s"
                        hex = &H4600000F
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "cvt.s.w"
                        hex = &H46800020
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "cvt.w.s"
                        hex = &H46000024
                        hex = float_sel(ss(0), hex, 3)
                        hex = float_sel(ss(1), hex, 2)
                    Case "c.f.s"
                        hex = &H46000030
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.un.s"
                        hex = &H46000031
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.eq.s"
                        hex = &H46000032
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ueq.s"
                        hex = &H46000033
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.olt.s"
                        hex = &H46000034
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ult.s"
                        hex = &H46000035
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ole.s"
                        hex = &H46000036
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ule.s"
                        hex = &H46000037
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.sf.s"
                        hex = &H46000038
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ngle.s"
                        hex = &H46000039
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.seq.s"
                        hex = &H4600003A
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ngl.s"
                        hex = &H4600003B
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.lt.s"
                        hex = &H4600003C
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.nge.s"
                        hex = &H4600003D
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.le.s"
                        hex = &H4600003E
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)
                    Case "c.ngt.s"
                        hex = &H4600003F
                        hex = float_sel(ss(0), hex, 2)
                        hex = float_sel(ss(1), hex, 1)

                        '0x50
                    Case "beql"
                        hex = &H50000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bnel"
                        hex = &H54000000
                        hex = reg_boolean_para(ss(0), hex, 0)
                        hex = reg_boolean_para(ss(1), hex, 1)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "blezl"
                        hex = &H58000000
                        hex = reg_boolean_para(str, hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bgtzl"
                        hex = &H5C000000
                        hex = reg_boolean_para(str, hex, 0)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "mfic"
                        hex = &H70000024
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = valdec_boolean_para(str, hex, 2)
                    Case "mtic"
                        hex = &H70000026
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = valdec_boolean_para(str, hex, 2)
                    Case "mfdr"
                        hex = &H7000003D
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = debug_reg(ss(1), hex)
                    Case "mtdr"
                        hex = &H7080003D
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = debug_reg(ss(1), hex)
                    Case "dbreak"
                        hex = &H7000003F
                    Case "dret"
                        hex = &H7000003E
                    Case "haltl"
                        hex = &H70000000
                    Case "seb"
                        hex = &H7C000420
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "seh"
                        hex = &H7C000620
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "wsbh"
                        hex = &H7C0000A0
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "wsbw"
                        hex = &H7C0000E0
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "bitrev"
                        hex = &H7C000520
                        hex = reg_boolean_para(ss(0), hex, 2)
                        hex = reg_boolean_para(ss(1), hex, 1)
                    Case "halt"
                        hex = &H7C000000
                    Case "ext"
                        hex = &H7C000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = valdec_boolean_para(ss(2), hex, 3)
                        hex = valdec_ext_para(ss(3), hex, 2)
                    Case "ins"
                        hex = &H7C000004
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = reg_boolean_para(ss(1), hex, 0)
                        hex = valdec_boolean_para(ss(2), hex, 3)
                        hex = valdec_ins_para(ss(3), hex, 2)

                        '0x80
                    Case "lb"
                        hex = &H80000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lh"
                        hex = &H84000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lw"
                        hex = &H8C000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lbu"
                        hex = &H90000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lhu"
                        hex = &H94000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lwu"
                        hex = &H9C000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lwl"
                        hex = &H88000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lwr"
                        hex = &H98000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)

                        '0xA0
                    Case "sb"
                        hex = &HA0000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "sh"
                        hex = &HA4000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "sw"
                        hex = &HAC000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "swl"
                        hex = &HA8000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "swr"
                        hex = &HB8000000
                        hex = reg_boolean2(str, hex, 0)
                        hex = offset_boolean3(str, hex)
                        '0xc0
                    Case "ll"
                        hex = &HC0000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "lwc1"
                        hex = &HC4000000
                        hex = float_sel(ss(0), hex, 1)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                        hex = offset_boolean3(str, hex)

                        '0xe0
                    Case "sc"
                        hex = &HE0000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                        hex = offset_boolean3(str, hex)
                    Case "swc1"
                        hex = &HE4000000
                        hex = float_sel(ss(0), hex, 1)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                        hex = offset_boolean3(str, hex)

                    Case "cache"
                        hex = &HBC000000
                        hex = valdec_boolean_para(ss(0), hex, 1)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                        hex = offset_boolean3(str, hex)

                        'vfpu
                    Case "bvf"
                        '"bvf", "0x49000000", "0xFFE30000", "%Zc,%O", _
                        hex = &H49000000
                        hex = Zc(ss(0), hex)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bvfl"
                        '"bvfl", "0x49020000", "0xFFE30000", "%Zc,%O", _
                        hex = &H49020000
                        hex = Zc(ss(0), hex)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bvt"
                        '"bvt", "0x49010000", "0xFFE30000", "%Zc,%O", _
                        hex = &H49010000
                        hex = Zc(ss(0), hex)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "bvtl"
                        '"bvtl", "0x49030000", "0xFFE30000", "%Zc,%O", _
                        hex = &H49030000
                        hex = Zc(ss(0), hex)
                        hex = offset_boolean2(str, hex, hex2)
                    Case "lv.q"
                        '"lv.q", "0xD8000000", "0xFC000002", "%Xq,%Y", _
                        hex = &HD8000000
                        hex = Xq(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "lv.s"
                        '"lv.s", "0xC8000000", "0xFC000000", "%Xs,%Y", _
                        hex = &HC8000000
                        hex = Xs(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "lvl.q"
                        '"lvl.q", "0xD4000000", "0xFC000002", "%Xq,%Y", _
                        hex = &HD4000000
                        hex = Xq(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "lvr.q"
                        '"lvr.q", "0xD4000002", "0xFC000002", "%Xq,%Y", _
                        hex = &HD4000002
                        hex = Xq(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "sv.q"
                        '"sv.q", "0xF8000000", "0xFC000002", "%Xq,%Y", _
                        hex = &HF8000000
                        hex = Xq(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "sv.s"
                        '"sv.s", "0xE8000000", "0xFC000000", "%Xs,%Y", _
                        hex = &HE8000000
                        hex = Xs(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "svl.q"
                        '"svl.q", "0xF4000000", "0xFC000002", "%Xq,%Y", _
                        hex = &HF4000000
                        hex = Xq(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    Case "svr.q"
                        '"svr.q", "0xF4000002", "0xFC000002", "%Xq,%Y", _
                        hex = &HF4000002
                        hex = Xq(ss(0), hex)
                        hex = Y(str, hex)
                        hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)

                    Case "mfv"
                        '"mfv", "0x48600000", "0xFFE0FF80", "%t,%zs", _
                        hex = &H48600000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = xyzs(ss(1), hex, 0)

                    Case "mfvc"
                        '"mfvc", "0x48600000", "0xFFE0FF00", "%t,%2d", _
                        hex = &H48600080
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = cop2_parse(ss(1), hex)

                    Case "mtv"
                        '"mtv", "0x48E00000", "0xFFE0FF80", "%t,%zs", _
                        hex = &H48E00000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = xyzs(ss(1), hex, 0)

                    Case "mtvc"
                        '"mtvc", "0x48E00000", "0xFFE0FF00", "%t,%2d", _
                        hex = &H48E00080
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = cop2_parse(ss(1), hex)

                    Case "vabs.p"
                        '"vabs.p", "0xD0010080", "0xFFFF8080", "%zp,%yp", _
                        hex = &HD0010080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vabs.t"
                        '"vabs.t", "0xD0018000", "0xFFFF8080", "%zt,%yt", _
                        hex = &HD0018000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vabs.s"
                        '"vabs.s", "0xD0010000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0010000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vabs.q"
                        '"vabs.q", "0xD0018080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD0018080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vadd.q"
                        '"vadd.q", "0x60008080", "0xFF808080", "%zq,%yq,%xq", _
                        hex = &H60008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vadd.p"
                        '"vadd.p", "0x60000080", "0xFF808080", "%zp,%yp,%xp", _
                        hex = &H60000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vadd.s"
                        '"vadd.s", "0x60000000", "0xFF808080", "%zs,%ys,%xs", _
                        hex = &H60000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vadd.t"
                        '"vadd.t", "0x60008000", "0xFF808080", "%zt,%yt,%xt", _
                        hex = &H60008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)

                    Case "vasin.p"
                        '"vasin.p", "0xD0170080", "0xFFFF8080", "%zp,%yp", _
                        hex = &HD0170080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vasin.q"
                        '"vasin.q", "0xD0178080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD0178080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vasin.s"
                        '"vasin.s", "0xD0170000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0170000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vasin.t"
                        '"vasin.t", "0xD0178000", "0xFFFF8080", "%zt,%yt", _
                        hex = &HD0178000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vnasin.p"
                        '"vnasin.p", "0xD01f0080", "0xFFFF8080", "%zp,%yp", _
                        hex = &HD01F0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vnasin.q"
                        '"vnasin.q", "0xD01f8080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD01F8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vnasin.s"
                        '"vnasin.s", "0xD01f0000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD01F0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vnasin.t"
                        '"vnasin.t", "0xD01f8000", "0xFFFF8080", "%zt,%yt", _
                        hex = &HD01F8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vavg.p"
                        '"vavg.p", "0xD0470080", "0xFFFF8080", "%zs,%yp", _
                        hex = &HD0470080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vavg.q"
                        '"vavg.q", "0xD0478080", "0xFFFF8080", "%zs,%yq", _
                        hex = &HD0478080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vavg.t"
                        '"vavg.t", "0xD0478000", "0xFFFF8080", "%zs,%yt", _
                        hex = &HD0478000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vavg.s"
                        '"vavg.s", "0xD0470000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0470000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)

                    Case "vbfy1.p"
                        '"vbfy1.p", "0xD0420080", "0xFFFF8080", "%zp,%yp", _
                        hex = &HD0420080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vbfy1.q"
                        '"vbfy1.q", "0xD0428080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD0428080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vbfy2.q"
                        '"vbfy2.q", "0xD0438080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD0438080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)

                    Case "vcmp.p"
                        '"vcmp.p", "0x6C000080", "0xFF8080F0", "%Zn,%yp,%xp", _
                        hex = &H6C000080
                        hex = Zn(ss(0), hex)
                        If ss.Length >= 2 Then
                            hex = xyzp(ss(1), hex, 1)
                        End If
                        If ss.Length >= 3 Then
                            hex = xyzp(ss(2), hex, 2)
                        End If
                    Case "vcmp.q"
                        '"vcmp.q", "0x6C008080", "0xFF8080F0", "%Zn,%yq,%xq", _
                        hex = &H6C008080
                        hex = Zn(ss(0), hex)
                        If ss.Length >= 2 Then
                            hex = xyzq(ss(1), hex, 1)
                        End If
                        If ss.Length >= 3 Then
                            hex = xyzq(ss(2), hex, 2)
                        End If
                    Case "vcmp.s"
                        '"vcmp.s", "0x6C000000", "0xFF8080F0", "%Zn,%ys,%xs", _
                        hex = &H6C000000
                        hex = Zn(ss(0), hex)
                        If ss.Length >= 2 Then
                            hex = xyzs(ss(1), hex, 1)
                        End If
                        If ss.Length >= 3 Then
                            hex = xyzs(ss(2), hex, 2)
                        End If
                    Case "vcmp.t"
                        '"vcmp.t", "0x6C008000", "0xFF8080F0", "%Zn,%yt,%xt", _
                        hex = &H6C008000
                        hex = Zn(ss(0), hex)
                        If ss.Length >= 2 Then
                            hex = xyzt(ss(1), hex, 1)
                        End If
                        If ss.Length >= 3 Then
                            hex = xyzt(ss(2), hex, 2)
                        End If

                    Case "vcos.p"
                        '"vcos.p", "0xD0130080", "0xFFFF8080", "%zp,%yp", _
                        hex = &HD0130080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vcos.q"
                        '"vcos.q", "0xD0138080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD0138080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vcos.s"
                        '"vcos.s", "0xD0130000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0130000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vcos.t"
                        '"vcos.t", "0xD0138000", "0xFFFF8080", "%zt,%yt", _
                        hex = &HD0138000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vcrs.t", "vouterp.t"
                        '"vcrs.t", "0x66808000", "0xFF808080", "%zt,%yt,%xt", _
                        hex = &H66808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vcrsp.t"
                        '"vcrsp.t", "0xF2808000", "0xFF808080", "%zt,%yt,%xt", _
                        hex = &HF2808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vdet.s", "vcdps.s"
                        hex = &H67000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)

                    Case "vdet.p", "vcdps.p"
                        '"vdet.p", "0x67000080", "0xFF808080", "%zs,%yp,%xp", _
                        hex = &H67000080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vdet.t", "vcdps.t"
                        hex = &H67008000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)

                    Case "vdet.q", "vcdps.q"
                        '"vdet.p", "0x67008080", "0xFF808080", "%zs,%yq,%xq", _
                        hex = &H67008080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)

                    Case "vdiv.p"
                        '"vdiv.p", "0x63800080", "0xFF808080", "%zp,%yp,%xp", _
                        hex = &H63800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vdiv.q"
                        '"vdiv.q", "0x63808080", "0xFF808080", "%zq,%yq,%xq", _
                        hex = &H63808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vdiv.s"
                        '"vdiv.s", "0x63800000", "0xFF808080", "%zs,%ys,%xs", _
                        hex = &H63800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vdiv.t"
                        '"vdiv.t", "0x63808000", "0xFF808080", "%zt,%yt,%xt", _
                        hex = &H63808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)


                    Case "vdot.s"
                        '"vdot.s", "0x64800080", "0xFF808080", "%zs,%ys,%xs", _
                        hex = &H64800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vdot.p"
                        '"vdot.p", "0x64800080", "0xFF808080", "%zs,%yp,%xp", _
                        hex = &H64800080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vdot.q"
                        '"vdot.q", "0x64808080", "0xFF808080", "%zs,%yq,%xq", _
                        hex = &H64808080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vdot.t"
                        '"vdot.t", "0x64808000", "0xFF808080", "%zs,%yt,%xt", _
                        hex = &H64808000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)

                    Case "vexp2.p"
                        '"vexp2.p", "0xD0140080", "0xFFFF8080", "%zp,%yp", _
                        hex = &HD0140080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vexp2.q"
                        '"vexp2.q", "0xD0148080", "0xFFFF8080", "%zq,%yq", _
                        hex = &HD0148080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vexp2.s"
                        '"vexp2.s", "0xD0140000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0140000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vexp2.t"
                        '"vexp2.t", "0xD0148000", "0xFFFF8080", "%zt,%yt", _
                        hex = &HD0148000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vf2h.p"
                        '"vf2h.p", "0xD0320080", "0xFFFF8080", "%zs,%yp", _
                        hex = &HD0320080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vf2h.q"
                        '"vf2h.q", "0xD0328080", "0xFFFF8080", "%zp,%yq", _
                        hex = &HD0328080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vf2h.s"
                        '"vf2h.s", "0xD0320000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0320000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vf2h.t"
                        '"vf2h.t", "0xD0328000", "0xFFFF8080", "%zq,%yt", _
                        hex = &HD0328000
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vfad.p", "vsum.p"
                        '"vfad.p", "0xD0460080", "0xFFFF8080", "%zs,%yp", _
                        hex = &HD0460080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vfad.q", "vsum.q"
                        '"vfad.q", "0xD0468080", "0xFFFF8080", "%zs,%yq", _
                        hex = &HD0468080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vfad.t", "vsum.t"
                        '"vfad.t", "0xD0468000", "0xFFFF8080", "%zs,%yt", _
                        hex = &HD0468000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vfad.s", "vsum.s"
                        '"vfad.s", "0xD0468000", "0xFFFF8080", "%zs,%ys", _
                        hex = &HD0460000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)

                    Case "vfim.s"
                        '"vfim.s", "0xDF800000", "0xFF800000", "%xs,%vh", _
                        hex = &HDF800000
                        hex = xyzs(ss(0), hex, 2)
                        If str.Contains("hf") Then
                            hex = valhex_boolean(str, hex)
                        ElseIf str.Contains("+inf") Then
                            hex = hex Or &H7C00
                        ElseIf str.Contains("-inf") Then
                            hex = hex Or &HFC00
                        ElseIf str.Contains("+nan") Then
                            hex = hex Or &H7FFF
                        ElseIf str.Contains("-nan") Then
                            hex = hex Or &HFFFF
                        Else
                            str = str.Remove(0, str.IndexOf(",")).Trim
                            hex = hex Or Convert.ToInt32(converthalffloat(cvt_float(valfloat(str)).ToString("X8")), 16)
                        End If
                    Case "viim.s"
                        '"viim.s", "0xDF000000", "0xFF800000", "%xs,%vi", _
                        hex = &HDF000000
                        hex = xyzs(ss(0), hex, 2)
                        hex = valhex_boolean(str, hex)
                    Case "vflush"
                        '"vflush", "0xFFFF040D", "0xFFFFFFFF", "", _
                        hex = &HFFFF040D

                    Case "vh2f.p"
                        '"vh2f.p","0xD0330080","0xFFFF8080","%zq,%yp",
                        hex = &HD0330080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vh2f.s"
                        '"vh2f.s","0xD0330000","0xFFFF8080","%zp,%ys",
                        hex = &HD0330000
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vh2f.t"
                        '"vh2f.p","0xD0330080","0xFFFF8080","%zq,%yp",
                        hex = &HD0338000
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vh2f.q"
                        '"vh2f.q","0xD0338080","0xFFFF8080","%zq,%yp",
                        hex = &HD0338080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vhdp.p"
                        '"vhdp.p","0x66000080","0xFF808080","%zs,%yp,%xp",
                        hex = &H66000080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vhdp.q"
                        '"vhdp.q","0x66008080","0xFF808080","%zs,%yq,%xq",
                        hex = &H66008080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vhdp.t"
                        '"vhdp.t","0x66008000","0xFF808080","%zs,%yt,%xt",
                        hex = &H66008000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vi2c.q"
                        '"vi2c.q","0xD03D8080","0xFFFF8080","%zs,%yq",
                        hex = &HD03D8080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vi2s.p"
                        '"vi2s.p","0xD03F0080","0xFFFF8080","%zs,%yp",
                        hex = &HD03F0080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vi2s.q"
                        '"vi2s.q","0xD03F8080","0xFFFF8080","%zp,%yq",
                        hex = &HD03F8080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vi2uc.q"
                        '"vi2uc.q","0xD03C8080","0xFFFF8080","%zs,%yq",
                        hex = &HD03C8080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vi2us.p"
                        '"vi2us.p","0xD03E0080","0xFFFF8080","%zs,%yq",
                        hex = &HD03E0080
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vi2us.q"
                        '"vi2us.q","0xD03E8080","0xFFFF8080","%zp,%yq",
                        hex = &HD03E8080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vidt.p"
                        '"vidt.p","0xD0030080","0xFFFFFF80","%zp",
                        hex = &HD0030080
                        hex = xyzp(ss(0), hex, 0)
                    Case "vidt.q"
                        '"vidt.q","0xD0038080","0xFFFFFF80","%zq",
                        hex = &HD0038080
                        hex = xyzq(ss(0), hex, 0)
                    Case "vlgb.s"
                        '"vlgb.s","0xD0370000","0xFFFF8080","%zs,%ys",
                        hex = &HD0370000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vlog2.p"
                        '"vlog2.p","0xD0150080","0xFFFF8080","%zp,%yp",
                        hex = &HD0150080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vlog2.q"
                        '"vlog2.q","0xD0158080","0xFFFF8080","%zq,%yq",
                        hex = &HD0158080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vlog2.s"
                        '"vlog2.s","0xD0150000","0xFFFF8080","%zs,%ys",
                        hex = &HD0150000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vlog2.t"
                        '"vlog2.t","0xD0158000","0xFFFF8080","%zt,%yt",
                        hex = &HD0158000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vnlog2.p"
                        '"vnlog2.p","0xD01D0080","0xFFFF8080","%zp,%yp",
                        hex = &HD01D0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vnlog2.q"
                        '"vnlog2.q","0xD01D8080","0xFFFF8080","%zq,%yq",
                        hex = &HD01D8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vnlog2.s"
                        '"vnlog2.s","0xD01D0000","0xFFFF8080","%zs,%ys",
                        hex = &HD01D0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vnlog2.t"
                        '"vnlog2.t","0xD01D8000","0xFFFF8080","%zt,%yt",
                        hex = &HD01D8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vmax.p"
                        '"vmax.p","0x6D800080","0xFF808080","%zp,%yp,%xp",
                        hex = &H6D800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vmax.q"
                        '"vmax.q","0x6D808080","0xFF808080","%zq,%yq,%xq",
                        hex = &H6D808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vmax.s"
                        '"vmax.s","0x6D800000","0xFF808080","%zs,%ys,%xs",
                        hex = &H6D800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vmax.t"
                        '"vmax.t","0x6D808000","0xFF808080","%zt,%yt,%xt",
                        hex = &H6D808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vmin.p"
                        '"vmin.p","0x6D000080","0xFF808080","%zp,%yp,%xp",
                        hex = &H6D000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vmin.q"
                        '"vmin.q","0x6D008080","0xFF808080","%zq,%yq,%xq",
                        hex = &H6D008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vmin.s"
                        '"vmin.s","0x6D000000","0xFF808080","%zs,%ys,%xs",
                        hex = &H6D000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vmin.t"
                        '"vmin.t","0x6D008000","0xFF808080","%zt,%yt,%xt",
                        hex = &H6D008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vmone.p"
                        '"vmone.p","0xF3870080","0xFFFFFF80","%zm",
                        hex = &HF3870080
                        hex = xyzpm(ss(0), hex, 0)
                    Case "vmone.q"
                        '"vmone.q","0xF3878080","0xFFFFFF80","%zo",
                        hex = &HF3878080
                        hex = xyzqo(ss(0), hex, 0)
                    Case "vmone.t"
                        '"vmone.t","0xF3878000","0xFFFFFF80","%zn",
                        hex = &HF3878000
                        hex = xyztn(ss(0), hex, 0)
                    Case "vmov.p"
                        '"vmov.p","0xD0000080","0xFFFF8080","%zp,%yp",
                        hex = &HD0000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vmov.q"
                        '"vmov.q","0xD0008080","0xFFFF8080","%zq,%yq",
                        hex = &HD0008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vmov.s"
                        '"vmov.s","0xD0000000","0xFFFF8080","%zs,%ys",
                        hex = &HD0000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vmov.t"
                        '"vmov.t","0xD0008000","0xFFFF8080","%zt,%yt",
                        hex = &HD0008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vmul.p"
                        '"vmul.p","0x64000080","0xFF808080","%zp,%yp,%xp",
                        hex = &H64000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vmul.q"
                        '"vmul.q","0x64008080","0xFF808080","%zq,%yq,%xq",
                        hex = &H64008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vmul.s"
                        '"vmul.s","0x64000000","0xFF808080","%zs,%ys,%xs",
                        hex = &H64000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vmul.t"
                        '"vmul.t","0x64008000","0xFF808080","%zt,%yt,%xt",
                        hex = &H64008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vneg.p"
                        '"vneg.p","0xD0020080","0xFFFF8080","%zp,%yp",
                        hex = &HD0020080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vneg.q"
                        '"vneg.q","0xD0028080","0xFFFF8080","%zq,%yq",
                        hex = &HD0028080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vneg.s"
                        '"vneg.s","0xD0020000","0xFFFF8080","%zs,%ys",
                        hex = &HD0020000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vneg.t"
                        '"vneg.t","0xD0028000","0xFFFF8080","%zt,%yt",
                        hex = &HD0028000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vnop"
                        '"vnop","0xFFFF0000","0xFFFFFFFF","",
                        hex = &HFFFF0000
                    Case "vnrcp.p"
                        '"vnrcp.p","0xD0180080","0xFFFF8080","%zp,%yp",
                        hex = &HD0180080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vnrcp.q"
                        '"vnrcp.q","0xD0188080","0xFFFF8080","%zq,%yq",
                        hex = &HD0188080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vnrcp.s"
                        '"vnrcp.s","0xD0180000","0xFFFF8080","%zs,%ys",
                        hex = &HD0180000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vnrcp.t"
                        '"vnrcp.t","0xD0188000","0xFFFF8080","%zt,%yt",
                        hex = &HD0188000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vnsin.p"
                        '"vnsin.p","0xD01A0080","0xFFFF8080","%zp,%yp",
                        hex = &HD01A0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vnsin.q"
                        '"vnsin.q","0xD01A8080","0xFFFF8080","%zq,%yq",
                        hex = &HD01A8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vnsin.s"
                        '"vnsin.s","0xD01A0000","0xFFFF8080","%zs,%ys",
                        hex = &HD01A0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vnsin.t"
                        '"vnsin.t","0xD01A8000","0xFFFF8080","%zt,%yt",
                        hex = &HD01A8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vncos.p"
                        '"vncos.p","0xD01b0080","0xFFFF8080","%zp,%yp",
                        hex = &HD01B0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vncos.q"
                        hex = &HD01B8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vncos.s"
                        hex = &HD01B0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vncos.t"
                        hex = &HD01B8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vocp.p"
                        '"vocp.p","0xD0440080","0xFFFF8080","%zp,%yp",
                        hex = &HD0440080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vocp.q"
                        '"vocp.q","0xD0448080","0xFFFF8080","%zq,%yq",
                        hex = &HD0448080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vocp.s"
                        '"vocp.s","0xD0440000","0xFFFF8080","%zs,%ys",
                        hex = &HD0440000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vocp.t"
                        '"vocp.t","0xD0448000","0xFFFF8080","%zt,%yt",
                        hex = &HD0448000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vone.p"
                        '"vone.p","0xD0070080","0xFFFFFF80","%zp",
                        hex = &HD0070080
                        hex = xyzp(ss(0), hex, 0)
                    Case "vone.q"
                        '"vone.q","0xD0078080","0xFFFFFF80","%zq",
                        hex = &HD0078080
                        hex = xyzq(ss(0), hex, 0)
                    Case "vone.s"
                        '"vone.s","0xD0070000","0xFFFFFF80","%zs",
                        hex = &HD0070000
                        hex = xyzs(ss(0), hex, 0)
                    Case "vone.t"
                        '"vone.t","0xD0078000","0xFFFFFF80","%zt",
                        hex = &HD0078000
                        hex = xyzt(ss(0), hex, 0)
                    Case "vqmul.q"
                        '"vqmul.q","0xF2808080","0xFF808080","%zq,%yq,%xq",
                        hex = &HF2808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vrcp.p"
                        '"vrcp.p","0xD0100080","0xFFFF8080","%zp,%yp",
                        hex = &HD0100080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vrcp.q"
                        '"vrcp.q","0xD0108080","0xFFFF8080","%zq,%yq",
                        hex = &HD0108080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vrcp.s"
                        '"vrcp.s","0xD0100000","0xFFFF8080","%zs,%ys",
                        hex = &HD0100000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vrcp.t"
                        '"vrcp.t","0xD0108000","0xFFFF8080","%zt,%yt",
                        hex = &HD0108000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vrexp2.p"
                        '"vrexp2.p","0xD01C0080","0xFFFF8080","%zp,%yp",
                        hex = &HD01C0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vrexp2.q"
                        '"vrexp2.q","0xD01C8080","0xFFFF8080","%zq,%yq",
                        hex = &HD01C8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vrexp2.s"
                        '"vrexp2.s","0xD01C0000","0xFFFF8080","%zs,%ys",
                        hex = &HD01C0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vrexp2.t"
                        '"vrexp2.t","0xD01C8000","0xFFFF8080","%zt,%yt",
                        hex = &HD01C8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vrndf1.p"
                        '"vrndf1.p","0xD0220080","0xFFFFFF80","%zp",
                        hex = &HD0220080
                        hex = xyzp(ss(0), hex, 0)
                    Case "vrndf1.q"
                        '"vrndf1.q","0xD0228080","0xFFFFFF80","%zq",
                        hex = &HD0228080
                        hex = xyzq(ss(0), hex, 0)
                    Case "vrndf1.s"
                        '"vrndf1.s","0xD0220000","0xFFFFFF80","%zs",
                        hex = &HD0220000
                        hex = xyzs(ss(0), hex, 0)
                    Case "vrndf1.t"
                        '"vrndf1.t","0xD0228000","0xFFFFFF80","%zt",
                        hex = &HD0228000
                        hex = xyzt(ss(0), hex, 0)
                    Case "vrndf2.p"
                        '"vrndf2.p","0xD0230080","0xFFFFFF80","%zp",
                        hex = &HD0230080
                        hex = xyzp(ss(0), hex, 0)
                    Case "vrndf2.q"
                        '"vrndf2.q","0xD0238080","0xFFFFFF80","%zq",
                        hex = &HD0238080
                        hex = xyzq(ss(0), hex, 0)
                    Case "vrndf2.s"
                        '"vrndf2.s","0xD0230000","0xFFFFFF80","%zs",
                        hex = &HD0230000
                        hex = xyzs(ss(0), hex, 0)
                    Case "vrndf2.t"
                        '"vrndf2.t","0xD0238000","0xFFFFFF80","%zt",
                        hex = &HD0238000
                        hex = xyzt(ss(0), hex, 0)
                    Case "vrndi.p"
                        '"vrndi.p","0xD0210080","0xFFFFFF80","%zp",
                        hex = &HD0210080
                        hex = xyzp(ss(0), hex, 0)
                    Case "vrndi.q"
                        '"vrndi.q","0xD0218080","0xFFFFFF80","%zq",
                        hex = &HD0218080
                        hex = xyzq(ss(0), hex, 0)
                    Case "vrndi.s"
                        '"vrndi.s","0xD0210000","0xFFFFFF80","%zs",
                        hex = &HD0210000
                        hex = xyzs(ss(0), hex, 0)
                    Case "vrndi.t"
                        '"vrndi.t","0xD0218000","0xFFFFFF80","%zt",
                        hex = &HD0218000
                        hex = xyzt(ss(0), hex, 0)
                    Case "vrnds.s"
                        '"vrnds.s","0xD0200000","0xFFFF80FF","%ys",
                        hex = &HD0200000
                        hex = xyzs(ss(0), hex, 1)
                    Case "vrsq.p"
                        '"vrsq.p","0xD0110080","0xFFFF8080","%zp,%yp",
                        hex = &HD0110080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vrsq.q"
                        '"vrsq.q","0xD0118080","0xFFFF8080","%zq,%yq",
                        hex = &HD0118080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vrsq.s"
                        '"vrsq.s","0xD0110000","0xFFFF8080","%zs,%ys",
                        hex = &HD0110000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vrsq.t"
                        '"vrsq.t","0xD0118000","0xFFFF8080","%zt,%yt",
                        hex = &HD0118000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vnrsq.p"
                        '"vnrsq.p","0xD0190080","0xFFFF8080","%zp,%yp",
                        hex = &HD0190080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vnrsq.q"
                        '"vnrsq.q","0xD0198080","0xFFFF8080","%zq,%yq",
                        hex = &HD0198080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vnrsq.s"
                        '"vnrsq.s","0xD0190000","0xFFFF8080","%zs,%ys",
                        hex = &HD0190000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vnrsq.t"
                        '"vnrsq.t","0xD0198000","0xFFFF8080","%zt,%yt",
                        hex = &HD0198000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)

                    Case "vs2i.p"
                        '"vs2i.p","0xD03B0080","0xFFFF8080","%zq,%yp",
                        hex = &HD03B0080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vs2i.s"
                        '"vs2i.s","0xD03B0000","0xFFFF8080","%zp,%ys",
                        hex = &HD03B0000
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsat0.p"
                        '"vsat0.p","0xD0040080","0xFFFF8080","%zp,%yp",
                        hex = &HD0040080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vsat0.q"
                        '"vsat0.q","0xD0048080","0xFFFF8080","%zq,%yq",
                        hex = &HD0048080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsat0.s"
                        '"vsat0.s","0xD0040000","0xFFFF8080","%zs,%ys",
                        hex = &HD0040000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsat0.t"
                        '"vsat0.t","0xD0048000","0xFFFF8080","%zt,%yt",
                        hex = &HD0048000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vsat1.p"
                        '"vsat1.p","0xD0050080","0xFFFF8080","%zp,%yp",
                        hex = &HD0050080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vsat1.q"
                        '"vsat1.q","0xD0058080","0xFFFF8080","%zq,%yq",
                        hex = &HD0058080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsat1.s"
                        '"vsat1.s","0xD0050000","0xFFFF8080","%zs,%ys",
                        hex = &HD0050000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsat1.t"
                        '"vsat1.t","0xD0058000","0xFFFF8080","%zt,%yt",
                        hex = &HD0058000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vsbn.s"
                        '"vsbn.s","0x61000000","0xFF808080","%zs,%ys,%xs",
                        hex = &H61000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vsbz.s"
                        '"vsbz.s","0xD0360000","0xFFFF8080","%zs,%ys",
                        hex = &HD0360000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vscl.s"
                        '"vscl.s","0x65000000","0xFF808080","%zs,%ys,%xs",
                        hex = &H65000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vscl.p"
                        '"vscl.p","0x65000080","0xFF808080","%zp,%yp,%xs",
                        hex = &H65000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vscl.q"
                        '"vscl.q","0x65008080","0xFF808080","%zq,%yq,%xs",
                        hex = &H65008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vscl.t"
                        '"vscl.t","0x65008000","0xFF808080","%zt,%yt,%xs",
                        hex = &H65008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vscmp.p"
                        '"vscmp.p","0x6E800080","0xFF808080","%zp,%yp,%xp",
                        hex = &H6E800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vscmp.q"
                        '"vscmp.q","0x6E808080","0xFF808080","%zq,%yq,%xq",
                        hex = &H6E808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vscmp.s"
                        '"vscmp.s","0x6E800000","0xFF808080","%zs,%ys,%xs",
                        hex = &H6E800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vscmp.t"
                        '"vscmp.t","0x6E808000","0xFF808080","%zt,%yt,%xt",
                        hex = &H6E808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vsge.p"
                        '"vsge.p","0x6F000080","0xFF808080","%zp,%yp,%xp",
                        hex = &H6F000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vsge.q"
                        '"vsge.q","0x6F008080","0xFF808080","%zq,%yq,%xq",
                        hex = &H6F008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vsge.s"
                        '"vsge.s","0x6F000000","0xFF808080","%zs,%ys,%xs",
                        hex = &H6F000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vsge.t"
                        '"vsge.t","0x6F008000","0xFF808080","%zt,%yt,%xt",
                        hex = &H6F008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vsgn.p"
                        '"vsgn.p","0xD04A0080","0xFFFF8080","%zp,%yp",
                        hex = &HD04A0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vsgn.q"
                        '"vsgn.q","0xD04A8080","0xFFFF8080","%zq,%yq",
                        hex = &HD04A8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsgn.s"
                        '"vsgn.s","0xD04A0000","0xFFFF8080","%zs,%ys",
                        hex = &HD04A0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsgn.t"
                        '"vsgn.t","0xD04A8000","0xFFFF8080","%zt,%yt",
                        hex = &HD04A8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vsin.p"
                        '"vsin.p","0xD0120080","0xFFFF8080","%zp,%yp",
                        hex = &HD0120080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vsin.q"
                        '"vsin.q","0xD0128080","0xFFFF8080","%zq,%yq",
                        hex = &HD0128080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsin.s"
                        '"vsin.s","0xD0120000","0xFFFF8080","%zs,%ys",
                        hex = &HD0120000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsin.t"
                        '"vsin.t","0xD0128000","0xFFFF8080","%zt,%yt",
                        hex = &HD0128000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vslt.p"
                        '"vslt.p","0x6F800080","0xFF808080","%zp,%yp,%xp",
                        hex = &H6F800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vslt.q"
                        '"vslt.q","0x6F808080","0xFF808080","%zq,%yq,%xq",
                        hex = &H6F808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vslt.s"
                        '"vslt.s","0x6F800000","0xFF808080","%zs,%ys,%xs",
                        hex = &H6F800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vslt.t"
                        '"vslt.t","0x6F808000","0xFF808080","%zt,%yt,%xt",
                        hex = &H6F808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vsocp.p"
                        '"vsocp.p","0xD0450080","0xFFFF8080","%zq,%yp",
                        hex = &HD0450080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vsocp.s"
                        '"vsocp.s","0xD0450000","0xFFFF8080","%zp,%ys",
                        hex = &HD0450000
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsqrt.p"
                        '"vsqrt.p","0xD0160080","0xFFFF8080","%zp,%yp",
                        hex = &HD0160080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vsqrt.q"
                        '"vsqrt.q","0xD0168080","0xFFFF8080","%zq,%yq",
                        hex = &HD0168080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsqrt.s"
                        '"vsqrt.s","0xD0160000","0xFFFF8080","%zs,%ys",
                        hex = &HD0160000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsqrt.t"
                        '"vsqrt.t","0xD0168000","0xFFFF8080","%zt,%yt",
                        hex = &HD0168000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vnsqrt.p"
                        '"vnsqrt.p","0xD01e0080","0xFFFF8080","%zp,%yp",
                        hex = &HD01E0080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vnsqrt.q"
                        '"vnsqrt.q","0xD01e8080","0xFFFF8080","%zq,%yq",
                        hex = &HD01E8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vnsqrt.s"
                        '"vnsqrt.s","0xD01e0000","0xFFFF8080","%zs,%ys",
                        hex = &HD01E0000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vnsqrt.t"
                        '"vsqrt.t","0xD01e8000","0xFFFF8080","%zt,%yt",
                        hex = &HD01E8000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                    Case "vsrt1.q"
                        '"vsrt1.q","0xD0408080","0xFFFF8080","%zq,%yq",
                        hex = &HD0408080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsrt2.q"
                        '"vsrt2.q","0xD0418080","0xFFFF8080","%zq,%yq",
                        hex = &HD0418080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsrt3.q"
                        '"vsrt3.q","0xD0488080","0xFFFF8080","%zq,%yq",
                        hex = &HD0488080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsrt4.q"
                        '"vsrt4.q","0xD0498080","0xFFFF8080","%zq,%yq",
                        hex = &HD0498080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vsub.p"
                        '"vsub.p","0x60800080","0xFF808080","%zp,%yp,%xp",
                        hex = &H60800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vsub.q"
                        '"vsub.q","0x60808080","0xFF808080","%zq,%yq,%xq",
                        hex = &H60808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vsub.s"
                        '"vsub.s","0x60800000","0xFF808080","%zs,%ys,%xs",
                        hex = &H60800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vsub.t"
                        '"vsub.t","0x60808000","0xFF808080","%zt,%yt,%xt",
                        hex = &H60808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vsync"
                        '"vsync","0xFFFF0320","0xFFFFFFFF","",
                        hex = &HFFFF0320
                    Case "vt4444.q"
                        '"vt4444.q","0xD0598080","0xFFFF8080","%zq,%yq",
                        hex = &HD0598080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vt5551.q"
                        '"vt5551.q","0xD05A8080","0xFFFF8080","%zq,%yq",
                        hex = &HD05A8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vt5650.q"
                        '"vt5650.q","0xD05B8080","0xFFFF8080","%zq,%yq",
                        hex = &HD05B8080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                    Case "vus2i.p"
                        '"vus2i.p","0xD03A0080","0xFFFF8080","%zq,%yp",
                        hex = &HD03A0080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                    Case "vus2i.s"
                        '"vus2i.s","0xD03A0000","0xFFFF8080","%zp,%ys",
                        hex = &HD03A0000
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vzero.p"
                        '"vzero.p","0xD0060080","0xFFFFFF80","%zp",
                        hex = &HD0060080
                        hex = xyzp(ss(0), hex, 0)
                    Case "vzero.q"
                        '"vzero.q","0xD0068080","0xFFFFFF80","%zq",
                        hex = &HD0068080
                        hex = xyzq(ss(0), hex, 0)
                    Case "vzero.s"
                        '"vzero.s","0xD0060000","0xFFFFFF80","%zs",
                        hex = &HD0060000
                        hex = xyzs(ss(0), hex, 0)
                    Case "vzero.t"
                        '"vzero.t","0xD0068000","0xFFFFFF80","%zt",
                        hex = &HD0068000
                        hex = xyzt(ss(0), hex, 0)

                    Case "vcmovf.p"
                        '"vcmovf.p","0xD2A80080","0xFFF88080","%zp,%yp,%v3",
                        hex = &HD2A80080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovf.q"
                        '"vcmovf.q","0xD2A88080","0xFFF88080","%zq,%yq,%v3",
                        hex = &HD2A88080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovf.s"
                        '"vcmovf.s","0xD2A80000","0xFFF88080","%zs,%ys,%v3",
                        hex = &HD2A80000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovf.t"
                        '"vcmovf.t","0xD2A88000","0xFFF88080","%zt,%yt,%v3",
                        hex = &HD2A88000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovt.p"
                        '"vcmovt.p","0xD2A00080","0xFFF88080","%zp,%yp,%v3",
                        hex = &HD2A00080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovt.q"
                        '"vcmovt.q","0xD2A08080","0xFFF88080","%zq,%yq,%v3",
                        hex = &HD2A08080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovt.s"
                        '"vcmovt.s","0xD2A00000","0xFFF88080","%zs,%ys,%v3",
                        hex = &HD2A00000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcmovt.t"
                        '"vcmovt.t","0xD2A08000","0xFFF88080","%zt,%yt,%v3",
                        hex = &HD2A08000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI3(ss(2), hex)
                    Case "vcst.p"
                        '"vcst.p","0xD0600080","0xFFE0FF80","%zp,%vk",
                        hex = &HD0600080
                        hex = xyzp(ss(0), hex, 0)
                        hex = VK(ss(1), hex)
                    Case "vcst.q"
                        '"vcst.q","0xD0608080","0xFFE0FF80","%zq,%vk",
                        hex = &HD0608080
                        hex = xyzq(ss(0), hex, 0)
                        hex = VK(ss(1), hex)
                    Case "vcst.s"
                        '"vcst.s","0xD0600000","0xFFE0FF80","%zs,%vk",
                        hex = &HD0600000
                        hex = xyzs(ss(0), hex, 0)
                        hex = VK(ss(1), hex)
                    Case "vcst.t"
                        '"vcst.t","0xD0608000","0xFFE0FF80","%zt,%vk",
                        hex = &HD0608000
                        hex = xyzt(ss(0), hex, 0)
                        hex = VK(ss(1), hex)
                    Case "vf2id.p"
                        '"vf2id.p","0xD2600080","0xFFE08080","%zp,%yp,%v5",
                        hex = &HD2600080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2id.q"
                        '"vf2id.q","0xD2608080","0xFFE08080","%zq,%yq,%v5",
                        hex = &HD2608080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2id.s"
                        '"vf2id.s","0xD2600000","0xFFE08080","%zs,%ys,%v5",
                        hex = &HD2600000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2id.t"
                        '"vf2id.t","0xD2608000","0xFFE08080","%zt,%yt,%v5",
                        hex = &HD2608000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2in.p"
                        '"vf2in.p","0xD2000080","0xFFE08080","%zp,%yp,%v5",
                        hex = &HD2000080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2in.q"
                        '"vf2in.q","0xD2008080","0xFFE08080","%zq,%yq,%v5",
                        hex = &HD2008080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2in.s"
                        '"vf2in.s","0xD2000000","0xFFE08080","%zs,%ys,%v5",
                        hex = &HD2000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2in.t"
                        '"vf2in.t","0xD2008000","0xFFE08080","%zt,%yt,%v5",
                        hex = &HD2008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iu.p"
                        '"vf2iu.p","0xD2400080","0xFFE08080","%zp,%yp,%v5",
                        hex = &HD2400080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iu.q"
                        '"vf2iu.q","0xD2408080","0xFFE08080","%zq,%yq,%v5",
                        hex = &HD2408080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iu.s"
                        '"vf2iu.s","0xD2400000","0xFFE08080","%zs,%ys,%v5",
                        hex = &HD2400000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iu.t"
                        '"vf2iu.t","0xD2408000","0xFFE08080","%zt,%yt,%v5",
                        hex = &HD2408000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iz.p"
                        '"vf2iz.p","0xD2200080","0xFFE08080","%zp,%yp,%v5",
                        hex = &HD2200080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iz.q"
                        '"vf2iz.q","0xD2208080","0xFFE08080","%zq,%yq,%v5",
                        hex = &HD2208080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iz.s"
                        '"vf2iz.s","0xD2200000","0xFFE08080","%zs,%ys,%v5",
                        hex = &HD2200000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vf2iz.t"
                        '"vf2iz.t","0xD2208000","0xFFE08080","%zt,%yt,%v5",
                        hex = &HD2208000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vi2f.p"
                        '"vi2f.p","0xD2800080","0xFFE08080","%zp,%yp,%v5",
                        hex = &HD2800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzp(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vi2f.q"
                        '"vi2f.q","0xD2808080","0xFFE08080","%zq,%yq,%v5",
                        hex = &HD2808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzq(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vi2f.s"
                        '"vi2f.s","0xD2800000","0xFFE08080","%zs,%ys,%v5",
                        hex = &HD2800000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)
                    Case "vi2f.t"
                        '"vi2f.t","0xD2808000","0xFFE08080","%zt,%yt,%v5",
                        hex = &HD2808000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyzt(ss(1), hex, 1)
                        hex = VI5(ss(2), hex)

                    Case "vhtfm2.p"
                        '"vhtfm2.p","0xF0800000","0xFF808080","%zp,%ym,%xp",
                        hex = &HF0800000
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzpm(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vhtfm3.t"
                        '"vhtfm3.t","0xF1000080","0xFF808080","%zt,%yn,%xt",
                        hex = &HF1000080
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyztn(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vhtfm4.q"
                        '"vhtfm4.q","0xF1808000","0xFF808080","%zq,%yo,%xq",
                        hex = &HF1808000
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzqo(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)
                    Case "vmidt.p"
                        '"vmidt.p","0xF3830080","0xFFFFFF80","%zm",
                        hex = &HF3830080
                        hex = xyzpm(ss(0), hex, 0)
                    Case "vmidt.q"
                        '"vmidt.q","0xF3838080","0xFFFFFF80","%zo",
                        hex = &HF3838080
                        hex = xyzqo(ss(0), hex, 0)
                    Case "vmidt.t"
                        '"vmidt.t","0xF3838000","0xFFFFFF80","%zn",
                        hex = &HF3838000
                        hex = xyztn(ss(0), hex, 0)
                    Case "vmmov.p"
                        '"vmmov.p","0xF3800080","0xFFFF8080","%zm,%ym",
                        hex = &HF3800080
                        hex = xyzpm(ss(0), hex, 0)
                        hex = xyzpm(ss(1), hex, 1)
                    Case "vmmov.q"
                        '"vmmov.q","0xF3808080","0xFFFF8080","%zo,%yo",
                        hex = &HF3808080
                        hex = xyzqo(ss(0), hex, 0)
                        hex = xyzqo(ss(1), hex, 1)
                    Case "vmmov.t"
                        '"vmmov.t","0xF3808000","0xFFFF8080","%zn,%yn",
                        hex = &HF3808000
                        hex = xyztn(ss(0), hex, 0)
                        hex = xyztn(ss(1), hex, 1)
                    Case "vmscl.p"
                        '"vmscl.p","0xF2000080","0xFF808080","%zm,%ym,%xs",
                        hex = &HF2000080
                        hex = xyzpm(ss(0), hex, 0)
                        hex = xyzpm(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vmscl.q"
                        '"vmscl.q","0xF2008080","0xFF808080","%zo,%yo,%xs",
                        hex = &HF2008080
                        hex = xyzqo(ss(0), hex, 0)
                        hex = xyzqo(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vmscl.t"
                        '"vmscl.t","0xF2008000","0xFF808080","%zn,%yn,%xs",
                        hex = &HF2008000
                        hex = xyztn(ss(0), hex, 0)
                        hex = xyztn(ss(1), hex, 1)
                        hex = xyzs(ss(2), hex, 2)
                    Case "vmzero.p"
                        '"vmzero.p","0xF3860080","0xFFFFFF80","%zm",
                        hex = &HF3860080
                        hex = xyzpm(ss(0), hex, 0)
                    Case "vmzero.q"
                        '"vmzero.q","0xF3868080","0xFFFFFF80","%zo",
                        hex = &HF3868080
                        hex = xyzqo(ss(0), hex, 0)
                    Case "vmzero.t"
                        '"vmzero.t","0xF3868000","0xFFFFFF80","%zn",
                        hex = &HF3868000
                        hex = xyztn(ss(0), hex, 0)
                    Case "vtfm2.p"
                        '"vtfm2.p","0xF0800080","0xFF808080","%zp,%ym,%xp",
                        hex = &HF0800080
                        hex = xyzp(ss(0), hex, 0)
                        hex = xyzpm(ss(1), hex, 1)
                        hex = xyzp(ss(2), hex, 2)
                    Case "vtfm3.t"
                        '"vtfm3.t","0xF1008000","0xFF808080","%zt,%yn,%xt",
                        hex = &HF1008000
                        hex = xyzt(ss(0), hex, 0)
                        hex = xyztn(ss(1), hex, 1)
                        hex = xyzt(ss(2), hex, 2)
                    Case "vtfm4.q"
                        '"vtfm4.q","0xF1808080","0xFF808080","%zq,%yo,%xq",
                        hex = &HF1808080
                        hex = xyzq(ss(0), hex, 0)
                        hex = xyzqo(ss(1), hex, 1)
                        hex = xyzq(ss(2), hex, 2)

                    Case "vmmul.p"
                        '"vmmul.p","0xF0000080","0xFF808080","%?%zm,%ym,%xm",
                        hex = &HF0000080
                        hex = xyzpm(ss(0), hex, 0)
                        Dim x As Integer = xyzpm(ss(1), 0, 0)
                        If (x And &H20) <> 0 Then
                            x = x And &H5F
                        Else
                            x = x Or &H20
                        End If
                        hex = hex Or (x << 8)
                        hex = xyzpm(ss(2), hex, 2)
                    Case "vmmul.q"
                        '"vmmul.q","0xF0008080","0xFF808080","%?%zo,%yo,%xo",
                        hex = &HF0008080
                        hex = xyzqo(ss(0), hex, 0)
                        Dim x As Integer = xyzqo(ss(1), 0, 0)
                        If (x And &H20) <> 0 Then
                            x = x And &H5F
                        Else
                            x = x Or &H20
                        End If
                        hex = hex Or (x << 8)
                        hex = xyzqo(ss(2), hex, 2)
                    Case "vmmul.t"
                        '"vmmul.t","0xF0008000","0xFF808080","%?%zn,%yn,%xn",
                        hex = &HF0008000
                        hex = xyztn(ss(0), hex, 0)
                        Dim x As Integer = xyztn(ss(1), 0, 0)
                        If (x And &H20) <> 0 Then
                            x = x And &H5F
                        Else
                            x = x Or &H20
                        End If
                        hex = hex Or (x << 8)
                        hex = xyztn(ss(2), hex, 2)

                    Case "vmfvc"
                        '"vmfvc","0xD0500000","0xFFFF0080","%zs,%2s",
                        hex = &HD0500000
                        hex = xyzs(ss(0), hex, 0)
                        hex = sd(ss(1), hex, 1)
                    Case "vmtvc"
                        '"vmtvc","0xD0510000","0xFFFF8000","%2d,%ys",
                        hex = &HD0510000
                        hex = sd(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                    Case "vsync"
                        '"vsync","0xFFFF0000","0xFFFF0000","%I",
                        hex = &HFFFF0000
                        hex = Imm(ss(0), hex)
                    Case "vwb.q"
                        '"vwb.q","0xF8000002","0xFC000002","%Xq,%Y",
                        hex = &HF8000002
                        hex = Xq(ss(0), hex)
                        hex = Y(" " & ss(1), hex)
                    Case "vwbn.s", "vsbi.s"
                        '"vwbn.s","0xD3000000","0xFF008080","%zs,%ys,%N",
                        hex = &HD3000000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = hex Or ((Imm(ss(2), 0) And &HFF) << 16)
                    Case "mfvme"
                        '"mfvme","0x68000000","0xFC000000","%t,%i",
                        hex = &H68000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = Imm(ss(1), hex)
                    Case "mtvme"
                        '"mtvme","0xb0000000","0xFC000000","%t,%i",
                        hex = &HB0000000
                        hex = reg_boolean_para(ss(0), hex, 1)
                        hex = Imm(ss(1), hex)
                    Case "vrot.s", "vnrot.s"
                        '"vrot.s","0xF3A00000","0xFFE08080","%zs,%ys,%vr",
                        hex = &HF3A00000
                        hex = xyzs(ss(0), hex, 0)
                        hex = xyzs(ss(1), hex, 1)
                        hex = VR(str, hex, 1)
                        hex = negationswap(hex, &H100000, mips(1) = "n")
                    Case "vrot.p", "vnrot.p"
                            '"vrot.p","0xF3A00080","0xFFE08080","%zp,%ys,%vr",
                            hex = &HF3A00080
                            hex = xyzp(ss(0), hex, 0)
                            hex = xyzs(ss(1), hex, 1)
                        hex = VR(str, hex, 2)
                        hex = negationswap(hex, &H100000, mips(1) = "n")
                    Case "vrot.q", "vnrot.q"
                            '"vrot.q","0xF3A08080","0xFFE08080","%zq,%ys,%vr",
                            hex = &HF3A08080
                            hex = xyzq(ss(0), hex, 0)
                            hex = xyzs(ss(1), hex, 1)
                        hex = VR(str, hex, 4)
                        hex = negationswap(hex, &H100000, mips(1) = "n")
                    Case "vrot.t", "vnrot.t"
                            '"vrot.t","0xF3A08000","0xFFE08080","%zt,%ys,%vr",
                            hex = &HF3A08000
                            hex = xyzt(ss(0), hex, 0)
                            hex = xyzs(ss(1), hex, 1)
                        hex = VR(str, hex, 3)
                        hex = negationswap(hex, &H100000, mips(1) = "n")
                    Case "vpfxd"
                            '"vpfxd","0xDE000000","0xFF000000","[%vp4,%vp5,%vp6,%vp7]",
                            hex = &HDE000000
                            hex = vp4567(ss(0), hex, 4)
                            hex = vp4567(ss(1), hex, 5)
                            hex = vp4567(ss(2), hex, 6)
                            hex = vp4567(ss(3), hex, 7)
                    Case "vpfxs"
                            '"vpfxs","0xDC000000","0xFF000000","[%vp0,%vp1,%vp2,%vp3]",
                            hex = &HDC000000
                            hex = vp0123(ss(0).Replace("[", ""), hex, 0)
                            hex = vp0123(ss(1), hex, 1)
                            hex = vp0123(ss(2), hex, 2)
                            hex = vp0123(ss(3).Replace("]", ""), hex, 3)
                    Case "vpfxt"
                            '"vpfxt","0xDD000000","0xFF000000","[%vp0,%vp1,%vp2,%vp3]",
                            hex = &HDD000000
                            hex = vp0123(ss(0).Replace("[", ""), hex, 0)
                            hex = vp0123(ss(1), hex, 1)
                            hex = vp0123(ss(2), hex, 2)
                            hex = vp0123(ss(3).Replace("]", ""), hex, 3)
                    Case ".word"
                            If RPN.Checked = True Then
                                hex = valint(str.Replace(".word", "").Trim)
                            Else
                                hex = valword(str.Trim)
                            End If
                    Case ".float"
                            hex = cvt_float(valfloat(str.Trim.Replace(".float", "")))
                End Select

                asm = "0x" & Convert.ToString(hex, 16).ToUpper.PadLeft(8, "0"c)
            End If

                Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return ""
        End Try
    End Function

    Function negationswap(ByVal hex As Integer, ByVal sw As Integer, ByVal ok As Boolean) As Integer
        If ok = True Then
            hex = hex Xor sw
        End If
        Return hex
    End Function

    Function vp0123(ByVal str As String, ByVal hex As Integer, ByVal p As Integer) As Integer

        Dim pfx_cst_names As String() = {"0", "1", "2", "1/2", "3", "1/3", "1/4", "1/6"}
        Dim pfx_swz_names As String() = {"x", "y", "z", "w"}

        'unsigned int base = '0';
        'unsigned int negation = (l >> (pos - base + 16)) & 1;
        'unsigned int constant = (l >> (pos - base + 12)) & 1;
        'unsigned int abs_consthi = (l >> (pos - base + 8)) & 1;
        'unsigned int swz_constlo = (l >> ((pos - base) * 2)) & 3;

        '     If (negation) Then
        '	len = sprintf(output, "-");
        '         If (constant) Then
        '{
        '	len += sprintf(output+len, "%s", pfx_cst_names[(abs_consthi << 2) | swz_constlo]);
        '}
        '         Else
        '{
        '             If (abs_consthi) Then
        '		len += sprintf(output+len, "|%s|", pfx_swz_names[swz_constlo]);
        '             Else
        '		len += sprintf(output+len, "%s", pfx_swz_names[swz_constlo]);
        '}
        str = str.Trim

        Dim i As Integer = 0
        Dim k As Integer = 0
        If str.Contains("-") Then
            k += 1 << (p + 16)
            str = str.Replace("-", "")
        End If
        If str.Contains("|") Then
            str = str.Replace("|", "")
            k += 1 << (p + 8)
            For i = 0 To 3
                If (str = pfx_swz_names(i)) Then
                    Exit For
                End If
            Next
            k += (i And 3) << (p * 2)
        Else
            For i = 0 To 3
                If (str = pfx_swz_names(i)) Then
                    Exit For
                End If
            Next
            If i < 4 Then
                k += i << (p * 2)
            Else
                For i = 0 To 7
                    If (str = pfx_cst_names(i)) Then
                        Exit For
                    End If
                Next
                If i < 8 Then
                    k += (i And 3) << (p * 2)
                    k += 1 << (p + 12)
                    k += (i >> 2) << (p + 8)
                End If
            End If
        End If

        hex = hex Or k

        Return hex
    End Function

    Function vp4567(ByVal str As String, ByVal hex As Integer, ByVal p As Integer) As Integer
        Dim pfx_sat_names As String() = {"", "[0:1]", "", "[-1:1]"}
        'unsigned int base = '4';
        'unsigned int mask = (l >> (pos - (base - 8))) & 1K;
        'unsigned int saturation = (l >> ((pos - base) * 2)) & 3;

        '     If (mask) Then
        '	len += sprintf(output, "m");
        '     Else
        '	len += sprintf(output, "%s", pfx_sat_names[saturation]);
        str = str.Trim
        Dim valdec As New Regex("\[(0|-1):1\]")
        Dim valdecm As Match = valdec.Match(str)
        Dim i As Integer = 0
        Dim k As Integer = 0
        If str.Contains("m") Then
            k += 1 << (p - 4 + 8)
        ElseIf valdecm.Success Then
            str = valdecm.Value
            For i = 0 To 3
                If (str = pfx_sat_names(i)) Then
                    Exit For
                End If
            Next
            If i < 4 Then
                k += (i And 3) << ((p - 4) * 2)
            End If
        End If
        hex = hex Or k


        Return hex
    End Function

    Function VR(ByVal str As String, ByVal hex As Integer, ByVal m As Integer) As Integer
        'Dim s As String() = {
        '"[c,s,s,s]", "[s,c,0,0]", "[s,0,c,0]", "[s,0,0,c]", "[c,s,0,0]", "[s,c,s,s]", "[0,s,c,0]", "[0,s,0,c]", 
        '"[c,0,s,0]", "[0,c,s,0]", "[s,s,c,s]", "[0,0,s,c]", "[c,0,0,s]", "[0,c,0,s]", "[0,0,c,s]", "[s,s,s,c]", 
        '"[c,-s,-s,-s]", "[-s,c,0,0]", "[-s,0,c,0]", "[-s,0,0,c]", "[c,-s,0,0]",  "[-s,c,-s,-s]", "[0,-s,c,0]", "[0,-s,0,c]", 
        '"[c,0,-s,0]", "[0,c,-s,0]", "[-s,-s,c,-s]", "[0,0,-s,c]", "[c,0,0,-s]", "[0,c,0,-s]", "[0,0,c,-s]", "[-s,-s,-s,c]"}

        Dim i As Integer = 0
        Dim csct As Integer = -1
        Dim sct As Integer = -1
        Dim zct As Integer = -1
        Dim neg As Boolean = False
        Dim lotlo As Integer = -1
        Dim lothi As Integer = -1
        Dim valk As New Regex("\[.+\]")
        Dim valkm As Match = valk.Match(str)
        Dim cs0 As New Regex("(-?s|[c0])")
        Dim cs0m As Match
        Dim st(4) As String
        If valkm.Success Then
            cs0m = cs0.Match(valkm.Value)
            While cs0m.Success
                st(i) = cs0m.Value
                If st(i) = "c" Then
                    lotlo = i
                    csct += 1
                    If csct > 0 Then
                        MessageBox.Show("コサインは1つだけ指定可能です")
                        Return hex
                    End If
                ElseIf st(i) = "0" Then
                    zct += 1
                Else
                    lothi = i
                    sct += 1
                    If st(i).Contains("-") Then
                        neg = True
                    End If
                    If zct > 0 AndAlso sct > 1 Then
                        MessageBox.Show("0がある場合-s,sは１つだけ指定可能です")
                        Return hex
                    End If
                    If neg = True AndAlso st(i) = "s" Then
                        MessageBox.Show("サインの符号は1種類のみです。-s,sを同時に指定はできません")
                        Return hex
                    End If
                End If
                i += 1
                cs0m = cs0m.NextMatch
            End While

            If m = 4 Then
                If csct < 0 Then
                    MessageBox.Show("コサインが指定されてません。最低1つは必要です")
                    Return hex
                End If
                i = lotlo
                'sが１つだけある
                If lothi >= 0 AndAlso sct = 0 Then
                    i = i Or ((lothi) << 2)
                    'sが２つ以上
                ElseIf lothi >= 0 AndAlso sct > 0 Then
                    i = i Or ((lotlo) << 2)
                End If
            ElseIf m = 3 Then
                'ない場合は自然と３
                If lotlo = -1 Then
                    lotlo = 3
                End If
                i = lotlo
                If lothi >= 0 AndAlso sct = 0 Then
                    i = i Or ((lothi) << 2)
                ElseIf lothi >= 0 AndAlso sct > 0 Then
                    i = i Or ((lotlo) << 2)
                    '00Cパターン
                Else
                    i += 12
                End If
            ElseIf m = 2 Then
                If lotlo = -1 Then
                    lotlo = 2
                End If
                i = lotlo
                If lothi >= 0 AndAlso sct = 0 Then
                    i = i Or ((lothi) << 2)
                ElseIf lothi >= 0 AndAlso sct > 0 Then
                    i = i Or ((lotlo) << 2)
                    '00
                ElseIf zct = 1 Then
                    i = 11
                Else
                    i += 8
                End If
            Else
                Select Case st(0)
                    Case "0"
                        i = 6
                    Case "c"
                        i = 0
                    Case "s", "-s"
                        i = 1
                End Select
            End If
        End If

        If neg = True Then
            i = i Or &H10
        End If

        hex = hex Or (i << 16)

        Return hex
    End Function

    Function VI3(ByVal str As String, ByVal hex As Integer) As Integer
        Dim k As Integer = 0
        Dim valdec As New Regex("\d{1,3}")
        Dim valdecm As Match = valdec.Match(str)
        If valdecm.Success Then
            k = CInt(valdecm.Value)
        End If
        hex = hex Or ((k And 7) << 16)
        Return hex
    End Function

    Function VI5(ByVal str As String, ByVal hex As Integer) As Integer
        Dim k As Integer = 0
        Dim valdec As New Regex("\d{1,3}")
        Dim valdecm As Match = valdec.Match(str)
        If valdecm.Success Then
            k = CInt(valdecm.Value)
        End If
        hex = hex Or ((k And &H1F) << 16)
        Return hex
    End Function

    Function VI8(ByVal str As String, ByVal hex As Integer) As Integer
        Dim k As Integer = 0
        Dim valdec As New Regex("\d{1,3}")
        Dim valdecm As Match = valdec.Match(str)
        If valdecm.Success Then
            k = CInt(valdecm.Value)
        End If
        hex = hex Or ((k And 255) << 16)
        Return hex
    End Function

    Function VK(ByVal str As String, ByVal hex As Integer) As Integer
        hex = hex Or ((VK_sel(str) And &H1F) << 16)
        Return hex
    End Function

    Function VK_sel(ByVal str As String) As Integer
        Dim s As String() = {
  "",
  "vfpu_huge",
  "vfpu_sqrt2",
  "vfpu_sqrt1_2",
  "vfpu_2_sqrtpi",
  "vfpu_2_pi",
  "vfpu_1_pi",
  "vfpu_pi_4",
  "vfpu_pi_2",
  "vfpu_pi",
  "vfpu_e",
  "vfpu_log2e",
  "vfpu_log10e",
  "vfpu_ln2",
  "vfpu_ln10",
  "vfpu_2pi",
  "vfpu_pi_6",
  "vfpu_log10two",
  "vfpu_log2ten",
  "vfpu_sqrt3_2"}
        str = str.Trim
        If str.Contains("vfpu") = True Then
            Dim i As Integer
            For i = 0 To 20
                If s(i) = str Then
                    Exit For
                End If
            Next
            If i < 20 Then
                Return i
            End If
        End If
        Dim valdec As New Regex("\d{1,3}")
        Dim valdecm As Match = valdec.Match(str)
        If valdecm.Success Then

            Return CInt(valdecm.Value) And &H1F

        End If

        Return 0

    End Function

    Function sd(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        If str.Contains("vfpu") = True Then
            hex = hex Or ((cop2_sel(str) Or &H80) << (8 * k))
        Else
            Dim j As Integer = Imm(str, j) And &H7F
            hex = hex Or (j << (8 * k))
        End If
        Return hex
    End Function

    Function cop2_sel(ByVal str As String) As Integer
        Dim ss As String() = {
 "vfpu_pfxs",
 "vfpu_pfxt",
 "vfpu_pfxd",
 "vfpu_cc",
 "vfpu_inf4",
     "",
     "",
 "vfpu_rev",
 "vfpu_rcx0",
 "vfpu_rcx1",
 "vfpu_rcx2",
 "vfpu_rcx3",
 "vfpu_rcx4",
 "vfpu_rcx5",
 "vfpu_rcx6",
 "vfpu_rcx7"}
        str = str.Trim
        If str.Contains("vfpu") = True Then
            Dim i As Integer
            For i = 0 To 15
                If ss(i) = str Then
                    Exit For
                End If
            Next
            If i < 16 Then
                Return i
            End If
            Dim valdec As New Regex("\d{1,3}")
            Dim valdecm As Match = valdec.Match(str)
            If valdecm.Success Then

                Return CInt(valdecm.Value) And &H7F

            End If
        End If
        Return 5


    End Function

    Function cop2_parse(ByVal str As String, ByVal hex As Integer) As Integer
        hex = hex Or cop2_sel(str)
        Return hex
    End Function

    Function Zn(ByVal str As String, ByVal hex As Integer) As Integer
        Dim reg As New Regex("(FL|EQ|LT|LE|TR|NE|GE|GT|EZ|EN|EI|ES|NZ|NN|NI|NS)", RegexOptions.IgnoreCase)
        Dim regm As Match = reg.Match(str)
        If regm.Success Then
            hex = hex Or (vbranch_sel(regm.Value))
        End If
        Return hex
    End Function

    Function vbranch_sel(ByVal s As String) As Integer
        Dim ss As String() = {
            "fl", "eq", "lt", "le",
          "tr", "ne", "ge", "gt",
          "ez", "en", "ei", "es",
          "nz", "nn", "ni", "ns"
        }
        Dim i As Integer = 0

        For i = 0 To 15
            If ss(i) = s Then
                Exit For
            End If
        Next
        If i = 16 Then
            i = 0
        End If

        Return i
    End Function

    Function xyzpm(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("(m[0-7]?[02]?[02]|e[0-7]?[02]?[02])")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "m" Then
                If (tmp And 2) = 2 Then
                    hex = hex Or ((&H40 + ((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                Else
                    hex = hex Or ((((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                End If
            Else
                If ((tmp >> 4) And 2) = 2 Then
                    hex = hex Or ((&H60 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                Else
                    hex = hex Or ((&H20 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                End If
            End If
        End If
        Return hex
    End Function

    Function xyztn(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("(m[0-7]?[01]?[01]|e[0-7]?[01]?[01])")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "m" Then
                If (tmp And 1) = 1 Then
                    hex = hex Or ((&H40 + ((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                Else
                    hex = hex Or ((((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                End If
            Else
                If ((tmp >> 4) And 1) = 1 Then
                    hex = hex Or ((&H60 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                Else
                    hex = hex Or ((&H20 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                End If
            End If
        End If
        Return hex
    End Function

    Function xyzqo(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("(m[0-7]?0?0]|e[0-7]?0?0)")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "m" Then
                hex = hex Or ((((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
            Else
                hex = hex Or ((&H20 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
            End If
        End If
        Return hex
    End Function

    Function xyzs(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("s[0-7]?[0-3]{1,2}")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            hex = hex Or (((tmp >> 8) << 2) + ((tmp >> 4) And 3) + ((tmp And 3) << 5) << (8 * k))
        End If
        Return hex
    End Function

    Function xyzp(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("(c[0-7]?[0-3]?[02]|r[0-7]?[02]?[0-3])")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "c" Then
                If (tmp And 2) = 2 Then
                    hex = hex Or ((&H40 + ((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                Else
                    hex = hex Or ((((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                End If
            Else
                If ((tmp >> 4) And 2) = 2 Then
                    hex = hex Or ((&H60 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                Else
                    hex = hex Or ((&H20 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                End If
            End If
        End If
        Return hex
    End Function

    Function xyzt(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("(c[0-7]?[0-3]?[01]|r[0-7]?[01]?[0-3])")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "c" Then
                If (tmp And 1) = 1 Then
                    hex = hex Or ((&H40 + ((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                Else
                    hex = hex Or ((((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
                End If
            Else
                If ((tmp >> 4) And 1) = 1 Then
                    hex = hex Or ((&H60 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                Else
                    hex = hex Or ((&H20 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
                End If
            End If
        End If
        Return hex
    End Function

    Function xyzq(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim vf As New Regex("(c[0-7]?[0-3]?0|r[0-7]?0?[0-3])")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "c" Then
                hex = hex Or ((((tmp >> 8) << 2) + ((tmp >> 4) And 3)) << (8 * k))
            Else
                hex = hex Or ((&H20 + ((tmp >> 8) << 2) + (tmp And 3)) << (8 * k))
            End If
        End If
        Return hex
    End Function

    Function Xs(ByVal str As String, ByVal hex As Integer) As Integer
        Dim vf As New Regex("s[0-7]?[0-3]{1,2}")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            hex = hex Or ((tmp >> 8) << 18) + (((tmp >> 4) And 3) << 16) + ((tmp And 3))
        End If
        Return hex
    End Function

    Function Xq(ByVal str As String, ByVal hex As Integer) As Integer
        Dim vf As New Regex("(c[0-7]?[0-3]?0|r[0-7]?0?[0-3])")
        Dim vfm As Match = vf.Match(str)
        Dim tmp As Integer = 0
        If vfm.Success Then
            tmp = Convert.ToInt32(vfm.Value.Remove(0, 1), 16)
            If vfm.Value(0) = "c" Then
                hex = hex Or ((tmp >> 8) << 18) + (((tmp >> 4) And 3) << 16)
            Else
                hex += 1
                hex = hex Or ((tmp >> 8) << 18) + ((tmp And 3) << 16)
            End If
        End If
        Return hex
    End Function

    Function Zc(ByVal str As String, ByVal hex As Integer) As Integer
        Dim dec As New Regex("\d")
        Dim decm As Match = dec.Match(str)
        If decm.Success Then
            hex = hex Or ((CInt(decm.Value) And 7) << 18)
        End If
        Return hex
    End Function

    Function float_sel(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim freg As New Regex("\$(f|fpr)\d{1,2}")
        Dim fregm As Match = freg.Match(str)
        If fregm.Success Then
            Dim dec As New Regex("\d{1,2}")
            Dim decm As Match = dec.Match(fregm.Value)
            hex = hex Or ((CInt(decm.Value) And 31) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function cop_sel(ByVal str As String, ByVal mode As String) As Integer
        Dim cop0 As String() = {"INDEX", "RANDOM", "ENTRYLO0", "ENTRYLO1", "CONTEXT", "PAGEMASK", "WIRED", "7", "BADVADDR", "COUNT", "ENTRYHI", "COMPARE", "STATUS", "CAUSE", "EPC", "PRID", "CONFIG", "LLADDR", "WATCHLO", "WATCHHI", "XCONTEXT", "21", "22", "DEBUG", "DEPC", "PERFCNT", "ERRCTL", "CACHEERR", "TAGLO", "TAGHI", "ERROREPC", "DESAVE"}
        str = str.Replace("$", "")
        Dim i As Integer
        If Integer.TryParse(str, i) Then
            i = CInt(str) And 31
        ElseIf mode = "COP0" Then
            For i = 0 To 32
                If i = 32 Then
                    i = 0
                    Exit For
                ElseIf str.Contains(cop0(i).ToLower) Then
                    Exit For
                End If
            Next
        End If
        Return i
    End Function

    Function debug_reg(ByVal str As String, ByVal hex As Integer) As Integer
        Dim dr As String() = {"DRCNTL", "DEPC", "DDATA0", "DDATA1", "IBC", "DBC", "6", "7", _
 "IBA", "IBAM", "10", "11", "DBA", "DBAM", "DBD", "DBDM"}
        str = str.Replace("$", "").Trim
        Dim i As Integer
        If Integer.TryParse(str, i) Then
            i = CInt(str) And 31
        Else
            For i = 0 To 15
                If i = 32 Then
                    i = 0
                    Exit For
                ElseIf str.Contains(dr(i).ToLower) Then
                    Exit For
                End If
            Next
        End If
        hex = hex Or (i << 11)
        Return hex
    End Function

    'type j/jal
    Function offset_boolean(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim k As Integer = 0
        Dim j As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If MODE.SelectedIndex = 4 Then
                If k >= &H9800000 Then
                    k -= &H9800000
                End If
            Else
                If k < &H1800000 Then
                    k += &H8800000
                End If
            End If
            hex = hex Or ((k >> 2) And &H3FFFFFF)
        Else
            Dim cma As Integer = str.Trim.LastIndexOf(" ")
            If cma < 0 Then
                cma = str.Trim.LastIndexOf(vbTab)
            End If
            str = str.Substring(cma + 1, str.Length - cma - 1).Trim
            For j = 0 To 255
                If label(j) = str Then
                    Exit For
                End If
            Next
            If (j < 255) Then
                k = label_addr(j)

                If MODE.SelectedIndex = 4 Then
                    If k >= &H9800000 Then
                        k -= &H9800000
                    End If
                Else
                    If k < &H1800000 Then
                        k += &H8800000
                    End If
                End If
                hex = hex Or ((k >> 2) And &H3FFFFFF)
            Else
                MessageBox.Show(str & "に該当するラベルがみつかりませんでした", "")
            End If
        End If
            Return hex
    End Function

    'type branch offset
    Function offset_boolean2(ByVal str As String, ByVal hex As Integer, ByVal hex2 As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|\-)?\d{1,4}$")
        Dim valdecm As Match = valdec.Match(str)
        Dim k As Integer = 0
        Dim j As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If MODE.SelectedIndex = 4 Then
                If k >= &H9800000 Then
                    k -= &H9800000
                End If
                If hex2 >= &H9800000 Then
                    hex2 -= &H9800000
                End If
            Else
                If k < &H1800000 Then
                    k += &H8800000
                End If
                If hex2 < &H1800000 Then
                    hex2 += &H8800000
                End If
            End If
            hex = hex Or ((k - hex2 - 4) >> 2 And &HFFFF)
        ElseIf valdecm.Success Then
            hex = hex Or ((Convert.ToInt32(valdecm.Value.Remove(0, 1)) - 1) And &HFFFF)
        Else
            Dim cma As Integer = str.LastIndexOf(",")
            str = str.Substring(cma + 1, str.Length - cma - 1).Trim
            For j = 0 To 255
                If label(j) = str Then
                    Exit For
                End If
            Next
            If (j < 255) Then
                k = label_addr(j)
                If MODE.SelectedIndex = 4 Then
                    If k >= &H9800000 Then
                        k -= &H9800000
                    End If
                    If hex2 >= &H9800000 Then
                        hex2 -= &H9800000
                    End If
                Else
                    If k < &H1800000 Then
                        k += &H8800000
                    End If
                    If hex2 < &H1800000 Then
                        hex2 += &H8800000
                    End If
                End If
                hex = hex Or ((k - hex2 - 4) >> 2 And &HFFFF)
            Else
                MessageBox.Show(str & "に該当するラベルがみつかりませんでした", "")
            End If
            End If
        Return hex
    End Function

    'type lw/sw offset
    Function offset_boolean3(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)-?(\$|0x)[0-9A-Fa-f]{1,4}\(")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|-)?\d{1,5}\(")
        Dim valdecm As Match = valdec.Match(str)
        Dim vallb As New Regex("(\x20|,|\t)s?(lower_).*?\(")
        Dim vallbm As Match = vallb.Match(str)
        Dim valadd As New Regex(",\[(\+|\-)\d{1,5}\]$")
        Dim valaddm As Match = valadd.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            If s.Contains("-") Then
                k = &H10000
                s = s.Replace("-", "")
                k = k - Convert.ToInt32(s.Replace("$", "").Remove(0, 1).Replace("(", ""), 16)
            Else
                k = Convert.ToInt32(s.Replace("$", "").Remove(0, 1).Replace("(", ""), 16)
            End If
            hex = hex Or (k And &HFFFF)
        ElseIf valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1).Replace("(", "")) And &HFFFF)
        Else
            Dim j As Integer = 0
            Dim sl As Integer = 0
            If vallbm.Success Then
                sl = 2
                str = vallbm.Value.Remove(0, 1).Replace("(", "")
                str = str.Remove(0, str.IndexOf("_") + 1)
                For j = 0 To 255
                    If label(j) = str Then
                        Exit For
                    End If
                Next
                If (j < 255) Then
                    k = label_addr(j)
                    If MODE.SelectedIndex = 4 Then
                        If k >= &H9800000 Then
                            k -= &H9800000
                        End If
                    Else
                        If k < &H1800000 Then
                            k += &H8800000
                        End If
                    End If
                    If (sl And 2) = 2 Then
                        k = k And &HFFFF
                    End If
                    hex = hex Or k
                Else
                    MessageBox.Show(str & "に該当するラベルがみつかりませんでした", "")
                End If
                Else
                    MessageBox.Show(str & "でラベルを使用する場合はラベル名の前にslowerかlower_をつけて下さい", "")
                End If
        End If

        If valaddm.Success Then
            Dim s As String = valaddm.Value
            hex = hex + (Convert.ToInt32(s.Remove(0, 2).Replace("]", "")))
        End If

        Return hex
    End Function

    'vfpu Imm
    Function Imm(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\$|0x)[0-9A-Fa-f]{1,4}")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("\d{1,5}")
        Dim valdecm As Match = valdec.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            k = Convert.ToInt32(s.Replace("$", ""), 16)
            hex = hex Or (k And &HFFFF)
        End If
        If valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value) And &HFFFF)
        End If
        Return hex
    End Function

    'vfpu offset
    Function Y(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)-?(\$|0x)[0-9A-Fa-f]{1,4}\(")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|-)?\d{1,5}\(")
        Dim valdecm As Match = valdec.Match(str)
        Dim vallb As New Regex("(\x20|,|\t)s?(lower_).*?\(")
        Dim vallbm As Match = vallb.Match(str)
        Dim valadd As New Regex(",\[(\+|\-)\d{1,5}\]$")
        Dim valaddm As Match = valadd.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            If s.Contains("-") Then
                k = &H10000
                s = s.Replace("-", "")
                k = k - Convert.ToInt32(s.Replace("$", "").Remove(0, 1).Replace("(", ""), 16)
            Else
                k = Convert.ToInt32(s.Replace("$", "").Remove(0, 1).Replace("(", ""), 16)
            End If
            hex = hex Or (k And &HFFFC)
        ElseIf valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1).Replace("(", "")) And &HFFFC)
        Else
            Dim j As Integer = 0
            Dim sl As Integer = 0
            If vallbm.Success Then
                sl = 2
                str = vallbm.Value.Remove(0, 1).Replace("(", "")
                str = str.Remove(0, str.IndexOf("_") + 1)
                For j = 0 To 255
                    If label(j) = str Then
                        Exit For
                    End If
                Next
                If (j < 255) Then
                    k = label_addr(j)

                    If MODE.SelectedIndex = 4 Then
                        If k >= &H9800000 Then
                            k -= &H9800000
                        End If
                    Else
                        If k < &H1800000 Then
                            k += &H8800000
                        End If
                    End If
                    If (sl And 2) = 2 Then
                        k = k And &HFFFF
                    End If
                    hex = hex Or (k And &HFFFC)
                Else
                    MessageBox.Show(str & "に該当するラベルがみつかりませんでした", "")
                End If
                Else
                    MessageBox.Show(str & "でラベルを使用する場合はラベル名の前にslowerかlower_をつけて下さい", "")
                End If
        End If
        If valaddm.Success Then
            Dim s As String = valaddm.Value
            hex = hex + (Convert.ToInt32(s.Remove(0, 2).Replace("]", "")))
        End If

        Return hex
    End Function

    Function valhex_syscall(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,5}$")
        Dim valhexm As Match = valhex.Match(str)
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            Dim minus As Integer = 0
            minus = Convert.ToInt32(s.Replace("$", "").Remove(0, 1), 16)
            hex = hex Or (minus And &HFFFFF) << 6
        End If
        Return hex
    End Function

    Function valhex_boolean(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,)-?(\$|0x)[0-9A-Fa-f]{1,4}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,)-?\d{1,5}$")
        Dim valdecm As Match = valdec.Match(str)
        Dim valfloat As New Regex("(\x20|,)-?\d+\.?\d*f$")
        Dim valfloatm As Match = valfloat.Match(str)
        Dim valhfloat As New Regex("(\x20|,)-?\d+\.?\d*hf$")
        Dim vallb As New Regex("(\x20|,)s?(upper_|lower_)")
        Dim vallbm As Match = vallb.Match(str)
        Dim valhfloatm As Match = valhfloat.Match(str)
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            Dim minus As Integer = 0
            If s.Contains("-") Then
                s = s.Replace("-", "")
                minus = &H10000
                minus -= Convert.ToInt32(s.Replace("$", "").Remove(0, 1), 16) And &HFFFF
            Else
                minus = Convert.ToInt32(s.Replace("$", "").Remove(0, 1), 16)
            End If
            hex = hex Or (minus And &HFFFF)
        ElseIf valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1)) And &HFFFF)

        ElseIf valfloatm.Success Then

            Dim f As Single = Convert.ToSingle(valfloatm.Value.Remove(0, 1).Replace("f", ""))
            Dim bit() As Byte = BitConverter.GetBytes(f)
            Dim sb As New System.Text.StringBuilder()
            Dim i As Integer = 3
            While i >= 0
                sb.Append(Convert.ToString(bit(i), 16).PadLeft(2, "0"c))
                i -= 1
            End While
            hex = hex Or (Convert.ToInt32(sb.ToString.Substring(0, 4), 16))
        ElseIf valhfloatm.Success Then

            Dim f As Single = Convert.ToSingle(valhfloatm.Value.Remove(0, 1).Replace("hf", ""))
            Dim bit() As Byte = BitConverter.GetBytes(f)
            Dim sb As New System.Text.StringBuilder()
            Dim i As Integer = 3
            While i >= 0
                sb.Append(Convert.ToString(bit(i), 16).PadLeft(2, "0"c))
                i -= 1
            End While

            Dim s As String = converthalffloat(sb.ToString)

            hex = hex Or (Convert.ToInt32(s.Substring(0, 4), 16))
        Else
            Dim k As Integer = 0
            Dim j As Integer = 0
            Dim sl As Integer = 0
            Dim cma As Integer = 0
            If vallbm.Success Then
                If vallbm.Value.Contains("upper") Then
                    sl = 1
                Else
                    sl = 2
                End If
                If vallbm.Value.Contains("s") Then
                    sl += 4
                End If
                str = str.Replace(vallbm.Value.Remove(0, 1), "")
                cma = vallbm.Index
            End If
            str = str.Remove(0, cma + 1).Trim
            For j = 0 To 255
                If label(j) = str Then
                    Exit For
                End If
            Next
            If (j < 255) Then
                k = label_addr(j)
                If MODE.SelectedIndex = 4 Then
                    If k >= &H9800000 Then
                        k -= &H9800000
                    End If
                Else
                    If k < &H1800000 Then
                        k += &H8800000
                    End If
                End If
                If (sl And 1) = 1 Then
                    If (sl And 4) = 4 AndAlso (k And &HFFFF) >= &H8000 Then
                        k = k >> 16
                        k += 1
                    Else
                        k = k >> 16
                    End If
                ElseIf (sl And 2) = 2 Then
                    k = k And &HFFFF
                End If
                hex = hex Or k
            Else
                MessageBox.Show(str & "に該当するラベルがみつかりませんでした", "")
            End If
            End If
        Return hex
    End Function

    Function valword(ByVal str As String) As Integer
        Dim valhex As New Regex("(\x20|,)-?(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,)-?\d{1,10}$")
        Dim valdecm As Match = valdec.Match(str)
        Dim vallb As New Regex("(\x20|,)s?(upper_|lower_)")
        Dim vallbm As Match = vallb.Match(str)
        Dim minus As Int64 = 0
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            If s.Contains("-") Then
                s = s.Replace("-", "")
                minus = &H10000
                minus -= Convert.ToInt64(s.Replace("$", "").Remove(0, 1), 16)
            Else
                minus = Convert.ToInt64(s.Replace("$", "").Remove(0, 1), 16)
            End If
        ElseIf valdecm.Success Then
            minus = Convert.ToInt64(valdecm.Value.Remove(0, 1))
        Else
            Dim k As Integer = 0
            Dim j As Integer = 0
            Dim sl As Integer = 0
            If vallbm.Success Then
                If vallbm.Value.Contains("up") Then
                    sl = 1
                Else
                    sl = 2
                End If
                If vallbm.Value.Contains("s") Then
                    sl += 4
                End If
                str = str.Replace(vallbm.Value, "")
            End If
            Dim cma As Integer = str.Trim.LastIndexOf(" ")
            If cma < 0 Then
                cma = str.Trim.LastIndexOf(vbTab)
            End If
            str = str.Substring(cma + 1, str.Length - cma - 1).Trim
            For j = 0 To 255
                If label(j) = str Then
                    Exit For
                End If
            Next
            If (j < 255) Then
                k = label_addr(j)


                If MODE.SelectedIndex = 4 Then
                    If k >= &H9800000 Then
                        k -= &H9800000
                    End If
                Else
                    If k < &H1800000 Then
                        k += &H8800000
                    End If
                End If
                If (sl And 1) = 1 Then
                    If (sl And 4) = 4 AndAlso (k And &HFFFF) >= &H8000 Then
                        k = k >> 16
                        k += 1
                    Else
                        k = k >> 16
                    End If
                ElseIf (sl And 2) = 2 Then
                    k = k And &HFFFF
                End If
                minus = k
            Else
                MessageBox.Show(str & "に該当するラベルがみつかりませんでした", "")
            End If
        End If

        minus = minus And 4294967295
        If minus >= 2147483648 Then
            minus -= 4294967296
        End If
        Return CInt(minus)
    End Function


    Dim maths As String() = {"tan", "tanh", "sin", "sinh", "cos", "cosh", "sqrt", "ln", "log", "exp", "pow", _
                             "atan", "asin", "acos", "atan2_", "atand", "asind", "acosd", "atan2_d", _
                             "atanr", "asinr", "acosr", "atan2_r", "atang", "asing", "acosg", "atan2_g", _
                             "cbrt", "xrt", "yrt"
                            }
    
    Dim mathsconst As String() = {"π", "円周率", "黄金比", "自然対数の底"}
    Dim mathrp As String() = {"pi", "pi", "goldenratio", "e"}

    Private Function cvt_dbl(ByVal s As String) As Double
        Dim dem As Double = 0
        Dim cnst As New Regex("-?(pi|goldenratio|e)")
        Dim cnstm As Match = cnst.Match(s)
        Dim frac As New Regex("-?\d+\.?\d*")
        Dim fracm As Match = frac.Match(s)
        If cnstm.Success Then
            If cnstm.Value.Contains("goldenratio") Then
                dem = ((1 + Math.Sqrt(5)) / 2)
            ElseIf cnstm.Value.Contains("e") Then
                dem = (Math.E)
            ElseIf cnstm.Value.Contains("pi") Then
                dem = (Math.PI)
            End If
            If cnstm.Value.Contains("-") Then
                dem = -dem
            End If
            If fracm.Success Then
                dem *= Convert.ToDouble(fracm.Value)
            End If
        ElseIf fracm.Success Then
            dem = Convert.ToDouble(fracm.Value)
        End If

            Return dem
    End Function

    Private Function swapper(ByVal dem As Double()) As Double()
        Dim demt As Double
        demt = dem(1)
        dem(1) = dem(0)
        dem(0) = demt
        Return dem
    End Function

    Private Function swapper2(ByVal dem As Double()) As Double()
        If LOOKSORDER.Checked Then
            dem = swapper(dem)
        End If
        Return dem
    End Function

    Private Function swapper3(ByVal dem As Double()) As Double()
        Dim demt As Double
        If LOOKSORDER.Checked = False Then
            demt = dem(0)
            dem(0) = dem(2)
            dem(2) = demt
        End If
        Return dem
    End Function

    Private Function hmsdms(ByVal dd As Double, ByVal k As Decimal) As Double
        Dim b As Decimal = Convert.ToDecimal(dd)
        Dim d As Decimal = Math.Floor(b)
        Dim m As Decimal = Math.Floor((b - d) * 100)
        Dim s As Decimal = (((b - d) * 100) - m) * 100
        b = (d + m / 60 + s / 3600) * k
        Return Convert.ToDouble(b)
    End Function

    Private Function cvtdrg(ByVal d As Double, ByVal s As String) As Double
        Select Case s
            Case "d"
                d = d * 180 / Math.PI
            Case "r"
                d = d * 2 / Math.PI
            Case "g"
                d = d * 200 / Math.PI
        End Select
        Return d
    End Function

    Private Function cvtdrg2rad(ByVal d As Double, ByVal s As String) As Double
        Select Case s
            Case "d"
                d = d / 180 * Math.PI
            Case "r"
                d = d / 2 * Math.PI
            Case "g"
                d = d / 200 * Math.PI
        End Select
        Return d
    End Function

    Private Function rpndbl(ByVal str As String) As Double
        Dim ss As String() = str.ToLower.Split(CChar(","))
        Dim len As Integer = ss.Length - 1
        Dim dem(len) As Double
        For i = 0 To len
            Select Case ss(i).Trim
                Case "chs", "+/-"
                    dem(0) = -dem(0)
                Case "abs", "|x|"
                    dem(0) = Math.Abs(dem(0))
                Case "drop"
                    Array.Copy(dem, 1, dem, 0, len)
                Case "swap"
                    dem = swapper(dem)
                Case "swap3"
                    dem = swapper3(dem)
                Case "dms2deg", "dms2d", "hms2h"
                    dem(0) = hmsdms(dem(0), 1)
                Case "hms2deg", "hms2d"
                    dem(0) = hmsdms(dem(0), 15)
                Case "dms>deg", "dms>d"
                    dem = swapper3(dem)
                    dem(2) = dem(2) + dem(1) / 60 + dem(0) / 3600
                    Array.Copy(dem, 2, dem, 0, len - 2)
                Case "hms>deg", "hms>d"
                    dem = swapper3(dem)
                    dem(2) = 15 * (dem(2) + dem(1) / 60 + dem(0) / 3600)
                    Array.Copy(dem, 2, dem, 0, len - 2)
                Case "hms>h"
                    dem = swapper3(dem)
                    dem(2) = (dem(2) + dem(1) / 60 + dem(0) / 3600)
                    Array.Copy(dem, 2, dem, 0, len - 2)
                Case "deg2rad", "d2rad"
                    dem(0) = dem(0) * Math.PI / 180
                Case "deg2grad", "d2g"
                    dem(0) = dem(0) * 100 / 90
                Case "deg2r", "d2r"
                    dem(0) = dem(0) / 90
                Case "rad2deg", "rad2d"
                    dem(0) = dem(0) * 180 / Math.PI
                Case "rad2grad", "rad2g"
                    dem(0) = dem(0) * 200 / Math.PI
                Case "rad2r"
                    dem(0) = dem(0) * 2 / Math.PI
                Case "grad2deg", "g2d"
                    dem(0) = dem(0) * 9 / 10
                Case "grad2rad", "g2rad"
                    dem(0) = dem(0) * Math.PI / 200
                Case "grad2r", "g2r"
                    dem(0) = dem(0) / 100
                Case "r2deg", "r2d"
                    dem(0) = dem(0) * 90
                Case "r2rad"
                    dem(0) = dem(0) * Math.PI / 2
                Case "r2grad", "r2g"
                    dem(0) = dem(0) * 100
                Case "="
                Case "+"
                    dem(1) = dem(0) + dem(1)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "-"
                    dem(1) = dem(1) - dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "*"
                    dem(1) = dem(1) * dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "/"
                    dem(1) = dem(1) / dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "rpow", "y^x"
                    dem = swapper2(dem)
                    dem(1) = Math.Pow(dem(1), dem(0))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "pow", "x^y", "^"
                    dem = swapper2(dem)
                    dem(1) = Math.Pow(dem(0), dem(1))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "xrt"
                    dem = swapper2(dem)
                    dem(1) = Math.Pow(dem(1), 1 / dem(0))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "yrt"
                    dem = swapper2(dem)
                    dem(1) = Math.Pow(dem(0), 1 / dem(1))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "logy", "logms"
                    dem = swapper2(dem)
                    dem(1) = Math.Log(dem(0), dem(1))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "logx"
                    dem = swapper2(dem)
                    dem(1) = Math.Log(dem(1), dem(0))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "1/x", "reci"
                    dem(0) = 1 / dem(0)
                Case "hypot"
                    dem(1) = Math.Sqrt(Math.Pow(dem(1), 2) + Math.Pow(dem(0), 2))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "exp"
                    dem(0) = Math.Exp(dem(0))
                Case "sqrt", "√"
                    dem(0) = Math.Sqrt(dem(0))
                Case "cbrt"
                    dem(0) = Math.Pow(dem(0), 1 / 3)
                Case "logtwo"
                    dem(0) = Math.Log(dem(0), 2)
                Case "logtree"
                    dem(0) = Math.Log(dem(0), 3)
                Case "logten"
                    dem(0) = Math.Log(dem(0), 10)
                Case "ln", "loge", "log"
                    dem(0) = Math.Log(dem(0), Math.E)
                Case "atanh2_d", "atanh2_", "atanh2_r", "atanh2_g"
                    '0.86867096148601,1.32460908925201,atanh2_d
                    dem = swapper2(dem)
                    dem(0) = dem(1) / dem(0)
                    dem(1) = Math.Log(((1 + dem(0)) / (1 - dem(0)))) / 2
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 7))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "atanh2ms_d", "atanh2ms_", "atanh2ms_r", "atanh2ms_g"
                    '1.32460908925201,0.86867096148601,atanh2ms_d
                    dem = swapper2(dem)
                    dem(0) = dem(0) / dem(1)
                    dem(1) = Math.Log(((1 + dem(0)) / (1 - dem(0)))) / 2
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 9))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "atanh", "atanhd", "atanhr", "atanhg"
                    '1.32460908925201,0.86867096148601,/,reci,atanhd
                    dem(0) = Math.Log(((1 + dem(0)) / (1 - dem(0)))) / 2
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 5))
                Case "acosh", "acoshd", "acoshr", "acoshg"
                    '1.32460908925201,acoshd
                    dem(0) = Math.Log(dem(0) + Math.Sqrt(Math.Pow(dem(0), 2) - 1))
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 5))
                Case "acoshn", "acoshnd", "acoshnr", "acoshng"
                    '1.32460908925201,acoshd
                    dem(0) = Math.Log(dem(0) - Math.Sqrt(Math.Pow(dem(0), 2) - 1))
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 6))
                Case "asinh", "asinhd", "asinhr", "asinhg"
                    '0.86867096148601,asinhd
                    dem(0) = Math.Log(dem(0) + Math.Sqrt(Math.Pow(dem(0), 2) + 1))
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 5))
                Case "atan", "atand", "atanr", "atang"
                    dem(0) = Math.Atan(dem(0))
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 4))
                Case "atan2_", "atan2_d", "atan2_r", "atan2_g"
                    '4*(4*atan(1/5)-atan(1/239))
                    '4,4,5,1/x,atan,*,239,1/x,atan,-,*
                    dem = swapper2(dem)
                    dem(1) = Math.Atan2(dem(1), dem(0))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 6))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "atan2ms_", "atan2ms_d", "atan2ms_r", "atan2ms_g"
                    dem = swapper2(dem)
                    dem(1) = Math.Atan2(dem(0), dem(1))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 8))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "acos", "acosd", "acosr", "acosg"
                    dem(0) = Math.Acos(dem(0))
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 4))
                Case "asin", "asind", "asinr", "asing"
                    dem(0) = Math.Asin(dem(0))
                    dem(0) = cvtdrg(dem(0), ss(i).Trim.Remove(0, 4))
                Case "tanh", "tanhd", "tanhr", "tanhg"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = Math.Tanh(dem(0))
                Case "cosh", "coshd", "coshr", "coshg"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = Math.Cosh(dem(0))
                Case "sinh", "sinhd", "sinhr", "sinhg"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = Math.Sinh(dem(0))
                Case "tan", "tand", "tanr", "tang"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 3))
                    dem(0) = Math.Tan(dem(0))
                Case "cos", "cosd", "cosr", "cosg"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 3))
                    dem(0) = Math.Cos(dem(0))
                Case "sin", "sind", "sinr", "sing"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 3))
                    dem(0) = Math.Sin(dem(0))
                Case "asechd", "asechr", "asechg", "asech"
                    'ハイパーボリック(アークセカント(AsecH(x)))
                    'Log((Sqrt(-x * x + 1) + 1) / x)
                    dem(0) = Math.Log((Math.Sqrt(-dem(0) * dem(0) + 1) + 1) / dem(0))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 5))
                Case "acschd", "acschr", "acschg", "acsch"
                    'ハイパーボリック(アークコセカント(Acsch(x)))
                    'Log((Sign(x) * Sqrt(x * x + 1) + 1) / x)
                    dem(0) = Math.Log((Math.Sign(dem(0)) * Math.Sqrt(-dem(0) * dem(0) + 1) + 1) / dem(0))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 5))
                Case "acothd", "acothr", "acothg", "acoth"
                    'ハイパーボリック(アークコタンジェント(Acoth(x)))
                    'Log((x + 1) / (x – 1)) / 2 
                    dem(0) = Math.Log((dem(0) + 1) / (dem(0) - 1)) / 2
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 5))
                Case "actanhd", "actanhr", "actanhg", "actanh"
                    'ハイパーボリック(アークコタンジェント(Acoth(x)))
                    'Log((x + 1) / (x – 1)) / 2 
                    dem(0) = Math.Log((dem(0) + 1) / (dem(0) - 1)) / 2
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 6))
                Case "actang", "actand", "actanr", "actan"
                    'アークコタンジェント(Acot(x))
                    '2 * Atan(1) - Atan(x) 
                    dem(0) = 2 * Math.Atan(1) - Math.Atan(dem(0))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 5))
                Case "acotg", "acotd", "acotr", "acot"
                    'アークコタンジェント(Acot(x))
                    '2 * Atan(1) - Atan(x) 
                    dem(0) = 2 * Math.Atan(1) - Math.Atan(dem(0))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 4))
                Case "asecg", "asecd", "asecr", "asec"
                    'アークセカント(Asec(x))
                    '2 * Atan(1) – Atan(Sign(x) / Sqrt(x * x – 1))
                    dem(0) = 2 * Math.Atan(1) - Math.Atan(Math.Sign(dem(0)) / Math.Sqrt(dem(0) * dem(0) - 1))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 4))
                Case "acscg", "acscd", "acscr", "acsc"
                    'アークコセカント(Acsc(x))
                    'Atan(Sign(x) / Sqrt(x * x – 1))
                    dem(0) = Math.Atan(Math.Sign(dem(0) / Math.Sqrt(dem(0) * dem(0) - 1)))
                Case "sechg", "sechd", "sechr", "sech"
                    'ハイパーボリック(セカント(Sech(x)))
                    '2 / (Exp(x) + Exp(-x))
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = 2 / (Math.Exp(dem(0)) + Math.Exp(-dem(0)))
                Case "cschg", "cschd", "cschr", "csch"
                    'ハイパーボリック(コセカント(Csch(x)))
                    '2 / (Exp(x) – Exp(-x))
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = 2 / (Math.Exp(dem(0)) - Math.Exp(-dem(0)))
                Case "ctanhg", "ctanhd", "ctanhr", "ctanh"
                    'ハイパーボリック(コタンジェント(Coth(x)))
                    '(Exp(x) + Exp(-x)) / (Exp(x) – Exp(-x)) 
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 5))
                    dem(0) = (Math.Exp(dem(0)) + Math.Exp(-dem(0))) / (Math.Exp(dem(0)) - Math.Exp(-dem(0)))
                Case "cothg", "cothd", "cothr", "coth"
                    'ハイパーボリック(コタンジェント(Coth(x)))
                    '(Exp(x) + Exp(-x)) / (Exp(x) – Exp(-x)) 
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = (Math.Exp(dem(0)) + Math.Exp(-dem(0))) / (Math.Exp(dem(0)) - Math.Exp(-dem(0)))
                    dem(1) = cvtdrg(dem(1), ss(i).Trim.Remove(0, 4))
                Case "ctang", "ctand", "ctanr", "ctan"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 4))
                    dem(0) = 1 / Math.Tan(dem(0))
                Case "cotg", "cotd", "cotr", "cot"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 3))
                    dem(0) = 1 / Math.Tan(dem(0))
                Case "cscg", "cscd", "cscr", "csc"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 3))
                    dem(0) = 1 / Math.Sin(dem(0))
                Case "secg", "secd", "secr", "sec"
                    dem(0) = cvtdrg2rad(dem(0), ss(i).Trim.Remove(0, 3))
                    dem(0) = 1 / Math.Cos(dem(0))
                Case Else
                    If (ss(i)).Trim <> "" Then
                        Array.Copy(dem, 0, dem, 1, len)
                        dem(0) = cvt_dbl(ss(i))
                    End If
            End Select
        Next
        dem(0) = Math.Round(dem(0), 14)

        Return dem(0)

    End Function

    Function valfloat(ByVal str As String) As Double

        Dim i As Integer = 0
        For i = 0 To mathsconst.Length - 1
            If str.Contains(mathsconst(i)) Then
                str = str.Replace(mathsconst(i), mathrp(i))
            End If
        Next
        Dim k As Double = 0
        If RPN.Checked Then
            If CVTRPN.Checked Then
                Dim p As New Polish
                str = p.Main(str, LOOKSORDER.Checked, cr.Checked, False)
            End If
            k = rpndbl(str)
        Else
            k = calcdbl(str)
        End If
        Return k

    End Function

    Private Function calcdbl(ByVal str As String) As Double
        Dim i As Integer = 0

        Dim k As Double = 0
        Dim valfl As New Regex("(\x20|,)[+-]?\d+\.?\d+$")
        Dim valflm As Match = valfl.Match(str)
        Dim valmugen As New Regex("(\x20|,)[+-]?(nan|inf|e|pi|goldenratio)$")
        Dim valmugenm As Match = valmugen.Match(str)
        Dim valmath As New Regex("(tanh|tan|sinh|sin|cosh|cos|sqrt|cbrt|[xy]rt|ln|log|exp|pow|atan2_[drg]?|atan[drg]?|asin[drg]?|acos[drg]?)")
        Dim valmathm As Match = valmath.Match(str)
        If valflm.Success Then
            k = Convert.ToSingle(valflm.Value.Remove(0, 1))
        ElseIf valmugenm.Success Then
            If valmugenm.Value.Contains("pi") Then
                k = Convert.ToSingle(Math.PI)
            ElseIf valmugenm.Value.Contains("goldenratio") Then
                k = Convert.ToDouble((1 + Math.Sqrt(5)) / 2)
            ElseIf valmugenm.Value.Contains("e") Then
                k = Convert.ToDouble(Math.E)
            ElseIf valmugenm.Value.Contains("nan") Then
                k = 1.0E+308
            Else
                k = 1.0E+307
            End If
            If valmugenm.Value.Contains("-") Then
                k = -k
            End If
        ElseIf valmathm.Success Then
            Dim hh As String = valmathm.Value
            str = str.Remove(0, valmathm.Index + valmathm.Length).Replace("deg", "dgr").Replace("grade", "grad")
            Dim valmu As New Regex("[+-]?(e|pi|goldenratio)")
            Dim valmum As Match = valmu.Match(str)
            While valmum.Success
                Dim y As Double = 0
                If valmum.Value.Contains("pi") Then
                    y = Convert.ToSingle(Math.PI)
                ElseIf valmum.Value.Contains("goldenratio") Then
                    y = Convert.ToSingle((1 + Math.Sqrt(5)) / 2)
                ElseIf valmum.Value.Contains("e") Then
                    y = Convert.ToSingle(Math.E)
                End If
                If valmum.Value.Contains("-") Then
                    y = -y
                End If
                str = str.Replace(valmum.Value, y.ToString)
                valmum = valmum.NextMatch
            End While
            Dim vald As New Regex("-?\d+\.?\d*")
            Dim valdm As Match = vald.Match(str)
            k = Convert.ToDouble(valdm.Value)
            Dim ss As String() = str.Split(CChar(","))
            Dim angle As Double = calcradian(str)
            angle = angle_cvt(str.Trim, angle)

            For i = 0 To maths.Length - 1
                If hh = maths(i) Then
                    Select Case i
                        Case 0
                            k = Math.Tan(angle)
                            If angle = Math.PI / 2 Or angle = -Math.PI * 3 / 2 Then
                                k = 1.0E+307
                            ElseIf angle = -Math.PI / 2 Or angle = Math.PI * 3 / 2 Then
                                k = -1.0E+307
                            ElseIf angle = Math.PI Or angle = -Math.PI Then
                                k = 0
                            End If
                        Case 1
                            k = Math.Tanh(angle)
                        Case 2
                            k = Math.Sin(angle)
                            If angle = Math.PI Or angle = -Math.PI Then
                                k = 0
                            End If
                        Case 3
                            k = Math.Sinh(angle)
                        Case 4
                            k = Math.Cos(angle)
                            If angle = Math.PI / 2 Or angle = -Math.PI / 2 Or angle = -Math.PI * 3 / 2 Or angle = Math.PI * 3 / 2 Then
                                k = 0
                            End If
                        Case 5
                            k = Math.Cosh(angle)
                        Case 6
                            k = Math.Sqrt(k)
                        Case 27
                            k = Math.Pow(k, 1 / 3)
                        Case 7
                            k = Math.Log(k)
                        Case 8
                            k = valfloat(ss(0))
                            If ss.Length < 2 Then
                                MessageBox.Show("引数が２つ必要です。(x,y)で指定してください")
                                k = 0
                            Else
                                angle = valfloat(ss(1))
                                k = Math.Log(angle, k)
                            End If
                        Case 9
                            k = Math.Exp(k)
                        Case 10, 28, 29
                            If ss.Length < 2 Then
                                ss = str.Split(CChar("^"))
                            End If
                            If ss.Length < 2 Then
                                MessageBox.Show("引数が2つ必要です。(x,y)かx^yで指定してください")
                                k = 0
                            Else
                                k = valfloat(ss(0))
                                angle = valfloat(ss(1))
                                If i = 28 Then
                                    Dim sw As Double = 1 / k
                                    k = angle
                                    angle = sw
                                ElseIf i = 29 Then
                                    angle = 1 / angle
                                End If
                                k = Math.Pow(k, angle)
                            End If
                        Case 11, 15, 19, 23
                            k = Math.Atan(k)
                        Case 12, 16, 20, 24
                            k = Math.Asin(k)
                        Case 13, 17, 21, 25
                            k = Math.Acos(k)
                        Case 14, 18, 22, 26
                            If ss.Length < 2 Then
                                MessageBox.Show("引数が2つ必要です。(x,y)で指定してください")
                                k = 0
                            Else
                                k = valfloat(ss(0))
                                angle = valfloat(ss(1))
                                k = Math.Atan2(k, angle)
                            End If
                    End Select
                    Exit For
                End If
            Next
            '度数
            If i >= 15 AndAlso i <= 18 Then
                k = k * 180 / Math.PI
                '直角
            ElseIf i >= 19 AndAlso i <= 22 Then
                k = k * 2 / Math.PI
                'グラード
            ElseIf i >= 22 AndAlso i <= 26 Then
                k = k * 200 / Math.PI
            End If

        Else
            k = calcradian(str)
        End If

        Return k
    End Function

    Private Function cvt_float(ByVal k As Double) As Integer
        If k = 1.0E+307 Then
            Return &H7F000000
        ElseIf k = 1.0E+308 Then
            k = 1.0E+308
            Return &H7F800000
        ElseIf k = -1.0E+307 Then
            Return &HFF800000
        ElseIf k = -1.0E+308 Then
            Return &HFF800000
        End If
        Dim f As Single = Convert.ToSingle(k)
        Dim b As Byte() = BitConverter.GetBytes(f)
        Return BitConverter.ToInt32(b, 0)
    End Function

    Private Function angle_cvt(ByVal s As String, ByVal angle As Double) As Double

        Dim valmu As New Regex("(度|グラード|直角|dgr|grad|gon|°|rad|r|∟)")
        Dim valmum As Match = valmu.Match(s)
        Dim valhms As New Regex("-?\d+\.?\d*[dhms度°時分秒′″]")
        Dim valhmsm As Match = valhms.Match(s)
        Dim hms As Boolean = False
        If valhmsm.Success Then
            angle = 0
            Dim vals As String = ""
            Dim hh As String = ""
            Dim len As Integer = 0
            While valhmsm.Success
                len = valhmsm.Value.Length - 1
                vals = valhmsm.Value.Substring(0, len)
                hh = valhmsm.Value.Remove(0, len)
                Select Case hh
                    Case "d", "°", "度"
                        hms = False
                        angle += Convert.ToDouble(vals)
                    Case "h", "時"
                        hms = True
                        angle += 15 * Convert.ToDouble(vals)
                    Case "m", "分", "′"
                        If hms = True Then
                            angle += Convert.ToDouble(vals) / 4
                        Else
                            angle += Convert.ToDouble(vals) / 60
                        End If
                    Case "s", "秒", "″"
                        If hms = True Then
                            angle += Convert.ToDouble(vals) / 240
                        Else
                            angle += Convert.ToDouble(vals) / 3600
                        End If
                End Select
                valhmsm = valhmsm.NextMatch
            End While
            angle = angle * Math.PI / 180
        ElseIf valmum.Success Then
            Select Case valmum.Value
                Case "grad", "gon", "グラード"
                    angle = angle * 90 / 100
                Case "r", "直角", "∟"
                    angle = angle * 90
            End Select
            If valmum.Value <> "rad" Then
                angle = angle * Math.PI / 180
            End If
        End If
        Return angle
    End Function

    Private Function calcradian(ByVal st As String) As Double
        Dim ss(255) As String
        Dim s As String = st.Replace(" ", "")
        Dim sbk As String = ""
        Dim i As Integer = 0
        Dim sb As New StringBuilder
        Dim f As Double = 0
        While i < s.Length
            If s(i) = "*" Or s(i) = "/" Then
                f = floatcal(sb.ToString, f, sbk)
                sbk = s(i)
                sb.Clear()
            ElseIf s(i) = "," Or s(i) = "^" Then
                Exit While
            Else
                sb.Append(s(i))
            End If
            i += 1
        End While
        Return floatcal(sb.ToString, f, sbk)

    End Function

    Private Function floatcal(ByVal str As String, ByVal f As Double, ByVal sbk As String) As Double
        Dim g As Double
        Dim ff As New Regex("-?\d\.?\d*")
        Dim ffm As Match = ff.Match(str)
        Dim valmu As New Regex("[+-]?(pi|goldenratio|e)")
        Dim valmum As Match = valmu.Match(str)

        If ffm.Success Then
            g = Convert.ToDouble(ffm.Value)
            str = str.Replace(ffm.Value, "")
        ElseIf valmum.Success Then
            If valmum.Value.Contains("pi") Then
                g = Convert.ToSingle(Math.PI)
            ElseIf valmum.Value.Contains("goldenratio") Then
                g = Convert.ToSingle((1 + Math.Sqrt(5)) / 2)
            ElseIf valmum.Value.Contains("e") Then
                g = Convert.ToSingle(Math.E)
            End If
            If valmum.Value.Contains("-") Then
                g = -g
            End If
        End If

        Dim cc As New Regex("[\*/]")
        Dim ccm As Match = cc.Match(sbk)
        If ccm.Success Then
            Select Case ccm.Value
                Case "*"
                    f = f * g
                Case "/"
                    f = f / g
            End Select
        Else
            f = g
        End If

        Return f
    End Function

    Private Function swapperint(ByVal dem As Integer()) As Integer()
        Dim demt As Integer
        demt = dem(1)
        dem(1) = dem(0)
        dem(0) = demt
        Return dem
    End Function

    Private Function cvt_int(ByVal s As String) As Integer
        Dim dem As Integer = 0
        Dim frac As New Regex("-?0x[0-9A-fa-f]{1,8}")
        Dim fracm As Match = frac.Match(s)
        Dim int As New Regex("-?\d{1,10}")
        Dim intm As Match = int.Match(s)
        Dim bin As New Regex("bin[01]{1,32}")
        Dim binm As Match = bin.Match(s)
        Dim oct As New Regex("oct[0-3]?[0-7]{1,10}")
        Dim octm As Match = oct.Match(s)
        Dim vallb As New Regex("s?(upper_|lower_)")
        Dim vallbm As Match = vallb.Match(s)
        Dim minus As Int64 = 0
        If binm.Success Then
            s = binm.Value.Remove(0, 3)
            For i = 0 To s.Length - 1
                dem = dem Or (CInt(s.Substring(s.Length - 1 - i, 1)) << i)
            Next
        ElseIf octm.Success Then
            s = octm.Value.Remove(0, 3)
            For i = 0 To s.Length - 1
                dem = dem Or (CInt(s.Substring(s.Length - 1 - i, 1)) << (3 * i))
            Next
        ElseIf fracm.Success Then
            dem = Convert.ToInt32(fracm.Value, 16)
        ElseIf intm.Success Then
            dem = Convert.ToInt32(intm.Value)
        Else
            Dim k As Integer = 0
            Dim j As Integer = 0
            Dim sl As Integer = 0
            If vallbm.Success Then
                If vallbm.Value.Contains("up") Then
                    sl = 1
                Else
                    sl = 2
                End If
                If vallbm.Value.Contains("s") Then
                    sl += 4
                End If
                s = s.Replace(vallbm.Value, "")
            End If

            For j = 0 To 255
                If label(j) = s Then
                    Exit For
                End If
            Next
            If (j < 255) Then
                k = label_addr(j)
                If MODE.SelectedIndex = 4 Then
                    If k >= &H9800000 Then
                        k -= &H9800000
                    End If
                Else
                    If k < &H1800000 Then
                        k += &H8800000
                    End If
                End If
                If (sl And 1) = 1 Then
                    If (sl And 4) = 4 AndAlso (k And &HFFFF) >= &H8000 Then
                        k = k >> 16
                        k += 1
                    Else
                        k = k >> 16
                    End If
                ElseIf (sl And 2) = 2 Then
                    k = k And &HFFFF
                End If
                dem = k
            Else
                MessageBox.Show(s & "に該当するラベルがみつかりませんでした", "")
            End If
        End If

        Return dem
    End Function

    Private Function overflow(ByVal dem As Integer(), ByVal s As String) As Integer()
        Dim i As Long = CLng(dem(0))
        Dim k As Long = CLng(dem(1))
        Dim mask As Long = 4294967295
        Select Case s
            Case "+"
                k = k + i
            Case "-"
                k = k - i
            Case "*"
                k = k * i
            Case "/"
                k = CLng(k / i)
        End Select
        k = k And mask
        If k > 2147483647 Then
            k = k - 4294967296
        End If
        dem(1) = Convert.ToInt32(k)
        Return dem
    End Function

    Private Function rpnint(ByVal str As String) As Integer
        Dim ss As String() = str.ToLower.Split(CChar(","))
        Dim len As Integer = ss.Length - 1
        Dim dem(len) As Integer
        Dim k As Long = 0
        Dim tr As Boolean = False
        For i = 0 To len
            Select Case ss(i).Trim
                Case "chs", "+/-"
                    dem(0) = -dem(0)
                Case "abs", "|x|"
                    If dem(0) = -2147483648 Then
                        dem(0) = 0
                    End If
                    If (dem(0) < 0) Then
                        dem(0) = dem(0) - dem(0) - dem(0)
                    End If
                Case "drop"
                    Array.Copy(dem, 1, dem, 0, len)
                Case "swap"
                    dem = swapperint(dem)
                Case "="
                Case "+", "-", "*", "/"
                    dem = overflow(dem, ss(i))
                    Array.Copy(dem, 1, dem, 0, len)
                Case ">>", "sra"
                    If dem(0) >= 32 Or dem(0) < 0 Then
                        MessageBox.Show("シフトさせる範囲は1～31でなくてはなりません")
                        Return 0
                    Else
                        dem(1) = dem(1) >> dem(0)
                    End If
                    Array.Copy(dem, 1, dem, 0, len)
                Case ">>>", "srl" '論理シフト
                    tr = False
                    If dem(0) >= 32 Or dem(0) < 1 Then
                        MessageBox.Show("シフトさせる範囲は1～31でなくてはなりません")
                        Return 0
                    Else
                        If (dem(1) And &H80000000) <> 0 Then
                            dem(1) = dem(1) And &H7FFFFFFF
                            tr = True
                        End If
                        dem(1) = dem(1) >> dem(0)
                        If tr = True Then
                            dem(1) = dem(1) Or ((1 << (31 - dem(0))))
                        End If
                    End If
                    Array.Copy(dem, 1, dem, 0, len)
                Case "<<", "sll"
                    If dem(0) >= 32 Or dem(0) < 1 Then
                        MessageBox.Show("シフトさせる範囲は1～31でなくてはなりません")
                        Return 0
                    Else
                        dem(1) = dem(1) << dem(0)
                    End If
                    Array.Copy(dem, 1, dem, 0, len)
                Case "ror"
                    'ror(0x87654321,16)
                    If dem(0) >= 32 Or dem(0) < 1 Then
                        MessageBox.Show("シフトさせる範囲は1～31でなくてはなりません")
                        Return 0
                    Else
                        Dim msk As Integer = (&HFFFFFFFF << dem(0))
                        Dim tmp As Integer = (dem(1)) And Not msk
                        dem(1) = (dem(1) >> dem(0)) And Not (&HFFFFFFFF << (32 - dem(0)))
                        dem(1) = (dem(1) Or (tmp << (32 - dem(0))))
                    End If
                    Array.Copy(dem, 1, dem, 0, len)
                Case "rol"
                    'rol(0x87654321,16)
                    If dem(0) >= 32 Or dem(0) < 1 Then
                        MessageBox.Show("シフトさせる範囲は1～31でなくてはなりません")
                        Return 0
                    Else
                        Dim msk As Integer = (&HFFFFFFFF << dem(0))
                        Dim tmp As Integer = (dem(1) >> (32 - dem(0))) And (Not msk)
                        dem(1) = (dem(1) << dem(0))
                        dem(1) = (dem(1) Or tmp)
                    End If
                    Array.Copy(dem, 1, dem, 0, len)
                Case "\", "mod"
                    dem(1) = dem(1) Mod dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "&", "and"
                    dem(1) = dem(1) And dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "|", "or"
                    dem(1) = dem(1) Or dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "^", "xor"
                    dem(1) = dem(1) Xor dem(0)
                    Array.Copy(dem, 1, dem, 0, len)
                Case "~", "not"
                    dem(0) = dem(0) Xor &HFFFFFFFF
                Case Else
                    If (ss(i)).Trim <> "" Then
                        Array.Copy(dem, 0, dem, 1, len)
                        dem(0) = cvt_int((ss(i)))
                    End If
            End Select
        Next

        Return dem(0)
    End Function

    Function valint(ByVal str As String) As Integer

        Dim i As Integer = 0
        Dim k As Integer = 0
        If RPN.Checked Then
            If CVTRPN.Checked Then
                Dim p As New Polish
                str = p.Main(str.Replace("~", "not"), LOOKSORDER.Checked, False, True)
            End If
            k = rpnint(str)
        Else
            k = valword(str)
        End If
        Return k

    End Function

    Function reg_sel(ByVal s As String) As Integer
        Dim ss As String() = {"zr", "at", "v0", "v1", "a0", "a1", "a2", "a3", "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "t8", "t9", "k0", "k1", "gp", "sp", "fp", "ra"}
        Dim i As Integer
        If s = "zero" Then
            i = 0
        ElseIf s = "s8" Then
            i = 30
        Else
            For i = 0 To 31
                If ss(i) = s Then
                    Exit For
                End If
            Next
        End If
        Return i
    End Function

    Function valdec_boolean_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim valdec As New Regex("\d{1,2}$")
        Dim valdecm As Match = valdec.Match(str.Trim)
        If valdecm.Success Then
            hex = hex Or (CInt(valdecm.Value) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function valdec_ext_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim valdec As New Regex("\d{1,2}$")
        Dim valdecm As Match = valdec.Match(str.Trim)
        If valdecm.Success Then
            hex = hex Or ((CInt(valdecm.Value) - 1) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function valdec_ins_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim valdec As New Regex("-?\d{1,2}$")
        Dim valdecm As Match = valdec.Match(str.Trim)
        If valdecm.Success Then
            hex = hex Or ((CInt(valdecm.Value) + (CInt(hex >> 6) And &H1F) - 1) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function reg_boolean_lbsb(ByVal str As String) As String
        Dim ss As String() = str.ToLower.Split(CChar("("))
        Return ss(1)
    End Function

    Function reg_boolean_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer

        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        If regm.Success Then
            hex = hex Or ((reg_sel(regm.Value) << 21) >> (5 * k))
        End If
        Return hex
    End Function

    Function reg_boolean(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 21) >> (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function

    Function reg_boolean2(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 16) << (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function

    Function reg_boolean3(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 11) << (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function

    Function reg_boolean4(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-3]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 6) << (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function


    Dim mscp As Integer() = {0, 37, 437, 500, 708, 709, 710, 720, 737, 775, 850, 852, 855, 857, 858, 860, 861, 862, 863, 864, 865, 866, 869, 870, 874, 875, 932, 936, 949, 950, 1026, 1047, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149, 1200, 1201, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1361, 10000, 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10008, 10010, 10017, 10021, 10029, 10079, 10081, 10082, 12000, 12001, 20000, 20001, 20002, 20003, 20004, 20005, 20105, 20106, 20107, 20108, 20127, 20261, 20269, 20273, 20277, 20278, 20280, 20284, 20285, 20290, 20297, 20420, 20423, 20424, 20833, 20838, 20866, 20871, 20880, 20905, 20924, 20932, 20936, 20949, 21025, 21027, 21866, 28591, 28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 28603, 28605, 29001, 38598, 50220, 50221, 50222, 50225, 50227, 50229, 50930, 50931, 50933, 50935, 50936, 50937, 50939, 51932, 51936, 51949, 51950, 52936, 54936, 57002, 57003, 57004, 57005, 57006, 57007, 57008, 57009, 57010, 57011, 65000, 65001}

    Function checkcp(ByVal cp As Integer) As Boolean
        Dim len As Integer = mscp.Length - 1
        For i = 0 To len
            If cp = mscp(i) Then
                Return True
            End If
        Next
        Return False

    End Function

    Function getstring(ByVal str As String) As Byte()
        Dim st As Regex = New Regex(""".*?""$")
        Dim stm As Match = st.Match(str)
        Dim stt As String = ""
        Dim valdec As New Regex("string\d{1,10}")
        Dim valdecm As Match = valdec.Match(str)
        Dim enc As Int32 = 0
        Dim b As Byte() = Nothing
        Dim nulls(1) As Byte
        If valdecm.Success Then
            enc = Convert.ToInt32(valdecm.Value.Remove(0, 6))
        End If
        If stm.Success Then
            stt = stm.Value.Substring(1, stm.Length - 2)
        Else
            Return nulls
        End If

        If checkcp(enc) = True Then
            b = Encoding.GetEncoding(enc).GetBytes(stt)
        Else
            MessageBox.Show("CP" & enc.ToString & "は対応してないコードページです。NULLで処理します", "エラー")
            Return nulls
        End If

        Return b
    End Function

#End Region

    Dim label(255) As String
    Dim label_addr(255) As Integer
    Dim normalize As String

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles cvt_asm2code.Click
        Dim sa As New StringBuilder
        Dim st As Integer = Convert.ToInt32(ADDR.Text, 16)
        If MODE.SelectedIndex >= 6 Then
            Dim jrra As New Regex("[ 　\t]?jr[ 　\t]+ra")
            Dim jrm As Match = jrra.Match(ASM.Text)
            If jrm.Success = False Then
                MessageBox.Show("サブルーチンコードにはja ra が必ず必要です。")
                Exit Sub
            End If
        End If

        If MODE.SelectedIndex = 5 Then
            cvtget(st, (ASM.Text & vbCrLf), 1)
            Dim ss As String() = Regex.Split(normalize, "setpc")
            sa.Append(cvtget(st, ss(0), 2))
            For i = 1 To ss.Length - 1
                Dim sts As String = ss(i).Substring(0, ss(i).IndexOf(vbLf)).Trim
                st = offset_boolean(" " & sts, 0) << 2
                sa.Append(cvtget(st, "//" & ss(i), 4))
            Next
        Else
            sa.Append(cvtget(st, (ASM.Text & vbCrLf), 0))
        End If

        CODE.Text = sa.ToString
    End Sub

    Private Function cvtget(ByVal st As Integer, ByVal asms As String, ByVal selm As Integer) As String
        Dim ss As String() = (asms).Split(CChar(vbLf))
        Dim sa As New StringBuilder
        Dim sb As New StringBuilder
        Dim sc As New StringBuilder
        Dim i As Integer = st
        Dim modesel As Integer = MODE.SelectedIndex
        Dim cmfaddrval(1) As Int64
        Dim cmfst As String = ""
        If MODE.SelectedIndex < 2 Then
            If st >= &H8800000 Then
                i -= &H8800000
            End If
            If i >= &H8800000 Then
                i -= &H8800000
            End If
        ElseIf modesel = 4 Then
            If st >= &H9800000 Then
                st -= &H9800000
            End If
            If i >= &H9800000 Then
                i -= &H9800000
            End If
        Else
            If st < &H1800000 Then
                st += &H8800000
            End If
            If i < &H1800000 Then
                i += &H8800000
            End If
        End If

        Dim ct As Integer = 0
        Dim ii As Integer = i
        Dim setpc As Integer = 0
        Dim odd As Boolean = False
        Dim odd2 As Boolean = False
        Dim macros As Boolean = False
        Dim macrost(255) As String
        Dim macrobase(255) As String
        Dim macroct As Integer = 0
        If selm < 2 Then

            Dim psdis As New Regex("(\t| |　)?(#|;|//).+$")
            Dim llb As New Regex("^.*?:( |\t|　)+")
            Dim shead As New Regex("^[a-z0-9\._\-]+(\x20|\t)+")
            Dim sheadm As Match
            Dim psdism As Match
            Dim llbm As Match
            Dim rg As String = ""
            Dim lst As String = ""
            Dim head As String = ""
            Dim k As Integer = 0
            For Each s As String In ss
                psdism = psdis.Match(s.Trim)
                If psdism.Success Then
                    s = s.Trim().Substring(0, psdism.Index)
                End If
                llbm = llb.Match(s.Trim)
                If llbm.Success Then
                    sc.AppendLine(llbm.Value.Trim)
                    s = s.Trim().Remove(0, llbm.Length)
                End If
                sheadm = shead.Match(s.Trim)
                s = s.Trim
                If macros = True AndAlso s.Length > 5 AndAlso s.Remove(0, 5) <> ".endm" Then
                    sa.AppendLine(s)
                ElseIf s.Length > 5 AndAlso s.Substring(0, 6) = ".macro" Then
                    sa.Clear()
                    macros = True
                    s = s.Remove(0, 6).Trim
                    s = s.Replace(vbTab, " ")
                    macrost(macroct) = s
                ElseIf s.Length > 4 AndAlso s.Substring(0, 5) = ".endm" Then
                    macros = False
                    macrobase(macroct) = sa.ToString
                    macroct += 1
                ElseIf sheadm.Success Then
                    head = sheadm.Value.Trim.ToLower
                    Select Case head
                        Case "la"
                            lst = s.Remove(0, s.LastIndexOf(",") + 1).ToLower
                            rg = s.Substring(0, s.IndexOf(",")).Remove(0, 2).Trim
                            sc.Append("lui ")
                            sc.Append(rg)
                            sc.Append(",upper_")
                            sc.AppendLine(lst)
                            sc.Append("ori ")
                            sc.Append(rg)
                            sc.Append(",")
                            sc.Append(rg)
                            sc.Append(",lower_")
                            sc.AppendLine(lst)
                        Case "li", "mfloat"
                            If head = "li" Then
                                k = valword(s.Trim)
                                rg = s.Substring(0, s.IndexOf(",")).Remove(0, 2).Trim
                            Else
                                lst = s.Remove(0, s.LastIndexOf(",") + 1).ToLower
                                rg = s.Substring(0, s.IndexOf(",")).Remove(0, 6).Trim
                                k = cvt_float(valfloat(lst))
                            End If

                            If (k >> 16 <> 0) Then
                                sc.Append("lui ")
                                sc.Append(rg)
                                sc.Append(",0x")
                                sc.AppendLine((k >> 16).ToString("X4"))
                            End If
                            If ((k And &HFFFF) <> 0) Then
                                sc.Append("ori ")
                                sc.Append(rg)
                                sc.Append(",")
                                If (k >> 16 <> 0) Then
                                    sc.Append(rg)
                                Else
                                    sc.Append("zr")
                                End If
                                sc.Append(",0x")
                                sc.AppendLine((k And &HFFFF).ToString("X4"))
                            End If
                            If k = 0 Then
                                sc.AppendLine(s.Trim)
                            End If
                        Case ".ascii"
                            rg = s.Remove(0, 6).Trim
                            sc.Append(".string20127 ")
                            sc.AppendLine(rg)
                        Case ".shift_jis"
                            rg = s.Remove(0, 10).Trim
                            sc.Append(".string932 ")
                            sc.AppendLine(rg)
                        Case ".euc-jp"
                            rg = s.Remove(0, 7).Trim
                            sc.Append(".string51932 ")
                            sc.AppendLine(rg)
                        Case ".utf-16be"
                            rg = s.Remove(0, 8).Trim
                            sc.Append(".string1201 ")
                            sc.AppendLine(rg)
                        Case ".utf-16le"
                            rg = s.Remove(0, 8).Trim
                            sc.Append(".string1200 ")
                            sc.AppendLine(rg)
                        Case ".utf-8"
                            rg = s.Remove(0, 5).Trim
                            sc.Append(".string65001 ")
                            sc.AppendLine(rg)
                        Case ".uhc"
                            rg = s.Remove(0, 4).Trim
                            sc.Append(".string949 ")
                            sc.AppendLine(rg)
                        Case ".euc-kr"
                            rg = s.Remove(0, 7).Trim
                            sc.Append(".string51949 ")
                            sc.AppendLine(rg)
                        Case ".big5"
                            rg = s.Remove(0, 4).Trim
                            sc.Append(".string950 ")
                            sc.AppendLine(rg)
                            'Case ".big5-hkscs"
                            '    rg = s.Remove(0, 10).Trim
                            '    sc.Append(".string951 ")
                            '    sc.AppendLine(rg)
                        Case ".gbk"
                            rg = s.Remove(0, 4).Trim
                            sc.Append(".string936 ")
                            sc.AppendLine(rg)
                        Case ".gb18030"
                            rg = s.Remove(0, 8).Trim
                            sc.Append(".string54936 ")
                            sc.AppendLine(rg)
                        Case "ulv.q"
                            rg = s.Remove(0, 5).Trim
                            sc.Append("lvl.q ")
                            sc.Append(rg)
                            sc.AppendLine(",[+12]")
                            sc.Append("lvr.q ")
                            sc.AppendLine(rg)
                        Case "usv.q"
                            rg = s.Remove(0, 5).Trim
                            sc.Append("svl.q ")
                            sc.Append(rg)
                            sc.AppendLine(",[+12]")
                            sc.Append("svr.q ")
                            sc.AppendLine(rg)
                        Case "ulw"
                            rg = s.Remove(0, 3).Trim
                            sc.Append("lwl ")
                            sc.Append(rg)
                            sc.AppendLine(",[+3]")
                            sc.Append("lwr ")
                            sc.AppendLine(rg)
                        Case "usw"
                            rg = s.Remove(0, 3).Trim
                            sc.Append("swl ")
                            sc.Append(rg)
                            sc.AppendLine(",[+3]")
                            sc.Append("swr ")
                            sc.AppendLine(rg)
                        Case "mul"
                            Dim sk As String() = s.Split(CChar(","))
                            If sk.Length > 2 Then
                                lst = s.Remove(0, s.IndexOf(",") + 1).Trim
                                rg = s.Remove(0, 4).Trim
                                rg = rg.Substring(0, rg.IndexOf(",")).Trim
                                sc.Append("mult ")
                                sc.AppendLine(lst)
                                sc.Append("mflo ")
                                sc.AppendLine(rg)
                            Else
                                sc.AppendLine(s.Trim)
                            End If
                        Case "mulu"
                            Dim sk As String() = s.Split(CChar(","))
                            If sk.Length > 2 Then
                                lst = s.Remove(0, s.IndexOf(",") + 1).Trim
                                rg = s.Remove(0, 4).Trim
                                rg = rg.Substring(0, rg.IndexOf(",")).Trim
                                sc.Append("multu ")
                                sc.AppendLine(lst)
                                sc.Append("mflo ")
                                sc.AppendLine(rg)
                            Else
                                sc.AppendLine(s.Trim)
                            End If
                        Case "bgt", "blt", "bgtu", "bltu", "bgti", "blti", "bgtiu", "bltiu" _
                            , "bgtl", "bltl", "bgtul", "bltul", "bgtil", "bltil", "bgtiul", "bltiul" _
                            , "bge", "ble", "bgeu", "bleu", "bgei", "blei", "bgeiu", "bleiu" _
                            , "bgel", "blel", "bgeul", "bleul", "bgeil", "bleil", "bgeiul", "bleiul"
                            rg = s.Remove(0, sheadm.Length).Trim

                            Dim sk As String() = rg.Split(CChar(","))
                            If sk.Length > 2 Then
                                If head.Contains("iu") Then
                                    sc.Append("sltiu at,")
                                ElseIf head.Contains("i") Then
                                    sc.Append("slti at,")
                                ElseIf head.Contains("u") Then
                                    sc.Append("sltu at,")
                                Else
                                    sc.Append("slt at,")
                                End If
                                If head.Contains("i") AndAlso (head.Contains("gt") Or head.Contains("le")) Then
                                    sc.Append(sk(0))
                                    sc.Append(",0x")
                                    Dim tmp As Integer = valhex_boolean("," & sk(1), 0)
                                    If head.Contains("gt") Then
                                        tmp += 1
                                    Else
                                        tmp -= 1
                                    End If
                                    tmp = tmp And &HFFFF
                                    sc.AppendLine(tmp.ToString("X"))
                                ElseIf head.Contains("gt") Or head.Contains("le") Then
                                    sc.Append(sk(1))
                                    sc.Append(",")
                                    sc.AppendLine(sk(0))
                                Else
                                    sc.Append(sk(0))
                                    sc.Append(",")
                                    sc.AppendLine(sk(1))
                                End If
                                If head.Contains("i") AndAlso (head.Contains("gt") Or head.Contains("le")) Then
                                    If head.Contains("gt") Then
                                        sc.Append("beq")
                                    Else
                                        sc.Append("bne")
                                    End If
                                ElseIf head.Contains("gt") Or head.Contains("lt") Then
                                    sc.Append("bne")
                                Else
                                    sc.Append("beq")
                                End If
                                If head(head.Length - 1) = "l" Then
                                    sc.Append("l")
                                End If
                                sc.Append(" at,zr,")
                                sc.AppendLine(sk(2))
                            Else
                                sc.AppendLine(s.Trim)
                            End If
                            '                                                           if(R[rs]>R[rt])
                            'bgt $rs,$rt,Label 	slt $at,$rt,$rs; bne $at,$zero,Label 　 if(R[rt]<R[rs]) PC=Label,(R[rt]>=R[rs])PC+4
                            'blt $rs,$rt,Label 	slt $at,$rs,$rt; bne $at,$zero,Label 	if(R[rs]<R[rt]) PC=Label,(R[rs]>=R[rt])PC+4
                            'bge $rs,$rt,Label 	slt $at,$rs,$rt; beq $at,$zero,Label 　	if(R[rs]>=R[rt]) PC=Label,(R[rs]<R[rt])PC+4
                            'ble $rs,$rt,Label 	slt $at,$rt,$rs; beq $at,$zero,Label    if(R[rt]>=R[rs]) PC=Label,(R[rt]<R[rs])PC+4
                            '                                                               rs<=rt
                        Case Else
                            If head <> "" AndAlso head(0) = "v" AndAlso head <> "vpfxd" AndAlso head <> "vpfxs" AndAlso head <> "vpfxt" Then
                                Dim prifix As New Regex("\[.*?\]")
                                Dim prifixm As Match = prifix.Match(s)
                                Dim parg(3) As String
                                Dim v As Integer = 0
                                While prifixm.Success
                                    parg(v) = prifixm.Value
                                    s = s.Replace(prifixm.Value, "!" & v.ToString)
                                    v += 1
                                    prifixm = prifixm.NextMatch
                                End While
                                If v = 0 Then
                                    sc.AppendLine(s.Trim)
                                Else
                                    Dim arg As String() = s.Split(CChar(","))
                                    For k = 0 To decoder.Length - 4
                                        If head = decoder(k) Then
                                            Exit For
                                        End If
                                        k += 3
                                    Next
                                    Dim arg2 As String() = decoder(k + 3).Split(CChar(","))
                                    For k = 0 To arg2.Length - 1
                                        Select Case arg2(k)
                                            Case "%zs", "%zp", "zt", "zq"
                                                If arg(k).Contains("!") Then
                                                    sc.Append("vpfxd ")
                                                    sc.AppendLine(parg(CInt(arg(k).Remove(0, arg(k).Length - 1))))
                                                End If
                                            Case "%ys", "%yp", "%yt", "%yq"
                                                If arg(k).Contains("!") Then
                                                    sc.Append("vpfxs ")
                                                    sc.AppendLine(parg(CInt(arg(k).Remove(0, arg(k).Length - 1))))
                                                End If
                                            Case "%xs", "%xp", "%xt", "%xq"
                                                If arg(k).Contains("!") Then
                                                    sc.Append("vpfxt ")
                                                    sc.AppendLine(parg(CInt(arg(k).Remove(0, arg(k).Length - 1))))
                                                End If
                                        End Select
                                    Next
                                    sc.AppendLine(s.Trim)
                                End If

                            Else
                                sc.AppendLine(s.Trim)
                            End If
                    End Select
                Else
                    sc.AppendLine(s.Trim)
                End If
            Next

            Dim sss As String = sc.ToString

            ss = sss.Split(CChar(vbLf))
            normalize = sc.ToString
            Array.Clear(label, 0, 256)
            Array.Clear(label_addr, 0, 256)
            macros = False

            For Each s As String In ss
                If s.Trim = "" Then
                ElseIf macros = True Then
                ElseIf s.Length > 6 AndAlso s.Substring(0, 6) = ".macro" Then
                    macros = True
                ElseIf s.Length > 5 AndAlso s.Substring(0, 5) = ".endm" Then
                    macros = False
                ElseIf s.Length > 0 AndAlso (s(0) = "_" Or s(0) = "/" Or s(0) = "#" Or s(0) = ";") Then
                ElseIf s.Length > 2 AndAlso (s.Substring(0, 3) = "FNC") Then
                ElseIf s.Length > 2 AndAlso s.Substring(s.Length - 2, 1) = ":" Then
                    label(ct) = s.Remove(s.Length - 2, 2).ToLower.Trim
                    label_addr(ct) = ii
                    ct += 1
                ElseIf s.Length > 2 AndAlso s.Contains(":") Then
                    label(ct) = s.Substring(0, s.IndexOf(":")).ToLower.Trim
                    label_addr(ct) = ii
                    ct += 1
                    ii += 4
                ElseIf s.Length > 4 AndAlso s.Substring(0, 5) = "label" Then
                    label(ct) = s.Remove(0, 5).ToLower.Trim
                    label_addr(ct) = ii
                    ct += 1
                ElseIf s.Length > 7 AndAlso s.Substring(0, 7) = ".string" Then
                    Dim b As Byte() = getstring(s.Trim)
                    ii += b.Length + 1
                    If (ii And 3) <> 0 Then
                        ii = (ii And &H9FFFFFC) + 4
                    End If
                ElseIf s.Length > 5 AndAlso s.Substring(0, 5) = "setpc" Then
                    setpc = offset_boolean(s.Trim, 0) << 2
                    If setpc <> 0 Then
                        If MODE.SelectedIndex < 2 Then
                            If setpc >= &H8800000 Then
                                setpc -= &H8800000
                            End If
                        ElseIf MODE.SelectedIndex = 4 Then
                            If setpc >= &H9800000 Then
                                setpc -= &H9800000
                            End If
                        ElseIf (MODE.SelectedIndex = 6 AndAlso setpc <> tmp) Or (MODE.SelectedIndex > 6 AndAlso setpc <> cmf) Then
                            MessageBox.Show("TEMP/CMFサブルーチンは開始アドレスがカーネルメモリ固定のためSETPCは使えません。")
                            Return ""
                        End If
                        ii = setpc
                    End If
                Else
                    ii += 4
                End If

            Next
        End If

        If (selm And 1) = 0 Then

            odd = False
            macros = False
            For Each s As String In ss
                If s.Trim = "" Then
                ElseIf macros = True Then
                ElseIf s.Length > 0 AndAlso (s(0) = "_" Or s(0) = "/" Or s(0) = "#" Or s(0) = ";") Then
                ElseIf s.Length > 2 AndAlso (s.Substring(0, 3) = "FNC") Then
                ElseIf s.Length > 2 AndAlso s.Substring(s.Length - 2, 1) = ":" Then
                ElseIf s.Length > 6 AndAlso s.Substring(0, 6) = ".macro" Then
                    macros = True
                ElseIf s.Length > 5 AndAlso s.Substring(0, 5) = ".endm" Then
                    macros = False
                ElseIf s.Length > 4 AndAlso s.Substring(0, 5) = "label" Then
                ElseIf s.Length > 5 AndAlso s.Substring(0, 5) = "setpc" Then
                    setpc = offset_boolean(s.Trim, 0) << 2
                    If setpc <> 0 Then
                        If MODE.SelectedIndex < 2 Then
                            If setpc >= &H8800000 Then
                                setpc -= &H8800000
                            End If
                        ElseIf MODE.SelectedIndex = 4 Then
                            If setpc >= &H9800000 Then
                                setpc -= &H9800000
                            End If
                        End If
                        i = setpc
                    End If
                ElseIf s.Length > 7 AndAlso s.Substring(0, 7) = ".string" Then
                    Dim b As Byte() = getstring(s.Trim)
                    Dim j As Integer = b.Length + 1
                    Dim k As Integer = 0
                    If (j And 3) <> 0 Then
                        j = (j And &HFFFC) + 4
                    End If
                    Array.Resize(b, j)
                    odd2 = False
                    While (k < j)
                        If modesel < 5 Then
                            Select Case modesel
                                Case 0
                                    sb.Append("0x")
                                    sb.Append(Convert.ToString(i + k, 16).ToUpper.PadLeft(8, "0"c))
                                Case 1
                                    sb.Append("_L 0x")
                                    sb.Append(Convert.ToString((i + k) Or &H20000000, 16).ToUpper.PadLeft(8, "0"c))
                                Case 2
                                    sb.Append("_M 0x")
                                    sb.Append(Convert.ToString((i + k) And &HFFFFFFF, 16).ToUpper.PadLeft(8, "0"c))
                                Case 3
                                    sb.Append("_NWR 0x80000000 0x")
                                    sb.Append(Convert.ToString((i + k) And &HFFFFFFF, 16).ToUpper.PadLeft(8, "0"c))
                                Case 4
                                    sb.Append("_L ")
                                    sb.Append(Convert.ToString(((i + k) And &HFFFFFFF) Or &H80000000, 16).ToUpper.PadLeft(8, "0"c))
                            End Select
                            If modesel < 4 Then
                                sb.Append(" 0x")
                                sb.AppendLine(BitConverter.ToInt32(b, k).ToString("X8"))
                            Else
                                sb.Append(" ")
                                sb.AppendLine((BitConverter.ToInt32(b, k) And &HFFFF).ToString("X4"))
                                sb.Append("_L ")
                                sb.Append(Convert.ToString(((i + k + 2) And &HFFFFFFF) Or &H80000000, 16).ToUpper.PadLeft(8, "0"c))
                                sb.Append(" ")
                                sb.AppendLine(((BitConverter.ToInt32(b, k) >> 16) And &HFFFF).ToString("X4"))
                            End If
                        Else
                            Select Case modesel
                                Case 5
                                    If odd = False Then
                                        sb.Append("_M 0x")
                                        sb.Append(BitConverter.ToInt32(b, k).ToString("X8"))
                                        sb.Append(" 0x")
                                        odd2 = True
                                    Else
                                        sb.AppendLine(BitConverter.ToInt32(b, k).ToString("X8"))
                                        odd2 = False
                                    End If
                                Case 6
                                    If odd2 = False Then
                                        sb.Append("_N 0x")
                                        sb.Append(BitConverter.ToInt32(b, k).ToString("X8"))
                                        sb.Append(" 0x")
                                        odd = True
                                    Else
                                        sb.AppendLine(BitConverter.ToInt32(b, k).ToString("X8"))
                                        odd2 = False
                                    End If
                                Case 7
                                    If odd2 = False Then
                                        sb.Append("_L 0x")
                                        cmfst = BitConverter.ToInt32(b, k).ToString("X8")
                                        If modesel > 7 Then
                                            cmfaddrval(0) = Convert.ToInt64(cmfst, 16)
                                        Else
                                            sb.Append(cmfst)
                                            sb.Append(" 0x")
                                        End If
                                        odd2 = True
                                    Else
                                        cmfst = BitConverter.ToInt32(b, k).ToString("X8")
                                        If modesel > 7 Then
                                            cmfaddrval(1) = Convert.ToInt64(cmfst, 16)
                                            cmfaddrval = EncryptCB(cmfaddrval)
                                            If (modesel = 9) Then
                                                cmfaddrval = SwapFF(cmfaddrval)
                                                cmfaddrval = EncryptCB(cmfaddrval)
                                            End If
                                            sb.Append(cmfaddrval(0).ToString("X8"))
                                            sb.Append(" 0x")
                                            sb.AppendLine(cmfaddrval(1).ToString("X8"))
                                        Else
                                            sb.AppendLine(cmfst)
                                        End If
                                        odd2 = False
                                    End If
                            End Select
                        End If
                        k += 4
                    End While
                    i += j
                Else
                    Select Case modesel
                        Case 0
                            'If MODE.Text = "NITEPR" Then
                            sb.Append("0x")
                            sb.Append(Convert.ToString(i, 16).ToUpper.PadLeft(8, "0"c))
                            sb.Append(" ")
                            sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                            i += 4
                            '            End If
                        Case 1
                            'If MODE.Text = "CWCHEAT" Then
                            sb.Append("_L ")
                            sb.Append("0x")
                            sb.Append(Convert.ToString(i Or &H20000000, 16).ToUpper.PadLeft(8, "0"c))
                            sb.Append(" ")
                            sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                            i += 4
                            'End If
                        Case 2
                            'If MODE.Text = "PSPAR" Then
                            sb.Append("_M ")
                            sb.Append("0x")
                            sb.Append(Convert.ToString(i And &HFFFFFFF, 16).ToUpper.PadLeft(8, "0"c))
                            sb.Append(" ")
                            sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                            i += 4
                            'End If
                        Case 3
                            'If MODE.Text = "PMETAN" Then
                            sb.Append("_NWR ")
                            sb.Append("0x80000000 0x")
                            sb.Append(Convert.ToString(i And &HFFFFFFF, 16).ToUpper.PadLeft(8, "0"c))
                            sb.Append(" ")
                            sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                            i += 4
                            'End If
                        Case 4
                            'If MODE.Text = "CWCPOPS" Then
                            setpc = Convert.ToInt32(assembler(s.Trim.ToLower, Convert.ToString(i, 16)), 16)
                            sb.Append("_L ")
                            sb.Append(Convert.ToString((i And &HFFFFFFF) Or &H80000000, 16).ToUpper.PadLeft(8, "0"c))
                            sb.Append(" ")
                            sb.AppendLine((setpc And &HFFFF).ToString("X4"))
                            sb.Append("_L ")
                            sb.Append(Convert.ToString(((i + 2) And &HFFFFFFF) Or &H80000000, 16).ToUpper.PadLeft(8, "0"c))
                            sb.Append(" ")
                            sb.AppendLine(((setpc >> 16) And &HFFFF).ToString("X4"))
                            i += 4
                            'End If
                        Case 5
                            'If MODE.Text = "PSPAR(0xE)" Then
                            If odd = False Then
                                sb.Append("_M ")
                                sb.Append(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                                sb.Append(" ")
                                odd = True
                            Else
                                sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                                odd = False
                            End If
                            i += 4
                            'End If
                        Case 6
                            'If MODE.Text = "TEMPAR(0xC2)" Then
                            If odd = False Then
                                sb.Append("_N ")
                                sb.Append(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                                sb.Append(" ")
                                odd = True
                            Else
                                sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                                odd = False
                            End If
                            i += 4
                            'End If
                        Case Else
                            'If MODE.Text = "CMFUSION(0xF0)" Then
                            If odd = False Then
                                sb.Append("_L ")
                                cmfst = assembler(s.Trim.ToLower, Convert.ToString(i, 16))
                                If modesel > 7 Then
                                    cmfaddrval(0) = Convert.ToInt64(cmfst, 16)
                                Else
                                    sb.Append(cmfst)
                                    sb.Append(" ")
                                End If
                                odd = True
                            Else
                                cmfst = assembler(s.Trim.ToLower, Convert.ToString(i, 16))
                                If modesel > 7 Then
                                    cmfaddrval(1) = Convert.ToInt64(cmfst, 16)
                                    cmfaddrval = EncryptCB(cmfaddrval)
                                    If (modesel = 9) Then
                                        cmfaddrval = SwapFF(cmfaddrval)
                                        cmfaddrval = EncryptCB(cmfaddrval)
                                    End If
                                    sb.Append("0x")
                                    sb.Append(cmfaddrval(0).ToString("X8"))
                                    sb.Append(" 0x")
                                    sb.AppendLine(cmfaddrval(1).ToString("X8"))
                                Else
                                    sb.AppendLine(cmfst)
                                End If
                                odd = False
                            End If
                            i += 4
                    End Select
                End If
            Next
            If odd = True Then
                If modesel > 7 Then
                    cmfaddrval(1) = 0
                    cmfaddrval = EncryptCB(cmfaddrval)
                    If (modesel = 9) Then
                        cmfaddrval = SwapFF(cmfaddrval)
                        cmfaddrval = EncryptCB(cmfaddrval)
                    End If
                    sb.Append("0x")
                    sb.Append(cmfaddrval(0).ToString("X8"))
                    sb.Append(" 0x")
                    sb.AppendLine(cmfaddrval(1).ToString("X8"))
                Else
                    sb.AppendLine("0x00000000")
                End If
            End If
            i = i - st
            If MODE.Text = "PSPAR(0xE)" Then
                sb.Insert(0, "_M 0xE" & (st And &HFFFFFFF).ToString("X").ToUpper.PadLeft(7, "0"c) & " 0x0000" & (i And &HFFFF).ToString("X4") & vbCrLf)
            ElseIf MODE.Text = "TEMPAR(0xC2)" Then
                sb.Insert(0, "_N 0xC2000000 0x00000" & (i And &H3FF).ToString("X3") & vbCrLf)
            ElseIf MODE.Text.Contains("CMFUSION") Then
                i = i * 2
                If odd = True Then
                    i += 8
                End If
                sb.Insert(0, "_L 0xF00000" & ((i And 255) >> 4).ToString("X2") & " 0x000000" & (MODE.SelectedIndex - 7).ToString("X2") & vbCrLf)
            End If
        End If
        Return sb.ToString
    End Function

    Private Sub only_hexdicimal(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ADDR.KeyPress
        Dim hex As New Regex("[^0-9A-Fa-fx\x08]")
        Dim hexm As Match = hex.Match(e.KeyChar)
        If hexm.Success Then
            e.Handled = True
            Beep()
        ElseIf e.KeyChar <> "x" Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If
    End Sub

    Private Sub MODE_SelectedIndexChanged(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles MODE.KeyPress
        e.Handled = True
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ASM.KeyDown, CODE.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
        End If
    End Sub

    Private Sub versioncheck(sender As System.Object, e As System.EventArgs) Handles バージョン.Click
        Dim v As New version
        v.ShowDialog()
        v.Close()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles enc.Click
        Dim f As New codepage
        f.ShowDialog(Me)
        f.Dispose()
    End Sub

    Private Sub SAVECODE(sender As System.Object, e As System.EventArgs) Handles save2.Click, コード保存ToolStripMenuItem.Click
        Dim f As New SaveFileDialog
        f.InitialDirectory = Application.StartupPath
        f.Filter = "テキストファイル（*.txt)|*.txt"
        f.Title = "コードの保存"
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim sr As New StreamWriter(f.FileName, False, Encoding.GetEncoding(cpg))
            sr.Write(CODE.Text)
            sr.Close()
        End If
    End Sub

    Private Sub SAVEASM(sender As System.Object, e As System.EventArgs) Handles ASM保存ToolStripMenuItem.Click, ASMSB.Click
        Dim f As New SaveFileDialog
        f.InitialDirectory = Application.StartupPath
        f.Filter = "テキストファイル（*.txt)|*.txt"
        f.Title = "ASMテキストの保存"
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim sr As New StreamWriter(f.FileName, False, Encoding.GetEncoding(cpg))
            sr.Write(ASM.Text)
            sr.Close()
        End If
    End Sub

    Private Sub 終了ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 終了.Click
        Me.Close()
    End Sub

    Private Sub フォント_Click(sender As System.Object, e As System.EventArgs) Handles フォント.Click
        Dim fd As New FontDialog()
        fd.Font = ASM.Font
        fd.ShowColor = False
        If fd.ShowDialog() <> DialogResult.Cancel Then
            ASM.Font = fd.Font
            CODE.Font = fd.Font
        End If

    End Sub

    Private Sub Removeheader(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim rm As New Regex("^_[A-Z]*? ", RegexOptions.ECMAScript)
        Dim rmm As Match = rm.Match(CODE.Text)
        While rmm.Success
            CODE.Text = CODE.Text.Replace(rmm.Value, "")
            rmm = rmm.NextMatch
        End While
    End Sub

    Private Sub cnt_code2asm_Click(sender As System.Object, e As System.EventArgs) Handles cnt_code2asm.Click
        Dim s As String = ""
        Dim ss As String() = CODE.Text.Split(CChar(vbLf))
        Dim address(ss.Length * 2) As Integer
        Dim values(ss.Length * 2) As UInteger
        Dim h As New Regex("0x([2C0][01]|0[89])[0-9A-Fa-f]{6} 0x[0-9A-Fa-f]{8}")
        Dim hm As Match
        Dim cd As New Regex("0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}")
        Dim cdm As Match
        Dim pmetan As New Regex("0x8[0-9A-Fa-f]{7} 0x0[0189][0-9A-Fa-f]{6} 0x[0-9A-Fa-f]{8}")
        Dim pmem As Match
        Dim sbr As New Regex("0x(E[89]|F[0-9A-Fa-f]|C2)[0-9A-Fa-f]{6} 0x[0-9A-Fa-f]{8}")
        Dim sbm As Match
        Dim pop As New Regex("[8][0-9A-Fa-f]{7} [0-9A-Fa-f]{4}")
        Dim popm As Match
        Dim subrutin As Integer = 0
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim j As Integer = 0
        Dim cmfenc As Integer = 0
        Dim tmpadr As Integer = 0
        Dim cmfaddrval(1) As Integer
        Dim st As Integer = Convert.ToInt32(ADDR.Text, 16)
        If st <= &H1800000 Then
            st = st + &H8800000
        End If

        For i = 0 To ss.Length - 1
            hm = h.Match(ss(i))
            pmem = pmetan.Match(ss(i))
            sbm = sbr.Match(ss(i))
            cdm = cd.Match(ss(i))
            popm = pop.Match(ss(i))
            If pmem.Success Then
                address(k) = Convert.ToInt32(pmem.Value.Substring(13, 8), 16)
                values(k) = Convert.ToUInt32(pmem.Value.Substring(24, 8), 16)
                k += 1
            ElseIf sbm.Success Then
                l = Convert.ToInt32(sbm.Value.Substring(19, 2), 16)
                j = k
                If sbm.Value.Contains("0xE") Then
                    tmpadr = (Convert.ToInt32(sbm.Value.Substring(2, 8), 16) And &H9FFFFFF)
                    subrutin = (l >> 3)
                    If (l And 7) <> 0 Then
                        subrutin += 1
                    End If
                ElseIf sbm.Value.Contains("0xC2") Then
                    tmpadr = tmp
                    subrutin = (l >> 3)
                    If (l And 7) <> 0 Then
                        subrutin += 1
                    End If
                ElseIf sbm.Value.Contains("0xF") Then
                    tmpadr = cmf
                    cmfenc = l
                    l = 0
                    subrutin = Convert.ToInt32(sbm.Value.Substring(8, 2), 16)
                Else
                    subrutin = 0
                End If
            ElseIf subrutin > 0 AndAlso cdm.Success Then
                address(k) = (tmpadr + (k - j) * 4)
                values(k) = Convert.ToUInt32(cdm.Value.Substring(2, 8), 16)
                If (subrutin > 1) Then
                    address(k + 1) = (tmpadr + (k + 1 - j) * 4)
                    values(k + 1) = Convert.ToUInt32(cdm.Value.Substring(13, 8), 16)
                    If (cmfenc > 0) Then
                        values = DecryptCB(values, k)
                        If (cmfenc = 2) Then
                            values = SwapBack(values, k)
                            values = DecryptCB(values, k)
                        End If
                    End If
                    k += 2
                ElseIf (subrutin = 1 AndAlso ((l And 7) > 4 Or (l And 7) = 0)) Then
                    address(k + 1) = (tmpadr + (k + 1 - j) * 4)
                    values(k + 1) = Convert.ToUInt32(cdm.Value.Substring(13, 8), 16)
                    If (cmfenc > 0) Then
                        values = DecryptCB(values, k)
                        If (cmfenc = 2) Then
                            values = SwapBack(values, k)
                            values = DecryptCB(values, k)
                        End If
                    End If
                    k += 2
                Else
                    If (cmfenc > 0) Then
                        values = DecryptCB(values, k)
                        If (cmfenc = 2) Then
                            values = SwapBack(values, k)
                            values = DecryptCB(values, k)
                        End If
                    End If
                    k += 1
                End If
                subrutin -= 1
            ElseIf hm.Success Then
                address(k) = Convert.ToInt32(hm.Value.Substring(2, 8), 16)
                values(k) = Convert.ToUInt32(hm.Value.Substring(13, 8), 16)
                k += 1
            ElseIf popm.Success Then
                address(k) = Convert.ToInt32(popm.Value.Substring(0, 8), 16)
                values(k) = Convert.ToUInt32(popm.Value.Substring(9, 4), 16)
                k += 1
            End If
        Next
        Array.Resize(address, k)
        Array.Resize(values, k)
        Array.Sort(address, values)
        Array.Resize(address, k + 2)
        address(k + 1) = (address(k) + 4)
        Dim sb As New StringBuilder
        Dim diff As Integer = CInt(address(1) - address(0))
        tmpadr = (address(0) And &H9FFFFFF)
        If tmpadr <= &H1800000 Then
            tmpadr = (tmpadr + &H8800000)
        End If

        If diff <> 0 AndAlso st <> tmpadr Then
            sb.Append("setpc" & vbTab & "0x")
            tmpadr = (address(0) And &H9FFFFFF)
            If tmpadr <= &H1800000 Then
                tmpadr = (tmpadr + &H8800000)
            End If
            sb.AppendLine(tmpadr.ToString("X"))
        End If

        For i = 0 To k - 1
            If diff = 4 Then
                diff = (address(i + 1) - address(i))
                sb.AppendLine(decoders(values(i), CInt(address(i))))
            ElseIf diff = 2 Then
                sb.AppendLine(decoders((values(i + 1) << 16) Or values(i), CInt(address(i))))
                i += 1
            Else
                sb.AppendLine()
                sb.Append("setpc" & vbTab & "0x")
                tmpadr = (address(i) And &H9FFFFFF)
                If tmpadr <= &H1800000 Then
                    tmpadr = (tmpadr + &H8800000)
                End If
                sb.AppendLine(tmpadr.ToString("X"))
                sb.AppendLine(decoders(values(i), (address(i))))
            End If
        Next

        ASM.Text = sb.ToString
    End Sub

    Dim CBSEED As Int64() = {
      &H288596, &HA0B8D9B, &H1DD9A10A,
      &H37DD28, &HA0133F8, &HB95AB9B0,
      &H3BEEF1, &HAF733EC, &H5CF5D328,
      &HBC822, &HA15C574, &H95FE7F10,
      &HBC935D, &HA50AC20, &H8E2D6303,
      &HA139F2, &HA920FB9, &H16BB6286,
      &HE9BBF8, &HA599F0B, &HE389324C,
      &HF57F7B, &HA4AA0E3, &H7AC6EA8,
      &H90D704, &HA21C012, &HAA4811D8,
      &H1814D4, &HA906254, &H76CE4E18,
      &HC5848E, &HA31FD54, &HFE447516,
      &H5B83E7, &HA091C0E, &HF9CD94D0,
      &H108CF7, &HA372B38, &H4C24DEDB,
      &H46CE5A, &HA6F266C, &H68275C4E,
      &H3A5BF4, &HA61DD4A, &H72494382,
      &H6FAFFC, &HA0DBF92, &HC8AA88E8}

    Private Function EncryptCB(ByVal addrval As Int64()) As Int64()
        Dim s As String = ""
        Dim addr As Int64 = addrval(0)
        Dim val As Int64 = addrval(1)
        Dim cmd As Integer = CInt((addr And &HF0000000) >> 28)
        Dim tmp4 As Int64 = (addr >> 8) And &HFFFF
        Dim tmp3 As Int64 = (addr << 16) And &HFF0000
        Dim tmp2 As Int64 = tmp4 Or tmp3
        Dim tmp1 As Int64 = addr And &HFF000000
        Dim mask As Int64 = 4294967295
        tmp2 = tmp2 + CBSEED(3 * cmd)
        If (tmp2 >= &H1000000) Then
            tmp2 -= &H1000000
        End If
        addr = tmp1 Or tmp2
        addr = addr Xor CBSEED(3 * cmd + 1)
        If cmd > 2 Then
            val = val + CBSEED(3 * cmd + 2)
            val = val Xor addr
        End If
        addr = addr And mask
        val = val And mask
        addrval(0) = addr
        addrval(1) = val

        '	int cmd = (addr & 0xf0000000) >> 28;
        '	if(cmd > 2)
        '	{
        '		val ^= addr;
        '		val -= CBseeds[cmd].value;
        '	}
        '	addr ^= CBseeds[cmd].XOR;
        '	int temp1 = (addr & 0xff000000);
        '	int temp2 = (addr & 0xffffff) - CBseeds[cmd].addition;
        '	if(temp2 < 0)
        '		temp2 += 0x1000000;
        '	int temp3 = (temp2 & 0xff0000) >> 16;
        '	int temp4 = (temp2 & 0xffff) << 8;
        '	addr = temp1 + temp3 + temp4;

        Return addrval
    End Function

    Private Function DecryptCB(ByVal addrval As UInteger(), ByVal k As Integer) As UInteger()

        '	int cmd = (addr & 0xf0000000) >> 28;
        '	if(cmd > 2)
        '	{
        '		val ^= addr;
        '		val -= CBseeds[cmd].value;
        '	}
        '	addr ^= CBseeds[cmd].XOR;
        '	int temp1 = (addr & 0xff000000);
        '	int temp2 = (addr & 0xffffff) - CBseeds[cmd].addition;
        '	if(temp2 < 0)
        '		temp2 += 0x1000000;
        '	int temp3 = (temp2 & 0xff0000) >> 16;
        '	int temp4 = (temp2 & 0xffff) << 8;
        '	addr = temp1 + temp3 + temp4;
        Dim addr As Int64 = addrval(k)
        Dim val As Int64 = addrval(k + 1)
        Dim cmd As Integer = CInt((addr And &HF0000000) >> 28)
        Dim mask As Int64 = 4294967295

        If (cmd > 2) Then
            val = val Xor addr
            val -= CBSEED(cmd * 3 + 2)
        End If

        addr = addr Xor CBSEED(3 * cmd + 1)
        Dim temp1 As Int64 = CLng((addr And &HFF000000))
        Dim temp2 As Int64 = CLng((addr And &HFFFFFF) - CBSEED(3 * cmd))
        If (temp2 < 0) Then
            temp2 += &H1000000
        End If
        Dim temp3 As Int64 = (temp2 And &HFF0000) >> 16
        Dim temp4 As Int64 = (temp2 And &HFFFF) << 8
        addr = temp1 Or temp3 Or temp4

        addr = addr And mask
        val = val And mask
        addrval(k) = CUInt(addr)
        addrval(k + 1) = CUInt(val)

        Return addrval
    End Function

    Private Function SwapFF(ByVal addrval As Int64()) As Int64()
        'u32 a=code->adr;
        'u32 v=code->val;
        'code->adr=(a&0xFF)|((v&0xFF0000)>>8)|((a&0xFF00)<<8)|(v&0xFF000000);
        'code->val=((v&0xFF00)<<16)|((a&0xFF000000)>>8)|((v&0xFF)<<8)|((a&0xFF0000)>>16);
        Dim a As Int64 = addrval(0)
        Dim v As Int64 = addrval(1)
        '0xAA33BB44 0xCC11DD22
        '0x11223344 0xAABBCCDD
        addrval(0) = CUInt((a And &HFF) Or ((v And &HFF0000) << 8) Or ((a And &HFF0000) >> 8) Or ((v And &HFF) << 16))
        addrval(1) = CUInt(((a And &HFF000000)) Or ((v And &HFF000000) >> 16) Or ((a And &HFF00) << 8) Or ((v And &HFF00) >> 8))

        Return addrval
    End Function

    Private Function SwapBack(ByVal addrval As UInteger(), ByVal k As Integer) As UInteger()
        'u32 a=code->adr;
        'u32 v=code->val;
        'code->adr=(a&0xFF)|((v&0xFF0000)>>8)|((a&0xFF00)<<8)|(v&0xFF000000);
        'code->val=((v&0xFF00)<<16)|((a&0xFF000000)>>8)|((v&0xFF)<<8)|((a&0xFF0000)>>16);
        Dim a As Int64 = addrval(k)
        Dim v As Int64 = addrval(k + 1)
        '0x11223344 0xAABBCCDD
        '0xAA33BB44 0xCC11DD22
        addrval(k) = CUInt((a And &HFF) Or ((v And &HFF0000) >> 8) Or ((a And &HFF00) << 8) Or (v And &HFF000000))
        addrval(k + 1) = CUInt(((v And &HFF00) << 16) Or ((a And &HFF000000) >> 8) Or ((v And &HFF) << 8) Or ((a And &HFF0000) >> 16))

        Return addrval
    End Function

    Private Sub 設定ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 設定ToolStripMenuItem.Click
    End Sub

    Private Sub ASMopen_Click(sender As System.Object, e As System.EventArgs) Handles ASMopen.Click
        Dim f As New OpenFileDialog
        f.InitialDirectory = Application.StartupPath
        f.Filter = "テキストファイル（*.txt)|*.txt"
        f.Title = "開くテキストを選んでください"
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim sr As New StreamReader(f.FileName, Encoding.GetEncoding(cpg))
            ASM.Text = sr.ReadToEnd()
            sr.Close()
        End If
    End Sub

    Private Sub CODEopen_Click(sender As System.Object, e As System.EventArgs) Handles CODEopen.Click
        Dim f As New OpenFileDialog
        f.InitialDirectory = Application.StartupPath
        f.Filter = "テキストファイル（*.txt)|*.txt|バイナリファイル(*.bin)|*.bin|パッチファイル(*.pat)|*.pat"
        f.Title = "開くテキスト/バイナリを選んでください"
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim ext As String = Path.GetExtension(f.FileName)
            If ext = ".bin" Or ext = ".pat" Then
                bin2txt(f.FileName, ext)
            Else
                Dim sr As New StreamReader(f.FileName, Encoding.GetEncoding(cpg))
                CODE.Text = sr.ReadToEnd()
                sr.Close()
            End If
        End If
    End Sub

    Private Function bin2txt(ByVal fn As String, ByVal ext As String) As Boolean
        Dim bin As New FileStream(fn, FileMode.Open, FileAccess.Read)
        Dim buffer(2048) As Byte
        Dim sb As StringBuilder = New StringBuilder
        Dim st As Integer = Convert.ToInt32(ADDR.Text, 16)
        Dim readSize As Integer
        Dim total As Integer
        If ext = ".pat" Then
            bin.Read(buffer, 0, 4)
            st = BitConverter.ToInt32(buffer, 0)
        End If

        While True
            readSize = bin.Read(buffer, 0, buffer.Length)
            total += readSize
            readSize = readSize >> 2
            If readSize = 0 Then
                Exit While
            End If
            For i = 0 To readSize - 1
                sb.Append("0x")
                sb.Append(st.ToString("X8"))
                sb.Append(" 0x")
                sb.Append(BitConverter.ToInt32(buffer, i << 2).ToString("X8"))
                sb.AppendLine()
                st += 4
            Next
            If total >= 1000 Then
                MessageBox.Show("4960バイト以上はよみこめません")
                Exit While
            End If
        End While
        bin.Close()

        CODE.Text = sb.ToString
        Return True
    End Function

    Private Sub RPN_Click(sender As System.Object, e As System.EventArgs) Handles RPN.Click
        RPN.Checked = Not RPN.Checked
    End Sub

    Private Sub サブルーチン仮アドレスToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles サブルーチン仮アドレスToolStripMenuItem.Click
        Dim f As New Form2
        f.ShowDialog(Me)
        f.Dispose()
    End Sub

    Private Sub STACKORDER_Click(sender As System.Object, e As System.EventArgs) Handles STACKORDER.Click, LOOKSORDER.Click
        STACKORDER.Checked = Not STACKORDER.Checked
        LOOKSORDER.Checked = Not STACKORDER.Checked
    End Sub

    Private Sub CVTRPN_Click(sender As System.Object, e As System.EventArgs) Handles CVTRPN.Click
        CVTRPN.Checked = Not CVTRPN.Checked
    End Sub

    Private Sub cr_Click(sender As System.Object, e As System.EventArgs) Handles cr.Click
        cr.Checked = Not cr.Checked
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ss As String() = CODE.Text.Split(CChar(vbLf))
        Dim rg As New Regex("^_L 0x[0-9A-Fa-f]{8} 0x[0-9A-Fa-f]{8}")
        Dim m As Match
        Dim sb As New StringBuilder
        Dim k As Integer
        For Each s In ss
            m = rg.Match(s)
            If m.Success Then
                k = Convert.ToInt32(s.Substring(5, 8), 16)
                k = k Xor &HD6F73BEE
                sb.Append("_L 0x")
                sb.Append(k.ToString("X8"))
                sb.AppendLine(s.Remove(0, 13))

            Else
                sb.AppendLine(s.Trim)
            End If

        Next

        CODE.Text = sb.ToString

    End Sub
End Class
