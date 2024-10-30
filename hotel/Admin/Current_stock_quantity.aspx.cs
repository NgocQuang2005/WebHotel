using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace hotel.Admin
{
    public partial class Current_stock_quantity : System.Web.UI.Page
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
                bindInventoryDetailsToListView("");
                loadDataStock(SearchStock);
                loadDataWareHouse(SearchWarehouse);
                // Set the value of HiddenField with ClientID of SearchName input
                hfSearchNameClientID.Value = SearchName.ClientID;
            }
        }
        public void loadDataStock(DropDownList doromd)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM stock WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);

                // Mở kết nối
                con.Open();

                // Thực thi truy vấn và liên kết dữ liệu với DropDownList
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    doromd.DataSource = reader;
                    doromd.DataTextField = "name";
                    doromd.DataValueField = "id";
                    doromd.DataBind();
                }

                // Chèn mục mặc định vào DropDownList
                doromd.Items.Insert(0, new ListItem("Chọn loại Hàng", ""));
            }
        }
        public void loadDataWareHouse(DropDownList droie)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //======= Select Query with search parameter.
            string query = "SELECT id  , name FROM warehouse WHERE active = 1 ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo SqlCommand
                SqlCommand cmd = new SqlCommand(query, con);
                // Mở kết nối
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    droie.DataSource = reader;
                    droie.DataTextField = "name";
                    droie.DataValueField = "id";
                    droie.DataBind();
                }
                droie.Items.Insert(0, new ListItem("Chọn Kho", ""));
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the search text from the textbox
            string searchName = SearchName.Text;

            // Call the method to search for rooms
            bindInventoryDetailsToListView(searchName);
        }
        void bindInventoryDetailsToListView(string searchName)
        {
            string condition = "";//1=1
            if (!String.IsNullOrEmpty(searchName))
            {
                condition += "s.name LIKE '%" + searchName + "%'";
            }
            //if (!String.IsNullOrEmpty(SearchActiveDropdown.SelectedValue))
            //{
            //    if (!String.IsNullOrEmpty(condition))
            //    {
            //        condition += " and ";
            //    }
            //    condition += "ied.active = " + SearchActiveDropdown.SelectedValue;
            //}
            if (!String.IsNullOrEmpty(SearchStock.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "s.id = " + SearchStock.SelectedValue;
            }
            if (!String.IsNullOrEmpty(SearchWarehouse.SelectedValue))
            {
                if (!String.IsNullOrEmpty(condition))
                {
                    condition += " and ";
                }
                condition += "w.id = " + SearchWarehouse.SelectedValue;
            }
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //======= Select Query.
            string cmdText = @" 
                        WITH ImportExportSummary AS (
                            SELECT 
                                stock_id,
                                warehouse_id,
                                SUM(CASE WHEN type = 1 THEN stock_quantity ELSE 0 END) AS TotalImport,
                                SUM(CASE WHEN type = 2 THEN stock_quantity ELSE 0 END) AS TotalExport
                            FROM 
                                import_export_detail
                            GROUP BY 
                                stock_id, warehouse_id
                        ),

                        TransferSummary AS (
                            SELECT 
                                stock_id,
                                from_warehouse_id AS warehouse_id,
                                -SUM(quantity) AS TotalTransferred -- Số lượng xuất khỏi kho
                            FROM 
                                stock_transfer
                            GROUP BY 
                                stock_id, from_warehouse_id

                            UNION ALL

                            SELECT 
                                stock_id,
                                to_warehouse_id AS warehouse_id,
                                SUM(quantity) AS TotalTransferred -- Số lượng nhập vào kho
                            FROM 
                                stock_transfer
                            GROUP BY 
                                stock_id, to_warehouse_id
                        ),

                       
                        InventorySummary AS (
                            SELECT 
                                stock_id,
                                warehouse_id,
                                SUM(quantity) AS TotalInventory
                            FROM 
                                stock_inventory
                            GROUP BY 
                                stock_id, warehouse_id
                        )

                        SELECT 
                            s.id AS stock_id,
                            s.name AS stock_name,
                            w.id AS warehouse_id,
                            w.name AS warehouse_name,
                            ISNULL(i.TotalInventory, 0) + 
                            ISNULL(e.TotalImport, 0) - ISNULL(e.TotalExport, 0) + 
                            ISNULL(t.TotalTransferred, 0) AS CurrentQuantity
                        FROM 
                            stock s
                        CROSS JOIN 
                            warehouse w
                        LEFT JOIN 
                            InventorySummary i ON s.id = i.stock_id AND w.id = i.warehouse_id
                        LEFT JOIN 
                            ImportExportSummary e ON s.id = e.stock_id AND w.id = e.warehouse_id
                        LEFT JOIN 
                            TransferSummary t ON s.id = t.stock_id AND w.id = t.warehouse_id";

            if (!String.IsNullOrEmpty(condition))
            {
                cmdText += " WHERE " + condition;
            }

            // Add ORDER BY clause
            cmdText += " ORDER BY s.id, w.id";

            SqlCommand cmd = new SqlCommand(cmdText, con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //===== Execute Query and bind data to ListView.
            lstViewInventoryDetails.DataSource = cmd.ExecuteReader();
            lstViewInventoryDetails.DataBind();
        }


    }
}