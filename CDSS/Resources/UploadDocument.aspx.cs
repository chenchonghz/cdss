using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient; //连接数据库所需语句
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CDSS
{
    public partial class UploadDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            Connection.Open();
            String strSQL = "";
            if (Session["type"].ToString() == "Developer")
                strSQL = "Select top 1 * From DeveloperInfo where (LoginName=@UserName)";
            else
                Response.Write("<script>window.alert('请先登录！');</script>");
            SqlCommand command = new SqlCommand(strSQL, Connection);
            command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
            Connection.Close();
        }
        protected void ButtonUploadDoc_Click(object sender, EventArgs e)
        {         
            if (FileUploadDoc.HasFile)
            {
                string Describe=TextAreaDocStatus.Value;//填写任务完成情况
                string ProvideName = Session["UserName"].ToString();//填写上传者名字
                if (Describe != "")
                {
                    DateTime Now = DateTime.Now;//获取时间
                    string FileName = FileUploadDoc.PostedFile.FileName;//获取文件名
                    string FileType = FileUploadDoc.PostedFile.ContentType;//获取文件类型
                    string fex = Path.GetExtension(FileName);//获取文件扩展名
                    string filepath = Path.GetFullPath(FileName);//获取文件保存路径
                    int FileSize = FileUploadDoc.PostedFile.ContentLength;//获取文件大小
                    string strSQLE = "server =  localhost; database = CDSS; uid= cdssuser; pwd = cdssuser";
                    SqlConnection conn = new SqlConnection(strSQLE);//连接数据库
                    conn.Open();
                    string sql1 = "select count(*) from WebpageResults where FileName='" + FileName + "'";//在数据表中检测是否已有过文件名
                    SqlCommand cmd1 = new SqlCommand(sql1, conn);
                    cmd1.CommandText = sql1;
                    int count = (int)cmd1.ExecuteScalar();
                    cmd1.ExecuteNonQuery();
                    if (count > 0)
                    {
                        Response.Write("<script>alert('该文件名已存在，请修改文件名后再上传！')</script>");
                        // Response.Redirect("~/Resources/UploadDocument.aspx");//刷新修改信息页面;
                    }
                    else
                    {
                        FileUploadDoc.PostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("/Upload/WorkResults" + "\\" + FileName));//保存文件

                        string strInsert = "insert into dbo.WebpageResults(UploadTime,FileName,UploaderName,ResultStatus,SavedPath) " +
                       "values ('" + Now.ToString() + "','" + FileName + "','" + ProvideName + "','" + Describe + "','" + filepath + "')";

                        SqlCommand myCmd = new SqlCommand(strInsert, conn);
                        myCmd.ExecuteNonQuery();
                        conn.Close();// 关闭数据库

                        Response.Write("<script>window.alert('上传成功！');</script>");
                        //Response.Redirect("~/Resources/UploadDocument.aspx");//刷新修改信息页面;
                    }
                }
                else
                {
                    TextAreaDocStatus.Value = "";
                    Response.Write("<script>window.alert('请填写完成情况！');</script>");
                    return;
                }
            }
            else
            {
                Response.Write("<script>window.alert('请选择文件！');</script>");
            }
            this.Server.Transfer("UploadDocument.aspx");
        }
    }
}