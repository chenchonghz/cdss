using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
namespace CdssProgram
{
    public struct SmptomProperyName  //用于保存症状及其相关属性和选项
    {
        public string SymptomName;
        public string PropertyName;
        public string OptionName;
    }

    public struct DiagnosisProb  //用于保存依据症候及其属性和选项获取的相关疾病发病概率
    {
        public string DiseaseName;
        public double ProbValue;
    }

    public struct DiagnosisProb1  //用于保存依据症候及其属性和选项获取的相关疾病发病概率
    {
        public string DiseaseName;
        public string DiseaseProb;
        public double SumProbValue;
    }
    public struct DiagnosisProb2  //用于保存依据症候及其属性和选项获取的相关疾病发病概率
    {
        public string DiseaseName;
        public string DiseaseProb;
        public double SumProbValue;
        public double ProbValue;

    }

    public class CdssAlgorithm
    {
        private static int CurrentUsersAge = 36; //保存当前用户的年龄信息

        public static string CDSSConnectionString; //用于接收用户数据连接字符串信息
        public static string CdssUsersPhrNumber;  //用于接收用户的个人健康档案编号信息

        public XmlDocument CdssAlgorithmPara;//用于存储多症候贝叶斯公式参数

        public static double SpecifityWeight = 0.6; //定义根据症状属性特征度（Specifity）的权重
        public static int PropertyListCount = 20; //定义每次获取症状属性列表的最大数量
        public static int DiseaseListCount = 6; //定义每次获取症状属性列表的时候，需要考虑的最多前几位的疾病信息
        public static double[] FreqDefine = new double[6] { 0.01, 0.19, 0.39, 0.59, 0.79, 0.99 }; //预定义频度级别与概率函数的关系
        public static double[] SpecDefine = new double[6] { 0.01, 0.15, 0.35, 0.55, 0.75, 0.95 }; //预定义特异度级别与概率函数的关系
        public static XmlDocument GetCdssOutput(XmlDocument InputXml)
        {
            XmlDocument OutputXml = InputXml;
            //OutputXml.Save("C://1.xml");
            int LoopCount = getLoopCount(InputXml);
            if (LoopCount == 0) //如果是第一次调用算法 
            {
                OutputXml = InitializeDiseaseList(OutputXml);//单症候贝叶斯公式
            }
            else if ((LoopCount > 0) && (LoopCount < 5))
            {
                OutputXml = GetDiseaseListWithSymptom(OutputXml);
            }

            return OutputXml;
        }

        private static XmlDocument GetDiseaseListWithSymptom(XmlDocument xmlDoc)
        {
            XmlNodeList SymptomList = xmlDoc.GetElementsByTagName("Symptom");
            XmlNode SymptomNode = SymptomList[0];
            string CurSymptomName = SymptomNode.Attributes["SymptomName"].Value.Trim();
            string CurPropertyName = SymptomNode.Attributes["PropertyName"].Value.Trim();
            string CurOptionName = SymptomNode.Attributes["OptionName"].Value.Trim();
            //根据症候名称、属性及特征获取相关疾病的发生概率
            List<DiagnosisProb1> DiagnosisProbList1 = GetSumProbList(xmlDoc, CurSymptomName, CurPropertyName, CurOptionName);
            List<DiagnosisProb2> DiagnosisProbList2 = BayesWithManySymptom(DiagnosisProbList1);//计算多症候贝叶斯公式
            List<DiagnosisProb> DiagnosisProbList = GetDiagnosisProbList(CurSymptomName, CurPropertyName, CurOptionName);
            //从以往得出的XML文件中获取相关疾病的发生概率
            //List<DiagnosisProb> PreviousProbList = GetPreviousProbList(xmlDoc);
            // List<DiagnosisProb> NewProbList = CalculateNewProbList(PreviousProbList, DiagnosisProbList);
            List<DiagnosisProb2> NewProbList1 = CalculateNewProbList1(DiagnosisProbList2, DiagnosisProbList);//多症候贝叶斯公式下疾病概率
            //按照疾病发生概率从大到小排列诊断列表
            //IEnumerable<DiagnosisProb> DiagnosisQuery = NewProbList.OrderBy(DiagnosisProb => DiagnosisProb.ProbValue);
            IEnumerable<DiagnosisProb2> DiagnosisQuery1 = NewProbList1.OrderBy(DiagnosisProb2 => DiagnosisProb2.ProbValue);
            // NewProbList = DiagnosisQuery.ToList();
            NewProbList1 = DiagnosisQuery1.ToList();
            //NewProbList.Reverse();
            NewProbList1.Reverse();
            //把疾病及其发生概率的信息添加到XML文档中
            //xmlDoc = RenewDocProblist(xmlDoc, NewProbList);
            xmlDoc = RenewDocProblist1(xmlDoc, NewProbList1);
            //获取排列前几位的疾病名称列表(其中DiseaseListCount预定义了需要考虑的前几位疾病信息)
            // List<string> RankTopDiseases = GetRankTopDiseases(NewProbList, DiseaseListCount);
            List<string> RankTopDiseases = GetRankTopDiseases1(NewProbList1, DiseaseListCount);
            //获取排位在前面几种疾病相关的症候特征信息(症候信息列表数量有自定义参数全局变量PropertyListCount决定)
            List<SmptomProperyName> NewSymptomProperty = GetNewSymptomProperty(RankTopDiseases, PropertyListCount);
            //把新的症候特征列表添加的XML文档中
            xmlDoc = RenewDocSymptomProperty(xmlDoc, NewSymptomProperty);
            //lcf tobecontinued 20141218
            return xmlDoc;
        }
        private static XmlDocument RenewDocSymptomProperty(XmlDocument xmlDoc, List<SmptomProperyName> SmptomProperList)
        {
            //首先清除所有以往添加的疾病症候特征信息列表
            int PropertyCount = SmptomProperList.Count;
            XmlNodeList SymptomsList = xmlDoc.GetElementsByTagName("Symptoms");
            XmlNode SymtomsNode = SymptomsList[0];
            if (SymtomsNode.ChildNodes.Count > 0)
            {
                SymtomsNode.RemoveAll();
            }
            string SymptomName = "";
            string PropertyName = "";
            string OptionName = "";
            for (int i = 0; i < PropertyCount; i++)
            {
                SmptomProperyName tmpSmptomPropery = SmptomProperList[i];
                SymptomName = tmpSmptomPropery.SymptomName;
                PropertyName = tmpSmptomPropery.PropertyName;
                OptionName = tmpSmptomPropery.OptionName;

                XmlElement SymptomElement = xmlDoc.CreateElement("Symptom");
                SymptomElement.SetAttribute("SymptomName", SymptomName);
                SymptomElement.SetAttribute("PropertyName", PropertyName);
                SymptomElement.SetAttribute("OptionName", OptionName);
                SymtomsNode.AppendChild(SymptomElement);

            }
            return xmlDoc;
        }
        private static List<SmptomProperyName> GetNewSymptomProperty(List<string> RankTopDiseases, int RankCount)
        {
            List<SmptomProperyName> NewSymptomProperty = new List<SmptomProperyName>();
            //首先判断最多可以考虑多少种疾病
            int DiseaseCount = RankTopDiseases.Count;
            int NeedConsiderred = System.Math.Min(DiseaseCount, RankCount);
            string tmpDiseaseName = "";
            string PartialCondition = "";

            //查询数据库获取排位靠前疾病的相关症候特征选项 
            for (int i = 0; i < NeedConsiderred; i++)
            {
                tmpDiseaseName = RankTopDiseases[i];
                if (PartialCondition == "")
                {
                    PartialCondition = "([DiseaseName]='" + tmpDiseaseName + "')";
                }
                else
                {
                    PartialCondition = PartialCondition + " or ([DiseaseName]='" + tmpDiseaseName + "')";
                }
            }

            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT Top " + RankCount.ToString() +
                " [FindingName],[PropertyName],[OptionName],[Specifity] " +
                " FROM [DiagnosisData] Where ([FindingType]='Symptom') and " +
                " (" + PartialCondition + ") " +
                " Order by [Specifity] desc";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurConn.Open();

            SqlDataReader CurReader = CurCmd.ExecuteReader();
            //string tmpSymptomName, tmpPropertyName, tmpOptionName;
            SmptomProperyName tmpSmptomPropery;
            while (CurReader.Read())
            {
                tmpSmptomPropery.SymptomName = CurReader["FindingName"].ToString().Trim();
                tmpSmptomPropery.PropertyName = CurReader["PropertyName"].ToString().Trim();
                tmpSmptomPropery.OptionName = CurReader["OptionName"].ToString().Trim();
                NewSymptomProperty.Add(tmpSmptomPropery);
            }
            return NewSymptomProperty;
        }
        private static List<string> GetRankTopDiseases(List<DiagnosisProb> ProbList, int RankCount)
        {
            List<string> RankTopDiseases = new List<string>();
            string tmpDisease;
            for (int i = 0; i < RankCount; i++)
            {
                tmpDisease = ProbList[i].DiseaseName;
                RankTopDiseases.Add(tmpDisease);
            }
            return RankTopDiseases;
        }
        private static List<string> GetRankTopDiseases1(List<DiagnosisProb2> ProbList, int RankCount)
        {
            List<string> RankTopDiseases = new List<string>();
            string tmpDisease;
            if (RankCount < ProbList.Count)
            {
                for (int i = 0; i < RankCount; i++)
                {
                    tmpDisease = ProbList[i].DiseaseName;
                    RankTopDiseases.Add(tmpDisease);
                }
            }
            else
            {
                for (int i = 0; i < ProbList.Count; i++)
                {
                    tmpDisease = ProbList[i].DiseaseName;
                    RankTopDiseases.Add(tmpDisease);
                }
            }
            return RankTopDiseases;
        }
        private static XmlDocument RenewDocProblist(XmlDocument xmlDoc, List<DiagnosisProb> NewProbList)
        {
            //首先清除所有以往添加的疾病及其发生概率信息列表
            XmlNodeList DiagnosesList = xmlDoc.GetElementsByTagName("Diagnoses");
            XmlNode DiagnosisNode = DiagnosesList[0];
            if (DiagnosisNode.ChildNodes.Count > 0)
            {
                DiagnosisNode.RemoveAll();
            }
            string DiseaseName;
            double ProbValue;
            for (int i = 0; i < NewProbList.Count; i++)
            {
                DiseaseName = NewProbList[i].DiseaseName;
                ProbValue = NewProbList[i].ProbValue;
                XmlElement DiagnosisElement = xmlDoc.CreateElement("Diagnosis");
                DiagnosisElement.SetAttribute("DiseaseName", DiseaseName);
                DiagnosisElement.SetAttribute("ProbValue", Convert.ToString(ProbValue));
                DiagnosisNode.AppendChild(DiagnosisElement);
            }
            return xmlDoc;
        }

        private static XmlDocument RenewDocProblist1(XmlDocument xmlDoc, List<DiagnosisProb2> NewProbList)
        {
            //首先清除所有以往添加的疾病及其发生概率信息列表
            XmlNodeList DiagnosesList = xmlDoc.GetElementsByTagName("Diagnoses");
            XmlNode DiagnosisNode = DiagnosesList[0];
            if (DiagnosisNode.ChildNodes.Count > 0)
            {
                DiagnosisNode.RemoveAll();
            }
            string DiseaseName;
            string Incidence;
            double SumProbValue;
            double ProbValue;
            for (int i = 0; i < NewProbList.Count; i++)
            {
                DiseaseName = NewProbList[i].DiseaseName;
                ProbValue = NewProbList[i].ProbValue;
                Incidence = NewProbList[i].DiseaseProb;
                SumProbValue = NewProbList[i].SumProbValue;
                XmlElement DiagnosisElement = xmlDoc.CreateElement("Diagnosis");
                DiagnosisElement.SetAttribute("DiseaseName", DiseaseName);
                DiagnosisElement.SetAttribute("ProbValue", Convert.ToString(ProbValue));
                DiagnosisElement.SetAttribute("Incidence", Incidence);
                DiagnosisElement.SetAttribute("SumProbValue", Convert.ToString(SumProbValue));
                DiagnosisNode.AppendChild(DiagnosisElement);
            }
            return xmlDoc;
        }
        private static List<DiagnosisProb> CalculateNewProbList(List<DiagnosisProb> OldList, List<DiagnosisProb> NewList)
        {
            List<DiagnosisProb> DiagnosisProbList = new List<DiagnosisProb>();
            DiagnosisProb tmpDiagnosisProb;
            string NewDiseaseName, OldDiseaseName;
            double NewProbValue, OldProbValue, tmpProbValue;

            for (int j = 0; j < OldList.Count; j++)
            {
                tmpDiagnosisProb = OldList[j];
                OldDiseaseName = OldList[j].DiseaseName;
                OldProbValue = OldList[j].ProbValue;
                for (int i = 0; i < NewList.Count; i++)
                {
                    NewDiseaseName = NewList[i].DiseaseName;
                    NewProbValue = NewList[i].ProbValue;
                    if (OldDiseaseName == NewDiseaseName)
                    {
                        tmpProbValue = BayesWithTwoProbability(NewProbValue, OldProbValue);
                        tmpDiagnosisProb.ProbValue = tmpProbValue;
                    }
                }
                DiagnosisProbList.Add(tmpDiagnosisProb);
            }
            DiagnosisProbList = NormalizeProblist(DiagnosisProbList);//归一化疾病概率
            return DiagnosisProbList;
        }

        private static List<DiagnosisProb2> CalculateNewProbList1(List<DiagnosisProb2> OldList, List<DiagnosisProb> NewList)
        {
            List<DiagnosisProb2> DiagnosisProbList = new List<DiagnosisProb2>();
            DiagnosisProb2 tmpDiagnosisProb;
            string NewDiseaseName, OldDiseaseName;
            double NewProbValue, OldProbValue, tmpProbValue;

            for (int j = 0; j < OldList.Count; j++)
            {
                tmpDiagnosisProb = OldList[j];
                OldDiseaseName = OldList[j].DiseaseName;
                OldProbValue = OldList[j].ProbValue;
                for (int i = 0; i < NewList.Count; i++)
                {
                    NewDiseaseName = NewList[i].DiseaseName;
                    NewProbValue = NewList[i].ProbValue;
                    if (OldDiseaseName == NewDiseaseName)
                    {
                        tmpProbValue = BayesWithTwoProbability(NewProbValue, OldProbValue);
                        tmpDiagnosisProb.ProbValue = tmpProbValue;
                    }
                }
                DiagnosisProbList.Add(tmpDiagnosisProb);
            }
            DiagnosisProbList = NormalizeProblist1(DiagnosisProbList);//归一化疾病概率
            return DiagnosisProbList;
        }


        private static List<DiagnosisProb2> NormalizeProblist1(List<DiagnosisProb2> OldList)//归一化疾病概率
        {
            List<DiagnosisProb2> NewList = new List<DiagnosisProb2>();
            DiagnosisProb2 tmpDiagnosisProb;
            double tmpProbValue, totalProbValue;
            totalProbValue = 0.0;
            for (int i = 0; i < OldList.Count; i++)
            {
                tmpProbValue = OldList[i].ProbValue;
                totalProbValue = totalProbValue + tmpProbValue;
            }
            for (int i = 0; i < OldList.Count; i++)
            {
                tmpDiagnosisProb = OldList[i];
                tmpProbValue = tmpDiagnosisProb.ProbValue / totalProbValue;
                tmpDiagnosisProb.ProbValue = tmpProbValue;
                NewList.Add(tmpDiagnosisProb);
            }
            return NewList;
        }
        private static List<DiagnosisProb> NormalizeProblist(List<DiagnosisProb> OldList)//归一化疾病概率
        {
            List<DiagnosisProb> NewList = new List<DiagnosisProb>();
            DiagnosisProb tmpDiagnosisProb;
            double tmpProbValue, totalProbValue;
            totalProbValue = 0.0;
            for (int i = 0; i < OldList.Count; i++)
            {
                tmpProbValue = OldList[i].ProbValue;
                totalProbValue = totalProbValue + tmpProbValue;
            }
            for (int i = 0; i < OldList.Count; i++)
            {
                tmpDiagnosisProb = OldList[i];
                tmpProbValue = tmpDiagnosisProb.ProbValue / totalProbValue;
                tmpDiagnosisProb.ProbValue = tmpProbValue;
                NewList.Add(tmpDiagnosisProb);
            }
            return NewList;
        }
        private static double BayesWithTwoProbability(double NewProbValue, double OldProbValue)
        {
            NewProbValue = OldProbValue + NewProbValue * (1 + SpecifityWeight);
            //lcf tobemodified further 20141219
            return NewProbValue;
        }
        private static List<DiagnosisProb> GetPreviousProbList(XmlDocument xmlDoc)
        {
            List<DiagnosisProb> DiagnosisProbList = new List<DiagnosisProb>();
            XmlNodeList DiagnosisElementList = xmlDoc.GetElementsByTagName("Diagnosis");
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
            return DiagnosisProbList;
        }

        //多症候贝叶斯
        private static List<DiagnosisProb1> GetSumProbList(XmlDocument xmlDoc, string CurSymptomName, string CurPropertyName, string CurOptionName)//计算多症候下疾病名称及先验概率，贝叶斯中的P(SK|Di)积参数，用于多症候贝叶斯公式
        {
            List<DiagnosisProb1> DiagnosisProbList = new List<DiagnosisProb1>();
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            //SqlConnection CurConn1 = new SqlConnection(CDSSConnectionString);
            XmlNodeList DiagnosisElementList = xmlDoc.GetElementsByTagName("Diagnosis");
            string tmpDiseaseName1;
            string tmpDiseaseProb;
            string temSumProbValue;
            double x;
            for (int i = 0; i < DiagnosisElementList.Count; i++)
            {
                tmpDiseaseName1 = DiagnosisElementList[i].Attributes["DiseaseName"].Value.Trim();
                tmpDiseaseProb = DiagnosisElementList[i].Attributes["Incidence"].Value.Trim();
                temSumProbValue = DiagnosisElementList[i].Attributes["SumProbValue"].Value.Trim();
                x = Convert.ToDouble(temSumProbValue);
                string SqlString = "SELECT distinct [DiseaseName],[Frequency] FROM [DiagnosisData]" +
                             "where (PropertyName is null) and (OptionName is null) and " +
                             "FindingType ='Symptom' and DiseaseName=@tmpDiseaseName1 and FindingName=@CurSymptomName";
                SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
                CurCmd.Parameters.AddWithValue("@CurSymptomName", CurSymptomName);
                CurCmd.Parameters.AddWithValue("@CurPropertyName", CurPropertyName);
                CurCmd.Parameters.AddWithValue("@CurOptionName", CurOptionName);
                CurCmd.Parameters.AddWithValue("@tmpDiseaseName1", tmpDiseaseName1);
                CurConn.Open();
                SqlDataReader CurReader = CurCmd.ExecuteReader();
                int tmpFrequency;
                double tmpSumProbValue;
                //int tmpFrequency1;
                //double tmpDiseaseProb=0.0;
                string tmpDiseaseName;
                //Convert.ToDouble(AgeEffect);
                DiagnosisProb1 tmpFindingSpecifity;
                while (CurReader.Read())
                {
                    tmpDiseaseName = CurReader["DiseaseName"].ToString();
                    tmpFrequency = Convert.ToInt16(CurReader["Frequency"].ToString());
                    tmpSumProbValue = FreqDefine[tmpFrequency];
                    tmpSumProbValue = tmpSumProbValue * x;//多症候贝叶斯中的P(SK|Di)的积；
                    /*string Sql = "SELECT [Frequency] FROM [DiagnosisData] " +
                      "where (DiseaseName=@DiseaseName) and (FindingName='易感因素') " +
                      "and (PropertyName is null) and (OptionName is null)";
                    SqlCommand CurCmd1 = new SqlCommand(Sql, CurConn1);
                    CurCmd1.Parameters.AddWithValue("@DiseaseName", tmpDiseaseName);
                    CurConn1.Open();//error!!!!
                    SqlDataReader CurReader1 = CurCmd1.ExecuteReader();
                    while (CurReader1.Read())
                    {
                        tmpFrequency1 = Convert.ToInt16(CurReader1["Frequency"].ToString());
                        tmpDiseaseProb = FreqDefine[tmpFrequency];
                    }*/
                    tmpFindingSpecifity.DiseaseName = tmpDiseaseName;
                    tmpFindingSpecifity.SumProbValue = tmpSumProbValue;
                    tmpFindingSpecifity.DiseaseProb = tmpDiseaseProb;
                    //tmpFindingSpecifity.ProbValue = 1;
                    DiagnosisProbList.Add(tmpFindingSpecifity);
                }
                CurConn.Close();
                // CurConn1.Close();
            }
            return DiagnosisProbList;

        }


        private static List<DiagnosisProb> GetDiagnosisProbList(string CurSymptomName, string CurPropertyName, string CurOptionName)
        {
            List<DiagnosisProb> DiagnosisProbList = new List<DiagnosisProb>();
            //查询数据库获取当前症候特征选项的特异性

            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT distinct [DiseaseName],[Specifity] FROM [DiagnosisData]" +
                               "where ((PropertyName =@CurPropertyName) and (OptionName =@CurOptionName) and " +
                               "(FindingType ='Symptom')) and (FindingName=@CurSymptomName)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@CurSymptomName", CurSymptomName);
            CurCmd.Parameters.AddWithValue("@CurPropertyName", CurPropertyName);
            CurCmd.Parameters.AddWithValue("@CurOptionName", CurOptionName);
            CurConn.Open();

            SqlDataReader CurReader = CurCmd.ExecuteReader();
            DiagnosisProb tmpFindingSpecifity;
            int tmpSpecifity;
            double tmpDiagnosisProb;
            string tmpDiseaseName;
            while (CurReader.Read())
            {
                tmpDiseaseName = CurReader["DiseaseName"].ToString();
                tmpSpecifity = Convert.ToInt16(CurReader["Specifity"].ToString());
                tmpDiagnosisProb = SpecDefine[tmpSpecifity];
                tmpFindingSpecifity.DiseaseName = tmpDiseaseName;
                tmpFindingSpecifity.ProbValue = tmpDiagnosisProb;
                DiagnosisProbList.Add(tmpFindingSpecifity);
            }
            return DiagnosisProbList;
        }
        private static XmlDocument GetChiefcompaintPropertyInfo(string ChiefComplaint, XmlDocument xmlDoc)
        {
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT distinct [FindingName], [PropertyName] ,[OptionName],[Specifity] FROM [DiagnosisData]" +
                               "where ((PropertyName is not null) and (OptionName is not null) and " +
                               "(FindingType ='Symptom')) and (FindingName=@ChiefComplaint) order by [Specifity] desc";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@ChiefComplaint", ChiefComplaint);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();

            List<SmptomProperyName> SmptomProperList = new List<SmptomProperyName>();
            SmptomProperyName tmpPropertyName;
            while (CurReader.Read())
            {
                tmpPropertyName.SymptomName = CurReader["FindingName"].ToString().Trim();
                tmpPropertyName.PropertyName = CurReader["PropertyName"].ToString().Trim();
                tmpPropertyName.OptionName = CurReader["OptionName"].ToString().Trim();
                SmptomProperList.Add(tmpPropertyName);
            }
            int PropertyCount = SmptomProperList.Count;
            XmlNodeList SymptomsList = xmlDoc.GetElementsByTagName("Symptoms");
            XmlNode SymtomsNode = SymptomsList[0];
            string SymptomName = "";
            string PropertyName = "";
            string OptionName = "";
            for (int i = 0; i < PropertyCount; i++)
            {
                SmptomProperyName tmpSmptomPropery = SmptomProperList[i];
                SymptomName = tmpSmptomPropery.SymptomName;
                PropertyName = tmpSmptomPropery.PropertyName;
                OptionName = tmpSmptomPropery.OptionName;

                XmlElement SymptomElement = xmlDoc.CreateElement("Symptom");
                SymptomElement.SetAttribute("SymptomName", SymptomName);
                SymptomElement.SetAttribute("PropertyName", PropertyName);
                SymptomElement.SetAttribute("OptionName", OptionName);
                SymtomsNode.AppendChild(SymptomElement);

            }
            return xmlDoc;
        }

        private static int GetCurrentUsersAge(String PhrNumber)
        {
            int UsersAge = 36;
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT [DOB] FROM [CommonUsersPhr] where (PhrNumber=@PhrNumber)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@PhrNumber", PhrNumber);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            while (CurReader.Read())
            {
                DateTime PatientDOB = (DateTime)CurReader["DOB"];
                DateTime CurTime = DateTime.Now;
                TimeSpan ExactAge = CurTime - PatientDOB;
                int AgeDays = ExactAge.Days;
                UsersAge = Convert.ToInt16(AgeDays / 365.0);
            }
            return UsersAge;
        }
        private static XmlDocument InitializeDiseaseList(XmlDocument xmlDoc)
        {
            //获取用户健康档案疾病信息，如年龄等，用于进一步的算法计算参数
            CurrentUsersAge = GetCurrentUsersAge(CdssUsersPhrNumber);
            //初始化疾病名称及发生概率列表
            XmlNode RootNode = xmlDoc.FirstChild;
            CreateNewNode(xmlDoc, RootNode, "LoopCount", "1");
            string ChiefComplaint = "";
            XmlNodeList SymptomList = xmlDoc.GetElementsByTagName("Symptom");
            //如果InputXML有症状名称这一项
            if (SymptomList.Count > 0)
            {
                ChiefComplaint = SymptomList[0].Attributes["SymptomName"].Value.Trim(); //第一次输入的症状，即主诉
                List<string> DiseasesList = GetDieasesList(ChiefComplaint); //通过主诉获取所有可能的疾病名称列表
                //根据最初可能疾病的数量，初始化每一种疾病概率的大小（平均值）
                int DiseaseCount = DiseasesList.Count;
                double AverageProb = 1 / (double)DiseaseCount;
                string ProbValueStr = AverageProb.ToString("N3");//保留小数点后三位
                //在Xml文档中增加诊断信息列表项目
                XmlNodeList DiagnosesElementList = xmlDoc.GetElementsByTagName("Diagnoses");
                XmlNode DiagnosesNode = DiagnosesElementList[0];
                string tmpStr = "";
                for (int i = 0; i < DiseaseCount; i++)
                {
                    tmpStr = DiseasesList[i];
                    XmlElement DiagnosisElement = xmlDoc.CreateElement("Diagnosis");
                    DiagnosisElement.SetAttribute("DiseaseName", tmpStr);
                    DiagnosisElement.SetAttribute("ProbValue", ProbValueStr);
                    DiagnosisElement.SetAttribute("Incidence", "12");
                    DiagnosisElement.SetAttribute("SumProbValue", "1");
                    DiagnosesNode.AppendChild(DiagnosisElement);
                }
                //根据患者的主诉计算可能相关的疾病发生概率
                xmlDoc = CalculateProbAndIncidence(ChiefComplaint, xmlDoc);
                //第一次获取与主诉症状相关的所有症状属性及其选项信息
                xmlDoc = GetChiefcompaintPropertyInfo(ChiefComplaint, xmlDoc);
            }
            else
            {
                //InputXML没有症状信息，输入错误！
            }
            return xmlDoc;
        }

        private static XmlDocument CalculateProbAndIncidence(string ChiefComplaint, XmlDocument xmlDoc)
        {
            //根据患者年龄计算对应的发病率；根据主诉症状和贝叶斯公式计算机每个疾病的发生概率
            XmlNodeList DiseaseNodeList = xmlDoc.GetElementsByTagName("Diagnosis");
            string DiseaseName = "";
            //string ProbValue="";
            string Incidence = "";
            string SymptomRate = "";
            string AgeEffect = "";
            string DiseaseFreq = "";
            int CurPatientAge = CurrentUsersAge;
            double x, y, z;
            List<string> ProbDiseaseList = new List<string>();
            List<string> ProbSympWithDisList = new List<string>();
            List<string> ProbDisWithSympList = new List<string>();
            for (int i = 0; i < DiseaseNodeList.Count; i++)
            {
                XmlNode DiseaseNode = DiseaseNodeList[i];
                DiseaseName = DiseaseNode.Attributes["DiseaseName"].Value;

                AgeEffect = GetAgeEffect(DiseaseName, CurPatientAge);//易感因子：年龄
                DiseaseFreq = GetDiseaseFreq(DiseaseName);//0.01, 0.19, 0.39, 0.59, 0.79, 0.99这六个概率 训练？？？
                x = Convert.ToDouble(AgeEffect);
                y = Convert.ToDouble(DiseaseFreq);
                z = x * y / 100;
                Incidence = Convert.ToString(z);//相当于贝叶斯公式里面的P(Di)，归一化==无影响 
                DiseaseNode.Attributes["Incidence"].Value = Incidence;
                ProbDiseaseList.Add(Incidence);

                SymptomRate = GetSymptomRate(DiseaseName, ChiefComplaint);// 相当于贝叶斯公式里面的P(S|Di) 0.01, 0.15, 0.35, 0.55, 0.75, 0.95  训练？？？
                ProbSympWithDisList.Add(SymptomRate);
            }
            //ProbDiseaseList = NormalizePd(ProbDiseaseList);//对P(Di）进行归一化
            ProbDisWithSympList = BayesWithSingleSymptom(ProbDiseaseList, ProbSympWithDisList);
            for (int i = 0; i < ProbDisWithSympList.Count; i++)
            {
                DiseaseNodeList[i].Attributes["ProbValue"].Value = ProbDisWithSympList[i];
            }
            return xmlDoc;
        }
        private static List<string> NormalizePd(List<string> Pd)
        {
            List<string> pd = new List<string>();
            double SumPd = 0.0;
            double x, tempd;
            string tempdstr;
            for (int i = 0; i < Pd.Count; i++)
            {
                x = Convert.ToDouble(Pd[i]);
                SumPd = SumPd + x;
            }
            for (int i = 0; i < Pd.Count; i++)
            {
                x = Convert.ToDouble(Pd[i]);
                tempd = x / SumPd;
                tempdstr = Convert.ToString(tempd);
                pd.Add(tempdstr);
            }
            return pd;
        }
        private static List<string> BayesWithSingleSymptom(List<string> Pd, List<string> Psd)
        {
            List<string> Pds = new List<string>();
            double SumProduct = 0.0;
            double tmpPds, x, y, z;
            string tmpStr;
            for (int i = 0; i < Pd.Count; i++)
            {
                x = Convert.ToDouble(Pd[i]);
                y = Convert.ToDouble(Psd[i]);
                z = x * y;
                SumProduct = SumProduct + z;
            }

            for (int i = 0; i < Pd.Count; i++)
            {
                x = Convert.ToDouble(Pd[i]);
                y = Convert.ToDouble(Psd[i]);
                z = x * y;
                tmpPds = z / SumProduct;
                tmpStr = Convert.ToString(tmpPds);
                Pds.Add(tmpStr);
            }
            return Pds;
        }

        private static List<DiagnosisProb2> BayesWithManySymptom(List<DiagnosisProb1> GetSumProbList)
        {
            List<DiagnosisProb2> DiagnosisProbList = new List<DiagnosisProb2>();
            DiagnosisProb2 tmpDiagnosisProb;
            double x, y, totalSumProbValue = 0.0;
            for (int i = 0; i < GetSumProbList.Count; i++)
            {
                x = Convert.ToDouble(GetSumProbList[i].DiseaseProb) * GetSumProbList[i].SumProbValue;
                totalSumProbValue = totalSumProbValue + x;
            }
            for (int i = 0; i < GetSumProbList.Count; i++)
            {
                x = Convert.ToDouble(GetSumProbList[i].DiseaseProb) * GetSumProbList[i].SumProbValue;
                y = x / totalSumProbValue;
                tmpDiagnosisProb.DiseaseName = GetSumProbList[i].DiseaseName;
                tmpDiagnosisProb.DiseaseProb = GetSumProbList[i].DiseaseProb;
                tmpDiagnosisProb.SumProbValue = GetSumProbList[i].SumProbValue;
                tmpDiagnosisProb.ProbValue = y;
                DiagnosisProbList.Add(tmpDiagnosisProb);

            }

            return DiagnosisProbList;

        }
        private static string GetSymptomRate(string DiseaseName, string ChiefComplaint)
        {
            int SymptomRate = 0;
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT [Frequency] FROM [DiagnosisData] " +
                      "where (DiseaseName=@DiseaseName) and (FindingName=@ChiefComplaint) " +
                      "and (PropertyName is null) and (OptionName is null)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@DiseaseName", DiseaseName);
            CurCmd.Parameters.AddWithValue("@ChiefComplaint", ChiefComplaint);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            while (CurReader.Read())
            {
                SymptomRate = Convert.ToInt16(CurReader["Frequency"].ToString());
            }
            double CurSymptomRate = FreqDefine[SymptomRate];
            return Convert.ToString(CurSymptomRate);
        }

        private static string GetDiseaseFreq(string DiseaseName)
        {
            int DisFrequency = 0;
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT [Frequency] FROM [DiagnosisData] " +
                      "where (DiseaseName=@DiseaseName) and (FindingName='易感因素') " +
                      "and (PropertyName is null) and (OptionName is null)";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@DiseaseName", DiseaseName);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            while (CurReader.Read())
            {
                DisFrequency = Convert.ToInt16(CurReader["Frequency"].ToString());//
            }
            double CurDiseaseFreq = FreqDefine[DisFrequency];
            return Convert.ToString(CurDiseaseFreq);
        }
        private static string GetAgeEffect(string DiseaseName, int CurPatientAge)
        {
            string IncidenceRate = "12";//？？？
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT [OptionName] FROM [DiagnosisData] " +
                    "where (DiseaseName=@DiseaseName) and (FindingName='易感因素') and (PropertyName='年龄')";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@DiseaseName", DiseaseName);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            string OptionName = "";
            while (CurReader.Read())
            {
                OptionName = CurReader["OptionName"].ToString().Trim();
            }
            CurConn.Close();
            SqlString = "SELECT [Dependent] FROM [FunctionSetting] " +
                    "where (FunctionTitle=@OptionName) and (Independent=@CurPatientAge)";
            CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@OptionName", OptionName);
            CurCmd.Parameters.AddWithValue("@CurPatientAge", CurPatientAge);
            CurConn.Open();
            CurReader = CurCmd.ExecuteReader();
            while (CurReader.Read())
            {
                IncidenceRate = CurReader["Dependent"].ToString().Trim();
            }
            return IncidenceRate;
        }

        private static List<SmptomProperyName> AddSymptomPropertyList(XmlDocument xmlDoc, List<string> DiseasesList)
        {
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT distinct [FindingName], [PropertyName] ,[OptionName] FROM [DiagnosisData]" +
                               "where ((PropertyName is not null) and (OptionName is not null) and (FindingType ='Symptom'))"; // and ()";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            //CurCmd.Parameters.AddWithValue("@ChiefComplaint", ChiefComplaint);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();

            List<SmptomProperyName> SmptomProperList = new List<SmptomProperyName>();
            SmptomProperyName tmpPropertyName;
            while (CurReader.Read())
            {
                tmpPropertyName.SymptomName = CurReader["FindingName"].ToString().Trim();
                tmpPropertyName.PropertyName = CurReader["PropertyName"].ToString().Trim();
                tmpPropertyName.OptionName = CurReader["OptionName"].ToString().Trim();
                SmptomProperList.Add(tmpPropertyName);
            }
            return SmptomProperList;
        }
        private static List<string> GetDieasesList(string ChiefComplaint)
        {
            SqlConnection CurConn = new SqlConnection(CDSSConnectionString);
            string SqlString = "SELECT distinct [DiseaseName] FROM  [DiagnosisData] where [FindingName]=@ChiefComplaint";
            SqlCommand CurCmd = new SqlCommand(SqlString, CurConn);
            CurCmd.Parameters.AddWithValue("@ChiefComplaint", ChiefComplaint);
            CurConn.Open();
            SqlDataReader CurReader = CurCmd.ExecuteReader();
            List<string> DiseasesList = new List<string>();
            string tmpStr = "";
            while (CurReader.Read())
            {
                tmpStr = CurReader["DiseaseName"].ToString().Trim();
                DiseasesList.Add(tmpStr);
            }
            return DiseasesList;
        }
        private static XmlDocument CreateNewNode(XmlDocument xmlDoc, XmlNode CurNode, string NodeName, string NodeText)
        {
            XmlNode NewNode = xmlDoc.CreateNode(XmlNodeType.Element, NodeName, String.Empty);
            NewNode.InnerText = NodeText;
            CurNode.AppendChild(NewNode);
            //xmlDoc.FirstChild.AppendChild(NewNode);
            return xmlDoc;
        }
        private static int getLoopCount(XmlDocument InputXml)
        {
            XmlNodeList tmpNodelist = InputXml.GetElementsByTagName("LoopCount");

            if (tmpNodelist.Count == 0)
            {
                return 0;
            }
            else
            {
                XmlNode LoopCountNode = tmpNodelist[0];
                string LoopCountStr = LoopCountNode.InnerText;
                int RetValue = Convert.ToInt16(LoopCountStr);
                return RetValue;
            }
        }
    }
}
