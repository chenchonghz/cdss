using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient; //连接数据库所需语句
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.ComponentModel;
namespace CDSS
{
    public partial class DisplayPhysicalData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Chart1.BorderSkin.SkinStyle = System.Web.UI.DataVisualization.Charting.BorderSkinStyle.Emboss;//设置图像的边框外观样式            
            Chart1.BackColor = System.Drawing.Color.Pink;//设置图表的背景颜色
            Chart1.BackSecondaryColor = System.Drawing.Color.Green;//设置背景的辅助颜色 
            Chart1.BorderlineColor = System.Drawing.Color.Pink;//设置图像边框的颜色         
            Chart1.BorderlineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;//设置图像边框线的样式(实线、虚线、点线) 
            Chart1.BorderlineWidth = 2;//设置图像的边框宽度
           

            Chart1.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.Yellow;//设置图表区域的背景颜色
            Chart1.ChartAreas["ChartArea1"].AxisX.LineColor = Color.Red;  //设置X轴颜色
            Chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Red;  //设置Y轴颜色
            Chart1.ChartAreas["ChartArea1"].AxisX.LineWidth = 2;//设置X轴坐标线宽度
            Chart1.ChartAreas["ChartArea1"].AxisY.LineWidth = 2;//设置Y轴坐标线宽度
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = "测试时间";//设置X轴标题
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "体温(摄氏度)";   //设置Y轴标题
            //中间X,Y线条的线性及颜色设置
            Chart1.ChartAreas["ChartArea1"].AxisX.LineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Blue;
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Blue;
            //X.Y轴数据显示间隔
            Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 0.5;
            // X轴线条显示间隔
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1;
            Chart1.ChartAreas[0].AxisY.Minimum = 34;//设定y轴的最小值

            Legend l = new Legend();//初始化一个图例的实例  
            l.Alignment = System.Drawing.StringAlignment.Near;//设置图表的对齐方式
            l.BackColor = System.Drawing.Color.Blue;//设置图例的背景颜色 
            l.DockedToChartArea = "ChartArea1";//设置图例要停靠在哪个区域上 
            l.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;//设置停靠在图表区域的位置
            l.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);//设置图例的字体属性 
            l.IsTextAutoFit = true;
            l.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Column;//设置显示图例项方式
            l.Name = "体温";//设置图例的名称
            Chart1.Legends.Add(l.Name);
            Chart1.Series["体温"].Color = System.Drawing.Color.Red;//设置线条颜色
            Chart1.Series["体温"].XValueMember = "AcquisitionTime";
            Chart1.Series["体温"].YValueMembers = "PhysicalDataValue";
            Chart1.Series["体温"]["DrawingStyle"] = "Emboss";
            Chart1.DataBind();
            */
        }
    }
}