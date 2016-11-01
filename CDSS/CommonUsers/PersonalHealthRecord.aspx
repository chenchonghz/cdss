<%@ Page Title="个人健康档案" Language="C#" MasterPageFile="~/CommonUser.master" AutoEventWireup="true" CodeBehind="PersonalHealthRecord.aspx.cs" Inherits="CDSS.PersonalHealthRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderCommonUser" runat="server">
    <p>
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left; color: #0000FF; width: 119px; height: 17px;">基本信息：</td>
                <td style="color: #0000FF; text-align: left; height: 17px;">
                    <asp:Literal ID="LiteralLoginName" runat="server" Text="LoginName" Visible="False"></asp:Literal>
                </td>
                <td style="color: #0000FF; text-align: left; height: 17px;">
                    <asp:Literal ID="LiteralPhrUID" runat="server" Text="PhrUID" Visible="False"></asp:Literal>
                </td>
                <td style="color: #0000FF; text-align: left; height: 17px;"></td>
                <td style="color: #0000FF; text-align: left; height: 17px;">
                    <asp:Literal ID="LiteralUserUID" runat="server" Text="UserUID" Visible="False"></asp:Literal>
                </td>
                <td style="color: #0000FF; text-align: left; width: 84px; height: 17px;"></td>
                <td style="text-align: right; height: 17px;">PHR编号：</td>
                <td style="text-align: left; height: 17px;">
                    <asp:Literal ID="LiteralPhrNumber" runat="server" Text="PhrNumber"></asp:Literal>
                </td>
                <td style="color: #0000FF; text-align: left; height: 17px;"></td>
                <td style="text-align: left; height: 17px;"></td>
            </tr>
            <tr>
                <td style="text-align: right; width: 119px;">姓名：</td>
                <td style="text-align: left; margin-left: 40px;">
                    <asp:TextBox ID="TextBoxName" runat="server" Width="102px"></asp:TextBox>
                </td>
                <td style="text-align: right">性别：</td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBoxSex" runat="server" Width="41px"></asp:TextBox>
                </td>
                <td style="text-align: right">出生日期：</td>
                <td style="text-align: left; width: 84px;">
                    <asp:TextBox ID="TextBoxDOB" runat="server" Width="114px"></asp:TextBox>
                </td>
                <td style="text-align: right">职业：</td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBoxOccupation" runat="server" Width="131px"></asp:TextBox>
                </td>
                <td style="text-align: left">
                    &nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: right; width: 119px;">&nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
                <td style="text-align: right">&nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
                <td style="text-align: right">&nbsp;</td>
                <td style="text-align: center; width: 84px;">
                    &nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
                <td style="text-align: left">
                    <asp:Button ID="ButtonSaveBasicalInfo" runat="server" OnClick="ButtonSaveBasicalInfo_Click" Text="保存基本信息" Width="140px" />
                </td>
                <td style="text-align: center">
                    &nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; height: 17px; color: #0000FF; width: 119px;">过敏史：</td>
                <td style="color: #0000FF; text-align: left; height: 17px;" colspan="8"></td>
                <td style="text-align: left; height: 17px;"></td>
            </tr>
            <tr>
                <td style="text-align: left; width: 119px;">&nbsp;</td>
                <td style="text-align: center" colspan="8">添加过敏史的相关内容</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; width: 119px;">家族史：</td>
                <td style="color: #0000FF; text-align: left;" colspan="8">&nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 119px;">&nbsp;</td>
                <td style="text-align: center" colspan="8">添加家族史的相关内容</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; width: 119px;">手术史：</td>
                <td style="color: #0000FF; text-align: left; height: 17px;" colspan="8">&nbsp;</td>
                <td style="text-align: left; height: 17px;"></td>
            </tr>
            <tr>
                <td style="text-align: left; width: 119px;">&nbsp;</td>
                <td style="text-align: center" colspan="8">添加手术史的相关内容</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; width: 119px;">生育史：</td>
                <td style="color: #0000FF; text-align: left;" colspan="8">&nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; height: 17px; width: 119px;"></td>
                <td style="text-align: center; height: 17px;" colspan="8">添加生育史的相关内容</td>
                <td style="text-align: left; height: 17px;"></td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; width: 119px;">个人史：</td>
                <td style="color: #0000FF; text-align: left;" colspan="8">&nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; height: 17px; width: 119px;"></td>
                <td style="text-align: center" colspan="8">添加个人史的相关内容</td>
                <td style="text-align: left; height: 17px;"></td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; width: 119px;">主要生理参数监测：</td>
                <td style="color: #0000FF; text-align: left;" colspan="8">&nbsp;</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 119px;">&nbsp;</td>
                <td style="text-align: center" colspan="8">添加个人史的相关内容</td>
                <td style="text-align: left">&nbsp;</td>
            </tr>
        </table>
</p>
<p>
        This is the page for creating personal health record.</p>
</asp:Content>
