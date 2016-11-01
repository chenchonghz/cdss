<%@ Page Title="开发人员个人信息" Language="C#" MasterPageFile="~/Developer.master" AutoEventWireup="true" CodeBehind="DevelopersInfo.aspx.cs" Inherits="CDSS.DevelopersInfo" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDeveloper" runat="server">
    <p>
        <table style="width:100%; text-align: center;">
            <tr>
                <td style="text-align: left">&nbsp;登 录 名： <asp:TextBox ID="TextBoxLoginName" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelLoginName" runat="server" Text="*无需修改可以不填" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
                <td>
                </td>
                <td rowspan="6">
                    <asp:Image ID="ImagePhoto" runat="server" Height="205px" Width="160px"  ImageUrl="D:\QQMiniDL\79931944\ProjCDSS20141026-1810\ProjCDSS\CDSS\UserPhoto\test.jpg"/>
                    <br />
                    <asp:FileUpload ID="FileUpload" runat="server" Width="213px" />
                    <br />
                    <asp:Label ID="Upload_Info" runat="server" CssClass="message-error" BorderStyle="None" Font-Size="Smaller" Width="209px" Text="点击'选择文件'选择上传头像,然后点击'确认上传'"></asp:Label>
                    <br />
                    <asp:Button ID="ButtonImportPhoto" runat="server" Text="确认上传" OnClick="ButtonImportPhoto_Click" Width="114px" />
                    <br />
                </td>
            </tr>
            <tr>
            <td style="text-align: left">真实姓名：<asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox>
                <asp:Label ID="LabelName" runat="server" Text="*必须填写真实姓名" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
            </tr>


            <tr>
                <td style="text-align: left">性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 别：<asp:TextBox ID="TextBoxSex" runat="server" ></asp:TextBox>
                    <asp:Label ID="LabelSex" runat="server" Text="*必填,男/女" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">出生日期：<asp:TextBox ID="TextBoxDOB" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelDOB" runat="server"  Text="*格式：1991-01-01" CssClass="message-error" BorderStyle="None" Font-Size="Smaller" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">电子邮箱：<asp:TextBox ID="TextBoxEmailAddress" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelEmailAddress" runat="server" Text="*必须填写有效的邮箱" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
            </tr>
            <tr>
               <td style="text-align: left">移动电话：<asp:TextBox ID="TextBoxMobilePhone" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelMobilePhone" runat="server" Text="*选填" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
            </tr>
            <tr> <td style="text-align: left">固定电话：<asp:TextBox ID="TextBoxTelephone" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelTelephone" runat="server" Text="*选填" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
                
                <td>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left">职&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 业：<asp:TextBox ID="TextBoxOccupation" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelOccupation" runat="server" Text="*选填" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
                <td>
                </td>
                <td style="text-align: center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left">技术职称：<asp:TextBox ID="TextBoxAcademicTitle" runat="server"></asp:TextBox>
                    <asp:Label ID="LabelAcademicTitle" runat="server" Text="*选填" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td>
                <td style="text-align: left">&nbsp;</td>
                <td>
                </td>
                
            </tr>
            <tr>
                <td colspan="2" style="text-align: left">工作单位：<asp:TextBox ID="TextBoxWorkingUnit" runat="server" Width="427px"></asp:TextBox>
                    <asp:Label ID="LabelWorkingUnit" runat="server" Text="*选填" CssClass="message-error" BorderStyle="None" Font-Size="Smaller"></asp:Label>
                </td> 
                <td>
                    <asp:Button ID="ButtonSaveDeveloperInfo" runat="server" Text="*保存个人信息修改" OnClick="ButtonSaveDeveloperInfo_Click" />
                </td>
            </tr>
           
        </table>
    </p>
    <p style="text-align: left">
        本页面需要完成的工作：</p>
    <p style="text-align: left">
        1、完成开发者个人信息的修改与保存（当前开发者为当前登录的用户）；</p>
    <p style="text-align: left">
        2、完成开发者信息每一项信息的必要验证；</p>
    <p style="text-align: left">
        3、完成照片图片从本地以及从数据库的读取与显示。</p>
</asp:Content>
