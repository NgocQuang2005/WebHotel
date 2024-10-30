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
    public partial class EventContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"] == null)
            //{
            //    Response.Redirect("~/Website/Login.aspx");
            //}
            string idwc = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(idwc))
            {
                bindEventContentDetailsToListView(idwc);
            }
            bindEventOtherToListView();
        }

        void bindEventContentDetailsToListView(string idwc)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE wc.id = @idwc ;";
            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Parameters.AddWithValue("@idwc", idwc);

            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            ListViewEventContent.DataSource = ds.Tables[0];
            ListViewEventContent.DataBind();
        }
        void bindEventOtherToListView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            string cmdText = "SELECT wc.id, wc.id_author, e.name as name, " +
                             "CASE WHEN wc.active = 1 THEN 'Hoat dong' WHEN wc.active = 0 THEN 'Khong hoat dong' END AS active, " +
                             "wc.title as title, wc.content as content, wc.publish_date as date, wc.keyword as keyword, " +
                             "wc.category as category, wc.luotxem, wc.created_by, wc.created_when, wc.last_update_by, wc.last_update_when, di.URL AS URL " +
                             "FROM website_content wc " +
                             "LEFT JOIN document_information di on di.website_to = wc.id " +
                             "JOIN employee e on e.id = wc.id_author " +
                             "WHERE category = 'event'";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            ListViewOther.DataSource = cmd.ExecuteReader();
            ListViewOther.DataBind();
        }
    }
}