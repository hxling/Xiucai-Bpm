using System.Web;
using System.Web.SessionState;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Omu.ValueInjecter;
using Xiucai.Common;

namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// Summary description for PermissionHandler
    /// </summary>
    public class ButtonHandler : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if(SysVisitor.Instance.IsGuest)
            {
                context.Response.Write(
                    new JsonMessage{ Success = false,Data = "-99",Message = "登录已过期，请重新登录"}.ToString()
                    );
                context.Response.End();
            }

            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<Button>(context) { CurrentContext = context };
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<Button>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            { 
                case "add":
                    var b = new Button();
                    b.InjectFrom(rpm.Entity);
                    context.Response.Write(ButtonBll.Instance.AddButton(b));
                    break;
                case "edit":
                    var p = new Button();
                    p.InjectFrom(rpm.Entity);
                    p.KeyId = rpm.KeyId;
                    context.Response.Write(ButtonBll.Instance.EditButton(p));
                    break;
                case "delete":
                    context.Response.Write(ButtonBll.Instance.DelButton(rpm.KeyId));
                    break;
                default:
                    context.Response.Write(ButtonDal.Instance.JsonDataForEasyUIdataGrid(rpm.Pageindex, rpm.Pagesize,rpm.Filter,rpm.Sort,rpm.Order));
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