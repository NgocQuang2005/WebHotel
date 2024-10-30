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
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] == null)
            //{
            //    Response.Redirect("~/Website/Login.aspx");
            //}
            if (!Page.IsPostBack)
            {
                bindEmployeeDetailsToListView();
                bindBlogDetailsToListView();
                bindEventOtherToListView();
                bindReviewsOtherToListView();
            }
        }

        void bindEmployeeDetailsToListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT TOP 1 wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE category = 1 ";
            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            lstViewEmployeeDetails.DataSource = cmd.ExecuteReader();
            lstViewEmployeeDetails.DataBind();
        }
        void bindBlogDetailsToListView()
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT TOP 2 wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE category = 2 ";
            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            ListViewBlog.DataSource = cmd.ExecuteReader();
            ListViewBlog.DataBind();
        }
        void bindEventOtherToListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT TOP 3 wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category as category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE category = 3";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            ListViewOther.DataSource = cmd.ExecuteReader();
            ListViewOther.DataBind();
        }
        void bindReviewsOtherToListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = @"SELECT TOP 3 wc.id, wc.id_author,
                             CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active,  
                             wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword,
                             wc.category as category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when AS URL FROM website_content wc where keyword ='reviews'";



            SqlCommand cmd = new SqlCommand(cmdText, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            listviewreviews.DataSource = cmd.ExecuteReader();
            listviewreviews.DataBind();
        }
    }
}