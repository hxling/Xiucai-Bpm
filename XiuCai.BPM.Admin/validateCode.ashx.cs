using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xiucai.BPM.Admin
{
    /// <summary>
    /// validateCode 的摘要说明
    /// </summary>
    public class validateCode : Xiucai.Common.ValidateCode.VcodePage
    {

        public void ProcessRequest(HttpContext context)
        {
            base.BuildVcode();
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