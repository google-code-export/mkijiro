Imports System.IO
Imports System.Text     'Encoding用

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

                                    If counts(1) >= 20 Then
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

                If counts(1) >= 20 Then

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

                If counts(1) >= 20 Then

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
        Dim buffer(4) As String ' 0 = stream buffer, 1 = SLUS address, 2 = Game name, 3 = Codes, 4 = fixed codes
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

        Dim cf_utf16(33) As Byte
        Dim str As String = Nothing
        Dim i As Integer = 0
        Dim n As Integer = -3
        While i < bs.Length - 3

            If bs(i) = &H47 And bs(i + 1) = &H20 Then 'G
                If b6 <> Nothing Then
                    cnode.Tag = b6
                    b6 = Nothing
                End If
                Do Until bs(i) = 10 'linefeed
                    n += 1
                    i += 1
                Loop
                Dim name(n + 2) As Byte
                Array.ConstrainedCopy(bs, i - n - 1, name, 0, n + 1)
                str = System.Text.Encoding.GetEncoding(1201).GetString(name)
                n = -3
                gnode = New TreeNode(str.Trim)
                With gnode
                    .Name = str.Trim
                    .Tag = Nothing
                    .ImageIndex = 1
                End With
                m.codetree.Nodes(0).Nodes.Add(gnode)
                counts(1) += 1

            ElseIf bs(i) = &H4D And bs(i + 1) = &H20 Then 'M
                i += 34
                Array.ConstrainedCopy(bs, i - 32, cf_utf16, 0, 32)
                str = System.Text.Encoding.GetEncoding(1201).GetString(cf_utf16)
                Dim s1 As Integer = Convert.ToInt32(str.Substring(0, 2), 16)
                Dim s2 As Integer = Convert.ToInt32(str.Substring(2, 2), 16)
                Dim s3 As Integer = Convert.ToInt32(str.Substring(4, 2), 16)
                Dim s4 As Integer = Convert.ToInt32(str.Substring(6, 2), 16)
                b3 = Chr(s1) & Chr(s2) & Chr(s3)
                b3 &= Chr(s4) & "-" & str.Substring(8, 5)
                gnode.Tag = b3
                counts(1) += 1

            ElseIf bs(i) = &H44 And bs(i + 1) = &H20 Then 'D

                If b6 <> Nothing Then
                    cnode.Tag = b6
                    b6 = Nothing
                End If
                Do Until bs(i) = 10 'linefeed
                    n += 1
                    i += 1
                Loop
                Dim cname(n + 2) As Byte
                Array.ConstrainedCopy(bs, i - n - 1, cname, 0, n + 1)
                str = System.Text.Encoding.GetEncoding(1201).GetString(cname)
                n = -3
                cnode = New TreeNode(str.Trim)
                cnode.Name = str.Trim
                cnode.ImageIndex = 2
                gnode.Nodes.Add(cnode)
                b6 = "0" & vbCrLf
                counts(1) += 1

            ElseIf bs(i) = &H43 And bs(i + 1) = &H20 Then 'C
                b5 = Nothing
                i += 34
                Array.ConstrainedCopy(bs, i - 32, cf_utf16, 0, 32)
                str = System.Text.Encoding.GetEncoding(1201).GetString(cf_utf16)
                b5 &= "0x" & str.Substring(0, 8) & " "
                b5 &= "0x" & str.Substring(8, 8) & vbCrLf
                b6 &= b5
                counts(1) += 1
            End If

            i += 1

            If counts(1) = 20 Then

                ' Update the progressbar every 20 repetitions otherwise the program 
                ' will slow to a crawl from the constant re-draw of the progress bar
                percent = (i * 100) / bs.Length
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
            If file.ReadByte = &H47 Then ' If we're on a code line

            cf = True
            Else

            cf = False

        End If

        file.Close()
        Return cf
    End Function

End Class