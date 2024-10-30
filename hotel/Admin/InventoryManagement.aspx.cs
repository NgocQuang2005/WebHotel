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
    public partial class InventoryManagement : System.Web.UI.Page
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
                bindInventoryDetailsToListView("");
                loadDataStock(txtstock);
                loadDataWareHouse(txtwh);
                loadDataStock(SearchStock);
                loadDataWareHouse(SearchWarehouse);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataStock(DropDownList doromd)
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
                    doromd.DataSource = reader;
                    doromd.DataTextField = "name";
                    doromd.DataValueField = "id";
                    doromd.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                doromd.Items.Insert(0, new ListItem("Chọn loại Hàng", ""));
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
            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập type";
                return;
            }
            if (string.IsNullOrEmpty(txtquantity.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập số lượng";
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

            //======= Insert Query.
            string cmdText = "INSERT INTO stock_inventory (active, stock_id, warehouse_id, quantity, type,  created_by, created_when, last_update_by, last_update_when) VALUES (@active, @stock_id, @warehouse_id, @quantity, @type, @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@stock_id", Convert.ToInt32(txtstock.SelectedValue));
            cmd.Parameters.AddWithValue("@warehouse_id", Convert.ToInt32(txtwh.SelectedValue));
            cmd.Parameters.AddWithValue("@quantity", Convert.ToDouble(txtquantity.Text));
            cmd.Parameters.AddWithValue("@type", txttype.Text);
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
            bindInventoryDetailsToListView("");
            txtlableer.Text = "Thêm mới thành công";
            txtlableer.Visible = true;
            txtlableer.ForeColor = System.Drawing.Color.Green;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;

        }

        void bindInventoryDetailsToListView(string searchName)
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
                condition += "si.active = " + SearchActiveDropdown.SelectedValue;
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
            string cmdText = @" SELECT si.id as id , si.active as active , si.stock_id , si.warehouse_id, si.quantity as quantity , si.type as type , s.id , s.name as tenhang , w.id , w.name as tenkho
                                FROM stock_inventory si 
                                JOIN stock s ON s.id = si.stock_id
                                JOIN warehouse w ON w.id = si.warehouse_id ";
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
            lstViewInventoryDetails.DataSource = cmd.ExecuteReader();
            lstViewInventoryDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtstock.SelectedIndex = 0;
            txtwh.SelectedIndex = 0;
            txtquantity.Text = string.Empty;
            txttype.Text = string.Empty;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindInventoryDetailsToListView(searchName);
        }
        
        protected void lstViewInventoryDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindInventoryDetailToEdit(id);
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
                string cmdText = "DELETE FROM stock_inventory WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindInventoryDetailsToListView("");
                txtlableer.Text = "xóa thành công";
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

        public void bindInventoryDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Chỉnh Sửa Chi Tiết Hoạt Động Của Hàng";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM stock_inventory WHERE id=@id";
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
                txtstock.SelectedValue = dr["stock_id"].ToString();
                txtwh.SelectedValue = dr["warehouse_id"].ToString();
                txtquantity.Text = dr["quantity"].ToString();
                txttype.Text = dr["type"].ToString();
                
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
            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập type";
                return;
            }
            if (string.IsNullOrEmpty(txtquantity.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập số lượng";
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
            string cmdText = "UPDATE stock_inventory SET active=@active, stock_id=@stock_id, warehouse_id=@warehouse_id, quantity=@quantity, type=@type,  last_update_by=@last_update_by, last_update_when=@last_update_when WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@stock_id", Convert.ToInt32(txtstock.SelectedValue));
            cmd.Parameters.AddWithValue("@warehouse_id", Convert.ToInt32(txtwh.SelectedValue));
            cmd.Parameters.AddWithValue("@quantity", Convert.ToDouble(txtquantity.Text));
            cmd.Parameters.AddWithValue("@type", txttype.Text);
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
            bindInventoryDetailsToListView("");
            //===== Show Save button and hide update button.
            btnSave.Visible = false;
            btnUpdate.Visible = true;
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
            txtquantity.Text = "";
            txttype.Text ="";
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