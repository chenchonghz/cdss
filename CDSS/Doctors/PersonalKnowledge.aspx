<%@ Page Title="" Language="C#" MasterPageFile="~/Doctor.master" AutoEventWireup="true" CodeBehind="PersonalKnowledge.aspx.cs" Inherits="CDSS.PersonalKnowledge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDoctor" runat="server">
     <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Text="医生个性化疾病知识库"></asp:Label>
&nbsp;<table style="width:100%;">
            <tr>
                <td style="text-align: left; background-color: #909090; font-weight: bold; color: #0000FF; height: 19px; width: 335px;">选择并修改疾病知识库</td>
                <td style="text-align: left; background-color: #909090; font-weight: bold; color: #0000FF; width: 375px; height: 19px;">添加症候<mark>名称</mark>并设置其属性类型和级别</td>
                <td style="text-align: left; background-color: #909090; font-weight: bold; color: #0000FF; height: 19px;">添加症候<mark>特征</mark>并设置其属性及级别</td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top; width: 335px;">疾病系统：<asp:DropDownList ID="DropDownListSystemType" runat="server" Height="16px" Width="267px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSystemType_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    疾病名称：<asp:DropDownList ID="DropDownListDiseaseName" runat="server" Height="16px" Width="267px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListDiseaseName_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="Label1" runat="server" Text="疾病属性："></asp:Label>
                    <asp:TextBox ID="TextBox_Edit" runat="server" Font-Size="Small" Width="190px"></asp:TextBox>&nbsp;
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="修改" />
                    <asp:Button ID="ButtonSaveDiseaseXML" runat="server" Text="保存疾病知识库修改" Width="333px" OnClick="ButtonSaveDiseaseXML_Click" />
                    <br />
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="267px" BorderStyle="None" />&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="上传" OnClick="Button2_Click" />
                    <br />
                    疾病知识库：<asp:Panel ID="Panel2" runat="server" Height="544px" ScrollBars="Auto" Width="338px">
                        <asp:TreeView style="line-height:50%" ID="TreeViewDisease" runat="server" ShowLines="True" Width="322px" OnSelectedNodeChanged="TreeViewDisease_SelectedNodeChanged" Font-Size="Small" NodeIndent="5" LineImagesFolder="~/TreeLineImages">
                            <LeafNodeStyle HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </asp:Panel>
                </td>
                <td style="text-align: left; vertical-align: top; width: 375px;">选择添加属性类型:<asp:RadioButtonList ID="RadioButtonListPropertyType1" runat="server" Font-Size="Smaller" Height="16px" RepeatColumns="3" Width="259px">
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
                    <br />
                    <asp:Button ID="ButtonAddFinding" runat="server" Text="添加症候名称&lt;&lt;&lt;" Width="260px" OnClick="ButtonAddFinding_Click" />
                    <br />
                    选择症候类型:<asp:DropDownList ID="DropDownListFindingType" runat="server" Height="20px" Width="178px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListFindingType_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    选择添加症状:<br />
                    <asp:ListBox ID="ListBoxFindings" runat="server" Height="515px" Width="260px" AutoPostBack="True" OnSelectedIndexChanged="ListBoxFindings_SelectedIndexChanged"></asp:ListBox>
                    <br />
                </td>
                <td style="vertical-align: top; text-align: left">选择添加属性类型:<asp:RadioButtonList ID="RadioButtonListPropertyType2" runat="server" Font-Size="Smaller" Height="19px" RepeatColumns="3" RepeatDirection="Horizontal" Width="260px">
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
                    <br />
                    <asp:Button ID="ButtonAddProperty" runat="server" Text="添加症候特征&lt;&lt;&lt;" Width="322px" OnClick="ButtonAddProperty_Click" />
                    <br />
                    选择症状属性:<asp:Panel ID="Panel1" runat="server" Height="532px" ScrollBars="Auto">
                        <asp:TreeView style="line-height:50%" ID="TreeViewFinding" runat="server" NodeIndent="5" ShowLines="True" Width="324px" ShowCheckBoxes="Leaf" Font-Size="Small" Height="48px" LineImagesFolder="~/TreeLineImages">
                            <LeafNodeStyle HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 335px">&nbsp;</td>
                <td style="width: 375px">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p>
    <p style="text-align: left">
        &nbsp;</p>
</asp:Content>
