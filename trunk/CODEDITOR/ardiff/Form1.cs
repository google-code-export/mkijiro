using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("diff.htm"))
            {
                StringBuilder sb = new StringBuilder();
                StreamReader sr = null;
                try
                {

                    sr = new StreamReader("diff.htm", Encoding.GetEncoding(65001));
                    string line = "";
                    string head = "";
                    string game = "";
                    string code = "";
                    string fcode = "";
                    Boolean tr = false;
                    Boolean gg = false;
                    Boolean cc = false;
                    Boolean fl = false;
                    int ct = 0;
                    int mode = 0;
                    int codes = -1;


                    while (sr.Peek() > -1)
                    {
                        line = sr.ReadLine();



                        if (line.Contains("<tr"))
                        {
                            tr = true;
                            ct = 0;
                        }
                        else if (tr = true && line.Contains("<td"))
                        {
                            if (ct == 1)
                            {

                                if (line.Contains("_S "))
                                {
                                    head = line;
                                    cc = false; 
                                    fl = false;
                                }
                                if (line.Contains("_G "))
                                {
                                    game = line;
                                    gg = true;
                                }
                                if (line.Contains("_C0 "))
                                {
                                    code = line;
                                    cc = true;
                                    if (codes >= 0)
                                    {
                                        codes -= 1;
                                    }
                                    if (codes == -1)
                                    {
                                        fl = false;
                                    }

                                }
                                if (line.Contains("_M 0xCF"))
                                {
                                    fcode = code;
                                    Regex r = new Regex("0x[0-9A-F]{8}");
                                    Match m = r.Match(line);
                                    if (m.Success == true)
                                    {
                                        mode = Convert.ToInt32(m.Value, 16) & 0xF;
                                        m = m.NextMatch();
                                        codes = Convert.ToInt32(m.Value, 16) & 0xFF;
                                    }
                                    fl = true;
                                }

                                if (line.Contains("color"))
                                {

                                    if (line.Contains("_M 0xCF"))
                                    {
                                        fl = false;
                                    }

                                    if (line.Contains("_S "))
                                    {

                                    }
                                    else if (line.Contains("_M ") == false)
                                    {


                                        if (line.Contains("_G "))
                                        {
                                            sb.Append("・");
                                            sb.Append(cnv(line));
                                            sb.Append("(");
                                            sb.Append(cnv(head));
                                            sb.AppendLine(")");
                                            gg = false;
                                        }
                                        else if (line.Contains("_C0 "))
                                        {

                                            if (gg == true)
                                            {
                                                sb.Append("・");
                                                sb.Append(cnv(game));
                                                sb.Append("(");
                                                sb.Append(cnv(head));
                                                sb.AppendLine(")");
                                                gg = false;
                                            }
                                            if (fl == true)
                                            {
                                                sb.AppendLine(cnv(fcode));
                                                fl = false;
                                            }
                                            if (codes >= 0)
                                            {
                                                if (mode == 0)
                                                {
                                                    sb.Append("○");
                                                }
                                                else if (mode == 1)
                                                {
                                                    sb.Append("※");
                                                }
                                                else if (mode == 2)
                                                {
                                                    sb.Append("□");
                                                }

                                            }

                                            cc = false;
                                            sb.AppendLine(cnv(line));
                                        }
                                    }
                                    else
                                    {
                                        if (gg == true)
                                        {
                                            sb.Append("・");
                                            sb.Append(cnv(game));
                                            sb.Append("(");
                                            sb.Append(cnv(head));
                                            sb.AppendLine(")");
                                            gg = false;
                                        }
                                        if (fl == true)
                                        {
                                            sb.AppendLine(cnv(fcode));
                                            fl = false;
                                        }
                                        if (cc == true)
                                        {
                                            if (codes >= 0)
                                            {
                                                if (mode == 0)
                                                {
                                                    sb.Append("○");
                                                }
                                                else if (mode == 1)
                                                {
                                                    sb.Append("※");
                                                }
                                                else if (mode == 2)
                                                {
                                                    sb.Append("□");
                                                }

                                            }
                                            sb.AppendLine(cnv(code));
                                            cc = false;
                                        }
                                    }

                                }
                            }
                            ct++;
                        }
                        else if (line.Contains("</tr"))
                        {
                            tr = false;
                        }


                    }
                }
                catch (Exception ex){
                    MessageBox.Show(ex.Message);
                }
                finally{
                    sr.Close();
                    richTextBox1.Text = ConvertKana(sb.ToString());
                }

            }

        }

        public static string ConvertKana(string src)
        {
            return System.Text.RegularExpressions.Regex.Replace(src, "[\uFF61-\uFF9F+]+", MatchKanaEvaluator);
        }

        private static string MatchKanaEvaluator(System.Text.RegularExpressions.Match m)
        {
            return Microsoft.VisualBasic.Strings.StrConv(m.Value, Microsoft.VisualBasic.VbStrConv.Wide, 0x411);
        }

        string cnv(string line) {

            Regex r = new Regex("<[^>]*>");
            Match m = r.Match(line);
           string s = line;
            while (m.Success == true)
            {
                s = s.Replace(m.Value, "");
                m = m.NextMatch();
            }
        return s.Remove(0,3).Trim();
        }


        private void button2_Click(object sender, EventArgs e)
        {

            richTextBox1.Text = Microsoft.VisualBasic.Strings.StrConv(richTextBox1.Text, Microsoft.VisualBasic.VbStrConv.Hiragana, 0x411);
            richTextBox1.SelectionStart = 0;
            richTextBox1.Focus();
            richTextBox1.ScrollToCaret();
            richTextBox2.SelectionStart = 0;
            richTextBox2.Focus();
            richTextBox2.ScrollToCaret();
            richTextBox2.Text = richTextBox1.Text;
        }

           string[] html = {"&amp;", "&gt;", "&lt;", "&apos;", "&quot;", "&nbsp;"};
           string[] html2 = { "&", "<", ">", "'", ",", " " };


        private string rephtml(string s){

            for (int i = 0; i < 6; i++) {
                s = s.Replace(html[i], html2[i]);  
            }

                return s;
        }



        private void button3_Click(object sender, EventArgs e)
        {


            string sss = richTextBox1.Text;
            if (checkBox1.Checked == true)
            {
                sss = sss.Replace(" ", "");
            }

            if (checkBox2.Checked == true)
            {
                sss = sss.Replace("[U]", "[投稿]");
            }

           sss = rephtml(sss);

            string[] ss = sss.Split((char)0xA);
            StringBuilder sb = new StringBuilder();
            string s1 = "";

              foreach(string s in ss){
                  s1 = s.Trim();
                  s1 = HttpUtility.UrlEncode(s1);
                sb.AppendLine(Example.Ext.ToKanji(s1));
              }
              richTextBox1.SelectionStart = 0;
              richTextBox1.Focus();
              richTextBox1.ScrollToCaret();
              richTextBox2.SelectionStart = 0;
              richTextBox2.Focus();
              richTextBox2.ScrollToCaret();
            richTextBox2.Text=  sb.ToString();


        }

        private void richTextBox2_SelectionChanged(object sender, EventArgs e)
        {
            int line = 0;
            int linect = 0;
            int linect2 = 0;
            int bk=richTextBox2.SelectionStart;
            string s = richTextBox2.Text;
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionBackColor = Color.White;
            richTextBox1.SelectionLength = 0;

            while (true)
            {
                line = s.IndexOf((char)0xA,line+1);
                if (line < 0)
                {
                    break;
                }
                linect += 1;
                if (line >= richTextBox2.SelectionStart)
                {
                    linect -= 1;
                    break;
                }
            }
            line = 0;
            while (true)
            {
                line = richTextBox1.Text.IndexOf((char)0xA, line+1);
                if (line < 0) {
                    break;
                }
                linect2 += 1;
                if (0 == linect)
                {
                    richTextBox1.SelectionStart = 0;
                    richTextBox1.SelectionLength = line ;
                    break;
                }
                else if (linect2 == linect)
                {
                    richTextBox1.SelectionStart = line + 1;
                    line = richTextBox1.Text.IndexOf((char)0xA, line + 1);
                    if (line < 0)
                    {
                        break;
                    }
                    richTextBox1.SelectionLength = line - richTextBox1.SelectionStart;
                    break;
                }

            }

            richTextBox1.SelectionBackColor = Color.Aquamarine;
            richTextBox2.SelectionStart=bk;
            richTextBox2.Focus();
        }

        private void コピーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s ;
            if (sender.ToString().Contains("richTextBox1"))
            {
                s = richTextBox1.SelectedText;
            }
            else {
                s =  richTextBox2.SelectedText;
            }
            if(s!=""){
            Clipboard.SetText(s);
            }
        }

        private void 貼り付けToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (Clipboard.ContainsText())
            {
                string s = Clipboard.GetText();
                if (sender.ToString().Contains("richTextBox1"))
                {

                    richTextBox1.SelectedText=s;
                }
                else
                {

                     richTextBox2.SelectedText=s;
                }
            }



        }

        private void 全て選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }
}
