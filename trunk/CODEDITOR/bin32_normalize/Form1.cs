using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            int hex = Convert.ToInt32(textBox2.Text, 16);
            int sign = hex>>31;
            int exponent = ((hex >> 23)& 0xFF);
            int franction = hex & 0x7FFFFF;
            float bin = total(franction); 
            string s1 = "";
            string s2 = exponent.ToString();
            string s3 = bin.ToString();
            textBox1.Text = "sign:";
            if (sign==-1) {
                s1 = "-";
            }
            else{
                s1 = "+";
            }
            textBox1.Text += s1;
            textBox1.Text += "\r\n";
            textBox1.Text += "exponent:";
            textBox1.Text += s2;
            textBox1.Text += "\r\n";
            textBox1.Text += "franctino:";
            textBox1.Text += s3;
            textBox1.Text += "\r\n";

            textBox1.Text += s1;
            textBox1.Text += "1 x 2^(";
            textBox1.Text += s2;
            textBox1.Text += "-127) x 1.";
            textBox1.Text += s3.Replace("0.","");


        }

        private float total(int num){
            float z = 0.0f;
            float t = 1.0f;
            for(int i=0 ; i<23;i++){
                t /= 2.0f;
                if (((num >>(22-i))&1) ==1){
                z += t;
                }
            }
            return z;
        }
    }
}
