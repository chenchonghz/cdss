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
using System.IO;

namespace CDSS
{
    public partial class DiseaseKnowledgeEditor : System.Web.UI.Page
    {
        //全局sql连接对象定义
        SqlConnection connection = new SqlConnection();
        //全局数据集定义
        DataSet ds = new DataSet();
        int flag = 0;
        TreeNode tree_Node = null;
        int UniqueNodeValue = 0;

        //数据库连接函数
        private void SqlReader()
        {
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            connection.ConnectionString = strConnection;
        }

        //初始化DiseasesBase和FindingsBase数据集
        private void SqlToDataset()
        {
            //产生DiseasesBase的SystemType数据集
            SqlReader();
            String DSystemType = "Select distinct SystemType From DiseasesBase order by SystemType Desc";
            SqlDataAdapter da_DST = new SqlDataAdapter(DSystemType, connection);
            da_DST.Fill(ds, "DSystemType");

            //产生FindingsBase的FindingType数据集
            String FFindingType = "Select distinct FindingType From FindingsBase order by FindingType Desc";
            SqlDataAdapter da_FFT = new SqlDataAdapter(FFindingType, connection);
            da_FFT.Fill(ds, "FFindingType");

        }

        //创建DChineseName数据集
        private void DCN_SqlToDataset(string systemType)
        {
            SqlReader();
            String DChineseName = "Select ChineseName From DiseasesBase Where SystemType='" + systemType +
                "' Order by ChineseName";
            SqlDataAdapter da_DCN = new SqlDataAdapter(DChineseName, connection);
            da_DCN.Fill(ds, "DChineseName");
        }

        //创建FChineseName数据集
        private void FCN_SqlToDataset(string findingType)
        {
            SqlReader();
            String FChineseName = "Select ChineseName From FindingsBase Where FindingType='" + findingType +
                "' Order by ChineseName";
            SqlDataAdapter da_FCN = new SqlDataAdapter(FChineseName, connection);
            da_FCN.Fill(ds, "FChineseName");
        }

        //读取数据库Xml文件并转换成String字符串
        private string LoadXmlToString(string TabelName, String FieldName, string ChineseName)
        {
            SqlReader();
            String XmlRead = "Select " + FieldName + " From " + TabelName + " Where ChineseName='" + ChineseName + "'";
            SqlCommand cmd1 = new SqlCommand(XmlRead, connection);
            connection.Open();
            SqlDataReader dr = cmd1.ExecuteReader();
            string Description = null;

            if (dr.Read())
            {
                Description = dr[FieldName].ToString();
            }
            connection.Close();
            return Description;

        }

        //把Xml对应的字符串存储到数据库中
        private bool SaveDiagnosisKnowledge(string Description, string ChineseName, String SavedType)
        {
            SqlReader();
            DateTime CurTime = DateTime.Now;//获取时间
            string CreateTime = CurTime.ToString();
            string CreatorName = Session["UserName"].ToString();//填写上传者名字     
            string SqlString = "Update DiseasesBase Set Description=@Description,CreateTime=@CreateTime," +
                "CreatorName=@CreatorName,";

            //SavedType="NewCreate",or "TempSave", or "WaitModify"
            if (SavedType == "TempSave")
            {
                SqlString = SqlString + "ModifyNeeded=0 where ChineseName=@ChineseName";
            }
            else if (SavedType == "NewCreate")
            {
                SqlString = SqlString + "ModifyNeeded=0 where ChineseName=@ChineseName";
            }
            else if (SavedType == "WaitModify")
            {
                SqlString = SqlString + "ModifyNeeded=1 where ChineseName=@ChineseName";
            }
            SqlCommand cmd1 = new SqlCommand(SqlString, connection);
            cmd1.Parameters.AddWithValue("@Description", Description);
            cmd1.Parameters.AddWithValue("@CreateTime", CreateTime);
            cmd1.Parameters.AddWithValue("@CreatorName", CreatorName);
            cmd1.Parameters.AddWithValue("@ChineseName", ChineseName);
            connection.Open();
            int AffectedRow=cmd1.ExecuteNonQuery();
            connection.Close();
            if (AffectedRow > 0)
            { return true;}
            else
            {return false;}
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
                newTreeNode.Value = UniqueNodeValue.ToString(); UniqueNodeValue++;

                if (xmlNode.HasChildNodes)
                {
                    if (xmlNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                    {
                        newTreeNode.Text += " 【 " + xmlNode.ChildNodes[0].Value + "】";
                        newTreeNode.Value = UniqueNodeValue.ToString(); UniqueNodeValue++;
                        //newTreeNode.Value = xmlNode.ChildNodes[0].Value;
                    }
                    XmlToTreeview(xmlNode.ChildNodes, newTreeNode.ChildNodes);
                }
                treeNode.Add(newTreeNode);
            }
        }

        //将treeview类型转化成Xml类型
        private void TreeviewToXml(TreeNodeCollection treeNodes, XmlNode xmlNode)
        {
            XmlDocument doc = xmlNode.OwnerDocument;
            foreach (TreeNode treeNode in treeNodes)
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

        //搜索对应的treeview节点并修改属性
        public void treeviewSearch(TreeNodeCollection nodeList, String label, String text)
        {
            foreach (TreeNode treeNode in nodeList)
            {
                string[] Array = treeNode.Text.Split(new char[2] { '【', '】' });
                if (Array[0].Trim() == label.Trim())
                {
                    treeNode.Text = Array[0].Trim() + " 【 " + text + "】";
                    break;
                }

                if (treeNode.ChildNodes.Count > 0)
                {
                    treeviewSearch(treeNode.ChildNodes, label, text);
                }
            }
        }

        //搜索treeview节点并添加节点
        private void AddTreeViewNode(TreeNodeCollection treeNodeCollection, TreeNode treeNode, string type, string name = "")
        {
            foreach (TreeNode tNode in treeNodeCollection)
            {
                string[] Array = tNode.Text.Split(new char[2] { '【', '】' });
                try
                {
                    if (Array[0].Trim() == type.Trim() && Array[1].Trim() == name.Trim())
                    {
                        treeNode.ChildNodes[0].Value = UniqueNodeValue.ToString(); UniqueNodeValue++;
                        tNode.Parent.ChildNodes[4].ChildNodes.Add(treeNode.ChildNodes[0]);
                        break;
                    }
                }
                catch (Exception)
                {
                    if (Array[0].Trim() == type)
                    {
                        treeNode.ChildNodes[0].Value = UniqueNodeValue.ToString(); UniqueNodeValue++;
                        tNode.ChildNodes.Add(treeNode.ChildNodes[0]);
                        break;
                    }
                }

                if (tNode.ChildNodes.Count > 0)
                {
                    AddTreeViewNode(tNode.ChildNodes, treeNode, type, name);
                }
            }
        }


        //搜索treeview节点，判断是否搜索到节点并返回对应节点的位置
        public void SearchForNode(TreeNodeCollection nodeList, String PropertyName, String PropertyValue)
        {
            foreach (TreeNode treeNode in nodeList)
            {
                String[] array = treeNode.Text.Split(new char[2] { '【', '】' });
                try
                {
                    if (array[0].Trim() == PropertyName && array[1].Trim() == PropertyValue.Trim())
                    {
                        flag = 1;
                        tree_Node = treeNode;
                        break;
                    }
                }
                catch (Exception) { }

                if (treeNode.ChildNodes.Count > 0)
                {
                    SearchForNode(treeNode.ChildNodes, PropertyName, PropertyValue);
                }
            }
        }

        //把症候属性添加到对应的节点上
        private string selected_name(string text)
        {
            switch (text)
            {
                case "易感因素": return "Predispositions";
                case "症状": return "Symptoms";
                case "体征": return "Signs";
                case "实验室检查": return "Laboratorys";
                case "影像学检查": return "Radiologys";
                default: return "OtherExams";
            }
        }

        //页面载入函数
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //疾病系统初始化
                SqlToDataset();   //产生FFindingType和DSystemType数据集

                //用SystemType数据集对DropDownListSystemType控件进行绑定
                DropDownListSystemType.DataSource = ds.Tables["DSystemType"];
                DropDownListSystemType.DataTextField = "SystemType";
                DropDownListSystemType.DataBind();

                //用FindingType数据集对DropDownListFindingType控件进行绑定
                DropDownListFindingType.DataSource = ds.Tables["FFindingType"];
                DropDownListFindingType.DataTextField = "FindingType";
                DropDownListFindingType.DataBind();

                DropDownListSystemType_SelectedIndexChanged(sender, e);
                DropDownListFindingType_SelectedIndexChanged(sender, e);
            }

        }

        //疾病系统改变函数
        protected void DropDownListSystemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DCN_SqlToDataset(this.DropDownListSystemType.Text); //产生DChineseName数据集

            //用DChineseName数据集对DropDownListDiseaseName控件进行绑定
            DropDownListDiseaseName.DataSource = ds.Tables["DChineseName"];
            DropDownListDiseaseName.DataTextField = "ChineseName";
            DropDownListDiseaseName.DataBind();
            DropDownListDiseaseName_SelectedIndexChanged(sender, e);
        }

        //疾病名称改变函数
        protected void DropDownListDiseaseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Description = LoadXmlToString("DiseasesBase", "Description", this.DropDownListDiseaseName.Text);
            if (Description == null || Description == "")
            {
                //自动生成DiseaseKnowledge的xml结构（设置模板有利于后期修改）
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(@"<DiseaseKnowledge>
                            <ChineseName></ChineseName>
                            <EnglishName></EnglishName>
                            <Introduction></Introduction>
                            <Symptoms></Symptoms>
                            <Signs></Signs>
                            <Laboratorys></Laboratorys>
                            <Radiologys></Radiologys>
                            <OtherExams></OtherExams>
                            </DiseaseKnowledge>");
                XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                this.TreeViewDisease.Nodes.Clear();
                XmlToTreeview(xmlNodes, this.TreeViewDisease.Nodes);
                this.TreeViewDisease.ExpandAll();
                SqlReader();
                String sql = "select * from DiseasesBase where ChineseName='" + this.DropDownListDiseaseName.Text + "'";
                SqlCommand cmd1 = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader rd = cmd1.ExecuteReader();
                if (rd.Read())
                {
                    treeviewSearch(this.TreeViewDisease.Nodes, "ChineseName", rd["ChineseName"].ToString());
                    treeviewSearch(this.TreeViewDisease.Nodes, "EnglishName", rd["EnglishName"].ToString());

                }
                connection.Close();
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Description);
                XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                this.TreeViewDisease.Nodes.Clear();
                XmlToTreeview(xmlNodes, this.TreeViewDisease.Nodes);
                this.TreeViewDisease.ExpandAll();
            }
        }

        //症候类型改变函数
        protected void DropDownListFindingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FCN_SqlToDataset(this.DropDownListFindingType.Text);//产生FChineseName数据集

            //用FChineseName数据集对ListBoxFindings控件进行绑定
            ListBoxFindings.DataSource = ds.Tables["FChineseName"];
            ListBoxFindings.DataTextField = "ChineseName";
            ListBoxFindings.DataBind();
            this.TreeViewFinding.Nodes.Clear();
        }

        //症候名称列表函数
        protected void ListBoxFindings_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string Description = LoadXmlToString("FindingsBase", "Description", this.ListBoxFindings.SelectedItem.Text);
            if (Description == "")
            {
                string ErrorMessage = "症候【" + this.ListBoxFindings.SelectedItem.Text + "】知识XML尚未创建！";
                Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
            }
            else
            {
                xmlDoc.LoadXml(Description);
                XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                this.TreeViewFinding.Nodes.Clear();
                XmlToTreeview(xmlNodes, this.TreeViewFinding.Nodes);
                this.TreeViewFinding.ExpandAll();
            }
        }

        //保存疾病修改，并上传疾病的xml结构
        protected void ButtonTempSave_Click(object sender, EventArgs e)
        {
            if (this.DropDownListDiseaseName.Text != "")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<main></main>");
                XmlNode root = doc.DocumentElement;
                TreeviewToXml(this.TreeViewDisease.Nodes, root);
                string Description = root.InnerXml;
                string DiseaseName = this.DropDownListDiseaseName.Text;
                //SavedType="NewCreate",or "TempSave", or "WaitModify"
                if (SaveDiagnosisKnowledge(Description, DiseaseName, "TempSave"))
                {Response.Write("<script>alert('保存疾病成功！')</script>");}
                else {Response.Write("<script>alert('保存疾病失败！请检查疾病名称以及文件格式是否正确。')</script>");}                               
            }
            else
            {
                Response.Write("<script>alert('请选择所要保存的疾病！')</script>");
            }
        }

        private void CreateFirstLevelProperty(string NewPropery)
        {
            flag = 0;
            SearchForNode(this.TreeViewDisease.CheckedNodes, "Predisposition", "");
            if (flag != 1)
            {
                string TimeStr = DateTime.Now.ToShortTimeString();
                System.Web.UI.WebControls.TreeNode NewNode = new System.Web.UI.WebControls.TreeNode("Predispositions", TimeStr);
                this.TreeViewDisease.Nodes[0].ChildNodes.Add(NewNode);
            }
        }

        //添加症候名字控件
        protected void ButtonAddFinding_Click(object sender, EventArgs e)
        {
            string xmlname="";
            switch (this.DropDownListFindingType.Text.Trim())
            {
                case "易感因素": xmlname = "PredispositionName"; break;
                case "症状": xmlname = "SymptomName"; break;
                case "体征": xmlname = "SignName"; break;
                case "实验室检查": xmlname = "LaboratoryName"; break;
                case "影像学检查": xmlname = "RadiologyName"; break;
                case "其他检查": xmlname = "OtherExamName"; break;
            }

            if (xmlname == "PredispositionName")
            {
                CreateFirstLevelProperty("Predisposition");
            }

            flag = 0;
            try
            {
                SearchForNode(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
            }
            catch (Exception) { }

            if (flag == 1)
            {
                String str_name1 = null;
                switch (this.RadioButtonListPropertyType1.Text.Trim())
                {
                    case "常见度": str_name1 = "Frequency"; break;
                    case "特异度": str_name1 = "Specifity"; break;
                    case "重要度": str_name1 = "Importance"; break;
                }

                //设置treeNode的属性参数
                foreach (TreeNode TNode in tree_Node.Parent.ChildNodes)
                {
                    string[] Array = TNode.Text.Split(new char[2] { '【', '】' });

                    if (Array[0].Trim() == str_name1)
                    {
                        TNode.Text = Array[0].Trim() + " 【 " + this.RadioButtonListPropertyScale1.SelectedItem.Text + "】";
                    }
                }
            }
            else
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    string xml_string="";
                    switch (this.DropDownListFindingType.Text.Trim())
                    {
                        case "易感因素": xml_string = @"<Predisposition><PredispositionName/><Frequency />
                            <Specifity /><Importance/><Properties/></Predisposition>"; break;
                        case "症状": xml_string = @"<Symptom><SymptomName/><Frequency />
                            <Specifity /><Importance/><Properties/></Symptom>"; break;
                        case "体征": xml_string = @"<Sign><SignName/><Frequency />
                            <Specifity /><Importance/><Properties/></Sign>"; break;
                        case "实验室检查": xml_string = @"<Laboratory><LaboratoryName/><Frequency />
                            <Specifity /><Importance/><Properties/></Laboratory>"; break;
                        case "影像学检查": xml_string = @"<Radiology><RadiologyName/><Frequency />
                            <Specifity /><Importance/><Properties/></Radiology>"; break;
                        case "其他检查": xml_string = @"<OtherExam><OtherExamName/><Frequency />
                            <Specifity /><Importance/><Properties/></OtherExam>"; break;
                    }
                    xmlDoc.LoadXml(xml_string);

                    XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                    TreeNode treeNode = new TreeNode();
                    XmlToTreeview(xmlNodes, treeNode.ChildNodes);

                    String str_name1 = null;
                    switch (this.RadioButtonListPropertyType1.Text.Trim())
                    {
                        case "常见度": str_name1 = "Frequency"; break;
                        case "特异度": str_name1 = "Specifity"; break;
                        case "重要度": str_name1 = "Importance"; break;
                    }

                    //设置treeNode的属性参数
                    foreach (TreeNode TNode in treeNode.ChildNodes[0].ChildNodes)
                    {
                        string[] Array = TNode.Text.Split(new char[2] { '【', '】' });
                        if (Array[0].Trim() == xmlname)
                        {
                            TNode.Text = Array[0].Trim() + " 【 " + this.ListBoxFindings.SelectedItem.Text + "】";
                        }
                        else if (Array[0].Trim() == str_name1)
                        {
                            TNode.Text = Array[0].Trim() + " 【 " + this.RadioButtonListPropertyScale1.SelectedItem.Text + "】";
                        }
                    }

                    //添加treeview节点
                    AddTreeViewNode(this.TreeViewDisease.Nodes, treeNode, selected_name(this.DropDownListFindingType.Text.Trim()));
                    //this.Label2.Text = "添加症候名成功！";
                }
                catch (Exception)
                {
                    // this.Label2.Text = "请选择所需的症候名";
                }
            }
        }


        //添加症候属性控件处理函数
        protected void ButtonAddProperty_Click(object sender, EventArgs e)
        {
            int SelectedNodeLevel = 0;
            if (this.TreeViewFinding.SelectedNode != null)
            {
                SelectedNodeLevel = this.TreeViewFinding.SelectedNode.Depth;
            }
            else
            {
                SelectedNodeLevel = 0;
            }

            if (SelectedNodeLevel != 4)
            {
                string MessageStr = "添加特征前必须选定某个特征属性选项（Option）!";
                Response.Write("<script>window.alert('" + MessageStr + "');</script>");
            }
            else
            {
                string xmlname;
                switch (this.DropDownListFindingType.Text.Trim())
                {
                    case "易感因素": xmlname = "PredispositionName"; break;
                    case "症状": xmlname = "SymptomName"; break;
                    case "体征": xmlname = "SignName"; break;
                    case "实验室检查": xmlname = "LaboratoryName"; break;
                    case "影像学检查": xmlname = "RadiologyName"; break;
                    default: xmlname = "OtherExamName"; break;
                }
                flag = 0;
                try
                {
                    SearchForNode(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
                    flag = 0;
                    string[] PropertyName = this.TreeViewFinding.SelectedNode.Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                    SearchForNode(tree_Node.Parent.ChildNodes, "PropertyName", PropertyName[1].Trim());
                }
                catch (Exception) { }

                if (flag == 1)
                {
                    flag = 0;
                    SearchForNode(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
                    TreeNode treeNode_1 = tree_Node;
                    try
                    {
                        string[] PropertyName = this.TreeViewFinding.SelectedNode.Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                        SearchForNode(treeNode_1.Parent.ChildNodes, "PropertyName", PropertyName[1].Trim());
                    }
                    catch (Exception) { }
                    if (flag == 1)
                    {
                        String str_name1 = null;
                        switch (this.RadioButtonListPropertyType2.Text.Trim())
                        {
                            case "常见度": str_name1 = "Frequency"; break;
                            case "特异度": str_name1 = "Specifity"; break;
                            case "重要度": str_name1 = "Importance"; break;
                        }
                        //设置treeNode的属性参数
                        foreach (TreeNode TNode in tree_Node.Parent.ChildNodes)
                        {
                            string[] Array = TNode.Text.Split(new char[2] { '【', '】' });

                            if (Array[0].Trim() == str_name1)
                            {
                                TNode.Text = Array[0].Trim() + " 【 " + this.RadioButtonListPropertyScale2.SelectedItem.Text + "】";
                            }
                        }
                    }
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(@"<Property><PropertyName/><OptionName /><Frequency/>
                                <Specifity /><Importance /></Property>");
                    XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                    TreeNode treeNode = new TreeNode();
                    XmlToTreeview(xmlNodes, treeNode.ChildNodes);
                    String str_name1 = null;
                    switch (this.RadioButtonListPropertyType1.Text.Trim())
                    {
                        case "常见度": str_name1 = "Frequency"; break;
                        case "特异度": str_name1 = "Specifity"; break;
                        case "重要度": str_name1 = "Importance"; break;
                    }
                    //设置treeNode的属性参数
                    foreach (TreeNode TNode in treeNode.ChildNodes[0].ChildNodes)
                    {
                        string[] Array = TNode.Text.Split(new char[2] { '【', '】' });
                        if (Array[0].Trim() == "PropertyName")
                        {
                            string[] PropertyName = this.TreeViewFinding.SelectedNode.Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                            TNode.Text = Array[0].Trim() + " 【 " + PropertyName[1].Trim() + "】";
                        }
                        else if (Array[0].Trim() == "OptionName")
                        {
                            string[] PropertyOption = this.TreeViewFinding.SelectedNode.Text.Split(new char[2] { '【', '】' });
                            TNode.Text = Array[0].Trim() + " 【 " + PropertyOption[1].Trim() + "】";
                        }
                        else if (Array[0].Trim() == str_name1)
                        {
                            TNode.Text = Array[0].Trim() + " 【 " + this.RadioButtonListPropertyScale2.SelectedItem.Text + "】";
                        }
                    }
                    //添加treeview节点
                    AddTreeViewNode(this.TreeViewDisease.Nodes, treeNode, xmlname, this.ListBoxFindings.SelectedItem.Text);
                    try
                    {
                        flag = 0;
                        SearchForNode(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
                        flag = 0;
                        string[] Property_Name = this.TreeViewFinding.SelectedNode.Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                        SearchForNode(tree_Node.Parent.ChildNodes, "PropertyName", Property_Name[1].Trim());
                        if (flag == 1)
                        {
                            //this.Label2.Text = "添加症候属性成功！";
                        }
                    }
                    catch (Exception)
                    {
                        //this.Label2.Text = "未添加症候名！";
                    }
                }
            }
        }

        protected void ButtonSaveUploaded_Click(object sender, EventArgs e)
        {
            if (this.FileUpload1.FileName=="")
            {
                Response.Write("<script>window.alert('请选择上传文件!');</script>");                 
            }
            else 
            {
                try
                {
                    System.IO.Stream myStream = this.FileUpload1.FileContent;//读出fileuploadfindingfile中xml文件内容
                    System.IO.StreamReader myread = new System.IO.StreamReader(myStream);
                    string result = myread.ReadToEnd();//讲文件内容转换成string
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);

                    if (doc.GetElementsByTagName("ChineseName").Count>0)
                    {
                        string Description=doc.DocumentElement.OuterXml;
                        string DiseaseName = doc.GetElementsByTagName("ChineseName")[0].InnerText;
                        //SavedType="NewCreate",or "TempSave", or "WaitModify"
                        if (SaveDiagnosisKnowledge(Description, DiseaseName, "NewCreate"))
                        {Response.Write("<script>window.alert('上传xml文件成功！');</script>");}
                        else 
                        { 
                            string MsgString ="上传xml文件失败！请检查疾病名称【"+DiseaseName+"】及文档格式是否正确。";
                            Response.Write("<script>window.alert('" + MsgString + "');</script>"); 
                        }
                    }
                    else {Response.Write("<script>window.alert('文档格式存在问题，请检查确认。');</script>");}
                    
                    DropDownListDiseaseName_SelectedIndexChanged(sender, e);
                }
                catch (Exception)
                {
                    Response.Write("<script>window.alert('请检查上传文件格式是否正确!');</script>");
                }
            }
        }

        protected void ButtonCompleteCreate_Click(object sender, EventArgs e)
        {
            if (this.DropDownListDiseaseName.Text != "")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<main></main>");
                XmlNode root = doc.DocumentElement;
                TreeviewToXml(this.TreeViewDisease.Nodes, root);

                string Description = root.InnerXml;
                string DiseaseName = this.DropDownListDiseaseName.Text;
                //SavedType="NewCreate",or "TempSave", or "WaitModify"
                if (SaveDiagnosisKnowledge(Description, DiseaseName, "WaitModify"))
                {Response.Write("<script>window.alert('疾病成功保存并提交进一步修改!');</script>");}
                else {Response.Write("<script>window.alert('提交审核失败，请联系系统管理员!');</script>");}                
            }
            else
            {
                Response.Write("<script>window.alert('请选择所要保存的疾病!');</script>");
            }
        }

        protected void ButtonDeleteProperty_Click(object sender, EventArgs e)
        {
            string CurSelectedNodeText = this.TreeViewDisease.SelectedNode.Text.Trim();
            string[] Array = CurSelectedNodeText.Split(new char[2] { '【', '】' });
            string CurProp = Array[0];
            CurProp = CurProp.Trim();
            bool CorrectSelection = ((CurProp == "Symptom") | (CurProp == "Sign") | (CurProp == "Laboratory"));
            CorrectSelection = ((CorrectSelection) | (CurProp == "Radiology") | (CurProp == "OtherExam"));
            CorrectSelection = ((CorrectSelection) | (CurProp == "Properties") | (CurProp == "Property"));
            if (CorrectSelection)
            {
                TreeViewDisease.SelectedNode.ChildNodes.Clear();
                TreeViewDisease.SelectedNode.Parent.ChildNodes.Remove(TreeViewDisease.SelectedNode);
                TreeViewDisease.Nodes[0].Select();
            }
            else
            {
                string ErrorMessage = "您能删除“Symptom”、“Sign”、“Laboratory”、" +
                    "“Radiology”、“OtherExam”、“Properties”和“Property”，其他的节点不能删除。";
                Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
            }
        }

        protected void TreeViewFinding_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                string XmlNodeText = TreeViewFinding.SelectedNode.Text;
                string[] Array = XmlNodeText.Split(new char[2] { '【', '】' });
                XmlProperty.Text = Array[0];
                XmlText.Text = Array[1];
            }
            catch (Exception)
            {
                string XmlNodeText = TreeViewFinding.SelectedNode.Text;
                XmlProperty.Text = XmlNodeText;
                XmlText.Text = "";
            }
        }

        protected void XmlSave_Click(object sender, EventArgs e)
        {
            try
            {
                string xml1 = XmlProperty.Text;
                string xml2 = XmlText.Text;
                TreeViewFinding.SelectedNode.Text = xml1 + "【" + xml2 + "】";
            }
            catch (Exception)
            {
                string MsgInfo = "选择症候类型！";
                Response.Write("<script>window.alert('" + MsgInfo + "');</script>");
            }
        }

        protected void DeleteNode_Click(object sender, EventArgs e)
        {
            string CurSelectedNodeText = TreeViewFinding.SelectedNode.Text.Trim();
            string[] Array = CurSelectedNodeText.Split(new char[2] { '【', '】' });
            string CurSelectedProperty = Array[0];
            CurSelectedProperty = CurSelectedProperty.Trim();
            //TreeViewFinding.SelectedNode.
            if (CurSelectedProperty == "Property")
            {
                TreeViewFinding.SelectedNode.ChildNodes.Clear();
                TreeViewFinding.SelectedNode.Parent.ChildNodes.Remove(TreeViewFinding.SelectedNode);
                TreeViewFinding.Nodes[0].Select();
            }
            else if (CurSelectedProperty == "Option")
            {
                TreeViewFinding.SelectedNode.ChildNodes.Clear();
                TreeViewFinding.SelectedNode.Parent.ChildNodes.Remove(TreeViewFinding.SelectedNode);
                TreeViewFinding.Nodes[0].Select();
            }
            else
            {
                string MsgStr = "您只能删除“Property”或“Option”节点。";
                Response.Write("<script>window.alert('" + MsgStr + "');</script>");
            }
        }

        protected void AddProperty_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TreeNode PropertyNode = new System.Web.UI.WebControls.TreeNode();
            System.Web.UI.WebControls.TreeNode OptionsNode = new System.Web.UI.WebControls.TreeNode();
            string TimeStr = DateTime.Now.ToShortTimeString();
            OptionsNode.Text = "Options";
            OptionsNode.Value = TimeStr + "1";
            OptionsNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Option【新选项】", TimeStr + "2"));
            OptionsNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Option【新选项】", TimeStr + "3"));

            PropertyNode.Text = "Property";
            PropertyNode.Value = TimeStr + "4";
            PropertyNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Name【新特性】", TimeStr + "5"));
            PropertyNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Question", TimeStr + "6"));
            PropertyNode.ChildNodes.Add(OptionsNode);


            for (int i = 0; i < TreeViewFinding.Nodes[0].ChildNodes.Count; i++)
            {
                string CurNodeText = TreeViewFinding.Nodes[0].ChildNodes[i].Text.Trim();
                if (CurNodeText == "Properties")
                {
                    TreeViewFinding.Nodes[0].ChildNodes[i].ChildNodes.Add(PropertyNode);
                    TreeViewFinding.Nodes[0].ChildNodes[i].Selected = true;
                    break;
                }
            } 
        }

        protected void AddOption_Click(object sender, EventArgs e)
        {
            string CurSelectNodeText = TreeViewFinding.SelectedNode.Text.Trim();

            if (CurSelectNodeText == "Options")
            {
                TreeViewFinding.SelectedNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Option【新选项】"));
            }
            else
            {
                string MsgStr = "你当前选择的是“" + CurSelectNodeText +
                    "”节点，要添加请添加新的选项（Option），您必须选择需要添加选项的“Options”。";
                Response.Write("<script>window.alert('" + MsgStr + "');</script>");
            }
        }

        protected void ButtonSaveFinding_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<main></main>");
            XmlNode root = doc.DocumentElement;
            TreeviewToXml(this.TreeViewFinding.Nodes, root);
            string Description = root.InnerXml;

            SqlReader();
            string ChineseName = "";
            if (doc.FirstChild.HasChildNodes)
            {
                ChineseName = doc.GetElementsByTagName("ChineseName")[0].InnerText.Trim();
            }

            string ListedName = "";
            if (ListBoxFindings.SelectedIndex >= 0)
            {
                ListBoxFindings.SelectedItem.ToString().Trim();
            }

            if (ChineseName =="")
            {
                Response.Write("<script>alert('请从列表中选择需要修改的症候名称！')</script>");
            }
            else if (ListedName=="")
            {
                Response.Write("<script>alert('请从列表中选择需要修改的症候名称！')</script>");
            }
            else if (ChineseName == ListedName)
            {
                DateTime CurTime = DateTime.Now;//获取时间
                string ModifyTime = CurTime.ToString();
                string ModifierName = Session["UserName"].ToString();//填写上传者名字     
                string SqlString = "Update FindingsBase Set Description=@Description,ModifyTime=@ModifyTime," +
                    "ModifierName=@ModifierName where ChineseName=@ChineseName";
                SqlCommand cmd1 = new SqlCommand(SqlString, connection);
                cmd1.Parameters.AddWithValue("@Description", Description);
                cmd1.Parameters.AddWithValue("@ModifyTime", ModifyTime);
                cmd1.Parameters.AddWithValue("@ModifierName", ModifierName);
                cmd1.Parameters.AddWithValue("@ChineseName", ChineseName);
                connection.Open();
                int AffectedRow = cmd1.ExecuteNonQuery();
                connection.Close();

                if (AffectedRow > 0)
                { 
                    Response.Write("<script>alert('症候XML保存成功！')</script>"); 
                }
                else 
                { 
                    Response.Write("<script>alert('症候XML保存失败！')</script>"); 
                }
            }
            else
            {
                Response.Write("<script>alert('症候名称与列表名称不一致，保存失败！')</script>");
            }               
        }
    }
}
