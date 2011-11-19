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
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.fontcustom = New System.Windows.Forms.RadioButton()
        Me.font5 = New System.Windows.Forms.RadioButton()
        Me.font4 = New System.Windows.Forms.RadioButton()
        Me.font3 = New System.Windows.Forms.RadioButton()
        Me.font2 = New System.Windows.Forms.RadioButton()
        Me.font1 = New System.Windows.Forms.RadioButton()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(32, 56)
        Me.RadioButton2.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(114, 19)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "RadioButton2"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.fontcustom)
        Me.GroupBox1.Controls.Add(Me.font5)
        Me.GroupBox1.Controls.Add(Me.font4)
        Me.GroupBox1.Controls.Add(Me.font3)
        Me.GroupBox1.Controls.Add(Me.font2)
        Me.GroupBox1.Controls.Add(Me.font1)
        Me.GroupBox1.Location = New System.Drawing.Point(32, 15)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(203, 190)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "ふぉんと選択"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(121, 162)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(27, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'fontcustom
        '
        Me.fontcustom.AutoSize = True
        Me.fontcustom.Location = New System.Drawing.Point(10, 164)
        Me.fontcustom.Name = "fontcustom"
        Me.fontcustom.Size = New System.Drawing.Size(107, 19)
        Me.fontcustom.TabIndex = 5
        Me.fontcustom.TabStop = True
        Me.fontcustom.Text = "かすたむ指定"
        Me.fontcustom.UseVisualStyleBackColor = True
        '
        'font5
        '
        Me.font5.AutoSize = True
        Me.font5.Location = New System.Drawing.Point(9, 138)
        Me.font5.Margin = New System.Windows.Forms.Padding(4)
        Me.font5.Name = "font5"
        Me.font5.Size = New System.Drawing.Size(173, 19)
        Me.font5.TabIndex = 4
        Me.font5.TabStop = True
        Me.font5.Text = "acorn_bold(日本語なし)"
        Me.font5.UseVisualStyleBackColor = True
        '
        'font4
        '
        Me.font4.AutoSize = True
        Me.font4.Location = New System.Drawing.Point(9, 110)
        Me.font4.Margin = New System.Windows.Forms.Padding(4)
        Me.font4.Name = "font4"
        Me.font4.Size = New System.Drawing.Size(102, 19)
        Me.font4.TabIndex = 3
        Me.font4.TabStop = True
        Me.font4.Text = "美咲カタカナ"
        Me.font4.UseVisualStyleBackColor = True
        '
        'font3
        '
        Me.font3.AutoSize = True
        Me.font3.Location = New System.Drawing.Point(9, 81)
        Me.font3.Margin = New System.Windows.Forms.Padding(4)
        Me.font3.Name = "font3"
        Me.font3.Size = New System.Drawing.Size(105, 19)
        Me.font3.TabIndex = 2
        Me.font3.TabStop = True
        Me.font3.Text = "美咲ひらがな"
        Me.font3.UseVisualStyleBackColor = True
        '
        'font2
        '
        Me.font2.AutoSize = True
        Me.font2.Location = New System.Drawing.Point(9, 52)
        Me.font2.Margin = New System.Windows.Forms.Padding(4)
        Me.font2.Name = "font2"
        Me.font2.Size = New System.Drawing.Size(168, 19)
        Me.font2.TabIndex = 1
        Me.font2.TabStop = True
        Me.font2.Text = "telazorn＋美咲カタカナ"
        Me.font2.UseVisualStyleBackColor = True
        '
        'font1
        '
        Me.font1.AutoSize = True
        Me.font1.Location = New System.Drawing.Point(9, 24)
        Me.font1.Margin = New System.Windows.Forms.Padding(4)
        Me.font1.Name = "font1"
        Me.font1.Size = New System.Drawing.Size(171, 19)
        Me.font1.TabIndex = 0
        Me.font1.TabStop = True
        Me.font1.Text = "telazorn＋美咲ひらがな"
        Me.font1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(261, 82)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 29)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "ぱっち"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(41, 213)
        Me.CheckBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(319, 19)
        Me.CheckBox1.TabIndex = 4
        Me.CheckBox1.Text = "PRO/MEの翻訳TXTをflash0:/から読むようにする"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(389, 248)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.RadioButton2)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.Text = "ｐｒｘふぉんとぱっちゃー"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents font4 As System.Windows.Forms.RadioButton
    Friend WithEvents font3 As System.Windows.Forms.RadioButton
    Friend WithEvents font2 As System.Windows.Forms.RadioButton
    Friend WithEvents font1 As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents font5 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents fontcustom As System.Windows.Forms.RadioButton

End Class
