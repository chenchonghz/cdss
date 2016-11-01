<%@ Page Title="登录" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CDSS.Account.Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <section id="loginForm">
        <h2>使用本地帐户登录。</h2>
        <asp:Login runat="server" ViewStateMode="Disabled" RenderOuterTable="false" ID="LoginMain"  > 
            <LayoutTemplate>
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <legend>登录表单</legend>
                    <table text-align: center >
                        <tr>
                            <td class="auto-style4" style="text-align: right;"><asp:Label runat="server" AssociatedControlID="UserChoose" Width="128px" >用户类型：</asp:Label></td>
                            <td class="auto-style3" style="padding-left:5%;">
                                <asp:RadioButtonList ID="UserChoose" runat="server" RepeatDirection="Horizontal" CssClass="auto-style7" Width="260px" >
                                    <asp:ListItem Selected="True">普通用户</asp:ListItem>
                                    <asp:ListItem>医生</asp:ListItem>
                                    <asp:ListItem>开发者</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td class="auto-style2"></td>

                        </tr>
                        <tr>
                            <td class="auto-style4" style="text-align: right;"><asp:Label runat="server" AssociatedControlID="UserName" Width="128px">用户名：</asp:Label></td>
                           <td class="auto-style3" style="padding-left:0px;"><asp:TextBox runat="server" ID="UserName" Width="198px" /></td>
                            <td class="auto-style2"><asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="用户名字段是必填字段。" Font-Size="Smaller" /></td>
                        </tr>
                        <tr>
                            <td class="auto-style4" style="text-align: right;"><asp:Label runat="server" AssociatedControlID="Password" Width="128px" >密码：</asp:Label></td>
                            <td class="auto-style3" style="padding-left:0px;"><asp:TextBox runat="server" ID="Password" TextMode="Password" Width="198px" OnTextChanged="Password_TextChanged" /></td>
                            <td class="auto-style2"><asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="密码字段是必填字段。" Font-Size="Smaller" /></td>
                        </tr>
                        <tr>                         
                            <td class="auto-style4" style="text-align: right">&nbsp;</td>
                            <td class="auto-style3">
                                <asp:CheckBox ID="RememberMe" runat="server" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">记住我?</asp:Label></td>
                            <td class="auto-style2"></td>
                        </tr>
                        <tr>
                            <td class="auto-style4" style="text-align: right">&nbsp;</td>
                            <td class="auto-style3">
                                <asp:Button runat="server" CommandName="Login" Text="登录" ID="ButtonLogin" OnClick="ButtonLogin_Click" />
                            </td>
                            <td class="auto-style2"></td>
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

    </asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style2 {
            width: 344px;
        }
        .auto-style3 {
            width: 319px;
        }
        .auto-style4 {
            width: 202px;
        }
    </style>
</asp:Content>


