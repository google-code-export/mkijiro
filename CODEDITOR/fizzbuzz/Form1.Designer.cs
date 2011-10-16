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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Plus1 = new System.Windows.Forms.RadioButton();
            this.Fibonacci_number = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SQ = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 119);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(242, 191);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "(´・ω・｀)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(131, 90);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "(ﾟ∀ﾟ)すうがく";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Plus1
            // 
            this.Plus1.AutoSize = true;
            this.Plus1.Checked = true;
            this.Plus1.Location = new System.Drawing.Point(12, 18);
            this.Plus1.Name = "Plus1";
            this.Plus1.Size = new System.Drawing.Size(67, 16);
            this.Plus1.TabIndex = 3;
            this.Plus1.TabStop = true;
            this.Plus1.Text = "FizzBuzz";
            this.Plus1.UseVisualStyleBackColor = true;
            // 
            // Fibonacci_number
            // 
            this.Fibonacci_number.AutoSize = true;
            this.Fibonacci_number.Location = new System.Drawing.Point(12, 49);
            this.Fibonacci_number.Name = "Fibonacci_number";
            this.Fibonacci_number.Size = new System.Drawing.Size(63, 16);
            this.Fibonacci_number.TabIndex = 4;
            this.Fibonacci_number.Text = "FibBuzz";
            this.Fibonacci_number.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SQ);
            this.groupBox1.Controls.Add(this.Fibonacci_number);
            this.groupBox1.Controls.Add(this.Plus1);
            this.groupBox1.Location = new System.Drawing.Point(17, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 71);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // SQ
            // 
            this.SQ.AutoSize = true;
            this.SQ.Location = new System.Drawing.Point(100, 19);
            this.SQ.Name = "SQ";
            this.SQ.Size = new System.Drawing.Size(78, 16);
            this.SQ.TabIndex = 5;
            this.SQ.TabStop = true;
            this.SQ.Text = "FizzSquare";
            this.SQ.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 322);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "ふぃっずばうｚねた";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton Plus1;
        private System.Windows.Forms.RadioButton Fibonacci_number;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SQ;
    }
}

