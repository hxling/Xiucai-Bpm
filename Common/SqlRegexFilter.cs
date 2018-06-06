using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
namespace Xiucai.Common
{

    /// <summary>
    /// 防SQL注入漏洞的HttpModule
    /// Powered By killkill
    /// </summary>
    public class SqlRegexFilter : IHttpModule
    {
        #region IHttpModule 成员

        public void Dispose()
        {
        }

        /// <summary>
        /// 检测的最短长长度，在web.config中配置
        /// </summary>
        private int minQueryLength = 0;

        /// <summary>
        /// SQL注入检测的正则表达式，在web.config中配置
        /// </summary>
        private Regex denyRegex = null;

        /// <summary>
        /// 检测到SQL注入后跳转到的页面，在web.config中配置
        /// </summary>
        private string redirectPage = null;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            denyRegex =
                new Regex(
                    ConfigurationManager.AppSettings["killkill_DenyRegex"],
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.minQueryLength =
                int.Parse(ConfigurationManager.AppSettings["killkill_QueryLength"]);
            this.redirectPage =
                ConfigurationManager.AppSettings["killkill_RedirectPage"];
        }

        /// <summary>
        /// 截获每个请求并分析其Request参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication Application = (HttpApplication)sender;
            HttpContext ctx = Application.Context;
            foreach (string key in ctx.Request.QueryString.Keys)
            {
                string value = ctx.Request[key];
                if (value.Length > 10)
                {
                    if (denyRegex.Match(value).Success)
                    {
                        Application.CompleteRequest();
                        ctx.Response.Redirect(redirectPage);
                    }
                }
            }
        }

        #endregion
    }
}

