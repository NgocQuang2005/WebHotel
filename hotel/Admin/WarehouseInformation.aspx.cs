using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace hotel.Admin
{
    public partial class WarehouseInformation : System.Web.UI.Page
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
                bindWarehouseDetailsToListView("");
                loadDataCompany(txtcompany);
                loadDataWareEmployee(txtemploy);
                loadDataCompany(SearchCompany);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataCompany(DropDownList doromd)
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
                    doromd.DataSource = reader;
                    doromd.DataTextField = "name";
                    doromd.DataValueField = "id";
                    doromd.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                doromd.Items.Insert(0, new ListItem("Chọn Công Ty", ""));
            }
        }
        public void loadDataWareEmployee(DropDownList droie)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM employee WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Mở kết nối
                con.Open();

                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    droie.DataSource = reader;
                    droie.DataTextField = "name";
                    droie.DataValueField = "id";
                    droie.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                droie.Items.Insert(0, new ListItem("Chọn Nhân Viên Tạo", ""));
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
            if (txtcompany.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn công ty";
                return;
            }
            if (txtemploy.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn nhân viên";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO warehouse (active, name, description, branch_id,  created_by, created_when, last_update_by, last_update_when , manager_id) VALUES (@active, @name, @description, @branch_id,   @created_by, @created_when, @last_update_by, @last_update_when, @manager_id)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtnote.Text) ? (object)DBNull.Value : txtnote.Text);
            cmd.Parameters.AddWithValue("@branch_id",  Convert.ToInt32(txtcompany.SelectedValue));
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by",  Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@manager_id", Convert.ToInt32(txtemploy.SelectedValue));
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
            bindWarehouseDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            list_data.Visible = true;
            form_data.Visible = false;
        }

        //===== Method to bind employee records to ListView control.
        void bindWarehouseDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "w.name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "w.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchCompany.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "c.id = " + SearchCompany.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = @" SELECT  w.id as id , w.active as active ,c.name as tencongty, e.name as tennhanvien , 
                                w.name as name, w.description as description , w.branch_id, w.created_by,
                                w.created_when, w.last_update_by, w.last_update_when, w.manager_id FROM warehouse w
                                JOIN company c on c.id = w.branch_id
                                JOIN employee e on e.id = w.manager_id";
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
            lstViewWarehouseDetails.DataSource = cmd.ExecuteReader();
            lstViewWarehouseDetails.DataBind();

        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtname.Text = string.Empty;
            txtnote.Text = string.Empty;
            txtcompany.SelectedIndex = 0;
            txtemploy.SelectedIndex = 0;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindWarehouseDetailsToListView(searchName);
        }
        
        protected void lstViewWarehouseDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindWarehouseDetailToEdit(id);
                    list_data.Visible = false;
                    form_data.Visible = true;
                    break;
            }
        }

        void delete(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "DELETE FROM warehouse WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindWarehouseDetailsToListView("");
                txtlableer.Text = "Xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();
            }
            catch (Exception ex)
            {
                string script = "alert('err');";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }
            

        }

        public void bindWarehouseDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Sửa Thông Tin Kho";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM warehouse WHERE id=@id";
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
                txtacti.Checked = Convert.ToBoolean(dr["active"]);
                txtname.Text = dr["name"].ToString();
                txtnote.Text = dr["description"].ToString();
                txtcompany.SelectedValue = dr["branch_id"].ToString();
                txtemploy.SelectedValue = dr["manager_id"].ToString();

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
            if (txtcompany.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn công ty";
                return;
            }
            if (txtemploy.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn nhân viên";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE warehouse SET active=@active, name=@name, description=@description, branch_id=@branch_id,   last_update_by=@last_update_by, last_update_when=@last_update_when , manager_id=@manager_id WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtnote.Text) ? (object)DBNull.Value : txtnote.Text);
            cmd.Parameters.AddWithValue("@branch_id", Convert.ToInt32(txtcompany.SelectedValue));
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@manager_id", Convert.ToInt32(txtemploy.SelectedValue));
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
            bindWarehouseDetailsToListView("");

            //===== Show Save button and hide update button.
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
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
            txtname.Text = "";
            txtnote.Text = "";
            txtcompany.SelectedIndex = 0;
            txtemploy.SelectedIndex = 0;
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
        //    txth2.Text = "Thêm Kho Lưu Trữ ";
        //    txtlableer.Visible = false;
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