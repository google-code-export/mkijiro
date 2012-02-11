﻿Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.Diagnostics

Public Class Form1
    Dim iso As String = "NULL"

#Region "FORM"
    Private Sub ffload(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim cmds() As String
        cmds = System.Environment.GetCommandLineArgs()
        Dim cmd As String
        Dim i As Integer = 0
        For Each cmd In cmds
            If i = 1 Then
                iso = cmd
            End If
            i += 1
        Next
        If File.Exists(Application.StartupPath & "\conf") Then
            Dim fs As New System.IO.FileStream(Application.StartupPath & "\conf", System.IO.FileMode.Open, System.IO.FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            fs.Read(bs, 0, bs.Length)
            If bs.Length >= 3 Then
                If bs(0) = 1 Then
                    uid_parent.Checked = True
                End If
                If bs(1) = 1 Then
                    gridview.Checked = True
                End If
                If bs(2) = 1 Then
                    localtime.Checked = True
                End If
            End If
            fs.Close()
        End If

        Dim psf As New psf
        If File.Exists(iso) Then
            If psf.video(iso) <> "" Then
                Button1_Click(sender, e)
                title.Text = psf.GETNAME(iso)
                discid.Text = psf.GETID(iso)
            Else
                iso = ""
            End If
            Button1_Click(sender, e)
        End If

    End Sub

    Private Sub ffclose(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Dim fs As New FileStream(Application.StartupPath & "\conf", System.IO.FileMode.Create, FileAccess.Write)
        Dim bs(2) As Byte
        If uid_parent.Checked = True Then
            bs(0) = 1
        End If
        If gridview.Checked = True Then
            bs(1) = 1
        End If
        If localtime.Checked = True Then
            bs(2) = 1
        End If
        fs.Write(bs, 0, 3)
        fs.Close()
    End Sub

    Private Sub ffDragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub ffDragDrop(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.DragEventArgs) _
            Handles Me.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する
        Dim psf As New psf
        Dim fileName As String() = CType( _
            e.Data.GetData(DataFormats.FileDrop, False), _
            String())
        iso = fileName(0)
        If File.Exists(iso) Then
            If psf.video(iso) <> "" Then
                Button1_Click(sender, e)
                title.Text = psf.GETNAME(iso)
                discid.Text = psf.GETID(iso)
            Else
                iso = ""
            End If
        End If
    End Sub

#End Region

#Region "BUTTON"

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TreeView1.Nodes.Clear()
        If File.Exists(iso) Then
            Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
            Dim str_len As Integer
            Dim num(1000) As Integer
            Dim parent(1000) As Integer
            Dim level(1000) As Integer
            Dim lba As Integer
            Dim lba_m As Integer
            Dim table_len As Integer
            Dim parent_node As New TreeNode
            Dim name As String
            Dim i As Integer
            Dim k As Integer = 1
            Dim bb(1) As Byte
            Dim bbbb(3) As Byte
            Dim na As Byte() = Nothing
            Dim sb As New StringBuilder
            Dim bs(2047) As Byte

            If uid_parent.Checked Then
                TreeView1.Nodes.Add("ISO[0,0]")
            Else
                TreeView1.Nodes.Add("ISO")
            End If
            TreeView1.Nodes(0).Name = "0"
            TreeView1.Nodes(0).Tag = "-1"
            fs.Seek(&H8084, SeekOrigin.Begin)
            fs.Read(bbbb, 0, 4)
            'パステーブルサイズ
            table_len = cvt32bit(bbbb) << 11
            fs.Seek(&H808C, SeekOrigin.Begin)
            fs.Read(bbbb, 0, 4)
            'リトルエンディアンパステーブル(L型)
            lba = cvt32bit(bbbb) << 11
            fs.Read(bbbb, 0, 4)
            '任意L形パステーブル
            lba_m = cvt32bit(bbbb) << 11
            fs.Seek(lba, SeekOrigin.Begin)
            'LBA読み込みサイズを拡張
            Array.Resize(bs, lba_m - lba)
            fs.Read(bs, 0, bs.Length)
            parent_node = TreeView1.Nodes(0)
            While i < table_len
                '文字の長さ
                str_len = bs(i)
                Array.Copy(bs, i + 2, bbbb, 0, 4)
                'LBA
                lba = cvt32bit(bbbb)
                Array.Copy(bs, i + 6, bb, 0, 2)
                '親のUID番号
                parent(k) = cvt16bit(bb)
                Array.Resize(na, str_len)
                'デレクトリの名前
                Array.Copy(bs, i + 8, na, 0, str_len)
                name = Encoding.GetEncoding(0).GetString(na)
                '各ノードにユニーク番号を付加
                sb.Append("UID:")
                sb.Append(k.ToString)
                sb.Append(",LBA:")
                sb.Append(lba.ToString)
                '名前が空の場合ルート
                If name = vbNullChar Then
                    name = "ROOT"
                    parent(1) = 0
                End If
                sb.Append(",PARENT:")
                sb.Append(parent(k).ToString)
                sb.Append(",LEVEL:")
                level(k) = level(parent(k)) + 1
                sb.Append(level(k).ToString)
                sb.Append(",NAME:")
                sb.AppendLine(name)
                Dim x As New TreeNode
                x.Text = name
                If uid_parent.Checked = True Then
                    x.Text &= "[" & k & "," & parent(k) & "]"
                End If
                x.Tag = lba 'parent(k)
                x.ImageIndex = 0
                x.Name = k.ToString
                '親ノードを検索し追加する
                Dim seek_parent_node As New TreeNode
                Dim arr As TreeNode() = TreeView1.Nodes.Find(parent(k).ToString, True)
                seek_parent_node = arr(0)
                seek_parent_node.Nodes.Add(x)

                k += 1
                i += 8
                i += str_len
                If (str_len And 1) <> 0 Then
                    i += 1
                End If
                If bs(i) = 0 Then
                    Exit While
                End If
            End While

            TreeView1.ExpandAll()
            TextBox1.Text = sb.ToString
            fs.Close()
        ElseIf iso <> "NULL" Then
            MessageBox.Show("ファイルをドロップして下さい")
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        Dim name As String = String.Empty

        name = uid_seek.Text

        Dim b As Boolean = True

        If name <> String.Empty Then

            Try

                Dim arr As TreeNode() = TreeView1.Nodes.Find(name, b)

                For i = 0 To arr.Length - 1

                    TreeView1.SelectedNode = arr(i)

                    TreeView1.SelectedNode.BackColor = Color.Red

                Next

            Catch



            End Try

        Else

            MessageBox.Show("Enter Name")

        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim url As String = "http://wikitravel.org/en/Time_zones"
        If My.Application.Culture.Name.ToString.Contains("JP") Then
            url = "http://wikitravel.org/ja/%25E6%2599%2582%25E5%25B7%25AE%25E3%2581%2582%25E3%2582%258C%25E3%2581%2593%25E3%2582%258C#.E6.99.82.E5.B7.AE.E6.97.A9.E8.A6.8B.E8.A1.A8"
        End If
        Dim browserPath As String = GetDefaultBrowserExePath()

        Process.Start(browserPath, url)
    End Sub


    Private Function GetDefaultBrowserExePath() As String
        Return _GetDefaultExePath("http\shell\open\command")
    End Function


    Private Function _GetDefaultExePath(ByVal keyPath As String) As String
        Dim path As String = ""

        ' レジストリ・キーを開く
        ' 「HKEY_CLASSES_ROOT\xxxxx\shell\open\command」
        Dim rKey As RegistryKey = _
          Registry.ClassesRoot.OpenSubKey(keyPath)
        If Not rKey Is Nothing Then
            ' レジストリの値を取得する
            Dim command As String = _
              CType(rKey.GetValue(String.Empty), String)
            If command = Nothing Then
                Return path
            End If

            ' 前後の余白を削る
            command = command.Trim()
            If command.Length = 0 Then
                Return path
            End If

            ' 「"」で始まる長いパス形式かどうかで処理を分ける
            If command.Chars(0) = """"c Then
                ' 「"～"」間の文字列を抽出
                Dim endIndex As Integer = command.IndexOf(""""c, 1)
                If endIndex <> -1 Then
                    ' 抽出開始を「1」ずらす分、長さも「1」引く
                    path = command.Substring(1, endIndex - 1)
                End If
            Else
                ' 「（先頭）～（スペース）」間の文字列を抽出
                Dim endIndex As Integer = command.IndexOf(" "c)
                If endIndex <> -1 Then
                    path = command.Substring(0, endIndex)
                Else
                    path = command
                End If
            End If
        End If

        Return path

    End Function

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles gridview.CheckedChanged
        ListView1.GridLines = gridview.Checked
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles localtime.CheckedChanged
        If TreeView1.SelectedNode IsNot Nothing Then
            getlist(CInt(TreeView1.SelectedNode.Tag))
        End If
    End Sub

#End Region

#Region "TREE_LIST"
    Private Sub TreeView1_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        getlist(CInt(TreeView1.SelectedNode.Tag))
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Dim itemx As New ListViewItem
        itemx = ListView1.SelectedItems(0)
        If itemx.Index = 0 AndAlso itemx.Text = ".." Then
            TreeView1.SelectedNode = TreeView1.SelectedNode.Parent
            getlist(CInt(TreeView1.SelectedNode.Tag))
        ElseIf itemx.ImageIndex = 0 Then
            Dim seek_node As New TreeNode
            Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
            For Each ss As TreeNode In TreeView1.SelectedNode.Nodes
                If s = ss.Tag.ToString Then
                    TreeView1.SelectedNode = ss
                    Exit For
                End If
            Next
            getlist(CInt(TreeView1.SelectedNode.Tag))
        End If
    End Sub

    Private Sub ColumnClick(ByVal o As Object, ByVal e As ColumnClickEventArgs) Handles ListView1.ColumnClick
        ' Set the ListViewItemSorter property to a new ListViewItemComparer 
        ' object. Setting this property immediately sorts the 
        ' ListView using the ListViewItemComparer object.
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(e.Column)
    End Sub

    Private Sub ListView1_Selected_all(sender As System.Object, ByVal e As KeyEventArgs) Handles ListView1.KeyDown
        If e.Control = True Then
            If e.KeyValue = Keys.A Then
                For Each itemx As ListViewItem In ListView1.Items
                    itemx.Selected = True
                Next
            End If
        End If
    End Sub

#End Region

#Region "CONTEXT"

    Private Sub TREECOLLASEToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TREECOLLASEToolStripMenuItem.Click
        TreeView1.CollapseAll()
    End Sub

    Private Sub TREEEXPANDToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TREEEXPANDToolStripMenuItem.Click
        TreeView1.ExpandAll()
    End Sub

    Private Sub GETDATAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GETDATAToolStripMenuItem.Click
        If ListView1.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Dim itemx As New ListViewItem
        Dim errorm As New StringBuilder

        For k = 0 To ListView1.SelectedItems.Count - 1
            itemx = ListView1.SelectedItems(k)

            If itemx.Index = 0 AndAlso itemx.Text = ".." Then
            ElseIf itemx.ImageIndex >= 2 Then
                If File.Exists(iso) Then
                    Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
                    Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
                    Dim ss As String = ListView1.Items(itemx.Index).SubItems(2).Text
                    Dim iso_len As Long = fs.Length
                    If ((CLng(s) << 11) + CLng(ss)) > iso_len Then
                        fs.Close()
                        errorm.Append(s)
                        errorm.Append(",")
                        errorm.Append(ListView1.Items(itemx.Index).SubItems(0).Text)
                        errorm.Append(",")
                        errorm.Append(ss)
                        errorm.AppendLine()
                    Else
                        Dim p As String = Application.StartupPath & "\" & TreeView1.SelectedNode.FullPath & "\" & itemx.Text
                        Directory.CreateDirectory(Path.GetDirectoryName(p))
                        If File.Exists(p) Then
                            File.Delete(p)
                        End If
                        Dim save As New FileStream(p, FileMode.CreateNew, FileAccess.Write)
                        Dim bs(CInt(ss) - 1) As Byte
                        fs.Seek(CInt(s) << 11, SeekOrigin.Begin)
                        fs.Read(bs, 0, bs.Length)
                        fs.Close()
                        save.Write(bs, 0, bs.Length)
                        save.Close()
                    End If
                End If
            ElseIf itemx.ImageIndex = 0 Then
                If File.Exists(iso) Then
                    Dim seek_node As New TreeNode
                    Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
                    For Each ss As TreeNode In TreeView1.SelectedNode.Nodes
                        If s = ss.Tag.ToString Then
                            seek_node = ss
                            Exit For
                        End If
                    Next
                    Dim Ar As New ArrayList
                    Ar = GetAllNodes(seek_node.Nodes)

                    getfile(seek_node)
                    For Each tt As TreeNode In Ar
                        errorm.Append(getfile(tt))
                    Next

                End If
            End If
        Next

        If errorm.Length > 0 Then
            errorm.Insert(0, vbCrLf)
            errorm.Insert(0, "!!破損ファイルがありました")
            MessageBox.Show(errorm.ToString)
        End If

    End Sub

    Private Sub EXTRACTDATAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EXTRACTDATAToolStripMenuItem.Click
        Dim Ar As New ArrayList
        Dim errorm As New StringBuilder
        Ar = GetAllNodes(TreeView1.SelectedNode.Nodes)
        If TreeView1.SelectedNode.Level > 0 Then
            errorm.Append(getfile(TreeView1.SelectedNode))
        End If

        For Each tt As TreeNode In Ar
            errorm.Append(getfile(tt))
        Next

        If errorm.Length > 0 Then
            errorm.Insert(0, vbCrLf)
            errorm.Insert(0, "!!破損ファイルがありました")
            MessageBox.Show(errorm.ToString)
        End If

    End Sub

    Private Sub EXTRACTLBAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ABSOLUTPATHToolStripMenuItem.Click, OFFSETPATHToolStripMenuItem.Click
        Dim Ar As New ArrayList
        Ar = GetAllNodes(TreeView1.SelectedNode.Nodes)
        Dim title_st As String = title.Text
        Dim stbyte As Byte() = System.Text.Encoding.GetEncoding(932).GetBytes(title_st)
        title_st = System.Text.Encoding.GetEncoding(932).GetString(stbyte)

        Dim dosmoji As String() = {"\", "/", ":", "*", "?", """", "<", ">", "|", vbCr, vbLf}
        For i = 0 To 10
            title_st = title_st.Replace(dosmoji(i), "")
        Next

        Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\" & title_st & ".txt", False, System.Text.Encoding.GetEncoding(65001))
        Dim sb As New StringBuilder
        If TreeView1.SelectedNode.Level > 0 Then
            sb.Append(getlba(TreeView1.SelectedNode, sender))
        End If

        For Each tt As TreeNode In Ar
            sb.Append(getlba(tt, sender))
        Next

        Dim ss As String() = sb.ToString().Split(CChar(vbLf))
        Array.Sort(ss)
        sb.Clear()
        For j = 0 To ss.Length - 1
            sb.Append(ss(j))
            sb.Append(vbLf)
        Next
        If sb(0) = vbLf Then
            sb.Remove(0, 1)
        End If
        sw.Write(sb.ToString)

        sw.Close()
    End Sub
#End Region

#Region "GETFUNC"

    Function cvt16bit(ByVal b As Byte()) As Integer
        Return BitConverter.ToInt16(b, 0)
    End Function

    Function cvt32bit(ByVal b As Byte()) As Integer
        Return BitConverter.ToInt32(b, 0)
    End Function

    Function getlist(ByVal dst As Integer) As Boolean
        Try
            ListView1.Clear()
            If File.Exists(iso) Then
                If TreeView1.SelectedNode.Level = 0 Then
                    Return True
                End If
                ListView1.View = View.Details
                ListView1.AutoSize = True
                Dim zz As Integer
                Dim finalcol As Integer

                ListView1.Columns.Add("NAME", -1, HorizontalAlignment.Left)
                ListView1.Columns.Add("LBA", -1, HorizontalAlignment.Left)
                ListView1.Columns.Add("SIZE", -1, HorizontalAlignment.Left)
                If localtime.Checked Then
                    ListView1.Columns.Add("DATE(LOCAL)", -1, HorizontalAlignment.Left)
                    zz = 3
                    finalcol = 150
                Else
                    ListView1.Columns.Add("DATE", -1, HorizontalAlignment.Left)
                    ListView1.Columns.Add("UTCDIFF", -1, HorizontalAlignment.Left)
                    zz = 4
                    finalcol = 80
                End If



                Dim lba As Integer = 0
                Dim lba_base As Integer = dst << 11

                Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
                Dim bs(2047) As Byte
                Dim bb(1) As Byte
                Dim bbbb(3) As Byte
                Dim next_len As Integer
                Dim filesize As Integer
                Dim str_len As Integer
                Dim iso_len As Long = fs.Length
                Dim name As String
                Dim i As Integer
                Dim yyyymmdd(6) As Byte
                Dim na As Byte() = Nothing

                Dim seek_parent_node As New TreeNode
                Dim arr As TreeNode() = TreeView1.Nodes.Find((CInt(TreeView1.SelectedNode.Name) + 1).ToString, True)
                Dim nextlba As Integer = -dst
                If arr.Length > 0 Then
                    nextlba += CInt(arr(0).Tag.ToString)
                Else
                    fs.Seek((CInt(TreeView1.SelectedNode.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
                    fs.Read(bs, 0, 2048)
                    While True 'i < 2048
                        next_len = bs(i)
                        If bs(i + 33) >= 32 Then
                            Array.Copy(bs, i + 2, bbbb, 0, 4)
                            lba = cvt32bit(bbbb)
                            Array.Copy(bs, i + 10, bbbb, 0, 4)
                            filesize = cvt32bit(bbbb)
                            str_len = bs(i + 32)
                            Array.Resize(na, str_len)
                            Array.Copy(bs, i + 33, na, 0, str_len)
                            name = Encoding.GetEncoding(0).GetString(na)
                            'フォルダかつLBAがおなじ
                            If ((bs(i + 25) >> 1) And 1) = 1 Then
                                If dst = lba Then
                                    Exit While
                                End If
                            End If
                        End If

                        i += next_len

                        '見つかるまで無限ループ
                        If bs(i) = 0 Then
                            fs.Read(bs, 0, 2048)
                            i = 0
                        End If
                    End While
                    nextlba = (filesize >> 11) - 1
                    i = 0
                End If

                fs.Seek(lba_base, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)

                ListView1.BeginUpdate()
                Dim unix_back As New ListViewItem
                unix_back.Text = ".."
                unix_back.ImageIndex = 0
                unix_back.SubItems.Add("")
                unix_back.SubItems.Add("")
                unix_back.SubItems.Add("")
                If localtime.Checked = False Then
                    unix_back.SubItems.Add("")
                End If
                If TreeView1.SelectedNode.Parent.Level > 1 Then
                    ListView1.Items.Add(unix_back)
                End If

                While i < bs.Length
                    next_len = bs(i)
                    If bs(i + 33) >= 32 Then
                        Array.Copy(bs, i + 2, bbbb, 0, 4)
                        lba = cvt32bit(bbbb)
                        Array.Copy(bs, i + 10, bbbb, 0, 4)
                        filesize = cvt32bit(bbbb)
                        Array.Copy(bs, i + 18, yyyymmdd, 0, 7)
                        str_len = bs(i + 32)
                        Array.Resize(na, str_len)
                        Array.Copy(bs, i + 33, na, 0, str_len)
                        name = Encoding.GetEncoding(0).GetString(na)
                        Dim itemx As New ListViewItem
                        itemx.Text = name
                        If ((bs(i + 25) >> 1) And 1) = 0 Then
                            itemx.ImageIndex = 2
                        Else
                            itemx.ImageIndex = 0
                        End If
                        If ((lba << 11) + filesize) > iso_len Then
                            itemx.ImageIndex = itemx.ImageIndex Or 1
                        End If
                        itemx.SubItems.Add(lba.ToString)
                        itemx.SubItems.Add(filesize.ToString)
                        itemx.SubItems.Add(cvt_date(yyyymmdd))
                        If localtime.Checked = False Then
                            itemx.SubItems.Add(cvt_utc(yyyymmdd(6)))
                        End If

                        ListView1.Items.Add(itemx)
                    End If

                    i += next_len

                    If bs(i) = 0 Then
                        nextlba -= 1
                        If nextlba > 0 Then
                            i = 0
                            fs.Read(bs, 0, 2048)
                        Else
                            Exit While
                        End If
                    End If
                End While

                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                ListView1.Columns(zz).Width = finalcol

                ListView1.EndUpdate()

                ListView2.Items.Clear()
                Dim itemx2 As New ListViewItem
                Dim s As String = TreeView1.SelectedNode.FullPath
                Dim rm As New Regex("\[\d+,\d+\]")
                Dim m As Match = rm.Match(s)
                While m.Success
                    s = s.Replace(m.Value, "")
                    m = m.NextMatch
                End While
                itemx2.Text = s
                ListView2.Items.Add(itemx2)

                fs.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return True
    End Function

    Function cvt_utc(ByVal utc As Byte) As String
        Dim sb As New StringBuilder
        Dim z As Integer = utc
        If z > 127 Then
            sb.Append("-")
            z = 256 - z
        Else
            sb.Append("+")
        End If
        sb.Append(z >> 2)
        sb.Append(":")
        sb.Append(z Mod 4)

        Return sb.ToString
    End Function

    Function cvt_date(ByVal ymd As Byte()) As String
        Dim sb As New StringBuilder
        sb.Append((ymd(0) + 1900))
        sb.Append("/")
        sb.Append((ymd(1).ToString.PadLeft(2, "0"c)))
        sb.Append("/")
        sb.Append((ymd(2).ToString).PadLeft(2, "0"c))
        sb.Append(" ")
        sb.Append((ymd(3).ToString.PadLeft(2, "0"c)))
        sb.Append(":")
        sb.Append((ymd(4).ToString.PadLeft(2, "0"c)))
        sb.Append(":")
        sb.Append((ymd(5).ToString.PadLeft(2, "0"c)))

        If localtime.Checked = True Then
            Dim cFormat As New System.Globalization.CultureInfo("ja-JP", False)
            Dim dtBirth As DateTime = DateTime.Parse(sb.ToString, cFormat)

            Dim z As Integer = ymd(6)
            If (z >> 7) = 1 Then
                z -= 256
            End If
            dtBirth = dtBirth.ToLocalTime()
            dtBirth = dtBirth.AddHours(-(z >> 2))
            dtBirth = dtBirth.AddMinutes(-((z And 3) * 15))
            sb.Clear()

            sb.Append(dtBirth.ToString())
        End If

        Return sb.ToString
    End Function

    Function getfile(ByVal tt As TreeNode) As String
        Dim basepath As String = Application.StartupPath & "\" & tt.FullPath & "\"
        Directory.CreateDirectory(Application.StartupPath & "\" & tt.FullPath)
        Dim dst As Integer = CInt(tt.Tag.ToString)
        Dim lba As Integer = 0
        Dim lba_base As Integer = dst << 11


        Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
        Dim next_len As Integer
        Dim filesize As Integer
        Dim str_len As Integer
        Dim name As String
        Dim iso_len As Long = fs.Length
        Dim i As Integer = 0
        Dim bb(1) As Byte
        Dim bbbb(3) As Byte
        Dim na As Byte() = Nothing
        Dim bs(2047) As Byte
        Dim error_file As New StringBuilder

        Dim seek_parent_node As New TreeNode
        Dim arr As TreeNode() = TreeView1.Nodes.Find((CInt(tt.Name) + 1).ToString, True)
        Dim nextlba As Integer = -dst
        If arr.Length > 0 Then
            nextlba += CInt(arr(0).Tag.ToString)
        Else
            fs.Seek((CInt(tt.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)
            While i < 2048
                next_len = bs(i)
                If bs(i + 33) >= 32 Then
                    Array.Copy(bs, i + 2, bbbb, 0, 4)
                    lba = cvt32bit(bbbb)
                    Array.Copy(bs, i + 10, bbbb, 0, 4)
                    filesize = cvt32bit(bbbb)
                    str_len = bs(i + 32)
                    Array.Resize(na, str_len)
                    Array.Copy(bs, i + 33, na, 0, str_len)
                    name = Encoding.GetEncoding(0).GetString(na)
                    'フォルダかつLBAがおなじ
                    If ((bs(i + 25) >> 1) And 1) = 1 Then
                        If dst = lba Then
                            Exit While
                        End If
                    End If
                End If

                i += next_len

                '見つかるまで無限ループ
                If bs(i) = 0 Then
                    fs.Read(bs, 0, 2048)
                    i = 0
                End If
            End While
            nextlba = (filesize >> 11) - 1
            i = 0
        End If

        fs.Seek(lba_base, SeekOrigin.Begin)
        fs.Read(bs, 0, 2048)

        While i < bs.Length
            next_len = bs(i)
            If bs(i + 33) >= 32 Then
                If ((bs(i + 25) >> 1) And 1) = 0 Then
                    'file
                    Array.Copy(bs, i + 2, bbbb, 0, 4)
                    lba = cvt32bit(bbbb)
                    Array.Copy(bs, i + 10, bbbb, 0, 4)
                    filesize = cvt32bit(bbbb)
                    str_len = bs(i + 32)
                    Array.Resize(na, str_len)
                    Array.Copy(bs, i + 33, na, 0, str_len)
                    name = Encoding.GetEncoding(0).GetString(na)
                    If ((lba << 11) + filesize) > iso_len Then
                        error_file.Append(lba)
                        error_file.Append(",")
                        error_file.Append(name)
                        error_file.Append(",")
                        error_file.AppendLine(filesize.ToString)
                    Else
                        Dim fss As New FileStream(iso, FileMode.Open, FileAccess.Read)
                        If File.Exists(basepath & name) Then
                            File.Delete(basepath & name)
                        End If
                        Dim save As New FileStream(basepath & name, FileMode.CreateNew, FileAccess.Write)
                        Dim bss(filesize - 1) As Byte
                        fss.Seek(lba << 11, SeekOrigin.Begin)
                        fss.Read(bss, 0, bss.Length)
                        fss.Close()
                        save.Write(bss, 0, bss.Length)
                        save.Close()
                    End If
                End If
            End If

            i += next_len

            If bs(i) = 0 Then
                nextlba -= 1
                If nextlba > 0 Then
                    i = 0
                    fs.Read(bs, 0, 2048)
                Else
                    Exit While
                End If
            End If
        End While

        fs.Close()
        Return error_file.ToString
    End Function

    Function getlba(ByVal tt As TreeNode, ByRef sender As Object) As String
        Dim base As String = tt.FullPath & "\"
        If sender Is OFFSETPATHToolStripMenuItem Then
            base = base.Replace(TreeView1.SelectedNode.FullPath, "")
        End If

        Dim dst As Integer = CInt(tt.Tag.ToString)
        Dim lba As Integer = 0
        Dim lba_base As Integer = dst << 11


        Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
        Dim next_len As Integer
        Dim filesize As Integer
        Dim iso_len As Long = fs.Length
        Dim str_len As Integer
        Dim name As String
        Dim i As Integer = 0
        Dim bb(1) As Byte
        Dim bbbb(3) As Byte
        Dim yyyymmdd(6) As Byte
        Dim na As Byte() = Nothing
        Dim bs(2047) As Byte

        Dim seek_parent_node As New TreeNode
        Dim arr As TreeNode() = TreeView1.Nodes.Find((CInt(tt.Name) + 1).ToString, True)
        Dim nextlba As Integer = -dst
        If arr.Length > 0 Then
            nextlba += CInt(arr(0).Tag.ToString)
        Else
            fs.Seek((CInt(tt.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)
            While i < 2048
                next_len = bs(i)
                If bs(i + 33) >= 32 Then
                    Array.Copy(bs, i + 2, bbbb, 0, 4)
                    lba = cvt32bit(bbbb)
                    Array.Copy(bs, i + 10, bbbb, 0, 4)
                    filesize = cvt32bit(bbbb)
                    str_len = bs(i + 32)
                    Array.Resize(na, str_len)
                    Array.Copy(bs, i + 33, na, 0, str_len)
                    name = Encoding.GetEncoding(0).GetString(na)
                    'フォルダかつLBAがおなじ
                    If ((bs(i + 25) >> 1) And 1) = 1 Then
                        If dst = lba Then
                            Exit While
                        End If
                    End If
                End If

                i += next_len

                '見つかるまで無限ループ
                If bs(i) = 0 Then
                    fs.Read(bs, 0, 2048)
                    i = 0
                End If
            End While
            nextlba = (filesize >> 11) - 1
            i = 0
        End If

        fs.Seek(lba_base, SeekOrigin.Begin)
        fs.Read(bs, 0, 2048)
        Dim sb As New StringBuilder

        While i < bs.Length
            next_len = bs(i)
            If bs(i + 33) >= 32 Then
                If ((bs(i + 25) >> 1) And 1) = 0 Then
                    'file
                    Array.Copy(bs, i + 2, bbbb, 0, 4)
                    lba = cvt32bit(bbbb)
                    Array.Copy(bs, i + 10, bbbb, 0, 4)
                    filesize = cvt32bit(bbbb)
                    str_len = bs(i + 32)
                    Array.Resize(na, str_len)
                    Array.Copy(bs, i + 33, na, 0, str_len)
                    name = Encoding.GetEncoding(0).GetString(na)

                    Array.Copy(bs, i + 18, yyyymmdd, 0, 7)
                    str_len = bs(i + 32)
                    Array.Resize(na, str_len)
                    Array.Copy(bs, i + 33, na, 0, str_len)
                    name = Encoding.GetEncoding(0).GetString(na)

                    If ((lba << 11) + filesize) > iso_len Then
                        sb.Append("!!")
                    End If
                    sb.Append(lba.ToString.PadLeft(7, "0"c))
                    sb.Append(vbTab)
                    sb.Append(base)
                    sb.Append(name)
                    sb.Append(vbTab)
                    sb.Append(filesize)
                    sb.Append(vbTab)
                    sb.Append(cvt_date(yyyymmdd))
                    If localtime.Checked Then
                        sb.AppendLine()
                    Else
                        sb.Append(vbTab)
                        sb.AppendLine(cvt_utc(yyyymmdd(6)))
                    End If
                End If

            End If

            i += next_len

            If bs(i) = 0 Then
                nextlba -= 1
                If nextlba > 0 Then
                    i = 0
                    fs.Read(bs, 0, 2048)
                Else
                    Exit While
                End If
            End If
        End While
        fs.Close()

        Return sb.ToString
    End Function

    Private Function GetAllNodes(ByVal Nodes As TreeNodeCollection) As ArrayList

        Dim Ar As New ArrayList
        Dim Node As TreeNode

        For Each Node In Nodes
            Ar.Add(Node)
            If Node.GetNodeCount(False) > 0 Then
                Ar.AddRange(GetAllNodes(Node.Nodes))
            End If
        Next

        Return Ar

    End Function

#End Region

End Class

Class ListViewItemComparer
    Implements IComparer

    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
       Implements IComparer.Compare
        Dim xx As String = CType(x, ListViewItem).SubItems(col).Text
        Dim yy As String = CType(y, ListViewItem).SubItems(col).Text
        If xx.Length <> yy.Length AndAlso col > 0 Then
            xx = xx.PadLeft(yy.Length, " "c)
            yy = yy.PadLeft(xx.Length, " "c)
            xx = xx.PadLeft(yy.Length, " "c)
        End If
        Return [String].Compare(xx, yy)
    End Function

End Class
