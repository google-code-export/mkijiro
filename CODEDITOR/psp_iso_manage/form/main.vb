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
            Dim head As String = ""
            Dim mask As String = ""
            Dim add As Boolean = False
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

        If File.Exists(My.Settings.xml) = False Then
            My.Settings.xml = Application.StartupPath & "\datas\ADVANsCEne_PSP.xml"
        End If

        If Directory.Exists(My.Settings.imgdir) = False Then
            My.Settings.imgdir = Application.StartupPath & "\imgs\ADVANsCEne Sony PSP Collection\"
        End If

        If My.Settings.topmost = True Then
            Me.TopMost = True
            GUITOP.Checked = True
        End If


        If My.Settings.lockdrive = True Then
            lockdriveletter.Checked = True
        End If


        If My.Settings.pspgid = True Then
            t_gid.Checked = True
        End If

        drivelettter.Text = My.Settings.drivepath

        PictureBox1.AllowDrop = True
        PictureBox2.AllowDrop = True
        AddHandler TreeView1.ItemDrag, AddressOf TreeView1_ItemDrag
        AddHandler TreeView1.DragOver, AddressOf TreeView1_DragOver
        AddHandler TreeView1.DragDrop, AddressOf TreeView1_DragDrop

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CRCimage.Click
        Try
            If File.Exists(My.Settings.xml) = True Then
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

                            Using fs As FileStream = File.OpenRead(path)
                                For Each b As Byte In crc32.ComputeHash(fs)
                                    hash += b.ToString("x2").ToLower()
                                Next
                            End Using

                            hash = hash.ToUpper
                        Else
                            MessageBox.Show(path & "が見つかりません", "エラー")
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
                        img &= st.ToString & "-" & en.ToString & "\"

                        bitmap_resize(PictureBox1, img & num.ToString & "a.png", 104, 181)
                        bitmap_resize(PictureBox2, img & num.ToString & "b.png", 382, 181)
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


    Private Sub crc_xml_Click(sender As System.Object, e As System.EventArgs) Handles crc_xml.Click

        Try
            If File.Exists(My.Settings.xml) = True Then
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

                            Using fs As FileStream = File.OpenRead(path)
                                For Each b As Byte In crc32.ComputeHash(fs)
                                    hash += b.ToString("x2").ToLower()
                                Next
                            End Using

                            hash = hash.ToUpper
                        Else
                            MessageBox.Show(path & "が見つかりません", "エラー")
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
                        MessageBox.Show("ISOと同じCRC32;" & hash & "がみつかりませんでした", "CRC不一致")
                    End If

                End If
            Else
                MessageBox.Show("検索用のoffline用XMLがみつかりません", "XMLエラー")
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
                        isosize.Text = fs.Length.ToString("0,0,0")
                        fs.Seek(&H8050, SeekOrigin.Begin)
                        fs.Read(size, 0, 4)
                        If psf.video(isopath) = "PBP" Then
                            isolba.Text = "PBPファイルです"
                        Else
                            isolba.Text = (BitConverter.ToInt32(size, 0) << 11).ToString("0,0,0")
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

                If File.Exists(My.Settings.imgdir & "1-500\1a.png") = True Then
                    'なにもないときはりっじダミー表示
                    If p1 = False AndAlso p2 = False Then
                        PictureBox1.Image = System.Drawing.Image.FromFile(My.Settings.imgdir & "1-500\1a.png")
                        PictureBox2.Image = System.Drawing.Image.FromFile(My.Settings.imgdir & "1-500\1b.png")
                    End If
                End If

                Dim psp As String = findpsp()
                If psp <> "" Then
                    Dim drive As New System.IO.DriveInfo(psp)
                    Dim ms As Single = drive.TotalFreeSpace
                    'ドライブの準備ができているか調べる
                    If drive.IsReady Then
                        If CInt(ms) - CInt(isosize.Text) > 0 Then
                            free.ForeColor = Color.Black
                        Else
                            free.ForeColor = Color.Red
                        End If

                        If ms > 1 << 30 Then
                            free.Text = "空き容量;" & (ms / (1 << 30)).ToString("N") & "GiB"
                        ElseIf ms > 1 << 20 Then
                            free.Text = "空き容量;" & (ms / (1 << 20)).ToString("N") & "MiB"
                        ElseIf ms > 1 << 10 Then
                            free.Text = "空き容量;" & (ms / (1 << 10)).ToString("N") & "KiB"
                        End If
                    End If
                Else
                    free.Text = "空き容量;不明"
                End If
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

    Private Sub 追加ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles rg_add.Click

        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
                If treenode.Level = 1 Then
                    treenode = treenode.Parent
                End If
        Dim rg As New editor
                Me.TopMost = False
                Dim treenode1 As New TreeNode
                rg.Text &= "(新規追加)"
                rg.ShowDialog()
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
                End If
                If My.Settings.topmost = True Then
                    Me.TopMost = True
                End If
                rg.Dispose()
                TreeView1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub 登録編集ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles rg_edit.Click
        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
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
                If psf.video(treenode.Tag.ToString) = "PSP" Then
                    rg.dir.Text = "X:\ISO\"
                ElseIf psf.video(treenode.Tag.ToString) = "VIDEO" Then
                    rg.dir.Text = "X:\ISO\VIDEO\"
                Else
                    rg.dir.Text = "X:\PSP\GAME\"
                End If

                Dim ss As String() = treenode.Parent.Name.Split(CChar(vbLf))
                For Each s In ss
                    If s.Contains("X:\") Then
                        rg.dir.Text = s.Trim
                    ElseIf s <> "" Then
                        rg.note.Text &= s.Trim & vbCrLf
                    End If
                Next
                rg.ShowDialog()
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
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub 削除ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rg_del.Click

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


        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub move_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles movepsp.Click
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

                If psp <> "" Then
                    Dim drive As New System.IO.DriveInfo(psp)
                    Dim ms As Single = drive.TotalFreeSpace
                    'ドライブの準備ができているか調べる
                    If drive.IsReady Then
                        If CInt(ms) - CInt(isosize.Text) > 0 Then
                            free.ForeColor = Color.Black
                        Else
                            free.ForeColor = Color.Red
                        End If

                        If ms > 1 << 30 Then
                            free.Text = "空き容量;" & (ms / (1 << 30)).ToString("N") & "GiB"
                        ElseIf ms > 1 << 20 Then
                            free.Text = "空き容量;" & (ms / (1 << 20)).ToString("N") & "MiB"
                        ElseIf ms > 1 << 10 Then
                            free.Text = "空き容量;" & (ms / (1 << 10)).ToString("N") & "KiB"
                        End If
                    End If
                Else
                    free.Text = "空き容量;不明"
                End If

            Else
                Beep()
                MessageBox.Show("PSPが見つかりません,USB接続していないかメモステフォーマット時に作成されるMEMSTICK.INDがないようです", "PSP接続エラー")
            End If
        Catch ex As Exception
            Beep()
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
                        files = System.IO.Directory.GetFiles( _
            Path.GetDirectoryName(cp2), "*", System.IO.SearchOption.AllDirectories)
                        UMD = "PBP"
                    End If

                    If psp <> "" AndAlso File.Exists(cp2) = True AndAlso MessageBox.Show("PSPからファイルを削除してもよろしいですか？", "削除の確認", _
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
                        MessageBox.Show(UMD & "を削除しました" & vbCrLf & cp2, "削除")
                    End If
                End If

                If psp <> "" Then
                    Dim drive As New System.IO.DriveInfo(psp)
                    Dim ms As Single = drive.TotalFreeSpace
                    'ドライブの準備ができているか調べる
                    If drive.IsReady Then
                        If CInt(ms) - CInt(isosize.Text) > 0 Then
                            free.ForeColor = Color.Black
                        Else
                            free.ForeColor = Color.Red
                        End If

                        If ms > 1 << 30 Then
                            free.Text = "空き容量;" & (ms / (1 << 30)).ToString("N") & "GiB"
                        ElseIf ms > 1 << 20 Then
                            free.Text = "空き容量;" & (ms / (1 << 20)).ToString("N") & "MiB"
                        ElseIf ms > 1 << 10 Then
                            free.Text = "空き容量;" & (ms / (1 << 10)).ToString("N") & "KiB"
                        End If
                    End If
                Else
                    free.Text = "空き容量;不明"
                End If

            Else
                Beep()
                MessageBox.Show("PSPが見つかりません,USB接続していないかメモステフォーマット時に作成されるMEMSTICK.INDがないようです", "PSP接続エラー")
            End If
        Catch ex As Exception
            Beep()
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
                    MessageBox.Show(ex.Message, "例外")
                End Try
            Else
                MessageBox.Show(path & "がみつかりません", "エラー")
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
                            isoinfo.Text = "MD5: " & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    md5hash.Text = hash
                    Beep()
                    TreeView1.SelectedNode = treenode.Parent
                    TreeView1.Focus()


                Catch ex As Exception
                    MessageBox.Show(ex.Message, "例外")
                End Try
            Else
                MessageBox.Show(path & "がみつかりません", "エラー")
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
                            isoinfo.Text = "SHA-1: " & hash
                            treenode.Parent.Nodes.Add(isoinfo)
                        End If
                    Next
                    sha.Text = hash
                    Beep()
                    TreeView1.SelectedNode = treenode.Parent
                    TreeView1.Focus()

                Catch ex As Exception
                    MessageBox.Show(ex.Message, "例外")
                End Try
            Else
                MessageBox.Show(path & "がみつかりません", "エラー")
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
                    MessageBox.Show(path & "がみつかりません", "エラー")
                End If
            End If
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

                Else
                    MessageBox.Show(path & "がみつかりません", "エラー")
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

                Else
                    MessageBox.Show(path & "がみつかりません", "エラー")
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

                ofd.InitialDirectory = My.Settings.imgbase
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
                    My.Settings.imgbase = Path.GetDirectoryName(ofd.FileName)
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
                ofd.InitialDirectory = My.Settings.imgbase
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
                    My.Settings.imgbase = Path.GetDirectoryName(ofd.FileName)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub xmlselect_Click(sender As System.Object, e As System.EventArgs) Handles xmlselect.Click
        Dim ofd As New OpenFileDialog()
        ofd.InitialDirectory = Application.StartupPath & "\datas"
        ofd.Filter = "xml/zipファイル(*.xml;*.zip)|*xml;*.zip"
        ofd.Title = "XML/ZIPファイルを選択してください"
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
                MessageBox.Show("OFFLINE用のXMLではありません", "不明XML")
            End If
        End If
    End Sub

    Function bitmap_resize(ByRef pc As PictureBox, ByVal path As String, ByVal width As Integer, ByVal height As Integer) As Boolean

        Dim image = New Bitmap(path)
        'PictureBox1のGraphicsオブジェクトの作成
        Dim g As Graphics = pc.CreateGraphics()
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

    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles tree_apply.Click

        Try
            Dim treenode As TreeNode = TreeView1.SelectedNode
            If treenode IsNot Nothing Then
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
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub

    Private Sub 転用先リスト編集ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles 転用先リスト編集ToolStripMenuItem.Click
        Dim l As New list
        Me.TopMost = False
        l.ShowDialog()
        If My.Settings.topmost = True Then
            Me.TopMost = True
        End If
        l.Dispose()
    End Sub

    Private Sub オンラインヘルプToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles オンラインヘルプToolStripMenuItem.Click
        Process.Start("http://unzu127xp.pa.land.to/data/UMDRAW.html")
    End Sub

    Private Sub gid_sort_up_Click(sender As System.Object, e As System.EventArgs) Handles gid_sort_up.Click
        Dim s As New sort
        s.sort_game("GID_UP")
    End Sub

    Private Sub gid_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles gid_sort_down.Click
        Dim s As New sort
        s.sort_game("GID_DW")
    End Sub

    Private Sub mane_sort_up_Click(sender As System.Object, e As System.EventArgs) Handles mane_sort_up.Click
        Dim s As New sort
        s.sort_game("GNAME_UP")
    End Sub

    Private Sub mane_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles mane_sort_down.Click
        Dim s As New sort
        s.sort_game("GNAME_DW")
    End Sub

    Private Sub psf_sort_up_Click(sender As System.Object, e As System.EventArgs) Handles psf_sort_up.Click

        Dim s As New sort
        s.sort_game("PSF_UP")
    End Sub

    Private Sub psf_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles psf_sort_down.Click

        Dim s As New sort
        s.sort_game("PSF_DW")
    End Sub

    Private Sub file_sort_down_Click(sender As System.Object, e As System.EventArgs) Handles file_sort_down.Click
        Dim s As New sort
        s.sort_game("FILE_DW")
    End Sub

    Private Sub file_sort_up_Click_1(sender As System.Object, e As System.EventArgs) Handles file_sort_up.Click

        Dim s As New sort
        s.sort_game("FILE_UP")
    End Sub

    Private Sub sort_jp_Click(sender As System.Object, e As System.EventArgs) Handles sort_jp.Click
        Dim s As New sort
        s.sort_game("GID_UP_JP")
    End Sub

End Class
