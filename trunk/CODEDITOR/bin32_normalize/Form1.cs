using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace binary32_normalize

{
    public partial class Form1 : Form
    {

        string back = "0x3F800000";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int hex = Convert.ToInt32(textBox2.Text, 16);

            richTextBox1.Text = f2bin(hex,32);
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength= 1;
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.SelectionStart = 1;
            richTextBox1.SelectionLength = 8;
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionStart = 9;
            richTextBox1.SelectionLength = 23;
            richTextBox1.SelectionColor = Color.Green;

            int sign = hex>>31;
            int exponent = ((hex >> 23)& 0xFF);
            int fraction = hex & 0x7FFFFF;
            float bin = total(fraction,23); 
            string s1 = "+";//符号
            string s2 = exponent.ToString();//指数
            string s3 = bin.ToString();//仮数
            if (sign==-1) { s1 = "-"; }
            CultureInfo ci = CultureInfo.CurrentCulture;
            string trans1,trans2,trans3,trans4 ,trans5="";
            if (ci.ToString() == "ja-JP"){
                trans1 = Properties.Resources.s1;
                trans2 = Properties.Resources.s2;
                trans3 = Properties.Resources.s3;
                trans4 = Properties.Resources.s4;
                trans5 = Properties.Resources.s5;
            }
            else{
                trans1 = Properties.Resources.s1_e;
                trans2 = Properties.Resources.s2_e;
                trans3 = Properties.Resources.s3_e;
                trans4 = Properties.Resources.s4_e;
                trans5 = Properties.Resources.s5_e;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(trans1);
            sb.Append(s1);
            sb.Append("\r\n");
            sb.Append(trans2);
            sb.Append(s2);
            sb.Append("\r\n");
            sb.Append(trans3);
            sb.Append(s3);
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append(s1);
            if ((hex & 0x7FFFFFFF) < 0x7F800000){
            sb.Append("1 x 2^(");
            sb.Append(s2);
            sb.Append("-127) x (1.0+");
            sb.Append(s3);
            sb.Append(") = ");
            float x =0;
            double exf =Math.Pow(2,exponent-127);
            x = Convert.ToSingle(exf) * (bin + 1);
              if (sign==-1)  {x *= -1;}
            sb.Append(x.ToString());
            }
            else if (fraction == 0)
            {
                sb.Append(trans4);
            }
            else
            {
                sb.Append(trans5);
            }
            textBox1.Text = sb.ToString();
        }

        private  string f2bin(int num,int m){
            string s ="";
            for (int i = 0; i < m; i++) {
                if (((num  >> (m-1 - i)) & 1) == 1)
                {
                    s += "1";
                }
                else {
                    s += "0";
                } 
            }
            return s;
        }

        private float total(int num,int m){
            float z = 0.0f;
            float t = 1.0f;
            for(int i=0 ; i<m;i++){
                t /= 2.0f;
                if (((num >>(m-1-i))&1) ==1){//bit boxing
                z += t;
                }
            }
            return z;
        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(
    textBox2.Text, "0x[0-9a-fA-F]+",
    System.Text.RegularExpressions.RegexOptions.ECMAScript))
            {
                back = textBox2.Text;
            }
            else
            {
                textBox2.Text = back;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int hex = Convert.ToInt32(textBox2.Text.Trim(), 16);
            if ((hex & 0x7FFFFFFF) > 0xFFFF)
            {
                MessageBox.Show("16BIT 0x0～0xFFFF", "ERROR");
                return;
            }
            richTextBox1.Text = f2bin(hex,16);
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = 1;
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.SelectionStart = 1;
            richTextBox1.SelectionLength = 5;
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionStart = 6;
            richTextBox1.SelectionLength = 10;
            richTextBox1.SelectionColor = Color.Green;

            int sign = hex >>15;
            int exponent = ((hex >> 10) & 0x1F);
            int fraction = hex & 0x3FF;
            float bin = total(fraction,10);
            string s1 = "+";//符号
            string s2 = exponent.ToString();//指数
            string s3 = bin.ToString();//仮数
            if (sign == -1) { s1 = "-"; }
            CultureInfo ci = CultureInfo.CurrentCulture;
            string trans1, trans2, trans3, trans4, trans5 = "";
            if (ci.ToString() == "ja-JP")
            {
                trans1 = Properties.Resources.s1;
                trans2 = Properties.Resources.s2;
                trans3 = Properties.Resources.s3;
                trans4 = Properties.Resources.s4;
                trans5 = Properties.Resources.s5;
            }
            else
            {
                trans1 = Properties.Resources.s1_e;
                trans2 = Properties.Resources.s2_e;
                trans3 = Properties.Resources.s3_e;
                trans4 = Properties.Resources.s4_e;
                trans5 = Properties.Resources.s5_e;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(trans1);
            sb.Append(s1);
            sb.Append("\r\n");
            sb.Append(trans2);
            sb.Append(s2);
            sb.Append("\r\n");
            sb.Append(trans3);
            sb.Append(s3);
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append(s1);
            if ((hex & 0x7FFF) < 0x7C00)
            {
                sb.Append("1 x 2^(");
                sb.Append(s2);
                sb.Append("-15) x (1.0+");
                sb.Append(s3);
                sb.Append(") = ");
                float x = 0;
                double exf = Math.Pow(2, exponent - 15);
                x = Convert.ToSingle(exf) * (bin + 1);
                if (sign == -1) { x *= -1; }
                sb.Append(x.ToString());
            }
            else if(fraction ==0)
            {
                sb.Append(trans4);
            }
            else
            {
                sb.Append(trans5);
            }
            textBox1.Text = sb.ToString();
        }
    }
}
