using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace hotel.Admin
{
    public partial class CompanyInformation : System.Web.UI.Page
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
                bindCompanyDetailsToListView("");

                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
                LoadCountries();
            }
        }

        public class Country
        {
            [JsonProperty("name")]
            public Name Name { get; set; }

            // Thêm thuộc tính này để binding
            public string CommonName => Name?.Common;
        }

        public class Name
        {
            [JsonProperty("common")]
            public string Common { get; set; }
        }

        private void LoadCountries()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://restcountries.com/v3.1/all";
                HttpResponseMessage response = client.GetAsync(apiUrl).Result; // Chuyển từ await sang .Result để đồng bộ hóa

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result; // Chuyển từ await sang .Result
                    List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(jsonResponse);

                    ddlCountries.DataSource = countries.OrderBy(c => c.CommonName).ToList();
                    ddlCountries.DataTextField = "CommonName";
                    ddlCountries.DataValueField = "CommonName";
                    ddlCountries.DataBind();

                    ddlCountries.Items.Insert(0, new ListItem("Select Country", ""));
                }
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
            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập type";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO company (active, type,  name, address, email, phone, establish_date , description,  country ,city,  created_by, created_when, last_update_by, last_update_when) VALUES (@active, @type, @name,@address, @email, @phone,@establist_date, @description, @country,  @city,  @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@type", txttype.Text);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@establish_date", string.IsNullOrEmpty(txtesdate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtesdate.Text));
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtdescription.Text) ? (object)DBNull.Value : txtdescription.Text);
            cmd.Parameters.AddWithValue("@country", string.IsNullOrEmpty(ddlCountries.SelectedValue) ? (object)DBNull.Value : ddlCountries.SelectedValue);
            cmd.Parameters.AddWithValue("@city", string.IsNullOrEmpty(txtcity.Text) ? (object)DBNull.Value : txtcity.Text);
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
            bindCompanyDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;

        }

        //===== Method to bind employee records to ListView control.
        void bindCompanyDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += " name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += " active = " + SearchActiveDropdown.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = "SELECT id ,active ,type ,name ,address ,email ,phone ,establish_date ,description ,country,city ,created_by ,created_when ,last_update_by,last_update_when FROM company ";

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
            lstViewCompanyDetails.DataSource = cmd.ExecuteReader();
            lstViewCompanyDetails.DataBind();
        }

        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txttype.Text = string.Empty;
            txtname.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtesdate.Text = string.Empty;
            txtdescription.Text = string.Empty;
            ddlCountries.SelectedIndex = 0;
            txtcity.Text = string.Empty;

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindCompanyDetailsToListView(searchName);
        }

        protected void lstViewCompanyDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    deleteCompany(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindCompanyDetailToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }

        void deleteCompany(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                string cmdText = "DELETE FROM company WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindCompanyDetailsToListView("");
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

        public void bindCompanyDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            txth2.Text = "Chỉnh Sửa Thông Tin Công Ty";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM company WHERE id=@id";
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
                txttype.Text = dr["type"].ToString();
                txtname.Text = dr["name"].ToString();
                txtaddress.Text = dr["address"].ToString();
                txtemail.Text = dr["email"].ToString();
                txtphone.Text = dr["phone"].ToString();

                if (!dr.IsDBNull(dr.GetOrdinal("establish_date")))
                {
                    txtesdate.Text = Convert.ToDateTime(dr["establish_date"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtesdate.Text = "";
                }

                txtdescription.Text = dr["description"].ToString();

                // Kiểm tra và gán giá trị cho ddlCountries nếu có trong danh sách
                string countryValue = dr["country"].ToString();
                if (ddlCountries.Items.FindByValue(countryValue) != null)
                {
                    ddlCountries.SelectedValue = countryValue;
                }
                else
                {
                    ddlCountries.SelectedIndex = -1; // Hoặc bạn có thể chọn giá trị mặc định nếu muốn
                }

                txtcity.Text = dr["city"].ToString();

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
                txtlableer.Text = "Vui lòng nhập tên";
                return;
            }
            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập type";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Update Query.
            string cmdText = "UPDATE company SET active=@active, type=@type, name=@name, address=@address, email=@email, phone=@phone, establish_date= @establish_date , description = @description, country= @country,  city=@city,  created_by=@created_by, created_when=@created_when, last_update_by=@last_update_by, last_update_when=@last_update_when WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@type", txttype.Text);
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@establish_date", string.IsNullOrEmpty(txtesdate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtesdate.Text));
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtdescription.Text) ? (object)DBNull.Value : txtdescription.Text);
            cmd.Parameters.AddWithValue("@country", string.IsNullOrEmpty(ddlCountries.SelectedValue) ? (object)DBNull.Value : ddlCountries.SelectedValue);
            cmd.Parameters.AddWithValue("@city", string.IsNullOrEmpty(txtcity.Text) ? (object)DBNull.Value : txtcity.Text);
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
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
            bindCompanyDetailsToListView("");

            //===== Show Save button and hide update button.
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
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
            txttype.Text = "";
            txtname.Text = "";
            txtaddress.Text = "";
            txtemail.Text = "";
            txtphone.Text = "";
            txtesdate.Text = "";
            txtdescription.Text = "";
            ddlCountries.SelectedIndex = 0;
            txtcity.Text = "";
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