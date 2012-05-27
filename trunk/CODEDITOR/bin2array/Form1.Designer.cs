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
            this.label1 = new System.Windows.Forms.Label();
            this.hairetu = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.kensaku = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fileoffset = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ANDs = new System.Windows.Forms.RadioButton();
            this.byteloop = new System.Windows.Forms.RadioButton();
            this.MD5 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.HEX = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CS = new System.Windows.Forms.RadioButton();
            this.VBNET = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "出力配列名";
            // 
            // hairetu
            // 
            this.hairetu.Location = new System.Drawing.Point(126, 6);
            this.hairetu.Name = "hairetu";
            this.hairetu.Size = new System.Drawing.Size(58, 19);
            this.hairetu.TabIndex = 1;
            this.hairetu.Text = "array";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "検索用バイト配列名";
            // 
            // kensaku
            // 
            this.kensaku.Location = new System.Drawing.Point(126, 31);
            this.kensaku.Name = "kensaku";
            this.kensaku.Size = new System.Drawing.Size(58, 19);
            this.kensaku.TabIndex = 3;
            this.kensaku.Text = "bs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "ファイルオフセット";
            // 
            // fileoffset
            // 
            this.fileoffset.Location = new System.Drawing.Point(126, 57);
            this.fileoffset.Name = "fileoffset";
            this.fileoffset.Size = new System.Drawing.Size(58, 19);
            this.fileoffset.TabIndex = 5;
            this.fileoffset.Text = "i";
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(15, 166);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(263, 133);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // ANDs
            // 
            this.ANDs.AutoSize = true;
            this.ANDs.Location = new System.Drawing.Point(54, 16);
            this.ANDs.Name = "ANDs";
            this.ANDs.Size = new System.Drawing.Size(49, 16);
            this.ANDs.TabIndex = 7;
            this.ANDs.Text = "Ands";
            this.ANDs.UseVisualStyleBackColor = true;
            this.ANDs.CheckedChanged += new System.EventHandler(this.ANDs_CheckedChanged);
            // 
            // byteloop
            // 
            this.byteloop.AutoSize = true;
            this.byteloop.Location = new System.Drawing.Point(105, 16);
            this.byteloop.Name = "byteloop";
            this.byteloop.Size = new System.Drawing.Size(70, 16);
            this.byteloop.TabIndex = 8;
            this.byteloop.Text = "シーケンス";
            this.byteloop.UseVisualStyleBackColor = true;
            // 
            // MD5
            // 
            this.MD5.AutoSize = true;
            this.MD5.Location = new System.Drawing.Point(178, 16);
            this.MD5.Name = "MD5";
            this.MD5.Size = new System.Drawing.Size(46, 16);
            this.MD5.TabIndex = 9;
            this.MD5.Text = "MD5";
            this.MD5.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.HEX);
            this.groupBox1.Controls.Add(this.ANDs);
            this.groupBox1.Controls.Add(this.byteloop);
            this.groupBox1.Controls.Add(this.MD5);
            this.groupBox1.Location = new System.Drawing.Point(17, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 38);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力タイプ";
            // 
            // HEX
            // 
            this.HEX.AutoSize = true;
            this.HEX.Checked = true;
            this.HEX.Location = new System.Drawing.Point(7, 16);
            this.HEX.Name = "HEX";
            this.HEX.Size = new System.Drawing.Size(35, 16);
            this.HEX.TabIndex = 10;
            this.HEX.TabStop = true;
            this.HEX.Text = "0x";
            this.HEX.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.CS);
            this.groupBox2.Controls.Add(this.VBNET);
            this.groupBox2.Location = new System.Drawing.Point(17, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 39);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "言語";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 2;
            // 
            // CS
            // 
            this.CS.AutoSize = true;
            this.CS.Checked = true;
            this.CS.Location = new System.Drawing.Point(7, 14);
            this.CS.Name = "CS";
            this.CS.Size = new System.Drawing.Size(37, 16);
            this.CS.TabIndex = 1;
            this.CS.TabStop = true;
            this.CS.Text = "C#";
            this.CS.UseVisualStyleBackColor = true;
            // 
            // VBNET
            // 
            this.VBNET.AutoSize = true;
            this.VBNET.Location = new System.Drawing.Point(59, 14);
            this.VBNET.Name = "VBNET";
            this.VBNET.Size = new System.Drawing.Size(63, 16);
            this.VBNET.TabIndex = 0;
            this.VBNET.Text = "VB.NET";
            this.VBNET.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 20);
            this.button1.TabIndex = 14;
            this.button1.Text = "and";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(225, 303);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 20);
            this.button2.TabIndex = 15;
            this.button2.Text = "シーケンス";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(87, 303);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 20);
            this.button3.TabIndex = 16;
            this.button3.Text = "md5";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(156, 303);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 20);
            this.button4.TabIndex = 17;
            this.button4.Text = "外人";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(189, 11);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(93, 16);
            this.radioButton1.TabIndex = 18;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "BIN→ARRAY";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(189, 34);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(93, 16);
            this.radioButton2.TabIndex = 19;
            this.radioButton2.Text = "ARRAY→BIN";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(189, 57);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 16);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "CMF12x12";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 337);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.fileoffset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.kensaku);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hairetu);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bin<->array(ファイルをドロップして下さい)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hairetu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox kensaku;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileoffset;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton ANDs;
        private System.Windows.Forms.RadioButton byteloop;
        private System.Windows.Forms.RadioButton MD5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton VBNET;
        private System.Windows.Forms.RadioButton CS;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton HEX;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

