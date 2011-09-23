Imports System.IO
Imports System.Text     'Encoding用
Imports System.Text.RegularExpressions

Public Class load_db

    Public Sub read_PSP(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(file, _
                                   System.Text.Encoding.GetEncoding(enc1))
        Dim buffer(4) As String ' 0 = stream buffer, 1 = Game ID address, 2 = Game name, 3 = Codes 4= comment
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        Dim NULLCODE As Boolean = False
        Dim cwcar As String = "_L"
        Dim b4 As String = Nothing

        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Value = 0 ' Reset the progress bar
        m.progbar.Visible = True ' Show the progress bar 

        reset_errors() ' Clear the error list before loading

        Try

            Do Until sr.EndOfStream = True ' Begin reading the file and stop when we reach the end

                If skip = False Then

                    buffer(0) = sr.ReadLine
                    percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                    counts(0) += 1 ' Keep track of the line #
                    counts(1) += 1

                End If

                If sr.EndOfStream = True Then 'Check if we are at the end of the file
                    If buffer(0).Length >= 4 Then
                        If buffer(0).Substring(0, 2) = "_G" Then
                            buffer(2) = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                            gnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                            With gnode
                                .Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                .Tag = buffer(1)
                                .ImageIndex = 1
                            End With
                            m.codetree.Nodes(0).Nodes.Add(gnode)

                            buffer(3) = Nothing
                            b4 = Nothing
                            Exit Do
                        End If

                        If buffer(0).Substring(0, 3) = "_L " Or buffer(0).Substring(0, 3) = "_M " Or buffer(0).Substring(0, 3) = "_N " Then
                            NULLCODE = False
                            cwcar = buffer(0).Substring(0, 2)
                            '_L 0x12345678 0x12345678 24文字
                            buffer(0) = System.Text.RegularExpressions.Regex.Replace( _
                    buffer(0), "[g-zG-Z]", "A")
                            buffer(0) = buffer(0).ToUpper
                            buffer(0) = buffer(0).Replace(" 0A", " 0x")
                            buffer(0) = buffer(0).PadRight(24, "0"c)
                            If buffer(0).Substring(3, 2) = "0x" And buffer(0).Substring(14, 2) = "0x" Then 'If it is a correctly formed code record it
                                buffer(3) &= buffer(0).Substring(3, 21).Trim & vbCrLf
                            End If
                        End If
                        If buffer(0).Substring(0, 2) = "_C" Then
                            If NULLCODE = True Then
                                buffer(3) &= "" & vbCrLf
                                cnode.Tag = buffer(3) & b4
                            End If
                            buffer(3) = Nothing
                            b4 = Nothing

                            If buffer(0).Substring(2, 1) = "1" Then
                                buffer(3) = "1" & vbCrLf
                            Else
                                buffer(3) = "0" & vbCrLf
                            End If

                            cnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                            cnode.Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                            cnode.ImageIndex = 2
                            gnode.Nodes.Add(cnode)
                            NULLCODE = True
                        End If
                    End If
                    If NULLCODE = True Then
                        buffer(3) &= "" & vbCrLf
                    End If
                    If buffer(0).Length > 1 And buffer(0).Substring(0, 1) = "#" Then
                        b4 &= buffer(0) & vbCrLf
                    End If
                    cnode.Tag = buffer(3) & b4
                    Exit Do
                End If

                If buffer(0).Length >= 4 Then

                    Select Case buffer(0).Substring(0, 3)

                        Case Is = "_S "
                            skip = False

                            If NULLCODE = True Then
                                buffer(3) &= "" & vbCrLf
                                cnode.Tag = buffer(3) & b4
                                NULLCODE = False
                            End If

                            buffer(3) = Nothing
                            b4 = Nothing
                            buffer(1) = buffer(0).Substring(3, buffer(0).Length - 3).Trim

                        Case Is = "_G "
                            skip = False
                            buffer(2) = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                            gnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                            With gnode
                                .Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                .Tag = buffer(1)
                                .ImageIndex = 1
                            End With
                            m.codetree.Nodes(0).Nodes.Add(gnode)

                        Case Is = "_C0", "_C1", "_C2", "_CO"

                            skip = False

                            If NULLCODE = True Then
                                buffer(3) &= "" & vbCrLf
                                cnode.Tag = buffer(3) & b4
                            End If
                            buffer(3) = Nothing
                            b4 = Nothing

                            If buffer(0).Substring(2, 1) = "1" Then
                                buffer(3) = "1" & vbCrLf
                            Else
                                buffer(3) = "0" & vbCrLf
                            End If

                            cnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                            cnode.Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                            cnode.ImageIndex = 2
                            gnode.Nodes.Add(cnode)
                            NULLCODE = True

                        Case Is = "_L ", "_M ", "_N "
                            NULLCODE = False
                            skip = False
                            cwcar = buffer(0).Substring(0, 3)
                            If cwcar = "_M " Then
                                Dim z As Integer = Integer.Parse(buffer(3).Substring(0, 1))
                                buffer(3) = buffer(3).Remove(0, 1)
                                z = z And 1
                                z = 2 Or z
                                buffer(3) = buffer(3).Insert(0, z.ToString())
                            ElseIf cwcar = "_N " Then
                                Dim z As Integer = Integer.Parse(buffer(3).Substring(0, 1))
                                buffer(3) = buffer(3).Remove(0, 1)
                                z = z And 1
                                z = 4 Or z
                                buffer(3) = buffer(3).Insert(0, z.ToString())
                            Else  ' cwc
                            End If

                            '_L 0x12345678 0x12345678 24文字
                            buffer(0) = buffer(0).PadRight(24, "0"c)
                            If buffer(0).Substring(3, 2) = "0x" And buffer(0).Substring(14, 2) = "0x" Then 'If it is a correctly formed code record it
                                buffer(0) = System.Text.RegularExpressions.Regex.Replace( _
                        buffer(0), "[g-zG-Z]", "A")
                                buffer(0) = buffer(0).ToUpper
                                buffer(0) = buffer(0).Replace(" 0A", " 0x")
                                buffer(3) &= buffer(0).Substring(3, 21).Trim & vbCrLf
                            ElseIf buffer(0).Substring(0, 1) = "#" Then
                                b4 &= buffer(0) & vbCrLf
                            Else ' If it is incorrectly formed, ignore it.

                                counts(2) += 1
                                If buffer(0).Trim = Nothing Then 'If the line is blank
                                    write_errors(counts(0), counts(2), "<!!空白しかない行です>", gnode.Text, cnode.Text)
                                Else
                                    write_errors(counts(0), counts(2), buffer(0) & " <!!対応してないコード形式です>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                End If

                                If ew.Visible = False Then

                                    ew.Show()
                                    ew.tab_error.SelectedIndex = 0 ' Give focus to the "Load Error" tab
                                    m.Focus()
                                    reset_toolbar()

                                End If

                            End If

                            Do Until skip = True

                                buffer(0) = sr.ReadLine
                                counts(0) += 1 ' Keep track of the line #
                                percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                                counts(1) += 1

                                If buffer(0) = Nothing Then ' If we've reached the end of the file or a blank line

                                    If sr.EndOfStream = True Then 'Check if we are at the end of the file
                                        cnode.Tag = buffer(3) & b4
                                        buffer(3) = Nothing
                                        b4 = Nothing
                                        Exit Do
                                    End If
                                End If

                                If buffer(0).Length >= 2 Then
                                    buffer(0) = buffer(0).PadRight(24)
                                    If buffer(0).Substring(0, 3) = cwcar Then
                                        If buffer(0).Substring(3, 2) = "0x" And buffer(0).Substring(14, 2) = "0x" Then 'If it is a correctly formed code record it
                                            buffer(0) = System.Text.RegularExpressions.Regex.Replace( _
                                    buffer(0), "[g-zG-Z]", "A")
                                            buffer(0) = buffer(0).ToUpper
                                            buffer(0) = buffer(0).Replace(" 0A", " 0x")
                                            buffer(3) &= buffer(0).Substring(3, 21).Trim & vbCrLf
                                        Else ' If it is incorrectly formed, add it to the error list and ignore it
                                            counts(2) += 1

                                            If buffer(0).Trim = Nothing Then 'If the line is blank
                                                write_errors(counts(0), counts(2), "<!空白しかない行です>", gnode.Text, cnode.Text)
                                            Else
                                                write_errors(counts(0), counts(2), buffer(0) & " <!対応してないコード形式です。>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                            End If

                                            If ew.Visible = False Then

                                                ew.Show()
                                                ew.tab_error.SelectedIndex = 0 ' Give focus to the "Load Error" tab
                                                m.Focus()
                                                reset_toolbar()

                                            End If

                                        End If

                                    ElseIf buffer(0).Substring(0, 1) = "#" Then
                                        b4 &= buffer(0) & vbCrLf

                                    ElseIf buffer(0).Substring(0, 2) = "_C" Or buffer(0).Substring(0, 2) = "_S" Then
                                        cnode.Tag = buffer(3) & b4 ' Store all collected codes in the nodes 'tag'
                                        buffer(3) = Nothing
                                        b4 = Nothing
                                        skip = True ' If a new game or code is found, skip the initial read so it is processed

                                    End If

                                    If counts(1) >= 100 Then
                                        ' Update the progressbar every 20 repetitions otherwise the program 
                                        ' will slow to a crawl from the constant re-draw of the progress bar
                                        m.progbar.Value = Convert.ToInt32(percent)
                                        m.progbar.PerformStep()
                                        Application.DoEvents()
                                        counts(1) = 0
                                    End If
                                End If
                            Loop


                        Case Else ' This will catch anything that is out of place

                            If buffer(0).Substring(0, 1) = "#" Then
                                b4 &= buffer(0) & vbCrLf

                            ElseIf counts(0) = 1 AndAlso buffer(0).Contains("[CP") AndAlso buffer(0).Contains("]") Then

                            Else ' If what we found isn't a comment, ignore it

                                counts(2) += 1

                                If buffer(0).Trim = Nothing Then 'If the line is blank
                                    write_errors(counts(0), counts(2), "<空白しかない行です。>", gnode.Text, cnode.Text)
                                Else
                                    write_errors(counts(0), counts(2), buffer(0) & " <不正なコードなため追加されませんでした。>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                End If

                                If ew.Visible = False Then

                                    ew.Show()
                                    ew.tab_error.SelectedIndex = 0 ' Give focus to the "Load Error" tab
                                    m.Focus()
                                    reset_toolbar()

                                End If

                                buffer(0) = sr.ReadLine ' Read the next line after the error
                                counts(0) += 1
                                counts(1) += 1
                                skip = True ' Skip the intial read
                            End If

                    End Select

                Else
                    buffer(0) = buffer(0).PadRight(2)
                    If buffer(0).Substring(0, 1) = "#" Then
                        b4 &= buffer(0).Trim & vbCrLf
                    Else
                        ' This is set if there is a garbage line in the database and
                        ' will write the line to the error window and try to continue loading
                        counts(2) += 1
                        'Determine if it's a blank line
                        If buffer(0).Trim = Nothing Then
                            write_errors(counts(0), counts(2), "<空白しかない行です>", gnode.Text, cnode.Text)
                        Else
                            write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした>", gnode.Text, cnode.Text)
                        End If
                    End If
                    skip = False
                End If

                If counts(1) >= 100 Then

                    ' Update the progressbar every 20 repetitions otherwise the program 
                    ' will slow to a crawl from the constant re-draw of the progress bar
                    m.progbar.Value = Convert.ToInt32(percent)
                    m.progbar.PerformStep()
                    Application.DoEvents()
                    counts(1) = 0

                End If
            Loop

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()

        End If

        m.progbar.Visible = False
        sr.Close()
        file.Close()
        memory.FlushMemory() ' Force a garbage collection after all the memory processing

    End Sub

    Public Sub read_PSX(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(file, _
                                   System.Text.Encoding.GetEncoding(enc1))
        Dim buffer(4) As String ' 0 = stream buffer, 1 = SLUS address, 2 = Game name, 3 = Codes, 4 = fixed codes
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        Dim b4 As String = Nothing
        Dim nullcode As Boolean = False
        buffer(0) = Nothing
        gnode.Text = Nothing
        cnode.Text = Nothing
        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Visible = True ' Show the progress bar and reset it's value
        m.progbar.Value = 0 ' Reset the progress bar
        reset_errors() ' Clear the error list before loading

        Try

            Do Until sr.EndOfStream = True ' Begin reading the file and stop when we reach the end

                If skip = False Then

                    buffer(0) = sr.ReadLine
                    percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                    counts(0) += 1 ' Keep track of the line #
                    counts(1) += 1

                End If

                If buffer(0).Length >= 4 Then

                    Select Case buffer(0).Substring(0, 2)

                        Case Is = "_S"
                            skip = False

                            If nullcode = True Then
                                buffer(3) &= "" & vbCrLf
                                cnode.Tag = buffer(3) & b4
                            End If
                            buffer(3) = Nothing
                            b4 = Nothing
                            buffer(1) = buffer(0).Substring(3, buffer(0).Length - 3).Trim

                        Case Is = "_G"
                            skip = False
                            buffer(2) = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                            gnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                            With gnode
                                .Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                                .Tag = buffer(1)
                                .ImageIndex = 1
                            End With
                            m.codetree.Nodes(0).Nodes.Add(gnode)

                        Case Is = "_C"
                            skip = False
                            If nullcode = True Then
                                buffer(3) &= "" & vbCrLf
                                cnode.Tag = buffer(3) & b4
                            End If
                            buffer(3) = Nothing
                            b4 = Nothing

                            If buffer(0).Substring(2, 1) = "1" Then
                                buffer(3) = "1" & vbCrLf
                            Else
                                buffer(3) = "0" & vbCrLf
                            End If

                            cnode = New TreeNode(buffer(0).Substring(3, buffer(0).Length - 3).Trim)
                            cnode.Name = buffer(0).Substring(3, buffer(0).Length - 3).Trim
                            cnode.ImageIndex = 2
                            gnode.Nodes.Add(cnode)
                            nullcode = True

                        Case Is = "_L"

                            skip = False
                            nullcode = False
                            buffer(0) = buffer(0).Trim
                            '_L 12345678 1234
                            If buffer(0).Length = 16 And buffer(0).Substring(11, 1) = " " Then 'If it is a correctly formed code record it
                                buffer(0) = buffer(0).Replace("?", "A")
                                buffer(3) &= buffer(0).Substring(3, 13) & vbCrLf
                            Else

                                buffer(4) = clean_PSX(buffer(0))

                                If buffer(4).Length = 16 Then 'Attempt to remove white spaces and re-check
                                    buffer(3) &= buffer(4).Substring(3, 13) & vbCrLf

                                Else ' If it is incorrectly formed, ignore it.

                                    counts(2) += 1
                                    If buffer(0).Trim = Nothing Then 'If the line is blank
                                        write_errors(counts(0), counts(2), "<空白しかない行です>", gnode.Text, cnode.Text)
                                    Else
                                        write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした。>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                    End If

                                    If ew.Visible = False Then
                                        ew.Visible = True
                                        ew.Show()
                                        ew.tab_error.SelectedIndex = 0 ' Give focus to the "Load Error" tab
                                        m.Focus()
                                        reset_toolbar()

                                    End If

                                End If

                            End If

                            Do Until skip = True

                                buffer(0) = sr.ReadLine
                                counts(0) += 1 ' Keep track of the line #
                                percent = (sr.BaseStream.Position * 100) / sr.BaseStream.Length
                                counts(1) += 1

                                If buffer(0) = Nothing Then ' If we've reached the end of the file

                                    If sr.EndOfStream = True Then
                                        cnode.Tag = buffer(3) & b4
                                        buffer(3) = Nothing
                                        b4 = Nothing
                                    End If

                                    Exit Do

                                ElseIf buffer.Length = 1 Then

                                ElseIf buffer(0).Substring(0, 2) = "_L" Then
                                    buffer(0) = buffer(0).Trim

                                    If buffer(0).Length = 16 And buffer(0).Substring(11, 1) = " " Then
                                        buffer(0) = buffer(0).Replace("?", "A")
                                        buffer(3) &= buffer(0).Substring(3, 13) & vbCrLf
                                    Else

                                        buffer(4) = clean_PSX(buffer(0).Trim)

                                        If buffer(4).Length = 16 And buffer(0).Substring(11, 1) = " " Then 'Attempt to remove white spaces and re-check
                                            buffer(3) &= buffer(4).Substring(3, 13) & vbCrLf

                                        Else ' If it is incorrectly formed, ignore it.

                                            counts(2) += 1
                                            If buffer(0).Replace(" ", "") = Nothing Then 'If the line is blank
                                                write_errors(counts(0), counts(2), "<空白しかない行です>", gnode.Text, cnode.Text)
                                            Else
                                                write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                            End If

                                            If ew.Visible = False Then
                                                ew.Visible = True
                                                ew.Show()
                                                ew.tab_error.SelectedIndex = 0 ' Give focus to the "Load Error" tab
                                                m.Focus()
                                                reset_toolbar()

                                            End If

                                        End If

                                    End If
                                ElseIf buffer(0).Substring(0, 1) = "#" Then
                                    b4 &= buffer(0) & vbCrLf

                                ElseIf buffer(0).Substring(0, 2) = "_S" Or buffer(0).Substring(0, 2) = "_C" Then
                                    cnode.Tag = buffer(3) & b4 ' Store all collected codes in the nodes 'tag'
                                    buffer(3) = Nothing
                                    b4 = Nothing
                                    skip = True ' If a new game or code is found, skip the initial read so it is processed

                                End If

                                If counts(1) >= 20 Then
                                    ' Update the progressbar every 20 repetitions otherwise the program 
                                    ' will slow to a crawl from the constant re-draw of the progress bar
                                    m.progbar.Value = Convert.ToInt32(percent)
                                    m.progbar.PerformStep()
                                    Application.DoEvents()
                                    counts(1) = 0
                                End If

                            Loop

                        Case Else ' This will catch anything that is out of place
                            If buffer(0).Substring(0, 1) = "#" Then
                                b4 &= buffer(0).Trim & vbCrLf

                            ElseIf counts(0) = 1 AndAlso buffer(0).Contains("[CP") AndAlso buffer(0).Contains("]") Then
                            Else ' what we found isn't a comment, ignore it

                                counts(2) += 1
                                If buffer(0).Trim = Nothing Then 'If the line is blank
                                    write_errors(counts(0), counts(2), "<空白しかない行です>", gnode.Text, cnode.Text)
                                Else
                                    write_errors(counts(0), counts(2), buffer(0) & " <追加されませんでした>", gnode.Text, cnode.Text) ' Write the ignored line to the error list
                                End If

                                If ew.Visible = False Then

                                    ew.Show()
                                    ew.tab_error.SelectedIndex = 0 ' Give focus to the "Load Error" tab
                                    m.Focus()
                                    reset_toolbar()

                                End If

                                buffer(0) = sr.ReadLine
                                counts(0) += 1
                                counts(1) += 1
                                skip = True

                            End If

                    End Select

                Else
                    buffer(0) = buffer(0).PadRight(2)
                    If buffer(0).Substring(0, 1) = "#" Then
                        b4 &= buffer(0).Trim & vbCrLf
                    Else
                        ' This is set if there is a garbage line or blank line in the database and
                        ' will write the line to the error window and try to continue loading
                        counts(2) += 1
                        'Determine if it's a blank line

                        If buffer(0).Trim = Nothing Then
                            write_errors(counts(0), counts(2), "<!空白しかない行です>", gnode.Text, cnode.Text)
                        Else
                            write_errors(counts(0), counts(2), buffer(0) & " <!追加されませんでした>", gnode.Text, cnode.Text)
                        End If

                        skip = False

                    End If
                End If

                If counts(1) >= 100 Then

                    ' Update the progressbar every 20 repetitions otherwise the program 
                    ' will slow to a crawl from the constant re-draw of the progress bar
                    m.progbar.Value = Convert.ToInt32(percent)
                    m.progbar.PerformStep()
                    Application.DoEvents()
                    counts(1) = 0

                End If
            Loop

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

        m.progbar.Visible = False
        sr.Close()
        file.Close()
        memory.FlushMemory() ' Force a garbage collection after all the memory processing

    End Sub

    Public Sub read_cf(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        Dim b3 As String = Nothing
        Dim b4 As String = Nothing
        Dim b5 As String = Nothing
        Dim b6 As String = Nothing
        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Visible = True ' Show the progress bar and reset it's value
        m.progbar.Value = 0 ' Reset the progress bar


        reset_errors() ' Clear the error list before loading
        'ファイルを読み込むバイト型配列を作成する
        Dim bs(CInt(file.Length)) As Byte
        'ファイルの内容をすべて読み込む
        file.Read(bs, 0, bs.Length)
        Dim cfdatlen As Integer = bs.Length
        Dim cf_utf16(33) As Byte
        Dim str As String = Nothing
        Dim gname() As Byte = Nothing
        Dim cname() As Byte = Nothing
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim s1 As String = Nothing
        Dim s2 As String = Nothing
        Dim s3 As String = Nothing
        Dim s4 As String = Nothing
        Dim s5 As String = Nothing
        Dim sb As New System.Text.StringBuilder()
        counts(0) = cfdatlen \ 36

        While i < cfdatlen - 3

            If (i And 1) = 0 Then
                If bs(i) = &H47 AndAlso bs(i + 1) = &H20 Then 'G ゲーム名
                    If b6 <> Nothing Then
                        cnode.Tag = b6
                        b6 = Nothing
                    End If
                    i += 2
                    'ヽ|・∀・|ノCP1201　上=0x\4E0A
                    '　|＿＿＿|
                    '　　|　|
                    Do Until bs(i) = 10 AndAlso bs(i + 1) = 10 AndAlso (i And 1) = 0 '0A0A
                        n += 1
                        i += 1
                    Loop
                    Array.Resize(gname, n)
                    Array.ConstrainedCopy(bs, i - n, gname, 0, n)
                    str = System.Text.Encoding.GetEncoding(1201).GetString(gname)
                    n = 0
                    gnode = New TreeNode(str.Trim)
                    With gnode
                        .Name = str.Trim
                        .Tag = Nothing
                        .ImageIndex = 1
                    End With
                    m.codetree.Nodes(0).Nodes.Add(gnode)
                    counts(1) += 1

                ElseIf bs(i) = &H4D AndAlso bs(i + 1) = &H20 Then 'M ゲームID
                    i += 34
                    Array.ConstrainedCopy(bs, i - 32, cf_utf16, 0, 32)
                    str = System.Text.Encoding.GetEncoding(1201).GetString(cf_utf16)
                    sb.Clear()
                    s1 = Chr(Convert.ToInt32(str.Substring(0, 2), 16))
                    s2 = Chr(Convert.ToInt32(str.Substring(2, 2), 16))
                    s3 = Chr(Convert.ToInt32(str.Substring(4, 2), 16))
                    s4 = Chr(Convert.ToInt32(str.Substring(6, 2), 16))
                    s5 = str.Substring(8, 5)
                    sb.Append(s1)
                    sb.Append(s2)
                    sb.Append(s3)
                    sb.Append(s4)
                    sb.Append("-")
                    sb.Append(s5)
                    b3 = sb.ToString()
                    b3 = b3.Replace(CChar(Chr(0)), "0")
                    gnode.Tag = b3
                    counts(1) += 1

                ElseIf bs(i) = &H44 AndAlso bs(i + 1) = &H20 Then 'D コード名

                    If b6 <> Nothing Then
                        cnode.Tag = b6
                        b6 = Nothing
                    End If
                    i += 2
                    'ヽ|・∀・|ノCP1201　上=0x\4E0A
                    '　|＿＿＿|
                    '　　|　|
                    Do Until bs(i) = 10 AndAlso bs(i + 1) = 10 AndAlso (i And 1) = 0  '0A0A
                        n += 1
                        i += 1
                    Loop
                    Array.Resize(cname, n)
                    Array.ConstrainedCopy(bs, i - n, cname, 0, n)
                    str = System.Text.Encoding.GetEncoding(1201).GetString(cname)
                    n = 0
                    cnode = New TreeNode(str.Trim)
                    cnode.Name = str.Trim
                    cnode.ImageIndex = 2
                    gnode.Nodes.Add(cnode)
                    b6 = "0" & vbCrLf
                    counts(1) += 1

                ElseIf bs(i) = &H43 AndAlso bs(i + 1) = &H20 Then 'C コード内容
                    b5 = Nothing
                    i += 34
                    Array.ConstrainedCopy(bs, i - 32, cf_utf16, 0, 32)
                    str = System.Text.Encoding.GetEncoding(1201).GetString(cf_utf16)
                    sb.Clear()
                    sb.Append("0x")
                    sb.Append(str.Substring(0, 8))
                    sb.Append(" 0x")
                    sb.Append(str.Substring(8, 8))
                    sb.Append(vbCrLf)
                    b5 = sb.ToString
                    b6 &= b5
                    counts(1) += 1
                End If
            End If
            i += 1

            If counts(1) = counts(0) Then

                ' Update the progressbar every 20 repetitions otherwise the program 
                ' will slow to a crawl from the constant re-draw of the progress bar
                percent = (i * 100) / cfdatlen
                m.progbar.Value = Convert.ToInt32(percent)
                m.progbar.PerformStep()
                Application.DoEvents()
                counts(1) = 0
            End If

        End While

        If b6 <> Nothing Then
            cnode.Tag = b6
        End If

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

        m.progbar.Visible = False
        file.Close()
        memory.FlushMemory() ' Force a garbage collection after all the memory processing

    End Sub

    Public Sub read_ar(ByVal filename As String, ByVal enc1 As Integer)

        Dim m As MERGE = MERGE
        Dim ew As error_window = error_window
        Dim memory As New MemoryManagement
        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim counts(2) As Integer ' 0 = Line #, 1 = Progress bar counter, 2 = Total formatting errors, 3 = Error number
        Dim percent As Double = 0
        Dim gnode As New TreeNode ' Game name node for the TreeView control
        Dim cnode As New TreeNode ' Code name node for the TreeView control
        Dim skip As Boolean = False
        m.codetree.Nodes.Add(Path.GetFileNameWithoutExtension(filename)).ImageIndex = 0 ' Add the root node and set its icon
        m.progbar.Visible = True ' Show the progress bar and reset it's value
        m.progbar.Value = 0 ' Reset the progress bar

        reset_errors() ' Clear the error list before loading
        'ファイルを読み込むバイト型配列を作成する
        Dim bs(CInt(file.Length)) As Byte
        'ファイルの内容をすべて読み込む
        file.Read(bs, 0, bs.Length)
        Dim datellen As Integer = bs.Length
        Dim str As String = Nothing
        Dim i As Integer = 28
        Dim k As Integer = 0
        Dim l As Integer = 0
        Dim blocklen As Integer = 0
        Dim id(9) As Byte
        Dim code(3) As Byte
        Dim gname() As Byte = Nothing
        Dim cname() As Byte = Nothing
        Dim codeline As Integer = 0
        Dim parsemode As Boolean = False
        Dim nextcode As Integer = 0
        Dim sb As New System.Text.StringBuilder()
        counts(0) = datellen \ 32

        While i < datellen
            blocklen = (CInt(bs(i)) + (CInt(bs(i + 1)) << 8)) << 2
            If blocklen = 0 Then
                blocklen = datellen - i
            End If
            While k < blocklen
                If parsemode = False Then
                    Array.ConstrainedCopy(bs, i + 7, id, 0, 10)
                    str = Encoding.GetEncoding(932).GetString(id)
                    str = str.PadRight(10, " "c)
                    gnode = New TreeNode(str)
                    With gnode
                        .Name = Nothing
                        .Tag = str
                        .ImageIndex = 1
                    End With
                    m.codetree.Nodes(0).Nodes.Add(gnode)
                    k = CInt(bs(i + 4)) - 18
                    Array.Resize(gname, k)
                    Array.ConstrainedCopy(bs, i + 18, gname, 0, k)
                    str = Encoding.GetEncoding(932).GetString(gname)
                    str = str.Replace(vbNullChar, "")
                    gnode.Text = str
                    gnode.Name = str
                    k = CInt(bs(i + 4))
                    parsemode = True
                ElseIf parsemode = True Then
                    codeline = CInt(bs(i + k))
                    l = CInt(bs(i + k + 1)) - 1
                    Array.Resize(cname, l)
                    Array.ConstrainedCopy(bs, i + k + 4, cname, 0, l)
                    str = Encoding.GetEncoding(932).GetString(cname)
                    cnode = New TreeNode(str.Trim)
                    cnode.Name = str.Trim
                    cnode.ImageIndex = 2
                    gnode.Nodes.Add(cnode)
                    sb.Clear()
                    sb.Append("2")
                    sb.Append(vbCrLf)
                    l = CInt(bs(i + k + 2)) << 2
                    While codeline > 0
                        Array.ConstrainedCopy(bs, i + k + l, code, 0, 4)
                        str = Convert.ToString(BitConverter.ToInt32(code, 0), 16)
                        str = str.ToUpper.PadLeft(8, "0"c)
                        sb.Append("0x")
                        sb.Append(str)
                        sb.Append(" 0x")
                        Array.ConstrainedCopy(bs, i + k + l + 4, code, 0, 4)
                        str = Convert.ToString(BitConverter.ToInt32(code, 0), 16)
                        str = str.ToUpper.PadLeft(8, "0"c)
                        sb.Append(str)
                        sb.Append(vbCrLf)
                        l += 8
                        codeline -= 1
                    End While
                    cnode.Tag = sb.ToString
                    nextcode = CInt(bs(i + k + 3)) << 2
                    k += nextcode
                    counts(1) += 1
                    If nextcode = 0 Then
                        Exit While
                    End If
                End If
            End While
            i += blocklen
            k = 0
            parsemode = False

            If counts(1) = counts(0) Then

                ' Update the progressbar every 20 repetitions otherwise the program 
                ' will slow to a crawl from the constant re-draw of the progress bar
                percent = (i * 100) / datellen
                m.progbar.Value = Convert.ToInt32(percent)
                m.progbar.PerformStep()
                Application.DoEvents()
                counts(1) = 0
            End If

        End While

        If ew.list_load_error.Items.Count = 0 And ew.list_save_error.Items.Count > 0 Then
            ew.Show()
            ew.tab_error.SelectedIndex = 1
            m.Focus()
            reset_toolbar()
        End If

        m.progbar.Visible = False
        file.Close()
        memory.FlushMemory() ' Force a garbage collection after all the memory processing

    End Sub

    Private Sub write_errors(ByVal line As Integer, ByVal error_n As Integer, ByVal error_t As String, _
                             ByVal game_t As String, ByVal code_t As String)

        Dim ew As error_window = error_window

        With ew.list_load_error
            .Items.Add(error_n.ToString)
            .Items(error_n - 1).SubItems.Add(line.ToString)
            .Items(error_n - 1).SubItems.Add(game_t)
            .Items(error_n - 1).SubItems.Add(code_t)
            .Items(error_n - 1).SubItems.Add(error_t.Trim)
        End With

        Application.DoEvents()

    End Sub

    Private Sub reset_errors()

        Dim ew As error_window = error_window
        Dim m As MERGE = MERGE

        ew.Hide()
        m.options_error.Text = "エラーログを見る"
        m.options_error.Checked = False
        ew.list_load_error.Items.Clear()

    End Sub

    Private Sub reset_toolbar()

        If MERGE.options_error.Checked = False Then
            MERGE.options_error.Checked = True
            MERGE.options_error.Text = "エラーログを隠す"
        End If

    End Sub

    Private Function clean_PSX(ByVal s As String) As String

        ' This will attempt to remove any extra white spaces
        ' if the attempt fails, it will be marked as incorrect
        ' and written into the error list.

        Dim i As Integer = 0
        clean_PSX = Nothing

        For i = 0 To s.Length - 1

            If s.Substring(i, 1) = " " And i <> 10 And i <> 2 Then ' If we're not on the 3rd space or the 11th
            Else
                clean_PSX &= s.Substring(i, 1)
            End If

        Next

        ' This will attempt to fix a broken code missing the code type after the white spaces are removed.
        ' First it will check if the length is incorrect. If so, calculate the value and place the correct code type.
        ' The only problem with this is if the code was a 16-bit 'equal to' type (AKA joker), it will be incorrect
        ' since there is no way to determine if it was.  More than likely it won't be.

        If clean_PSX.Length = 15 Then ' If we are 1 characters short of an actual code

            If clean_PSX.Substring(3, 1) = "0" Then ' We know it's missing its code type

                If clean_PSX.Substring(clean_PSX.Length - 4, 4) = "????" _
                Or clean_PSX.Substring(clean_PSX.Length - 3, 3) = "???" _
                Or clean_PSX.Substring(clean_PSX.Length - 4, 2) = "??" _
                Or clean_PSX.Substring(clean_PSX.Length - 4, 1) = "?" Then

                    clean_PSX = clean_PSX.Substring(0, 3) & "8" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 16-bit write

                ElseIf clean_PSX.Substring(clean_PSX.Length - 2, 2) = "??" Or clean_PSX.Substring(clean_PSX.Length - 1, 1) = "?" Then

                    clean_PSX = clean_PSX.Substring(0, 3) & "3" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 8-bit write

                Else

                    If Convert.ToInt32(clean_PSX.Substring(clean_PSX.Length - 4, 4), 16) < 256 Then

                        clean_PSX = clean_PSX.Substring(0, 3) & "3" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 8-bit write

                    Else

                        clean_PSX = clean_PSX.Substring(0, 3) & "8" & clean_PSX.Substring(3, clean_PSX.Length - 3) ' 16-bit write

                    End If

                End If

            End If

        End If

    End Function

    Public Function check_enc(ByVal filename As String) As Integer

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim codepage As Integer = 932
        Dim cp(7) As Byte
        Dim bs(1) As Byte
        Dim str As String
        '5B 43 50 39 33 36 5D
        If file.ReadByte = &H5B Then
            file.Seek(0, SeekOrigin.Begin)
            file.Read(cp, 0, 8)
            str = Encoding.GetEncoding(0).GetString(cp)
            Dim r As New Regex("\[CP\d\d\d\]", RegexOptions.ECMAScript)
            Dim m As Match = r.Match(str)
            If m.Success Then
                file.Close()
                str = m.Value
                If str = "[CP932]" Then
                    Return 932
                ElseIf str = "[CP936]" Then
                    Return 936
                ElseIf str = "[CP1201]" Then
                    Return 1201
                End If
            End If
        End If

        file.Close()

        Return My.Settings.MSCODEPAGE

    End Function

    Public Function check_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(file, _
                                   System.Text.Encoding.GetEncoding(enc1))
        Dim buffer As String = Nothing
        Dim cwcpop As Boolean = False
        Do Until sr.EndOfStream = True

            buffer = sr.ReadLine
            buffer = buffer.PadRight(2)
            Try
                If buffer.Substring(0, 2) = "_L" Then ' If we're on a code line

                    If buffer.Substring(3, 2) <> "0x" And buffer.Length <> 24 Then ' If the format isn't a PSP format

                        If buffer.Length = 16 And buffer.Substring(11, 1) = " " Then ' Check if the length and space is correct for PSX

                            cwcpop = True ' It's a POPs file
                            Exit Do
                        End If

                    Else

                        cwcpop = False ' Not a POPs file
                        Exit Do

                    End If

                End If

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        Loop
        file.Close()
        sr.Close()
        Return cwcpop
    End Function

    Public Function check2_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim cf As Boolean = False
        Dim Code(1) As Byte
        Dim bs(1) As Byte
        If file.ReadByte = &H47 And file.Length Mod 2 = 0 And file.Length > 54 Then
            file.Seek(file.Length - 36, SeekOrigin.Begin)
            file.Read(Code, 0, 2)
            file.Seek(file.Length - 2, SeekOrigin.Begin)
            file.Read(bs, 0, 2)
            If Code(0) = &H43 And Code(1) = &H20 And bs(0) = 10 And bs(1) = 10 Then
                cf = True
            End If
        End If

        file.Close()

        Return cf
    End Function

    Public Function check3_db(ByVal filename As String, ByVal enc1 As Integer) As Boolean

        Dim file As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim ar As Boolean = False
        Dim Code(20) As Byte
        Dim binsize(3) As Byte
        Dim binstr(3) As Byte
        Dim arheader As String = Nothing
        Dim size As Integer = 0
        Dim hash(1) As String
        Dim digit(1) As String
        Dim z As UInteger = 0
        If file.ReadByte = &H50 Then
            file.Seek(0, SeekOrigin.Begin)
            file.Read(Code, 0, 20)
            arheader = Encoding.GetEncoding(0).GetString(Code)
            arheader = arheader.Substring(0, 8)
            Array.ConstrainedCopy(Code, 16, binsize, 0, 4)
            size = BitConverter.ToInt32(binsize, 0) + 28
            If arheader = "PSPARC01" AndAlso size = file.Length Then
                'ARCを抜いたへっだのはっしゅ
                Array.ConstrainedCopy(Code, 8, binstr, 0, 4)
                size = BitConverter.ToInt32(binstr, 0)
                digit(1) = size.ToString("X")

                'コード部ばいなりのはっしゅ
                Array.ConstrainedCopy(Code, 12, binstr, 0, 4)
                size = BitConverter.ToInt32(binstr, 0)
                digit(0) = size.ToString("X")

                '偽CRCっぽいあれ、JADでおｋ
                z = datel_hash(Code, 15, 12, 8)
                hash(1) = Convert.ToString(z, 16).ToUpper

                Array.Resize(Code, CInt(file.Length))
                file.Seek(0, SeekOrigin.Begin)
                file.Read(Code, 0, CInt(file.Length))
                z = datel_hash(Code, CInt(file.Length) - 29, 28, CInt(file.Length) - 28)
                hash(0) = Convert.ToString(z, 16).ToUpper
                If hash(0) = digit(0) AndAlso hash(1) = digit(1) Then
                    ar = True
                End If
            End If
        End If

        file.Close()

        Return ar
    End Function

    Public Function datel_hash(ByVal bin() As Byte, ByVal t As Integer, ByVal v As Integer, ByVal w As Integer) As UInteger

        'http://www.varaneckas.com/jad by JADED  playarts
        Dim tmp(t) As Byte
        Array.ConstrainedCopy(bin, v, tmp, 0, w)
        Dim z As UInteger = 0
        Dim y As UInteger = &H20000000
        Dim x As UInteger = &H17072008
        Dim i As Integer = 0
        For i = 0 To t
            z += Convert.ToUInt32(tmp(i))
            If ((z And 1) = 1) Then
                z += y
            End If
            z >>= 1
        Next
        z = z Xor x
        Return z
    End Function

End Class