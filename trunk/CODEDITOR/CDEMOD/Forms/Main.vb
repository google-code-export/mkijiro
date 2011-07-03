﻿Imports System.IO       'Stream、StreamWriter、StreamReader、FileStream用
Imports System.Text     'Encoding用

Public Class Main
    Friend database As String = Nothing
    Friend loaded As Boolean = False
    Friend PSX As Boolean = False
    Dim enc1 As Integer = My.Settings.MSCODEPAGE

#Region "Menubar procedures"


#Region "Open Database/Save Database"

    Private Sub new_psp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles new_psp.Click
        If loaded = False Then
            codetree.BeginUpdate()
            reset_PSP()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
            loaded = True
        ElseIf MessageBox.Show("新規データベースを作成すると現在のデータベースが消えてしまいます。このまま新規データベースを作成してもよろしいですか？", "データベース保存の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then
            codetree.BeginUpdate()
            reset_PSP()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
        End If
        PSX = False
        saveas_cwcheat.Enabled = True
        saveas_pspar.Enabled = True
        saveas_psx.Enabled = False
        file_saveas.Enabled = True

    End Sub

    Private Sub new_psx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles new_psx.Click
        If loaded = False Then
            codetree.BeginUpdate()
            reset_PSX()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
            loaded = True
        ElseIf MessageBox.Show("新規データベースを作成すると現在のデータベースが消えてしまいます。このまま新規データベースを作成してもよろしいですか？", "データベース保存の確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then
            codetree.BeginUpdate()
            reset_PSX()
            codetree.Nodes.Clear()
            codetree.Nodes.Add("新規データベース").ImageIndex = 0 ' Add the root node and set its icon
            codetree.EndUpdate()
        End If
        PSX = True
        saveas_cwcheat.Enabled = False
        saveas_pspar.Enabled = False
        saveas_psx.Enabled = True
        file_saveas.Enabled = True
    End Sub

    Private Sub file_open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles file_open.Click

        Dim open As New load_db

        If open_file.ShowDialog = Windows.Forms.DialogResult.OK And open_file.FileName <> Nothing Then
            database = open_file.FileName

            error_window.list_save_error.Items.Clear() 'Clear any save errors from a previous database
            PSX = open.check_db(database, enc1) ' Check the file's format
            codetree.Nodes.Clear()
            codetree.BeginUpdate()
            error_window.list_load_error.BeginUpdate()

            If PSX = False Then
                reset_PSP()
                Application.DoEvents()
                open.read_PSP(database, enc1)
                saveas_cwcheat.Enabled = True
                saveas_pspar.Enabled = True
                saveas_psx.Enabled = False
            Else
                reset_PSX()
                Application.DoEvents()
                open.read_PSX(database, enc1)
                saveas_psx.Enabled = True
                saveas_cwcheat.Enabled = False
                saveas_pspar.Enabled = False
            End If

            codetree.EndUpdate()
            error_window.list_load_error.EndUpdate()
            loaded = True
            file_saveas.Enabled = True

        End If

    End Sub


    Private Sub file_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles file_exit.Click

        Close()

    End Sub

#End Region

#Region "Sort procedures"

    Private Sub sort_GID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sort_GID.Click
        error_window.Visible = False
        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes
        codetree.TreeViewNodeSorter = New GID_sort
        codetree.TreeViewNodeSorter = New GID_sort
        codetree.EndUpdate() ' Update the changes made to the tree view.

        If options_error.Checked = True Then
            error_window.Visible = True
        End If


    End Sub

    Private Sub Sort_GTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sort_GTitle.Click

        error_window.Visible = False
        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes
        codetree.TreeViewNodeSorter = New G_Title_sort
        codetree.TreeViewNodeSorter = New G_Title_sort
        codetree.EndUpdate() ' Update the changes made to the tree view.

        If options_error.Checked = True Then
            error_window.Visible = True
        End If

    End Sub

    Private Sub Sort_CTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sort_CTitle.Click

        error_window.Visible = False
        codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes
        codetree.TreeViewNodeSorter = New C_Title_sort
        codetree.TreeViewNodeSorter = New C_Title_sort
        codetree.EndUpdate() ' Update the changes made to the tree view.

        If options_error.Checked = True Then
            error_window.Visible = True
        End If

    End Sub

#End Region

#Region "Options"

    Private Sub options_error_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles options_error.Click

        If options_error.Checked = False Then
            error_window.Show()
            options_error.Checked = True
            options_error.Text = "エラー画面を隠す"
            Me.Focus()

            If options_ontop.Checked = True Then
                Me.TopMost = True
                error_window.TopMost = True
            End If

        Else
            error_window.Hide()
            options_error.Checked = False
            options_error.Text = "エラー画面を表示"
        End If

    End Sub

    Private Sub options_ontop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles options_ontop.Click

        If options_ontop.Checked = False Then
            Me.TopMost = True
            error_window.TopMost = True
            options_ontop.Checked = True
        Else
            Me.TopMost = False
            error_window.TopMost = False
            options_ontop.Checked = False
        End If

    End Sub

#End Region

#End Region

#Region "Toolbar buttons procedures"
    Private Sub add_game_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles add_game.Click

        Try

            Dim newgame As New TreeNode
            With newgame
                .Name = "新規ゲーム"
                .Text = "新規ゲーム"
                .ImageIndex = 1
                .Tag = "0000-00000"
            End With
            codetree.Nodes(0).Nodes.Insert(0, newgame)
            codetree.SelectedNode = newgame
            GT_tb.Enabled = True
            GT_tb.Text = "新規ゲーム"
        Catch ex As Exception

        End Try


    End Sub

    Private Sub rem_game_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rem_game.Click

        Try
            Select Case codetree.SelectedNode.Level

                Case Is <> 0
                    If MessageBox.Show("選択しているゲームとコードをすべて削除しますか？", "削除の確認", _
                                      MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        Select Case codetree.SelectedNode.Level
                            Case Is = 1
                                codetree.SelectedNode.Remove()
                            Case Is = 2
                                codetree.SelectedNode.Parent.Remove()
                        End Select

                    End If

            End Select

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Add_cd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Add_cd.Click

        Try
            Dim newcode As New TreeNode

            With newcode
                .ImageIndex = 2
                .SelectedImageIndex = 3
                .Name = "新規コード"
                .Text = "新規コード"
                .Tag = "0"
            End With

            Select Case codetree.SelectedNode.Level

                Case Is = 1

                    off_rd.Checked = True
                    CT_tb.Enabled = True
                    CT_tb.Text = "新規コード"
                    cmt_tb.Enabled = True
                    cl_tb.Enabled = True
                    codetree.SelectedNode.Nodes.Insert(0, newcode)
                    codetree.SelectedNode = newcode
                Case Is = 2

                    off_rd.Checked = True
                    CT_tb.Enabled = True
                    CT_tb.Text = "新規コード"
                    cmt_tb.Enabled = True
                    cl_tb.Enabled = True
                    codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index + 1, newcode)
                    codetree.SelectedNode = newcode

            End Select

        Catch ex As Exception

        End Try

    End Sub

    Private Sub rem_cd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rem_cd.Click

        Try
            If codetree.SelectedNode.Level = 2 Then

                If MessageBox.Show("選択されたコードを削除しますか?", "削除の確認", MessageBoxButtons.OKCancel, _
                   MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

                    codetree.SelectedNode.Remove()

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub save_gc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_gc.Click

        Try

            Select Case codetree.SelectedNode.Level

                Case Is = 1
                    With codetree.SelectedNode
                        .Name = GT_tb.Text
                        .Text = GT_tb.Text
                        .Tag = GID_tb.Text
                    End With

                Case Is = 2
                    With codetree.SelectedNode.Parent
                        .Name = GT_tb.Text
                        .Text = GT_tb.Text
                        .Tag = GID_tb.Text
                    End With
                    codetree.EndUpdate()
            End Select

        Catch ex As Exception

        End Try

    End Sub

    Private Sub save_cc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_cc.Click

        Try

            Dim b1 As String = cl_tb.Text
            Dim buffer As String = Nothing
            Dim i As Integer = 0
            Dim b5 As String = cmt_tb.Text
            cl_tb.Text = Nothing
            cmt_tb.Text = Nothing
            If off_rd.Checked = True Then
                buffer = "0" & vbCrLf
            Else
                buffer = "1" & vbCrLf
            End If

            If PSX = True Then
                Dim r As New System.Text.RegularExpressions.Regex( _
        "[0-9a-fA-F]{8} [0-9a-zA-Z]{4}", _
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                Dim m As System.Text.RegularExpressions.Match = r.Match(b1)

                While m.Success
                    buffer &= (m.Value) & vbCrLf
                    cl_tb.Text &= (m.Value) & vbCrLf
                    m = m.NextMatch()
                End While
            Else
                b1 = b1.Replace("_L ", "")
                Dim r As New System.Text.RegularExpressions.Regex( _
        "0x........ 0x........", _
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                Dim m As System.Text.RegularExpressions.Match = r.Match(b1)

                While m.Success
                    buffer &= (m.Value) & vbCrLf
                    cl_tb.Text &= (m.Value) & vbCrLf
                    m = m.NextMatch()
                End While

                '        b1 = cl_tb.Text.Replace("_L ", "")
                '        b1 = System.Text.RegularExpressions.Regex.Replace( _
                '            b1, "_C.+\n", vbCrLf)
                '        b1 = System.Text.RegularExpressions.Regex.Replace( _
                '        b1, "[!-/;-@\u005B-`\u007B-\uFFFF].+\n", vbCrLf)
                buffer = System.Text.RegularExpressions.Regex.Replace( _
        buffer, "[g-zG-Z]", "A")
                buffer = buffer.ToUpper
                buffer = System.Text.RegularExpressions.Regex.Replace( _
        buffer, "^0A", "0x")
                buffer = System.Text.RegularExpressions.Regex.Replace( _
        buffer, "(\r|\n)0A", vbCrLf & "0x")
                buffer = buffer.Replace(" 0A", " 0x")
                '        b1 = System.Text.RegularExpressions.Regex.Replace( _
                'b1, "[!-/;-@\u005B-`\u007B-\uFFFF].+[^0-9A-F]$", "")
                '        Dim b2 As String() = b1.Split(CChar(vbCrLf))
            End If

            If codetree.SelectedNode.Level = 2 Then
                codetree.SelectedNode.Name = CT_tb.Text.Replace("_C0 ", "")
                codetree.SelectedNode.Text = CT_tb.Text.Replace("_C0 ", "")
                codetree.SelectedNode.Name = codetree.SelectedNode.Name.Replace("_C1 ", "")
                codetree.SelectedNode.Text = codetree.SelectedNode.Text.Replace("_C1 ", "")
                CT_tb.Text = codetree.SelectedNode.Name
                'For Each s As String In b2

                '    If s <> vbCrLf Then
                '        If i = 0 Then
                '            If off_rd.Checked = True Then
                '                buffer = "0" & vbCrLf
                '            Else
                '                buffer = "1" & vbCrLf
                '            End If
                '            i += 1
                '        End If

                '        If i > 0 And s.Length > 2 Then
                '            buffer &= s.Trim & vbCrLf
                '        End If
                '    End If

                'Next
                If b5 <> Nothing Then
                    Dim b3 As String() = b5.Split(CChar(vbLf))
                    For Each s As String In b3
                        s = s.Replace("#", "")
                        If i = 0 Then
                            If s.Substring(0, 1) >= "!" Then
                                buffer &= "#" & s.Trim & vbCrLf
                                cmt_tb.Text &= s.Trim & vbCrLf

                            End If
                        End If

                        If i > 0 And s.Length > 1 Then
                            buffer &= "#" & s.Trim & vbCrLf
                            cmt_tb.Text &= s.Trim & vbCrLf
                        End If
                        i += 1
                    Next
                End If


                codetree.SelectedNode.Tag = buffer
                codetree.EndUpdate()
            End If


        Catch ex As Exception

        End Try

    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click

        Try

            Dim newcode As New TreeNode

            If codetree.SelectedNode.Level = 1 Then
                codetree.BeginUpdate()
                Dim cln As TreeNode = CType(codetree.SelectedNode.Clone(), TreeNode)
                codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index - 1, cln)
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = cln
                codetree.EndUpdate()
            End If

            If codetree.SelectedNode.Level = 2 Then

                With newcode
                    .ImageIndex = 2
                    .SelectedImageIndex = 3
                    .Name = codetree.SelectedNode.Name
                    .Text = codetree.SelectedNode.Text
                    .Tag = codetree.SelectedNode.Tag
                End With

                codetree.BeginUpdate()
                codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index - 1, newcode)
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = newcode
                codetree.EndUpdate()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click

        Try
            Dim newcode As New TreeNode


            If codetree.SelectedNode.Level = 1 Then
                codetree.BeginUpdate()
                Dim cln As TreeNode = CType(codetree.SelectedNode.Clone(), TreeNode)
                codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index + 2, cln)
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = cln
                codetree.EndUpdate()
            End If

            If codetree.SelectedNode.Level = 2 Then

                With newcode
                    .ImageIndex = 2
                    .SelectedImageIndex = 3
                    .Name = codetree.SelectedNode.Name
                    .Text = codetree.SelectedNode.Text
                    .Tag = codetree.SelectedNode.Tag
                End With

                codetree.BeginUpdate()
                codetree.SelectedNode.Parent.Nodes.Insert(codetree.SelectedNode.Index + 2, newcode)
                codetree.SelectedNode.Remove()
                codetree.SelectedNode = newcode
                codetree.EndUpdate()
            End If

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Code tree procedures"

    Private Sub codetree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles codetree.AfterSelect
        Dim j As New joker

        Select Case codetree.SelectedNode.Level

            Case Is = 0
                codetree.SelectedNode.SelectedImageIndex = 0
                resets_level1() ' Sets appropriate access to code editing
            Case Is = 1
                codetree.SelectedNode.SelectedImageIndex = 1
                GID_tb.Text = codetree.SelectedNode.Tag.ToString.Trim
                GT_tb.Text = codetree.SelectedNode.Text.Trim
                resets_level2() ' Sets appropriate access to code editing
            Case Is = 2
                Dim b1 As String = codetree.SelectedNode.Tag.ToString
                Dim b2 As String() = b1.Split(CChar(vbCrLf))
                Dim i As Integer = 0
                Dim skip As Boolean = False

                codetree.SelectedNode.SelectedImageIndex = 3
                CT_tb.Text = codetree.SelectedNode.Text.Trim
                GID_tb.Text = codetree.SelectedNode.Parent.Tag.ToString.Trim
                GT_tb.Text = codetree.SelectedNode.Parent.Text.Trim
                resets_level3() ' Sets appropriate access to code editing

                For Each s As String In b2

                    skip = False

                    s = s.Trim ' Remove the new line character so it doesn't interfere with checks

                    If i = 0 Then ' If on the first line, check if the code is enabled by default

                        If s = "1" Then
                            on_rd.Checked = True
                        Else
                            off_rd.Checked = True
                        End If

                        skip = True

                    End If

                    i += 1

                    Try

                        If s <> Nothing And skip = False Then

                            ' Check for a joker
                            If s.Trim.Length = 21 Then

                                If s.Substring(2, 1).ToUpper = "D" And s.Substring(13, 1) = "1" Then
                                    j.button_value(s)
                                ElseIf s.Substring(2, 1).ToUpper = "D" And s.Substring(13, 1) = "3" Then
                                    inverse_chk.Checked = True
                                    j.button_value(s)
                                End If

                            End If

                            If s.Length >= 2 Then

                                If s.Contains("#") Then
                                    cmt_tb.Text &= s.Substring(1, s.Length - 1) & vbCrLf
                                Else
                                    cl_tb.Text &= s & vbCrLf
                                End If

                            End If

                        End If

                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                Next

        End Select

    End Sub

#End Region

#Region "Joker code procedures"

    Private Sub button_list_keypress(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_list.KeyPress

        ' Used if a user checks the options using spacebar since
        ' for some reason, IndexChanged doesn't work when the
        ' spacebar is used

        Dim j As New joker
        Dim x As Integer = 0
        Dim proceed As Boolean = False

        If cl_tb.Text.Trim.Length >= 21 Then ' If the code text box contains at least one code or more

            For x = 0 To 19  ' Check if any joker buttons were selected
                If button_list.GetItemChecked(x) = True Then
                    proceed = True
                    Exit For ' No need to continue since we know something is checked
                End If
            Next

        End If

        If proceed = True Then ' If a joker was selected, calculate the code
            j.add_joker()
        Else ' If not, remove any jokers if they exist
            j.remove_joker()
        End If

    End Sub

    Private Sub button_list_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_list.SelectedIndexChanged

        Dim j As New joker
        Dim x As Integer = 0
        Dim proceed As Boolean = False

        If cl_tb.Text.Trim.Length >= 21 Then ' If the code text box contains at least one code or more

            For x = 0 To 19  ' Check if any joker buttons were selected
                If button_list.GetItemChecked(x) = True Then
                    proceed = True
                    Exit For ' No need to continue since we know something is checked
                End If
            Next

        End If

        If proceed = True Then ' If a joker was selected, calculate the code
            j.add_joker()
        Else ' If not, remove any jokers if they exist
            j.remove_joker()
        End If

    End Sub

    Private Sub button_list_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_list.DoubleClick

        ' For some reason, when clicking quickly on the checkbox list it will not fire 
        ' off the SelectedIndexChanged event  so this is used to capture any changes 
        ' when the user clicks on the control quickly.

        Dim x As Integer = 0
        Dim proceed As Boolean = False
        Dim j As New joker

        If cl_tb.Text.Trim.Length >= 21 Then ' If the code text box contains at least one code or more

            For x = 0 To 19  ' Check if any joker buttons were selected
                If button_list.GetItemChecked(x) = True Then
                    proceed = True
                    Exit For ' No need to continue since we know something is checked
                End If
            Next

        End If

        If proceed = True Then ' If a joker was selected, calculate the code
            j.add_joker()
        Else ' If not, remove any jokers if they exist
            j.remove_joker()
        End If

    End Sub

    Private Sub inverse_chk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles inverse_chk.CheckedChanged

        Dim x As Integer = 0
        Dim proceed As Boolean = False
        Dim j As New joker

        If cl_tb.Text.Trim.Length >= 21 Then ' If the code text box contains at least one code or more

            For x = 0 To 19  ' Check if any joker buttons were selected
                If button_list.GetItemChecked(x) = True Then
                    proceed = True
                End If
            Next

        End If

        If proceed = True Then ' If a joker was selected, calculate the code
            j.add_joker()
        Else ' If not, remove any jokers if they exist
            j.remove_joker()
        End If

    End Sub

    Private Sub button_list_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles button_list.ItemCheck


        ' Restricts the amount of checked items to 3 since CWcheat 
        ' only supports a 3 button press combination for joker codes

        If button_list.CheckedItems.Count >= 3 Then

            e.NewValue = CheckState.Unchecked

        End If

    End Sub

#End Region

#Region "Control resets"

    Private Sub resets_level1()

        ' Disable editing of games and codes if the root node is selected
        GID_tb.Enabled = False
        GID_tb.Text = Nothing
        GT_tb.Enabled = False
        GT_tb.Text = Nothing
        cmt_tb.Enabled = False
        cmt_tb.Text = Nothing
        cl_tb.Enabled = False
        cl_tb.Text = Nothing
        CT_tb.Enabled = False
        CT_tb.Text = Nothing
        off_rd.Enabled = False
        on_rd.Enabled = False

        If PSX = False Then

            button_list.Enabled = False
            inverse_chk.Enabled = False
            inverse_chk.Checked = False

        End If

        For i = 0 To 19 ' Reset the checked list box state
            button_list.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub resets_level2()

        ' Disable editing of a code if one is not selected
        GID_tb.Enabled = True
        GT_tb.Enabled = True
        cmt_tb.Enabled = False
        cmt_tb.Text = Nothing
        cl_tb.Enabled = False
        cl_tb.Text = Nothing
        CT_tb.Enabled = False
        CT_tb.Text = Nothing
        off_rd.Enabled = False
        on_rd.Enabled = False

        If PSX = False Then

            button_list.Enabled = False
            inverse_chk.Enabled = False
            inverse_chk.Checked = False

        End If

        For i = 0 To 19 ' Reset the checked list box state
            button_list.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub resets_level3()

        ' Enable editing of all controls
        cmt_tb.Enabled = True
        cmt_tb.Text = Nothing
        cl_tb.Enabled = True
        cl_tb.Text = Nothing
        CT_tb.Enabled = True
        off_rd.Enabled = True
        on_rd.Enabled = True

        If PSX = False Then

            button_list.Enabled = True
            inverse_chk.Enabled = True

        End If

        For i = 0 To 19 ' Reset the checked list box state
            button_list.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub reset_PSX()

        button_list.Enabled = False
        inverse_chk.Enabled = False
        codetree.ImageList = PSX_iconset
        Sort_GTitle.Image = My.Resources.sony_playstation
        With tool_menu
            add_game.Image = My.Resources.Resources.add_PSX_game
            rem_game.Image = My.Resources.Resources.remove_PSX_game
            save_gc.Image = My.Resources.Resources.save_PSX_game
        End With

    End Sub

    Private Sub reset_PSP()

        button_list.Enabled = True
        inverse_chk.Enabled = True
        codetree.ImageList = iconset
        Sort_GTitle.Image = My.Resources.sony_psp
        With tool_menu
            add_game.Image = My.Resources.Resources.add_game
            rem_game.Image = My.Resources.remove_game
            save_gc.Image = My.Resources.Resources.save_game
        End With

    End Sub

#End Region

#Region "Window control"

    ' This makes sure the error list window always ends up below the main window
    Private Sub Main_locationchanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.LocationChanged

        If error_window.Visible = True Then

            Dim point As New Point
            point.X = Me.Location.X
            point.Y = Me.Location.Y + Me.Height
            error_window.Location = point

        End If

    End Sub

    Private Sub Main_resized(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        If error_window.Visible = True Then

            Dim point As New Point
            error_window.Width = Me.Width
            point.X = Me.Location.X
            point.Y = Me.Location.Y + Me.Height
            error_window.Location = point

        End If

    End Sub

#End Region

#Region "Hotkeys"

    Private Sub main_key_down(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown

        ' CTRL + V
        If e.Control = True AndAlso e.KeyCode = Keys.V Then
            'to do
        End If

        ' CTRL + C
        If e.Control = True AndAlso e.KeyCode = Keys.C Then
            'to do
        End If

    End Sub

#End Region


    Private Sub saveas_cwcheat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_cwcheat.Click
        Dim open As New load_db
        Dim s As New save_db

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then
            database = save_file.FileName


            s.save_cwcheat(database, enc1)

            ' Reload the file
            codetree.Nodes.Clear()
            codetree.BeginUpdate()
            error_window.list_load_error.BeginUpdate()

            reset_PSP()
            Application.DoEvents()
            open.read_PSP(database, enc1)

            codetree.EndUpdate()
            error_window.list_load_error.EndUpdate()
            file_saveas.Enabled = True

        End If
    End Sub

    Private Sub saveas_psx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_psx.Click
        Dim open As New load_db
        Dim s As New save_db

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then
            database = save_file.FileName

            s.save_psx(database, enc1)

            ' Reload the file
            codetree.Nodes.Clear()
            codetree.BeginUpdate()
            error_window.list_load_error.BeginUpdate()

            reset_PSX()
            Application.DoEvents()
            open.read_PSX(database, enc1)

            codetree.EndUpdate()
            error_window.list_load_error.EndUpdate()
            file_saveas.Enabled = True

        End If
    End Sub

    Private Sub saveas_pspar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveas_pspar.Click
        Dim open As New load_db
        Dim s As New save_db

        If save_file.ShowDialog = Windows.Forms.DialogResult.OK And save_file.FileName <> Nothing Then
            database = save_file.FileName


            s.save_pspar(database, enc1)

            ' Reload the file
            codetree.Nodes.Clear()
            codetree.BeginUpdate()
            error_window.list_load_error.BeginUpdate()

            reset_PSP()
            Application.DoEvents()
            open.read_PSP(database, enc1)

            codetree.EndUpdate()
            error_window.list_load_error.EndUpdate()
            file_saveas.Enabled = True

        End If
    End Sub

    Private Sub progbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles progbar.Click

    End Sub

    Private Sub menu_font_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_font.Click
        Dim fd As New FontDialog()

        fd.Font = CT_tb.Font

        ' 初期選択する色を設定する
        fd.Color = CT_tb.ForeColor

        ' 選択可能なフォントサイズの最大値を設定する
        fd.MaxSize = 24

        ' 選択可能なフォントサイズの最小値を設定する
        fd.MinSize = 9

        ' 存在しないフォントやスタイルを選択すると警告を表示する (初期値 False)
        fd.FontMustExist = True

        ' 色を選択できるようにする (初期値 False)
        fd.ShowColor = True

        ' 取り消し線、下線、テキストの色などのオプションを指定可能にする (初期値 True)
        fd.ShowEffects = True

        ' [ヘルプ] ボタンを表示する (初期値 False)
        fd.ShowHelp = True

        ' [適用] ボタンを表示する (初期値 False)
        fd.ShowApply = True

        ' 非 OEM 文字セット、Symbol 文字セット、ANSI 文字セットを表示する (初期値 False)
        fd.ScriptsOnly = True

        ' 固定ピッチフォント (等幅フォント) だけを表示する (初期値 False)
        fd.FixedPitchOnly = True


        'ダイアログを表示する
        If fd.ShowDialog() <> DialogResult.Cancel Then
            'TextBox1のフォントと色を変える
            CT_tb.Font = fd.Font
            GID_tb.Font = fd.Font
            GT_tb.Font = fd.Font
            cmt_tb.Font = fd.Font
            cl_tb.Font = fd.Font
            codetree.Font = fd.Font
            progbar.Font = fd.Font
            My.Settings.codetree = fd.Font
        End If
    End Sub

    Private Sub menu_options_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_options.Click

    End Sub


    Private Sub CP932ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CP932ToolStripMenuItem.Click

        'エンコードを指定する場合
        My.Settings.MSCODEPAGE = 932
        enc1 = 932
        'If CP932ToolStripMenuItem.Checked = False Then
        '    GBKToolStripMenuItem.Checked = False
        '    CP932ToolStripMenuItem.Checked = True
        'End If

    End Sub

    Private Sub GBKToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GBKToolStripMenuItem.Click

        'エンコードを指定する場合
        My.Settings.MSCODEPAGE = 936
        enc1 = 936
        'If GBKToolStripMenuItem.Checked = False Then
        '    GBKToolStripMenuItem.Checked = True
        '    CP932ToolStripMenuItem.Checked = False
        'End If

    End Sub

    Private Sub EncodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EncodeToolStripMenuItem.Click

        If My.Settings.MSCODEPAGE = 932 Then
            GBKToolStripMenuItem.Checked = False
            CP932ToolStripMenuItem.Checked = True
        Else
            GBKToolStripMenuItem.Checked = True
            CP932ToolStripMenuItem.Checked = False
        End If
    End Sub

    Private Sub cl_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cl_tb.TextChanged

    End Sub

    Private Sub GT_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GT_tb.TextChanged

    End Sub

    Private Sub menu_sort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menu_sort.Click

    End Sub

    Private Sub sort_name_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sort_name.Click

    End Sub


    Private Sub file_new_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles file_new.Click

    End Sub

    Private Sub file_saveas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles file_saveas.Click

    End Sub

    Private Sub cmt_tb_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmt_tb.TextChanged

    End Sub

    Private Sub Joker_lbl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Joker_lbl.Click

    End Sub

    Private Sub すべて閉じるToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles すべて閉じるToolStripMenuItem.Click
        codetree.CollapseAll()
    End Sub

    Private Sub 全て展開するToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 全て展開するToolStripMenuItem.Click
        codetree.ExpandAll()
    End Sub

    Private Sub paserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles paserToolStripMenuItem.Click
        Dim backup As String = cmt_tb.Text
        Dim f As New parser
        f.ShowDialog(Me)
        Dim b1 As String = cmt_tb.Text
        Dim b2 As String() = b1.Split(CChar(vbLf))
        Dim cname As String = Nothing
        Dim code As String = Nothing
        Dim cname2 As String = Nothing
        Dim code2 As String = Nothing
        Dim coment As String = Nothing
        Dim add As Integer = 0
        Dim nullcode As Integer = 0
        Dim i As Integer = 0

        For Each s As String In b2

            If s.Length >= 2 Then
                If s.Substring(0, 2) = "_C" Then
                    nullcode = 1
                    If i = 0 Then
                        If s.Substring(2, 1) = "0" Then
                            code = "0" & vbCrLf
                        Else
                            code = "1" & vbCrLf
                        End If
                        cname = s.Substring(3, s.Length - 3).Trim
                    Else
                        add = 1
                        If nullcode = 1 Then
                            code &= "0" & vbCrLf
                        End If
                        code = code & coment
                        If s.Substring(2, 1) = "0" Then
                            code2 = "0" & vbCrLf
                        Else
                            code2 = "1" & vbCrLf
                        End If
                        cname2 = s.Substring(3, s.Length - 3).Trim
                    End If
                    i += 1
                End If

                If s.Substring(0, 2) = "_L" Then
                    nullcode = 0
                    '_L 0x12345678 0x12345678
                    If s.Substring(3, 2) = "0x" And s.Substring(14, 2) = "0x" Then
                        code &= s.Substring(3, 21).Trim & vbCrLf
                    End If
                End If

                If s.Substring(0, 1) = "#" Then
                    s = s.Replace("#", "")
                    coment &= "#" & s.Trim & vbCrLf
                End If
            End If

            If add = 1 Then
                Try
                    Dim newcode As New TreeNode

                    With newcode
                        .ImageIndex = 2
                        .SelectedImageIndex = 3
                        .Name = cname
                        .Text = cname
                        .Tag = code
                    End With

                    Select Case codetree.SelectedNode.Level

                        Case Is = 1
                            off_rd.Checked = True
                            CT_tb.Enabled = True
                            cmt_tb.Enabled = True
                            cl_tb.Enabled = True
                            codetree.SelectedNode.Nodes.Add(newcode)
                        Case Is = 2
                            off_rd.Checked = True
                            CT_tb.Enabled = True
                            cmt_tb.Enabled = True
                            cl_tb.Enabled = True
                            codetree.SelectedNode.Parent.Nodes.Add(newcode)
                    End Select

                Catch ex As Exception

                End Try

                code = code2
                cname = cname2
                coment = Nothing
                add = 0
            End If
        Next
        cmt_tb.Text = backup
    End Sub

    Private Sub CWCWIKIToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CWCWIKIToolStripMenuItem.Click

    End Sub

    Private Sub wikiToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wikiToolStripMenuItem1.Click
        Process.Start("http://www21.atwiki.jp/cwcwiki/")
    End Sub

    Private Sub OHGToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OHGToolStripMenuItem.Click
        Process.Start("http://www.onehitgamer.com/forum/")
    End Sub

    Private Sub HAXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HAXToolStripMenuItem.Click
        Process.Start("http://haxcommunity.org/")
    End Sub

    Private Sub KAKASI変換ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KAKASI変換ToolStripMenuItem.Click
        Process.Start("APP\kanahenkan.bat")
    End Sub

    Private Sub PMETAN変換ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PMETAN変換ToolStripMenuItem.Click
        Process.Start("APP\pme.bat")
    End Sub

    Private Sub TEMPAR鶴ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMPAR鶴ToolStripMenuItem.Click
        Process.Start("APP\temp.bat")
    End Sub

    Private Sub WgetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WgetToolStripMenuItem.Click
        Process.Start("APP\wget.bat")
    End Sub

    Private Sub JaneStyleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JaneStyleToolStripMenuItem.Click
        Process.Start("APP\jane.bat")
    End Sub

    Private Sub CNGBAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CNGBAToolStripMenuItem.Click
        Process.Start("http://www.cngba.com/forum-988-1.html")
    End Sub

    Private Sub GOOGLEToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GOOGLEToolStripMenuItem.Click
        Process.Start("http://www.google.co.jp/")
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Process.Start("APP\kakasi.bat")
    End Sub

    Private Sub PSPへコードコピーToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PSPへコードコピーToolStripMenuItem.Click
        Process.Start("APP\cp.bat")
    End Sub
End Class
