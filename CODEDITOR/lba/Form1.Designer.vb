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
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.GETDATAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.TREECOLLASEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TREEEXPANDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ListView3 = New System.Windows.Forms.ListView()
        Me.EXTRACTDATAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.Panel1.SuspendLayout()
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
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "code title.png")
        Me.ImageList1.Images.SetKeyName(1, "code selected.png")
        Me.ImageList1.Images.SetKeyName(2, "file.png")
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
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(550, 468)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 21)
        Me.TextBox2.TabIndex = 3
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
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.Location = New System.Drawing.Point(353, 76)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(535, 356)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView1.TabIndex = 5
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GETDATAToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(158, 30)
        '
        'GETDATAToolStripMenuItem
        '
        Me.GETDATAToolStripMenuItem.Name = "GETDATAToolStripMenuItem"
        Me.GETDATAToolStripMenuItem.Size = New System.Drawing.Size(157, 26)
        Me.GETDATAToolStripMenuItem.Text = "SAVE DATA"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(365, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 14)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "dir"
        '
        'ListView2
        '
        Me.ListView2.Location = New System.Drawing.Point(393, 34)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(495, 36)
        Me.ListView2.TabIndex = 7
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.SmallIcon
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TREECOLLASEToolStripMenuItem, Me.TREEEXPANDToolStripMenuItem, Me.ToolStripSeparator1, Me.EXTRACTDATAToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(194, 110)
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(33, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(296, 68)
        Me.Panel1.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 14)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "TITLE;"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 14)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "DISCID;"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(69, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(11, 14)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = " "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(72, 36)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(11, 14)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = " "
        '
        'ListView3
        '
        Me.ListView3.Location = New System.Drawing.Point(751, 457)
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Size = New System.Drawing.Size(121, 97)
        Me.ListView3.TabIndex = 11
        Me.ListView3.UseCompatibleStateImageBehavior = False
        Me.ListView3.Visible = False
        '
        'EXTRACTDATAToolStripMenuItem
        '
        Me.EXTRACTDATAToolStripMenuItem.Name = "EXTRACTDATAToolStripMenuItem"
        Me.EXTRACTDATAToolStripMenuItem.Size = New System.Drawing.Size(193, 26)
        Me.EXTRACTDATAToolStripMenuItem.Text = "EXTRACT　DATA"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(190, 6)
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(901, 578)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ListView3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.TextBox2)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DROP_IMAGE"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents GETDATAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents TREECOLLASEToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TREEEXPANDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ListView3 As System.Windows.Forms.ListView
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EXTRACTDATAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
