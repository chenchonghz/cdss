<%@ Page Title="登录" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoginOld.aspx.cs" Inherits="CDSS.Account.Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>
    <section id="loginForm">
        <h2>使用本地帐户登录。</h2>
        <asp:Login runat="server" ViewStateMode="Disabled" RenderOuterTable="false" ID="LoginMain" OnLoginError="LoginMain_LoginError"  > 
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <legend>登录表单</legend>
                    <table text-align: center >
                        <tr>
                            <td class="auto-style35" style="text-align: right;"><asp:Label runat="server" AssociatedControlID="UserChoose" Width="128px" >用户类型：</asp:Label></td>
                            <td class="auto-style36" style="padding-left:5%;">
                                <asp:RadioButtonList ID="UserChoose" runat="server" RepeatDirection="Horizontal" CssClass="auto-style7" Width="260px" >
                                    <asp:ListItem Selected="True">普通用户</asp:ListItem>
                                    <asp:ListItem>医生</asp:ListItem>
                                    <asp:ListItem>开发者</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td class="auto-style37"></td>

                        </tr>
                        <tr>
                            <td class="auto-style33" style="text-align: right;"><asp:Label runat="server" AssociatedControlID="UserName" Width="128px">用户名：</asp:Label></td>
                           <td class="auto-style34" style="padding-left:0px;"><asp:TextBox runat="server" ID="UserName" Width="198px" /></td>
                            <td class="auto-style11"><asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="用户名字段是必填字段。" Font-Size="Smaller" /></td>
                        </tr>
                        <tr>
                            <td class="auto-style45" style="text-align: right;"><asp:Label runat="server" AssociatedControlID="Password" Width="128px" >密码：</asp:Label></td>
                            <td class="auto-style47" style="padding-left:0px;"><asp:TextBox runat="server" ID="Password" TextMode="Password" Width="198px" OnTextChanged="Password_TextChanged" /></td>
                            <td class="auto-style42"><asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="密码字段是必填字段。" Font-Size="Smaller" /></td>
                        </tr>
                        <tr>                         
                            <td class="auto-style45" style="text-align: right">&nbsp;</td>
                            <td class="auto-style47">
                                <asp:CheckBox ID="RememberMe" runat="server" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">记住我?</asp:Label></td>
                            <td class="auto-style42"></td>
                        </tr>
                        <tr>
                            <td class="auto-style45" style="text-align: right">&nbsp;</td>
                            <td class="auto-style47">
                                <asp:Button runat="server" CommandName="Login" Text="登录" ID="ButtonLogin" OnClick="ButtonLogin_Click" />
                            </td>
                            <td class="auto-style42"></td>
                        </tr>
                  </table>
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
        <p>
            <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">注册</asp:HyperLink>
            如果你没有帐户。
        </p>
    </section>
    <section id="socialLoginForm">
        <h2>使用其他服务登录。</h2>
        <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
    </section>

</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style7 {
        }
        .auto-style11 {
            width: 172px;
            height: 62px;
        }
        .auto-style33 {
            width: 155px;
            height: 24px;
            left: 100px;
        }
        .auto-style34 {
            width: 215px;
            height: 62px;
        }
        .auto-style35 {
            width: 155px;
            height: 66px;
        }
        .auto-style36 {
            width: 215px;
            height: 66px;
        }
        .auto-style37 {
            width: 172px;
            height: 66px;
        }
        .auto-style42 {
            width: 172px;
        }
        .auto-style45 {
            width: 155px;
        }
        .auto-style46 {
            width: 249px;
        }
        .auto-style47 {
            width: 215px;
        }
    </style>
</asp:Content>

