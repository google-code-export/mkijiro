using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        uint cpm = 0;

        public Form1()
        {
            InitializeComponent();
            if (File.Exists(Application.StartupPath + "\\config"))
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\config", FileMode.Open, FileAccess.Read);
                byte[] bs = new byte[20];
                fs.Read(bs, 0, 20);
                fs.Close();
                if (bs[0] != 0)
                {
                    checkBox1.Checked = true;
                }
                if (bs[1] != 0)
                {
                    checkBox2.Checked = true;
                }
                uint cp = 0;

                cpsel("");
                cp = BitConverter.ToUInt16(bs, 2);
                cpm = cp;
                switch (cp)
                {
                    case 932:
                        sJIS932.Checked = true;
                        break;
                    case 51932:
                        eUC51932.Checked = true;
                        break;
                    case 936:
                        gBK936.Checked = true;
                        break;
                    case 1201:
                        uTF16BE1201.Checked = true;
                        break;
                }

            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            FileStream fs = new FileStream(Application.StartupPath + "\\config", FileMode.Create, FileAccess.Write);
            byte[] bs = new byte[20];
            if (checkBox1.Checked == true) {
                bs[0] = 1;
            }
            if (checkBox2.Checked == true)
            {
                bs[1] = 1;
            }
            byte[] cps = BitConverter.GetBytes(cpm);
            Array.Copy(cps, 0, bs, 2, 4);

            fs.Write(bs, 0, 20);
            fs.Close();

        }




              private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DAT(*.dat)|*.dat|ALL FILES(*.*)|*.*";
            ofd.Title = "SELECT FILE";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                parse(ofd.FileName);
            }
        }

              private void SAVE_Click(object sender, EventArgs e)
              {
                if  (treeView1.SelectedNode != null) {
                    TreeNode edittree = treeView1.SelectedNode;


                    if (edittree.Level == 0)
                    {
                        edittree.Text = gtitle.Text;
                    }
                    if (edittree.Level == 1)
                    {

                        edittree.Text = codename.Text;
                        Regex r = new Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}");
                        Match mr = r.Match(codehex.Text.Replace("0x", ""));
                        StringBuilder sb = new StringBuilder();
                        while (mr.Success) {
                            sb.AppendLine(mr.Value);
                            if (edittree.Index == 0)
                            {
                                gameid.Text = cf2sceid(mr.Value.Replace(" ",""));
                                edittree.Parent.Tag = gameid.Text;
                                break;
                            }
                            mr = mr.NextMatch();
                        }
                        if (sb.Length > 0)
                        {
                            edittree.Tag = sb.ToString();
                        }
                    }
                
                }


              }



        private void parse(string fs) {
                  //CP1201　UFT16ビッグエンディアンでコードフリークDATを読む
                  StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(1201));
                  string s = sr.ReadToEnd();
                  sr.Close();
                  s = rpstringin(s);
                  //U+0A0Aで分割
                  string[] ss = s.Split('ਊ');
                  string head = "";
                  string sb = "";
                  bool cmt = false;
                  TreeNode gnode = new TreeNode();
                  TreeNode cnode = new TreeNode();
                  treeView1.Nodes.Clear();
                  for (int i = 0; i < ss.Length; i++)
                  {
                      if (ss[i].Length > 1)
                      {
                          head = ss[i].Substring(0, 1);
                          s = ss[i].Remove(0, 1);
                          //U+4720でコードタイトル
                          if (head == "䜠")
                          {
                              gnode = new TreeNode();
                              gnode.Text = s;
                              treeView1.Nodes.Add(gnode);
                          }
                          //U+4D20でゲームID
                          else if (head == "䴠")
                          {
                              cnode = new TreeNode();
                              cnode.Text = "(M)";
                              gnode.Tag = cf2sceid(s);
                              s = s.Insert(8, " ");
                              cnode.Tag = s + "\r\n";
                              gnode.Nodes.Add(cnode);
                          }
                          //U+4420でコード名
                          else if (head == "䐠")
                          {
                              //コード名が’’(アポストロフィx2)の場合コメント
                              if (ss[i].Length > 2 && s.Substring(0, 2) == "''")
                              {
                                  cmt = true;
                                  sb = s;
                              }
                              else
                              {
                                  cmt = false;
                                  cnode = new TreeNode();
                                  cnode.Text = s;
                                  cnode.Tag = "";
                                  gnode.Nodes.Add(cnode);
                              }
                          }
                          //U+4320でコード内容
                          else if (head == "䌠")
                          {
                              if (cmt == false)
                              {
                                  s = s.Insert(8, " ") + "\r\n";
                                  cnode.Tag += s;
                              }
                              //コメント
                              else
                              {
                                  s = sb + "\r\n";
                                  cnode.Tag += s;
                              }
                          }
                      }
                  }
        }


        private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void treeView1_DragDrop(object sender,  System.Windows.Forms.DragEventArgs e)
        {
            string[] fileName =
                (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (File.Exists(fileName[0]) == true) {
                parse(fileName[0]);
            }
        }


        private string  cf2sceid(string s){
            byte[] bb = new byte[5];
            for (int i = 0; i < 4; i++) {
                bb[i]= hexed(s.Substring(i*2, 2));
            }
            bb[4] = (byte)'-';
            s  = Encoding.GetEncoding(1252).GetString(bb) +s.Substring(8, 5);
            return s;
        }


        private byte hexed(string s)
        {
                return    Convert.ToByte(s, 16);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            codename.Text = "";
            codehex.Text = "";
            cmtb.Text = "";
            gtitle.Enabled = false;
            codename.Enabled = false;
            codehex.Enabled = false;
            cmtb.Enabled = false;

            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 0)
                {
                    gtitle.Text = treeView1.SelectedNode.Text;
                    gameid.Text = treeView1.SelectedNode.Tag.ToString();
                    gtitle.Enabled = true;

                }
                else
                {
                    codename.Enabled = true;
                    codehex.Enabled = true;
                    cmtb.Enabled = true;
                    codename.Text = treeView1.SelectedNode.Text;
                    Regex rg = new Regex("''");
                    string[] ss = rg.Split(treeView1.SelectedNode.Tag.ToString());
                    codehex.Text = ss[0];
                    for (int i = 1; i < ss.Length; i++)
                    {
                        cmtb.Text += ss[i];
                    }
                }
            }
        }

        string[] pattern = { "<(C)>", "<(R)>", "<(TM)>", "<肉>", "<どくろ>", "<顔白>", "<かえる>" };
        string[] rp = { "\xA9", "\xAE", "\x2122", "\x1C", "\x1D", "\x1E", "\x1F" };

        private string rpstringout(string s){
            Regex r = new Regex("<.*?>");
            Match m = r.Match(s);
            while (m.Success == true) {
                for (int i = 0; i < 3; i++) {//bindato7
                    if (m.Value == pattern[i]) {
                        s = s.Replace(m.Value, rp[i]);
                        break;
                    }
                }
                m = m.NextMatch();
            }
            return s;
        }
        
        private string rpstringin(string s)
        {
            Regex r = new Regex("[\x1C-\x1F]");
            Match m = r.Match(s);
            while (m.Success == true)
            {
                for (int i = 3; i < 7; i++)
                {
                    if (m.Value == rp[i])
                    {
                        s = s.Replace(m.Value, pattern[i]);
                        break;
                    }
                }
                m = m.NextMatch();
            }
            Regex u = new Regex("<.*?>");
            Match n = u.Match(s);
            while (n.Success == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (n.Value == pattern[i])
                    {
                        s = s.Replace(n.Value, rp[i]);
                        break;
                    }
                }
                n = n.NextMatch();
            }
            return s;
        }

        private void OUT_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            Regex r = new Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}");
            Match mr;
            Boolean enc = checkBox2.Checked;
            uint cf = 0;
            Boolean encok= false;
            int pos = 0;
            if (checkBox1.Checked == false)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    encok = false;
                    sb.Append("\"");
                    sb.Append(rpstringout(n.Text));
                    sb.AppendLine("\"");
                    sb.Append("''");
                    sb.AppendLine(n.Tag.ToString());
                    foreach (TreeNode m in n.Nodes)
                    {
                        sb.AppendLine(rpstringout(m.Text));
                        mr = r.Match(rpstringout(m.Tag.ToString()));


                        while (mr.Success == true)
                        {
                            sb.Append("$");
                            if (encok == true)
                            {
                                sb.AppendLine(codefreakdec(mr.Value));
                            }
                            else
                            {
                                sb.AppendLine(mr.Value);
                            }

                            if (enc = true && m.Text == "(M)")
                            {
                                cf = Convert.ToUInt32(mr.Value.Substring(9, 8), 16);
                                if ((cf & 0x800) == 0)
                                {
                                    encok = true;
                                }
                            }


                            mr = mr.NextMatch();
                        }


                        pos = m.Tag.ToString().IndexOf("''");
                        if (pos > 0)
                        {
                            sb.AppendLine(rpstringout((m.Tag.ToString().Remove(0, pos))));
                        }
                    }
                }
            }
            else
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    encok = false;
                    sb.Append("_S ");
                    sb.AppendLine(rpstringout(n.Tag.ToString()));
                    sb.Append("_G ");
                    sb.AppendLine(n.Text);
                    foreach (TreeNode m in n.Nodes)
                    {
                        if(m.Text !="(M)"){
                        sb.Append("_C0 ");
                        sb.AppendLine(rpstringout(m.Text));
                        mr = r.Match(rpstringout(m.Tag.ToString()));
                        while (mr.Success == true)
                        {
                            sb.Append("_L 0x");
                            if (encok == true)
                            {
                                sb.AppendLine(codefreakdec(mr.Value).Insert(9, "0x"));
                            }
                            else
                            {
                                sb.AppendLine(mr.Value.Insert(9, "0x"));
                            }
                            mr = mr.NextMatch();
                        }
                        pos = m.Tag.ToString().IndexOf("''");
                        if(pos>0){
                        sb.AppendLine( rpstringout((m.Tag.ToString().Remove(0, pos).Replace("''", "#"))));
                            }
                        }
                        else if (enc = true && m.Text == "(M)")
                        {
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            if (mr.Success == true)
                            {
                                cf = Convert.ToUInt32(mr.Value.Substring(9, 8), 16);
                                if ((cf & 0x800) == 0)
                                {
                                    encok = true;
                                }
                            }
                        }
                    }
                }
            }

            StreamWriter sw = new StreamWriter("converted_cf.txt", false, Encoding.GetEncoding(getcp()));
            sw.Write(sb.ToString());
            sw.Close();
        }


        private void OUTDAT_Click(object sender, EventArgs e)
        {            
            StringBuilder sb = new StringBuilder();
            Regex r = new Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}");
            Match mr;
            Boolean enc = checkBox2.Checked;
            uint cf = 0;
            Boolean encok= false;
                foreach (TreeNode n in treeView1.Nodes)
                {
                    encok = false;
                    sb.Append("䜠");
                    sb.Append(n.Text);
                    sb.Append("ਊ");
                    foreach (TreeNode m in n.Nodes)
                    {
                        
                       if (m.Text == "(M)")
                        {
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            if (mr.Success == true)
                            {

                                sb.Append("䴠");
                                cf = Convert.ToUInt32(mr.Value.Substring(9, 8), 16);
                                if ((cf & 0x800) == 0)
                                {
                                    encok = true;
                                    sb.Append(mr.Value.Substring(0,8));
                                    cf = (cf & 0xFFFFF0FF) | 0x800;
                                    sb.Append(cf.ToString("X8"));
                                }
                                else
                                {
                                    sb.Append(mr.Value).Replace(" ", "");
                                }
                                sb.Append("ਊ");
                            }
                        }
                        else if (m.Text != "(M)")
                        {
                            sb.Append("䐠");
                            sb.Append(rpstringout(m.Text));
                            sb.Append("ਊ");
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            while (mr.Success == true)
                            {
                                sb.Append("䌠");
                                if (encok == true)
                                {
                                    sb.Append(codefreakdec(mr.Value).Replace(" ",""));
                                }
                                else
                                {
                                    sb.Append(mr.Value).Replace(" ", "");
                                }
                                sb.Append("ਊ");
                                mr = mr.NextMatch();
                            }
                        }
                    }
                }

            const bool bigEndian = true;
            const bool bom = true;
            Encoding nobomcp1201= new UnicodeEncoding(bigEndian, !bom);
            StreamWriter sw = new StreamWriter("converted_cf.dat", false, nobomcp1201);
            sw.Write(sb.ToString());
            sw.Close();

           
        }

        private int getcp() { 
            uint cp=0;
            if (sJIS932.Checked == true) {
                cp = 932;
            }
            if (gBK936.Checked == true)
            {
                cp = 936;
            }
            if (eUC51932.Checked == true)
            {
                cp = 51932;
            }
            if (uTF16BE1201.Checked == true)
            {
                cp =1201;
            }
            cpm = cp;

            return Convert.ToInt32(cp);
        }

        private string codefreakdec(string basest){
            StringBuilder sb = new StringBuilder();
                uint codefreak = 0;
        string[] s = basest.Split('\n');
        foreach (string ss in s)
        {
        if (ss.Length > 8)
        {
        codefreak = Convert.ToUInt32(ss.Substring(0, 8), 16);
        codefreak ^= 0xD6F73BEE;
        sb.Append(codefreak.ToString("X8"));
        sb.Append(ss.Remove(0,8).TrimEnd());
        }
        }
            return sb.ToString();
        }

        private void vERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CF2TXT.Form2 f = new  CF2TXT.Form2();
            f.ShowDialog();
            f.Dispose();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cpsel(string s){
     
        
            sJIS932.Checked = false;
            gBK936.Checked = false;
            eUC51932.Checked = false;
            uTF16BE1201.Checked = false;
            if (s.Contains("SJIS")) {

                sJIS932.Checked = true;
            }
            if (s.Contains("UTF"))
            {

                uTF16BE1201.Checked = true;
            }
            if (s.Contains("GBK"))
                gBK936.Checked = true;
            {

            }
            if (s.Contains("EUC"))
            {

                eUC51932.Checked = true;
            }
            getcp();
        }


        private void eNCODEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = sender.ToString();
            cpsel(s);

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox2.Checked;
        }


        
    }
 }
