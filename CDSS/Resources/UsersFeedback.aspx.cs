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
    public partial class UsersFeedback : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
          //string strSQL = "server =  localhost; database = CDSS; uid= cdssuser; pwd = cdssuser";
            //SqlConnection conn = new SqlConnection(strSQL);//连接数据库
           // conn.Open();
   
          // string sql = "select * from dbo.UsersFeedback";
            //SqlDataAdapter da = new SqlDataAdapter(sql, conn);
          //  DataSet ds = new DataSet();
           // da.Fill(ds, "dbo.UsersFeedback"); //填充数据到DataSet中
           // GridViewFeedList.DataSource = new DataView(ds.Tables[0]);
           // GridViewFeedList.DataBind(); //讲信息绑定到GridView
           
            
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            Connection.Open();
            //String strSQL = "";
            /*if (Session["type"].ToString() == "Developer")
                strSQL = "Select top 1 * From DeveloperInfo where (LoginName=@UserName)";
            else
                Response.Write("<script>window.alert('请先登录！');</script>");*/
            string sql = "select * from dbo.UsersFeedback";
            SqlDataAdapter da = new SqlDataAdapter(sql,Connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "dbo.UsersFeedback"); //填充数据到DataSet中
            GridViewFeedList.DataSource = new DataView(ds.Tables[0]);
            GridViewFeedList.DataBind(); //将信息绑定到GridView

            //SqlCommand command = new SqlCommand(strSQL, Connection);
            //command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
            //Connection.Close();
           
        }

        protected void ButtonUploadFeed_Click(object sender, EventArgs e)
        {
            string strSQL = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strSQL);//连接数据库
            conn.Open();

           // string sql_string;
            if (ButtonUploadFeed.Text == "提交您的建议")
            {
                string content = TextAreaFeedContent.Value;
                string title = TextBoxFeedTitle.Text;
                string ProviderName = Session["UserName"].ToString();//填写上传者名字

                if (title!= "" && content!= "")
                {
                    DateTime tt = System.DateTime.Now;
                   // sql_string = "insert into dbo.UsersFeedback(ProviderTime,FeedTitle,FeedContent) values ('" + tt.ToString() + "','" + title + "','" + content + "')";


                    string strInsert = "insert into dbo.UsersFeedback(ProviderTime,ProviderName,FeedTitle,FeedContent) " +
                       "values ('" + tt.ToString() + "','" + ProviderName + "','" + title + "','" + content + "')";

                    SqlCommand myCmd = new SqlCommand(strInsert, conn);
                    myCmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    title = "";
                    content = "";
                    Response.Write("<script>window.alert('请填写完整！！');</script>");
                    return;
                }
            }
     
            this.Server.Transfer("UsersFeedback.aspx");
        }

       
    }
}