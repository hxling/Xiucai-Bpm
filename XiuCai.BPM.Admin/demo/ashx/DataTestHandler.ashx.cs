using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Omu.ValueInjecter;
using XiuCai.Demo;
using Xiucai.BPM.Core;
using Xiucai.Common;
using Xiucai.Common.Data;
using Xiucai.Common.Data.Filter;

namespace Xiucai.BPM.Admin.demo.ashx
{
    /// <summary>
    /// DataTestHandler 的摘要说明
    /// </summary>
    public class DataTestHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            context.Response.ContentType = "text/plain";

            if (SysVisitor.Instance.IsGuest)
            {
                context.Response.Write(
                    new JsonMessage { Success = false, Data = "-99", Message = "登录已过期，请重新登录" }.ToString()
                    );
                context.Response.End();
            }

            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<DemoMember>(context) { CurrentContext = context };
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<DemoMember>>(json);
                rpm.CurrentContext = context;
            }
            int k = 0;
            switch (rpm.Action)
            {
                case "add":
                    var b = new DemoMember();
                    b.InjectFrom(rpm.Entity);
                    b.Ownner = SysVisitor.Instance.UserId; //当前用户ID
                    b.DepID = SysVisitor.Instance.CurrentUser.DepartmentId; //当前用户所在的部门
                    k=DemoMemberDal.Instance.Insert(b);
                    context.Response.Write(new JsonMessage{Data = k.ToString(),Message = "添加成功",Success = true}.ToString());
                    break;
                case "edit":
                    var p = new DemoMember();
                    p.InjectFrom(rpm.Entity);
                    p.KeyID = rpm.KeyId;
                    k = DemoMemberDal.Instance.Update(p);
                    context.Response.Write(new JsonMessage { Data = k.ToString(), Message = "编辑成功", Success = true }.ToString());
                    break;
                case "delete":
                    k = DemoMemberDal.Instance.Delete(rpm.KeyId);
                    context.Response.Write(new JsonMessage { Data = k.ToString(), Message = "删除成功", Success = true }.ToString());
                    break;
                default:
                    context.Response.Write(JsonDataForEasyUIdataGrid(rpm.Pageindex, rpm.Pagesize, rpm.Filter));
                    break;
            }
        }


        private string JsonDataForEasyUIdataGrid(int pageindex, int pagesize, string filterJSON)
        {
            //数据权限条件
            int userid = SysVisitor.Instance.UserId;
            string deps = SysVisitor.Instance.Departments;

            string where = "ownner=" + userid + " and depid in (" + deps + ")";

            if (SysVisitor.Instance.IsAdmin)
                where = "";
            else
            {
                if (filterJSON != "")
                {
                    where = FilterTranslator.ToSql(filterJSON) + " and " + where;
                }
            }
            var pcp = new ProcCustomPage("demo_users")
            {
                PageIndex = pageindex,
                PageSize = pagesize,
                OrderFields = "keyid asc",
                WhereString = where
            };
            int recordCount;
            DataTable dt = DbUtils.GetPageWithSp(pcp, out recordCount);
            return JSONhelper.FormatJSONForEasyuiDataGrid(recordCount, dt);
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