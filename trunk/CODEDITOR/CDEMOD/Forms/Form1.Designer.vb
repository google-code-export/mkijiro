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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.アドレス = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.編集タイプ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.入力値 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.アドレス, Me.値, Me.編集タイプ, Me.入力値})
        Me.DataGridView1.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.DataGridView1.Location = New System.Drawing.Point(3, 39)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 21
        Me.DataGridView1.Size = New System.Drawing.Size(443, 240)
        Me.DataGridView1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(131, 10)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "LOAD"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(229, 10)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "APPLY"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(310, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(9, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = " "
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(483, 291)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "グリッドエディター"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents アドレス As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 値 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 編集タイプ As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 入力値 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
