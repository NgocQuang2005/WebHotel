using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;

namespace hotel.Admin
{
    public partial class ListOfReservations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                bindBookingDetailsToListView("");
                loadDataCus(txtcustomer);
                loadDataRoom(SearchRoom);
                loadDataRoom(txtroom);
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataCus(DropDownList dropemploy)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT id  , name FROM contact_management WHERE active = 1 AND type = 'Customer' ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dropemploy.DataSource = reader;
                    dropemploy.DataTextField = "name";
                    dropemploy.DataValueField = "id";
                    dropemploy.DataBind();
                }
                // Chèn mục mặc định vào DropDownList
                dropemploy.Items.Insert(0, new ListItem("Chọn Khách Hàng", ""));
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

        private string GetNextOrderCode()
        {
            string nextOrderCode = "mp1";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT MAX(CAST(SUBSTRING(code, 3, LEN(code) - 2) AS INT)) FROM orders WHERE code LIKE 'mp%'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int maxCode = Convert.ToInt32(result);
                        nextOrderCode = "mp" + (maxCode + 1);
                    }
                }
            }

            return nextOrderCode;
        }
        private DataRow GetRoomDetails(int roomId)
        {
            DataRow roomDetails = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = @"SELECT r.id as idphong, r.name as tenphong , rc.price as sotien
                            FROM room r
                            JOIN room_type rt on rt.id = r.type_room_id
                            JOIN room_config rc on rc.type_room_id = rt.id
                            WHERE r.id = @roomId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    conn.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            roomDetails = dt.Rows[0];
                        }
                    }
                }
            }

            return roomDetails;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtnumofpp.Text))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Số Lượng Người";
                return;
            }
            if (string.IsNullOrEmpty(txtCheckIn.Text))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Giờ Vào";
                return;
            }
            if (string.IsNullOrEmpty(txtCheckOut.Text))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Giờ Ra";
                return;
            }

            int active = txtacti.Checked ? 1 : 0;
           
            int paymentstt = txtthanhtoan.Checked ? 1 : 0;
            string roomIdStr = txtroom.SelectedValue;
            if (string.IsNullOrEmpty(roomIdStr))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Phòng";
                return;
            }

            int roomId = Convert.ToInt32(roomIdStr);
            DataRow dr = GetRoomDetails(roomId);
            if (dr == null)
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Không Tìm Thấy Thông Tin Phòng";
                return;
            }
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra giá trị last_update_by trong bảng account
                        int UpdateBy = Convert.ToInt32(Session["AccountId"]);
                        string checkAccountQuery = "SELECT COUNT(1) FROM account WHERE id = @id";
                        using (SqlCommand checkCmd = new SqlCommand(checkAccountQuery, conn, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@id", UpdateBy);
                            int exists = (int)checkCmd.ExecuteScalar();
                            if (exists == 0)
                            {
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Người dùng không tồn tại.";
                                transaction.Rollback();
                                return;
                            }
                        }

                        string orderCode = GetNextOrderCode();

                        string insertOrderQuery = @"
                INSERT INTO orders (code, active, custom_id, total_payment, payment_status, created_by, created_when, last_update_by, last_update_when)
                OUTPUT INSERTED.id
                VALUES (@code, @active, @custom_id, @total_payment, @payment_status, @created_by, @created_when, @last_update_by, @last_update_when)";

                        int orderId;

                        using (SqlCommand cmd = new SqlCommand(insertOrderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@code", orderCode);
                            cmd.Parameters.AddWithValue("@active", active);
                            cmd.Parameters.AddWithValue("@custom_id", txtcustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@total_payment", CalculateTotalPayment(dr));
                            cmd.Parameters.AddWithValue("@payment_status", paymentstt);
                            cmd.Parameters.AddWithValue("@created_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@last_update_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

                            orderId = (int)cmd.ExecuteScalar();
                        }

                        string insertOrderDetailQuery = @"
                INSERT INTO order_detail (active, room_id, order_id, status, total_amount, number_of_people, check_in_time, check_out_time, created_by, created_when, last_update_by, last_update_when)
                VALUES (@active, @room_id, @order_id, @status, @total_amount, @number_of_people, @check_in_time, @check_out_time, @created_by, @created_when, @last_update_by, @last_update_when)";

                        using (SqlCommand cmd = new SqlCommand(insertOrderDetailQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@active", active);
                            cmd.Parameters.AddWithValue("@room_id", roomId);
                            cmd.Parameters.AddWithValue("@order_id", orderId);
                            cmd.Parameters.AddWithValue("@total_amount", dr["sotien"]);
                            cmd.Parameters.AddWithValue("@number_of_people", txtnumofpp.Text);
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt32(drStatus.SelectedValue));
                            cmd.Parameters.AddWithValue("@check_in_time", Convert.ToDateTime(txtCheckIn.Text));
                            cmd.Parameters.AddWithValue("@check_out_time", Convert.ToDateTime(txtCheckOut.Text));
                            cmd.Parameters.AddWithValue("@created_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@last_update_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        clearInputControls();
                        bindBookingDetailsToListView("");
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Bạn đã đặt phòng thành công";
                        lblSuccess.ForeColor = System.Drawing.Color.Green;
                        RegisterHideLabelScript();
                        form_data.Visible = false;
                        list_data.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Đã xảy ra lỗi khi đặt phòng. Lỗi: " + ex;
                        lblSuccess.ForeColor = System.Drawing.Color.Red;
                    }
                }
                conn.Close();
            }
        }


        private decimal CalculateTotalPayment(DataRow roomDetails)
        {
            DateTime checkIn = Convert.ToDateTime(txtCheckIn.Text);
            DateTime checkOut = Convert.ToDateTime(txtCheckOut.Text);
            int durationDays = (checkOut - checkIn).Days;
            return durationDays * Convert.ToDecimal(roomDetails["sotien"]);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtacti.Checked = false;
            txtcustomer.SelectedIndex = 0;
            txtnumofpp.Text = "";
            txtCheckIn.Text = "";
            txtCheckOut.Text = "";
            txtroom.SelectedIndex = 0;
            form_data.Visible = false;
            list_data.Visible = true;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = true;
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            lblSuccess.Visible = false;
            form_data.Visible = true;
            list_data.Visible = false;
        }
        void bindBookingDetailsToListView(string searchName)
        {
            try
            {
                string condition = "1 = 1";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(searchName))
                {
                    condition += " AND cm.phone LIKE @searchPhone";
                    parameters.Add(new SqlParameter("@searchPhone", "%" + searchName + "%"));
                }
                if (!string.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
                {
                    condition += " AND o.active = @active";
                    parameters.Add(new SqlParameter("@active", SearchActiveDropdown.SelectedValue));
                }
                if (!string.IsNullOrEmpty(SearchRoom.SelectedValue))
                {
                    condition += " AND r.id = @nameroom";
                    parameters.Add(new SqlParameter("@nameroom", SearchRoom.SelectedValue));
                }
                if (!string.IsNullOrEmpty(SearchNguoiTao.SelectedValue))
                {
                    if (SearchNguoiTao.SelectedValue == "1")
                    {
                        condition += " AND acct.employee_id IS NOT NULL";
                    }
                    else if (SearchNguoiTao.SelectedValue == "0")
                    {
                        condition += " AND acct.customer_id IS NOT NULL";
                    }
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    // Câu lệnh truy vấn Select.
                    string cmdText = @"
                SELECT 
                    CASE 
                        WHEN acct.employee_id IS NOT NULL THEN 'Nhân Viên'
                        WHEN acct.customer_id IS NOT NULL THEN 'Khách Hàng'
                        END AS nguoitao,
                        CASE 
                            WHEN acct.employee_id IS NOT NULL THEN emp.name
                            WHEN acct.customer_id IS NOT NULL THEN cm2.name
                        END AS ten_nguoitao,
                        o.active AS active, 
                        cm.id AS customer_id, 
                        cm.name AS customer_name, 
                        cm.email AS customer_email,
                        cm.phone AS customer_phone, 
                        o.id AS order_id, 
                        o.code AS order_code,
                        o.total_payment, 
                        o.payment_status AS payment, 
                        od.id AS order_detail_id,
                        od.room_id, 
                        r.id AS room_id,
                        r.name AS room_name, 
                        od.number_of_people AS nb_people, 
                        od.check_in_time, 
                        od.check_out_time,
                        od.status AS trangthai
                    FROM 
                        orders o 
                        JOIN contact_management cm ON cm.id = o.custom_id 
                        JOIN account acct ON o.created_by = acct.id
                        JOIN order_detail od ON acct.id = od.created_by and  o.id = od.order_id
                        JOIN room r ON od.room_id = r.id
                        LEFT JOIN employee emp ON acct.employee_id = emp.id
                        LEFT JOIN contact_management cm2 ON acct.customer_id = cm2.id
                    WHERE 
                        cm.type = 'customer' AND " + condition;

                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        // Thêm các tham số để tránh SQL injection.
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                        // Log câu lệnh SQL để kiểm tra lỗi.
                        System.Diagnostics.Debug.WriteLine("SQL Command: " + cmd.CommandText);
                        foreach (SqlParameter param in cmd.Parameters)
                        {
                            System.Diagnostics.Debug.WriteLine(param.ParameterName + ": " + param.Value);
                        }

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        // Thực thi câu lệnh và gán dữ liệu vào ListView.
                        lstViewBookingDetails.DataSource = cmd.ExecuteReader();
                        lstViewBookingDetails.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor= System.Drawing.Color.Red;
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindBookingDetailsToListView(searchName);
        }

        protected void lstViewBookingDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):
                    int id = Convert.ToInt32(e.CommandArgument);
                    delete(id);
                    break;
                case ("Edt"):
                    id = Convert.ToInt32(e.CommandArgument);
                    bindBookingDetailsToEdit(id);
                    form_data.Visible = true;
                    list_data.Visible = false;
                    break;
            }
        }
        void bindBookingDetailsToEdit(int id)
        {
            lblSuccess.Visible = true;
            lblSuccess.Text = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                // Câu lệnh truy vấn Select để lấy thông tin của đơn hàng
                string cmdText = @"
            SELECT 
                o.active AS active, 
                cm.id AS customer_id, 
                cm.name AS customer_name, 
                cm.email AS customer_email,
                cm.phone AS customer_phone, 
                o.id AS order_id, 
                o.code AS order_code,
                o.total_payment, 
                o.payment_status AS payment, 
                od.id AS order_detail_id,
                od.room_id,
                r.id AS room_id,
                r.name AS room_name, 
                od.number_of_people AS nb_people, 
                od.check_in_time, 
                od.check_out_time,
                od.status as trangthai 
            FROM 
                orders o 
            JOIN contact_management cm ON cm.id = o.custom_id 
            JOIN order_detail od ON o.id = od.order_id 
            JOIN room r ON od.room_id = r.id
            WHERE 
                cm.type = 'customer' AND o.id = @orderId";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.Add(new SqlParameter("@orderId", id));

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtacti.Checked = reader["active"] != DBNull.Value && Convert.ToBoolean(reader["active"]);
                        if (reader["payment"] != DBNull.Value)
                        {
                            txtthanhtoan.Checked = Convert.ToBoolean(reader["payment"]);
                        }
                        else
                        {
                            txtthanhtoan.Checked = false;
                        }
                        if (reader["trangthai"] != DBNull.Value)
                        {
                            drStatus.SelectedValue = reader["trangthai"].ToString();
                        }
                        else
                        {
                            drStatus.SelectedIndex = -1;
                        }
                        if (reader["customer_id"] != DBNull.Value)
                        {
                            txtcustomer.SelectedValue = reader["customer_id"].ToString();
                        }
                        else
                        {
                            txtcustomer.SelectedIndex = -1;
                        }
                        if (reader["room_id"] != DBNull.Value)
                        {
                            string roomId = reader["room_id"].ToString();
                            ListItem roomItem = txtroom.Items.FindByValue(roomId);
                            if (roomItem != null)
                            {
                                txtroom.SelectedValue = roomId;
                            }
                            else
                            {
                                txtroom.SelectedIndex = -1;
                                Console.WriteLine("Không tìm thấy Room ID: " + roomId);
                            }
                        }
                        else
                        {
                            txtroom.SelectedIndex = -1;
                        }
                        if (reader["check_in_time"] != DBNull.Value)
                        {
                            txtCheckIn.Text = Convert.ToDateTime(reader["check_in_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            txtCheckIn.Text = string.Empty;
                        }
                        if (reader["check_out_time"] != DBNull.Value)
                        {
                            txtCheckOut.Text = Convert.ToDateTime(reader["check_out_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            txtCheckOut.Text = string.Empty;
                        }
                        if (reader["nb_people"] != DBNull.Value)
                        {
                            txtnumofpp.Text = reader["nb_people"].ToString();
                        }
                        else
                        {
                            txtnumofpp.Text = string.Empty;
                        }
                        if (reader["order_id"] != DBNull.Value)
                        {
                            ViewState["CurrentOrderID"] = reader["order_id"];
                        }
                        else
                        {
                            ViewState["CurrentOrderID"] = null;
                        }

                        if (reader["order_detail_id"] != DBNull.Value)
                        {
                            ViewState["CurrentOrderDetailID"] = reader["order_detail_id"];
                        }
                        else
                        {
                            ViewState["CurrentOrderDetailID"] = null;
                        }
                        // Make update button visible and save button invisible
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                        btnCancel.Visible = true;
                    }
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtnumofpp.Text))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Số Lượng Người";
                return;
            }
            if (string.IsNullOrEmpty(txtCheckIn.Text))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Giờ Vào";
                return;
            }
            if (string.IsNullOrEmpty(txtCheckOut.Text))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Giờ Ra";
                return;
            }

            int active = txtacti.Checked ? 1 : 0;
            
            int paymentstt = txtthanhtoan.Checked ? 1 : 0;
            string roomIdStr = txtroom.SelectedValue;
            if (string.IsNullOrEmpty(roomIdStr))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Vui Lòng Chọn Phòng";
                return;
            }

            int roomId = Convert.ToInt32(roomIdStr);
            DataRow dr = GetRoomDetails(roomId);
            if (dr == null)
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Không Tìm Thấy Thông Tin Phòng";
                return;
            }
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int UpdateBy = Convert.ToInt32(Session["AccountId"]);
                        string checkAccountQuery = "SELECT COUNT(1) FROM account WHERE id = @id";
                        using (SqlCommand checkCmd = new SqlCommand(checkAccountQuery, conn, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@id", UpdateBy);
                            int exists = (int)checkCmd.ExecuteScalar();
                            if (exists == 0)
                            {
                                lblSuccess.Visible = true;
                                lblSuccess.Text = "Người dùng không tồn tại.";
                                transaction.Rollback();
                                return;
                            }
                        }
                        // Update order details
                        string updateOrderQuery = @"UPDATE orders
                                            SET active = @active,
                                                custom_id = @custom_id,
                                                total_payment = @total_payment,
                                                payment_status=@payment_status,
                                                last_update_by = @last_update_by,
                                                last_update_when = @last_update_when
                                            WHERE id = @order_id";

                        using (SqlCommand cmd = new SqlCommand(updateOrderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@active", active);
                            cmd.Parameters.AddWithValue("@custom_id", txtcustomer.SelectedValue);

                            DateTime checkIn = Convert.ToDateTime(txtCheckIn.Text);
                            DateTime checkOut = Convert.ToDateTime(txtCheckOut.Text);
                            decimal totalPayment = (checkOut - checkIn).Days * Convert.ToDecimal(dr["sotien"]);

                            cmd.Parameters.AddWithValue("@total_payment", totalPayment);
                            cmd.Parameters.AddWithValue("@payment_status", paymentstt);
                            cmd.Parameters.AddWithValue("@last_update_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                            cmd.Parameters.AddWithValue("@order_id", ViewState["CurrentOrderID"]);

                            cmd.ExecuteNonQuery();
                        }

                        // Update order details
                        string updateOrderDetailQuery = @"UPDATE order_detail
                                                  SET active = @active,
                                                      room_id = @room_id,
                                                      total_amount = @total_amount,
                                                      number_of_people = @number_of_people,
                                                      status = @status,
                                                      check_in_time = @check_in_time,
                                                      check_out_time = @check_out_time,
                                                      last_update_by = @last_update_by,
                                                      last_update_when = @last_update_when
                                                  WHERE id = @order_detail_id";

                        using (SqlCommand cmd = new SqlCommand(updateOrderDetailQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@active", active);
                            cmd.Parameters.AddWithValue("@room_id", txtroom.SelectedValue);
                            cmd.Parameters.AddWithValue("@total_amount", dr["sotien"]);
                            cmd.Parameters.AddWithValue("@number_of_people", txtnumofpp.Text);
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt32(drStatus.SelectedValue));
                            cmd.Parameters.AddWithValue("@check_in_time", Convert.ToDateTime(txtCheckIn.Text));
                            cmd.Parameters.AddWithValue("@check_out_time", Convert.ToDateTime(txtCheckOut.Text));
                            cmd.Parameters.AddWithValue("@last_update_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                            cmd.Parameters.AddWithValue("@order_detail_id", ViewState["CurrentOrderDetailID"]);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        clearInputControls();
                        bindBookingDetailsToListView("");
                        btnSave.Visible = true;
                        btnUpdate.Visible = false;
                        btnCancel.Visible = false;
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Cập nhật thông tin đặt phòng thành công";
                        lblSuccess.ForeColor = System.Drawing.Color.Green;
                        RegisterHideLabelScript();
                        form_data.Visible = false;
                        list_data.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Có lỗi xảy ra khi cập nhật thông tin. Vui lòng thử lại.";
                        lblSuccess.ForeColor = System.Drawing.Color.Red;
                        throw new Exception("Error updating order. See inner exception for details.", ex);
                    }
                }
            }
        }

        void delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Delete from order_detail
                        string deleteOrderDetailQuery = "DELETE FROM order_detail WHERE order_id = @id";
                        using (SqlCommand cmd = new SqlCommand(deleteOrderDetailQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete from orders
                        string deleteOrderQuery = "DELETE FROM orders WHERE id = @id";
                        using (SqlCommand cmd = new SqlCommand(deleteOrderQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        bindBookingDetailsToListView("");
                        RegisterHideLabelScript();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblMessage.Text = "Có lỗi xảy ra khi xóa đặt phòng. Lỗi: "  + ex;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        private void RegisterHideLabelScript()
        {
            string script = "hideLabelAfterTimeout();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideLabelScript", script, true);
        }
        void clearInputControls()
        {
            txtacti.Checked = false;
            txtnumofpp.Text = string.Empty;
            txtCheckIn.Text = string.Empty;
            txtCheckOut.Text = string.Empty;
            txtroom.SelectedIndex = 0;
            txtcustomer.SelectedIndex = 0;

        }
    }
}