using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Image = System.Web.UI.WebControls.Image;

namespace hotel.Admin
{
    public partial class CrudRoom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                bindRoomData("");
                loadData(txttyperoom);
                loadData(SearchTypeRoom);
                hfSearchNameClientID.Value = SearchName.ClientID;
                int id = Convert.ToInt32(Request.QueryString["id"]);
                bindRoomDetailToEdit(id);
            }
        }

        public void loadData(DropDownList dropDownList)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT id, name FROM room_type WHERE active = 1";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dropDownList.DataSource = reader;
                    dropDownList.DataTextField = "name";
                    dropDownList.DataValueField = "id";
                    dropDownList.DataBind();
                }
                dropDownList.Items.Insert(0, new ListItem("Chọn loại phòng", ""));
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
            if (txttyperoom.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn loại phòng";
                return;
            }

            int active = txtactive.Checked ? 1 : 0;
            int state = txtstt.Checked ? 1 : 0;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "INSERT INTO room (active, name, num_floor, spread, num_bed, description, state,quantity, type_room_id, created_by, created_when, last_update_by, last_update_when) " +
                             "VALUES (@active, @name, @num_floor, @spread, @num_bed, @description, @state,@quantity ,@type_room_id, @created_by, @created_when, @last_update_by, @last_update_when); " +
                             "SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            int numFloor;
            if (int.TryParse(txtnumfllor.Text, out numFloor))
            {
                cmd.Parameters.AddWithValue("@num_floor", numFloor);
            }
            else
            {
                cmd.Parameters.AddWithValue("@num_floor", DBNull.Value);
            }

            int spread;
            if (int.TryParse(txtspread.Text, out spread))
            {
                cmd.Parameters.AddWithValue("@spread", spread);
            }
            else
            {
                cmd.Parameters.AddWithValue("@spread", DBNull.Value);
            }

            int numBed;
            if (int.TryParse(txtnumbed.Text, out numBed))
            {
                cmd.Parameters.AddWithValue("@num_bed", numBed);
            }
            else
            {
                cmd.Parameters.AddWithValue("@num_bed", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtdescription.Text) ? (object)DBNull.Value : txtdescription.Text);

            cmd.Parameters.AddWithValue("@state", state);

            int quantity;
            if (int.TryParse(txtquantity.Text, out quantity))
            {
                cmd.Parameters.AddWithValue("@quantity", quantity);
            }
            else
            {
                cmd.Parameters.AddWithValue("@quantity", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@type_room_id", Convert.ToInt32(txttyperoom.SelectedValue));
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

            con.Open();
            int roomId = Convert.ToInt32(cmd.ExecuteScalar());

            // Handle File Upload for Images
            if (FileUploadImage.HasFile)
            {
                foreach (HttpPostedFile uploadedFile in FileUploadImage.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
                    if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                    {
                        string fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName) + fileExtension;
                        string filePath = Server.MapPath("~/UploadedImages/") + fileName;
                        uploadedFile.SaveAs(filePath);

                        string imageUrl = "~/UploadedImages/" + fileName;
                        SaveImageInfo(roomId, imageUrl);
                    }
                    else
                    {
                        txtlableer.Visible = true;
                        txtlableer.ForeColor = System.Drawing.Color.Red;
                        txtlableer.Text = "Chỉ cho phép tải lên các file JPG, JPEG, PNG hoặc GIF.";
                        return;
                    }
                }
            }

            con.Close();
            clearInputControls();
            bindRoomData("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }
        void clearInputControls()
        {
            txtactive.Checked = false;
            txtname.Text = string.Empty;
            txtnumfllor.Text = string.Empty;
            txtspread.Text = string.Empty;
            txtnumbed.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtstt.Checked = false;
            txttyperoom.SelectedIndex = 0;
        }

        void SaveImageInfo(int roomId, string imageUrl)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Kiểm tra sự tồn tại của roomId trong bảng room
                string checkRoomQuery = "SELECT COUNT(*) FROM room WHERE id = @roomId";
                SqlCommand checkRoomCmd = new SqlCommand(checkRoomQuery, con);
                checkRoomCmd.Parameters.AddWithValue("@roomId", roomId);

                con.Open();
                int roomCount = (int)checkRoomCmd.ExecuteScalar();

                if (roomCount > 0)
                {
                    // Nếu roomId tồn tại, thực hiện chèn hình ảnh
                    string query = "INSERT INTO document_information (active, created_by, created_when, last_update_by, last_update_when, type, URL, room_content) " +
                                   "VALUES (@active, @created_by, @created_when, @last_update_by, @last_update_when, 'room_image', @URL, @room_content)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@active", 1);
                    cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                    cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                    cmd.Parameters.AddWithValue("@URL", imageUrl);
                    cmd.Parameters.AddWithValue("@room_content", roomId);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    txtlableer.Visible = true;
                    txtlableer.Text = ("Room ID không tồn tại.");
                    RegisterHideLabelScript();
                }
            }
        }
        void bindRoomData(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "r.name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += " r.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchTypeRoom.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += " rt.id = " + SearchTypeRoom.SelectedValue;
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"SELECT r.id, r.active, r.name, r.num_bed, r.num_floor, r.spread, r.state, r.description, rt.name as typename, rt.id as idtype
                            FROM room r
                            JOIN room_type rt on rt.id = r.type_room_id";
                if (!String.IsNullOrEmpty(condition))
                {
                    cmdText += " where " + condition;
                }
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    lstViewRoomDetails.DataSource = cmd.ExecuteReader();
                    lstViewRoomDetails.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindRoomData(searchName);
        }
        protected void lstViewRoomDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Del":
                    deleteRoom(id);
                    break;
                case "Edt":
                    bindRoomDetailToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }
        void deleteRoom(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();

                    // Xóa các bản ghi liên quan trong bảng document_information
                    string deleteDocumentsCmdText = "DELETE FROM document_information WHERE room_content=@id";
                    using (SqlCommand deleteDocumentsCmd = new SqlCommand(deleteDocumentsCmdText, con))
                    {
                        deleteDocumentsCmd.Parameters.AddWithValue("@id", id);
                        deleteDocumentsCmd.ExecuteNonQuery();
                    }

                    // Xóa bản ghi trong bảng room
                    string deleteRoomCmdText = "DELETE FROM room WHERE id=@id";
                    using (SqlCommand deleteRoomCmd = new SqlCommand(deleteRoomCmdText, con))
                    {
                        deleteRoomCmd.Parameters.AddWithValue("@id", id);
                        deleteRoomCmd.ExecuteNonQuery();
                    }
                }

                bindRoomData("");  // Tải lại dữ liệu sau khi xóa
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
        void bindRoomDetailToEdit(int id)
        {
            imagePreview.Visible = false;
            imgProfile.Visible = true;
            newImg.Visible = true;
            FileUploadImage.Visible = false;
            txtlableer.Visible = false;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                // Lấy chi tiết phòng và các hình ảnh liên quan
                string query = @"
                    SELECT r.*, di.URL , di.activeImg
                    FROM room r
                    LEFT JOIN document_information di ON di.room_content = r.id
                    WHERE r.id = @id AND di.activeImg = 1";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<string> imageUrls = new List<string>();
                        bool isFirstRow = true;
                        while (reader.Read())
                        {
                            if (isFirstRow)
                            {
                                txtactive.Checked = Convert.ToBoolean(reader["active"]);
                                txtname.Text = reader["name"].ToString();
                                txtnumfllor.Text = reader["num_floor"].ToString();
                                txtspread.Text = reader["spread"].ToString();
                                txtnumbed.Text = reader["num_bed"].ToString();
                                txtquantity.Text = reader["quantity"].ToString();
                                txtdescription.Text = reader["description"].ToString();
                                txtstt.Checked = Convert.ToBoolean(reader["state"]);
                                txttyperoom.SelectedValue = reader["type_room_id"].ToString();
                                isFirstRow = false;
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("URL")))
                            {
                                imgProfile.Src = reader["URL"].ToString();
                            }
                            else
                            {
                                imgProfile.Src = "../App_Themes/Admin_Pages/Layout/assets/img/avatars/avt.png"; // Set a default or placeholder image if no image exists
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("URL")))
                            {
                                imageUrls.Add(reader["URL"].ToString());
                            }
                            hfSelectedRecord.Value = id.ToString();
                            btnUpdate.Visible = true;
                            btnSave.Visible = false;
                            btnCancel.Visible = true;
                        }
                    }

                }
                newImg.HRef = "imgRoom.aspx?id=" + id;
                con.Close();
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập tên";
                return;
            }
            if (txttyperoom.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn loại phòng";
                return;
            }

            int active = txtactive.Checked ? 1 : 0;
            int state = txtstt.Checked ? 1 : 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"UPDATE room 
                           SET active = @active, 
                               name = @name, 
                               num_floor = @num_floor, 
                               spread = @spread, 
                               num_bed = @num_bed, 
                               description = @description, 
                               state = @state, 
                               quantity = @quantity, 
                               type_room_id = @type_room_id, 
                               last_update_by = @last_update_by, 
                               last_update_when = @last_update_when 
                           WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));
                    cmd.Parameters.AddWithValue("@active", active);
                    cmd.Parameters.AddWithValue("@name", txtname.Text);

                    // Kiểm tra và thêm tham số num_floor
                    cmd.Parameters.AddWithValue("@num_floor", int.TryParse(txtnumfllor.Text, out int numFloor) ? (object)numFloor : DBNull.Value);
                    cmd.Parameters.AddWithValue("@spread", int.TryParse(txtspread.Text, out int spread) ? (object)spread : DBNull.Value);
                    cmd.Parameters.AddWithValue("@num_bed", int.TryParse(txtnumbed.Text, out int numBed) ? (object)numBed : DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtdescription.Text) ? (object)DBNull.Value : txtdescription.Text);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@quantity", int.TryParse(txtquantity.Text, out int quantity) ? (object)quantity : DBNull.Value);
                    cmd.Parameters.AddWithValue("@type_room_id", Convert.ToInt32(txttyperoom.SelectedValue));
                    cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        txtlableer.Text = "Không tìm thấy bản ghi để cập nhật";
                        txtlableer.Visible = true;
                        return;
                    }
                }
            }

            // Handle File Upload for Images
            if (FileUploadImage.HasFile)
            {
                foreach (HttpPostedFile uploadedFile in FileUploadImage.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
                    if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                    {
                        string fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName) + fileExtension;
                        string filePath = Server.MapPath("~/UploadedImages/") + fileName;
                        uploadedFile.SaveAs(filePath);

                        string imageUrl = "~/UploadedImages/" + fileName;
                        SaveImageInfo(Convert.ToInt32(hfSelectedRecord.Value), imageUrl); // Sử dụng id hiện tại để lưu thông tin ảnh
                    }
                    else
                    {
                        txtlableer.Visible = true;
                        txtlableer.ForeColor = System.Drawing.Color.Red;
                        txtlableer.Text = "Chỉ cho phép tải lên các file JPG, JPEG, PNG hoặc GIF.";
                        return;
                    }
                }
            }

            clearInputControls();
            bindRoomData("");
            txtlableer.Text = "Cập nhật thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }


        //void UpdateImageInfo(int roomId, string imageUrl)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        // Kiểm tra sự tồn tại của roomId trong bảng room
        //        string checkRoomQuery = "SELECT COUNT(*) FROM room WHERE id = @roomId";
        //        SqlCommand checkRoomCmd = new SqlCommand(checkRoomQuery, con);
        //        checkRoomCmd.Parameters.AddWithValue("@roomId", roomId);

        //        con.Open();
        //        int roomCount = (int)checkRoomCmd.ExecuteScalar();

        //        if (roomCount > 0)
        //        {
        //            // Nếu roomId tồn tại, thực hiện cập nhật hình ảnh
        //            string query = @"UPDATE document_information SET active = @active, last_update_by = @last_update_by, last_update_when = @last_update_when, URL = @URL 
        //                   WHERE room_content = @room_content";

        //            SqlCommand cmd = new SqlCommand(query, con);
        //            cmd.Parameters.AddWithValue("@active", 1);
        //            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
        //            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@URL", imageUrl);
        //            cmd.Parameters.AddWithValue("@room_content", roomId);

        //            cmd.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            txtlableer.Visible = true;
        //            txtlableer.Text = ("Room ID không tồn tại." + roomId);
        //            RegisterHideLabelScript();
        //        }
        //    }
        //}

        //bool ImageExists(int roomId, string imageUrl)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        string query = "SELECT COUNT(*) FROM document_information WHERE room_content = @roomId AND URL = @imageUrl AND type = 'room_image'";
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@roomId", roomId);
        //            cmd.Parameters.AddWithValue("@imageUrl", imageUrl);
        //            con.Open();
        //            int count = (int)cmd.ExecuteScalar();
        //            return count > 0;
        //        }
        //    }
        //}
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtactive.Checked = false;
            txtname.Text = "";
            txtnumfllor.Text = "";
            txtspread.Text = "";
            txtnumbed.Text = "";
            txtquantity.Text = "";
            txtdescription.Text = "";
            txtstt.Checked = false;
            txttyperoom.SelectedIndex = 0;
            form_data.Visible = false;
            list_data.Visible = true;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
        }
        void RegisterHideLabelScript()
        {
            string script = "window.setTimeout(function() { document.getElementById('" + txtlableer.ClientID + "').style.display = 'none'; }, 3000);";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            form_data.Visible = true;
            list_data.Visible = false;
            FileUploadImage.Visible = true;
            imagePreview.Visible = true;
            imgProfile.Visible = false;
            newImg.Visible = false;
        }
    }
}
