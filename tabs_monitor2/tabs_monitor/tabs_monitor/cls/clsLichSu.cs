using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace tabs_monitor.cls
{
    internal class clsLichSu
    {
        public string Log_id = "";
        public string User_id = "";
        public string Url = "";
        public string Title = "";
        public string Violation = "";
        public string Session_time = "";
        public string Created_at = "";

        public DataTable layDuLieuLichSu(string sql)
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

        public void themLichSu()
        {
            clsKetNoi cn = new clsKetNoi();
            string sql = "INSERT INTO history(log_id, user_id, urls, title, violation, session_time, created_at)" +
                        "VALUES ('" + Log_id + "', '" + User_id + "', '" + Url + "', '"+ Title + "', '"+Violation + "', '" + Session_time + "', '" + DateTime.Now.ToString() + "')";
            try
            {
                cn.ketNoi();
                SqlCommand cmd = new SqlCommand(sql, cn.con);
                cmd.ExecuteNonQuery();
                cn.dongketNoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Thêm dữ liệu History không thành công");
            }

        }



    }
}
