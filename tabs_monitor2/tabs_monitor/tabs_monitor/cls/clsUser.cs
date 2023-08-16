using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tabs_monitor.cls
{
    internal class clsUser
    {
        public string User_id;
        public string Roles;
        public string Uname;
        public string Pwd;

        public bool CheckLoginCredentials(string uname, string pwd)
        {
            clsKetNoi cn = new clsKetNoi();
            cn.ketNoi();
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM users WHERE uname=@Username AND pwd=@Password", cn.con);
            command.Parameters.AddWithValue("@Username", uname);
            command.Parameters.AddWithValue("@Password", pwd);
            try
            {
                // Execute the command and get the result
                int result = (int)command.ExecuteScalar();

                // Check if the user exists
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the connection to the database
                cn.dongketNoi();
            }
            return false;
        }

        public User layDulieuNguoiDung(string uname)
        {
            clsKetNoi cn = new clsKetNoi();
            cn.ketNoi();
            User u = new User();
            SqlCommand command = new SqlCommand("SELECT * FROM users WHERE uname=@Username", cn.con);
            command.Parameters.AddWithValue("@Username", uname);
            using (SqlDataReader oReader = command.ExecuteReader())
            {
                while (oReader.Read())
                {
                    this.User_id = oReader["user_id"].ToString();
                    this.Roles = oReader["roles"].ToString();
                }
                cn.dongketNoi();
            }
            return u;
        }
        public User layDuLieuById(string id)
        {
            User u = new User();
            try
            {
                clsKetNoi cn = new clsKetNoi();
                cn.ketNoi();
                SqlCommand command = new SqlCommand("SELECT * FROM users WHERE user_id=@id", cn.con);
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        this.User_id = oReader["user_id"].ToString();
                        this.Roles = oReader["roles"].ToString();
                        this.Uname = oReader["uname"].ToString();
                        this.Pwd = oReader["pwd"].ToString();
                    }
                    cn.dongketNoi();
                }
                return u;
            }
            catch
            {
                MessageBox.Show("Không tìm thấy người dùng trong csdl Users");
            }
            return u;

        }

        public void themUser()
        {
            clsKetNoi cn = new clsKetNoi();
            string sql = "INSERT INTO users(user_id, roles, uname, pwd)" +
                        "VALUES ('" + User_id + "', '" + Roles + "', '" + Uname + "', '" + Pwd + "')";
            try
            {
                cn.ketNoi();
                SqlCommand cmd = new SqlCommand(sql, cn.con);
                cmd.ExecuteNonQuery();
                cn.dongketNoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu Users không thành công");
            }
        }
        public void suaUser()
        {
            clsKetNoi cn = new clsKetNoi();
            string sql = "UPDATE users SET pwd='" + Pwd + "' WHERE user_id='" + User_id + "'";
            try
            {
                cn.ketNoi();
                SqlCommand cmd = new SqlCommand(sql, cn.con);
                cmd.ExecuteNonQuery();
                cn.dongketNoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa dữ liệu không thành công");
            }
        }
        public void xoaUser()
        {
            clsKetNoi cn = new clsKetNoi();
            string sql = "DELETE FROM users WHERE user_id='" + User_id + "'";
            try
            {
                cn.ketNoi();
                SqlCommand cmd = new SqlCommand(sql, cn.con);
                cmd.ExecuteNonQuery();
                cn.dongketNoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa dữ liệu không thành công");
            }
        }


    }
}
