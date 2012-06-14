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
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.unihex = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'READ
        '
        Me.READ.Location = New System.Drawing.Point(27, 31)
        Me.READ.Name = "READ"
        Me.READ.Size = New System.Drawing.Size(75, 23)
        Me.READ.TabIndex = 2
        Me.READ.Text = "TEST"
        Me.READ.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("メイリオ", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(27, 60)
        Me.TextBox1.MaxLength = 0
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(373, 362)
        Me.TextBox1.TabIndex = 3
        '
        'SAVE
        '
        Me.SAVE.Location = New System.Drawing.Point(125, 31)
        Me.SAVE.Name = "SAVE"
        Me.SAVE.Size = New System.Drawing.Size(75, 23)
        Me.SAVE.TabIndex = 4
        Me.SAVE.Text = "保存"
        Me.SAVE.UseVisualStyleBackColor = True
        '
        'CLEAR
        '
        Me.CLEAR.Location = New System.Drawing.Point(227, 31)
        Me.CLEAR.Name = "CLEAR"
        Me.CLEAR.Size = New System.Drawing.Size(75, 23)
        Me.CLEAR.TabIndex = 5
        Me.CLEAR.Text = "初期化"
        Me.CLEAR.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(27, 9)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(100, 16)
        Me.RadioButton1.TabIndex = 6
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "SHIFTJIS_2004"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(133, 9)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(95, 16)
        Me.RadioButton2.TabIndex = 7
        Me.RadioButton2.Text = "EUC_JIS_2004"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(325, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "FONT"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(235, 8)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(60, 16)
        Me.RadioButton3.TabIndex = 9
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "EXTRA"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'unihex
        '
        Me.unihex.AutoSize = True
        Me.unihex.Location = New System.Drawing.Point(333, 36)
        Me.unihex.Name = "unihex"
        Me.unihex.Size = New System.Drawing.Size(9, 12)
        Me.unihex.TabIndex = 10
        Me.unihex.Text = " "
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 434)
        Me.Controls.Add(Me.unihex)
        Me.Controls.Add(Me.RadioButton3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.CLEAR)
        Me.Controls.Add(Me.SAVE)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.READ)
        Me.MaximumSize = New System.Drawing.Size(437, 472)
        Me.MinimumSize = New System.Drawing.Size(437, 472)
        Me.Name = "Form1"
        Me.Text = "JIS0213_TXTえでぃた"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents READ As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents SAVE As System.Windows.Forms.Button
    Friend WithEvents CLEAR As System.Windows.Forms.Button
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents unihex As System.Windows.Forms.Label

End Class
