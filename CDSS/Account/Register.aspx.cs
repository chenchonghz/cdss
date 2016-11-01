using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
namespace CDSS.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            string CurUserName=UserName.Text;
            string CurPassword = Password.Text;
            string CurEmail = Email.Text;
            int CurUserSelect=RadioButtonListUserChoose.SelectedIndex;
            string UserType = "";

            if (CurUserSelect == 0)
            {
                UserType = "Commonuser";
            }
            else if (CurUserSelect == 1)
            {
                UserType = "Doctor";
            }
            else if (CurUserSelect == 2)
            {
                UserType = "Developer";
            }
            else
            {
                Response.Write("<script>window.alert('请选择用户类型！');</script>");
                Response.Redirect("Default.aspx");
            };

            if (CurrentUserExist(CurUserName, UserType))
            {
                string ErrorMessage="用户名【"+CurUserName+"】已经存在，无法在同一用户群注册相同用户名！";
                Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
                Response.Redirect("Default.aspx");
            };
            bool Authenticated = CreateUserProfile(CurUserName, CurPassword, CurEmail, UserType);
            FormsAuthentication.SetAuthCookie(CurUserName, createPersistentCookie: false);
            //string continueUrl = RegisterUser.ContinueDestinationPageUrl;            
            if (Authenticated == true)
            {
                string MsgInfo = "注册成功！";
                Response.Write("<script>window.alert('" + MsgInfo + "');</script>");
                Session["UserName"] = CurUserName;
                Thread.Sleep(200);
                if(Session["type"].ToString()=="Developer")
                    Response.Redirect("~/DevelopersPage.aspx");
                else if (Session["type"].ToString()=="Doctor")
                    Response.Redirect("~/DoctorsPage.aspx");
                else if (Session["type"].ToString() == "Commonuser")
                    Response.Redirect("~/CommonUsersPage.aspx");
                else
                {
                    MessageBox.Show("请重新注册");
                    Response.Redirect("../Account/Register.aspx");
                }
            }
            
        }
        private bool CurrentUserExist(string CurUserName, string UserType)
        {
            bool boolReturnValue = false;
            string conString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd;
            if (UserType == "Commonuser")
            {
                cmd = new SqlCommand("Select LoginName from CommonUserInfo where LoginName=@CurUserName", con);
            }
            else if (UserType == "Doctor")
            {
                cmd = new SqlCommand("Select LoginName from DoctorInfo where LoginName=@CurUserName", con); 
            }
            else
            {
                cmd = new SqlCommand("Select LoginName from DeveloperInfo where LoginName=@CurUserName", con); 
            }
            cmd.Parameters.AddWithValue("@CurUserName", CurUserName);
            con.Open();
            SqlDataReader DataReader = cmd.ExecuteReader();
            if (DataReader.HasRows)
            {
                boolReturnValue = true;
            }
            return boolReturnValue;
        }
        private bool CreateUserProfile(string CurUserName, string CurPassword, string CurEmail, string type)
        {
            string CurTime = DateTime.Now.ToString();
            string strPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Text, "MD5");//密码加密生成哈希密码
            bool boolReturnValue = false;
            string conString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd;
            if(type=="Commonuser")
            {
                cmd = new SqlCommand("INSERT into dbo.CommonUserInfo (LoginName,LoginPWD,EmailAddress,RegisterTime) VALUES(@CurUserName,@strPwd,@CurEmail,@CurTime)", con);
            }
            else if(type=="Doctor")
            {
                cmd = new SqlCommand("INSERT into dbo.DoctorInfo (LoginName,LoginPWD,EmailAddress,RegisterTime) VALUES(@CurUserName,@strPwd,@CurEmail,@CurTime)", con);
            }
            else
            {
                cmd = new SqlCommand("INSERT into dbo.DeveloperInfo (LoginName,LoginPWD,EmailAddress,RegisterTime) VALUES(@CurUserName,@strPwd,@CurEmail,@CurTime)", con);
            }
            cmd.Parameters.AddWithValue("@CurUserName", CurUserName);
            cmd.Parameters.AddWithValue("@strPwd", strPwd);
            cmd.Parameters.AddWithValue("@CurEmail", CurEmail);
            cmd.Parameters.AddWithValue("@CurTime", CurTime);         
            con.Open();
            int i=cmd.ExecuteNonQuery();   
            if(i!=0)
            {
                boolReturnValue = true;
                Session["type"] = type;
                Session["UserName"] = CurUserName;
                Session["LoginTime"] = CurTime;
            }
            else
            {
                Session["type"] = "";
                string MsgInfo = "注册失败，请重新注册！";
                Response.Write("<script>window.alert('" + MsgInfo + "');</script>");
                Thread.Sleep(200);
                Response.Redirect("<script language=javascript>history.go(-2);</script>");
                //Response.Redirect("~/Account/Register.aspx");
            }
            con.Close();
            return boolReturnValue;
        }
    }
}