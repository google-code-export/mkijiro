using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Media;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO.Compression;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        
        string isofile = "NULL";
        long trimsize = 0;
        string open_dir = "";

        [DllImport("libmecab.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr mecab_new2(string arg);
        [DllImport("libmecab.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        private extern static string mecab_sparse_tostr(IntPtr m, string str);
        [DllImport("libmecab.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void mecab_destroy(IntPtr m);


        private void textBox2_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
                e.Handled = true;
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = System.Text.RegularExpressions.Regex.Replace(textBox2.Text, "[^0-9]", "");
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (File.Exists(Application.StartupPath +"\\" + "setting"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(Application.StartupPath + "\\" + "setting", System.Text.Encoding.GetEncoding(932));
                string s = "";
                while (sr.Peek() > -1)
                {
                    s = sr.ReadLine();
                    if (s.Contains("MECAB"))
                    {
                        checkBox1.Checked = true;
                        checkBox2.Checked = true;
                    }
                    if (s.Contains("BUFFER"))
                    {
                        textBox2.Text = s.Remove(0, 6);
                    }
                }
                sr.Close();
            }

            if (Directory.Exists(open_dir) == false) {
                open_dir = Application.StartupPath;
            }

            string[] cmds;
            cmds = System.Environment.GetCommandLineArgs();
            //コマンドライン引数の表示
            int i = 0;
            foreach (string cmd in cmds)
            { 
                if(i==1){
                    isofile = cmd;
            }
                i++;
            }
            if (File.Exists(isofile) ==true){
            textBox1.Text = getsize(isofile);
            if (textBox1.Text.Contains("ません"))
            {

                groupBox1.Enabled = false;
                sectorview.Enabled = false;
            }
            else if (textBox1.Text.Contains("Deflate")) {
                        groupBox1.Visible = false;
                        groupBox3.Visible = true;
                        groupBox2.Visible = false;
                        groupBox4.Visible = true;
                        groupBox1.Enabled = true;
                        sectorview.Enabled = true;
            }
             else 
            {
                groupBox1.Enabled = true;
                sectorview.Enabled = true;
            }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.StreamWriter sr = new System.IO.StreamWriter(Application.StartupPath + "\\" + "setting", false, System.Text.Encoding.GetEncoding(932));
            StringBuilder s = new StringBuilder();
            if(checkBox1.Checked == true)
            {
                s.AppendLine("MECAB");
            }
            else
            {
                s.AppendLine();
            }

            if (textBox2.Text != "") {                 
                s.Append("BUFFER");
                s.AppendLine(textBox2.Text);
            }
            sr.Write(s.ToString());
            
            sr.Close();
        }

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
            this.groupBox3.Location = new Point(7, 169);
            this.groupBox4.Location = new Point(7, 265);
            this.MinimumSize = new Size(244, 383);
            this.MaximumSize = new Size(244, 383);
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

            button5.Enabled = false;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            isofile = s[0];
            button_bool(s[0]);
            //groupBox1.Visible = true;
            //groupBox3.Visible = false;
            //groupBox2.Visible = true;
            //groupBox4.Visible = false;
            //textBox1.Text = getsize(s[0]);
            //if (textBox1.Text.Contains("ません"))
            //{

            //    groupBox1.Enabled = false;
            //    sectorview.Enabled = false;
            //}
            //else
            //{
            //    groupBox1.Enabled = true;
            //    sectorview.Enabled = true;
            //}
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

            button5.Enabled = false;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            isofile = s[0];
            button_bool(s[0]);
            //groupBox1.Visible = true;
            //groupBox3.Visible = false;
            //groupBox2.Visible = true;
            //groupBox4.Visible = false;
            //textBox1.Text = getsize(s[0]);
            //if (textBox1.Text.Contains("ません"))
            //{
            //    groupBox1.Enabled = false;
            //    sectorview.Enabled = false;
            //}
            //else
            //{
            //    groupBox1.Enabled = true;
            //    sectorview.Enabled = true;
            //}
        }

        private void button_bool(string s)
        {
            groupBox1.Visible = true;
            groupBox3.Visible = false;
            groupBox2.Visible = true;
            groupBox4.Visible = false;
            textBox1.Text = getsize(s);
            if (textBox1.Text.Contains("ません"))
            {
                groupBox1.Enabled = false;
                sectorview.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                sectorview.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button5.Enabled = false; 
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = "default.html";
            ofd.InitialDirectory = open_dir;
            ofd.Filter =
                "UMDイメージファイル(*.iso;*.cso)|*.iso;*.cso";
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                isofile = ofd.FileName;
                open_dir = Path.GetDirectoryName(isofile);
                button_bool(isofile);
            }
        }


        //IMAGECHECK
        private string getsize(string filepath)
        {
            label1.Text = "ファイルサイズ:";
            label2.Text = "セクター算出　:";

            if (File.Exists(filepath) == true)
            {
            FileStream fs = new FileStream(filepath,FileMode.Open, FileAccess.Read);
            long isosize = 0;
            long isobig = 0;
            long fsize = fs.Length;
            
                if (fs.Length > 0x8060)
                {
                    byte[] bs = new byte[8];
                    byte[] big = new byte[8];
                    byte[] str = new byte[5];
                    byte[] str2 = new byte[9];
                    byte[] x = new byte[3];
                    string iso = "", iso2 = "";
                    fs.Read(str, 0, 4);
                    iso = Encoding.GetEncoding(0).GetString(str);
                    iso = iso.Replace("\x0", "");
                    if (iso == "DAX")
                    {
                        fs.Close();
                        return "Deflate圧縮ISO,DAXには対応してません";
                    }
                    else if (iso == "CISO")
                    {
                        groupBox1.Visible = false;
                        groupBox3.Visible = true;
                        groupBox2.Visible = false;
                        groupBox4.Visible = true;

                        byte[] integer = new byte[8];
                        byte[] source = new byte[2048];
                        fs.Seek(8, System.IO.SeekOrigin.Begin);
                        fs.Read(integer, 0, 4);
                        long size = BitConverter.ToInt64(integer, 0);
                        fs.Seek(0x10, System.IO.SeekOrigin.Begin);
                        fs.Read(integer, 0, 4);
                        long sec = BitConverter.ToInt64(integer, 0);
                        fs.Seek(0x14, System.IO.SeekOrigin.Begin);
                        fs.Read(integer, 0, 4);
                        int align = BitConverter.ToInt32(integer, 0) >> 8;
                        long ct = size / sec;
                        int[] offset_csio = new int[ct + 1];
                        for (int i = 0; i < ct + 1; i++)
                        {
                            fs.Seek(0x18 + 4 * i, System.IO.SeekOrigin.Begin);
                            fs.Read(integer, 0, 4);
                            offset_csio[i] = (BitConverter.ToInt32(integer, 0)); //& 0x7fffffff) << align;
                        }

                        fs.Seek((offset_csio[16] & 0x7fffffff) << align, System.IO.SeekOrigin.Begin);
                        fs.Read(source, 0, ((offset_csio[17] & 0x7fffffff) - (offset_csio[16] & 0x7fffffff)) << align);

                        if ((offset_csio[16] & 0x80000000) != 0)
                        {
                        }
                        else
                        {
                            MemoryStream ms = new MemoryStream();
                            ms.Write(source, 0, 2048);
                            ms.Position = 0;
                            DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                            zipStream.Read(source, 0, 2048);
                            zipStream.Close();
                            ms.Close();
                        }

                        Array.Copy(source, 0x50, integer, 0, 4);
                        trimsize = BitConverter.ToInt64(integer, 0) << 11;
                        label1.Text = "推定サイズ　;" + size.ToString();
                        label2.Text = "セクター算出;" + trimsize.ToString();

                        sizecolor(size, trimsize);
                        fs.Close();

                        return "Deflate圧縮ISO,CSOは圧縮されてるためアンパックが必要です、推定サイズ情報はアンパック後の予想サイズです";
                    }
                    else if (iso == "JISO")
                    {
                        fs.Close();
                        return "LZO圧縮ISO,JSOには対応してません";
                    }
                    else if (iso != "")
                    {
                        fs.Close();
                        return "ISOではありません";
                    }
                    fs.Seek(0x8001, SeekOrigin.Begin);
                    fs.Read(str, 0, 5);
                    fs.Seek(0x8008, SeekOrigin.Begin);
                    fs.Read(str2, 0, 9);
                    fs.Seek(0x8050, SeekOrigin.Begin);
                    fs.Read(bs, 0, 3);
                    fs.Seek(0x8055, SeekOrigin.Begin);
                    fs.Read(big, 0, 3);
                    fs.Close();
                    Array.ConstrainedCopy(big, 0, x, 0, 3);
                    for (int i = 0; i < 3; i++)
                    {
                        Array.ConstrainedCopy(x, 2 - i, big, i, 1);
                    }
                    iso = Encoding.GetEncoding(0).GetString(str);
                    iso2 = Encoding.GetEncoding(0).GetString(str2);
                    isosize = BitConverter.ToInt64(bs, 0);
                    isobig = BitConverter.ToInt64(big, 0);
                    if (isosize != isobig || iso != "CD001")
                    {
                        return "ISOではありません";
                    }
                    if (iso2.Contains("PSP GAME") || iso2.Contains("UMD VIDEO"))
                    { }
                    else
                    {
                        return "ISOではありません";
                    }
                    isosize *= 2048;
                    trimsize = isosize;
                    sizecolor(fsize, trimsize);
                    label1.Text += Convert.ToString(fsize);
                    label2.Text += Convert.ToString(isosize);
                    if (isosize - fsize == 2048)
                    {
                        return "M33USBマウントやSSSで発生する-2048サイズ欠けです,セクタビュー最後で00かFFが連続してない場合バッドバンプの可能性が高いです\n\nPSPFILER/ISOTOOL/TEMPARなどを使ってください\r\nまたUSBマウントは修正版がでているので使ってください";
                    }
                    else if (isosize - fsize == 6144)
                    {
                        return "SSSで発生する-6144サイズ欠けです,セクタビュー最後で00かFFが連続してない場合バッドバンプの可能性が高いです\n手動補完するかPSPFILER/ISOTOOL/TEMPARなどを使ってください";
                    }
                    else if (isosize < fsize)
                    {
                        button5.Enabled = true;
                        return "オーバーダンプです,トリムすると正常になることが多いです\r\nトリムはメモリースティック上だと時間がかかるため、高速なHDD/SSD/RAMDISK上での実行を推奨します";
                    }
                    else if (isosize == fsize)
                    {
                        FileStream sector = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                        byte[] beforefinal = new byte[2048];
                        byte[] finalsector = new byte[2048];
                        byte[] hash = new byte[32];
                        string NU = "70BC8F4B72A86921468BF8E8441DCE51";//null
                        string FF = "0D7DC4266497100E4831F5B31B6B274F";//FF

                        sector.Seek(isosize - 8192, SeekOrigin.Begin);
                        sector.Read(beforefinal, 0, 2048);
                        sector.Read(finalsector, 0, 2048);

                        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                        Array.ConstrainedCopy(beforefinal, 2016, hash, 0, 32);
                        byte[] b = md5.ComputeHash(hash);
                        string result = BitConverter.ToString(b).ToUpper().Replace("-", "");
                        Array.ConstrainedCopy(finalsector, 0, hash, 0, 32);
                        byte[] c = md5.ComputeHash(hash);
                        string result2 = BitConverter.ToString(c).ToUpper().Replace("-", "");

                        sector.Seek(isosize - 4096, SeekOrigin.Begin);
                        sector.Read(beforefinal, 0, 2048);
                        sector.Read(finalsector, 0, 2048);

                        Array.ConstrainedCopy(beforefinal, 2016, hash, 0, 32);
                        byte[] d = md5.ComputeHash(hash);
                        string result3 = BitConverter.ToString(d).ToUpper().Replace("-", "");
                        Array.ConstrainedCopy(finalsector, 0, hash, 0, 32);
                        byte[] e = md5.ComputeHash(hash);
                        string result4 = BitConverter.ToString(e).ToUpper().Replace("-", "");

                        sector.Close();

                        if (result == result2)
                        {//FILLED wiht NULL
                            return "正常なサイズです";
                        }
                        else if (result == FF && result2 == NU)
                        {
                            return "サイズは正常です,0xFF埋めのあと0x00埋めになってます(最終-6k付近不連続ダミー判定)\r\nセクタービューで確認してください";
                        }
                        else if (result != NU && result2 == NU)
                        {
                            return "サイズは正常ですが,バッドダンプの可能性があります(最終-6k付近データ連続判定)\r\nセクタービューで確認してください";
                        }
                        else if (result3 == FF && result4 == NU)
                        {
                            return "サイズは正常です,0xFF埋めのあと0x00埋めになってます(最終-2k付近不連続ダミー判定)\r\nヴァルキリープロファイルなどが該当しますが正常なイメージだと確認済みです\r\nセクタービューで確認してください";
                        }
                        else if (result3 != NU && result4 == NU)
                        {
                            return "サイズは正常ですが,バッドダンプの可能性があります(最終-2k付近データ連続判定)\r\nセクタービューで確認してください";
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
            else
            {
                return "ファイルが存在しません";
            }
        }

        //ID
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

        //LABEL
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
                    textBox1.Text = "ISOラベル名が有りませんでした";
                }
            }
            else
            {
                textBox1.Text = "ファイルが存在しません";
            }
        }

        //PSF
        private void button3_Click(object sender, EventArgs e)//psfname
        {

            if (System.IO.File.Exists(isofile))
            {
                byte[] lba = new byte[4];
                byte[] size = new byte[8];
                byte[] psfname = new byte[129];
                byte[] sector = new byte[2048];
                byte[] str = new byte[9];
                int i =0,k=0,z=0;
                FileStream fs = new FileStream(isofile, FileMode.Open, FileAccess.Read);

                fs.Seek(0x8008, SeekOrigin.Begin);
                fs.Read(str, 0, 9);
                string iso = Encoding.GetEncoding(0).GetString(str);
                fs.Seek(0x8050, SeekOrigin.Begin);
                fs.Read(size, 0, 5);
                long lbatotal = BitConverter.ToInt64(size, 0);
                lbatotal *= 2048;
                if (lbatotal - fs.Length < 2048*3 && (iso.Contains("PSP GAME") || iso.Contains("UMD VIDEO")))
                {
                    fs.Seek(0x808C, SeekOrigin.Begin);//0x809E root
                    fs.Read(lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    fs.Seek(z * 2048, SeekOrigin.Begin);
                    fs.Read(sector, 0, 2048);
                    //PSP_GAME,UMD_VIDEO,LPATHTABLE
                    i = 6;
                    while (true)
                    {
                        if (iso.Contains("PSP GAME") && sector[i] == 0x50 && sector[i + 1] == 0x53 && sector[i + 2] == 0x50 && sector[i + 3] == 0x5f && sector[i + 4] == 0x47) break;
                        if (iso.Contains("UMD VIDEO") && sector[i] == 0x55 && sector[i + 1] == 0x4D && sector[i + 2] == 0x44 && sector[i + 3] == 0x5f && sector[i + 4] == 0x56) break;
                        if (i > 2038) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                        i++;
                    }
                    Array.ConstrainedCopy(sector, i - 6, lba, 0, 2);//-31 0x809E rootdir
                    z = BitConverter.ToInt32(lba, 0);
                    if (z * 2048 > fs.Length) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                    fs.Seek(z * 2048, SeekOrigin.Begin);
                    fs.Read(sector, 0, 2048);
                    i = 31;
                    //PARAM.SFO
                    while (true)
                    {
                        if (sector[i] == 0x50 && sector[i + 1] == 0x41 && sector[i + 2] == 0x52 && sector[i + 3] == 0x41) break;
                        if (i > 2038) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                        i++;
                    }
                    Array.ConstrainedCopy(sector, i - 31, lba, 0, 3);
                    z = BitConverter.ToInt32(lba, 0);
                    if (z * 2048 > fs.Length) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                    fs.Seek(z * 2048, SeekOrigin.Begin);
                    fs.Read(sector, 0, 2048);
                    Array.ConstrainedCopy(sector, 12, lba, 0, 4);
                    i = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(sector, 8, lba, 0, 4);
                    k = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(sector, 16, lba, 0, 4);
                    z = BitConverter.ToInt32(lba, 0);
                    byte[] tmp = new byte[200];
                    Array.ConstrainedCopy(sector, k, tmp, 0, 200);
                    string strs = Encoding.GetEncoding(65001).GetString(tmp);
                    strs = strs.Substring(0, strs.IndexOf("\x0\x0"));
                    string[] name = strs.Split('\x0');
                    for (k = 0; ; k++)
                    {
                        if (name[k] == "TITLE") break;
                        if (k == z)
                        {
                            fs.Close();textBox1.Text ="PSF取得に失敗しました";
                            return;
                        }
                    }
                    z = (k+2) * 16;
                    Array.ConstrainedCopy(sector, z , lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(sector, z + i, psfname, 0, 129);
                    fs.Close();
                    string rpname = Encoding.GetEncoding(65001).GetString(psfname);
                    i = rpname.IndexOf("\x0");
                    rpname = rpname.Substring(0, i) + ".ISO";
                    string[] dosmoji;
                    dosmoji = new string[11] {"\\", "/", ":", "*", "?", "\"", "<", ">", "|", "\n", "\r" };
                    for (i = 0; i < 11; i++)
                    {
                        if (rpname.Contains(dosmoji[i])){
                            rpname = rpname.Replace(dosmoji[i], " ");
                        }
                    }

                    if(checkBox1.Checked == true) {
                        rpname = rpname.Replace("　", " ");
                        IntPtr mecab = mecab_new2("-Oyomi");
                        rpname = mecab_sparse_tostr(mecab, rpname);
                        mecab_destroy(mecab);
                        
                        string[] kana = {"・","ー","ァ","ィ","ゥ","ェ","ォ","ヶ","ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ヤ", "ユ", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "ヰ", "ヱ", "ヲ", "ン", "ガ", "ギ", "グ", "ゲ", "ゴ", "ザ", "ジ", "ズ", "ゼ", "ゾ", "ダ", "ヂ", "ヅ", "デ", "ド", "バ", "ビ", "ブ", "ベ", "ボ", "パ", "ピ", "プ", "ペ", "ポ", "キャ", "キュ", "キョ", "シャ", "シュ", "ショ", "チャ", "チュ", "チョ", "ニャ", "ニュ", "ニョ", "ヒャ", "ヒュ", "ヒョ", "ミャ", "ミュ", "ミョ", "リャ", "リュ", "リョ", "ギャ", "ギュ", "ギョ", "ジャ", "ジュ", "ジョ", "ビャ", "ビュ", "ビョ", "ピャ", "ピュ", "ピョ", "ファ", "フィ", "フェ", "フォ", "ヴァ", "ヴィ", "ヴ", "ヴェ", "ヴォ"};
                        string[] roma = {" ","-","a","i","u","e","o","ke", "A", "I", "U", "E", "O", "KA", "KI", "KU", "KE", "KO", "SA", "SHI", "SU", "SE", "SO", "TA", "CHI", "TSU", "TE", "TO", "NA", "NI", "NU", "NE", "NO", "HA", "HI", "FU", "HE", "HO", "MA", "MI", "MU", "ME", "MO", "YA", "YU", "YO", "RA", "RI", "RU", "RE", "RO", "WA", "I", "E", "O", "N", "GA", "GI", "GU", "GE", "GO", "ZA", "JI", "ZU", "ZE", "ZO", "DA", "JI", "ZU", "DE", "DO", "BA", "BI", "BU", "BE", "BO", "PA", "PI", "PU", "PE", "PO", "KYA", "KYU", "KYO", "SHA", "SHU", "SHO", "CHA", "CHU", "CHO", "NYA", "NYU", "NYO", "HYA", "HYU", "HYO", "MYA", "MYU", "MYO", "RYA", "RYU", "RYO", "GYA", "GYU", "GYO", "JA", "JU", "JO", "BYA", "BYU", "BYO", "PYA", "PYU", "PYO", "FA", "FI", "FE", "FO", "VA", "VI", "VU", "VE", "VO" };
                        //

                        string[] suuji = { "零", "壱", "弐", "参", "廿", "卅", "卌"};
                        string[] romasuuji = { "ZERO", "ITI", "NI", "SAN", "NIJUU", "SANJUU", "SIJUU"};

                        for (i = 6; i >= 0; i--)
                        {
                            if (rpname.Contains(suuji[i]) == true)
                            {
                                rpname = rpname.Replace(suuji[i], romasuuji[i]);
                            }
                        }

                        rpname = abc123ToHankaku(rpname);

                        string cp = "";
                        string rp2 = rpname;
                        for(i=0;i<rp2.Length-4;i++){
                            cp = rp2.Substring(i, 1);
                            char x = Convert.ToChar(cp);
                            if(hankaku_zenkana(x)==false){
                                rpname = rpname.Replace(cp, "");
                            }
                        }

                        for (i = 122; i>=0 ; i--) {
                            if (rpname.Contains(kana[i]) == true) {
                                rpname = rpname.Replace(kana[i],roma[i]);
                            }
                        }
                        int tu = 0;
                        while(true){
                            tu = rpname.LastIndexOf("ッ");
                            if (tu == -1) break;
                            cp = rpname.Substring(tu + 1, 1);
                            rpname = rpname.Substring(0, tu) + cp + rpname.Substring(tu+1, rpname.Length-tu-1);
                        }
                        rpname = Strings.StrConv(rpname, VbStrConv.Narrow, 0x0411);

                        //string aa = "・ーヾヿ";
                        //aa = Strings.StrConv(aa, VbStrConv.Narrow, 0x0411);

                        for (i = 0; i < 11; i++)
                        {
                            if (rpname.Contains(dosmoji[i]))
                            {
                                rpname = rpname.Replace(dosmoji[i], " ");
                            }
                        }
                    }

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
                    textBox1.Text ="6k以上欠けてるためPARAM.SFO取得を停止しました";
                }
            }
            else
            {
                textBox1.Text = "ファイルが存在しません";
            }
        }


        static string abc123ToHankaku(string s)
        {

            Regex re = new Regex("[０-９Ａ-Ｚａ-ｚ：－　]+");
            string output = re.Replace(s, myReplacer);

            return output;
        }

        static string myReplacer(Match m)
        {
            return Strings.StrConv(m.Value, VbStrConv.Narrow, 0);
        }

        public static bool hankaku_zenkana(char c)
        {
            return ('\u0020' <= c && c <= '\u007E')
                || ('\u30A0' <= c && c <= '\u30FC')
                || ('\u31F0' <= c && c <= '\u31FF')
                || ('\u3099' <= c && c <= '\u309C');
        }        

        //OVERFUMPTRIM
        private void button5_Click(object sender, EventArgs e)
        {

            int bufsize = Convert.ToInt32(textBox2.Text)<<20;
            byte[] bs = new byte[bufsize];
            File.Delete(isofile + ".trim");
            for( long z = 0; z < trimsize; z+=bufsize)
            {
            FileStream fs = new FileStream(isofile,FileMode.Open,FileAccess.Read);
            fs.Seek(z, SeekOrigin.Begin);

            if (z + bufsize > trimsize) {
                bufsize= Convert.ToInt32(trimsize - z);
            }
            fs.Read(bs, 0, bufsize);
            fs.Close();
            FileStream wr =  new FileStream(isofile + ".trim", FileMode.Append, FileAccess.Write);
            wr.Write(bs, 0, bufsize);
                wr.Close();
            }
            textBox1.Text = "トリムが完了しました";
            SystemSounds.Asterisk.Play();
            button5.Enabled = false;
        }

        //SECTORE VIEW
        private void button6_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Text = isofile;
            f.ShowDialog(this);
            f.Dispose();
        }
        
        //ID_CISO
        private void button6_Click_1(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(isofile))
            {
                System.IO.FileStream fs = new System.IO.FileStream(isofile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //ファイルを読み込むバイト型配列を作成する
                byte[] integer = new byte[8];
                byte[] source = new byte[2048];
                byte[] bufferw = new byte[1 << 20];
                fs.Read(source, 0, 2048);
                fs.Seek(8, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                long size = BitConverter.ToInt64(integer, 0);
                fs.Seek(0x10, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                long sector = BitConverter.ToInt64(integer, 0);
                fs.Seek(0x14, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                int align = BitConverter.ToInt32(integer, 0) >> 8;
                long counter = size / sector;
                //閉じる
                int[] offset = new int[counter + 1];
                for (int i = 0; i < counter + 1; i++)
                {
                    fs.Seek(0x18 + 4 * i, System.IO.SeekOrigin.Begin);
                    fs.Read(integer, 0, 4);
                    offset[i] = (BitConverter.ToInt32(integer, 0) & 0x7fffffff) << align;
                }

                fs.Seek(offset[16], System.IO.SeekOrigin.Begin);
                fs.Read(source, 0, offset[17] - offset[16]);
                if ((offset[17] - offset[16]) != (2048 >> align))
                {

                    MemoryStream ms = new MemoryStream();
                    ms.Write(source, 0, 2048);
                    ms.Position = 0;
                    DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                    zipStream.Read(source, 0, 2048);
                    zipStream.Close();
                    ms.Close();
                }

                byte[] lba = new byte[4];
                byte[] isize = new byte[8];
                byte[] psfname = new byte[129];

                Array.Copy(source, 0x50, isize, 0, 5);
                long lbatotal = BitConverter.ToInt64(isize, 0);
                lbatotal *= 2048;
                sizecolor(size, lbatotal);
                label1.Text = "推定サイズ　;" + size.ToString();
                label2.Text = "セクター算出;" + lbatotal.ToString();

                fs.Close();

                byte[] gid = new byte[10];
                Array.Copy(source, 0x373, gid, 0, 10);
                string rpname = Encoding.GetEncoding(0).GetString(gid) + ".CSO";
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

        //LABEL_CISO
        private void button7_Click(object sender, System.EventArgs e)
        {
            if (System.IO.File.Exists(isofile))
            {
            System.IO.FileStream fs = new System.IO.FileStream(isofile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] integer = new byte[8];
            byte[] source = new byte[2048];
            byte[] bufferw = new byte[1 << 20];
            fs.Read(source, 0, 2048);
            fs.Seek(8, System.IO.SeekOrigin.Begin);
            fs.Read(integer, 0, 4);
            long size = BitConverter.ToInt64(integer, 0);
            fs.Seek(0x10, System.IO.SeekOrigin.Begin);
            fs.Read(integer, 0, 4);
            long sector = BitConverter.ToInt64(integer, 0);
            fs.Seek(0x14, System.IO.SeekOrigin.Begin);
            fs.Read(integer, 0, 4);
            int align = BitConverter.ToInt32(integer, 0)>>8;
            long counter = size / sector;
            //閉じる
            int[] offset = new int[counter + 1];
            for (int i = 0; i < counter + 1; i++)
            {
                fs.Seek(0x18 + 4 * i, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                offset[i] = (BitConverter.ToInt32(integer, 0) & 0x7fffffff) << align;
            }

            fs.Seek(offset[16], System.IO.SeekOrigin.Begin);
            fs.Read(source, 0, offset[17] - offset[16]);



            if ((offset[17] - offset[16]) != (2048 >> align))
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(source, 0, 2048);
                ms.Position = 0;
                DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                zipStream.Read(source, 0, 2048);
                zipStream.Close();
                ms.Close();
            }

            byte[] lba = new byte[4];
            byte[] isize = new byte[8];
            byte[] psfname = new byte[129];

            Array.Copy(source, 0x50, isize, 0, 5);
            long lbatotal = BitConverter.ToInt64(isize, 0);
            lbatotal *= 2048;
            sizecolor(size, lbatotal);
            label1.Text = "推定サイズ　;" + (counter << 11).ToString();
            label2.Text = "セクター算出;" + lbatotal.ToString();

            fs.Close();

            byte[] isohead = new byte[32];
            Array.Copy(source,0x28, isohead, 0, 32);
            string rpname = Encoding.GetEncoding(0).GetString(isohead).Trim();
            rpname += ".CSO";
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

        //PSF_CISO
        private void button8_Click(object sender, System.EventArgs e)
        {

            if (System.IO.File.Exists(isofile))
            {
                System.IO.FileStream fs = new System.IO.FileStream(isofile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //ファイルを読み込むバイト型配列を作成する
                byte[] integer = new byte[8];
                byte[] source = new byte[2048];
                byte[] bufferw = new byte[1 << 20];
                fs.Read(source, 0, 2048);
                fs.Seek(8, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                long size = BitConverter.ToInt64(integer, 0);
                fs.Seek(0x10, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                long sector = BitConverter.ToInt64(integer, 0);
                fs.Seek(0x14, System.IO.SeekOrigin.Begin);
                fs.Read(integer, 0, 4);
                int align = BitConverter.ToInt32(integer, 0) >> 8;
                long counter = size / sector;
                //閉じる
                int[] offset = new int[counter + 1];
                for (int i = 0; i < counter + 1; i++)
                {
                    fs.Seek(0x18 + 4 * i, System.IO.SeekOrigin.Begin);
                    fs.Read(integer, 0, 4);
                    offset[i] = (BitConverter.ToInt32(integer, 0) & 0x7fffffff) << align;
                }

                fs.Seek(offset[16], System.IO.SeekOrigin.Begin);
                fs.Read(source, 0, offset[17] - offset[16]);


                if ((offset[17] - offset[16]) != (2048 >> align))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(source, 0, 2048);
                    ms.Position = 0;
                    DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                    zipStream.Read(source, 0, 2048);
                    zipStream.Close();
                    ms.Close();
                }

                byte[] lba = new byte[4];
                byte[] isize = new byte[8];
                byte[] psfname = new byte[129];
                byte[] str = new byte[9];
                int j = 0;
                int k = 0;
                int z = 0;

                textBox1.Text = "";
                Array.Copy(source, 8, str, 0, 9);

                string iso = Encoding.GetEncoding(0).GetString(str);
                Array.Copy(source, 0x50, isize, 0, 5);
                long lbatotal = BitConverter.ToInt64(isize, 0);
                lbatotal *= 2048;
                sizecolor(size, lbatotal);
                label1.Text = "推定サイズ　;"+ (counter<<11).ToString();
                label2.Text = "セクター算出;" + lbatotal.ToString();
                if ((iso.Contains("PSP GAME") || iso.Contains("UMD VIDEO")))
                {
                    if (lbatotal - size != 0)
                    {
                        textBox1.Text = "ヘッダサイズ情報(=アンパック時のサイズ)とセクター算出が一致しないCSOです\n";
                    }
                    Array.Copy(source, 0x8C, lba, 0, 2);//0x809E root
                    z = BitConverter.ToInt32(lba, 0);

                    if (z > counter) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                    fs.Seek(offset[z], System.IO.SeekOrigin.Begin);
                    fs.Read(source, 0, offset[z + 1] - offset[z]);


                    if ((offset[z + 1] - offset[z]) != (2048 >> align))
                    {

                        MemoryStream ms = new MemoryStream();
                        ms.Write(source, 0, 2048);
                        ms.Position = 0;
                        DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                        zipStream.Read(source, 0, 2048);
                        zipStream.Close();
                        ms.Close();
                    }

                    j = 6;
                    while (true)
                    {
                        if (iso.Contains("PSP GAME") == true && source[j] == 0x50 && source[j + 1] == 0x53 && source[j + 2] == 0x50 && source[j + 3] == 0x5f && source[j + 4] == 0x47) break;
                        if (iso.Contains("UMD VIDEO") == true && source[j] == 0x55 && source[j + 1] == 0x4D && source[j + 2] == 0x44 && source[j + 3] == 0x5f && source[j + 4] == 0x56) break;
                        if (j > 2038) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                        j++;
                    }
                    Array.ConstrainedCopy(source, (j - 6), lba, 0, 2); //-31 0x809E rootdir
                    z = BitConverter.ToInt32(lba, 0);
                    if (z > counter) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                    fs.Seek(offset[z], System.IO.SeekOrigin.Begin);
                    fs.Read(source, 0, offset[z + 1] - offset[z]);


                    if ((offset[z + 1] - offset[z]) != (2048 >> align))
                    {
                        MemoryStream ms = new MemoryStream();
                        ms.Write(source, 0, 2048);
                        ms.Position = 0;
                        DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                        zipStream.Read(source, 0, 2048);
                        zipStream.Close();
                        ms.Close();
                    }


                    j = 31;
                    //PARAM.SFO
                    while (true)
                    {
                        if (source[j] == 0x50 && source[j + 1] == 0x41 && source[j + 2] == 0x52 && source[j + 3] == 0x41) break;
                        if (j > 2038) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                        j++;
                    }
                    Array.ConstrainedCopy(source, j - 31, lba, 0, 3);
                    z = BitConverter.ToInt32(lba, 0);
                    if (z > counter) { fs.Close(); textBox1.Text = "PSF取得に失敗しました"; return; }
                    fs.Seek(offset[z], System.IO.SeekOrigin.Begin);
                    fs.Read(source, 0, offset[z + 1] - offset[z]);


                    if ((offset[z + 1] - offset[z]) != (2048 >> align))
                    {
                        MemoryStream ms = new MemoryStream();
                        ms.Write(source, 0, 2048);
                        ms.Position = 0;
                        DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                        zipStream.Read(source, 0, 2048);
                        zipStream.Close();
                        ms.Close();
                    }

                    Array.ConstrainedCopy(source, 12, lba, 0, 4);
                    j = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(source, 8, lba, 0, 4);
                    k = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(source, 16, lba, 0, 4);
                    z = BitConverter.ToInt32(lba, 0);
                    byte[] tmp = new byte[200];
                    Array.ConstrainedCopy(source, k, tmp, 0, 200);
                    string strs = Encoding.GetEncoding(65001).GetString(tmp);
                    strs = strs.Substring(0, strs.IndexOf("\x0\x0"));
                    string[] name = strs.Split('\x0');
                    for (k = 0; ; k++)
                    {
                        if (name[k] == "TITLE") break;
                        if (k == z)
                        {
                            fs.Close(); textBox1.Text = "PSF取得に失敗しました";
                            return;
                        }
                    }
                    z = (k + 2) * 16;
                    Array.ConstrainedCopy(source, z, lba, 0, 2);
                    z = BitConverter.ToInt32(lba, 0);
                    Array.ConstrainedCopy(source, z + j, psfname, 0, 129);
                    fs.Close();
                    string rpname = Encoding.GetEncoding(65001).GetString(psfname);
                    j = rpname.IndexOf("\x0");
                    rpname = rpname.Substring(0, j) + ".CSO";
                    string[] dosmoji;
                    dosmoji = new string[11] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|", "\n", "\r" };
                    for (j = 0; j < 11; j++)
                    {
                        if (rpname.Contains(dosmoji[j]))
                        {
                            rpname = rpname.Replace(dosmoji[j], " ");
                        }
                    }

                    if (checkBox2.Checked == true)
                    {
                        rpname = rpname.Replace("　", " ");
                        IntPtr mecab = mecab_new2("-Oyomi");
                        rpname = mecab_sparse_tostr(mecab, rpname);
                        mecab_destroy(mecab);

                        string[] kana = { "・", "ー", "ァ", "ィ", "ゥ", "ェ", "ォ", "ヶ", "ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ヤ", "ユ", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "ヰ", "ヱ", "ヲ", "ン", "ガ", "ギ", "グ", "ゲ", "ゴ", "ザ", "ジ", "ズ", "ゼ", "ゾ", "ダ", "ヂ", "ヅ", "デ", "ド", "バ", "ビ", "ブ", "ベ", "ボ", "パ", "ピ", "プ", "ペ", "ポ", "キャ", "キュ", "キョ", "シャ", "シュ", "ショ", "チャ", "チュ", "チョ", "ニャ", "ニュ", "ニョ", "ヒャ", "ヒュ", "ヒョ", "ミャ", "ミュ", "ミョ", "リャ", "リュ", "リョ", "ギャ", "ギュ", "ギョ", "ジャ", "ジュ", "ジョ", "ビャ", "ビュ", "ビョ", "ピャ", "ピュ", "ピョ", "ファ", "フィ", "フェ", "フォ", "ヴァ", "ヴィ", "ヴ", "ヴェ", "ヴォ" };
                        string[] roma = { " ", "-", "a", "i", "u", "e", "o", "ke", "A", "I", "U", "E", "O", "KA", "KI", "KU", "KE", "KO", "SA", "SHI", "SU", "SE", "SO", "TA", "CHI", "TSU", "TE", "TO", "NA", "NI", "NU", "NE", "NO", "HA", "HI", "FU", "HE", "HO", "MA", "MI", "MU", "ME", "MO", "YA", "YU", "YO", "RA", "RI", "RU", "RE", "RO", "WA", "I", "E", "O", "N", "GA", "GI", "GU", "GE", "GO", "ZA", "JI", "ZU", "ZE", "ZO", "DA", "JI", "ZU", "DE", "DO", "BA", "BI", "BU", "BE", "BO", "PA", "PI", "PU", "PE", "PO", "KYA", "KYU", "KYO", "SHA", "SHU", "SHO", "CHA", "CHU", "CHO", "NYA", "NYU", "NYO", "HYA", "HYU", "HYO", "MYA", "MYU", "MYO", "RYA", "RYU", "RYO", "GYA", "GYU", "GYO", "JA", "JU", "JO", "BYA", "BYU", "BYO", "PYA", "PYU", "PYO", "FA", "FI", "FE", "FO", "VA", "VI", "VU", "VE", "VO" };


                        string[] suuji = { "零", "壱", "弐", "参", "廿", "卅", "卌" };
                        string[] romasuuji = { "ZERO", "ITI", "NI", "SAN", "NIJUU", "SANJUU", "SIJUU" };

                        for (j = 6; j >= 0; j--)
                        {
                            if (rpname.Contains(suuji[j]) == true)
                            {
                                rpname = rpname.Replace(suuji[j], romasuuji[j]);
                            }
                        }

                        rpname = abc123ToHankaku(rpname);

                        string cp = "";
                        string rp2 = rpname;
                        for (j = 0; j < rp2.Length - 4; j++)
                        {
                            cp = rp2.Substring(j, 1);
                            char x = Convert.ToChar(cp);
                            if (hankaku_zenkana(x) == false)
                            {
                                rpname = rpname.Replace(cp, " ");
                            }
                        }

                        for (j = 122; j >= 0; j--)
                        {
                            if (rpname.Contains(kana[j]) == true)
                            {
                                rpname = rpname.Replace(kana[j], roma[j]);
                            }
                        }
                        int tu = 0;
                        while (true)
                        {
                            tu = rpname.LastIndexOf("ッ");
                            if (tu == -1) break;
                            cp = rpname.Substring(tu + 1, 1);
                            rpname = rpname.Substring(0, tu) + cp + rpname.Substring(tu + 1, rpname.Length - tu - 1);
                        }
                        rpname = Strings.StrConv(rpname, VbStrConv.Narrow, 0x0411);

                        for (j = 0; j < 11; j++)
                        {
                            if (rpname.Contains(dosmoji[j]))
                            {
                                rpname = rpname.Replace(dosmoji[j], " ");
                            }
                        }
                    }

                    fs.Close();
                    textBox1.Text += rpname + "にリネームしました";
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
                    textBox1.Text = "正常なCISOではありません";
                }
            }
            else
            {
                textBox1.Text = "ファイルが存在しません";
            }
        }

        
        void sizecolor(long size, long trim)
        {
            if (size < trim)
            {
                label1.ForeColor = Color.Blue;
            }
            else if (size > trim)
            {
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.ForeColor = Color.Black;
            }
        }

        //UNPACK_CISO
        private void button9_Click(object sender, EventArgs e)
        {
             if (File.Exists(isofile) == true)
             {
                 System.IO.FileStream fs = new System.IO.FileStream(isofile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                 string rp = Path.GetDirectoryName(isofile) + "\\"+ Path.GetFileNameWithoutExtension(isofile) + "_unpack.iso";
                 System.IO.FileStream wss = new System.IO.FileStream(rp, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                 //ファイルを読み込むバイト型配列を作成する
                 byte[] integer = new byte[8];
                 byte[] source = new byte[2048];
                 fs.Read(source, 0, 2048);
                 fs.Seek(8, System.IO.SeekOrigin.Begin);
                 fs.Read(integer, 0, 4);
                 long size = BitConverter.ToInt64(integer, 0);
                 fs.Seek(0x10, System.IO.SeekOrigin.Begin);
                 fs.Read(integer, 0, 4);
                 long sector = BitConverter.ToInt64(integer, 0);
                 fs.Seek(0x14, System.IO.SeekOrigin.Begin);
                 fs.Read(integer, 0, 4);
                 int align = BitConverter.ToInt32(integer, 0) >> 8;
                 long counter = size / sector;
                 int z = 0;
                 int[] offset = new int[counter + 1];
                 for (int i = 0; i < counter + 1; i++)
                 {
                     fs.Seek(0x18 + 4 * i, System.IO.SeekOrigin.Begin);
                     fs.Read(integer, 0, 4);
                     offset[i] = (BitConverter.ToInt32(integer, 0));
                 }

                 fs.Seek((offset[0] & 0x7fffffff) << align, System.IO.SeekOrigin.Begin);

                 for (z = 0; z < counter-1; z++)
                 {
                     //fs.Seek((offset[z] & 0x7fffffff) << align, System.IO.SeekOrigin.Begin);
                     fs.Read(source, 0, ((offset[z + 1] & 0x7fffffff) - (offset[z] & 0x7fffffff)) << align);

                     if ((offset[z] & 0x80000000) != 0) {
                     }
                     else
                     {
                         if (offset[z + 1] == offset[z])
                         {
                             Array.Clear(source, 0, 2048);
                         }
                         else
                         {
                             MemoryStream ms = new MemoryStream();
                             ms.Write(source, 0, 2048);
                             ms.Position = 0;
                             DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                             zipStream.Read(source, 0, 2048);
                             zipStream.Close();
                             ms.Close();
                         }
                     }
                     wss.Write(source, 0, 2048);
                 }

                     //fs.Seek((offset[z] & 0x7fffffff) << align, System.IO.SeekOrigin.Begin);
                 if (align == 0)
                 {
                     fs.Read(source, 0, ((offset[z + 1] & 0x7fffffff) - (offset[z] & 0x7fffffff)) << align);
                 }
                 else
                 {
                     fs.Read(source, 0, ((offset[z + 1] & 0x7fffffff) - (offset[z] & 0x7fffffff) + 1) << align);
                 }

                     if ((offset[z] & 0x80000000) != 0) {
                     }
                     else
                     {
                         if (offset[z + 1] == offset[z])
                         {
                             Array.Clear(source, 0, 2048);
                         }
                         else
                         {
                             MemoryStream ms = new MemoryStream();
                             ms.Write(source, 0, 2048);
                             ms.Position = 0;
                             DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
                             zipStream.Read(source, 0, 2048);
                             zipStream.Close();
                             ms.Close();
                         }
                     }
                     wss.Write(source, 0, 2048);


                 fs.Close();
                 wss.Close();
                 SystemSounds.Asterisk.Play();
                 textBox1.Text = "アンパックが完了しました";
             }
             else
             {
                 textBox1.Text = "ファイルが存在しません";
             }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
             checkBox1.Checked = checkBox2.Checked;
        }



    }
}
