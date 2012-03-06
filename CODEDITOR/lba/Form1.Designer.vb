<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TREECOLLASEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TREEEXPANDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.EXTRACTDATAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.絶対パスで展開ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.相対パスで展開ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EXTRACTLBAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ABSOLUTPATHToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OFFSETPATHToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SECTORVIEWTREE = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.uid_seek = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RUNAPPZ = New System.Windows.Forms.ToolStripMenuItem()
        Me.SECTORVIEW = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SAVEDATA = New System.Windows.Forms.ToolStripMenuItem()
        Me.SAVEDATAOFFSET = New System.Windows.Forms.ToolStripMenuItem()
        Me.dir = New System.Windows.Forms.Label()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.discid = New System.Windows.Forms.Label()
        Me.title = New System.Windows.Forms.Label()
        Me.id_h = New System.Windows.Forms.Label()
        Me.title_h = New System.Windows.Forms.Label()
        Me.uid_parent = New System.Windows.Forms.CheckBox()
        Me.gridview = New System.Windows.Forms.CheckBox()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.localtime = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.VIRTUAL = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ファイルToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.開くToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.設定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.バージョンToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tree = New System.Windows.Forms.CheckBox()
        Me.fsbuf = New System.Windows.Forms.TextBox()
        Me.SAVEMODE = New System.Windows.Forms.Label()
        Me.sdir = New System.Windows.Forms.TextBox()
        Me.nodemax = New System.Windows.Forms.TextBox()
        Me.addlistmax = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.enc = New System.Windows.Forms.Label()
        Me.bool_exe = New System.Windows.Forms.Label()
        Me.exe = New System.Windows.Forms.TextBox()
        Me.vlistmax = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PATHTABLEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView1
        '
        Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip2
        Me.TreeView1.ImageIndex = 0
        Me.TreeView1.ImageList = Me.ImageList1
        Me.TreeView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.TreeView1.LineColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.TreeView1.Location = New System.Drawing.Point(36, 76)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.SelectedImageIndex = 0
        Me.TreeView1.Size = New System.Drawing.Size(293, 357)
        Me.TreeView1.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TREECOLLASEToolStripMenuItem, Me.TREEEXPANDToolStripMenuItem, Me.SECTORVIEWTREE, Me.ToolStripSeparator1, Me.EXTRACTDATAToolStripMenuItem, Me.EXTRACTLBAToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(194, 162)
        '
        'TREECOLLASEToolStripMenuItem
        '
        Me.TREECOLLASEToolStripMenuItem.Name = "TREECOLLASEToolStripMenuItem"
        Me.TREECOLLASEToolStripMenuItem.Size = New System.Drawing.Size(193, 26)
        Me.TREECOLLASEToolStripMenuItem.Text = "TREE COLLAPSE"
        '
        'TREEEXPANDToolStripMenuItem
        '
        Me.TREEEXPANDToolStripMenuItem.Name = "TREEEXPANDToolStripMenuItem"
        Me.TREEEXPANDToolStripMenuItem.Size = New System.Drawing.Size(193, 26)
        Me.TREEEXPANDToolStripMenuItem.Text = "TREE EXPAND"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(190, 6)
        '
        'EXTRACTDATAToolStripMenuItem
        '
        Me.EXTRACTDATAToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.絶対パスで展開ToolStripMenuItem, Me.相対パスで展開ToolStripMenuItem})
        Me.EXTRACTDATAToolStripMenuItem.Name = "EXTRACTDATAToolStripMenuItem"
        Me.EXTRACTDATAToolStripMenuItem.Size = New System.Drawing.Size(193, 26)
        Me.EXTRACTDATAToolStripMenuItem.Text = "EXTRACT DATA"
        '
        '絶対パスで展開ToolStripMenuItem
        '
        Me.絶対パスで展開ToolStripMenuItem.Name = "絶対パスで展開ToolStripMenuItem"
        Me.絶対パスで展開ToolStripMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.絶対パスで展開ToolStripMenuItem.Text = "絶対パスで展開"
        '
        '相対パスで展開ToolStripMenuItem
        '
        Me.相対パスで展開ToolStripMenuItem.Name = "相対パスで展開ToolStripMenuItem"
        Me.相対パスで展開ToolStripMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.相対パスで展開ToolStripMenuItem.Text = "相対パスで展開"
        '
        'EXTRACTLBAToolStripMenuItem
        '
        Me.EXTRACTLBAToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ABSOLUTPATHToolStripMenuItem, Me.OFFSETPATHToolStripMenuItem})
        Me.EXTRACTLBAToolStripMenuItem.Name = "EXTRACTLBAToolStripMenuItem"
        Me.EXTRACTLBAToolStripMenuItem.Size = New System.Drawing.Size(193, 26)
        Me.EXTRACTLBAToolStripMenuItem.Text = "EXTRACT LBA"
        '
        'ABSOLUTPATHToolStripMenuItem
        '
        Me.ABSOLUTPATHToolStripMenuItem.Name = "ABSOLUTPATHToolStripMenuItem"
        Me.ABSOLUTPATHToolStripMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.ABSOLUTPATHToolStripMenuItem.Text = "絶対パスで出力"
        '
        'OFFSETPATHToolStripMenuItem
        '
        Me.OFFSETPATHToolStripMenuItem.Name = "OFFSETPATHToolStripMenuItem"
        Me.OFFSETPATHToolStripMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.OFFSETPATHToolStripMenuItem.Text = "相対パスで出力"
        '
        'SECTORVIEWTREE
        '
        Me.SECTORVIEWTREE.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PATHTABLEToolStripMenuItem})
        Me.SECTORVIEWTREE.Name = "SECTORVIEWTREE"
        Me.SECTORVIEWTREE.Size = New System.Drawing.Size(193, 26)
        Me.SECTORVIEWTREE.Text = "SECTORVIEW"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "code title.png")
        Me.ImageList1.Images.SetKeyName(1, "corrupt_dir.png")
        Me.ImageList1.Images.SetKeyName(2, "file.png")
        Me.ImageList1.Images.SetKeyName(3, "corrupt_f.png")
        Me.ImageList1.Images.SetKeyName(4, "code selected.png")
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(459, 439)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(85, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "pathtable"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(33, 439)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(399, 127)
        Me.TextBox1.TabIndex = 2
        '
        'uid_seek
        '
        Me.uid_seek.Location = New System.Drawing.Point(550, 468)
        Me.uid_seek.Name = "uid_seek"
        Me.uid_seek.Size = New System.Drawing.Size(100, 21)
        Me.uid_seek.TabIndex = 3
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(459, 468)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(85, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "UID"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.AllowDrop = True
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(353, 76)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(548, 356)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.TabIndex = 5
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.VirtualMode = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RUNAPPZ, Me.SECTORVIEW, Me.ToolStripSeparator2, Me.SAVEDATA, Me.SAVEDATAOFFSET})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(224, 114)
        '
        'RUNAPPZ
        '
        Me.RUNAPPZ.Name = "RUNAPPZ"
        Me.RUNAPPZ.Size = New System.Drawing.Size(223, 26)
        Me.RUNAPPZ.Text = "RUN APP"
        '
        'SECTORVIEW
        '
        Me.SECTORVIEW.Name = "SECTORVIEW"
        Me.SECTORVIEW.Size = New System.Drawing.Size(223, 26)
        Me.SECTORVIEW.Text = "SECTORVIEW"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(220, 6)
        '
        'SAVEDATA
        '
        Me.SAVEDATA.Name = "SAVEDATA"
        Me.SAVEDATA.Size = New System.Drawing.Size(223, 26)
        Me.SAVEDATA.Text = "SAVE DATA"
        '
        'SAVEDATAOFFSET
        '
        Me.SAVEDATAOFFSET.Name = "SAVEDATAOFFSET"
        Me.SAVEDATAOFFSET.Size = New System.Drawing.Size(223, 26)
        Me.SAVEDATAOFFSET.Text = "SAVE DATA(OFFSET)"
        '
        'dir
        '
        Me.dir.AutoSize = True
        Me.dir.Location = New System.Drawing.Point(350, 44)
        Me.dir.Name = "dir"
        Me.dir.Size = New System.Drawing.Size(22, 14)
        Me.dir.TabIndex = 6
        Me.dir.Text = "dir"
        '
        'ListView2
        '
        Me.ListView2.Location = New System.Drawing.Point(378, 34)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(523, 36)
        Me.ListView2.TabIndex = 7
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.SmallIcon
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.discid)
        Me.Panel1.Controls.Add(Me.title)
        Me.Panel1.Controls.Add(Me.id_h)
        Me.Panel1.Controls.Add(Me.title_h)
        Me.Panel1.Location = New System.Drawing.Point(33, 32)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(296, 40)
        Me.Panel1.TabIndex = 10
        '
        'discid
        '
        Me.discid.AutoSize = True
        Me.discid.Location = New System.Drawing.Point(72, 24)
        Me.discid.Name = "discid"
        Me.discid.Size = New System.Drawing.Size(11, 14)
        Me.discid.TabIndex = 12
        Me.discid.Text = " "
        '
        'title
        '
        Me.title.AutoEllipsis = True
        Me.title.Location = New System.Drawing.Point(69, 3)
        Me.title.Name = "title"
        Me.title.Size = New System.Drawing.Size(224, 14)
        Me.title.TabIndex = 11
        '
        'id_h
        '
        Me.id_h.AutoSize = True
        Me.id_h.Location = New System.Drawing.Point(15, 24)
        Me.id_h.Name = "id_h"
        Me.id_h.Size = New System.Drawing.Size(51, 14)
        Me.id_h.TabIndex = 10
        Me.id_h.Text = "DISCID;"
        '
        'title_h
        '
        Me.title_h.AutoEllipsis = True
        Me.title_h.Location = New System.Drawing.Point(15, 3)
        Me.title_h.Name = "title_h"
        Me.title_h.Size = New System.Drawing.Size(45, 14)
        Me.title_h.TabIndex = 9
        Me.title_h.Text = "TITLE;"
        '
        'uid_parent
        '
        Me.uid_parent.AutoSize = True
        Me.uid_parent.Location = New System.Drawing.Point(551, 439)
        Me.uid_parent.Name = "uid_parent"
        Me.uid_parent.Size = New System.Drawing.Size(112, 18)
        Me.uid_parent.TabIndex = 11
        Me.uid_parent.Text = "[UID,PARENT]"
        Me.uid_parent.UseVisualStyleBackColor = True
        '
        'gridview
        '
        Me.gridview.AutoSize = True
        Me.gridview.Location = New System.Drawing.Point(698, 439)
        Me.gridview.Name = "gridview"
        Me.gridview.Size = New System.Drawing.Size(57, 18)
        Me.gridview.TabIndex = 12
        Me.gridview.Text = "GRID"
        Me.gridview.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(3, 13)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(57, 14)
        Me.LinkLabel1.TabIndex = 13
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "UTCWIKI"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.LinkLabel1)
        Me.Panel2.Location = New System.Drawing.Point(689, 463)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(87, 100)
        Me.Panel2.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 42)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "+9 JAPAN" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "+0 UK" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "-8 USA"
        '
        'localtime
        '
        Me.localtime.AutoSize = True
        Me.localtime.Location = New System.Drawing.Point(785, 439)
        Me.localtime.Name = "localtime"
        Me.localtime.Size = New System.Drawing.Size(103, 18)
        Me.localtime.TabIndex = 15
        Me.localtime.Text = "LOCAL TIME"
        Me.localtime.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(462, 515)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(200, 14)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "+SHIFT:START-END　SELCTION"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(462, 529)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(164, 14)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "+CTRL:MULTI SELECTION"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(462, 543)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 14)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "CTRL+A:ALL SELECTION"
        '
        'VIRTUAL
        '
        Me.VIRTUAL.AutoSize = True
        Me.VIRTUAL.Location = New System.Drawing.Point(785, 464)
        Me.VIRTUAL.Name = "VIRTUAL"
        Me.VIRTUAL.Size = New System.Drawing.Size(81, 18)
        Me.VIRTUAL.TabIndex = 19
        Me.VIRTUAL.Text = "VIRTUAL"
        Me.VIRTUAL.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(782, 555)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 14)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Label5"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Control
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ファイルToolStripMenuItem, Me.設定ToolStripMenuItem, Me.バージョンToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(914, 29)
        Me.MenuStrip1.TabIndex = 21
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ファイルToolStripMenuItem
        '
        Me.ファイルToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.開くToolStripMenuItem})
        Me.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem"
        Me.ファイルToolStripMenuItem.Size = New System.Drawing.Size(78, 25)
        Me.ファイルToolStripMenuItem.Text = "ファイル"
        '
        '開くToolStripMenuItem
        '
        Me.開くToolStripMenuItem.Name = "開くToolStripMenuItem"
        Me.開くToolStripMenuItem.Size = New System.Drawing.Size(108, 26)
        Me.開くToolStripMenuItem.Text = "開く"
        '
        '設定ToolStripMenuItem
        '
        Me.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem"
        Me.設定ToolStripMenuItem.Size = New System.Drawing.Size(50, 25)
        Me.設定ToolStripMenuItem.Text = "設定"
        '
        'バージョンToolStripMenuItem
        '
        Me.バージョンToolStripMenuItem.Name = "バージョンToolStripMenuItem"
        Me.バージョンToolStripMenuItem.Size = New System.Drawing.Size(92, 25)
        Me.バージョンToolStripMenuItem.Text = "バージョン"
        '
        'tree
        '
        Me.tree.AutoSize = True
        Me.tree.Location = New System.Drawing.Point(783, 489)
        Me.tree.Name = "tree"
        Me.tree.Size = New System.Drawing.Size(95, 18)
        Me.tree.TabIndex = 22
        Me.tree.Text = "TREEOPEN"
        Me.tree.UseVisualStyleBackColor = True
        '
        'fsbuf
        '
        Me.fsbuf.Location = New System.Drawing.Point(65, 29)
        Me.fsbuf.MaxLength = 2
        Me.fsbuf.Name = "fsbuf"
        Me.fsbuf.Size = New System.Drawing.Size(17, 21)
        Me.fsbuf.TabIndex = 23
        Me.fsbuf.Text = "0"
        Me.fsbuf.UseWaitCursor = True
        Me.fsbuf.Visible = False
        '
        'SAVEMODE
        '
        Me.SAVEMODE.AutoSize = True
        Me.SAVEMODE.Location = New System.Drawing.Point(3, 9)
        Me.SAVEMODE.Name = "SAVEMODE"
        Me.SAVEMODE.Size = New System.Drawing.Size(16, 14)
        Me.SAVEMODE.TabIndex = 24
        Me.SAVEMODE.Text = "A"
        Me.SAVEMODE.UseWaitCursor = True
        '
        'sdir
        '
        Me.sdir.Location = New System.Drawing.Point(4, 29)
        Me.sdir.Name = "sdir"
        Me.sdir.Size = New System.Drawing.Size(49, 21)
        Me.sdir.TabIndex = 25
        Me.sdir.Text = "SDIR"
        Me.sdir.UseWaitCursor = True
        '
        'nodemax
        '
        Me.nodemax.Location = New System.Drawing.Point(4, 56)
        Me.nodemax.Name = "nodemax"
        Me.nodemax.Size = New System.Drawing.Size(49, 21)
        Me.nodemax.TabIndex = 26
        Me.nodemax.Text = "50000"
        Me.nodemax.UseWaitCursor = True
        '
        'addlistmax
        '
        Me.addlistmax.Location = New System.Drawing.Point(66, 56)
        Me.addlistmax.Name = "addlistmax"
        Me.addlistmax.Size = New System.Drawing.Size(45, 21)
        Me.addlistmax.TabIndex = 27
        Me.addlistmax.Text = "3000"
        Me.addlistmax.UseWaitCursor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.enc)
        Me.Panel3.Controls.Add(Me.bool_exe)
        Me.Panel3.Controls.Add(Me.exe)
        Me.Panel3.Controls.Add(Me.vlistmax)
        Me.Panel3.Controls.Add(Me.sdir)
        Me.Panel3.Controls.Add(Me.fsbuf)
        Me.Panel3.Controls.Add(Me.nodemax)
        Me.Panel3.Controls.Add(Me.addlistmax)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.SAVEMODE)
        Me.Panel3.Location = New System.Drawing.Point(919, 35)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(123, 135)
        Me.Panel3.TabIndex = 28
        Me.Panel3.UseWaitCursor = True
        Me.Panel3.Visible = False
        '
        'enc
        '
        Me.enc.AutoSize = True
        Me.enc.Location = New System.Drawing.Point(65, 110)
        Me.enc.Name = "enc"
        Me.enc.Size = New System.Drawing.Size(14, 14)
        Me.enc.TabIndex = 32
        Me.enc.Text = "0"
        Me.enc.UseWaitCursor = True
        '
        'bool_exe
        '
        Me.bool_exe.AutoSize = True
        Me.bool_exe.Location = New System.Drawing.Point(6, 111)
        Me.bool_exe.Name = "bool_exe"
        Me.bool_exe.Size = New System.Drawing.Size(14, 14)
        Me.bool_exe.TabIndex = 31
        Me.bool_exe.Text = "0"
        Me.bool_exe.UseWaitCursor = True
        '
        'exe
        '
        Me.exe.Location = New System.Drawing.Point(66, 82)
        Me.exe.Name = "exe"
        Me.exe.Size = New System.Drawing.Size(45, 21)
        Me.exe.TabIndex = 30
        Me.exe.Text = "EXE"
        Me.exe.UseWaitCursor = True
        '
        'vlistmax
        '
        Me.vlistmax.Location = New System.Drawing.Point(4, 83)
        Me.vlistmax.Name = "vlistmax"
        Me.vlistmax.Size = New System.Drawing.Size(49, 21)
        Me.vlistmax.TabIndex = 28
        Me.vlistmax.Text = "50000"
        Me.vlistmax.UseWaitCursor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(62, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 14)
        Me.Label6.TabIndex = 29
        Me.Label6.Text = "Label6"
        Me.Label6.UseWaitCursor = True
        Me.Label6.Visible = False
        '
        'Timer1
        '
        '
        'PATHTABLEToolStripMenuItem
        '
        Me.PATHTABLEToolStripMenuItem.Name = "PATHTABLEToolStripMenuItem"
        Me.PATHTABLEToolStripMenuItem.Size = New System.Drawing.Size(158, 26)
        Me.PATHTABLEToolStripMenuItem.Text = "PATHTABLE"
        Me.PATHTABLEToolStripMenuItem.Visible = False
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(914, 578)
        Me.Controls.Add(Me.tree)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.VIRTUAL)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.localtime)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.gridview)
        Me.Controls.Add(Me.uid_parent)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.dir)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.uid_seek)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximumSize = New System.Drawing.Size(1500, 621)
        Me.MinimumSize = New System.Drawing.Size(932, 621)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DROP_IMAGE"
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents uid_seek As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents dir As System.Windows.Forms.Label
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SAVEDATA As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents TREECOLLASEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TREEEXPANDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents discid As System.Windows.Forms.Label
    Friend WithEvents title As System.Windows.Forms.Label
    Friend WithEvents id_h As System.Windows.Forms.Label
    Friend WithEvents title_h As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EXTRACTDATAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uid_parent As System.Windows.Forms.CheckBox
    Friend WithEvents gridview As System.Windows.Forms.CheckBox
    Friend WithEvents EXTRACTLBAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ABSOLUTPATHToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OFFSETPATHToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents localtime As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents VIRTUAL As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ファイルToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 開くToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 設定ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents バージョンToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tree As System.Windows.Forms.CheckBox
    Friend WithEvents fsbuf As System.Windows.Forms.TextBox
    Friend WithEvents SAVEMODE As System.Windows.Forms.Label
    Friend WithEvents 絶対パスで展開ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 相対パスで展開ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAVEDATAOFFSET As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sdir As System.Windows.Forms.TextBox
    Friend WithEvents nodemax As System.Windows.Forms.TextBox
    Friend WithEvents addlistmax As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents vlistmax As System.Windows.Forms.TextBox
    Friend WithEvents RUNAPPZ As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents exe As System.Windows.Forms.TextBox
    Friend WithEvents bool_exe As System.Windows.Forms.Label
    Friend WithEvents SECTORVIEW As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents enc As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SECTORVIEWTREE As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PATHTABLEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
