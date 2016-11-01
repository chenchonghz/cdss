using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Text;
using CdssProgram;
namespace CDSS
{
    
    public partial class DoctorsCDSS : System.Web.UI.Page
    {
        public static int i ;
        public static int j ;
        public static int k ;
        public static int i1;
        public static int j1;
        public static int k1;
        public static int o=1;
        public static int p= 1;
        string Findings;
        string Finding;
        string FindingName;
        static string FN;
        static string PN;
        static string ON;
        static string XmlInfoInput;
        public string XmlArithmetic(string XmlInput)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlInput);

            //xmlDoc.Save("c:/InputXML.xml");

            XmlDocument OutputXmlDoc = CdssAlgorithm.GetCdssOutput(xmlDoc);
            string OutputString = OutputXmlDoc.OuterXml;

            return OutputString;
        }
        public void CreateXmlFile()
        {          
            string str;          
            if (XmlInfoInput == null)
            {
                str = @"<XmlInfoInput><Symptoms></Symptoms><Signs></Signs><Laboratorys></Laboratorys><Radiologys></Radiologys><OtherExams></OtherExams></XmlInfoInput>";
            }
            else
            {
                str = XmlInfoInput;
            }         
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            XmlElement root = xmlDoc.DocumentElement;   //获取根节点 <XmlInfoInput>
            XmlNode XmlFindings = xmlDoc.SelectSingleNode("/XmlInfoInput/" + Findings + "");//选择FindingsName节点
            XmlNode XmlFinding = XmlFindings.SelectSingleNode("" + Finding + "[" + FindingName + "='" + FN + "']");
            if (XmlFinding == null)//若对应的症候不存在，则新建该症候，并保存该症候的信息
            {
                XmlFinding = xmlDoc.CreateElement(Finding);
                XmlFindings.AppendChild(XmlFinding);
                XmlElement XmlFindingName = xmlDoc.CreateElement(FindingName);
                XmlFindingName.InnerText = FN;// BulletedList1.Items[i].Text.Trim();
                XmlFinding.AppendChild(XmlFindingName);
                XmlElement XmlProperties = xmlDoc.CreateElement("Properties");
                XmlFinding.AppendChild(XmlProperties);
                XmlElement XmlProperty = xmlDoc.CreateElement("Property");
                XmlProperties.AppendChild(XmlProperty);
                XmlElement XmlPropertyName = xmlDoc.CreateElement("PropertyName");
                XmlPropertyName.InnerText = PN;// BulletedList2.Items[j].Text.Trim();
                XmlProperty.AppendChild(XmlPropertyName);
                XmlElement XmlOptionName = xmlDoc.CreateElement("OptionName");
                XmlOptionName.InnerText = ON;// BulletedList3.Items[k].Text.Trim();
                XmlProperty.AppendChild(XmlOptionName);
            }
            else
            {
                XmlNode XmlProperty = XmlFinding.SelectSingleNode("Properties/Property[PropertyName='" + PN + "']");
                if (XmlProperty == null)//若该症候下的属性不存在，则新建属性，并保存属性的信息
                {
                    if (PN != null | ON != null)
                    {
                        XmlNode XmlProperties = XmlFinding.SelectSingleNode("Properties");
                        XmlProperty = xmlDoc.CreateElement("Property");
                        XmlProperties.AppendChild(XmlProperty);
                        XmlElement XmlPropertyName = xmlDoc.CreateElement("PropertyName");
                        XmlPropertyName.InnerText = PN;// BulletedList2.Items[j].Text.Trim();
                        XmlProperty.AppendChild(XmlPropertyName);
                        XmlElement XmlOptionName = xmlDoc.CreateElement("OptionName");
                        XmlOptionName.InnerText = ON;// BulletedList3.Items[k].Text.Trim();
                        XmlProperty.AppendChild(XmlOptionName);
                    }
                }
                else//若属性已存在，修改该属性的选项值
                {
                    XmlNode XmlOptionName = XmlProperty.SelectSingleNode("OptionName");
                    XmlOptionName.InnerText = ON;// BulletedList3.Items[k].Text.Trim();
                }
            }
            XmlInfoInput=xmlDoc.InnerXml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlInfoInput);
            doc.Save("c://XmlInfoInput.xml");//便于测试  将XmlInfoInput变量转换成XmlInfoInput.xml存至c盘
            DisplayXmlFile();           

        }
        public void DisplayXmlFile()//将XmlInfoOutput显示在BulletedList中
        {
            string XmlInfoOutput = XmlArithmetic(XmlInfoInput);
            BulletedList1.Items.Clear();
            BulletedList2.Items.Clear();
            BulletedList3.Items.Clear();
            BulletedList4.Items.Clear();
            BulletedList5.Items.Clear();
            BulletedList6.Items.Clear();
            BulletedListLaboratory.Items.Clear();
            BulletedListRadiology.Items.Clear();
            BulletedListOtherExam.Items.Clear();
            XmlDocument reader = new XmlDocument();
            reader.LoadXml(XmlInfoOutput);
            XmlElement root = reader.DocumentElement;   //获取根节点 

            XmlNodeList SymptomNodes = root.GetElementsByTagName("Symptom"); 
            foreach (XmlNode xmlNode in SymptomNodes)
            {
                string SN = xmlNode.SelectSingleNode("SymptomName").InnerText;
                BulletedList1.Items.Add(SN);
            }
            XmlNodeList SignNodes = root.GetElementsByTagName("Sign");
            foreach (XmlNode xmlNode in SignNodes)
            {
                string SN = xmlNode.SelectSingleNode("SignName").InnerText;
                BulletedList4.Items.Add(SN);
            }
            XmlNodeList LaboratoryNodes = root.GetElementsByTagName("Laboratory");
            foreach (XmlNode xmlNode in LaboratoryNodes)
            {
                string LN = xmlNode.SelectSingleNode("LaboratoryName").InnerText;
                BulletedListLaboratory.Items.Add(LN);
            }
            XmlNodeList RadiologyNodes = root.GetElementsByTagName("Radiology");
            foreach (XmlNode xmlNode in RadiologyNodes)
            {
                string RN = xmlNode.SelectSingleNode("RadiologyName").InnerText;
                BulletedListRadiology.Items.Add(RN);
            }
            XmlNodeList OtherExamNodes = root.GetElementsByTagName("OtherExam");
            foreach (XmlNode xmlNode in OtherExamNodes)
            {
                string OEN = xmlNode.SelectSingleNode("OtherExamName").InnerText;
                BulletedListOtherExam.Items.Add(OEN);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["type"] = "Doctor"; //lcf20141214 for debug only
            Session["UserName"] = "Test"; //lcf20141214 for debug only
            if (!IsPostBack)
            {
                string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection Connection = new SqlConnection(strConnection);
                Connection.Open();
                String strSQL = "";
                if (Session["type"].ToString() == "Doctor")
                    strSQL = "Select top 1 * From DeveloperInfo where (LoginName=@UserName)";
                else
                    MessageBox.Show("请先登录！");
                SqlCommand command = new SqlCommand(strSQL, Connection);
                command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString().Trim());
                SqlDataReader DataReader = command.ExecuteReader();
                while (DataReader.Read())
                {
                    LiteralName.Text = Session["UserName"].ToString().Trim();
                }
                DataReader.Close();
                Connection.Close();
            }
        }

        

        protected void BulletedList1_Click(object sender, BulletedListEventArgs e)
        {
               i=e.Index ;   
               BulletedList1.Items[e.Index].Attributes.Add("Style", "color:red");
               FN = BulletedList1.Items[i].Text.Trim();
               BulletedList2.Items.Clear();
               BulletedList3.Items.Clear();
               if (o == 1)//向BulletedList控件只写入一次数据库症候属性
               {
                  o--;
               string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
               SqlConnection conn = new SqlConnection(strConn);
               string Description = "Select Description From FindingsBase where ChineseName='" + BulletedList1.Items[e.Index].Text + "'";
               SqlCommand InsertXml = new SqlCommand(Description, conn);
               conn.Open();
               SqlDataReader dr = InsertXml.ExecuteReader();
               if (dr.Read())
               {
                   Description = dr["Description"].ToString();
               }
               conn.Close();
               XmlDocument reader = new XmlDocument();
               reader.LoadXml(Description);

               XmlElement rootElem = reader.DocumentElement;   //获取根节点 
               XmlNodeList PropertyNodes = rootElem.GetElementsByTagName("Property"); //获取Property子节点集合
               foreach (XmlNode xmlNode in PropertyNodes)
               {
                   XmlNodeList NameNode = ((XmlElement)xmlNode).GetElementsByTagName("Name");  //获取Property子XmlElement集合  
                   string Name = NameNode[0].InnerText;
                   BulletedList2.Items.Add(Name);
               } 
               }
               else
            {
                string XmlInfoOutput = XmlArithmetic(XmlInfoInput);
                XmlDocument reader = new XmlDocument();
                reader.LoadXml(XmlInfoOutput);

                XmlElement root = reader.DocumentElement;   //获取根节点 

                XmlNodeList SymptomNodes = root.GetElementsByTagName("Symptom");
                foreach (XmlNode xmlNode in SymptomNodes)
                {
                    XmlNodeList SymptomNameNodes = ((XmlElement)xmlNode).GetElementsByTagName("SymptomName");

                    XmlNodeList PropertyNodes = root.GetElementsByTagName("Property"); //获取Property子节点集合
                    string name = SymptomNameNodes[0].InnerText;
                    if (string.Compare(name, BulletedList1.Items[e.Index].ToString(), true) == 0)
                    {
                        foreach (XmlNode xmlNode1 in PropertyNodes)
                        {
                            XmlNodeList NameNode = ((XmlElement)xmlNode1).GetElementsByTagName("PropertyName"); 
                            string Name = NameNode[0].InnerText;
                            BulletedList2.Items.Add(Name);
                        }
                    }
                }
            }

           

      
        }
        protected void BulletedList2_Click(object sender, BulletedListEventArgs e)
        {
            BulletedList1.Items[i].Attributes.Add("Style", "color:red");
            BulletedList3.Items.Clear();
            j = e.Index; 
            BulletedList2.Items[j].Attributes.Add("Style", "color:red");
            PN=BulletedList2.Items[j].Text.Trim();
            if (p == 1)//向BulletedList控件只写入一次数据库症候属性
            {
                p--;
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                string Description = "Select Description From FindingsBase where ChineseName='" + BulletedList1.Items[i].Text + "'";
                SqlCommand InsertXml = new SqlCommand(Description, conn);
                conn.Open();
                SqlDataReader dr = InsertXml.ExecuteReader();
                if (dr.Read())
                {
                    Description = dr["Description"].ToString();
                }
                conn.Close();
                BulletedList3.Items.Clear();
                XmlDocument reader = new XmlDocument();
                reader.LoadXml(Description);

                XmlElement rootElem1 = reader.DocumentElement;   //获取根节点 
                XmlNodeList PropertyNodes = rootElem1.GetElementsByTagName("Property");
                foreach (XmlNode node1 in PropertyNodes)
                {
                    XmlNodeList NameNodes = ((XmlElement)node1).GetElementsByTagName("Name");
                    XmlNodeList OptionsNodes = ((XmlElement)node1).GetElementsByTagName("Options");
                    string name = NameNodes[0].InnerText;
                    if (string.Compare(name, BulletedList2.Items[e.Index].ToString(), true) == 0)
                    {
                        foreach (XmlNode xmlNode in OptionsNodes)
                        {
                            XmlNodeList OptionNode = ((XmlElement)xmlNode).GetElementsByTagName("Option");
                            for (int x = 0; x < OptionNode.Count; x++)
                            {
                                string Option = OptionNode[x].InnerText;
                                BulletedList3.Items.Add(Option);
                            }
                        }

                    }
                }
            }
            else 
            {
                string XmlInfoOutput = XmlArithmetic(XmlInfoInput);
                XmlDocument reader = new XmlDocument();
                reader.LoadXml(XmlInfoOutput);

                XmlElement root = reader.DocumentElement;   //获取根节点 

                XmlNodeList SymptomNodes = root.GetElementsByTagName("Symptom");
                foreach (XmlNode xmlNode in SymptomNodes)
                {
                    XmlNodeList SymptomNameNodes = ((XmlElement)xmlNode).GetElementsByTagName("SymptomName");

                    XmlNodeList PropertyNodes = root.GetElementsByTagName("Property"); //获取Property子节点集合
                    string name = SymptomNameNodes[0].InnerText;
                    if (string.Compare(name, BulletedList1.Items[i].ToString(), true) == 0)
                    {
                        foreach (XmlNode xmlNode1 in PropertyNodes)
                        {
                            XmlNodeList NameNode = ((XmlElement)xmlNode1).GetElementsByTagName("OptionName");   
                            string Name = NameNode[0].InnerText;
                            BulletedList3.Items.Add(Name);
                        }
                    }
                }
            }
        }
        protected void BulletedList3_Click(object sender, BulletedListEventArgs e)
        {

            BulletedList1.Items[i].Attributes.Add("Style", "color:red");//保持Blletedlist1不变
            BulletedList2.Items[j].Attributes.Add("Style", "color:red");
            k = e.Index;
            BulletedList3.Items[k].Attributes.Add("Style", "color:red");
            ON = BulletedList3.Items[k].Text.Trim();
            // 注：BulletedList与其它List控件不同，它不支持属性SelectedIndex、SelectedItem、SelectedValue。因为它压根儿无法选择。 
            TextBox1.Text = "你的症状：" + BulletedList1.Items[i].Text.Trim() + "，" + BulletedList2.Items[j].Text.Trim() + "是" + BulletedList3.Items[k].Text.Trim();//根据右侧病史采集的结果,自动生成的患者现病史的文本表述
           
            
            
        }

        protected void ButtonSymptom_Click(object sender, EventArgs e)
        {
            
            Findings = "Symptoms"; Finding = "Symptom"; FindingName = "SymptomName";
            if (BulletedList1.Items.Count == 0) 
            {
                Response.Write("<script>alert('没有选择问题，选项！')</script>");
            }
            else if(BulletedList1.Items.Count !=0 & BulletedList2.Items.Count==0)
            {
                PN = null; ON = null; CreateXmlFile();
            }
            else if (BulletedList1.Items.Count != 0 &BulletedList2.Items.Count != 0 & BulletedList3.Items.Count == 0)
            {
                ON = null; CreateXmlFile();
            }
            else if(BulletedList1.Items.Count != 0 &BulletedList2.Items.Count != 0 & BulletedList3.Items.Count != 0)
            {
                CreateXmlFile();
            }
        }

        protected void BulletedList4_Click(object sender, BulletedListEventArgs e)
        {
            BulletedList5.Items.Clear();
            
            i1 = e.Index;
            BulletedList4.Items[e.Index].Attributes.Add("Style", "color:red");
            FN = BulletedList4.Items[i].Text.Trim();
            string XmlInfoOutput = XmlArithmetic(XmlInfoInput);
            XmlDocument reader = new XmlDocument();
            reader.LoadXml(XmlInfoOutput);

            XmlElement root = reader.DocumentElement;   //获取根节点 

            XmlNodeList SignNodes = root.GetElementsByTagName("Sign");
            foreach (XmlNode xmlNode in SignNodes)
            {
                XmlNodeList SignNameNodes = ((XmlElement)xmlNode).GetElementsByTagName("SignName");

                XmlNodeList PropertyNodes = root.GetElementsByTagName("Property"); //获取Property子节点集合
                string name = SignNameNodes[0].InnerText;
                if (string.Compare(name, BulletedList4.Items[e.Index].ToString(), true) == 0)
                {
                    foreach (XmlNode xmlNode1 in PropertyNodes)
                    {
                        XmlNodeList NameNode = ((XmlElement)xmlNode1).GetElementsByTagName("PropertyName");
                        string Name = NameNode[0].InnerText;
                        BulletedList5.Items.Add(Name);
                    }
                }
            }
        }

        protected void BulletedList5_Click(object sender, BulletedListEventArgs e)
        {
            BulletedList4.Items[i1].Attributes.Add("Style", "color:red");
            BulletedList6.Items.Clear();
            
            j1 = e.Index;
            BulletedList2.Items[j1].Attributes.Add("Style", "color:red");
            PN = BulletedList5.Items[j1].Text.Trim();
            string XmlInfoOutput = XmlArithmetic(XmlInfoInput);
            XmlDocument reader = new XmlDocument();
            reader.LoadXml(XmlInfoOutput);

            XmlElement root = reader.DocumentElement;   //获取根节点 

            XmlNodeList SignNodes = root.GetElementsByTagName("Sign");
            foreach (XmlNode xmlNode in SignNodes)
            {
                XmlNodeList SignNameNodes = ((XmlElement)xmlNode).GetElementsByTagName("SignName");

                XmlNodeList PropertyNodes = root.GetElementsByTagName("Property"); //获取Property子节点集合
                string name = SignNameNodes[0].InnerText;
                if (string.Compare(name, BulletedList4.Items[i1].ToString(), true) == 0)
                {
                    foreach (XmlNode xmlNode1 in PropertyNodes)
                    {
                        XmlNodeList NameNode = ((XmlElement)xmlNode1).GetElementsByTagName("OptionName");
                        string Name = NameNode[0].InnerText;
                        BulletedList6.Items.Add(Name);
                    }
                }
            }
        }

        protected void BulletedList6_Click(object sender, BulletedListEventArgs e)
        {
            BulletedList4.Items[i1].Attributes.Add("Style", "color:red");//保持Blletedlist1不变
            BulletedList5.Items[j1].Attributes.Add("Style", "color:red");
            k1 = e.Index;
            BulletedList6.Items[k1].Attributes.Add("Style", "color:red");
            ON = BulletedList6.Items[k1].Text.Trim();
        }

        protected void ButtonSign_Click(object sender, EventArgs e)
        {
            Findings = "Signs"; Finding = "Sign"; FindingName = "SignName";
            if (BulletedList4.Items.Count == 0)
            {
                Response.Write("<script>alert('没有选择问题，选项！')</script>");
            }
            else if (BulletedList4.Items.Count != 0 & BulletedList5.Items.Count == 0)
            {
                PN = null; ON = null; CreateXmlFile();
            }
            else if (BulletedList4.Items.Count != 0 & BulletedList5.Items.Count != 0 & BulletedList6.Items.Count == 0)
            {
                ON = null; CreateXmlFile();
            }
            else if (BulletedList4.Items.Count != 0 & BulletedList5.Items.Count != 0 & BulletedList6.Items.Count != 0)
            {
                CreateXmlFile();
            }
        }

        protected void BulletedListLaboratory_Click(object sender, BulletedListEventArgs e)
        {
            FN = BulletedListLaboratory.Items[e.Index].Text.Trim();
        }
        protected void ButtonLaboratory_Click(object sender, EventArgs e)
        {
            Findings = "Laboratorys"; Finding = "Laboratory"; FindingName = "LaboratoryName";
            if (BulletedListLaboratory.Items.Count == 0)
            {
                Response.Write("<script>alert('没有选择问题，选项！')</script>");
            }
            if (BulletedListLaboratory.Items.Count != 0)
            {
                PN = null; ON = null; CreateXmlFile();
            }
        }

        protected void BulletedListRadiology_Click(object sender, BulletedListEventArgs e)
        {
            FN = BulletedListRadiology.Items[e.Index].Text.Trim();
        }

        protected void ButtonRadiology_Click(object sender, EventArgs e)
        {
            Findings = "Radiologys"; Finding = "Radiology"; FindingName = "RadiologyName";
            if (BulletedListRadiology.Items.Count == 0)
            {
                Response.Write("<script>alert('没有选择问题，选项！')</script>");
            }
            if (BulletedListRadiology.Items.Count != 0)
            {
                PN = null; ON = null; CreateXmlFile();
            }
        }

        protected void BulletedListOtherExam_Click(object sender, BulletedListEventArgs e)
        {
            FN = BulletedListOtherExam.Items[e.Index].Text.Trim();
        }

        protected void ButtonOtherExam_Click(object sender, EventArgs e)
        {
            Findings = "OtherExams"; Finding = "OtherExam"; FindingName = "OtherExamName";
            if (BulletedListOtherExam.Items.Count == 0)
            {
                Response.Write("<script>alert('没有选择问题，选项！')</script>");
            }
            if (BulletedListOtherExam.Items.Count != 0)
            {
                PN = null; ON = null; CreateXmlFile();
            }
        }

        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            XmlInfoInput = @"<XmlInfoInput><Symptoms></Symptoms><Signs></Signs><Laboratorys></Laboratorys><Radiologys></Radiologys><OtherExams></OtherExams></XmlInfoInput>"; ;
            BulletedList1.Items.Clear();
            BulletedList2.Items.Clear();
            BulletedList3.Items.Clear();
            BulletedList4.Items.Clear();
            BulletedList5.Items.Clear();
            BulletedList6.Items.Clear();
            BulletedListLaboratory.Items.Clear();
            BulletedListRadiology.Items.Clear();
            BulletedListOtherExam.Items.Clear();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlInfoInput);
            doc.Save("c://XmlInfoInput.xml");//便于测试  将XmlInfoInput变量转换成XmlInfoInput.xml存至c盘



        }


    }
}