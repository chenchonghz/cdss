<%@ Page Title="体温数据" Language="C#" MasterPageFile="~/Doctor.master" AutoEventWireup="true" CodeBehind="Webshow.aspx.cs" Inherits="CDSS.Webshow" %><%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDoctor" runat="server">
    <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSourcePhysicalData" Height="343px" style="margin-right: 0px" Width="855px">
        <series>
            <asp:Series ChartType="Line" Name="体温" XValueMember="AcquisitionTime" YValueMembers="PhysicalDataValue">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1">
            </asp:ChartArea>
        </chartareas>
        <Legends> 
        <asp:Legend Alignment="Center" Docking="Right" Name="Legend1" Title="图例"> 
        </asp:Legend> 
        </Legends>
         <Titles> 
        <asp:Title Font="微软雅黑, 16pt" Name="Title1" Text="近一个月的体温情况">

        </asp:Title> </Titles>
    </asp:Chart>
    <br />
    <br />
    <p>
        编程任务：熟悉一下数据图表控件(Chart)相关的属性和控制,看看如何给这个图表增加必要的标题，图例有关信息，看看能够对Chart能够进行那些交互式操作。你可以在任何一个页面上增加类似如下所示的内容以便调用，调试这个页面。
        查看<a href="/Doctors/DisplayPhysicalData">体温图表</a>。
    </p>
    <asp:SqlDataSource ID="SqlDataSourcePhysicalData" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [AcquisitionTime], [PhysicalDataValue] FROM [PhysicalData] "></asp:SqlDataSource>
</asp:Content>
