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
            if (fs.Length > 0x8060)
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
                 if (isosize - fsize == 2048)
                {
                    return "M33USBマウントのみで発生する-2048サイズ欠けです\nPSPFILER/ISOTOOL/TEMPARなどを使ってください";
                }
                else if (isosize < fsize)
                {
                    return "オーバーダンプです";
                }
                 else if (isosize == fsize)
                 {
                     FileStream sector = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                     byte[] beforefinal = new byte[2048];
                     byte[] finalsector = new byte[2048];
                     byte[] hash = new byte[32];
                     sector.Seek(isosize - 4096, SeekOrigin.Begin);
                     sector.Read(beforefinal, 0, 2048);
                     sector.Read(finalsector, 0, 2048);
                     sector.Close();

                     System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                     Array.ConstrainedCopy(beforefinal,2016,hash,0,32);
                     byte[] b = md5.ComputeHash(hash);
                     string result = BitConverter.ToString(b).ToUpper().Replace("-", "");
                     Array.ConstrainedCopy(finalsector, 0, hash, 0, 32);
                     byte[] c = md5.ComputeHash(hash);
                     string result2 = BitConverter.ToString(c).ToUpper().Replace("-", "");
                     string NU = "70BC8F4B72A86921468BF8E8441DCE51";//null
                     string FF = "0D7DC4266497100E4831F5B31B6B274F";//FF
                     if (result == result2)
                     {//FILLED wiht NULL
                         return "正常なサイズです";
                     }
                     else if (result2 == NU && result != NU)
                     {
                         return "サイズは正常ですが,バッドダンプの可能性があります";
                     }
                     else if (result2 == FF && result != NU)
                     {
                         return "サイズは正常ですが,バッドダンプの可能性があります";
                     }
                     else
                     {
                         return "正常なサイズです";
                     }
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
