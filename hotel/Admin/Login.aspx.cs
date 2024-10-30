using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Demo1.Admin
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] == "logout")
                {
                    // Xóa Session và chuyển hướng về trang Login
                    Session.Abandon();
                    Response.Redirect("Login.aspx");
                }
                ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
        }
        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string hashedPassword = GetMd5Hash(password);
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT  employee_id FROM account 
                    WHERE username = @username AND password = @password AND active = 1 AND NOT role = 3";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    try
                    {
                        if (result != null)
                        {
                            int employeeId = Convert.ToInt32(result);

                            // Lấy thông tin từ account
                            string accountQuery = @"
                            SELECT id, username,  password, role FROM account 
                            WHERE employee_id = @employeeId AND NOT role = 3";
                            using (SqlCommand accountCmd = new SqlCommand(accountQuery, con))
                            {
                                accountCmd.Parameters.AddWithValue("@employeeId", employeeId);
                                using (SqlDataReader reader = accountCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int accountId = Convert.ToInt32(reader["id"]);
                                        string user = reader["username"].ToString();
                                        string pass = reader["password"].ToString();
                                        string role = reader["role"].ToString();

                                        // Lưu thông tin người dùng vào session
                                        
                                        Session["UserID"] = employeeId;
                                        Session["AccountId"] = accountId;
                                        Session["Username"] = user;
                                        Session["Password"] = pass;
                                        Session["Role"] = role;
                                    }
                                }
                            }

                            // Cập nhật thời gian đăng nhập cuối cùng
                            string updateQuery = @"
                            UPDATE account 
                            SET last_login_time = @lastLoginTime 
                            WHERE employee_id = @employeeId";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                            {
                                updateCmd.Parameters.AddWithValue("@lastLoginTime", DateTime.Now);
                                updateCmd.Parameters.AddWithValue("@employeeId", employeeId);
                                updateCmd.ExecuteNonQuery();
                            }
                            // Thiết lập session và chuyển hướng đến trang chủ

                            Response.Redirect("Home.aspx");
                        }
                        else
                        {
                            litMessage.Text = "Sai username hoặc password!";
                        }
                    }
                    catch (Exception ex)
                    {
                        string script = ex.Message;
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
                    }
                }
            }
        }
    }
}
