<%@ Page Title="" Language="C#" MasterPageFile="~/Doctor.master" AutoEventWireup="true" CodeBehind="FamilyDoctor.aspx.cs" Inherits="CDSS.FamilyDoctor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDoctor" runat="server">
   <script type="text/javascript">
       function chat() {
           var location = "../Chat/IM.aspx?conversitation";
                 document.getElementById('frame1').src = location;
             }
        </script>
    <%--<a target="_blank" href="tencent://message/?uin=1395420683&Site=szhytech.net&Menu=yes">--%><%--<img border="0" SRC=http://wpa.qq.com/pa?p=1:1395420683:4 alt="点击这里给我发消息" />--%><%--</a> --%>
    <div class="left"><strong><a target="_blank" href="tencent://message/?uin=1395420683&Site=szhytech.net&Menu=yes"><img border="0" SRC=http://wpa.qq.com/pa?p=1:1395420683:4 alt="点击这里给我发消息" /></a>在线QQ :79931944 </strong></div>
     <div>
   
    <p>请<a onclick="chat()">登录会诊平台<%--<asp:Button ID="Button1" runat="server" Text="联系医生" OnClick="Button1_Click" />--%></a></p> 
    
    <div class="float-right">
    <iframe  id="frame1" name="displayinhere" frameborder="0" style="width: 400px; height: 700px;">
    </div>
    </div>
</asp:Content>
