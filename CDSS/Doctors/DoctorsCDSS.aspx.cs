using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using CdssProgram;

namespace CDSS
{

    public partial class DoctorsCDSS : System.Web.UI.Page
    {
        string BasicalXmlStru = "<CdssXml><Symptoms></Symptoms><Signs></Signs><Laboratorys></Laboratorys>" +
                "<Radiologys></Radiologys><OtherExams></OtherExams><Diagnoses></Diagnoses></CdssXml>"; //CDSS基础XML结构

        public struct DiagnosisProb
        {
            public double ProbValue;
            public string DiseaseName;
        }

        public static XmlDocument SelectedXmlInfo; //用于保存已经选择的症候相关信息
        public static XmlDocument CdssXmlForList; //用于算法传递参数信息
        public static XmlDocument CdssOutputXml; //用于算法传递参数信息
        static string CurSelectedSymptomName=""; //用于保存当前最后选择的症候名称
        static string CurSelectedSymptomProperty=""; //用于保存当前最后选择的症候属性
        static string CurSelectedSymptomOption=""; //用于保存当前最后选择的症候选项
        static string CurSelectedUsersPhrNumber; //用于保存当前选中用户的个人健康档案号
        static string CDSSConnectionString; //用于建立并保存的统一数据库连接
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CDSSConnectionString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            }
        }

        private List<string> GetOptionNameList(string CurSymptomName, string CurSymptomProperty, XmlDocument CurXmlDoc)
        {
            XmlNodeList SymptomElementList = CurXmlDoc.GetElementsByTagName("Symptom");
            List<string> OptionNameList = new List<string>();
            string tmpSymptomName = "";
            string tmpPropertyName = "";
            string tmpOptionName = "";
            for (int i = 0; i < SymptomElementList.Count; i++)
            {
                tmpSymptomName = SymptomElementList[i].Attributes["SymptomName"].Value.Trim();
                tmpPropertyName = SymptomElementList[i].Attributes["PropertyName"].Value.Trim();
                tmpOptionName = SymptomElementList[i].Attributes["OptionName"].Value.Trim();
                int OptionIndex = OptionNameList.IndexOf(tmpOptionName);
                bool SameSource = ((tmpSymptomName == CurSymptomName) && (tmpPropertyName == CurSymptomProperty));
                if (SameSource && (tmpOptionName != "") && (OptionIndex < 0))
                {
                    OptionNameList.Add(tmpOptionName);
                }
            }
            return OptionNameList;
        }

        private List<string> GetPropertyNameList(string CurSymptomName,XmlDocument CurXmlDoc)
        {
            XmlNodeList SymptomElementList = CurXmlDoc.GetElementsByTagName("Symptom");
            List<string> PropertyNameList = new List<string>();
            string tmpSymptomName = "";
            string tmpPropertyName = "";
            for (int i = 0; i < SymptomElementList.Count; i++)
            {
                tmpSymptomName = SymptomElementList[i].Attributes["SymptomName"].Value.Trim();
                tmpPropertyName = SymptomElementList[i].Attributes["PropertyName"].Value.Trim();
                int PropertyIndex = PropertyNameList.IndexOf(tmpPropertyName);
                if ((tmpSymptomName == CurSymptomName) && (tmpPropertyName != "") && (PropertyIndex < 0))
                {
                    PropertyNameList.Add(tmpPropertyName);
                }
            }
            return PropertyNameList;
        }

        private void DisplayBulletedList(BulletedList CurList,List<string> OldList, List<string> NewList)
        {
            CurList.Items.Clear();
            string ListedName = "";
            int CurIndex, TotalItem;
            for (int i = 0; i < OldList.Count; i++)
            {
                ListedName = OldList[i];
                ListItem CurItem = new ListItem(ListedName);
                CurIndex = CurList.Items.IndexOf(CurItem);
                if ((ListedName != "") && (CurIndex < 0))
                {
                    CurList.Items.Add(ListedName);
                    TotalItem = CurList.Items.Count;
                    CurList.Items[TotalItem - 1].Attributes.Add("Style", "color:red");
                }
            }

            for (int i = 0; i < NewList.Count; i++)
            {
                ListedName = NewList[i];
                ListItem CurItem = new ListItem(ListedName);
                CurIndex = CurList.Items.IndexOf(CurItem);
                if ((ListedName != "") && (CurIndex < 0))
                {
                    CurList.Items.Add(ListedName);
                }
            }
        }

        protected void blSymptomName_Click(object sender, BulletedListEventArgs e)
        {
            //首先检查当前症状是否有进一步的属性信息
            string SelSymptomName = this.blSymptomName.Items[e.Index].Value.ToString().Trim();
            CurSelectedSymptomName = SelSymptomName;

            DisplaySymptomProperty(SelSymptomName);
        }

        private void DisplaySymptomProperty(string SelSymptomName)
        {
            List<string> OldList = GetPropertyNameList(SelSymptomName, SelectedXmlInfo);
            List<string> NewList = GetPropertyNameList(SelSymptomName, CdssXmlForList);
            //如果有属性信息则显示属性信息，否则调用CDSS算法并返回进一步问诊信息
            if (NewList.Count > 0)
            {
                DisplayBulletedList(this.blSymptomProperty, OldList, NewList);
            }
            else
            {
                //在CdssOutputXml基础上，把当前症状名称选择结果添加到CdssAlgorithm的输入信息中
                AddSelSymptomToCdssOutput(SelSymptomName, "", "");
                AddSelSymptomToSelectedInfo(SelSymptomName, "", "");
                //第一次调用CdssAlgorithm之前需要初始化CDSS算法的数据库连接以及用户的健康档案号

                CdssAlgorithm.CDSSConnectionString = CDSSConnectionString;
                CdssAlgorithm.CdssUsersPhrNumber = CurSelectedUsersPhrNumber;
                //调用CDSS算法
                CdssXmlForList = CdssAlgorithm.GetCdssOutput(CdssOutputXml);
                DisplayChiefComplaintCDSS(SelSymptomName);
            }
        }
        private void DisplayChiefComplaintCDSS(string ChiefComplaint)
        {
            //显示主诉的属性列表
            XmlNodeList SymptomElementList = CdssXmlForList.GetElementsByTagName("Symptom");
            List<string> PropertyNameList = new List<string>();
            string tmpSymptomName = "";
            string tmpPropertyName = "";
            for (int i = 0; i < SymptomElementList.Count; i++)
            {
                tmpSymptomName = SymptomElementList[i].Attributes["SymptomName"].Value.Trim();
                tmpPropertyName = SymptomElementList[i].Attributes["PropertyName"].Value.Trim();
                int PropertyIndex = PropertyNameList.IndexOf(tmpPropertyName);
                if ((tmpSymptomName == ChiefComplaint) && (tmpPropertyName != "") && (PropertyIndex < 0))
                {
                    PropertyNameList.Add(tmpPropertyName);
                }
            }

            this.blSymptomProperty.Items.Clear();
            if (PropertyNameList.Count > 0)
            {
                for (int i = 0; i < PropertyNameList.Count; i++)
                {
                    blSymptomProperty.Items.Add(PropertyNameList[i]);
                }
            }
            else
            {
                //如果没有症状属性列表，说明数据库存在问题，需要完善数据库
            }
            DisplayDiseaseAndProbList(); //图表显示疾病及其概率           
        }

        private void DisplayDiseaseAndProbList() //图表显示疾病及其概率 
        {
            List<DiagnosisProb> DiagnosisProbList = new List<DiagnosisProb>();
            XmlNodeList DiagnosisElementList = CdssXmlForList.GetElementsByTagName("Diagnosis");
            string tmpDiseaseName;
            string tmpProbability;
            DiagnosisProb tmpDiagnosisProb;
            for (int i = 0; i < DiagnosisElementList.Count; i++)
            {
                tmpDiseaseName = DiagnosisElementList[i].Attributes["DiseaseName"].Value.Trim();
                tmpProbability = DiagnosisElementList[i].Attributes["ProbValue"].Value.Trim();
                tmpDiagnosisProb.DiseaseName = tmpDiseaseName;
                tmpDiagnosisProb.ProbValue = Convert.ToDouble(tmpProbability);
                DiagnosisProbList.Add(tmpDiagnosisProb);
            }
            //按照疾病发病可能性从大到小排序
            IEnumerable<DiagnosisProb> DiagnosisQuery = DiagnosisProbList.OrderBy(DiagnosisProb => DiagnosisProb.ProbValue);
            DiagnosisProbList=DiagnosisQuery.ToList();
            DiagnosisProbList.Reverse();

            blDiagnosisProb.Items.Clear();

            Chart1.Series.Clear();
            Series seriesBars = new Series("");
            seriesBars.ChartType = SeriesChartType.Column;
            seriesBars.IsVisibleInLegend = false;
            Chart1.Series.Add(seriesBars);

            //按照疾病发生可能性大小显示于列表以及图表
            string tmpStr = "";
            int ProbPercent;
            for (int i = 0; i < DiagnosisProbList.Count; i++)
            {
                tmpDiseaseName = DiagnosisProbList[i].DiseaseName;
                ProbPercent = Convert.ToInt16(Math.Round(DiagnosisProbList[i].ProbValue * 100));
                if (ProbPercent>0)
                {
                    tmpProbability = Convert.ToString(ProbPercent);
                    tmpStr = tmpDiseaseName + "(" + tmpProbability + "%)";
                    if (i < 6)
                    {
                        seriesBars.Points.AddXY(tmpDiseaseName, tmpProbability);
                    }
                    blDiagnosisProb.Items.Add(tmpStr);
                }                
            }
        }

        private void AddSelSymptomToSelectedInfo(string SymptomName, string PropertyName, string OptionName)
        {
            //首先是否存在相同的症状信息
            bool ExistSameSymptom = false;
            XmlNodeList SymptomList = SelectedXmlInfo.GetElementsByTagName("Symptom");
            XmlNode SymptomNode = null;
            XmlNode SymptomsNode = null;
            string tmpSymptom = "";
            string tmpProperty = "";
            string tmpOption = "";
            for (int i = 0; i < SymptomList.Count;i++ )
            {
                SymptomNode=SymptomList[i];
                tmpSymptom = SymptomNode.Attributes["SymptomName"].Value.Trim();
                tmpProperty = SymptomNode.Attributes["PropertyName"].Value.Trim();
                tmpOption = SymptomNode.Attributes["OptionName"].Value.Trim();
                if ((tmpSymptom==SymptomName) && (tmpProperty==PropertyName) && (tmpOption==OptionName))
                {
                    ExistSameSymptom = true;
                }
            }
            //首先确定症状的父节点
            if (SymptomNode == null)
            {
                XmlNodeList SymptomsList = SelectedXmlInfo.GetElementsByTagName("Symptoms");
                SymptomsNode = SymptomsList[0];
            }
            else
            {
                SymptomsNode = SymptomNode.ParentNode;
            }
            //如果没有添加相同的症状信息，则添加当前信息
            if (ExistSameSymptom == false)
            {
                XmlElement SymptomElement = SelectedXmlInfo.CreateElement("Symptom");
                SymptomElement.SetAttribute("SymptomName", SymptomName);
                SymptomElement.SetAttribute("PropertyName", PropertyName);
                SymptomElement.SetAttribute("OptionName", OptionName);
                SymptomsNode.AppendChild(SymptomElement);
            }
            DisplayHistoryText();
        }

        private void DisplayHistoryText()
        {
            string TextHistory = "";
            string BriefComplain = GetBriefComplain();
            string CurHistory = "";
            string PhysicalExam = "";
            string PreliminaryDiag = "";
            string PreliminaryTreat = "";
            //lcf tobecontinued 20141220 18:30

            if (BriefComplain != "")
            {
                LiteralBriefComplain.Text = "主诉：" + BriefComplain;
            }
            if (CurHistory != "")
            {
                TextHistory = TextHistory + "主诉：" + Environment.NewLine + CurHistory;
            }
            if (PhysicalExam != "")
            {
                TextHistory = TextHistory + "主诉：" + Environment.NewLine + PhysicalExam;
            }
            if (PreliminaryDiag != "")
            {
                TextHistory = TextHistory + "主诉：" + Environment.NewLine + PreliminaryDiag;
            }
            if (PreliminaryTreat != "")
            {
                TextHistory = TextHistory + "主诉：" + Environment.NewLine + PreliminaryTreat;
            }
        }

        private string GetBriefComplain()
        {
            string BriefComplain="获取患者主诉测试函数（咳嗽、咳痰数月，伴发热数天）";
            return BriefComplain;
        }
        private void AddSelSymptomToCdssOutput(string SymptomName, string PropertyName, string OptionName)
        {
            //首先清除所有以往添加的症候信息列表
            XmlNodeList SymptomsList = CdssOutputXml.GetElementsByTagName("Symptoms");
            XmlNode SymptomsNode = SymptomsList[0];
            if (SymptomsNode.ChildNodes.Count>0)
            {
                SymptomsNode.RemoveAll();
            }

            XmlElement SymptomElement = CdssOutputXml.CreateElement("Symptom");
            SymptomElement.SetAttribute("SymptomName", SymptomName);
            SymptomElement.SetAttribute("PropertyName", PropertyName);
            SymptomElement.SetAttribute("OptionName", OptionName);
            SymptomsNode.AppendChild(SymptomElement);
        }

        protected void GridViewUsersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedXmlInfo = new XmlDocument();
            SelectedXmlInfo.LoadXml(BasicalXmlStru); //初始化用于保存已经选择的症候相关信息

            CdssXmlForList = new XmlDocument();
            CdssXmlForList.LoadXml(BasicalXmlStru); //初始化算法传递参数信息

            CdssOutputXml = new XmlDocument();
            CdssOutputXml.LoadXml(BasicalXmlStru); //初始化算法传递参数信息

            DisplayUsersInfo();//显示已选择病人的基本信息在网页（姓名，性别，年龄，PHR编号）
            InitializeCdssXml();//初始化第一次问诊所需的症状列表信息
            GridViewUsersList_SelectedIndexChanged2();//为了防止页面刷新bug
            
        }
        protected void GridViewUsersList_SelectedIndexChanged2()
        {
            SelectedXmlInfo = new XmlDocument();
            SelectedXmlInfo.LoadXml(BasicalXmlStru); //初始化用于保存已经选择的症候相关信息

            CdssXmlForList = new XmlDocument();
            CdssXmlForList.LoadXml(BasicalXmlStru); //初始化算法传递参数信息

            CdssOutputXml = new XmlDocument();
            CdssOutputXml.LoadXml(BasicalXmlStru); //初始化算法传递参数信息

            DisplayUsersInfo();//显示已选择病人的基本信息在网页（姓名，性别，年龄，PHR编号）
            InitializeCdssXml();//初始化第一次问诊所需的症状列表信息

        }

        private void DisplayUsersInfo()
        {
            GridViewRow CurRow = this.GridViewUsersList.SelectedRow;
            string UserPhrNumber = CurRow.Cells[1].Text;

            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "Select Name,Sex,DOB from CommonUsersPhr where (PhrNumber=@UserPhrNumber)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@UserPhrNumber", UserPhrNumber);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            if (CurReader.Read())
            {
                string NameStr = CurReader["Name"].ToString().Trim();
                string SexStr = CurReader["Sex"].ToString().Trim();
                DateTime PatientDOB = (DateTime)CurReader["DOB"];
                DateTime CurTime = DateTime.Now;
                TimeSpan ExactAge = CurTime - PatientDOB;
                int AgeDays = ExactAge.Days;
                int AgeYears = Convert.ToInt16(AgeDays / 365.0);
                CurSelectedUsersPhrNumber = UserPhrNumber;
                string AgeStr = Convert.ToString(AgeYears) + "岁";
                LiteralBasicalInfo.Text = "姓名：" + NameStr +
                    "；      性别：" + SexStr +
                    "；      年龄：" + AgeStr +
                    "；      PHR编号：" + UserPhrNumber; 
            }            
            CurConn.Close();
        }
        private void InitializeCdssXml()
        {
            //初始化第一次问诊所需的症状列表信息
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT distinct [FindingName],count(*) as tmpCount FROM [DiagnosisData] " +
                "where (FindingType ='Symptom') group by FindingName order by tmpCount desc";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            string SymptomName = "";
            List<string> SmptomProperList = new List<string>();

            while (CurReader.Read())
            {
                SymptomName = CurReader["FindingName"].ToString().Trim();
                if (SymptomName!="")
                {
                    SmptomProperList.Add(SymptomName);
                }
            }

            XmlNodeList SymptomsList = CdssXmlForList.GetElementsByTagName("Symptoms");
            XmlNode SymtomsNode = SymptomsList[0];
            for (int i = 0; i < SmptomProperList.Count; i++)
            {
                SymptomName = SmptomProperList[i];
                XmlElement SymptomElement = CdssXmlForList.CreateElement("Symptom");
                SymptomElement.SetAttribute("SymptomName", SymptomName);
                SymptomElement.SetAttribute("PropertyName", "");
                SymptomElement.SetAttribute("OptionName", "");
                SymtomsNode.AppendChild(SymptomElement);
            }
            DisplaySymptomNameList();//症候列表显示
            CurConn.Close();
        }

        private List<string> GetSymptomNameList(XmlDocument CurXmlDoc)
        {
            List<string> SymptomNameList = new List<string>();
            XmlNodeList SymptomElementList = CurXmlDoc.GetElementsByTagName("Symptom");
            string tmpSymptomName = "";
            for (int i = 0; i < SymptomElementList.Count; i++)
            {
                tmpSymptomName = SymptomElementList[i].Attributes["SymptomName"].Value.Trim();
                int SymptomIndex = SymptomNameList.IndexOf(tmpSymptomName);
                if ((tmpSymptomName != "") && (SymptomIndex < 0))
                {
                    SymptomNameList.Add(tmpSymptomName);
                }
            }
            return SymptomNameList;
        }
        private void DisplaySymptomNameList()
        {
            List<string> OldList = GetSymptomNameList(SelectedXmlInfo);
            List<string> NewList = GetSymptomNameList(CdssXmlForList);
            DisplayBulletedList(this.blSymptomName, OldList, NewList);
            if (CurSelectedSymptomName!="")
            {
                DisplaySymptomProperty(CurSelectedSymptomName);//显示症候特征
                if (CurSelectedSymptomProperty!="")
                {
                    DisplaySymptomOption(CurSelectedSymptomName, CurSelectedSymptomProperty);//显示特征属性
                }
            }
            DisplayDiseaseAndProbList();
        }

        protected void blSymptomOption_Click(object sender, BulletedListEventArgs e)
        {
            //首先获取选项列表信息
            string SelSymptomName = CurSelectedSymptomName;
            string SelSymptomProperty = CurSelectedSymptomProperty;
            string SelSymptomOption = this.blSymptomOption.Items[e.Index].Value.ToString().Trim();
            CurSelectedSymptomOption = SelSymptomOption;
           // SelectedSymtoms();//看看已选的症候特征
            AddSelSymptomToCdssOutput(SelSymptomName, SelSymptomProperty, SelSymptomOption);
            AddSelSymptomToSelectedInfo(SelSymptomName, SelSymptomProperty, SelSymptomOption);
            CdssXmlForList = CdssAlgorithm.GetCdssOutput(CdssOutputXml);

            blSymptomName.Items.Clear();
            blSymptomProperty.Items.Clear();
            blSymptomOption.Items.Clear();
            DisplaySymptomNameList();
        }
        protected void SureButton_Click(object sender, EventArgs e)
        {
            String result=this.DropDownList1.SelectedValue.ToString().Trim();
            CdssAlgorithm.GetCdssResult(CdssOutputXml,SelectedXmlInfo,result);

        }
        private void SelectedSymtoms()
        {
            XmlDocument xml = SelectedXmlInfo;
            XmlDocument xml1 = SelectedXmlInfo;
        }



        protected void blSymptomProperty_Click(object sender, BulletedListEventArgs e)
        {
            //首先获取选项列表信息
            string SelSymptomName = CurSelectedSymptomName;
            string SelSymptomProperty = this.blSymptomProperty.Items[e.Index].Value.ToString().Trim();
            CurSelectedSymptomProperty = SelSymptomProperty;

            DisplaySymptomOption(SelSymptomName, SelSymptomProperty);
        }
        private void DisplaySymptomOption(string SelSymptomName, string SelSymptomProperty)
        {
            List<string> OldList = GetOptionNameList(SelSymptomName, SelSymptomProperty, SelectedXmlInfo);
            List<string> NewList = GetOptionNameList(SelSymptomName, SelSymptomProperty, CdssXmlForList);

            if (NewList.Count > 0)
            {
                DisplayBulletedList(this.blSymptomOption, OldList, NewList);
            }
            else
            {
                //如果没有说明知识库内容存在问题需要纠正完善
            }
        }
        protected void Chart1_Load(object sender, EventArgs e)
        {

        }

        protected void blDiagnosisProb_Click(object sender, BulletedListEventArgs e)
        {
            string CurDiagnosisName=blDiagnosisProb.Items[e.Index].ToString().Trim();
            string[] StrArray = CurDiagnosisName.Split(new char[2] { '(', ')' });
            CurDiagnosisName = StrArray[0];
            string CurIntroduction = GetCurIntroduction(CurDiagnosisName);
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(CurDiagnosisName);
            DropDownList1.Items.Add("其他");

            LiteralDiseaseIntro.Text = CurDiagnosisName+":"+CurIntroduction;
        }

        private string GetCurIntroduction(string CurDiseaseName)
        {
            string LiteralDiseaseIntro = "";

            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT [Description] FROM  [DiseasesBase] where ([ChineseName]=@CurDiseaseName)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@CurDiseaseName", CurDiseaseName);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            if (CurReader.Read())
            {
                string CurDescriptio = CurReader["Description"].ToString().Trim();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(CurDescriptio); //初始化用于保存已经选择的症候相关信息
                XmlNodeList CurNodes = xmlDoc.GetElementsByTagName("Introduction");
                XmlNode CurNode = CurNodes[0];
                LiteralDiseaseIntro = CurNode.InnerText;
            }
            CurConn.Close();
            return LiteralDiseaseIntro;
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }


    }
}