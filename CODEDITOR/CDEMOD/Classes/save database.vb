Imports System
Imports System.IO

Public Class save_db

    Public Sub save_cwcheat(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim i As Integer = 0 ' Error count
        Dim buffer As String()
        Dim tw As New StreamWriter(filename, False, _
                                   System.Text.Encoding.GetEncoding(enc1))
        Dim ew As error_window = error_window
        Dim errors As Boolean = False
        Dim cwcar As String = "_L "
        Dim b1 As String = Nothing

        reset_errors() ' Clear prior save errors if any

        tw.Write("[CP" & enc1.ToString & "]" & vbCrLf)

        Try

            For Each n As TreeNode In m.codetree.Nodes(0).Nodes

                tw.Write("_S " & n.Tag.ToString.Trim & vbCrLf)
                tw.Write("_G " & n.Text.Trim & vbCrLf)

                For Each n1 As TreeNode In n.Nodes

                    If n1.Tag Is Nothing Then

                        If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                        Else
                            tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                        End If
                        ' If the code title had no actual codes, don't save it
                        'i += 1
                        'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                        'errors = True

                        'ElseIf n1.Tag.ToString.Trim >= "0" Or n1.Tag.ToString.Trim <= "5" Then

                        '    If n1.Tag.ToString.Substring(0, 1) = "0" Then
                        '        tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                        '    Else
                        '        tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                        '    End If
                        '    ' If the code title had no actual codes, don't save it
                        '    'i += 1
                        '    'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                        '    'errors = True

                    Else

                        buffer = n1.Tag.ToString.Split(CChar(vbCrLf))

                        For Each s As String In buffer
                            If s.Length = 1 Then
                                If s = "0" Or s = "2" Or s = "4" Then
                                    If s = "0" Then
                                        cwcar = "_L "
                                    ElseIf s = "2" Then
                                        cwcar = "_M "
                                    ElseIf s = "4" Then
                                        cwcar = "_N "
                                    End If
                                    tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                                ElseIf s = "1" Or s = "3" Or s = "5" Then
                                    If s = "1" Then
                                        cwcar = "_L "
                                    ElseIf s = "3" Then
                                        cwcar = "_M "
                                    ElseIf s = "5" Then
                                        cwcar = "_N "
                                    End If
                                    tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                                End If
                            ElseIf s.Length > 1 Then

                                If s.Contains("#") Then

                                    tw.Write(s.Trim & vbCrLf)

                                Else
                                    '0x00000000 0x00000000
                                    If s.Contains("0x") Then

                                        tw.Write(cwcar & s.Trim & vbCrLf)

                                    Else
                                        ' Error, code length was incorrect
                                        i += 1
                                        write_errors(i, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s.Trim)
                                        errors = True
                                    End If

                                End If

                            End If

                        Next

                    End If

                Next

            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        tw.Close()

    End Sub

    Public Sub save_psx(ByVal filename As String, ByVal enc1 As Integer
                        )

        Dim m As MERGE = MERGE
        Dim i As Integer = 0 ' Error count
        Dim buffer As String()
        Dim tw As New StreamWriter(filename, False, _
                                   System.Text.Encoding.GetEncoding(enc1))
        Dim ew As error_window = error_window
        Dim errors As Boolean = False
        Dim code As String = Nothing

        tw.Write("[CP" & enc1.ToString & "]" & vbCrLf)

        reset_errors() ' Clear prior save errors if any

        For Each n As TreeNode In m.codetree.Nodes(0).Nodes

            tw.Write("_S " & n.Tag.ToString.Trim & vbCrLf)
            tw.Write("_G " & n.Text.Trim & vbCrLf)

            For Each n1 As TreeNode In n.Nodes

                If n1.Tag Is Nothing Then

                    If n1.Tag.ToString.Substring(0, 1) = "0" Then
                        tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                    Else

                        tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                    End If
                    ' If the code title had no actual codes, don't save it
                    'i += 1
                    'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                    'errors = True

                ElseIf n1.Tag.ToString.Trim = "0" Or n1.Tag.ToString.Trim = "1" Then

                    If n1.Tag.ToString.Substring(0, 1) = "0" Then
                        tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                    Else
                        tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                    End If
                    ' If the code title had no actual codes, don't save it
                    'i += 1
                    'write_errors(i, n.Text.Trim, n1.Text.Trim, "Error:  Code title contained no codes, not saved.")
                    'errors = True

                Else

                    If n1.Tag.ToString.Substring(0, 1) = "0" Then
                        tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                    Else
                        tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                    End If


                    buffer = n1.Tag.ToString.Split(CChar(vbCrLf))

                    For Each s As String In buffer

                        If s.Length > 1 Then

                            If s.Contains("#") Then

                                tw.Write(s.Trim & vbCrLf)

                            Else
                                If System.Text.RegularExpressions.Regex.IsMatch( _
    s, _
    "[0-9A-Fa-f]{8} [0-9A-Fa-f?]{4}", _
    System.Text.RegularExpressions.RegexOptions.ECMAScript) Then

                                    tw.Write("_L " & s.Trim & vbCrLf)

                                Else
                                    ' Error, code length was incorrect
                                    i += 1
                                    write_errors(i, n.Text.Trim, n1.Text.Trim, "Incorrectly formatted code: " & s.Trim)
                                    errors = True
                                End If

                            End If

                        End If

                    Next

                End If

            Next

        Next

        tw.Close()

    End Sub

    Public Sub save_cf(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim i As Integer = 0
        Dim buffer As String()
        Dim fs As New System.IO.FileStream(filename, _
                                           System.IO.FileMode.Create, _
                                            System.IO.FileAccess.Write)
        Dim cp1201len As Integer = 0
        Dim bs(3 * 1024 * 1024) As Byte '３Mばいとぐらい
        Dim cfutf16be(34) As Byte

        Try

            For Each n As TreeNode In m.codetree.Nodes(0).Nodes
                Dim gname As String = n.Name.ToString
                Dim gid As String = n.Tag.ToString.Remove(4, 1)
                Dim bytesData As Byte()
                'Shift JISとして文字列に変換
                bytesData = System.Text.Encoding.GetEncoding(932).GetBytes(gid)
                Dim s1 As Integer = CType(bytesData(0), Integer)
                Dim s2 As Integer = CType(bytesData(1), Integer)
                Dim s3 As Integer = CType(bytesData(2), Integer)
                Dim s4 As Integer = CType(bytesData(3), Integer)
                gid = (s1 \ &H10).ToString("X") & (s1 And &HF).ToString("X")
                gid &= (s2 \ &H10).ToString("X") & (s2 And &HF).ToString("X")
                gid &= (s3 \ &H10).ToString("X") & (s3 And &HF).ToString("X")
                gid &= (s4 \ &H10).ToString("X") & (s4 And &HF).ToString("X")
                gid &= n.Tag.ToString.Remove(0, 5) & "820" 'CWC生コードモード
                bs(i) = &H47 'G ゲームタイトル
                bs(i + 1) = &H20
                i += 2
                cp1201len = gname.Length * 2
                Dim name(cp1201len + 1) As Byte
                name = System.Text.Encoding.GetEncoding(1201).GetBytes(gname)
                Array.ConstrainedCopy(name, 0, bs, i, cp1201len)
                i += cp1201len
                bs(i) = 10
                bs(i + 1) = 10
                i += 2
                bs(i) = &H4D    'M ゲームID
                bs(i + 1) = &H20
                i += 2
                cp1201len = gid.Length * 2
                cfutf16be = System.Text.Encoding.GetEncoding(1201).GetBytes(gid)
                Array.ConstrainedCopy(cfutf16be, 0, bs, i, cp1201len)
                i += cp1201len
                bs(i) = 10
                bs(i + 1) = 10
                i += 2

                For Each n1 As TreeNode In n.Nodes

                    bs(i) = &H44 'D コード名
                    bs(i + 1) = &H20
                    i += 2
                    Dim ccname As String = n1.Text.Trim
                    cp1201len = ccname.Length * 2
                    Dim cname(cp1201len + 1) As Byte
                    cname = System.Text.Encoding.GetEncoding(1201).GetBytes(ccname)
                    Array.ConstrainedCopy(cname, 0, bs, i, cp1201len)
                    i += cp1201len
                    bs(i) = 10
                    bs(i + 1) = 10
                    i += 2

                    buffer = n1.Tag.ToString.Split(CChar(vbCrLf))

                    For Each s As String In buffer
                        If s.Contains("0x") Then
                            bs(i) = &H43 'C コード内容
                            bs(i + 1) = &H20
                            i += 2
                            s = s.Replace("0x", "")
                            s = s.Replace(" ", "")
                            s = s.Remove(0, 1)
                            cp1201len = s.Length * 2
                            cfutf16be = System.Text.Encoding.GetEncoding(1201).GetBytes(s)
                            Array.ConstrainedCopy(cfutf16be, 0, bs, i, cp1201len)
                            i += cp1201len
                            bs(i) = 10
                            bs(i + 1) = 10
                            i += 2
                        End If
                    Next

                Next


            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        fs.Write(bs, 0, i)
        fs.Close()

    End Sub

    Private Sub reset_errors()

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE

        ew.Hide()
        m.options_error.Text = "エラー画面を隠す"
        m.options_error.Checked = False
        ew.list_save_error.Items.Clear()

    End Sub

    Private Sub write_errors(ByVal error_n As Integer, ByVal game_t As String, ByVal code_t As String, _
                             ByVal error_r As String)

        Dim ew As error_window = error_window

        With ew.list_save_error
            .Items.Add(error_n.ToString)
            .Items(error_n - 1).SubItems.Add(game_t)
            .Items(error_n - 1).SubItems.Add(code_t)
            .Items(error_n - 1).SubItems.Add(error_r)
        End With

        Application.DoEvents()

    End Sub

    Public Sub clipboad(ByVal MODE As String)

        Dim m As MERGE = MERGE
        Dim i As Integer = 0 ' Error count
        Dim buffer As String()
        Dim filename = "TMP"
        Dim ew As error_window = error_window
        Dim errors As Boolean = False
        Dim cwcar As String = "_L "
        Dim b1 As String = Nothing
        Dim gid As String = ""
        Dim scm As String = ""
        Dim cmf As String = ""
        Dim scmclose As Boolean = False
        Dim out As Boolean = False
        Dim nullcode As Boolean = False
        Dim line As Integer = 0
        Dim nnnn As Integer = 0
        reset_errors() ' Clear prior save errors if any

        Try

            Dim n As TreeNode = m.codetree.SelectedNode

            If n.Level = 0 Then

            ElseIf n.Level > 0 Then
                If n.Level = 2 Then
                    n = n.Parent
                End If
                gid = n.Tag.ToString.Trim
                b1 = "_S " & gid & vbCrLf
                scm = "ID:" & gid & vbCrLf
                b1 &= "_G " & n.Text.Trim & vbCrLf
                scm &= "NAME:" & n.Text.Trim & vbCrLf
                cmf = b1

                If m.codetree.SelectedNode.Level = 2 Then
                        b1 = ""
                End If

                For Each n1 As TreeNode In n.Nodes

                    If n1.Tag Is Nothing Then
                        If n1.Tag.ToString.Substring(0, 1) = "0" Then
                            b1 &= "_C0 " & n1.Text.Trim & vbCrLf
                        Else
                            b1 &= "_C1 " & n1.Text.Trim & vbCrLf
                        End If
                        If scmclose = True Then
                            scm = scm.Insert(scm.Length - 1, "}")
                        End If
                        scm &= "$" & n1.Text.Trim & "{" & vbCrLf
                        scm &= "$ $2 $(FFFFFFFF FFFFFFFF)}" & vbCrLf
                        cmf &= "_C0 " & n1.Text.Trim & vbCrLf
                        line = 0
                    Else

                        buffer = n1.Tag.ToString.Split(CChar(vbCrLf))

                        For Each s As String In buffer
                            If s.Length = 1 Then
                                If s = "0" Or s = "2" Or s = "4" Then
                                    If s = "0" Then
                                        cwcar = "_L "
                                    ElseIf s = "2" Then
                                        cwcar = "_M "
                                    ElseIf s = "4" Then
                                        cwcar = "_N "
                                    End If
                                    b1 &= "_C0 " & n1.Text.Trim & vbCrLf
                                    If MODE <> "CLIP" AndAlso s = "0" Then
                                        If nullcode = True Then
                                            scm &= "$ $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
                                        End If
                                        If scmclose = True Then
                                            scm = scm.Insert(scm.Length - 2, "}")
                                        End If
                                        scm &= "$" & n1.Text.Trim & "{" & vbCrLf
                                        cmf &= "_C0 " & n1.Text.Trim & vbCrLf
                                        line = 0
                                        out = True
                                        nullcode = True
                                    Else
                                        out = False
                                    End If
                                ElseIf s = "1" Or s = "3" Or s = "5" Then
                                    If s = "1" Then
                                        cwcar = "_L "
                                    ElseIf s = "3" Then
                                        cwcar = "_M "
                                    ElseIf s = "5" Then
                                        cwcar = "_N "
                                    End If
                                    b1 &= "_C1 " & n1.Text.Trim & vbCrLf
                                    If MODE <> "CLIP" AndAlso s = "1" Then
                                        If nullcode = True Then
                                            scm &= "$ $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
                                        End If
                                        If scmclose = True Then
                                            scm = scm.Insert(scm.Length - 2, "}")
                                        End If
                                        scm &= "$" & n1.Text.Trim & "{" & vbCrLf
                                        cmf &= "_C1 " & n1.Text.Trim & vbCrLf
                                        line = 0
                                        nullcode = True
                                        out = True
                                    Else
                                        out = False
                                    End If
                                End If

                            ElseIf s.Length > 1 Then

                                If s.Contains("#") AndAlso MODE <> "SCM" Then
                                    b1 &= s.Trim & vbCrLf
                                Else
                                    '0x00000000 0x00000000
                                    If s.Contains("0x") Then
                                        b1 &= cwcar & s.Trim & vbCrLf
                                        scmclose = True
                                        nullcode = False
                                        If out = True Then
                                            If (s.Substring(3, 1) = "4" Or s.Substring(3, 1) = "8" Or s.Substring(3, 1) = "5" Or s.Substring(3, 3) = "305" Or s.Substring(3, 3) = "306") _
                                                AndAlso line = 0 Then
                                                scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & " "
                                                line = 4
                                            ElseIf s.Substring(3, 1) = "6" AndAlso line = 0 Then
                                                scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & " "
                                                line = 6
                                            ElseIf line = 6 Then
                                                If CInt(s.Substring(10, 1)) > 1 Then
                                                    scm &= s.Trim.Replace("0x", "") & " "
                                                    nnnn = CInt(s.Substring(10, 1))
                                                    line = 9
                                                Else
                                                    scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    line = 0
                                                End If
                                            ElseIf line = 9 Then
                                                If s.Substring(3, 1) = "2" Or s.Substring(3, 1) = "3" Then
                                                    scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    nnnn = nnnn \ 2 - 1
                                                    If nnnn > 0 Then
                                                        line = 2
                                                    Else
                                                        line = 0
                                                    End If
                                                Else
                                                    scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    line = 0
                                                End If
                                            ElseIf line = 2 Then
                                                If nnnn > 0 Then
                                                    scm &= "$└ $2 $(" & s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                Else
                                                    scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                    line = 0
                                                End If
                                            ElseIf line = 4 Then
                                                scm &= s.Trim.Replace("0x", "") & ")" & vbCrLf
                                                line = 0
                                            Else
                                                scm &= "$ $2 $(" & s.Trim.Replace("0x", "") & ")" & vbCrLf
                                            End If
                                            cmf &= cwcar & s.Trim & vbCrLf
                                        End If
                                    Else
                                        ' Error, code length was incorrect
                                        i += 1
                                        write_errors(i, n.Text.Trim, n1.Text.Trim, "不正なコード形式です: " & s.Trim)
                                        errors = True
                                    End If

                                End If

                            End If

                        Next

                        End If

                    If m.codetree.SelectedNode.Level = 2 Then
                        If n1.Index = m.codetree.SelectedNode.Index Then
                            Exit For
                        Else
                            b1 = ""
                        End If
                    End If

                Next
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        filename = Application.StartupPath & "\" & gid

        If nullcode = True Then
            scm &= "$ $2 $(FFFFFFFF FFFFFFFF)" & vbCrLf
        End If
        scm = scm.Insert(scm.Length - 2, "}")

        If MODE = "CLIP" Then
            Clipboard.SetText(b1)
        ElseIf MODE = "CMF" Then
            filename &= ".cmf"
            Dim tw As New StreamWriter(filename, False, System.Text.Encoding.GetEncoding(936))
            tw.Write(cmf)
            tw.Close()
        ElseIf MODE = "SCM" Then
            filename &= ".scm"
            Dim tw As New StreamWriter(filename, False, System.Text.Encoding.GetEncoding(936))
            tw.Write(scm)
            tw.Close()
        End If

    End Sub
End Class
