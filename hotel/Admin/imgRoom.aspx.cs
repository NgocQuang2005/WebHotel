using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using hotel.Website;

namespace hotel.Admin
{
    public partial class imgRoom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                string roomId = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(roomId))
                {
                    // Gọi hàm để bind dữ liệu vào ListView dựa trên roomId
                    bindDocumentDetailsToListView(roomId, "");
                }
                loadDataRoom(txtroom);
                loadDataRoom(SearchRoom);
                
                //Search

                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }

        
        public void loadDataRoom(DropDownList droproom)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT id  , name FROM room WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    droproom.DataSource = reader;
                    droproom.DataTextField = "name";
                    droproom.DataValueField = "id";
                    droproom.DataBind();
                }
                droproom.Items.Insert(0, new ListItem("Chọn phòng", ""));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;

            int active = txtacti.Checked ? 1 : 0;
            int activeImg = txtatvImg.Checked ? 1 : 0;

            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập type";
                return;
            }

            if (!FileUploadImage.HasFiles)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn ít nhất một ảnh.";
                return;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            con.Open();
            int roomId = string.IsNullOrEmpty(txtroom.SelectedValue) ? 0 : Convert.ToInt32(txtroom.SelectedValue);
            foreach (HttpPostedFile uploadedFile in FileUploadImage.PostedFiles)
            {
                string fileUrl = UploadFile(uploadedFile);  // Gọi phương thức UploadFile với từng file được chọn

                string cmdText = "INSERT INTO document_information (active, created_by, created_when, last_update_by, last_update_when, type, URL, room_content, activeImg) " +
                                 "VALUES (@active, @created_by, @created_when, @last_update_by, @last_update_when, @type, @URL,  @room_content, @activeImg)";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@active", active);
                    cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                    cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                    cmd.Parameters.AddWithValue("@type", txttype.Text);
                    cmd.Parameters.AddWithValue("@URL", string.IsNullOrEmpty(fileUrl) ? (object)DBNull.Value : fileUrl);
                    cmd.Parameters.AddWithValue("@room_content", roomId == 0 ? (object)DBNull.Value : roomId);
                    cmd.Parameters.AddWithValue("@activeImg", activeImg);

                    cmd.ExecuteNonQuery();
                }
            }

            con.Close();

            clearInputControls();
            bindDocumentDetailsToListView(roomId.ToString(), "");
            txtlableer.Text = "Thêm mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();

            form_data.Visible = false;
            list_data.Visible = true;
        }


        void bindDocumentDetailsToListView(string roomId, string searchName)
        {

            string condition = "";
            if (!String.IsNullOrEmpty(roomId))
            {
                condition += "r.id = " + roomId;
            }
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "di.type LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "di.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchRoom.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += " r.id = " + SearchRoom.SelectedValue;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = @"SELECT di.id as iddi, r.id as idroom , di.active as active , di.belong_to ,di.created_by , di.created_when , di.id , di.last_update_by , di.last_update_when , di.room_content ,  di.type, di.URL, di.website_to
                                , r.name as tenphong , di.activeImg AS activeImg
                                FROM   document_information di 
                                RIGHT JOIN room  r on r.id = di.room_content
                                ";

            if (!String.IsNullOrEmpty(condition))
            {
                cmdText += " where " + condition;
            }
            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            lstViewDocumentDetails.DataSource = cmd.ExecuteReader();
            lstViewDocumentDetails.DataBind();
        }

        void clearInputControls()
        {
            txtacti.Checked = false;
            txttype.Text = string.Empty;
            txtroom.SelectedIndex = 0;
            txtatvImg.Checked = false;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindDocumentDetailsToListView( "",searchName);

        }

        protected void lstViewDocumentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindDocumentDetailToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }

        void delete(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    string cmdText = "DELETE FROM document_information WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                bindDocumentDetailsToListView("", ""); // Làm mới danh sách mà không cần điều kiện
                txtlableer.Text = "Xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();
            }
            catch (Exception ex)
            {
                string script = $"alert('{ex.Message}');"; // Hiển thị thông báo lỗi dưới dạng cảnh báo JavaScript
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }
        }


        public void bindDocumentDetailToEdit(int id)
        {
            try
            {
                txtlableer.Visible = false;
                txth2.Text = "Sửa Thông Tin Ảnh Phòng";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "SELECT * FROM document_information WHERE id=@id";
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
                    txttype.Text = dr["type"].ToString();
                    txtroom.SelectedValue = dr["room_content"].ToString();
                    if (dr["activeImg"] != DBNull.Value)
                    {
                        txtatvImg.Checked = Convert.ToBoolean(dr["activeImg"]);
                    }
                    else
                    {
                        txtatvImg.Checked = false;
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("URL")))
                    {
                        imgProfile.Src = dr["URL"].ToString();
                    }
                    else
                    {
                        imgProfile.Src = "../App_Themes/Admin_Pages/Layout/assets/img/avatars/avt.png"; // Set a default or placeholder image if no image exists
                    }
                    hfSelectedRecord.Value = id.ToString();
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                txtlableer.Visible = true;
                txtlableer.Text = ex.Message;
            }
        }
        private string UploadFile(HttpPostedFile file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string folderPath = Server.MapPath("~/UploadedImages/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, fileName);
                    file.SaveAs(filePath);
                    return "~/UploadedImages/" + fileName;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error uploading file: " + ex.Message);
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Có lỗi xảy ra khi tải lên tệp: " + ex.Message;
                    lblErrorMessage.ForeColor = System.Drawing.Color.Red;
                    return null;
                }
            }
            return null;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                txtlableer.Visible = false;
                txtlableer.ForeColor = System.Drawing.Color.Red;

                int active = txtacti.Checked ? 1 : 0;
                int activeImg = txtatvImg.Checked ? 1 : 0;

                if (string.IsNullOrEmpty(txttype.Text))
                {
                    txtlableer.Visible = true;
                    txtlableer.Text = "Vui lòng nhập type";
                    return;
                }

                string fileUrl = null;
                if (FileUploadImage.HasFile)
                {
                    fileUrl = UploadFile(FileUploadImage.PostedFile);  // Gọi phương thức UploadFile nếu có tệp được chọn
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    string cmdText = "UPDATE document_information SET active=@active, last_update_by=@last_update_by, last_update_when=@last_update_when, type=@type,  room_content=@room_content, activeImg=@activeImg";

                    if (fileUrl != null)
                    {
                        cmdText += ", URL=@URL";
                    }

                    cmdText += " WHERE id=@id";

                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.Parameters.AddWithValue("@active", active);
                        cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
                        cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                        cmd.Parameters.AddWithValue("@type", txttype.Text);
                        cmd.Parameters.AddWithValue("@room_content", string.IsNullOrEmpty(txtroom.SelectedValue) ? (object)DBNull.Value : Convert.ToInt32(txtroom.SelectedValue));
                        cmd.Parameters.AddWithValue("@activeImg", activeImg);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));

                        if (fileUrl != null)
                        {
                            cmd.Parameters.AddWithValue("@URL", fileUrl);
                        }

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    clearInputControls();
                    bindDocumentDetailsToListView(txtroom.SelectedValue, "");
                    btnSave.Visible = true;
                    btnUpdate.Visible = false;
                    btnCancel.Visible = true;
                    hfSelectedRecord.Value = string.Empty;
                    txtlableer.Text = "Cập nhật thành công";
                    txtlableer.ForeColor = System.Drawing.Color.Green;
                    txtlableer.Visible = true;
                    RegisterHideLabelScript();
                    form_data.Visible = false;
                    list_data.Visible = true;
                }
            }
            catch (Exception ex)
            {
                txtlableer.Visible = true;
                txtlableer.Text = ex.Message;
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtacti.Checked = false;
            txttype.Text = "";
            txtroom.SelectedIndex = 0;
            txtatvImg.Checked = false;
            form_data.Visible = false;
            list_data.Visible = true;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
        }
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
            imgProfile.Src = "~/App_Themes/Admin_Pages/Layout/assets/img/avatars/avt.png";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
        }
    }
}