

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
        Select Case sender.SelectedIndex

            Case 0 '8 Bit
                max = 2

            Case 1 '16 Bit
                max = 4

            Case 2 '32 Bit
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
End Class
