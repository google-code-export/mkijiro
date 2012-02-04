Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Form1
    Dim iso As String

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
            Dim parent_node As New TreeNode
            Dim name As String
            Dim i As Integer
            Dim k As Integer = 1
            Dim bb(1) As Byte
            Dim bbbb(3) As Byte
            Dim na As Byte() = Nothing
            Dim sb As New StringBuilder
            Dim bs(2047) As Byte
            fs.Seek(&H808C, SeekOrigin.Begin)
            fs.Read(bbbb, 0, 4)
            'リトルエンディアンパステーブル(L型)
            lba = cvt32bit(bbbb) << 11
            fs.Read(bbbb, 0, 4)
            'ビッグエンディアンパステーブル(M型)
            lba_m = cvt32bit(bbbb) << 11
            fs.Seek(lba, SeekOrigin.Begin)
            'LBA読み込みサイズを拡張
            Array.Resize(bs, lba_m - lba)
            fs.Read(bs, 0, bs.Length)
            TreeView1.Nodes.Add("ISO[0,]")
            TreeView1.Nodes(0).Name = "0"
            parent_node = TreeView1.Nodes(0)
            While i < bs.Length
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
                x.Tag = parent(k)
                x.Name = k
                '親ノードを検索し追加する
                Dim seek_parent_node As New TreeNode
                Dim arr As TreeNode() = TreeView1.Nodes.Find(parent(k), True)
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
        Else
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

End Class
