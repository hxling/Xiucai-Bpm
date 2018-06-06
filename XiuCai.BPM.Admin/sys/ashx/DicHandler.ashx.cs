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
using Omu.ValueInjecter;
namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// DicHandler 的摘要说明
    /// </summary>
    public class DicHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            if(SysVisitor.Instance.IsGuest)
            {
                context.Response.Write(
                    new JsonMessage { Success = false, Data = "-99", Message = "登录已过期，请重新登录" }.ToString()
                    );
                context.Response.End();
            }

            int k;
            var json = HttpContext.Current.Request["json"];
            var rpm = new RequestParamModel<Dic>(context) { CurrentContext = context, Action = context.Request["action"]};
            if (!string.IsNullOrEmpty(json))
            {
                rpm = JSONhelper.ConvertToObject<RequestParamModel<Dic>>(json);
                rpm.CurrentContext = context;
            }

            switch (rpm.Action)
            {
                case "category": //读取字典类别
                    context.Response.Write(DicBll.Instance.DicCategoryJson());
                    break;
                case "add_cate": //添加字典类别
                    var dc = new DicCategory{
                        Code = rpm.Request("code"), 
                        Title = rpm.Request("title"),
                        Sortnum = PublicMethod.GetInt(rpm.Request("sortnum")), 
                        Remark = rpm.Request("remark")
                    };
                    AddCategory(dc, context);
                    break;
                case "edit_cate":
                    dc = new DicCategory{
                        KeyId =PublicMethod.GetInt(rpm.Request("keyid")),
                        Code = rpm.Request("code"), 
                        Title = rpm.Request("title"),
                        Sortnum = PublicMethod.GetInt(rpm.Request("sortnum")), 
                        Remark = rpm.Request("remark")
                    };
                    EditCategory(dc, context);
                    break;
                case "del_cate":
                    var cateId = PublicMethod.GetInt(rpm.Request("cateId"));
                    DelCategory(cateId, context);
                    break;
                case "add":
                    k = DicBll.Instance.Add(rpm.Entity);
                    context.Response.Write(new JsonMessage { Success = k > 0, Data = k.ToString(), Message = (k > 0 ? "添加成功！" : "字典编码已存在,请更改编码。") }.ToString());
                    break;
                case "edit":
                    if(rpm.KeyId == rpm.Entity.ParentId)
                    {
                        context.Response.Write(new JsonMessage { Success = false, Data = "0", Message = "上级字典不能与当前字典相同！" }.ToString());
                        context.Response.End();
                    }

                    Dic d = new Dic();
                    d.InjectFrom(rpm.Entity);
                    d.KeyId = rpm.KeyId;
                    k = DicBll.Instance.Edit(d);
                    context.Response.Write(new JsonMessage { Success = k>0, Data = k.ToString(), Message = (k > 0 ? "编辑成功！" : "字典编码已存在,请更改编码。") }.ToString());
                    break;
                case "del":
                    k = DicBll.Instance.Delete(rpm.KeyId);
                    var msg = "删除成功。";
                    
                    switch (k)
                    {
                        case 0:
                            msg = "参数错误！";
                            break;
                        case 2: 
                            msg = "请先删除子字典数据。";
                            break;
                    }

                    context.Response.Write(new JsonMessage { Success = k==1, Data = k.ToString(), Message = msg}.ToString());

                    break;
                default: //字典列表
                    var categoryId = PublicMethod.GetInt(rpm.Request("categoryId"));
                    string dicJson = DicBll.Instance.GetDicListBy(categoryId);
                    context.Response.Write(dicJson);
                    break;
            }


        }

        void AddCategory(DicCategory dc,HttpContext context)
        {
            int k = DicCategoryDal.Instance.Insert(dc);
            var msg = "添加成功。";
            if (k <= 0)
                msg = "添加失败。";
            context.Response.Write(new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString());
        }

        void EditCategory(DicCategory dc,HttpContext context)
        {
            int k = DicCategoryDal.Instance.Update(dc);
            var msg = "编辑成功。";
            if (k <= 0)
                msg = "编辑失败。";
            context.Response.Write(new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString());
        }

        void DelCategory(int cateId,HttpContext context)
        {
            int k = DicCategoryDal.Instance.Delete(cateId);
            var msg = "删除成功。";
            if (k <= 0)
                msg = "删除失败。";
            context.Response.Write(new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString());
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