using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Xiucai.BPM.Core.Model
{
    /// <summary>
    /// 用户个性化设置
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// 皮肤名称
        /// </summary>
        [Description("皮肤")]
        public Theme Theme { get; set; }

        /// <summary>
        /// 表现方式
        /// </summary>
        public string ShowType { get; set; }

        /// <summary>
        /// Grid 每页显示记录数
        /// </summary>
        public int GridRows { get; set; }

        public bool showValidateCode { get; set; }

    }

    public class Theme
    {
        /// <summary>
        /// 主题名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 主题路径名称
        /// </summary>
        public string Name { get; set; }
    }
}
