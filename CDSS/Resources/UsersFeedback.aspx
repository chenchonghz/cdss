<%@ Page Title="用户反馈" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="UsersFeedback.aspx.cs" Inherits="CDSS.UsersFeedback" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">

    <p style="text-align: left">
        <table style="width:100%;">
            <tr>
                <td style="text-align: left; height: 47px;">用户建议标题：<asp:TextBox ID="TextBoxFeedTitle" runat="server" Width="482px"></asp:TextBox>
                    <asp:Button ID="ButtonUploadFeed" runat="server" Text="提交您的建议" Width="112px" OnClick="ButtonUploadFeed_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left">详细建议内容：<textarea id="TextAreaFeedContent"  runat="server" name="S1"></textarea></td>
            </tr>
            <tr>
                <td style="text-align: left; height: 47px;">已经提交的建议列表：</td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewFeedList" runat="server" AutoGenerateColumns="False" CellPadding="4"  ForeColor="#333333" GridLines="None" Height="16px" ShowHeaderWhenEmpty="True" Width="691px">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="ProviderTime" HeaderText="建议提交时间" SortExpression="ProviderTime" />
                            <asp:BoundField DataField="ProviderName" HeaderText="提交者姓名" SortExpression="ProviderName" />
                            <asp:BoundField DataField="FeedTitle" HeaderText="提交意见标题" SortExpression="FeedTitle" />
                             <asp:BoundField DataField="FeedContent" HeaderText="提交意见内容" SortExpression="FeedContent" />
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
                    <asp:SqlDataSource ID="SqlDataSourceFeedList" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [FeedTitle], [ProviderName], [ProviderTime] FROM [UsersFeedback]"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </p>
    <p style="text-align: left">本页面需要完成的工作：</p>
    <p style="text-align: left">1、实现“提交用户建议”的功能并显示于下方的表格中；</p>
    <p style="text-align: left">2、提交之前必要的数据验证（标题和内容都不应为空）；</p>
    <p style="text-align: left">3、如果熟悉页面布局，请帮助美化一下页面的布局。</p>
</asp:Content>
