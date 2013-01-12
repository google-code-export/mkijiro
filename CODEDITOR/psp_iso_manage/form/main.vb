Imports System.Security.Cryptography
Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Media
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices


Module Module1

    <DllImport("kernel32.dll")> _
    Public Function GetShortPathName(ByVal longPath As String, ByVal shortPathBuffer As StringBuilder, ByVal bufferLength As Integer) As Integer
    End Function


End Module


Public Class umdisomanger
    Friend psx As Boolean = False
    Friend lang(50) As String
    Friend check As Boolean = False

#Region "form"
    Private Sub load_list(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim s As String = Application.StartupPath & "\path.txt"

            If File.Exists(s) = True Then
                Dim lw As New System.IO.StreamReader(s, System.Text.Encoding.GetEncoding(932))
                Dim isoname As TreeNode = Nothing
                Dim isoinfo As TreeNode = Nothing
                Dim line As String = ""
                Dim head As String = ""
                Dim mask As String = ""
                Dim add As Boolean = False
                Dim temp As String = ""
                Dim crc As New Regex("CRC32(:|;)\x20?[0-9A-F]{8}", RegexOptions.ECMAScript)
                Dim m As Match
                Dim md As New Regex("MD5(:|;)\x20?[0-9A-F]{32}", RegexOptions.ECMAScript)
                Dim md5 As Match
                Dim sh As New Regex("SHA\-1(:|;)\x20?[0-9A-F]{40}", RegexOptions.ECMAScript)
                Dim sha As Match
                While lw.Peek() > -1
                    line = lw.ReadLine
                    If line.Length > 3 Then
                        head = line.Substring(0, 3)
                        If head = "_S " Then
                            isoname = New TreeNode(Path.GetFileNameWithoutExtension(line))
                            With isoname
                                .Name = ""
                                .Tag = line.Remove(0, 3)
                            End With
                        ElseIf head = "_G " Then
                            isoname.Text = line.Remove(0, 3)
                            TreeView1.Nodes.Add(isoname)
                        ElseIf head = "_P " Then
                            isoinfo = New TreeNode(isoname.Tag.ToString) 'text
                            isoinfo.Tag = line.Remove(0, 3)
                        ElseIf head = "_I " Then
                            isoinfo.Name = line.Remove(0, 3)
                            isoname.Nodes.Add(isoinfo)
                        ElseIf head = "_H " Then
                            line = line.Remove(0, 3)
                            m = crc.Match(line)
                            If m.Success Then
                                temp = "CRC32; " & m.Value.Remove(0, m.Value.Length - 8)
                                isoinfo = New TreeNode(temp)
                                isoname.Nodes.Add(isoinfo)
                            End If
                            md5 = md.Match(line)
                            If md5.Success Then
                                temp = "MD5; " & md5.Value.Remove(0, md5.Value.Length - 32)
                                isoinfo = New TreeNode(temp)
                                isoname.Nodes.Add(isoinfo)
                            End If
                            sha = sh.Match(line)
                            If sha.Success Then
                                temp = "SHA-1; " & sha.Value.Remove(0, sha.Value.Length - 40)
                                isoinfo = New TreeNode(temp)
                                isoname.Nodes.Add(isoinfo)
                            End If
                        ElseIf line.Contains("#") Then
                            isoname.Name &= line.Remove(0, 1) & vbCrLf
                        End If
                    Else
                        If line.Contains("#") Then
                            isoname.Name &= line.Remove(0, 1) & vbCrLf
                        End If
                    End If
                End While

                lw.Close()
            End If

            If Directory.Exists(Application.StartupPath & "\imgs\user") = False Then
                Directory.CreateDirectory(Application.StartupPath & "\imgs\user")
            End If
            If Directory.Exists(Application.StartupPath & ("\diff\imgs")) = False Then
                Directory.CreateDirectory(Application.StartupPath & "\diff\imgs")
            End If

            If Directory.Exists(Application.StartupPath & ("\diff\datas")) = False Then

                Directory.CreateDirectory(Application.StartupPath & "\diff\datas")
            End If


            If My.Settings.pspinsdir = "" Then
                My.Settings.pspinsdir = Application.StartupPath & "\"
            End If

            pathcrc.Checked = True = My.Settings.filenamecrc
            lockdriveletter.Checked = My.Settings.lockdrive = True
            t_gid.Checked = My.Settings.pspgid = True
            disck_ver.Checked = My.Settings.diskver
            ROMCODEs.Checked = My.Settings.romcode
            Me.TopMost = My.Settings.topmost
            GUITOP.Checked = My.Settings.topmost
            ALSAVE.Checked = My.Settings.alwayssave
            USEMD5.Checked = My.Settings.usemd5

            If (1 And My.Settings.rename) = 1 Then
                FILEPATH.Checked = True
            End If
            If (2 And My.Settings.rename) = 2 Then
                MNAME.Checked = True
            End If


            If File.Exists(My.Settings.datpath) = False Then
                My.Settings.datpath = Application.StartupPath & "\datas\nanamitan.dat"
            End If
            If File.Exists(My.Settings.xml) = False Then
                My.Settings.xml = Application.StartupPath & "\datas\nanamitan.xml"
            End If
            'System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

            If CultureInfo.CurrentCulture.ToString.Contains("ja") Then
                ADD.ToolTipText = My.Resources.fileadd
                SAVELS.ToolTipText = My.Resources.filesave
                editpspdir.ToolTipText = My.Resources.pspdir
                xmlselect.ToolTipText = My.Resources.xml
                EXPORTPSPINS.ToolTipText = My.Resources.po_gei
                rename_dat.ToolTipText = My.Resources.rm
                RMXML.ToolTipText = My.Resources.rmxml
                RMDAT.ToolTipText = My.Resources.rmdat
                If File.Exists(Application.StartupPath & "\ja\ja.txt") = True Then
                    Dim lw As New System.IO.StreamReader(Application.StartupPath & "\ja\ja.txt", System.Text.Encoding.GetEncoding(932))
                    Dim i As Integer
                    While lw.Peek() > -1
                        lang(i) = lw.ReadLine
                        i += 1
                    End While
                    lw.Close()
                End If
            Else
                If File.Exists(Application.StartupPath & "\ja\en.txt") = True Then
                    Dim lw As New System.IO.StreamReader(Application.StartupPath & "\ja\en.txt", System.Text.Encoding.GetEncoding(0))
                    Dim i As Integer
                    While lw.Peek() > -1
                        lang(i) = lw.ReadLine
                        i += 1
                    End While
                    lw.Close()
                End If
            End If

            drivelettter.Text = My.Settings.drivepath
            My.Settings.edit = False

            PictureBox1.AllowDrop = True
            PictureBox2.AllowDrop = True
            AddHandler TreeView1.ItemDrag, AddressOf TreeView1_ItemDrag
            AddHandler TreeView1.DragOver, AddressOf TreeView1_DragOver
            AddHandler TreeView1.DragDrop, AddressOf TreeView1_DragDrop
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message)
        End Try

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If My.Settings.edit = True AndAlso My.Settings.alwayssave = False Then
            Dim dr As DialogResult
            dr = MessageBox.Show(Me, lang(49), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If dr = DialogResult.No Or dr = Windows.Forms.DialogResult.Cancel Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub SAVE(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If My.Settings.alwayssave = True Then
            SAVELIST(sender, e)
        End If
    End Sub
#End Region


#Region "tree"
    Private Sub TreeView1_drop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragEnter, PictureBox1.DragEnter, PictureBox2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub


    Private Sub tree_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragDrop
        Try
            Dim isoname As TreeNode
            Dim isoinfo As TreeNode
            Dim psf As New psf
            Dim add As Boolean = True
            Dim beeps As Boolean = False
            Dim sb As New StringBuilder
            Dim cr As New Regex("\[[0-9A-Fa-f]{8}\]", RegexOptions.ECMAScript)
            Dim crcs As Match

            If e.Data.GetDataPresent(DataFormats.FileDrop) Then

                Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
                If Directory.Exists(fileName(0)) Then
                    fileName = System.IO.Directory.GetFiles( _
                fileName(0), "*", System.IO.SearchOption.AllDirectories)
                End If
                For i = 0 To fileName.Length - 1

                    If System.IO.Directory.Exists(fileName(i)) = True Or psf.GETID(fileName(i)) = "" Then
                    Else
                        isoname = New TreeNode(psf.GETNAME(fileName(i), "N"))
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
                                sb.Append(psf.GETNAME(fileName(i), "N"))
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

                            crcs = cr.Match(Path.GetFileNameWithoutExtension(fileName(i)))
                            If crcs.Success Then
                                Dim crctree As New TreeNode
                                crctree.Text = ("CRC32; " & crcs.Value.Substring(1, 8))
                                isoname.Nodes.Add(crctree)
                            End If
                        End If

                    End If
                Next
                If beeps = True Then
                    '"同じゲームがすで登録されてます" "ゲームID重複"
                    sb.Insert(0, lang(8) & vbCrLf)
                    Me.TopMost = False
                    MessageBox.Show(Me, sb.ToString, lang(9))
                    If My.Settings.topmost = True Then
                        Me.TopMost = False
                    End If
                End If
                My.Settings.edit = True
            End If

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub node_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCheck
        ' The code only executes if the user caused the checked state to change.
        If e.Action = TreeViewAction.ByMouse Then
            If e.Node.Checked = True Then
                e.Node.Checked = False
            Else
                e.Node.Checked = True
            End If
        End If
    End Sub


    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        Try

            isolba.ForeColor = Color.Black
            Dim treenode As TreeNode = TreeView1.SelectedNode

            If treenode IsNot Nothing Then
                Dim isopath As String = ""
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If
                'Dim str As String = CType(Me.Owner, extra).Text
                'CType(Me.Owner, extra).Text = treenode.Parent.Tag.ToString
                'Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Parent.Tag.ToString
                'exim.bitmap_resize(exim.PictureBox1, picture & "boxart.png", 790, 630)
                'exim.bitmap_resize(exim.PictureBox2, picture & "umdfront.png", 280, 260)
                'exim.bitmap_resize(exim.PictureBox3, picture & "umdback.png", 280, 260)



                Dim userpic As String = Application.StartupPath & "\imgs\user\" & treenode.Parent.Tag.ToString & "a.png"
                Dim userpic2 As String = Application.StartupPath & "\imgs\user\" & treenode.Parent.Tag.ToString & "b.png"
                Dim p1 As Boolean = False
                Dim p2 As Boolean = False
                Dim psf As New psf
                Dim tempsize As Long = 0

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
                    Dim lba As Long
                    isosize.Text = fs.Length.ToString("0,0,0")
                    tempsize = fs.Length
                    fs.Seek(&H8050, SeekOrigin.Begin)
                    fs.Read(size, 0, 4)
                    fs.Close()
                    If psf.video(isopath) = "PBP" Then
                        '"PBPファイルです"
                        isolba.Text = lang(10)
                    ElseIf psf.video(isopath) = "CISO" Then
                        isolba.Text = psf.ciso_size(isopath).ToString
                        If psf.ciso_size(isopath) > 0 Then
                            isolba.ForeColor = Color.Red
                        ElseIf psf.ciso_size(isopath) < 0 Then
                            isolba.ForeColor = Color.Blue
                        End If
                    Else
                        lba = (BitConverter.ToInt32(size, 0) << 11)
                        isolba.Text = (tempsize - lba).ToString
                        If tempsize - lba > 0 Then
                            isolba.ForeColor = Color.Red
                        ElseIf tempsize - lba < 0 Then
                            isolba.ForeColor = Color.Blue
                        End If

                    End If
                End If

                If File.Exists(userpic) Then
                    bitmap_resize(PictureBox1, userpic, 104, 181)
                    p1 = True
                End If
                If File.Exists(userpic2) Then
                    bitmap_resize(PictureBox2, userpic2, 320, 181)
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
                        bitmap_resize(PictureBox2, path2, 320, 181)
                        p2 = True
                    End If
                End If

                If File.Exists(My.Settings.imgdir & "1-500\1a.png") = True Then
                    'なにもないときはダミー表示
                    If p1 = False Then
                        PictureBox1.Image = System.Drawing.Image.FromFile(Application.StartupPath & "\imgs\user\0a.png")
                    End If
                    If p2 = False Then
                        PictureBox2.Image = System.Drawing.Image.FromFile(Application.StartupPath & "\imgs\user\0b.png")
                    End If
                End If

                Dim psp As String = findpsp()
                If psp <> "" Then
                    Dim drive As New System.IO.DriveInfo(psp)
                    Dim ms As Single = drive.TotalFreeSpace
                    Dim ms2 As Long = drive.TotalFreeSpace
                    'ドライブの準備ができているか調べる
                    If drive.IsReady Then
                        If ms2 - tempsize > 0 Then
                            free.ForeColor = Color.Black
                        Else
                            free.ForeColor = Color.Red
                        End If

                        If ms > 1 << 30 Then
                            '"空き容量;不明"
                            free.Text = lang(11) & (ms / (1 << 30)).ToString("N") & "GiB"
                        ElseIf ms > 1 << 20 Then
                            free.Text = lang(11) & (ms / (1 << 20)).ToString("N") & "MiB"
                        ElseIf ms > 1 << 10 Then
                            free.Text = lang(11) & (ms / (1 << 10)).ToString("N") & "KiB"
                        End If
                    End If
                Else
                    free.Text = lang(12)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub


    Private Sub codetree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TreeView1.KeyUp
        If e.KeyCode = Keys.Delete Then
            treenode_delete(sender, e)
        End If
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
                My.Settings.edit = True
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

#End Region


#Region "ボタン"
    Private Sub crc_xml_Click(sender As System.Object, e As System.EventArgs) Handles crc_xml.Click

        Try
            If File.Exists(My.Settings.xml) = True Then
                My.Settings.edit = True
                Dim xml As String = My.Settings.xml
                Dim treenode As TreeNode = TreeView1.SelectedNode
                If treenode IsNot Nothing Then
                    If treenode.Level = 0 Then
                        treenode = treenode.Nodes(0)
                    Else
                        treenode = treenode.Parent.Nodes(0)
                    End If
                    Dim path As String = treenode.Tag.ToString

                    Dim hash As String = crc.Text.ToUpper

                    If crc.Text = "" Then

                        If File.Exists(path) = True Then
                            Dim crc32 As New CRC32()

                            Dim psf As New psf
                            Dim gid As String = My.Settings.unpackdir & treenode.Parent.Tag.ToString
                            Dim fss As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                            Dim bst(4) As Byte
                            If fss.Length > 4 Then
                                fss.Read(bst, 0, 4)
                                fss.Close()
                                If File.Exists(gid) = True Then
                                    path = gid
                                ElseIf bst(0) = &H43 AndAlso bst(1) = &H49 AndAlso bst(2) = &H53 AndAlso bst(3) = &H4F Then
                                    If MessageBox.Show(Me, "CSOなのでアンパックが必要です、時間がかかりますがよろしいですか？", "アンパック", _
                           MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                                        If psf.unpack_cso(path, gid) = True Then
                                            Beep()
                                            path = gid
                                        Else
                                            MessageBox.Show(Me, "アンパックに失敗しました")
                                            Exit Sub
                                        End If
                                    Else
                                        Exit Sub
                                    End If
                                End If
                            End If

                            Using fs As FileStream = File.OpenRead(path)
                                For Each b As Byte In crc32.ComputeHash(fs)
                                    hash += b.ToString("x2").ToLower()
                                Next
                            End Using

                            hash = hash.ToUpper
                            Beep()
                        Else
                            MessageBox.Show(Me, path & lang(0), lang(1))
                            Exit Sub
                        End If
                    End If

                    Dim ffs As New FileStream(xml, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    'ファイルを読み込むバイト型配列を作成する
                    Dim bs(CInt(ffs.Length - 1)) As Byte
                    'ファイルの内容をすべて読み込む
                    ffs.Read(bs, 0, bs.Length)
                    '閉じる
                    ffs.Close()
                    'ZIPHEAD
                    If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                        '展開するZIP書庫のパス
                        Dim zipPath As String = xml
                        'ZIP書庫を読み込む 
                        Dim zfs As New System.IO.FileStream( _
                            zipPath, _
                            System.IO.FileMode.Open, _
                            System.IO.FileAccess.Read)
                        'ZipInputStreamオブジェクトの作成 
                        Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                        'ZIP内のエントリを列挙 
                        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                        Dim xmll As Boolean = False
                        While True
                            'ZipEntryを取得
                            ze = zis.GetNextEntry()
                            If ze Is Nothing Then
                                Exit While
                            End If
                            If Not ze.IsDirectory Then
                                '展開先のファイル名を決定 
                                Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                                If ze.Name.Contains(".xml") = True Then
                                    '展開するファイルを読み込む
                                    Dim buffer As Byte() = New Byte(2047) {}
                                    Dim len As Integer = 0
                                    Dim i As Integer = 0
                                    While True
                                        len = zis.Read(buffer, 0, buffer.Length)
                                        If len = 0 Then
                                            Exit While
                                        End If
                                        'ファイルに書き込む
                                        Array.Resize(bs, i + buffer.Length)
                                        Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                        i += buffer.Length
                                    End While
                                    xmll = True
                                End If
                            End If
                        End While

                        '閉じる 
                        zis.Close()
                        zfs.Close()
                    End If
                    Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                    Dim hash2 As String = StrConv(hash, VbStrConv.Lowercase)

                    Dim mask As String = "<romCRC extension="".iso"">" & hash & "</romCRC>"
                    Dim mask2 As String = "<romCRC extension="".iso"">" & hash2 & "</romCRC>"
                    Dim r As New Regex(mask, RegexOptions.ECMAScript)
                    Dim m As Match = r.Match(s)
                    Dim q As New Regex(mask2, RegexOptions.ECMAScript)
                    Dim n As Match = q.Match(s)
                    If m.Success Or n.Success Then
                        If m.Success Then
                            s = s.Remove(m.Index + m.Length, s.Length - (m.Index + m.Length))
                        ElseIf n.Success Then
                            s = s.Remove(n.Index + n.Length, s.Length - (n.Index + n.Length))
                        End If
                        s = s.Remove(0, s.LastIndexOf("<game>"))
                        mask = "<title>.+</title>"
                        Dim imgnum As New Regex(mask, RegexOptions.ECMAScript)
                        Dim z As Match = imgnum.Match(s)
                        s = z.Value.Replace("<title>", "")
                        s = s.Replace("</title>", "")
                        If s <> "" Then
                            managename.Text = s
                            treenode.Parent.Text = s
                        End If

                        If crc.Text = "" Then

                            Dim k As Integer = treenode.Parent.Nodes.Count
                            For Each nn As TreeNode In treenode.Parent.Nodes
                                If nn.Text.Contains("CRC32") Then
                                    nn.Text = "CRC32: " & hash
                                    Exit For
                                ElseIf k = nn.Index + 1 Then
                                    Dim isoinfo As New TreeNode
                                    isoinfo.Text = "CRC32: " & hash
                                    treenode.Parent.Nodes.Add(isoinfo)
                                End If
                            Next
                            crc.Text = hash
                        End If
                    Else
                        Beep()
                        MessageBox.Show(Me, lang(2) & hash & lang(3), lang(4))
                    End If

                End If
            Else
                MessageBox.Show(Me, lang(5), lang(6))
            End If

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try


    End Sub

    Private Sub SAVELIST(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SAVELS.Click, SAVELS2.Click
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

                Dim ss As String() = n.Name.Split(CChar(vbLf))
                For Each str As String In ss
                    If str <> "" Then
                        sb.Append("#")
                        sb.AppendLine(str.Trim)
                    End If
                Next
            Next
            sw.Write(sb.ToString)
            sw.Close()
            My.Settings.edit = False


        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Function msdos83(ByVal s As String) As String

        'Dim ss As String() = s.Split(CChar("."))
        'If ss.Length >= 2 Then
        '    If ss(0).Length > 6 Then
        '        ss(0) = ss(0).Substring(0, 6)
        '    End If
        '    s = ss(0) & "~1." & ss(ss.Length - 1)

        'End If

        Dim bufferLength As Integer = 260
        Dim shortPathBuffer As New StringBuilder(bufferLength)
        Module1.GetShortPathName(s, shortPathBuffer, bufferLength)

        Dim shortPath As String = shortPathBuffer.ToString()

        Return shortPath
    End Function




    Private Sub VITAMSDOS83ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VITAMSDOS83ToolStripMenuItem.Click


        If Directory.Exists(My.Settings.vitasavedir) Then

            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If
                Dim cp As String = treenode.Tag.ToString

                If File.Exists(cp) = False Then
                    Beep()
                    MessageBox.Show(Me, "コピー元のファイルが見つかりません", "FILE EXIST")
                    Exit Sub

                End If

                Dim cp2 As String = My.Settings.vitasavedir & "\" & msdos83(Path.GetFileName(cp))

                If File.Exists(cp2) = False Then
                    My.Computer.FileSystem.CopyFile(cp, cp2, FileIO.UIOption.AllDialogs)
                    Beep()
                Else
                    Beep()
                    MessageBox.Show(Me, "すでに同じMSDOS8.3形式の名前が存在します", "FILE EXIST")
                End If

            End If
        Else
            Beep()
            MessageBox.Show(Me, "VITAのセーブフォルダが指定されてないか見つかりません", "NO VITA SAVEDATA")
        End If


    End Sub



    Private Sub move_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles movepsp.Click, cnt_install.Click
        Try
            Dim psp As String = findpsp()



            If psp = "" Then
                Beep()
                MessageBox.Show(Me, lang(18), lang(19))
            ElseIf free.ForeColor = Color.Red Then
                Beep()
                MessageBox.Show(Me, "空き容量が足りません", "MSFREE")
            Else

                Dim treenode As TreeNode = TreeView1.SelectedNode
                If treenode IsNot Nothing Then
                    If treenode.Level = 0 Then
                        treenode = treenode.Nodes(0)
                    Else
                        treenode = treenode.Parent.Nodes(0)
                    End If
                    Dim cp As String = treenode.Tag.ToString

                    Dim r As New Regex("X:\\.*?\n", RegexOptions.ECMAScript)
                    Dim m As Match = r.Match(treenode.Parent.Name)
                    Dim category As String = ""

                    Dim cp2 As String = psp & "\ISO\" & Path.GetFileName(cp)
                    If m.Success Then
                        category = m.Value.Remove(0, 2)
                        category = category.Remove(category.Length - 2, 2)
                        cp2 = psp & category & Path.GetFileName(cp)
                        If Directory.Exists(Path.GetDirectoryName(cp2)) = False Then
                            Directory.CreateDirectory(Path.GetDirectoryName(cp2))
                        End If
                    End If
                    If My.Settings.pspgid = True Then
                        cp2 = psp & "\ISO\" & treenode.Parent.Tag.ToString & ".ISO"
                        If m.Success Then
                            cp2 = psp & category & treenode.Parent.Tag.ToString & ".ISO"
                        End If

                    End If


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
                        If m.Success Then
                            cp2 = psp & category & psf.GETID(cp) & "\EBOOT.PBP"
                            dir = psp & category & psf.GETID(cp)
                        End If
                        If treenode.Parent.Tag.ToString.Contains("UP") Then
                            cp2 = psp & "\PSP\GAME\UPDATE\" & "\EBOOT.PBP"
                            dir = psp & "\PSP\GAME\UPDATE\"
                        End If

                        If Directory.Exists(dir) = False Then
                            Directory.CreateDirectory(dir)
                        End If
                        files = System.IO.Directory.GetFiles( _
            Path.GetDirectoryName(cp), "*", System.IO.SearchOption.AllDirectories)
                        UMD = "PBP"
                    End If

                    Me.TopMost = False

                    If psp <> "" AndAlso File.Exists(cp2) = False AndAlso MessageBox.Show(Me, cp & lang(41), lang(42), _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        If UMD = "PBP" Then
                            For Each s In files
                                If File.Exists(dir & s) = False Then
                                    My.Computer.FileSystem.CopyFile( _
                                      s, dir & s.Replace(Path.GetDirectoryName(cp), ""), FileIO.UIOption.AllDialogs)
                                    'File.Copy(s, dir & s.Replace(Path.GetDirectoryName(cp), ""))
                                End If
                            Next

                        Else
                            My.Computer.FileSystem.CopyFile(cp, cp2, FileIO.UIOption.AllDialogs)
                        End If
                        Beep()
                        '"の転送が完了しました", "転送成功"
                        MessageBox.Show(Me, UMD & lang(16), lang(17))
                    ElseIf psp = "" Then
                        Beep()
                        '"PSPが見つかりません,USB接続していないかメモステフォーマット時に作成されるMEMSTICK.INDがないようです
                        '"PSP接続エラー
                        MessageBox.Show(Me, lang(18), lang(19))
                    ElseIf File.Exists(cp2) = True Then
                        Beep()
                        '"すでにPSPに転送されてます", "ファイル重複"
                        MessageBox.Show(Me, lang(20), lang(21))
                    End If
                End If

                If psp <> "" Then
                    Dim drive As New System.IO.DriveInfo(psp)
                    Dim ms As Single = drive.TotalFreeSpace
                    Dim ms2 As Long = drive.TotalFreeSpace
                    'ドライブの準備ができているか調べる
                    If drive.IsReady Then
                        If ms2 - Convert.ToInt64(isosize.Text.Replace(",", "")) > 0 Then
                            free.ForeColor = Color.Black
                        Else
                            free.ForeColor = Color.Red
                        End If

                        If ms > 1 << 30 Then
                            free.Text = lang(11) & (ms / (1 << 30)).ToString("N") & "GiB"
                        ElseIf ms > 1 << 20 Then
                            free.Text = lang(11) & (ms / (1 << 20)).ToString("N") & "MiB"
                        ElseIf ms > 1 << 10 Then
                            free.Text = lang(11) & (ms / (1 << 10)).ToString("N") & "KiB"
                        End If
                    End If
                Else
                    free.Text = lang(12)
                End If

                If My.Settings.topmost = True Then
                    Me.TopMost = True
                End If

            End If
        Catch ex As Exception
            Beep()
            MessageBox.Show(Me, ex.Message, lang(1))
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

                    Dim r As New Regex("X:\\.*?\n", RegexOptions.ECMAScript)
                    Dim m As Match = r.Match(treenode.Parent.Name)
                    Dim category As String = ""

                    Dim cp2 As String = psp & "\ISO\" & Path.GetFileName(cp)
                    If m.Success Then
                        category = m.Value.Remove(0, 2)
                        category = category.Remove(category.Length - 2, 2)
                        cp2 = psp & category & Path.GetFileName(cp)
                        If Directory.Exists(Path.GetDirectoryName(cp2)) = False Then
                            Directory.CreateDirectory(Path.GetDirectoryName(cp2))
                        End If
                    End If

                    If My.Settings.pspgid = True Then
                        cp2 = psp & "\ISO\" & treenode.Parent.Tag.ToString & ".ISO"
                    End If
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
                        If m.Success Then
                            cp2 = psp & category & psf.GETID(cp) & "\EBOOT.PBP"
                            dir = psp & category & psf.GETID(cp)
                        End If

                        If treenode.Parent.Tag.ToString.Contains("UP") Then
                            cp2 = psp & "\PSP\GAME\UPDATE\" & "\EBOOT.PBP"
                            dir = psp & "\PSP\GAME\UPDATE\"
                        End If
                        files = System.IO.Directory.GetFiles( _
            Path.GetDirectoryName(cp2), "*", System.IO.SearchOption.AllDirectories)
                        UMD = "PBP"
                    End If
                    '"PSPからファイルを削除してもよろしいですか？"
                    If psp <> "" AndAlso File.Exists(cp2) = True AndAlso MessageBox.Show(Me, lang(22), lang(14), _
                               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        If UMD = "PBP" Then
                            For Each s In files
                                If File.Exists(s) Then
                                    Dim cFileInfo As New System.IO.FileInfo(s)
                                    If (cFileInfo.Attributes And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
                                        cFileInfo.Attributes = System.IO.FileAttributes.Normal
                                    End If
                                    File.Delete(s)
                                End If
                            Next
                            Directory.Delete(dir)
                        Else
                            Dim cFileInfo As New System.IO.FileInfo(cp2)
                            If (cFileInfo.Attributes And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
                                cFileInfo.Attributes = System.IO.FileAttributes.Normal
                            End If
                            File.Delete(cp2)
                        End If
                        Beep()
                        '"を削除しました" & vbCrLf & cp2, "削除")
                        MessageBox.Show(Me, UMD & lang(23) & vbCrLf & cp2, lang(24))
                    End If
                End If

                If psp <> "" Then
                    Dim drive As New System.IO.DriveInfo(psp)
                    Dim ms As Single = drive.TotalFreeSpace
                    Dim ms2 As Long = drive.TotalFreeSpace
                    'ドライブの準備ができているか調べる
                    If drive.IsReady Then
                        If ms2 - Convert.ToInt64(isosize.Text.Replace(",", "")) > 0 Then
                            free.ForeColor = Color.Black
                        Else
                            free.ForeColor = Color.Red
                        End If

                        If ms > 1 << 30 Then
                            free.Text = lang(11) & (ms / (1 << 30)).ToString("N") & "GiB"
                        ElseIf ms > 1 << 20 Then
                            free.Text = lang(11) & (ms / (1 << 20)).ToString("N") & "MiB"
                        ElseIf ms > 1 << 10 Then
                            free.Text = lang(11) & (ms / (1 << 10)).ToString("N") & "KiB"
                        End If
                    End If
                Else
                    free.Text = lang(12)
                End If

            Else
                Beep()
                MessageBox.Show(Me, lang(18), lang(19))
            End If
        Catch ex As Exception
            Beep()
            MessageBox.Show(Me, ex.Message, lang(1))
        End Try
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles google.Click
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

        Dim t1 As Integer = System.Environment.TickCount


        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            My.Settings.edit = True
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

                    Dim psf As New psf
                    Dim gid As String = My.Settings.unpackdir & treenode.Parent.Tag.ToString
                    Dim fss As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Dim bst(4) As Byte
                    If fss.Length > 4 Then
                        fss.Read(bst, 0, 4)
                        fss.Close()
                        If File.Exists(gid) = True Then
                            path = gid
                        ElseIf bst(0) = &H43 AndAlso bst(1) = &H49 AndAlso bst(2) = &H53 AndAlso bst(3) = &H4F Then
                            If MessageBox.Show(Me, "CSOなのでアンパックが必要です、時間がかかりますがよろしいですか？", "アンパック", _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                                If psf.unpack_cso(path, gid) = True Then
                                    Beep()
                                    path = gid
                                Else
                                    MessageBox.Show(Me, "アンパックに失敗しました")
                                    Exit Sub
                                End If
                            Else
                                Exit Sub
                            End If
                        End If
                    End If

                    Using fs As FileStream = File.OpenRead(path)
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
                            isoinfo.Text = "CRC32: " & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    crc.Text = hash
                    Beep()
                    TreeView1.SelectedNode = treenode.Parent
                    TreeView1.Focus()

                Catch ex As Exception
                    MessageBox.Show(Me, ex.Message, lang(7))
                End Try
            Else
                MessageBox.Show(Me, path & lang(0), lang(1))
            End If
        End If

        Label8.Text = (System.Environment.TickCount - t1).ToString

    End Sub


    Private Sub calc_md5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calc_md5.Click
        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            My.Settings.edit = True
            If treenode.Level = 0 Then
                treenode = treenode.Nodes(0)
            Else
                treenode = treenode.Parent.Nodes(0)
            End If

            Dim path As String = treenode.Tag.ToString

            If File.Exists(path) = True Then

                Try

                    Dim psf As New psf
                    Dim gid As String = My.Settings.unpackdir & treenode.Parent.Tag.ToString
                    Dim fss As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Dim bst(4) As Byte
                    If fss.Length > 4 Then
                        fss.Read(bst, 0, 4)
                        fss.Close()
                        If File.Exists(gid) = True Then
                            path = gid
                        ElseIf bst(0) = &H43 AndAlso bst(1) = &H49 AndAlso bst(2) = &H53 AndAlso bst(3) = &H4F Then
                            If MessageBox.Show(Me, "CSOなのでアンパックが必要です、時間がかかりますがよろしいですか？", "アンパック", _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                                If psf.unpack_cso(path, gid) = True Then
                                    Beep()
                                    path = gid
                                Else
                                    MessageBox.Show(Me, "アンパックに失敗しました")
                                    Exit Sub
                                End If
                            Else
                                Exit Sub
                            End If
                        End If
                    End If

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
                            isoinfo.Text = "MD5: " & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    md5hash.Text = hash
                    Beep()
                    TreeView1.SelectedNode = treenode.Parent
                    TreeView1.Focus()


                Catch ex As Exception
                    MessageBox.Show(Me, ex.Message, lang(7))
                End Try
            Else
                MessageBox.Show(Me, path & lang(0), lang(1))
            End If
        End If
    End Sub

    Private Sub calc_sha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calc_sha.Click

        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            My.Settings.edit = True
            If treenode.Level = 0 Then
                treenode = treenode.Nodes(0)
            Else
                treenode = treenode.Parent.Nodes(0)
            End If

            Dim path As String = treenode.Tag.ToString

            If File.Exists(path) = True Then
                Try

                    Dim psf As New psf
                    Dim gid As String = My.Settings.unpackdir & treenode.Parent.Tag.ToString
                    Dim fss As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Dim bst(4) As Byte
                    If fss.Length > 4 Then
                        fss.Read(bst, 0, 4)
                        fss.Close()
                        If File.Exists(gid) = True Then
                            path = gid
                        ElseIf bst(0) = &H43 AndAlso bst(1) = &H49 AndAlso bst(2) = &H53 AndAlso bst(3) = &H4F Then
                            If MessageBox.Show(Me, "CSOなのでアンパックが必要です、時間がかかりますがよろしいですか？", "アンパック", _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                                If psf.unpack_cso(path, gid) = True Then
                                    Beep()
                                    path = gid
                                Else
                                    MessageBox.Show(Me, "アンパックに失敗しました")
                                    Exit Sub
                                End If
                            Else
                                Exit Sub
                            End If
                        End If
                    End If

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
                            isoinfo.Text = "SHA-1: " & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    sha.Text = hash
                    Beep()
                    TreeView1.SelectedNode = treenode.Parent
                    TreeView1.Focus()

                Catch ex As Exception
                    MessageBox.Show(Me, ex.Message, lang(7))
                End Try
            Else
                MessageBox.Show(Me, path & lang(0), lang(1))
            End If
        End If
    End Sub

    Private Sub all_hash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles all_hash.Click
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
                    calc_crc_Click(sender, e)
                    calc_md5_Click(sender, e)
                    calc_sha_Click(sender, e)
                Else
                    MessageBox.Show(Me, path & lang(0), lang(1))
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub GAMEID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GAMEID.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                My.Settings.edit = True
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

                Else
                    MessageBox.Show(Me, path & lang(0), lang(1))
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub PFS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PFS.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                My.Settings.edit = True
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If
                Dim path As String = treenode.Tag.ToString

                If File.Exists(path) = True Then
                    Dim psf As psf = New psf
                    Dim id As String = psf.GETNAME(path, "N")
                    If id <> "" Then
                        managename.Text = id
                        treenode.Parent.Text = id
                    End If

                Else
                    MessageBox.Show(Me, path & lang(0), lang(1))
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
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
            drivelettter.Text = m.Value.ToUpper & ":"
            My.Settings.drivepath = drivelettter.Text
        Else
            drivelettter.Text = "Z:"
            My.Settings.drivepath = drivelettter.Text
        End If
    End Sub

    Private Sub version_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles version.Click
        Dim f As New ver

        Me.TopMost = False
        f.ShowDialog()
        If My.Settings.topmost = True Then
            Me.TopMost = True
        End If
        f.Dispose()
    End Sub

    Private Sub ADD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADD.Click
        Try
            Dim ofd As New OpenFileDialog()
            ofd.InitialDirectory = My.Settings.isobase
            '"ISO/PBPファイル(*iso;*.pbp)|*.iso;*.pbp"
            '"ISO/PBPファイルを選択してください"
            ofd.Filter = lang(25)
            ofd.Title = lang(26)
            ofd.Multiselect = True
            ofd.RestoreDirectory = True

            If ofd.ShowDialog() = DialogResult.OK Then
                My.Settings.edit = True
                Dim isoname As TreeNode
                Dim isoinfo As TreeNode
                Dim crctree As New TreeNode
                Dim psf As New psf
                Dim add As Boolean = True
                Dim beeps As Boolean = False
                Dim sb As New StringBuilder
                Dim cr As New Regex("\[[0-9A-Fa-f]{8}\]", RegexOptions.ECMAScript)
                Dim crcs As Match

                Dim paths As String() = ofd.FileNames

                For Each s As String In paths
                    If System.IO.Directory.Exists(s) = True Or psf.GETID(s) = "" Then
                    Else
                        isoname = New TreeNode(psf.GETNAME(s, "N"))
                        With isoname
                            .Name = Path.GetFileNameWithoutExtension(s)
                            .Tag = psf.GETID(s)
                        End With

                        For k = 0 To TreeView1.Nodes.Count - 1
                            If isoname.Tag.ToString <> TreeView1.Nodes(k).Tag.ToString Then
                                add = True
                            Else
                                add = False
                                beeps = True
                                sb.Append(psf.GETID(s))
                                sb.Append(",")
                                sb.Append(psf.GETNAME(s, "N"))
                                sb.Append(",")
                                sb.AppendLine(s)
                                Beep()
                                Exit For
                            End If
                        Next
                        If add = True Then
                            TreeView1.Nodes.Add(isoname)
                            isoinfo = New TreeNode(psf.GETID(s))
                            With isoinfo
                                .Name = Path.GetFileNameWithoutExtension(s) 'image
                                .Tag = ofd.FileName
                            End With
                            isoname.Nodes.Add(isoinfo)
                            crcs = cr.Match(Path.GetFileNameWithoutExtension(s))
                            If crcs.Success Then
                                crctree = New TreeNode("CRC32; " & crcs.Value.Substring(1, 8))
                                isoname.Nodes.Add(crctree)
                            End If
                        End If

                    End If
                Next
                If beeps = True Then
                    sb.Insert(0, lang(8) & vbCrLf)
                    MessageBox.Show(Me, sb.ToString, lang(9))
                End If
                My.Settings.isobase = Path.GetDirectoryName(paths(0))
            End If

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles tree_apply.Click

        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                My.Settings.edit = True
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                treenode.Tag = gid.Text
                treenode.Text = managename.Text
                treenode.Nodes(0).Text = gid.Text
                Dim z As Integer = treenode.Nodes.Count
                If crc.TextLength = 8 Then
                    For Each n As TreeNode In treenode.Nodes
                        If n.Text.Contains("CRC32") Then
                            n.Text = "CRC32: " & crc.Text
                            Exit For
                        ElseIf z = n.Index + 1 Then
                            Dim isoinfo As New TreeNode
                            isoinfo.Text = "CRC32:" & crc.Text
                            treenode.Nodes.Add(isoinfo)
                        End If
                    Next
                End If
                If md5hash.TextLength = 32 Then
                    For Each n As TreeNode In treenode.Nodes
                        If n.Text.Contains("MD5") Then
                            n.Text = "MD5: " & md5hash.Text
                            Exit For
                        ElseIf z = n.Index + 1 Then
                            Dim isoinfo As New TreeNode
                            isoinfo.Text = "MD5:" & md5hash.Text
                            treenode.Nodes.Add(isoinfo)
                        End If
                    Next
                End If
                If sha.TextLength = 41 Then
                    For Each n As TreeNode In treenode.Nodes
                        If n.Text.Contains("SHA") Then
                            n.Text = "SHA-1: " & sha.Text
                            Exit For
                        ElseIf z = n.Index + 1 Then
                            Dim isoinfo As New TreeNode
                            isoinfo.Text = "SHA-1:" & sha.Text
                            treenode.Nodes.Add(isoinfo)
                        End If
                    Next
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub


    Private Sub SAVE_clrmamepro_Click(sender As System.Object, e As System.EventArgs) Handles SAVE_clrmamepro.Click

        Try
            check = False
            Dim utf8nobom As New UTF8Encoding
            Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\" & System.Environment.ExpandEnvironmentVariables("%username%") & ".dat", False, utf8nobom)
            Dim sb As New StringBuilder
            Dim s As String = ""
            Dim iso As Boolean = False
            Dim size As Long = 0
            Dim lbas(7) As Byte
            Dim lba As Long = 0
            Dim impath As String = ""
            Dim fpath As String = ""
            Dim mode As String = ""
            Dim ss As String = ""
            Dim st As String = ""
            Dim instdir As String = ""
            Dim sizeoffset As Long = 0
            Dim romcode As String() = {" [o]", " []", " [b]", " [!]"}
            Dim emphash As String() = {"00000000", "", ""}
            Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
            Dim hash(2) As String
            Dim psf As New psf
            Dim sss As String = ""
            Dim hit As Integer = 0
            Dim mask As String = ""
            Dim no_intro As String = ""
            sb.AppendLine("clrmamepro (")
            sb.Append(vbTab)
            sb.AppendLine("name ""Sony - PlayStation Portable""")
            sb.Append(vbTab)
            sb.AppendLine("description ""Sony - PlayStation Portable""")
            sb.Append(vbTab)
            sb.Append("version ")
            sb.AppendLine(Now.ToString.Replace("/", "").Substring(0, 8))
            sb.Append(vbTab)
            sb.Append("comment """)
            sb.Append(System.Environment.ExpandEnvironmentVariables("%username%"))
            sb.AppendLine("""")
            sb.AppendLine(")")
            sb.AppendLine()

            If ROMCODEs.Checked = True Then
                Dim dat As String = My.Settings.datpath
                check = False
                Dim ci As System.Globalization.CompareInfo = _
        System.Globalization.CompareInfo.GetCompareInfo("ja-JP")
                If File.Exists(dat) = True Then
                    Dim fs As New FileStream(dat, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    'ファイルを読み込むバイト型配列を作成する
                    Dim bs(CInt(fs.Length - 1)) As Byte
                    'ファイルの内容をすべて読み込む
                    fs.Read(bs, 0, bs.Length)
                    '閉じる
                    fs.Close()
                    'ZIPHEAD
                    If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                        '展開するZIP書庫のパス
                        Dim zipPath As String = dat
                        'ZIP書庫を読み込む 
                        Dim zfs As New System.IO.FileStream( _
                            zipPath, _
                            System.IO.FileMode.Open, _
                            System.IO.FileAccess.Read)
                        'ZipInputStreamオブジェクトの作成 
                        Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                        'ZIP内のエントリを列挙 
                        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                        Dim xmll As Boolean = False
                        While True
                            'ZipEntryを取得
                            ze = zis.GetNextEntry()
                            If ze Is Nothing Then
                                Exit While
                            End If
                            If Not ze.IsDirectory Then
                                '展開先のファイル名を決定 
                                Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                                If ze.Name.Contains(".dat") = True Then
                                    '展開するファイルを読み込む
                                    Dim buffer As Byte() = New Byte(2047) {}
                                    Dim len As Integer = 0
                                    Dim i As Integer = 0
                                    While True
                                        len = zis.Read(buffer, 0, buffer.Length)
                                        If len = 0 Then
                                            Exit While
                                        End If
                                        'ファイルに書き込む
                                        Array.Resize(bs, i + buffer.Length)
                                        Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                        i += buffer.Length
                                    End While
                                    xmll = True
                                End If
                            End If
                        End While

                        '閉じる 
                        zis.Close()
                        zfs.Close()
                    End If

                    no_intro = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                    If no_intro.Contains("crc FB3DF0B7") Then
                        check = True
                    Else
                        MessageBox.Show(Me, "NO_INTRO DAT NOT FOUND")
                        sw.Close()
                        Exit Sub
                    End If
                End If
            End If

            For Each n As TreeNode In TreeView1.Nodes
                If n.Tag.ToString.Contains("HB") Or n.Tag.ToString.Contains("UP") Then
                Else
                    ss = n.Text
                    fpath = n.Nodes(0).Tag.ToString
                    Dim fs As New System.IO.FileStream(fpath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    size = fs.Length
                    fs.Read(lbas, 0, 4)
                    If lbas(0) = &H43 AndAlso lbas(1) = &H49 AndAlso lbas(2) = &H53 AndAlso lbas(3) = &H4F Then
                        fs.Seek(8, SeekOrigin.Begin)
                        fs.Read(lbas, 0, 4)
                        size = BitConverter.ToInt64(lbas, 0)
                        lba = psf.ciso_size(fpath) + size
                    Else
                        fs.Seek(&H8050, SeekOrigin.Begin)
                        fs.Read(lbas, 0, 4)
                        lba = BitConverter.ToInt64(lbas, 0) << 11
                    End If
                    fs.Close()
                    If disck_ver.Checked = True Then
                        If Not n.Text.Contains("(v") Then
                            st = psf.GETNAME(n.Nodes(0).Tag.ToString, "I")
                            If st <> "" Then
                                ss &= " (v" & st & ")"
                            End If
                        End If
                    End If
                    hash(0) = emphash(0)
                    hash(1) = emphash(1)
                    hash(2) = emphash(2)

                    For Each m As TreeNode In n.Nodes
                        If m.Text.Contains("CRC32") Then
                            hash(0) = m.Text.ToString.Remove(0, 7)
                        End If
                        If m.Text.Contains("MD5") Then
                            hash(1) = m.Text.ToString.Remove(0, 5)
                        End If
                        If m.Text.Contains("SHA-1") Then
                            hash(2) = m.Text.ToString.Remove(0, 7)
                        End If
                    Next

                    If ROMCODEs.Checked = True Then
                        mask = "crc " & hash(0) & " md5 " & hash(1) & " sha1 " & hash(2) & ".*? \)\r\n"
                        Dim r As New Regex(mask, RegexOptions.ECMAScript)
                        Dim m As Match = r.Match(no_intro)
                        ss = ss.Replace(romcode(3), "")
                        sizeoffset = size - lba
                        If n.Name.Contains("BADDUMP") Then
                            ss &= romcode(2)
                        End If
                        If n.Checked = True AndAlso check = True AndAlso Not n.Text.Contains("[b]") AndAlso sizeoffset = 0 AndAlso m.Success Then
                            ss &= romcode(3)
                        End If
                        If sizeoffset > 0 AndAlso ss.Contains(romcode(0)) = False Then
                            ss &= romcode(0)
                        ElseIf sizeoffset < 0 AndAlso ss.Contains("[-") = False Then
                            ss &= romcode(1)
                            ss = ss.Insert(ss.Length - 1, (sizeoffset >> 10).ToString & "k")
                        End If
                    End If
                    sb.AppendLine("game (")
                    sb.Append(vbTab)
                    sb.Append("name """)
                    sb.Append(ss)
                    sb.AppendLine("""")
                    sb.Append(vbTab)
                    sb.Append("description """)
                    sb.Append(ss)
                    sb.AppendLine("""")
                    sb.Append(vbTab)
                    sb.Append("serial """)
                    sb.Append(n.Tag.ToString)
                    sb.AppendLine("""")
                    sb.Append(vbTab)
                    sb.Append("rom ( name """)
                    sb.Append(ss)
                    sb.Append(".iso"" size ")
                    sb.Append(size.ToString)
                    sb.Append(" crc ")
                    sb.Append(hash(0))
                    If hash(1) <> "" Then
                        sb.Append(" md5 ")
                        sb.Append(hash(1))
                    End If
                    If hash(2) <> "" Then
                        sb.Append(" sha1 ")
                        sb.Append(hash(2))
                    End If
                    sb.AppendLine(" )")
                    sb.AppendLine(")")
                    sb.AppendLine()
                End If
            Next
            sw.Write(sb.ToString)
            sw.Close()
            Beep()


        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub


    Private Sub EXPORTPSPINSToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EXPORTPSPINS.Click
        Try
            Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\games.ini", False, System.Text.Encoding.GetEncoding(932))
            Dim sb As New StringBuilder
            Dim s As String = ""
            Dim iso As Boolean = False
            Dim impath As String = ""
            Dim fpath As String = ""
            Dim mode As String = ""
            Dim instdir As String = ""
            Dim psf As New psf

            For Each n As TreeNode In TreeView1.Nodes
                sb.Append(po_gei(n.Text))
                sb.Append("|")
                Dim ss As String() = n.Name.Split(CChar(vbLf))
                For Each str As String In ss
                    If str.Contains("X:\") AndAlso iso = False Then
                        instdir = str.Trim
                        iso = True
                    End If
                Next

                For Each m As TreeNode In n.Nodes
                    If m.Tag IsNot Nothing Then
                        fpath = po_gei(m.Tag.ToString)
                        mode = psf.video(m.Tag.ToString)
                        impath = Application.StartupPath & "\imgs\user\" & n.Tag.ToString & "a.png"
                        If File.Exists(impath) Then
                            sb.Append(po_gei(impath))
                        Else
                            If m.Name.ToString.Length > 4 Then
                                impath = m.Name.ToString.Insert(m.Name.ToString.Length - 4, "a")
                                sb.Append(po_gei(impath))
                            End If
                        End If
                        sb.Append("||")
                    End If
                Next

                sb.Append(fpath)
                sb.Append("|")
                sb.Append(Path.GetFileName(fpath))
                sb.Append("|")
                If iso = True AndAlso mode <> "VIDEO" Then
                    sb.Append(instdir)
                    sb.Append("|")
                Else
                    If mode = "PSP" Then
                        sb.Append("X:\ISO\")
                    ElseIf mode = "PBP" Then
                        sb.Append("X:\PSP\GAME\")
                    Else
                        sb.Append("X:\ISO\VIDEO\")
                    End If
                    sb.Append("|")
                End If

                Dim fs As New System.IO.FileStream(fpath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                sb.Append(fs.Length.ToString)
                sb.AppendLine("|")
                fs.Close()
                iso = False
            Next
            sw.Write(sb.ToString)
            sw.Close()


        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Function po_gei(ByVal s As String) As String
        Dim rp As String() = {"－", "ポ", "л", "榎", "掛", "弓", "芸", "鋼", "旨", "楯", "酢", "掃", "竹", "倒", "培", "怖", "翻", "慾", "處", "嘶", "斈", "忿", "掟", "桍", "毫", "烟", "痞", "窩", "縹", "艚", "蛞", "諫", "轎", "閖", "驂", "黥", "埈", "蒴", "僴", "礰"}
        Dim rp2 As String() = {"-", "ﾎﾟ", "ラムダ", "えのき", "かけ", "ゆみ", "げい", "はがね", "むね", "たて", "す", "そう", "たけ", "とう", "ばい", "ふ", "ほん", "よく", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}
        For i = 0 To 39
            If s.Contains(rp(i)) Then
                s = s.Replace(rp(i), rp2(i))
            End If
        Next
        Return s
    End Function

    Private Sub CLOSEToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CLOSEToolStripMenuItem.Click
        Me.Close()
    End Sub

    'SONY
    Private Sub Button2_Click_2(sender As System.Object, e As System.EventArgs) Handles sony.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim s As String = treenode.Tag.ToString
                s = s.Replace("-", "")
                s = StrConv(s, VbStrConv.Lowercase)
                Dim b As Byte() = System.Text.Encoding.UTF8.GetBytes(treenode.Text)
                Dim ss As String = ""
                For i = 0 To b.Length - 1
                    ss &= "%" & b(i).ToString("X")
                Next

                If s.Contains("us") Then
                    Process.Start("http://us.playstation.com/search/index.htm?id=" & ss & " psp&section=true")
                ElseIf s.Contains("es") Then
                    Process.Start("http://uk.playstation.com/search-results/?searchPattern=" & ss & " psp&_newSearch=true&submit-search=Search")
                ElseIf s.Contains("np") Then
                    Process.Start("http://search.jp.playstation.com/search?charset=utf-8&design=2&group=0&query=" & ss & "&submit.x=0&submit.y=0")
                ElseIf s.Contains("hb") Then

                ElseIf s.Contains("up") Then
                    Process.Start(" http://www.jp.playstation.com/psp/update/ud_01.html")
                Else
                    s = "http://www.jp.playstation.com/software/title/" & s & ".html"
                    If status(s) = True Then
                        Process.Start(s)
                    Else
                        Process.Start("http://search.jp.playstation.com/search?charset=utf-8&design=2&group=0&query=" & ss & "&submit.x=0&submit.y=0")
                    End If
                End If

            End If
        Catch ex As Exception
        End Try
    End Sub

    Function status(ByVal url As String) As Boolean

        'WebRequestの作成
        Dim webreq As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)

        Dim webres As System.Net.HttpWebResponse = Nothing
        Try
            'サーバーからの応答を受信するためのWebResponseを取得
            webres = CType(webreq.GetResponse(), Net.HttpWebResponse)

            '応答ステータスコードを表示する
            If webres.StatusCode = Net.HttpStatusCode.OK Then
                Return True
            Else
                Return False
            End If
        Catch ex As System.Net.WebException
            'HTTPプロトコルエラーかどうか調べる
            If ex.Status = System.Net.WebExceptionStatus.ProtocolError Then
                'HttpWebResponseを取得()
                Dim errres As System.Net.HttpWebResponse = _
                    CType(ex.Response, System.Net.HttpWebResponse)
                '応答したURIを表示する
                'MessageBox.Show(Me,errres.StatusCode & vbCrLf & errres.StatusDescription)
                'Console.WriteLine(errres.ResponseUri)
                '応答ステータスコードを表示する
                'Console.WriteLine("{0}:{1}", errres.StatusCode, errres.StatusDescription)
            Else
                MessageBox.Show(Me, ex.Message)
                'Console.WriteLine(ex.Message)
            End If
            Return False
        Finally
            '閉じる
            If Not (webres Is Nothing) Then
                webres.Close()
            End If
        End Try
    End Function

#End Region

#Region "コンテキスト"

    Private Sub treenode_delete(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rg_del.Click

        Try
            If TreeView1.SelectedNode.Level = 0 Then
                If MessageBox.Show(Me, lang(13), lang(14), _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    TreeView1.SelectedNode.Remove()
                    My.Settings.edit = True
                End If
            End If
            If TreeView1.SelectedNode.Level = 1 Then
                If MessageBox.Show(Me, lang(13), lang(14), _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    TreeView1.SelectedNode.Parent.Remove()
                    My.Settings.edit = True
                End If
            End If


        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub tree_add(sender As System.Object, e As System.EventArgs) Handles rg_add.Click

        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            Dim cr As New Regex("\[[0-9A-fa-f]{8}\]", RegexOptions.ECMAScript)
            Dim crcs As Match
            If treenode IsNot Nothing Then
                My.Settings.edit = True
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim rg As New editor
                Me.TopMost = False
                Dim treenode1 As New TreeNode
                '"(新規追加)"
                rg.Text &= lang(15)
                rg.ShowDialog(Me)
                If rg.Text = "APPLY" Then
                    treenode1.Text = rg.gname.Text
                    treenode1.Name = rg.dir.Text.Trim & vbCrLf & rg.note.Text.Trim
                    treenode1.Tag = rg.gid.Text
                    TreeView1.Nodes.Insert(treenode.Index + 1, treenode1)
                    Dim treenode2 As New TreeNode
                    treenode2.Name = rg.impath.Text
                    treenode2.Tag = rg.fpath.Text
                    treenode2.Text = rg.gid.Text
                    treenode1.Nodes.Add(treenode2)
                    crcs = cr.Match(Path.GetFileNameWithoutExtension(rg.fpath.Text))
                    If crcs.Success Then
                        Dim crctree As New TreeNode
                        crctree.Text = ("CRC32; " & crcs.Value.Substring(1, 8))
                        treenode1.Nodes.Add(crctree)
                    End If
                End If
                If My.Settings.topmost = True Then
                    Me.TopMost = True
                End If
                rg.Dispose()
                TreeView1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub edit_tree(sender As System.Object, e As System.EventArgs) Handles rg_edit.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                My.Settings.edit = True
                If treenode.Level = 0 Then
                    treenode = treenode.Nodes(0)
                Else
                    treenode = treenode.Parent.Nodes(0)
                End If

                Dim rg As New editor
                Dim psf As New psf
                rg.Text &= "(" & treenode.Parent.Text & ")"
                Me.TopMost = False

                rg.gname.Text = managename.Text
                rg.gid.Text = gid.Text
                rg.fpath.Text = treenode.Tag.ToString
                rg.impath.Text = treenode.Name
                If File.Exists(treenode.Tag.ToString) Then
                    If psf.video(treenode.Tag.ToString) = "PSP" Then
                        rg.dir.Text = "X:\ISO\"
                    ElseIf psf.video(treenode.Tag.ToString) = "VIDEO" Then
                        rg.dir.Text = "X:\ISO\VIDEO\"
                    Else
                        rg.dir.Text = "X:\PSP\GAME\"
                    End If
                End If

                Dim ss As String() = treenode.Parent.Name.Split(CChar(vbLf))
                For Each s In ss
                    If s.Contains("X:\") Then
                        rg.dir.Text = s.Trim
                    ElseIf s <> "" Then
                        rg.note.Text &= s.Trim & vbCrLf
                    End If
                Next
                rg.ShowDialog(Me)
                If rg.Text = "APPLY" Then
                    treenode.Parent.Text = rg.gname.Text
                    If psf.video(rg.fpath.Text) = "PSP" AndAlso rg.dir.Text = "X:\ISO\" Then
                        treenode.Parent.Name = rg.note.Text.Trim
                    ElseIf psf.video(rg.fpath.Text) = "VIDEO" Then
                        treenode.Parent.Name = rg.note.Text.Trim
                    Else
                        treenode.Parent.Name = rg.dir.Text & vbCrLf & rg.note.Text.Trim
                    End If
                    treenode.Parent.Tag = rg.gid.Text
                    treenode.Name = rg.impath.Text
                    treenode.Tag = rg.fpath.Text
                    treenode.Text = rg.gid.Text
                End If


                If My.Settings.topmost = True Then
                    Me.TopMost = True
                End If
                rg.Dispose()
                TreeView1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub VIEW_Click(sender As System.Object, e As System.EventArgs) Handles VIEW.Click
        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            If treenode.Level = 0 Then
                treenode = treenode.Nodes(0)
            Else
                treenode = treenode.Parent.Nodes(0)
            End If
            Dim rg As New extra
            rg.Text = treenode.Parent.Tag.ToString
            rg.Show()
        End If
    End Sub

    Private Sub EditImage2_Click(sender As System.Object, e As System.EventArgs) Handles EditImage2.Click
        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            If treenode.Level = 1 Then
                treenode = treenode.Parent
            End If

            Dim picture As String = "imgs\user\" & treenode.Tag.ToString & "b.png"
            runapp(picture)
        End If
    End Sub

    Private Sub editimage_Click(sender As System.Object, e As System.EventArgs) Handles editimage.Click
        Dim treenode As TreeNode = TreeView1.SelectedNode
        If treenode IsNot Nothing Then
            If treenode.Level = 1 Then
                treenode = treenode.Parent
            End If

            Dim picture As String = "imgs\user\" & treenode.Tag.ToString & "a.png"
            runapp(picture)
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
        Return FindAssociatedExecutableFile(fileName, "edit")
    End Function

    Function runapp(ByVal impath As String) As Boolean
        Try
            Dim exe As String = FindAssociatedExecutableFile(impath)
            If exe = "" Then
                MessageBox.Show(Me, "関連付けられたプログラムが見つかりませんでした。")
                Return False
            End If
            If File.Exists(impath) = True Then
                Dim boot As New Regex(""".*?""")
                Dim m As Match = boot.Match(exe)
                If m.Success Then
                    Dim p As String = "%1"
                    Process.Start(m.Value, p.Replace("%1", impath))
                End If
            Else
                MessageBox.Show(Me, "ユーザー画像が見つかりません")
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message)
        End Try

        Return True
    End Function

#End Region

#Region "画像"
    Public Function AutoGraphics(ByVal picSource As PictureBox) As Graphics

        If picSource.Image Is Nothing Then

            picSource.Image = New Bitmap(picSource.ClientRectangle.Width, picSource.ClientRectangle.Height)

        End If

        Return Graphics.FromImage(picSource.Image)

    End Function

    Private Sub picture_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles PictureBox1.DragDrop
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim fileName As String() = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "a.png"
                Me.Focus()
                If picture = fileName(0) Then

                ElseIf File.Exists(picture) = True AndAlso MessageBox.Show(Me, lang(39) & picture & lang(40), lang(14), _
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    File.Delete(picture)
                    File.Copy(fileName(0), picture)
                Else
                    File.Copy(fileName(0), picture)
                End If
                bitmap_resize(PictureBox1, picture, 104, 181)

                If picture <> fileName(0) Then
                    Dim bmp As New Bitmap(fileName(0))
                    bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                    bmp.Dispose()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
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
                Me.Focus()
                If picture = fileName(0) Then

                ElseIf File.Exists(picture) = True AndAlso MessageBox.Show(Me, lang(39) & picture & lang(40), lang(14), _
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                    File.Delete(picture)
                    File.Copy(fileName(0), picture)
                Else
                    File.Copy(fileName(0), picture)
                End If
                bitmap_resize(PictureBox2, picture, 320, 181)
                If picture <> fileName(0) Then
                    Dim bmp As New Bitmap(fileName(0))
                    bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                    bmp.Dispose()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick, changeimage.Click

        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
                Dim ofd As New OpenFileDialog()

                ofd.InitialDirectory = My.Settings.imgbase
                '"bmp/png/jpgファイル(*bmp;*.png;*.jpg)|*bmp;*.png;*.jpg"
                '"BMP/PNG/JPGファイルを選択してください"
                ofd.Filter = lang(27)
                ofd.Title = lang(28)
                ofd.RestoreDirectory = True
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "a.png"

                Me.Focus()
                If ofd.ShowDialog() = DialogResult.OK Then
                    If picture = ofd.FileName Then

                    ElseIf File.Exists(picture) = True AndAlso MessageBox.Show(Me, lang(39) & picture & lang(40), lang(14), _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        File.Delete(picture)
                        File.Copy(ofd.FileName, picture)
                    Else
                        File.Copy(ofd.FileName, picture)
                    End If
                    bitmap_resize(PictureBox1, picture, 104, 181)
                    My.Settings.imgbase = Path.GetDirectoryName(ofd.FileName)
                    If picture <> ofd.FileName Then
                        Dim bmp As New Bitmap(ofd.FileName)
                        bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                        bmp.Dispose()
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.DoubleClick, ChangeImage2.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If

                Dim ofd As New OpenFileDialog()
                ofd.InitialDirectory = My.Settings.imgbase
                ofd.Filter = lang(27)
                ofd.Title = lang(28)
                ofd.RestoreDirectory = True
                Dim picture As String = Application.StartupPath & "\imgs\user\" & treenode.Tag.ToString & "b.png"

                Me.Focus()

                If ofd.ShowDialog() = DialogResult.OK Then
                    If picture = ofd.FileName Then

                    ElseIf File.Exists(picture) = True AndAlso MessageBox.Show(Me, lang(39) & picture & lang(40), lang(14), _
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        File.Delete(picture)
                        File.Copy(ofd.FileName, picture)
                    Else
                        File.Copy(ofd.FileName, picture)
                    End If
                    bitmap_resize(PictureBox2, picture, 320, 181)
                    My.Settings.imgbase = Path.GetDirectoryName(ofd.FileName)
                    If picture <> ofd.FileName Then
                        Dim bmp As New Bitmap(ofd.FileName)
                        bmp.Save(picture, System.Drawing.Imaging.ImageFormat.Png)
                        bmp.Dispose()
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

    Function bitmap_resize(ByRef pc As PictureBox, ByVal path As String, ByVal width As Integer, ByVal height As Integer) As Boolean

        Dim image = New Bitmap(path)
        'PictureBox1のGraphicsオブジェクトの作成

        pc.Image = Nothing
        Dim g As Graphics = AutoGraphics(pc) 'pc.CreateGraphics()
        '補間方法として最近傍補間を指定する
        g.InterpolationMode = _
            System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        g.Clear(Color.White)
        '画像を縮小表示
        If image.Width = image.Height AndAlso pc Is PictureBox1 Then
            g.DrawImage(image, 0, 40, width, width)
        ElseIf image.Width = image.Height AndAlso pc Is PictureBox2 Then
            g.DrawImage(image, 80, 0, height, height)
        Else
            g.DrawImage(image, 0, 0, width, height)
        End If
        '補間方法として高品質双三次補間を指定する
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

        'BitmapとGraphicsオブジェクトを破棄
        image.Dispose()
        g.Dispose()
        Return True
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CRCimage.Click
        Try
            If File.Exists(My.Settings.xml) = True Then
                My.Settings.edit = True
                Dim xml As String = My.Settings.xml
                Dim img As String = My.Settings.imgdir
                Dim treenode As TreeNode = TreeView1.SelectedNode
                If treenode IsNot Nothing Then
                    If treenode.Level = 0 Then
                        treenode = treenode.Nodes(0)
                    Else
                        treenode = treenode.Parent.Nodes(0)
                    End If
                    Dim path As String = treenode.Tag.ToString

                    Dim hash As String = crc.Text.ToUpper

                    If crc.Text = "" Then

                        If File.Exists(path) = True Then
                            Dim crc32 As New CRC32()

                            Dim psf As New psf
                            Dim gid As String = My.Settings.unpackdir & treenode.Parent.Tag.ToString
                            Dim fss As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                            Dim bst(4) As Byte
                            If fss.Length > 4 Then
                                fss.Read(bst, 0, 4)
                                fss.Close()
                                If File.Exists(gid) = True Then
                                    path = gid
                                ElseIf bst(0) = &H43 AndAlso bst(1) = &H49 AndAlso bst(2) = &H53 AndAlso bst(3) = &H4F Then
                                    If MessageBox.Show(Me, "CSOなのでアンパックが必要です、時間がかかりますがよろしいですか？", "アンパック", _
                           MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                                        If psf.unpack_cso(path, gid) = True Then
                                            Beep()
                                            path = gid
                                        Else
                                            MessageBox.Show(Me, "アンパックに失敗しました")
                                            Exit Sub
                                        End If
                                    Else
                                        Exit Sub
                                    End If
                                End If
                            End If

                            Using fs As FileStream = File.OpenRead(path)
                                For Each b As Byte In crc32.ComputeHash(fs)
                                    hash += b.ToString("x2").ToLower()
                                Next
                            End Using

                            hash = hash.ToUpper
                        Else
                            '"が見つかりません", "エラー")
                            MessageBox.Show(Me, path & lang(0), lang(1))
                            Exit Sub
                        End If
                    End If

                    Dim ffs As New FileStream(xml, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    'ファイルを読み込むバイト型配列を作成する
                    Dim bs(CInt(ffs.Length - 1)) As Byte
                    'ファイルの内容をすべて読み込む
                    ffs.Read(bs, 0, bs.Length)
                    '閉じる
                    ffs.Close()
                    'ZIPHEAD
                    If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                        '展開するZIP書庫のパス
                        Dim zipPath As String = xml
                        'ZIP書庫を読み込む 
                        Dim zfs As New System.IO.FileStream( _
                            zipPath, _
                            System.IO.FileMode.Open, _
                            System.IO.FileAccess.Read)
                        'ZipInputStreamオブジェクトの作成 
                        Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                        'ZIP内のエントリを列挙 
                        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                        Dim xmll As Boolean = False
                        While True
                            'ZipEntryを取得
                            ze = zis.GetNextEntry()
                            If ze Is Nothing Then
                                Exit While
                            End If
                            If Not ze.IsDirectory Then
                                '展開先のファイル名を決定 
                                Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                                If ze.Name.Contains(".xml") = True Then
                                    '展開するファイルを読み込む
                                    Dim buffer As Byte() = New Byte(2047) {}
                                    Dim len As Integer = 0
                                    Dim i As Integer = 0
                                    While True
                                        len = zis.Read(buffer, 0, buffer.Length)
                                        If len = 0 Then
                                            Exit While
                                        End If
                                        'ファイルに書き込む
                                        Array.Resize(bs, i + buffer.Length)
                                        Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                        i += buffer.Length
                                    End While
                                    xmll = True
                                End If
                            End If
                        End While

                        '閉じる 
                        zis.Close()
                        zfs.Close()
                    End If
                    Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                    Dim hash2 As String = StrConv(hash, VbStrConv.Lowercase)

                    Dim mask As String = "<romCRC extension="".iso"">" & hash & "</romCRC>"
                    Dim mask2 As String = "<romCRC extension="".iso"">" & hash2 & "</romCRC>"
                    Dim r As New Regex(mask, RegexOptions.ECMAScript)
                    Dim m As Match = r.Match(s)
                    Dim q As New Regex(mask2, RegexOptions.ECMAScript)
                    Dim n As Match = q.Match(s)
                    If m.Success Or n.Success Then
                        If m.Success Then
                            s = s.Remove(m.Index + m.Length, s.Length - (m.Index + m.Length))
                        ElseIf n.Success Then
                            s = s.Remove(n.Index + n.Length, s.Length - (n.Index + n.Length))
                        End If
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
                        If num Mod 500 = 0 Then
                            st = 500 * (num \ 500 - 1) + 1
                            en = 500 * (num \ 500)
                        End If
                        img &= st.ToString & "-" & en.ToString & "\"

                        If File.Exists(img & num.ToString & "a.png") Then
                            bitmap_resize(PictureBox1, img & num.ToString & "a.png", 104, 181)
                        End If
                        If File.Exists(img & num.ToString & "b.png") Then
                            bitmap_resize(PictureBox2, img & num.ToString & "b.png", 320, 181)
                        End If
                        treenode.Name = img & num.ToString & ".png"

                        If crc.Text = "" Then

                            Dim k As Integer = treenode.Parent.Nodes.Count
                            For Each nn As TreeNode In treenode.Parent.Nodes
                                If nn.Text.Contains("CRC32") Then
                                    nn.Text = "CRC32: " & hash
                                    Exit For
                                ElseIf k = nn.Index + 1 Then
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
                        'ISOと同じCRC32; "がみつかりませんでした", "CRC不一致")
                        MessageBox.Show(Me, lang(2) & hash & lang(3), lang(4))
                    End If

                End If
            Else
                '"画像検索用のoffline用XMLがみつかりません", "XMLエラー"
                MessageBox.Show(Me, lang(5), lang(6))
            End If

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, lang(7))
        End Try
    End Sub

#End Region


#Region "設定"
    Private Sub rename_dat_Click(sender As System.Object, e As System.EventArgs) Handles rename_dat.Click

        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = Path.GetDirectoryName(My.Settings.datpath)
        '"DAT/ZIPファイルを選択してください"
        ofd.Filter = lang(46)
        ofd.Title = lang(47)
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = DialogResult.OK Then

            Dim fs As New FileStream(ofd.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            'ファイルを読み込むバイト型配列を作成する
            Dim bs(CInt(fs.Length - 1)) As Byte
            'ファイルの内容をすべて読み込む
            fs.Read(bs, 0, bs.Length)
            '閉じる
            fs.Close()
            'ZIPHEAD
            If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                '展開するZIP書庫のパス
                Dim zipPath As String = ofd.FileName
                'ZIP書庫を読み込む 
                Dim zfs As New System.IO.FileStream( _
                    zipPath, _
                    System.IO.FileMode.Open, _
                    System.IO.FileAccess.Read)
                'ZipInputStreamオブジェクトの作成 
                Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                'ZIP内のエントリを列挙 
                Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                Dim xmll As Boolean = False
                While True
                    'ZipEntryを取得
                    ze = zis.GetNextEntry()
                    If ze Is Nothing Then
                        Exit While
                    End If
                    If Not ze.IsDirectory Then
                        '展開先のファイル名を決定 
                        Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                        If ze.Name.Contains(".dat") = True Then
                            '展開するファイルを読み込む
                            Dim buffer As Byte() = New Byte(2047) {}
                            Dim len As Integer = 0
                            Dim i As Integer = 0
                            While True
                                len = zis.Read(buffer, 0, buffer.Length)
                                If len = 0 Then
                                    Exit While
                                End If
                                'ファイルに書き込む
                                Array.Resize(bs, i + buffer.Length)
                                Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                i += buffer.Length
                            End While
                            xmll = True
                        End If
                    End If
                End While

                '閉じる 
                zis.Close()
                zfs.Close()
            End If

            Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
            Dim maskdat As String = "clrmamepro \(\r\n\tname "".+""\r\n\tdescription "".+""\r\n\tversion \d{8}\r\n\tcomment "".+""\r\n\)"
            Dim q As New Regex(maskdat, RegexOptions.ECMAScript)
            Dim dat As Match = q.Match(s)
            If dat.Success Then
                My.Settings.datpath = ofd.FileName
            Else
                '"DATではありません", "DAT"
                MessageBox.Show(Me, lang(48), lang(45))
            End If
        End If
    End Sub

    Private Sub xmlselect_Click(sender As System.Object, e As System.EventArgs) Handles xmlselect.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = Path.GetDirectoryName(My.Settings.xml)
        '"xml/zipファイル(*.xml;*.zip)|*xml;*.zip"
        '"XML/ZIPファイルを選択してください"
        ofd.Filter = lang(29)
        ofd.Title = lang(30)
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = DialogResult.OK Then

            My.Settings.xml = ofd.FileName

            Dim fs As New FileStream(ofd.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            'ファイルを読み込むバイト型配列を作成する
            Dim bs(CInt(fs.Length - 1)) As Byte
            'ファイルの内容をすべて読み込む
            fs.Read(bs, 0, bs.Length)
            '閉じる
            fs.Close()
            'ZIPHEAD
            If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                '展開するZIP書庫のパス
                Dim zipPath As String = ofd.FileName
                'ZIP書庫を読み込む 
                Dim zfs As New System.IO.FileStream( _
                    zipPath, _
                    System.IO.FileMode.Open, _
                    System.IO.FileAccess.Read)
                'ZipInputStreamオブジェクトの作成 
                Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                'ZIP内のエントリを列挙 
                Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                Dim xmll As Boolean = False
                While True
                    'ZipEntryを取得
                    ze = zis.GetNextEntry()
                    If ze Is Nothing Then
                        Exit While
                    End If
                    If Not ze.IsDirectory Then
                        '展開先のファイル名を決定 
                        Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                        If ze.Name.Contains(".xml") = True Then
                            '展開するファイルを読み込む
                            Dim buffer As Byte() = New Byte(2047) {}
                            Dim len As Integer = 0
                            Dim i As Integer = 0
                            While True
                                len = zis.Read(buffer, 0, buffer.Length)
                                If len = 0 Then
                                    Exit While
                                End If
                                'ファイルに書き込む
                                Array.Resize(bs, i + buffer.Length)
                                Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                i += buffer.Length
                            End While
                            xmll = True
                        End If
                    End If
                End While

                '閉じる 
                zis.Close()
                zfs.Close()
            End If

            Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
            Dim maskim As String = "<imFolder>.+</imFolder>"
            Dim maskdat As String = "<datName>.+</datName>"
            Dim r As New Regex(maskim, RegexOptions.ECMAScript)
            Dim im As Match = r.Match(s)
            Dim q As New Regex(maskdat, RegexOptions.ECMAScript)
            Dim dat As Match = q.Match(s)
            Dim ss As String = dat.Value
            ss = ss.Replace("<datName>", "")
            ss = ss.Replace("</datName>", "")
            If Directory.Exists(Application.StartupPath & "\imgs\" & ss & "\") Then
                My.Settings.imgdir = Application.StartupPath & "\imgs\" & ss & "\"
            End If
            ss = im.Value
            ss = ss.Replace("<imFolder>", "")
            ss = ss.Replace("</imFolder>", "")
            If ss <> "" AndAlso Directory.Exists(Application.StartupPath & "\imgs\" & ss) Then
                My.Settings.imgdir = Application.StartupPath & "\imgs\" & ss & "\"
            End If

            If dat.Value = "" Then
                '"OFFLINE用のXMLではありません", "不明XML"
                MessageBox.Show(Me, lang(31), lang(32))
            End If
        End If
    End Sub

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

    Private Sub lockdriveletter_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles lockdriveletter.CheckedChanged

        If lockdriveletter.Checked = False Then
            My.Settings.lockdrive = False
        Else
            My.Settings.lockdrive = True
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles t_gid.CheckedChanged

        If t_gid.Checked = False Then
            My.Settings.pspgid = False
        Else
            My.Settings.pspgid = True
        End If
    End Sub


    Private Sub editpspdirs(sender As System.Object, e As System.EventArgs) Handles editpspdir.Click
        Dim l As New list
        Me.TopMost = False
        l.ShowDialog()
        If My.Settings.topmost = True Then
            Me.TopMost = True
        End If
        l.Dispose()
    End Sub

    Private Sub html(sender As System.Object, e As System.EventArgs) Handles online.Click
        Process.Start("http://unzu127xp.pa.land.to/data/UMDRAW.html")
    End Sub

#End Region

#Region "SORT"

    Private Sub gid_sort_up_Click(sender As System.Object, e As System.EventArgs) Handles gid_sort_up.Click
        Dim s As New sort
        s.sort_game("GID_UP")
        My.Settings.edit = True
    End Sub

    Private Sub gid_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles gid_sort_down.Click
        Dim s As New sort
        s.sort_game("GID_DW")
        My.Settings.edit = True
    End Sub

    Private Sub mane_sort_up_Click(sender As System.Object, e As System.EventArgs) Handles mane_sort_up.Click
        Dim s As New sort
        s.sort_game("GNAME_UP")
        My.Settings.edit = True
    End Sub

    Private Sub mane_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles mane_sort_down.Click
        Dim s As New sort
        s.sort_game("GNAME_DW")
        My.Settings.edit = True
    End Sub

    Private Sub psf_sort_up_Click(sender As System.Object, e As System.EventArgs) Handles psf_sort_up.Click

        Dim s As New sort
        s.sort_game("PSF_UP")
        My.Settings.edit = True
    End Sub

    Private Sub psf_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles psf_sort_down.Click

        Dim s As New sort
        s.sort_game("PSF_DW")
        My.Settings.edit = True
    End Sub

    Private Sub file_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles file_sort_down.Click
        Dim s As New sort
        s.sort_game("FILE_DW")
        My.Settings.edit = True
    End Sub

    Private Sub file_sort_up_Click_1(sender As System.Object, e As System.EventArgs) Handles file_sort_up.Click

        Dim s As New sort
        s.sort_game("FILE_UP")
        My.Settings.edit = True
    End Sub

    Private Sub sort_jp_Click(sender As System.Object, e As System.EventArgs) Handles sort_jp.Click
        Dim s As New sort
        s.sort_game("GID_UP_JP_COUNTRY")
        My.Settings.edit = True
    End Sub

    Private Sub PriorEUToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PriorEUToolStripMenuItem.Click
        Dim s As New sort
        s.sort_game("GID_UP_US_COUNTRY")
        My.Settings.edit = True
    End Sub

    Private Sub PpriorUSAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PpriorUSAToolStripMenuItem.Click
        Dim s As New sort
        s.sort_game("GID_UP_EU_COUNTRY")
        My.Settings.edit = True
    End Sub

#End Region

#Region "リネーム"
    Private Sub DATToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RMDAT.Click
        Try
            Dim dat As String = My.Settings.datpath
            check = False
            Dim utf8nobom As New UTF8Encoding
            Dim ci As System.Globalization.CompareInfo = System.Globalization.CompareInfo.GetCompareInfo("ja-JP")
            If File.Exists(dat) = True Then
                My.Settings.edit = True

                Dim fs As New FileStream(dat, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                'ファイルを読み込むバイト型配列を作成する
                Dim bs(CInt(fs.Length - 1)) As Byte
                'ファイルの内容をすべて読み込む
                fs.Read(bs, 0, bs.Length)
                '閉じる
                fs.Close()
                'ZIPHEAD
                If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                    '展開するZIP書庫のパス
                    Dim zipPath As String = dat
                    'ZIP書庫を読み込む 
                    Dim zfs As New System.IO.FileStream( _
                        zipPath, _
                        System.IO.FileMode.Open, _
                        System.IO.FileAccess.Read)
                    'ZipInputStreamオブジェクトの作成 
                    Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                    'ZIP内のエントリを列挙 
                    Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                    Dim xmll As Boolean = False
                    While True
                        'ZipEntryを取得
                        ze = zis.GetNextEntry()
                        If ze Is Nothing Then
                            Exit While
                        End If
                        If Not ze.IsDirectory Then
                            '展開先のファイル名を決定 
                            Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                            If ze.Name.Contains(".dat") = True Then
                                '展開するファイルを読み込む
                                Dim buffer As Byte() = New Byte(2047) {}
                                Dim len As Integer = 0
                                Dim i As Integer = 0
                                While True
                                    len = zis.Read(buffer, 0, buffer.Length)
                                    If len = 0 Then
                                        Exit While
                                    End If
                                    'ファイルに書き込む
                                    Array.Resize(bs, i + buffer.Length)
                                    Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                    i += buffer.Length
                                End While
                                xmll = True
                            End If
                        End If
                    End While

                    '閉じる 
                    zis.Close()
                    zfs.Close()
                End If
                Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                If s.Contains("crc FB3DF0B7") Then
                    check = True
                End If
                Dim md5pr As Boolean = My.Settings.usemd5
                Dim ss As String = ""
                Dim rp As String = ""
                Dim path As String = ""
                Dim hit As Integer = 0
                TreeView1.CheckBoxes = True
                TreeView1.BeginUpdate()
                For Each n As TreeNode In TreeView1.Nodes
                    n.Checked = False
                    ss = n.Tag.ToString
                    If ss.Contains("UP") Or ss.Contains("HB") Then
                    Else
                        For Each b As TreeNode In n.Nodes
                            If b.Index = 0 Then
                                path = b.Tag.ToString
                            End If
                            If md5pr = True Then
                                If b.Text.Contains("MD5") Then
                                    ss = b.Text.Remove(0, 5).Trim
                                    Dim mask As String = "md5 " & ss & " .*?\)\r\n"
                                    Dim r As New Regex(mask, RegexOptions.ECMAScript)
                                    Dim m As Match = r.Match(s)
                                    If m.Success Then
                                        hit = m.Index
                                        ss = s.Remove(hit + 8, s.Length - (hit + 8))
                                        hit = ss.LastIndexOf("name """)
                                        ss = ss.Remove(0, hit + 6)
                                        hit = ss.IndexOf("""")
                                        ss = ss.Remove(hit, ss.Length - hit)
                                        If My.Settings.filenamecrc = True Then
                                            Dim cr As New Regex("\[[0-9A-Fa-f]{8}\}", RegexOptions.ECMAScript)
                                            Dim crcs As Match = cr.Match(ss)
                                            If crcs.Success Then
                                            ElseIf ss.Length > 4 Then
                                                ss = ss.Insert(ss.Length - 4, " [" & b.Text.Remove(0, 6).Trim & "]")
                                            End If
                                        End If
                                        ss = ss.Replace(".iso", "")
                                        rp = System.IO.Path.GetDirectoryName(path) & "\" & ss & System.IO.Path.GetExtension(path).ToLower

                                        If MNAME.Checked = False AndAlso FILEPATH.Checked = False Then
                                            b.Parent.Checked = True
                                        End If
                                        If MNAME.Checked = True Then
                                            b.Parent.Text = ss.Replace(".iso", "")
                                            b.Parent.Checked = True
                                        End If
                                        If FILEPATH.Checked = True Then
                                            If ci.Compare(rp, path, System.Globalization.CompareOptions.IgnoreCase) = 0 Then
                                                b.Parent.Checked = True
                                            ElseIf File.Exists(rp) = True Then

                                            ElseIf File.Exists(path) = True Then
                                                File.Move(path, rp)
                                                b.Parent.Nodes(0).Tag = rp
                                                b.Parent.Checked = True
                                            End If
                                        End If
                                        If m.Value.Contains("baddump") AndAlso b.Parent.Name.Contains("BADDUMP") = False Then
                                            b.Parent.Name = "BADDUMP" & vbCrLf & b.Parent.Name
                                        ElseIf m.Value.Contains("verified") AndAlso b.Parent.Name.Contains("VRIFIED") = False Then
                                            b.Parent.Name = "VRIFIED" & vbCrLf & b.Parent.Name
                                        End If
                                    End If
                                End If

                            Else

                                If b.Text.Contains("CRC32") Then
                                    ss = b.Text.Remove(0, 6).Trim
                                    Dim mask As String = "crc " & ss & " .*?\)\r\n"
                                    Dim r As New Regex(mask, RegexOptions.ECMAScript)
                                    Dim m As Match = r.Match(s)
                                    If m.Success Then
                                        hit = m.Index
                                        ss = s.Remove(hit + 8, s.Length - (hit + 8))
                                        hit = ss.LastIndexOf("name """)
                                        ss = ss.Remove(0, hit + 6)
                                        hit = ss.IndexOf("""")
                                        ss = ss.Remove(hit, ss.Length - hit)
                                        If My.Settings.filenamecrc = True Then
                                            Dim cr As New Regex("\[[0-9A-Fa-f]{8}\}", RegexOptions.ECMAScript)
                                            Dim crcs As Match = cr.Match(ss)
                                            If crcs.Success Then
                                            ElseIf ss.Length > 4 Then
                                                ss = ss.Insert(ss.Length - 4, " [" & b.Text.Remove(0, 6).Trim & "]")
                                            End If
                                        End If
                                        ss = ss.Replace(".iso", "")
                                        rp = System.IO.Path.GetDirectoryName(path) & "\" & ss & System.IO.Path.GetExtension(path).ToLower

                                        If MNAME.Checked = False AndAlso FILEPATH.Checked = False Then
                                            b.Parent.Checked = True
                                        End If
                                        If MNAME.Checked = True Then
                                            b.Parent.Text = ss.Replace(".iso", "")
                                            b.Parent.Checked = True
                                        End If
                                        If FILEPATH.Checked = True Then
                                            If ci.Compare(rp, path, System.Globalization.CompareOptions.IgnoreCase) = 0 Then
                                                b.Parent.Checked = True
                                            ElseIf File.Exists(rp) Then

                                            ElseIf File.Exists(path) = True Then
                                                File.Move(path, rp)
                                                b.Parent.Nodes(0).Tag = rp
                                                b.Parent.Checked = True
                                            End If
                                        End If
                                        If m.Value.Contains("baddump") AndAlso b.Parent.Name.Contains("BADDUMP") = False Then
                                            b.Parent.Name = "BADDUMP" & vbCrLf & b.Parent.Name
                                        ElseIf m.Value.Contains("verified") AndAlso b.Parent.Name.Contains("VRIFIED") = False Then
                                            b.Parent.Name = "VRIFIED" & vbCrLf & b.Parent.Name
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
                Beep()
                diffCMPRO.Enabled = True
                diffXML.Enabled = False
                TreeView1.EndUpdate()
            Else
                '"リネーム用DATがみつかりません", "DATエラー"
                MessageBox.Show(Me, lang(44), lang(45))
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "")
            TreeView1.EndUpdate()
        End Try
    End Sub

    Private Sub XML使用ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RMXML.Click

        Try
            Dim xml As String = My.Settings.xml
            check = False
            Dim ci As System.Globalization.CompareInfo = _
    System.Globalization.CompareInfo.GetCompareInfo("ja-JP")
            If File.Exists(xml) = True Then
                My.Settings.edit = True
                Dim hash As String = crc.Text.ToUpper
                Dim ffs As New FileStream(xml, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                'ファイルを読み込むバイト型配列を作成する
                Dim bs(CInt(ffs.Length - 1)) As Byte
                'ファイルの内容をすべて読み込む
                ffs.Read(bs, 0, bs.Length)
                '閉じる
                ffs.Close()
                'ZIPHEAD
                If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                    '展開するZIP書庫のパス
                    Dim zipPath As String = xml
                    'ZIP書庫を読み込む 
                    Dim zfs As New System.IO.FileStream( _
                        zipPath, _
                        System.IO.FileMode.Open, _
                        System.IO.FileAccess.Read)
                    'ZipInputStreamオブジェクトの作成 
                    Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                    'ZIP内のエントリを列挙 
                    Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                    Dim xmll As Boolean = False
                    While True
                        'ZipEntryを取得
                        ze = zis.GetNextEntry()
                        If ze Is Nothing Then
                            Exit While
                        End If
                        If Not ze.IsDirectory Then
                            '展開先のファイル名を決定 
                            Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                            If ze.Name.Contains(".xml") = True Then
                                '展開するファイルを読み込む
                                Dim buffer As Byte() = New Byte(2047) {}
                                Dim len As Integer = 0
                                Dim i As Integer = 0
                                While True
                                    len = zis.Read(buffer, 0, buffer.Length)
                                    If len = 0 Then
                                        Exit While
                                    End If
                                    'ファイルに書き込む
                                    Array.Resize(bs, i + buffer.Length)
                                    Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                    i += buffer.Length
                                End While
                                xmll = True
                            End If
                        End If
                    End While

                    '閉じる 
                    zis.Close()
                    zfs.Close()
                End If
                Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                ffs.Close()
                If s.Contains("<romCRC extension="".iso"">A950F5E8</romCRC>") AndAlso s.Contains("<romCRC extension="".iso"">FB3DF0B7</romCRC>") Then
                    check = True
                End If
                Dim ss As String = ""
                Dim rp As String = ""
                Dim path As String = ""
                Dim hit As Integer = 0
                TreeView1.CheckBoxes = True
                TreeView1.BeginUpdate()
                For Each n As TreeNode In TreeView1.Nodes
                    n.Checked = False
                    ss = n.Tag.ToString
                    If ss.Contains("UP") Or ss.Contains("HB") Then
                    Else
                        For Each b As TreeNode In n.Nodes
                            If b.Index = 0 Then
                                path = b.Tag.ToString
                            End If
                            If b.Text.Contains("CRC32") Then
                                ss = b.Text.Remove(0, 6).Trim
                                Dim mask As String = "<romCRC extension="".iso"">" & ss & "</romCRC>"
                                Dim r As New Regex(mask, RegexOptions.ECMAScript)
                                Dim m As Match = r.Match(s)
                                If m.Success Then
                                    ss = s.Remove(m.Index + m.Length, s.Length - (m.Index + m.Length))
                                    ss = ss.Remove(0, ss.LastIndexOf("<title>") + 7)
                                    ss = ss.Remove(ss.LastIndexOf("</title>"), ss.Length - ss.LastIndexOf("</title>"))
                                    If My.Settings.filenamecrc = True Then
                                        Dim cr As New Regex("\[[0-9A-Fa-f]{8}\}", RegexOptions.ECMAScript)
                                        Dim crcs As Match = cr.Match(ss)
                                        If crcs.Success Then
                                        Else
                                            ss &= " [" & b.Text.Remove(0, 6).Trim & "]"
                                        End If
                                    End If
                                    ss = doskiller(ss)


                                    rp = System.IO.Path.GetDirectoryName(path) & "\" & ss & System.IO.Path.GetExtension(path).ToLower
                                    If MNAME.Checked = False AndAlso FILEPATH.Checked = False Then
                                        b.Parent.Checked = True
                                    End If
                                    If MNAME.Checked = True Then
                                        b.Parent.Text = ss
                                        b.Parent.Checked = True
                                    End If
                                    If FILEPATH.Checked = True Then
                                        If ci.Compare(rp, path, System.Globalization.CompareOptions.IgnoreCase) = 0 Then
                                            b.Parent.Checked = True
                                        ElseIf File.Exists(rp) = True Then

                                        ElseIf File.Exists(path) Then
                                            File.Move(path, rp)
                                            b.Parent.Nodes(0).Tag = rp
                                            b.Parent.Checked = True
                                        End If
                                    End If

                                End If
                            End If
                        Next
                    End If
                Next
                diffCMPRO.Enabled = False
                diffXML.Enabled = True
                TreeView1.EndUpdate()
                Beep()
            Else
                '"画像検索用のoffline用XMLがみつかりません", "XMLエラー"
                MessageBox.Show(Me, lang(5), lang(6))
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "")
            TreeView1.EndUpdate()
        End Try
    End Sub

    Private Sub CMPROToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles diffCMPRO.Click
        Try
            Dim dat As String = My.Settings.datpath
            Dim i As Integer = 0
            Dim ci As System.Globalization.CompareInfo = _
    System.Globalization.CompareInfo.GetCompareInfo("ja-JP")

            If File.Exists(dat) = True Then

                Dim fs As New FileStream(dat, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                'ファイルを読み込むバイト型配列を作成する
                Dim bs(CInt(fs.Length - 1)) As Byte
                'ファイルの内容をすべて読み込む
                fs.Read(bs, 0, bs.Length)
                '閉じる
                fs.Close()
                'ZIPHEAD
                If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                    '展開するZIP書庫のパス
                    Dim zipPath As String = dat
                    'ZIP書庫を読み込む 
                    Dim zfs As New System.IO.FileStream( _
                        zipPath, _
                        System.IO.FileMode.Open, _
                        System.IO.FileAccess.Read)
                    'ZipInputStreamオブジェクトの作成 
                    Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                    'ZIP内のエントリを列挙 
                    Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                    Dim xmll As Boolean = False
                    While True
                        'ZipEntryを取得
                        ze = zis.GetNextEntry()
                        If ze Is Nothing Then
                            Exit While
                        End If
                        If Not ze.IsDirectory Then
                            '展開先のファイル名を決定 
                            Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                            If ze.Name.Contains(".dat") = True Then
                                '展開するファイルを読み込む
                                Dim buffer As Byte() = New Byte(2047) {}
                                Dim len As Integer = 0
                                i = 0
                                While True
                                    len = zis.Read(buffer, 0, buffer.Length)
                                    If len = 0 Then
                                        Exit While
                                    End If
                                    'ファイルに書き込む
                                    Array.Resize(bs, i + buffer.Length)
                                    Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                    i += buffer.Length
                                End While
                                xmll = True
                            End If
                        End If
                    End While

                    '閉じる 
                    zis.Close()
                    zfs.Close()
                End If

                Dim utf8nobom As New UTF8Encoding
                Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                Dim ss As String = ""
                Dim rp As String = ""
                Dim path As String = ""
                Dim hit As Integer = 0
                Dim sb As New StringBuilder
                Dim sba As New StringBuilder
                Dim rr As New StringBuilder
                Dim romcode As String() = {" [o]", " []", " [b]", " [!]"}
                Dim emphash As String() = {"00000000", "", ""}
                Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
                Dim title(10000) As String
                Dim desk(10000) As String
                Dim rom(10000) As String
                Dim id(10000) As String
                Dim opath As String = ""
                Dim ppath As String = ""
                Dim tmp As String = ""
                i = 0
                Dim s1 As String = ""
                Dim sss As String = ""
                Dim asd As String = ""
                Dim k As Integer = 0
                Dim mi As Integer = 0
                Dim ud As Integer = 0
                Dim size As Long = 0
                Dim hash(2) As String
                Dim psf As New psf
                Dim st As String = ""
                Dim fpath As String = ""
                Dim lbas(7) As Byte
                Dim lba As Long = 0
                Dim sabun As Boolean = False

                sba.AppendLine("clrmamepro (")
                sba.Append(vbTab)
                sba.AppendLine("name ""Sony - PlayStation Portable""")
                sba.Append(vbTab)
                sba.AppendLine("description ""Sony - PlayStation Portable""")
                sba.Append(vbTab)
                sba.Append("version ")
                sba.AppendLine(Now.ToString.Replace("/", "").Substring(0, 8))
                sba.Append(vbTab)
                sba.Append("comment """)
                sba.Append(System.Environment.ExpandEnvironmentVariables("%username%"))
                sba.AppendLine("""")
                sba.AppendLine(")")
                sba.AppendLine()

                TreeView1.CheckBoxes = True
                TreeView1.BeginUpdate()
                For Each n As TreeNode In TreeView1.Nodes
                    ss = n.Tag.ToString
                    hash(0) = emphash(0)
                    hash(1) = emphash(1)
                    hash(2) = emphash(2)
                    If ss.Contains("UP") Or ss.Contains("HB") Then
                    Else
                        For Each bb As TreeNode In n.Nodes
                            If bb.Index = 0 Then
                                path = bb.Tag.ToString
                            End If
                            If bb.Text.Contains("MD5") Then
                                hash(1) = bb.Text.ToString.Remove(0, 5)
                            End If
                            If bb.Text.Contains("SHA-1") Then
                                hash(2) = bb.Text.ToString.Remove(0, 7)
                            End If
                            If bb.Text.Contains("CRC32") Then
                                hash(0) = bb.Text.ToString.Remove(0, 7)
                            End If

                            If bb.Index = n.Nodes.Count - 1 AndAlso n.Checked = False Then

                                Dim fss As New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                size = fss.Length
                                fss.Read(lbas, 0, 4)
                                If lbas(0) = &H43 AndAlso lbas(1) = &H49 AndAlso lbas(2) = &H53 AndAlso lbas(3) = &H4F Then
                                    fss.Seek(8, SeekOrigin.Begin)
                                    fss.Read(lbas, 0, 4)
                                    size = BitConverter.ToInt64(lbas, 0)
                                    lba = psf.ciso_size(path) + size
                                Else
                                    fss.Seek(&H8050, SeekOrigin.Begin)
                                    fss.Read(lbas, 0, 4)
                                    lba = BitConverter.ToInt64(lbas, 0) << 11
                                End If
                                fss.Close()
                                ss = n.Text
                                If disck_ver.Checked = True Then
                                    If Not n.Text.Contains("(v") Then
                                        st = psf.GETNAME(path, "I")
                                        If st <> "" Then
                                            ss &= " (v" & st & ")"
                                        End If
                                    End If
                                End If

                                If ROMCODEs.Checked = True Then
                                    Dim sizeoffset As Long = size - lba
                                    If n.Name.Contains("BADDUMP") Then
                                        ss &= romcode(2)
                                    End If
                                    If sizeoffset > 0 AndAlso ss.Contains(romcode(0)) = False Then
                                        ss &= romcode(0)
                                    ElseIf sizeoffset < 0 AndAlso ss.Contains("[-") = False Then
                                        ss &= romcode(1)
                                        ss = ss.Insert(ss.Length - 1, (sizeoffset >> 10).ToString & "k")
                                    End If
                                End If

                                sb.AppendLine("game (")
                                sb.Append(vbTab)
                                sb.Append("name """)
                                sb.Append(ss)
                                sb.AppendLine("""")
                                sb.Append(vbTab)
                                sb.Append("description """)
                                sb.Append(ss)
                                sb.AppendLine("""")
                                sb.Append(vbTab)
                                sb.Append("serial """)
                                sb.Append(n.Tag.ToString)
                                sb.AppendLine("""")
                                sb.Append(vbTab)
                                sb.Append("rom ( name """)
                                sb.Append(ss)
                                sb.Append(".iso"" size ")
                                sb.Append(size.ToString)
                                sb.Append(" crc ")
                                sb.Append(hash(0))
                                If hash(1) <> "" Then
                                    sb.Append(" md5 ")
                                    sb.Append(hash(1))
                                End If
                                If hash(2) <> "" Then
                                    sb.Append(" sha1 ")
                                    sb.Append(hash(2))
                                End If
                                sb.AppendLine(" )")
                                sb.AppendLine(")")
                                sb.AppendLine()
                            End If
                        Next
                    End If
                Next

                Dim wr As New System.IO.StreamWriter(Application.StartupPath & "\diff\" & Now.ToString.Replace("/", "").Replace(":", "") & ".dat",
                        False, utf8nobom)
                wr.Write(sba.ToString)
                wr.Write(sb.ToString)
                wr.Close()
                diffCMPRO.Enabled = True
                diffXML.Enabled = False
                Beep()
                TreeView1.EndUpdate()

            Else
                '"リネーム用DATがみつかりません", "DATエラー"
                MessageBox.Show(Me, lang(44), lang(45))
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "")
            TreeView1.EndUpdate()
        End Try
    End Sub


    Private Sub ToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles exportxml.Click

        Try

            Dim imgbase As New Regex("<imFolder>.*?</imFolder>")
            Dim imb As Match = imgbase.Match(My.Settings.xmlhead)
            If imb.Success Then
                Dim s As String = imb.Value.Remove(0, 10)
                s = s.Remove(s.Length - 11, 11) & "\"
                Dim i As Integer = 0
                Dim ss As String = ""
                Dim rp As String = ""
                Dim path As String = ""
                Dim impath As String = ""
                Dim impathu As String = ""
                Dim impathu2 As String = ""
                Dim tmpim As String = ""
                Dim tmpim2 As String = ""
                Dim img As String = ""
                Dim hit As Integer = 0
                Dim sb As New StringBuilder
                Dim rr As New StringBuilder
                Dim romcode As String() = {" [o]", " []", " [b]", " [!]"}
                Dim emphash As String() = {"00000000", "000000000000000000000000", "000000000000000000000000000000000"}
                Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
                Dim title(10000) As String
                Dim desk(10000) As String
                Dim rom(10000) As String
                Dim id(10000) As String
                Dim opath As String = ""
                Dim ppath As String = ""
                Dim tmp As String = ""
                i = My.Settings.xmlindex
                Dim s1 As String = ""
                Dim sss As String = ""
                Dim asd As String = ""
                Dim k As Integer = 0
                Dim mi As Integer = 0
                Dim ud As Integer = 0
                Dim size As Long = 0
                Dim hash(2) As String
                Dim psf As New psf
                Dim st As String = ""
                Dim sabun As Boolean = False
                Dim nn As Integer = 0
                Dim lbas(7) As Byte

                img = Application.StartupPath & "\diff\imgs\" & s & "\"
                If Directory.Exists(img) = False Then
                    Directory.CreateDirectory(img)
                End If
                TreeView1.CheckBoxes = True
                TreeView1.BeginUpdate()
                For Each n As TreeNode In TreeView1.Nodes
                    ss = n.Tag.ToString
                    If ss.Contains("UP") Or ss.Contains("HB") Then
                    Else
                        For Each bb As TreeNode In n.Nodes
                            If bb.Index = 0 Then
                                hash(0) = emphash(0)
                                hash(1) = emphash(1)
                                hash(2) = emphash(2)
                                path = bb.Tag.ToString
                                impath = bb.Name.ToString
                                impathu = Application.StartupPath & "\imgs\user\" & ss & "a.png"
                                impathu2 = Application.StartupPath & "\imgs\user\" & ss & "b.png"
                            End If
                            If bb.Text.Contains("MD5") Then
                                hash(1) = bb.Text.ToString.Remove(0, 5)
                            End If
                            If bb.Text.Contains("SHA-1") Then
                                hash(2) = bb.Text.ToString.Remove(0, 7)
                            End If
                            If bb.Text.Contains("CRC32") Then
                                hash(0) = bb.Text.ToString.Remove(0, 7)
                            End If

                            If bb.Index = n.Nodes.Count - 1 Then

                                path = n.Nodes(0).Tag.ToString
                                Dim fhs As New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                size = fhs.Length
                                fhs.Read(lbas, 0, 4)
                                If lbas(0) = &H43 AndAlso lbas(1) = &H49 AndAlso lbas(2) = &H53 AndAlso lbas(3) = &H4F Then
                                    fhs.Seek(8, SeekOrigin.Begin)
                                    fhs.Read(lbas, 0, 4)
                                    size = BitConverter.ToInt64(lbas, 0)
                                End If
                                fhs.Close()
                                ss = doskiller2(n.Text)

                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<game>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<imageNumber>")
                                sb.Append(i.ToString)
                                sb.AppendLine("</imageNumber>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<releaseNumber>")
                                sb.Append(i.ToString)
                                sb.AppendLine("</releaseNumber>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<title>")
                                sb.Append(ss)
                                sb.AppendLine("</title>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<saveType>9999/99/99</saveType>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<romSize>")
                                sb.Append(size.ToString)
                                sb.AppendLine("</romSize>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<publisher>PSP</publisher>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<location>7</location>")
                                'sb.Append("<location>")
                                'sb.Append(nn.ToString)
                                'sb.AppendLine("</location>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<sourceRom>PSP Filer Ver.3.9以降</sourceRom>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<language>256</language>")
                                'sb.Append("<language>")
                                'sb.Append((1 << nn).ToString)
                                'sb.AppendLine("</language>")
                                nn += 1
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<files>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<romCRC extension="".iso"">")
                                sb.Append(hash(0))
                                sb.AppendLine("</romCRC>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("</files>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<im1CRC>")
                                sb.Append(n.Tag.ToString)
                                sb.Append(" / ")
                                sb.Append(n.Tag.ToString.Replace("-", ""))
                                'sb.Append(hash(1))
                                sb.AppendLine("</im1CRC>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<im2CRC>")
                                sb.Append(psf.GETNAME(n.Nodes(0).Tag.ToString, "XML"))
                                'sb.Append("9.99 / 9.99")
                                'sb.Append(hash(2))
                                sb.AppendLine("</im2CRC>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<comment>")
                                sb.AppendLine("</comment>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<duplicateID>")
                                sb.Append(i.ToString)
                                sb.AppendLine("</duplicateID>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("</game>")
                                Dim num As Integer = i
                                Dim stt, en As Integer
                                stt = 500 * (num \ 500) + 1
                                en = 500 * (num \ 500 + 1)
                                If num Mod 500 = 0 Then
                                    stt = 500 * (num \ 500 - 1) + 1
                                    en = 500 * (num \ 500)
                                End If
                                Dim imgt As String = img & stt.ToString & "-" & en.ToString & "\"
                                If Directory.Exists(imgt) = False Then
                                    Directory.CreateDirectory(imgt)
                                End If
                                If impath.Length > 4 Then
                                    tmpim = impath.Insert(impath.Length - 4, "a")
                                    tmpim2 = impath.Insert(impath.Length - 4, "b")
                                Else
                                    tmpim = ""
                                    tmpim2 = ""
                                End If


                                If File.Exists(impathu) = True Then
                                    If File.Exists(imgt & i.ToString & "a.png") Then
                                        File.Delete(imgt & i.ToString & "a.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(impathu)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 104
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30

                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "a.png")
                                ElseIf File.Exists(tmpim) Then
                                    If File.Exists(imgt & i.ToString & "a.png") Then
                                        File.Delete(imgt & i.ToString & "a.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(tmpim)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 104
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30
                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "a.png")

                                End If


                                If File.Exists(impathu2) = True Then
                                    If File.Exists(imgt & i.ToString & "b.png") Then
                                        File.Delete(imgt & i.ToString & "b.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(impathu2)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 320
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30

                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "b.png")
                                ElseIf File.Exists(tmpim2) Then
                                    If File.Exists(imgt & i.ToString & "b.png") Then
                                        File.Delete(imgt & i.ToString & "b.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(tmpim2)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 320
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30

                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "b.png")
                                End If
                                i += 1
                            End If
                        Next
                    End If
                Next

                PictureBox1.SetBounds(260, 64, 104, 181)
                Dim utf8nobom As New UTF8Encoding
                Dim wr As New System.IO.StreamWriter(Application.StartupPath & "\" & Now.ToString.Replace("/", "").Replace(":", "") & ".xml",
                                        False, utf8nobom)
                wr.Write(My.Settings.xmlhead)
                wr.Write(sb.ToString)
                wr.Write(My.Settings.xmlfoot)
                wr.Close()
                Beep()
                TreeView1.EndUpdate()
            Else
                MessageBox.Show(Me, "XMLヘッダーに<imFoder>がありません")
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "")
            TreeView1.EndUpdate()
        End Try
    End Sub

    Private Sub XMLToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles diffXML.Click

        Try
            Dim xml As String = My.Settings.xml
            Dim i As Integer = 0
            If File.Exists(xml) = True Then
                My.Settings.edit = True
                Dim ffs As New FileStream(xml, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                'ファイルを読み込むバイト型配列を作成する
                Dim bs(CInt(ffs.Length - 1)) As Byte
                'ファイルの内容をすべて読み込む
                ffs.Read(bs, 0, bs.Length)
                '閉じる
                ffs.Close()
                'ZIPHEAD
                If bs(0) = &H50 AndAlso bs(1) = &H4B AndAlso bs(2) = &H3 AndAlso bs(3) = &H4 Then
                    '展開するZIP書庫のパス
                    Dim zipPath As String = xml
                    'ZIP書庫を読み込む 
                    Dim zfs As New System.IO.FileStream( _
                        zipPath, _
                        System.IO.FileMode.Open, _
                        System.IO.FileAccess.Read)
                    'ZipInputStreamオブジェクトの作成 
                    Dim zis As New ICSharpCode.SharpZipLib.Zip.ZipInputStream(zfs)

                    'ZIP内のエントリを列挙 
                    Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry
                    Dim xmll As Boolean = False
                    While True
                        'ZipEntryを取得
                        ze = zis.GetNextEntry()
                        If ze Is Nothing Then
                            Exit While
                        End If
                        If Not ze.IsDirectory Then
                            '展開先のファイル名を決定 
                            Dim fileName As String = System.IO.Path.GetFileName(ze.Name)

                            If ze.Name.Contains(".xml") = True Then
                                '展開するファイルを読み込む
                                Dim buffer As Byte() = New Byte(2047) {}
                                Dim len As Integer = 0
                                While True
                                    len = zis.Read(buffer, 0, buffer.Length)
                                    If len = 0 Then
                                        Exit While
                                    End If
                                    'ファイルに書き込む
                                    Array.Resize(bs, i + buffer.Length)
                                    Array.ConstrainedCopy(buffer, 0, bs, i, buffer.Length)
                                    i += buffer.Length
                                End While
                                xmll = True
                            End If
                        End If
                    End While

                    '閉じる 
                    zis.Close()
                    zfs.Close()
                End If

                Dim s As String = System.Text.Encoding.GetEncoding(65001).GetString(bs)
                Dim maskim As String = "<imFolder>.+</imFolder>"
                Dim maskdat As String = "<datName>.+</datName>"
                Dim r As New Regex(maskim, RegexOptions.ECMAScript)
                Dim im As Match = r.Match(s)
                Dim q As New Regex(maskdat, RegexOptions.ECMAScript)
                Dim dat As Match = q.Match(s)
                Dim ss As String = dat.Value
                ss = ss.Replace("<datName>", "")
                ss = ss.Replace("</datName>", "")
                If Directory.Exists(Application.StartupPath & "\imgs\" & ss & "\") Then
                    s = ss & "\"
                End If
                ss = im.Value
                ss = ss.Replace("<imFolder>", "")
                ss = ss.Replace("</imFolder>", "")
                If ss <> "" AndAlso Directory.Exists(Application.StartupPath & "\imgs\" & ss) Then
                    s = ss & "\"
                End If

                If dat.Value = "" Then
                    '"OFFLINE用のXMLではありません", "不明XML"
                    MessageBox.Show(Me, lang(31), lang(32))
                    Exit Sub
                End If

                Dim rp As String = ""
                Dim path As String = ""
                Dim impath As String = ""
                Dim impathu As String = ""
                Dim impathu2 As String = ""
                Dim tmpim As String = ""
                Dim tmpim2 As String = ""
                Dim img As String = ""
                Dim hit As Integer = 0
                Dim sb As New StringBuilder
                Dim rr As New StringBuilder
                Dim romcode As String() = {" [o]", " []", " [b]", " [!]"}
                Dim emphash As String() = {"00000000", "000000000000000000000000", "000000000000000000000000000000000"}
                Dim flag As String() = {" flags verified", " flag baddump", " flags nodump"}
                Dim title(10000) As String
                Dim desk(10000) As String
                Dim rom(10000) As String
                Dim id(10000) As String
                Dim opath As String = ""
                Dim ppath As String = ""
                Dim tmp As String = ""
                i = My.Settings.sabunindex
                Dim s1 As String = ""
                Dim sss As String = ""
                Dim asd As String = ""
                Dim k As Integer = 0
                Dim mi As Integer = 0
                Dim ud As Integer = 0
                Dim size As Long = 0
                Dim hash(2) As String
                Dim psf As New psf
                Dim st As String = ""
                Dim lbas(7) As Byte
                Dim sabun As Boolean = False

                img = Application.StartupPath & "\diff\imgs\" & s
                If Directory.Exists(img) = False Then
                    Directory.CreateDirectory(img)
                End If
                TreeView1.CheckBoxes = True
                TreeView1.BeginUpdate()
                For Each n As TreeNode In TreeView1.Nodes
                    ss = n.Tag.ToString
                    If ss.Contains("UP") Or ss.Contains("HB") Then
                    Else
                        For Each bb As TreeNode In n.Nodes
                            If bb.Index = 0 Then
                                hash(0) = emphash(0)
                                hash(1) = emphash(1)
                                hash(2) = emphash(2)
                                path = bb.Tag.ToString
                                impath = bb.Name.ToString
                                impathu = Application.StartupPath & "\imgs\user\" & ss & "a.png"
                                impathu2 = Application.StartupPath & "\imgs\user\" & ss & "b.png"
                            End If
                            If bb.Text.Contains("MD5") Then
                                hash(1) = bb.Text.ToString.Remove(0, 5)
                            End If
                            If bb.Text.Contains("SHA-1") Then
                                hash(2) = bb.Text.ToString.Remove(0, 7)
                            End If
                            If bb.Text.Contains("CRC32") Then
                                hash(0) = bb.Text.ToString.Remove(0, 7)
                            End If

                            If bb.Index = n.Nodes.Count - 1 AndAlso n.Checked = False Then
                                path = n.Nodes(0).Tag.ToString
                                Dim fhs As New System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                size = fhs.Length
                                fhs.Read(lbas, 0, 4)
                                If lbas(0) = &H43 AndAlso lbas(1) = &H49 AndAlso lbas(2) = &H53 AndAlso lbas(3) = &H4F Then
                                    fhs.Seek(8, SeekOrigin.Begin)
                                    fhs.Read(lbas, 0, 4)
                                    size = BitConverter.ToInt64(lbas, 0)
                                End If
                                fhs.Close()
                                ss = doskiller2(n.Text)

                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<game>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<imageNumber>")
                                sb.Append(i.ToString)
                                sb.AppendLine("</imageNumber>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<releaseNumber>")
                                sb.Append(i.ToString)
                                sb.AppendLine("</releaseNumber>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<title>")
                                sb.Append(ss)
                                sb.AppendLine("</title>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<saveType>9999/99/99</saveType>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<romSize>")
                                sb.Append(size.ToString)
                                sb.AppendLine("</romSize>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<publisher>ユーザー</publisher>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<location>7</location>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<sourceRom>PSP Filer Ver.3.9以降</sourceRom>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<language>256</language>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("<files>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<romCRC extension="".iso"">")
                                sb.Append(hash(0))
                                sb.AppendLine("</romCRC>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("</files>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<im1CRC>")
                                sb.Append(n.Tag.ToString)
                                sb.Append(" / ")
                                sb.Append(n.Tag.ToString.Replace("-", ""))
                                'sb.Append(hash(1))
                                sb.AppendLine("</im1CRC>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<im2CRC>")
                                sb.Append(psf.GETNAME(n.Nodes(0).Tag.ToString, "XML"))
                                'sb.Append("9.99 / 9.99")
                                'sb.Append(hash(2))
                                sb.AppendLine("</im2CRC>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<comment>")
                                sb.AppendLine("</comment>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.Append("<duplicateID>")
                                sb.Append(i.ToString)
                                sb.AppendLine("</duplicateID>")
                                sb.Append(vbTab)
                                sb.Append(vbTab)
                                sb.AppendLine("</game>")
                                Dim num As Integer = i
                                Dim stt, en As Integer
                                stt = 500 * (num \ 500) + 1
                                en = 500 * (num \ 500 + 1)
                                If (num Mod 500) = 0 Then
                                    stt = 500 * (num \ 500 - 1) + 1
                                    en = 500 * (num \ 500)
                                End If
                                Dim imgt As String = img & stt.ToString & "-" & en.ToString & "\"
                                If Directory.Exists(imgt) = False Then
                                    Directory.CreateDirectory(imgt)
                                End If
                                If impath.Length > 4 Then
                                    tmpim = impath.Insert(impath.Length - 4, "a")
                                    tmpim2 = impath.Insert(impath.Length - 4, "b")
                                Else
                                    tmpim = ""
                                    tmpim2 = ""
                                End If


                                If File.Exists(impathu) = True Then
                                    If File.Exists(imgt & i.ToString & "a.png") Then
                                        File.Delete(imgt & i.ToString & "a.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(impathu)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 104
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30

                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "a.png")
                                ElseIf File.Exists(tmpim) Then
                                    If File.Exists(imgt & i.ToString & "a.png") Then
                                        File.Delete(imgt & i.ToString & "a.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(tmpim)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 104
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30
                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "a.png")

                                End If


                                If File.Exists(impathu2) = True Then
                                    If File.Exists(imgt & i.ToString & "b.png") Then
                                        File.Delete(imgt & i.ToString & "b.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(impathu2)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 320
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30

                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "b.png")
                                ElseIf File.Exists(tmpim2) Then
                                    If File.Exists(imgt & i.ToString & "b.png") Then
                                        File.Delete(imgt & i.ToString & "b.png")
                                    End If

                                    PictureBox1.Image = Image.FromFile(tmpim2)
                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Dim objBtm As Bitmap
                                    Dim lngW As Integer = 320
                                    Dim lngH As Integer = 181
                                    objBtm = New Bitmap(PictureBox1.Image)
                                    Dim objBtm2 As New Bitmap(objBtm, lngW, lngH)

                                    PictureBox1.Image = objBtm2

                                    PictureBox1.SetBounds(0, 0, _
                                    PictureBox1.Image.Size.Width + 2, _
                                    PictureBox1.Image.Size.Height + 30)

                                    Me.Width = PictureBox1.Image.Size.Width + 2
                                    Me.Height = PictureBox1.Image.Size.Height + 30

                                    Dim strImageFileName As String
                                    strImageFileName = "cvt.png"
                                    PictureBox1.Image.Save(strImageFileName, Imaging.ImageFormat.Png)
                                    File.Copy("cvt.png", imgt & i.ToString & "b.png")
                                End If
                                i += 1
                            End If
                        Next
                    End If
                Next

                PictureBox1.SetBounds(260, 64, 104, 181)
                Dim utf8nobom As New UTF8Encoding
                Dim wr As New System.IO.StreamWriter(Application.StartupPath & "\diff\datas\" & s.Replace("\", "") & "_diff_" & Now.ToString.Replace("/", "").Replace(":", "") & ".xml",
                                        False, utf8nobom)
                wr.Write(sb.ToString)
                wr.Close()
                Beep()
                TreeView1.EndUpdate()
            Else
                '"画像検索用のoffline用XMLがみつかりません", "XMLエラー"
                MessageBox.Show(Me, lang(5), lang(6))
            End If
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "")
            TreeView1.EndUpdate()
        End Try
    End Sub

    Function doskiller(ByVal s As String) As String

        Dim ss As String() = {"/", "\", "?", "*", ":", "|", """", "<", ">"}
        'http://www.scollabo.com/banban/apply/ap8.html
        Dim html As String() = {"&amp;", "&gt;", "&lt;", "&apos;", "&quot;", "&nbsp;", "&iexcl;", "&cent;", "&pound;", "&curren;", "&yen;", "&brvbar;", "&sect;", "&uml;", "&copy;", "&ordf;", "&laquo;", "&not;", "&shy;", "&reg;", "&macr;", "&deg;", "&plusmn;", "&sup2;", "&sup3;", "&acute;", "&micro;", "&para;", "&middot;", "&cedil;", "&sup1;", "&ordm;", "&raquo;", "&frac14;", "&frac12;", "&frac34;", "&iquest;", "&Agrave;", "&Aacute;", "&Acirc;", "&Atilde;", "&Auml;", "&Aring;", "&AElig;", "&Ccedil;", "&Egrave;", "&Eacute;", "&Ecirc;", "&Euml;", "&Igrave;", "&Iacute;", "&Icirc;", "&Iuml;", "&ETH;", "&Ntilde;", "&Ograve;", "&Oacute;", "&Ocirc;", "&Otilde;", "&Ouml;", "&times;", "&Oslash;", "&Ugrave;", "&Uacute;", "&Ucirc;", "&Uuml;", "&Yacute;", "&THORN;", "&szlig;", "&agrave;", "&aacute;", "&acirc;", "&atilde;", "&auml;", "&aring;", "&aelig;", "&ccedil;", "&egrave;", "&eacute;", "&ecirc;", "&euml;", "&igrave;", "&iacute;", "&icirc;", "&iuml;", "&eth;", "&ntilde;", "&ograve;", "&oacute;", "&ocirc;", "&otilde;", "&ouml;", "&divide;", "&oslash;", "&ugrave;", "&uacute;", "&ucirc;", "&uuml;", "&yacute;", "&thorn;", "&yuml;", "&Alpha;", "&Beta;", "&Gamma;", "&Delta;", "&Epsilon;", "&Zeta;", "&Eta;", "&Theta;", "&Iota;", "&Kappa;", "&Lambda;", "&Mu;", "&Nu;", "&Xi;", "&Omicron;", "&Pi;", "&Pho;", "&Sigma;", "&Tau;", "&Upsilon;", "&Phi;", "&Chi;", "&Psi;", "&Omega;", "&alpha;", "&beta;", "&gamma;", "&delta;", "&epsilon;", "&zeta;", "&eta;", "&theta;", "&iota;", "&kappa;", "&lambda;", "&mu;", "&nu;", "&xi;", "&omicron;", "&pi;", "&rho;", "&sigmaf;", "&sigma;", "&tau;", "&upsilon;", "&phi;", "&chi;", "&psi;", "&omega;", "&ndash;", "&mdash;", "&lsquo;", "&rsquo;", "&sbquo;", "&ldquo;", "&rdquo;", "&bdquo;", "&dagger;", "&Dagger;", "&bull;", "&hellip;", "&permil;", "&prime;", "&Prime;", "&lsaquo;", "&rsaquo;", "&oline;", "&frasl;", "&euro;", "&weierp;", "&image;", "&real;", "&trade;", "&alefsym;", "&larr;", "&rarr;", "&darr;", "&harr;", "&crarr;", "&lArr;", "&uArr;", "&rArr;", "&dArr;", "&hArr;", "&loz;", "&spades;", "&clubs;", "&hearts;", "&diams;", "&forall;", "&part;", "&exist;", "&empty;", "&nabla;", "&isin;", "&notin;", "&ni;", "&prod;", "&sum;", "&minus;", "&lowast;", "&radic;", "&prop;", "&infin;", "&ang;", "&and;", "&or;", "&cap;", "&cup;", "&int;", "&there4;", "&sim;", "&cong;", "&asymp;", "&ne;", "&equiv;", "&le;", "&ge;", "&sub;", "&sup;", "&nsub;", "&sube;", "&supe;", "&oplus;", "&otimes;", "&perp;"}
        Dim html2 As String() = {"&", "", "", "'", "", "&#160;", "&#161;", "&#162;", "&#163;", "&#164;", "&#165;", "&#166;", "&#167;", "&#168;", "&#169;", "&#170;", "&#171;", "&#172;", "&#173;", "&#174;", "&#175;", "&#176;", "&#177;", "&#178;", "&#179;", "&#180;", "&#181;", "&#182;", "&#183;", "&#184;", "&#185;", "&#186;", "&#187;", "&#188;", "&#189;", "&#190;", "&#191;", "&#192;", "&#193;", "&#194;", "&#195;", "&#196;", "&#197;", "&#198;", "&#199;", "&#200;", "&#201;", "&#202;", "&#203;", "&#204;", "&#205;", "&#206;", "&#207;", "&#208;", "&#209;", "&#210;", "&#211;", "&#212;", "&#213;", "&#214;", "&#215;", "&#216;", "&#217;", "&#218;", "&#219;", "&#220;", "&#221;", "&#222;", "&#223;", "&#224;", "&#225;", "&#226;", "&#227;", "&#228;", "&#229;", "&#230;", "&#231;", "&#232;", "&#233;", "&#234;", "&#235;", "&#236;", "&#237;", "&#238;", "&#239;", "&#240;", "&#241;", "&#242;", "&#243;", "&#244;", "&#245;", "&#246;", "&#247;", "&#248;", "&#249;", "&#250;", "&#251;", "&#252;", "&#253;", "&#254;", "&#255;", "&#913;", "&#914;", "&#915;", "&#916;", "&#917;", "&#918;", "&#919;", "&#920;", "&#921;", "&#922;", "&#923;", "&#924;", "&#925;", "&#926;", "&#927;", "&#928;", "&#929;", "&#931;", "&#932;", "&#933;", "&#934;", "&#935;", "&#936;", "&#937;", "&#945;", "&#946;", "&#947;", "&#948;", "&#949;", "&#950;", "&#951;", "&#952;", "&#953;", "&#954;", "&#955;", "&#956;", "&#957;", "&#958;", "&#959;", "&#960;", "&#961;", "&#962;", "&#963;", "&#964;", "&#965;", "&#966;", "&#967;", "&#968;", "&#969;", "&#8211;", "&#8212;", "&#8216;", "&#8217;", "&#8218;", "&#8220;", "&#8221;", "&#8222;", "&#8224;", "&#8225;", "&#8226;", "&#8230;", "&#8240;", "&#8242;", "&#8243;", "&#8249;", "&#8250;", "&#8254;", "&#8260;", "&#8364;", "&#8472;", "&#8465;", "&#8476;", "&#8482;", "&#8501;", "&#8592;", "&#8594;", "&#8595;", "&#8596;", "&#8629;", "&#8656;", "&#8657;", "&#8658;", "&#8659;", "&#8660;", "&#9674;", "&#9824;", "&#9827;", "&#9829;", "&#9830;", "&#8704;", "&#8706;", "&#8707;", "&#8709;", "&#8711;", "&#8712;", "&#8713;", "&#8715;", "&#8719;", "&#8721;", "&#8722;", "&#8727;", "&#8730;", "&#8733;", "&#8734;", "&#8736;", "&#8743;", "&#8744;", "&#8745;", "&#8746;", "&#8747;", "&#8756;", "&#8764;", "&#8773;", "&#8776;", "&#8800;", "&#8801;", "&#8804;", "&#8805;", "&#8834;", "&#8835;", "&#8836;", "&#8838;", "&#8839;", "&#8853;", "&#8855;", "&#8869;"}

        Dim ht As New Regex("&[A-Za-z]*?;", RegexOptions.ECMAScript)
        Dim htt As Match = ht.Match(s)
        If htt.Success Then
            For i = 0 To 226
                If s.Contains(html(i)) Then
                    s = s.Replace(html(i), html2(i))
                End If
            Next
        End If

        'Dim a As String = "&#12415;&#12435;&#12394;&#12398;GOLF &#12509;&#12540;&#12479;&#12502;&#12523;"
        Dim htmluni As New Regex("&#\d{1,5};", RegexOptions.ECMAScript)
        Dim uni As Match = htmluni.Match(s)
        Dim k As UInt16 = 0
        Dim b(1) As Byte
        While uni.Success
            k = Convert.ToUInt16(uni.Value.Substring(2, uni.Value.Length - 3))
            b = BitConverter.GetBytes(k)
            s = s.Replace(uni.Value, System.Text.Encoding.GetEncoding(1200).GetString(b))
            uni = uni.NextMatch
        End While

        'Dim ba As String = "&#x30D5;&#x30ED;&#x30E0;&#x30FB;&#x30BD;&#x30D5;&#x30C8;&#x30A6;&#x30A7;&#x30A2;"
        Dim htmluni2 As New Regex("&#x[0-9A-Fa-f]{1,4};", RegexOptions.ECMAScript)
        Dim uni2 As Match = htmluni2.Match(s)
        While uni2.Success
            k = Convert.ToUInt16(uni2.Value.Substring(3, uni2.Value.Length - 4), 16)
            b = BitConverter.GetBytes(k)
            s = s.Replace(uni2.Value, System.Text.Encoding.GetEncoding(1200).GetString(b))
            uni2 = uni2.NextMatch
        End While

        For i = 0 To 8
            s = s.Replace(ss(i), "")
        Next
        If s.Length > 256 Then
            s = s.Substring(0, 255)
        End If
        Return s
    End Function

    Function doskiller2(ByVal s As String) As String

        Dim ss As String() = {"/", "\", "?", "*", ":", "|", """", "<", ">"}
        'http://www.scollabo.com/banban/apply/ap8.html
        'Dim html As String() = {"&amp;", "&gt;", "&lt;"} ' "&apos;", "&quot;", "&nbsp;", "&iexcl;", "&cent;", "&pound;", "&curren;", "&yen;", "&brvbar;", "&sect;", "&uml;", "&copy;", "&ordf;", "&laquo;", "&not;", "&shy;", "&reg;", "&macr;", "&deg;", "&plusmn;", "&sup2;", "&sup3;", "&acute;", "&micro;", "&para;", "&middot;", "&cedil;", "&sup1;", "&ordm;", "&raquo;", "&frac14;", "&frac12;", "&frac34;", "&iquest;", "&Agrave;", "&Aacute;", "&Acirc;", "&Atilde;", "&Auml;", "&Aring;", "&AElig;", "&Ccedil;", "&Egrave;", "&Eacute;", "&Ecirc;", "&Euml;", "&Igrave;", "&Iacute;", "&Icirc;", "&Iuml;", "&ETH;", "&Ntilde;", "&Ograve;", "&Oacute;", "&Ocirc;", "&Otilde;", "&Ouml;", "&times;", "&Oslash;", "&Ugrave;", "&Uacute;", "&Ucirc;", "&Uuml;", "&Yacute;", "&THORN;", "&szlig;", "&agrave;", "&aacute;", "&acirc;", "&atilde;", "&auml;", "&aring;", "&aelig;", "&ccedil;", "&egrave;", "&eacute;", "&ecirc;", "&euml;", "&igrave;", "&iacute;", "&icirc;", "&iuml;", "&eth;", "&ntilde;", "&ograve;", "&oacute;", "&ocirc;", "&otilde;", "&ouml;", "&divide;", "&oslash;", "&ugrave;", "&uacute;", "&ucirc;", "&uuml;", "&yacute;", "&thorn;", "&yuml;", "&Alpha;", "&Beta;", "&Gamma;", "&Delta;", "&Epsilon;", "&Zeta;", "&Eta;", "&Theta;", "&Iota;", "&Kappa;", "&Lambda;", "&Mu;", "&Nu;", "&Xi;", "&Omicron;", "&Pi;", "&Pho;", "&Sigma;", "&Tau;", "&Upsilon;", "&Phi;", "&Chi;", "&Psi;", "&Omega;", "&alpha;", "&beta;", "&gamma;", "&delta;", "&epsilon;", "&zeta;", "&eta;", "&theta;", "&iota;", "&kappa;", "&lambda;", "&mu;", "&nu;", "&xi;", "&omicron;", "&pi;", "&rho;", "&sigmaf;", "&sigma;", "&tau;", "&upsilon;", "&phi;", "&chi;", "&psi;", "&omega;", "&ndash;", "&mdash;", "&lsquo;", "&rsquo;", "&sbquo;", "&ldquo;", "&rdquo;", "&bdquo;", "&dagger;", "&Dagger;", "&bull;", "&hellip;", "&permil;", "&prime;", "&Prime;", "&lsaquo;", "&rsaquo;", "&oline;", "&frasl;", "&euro;", "&weierp;", "&image;", "&real;", "&trade;", "&alefsym;", "&larr;", "&rarr;", "&darr;", "&harr;", "&crarr;", "&lArr;", "&uArr;", "&rArr;", "&dArr;", "&hArr;", "&loz;", "&spades;", "&clubs;", "&hearts;", "&diams;", "&forall;", "&part;", "&exist;", "&empty;", "&nabla;", "&isin;", "&notin;", "&ni;", "&prod;", "&sum;", "&minus;", "&lowast;", "&radic;", "&prop;", "&infin;", "&ang;", "&and;", "&or;", "&cap;", "&cup;", "&int;", "&there4;", "&sim;", "&cong;", "&asymp;", "&ne;", "&equiv;", "&le;", "&ge;", "&sub;", "&sup;", "&nsub;", "&sube;", "&supe;", "&oplus;", "&otimes;", "&perp;"}
        'Dim html2 As String() = {"&", "", "", "'", ""} ' "&#160;", "&#161;", "&#162;", "&#163;", "&#164;", "&#165;", "&#166;", "&#167;", "&#168;", "&#169;", "&#170;", "&#171;", "&#172;", "&#173;", "&#174;", "&#175;", "&#176;", "&#177;", "&#178;", "&#179;", "&#180;", "&#181;", "&#182;", "&#183;", "&#184;", "&#185;", "&#186;", "&#187;", "&#188;", "&#189;", "&#190;", "&#191;", "&#192;", "&#193;", "&#194;", "&#195;", "&#196;", "&#197;", "&#198;", "&#199;", "&#200;", "&#201;", "&#202;", "&#203;", "&#204;", "&#205;", "&#206;", "&#207;", "&#208;", "&#209;", "&#210;", "&#211;", "&#212;", "&#213;", "&#214;", "&#215;", "&#216;", "&#217;", "&#218;", "&#219;", "&#220;", "&#221;", "&#222;", "&#223;", "&#224;", "&#225;", "&#226;", "&#227;", "&#228;", "&#229;", "&#230;", "&#231;", "&#232;", "&#233;", "&#234;", "&#235;", "&#236;", "&#237;", "&#238;", "&#239;", "&#240;", "&#241;", "&#242;", "&#243;", "&#244;", "&#245;", "&#246;", "&#247;", "&#248;", "&#249;", "&#250;", "&#251;", "&#252;", "&#253;", "&#254;", "&#255;", "&#913;", "&#914;", "&#915;", "&#916;", "&#917;", "&#918;", "&#919;", "&#920;", "&#921;", "&#922;", "&#923;", "&#924;", "&#925;", "&#926;", "&#927;", "&#928;", "&#929;", "&#931;", "&#932;", "&#933;", "&#934;", "&#935;", "&#936;", "&#937;", "&#945;", "&#946;", "&#947;", "&#948;", "&#949;", "&#950;", "&#951;", "&#952;", "&#953;", "&#954;", "&#955;", "&#956;", "&#957;", "&#958;", "&#959;", "&#960;", "&#961;", "&#962;", "&#963;", "&#964;", "&#965;", "&#966;", "&#967;", "&#968;", "&#969;", "&#8211;", "&#8212;", "&#8216;", "&#8217;", "&#8218;", "&#8220;", "&#8221;", "&#8222;", "&#8224;", "&#8225;", "&#8226;", "&#8230;", "&#8240;", "&#8242;", "&#8243;", "&#8249;", "&#8250;", "&#8254;", "&#8260;", "&#8364;", "&#8472;", "&#8465;", "&#8476;", "&#8482;", "&#8501;", "&#8592;", "&#8594;", "&#8595;", "&#8596;", "&#8629;", "&#8656;", "&#8657;", "&#8658;", "&#8659;", "&#8660;", "&#9674;", "&#9824;", "&#9827;", "&#9829;", "&#9830;", "&#8704;", "&#8706;", "&#8707;", "&#8709;", "&#8711;", "&#8712;", "&#8713;", "&#8715;", "&#8719;", "&#8721;", "&#8722;", "&#8727;", "&#8730;", "&#8733;", "&#8734;", "&#8736;", "&#8743;", "&#8744;", "&#8745;", "&#8746;", "&#8747;", "&#8756;", "&#8764;", "&#8773;", "&#8776;", "&#8800;", "&#8801;", "&#8804;", "&#8805;", "&#8834;", "&#8835;", "&#8836;", "&#8838;", "&#8839;", "&#8853;", "&#8855;", "&#8869;"}

        'Dim ht As New Regex("&[^;]+", RegexOptions.ECMAScript)
        'Dim htt As Match = ht.Match(s)
        'While htt.Success
        If s.Contains("&") = True Then
            s = s.Replace("amp;", "")
            s = s.Replace("&", "&amp;")
        End If

        For i = 0 To 8
            s = s.Replace(ss(i), "")
        Next
        If s.Length > 256 Then
            s = s.Substring(0, 255)
        End If
        Return s
    End Function


    Private Sub disck_ver_Click(sender As System.Object, e As System.EventArgs) Handles disck_ver.Click
        If disck_ver.Checked = False Then
            disck_ver.Checked = True
            My.Settings.diskver = True
        Else
            disck_ver.Checked = False
            My.Settings.diskver = False
        End If

    End Sub

    Private Sub FILEPATHToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FILEPATH.Click
        If FILEPATH.Checked = False Then
            FILEPATH.Checked = True
            My.Settings.rename = 1 Or My.Settings.rename
        Else
            FILEPATH.Checked = False
            My.Settings.rename = 2 And My.Settings.rename
        End If
        If FILEPATH.Checked = False AndAlso MNAME.Checked = False Then
            My.Settings.rename = 0
        End If
    End Sub

    Private Sub MNAME_Click(sender As System.Object, e As System.EventArgs) Handles MNAME.Click
        If MNAME.Checked = False Then
            MNAME.Checked = True
            My.Settings.rename = 2 Or My.Settings.rename
        Else
            MNAME.Checked = False
            My.Settings.rename = 1 And My.Settings.rename
        End If
        If FILEPATH.Checked = False AndAlso MNAME.Checked = False Then
            My.Settings.rename = 0
        End If
    End Sub


    Private Sub ALSAVE_Click(sender As System.Object, e As System.EventArgs) Handles ALSAVE.Click
        If ALSAVE.Checked = False Then
            ALSAVE.Checked = True
            My.Settings.alwayssave = True
        Else
            ALSAVE.Checked = False
            My.Settings.alwayssave = False
        End If
    End Sub

    Private Sub ROMCODEs_Click(sender As System.Object, e As System.EventArgs) Handles ROMCODEs.Click

        If ROMCODEs.Checked = False Then
            ROMCODEs.Checked = True
            My.Settings.romcode = True
        Else
            ROMCODEs.Checked = False
            My.Settings.romcode = False
        End If
    End Sub

    Private Sub XMLヘッダフッター編集ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles XMLヘッダフッター編集ToolStripMenuItem.Click
        Dim f As New xmlhf
        Me.TopMost = False
        f.ShowDialog()
        f.Dispose()
        If My.Settings.topmost = True Then
            Me.TopMost = True
        End If
    End Sub

    Private Sub 差分開始インデックスToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles sabunindex.Click
        Dim f As New uid
        Me.TopMost = False
        f.ShowDialog()
        f.Dispose()
        If My.Settings.topmost = True Then
            Me.TopMost = True
        End If
    End Sub

    Private Sub pathcrc_Click(sender As System.Object, e As System.EventArgs) Handles pathcrc.Click

        If pathcrc.Checked = False Then
            pathcrc.Checked = True
            My.Settings.filenamecrc = True
        Else
            pathcrc.Checked = False
            My.Settings.filenamecrc = False
        End If
    End Sub


    Private Sub USEMD5ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles USEMD5.Click

        USEMD5.Checked = Not USEMD5.Checked
        My.Settings.usemd5 = USEMD5.Checked

    End Sub

    Private Sub ToolStripMenuItem2_Click_1(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem2.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "フォルダを指定してください。"
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        fbd.SelectedPath = My.Settings.unpackdir
        fbd.ShowNewFolderButton = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            My.Settings.unpackdir = fbd.SelectedPath & "\"
        End If
    End Sub
#End Region

    Private Sub VITASAVEDATAToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VITASAVEDATAToolStripMenuItem.Click
        Dim fbd As New FolderBrowserDialog

        '上部に表示する説明テキストを指定する
        fbd.Description = "フォルダを指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        '最初に選択するフォルダを指定する
        'RootFolder以下にあるフォルダである必要がある

        fbd.SelectedPath = "C:\"

        If Directory.Exists(My.Settings.vitasavedir) Then
            fbd.SelectedPath = My.Settings.vitasavedir
        End If

        'ユーザーが新しいフォルダを作成できるようにする
        'デフォルトでTrue
        fbd.ShowNewFolderButton = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            '選択されたフォルダを表示する
            My.Settings.vitasavedir = fbd.SelectedPath
        End If


    End Sub

End Class
