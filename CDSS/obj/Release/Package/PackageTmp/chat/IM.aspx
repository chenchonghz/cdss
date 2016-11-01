<%@ Page Language="C#" AutoEventWireup="true" Inherits="IM" ValidateRequest="false" Codebehind="IM.aspx.cs" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>网络会诊</title>
    <style type="text/css">
    #divMsg i /*时间*/
    {
		color:gray;
		font-style:normal;
		text-decoration:none;
    }
    #divMsg b /*用户*/
    {
		color:blue;
		font-weight:normal;
		text-decoration:none;
    }
    #divMsg span /*公共内容*/
    {
		color:black;
    }    
    #divMsg .spanMe /*我说的话*/
    {
		color:green;
    }
    #divMsg .spanToMe /*对我说的话*/
    {
		color:red;
    }
    </style>
</head>
<body style="line-height:2em; font-family:Consolas;">
	<div>
	昵称：<input id="txtName" type="text" value="<%= userName %>" maxlength="20" /><br />
	内容：<input id="txtContent" type="text" style="width:500px;" maxlength="100" /><br />
	<input id="btn" onclick="sendMsg();" style="margin-left:47px;" type="button" value="提 交" />
	<input id="chkTo" type="checkbox"  style="display:none;"/><label id="lblTo" for="chkTo"  style="display:none;">test</label>
	</div>
	<hr />
    <div id="divMsg">
    加载中...
    </div>
    
    <script type="text/javascript">
    var pageUrl = '<%= pageUrl %>';
    var userId = '<%= userId %>';
	var btn = document.getElementById('btn');
	var txtName = document.getElementById('txtName');
	var txtContent = document.getElementById('txtContent');
	var divMsg = document.getElementById('divMsg');
	var chkTo = document.getElementById('chkTo');
	var lblTo = document.getElementById('lblTo');
	var to='';
    <%if (Session["type"].ToString()=="Doctor"){ %>
	   to = '病人';
    <%}else if (Session["type"].ToString()=="Commonuser"){ %>
       to = '医生';
    <%}else{%>
        to = '其他';
    <%}%>
    //当按下回车时触发提交事件
    txtContent.onkeydown = txtContentKeyDown;
    function txtContentKeyDown(evt)
    {
		evt = evt || event;
		if(evt.keyCode == 13)
		{
			if(!btn.disabled)
			{
				btn.click();
			}
		}
    }
    
    //当前消息接收标记
    var lastIndex = -1;
    
    //避免重复提交获取请求
    var isRun = false;
    
    //ajax与后台通讯
    function ajax(para, fn)
    {
		var ajaxObject = window.ActiveXObject?new ActiveXObject("Microsoft.XMLHTTP"):new XMLHttpRequest();
		ajaxObject.onreadystatechange = function ajaxStateChange()
		{
		   if(ajaxObject.readyState==4)
		   {
				if(ajaxObject.status==200)
				{
					fn(ajaxObject.responseText);
				}
				else
				{
					alert('网络繁忙!');
				}
			}
		};
		ajaxObject.open('POST', pageUrl, true);
		ajaxObject.setRequestHeader("Content-Type","application/x-www-form-urlencoded;charset=Utf-8");
		ajaxObject.send(para);
		return ajaxObject;
    }
    
    //发送消息
    function sendMsg()
    {
		if(btn.disabled) return;
		if(txtName.value == '')
		{
			alert('昵称必须填写！');
			txtName.focus();
			return;
		}
		if(txtContent.value == '')
		{
			alert('内容必须填写！');
			txtContent.focus();
			return;
		}
		btn.disabled = true;
		var para = 'uname=' + encodeURIComponent(txtName.value) + '&content=' + encodeURIComponent(txtContent.value) + '&to=' + to;
		ajax(para, sendMsgBack);
    }
    
    //发送消息后，服务器返回
    function sendMsgBack(str)
    {
		btn.disabled = false;
		txtContent.value = '';
		txtContent.focus();
		getMsg();
    }
    
    //获取消息
	function getMsg()
	{
		if(isRun)return;
		isRun = true;
		ajax('lastIndex=' + lastIndex, getMsgBack);
	}
	
	var autoRun = null;
    //获取消息，服务器返回
	function getMsgBack(str)
	{
		if(autoRun)clearTimeout(autoRun);
		if(lastIndex == -1)
		{
			divMsg.innerHTML = '';
		}
		var strs = str.split('<');
		lastIndex = strs[0];
		var strHTML = '';
		for(var i=1; i<strs.length; i++)
		{
			var item = strs[i].split('>');
			var contentClass = '';
			if(item[2] == userId)
			{
				contentClass = 'spanMe';
			}
			else if(item[4] == userId)
			{
				contentClass = 'spanToMe';
			}
			strHTML += '<div>';
			strHTML += '<i>' + item[0] + '</i> ';
			if(item[2] == userId)
			{
				strHTML += '<b>' + (item[1] || item[2]) + '</b>';
			}
			else
			{
			    strHTML += '<b style="cursor:pointer;" onclick="toSay(this);" title="' + item[2] + '";>' + (item[1] || item[2]) + '</b>';
			}
			if(item[4] != '')
			{
			    strHTML += ' 对 <b>' + (item[5] || item[4]) + '</b>';//
			}
			strHTML += '<br><span class="' + contentClass + '">' + item[3] + '</span>';
			strHTML += '</div>';
		}
		divMsg.innerHTML = strHTML + divMsg.innerHTML;
		isRun = false;
		
		//每隔1秒从服务器获取一次数据
		autoRun = setTimeout(getMsg, 1000);
	}
	
	//对指定人发信息
	function toSay(o)
	{
		chkTo.checked = true;
		chkTo.style.display = 'inline';
		lblTo.innerHTML = o.innerHTML;
		lblTo.style.display = 'inline';
		to = o.title;
		txtContent.focus();
	}
	
	getMsg();

    </script>
</body>
</html>
