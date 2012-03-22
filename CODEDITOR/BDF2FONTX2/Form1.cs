using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string lastdir = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(lastdir != ""){
            ofd.InitialDirectory = lastdir;}
            else{
            ofd.InitialDirectory = Application.StartupPath;
            }
            ofd.Filter = "bdfファイル(*.bdf)|*.bdf";
            ofd.Title =  "開くファイルを選択してください";

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lastdir = Path.GetDirectoryName(ofd.FileName);
                string path = ofd.FileName;
                string s = "";
                bool parse = false;
                int stpos = 0;
                int tmp = 0;
                int basepos = 0;
                int yoko = 0;
                int tate = 0;
                int total = 0;
                bool zenkaku = false;
                byte[] hex = new byte[4];
                byte[] header = { 0x46, 0x4F, 0x4E, 0x54, 0x58, 0x32, 0x42, 0x44, 0x46, 0x32, 0x46, 0x54, 0x58, 0x32, 0x8, 0x10, 0 };
                UInt16[] ftable = new UInt16[1024*50];
                byte[] font = new byte[1024 * 1024];
                StreamReader sr = new System.IO.StreamReader(path, System.Text.Encoding.GetEncoding(932));
                while (sr.Peek() > -1)
                {
                    s = sr.ReadLine();
                    if (zenkaku == false)
                    {
                        if (s.Contains("CHARSET")) { }
                        else if (s.Contains("ENCODING"))
                        {
                            basepos = Convert.ToInt32(s.Replace("ENCODING", "").Trim(), 10);
                            if (zenkaku == false && basepos > 255)
                            {
                                zenkaku = true;
                                ftable[0] = Convert.ToUInt16(basepos);
                            }
                        }
                        else if ((s.Contains("BBX") == true) && (yoko == 0))
                        {
                            Regex r = new System.Text.RegularExpressions.Regex("\\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            Match m = r.Match(s);
                            int l = 0;

                            while (m.Success)
                            {
                                if (l == 0)
                                {
                                    yoko = Convert.ToInt32(m.Value);
                                }
                                if (l == 1)
                                {
                                    tate = Convert.ToInt32(m.Value);
                                }
                                l++;
                                m = m.NextMatch();
                            }

                        }
                        else if (s.Contains("BITMAP"))
                        {
                            parse = true;
                        }
                        else if (s.Contains("ENDCHAR"))
                        {
                            stpos = 0;
                            parse = false;
                        }
                        else if (parse)
                        {
                            tmp = Convert.ToInt32(s, 16);
                            hex = BitConverter.GetBytes(tmp);
                            if (yoko > 8)
                            {
                                Array.Copy(hex, 1, font, (basepos * tate * 2) + stpos, 1);
                                Array.Copy(hex, 0, font, (basepos * tate * 2) + stpos + 1, 1);
                                stpos += 2;
                            }
                            else
                            {
                                Array.Copy(hex, 0, font, (basepos * tate) + stpos, 1);
                                stpos++;
                            }
                        }
                    }
                    else
                    {
                        if (s.Contains("ENCODING"))
                        {
                            total++;
                            ftable[total] = Convert.ToUInt16(s.Replace("ENCODING", "").Trim(), 10);
                        }
                        else if ((s.Contains("BBX") == true) && (yoko == 0))
                        {
                            Regex r = new System.Text.RegularExpressions.Regex("\\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            Match m = r.Match(s);
                            int l = 0;

                            while (m.Success)
                            {
                                if (l == 0)
                                {
                                    yoko = Convert.ToInt32(m.Value);
                                }
                                if (l == 1)
                                {
                                    tate = Convert.ToInt32(m.Value);
                                }
                                l++;
                                m = m.NextMatch();
                            }

                        }
                        else if (s.Contains("BITMAP"))
                        {
                            parse = true;
                        }
                        else if (s.Contains("ENDCHAR"))
                        {
                            stpos = 0;
                            parse = false;
                        }
                        else if (parse)
                        {
                            tmp = Convert.ToInt32(s, 16);
                            hex = BitConverter.GetBytes(tmp);
                            if (yoko > 8)
                            {
                                Array.Copy(hex, 1, font, (total * tate * 2) + stpos, 1);
                                Array.Copy(hex, 0, font, (total * tate * 2) + stpos + 1, 1);
                                stpos += 2;
                            }
                            else
                            {
                                Array.Copy(hex, 0, font, (total * tate) + stpos, 1);
                                stpos++;
                            }
                        }

                    }
                }
                sr.Close();
                hex = BitConverter.GetBytes(yoko);
                Array.Copy(hex, 0, header, 14, 1);
                hex = BitConverter.GetBytes(tate);
                Array.Copy(hex, 0, header, 15, 1);

                if (zenkaku == false)
                {
                    if (yoko > 8)
                    {
                        Array.Resize(ref font, tate * 255 * 2);
                    }
                    else
                    {
                        Array.Resize(ref font, tate * 255);
                    }
                }
                else{

                    if (yoko > 8)
                    {
                        Array.Resize(ref ftable,  total+1);
                        Array.Resize(ref font, tate * (total+1) * 2);
                    }
                    else
                    {
                        Array.Resize(ref ftable, total+1);
                        Array.Resize(ref font, (total+1) * 255);
                    }
                
                
                }
                FileStream fs = new System.IO.FileStream(Path.GetFileNameWithoutExtension(path) + ".fnt", FileMode.Create, FileAccess.Write);
               // FileStream ffs = new System.IO.FileStream(Path.GetFileNameWithoutExtension(path) + ".table", FileMode.Create, FileAccess.Write);
                //FileStream fffs = new System.IO.FileStream(Path.GetFileNameWithoutExtension(path) + ".sjis", FileMode.Create, FileAccess.Write);
                if (zenkaku == false) {
                    fs.Write(header, 0, 17);
                }
                else
                {
                    fs.Write(header, 0, 16);
                    if (SJIS.Checked == true)
                    {
                        hex[0] = 1;
                    }
                    else {
                        hex[0] = 2;
                    }
                    fs.Write(hex, 0, 1);
                    byte[] hex3 = new byte[2];
                    byte[] hex4 = new byte[1024*50];
                    int c1=0;
                    int c2=0;
                    int bk = 0;
                    int st = 0;
                    int en = 0;
                    int kk = 0;

                    for (int k = 0; k < total+1; k++)
                    {
                        c1=ftable[k]>>8;
                        c2 = ftable[k]&0xFF;
                        if (SJIS.Checked == true)
                        {
                            //http://www.tohoho-web.com/wwwkanji.htm
                            if ((c1 & 1) == 1)
                            {
                                c1 = ((c1 + 1) >> 1) + 0x70;
                                c2 = c2 + 0x1f;
                            }
                            else
                            {
                                c1 = (c1 >> 1) + 0x70;
                                c2 = c2 + 0x7d;
                            }
                            if (c1 >= 0xa0) { c1 = c1 + 0x40; }
                            if (c2 >= 0x7f) { c2 = c2 + 1; }
                        }
                        else {
                            c1 += 0x80;
                            c2 += 0x80;
                        }


                        c1 = (c1 << 8) + c2;
                        if (bk == 0) {
                            st = c1;
                        }
                        else if(c1-bk !=1){
                            hex3 = BitConverter.GetBytes(st);
                            //fs.Write(hex3, 0, 2);
                            Array.Copy(hex3,0,hex4,kk*4,2);
                            hex3 = BitConverter.GetBytes(st+en);
                            //fs.Write(hex3, 0, 2);
                            Array.Copy(hex3, 0, hex4, (kk * 4)+2, 2);
                            st = c1;
                            en = 0;
                            kk++;
                        }
                        else{
                            en++;
                        }


                        bk = c1;

                        //hex3 = BitConverter.GetBytes(c1);             
                        //ffs.Write(hex3, 0, 2);
                    }
                    
                    hex= BitConverter.GetBytes(kk);
                    fs.Write(hex, 0, 1);
                    fs.Write(hex4, 0, kk*4);

                }
                fs.Write(font, 0, font.Length);
                fs.Close();
                //ffs.Close();
                //fffs.Close();

            }
        }
    }
}
