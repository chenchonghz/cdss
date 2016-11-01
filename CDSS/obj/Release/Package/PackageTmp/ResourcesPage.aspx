<%@ Page Title="信息与资源" Language="C#" MasterPageFile="~/Resource.master" AutoEventWireup="true" CodeBehind="ResourcesPage.aspx.cs" Inherits="CDSS.ResourcesPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderResource" runat="server">    
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>本页面用于显示或下载已经编辑或上载的有关资源信息。</h2>
            </hgroup>
            <p style="text-align: left">
                相关信息资源包括每一个编程同学每周的完成的编程文档，需要上载共享资源以及本网站用户提出的意见和简单等。</p>
        </div>
    </section>
        <h3>利用该页面您可以：</h3>
    <ol class="round">
        <li class="one">
            <h5>上载及下载编程文档</h5>
            为了便于程序开发过程中的编程文档管理，项目组要求每位参加编程的同学，在下一周课题讨论之前把自己编写的文档上传到我们的工作服务器，以便项目主持人及时分析大家的工作完成情况，并将大家各自的工作成果整合的项目框架之中，同时及时反馈修改意见或建议。【注>本项工作的完成情况将作为大家工作量化考核的主要依据。</li>
        <li class="two">
            <h5>上载及下载资源文档</h5>
            为了更好地实现资源共享，请大家把所有与本项目有关的重要参考资料文档上载到我们的工作服务器。参考资料文档可以是有关网页编程的资料，也可以是算法研究或实现方法的资料，也可以是医疗信息等资料，只要大家觉得有用，就请你们把你们发现的资料上传的这里。【注】本项工作的完成情况也将作为大家工作量化考核的重要依据。</li>
        <li class="three">
            <h5>设置已注册用户权限</h5>
            为了避免未授权用户有意或无意修改相关数据，需要加强用户权限管理，目的是为已经注册的用户设置相应的权限。</li>
    </ol>
</asp:Content>
