Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Form1

    Dim mathsconst As String() = {"π", "円周率", "黄金比", "自然対数の底"}
    Dim mathrp As String() = {"pi", "pi", "goldenratio", "e"}

    Private Function cvt_dbl(ByVal s As String) As Double
        Dim dem As Double = 0
        Dim cnst As New Regex("-?(pi|goldenratio|e)")
        Dim cnstm As Match = cnst.Match(s)
        Dim frac As New Regex("-?\d+\.?\d*")
        Dim fracm As Match = frac.Match(s)
        If cnstm.Success Then
            If cnstm.Value.Contains("pi") Then
                dem = (Math.PI)
            ElseIf cnstm.Value.Contains("goldendratio") Then
                dem = ((1 + Math.Sqrt(5)) / 2)
            ElseIf cnstm.Value.Contains("e") Then
                dem = (Math.E)
            Else
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
                Case "dot"
                    'dem(0),dem(1),dem(2) | dem(3),dem(4),dem(5)
                    dem(5) = dem(0) * dem(3) + dem(1) * dem(4) + dem(2) * dem(5)
                    Array.Copy(dem, 5, dem, 0, len - 5)
                Case "cross"
                    'dem(0),dem(1),dem(2) | dem(3),dem(4),dem(5)
                    dem(2) = dem(0) * dem(4) - dem(1) * dem(3)
                    dem(1) = dem(2) * dem(3) - dem(0) * dem(5)
                    dem(0) = dem(1) * dem(5) - dem(2) * dem(4)
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
                Case "\"
                    Dim k As Double = dem(0)
                    Dim j As Double = dem(1)
                    If k < 0 Then
                        k = -k
                    End If
                    If j < 0 Then
                        j = -j
                    End If
                    While (True)
                        If j - k > 0 Then
                            j -= k
                        Else
                            Exit While
                        End If
                    End While
                    If dem(1) < 0 Then
                        dem(1) = -j
                    Else
                        dem(1) = j
                    End If
                    Array.Copy(dem, 1, dem, 0, len)
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
                Case "hypot"
                    dem(1) = Math.Sqrt(Math.Pow(dem(1), 2) + Math.Pow(dem(0), 2))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "heron"
                    'http://ja.wikipedia.org/wiki/%E3%83%98%E3%83%AD%E3%83%B3%E3%81%AE%E5%85%AC%E5%BC%8F
                    'a < b+c
                    'b < a+c
                    'c < a+b
                    Dim s As Double = (dem(0) + dem(1) + dem(2)) / 2
                    dem(2) = Math.Sqrt(s * (s - dem(0)) * (s - dem(1)) * (s - dem(2)))
                    Array.Copy(dem, 2, dem, 0, len - 2)
                Case "bretschneider"
                    'http://ja.wikipedia.org/wiki/%E3%83%96%E3%83%AC%E3%83%BC%E3%83%88%E3%82%B7%E3%83%A5%E3%83%8A%E3%82%A4%E3%83%80%E3%83%BC%E3%81%AE%E5%85%AC%E5%BC%8F
                    Dim t As Double = (dem(5) + dem(4) + dem(2) + dem(3)) / 2
                    dem(5) = Math.Sqrt(((t - dem(2)) * (t - dem(3)) * (t - dem(4)) * (t - dem(5))) - ((dem(2) * dem(3) * dem(4) * dem(5) * Math.Pow(Math.Cos((dem(0) + dem(1)) / 2), 2))))
                    Array.Copy(dem, 5, dem, 0, len - 5)
                Case "1/x", "reci"
                    dem(0) = 1 / dem(0)
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
                            dem(1) = dem(1) Or (1 << (31 - dem(0)))
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

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If CheckBox2.Checked Then
            Dim d As Integer = rpnint(TextBox2.Text)
            TextBox3.Text = d.ToString("X8")
        Else
            Dim d As Double = rpndbl(TextBox2.Text)
            TextBox3.Text = d.ToString
        End If

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim s As String = TextBox1.Text.Trim
        Dim p As New Polish
        If CheckBox2.Checked Then
            s = s.Replace("~", "not")
            s = p.Main(s, LOOKSORDER.Checked, CheckBox1.Checked, True)
        Else
            s = p.Main(s, LOOKSORDER.Checked, CheckBox1.Checked, False)
        End If
        TextBox2.Text = s

        If CheckBox3.Checked Then
            Button2_Click(sender, e)
        End If

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs)

    End Sub
End Class


