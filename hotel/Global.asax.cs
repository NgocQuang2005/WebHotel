using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                new ScriptResourceDefinition
                {
                    Path = "~/scripts/jquery-1.7.2.min.js",
                    DebugPath = "~/scripts/jquery-1.7.2.js",
                    CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.min.js",
                    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.js"
                });
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "AdminHomeRoute",          // Tên của tuyến đường
                "admin/home/{id}",         // Mẫu URL
                "~/admin/home.aspx",       // Đường dẫn thực tế đến tệp .aspx
                false,
                new RouteValueDictionary { },  // Giá trị mặc định rỗng
                new RouteValueDictionary { { "id", @"\d*" } }
            );
        }
    }
}