<%@ Page Title="" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="FunctionSetting.aspx.cs" Inherits="CDSS.FunctionSetting" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">
    <table style="width:100%;">
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td style="vertical-align: top; text-align: left">函数类型：&nbsp;<br />
&nbsp;<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="FunctionKind" DataValueField="FunctionKind" Height="23px" Width="180px">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT DISTINCT [FunctionKind] FROM [FunctionSetting]"></asp:SqlDataSource>
            <br />
            函数名称：<br />
            <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="FunctionTitle" DataValueField="FunctionTitle" Height="249px" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" Width="185px"></asp:ListBox>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT DISTINCT [FunctionTitle] FROM [FunctionSetting] WHERE ([FunctionKind] = @FunctionKind)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList1" DefaultValue="发病率与年龄关系" Name="FunctionKind" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
        <td>
            <asp:Chart ID="Chart1" runat="server" style="margin-left: 0px" Width="614px" meta:resourcekey="Chart1Resource1" OnClick="Chart1_Click" OnDataBinding="Chart1_DataBinding" OnPrePaint="Chart1_PrePaint">
                <Series>
                    <asp:Series ChartType="Spline" Name="Series1">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
            <br />
            <br />
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>
            &nbsp;</td>
        <td>&nbsp;</td>
    </tr>
</table>
</asp:Content>
