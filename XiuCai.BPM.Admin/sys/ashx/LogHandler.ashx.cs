using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Xiucai.Common;

namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// LogHandler 的摘要说明
    /// </summary>
    public class LogHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (SysVisitor.Instance.IsGuest)
            {
                context.Response.Write(
                    new JsonMessage { Success = false, Data = "-99", Message = "登录已过期，请重新登录" }.ToString()
                    );
                context.Response.End();
            }

            var rpm = new RequestParamModel<LogModel>(context) { CurrentContext = context,Action = context.Request["action"],
                                                                 KeyId = PublicMethod.GetInt(context.Request["keyid"])
            };
            switch (rpm.Action)
            {
                case "logdetail":
                    context.Response.Write(JSONhelper.ToJson(LogDetailDal.Instance.GetBy(rpm.KeyId).ToList()));
                    break;
                case "clearlog":
                    LogBll<object> log = new LogBll<object>();
                    int days = PublicMethod.GetInt(context.Request["days"]);
                    context.Response.Write(log.ClearLog(days));
                    break;
                default:
                    string s = LogDal.Instance.JsonDataForEasyUIdataGrid(rpm.Pageindex, rpm.Pagesize, rpm.Filter);
                    context.Response.Write(s);
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