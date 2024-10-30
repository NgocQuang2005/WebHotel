using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace hotel.Admin
{
    public partial class EmployeeSalary : System.Web.UI.Page
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
                bindSalaryDetailsToListView("");
                loadData(txtemploy);

                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }

        public void loadData(DropDownList dropDownList)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM employee WHERE active = 1 ";

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
                dropDownList.Items.Insert(0, new ListItem("Chọn Nhân Viên", ""));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            txtlableer.Visible = false;
            txtlableer.ForeColor = System.Drawing.Color.Red;
            
            if (txtemploy.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn nhân viên";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Insert Query.
            string cmdText = "INSERT INTO employee_salary (active, date_from, date_to, base_salary, allowance, bonus, deduction, note, employee_id,  created_by, created_when, last_update_by, last_update_when) VALUES (@active, @date_from, @date_to,@base_salary, @allowance, @bonus, @deduction, @note, @employee_id, @created_by, @created_when, @last_update_by, @last_update_when)";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            //===== Adding parameters/Values.
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@date_from", string.IsNullOrEmpty(txtdatefrom.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtdatefrom.Text));
            cmd.Parameters.AddWithValue("@date_to", string.IsNullOrEmpty(txtdateto.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtdateto.Text));
            cmd.Parameters.AddWithValue("@base_salary", string.IsNullOrEmpty(txtbasesalary.Text) ? (object)DBNull.Value : Math.Round(float.Parse(txtbasesalary.Text), 2));
            cmd.Parameters.AddWithValue("@allowance", string.IsNullOrEmpty(txtallowance.Text) ? (object)DBNull.Value : Math.Round(float.Parse(txtallowance.Text),2));
            cmd.Parameters.AddWithValue("@bonus", string.IsNullOrEmpty(txtbonus.Text) ? (object)DBNull.Value : Math.Round(float.Parse(txtbonus.Text), 2));
            cmd.Parameters.AddWithValue("@deduction", string.IsNullOrEmpty(txtdeduction.Text) ? (object)DBNull.Value : Math.Round(float.Parse(txtdeduction.Text), 2));
            cmd.Parameters.AddWithValue("@note", string.IsNullOrEmpty(txtnote.Text) ? (object)DBNull.Value : txtnote.Text);
            cmd.Parameters.AddWithValue("@employee_id", Convert.ToInt32(txtemploy.SelectedValue));
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
            bindSalaryDetailsToListView("");
            txtlableer.Text = "Thêm Mới thành công";
            txtlableer.ForeColor = System.Drawing.Color.Green;
            txtlableer.Visible = true;
            RegisterHideLabelScript();
            form_data.Visible = false;
            list_data.Visible = true;
        }

        //===== Method to bind employee records to ListView control.
        void bindSalaryDetailsToListView(string searchName)
        {
            string condition = "";
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "el.name LIKE '%" + searchName + "%'";
            }
            if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "es.active = " + SearchActiveDropdown.SelectedValue;
            }
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = "SELECT es.id as id, el.id as idnv ,el.name as name , el.phone as phone , el.address ,  es.base_salary as base_salary , es.allowance as allowance, es.bonus as bonus, es.deduction as deduction,es.note as note ,es.active as active FROM employee  el RIGHT JOIN employee_salary  es ON el.id = es.employee_id ";
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
            lstViewSalaryDetails.DataSource = cmd.ExecuteReader();
            lstViewSalaryDetails.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindSalaryDetailsToListView(searchName);
        }
        
        //===== Clear Input control's data.
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtdatefrom.Text = string.Empty;
            txtdateto.Text = string.Empty;
            txtbasesalary.Text = string.Empty;
            txtallowance.Text = string.Empty;
            txtbonus.Text = string.Empty;
            txtdeduction.Text = string.Empty;
            txtnote.Text = string.Empty;
            txtemploy.SelectedIndex = 0;
        }



        protected void lstViewSalaryDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindSalaryDetailToEdit(id);
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
                string cmdText = "DELETE FROM employee_salary WHERE id=@id";
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.Parameters.AddWithValue("@id", id);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
                bindSalaryDetailsToListView("");
                txtlableer.Text = "Xóa thành công";
                txtlableer.ForeColor = System.Drawing.Color.Green;
                txtlableer.Visible = true;
                RegisterHideLabelScript();
            }
            catch(Exception ex)
            {
                string script = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "ShowError", script, true);
            }

        }

        public void bindSalaryDetailToEdit(int id)
        {
            txtlableer.Visible = false;
            //btnback.Visible = true;
            txth2.Text = "Sửa Lương Nhân Viên";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "SELECT * FROM employee_salary WHERE id=@id";
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
                txtacti.Checked =Convert.ToBoolean( dr["active"]);
               
                if (!dr.IsDBNull(dr.GetOrdinal("date_from")))
                {
                    txtdatefrom.Text = Convert.ToDateTime(dr["date_from"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtdatefrom.Text = "";
                }

                if (!dr.IsDBNull(dr.GetOrdinal("date_to")))
                {
                    txtdateto.Text = Convert.ToDateTime(dr["date_to"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtdateto.Text = "";
                }
                
                txtbasesalary.Text = dr["base_salary"].ToString();
                txtallowance.Text = dr["allowance"].ToString();
                txtbonus.Text = dr["bonus"].ToString();
                txtdeduction.Text = dr["deduction"].ToString();
                txtnote.Text = dr["note"].ToString();
                txtemploy.SelectedValue = dr["employee_id"].ToString();

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

            if (txtemploy.SelectedIndex == 0)
            {
                txtlableer.Visible = true;
                txtlableer.Text = "Vui lòng chọn nhân viên";
                return;
            }
            int active = 0;
            if (txtacti.Checked)
            {
                active = 1;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            string cmdText = "UPDATE employee_salary SET active=@active, date_from=@date_from, date_to=@date_to, base_salary=@base_salary, allowance=@allowance, bonus=@bonus, deduction=@deduction,note=@note, employee_id= @employee_id,  last_update_by=@last_update_by, last_update_when=@last_update_when WHERE id=@id";

            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@date_from", string.IsNullOrEmpty(txtdatefrom.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtdatefrom.Text));
            cmd.Parameters.AddWithValue("@date_to", string.IsNullOrEmpty(txtdateto.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtdateto.Text));
            cmd.Parameters.AddWithValue("@base_salary", string.IsNullOrEmpty(txtbasesalary.Text) ? (object)DBNull.Value : float.Parse(txtbasesalary.Text));
            cmd.Parameters.AddWithValue("@allowance", string.IsNullOrEmpty(txtallowance.Text) ? (object)DBNull.Value : float.Parse(txtallowance.Text));
            cmd.Parameters.AddWithValue("@bonus", string.IsNullOrEmpty(txtbonus.Text) ? (object)DBNull.Value : float.Parse(txtbonus.Text));
            cmd.Parameters.AddWithValue("@deduction", string.IsNullOrEmpty(txtdeduction.Text) ? (object)DBNull.Value : float.Parse(txtbasesalary.Text));
            cmd.Parameters.AddWithValue("@note", string.IsNullOrEmpty(txtnote.Text) ? (object)DBNull.Value : txtnote.Text);
            cmd.Parameters.AddWithValue("@employee_id", Convert.ToInt32(txtemploy.SelectedValue));
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
            bindSalaryDetailsToListView("");
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            btnCancel.Visible = false;
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
            txtdatefrom.Text = "";
            txtdateto.Text = "";
            txtbasesalary.Text = "";
            txtallowance.Text = "";
            txtbonus.Text = "";
            txtdeduction.Text = "";
            txtnote.Text = "";
            txtemploy.SelectedIndex = 0;
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