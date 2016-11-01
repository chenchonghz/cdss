<%@ Page Title="" Language="C#" MasterPageFile="~/CommonUser.master" AutoEventWireup="true" CodeBehind="HealthConsultation.aspx.cs" Inherits="CDSS.HealthConsultation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderCommonUser" runat="server">
         <script type="text/javascript">
             function chat() {
                 var location="../Chat/IM.aspx?conversitation"<%--+"<%=Session["UserName"].ToString()%>"--%>;
                 document.getElementById('frame1').src = location;
             }
        </script>
    
    <div>
   
    <input type="button" value="联系医生" onClick="chat()">
    
    <div class="float-right">
    <iframe  id="frame1" name="displayinhere" frameborder="0" style="width: 400px; height: 700px;">
    </div>
    </div>
 
</asp:Content>
