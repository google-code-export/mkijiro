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

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            InitializeListView();
        }

        private void InitializeListView()
        {
            listView1.GridLines = true;
            listView1.Sorting = SortOrder.Ascending;
            listView1.View = View.Details;

            ColumnHeader[] columnHead = new ColumnHeader[17];
            for (int i = 0; i < 17; i++)
                columnHead[i] = new ColumnHeader();

            columnHead[0].Text = "オフセット      ";
            columnHead[0].Width = 100;

            for (int k = 1; k < 17; k++)
            {
                columnHead[k].Text = "0" + Convert.ToString(k - 1, 16).ToUpper();
                columnHead[k].Width = 50;
            }

            listView1.Columns.AddRange(columnHead);
            listView1.HideSelection = false;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }


        private void button1_Click(object sender, EventArgs e)
        {

            string filepath = this.Text;

            if (File.Exists(filepath) == true)
            {

                hexview(filepath, 128  - 2);
            }
            else
            {
                MessageBox.Show("ファイルが存在しません", "ファイルなし");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string filepath = this.Text;

            if (File.Exists(filepath) == true)
            {
                hexview(filepath, 128 * 3 - 2);
            }
            else
            {
                MessageBox.Show("ファイルが存在しません", "ファイルなし");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            string filepath = this.Text;

            if (File.Exists(filepath) == true)
            {
                hexview(filepath, 128 * 4 - 2);
            }
            else
            {
                MessageBox.Show("ファイルが存在しません", "ファイルなし");
            }
        }

        public int hexview(string filepath,int pos)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long trimsize = 0;
            long isosize = fs.Length;
            long get = 0;
            long offset = 0;
            int counter = 0;
            byte[] sector = new byte[8192];
            byte[] hex = new byte[16];
            byte[] x = new byte[8];
            fs.Seek(0x8050, SeekOrigin.Begin);
            fs.Read(x, 0, 3);
            trimsize = BitConverter.ToInt64(x, 0) << 11;
            label1.Text = "-2kのオフセット;" + Convert.ToString(trimsize - 2048, 16).ToUpper(); ;
            label2.Text = "-6kのオフセット;" + Convert.ToString(trimsize - 6144, 16).ToUpper(); ;

            if (fs.Length + 2048 < trimsize && pos > 128)
            {
                MessageBox.Show("-6kかけてるため選択できません", "範囲外エラー");
                button2.Enabled = false;
            }
            else
            {
                get = isosize - (trimsize - 8192);
                offset = trimsize - 8192;
                if (get > 8192)
                {
                    get = 8192 + 2048;
                    isosize = trimsize + 2048;
                    label5.Enabled = true;
                    button3.Enabled = true;
                    label5.Text = "オーバーダンプ:" + Convert.ToString(trimsize,16).ToUpper();
                    Array.Resize(ref sector, Convert.ToInt32(get));
                }
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(sector, 0, Convert.ToInt32(get));

                listView1.Items.Clear();
                string[] item = new string[17];

                while ((offset + 16 * counter) < isosize)
                {
                    Array.ConstrainedCopy(sector, 16 * counter, hex, 0, 16);
                    item[0] = Convert.ToString(offset + 16 * counter, 16).ToUpper();
                    if (counter % 128 == 0)
                    {
                        item[0] += "☆";
                    }
                    if (counter % 128 == 127)
                    {
                        item[0] += "★";
                    }
                    for (int i = 1; i < 17; i++)
                    {
                        item[i] = Convert.ToString(hex[i - 1], 16).ToUpper();
                    }

                    listView1.Items.Add(new ListViewItem(item));
                    counter++;
                }

                listView1.EnsureVisible(listView1.Items.Count - 1);
                listView1.EnsureVisible(pos);
                listView1.Focus();
            }
            fs.Close();
                return 0;
        }

    }
}
