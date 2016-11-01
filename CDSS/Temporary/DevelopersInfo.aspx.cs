using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient; //连接数据库所需语句
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace CDSS
{
    public partial class DevelopersInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public static string UserPhotoPath;
        public static string UserPhotoShow;
        public string loginname;
        protected void ButtonSaveDeveloperInfo_Click(object sender, EventArgs e)
        {
            bool regflag = true;//定义一个注册标签，如果值为false说明不能注册，true就可以注册
           
            //打开数据库
            string strConn = "server =  202.38.78.73; database = CDSS; uid= cdssuser; pwd = cdssuser";
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
           
            //判断修改的登录名有效性
            
                if(TextBoxLoginName.Text.Length==0)
                {
                    LabelLoginName.Text = "";
                    loginname = "zdustc1";
                }
                else if (!Regex.IsMatch(TextBoxLoginName.Text, @"^[A-Za-z0-9]+$"))
                {
                    LabelLoginName.Text = "*请输入字母或数字";
                    regflag = false;                
                }
                else if ((TextBoxLoginName.Text.Length < 4) | (TextBoxLoginName.Text.Length > 12))
                {
                    LabelLoginName.Text = "*登录名长度不正确";
                    regflag = false;            
                }
                else
                {
                    string sql = "select LoginName from dbo.Developerinfo where LoginName='" + TextBoxLoginName.Text + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["LoginName"].ToString() == TextBoxLoginName.Text)
                        {
                            LabelLoginName.Text = "*登录名已经存在";
                            regflag = false;
                        }
                        else
                        {
                            loginname = TextBoxLoginName.Text;
                            LabelLoginName.Text = "";
                        }
                    }              
                    reader.Close();
                }
            


            //判断真实姓名的有效性，检测输入是否为汉字
           if(TextBoxName.Text.Length==0)
           {
               LabelName.Text = "*真实姓名不能为空";
               regflag = false;
           }
           else if (!Regex.IsMatch(TextBoxName.Text, @"^[\u4e00-\u9fa5]+$"))
           {
              
                LabelName.Text = "*请输入汉字";
                regflag = false;        
           }
           else
           {
                LabelName.Text = "";                   
            }
            
          
            

            //判断性别输入的有效性
            if (TextBoxSex.Text.Length==0)
            {
               LabelSex.Text = "*性别不能为空";
               regflag = false;
            }
            else if(TextBoxSex.Text.Trim().ToString()=="男"|TextBoxSex.Text.Trim().ToString()=="女")
            {
                LabelSex.Text = "";             
            }
            else
            {
                LabelSex.Text = "*输入性别不正确";
               regflag = false;

            }
    

            //判断出生日期输入格式是否正确
            if (TextBoxDOB.Text.Length == 0)
            {
                LabelDOB.Text = "*出生日期不能为空";
                regflag = false;
            }
           else if( !Regex.IsMatch(TextBoxDOB.Text, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$"))
           {
               LabelDOB.Text = "*格式不正确,正确示例:1990-01-01";
               regflag = false;
           }
           else
           {
               LabelDOB.Text = "";
           }
             
            //判断电子邮箱格式是否正确
            if (TextBoxEmailAddress.Text.Length == 0)
            {
                LabelEmailAddress.Text = "*邮箱不能为空";
                regflag = false;
            }
            else if (!Regex.IsMatch(TextBoxEmailAddress.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                LabelEmailAddress.Text = "*邮箱格式不正确";
                regflag = false;
            }
            else
            {
                LabelEmailAddress.Text = "";
            }

            //判断移动电话的格式是否正确
            if (TextBoxMobilePhone.Text.Length == 0)
            {
                LabelMobilePhone.Text = "";
            }
            else if (!Regex.IsMatch(TextBoxMobilePhone.Text, @"^[1]+[3,5]+\d{9}"))
            {
                LabelMobilePhone.Text = "*移动电话格式不正确";
                regflag = false;
            }
            else
            {
                LabelMobilePhone.Text = "";
            }
        

            //判断固定电话的格式是否正确，暂时只能
            if (TextBoxTelephone.Text.Length == 0)
            {
                LabelTelephone.Text = "";
            }
            else if (!Regex.IsMatch(TextBoxTelephone.Text, @"^[0-9]*$"))
            {
                LabelTelephone.Text = "*固定电话格式不正确";
                regflag = false;
            }
            else
            {
                LabelTelephone.Text = "";
            }

            //判断职业输入是否正确
            if (TextBoxOccupation.Text.Length == 0)
            {
                LabelOccupation.Text = "";
            }
            else if (!Regex.IsMatch(TextBoxOccupation.Text, @"^[\u4e00-\u9fa5]+$"))
            {
                LabelOccupation.Text = "*请输入正确的职业";
                regflag = false;
            }
            else
            {
                LabelOccupation.Text = "";
            }

            //判断技术职称输入是否正确
            if (TextBoxAcademicTitle.Text.Length == 0)
            {
                LabelAcademicTitle.Text = "";
            }
            else if (!Regex.IsMatch(TextBoxAcademicTitle.Text, @"^[\u4e00-\u9fa5]+$"))
            {
                LabelAcademicTitle.Text = "*请输入正确的技术职称";
                regflag = false;
            }
            else
            {
                LabelAcademicTitle.Text = "";
            }
            //判断工作单位输入是否正确
            if (TextBoxWorkingUnit.Text.Length == 0)
            {
                LabelWorkingUnit.Text = "";
            }
            else if (!Regex.IsMatch(TextBoxWorkingUnit.Text, @"^[\u4e00-\u9fa5]+$"))
            {
                LabelWorkingUnit.Text = "*请输入正确的工作单位名称";
                regflag = false;
            }
            else
            {
                LabelWorkingUnit.Text = "";
            }

            //向数据库中写入更新数据,因为此时没有确定登陆者名称，应该是session（"name"）
            if(regflag==true)//判断regflag是否为true，是就可以写入
            {
                //Upload_Info.Text = UserPhotoPath;
                string sql = "update dbo.Developerinfo set LoginName='"+loginname+"',Photo='" + UserPhotoPath + "',Name='" + TextBoxName.Text + "',Sex='" + TextBoxSex.Text + "',DOB='" + TextBoxDOB.Text + "',EmailAddress='" + TextBoxEmailAddress.Text + "',MobilePhone='" + TextBoxMobilePhone.Text + "',Telephone='" + TextBoxTelephone.Text + "',Occupation='" + TextBoxOccupation.Text + "',AcademicTitle='" + TextBoxAcademicTitle.Text + "',WorkingUnit='" + TextBoxWorkingUnit.Text + "'where LoginName='zdustc1'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int i = cmd.ExecuteNonQuery();
                if(i!=0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "<script>confirm('信息修改成功')</script>");
                    //Response.Redirect("DeveloperInfo.cs");//此处可以定义信息修改成功后转向的页面
                }
                conn.Close();
            }
        }


        //头像上传检测
        protected void ButtonImportPhoto_Click(object sender, EventArgs e)
        {
            string strConn = "server =  202.38.78.73; database = CDSS; uid= cdssuser; pwd = cdssuser";
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            string sql = "select top 1 uid from dbo.Developerinfo where LoginName='zdustc1'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (FileUpload.PostedFile.FileName == "")
            {
                this.Upload_Info.Text = "*请选择上传文件！";
            }
            else
            {
                string fType = FileUpload.PostedFile.ContentType;//获取图像的类型
                if (fType == "image/bmp" || fType == "image/gif" || fType == "image/jpg" || fType == "image/jpeg" || fType == "image/png")
                {
                    string filepath = FileUpload.PostedFile.FileName;  //得到的是文件的完整路径,包括文件名，如：D:\QQMiniDL\79931944\ProjCDSS20141026-1810\ProjCDSS\CDSS\UserPhoto\test.jpg                 
                    string type = FileUpload.FileName.Substring(FileUpload.FileName.LastIndexOf("."));
                    while (reader.Read())//从数据库读取信息
                    {
                        // 此处应该使用session（“name”）
                        UserPhotoPath = Server.MapPath("~/UserPhoto/") + reader["uid"].ToString() + type;//取得文件在服务器上保存的位置C:\Inetpub\wwwroot\WebSite1\UserPhoto\test.jpg ，使用绝对位置
                        UserPhotoShow = "../UserPhoto/" + reader["uid"].ToString() + type; //使用相对位置
                    }
                    reader.Close();
                    FileUpload.PostedFile.SaveAs(UserPhotoPath);//将上传的文件存储到UserPhoto文件夹
                    this.Upload_Info.Text = "上载成功！";
                    ImagePhoto.ImageUrl = UserPhotoShow;     
                    
                }
                else
                {
                    this.Upload_Info.Text = "*图片格式不正确";
                }
            }
        }
    }
}