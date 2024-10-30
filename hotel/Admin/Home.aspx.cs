using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace hotel.Admin
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                BindLastLoggedInUsers();
                int employeeId = Convert.ToInt32(Session["UserID"]);
                viewname(employeeId); // Gọi hàm để load thông tin người dùng
            }
        }

        private void BindLastLoggedInUsers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    WITH RankedDocumentInfo AS (
                    SELECT 
                        di.URL,
                        di.belong_to,
                        ROW_NUMBER() OVER (PARTITION BY di.belong_to ORDER BY di.created_when DESC) AS RowNum
                    FROM document_information di )
                    SELECT TOP 4 
                        a.username AS FullName,
                        a.role AS Role,
                        a.last_login_time AS LastLoginTime,
                        rdi.URL AS URL
                    FROM account a
                    JOIN employee e ON a.employee_id = e.id
                    LEFT JOIN RankedDocumentInfo rdi ON rdi.belong_to = e.id AND rdi.RowNum = 1
                    WHERE a.active = 1 AND a.last_login_time IS NOT NULL
                    ORDER BY a.last_login_time DESC;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    rptLastLoggedInUsers.DataSource = reader;
                    rptLastLoggedInUsers.DataBind();
                }
            }
        }
        private void viewname(int employeeId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string query = "SELECT e.id as id, e.name AS name  FROM employee e WHERE e.id = @employeeId ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@employeeId", employeeId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader["name"].ToString();
                    litUserName.Text = name ;
                }
                reader.Close();
            }

        }
    }
}
