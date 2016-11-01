<%@ Page Title="资源文档" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="UploadResource.aspx.cs" Inherits="CDSS.UploadResource" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">
    
        <table style="width:100%;">
            <tr>
                <td style="text-align: left">资源文件标题：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="TextBoxResTitle" runat="server" Width="919px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">资源文件介绍：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="TextBoxResIntro" runat="server" TextMode="MultiLine" Width="920px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; height: 47px;">上载资源文件（小于4M)：<asp:FileUpload ID="FileUploadRes"   runat="server" Width="749px" />
                    <asp:Button ID="ButtonUploadRes" runat="server" Text="完成上载并保存" Width="186px" OnClick="ButtonUploadRes_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; height: 19px;">已经上载资源列表：</td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <asp:GridView ID="GridViewResList" runat="server" AutoGenerateColumns="False"  Height="16px" ShowHeaderWhenEmpty="True" Width="1086px" AllowPaging="True" AllowSorting="True" OnSelectedIndexChanged="GridViewResList_SelectedIndexChanged" PageSize="5" DataSourceID="SqlDataSourceResList" ForeColor="#333333" GridLines="None" CellPadding="4">
                        <AlternatingRowStyle BackColor="White" BorderStyle="Inset" ForeColor="#284775" />
                        <Columns>
                            
                            <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" />
                            
                            <asp:BoundField DataField="ResTitle" HeaderText="资源标题" SortExpression="ResTitle" />
                            <asp:BoundField DataField="SavedFilename" HeaderText="文件名" SortExpression="SavedFilename" />
                            <asp:BoundField DataField="ProvideTime" HeaderText="提供时间" SortExpression="ProvideTime" />
                            <asp:BoundField DataField="SavedPath" HeaderText="保存路径" SortExpression="SavedPath" />
                           
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" BorderStyle="Dashed" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerSettings FirstPageText="第一页" LastPageText="最后一页" NextPageText="下一页" PreviousPageText="上一页" />
                        <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#284775" BorderStyle="Dotted" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSourceResList" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" 
                         SelectCommand="SELECT [ResTitle], [SavedFilename], [ProvideTime], [SavedPath] FROM [GroupResources]"
                          DeleteCommand ="DELETE FROM [GroupResources] WHERE  [ResTitle]=@ResTitle" CacheExpirationPolicy="Sliding">
                          
                      
                         <DeleteParameters>
                             
                             <asp:Parameter Name="ResTitle" Type="String" />
                             <asp:Parameter Name="original_ResTitle" Type="Char" />
                            
                             <asp:Parameter Name="original_SavedPath" Type="String" />
                             <asp:Parameter Name="original_SavedFilename" Type="String" />
                             <asp:Parameter Name="original_ProvideTime" Type="DateTime" />
                         </DeleteParameters>
                     
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    
    </asp:Content>
