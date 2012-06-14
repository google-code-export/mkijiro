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
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(57, 45)
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
        Me.Panel1.Location = New System.Drawing.Point(27, 105)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(231, 104)
        Me.Panel1.TabIndex = 9
        '
        'ENCODE
        '
        Me.ENCODE.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ENCODE.FormattingEnabled = True
        Me.ENCODE.Items.AddRange(New Object() {"SHIFT-JIS(UTF8<->CP932)", "EUC-JP(UTF8<->CP51932)", "GBK(UTF8<->CP936)", "UTF16/32→EUC-JP(UTF8→UTF16/32→CP51932)", "UTF16/32→SJIS(UTF8→UTF16/32→CP932)", "UTF16/32→GBK(UTF8→UTF16/32→CP936)", "Unicode Consortiumテキストテーブル(CP1201→JIS/EUC/SJIS)", "JIS213_2004テキストテーブル(CP12001→JIS/SJS/EUC)", "(c1^0x20-0xA1)*192+c2-0x40(SJIS→CP936/CP65001)", "(c1-0x81)*192+c2-0x40(GBK→CP932/CP65001)", "c3*94*94+(c1-0xA1)*94+c2-0xa1(EUC→CP936/CP65001)"})
        Me.ENCODE.Location = New System.Drawing.Point(68, 10)
        Me.ENCODE.Name = "ENCODE"
        Me.ENCODE.Size = New System.Drawing.Size(144, 20)
        Me.ENCODE.TabIndex = 10
        Me.ENCODE.Text = "SHIFT-JIS(UTF8<->CP932)"
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
        'TOFUSTR
        '
        Me.TOFUSTR.FormattingEnabled = True
        Me.TOFUSTR.Items.AddRange(New Object() {"□", "■", "〓", "・"})
        Me.TOFUSTR.Location = New System.Drawing.Point(231, 81)
        Me.TOFUSTR.Name = "TOFUSTR"
        Me.TOFUSTR.Size = New System.Drawing.Size(34, 20)
        Me.TOFUSTR.TabIndex = 13
        Me.TOFUSTR.Text = "□"
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
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(264, 184)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(19, 23)
        Me.Button3.TabIndex = 16
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        Me.Button3.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 219)
        Me.Controls.Add(Me.sp)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TOFUSTR)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.EX)
        Me.Controls.Add(Me.ENCODE)
        Me.Controls.Add(Me.Label4)
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

End Class
