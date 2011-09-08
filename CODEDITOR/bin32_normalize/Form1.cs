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
            int sign = hex>>31;
            int exponent = ((hex >> 23)& 0xFF);
            int fraction = hex & 0x7FFFFF;
            float bin = total(fraction); 
            string s1 = "+";//符号
            string s2 = exponent.ToString();//指数
            string s3 = bin.ToString();//仮数
            if (sign==-1) { s1 = "-"; }
            CultureInfo ci = CultureInfo.CurrentCulture;
            string trans1,trans2,trans3,trans4 = "";
            if (ci.ToString() == "ja-JP"){
                trans1 = Properties.Resources.s1;
                trans2 = Properties.Resources.s2;
                trans3 = Properties.Resources.s3;
                trans4 = Properties.Resources.s4;
            }
            else{
                trans1 = Properties.Resources.s1_e;
                trans2 = Properties.Resources.s2_e;
                trans3 = Properties.Resources.s3_e;
                trans4 = Properties.Resources.s4_e;
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
            if ((hex & 0x7FFFFFFF) <= 0x7F800000){
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
            else{
                sb.Append(trans4);
            }
            textBox1.Text = sb.ToString();
        }

        private float total(int num){
            float z = 0.0f;
            float t = 1.0f;
            for(int i=0 ; i<23;i++){
                t /= 2.0f;
                if (((num >>(22-i))&1) ==1){//bit boxing
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
    }
}
