using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Diagnostics;

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
            string[] vfpurg =  gettxt();
            uint k= 0;

            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            listView6.Items.Clear();
            listView7.Items.Clear();
            listView8.Items.Clear();
            k=lsadd(vfpurg,k, listView1);
            k = lsadd(vfpurg, k, listView2);
            k = lsadd(vfpurg, k, listView3);
            k = lsadd(vfpurg, k, listView4);
            k = lsadd(vfpurg, k, listView5);
            k = lsadd(vfpurg, k, listView6);
            k = lsadd(vfpurg, k, listView7);
            k = lsadd(vfpurg, k, listView8);
        }

        private string[] gettxt()
        {
            string[] vfpurg = new string[128];
            if (File.Exists("vfpureg.txt")==true){
            StreamReader r = new StreamReader("vfpureg.txt", Encoding.GetEncoding(932));
            string s = "";
            string[] vfpusp;
            string[] cut = new string[1];
            uint i = 0;
            cut[0] = "- ";
            while (r.Peek() > -1)
            {
                s = r.ReadLine();
                vfpusp = s.Split(cut, StringSplitOptions.None);
                vfpurg[i] = vfpusp[0].Trim().Remove(0, 6);
                vfpurg[i + 1] = vfpusp[1].Trim().Remove(0, 6);
                i += 2;
            }
            r.Close();
            }
            return vfpurg;
        }

        private uint lsadd(string[] vfpurg, uint k, ListView ls)
        {
            string[] item1 = { "+0", vfpurg[k], vfpurg[k + 4], vfpurg[k + 8], vfpurg[k + 12] };
            string[] item2 = { "+1", vfpurg[k + 1], vfpurg[k + 5], vfpurg[k + 9], vfpurg[k + 13] };
            string[] item3 = { "+2", vfpurg[k + 2], vfpurg[k + 6], vfpurg[k + 10], vfpurg[k + 14] };
            string[] item4 = { "+3", vfpurg[k + 3], vfpurg[k + 7], vfpurg[k + 11], vfpurg[k + 15] };
            ls.Items.Add(new ListViewItem(item1));
            ls.Items.Add(new ListViewItem(item2));
            ls.Items.Add(new ListViewItem(item3));
            ls.Items.Add(new ListViewItem(item4));
            k += 16;
            return k;
        }

        private string[] remap(string[] vfpurg)
        {
            string[] vfpurg2 = new string[128];
            uint i = 0;
            uint j= 0; 
            uint k = 0;
            for (i = 0; i < 128; i+=4)
            {
                if (i != 0 && (i % 16) == 0)
                {
                    j = 0;
                    k += 16;
                }
                vfpurg2[i] = vfpurg[k+j];
                vfpurg2[i + 1] = vfpurg[k + 4+j];
                vfpurg2[i + 2] = vfpurg[k + 8+j];
                vfpurg2[i + 3] = vfpurg[k + 12+j];
                j++;
            }
            return vfpurg2;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string[] vfpurg = gettxt();
            vfpurg = remap(vfpurg);
            StringBuilder sb = new StringBuilder();
            uint i = 0;
            uint j = 0;

            for (j = 0; j < 128; j += 16)
            {
                sb.Append("<table border=1><tr><th>S");
                sb.Append((j<<4).ToString("X3"));
                sb.Append("</th><th width=\"100\">+00</th><th width=\"100\">+10</th><th width=\"100\">+20</th><th width=\"100\">+30</th></tr>");
                for (i = 0; i < 16; i += 4)                {
                    sb.Append("<tr><th>+");
                    sb.Append((i>>2).ToString());
                    sb.Append("</th><td title=\"S");
                    sb.Append(((j<<4)+(i >>2)).ToString("X3"));
                     sb.Append("\">");
                     sb.Append(vfpurg[i + j]);
                     sb.Append("</td><td title=\"S");
                     sb.Append(((j << 4) + (i >> 2) +16).ToString("X3"));
                     sb.Append("\">");
                     sb.Append(vfpurg[i + 1 + j]);
                     sb.Append("</td><td title=\"S");
                     sb.Append(((j << 4) + (i >> 2)+32).ToString("X3"));
                     sb.Append("\">");
                     sb.Append(vfpurg[i + 2 + j]);
                     sb.Append("</td><td title=\"S");
                     sb.Append(((j << 4) + (i >> 2)+48).ToString("X3"));
                     sb.Append("\">");
                    sb.Append(vfpurg[i + 3+j]);
                    sb.Append("</td></tr>");
                }
                sb.AppendLine("</table>");
            }
        StreamWriter sw = new StreamWriter("vfpure.html",    false, System.Text.Encoding.GetEncoding(65001));
            sw.Write(sb.ToString());
            sw.Close();
            Process.Start("vfpure.html");

        }
        
        //private void vcp(ListView ls,uint k)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("VREG_CP;S ");
        //    sb.Append(",P ");
        //    sb.Append(",T ");
        //    sb.Append(",Q ");
        //    sb.Append(",P ");
        //    sb.Append(",M ");
        //    sb.Append(",N ");
        //    sb.Append(",O ");
        //}


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rmaptbl(ListView ls, int x, int y)
        {
            listView1.SetBounds(9 + x * 308, 3 + y * 103, 0, 0, BoundsSpecified.Location);
        }

        private void v24_Click(object sender, EventArgs e)
        {
            this.Size = new Size(676, 534);
            listView1.SetBounds(9 + 0* 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView2.SetBounds(9 + 0* 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView3.SetBounds(9 + 0 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView4.SetBounds(9 + 0 * 308, 3 + 3 * 103, 0, 0, BoundsSpecified.Location);
            listView5.SetBounds(9 + 1 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView6.SetBounds(9 + 1 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView7.SetBounds(9 + 1 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView8.SetBounds(9 + 1 * 308, 3 + 3 * 103, 0, 0, BoundsSpecified.Location);
        }

        private void h24_Click(object sender, EventArgs e)
        {
            this.Size = new Size(676, 534);
            listView1.SetBounds(9 + 0 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView2.SetBounds(9 + 1 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView3.SetBounds(9 + 0 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView4.SetBounds(9 + 1 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView5.SetBounds(9 + 0 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView6.SetBounds(9 + 1 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView7.SetBounds(9 + 0 * 308, 3 + 3 * 103, 0, 0, BoundsSpecified.Location);
            listView8.SetBounds(9 + 1 * 308, 3 + 3 * 103, 0, 0, BoundsSpecified.Location);
        }

        private void v33_Click(object sender, EventArgs e)
        {
            this.Size = new Size(1000, 430);
            listView1.SetBounds(9 + 0 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView2.SetBounds(9 + 0 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView3.SetBounds(9 + 0 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView4.SetBounds(9 + 1 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView5.SetBounds(9 + 1 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView6.SetBounds(9 + 1 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView7.SetBounds(9 + 2 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView8.SetBounds(9 + 2 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);

        }

        private void h33_Click(object sender, EventArgs e)
        {
            this.Size = new Size(1000, 430);
            listView1.SetBounds(9 + 0 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView2.SetBounds(9 + 1 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView3.SetBounds(9 + 2 * 308, 3 + 0 * 103, 0, 0, BoundsSpecified.Location);
            listView4.SetBounds(9 + 0 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView5.SetBounds(9 + 1 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView6.SetBounds(9 + 2 * 308, 3 + 1 * 103, 0, 0, BoundsSpecified.Location);
            listView7.SetBounds(9 + 0 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);
            listView8.SetBounds(9 + 1 * 308, 3 + 2 * 103, 0, 0, BoundsSpecified.Location);

        }

        private void vERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VFPUHELP.Form2 f = new VFPUHELP.Form2();
            f.ShowDialog(this);
            f.Dispose();

        }
    }
}
