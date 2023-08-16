using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tabs_monitor.cls;

namespace tabs_monitor.frm
{
    public partial class frmDangNhap : Form
    {
        string uname;
        string pwd;

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start();
        }

        private void start()
        {
            uname = this.textBox1.Text;
            pwd = this.textBox2.Text;
            dangNhap();
        }

        private void dangNhap()
        {
            clsUser nd = new clsUser();
            bool kq = nd.CheckLoginCredentials(uname, pwd);
            if (kq)
            {
                this.Hide();
                frmMainForm mf = new frmMainForm();
                mf.uname = uname;
                mf.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password!");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                start();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                start();
            }
        }
    }
}
