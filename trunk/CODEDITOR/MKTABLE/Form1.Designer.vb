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
        Me.MAKETABLE = New System.Windows.Forms.Button()
        Me.TABLETEST = New System.Windows.Forms.Button()
        Me.BASESTR = New System.Windows.Forms.TextBox()
        Me.INPUTSTRING = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CVTSTR = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.STRHEX = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ENCODE = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TOFUSTR = New System.Windows.Forms.ComboBox()
        Me.EX = New System.Windows.Forms.CheckBox()
        Me.sp = New System.Windows.Forms.CheckBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.selmode = New System.Windows.Forms.ComboBox()
        Me.jis = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cpuni = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MAKETABLE
        '
        Me.MAKETABLE.Location = New System.Drawing.Point(14, 91)
        Me.MAKETABLE.Name = "MAKETABLE"
        Me.MAKETABLE.Size = New System.Drawing.Size(124, 20)
        Me.MAKETABLE.TabIndex = 0
        Me.MAKETABLE.Text = "MAKE TABLE"
        Me.MAKETABLE.UseVisualStyleBackColor = True
        '
        'TABLETEST
        '
        Me.TABLETEST.Location = New System.Drawing.Point(14, 117)
        Me.TABLETEST.Name = "TABLETEST"
        Me.TABLETEST.Size = New System.Drawing.Size(124, 20)
        Me.TABLETEST.TabIndex = 1
        Me.TABLETEST.Text = "TABLE TEST"
        Me.TABLETEST.UseVisualStyleBackColor = True
        '
        'BASESTR
        '
        Me.BASESTR.Location = New System.Drawing.Point(83, 7)
        Me.BASESTR.MaxLength = 200
        Me.BASESTR.Name = "BASESTR"
        Me.BASESTR.Size = New System.Drawing.Size(137, 19)
        Me.BASESTR.TabIndex = 2
        '
        'INPUTSTRING
        '
        Me.INPUTSTRING.AutoSize = True
        Me.INPUTSTRING.Location = New System.Drawing.Point(7, 9)
        Me.INPUTSTRING.Name = "INPUTSTRING"
        Me.INPUTSTRING.Size = New System.Drawing.Size(57, 12)
        Me.INPUTSTRING.TabIndex = 4
        Me.INPUTSTRING.Text = "UTF8文字"
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
        'CVTSTR
        '
        Me.CVTSTR.Location = New System.Drawing.Point(83, 29)
        Me.CVTSTR.Name = "CVTSTR"
        Me.CVTSTR.Size = New System.Drawing.Size(137, 19)
        Me.CVTSTR.TabIndex = 6
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
        'STRHEX
        '
        Me.STRHEX.Location = New System.Drawing.Point(83, 52)
        Me.STRHEX.Multiline = True
        Me.STRHEX.Name = "STRHEX"
        Me.STRHEX.Size = New System.Drawing.Size(137, 42)
        Me.STRHEX.TabIndex = 8
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.INPUTSTRING)
        Me.Panel1.Controls.Add(Me.STRHEX)
        Me.Panel1.Controls.Add(Me.BASESTR)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.CVTSTR)
        Me.Panel1.Location = New System.Drawing.Point(12, 146)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(231, 104)
        Me.Panel1.TabIndex = 9
        '
        'ENCODE
        '
        Me.ENCODE.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ENCODE.FormattingEnabled = True
        Me.ENCODE.Items.AddRange(New Object() {"SHIFT-JIS(UTF8<->CP932)", "EUC-JP(UTF8<->CP51932)", "GBK(UTF8<->CP936)", "UTF16/32→EUC-JP(UTF8→UTF16/32→CP51932)", "UTF16/32→SJIS(UTF8→UTF16/32→CP932)", "UTF16/32→GBK(UTF8→UTF16/32→CP936)", "Unicode Consortiumテキストテーブル(CP1201→JIS/EUC/SJIS)", "JIS213_2004テキストテーブル(CP12001→JIS/SJS/EUC)", "(c1^0x20-0xA1)*192+c2-0x40(SJIS→CP936/CP65001)", "(c1-0x81)*192+c2-0x40(GBK→CP932/CP65001)", "c3*94*94+(c1-0xA1)*94+c2-0xa1(EUC→CP936/CP65001)", "c3*94*94+(c1-0x21)*94+c2-0x21(JIS→CP936/CP65001)", "文字コードTSVのHTML化(LOCALENC→CP120001)"})
        Me.ENCODE.Location = New System.Drawing.Point(68, 10)
        Me.ENCODE.Name = "ENCODE"
        Me.ENCODE.Size = New System.Drawing.Size(172, 20)
        Me.ENCODE.TabIndex = 10
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 12)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "作成モード"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(144, 121)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 12)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "豆腐"
        '
        'TOFUSTR
        '
        Me.TOFUSTR.FormattingEnabled = True
        Me.TOFUSTR.Items.AddRange(New Object() {"□", "■", "〓", "・"})
        Me.TOFUSTR.Location = New System.Drawing.Point(179, 118)
        Me.TOFUSTR.Name = "TOFUSTR"
        Me.TOFUSTR.Size = New System.Drawing.Size(34, 20)
        Me.TOFUSTR.TabIndex = 13
        Me.TOFUSTR.Text = "□"
        '
        'EX
        '
        Me.EX.AutoSize = True
        Me.EX.Location = New System.Drawing.Point(14, 36)
        Me.EX.Name = "EX"
        Me.EX.Size = New System.Drawing.Size(65, 16)
        Me.EX.TabIndex = 14
        Me.EX.Text = "えくすとら"
        Me.EX.UseVisualStyleBackColor = True
        '
        'sp
        '
        Me.sp.AutoSize = True
        Me.sp.Location = New System.Drawing.Point(85, 36)
        Me.sp.Name = "sp"
        Me.sp.Size = New System.Drawing.Size(55, 16)
        Me.sp.TabIndex = 15
        Me.sp.Text = "NOM$"
        Me.sp.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(18, 71)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(71, 23)
        Me.Button3.TabIndex = 16
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(18, 47)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(70, 23)
        Me.Button4.TabIndex = 17
        Me.Button4.Text = "Button4"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(18, 18)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(75, 23)
        Me.Button5.TabIndex = 18
        Me.Button5.Text = "Button5"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button5)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Location = New System.Drawing.Point(297, 99)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox1.TabIndex = 19
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        Me.GroupBox1.Visible = False
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(315, 80)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 20)
        Me.ComboBox1.TabIndex = 21
        '
        'selmode
        '
        Me.selmode.FormattingEnabled = True
        Me.selmode.Items.AddRange(New Object() {"SJIS->FAKEJIS(ALLOW MORE THEN 0x7F,M$JIS)", "SJIS2004->JIS", "EUC->JIS", "FAKEEUC->JIS(0xA121～0xFEFE,FORCE 2byte EUC )"})
        Me.selmode.Location = New System.Drawing.Point(95, 56)
        Me.selmode.Name = "selmode"
        Me.selmode.Size = New System.Drawing.Size(145, 20)
        Me.selmode.TabIndex = 22
        '
        'jis
        '
        Me.jis.AutoSize = True
        Me.jis.Location = New System.Drawing.Point(14, 58)
        Me.jis.Name = "jis"
        Me.jis.Size = New System.Drawing.Size(77, 16)
        Me.jis.TabIndex = 20
        Me.jis.Text = "強制JIS化"
        Me.jis.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(146, 91)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(94, 21)
        Me.Button1.TabIndex = 23
        Me.Button1.Text = "HTML出力設定"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'cpuni
        '
        Me.cpuni.AutoSize = True
        Me.cpuni.Location = New System.Drawing.Point(146, 34)
        Me.cpuni.Name = "cpuni"
        Me.cpuni.Size = New System.Drawing.Size(70, 16)
        Me.cpuni.TabIndex = 24
        Me.cpuni.Text = "CP->UNI"
        Me.cpuni.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(252, 257)
        Me.Controls.Add(Me.cpuni)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.jis)
        Me.Controls.Add(Me.sp)
        Me.Controls.Add(Me.EX)
        Me.Controls.Add(Me.TOFUSTR)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ENCODE)
        Me.Controls.Add(Me.selmode)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TABLETEST)
        Me.Controls.Add(Me.MAKETABLE)
        Me.Controls.Add(Me.Panel1)
        Me.MaximumSize = New System.Drawing.Size(268, 295)
        Me.MinimumSize = New System.Drawing.Size(268, 295)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MKTABLE"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MAKETABLE As System.Windows.Forms.Button
    Friend WithEvents TABLETEST As System.Windows.Forms.Button
    Friend WithEvents BASESTR As System.Windows.Forms.TextBox
    Friend WithEvents INPUTSTRING As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CVTSTR As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents STRHEX As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ENCODE As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TOFUSTR As System.Windows.Forms.ComboBox
    Friend WithEvents EX As System.Windows.Forms.CheckBox
    Friend WithEvents sp As System.Windows.Forms.CheckBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents selmode As System.Windows.Forms.ComboBox
    Friend WithEvents jis As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cpuni As System.Windows.Forms.CheckBox

End Class
