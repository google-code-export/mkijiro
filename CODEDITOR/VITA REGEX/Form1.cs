using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

          string[] files = Directory.GetFiles(Application.StartupPath);
         listView1.Clear();
         listView1.Columns.Add("FILEPATH",100, HorizontalAlignment.Left);
         listView1.Columns.Add("OK", 150, HorizontalAlignment.Left);


         string[] ls = new string[2];

         foreach (string s in files)
         {            
            ls[0]= Path.GetFileName(s);
            Regex VITA＿FILE_NAME_MASK = new Regex("^[A-Z0-9\\-_]{1,8}(\\.[A-Z0-9\\-_]{1,3})?$");
            Match m = VITA＿FILE_NAME_MASK.Match(Path.GetFileName(s));
            if (m.Success == false)
            {
                if (ls[0].Substring(0, 1) == ".")
                {
                    ls[1] = ("UNIX DOTFILE(IGNORE)");
                }
                else
                {
                    Match mm = VITA＿FILE_NAME_MASK.Match(Path.GetFileName(s.ToUpper()));
                    if (mm.Success)
                    {
                        ls[1] = ("NEED UPPER STRING");
                    }
                    else
                    {
                        ls[1] = ("NEED FIX(VITA8.3RULE)");
                    }
                }
            }
            else {
                ls[1]=("OK");
            }

            listView1.Items.Add(new ListViewItem(ls));
         }
        }

        private string vita83(string s){

            Regex rip = new Regex("[^0-9A-Za-z\\.\\-_]");
            Match r = rip.Match(s);
            while (r.Success)
            {
                s = s.Replace(r.Value, "");
                r = r.NextMatch();
            }

            string[] ss = s.Split('.');

            
            if (ss.Length >= 2)
            {
                if (ss.Length >= 3)
                {
                    ss[0] = s.Substring(0, s.LastIndexOf("."));
                }

                if (ss[0].Length > 6)
                {
                    ss[0] = ss[0].Substring(0, 6);
                }
                if (ss[ss.Length - 1].Length > 3) {
                    ss[ss.Length - 1] = ss[ss.Length - 1].Substring(0, 3);
                }

                s = ss[0].Replace(".", "_") + "N1." + ss[ss.Length - 1].Replace(".", "_");
            }
            else
            {
                if (s.Length > 6)
                {
                    s = s.Substring(0, 6);
                }
                s = ss[0].Replace(".", "_") + "N1";            
            }

            return s.ToUpper();
        }


        private string dosfilerenamer(string cp, string cp2)
        {
            string dir = Application.StartupPath;

            string bn = Path.GetFileNameWithoutExtension(cp2);

              bn= bn.Substring(0, bn.LastIndexOf("N"));

                string ext = Path.GetExtension(cp2);
                int i = 0;
                for (i = 1; i < 10; i++)
                {
                    cp2 =  bn + "N" + i.ToString("D1") + ext;
                    if (File.Exists(cp2) == false)
                    {
                        File.Move(cp, cp2);
                        return "";
                    }
                }

                bn = bn.Substring(0, bn.Length - 1);
                for (i = 1; i < 100; i++)
                {
                    cp2 =  bn + "N" + i.ToString("D2") + ext;
                    if (File.Exists(cp2) == false)
                    {
                        File.Move(cp, cp2);
                        return "";
                    }
                }


            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string[] files = Directory.GetFiles(Application.StartupPath);
            listView1.Clear();
            listView1.Columns.Add("FILEPATH", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("OK", 100, HorizontalAlignment.Left);


            string[] ls = new string[2];

            foreach (string s in files)
            {
                ls[0] = Path.GetFileName(s);
                Regex VITA＿FILE_NAME_MASK = new Regex("^[A-Z0-9\\-_]{1,8}(\\.[A-Z0-9\\-_]{1,3})?$");
                Match m = VITA＿FILE_NAME_MASK.Match(Path.GetFileName(s));
                if (m.Success == false)
                {
                    if (ls[0].Substring(0, 1) != ".")
                    {
                        File.Move(s, Application.StartupPath + "TEMP");
                        if (File.Exists(Application.StartupPath + "TEMP") == true)
                        {
                            Match mm = VITA＿FILE_NAME_MASK.Match(Path.GetFileName(s.ToUpper()));
                            if (mm.Success == true)
                            {
                                File.Move(Application.StartupPath +"TEMP", s.ToUpper());
                            }
                            else
                            {
                                dosfilerenamer(Application.StartupPath + "TEMP", vita83(Path.GetFileName(s)));
                            }
                        }
                    }
                }

            }

        }

        private void fIXNANUALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Form2 f = new Form2();
                f.Text = listView1.SelectedItems[0].Text;
                f.ShowDialog(this);
                f.Dispose();
            }
        }

        private string addunixdot(string s) {
            if (s.Substring(0, 1) != ".")
            {
                s = "." + s;
            }

            return s;
        }

        private string rmunixdot(string s)
        {
            while (s.Substring(0, 1) == ".") {
                s = s.Substring(1,s.Length - 1);
            }

            return s;
        }

        private void cHAGEUNIXDOTFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0){
            string s = listView1.SelectedItems[0].Text;
            if (File.Exists(Application.StartupPath + "\\" + addunixdot(s)) == false)
            {
                File.Move(Application.StartupPath + "\\" + s, Application.StartupPath + "\\" + addunixdot(s));
            }
        }
        }

        private void rEMOVEUNIXDOTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0){
              string s = listView1.SelectedItems[0].Text;
             if (File.Exists(Application.StartupPath + rmunixdot(s)) == false)
             {
                 File.Move(Application.StartupPath + "\\" + s, Application.StartupPath + "\\" + rmunixdot(s));
             }
        }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                fIXNANUALToolStripMenuItem.Enabled = true;
                string s = listView1.SelectedItems[0].Text;
                if (s.Substring(0, 1) == ".") {
                    fIXNANUALToolStripMenuItem.Enabled = false;
                }
            }
        }
    }
}
