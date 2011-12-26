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
            Dim index(450) As Integer
            Dim goodcrc(450) As String
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
                Dim q As New Regex("<imageNumber>\d+</imageNumber>", RegexOptions.ECMAScript)
                Dim n As Match
                Dim p As New Regex("<romCRC extension=""\.iso"">[0-9A-F]{8}</romCRC>", RegexOptions.ECMAScript)
                Dim l As Match
                Dim str As New StringBuilder
                Dim rplace As Boolean = False
                Dim filer As Boolean = False
                i = 0
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
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
                        ss = n.Value.Remove(0, 13)
                        ss = ss.Remove(ss.Length - 14, 14)
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
                    If l.Success AndAlso rplace = True Then
                        s = s.Replace(l.Value, "<romCRC extension="".iso"">" & goodcrc(i) & "</romCRC>")
                        i += 1
                    End If
                    str.AppendLine(s)
                End While
                sr2.Close()
                Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "(nointro_fix).xml",
                    False, System.Text.Encoding.GetEncoding(65001))
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
            Dim index(450) As Integer
            Dim goodcrc(450) As String
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
                Dim q As New Regex("<imageNumber>\d+</imageNumber>", RegexOptions.ECMAScript)
                Dim n As Match
                Dim p As New Regex("<romCRC extension=""\.iso"">[0-9A-F]{8}</romCRC>", RegexOptions.ECMAScript)
                Dim l As Match
                Dim str As New StringBuilder
                Dim rplace As Boolean = False
                Dim filer As Boolean = False
                i = 0
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
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
                        ss = n.Value.Remove(0, 13)
                        ss = ss.Remove(ss.Length - 14, 14)
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
                    If l.Success AndAlso rplace = True Then
                        s = s.Replace(l.Value, "<romCRC extension="".iso"">" & goodcrc(i) & "</romCRC>")
                        i += 1
                    End If
                    str.AppendLine(s)
                End While
                sr2.Close()
                Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "(redump_fix).xml",
                    False, System.Text.Encoding.GetEncoding(65001))
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
            Dim title(4000) As String
            Dim size(4000) As String
            Dim crc(4000) As String
            Dim id(4000) As String
            Dim ppath As String = ""
            Dim i As Integer = 0

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
                Dim p As New Regex("<romCRC extension=""\.iso"">[0-9A-Fa-f]{8}</romCRC>", RegexOptions.ECMAScript)
                Dim l As Match
                Dim str As New StringBuilder
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
                    t = r.Match(s)
                    dd = d.Match(s)
                    If n.Success Then
                        s = n.Value.Remove(0, 7)
                        s = s.Remove(s.Length - 8, 8)
                        title(i) = doskiller(s)
                    ElseIf t.Success Then
                        s = t.Value.Remove(0, 9)
                        s = s.Remove(s.Length - 10, 10)
                        size(i) = s
                    ElseIf l.Success Then
                        s = l.Value.Remove(0, 25)
                        s = s.Remove(s.Length - 9, 9)
                        crc(i) = s
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
                sb.Append(title(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("serial """)
                sb.Append(id(i))
                sb.AppendLine("""")
                sb.Append(vbTab)
                sb.Append("rom ( name """)
                sb.Append(title(i))
                sb.Append(".iso"" size ")
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
            Dim title(4000) As String
            Dim desk(4000) As String
            Dim rom(4000) As String
            Dim id(4000) As String
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
            Dim title(4000) As String
            Dim desk(4000) As String
            Dim rom(4000) As String
            Dim id(4000) As String
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
End Class
