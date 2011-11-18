Public Class Form4


    Private Sub form4load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Settings.daemonfinder = True Then
            daemonfinder.Checked = True
            GroupBox1.Enabled = True
        Else
            daemonfinder.Checked = False
            GroupBox1.Enabled = False
        End If
        If My.Settings.wait = True Then
            customwait.Checked = True
        Else
            customwait.Checked = False
        End If

        Dim sta As String() = My.Settings.dhcpstart.Split("."c)
        Dim en As String() = My.Settings.dhcpend.Split("."c)
        Dim k As Integer = 0
        For Each s In sta
            sta(k) = s.ToString.Replace(".", "")
            k += 1
        Next
        k = 0
        For Each s In en
            en(k) = s.ToString.Replace(".", "")
            k += 1
        Next
        IP1.Text = sta(0)
        IP2.Text = sta(1)
        IP3.Text = sta(2)
        IP4.Text = sta(3)
        IP5.Text = en(0)
        IP6.Text = en(1)
        IP7.Text = en(2)
        IP8.Text = en(3)
        Dim z As Integer = My.Settings.second
        wait.Text = My.Settings.second.ToString


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim trans As String
        If IP1.Text = "" Then
            IP1.Text = "0"
        End If
        If IP2.Text = "" Then
            IP2.Text = "0"
        End If
        If IP3.Text = "" Then
            IP3.Text = "0"
        End If
        If IP4.Text = "" Then
            IP4.Text = "0"
        End If
        If IP5.Text = "" Then
            IP5.Text = "0"
        End If
        If IP6.Text = "" Then
            IP6.Text = "0"
        End If
        If IP7.Text = "" Then
            IP7.Text = "0"
        End If
        If IP8.Text = "" Then
            IP8.Text = "0"
        End If

        Dim IPval1 As Integer = CInt(IP1.Text)
        Dim IPval2 As Integer = CInt(IP2.Text)
        Dim IPval3 As Integer = CInt(IP3.Text)
        Dim IPval4 As Integer = CInt(IP4.Text)
        Dim IPval5 As Integer = CInt(IP5.Text)
        Dim IPval6 As Integer = CInt(IP6.Text)
        Dim IPval7 As Integer = CInt(IP7.Text)
        Dim IPval8 As Integer = CInt(IP8.Text)
        Dim statotal As Integer = (IPval2 << 16) + (IPval3 << 8) + IPval4
        Dim endtotal As Integer = (IPval6 << 16) + (IPval7 << 8) + IPval8

        If ((IPval1.ToString = "192" AndAlso IPval2.ToString = "168") Or _
            (IPval1.ToString = "172" AndAlso IPval2 > 15 AndAlso IPval2 < 32 AndAlso IPval6 > 15 AndAlso IPval6 < 32) _
            Or IPval1.ToString = "10") AndAlso IP1.Text = IP5.Text AndAlso statotal <= endtotal _
            AndAlso IPval2 < 256 AndAlso IPval3 < 256 _
            AndAlso IPval4 < 256 AndAlso IPval6 < 256 _
            AndAlso IPval7 < 256 AndAlso IPval8 < 256 Then
            My.Settings.dhcpstart = IPval1.ToString & "." & IPval2.ToString & "." & IPval3.ToString & "." & IPval4.ToString
            My.Settings.dhcpend = IPval5.ToString & "." & IPval6.ToString & "." & IPval7.ToString & "." & IPval8.ToString
            Me.Close()
        ElseIf statotal > endtotal Then
            If My.Application.Culture.Name = "ja-JP" Then
                trans = My.Resources.s16
            Else
                trans = My.Resources.s16_e
            End If
            MessageBox.Show(Me, trans, "INVAIDIP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            'IP
            If My.Application.Culture.Name = "ja-JP" Then
                trans = My.Resources.s13
            Else
                trans = My.Resources.s13_e
            End If
            MessageBox.Show(Me, trans, "INVAIDIP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub daemonfinder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles daemonfinder.CheckedChanged
        If daemonfinder.Checked = True Then
            My.Settings.daemonfinder = True
            GroupBox1.Enabled = True
        Else
            GroupBox1.Enabled = False
            My.Settings.daemonfinder = False
        End If
    End Sub

    Private Sub customwait_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles customwait.CheckedChanged
        If customwait.Checked = True Then
            My.Settings.wait = True
        Else
            My.Settings.wait = False
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
      Handles IP1.KeyPress, IP2.KeyPress, IP3.KeyPress, IP4.KeyPress, IP5.KeyPress, IP6.KeyPress, IP7.KeyPress, IP8.KeyPress, wait.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub IP1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IP1.TextChanged
        IP5.Text = IP1.Text
        If IP1.Text = "192" Then
            IP2.Text = "168"
            IP6.Text = "168"
        End If
    End Sub

    Private Sub IP5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IP5.TextChanged
        IP1.Text = IP5.Text
        If IP5.Text = "192" Then
            IP2.Text = "168"
            IP6.Text = "168"
        End If
    End Sub

    Private Sub wait_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wait.TextChanged
        Dim z As Integer = CInt(wait.Text)
        My.Settings.second = z
    End Sub
End Class