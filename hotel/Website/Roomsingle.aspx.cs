using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel.Website
{
    public partial class Roomsingle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string roomid = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(roomid))
                {
                    hfRoomId.Value = roomid; // Lưu trữ ID của phòng trong Hidden Field
                    bindRoomDetailsToListView(roomid);
                    bindUrlDetailsToListView(roomid);
                }
            }
        }

        void bindRoomDetailsToListView(string roomid)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"SELECT       r.id, r.type_room_id as room_type_id, r.name as name, r.num_floor as num_floor, r.num_bed as num_bed, rc.price as price, r.description as description 
                                    FROM        room r 
                                        JOIN    room_type rt on rt.id = r.type_room_id 
                                        JOIN    room_config rc on rt.id = rc.type_room_id 
                                    WHERE       r.id = @roomid AND r.active = 1 ";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@roomid", roomid);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        // Bind data to ListView
                        lstViewRoomDetails.DataSource = ds.Tables[0];
                        lstViewRoomDetails.DataBind();
                    }
                }
            }
        }
        void bindUrlDetailsToListView(string roomid)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string cmdText = @"SELECT       r.id,  r.name as name , di.URL
                                    FROM        room r 
                                        JOIN    document_information di on r.id = di.room_content 
                                    WHERE       r.id = @roomid AND r.active = 1 ";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@roomid", roomid);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        // Bind data to ListView
                        lstViewImageSlider.DataSource = ds.Tables[0];
                        lstViewImageSlider.DataBind();
                    }
                }
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
            string query = @"SELECT     r.id as idphong, r.state as tinhtrang, r.name as tenphong , rc.price as sotien
                            FROM        room r
                                JOIN    room_type rt on rt.id = r.type_room_id
                                JOIN    room_config rc on rc.type_room_id = rt.id
                            WHERE       r.id = @roomId AND r.active = 1 ";

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

        protected void Datphong_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null )
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Bạn Cần Đăng Nhập Trước Khi Đặt Phòng!');", true);
            }
            else
            {
                if (string.IsNullOrEmpty(txtNumOfPeople.Text))
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

                int roomId = int.Parse(hfRoomId.Value); // Lấy ID của phòng từ Hidden Field
                DataRow dr = GetRoomDetails(roomId);
                if (dr == null)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Không tìm thấy thông tin phòng.";
                    return;
                }
                int roomState = Convert.ToInt32(dr["tinhtrang"]);
                if (roomState == 1) // Nếu trạng thái phòng là 1, không cho đặt phòng
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showWarningToast", "showWarningToast();", true);
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
                            int UserBy = Convert.ToInt32(Session["AccountId"]);
                            string checkAccountQuery = "SELECT COUNT(1) FROM account WHERE id = @id";
                            using (SqlCommand checkCmd = new SqlCommand(checkAccountQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@id", UserBy);
                                int exists = (int)checkCmd.ExecuteScalar();
                                if (exists == 0)
                                {
                                    lblSuccess.Visible = true;
                                    lblSuccess.Text = "Vui lòng kiểm tra lại AccountId.";
                                    transaction.Rollback();
                                    return;
                                }
                            }
                            string orderCode = GetNextOrderCode();

                            string insertOrderQuery = "INSERT INTO orders (code, active, custom_id, total_payment, created_by, created_when, last_update_by , last_update_when) OUTPUT INSERTED.id VALUES (@code, @active, @custom_id, @total_payment, @created_by, @created_when, @last_update_by , @last_update_when)";
                            int orderId = 0;

                            using (SqlCommand cmd = new SqlCommand(insertOrderQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@code", orderCode);
                                cmd.Parameters.AddWithValue("@active", 1);
                                cmd.Parameters.AddWithValue("@custom_id", Convert.ToInt32(Session["UserId"]));

                                DateTime checkIn = Convert.ToDateTime(txtCheckIn.Text);
                                DateTime checkOut = Convert.ToDateTime(txtCheckOut.Text);
                                decimal totalPayment = (checkOut - checkIn).Days * Convert.ToDecimal(dr["sotien"]);

                                cmd.Parameters.AddWithValue("@total_payment", totalPayment);
                                cmd.Parameters.AddWithValue("@created_by", UserBy);
                                cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                                cmd.Parameters.AddWithValue("@last_update_by", UserBy);
                                cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

                                orderId = (int)cmd.ExecuteScalar();
                                lblErrorMessage.Visible = true;
                                lblErrorMessage.Text = orderId.ToString();
                            }

                            string insertOrderDetailQuery = "INSERT INTO order_detail (room_id, order_id, total_amount, number_of_people, status, check_in_time, check_out_time, created_by, created_when, last_update_by , last_update_when) VALUES (@room_id, @order_id, @total_amount, @number_of_people,@status, @check_in_time, @check_out_time, @created_by, @created_when, @last_update_by , @last_update_when)";

                            using (SqlCommand cmd = new SqlCommand(insertOrderDetailQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@room_id", roomId);
                                cmd.Parameters.AddWithValue("@order_id", orderId);
                                cmd.Parameters.AddWithValue("@total_amount", dr["sotien"]);
                                cmd.Parameters.AddWithValue("@number_of_people", txtNumOfPeople.Text);
                                cmd.Parameters.AddWithValue("@status", 1);
                                cmd.Parameters.AddWithValue("@check_in_time", Convert.ToDateTime(txtCheckIn.Text));
                                cmd.Parameters.AddWithValue("@check_out_time", Convert.ToDateTime(txtCheckOut.Text));
                                cmd.Parameters.AddWithValue("@created_by", UserBy);
                                cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                                cmd.Parameters.AddWithValue("@last_update_by", UserBy);
                                cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);
                                cmd.ExecuteNonQuery();
                            }
                            string insertStateRoomDetailQuery = @"INSERT INTO room (state ,created_by,created_when, last_update_by, last_update_when) VALUES (@state ,@created_by,@created_when, @last_update_by , @last_update_when)";
                            using(SqlCommand cmd = new SqlCommand(insertStateRoomDetailQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("state", 1);
                                cmd.Parameters.AddWithValue("created_by", UserBy);
                                cmd.Parameters.AddWithValue("created_when", DateTime.Now);
                                cmd.Parameters.AddWithValue("last_update_by", UserBy);
                                cmd.Parameters.AddWithValue("last_update_when",DateTime.Now);
                            }
                            transaction.Commit();
                            ClientScript.RegisterStartupScript(this.GetType(), "showSuccessToast", "showSuccessToast();", true);

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error processing order. See inner exception for details.", ex);
                        }
                    }
                }
            }
        }
    }
}
