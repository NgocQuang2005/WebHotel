using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace hotel.Website
{
    public partial class BookingOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBookingDetails();
            }
        }

        private void BindBookingDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string cmdText = @"
        SELECT 
            o.code AS order_code,
            r.name AS room_name, 
            od.number_of_people AS nb_people, 
            o.total_payment,
	        o.discount,
            od.check_in_time, 
            od.check_out_time,
            od.status as trangthai,
            o.id AS order_id
        FROM 
            orders o 
        JOIN contact_management cm ON cm.id = o.custom_id 
        JOIN order_detail od ON o.id = od.order_id 
        JOIN room r ON od.room_id = r.id
        WHERE 
            cm.type = 'customer' AND cm.id = @customerId
        ORDER BY 
            o.code DESC";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    int customerId = Convert.ToInt32(Session["UserId"]);
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    lstBookingDetails.DataSource = reader;
                    lstBookingDetails.DataBind();
                }
            }
        }
        protected void showList_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int orderId = int.Parse(btn.CommandArgument);

            // Lấy thông tin chi tiết đơn đặt phòng từ cơ sở dữ liệu dựa trên orderId
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                    o.code AS order_code,
                    r.name AS room_name, 
                    od.number_of_people AS nb_people, 
                    o.total_payment,
                    o.discount,
                    od.check_in_time, 
                    od.check_out_time,
                    od.status as trangthai,
                    o.id AS order_id,
					rc.price as price
                 FROM 
                    orders o 
                 LEFT JOIN contact_management cm ON cm.id = o.custom_id 
                 JOIN order_detail od ON o.id = od.order_id 
                 LEFT JOIN room r ON od.room_id = r.id
				 JOIN room_type rt ON rt.id = r.type_room_id
				JOIN room_config rc ON rt.id = rc.type_room_id
                 WHERE 
                    o.id = @orderId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Bind dữ liệu vào các điều khiển trong modal
                    lblCheckInTime.Text = reader["check_in_time"] != DBNull.Value ? ((DateTime)reader["check_in_time"]).ToString("HH:mm 'ngày' dd 'tháng' MM 'năm' yyyy") : "N/A";
                    lblCheckOutTime.Text = reader["check_out_time"] != DBNull.Value ? ((DateTime)reader["check_out_time"]).ToString("HH:mm 'ngày' dd 'tháng' MM 'năm' yyyy") : "N/A";
                    lblRoomName.Text = reader["room_name"] != DBNull.Value ? reader["room_name"].ToString() : "N/A";
                    lblOrderCode.Text = reader["order_code"] != DBNull.Value ? reader["order_code"].ToString() : "N/A";
                    lblNbPeople.Text = reader["nb_people"] != DBNull.Value ? reader["nb_people"].ToString() : "N/A";
                    lblmonney.Text = reader["price"] != DBNull.Value ? reader["price"].ToString() + " $" : "N/A";

                    decimal totalPayment = reader["price"] != DBNull.Value ? Convert.ToDecimal(reader["price"]) : 0;
                    decimal discount = reader["discount"] != DBNull.Value ? Convert.ToDecimal(reader["discount"]) : 0;
                    lblTotalPayment.Text = (totalPayment + discount).ToString("N0") + " $";
                    lblDiscount.Text = discount.ToString("N0") + " $";
                }
                conn.Close();
            }

            // Hiển thị modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalCenter').modal('show');", true);
        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32((sender as Button).CommandArgument);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"
            SELECT 
                od.check_in_time,
                od.check_out_time
            FROM 
                orders o
            JOIN order_detail od ON o.id = od.order_id
            WHERE 
                o.id = @orderId";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        DateTime checkInTime = Convert.ToDateTime(reader["check_in_time"]);
                        DateTime currentTime = DateTime.Now;

                        if (checkInTime <= currentTime.AddHours(3))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "showWarningSuadonToast", "showWarningSuadonToast();", true);
                            form_data.Visible = false; // Ensure form is hidden if not allowed
                        }
                        else
                        {
                            bindBookingDetailsToEdit(orderId);
                        }
                    }
                }
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32((sender as Button).CommandArgument);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"
            SELECT 
                od.check_in_time
            FROM 
                orders o
            JOIN order_detail od ON o.id = od.order_id
            WHERE 
                o.id = @orderId";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        DateTime checkInTime = Convert.ToDateTime(reader["check_in_time"]);
                        DateTime currentTime = DateTime.Now;

                        if (checkInTime <= currentTime.AddHours(3))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "showWarningHuydonToast", "showWarningHuydonToast();", true);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "showSuccessHuyBookingToast", "showSuccessHuyBookingToast();", true);
                        }
                    }
                }
            }
        }


        private void bindBookingDetailsToEdit(int id)
        {
            lblSuccess.Visible = false;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
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
                        txtnumofpp.Text = reader["nb_people"].ToString();
                        txtCheckIn.Text = Convert.ToDateTime(reader["check_in_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                        txtCheckOut.Text = Convert.ToDateTime(reader["check_out_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                        ViewState["CurrentOrderID"] = reader["order_id"];
                        ViewState["CurrentOrderDetailID"] = reader["order_detail_id"];

                        form_data.Visible = true;
                        listbookings.Visible = false;
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

            DateTime checkIn = Convert.ToDateTime(txtCheckIn.Text);
            DateTime currentTime = DateTime.Now;

            if (checkIn <= currentTime.AddHours(3))
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Không thể sửa vì quá thời gian để sửa.";
                return;
            }

            int active = 1; // Set this to 1 or 0 based on your logic
                            // Set this to 1 or 0 based on your logic

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int UpdateBy = Convert.ToInt32(Session["AccountId"]);

                        // Update order details
                        string updateOrderQuery = @"UPDATE orders
                                        SET active = @active,
                                            last_update_by = @last_update_by,
                                            last_update_when = @last_update_when
                                        WHERE id = @order_id";

                        using (SqlCommand cmd = new SqlCommand(updateOrderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@active", active);
                            cmd.Parameters.AddWithValue("@last_update_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                            cmd.Parameters.AddWithValue("@order_id", ViewState["CurrentOrderID"]);

                            cmd.ExecuteNonQuery();
                        }

                        // Update order details
                        string updateOrderDetailQuery = @"UPDATE order_detail
                                              SET active = @active,
                                                  number_of_people = @number_of_people,
                                                  check_in_time = @check_in_time,
                                                  check_out_time = @check_out_time,
                                                  last_update_by = @last_update_by,
                                                  last_update_when = @last_update_when
                                              WHERE id = @order_detail_id";

                        using (SqlCommand cmd = new SqlCommand(updateOrderDetailQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@active", active);
                            cmd.Parameters.AddWithValue("@number_of_people", txtnumofpp.Text);
                            cmd.Parameters.AddWithValue("@check_in_time", checkIn);
                            cmd.Parameters.AddWithValue("@check_out_time", Convert.ToDateTime(txtCheckOut.Text));
                            cmd.Parameters.AddWithValue("@last_update_by", UpdateBy);
                            cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                            cmd.Parameters.AddWithValue("@order_detail_id", ViewState["CurrentOrderDetailID"]);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        BindBookingDetails();
                        lblSuccess.Visible = true;
                        lblSuccess.Text = "Cập nhật thông tin đặt phòng thành công";
                        lblSuccess.ForeColor = System.Drawing.Color.Green;
                        form_data.Visible = false;
                        listbookings.Visible = true;
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

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            listbookings.Visible = true;
            form_data.Visible = false;
        }
    }
}