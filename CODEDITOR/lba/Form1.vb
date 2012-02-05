Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Form1
    Dim iso As String = "NULL"


    Private Sub ll(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        For Each cmd As String In My.Application.CommandLineArgs
            If cmd.Contains(".iso") Then
                iso = cmd
            End If
        Next
        Button1_Click(sender, e)
    End Sub


    Private Sub ListBox1_DragEnter(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub ListBox1_DragDrop(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.DragEventArgs) _
            Handles Me.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する
        Dim fileName As String() = CType( _
            e.Data.GetData(DataFormats.FileDrop, False), _
            String())
        iso = fileName(0)
        Button1_Click(sender, e)
    End Sub

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
            TreeView1.Nodes.Add("ISO[0,0]")
            TreeView1.Nodes(0).Name = "0"
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
                sb.Append("NUM:")
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
                x.Text = name & "[" & k & "," & parent(k) & "]"
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

    Function cvt16bit(ByVal b As Byte()) As Integer
        Return BitConverter.ToInt16(b, 0)
    End Function

    Function cvt32bit(ByVal b As Byte()) As Integer
        Return BitConverter.ToInt32(b, 0)
    End Function


    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        Dim name As String = String.Empty

        name = TextBox2.Text

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

    Private Sub TreeView1_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        getlist(CInt(TreeView1.SelectedNode.Tag))
    End Sub

    Function getlist(ByVal dst As Integer) As Boolean
        Try
            ListView1.Clear()
            ListView1.View = View.Details
            ListView1.HideSelection = True
            ListView1.AutoSize = True

            ListView1.Columns.Add("NAME", -1, HorizontalAlignment.Left)
            ListView1.Columns.Add("LBA", -1, HorizontalAlignment.Left)
            ListView1.Columns.Add("SIZE", -1, HorizontalAlignment.Left)
            ListView1.Columns.Add("DATE", -1, HorizontalAlignment.Left)

            Dim lba As Integer = 0
            Dim lba_base As Integer = dst << 11

            Dim fs As New FileStream(iso, FileMode.Open, FileAccess.Read)
            Dim next_len As Integer
            Dim filesize As Integer
            Dim str_len As Integer
            Dim name As String
            Dim i As Integer
            Dim bb(1) As Byte
            Dim bbbb(3) As Byte
            Dim yyyymmdd(6) As Byte
            Dim na As Byte() = Nothing
            Dim bs(2047) As Byte
            fs.Seek(lba_base, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)

            ListView1.BeginUpdate()
            Dim unix_back As New ListViewItem
            unix_back.Text = ".."
            If TreeView1.SelectedNode.Parent IsNot Nothing Then
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
                    itemx.SubItems.Add(lba.ToString)
                    itemx.SubItems.Add(filesize.ToString)
                    itemx.SubItems.Add(cvt_date(yyyymmdd))

                    ListView1.Items.Add(itemx)
                End If

                i += next_len

                If bs(i) = 0 Then
                    Exit While
                End If
            End While

            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

            ListView1.EndUpdate()

            ListView2.Items.Clear()
            Dim itemx2 As New ListViewItem
            Dim s As String = TreeView1.SelectedNode.FullPath
            'Dim ss As String() = s.Split("\"c)
            'For i = 0 To ss.Length - 1
            '    If i = 0 Then
            '        itemx2.Text = ss(0)
            '    Else
            '        itemx2.SubItems.Add(ss(i))
            '    End If
            'Next
            Dim rm As New Regex("\[\d+,\d+\]")
            Dim m As Match = rm.Match(s)
            While m.Success
                s = s.Replace(m.Value, "")
                m = m.NextMatch
            End While
            itemx2.Text = s
            ListView2.Items.Add(itemx2)

            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return True
    End Function

    Function cvt_date(ByVal ymd As Byte()) As String
        Dim sb As New StringBuilder
        sb.Append((ymd(0) + 1900))
        sb.Append("/")
        sb.Append((ymd(1).ToString.PadLeft(2, "0"c)))
        sb.Append("/")
        sb.Append((ymd(2).ToString).PadLeft(2, "0"c))
        sb.Append("/")
        sb.Append((ymd(3).ToString.PadLeft(2, "0"c)))
        sb.Append(" ")
        sb.Append((ymd(4).ToString.PadLeft(2, "0"c)))
        sb.Append(":")
        sb.Append((ymd(5).ToString.PadLeft(2, "0"c)))
        sb.Append(":")
        sb.Append((ymd(6).ToString.PadLeft(2, "0"c)))

        Return sb.ToString
    End Function

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
            'getlist(CInt(s))
        End If
    End Sub
End Class
