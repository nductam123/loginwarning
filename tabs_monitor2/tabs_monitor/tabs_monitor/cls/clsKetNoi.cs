using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tabs_monitor.cls
{
    internal class clsKetNoi
    {
        public string ChuoiKetNoi = "Data Source = LAPTOP-0QGJC7CL\\SQLEXPRESS" +
                                   ";database=monitor_log" +
                                   ";Integrated Security = True" +
                                   ";User Instance = False";
        public SqlConnection con = new SqlConnection();
        public bool ketNoi()
        {
            try
            {
                con = new SqlConnection(ChuoiKetNoi);
                con.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối trong class clsKetnoi" + ex.ToString());
            }
            return false;
        }
        public void dongketNoi()
        {
            con.Close();
        }
    }
}
