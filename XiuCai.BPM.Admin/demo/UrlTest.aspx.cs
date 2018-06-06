using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Xiucai.BPM.Admin.demo
{
    public partial class UrlTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var url = this.Request.Url.ToString();
            var absUrl = this.Request.Url.AbsolutePath;

            this.Response.Write(url);
            Response.Write("<br>");
            this.Response.Write("absolutePath:"+ absUrl);

            
        }
    }
}