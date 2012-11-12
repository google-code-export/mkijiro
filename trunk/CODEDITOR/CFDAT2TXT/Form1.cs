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
                CF.CODEFEAKReader sr = new CF.CODEFEAKReader(fs, Encoding.GetEncoding(1201));
                try
                {
                string s;
                string head = "";
                string sb = "";
                bool cmt = false;
                TreeNode gnode = new TreeNode();
                TreeNode cnode = new TreeNode();
                treeView1.Nodes.Clear();
                while (sr.Peek() > -1)
                {
                    s = sr.ReadCF();
                    if (s.Length > 1)
                    {
                        head = s.Substring(0, 1);
                        s = s.Remove(0, 1);
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
                            if (s.Length > 2 && s.Substring(0, 2) == "''")
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
           catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            finally
            {
                sr.Close();
            }
        }


        private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        //ノードがドラッグされた時
        private void TreeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            tv.SelectedNode = (TreeNode)e.Item;
            tv.Focus();
            //ノードのドラッグを開始する
            DragDropEffects dde =  tv.DoDragDrop(e.Item, DragDropEffects.All);
        }

        //ドラッグしている時
        private void TreeView1_DragOver(object sender, DragEventArgs e)
        {
            //ドラッグされているデータがTreeNodeか調べる
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) { }
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                if ((e.KeyState & 8) == 8 &&
                    (e.AllowedEffect & DragDropEffects.Copy) ==
                    DragDropEffects.Copy)
                    //Ctrlキーが押されていればCopy
                    //"8"はCtrlキーを表す
                    e.Effect = DragDropEffects.Copy;
                else if ((e.AllowedEffect & DragDropEffects.Move) ==
                    DragDropEffects.Move)
                    //何も押されていなければMove
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                //TreeNodeでなければ受け入れない
                e.Effect = DragDropEffects.None;

            //マウス下のNodeを選択する
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                if (e.Effect != DragDropEffects.None)
                {
                    TreeView tv = (TreeView)sender;
                    //マウスのあるNodeを取得する
                    TreeNode target =
                        tv.GetNodeAt(tv.PointToClient(new Point(e.X, e.Y)));
                    //ドラッグされているNodeを取得する
                    TreeNode source =
                        (TreeNode)e.Data.GetData(typeof(TreeNode));
                    //マウス下のNodeがドロップ先として適切か調べる
                    if (target != null && target != source &&
                            IsChildNode(source, target))
                    {
                        //Nodeを選択する
                        if (target.IsSelected == false)
                            tv.SelectedNode = target;
                    }
                    else
                        e.Effect = DragDropEffects.None;
                }
            }
        }

        //ドロップされたとき
        private void TreeView1_DragDrop(object sender, DragEventArgs e)
        {            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (File.Exists(fileName[0]) == true)
                {
                    parse(fileName[0]);
                }
            }
            //ドロップされたデータがTreeNodeか調べる
            else if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                TreeView tv = (TreeView)sender;
                //ドロップされたデータ(TreeNode)を取得
                TreeNode source =
                    (TreeNode)e.Data.GetData(typeof(TreeNode));
                //ドロップ先のTreeNodeを取得する
                TreeNode target =
                    tv.GetNodeAt(tv.PointToClient(new Point(e.X, e.Y)));
                //マウス下のNodeがドロップ先として適切か調べる
                if (target != null && target != source &&
                    IsChildNode(source, target))
                {
                    //ドロップされたNodeのコピーを作成
                    TreeNode cln = (TreeNode)source.Clone();
                    //Nodeを追加
                    if (target.Level == 0)
                    {
                       treeView1.Nodes.RemoveAt(source.Index);
                     treeView1.Nodes.Insert(target.Index, cln);
                    }
                    else
                    {
                    source.Remove();
                    target.Parent.Nodes.Insert(target.Index, cln);
                    }
                    //追加されたNodeを選択
                    tv.SelectedNode = cln;
                    e.Effect = DragDropEffects.None;

                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private static bool IsChildNode(TreeNode childNode, TreeNode childNode2)
        {
            if (childNode.Parent == childNode2.Parent && childNode.Index>0 && childNode2.Index>0)
                return true;
            else if (childNode.Level ==0 && childNode2.Level ==0)
                return true;
            else
                return false;
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


        private string sceid2cf(string s)
        {

            s = s.PadRight(10, (char)'0');
            byte[] bb = Encoding.ASCII.GetBytes(s);    
            s = " " + s.Substring(5,5);
            string ss = "";
            for (int i = 0; i < 4; i++)
            {
                ss +=(bb[i] >> 4).ToString("X") + (bb[i] & 0xF).ToString("X");
            }
            s =ss + s+"820";
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
            コード削除.Enabled = false;

            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 0)
                {
                    gtitle.Text = treeView1.SelectedNode.Text;
                    gameid.Text = treeView1.SelectedNode.Tag.ToString();
                    gtitle.Enabled = true;
                    コード削除.Enabled=true;

                }
                else
                {

                    if (treeView1.SelectedNode.Index != 0)
                    {
                        codename.Enabled = true;
                        cmtb.Enabled = true;
                        コード削除.Enabled = true;          
                    }
                    codehex.Enabled = true;
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

            Regex r = new Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}");
            Match mr;
            Boolean enc = checkBox2.Checked;
            uint cf = 0;
            Boolean encok= false;
            int pos = 0;
            StreamWriter sw = new StreamWriter("converted_cf.txt", false, Encoding.GetEncoding(getcp()));
            if (checkBox1.Checked == false)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    encok = false;
                    sw.Write("\"");
                    sw.Write(rpstringout(n.Text));
                    sw.WriteLine("\"");
                    sw.Write("''");
                    sw.WriteLine(n.Tag.ToString());
                    foreach (TreeNode m in n.Nodes)
                    {
                        sw.WriteLine(rpstringout(m.Text));
                        mr = r.Match(rpstringout(m.Tag.ToString()));


                        while (mr.Success == true)
                        {
                            sw.Write("$");
                            if (encok == true)
                            {
                                sw.WriteLine(codefreakdec(mr.Value));
                            }
                            else
                            {
                                sw.WriteLine(mr.Value);
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
                            sw.WriteLine(rpstringout((m.Tag.ToString().Remove(0, pos))));
                        }
                    }
                }
            }
            else
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    encok = false;
                    sw.Write("_S ");
                    sw.WriteLine(rpstringout(n.Tag.ToString()));
                    sw.Write("_G ");
                    sw.WriteLine(n.Text);
                    foreach (TreeNode m in n.Nodes)
                    {
                        if(m.Text !="(M)"){
                        sw.Write("_C0 ");
                        sw.WriteLine(rpstringout(m.Text));
                        mr = r.Match(rpstringout(m.Tag.ToString()));
                        while (mr.Success == true)
                        {
                            sw.Write("_L 0x");
                            if (encok == true)
                            {
                                sw.WriteLine(codefreakdec(mr.Value).Insert(9, "0x"));
                            }
                            else
                            {
                                sw.WriteLine(mr.Value.Insert(9, "0x"));
                            }
                            mr = mr.NextMatch();
                        }
                        pos = m.Tag.ToString().IndexOf("''");
                        if(pos>0){
                        sw.WriteLine( rpstringout((m.Tag.ToString().Remove(0, pos).Replace("''", "#"))));
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

            sw.Close();
        }


        private void OUTDAT_Click(object sender, EventArgs e)
        {            

            Regex r = new Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}");
            Match mr;
            Boolean enc = checkBox2.Checked;
            uint cf = 0;
            Boolean encok= false;


            const bool bigEndian = true;
            const bool bom = true;
            Encoding nobomcp1201 = new UnicodeEncoding(bigEndian, !bom);
            StreamWriter sw = new StreamWriter("converted_cf.dat", false, nobomcp1201);
                foreach (TreeNode n in treeView1.Nodes)
                {
                    encok = false;
                    sw.Write("䜠");
                    sw.Write(n.Text);
                    sw.Write("ਊ");
                    foreach (TreeNode m in n.Nodes)
                    {
                        
                       if (m.Index ==0)
                        {
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            if (mr.Success == true)
                            {

                                sw.Write("䴠");
                                cf = Convert.ToUInt32(mr.Value.Substring(9, 8), 16);
                                if ((enc==true) &&((cf & 0x800) == 0))
                                {
                                    encok = true;
                                    sw.Write(mr.Value.Substring(0,8));
                                    cf = (cf & 0xFFFFF0FF) | 0x800;
                                    sw.Write(cf.ToString("X8"));
                                }
                                else
                                {
                                    sw.Write(mr.Value.Replace(" ", ""));
                                }
                                sw.Write("ਊ");
                            }
                        }
                        else
                        {
                            sw.Write("䐠");
                            sw.Write(rpstringout(m.Text));
                            sw.Write("ਊ");
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            while (mr.Success == true)
                            {
                                sw.Write("䌠");
                                if (encok == true)
                                {
                                    sw.Write(codefreakdec(mr.Value).Replace(" ",""));
                                }
                                else
                                {
                                    sw.Write(mr.Value.Replace(" ", ""));
                                }
                                sw.Write("ਊ");
                                mr = mr.NextMatch();
                            }
                        }
                    }
                }

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



        private void PARSER_Click(object sender, EventArgs e)
        {

            this.Text = "CF2TXT";
            CF2TXT.Form3 f = new CF2TXT.Form3();
            f.ShowDialog(this);
            f.Dispose();

            if (this.Text != "CF2TXT")
            {
                string[] ss = this.Text.Split((char)'\n');
                string l = "";
                string head = "";

                TreeNode gnode = new TreeNode();
                TreeNode cnode = new TreeNode();
                foreach (string s in ss)
                {
                    if (s.Length > 2)
                    {
                        head = s.Substring(0, 2);
                        l = s.Remove(0, 3).Trim();
                        switch (head)
                        {
                            case "_S":
                                gnode = new TreeNode();
                                gnode.Tag = l;
                                treeView1.Nodes.Add(gnode);
                                break;
                            case "_G":
                                gnode.Text = l;
                                cnode = new TreeNode();
                                cnode.Text = "(M)";
                                l = sceid2cf(gnode.Tag.ToString());
                                cnode.Tag = l;
                                gnode.Nodes.Add(cnode);
                                break;
                            case "_C":
                                cnode = new TreeNode();
                                cnode.Text = l;
                                cnode.Tag = "";
                                gnode.Nodes.Add(cnode);

                                break;
                            case "_L":
                                l = l.Replace("0x", "") + "\r\n";
                                cnode.Tag += l;
                                break;
                            default:
                                if (head.Contains("#"))
                                {

                                    l = l.Replace("#", "''") + "\r\n";
                                    cnode.Tag += l;
                                }
                                break;
                        }
                    }

                }
            }
            this.Text = "CF2TXT";

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

        private void ゲーム追加_Click(object sender, EventArgs e)
        {


                try
                {
                    TreeNode gnode = new TreeNode();
                    TreeNode cnode = new TreeNode();

                    gnode.Text = "新規ゲーム";
                    gnode.Tag = "ULJM-00000";
                    cnode.Text="(M)";
                    cnode.Tag = "00000000 00000000";
                    gnode.Nodes.Add(cnode);
                    treeView1.Nodes.Insert(0, gnode);
                    treeView1.SelectedNode = gnode;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                    }

        private void コード追加_Click(object sender, EventArgs e)
        {
            TreeNode tv = treeView1.SelectedNode;
            if (tv != null)
            {

                try
                {
                    TreeNode cnode = new TreeNode();

                    cnode.Text = "新規コード";
                    cnode.Tag = "00000000 00000000";
                    if (tv.Level == 0) {
                        tv.Nodes.Add(cnode);
                    }
                    else
                    {
                        tv.Parent.Nodes.Insert(tv.Index, cnode);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }



            }
        }

        private void コード暗号化復号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tv = treeView1.SelectedNode;
            if (tv!=null){

                if (tv.Level==1)
                {
                    tv = tv.Parent;
                }

                try
                {
                    Regex r = new Regex("[0-9A-Fa-f]{8} [0-9A-Fa-f]{8}");
                    Match mr;
                    uint cf = 0;
                    StringBuilder sb = new StringBuilder();

                    foreach (TreeNode m in tv.Nodes)
                    {

                        if (m.Index == 0 )
                        {
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            if (mr.Success == true)
                            {

                                cf = Convert.ToUInt32(mr.Value.Substring(9, 8), 16);
                                sb.Append(mr.Value.Substring(0, 9));
                                if ((cf & 0x800) == 0)
                                {
                                    cf = (cf & 0xFFFFF0FF) | 0x800;
                                }
                                else
                                {
                                    cf = (cf & 0xFFFFF0FF);

                                }
                                sb.Append(cf.ToString("X8"));
                                m.Tag = sb.ToString();
                                sb.Clear();
                            }
                        }
                        else if (m.Text != "(M)")
                        {
                            mr = r.Match(rpstringout(m.Tag.ToString()));
                            while (mr.Success == true)
                            {
                                sb.AppendLine(codefreakdec(mr.Value));
                                mr = mr.NextMatch();
                            }
                            m.Tag = sb.ToString();
                            sb.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            
            
            }

        }

        private void コード削除_Click(object sender, EventArgs e)
        {
            
            TreeNode tv = treeView1.SelectedNode;
            if (tv!=null){
                
                try
                {
                        if (MessageBox.Show("選択しているコードを削除してもよろしいですか？", "削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            tv.Remove();
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }



            }
        }

        private void TextBox1_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e)
        {

            int pos = codehex.SelectionStart;
            int mlen = codehex.Text.Length;
            string[] ss = codehex.Text.Split((char)'\n');
            int len = 0;
            foreach (string s in ss) {
                if (len + s.Length+1 <= pos)
                {
                    len += s.Length + 1;
                }            
            }
            int posbk = pos;
            pos -= len;

            if (pos == 8 && e.KeyChar != '\b')
            {
             e.KeyChar = ' ';
            }
            else if (pos == 17 && posbk == mlen)
            {
                e.KeyChar = (char)Keys.Enter;
            }
            else if (pos == 17 && posbk < mlen)
            {
                codehex.SelectionStart += 2;
                e.Handled = true;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != 13 && (e.KeyChar < 'a' || e.KeyChar > 'f') && (e.KeyChar < 'A' || e.KeyChar > 'F'))
            {
                e.Handled = true;
            }
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }



        
    }
 }
