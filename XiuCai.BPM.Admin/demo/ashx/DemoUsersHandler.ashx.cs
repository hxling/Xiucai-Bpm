using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.Demo.Model;
using Xiucai.Demo.Bll;

using Omu.ValueInjecter;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.Common;

namespace Xiucai.BPM.Admin.demo.ashx
{
    /// <summary>
    /// dbHandler 的摘要说明
    /// </summary>
    public class DemoUsersHandler : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            UserBll.Instance.CheckUserOnlingState();

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<DemoUsersModel>(context) { CurrentContext = context };
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<DemoUsersModel>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            {
                case "add":
                    context.Response.Write(DemoUsersBll.Instance.Add(rpm.Entity));
                    break;
                case "edit":
                    DemoUsersModel d = new DemoUsersModel();
                    d.InjectFrom(rpm.Entity);
                    d.KeyId = rpm.KeyId;
                    context.Response.Write(DemoUsersBll.Instance.Update(d));
                    break;
                case "delete":
                    context.Response.Write(DemoUsersBll.Instance.Delete(rpm.KeyId));
                    break;
                default:
                    context.Response.Write(DemoUsersBll.Instance.GetJson(rpm.Pageindex, rpm.Pagesize, rpm.Filter, rpm.Sort, rpm.Order));
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