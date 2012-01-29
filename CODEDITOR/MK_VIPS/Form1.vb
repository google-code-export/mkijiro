Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Private Sub ff(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If File.Exists("conf") = True Then
            Dim sr As New System.IO.StreamReader("conf", System.Text.Encoding.GetEncoding(0))
            Dim s As String
            While sr.Peek() > -1
                s = sr.ReadLine()
                If s.Contains("ADDR") Then
                    ADDR.Text = s.Remove(0, 4)
                ElseIf s.Contains("MODE") Then
                    MODE.Text = s.Remove(0, 4)
                End If
            End While
            sr.Close()
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Dim sr As New System.IO.StreamWriter("conf", False, System.Text.Encoding.GetEncoding(0))
        Dim s As String
        s = "ADDR" & ADDR.Text
        sr.WriteLine(s)
        s = "MODE" & MODE.Text
        sr.WriteLine(s)
        sr.Close()
    End Sub


    Function assembler(ByVal str As String, ByVal str2 As String) As String
        Try
            Dim hex As Integer = 0
            Dim hex2 As Integer = Convert.ToInt32(str2, 16) And &H9FFFFFFF
            Dim asm As String = ""
            Dim mips As String = ""

            Dim psdis As New Regex("(\t|\x20|　)*?#.+$")
            Dim psdism As Match = psdis.Match(str)
            If psdism.Success Then
                str = str.Substring(0, psdism.Index)
            End If
            str &= " "

            Dim valhex As New Regex("(\$|0x)[0-9A-Fa-f]{3,8}")
            Dim valhexm As Match = valhex.Match(str)
            If valhexm.Success Then
                str = str.Replace(valhexm.Value, valhexm.Value.ToUpper)
                str = str.Replace("0X", "0x")
            End If
            Dim ss As String() = str.Split(CChar(","))
            Dim shead As New Regex("^[a-z0-9\.]+(\x20|\t)+")
            Dim sheadm As Match = shead.Match(str)

            If sheadm.Success Then
                mips = sheadm.Value.Replace(" ", "")
                mips = mips.Replace(vbTab, "")
                str = str.Trim
                ss(0) = ss(0).Replace(sheadm.Value, "").ToLower
                If mips = "nop" Then
                ElseIf mips = "syscall" Then
                    hex = 12
                ElseIf mips = "break" Then
                    hex = &H1CD '13
                ElseIf mips = "sync" Then
                    hex = 15
                ElseIf mips = "sll" Then
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "rotr" Then
                    hex = hex Or &H200002
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "rotv" Then
                    hex = hex Or &H46
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srl" Then
                    hex = hex Or &H2
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "sra" Then
                    hex = hex Or &H3
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "sllv" Then
                    hex = hex Or &H4
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srlv" Then
                    hex = hex Or &H6
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srav" Then
                    hex = hex Or &H7
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "jalr" Then
                    hex = hex Or &H9
                    If ss.Length = 1 Then
                        Array.Resize(ss, 2)
                        ss(1) = "ra"
                    End If
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 2)
                ElseIf mips = "movz" Then
                    hex = hex Or &HA
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "movn" Then
                    hex = hex Or &HB
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "mfhi" Then
                    hex = hex Or &H10
                    hex = reg_boolean_para(ss(0), hex, 2)
                ElseIf mips = "mthi" Then
                    hex = hex Or &H11
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "mflo" Then
                    hex = hex Or &H12
                    hex = reg_boolean_para(ss(0), hex, 2)
                ElseIf mips = "mtlo" Then
                    hex = hex Or &H13
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "clz" Then
                    hex = hex Or &H16
                    hex = reg_boolean2(str, hex, 0)
                ElseIf mips = "clo" Then
                    hex = hex Or &H17
                    hex = reg_boolean2(str, hex, 0)
                ElseIf mips = "add" Then
                    hex = hex Or &H20
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "addu" Then
                    hex = hex Or &H21
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "mov" Or mips = "move" Then
                    hex = hex Or &H21
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "sub" Then
                    hex = hex Or &H22
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "neg" Then
                    hex = hex Or &H22
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "subu" Then
                    hex = hex Or &H23
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "negu" Then
                    hex = hex Or &H23
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "or" Then
                    hex = hex Or &H25
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "and" Then
                    hex = hex Or &H24
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "xor" Then
                    hex = hex Or &H26
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "nor" Then
                    hex = hex Or &H27
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "not" Then
                    hex = hex Or &H27
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "slt" Then
                    hex = hex Or &H2A
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "sltu" Then
                    hex = hex Or &H2B
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "max" Then
                    hex = hex Or &H2C
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "min" Then
                    hex = hex Or &H2D
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "j" Then
                    hex = hex Or &H8000000
                    hex = offset_boolean(str, hex)
                ElseIf mips = "jal" Then
                    hex = hex Or &HC000000
                    hex = offset_boolean(str, hex)
                ElseIf mips = "jr" Then
                    hex = hex Or &H8
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "mult" Then
                    hex = hex Or &H18
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "multu" Then
                    hex = hex Or &H19
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "div" Then
                    hex = hex Or &H1A
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "divu" Then
                    hex = hex Or &H1B
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "madd" Then
                    hex = hex Or &H1C
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "maddu" Then
                    hex = hex Or &H1D
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "msub" Then
                    hex = hex Or &H2E
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "msubu" Then
                    hex = hex Or &H2F
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "bltz" Then
                    hex = hex Or &H4000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgez" Then
                    hex = hex Or &H4010000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzl" Then
                    hex = hex Or &H4020000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezl" Then
                    hex = hex Or &H4030000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzal" Then
                    hex = hex Or &H4100000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezal" Then
                    hex = hex Or &H4110000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzall" Then
                    hex = hex Or &H4120000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezall" Then
                    hex = hex Or &H4130000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "mtsab" Then
                    hex = hex Or &H4180000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = valhex_boolean(ss(1), hex)
                ElseIf mips = "mtsah" Then
                    hex = hex Or &H4190000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = valhex_boolean(ss(1), hex)

                    '0x10 branch
                ElseIf mips = "beq" Then
                    hex = hex Or &H10000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bne" Then
                    hex = hex Or &H14000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "blez" Then
                    hex = hex Or &H18000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgtz" Then
                    hex = hex Or &H1C000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)

                    '0x20 add/boolean
                ElseIf mips = "addi" Then
                    hex = hex Or &H20000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "addiu" Then
                    hex = hex Or &H24000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "li" Then
                    hex = hex Or &H24000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "slti" Then
                    hex = hex Or &H28000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "sltiu" Then
                    hex = hex Or &H2C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "andi" Then
                    hex = hex Or &H30000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "ori" Then
                    hex = hex Or &H34000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "xori" Then
                    hex = hex Or &H38000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "lui" Then
                    hex = hex Or &H3C000000
                    hex = reg_boolean_para(str, hex, 1)
                    hex = valhex_boolean(str, hex)

                    '0x40 FPU
                ElseIf mips = "mfc0" Then
                    hex = hex Or &H40000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                ElseIf mips = "mtc0" Then
                    hex = hex Or &H40800000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                ElseIf mips = "cfc0" Then
                    hex = hex Or &H40400000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "ctc0" Then
                    hex = hex Or &H40C00000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "eret" Then
                    hex = &H42000018
                ElseIf mips = "cfc1" Then
                    hex = hex Or &H44400000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "ctc1" Then
                    hex = hex Or &H44C00000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "mfc1" Then
                    hex = hex Or &H44000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "mtc1" Then
                    hex = hex Or &H44800000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "bc1f" Then
                    hex = hex Or &H45000000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1t" Then
                    hex = hex Or &H45010000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1tl" Then
                    hex = hex Or &H45020000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1tl" Then
                    hex = hex Or &H45030000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "add.s" Then
                    hex = hex Or &H46000000
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "sub.s" Then
                    hex = hex Or &H46000001
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "mul.s" Then
                    hex = hex Or &H46000002
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "div.s" Then
                    hex = hex Or &H46000003
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "sqrt.s" Then
                    hex = hex Or &H46000004
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "abs.s" Then
                    hex = hex Or &H46000005
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "mov.s" Then
                    hex = hex Or &H46000006
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "neg.s" Then
                    hex = hex Or &H46000007
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "round.w.s" Then
                    hex = hex Or &H4600000C
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "trunc.w.s" Then
                    hex = hex Or &H4600000D
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "ceil.w.s" Then
                    hex = hex Or &H4600000E
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "floor.w.s" Then
                    hex = hex Or &H4600000F
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "cvt.s.w" Then
                    hex = hex Or &H46800020
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "cvt.w.s" Then
                    hex = hex Or &H46000024
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "c.f.s" Then
                    hex = hex Or &H46000030
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.un.s" Then
                    hex = hex Or &H46000031
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.eq.s" Then
                    hex = hex Or &H46000032
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ueq.s" Then
                    hex = hex Or &H46000033
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.olt.s" Then
                    hex = hex Or &H46000034
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ult.s" Then
                    hex = hex Or &H46000035
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ole.s" Then
                    hex = hex Or &H46000036
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ule.s" Then
                    hex = hex Or &H46000037
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.sf.s" Then
                    hex = hex Or &H46000038
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngle.s" Then
                    hex = hex Or &H46000039
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.seq.s" Then
                    hex = hex Or &H4600003A
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngl.s" Then
                    hex = hex Or &H4600003B
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.lt.s" Then
                    hex = hex Or &H4600003C
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.nge.s" Then
                    hex = hex Or &H4600003D
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.le.s" Then
                    hex = hex Or &H4600003E
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngl.s" Then
                    hex = hex Or &H4600003F
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)

                    '0x50
                ElseIf mips = "beql" Then
                    hex = hex Or &H50000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bnel" Then
                    hex = hex Or &H54000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "blezl" Then
                    hex = hex Or &H58000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgtzl" Then
                    hex = hex Or &H5C000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)

                    '     case 0x70:
                    '        if(a_opcode >> 24 == 0x70){
                    '                switch(a_opcode & 0xE007FF){
                    '                case 0x24:
                    '                pspDebugScreenPuts("mfic     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                mipsNibble(a_opcode, 2, 0);
                    '                break;

                    '                case 0x26:
                    '                pspDebugScreenPuts("mtic     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                mipsNibble(a_opcode, 2, 0);
                    '                break;

                    '                case 0x3D:
                    '                pspDebugScreenPuts("mfdr     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                DrRegister(a_opcode,2,0);
                    '                break;

                    '                case 0x80003D:
                    '                pspDebugScreenPuts("mtdr     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                DrRegister(a_opcode,2,0);
                    '                break; 
                    '//"mfdr",              0x7000003D, 0xFFE007FF, "%t, %r"},
                    '//"mtdr",              0x7080003D, 0xFFE007FF, "%t, %r"},
                    '//"mfic",              0x70000024, 0xFFE007FF, "%t, %p"},
                    '//"mtic",              0x70000026, 0xFFE007FF, "%t, %p"},
                    '                }
                ElseIf mips = "dbreak" Then
                    hex = &H7000003F
                ElseIf mips = "dret" Then
                    hex = &H7000003E
                ElseIf mips = "haltl" Then
                    hex = &H70000000
                ElseIf mips = "seb" Then
                    hex = &H7C000420
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "seh" Then
                    hex = &H7C000620
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "wsbbn" Then
                    hex = &H7C0000A0
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "wsbw" Then
                    hex = &H7C0000E0
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "bitrev" Then
                    hex = &H7C000520
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(0), hex, 1)

                    '        switch(a_opcode & 0x03F){
                    '        case 0x0:
                    '        pspDebugScreenPuts("ext      ");
                    '        mipsRegister(a_opcode, T, 1);
                    '        mipsRegister(a_opcode, S, 1);
                    '        mipsNibble(a_opcode, 3, 1);
                    '        a_opcode+=0x800;
                    '        mipsNibble(a_opcode, 2, 0);
                    '        break;

                    '        case 0x4:
                    '        pspDebugScreenPuts("ins      ");
                    '        mipsRegister(a_opcode, T, 1);
                    '        mipsRegister(a_opcode, S, 1);
                    '        mipsNibble(a_opcode, 3, 1);
                    '        mipsins(a_opcode);
                    '        break;
                    '        }
                    '    break;

                    '0x80
                ElseIf mips = "lb" Then
                    hex = hex Or &H80000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lh" Then
                    hex = hex Or &H84000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lw" Then
                    hex = hex Or &H8C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lbu" Then
                    hex = hex Or &H90000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lhu" Then
                    hex = hex Or &H94000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwu" Then
                    hex = hex Or &H9C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwl" Then
                    hex = hex Or &H88000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwr" Then
                    hex = hex Or &H98000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xA0
                ElseIf mips = "sb" Then
                    hex = hex Or &HA0000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "sh" Then
                    hex = hex Or &HA4000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "sw" Then
                    hex = hex Or &HAC000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swl" Then
                    hex = hex Or &HA8000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swr" Then
                    hex = hex Or &HB8000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xc0
                ElseIf mips = "lwc1" Then
                    hex = hex Or &HC4000000
                    hex = float_sel(ss(0), hex, 1)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xe0
                ElseIf mips = "swc1" Then
                    hex = hex Or &HE4000000
                    hex = float_sel(ss(0), hex, 1)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = offset_boolean3(str, hex)

                End If

                asm = "0x" & Convert.ToString(hex, 16).ToUpper.PadLeft(8, "0"c)
            End If

            'Dim vfreg As New Regex("(S|C|M)\d\d\d")

            Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return ""
        End Try
    End Function

    Function decoderr(ByVal str As String) As String
        Try
            Dim hex As Integer = Convert.ToInt32(str)
            Dim asm As String = ""

            Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return str
        End Try
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

    Function offset_boolean(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If k < &H1800000 Then
                k += &H8800000
            End If
            hex = hex Or ((k >> 2) And &H3FFFFFF)
        End If
        Return hex
    End Function

    Function offset_boolean2(ByVal str As String, ByVal hex As Integer, ByVal hex2 As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|-)?\d{1,4}$")
        Dim valdecm As Match = valdec.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If k < &H1800000 Then
                k += &H8800000
            End If
            If hex2 < &H1800000 Then
                hex2 += &H8800000
            End If
            hex = hex Or ((k - hex2 - 4) >> 2 And &HFFFF)
        End If
        If valdecm.Success Then
            hex = hex Or ((Convert.ToInt32(valdecm.Value.Remove(0, 1)) - 1) And &HFFFF)
        End If
        Return hex
    End Function

    Function offset_boolean3(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)-?(\$|0x)[0-9A-Fa-f]{1,4}\(")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|-)?\d{1,5}\(")
        Dim valdecm As Match = valdec.Match(str)
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
        End If
        If valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1).Replace("(", "")) And &HFFFF)
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
        End If
        If valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1)) And &HFFFF)
        End If
        If valfloatm.Success Then
            Dim f As Single = Convert.ToSingle(valfloatm.Value.Remove(0, 1).Replace("f", ""))
            Dim bit() As Byte = BitConverter.GetBytes(f)
            Dim sb As New System.Text.StringBuilder()
            Dim i As Integer = 3
            While i >= 0
                sb.Append(Convert.ToString(bit(i), 16).PadLeft(2, "0"c))
                i -= 1
            End While
            hex = hex Or (Convert.ToInt32(sb.ToString.Substring(0, 4), 16))
        End If
        Return hex
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
        Dim valdecm As Match = valdec.Match(str)
        If valdecm.Success Then
            hex = hex Or (CInt(valdecm.Value) << 21) >> (5 * k)
        End If
        Return hex
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

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim ss As String() = TextBox1.Text.Split(CChar(vbLf))
        Dim sb As New StringBuilder
        Dim st As Integer = Convert.ToInt32(ADDR.Text, 16)
        Dim i As Integer = Convert.ToInt32(ADDR.Text, 16)
        If MODE.SelectedIndex < 2 Then
            If st >= &H8800000 Then
                i -= &H8800000
            End If
            If i >= &H8800000 Then
                i -= &H8800000
            End If
        Else
            If st < &H1800000 Then
                st += &H8800000
            End If
            If i < &H1800000 Then
                i += &H8800000
            End If
        End If

        Dim odd As Boolean = False
        For Each s As String In ss
            If s.Trim = "" Then
            ElseIf s.Length > 3 AndAlso (s.Substring(0, 2) = "__" Or s.Substring(0, 3) = "FNC" Or s.Substring(0, 2) = "//") Then
            Else
                If MODE.Text = "NITEPR" Then
                    sb.Append("0x")
                    sb.Append(Convert.ToString(i, 16).ToUpper.PadLeft(8, "0"c))
                    sb.Append(" ")
                    sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                    i += 4
                End If
                If MODE.Text = "CWCHEAT" Then
                    sb.Append("_L ")
                    sb.Append("0x")
                    sb.Append(Convert.ToString(i Or &H20000000, 16).ToUpper.PadLeft(8, "0"c))
                    sb.Append(" ")
                    sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                    i += 4
                End If
                If MODE.Text = "PSPAR" Then
                    sb.Append("_M ")
                    sb.Append("0x")
                    sb.Append(Convert.ToString(i And &HFFFFFFF, 16).ToUpper.PadLeft(8, "0"c))
                    sb.Append(" ")
                    sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                    i += 4
                End If
                If MODE.Text = "PMETAN" Then
                    sb.Append("_NWR ")
                    sb.Append("0x80000000 0x")
                    sb.Append(Convert.ToString(i And &HFFFFFFF, 16).ToUpper.PadLeft(8, "0"c))
                    sb.Append(" ")
                    sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                    i += 4
                End If
                If MODE.Text = "PSPAR(0xE)" Then
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
                End If
                If MODE.Text = "TEMPAR(0xC2)" Then
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
                End If
                If MODE.Text = "CMFUSION(0xF0)" Then
                    If odd = False Then
                        sb.Append("_L ")
                        sb.Append(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                        sb.Append(" ")
                        odd = True
                    Else
                        sb.AppendLine(assembler(s.Trim.ToLower, Convert.ToString(i, 16)))
                        odd = False
                    End If
                    i += 8
                End If
            End If
        Next
        If odd = True Then
            sb.Append("0x00000000")
        End If
        i = i - st
        If MODE.Text = "PSPAR(0xE)" Then
            sb.Insert(0, "_M 0xE" & (st And &HFFFFFFF).ToString("X").ToUpper.PadLeft(7, "0"c) & " 0x000000" & Convert.ToString((i), 16).ToUpper.PadLeft(2, "0"c) & vbCrLf)
        ElseIf MODE.Text = "TEMPAR(0xC2)" Then
            sb.Insert(0, "_N 0xC2000000 0x000000" & Convert.ToString((i), 16).ToUpper.PadLeft(2, "0"c) & vbCrLf)
        ElseIf MODE.Text = "CMFUSION(0xF0)" Then
            If odd = True Then
                i += 8
            End If
            sb.Insert(0, "_L 0xF00000" & Convert.ToString((i >> 4), 16).ToUpper.PadLeft(2, "0"c) & " 0x00000000" & vbCrLf)
        End If

        TextBox2.Text = sb.ToString
    End Sub

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

End Class
