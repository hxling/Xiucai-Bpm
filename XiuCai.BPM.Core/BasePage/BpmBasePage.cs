using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Common;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Model;

namespace Xiucai.BPM.Core.BasePage
{
    public class BpmBasePage:System.Web.UI.Page
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; private set; }
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// NavId
        /// </summary>
        public int NavId { get; private set; }

        /// <summary>
        /// 当前页面可用按钮列表
        /// </summary>
        public IEnumerable<Button> PageButtons { get; private set; }

        /// <summary>
        /// 当前页面可用按钮JSON数据
        /// </summary>
        public string PageButtonsJson
        {
            get { return JSONhelper.ToJson(PageButtons.ToList()); }
        }

        private const string LoginUrl = "/login.html";
        private const string MainPage = "/default.aspx";

        protected override void OnPreInit(EventArgs e)
        {
            string absolutePath = Request.Url.AbsolutePath.ToLower();
            string dir = absolutePath.Substring(0, absolutePath.LastIndexOf('/')); 

            if(SysVisitor.Instance.IsGuest)
            {
                if(absolutePath == dir + MainPage)
                    Response.Redirect(dir+LoginUrl, true);

                var loginUrlWithVirturalDir = LoginUrl;
                if(dir.IndexOf(dir, System.StringComparison.Ordinal) > -1)
                    loginUrlWithVirturalDir = dir + LoginUrl;

                Response.Write("<script>alert('亲，登录已过期！请重新登录哦。');window.top.location='" + loginUrlWithVirturalDir + "';</script>");
                Response.End();
            }

            UserId = SysVisitor.Instance.UserId;
            UserName = SysVisitor.Instance.UserName;

            NavId = PublicMethod.GetInt(Request["navid"]);

            if (NavId <= 0) return;
            if(!SysVisitor.Instance.IsAdmin)
            {
                if (!UserBll.Instance.HasMenu(UserId, NavId))
                {
                    Response.Write(AlertMessage("亲，您没有权限哦！",true));
                    Response.End();
                }
            }

            PageButtons = UserBll.Instance.GetPageButtons(UserId, NavId);
        }

        private string AlertMessage(string msg, bool hasjsfile)
        {
            string jsfile = "<script src=\"/Scripts/jquery-1.7.2.min.js\" type=\"text/javascript\"></script><script src=\"/Scripts/easyui/jquery.easyui.min.js\" type=\"text/javascript\"></script><link href=\"/Scripts/easyui/themes/default/easyui.css\" rel=\"stylesheet\" type=\"text/css\" />";
            string resoult = "<script type=\"text/javascript\">$(function () {$.messager.alert('系统提示！', '<span style=\"color:red;font-weight:bold;\">" + msg + "</span>', 'error');     }) </script>";
            if (hasjsfile)
                return jsfile + resoult;
            return resoult;
        }

        /// <summary>
        /// 创建页面工具栏
        /// </summary>
        /// <returns></returns>
        public string BuildToolbar()
        {
            return UserBll.Instance.PageButtons(UserId, NavId);
        }

        public string ThemeName
        {
            get
            {
                var configJson = SysVisitor.Instance.CurrentUser.ConfigJson;
                if(string.IsNullOrEmpty(configJson))
                    return "default";
                return JSONhelper.ConvertToObject<ConfigModel>(configJson).Theme.Name;
            }
        }

        public string SitePath
        {
            get
            {
                return ConfigHelper.GetValue("sitepath");
            }
        }
    }
}
