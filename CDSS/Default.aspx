<%@ Page Title="网站主页" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CDSS._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>全民健康医疗服务系统<mark>(iDoctors)</mark></h1>
            </hgroup>
            <p>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                通过医疗信息技术与人工智能技术的结合，开发出以智能化个人健康档案(intelligent Personal Health Record, <mark>iPHR</mark>)为核心数据以及以临床决策支持系统(Clinical Desicion Support System, <mark>CDSS</mark>)的核心算法的智能化网络家庭医生系统（intelligent internet doctor system, <mark>iDoctors</mark>）远程医疗系统，为广大民众提供全方位的健康医疗服务。
            </p>
            <p>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                如果您对本系统有任何您的意见或建议，请点击<a href="/Resources/UsersFeedback">用户意见反馈</a>提出您的报告意见。
                查看<a href="/Doctors/DisplayPhysicalData">体温图表</a>。
            </p>
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>项目研发计划分为三个阶段分步实施 ：</h3>
    <ol class="round">
        <li class="one">
            <h5>社区养老健康医疗服务系统</h5>
            第一阶段，通过建立智能化个人健康档案(iPHR)并融合可穿戴式或便携式生理参数采集设备，针对严重危害老年人生命健康的常见疾病，开发出可用于社区或家庭养老的“社区养老健康医疗服务系统”，旨在为专业化养老社区或家庭提供便捷的健康养老医疗服务。
        </li>
        <li class="two">
            <h5>基层医师诊疗支持系统</h5>
            二阶段，在“社区养老健康医疗服务系统”的基础上，针对基层医院的常见病、多发病以及慢性疾病，开发出为基层医院医生提供临床决策支持的“基层医师诊疗支持系统”，旨在通过该系统的智能化专业平台，提高基层医师的诊断和治疗水平，显著提升基层社区医院的医疗服务水平，充分发挥基层医院的重要作用。
        </li>
        <li class="three">
            <h5>全民健康医疗服务网络系统</h5>
            第三阶段，在“基层医师诊疗支持系统”的基础上，不断扩大系统覆盖的疾病种类和服务范围。通过建立iPHR与医院信息系统电子病历的数据接口，逐步形成以iPHR云数据平台为核心、覆盖全国的医疗信息服务网络，开发出能够为所有级别医院医生，所有患者以及健康人群提供服务的“全民健康医疗服务网络系统”。旨在为医生开展远程医疗服务提供全方位临床决策支持服务，为患者提供全面的远程医疗服务，为健康人提供全面的卫生保健服务。
        </li>
    </ol>
    <p>
        打开<a href="/TestPage">测试页面</a>。
    </p>
</asp:Content>
