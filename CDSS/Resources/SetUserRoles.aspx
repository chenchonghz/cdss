<%@ Page Title="" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="SetUserRoles.aspx.cs" Inherits="CDSS.SetUserRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">
    <table style="width:100%;">
    <tr>
        <td>&nbsp;</td>
        <td colspan="2">
            <asp:RadioButtonList ID="RadioButtonListUserType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonListUserType_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0" Selected="True">普通用户</asp:ListItem>
                <asp:ListItem Value="1">医生用户</asp:ListItem>
                <asp:ListItem Value="2">开发人员</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td style="text-align: right">
            <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                <asp:View ID="View11" runat="server">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" AllowPaging="True" DataKeyNames="CommonUserUID" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">

                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="RegisterTime" HeaderText="注册时间" SortExpression="RegisterTime" />
                            <asp:BoundField DataField="LoginName" HeaderText="注册名" SortExpression="LoginName" />
                            <asp:BoundField DataField="Name" HeaderText="真实姓名" SortExpression="Name" />
                            <asp:BoundField DataField="Sex" HeaderText="性别" SortExpression="Sex" />
                            <asp:BoundField DataField="Occupation" HeaderText="职业" SortExpression="Occupation" />
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [LoginName], [RegisterTime], [Name], [Sex], [Occupation], [CommonUserUID] FROM [CommonUserInfo]"></asp:SqlDataSource>
                </asp:View>
                <asp:View ID="View12" runat="server">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource2" ForeColor="#333333" GridLines="None" AllowPaging="True" DataKeyNames="DoctorUID" OnSelectedIndexChanged="GridView2_SelectedIndexChanged">

                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="RegisterTime" HeaderText="注册时间" SortExpression="RegisterTime" />
                            <asp:BoundField DataField="LoginName" HeaderText="注册名" SortExpression="LoginName" />
                            <asp:BoundField DataField="Name" HeaderText="真实姓名" SortExpression="Name" />
                            <asp:BoundField DataField="Sex" HeaderText="性别" SortExpression="Sex" />
                            <asp:BoundField DataField="Occupation" HeaderText="职业" SortExpression="Occupation" />
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
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [LoginName], [Sex], [Occupation], [Name], [RegisterTime], [DoctorUID] FROM [DoctorInfo]"></asp:SqlDataSource>
                </asp:View>
                <asp:View ID="View13" runat="server">
                    <asp:GridView ID="GridView3" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource3" ForeColor="#333333" GridLines="None" AllowPaging="True" DataKeyNames="DeveloperUID" OnSelectedIndexChanged="GridView3_SelectedIndexChanged">

                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="RegisterTime" HeaderText="注册时间" SortExpression="RegisterTime" />
                            <asp:BoundField DataField="LoginName" HeaderText="注册名" SortExpression="LoginName" />
                            <asp:BoundField DataField="Name" HeaderText="真实姓名" SortExpression="Name" />
                            <asp:BoundField DataField="Sex" HeaderText="性别" SortExpression="Sex" />
                            <asp:BoundField DataField="Occupation" HeaderText="职业" SortExpression="Occupation" />
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
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [LoginName], [Sex], [Occupation], [Name], [RegisterTime], [DeveloperUID] FROM [DeveloperInfo]"></asp:SqlDataSource>
                </asp:View>
            </asp:MultiView>
        </td>
        <td style="text-align: left">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0">健康咨询</asp:ListItem>
                        <asp:ListItem Value="1">医疗咨询</asp:ListItem>
                    </asp:CheckBoxList>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:CheckBoxList ID="CheckBoxList2" runat="server">
                        <asp:ListItem Value="0">修改个人信息</asp:ListItem>
                        <asp:ListItem Value="1">临床决策支持</asp:ListItem>
                        <asp:ListItem Value="2">个性化知识库</asp:ListItem>
                        <asp:ListItem Value="3">网络家庭医生</asp:ListItem>
                        <asp:ListItem Value="4">网络会诊转诊</asp:ListItem>
                    </asp:CheckBoxList>
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <asp:CheckBoxList ID="CheckBoxList3" runat="server">
                        <asp:ListItem Value="0">上载资源文档</asp:ListItem>
                        <asp:ListItem Value="1">上载编程文档</asp:ListItem>
                        <asp:ListItem Value="2">用户权限设置</asp:ListItem>
                        <asp:ListItem Value="3">权限设置之四</asp:ListItem>
                        
                    </asp:CheckBoxList>
                </asp:View>
            </asp:MultiView>
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td colspan="2" style="text-align: center">
            <asp:Button ID="ButtonSaveSetting" runat="server" Text="确定选定用户的权限设置" Width="290px" OnClick="ButtonSaveSetting_Click" />
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
</asp:Content>
