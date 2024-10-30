using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel.Website
{
    public partial class Blog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindEmployeeDetailsToListView();
            }
        }

        void bindEmployeeDetailsToListView(string keyword = null)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category AS category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author  " +
                             "WHERE category = 2";

            if (!string.IsNullOrEmpty(keyword))
            {
                cmdText += " AND wc.keyword = @keyword ";
            }

            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (!string.IsNullOrEmpty(keyword))
            {
                cmd.Parameters.AddWithValue("@keyword", keyword);
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            lstViewEmployeeDetails.DataSource = cmd.ExecuteReader();
            lstViewEmployeeDetails.DataBind();
        }

        protected void FilterByKeyword(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            string keyword = btn.CommandArgument;
            bindEmployeeDetailsToListView(keyword);
        }
    }
}
