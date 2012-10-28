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
        public Form1()
        {
            InitializeComponent();
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
            cmtb.Text = "";
            if (treeView1.SelectedNode.Level == 0)
            {
                gameid.Text = treeView1.SelectedNode.Tag.ToString();
            }
            else
            {
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
            StreamWriter sw = new StreamWriter("converted_cf.txt", false, Encoding.GetEncoding(1201));
            sw.Write(sb.ToString());
            sw.Close();
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

        private void oPENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void cONVERTTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OUT_Click(sender,e);
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
        
    }
 }
