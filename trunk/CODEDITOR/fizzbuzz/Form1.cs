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

        //FizzBuzzNETA
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            string[] fb;
            fb = new string[3] { "FizzBuzz\r\n", "Fizz\r\n", "Buzz\r\n" };
           decimal f1 = 0, f2 = 1, fn = 0;
                for (int i = 1; i < 101; i++)
                {

                    if (Plus1.Checked == true)
                    { fn = i; }
                    else if (Fibonacci_number.Checked == true)
                    {
                        if (i == 1)
                        {
                            fn = f2;
                        }
                        else
                        {
                            fn = f2 + f1;
                            f1 = f2;
                            f2 = fn;
                        }
                    }
                    else if (SQ.Checked == true)
                    {
                        fn = i * i;//Math.Pow(i, i);
                    }
                    //else if (Cube.Checked == true)
                    //{
                    //        fn = i*i*i;
                    //}

                    if (fn % 3 == 0 && fn % 5 == 0) { textBox1.Text += fb[0]; }
                    else if (fn % 3 == 0) { textBox1.Text += fb[1]; }
                    else if (fn % 5 == 0) { textBox1.Text += fb[2]; }
                    else { textBox1.Text += Convert.ToString(fn) + "\r\n"; }
                }            
        }


        //FizzBuzzNETA MATHEMATIC MANIA STYLE
        private void button2_Click(object sender, EventArgs e)
        {

            textBox1.Text = "";
            int mode = 0,goukei = 0;
            decimal f1 = 0, f2 = 1, fn = 0;
            string suuji = "",ikketame ="";
            string[] fb;
            fb = new string[4] { "(ﾟ∀ﾟ)すうがくねた", "Fizz\r\n", "Buzz\r\n", "FizzBuzz\r\n" };
                for (int i = 1; i < 101; i++)
                {

                    if (Plus1.Checked == true)
                    {   fn=i;
                    }
                    else if (Fibonacci_number.Checked == true)
                    {
                        //fn=1/√5*((１＋√５)/2)^n-1/√5((１－√５)/2)^n　らすい？
                        //http://ja.wikipedia.org/wiki/%E3%83%95%E3%82%A3%E3%83%9C%E3%83%8A%E3%83%83%E3%83%81%E6%95%B0
                        double goldenrate = (1+ Math.Sqrt(5))*0.5;
                        double rootfive = Math.Sqrt(5);

                        //fn = Convert.ToDecimal(Math.Floor((Math.Pow((iti + root)*0.5, i) - Math.Pow((iti - root)*0.5, i))/root));
                        fn = Convert.ToDecimal(Math.Floor(Math.Pow(goldenrate, i) / rootfive + 0.5));
                        if (i == 70)
                        {
                            textBox1.Text += "Binary64限界,＼(^o^)／ｵﾜﾀ"; break;
                        }
                    }
                    else if (SQ.Checked == true)
                    {
                        if (i == 1)
                        {
                            fn = f2;
                        }
                        else
                        {
                            //斬蚊屍鬼（ ＾ω＾）
                            fn = f2 + 2 * i - 1;
                            f2 = fn;
                        }
                    }

                    mode = 0; goukei = 0;

                    suuji = Convert.ToString(fn);
                    ikketame = suuji.Substring(suuji.Length - 1, 1);

                    for (int z = 0; z < suuji.Length; z++)
                        goukei += Convert.ToInt32(suuji[z]);

                    while (goukei >= 3)
                        goukei -= 3;

                    if (goukei == 0)
                        mode |= 1;


                    if (ikketame == "0" || ikketame == "5")
                        mode |= 2;

                    if (mode != 0)
                        textBox1.Text += fb[mode];
                    else
                        textBox1.Text += Convert.ToString(fn) + "\r\n";
                }
        }
    }
}
