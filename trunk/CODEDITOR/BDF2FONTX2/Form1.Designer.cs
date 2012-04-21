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
            this.button1 = new System.Windows.Forms.Button();
            this.CMFUSION = new System.Windows.Forms.GroupBox();
            this.FC = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.FILER = new System.Windows.Forms.RadioButton();
            this.CMF = new System.Windows.Forms.RadioButton();
            this.EUC = new System.Windows.Forms.RadioButton();
            this.SJIS = new System.Windows.Forms.RadioButton();
            this.NOREMAP = new System.Windows.Forms.RadioButton();
            this.CP = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.JIS2SJIS = new System.Windows.Forms.ComboBox();
            this.SEIKI = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pos = new System.Windows.Forms.TextBox();
            this.sekitxt = new System.Windows.Forms.ComboBox();
            this.CMFUSION.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(98, 321);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "変換";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CMFUSION
            // 
            this.CMFUSION.Controls.Add(this.FC);
            this.CMFUSION.Controls.Add(this.radioButton1);
            this.CMFUSION.Controls.Add(this.FILER);
            this.CMFUSION.Controls.Add(this.CMF);
            this.CMFUSION.Location = new System.Drawing.Point(35, 199);
            this.CMFUSION.Name = "CMFUSION";
            this.CMFUSION.Size = new System.Drawing.Size(200, 116);
            this.CMFUSION.TabIndex = 5;
            this.CMFUSION.TabStop = false;
            this.CMFUSION.Text = "圧縮ふぉんと";
            // 
            // FC
            // 
            this.FC.AutoSize = true;
            this.FC.Location = new System.Drawing.Point(32, 68);
            this.FC.Name = "FC";
            this.FC.Size = new System.Drawing.Size(163, 18);
            this.FC.TabIndex = 8;
            this.FC.TabStop = true;
            this.FC.Text = "FREECHEAT12x12作成";
            this.FC.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(32, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(145, 18);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "無圧縮(通常ラスター)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // FILER
            // 
            this.FILER.AutoSize = true;
            this.FILER.Location = new System.Drawing.Point(32, 92);
            this.FILER.Name = "FILER";
            this.FILER.Size = new System.Drawing.Size(152, 18);
            this.FILER.TabIndex = 6;
            this.FILER.TabStop = true;
            this.FILER.Text = "6x12作成(FILER互換)";
            this.FILER.UseVisualStyleBackColor = true;
            this.FILER.CheckedChanged += new System.EventHandler(this.FILER_CheckedChanged);
            // 
            // CMF
            // 
            this.CMF.AutoSize = true;
            this.CMF.Location = new System.Drawing.Point(32, 44);
            this.CMF.Name = "CMF";
            this.CMF.Size = new System.Drawing.Size(153, 18);
            this.CMF.TabIndex = 5;
            this.CMF.TabStop = true;
            this.CMF.Text = "CMFUSION12x12作成";
            this.CMF.UseVisualStyleBackColor = true;
            this.CMF.CheckedChanged += new System.EventHandler(this.CMF_CheckedChanged_1);
            // 
            // EUC
            // 
            this.EUC.AutoSize = true;
            this.EUC.Location = new System.Drawing.Point(6, 44);
            this.EUC.Name = "EUC";
            this.EUC.Size = new System.Drawing.Size(84, 18);
            this.EUC.TabIndex = 2;
            this.EUC.Text = "JIS→EUC";
            this.EUC.UseVisualStyleBackColor = true;
            // 
            // SJIS
            // 
            this.SJIS.AutoSize = true;
            this.SJIS.Checked = true;
            this.SJIS.Location = new System.Drawing.Point(92, 20);
            this.SJIS.Name = "SJIS";
            this.SJIS.Size = new System.Drawing.Size(85, 18);
            this.SJIS.TabIndex = 1;
            this.SJIS.TabStop = true;
            this.SJIS.Text = "JIS→SJIS";
            this.SJIS.UseVisualStyleBackColor = true;
            // 
            // NOREMAP
            // 
            this.NOREMAP.AutoSize = true;
            this.NOREMAP.Location = new System.Drawing.Point(6, 20);
            this.NOREMAP.Name = "NOREMAP";
            this.NOREMAP.Size = new System.Drawing.Size(67, 18);
            this.NOREMAP.TabIndex = 3;
            this.NOREMAP.TabStop = true;
            this.NOREMAP.Text = "無変換";
            this.NOREMAP.UseVisualStyleBackColor = true;
            // 
            // CP
            // 
            this.CP.AutoSize = true;
            this.CP.Location = new System.Drawing.Point(96, 44);
            this.CP.Name = "CP";
            this.CP.Size = new System.Drawing.Size(71, 18);
            this.CP.TabIndex = 4;
            this.CP.TabStop = true;
            this.CP.Text = "CP指定";
            this.CP.UseVisualStyleBackColor = true;
            this.CP.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(173, 43);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(47, 21);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "0";
            this.textBox1.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.CP);
            this.groupBox1.Controls.Add(this.NOREMAP);
            this.groupBox1.Controls.Add(this.SJIS);
            this.groupBox1.Controls.Add(this.EUC);
            this.groupBox1.Location = new System.Drawing.Point(29, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 70);
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
            this.groupBox2.Location = new System.Drawing.Point(29, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 105);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "JIS->SJIS変換方法";
            // 
            // JIS2SJIS
            // 
            this.JIS2SJIS.FormattingEnabled = true;
            this.JIS2SJIS.Items.AddRange(new object[] {
            "上位バイト奇数判定後上下ともに変更",
            "上位バイト奇数判定後下位バイトのみ変更",
            "上位バイト-0x21後奇数判定",
            "最初に-0x2121後奇数判定",
            "外部TXT変換テーブル使用",
            "M$内部テーブル使用"});
            this.JIS2SJIS.Location = new System.Drawing.Point(6, 17);
            this.JIS2SJIS.Name = "JIS2SJIS";
            this.JIS2SJIS.Size = new System.Drawing.Size(228, 22);
            this.JIS2SJIS.TabIndex = 6;
            this.JIS2SJIS.SelectedIndexChanged += new System.EventHandler(this.JIS2SJIS_SelectedIndexChanged);
            // 
            // SEIKI
            // 
            this.SEIKI.AutoSize = true;
            this.SEIKI.Enabled = false;
            this.SEIKI.Location = new System.Drawing.Point(6, 47);
            this.SEIKI.Name = "SEIKI";
            this.SEIKI.Size = new System.Drawing.Size(82, 18);
            this.SEIKI.TabIndex = 7;
            this.SEIKI.Text = "正規変更";
            this.SEIKI.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(6, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "SJIS抽出位置";
            // 
            // pos
            // 
            this.pos.Enabled = false;
            this.pos.Location = new System.Drawing.Point(102, 72);
            this.pos.MaxLength = 1;
            this.pos.Name = "pos";
            this.pos.Size = new System.Drawing.Size(32, 21);
            this.pos.TabIndex = 10;
            this.pos.Text = "3";
            // 
            // sekitxt
            // 
            this.sekitxt.FormattingEnabled = true;
            this.sekitxt.Items.AddRange(new object[] {
            "\\n0x[0-9A-F]{4}\\t0x%J",
            "%J [0-9A-Fa-f]{4}"});
            this.sekitxt.Location = new System.Drawing.Point(99, 44);
            this.sekitxt.Name = "sekitxt";
            this.sekitxt.Size = new System.Drawing.Size(135, 22);
            this.sekitxt.TabIndex = 11;
            this.sekitxt.SelectedIndexChanged += new System.EventHandler(this.sekitxt_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 356);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.CMFUSION);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BDF2FONTX2";
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox JIS2SJIS;
        private System.Windows.Forms.CheckBox SEIKI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pos;
        private System.Windows.Forms.ComboBox sekitxt;
    }
}

