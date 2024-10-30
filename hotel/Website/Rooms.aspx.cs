using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace hotel.Website
{
    public partial class Rooms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //string roomTypeId = Request.QueryString["room_type_id"];
            if (!Page.IsPostBack)
            {
                bindRoomDetailsToListView("");
                loadData(SearchRoomType);
            }
        }
        public void loadData(DropDownList dropDownList)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT id, name FROM room_type WHERE active = 1";

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
                dropDownList.Items.Insert(0, new ListItem("Chọn loại phòng", ""));
            }
        }
        
        void bindRoomDetailsToListView(string searchName)
        {
            string condition = "1 = 1";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += " AND r.quantity LIKE @searchSoLuong";
                parameters.Add(new SqlParameter("@searchSoLuong", "%" + searchName + "%"));

            }
            if (!String.IsNullOrEmpty(SearchRoomType.SelectedValue))
            {
                condition += " AND rt.id = @nameroomType";
                parameters.Add(new SqlParameter("@nameroomType", SearchRoomType.SelectedValue));
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {

                //======= Select Query.
                string cmdText = @" SELECT              r.id, r.type_room_id as room_type_id, r.name as name, r.num_floor as num_floor, r.spread as dientich, r.num_bed as num_bed, r.description as description, di.URL as URL, 
                                                        CASE WHEN r.state = 1 THEN 'Đã Được Đặt' WHEN r.state = 0 THEN 'Còn Trống' END AS state , rt.id as idloaiphong , r.quantity, di.activeImg
                                    FROM                room r
                                            LEFT JOIN   document_information di ON r.id = di.room_content 
                                            JOIN        room_type rt ON rt.id = r.type_room_id
                                    WHERE               di.activeImg = 1  AND " + condition;

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
                    lstViewRoomDetails.DataSource = cmd.ExecuteReader();
                    lstViewRoomDetails.DataBind();
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = SearchName.Text;
            bindRoomDetailsToListView(searchName);
        }

    }
}