using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Xml;

namespace CDSS
{
    public partial class CheckAndSubmit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DisplaySelectedKnowledgeData();
        }

        protected void RadioButtonListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySelectedKnowledgeData();
        }

        protected void RadioButtonListKnowledge_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySelectedKnowledgeData();
        }
        protected void DisplaySelectedKnowledgeData()
        {
            String SqlString = null;
            if (this.RadioButtonListKnowledge.SelectedIndex == 0)
                SqlString = "SELECT [ChineseName], [EnglishName], [CreatorName], [CreateTime] FROM [FindingsBase]";
            else SqlString = "SELECT [ChineseName], [EnglishName], [CreatorName], [CreateTime] FROM [DiseasesBase]";
            switch (this.RadioButtonListType.SelectedIndex)
            {
                case 0: SqlString = SqlString + " Where (ModifyNeeded=1) and ((SubmitNeeded=0) or (SubmitNeeded is null))  "+
                    " and ((Submitted=0) or (Submitted is null)) ORDER BY [CreateTime] DESC"; break;
                case 1: SqlString = SqlString + " Where (SubmitNeeded=1) and ((Submitted=0) or (Submitted is null)) ORDER BY [CreateTime] DESC"; break;
                case 2: SqlString = SqlString + " Where (Submitted=1) ORDER BY [CreateTime] DESC"; break;
                //case 3: SqlString = SqlString ; break;
                default: SqlString = SqlString + " Where (ModifyNeeded=1) and ((SubmitNeeded=0) or (SubmitNeeded is null))  " +
                    " and ((Submitted=0) or (Submitted is null)) ORDER BY [CreateTime] DESC"; break;
            }
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            SqlDataAdapter sda = new SqlDataAdapter(SqlString, Connection);
            Connection.Open();
            DataSet ds = new DataSet();
            sda.Fill(ds);
            this.GridViewKnowledge.DataSource = ds;
            this.GridViewKnowledge.AutoGenerateColumns = false;
            this.GridViewKnowledge.DataBind();
            Connection.Close();
            if (this.GridViewKnowledge.Rows.Count>0)
            {this.GridViewKnowledge.SelectRow(0);}            
        }

        protected void GridViewKnowledge_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewKnowledge.PageIndex = e.NewPageIndex;
            //GridViewKnowledge.DataSource = getData();//调用绑定的具体方法
            GridViewKnowledge.DataBind();
        }

        protected void GridViewKnowledge_SelectedIndexChanged(object sender, EventArgs e)
        {
            String KnowledgeBaseName = null;
            if (this.RadioButtonListKnowledge.SelectedIndex == 0)
            { KnowledgeBaseName = "FindingsBase"; }
            else { KnowledgeBaseName = "DiseasesBase"; }
            GridViewRow CurRow = this.GridViewKnowledge.SelectedRow;
            string KnowledgeTitle = CurRow.Cells[1].Text;
            DisplayKnowledgeContent(KnowledgeBaseName, KnowledgeTitle);
        }

        private void DisplayKnowledgeContent(string KnowledgeBaseName, string KnowledgeTitle)
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                string SqlStr = "Select Description From " + KnowledgeBaseName + " where ChineseName=@KnowledgeTitle";
                SqlCommand cmd = new SqlCommand(SqlStr, conn);
                cmd.Parameters.AddWithValue("@KnowledgeTitle", KnowledgeTitle);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                string CurrentKnowledge = "";
                if (dr.Read())
                {
                    CurrentKnowledge = dr["Description"].ToString();
                }

                conn.Close();

                string KnowledgeType = "";
                if (KnowledgeBaseName == "FindingsBase")
                { KnowledgeType = "症候"; }
                else { KnowledgeType = "疾病"; }

                if (CurrentKnowledge == "")
                {
                    string ErrorMessage = "";
                    ErrorMessage = KnowledgeType+"【" + KnowledgeTitle + "】知识XML尚未创建！"; 
                    Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(CurrentKnowledge);
                    XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                    this.TreeViewKnowledge.Nodes.Clear();
                    XmlToTreeview(xmlNodes, this.TreeViewKnowledge.Nodes);
                    this.TreeViewKnowledge.ExpandAll();
                }
                this.LiteralKnowledgeInfo.Text = KnowledgeType + "【" + KnowledgeTitle + "】";
            }
            catch (Exception)
            {
                string ErrorMessage = "打开症候知识XML时出错，请联系系统管理员！";
                Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
            }
        }
        //将Xml类型转化成treeview类型
        private void XmlToTreeview(XmlNodeList xmlNodeList, TreeNodeCollection treeNode)
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = xmlNode.LocalName;

                if (xmlNode.HasChildNodes)
                {
                    if (xmlNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                    {
                        newTreeNode.Text += " 【 " + xmlNode.ChildNodes[0].Value + "】";
                        newTreeNode.Value = xmlNode.ChildNodes[0].Value;
                    }
                    XmlToTreeview(xmlNode.ChildNodes, newTreeNode.ChildNodes);
                }
                treeNode.Add(newTreeNode);
            }
        }
        protected void TreeViewFinding_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        protected void ButtonNeedRemodify_Click(object sender, EventArgs e)
        {
            MarkCurKnowledgeRemodify();            
        }

        private void MarkCurKnowledgeRemodify()
        {
            GridViewRow CurRow = this.GridViewKnowledge.SelectedRow;
            string ChineseName = CurRow.Cells[1].Text;
            string CreatorName = CurRow.Cells[2].Text;
            //string CreateTime = CurRow.Cells[3].Text;

            String SqlString = null;
            if (this.RadioButtonListKnowledge.SelectedIndex == 0)
            {
                SqlString = "Update [FindingsBase] set [ModifyNeeded]=0,[SubmitNeeded]=0,[Submitted]=0 ";
            }
            else
            {
                SqlString = "Update [DiseasesBase] set [ModifyNeeded]=0,[SubmitNeeded]=0,[Submitted]=0 ";
            }
            SqlString = SqlString + " Where (ChineseName=@ChineseName) and (CreatorName=@CreatorName)";

            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection CurConn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SqlString, CurConn);
            cmd.Parameters.AddWithValue("@ChineseName", ChineseName);
            cmd.Parameters.AddWithValue("@CreatorName", CreatorName);
            //cmd.Parameters.AddWithValue("@CreateTime",CreateTime);
            CurConn.Open();
            cmd.ExecuteNonQuery();
            DisplaySelectedKnowledgeData();
            CurConn.Close();
        }

        private void TreeviewToXml(System.Web.UI.WebControls.TreeNodeCollection treeNodes, XmlNode xmlNode)
        {

            XmlDocument doc = xmlNode.OwnerDocument;
            foreach (System.Web.UI.WebControls.TreeNode treeNode in treeNodes)
            {
                string[] Array = treeNode.Text.Split(new char[2] { '【', '】' });

                XmlNode element = doc.CreateNode("element", Array[0].Trim(), "");
                try
                {
                    element.InnerText = Array[1];
                }
                catch (Exception)
                {
                    element.InnerText = "";
                }
                xmlNode.AppendChild(element);

                if (treeNode.ChildNodes.Count > 0)
                {
                    TreeviewToXml(treeNode.ChildNodes, element);
                }

            }
        }
        private void SaveAndUpdateSubmitInfo(string ChineseName, string CreatorName, string Description)
        {
            string SubmitterName = Session["UserName"].ToString();
            string SubmitTime = DateTime.Now.ToString();

            String SqlString = null;
            if (this.RadioButtonListKnowledge.SelectedIndex == 0)
            {
                SqlString = "Update [FindingsBase] set [Submitted]=1,Description=@Description,SubmitTime=@SubmitTime,SubmitterName=@SubmitterName";
            }
            else
            {
                SqlString = "Update [DiseasesBase] set [Submitted]=1,Description=@Description,SubmitTime=@SubmitTime,SubmitterName=@SubmitterName ";
            }
            SqlString = SqlString + " Where (ChineseName=@ChineseName) and (CreatorName=@CreatorName)";

            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection CurConn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SqlString, CurConn);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@ChineseName", ChineseName);
            cmd.Parameters.AddWithValue("@CreatorName", CreatorName);
            cmd.Parameters.AddWithValue("@SubmitTime", SubmitTime);
            cmd.Parameters.AddWithValue("@SubmitterName", SubmitterName);
            CurConn.Open();
            cmd.ExecuteNonQuery();
            DisplaySelectedKnowledgeData();
            CurConn.Close();
        }

        private void DeleteDiseaseDignosisData(string DiseaseName)
        {
            String SqlString = "DELETE FROM [DiagnosisData] Where (DiseaseName=@DiseaseName)";
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection CurConn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SqlString, CurConn);
            cmd.Parameters.AddWithValue("@DiseaseName", DiseaseName);
            CurConn.Open();
            cmd.ExecuteNonQuery();
            DisplaySelectedKnowledgeData();
            CurConn.Close();
        }
        private void SaveDiseaseKnowledgeToTable(XmlDocument DocCurKnowledge)
        {            
            //[DiseaseName],[FindingType],[FindingName],[PropertyName],[OptionName],[Frequency],[Specifity],[Importance]

            XmlNode root = DocCurKnowledge.DocumentElement;
            XmlNode TempNode = root.SelectSingleNode("//DiseaseKnowledge/ChineseName");
            string DiseaseName = TempNode.InnerText.Trim();

            DeleteDiseaseDignosisData(DiseaseName);

            int TypeCount = 6;
            string[] FindingTypeList = new string[TypeCount];
            FindingTypeList[0] = "Predisposition";
            FindingTypeList[1] = "Symptom";
            FindingTypeList[2] = "Sign";
            FindingTypeList[3] = "Laboratory";
            FindingTypeList[4] = "Radiology";
            FindingTypeList[5] = "OtherExam";

            for (int i = 0; i < TypeCount; i++)
            {
                string FindingType=FindingTypeList[i];
                string CurXmlPath = "//DiseaseKnowledge/" + FindingType + "s/" + FindingType;
                XmlNodeList FindingNodeList = root.SelectNodes(CurXmlPath);
                                
                if (FindingNodeList.Count > 0)
                {
                    foreach (XmlNode FindingNode in FindingNodeList)
                    {
                        SaveDataFromFindingNode(DiseaseName, FindingType, FindingNode);                        
                    }
                }
            }            
        }

        private void SaveDataFromFindingNode(string DiseaseName, string FindingType, XmlNode FindingNode)
        {
            string FindingName = "";
            string Frequency = "";
            string Specifity = "";
            string Importance = "";
            string FindingTypeName = FindingType + "Name";
            for (int j = 0; j < (FindingNode.ChildNodes.Count); j++)
            {
                if (FindingNode.ChildNodes[j].LocalName != "Properties")
                {
                    if (FindingNode.ChildNodes[j].LocalName == FindingTypeName)
                    {
                        FindingName = FindingNode.ChildNodes[j].InnerText.Trim();
                    }
                    else
                    {
                        switch (FindingNode.ChildNodes[j].LocalName)
                        {
                            case "Frequency":
                                Frequency = FindingNode.ChildNodes[j].InnerText.Trim();
                                break;
                            case "Specifity":
                                Specifity = FindingNode.ChildNodes[j].InnerText.Trim();
                                break;
                            case "Importance":
                                Importance = FindingNode.ChildNodes[j].InnerText.Trim();
                                break;
                        }
                    }
                }
                else
                {
                    XmlNodeList PropertyNodeList = FindingNode.ChildNodes[j].ChildNodes;
                    for (int k = 0; k < (PropertyNodeList.Count); k++)
                    {
                        XmlNode FindingProperty = PropertyNodeList[k];
                        SaveDataFromFindingProperty(DiseaseName, FindingType, FindingName, FindingProperty);                     
                    }
                    //tobecontinue
                }
            }
            SaveFindingItselfKnowledge(DiseaseName, FindingType, FindingName, Frequency, Specifity, Importance);
        }

        private void SaveDataFromFindingProperty(string DiseaseName, string FindingType, string FindingName, XmlNode FindingProperty)
        {
            string PropertyName = "";
            string OptionName = "";
            string Frequency = "";
            string Specifity = "";
            string Importance = "";
            for (int j = 0; j < (FindingProperty.ChildNodes.Count); j++)
            {
                switch (FindingProperty.ChildNodes[j].LocalName)
                {
                    case "PropertyName":
                        PropertyName = FindingProperty.ChildNodes[j].InnerText.Trim();
                        break;
                    case "OptionName":
                        OptionName = FindingProperty.ChildNodes[j].InnerText.Trim();
                        break;
                    case "Frequency":
                        Frequency = FindingProperty.ChildNodes[j].InnerText.Trim();
                        break;
                    case "Specifity":
                        Specifity = FindingProperty.ChildNodes[j].InnerText.Trim();
                        break;
                    case "Importance":
                        Importance = FindingProperty.ChildNodes[j].InnerText.Trim();
                        break;
                }
            }
            SaveFindingPropertyKnowledge(DiseaseName, FindingType, FindingName, PropertyName,OptionName, Frequency, Specifity, Importance);
        }

        private void SaveFindingPropertyKnowledge(string DiseaseName, string FindingType, string FindingName, string PropertyName, string OptionName, string Frequency, string Specifity, string Importance)
        {
            String SqlString = SqlString = "INSERT INTO [DiagnosisData] (DiseaseName,FindingType,FindingName,PropertyName,OptionName,Frequency,Specifity,Importance) " +
                  " VALUES(@DiseaseName,@FindingType,@FindingName,@PropertyName,@OptionName,@Frequency,@Specifity,@Importance)";
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection CurConn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SqlString, CurConn);
            cmd.Parameters.AddWithValue("@DiseaseName", DiseaseName);
            cmd.Parameters.AddWithValue("@FindingType", FindingType);
            cmd.Parameters.AddWithValue("@FindingName", FindingName);
            cmd.Parameters.AddWithValue("@PropertyName", PropertyName);
            cmd.Parameters.AddWithValue("@OptionName", OptionName);
            cmd.Parameters.AddWithValue("@Frequency", Frequency);
            cmd.Parameters.AddWithValue("@Specifity", Specifity);
            cmd.Parameters.AddWithValue("@Importance", Importance);

            CurConn.Open();
            cmd.ExecuteNonQuery();
            CurConn.Close(); 
        }

        private void SaveFindingItselfKnowledge(string DiseaseName, string FindingType, string FindingName, string Frequency, string Specifity, string Importance)
        {
            String SqlString = SqlString = "INSERT INTO [DiagnosisData] (DiseaseName,FindingType,FindingName,Frequency,Specifity,Importance) "+
                  " VALUES(@DiseaseName,@FindingType,@FindingName,@Frequency,@Specifity,@Importance)";            
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection CurConn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SqlString, CurConn);
            cmd.Parameters.AddWithValue("@DiseaseName", DiseaseName);
            cmd.Parameters.AddWithValue("@FindingType", FindingType);
            cmd.Parameters.AddWithValue("@FindingName", FindingName);
            cmd.Parameters.AddWithValue("@Frequency", Frequency);
            cmd.Parameters.AddWithValue("@Specifity", Specifity);
            cmd.Parameters.AddWithValue("@Importance", Importance);

            CurConn.Open();
            cmd.ExecuteNonQuery();
            CurConn.Close();            
        }
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            GridViewRow CurRow = this.GridViewKnowledge.SelectedRow;
            string ChineseName = CurRow.Cells[1].Text;
            string CreatorName = CurRow.Cells[2].Text;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<main></main>");
            XmlNode NodeKnowledge = xmlDoc.DocumentElement;
            TreeviewToXml(this.TreeViewKnowledge.Nodes, NodeKnowledge);
            string Description = NodeKnowledge.InnerXml;

            SaveAndUpdateSubmitInfo(ChineseName, CreatorName, Description);

            if (RadioButtonListKnowledge.SelectedIndex == 1)
            {
                XmlDocument DocCurKnowledge = new XmlDocument();
                DocCurKnowledge.LoadXml(Description);
                //XmlNodeList CurNodeList = DocCurKnowledge.ChildNodes;
                SaveDiseaseKnowledgeToTable(DocCurKnowledge);            
            }
        }
    }

}