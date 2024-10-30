using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace hotel.Website
{
    public partial class EditProfile : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] == null)
            //{
            //    Response.Redirect("~/Admin/Login.aspx");
            //}

            if (!IsPostBack)
            {
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            if (Session["UserID"] != null)
            {
                int userId = (int)Session["UserID"];
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT name, email, phone, address, birthday FROM contact_management WHERE id = @userId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtname.Text = reader["name"].ToString();
                                txtemail.Text = reader["email"].ToString();
                                txtphone.Text = reader["phone"].ToString();
                                txtaddress.Text = reader["address"].ToString();
                                if (!reader.IsDBNull(reader.GetOrdinal("birthday")))
                                {
                                    txtbirthday.Text = Convert.ToDateTime(reader["birthday"]).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    txtbirthday.Text = "";
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tên";
                return;
            }
            int userId = (int)Session["UserID"];
            string name = txtname.Text.Trim();
            string email = txtemail.Text.Trim();
            string phone = txtphone.Text.Trim();
            string address = txtaddress.Text.Trim();
            
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE contact_management SET name = @name, email = @email, phone = @phone, address = @address, birthday = @birthday , last_update_when=@last_update_when WHERE id = @userId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtbirthday.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtbirthday.Text));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            
            txtlableer.Visible = true;
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Text = "Thông Tin Được Cập Nhật Thành Công";
            RegisterHideLabelScript();
        }

       
        private void RegisterHideLabelScript()
        {
            string script = "hideLabelAfterTimeout();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }
    }
}
