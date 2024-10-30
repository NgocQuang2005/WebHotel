using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace hotel.Admin
{
    public partial class AccountSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    txtusername.Text = Session["Username"].ToString();
                    txtpassword.Text = Session["Password"].ToString();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng UserName";
                return;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng PassWord";
                return;
            }
            if (Session["UserID"] != null)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                string username = txtusername.Text.Trim();
                string password = txtpassword.Text.Trim();

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string updateQuery = @"
                        UPDATE account 
                        SET  username = @Username, password = @Password, last_update_when = @LastUpdateWhen 
                        WHERE employee_id = @EmployeeId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@LastUpdateWhen", DateTime.Now);
                        cmd.Parameters.AddWithValue("@EmployeeId", userId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật thông tin trong session
                    Session["Username"] = username;
                    Session["Password"] = password;
                    txtlableer.Visible = true;
                    txtlableer.ForeColor = System.Drawing.Color.Green;
                    txtlableer.Text = "Thông tin tài khoản đã được cập nhật thành công!";
                    RegisterHideLabelScript();
                }
            }
        }
        private void RegisterHideLabelScript()
        {
            string script = "hideLabelAfterTimeout();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }
    }
}