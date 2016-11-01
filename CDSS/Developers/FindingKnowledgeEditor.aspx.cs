using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace CDSS
{
    public partial class FindingsKnowledgeEditor : System.Web.UI.Page
    {
        //将Xml类型转化成treeview类型
        private void XmlToTreeview(XmlNodeList xmlNodeList, System.Web.UI.WebControls.TreeNodeCollection treeNode)
        {
            int CurOrder = 0;
            string CurTime = DateTime.Now.ToLongTimeString();
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                System.Web.UI.WebControls.TreeNode newTreeNode = new System.Web.UI.WebControls.TreeNode();
                newTreeNode.Text = xmlNode.LocalName;
                newTreeNode.Value = CurTime + CurOrder.ToString();
                CurOrder++;
                if (xmlNode.HasChildNodes)
                {
                    if (xmlNode.ChildNodes[0].NodeType == XmlNodeType.Text)
                    {
                        newTreeNode.Text += " 【 " + xmlNode.ChildNodes[0].Value+"】";
                        //newTreeNode.Value = xmlNode.ChildNodes[0].Value;
                        newTreeNode.Value = CurTime + CurOrder.ToString();
                        CurOrder++;
                    }
                    XmlToTreeview(xmlNode.ChildNodes, newTreeNode.ChildNodes);
                }
                treeNode.Add(newTreeNode);
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection Connection = new SqlConnection(strConnection);
                Connection.Open();
                if (Session["type"].ToString() == "Developer")
                { }
                else
                {
                    string MsgInfo = "请先登录！";
                    Response.Write("<script>window.alert('" + MsgInfo + "');</script>"); 
                    Response.Redirect("Default.aspx");
                }
                Connection.Close();
            }
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void SaveFindingKnowledgeToDatabase(string Description, string ChineseName, string SavedType)
        {
            //SavedType=="NewCreate",or "TempSave", or "WaitModify"
            //获取数据库连接字符串
            string conString = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            DateTime CurTime = DateTime.Now;//获取时间
            string CreateTime = CurTime.ToString();
            string CreatorName = Session["UserName"].ToString();//填写上传者名字

            //设置带参数的SQL语句
            string SqlString = "Update FindingsBase Set Description = @Description, ";
            if (SavedType == "NewCreate")  //or "TempSave", or "WaitModify"
            {
                SqlString = SqlString+"CreateTime=@CreateTime, CreatorName=@CreatorName,ModifyNeeded=0 " +
                    "where ChineseName = @ChineseName";
            }
            else if (SavedType == "TempSave")
            {
                SqlString = SqlString + "CreateTime=@CreateTime, CreatorName=@CreatorName,ModifyNeeded=0 " +
                    "where ChineseName = @ChineseName"; 
            }
            else if (SavedType == "WaitModify")
            {
                SqlString = SqlString + "CreateTime=@CreateTime, CreatorName=@CreatorName,ModifyNeeded=1 " +
                    "where ChineseName = @ChineseName"; 
            }

            //创建SQL连接和SQL命令
            SqlConnection conn = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(SqlString, conn);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@ChineseName", ChineseName);
            cmd.Parameters.AddWithValue("@CreateTime", CreateTime);
            cmd.Parameters.AddWithValue("@CreatorName", CreatorName);

            conn.Open();
            //判断SQL语句是否成功执行
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        protected void ButtonUploadFindingFile_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();

            string CurFileName = FileUploadFindingFile.FileName;
            System.IO.Stream myStream = FileUploadFindingFile.FileContent;//读出fileuploadfindingfile中xml文件内容
            System.IO.StreamReader myread = new System.IO.StreamReader(myStream);
            string XmlString = myread.ReadToEnd();//讲文件内容转换成string
            if (XmlString.Trim() == "")
            {
                doc.Load(CurFileName);
            }
            else
            {
                doc.LoadXml(XmlString); 
            }           

            if (doc.GetElementsByTagName("ChineseName").Count > 0)
            {
                string Description = doc.DocumentElement.OuterXml;
                string DiseaseName = doc.GetElementsByTagName("ChineseName")[0].InnerText.Trim();

                GridViewRow CurRow = this.GridViewFindingList.SelectedRow;
                string CurFinding = CurRow.Cells[1].Text.Trim();

                if (DiseaseName == CurFinding)
                {
                    string CurDescription = doc.DocumentElement.OuterXml;
                    //保存XML内容到数据库相应字段
                    SaveFindingKnowledgeToDatabase(CurDescription, CurFinding, "NewCreate");
                    Response.Write("<script>window.alert('上传xml文件成功！');</script>");
                }
                else
                {
                    Response.Write("<script>window.alert('当前选择的症候名称与XML文档内的症候名称不一致，上传xml文件失败！');</script>");
                }                
            }
            else { Response.Write("<script>window.alert('文档格式存在问题，请检查确认。');</script>"); }
        }

        protected void ListBoxFindingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySelectFindingList();
        }

        protected void ListBoxFindingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                GridViewRow CurRow = this.GridViewFindingList.SelectedRow;
                string CurFinding = CurRow.Cells[1].Text;
                string Description = "Select Description From FindingsBase where ChineseName='" + CurFinding + "'";
                SqlCommand InsertXml = new SqlCommand(Description, conn);
                conn.Open();
                SqlDataReader dr = InsertXml.ExecuteReader();

                if (dr.Read())
                {
                    Description = dr["Description"].ToString();
                }

                conn.Close();

                if (Description=="") 
                {                    
                    string ErrorMessage = "症候【" + CurFinding + "】知识XML尚未创建！";
                    Response.Write("<script>window.alert('" + ErrorMessage + "');</script>"); 
                }
                else 
                {
                    XmlDocument xmlDoc = new XmlDocument();               
                xmlDoc.LoadXml(Description);
                XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                this.TreeViewFinding.Nodes.Clear();
                XmlToTreeview(xmlNodes, this.TreeViewFinding.Nodes);
                this.TreeViewFinding.ExpandAll();
                }
            }
            catch(Exception)
            {
                string ErrorMessage = "打开症候知识XML时出错，请联系系统管理员！";
                Response.Write("<script>window.alert('" + ErrorMessage + "');</script>"); 
            }
        }

        protected void TreeViewFinding_SelectedNodeChanged(object sender, EventArgs e)
        {
            try {             
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
            //TreeViewFinding.SelectedNode.Selected = true;
        }


        protected void ButtonTempSave_Click(object sender, EventArgs e)
        {
            /*try
            {
                string xml1 = XmlProperty.Text;
                string xml2 = XmlText.Text;
                TreeViewFinding.SelectedNode.Text = xml1 + "【" + xml2 + "】";
            }
            catch (Exception)
            {
                string MsgInfo = "选择症候类型！";
                Response.Write("<script>window.alert('" + MsgInfo + "');</script>");
            } */
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<main></main>");

            XmlNode root = doc.DocumentElement;
            TreeviewToXml(this.TreeViewFinding.Nodes, root);

            GridViewRow CurRow = this.GridViewFindingList.SelectedRow;
            string CurFinding = CurRow.Cells[1].Text; 
            //string CurFinding = this.ListBoxFindingList.SelectedItem.Text;
            //获取XML的内容
            string CurDescription = root.InnerXml;
            //保存XML内容到数据库相应字段
            //SavedType=="NewCreate",or "TempSave", or "WaitModify"
            SaveFindingKnowledgeToDatabase(CurDescription, CurFinding, "TempSave");
            Response.Write("<script>window.alert('症候知识保存成功！');</script>");
            DisplaySelectFindingList();
        }

        protected void XmlSave_Click(object sender, EventArgs e)
        {
            try
            {
                string xml1 =XmlProperty.Text;
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
                string MsgStr="您只能删除“Property”或“Option”节点。";
                Response.Write("<script>window.alert('" + MsgStr + "');</script>"); 
            }
        }

        protected void ButtonCompleteCreate_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<main></main>");

            XmlNode root = doc.DocumentElement;
            TreeviewToXml(this.TreeViewFinding.Nodes, root);

            GridViewRow CurRow = this.GridViewFindingList.SelectedRow;
            string CurFinding = CurRow.Cells[1].Text; 
            //string CurFinding = this.ListBoxFindingList.SelectedItem.Text;
            //获取XML的内容
            string CurDescription = root.InnerXml;
            //保存XML内容到数据库相应字段
            //SavedType=="NewCreate",or "TempSave", or "WaitModify"
            SaveFindingKnowledgeToDatabase(CurDescription, CurFinding, "WaitModify");
            Response.Write("<script>window.alert('保存并提交审核！');</script>");
            DisplaySelectFindingList();
        }

        protected void AddProperty_Click(object sender, EventArgs e)
        {            
            System.Web.UI.WebControls.TreeNode PropertyNode = new System.Web.UI.WebControls.TreeNode();
            System.Web.UI.WebControls.TreeNode OptionsNode = new System.Web.UI.WebControls.TreeNode();
            string TimeStr = DateTime.Now.ToShortTimeString();
            OptionsNode.Text = "Options";
            OptionsNode.Value=TimeStr+"1";
            OptionsNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Option【新选项】", TimeStr + "2"));
            OptionsNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Option【新选项】",TimeStr + "3"));

            PropertyNode.Text="Property";
            PropertyNode.Value = TimeStr + "4"; 
            PropertyNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Name【新特性】",TimeStr + "5"));
            PropertyNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Question",TimeStr + "6"));
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

            if (CurSelectNodeText=="Options")
            {
                TreeViewFinding.SelectedNode.ChildNodes.Add(new System.Web.UI.WebControls.TreeNode("Option【新选项】"));
            }
            else
            {
                string MsgStr="你当前选择的是“"+CurSelectNodeText+
                    "”节点，要添加请添加新的选项（Option），您必须选择需要添加选项的“Options”。";
                Response.Write("<script>window.alert('" + MsgStr + "');</script>"); 
            }
        }

        protected void GridViewFindingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strConn = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                GridViewRow CurRow = this.GridViewFindingList.SelectedRow;
                string CurFinding = CurRow.Cells[1].Text;
                string Description = "Select Description From FindingsBase where ChineseName='" + CurFinding + "'";
                SqlCommand InsertXml = new SqlCommand(Description, conn);
                conn.Open();
                SqlDataReader dr = InsertXml.ExecuteReader();

                if (dr.Read())
                {
                    Description = dr["Description"].ToString();
                }

                conn.Close();

                if (Description == "")
                {
                    string ErrorMessage = "症候【" + CurFinding + "】知识XML尚未创建！";
                    Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Description);
                    XmlNodeList xmlNodes = xmlDoc.ChildNodes;
                    this.TreeViewFinding.Nodes.Clear();
                    XmlToTreeview(xmlNodes, this.TreeViewFinding.Nodes);
                    this.TreeViewFinding.ExpandAll();
                }
            }
            catch (Exception)
            {
                string ErrorMessage = "打开症候知识XML时出错，请联系系统管理员！";
                Response.Write("<script>window.alert('" + ErrorMessage + "');</script>");
            }
        }

        protected void RadioButtonListFindingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySelectFindingList();
        }
        private void DisplaySelectFindingList()
        {
            string FindingType = this.ListBoxFindingType.SelectedValue;
            String SqlString = "SELECT [ChineseName],[CreatorName] FROM [FindingsBase] WHERE ([FindingType] = @FindingType)"; //Order by ChineseName
            switch (this.RadioButtonListFindingType.SelectedIndex)
            {
                case 0: SqlString = SqlString + " and (Description is Null) "; break;
                case 1: SqlString = SqlString + " and ((ModifyNeeded=0) or (ModifyNeeded is Null))"; break;
                case 2: SqlString = SqlString +""; break;
            }
            string CurUser=Session["UserName"].ToString();
            if (CheckBoxListYourself.Checked)
            {
                SqlString = SqlString + "and (CreatorName=@CurUser)";
            }
            SqlString = SqlString + " Order by ChineseName";
            string strConnection = WebConfigurationManager.ConnectionStrings["CDSSConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(strConnection);
            SqlDataAdapter sda = new SqlDataAdapter(SqlString, Connection);
            sda.SelectCommand.Parameters.AddWithValue("@FindingType", FindingType);
            sda.SelectCommand.Parameters.AddWithValue("@CurUser", CurUser);
            Connection.Open();
            DataSet ds = new DataSet();
            sda.Fill(ds);
            this.GridViewFindingList.DataSource = ds;
            this.GridViewFindingList.AutoGenerateColumns = false;
            this.GridViewFindingList.DataBind();
            Connection.Close();
        }

        protected void CheckBoxListYourself_CheckedChanged(object sender, EventArgs e)
        {
            DisplaySelectFindingList();
        }


        protected void GridViewFindingList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            DisplaySelectFindingList(); 
            GridViewFindingList.PageIndex = e.NewPageIndex;
            GridViewFindingList.DataBind();
        }
    }
}
