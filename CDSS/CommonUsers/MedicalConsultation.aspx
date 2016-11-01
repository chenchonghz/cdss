<%@ Page Title="" Language="C#" MasterPageFile="~/CommonUser.master" AutoEventWireup="true" CodeBehind="MedicalConsultation.aspx.cs" Inherits="CDSS.MedicalConsultation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderCommonUser" runat="server">
        <table  style="width:100%; height: 555px;" border="1">
            <tr>
                <th style="width: 181px; height: 13px; text-align: center;">我的基本资料</th>
                <td rowspan="3" style="text-align: left; width: 20px;">
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" Font-Size="XX-Small" Height="136px" RepeatLayout="Flow" Width="172px">
                        <asp:ListItem>选项之一</asp:ListItem>
                        <asp:ListItem>选项之二</asp:ListItem>
                        <asp:ListItem>选项之三</asp:ListItem>
                        <asp:ListItem>选项之四</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <th colspan="4" style="text-align: center">CDSS医疗诊断系统</th>
            </tr>
            <tr>
                <td rowspan="2" style="width: 181px; text-align: left;">登录名：<asp:Label ID="LabelMyLoginName" runat="server" Text="LabelMyLoginName" Font-Bold="True"></asp:Label>
                    <br />
                    姓名：<asp:Label ID="LabelMyName" runat="server" Text="LabelMyName" Font-Bold="True"></asp:Label>
                    <br />
                    性别：<asp:Label ID="LabelMySex" runat="server" Text="LabelMySex" Font-Bold="True"></asp:Label>
                    <br />
                    出生日期：<asp:Label ID="LabelMyBirthday" runat="server" Text="LabelMyBirthday" Font-Bold="True"></asp:Label>
                    <br />
                </td>
                <td style="width: 181px; height: 13px;">
                    <strong>
                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" Font-Bold="True">
                        <asp:ListItem>症状</asp:ListItem>
                        <asp:ListItem>体征</asp:ListItem>
                        <asp:ListItem>实验室检查</asp:ListItem>
                        <asp:ListItem>影像学检查</asp:ListItem>
                        <asp:ListItem>其他检查</asp:ListItem>
                    </asp:DropDownList>
                    </strong>
                </td>
                <td style="width: 181px; height: 13px;"><strong>特征</strong></td>
                <td style="height: 13px; width: 181px;"><strong>选项</strong></td>
                <td style="height: 13px; width: 181px;"><strong>诊断</strong></td>
                <tr>
                <td style="width: 181px">
                <asp:Panel ID="Panel1" runat="server" Height="220px" HorizontalAlign="Left" ScrollBars="Vertical" Width="180px">
                    <asp:BulletedList ID="BulletedList1" runat="server" DataSourceID="SqlDataSourceFindingBase" DataTextField="ChineseName" DataValueField="ChineseName" OnClick="BulletedList1_Click" style="margin-left: 0px" DisplayMode="LinkButton">
                    </asp:BulletedList>
                    <asp:SqlDataSource ID="SqlDataSourceFindingBase" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [ChineseName] FROM [FindingsBase] WHERE ([FindingType] = @FindingType) ORDER BY [ChineseName]">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="FindingType" PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
                </td>
                <td style="width: 181px">
                    <asp:Panel ID="Panel2" runat="server" Height="220px" HorizontalAlign="Left" ScrollBars="Vertical" Width="180px">
                        <asp:BulletedList ID="BulletedList2" runat="server" DisplayMode="LinkButton" style="margin-left: 0px" OnClick="BulletedList2_Click">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="width: 181px">
                    <asp:Panel ID="Panel3" runat="server" Height="220px" HorizontalAlign="Left" ScrollBars="Vertical" Width="180px">
                        <asp:BulletedList ID="BulletedList3" runat="server" DisplayMode="LinkButton" style="margin-left: 0px" OnClick="BulletedList3_Click">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="width: 181px">
                    <asp:Panel ID="Panel4" runat="server" Height="220px" Width="180px" HorizontalAlign="Left" ScrollBars="Vertical">
                        <asp:BulletedList ID="BulletedList4" runat="server" style="margin-left: 0px" DataSourceID="SqlDataSourceDiseaseBase" DataTextField="ChineseName" DataValueField="ChineseName">
                        </asp:BulletedList>
                        <asp:SqlDataSource ID="SqlDataSourceDiseaseBase" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [ChineseName] FROM [DiseasesBase] ORDER BY [ChineseName]"></asp:SqlDataSource>
                    </asp:Panel>
                </td>
            </tr>
            <tr><td colspan="6" style="height: 31px"></td></tr>
            <tr><th style="width: 181px; text-align: center;">医生列表</th><th colspan="2" style="text-align: center">医生信息</th><th colspan="3" style="text-align: center">对话窗口</th></tr>
            <tr><td rowspan="3" style="width: 181px">
                <asp:Panel ID="Panel5" runat="server" Height="250px" HorizontalAlign="Left" ScrollBars="Vertical" Width="180px">
                    <asp:BulletedList ID="BulletedList5" runat="server" DataSourceID="SqlDataSourceDoctorInfo" DataTextField="LoginName" DataValueField="LoginName" DisplayMode="LinkButton" style="margin-left: 0px" OnClick="BulletedList5_Click">
                    </asp:BulletedList>
                    <asp:SqlDataSource ID="SqlDataSourceDoctorInfo" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [LoginName] FROM [DoctorInfo] ORDER BY [LoginName]"></asp:SqlDataSource>
                </asp:Panel>
                </td><td colspan="2" style="text-align: left" rowspan="3">姓名：<asp:Label ID="LabelDocName" runat="server" Font-Bold="True"></asp:Label>
                    <br />
                    性别：<asp:Label ID="LabelDocSex" runat="server" Font-Bold="True"></asp:Label>
                    <br />
                    职业：<asp:Label ID="LabelDocOccupation" runat="server" Font-Bold="True"></asp:Label>
                    <br />
                    工作单位：<asp:Label ID="LabelDocWorkingUnit" runat="server" Font-Bold="True"></asp:Label>
                    <br />
                    邮箱：<asp:Label ID="LabelDocEmail" runat="server" Font-Bold="True"></asp:Label>
                    <br />
                    <asp:HyperLink ID="HyperLinkInformation" runat="server" ForeColor="Blue" NavigateUrl="~/Default.aspx">详细信息</asp:HyperLink>
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="联系该医生" />
                </td><td colspan="3" style="height: 21px; text-align:left">&nbsp; &nbsp;<asp:Label ID="LabelDocLoginName" runat="server" Font-Bold="True"></asp:Label>
                </td></tr>
            <tr><td colspan="3" style="height: 191px;">
                <asp:ListBox ID="ListBox1" runat="server" Height="172px" style="margin-top: 0px" Width="570px"></asp:ListBox>
                </td></tr>
            <tr><td colspan="3">
                <asp:TextBox ID="TextBox1" runat="server" Width="449px"></asp:TextBox>
&nbsp;
                <asp:Button ID="Button2" runat="server" Text="发  送" />
                </td></tr>
        </table>
    <p>
        医疗咨询页面的基本思路：       医疗咨询页面的基本思路：</p>
    <p>
        普通用户从现有的医生用户中选定一个医生，授权该医生访问自己的个人健康档案，同时提供个人当前面临的疾病问题，咨询疾病相关的问题。与健康咨询不同的是，本页面应当提详细的个人目前疾病具体情况，即需要通过我们的CDSS功能获取患者疾病相关的信息。</p>
    <p>
        难点：如何实现普通用户与医生之间包括文字、声音以及视频图像的实时交互功能。</p>
    <p>
        &nbsp;</p>
</asp:Content>
