<%@ Page Title="注册" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Register.aspx.cs" Inherits="CDSS.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h2>使用以下表单创建新帐户。<br />
        </h2>
    </hgroup>


    <contenttemplate>
                    <p class="message-info">
                        密码必须至少包含6个字符。
                    </p>

                    <p class="validation-summary-errors">
                        <asp:Literal runat="server" ID="ErrorMessage" />
                    </p>
                    <fieldset>
                        <legend>注册表单</legend>
                        <table text-align: center>
                            <tr>
                                <td class="auto-style2" style="text-align: right"><asp:Label runat="server" AssociatedControlID="UserName">用户名：</asp:Label></td>
                                <td class="auto-style4" style="text-align: left"><asp:TextBox runat="server" ID="UserName" />
                                    <br />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                    CssClass="field-validation-error" ErrorMessage="用户名字段是必填字段。" ID="RequiredFieldValidator1" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style3" style="text-align: right"><asp:Label runat="server" AssociatedControlID="Email">电子邮件地址：</asp:Label></td>
                                <td class="auto-style5" style="text-align: left"><asp:TextBox runat="server" ID="Email" />
                                    <br />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"                               
                                    CssClass="field-validation-error" ErrorMessage="电子邮件地址字段是必需的。" ID="RequiredFieldValidator2" /></td>
                               <td class="auto-style1">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2" style="text-align: right"><asp:Label runat="server" AssociatedControlID="Password">密码：</asp:Label></td>
                                <td class="auto-style4" style="text-align: left"><asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                    <br />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                    CssClass="field-validation-error" ErrorMessage="密码字段是必填字段。" ID="RequiredFieldValidator3" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                               <td class="auto-style2" style="text-align: right"> <asp:Label runat="server" AssociatedControlID="ConfirmPassword">确认密码：</asp:Label></td>
                                <td class="auto-style4" style="text-align: left"><asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />
                                    <br />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                     CssClass="field-validation-error" Display="Dynamic" ErrorMessage="“确认密码”字段是必填字段。" ID="RequiredFieldValidator4" />
                                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                     CssClass="field-validation-error" Display="Dynamic" ErrorMessage="密码和确认密码不匹配。" ID="CompareValidator1" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style2" style="text-align: right"><asp:Label runat="server" Font-Bold="True" Font-Size="Medium">用户类型：</asp:Label></td>
                                    <td class="auto-style4" style="text-align: left">
                                        <asp:RadioButtonList ID="RadioButtonListUserChoose" runat="server" CellPadding="1" Height="86px" RepeatDirection="Horizontal" Width="404px">                                            
                                            <asp:ListItem Text="CommonUser" Value="CommonUser" Selected="True" >普通用户</asp:ListItem>
                                            <asp:ListItem Text="Doctor" Value="Doctor">医生</asp:ListItem>
                                            <asp:ListItem Text="Developer" Value="Developer">开发者</asp:ListItem>                                        
                                        </asp:RadioButtonList>
                                    </td>                             
                            </tr>
                        <tr>
                        <td class="auto-style2" style="text-align: right"></td>
                        <td style="text-align:left" class="auto-style4"><asp:Button runat="server" CommandName="MoveNext" Text="注册" Width="62px" OnClick="RegisterUser_CreatedUser" /></td>
                        </tr>
                        </table>
                        <br />
                    </fieldset>
                </contenttemplate>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style1 {
            height: 45px;
        }
        .auto-style2 {
            width: 438px;
        }
        .auto-style3 {
            height: 45px;
            width: 438px;
        }
        .auto-style4 {
            width: 402px;
        }
        .auto-style5 {
            height: 45px;
            width: 402px;
        }
    </style>
</asp:Content>
