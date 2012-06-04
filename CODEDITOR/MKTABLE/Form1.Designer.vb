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
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ENCODE = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.textbox4 = New System.Windows.Forms.ComboBox()
        Me.EX = New System.Windows.Forms.CheckBox()
        Me.sp = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(57, 43)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(124, 20)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "MAKE TABLE"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(57, 80)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(124, 20)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "TABLE TEST"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(83, 7)
        Me.TextBox1.MaxLength = 200
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(137, 19)
        Me.TextBox1.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 12)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "UTF8文字"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "文字出力"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(83, 29)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(137, 19)
        Me.TextBox2.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 12)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "16進数コード"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(83, 52)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(137, 42)
        Me.TextBox3.TabIndex = 8
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextBox3)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TextBox2)
        Me.Panel1.Location = New System.Drawing.Point(27, 105)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(231, 104)
        Me.Panel1.TabIndex = 9
        '
        'ENCODE
        '
        Me.ENCODE.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ENCODE.FormattingEnabled = True
        Me.ENCODE.Items.AddRange(New Object() {"SHIFT-JIS(CP932)", "EUC-JP(CP51932)", "GBK(CP936)", "UTF16LE(CP1200)", "UTF16BE(CP1201)", "UTF16BE→EUC-JP(UTF8->UTF16BE->CP51932)", "UTF16BE→SJIS(UTF8->UTF16BE->CP932)", "UTF16BE→GBK(UTF8->UTF16BE->CP936)", "Unicode Consortiumテキストテーブル(CP1201)", "JIS213_2004テキストテーブル(CP12001)", "(c1^0x20-0xA1)*192+c2-0x40(SJIS,CP936/CP65001)", "(c1-0x81)*192+c2-0x40(GBK,CP932/CP65001)", "(c1-0xA1)*96+c2-0xa1(EUC,CP936/CP65001)"})
        Me.ENCODE.Location = New System.Drawing.Point(92, 10)
        Me.ENCODE.Name = "ENCODE"
        Me.ENCODE.Size = New System.Drawing.Size(120, 20)
        Me.ENCODE.TabIndex = 10
        Me.ENCODE.Text = "SHIFT-JIS(CP932)"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(33, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 12)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "エンコード"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(195, 83)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 12)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "豆腐"
        '
        'textbox4
        '
        Me.textbox4.FormattingEnabled = True
        Me.textbox4.Items.AddRange(New Object() {"□", "■", "〓", "・"})
        Me.textbox4.Location = New System.Drawing.Point(231, 81)
        Me.textbox4.Name = "textbox4"
        Me.textbox4.Size = New System.Drawing.Size(34, 20)
        Me.textbox4.TabIndex = 13
        Me.textbox4.Text = "□"
        '
        'EX
        '
        Me.EX.AutoSize = True
        Me.EX.Location = New System.Drawing.Point(218, 9)
        Me.EX.Name = "EX"
        Me.EX.Size = New System.Drawing.Size(65, 16)
        Me.EX.TabIndex = 14
        Me.EX.Text = "えくすとら"
        Me.EX.UseVisualStyleBackColor = True
        '
        'sp
        '
        Me.sp.AutoSize = True
        Me.sp.Location = New System.Drawing.Point(218, 28)
        Me.sp.Name = "sp"
        Me.sp.Size = New System.Drawing.Size(55, 16)
        Me.sp.TabIndex = 15
        Me.sp.Text = "NOM$"
        Me.sp.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 219)
        Me.Controls.Add(Me.sp)
        Me.Controls.Add(Me.textbox4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.EX)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ENCODE)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MKTABLE"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ENCODE As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents textbox4 As System.Windows.Forms.ComboBox
    Friend WithEvents EX As System.Windows.Forms.CheckBox
    Friend WithEvents sp As System.Windows.Forms.CheckBox

End Class
