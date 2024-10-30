using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace hotel.Admin
{
    public partial class ManageCustomerContacts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                bindCustomDetailsToListView("");
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
            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng Type";
                return;
            }
            
            int active = 0;
            if (txtactive.Checked)
            {
                active = 1;
            }
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO contact_management (active, type ,name ,gender ,birthday ,address  ,email  ,phone ,identify_number ,nationality  ,description,  created_by, created_when, last_update_by, last_update_when) VALUES (@active, @type ,@name ,@gender ,@birthday ,@address  ,@email  ,@phone ,@identify_number ,@nationality  ,@description, @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@type", txttype.Text);
            cmd.Parameters.AddWithValue("@name", string.IsNullOrEmpty(txtname.Text) ? (object)DBNull.Value : txtname.Text);
            cmd.Parameters.AddWithValue("@gender", Convert.ToInt32(txtgtinh.SelectedValue));
            cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtbthday.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtbthday.Text));
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@identify_number", string.IsNullOrEmpty(txtndphone.Text) ? (object)DBNull.Value : txtndphone.Text);
            cmd.Parameters.AddWithValue("@nationality", string.IsNullOrEmpty(ddlCountries.SelectedValue) ? (object)DBNull.Value : ddlCountries.SelectedValue);
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtmt.Text) ? (object)DBNull.Value : txtmt.Text);
            cmd.Parameters.AddWithValue("@created_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.ExecuteNonQuery();
            con.Close();
            clearInputControls();

            //===== Bind data to ListView.
            bindCustomDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }
        void bindCustomDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += " phone LIKE '%" + searchName + "%'";
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
            string cmdText = "SELECT id,  active ,type, name , gender ,birthday ,address ,email ,phone ,identify_number ,nationality ,description ,created_by ,created_when ,last_update_by ,last_update_when FROM contact_management  ";
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
            lstViewCustomDetails.DataSource = cmd.ExecuteReader();
            lstViewCustomDetails.DataBind();
        }
       
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindCustomDetailsToListView(searchName);
        }

        void clearInputControls()
        {
            txtactive.Checked = false;
            txttype.Text = string.Empty;
            txtname.Text = string.Empty;
            txtgtinh.SelectedIndex = 0;
            txtbthday.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphone.Text = string.Empty;
            ddlCountries.SelectedIndex = 0;
            txtmt.Text = string.Empty;
            txtndphone.Text = string.Empty;
        }


        protected void lstViewCustomDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindCustomDetailToEdit(id);
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
                string cmdText = "DELETE FROM contact_management WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindCustomDetailsToListView("");
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
        public void bindCustomDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            txth2.Text = "Sửa Thông Tin Khách Hàng";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM contact_management WHERE id=@id";
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
                txtactive.Checked = Convert.ToBoolean(dr["active"]);
                txttype.Text = dr["type"].ToString();
                txtname.Text = dr["name"].ToString();
                // Cập nhật giới tính
                string genderValue = dr["gender"].ToString();
                if (txtgtinh.Items.FindByValue(genderValue) != null)
                {
                    txtgtinh.SelectedValue = genderValue;
                }
                else
                {
                    txtgtinh.SelectedIndex = 0; // Hoặc giá trị mặc định
                }
                if (!dr.IsDBNull(dr.GetOrdinal("birthday")))
                {
                    txtbthday.Text = Convert.ToDateTime(dr["birthday"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtbthday.Text = "";
                }
                txtaddress.Text = dr["address"].ToString();
                txtemail.Text = dr["email"].ToString();
                txtphone.Text = dr["phone"].ToString();
                txtndphone.Text = dr["identify_number"].ToString();
                string nationality = dr["nationality"].ToString();
                if (ddlCountries.Items.FindByValue(nationality) != null)
                {
                    ddlCountries.SelectedValue = nationality;
                }
                else
                {
                    ddlCountries.Items.Add(new ListItem(nationality, nationality));
                    ddlCountries.SelectedValue = nationality;
                }
                txtmt.Text = dr["description"].ToString();
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
            if (string.IsNullOrEmpty(txttype.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng Type";
                return;
            }

            int active = 0;
            if (txtactive.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "UPDATE contact_management SET active=@active, type= @type, name=@name,gender=@gender ,birthday=@birthday ,address=@address  ,email=@email  ,phone=@phone ,identify_number=@identify_number ,nationality=@nationality  ,description=@description ,  last_update_by=@last_update_by, last_update_when=@last_update_when WHERE id=@id";
            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@type", txttype.Text);
            cmd.Parameters.AddWithValue("@name", string.IsNullOrEmpty(txtname.Text) ? (object)DBNull.Value : txtname.Text);
            cmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(txtgtinh.SelectedValue) ? (object)DBNull.Value : Convert.ToInt32(txtgtinh.SelectedValue));
            cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtbthday.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtbthday.Text));
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@identify_number", string.IsNullOrEmpty(txtndphone.Text) ? (object)DBNull.Value : txtndphone.Text);
            cmd.Parameters.AddWithValue("@nationality", string.IsNullOrEmpty(ddlCountries.SelectedValue) ? (object)DBNull.Value : ddlCountries.SelectedValue);
            cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(txtmt.Text) ? (object)DBNull.Value : txtmt.Text);
            cmd.Parameters.AddWithValue("@last_update_by", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfSelectedRecord.Value));

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.ExecuteNonQuery();
            con.Close();
            clearInputControls();
            bindCustomDetailsToListView("");
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
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
            txtactive.Checked = false;
            txtname.Text = "";
            txtgtinh.SelectedIndex = 0;
            txtbthday.Text = "";
            txtaddress.Text = "";
            txtemail.Text = "";
            txtphone.Text = "";
            txtndphone.Text = "";
            ddlCountries.SelectedIndex = 0;
            txtmt.Text = "";
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