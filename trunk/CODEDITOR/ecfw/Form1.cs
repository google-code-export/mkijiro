using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string line = "";
            StringBuilder sb = new StringBuilder();

            if (File.Exists("game.txt"))
            {
                if (checkBox1.Checked == true)
                {

                    StreamReader sr = new StreamReader("game.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                    //内容を一行ずつ読み込む
                    while (sr.Peek() > -1)
                    {
                        line = sr.ReadLine().Trim();
                        sb.Append("umdemu,");
                        sb.AppendLine(line.Replace(" ", ","));
                    }
                    //閉じる
                    sr.Close();
                }


                if (checkBox2.Checked == true)
                {
                StreamReader sh = new StreamReader("game.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                //内容を一行ずつ読み込む
                while (sh.Peek() > -1)
                {
                    line = sh.ReadLine().Trim();
                    sb.Append("game,");
                    sb.AppendLine(line.Replace(" ", ","));
                }
                //閉じる
                sh.Close();
            }

            }

            if (File.Exists("pops.txt") && checkBox3.Checked == true)
            {
                {
                    StreamReader sp = new StreamReader("pops.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                    //内容を一行ずつ読み込む
                    while (sp.Peek() > -1)
                    {
                        line = sp.ReadLine().Trim();
                        sb.Append("pops,");
                        sb.AppendLine(line.Replace(" ", ","));
                    }
                    //閉じる
                    sp.Close();
                }
            }

            line = sb.ToString();
            if (checkBox4.Checked == true) {
                line = line.Replace("ef0:", "ms0:");           
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter("PLUGINS.TXT",  false, System.Text.Encoding.GetEncoding("shift_jis"));
            //TextBox1.Textの内容を書き込む
            sw.Write(line);
            //閉じる
            sw.Close();



        }
    }
}
