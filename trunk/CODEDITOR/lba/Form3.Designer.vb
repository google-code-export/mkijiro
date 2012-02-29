<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SDIR = New System.Windows.Forms.RadioButton()
        Me.FD = New System.Windows.Forms.RadioButton()
        Me.APATH = New System.Windows.Forms.RadioButton()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TXTBOOT = New System.Windows.Forms.CheckBox()
        Me.NOUMD = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 15)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(263, 220)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabPage1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.TabPage1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(255, 192)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "ファイル書き込み"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(252, 189)
        Me.Panel1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(196, 146)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 14)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "MiB"
        '
        'TextBox1
        '
        Me.TextBox1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox1.Location = New System.Drawing.Point(163, 143)
        Me.TextBox1.MaxLength = 2
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(27, 21)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = "0"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(140, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "一時バッファ(0で無制限)"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.SDIR)
        Me.GroupBox1.Controls.Add(Me.FD)
        Me.GroupBox1.Controls.Add(Me.APATH)
        Me.GroupBox1.Location = New System.Drawing.Point(20, 17)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 99)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "保存先"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(124, 41)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(24, 21)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = ".."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'SDIR
        '
        Me.SDIR.AutoSize = True
        Me.SDIR.Location = New System.Drawing.Point(26, 44)
        Me.SDIR.Name = "SDIR"
        Me.SDIR.Size = New System.Drawing.Size(92, 18)
        Me.SDIR.TabIndex = 2
        Me.SDIR.TabStop = True
        Me.SDIR.Text = "指定フォルダ"
        Me.SDIR.UseVisualStyleBackColor = True
        '
        'FD
        '
        Me.FD.AutoSize = True
        Me.FD.Location = New System.Drawing.Point(26, 68)
        Me.FD.Name = "FD"
        Me.FD.Size = New System.Drawing.Size(154, 18)
        Me.FD.TabIndex = 1
        Me.FD.TabStop = True
        Me.FD.Text = "ファイルダイアログで指定"
        Me.FD.UseVisualStyleBackColor = True
        '
        'APATH
        '
        Me.APATH.AutoSize = True
        Me.APATH.Checked = True
        Me.APATH.Location = New System.Drawing.Point(26, 20)
        Me.APATH.Name = "APATH"
        Me.APATH.Size = New System.Drawing.Size(133, 18)
        Me.APATH.TabIndex = 0
        Me.APATH.TabStop = True
        Me.APATH.Text = "EXE起動パスと同じ"
        Me.APATH.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage2.Controls.Add(Me.Panel2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(255, 192)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "その他"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.Controls.Add(Me.GroupBox3)
        Me.Panel2.Controls.Add(Me.GroupBox2)
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(249, 199)
        Me.Panel2.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TextBox4)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.TextBox3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.TextBox2)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(195, 108)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "表示限界数"
        '
        'TextBox4
        '
        Me.TextBox4.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TextBox4.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox4.Location = New System.Drawing.Point(122, 78)
        Me.TextBox4.MaxLength = 5
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(50, 21)
        Me.TextBox4.TabIndex = 5
        Me.TextBox4.Text = "50000"
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 81)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(110, 14)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "仮想モード時リスト"
        '
        'TextBox3
        '
        Me.TextBox3.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TextBox3.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox3.Location = New System.Drawing.Point(122, 50)
        Me.TextBox3.MaxLength = 4
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(50, 21)
        Me.TextBox3.TabIndex = 3
        Me.TextBox3.Text = "3000"
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 53)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(115, 14)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "通常MODE時リスト"
        '
        'TextBox2
        '
        Me.TextBox2.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox2.Location = New System.Drawing.Point(122, 23)
        Me.TextBox2.MaxLength = 5
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(50, 21)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.Text = "50000"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(66, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "ツリーノード"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(98, 243)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "OK"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TXTBOOT
        '
        Me.TXTBOOT.AutoSize = True
        Me.TXTBOOT.Location = New System.Drawing.Point(9, 20)
        Me.TXTBOOT.Name = "TXTBOOT"
        Me.TXTBOOT.Size = New System.Drawing.Size(206, 18)
        Me.TXTBOOT.TabIndex = 4
        Me.TXTBOOT.Text = "LBAリストをTXTエディターで起動"
        Me.TXTBOOT.UseVisualStyleBackColor = True
        '
        'NOUMD
        '
        Me.NOUMD.AutoSize = True
        Me.NOUMD.Location = New System.Drawing.Point(9, 44)
        Me.NOUMD.Name = "NOUMD"
        Me.NOUMD.Size = New System.Drawing.Size(145, 18)
        Me.NOUMD.TabIndex = 5
        Me.NOUMD.Text = "UMD以外の指定EXE"
        Me.NOUMD.UseVisualStyleBackColor = True
        Me.NOUMD.Visible = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Button3)
        Me.GroupBox3.Controls.Add(Me.TXTBOOT)
        Me.GroupBox3.Controls.Add(Me.NOUMD)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 120)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(226, 69)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "EXE起動"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(160, 41)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(24, 21)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = ".."
        Me.Button3.UseVisualStyleBackColor = True
        Me.Button3.Visible = False
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(286, 278)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Form3"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "設定"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SDIR As System.Windows.Forms.RadioButton
    Friend WithEvents FD As System.Windows.Forms.RadioButton
    Friend WithEvents APATH As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents TXTBOOT As System.Windows.Forms.CheckBox
    Friend WithEvents NOUMD As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
