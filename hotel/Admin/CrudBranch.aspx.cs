using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;

namespace hotel.Admin
{
    public partial class CrudBranch : System.Web.UI.Page
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
                bindOrgstrucDetailsToListView("");
                loadDataMan(txtmanag) ;
                loadDataCom(txtbranch);
                loadDataCom(SearchBranch);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataMan(DropDownList dropmanna)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM orgstructure WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Mở kết nối
                con.Open();

                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dropmanna.DataSource = reader;
                    dropmanna.DataTextField = "name";
                    dropmanna.DataValueField = "id";
                    dropmanna.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                dropmanna.Items.Insert(0, new ListItem("Chọn Tạo Bởi Chi Nhánh", ""));
            }
        }
        public void loadDataCom(DropDownList drocompany)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM company WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Mở kết nối
                con.Open();

                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    drocompany.DataSource = reader;
                    drocompany.DataTextField = "name";
                    drocompany.DataValueField = "id";
                    drocompany.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                drocompany.Items.Insert(0, new ListItem("Chọn Công Ty", ""));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập tên";
                return;
            }
            if (string.IsNullOrEmpty(txtcode.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập code";
                return;
            }
            if (txtbranch.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn công ty";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO orgstructure (active, type, managed_by, code , name,   email, phone,  establish_date, description, branch_id, created_by, created_when, last_update_by, last_update_when) VALUES (@active, @type, @managed_by, @code , @name,   @email, @phone,  @establish_date, @description, @branch_id, @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@type", string.IsNullOrEmpty(txttype.Text) ? (object)DBNull.Value : txttype.Text);
            cmd.Parameters.AddWithValue("@managed_by", string.IsNullOrEmpty(txtmanag.SelectedValue) ? (object)DBNull.Value : Convert.ToInt32(txtmanag.SelectedValue));
            cmd.Parameters.AddWithValue("@code", txtcode.Text);
            cmd.Parameters.AddWithValue("@name",  txtname.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@establish_date", string.IsNullOrEmpty(txtesdate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtesdate.Text));
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtdescription.Text) ? (object)DBNull.Value : txtdescription.Text);
            cmd.Parameters.AddWithValue("@branch_id", Convert.ToInt32(txtbranch.SelectedValue));
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
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
            bindOrgstrucDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;

        }

        //===== Method to bind employee records to ListView control.
        void bindOrgstrucDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "active = " + SearchActiveDropdown.SelectedValue;
            }
            
            if (!String.IsNullOrEmpty(SearchBranch.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "namecompany = " + SearchBranch.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = "SELECT  * FROM View_Orgstructure_Company";
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
            lstViewOrgstrucDetails.DataSource = cmd.ExecuteReader();
            lstViewOrgstrucDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txttype.Text = string.Empty;
            txtmanag.SelectedIndex =0 ;
            txtcode.Text = string.Empty;
            txtname.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtesdate.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtbranch.SelectedIndex = 0;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindOrgstrucDetailsToListView(searchName);
        }
        
        protected void lstViewOrgstrucDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindOrgstrucDetailToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }

        void delete(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "DELETE FROM orgstructure WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindOrgstrucDetailsToListView("");
                txtlableer.Text = "xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();
            }
            catch(Exception ex) {
                string script = "alert('err');";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }
        }

        public void bindOrgstrucDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            txth2.Text = "Chỉnh Sửa Chi Nhánh";
            //btnback.Visible = true;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM orgstructure WHERE id=@id";
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
                txttype.Text = dr["type"].ToString();
                txtmanag.SelectedValue = dr["managed_by"].ToString();
                txtcode.Text = dr["code"].ToString();
                txtname.Text = dr["name"].ToString();
                txtemail.Text = dr["email"].ToString();
                txtphone.Text = dr["phone"].ToString();
                if (!dr.IsDBNull(dr.GetOrdinal("establish_date")))
                {
                    txtesdate.Text = Convert.ToDateTime(dr["establish_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtesdate.Text = "";
                }
                txtdescription.Text = dr["description"].ToString();
                txtbranch.SelectedValue = dr["branch_id"].ToString();
                
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
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập tên";
                return;
            }
            if (string.IsNullOrEmpty(txtcode.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập code";
                return;
            }
            if (txtbranch.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn công ty";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE orgstructure SET active=@active, type=@type, managed_by=@managed_by, code=@code, name=@name , email= @email, phone=@phone, establish_date=@establish_date, description=@description, branch_id= @branch_id,  last_update_by=@last_update_by, last_update_when=@last_update_when WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@type", string.IsNullOrEmpty(txttype.Text) ? (object)DBNull.Value : txttype.Text);
            cmd.Parameters.AddWithValue("@managed_by", string.IsNullOrEmpty(txtmanag.SelectedValue) ? (object)DBNull.Value : Convert.ToInt32(txtmanag.SelectedValue));
            cmd.Parameters.AddWithValue("@code", txtcode.Text);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@establish_date", string.IsNullOrEmpty(txtesdate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtesdate.Text));
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtdescription.Text) ? (object)DBNull.Value : txtdescription.Text);
            cmd.Parameters.AddWithValue("@branch_id", Convert.ToInt32(txtbranch.SelectedValue));
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
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
            bindOrgstrucDetailsToListView("");

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
            txttype.Text = "";
            txtmanag.SelectedIndex = 0;
            txtcode.Text = "";
            txtname.Text = "";
            txtemail.Text = "";
            txtphone.Text = "";
            txtesdate.Text = "";
            txtdescription.Text = "";
            txtbranch.SelectedIndex = 0;
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
        //    txth2.Text = "Thêm Chi Nhánh ";
        //    txtlableer.Visible= false;
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
