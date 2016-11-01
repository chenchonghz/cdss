<%@ Page Title="临床决策支持系统" Language="C#" MasterPageFile="~/Doctor.master" AutoEventWireup="true" CodeBehind="DoctorsCDSS-Old.aspx.cs" Inherits="CDSS.DoctorsCDSS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDoctor" runat="server">
        <table style="width:100%;">
            <tr>
                <td style="text-align: left; color: #0000FF;" rowspan="20">&nbsp;</td>
                <td style="text-align: left; height: 23px; color: #0000FF;" colspan="3">基本资料：</td>
                <td style="text-align: left; height: 23px; color: #0000FF;" colspan="4">病史采集：</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 161px;">姓名:
                    <asp:Literal ID="LiteralName" runat="server" Text="张三"></asp:Literal>
                </td>
                <td style="text-align: left; width: 131px;">性别: <asp:Literal ID="LiteralSex" runat="server" Text="男"></asp:Literal>
                </td>
                <td style="text-align: left; width: 175px;">年龄:
                    <asp:Literal ID="LiteralAge" runat="server" Text="22岁"></asp:Literal>
                </td>
                <td style="text-align: left; width: 109px;" rowspan="3">
                    <asp:Panel ID="Panel1" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                        症候：<asp:BulletedList ID="BlSymptomName" runat="server" DisplayMode="LinkButton" Height="16px" OnClick="BulletedList1_Click" Width="62px">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="text-align: left; width: 90px;" rowspan="3" colspan="2">
                    <asp:Panel ID="Panel2" runat="server" Height="127px" ScrollBars="Auto" Width="150px">
                        属性<br />
                        <asp:BulletedList ID="BulletedList2" runat="server" DisplayMode="LinkButton" OnClick="BulletedList2_Click" style="margin-bottom: 19px; margin-top: 0px;" Height="17px" Width="85px">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="text-align: left" rowspan="3">
                    <asp:Panel ID="Panel3" runat="server" Height="127px" ScrollBars="Vertical" Width="205px">
                        选项<asp:BulletedList ID="BulletedList3" runat="server" DisplayMode="LinkButton" OnClick="BulletedList3_Click">
                        </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="text-align: left" rowspan="3">
                    <asp:Button ID="ButtonSymptom" runat="server" OnClick="ButtonSymptom_Click" Text="提交" />
                    <asp:Button ID="ButtonClear" runat="server" OnClick="ButtonClear_Click" Text="清空" />
                </td>

            </tr>
            <tr>
                <td style="text-align: left; width: 161px; height: 19px;">民族:
                    <asp:Literal ID="LiteralNation" runat="server" Text="汉族"></asp:Literal>
                </td>
                <td style="text-align: left; height: 19px; width: 131px;">婚姻:
                    <asp:Literal ID="LiteralMarriage" runat="server" Text="已婚"></asp:Literal>
                </td>
                <td style="text-align: left; height: 19px; width: 175px;">职业:
                    <asp:Literal ID="LiteralOccupation" runat="server" Text="学生"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 26px;" colspan="3">其他...&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; height: 12px;" colspan="3">主诉：</td>
                <td style="text-align: left; color: #0000FF; height: 12px;" colspan="4">
                    体格检查：</td>
            </tr>
            <tr>
                <td style="text-align: left; height: 17px;" colspan="3">
                    <asp:TextBox ID="TextBox1" runat="server" Height="47px" Width="243px" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td style="text-align: left; height: 17px; width: 109px;">
                    <asp:Panel ID="Panel4" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                    <asp:BulletedList ID="BulletedList4" runat="server" DisplayMode="LinkButton" OnClick="BulletedList4_Click">
                    </asp:BulletedList>
                    </asp:Panel>
                </td>
                <td style="text-align: left; height: 17px; width: 90px;" colspan="2">
                    <asp:Panel ID="Panel5" runat="server" Height="127px" ScrollBars="Auto" Width="150px">
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CDSSConnectionString %>" SelectCommand="SELECT [ChineseName] FROM [FindingsBase] WHERE ([FindingType] = @FindingType)">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="症状" Name="FindingType" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    <asp:BulletedList ID="BulletedList5" runat="server" DisplayMode="LinkButton" OnClick="BulletedList5_Click">
                    </asp:BulletedList>
                        </asp:Panel>
                </td>
                <td style="text-align: left; height: 17px;">
                    <asp:Panel ID="Panel6" runat="server" Height="127px" ScrollBars="Vertical" Width="205px">
                    <asp:BulletedList ID="BulletedList6" runat="server" DisplayMode="LinkButton" OnClick="BulletedList6_Click">
                    </asp:BulletedList>
                        </asp:Panel>
                </td>
                <td style="text-align: left; height: 17px;">
                    <asp:Button ID="ButtonSign" runat="server" Text="提交" OnClick="ButtonSign_Click" />
                    </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF; height: 17px;" colspan="3">现病史：</td>
                <td style="text-align: left; color: #0000FF; height: 17px;" colspan="4">实验室检查：</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Literal ID="Literal2" runat="server" Text="根据右侧病史采集的结果,自动生成的患者现病史的文本表述"></asp:Literal>
                </td>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:Panel ID="Panel7" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                    <asp:BulletedList ID="BulletedListLaboratory" runat="server" DisplayMode="LinkButton" OnClick="BulletedListLaboratory_Click">
                    </asp:BulletedList>
                        </asp:Panel>
                </td>
                <td style="text-align: left" colspan="2">实验室检查的目的与意义(<Mark>申请相应检查</Mark>)</td>
                <td style="text-align: left" colspan="2">
                    <asp:Button ID="ButtonLaboratory" runat="server" Text="提交" OnClick="ButtonLaboratory_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="3">体格检查：</td>
                <td style="text-align: left; color: #0000FF;" colspan="4">影像学检查：</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Literal ID="Literal3" runat="server" Text="根据右侧体格检查选定结果,自动生成的患者体格检查的文本表述"></asp:Literal>
                </td>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:Panel ID="Panel8" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                    <asp:BulletedList ID="BulletedListRadiology" runat="server" DisplayMode="LinkButton" OnClick="BulletedListRadiology_Click">
                    </asp:BulletedList>
                         </asp:Panel>
                </td>
                <td style="text-align: left" colspan="2">影像学检查的目的与意义(<Mark>申请相应检查</Mark>)</td>
                <td style="text-align: left" colspan="2">
                    <asp:Button ID="ButtonRadiology" runat="server" Text="提交" OnClick="ButtonRadiology_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="3">相关检查：</td>
                <td style="text-align: left; color: #0000FF;" colspan="4">其他检查：</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Literal ID="Literal4" runat="server" Text="根据右侧实验室检查、影像学检查以及其他检查选定结果,自动生成的患者体格检查的文本表述"></asp:Literal>
                </td>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:Panel ID="Panel9" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                    <asp:BulletedList ID="BulletedListOtherExam" runat="server" DisplayMode="LinkButton" OnClick="BulletedListOtherExam_Click">
                    </asp:BulletedList>
                        </asp:Panel>
                </td>
                <td style="text-align: left" colspan="2">其他检查的目的与意义(<Mark>申请相应检查</Mark>)</td>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:Button ID="ButtonOtherExam" runat="server" Text="提交" OnClick="ButtonOtherExam_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="3">临床诊断：</td>
                <td style="text-align: left; color: #0000FF;" colspan="4">诊断与鉴别诊断：</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Literal ID="Literal5" runat="server" Text="根据右侧诊断和鉴别诊断选定结果,自动生成的患者临床诊断的文本表述"></asp:Literal>
                </td>
                <td style="text-align: left; width: 268435408px;" colspan="2">

                    <asp:Panel ID="Panel10" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                    <asp:BulletedList ID="BulletedList10" runat="server" Width="140px">
                        <asp:ListItem>诊断的可能性之一(50%)</asp:ListItem>
                        <asp:ListItem>诊断的可能性之二(25%)</asp:ListItem>
                        <asp:ListItem Value="诊断的可能性之三(12%)">诊断的可能性之三(25%)</asp:ListItem>
                        <asp:ListItem>其他的可能诊断</asp:ListItem>
                    </asp:BulletedList>
                        </asp:Panel>
                </td>
                <td style="text-align: left" colspan="2">相关疾病的简要说明与介绍(<Mark>链接相关参考资料</Mark>)</td>
                <td style="text-align: left" colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left; color: #0000FF;" colspan="3">处理意见：</td>
                <td style="text-align: left; color: #0000FF;" colspan="4">治疗措施与建议：</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">
                    <asp:Literal ID="Literal6" runat="server" Text="根据右侧治疗措施与建议的选定结果,自动生成的患者处理意见的文本表述"></asp:Literal>
                </td>
                <td style="text-align: left; width: 268435408px;" colspan="2">
                    <asp:Panel ID="Panel11" runat="server" Height="127px" ScrollBars="Vertical" Width="150px">
                    <asp:BulletedList ID="BulletedList11" runat="server" Width="130px">
                        <asp:ListItem>治疗措施之一</asp:ListItem>
                        <asp:ListItem>治疗措施之二</asp:ListItem>
                        <asp:ListItem Value="治疗措施之三">治疗措施之三</asp:ListItem>
                        <asp:ListItem>...</asp:ListItem>
                    </asp:BulletedList>
                        </asp:Panel>
                </td>
                <td style="text-align: left" colspan="2">
                    相关的介绍(<Mark>选定当前的治疗措施或建议</Mark>)</td>
                <td style="text-align: left" colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">&nbsp;</td>
                <td style="text-align: left" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">&nbsp;</td>
                <td style="text-align: left" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">&nbsp;</td>
                <td style="text-align: left" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left" colspan="3">&nbsp;</td>
                <td style="text-align: left" colspan="4">&nbsp;</td>
            </tr>
        </table>
    <p>
    Working with clinical decision support system here.</p>
</asp:Content>
