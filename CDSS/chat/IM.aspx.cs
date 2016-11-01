using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Windows.Forms;
public partial class IM : System.Web.UI.Page
{
	//当前用户昵称
	protected string userName = "";
	//当前用户ID
	protected string userId = "";
	//当前页面地址
	protected string pageUrl = "";
	//消息列表
	private static IList<Msg> MsgList = new List<Msg>();
	//消息唯一标识
	private static int MsgIndex = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
		userId = GetUserId();
       
        if (Request.Params["lastIndex"] != null)
        {
            //获取未读信息

            int lastIndex = -1;
            if (Request.Params["lastIndex"] != "")
            {
                lastIndex = int.Parse(Request.Params["lastIndex"]);
            }
            string str = GetMsg(lastIndex);
            Response.Write(str);
            Response.End();
        }
        else if (Request.Params["uname"] != null)
        {
            //发送信息
            AddMsg(Request.Params["uname"], Request.Params["content"], Request.Params["to"]);
            Response.End();
        }
        else
        {
            pageUrl = "IM.aspx" + Request.Url.Query;//Request.Url.Query是用来设置群组的
            //初始化，从cookie中获取昵称
            //if (Request.Cookies["IM_USER"] != null)
            //{
            //    userName = Request.Cookies["IM_USER"].Value;          
            //}
             if (Session["type"].ToString()=="Doctor")
            {
                userName = "医生";
            }
            else if(Session["type"].ToString()=="Commonuser")
            {
                userName = "病人";
            }
            else
            {
                userName = "其他";
            }
        }
    }

	//添加消息
	private void AddMsg(string user, string content, string to)
	{
		//将昵称记入cookie
		HttpCookie cookie = new HttpCookie("IM_USER");
		cookie.Expires = DateTime.Now.AddDays(365);
		cookie.Value = user;
		Response.Cookies.Add(cookie);

		Msg item = new Msg();
		MsgIndex++;
		item.userId = userId;
		item.index = MsgIndex;
		item.dt = DateTime.Now;
		item.user = user;
		item.content = content;
		item.to = to;
		item.toUser = GetUserName(item.to);
		item.group = Request.Url.Query;
		MsgList.Add(item);

		if (MsgList.Count > 5000)
		{
			//防止消息列表数据超过5000条
			for (int i = 0; i < MsgList.Count - 5000; i++)
			{
				MsgList.RemoveAt(0);
			}
		}
	}

	//获取消息
	private string GetMsg(int lastIndex)
	{
		string strReturn = MsgIndex.ToString();
		int j = 0;
		for (int i = MsgList.Count - 1; i >= 0; i--)
		{
			//只显示同组信息
			if (MsgList[i].group != Request.Url.Query)
			{
				continue;
			}

			//显示自己发的消息，显示给我的消息，显示公共消息
			if (!string.IsNullOrEmpty(MsgList[i].to) && MsgList[i].to != userId && MsgList[i].userId != userId)
			{
				continue;
			}

			//获取未读的最近100条消息
			if (lastIndex < MsgList[i].index)
			{
				if (j++ > 100)
				{
					break;
				}
				strReturn += "<" + MsgList[i].ToString();
			}
		}
		
		return strReturn;
	}

	//获取用户名
	private string GetUserName(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return "";
		}

		string userName = "";
		for (int i = MsgList.Count - 1; i >= 0; i--)
		{
			if (MsgList[i].userId == id)
			{
				userName = MsgList[i].user;
				break;
			}
		}

		return userName;
	}

	//获取当前用户唯一ID
	private string GetUserId()
	{
		string id = "";

		if (Request.Cookies["IM_USER_ID"] != null)
		{
			id = Request.Cookies["IM_USER_ID"].Value;
		}
		else
		{
			id = Guid.NewGuid().ToString();
			//将用户标示记入cookie
			HttpCookie cookie = new HttpCookie("IM_USER_ID");
			cookie.Expires = DateTime.Now.AddDays(365);
			cookie.Value = id;
			Response.Cookies.Add(cookie);
		}

		return id;
	}

	/// <summary>
	/// 消息类
	/// </summary>
	public class Msg
	{
		/// <summary>
		/// 消息ID
		/// </summary>
		public int index;

		/// <summary>
		/// 消息时间
		/// </summary>
		public DateTime dt;

		/// <summary>
		/// 发送人ID
		/// </summary>
		public string userId;

		/// <summary>
		/// 发送人昵称
		/// </summary>
		public string user;

		/// <summary>
		/// 内容
		/// </summary>
		public string content;

		/// <summary>
		/// 组
		/// </summary>
		public string group;

		/// <summary>
		/// 接收者ID 当为空时为公共消息
		/// </summary>
		public string to;

		/// <summary>
		/// 接收者昵称
		/// </summary>
		public string toUser;

		/// <summary>
		/// 拼接消息
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string strReturn = dt.ToString("yyyy-MM-dd HH:mm:ss");
			strReturn += ">" + FormatPara(user);
			strReturn += ">" + userId;
			strReturn += ">" + FormatPara(content);
			strReturn += ">" + to;
			strReturn += ">" + FormatPara(toUser);
			return strReturn;
		}

		//格式化HTML
		private string FormatPara(string str)
		{
			str = str.Replace("&", "&amp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			return str;
		}
	}
}
