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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.アドレス = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.編集タイプ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.入力値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridsave = New System.Windows.Forms.CheckBox()
        Me.g_address = New System.Windows.Forms.RadioButton()
        Me.g_value = New System.Windows.Forms.RadioButton()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.アドレス, Me.値, Me.編集タイプ, Me.入力値})
        Me.DataGridView1.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.DataGridView1.Location = New System.Drawing.Point(4, 49)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 21
        Me.DataGridView1.Size = New System.Drawing.Size(591, 300)
        Me.DataGridView1.TabIndex = 0
        '
        'アドレス
        '
        Me.アドレス.HeaderText = "アドレス"
        Me.アドレス.MaxInputLength = 10
        Me.アドレス.Name = "アドレス"
        Me.アドレス.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.アドレス.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        '値
        '
        Me.値.HeaderText = "値"
        Me.値.MaxInputLength = 10
        Me.値.Name = "値"
        Me.値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        '編集タイプ
        '
        Me.編集タイプ.HeaderText = "編集タイプ"
        Me.編集タイプ.MaxInputLength = 20
        Me.編集タイプ.Name = "編集タイプ"
        Me.編集タイプ.ReadOnly = True
        Me.編集タイプ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        '入力値
        '
        Me.入力値.HeaderText = "入力値"
        Me.入力値.MaxInputLength = 11
        Me.入力値.Name = "入力値"
        Me.入力値.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(348, 9)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(100, 29)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "APPLY"
        Me.Button2.UseVisualStyleBackColor = True
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
        Me.gridsave.Size = New System.Drawing.Size(145, 19)
        Me.gridsave.TabIndex = 4
        Me.gridsave.Text = "APPLY同時に保存"
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
        'datagrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 364)
        Me.Controls.Add(Me.g_value)
        Me.Controls.Add(Me.g_address)
        Me.Controls.Add(Me.gridsave)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "datagrid"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "データグリッドエディター"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents アドレス As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 編集タイプ As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 入力値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents gridsave As System.Windows.Forms.CheckBox
    Friend WithEvents g_address As System.Windows.Forms.RadioButton
    Friend WithEvents g_value As System.Windows.Forms.RadioButton
End Class
