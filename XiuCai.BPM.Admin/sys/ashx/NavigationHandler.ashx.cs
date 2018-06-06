using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Xiucai.Common;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Omu.ValueInjecter;
namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// Summary description for NavigationHandler
    /// </summary>
    public class NavigationHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            UserBll.Instance.CheckUserOnlingState();

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<Navigation>(context) { CurrentContext = context,Action = context.Request["action"]};
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<Navigation>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            { 
                case "btns":
                    context.Response.Write(NavigationBll.Instance.GetAllButtons());
                    break;
                case "add":
                    context.Response.Write(NavigationBll.Instance.AddNewNav(rpm.Entity));
                    break;
                case "edit":

                    if(rpm.KeyId == rpm.Entity.ParentID)
                    {
                        context.Response.Write("上级菜单不能与当前菜单相同。");
                        context.Response.End();
                    }

                    var nav = new Navigation();
                    nav.InjectFrom(rpm.Entity);
                    nav.KeyId = rpm.KeyId;
                    context.Response.Write(NavigationBll.Instance.EditNav(nav));
                    break;
                case "delete":
                    context.Response.Write(NavigationBll.Instance.DeleteNav(rpm.KeyIds));
                    break;
                case "setbtns":
                    k = NavigationBll.Instance.SetNavButtons(rpm.KeyId, rpm.KeyIds);
                    context.Response.Write(k);
                    break;
                case "buildIcon":
                    string path = context.Server.MapPath("~/css/icon/32/");
                    string[] files = Directory.GetFiles(path);

                    FileInfo fileinfo;
                    StringBuilder sb = new StringBuilder();
                    foreach (string file in files)
                    {
                        fileinfo = new FileInfo(file);
                        sb.AppendFormat("<li title=\"{0}\"><img src=\"{0}\"/></li>", "/css/icon/32/" + fileinfo.Name);
                        sb.AppendLine();
                    }
                    context.Response.Write(sb.ToString());
                    break;
                default:
                    context.Response.Write(NavigationBll.Instance.BuildNavTreeJSON());
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