using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;
using System.Collections;

namespace hotel.Admin
{
    public partial class ImportExportDetail : System.Web.UI.Page
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
                bindImportDetailsToListView("");
                loadDataStock(txtstock);
                loadDataStock(SearchStock);
                loadDataWareHouse(txtwh);
                loadDataWareHouse(SearchWarehouse);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataStock(DropDownList dropstock)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM stock WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Mở kết nối
                con.Open();

                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dropstock.DataSource = reader;
                    dropstock.DataTextField = "name";
                    dropstock.DataValueField = "id";
                    dropstock.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                dropstock.Items.Insert(0, new ListItem("Chọn Mặt Hàng", ""));
            }
        
        }
        public void loadDataWareHouse(DropDownList droie)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM warehouse WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);
                // Mở kết nối
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    droie.DataSource = reader;
                    droie.DataTextField = "name";
                    droie.DataValueField = "id";
                    droie.DataBind();
                }
                droie.Items.Insert(0, new ListItem("Chọn Kho", ""));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            if (txtstock.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn loại hàng";
                return;
            }
            if (txtinout.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn type";
                return;
            }
            if (txtwh.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn kho";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO import_export_detail (active, stock_id, stock_quantity, type,   created_by, created_when, last_update_by, last_update_when , warehouse_id ) VALUES (@active, @stock_id,@stock_quantity, @type,  @created_by, @created_when, @last_update_by, @last_update_when ,@warehouse_id)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@stock_id",  Convert.ToInt32(txtstock.SelectedValue));
            cmd.Parameters.AddWithValue("@stock_quantity", string.IsNullOrEmpty(txtstquantity.Text) ? (object)DBNull.Value :  Convert.ToInt32(txtstquantity.Text));
            cmd.Parameters.AddWithValue("@type", Convert.ToInt32(txtinout.SelectedValue));
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@warehouse_id", Convert.ToInt32(txtwh.SelectedValue));

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
            bindImportDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        //===== Method to bind employee records to ListView control.
        void bindImportDetailsToListView(string searchName)
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
                condition += "ied.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchStock.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "s.id = " + SearchStock.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchWarehouse.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "w.id = " + SearchWarehouse.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = " SELECT ied.id AS id, ied.active as active , ied.type AS type,ied.warehouse_id,  w.name AS w_name,  s.name AS s_name, s.description AS s_description,  ied.stock_quantity as quantity, ied.created_by, ied.created_when, ied.last_update_by, ied.last_update_when  FROM import_export_detail ied  join stock s on s.id = ied.stock_id join warehouse w on ied.warehouse_id = w.id  ";
            if (!String.IsNullOrEmpty(condition))
            {
                cmdText += " WHERE " + condition;
            }
            
            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //===== Execute Query and bind data to ListView.
            lstViewImportDetails.DataSource = cmd.ExecuteReader();
            lstViewImportDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtstock.SelectedIndex = 0;
            txtwh.SelectedIndex = 0;
            txtstquantity.Text = string.Empty;
            txtinout.SelectedIndex  = 0;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindImportDetailsToListView(searchName);
        }
       
        protected void lstViewImportDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindImportDetailToEdit(id);
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
                string cmdText = "DELETE FROM import_export_detail WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindImportDetailsToListView("");
                txtlableer.Text = "Xóa thành công";
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

        public void bindImportDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = " Sửa Danh Sách Chi Tiết Hàng Hóa";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM import_export_detail WHERE id=@id";
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
                txtstock.SelectedValue = dr["stock_id"].ToString();
                txtwh.SelectedValue = dr["warehouse_id"].ToString();
                txtstquantity.Text = dr["stock_quantity"].ToString();
                string typeValue = dr["type"].ToString();
                if (txtinout.Items.FindByValue(typeValue) != null)
                {
                    txtinout.SelectedValue = typeValue;
                }
                else
                {
                    // Xử lý khi giá trị type không tồn tại trong danh sách
                    txtinout.SelectedIndex = 0; // Hoặc bạn có thể đặt giá trị mặc định khác
                }
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
            if (txtinout.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn type";
                return;
            }
            if (txtstock.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn loại hàng";
                return;
            }
            if (txtwh.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn kho";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE import_export_detail SET active=@active, stock_id=@stock_id, stock_quantity=@stock_quantity ,  type=@type,   last_update_by=@last_update_by, last_update_when=@last_update_when , warehouse_id=@warehouse_id  WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@stock_id", Convert.ToInt32(txtstock.SelectedValue));
            cmd.Parameters.AddWithValue("@stock_quantity", string.IsNullOrEmpty(txtstquantity.Text) ? (object)DBNull.Value :  Convert.ToInt32(txtstquantity.Text));
            cmd.Parameters.AddWithValue("@type", Convert.ToInt32(txtinout.Text));
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@warehouse_id", Convert.ToInt32(txtwh.SelectedValue));
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
            bindImportDetailsToListView("");

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
            txtstock.SelectedIndex = 0;
            txtwh.SelectedIndex = 0;
            txtstquantity.Text = "";
            txtinout.SelectedIndex = 0;
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
        //    txth2.Text = "Thêm danh sách chi tiết hàng hóa ";
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