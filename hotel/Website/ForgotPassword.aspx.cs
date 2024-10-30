using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace hotel.Website
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private static string verificationCode;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSendCode_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (IsValidEmail(email))
            {
                if (IsEmailRegistered(email))
                {
                    // Tạo mã xác minh
                    verificationCode = GenerateVerificationCode();

                    // Gửi mã đến email của người dùng
                    if (SendVerificationCode(email, verificationCode))
                    {
                        lblMessage.Text = "Mã xác minh đã được gửi đến gmail của bạn.";
                        pnlEmail.Visible = false;
                        pnlCode.Visible = true;
                    }
                    else
                    {
                        //lblMessage.Text = "Không gửi đươc mã xác mình .Vui lòng thử lại!";
                    }
                }
                else
                {
                    lblMessage.Text = "Email chưa được đăng ký ở website.";
                }
            }
            else
            {
                lblMessage.Text = "Vui lòng nhập email hợp lệ.";
            }
        }

        protected void btnVerifyCode_Click(object sender, EventArgs e)
        {
            string code = txtCode.Text.Trim();
            if (code == verificationCode)
            {
                Response.Redirect("/AccountSettings.aspx");
            }
            else
            {
                lblMessage.Text = "Mã xác minh không hợp lệ.";
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsEmailRegistered(string email)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM contact_management WHERE email = @Email"; // Adjust this query as needed
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                return false;
            }
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool SendVerificationCode(string email, string code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("quang111420@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Password Reset Verification Code";
                mail.Body = "Your verification code is: " + code;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential("quang111420@gmail.com", "Quang20092005:");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Không thể gửi được mã : ," + ex.Message;
                return false;
            }
        }
    }
}
