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
            this.SJIS = new System.Windows.Forms.RadioButton();
            this.EUC = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CP = new System.Windows.Forms.RadioButton();
            this.NOREMAP = new System.Windows.Forms.RadioButton();
            this.CMFUSION = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.FILER = new System.Windows.Forms.RadioButton();
            this.CMF = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.CMFUSION.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(88, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "変換";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SJIS
            // 
            this.SJIS.AutoSize = true;
            this.SJIS.Checked = true;
            this.SJIS.Location = new System.Drawing.Point(96, 20);
            this.SJIS.Name = "SJIS";
            this.SJIS.Size = new System.Drawing.Size(85, 18);
            this.SJIS.TabIndex = 1;
            this.SJIS.TabStop = true;
            this.SJIS.Text = "JIS→SJIS";
            this.SJIS.UseVisualStyleBackColor = true;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.CP);
            this.groupBox1.Controls.Add(this.NOREMAP);
            this.groupBox1.Controls.Add(this.SJIS);
            this.groupBox1.Controls.Add(this.EUC);
            this.groupBox1.Location = new System.Drawing.Point(29, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 79);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FONTX2文字テーブル";
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
            // CMFUSION
            // 
            this.CMFUSION.Controls.Add(this.radioButton2);
            this.CMFUSION.Controls.Add(this.radioButton1);
            this.CMFUSION.Controls.Add(this.FILER);
            this.CMFUSION.Controls.Add(this.CMF);
            this.CMFUSION.Location = new System.Drawing.Point(29, 97);
            this.CMFUSION.Name = "CMFUSION";
            this.CMFUSION.Size = new System.Drawing.Size(200, 116);
            this.CMFUSION.TabIndex = 5;
            this.CMFUSION.TabStop = false;
            this.CMFUSION.Text = "圧縮ふぉんと";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(33, 92);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(53, 18);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "自動";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
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
            this.FILER.Location = new System.Drawing.Point(32, 68);
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
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 254);
            this.Controls.Add(this.CMFUSION);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BDF2FONTX2";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.CMFUSION.ResumeLayout(false);
            this.CMFUSION.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton SJIS;
        private System.Windows.Forms.RadioButton EUC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox CMFUSION;
        private System.Windows.Forms.RadioButton NOREMAP;
        private System.Windows.Forms.RadioButton CMF;
        private System.Windows.Forms.RadioButton CP;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton FILER;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}

