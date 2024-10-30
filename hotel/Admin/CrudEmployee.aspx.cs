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
using System.Net.NetworkInformation;
using System.Collections;
using System.Web.UI.HtmlControls;

namespace hotel.Admin
{
    public partial class CrudEmployee : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                bindEmployeeDetailsToListView("");
                loadData(txtdepar);
                loadData(SearchOrgstruc);
                hfSearchNameClientID.Value = SearchName.ClientID;
                
            }
        }
        public void loadData(DropDownList dropDownList)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT id  , name FROM orgstructure WHERE active = 1 ";

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
                dropDownList.Items.Insert(0, new ListItem("Chọn Chi Nhánh", ""));
            }
        }
        private string SaveUploadedFile(FileUpload fileUpload)
        {
            if (fileUpload.HasFile)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = System.IO.Path.GetExtension(fileUpload.FileName).ToLower();

                if (allowedExtensions.Contains(fileExtension))
                {
                    string fileName = System.IO.Path.GetFileName(fileUpload.FileName);
                    string filePath = Server.MapPath("~/UploadedImages/") + fileName;
                    fileUpload.SaveAs(filePath);
                    return "~/UploadedImages/" + fileName;
                }
                else
                {
                    txtlableer.Visible = true;
                    txtlableer.ForeColor = System.Drawing.Color.Red;
                    txtlableer.Text = "Chỉ cho phép tải lên các file JPG, JPEG, PNG hoặc GIF.";
                }
            }
            return null;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tên";
                return;
            }
            if (txtgtinh.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Giới Tính";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }

            string imageUrl = SaveUploadedFile(FileUploadImage);

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "INSERT INTO employee (active, name, gender, birthday, email, phone, address, city, start_work, end_work, department_id, created_by, created_when, last_update_by, last_update_when) VALUES (@active, @name, @gender, @birthday, @email, @phone, @address, @city, @start_work, @end_work, @department_id, @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@gender", Convert.ToInt32(txtgtinh.SelectedValue));
            cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtbthday.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtbthday.Text));
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
            cmd.Parameters.AddWithValue("@city", string.IsNullOrEmpty(txtcity.Text) ? (object)DBNull.Value : txtcity.Text);
            cmd.Parameters.AddWithValue("@start_work", string.IsNullOrEmpty(txtstarwork.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtstarwork.Text));
            cmd.Parameters.AddWithValue("@end_work", string.IsNullOrEmpty(txtenwork.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtenwork.Text));
            cmd.Parameters.AddWithValue("@department_id", string.IsNullOrEmpty(txtdepar.SelectedValue) ? (object)DBNull.Value : Convert.ToInt32(txtdepar.SelectedValue));
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.ExecuteNonQuery();

            // Save image URL to document_information table
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string docCmdText = "INSERT INTO document_information (active, created_by, created_when, type, URL, belong_to) VALUES (1, @created_by, @created_when, 'Profile Image', @URL, (SELECT IDENT_CURRENT('employee')))";
                SqlCommand docCmd = new SqlCommand(docCmdText, con);
                docCmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
                docCmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                docCmd.Parameters.AddWithValue("@URL", imageUrl);
                docCmd.ExecuteNonQuery();
            }

            con.Close();
            clearInputControls();
            bindEmployeeDetailsToListView("");
            txtlableer.Text = "Thêm Mới Thành Công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }
        void bindEmployeeDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "e.name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "e.active = " + SearchActiveDropdown.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchOrgstruc.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "org.id = " + SearchOrgstruc.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT  e.id as id , e.active, e.name, e.gender,e.birthday,di.URL as url,e.email,e.phone,e.address,e.city,e.start_work,e.end_work  ,e.department_id,e.created_by,e.created_when,e.last_update_by,e.last_update_when, org.name as tenchinhanh, org.id as idchinhanh FROM employee e left join document_information di on di.belong_to = e.id left join orgstructure org on org.id =e.department_id";

            if (!String.IsNullOrEmpty(condition))
            {
                cmdText += " where " + condition;
            }
            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            lstViewEmployeeDetails.DataSource = cmd.ExecuteReader();
            lstViewEmployeeDetails.DataBind();
        }
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtname.Text = string.Empty;
            txtgtinh.SelectedIndex = 0;
            txtbthday.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtcity.Text = string.Empty;
            txtstarwork.Text = string.Empty;
            txtenwork.Text = string.Empty;
            txtdepar.SelectedIndex = 0;
        }

       
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindEmployeeDetailsToListView(searchName);
        }
        
        protected void lstViewEmployeeDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    deleteEmployee(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindEmployeeDetailToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }

        void deleteEmployee(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "DELETE FROM employee WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindEmployeeDetailsToListView("");
                txtlableer.Text = "Xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();
            }
            catch(Exception ex)
            {
                string script = "alert('err');";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }

        }

        public void bindEmployeeDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            txth2.Text = "Chỉnh Sửa Thông Tin Nhân Viên";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = @"
                                SELECT          e.*, d.URL 
                                FROM employee   e
                                LEFT JOIN       document_information d ON e.id = d.belong_to
                                WHERE           e.id=@id";
            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Parameters.AddWithValue("@id", id);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                txtacti.Checked = Convert.ToBoolean(dr["active"]);
                txtname.Text = dr["name"].ToString();
                string genderValue = dr["gender"].ToString();
                if (txtgtinh.Items.FindByValue(genderValue) != null)
                {
                    txtgtinh.SelectedValue = genderValue;
                }
                else
                {
                    txtgtinh.SelectedIndex = 0; // Or set a default value
                }
                if (!dr.IsDBNull(dr.GetOrdinal("birthday")))
                {
                    txtbthday.Text = Convert.ToDateTime(dr["birthday"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtbthday.Text = "";
                }
                txtemail.Text = dr["email"].ToString();
                txtphone.Text = dr["phone"].ToString();
                txtaddress.Text = dr["address"].ToString();
                txtcity.Text = dr["city"].ToString();
                if (!dr.IsDBNull(dr.GetOrdinal("start_work")))
                {
                    txtstarwork.Text = Convert.ToDateTime(dr["start_work"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtstarwork.Text = "";
                }

                if (!dr.IsDBNull(dr.GetOrdinal("end_work")))
                {
                    txtenwork.Text = Convert.ToDateTime(dr["end_work"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtenwork.Text = "";
                }
                txtdepar.SelectedValue = dr["department_id"].ToString();

                // Set image URL
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


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            if (string.IsNullOrEmpty(txtname.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Nhập Tên";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            if (txtgtinh.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui Lòng Chọn Giới Tính";
                return;
            }

            string imageUrl = SaveUploadedFile(FileUploadImage);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"
            UPDATE employee 
            SET active=@active, name=@name, gender=@gender, birthday=@birthday, email=@email, phone=@phone, 
                address=@address, city=@city, start_work=@start_work, end_work=@end_work, 
                department_id=@department_id, last_update_by=@last_update_by, last_update_when=@last_update_when 
            WHERE id=@id";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@active", active);
                    cmd.Parameters.AddWithValue("@name", txtname.Text);
                    cmd.Parameters.AddWithValue("@gender", Convert.ToInt32(txtgtinh.SelectedValue));
                    cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtbthday.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtbthday.Text));
                    cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
                    cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
                    cmd.Parameters.AddWithValue("@city", string.IsNullOrEmpty(txtcity.Text) ? (object)DBNull.Value : txtcity.Text);
                    cmd.Parameters.AddWithValue("@start_work", string.IsNullOrEmpty(txtstarwork.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtstarwork.Text));
                    cmd.Parameters.AddWithValue("@end_work", string.IsNullOrEmpty(txtenwork.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtenwork.Text));
                    cmd.Parameters.AddWithValue("@department_id", string.IsNullOrEmpty(txtdepar.SelectedValue) ? (object)DBNull.Value : Convert.ToInt32(txtdepar.SelectedValue));
                    cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    string checkImageCmdText = "SELECT COUNT(*) FROM document_information WHERE belong_to=@id ";
                    using (SqlCommand checkImageCmd = new SqlCommand(checkImageCmdText, con))
                    {
                        checkImageCmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));
                        int imageCount = (int)checkImageCmd.ExecuteScalar();

                        if (imageCount > 0)
                        {
                            string updateImageCmdText = "UPDATE document_information SET URL=@URL WHERE belong_to=@id ";
                            using (SqlCommand updateImageCmd = new SqlCommand(updateImageCmdText, con))
                            {
                                updateImageCmd.Parameters.AddWithValue("@URL", imageUrl);
                                updateImageCmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));
                                updateImageCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string insertImageCmdText = "INSERT INTO document_information (belong_to, URL, type) VALUES (@id, @URL, 'Profile Image')";
                            using (SqlCommand insertImageCmd = new SqlCommand(insertImageCmdText, con))
                            {
                                insertImageCmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));
                                insertImageCmd.Parameters.AddWithValue("@URL", imageUrl);
                                insertImageCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                con.Close();
            }

            clearInputControls();
            bindEmployeeDetailsToListView("");
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
            hfSelectedRecord.Value = string.Empty;
            txtlableer.Text = "Update Thành Công";
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
            txtgtinh.SelectedIndex = 0;
            txtbthday.Text = "";
            txtemail.Text = "";
            txtphone.Text = "";
            txtaddress.Text = "";
            txtcity.Text = "";
            txtstarwork.Text = "";
            txtenwork.Text = "";
            txtdepar.SelectedIndex = 0;
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
        }
    }
}
