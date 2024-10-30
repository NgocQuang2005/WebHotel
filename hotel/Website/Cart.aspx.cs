using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel.Website
{
    public partial class Cart : System.Web.UI.Page
    {
        private DataTable gioHang;

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadCartFromSession();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCartFromSession();

                if (Request.QueryString["id"] != null)
                {
                    AddCart(Request.QueryString["id"]);
                }
            }
        }
        private void LoadCartFromSession()
        {
            if (Session["GH"] != null)
            {
                gioHang = (DataTable)Session["GH"];
            }
            else
            {
                gioHang = CreateDataTable();
            }

            lstViewCart.DataSource = gioHang;
            lstViewCart.DataBind();
        }
        private void UpdateCartCount()
        {
            if (gioHang != null)
            {
                Session["CartCount"] = gioHang.Rows.Count;
            }
            else
            {
                Session["CartCount"] = 0;
            }
        }

        private void SaveCartToSession()
        {
            Session["GH"] = gioHang;
            UpdateCartCount();
        }

        public DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("STT", typeof(int));
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("TenPhong", typeof(string));
            dataTable.Columns.Add("SL", typeof(int));
            dataTable.Columns.Add("Gia", typeof(decimal));
            dataTable.Columns.Add("tinhtrang", typeof(int));
            return dataTable;
        }

        private DataRow GetRoomDetails(int roomId)
        {
            DataRow roomDetails = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = @"SELECT r.id as idphong, r.state AS tinhtrang, r.name as tenphong , rc.price as sotien
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
        public void AddCart(string id, int quantity = 1)
        {
            if (gioHang == null)
            {
                gioHang = CreateDataTable();
            }

            bool check = false;
            foreach (DataRow dr in gioHang.Rows)
            {
                if (dr["ID"].ToString() == id)
                {
                    dr["SL"] = Convert.ToInt32(dr["SL"]) + quantity;
                    check = true;
                    break;
                }
            }

            if (!check)
            {
                DataRow roomDetails = GetRoomDetails(Convert.ToInt32(id));
                DataRow drNew = gioHang.NewRow();
                drNew["STT"] = gioHang.Rows.Count + 1;
                drNew["ID"] = roomDetails["idphong"];
                drNew["TenPhong"] = roomDetails["tenphong"];
                drNew["SL"] = quantity;
                drNew["Gia"] = roomDetails["sotien"];
                drNew["tinhtrang"] = roomDetails["tinhtrang"];
                gioHang.Rows.Add(drNew);
            }

            lstViewCart.DataSource = gioHang;
            lstViewCart.DataBind();
            SaveCartToSession();

        }

        protected void lstViewCart_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            try
            {
               //load giỏ hàng ra nếu chưa có
                LoadCartFromSession();
                // Lấy ID của mục cần xóa
                int roomId = Convert.ToInt32(lstViewCart.DataKeys[e.ItemIndex].Value);
                //Xóa Khỏi DataTable (gioHang)
                DataRow rowToDelete = gioHang.AsEnumerable().FirstOrDefault(row => row.Field<int>("ID") == roomId);
                if (rowToDelete != null)
                {
                    gioHang.Rows.Remove(rowToDelete);
                    //Gán lại STT(số thứ tự) sau khi xóa hàng
                    for (int i = 0; i < gioHang.Rows.Count; i++)
                    {
                        gioHang.Rows[i]["STT"] = i + 1;
                    }
                    //Liên kết lại ListView với DataTable đã cập nhật
                    lstViewCart.DataSource = gioHang;
                    lstViewCart.DataBind();
                    // Lưu DataTable đã cập nhật và session
                    SaveCartToSession();
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Lỗi: " + ex.Message;
                e.Cancel = true;
            }

        }
        protected void lstViewCart_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            if (e.CommandName == "Delete")
            {
                if (gioHang == null)
                {
                    LoadCartFromSession();
                }

                string roomId = e.CommandArgument.ToString();
                DataRow rowToDelete = gioHang.AsEnumerable().FirstOrDefault(row => row.Field<int>("ID") == Convert.ToInt32(roomId));
                if (rowToDelete != null)
                {
                    gioHang.Rows.Remove(rowToDelete);
                    for (int i = 0; i < gioHang.Rows.Count; i++)
                    {
                        gioHang.Rows[i]["STT"] = i + 1;
                    }
                    SaveCartToSession();
                    lstViewCart.DataSource = gioHang;
                    lstViewCart.DataBind();
                }
            }

        }

        protected void Datphong_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Bạn Cần Đăng Nhập Trước Khi Đặt Phòng!');", true);//gọi trình duyệt alert
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
                foreach (DataRow dr in gioHang.Rows)
                {
                    int roomState = Convert.ToInt32(dr["tinhtrang"]);
                    if (roomState == 1) // Nếu trạng thái phòng là 1, không cho đặt phòng
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "showWarningToast", "showWarningToast();", true);
                        return;
                    }
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

                            string insertOrderQuery = "INSERT INTO orders (code, active, custom_id, total_payment,  created_by, created_when, last_update_by , last_update_when) OUTPUT INSERTED.id VALUES (@code, @active, @custom_id, @total_payment, @created_by, @created_when, @last_update_by , @last_update_when)";
                            int orderId = 0;

                            using (SqlCommand cmd = new SqlCommand(insertOrderQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@code", orderCode);
                                cmd.Parameters.AddWithValue("@active", 1);
                                cmd.Parameters.AddWithValue("@custom_id", Convert.ToInt32(Session["UserId"]));

                                decimal totalPayment = 0;
                                if (gioHang != null && gioHang.Rows.Count > 0)
                                {
                                    totalPayment = gioHang.AsEnumerable().Sum(r => r.Field<decimal>("Gia") * r.Field<int>("SL"));
                                }
                                cmd.Parameters.AddWithValue("@total_payment", totalPayment);
                                cmd.Parameters.AddWithValue("@created_by", UserBy);
                                cmd.Parameters.AddWithValue("@created_when", DateTime.Now);
                                cmd.Parameters.AddWithValue("@last_update_by", UserBy);
                                cmd.Parameters.AddWithValue("@last_update_when", DateTime.Now);

                                orderId = (int)cmd.ExecuteScalar();
                                lblErrorMessage.Visible = true;
                                lblErrorMessage.Text = orderId.ToString();
                            }

                            if (gioHang != null && gioHang.Rows.Count > 0)
                            {
                                string insertOrderDetailQuery = "INSERT INTO order_detail (room_id, order_id, total_amount, number_of_people,status ,check_in_time, check_out_time, created_by, created_when, last_update_by , last_update_when) VALUES (@room_id, @order_id, @total_amount, @number_of_people,@status, @check_in_time, @check_out_time, @created_by, @created_when, @last_update_by , @last_update_when)";

                                foreach (DataRow dr in gioHang.Rows)
                                {
                                    using (SqlCommand cmd = new SqlCommand(insertOrderDetailQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@room_id", dr["ID"]);
                                        cmd.Parameters.AddWithValue("@order_id", orderId);
                                        cmd.Parameters.AddWithValue("@total_amount", dr["Gia"]);
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
                                }
                                string insertStateRoomDetailQuery = @"INSERT INTO room (state ,created_by,created_when, last_update_by, last_update_when) VALUES (@state ,@created_by,@created_when, @last_update_by , @last_update_when)";
                                using (SqlCommand cmd = new SqlCommand(insertStateRoomDetailQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("state", 1);
                                    cmd.Parameters.AddWithValue("created_by", UserBy);
                                    cmd.Parameters.AddWithValue("created_when", DateTime.Now);
                                    cmd.Parameters.AddWithValue("last_update_by", UserBy);
                                    cmd.Parameters.AddWithValue("last_update_when", DateTime.Now);
                                }
                            }

                            transaction.Commit();
                            gioHang.Clear();
                            SaveCartToSession();
                            lstViewCart.DataSource = gioHang;
                            lstViewCart.DataBind();
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
    }
}
