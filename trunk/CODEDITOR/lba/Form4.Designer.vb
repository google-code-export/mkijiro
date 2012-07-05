<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.parse_mode = New System.Windows.Forms.Label()
        Me.fsize = New System.Windows.Forms.Label()
        Me.lbas = New System.Windows.Forms.Label()
        Me.bsw = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Meiryo UI", 9.163636!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(20, 34)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(657, 259)
        Me.TextBox1.TabIndex = 0
        '
        'ComboBox1
        '
        Me.ComboBox1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Shift-JIS (932)", "GBK(936)", "ANSI(1252)", "EUC-JP(51932)", "M$JIS(50220)", "UTF7(65000)", "UTF8N(65001)", "UTF16LE(1200)", "UTF16BE(1201)", "UTF32(12000)"})
        Me.ComboBox1.Location = New System.Drawing.Point(478, 6)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(104, 20)
        Me.ComboBox1.TabIndex = 6
        Me.ComboBox1.Text = "UTF8N(65001)"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(422, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 12)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "ENCODE"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(20, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 20)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "START"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(172, 5)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(64, 20)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "END"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(89, 5)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(34, 20)
        Me.Button3.TabIndex = 10
        Me.Button3.Text = "<"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(129, 5)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(39, 20)
        Me.Button4.TabIndex = 11
        Me.Button4.Text = ">"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(249, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 12)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "CURR/END"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(320, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(23, 12)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "0/0"
        '
        'parse_mode
        '
        Me.parse_mode.AutoSize = True
        Me.parse_mode.Location = New System.Drawing.Point(366, 11)
        Me.parse_mode.Name = "parse_mode"
        Me.parse_mode.Size = New System.Drawing.Size(9, 12)
        Me.parse_mode.TabIndex = 14
        Me.parse_mode.Text = " "
        Me.parse_mode.Visible = False
        '
        'fsize
        '
        Me.fsize.AutoSize = True
        Me.fsize.Location = New System.Drawing.Point(381, 11)
        Me.fsize.Name = "fsize"
        Me.fsize.Size = New System.Drawing.Size(9, 12)
        Me.fsize.TabIndex = 15
        Me.fsize.Text = " "
        Me.fsize.Visible = False
        '
        'lbas
        '
        Me.lbas.AutoSize = True
        Me.lbas.Location = New System.Drawing.Point(397, 17)
        Me.lbas.Name = "lbas"
        Me.lbas.Size = New System.Drawing.Size(9, 12)
        Me.lbas.TabIndex = 16
        Me.lbas.Text = " "
        Me.lbas.Visible = False
        '
        'bsw
        '
        Me.bsw.AutoSize = True
        Me.bsw.Location = New System.Drawing.Point(589, 6)
        Me.bsw.Name = "bsw"
        Me.bsw.Size = New System.Drawing.Size(90, 16)
        Me.bsw.TabIndex = 17
        Me.bsw.Text = "ばいとすわっぷ"
        Me.bsw.UseVisualStyleBackColor = True
        '
        'Form4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(689, 303)
        Me.Controls.Add(Me.bsw)
        Me.Controls.Add(Me.lbas)
        Me.Controls.Add(Me.fsize)
        Me.Controls.Add(Me.parse_mode)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Name = "Form4"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form4"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents parse_mode As System.Windows.Forms.Label
    Friend WithEvents fsize As System.Windows.Forms.Label
    Friend WithEvents lbas As System.Windows.Forms.Label
    Friend WithEvents bsw As System.Windows.Forms.CheckBox
End Class
