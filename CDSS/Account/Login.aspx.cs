using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Threading;
using System.Web.Security;
namespace CDSS.Account
{  
    public partial class Login : Page
    {
        string LoginErrorInfo = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];

            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        private bool SiteLevelCustomAuthenticationMethod(string UserName, string CurPassword, string type)
        {
            string strPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(CurPassword, "MD5");//密码加密生成哈希密码
            bool boolReturnValue = false ;
            LoginErrorInfo = "不明原因";
            string UserType = "";
            // Insert code that implements a site-specific custom 
            // authentication method here.
            // This example implementation always returns false.
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            String strSQL="";
             if(type=="Commonuser")
             {
                 strSQL = "Select LoginPWD From CommonuserInfo where (LoginName=@UserName)";
                 UserType = "普通用户";
             }
             else if(type=="Doctor")
             {
                 strSQL = "Select LoginPWD From DoctorInfo where (LoginName=@UserName)";
                 UserType = "医生用户";
             }
             else if (type == "Developer")
             {
                 strSQL = "Select LoginPWD From DeveloperInfo where (LoginName=@UserName)";
                 UserType = "开发人员用户";
             }
             else UserType = "类型错误";

             if (UserType == "类型错误")
             {
                 LoginErrorInfo = "用户类型错误";
             }
             else
             {
                SqlCommand command =new SqlCommand(strSQL, Connection);
                command.Parameters.AddWithValue("@UserName", UserName);
                Connection.Open();
                SqlDataReader DataReader = command.ExecuteReader();
                string SavedPWD = "";
                int RowCount=0;
                while (DataReader.Read()) 
                {
                    SavedPWD = Convert.ToString(DataReader["LoginPWD"]);
                    RowCount++;
                }
                if (RowCount==0)
                {
                    LoginErrorInfo = UserType + "中没有发现注册为【" + UserName + "】的用户"; 
                }
                else if (RowCount > 1)
                {
                    LoginErrorInfo = UserType + "中注册了多个用户名【" + UserName + "】的用户";
                }
                else
                {                    
                    if (SavedPWD != strPwd)
                    {
                        LoginErrorInfo = "密码输入错误";
                    }
                    else
                    {
                        LoginErrorInfo = "";
                        boolReturnValue = true;
                        Session["type"] = type;
                    }
                }
                DataReader.Close();
                Connection.Close();
             }
             return boolReturnValue;
        }

         protected void ButtonLogin_Click(object sender, EventArgs e)
         {
             var userchoose = LoginMain.FindControl("UserChoose") as RadioButtonList;
             bool Authentication = false;
             if (userchoose.SelectedIndex == 0)
             {
                 Authentication = SiteLevelCustomAuthenticationMethod(LoginMain.UserName, LoginMain.Password, "Commonuser");
             }
             else if (userchoose.SelectedIndex == 1)
             {
                 Authentication = SiteLevelCustomAuthenticationMethod(LoginMain.UserName, LoginMain.Password, "Doctor");
             }
             else if (userchoose.SelectedIndex == 2)
             {
                 Authentication = SiteLevelCustomAuthenticationMethod(LoginMain.UserName, LoginMain.Password, "Developer");
             }
             else
             {
                 string MsgInfo = "请选择你的身份重新登录！";
                 Response.Write("<script>window.alert('" + MsgInfo + "');</script>");

             }
             //e.Authenticated = Authenticated;
             if (Authentication == true)
             {
                 Session["UserName"] = LoginMain.UserName;
                 Session["LoginTime"] = DateTime.Now.ToString();
                 FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), createPersistentCookie: false);
                 SaveUserLoginInfo(); //string ip = Request.UserHostAddress;
                 if (Session["type"].ToString() == "Developer")
                     Response.Redirect("~/DevelopersPage.aspx");
                 else if (Session["type"].ToString() == "Doctor")
                     Response.Redirect("~/DoctorsPage.aspx");
                 else
                     Response.Redirect("~/CommonUsersPage.aspx");
             }
             else//如果登录失败就让用户重新登录
             {
                 Session["UserName"] = "";
                 Session["type"] = "";
                 Session["LoginTime"] = "";
                 string ErrorMessage = LoginErrorInfo + ",登录失败，请重新登录！";
                 Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
                 //Thread.Sleep(200);
                 //Response.Redirect("~/Account/Login.aspx");
             }      
         }
        private void SaveUserLoginInfo()
        {
            var userchoose = LoginMain.FindControl("UserChoose") as RadioButtonList;
            string UserType = "";
            if (userchoose.SelectedIndex == 0)
            {
                UserType = "普通用户";
            }
            else if (userchoose.SelectedIndex == 1)
            {
                UserType = "医生用户";
            }
            else if (userchoose.SelectedIndex == 2)
            {
                UserType = "开发人员";
            } 
            string LoginIP = Request.UserHostAddress;
            string LoginName = LoginMain.UserName;
            string LoginTime = DateTime.Now.ToString();

            string conString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            string SqlStr="INSERT into UsersLogin (LoginTime,LoginName,UserType,LoginIP) "+
                "VALUES(@LoginTime,@LoginName,@UserType,@LoginIP)";;

            SqlCommand cmd= new SqlCommand(SqlStr, con);;

            cmd.Parameters.AddWithValue("@LoginTime", LoginTime);
            cmd.Parameters.AddWithValue("@LoginName", LoginName);
            cmd.Parameters.AddWithValue("@UserType", UserType);
            cmd.Parameters.AddWithValue("@LoginIP", LoginIP);         
            con.Open();
            cmd.ExecuteNonQuery();              
        } 
         protected void Password_TextChanged(object sender, EventArgs e)
         {

         }        
    }
}