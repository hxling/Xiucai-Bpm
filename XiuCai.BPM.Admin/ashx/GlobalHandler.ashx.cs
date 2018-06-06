using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xiucai.Common;

namespace Xiucai.BPM.Admin.ashx
{
    /// <summary>
    /// GlobalHandler 的摘要说明
    /// </summary>
    public class GlobalHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var showValidateCode = ConfigHelper.GetValue("showValidateCode");
            context.Response.Write("var showValidateCode = "+showValidateCode + ";");
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