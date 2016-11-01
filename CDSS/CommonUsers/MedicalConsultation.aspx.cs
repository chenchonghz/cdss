using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient; 
using System.Data;
using System.Web.Configuration;
using System.Xml;

namespace CDSS
{
    public partial class MedicalConsultation : System.Web.UI.Page
    {
        public static int i, n1, n2, n3;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection Conn = new SqlConnection(strConn);
                Conn.Open();
                if (Session["type"].ToString() == "Commonuser")//判断登陆者是否为普通用户
                {
                    string strSQL = "Select top 1 * From DeveloperInfo where (LoginName=@UserName)";
                    SqlCommand command = new SqlCommand(strSQL, Conn);
                    command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
                    SqlDataReader DataReader = command.ExecuteReader();
                    while (DataReader.Read())
                    {
                        this.LabelMyLoginName.Text = Session["UserName"].ToString().Trim();//将登陆者信息显示
                        this.LabelMyName.Text = DataReader["Name"].ToString().Trim();
                        this.LabelMySex.Text = DataReader["Sex"].ToString().Trim();
                        if (DataReader["Sex"].ToString().Length != 0) { this.LabelMyBirthday.Text = Convert.ToDateTime(DataReader["DOB"]).ToString("yyyy-MM-dd"); }
                    }
                    DataReader.Close();
                }
                else
                {
                    //Response.Write("<script>alert('请先登录！')</script>");
                    Response.Redirect("Default.aspx");//若不是普通用户，跳转至主页
                }
                Conn.Close();
            }
        }

        protected void BulletedList1_Click(object sender, BulletedListEventArgs e)
        {
            n1 = e.Index;
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString); 
            string sql = "select Description from dbo.FindingsBase where ChineseName='" + BulletedList1.Items[e.Index].Text + "'";
            /*if (e.Index > n)
            {
                string temp = BulletedList1.Items[n].Text;
                BulletedList1.Items[n].Text = BulletedList1.Items[e.Index].Text;
                BulletedList1.Items[e.Index].Text = temp;
                n++;
            }*/
            BulletedList1.Items[e.Index].Attributes.Add("style", "color:red");
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sql = dr["Description"].ToString();
            }
            conn.Close();
            BulletedList2.Items.Clear();
            BulletedList3.Items.Clear();//清除特征、选项列表中已有项
            XmlDocument reader = new XmlDocument();
            reader.LoadXml(sql);  //将数据库中xml文件读出
            XmlElement rootElem = reader.DocumentElement;
            XmlNodeList PropertyNodes = rootElem.GetElementsByTagName("Property");
            foreach (XmlNode node in PropertyNodes)
            {
                XmlNodeList NameNodes = ((XmlElement)node).GetElementsByTagName("Name");
                string name = NameNodes[0].InnerText;
                BulletedList2.Items.Add(name);//将xml特征名称全部写入到特征列表中
            }
        }

        protected void BulletedList2_Click(object sender, BulletedListEventArgs e)
        {
            n2 = e.Index;
            BulletedList1.Items[n1].Attributes.Add("style", "color:red");
            BulletedList2.Items[e.Index].Attributes.Add("style", "color:red");
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString);
            string sql = "select Description from dbo.FindingsBase where ChineseName='" + BulletedList1.Items[n1].Text + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sql = dr["Description"].ToString();
            }
            conn.Close();
            BulletedList3.Items.Clear();
            XmlDocument reader = new XmlDocument();
            reader.LoadXml(sql);
            XmlElement rootElem = reader.DocumentElement;
            XmlNodeList PropertyNodes = rootElem.GetElementsByTagName("Property");
            foreach (XmlNode node in PropertyNodes)
            {
                XmlNodeList NameNodes = ((XmlElement)node).GetElementsByTagName("Name");
                string name = NameNodes[0].InnerText;
                if (string.Compare(name, BulletedList2.Items[e.Index].ToString(), true) == 0)
                {
                    XmlNodeList OptionNodes = ((XmlElement)node).GetElementsByTagName("Option");
                    for (int i = 0; i < OptionNodes.Count; i++)
                    {
                        string Option = OptionNodes[i].InnerText;
                        BulletedList3.Items.Add(Option);
                    }
                }
            }
        }

        protected void BulletedList3_Click(object sender, BulletedListEventArgs e)
        {
            n3 = e.Index;
            BulletedList1.Items[n1].Attributes.Add("style", "color:red");
            BulletedList2.Items[n2].Attributes.Add("style", "color:red");
            BulletedList3.Items[e.Index].Attributes.Add("style", "color:red");
        }

        protected void BulletedList5_Click(object sender, BulletedListEventArgs e)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString);
            conn.Open();
            string sql = "select * from DoctorInfo where LoginName='" + BulletedList5.Items[e.Index] + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader DataReader = cmd.ExecuteReader();
            while (DataReader.Read())
            {
                this.LabelDocName.Text = DataReader["Name"].ToString().Trim();
                this.LabelDocSex.Text = DataReader["Sex"].ToString().Trim();
                this.LabelDocOccupation.Text = DataReader["Occupation"].ToString().Trim();
                this.LabelDocWorkingUnit.Text = DataReader["WorkingUnit"].ToString().Trim();
                this.LabelDocEmail.Text = DataReader["EmailAddress"].ToString().Trim();
                this.LabelDocLoginName.Text = DataReader["LoginName"].ToString().Trim();
            }
            DataReader.Close();
            conn.Close();
        }

    }
}