using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.Common.Data.Filter;
using Xiucai.Model;
using Xiucai.Bll;

using Omu.ValueInjecter;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.Common;
using Xiucai.Common.Data;

namespace Xiucai.BPM.Admin.Demo.ashx
{
    /// <summary>
    /// dbHandler 的摘要说明
    /// </summary>
    public class DemoRuKuDanHandler : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            UserBll.Instance.CheckUserOnlingState();

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<DemoRuKuDanModel>(context) { 
                CurrentContext = context,
                Action = context.Request["action"],
                KeyId = PublicMethod.GetInt( context.Request["keyid"])
            };

            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<DemoRuKuDanModel>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            {
                case "add":
                    context.Response.Write(DemoRuKuDanBll.Instance.Add(rpm.Entity));
                    break;
                case "edit":
                    DemoRuKuDanModel d = new DemoRuKuDanModel();
                    d.InjectFrom(rpm.Entity);
                    d.KeyId = rpm.KeyId;
                    context.Response.Write(DemoRuKuDanBll.Instance.Update(d));
                    break;
                case "delete":
                    context.Response.Write(DemoRuKuDanBll.Instance.Delete(rpm.KeyId));
                    break;
                case "mx":
                    var str = new SqlFilter(GroupOp.AND.ToString(), new FilterRule("rkdid", rpm.KeyId, "eq"));

                    context.Response.Write(
                        DemoRuKuDanMingXiBll.Instance.GetJson(rpm.Pageindex, rpm.Pagesize,
                        str.ToString(), rpm.Sort, rpm.Order)
                        );
                    break;
                default:
                    context.Response.Write(DemoRuKuDanBll.Instance.GetJson(rpm.Pageindex, rpm.Pagesize, rpm.Filter, rpm.Sort, rpm.Order));
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