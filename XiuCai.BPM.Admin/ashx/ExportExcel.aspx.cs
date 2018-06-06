using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xiucai.BPM.Core;
using Xiucai.Common;
using System.Data;
using Xiucai.Common.Data;
using Xiucai.Common.Data.Filter;

namespace KaoQin.Web.ajax
{
    public partial class ExportExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SysVisitor.Instance.IsGuest)
            {
                string fields = PublicMethod.GetString(Request["fields"]);
                string filters = PublicMethod.GetString(Request["filters"]);
                string tableName = PublicMethod.GetString(Request["tableName"]);
               

                var pcp = new ProcCustomPage(tableName)
                {
                    ShowFields = fields,
                    PageIndex = 1,
                    PageSize = 9999999,
                    OrderFields = "",
                    WhereString = FilterTranslator.ToSql(filters)
                };
                int recordCount;
                DataTable dt = DbUtils.GetPageWithSp(pcp, out recordCount);
                

                GridViewExportUtil.Export(DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls", dt);
            }
            else
            {
                Response.Write("<h1>没有登录啊，你懂的！</h1>");
            }
        }
    }
}