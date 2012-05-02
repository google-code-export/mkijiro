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
            JIS2SJIS.SelectedIndex = 0;
            sekitxt.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool uni = false;
            if((JIS2SJIS.SelectedIndex == 4) && (File.Exists("JIS0208.txt")==false)){
                uni = true;
            }

            if(lastdir != ""){
            ofd.InitialDirectory = lastdir;}
            else{
            ofd.InitialDirectory = Application.StartupPath;
            }
            ofd.Filter = "bdfファイル(*.bdf)|*.bdf";
            ofd.Title = "開くファイルを選択してください";

            if (((CMF.Checked == true && EUC.Checked == true) || (FC.Checked == true && SJIS.Checked == true) || (CMF.Checked == false)) && (uni == false))
            { 

                //ダイアログを表示する
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    lastdir = Path.GetDirectoryName(ofd.FileName);
                    string path = ofd.FileName;
                    string s = "";
                    string sss = "";
                    if((JIS2SJIS.SelectedIndex == 4)){
                    StreamReader ssr = new StreamReader("JIS0208.TXT", Encoding.GetEncoding(932));
                    sss = ssr.ReadToEnd();
                    ssr.Close();
                    }
                    StringBuilder ss = new System.Text.StringBuilder();
                    bool parse = false;
                    int stpos = 0;
                    int sttpos = 0;
                    int tmp = 0;
                    int tmpb = 0;
                    int tmp2 = 0;
                    int tmp3 = 0;
                    int basepos = 0;
                    int yoko = 0;
                    int tate = 0;
                    int total = 0;
                    bool fail = false;
                    bool zenkaku = false;
                    byte[] hex = new byte[4];
                    byte[] hexcmf = new byte[6];
                    byte[] header = { 0x46, 0x4F, 0x4E, 0x54, 0x58, 0x32, 0x42, 0x44, 0x46, 0x32, 0x46, 0x54, 0x58, 0x32, 0x8, 0x10, 0 };
                    UInt16[] ftable = new UInt16[1024 * 50];
                    byte[] font = new byte[1024 * 1024];

                    System.IO.StreamWriter sw = new System.IO.StreamWriter("filer_src.c", false, System.Text.Encoding.GetEncoding(932));
                    StreamReader sr = new System.IO.StreamReader(path, System.Text.Encoding.GetEncoding(932));
                    while (sr.Peek() > -1)
                    {
                        s = sr.ReadLine();
                        if (zenkaku == false && CMF.Checked == false && FILER.Checked == false && FC.Checked == false)
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
                        else if (zenkaku == true && CMF.Checked == false && FILER.Checked == false && FC.Checked == false)
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
                        else if (CMF.Checked == true || FILER.Checked == true || FC.Checked == true)
                        {

                            if (s.Contains("CHARSET")) { }
                            else if (s.Contains("ENCODING"))
                            {

                                total++;
                                ftable[total - 1] = Convert.ToUInt16(s.Replace("ENCODING", "").Trim(), 10);

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
                                        if (yoko != 12 && yoko !=6)
                                        {
                                            MessageBox.Show("12x12(13)以外は変換できません。");
                                            fail = true;
                                            break;
                                        }
                                    }
                                    if (l == 1)
                                    {
                                        tate = Convert.ToInt32(m.Value);
                                        if (tate < 12 || tate > 13)
                                        {
                                            MessageBox.Show("12x12(13)以外は変換できません。");
                                            fail = true;
                                            break;
                                        }
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
                                sttpos = 0;
                                parse = false;
                                if (FILER.Checked == true && yoko == 6)
                                {
                                    ss.Append("/*");
                                    ss.Append(ftable[total - 1].ToString("X2"));
                                    ss.Append("*/ ");
                                    for (int l = 0; l < 9; l++)
                                    {
                                        ss.Append("0x");
                                        ss.Append(font[((total) * 9) + l].ToString("X2"));
                                        ss.Append(",");
                                    }
                                    ss.AppendLine();
                                }
                            }
                            else if (parse)
                            {
                                tmp = Convert.ToInt32(s, 16);
                                if (CMF.Checked == true || FC.Checked == true)
                                {
                                    if (yoko == 12)
                                    {
                                        if ((stpos & 1) == 0)
                                        {
                                            tmp2 = tmp / 0x100;
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 0, 1);
                                            tmp2 = tmp;
                                        }
                                        if ((stpos & 1) == 1)
                                        {
                                            tmp2 = (tmp2 / 0x10) & 0xf;
                                            tmp2 += (tmp / 0x1000) * 0x10;
                                            tmp2 = (tmp2 & 0xf) * 0x10 + tmp2 / 0x10;
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 1, 1);
                                            tmp2 = (tmp / 0x10) & 0xff;
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 2, 1);
                                            Array.Copy(hexcmf, 0, font, (total * 18) + sttpos, 3);
                                            sttpos += 3;
                                        }
                                        stpos += 1;
                                    }
                                }
                                else {
                                    if (yoko == 6)
                                    {
                                        if ((stpos & 3) == 0)
                                        {
                                            tmp2 = tmp;
                                        }
                                        if ((stpos & 3) == 1)
                                        {
                                            tmp2 = tmp2 + ((tmp >> 6)&0x3);
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 0, 1);
                                            tmp2 = (tmp << 2) & 0xF0;
                                        }
                                        if ((stpos & 3) == 2)
                                        {
                                            tmp2 = tmp2 + ((tmp >> 4) & 0xF);
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 1, 1);
                                            tmp2 = (tmp << 4) & 0xc0;
                                        }
                                        if ((stpos & 3) == 3)
                                        {
                                            tmp2 = tmp2 + ((tmp >> 2) & 0x3F);
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 2, 1);
                                            Array.Copy(hexcmf, 0, font, (total * 9) + sttpos, 3);
                                            sttpos += 3;
                                        }
                                    }
                                    else if(yoko==12) {
                                        if ((stpos & 3) == 0)
                                        {
                                            tmp2 = (tmp>>8)&0xFc;
                                            tmp3 = (tmp>>2)&0xFc;
                                        }
                                        if ((stpos & 3) == 1)
                                        {
                                            tmp2 = tmp2 + ((tmp >> (6 + 8)) &0x3);
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 0, 1);
                                            tmpb = tmp <<6;
                                            tmp3 = tmp3 + ((tmpb >> (6 + 8)) & 0x3);
                                            hex = BitConverter.GetBytes(tmp3);
                                            Array.Copy(hex, 0, hexcmf, 3, 1);
                                            tmp2 = ((tmp >> 8) << 2) & 0xF0;
                                            tmp3 = ((tmpb >> 8) << 2) & 0xF0;
                                        }
                                        if ((stpos & 3) == 2)
                                        {
                                            tmp2 = tmp2 + ((tmp >> (4 + 8)) & 0xF);
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 1, 1);
                                            tmpb = tmp << 6;
                                            tmp3 = tmp3 + ((tmpb >> (4 + 8)) & 0xF);
                                            hex = BitConverter.GetBytes(tmp3);
                                            Array.Copy(hex, 0, hexcmf, 4, 1);
                                            tmp2 = ((tmp >>8) << 4) & 0xc0;
                                            tmp3 = ((tmpb >> 8) << 4) & 0xc0;
                                        }
                                        if ((stpos & 3) == 3)
                                        {
                                            tmp2 = tmp2 + ((tmp >> (2+8)) & 0x3f);
                                            hex = BitConverter.GetBytes(tmp2);
                                            Array.Copy(hex, 0, hexcmf, 2, 1);
                                            tmpb = tmp << 6;
                                            tmp3 = tmp3 + ((tmpb >> (2+8)) & 0x3f);
                                            hex = BitConverter.GetBytes(tmp3);
                                            Array.Copy(hex, 0, hexcmf, 5, 1);
                                            Array.Copy(hexcmf, 0, font, (total * 18) + sttpos, 3);
                                            Array.Copy(hexcmf, 3, font, (total * 18) + sttpos+9, 3);
                                            sttpos += 3;
                                        }
                                    
                                    }
                                    stpos += 1;
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
                            if (CMF.Checked == true || FILER.Checked==true || FC.Checked==true)
                            {
                                zenkaku = true;
                                Array.Resize(ref font, (total + 1) * 18);
                            }
                            else
                            {
                                Array.Resize(ref font, tate * 255 * 2);
                            }
                        }
                        else
                        {
                            Array.Resize(ref font, tate * 255);
                        }
                    }
                    else
                    {

                        if (yoko > 8)
                        {
                            Array.Resize(ref ftable, total + 1);
                            Array.Resize(ref font, tate * (total + 1) * 2);
                        }
                        else
                        {
                            Array.Resize(ref ftable, total + 1);
                            Array.Resize(ref font, (total + 1) * 255);
                        }


                    }

                    string fname = Path.GetFileNameWithoutExtension(path) + ".fnt";
                    string fontable = Path.GetFileNameWithoutExtension(path) + ".table";
                    if (CMF.Checked == true)
                    {
                        fname = "base.fnt";
                        fontable = "base.table";
                    }
                    if (FC.Checked == true)
                    {
                        fname = "base.fnt";
                        fontable = "base.table";
                    }
                    if (FILER.Checked == true)
                    {
                        fname = "filer.fnt";
                        fontable = "filer.table";
                    }
                    FileStream fs = new System.IO.FileStream(fname, FileMode.Create, FileAccess.Write);
                    FileStream ffs = new System.IO.FileStream(fontable, FileMode.Create, FileAccess.Write);
                    //FileStream eee = new System.IO.FileStream("a.txt", FileMode.Create, FileAccess.Write);
                    if (zenkaku == false && CMF.Checked == false && FILER.Checked == false && FC.Checked== false){
                        fs.Write(header, 0, 17);
                    }
                    else if (zenkaku == true)
                    {
                        fs.Write(header, 0, 16);
                        if (SJIS.Checked == true)
                        {
                            hex[0] = 1;
                        }
                        else
                        {
                            hex[0] = 2;
                        }
                        fs.Write(hex, 0, 1);
                        byte[] hex3 = new byte[2];
                        byte[] hexjis = new byte[5];
                        hexjis[0] = 0x1B;
                        hexjis[1] = 0x24;
                        hexjis[2] = 0x42;
                        byte[] crlf = new byte[2];
                        crlf[0] = 0xd;
                        crlf[1] = 0xa;
                        byte[] hex4 = new byte[1024 * 50];
                        int strpos = 3;
                        int c1 = 0;
                        int c2 = 0;
                        int code = 0;
                        int bk = 0;
                        int st = 0;
                        int en = 0;
                        int kk = 0;
                        string nn = "";
                        string sekitmp = "";
                        if (SEIKI.Checked == true)
                        {
                            strpos = Convert.ToInt32(pos.Text);
                            sekitmp = sekitxt.Text;
                            sekitmp = sekitmp.Replace("\\t","\t");
                            sekitmp = sekitmp.Replace("\\n","\n");
                        }

                        for (int k = 0; k < total + 1; k++)
                        {
                            c1 = ftable[k] >> 8;
                            c2 = ftable[k] & 0xFF;
                            if (SJIS.Checked == true)
                            {

                                //http://oku.edu.mie-u.ac.jp/~okumura/algo/
                                //http://www.st.rim.or.jp/~phinloda/cqa/cqa15.html#Q4 合成
                                if (JIS2SJIS.SelectedIndex == 0)
                                {
                                    if ((c1 & 1) != 0)
                                    {
                                        c2 += 0x20;
                                        if ((c2 & 0x80) == 0) c2--;
                                    }
                                    else c2 += 0x7E;

                                    c1 = (((c1 - 1) >> 1) + 0x91) ^ 0x20;
                                }
                                //http://www.tohoho-web.com/wwwkanji.htm
                                if (JIS2SJIS.SelectedIndex == 1)
                                {
                                    if ((c1 & 1) == 1)
                                    {
                                        c1 = ((c1 + 1) >> 1) + 0x70;
                                        c2 +=  0x1f;
                                    }
                                    else
                                    {
                                        c1 = (c1 >> 1) + 0x70;
                                        c2 += 0x7D;
                                    }
                                    if (c1 >= 0xa0) { c1 += 0x40; }
                                    if (c2 >= 0x7f) { c2 ++; }
                                }

                                //http://oku.edu.mie-u.ac.jp/~okumura/algo/
                                if (JIS2SJIS.SelectedIndex == 2)
                                {
                                    if ((c1 & 1) != 0)
                                    {
                                        if (c2 < 0x60) c2 += 0x1F;
                                        else c2 += 0x20;
                                    }
                                    else c2 += 0x7E;

                                    if (c1 < 0x5F) c1 = (c1 + 0xE1) >> 1;
                                    else c1 = (c1 + 0x161) >> 1;
                                }
                         
                                //http://www.unixuser.org/~euske/doc/kanjicode/
                                if (JIS2SJIS.SelectedIndex == 3)
                                {
                                    c1 -= 0x21;
                                    if ((c1 & 1) == 0)
                                    {
                                        c2 +=0x1f;
                                    }
                                    else c2 += 0x7E;

                                    if (c2 >= 0x7f && c2 <= 0x9d)
                                    {
                                        c2 += 1;
                                    }

                                    c1 >>= 1;
                                    if (c1 >= 0 && c1 <= 0x1e)
                                    {
                                        c1 += 0x81;
                                    }
                                    else if (c1 >= 0x1f && c1 <= 0x2e)
                                    {
                                        c1 += 0xc1;
                                    }

                                }

                                //SJIS　区点番号
                                if (JIS2SJIS.SelectedIndex == 4)
                                {
                                    c1 -= 0x20;
                                    c2 -= 0x20;
                                    if ((c1 & 1) == 0)
                                    {
                                        c2 += 0x9e;
                                    }
                                    else {
                                        if (c2 >= 1 && c2 <= 63) {
                                            c2 += 0x3f; 
                                        }
                                        else if (c2 >= 64 && c2 <= 94){
                                            c2 += 0x40;
                                        }
                                    }
                                    if (c1 >= 1 && c1 <= 62) { c1 = ((c1-1)>>1) + 0x81; }
                                    if (c1 >= 63 && c1 <= 94) { c1 =((c1-1)>>1) + 0xc1; }
                                }

                                //http://www.d2.dion.ne.jp/~imady/charset/charcode_mame.html#SJIS_JIS
                                 //■ JISX0208 → SJIS
                                 //   コードから 0x2121 を引く
                                 //   (コード & 0x100) が０でなければ コードに 0x9E, ０ならばコードに 0x40 を足す
                                 //   下位バイトが 0x7F 以上ならば コードに 1 を足す
                                 //   [上位バイトだけの処理] 上位バイトを右に１ビット シフトし、それに 0x81 を足す
                                //   (0xA000 <= コード) ならばコードに 0x4000 を足す 
                                if (JIS2SJIS.SelectedIndex == 5)
                                {
                                    c1 -= 0x21;
                                    c2 -= 0x21;
                                    if ((c1 & 1) !=0)
                                    {
                                        c2 += 0x9e;
                                    }
                                    else
                                    {
                                        c2 += 0x40;
                                    }
                                    if (c2 >= 0x7f)
                                    {
                                        c2 += 1;
                                    }
                                                                       
                                    c1 = (c1 >> 1) + 0x81;
                                    if (c1 >= 0xA0)
                                    {
                                        c1 += 0x40;
                                    }
                                }

                                //TXTテーブル正規表現
                                if (JIS2SJIS.SelectedIndex ==6)
                                {
                                    code = (c1 << 8) + c2;
                                    if (SEIKI.Checked == true) {
                                        nn = sekitmp.Replace("%J", code.ToString("X4"));
                                    }
                                    else
                                    {
                                        nn = "\n0x[0-9A-F]{4}\t0x" + code.ToString("X4");
                                    }
                                    Regex r =   new Regex(nn,  RegexOptions.ECMAScript);
                                    Match m = r.Match(sss);
                                    if (m.Success){
                                        code = Convert.ToInt32(m.Value.Substring(strpos, 4), 16);
                                        c1 = code >>8;
                                        c2 = code & 0xff;
                                    }
                                    else
                                    {
					//例外の補完
                                        if ((c1 & 1) == 1)
                                        {
                                            if (c2 < 0x60) c2 += 0x1F;
                                            else c2 += 0x20;
                                        }
                                        else c2 += 0x7E;
                                        if (c1 < 0x5F) c1 = (c1 + 0xE1) >> 1;
                                        else c1 = (c1 + 0x161) >> 1;
                                    }

                                }

                                //M＄ EUCJP経由
                                if (JIS2SJIS.SelectedIndex == 8)
                                {
                                    c1 += 0x80;
                                    c2 += 0x80;
                                    hex3[0] = (byte)c1;
                                    hex3[1] = (byte)c2;
                                    nn = System.Text.Encoding.GetEncoding(51932).GetString(hex3);
                                    hex3 = System.Text.Encoding.GetEncoding(932).GetBytes(nn);
                                    //eee.Write(hex3, 0, 2);
                                    //eee.Write(crlf, 0, 2);
                                    if (hex3.Length == 2)
                                    {
                                        c1 = hex3[0];
                                        c2 = hex3[1];
                                    }

                                }
                                //M$ ISO2022経由
                                if (JIS2SJIS.SelectedIndex == 7)
                                {
                                    //JIS C 6226-1978(第1・第2水準漢字) 	1B 24 40 	ESC $ @
                                    //JIS X 0208-1983(第1・第2水準漢字) 	1B 24 42 	ESC $ B
                                    //JIS X 0208-1990(第1・第2水準漢字) 	1B 26 40 1B 24 42 	ESC & @ ESC $ B
                                    //JIS X 0212-1990(補助漢字) 	1B 24 28 44 	ESC $ ( D
                                    //JIS X 0213:2000 1面(第1・第2水準漢字) 	1B 24 28 4F 	ESC $ ( O
                                    //JIS X 0213:2004 1面(第1・第2水準漢字) 	1B 24 28 51 	ESC $ ( Q
                                    //JIS X 0213:2000 2面(第3・第4水準漢字) 	1B 24 28 50 	ESC $ ( P
                                    //JIS X 0201 ラテン(半角英数) 	1B 28 4A 	ESC ( J
                                    //JIS X 0201 カナ(半角カナ) 	1B 28 49 	ESC ( I
                                    //ISO/IEC 646(ASCII) 	1B 28 42 	ESC ( B
                                    hexjis[3] = (byte)c1;
                                    hexjis[4] = (byte)c2;
                                    nn = System.Text.Encoding.GetEncoding(50222).GetString(hexjis);
                                    hex3 = System.Text.Encoding.GetEncoding(932).GetBytes(nn);
                                    //eee.Write(hex3, 0, 2);
                                    //eee.Write(crlf, 0, 2);
                                    if (hex3.Length == 2)
                                    {
                                        c1 = hex3[0];
                                        c2 = hex3[1];
                                    }
                                }


                            }
                            else if (EUC.Checked==true)
                            {
                                c1 += 0x80;
                                c2 += 0x80;
                            }


                            c1 = (c1 << 8) + c2;
                            if (bk == 0)
                            {
                                st = c1;
                            }
                            else if (c1 - bk != 1)
                            {
                                hex3 = BitConverter.GetBytes(st);
                                //fss.Write(hex3, 0, 2);
                                Array.Copy(hex3, 0, hex4, kk * 4, 2);
                                hex3 = BitConverter.GetBytes(st + en);
                                //fss.Write(hex3, 0, 2);
                                Array.Copy(hex3, 0, hex4, (kk * 4) + 2, 2);
                                st = c1;
                                en = 0;
                                kk++;
                            }
                            else
                            {
                                en++;
                            }


                            bk = c1;

                            if (CMF.Checked)
                            {
                                hex3 = BitConverter.GetBytes(c1);
                                ffs.Write(hex3, 0, 2);
                            }
                            if (FC.Checked)
                            {
                                hex3 = BitConverter.GetBytes(c1);
                                ffs.Write(hex3, 0, 2);
                            }
                            if (FILER.Checked == true && yoko == 12)
                            {
                                if (k < total)
                                {
                                    ss.Append("/*");
                                    ss.Append(c1.ToString("X2"));
                                    ss.Append("*/ ");
                                    for (int l = 0; l < 18; l++)
                                    {
                                        ss.Append("0x");
                                        ss.Append(font[((k + 1) * 18) + l].ToString("X2"));
                                        ss.Append(",");
                                    }
                                    ss.AppendLine();
                                }
                            }
                        }

                        hex = BitConverter.GetBytes(kk);
                        fs.Write(hex, 0, 1);
                        fs.Write(hex4, 0, kk * 4);

                    }
                    if (fail == false)
                    {
                        if (CMF.Checked == true)
                        {
                            fs.Write(font, 18, font.Length - 18);
                            if (File.Exists("newfont.dat") == true)
                            {
                                fs.Close();
                                ffs.Close();
                                FileStream ffffs = new System.IO.FileStream("base.table", FileMode.Open, FileAccess.Read);
                                byte[] table = new byte[ffffs.Length];
                                ffffs.Read(table, 0, table.Length);
                                ffffs.Close();
                                FileStream fon = new System.IO.FileStream("newfont.dat", FileMode.Open, FileAccess.Read);
                                byte[] ff = new byte[fon.Length];
                                fon.Read(ff, 0, ff.Length);
                                int bk = ff.Length;
                                Array.Resize(ref ff, bk * 100);
                                fon.Close();
                                FileStream fontx = new System.IO.FileStream("base.fnt", FileMode.Open, FileAccess.Read);
                                byte[] ftx = new byte[fontx.Length];
                                fontx.Read(ftx, 0, ftx.Length);
                                total = ftx[17];
                                long bkk = ftx.Length;
                                fontx.Close();
                                int ct = 0;
                                long pos = 0;
                                int dest = 0;
                                int seek2 = 0;
                                for (int t = 0; t < table.Length; t += 2, ct++)
                                {
                                    pos = 18 + (total * 4) + (ct * 18);
                                    seek2 = BitConverter.ToUInt16(table, t);
                                    if (pos <= bkk-18)
                                    {
                                        if (seek2 >= 0xA1A1 && seek2 <= 0xFCFF)
                                        {
                                            dest = (((seek2 / 0x100) - 0xa1) * 94) + ((seek2 & 0xff) - 0xa1);
                                        }

                                        Array.Copy(ftx, pos, ff, 2048 + dest * 18, 18);
                                    }
                                }
                                FileStream fss = new System.IO.FileStream("font_euc.dat", FileMode.Create, FileAccess.Write);
                                Array.Resize(ref ff, bk);
                                fss.Write(ff, 0, ff.Length);
                                fss.Close();
                            }
                            else
                            {
                                MessageBox.Show("newfont.datがありません。");
                            }
                        } 
                        else if (FC.Checked == true)
                        {
                            fs.Write(font, 18, font.Length - 18);
                            if (File.Exists("newfont.dat") == true)
                            {
                                fs.Close();
                                ffs.Close();
                                FileStream ffffs = new System.IO.FileStream("base.table", FileMode.Open, FileAccess.Read);
                                byte[] table = new byte[ffffs.Length];
                                ffffs.Read(table, 0, table.Length);
                                ffffs.Close();
                                FileStream fon = new System.IO.FileStream("newfont.dat", FileMode.Open, FileAccess.Read);
                                byte[] ff = new byte[fon.Length];
                                fon.Read(ff, 0, ff.Length);
                                int bk = ff.Length;
                                Array.Resize(ref ff, bk * 100);
                                fon.Close();
                                FileStream fontx = new System.IO.FileStream("base.fnt", FileMode.Open, FileAccess.Read);
                                byte[] ftx = new byte[fontx.Length];
                                fontx.Read(ftx, 0, ftx.Length);
                                total = ftx[17];
                                long bkk = ftx.Length;
                                fontx.Close();
                                int ct = 0;
                                long pos = 0;
                                int dest = 0;
                                int seek2 = 0;
                                for (int t = 0; t < table.Length; t += 2, ct++)
                                {
                                    pos = 18 + (total * 4) + (ct * 18);
                                    seek2 = BitConverter.ToUInt16(table, t);
                                    if (pos <= bkk - 18)
                                    {
                                        if (seek2 >= 0x8140 && seek2 <= 0x9FFF)
                                        {
                                            dest = (((seek2 >> 8) - 0x81) * 192) + ((seek2 & 0xff) - 0x40);
                                        }
                                        else if (seek2 >= 0xE040 && seek2 <= 0xFCFF)
                                        {
                                            dest = (192 * 31) + (((seek2 >> 8) - 0xE0) * 192) + ((seek2 & 0xff) - 0x40);//0xBF80
                                        }

                                        Array.Copy(ftx, pos, ff, 2048 + dest * 18, 18);
                                    }
                                }

                                
                        byte[] bb = new byte[2];
                        byte[] bsss = new byte[65535*2];
                        string utf16st ="";
                        byte[] bbb ;
                        for(UInt16 i=0;i<0xFFFF;i++){
                            bb[0] = Convert.ToByte(i & 0xff);
                            bb[1] = Convert.ToByte(i >>8);
                            utf16st = System.Text.Encoding.GetEncoding(1200).GetString(bb);
                            bbb = System.Text.Encoding.GetEncoding(932).GetBytes(utf16st);
                        Array.Resize(ref bbb,2);
                        Array.Copy(bbb,0,ff,0x610d6+ i*2,2);//0x610D6 table 開始
                        }

                                FileStream fss = new System.IO.FileStream("sjis.dat", FileMode.Create, FileAccess.Write);
                                Array.Resize(ref ff, 528598);
                                fss.Write(ff, 0, ff.Length);
                                fss.Close();
                            }
                            else
                            {
                                MessageBox.Show("newfont.datがありません。");
                            }
                        }
                        else
                        {
                            if (FILER.Checked == true)
                            {
                                fs.Write(font, 9 * (yoko / 6), font.Length - 9 * (yoko / 6));
                            sw.Write(ss);}
                            else{
                                fs.Write(font, 0, font.Length);
                            }
                        }
                    }
                    sw.Close();
                    fs.Close();
                    ffs.Close(); System.Media.SystemSounds.Beep.Play();

                    if (FILER.Checked == false)
                    {
                        File.Delete("filer_src.c");
                    }
                    if (CMF.Checked == false || FC.Checked ==false)
                    {
                        File.Delete(fontable);
                    }
                }
            }
            else if (uni == true) {
                MessageBox.Show("JIS0208.TXTがありません。Unicode Consortiumからダウンロードしてください。\nftp://ftp.unicode.org/Public/MAPPINGS/OBSOLETE/EASTASIA/JIS/JIS0208.TXT\nまた正規表現を変更すればほかサイトのものでも使用可能です");
            }
            else
            {
                MessageBox.Show("CMFEUC用フォント作成する場合はJIS->EUCを選んで下さい。\nFREECHEATSJIS用フォントはJIS->SJISを選んで下さい");
            }
        }

        private void CMF_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FILER_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CMF_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void JIS2SJIS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JIS2SJIS.SelectedIndex == 6) {
                SEIKI.Enabled = true;
                pos.Enabled = true;
                sekitxt.Enabled= true;
                label1.Enabled = true;
            }
            else
            {
                SEIKI.Enabled = false;
                pos.Enabled = false;
                sekitxt.Enabled = false;
                label1.Enabled = false;
            }
        }

        private void sekitxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sekitxt.SelectedIndex == 0) {
                pos.Text = "3";
            }
            if (sekitxt.SelectedIndex == 1)
            {
                pos.Text = "5";
            }

        }

        private void SJIS_CheckedChanged(object sender, EventArgs e)
        {
            if (SJIS.Checked == true)
            {
                groupBox2.Enabled = true;
            }
            else {
                groupBox2.Enabled = false;
            }

        }
    }
}
