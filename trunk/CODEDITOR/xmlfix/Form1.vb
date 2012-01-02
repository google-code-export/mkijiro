Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net

Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Dim sr As New System.IO.StreamReader(Application.StartupPath & "\nointro.txt", _
                System.Text.Encoding.GetEncoding(932))
            Dim s As String = ""
            Dim ss As String = ""
            Dim fn As String = "NOINTRO_FIX"
            Dim index(1000) As Integer
            Dim goodcrc(1000) As String
            'B9D68496 > FD305564  0003
            Dim r As New Regex("[0-9A-F]{8} > [0-9A-F]{8}  \d{4}", RegexOptions.ECMAScript)
            Dim m As Match
            Dim i As Integer
            While sr.Peek() > -1
                s = sr.ReadLine()
                m = r.Match(s)
                If m.Success Then
                    index(i) = CInt(m.Value.Remove(0, m.Value.Length - 4))
                    goodcrc(i) = m.Value.Substring(11, 8)
                    i += 1
                End If
            End While
            sr.Close()

            Dim ofd As New OpenFileDialog()
            ofd.Filter = "XMLファイル(*.xml;)|*.xml"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim sr2 As New System.IO.StreamReader(ofd.FileName, _
                    System.Text.Encoding.GetEncoding(65001))
                Dim q As New Regex("<releaseNumber>\d+</releaseNumber>", RegexOptions.ECMAScript)
                Dim n As Match
                Dim p As New Regex(My.Settings.mask, RegexOptions.ECMAScript)
                Dim l As Match
                Dim cr As New Regex("[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                Dim crc As Match
                Dim srr As New Regex("<sourceRom>.*?</sourceRom>", RegexOptions.ECMAScript)
                Dim srm As Match
                Dim str As New StringBuilder
                Dim rplace As Boolean = False
                Dim filer As Boolean = False
                i = 0
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
                    srm = srr.Match(s)
                    If s.Contains("Sony PSP Collection 日本語版 CE") Then
                        MessageBox.Show("FILERSORCEです、パッチの必要はありません")
                        sr2.Close()
                        Exit Sub
                    End If
                    If s.Contains("<datVersion>") Then
                        s = vbTab & vbTab & "<datVersion>999</datVersion>"
                    End If

                    If n.Success Then
                        rplace = False
                        ss = n.Value.Remove(0, 15)
                        ss = ss.Remove(ss.Length - 16, 16)
                        While CDbl(ss) > index(i)
                            If index(i) = 0 Then
                                Exit While
                            End If
                            i += 1
                        End While

                        If CDbl(ss) = index(i) Then
                            rplace = True
                        End If
                    End If
                    If rplace = True Then
                        If srm.Success Then
                            ss = srm.Value.Remove(0, 11)
                            ss = ss.Remove(ss.Length - 12, 12)
                            s = s.Replace(ss, fn)
                        End If
                        If l.Success Then
                            crc = cr.Match(l.Value)
                            s = s.Replace(crc.Value, goodcrc(i))
                            i += 1
                        End If
                    End If
                    str.AppendLine(s)
                End While
                sr2.Close()
                Dim utf8NoBOM As New UTF8Encoding()
                Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "(nointro_fix).xml",
                    False, utf8NoBOM)
                wr.Write(str.ToString)
                wr.Close()
                Beep()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Try
            Dim sr As New System.IO.StreamReader(Application.StartupPath & "\redump.txt", _
                System.Text.Encoding.GetEncoding(932))
            Dim s As String = ""
            Dim ss As String = ""
            Dim fn As String = "REDUMP_FIX"
            Dim index(1000) As Integer
            Dim goodcrc(1000) As String
            '0105 - Yarudora_Portable_Yukiwari_no_Hana 	Missing last 2kb 	2E37A384 	B8F9B04E 
            Dim r As New Regex("[0-9]{4}.+[0-9A-F]{8}[\x20\t]{1,2}[0-9A-F]{8}", RegexOptions.ECMAScript)
            Dim m As Match
            Dim i As Integer
            While sr.Peek() > -1
                s = sr.ReadLine()
                m = r.Match(s)
                If m.Success Then
                    index(i) = CInt(m.Value.Substring(0, 4))
                    goodcrc(i) = m.Value.Substring(m.Value.Length - 8, 8)
                    i += 1
                End If
            End While
            sr.Close()

            Dim ofd As New OpenFileDialog()
            ofd.Filter = "XMLファイル(*.xml;)|*.xml"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim sr2 As New System.IO.StreamReader(ofd.FileName, _
                    System.Text.Encoding.GetEncoding(65001))
                Dim q As New Regex("<releaseNumber>\d+</releaseNumber>", RegexOptions.ECMAScript)
                Dim n As Match
                Dim mask As String = My.Settings.mask2
                Dim p As New Regex(mask, RegexOptions.ECMAScript)
                Dim l As Match
                Dim cr As New Regex("[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                Dim crc As Match
                Dim srr As New Regex("<sourceRom>.*?</sourceRom>", RegexOptions.ECMAScript)
                Dim srm As Match
                Dim str As New StringBuilder
                Dim rplace As Boolean = False
                Dim filer As Boolean = False
                i = 0
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
                    srm = srr.Match(s)
                    If s.Contains("Sony PSP Collection 日本語版 CE") Then
                        MessageBox.Show("FILERSORCEです、パッチの必要はありません")
                        sr2.Close()
                        Exit Sub
                    End If
                    If s.Contains("<datVersion>") Then
                        s = vbTab & vbTab & "<datVersion>999</datVersion>"
                    End If

                    If n.Success Then
                        rplace = False
                        ss = n.Value.Remove(0, 15)
                        ss = ss.Remove(ss.Length - 16, 16)
                        While CDbl(ss) > index(i)
                            If index(i) = 0 Then
                                Exit While
                            End If
                            i += 1
                        End While


                        If CDbl(ss) = index(i) Then
                            rplace = True
                        End If
                    End If
                    If rplace = True Then
                        If srm.Success Then
                            ss = srm.Value.Remove(0, 11)
                            ss = ss.Remove(ss.Length - 12, 12)
                            s = s.Replace(ss, fn)
                        End If
                        If l.Success Then
                            crc = cr.Match(l.Value)
                            s = s.Replace(crc.Value, goodcrc(i))
                            i += 1
                        End If
                    End If
                    str.AppendLine(s)
                End While
                sr2.Close()
                Dim utf8NoBOM As New UTF8Encoding()
                Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "(redump_fix).xml",
                    False, utf8NoBOM)
                wr.Write(str.ToString)
                wr.Close()
                Beep()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Dim sb As New StringBuilder
            Dim s As String = ""
            Dim romcode As String() = {" [o]", " []", " [b]", " [!]"}
            Dim emphash As String() = {"00000000", "000000000000000000000000", "000000000000000000000000000000000"}
            Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
            Dim title(10000) As String
            Dim size(10000) As String
            Dim crc(10000) As String
            Dim id(10000) As String
            Dim ppath As String = ""
            Dim i As Integer = 0
            Dim ext As String = ".iso"
            Dim datname As String = System.Environment.ExpandEnvironmentVariables("%username%") & "'s dat"



            Dim ofd As New OpenFileDialog()
            ofd.Filter = "XMLファイル(*.xml;)|*.xml"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim sr2 As New System.IO.StreamReader(ofd.FileName, _
                    System.Text.Encoding.GetEncoding(65001))
                Dim q As New Regex("<title>.*?</title>", RegexOptions.ECMAScript)
                Dim n As Match
                Dim r As New Regex("<romSize>\d+</romSize>", RegexOptions.ECMAScript)
                Dim t As Match
                Dim d As New Regex("[A-Z]{4}-[0-9]{5}", RegexOptions.ECMAScript)
                Dim dd As Match
                Dim cr As New Regex("[0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                Dim crr As Match
                Dim mask As String = My.Settings.mask
                If My.Settings.cmmask = False Then
                    mask = My.Settings.mask2
                End If
                Dim p As New Regex(mask, RegexOptions.ECMAScript)
                Dim l As Match
                Dim ex As New Regex(""".*?""", RegexOptions.ECMAScript)
                Dim exx As Match
                Dim str As New StringBuilder
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
                    t = r.Match(s)
                    dd = d.Match(s)
                    If s.Contains("<datName>") Then
                        datname = s.Replace("<datName>", "")
                        datname = datname.Replace("</datName>", "")
                        datname = doskiller(datname.Trim)
                    End If
                    If n.Success Then
                        s = n.Value.Remove(0, 7)
                        s = s.Remove(s.Length - 8, 8)
                        title(i) = doskiller(s)
                    ElseIf t.Success Then
                        s = t.Value.Remove(0, 9)
                        s = s.Remove(s.Length - 10, 10)
                        size(i) = s
                    ElseIf l.Success Then
                        crr = cr.Match(l.Value)
                        crc(i) = crr.Value
                        exx = ex.Match(l.Value)
                        If exx.Success Then
                            ext = exx.Value.Replace("""", "")
                        End If
                    ElseIf dd.Success Then
                        id(i) = dd.Value
                    ElseIf s.Contains("</game>") Then
                        i += 1
                    End If
                End While
                sr2.Close()
                ppath = ofd.FileName
            End If

            Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ppath) & "\" & Path.GetFileNameWithoutExtension(ppath) & "_convert.dat",
                    False, System.Text.Encoding.GetEncoding(932))

            sb.AppendLine("clrmamepro (")
            sb.Append(vbTab)
            sb.Append("name """)
            sb.Append(datname)
            sb.AppendLine("""")
            sb.Append(vbTab)
            sb.Append("description """)
            sb.Append(datname)
            sb.AppendLine("""")
            sb.Append(vbTab)
            sb.Append("version ")
            sb.AppendLine(Now.ToString.Replace("/", "").Substring(0, 8))
            sb.Append(vbTab)
            sb.Append("comment """)
            sb.Append(System.Environment.ExpandEnvironmentVariables("%username%"))
            sb.AppendLine("""")
            sb.AppendLine(")")
            sb.AppendLine()

            Dim z As Integer = i

            For i = 0 To z - 1
                sb.AppendLine("game (")
                sb.Append(vbTab)
                sb.Append("name """)
                sb.Append(title(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("description """)
                sb.Append(title(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("serial """)
                sb.Append(id(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("rom ( name """)
                sb.Append(title(i))
                sb.Append(ext)
                sb.Append(""" size ")
                sb.Append(size(i))
                sb.Append(" crc ")
                sb.Append(crc(i))
                If size(i) = "0" Then
                    sb.Append(flag(2))
                End If
                'sb.Append(" md5 ")
                'sb.Append(hash(1))
                'sb.Append(" sha1 ")
                'sb.Append(hash(2))
                sb.AppendLine(" )")
                sb.AppendLine(")")
                sb.AppendLine()
            Next

            wr.Write(sb.ToString)
            wr.Close()
            Beep()


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Try
            Dim sb As New StringBuilder
            Dim s As String = ""
            Dim emphash As String() = {"00000000", "000000000000000000000000", "000000000000000000000000000000000"}
            Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
            Dim title(10000) As String
            Dim desk(10000) As String
            Dim rom(10000) As String
            Dim id(10000) As String
            Dim opath As String = ""
            Dim ppath As String = ""
            Dim tmp As String = ""
            Dim i As Integer = 0

            Dim ofd As New OpenFileDialog()
            ofd.Filter = "DATファイル(*.dat;)|*.dat"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim sr2 As New System.IO.StreamReader(ofd.FileName, _
                    System.Text.Encoding.GetEncoding(932))
                opath = ofd.FileName
                If ofd.ShowDialog() = DialogResult.OK Then
                    Dim sr3 As New System.IO.StreamReader(ofd.FileName, _
                        System.Text.Encoding.GetEncoding(932))
                    Dim ss As String = sr3.ReadToEnd()
                    Dim r As New Regex("\tname "".*?""", RegexOptions.ECMAScript)
                    Dim t As Match
                    Dim dd As New Regex("\tserial "".*?""", RegexOptions.ECMAScript)
                    Dim idd As Match
                    Dim a As New Regex("\tdescription "".*?""", RegexOptions.ECMAScript)
                    Dim b As Match
                    Dim c As New Regex("\trom \( name "".*?"" size \d+ crc [0-9A-F]{8}.*?\)", RegexOptions.ECMAScript)
                    Dim d As Match
                    Dim q As New Regex("crc [0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                    Dim n As Match
                Dim str As New StringBuilder
                While sr2.Peek() > -1
                        s = sr2.ReadLine()
                        t = r.Match(s)
                        b = a.Match(s)
                        d = c.Match(s)
                        idd = dd.Match(s)
                        If t.Success Then
                            s = s.Remove(0, 7)
                            title(i) = doskiller(s.Remove(s.Length - 1, 1))
                        ElseIf b.Success Then
                            s = b.Value.Remove(0, 14)
                            s = s.Remove(s.Length - 1, 1)
                            desk(i) = doskiller(s)
                        ElseIf idd.Success Then
                            s = idd.Value.Remove(0, 9)
                            s = s.Remove(s.Length - 1, 1)
                            id(i) = s
                        ElseIf d.Success Then
                            n = q.Match(s)
                            If title(i) = "" Then
                                title(i) = title(i - 1)
                                desk(i) = desk(i - 1)
                                id(i) = id(i - 1)
                            End If
                            rom(i) = s
                            s = n.Value.Remove(0, 4)
                            Dim y As New Regex("crc " & s, RegexOptions.ECMAScript)
                            Dim u As Match = y.Match(ss)
                            If u.Success Then
                                tmp = ss.Remove(u.Index, ss.Length - u.Index)
                                tmp = tmp.Remove(0, tmp.LastIndexOf("description") - 2)
                                b = a.Match(tmp)
                                s = b.Value.Remove(0, 14)
                                s = s.Remove(s.Length - 1, 1)
                                desk(i) = doskiller(s)
                            End If
                            i += 1
                            End If
                    End While
                    sr2.Close()
                    sr3.Close()
                    ppath = ofd.FileName
                End If
            End If

            Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ppath) & "\" & Now.ToString.Replace("/", "").Replace(":", "") & "_merge.dat",
                    False, System.Text.Encoding.GetEncoding(932))

            sb.AppendLine("clrmamepro (")
            sb.Append(vbTab)
            sb.AppendLine("name ""Sony - PlayStation Portable""")
            sb.Append(vbTab)
            sb.AppendLine("description ""Sony - PlayStation Portable""")
            sb.Append(vbTab)
            sb.Append("version ")
            sb.AppendLine(Now.ToString.Replace("/", "").Substring(0, 8))
            sb.Append(vbTab)
            sb.Append("comment """)
            sb.Append(System.Environment.ExpandEnvironmentVariables("%username%"))
            sb.AppendLine("""")
            sb.AppendLine(")")
            sb.AppendLine()

            Dim z As Integer = i

            For i = 0 To z - 1
                sb.AppendLine("game (")
                sb.Append(vbTab)
                sb.Append("name """)
                sb.Append(title(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("description """)
                sb.Append(desk(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("serial """)
                sb.Append(id(i))
                sb.AppendLine("""")
                sb.AppendLine(rom(i))
                'sb.Append("rom ( name """)
                'sb.Append(title(i))
                'sb.Append(".iso"" size ")
                'sb.Append(size(i))
                'sb.Append(" crc ")
                'sb.Append(crc(i))
                'sb.Append(" md5 ")
                'sb.Append(hash(1))
                'sb.Append(" sha1 ")
                'sb.Append(hash(2))
                'sb.AppendLine(" )")
                sb.AppendLine(")")
                sb.AppendLine()
            Next

            wr.Write(sb.ToString)
            wr.Close()
            Beep()


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Try
            Dim sb As New StringBuilder
            Dim sbn As New StringBuilder
            Dim sba As New StringBuilder
            Dim rr As New StringBuilder
            Dim s As String = ""
            Dim emphash As String() = {"00000000", "000000000000000000000000", "000000000000000000000000000000000"}
            Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
            Dim title(10000) As String
            Dim desk(10000) As String
            Dim rom(10000) As String
            Dim id(10000) As String
            Dim opath As String = ""
            Dim ppath As String = ""
            Dim tmp As String = ""
            Dim i As Integer = 0
            Dim ss As String = ""
            Dim s1 As String = ""
            Dim sss As String = ""
            Dim asd As String = ""
            Dim k As Integer = 0
            Dim mi As Integer = 0
            Dim ud As Integer = 0

            sba.AppendLine("clrmamepro (")
            sba.Append(vbTab)
            sba.AppendLine("name ""Sony - PlayStation Portable""")
            sba.Append(vbTab)
            sba.AppendLine("description ""Sony - PlayStation Portable""")
            sba.Append(vbTab)
            sba.Append("version ")
            sba.AppendLine(Now.ToString.Replace("/", "").Substring(0, 8))
            sba.Append(vbTab)
            sba.Append("comment """)
            sba.Append(System.Environment.ExpandEnvironmentVariables("%username%"))
            sba.AppendLine("""")
            sba.AppendLine(")")
            sba.AppendLine()

            Dim ofd As New OpenFileDialog()
            ofd.Filter = "DATファイル(*.dat;)|*.dat"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim sr2 As New System.IO.StreamReader(ofd.FileName, _
                    System.Text.Encoding.GetEncoding(932))
                opath = ofd.FileName
                If ofd.ShowDialog() = DialogResult.OK Then
                    Dim sr3 As New System.IO.StreamReader(ofd.FileName, _
                        System.Text.Encoding.GetEncoding(932))
                    While sr2.Peek() > -1
                        asd = sr2.ReadLine
                        If asd.Contains("size 0") Or asd.Contains(flag(1)) Then
                            ud += 1
                        End If
                        If asd.Contains("重複;") Or asd.Contains("追加;") Or asd.Contains("未ダンプ;") Then
                        Else
                            rr.AppendLine(asd)
                        End If
                    End While
                    ss = rr.ToString
                    Dim r As New Regex("\tname "".*?""", RegexOptions.ECMAScript)
                    Dim t As Match
                    Dim dd As New Regex("\tserial "".*?""", RegexOptions.ECMAScript)
                    Dim idd As Match
                    Dim a As New Regex("\tdescription "".*?""", RegexOptions.ECMAScript)
                    Dim b As Match
                    Dim c As New Regex("\trom \( name "".*?"" size \d+ crc [0-9A-Fa-f]{8}.*?\)", RegexOptions.ECMAScript)
                    Dim d As Match
                    Dim q As New Regex("crc [0-9A-Fa-f]{8}", RegexOptions.ECMAScript)
                    Dim n As Match
                    Dim str As New StringBuilder
                    While sr3.Peek() > -1
                        s = sr3.ReadLine()
                        t = r.Match(s)
                        b = a.Match(s)
                        d = c.Match(s)
                        idd = dd.Match(s)
                        If t.Success Then
                            s = s.Remove(0, 7)
                            title(i) = s.Remove(s.Length - 1, 1)
                        ElseIf b.Success Then
                            s = b.Value.Remove(0, 14)
                            s = s.Remove(s.Length - 1, 1)
                            desk(i) = s
                        ElseIf idd.Success Then
                            s = idd.Value.Remove(0, 9)
                            s = s.Remove(s.Length - 1, 1)
                            id(i) = s
                        ElseIf d.Success Then
                            n = q.Match(s)
                            rom(i) = s
                            s = n.Value.Remove(0, 4)
                            Dim y As New Regex("crc " & s, RegexOptions.ECMAScript)
                            Dim u As Match = y.Match(ss)
                            If u.Success Then
                                    mi += 1
                            Else
                                If rom(i).Contains("size 0 ") AndAlso title(i).Contains("[[[[[[[[[[[[[[[[[[") = False Then
                                    sbn.AppendLine("game (")
                                    sbn.Append(vbTab)
                                    sbn.Append("name """)
                                    sbn.Append(title(i))
                                    sbn.AppendLine("""")
                                    sbn.Append(vbTab)
                                    sbn.Append("description """)
                                    sbn.Append(desk(i))
                                    sbn.AppendLine("""")
                                    sbn.Append(vbTab)
                                    sbn.Append("serial """)
                                    sbn.Append(id(i))
                                    sbn.AppendLine("""")
                                    sbn.Append(rom(i))
                                    If rom(i).Contains("flags ") = False Then
                                        sbn.Insert(sbn.Length - 2, " flags nodump")
                                    End If
                                    sbn.AppendLine()
                                    sbn.AppendLine(")")
                                    sbn.AppendLine()
                                    ud += 1
                                Else
                                    sb.AppendLine("game (")
                                    sb.Append(vbTab)
                                    sb.Append("name """)
                                    sb.Append(title(i))
                                    sb.AppendLine("""")
                                    sb.Append(vbTab)
                                    sb.Append("description """)
                                    sb.Append(desk(i))
                                    sb.AppendLine("""")
                                    sb.Append(vbTab)
                                    sb.Append("serial """)
                                    sb.Append(id(i))
                                    sb.AppendLine("""")
                                    sb.Append(rom(i))
                                    sb.AppendLine()
                                    sb.AppendLine(")")
                                    sb.AppendLine()
                                    k += 1
                                End If
                            End If
                            i += 1
                        End If
                    End While
                    sr2.Close()
                    sr3.Close()
                    ppath = ofd.FileName
                End If
            End If

            sbn.AppendLine("重複;" & mi.ToString)
            sbn.AppendLine("追加;" & k.ToString)
            sbn.AppendLine("未ダンプ;" & ud.ToString)

            Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ppath) & "\" & Now.ToString.Replace("/", "").Replace(":", "") & "_doublecheck.dat",
                    False, System.Text.Encoding.GetEncoding(932))
            wr.Write(ss)
            wr.Write(sb.ToString)
            wr.Write(sbn.ToString)
            wr.Close()

            Beep()


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Function get_list(ByVal url As String) As String

        Dim enc As Encoding = Encoding.GetEncoding("Shift_JIS")

        Dim req As WebRequest = WebRequest.Create(url)
        Dim res As WebResponse = req.GetResponse()

        Dim st As Stream = res.GetResponseStream()
        Dim sr As StreamReader = New StreamReader(st, enc)
        Dim html As String = sr.ReadToEnd()
        sr.Close()
        st.Close()

        Return html
    End Function

    Function doskiller(ByVal s As String) As String
        Dim ss As String() = {"/", "\", "?", "*", ":", "|", """", "<", ">"}
        Dim html As String() = {"&amp;", "&gt;", "&lt;", "&apos;", "&quot;"}
        Dim html2 As String() = {"&", "", "", "'", ""}
        For i = 0 To 4
            s = s.Replace(html(i), html2(i))
        Next

        'Dim a As String = "&#12415;&#12435;&#12394;&#12398;GOLF &#12509;&#12540;&#12479;&#12502;&#12523;"
        Dim htmluni As New Regex("&#\d{1,5};", RegexOptions.ECMAScript)
        Dim uni As Match = htmluni.Match(s)
        Dim k As UInt16 = 0
        Dim b(1) As Byte
        While uni.Success
            k = Convert.ToUInt16(uni.Value.Substring(2, uni.Value.Length - 3))
            b = BitConverter.GetBytes(k)
            s = s.Replace(uni.Value, System.Text.Encoding.GetEncoding(1200).GetString(b))
            uni = uni.NextMatch
        End While

        'Dim ba As String = "&#x30D5;&#x30ED;&#x30E0;&#x30FB;&#x30BD;&#x30D5;&#x30C8;&#x30A6;&#x30A7;&#x30A2;"
        Dim htmluni2 As New Regex("&#x[0-9A-Fa-f]{1,4};", RegexOptions.ECMAScript)
        Dim uni2 As Match = htmluni2.Match(s)
        While uni2.Success
            k = Convert.ToUInt16(uni2.Value.Substring(3, uni2.Value.Length - 4), 16)
            b = BitConverter.GetBytes(k)
            s = s.Replace(uni2.Value, System.Text.Encoding.GetEncoding(1200).GetString(b))
            uni2 = uni2.NextMatch
        End While

        For i = 0 To 8
            s = s.Replace(ss(i), "")
        Next
        Return s
    End Function

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Try
            Dim redump As New StringBuilder
            redump.AppendLine(get_list("http://wiki.redump.org/index.php?title=Sony_PSP_USA_Missing_Games"))
            redump.AppendLine(get_list("http://wiki.redump.org/index.php?title=Sony_PSP_Europe_Missing_Games"))
            redump.AppendLine(get_list("http://wiki.redump.org/index.php?title=Sony_PSP_Korea_Missing_Games"))
            redump.AppendLine(get_list("http://wiki.redump.org/index.php?title=Sony_PSP_Japan_Missing_Games"))
            redump.AppendLine(get_list("http://wiki.redump.org/index.php?title=Sony_PSP_Japan_Best_Of_Missing_Games"))
            redump.AppendLine(get_list("http://wiki.redump.org/index.php?title=Sony_PSP_Japan_Limited_Edition_Missing_Games"))
            Dim www As New System.IO.StreamWriter(Application.StartupPath & "\redump_wiki.txt",
                    False, System.Text.Encoding.GetEncoding(932))
            Dim tag As New System.Text.RegularExpressions.Regex("<.*?>", RegexOptions.ECMAScript)
            Dim temp As String = redump.ToString
            Dim td As System.Text.RegularExpressions.Match = tag.Match(redump.ToString)
            While td.Success
                temp = temp.Replace(td.Value, "")
                td = td.NextMatch()
            End While
            www.Write(temp)
            www.Close()
            Dim sr As New StreamReader(Application.StartupPath & "\redump_wiki.txt", System.Text.Encoding.GetEncoding(932))
            Dim s As String = ""
            Dim ss As String = ""
            Dim index(5000) As String
            Dim name(5000) As String
            Dim wikicrc(5000) As String
            Dim r As New Regex("^[A-Z]{1,4}-[0-9]{3,5}\t.+", RegexOptions.ECMAScript)
            Dim m As Match
            Dim dd As New Regex("^[A-Z]{1,4}-[0-9]{3,5}", RegexOptions.ECMAScript)
            Dim id As Match
            Dim crc32 As New Regex("\[[0-9A-Fa-f]{8}\]", RegexOptions.ECMAScript)
            Dim cr As Match
            Dim i As Integer = 0
            Dim z As Integer = 0
            While sr.Peek() > -1
                s = sr.ReadLine()
                m = r.Match(s)
                id = dd.Match(s)
                If m.Success AndAlso id.Success Then
                    cr = crc32.Match(s)
                    index(i) = id.Value
                    s = m.Value.Remove(0, id.Value.Length)
                    If cr.Success Then
                        ss = cr.Value.Substring(1, 8).ToUpper
                        wikicrc(i) = "crc " & ss
                        s = s.Replace(cr.Value, "")
                    Else
                        wikicrc(i) = "crc " & i.ToString.PadLeft(8, "0"c)
                    End If
                    name(i) = doskiller(s.Trim)
                    i += 1
                End If
            End While
            z = i
            sr.Close()
            Dim sb As New StringBuilder

            sb.AppendLine("clrmamepro (")
            sb.Append(vbTab)
            sb.AppendLine("name ""Sony - PlayStation Portable""")
            sb.Append(vbTab)
            sb.AppendLine("description ""Sony - PlayStation Portable""")
            sb.Append(vbTab)
            sb.Append("version ")
            sb.AppendLine(Now.ToString.Replace("/", "").Substring(0, 8))
            sb.Append(vbTab)
            sb.Append("comment """)
            sb.Append(System.Environment.ExpandEnvironmentVariables("redump_wiki"))
            sb.AppendLine("""")
            sb.AppendLine(")")
            sb.AppendLine()
            For i = 0 To z - 1
                sb.AppendLine("game (")
                sb.Append(vbTab)
                sb.Append("name """)
                sb.Append(name(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("description """)
                sb.Append(name(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("serial """)
                sb.Append(index(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("rom ( name """)
                sb.Append(name(i))
                sb.Append(".iso"" size 0 ")
                sb.Append(wikicrc(i))
                sb.AppendLine(" )")
                sb.AppendLine(")")
                sb.AppendLine()
            Next
            Dim wr As New System.IO.StreamWriter(Application.StartupPath & "\" & "redump_wiki.dat",
                False, System.Text.Encoding.GetEncoding(932))
            wr.Write(sb.ToString)
            wr.Close()
            Beep()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub バージョンToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles バージョンToolStripMenuItem.Click
        Dim f As New Form3
        f.ShowDialog()
        f.Close()
    End Sub

    Private Sub CRCマスクToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CRCマスクToolStripMenuItem.Click
        Dim f As New Form2
        f.ShowDialog()
        f.Close()
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        Try
            Dim ofd As New OpenFileDialog
            ofd.InitialDirectory = Application.StartupPath
            ofd.Title = "XMLファイルを選んで下さい"
            ofd.Filter = "XMLファイル(*.xml)|*.xml"
            Dim xml_parse As Boolean = False
            If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then

                Dim reader As System.Xml.XmlReader = System.Xml.XmlReader.Create(ofd.FileName)
                Dim sb As New StringBuilder
                sb.Append("画像")
                sb.Append(vbTab)
                sb.Append("UID")
                sb.Append(vbTab)
                sb.Append("ゲーム名")
                sb.Append(vbTab)
                sb.Append("セーブタイプ")
                sb.Append(vbTab)
                sb.Append("サイズ")
                sb.Append(vbTab)
                sb.Append("メーカー")
                sb.Append(vbTab)
                sb.Append("国")
                sb.Append(vbTab)
                sb.Append("ソース")
                sb.Append(vbTab)
                sb.Append("言語")
                sb.Append(vbTab)
                sb.Append("CRC32")
                sb.Append(vbTab)
                sb.Append("画像1CRC")
                sb.Append(vbTab)
                sb.Append("画像2CRC")
                sb.Append(vbTab)
                sb.Append("コメント")
                sb.Append(vbTab)
                sb.AppendLine("重複ID")

                While reader.Read
                    If reader.NodeType = Xml.XmlNodeType.EndElement AndAlso reader.LocalName = "configuration" Then
                        xml_parse = True
                    End If
                    If reader.NodeType = Xml.XmlNodeType.Element AndAlso xml_parse = True Then
                        If reader.LocalName = "imageNumber" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "releaseNumber" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "title" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "saveType" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "romSize" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "publisher" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "location" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "sourceRom" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "language" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "romCRC" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "im1CRC" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "im2CRC" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "comment" Then
                            sb.Append(reader.ReadString())
                            sb.Append(vbTab)
                        End If
                        If reader.LocalName = "duplicateID" Then
                            sb.AppendLine(reader.ReadString())
                        End If
                    End If
                End While
                Dim sw As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "_cvt.tsv", False, Encoding.GetEncoding(932))
                sw.Write(sb.ToString)
                Beep()
                sw.Close()

                reader.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        Try
            Dim ofd As New OpenFileDialog
            ofd.InitialDirectory = Application.StartupPath
            ofd.Title = "XMLファイルを選んで下さい"
            ofd.Filter = "XMLファイル(*.xml)|*.xml"
            Dim xml_parse As Boolean = False
            If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim s As String = ""
                Dim reader As System.Xml.XmlReader = System.Xml.XmlReader.Create(ofd.FileName)
                Dim sb As New StringBuilder
                sb.Append("""画像""")
                sb.Append(",")
                sb.Append("""UID""")
                sb.Append(",")
                sb.Append("""ゲーム名""")
                sb.Append(",")
                sb.Append("""セーブタイプ""")
                sb.Append(",")
                sb.Append("""サイズ""")
                sb.Append(",")
                sb.Append("""メーカー""")
                sb.Append(",")
                sb.Append("""国""")
                sb.Append(",")
                sb.Append("""ソース""")
                sb.Append(",")
                sb.Append("""言語""")
                sb.Append(",")
                sb.Append("""CRC32""")
                sb.Append(",")
                sb.Append("""画像1CRC""")
                sb.Append(",")
                sb.Append("""画像2CRC""")
                sb.Append(",")
                sb.Append("""コメント""")
                sb.Append(",")
                sb.AppendLine("""重複ID""")

                While reader.Read
                    If reader.NodeType = Xml.XmlNodeType.EndElement AndAlso reader.LocalName = "configuration" Then
                        xml_parse = True
                    End If
                    If reader.NodeType = Xml.XmlNodeType.Element AndAlso xml_parse = True Then
                        If reader.LocalName = "imageNumber" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "releaseNumber" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "title" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(doskiller(s))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "saveType" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "romSize" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "publisher" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s)
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "location" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "sourceRom" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "language" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "romCRC" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "im1CRC" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "im2CRC" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "comment" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.Append("""")
                            sb.Append(",")
                        End If
                        If reader.LocalName = "duplicateID" Then
                            s = reader.ReadString()
                            sb.Append("""")
                            sb.Append(s.Replace("""", """"""))
                            sb.AppendLine("""")
                        End If
                    End If
                End While
                Dim sw As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "_cvt.csv", False, Encoding.GetEncoding(932))
                sw.Write(sb.ToString)
                sw.Close()
                Beep()
                reader.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

End Class
