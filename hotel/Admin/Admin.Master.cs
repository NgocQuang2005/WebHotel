using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

namespace hotel.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Kiểm tra xem có UserID trong Session hay không
                if (Session["UserID"] != null)
                {
                    int employeeId = Convert.ToInt32(Session["UserID"]);
                    LoadUserInfo(employeeId); // Gọi hàm để load thông tin người dùng
                }
                else
                {
                    // Nếu không có UserID trong Session, có thể xử lý redirect về trang đăng nhập
                    Response.Redirect("~/Login.aspx");
                }
            }
            LoadEmployee();
            LoadBranch();
            LoadRoom();
        }

        private void LoadUserInfo(int employeeId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string query = "SELECT e.id as id, e.email as email, e.name AS name, di.URL as URL FROM employee e " +
                               "LEFT JOIN document_information di ON di.belong_to = e.id " +
                               "WHERE e.id = @employeeId " +
                               "ORDER BY di.created_when  DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@employeeId", employeeId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string email = reader["email"].ToString();
                    string imageUrl = reader["URL"].ToString();

                    imgUser.ImageUrl = ResolveUrl("") + imageUrl;
                    Image1.ImageUrl = ResolveUrl("") + imageUrl;
                    litUserName.Text = name + " - <small class=\"text-muted\">" + email + "</small>";

                }
                reader.Close();
            }
        }
        private void LoadEmployee()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(id) FROM employee";
                SqlCommand command = new SqlCommand(query, connection);

                // ExecuteScalar returns the first column of the first row in the result set
                int employeeCount = (int)command.ExecuteScalar();

                // Set the text of txtemp to the employee count
                txtemp.Text = employeeCount.ToString();
            }
        }
        private void LoadRoom()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(id) FROM room";
                SqlCommand command = new SqlCommand(query, connection);

                // ExecuteScalar returns the first column of the first row in the result set
                int RoomCount = (int)command.ExecuteScalar();

                // Set the text of txtemp to the employee count
                txtroom.Text = RoomCount.ToString();
            }
        }
        protected string IsActive(string currentUrl, string targetUrl)
        {
            // Kiểm tra xem currentUrl có bao gồm targetUrl hay không
            if (!string.IsNullOrEmpty(currentUrl) && currentUrl.EndsWith(targetUrl, StringComparison.OrdinalIgnoreCase))
            {
                return "active";
            }

            return "";
        }
        protected string IsOpen(string currentUrl, params string[] targetUrls)
        {
            if (!string.IsNullOrEmpty(currentUrl))
            {
                foreach (string targetUrl in targetUrls)
                {
                    if (currentUrl.EndsWith(targetUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        return "open active ";
                    }
                }
            }
            return "";
        }
        private void LoadBranch()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(id) FROM orgstructure";
                SqlCommand command = new SqlCommand(query, connection);

                // ExecuteScalar returns the first column of the first row in the result set
                int BranchCount = (int)command.ExecuteScalar();

                // Set the text of txtemp to the employee count
                txtbranch.Text = BranchCount.ToString();
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
            Response.Redirect("~/Admin/Login.aspx");
        }


    }
}