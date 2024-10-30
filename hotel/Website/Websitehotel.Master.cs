using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel.Website
{
    public partial class Websitehotel : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    int customerId = Convert.ToInt32(Session["UserID"]);
                    LoadUserInfo(customerId); // Gọi hàm để load thông tin người dùng
                    account.Visible = true;
                    connectacc.Visible = false;
                    cdtk.Visible = true;
                    cshs.Visible = true;
                    dxuattk.Visible = true;
                }
                else
                {
                    account.Visible = false;
                    connectacc.Visible = true;
                    dnhap.Visible = true;
                    dxuat.Visible = true;
                    // Nếu không có UserID trong Session, có thể xử lý redirect về trang đăng nhập
                    //Response.Redirect("~/Login.aspx");
                }
                LoadCompanyInfo();
            }
            UpdateCartCount();
        }

        private void LoadUserInfo(int customerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string query = "SELECT cm.id as id, cm.email as email, cm.name AS name FROM contact_management cm " +
                               "WHERE cm.id = @customerId ";
                               
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@customerId", customerId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string email = reader["email"].ToString();
                   
                    litUserName.Text = name + " - <small class=\"text-muted\">" + email + "</small>";

                }
                reader.Close();
            }
        }
        protected string IsActive(string currentUrl, string targetUrl)
        {
            if (!string.IsNullOrEmpty(currentUrl) && currentUrl.EndsWith(targetUrl, StringComparison.OrdinalIgnoreCase))
            {
                return "active";
            }
            return "";
        }

        private void LoadCompanyInfo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT address, email, phone FROM company WHERE active = 1"; // Assuming 'active' indicates the active company
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string address = reader["address"].ToString();
                    string email = reader["email"].ToString();
                    string phone = reader["phone"].ToString();

                    litAddress.Text = $"<a href='contact.html'>{address}</a>";
                    litEmail.Text = $"<a href='mailto:{email}'>{email}</a>";
                    litPhone.Text = $"<a href='tel:{phone}'>{phone}</a>";
                }

                reader.Close();
            }
        }
        private void UpdateCartCount()
        {
            if (Session["CartCount"] != null)
            {
                litCartCount.Text = Session["CartCount"].ToString();
                txtcartmobi.Text = Session["CartCount"].ToString();
            }
            else
            {
                litCartCount.Text = "0";
                txtcartmobi.Text = "0";
            }
        }
        protected void btnlogout_Click(object sender, EventArgs e)
        {
            // Xóa các session và cookie (nếu có)
            Session.Clear();
            Session.Abandon();

            // Xóa cookie (nếu có)
            if (Request.Cookies["YourCookieName"] != null)
            {
                var c = new HttpCookie("YourCookieName");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            // Chuyển hướng về trang đăng nhập
            Response.Redirect("~/Website/Home.aspx");
        }
    }
}