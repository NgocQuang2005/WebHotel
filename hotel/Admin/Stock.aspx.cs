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
using System.Collections;

namespace hotel.Admin
{
    public partial class CheckInventoryList : System.Web.UI.Page
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
                bindStockInDetailsToListView("");
                loadDataNhacungcap(txtncc);
                loadDataNhacungcap(SearchNcc);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataNhacungcap(DropDownList doromd)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM contact_management WHERE active = 1 AND type='Supplier' ";

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
                doromd.Items.Insert(0, new ListItem("Chọn Nhà Cung Cấp", ""));
            }
        }
         
        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor= System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tên Sản Phẩm";
                return;
            }
            if (txtncc.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Nhà Cung Cấp";
                return;
            }
            
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO stock (active, name , category , description,   import_price , export_price, created_by, created_when, last_update_by, last_update_when, supplier_id) VALUES (@active, @name , @category , @description,  @import_price , @export_price,  @created_by, @created_when, @last_update_by, @last_update_when, @supplier_id)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@name",txtname.Text);
            cmd.Parameters.AddWithValue("@category", string.IsNullOrEmpty(txtcategory.Text) ? (object)DBNull.Value : txtcategory.Text);
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtnote.Text) ? (object)DBNull.Value : txtnote.Text);
            cmd.Parameters.AddWithValue("@import_price", string.IsNullOrEmpty(txtnhap.Text) ? (object)DBNull.Value : float.Parse(txtnhap.Text));
            cmd.Parameters.AddWithValue("@export_price", string.IsNullOrEmpty(txtxuat.Text) ? (object)DBNull.Value : float.Parse(txtxuat.Text));
            cmd.Parameters.AddWithValue("@supplier_id", Convert.ToInt32(txtncc.SelectedValue));
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
            bindStockInDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.Visible = true;
            txtlableer.ForeColor = System.Drawing.Color.Green;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        //===== Method to bind employee records to ListView control.
        void bindStockInDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "s.name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "s.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchNcc.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "cm.id = " + SearchNcc.SelectedValue;
            }
           
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = @"SELECT s.id ,s.active as active , s.name as s_name , s.category as s_cate, s.description ,  s.import_price as gianhap, s.export_price as giaban, cm.name as tenncc , cm.id 
                                FROM stock s
                                JOIN contact_management cm ON cm.id = s.supplier_id ";

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
            lstViewStockInDetails.DataSource = cmd.ExecuteReader();
            lstViewStockInDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtname.Text = string.Empty;
            txtcategory.Text = string.Empty;
            txtnote.Text = string.Empty;
            txtnhap.Text = string.Empty;
            txtxuat.Text = string.Empty;
            txtncc.SelectedIndex = 0;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindStockInDetailsToListView(searchName);
        }
        
        protected void lstViewStockInDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindStockDetailToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }

        void delete(int id)
        {
            try {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "DELETE FROM stock WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindStockInDetailsToListView("");
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

        public void bindStockDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Chỉnh Sửa Sản Phẩm";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM stock WHERE id=@id";
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
                txtname.Text = dr["name"].ToString();
                txtcategory.Text = dr["category"].ToString();
                txtnote.Text = dr["description"].ToString();
                txtnhap.Text = dr["import_price"].ToString();
                txtxuat.Text = dr["export_price"].ToString();
                txtncc.SelectedValue = dr["supplier_id"].ToString();

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
                txtlableer.Text = "Vui Lòng Nhập Tên Sản Phẩm";
                return;
            }
            if (txtncc.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Nhà Cung Cấp";
                return;
            }

            int active = txtacti.Checked ? 1 : 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                // Mở kết nối
                con.Open();

                // Câu lệnh cập nhật
                string cmdText = "UPDATE stock SET active=@active, name=@name, category=@category, description=@description, " +
                                 "import_price=@import_price, export_price=@export_price, last_update_by=@last_update_by, " +
                                 "last_update_when=@last_update_when, supplier_id=@supplier_id WHERE id=@id";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    // Gán giá trị cho các tham số
                    cmd.Parameters.AddWithValue("@active", active);
                    cmd.Parameters.AddWithValue("@name", txtname.Text);
                    cmd.Parameters.AddWithValue("@category", string.IsNullOrEmpty(txtcategory.Text) ? (object)DBNull.Value : txtcategory.Text);
                    cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtnote.Text) ? (object)DBNull.Value : txtnote.Text);
                    cmd.Parameters.AddWithValue("@import_price", string.IsNullOrEmpty(txtnhap.Text) ? (object)DBNull.Value : float.Parse(txtnhap.Text));
                    cmd.Parameters.AddWithValue("@export_price", string.IsNullOrEmpty(txtxuat.Text) ? (object)DBNull.Value : float.Parse(txtxuat.Text));
                    cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                    cmd.Parameters.AddWithValue("@supplier_id", txtncc.SelectedValue);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));

                    // Thực thi lệnh cập nhật
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        txtlableer.Text = "Cập nhật thành công";
                        txtlableer.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        txtlableer.Text = "Không tìm thấy sản phẩm để cập nhật";
                        txtlableer.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

            // Cập nhật giao diện
            clearInputControls();
            bindStockInDetailsToListView("");
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
            hfSelectedRecord.Value = string.Empty;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtacti.Checked = false;
            txtname.Text = "";
            txtcategory.Text = "";
            txtnote.Text = "";
            txtnhap.Text = "";
            txtxuat.Text = "";
            txtncc.SelectedIndex = 0;
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
        //    txth2.Text = "Thêm Hàng Tồn Kho ";
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