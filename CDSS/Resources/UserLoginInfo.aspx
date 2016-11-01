<%@ Page Title="用户登录信息" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="UserLoginInfo.aspx.cs" Inherits="CDSS.UserLoginInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">
    <table style="width:100%;">
        <tr>
            <td>&nbsp;</td>
            <td>用户登录信息列表</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceUserLoginInfo" ForeColor="#333333" GridLines="None" Width="815px" PageSize="30">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="LoginTime" HeaderText="LoginTime" SortExpression="LoginTime" />
                        <asp:BoundField DataField="LoginName" HeaderText="LoginName" SortExpression="LoginName" />
                        <asp:BoundField DataField="UserType" HeaderText="UserType" SortExpression="UserType" />
                        <asp:BoundField DataField="LoginIP" HeaderText="LoginIP" SortExpression="LoginIP" />
                        <asp:BoundField DataField="LogoutTime" HeaderText="LogoutTime" SortExpression="LogoutTime" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSourceUserLoginInfo" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [LoginTime], [LoginName], [UserType], [LoginIP], [LogoutTime] FROM [UsersLogin] WHERE ([LoginName] <>'Test') ORDER BY [LoginTime] DESC">
                </asp:SqlDataSource>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
