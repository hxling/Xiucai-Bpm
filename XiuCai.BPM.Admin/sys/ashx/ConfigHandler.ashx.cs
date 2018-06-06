using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Xiucai.Common;

namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// ConfigHandler 的摘要说明
    /// </summary>
    public class ConfigHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var json = context.Request["json"];
            var action = context.Request["action"];
            switch (action)
            {
                case "js":
                    User u = UserDal.Instance.Get(SysVisitor.Instance.UserId);
                    string cj = u.ConfigJson;
                    if (string.IsNullOrEmpty(cj))
                        context.Response.Write("var sys_config ={\"theme\":{\"title\":\"默认皮肤\",\"name\":\"default\",\"selected\":true},\"showType\":\"Accordion\",\"gridRows\":20}");
                    else
                    {
                        string js = "var sys_config = " + cj;
                        context.Response.Write(js);
                    }
                    break;
                default:
                    int k = UserDal.Instance.UpdateUserConfig(SysVisitor.Instance.UserId, json);
                    SysVisitor.Instance.CurrentUser.ConfigJson = json;
                    context.Response.Write(k);
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