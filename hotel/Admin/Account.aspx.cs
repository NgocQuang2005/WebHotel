using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;


namespace hotel.Admin
{
    public partial class EmployeeAccount : System.Web.UI.Page
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
                bindAcountDetailsToListView("");
                loadData(txtemployee);
                loadData(SearchEmployee);
                loadDataCustomer(txtcustomer);
                loadDataCustomer(SearchCustomer);
                hfSearchNameClientID.Value = SearchName.ClientID;
               
            }
        }
        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public void loadDataCustomer(DropDownList dropcustomer)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM contact_management WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Mở kết nối
                con.Open();

                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dropcustomer.DataSource = reader;
                    dropcustomer.DataTextField = "name";
                    dropcustomer.DataValueField = "id";
                    dropcustomer.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                dropcustomer.Items.Insert(0, new ListItem("Chọn Khách Hàng", ""));
            }
        }
        public void loadData(DropDownList dropDownList)
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
                    dropDownList.DataSource = reader;
                    dropDownList.DataTextField = "name";
                    dropDownList.DataValueField = "id";
                    dropDownList.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                dropDownList.Items.Insert(0, new ListItem("Chọn Nhân Viên", ""));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(txttendangnhap.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập username";
                return;
            }
            string errorMessage;
            if (!IsPasswordValid(txtmatkhau.Text, out errorMessage))
            {
                lblMatKhauError.Visible = true;
                lblMatKhauError.Text = errorMessage;
                return;
            }
            if(txtcustomer.SelectedIndex == 0 && txtemployee.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn nhân viên hoặc khách hàng";
                return;
            }
            if (txtvaitro.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn vai trò ";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            string hashedPassword = GetMd5Hash(txtmatkhau.Text);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO account ( username, password, role, active,  created_by, created_when, last_update_by, last_update_when, employee_id , custom_id) VALUES ( @username, @password, @role, @active,  @created_by, @created_when, @last_update_by, @last_update_when, @employee_id, @custom_id)";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            
            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@username", txttendangnhap.Text);
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@role",  txtvaitro.SelectedValue);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@created_by",  Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@employee_id", string.IsNullOrEmpty(txtemployee.SelectedValue) ? (object)DBNull.Value : txtemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@custom_id", string.IsNullOrEmpty(txtcustomer.SelectedValue) ? (object)DBNull.Value : txtcustomer.SelectedValue);
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
            bindAcountDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        //===== Method to bind employee records to ListView control.
        void bindAcountDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "e.name LIKE '%" + searchName + "%'" + " OR cm.name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "a.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchEmployee.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and "; 
                }
                condition += "e.id = " + SearchEmployee.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchCustomer.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "cm.id = " + SearchCustomer.SelectedValue;
            }
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = "SELECT a.id ,a.username ,a.password ,a.role ,a.active as active ,cm.id , cm.name as tenkhachhang, a.created_by, e.id as idnv ,a.created_when ,a.last_update_by ,a.last_update_when ,a.employee_id,  a.custom_id ,e.name as name FROM account a left join employee e on e.id = a.employee_id left join contact_management cm on cm.id = a.custom_id";

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
            lstViewAccountDetails.DataSource = cmd.ExecuteReader();
            lstViewAccountDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txttendangnhap.Text = string.Empty;
            txtmatkhau.Text = string.Empty;
            txtvaitro.SelectedIndex = 0;
            txtacti.Checked = false;
            txtemployee.SelectedIndex = 0;
            txtcustomer.SelectedIndex = 0;
        }

        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindAcountDetailsToListView(searchName);
        }
        

        protected void lstViewAccountDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindAccountDetailToEdit(id);
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
                string cmdText = "DELETE FROM account WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindAcountDetailsToListView("");
                txtlableer.Text = "xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();

            }
            catch (Exception ex)
            {
                string script = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }

        }

        public void bindAccountDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Chỉnh sửa tài khoản";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM account WHERE id=@id";
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
                txttendangnhap.Text = dr["username"].ToString();
                if (!dr.IsDBNull(dr.GetOrdinal("password")))
                {
                    txtmatkhau.Text =dr["password"].ToString();
                }
                else
                {
                    txtmatkhau.Text = "";
                }
                string genderValue = dr["role"].ToString();
                if (txtvaitro.Items.FindByValue(genderValue) != null)
                {
                    txtvaitro.SelectedValue = genderValue;
                }
                else
                {
                    // Xử lý khi giá trị gender không tồn tại trong danh sách
                    txtvaitro.SelectedIndex = 0; // Hoặc bạn có thể đặt giá trị mặc định khác
                }
                txtacti.Checked = Convert.ToBoolean( dr["active"]);
                txtemployee.SelectedValue = dr["employee_id"].ToString();
                txtcustomer.SelectedValue = dr["custom_id"].ToString();

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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txttendangnhap.Text = "";
            txtmatkhau.Text = "";
            txtvaitro.SelectedIndex = 0;
            txtacti.Checked = false;
            txtemployee.SelectedIndex = 0;
            txtcustomer.SelectedIndex = 0;
            form_data.Visible = false;
            list_data.Visible = true;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
        }
        private bool IsPasswordValid(string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(password))
            {
                errorMessage = "Vui lòng nhập mật khẩu";
                return false;
            }
            if (password.Length < 8)
            {
                errorMessage = "Mật khẩu phải có ít nhất 8 ký tự";
                return false;
            }
            if (!password.Any(char.IsUpper))
            {
                errorMessage = "Mật khẩu phải có ít nhất một chữ cái viết hoa";
                return false;
            }
            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Mật khẩu phải có ít nhất một số";
                return false;
            }
            return true;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(txttendangnhap.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập username";
                return;
            }
            string errorMessage;
            if (!IsPasswordValid(txtmatkhau.Text, out errorMessage))
            {
                lblMatKhauError.Visible = true;
                lblMatKhauError.Text = errorMessage;
                return;
            }
            if (txtcustomer.SelectedIndex == 0 && txtemployee.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn nhân viên hoặc khách hàng";
                return;
            }
            if (txtvaitro.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn vai trò ";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            string hashedPassword = GetMd5Hash(txtmatkhau.Text);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE account SET username=@username, password= @password, role=@role, active=@active,   last_update_by=@last_update_by, last_update_when=@last_update_when, employee_id= @employee_id , custom_id= @custom_id WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@username", txttendangnhap.Text);
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@role",  txtvaitro.SelectedValue);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@employee_id", string.IsNullOrEmpty(txtemployee.SelectedValue) ? (object)DBNull.Value : txtemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@custom_id", string.IsNullOrEmpty(txtcustomer.SelectedValue) ? (object)DBNull.Value : txtcustomer.SelectedValue);
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
            bindAcountDetailsToListView("");

            //===== Show Save button and hide update button.
            //btnSave.Visible = true;
            //btnUpdate.Visible = false;
            //btnCancel.Visible = false;
            //===== Clear Hiddenfield
            hfSelectedRecord.Value = string.Empty;
            txtlableer.Text = "Update thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }
        //protected void btnback_Click(object sender, EventArgs e)
        //{
        //    clearInputControls();
        //    btnSave.Visible = true;
        //    btnCancel.Visible = true;
        //    btnUpdate.Visible = false;
        //    btnback.Visible = false;
        //    txth2.Text = "Thêm Tài Khoản Nhân Viên ";
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
            txttendangnhap.Text = "";
            txtmatkhau.Text = "";
            txtlableer.Visible = false;
            form_data.Visible = true;
            list_data.Visible = false;
        }
    }
}