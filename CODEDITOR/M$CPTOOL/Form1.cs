using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
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
            Int64 i = 0;
            int k = 0;
            int z = 0;
            int mscp = 0;
            int skipok = -1;
            Int64 skip = 0;
            byte[] st = new byte[1];
            byte[] st1 = new byte[1];
            byte[] st2 = new byte[2];
            byte[] st3 = new byte[3];
            byte[] st4 = new byte[4];
            string s = "";
            string ss = "";
            string sss = "";
            byte[] unicp = new byte[4];
            byte[] zenkaku = new byte[255];
            int zct = 0;
            byte[] hex = new byte[8];
            StringBuilder sb = new System.Text.StringBuilder();

            Regex r = new Regex("^[0-9]+", RegexOptions.ECMAScript);
            Match m = r.Match(comboBox1.Text);
            if (m.Success == true)
            {
                mscp = Convert.ToInt32(m.Value, 10);
            }


            sb.Append("//CP");
            sb.Append(mscp.ToString());
            sb.AppendLine("(IN,LOCAL->UNICODE)");

            ss = "CP" + mscp.ToString() + "(IN)";
            sss = "CP" + mscp.ToString() + "(OUT).txt";

            if (checkBox1.Checked ==true){
                if (File.Exists(sss)==true) {
                    StreamReader sr = new StreamReader(sss,  Encoding.GetEncoding(65001));
                    s= sr.ReadToEnd();
                    sr.Close();

                    Regex rk = new Regex("U.30FB\t0x[0-9A-Fa-f]+", RegexOptions.ECMAScript);
                    Match mk = rk.Match(s);
                    if (mk.Success == true)
                    {
                        skipok = Convert.ToInt32(mk.Value.Remove(0, 9), 16);
                        textBox1.Text = "0x" + skipok.ToString("X");
                    }
                }
            }
            else
            {

                Regex rr = new Regex("0x[0-9A-Fa-f]+", RegexOptions.ECMAScript);
                Match mm = rr.Match(textBox1.Text);
                if (mm.Success == true)
                {
                    skipok = Convert.ToInt32(mm.Value.Remove(0, 2), 16);
                }
            }

            for (i = 0; i < 0x10000; i++)
            {
                hex = BitConverter.GetBytes(i);
                if (i >= 0x1000000)
                {
                    st4[0] = (byte)(i >> 124);
                    st4[1] = (byte)(i >> 16);
                    st4[2] = (byte)(i >> 8);
                    st4[3] = (byte)(i & 0xff);
                    st[0]=st4[0];
                    s = Encoding.GetEncoding(mscp).GetString(st4);
                    z = 4;
                }
                else if (i >= 0x10000)
                {
                    st3[0] = (byte)(i >> 16);
                    st3[1] = (byte)(i >> 8);
                    st3[2] = (byte)(i & 0xff);
                    st[0]=st3[0];
                    s = Encoding.GetEncoding(mscp).GetString(st3);
                    z = 3;
                }
                else if (i >= 0x100) {

                    st2[0] = (byte)(i >> 8);
                    st2[1] = (byte)(i & 0xff);
                    st[0]=st2[0];

                    s = Encoding.GetEncoding(mscp).GetString(st2);
                    z = 2;
                }
                else 
                {
                    st1[0] = (byte)(i & 0xff);
                    st[0]=st1[0];
                    s = Encoding.GetEncoding(mscp).GetString(st1);
                    z = 1;
                }

                unicp = Encoding.GetEncoding(12000).GetBytes(s);
                skip = BitConverter.ToInt32(unicp, 0);

                if (i ==0xa || i==0xd)
                {
                    s = "";
                }

                if (i < 256 && skip == 0x30fb)
                {
                    zenkaku[zct] = (byte)i;
                    zct++;
                }
                if (i == 255)
                {
                    Array.Resize(ref zenkaku, zct);
                }

                if((i<256) ||(iszenkaku(st[0],zenkaku)==true)){

                if ((i != 0x3f) && ((skip == 0x3f) ))
                {
                }
                if ((i!=skipok) && ((skip == 0x30fb)))
                {
                }
                else
                {
                    sb.Append("0x");
                    k = 0;
                    if (i >= 0x1000000)
                    {
                        while (k < z)
                        {
                            sb.Append(st4[k].ToString("X2"));
                            k++;
                        }
                    }
                    else if (i >= 0x10000)
                    {
                        while (k < z)
                        {
                            sb.Append(st3[k].ToString("X2"));
                            k++;
                        }
                    }
                    else if (i >= 0x100) {
                        while (k < z)
                        {
                            sb.Append(st2[k].ToString("X2"));
                            k++;
                        }
                    }
                    else 
                    {
                            sb.Append(st1[0].ToString("X2"));
                    }
                    sb.Append("\t");
                    sb.Append("U+");
                    sb.Append(skip.ToString("X"));
                    sb.Append("\t");
                    sb.Append("#");
                        sb.AppendLine(s.Replace("\x0", ""));
                }
                }

            }

            StreamWriter sw = new StreamWriter(ss + ".txt", false, Encoding.GetEncoding(65001));
            sw.Write(sb.ToString());
            sw.Close();
            System.Media.SystemSounds.Beep.Play();

        }

        private bool iszenkaku(byte h,byte[] k) {
            for (int i = 0;i<k.Length ; i++) {
                if (h == k[i]) {
                    return true;
                }            
            }
                return false;
          }


        private void button2_Click(object sender, EventArgs e)
        {

            int i = 0;
            int k = 0;
            int z = 0;
            int mscp = 0;
            Int64 skip = 0;
            byte[] st = new byte[1];
            string s = "";
            string ss = "";
            byte[] unicp = new byte[4];
            byte[] hex = new byte[8];          
            StringBuilder sb = new System.Text.StringBuilder();

            Regex r = new Regex("^[0-9]+", RegexOptions.ECMAScript);
            Match m = r.Match(comboBox1.Text);
              if (m.Success == true)
            {
                mscp = Convert.ToInt32(m.Value, 10);
            }

              sb.Append("//CP");
              sb.Append(mscp.ToString());
              sb.AppendLine("(OUT,UNICODE->LOCAL)");

              ss = "CP" + mscp.ToString() + "(OUT)";

            for (i = 0; i < 0x110000; i++)
            {
                unicp = BitConverter.GetBytes(i);
                s = Encoding.GetEncoding(12000).GetString(unicp);
                st = Encoding.GetEncoding(mscp).GetBytes(s);
                z = st.Length;
                Array.Clear(hex, 0, 8);
                Array.Copy(st, 0, hex, 0, z);
                skip = BitConverter.ToInt64(hex, 0);

                if ((i != 0x3f) && ((skip == 0x3f)|| (skip == 0x3f3f)||(skip == 0x3f3f3f)||(skip == 0x3f3f3f3f))){
                }
                else{
                sb.Append("U+");
                sb.Append(i.ToString("X"));
                sb.Append("\t");
                sb.Append("0x");
                k = 0;
                while (k < z) {
                    sb.Append(st[k].ToString("X2"));
                    k++;
                }
                sb.Append("\t");
                sb.Append("#");
                if (i == 0xa || i == 0xd)
                {
                    s = "";
                }
                sb.AppendLine(s.Replace("\x0",""));
                }

            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(ss+".txt", false, System.Text.Encoding.GetEncoding(65001));  
            sw.Write(sb.ToString());
            sw.Close();

            System.Media.SystemSounds.Beep.Play();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !checkBox1.Checked;
        }
    }
}
