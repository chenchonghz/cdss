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

        private string SiteLevelCustomAuthenticationMethod(string UserName, string CurPassword, string type)
        {
            string strPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(CurPassword, "MD5");//密码加密生成哈希密码
            //bool boolReturnValue = false ;
            string ReturnValue = "不明原因";
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
             else if (type == "Developers")
             {
                 strSQL = "Select LoginPWD From DeveloperInfo where (LoginName=@UserName)";
                 UserType = "开发人员用户";
             }
             else UserType = "类型错误";

             if (UserType == "类型错误")
             {
                 ReturnValue = "用户类型错误";
             }
             else
             {
                SqlCommand command =new SqlCommand(strSQL, Connection);
                command.Parameters.AddWithValue("@UserName", UserName);
                SqlDataReader DataReader;
                Connection.Open();
                DataReader = command.ExecuteReader();
                int RowCount=0;
                while (DataReader.Read()) {RowCount++;}
                if (RowCount==0)
                {
                    ReturnValue =UserType+ "中没有发现注册为【" + UserName+"】的用户"; 
                }
                else if (RowCount > 1)
                {
                    ReturnValue = UserType + "中注册了多个用户名【" + UserName + "】的用户";
                }
                else
                {
                    string SavedPWD = DataReader[0].ToString();
                    if (SavedPWD != strPwd)
                    {
                        ReturnValue = "密码输入错误";
                    }
                    else
                    { 
                        ReturnValue = "";
                        Session["type"] = type;
                    }
                }
                DataReader.Close();
                Connection.Close();
             }
             return ReturnValue;
        }

         protected void ButtonLogin_Click(object sender, EventArgs e)
         {
             var userchoose = LoginMain.FindControl("UserChoose") as RadioButtonList;
             string Authentication = "";
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
                 MessageBox.Show("请选择你的身份重新登录！");
             }
             //e.Authenticated = Authenticated;
             if (Authentication == "") //登录成功
             {
                 Session["UserName"] = LoginMain.UserName;
                 FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), createPersistentCookie: false);
                 if (Session["type"].ToString() == "Developer")
                     Response.Redirect("~/DevelopersPage.aspx");
                 else if (Session["type"] == "Doctor")
                     Response.Redirect("~/DoctorsPage.aspx");
                 else if (Session["type"] == "CommonUser")
                     Response.Redirect("~/CommonUsersPage.aspx");
             }
             else//如果登录失败就让用户重新登录
             {
                 Session["type"] = "";
                 Session["UserName"] = "";
                 string ErrorMessage = Authentication + ",登录失败！";
                 //Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
                 //LoginMain.FailureText = ErrorMessage;
                 
                 //this.LoginMain_LoginError(sender,e);
                 //MessageBox.Show(ErrorMessage);
                 Thread.Sleep(200);
                 Response.Redirect("~/Account/Login.aspx");
             }      
         }

         protected void Password_TextChanged(object sender, EventArgs e)
         {

         }

         protected void LoginMain_LoginError(object sender, EventArgs e)
         {
             //MessageBox.Show(LoginMain.FailureText);
         }        
    }
}