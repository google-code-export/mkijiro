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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.romname = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmdescription = New System.Windows.Forms.ComboBox()
        Me.cmname = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.codepage_select = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.alwaysencode = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.romname)
        Me.GroupBox1.Controls.Add(Me.cmdescription)
        Me.GroupBox1.Controls.Add(Me.cmname)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 26)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(307, 221)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CMPRO出力"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Location = New System.Drawing.Point(6, 108)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(294, 105)
        Me.Panel1.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(4, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 14)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "プレビュー"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(3, 21)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(288, 82)
        Me.TextBox1.TabIndex = 0
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.Location = New System.Drawing.Point(255, 79)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(45, 23)
        Me.Button4.TabIndex = 8
        Me.Button4.Text = "LIST"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(255, 49)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(45, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "LIST"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("MS UI Gothic", 7.854546!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(255, 23)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(46, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "LIST"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'romname
        '
        Me.romname.ContextMenuStrip = Me.ContextMenuStrip1
        Me.romname.FormattingEnabled = True
        Me.romname.Location = New System.Drawing.Point(89, 77)
        Me.romname.Name = "romname"
        Me.romname.Size = New System.Drawing.Size(161, 22)
        Me.romname.TabIndex = 5
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'cmdescription
        '
        Me.cmdescription.ContextMenuStrip = Me.ContextMenuStrip1
        Me.cmdescription.FormattingEnabled = True
        Me.cmdescription.Location = New System.Drawing.Point(89, 49)
        Me.cmdescription.Name = "cmdescription"
        Me.cmdescription.Size = New System.Drawing.Size(161, 22)
        Me.cmdescription.TabIndex = 4
        '
        'cmname
        '
        Me.cmname.ContextMenuStrip = Me.ContextMenuStrip1
        Me.cmname.FormattingEnabled = True
        Me.cmname.Location = New System.Drawing.Point(89, 24)
        Me.cmname.Name = "cmname"
        Me.cmname.Size = New System.Drawing.Size(161, 22)
        Me.cmname.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 82)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 14)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "romname"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 14)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "description"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "name"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.alwaysencode)
        Me.GroupBox2.Controls.Add(Me.codepage_select)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(18, 310)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(301, 88)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "CSV,TSV変換"
        '
        'codepage_select
        '
        Me.codepage_select.ContextMenuStrip = Me.ContextMenuStrip1
        Me.codepage_select.FormattingEnabled = True
        Me.codepage_select.Items.AddRange(New Object() {"ANSI(1252)", "Shift-JIS (932)", "GBK(936)", "EUC-JP(51932)", "JIS(50220)", "UTF-7(65000)", "UTF8N(65001)", "UTF16LE(1200)", "UTF16BE(1201)", "UTF32(12000)"})
        Me.codepage_select.Location = New System.Drawing.Point(154, 28)
        Me.codepage_select.Name = "codepage_select"
        Me.codepage_select.Size = New System.Drawing.Size(118, 22)
        Me.codepage_select.TabIndex = 1
        Me.codepage_select.Text = "Shift-JIS(932)"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(142, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "保存時のエンコーディング"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(124, 404)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(28, 250)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(253, 42)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "%t=タイトル名; %g=ゲームID(FILER_XMLのみ);" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "%c=CRC32;　　%f=ファイルサイズ " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※DOS文字 <>/\:|?*は使えません"
        '
        'alwaysencode
        '
        Me.alwaysencode.AutoSize = True
        Me.alwaysencode.Location = New System.Drawing.Point(13, 64)
        Me.alwaysencode.Name = "alwaysencode"
        Me.alwaysencode.Size = New System.Drawing.Size(210, 18)
        Me.alwaysencode.TabIndex = 2
        Me.alwaysencode.Text = "変換時常にエンコード設定を出す"
        Me.alwaysencode.UseVisualStyleBackColor = True
        '
        'Form4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(327, 425)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form4"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "XML"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents codepage_select As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents romname As System.Windows.Forms.ComboBox
    Friend WithEvents cmdescription As System.Windows.Forms.ComboBox
    Friend WithEvents cmname As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents alwaysencode As System.Windows.Forms.CheckBox
End Class
