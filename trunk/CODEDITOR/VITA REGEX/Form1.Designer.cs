namespace WindowsFormsApplication6
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cHAGEUNIXDOTFILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fIXNANUALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.rEMOVEUNIXDOTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(12, 57);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(286, 193);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "LIST";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(117, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "FIX";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIXNANUALToolStripMenuItem,
            this.toolStripSeparator1,
            this.cHAGEUNIXDOTFILEToolStripMenuItem,
            this.rEMOVEUNIXDOTToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(255, 76);
            // 
            // cHAGEUNIXDOTFILEToolStripMenuItem
            // 
            this.cHAGEUNIXDOTFILEToolStripMenuItem.Name = "cHAGEUNIXDOTFILEToolStripMenuItem";
            this.cHAGEUNIXDOTFILEToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.cHAGEUNIXDOTFILEToolStripMenuItem.Text = "ADD UNIX DOTFLAG(NGLIST)";
            this.cHAGEUNIXDOTFILEToolStripMenuItem.ToolTipText = "VITA CMA NEVER RECOGNIZE UNIX DOT FILE.THIS APP INGNOR TO FIX.\r\n先頭ドットをつけるとVITACMA" +
    "で認識されなくなります。このアプリのFIXの対象外になります";
            this.cHAGEUNIXDOTFILEToolStripMenuItem.Click += new System.EventHandler(this.cHAGEUNIXDOTFILEToolStripMenuItem_Click);
            // 
            // fIXNANUALToolStripMenuItem
            // 
            this.fIXNANUALToolStripMenuItem.Name = "fIXNANUALToolStripMenuItem";
            this.fIXNANUALToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.fIXNANUALToolStripMenuItem.Text = "NANUAL FIX(VITA8.3 REGEX)";
            this.fIXNANUALToolStripMenuItem.ToolTipText = "手動でリネームします。VITAの8.3法則以外ではリネームできないようになってます";
            this.fIXNANUALToolStripMenuItem.Click += new System.EventHandler(this.fIXNANUALToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(251, 6);
            // 
            // rEMOVEUNIXDOTToolStripMenuItem
            // 
            this.rEMOVEUNIXDOTToolStripMenuItem.Name = "rEMOVEUNIXDOTToolStripMenuItem";
            this.rEMOVEUNIXDOTToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.rEMOVEUNIXDOTToolStripMenuItem.Text = "REMOVE UNIX DOTFLAG";
            this.rEMOVEUNIXDOTToolStripMenuItem.ToolTipText = "remove unix dotfile flag\r\nファイル名先頭ドットを除去します";
            this.rEMOVEUNIXDOTToolStripMenuItem.Click += new System.EventHandler(this.rEMOVEUNIXDOTToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VITA_FILE_FIXER";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fIXNANUALToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cHAGEUNIXDOTFILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rEMOVEUNIXDOTToolStripMenuItem;
    }
}

