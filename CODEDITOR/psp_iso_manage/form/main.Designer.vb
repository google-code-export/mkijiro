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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(umdisomanger))
        Me.CRCimage = New System.Windows.Forms.Button()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.rg_add = New System.Windows.Forms.ToolStripMenuItem()
        Me.rg_edit = New System.Windows.Forms.ToolStripMenuItem()
        Me.rg_del = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.VIEW = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.menufile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ADD = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SAVELS = New System.Windows.Forms.ToolStripMenuItem()
        Me.EXPORTPSPINS = New System.Windows.Forms.ToolStripMenuItem()
        Me.SAVE_clrmamepro = New System.Windows.Forms.ToolStripMenuItem()
        Me.disck_ver = New System.Windows.Forms.ToolStripMenuItem()
        Me.ROMCODEs = New System.Windows.Forms.ToolStripMenuItem()
        Me.exportxml = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.diffCMPRO = New System.Windows.Forms.ToolStripMenuItem()
        Me.diffXML = New System.Windows.Forms.ToolStripMenuItem()
        Me.sabunindex = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CLOSEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.name_sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.mane_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.mane_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.GAMEID_sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.gid_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.gid_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.sort_jp = New System.Windows.Forms.ToolStripMenuItem()
        Me.PpriorUSAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PriorEUToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PSFtitle_sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.psf_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.psf_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.file_sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.file_sort_up = New System.Windows.Forms.ToolStripMenuItem()
        Me.file_sort_down = New System.Windows.Forms.ToolStripMenuItem()
        Me.RENAME = New System.Windows.Forms.ToolStripMenuItem()
        Me.RMXML = New System.Windows.Forms.ToolStripMenuItem()
        Me.RMDAT = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MNEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.MNAME = New System.Windows.Forms.ToolStripMenuItem()
        Me.FILEPATH = New System.Windows.Forms.ToolStripMenuItem()
        Me.setting = New System.Windows.Forms.ToolStripMenuItem()
        Me.GUITOP = New System.Windows.Forms.ToolStripMenuItem()
        Me.xmlselect = New System.Windows.Forms.ToolStripMenuItem()
        Me.rename_dat = New System.Windows.Forms.ToolStripMenuItem()
        Me.editpspdir = New System.Windows.Forms.ToolStripMenuItem()
        Me.XMLヘッダフッター編集ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ALSAVE = New System.Windows.Forms.ToolStripMenuItem()
        Me.HELPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.online = New System.Windows.Forms.ToolStripMenuItem()
        Me.version = New System.Windows.Forms.ToolStripMenuItem()
        Me.del_psp = New System.Windows.Forms.Button()
        Me.t_gid = New System.Windows.Forms.CheckBox()
        Me.crc_xml = New System.Windows.Forms.Button()
        Me.free = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SAVELS2 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CRCimage
        '
        resources.ApplyResources(Me.CRCimage, "CRCimage")
        Me.CRCimage.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.CRCimage.Name = "CRCimage"
        Me.CRCimage.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        resources.ApplyResources(Me.TreeView1, "TreeView1")
        Me.TreeView1.AllowDrop = True
        Me.TreeView1.ContextMenuStrip = Me.ContextMenuStrip2
        Me.TreeView1.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.TreeView1.LabelEdit = True
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.SelectedImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        '
        'ContextMenuStrip2
        '
        resources.ApplyResources(Me.ContextMenuStrip2, "ContextMenuStrip2")
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.rg_add, Me.rg_edit, Me.rg_del, Me.ToolStripSeparator5, Me.VIEW})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        '
        'rg_add
        '
        resources.ApplyResources(Me.rg_add, "rg_add")
        Me.rg_add.Name = "rg_add"
        '
        'rg_edit
        '
        resources.ApplyResources(Me.rg_edit, "rg_edit")
        Me.rg_edit.Name = "rg_edit"
        '
        'rg_del
        '
        resources.ApplyResources(Me.rg_del, "rg_del")
        Me.rg_del.Name = "rg_del"
        '
        'ToolStripSeparator5
        '
        resources.ApplyResources(Me.ToolStripSeparator5, "ToolStripSeparator5")
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        '
        'VIEW
        '
        resources.ApplyResources(Me.VIEW, "VIEW")
        Me.VIEW.Name = "VIEW"
        '
        'GAMEID
        '
        resources.ApplyResources(Me.GAMEID, "GAMEID")
        Me.GAMEID.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.GAMEID.Name = "GAMEID"
        Me.GAMEID.UseVisualStyleBackColor = True
        '
        'PFS
        '
        resources.ApplyResources(Me.PFS, "PFS")
        Me.PFS.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.PFS.Name = "PFS"
        Me.PFS.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'managename
        '
        resources.ApplyResources(Me.managename, "managename")
        Me.managename.Name = "managename"
        '
        'PictureBox2
        '
        resources.ApplyResources(Me.PictureBox2, "PictureBox2")
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.TabStop = False
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label1.Name = "Label1"
        '
        'movepsp
        '
        resources.ApplyResources(Me.movepsp, "movepsp")
        Me.movepsp.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.movepsp.Name = "movepsp"
        Me.movepsp.UseVisualStyleBackColor = True
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'drivelettter
        '
        resources.ApplyResources(Me.drivelettter, "drivelettter")
        Me.drivelettter.ContextMenuStrip = Me.ContextMenuStrip1
        Me.drivelettter.Name = "drivelettter"
        '
        'ContextMenuStrip1
        '
        resources.ApplyResources(Me.ContextMenuStrip1, "ContextMenuStrip1")
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        '
        'lockdriveletter
        '
        resources.ApplyResources(Me.lockdriveletter, "lockdriveletter")
        Me.lockdriveletter.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.lockdriveletter.Name = "lockdriveletter"
        Me.lockdriveletter.UseVisualStyleBackColor = True
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label5.Name = "Label5"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label6.Name = "Label6"
        '
        'isosize
        '
        resources.ApplyResources(Me.isosize, "isosize")
        Me.isosize.ContextMenuStrip = Me.ContextMenuStrip1
        Me.isosize.Name = "isosize"
        '
        'isolba
        '
        resources.ApplyResources(Me.isolba, "isolba")
        Me.isolba.ContextMenuStrip = Me.ContextMenuStrip1
        Me.isolba.Name = "isolba"
        '
        'crc
        '
        resources.ApplyResources(Me.crc, "crc")
        Me.crc.Name = "crc"
        '
        'md5hash
        '
        resources.ApplyResources(Me.md5hash, "md5hash")
        Me.md5hash.Name = "md5hash"
        '
        'sha
        '
        resources.ApplyResources(Me.sha, "sha")
        Me.sha.Name = "sha"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Label7.Name = "Label7"
        '
        'gid
        '
        resources.ApplyResources(Me.gid, "gid")
        Me.gid.Name = "gid"
        '
        'calc_crc
        '
        resources.ApplyResources(Me.calc_crc, "calc_crc")
        Me.calc_crc.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.calc_crc.Name = "calc_crc"
        Me.calc_crc.UseVisualStyleBackColor = True
        '
        'calc_md5
        '
        resources.ApplyResources(Me.calc_md5, "calc_md5")
        Me.calc_md5.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.calc_md5.Name = "calc_md5"
        Me.calc_md5.UseVisualStyleBackColor = True
        '
        'calc_sha
        '
        resources.ApplyResources(Me.calc_sha, "calc_sha")
        Me.calc_sha.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.calc_sha.Name = "calc_sha"
        Me.calc_sha.UseVisualStyleBackColor = True
        '
        'all_hash
        '
        resources.ApplyResources(Me.all_hash, "all_hash")
        Me.all_hash.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.all_hash.Name = "all_hash"
        Me.all_hash.UseVisualStyleBackColor = True
        '
        'tree_apply
        '
        resources.ApplyResources(Me.tree_apply, "tree_apply")
        Me.tree_apply.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.tree_apply.Name = "tree_apply"
        Me.tree_apply.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        resources.ApplyResources(Me.MenuStrip1, "MenuStrip1")
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menufile, Me.sort, Me.RENAME, Me.setting, Me.HELPToolStripMenuItem})
        Me.MenuStrip1.Name = "MenuStrip1"
        '
        'menufile
        '
        resources.ApplyResources(Me.menufile, "menufile")
        Me.menufile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ADD, Me.ToolStripSeparator2, Me.SAVELS, Me.EXPORTPSPINS, Me.SAVE_clrmamepro, Me.exportxml, Me.ToolStripMenuItem1, Me.ToolStripSeparator3, Me.CLOSEToolStripMenuItem})
        Me.menufile.Name = "menufile"
        '
        'ADD
        '
        resources.ApplyResources(Me.ADD, "ADD")
        Me.ADD.Name = "ADD"
        '
        'ToolStripSeparator2
        '
        resources.ApplyResources(Me.ToolStripSeparator2, "ToolStripSeparator2")
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        '
        'SAVELS
        '
        resources.ApplyResources(Me.SAVELS, "SAVELS")
        Me.SAVELS.Name = "SAVELS"
        '
        'EXPORTPSPINS
        '
        resources.ApplyResources(Me.EXPORTPSPINS, "EXPORTPSPINS")
        Me.EXPORTPSPINS.Name = "EXPORTPSPINS"
        '
        'SAVE_clrmamepro
        '
        resources.ApplyResources(Me.SAVE_clrmamepro, "SAVE_clrmamepro")
        Me.SAVE_clrmamepro.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.disck_ver, Me.ROMCODEs})
        Me.SAVE_clrmamepro.Name = "SAVE_clrmamepro"
        '
        'disck_ver
        '
        resources.ApplyResources(Me.disck_ver, "disck_ver")
        Me.disck_ver.Name = "disck_ver"
        '
        'ROMCODEs
        '
        resources.ApplyResources(Me.ROMCODEs, "ROMCODEs")
        Me.ROMCODEs.Name = "ROMCODEs"
        '
        'exportxml
        '
        resources.ApplyResources(Me.exportxml, "exportxml")
        Me.exportxml.Name = "exportxml"
        '
        'ToolStripMenuItem1
        '
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.diffCMPRO, Me.diffXML})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        '
        'diffCMPRO
        '
        resources.ApplyResources(Me.diffCMPRO, "diffCMPRO")
        Me.diffCMPRO.Name = "diffCMPRO"
        '
        'diffXML
        '
        resources.ApplyResources(Me.diffXML, "diffXML")
        Me.diffXML.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sabunindex})
        Me.diffXML.Name = "diffXML"
        '
        'sabunindex
        '
        resources.ApplyResources(Me.sabunindex, "sabunindex")
        Me.sabunindex.Name = "sabunindex"
        '
        'ToolStripSeparator3
        '
        resources.ApplyResources(Me.ToolStripSeparator3, "ToolStripSeparator3")
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        '
        'CLOSEToolStripMenuItem
        '
        resources.ApplyResources(Me.CLOSEToolStripMenuItem, "CLOSEToolStripMenuItem")
        Me.CLOSEToolStripMenuItem.Name = "CLOSEToolStripMenuItem"
        '
        'sort
        '
        resources.ApplyResources(Me.sort, "sort")
        Me.sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.name_sort, Me.GAMEID_sort, Me.PSFtitle_sort, Me.file_sort})
        Me.sort.Name = "sort"
        '
        'name_sort
        '
        resources.ApplyResources(Me.name_sort, "name_sort")
        Me.name_sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mane_sort_up, Me.mane_sort_down})
        Me.name_sort.Name = "name_sort"
        '
        'mane_sort_up
        '
        resources.ApplyResources(Me.mane_sort_up, "mane_sort_up")
        Me.mane_sort_up.Name = "mane_sort_up"
        '
        'mane_sort_down
        '
        resources.ApplyResources(Me.mane_sort_down, "mane_sort_down")
        Me.mane_sort_down.Name = "mane_sort_down"
        '
        'GAMEID_sort
        '
        resources.ApplyResources(Me.GAMEID_sort, "GAMEID_sort")
        Me.GAMEID_sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.gid_sort_up, Me.gid_sort_down, Me.ToolStripSeparator1, Me.sort_jp, Me.PpriorUSAToolStripMenuItem, Me.PriorEUToolStripMenuItem})
        Me.GAMEID_sort.Name = "GAMEID_sort"
        '
        'gid_sort_up
        '
        resources.ApplyResources(Me.gid_sort_up, "gid_sort_up")
        Me.gid_sort_up.Name = "gid_sort_up"
        '
        'gid_sort_down
        '
        resources.ApplyResources(Me.gid_sort_down, "gid_sort_down")
        Me.gid_sort_down.Name = "gid_sort_down"
        '
        'ToolStripSeparator1
        '
        resources.ApplyResources(Me.ToolStripSeparator1, "ToolStripSeparator1")
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        '
        'sort_jp
        '
        resources.ApplyResources(Me.sort_jp, "sort_jp")
        Me.sort_jp.Name = "sort_jp"
        '
        'PpriorUSAToolStripMenuItem
        '
        resources.ApplyResources(Me.PpriorUSAToolStripMenuItem, "PpriorUSAToolStripMenuItem")
        Me.PpriorUSAToolStripMenuItem.Name = "PpriorUSAToolStripMenuItem"
        '
        'PriorEUToolStripMenuItem
        '
        resources.ApplyResources(Me.PriorEUToolStripMenuItem, "PriorEUToolStripMenuItem")
        Me.PriorEUToolStripMenuItem.Name = "PriorEUToolStripMenuItem"
        '
        'PSFtitle_sort
        '
        resources.ApplyResources(Me.PSFtitle_sort, "PSFtitle_sort")
        Me.PSFtitle_sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.psf_sort_up, Me.psf_sort_down})
        Me.PSFtitle_sort.Name = "PSFtitle_sort"
        '
        'psf_sort_up
        '
        resources.ApplyResources(Me.psf_sort_up, "psf_sort_up")
        Me.psf_sort_up.Name = "psf_sort_up"
        '
        'psf_sort_down
        '
        resources.ApplyResources(Me.psf_sort_down, "psf_sort_down")
        Me.psf_sort_down.Name = "psf_sort_down"
        '
        'file_sort
        '
        resources.ApplyResources(Me.file_sort, "file_sort")
        Me.file_sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.file_sort_up, Me.file_sort_down})
        Me.file_sort.Name = "file_sort"
        '
        'file_sort_up
        '
        resources.ApplyResources(Me.file_sort_up, "file_sort_up")
        Me.file_sort_up.Name = "file_sort_up"
        '
        'file_sort_down
        '
        resources.ApplyResources(Me.file_sort_down, "file_sort_down")
        Me.file_sort_down.Name = "file_sort_down"
        '
        'RENAME
        '
        resources.ApplyResources(Me.RENAME, "RENAME")
        Me.RENAME.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RMXML, Me.RMDAT, Me.ToolStripSeparator4, Me.MNEN})
        Me.RENAME.Name = "RENAME"
        '
        'RMXML
        '
        resources.ApplyResources(Me.RMXML, "RMXML")
        Me.RMXML.Name = "RMXML"
        '
        'RMDAT
        '
        resources.ApplyResources(Me.RMDAT, "RMDAT")
        Me.RMDAT.Name = "RMDAT"
        '
        'ToolStripSeparator4
        '
        resources.ApplyResources(Me.ToolStripSeparator4, "ToolStripSeparator4")
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        '
        'MNEN
        '
        resources.ApplyResources(Me.MNEN, "MNEN")
        Me.MNEN.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MNAME, Me.FILEPATH})
        Me.MNEN.Name = "MNEN"
        '
        'MNAME
        '
        resources.ApplyResources(Me.MNAME, "MNAME")
        Me.MNAME.Name = "MNAME"
        '
        'FILEPATH
        '
        resources.ApplyResources(Me.FILEPATH, "FILEPATH")
        Me.FILEPATH.Name = "FILEPATH"
        '
        'setting
        '
        resources.ApplyResources(Me.setting, "setting")
        Me.setting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GUITOP, Me.xmlselect, Me.rename_dat, Me.editpspdir, Me.XMLヘッダフッター編集ToolStripMenuItem, Me.ALSAVE})
        Me.setting.Name = "setting"
        '
        'GUITOP
        '
        resources.ApplyResources(Me.GUITOP, "GUITOP")
        Me.GUITOP.Name = "GUITOP"
        '
        'xmlselect
        '
        resources.ApplyResources(Me.xmlselect, "xmlselect")
        Me.xmlselect.Name = "xmlselect"
        '
        'rename_dat
        '
        resources.ApplyResources(Me.rename_dat, "rename_dat")
        Me.rename_dat.Name = "rename_dat"
        '
        'editpspdir
        '
        resources.ApplyResources(Me.editpspdir, "editpspdir")
        Me.editpspdir.Name = "editpspdir"
        '
        'XMLヘッダフッター編集ToolStripMenuItem
        '
        resources.ApplyResources(Me.XMLヘッダフッター編集ToolStripMenuItem, "XMLヘッダフッター編集ToolStripMenuItem")
        Me.XMLヘッダフッター編集ToolStripMenuItem.Name = "XMLヘッダフッター編集ToolStripMenuItem"
        '
        'ALSAVE
        '
        resources.ApplyResources(Me.ALSAVE, "ALSAVE")
        Me.ALSAVE.Name = "ALSAVE"
        '
        'HELPToolStripMenuItem
        '
        resources.ApplyResources(Me.HELPToolStripMenuItem, "HELPToolStripMenuItem")
        Me.HELPToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.online, Me.version})
        Me.HELPToolStripMenuItem.Name = "HELPToolStripMenuItem"
        '
        'online
        '
        resources.ApplyResources(Me.online, "online")
        Me.online.Name = "online"
        '
        'version
        '
        resources.ApplyResources(Me.version, "version")
        Me.version.Name = "version"
        '
        'del_psp
        '
        resources.ApplyResources(Me.del_psp, "del_psp")
        Me.del_psp.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.del_psp.Name = "del_psp"
        Me.del_psp.UseVisualStyleBackColor = True
        '
        't_gid
        '
        resources.ApplyResources(Me.t_gid, "t_gid")
        Me.t_gid.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.t_gid.Name = "t_gid"
        Me.t_gid.UseVisualStyleBackColor = True
        '
        'crc_xml
        '
        resources.ApplyResources(Me.crc_xml, "crc_xml")
        Me.crc_xml.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.crc_xml.Name = "crc_xml"
        Me.crc_xml.UseVisualStyleBackColor = True
        '
        'free
        '
        resources.ApplyResources(Me.free, "free")
        Me.free.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.free.Name = "free"
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.Button2.Name = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'SAVELS2
        '
        resources.ApplyResources(Me.SAVELS2, "SAVELS2")
        Me.SAVELS2.ImageKey = Global.umd_rawimage_manger.My.Resources.Resources.xmlfoot
        Me.SAVELS2.Name = "SAVELS2"
        Me.SAVELS2.UseVisualStyleBackColor = True
        '
        'umdisomanger
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SAVELS2)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.free)
        Me.Controls.Add(Me.gid)
        Me.Controls.Add(Me.t_gid)
        Me.Controls.Add(Me.del_psp)
        Me.Controls.Add(Me.crc_xml)
        Me.Controls.Add(Me.md5hash)
        Me.Controls.Add(Me.calc_sha)
        Me.Controls.Add(Me.crc)
        Me.Controls.Add(Me.calc_md5)
        Me.Controls.Add(Me.sha)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tree_apply)
        Me.Controls.Add(Me.calc_crc)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lockdriveletter)
        Me.Controls.Add(Me.drivelettter)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.all_hash)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.movepsp)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.isolba)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.CRCimage)
        Me.Controls.Add(Me.isosize)
        Me.Controls.Add(Me.PFS)
        Me.Controls.Add(Me.managename)
        Me.Controls.Add(Me.GAMEID)
        Me.Controls.Add(Me.Label1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "umdisomanger"
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
    Friend WithEvents GAMEID_sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gid_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents name_sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mane_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mane_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents file_sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents file_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents file_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gid_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents setting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents del_psp As System.Windows.Forms.Button
    Friend WithEvents menufile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ADD As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAVELS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GUITOP As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents xmlselect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents t_gid As System.Windows.Forms.CheckBox
    Friend WithEvents crc_xml As System.Windows.Forms.Button
    Friend WithEvents free As System.Windows.Forms.Label
    Friend WithEvents rg_add As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rg_edit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PSFtitle_sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents psf_sort_up As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents psf_sort_down As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents editpspdir As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sort_jp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HELPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents online As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents version As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EXPORTPSPINS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CLOSEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PpriorUSAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PriorEUToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents RENAME As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RMXML As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RMDAT As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rename_dat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MNEN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FILEPATH As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MNAME As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAVE_clrmamepro As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAVELS2 As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents VIEW As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents disck_ver As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ALSAVE As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ROMCODEs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents diffCMPRO As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents diffXML As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exportxml As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents XMLヘッダフッター編集ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sabunindex As System.Windows.Forms.ToolStripMenuItem

End Class
