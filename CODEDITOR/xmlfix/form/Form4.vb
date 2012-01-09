Imports System
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Form4

    Private Sub ffload(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        If My.Settings.ask = True Then
            alwaysencode.Checked = True
        End If

        Dim ss As String() = My.Settings.namepattern.Split(CChar(vbLf))
        cmname.Items.Clear()
        For Each s As String In ss
            cmname.Items.Add(s.Trim)
        Next
        Dim sss As String() = My.Settings.descpattern.Split(CChar(vbLf))
        cmdescription.Items.Clear()
        For Each s As String In sss
            cmdescription.Items.Add(s.Trim)
        Next
        Dim ssss As String() = My.Settings.romnamepattern.Split(CChar(vbLf))
        romname.Items.Clear()
        For Each s As String In ssss
            romname.Items.Add(s.Trim)
        Next

        cmname.MaxLength = 77
        cmdescription.MaxLength = 77
        romname.MaxLength = 77
        cmname.Text = My.Settings.cmname
        cmdescription.Text = My.Settings.cmdesc
        romname.Text = My.Settings.cmromname

        Dim codepage As String = My.Settings.encode.ToString
        For i = 0 To 10
            If i = 10 Then
                codepage_select.SelectedIndex = 1
            End If
            codepage_select.SelectedIndex = i
            If codepage_select.Text.Contains(codepage) Then
                Exit For
            End If
        Next
        preview()
    End Sub

    Private Sub neverKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles codepage_select.KeyPress, cmdescription.KeyPress, romname.KeyPress, cmname.KeyPress, TextBox1.KeyPress
        If sender Is codepage_select Then
            e.Handled = True
        ElseIf sender Is TextBox1 Then
            e.Handled = True
        Else
            Dim mask As New Regex("[<>\|\*\?:/\\]", RegexOptions.ECMAScript)
            Dim m As Match = mask.Match(e.KeyChar)
            If m.Success Then
                e.Handled = True
                Beep()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        My.Settings.cmname = cmname.Text
        My.Settings.cmdesc = cmdescription.Text
        My.Settings.cmromname = romname.Text

        Dim en As New Regex("\([0-9]+\)", RegexOptions.ECMAScript)
        Dim m As Match = en.Match(codepage_select.Text)
        My.Settings.encode = CInt(m.Value.Substring(1, m.Value.Length - 2))
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim f As New Form5
        f.Text = "name"
        f.ShowDialog()
        ffload(sender, e)
        f.Dispose()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Dim f As New Form5
        f.Text = "description"
        f.ShowDialog()
        ffload(sender, e)
        f.Dispose()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click

        Dim f As New Form5
        f.Text = "romname"
        f.ShowDialog()
        ffload(sender, e)
        f.Dispose()
    End Sub

    Private Sub cmname_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmname.SelectedIndexChanged, cmdescription.SelectedIndexChanged, romname.SelectedIndexChanged, cmdescription.TextChanged, romname.TextChanged, cmname.SelectedIndexChanged, cmname.TextChanged
        preview()
        If DirectCast(sender, System.Windows.Forms.ComboBox).Name = "romname" Then
            TextBox1.SelectionStart = TextBox1.Text.Length - 1
        End If
    End Sub
    Function preview() As Boolean
        Dim base As String = My.Resources.preview
        Dim xml As String() = {My.Resources.title, My.Resources.gid, My.Resources.crc, My.Resources.size}

        base = base.Replace("s1", tempsrp(cmname.Text, xml))
        base = base.Replace("s2", tempsrp(cmdescription.Text, xml))
        base = base.Replace("s3", xml(1))
        base = base.Replace("s4", tempsrp(romname.Text, xml))
        base = base.Replace("s5", xml(3))
        base = base.Replace("s6", xml(2))

        TextBox1.Text = base
        Return True
    End Function

    Function tempsrp(ByVal base As String, ByVal xml As String()) As String
        If base.Contains("%t") Then
            base = base.Replace("%t", xml(0))
        End If
        If base.Contains("%g") Then
            base = base.Replace("%g", xml(1))
        End If
        If base.Contains("%c") Then
            base = base.Replace("%c", xml(2))
        End If
        If base.Contains("%f") Then
            base = base.Replace("%f", xml(3))
        End If
        If base.Length > 256 Then
            base = base.Substring(0, 255)
        End If
        Return base
    End Function

    Private Sub alwaysencode_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles alwaysencode.CheckedChanged

        If alwaysencode.Checked = True Then
            My.Settings.ask = True
        Else
            My.Settings.ask = False
        End If
    End Sub
End Class