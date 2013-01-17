using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            Regex VITA＿FILE_NAME_MASK = new Regex("^[A-Z0-9\\-_]{1,8}(\\.[A-Z0-9\\-_]{1,3})?$");
            Match m = VITA＿FILE_NAME_MASK.Match(s);
            if (m.Success)
            {
                if (File.Exists(Application.StartupPath + "\\" + s) == false)
                {
                    File.Move(Application.StartupPath + "\\" + this.Text, Application.StartupPath + "\\" + s);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("same name exit\n同じ名前が存在します");
                }
            }
            else {
                MessageBox.Show("this name donot match VITA8.3 rule.\nVITA8.3ルール外です\n//for example\nABCD0123\tOK\nVITA.H\tOK\nABCD1234.ISO\tOK\n_123-\tOK\n123456789\tNG FILENAME WITHOUT EXTESION OVER 8 chars\n1.CONF\tNG EXTENSION OVER 3 chars\n.UNIX\tNG unixdotfile\nAVC..\tNG double dot");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
                        textBox1.Text = this.Text;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            Regex VITA＿FILE_NAME_MASK = new Regex("[^A-Za-z0-9\\-_\\.\b]");
            Match m = VITA＿FILE_NAME_MASK.Match(Convert.ToString(e.KeyChar));
            if (e.KeyChar == 0x2E) {
                if (textBox1.Text.Contains("."))
                {
                    MessageBox.Show("'.' alreay exist.\n'.'は１つしか使えません");
                    e.Handled = true;
                }
                else if (textBox1.SelectionStart == 0)
                {
                    MessageBox.Show("1st pos '.' is unix dotfile.unix dotfile ignored from vita CMA\nファイル名先頭が'.'の場合VITAでは認識されません");
                   e.Handled = true;
                }
            
            }
            else if (m.Success == true) {
                MessageBox.Show("//INPUT MASK,ONLY ACCEPT THESE CHARS、使用可能文字\n0123456789\nABCDEFGHIJKLMNOPQRSTUVWXYZ\n-_.", Convert.ToString(e.KeyChar) +" cannot use、使用不可");
                e.Handled = true;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
