using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Windows.Forms;

namespace tabs_monitor.cls
{
    internal class clsTicket
    {
        public string Ticket_id;
        public string Log_id;
        public string Session_time;
        public string Created_at;

        public DataTable layDuLieuTicket(string sql)
        {
            clsKetNoi cn = new clsKetNoi();
            DataSet ds = new DataSet();
            try
            {
                cn.ketNoi();
                SqlDataAdapter dta = new SqlDataAdapter(sql, cn.con);
                dta.Fill(ds, sql);
                DataTable tbl = ds.Tables[0];
                return tbl;
            }
            catch
            {
                return null;
            }
        }

        public void themTicket()
        {
            clsKetNoi cn = new clsKetNoi();
            string sql = "INSERT INTO violations(ticket_id, log_id, session_time, created_at)" +
                        "VALUES ('" + Ticket_id + "', '" + Log_id + "', '" + Session_time + "', '" + DateTime.Now.ToString() + "')";
            try
            {
                cn.ketNoi();
                SqlCommand cmd = new SqlCommand(sql, cn.con);
                cmd.ExecuteNonQuery();
                cn.dongketNoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu Ticket không thành công\n{0}", ex.ToString());
            }

        }
    }
}
