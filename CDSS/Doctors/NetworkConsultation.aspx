<%@ Page Title="" Language="C#" MasterPageFile="~/Doctor.master" AutoEventWireup="true" CodeBehind="NetworkConsultation.aspx.cs" Inherits="CDSS.NetworkConsultation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDoctor" runat="server">
    <script>
        function DoctorChat() {
            //document.write("nihao");
            document.getElementById('frame1').src = 'http://192.168.0.100/omaster/webclass.php?token=c95c29059ba77c6e5aabe9ac1b071254&id=1';
        }
 </script>
    <table>
    <tr>
     <td style="text-align:left">
    <input type="button" value="登陆网络会诊平台" onclick="DoctorChat()">
     </td>
    </tr>
    <tr>   
    <td> 
    <div class="float-right">
    <iframe  id="frame1" name="displayinhere"  frameborder="0" style="width: 1200px; height: 700px;">
    </div>
    </td>
    </tr>
    </table>
</asp:Content>
