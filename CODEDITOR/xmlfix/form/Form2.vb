Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net

Public Class Form2

    Private Sub ffload(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.mask
        TextBox2.Text = My.Settings.mask2
        If My.Settings.cmmask = False Then
            RadioButton2.Checked = True
        End If
        If My.Settings.crcblock = True Then
            crcblock.Checked = True
        End If
        If My.Settings.cvt_country_lang = True Then
            cvt_country_lang.Checked = True
        End If
        If My.Settings.usexml = True Then
            USEXML.Checked = True
        End If
        If My.Settings.ask = True Then
            alwaysencode.Checked = True
        End If
        If My.Settings.custom_country = True Then
            RadioButton4.Checked = True
        End If
        If My.Settings.custom_lang = True Then
            RadioButton6.Checked = True
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

        cs_country.Items.Clear()
        Dim files As String = "lang\country"
        For x = 0 To 9
            If File.Exists(files & x.ToString & ".txt") Then
                cs_country.Items.Add("カスタム国" & x.ToString)
                If x = CInt(My.Settings.custom_country_num) Then
                    cs_country.Text = "カスタム国" & x.ToString
                End If
            Else
                Exit For
            End If
        Next

        cs_lang.Items.Clear()
        files = "lang\lang"
        For x = 0 To 9
            If File.Exists(files & x.ToString & ".txt") Then
                cs_lang.Items.Add("カスタム言語" & x.ToString)
                If x = CInt(My.Settings.custom_lang_num) Then
                    cs_lang.Text = "カスタム言語" & x.ToString
                End If
            Else
                Exit For
            End If
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

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        My.Settings.mask = TextBox1.Text
        My.Settings.mask2 = TextBox2.Text
        If RadioButton1.Checked = True Then
            My.Settings.cmmask = True
        Else
            My.Settings.cmmask = False
        End If
        If crcblock.Checked = True Then
            My.Settings.crcblock = True
        Else
            My.Settings.crcblock = False
        End If
        If cvt_country_lang.Checked = True Then
            My.Settings.cvt_country_lang = True
        Else
            My.Settings.cvt_country_lang = False
        End If
        If RadioButton4.Checked = True Then
            My.Settings.custom_country = True
        Else
            My.Settings.custom_country = False
        End If
        If RadioButton6.Checked = True Then
            My.Settings.custom_lang = True
        Else
            My.Settings.custom_lang = False
        End If

        If alwaysencode.Checked = True Then
            My.Settings.ask = True
        Else
            My.Settings.ask = False
        End If
        If USEXML.Checked = True Then
            My.Settings.usexml = True
        Else
            My.Settings.usexml = False
        End If

        My.Settings.cmname = cmname.Text
        My.Settings.cmdesc = cmdescription.Text
        My.Settings.cmromname = romname.Text

        Dim en As New Regex("\([0-9]+\)", RegexOptions.ECMAScript)
        Dim m As Match = en.Match(codepage_select.Text)
        My.Settings.encode = CInt(m.Value.Substring(1, m.Value.Length - 2))

        Me.Close()
    End Sub

    Private Sub neverKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles codepage_select.KeyPress, cmdescription.KeyPress, romname.KeyPress, cmname.KeyPress, TextBox1.KeyPress, cs_country.KeyPress, cs_lang.KeyPress
        If sender Is codepage_select Then
            e.Handled = True
        ElseIf sender Is TextBox3 Then
            e.Handled = True
        ElseIf sender Is cs_country Then
            e.Handled = True
        ElseIf sender Is cs_lang Then
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
        'If DirectCast(sender, System.Windows.Forms.ComboBox).Name = "romname" Then
        '    TextBox3.SelectionStart = TextBox1.Text.Length - 1
        'End If
    End Sub

    Function preview() As Boolean
        Dim base As String = My.Resources.preview
        Dim xml As String() = {My.Resources.title, My.Resources.gid, My.Resources.crc, My.Resources.size, _
                         My.Resources.boxid, My.Resources.fwver, My.Resources.diskver, My.Resources.rdate, _
                            My.Resources.mk, My.Resources.comment, "Japan", "Japanese"}
        'Dim country As Integer() = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25}
        Dim countryname As String() = {"Europe", "USA", "Germany", "China", "Spain", "France", "Italy", "Japan", "Netherlands", "England", "Denmark", "Finland", "Norway", "Poland", "Portugal", "Sweden", "Europe USA", "Europe USA Japan", "USA Japan", "Australia", "North Korea", "Brazil", "South Korea", "Europe Brazil", "Europe USA Brazil", "USA Brazil"}
        'Dim lang As Integer() = {0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536}
        Dim langname As String() = {"nothing", "French", "English (US)", "Chinese", "Danish", "Dutch", "Finland", "German", "Italian", "Japanese", "Norwegian", "Polish", "Portuguese", "Spanish", "Swedish", "English (UK)", "Portuguese (BR)", "Korean"}
        If RadioButton4.Checked = True Then
            Dim cntxt As String = "lang\country" & My.Settings.custom_country_num & ".txt"
            If File.Exists(cntxt) Then
                Dim sr As New System.IO.StreamReader(cntxt, System.Text.Encoding.GetEncoding(65001))
                Dim y As Integer = 0
                While sr.Peek() > -1
                    countryname(y) = sr.ReadLine
                    y += 1
                    If y = 26 Then
                        Exit While
                    End If
                End While
                sr.Close()
            End If
        End If
        If RadioButton6.Checked = True Then
            Dim lntxt As String = "lang\lang" & My.Settings.custom_lang_num & ".txt"
            If File.Exists(lntxt) Then
                Dim sr As New System.IO.StreamReader(lntxt, System.Text.Encoding.GetEncoding(65001))
                Dim x As Integer = 0
                While sr.Peek() > -1
                    langname(x) = sr.ReadLine
                    x += 1
                    If x = 18 Then
                        Exit While
                    End If
                End While
                sr.Close()
            End If
        End If


        Dim s As String = ""
        Dim num As Integer = 0

        If USEXML.Checked = True Then
            Array.Clear(xml, 0, 11)
            If File.Exists(My.Settings.lastxml) = True Then
                Try
                    Dim reader As System.Xml.XmlReader = System.Xml.XmlReader.Create(My.Settings.lastxml)
                    Dim xml_parse As Boolean = False
                    While reader.Read
                        If reader.NodeType = System.Xml.XmlNodeType.EndElement AndAlso reader.LocalName = "configuration" Then
                            xml_parse = True
                        End If
                        If reader.NodeType = System.Xml.XmlNodeType.Element AndAlso xml_parse = True Then
                            If reader.LocalName = "imageNumber" Then

                            End If
                            If reader.LocalName = "releaseNumber" Then

                            End If
                            If reader.LocalName = "title" Then
                                xml(0) = reader.ReadString()
                            End If
                            If reader.LocalName = "saveType" Then
                                s = reader.ReadString()
                                Dim rd As New Regex("\d{4}/\d{1,2}/\d{1,2}", RegexOptions.ECMAScript)
                                Dim rdm As Match = rd.Match(s)
                                If rdm.Success Then
                                    xml(7) = rdm.Value
                                End If
                            End If
                            If reader.LocalName = "romSize" Then
                                xml(3) = reader.ReadString()
                            End If
                            If reader.LocalName = "publisher" Then
                                xml(8) = reader.ReadString()
                            End If
                            If reader.LocalName = "location" Then
                                s = reader.ReadString()
                                num = CInt(s) Mod 26
                                xml(10) = countryname(num)
                            End If
                            If reader.LocalName = "sourceRom" Then

                            End If
                            If reader.LocalName = "language" Then
                                s = reader.ReadString()
                                num = CInt(s)
                                s = ""
                                If num = 0 Then
                                    s = langname(0)
                                Else
                                    For i = 0 To 16
                                        If (num And (1 << i)) <> 0 Then
                                            s &= langname(i + 1) & vbTab
                                        End If
                                    Next
                                    s = s.Replace(vbTab, " - ")
                                    s = s.Remove(s.Length - 3, 3)
                                End If
                                xml(11) = s
                            End If
                            If reader.LocalName = "romCRC" Then
                                xml(2) = reader.ReadString()
                            End If
                            If reader.LocalName = "im1CRC" Then
                                s = reader.ReadString()
                                Dim d As New Regex("[A-Z]{4}[0-9]{5}", RegexOptions.ECMAScript)
                                Dim dd As Match = d.Match(s)
                                Dim bi As New Regex("[A-Z]{4}-[0-9]{5}", RegexOptions.ECMAScript)
                                Dim bidm As Match = bi.Match(s)
                                If bidm.Success Then
                                    xml(4) = dd.Value
                                End If
                                If dd.Success Then
                                    xml(1) = dd.Value
                                End If
                            End If
                            If reader.LocalName = "im2CRC" Then
                                Dim fv As New Regex("\d\.\d\d / \d.\d\d", RegexOptions.ECMAScript)
                                s = reader.ReadString()
                                Dim fvm As Match = fv.Match(s)
                                If fvm.Success Then
                                    xml(5) = fvm.Value.Substring(0, 4)
                                    xml(6) = fvm.Value.Substring(fvm.Value.Length - 4, 4)
                                End If

                            End If
                            If reader.LocalName = "comment" Then
                                xml(9) = reader.ReadString()
                            End If
                            If reader.LocalName = "duplicateID" Then

                            End If
                        End If
                        If reader.NodeType = System.Xml.XmlNodeType.EndElement AndAlso reader.LocalName = "game" Then
                            Exit While
                        End If
                    End While

                    'リーダーを閉じる
                    reader.Close()

                Catch ex As System.Xml.XmlException
                    MessageBox.Show(ex.Message)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            End If
        End If

        base = base.Replace("s1", tempsrp(cmname.Text, xml).Replace("/", ""))
        base = base.Replace("s2", tempsrp(cmdescription.Text, xml))
        base = base.Replace("s3", xml(1))
        base = base.Replace("s4", tempsrp(romname.Text, xml).Replace("/", ""))
        base = base.Replace("s5", xml(3))
        base = base.Replace("s6", xml(2))

        TextBox3.Text = base
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
        If base.Contains("%b") Then
            base = base.Replace("%b", xml(4))
        End If
        If base.Contains("%w") Then
            base = base.Replace("%w", xml(5))
        End If
        If base.Contains("%v") Then
            base = base.Replace("%v", xml(6))
        End If
        If base.Contains("%r") Then
            base = base.Replace("%r", xml(7))
        End If
        If base.Contains("%p") Then
            base = base.Replace("%p", xml(8))
        End If
        If base.Contains("%m") Then
            base = base.Replace("%m", xml(9))
        End If
        '国
        If base.Contains("%o") Then
            base = base.Replace("%o", xml(10))
        End If
        '言語
        If base.Contains("%a") Then
            base = base.Replace("%a", xml(11))
        End If
        If base.Length > 256 Then
            base = base.Substring(0, 255)
        End If
        Return base
    End Function

    Private Sub USEXML_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles USEXML.CheckedChanged
        preview()
    End Sub


    Private Sub cs_country_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cs_country.SelectedIndexChanged
        My.Settings.custom_country_num = cs_country.SelectedIndex.ToString
    End Sub

    Private Sub cs_lang_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cs_lang.SelectedIndexChanged
        My.Settings.custom_lang_num = cs_lang.SelectedIndex.ToString
    End Sub

    Private Sub edit_lang_Click(sender As System.Object, e As System.EventArgs) Handles edit_lang.Click
        Dim f As New Form4
        Dim s As String = "lang\lang" & My.Settings.custom_lang_num & ".txt"
        If File.Exists(s) Then
            f.TextBox2.Text = s
            f.ShowDialog()
            f.Dispose()
        Else
            MessageBox.Show(s & "がみつかりません")
        End If
    End Sub


    Private Sub edit_country_Click(sender As System.Object, e As System.EventArgs) Handles edit_country.Click
        Dim f As New Form4
        Dim s As String = "lang\country" & My.Settings.custom_country_num & ".txt"
        If File.Exists(s) Then
            f.TextBox2.Text = s
            f.ShowDialog()
            f.Dispose()
        Else
            MessageBox.Show(s & "がみつかりません")
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles crcblock.CheckedChanged

    End Sub
End Class