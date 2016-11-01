using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient; //连接数据库所需语句
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;

namespace CDSS
{
    public partial class UploadResource : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
     

        protected void ButtonUploadRes_Click(object sender, EventArgs e)
        {
            
        

            if (TextBoxResTitle.Text == "")
            {
                Response.Write("<script>alert('文件名不能为空!');</script>");
            }
            else if (!FileUploadRes.HasFile)
            {
                Response.Write("<script>alert('请上传文件！');</script>");
            }
            else
            {

                string title = TextBoxResTitle.Text;
                string count1 = TextBoxResIntro.Text;
                DateTime time = DateTime.Now;
                //string name = "System.Data.Sqlclient";
                
                //string name = FileUploadRes.ClientID;
                //TextBox1.Text = name;
                string fName = FileUploadRes.FileName;
                string FileType = Path.GetExtension(fName);
                string FilePath = FileUploadRes.PostedFile.FileName;
               
                FileUploadRes.PostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("../Upload/Resources") + "\\" + fName);
                

               

                //数据库连接部分   
               
                string strConn = "server =  localhost; database = CDSS; uid= cdssuser; pwd = cdssuser";
                SqlConnection conn = new SqlConnection(strConn);
                
                conn.Open();
               
              

                string sql1 = "select count(*) from GroupResources where ResTitle='" + title + "'";
                    SqlCommand cmd1 = new SqlCommand(sql1, conn);
                    cmd1.CommandText = sql1;
                    int count = (int)cmd1.ExecuteScalar();
                    cmd1.ExecuteNonQuery();
                    if (count > 0)
                    {
                        Response.Write("<script>alert('标题已经存在！')</script>");
                       
                    }
                    else
                    {
                        string name = Session["UserName"].ToString();
                        string sql = "insert into dbo.GroupResources(ResTitle,ResIntroduction,SavedPath,SavedFilename,ProvideTime)" + "values('" + title + "','" + count1 + "','" + FilePath + "','" + fName + "','" + time + "')";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }


                    //SqlCommand cmd1 = new SqlCommand(sql, conn);
                    
                    
                //}
                
             
               
                 

             



              



            }
       
       
           
        }

        protected void GridViewResList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
      

       
        
        

        
    }
}