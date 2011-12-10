<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class umdisomanger
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.CRCimage = New System.Windows.Forms.Button()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.rg_add = New System.Windows.Forms.ToolStripMenuItem()
        Me.rg_edit = New System.Windows.Forms.ToolStripMenuItem()
        Me.rg_del = New System.Windows.Forms.ToolStripMenuItem()
        Me.SAVE = New System.Windows.Forms.Button()
        Me.GAMEID = New System.Windows.Forms.Button()
        Me.PFS = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.managename = New System.Windows.Forms.TextBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.movepsp = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.drivelettter = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.lockdriveletter = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.isosize = New System.Windows.Forms.TextBox()
        Me.isolba = New System.Windows.Forms.TextBox()
        Me.crc = New System.Windows.Forms.TextBox()
        Me.md5hash = New System.Windows.Forms.TextBox()
        Me.sha = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.gid = New System.Windows.Forms.TextBox()
        Me.calc_crc = New System.Windows.Forms.Button()
        Me.calc_md5 = New System.Windows.Forms.Button()
        Me.calc_sha = New System.Windows.Forms.Button()
        Me.all_hash = New System.Windows.Forms.Button()
        Me.tree_apply = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ファイルToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ADD = New System.Windows.Forms.ToolStripMenuItem()
        Me.SAVELS = New System.Windows.Forms.ToolStripMenuItem()
        Me.sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.管理名ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mane_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.mane_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.GAMEIDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gid_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.gid_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.sort_jp = New System.Windows.Forms.ToolStripMenuItem()
        Me.PSFタイトルToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.psf_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.psf_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.file_sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.file_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.file_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.設定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GUITOP = New System.Windows.Forms.ToolStripMenuItem()
        Me.xmlselect = New System.Windows.Forms.ToolStripMenuItem()
        Me.転用先リスト編集ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ヘルプToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.オンラインヘルプToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.version = New System.Windows.Forms.ToolStripMenuItem()
        Me.del_psp = New System.Windows.Forms.Button()
        Me.t_gid = New System.Windows.Forms.CheckBox()
        Me.crc_xml = New System.Windows.Forms.Button()
        Me.free = New System.Windows.Forms.Label()
        Me.ContextMenuStrip2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CRCimage
        '
        Me.CRCimage.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CRCimage.Location = New System.Drawing.Point(409, 41)
        Me.CRCimage.Name = "CRCimage"
        Me.CRCimage.Size = New System.Drawing.Size(110, 23)
        Me.CRCimage.TabIndex = 0
        Me.CRCimage.Text = "CRC画像検索"
        Me.CRCimage.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        Me.TreeView1.AllowDrop = True
        Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip2
        Me.TreeView1.LabelEdit = True
        Me.TreeView1.Location = New System.Drawing.Point(12, 41)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(276, 426)
        Me.TreeView1.TabIndex = 1
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.rg_add, Me.rg_edit, Me.rg_del})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(141, 88)
        '
        'rg_add
        '
        Me.rg_add.Name = "rg_add"
        Me.rg_add.Size = New System.Drawing.Size(140, 28)
        Me.rg_add.Text = "登録追加"
        '
        'rg_edit
        '
        Me.rg_edit.Name = "rg_edit"
        Me.rg_edit.Size = New System.Drawing.Size(140, 28)
        Me.rg_edit.Text = "登録編集"
        '
        'rg_del
        '
        Me.rg_del.Name = "rg_del"
        Me.rg_del.Size = New System.Drawing.Size(140, 28)
        Me.rg_del.Text = "登録削除"
        '
        'SAVE
        '
        Me.SAVE.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.SAVE.Location = New System.Drawing.Point(298, 41)
        Me.SAVE.Name = "SAVE"
        Me.SAVE.Size = New System.Drawing.Size(82, 23)
        Me.SAVE.TabIndex = 2
        Me.SAVE.Text = "リスト保存"
        Me.SAVE.UseVisualStyleBackColor = True
        '
        'GAMEID
        '
        Me.GAMEID.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GAMEID.Location = New System.Drawing.Point(469, 288)
        Me.GAMEID.Name = "GAMEID"
        Me.GAMEID.Size = New System.Drawing.Size(55, 23)
        Me.GAMEID.TabIndex = 3
        Me.GAMEID.Text = "取得"
        Me.GAMEID.UseVisualStyleBackColor = True
        '
        'PFS
        '
        Me.PFS.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PFS.Location = New System.Drawing.Point(562, 255)
        Me.PFS.Name = "PFS"
        Me.PFS.Size = New System.Drawing.Size(75, 23)
        Me.PFS.TabIndex = 4
        Me.PFS.Text = "PSF取得"
        Me.PFS.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(297, 69)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(104, 181)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'managename
        '
        Me.managename.Location = New System.Drawing.Point(361, 256)
        Me.managename.MaxLength = 64
        Me.managename.Name = "managename"
        Me.managename.Size = New System.Drawing.Size(195, 22)
        Me.managename.TabIndex = 6
        '
        'PictureBox2
        '
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureBox2.Location = New System.Drawing.Point(409, 70)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(328, 180)
        Me.PictureBox2.TabIndex = 7
        Me.PictureBox2.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(297, 259)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 15)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "管理名"
        '
        'movepsp
        '
        Me.movepsp.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.movepsp.Location = New System.Drawing.Point(292, 418)
        Me.movepsp.Name = "movepsp"
        Me.movepsp.Size = New System.Drawing.Size(93, 23)
        Me.movepsp.TabIndex = 9
        Me.movepsp.Text = "PSPに転送"
        Me.movepsp.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(541, 41)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(103, 23)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "GOOGLE検索"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'drivelettter
        '
        Me.drivelettter.ContextMenuStrip = Me.ContextMenuStrip1
        Me.drivelettter.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.drivelettter.Location = New System.Drawing.Point(578, 416)
        Me.drivelettter.MaxLength = 2
        Me.drivelettter.Name = "drivelettter"
        Me.drivelettter.Size = New System.Drawing.Size(29, 22)
        Me.drivelettter.TabIndex = 11
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'lockdriveletter
        '
        Me.lockdriveletter.AutoSize = True
        Me.lockdriveletter.Location = New System.Drawing.Point(423, 418)
        Me.lockdriveletter.Name = "lockdriveletter"
        Me.lockdriveletter.Size = New System.Drawing.Size(146, 19)
        Me.lockdriveletter.TabIndex = 12
        Me.lockdriveletter.Text = "ドライブレターを固定"
        Me.lockdriveletter.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(550, 292)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 15)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "ファイルサイズ;"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(553, 323)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 15)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "セクター算出;"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(297, 323)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 15)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "CRC32;"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(297, 355)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 15)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "MD5;"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(297, 389)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 15)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "SHA-1;"
        '
        'isosize
        '
        Me.isosize.ContextMenuStrip = Me.ContextMenuStrip1
        Me.isosize.Location = New System.Drawing.Point(643, 289)
        Me.isosize.MaxLength = 10
        Me.isosize.Name = "isosize"
        Me.isosize.Size = New System.Drawing.Size(94, 22)
        Me.isosize.TabIndex = 18
        '
        'isolba
        '
        Me.isolba.ContextMenuStrip = Me.ContextMenuStrip1
        Me.isolba.Location = New System.Drawing.Point(643, 320)
        Me.isolba.MaxLength = 10
        Me.isolba.Name = "isolba"
        Me.isolba.Size = New System.Drawing.Size(94, 22)
        Me.isolba.TabIndex = 19
        '
        'crc
        '
        Me.crc.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.crc.Location = New System.Drawing.Point(363, 320)
        Me.crc.MaxLength = 8
        Me.crc.Name = "crc"
        Me.crc.Size = New System.Drawing.Size(100, 22)
        Me.crc.TabIndex = 20
        '
        'md5hash
        '
        Me.md5hash.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.md5hash.Location = New System.Drawing.Point(361, 352)
        Me.md5hash.MaxLength = 32
        Me.md5hash.Name = "md5hash"
        Me.md5hash.Size = New System.Drawing.Size(211, 22)
        Me.md5hash.TabIndex = 21
        '
        'sha
        '
        Me.sha.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.sha.Location = New System.Drawing.Point(361, 386)
        Me.sha.MaxLength = 40
        Me.sha.Name = "sha"
        Me.sha.Size = New System.Drawing.Size(283, 22)
        Me.sha.TabIndex = 22
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(297, 292)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 15)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "ゲームID;"
        '
        'gid
        '
        Me.gid.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.gid.Location = New System.Drawing.Point(363, 289)
        Me.gid.MaxLength = 10
        Me.gid.Name = "gid"
        Me.gid.Size = New System.Drawing.Size(100, 22)
        Me.gid.TabIndex = 24
        '
        'calc_crc
        '
        Me.calc_crc.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.calc_crc.Location = New System.Drawing.Point(469, 319)
        Me.calc_crc.Name = "calc_crc"
        Me.calc_crc.Size = New System.Drawing.Size(54, 23)
        Me.calc_crc.TabIndex = 25
        Me.calc_crc.Text = "計算"
        Me.calc_crc.UseVisualStyleBackColor = True
        '
        'calc_md5
        '
        Me.calc_md5.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.calc_md5.Location = New System.Drawing.Point(578, 352)
        Me.calc_md5.Name = "calc_md5"
        Me.calc_md5.Size = New System.Drawing.Size(59, 23)
        Me.calc_md5.TabIndex = 26
        Me.calc_md5.Text = "計算"
        Me.calc_md5.UseVisualStyleBackColor = True
        '
        'calc_sha
        '
        Me.calc_sha.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.calc_sha.Location = New System.Drawing.Point(650, 385)
        Me.calc_sha.Name = "calc_sha"
        Me.calc_sha.Size = New System.Drawing.Size(56, 23)
        Me.calc_sha.TabIndex = 27
        Me.calc_sha.Text = "計算"
        Me.calc_sha.UseVisualStyleBackColor = True
        '
        'all_hash
        '
        Me.all_hash.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.all_hash.Location = New System.Drawing.Point(650, 352)
        Me.all_hash.Name = "all_hash"
        Me.all_hash.Size = New System.Drawing.Size(75, 23)
        Me.all_hash.TabIndex = 28
        Me.all_hash.Text = "全ハッシュ"
        Me.all_hash.UseVisualStyleBackColor = True
        '
        'tree_apply
        '
        Me.tree_apply.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.tree_apply.Location = New System.Drawing.Point(643, 418)
        Me.tree_apply.Name = "tree_apply"
        Me.tree_apply.Size = New System.Drawing.Size(94, 23)
        Me.tree_apply.TabIndex = 29
        Me.tree_apply.Text = "ツリー反映"
        Me.tree_apply.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ファイルToolStripMenuItem, Me.sort, Me.設定ToolStripMenuItem, Me.ヘルプToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(756, 31)
        Me.MenuStrip1.TabIndex = 30
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ファイルToolStripMenuItem
        '
        Me.ファイルToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ADD, Me.SAVELS})
        Me.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem"
        Me.ファイルToolStripMenuItem.Size = New System.Drawing.Size(82, 27)
        Me.ファイルToolStripMenuItem.Text = "ファイル"
        '
        'ADD
        '
        Me.ADD.Name = "ADD"
        Me.ADD.Size = New System.Drawing.Size(170, 28)
        Me.ADD.Text = "いめーじ追加"
        Me.ADD.ToolTipText = "管理したいイメージを追加します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "複数まとめて追加したい場合はエクスプローラーから選択してツリーにドロップして下さい"
        '
        'SAVELS
        '
        Me.SAVELS.Name = "SAVELS"
        Me.SAVELS.Size = New System.Drawing.Size(170, 28)
        Me.SAVELS.Text = "リスト保存"
        Me.SAVELS.ToolTipText = "編集リストを保存します"
        '
        'sort
        '
        Me.sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.管理名ToolStripMenuItem, Me.GAMEIDToolStripMenuItem, Me.PSFタイトルToolStripMenuItem, Me.file_sort})
        Me.sort.Name = "sort"
        Me.sort.Size = New System.Drawing.Size(52, 27)
        Me.sort.Text = "整列"
        '
        '管理名ToolStripMenuItem
        '
        Me.管理名ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mane_sort_up, Me.mane_sort_down})
        Me.管理名ToolStripMenuItem.Name = "管理名ToolStripMenuItem"
        Me.管理名ToolStripMenuItem.Size = New System.Drawing.Size(168, 28)
        Me.管理名ToolStripMenuItem.Text = "管理名"
        '
        'mane_sort_up
        '
        Me.mane_sort_up.Name = "mane_sort_up"
        Me.mane_sort_up.Size = New System.Drawing.Size(110, 28)
        Me.mane_sort_up.Text = "昇順"
        '
        'mane_sort_down
        '
        Me.mane_sort_down.Name = "mane_sort_down"
        Me.mane_sort_down.Size = New System.Drawing.Size(110, 28)
        Me.mane_sort_down.Text = "降順"
        '
        'GAMEIDToolStripMenuItem
        '
        Me.GAMEIDToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gid_sort_up, Me.gid_sort_down, Me.sort_jp})
        Me.GAMEIDToolStripMenuItem.Name = "GAMEIDToolStripMenuItem"
        Me.GAMEIDToolStripMenuItem.Size = New System.Drawing.Size(168, 28)
        Me.GAMEIDToolStripMenuItem.Text = "GAMEID"
        '
        'gid_sort_up
        '
        Me.gid_sort_up.Name = "gid_sort_up"
        Me.gid_sort_up.Size = New System.Drawing.Size(140, 28)
        Me.gid_sort_up.Text = "昇順"
        '
        'gid_sort_down
        '
        Me.gid_sort_down.Name = "gid_sort_down"
        Me.gid_sort_down.Size = New System.Drawing.Size(140, 28)
        Me.gid_sort_down.Text = "降順"
        '
        'sort_jp
        '
        Me.sort_jp.Name = "sort_jp"
        Me.sort_jp.Size = New System.Drawing.Size(140, 28)
        Me.sort_jp.Text = "日本優先"
        '
        'PSFタイトルToolStripMenuItem
        '
        Me.PSFタイトルToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.psf_sort_up, Me.psf_sort_down})
        Me.PSFタイトルToolStripMenuItem.Name = "PSFタイトルToolStripMenuItem"
        Me.PSFタイトルToolStripMenuItem.Size = New System.Drawing.Size(168, 28)
        Me.PSFタイトルToolStripMenuItem.Text = "PSFタイトル"
        '
        'psf_sort_up
        '
        Me.psf_sort_up.Name = "psf_sort_up"
        Me.psf_sort_up.Size = New System.Drawing.Size(110, 28)
        Me.psf_sort_up.Text = "昇順"
        '
        'psf_sort_down
        '
        Me.psf_sort_down.Name = "psf_sort_down"
        Me.psf_sort_down.Size = New System.Drawing.Size(110, 28)
        Me.psf_sort_down.Text = "降順"
        '
        'file_sort
        '
        Me.file_sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.file_sort_up, Me.file_sort_down})
        Me.file_sort.Name = "file_sort"
        Me.file_sort.Size = New System.Drawing.Size(168, 28)
        Me.file_sort.Text = "ファイル名"
        '
        'file_sort_up
        '
        Me.file_sort_up.Name = "file_sort_up"
        Me.file_sort_up.Size = New System.Drawing.Size(110, 28)
        Me.file_sort_up.Text = "昇順"
        '
        'file_sort_down
        '
        Me.file_sort_down.Name = "file_sort_down"
        Me.file_sort_down.Size = New System.Drawing.Size(110, 28)
        Me.file_sort_down.Text = "降順"
        '
        '設定ToolStripMenuItem
        '
        Me.設定ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GUITOP, Me.xmlselect, Me.転用先リスト編集ToolStripMenuItem})
        Me.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem"
        Me.設定ToolStripMenuItem.Size = New System.Drawing.Size(52, 27)
        Me.設定ToolStripMenuItem.Text = "設定"
        '
        'GUITOP
        '
        Me.GUITOP.Name = "GUITOP"
        Me.GUITOP.Size = New System.Drawing.Size(215, 28)
        Me.GUITOP.Text = "常に前面表示"
        '
        'xmlselect
        '
        Me.xmlselect.Name = "xmlselect"
        Me.xmlselect.Size = New System.Drawing.Size(215, 28)
        Me.xmlselect.Text = "画像検索用XML指定"
        Me.xmlselect.ToolTipText = "OFFLINE用XMLを選択します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CRC32と一致する管理名,画像が検索ボタンで適用されます"
        '
        '転用先リスト編集ToolStripMenuItem
        '
        Me.転用先リスト編集ToolStripMenuItem.Name = "転用先リスト編集ToolStripMenuItem"
        Me.転用先リスト編集ToolStripMenuItem.Size = New System.Drawing.Size(215, 28)
        Me.転用先リスト編集ToolStripMenuItem.Text = "転送先リスト編集"
        Me.転用先リスト編集ToolStripMenuItem.ToolTipText = "PSPに転送するディレクトリーを編集します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ツリービューの右クリックから変更できるようになります" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "//例" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "X:\PSP\GAME\"
        '
        'ヘルプToolStripMenuItem
        '
        Me.ヘルプToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.オンラインヘルプToolStripMenuItem, Me.version})
        Me.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem"
        Me.ヘルプToolStripMenuItem.Size = New System.Drawing.Size(67, 27)
        Me.ヘルプToolStripMenuItem.Text = "ヘルプ"
        '
        'オンラインヘルプToolStripMenuItem
        '
        Me.オンラインヘルプToolStripMenuItem.Name = "オンラインヘルプToolStripMenuItem"
        Me.オンラインヘルプToolStripMenuItem.Size = New System.Drawing.Size(200, 28)
        Me.オンラインヘルプToolStripMenuItem.Text = "オンラインヘルプ"
        '
        'version
        '
        Me.version.Name = "version"
        Me.version.Size = New System.Drawing.Size(200, 28)
        Me.version.Text = "バージョン"
        '
        'del_psp
        '
        Me.del_psp.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.del_psp.Location = New System.Drawing.Point(292, 447)
        Me.del_psp.Name = "del_psp"
        Me.del_psp.Size = New System.Drawing.Size(109, 23)
        Me.del_psp.TabIndex = 31
        Me.del_psp.Text = "PSPから削除"
        Me.del_psp.UseVisualStyleBackColor = True
        '
        't_gid
        '
        Me.t_gid.AutoSize = True
        Me.t_gid.Location = New System.Drawing.Point(423, 448)
        Me.t_gid.Name = "t_gid"
        Me.t_gid.Size = New System.Drawing.Size(121, 19)
        Me.t_gid.TabIndex = 32
        Me.t_gid.Text = "ゲームIDで転送"
        Me.t_gid.UseVisualStyleBackColor = True
        '
        'crc_xml
        '
        Me.crc_xml.Font = New System.Drawing.Font("MS UI Gothic", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.crc_xml.Location = New System.Drawing.Point(643, 255)
        Me.crc_xml.Name = "crc_xml"
        Me.crc_xml.Size = New System.Drawing.Size(75, 23)
        Me.crc_xml.TabIndex = 33
        Me.crc_xml.Text = "XML検索"
        Me.crc_xml.UseVisualStyleBackColor = True
        '
        'free
        '
        Me.free.AutoSize = True
        Me.free.Location = New System.Drawing.Point(581, 455)
        Me.free.Name = "free"
        Me.free.Size = New System.Drawing.Size(86, 15)
        Me.free.TabIndex = 34
        Me.free.Text = "MS空き;不明"
        '
        'umdisomanger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(756, 479)
        Me.Controls.Add(Me.free)
        Me.Controls.Add(Me.t_gid)
        Me.Controls.Add(Me.crc_xml)
        Me.Controls.Add(Me.del_psp)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.calc_sha)
        Me.Controls.Add(Me.gid)
        Me.Controls.Add(Me.md5hash)
        Me.Controls.Add(Me.tree_apply)
        Me.Controls.Add(Me.calc_md5)
        Me.Controls.Add(Me.all_hash)
        Me.Controls.Add(Me.sha)
        Me.Controls.Add(Me.crc)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.calc_crc)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lockdriveletter)
        Me.Controls.Add(Me.movepsp)
        Me.Controls.Add(Me.isosize)
        Me.Controls.Add(Me.drivelettter)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.isolba)
        Me.Controls.Add(Me.managename)
        Me.Controls.Add(Me.PFS)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.SAVE)
        Me.Controls.Add(Me.CRCimage)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GAMEID)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "umdisomanger"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UMD RAWIMAGE MANAGER"
        Me.ContextMenuStrip2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CRCimage As System.Windows.Forms.Button
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents SAVE As System.Windows.Forms.Button
    Friend WithEvents GAMEID As System.Windows.Forms.Button
    Friend WithEvents PFS As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents managename As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents movepsp As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents drivelettter As System.Windows.Forms.TextBox
    Friend WithEvents lockdriveletter As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents isosize As System.Windows.Forms.TextBox
    Friend WithEvents isolba As System.Windows.Forms.TextBox
    Friend WithEvents crc As System.Windows.Forms.TextBox
    Friend WithEvents md5hash As System.Windows.Forms.TextBox
    Friend WithEvents sha As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents gid As System.Windows.Forms.TextBox
    Friend WithEvents calc_crc As System.Windows.Forms.Button
    Friend WithEvents calc_md5 As System.Windows.Forms.Button
    Friend WithEvents calc_sha As System.Windows.Forms.Button
    Friend WithEvents all_hash As System.Windows.Forms.Button
    Friend WithEvents tree_apply As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents rg_del As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GAMEIDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gid_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 管理名ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mane_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mane_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents file_sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents file_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents file_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gid_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 設定ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents del_psp As System.Windows.Forms.Button
    Friend WithEvents ファイルToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ADD As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAVELS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GUITOP As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents xmlselect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents t_gid As System.Windows.Forms.CheckBox
    Friend WithEvents crc_xml As System.Windows.Forms.Button
    Friend WithEvents free As System.Windows.Forms.Label
    Friend WithEvents rg_add As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rg_edit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PSFタイトルToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents psf_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents psf_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 転用先リスト編集ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ヘルプToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents オンラインヘルプToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents version As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sort_jp As System.Windows.Forms.ToolStripMenuItem

End Class
