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
    public partial class SetUserRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadioButtonListUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = RadioButtonListUserType.SelectedIndex;
            MultiView2.ActiveViewIndex = RadioButtonListUserType.SelectedIndex;
           
        }
        string[] RN = new string[5];
        protected void ButtonSaveSetting_Click(object sender, EventArgs e)
        {
            string id="";
            if (int.Parse(RadioButtonListUserType.SelectedValue) == 0)
            {
                id = GridView1.SelectedValue.ToString();
                

            for (int i = 0; i <= 1; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                   
                    RN[i]=CheckBoxList1.Items[i].Text;
                    
            }
           
            }
            else if (int.Parse(RadioButtonListUserType.SelectedValue) == 1)
            {
               id = GridView2.SelectedValue.ToString();
              
                for (int i = 0; i <= 4; i++)
                {
                    if (CheckBoxList2.Items[i].Selected)
                      
                        RN[i] = CheckBoxList2.Items[i].Text;

                }
            }

            else if (int.Parse(RadioButtonListUserType.SelectedValue) == 2)
            {
                id = GridView3.SelectedValue.ToString();
               
                for (int i = 0; i <= 3; i++)
                {
                    if (CheckBoxList3.Items[i].Selected)

                        RN[i] = CheckBoxList3.Items[i].Text;
                }

            }
            string id1 =id;
            Guid ID = new Guid(id1);
            
            
            string strConn = "server =  localhost; database = CDSS; uid= cdssuser; pwd = cdssuser";
            SqlConnection conn = new SqlConnection(strConn);

            conn.Open();

            string sql1 = "select count(*) from UsersRole where UserID='" + ID + "'";
            SqlCommand cmd1 = new SqlCommand(sql1, conn);
            cmd1.CommandText = sql1;
            int count = (int)cmd1.ExecuteScalar();
            cmd1.ExecuteNonQuery();
            if (count > 0)
            {
                Response.Write("<script>alert('权限已设置！')</script>");

            }
          
            else
            {
                string sql = "insert into dbo.UsersRole(UserID,RoleName1,RoleName2,RoleName3,RoleName4,RoleName5)" + "values('" + ID+ "','" + RN[0] + "','" + RN[1] + "','" + RN[2] + "','" + RN[3] + "','" + RN[4] + "')";
                Response.Write("<script>alert('权限已上传！')</script>");
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            for (int i = 0; i < 5; i++)
                RN[i] = "";
          
            
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {

          
            for (int i = 0; i < 5; i++)
                RN[i] = "";
        }

        protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
        {

           
            for (int i = 0; i < 5; i++)
                RN[i] = "";
        }
    }
}