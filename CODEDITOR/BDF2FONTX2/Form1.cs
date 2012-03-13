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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Filter =
                "bdfファイル(*.bdf)|*.bdf";
            ofd.Title = "開くファイルを選択してください";

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                string s = "";
                bool parse = false;
                int stpos = 0;
                int tmp = 0;
                int basepos =0;
                int yoko = 0;
                int tate = 0;
                byte[] hex = new byte[4];
                byte[] header = {0x46 ,0x4F ,0x4E ,0x54, 0x58 ,0x32 ,0x42 ,0x44 ,0x46 ,0x32 ,0x46, 0x54 ,0x58 ,0x32, 0x8,0x10, 0 };
                byte[] font = new byte[100*1024];
                StreamReader sr = new System.IO.StreamReader( path,System.Text.Encoding.GetEncoding(932));
                while (sr.Peek() > -1)
                {
                    s = sr.ReadLine();
                    if (s.Contains("STARTCHAR"))
                    {
                        basepos = Convert.ToInt32(s.Replace("STARTCHAR","").Trim(), 16);
                    }
                    else if ((s.Contains("BBX")==true)&& (yoko==0))
                    {
                       Regex r =    new System.Text.RegularExpressions.Regex("\\d+",   System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                       Match m = r.Match(s);
                        int l =0;

                        while (m.Success)
                        {
                            if (l==0){
                        yoko=Convert.ToInt32(m.Value);}
                            if(l==1){
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
                    else if (parse) {
                        tmp = Convert.ToInt32(s,16);
                        hex = BitConverter.GetBytes(tmp);
                        if (yoko > 8)
                        {
                            Array.Copy(hex, 1, font, (basepos * tate * 2) + stpos, 1);
                            Array.Copy(hex, 0, font, (basepos * tate *2) + stpos+1, 1);
                            stpos+=2;
                        }
                        else
                        {
                            Array.Copy(hex, 0, font, (basepos * tate) + stpos, 1);
                            stpos++;
                        }
                    }
                }
                sr.Close();
                hex = BitConverter.GetBytes(yoko);
                Array.Copy(hex, 0, header, 14, 1);
                hex = BitConverter.GetBytes(tate);
                Array.Copy(hex, 0, header, 15, 1);

                if (yoko > 8)
                {
                    Array.Resize(ref font, tate * 255 *2);
                }
                else
                {
                    Array.Resize(ref font, tate * 255);
                }
                FileStream fs = new System.IO.FileStream(Path.GetFileNameWithoutExtension(path)+".fnt",  FileMode.Create,FileAccess.Write);
                fs.Write(header,0,17);
                fs.Write(font,0,font.Length);
                fs.Close();

            }
        }
    }
}
