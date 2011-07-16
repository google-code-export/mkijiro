Imports System
Imports System.IO

Public Class save_db

    Public Sub save_cwcheat(ByVal filename As String, ByVal enc1 As Integer
                            )

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
                        'Dim w As String = n1.Tag.ToString.Trim
                        'If w = "0" Or w = "2" Or w = "5" Then
                        '    If w = "0" Then
                        '        cwcar = "_L "
                        '    ElseIf w = "2" Then
                        '        cwcar = "_M "
                        '    ElseIf w = "4" Then
                        '        cwcar = "_N "
                        '    End If
                        '    tw.Write("_C0 " & n1.Text.Trim & vbCrLf)
                        'ElseIf w = "1" Or w = "3" Or w = "5" Then
                        '    If w = "1" Then
                        '        cwcar = "_L "
                        '    ElseIf w = "3" Then
                        '        cwcar = "_M "
                        '    ElseIf w = "5" Then
                        '        cwcar = "_N "
                        '    End If
                        '    tw.Write("_C1 " & n1.Text.Trim & vbCrLf)
                        'End If


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

    Private Sub reset_errors()

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE

        ew.Hide()
        m.options_error.Text = "Show Error Log"
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

End Class
