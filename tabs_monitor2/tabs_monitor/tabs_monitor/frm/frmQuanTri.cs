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
    public partial class frmQuanTri : Form
    {
        private static Random random = new Random();
        private string id = RandomString(6);
        public frmQuanTri()
        {
            InitializeComponent();
            this.textBox1.Text = id;
        }

        private void frmQuanTri_Load(object sender, EventArgs e)
        {
            
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clsUser u = new clsUser();
            u.User_id = id;
            u.Uname = textBox2.Text.ToString();
            u.Roles = comboBox1.SelectedItem.ToString();
            u.Pwd = textBox3.Text.ToString();
            u.themUser();
            this.Close();
            //Console.Out.WriteLine("{0},{1},{2},{3}", u.User_id, u.Uname, u.Roles, u.Pwd);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clsUser u = new clsUser();
            u.layDuLieuById(textBox4.Text);
            this.textBox1.Text = u.User_id;
            this.textBox2.Text = u.Uname;
            this.comboBox1.SelectedItem = u.Roles;
            this.textBox3.Text = u.Pwd;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clsUser u = new clsUser();
            u.User_id = textBox1.Text;
            u.Pwd = textBox3.Text;
            u.suaUser();
            MessageBox.Show("Sửa dữ liệu thành công");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            clsUser u = new clsUser();
            u.User_id = textBox1.Text;
            u.xoaUser();
            MessageBox.Show("Đã xoá dữ liệu");
        }
    }
}
