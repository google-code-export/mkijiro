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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.unihex = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ファイルを開くToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OPENFILE = New System.Windows.Forms.ToolStripMenuItem()
        Me.SAVEAS = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.終了ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.文字コード = New System.Windows.Forms.ToolStripMenuItem()
        Me.SJIS = New System.Windows.Forms.ToolStripMenuItem()
        Me.EUC = New System.Windows.Forms.ToolStripMenuItem()
        Me.JIS = New System.Windows.Forms.ToolStripMenuItem()
        Me.JISX208 = New System.Windows.Forms.ToolStripMenuItem()
        Me.JIS83 = New System.Windows.Forms.ToolStripMenuItem()
        Me.JIS90 = New System.Windows.Forms.ToolStripMenuItem()
        Me.JIS2000 = New System.Windows.Forms.ToolStripMenuItem()
        Me.JIS2004 = New System.Windows.Forms.ToolStripMenuItem()
        Me.eucms = New System.Windows.Forms.ToolStripMenuItem()
        Me.BIG5HK = New System.Windows.Forms.ToolStripMenuItem()
        Me.USECUSTOM = New System.Windows.Forms.ToolStripMenuItem()
        Me.customtalbe = New System.Windows.Forms.ToolStripMenuItem()
        Me.SELCP = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.READ = New System.Windows.Forms.ToolStripMenuItem()
        Me.フォントToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.バージョンToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.AllowDrop = True
        Me.TextBox1.Font = New System.Drawing.Font("メイリオ", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(12, 29)
        Me.TextBox1.MaxLength = 0
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(397, 393)
        Me.TextBox1.TabIndex = 3
        '
        'unihex
        '
        Me.unihex.AutoSize = True
        Me.unihex.Location = New System.Drawing.Point(25, 425)
        Me.unihex.Name = "unihex"
        Me.unihex.Size = New System.Drawing.Size(141, 12)
        Me.unihex.TabIndex = 10
        Me.unihex.Text = " UTF-32:  UTF-16: UTF-8:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(357, 425)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(9, 12)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = " "
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ファイルを開くToolStripMenuItem, Me.文字コード, Me.フォントToolStripMenuItem, Me.バージョンToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(421, 26)
        Me.MenuStrip1.TabIndex = 13
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ファイルを開くToolStripMenuItem
        '
        Me.ファイルを開くToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OPENFILE, Me.SAVEAS, Me.ToolStripSeparator2, Me.終了ToolStripMenuItem})
        Me.ファイルを開くToolStripMenuItem.Name = "ファイルを開くToolStripMenuItem"
        Me.ファイルを開くToolStripMenuItem.Size = New System.Drawing.Size(68, 22)
        Me.ファイルを開くToolStripMenuItem.Text = "ファイル"
        '
        'OPENFILE
        '
        Me.OPENFILE.Name = "OPENFILE"
        Me.OPENFILE.Size = New System.Drawing.Size(172, 22)
        Me.OPENFILE.Text = "開く"
        '
        'SAVEAS
        '
        Me.SAVEAS.Name = "SAVEAS"
        Me.SAVEAS.Size = New System.Drawing.Size(172, 22)
        Me.SAVEAS.Text = "名前を付けて保存"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(169, 6)
        '
        '終了ToolStripMenuItem
        '
        Me.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem"
        Me.終了ToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.終了ToolStripMenuItem.Text = "終了"
        '
        '文字コード
        '
        Me.文字コード.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SJIS, Me.EUC, Me.JIS, Me.eucms, Me.BIG5HK, Me.USECUSTOM, Me.SELCP, Me.ToolStripSeparator1, Me.READ})
        Me.文字コード.Name = "文字コード"
        Me.文字コード.Size = New System.Drawing.Size(80, 22)
        Me.文字コード.Text = "文字コード"
        '
        'SJIS
        '
        Me.SJIS.Name = "SJIS"
        Me.SJIS.Size = New System.Drawing.Size(196, 22)
        Me.SJIS.Text = "Shift_JIS-2004"
        Me.SJIS.ToolTipText = resources.GetString("SJIS.ToolTipText")
        '
        'EUC
        '
        Me.EUC.Name = "EUC"
        Me.EUC.Size = New System.Drawing.Size(196, 22)
        Me.EUC.Text = "EUC-JIS-2004"
        Me.EUC.ToolTipText = "JISX0213規格のEUC-JP文字コードです,JISコードに以下の数値を追加" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS201(半角カナ) +0x8E00" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS1面 +0x8080" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JI" & _
    "S2面 +0x8F8080"
        '
        'JIS
        '
        Me.JIS.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JISX208})
        Me.JIS.Name = "JIS"
        Me.JIS.Size = New System.Drawing.Size(196, 22)
        Me.JIS.Text = "ISO-2022-JP-2004"
        Me.JIS.ToolTipText = "JISX0213規格のJIS文字コードです、エスケープ＋特殊パターンでモードが切り替わります" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1B 28 42: ASCII (厳密にはISO/IEC 646 " & _
    "国際基準版)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1B 24 28 51: JIS X 0213第1面" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1B 24 28 50: JIS X 0213第2面" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1b 24 28 4f :JIS" & _
    " X 0213:2000 1面 " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'JISX208
        '
        Me.JISX208.Checked = True
        Me.JISX208.CheckState = System.Windows.Forms.CheckState.Checked
        Me.JISX208.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JIS83, Me.JIS90, Me.JIS2000, Me.JIS2004})
        Me.JISX208.Name = "JISX208"
        Me.JISX208.Size = New System.Drawing.Size(183, 22)
        Me.JISX208.Text = "JISX208互換モード"
        Me.JISX208.ToolTipText = "JISX208のエスケープシーケンスを読みとるようにします" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS X 0208-1983(JIS83) 1b 24 42 ESC $ B" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS X 020" & _
    "8-1990(JIS90) 1b 26 40 1b 24 42 ESC & @ ESC $ B" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'JIS83
        '
        Me.JIS83.Name = "JIS83"
        Me.JIS83.Size = New System.Drawing.Size(122, 22)
        Me.JIS83.Text = "JIS83"
        Me.JIS83.ToolTipText = "jis90追加文字(凜熙)の出力方法をJISX208-1983にします(jis83規格外)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS X 0208-1983 1b 24 42 ESC $ B" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
    "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'JIS90
        '
        Me.JIS90.Name = "JIS90"
        Me.JIS90.Size = New System.Drawing.Size(122, 22)
        Me.JIS90.Text = "JIS90"
        Me.JIS90.ToolTipText = "jis90追加文字(凜熙)の出力方法をJISX208-1990にします(iso2022規格外)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS X 0208-1990 1b 26 40 1b 24 4" & _
    "2 ESC & @ ESC $ B " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'JIS2000
        '
        Me.JIS2000.Name = "JIS2000"
        Me.JIS2000.Size = New System.Drawing.Size(122, 22)
        Me.JIS2000.Text = "JIS2000"
        Me.JIS2000.ToolTipText = "jis90追加文字(凜熙)の出力方法をJISX213-2000にします" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS X 0213:2000 1面 1b 24 28 4f " & Global.Microsoft.VisualBasic.ChrW(9) & "ESC $ ( O"
        '
        'JIS2004
        '
        Me.JIS2004.Checked = True
        Me.JIS2004.CheckState = System.Windows.Forms.CheckState.Checked
        Me.JIS2004.Name = "JIS2004"
        Me.JIS2004.Size = New System.Drawing.Size(122, 22)
        Me.JIS2004.Text = "JIS2004"
        Me.JIS2004.ToolTipText = "jis90追加文字(凜熙)の出力方法をJISX213-2004にします" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "JIS X 0213:2004 1面 1b 24 28 51 ESC $ ( Q"
        '
        'eucms
        '
        Me.eucms.Name = "eucms"
        Me.eucms.Size = New System.Drawing.Size(196, 22)
        Me.eucms.Text = "eucJP-ms"
        Me.eucms.ToolTipText = resources.GetString("eucms.ToolTipText")
        '
        'BIG5HK
        '
        Me.BIG5HK.Name = "BIG5HK"
        Me.BIG5HK.Size = New System.Drawing.Size(196, 22)
        Me.BIG5HK.Text = "Big5-HKSCS"
        Me.BIG5HK.ToolTipText = "台湾繁体中国語BIG5に香港增補字符集 Hong Kong Supplementary Character Set (HKSCS)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "を追加した文字コードです。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "http://www.microsoft.com/hk/hkscs/code/HKSCS.htm"
        '
        'USECUSTOM
        '
        Me.USECUSTOM.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.customtalbe})
        Me.USECUSTOM.Name = "USECUSTOM"
        Me.USECUSTOM.Size = New System.Drawing.Size(196, 22)
        Me.USECUSTOM.Text = "カスタムテーブル使用"
        '
        'customtalbe
        '
        Me.customtalbe.Name = "customtalbe"
        Me.customtalbe.Size = New System.Drawing.Size(148, 22)
        Me.customtalbe.Text = "テーブル指定"
        Me.customtalbe.ToolTipText = "UNICODE←→LOCAL 変換時のテーブルを指定します"
        '
        'SELCP
        '
        Me.SELCP.Name = "SELCP"
        Me.SELCP.Size = New System.Drawing.Size(196, 22)
        Me.SELCP.Text = "コードページ指定"
        Me.SELCP.ToolTipText = "M$内臓のコードページ（UNICODE変換テーブル＋プログラム）を選択します"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(193, 6)
        '
        'READ
        '
        Me.READ.Name = "READ"
        Me.READ.Size = New System.Drawing.Size(196, 22)
        Me.READ.Text = "読み込みテスト"
        Me.READ.ToolTipText = "SJIS/EUC/ISO2022JP2004,BIG5HKのサンプルTXTファイルを読み込みます"
        '
        'フォントToolStripMenuItem
        '
        Me.フォントToolStripMenuItem.Name = "フォントToolStripMenuItem"
        Me.フォントToolStripMenuItem.Size = New System.Drawing.Size(68, 22)
        Me.フォントToolStripMenuItem.Text = "フォント"
        '
        'バージョンToolStripMenuItem
        '
        Me.バージョンToolStripMenuItem.Name = "バージョンToolStripMenuItem"
        Me.バージョンToolStripMenuItem.Size = New System.Drawing.Size(80, 22)
        Me.バージョンToolStripMenuItem.Text = "バージョン"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 442)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.unihex)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("Location", Global.JISX0213NETA.My.MySettings.Default, "pos", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Location = Global.JISX0213NETA.My.MySettings.Default.pos
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximumSize = New System.Drawing.Size(437, 480)
        Me.MinimumSize = New System.Drawing.Size(437, 480)
        Me.Name = "Form1"
        Me.Text = "JIS0213_TXTえでぃた"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents unihex As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ファイルを開くToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 文字コード As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents フォントToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SJIS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EUC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BIG5HK As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OPENFILE As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SAVEAS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SELCP As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents READ As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 終了ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JIS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents バージョンToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JISX208 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents eucms As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JIS83 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JIS90 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JIS2000 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JIS2004 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents USECUSTOM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents customtalbe As System.Windows.Forms.ToolStripMenuItem

End Class
