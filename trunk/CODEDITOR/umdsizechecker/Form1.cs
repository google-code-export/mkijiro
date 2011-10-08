using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.button1.DragDrop += new
            System.Windows.Forms.DragEventHandler(this.button1_DragDrop);
            this.button1.DragEnter += new
            System.Windows.Forms.DragEventHandler(this.button1_DragEnter);
            this.textBox1.DragDrop += new
            System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new
            System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
        }


        private void textBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBox1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBox1.Text = getsize(s[0]);
        }
        private void button1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textBox1.Text = getsize(s[0]);
        }

        private string getsize(string filepath){
            FileStream fs = new FileStream(filepath,FileMode.Open, FileAccess.Read);
            long isosize = 0;
            long isobig = 0;
            long fsize = fs.Length;

            label1.Text = "ファイルサイズ:";
            label2.Text = "セクター算出　:";
            if (fs.Length > 0x8050)
            {
                byte[] bs = new byte[8];
                byte[] big = new byte[8];
                byte[] x = new byte[3];
                fs.Seek(0x8050, SeekOrigin.Begin);
                fs.Read(bs, 0, 3);
                fs.Seek(0x8055, SeekOrigin.Begin);
                fs.Read(big, 0, 3);
                fs.Close();
                Array.ConstrainedCopy(big, 0, x, 0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Array.ConstrainedCopy(x, 2-i, big, i, 1);
                }
                isosize = BitConverter.ToInt64(bs, 0);
                isobig = BitConverter.ToInt64(big, 0);
                if (isosize != isobig)
                {
                    return "ISOではありません";
                }
                isosize *= 2048;
                label1.Text +=  Convert.ToString(fsize);
                label2.Text += Convert.ToString(isosize);
                if (isosize == fsize)
                {
                    return "正常なサイズです";
                }
                else if (isosize - fsize == 2048)
                {
                    return "M33USBマウントのみで発生する-2048サイズ欠けです\nPSPFILER/ISOTOOL/TEMPARなどを使ってください";
                }
                else if (isosize < fsize)
                {
                    return "オーバーダンプです";
                }
                else
                {
                    return "サイズが合いません";
                }
            }
            fs.Close();
            return "ISOではありません";
        }
    }
}
