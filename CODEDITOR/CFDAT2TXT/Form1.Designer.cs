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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.gameid = new System.Windows.Forms.TextBox();
            this.codename = new System.Windows.Forms.TextBox();
            this.codehex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmtb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OPEN = new System.Windows.Forms.ToolStripMenuItem();
            this.CONVERTTXT = new System.Windows.Forms.ToolStripMenuItem();
            this.eNCODEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uTF16BE1201 = new System.Windows.Forms.ToolStripMenuItem();
            this.sJIS932 = new System.Windows.Forms.ToolStripMenuItem();
            this.eUC51932 = new System.Windows.Forms.ToolStripMenuItem();
            this.gBK936 = new System.Windows.Forms.ToolStripMenuItem();
            this.vERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CONVERTDAT = new System.Windows.Forms.ToolStripMenuItem();
            this.sETTINGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SAVE = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox2 = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.gtitle = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.treeView1.Location = new System.Drawing.Point(12, 29);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(248, 380);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            // 
            // gameid
            // 
            this.gameid.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gameid.Location = new System.Drawing.Point(289, 87);
            this.gameid.Name = "gameid";
            this.gameid.ReadOnly = true;
            this.gameid.Size = new System.Drawing.Size(100, 25);
            this.gameid.TabIndex = 2;
            // 
            // codename
            // 
            this.codename.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.codename.Location = new System.Drawing.Point(287, 141);
            this.codename.Name = "codename";
            this.codename.Size = new System.Drawing.Size(204, 25);
            this.codename.TabIndex = 3;
            // 
            // codehex
            // 
            this.codehex.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.codehex.Location = new System.Drawing.Point(287, 188);
            this.codehex.Multiline = true;
            this.codehex.Name = "codehex";
            this.codehex.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.codehex.Size = new System.Drawing.Size(204, 128);
            this.codehex.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(287, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "CODE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(287, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "CODENAME";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(287, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "GAMEID";
            // 
            // cmtb
            // 
            this.cmtb.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmtb.Location = new System.Drawing.Point(287, 342);
            this.cmtb.Multiline = true;
            this.cmtb.Name = "cmtb";
            this.cmtb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cmtb.Size = new System.Drawing.Size(204, 67);
            this.cmtb.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(287, 327);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "COMMENT";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem,
            this.eNCODEToolStripMenuItem,
            this.sETTINGToolStripMenuItem,
            this.vERToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(505, 26);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fILEToolStripMenuItem
            // 
            this.fILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OPEN,
            this.CONVERTTXT,
            this.CONVERTDAT});
            this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
            this.fILEToolStripMenuItem.Size = new System.Drawing.Size(46, 22);
            this.fILEToolStripMenuItem.Text = "FILE";
            // 
            // OPEN
            // 
            this.OPEN.Name = "OPEN";
            this.OPEN.Size = new System.Drawing.Size(160, 22);
            this.OPEN.Text = "ファイルを開く";
            this.OPEN.Click += new System.EventHandler(this.button1_Click);
            // 
            // CONVERTTXT
            // 
            this.CONVERTTXT.Name = "CONVERTTXT";
            this.CONVERTTXT.Size = new System.Drawing.Size(160, 22);
            this.CONVERTTXT.Text = "TXTで保存";
            this.CONVERTTXT.Click += new System.EventHandler(this.OUT_Click);
            // 
            // eNCODEToolStripMenuItem
            // 
            this.eNCODEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uTF16BE1201,
            this.sJIS932,
            this.eUC51932,
            this.gBK936});
            this.eNCODEToolStripMenuItem.Name = "eNCODEToolStripMenuItem";
            this.eNCODEToolStripMenuItem.Size = new System.Drawing.Size(69, 22);
            this.eNCODEToolStripMenuItem.Text = "ENCODE";
            // 
            // uTF16BE1201
            // 
            this.uTF16BE1201.Checked = true;
            this.uTF16BE1201.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uTF16BE1201.Name = "uTF16BE1201";
            this.uTF16BE1201.Size = new System.Drawing.Size(167, 22);
            this.uTF16BE1201.Text = "UTF16BE(1201)";
            this.uTF16BE1201.Click += new System.EventHandler(this.eNCODEToolStripMenuItem_Click);
            // 
            // sJIS932
            // 
            this.sJIS932.Name = "sJIS932";
            this.sJIS932.Size = new System.Drawing.Size(167, 22);
            this.sJIS932.Text = "SJIS(932)";
            this.sJIS932.Click += new System.EventHandler(this.eNCODEToolStripMenuItem_Click);
            // 
            // eUC51932
            // 
            this.eUC51932.Name = "eUC51932";
            this.eUC51932.Size = new System.Drawing.Size(167, 22);
            this.eUC51932.Text = "EUC-JP(51932)";
            this.eUC51932.Click += new System.EventHandler(this.eNCODEToolStripMenuItem_Click);
            // 
            // gBK936
            // 
            this.gBK936.Name = "gBK936";
            this.gBK936.Size = new System.Drawing.Size(167, 22);
            this.gBK936.Text = "GBK(936)";
            this.gBK936.Click += new System.EventHandler(this.eNCODEToolStripMenuItem_Click);
            // 
            // vERToolStripMenuItem
            // 
            this.vERToolStripMenuItem.Name = "vERToolStripMenuItem";
            this.vERToolStripMenuItem.Size = new System.Drawing.Size(43, 22);
            this.vERToolStripMenuItem.Text = "VER";
            this.vERToolStripMenuItem.Click += new System.EventHandler(this.vERToolStripMenuItem_Click);
            // 
            // CONVERTDAT
            // 
            this.CONVERTDAT.Name = "CONVERTDAT";
            this.CONVERTDAT.Size = new System.Drawing.Size(160, 22);
            this.CONVERTDAT.Text = "DATで保存";
            this.CONVERTDAT.Click += new System.EventHandler(this.OUTDAT_Click);
            // 
            // sETTINGToolStripMenuItem
            // 
            this.sETTINGToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkBox1,
            this.checkBox2});
            this.sETTINGToolStripMenuItem.Name = "sETTINGToolStripMenuItem";
            this.sETTINGToolStripMenuItem.Size = new System.Drawing.Size(74, 22);
            this.sETTINGToolStripMenuItem.Text = "SETTING";
            // 
            // SAVE
            // 
            this.SAVE.Location = new System.Drawing.Point(410, 91);
            this.SAVE.Name = "SAVE";
            this.SAVE.Size = new System.Drawing.Size(75, 23);
            this.SAVE.TabIndex = 14;
            this.SAVE.Text = "SAVE";
            this.SAVE.UseVisualStyleBackColor = true;
            this.SAVE.Click += new System.EventHandler(this.SAVE_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(267, 22);
            this.checkBox1.Text = "CWC形式でTXT出力する";
            this.checkBox1.ToolTipText = "TXT出力時CWCHEAT\r\n_L 0x12345678 0xABCDEF000";
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(267, 22);
            this.checkBox2.Text = "CODEFREAK暗号コードを復号する";
            this.checkBox2.ToolTipText = "CODEFRAK暗号コードを復号して保存します\r\nDAT保存時は(M)の下3桁に800を追加してCWC生コードモードにします\r\nアドレス部;XORMASK 0xD" +
    "6F73BEE\r\n";
            this.checkBox2.Click += new System.EventHandler(this.checkBox2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(287, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "GAMETITLE";
            // 
            // gtitle
            // 
            this.gtitle.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gtitle.Location = new System.Drawing.Point(287, 44);
            this.gtitle.Name = "gtitle";
            this.gtitle.Size = new System.Drawing.Size(204, 25);
            this.gtitle.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 423);
            this.Controls.Add(this.gtitle);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SAVE);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmtb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.codehex);
            this.Controls.Add(this.codename);
            this.Controls.Add(this.gameid);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CFDAT2TXT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox gameid;
        private System.Windows.Forms.TextBox codename;
        private System.Windows.Forms.TextBox codehex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cmtb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OPEN;
        private System.Windows.Forms.ToolStripMenuItem CONVERTTXT;
        private System.Windows.Forms.ToolStripMenuItem vERToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eNCODEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sJIS932;
        private System.Windows.Forms.ToolStripMenuItem eUC51932;
        private System.Windows.Forms.ToolStripMenuItem gBK936;
        private System.Windows.Forms.ToolStripMenuItem uTF16BE1201;
        private System.Windows.Forms.ToolStripMenuItem CONVERTDAT;
        private System.Windows.Forms.ToolStripMenuItem sETTINGToolStripMenuItem;
        private System.Windows.Forms.Button SAVE;
        private System.Windows.Forms.ToolStripMenuItem checkBox1;
        private System.Windows.Forms.ToolStripMenuItem checkBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox gtitle;
    }
}

