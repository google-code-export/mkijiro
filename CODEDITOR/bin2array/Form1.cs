using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    
    public partial class Form1 : Form
    {
        string pp = "";

        public Form1()
        {
            InitializeComponent();
            this.textBox1.DragDrop += new
            System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new
            System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
        }

        private void textBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBox1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string filepath = s[0];
            pp = filepath;
            string LANG = "&H";
            if (CS.Checked == true) { LANG = "0x"; }
            string tmp = "";
            
            FileStream fs = new FileStream(filepath,FileMode.Open, FileAccess.Read);
            long fsize = fs.Length;

            if (fs.Length > 1 && fs.Length < 1024*1024)
            {
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                StringBuilder st = new StringBuilder();

                if (MD5.Checked ==false){
                if (CS.Checked == true)
                {
                    st.Append("byte[] ");
                    st.Append(hairetu.Text);
                    st.Append(" = {");
                }
                else
                {
                    st.Append("Dim ");
                    st.Append(hairetu.Text);
                    st.Append("() As byte = {");
                }

                for (int i = 0; i < bs.Length; i++)
                {
                    st.Append(LANG);
                    tmp = Convert.ToString(bs[i], 16).ToUpper();
                    st.Append(tmp);
                    if (i + 1 != bs.Length) st.Append(", ");
                }
                st.Append("}");

                if (CS.Checked == true)
                {
                    st.Append(";");
                }
            }
                st.AppendLine();


                if (ANDs.Checked == true)
                {
                    if (CS.Checked == true)
                    {
                        string s1 = global::bin2array.Properties.Resources.f1;
                        s1 = s1.Replace("i", fileoffset.Text);
                        s1 = s1.Replace("bs", hairetu.Text);
                        s1 = s1.Replace("kazu", bs.Length.ToString());
                        st.Append(s1);
                    }
                    else
                    {
                        string s1 = global::bin2array.Properties.Resources.f2;
                        s1 = s1.Replace("i", fileoffset.Text);
                        s1 = s1.Replace("bs", hairetu.Text);
                        s1 = s1.Replace("kazu", (bs.Length-1).ToString());
                        st.Append(s1);
                    }


                    st.Append("if ");
                    string LANG2 = " andalso ";
                    if (CS.Checked == true) { LANG2 = " && "; st.Append("("); }
                    for (int i = 0; i < bs.Length; i++)
                    {
                        st.Append(kensaku.Text);
                        if (CS.Checked == true)
                        {
                            st.Append("[");
                            st.Append(fileoffset.Text);
                            if (i > 0) { st.Append("+"); st.Append(i.ToString()); }
                            st.Append("]==");
                        }
                        else
                        {
                            st.Append("(");
                            st.Append(fileoffset.Text);
                            if (i > 0) { st.Append("+");st.Append(i.ToString()); }
                            st.Append(")=");
                        }
                        tmp = LANG + Convert.ToString(bs[i], 16).ToUpper();
                        st.Append(tmp);
                        if (i + 1 != bs.Length) st.Append(LANG2);
                    }

                    if (CS.Checked == true)
                    {
                        st.Append("){\r\n}\r\n}");
                    }
                    else
                    {
                        st.Append(" Then\r\nEnd If\r\nNext");
                    }

                }
                else if (byteloop.Checked==true){


                    if (CS.Checked == true)
                    {
                        string s1 = global::bin2array.Properties.Resources.f1;
                        s1 = s1.Replace("i", fileoffset.Text);
                        s1 = s1.Replace("bs", hairetu.Text);
                        s1 = s1.Replace("kazu", bs.Length.ToString());
                        st.Append("byte[] ");
                        st.Append(hairetu.Text);
                        st.Append("2");
                        st.Append(" = new byte[");
                        st.Append(bs.Length.ToString());
                        st.AppendLine("];");
                        st.Append(s1);
                        st.Append("Buffer.BlockCopy(");
                        st.Append(kensaku.Text);
                        st.Append(", ");
                        st.Append(fileoffset.Text);
                        st.Append(", ");
                        st.Append(hairetu.Text);
                        st.Append("2");
                        st.Append(", 0, ");
                        st.Append(bs.Length.ToString());
                        st.AppendLine(");");
                        st.Append("if(");
                        st.Append(hairetu.Text);
                        st.Append(".SequenceEqual(");
                        st.Append(hairetu.Text);
                        st.Append("2");
                        st.Append(")== true){\r\n}\r\n}");
                    }
                    else
                    {
                        string s1 = global::bin2array.Properties.Resources.f2;
                        s1 = s1.Replace("i", fileoffset.Text);
                        s1 = s1.Replace("bs", hairetu.Text);
                        s1 = s1.Replace("kazu", (bs.Length-1).ToString());
                        st.Append("Dim ");
                        st.Append(hairetu.Text);
                        st.Append("2");
                        st.Append("(");
                        st.Append((bs.Length - 1).ToString());
                        st.AppendLine(") As byte = nothing");
                        st.Append(s1);
                        st.Append("Buffer.BlockCopy(");
                        st.Append(kensaku.Text);
                        st.Append(", ");
                        st.Append(fileoffset.Text);
                        st.Append(", ");
                        st.Append(hairetu.Text);
                        st.Append("2");
                        st.Append(", 0, ");
                        st.Append(bs.Length.ToString());
                        st.AppendLine(")");
                        st.Append("If ");
                        st.Append(hairetu.Text);
                        st.Append(".SequenceEqual(");
                        st.Append(hairetu.Text);
                        st.Append("2");
                        st.Append(")= True Then\r\nEnd If\r\nNext");
                    }
                                        
                }
                else if (MD5.Checked==true){

                System.IO.FileStream ffs = new System.IO.FileStream(filepath,System.IO.FileMode.Open,System.IO.FileAccess.Read,System.IO.FileShare.Read);

                System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] md = md5.ComputeHash(ffs);
                string result2 = BitConverter.ToString(md).ToUpper().Replace("-","");
                ffs.Close();

                if (CS.Checked == true)
                {
                    string s1 = global::bin2array.Properties.Resources.f1;
                    s1 = s1.Replace("i", fileoffset.Text);
                    s1=s1.Replace("bs",hairetu.Text);
                    s1=s1.Replace("kazu",bs.Length.ToString());
                    st.Append("string md5hash =\"");
                    st.Append(result2);
                    st.AppendLine("\";");
                    st.Append("byte[] ");
                    st.Append(hairetu.Text);
                     st.Append(" = new byte[");
                    st.Append(bs.Length.ToString());
                    st.AppendLine("];");
                    st.Append(s1);
                    st.Append("Buffer.BlockCopy(");
                    st.Append(kensaku.Text);
                    st.Append(", ");
                    st.Append(fileoffset.Text);
                    st.Append(", ");
                    st.Append(hairetu.Text);
                    st.Append(", 0, ");
                    st.Append(bs.Length.ToString());
                    st.AppendLine(");");
                }
                else
                {
                    string s1 = global::bin2array.Properties.Resources.f2;
                    s1 = s1.Replace("i", fileoffset.Text);
                    s1 = s1.Replace("bs", hairetu.Text);
                    s1 = s1.Replace("kazu", (bs.Length-1).ToString());
                    st.Append("Dim md5hash As string = \"");
                    st.Append(result2);
                    st.AppendLine("\"");
                    st.Append("Dim ");
                    st.Append(hairetu.Text);
                    st.Append("(");
                    st.Append((bs.Length-1).ToString());
                    st.AppendLine(") As byte");
                    st.Append(s1);
                    st.Append("Buffer.BlockCopy(");
                    st.Append(kensaku.Text);
                    st.Append(", ");
                    st.Append(fileoffset.Text);
                    st.Append(", ");
                    st.Append(hairetu.Text);
                    st.Append(", 0, ");
                    st.Append(bs.Length.ToString());
                    st.AppendLine(")");
                }

                st.Append("if ");
                string LANG2 = " andalso ";
                int z = bs.Length;
                if(z > 4) {z = 4;}

                if (CS.Checked == true) { LANG2 = " && "; st.Append("("); }
                
                for (int i = 0; i < z; i++)
                {
                    st.Append(kensaku.Text);
                    if (CS.Checked == true)
                    {
                        st.Append("[");
                        st.Append(fileoffset.Text);
                        if (i > 0) { st.Append("+"); st.Append(i.ToString()); }
                        st.Append("]==");
                    }
                    else
                    {
                        st.Append("(");
                        st.Append(fileoffset.Text);
                        if (i > 0) { st.Append("+"); st.Append(i.ToString()); }
                        st.Append(")=");
                    }
                    tmp = LANG + Convert.ToString(bs[i], 16).ToUpper();
                    st.Append(tmp);
                    if (i+1  != z) st.Append(LANG2);
                }

                if (CS.Checked == true)
                {
                    string ss = global::bin2array.Properties.Resources.md2;
                    ss = ss.Replace("data", hairetu.Text);
                    ss = ss.Replace("bs", "temphash");
                    st.Append("){\r\n");
                    st.Append(ss);

                    st.Append("if(result == md5hash){\r\n}\r\n");
                    st.Append("}\r\n}");
                }
                else
                {
                    string ss = global::bin2array.Properties.Resources.md;
                    ss = ss.Replace("data", hairetu.Text);
                    ss = ss.Replace("bs", "temphash");
                    st.Append(" then\r\n");
                    st.Append(ss);
                    st.Append("If result = md5hash then\r\nend if\r\n");
                    st.Append("End If\r\nNext");
                }


                }
                textBox1.Text = st.ToString();
                st.Clear();
            }

            fs.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {          
        }

        private void ANDs_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (File.Exists(pp))
            {
                FileStream fs = new FileStream(pp, FileMode.Open, FileAccess.Read);
                long fsize = fs.Length;

                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                byte[] array = { 0x2F, 0x4C, 0x69, 0x6E, 0x6B, 0x49, 0x6E, 0x66, 0x6F };
                int t1 = System.Environment.TickCount;
                for (int i = 0; i < bs.Length - 9; i++)
                {
                    if (bs[i] == 0x2F && bs[i + 1] == 0x4C && bs[i + 2] == 0x69 && bs[i + 3] == 0x6E && bs[i + 4] == 0x6B && bs[i + 5] == 0x49 && bs[i + 6] == 0x6E && bs[i + 7] == 0x66 && bs[i + 8] == 0x6F)
                    {
                        textBox1.Text = i.ToString("X");
                        t1 = System.Environment.TickCount - t1;
                        textBox1.Text += "\r\n";
                        textBox1.Text += t1.ToString();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (File.Exists(pp)) {
            FileStream fs = new FileStream(pp, FileMode.Open, FileAccess.Read);
            long fsize = fs.Length;

                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                byte[] array = { 0x2F, 0x4C, 0x69, 0x6E, 0x6B, 0x49, 0x6E, 0x66, 0x6F };
                byte[] array2 = new byte[9];
                int t1 = System.Environment.TickCount;
                for (int i = 0; i < bs.Length - 9; i++)
                {
                    Buffer.BlockCopy(bs, i, array2, 0, 9);
                    if (array.SequenceEqual(array2) == true)
                    {
                        textBox1.Text = i.ToString("X");
                        t1 = System.Environment.TickCount - t1;
                        textBox1.Text += "\r\n";
                        textBox1.Text += t1.ToString();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (File.Exists(pp)) {
            FileStream fs = new FileStream(pp, FileMode.Open, FileAccess.Read);
            long fsize = fs.Length;

                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
string md5hash ="064244D42EE97C9A0C0087BE10D7FBA6";
byte[] array = new byte[9];
int t1 = System.Environment.TickCount;
for (int i = 0; i < bs.Length - 9; i++)
{
    if (bs[i] == 0x2F && bs[i + 1] == 0x4C && bs[i + 2] == 0x69 && bs[i + 3] == 0x6E)
    {
        Buffer.BlockCopy(bs, i, array, 0, 9);
        System.Security.Cryptography.MD5CryptoServiceProvider md5 =
            new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] temphash = md5.ComputeHash(array);
        string result = BitConverter.ToString(temphash).ToLower().Replace("-", ""); if (result == md5hash)
        {
        }
        textBox1.Text = i.ToString("X"); ;
        t1 = System.Environment.TickCount - t1;
        textBox1.Text += "\r\n";
        textBox1.Text += t1.ToString();
    }
}

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists(pp)) {
            FileStream fs = new FileStream(pp, FileMode.Open, FileAccess.Read);
            long fsize = fs.Length;
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                byte[] array = { 0x2F, 0x4C, 0x69, 0x6E, 0x6B, 0x49, 0x6E, 0x66, 0x6F };
                byte[] array2 = new byte[9];
                int t1 = System.Environment.TickCount;          
                for (int i = 0; i < bs.Length-9; i++)
                {
                    Buffer.BlockCopy(bs, i, array2, 0, 9);
                    if (UnsafeCompare(array, array2) == true)
                    {
                        textBox1.Text = i.ToString("X"); ;
                        t1 = System.Environment.TickCount - t1;
                        textBox1.Text += "\r\n";
                        textBox1.Text += t1.ToString();
                    }
            }
            }
        }

        //http://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net
        static unsafe bool UnsafeCompare(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return false;
            fixed (byte* p1 = a1, p2 = a2)
            {
                byte* x1 = p1, x2 = p2;
                int l = a1.Length;
                for (int i = 0; i < l >>3; i++, x1 += 8, x2 += 8)
                    if (*((long*)x1) != *((long*)x2)) return false;
                if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
                return true;
            }
        }

    }

    

}
