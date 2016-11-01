<%@ Page Title="开发人员个人信息" Language="C#" MasterPageFile="~/Developer.master" AutoEventWireup="true" CodeBehind="DevelopersInfo.aspx.cs" Inherits="CDSS.DevelopersInfo" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDeveloper" runat="server">
    
        <table style="width:100%; text-align: center;">
            <tr>
                <td style="text-align: left; width: 528px;">&nbsp;登 录 名： <asp:TextBox ID="TextBoxLoginName" runat="server" ></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text="*必填" Display="Dynamic"  Font-Size="Smaller" CssClass="label"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*登录名不能为空" ControlToValidate="TextBoxLoginName" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ></asp:RequiredFieldValidator>                   
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*必须输入字母或者数字" ControlToValidate="TextBoxLoginName" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="^[A-Za-z0-9]+$" ></asp:RegularExpressionValidator>
                    
                </td>
                <td style="width: 13px">                    
                    &nbsp;</td>
                <td rowspan="8">
                    <asp:Image ID="ImagePhoto" runat="server" Height="205px" Width="160px"  ImageUrl="D:\QQMiniDL\79931944\ProjCDSS20141026-1810\ProjCDSS\CDSS\UserPhoto\test.jpg"/>
                    <br />
                    <asp:FileUpload ID="FileUpload" runat="server" Width="213px" />
                    <br />
                    <asp:Label ID="Upload_Info" runat="server" CssClass="message-error" BorderStyle="None" Font-Size="Smaller" Width="209px" Text="点击'浏览'选择头像,然后点击'图片上载'"></asp:Label>
                    <br />
                    <asp:Button ID="ButtonImportPhoto" runat="server" Text="图片上载" OnClick="ButtonImportPhoto_Click" Width="114px" CausesValidation="False" ValidationGroup="2" />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 528px;">真实姓名：<asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="*必填" Display="Dynamic"  Font-Size="Smaller" CssClass="label"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*真实姓名不能为空" CssClass="message-error" ControlToValidate="TextBoxName" Display="Dynamic" Font-Size="Smaller"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"  ErrorMessage="*必须输入汉字" ControlToValidate="TextBoxName" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="^[\u4e00-\u9fa5]+$" ></asp:RegularExpressionValidator>
                    
                </td>
                <td style="width: 13px">                    
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 528px;">性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 别：<asp:TextBox ID="TextBoxSex" runat="server" ></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" Text="*必填,男/女" Display="Dynamic"  Font-Size="Smaller" CssClass="label"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*性别不能为空" CssClass="message-error" ControlToValidate="TextBoxSex" Display="Dynamic" Font-Size="Smaller"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"  ErrorMessage="*输入错误" ControlToValidate="TextBoxSex" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="[\u7537\u5973]" ></asp:RegularExpressionValidator>
                </td>

                <td style="width: 13px"></td>
            </tr>


            <tr>
                <td style="text-align: left; width: 528px;">出生日期：<asp:TextBox ID="TextBoxDOB" runat="server"></asp:TextBox>
                    <asp:Label ID="Label4" runat="server" Text="*必填,例1991-01-01" Display="Dynamic"  Font-Size="Smaller" CssClass="label"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*出生日期不能为空" CssClass="message-error" ControlToValidate="TextBoxDOB" Display="Dynamic" Font-Size="Smaller"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"  ErrorMessage="*格式错误" ControlToValidate="TextBoxDOB" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="[\d]{4}[-\ ][\d]{1,2}[-\ ][\d]{1,2}" ></asp:RegularExpressionValidator>
                </td>

                <td style="width: 13px">&nbsp;</td>
            </tr>


            <tr>
                <td style="text-align: left; width: 528px;">电子邮箱：<asp:TextBox ID="TextBoxEmailAddress" runat="server"></asp:TextBox><asp:Label ID="Label5" runat="server" Text="*必填" Display="Dynamic"  Font-Size="Smaller" CssClass="label"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*电子邮箱不能为空" CssClass="message-error" ControlToValidate="TextBoxEmailAddress" Display="Dynamic" Font-Size="Smaller"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"  ErrorMessage="*电子邮箱输入格式错误" ControlToValidate="TextBoxEmailAddress" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ></asp:RegularExpressionValidator>
                </td>

                 <td style="width: 13px"></td>
            </tr>


            <tr>
                <td style="text-align: left; width: 528px;">移动电话：<asp:TextBox ID="TextBoxMobilePhone" runat="server"></asp:TextBox><asp:Label ID="Label6" runat="server" Text="*必填" Display="Dynamic"  Font-Size="Smaller" CssClass="label"></asp:Label>
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"  ErrorMessage="*移动电话号码输入格式错误" ControlToValidate="TextBoxMobilePhone" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="^[1]+[3,5]+\d{9}" ></asp:RegularExpressionValidator>
                </td>

                 <td style="width: 13px">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 528px;">固定电话：<asp:TextBox ID="TextBoxTelephone" runat="server"></asp:TextBox>
                </td>
                 <td style="width: 13px"></td>
            </tr>
            <tr>
                <td style="text-align: left; width: 528px;">职&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 业：<asp:TextBox ID="TextBoxOccupation" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"  ErrorMessage="*必须输入汉字" ControlToValidate="TextBoxOccupation" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="^[\u4e00-\u9fa5]+$" ></asp:RegularExpressionValidator>
                </td>
                 <td style="width: 13px">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 528px;">QQ&nbsp; 号码：<asp:TextBox ID="TextBoxQqNumber" runat="server" Width="158px"></asp:TextBox>
                </td>
                 <td style="width: 13px"></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 528px;">技术职称：<asp:TextBox ID="TextBoxAcademicTitle" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server"  ErrorMessage="*必须输入汉字" ControlToValidate="TextBoxAcademicTitle" Display="Dynamic" CssClass="message-error" Font-Size="Smaller" ValidationExpression="^[\u4e00-\u9fa5]+$" ></asp:RegularExpressionValidator>
                </td>
                 <td style="width: 13px">&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="1" style="text-align: left; width: 528px;">工作单位：<asp:TextBox ID="TextBoxWorkingUnit" runat="server" Width="418px"></asp:TextBox>
                    <br />
                </td> 
                 <td style="width: 13px"></td>
                <td>
                    <asp:Button ID="ButtonSaveDeveloperInfo" runat="server" Text="*保存个人信息修改" OnClick="ButtonSaveDeveloperInfo_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 528px">
                    &nbsp;</td>
                <td style="width: 13px"></td>
                <td></td>
            </tr>
           
        </table>
    
    </asp:Content>
