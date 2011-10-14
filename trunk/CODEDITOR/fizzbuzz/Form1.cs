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
            for (int i = 1; i < 101; i++) {
                if (i % 3 == 0 && i % 5 == 0) { textBox1.Text += fb[0]; }
                else if (i % 3 == 0) { textBox1.Text += fb[1];}
                else if (i % 5 == 0) { textBox1.Text += fb[2]; }
                else { textBox1.Text += Convert.ToString(i) + "\r\n"; }            
            }

        }


        //FizzBuzzNET MATH MANIA
        private void button2_Click(object sender, EventArgs e)
        {

            textBox1.Text = "";
            int mode = 0,goukei = 0;
            string suuji = "",ikketame ="";
            string[] fb;
            fb = new string[4] { "(ﾟ∀ﾟ)すうがくねた", "Fizz\r\n", "Buzz\r\n", "FizzBuzz\r\n" };
            for (int i = 1; i < 101; i++)
            {
                mode=0;goukei = 0;
                
                suuji = Convert.ToString(i);
                ikketame = suuji.Substring(suuji.Length-1,1);

                for (int z=0;z<suuji.Length;z++)
                    goukei +=  Convert.ToInt32(suuji[z]);

                while (goukei >= 3)
                    goukei -= 3;

                if (goukei == 0)
                    mode |=1;


                if (ikketame == "0" || ikketame == "5")
                    mode |= 2;

                if (mode != 0)
                    textBox1.Text += fb[mode];
                else 
                    textBox1.Text += Convert.ToString(i) + "\r\n";
            }
        }
    }
}
