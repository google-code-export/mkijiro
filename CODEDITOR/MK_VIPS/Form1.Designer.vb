﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.ASM = New System.Windows.Forms.TextBox()
        Me.MODE = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CODE = New System.Windows.Forms.TextBox()
        Me.ADDR = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.save2 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FairuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.open = New System.Windows.Forms.ToolStripMenuItem()
        Me.save = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.終了 = New System.Windows.Forms.ToolStripMenuItem()
        Me.enc = New System.Windows.Forms.ToolStripMenuItem()
        Me.バージョン = New System.Windows.Forms.ToolStripMenuItem()
        Me.フォント = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ASM
        '
        Me.ASM.AllowDrop = True
        Me.ASM.Font = New System.Drawing.Font("Verdana", 9.163636!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ASM.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ASM.Location = New System.Drawing.Point(21, 54)
        Me.ASM.Multiline = True
        Me.ASM.Name = "ASM"
        Me.ASM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ASM.Size = New System.Drawing.Size(253, 318)
        Me.ASM.TabIndex = 0
        '
        'MODE
        '
        Me.MODE.FormattingEnabled = True
        Me.MODE.Items.AddRange(New Object() {"NITEPR", "CWCHEAT", "JPCSP", "PSPAR", "PMETAN", "PSPAR(0xE)", "TEMPAR(0xC2)", "CMFUSION(0xF0)"})
        Me.MODE.Location = New System.Drawing.Point(15, 70)
        Me.MODE.Name = "MODE"
        Me.MODE.Size = New System.Drawing.Size(101, 20)
        Me.MODE.TabIndex = 1
        Me.MODE.Text = "NITEPR"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(138, 27)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(76, 41)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "ASM->コード変換"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'CODE
        '
        Me.CODE.Font = New System.Drawing.Font("Verdana", 9.163636!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CODE.Location = New System.Drawing.Point(291, 54)
        Me.CODE.Multiline = True
        Me.CODE.Name = "CODE"
        Me.CODE.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.CODE.Size = New System.Drawing.Size(354, 318)
        Me.CODE.TabIndex = 3
        '
        'ADDR
        '
        Me.ADDR.Location = New System.Drawing.Point(15, 27)
        Me.ADDR.MaxLength = 10
        Me.ADDR.Name = "ADDR"
        Me.ADDR.Size = New System.Drawing.Size(101, 19)
        Me.ADDR.TabIndex = 4
        Me.ADDR.Text = "0x1000"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "変換モード"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 12)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "開始アドレス"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ADDR)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.MODE)
        Me.Panel1.Location = New System.Drawing.Point(23, 376)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(228, 104)
        Me.Panel1.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 12)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "MIPS INST"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(289, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 12)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "OUTPUT"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.save2)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Location = New System.Drawing.Point(291, 389)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(354, 91)
        Me.Panel2.TabIndex = 10
        '
        'save2
        '
        Me.save2.Location = New System.Drawing.Point(275, 9)
        Me.save2.Name = "save2"
        Me.save2.Size = New System.Drawing.Size(76, 46)
        Me.save2.TabIndex = 1
        Me.save2.Text = "テキスト保存"
        Me.save2.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(3, 14)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(86, 41)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "コード->ASM変換"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FairuToolStripMenuItem, Me.enc, Me.フォント, Me.バージョン})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(675, 26)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FairuToolStripMenuItem
        '
        Me.FairuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.open, Me.save, Me.ToolStripSeparator1, Me.終了})
        Me.FairuToolStripMenuItem.Name = "FairuToolStripMenuItem"
        Me.FairuToolStripMenuItem.Size = New System.Drawing.Size(68, 22)
        Me.FairuToolStripMenuItem.Text = "ファイル"
        '
        'open
        '
        Me.open.Name = "open"
        Me.open.Size = New System.Drawing.Size(100, 22)
        Me.open.Text = "開く"
        '
        'save
        '
        Me.save.Name = "save"
        Me.save.Size = New System.Drawing.Size(100, 22)
        Me.save.Text = "保存"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(97, 6)
        '
        '終了
        '
        Me.終了.Name = "終了"
        Me.終了.Size = New System.Drawing.Size(100, 22)
        Me.終了.Text = "終了"
        '
        'enc
        '
        Me.enc.Name = "enc"
        Me.enc.Size = New System.Drawing.Size(80, 22)
        Me.enc.Text = "文字コード"
        '
        'バージョン
        '
        Me.バージョン.Name = "バージョン"
        Me.バージョン.Size = New System.Drawing.Size(80, 22)
        Me.バージョン.Text = "バージョン"
        '
        'フォント
        '
        Me.フォント.Name = "フォント"
        Me.フォント.Size = New System.Drawing.Size(68, 22)
        Me.フォント.Text = "フォント"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 490)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.CODE)
        Me.Controls.Add(Me.ASM)
        Me.Controls.Add(Me.MenuStrip1)
        Me.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MK_VIPS（＾ω＾）"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ASM As System.Windows.Forms.TextBox
    Friend WithEvents MODE As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CODE As System.Windows.Forms.TextBox
    Friend WithEvents ADDR As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FairuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents バージョン As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents open As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 終了 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents save As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents enc As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents save2 As System.Windows.Forms.Button
    Friend WithEvents フォント As System.Windows.Forms.ToolStripMenuItem

End Class
