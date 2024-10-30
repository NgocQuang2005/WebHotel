using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http;

namespace hotel.Website
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
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

                    dropCountri.DataSource = countries.OrderBy(c => c.CommonName).ToList();
                    dropCountri.DataTextField = "CommonName";
                    dropCountri.DataValueField = "CommonName";
                    dropCountri.DataBind();

                    dropCountri.Items.Insert(0, new ListItem("Select Country", ""));
                }
            }
        }
        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập username";
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng nhập password";
                return;
            }

            string password = txtPassword.Text;
            string passwordPattern = @"^(?=.*[A-Z])(?=.*\d).{8,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Mật khẩu phải chứa ít nhất 1 kí tự in hoa, 1 kí tự số, và phải trên 8 kí tự";
                return;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            // Câu lệnh truy vấn để thêm vào contact_management và lấy customer_id đã chèn
            string cmdCustom = @"
                    INSERT INTO contact_management (active, type, name, gender, birthday, identify_number,nationality, address, email, phone, created_when, last_update_when) 
                    OUTPUT INSERTED.id 
                    VALUES (@active, @type, @name, @gender, @birthday ,@identify_number, @nationality, @address, @email, @phone, @created_when, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdCustom, con);

            // Thêm các tham số
            cmd.Parameters.AddWithValue("@active", 1);
            cmd.Parameters.AddWithValue("@type", "Customer");
            cmd.Parameters.AddWithValue("@name", string.IsNullOrEmpty(txtname.Text) ? (object)DBNull.Value : txtname.Text);
            cmd.Parameters.AddWithValue("@gender", Convert.ToInt32(txtgtinh.SelectedValue));
            cmd.Parameters.AddWithValue("@birthday", string.IsNullOrEmpty(txtngaysinh.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtngaysinh.Text));
            int identifyNumber;
            if (string.IsNullOrEmpty(txtidentifynumber.Text) || !Int32.TryParse(txtidentifynumber.Text, out identifyNumber))
            {
                cmd.Parameters.AddWithValue("@identify_number", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@identify_number", identifyNumber);
            }

            cmd.Parameters.AddWithValue("@nationality", string.IsNullOrEmpty(dropCountri.SelectedValue) ? (object)DBNull.Value : dropCountri.SelectedValue);
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(txtaddress.Text) ? (object)DBNull.Value : txtaddress.Text);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(txtemail.Text) ? (object)DBNull.Value : txtemail.Text);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(txtphone.Text) ? (object)DBNull.Value : txtphone.Text);
            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // Thực hiện câu lệnh truy vấn và lấy customer_id đã chèn
            int customerId = (int)cmd.ExecuteScalar();
            string hashedPassword = GetMd5Hash(txtPassword.Text);

            // Câu lệnh truy vấn để thêm vào bảng account
            string cmdAcount = "INSERT INTO account (username, password, role, active, created_when, last_update_when, custom_id) VALUES (@username, @password, @role, @active, @created_when, @last_update_when, @custom_id)";

            SqlCommand ssm = new SqlCommand(cmdAcount, con);
            ssm.Parameters.AddWithValue("@username", txtUserName.Text);
            ssm.Parameters.AddWithValue("@password", hashedPassword);
            ssm.Parameters.AddWithValue("@role", 3);
            ssm.Parameters.AddWithValue("@active", 1);
            ssm.Parameters.AddWithValue("@created_when", DateTime.Now);
            ssm.Parameters.AddWithValue("@last_update_when", DateTime.Now);
            ssm.Parameters.AddWithValue("@custom_id", customerId);

            ssm.ExecuteNonQuery();
            con.Close();

            txtlableer.Text = "Tạo Tài Khoản Thành Công";
            RegisterHideLabelScript();
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            Response.Redirect("~/Website/Login.aspx");
            ClientScript.RegisterStartupScript(this.GetType(), "showSuccessSignupToast", "showSuccessSignupToast();", true);
        }

        private void RegisterHideLabelScript()
        {
            string script = "hideLabelAfterTimeout();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }

    }
}