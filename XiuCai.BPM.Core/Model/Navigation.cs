using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xiucai.Common.Data;
using Xiucai.BPM.Core.Dal;

namespace Xiucai.BPM.Core.Model
{
    [TableName("sys_Navigations")]
    [Description("导航菜单")]
    public class Navigation
    {
        public int KeyId { get; set; }
        [Description("菜单名称")]
        public string NavTitle { get; set; }
        [Description("链接地址")]
        public string Linkurl { get; set; }
        [Description("排序")]
        public int Sortnum { get; set; }
        [Description("图标CSS")]
        public string iconCls { get; set; }
        [Description("图标URL")]
        public string iconUrl { get; set; }
        [Description("是否显示")]
        public bool IsVisible { get; set; }
        [Description("父ID")]
        public int ParentID { get; set; }
        [Description("菜单标识")]
        public string NavTag { get; set; }
        [Description("大图标路径")]
        public string BigImageUrl { get; set; }

        [Description("是否在新窗口打开")]
        public bool IsNewWindow { get; set; }
        [Description("新窗口宽度")]
        public int WinWidth { get; set; }
        [Description("新窗口高度")]
        public int WinHeight { get; set; }

        [DbField(false)]
        public IEnumerable<Navigation> children
        {
            get
            {
                return NavigationDal.Instance.GetList(KeyId);
            }
        }

        [DbField(false)]
        public IEnumerable<Button> Buttons
        {
            get { return ButtonDal.Instance.GetButtonsBy(KeyId); }
        }

    }
}
