<%@ Page Title="编辑疾病知识库" Language="C#" MasterPageFile="~/Developer.master" AutoEventWireup="true" CodeBehind="DiseaseKnowledgeEditor.aspx.cs" Inherits="CDSS.DiseaseKnowledgeEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDeveloper" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="text-align: left; background-color: #909090; font-weight: bold; color: #0000FF; height: 29px; width: 335px;">选择并修改疾病知识库</td>
            <td style="text-align: left; background-color: #909090; font-weight: bold; color: #0000FF; width: 375px; height: 29px;">添加症候<mark>名称</mark>并设置其属性类型和级别</td>
            <td style="text-align: left; background-color: #909090; font-weight: bold; color: #0000FF; height: 29px;">添加症候<mark>特征</mark>并设置其属性及级别</td>
        </tr>
        <tr>
            <td style="text-align: left; vertical-align: middle;">疾病系统：<asp:DropDownList ID="DropDownListSystemType" runat="server" Height="25px" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSystemType_SelectedIndexChanged">
            </asp:DropDownList>
                <br />
                疾病名称：<asp:DropDownList ID="DropDownListDiseaseName" runat="server" Height="27px" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListDiseaseName_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" Width="267px" BorderStyle="None" />
                <asp:Button ID="ButtonSaveUploaded" runat="server" Text="上传" OnClick="ButtonSaveUploaded_Click" />
            </td>
            <td style="text-align: left; vertical-align: middle;">选择添加属性类型:<asp:RadioButtonList ID="RadioButtonListPropertyType1" runat="server" Font-Size="Smaller" Height="16px" RepeatColumns="3" Width="259px">
                <asp:ListItem Selected="True">常见度</asp:ListItem>
                <asp:ListItem>特异度</asp:ListItem>
                <asp:ListItem>重要度</asp:ListItem>
            </asp:RadioButtonList>
                选择添加属性级别:<asp:RadioButtonList ID="RadioButtonListPropertyScale1" runat="server" Font-Size="Smaller" Height="19px" RepeatColumns="5" RepeatDirection="Horizontal" Width="262px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem Selected="True">3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td style="text-align: left; vertical-align: middle;">选择添加属性类型:<asp:RadioButtonList ID="RadioButtonListPropertyType2" runat="server" Font-Size="Smaller" Height="19px" RepeatColumns="3" RepeatDirection="Horizontal" Width="260px">
                <asp:ListItem Selected="True">常见度</asp:ListItem>
                <asp:ListItem>特异度</asp:ListItem>
                <asp:ListItem>重要度</asp:ListItem>
            </asp:RadioButtonList>
                选择添加属性级别:<asp:RadioButtonList ID="RadioButtonListPropertyScale2" runat="server" Font-Size="Smaller" Height="19px" RepeatColumns="5" RepeatDirection="Horizontal" Width="260px">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem Selected="True">3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; vertical-align: top; width: 335px;">
                <asp:Button ID="ButtonTempSave" runat="server" Text="保存数据修改内容" Width="160px" OnClick="ButtonTempSave_Click" />
                <asp:Button ID="ButtonCompleteCreate" runat="server" OnClick="ButtonCompleteCreate_Click" Text="保存并提交审核" Width="160px" />
                <br />
                疾病知识库：<asp:Panel ID="Panel2" runat="server" Height="441px" ScrollBars="Auto" Width="338px">
                    <asp:TreeView Style="line-height: 50%" ID="TreeViewDisease" runat="server" ShowLines="True" Width="322px" Font-Size="Small" NodeIndent="5" LineImagesFolder="~/TreeLineImages">
                        <LeafNodeStyle HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                        <SelectedNodeStyle BackColor="Yellow" ForeColor="Red" />
                    </asp:TreeView>
                </asp:Panel>
            </td>
            <td style="text-align: left; vertical-align: top; width: 375px;">
                <asp:Button ID="ButtonAddFinding" runat="server" Text="添加症候名称&lt;&lt;&lt;" Width="260px" OnClick="ButtonAddFinding_Click" />
                <br />
                选择症候类型:<asp:DropDownList ID="DropDownListFindingType" runat="server" Height="20px" Width="178px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListFindingType_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                选择添加症状:<br />
                <asp:ListBox ID="ListBoxFindings" runat="server" Height="438px" Width="260px" AutoPostBack="True" OnSelectedIndexChanged="ListBoxFindings_SelectedIndexChanged"></asp:ListBox>
                <br />
            </td>
            <td style="vertical-align: top; text-align: left">
                <asp:Button ID="ButtonAddProperty" runat="server" Text="添加症候特征&lt;&lt;&lt;" Width="322px" OnClick="ButtonAddProperty_Click" />
                <br />
                选择症状属性:<asp:Panel ID="Panel1" runat="server" Height="452px" ScrollBars="Auto">
                    <asp:TreeView Style="line-height: 50%" ID="TreeViewFinding" runat="server" NodeIndent="0" ShowLines="True" Width="324px" Font-Size="Small" Height="98px" LineImagesFolder="~/TreeLineImages" ImageSet="Simple" OnSelectedNodeChanged="TreeViewFinding_SelectedNodeChanged">
                        <SelectedNodeStyle BackColor="Yellow" BorderStyle="Dotted" ForeColor="Red" />
                    </asp:TreeView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="text-align: center; vertical-align: middle; background-color: #C0C0C0;" colspan="3">
                <asp:Button ID="ButtonDeleteProperty" runat="server" Text="删除疾病属性" Width="96px" OnClick="ButtonDeleteProperty_Click" />
                &nbsp;&nbsp;症候属性：<asp:TextBox ID="XmlProperty" runat="server" Width="63px"></asp:TextBox>
                &nbsp;值：<asp:TextBox ID="XmlText" runat="server" Width="111px"></asp:TextBox>
                &nbsp;<asp:Button ID="XmlSave" runat="server" OnClick="XmlSave_Click" Text="特征属性修改" Width="100px" />
                <asp:Button ID="DeleteNode" runat="server" OnClick="DeleteNode_Click" Text="特征属性删除" Width="100px" />
                <asp:Button ID="AddProperty" runat="server" OnClick="AddProperty_Click" Text="增加Property" Width="100px" />
                <asp:Button ID="AddOption" runat="server" OnClick="AddOption_Click" Text="增加Option" Width="100px" />
                <asp:Button ID="ButtonSaveFinding" runat="server" OnClick="ButtonSaveFinding_Click" Text="保存症候修改" Width="95px" />
            </td>
        </tr>
    </table>
    <p style="text-align: left">
        &nbsp;
    </p>
</asp:Content>
