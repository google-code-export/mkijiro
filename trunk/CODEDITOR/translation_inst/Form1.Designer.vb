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
        Me.INSTALL = New System.Windows.Forms.Button()
        Me.CFWME = New System.Windows.Forms.RadioButton()
        Me.CFWPRO = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CFWPROGO = New System.Windows.Forms.RadioButton()
        Me.drivelettter = New System.Windows.Forms.DomainUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HIRA = New System.Windows.Forms.RadioButton()
        Me.KANA = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lockdriveletter = New System.Windows.Forms.RadioButton()
        Me.AUTOPSP = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cfont = New System.Windows.Forms.RadioButton()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'INSTALL
        '
        Me.INSTALL.Enabled = False
        Me.INSTALL.ImageKey = "前回インストールしたｐｒｘ"
        Me.INSTALL.Location = New System.Drawing.Point(48, 380)
        Me.INSTALL.Margin = New System.Windows.Forms.Padding(4)
        Me.INSTALL.Name = "INSTALL"
        Me.INSTALL.Size = New System.Drawing.Size(131, 29)
        Me.INSTALL.TabIndex = 0
        Me.INSTALL.Text = "いんすとーる"
        Me.INSTALL.UseVisualStyleBackColor = True
        '
        'CFWME
        '
        Me.CFWME.AutoSize = True
        Me.CFWME.Checked = True
        Me.CFWME.ImageKey = "前回インストールしたｐｒｘ"
        Me.CFWME.Location = New System.Drawing.Point(17, 28)
        Me.CFWME.Margin = New System.Windows.Forms.Padding(4)
        Me.CFWME.Name = "CFWME"
        Me.CFWME.Size = New System.Drawing.Size(81, 19)
        Me.CFWME.TabIndex = 1
        Me.CFWME.TabStop = True
        Me.CFWME.Text = "CFW ME"
        Me.CFWME.UseVisualStyleBackColor = True
        '
        'CFWPRO
        '
        Me.CFWPRO.AutoSize = True
        Me.CFWPRO.ImageKey = "前回インストールしたｐｒｘ"
        Me.CFWPRO.Location = New System.Drawing.Point(17, 55)
        Me.CFWPRO.Margin = New System.Windows.Forms.Padding(4)
        Me.CFWPRO.Name = "CFWPRO"
        Me.CFWPRO.Size = New System.Drawing.Size(91, 19)
        Me.CFWPRO.TabIndex = 2
        Me.CFWPRO.Text = "CFW PRO"
        Me.CFWPRO.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CFWPROGO)
        Me.GroupBox1.Controls.Add(Me.CFWPRO)
        Me.GroupBox1.Controls.Add(Me.CFWME)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 13)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(202, 112)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "1.CFW選択"
        '
        'CFWPROGO
        '
        Me.CFWPROGO.AutoSize = True
        Me.CFWPROGO.Location = New System.Drawing.Point(17, 81)
        Me.CFWPROGO.Name = "CFWPROGO"
        Me.CFWPROGO.Size = New System.Drawing.Size(149, 19)
        Me.CFWPROGO.TabIndex = 3
        Me.CFWPROGO.Text = "CFW PRO(PSPGO)"
        Me.CFWPROGO.UseVisualStyleBackColor = True
        '
        'drivelettter
        '
        Me.drivelettter.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.drivelettter.Items.Add("D:")
        Me.drivelettter.Items.Add("E:")
        Me.drivelettter.Items.Add("F:")
        Me.drivelettter.Items.Add("G:")
        Me.drivelettter.Items.Add("H:")
        Me.drivelettter.Items.Add("I:")
        Me.drivelettter.Items.Add("J:")
        Me.drivelettter.Items.Add("K:")
        Me.drivelettter.Items.Add("L:")
        Me.drivelettter.Items.Add("M:")
        Me.drivelettter.Items.Add("N:")
        Me.drivelettter.Items.Add("O:")
        Me.drivelettter.Items.Add("P:")
        Me.drivelettter.Items.Add("Q:")
        Me.drivelettter.Items.Add("R:")
        Me.drivelettter.Items.Add("S:")
        Me.drivelettter.Items.Add("T:")
        Me.drivelettter.Items.Add("U:")
        Me.drivelettter.Items.Add("V:")
        Me.drivelettter.Items.Add("W:")
        Me.drivelettter.Items.Add("X:")
        Me.drivelettter.Items.Add("Y:")
        Me.drivelettter.Items.Add("Z:")
        Me.drivelettter.Location = New System.Drawing.Point(149, 61)
        Me.drivelettter.Margin = New System.Windows.Forms.Padding(4)
        Me.drivelettter.Name = "drivelettter"
        Me.drivelettter.Size = New System.Drawing.Size(44, 22)
        Me.drivelettter.TabIndex = 8
        Me.drivelettter.Text = "D:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(231, 170)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 15)
        Me.Label3.TabIndex = 12
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.MenuBar
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(227, 24)
        Me.MenuStrip1.TabIndex = 16
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'HIRA
        '
        Me.HIRA.AutoSize = True
        Me.HIRA.Location = New System.Drawing.Point(15, 26)
        Me.HIRA.Name = "HIRA"
        Me.HIRA.Size = New System.Drawing.Size(105, 19)
        Me.HIRA.TabIndex = 17
        Me.HIRA.TabStop = True
        Me.HIRA.Text = "美咲ひらがな"
        Me.HIRA.UseVisualStyleBackColor = True
        '
        'KANA
        '
        Me.KANA.AutoSize = True
        Me.KANA.Location = New System.Drawing.Point(15, 51)
        Me.KANA.Name = "KANA"
        Me.KANA.Size = New System.Drawing.Size(102, 19)
        Me.KANA.TabIndex = 18
        Me.KANA.TabStop = True
        Me.KANA.Text = "美咲カタカナ"
        Me.KANA.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.cfont)
        Me.GroupBox2.Controls.Add(Me.HIRA)
        Me.GroupBox2.Controls.Add(Me.KANA)
        Me.GroupBox2.Enabled = False
        Me.GroupBox2.Location = New System.Drawing.Point(15, 132)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 105)
        Me.GroupBox2.TabIndex = 19
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "2.フォント選択"
        '
        'lockdriveletter
        '
        Me.lockdriveletter.AutoSize = True
        Me.lockdriveletter.Location = New System.Drawing.Point(14, 61)
        Me.lockdriveletter.Name = "lockdriveletter"
        Me.lockdriveletter.Size = New System.Drawing.Size(134, 19)
        Me.lockdriveletter.TabIndex = 20
        Me.lockdriveletter.TabStop = True
        Me.lockdriveletter.Text = "ドライブレター指定"
        Me.lockdriveletter.UseVisualStyleBackColor = True
        '
        'AUTOPSP
        '
        Me.AUTOPSP.AutoSize = True
        Me.AUTOPSP.Location = New System.Drawing.Point(15, 35)
        Me.AUTOPSP.Name = "AUTOPSP"
        Me.AUTOPSP.Size = New System.Drawing.Size(180, 19)
        Me.AUTOPSP.TabIndex = 21
        Me.AUTOPSP.TabStop = True
        Me.AUTOPSP.Text = "自動（D～Zドライブ検索）"
        Me.AUTOPSP.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.drivelettter)
        Me.GroupBox3.Controls.Add(Me.lockdriveletter)
        Me.GroupBox3.Controls.Add(Me.AUTOPSP)
        Me.GroupBox3.Enabled = False
        Me.GroupBox3.Location = New System.Drawing.Point(15, 243)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox3.TabIndex = 22
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "3.ドライブレター検索方法"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 346)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(191, 15)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "4.PSPをUSBモードにして下さい"
        '
        'cfont
        '
        Me.cfont.AutoSize = True
        Me.cfont.Location = New System.Drawing.Point(15, 76)
        Me.cfont.Name = "cfont"
        Me.cfont.Size = New System.Drawing.Size(114, 19)
        Me.cfont.TabIndex = 19
        Me.cfont.TabStop = True
        Me.cfont.Text = "カスタムフォント"
        Me.cfont.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(125, 74)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(23, 23)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(227, 422)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.INSTALL)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("Location", Global.cfw_trans_inst.My.MySettings.Default, "mylocation", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Location = Global.cfw_trans_inst.My.MySettings.Default.mylocation
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "install_helper"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents INSTALL As System.Windows.Forms.Button
    Friend WithEvents CFWME As System.Windows.Forms.RadioButton
    Friend WithEvents CFWPRO As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents drivelettter As System.Windows.Forms.DomainUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents HIRA As System.Windows.Forms.RadioButton
    Friend WithEvents KANA As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lockdriveletter As System.Windows.Forms.RadioButton
    Friend WithEvents AUTOPSP As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents CFWPROGO As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cfont As System.Windows.Forms.RadioButton

End Class
