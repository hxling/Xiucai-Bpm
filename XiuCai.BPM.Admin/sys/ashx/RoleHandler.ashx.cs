using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.Common;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Omu.ValueInjecter;
using Xiucai.BPM.Core.Bll;

namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// Summary description for RoleHandler
    /// </summary>
    public class RoleHandler : IHttpHandler,IRequiresSessionState
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

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<Role>(context) { CurrentContext = context, Action = context.Request["action"] };
            if(!string.IsNullOrEmpty(json))
            {        
                rpm = JSONhelper.ConvertToObject<RequestParamModel<Role>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            {
                case "add":
                    context.Response.Write(RoleBll.Instance.Add(rpm.Entity));
                    break;
                case "edit":
                    var r = new Role();
                    r.InjectFrom(rpm.Entity);
                    r.KeyId = rpm.KeyId;
                    context.Response.Write(RoleBll.Instance.Update(r));
                    break;
                case "delete":
                    context.Response.Write(RoleBll.Instance.Delete(rpm.KeyId));
                    break;
                case "btnColumns":
                    context.Response.Write("var btns = "+RoleBll.Instance.BuildNavBtnsColumns());
                    break;
                case "authorize": //给角色授权
                    var data = rpm.Request("data");
                    if(string.IsNullOrEmpty(data))
                    {
                        context.Response.Write("参数错误！");
                        context.Response.End();
                    }

                    k = RoleBll.Instance.RoleAuthorize(data);
                    context.Response.Write(k);
                    break;
                case "menus":
                    context.Response.Write(RoleBll.Instance.GetRoleNavBtns(rpm.KeyId));
                    break;
                case "deps": //获取所有部门JSON数据
                    context.Response.Write(DepartmentBll.Instance.GetDepartmentTreeJson());
                    break;
                case "setdep":
                    var roleid = PublicMethod.GetInt(context.Request["keyid"]);
                    var deps = context.Request["deps"];
                    k = RoleBll.Instance.SetDepartments(roleid, deps);
                    context.Response.Write(k);
                    break;
                default:
                    context.Response.Write(RoleDal.Instance.JsonDataForEasyUIdataGrid(rpm.Pageindex,rpm.Pagesize,"",rpm.Sort,rpm.Order));
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