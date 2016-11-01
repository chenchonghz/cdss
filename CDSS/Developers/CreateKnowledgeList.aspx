<%@ Page Title="创建知识库列表" Language="C#" MasterPageFile="~/Developer.master" AutoEventWireup="true" CodeBehind="CreateKnowledgeList.aspx.cs" Inherits="CDSS.CreateKnowledgeList" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolderDeveloper">
    <table  style="width:100%;">
        <tr>
            <td style="background-color: #C0C0C0; height: 49px;" colspan="3">疾病知识库构建<asp:SqlDataSource ID="SqlDataSourceDisease" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT ChineseName, EnglishName, DiseasesUID, SystemType FROM DiseasesBase WHERE (SystemType = @SystemType) ORDER BY ChineseName, EnglishName">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ListBoxSystemType" DefaultValue="呼吸系统" Name="SystemType" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="font-size: medium; font-weight: bold;" rowspan="6"></td>
            <td colspan="3" style="background-color: #CCCCCC; height: 49px;">症候知识库构建<asp:SqlDataSource ID="SqlDataSourceFinding" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [FindingsUID], [ChineseName], [EnglishName] FROM [FindingsBase] WHERE ([FindingType] = @FindingType) ORDER BY ChineseName, EnglishName">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ListBoxFindingType" DefaultValue="症状" Name="FindingType" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="background-color: #CCCCCC; width: 111px; height: 19px;">疾病系统归属</td>
            <td style="background-color: #CCCCCC; height: 19px;">疾病中文名称</td>
            <td style="background-color: #CCCCCC; height: 19px;">疾病英文名称</td>
            <td style="background-color: #C0C0C0; height: 19px; width: 93px;">症候类型归属</td>
            <td style="background-color: #C0C0C0; height: 19px;">症候中文名称</td>
            <td style="background-color: #C0C0C0; height: 19px;">症候英文名称</td>
        </tr>
        <tr>
            <td style="background-color: #CCCCCC; height: 241px; width: 111px;">
                <asp:ListBox ID="ListBoxSystemType" runat="server" Height="250px" Width="100px" AutoPostBack="True">
                    <asp:ListItem>呼吸系统</asp:ListItem>
                    <asp:ListItem>循环系统</asp:ListItem>
                    <asp:ListItem>消化系统</asp:ListItem>
                    <asp:ListItem>泌尿系统</asp:ListItem>
                    <asp:ListItem>生殖系统</asp:ListItem>
                    <asp:ListItem>神经系统</asp:ListItem>
                    <asp:ListItem>运动系统</asp:ListItem>
                </asp:ListBox>
            </td>
            <td style="background-color: #CCCCCC; height: 241px;">
                <asp:ListBox ID="ListBoxDiseaseChineseName" runat="server" Height="250px" Width="140px" DataSourceID="SqlDataSourceDisease" DataTextField="ChineseName" DataValueField="ChineseName" AutoPostBack="True" OnSelectedIndexChanged="ListBoxDiseaseChineseNameSelectedIndexChanged"></asp:ListBox>
            </td>
            <td style="background-color: #CCCCCC; height: 241px;">
                <asp:ListBox ID="ListBoxDiseaseEnglishName" runat="server" Height="250px" Width="140px" DataSourceID="SqlDataSourceDisease" DataTextField="EnglishName" DataValueField="EnglishName" AutoPostBack="True" OnSelectedIndexChanged="ListBoxDiseaseEnglishNameSelectedIndexChanged"></asp:ListBox>
            </td>
            <td style="background-color: #C0C0C0; height: 241px; width: 93px;">
                <asp:ListBox ID="ListBoxFindingType" runat="server" Height="250px" Width="100px" AutoPostBack="True">
                    <asp:ListItem>症状</asp:ListItem>
                    <asp:ListItem>体征</asp:ListItem>
                    <asp:ListItem>实验室检查</asp:ListItem>
                    <asp:ListItem>影像学检查</asp:ListItem>
                    <asp:ListItem>其它检查</asp:ListItem>
                </asp:ListBox>
            </td>
            <td style="background-color: #C0C0C0; height: 241px;">
                <asp:ListBox ID="ListBoxFindingChineseName" runat="server" Height="250px" Width="140px" DataSourceID="SqlDataSourceFinding" DataTextField="ChineseName" DataValueField="ChineseName" AutoPostBack="True" OnSelectedIndexChanged="ListBoxFindingChineseNameSelectedIndexChanged"></asp:ListBox>
            </td>
            <td style="background-color: #C0C0C0; height: 241px;">
                <asp:ListBox ID="ListBoxFindingEnglishName" runat="server" Height="250px" Width="140px" DataSourceID="SqlDataSourceFinding" DataTextField="EnglishName" DataValueField="EnglishName" AutoPostBack="True" OnSelectedIndexChanged="ListBoxFindingEnglishNameSelectedIndexChanged"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td style="background-color: #CCCCCC; width: 111px; height: 19px;">系统归属</td>
            <td style="background-color: #CCCCCC; height: 19px;">中文名称</td>
            <td style="background-color: #CCCCCC; height: 19px;">英文名称</td>
            <td style="background-color: #C0C0C0; height: 19px; width: 93px;">类型归属</td>
            <td style="background-color: #C0C0C0; height: 19px;">中文名称</td>
            <td style="background-color: #C0C0C0; height: 19px;">英文名称</td>
        </tr>
        <tr>
            <td style="background-color: #CCCCCC; width: 111px; height: 45px;"><asp:DropDownList ID="DropDownListSystemType" runat="server" Height="28px" Width="110px">
                <asp:ListItem>呼吸系统</asp:ListItem>
                <asp:ListItem>循环系统</asp:ListItem>
                <asp:ListItem>消化系统</asp:ListItem>
                <asp:ListItem>泌尿系统</asp:ListItem>
                <asp:ListItem>生殖系统</asp:ListItem>
                <asp:ListItem>运动系统</asp:ListItem>
                <asp:ListItem>神经系统</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="background-color: #CCCCCC; height: 45px; "><asp:TextBox ID="TextBoxChineseName" runat="server" Width="140px" ValidationGroup="1" style="font-size: small"></asp:TextBox>
            </td>
            <td style="background-color: #CCCCCC; height: 45px; "><asp:TextBox ID="TextBoxEnglishName" runat="server" Width="140px" ValidationGroup="1" style="font-size: small"></asp:TextBox>
            </td>
            <td style="background-color: #C0C0C0; height: 45px; width: 93px;">
                <asp:DropDownList ID="DropDownListFindingType" runat="server" Height="28px" Width="110px">
                    <asp:ListItem>症状</asp:ListItem>
                    <asp:ListItem>体征</asp:ListItem>
                    <asp:ListItem>实验室检查</asp:ListItem>
                    <asp:ListItem>影像学检查</asp:ListItem>
                    <asp:ListItem>其他检查</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="background-color: #C0C0C0; height: 45px;"><asp:TextBox ID="TextBoxFindingChineseName" runat="server" Width="140px" ValidationGroup="2" style="font-size: small"></asp:TextBox> 
            </td>
            <td style="background-color: #C0C0C0; height: 45px;"><asp:TextBox ID="TextBoxFindingEnglishName" runat="server" Width="140px" ValidationGroup="2" style="font-size: small"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="background-color: #CCCCCC;" colspan="3">
                <asp:Button ID="ButtonAddNewDisease" runat="server" OnClick="ButtonAddNewDisease_Click" Text="添加" Width="80px" ValidationGroup="1" ToolTip="增加新的疾病" />
                &nbsp;
                &nbsp;
                <asp:Button ID="ButtonSaveDisease" runat="server" OnClick="ButtonSaveDisease_Click" Text="修改" Width="80px" ValidationGroup="1" ToolTip="修改已有疾病" />
            &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ButtonDeleteDisease" runat="server" OnClientClick="return confirm('确实要删除吗？')" Text="删除" Width="80px" ToolTip="删除选中疾病" OnClick="ButtonDeleteDisease_Click" />
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxChineseName" Display="Dynamic" ErrorMessage="中文名不能为空！" style="color: #FF0000" ValidationGroup="1"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxEnglishName" Display="Dynamic" ErrorMessage="英文名不能为空！" style="color: #FF0000" ValidationGroup="1"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TextBoxEnglishName" Display="Dynamic" ErrorMessage="英文名必须为英文！" style="color: #FF0000" ValidationExpression="^[A-Za-z ]+$" ValidationGroup="1"></asp:RegularExpressionValidator>
            </td>
            <td colspan="3" style="background-color: #C0C0C0;">
                <asp:Button ID="ButtonAddNewFinding" runat="server" OnClick="ButttonAddNewFinding_Click" Text="添加" Width="80px" ValidationGroup="2" ToolTip="增加新的症候" />
                &nbsp;
                &nbsp;
                <asp:Button ID="ButtonSaveFinding" runat="server" OnClick="ButtonSaveFinding_Click" Text="修改" Width="80px" ValidationGroup="2" ToolTip="修改已有症候" />
            &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ButtonDeleteFinding" runat="server" OnClientClick="return confirm('确实要删除吗？')" OnClick="ButtonDeleteFinding_Click" Text="删除" Width="80px" ToolTip="删除选中症候"  />
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxFindingChineseName" ErrorMessage="中文名不能为空！" style="color: #FF0000" ValidationGroup="2" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBoxFindingEnglishName" ErrorMessage="英文名不能为空！" style="color: #FF0000" ValidationGroup="2" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="TextBoxFindingEnglishName" ErrorMessage="英文名必须为英文！" style="color: #FF0000" ValidationExpression="^[A-Za-z ]+$" ValidationGroup="2" Display="Dynamic"></asp:RegularExpressionValidator>
            </td>
        </tr>
        </table>
    <p>
        <asp:Label ID="Label1" runat="server" ForeColor="#CC0000" style="font-size: large"></asp:Label>
       
    </p>
    <p style="text-align: left">
        本页面需要完成的工作：
       
    </p>
    <p style="text-align: left">
        1、完成疾病中英文名称和系统归属数据的添加、修改保存以及删除功能；</p>
    <p style="text-align: left">
        2、完成症候中英文名称和归属类型数据的添加、修改保存以及删除功能。</p>
    <p style="text-align: left">
        3、如果熟悉页面布局设置，可以帮助美化页面布局。</p>
</asp:Content>

