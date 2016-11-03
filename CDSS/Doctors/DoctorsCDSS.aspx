<%@ Page Title="临床决策支持系统" Language="C#" MasterPageFile="~/Doctor.master" AutoEventWireup="true" CodeBehind="DoctorsCDSS.aspx.cs" Inherits="CDSS.DoctorsCDSS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDoctor" runat="server">
    <p style="text-align: center;">
        <asp:Literal ID="LiteralBasicalInfo" runat="server"></asp:Literal>
                </p>
        <table style="width:100%; height: 544px;">
            <tr>
                <td style="text-align: left; color: #0000FF; vertical-align: top;" rowspan="11">
                    &nbsp;</td>
                <td style="text-align: left; height: 23px; color: #0000FF;">基本资料：</td>
                <td rowspan="11" style="text-align: left; width: 6px;"></td>
                <td style="text-align: left; height: 23px; color: #0000FF;" colspan="4">病史采集：</td>
                <td style="text-align: left; height: 23px; color: #0000FF;">诊断与鉴别诊断：</td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top;" rowspan="5">
                    <asp:GridView ID="GridViewUsersList" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="262px" OnSelectedIndexChanged="GridViewUsersList_SelectedIndexChanged" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="PhrNumber" HeaderText="PHR编号" SortExpression="PhrNumber" />
                            <asp:BoundField DataField="Name" HeaderText="用户姓名" SortExpression="Name" />
                            <asp:BoundField DataField="Sex" HeaderText="性别" SortExpression="Sex" />
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [PhrNumber], [Name], [Sex] FROM [CommonUsersPhr]"></asp:SqlDataSource>
                    <br />
                    <br />
                </td>
                <td style="text-align: left; width: 109px; vertical-align: top;">
                    <asp:Panel ID="Panel1" runat="server" Height="127px" ScrollBars="Auto" Width="150px">
                        <asp:BulletedList ID="blSymptomName" runat="server" DisplayMode="LinkButton" style="margin-bottom: 19px; margin-top: 0px;" Height="17px" Width="111px" OnClick="blSymptomName_Click">
                        </asp:BulletedList>
                    </asp:Panel>

                </td>
                <td style="text-align: left; width: 90px;" colspan="2">
                    <asp:Panel ID="Panel2" runat="server" Height="127px" ScrollBars="Auto" Width="150px">
                        <asp:BulletedList ID="blSymptomProperty" runat="server" DisplayMode="LinkButton" style="margin-bottom: 19px; margin-top: 0px;" Height="19px" Width="146px" OnClick="blSymptomProperty_Click">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="text-align: left; ">
                    <asp:Panel ID="Panel3" runat="server" Height="127px" ScrollBars="Auto" Width="150px">
                        <asp:BulletedList ID="blSymptomOption" runat="server" DisplayMode="LinkButton" OnClick="blSymptomOption_Click" style="height: 13px">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="text-align: left; vertical-align: top;" rowspan="9">
                    <asp:Panel ID="Panel4" runat="server" Height="281px" ScrollBars="Auto" Width="222px">
                    <asp:BulletedList ID="blDiagnosisProb" runat="server" Width="179px" Height="16px" style="margin-left: 17px" DisplayMode="LinkButton" OnClick="blDiagnosisProb_Click">
                    </asp:BulletedList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="4">体格检查：</td>
            </tr>
            <tr>
                <td style="text-align: left; height: 17px; width: 109px;">
                    <asp:BulletedList ID="BulletedList4" runat="server" Width="105px" Height="16px">
                    </asp:BulletedList>
                </td>
                <td style="text-align: left; height: 17px; width: 90px;" colspan="2">
                    <asp:BulletedList ID="BulletedList8" runat="server" Width="105px" Height="16px">
                    </asp:BulletedList>
                </td>
                <td style="text-align: left; height: 17px; ">
                    <asp:BulletedList ID="BulletedList9" runat="server" Width="111px" Height="16px">
                    </asp:BulletedList>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; height: 17px;" colspan="4">实验室检查：</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:BulletedList ID="BulletedList10" runat="server" Width="163px">
                    </asp:BulletedList>
                </td>
                <td style="text-align: left" colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top;" rowspan="5">
                    基本病史：<br />
                    <asp:Literal ID="LiteralBriefComplain" runat="server"></asp:Literal>
                    <br />
                    <asp:Literal ID="LiteralCurHistory" runat="server"></asp:Literal>
                    <br />
                    <asp:Literal ID="LiteralPhysicalExam" runat="server"></asp:Literal>
                    <br />
                    <asp:Literal ID="LiteralPreliminaryDiag" runat="server"></asp:Literal>
                    <br />
                    <asp:Literal ID="LiteralPreliminaryTreat" runat="server"></asp:Literal>
                    <br />
                </td>
                <td style="text-align: left; color: #0000FF;" colspan="4">影像学检查：</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 268435408px; height: 32px;" colspan="2">
                    <asp:BulletedList ID="BulletedList11" runat="server" Width="163px">
                    </asp:BulletedList>
                </td>
                <td style="text-align: left; height: 32px;" colspan="2">
                    </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="4">其他检查：</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:BulletedList ID="BulletedList12" runat="server" Width="159px">
                    </asp:BulletedList>
                </td>
                <td style="text-align: left" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="4">
                    <asp:Panel ID="Panel5" runat="server" Height="103px" ForeColor="Black">
                        <asp:Literal ID="LiteralDiseaseIntro" runat="server"></asp:Literal>
                    </asp:Panel>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <asp:Chart ID="Chart1" runat="server" BorderlineWidth="0" Height="203px" OnLoad="Chart1_Load" Width="217px" >
                        <Series>
                            <asp:Series BorderWidth="0" MarkerBorderWidth="0" MarkerSize="0" Name="Series1" ChartArea="ChartArea1">
                                <SmartLabelStyle Enabled="False" />
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <AxisX LineWidth="0" IsLabelAutoFit="False">
                                    <MajorGrid Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                    <LabelStyle Angle="45" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </td>
            </tr>
            <tr>
                <td  colspan="2"></td>
                <td style="text-align: left; color: #ff0000;" colspan="4">检查结果：</td>
            </tr>
            <tr>
                <td  colspan="2"></td>
                <td style="text-align: left; color: #ff0000;" colspan="4">
                    <asp:DropDownList ID="DropDownList1" runat="server" Height="29px" Width="150px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Button ID="SureButton" runat="server" Text="确定" Height="29px" Width="80px" OnClick="SureButton_Click"  />
                </td>
            </tr>
            </table>
    <p>
        &nbsp;</p>
</asp:Content>
