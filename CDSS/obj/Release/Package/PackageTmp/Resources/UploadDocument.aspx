<%@ Page Title="编程文档" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="UploadDocument.aspx.cs" Inherits="CDSS.UploadDocument" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">
  
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left; height: 47px;">上载文档名称：<br />
                    <asp:FileUpload ID="FileUploadDoc" runat="server" Width="462px" height="32px" />
                    
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                    <asp:Button ID="ButtonUploadDoc" runat="server"  Width="105px" height="36px" OnClick=ButtonUploadDoc_Click Text="上载编程文档" />
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left">完成情况概述：<br />
                    <textarea id="TextAreaDocStatus" name="S1" runat="server" rows="2" style="height:40px ;Width:595px"></textarea>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    &nbsp;&nbsp;&nbsp;
                   
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left; height: 19px;">上载文档列表：<br />
                    <asp:SqlDataSource ID="SqlDataSourceGetDocList" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [UploadTime], [UploaderName], [FileName], [ResultStatus] FROM [WebpageResults] ORDER BY [UploadTime] DESC"></asp:SqlDataSource>
                    <asp:GridView ID="GridViewDocList" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceGetDocList" ForeColor="#333333" GridLines="None" Width="598px" AllowPaging="True">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="UploadTime" HeaderText="UploadTime" SortExpression="UploadTime" />
                            <asp:BoundField DataField="UploaderName" HeaderText="UploaderName" SortExpression="UploaderName" />
                            <asp:BoundField DataField="FileName" HeaderText="FileName" SortExpression="FileName" />
                            <asp:BoundField DataField="ResultStatus" HeaderText="ResultStatus" SortExpression="ResultStatus" />
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
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
   
    </asp:Content>
