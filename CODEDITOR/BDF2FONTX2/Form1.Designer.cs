namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.CMFUSION = new System.Windows.Forms.GroupBox();
            this.CMGBK = new System.Windows.Forms.RadioButton();
            this.FC = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.FILER = new System.Windows.Forms.RadioButton();
            this.CMF = new System.Windows.Forms.RadioButton();
            this.EUC = new System.Windows.Forms.RadioButton();
            this.SJIS = new System.Windows.Forms.RadioButton();
            this.NOREMAP = new System.Windows.Forms.RadioButton();
            this.CP = new System.Windows.Forms.RadioButton();
            this.codepage = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sekitxt = new System.Windows.Forms.ComboBox();
            this.pos = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SEIKI = new System.Windows.Forms.CheckBox();
            this.JIS2SJIS = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.CMFUSION.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(52, 296);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "変換";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CMFUSION
            // 
            this.CMFUSION.Controls.Add(this.CMGBK);
            this.CMFUSION.Controls.Add(this.FC);
            this.CMFUSION.Controls.Add(this.radioButton1);
            this.CMFUSION.Controls.Add(this.FILER);
            this.CMFUSION.Controls.Add(this.CMF);
            this.CMFUSION.Location = new System.Drawing.Point(30, 171);
            this.CMFUSION.Name = "CMFUSION";
            this.CMFUSION.Size = new System.Drawing.Size(184, 119);
            this.CMFUSION.TabIndex = 5;
            this.CMFUSION.TabStop = false;
            this.CMFUSION.Text = "圧縮ふぉんと作成";
            // 
            // CMGBK
            // 
            this.CMGBK.AutoSize = true;
            this.CMGBK.Location = new System.Drawing.Point(22, 79);
            this.CMGBK.Name = "CMGBK";
            this.CMGBK.Size = new System.Drawing.Size(124, 16);
            this.CMGBK.TabIndex = 9;
            this.CMGBK.TabStop = true;
            this.CMGBK.Text = "CMF修正GBK12x12";
            this.CMGBK.UseVisualStyleBackColor = true;
            // 
            // FC
            // 
            this.FC.AutoSize = true;
            this.FC.Location = new System.Drawing.Point(22, 57);
            this.FC.Name = "FC";
            this.FC.Size = new System.Drawing.Size(122, 16);
            this.FC.TabIndex = 8;
            this.FC.TabStop = true;
            this.FC.Text = "FC/CMFSJIS12x12";
            this.FC.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(22, 18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(126, 16);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "無圧縮(通常ラスター)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // FILER
            // 
            this.FILER.AutoSize = true;
            this.FILER.Location = new System.Drawing.Point(22, 101);
            this.FILER.Name = "FILER";
            this.FILER.Size = new System.Drawing.Size(138, 16);
            this.FILER.TabIndex = 6;
            this.FILER.TabStop = true;
            this.FILER.Text = "FILER互換半全角6x12";
            this.FILER.UseVisualStyleBackColor = true;
            this.FILER.CheckedChanged += new System.EventHandler(this.FILER_CheckedChanged);
            // 
            // CMF
            // 
            this.CMF.AutoSize = true;
            this.CMF.Location = new System.Drawing.Point(22, 36);
            this.CMF.Name = "CMF";
            this.CMF.Size = new System.Drawing.Size(104, 16);
            this.CMF.TabIndex = 5;
            this.CMF.TabStop = true;
            this.CMF.Text = "CMF_EUC12x12";
            this.CMF.UseVisualStyleBackColor = true;
            this.CMF.CheckedChanged += new System.EventHandler(this.CMF_CheckedChanged_1);
            // 
            // EUC
            // 
            this.EUC.AutoSize = true;
            this.EUC.Location = new System.Drawing.Point(5, 38);
            this.EUC.Name = "EUC";
            this.EUC.Size = new System.Drawing.Size(75, 16);
            this.EUC.TabIndex = 2;
            this.EUC.Text = "JIS→EUC";
            this.EUC.UseVisualStyleBackColor = true;
            // 
            // SJIS
            // 
            this.SJIS.AutoSize = true;
            this.SJIS.Checked = true;
            this.SJIS.Location = new System.Drawing.Point(79, 17);
            this.SJIS.Name = "SJIS";
            this.SJIS.Size = new System.Drawing.Size(76, 16);
            this.SJIS.TabIndex = 1;
            this.SJIS.TabStop = true;
            this.SJIS.Text = "JIS→SJIS";
            this.SJIS.UseVisualStyleBackColor = true;
            this.SJIS.CheckedChanged += new System.EventHandler(this.SJIS_CheckedChanged);
            // 
            // NOREMAP
            // 
            this.NOREMAP.AutoSize = true;
            this.NOREMAP.Location = new System.Drawing.Point(5, 17);
            this.NOREMAP.Name = "NOREMAP";
            this.NOREMAP.Size = new System.Drawing.Size(59, 16);
            this.NOREMAP.TabIndex = 3;
            this.NOREMAP.TabStop = true;
            this.NOREMAP.Text = "無変換";
            this.NOREMAP.UseVisualStyleBackColor = true;
            // 
            // CP
            // 
            this.CP.AutoSize = true;
            this.CP.Location = new System.Drawing.Point(82, 38);
            this.CP.Name = "CP";
            this.CP.Size = new System.Drawing.Size(67, 16);
            this.CP.TabIndex = 4;
            this.CP.TabStop = true;
            this.CP.Text = "JIS→CP";
            this.CP.UseVisualStyleBackColor = true;
            // 
            // codepage
            // 
            this.codepage.ContextMenuStrip = this.contextMenuStrip1;
            this.codepage.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.codepage.Location = new System.Drawing.Point(148, 37);
            this.codepage.MaxLength = 5;
            this.codepage.Name = "codepage";
            this.codepage.Size = new System.Drawing.Size(41, 19);
            this.codepage.TabIndex = 5;
            this.codepage.Text = "936";
            this.codepage.TextChanged += new System.EventHandler(this.codepage_TextChanged);
            this.codepage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.codepage_KeyPress);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.codepage);
            this.groupBox1.Controls.Add(this.CP);
            this.groupBox1.Controls.Add(this.NOREMAP);
            this.groupBox1.Controls.Add(this.SJIS);
            this.groupBox1.Controls.Add(this.EUC);
            this.groupBox1.Location = new System.Drawing.Point(25, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 60);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FONTX2文字テーブル";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sekitxt);
            this.groupBox2.Controls.Add(this.pos);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.SEIKI);
            this.groupBox2.Controls.Add(this.JIS2SJIS);
            this.groupBox2.Location = new System.Drawing.Point(25, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 90);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "JIS->SJIS変換(JISこーど=0xS1S2)";
            // 
            // sekitxt
            // 
            this.sekitxt.ContextMenuStrip = this.contextMenuStrip1;
            this.sekitxt.FormattingEnabled = true;
            this.sekitxt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.sekitxt.Items.AddRange(new object[] {
            "\\n0x[0-9A-F]{4}\\t0x%J",
            "%J [0-9A-Fa-f]{4}"});
            this.sekitxt.Location = new System.Drawing.Point(85, 38);
            this.sekitxt.Name = "sekitxt";
            this.sekitxt.Size = new System.Drawing.Size(116, 20);
            this.sekitxt.TabIndex = 11;
            this.sekitxt.SelectedIndexChanged += new System.EventHandler(this.sekitxt_SelectedIndexChanged);
            // 
            // pos
            // 
            this.pos.Enabled = false;
            this.pos.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.pos.Location = new System.Drawing.Point(87, 62);
            this.pos.MaxLength = 1;
            this.pos.Name = "pos";
            this.pos.Size = new System.Drawing.Size(28, 19);
            this.pos.TabIndex = 10;
            this.pos.Text = "3";
            this.pos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.codepage_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(5, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "SJIS抽出位置";
            // 
            // SEIKI
            // 
            this.SEIKI.AutoSize = true;
            this.SEIKI.Enabled = false;
            this.SEIKI.Location = new System.Drawing.Point(5, 40);
            this.SEIKI.Name = "SEIKI";
            this.SEIKI.Size = new System.Drawing.Size(72, 16);
            this.SEIKI.TabIndex = 7;
            this.SEIKI.Text = "正規抽出";
            this.SEIKI.UseVisualStyleBackColor = true;
            // 
            // JIS2SJIS
            // 
            this.JIS2SJIS.ContextMenuStrip = this.contextMenuStrip1;
            this.JIS2SJIS.FormattingEnabled = true;
            this.JIS2SJIS.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.JIS2SJIS.Items.AddRange(new object[] {
            "S1奇数判定+S2シフト,S1を0xA1以上にシフトしXOR0x20でリマップ",
            "(S1奇数判定+S1S2ともにシフト),S1S2を調整",
            "(S1奇数判定+S2シフト),S1シフト",
            "S1-0x21,(S1奇数判定+S2シフト),S1シフト",
            "S1S2-0x2020,(S1奇数判定+S2シフト),S1シフト(JIS208 規定)",
            "S1S2-0x2121,(S1奇数判定+S2シフト),S1シフト",
            "外部TXT変換テーブル使用",
            "M$テーブル(ESC$B,ISO-2022-JP->CP932経由)",
            "M$テーブル(+0x8080,EUC-JP->CP932経由)",
            "SHIFT_JIS-2004(JIS213 2004規定)",
            "94x94バイナリ形式リマップ表((c1-0x21)*94+c2-21)"});
            this.JIS2SJIS.Location = new System.Drawing.Point(5, 15);
            this.JIS2SJIS.Name = "JIS2SJIS";
            this.JIS2SJIS.Size = new System.Drawing.Size(196, 20);
            this.JIS2SJIS.TabIndex = 6;
            this.JIS2SJIS.SelectedIndexChanged += new System.EventHandler(this.JIS2SJIS_SelectedIndexChanged);
            this.JIS2SJIS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.JIS2SJIS_KeyPress);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.Location = new System.Drawing.Point(134, 296);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 20);
            this.button2.TabIndex = 8;
            this.button2.Text = "復元";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 328);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.CMFUSION);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BDF2FONTX2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.CMFUSION.ResumeLayout(false);
            this.CMFUSION.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox CMFUSION;
        private System.Windows.Forms.RadioButton CMF;
        private System.Windows.Forms.RadioButton FILER;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton FC;
        private System.Windows.Forms.RadioButton EUC;
        private System.Windows.Forms.RadioButton SJIS;
        private System.Windows.Forms.RadioButton NOREMAP;
        private System.Windows.Forms.RadioButton CP;
        private System.Windows.Forms.TextBox codepage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox JIS2SJIS;
        private System.Windows.Forms.CheckBox SEIKI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pos;
        private System.Windows.Forms.ComboBox sekitxt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton CMGBK;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

