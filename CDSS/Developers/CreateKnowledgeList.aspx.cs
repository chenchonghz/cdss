using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient; //连接数据库所需语句
using System.Data;
using System.Web.Configuration;
//using System.Windows.Forms; //弹窗提示MessageBox.Show所需语句

namespace CDSS
{
    public partial class CreateKnowledgeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection Conn = new SqlConnection(strConn);
                Conn.Open();
                if (Session["type"].ToString() == "Developer")
                {

                }
                else
                {
                    Response.Write("<script>alert('请先登录！')</script>");
                    Response.Redirect("Default.aspx");
                }
                Conn.Close();
            }
        }
        protected void ListBoxDiseaseChineseNameSelectedIndexChanged(object sender, EventArgs e) //选择ListBoxDiseaseChineseName中某一项时激发
        {
            this.ListBoxDiseaseEnglishName.SelectedIndex = this.ListBoxDiseaseChineseName.SelectedIndex; //选中对应的英文名
            this.DropDownListSystemType.Text = ListBoxSystemType.Text;
            this.TextBoxChineseName.Text = ListBoxDiseaseChineseName.Text.ToString().Trim();  //TextBox中的值也发生变化
            this.TextBoxEnglishName.Text = ListBoxDiseaseEnglishName.Text.ToString().Trim();
        }
        protected void ListBoxDiseaseEnglishNameSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ListBoxDiseaseChineseName.SelectedIndex = this.ListBoxDiseaseEnglishName.SelectedIndex;
            this.DropDownListSystemType.Text = ListBoxSystemType.Text;
            this.TextBoxChineseName.Text = ListBoxDiseaseChineseName.Text.ToString().Trim();
            this.TextBoxEnglishName.Text = ListBoxDiseaseEnglishName.Text.ToString().Trim();
        }
        protected void ListBoxFindingChineseNameSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ListBoxFindingEnglishName.SelectedIndex = this.ListBoxFindingChineseName.SelectedIndex;
            this.DropDownListFindingType.Text = ListBoxFindingType.Text;
            this.TextBoxFindingChineseName.Text = ListBoxFindingChineseName.Text.ToString().Trim();
            this.TextBoxFindingEnglishName.Text = ListBoxFindingEnglishName.Text.ToString().Trim();
        }
        protected void ListBoxFindingEnglishNameSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ListBoxFindingChineseName.SelectedIndex = this.ListBoxFindingEnglishName.SelectedIndex;
            this.DropDownListFindingType.Text = ListBoxFindingType.Text;
            this.TextBoxFindingChineseName.Text = ListBoxFindingChineseName.Text.ToString().Trim();
            this.TextBoxFindingEnglishName.Text = ListBoxFindingEnglishName.Text.ToString().Trim();
        }
        protected void ButtonAddNewDisease_Click(object sender, EventArgs e) //疾病添加按钮
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;    //数据库连接部分
                SqlConnection conn = new SqlConnection(strConn);
                string countsql = "select count(*) from dbo.DiseasesBase where ChineseName='" + TextBoxChineseName.Text + "'";   //查找数据库与疾病中文名输入有无重复
                SqlCommand countcmd = new SqlCommand(countsql, conn);
                conn.Open();
                int count = (int)countcmd.ExecuteScalar();
                if (count == 0)          //如果没找到记录，说明不重复，可以插入数据 
                {
                    string sql = "insert into dbo.DiseasesBase(ChineseName,EnglishName,SystemType) values ('"
                        + TextBoxChineseName.Text + "','" + TextBoxEnglishName.Text + "','" + DropDownListSystemType.Text + "')"; //插入数据库语句
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    
                    Response.Redirect(Request.Url.ToString()); //刷新页面
                 //   this.Label1.Text = "添加成功！";
                   // Response.Write("<script>alert('添加成功！');</script>");
               
                    //Response.Write("<script>alert('添加成功！');opener.location.reload();window.close();</script>");*/    
                }
                else
                {
                    this.Label1.Text = "对不起，该疾病已存在！";
                    //Response.Write("<script>alert('对不起，该疾病已存在！')</script>");
                    //如重复，显示疾病已存在
                }
                conn.Close();
            }
            catch
            {
                Response .Write ("<script>alert('操作失败！')</script>");
            }
        }
        protected void ButtonSaveDisease_Click(object sender, EventArgs e) //疾病修改按钮
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                string countsql = "select count(*) from dbo.DiseasesBase where ChineseName='" + TextBoxChineseName.Text + "'";   //查找主键有无重复
                SqlCommand countcmd = new SqlCommand(countsql, conn);
                conn.Open();
                int count = (int)countcmd.ExecuteScalar();
                if (count == 0)
                {
                    string sql = "update dbo.DiseasesBase set ChineseName='" + TextBoxChineseName.Text + "',EnglishName='"
                        + TextBoxEnglishName.Text + "',SystemType='" + DropDownListSystemType.Text + "' where ChineseName='" + ListBoxDiseaseChineseName.Text + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    //MessageBox.Show("修改成功！");
                    Response.Redirect(Request.Url.ToString()); //刷新页面
                }
                else
                {
                    this.Label1.Text = "对不起，该疾病已存在！";
                    // Response.Write("<script>alert('对不起，该疾病已存在！')</script>");
                }
                conn.Close();
            }
            catch
            {
                Response.Write("<script>alert('操作失败！')</script>");
            }
        }  
        protected void ButtonDeleteDisease_Click(object sender, EventArgs e) //疾病删除按钮
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                string sql = "delete from dbo.DiseasesBase where ChineseName='" + ListBoxDiseaseChineseName.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
               // MessageBox.Show("删除成功！");
                Response.Redirect(Request.Url.ToString()); //刷新页面       
            }
            catch
            {
                Response.Write("<script>alert('操作失败！')</script>");
            }

        }
        protected void ButttonAddNewFinding_Click(object sender, EventArgs e) //症候添加按钮
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                string countsql = "select count(*) from dbo.FindingsBase where ChineseName='" + TextBoxFindingChineseName.Text + "'";   //查找主键有无重复
                SqlCommand countcmd = new SqlCommand(countsql, conn);
                conn.Open();
                int count = (int)countcmd.ExecuteScalar();
                if (count == 0)
                {
                    string sql = "insert into dbo.FindingsBase(ChineseName,EnglishName,FindingType) values('"
                        + TextBoxFindingChineseName.Text + "','" + TextBoxFindingEnglishName.Text + "','" + DropDownListFindingType.Text + "'  )";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                   // MessageBox.Show("添加成功！"); //执行成功显示添加成功
                    Response.Redirect(Request.Url.ToString()); //刷新页面
                }
                else
                {
                    this.Label1.Text = "对不起，该症候已存在！";
                   // MessageBox.Show("对不起，该症候已存在！");
                }
                conn.Close();
            }
            catch
            {
                Response.Write("<script>alert('操作失败！')</script>");
            }
        }
        protected void ButtonSaveFinding_Click(object sender, EventArgs e) //症候修改按钮
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                string countsql = "select count(*) from dbo.FindingsBase where ChineseName='" + TextBoxFindingChineseName.Text + "'";   //查找症候中文名有无重复
                SqlCommand countcmd = new SqlCommand(countsql, conn);
                conn.Open();
                int count = (int)countcmd.ExecuteScalar();
                if (count == 0)
                {
                    string sql = "update dbo.FindingsBase set ChineseName='" + TextBoxFindingChineseName.Text + "',EnglishName='"
                        + TextBoxFindingEnglishName.Text + "',FindingType='" + DropDownListFindingType.Text + "' where ChineseName='" + ListBoxFindingChineseName.Text + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                   // MessageBox.Show("修改成功！");
                    Response.Redirect(Request.Url.ToString());
                }
                else
                {
                    this.Label1.Text = "对不起，该症候已存在！"; 
                    //MessageBox.Show("对不起，该症候已存在！");
                }
                conn.Close();
            }
            catch
            {
                Response.Write("<script>alert('操作失败！')</script>");
            }
        } 
        protected void ButtonDeleteFinding_Click(object sender, EventArgs e) //症候删除按钮
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();

                string sql = "delete from dbo.FindingsBase where ChineseName='" + ListBoxFindingChineseName.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("删除成功！");
                Response.Redirect(Request.Url.ToString());
                conn.Close();
            }
            catch
            {
                Response.Write("<script>alert('操作失败！')</script>");
            }
        }
    }
}