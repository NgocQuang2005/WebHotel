using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace hotel.Website
{
    public partial class Event : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] == null)
            //{
            //    Response.Redirect("~/Website/Login.aspx");
            //}
            if (!Page.IsPostBack)
            {
                bindEventHpToListView();
                bindEventUpToListView();
            }
        }

        void bindEventHpToListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE keyword = 'happening'";


            SqlCommand cmd = new SqlCommand(cmdText, con);


            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            ListViewHappening.DataSource = cmd.ExecuteReader();
            ListViewHappening.DataBind();
        }
        void bindEventUpToListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE keyword = 'upcoming'";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            ListViewUpcoming.DataSource = cmd.ExecuteReader();
            ListViewUpcoming.DataBind();
        }
        protected string TruncateContent(string content, int wordLimit)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            var words = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length <= wordLimit)
                return content;

            string truncated = string.Join(" ", words.Take(wordLimit)) + "...";

            System.Diagnostics.Debug.WriteLine($"Original Content: {content}");
            System.Diagnostics.Debug.WriteLine($"Truncated Content: {truncated}");

            return truncated;
        }
    }
}