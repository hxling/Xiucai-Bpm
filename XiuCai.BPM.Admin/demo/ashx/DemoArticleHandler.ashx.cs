using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.Demo.Bll;
using Xiucai.Demo.Model;
using Xiucai.Model;
using Xiucai.Bll;

using Omu.ValueInjecter;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.Common;

namespace Xiucai.BPM.Admin.demo.ashx
{
    /// <summary>
    /// dbHandler 的摘要说明
    /// </summary>
    public class DemoArticleHandler : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            UserBll.Instance.CheckUserOnlingState();

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<DemoArticleModel>(context) { CurrentContext = context };
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<DemoArticleModel>>(json);
                rpm.CurrentContext = context;
            }

            var d = new DemoArticleModel();
            if (!string.IsNullOrEmpty(json))
            {
                string content = rpm.Request("content");
               
                d.InjectFrom(rpm.Entity);
                d.body = content;
            }

            switch (rpm.Action)
            {
                case "add":
                    context.Response.Write(DemoArticleBll.Instance.Add(d));
                    break;
                case "edit":
                    d.KeyId = rpm.KeyId;
                    context.Response.Write(DemoArticleBll.Instance.Update(d));
                    break;
                case "delete":
                    context.Response.Write(DemoArticleBll.Instance.Delete(rpm.KeyId));
                    break;
                default:
                    context.Response.Write(DemoArticleBll.Instance.GetJson(rpm.Pageindex, rpm.Pagesize, rpm.Filter, rpm.Sort, rpm.Order));
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}