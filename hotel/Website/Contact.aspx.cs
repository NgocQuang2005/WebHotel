using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel.Website
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] == null)
            //{
            //    Response.Redirect("~/Website/Login.aspx");
            //}
            BinContactListView();
        }
        void BinContactListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT address ,email ,phone FROM company";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            ListViewContact.DataSource = cmd.ExecuteReader();
            ListViewContact.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            if (Session["UserId"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Bạn Cần Đăng Nhập Trước Khi Gửi Lời Nhắn!');", true);//gọi trình duyệt alert
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (string.IsNullOrEmpty(txtMessage.Text))
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Vui lòng nhập nội  dung";
                        return;
                    }
                    string query = @"UPDATE contact_management SET id=id,  active=@active,description=@description, last_update_when=@last_update_when";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["UserID"]));
                        cmd.Parameters.AddWithValue("@description", txtMessage.Text);
                        cmd.Parameters.AddWithValue("@active", 1);
                        cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            lblStatus.Text = "Tin nhắn của bạn đã được gởi đi thành công!";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            txtMessage.Text = string.Empty;
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Error: " + ex.Message;
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtMessage.Text = "";
        }

    }
}
