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
        Me.READ = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.SAVE = New System.Windows.Forms.Button()
        Me.CLEAR = New System.Windows.Forms.Button()
        Me.SJIS = New System.Windows.Forms.RadioButton()
        Me.EUC = New System.Windows.Forms.RadioButton()
        Me.FONTs = New System.Windows.Forms.Button()
        Me.EX = New System.Windows.Forms.RadioButton()
        Me.unihex = New System.Windows.Forms.Label()
        Me.outTX = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout
        '
        'READ
        '
        Me.READ.Location = New System.Drawing.Point(27, 31)
        Me.READ.Name = "READ"
        Me.READ.Size = New System.Drawing.Size(87, 23)
        Me.READ.TabIndex = 2
        Me.READ.Text = "読み込みテスト"
        Me.READ.UseVisualStyleBackColor = true
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("メイリオ", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128,Byte))
        Me.TextBox1.Location = New System.Drawing.Point(27, 60)
        Me.TextBox1.MaxLength = 0
        Me.TextBox1.Multiline = true
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(373, 362)
        Me.TextBox1.TabIndex = 3
        '
        'SAVE
        '
        Me.SAVE.Location = New System.Drawing.Point(133, 31)
        Me.SAVE.Name = "SAVE"
        Me.SAVE.Size = New System.Drawing.Size(75, 23)
        Me.SAVE.TabIndex = 4
        Me.SAVE.Text = "保存"
        Me.SAVE.UseVisualStyleBackColor = true
        '
        'CLEAR
        '
        Me.CLEAR.Location = New System.Drawing.Point(235, 31)
        Me.CLEAR.Name = "CLEAR"
        Me.CLEAR.Size = New System.Drawing.Size(75, 23)
        Me.CLEAR.TabIndex = 5
        Me.CLEAR.Text = "初期化"
        Me.CLEAR.UseVisualStyleBackColor = true
        '
        'SJIS
        '
        Me.SJIS.AutoSize = true
        Me.SJIS.Checked = true
        Me.SJIS.Location = New System.Drawing.Point(27, 9)
        Me.SJIS.Name = "SJIS"
        Me.SJIS.Size = New System.Drawing.Size(100, 16)
        Me.SJIS.TabIndex = 6
        Me.SJIS.TabStop = true
        Me.SJIS.Text = "SHIFTJIS_2004"
        Me.SJIS.UseVisualStyleBackColor = true
        '
        'EUC
        '
        Me.EUC.AutoSize = true
        Me.EUC.Location = New System.Drawing.Point(133, 9)
        Me.EUC.Name = "EUC"
        Me.EUC.Size = New System.Drawing.Size(95, 16)
        Me.EUC.TabIndex = 7
        Me.EUC.Text = "EUC_JIS_2004"
        Me.EUC.UseVisualStyleBackColor = true
        '
        'FONTs
        '
        Me.FONTs.Location = New System.Drawing.Point(325, 6)
        Me.FONTs.Name = "FONTs"
        Me.FONTs.Size = New System.Drawing.Size(75, 23)
        Me.FONTs.TabIndex = 8
        Me.FONTs.Text = "FONT"
        Me.FONTs.UseVisualStyleBackColor = true
        '
        'EX
        '
        Me.EX.AutoSize = true
        Me.EX.Location = New System.Drawing.Point(235, 8)
        Me.EX.Name = "EX"
        Me.EX.Size = New System.Drawing.Size(60, 16)
        Me.EX.TabIndex = 9
        Me.EX.TabStop = true
        Me.EX.Text = "EXTRA"
        Me.EX.UseVisualStyleBackColor = true
        '
        'unihex
        '
        Me.unihex.AutoSize = true
        Me.unihex.Location = New System.Drawing.Point(25, 425)
        Me.unihex.Name = "unihex"
        Me.unihex.Size = New System.Drawing.Size(141, 12)
        Me.unihex.TabIndex = 10
        Me.unihex.Text = " UTF-32:  UTF-16: UTF-8:"
        '
        'outTX
        '
        Me.outTX.AutoSize = True
        Me.outTX.Location = New System.Drawing.Point(322, 35)
        Me.outTX.Name = "outTX"
        Me.outTX.Size = New System.Drawing.Size(87, 16)
        Me.outTX.TabIndex = 11
        Me.outTX.Text = "out.txtを読む"
        Me.outTX.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 442)
        Me.Controls.Add(Me.outTX)
        Me.Controls.Add(Me.unihex)
        Me.Controls.Add(Me.EX)
        Me.Controls.Add(Me.FONTs)
        Me.Controls.Add(Me.EUC)
        Me.Controls.Add(Me.SJIS)
        Me.Controls.Add(Me.CLEAR)
        Me.Controls.Add(Me.SAVE)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.READ)
        Me.MaximumSize = New System.Drawing.Size(437, 480)
        Me.MinimumSize = New System.Drawing.Size(437, 480)
        Me.Name = "Form1"
        Me.Text = "JIS0213_TXTえでぃた"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents READ As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents SAVE As System.Windows.Forms.Button
    Friend WithEvents CLEAR As System.Windows.Forms.Button
    Friend WithEvents SJIS As System.Windows.Forms.RadioButton
    Friend WithEvents EUC As System.Windows.Forms.RadioButton
    Friend WithEvents FONTs As System.Windows.Forms.Button
    Friend WithEvents EX As System.Windows.Forms.RadioButton
    Friend WithEvents unihex As System.Windows.Forms.Label
    Friend WithEvents outTX As System.Windows.Forms.CheckBox

End Class
