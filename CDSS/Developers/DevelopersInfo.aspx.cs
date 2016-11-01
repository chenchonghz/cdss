using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient; //连接数据库所需语句
using System.Data;
using System.Windows.Forms;
using System.Threading;

//using System.Text.RegularExpressions;
using System.IO;

namespace CDSS
{
    public partial class DevelopersInfo : System.Web.UI.Page
    {
        private static string UserPhotoShow;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection Connection = new SqlConnection(strConnection);
                Connection.Open();
                String strSQL="";
                if (Session["type"].ToString() == "Developer")
                    strSQL = "Select top 1 * From DeveloperInfo where (LoginName=@UserName)";
                else
                {
                    string MessageInfo = "请先登录！";
                    Response.Write("<script>window.alert('" + MessageInfo + "');</script>");
                }
                SqlCommand command = new SqlCommand(strSQL, Connection);
                command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
                SqlDataReader DataReader = command.ExecuteReader();
                while (DataReader.Read())
                {
                    TextBoxLoginName.Text = Session["UserName"].ToString().Trim();
                    TextBoxName.Text = DataReader["Name"].ToString().Trim();
                    TextBoxSex.Text = DataReader["Sex"].ToString().Trim();
                    if (DataReader["Sex"].ToString().Length != 0) { TextBoxDOB.Text = Convert.ToDateTime(DataReader["DOB"]).ToString("yyyy-MM-dd");}
                    TextBoxEmailAddress.Text = DataReader["EmailAddress"].ToString().Trim();
                    TextBoxMobilePhone.Text = DataReader["MobilePhone"].ToString().Trim();
                    TextBoxTelephone.Text = DataReader["Telephone"].ToString().Trim();
                    TextBoxQqNumber.Text = DataReader["QqNumber"].ToString().Trim();
                    TextBoxOccupation.Text = DataReader["Occupation"].ToString().Trim();
                    if(Session["type"].ToString()=="Developer")
                    TextBoxAcademicTitle.Text = DataReader["AcademicTitle"].ToString().Trim();                   
                    TextBoxWorkingUnit.Text = DataReader["WorkingUnit"].ToString().Trim();
                    if (DataReader["Photo"].ToString().Length != 0)
                    {                       
                        ImagePhoto.ImageUrl = DataReader["Photo"].ToString();
                    }               
                    //ImagePhoto.ImageUrl = "~/UserPhoto/" + DataReader["DeveloperUID"].ToString() +Session["type"].ToString()+type;                   
                }
                DataReader.Close();
                Connection.Close();
            }
        }
               
        //打开数据库将更新后的数据写入数据库并刷新页面显示
        protected void ButtonSaveDeveloperInfo_Click(object sender, EventArgs e)
        {     
            //打开数据库
            string username = Session["UserName"].ToString();           
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            Connection.Open();
            string sql = "";
            if (Session["type"].ToString() == "Developer")                     
                sql = "update dbo.DeveloperInfo set LoginName='" + TextBoxLoginName.Text + 
                    "',Photo='" + UserPhotoShow + "',Name='" + TextBoxName.Text + 
                    "',Sex='" + TextBoxSex.Text + "',DOB='" + TextBoxDOB.Text + 
                    "',EmailAddress='" + TextBoxEmailAddress.Text + 
                    "',MobilePhone='" + TextBoxMobilePhone.Text + 
                    "',Telephone='" + TextBoxTelephone.Text + 
                    "',Occupation='" + TextBoxOccupation.Text +
                    "',AcademicTitle='" + TextBoxAcademicTitle.Text +
                    "',WorkingUnit='" + TextBoxWorkingUnit.Text +
                    "',QqNumber='" + TextBoxQqNumber.Text + 
                    "'where LoginName='" + username + "'";            
            else
            {
                string MessageInfo = "抱歉，系统出错！";
                Response.Write("<script>window.alert('" + MessageInfo + "');</script>"); 
                
                Response.Redirect("Default.aspx");
            }              
            SqlCommand cmd = new SqlCommand(sql, Connection);
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    Session["UserName"] = TextBoxLoginName.Text.ToString().Trim();
                    string MessageInfo = "恭喜你，信息修改成功！";
                    Response.Write("<script>window.alert('" + MessageInfo + "');</script>"); 
                    Thread.Sleep(200);
                    Response.Redirect("~/Developers/DevelopersInfo.aspx");//刷新修改信息页面                 
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "<script>confirm('信息修改成功')</script>");                    
                }
            Connection.Close();           
        }

        //头像上载检测
        protected void ButtonImportPhoto_Click(object sender, EventArgs e)
        {           
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            Connection.Open();
             string sql="";
            if(Session["type"].ToString()=="Developer")
                sql = "select top 1 DeveloperUID from dbo.DeveloperInfo where LoginName='"+Session["UserName"]+"'";        
            else
            {
                string MessageInfo = "抱歉，系统出错！";
                Response.Write("<script>window.alert('" + MessageInfo + "');</script>"); 
                Response.Redirect("Default.aspx");
            }
            SqlCommand cmd = new SqlCommand(sql, Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (FileUpload.PostedFile.FileName == "")
            {
                this.Upload_Info.Text = "*请选择上传文件！";
            }
            else
            {
                string fType = FileUpload.PostedFile.ContentType;//获取图像的类型
                if (fType == "image/bmp" ||  fType == "image/jpg" || fType == "image/jpeg" || fType == "image/png")
                {
                    string UserPhotoPath="";                  
                    string filepath = FileUpload.PostedFile.FileName;  //得到的是文件的完整路径,包括文件名，如：D:\QQMiniDL\79931944\ProjCDSS20141026-1810\ProjCDSS\CDSS\UserPhoto\test.jpg                 
                    string type = FileUpload.FileName.Substring(FileUpload.FileName.LastIndexOf("."));
                    while (reader.Read())//从数据库读取信息
                    {         
                        if(Session["type"].ToString()=="Developer")
                        {
                            UserPhotoPath = Server.MapPath("~/UsersPhoto/") + reader["DeveloperUID"].ToString() + Session["type"].ToString() + type;//取得文件在服务器上保存的位置C:\Inetpub\wwwroot\WebSite1\UserPhoto\test.jpg ，使用绝对位置  
                            // if(UserPhotoPath.IndexOf(reader["DeveloperUID"].ToString())==-1)                          
                            UserPhotoShow = "../UsersPhoto/" + reader["DeveloperUID"].ToString() + Session["type"].ToString() + type; //使用相对位置  
                        }
                        else
                        {
                            string MessageInfo = "抱歉，系统出错！";
                            Response.Write("<script>window.alert('" + MessageInfo + "');</script>"); 
                            Response.Redirect("Default.aspx");
                        }
                                                                 
                    }                    
                    FileUpload.PostedFile.SaveAs(UserPhotoPath);//将上传的文件存储到UserPhoto文件夹
                    this.Upload_Info.Text = "上载成功！";
                    ImagePhoto.ImageUrl = UserPhotoShow;
                    reader.Close();               
                }
                else
                {
                    this.Upload_Info.Text = "*图片格式不正确,请上传JPG、PNG、BMP格式";
                }
                Connection.Close();
            }
        }
    }
}