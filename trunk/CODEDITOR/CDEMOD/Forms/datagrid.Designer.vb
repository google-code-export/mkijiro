<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class datagrid
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.CNVbikou = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.addline = New System.Windows.Forms.ToolStripMenuItem()
        Me.addmacro = New System.Windows.Forms.ToolStripMenuItem()
        Me.cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.moveup = New System.Windows.Forms.ToolStripMenuItem()
        Me.movedown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.notetag = New System.Windows.Forms.ToolStripMenuItem()
        Me.appy = New System.Windows.Forms.ToolStripMenuItem()
        Me.APPLY = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridsave = New System.Windows.Forms.CheckBox()
        Me.g_address = New System.Windows.Forms.RadioButton()
        Me.g_value = New System.Windows.Forms.RadioButton()
        Me.アドレス = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.編集タイプ = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.入力値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.備考 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CNVbikou.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.アドレス, Me.値, Me.編集タイプ, Me.入力値, Me.備考})
        Me.DataGridView1.ContextMenuStrip = Me.CNVbikou
        Me.DataGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DataGridView1.Location = New System.Drawing.Point(4, 49)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 21
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(591, 300)
        Me.DataGridView1.TabIndex = 0
        '
        'CNVbikou
        '
        Me.CNVbikou.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.addline, Me.cut, Me.copy, Me.paste, Me.ToolStripSeparator1, Me.moveup, Me.movedown, Me.ToolStripSeparator2, Me.notetag, Me.appy})
        Me.CNVbikou.Name = "CNVbikou"
        Me.CNVbikou.Size = New System.Drawing.Size(180, 240)
        Me.CNVbikou.Text = "備考変換"
        '
        'addline
        '
        Me.addline.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.addmacro})
        Me.addline.Name = "addline"
        Me.addline.Size = New System.Drawing.Size(179, 28)
        Me.addline.Text = "1行コード追加"
        '
        'addmacro
        '
        Me.addmacro.Enabled = False
        Me.addmacro.Name = "addmacro"
        Me.addmacro.Size = New System.Drawing.Size(200, 28)
        Me.addmacro.Text = "コードマクロ挿入"
        '
        'cut
        '
        Me.cut.Name = "cut"
        Me.cut.Size = New System.Drawing.Size(179, 28)
        Me.cut.Text = "カット"
        '
        'copy
        '
        Me.copy.Name = "copy"
        Me.copy.Size = New System.Drawing.Size(179, 28)
        Me.copy.Text = "コピー"
        '
        'paste
        '
        Me.paste.Name = "paste"
        Me.paste.Size = New System.Drawing.Size(179, 28)
        Me.paste.Text = "貼付け"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(176, 6)
        '
        'moveup
        '
        Me.moveup.Name = "moveup"
        Me.moveup.Size = New System.Drawing.Size(179, 28)
        Me.moveup.Text = "上に移動"
        Me.moveup.ToolTipText = "コードを1行上に移動します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CTRL押しながらメニューを表示すると☆マークが付き" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "一番上まで一気に移動します"
        '
        'movedown
        '
        Me.movedown.Name = "movedown"
        Me.movedown.Size = New System.Drawing.Size(179, 28)
        Me.movedown.Text = "下に移動"
        Me.movedown.ToolTipText = "コードを1行下に移動します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CTRL押しながらメニューを表示すると☆マークが付き" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "一番下まで一気に移動します"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(176, 6)
        '
        'notetag
        '
        Me.notetag.Name = "notetag"
        Me.notetag.Size = New System.Drawing.Size(179, 28)
        Me.notetag.Text = "備考タグ変換"
        Me.notetag.ToolTipText = "CWCコード横の説明部分/FREECHEATの_N2、SCMをタグに変換します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "//CWC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "_L 0x... 0x....(説明)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "//FREECHEA" & _
    "T" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "_N2 (説明)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "_L 0x... 0x...." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "$SCM{" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "$(説明)$2 $ (0123...)"
        '
        'appy
        '
        Me.appy.Name = "appy"
        Me.appy.Size = New System.Drawing.Size(179, 28)
        Me.appy.Text = "適用"
        '
        'APPLY
        '
        Me.APPLY.Location = New System.Drawing.Point(348, 9)
        Me.APPLY.Margin = New System.Windows.Forms.Padding(4)
        Me.APPLY.Name = "APPLY"
        Me.APPLY.Size = New System.Drawing.Size(100, 29)
        Me.APPLY.TabIndex = 2
        Me.APPLY.Text = "適用"
        Me.APPLY.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 16)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(12, 15)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = " "
        '
        'gridsave
        '
        Me.gridsave.AutoSize = True
        Me.gridsave.Location = New System.Drawing.Point(455, 15)
        Me.gridsave.Name = "gridsave"
        Me.gridsave.Size = New System.Drawing.Size(140, 19)
        Me.gridsave.TabIndex = 4
        Me.gridsave.Text = "適用と同時に保存"
        Me.gridsave.UseVisualStyleBackColor = True
        '
        'g_address
        '
        Me.g_address.AutoSize = True
        Me.g_address.Location = New System.Drawing.Point(203, 15)
        Me.g_address.Name = "g_address"
        Me.g_address.Size = New System.Drawing.Size(76, 19)
        Me.g_address.TabIndex = 5
        Me.g_address.TabStop = True
        Me.g_address.Text = "address"
        Me.g_address.UseVisualStyleBackColor = True
        '
        'g_value
        '
        Me.g_value.AutoSize = True
        Me.g_value.Checked = True
        Me.g_value.Location = New System.Drawing.Point(280, 15)
        Me.g_value.Name = "g_value"
        Me.g_value.Size = New System.Drawing.Size(61, 19)
        Me.g_value.TabIndex = 6
        Me.g_value.TabStop = True
        Me.g_value.Text = "value"
        Me.g_value.UseVisualStyleBackColor = True
        '
        'アドレス
        '
        Me.アドレス.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.アドレス.Frozen = True
        Me.アドレス.HeaderText = "アドレス"
        Me.アドレス.MaxInputLength = 10
        Me.アドレス.Name = "アドレス"
        Me.アドレス.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.アドレス.Width = 57
        '
        '値
        '
        Me.値.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.値.Frozen = True
        Me.値.HeaderText = "値"
        Me.値.MaxInputLength = 10
        Me.値.Name = "値"
        Me.値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.値.Width = 28
        '
        '編集タイプ
        '
        Me.編集タイプ.AutoComplete = False
        Me.編集タイプ.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.編集タイプ.Frozen = True
        Me.編集タイプ.HeaderText = "編集タイプ"
        Me.編集タイプ.Items.AddRange(New Object() {"DEC", "DEC16BIT", "BINARY32", "BIN32>>16", "BINARY16", "OR", "AND", "XOR"})
        Me.編集タイプ.Name = "編集タイプ"
        Me.編集タイプ.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.編集タイプ.Width = 75
        '
        '入力値
        '
        Me.入力値.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.入力値.Frozen = True
        Me.入力値.HeaderText = "入力値　　　 "
        Me.入力値.MaxInputLength = 11
        Me.入力値.MinimumWidth = 88
        Me.入力値.Name = "入力値"
        Me.入力値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.入力値.Width = 93
        '
        '備考
        '
        Me.備考.FillWeight = 200.0!
        Me.備考.HeaderText = "備考　　　　"
        Me.備考.MaxInputLength = 64
        Me.備考.MinimumWidth = 80
        Me.備考.Name = "備考"
        Me.備考.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.備考.Width = 83
        '
        'datagrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 364)
        Me.Controls.Add(Me.g_value)
        Me.Controls.Add(Me.g_address)
        Me.Controls.Add(Me.gridsave)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.APPLY)
        Me.Controls.Add(Me.DataGridView1)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "datagrid"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "データグリッドエディター"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CNVbikou.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents APPLY As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gridsave As System.Windows.Forms.CheckBox
    Friend WithEvents g_address As System.Windows.Forms.RadioButton
    Friend WithEvents g_value As System.Windows.Forms.RadioButton
    Friend WithEvents CNVbikou As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents notetag As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents appy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents moveup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents movedown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents addline As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents paste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents addmacro As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents アドレス As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 編集タイプ As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents 入力値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 備考 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
