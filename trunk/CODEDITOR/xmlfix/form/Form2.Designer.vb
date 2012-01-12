<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.USEXML = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.romname = New System.Windows.Forms.ComboBox()
        Me.cmdescription = New System.Windows.Forms.ComboBox()
        Me.cmname = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CSV_TSV = New System.Windows.Forms.GroupBox()
        Me.crcblock = New System.Windows.Forms.CheckBox()
        Me.cvt_country_lang = New System.Windows.Forms.CheckBox()
        Me.alwaysencode = New System.Windows.Forms.CheckBox()
        Me.codepage_select = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.edit_lang = New System.Windows.Forms.Button()
        Me.cs_lang = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.edit_country = New System.Windows.Forms.Button()
        Me.cs_country = New System.Windows.Forms.ComboBox()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.Panel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.CSV_TSV.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(138, 400)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.GroupBox3)
        Me.Panel2.Location = New System.Drawing.Point(8, 4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(315, 349)
        Me.Panel2.TabIndex = 9
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 230)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(220, 112)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "%t=タイトル名; %p=販売元;　　" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "%c=CRC32; %f=ファイルサイズ ;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "%o=国; %a=対応言語; %m=コメント;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "--これより下はFIL" & _
    "ER_XMLのみ対応--" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "%r=発売日;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "%b=ボックスID;　%g=ゲームID;%" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "%w=要求FW;　%v=ディスクバージョン;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "※DOS文字 <>/" & _
    "\:|?*は使えません"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Panel3)
        Me.GroupBox3.Controls.Add(Me.Button4)
        Me.GroupBox3.Controls.Add(Me.Button3)
        Me.GroupBox3.Controls.Add(Me.Button2)
        Me.GroupBox3.Controls.Add(Me.romname)
        Me.GroupBox3.Controls.Add(Me.cmdescription)
        Me.GroupBox3.Controls.Add(Me.cmname)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(307, 224)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "CMPRO出力"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.USEXML)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.TextBox3)
        Me.Panel3.Location = New System.Drawing.Point(6, 108)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(294, 110)
        Me.Panel3.TabIndex = 4
        '
        'USEXML
        '
        Me.USEXML.AutoSize = True
        Me.USEXML.Location = New System.Drawing.Point(69, 3)
        Me.USEXML.Name = "USEXML"
        Me.USEXML.Size = New System.Drawing.Size(178, 18)
        Me.USEXML.TabIndex = 2
        Me.USEXML.Text = "最後に使用したXMLファイル"
        Me.USEXML.UseVisualStyleBackColor = True
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
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(3, 21)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox3.Size = New System.Drawing.Size(288, 86)
        Me.TextBox3.TabIndex = 0
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
        Me.romname.FormattingEnabled = True
        Me.romname.Location = New System.Drawing.Point(89, 77)
        Me.romname.Name = "romname"
        Me.romname.Size = New System.Drawing.Size(161, 22)
        Me.romname.TabIndex = 5
        Me.romname.Text = "%t"
        '
        'cmdescription
        '
        Me.cmdescription.FormattingEnabled = True
        Me.cmdescription.Location = New System.Drawing.Point(89, 49)
        Me.cmdescription.Name = "cmdescription"
        Me.cmdescription.Size = New System.Drawing.Size(161, 22)
        Me.cmdescription.TabIndex = 4
        Me.cmdescription.Text = "%m"
        '
        'cmname
        '
        Me.cmname.FormattingEnabled = True
        Me.cmname.Location = New System.Drawing.Point(89, 24)
        Me.cmname.Name = "cmname"
        Me.cmname.Size = New System.Drawing.Size(161, 22)
        Me.cmname.TabIndex = 3
        Me.cmname.Text = "%t"
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
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 14)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "description"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 27)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(38, 14)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "name"
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(329, 349)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "XML変換"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CSV_TSV)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextBox2)
        Me.Panel1.Location = New System.Drawing.Point(3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(313, 342)
        Me.Panel1.TabIndex = 7
        '
        'CSV_TSV
        '
        Me.CSV_TSV.Controls.Add(Me.crcblock)
        Me.CSV_TSV.Controls.Add(Me.cvt_country_lang)
        Me.CSV_TSV.Controls.Add(Me.alwaysencode)
        Me.CSV_TSV.Controls.Add(Me.codepage_select)
        Me.CSV_TSV.Controls.Add(Me.Label3)
        Me.CSV_TSV.Location = New System.Drawing.Point(15, 180)
        Me.CSV_TSV.Name = "CSV_TSV"
        Me.CSV_TSV.Size = New System.Drawing.Size(285, 159)
        Me.CSV_TSV.TabIndex = 8
        Me.CSV_TSV.TabStop = False
        Me.CSV_TSV.Text = "CSV,TSV変換"
        '
        'crcblock
        '
        Me.crcblock.AutoSize = True
        Me.crcblock.Location = New System.Drawing.Point(13, 135)
        Me.crcblock.Name = "crcblock"
        Me.crcblock.Size = New System.Drawing.Size(124, 18)
        Me.crcblock.TabIndex = 9
        Me.crcblock.Text = "CRC32を[]で囲む"
        Me.crcblock.UseVisualStyleBackColor = True
        '
        'cvt_country_lang
        '
        Me.cvt_country_lang.AutoSize = True
        Me.cvt_country_lang.Location = New System.Drawing.Point(13, 99)
        Me.cvt_country_lang.Name = "cvt_country_lang"
        Me.cvt_country_lang.Size = New System.Drawing.Size(211, 18)
        Me.cvt_country_lang.TabIndex = 3
        Me.cvt_country_lang.Text = "国,言語の数字を文字に変換する"
        Me.cvt_country_lang.UseVisualStyleBackColor = True
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
        'codepage_select
        '
        Me.codepage_select.FormattingEnabled = True
        Me.codepage_select.Items.AddRange(New Object() {"ANSI(1252)", "Shift-JIS (932)", "GBK(936)", "EUC-JP(51932)", "JIS(50220)", "UTF-7(65000)", "UTF8N(65001)", "UTF16LE(1200)", "UTF16BE(1201)", "UTF32(12000)"})
        Me.codepage_select.Location = New System.Drawing.Point(154, 28)
        Me.codepage_select.Name = "codepage_select"
        Me.codepage_select.Size = New System.Drawing.Size(118, 22)
        Me.codepage_select.TabIndex = 1
        Me.codepage_select.Text = "Shift-JIS(932)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(142, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "保存時のエンコーディング"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(194, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "NOINTRO用CRC正規表現マスク"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 112)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 49)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CM変換用に使用するマスク"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(110, 20)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(79, 18)
        Me.RadioButton2.TabIndex = 6
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "REDUMP"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(21, 20)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(83, 18)
        Me.RadioButton1.TabIndex = 5
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "NOINTRO"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox1.Location = New System.Drawing.Point(15, 34)
        Me.TextBox1.MaxLength = 77
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(227, 21)
        Me.TextBox1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(190, 14)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "REDUMP用CRC正規表現マスク"
        '
        'TextBox2
        '
        Me.TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox2.Location = New System.Drawing.Point(15, 75)
        Me.TextBox2.MaxLength = 77
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(227, 21)
        Me.TextBox2.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(13, 13)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(337, 377)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage2.Controls.Add(Me.Panel2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(329, 349)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "CMPRO変換"
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TabPage3.Controls.Add(Me.Panel4)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(329, 349)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "XML国/言語置換"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.GroupBox5)
        Me.Panel4.Controls.Add(Me.GroupBox4)
        Me.Panel4.Location = New System.Drawing.Point(0, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(331, 352)
        Me.Panel4.TabIndex = 9
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.edit_lang)
        Me.GroupBox5.Controls.Add(Me.cs_lang)
        Me.GroupBox5.Controls.Add(Me.RadioButton6)
        Me.GroupBox5.Controls.Add(Me.RadioButton5)
        Me.GroupBox5.Location = New System.Drawing.Point(24, 162)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(299, 95)
        Me.GroupBox5.TabIndex = 1
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "対応言語の文字"
        '
        'edit_lang
        '
        Me.edit_lang.Location = New System.Drawing.Point(248, 47)
        Me.edit_lang.Name = "edit_lang"
        Me.edit_lang.Size = New System.Drawing.Size(45, 23)
        Me.edit_lang.TabIndex = 3
        Me.edit_lang.Text = "編集"
        Me.edit_lang.UseVisualStyleBackColor = True
        '
        'cs_lang
        '
        Me.cs_lang.ContextMenuStrip = Me.ContextMenuStrip1
        Me.cs_lang.FormattingEnabled = True
        Me.cs_lang.Location = New System.Drawing.Point(121, 48)
        Me.cs_lang.Name = "cs_lang"
        Me.cs_lang.Size = New System.Drawing.Size(121, 22)
        Me.cs_lang.TabIndex = 2
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(19, 49)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(88, 18)
        Me.RadioButton6.TabIndex = 1
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.Text = "カスタマイズ"
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Checked = True
        Me.RadioButton5.Location = New System.Drawing.Point(19, 20)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(89, 18)
        Me.RadioButton5.TabIndex = 0
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "標準(英語)"
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.edit_country)
        Me.GroupBox4.Controls.Add(Me.cs_country)
        Me.GroupBox4.Controls.Add(Me.RadioButton4)
        Me.GroupBox4.Controls.Add(Me.RadioButton3)
        Me.GroupBox4.Location = New System.Drawing.Point(24, 26)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(299, 102)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "国(国旗)の文字"
        '
        'edit_country
        '
        Me.edit_country.Location = New System.Drawing.Point(249, 53)
        Me.edit_country.Name = "edit_country"
        Me.edit_country.Size = New System.Drawing.Size(44, 23)
        Me.edit_country.TabIndex = 3
        Me.edit_country.Text = "編集"
        Me.edit_country.UseVisualStyleBackColor = True
        '
        'cs_country
        '
        Me.cs_country.ContextMenuStrip = Me.ContextMenuStrip1
        Me.cs_country.FormattingEnabled = True
        Me.cs_country.Location = New System.Drawing.Point(121, 55)
        Me.cs_country.Name = "cs_country"
        Me.cs_country.Size = New System.Drawing.Size(121, 22)
        Me.cs_country.TabIndex = 2
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(19, 56)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(88, 18)
        Me.RadioButton4.TabIndex = 1
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "カスタマイズ"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(19, 32)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(89, 18)
        Me.RadioButton3.TabIndex = 0
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "標準(英語)"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 435)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "変換時の出力"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.CSV_TSV.ResumeLayout(False)
        Me.CSV_TSV.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents romname As System.Windows.Forms.ComboBox
    Friend WithEvents cmdescription As System.Windows.Forms.ComboBox
    Friend WithEvents cmname As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CSV_TSV As System.Windows.Forms.GroupBox
    Friend WithEvents alwaysencode As System.Windows.Forms.CheckBox
    Friend WithEvents codepage_select As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents USEXML As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents cs_country As System.Windows.Forms.ComboBox
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents cs_lang As System.Windows.Forms.ComboBox
    Friend WithEvents edit_country As System.Windows.Forms.Button
    Friend WithEvents edit_lang As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cvt_country_lang As System.Windows.Forms.CheckBox
    Friend WithEvents crcblock As System.Windows.Forms.CheckBox
End Class
