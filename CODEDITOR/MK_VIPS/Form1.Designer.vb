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
        Me.ASM = New System.Windows.Forms.TextBox()
        Me.MODE = New System.Windows.Forms.ComboBox()
        Me.cvt_asm2code = New System.Windows.Forms.Button()
        Me.CODE = New System.Windows.Forms.TextBox()
        Me.ADDR = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ASMSB = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.save2 = New System.Windows.Forms.Button()
        Me.cnt_code2asm = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FairuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.open = New System.Windows.Forms.ToolStripMenuItem()
        Me.ASMopen = New System.Windows.Forms.ToolStripMenuItem()
        Me.CODEopen = New System.Windows.Forms.ToolStripMenuItem()
        Me.save = New System.Windows.Forms.ToolStripMenuItem()
        Me.ASM保存ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.コード保存ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.終了 = New System.Windows.Forms.ToolStripMenuItem()
        Me.enc = New System.Windows.Forms.ToolStripMenuItem()
        Me.フォント = New System.Windows.Forms.ToolStripMenuItem()
        Me.設定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RPN = New System.Windows.Forms.ToolStripMenuItem()
        Me.CVTRPN = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.STACKORDER = New System.Windows.Forms.ToolStripMenuItem()
        Me.LOOKSORDER = New System.Windows.Forms.ToolStripMenuItem()
        Me.サブルーチン仮アドレスToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.バージョン = New System.Windows.Forms.ToolStripMenuItem()
        Me.savea = New System.Windows.Forms.Button()
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
        Me.ASM.MaxLength = 0
        Me.ASM.Multiline = True
        Me.ASM.Name = "ASM"
        Me.ASM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ASM.Size = New System.Drawing.Size(253, 318)
        Me.ASM.TabIndex = 0
        '
        'MODE
        '
        Me.MODE.FormattingEnabled = True
        Me.MODE.Items.AddRange(New Object() {"NITEPR", "CWCHEAT", "PSPAR", "PMETAN", "CWCPOPS", "PSPAR(0xE)", "TEMPAR(0xC2)", "CMFUSION(0xF0)", "CMFUSION(0xF0,ENC1)", "CMFUSION(0xF0,ENC2)"})
        Me.MODE.Location = New System.Drawing.Point(15, 70)
        Me.MODE.Name = "MODE"
        Me.MODE.Size = New System.Drawing.Size(101, 20)
        Me.MODE.TabIndex = 1
        Me.MODE.Text = "NITEPR"
        '
        'cvt_asm2code
        '
        Me.cvt_asm2code.Location = New System.Drawing.Point(154, 13)
        Me.cvt_asm2code.Name = "cvt_asm2code"
        Me.cvt_asm2code.Size = New System.Drawing.Size(76, 41)
        Me.cvt_asm2code.TabIndex = 2
        Me.cvt_asm2code.Text = "ASM->コード変換"
        Me.cvt_asm2code.UseVisualStyleBackColor = True
        '
        'CODE
        '
        Me.CODE.AllowDrop = True
        Me.CODE.Font = New System.Drawing.Font("Verdana", 9.163636!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CODE.Location = New System.Drawing.Point(291, 54)
        Me.CODE.MaxLength = 0
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
        Me.Panel1.Controls.Add(Me.ASMSB)
        Me.Panel1.Controls.Add(Me.ADDR)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cvt_asm2code)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.MODE)
        Me.Panel1.Location = New System.Drawing.Point(23, 376)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(251, 102)
        Me.Panel1.TabIndex = 7
        '
        'ASMSB
        '
        Me.ASMSB.Location = New System.Drawing.Point(154, 68)
        Me.ASMSB.Name = "ASMSB"
        Me.ASMSB.Size = New System.Drawing.Size(75, 23)
        Me.ASMSB.TabIndex = 7
        Me.ASMSB.Text = "ASM保存"
        Me.ASMSB.UseVisualStyleBackColor = True
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
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.save2)
        Me.Panel2.Controls.Add(Me.cnt_code2asm)
        Me.Panel2.Location = New System.Drawing.Point(291, 378)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(354, 100)
        Me.Panel2.TabIndex = 10
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(172, 11)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(73, 41)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "_LMN除去"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'save2
        '
        Me.save2.Location = New System.Drawing.Point(265, 11)
        Me.save2.Name = "save2"
        Me.save2.Size = New System.Drawing.Size(76, 41)
        Me.save2.TabIndex = 1
        Me.save2.Text = "コード保存"
        Me.save2.UseVisualStyleBackColor = True
        '
        'cnt_code2asm
        '
        Me.cnt_code2asm.Location = New System.Drawing.Point(3, 11)
        Me.cnt_code2asm.Name = "cnt_code2asm"
        Me.cnt_code2asm.Size = New System.Drawing.Size(86, 41)
        Me.cnt_code2asm.TabIndex = 0
        Me.cnt_code2asm.Text = "コード->ASM変換"
        Me.cnt_code2asm.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FairuToolStripMenuItem, Me.enc, Me.フォント, Me.設定ToolStripMenuItem, Me.バージョン})
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
        Me.open.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ASMopen, Me.CODEopen})
        Me.open.Name = "open"
        Me.open.Size = New System.Drawing.Size(100, 22)
        Me.open.Text = "開く"
        '
        'ASMopen
        '
        Me.ASMopen.Name = "ASMopen"
        Me.ASMopen.Size = New System.Drawing.Size(109, 22)
        Me.ASMopen.Text = "ASM"
        '
        'CODEopen
        '
        Me.CODEopen.Name = "CODEopen"
        Me.CODEopen.Size = New System.Drawing.Size(109, 22)
        Me.CODEopen.Text = "CODE"
        '
        'save
        '
        Me.save.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ASM保存ToolStripMenuItem, Me.コード保存ToolStripMenuItem})
        Me.save.Name = "save"
        Me.save.Size = New System.Drawing.Size(100, 22)
        Me.save.Text = "保存"
        '
        'ASM保存ToolStripMenuItem
        '
        Me.ASM保存ToolStripMenuItem.Name = "ASM保存ToolStripMenuItem"
        Me.ASM保存ToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.ASM保存ToolStripMenuItem.Text = "ASM保存"
        '
        'コード保存ToolStripMenuItem
        '
        Me.コード保存ToolStripMenuItem.Name = "コード保存ToolStripMenuItem"
        Me.コード保存ToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.コード保存ToolStripMenuItem.Text = "コード保存"
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
        'フォント
        '
        Me.フォント.Name = "フォント"
        Me.フォント.Size = New System.Drawing.Size(68, 22)
        Me.フォント.Text = "フォント"
        '
        '設定ToolStripMenuItem
        '
        Me.設定ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RPN, Me.サブルーチン仮アドレスToolStripMenuItem})
        Me.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem"
        Me.設定ToolStripMenuItem.Size = New System.Drawing.Size(44, 22)
        Me.設定ToolStripMenuItem.Text = "設定"
        '
        'RPN
        '
        Me.RPN.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CVTRPN, Me.ToolStripSeparator2, Me.LOOKSORDER, Me.STACKORDER})
        Me.RPN.Name = "RPN"
        Me.RPN.Size = New System.Drawing.Size(208, 22)
        Me.RPN.Text = "FLOAT RPNモード"
        Me.RPN.ToolTipText = "逆ポーランド記法で複数の式を処理して単精度浮動小数点数を出力します。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "通常モード時は単体式のみ対応" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "通常モード;tan(45度) " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RPN式;9,2,3,*" & _
    ",6,*,9,+,tan"
        '
        'CVTRPN
        '
        Me.CVTRPN.Name = "CVTRPN"
        Me.CVTRPN.Size = New System.Drawing.Size(172, 22)
        Me.CVTRPN.Text = "数式をRPNに変換"
        Me.CVTRPN.ToolTipText = "数式をRPN式に変換して処理します" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "数式;tan(9+(2*3)*6)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "↓" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "RPN式;9,2,3,*,6,*,9,+,tan"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(193, 6)
        '
        'STACKORDER
        '
        Me.STACKORDER.Name = "STACKORDER"
        Me.STACKORDER.Size = New System.Drawing.Size(180, 22)
        Me.STACKORDER.Text = "②,① スタック降順"
        Me.STACKORDER.ToolTipText = "関数引数順番がスタック降順になります;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "pow(①,②)→②,①,pow" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logx(①,②)→②,①,logx" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logy(①,②)→②,①,logy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "at" & _
    "an2_(①,②)→②,①,atan2_" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "atan2ms_(①,②)→②,①,atan2ms_"
        '
        'LOOKSORDER
        '
        Me.LOOKSORDER.Checked = True
        Me.LOOKSORDER.CheckState = System.Windows.Forms.CheckState.Checked
        Me.LOOKSORDER.Name = "LOOKSORDER"
        Me.LOOKSORDER.Size = New System.Drawing.Size(180, 22)
        Me.LOOKSORDER.Text = "①,② スタック昇順"
        Me.LOOKSORDER.ToolTipText = "関数引数順番がスタック昇順になります" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "pow(①,②)→①,②,pow" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logx(①,②)→①,②,logx" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "logy(①,②)→①,②,logy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ata" & _
    "n2_(①,②)→①,②,atan2_" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "atan2ms_(①,②)→①,②,atan2ms_"
        '
        'サブルーチン仮アドレスToolStripMenuItem
        '
        Me.サブルーチン仮アドレスToolStripMenuItem.Name = "サブルーチン仮アドレスToolStripMenuItem"
        Me.サブルーチン仮アドレスToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.サブルーチン仮アドレスToolStripMenuItem.Text = "サブルーチン仮アドレス"
        '
        'バージョン
        '
        Me.バージョン.Name = "バージョン"
        Me.バージョン.Size = New System.Drawing.Size(80, 22)
        Me.バージョン.Text = "バージョン"
        '
        'savea
        '
        Me.savea.Location = New System.Drawing.Point(140, 70)
        Me.savea.Name = "savea"
        Me.savea.Size = New System.Drawing.Size(75, 23)
        Me.savea.TabIndex = 7
        Me.savea.Text = "ASM保存"
        Me.savea.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 488)
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
    Friend WithEvents cvt_asm2code As System.Windows.Forms.Button
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
    Friend WithEvents cnt_code2asm As System.Windows.Forms.Button
    Friend WithEvents open As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 終了 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents save As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents enc As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents save2 As System.Windows.Forms.Button
    Friend WithEvents フォント As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ASM保存ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents コード保存ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents savea As System.Windows.Forms.Button
    Friend WithEvents ASMSB As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents 設定ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ASMopen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CODEopen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RPN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents サブルーチン仮アドレスToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents STACKORDER As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LOOKSORDER As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CVTRPN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator

End Class
