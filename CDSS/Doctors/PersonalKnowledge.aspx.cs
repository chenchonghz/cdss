using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.IO;

namespace CDSS
{
    public partial class PersonalKnowledge : System.Web.UI.Page
    {
       //全局sql连接对象定义
        SqlConnection connection = new SqlConnection();
        //全局数据集定义
        DataSet ds = new DataSet();
        int flag = 0;
        TreeNode tree_Node = null;

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
            String DSystemType = "Select distinct SystemType From DiseasesBase";
            SqlDataAdapter da_DST = new SqlDataAdapter(DSystemType, connection);
            da_DST.Fill(ds, "DSystemType");

            //产生FindingsBase的FindingType数据集
            String FFindingType = "Select distinct FindingType From FindingsBase";
            SqlDataAdapter da_FFT = new SqlDataAdapter(FFindingType, connection);
            da_FFT.Fill(ds, "FFindingType");

        }

        //创建DChineseName数据集
        private void DCN_SqlToDataset(string systemType)
        {
            SqlReader();
            String DChineseName = "Select ChineseName From DiseasesBase Where SystemType='" + systemType + "'";
            SqlDataAdapter da_DCN = new SqlDataAdapter(DChineseName, connection);
            da_DCN.Fill(ds, "DChineseName");

        }

        //创建FChineseName数据集
        private void FCN_SqlToDataset(string findingType)
        {
            SqlReader();
            String FChineseName = "Select ChineseName From FindingsBase Where FindingType='" + findingType + "'";
            SqlDataAdapter da_FCN = new SqlDataAdapter(FChineseName, connection);
            da_FCN.Fill(ds, "FChineseName");

        }

        //读取数据库Xml文件并转换成String字符串
        private string SqlXmlToString(string SqlTabel, string ChineseName)
        {
            SqlReader();
            String XmlRead = "Select Description From " + SqlTabel + " Where ChineseName='" + ChineseName + "'";
            SqlCommand cmd1 = new SqlCommand(XmlRead, connection);
            connection.Open();
            SqlDataReader dr = cmd1.ExecuteReader();

            string Description = null;

            if (dr.Read())
            {
                Description = dr["Description"].ToString();
            }
            connection.Close();
            return Description;

        }

        //把Xml对应的字符串存储到数据库中
        private void StringToSqlXml(string description, string ChineseName)
        {
            SqlReader();
            string XmlInput = "Update DiseasesBase Set Description='" + description + "' where ChineseName='" + ChineseName + "'";
            SqlCommand cmd1 = new SqlCommand(XmlInput, connection);

            connection.Open();
            cmd1.ExecuteNonQuery();
            connection.Close();

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
                        tNode.Parent.ChildNodes[4].ChildNodes.Add(treeNode.ChildNodes[0]);
                        break;
                    }
                }
                catch (Exception)
                {
                    if (Array[0].Trim() == type)
                    {
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
        public void Search(TreeNodeCollection nodeList, String string1, String name)
        {
            foreach (TreeNode treeNode in nodeList)
            {
                String[] array = treeNode.Text.Split(new char[2] { '【', '】' });
                try
                {
                    if (array[0].Trim() == string1 && array[1].Trim() == name.Trim())
                    {
                        flag = 1;
                        tree_Node = treeNode;
                        break;
                    }
                }
                catch (Exception) { }

                if (treeNode.ChildNodes.Count > 0)
                {
                    Search(treeNode.ChildNodes, string1, name);
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
            string Description = SqlXmlToString("DiseasesBase", this.DropDownListDiseaseName.Text);
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

            this.TextBox_Edit.Text = "";
            this.Label1.Text = "疾病属性：";
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
            xmlDoc.LoadXml(SqlXmlToString("FindingsBase", this.ListBoxFindings.SelectedItem.Text));

            XmlNodeList xmlNodes = xmlDoc.ChildNodes;

            this.TreeViewFinding.Nodes.Clear();
            XmlToTreeview(xmlNodes, this.TreeViewFinding.Nodes);

            this.TreeViewFinding.ExpandAll();
        }

        //保存疾病修改，并上传疾病的xml结构
        protected void ButtonSaveDiseaseXML_Click(object sender, EventArgs e)
        {
            if (this.DropDownListDiseaseName.Text != "")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<main></main>");
                XmlNode root = doc.DocumentElement;
                TreeviewToXml(this.TreeViewDisease.Nodes, root);

                StringToSqlXml(root.InnerXml, this.DropDownListDiseaseName.Text);

                this.Label2.Text = "保存疾病成功！";
            }
            else
            {
                this.Label2.Text = "请选择所要保存的疾病";
            }

        }

        //添加症候名字控件
        protected void ButtonAddFinding_Click(object sender, EventArgs e)
        {
            string xmlname;
            switch(this.DropDownListFindingType.Text.Trim())
            {
                case "易感因素": xmlname = "PredispositionName"; break;
                case "症状": xmlname = "SymptomName"; break;
                case "体征": xmlname = "SignName"; break;
                case "实验室检查": xmlname="LaboratoryName"; break;
                case "影像学检查": xmlname = "RadiologyName"; break;
                default: xmlname="OtherExamName"; break;
            }

            flag = 0;
            try
            {
                Search(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
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

                    string xml_string;
                    switch(this.DropDownListFindingType.Text.Trim())
                    {
                        case "易感因素": xml_string = @"<Predisposition><PredispositionName/><Frequency />
                            <Specifity /><Importance/><Properties/></Predisposition>"; break;
                        case "症状": xml_string = @"<Symptom><SymptomName/><Frequency />
                            <Specifity /><Importance/><Properties/></Symptom>"; break;
                        case "体征": xml_string = @"<Sign><SignName/><Frequency />
                            <Specifity /><Importance/><Properties/></Sign>"; break;
                        case "实验室检查": xml_string=@"<Laboratory><LaboratoryName/><Frequency />
                            <Specifity /><Importance/><Properties/></Laboratory>"; break;
                        case "影像学检查": xml_string=@"<Radiology><RadiologyName/><Frequency />
                            <Specifity /><Importance/><Properties/></Radiology>"; break;
                        default: xml_string=@"<OtherExam><OtherExamName/><Frequency />
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


                    this.Label2.Text = "添加症候名成功！";
                }
                catch (Exception)
                {

                    this.Label2.Text = "请选择所需的症候名";

                }
            }

        }

        //症候名称改变处理函数
        protected void TreeViewDisease_SelectedNodeChanged(object sender, EventArgs e)
        {
            string[] Array = this.TreeViewDisease.SelectedNode.Text.Split(new char[2] { '【', '】' });
            this.Label1.Text = Array[0];
            try
            {
                this.TextBox_Edit.Text = Array[1];
            }
            catch (Exception)
            {
                this.TextBox_Edit.Text = "";
            }

        }

        //修改疾病属性控件
        protected void Button1_Click(object sender, EventArgs e)
        {
            treeviewSearch(this.TreeViewDisease.Nodes, this.Label1.Text, TextBox_Edit.Text);
        }

        //添加症候属性控件处理函数
        protected void ButtonAddProperty_Click(object sender, EventArgs e)
        {
            string xmlname;
            switch (this.DropDownListFindingType.Text.Trim())
            {
                case "易感因素": xmlname = "PredispositionName"; break;
                case "症状": xmlname = "SymptomName"; break;
                case "体征": xmlname = "SignName"; break;
                case "实验室检查": xmlname = "LaboratoryName";  break;
                case "影像学检查": xmlname = "RadiologyName";  break;
                default: xmlname = "OtherExamName";  break;
            }

            flag = 0;

            try
            {
                Search(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
                flag = 0;
                string[] PropertyName = this.TreeViewFinding.CheckedNodes[0].Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                Search(tree_Node.Parent.ChildNodes, "PropertyName", PropertyName[1].Trim());


            }
            catch (Exception) { }

            if (flag == 1)
            {
                flag = 0;
                Search(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
                TreeNode treeNode_1 = tree_Node;

                for (int i = 0; i < this.TreeViewFinding.CheckedNodes.Count; i++)
                {
                    try
                    {
                        string[] PropertyName = this.TreeViewFinding.CheckedNodes[i].Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });

                        Search(treeNode_1.Parent.ChildNodes, "PropertyName", PropertyName[1].Trim());
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

            }
            else
            {

                if (this.TreeViewFinding.CheckedNodes.Count == 0)
                {

                    this.Label2.Text = "未选择症候属性!";
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    
                    xmlDoc.LoadXml(@"<Property><PropertyName/><OptionName /><Frequency/>
                                <Specifity /><Importance /></Property>");

                    XmlNodeList xmlNodes = xmlDoc.ChildNodes;

                    for (int i = 0; i < this.TreeViewFinding.CheckedNodes.Count; i++)
                    {
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
                                string[] PropertyName = this.TreeViewFinding.CheckedNodes[i].Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                                TNode.Text = Array[0].Trim() + " 【 " + PropertyName[1].Trim() + "】";
                            }
                            else if (Array[0].Trim() == "OptionName")
                            {
                                string[] PropertyOption = this.TreeViewFinding.CheckedNodes[i].Text.Split(new char[2] { '【', '】' });
                                TNode.Text = Array[0].Trim() + " 【 " + PropertyOption[1].Trim() + "】";
                            }
                            else if (Array[0].Trim() == str_name1)
                            {
                                TNode.Text = Array[0].Trim() + " 【 " + this.RadioButtonListPropertyScale2.SelectedItem.Text + "】";
                            }

                        }
                        //添加treeview节点
                        AddTreeViewNode(this.TreeViewDisease.Nodes, treeNode, xmlname, this.ListBoxFindings.SelectedItem.Text);

                    }
                    try
                    {
                        flag = 0;
                        Search(this.TreeViewDisease.Nodes, xmlname, this.ListBoxFindings.SelectedItem.Text);
                        flag = 0;
                        string[] Property_Name = this.TreeViewFinding.CheckedNodes[0].Parent.Parent.ChildNodes[0].Text.Split(new char[2] { '【', '】' });
                        Search(tree_Node.Parent.ChildNodes, "PropertyName", Property_Name[1].Trim());
                        if (flag == 1)
                        {
                            this.Label2.Text = "添加症候属性成功！";
                        }
                    }
                    catch (Exception)
                    {
                        this.Label2.Text = "未添加症候名！";
                    }

                }
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {

                XmlDocument doc = new XmlDocument();
                System.IO.Stream myStream = this.FileUpload1.FileContent;//读出fileuploadfindingfile中xml文件内容
                System.IO.StreamReader myread = new System.IO.StreamReader(myStream);
                string result = myread.ReadToEnd();//讲文件内容转换成string
                doc.LoadXml(result);

                if (doc.GetElementsByTagName("ChineseName").Count>0)
                {

                    StringToSqlXml(doc.DocumentElement.OuterXml, doc.GetElementsByTagName("ChineseName")[0].InnerText);

                };
                this.Label2.Text = "上传xml文件成功！";

                DropDownListDiseaseName_SelectedIndexChanged(sender, e);
            }
            catch (Exception)
            {
                this.Label2.Text = "请选择上传文件";
            }

        }
    }
}