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

namespace hotel.Admin
{
    public partial class ManageWebsiteContent : System.Web.UI.Page
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
                bindWebDetailsToListView("");
                loadData(txtauthoremploy);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }

        public void loadData(DropDownList dropDownList)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Truy vấn để chọn các loại phòng đang hoạt động
            string query = "SELECT id, name  FROM employee WHERE active = 1";

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
            if (string.IsNullOrEmpty(txttitle.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tiêu Đề";
                return;
            }
            if (string.IsNullOrEmpty(txtcontent.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Nội Dung";
                return;
            }
            if (string.IsNullOrEmpty(txtpdate.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Ngày Đăng";
                return;
            }
            if (txtauthoremploy.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Nhân Viên Đăng Bài";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO website_content (id_author, active, title, content, publish_date, keyword, category, luotxem, created_by, created_when, last_update_by, last_update_when) VALUES (@id_author, @active, @title, @content, @publish_date, @keyword, @category, @luotxem, @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@id_author", txtauthoremploy.SelectedValue);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@title", txttitle.Text);
            cmd.Parameters.AddWithValue("@content", txtcontent.Text);
            cmd.Parameters.AddWithValue("@publish_date",  Convert.ToDateTime(txtpdate.Text));
            cmd.Parameters.AddWithValue("@keyword", string.IsNullOrEmpty(txtkeyword.Text) ? (object)DBNull.Value : txtkeyword.Text);
            cmd.Parameters.AddWithValue("@category", string.IsNullOrEmpty(txtcd.SelectedValue) ? (object)DBNull.Value : txtcd.SelectedValue);
            cmd.Parameters.AddWithValue("@luotxem", 1);
            cmd.Parameters.AddWithValue("@created_by",  Convert.ToInt32(Session["UserID"]));
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
            bindWebDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        //===== Method to bind employee records to ListView control.
        void bindWebDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "wc.category LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "wc.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchChude.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "wc.category = " + SearchChude.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = "SELECT wc.id ,wc.id_author  , wc.active as active ,wc.title ,wc.content ,wc.publish_date ,wc.keyword, e.name as namenv ,wc.category ,wc.luotxem ,wc.created_by ,wc.created_when ,wc.last_update_by ,wc.last_update_when, di.URL AS url FROM website_content wc left join document_information di on di.website_to = wc.id JOIN employee e on e.id = wc.id_author ";
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
            lstViewWebDetails.DataSource = cmd.ExecuteReader();
            lstViewWebDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtauthoremploy.SelectedIndex = 0;
            txtacti.Checked = false;
            txttitle.Text = string.Empty;
            txtcontent.Text = string.Empty;
            txtpdate.Text = string.Empty;
            txtkeyword.Text = string.Empty;
            txtcd.SelectedIndex = 0;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindWebDetailsToListView(searchName);
        }
       
        protected void lstViewWebDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindWebDetailToEdit(id);
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
                string cmdText = "DELETE FROM website_content WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindWebDetailsToListView("");
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

        public void bindWebDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Sửa Nội Dung WebSite";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM website_content WHERE id=@id";
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
                txtauthoremploy.SelectedValue = dr["id_author"].ToString();
                txtacti.Checked = Convert.ToBoolean( dr["active"]);
                txttitle.Text = dr["title"].ToString();
                txtcontent.Text = dr["content"].ToString();
                if (!dr.IsDBNull(dr.GetOrdinal("publish_date")))
                {
                    txtpdate.Text = Convert.ToDateTime(dr["publish_date"]).ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    txtpdate.Text = "";
                }
                txtkeyword.Text = dr["keyword"].ToString();
                string categoryValue = dr["category"].ToString();
                if (txtcd.Items.FindByValue(categoryValue) != null)
                {
                    txtcd.SelectedValue = categoryValue;
                }
                else
                {
                    // Xử lý khi giá trị gender không tồn tại trong danh sách
                    txtcd.SelectedIndex = 0; // Hoặc bạn có thể đặt giá trị mặc định khác
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
            if (string.IsNullOrEmpty(txttitle.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tiêu Đề";
                return;
            }
            if (string.IsNullOrEmpty(txtcontent.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Nội Dung";
                return;
            }
            if (txtauthoremploy.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Nhân Viên Đăng Bài";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE website_content SET id_author=@id_author, active=@active, title=@title , content=@content , publish_date =@publish_date, keyword=@keyword, category=@category , luotxem=@luotxem, last_update_by=@last_update_by, last_update_when=@last_update_when WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            cmd.Parameters.AddWithValue("@id_author", txtauthoremploy.SelectedValue);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@title", txttitle.Text);
            cmd.Parameters.AddWithValue("@content", txtcontent.Text);
            cmd.Parameters.AddWithValue("@publish_date", Convert.ToDateTime(txtpdate.Text));
            cmd.Parameters.AddWithValue("@keyword", string.IsNullOrEmpty(txtkeyword.Text) ? (object)DBNull.Value : txtkeyword.Text);
            cmd.Parameters.AddWithValue("@category", string.IsNullOrEmpty(txtcd.SelectedValue) ? (object)DBNull.Value : txtcd.SelectedValue);
            cmd.Parameters.AddWithValue("@luotxem", 1);
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
            bindWebDetailsToListView("");

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
            txtauthoremploy.SelectedIndex = 0;
            txtacti.Checked = false;
            txttitle.Text = "";
            txtcontent.Text = "";
            txtpdate.Text = "";
            txtkeyword.Text = "";
            txtcd.SelectedIndex = 0;
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
        //    txth2.Text = "Thêm Nội Dung Website ";
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