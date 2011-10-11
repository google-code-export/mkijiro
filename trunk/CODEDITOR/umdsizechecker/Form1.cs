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
        string isofile = ""; 

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
            isofile = s[0];
            textBox1.Text = getsize(s[0]);
            if (textBox1.Text.Contains("ません"))
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
            }
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
            isofile = s[0];
            textBox1.Text = getsize(s[0]);
            if (textBox1.Text.Contains("ません"))
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
            }
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
                byte[] str = new byte[5];
                byte[] x = new byte[3];
                fs.Seek(0x8001, SeekOrigin.Begin);
                fs.Read(str, 0, 5);
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
                string iso = Encoding.GetEncoding(0).GetString(str);
                isosize = BitConverter.ToInt64(bs, 0);
                isobig = BitConverter.ToInt64(big, 0);
                if (isosize != isobig && iso == "CD001")
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

        private void button2_Click(object sender, EventArgs e)//gid
        {
            if (System.IO.File.Exists(isofile))
            {
                byte[] gid = new byte[10];
                FileStream fs = new FileStream(isofile, FileMode.Open, FileAccess.Read);
                fs.Seek(0x8373, SeekOrigin.Begin);
                fs.Read(gid, 0, 10);
                fs.Close();
                string rpname = Encoding.GetEncoding(0).GetString(gid) + ".ISO";
                textBox1.Text = rpname + "にリネームしました";
                int last = isofile.LastIndexOf("\\") + 1;
                rpname = isofile.Substring(0, last) + rpname;
                if (System.IO.File.Exists(rpname))
                {
                    textBox1.Text = "同じ名前が存在します";
                }
                else
                {
                    File.Move(isofile, rpname);
                    isofile = rpname;
                }
            }
            else
            {
                textBox1.Text = "ファイルが存在しません";
            }
        }

        private void button4_Click(object sender, EventArgs e)//isohead
        {

            if (System.IO.File.Exists(isofile))
            {
                byte[] isohead = new byte[32];
                FileStream fs = new FileStream(isofile, FileMode.Open, FileAccess.Read);
                fs.Seek(0x8028, SeekOrigin.Begin);
                fs.Read(isohead, 0, 32);
                fs.Close();
                string rpname = Encoding.GetEncoding(0).GetString(isohead).Trim();
                if (rpname != "")
                {
                    rpname += ".ISO";
                    textBox1.Text = rpname + "にリネームしました";
                    int last = isofile.LastIndexOf("\\") + 1;
                    rpname = isofile.Substring(0, last) + rpname;
                    if (System.IO.File.Exists(rpname))
                    {
                        textBox1.Text = "同じ名前が存在します";
                    }
                    else
                    {
                        File.Move(isofile, rpname);
                        isofile = rpname;
                    }
                }
                else
                {
                    textBox1.Text = "ISOヘッダにタイトル名が有りませんでした";
                }
            }
            else
            {
                textBox1.Text = "ファイルが存在しません";
            }
        }

        private void button3_Click(object sender, EventArgs e)//psfname
        {

            if (System.IO.File.Exists(isofile))
            {
                byte[] lba = new byte[4];
                byte[] size = new byte[8];
                byte[] psfname = new byte[129];
                byte[] sector = new byte[2048];
                int i =0,z=0;
                FileStream fs = new FileStream(isofile, FileMode.Open, FileAccess.Read);                
                fs.Seek(0x8050, SeekOrigin.Begin);
                fs.Read(size, 0, 5);
                long lbatotal = BitConverter.ToInt64(size, 0);
                lbatotal *= 2048;
                if (lbatotal - fs.Length <= 2048)
                {
                    fs.Seek(0x80A5, SeekOrigin.Begin);
                    fs.Read(lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    fs.Seek(z * 2048, SeekOrigin.Begin);
                    fs.Read(sector, 0, 2048);
                    //PSP_GAME
                    while (true)
                    {
                        if (sector[i] == 0x50 && sector[i + 1] == 0x53 && sector[i + 2] == 0x50) break;
                        if (i > 2048) {fs.Close(); return; }
                        i++;
                    }

                    Array.ConstrainedCopy(sector, i - 31, lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    fs.Seek(z * 2048, SeekOrigin.Begin);
                    fs.Read(sector, 0, 2048);
                    i = 0;
                    //PARAM.SFO
                    while (true)
                    {
                        if (sector[i] == 0x50 && sector[i + 1] == 0x41 && sector[i + 2] == 0x52 && sector[i + 3] == 0x41) break;
                        if (i > 2048) { fs.Close(); return; }
                        i++;
                    }
                    Array.ConstrainedCopy(sector, i - 31, lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    fs.Seek(z * 2048, SeekOrigin.Begin);
                    fs.Read(sector, 0, 2048);
                    Array.ConstrainedCopy(sector, 12, lba, 0, 2);
                    i = BitConverter.ToInt32(lba, 0);
                    z = 12 + 16;
                    while (sector[z] != 0x80)
                    {
                        if (z > 2048) { fs.Close(); return; }
                        z += 16;
                    }
                    Array.ConstrainedCopy(sector, z + 4, lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(sector, z + i, psfname, 0, 129);
                    fs.Close();
                    string rpname = Encoding.GetEncoding(65001).GetString(psfname);
                    i = rpname.IndexOf("\x0");
                    rpname = rpname.Substring(0, i) + ".ISO";
                    textBox1.Text = rpname + "にリネームしました";
                    int last = isofile.LastIndexOf("\\") + 1;
                    rpname = isofile.Substring(0, last) + rpname;
                    if (System.IO.File.Exists(rpname))
                    {
                        textBox1.Text = "同じ名前が存在します";
                    }
                    else
                    {
                        File.Move(isofile, rpname);
                        isofile = rpname;
                    }
                }
                else
                {
                    fs.Close();
                    textBox1.Text ="2k以上欠けてるためPARAM.SFO取得を停止しました";
                }
            }
            else
            {
                textBox1.Text = "ファイルが存在しません";
            }
        }
    }
}
