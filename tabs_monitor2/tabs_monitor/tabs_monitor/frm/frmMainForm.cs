using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using tabs_monitor.cls;
using System.Linq;
using System.Collections.Generic;
using tabs_monitor.frm;

namespace tabs_monitor
{
    public partial class frmMainForm : Form
    {
        Timer timer1 = new Timer();
        Timer timer2 = new Timer();
        private string user_id;
        private string log_id = RandomString(6);
        private string title;
        public string uname;
        public string roles;
        private static Random random = new Random();

        public frmMainForm()
        {
            InitializeComponent();
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            getUserData();
            if (roles == "admin" || roles == "Admin")
            {
                button1.Enabled = true;
            }

            clearLog();
            getTabsInfo();
            this.textBox1.Text = user_id;
            this.textBox2.Text = roles;
            this.textBox3.Text = uname;
            InitTimer1s();
            InitTimer1m();
        }
        public void InitTimer1s()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 2000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            taiDuLieu();
            getTabsInfo();
        }
        public void InitTimer1m()
        {
            timer2 = new Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 31012; // in miliseconds
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            logSorting();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private void taiDuLieu()
        {
            string sql = "SELECT * FROM history";
            string sql1 = "SELECT * FROM violations";
            clsLichSu ls = new clsLichSu();
            clsTicket tk = new clsTicket();
            dgvLichSu.DataSource = ls.layDuLieuLichSu(sql);
            dvgTicket.DataSource = tk.layDuLieuTicket(sql1);
        }

        private void getUserData()
        {
            clsUser u = new clsUser();
            u.layDulieuNguoiDung(uname);
            user_id = u.User_id;
            roles = u.Roles;
        }

        private void getTabsInfo()
        {
            List<string> browsers = new List<string>() { "brave", "msedge" };

            foreach (var item in browsers)
            {
                Process[] procs = Process.GetProcessesByName(item);
                foreach (Process p in procs)
                {
                    // the chrome process must have a window
                    if (p.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }

                    // find the automation element
                    AutomationElement elm = AutomationElement.FromHandle(p.MainWindowHandle);
                    AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));

                    // if it can be found, get the value from the URL bar
                    if (elmUrlBar != null)
                    {
                        AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                        if (patterns.Length > 0)
                        {
                            ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                            string url = val.Current.Value;
                            if (url != "")
                            {
                                string tit = p.MainWindowTitle;
                                int indexOfSteam = tit.IndexOf("and");
                                if (indexOfSteam >= 0)
                                    tit = tit.Remove(indexOfSteam);
                                title = tit;
                                DateTime now = DateTime.Now;
                                textBox4.Text = tit;
                                textBox5.Text = now.ToString();
                                textBox6.Text = url;
                                logging(user_id + "," + log_id + "," + now.ToString() + "," + url);
                            }
                            else
                            {
                                DateTime now = DateTime.Now;
                                textBox5.Text = now.ToString();
                                textBox6.Text = "empty tab";
                                logging(user_id + "," + log_id + "," + now.ToString() + "," + "empty_tab");
                            }
                        }
                    }
                }
            }

        }

        private void judge(double min, string url)
        {
            if (min >= 0.5)
            {
                log_id = RandomString(6);
                mainFormThemLichSu(url, min, 1);
                mailing(title, url, min);
                mainFormThemTicket(min);
            }
            else if (min > 0.2 && min < 0.5)
            {
                log_id = RandomString(6);
                mainFormThemLichSu(url, min, 0);
            }
        }

        private void logSorting()
        {
            List<clsLichSu> lls = new List<clsLichSu>();
            using (var reader = new StreamReader(@"C:\Users\Admin\Desktop\log.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    lls.Add(new clsLichSu()
                    {
                        User_id = values[0],
                        Log_id = values[1],
                        Session_time = values[2],
                        Url = values[3]
                    });
                }
            }
            List<clsLichSu> lsSortedList = lls.OrderBy(o => o.Url).ToList();
           
            var duplicates = lsSortedList.GroupBy(x => x.Url)
                                    .Where(g => g.Count() > 1)
                                    .Select(x => x.Key);

            DateTime startTime;
            DateTime endTime;
            TimeSpan spent = TimeSpan.Zero;
            foreach (string l in duplicates)
            {
                for (int i = 0; i < lsSortedList.Count - 1; i++)
                {
                    if (l == lsSortedList[i].Url)
                    {
                        startTime = DateTime.Parse(lsSortedList[i].Session_time);
                        endTime = DateTime.Parse(lsSortedList[i + 1].Session_time);
                        spent += endTime.Subtract(startTime);
                    }
                }
                //Console.Out.WriteLine("Spent total {0} on {1}", spent.TotalMinutes.ToString("n2"), l);
                judge(Math.Abs(spent.TotalMinutes), l);
            }
        }

        private void logging(string logValues)
        {
            string filePath = "C:\\Users\\Admin\\Desktop\\log.csv";
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                //write to the file
                sw.WriteLine(logValues);
            }
        }

        private void clearLog()
        {
            File.WriteAllText(@"C:\Users\Admin\Desktop\log.csv", string.Empty);
        }

        private void mainFormThemLichSu(string url, double min, int violation)
        {
            clsLichSu ls = new clsLichSu();
            ls.Log_id = log_id;
            ls.User_id = user_id;
            ls.Url = url;
            ls.Title = title;
            ls.Violation = violation.ToString();
            ls.Session_time = min.ToString("n2");
            ls.themLichSu();
        }

        private void mainFormThemTicket(double min)
        {
            clsTicket ls = new clsTicket();
            ls.Ticket_id = RandomString(6);
            ls.Log_id = log_id;
            ls.Session_time = min.ToString("n2");
            ls.themTicket();
        }

        private void mailing(string title, string url, double min)
        {
            try 
            {
                //Gửi email thông báo khi lưu SQL thành công
                string subject = "Thông báo truy cập vượt quá thời gian cho phép";
                string body = "Người dùng "+ uname + " đã truy cập vào" + title + " " + min + " phút" + "\nĐịa chỉ: " + url;

                //Thay đổi thông tin email và password của người gửi
                string fromEmail = "nductam311@gmail.com";
                string fromPassword = "dnzphilqnyibfbtm";

                //Thay đổi thông tin email của người nhận
                string toEmail = "only.vayne03@gmail.com";

                MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(fromEmail, fromPassword);

                client.Send(message);
            } catch 
            {
                MessageBox.Show("Gửi email thông báo không thành công. Kiểm tra kết nối hoặc cài đặt");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm.frmQuanTri qt = new frmQuanTri();
            qt.ShowDialog();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
