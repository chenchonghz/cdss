using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;

namespace WinCDSS
{
    public partial class Form1 : Form
    {
        int[] LeftSideData = new int[100];
        int[] RightSideData = new int[100];
        int[] TotalBetaData = new int[100];
        public Form1()
        {
            InitializeComponent();
        }

        private int[] CurrentBetaCurveData()
        {
            double[] Fvalue = new double[100];
            int[] CurBetaData = new int[100];
            double BaseLine=(101.00-this.vScrollBarBase.Value)/(100.00);

            //double p = System.Math.Sqrt(this.hScrollBar1.Value);
            //double q = System.Math.Sqrt(this.hScrollBar2.Value);
            double p = this.hScrollBar1.Value;
            double q = this.hScrollBar2.Value;
            double BetaValue = this.chart1.DataManipulator.Statistics.BetaFunction(p, q);
            BetaValue = 1 / BetaValue;
            //double[] Fvalue = new double[100];
            double tmpVar;
            double AgeCount = 100;
            double CurMax = 0;
            for (int i = 1; i <= 99; i++)
            {
                double x = (i / AgeCount);
                double tmp1 = System.Math.Pow(x, (p - 1));
                double tmp2 = System.Math.Pow((1 - x), (q - 1));
                tmpVar = tmp1 * tmp2 * BetaValue;
                if (tmpVar > CurMax)
                { CurMax = tmpVar; }
                Fvalue[i] = tmpVar;
            }

            for (int i = 1; i <= 99; i++)
            {
                double BaseValue = BaseLine * CurMax;
                tmpVar = 99 * (BaseValue + Fvalue[i]) / (BaseValue + CurMax);
                CurBetaData[i] = Convert.ToInt16(tmpVar);
            }
            return CurBetaData;
        }

        private void DisplayCurrentData(Chart CurChart, int[] CurrentData)
        {
            CurChart.Series.Clear();
            Series seriesPies = new Series("");
            seriesPies.IsVisibleInLegend = false;
            CurChart.Series.Add(seriesPies);
            seriesPies.ChartType = SeriesChartType.Spline;
            for (int i = 1; i < 100; i++)
            {
                seriesPies.Points.AddXY(i, CurrentData[i]);
            }
        }

        private void buttonDisplay_Click(object sender, EventArgs e)
        {
            int[] CurBetaData = CurrentBetaCurveData();
            if (this.radioButton2.Checked)
            {
                //LeftSideData,RightSideData,TotalBetaData;
                RightSideData = CurBetaData;
                DisplayCurrentData(this.chart2, CurBetaData); 
            }
            else
            { 
                DisplayCurrentData(this.chart1, CurBetaData);
                LeftSideData = CurBetaData;
                TotalBetaData = CurBetaData;
            }           
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            buttonDisplay_Click(sender, e);
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            buttonDisplay_Click(sender, e);
        }

        private void buttonAddition_Click(object sender, EventArgs e)
        {
            //LeftSideData,RightSideData,TotalBetaData;
            double MaxAddition = 0.0;
            for (int i = 1; i < 100; i++)
            {
                TotalBetaData[i] = LeftSideData[i] + RightSideData[i];
                if (TotalBetaData[i] > MaxAddition)
                { MaxAddition=TotalBetaData[i]; }
            }
            if (MaxAddition>100)
            {
                double tmpVar = 0.0;
                for (int i = 1; i < 100; i++)
                {
                    tmpVar = 99*TotalBetaData[i] / MaxAddition;
                    TotalBetaData[i] = Convert.ToInt16(tmpVar);
                }
            }
            LeftSideData = TotalBetaData;
            DisplayCurrentData(this.chart1, TotalBetaData);
        }
        private string LoadPredispositionXmlString()
        {
            SqlConnection CurConn = new SqlConnection();
            string CurConnStr=this.textBox1.Text.ToString();
            CurConn.ConnectionString = CurConnStr;
            string ChineseName = "易感因素";
            String SqlString = "SELECT [Description] FROM [FindingsBase] where [ChineseName]=@ChineseName";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@ChineseName", ChineseName);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            string Description = null;

            if (CurReader.Read())
            {
                Description = CurReader["Description"].ToString();
            }
            CurConn.Close();
            return Description;        
        }

        private XmlNodeList GetAgeOptionsNodeList(string Description)
        {
            XmlNodeList OptionNodeList=null;
            if (Description != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Description);
                //XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                XmlNode root = xmlDoc.DocumentElement;
                string CurXmlPath = "//predispositionKnowledge/Properties/Property";
                XmlNodeList PropertiesNodeList = root.SelectNodes(CurXmlPath);
                if (PropertiesNodeList.Count > 0)
                {
                    foreach (XmlNode FindingNode in PropertiesNodeList)
                    {
                        string tmpStr=FindingNode["Name"].InnerText.Trim();
                        if (tmpStr=="年龄")
                        { 
                            OptionNodeList = FindingNode["Options"].ChildNodes; 
                        }
                    }
                }
            }
            return OptionNodeList;
        }

        private void LoadFunctionNameList()
        {
            string Description = LoadPredispositionXmlString();
            XmlNodeList OptionNodeList = GetAgeOptionsNodeList(Description);
            if (OptionNodeList !=null )
            {
                this.comboBoxFunctionTitle.Items.Clear();
                foreach (XmlNode FindingNode in OptionNodeList)
                {
                    string tmpStr = FindingNode.InnerText.Trim();
                    this.comboBoxFunctionTitle.Items.Add(tmpStr);
                }
                if (this.comboBoxFunctionTitle.Items.Count>0)
                {
                    this.comboBoxFunctionTitle.SelectedIndex=0;
                }
            }         
        }

        private void SaveCurrentDataToDatabase(string FunctionKind, string FunctionTitle)
        {
            SqlConnection CurConn = new SqlConnection();
            string CurConnStr = this.textBox1.Text.ToString();
            CurConn.ConnectionString = CurConnStr;
            String SqlString = "DELETE FROM [FunctionSetting] WHERE (FunctionKind=@FunctionKind) and (FunctionTitle=@FunctionTitle)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@FunctionKind", FunctionKind);
            CurCmd.Parameters.AddWithValue("@FunctionTitle", FunctionTitle);
            CurConn.Open();
            CurCmd.ExecuteNonQuery();

            int Independent = 0;
            int Dependent = 0;
            SqlString = "INSERT INTO [FunctionSetting] "+
                    "([FunctionKind],[FunctionTitle],[Independent],[Dependent]) "+
                    "VALUES (@FunctionKind,@FunctionTitle,@Independent,@Dependent)";
            CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@FunctionKind", FunctionKind);
            CurCmd.Parameters.AddWithValue("@FunctionTitle", FunctionTitle);
            CurCmd.Parameters.AddWithValue("@Independent", Independent);
            CurCmd.Parameters.AddWithValue("@Dependent", Dependent);
            for (int i = 1; i < 100; i++)
            {
                //Independent = i;
                //Dependent=TotalBetaData[i];
                CurCmd.Parameters["@Independent"].Value = i;
                CurCmd.Parameters["@Dependent"].Value = TotalBetaData[i];
                CurCmd.ExecuteNonQuery();
            }
            CurConn.Close();
        }

        private Boolean InputDataValidated(string FunctionKind, string FunctionTitle)
        {
            Boolean IsValidated = true;

            if (FunctionKind=="")
            {
                MessageBox.Show("函数类型不能为空！");
                IsValidated = false;
            }
            if (FunctionTitle == "")
            {
                MessageBox.Show("函数名称不能为空！");
                IsValidated = false;
            }
            int ZeroCount = 0;
            for (int i = 1; i < 100; i++)
            {
                if (TotalBetaData[i]==0)
                {
                    ZeroCount++;
                }
            }
            if (ZeroCount>95)
            {
                MessageBox.Show("请重新制作函数曲线！");
                IsValidated = false;
            }
            return IsValidated;
        }
        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            string FunctionKind = this.comboBoxFunctionKind.Text.Trim();
            string FunctionTitle = this.comboBoxFunctionTitle.Text.Trim();
            if (InputDataValidated(FunctionKind, FunctionTitle))
            {
                SaveCurrentDataToDatabase(FunctionKind,FunctionTitle);
                MessageBox.Show(FunctionKind+FunctionTitle+"数据保存成功！");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFunctionNameList();
        }

        private void vScrollBarBase_Scroll(object sender, ScrollEventArgs e)
        {
            buttonDisplay_Click(sender, e);
        }
    }
}
