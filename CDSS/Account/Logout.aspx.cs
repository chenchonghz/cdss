using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;


namespace CDSS.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            UpdateLogoutTime();
            Session["UserName"] = "";
            Session["type"] = "";
            Session["LoginTime"]="";
            Session.Abandon();
            Response.Redirect("~/");
        }
        private void UpdateLogoutTime()
        {
            string LogoutTime=DateTime.Now.ToString();
            string LoginName=Session["UserName"].ToString();
            string LoginTime=Session["LoginTime"].ToString();

            if (LoginName != "")
            {
                string conString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(conString);
                string SqlStr = "Update UsersLogin set Logouttime = @LogoutTime where (LoginName=@LoginName) and (LoginTime=@LoginTime)"; ;

                SqlCommand cmd = new SqlCommand(SqlStr, con); ;

                cmd.Parameters.AddWithValue("@LogoutTime", LogoutTime);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                cmd.Parameters.AddWithValue("@LoginTime", LoginTime);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }                       
        }
    }
}