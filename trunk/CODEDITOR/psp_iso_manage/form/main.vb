Imports System.Security.Cryptography
Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Media
Imports System.Drawing

Public Class umdisomanger
    Friend psx As Boolean = False

    Private Sub load_list(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If File.Exists(Application.StartupPath & "\path.txt") = True Then
            Dim lw As New System.IO.StreamReader(Application.StartupPath & "\path.txt", System.Text.Encoding.GetEncoding(932))
            Dim isoname As TreeNode = Nothing
            Dim isoinfo As TreeNode = Nothing
            Dim line As String = ""
            Dim mask As String = ""
            Dim add As Boolean = False
            While lw.Peek() > -1
                line = lw.ReadLine
                If line.Contains("_S ") Then
                    isoname = New TreeNode(Path.GetFileNameWithoutExtension(line))
                    With isoname
                        .Name = ""
                        .Tag = line.Remove(0, 3)
                    End With
                ElseIf line.Contains("_G ") Then
                    isoname.Text = line.Remove(0, 3)
                    TreeView1.Nodes.Add(isoname)
                ElseIf line.Contains("_P ") Then
                    isoinfo = New TreeNode(isoname.Tag.ToString) 'text
                    isoinfo.Tag = line.Remove(0, 3)
                ElseIf line.Contains("_I ") Then
                    isoinfo.Name = line.Remove(0, 3)
                    isoname.Nodes.Add(isoinfo)
                ElseIf line.Contains("_H ") Then
                    line = line.Remove(0, 3)
                    mask = "CRC32(:|;)\x20?[0-9A-F]{8}"
                    Dim crc As New Regex(mask, RegexOptions.ECMAScript)
                    Dim m As Match = crc.Match(line)
                    If m.Success Then
                        isoinfo = New TreeNode(m.Value)
                        isoname.Nodes.Add(isoinfo)
                    End If
                    mask = "MD5(:|;)\x20?[0-9A-F]{32}"
                    Dim md As New Regex(mask, RegexOptions.ECMAScript)
                    Dim md5 As Match = md.Match(line)
                    If md5.Success Then
                        isoinfo = New TreeNode(md5.Value)
                        isoname.Nodes.Add(isoinfo)
                    End If
                    mask = "SHA\-1(:|;)\x20?[0-9A-F]+"
                    Dim sh As New Regex(mask, RegexOptions.ECMAScript)
                    Dim sha As Match = sh.Match(line)
                    If sha.Success Then
                        isoinfo = New TreeNode(sha.Value)
                        isoname.Nodes.Add(isoinfo)
                    End If
                End If
            End While

            lw.Close()
        End If

        If Directory.Exists(Application.StartupPath & "\imgs\user") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\imgs\user")
        End If

        If My.Settings.topmost = True Then
            Me.TopMost = True
            GUITOP.Checked = True
        End If

        PictureBox1.AllowDrop = True
        PictureBox2.AllowDrop = True
        AddHandler TreeView1.ItemDrag, AddressOf TreeView1_ItemDrag
        AddHandler TreeView1.DragOver, AddressOf TreeView1_DragOver
        AddHandler TreeView1.DragDrop, AddressOf TreeView1_DragDrop

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CRCimage.Click
        Try
            If File.Exists(Application.StartupPath & "\datas\ADVANsCEne_PSP.xml") = True Then
                Dim xml As String = Application.StartupPath & "\datas\ADVANsCEne_PSP.xml"
                Dim img As String = Application.StartupPath & "\imgs\ADVANsCEne Sony PSP Collection\"
                Dim treenode As TreeNode = TreeView1.SelectedNode
                If treenode IsNot Nothing Then
                    If treenode.Level = 0 Then
                        treenode = treenode.Nodes(0)
                    Else
                        treenode = treenode.Parent.Nodes(0)
                    End If
                    Dim path As String = treenode.Tag.ToString

                    Dim hash As String = crc.Text

                    If crc.Text = "" Then

                        If File.Exists(path) = True Then
                            Dim crc32 As New CRC32()

                            Using fs As FileStream = File.Open(path, FileMode.Open)
                                For Each b As Byte In crc32.ComputeHash(fs)
                                    hash += b.ToString("x2").ToLower()
                                Next
                            End Using

                            hash = hash.ToUpper
                        Else
                            Exit Sub
                        End If
                    End If

                    Dim sr As New System.IO.StreamReader(xml, System.Text.Encoding.GetEncoding(65001))
                    Dim s As String = sr.ReadToEnd()
                    sr.Close()
                    Dim mask As String = "<romCRC extension="".iso"">" & hash & "</romCRC>"
                    Dim r As New Regex(mask, RegexOptions.ECMAScript)
                    Dim m As Match = r.Match(s)
                    If m.Success Then
                        s = s.Remove(m.Index + m.Length, s.Length - (m.Index + m.Length))
                        s = s.Remove(0, s.LastIndexOf("<game>"))
                        mask = "<imageNumber>\d+</imageNumber>"
                        Dim imgnum As New Regex(mask, RegexOptions.ECMAScript)
                        Dim z As Match = imgnum.Match(s)
                        s = z.Value.Replace("<imageNumber>", "")
                        s = s.Replace("</imageNumber>", "")
                        Dim num As Integer = CInt(s)
                        Dim st, en As Integer
                        st = 500 * (num \ 500) + 1
                        en = 500 * (num \ 500 + 1)
                        img &= st.ToString & "-" & en.ToString

                        bitmap_resize(PictureBox1, img & "\" & num.ToString & "a.png", 104, 181)
                        bitmap_resize(PictureBox2, img & "\" & num.ToString & "b.png", 382, 181)
                        treenode.Name = img & "\" & num.ToString & ".png"

                        If crc.Text = "" Then

                            Dim k As Integer = treenode.Parent.Nodes.Count
                            For Each n As TreeNode In treenode.Parent.Nodes
                                If n.Text.Contains("CRC32") Then
                                    n.Text = "CRC32: " & hash
                                    Exit For
                                ElseIf k = n.Index + 1 Then
                                    Dim isoinfo As New TreeNode
                                    isoinfo.Text = "CRC32: " & hash
                                    treenode.Parent.Nodes.Add(isoinfo)
                                End If
                            Next
                            crc.Text = hash
                        End If

                        Beep()
                    Else
                        Beep()
                        MessageBox.Show("ISOと同じCRC32;" & hash & "がみつかりませんでした", "CRC不一致")
                    End If

                End If
            Else
                MessageBox.Show("画像検索用のoffline用XMLがみつかりません", "XMLエラー")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub TreeView1_drop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragEnter, PictureBox1.DragEnter, PictureBox2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub


    Private Sub ListBox1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragDrop
        Try
            Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            Dim isoname As TreeNode
            Dim isoinfo As TreeNode
            Dim psf As New psf
            Dim add As Boolean = True
            Dim beeps As Boolean = False
            Dim sb As New StringBuilder

            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                For i = 0 To fileName.Length - 1

                    If System.IO.Directory.Exists(fileName(i)) = True Or psf.GETID(fileName(i)) = "" Then
                    Else
                        isoname = New TreeNode(psf.GETNAME(fileName(i)))
                        With isoname
                            .Name = Path.GetFileNameWithoutExtension(fileName(i))
                            .Tag = psf.GETID(fileName(i))
                        End With

                        For k = 0 To TreeView1.Nodes.Count - 1
                            If isoname.Tag.ToString <> TreeView1.Nodes(k).Tag.ToString Then
                                add = True
                            Else
                                add = False
                                beeps = True
                                sb.Append(psf.GETID(fileName(i)))
                                sb.Append(",")
                                sb.Append(psf.GETNAME(fileName(i)))
                                sb.Append(",")
                                sb.AppendLine(fileName(i))
                                Beep()
                                Exit For
                            End If
                        Next
                        If add = True Then
                            TreeView1.Nodes.Add(isoname)
                            isoinfo = New TreeNode(psf.GETID(fileName(i)))
                            With isoinfo
                                .Name = Path.GetFileNameWithoutExtension(fileName(i)) 'image
                                .Tag = fileName(i)
                            End With
                            isoname.Nodes.Add(isoinfo)
                        End If

                    End If
                Next
                If beeps = True Then
                    sb.Insert(0, "同じゲームがすで登録されてます" & vbCrLf)
                    MessageBox.Show(sb.ToString, "ゲームID重複")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        Try
            If File.Exists(Application.StartupPath & "\imgs\ADVANsCEne Sony PSP Collection\\1-500\1a.png") Then

                Dim treenode As TreeNode = TreeView1.SelectedNode

                If treenode IsNot Nothing Then
                    Dim isopath As String = ""
                    If treenode.Level = 0 Then
                        treenode = treenode.Nodes(0)
                    Else
                        treenode = treenode.Parent.Nodes(0)
                    End If

                    Dim userpic As String = Application.StartupPath & "\imgs\user\" & treenode.Parent.Tag.ToString & "a.png"
                    Dim userpic2 As String = Application.StartupPath & "\imgs\user\" & treenode.Parent.Tag.ToString & "b.png"
                    Dim p1 As Boolean = False
                    Dim p2 As Boolean = False
                    Dim psf As New psf

                    managename.Text = treenode.Parent.Text
                    gid.Text = treenode.Parent.Tag.ToString
                    isopath = treenode.Tag.ToString
                    crc.Text = ""
                    md5hash.Text = ""
                    sha.Text = ""

                    For Each n As TreeNode In treenode.Parent.Nodes
                        If n.Text.Contains("CRC32") Then
                            crc.Text = n.Text.Remove(0, 6).Trim
                        ElseIf n.Text.Contains("MD5") Then
                            md5hash.Text = n.Text.Remove(0, 4).Trim
                        ElseIf n.Text.Contains("SHA-1") Then
                            sha.Text = n.Text.Remove(0, 6).Trim
                        End If
                    Next

                    If File.Exists(isopath) = True Then
                        Dim fs As New System.IO.FileStream(isopath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                        Dim size(3) As Byte
                        isosize.Text = fs.Length.ToString
                        fs.Seek(&H8050, SeekOrigin.Begin)
                        fs.Read(size, 0, 4)
                        If psf.video(isopath) = "PBP" Then
                            isolba.Text = "PBPファイルです"
                        Else
                            isolba.Text = (BitConverter.ToInt32(size, 0) << 11).ToString
                        End If
                        fs.Close()
                    End If

                    If File.Exists(userpic) Then
                        bitmap_resize(PictureBox1, userpic, 104, 181)
                        p1 = True
                    End If
                    If File.Exists(userpic2) Then
                        bitmap_resize(PictureBox2, userpic2, 381, 181)
                        p2 = True
                    End If

                    Dim p As String = treenode.Name
                    If p.Length > 4 Then
                        Dim path As String = p.Insert(p.Length - 4, "a")
                        Dim path2 As String = p.Insert(p.Length - 4, "b")
                        If File.Exists(path) AndAlso p1 = False Then
                            bitmap_resize(PictureBox1, path, 104, 181)
                            p1 = True
                        End If
                        If File.Exists(path2) AndAlso p2 = False Then
                            bitmap_resize(PictureBox2, path2, 381, 181)
                            p2 = True
                        End If
                    End If

                    'なにもないときはりっじダミー表示
                    If p1 = False AndAlso p2 = False Then
                        PictureBox1.Image = System.Drawing.Image.FromFile(Application.StartupPath & "\imgs\ADVANsCEne Sony PSP Collection\1-500\1a.png")
                        PictureBox2.Image = System.Drawing.Image.FromFile(Application.StartupPath & "\imgs\ADVANsCEne Sony PSP Collection\1-500\1b.png")
                    End If
                End If
            Else
                MessageBox.Show("OFFLINELISTのリッジレーサー(ダミー用)が見つかりません", "画像エラー")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub


    Private Sub codetree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TreeView1.KeyUp
        If e.KeyCode = Keys.Delete Then
            Try
                If TreeView1.SelectedNode.Level = 0 Then
                    If MessageBox.Show("選択している情報を削除しますか？", "削除の確認", _
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        TreeView1.SelectedNode.Remove()
                    End If
                End If
                If TreeView1.SelectedNode.Level = 1 Then
                    If MessageBox.Show("選択している情報を削除しますか？", "削除の確認", _
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        TreeView1.SelectedNode.Parent.Remove()
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "例外")
            End Try

        End If
    End Sub


    Private Sub 削除ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 削除ToolStripMenuItem.Click

        Try
            If TreeView1.SelectedNode.Level = 0 Then
                If MessageBox.Show("選択しているゲームを削除しますか？", "削除の確認", _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    TreeView1.SelectedNode.Remove()
                End If
            End If
            If TreeView1.SelectedNode.Level = 1 Then
                If MessageBox.Show("選択しているゲームを削除しますか？", "削除の確認", _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    TreeView1.SelectedNode.Parent.Remove()
                End If
            End If
            

        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub



    'BeforeLabelEditイベントハンドラ
    'ツリーノードのラベルの編集が開始された時
    Private Sub TreeView1_BeforeLabelEdit(ByVal sender As Object, _
                                          ByVal e As NodeLabelEditEventArgs) Handles TreeView1.BeforeLabelEdit
        If e.Node.Level = 1 Then
            e.CancelEdit = True
        End If

    End Sub

    'AfterLabelEditイベントハンドラ
    'ツリーノードのラベルの編集された時
    Private Sub TreeView1_AfterLabelEdit(ByVal sender As Object, _
                                         ByVal e As NodeLabelEditEventArgs) Handles TreeView1.AfterLabelEdit
        'ラベルが変更されたか調べる
        'e.LabelがNothingならば、変更されていない
        If (e.Label = "") Then
            e.CancelEdit = True
        ElseIf (e.Label.Trim = "") Then
            e.CancelEdit = True
        End If
    End Sub

    'ノードがドラッグされた時
    Private Sub TreeView1_ItemDrag(ByVal sender As Object, _
            ByVal e As ItemDragEventArgs)
        Dim tv As TreeView = CType(sender, TreeView)
        tv.SelectedNode = CType(e.Item, TreeNode)
        tv.Focus()

        If tv.SelectedNode.Level = 0 Then
            'ノードのドラッグを開始する
            Dim dde As DragDropEffects = _
                tv.DoDragDrop(e.Item, DragDropEffects.All)

            '移動した時は、ドラッグしたノードを削除する
            If (dde And DragDropEffects.Move) = DragDropEffects.Move Then
                tv.Nodes.Remove(CType(e.Item, TreeNode))
            End If

        End If
    End Sub

    'ドラッグしている時
    Private Sub TreeView1_DragOver(ByVal sender As Object, _
            ByVal e As DragEventArgs)
        'ドラッグされているデータがTreeNodeか調べる
        If e.Data.GetDataPresent(GetType(TreeNode)) Then
            ' If (e.KeyState And 8) = 8 And (e.AllowedEffect And DragDropEffects.Copy) = DragDropEffects.Copy Then
            'Ctrlキーが押されていればCopy
            '"8"はCtrlキーを表す
            'e.Effect = DragDropEffects.Copy
            If (e.AllowedEffect And DragDropEffects.Move) = _
                DragDropEffects.Move Then
                '何も押されていなければMove
                e.Effect = DragDropEffects.Move
            Else
                e.Effect = DragDropEffects.None
            End If
        ElseIf e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'TreeNodeでなければ受け入れない
            e.Effect = DragDropEffects.None
        End If

        'マウス下のNodeを選択する
        If e.Effect <> DragDropEffects.None AndAlso e.Data.GetDataPresent(DataFormats.FileDrop) = False Then
            Dim tv As TreeView = CType(sender, TreeView)
            'マウスのあるNodeを取得する
            Dim target As TreeNode = _
                tv.GetNodeAt(tv.PointToClient(New Point(e.X, e.Y)))
            'ドラッグされているNodeを取得する
            Dim [source] As TreeNode = _
                CType(e.Data.GetData(GetType(TreeNode)), TreeNode)
            'マウス下のNodeがドロップ先として適切か調べる
            If Not target Is Nothing AndAlso _
                Not target Is [source] AndAlso _
                Not IsChildNode([source], target) Then
                'Nodeを選択する
                If target.IsSelected = False Then
                    tv.SelectedNode = target
                End If
            Else
                e.Effect = DragDropEffects.None
            End If
        End If
    End Sub

    'ドロップされたとき
    Private Sub TreeView1_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs)
        If e.Data.GetDataPresent(GetType(TreeNode)) Then
            Dim tv As TreeView = CType(sender, TreeView)
            'ドロップされたデータ(TreeNode)を取得
            Dim [source] As TreeNode = _
                CType(e.Data.GetData(GetType(TreeNode)), TreeNode)
            'ドロップ先のTreeNodeを取得する
            Dim target As TreeNode = _
                tv.GetNodeAt(tv.PointToClient(New Point(e.X, e.Y)))
            'マウス下のNodeがドロップ先として適切か調べる
            If Not target Is Nothing AndAlso _
                target.Level = [source].Level AndAlso _
                Not target Is [source] AndAlso _
                Not IsChildNode([source], target) Then
                'ドロップされたNodeのコピーを作成
                Dim cln As TreeNode = CType([source].Clone(), TreeNode)
                'Nodeを追加
                If target.Index < [source].Index Then
                    TreeView1.Nodes.Insert(target.Index, cln)
                Else
                    TreeView1.Nodes.Insert(target.Index + 1, cln)
                End If
                If e.Effect = DragDropEffects.Move Then
                    [source].Remove()
                End If
                '追加されたNodeを選択
                tv.SelectedNode = cln
            Else
                e.Effect = DragDropEffects.None
            End If
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Shared Function IsChildNode(ByVal parentNode As TreeNode, ByVal childNode As TreeNode) As Boolean
        If parentNode.Level = 0 AndAlso childNode.Level = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SAVE.Click, SAVELS.Click
        Try
            Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\path.txt", False, System.Text.Encoding.GetEncoding(932))
            Dim sb As New StringBuilder
            Dim s As String = ""

            For Each n As TreeNode In TreeView1.Nodes
                sb.Append("_S ")
                sb.AppendLine(n.Tag.ToString)
                sb.Append("_G ")
                sb.AppendLine(n.Text)
                For Each m As TreeNode In n.Nodes
                    If m.Tag IsNot Nothing Then
                        sb.Append("_P ")
                        sb.AppendLine(m.Tag.ToString)
                        sb.Append("_I ")
                        sb.AppendLine(m.Name.ToString)
                    Else
                        If m.Index = 1 Then
                            sb.Append("_H ")
                        End If
                        If m.Text.Contains("CRC") Then
                            sb.Append(m.Text.ToString)
                        End If
                        If m.Text.Contains("MD5") Then
                            sb.Append(m.Text.ToString)
                        End If
                        If m.Text.Contains("SHA-1") Then
                            sb.Append(m.Text.ToString)
                        End If
                        If n.Nodes.Count = m.Index + 1 Then
                            sb.Append(vbCrLf)
                        Else
                            sb.Append(",")
                        End If
                    End If
                Next
            Next
            sw.Write(sb.ToString)
            sw.Close()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub move_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles movepsp.Click
        Try
            Dim psp As String = findpsp()
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If
                Dim cp As String = treenode.Tag.ToString

                Dim cp2 As String = psp & "\ISO\" & Path.GetFileName(cp)
                Dim UMD As String = "UMDGAME"
                Dim dir As String = ""
                Dim files As String() = Nothing
                Dim psf As New psf
                If psf.video(cp) = "VIDEO" Then
                    cp2 = psp & "\ISO\VIDEO\" & Path.GetFileName(cp)
                    UMD = "UMDVIDEO"
                End If
                If psf.video(cp) = "PBP" Then
                    cp2 = psp & "\PSP\GAME\" & psf.GETID(cp) & "\EBOOT.PBP"
                    dir = psp & "\PSP\GAME\" & psf.GETID(cp)
                    If Directory.Exists(dir) = False Then
                        Directory.CreateDirectory(dir)
                    End If
                    files = System.IO.Directory.GetFiles( _
        Path.GetDirectoryName(cp), "*", System.IO.SearchOption.AllDirectories)

                    UMD = "PBP"
                End If

                If psp <> "" AndAlso File.Exists(cp2) = False Then
                    If UMD = "PBP" Then
                        For Each s In files
                            If File.Exists(dir & s) = False Then
                                File.Copy(s, dir & s.Replace(Path.GetDirectoryName(cp), ""))
                            End If
                        Next
                    Else
                        File.Copy(cp, cp2)
                    End If
                    Beep()
                    MessageBox.Show(UMD & "の転送が完了しました", "転送成功")
                ElseIf psp = "" Then
                    Beep()
                    MessageBox.Show("PSPが見つかりません,USB接続していないかメモステフォーマット時に作成されるMEMSTICK.INDがないようです", "PSP接続エラー")
                Else
                    Beep()
                    MessageBox.Show("すでにPSPに転送されてます", "ファイル重複")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "エラー")
        End Try
    End Sub


    Private Sub del_psp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles del_psp.Click

        Try
            Dim psp As String = findpsp()

            If psp <> "" Then
                Dim treenode As TreeNode = TreeView1.SelectedNode
                If treenode IsNot Nothing Then
                    If treenode.Level = 0 Then
                        treenode = treenode.Nodes(0)
                    Else
                        treenode = treenode.Parent.Nodes(0)
                    End If
                    Dim cp As String = treenode.Tag.ToString
                    Dim cp2 As String = psp & "\ISO\" & Path.GetFileName(cp)
                    Dim UMD As String = "UMDGAME"
                    Dim files As String() = Nothing
                    Dim dir As String = ""
                    Dim psf As New psf
                    If psf.video(cp) = "VIDEO" Then
                        cp2 = psp & "\ISO\VIDEO\" & Path.GetFileName(cp)
                        UMD = "UMDVIDEO"
                    End If
                    If psf.video(cp) = "PBP" Then
                        cp2 = psp & "\PSP\GAME\" & psf.GETID(cp) & "\EBOOT.PBP"
                        dir = psp & "\PSP\GAME\" & psf.GETID(cp)
                        files = System.IO.Directory.GetFiles( _
            Path.GetDirectoryName(cp2), "*", System.IO.SearchOption.AllDirectories)
                        UMD = "PBP"
                    End If

                    If psp <> "" AndAlso File.Exists(cp2) = True AndAlso MessageBox.Show("PSPからファイルを削除してもよろしいですか？", "削除の確認", _
                               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        If UMD = "PBP" Then
                            For Each s In files
                                If File.Exists(s) Then
                                    File.Delete(s)
                                End If
                            Next
                            Directory.Delete(dir)
                        Else
                            File.Delete(cp2)
                        End If
                        Beep()
                        MessageBox.Show(UMD & "を削除しました" & vbCrLf & cp2, "削除")
                    End If
                End If
            Else
                Beep()
                MessageBox.Show("PSPが見つかりません,USB接続していないかメモステフォーマット時に作成されるMEMSTICK.INDがないようです", "PSP接続エラー")
            End If
                Catch ex As Exception
            MessageBox.Show(ex.Message, "エラー")
        End Try
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TreeView1.SelectedNode IsNot Nothing Then
            Process.Start("http://www.google.co.jp/search?tbm=isch&hl=ja&source=hp&biw=1525&bih=814&q=" & TreeView1.SelectedNode.Text)
        End If
    End Sub


    Function findpsp() As String
        Dim PSP As String = " :\PSP"
        Dim driveletter As Integer = &H44 'drivepath D～Z
        Dim i As Integer
        For i = 0 To 22
            PSP = PSP.Remove(0, 1)
            PSP = PSP.Insert(0, Chr(driveletter))
            driveletter += 1
            If lockdriveletter.Checked = True Then
                PSP = drivelettter.Text & "\PSP"
            Else
                drivelettter.Text = PSP.Substring(0, 2)
            End If
            If My.Computer.FileSystem.DirectoryExists(PSP) AndAlso File.Exists(PSP.Substring(0, 2) & "\MEMSTICK.IND") Then
                PSP = PSP.Substring(0, 2)
                My.Settings.drivepath = PSP
                Return PSP
            End If
        Next
        Return ""
    End Function

    Private Sub calc_crc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calc_crc.Click

        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            If treenode.Level = 0 Then
                treenode = treenode.Nodes(0)
            Else
                treenode = treenode.Parent.Nodes(0)
            End If

            Dim path As String = treenode.Tag.ToString

            If File.Exists(path) = True Then

                Try
                    Dim crc32 As New CRC32()
                    Dim hash As [String] = [String].Empty

                    Using fs As FileStream = File.Open(path, FileMode.Open)
                        For Each b As Byte In crc32.ComputeHash(fs)
                            hash += b.ToString("x2").ToLower()
                        Next
                    End Using

                    hash = hash.ToUpper
                    Dim z As Integer = treenode.Parent.Nodes.Count
                    For Each n As TreeNode In treenode.Parent.Nodes
                        If n.Text.Contains("CRC32") Then
                            n.Text = "CRC32: " & hash
                            Exit For
                        ElseIf z = n.Index + 1 Then
                            Dim isoinfo As New TreeNode
                            isoinfo.Text = "CRC32:" & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    crc.Text = hash
                    Beep()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "例外")
                End Try

            End If
        End If
    End Sub


    Private Sub calc_md5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calc_md5.Click
        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            If treenode.Level = 0 Then
                treenode = treenode.Nodes(0)
            Else
                treenode = treenode.Parent.Nodes(0)
            End If

            Dim path As String = treenode.Tag.ToString

            If File.Exists(path) = True Then

                Try
                    'ファイルを開く
                    Dim fs As New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)

                    'MD5CryptoServiceProviderオブジェクトを作成 
                    Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()

                    'ハッシュ値を計算する 
                    Dim bs As Byte() = md5.ComputeHash(fs)

                    'ファイルを閉じる 
                    fs.Close()

                    Dim hash As String = BitConverter.ToString(bs).ToUpper().Replace("-", "")

                    Dim z As Integer = treenode.Parent.Nodes.Count
                    For Each n As TreeNode In treenode.Parent.Nodes
                        If n.Text.Contains("MD5") Then
                            n.Text = "MD5: " & hash
                            Exit For
                        ElseIf z = n.Index + 1 Then
                            Dim isoinfo As New TreeNode
                            isoinfo.Text = "MD5:" & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    md5hash.Text = hash
                    Beep()


                Catch ex As Exception
                    MessageBox.Show(ex.Message, "例外")
                End Try
            End If
        End If
    End Sub

    Private Sub calc_sha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calc_sha.Click

        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            If treenode.Level = 0 Then
                treenode = treenode.Nodes(0)
            Else
                treenode = treenode.Parent.Nodes(0)
            End If

            Dim path As String = treenode.Tag.ToString

            If File.Exists(path) = True Then
                Try
                    'ファイルを開く
                    Dim fs As New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)

                    'MD5CryptoServiceProviderオブジェクトを作成 
                    Dim sha1 As New System.Security.Cryptography.SHA1CryptoServiceProvider()

                    'ハッシュ値を計算する 
                    Dim bs As Byte() = sha1.ComputeHash(fs)

                    'ファイルを閉じる 
                    fs.Close()

                    Dim hash As String = BitConverter.ToString(bs).ToUpper().Replace("-", "")

                    Dim z As Integer = treenode.Parent.Nodes.Count
                    For Each n As TreeNode In treenode.Parent.Nodes
                        If n.Text.Contains("SHA-1") Then
                            n.Text = "SHA-1: " & hash
                            Exit For
                        ElseIf z = n.Index + 1 Then
                            Dim isoinfo As New TreeNode
                            isoinfo.Text = "SHA-1:" & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    sha.Text = hash
                    Beep()

                Catch ex As Exception
                    MessageBox.Show(ex.Message, "例外")
                End Try
            End If
        End If
    End Sub


    Private Sub all_hash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles all_hash.Click
        Try
            calc_crc_Click(sender, e)
            calc_md5_Click(sender, e)
            calc_sha_Click(sender, e)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub GAMEID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GAMEID.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If
                Dim path As String = treenode.Tag.ToString

                If File.Exists(path) = True Then
                    Dim psf As psf = New psf
                    Dim id As String = psf.GETID(path)
                    If id <> "" Then
                        gid.Text = id
                        treenode.Parent.Tag = id
                        treenode.Text = id
                    End If

                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub PFS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PFS.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If
                Dim path As String = treenode.Tag.ToString

                If File.Exists(path) = True Then
                    Dim psf As psf = New psf
                    Dim id As String = psf.GETNAME(path)
                    If id <> "" Then
                        managename.Text = id
                        treenode.Parent.Text = id
                    End If

                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub


    Private Sub drivelettter_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles drivelettter.KeyPress
        Dim mask As New Regex("[^D-Zd-z\x08]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        e.KeyChar = Char.ToUpper(e.KeyChar)
        If m.Success Then
            e.Handled = True
        End If

    End Sub

    Private Sub hexmask(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles crc.KeyPress, md5hash.KeyPress, sha.KeyPress
        Dim mask As New Regex("[^0-9A-Fa-f\x08]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        e.KeyChar = Char.ToUpper(e.KeyChar)
        If m.Success Then
            e.Handled = True
        End If

    End Sub


    Private Sub idmask(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gid.KeyPress
        Dim mask As New Regex("[^0-9A-Za-z\x08\-]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(e.KeyChar)
        e.KeyChar = Char.ToUpper(e.KeyChar)
        If m.Success Then
            e.Handled = True
        End If

    End Sub


    Private Sub nokey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles isolba.KeyPress, isosize.KeyPress
        e.Handled = True
    End Sub
    Private Sub nonkey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles isolba.KeyDown, isosize.KeyDown
        e.Handled = True
    End Sub

    Private Sub drivelettter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drivelettter.TextChanged, drivelettter.KeyPress
        Dim mask As New Regex("[D-Zd-z]", RegexOptions.ECMAScript)
        Dim m As Match = mask.Match(drivelettter.Text)
        If m.Success Then
            drivelettter.Text = m.Value & ":"
        End If
    End Sub

    Private Sub version_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles version.Click
        Dim f As New ver
        f.ShowDialog()
        f.Dispose()
    End Sub

    Private Sub ADD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADD.Click
        Try
            Dim ofd As New OpenFileDialog()
            ofd.InitialDirectory = Application.StartupPath
            ofd.Filter = "ISO/PBPファイル(*iso;*.pbp)|*.iso;*.pbp"
            ofd.Title = "ISO/PBPファイルを選択してください"
            ofd.RestoreDirectory = True

            If ofd.ShowDialog() = DialogResult.OK Then
                Dim isoname As TreeNode
                Dim isoinfo As TreeNode
                Dim psf As New psf
                Dim add As Boolean = True
                Dim beeps As Boolean = False
                Dim sb As New StringBuilder


                If System.IO.Directory.Exists(ofd.FileName) = True Or psf.GETID(ofd.FileName) = "" Then
                Else
                    isoname = New TreeNode(psf.GETNAME(ofd.FileName))
                    With isoname
                        .Name = Path.GetFileNameWithoutExtension(ofd.FileName)
                        .Tag = psf.GETID(ofd.FileName)
                    End With

                    For k = 0 To TreeView1.Nodes.Count - 1
                        If isoname.Tag.ToString <> TreeView1.Nodes(k).Tag.ToString Then
                            add = True
                        Else
                            add = False
                            beeps = True
                            sb.Append(psf.GETID(ofd.FileName))
                            sb.Append(",")
                            sb.Append(psf.GETNAME(ofd.FileName))
                            sb.Append(",")
                            sb.AppendLine(ofd.FileName)
                            Beep()
                            Exit For
                        End If
                    Next
                    If add = True Then
                        TreeView1.Nodes.Add(isoname)
                        isoinfo = New TreeNode(psf.GETID(ofd.FileName))
                        With isoinfo
                            .Name = Path.GetFileNameWithoutExtension(ofd.FileName) 'image
                            .Tag = ofd.FileName
                        End With
                        isoname.Nodes.Add(isoinfo)
                    End If

                End If
                If beeps = True Then
                    sb.Insert(0, "同じゲームがすで登録されてます" & vbCrLf)
                    MessageBox.Show(sb.ToString, "ゲームID重複")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub



    Private Sub picture_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragDrop
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "a.png"
                If File.Exists(picture) = True Then
                    File.Delete(picture)
                End If
                File.Copy(fileName(0), picture)
                bitmap_resize(PictureBox1, picture, 104, 181)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub picture2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox2.DragDrop
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "b.png"
                If File.Exists(picture) = True Then
                    File.Delete(picture)
                End If
                File.Copy(fileName(0), picture)
                bitmap_resize(PictureBox2, picture, 381, 181)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim ofd As New OpenFileDialog()

                ofd.InitialDirectory = Application.StartupPath
                ofd.Filter = "bmp/png/jpgファイル(*bmp;*.png;*.jpg)|*bmp;*.png;*.jpg"
                ofd.Title = "BMP/PNG/JPGファイルを選択してください"
                ofd.RestoreDirectory = True
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "a.png"

                If ofd.ShowDialog() = DialogResult.OK Then
                    If File.Exists(picture) = True Then
                        File.Delete(picture)
                    End If
                    File.Copy(ofd.FileName, picture)
                    bitmap_resize(PictureBox1, picture, 104, 181)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If

                Dim ofd As New OpenFileDialog()
                ofd.InitialDirectory = Application.StartupPath
                ofd.Filter = "bmp/png/jpgファイル(*bmp;*.png;*.jpg)|*bmp;*.png;*.jpg"
                ofd.Title = "BMP/PNG/JPGファイルを選択してください"
                ofd.RestoreDirectory = True
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "b.png"

                If ofd.ShowDialog() = DialogResult.OK Then
                    If File.Exists(picture) = True Then
                        File.Delete(picture)
                    End If
                    File.Copy(ofd.FileName, picture)
                    bitmap_resize(PictureBox2, picture, 381, 181)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Function bitmap_resize(ByRef pc As PictureBox, ByVal path As String, ByVal width As Integer, ByVal height As Integer) As Boolean

        Dim image = New Bitmap(path)
        'PictureBox1のGraphicsオブジェクトの作成
        Dim g As Graphics = pc.CreateGraphics()
        '補間方法として最近傍補間を指定する
        g.InterpolationMode = _
            System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        '画像を縮小表示
        g.DrawImage(image, 0, 0, width, height)
        '補間方法として高品質双三次補間を指定する
        g.InterpolationMode = _
            System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

        'BitmapとGraphicsオブジェクトを破棄
        image.Dispose()
        g.Dispose()
        Return True
    End Function

    Private Sub TOP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GUITOP.Click
        If Me.TopMost = False Then
            Me.TopMost = True
            GUITOP.Checked = True
        Else
            Me.TopMost = False
            GUITOP.Checked = False
        End If
        My.Settings.topmost = Me.TopMost
    End Sub

End Class
