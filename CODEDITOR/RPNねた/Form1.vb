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

    Private Function rpndbl(ByVal str As String) As Double
        Dim ss As String() = str.ToLower.Split(CChar(","))
        Dim len As Integer = ss.Length - 1
        Dim dem(len) As Double
        For i = 0 To len
            Select Case ss(i).Trim
                '4*(4*atan(1/5)-atan(1/239))
                '4,4,5,1/x,atan,*,239,1/x,atan,-,*
                Case "drop"
                    Array.Copy(dem, 1, dem, 0, len)
                Case "swap"
                    dem = swapper(dem)
                Case "deg2rad"
                    dem(0) = dem(0) * Math.PI / 180
                Case "deg2grad"
                    dem(0) = dem(0) * 100 / 90
                Case "deg2r"
                    dem(0) = dem(0) / 90
                Case "rad2deg"
                    dem(0) = dem(0) * 180 / Math.PI
                Case "rad2grad"
                    dem(0) = dem(0) * 200 / Math.PI
                Case "rad2r"
                    dem(0) = dem(0) * 2 / Math.PI
                Case "grad2deg"
                    dem(0) = dem(0) * 90 / 100
                Case "grad2rad"
                    dem(0) = dem(0) * Math.PI / 200
                Case "grad2r"
                    dem(0) = dem(0) / 100
                Case "r2deg"
                    dem(0) = dem(0) * 90
                Case "r2rad"
                    dem(0) = dem(0) * Math.PI / 2
                Case "r2grad"
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
                Case "logy"
                    dem = swapper2(dem)
                    dem(1) = Math.Log(dem(0), dem(1))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "logx"
                    dem = swapper2(dem)
                    dem(1) = Math.Log(dem(1), dem(0))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "1/x", "reci"
                    dem(0) = 1 / dem(0)
                Case "sqrt", "√"
                    dem(0) = Math.Sqrt(dem(0))
                Case "cbrt"
                    dem(0) = Math.Pow(dem(0), 1 / 3)
                Case "log"
                    dem(0) = Math.Log(dem(0), 10)
                Case "ln"
                    dem(0) = Math.Log(dem(0), Math.E)
                Case "atand"
                    dem(0) = Math.Atan(dem(0)) * 180 / Math.PI
                Case "atan"
                    dem(0) = Math.Atan(dem(0))
                Case "atanr"
                    dem(0) = Math.Atan(dem(0)) * 2 / Math.PI
                Case "atang"
                    dem(0) = Math.Atan(dem(0)) * 200 / Math.PI
                Case "atan2_d"
                    dem = swapper2(dem)
                    dem(1) = Math.Atan2(dem(0), dem(1)) * 180 / Math.PI
                    Array.Copy(dem, 1, dem, 0, len)
                Case "atan2_"
                    dem = swapper2(dem)
                    dem(1) = Math.Atan2(dem(0), dem(1))
                    Array.Copy(dem, 1, dem, 0, len)
                Case "atan2_r"
                    dem = swapper2(dem)
                    dem(1) = Math.Atan2(dem(0), dem(1)) * 2 / Math.PI
                    Array.Copy(dem, 1, dem, 0, len)
                Case "atan2_g"
                    dem = swapper2(dem)
                    dem(1) = Math.Atan2(dem(0), dem(1)) * 200 / Math.PI
                    Array.Copy(dem, 1, dem, 0, len)
                Case "acosd"
                    dem(0) = Math.Acos(dem(0)) * 180 / Math.PI
                Case "acos"
                    dem(0) = Math.Acos(dem(0))
                Case "acosr"
                    dem(0) = Math.Acos(dem(0)) * 2 / Math.PI
                Case "acosg"
                    dem(0) = Math.Acos(dem(0))
                Case "asind"
                    dem(0) = Math.Asin(dem(0)) * 180 / Math.PI
                Case "asin"
                    dem(0) = Math.Asin(dem(0))
                Case "asinr"
                    dem(0) = Math.Asin(dem(0)) * 2 / Math.PI
                Case "asing"
                    dem(0) = Math.Asin(dem(0))
                Case "tanhd"
                    dem(0) = Math.Tanh(dem(0) * 200 / Math.PI)
                Case "tahn"
                    dem(0) = Math.Tanh(dem(0))
                Case "tanhr"
                    dem(0) = Math.Tanh(dem(0) * Math.PI / 2)
                Case "tanhg"
                    dem(0) = Math.Tanh(dem(0) * 90 / 100 * Math.PI / 180)
                Case "coshd"
                    dem(0) = Math.Cosh(dem(0) * Math.PI / 180)
                Case "cohs"
                    dem(0) = Math.Cosh(dem(0))
                Case "coshr"
                    dem(0) = Math.Cosh(dem(0) * Math.PI / 2)
                Case "coshg"
                    dem(0) = Math.Cosh(dem(0) * 90 / 100 * Math.PI / 180)
                Case "sinhd"
                    dem(0) = Math.Sinh(dem(0) * Math.PI / 180)
                Case "sinh"
                    dem(0) = Math.Sinh(dem(0))
                Case "sinhr"
                    dem(0) = Math.Sinh(dem(0) * Math.PI / 2)
                Case "sinhg"
                    dem(0) = Math.Sinh(dem(0) * Math.PI / 200)
                Case "tand"
                    dem(0) = Math.Tan(dem(0) * Math.PI / 180)
                Case "tan"
                    dem(0) = Math.Tan(dem(0))
                Case "tanr"
                    dem(0) = Math.Tan(dem(0) * Math.PI / 2)
                Case "tang"
                    dem(0) = Math.Tan(dem(0) * Math.PI / 200)
                Case "cosd"
                    dem(0) = Math.Cos(dem(0) * Math.PI / 180)
                Case "cos"
                    dem(0) = Math.Cos(dem(0))
                Case "cosr"
                    dem(0) = Math.Cos(dem(0) * Math.PI / 2)
                Case "cosg"
                    dem(0) = Math.Cos(dem(0) * 90 / 100 * Math.PI / 180)
                Case "sind"
                    dem(0) = Math.Sin(dem(0) * Math.PI / 180)
                Case "sin"
                    dem(0) = Math.Sin(dem(0))
                Case "sinr"
                    dem(0) = Math.Sin(dem(0) * Math.PI / 2)
                Case "sing"
                    dem(0) = Math.Sin(dem(0) * 90 / 100 * Math.PI / 180)
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


