Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Form1

    Dim mathsconst As String() = {"π", "円周率", "黄金比", "自然対数の底"}
    Dim mathrp As String() = {"pi", "pi", "goldenratio", "e"}

    Private Function cvt_dbl(ByVal s As String) As Double
        Dim dem As Double = 0
        Dim cnst As New Regex("-?(e|pi|goldenratio)")
        Dim cnstm As Match = cnst.Match(s)
        Dim frac As New Regex("-?\d+\.?\d*")
        Dim fracm As Match = frac.Match(s)
        If cnstm.Success Then
            If cnstm.Value.Contains("e") Then
                dem = (Math.E)
            ElseIf cnstm.Value.Contains("pi") Then
                dem = (Math.PI)
            ElseIf cnstm.Value.Contains("goldendratio") Then
                dem = ((1 + Math.Sqrt(5)) / 2)
            End If
            If cnstm.Value.Contains("-") Then
                dem = -dem
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

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim d As Double = rpndbl(TextBox2.Text)
        TextBox3.Text = d.ToString
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim s As String = TextBox1.Text.Trim
        Dim p As New Polish
        TextBox2.Text = p.Main(s, LOOKSORDER.Checked)

    End Sub
End Class


