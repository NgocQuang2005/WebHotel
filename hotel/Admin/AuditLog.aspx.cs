using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Configuration;

namespace hotel.Admin
{
    public partial class AuditLog : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                //===== To bind employee's records from database.
                bindAuditLogView("");

                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            
            if (string.IsNullOrEmpty(txttablename.Text))
            {
                txtlableer.Visible = true; ;
                txtlableer.Text = "Vui lòng nhập table_name";
                return;
            }
            if (string.IsNullOrEmpty(txtoperation.Text))
            {
                txtlableer.Visible = true; ;
                txtlableer.Text = "Vui lòng nhập operation";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            int webclick = 0;
            if (txtwebclick.Checked)
            {
                webclick = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO audit_log (active,  jsondata,  created_by, created_when, last_update_by,table_name , operation , website_click , last_update_when) VALUES (@active,  @jsondata,  @created_by, @created_when, @last_update_by,@table_name , @operation , @website_click , @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@jsondata", string.IsNullOrEmpty(txtjsondata.Text) ? (object)DBNull.Value : txtjsondata.Text);
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when",  DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@table_name", txttablename.Text);
            cmd.Parameters.AddWithValue("@operation", txtoperation.Text);
            cmd.Parameters.AddWithValue("@website_click", webclick);
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //===== Execute Query.
            cmd.ExecuteNonQuery();

            //===== close the connection.
            con.Close();

            //===== Clear text from textboxes
            clearInputControls();

            //===== Bind data to ListView.
            bindAuditLogView("");
            txtlableer.Text = "Thêm Mới Thành Công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        //===== Method to bind employee records to ListView control.
        void bindAuditLogView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "operation LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "active = " + SearchActiveDropdown.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = "SELECT id , active ,  jsondata , created_by , created_when , last_update_by , table_name, operation,  website_click, last_update_when FROM audit_log";
            if (!String.IsNullOrEmpty(condition))
            {
                cmdText += " where " + condition;
            }

            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //===== Execute Query and bind data to ListView.
            lstViewAuditDetails.DataSource = cmd.ExecuteReader();
            lstViewAuditDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        { 
            txtacti.Checked = false;
            txtjsondata.Text = string.Empty;
            txttablename.Text = string.Empty;
            txtoperation.Text = string.Empty;
            txtwebclick.Checked = false;
            
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindAuditLogView(searchName);
        }
        
        protected void lstViewAuditDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    deleteAudit(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindAuditLogEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }

        void deleteAudit(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "DELETE FROM audit_log WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindAuditLogView("");
                txtlableer.Text = "Xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();
            }
            catch(Exception ex) {
                string script = "alert('err');";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }
            
        }

        public void bindAuditLogEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Sửa Thông Audit_log";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM audit_log WHERE id=@id";
            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Parameters.AddWithValue("@id", id);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataReader dr = cmd.ExecuteReader();

            //=== Read Data.
            if (dr.HasRows)
            {
                
                dr.Read();
                txtacti.Checked = Convert.ToBoolean( dr["active"]);
                txtjsondata.Text = dr["jsondata"].ToString();
                txttablename.Text = dr["table_name"].ToString();
                txtoperation.Text = dr["operation"].ToString();
                txtwebclick.Checked = Convert.ToBoolean(dr["website_click"]);

                // Store the employee ID in a hidden field
                hfSelectedRecord.Value = id.ToString();

                // Make update button visible and save button invisible
                btnUpdate.Visible = true;
                btnSave.Visible = false;
                btnCancel.Visible = true;
            }

            //===== close the connection.
            con.Close();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            
            if (string.IsNullOrEmpty(txttablename.Text))
            {
                txtlableer.Visible = true; ;
                txtlableer.Text = "Vui lòng nhập table_name";
                return;
            }
            if (string.IsNullOrEmpty(txtoperation.Text))
            {
                txtlableer.Visible = true; ;
                txtlableer.Text = "Vui lòng nhập operation";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            int webclick = 0;
            if (txtwebclick.Checked)
            {
                webclick = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE audit_log SET active=@active,  jsondata =@jsondata,  last_update_by=@last_update_by, table_name= @table_name , operation = @operation, website_click= @website_click, last_update_when=@last_update_when WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active",active);
            cmd.Parameters.AddWithValue("@jsondata", string.IsNullOrEmpty(txtjsondata.Text) ? (object)DBNull.Value : txtjsondata.Text);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@table_name", txttablename.Text);
            cmd.Parameters.AddWithValue("@operation", txtoperation.Text);
            cmd.Parameters.AddWithValue("@website_click", webclick);
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //===== Execute Query.
            cmd.ExecuteNonQuery();

            //===== close the connection.
            con.Close();

            //===== Clear text from textboxes
            clearInputControls();

            //===== Bind data to listview.
            bindAuditLogView("");

            //===== Show Save button and hide update button.
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
            //===== Clear Hiddenfield
            hfSelectedRecord.Value = string.Empty;
            txtlableer.Text = "Update thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtacti.Checked = false;
            txtjsondata.Text = "";
            txttablename.Text = "";
            txtoperation.Text = "";
            txtwebclick.Checked = false;
            form_data.Visible = false;
            list_data.Visible = true;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
        }
        //protected void btnback_Click(object sender, EventArgs e)
        //{
        //    clearInputControls();
        //    btnSave.Visible = true;
        //    btnCancel.Visible = true;
        //    btnUpdate.Visible = false;
        //    btnback.Visible = false;
        //    txth2.Text = "Thêm Mới Audit_log ";
        //    txtlableer.Visible=false;
        //    form_data.Visible = false;
        //    list_data.Visible = true;
        //}
        private void RegisterHideLabelScript()
        {
            string script = "hideLabelAfterTimeout();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            form_data.Visible = true;
            list_data.Visible = false;
        }
    }
}