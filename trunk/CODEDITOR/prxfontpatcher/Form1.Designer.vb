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
        Me.RadioButton2.Location = New System.Drawing.Point(24, 45)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(92, 16)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "RadioButton2"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.font5)
        Me.GroupBox1.Controls.Add(Me.font4)
        Me.GroupBox1.Controls.Add(Me.font3)
        Me.GroupBox1.Controls.Add(Me.font2)
        Me.GroupBox1.Controls.Add(Me.font1)
        Me.GroupBox1.Location = New System.Drawing.Point(24, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(152, 138)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "ふぉんと選択"
        '
        'font5
        '
        Me.font5.AutoSize = True
        Me.font5.Location = New System.Drawing.Point(7, 110)
        Me.font5.Name = "font5"
        Me.font5.Size = New System.Drawing.Size(139, 16)
        Me.font5.TabIndex = 4
        Me.font5.TabStop = True
        Me.font5.Text = "acorn_bold(日本語なし)"
        Me.font5.UseVisualStyleBackColor = True
        '
        'font4
        '
        Me.font4.AutoSize = True
        Me.font4.Location = New System.Drawing.Point(7, 88)
        Me.font4.Name = "font4"
        Me.font4.Size = New System.Drawing.Size(83, 16)
        Me.font4.TabIndex = 3
        Me.font4.TabStop = True
        Me.font4.Text = "美咲カタカナ"
        Me.font4.UseVisualStyleBackColor = True
        '
        'font3
        '
        Me.font3.AutoSize = True
        Me.font3.Location = New System.Drawing.Point(7, 65)
        Me.font3.Name = "font3"
        Me.font3.Size = New System.Drawing.Size(85, 16)
        Me.font3.TabIndex = 2
        Me.font3.TabStop = True
        Me.font3.Text = "美咲ひらがな"
        Me.font3.UseVisualStyleBackColor = True
        '
        'font2
        '
        Me.font2.AutoSize = True
        Me.font2.Location = New System.Drawing.Point(7, 42)
        Me.font2.Name = "font2"
        Me.font2.Size = New System.Drawing.Size(135, 16)
        Me.font2.TabIndex = 1
        Me.font2.TabStop = True
        Me.font2.Text = "telazorn＋美咲カタカナ"
        Me.font2.UseVisualStyleBackColor = True
        '
        'font1
        '
        Me.font1.AutoSize = True
        Me.font1.Location = New System.Drawing.Point(7, 19)
        Me.font1.Name = "font1"
        Me.font1.Size = New System.Drawing.Size(137, 16)
        Me.font1.TabIndex = 0
        Me.font1.TabStop = True
        Me.font1.Text = "telazorn＋美咲ひらがな"
        Me.font1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(196, 66)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "ぱっち"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(31, 157)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(256, 16)
        Me.CheckBox1.TabIndex = 4
        Me.CheckBox1.Text = "PRO/MEの翻訳TXTをflash0:/から読むようにする"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 189)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.RadioButton2)
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

End Class
