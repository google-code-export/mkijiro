using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CF2TXT
{
    

    public partial class Form3 : Form
    {

        
        public Form3()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Form1 fm2 = (WindowsFormsApplication1.Form1)this.Owner;
            fm2.Text = this.textBox1.Text;
            this.Close();

        }
    }
}
