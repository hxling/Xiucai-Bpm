using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.BPM.Core;
using Xiucai.Common;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Model;
using Omu.ValueInjecter;
namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// Summary description for UserHandler
    /// </summary>
    public class UserHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UserBll.Instance.CheckUserOnlingState();

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<User>(context) { CurrentContext = context,Action = context.Request["action"]};
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<User>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            {
                case "deps":
                    context.Response.Write(UserBll.Instance.GetDepartmentTreeData());
                    break;
                case "roles":
                    context.Response.Write(UserBll.Instance.GetAllRoles());
                    break;
                case "add":
                    var roleIds = rpm.Request("roles");
                    context.Response.Write(UserBll.Instance.AddUser(rpm.Entity,roleIds));
                    break;
                case "update":
                    User u = new User();
                    u.InjectFrom(rpm.Entity);
                    u.KeyId = rpm.KeyId;
                    
                    context.Response.Write(UserBll.Instance.EditUser(u));
                    break;
                case "editpass":
                    k = UserBll.Instance.EditPassword(rpm.KeyId, rpm.Request("password"));
                    context.Response.Write(k);
                    break;
                case "editpass2":
                    string oldPass = context.Request["old"];
                    string newPass = context.Request["new"];
                    context.Response.Write(UserBll.Instance.EditPassowrd(SysVisitor.Instance.UserId,oldPass,newPass));
                    break;
                case "delete": //删除用户
                    context.Response.Write(UserBll.Instance.DeleteUser(rpm.KeyId));
                    break;
                case "isadmin":
                    u = UserBll.Instance.GetUser(rpm.KeyId);
                    if(u!=null)
                    {
                        var isamdin = rpm.Request("val");
                        u.IsAdmin = isamdin != "true";
                        context.Response.Write(UserBll.Instance.EditUser(u));
                    }
                    else
                    {
                        context.Response.Write(0);
                    }
                    break;
                case "isdisabled": //禁用 激活 帐号
                    u = UserBll.Instance.GetUser(rpm.KeyId);
                    if(u!=null)
                    {
                        var isdisabled = rpm.Request("val");
                        u.IsDisabled = isdisabled!="true";
                        context.Response.Write(UserBll.Instance.EditUser(u));
                    }
                    else
                    {
                        context.Response.Write(0);
                    }
                    break;
                case "setroles": //为用户分配角色
                    var rolse = rpm.Request("roles");
                    k = UserBll.Instance.AddUserToRoles(rpm.KeyId,rolse);
                    context.Response.Write(k);
                    break;
                case "getroles": //获取指定用户的角色
                    context.Response.Write(UserBll.Instance.GetRolesBy(rpm.KeyId));
                    break;
                case "menus": //获取导航菜单及按钮用于用户授权
                    context.Response.Write(UserBll.Instance.GetNavBtnsJson(rpm.KeyId));
                    break;
                case "authorize": //为用户授权
                    var data = rpm.Request("data");
                    if(string.IsNullOrEmpty(data))
                    {
                        context.Response.Write("参数错误！");
                        context.Response.End();
                    }

                    k = UserBll.Instance.UserAuthorize(data);
                    context.Response.Write(k);
                    break;
               
                case "setdep":
                    var roleid = PublicMethod.GetInt(context.Request["keyid"]);
                    var deps = context.Request["deps"];
                    k = UserBll.Instance.SetDepartments(roleid, deps);
                    context.Response.Write(k);
                    break;
                default:
                    string j = UserBll.Instance.GetJsonData(rpm.Pageindex, rpm.Pagesize, rpm.Filter,rpm.Sort,rpm.Order);
                    context.Response.Write(j);
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