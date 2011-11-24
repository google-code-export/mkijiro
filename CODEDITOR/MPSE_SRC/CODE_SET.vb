Imports System.Text.RegularExpressions
Imports System.Media

Public Class CODE_SET

#Region "押されたボタン値を返す"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

#End Region

#Region "value TextBox 監視"

    Private Sub value_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles value.Leave

        Dim str As New Api.Str

        '0x 以外の文字を大文字に
        Dim tmp As String = "0x" & str.Fix(Replace(sender.Text, "0x", "").ToUpper, "[0-9A-F]", "")

        If tmp = "0x" Then

            tmp = tmp & "0"

        End If

        sender.Text = tmp


    End Sub

    Private Sub value_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles value.TextChanged

        Liv.Fix_Str(sender)

    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        Dim max As Integer

        '選択したBit数に応じて入力可能な最大文字数を設定
        Select Case ComboBox1.SelectedIndex

            Case 0 '8 Bit
                max = 2

            Case 1 '16 Bit
                max = 4
            Case 2, 3 '32 Bit
                max = 8
        End Select


        value.Text = "0x" & Microsoft.VisualBasic.Right(Replace(value.Text, "0x", "").PadLeft(8, "0"), max)
        value.MaxLength = max + 2


    End Sub

#End Region

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub CODE_SET_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim rokuyon As Int64 = 0
        Dim float As Single = 0
        Dim int As Integer = 0
        Dim mask As Int64() = {&HFF, &HFFFF, &HFFFFFFFF}
        If dec2hex.Text <> "" Then
            Select Case ComboBox1.SelectedIndex
                Case 0, 1, 2
                    Dim r As New Regex("^-?\d+", RegexOptions.IgnoreCase)
                    Dim m As Match = r.Match(dec2hex.Text)
                    If m.Success AndAlso m.Value.Length = dec2hex.Text.Length Then
                        rokuyon = Convert.ToInt64(dec2hex.Text) And mask(ComboBox1.SelectedIndex)
                        If rokuyon < 2147483648 AndAlso rokuyon > -2147483649 Then
                            int = Convert.ToInt32(rokuyon)
                            value.Text = "0x" & Convert.ToString(int, 16).PadLeft(value.MaxLength - 2, "0").ToUpper
                        Else
                            MessageBox.Show("please input vaid value,integer range;-2147483648 ～ 2147483647", "TOOBIG/SMALL value")
                        End If
                    Else
                        MessageBox.Show("please input vaid value", "BAD value")
                    End If
                Case 3

                    Dim r As New Regex("^-?\d+.?\d+", RegexOptions.IgnoreCase)
                    Dim m As Match = r.Match(dec2hex.Text)
                    If m.Success AndAlso m.Value.Length = dec2hex.Text.Length Then
                        float = Convert.ToSingle(dec2hex.Text)
                        Dim h As Byte() = BitConverter.GetBytes(float)
                        value.Text = "0x"
                        For i = 0 To 3
                            value.Text &= Convert.ToString(CInt(h(3 - i)), 16).PadLeft(2, "0").ToUpper
                        Next
                    Else
                        MessageBox.Show("please input vaid value", "BAD value")
                    End If
            End Select
        Else
            MessageBox.Show("please input vaid value", "None value")
        End If


    End Sub

    Private Sub val_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles value.KeyPress 'input only 0123456789 ABCDEF abcdef x BACKSPACE=0x08
        If (System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar, "[^0-9a-fA-F\x08]")) Then
            e.Handled = True
            SystemSounds.Beep.Play()
        End If

    End Sub

    Private Sub decval_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dec2hex.KeyPress

        Select ComboBox1.SelectedIndex
            Case 3
                If (e.KeyChar < "0"c Or e.KeyChar > "9"c) AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> vbBack Then
                    e.Handled = True
                    SystemSounds.Beep.Play()
                End If
            Case Else
                If (e.KeyChar < "0"c Or e.KeyChar > "9"c) AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> vbBack Then
                    e.Handled = True
                    SystemSounds.Beep.Play()
                End If
        End Select
    End Sub



    Private Sub decval_validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dec2hex.Validated
        decvaidcheck()
    End Sub

    Private Sub dec2hex_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dec2hex.TextChanged
        decvaidcheck()
    End Sub

    Function decvaidcheck() As Boolean

        Dim y, z As Integer
        y = dec2hex.Text.IndexOf("-")
        z = dec2hex.Text.LastIndexOf("-")
        If z > 0 Then
            dec2hex.Text = dec2hex.Text.Replace("-", "")
            If y = 0 Then
                dec2hex.Text = "-" & dec2hex.Text
            End If
        End If

        Select Case ComboBox1.SelectedIndex
            Case 3
                dec2hex.Text = dec2hex.Text.Replace("-.", "-")
                While (True)
                    y = dec2hex.Text.IndexOf(".")
                    z = dec2hex.Text.LastIndexOf(".")
                    If y = z AndAlso y <> 0 Then
                        Exit While
                    End If
                    If y > 0 Then
                        dec2hex.Text = dec2hex.Text.Substring(0, z) & dec2hex.Text.Substring(z + 1, dec2hex.Text.Length - z - 1).Replace(".", "")
                    Else
                        dec2hex.Text = dec2hex.Text.Substring(0, y).Replace(".", "") & dec2hex.Text.Substring(y + 1, dec2hex.Text.Length - y - 1)
                    End If
                End While
        End Select

        Return True
    End Function

    Private Sub TextBox1_KeyPress(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyPressEventArgs)

        'input only 0123456789 ABCDEF abcdef x BACKSPACE=0x08
        Dim r As New System.Text.RegularExpressions.Regex("[^0-9a-fA-Fx\x08]")

        If r.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class
