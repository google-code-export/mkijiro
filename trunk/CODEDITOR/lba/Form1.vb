Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.Diagnostics

Public Class Form1
    Dim iso As String = "NULL"
    Dim cso As Boolean = False
    Dim lssort As Integer = 1
    Dim virtual_item As ListViewItem()
    Dim ArG As New ArrayList

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
        'LBA
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(1)
        DoubleBuffered = True

        If File.Exists(Application.StartupPath & "\conf") Then
            Dim fs As New System.IO.FileStream(Application.StartupPath & "\conf", System.IO.FileMode.Open, System.IO.FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            fs.Read(bs, 0, bs.Length)
            If bs.Length >= 5 Then
                If bs(0) = 1 Then
                    uid_parent.Checked = True
                End If
                If bs(1) = 1 Then
                    gridview.Checked = True
                End If
                If bs(2) = 1 Then
                    localtime.Checked = True
                End If
                Me.ListView1.ListViewItemSorter = New ListViewItemComparer(bs(3))
                lssort = bs(3)
                If bs(4) = 1 Then
                    VIRTUAL.Checked = True
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
        Dim bs(4) As Byte
        If uid_parent.Checked = True Then
            bs(0) = 1
        End If
        If gridview.Checked = True Then
            bs(1) = 1
        End If
        If localtime.Checked = True Then
            bs(2) = 1
        End If
        Dim bb As Byte() = BitConverter.GetBytes(lssort)
        Array.Copy(bb, 0, bs, 3, 1)
        If VIRTUAL.Checked = True Then
            bs(4) = 1
        End If
        fs.Write(bs, 0, 5)
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
            Dim num(50000) As Integer
            Dim parent(50000) As Integer
            Dim level(50000) As Integer
            Dim lba As Integer
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


            fs.Read(bs, 0, 4)
            If bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                cso = True
            Else
                cso = False
            End If

            TreeView1.BeginUpdate()

            If cso = False Then
                fs.Seek(&H8084, SeekOrigin.Begin)
                fs.Read(bbbb, 0, 4)
                'パステーブルサイズ
                table_len = cvt32bit(bbbb)
                'リトルエンディアンパステーブル(L型)
                fs.Seek(&H808C, SeekOrigin.Begin)
                fs.Read(bbbb, 0, 4)
                lba = cvt32bit(bbbb) << 11
                fs.Seek(lba, SeekOrigin.Begin)
                'LBA読み込みサイズを拡張
                Array.Resize(bs, table_len + 1)
                fs.Read(bs, 0, table_len)
            Else
                bs = unpack_cso(16)
                Array.Copy(bs, &H84, bbbb, 0, 4)
                table_len = cvt32bit(bbbb)
                Array.Copy(bs, &H8C, bbbb, 0, 4)
                lba = cvt32bit(bbbb)
                Array.Resize(bs, table_len)
                bs = unpack_cso(lba)
            End If

            parent_node = TreeView1.Nodes(0)

            Dim Ar As New ArrayList
            Ar = GetAllNodes(TreeView1.Nodes)

            While i < table_len
                '文字の長さ
                str_len = bs(i)
                'LBA
                Array.Copy(bs, i + 2, bbbb, 0, 4)
                lba = cvt32bit(bbbb)
                '親のUID番号
                Array.Copy(bs, i + 6, bb, 0, 2)
                parent(k) = cvt16bit(bb)
                'ディレクトリの名前
                Array.Resize(na, str_len)
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
                '検索リストからノードに変換する
                seek_parent_node = CType(Ar(parent(k)), TreeNode)
                seek_parent_node.Nodes.Add(x)
                '検索リストに追加したノードを追加
                Ar.Add(seek_parent_node.Nodes(seek_parent_node.Nodes.Count - 1))

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
            TreeView1.SelectedNode = TreeView1.Nodes(0)
            TreeView1.Focus()
            TreeView1.EndUpdate()
            TextBox1.Text = sb.ToString
            fs.Close()

            '検索リストをキャッシュしておく
            ArG = Ar

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
            If VIRTUAL.Checked = False Then
                getlist(CInt(TreeView1.SelectedNode.Tag))
            Else
                getlist_virtual(CInt(TreeView1.SelectedNode.Tag))
            End If
        End If
    End Sub

    Private Sub VIRTUAL_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles VIRTUAL.CheckedChanged
        If TreeView1.SelectedNode IsNot Nothing Then
            If VIRTUAL.Checked = False Then
                getlist(CInt(TreeView1.SelectedNode.Tag))
            Else
                getlist_virtual(CInt(TreeView1.SelectedNode.Tag))
            End If
        End If
    End Sub
#End Region

#Region "TREE_LIST"
    Private Sub TreeView1_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        If VIRTUAL.Checked = False Then

            getlist(CInt(TreeView1.SelectedNode.Tag))
        Else
            getlist_virtual(CInt(TreeView1.SelectedNode.Tag))
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick

        Dim itemx As New ListViewItem
        If VIRTUAL.Checked = False Then
            If ListView1.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            itemx = ListView1.SelectedItems(0)
        Else
            If ListView1.SelectedIndices.Count = 0 Then
                Exit Sub
            End If
            itemx = ListView1.Items(ListView1.SelectedIndices(0))
        End If

        If itemx.Index = 0 AndAlso itemx.Text = ".." Then
            TreeView1.SelectedNode = TreeView1.SelectedNode.Parent
            If VIRTUAL.Checked = False Then
                getlist(CInt(TreeView1.SelectedNode.Tag))
            Else
                getlist_virtual(CInt(TreeView1.SelectedNode.Tag))
            End If
        ElseIf itemx.ImageIndex = 0 Then
            Dim seek_node As New TreeNode
            Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
            For Each ss As TreeNode In TreeView1.SelectedNode.Nodes
                If s = ss.Tag.ToString Then
                    TreeView1.SelectedNode = ss
                    Exit For
                End If
            Next
            If VIRTUAL.Checked = False Then
                getlist(CInt(TreeView1.SelectedNode.Tag))
            Else
                getlist_virtual(CInt(TreeView1.SelectedNode.Tag))
            End If
        End If
    End Sub

    Private Sub ColumnClick(ByVal o As Object, ByVal e As ColumnClickEventArgs) Handles ListView1.ColumnClick
        ' Set the ListViewItemSorter property to a new ListViewItemComparer 
        ' object. Setting this property immediately sorts the 
        ' ListView using the ListViewItemComparer object.
        If e.Column < 4 Then
            Me.ListView1.ListViewItemSorter = New ListViewItemComparer(e.Column)
            lssort = e.Column
            If VIRTUAL.Checked Then
                ListView1.BeginUpdate()
                Dim keys As Integer() = New Integer(virtual_item.Length - 1) {}
                Dim ss(virtual_item.Length - 1) As String
                For i = 0 To keys.Length - 1
                    ss(i) = virtual_item(i).SubItems(lssort).Text
                    If lssort > 0 Then
                        ss(i) = ss(i).PadLeft(10, " "c)
                    End If
                Next
                Array.Sort(ss, virtual_item)
                ListView1.VirtualListSize = virtual_item.Length
                ListView1.EndUpdate()
            End If
        End If
    End Sub

    Private Sub ListView1_Selected_all(sender As System.Object, ByVal e As KeyEventArgs) Handles ListView1.KeyDown
        If e.Control = True Then
            If e.KeyValue = Keys.A Then
                If VIRTUAL.Checked = False Then
                    For Each itemx As ListViewItem In ListView1.Items
                        itemx.Selected = True
                    Next
                ElseIf virtual_item.Length > 0 Then
                    ListView1.BeginUpdate()
                    ListView1.SelectedIndices.Clear()
                    For i = 0 To virtual_item.Length - 1
                        ListView1.SelectedIndices.Add(i)
                    Next
                    ListView1.EndUpdate()
                End If
            End If
        End If
    End Sub

#End Region

#Region "CONTEXT"

    Private Sub TREECOLLASEToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TREECOLLASEToolStripMenuItem.Click
        TreeView1.BeginUpdate()
        TreeView1.CollapseAll()
        TreeView1.EndUpdate()
    End Sub

    Private Sub TREEEXPANDToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TREEEXPANDToolStripMenuItem.Click
        TreeView1.BeginUpdate()
        TreeView1.ExpandAll()
        TreeView1.EndUpdate()
    End Sub

    Private Sub GETDATAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GETDATAToolStripMenuItem.Click

        Dim z As Integer
        If VIRTUAL.Checked = False Then
            If ListView1.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            z = ListView1.SelectedItems.Count - 1
        Else
            If ListView1.SelectedIndices.Count = 0 Then
                Exit Sub
            End If
            z = ListView1.SelectedIndices.Count - 1
        End If

        Dim itemx As New ListViewItem
        Dim errorm As New StringBuilder

        For k = 0 To z
            itemx = ListView1.Items(ListView1.SelectedIndices(k))

            If (itemx.ImageIndex And 2) <> 0 Then
                If File.Exists(iso) Then
                    Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
                    Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
                    Dim ss As String = ListView1.Items(itemx.Index).SubItems(2).Text
                    Dim iso_len As Long = fs.Length
                    If cso = True Then
                        Dim bbbb(3) As Byte
                        fs.Seek(8, SeekOrigin.Begin)
                        fs.Read(bbbb, 0, 4)
                        iso_len = cvt32bit(bbbb)
                    End If
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
                        If cso = False Then
                            fs.Seek(CInt(s) << 11, SeekOrigin.Begin)
                            fs.Read(bs, 0, bs.Length)
                        Else
                            Dim filesize As Integer = CInt(ss)
                            Dim count As Integer = (filesize >> 11) + 1
                            Dim lba As Integer = CInt(s)
                            Dim binn(2047) As Byte
                            Array.Resize(bs, count << 11)
                            For j = 0 To count - 1
                                binn = unpack_cso(lba)
                                If (lba + 1) << 11 < iso_len Then
                                    lba += 1
                                End If
                                Array.Copy(binn, 0, bs, j << 11, 2048)
                            Next
                            Array.Resize(bs, filesize)
                        End If
                        fs.Close()
                        save.Write(bs, 0, bs.Length)
                        save.Close()
                    End If
                End If
            ElseIf itemx.ImageIndex = 0 Then
                If itemx.Index = 0 AndAlso itemx.Text = ".." Then
                Else
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

                        errorm.Append(getfile(seek_node))
                        For Each tt As TreeNode In Ar
                            errorm.Append(getfile(tt))
                        Next

                    End If
                End If
            End If
        Next

        Beep()
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

        Beep()
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
        Beep()
    End Sub
#End Region

#Region "GETFUNC"

    Function cvt16bit(ByVal b As Byte()) As Integer
        Return BitConverter.ToInt16(b, 0)
    End Function

    Function cvt32bit(ByVal b As Byte()) As Integer
        Return BitConverter.ToInt32(b, 0)
    End Function

    Function unpack_cso(ByVal lba As Integer) As Byte()

        Dim cfs As New FileStream(iso, FileMode.Open, FileAccess.Read)
        Dim offset(7) As Byte
        Dim source(2047) As Byte
        Dim bss(23) As Byte
        Dim seek As Integer = 0
        cfs.Read(bss, 0, 24)
        Array.ConstrainedCopy(bss, 20, offset, 0, 4)
        Dim align As Integer = cvt32bit(offset) >> 8

        cfs.Seek(24 + lba * 4, System.IO.SeekOrigin.Begin)
        cfs.Read(offset, 0, 4)
        seek = cvt32bit(offset)
        Dim pos As Integer = (seek And &H7FFFFFFF) << align
        cfs.Read(offset, 0, 4)
        Dim pos2 As Integer = (cvt32bit(offset) And &H7FFFFFFF) << align

        cfs.Seek(pos, System.IO.SeekOrigin.Begin)
        cfs.Read(source, 0, pos2 - pos)

        If (seek And &H80000000) <> 0 Then
        Else
            If pos2 = pos Then
                Array.Clear(source, 0, 2048)
            Else
                Dim ms As New MemoryStream()
                ms.Write(source, 0, 2048)
                ms.Position = 0
                Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                zipStream.Read(source, 0, 2048)
                zipStream.Close()
                ms.Close()
            End If
        End If
        cfs.Close()

        Return source

    End Function

    Function getlist_virtual(ByVal dst As Integer) As Boolean
        Try
            ListView1.Clear()
            ListView1.View = View.Details
            ListView1.VirtualMode = True
            If dst < 0 Then
                Return True
            End If

            Array.Resize(virtual_item, 500000)

            ListView1.BeginUpdate()

            ListView1.Columns.Insert(0, "NAME", 150, HorizontalAlignment.Left)
            ListView1.Columns.Insert(1, "LBA", 60, HorizontalAlignment.Left)
            ListView1.Columns.Add("SIZE", 80, HorizontalAlignment.Left)
            If localtime.Checked Then
                ListView1.Columns.Add("DATE(LOCAL)", 150, HorizontalAlignment.Left)
            Else
                ListView1.Columns.Add("DATE", 150, HorizontalAlignment.Left)
                ListView1.Columns.Add("UTCDIFF", 50, HorizontalAlignment.Left)
            End If

            Dim lba As Integer = 0
            Dim lba_base As Integer = dst << 11
            Dim dst_next As Integer = dst

            Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
            Dim bs(2047) As Byte
            Dim bb(1) As Byte
            Dim bbbb(3) As Byte
            Dim next_len As Integer
            Dim filesize As Integer
            Dim str_len As Integer
            Dim iso_len As Long = fs.Length
            If cso = True Then
                fs.Seek(8, SeekOrigin.Begin)
                fs.Read(bbbb, 0, 4)
                iso_len = cvt32bit(bbbb)
            End If

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
                If cso = False Then
                    fs.Seek((CInt(TreeView1.SelectedNode.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
                    fs.Read(bs, 0, 2048)
                Else
                    bs = unpack_cso(CInt(TreeView1.SelectedNode.Parent.Tag.ToString))
                End If

                While True 'i < 2048
                    next_len = bs(i)
                    If bs(i + 33) >= 32 Then
                        'フォルダかつLBAがおなじ
                        If ((bs(i + 25) >> 1) And 1) = 1 Then
                            Array.Copy(bs, i + 2, bbbb, 0, 4)
                            lba = cvt32bit(bbbb)
                            If dst = lba Then
                                Array.Copy(bs, i + 10, bbbb, 0, 4)
                                filesize = cvt32bit(bbbb)
                                str_len = bs(i + 32)
                                Array.Resize(na, str_len)
                                Array.Copy(bs, i + 33, na, 0, str_len)
                                name = Encoding.GetEncoding(0).GetString(na)
                                Exit While
                            End If
                        End If
                    End If

                    i += next_len

                    '見つかるまで無限ループ
                    If bs(i) = 0 Then
                        If cso = False Then
                            fs.Read(bs, 0, 2048)
                        Else
                            dst_next += 1
                            bs = unpack_cso(dst_next)
                        End If
                        i = 0
                    End If
                End While
                nextlba = (filesize >> 11) - 1
                i = 0
            End If

            If cso = False Then
                fs.Seek(lba_base, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
            Else
                bs = unpack_cso(dst)
            End If

            Dim vi As Integer = 0
            
            Dim itemx As New ListViewItem
            itemx.Text = ".."
            itemx.ImageIndex = 0
            itemx.SubItems.Add("")
            itemx.SubItems.Add("")
            itemx.SubItems.Add("")
            If localtime.Checked = False Then
                itemx.SubItems.Add("")
            End If
            If TreeView1.SelectedNode.Parent.Level > 0 Then
                virtual_item(0) = itemx
                vi = 1
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

                    itemx = CType(itemx.Clone, ListViewItem)
                    itemx.Text = name
                    If ((bs(i + 25) >> 1) And 1) = 0 Then
                        itemx.ImageIndex = 2
                    Else
                        itemx.ImageIndex = 0
                    End If
                    If ((lba << 11) + filesize) > iso_len Then
                        itemx.ImageIndex = itemx.ImageIndex Or 1
                    End If
                    itemx.SubItems(1).Text = lba.ToString
                    itemx.SubItems(2).Text = (filesize.ToString)
                    itemx.SubItems(3).Text = (cvt_date(yyyymmdd))
                    If localtime.Checked = False Then
                        itemx.SubItems(4).Text = (cvt_utc(yyyymmdd(6)))
                    End If

                    virtual_item(vi) = itemx
                    vi += 1
                End If

                i += next_len

                If bs(i) = 0 Then
                    nextlba -= 1
                    If nextlba > 0 Then
                        i = 0
                        If cso = False Then
                            fs.Read(bs, 0, 2048)
                        Else
                            dst += 1
                            bs = unpack_cso(dst)
                        End If
                    Else
                        Exit While
                    End If
                End If
            End While

            Array.Resize(virtual_item, vi)

            Dim keys As Integer() = New Integer(virtual_item.Length - 1) {}
            Dim ss(virtual_item.Length - 1) As String
            For i = 0 To keys.Length - 1
                ss(i) = virtual_item(i).SubItems(lssort).Text
                If lssort > 0 Then
                    ss(i) = ss(i).PadLeft(10, " "c)
                End If
            Next

            Array.Sort(ss, virtual_item)

            ListView1.VirtualListSize = vi
            ListView1.EndUpdate()

            updatedir()

            Return True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return True
        End Try
    End Function

    Private Sub ListView1_RetrieveVirtualItem(ByVal sender As Object, ByVal e As System.Windows.Forms.RetrieveVirtualItemEventArgs) Handles ListView1.RetrieveVirtualItem
        'リストビューにリストをセット      
        Dim itemx As New ListViewItem
        itemx = CType(virtual_item(e.ItemIndex).Clone, ListViewItem)
        e.Item = itemx

    End Sub

    Function updatedir() As Boolean
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
        Return True
    End Function

    Function getlist(ByVal dst As Integer) As Boolean
        Try
            ListView1.Clear()
            ListView1.View = View.Details
            ListView1.VirtualMode = False
            If dst < 0 Then
                Return True
            End If
            Dim arraylist(50000) As ListViewItem

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
                Dim dst_next As Integer = dst

                Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
                Dim bs(2047) As Byte
                Dim bb(1) As Byte
                Dim bbbb(3) As Byte
                Dim next_len As Integer
                Dim filesize As Integer
                Dim str_len As Integer
                Dim iso_len As Long = fs.Length
                If cso = True Then
                    fs.Seek(8, SeekOrigin.Begin)
                    fs.Read(bbbb, 0, 4)
                    iso_len = cvt32bit(bbbb)
                End If

                Dim name As String
                Dim i As Integer
                Dim yyyymmdd(6) As Byte
                Dim na As Byte() = Nothing


                Dim seek_parent_node As New TreeNode
                Dim arr As TreeNode() = TreeView1.Nodes.Find((CInt(TreeView1.SelectedNode.Name) + 1).ToString, True)
                Dim nextlba As Integer = -dst
                If arr.Length > 0 Then
                    nextlba += CInt(arr(0).Tag.ToString)
                ElseIf cso = False Then

                    If cso = False Then
                        fs.Seek((CInt(TreeView1.SelectedNode.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                    Else
                        bs = unpack_cso(CInt(TreeView1.SelectedNode.Parent.Tag.ToString))
                    End If

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
                            If cso = False Then
                                fs.Read(bs, 0, 2048)
                            Else
                                dst_next += 1
                                bs = unpack_cso(dst_next)
                            End If
                            i = 0
                        End If
                    End While
                    nextlba = (filesize >> 11) - 1
                    i = 0
                Else

                End If

                If cso = False Then
                    fs.Seek(lba_base, SeekOrigin.Begin)
                    fs.Read(bs, 0, 2048)
                Else
                    bs = unpack_cso(dst)
                End If

                Dim ni As Integer = 0

                ListView1.BeginUpdate()

                Dim itemx As New ListViewItem
                itemx.Text = ".."
                itemx.ImageIndex = 0
                itemx.SubItems.Add("")
                itemx.SubItems.Add("")
                itemx.SubItems.Add("")
                If localtime.Checked = False Then
                    itemx.SubItems.Add("")
                End If
                If TreeView1.SelectedNode.Parent.Level > 0 Then
                    arraylist(0) = itemx
                    ni = 1
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
                        itemx = CType(itemx.Clone, ListViewItem)
                        itemx.Text = name
                        If ((bs(i + 25) >> 1) And 1) = 0 Then
                            itemx.ImageIndex = 2
                        Else
                            itemx.ImageIndex = 0
                        End If
                        If ((lba << 11) + filesize) > iso_len Then
                            itemx.ImageIndex = itemx.ImageIndex Or 1
                        End If
                        itemx.SubItems(1).Text = lba.ToString
                        itemx.SubItems(2).Text = (filesize.ToString)
                        itemx.SubItems(3).Text = (cvt_date(yyyymmdd))
                        If localtime.Checked = False Then
                            itemx.SubItems(4).Text = (cvt_utc(yyyymmdd(6)))
                        End If

                        arraylist(ni) = itemx
                        ni += 1
                    End If

                    i += next_len

                    If bs(i) = 0 Then
                        nextlba -= 1
                        If nextlba > 0 Then
                            i = 0
                            If cso = False Then
                                fs.Read(bs, 0, 2048)
                            Else
                                dst += 1
                                bs = unpack_cso(dst)
                            End If
                        Else
                            Exit While
                        End If
                    End If
                End While

                Array.Resize(arraylist, ni)
                ListView1.Items.AddRange(arraylist)

                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                ListView1.Columns(zz).Width = finalcol

                ListView1.EndUpdate()
                
                updatedir()

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
        z = z And 3
        If z <> 0 Then
            sb.Append(z * 15)
        Else
            sb.Append("0")
        End If

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
            z = z And 3
            If z <> 0 Then
                dtBirth = dtBirth.AddMinutes(-(z * 15))
            End If
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
        Dim dst_next As Integer = dst

        Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
        Dim next_len As Integer
        Dim filesize As Integer
        Dim str_len As Integer
        Dim name As String
        Dim iso_len As Long = fs.Length
        Dim i As Integer = 0
        Dim bb(1) As Byte
        Dim bbbb(3) As Byte
        If cso = True Then
            fs.Seek(8, SeekOrigin.Begin)
            fs.Read(bbbb, 0, 4)
            iso_len = cvt32bit(bbbb)
        End If
        Dim na As Byte() = Nothing
        Dim bs(2047) As Byte
        Dim error_file As New StringBuilder


        Dim find As Boolean = False
        Dim nextlba As Integer = -dst
        If ArG.Count > CInt(tt.Name) + 1 Then
            Dim seek_parent_node As New TreeNode
            seek_parent_node = CType(ArG(CInt(tt.Name) + 1), TreeNode)
            nextlba += CInt(seek_parent_node.Tag.ToString)
            find = True
        End If

        If find = False Then
            If cso = False Then
                fs.Seek((CInt(tt.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
            Else
                bs = unpack_cso(CInt(tt.Parent.Tag.ToString))
            End If

            While i < 2048
                next_len = bs(i)
                'フォルダかつLBAがおなじ
                If ((bs(i + 25) >> 1) And 1) = 1 Then
                    If bs(i + 33) >= 32 Then
                        Array.Copy(bs, i + 2, bbbb, 0, 4)
                        lba = cvt32bit(bbbb)
                        If dst = lba Then
                            Array.Copy(bs, i + 10, bbbb, 0, 4)
                            filesize = cvt32bit(bbbb)
                            str_len = bs(i + 32)
                            Array.Resize(na, str_len)
                            Array.Copy(bs, i + 33, na, 0, str_len)
                            name = Encoding.GetEncoding(0).GetString(na)
                            Exit While
                        End If
                    End If
                End If

                i += next_len

                '見つかるまで無限ループ
                If bs(i) = 0 Then
                    If cso = False Then
                        fs.Read(bs, 0, 2048)
                    Else
                        dst_next += 1
                        bs = unpack_cso(dst_next)
                    End If
                    i = 0
                End If
            End While
            nextlba = (filesize >> 11) - 1
            i = 0
        End If

        If cso = False Then
            fs.Seek(lba_base, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)
        Else
            bs = unpack_cso(dst)
        End If

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
                        Dim bss(filesize - 1) As Byte
                        Dim save As New FileStream(basepath & name, FileMode.CreateNew, FileAccess.Write)
                        If cso = False Then
                            fss.Seek(lba << 11, SeekOrigin.Begin)
                            fss.Read(bss, 0, bss.Length)
                        Else
                            Dim count As Integer = (filesize >> 11) + 1
                            Dim binn(2047) As Byte
                            Array.Resize(bss, count << 11)
                            For k = 0 To count - 1
                                binn = unpack_cso(lba)
                                If (lba + 1) << 11 < iso_len Then
                                    lba += 1
                                End If
                                Array.Copy(binn, 0, bss, k << 11, 2048)
                            Next
                            Array.Resize(bss, filesize)
                        End If
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
                    If cso = False Then
                        fs.Read(bs, 0, 2048)
                    Else
                        dst += 1
                        bs = unpack_cso(dst)
                    End If
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
        Dim dst_next As Integer = dst
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
        If cso = True Then
            fs.Seek(8, SeekOrigin.Begin)
            fs.Read(bbbb, 0, 4)
            iso_len = cvt32bit(bbbb)
        End If
        Dim yyyymmdd(6) As Byte
        Dim na As Byte() = Nothing
        Dim bs(2047) As Byte


        Dim find As Boolean = False
        Dim nextlba As Integer = -dst
        If ArG.Count > CInt(tt.Name) + 1 Then
            Dim seek_parent_node As New TreeNode
            seek_parent_node = CType(ArG(CInt(tt.Name) + 1), TreeNode)
            nextlba += CInt(seek_parent_node.Tag.ToString)
            find = True
        End If

        If find = False Then
            If cso = False Then
                fs.Seek((CInt(tt.Parent.Tag.ToString)) << 11, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
            Else
                bs = unpack_cso(CInt(tt.Parent.Tag.ToString))
            End If

            While i < 2048
                next_len = bs(i)
                If bs(i + 33) >= 32 Then
                    Array.Copy(bs, i + 2, bbbb, 0, 4)
                    lba = cvt32bit(bbbb)
                    If ((bs(i + 25) >> 1) And 1) = 1 Then
                        Array.Copy(bs, i + 10, bbbb, 0, 4)
                        filesize = cvt32bit(bbbb)
                        str_len = bs(i + 32)
                        Array.Resize(na, str_len)
                        Array.Copy(bs, i + 33, na, 0, str_len)
                        name = Encoding.GetEncoding(0).GetString(na)
                        'フォルダかつLBAがおなじ
                        If dst = lba Then
                            Exit While
                        End If
                    End If
                End If

                i += next_len

                '見つかるまで無限ループ
                If bs(i) = 0 Then
                    i = 0
                    If cso = False Then
                        fs.Read(bs, 0, 2048)
                    Else
                        dst_next += 1
                        bs = unpack_cso(dst_next)
                    End If
                End If
            End While
            nextlba = (filesize >> 11) - 1
            i = 0
        End If

        If cso = False Then
            fs.Seek(lba_base, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)
        Else
            bs = unpack_cso(dst)
        End If

        Dim sb As New StringBuilder

        While i < bs.Length
            next_len = bs(i)
            If ((bs(i + 25) >> 1) And 1) = 0 Then
                If bs(i + 33) >= 32 Then
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
                    If cso = False Then
                        fs.Read(bs, 0, 2048)
                    Else
                        dst += 1
                        bs = unpack_cso(dst)
                    End If
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
        If col = 5 Then
            col = 4
        End If
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
