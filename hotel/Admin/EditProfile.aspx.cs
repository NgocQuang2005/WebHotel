using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace hotel.Admin
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected HtmlImage imgProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadUserInfo();
                imgProfile.Src = GetProfileImageUrl();
            }
        }

        private void LoadUserInfo()
        {
            if (Session["UserID"] != null)
            {
                int userId = (int)Session["UserID"];
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT name, email, phone, address, city, birthday FROM employee WHERE id = @userId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtname.Text = reader["name"].ToString();
                                txtemail.Text = reader["email"].ToString();
                                txtphone.Text = reader["phone"].ToString();
                                txtaddress.Text = reader["address"].ToString();
                                txtcity.Text = reader["city"].ToString();
                                if (!reader.IsDBNull(reader.GetOrdinal("birthday")))
                                {
                                    txtbirthday.Text = Convert.ToDateTime(reader["birthday"]).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    txtbirthday.Text = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetProfileImageUrl()
        {
            int userId = (int)Session["UserID"];
            string imageUrl = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT  URL FROM document_information WHERE belong_to = @userId ORDER BY created_when DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        imageUrl = result.ToString();
                    }
                    else
                    {
                        // Nếu không tìm thấy ảnh hồ sơ, bạn có thể trả về một ảnh mặc định hoặc để trống
                        imageUrl = ResolveUrl("~/App_Themes/Admin_Pages/images/photos/thumb1.png");
                    }
                }
            }

            return imageUrl;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tên";
                return;
            }
            int userId = (int)Session["UserID"];
            string name = txtname.Text.Trim();
            string email = txtemail.Text.Trim();
            string phone = txtphone.Text.Trim();
            string address = txtaddress.Text.Trim();
            string city = txtcity.Text.Trim();
            

            if (FileUploadImage.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(FileUploadImage.FileName).ToLower();
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    string fileName = Path.GetFileNameWithoutExtension(FileUploadImage.PostedFile.FileName) + fileExtension;
                    string filePath = Server.MapPath("~/UploadedImages/") + fileName;
                    FileUploadImage.SaveAs(filePath);

                    string imageUrl = "~/UploadedImages/" + fileName;
                    UpdateProfileImage(userId, imageUrl);
                }
                else
                {
                    txtlableer.Visible = true;
                    txtlableer.ForeColor = System.Drawing.Color.Red;
                    txtlableer.Text = "Chỉ cho phép tải lên các file JPG, JPEG, PNG hoặc GIF.";
                    return;
                }
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE employee SET name = @name, email = @email, phone = @phone, address = @address, city = @city, birthday = @birthday , last_update_when=@last_update_when WHERE id = @userId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtbirthday.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtbirthday.Text));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            
            txtlableer.Visible = true;
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Text = "Thông Tin Được Cập Nhật Thành Công";
            RegisterHideLabelScript();
        }

        private void UpdateProfileImage(int userId, string imageUrl)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE document_information SET URL = @imageUrl, created_by = @userId  WHERE belong_to = @userId ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@imageUrl", imageUrl);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void RegisterHideLabelScript()
        {
            string script = "hideLabelAfterTimeout();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }
    }
}
