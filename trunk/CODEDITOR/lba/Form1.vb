Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class Form1
    Dim iso As String = "NULL"
    Dim confver As Integer = 1 'version
    Dim cso As Boolean = False
    Dim lssort As Integer = 1
    Dim force_offset As Boolean
    Dim virtual_item As ListViewItem()
    Dim arraylistdir() As ListViewItem
    Dim startpath As String
    Dim ArG As New ArrayList
    Dim cFormat As New System.Globalization.CultureInfo("ja-JP", False)
    Dim buffer As Integer
    Dim col_len As Integer() = {180, 60, 80, 140, 80}

#Region "FORM"
    Private Sub ffload(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim start As DateTime = Now


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
        DoubleBuffered = True

        If File.Exists(Application.StartupPath & "\conf") Then
            Dim fs As New System.IO.FileStream(Application.StartupPath & "\conf", System.IO.FileMode.Open, System.IO.FileAccess.Read)
            Dim bs(CInt(fs.Length - 1)) As Byte
            Dim conf(3) As Byte
            fs.Read(conf, 0, 4)
            If confver = BitConverter.ToInt32(conf, 0) AndAlso fs.Length >= 28 Then
                fs.Read(bs, 0, bs.Length - 4)
                If bs(0) = 1 Then
                    uid_parent.Checked = True
                End If
                If bs(1) = 1 Then
                    gridview.Checked = True
                End If
                If bs(2) = 1 Then
                    localtime.Checked = True
                End If
                lssort = bs(3)
                If bs(4) = 1 Then
                    VIRTUAL.Checked = True
                End If
                If bs(5) = 1 Then
                    tree.Checked = True
                End If
                If bs(5) = 1 Then
                    tree.Checked = True
                End If
                fsbuf.Text = bs(6).ToString
                SAVEMODE.Text = Chr(bs(7))
                Dim int(3) As Byte
                Array.Copy(bs, 8, int, 0, 4)
                nodemax.Text = BitConverter.ToInt32(int, 0).ToString
                Array.Copy(bs, 12, int, 0, 4)
                addlistmax.Text = BitConverter.ToInt32(int, 0).ToString
                Array.Copy(bs, 16, int, 0, 4)
                vlistmax.Text = BitConverter.ToInt32(int, 0).ToString
                For i = 0 To 3
                    If bs(i + 20) > 50 Then
                        col_len(i) = bs(i + 20)
                    End If
                Next
                If bs.Length - 24 > 0 Then
                    Dim dr(bs.Length - 24 - 1) As Byte
                    Array.Copy(bs, 24, dr, 0, dr.Length)
                    sdir.Text = Encoding.GetEncoding(65001).GetString(dr)
                End If
            End If
            fs.Close()
        End If

        If Directory.Exists(sdir.Text) = False Then
            sdir.Text = Application.StartupPath & "\"
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


        Label5.Text = (Now - start).TotalSeconds.ToString

    End Sub

    Private Sub ffclose(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Array.Resize(virtual_item, 0)
        Array.Resize(arraylistdir, 0)
        ArG.Clear()

        Dim fs As New FileStream(Application.StartupPath & "\conf", System.IO.FileMode.Create, FileAccess.Write)
        Dim bs(23) As Byte
        Dim conf As Byte() = BitConverter.GetBytes(confver)
        fs.Write(conf, 0, 4)
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
        If tree.Checked = True Then
            bs(5) = 1
        End If
        bb = BitConverter.GetBytes(CInt(fsbuf.Text))
        Array.Copy(bb, 0, bs, 6, 1)
        Dim bb2 As Byte() = System.Text.Encoding.GetEncoding(0).GetBytes(SAVEMODE.Text)
        Array.Copy(bb2, 0, bs, 7, 1)
        bb = BitConverter.GetBytes(CInt(nodemax.Text))
        Array.Copy(bb, 0, bs, 8, 4)
        bb = BitConverter.GetBytes(CInt(addlistmax.Text))
        Array.Copy(bb, 0, bs, 12, 4)
        bb = BitConverter.GetBytes(CInt(vlistmax.Text))
        Array.Copy(bb, 0, bs, 16, 4)
        For i = 0 To 3
            If col_len(i) > 255 Then
                col_len(i) = 255
            End If
            bb = BitConverter.GetBytes(col_len(i))
            Array.Copy(bb, 0, bs, i + 20, 1)
        Next
        fs.Write(bs, 0, 24)
        If Directory.Exists(sdir.Text) Then
            Dim dr() As Byte = Encoding.GetEncoding(65001).GetBytes(sdir.Text)
            fs.Write(dr, 0, dr.Length)
        End If
        fs.Close()
    End Sub

    Private Sub ffDragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter, ListView1.DragEnter
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
            Handles Me.DragDrop, ListView1.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する
        Dim start As DateTime = Now

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

        Label5.Text = (Now - start).TotalSeconds.ToString
    End Sub

#End Region

#Region "BUTTON"

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TreeView1.Nodes.Clear()
        If File.Exists(iso) Then
            Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
            Dim str_len As Integer
            Dim max As Integer = CInt(nodemax.Text)
            Dim num(max) As Integer
            Dim parent(max) As Integer
            Dim level(max) As Integer
            Dim lba As Integer
            Dim lba_bk As Integer = 99
            Dim sort As Boolean = False
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

                If k = max Then
                    MessageBox.Show("ノード数限界に達しました")
                    Exit While
                End If

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
            If tree.Checked = True Then
                TreeView1.ExpandAll()
            End If
            TreeView1.EndUpdate()
            TreeView1.Focus()
            TreeView1.SelectedNode = TreeView1.Nodes(0)
            TextBox1.Text = sb.ToString
            fs.Close()

            '検索リストをキャッシュしておく
            'Ar.Sort()
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

    Private Sub openfile(sender As System.Object, e As System.EventArgs) Handles 開くToolStripMenuItem.Click
        Dim start As DateTime = Now
        Dim ofd As New OpenFileDialog()

        ofd.Filter = "ISO/CSOファイル(*.iso;*.cso)|*.iso;*.cso"
        ofd.Title = "開くファイルを選択してください"

        If ofd.ShowDialog() = DialogResult.OK Then
            iso = ofd.FileName
            Dim psf As New psf
            If File.Exists(iso) Then
                If psf.video(iso) <> "" Then
                    Button1_Click(sender, e)
                    title.Text = psf.GETNAME(iso)
                    discid.Text = psf.GETID(iso)
                Else
                    iso = ""
                End If
            End If
        End If

        Label5.Text = (Now - start).TotalSeconds.ToString
    End Sub

    Private Sub バージョンToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles バージョンToolStripMenuItem.Click
        Dim f As New ver
        f.ShowDialog()
        f.Dispose()
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
                If virtual_item.Length < CInt(addlistmax.Text) Then
                    getlist(CInt(TreeView1.SelectedNode.Tag))
                Else
                    MessageBox.Show(CInt(addlistmax.Text) & "項目以上あるのでVIRTUALMODE以外では表示できません")
                End If
            Else
                getlist_virtual(CInt(TreeView1.SelectedNode.Tag))
            End If
        End If
    End Sub

    Private Sub 設定ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 設定ToolStripMenuItem.Click
        Dim f As New Form3
        f.ShowDialog(Me)
        f.Dispose()
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

    Private Sub ListView1_Doubleclick(sender As System.Object, ByVal e As System.EventArgs) Handles RUNAPPToolStripMenuItem.Click, ListView1.DoubleClick

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

        If sender Is ListView1 Then
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
            ElseIf itemx.ImageIndex = 2 Then
                runapp(itemx)
            End If
        ElseIf itemx.ImageIndex = 2 Then
            runapp(itemx)
        End If
    End Sub

    Public Shared Function FindAssociatedExecutableFile( _
    ByVal fileName As String, ByVal extra As String) As String
        '拡張子を取得
        Dim extName As String = System.IO.Path.GetExtension(fileName)
        'ファイルタイプを取得
        Dim regKey As Microsoft.Win32.RegistryKey = _
            Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extName)
        If regKey Is Nothing Then
            Return ""
            'Throw New Exception("見つかりませんでした。")
        End If
        Dim fileType As String = CStr(regKey.GetValue(""))
        regKey.Close()

        '「アクションを実行するアプリケーション」を取得
        Dim regKey2 As Microsoft.Win32.RegistryKey = _
            Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( _
            String.Format("{0}\shell\{1}\command", fileType, extra))
        If regKey2 Is Nothing Then
            ' Throw New Exception("見つかりませんでした。")
            Return ""
        End If
        Dim command As String = CStr(regKey2.GetValue(""))
        regKey2.Close()

        Return command
    End Function

    ''' <summary>
    ''' ファイルに関連付けられた実行ファイルのパスを取得する
    ''' </summary>
    ''' <param name="fileName">関連付けを調べるファイル</param>
    ''' <returns>実行ファイルのパス + コマンドライン引数</returns>
    Public Shared Function FindAssociatedExecutableFile( _
        ByVal fileName As String) As String
        Return FindAssociatedExecutableFile(fileName, "open")
    End Function

    Function runapp(ByVal itemx As ListViewItem) As Boolean
        Dim temp As String = Application.StartupPath & "\tmp\" & (itemx.Text)
        Dim filess As String() = System.IO.Directory.GetFiles((Application.StartupPath & "\tmp"), "*", System.IO.SearchOption.AllDirectories)
        For i = 0 To filess.Length - 1
            File.Delete(filess(i))
        Next
        Dim exe As String = FindAssociatedExecutableFile(itemx.Text)
        If exe = "" Then
            MessageBox.Show("関連付けられたプログラムが見つかりませんでした。")
            Return False
        End If
        If File.Exists(iso) Then
            Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
            Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
            Dim ss As String = ListView1.Items(itemx.Index).SubItems(2).Text
            Dim lba As Integer = CInt(s)
            Dim filesize As Integer = CInt(ss)
            Dim iso_len As Long = fs.Length
            If cso = True Then
                Dim bbbb(3) As Byte
                fs.Seek(8, SeekOrigin.Begin)
                fs.Read(bbbb, 0, 4)
                iso_len = cvt32bit(bbbb)
            End If
            If ((CLng(s) << 11) + CLng(ss)) > iso_len Then
                fs.Close()
            Else
                If File.Exists(temp) Then
                    File.Delete(temp)
                End If
                Dim save As New FileStream(temp, FileMode.CreateNew, FileAccess.Write)
                If cso = False Then
                    fs.Seek(lba << 11, SeekOrigin.Begin)

                    If filesize > buffer AndAlso buffer <> 0 Then
                        Dim bss(buffer - 1) As Byte
                        Dim ct As Integer

                        While True
                            Dim readSize As Integer = fs.Read(bss, 0, bss.Length)
                            If readSize = 0 Then
                                Exit While
                            End If

                            ct += readSize
                            If ct > filesize Then
                                readSize = filesize - (ct - readSize)
                                save.Write(bss, 0, readSize)
                                Exit While
                            End If

                            save.Write(bss, 0, readSize)
                        End While
                    Else
                        Dim bss(filesize - 1) As Byte
                        fs.Read(bss, 0, bss.Length)
                        save.Write(bss, 0, bss.Length)
                    End If
                Else
                    Dim count As Integer = (filesize >> 11) + 1
                    If (filesize And &H7FF) = 0 AndAlso filesize > 0 Then
                        count -= 1
                    End If
                    Dim binn(2047) As Byte
                    For j = 1 To count
                        binn = unpack_cso(lba)
                        If (lba + 1) << 11 < iso_len Then
                            lba += 1
                        End If
                        If j = count Then
                            save.Write(binn, 0, filesize - (j << 11) + 2048)
                            Exit For
                        End If
                        save.Write(binn, 0, 2048)
                    Next
                End If
                save.Close()
                fs.Close()
            End If
        End If
        Process.Start(temp)
        Return (True)
    End Function

    Private Sub ColumnClick(ByVal o As Object, ByVal e As ColumnClickEventArgs) Handles ListView1.ColumnClick
        Dim total As Integer = TreeView1.SelectedNode.Nodes.Count
        If TreeView1.SelectedNode.Parent IsNot Nothing Then
            total += 1
        End If

        If e.Column < 4 Then
            Dim templist(total - 1) As ListViewItem
            ListView1.Columns(lssort).Text = ListView1.Columns(lssort).Text.Replace(" ▼", "")
            lssort = e.Column
            ListView1.BeginUpdate()

            If VIRTUAL.Checked = True Then
                Dim vi As Integer = virtual_item.Length
                Array.Copy(virtual_item, 0, templist, 0, total)
                Array.Reverse(virtual_item)
                Array.Resize(virtual_item, vi - total)
                templist = list_item_sort(total, templist)
                virtual_item = list_item_sort(virtual_item.Length, virtual_item)
                virtual_item = templist.Union(virtual_item).ToArray()

                ListView1.Columns(lssort).Text &= " ▼"

                ListView1.VirtualListSize = virtual_item.Length

            ElseIf VIRTUAL.Checked = False Then
                For i = 0 To total - 1
                    templist(i) = CType(ListView1.Items(0).Clone, ListViewItem)
                    ListView1.Items(0).Remove()
                Next
                Dim ls(ListView1.Items.Count - 1) As ListViewItem
                For i = 0 To ListView1.Items.Count - 1
                    ls(i) = CType(ListView1.Items(i).Clone, ListViewItem)
                Next
                ListView1.Clear()

                ListView1.View = View.Details
                Dim zz As Integer
                Dim finalcol As Integer

                templist = list_item_sort(templist.Length, templist)
                ls = list_item_sort(ls.Length, ls)

                ListView1.Columns.Add("NAME", col_len(0), HorizontalAlignment.Right)
                ListView1.Columns.Add("LBA", col_len(1), HorizontalAlignment.Right)
                ListView1.Columns.Add("SIZE", col_len(2), HorizontalAlignment.Right)
                If localtime.Checked Then
                    ListView1.Columns.Add("DATE(LOCAL)", col_len(3), HorizontalAlignment.Right)
                    finalcol = 150
                Else
                    ListView1.Columns.Add("DATE", col_len(3), HorizontalAlignment.Right)
                    ListView1.Columns.Add("UTCDIFF", col_len(4), HorizontalAlignment.Right)
                    finalcol = 80
                End If
                ListView1.Columns(lssort).Text &= " ▼"

                ls = templist.Union(ls).ToArray
                ListView1.Items.AddRange(ls)

                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                ListView1.Columns(zz).Width = finalcol
            End If
            ListView1.EndUpdate()
        End If
    End Sub

    Private Sub Columnlen(ByVal o As Object, ByVal e As ColumnWidthChangedEventArgs) Handles ListView1.ColumnWidthChanged
        If ListView1.Columns.Count > 0 Then
            For i = 0 To ListView1.Columns.Count - 1
                col_len(i) = ListView1.Columns(i).Width
            Next
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

    Private Sub ListView1_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles ListView1.ItemDrag

        If TreeView1.Nodes.Count > 0 Then
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
            If z >= 0 Then
                Dim itemx As New ListViewItem
                Dim files As String = ""
                Dim FileName(z) As String
                Dim kk As Integer = 0
                For k = 0 To z
                    itemx = ListView1.Items(ListView1.SelectedIndices(k))
                    files = itemx.Text
                    If files <> ".." AndAlso files.Trim <> "" Then
                        FileName(kk) = Application.StartupPath & "\tmp\" & files
                        kk += 1
                    End If
                Next
                Array.Resize(FileName, kk)
                Dim null As System.EventArgs = Nothing
                Dim myDataObject As New DataObject(DataFormats.FileDrop, FileName)
                'ListView1.DoDragDrop(myDataObject, DragDropEffects.Move)

                GETDATAToolStripMenuItem_Click(TextBox1, null)
                ListView1.DoDragDrop(myDataObject, DragDropEffects.Move)
                Dim filess As String() = System.IO.Directory.GetFiles((Application.StartupPath & "\tmp"), "*", System.IO.SearchOption.AllDirectories)
                For i = 0 To filess.Length - 1
                    File.Delete(filess(i))
                Next
            End If
        End If
    End Sub

    'ドラッグをキャンセルする
    Private Sub ListBox1_QueryContinueDrag(ByVal sender As Object, ByVal e As QueryContinueDragEventArgs) Handles ListView1.QueryContinueDrag, Me.QueryContinueDrag
        'マウスの右ボタンが押されていればドラッグをキャンセル
        '"2"はマウスの右ボタンを表す
        If (e.KeyState And 2) = 2 Then
            e.Action = DragAction.Cancel
        End If
    End Sub

#End Region

#Region "CONTEXT"

    Private Sub TREEEXPANDToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TREEEXPANDToolStripMenuItem.Click
        TreeView1.BeginUpdate()
        TreeView1.ExpandAll()
        TreeView1.EndUpdate()
    End Sub

    Private Sub TREECOLLASEToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TREECOLLASEToolStripMenuItem.Click
        TreeView1.BeginUpdate()
        TreeView1.CollapseAll()
        TreeView1.EndUpdate()
    End Sub

    Private Sub GETDATAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SAVEDATA.Click, SAVEDATAOFFSET.Click

        Dim start As DateTime = Now
        Dim z As Integer
        buffer = CInt(fsbuf.Text) * 1024 * 1024
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

        startpath = Application.StartupPath
        Select Case SAVEMODE.Text
            Case "S"
                startpath = sdir.Text
            Case "F"
                Dim fbd As New FolderBrowserDialog
                fbd.Description = "保存先を選んで下さい"
                fbd.ShowNewFolderButton = True
                If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    startpath = fbd.SelectedPath
                Else
                    Exit Sub
                End If
        End Select

        Dim m As Integer = 0
        If sender Is TextBox1 Then
            startpath = Application.StartupPath & "\tmp\"
            m = 1
        End If
        If sender Is SAVEDATAOFFSET Then
            m = 1
        End If
        If sender Is SAVEDATA Then
            m = 0
        End If

        startpath = startpath_fix(startpath)

        For k = 0 To z
            itemx = ListView1.Items(ListView1.SelectedIndices(k))

            If (itemx.ImageIndex And 2) <> 0 Then
                If File.Exists(iso) Then
                    Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
                    Dim s As String = ListView1.Items(itemx.Index).SubItems(1).Text
                    Dim ss As String = ListView1.Items(itemx.Index).SubItems(2).Text
                    Dim lba As Integer = CInt(s)
                    Dim filesize As Integer = CInt(ss)
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
                        Dim p As String = startpath
                        Dim dirpath As String = TreeView1.SelectedNode.FullPath & "\"
                        If m = 1 Then
                            dirpath = ""
                        End If
                        p &= dirpath
                        p &= itemx.Text
                        Directory.CreateDirectory(Path.GetDirectoryName(p))
                        If File.Exists(p) Then
                            File.Delete(p)
                        End If
                        Dim save As New FileStream(p, FileMode.CreateNew, FileAccess.Write)
                        If cso = False Then
                            fs.Seek(lba << 11, SeekOrigin.Begin)

                            If filesize > buffer AndAlso buffer <> 0 Then
                                Dim bss(buffer - 1) As Byte
                                Dim ct As Integer

                                While True
                                    Dim readSize As Integer = fs.Read(bss, 0, bss.Length)
                                    If readSize = 0 Then
                                        Exit While
                                    End If

                                    ct += readSize
                                    If ct > filesize Then
                                        readSize = filesize - (ct - readSize)
                                        save.Write(bss, 0, readSize)
                                        Exit While
                                    End If

                                    save.Write(bss, 0, readSize)
                                End While
                            Else
                                Dim bss(filesize - 1) As Byte
                                fs.Read(bss, 0, bss.Length)
                                save.Write(bss, 0, bss.Length)
                            End If
                        Else
                            Dim count As Integer = (filesize >> 11) + 1
                            If (filesize And &H7FF) = 0 AndAlso filesize > 0 Then
                                count -= 1
                            End If
                            Dim binn(2047) As Byte
                            For j = 1 To count
                                binn = unpack_cso(lba)
                                If (lba + 1) << 11 < iso_len Then
                                    lba += 1
                                End If
                                If j = count Then
                                    save.Write(binn, 0, filesize - (j << 11) + 2048)
                                    Exit For
                                End If
                                save.Write(binn, 0, 2048)
                            Next
                        End If
                        fs.Close()
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

                        errorm.Append(getfile(seek_node, m))
                        For Each tt As TreeNode In Ar
                            errorm.Append(getfile(tt, m))
                        Next

                    End If
                End If
            End If
        Next

        Beep()
        If errorm.Length > 0 AndAlso sender IsNot Me Then
            errorm.Insert(0, vbCrLf)
            errorm.Insert(0, "!!破損ファイルがありました")
            MessageBox.Show(errorm.ToString)
        End If

        Label5.Text = (Now - start).TotalSeconds.ToString
    End Sub

    Private Sub EXTRACTLBAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ABSOLUTPATHToolStripMenuItem.Click, OFFSETPATHToolStripMenuItem.Click
        If TreeView1.Nodes.Count > 0 Then
            Dim start As DateTime = Now
            Dim Ar As New ArrayList
            Ar = GetAllNodes(TreeView1.SelectedNode.Nodes)
            Dim title_st As String = title.Text
            Dim stbyte As Byte() = System.Text.Encoding.GetEncoding(932).GetBytes(title_st)
            title_st = System.Text.Encoding.GetEncoding(932).GetString(stbyte)

            Dim dosmoji As String() = {"\", "/", ":", "*", "?", """", "<", ">", "|", vbCr, vbLf}
            For i = 0 To 10
                title_st = title_st.Replace(dosmoji(i), "")
            Next

            startpath = Application.StartupPath
            Select Case SAVEMODE.Text
                Case "S"
                    startpath = sdir.Text
                Case "F"
                    Dim fbd As New FolderBrowserDialog
                    fbd.Description = "保存先を選んで下さい"
                    fbd.ShowNewFolderButton = True
                    If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
                        startpath = fbd.SelectedPath
                    Else
                        Exit Sub
                    End If
            End Select

            Dim switch As Integer = 0
            If sender Is OFFSETPATHToolStripMenuItem Then
                switch = 1
            End If

            startpath = startpath_fix(startpath)

            Dim sw As New System.IO.StreamWriter(startpath & title_st & ".txt", False, System.Text.Encoding.GetEncoding(65001))
            Dim sb As New StringBuilder
            Dim error_m As New StringBuilder
            If TreeView1.SelectedNode.Level > 0 Then
                sb.Append(getlba(TreeView1.SelectedNode, switch))
            End If

            For Each tt As TreeNode In Ar
                sb.Append(getlba(tt, switch))
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

            Dim er As New Regex("(^|\n)!!\d+\t.*?\t", RegexOptions.ECMAScript)
            Dim mm As Match = er.Match(sb.ToString)
            While mm.Success
                error_m.Append(mm.Value)
                mm = mm.NextMatch
            End While
            If error_m.Length > 0 Then
                error_m.Insert(0, vbCrLf)
                error_m.Insert(0, "破損ファイルがありました")
                MessageBox.Show(error_m.ToString)
            End If



            Label5.Text = (Now - start).TotalSeconds.ToString
        End If
    End Sub

    Private Sub EXTRACTDATA_TREE(sender As System.Object, e As System.EventArgs) Handles 絶対パスで展開ToolStripMenuItem.Click, 相対パスで展開ToolStripMenuItem.Click
        If TreeView1.Nodes.Count > 0 Then
            Dim start As DateTime = Now
            Dim Ar As New ArrayList
            Dim errorm As New StringBuilder
            Dim m As Integer = 0
            If sender Is 相対パスで展開ToolStripMenuItem Then
                m = 1
            End If

            startpath = Application.StartupPath
            Select Case SAVEMODE.Text
                Case "S"
                    startpath = sdir.Text
                Case "F"
                    Dim fbd As New FolderBrowserDialog
                    fbd.Description = "保存先を選んで下さい"
                    fbd.ShowNewFolderButton = True
                    If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
                        startpath = fbd.SelectedPath
                    Else
                        Exit Sub
                    End If
            End Select


            If m = 1 Then
                startpath = startpath_fix(startpath)
                startpath &= TreeView1.SelectedNode.Text
            End If

            startpath = startpath_fix(startpath)

            buffer = CInt(fsbuf.Text) * 1024 * 1024

            Ar = GetAllNodes(TreeView1.SelectedNode.Nodes)
            If TreeView1.SelectedNode.Level > 0 Then
                errorm.Append(getfile(TreeView1.SelectedNode, m))
            End If

            For Each tt As TreeNode In Ar
                errorm.Append(getfile(tt, m))
            Next

            Beep()
            If errorm.Length > 0 Then
                errorm.Insert(0, vbCrLf)
                errorm.Insert(0, "!!破損ファイルがありました")
                MessageBox.Show(errorm.ToString)
            End If

            Label5.Text = (Now - start).TotalSeconds.ToString
        End If
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
            Dim start As DateTime = Now
            ListView1.Clear()
            ListView1.View = View.Details
            ListView1.VirtualMode = True
            If dst < 0 Then
                Return True
            End If

            Array.Resize(virtual_item, CInt(vlistmax.Text))
            Array.Resize(arraylistdir, TreeView1.SelectedNode.Nodes.Count + 1)

            ListView1.BeginUpdate()

            ListView1.Columns.Insert(0, "NAME", col_len(0), HorizontalAlignment.Right)
            ListView1.Columns.Insert(1, "LBA", col_len(1), HorizontalAlignment.Right)
            ListView1.Columns.Add("SIZE", col_len(2), HorizontalAlignment.Right)
            If localtime.Checked Then
                ListView1.Columns.Add("DATE(LOCAL)", col_len(3), HorizontalAlignment.Right)
            Else
                ListView1.Columns.Add("DATE", col_len(3), HorizontalAlignment.Right)
                ListView1.Columns.Add("UTCDIFF", 80, HorizontalAlignment.Right)
            End If
            ListView1.Columns(lssort).Text &= " ▼"

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
            Dim mi As Integer = 0

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
                arraylistdir(0) = itemx
                mi = 1
            End If

            While i < bs.Length
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

                    itemx = CType(itemx.Clone, ListViewItem)
                    itemx.Text = name
                    itemx.SubItems(1).Text = lba.ToString
                    itemx.SubItems(2).Text = (filesize.ToString)

                    Array.Copy(bs, i + 18, yyyymmdd, 0, 7)
                    itemx.SubItems(3).Text = (cvt_date(yyyymmdd))
                    If localtime.Checked = False Then
                        itemx.SubItems(4).Text = (cvt_utc(yyyymmdd(6)))
                    End If
                    If ((bs(i + 25) >> 1) And 1) = 0 Then
                        itemx.ImageIndex = 2
                        If ((lba << 11) + filesize) > iso_len Then
                            itemx.ImageIndex = itemx.ImageIndex Or 1
                        End If
                        virtual_item(vi) = itemx
                        vi += 1
                    Else
                        itemx.ImageIndex = 0
                        If ((lba << 11) + filesize) > iso_len Then
                            itemx.ImageIndex = itemx.ImageIndex Or 1
                        End If
                        arraylistdir(mi) = itemx
                        mi += 1
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

            Array.Resize(virtual_item, vi)
            Array.Resize(arraylistdir, mi)
            virtual_item = list_item_sort(vi, virtual_item)
            arraylistdir = list_item_sort(mi, arraylistdir)

            virtual_item = arraylistdir.Union(virtual_item).ToArray()

            ListView1.VirtualListSize = vi + mi
            ListView1.EndUpdate()

            updatedir()
            fs.Close()

            Label5.Text = (Now - start).TotalSeconds.ToString
            Return True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return True
        End Try
    End Function

    Function startpath_fix(ByVal p As String) As String
        If p(p.Length - 1) <> "\" Then
            p &= "\"
        End If
        Return p
    End Function

    Private Sub ListView1_RetrieveVirtualItem(ByVal sender As Object, ByVal e As System.Windows.Forms.RetrieveVirtualItemEventArgs) Handles ListView1.RetrieveVirtualItem
        'リストビューにリストをセット      
        Dim itemx As New ListViewItem
        itemx = CType(virtual_item(e.ItemIndex).Clone, ListViewItem)
        e.Item = itemx

    End Sub

    Function list_item_sort(ByVal vi As Integer, ByVal ls As ListViewItem()) As ListViewItem()
        Dim ss(vi - 1) As String
        For i = 0 To vi - 1
            ss(i) = ls(i).SubItems(lssort).Text
            If lssort > 0 Then
                ss(i) = ss(i).PadLeft(10, " "c)
            End If
        Next

        Array.Sort(ss, ls)
        Return ls
    End Function

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
            Dim start As DateTime = Now
            ListView1.Clear()
            ListView1.View = View.Details
            ListView1.VirtualMode = False
            If dst < 0 Then
                Return True
            End If
            Dim max As Integer = CInt(addlistmax.Text)
            Dim arraylist(max) As ListViewItem
            Dim arraylistdir(TreeView1.SelectedNode.Nodes.Count + 1) As ListViewItem

            If File.Exists(iso) Then
                If TreeView1.SelectedNode.Level = 0 Then
                    Return True
                End If
                ListView1.View = View.Details
                ListView1.AutoSize = True
                Dim zz As Integer
                Dim finalcol As Integer

                ListView1.Columns.Add("NAME", col_len(0), HorizontalAlignment.Right)
                ListView1.Columns.Add("LBA", col_len(1), HorizontalAlignment.Right)
                ListView1.Columns.Add("SIZE", col_len(2), HorizontalAlignment.Right)
                If localtime.Checked Then
                    ListView1.Columns.Add("DATE(LOCAL)", col_len(3), HorizontalAlignment.Right)
                    zz = 3
                    finalcol = 150
                Else
                    ListView1.Columns.Add("DATE", col_len(3), HorizontalAlignment.Right)
                    ListView1.Columns.Add("UTCDIFF", col_len(4), HorizontalAlignment.Right)
                    zz = 4
                    finalcol = 80
                End If
                ListView1.Columns(lssort).Text &= " ▼"

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
                Dim mi As Integer = 0

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
                    arraylistdir(0) = itemx
                    mi = 1
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
                        itemx.SubItems(1).Text = lba.ToString
                        itemx.SubItems(2).Text = (filesize.ToString)
                        itemx.SubItems(3).Text = (cvt_date(yyyymmdd))
                        If localtime.Checked = False Then
                            itemx.SubItems(4).Text = (cvt_utc(yyyymmdd(6)))
                        End If


                        If ((bs(i + 25) >> 1) And 1) = 0 Then
                            itemx.ImageIndex = 2
                            If ((lba << 11) + filesize) > iso_len Then
                                itemx.ImageIndex = itemx.ImageIndex Or 1
                            End If
                            arraylist(ni) = itemx
                            ni += 1
                            max -= 1
                        Else
                            itemx.ImageIndex = 0
                            If ((lba << 11) + filesize) > iso_len Then
                                itemx.ImageIndex = itemx.ImageIndex Or 1
                            End If

                            arraylistdir(mi) = itemx
                            mi += 1
                            max -= 1
                        End If
                        If 0 = max Then
                            fs.Close()
                            MessageBox.Show(addlistmax.Text & "項目以上あるので表示できません,VIRTUALMODEを使用して下さい")
                            Exit While
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

                Array.Resize(arraylistdir, mi)
                Array.Resize(arraylist, ni)

                arraylist = list_item_sort(ni, arraylist)
                arraylistdir = list_item_sort(mi, arraylistdir)
                arraylist = arraylistdir.Union(arraylist).ToArray

                ListView1.Items.AddRange(arraylist)

                ListView1.Columns(zz).Width = finalcol

                ListView1.EndUpdate()

                updatedir()

                fs.Close()

                Label5.Text = (Now - start).TotalSeconds.ToString

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

    Function getfile(ByVal tt As TreeNode, ByVal sender As Integer) As String
        Dim basepath As String = startpath
        Dim dirpath As String = tt.FullPath & "\"
        If sender = 1 Then
            dirpath = dirpath.Replace(TreeView1.SelectedNode.FullPath & "\", "")
        End If
        basepath &= dirpath
        Directory.CreateDirectory(startpath & dirpath)
        Dim dst As Integer = CInt(tt.Tag.ToString)
        Dim lba As Integer = 0
        Dim lba_base As Long = dst << 11
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

                        Dim save As New FileStream(basepath & name, FileMode.CreateNew, FileAccess.Write)
                        If cso = False Then
                            fss.Seek(lba << 11, SeekOrigin.Begin)
                            If buffer <> 0 Then
                                If filesize > buffer Then
                                    Dim bss(buffer - 1) As Byte
                                    Dim ct As Integer

                                    While True
                                        Dim readSize As Integer = fs.Read(bss, 0, bss.Length)
                                        If readSize = 0 Then
                                            Exit While
                                        End If

                                        ct += readSize
                                        If ct > filesize Then
                                            readSize = filesize - (ct - readSize)
                                            save.Write(bss, 0, readSize)
                                            Exit While
                                        End If

                                        save.Write(bss, 0, readSize)
                                    End While
                                End If
                            Else
                                Dim bss(filesize - 1) As Byte
                                fss.Read(bss, 0, bss.Length)
                                save.Write(bss, 0, bss.Length)
                            End If
                        Else
                            Dim count As Integer = (filesize >> 11) + 1
                            If (filesize And &H7FF) = 0 AndAlso filesize > 0 Then
                                count -= 1
                            End If
                            Dim binn(2047) As Byte
                            For j = 1 To count
                                binn = unpack_cso(lba)
                                If (lba + 1) << 11 < iso_len Then
                                    lba += 1
                                End If
                                If j = count Then
                                    save.Write(binn, 0, filesize - (j << 11) + 2048)
                                    Exit For
                                End If
                                save.Write(binn, 0, 2048)
                            Next
                        End If
                        'Dim bss(filesize - 1) As Byte
                        'Dim count As Integer = (filesize >> 11) + 1
                        'Dim binn(2047) As Byte
                        'Array.Resize(bss, count << 11)
                        'For k = 0 To count - 1
                        '    binn = unpack_cso(lba)
                        '    If (lba + 1) << 11 < iso_len Then
                        '        lba += 1
                        '    End If
                        '    Array.Copy(binn, 0, bss, k << 11, 2048)
                        'Next
                        'Array.Resize(bss, filesize)
                        'save.Write(bss, 0, bss.Length)
                        'End If
                        fss.Close()
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

    Function getlba(ByVal tt As TreeNode, ByVal sender As Integer) As String
        Dim dirpath As String = tt.FullPath & "\"
        If sender = 1 Then
            dirpath = dirpath.Replace(TreeView1.SelectedNode.FullPath & "\", "")
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
                    sb.Append(dirpath)
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


