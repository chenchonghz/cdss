using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using System.Math;
using System.Web.UI.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace CDSS
{
    public partial class FunctionSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FunctionKind = this.DropDownList1.Text;
            string FunctionTitle = this.ListBox1.Text;
            String SqlString = "SELECT [Independent],[Dependent] FROM [FunctionSetting] "+
                    "where (FunctionKind=@FunctionKind) and (FunctionTitle=@FunctionTitle)";
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection CurConn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SqlString, CurConn);
            cmd.Parameters.AddWithValue("@FunctionKind", FunctionKind);
            cmd.Parameters.AddWithValue("@FunctionTitle", FunctionTitle);
            CurConn.Open();

            SqlDataReader dr = cmd.ExecuteReader();  
            //SqlDataReader 提供一种从 SQL Server 数据库读取行的只进流的方式。若要创建 SqlDataReader，必须调用SqlCommand对象的  ExecuteReader方法，而不要直接使用构造函数。
            Chart1.Series[0].Points.Clear();
            while (dr.Read())
            {
                Chart1.Series[0].Points.AddXY(Convert.ToString(dr["Independent"]), Convert.ToString(dr["Dependent"]));
            }
            //或者 Chart1.DataBindTable(dr, "ShopName");  //因为数据源中只有两列ShopName和count,因此在调用Chart1.DataBindTable方法的时候,告诉了图表X轴的名称为ShopName,因此自动将count设置为Y轴的数据了

            dr.Close();
            //在使用SqlDataReader时，关联的  SqlConnection正忙于为 SqlDataReader服务，对 SqlConnection无法执行任何其他操作，只能将其关闭。除非调用  SqlDataReader的 close方法，否则会一直处于此状态。
            CurConn.Close();  
            //关闭与数据库的连接。这是关闭任何打开连接的首选方法。
        }

        protected void Chart1_Click(object sender, ImageMapEventArgs e)
        {

        }

        protected void Chart1_DataBinding(object sender, EventArgs e)
        {
            //this.Chart1.Series.Clear();
        }

        protected void Chart1_PrePaint(object sender, ChartPaintEventArgs e)
        {
            //this.Chart1.Series.Clear();
        }
    }
}