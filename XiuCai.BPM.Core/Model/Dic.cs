using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Xiucai.BPM.Core.Dal;
using Xiucai.Common.Data;

namespace Xiucai.BPM.Core.Model
{
    [TableName("Sys_Dics")]
    [Description("数据字典")]
    public class Dic
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键")]
        public int KeyId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Title { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Description("编码")]
        public string Code { get; set; }
        /// <summary>
        /// 类别ID
        /// </summary>
        [Description("类别ID")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Description("上级Id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int Sortnum { get; set; }
        /// <summary>
        /// 描述说明
        /// </summary>
        [Description("描述")]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 是否默认值 0 和 1
        /// </summary>
        [Description("是否默认值")]
        public int IsDefault { get; set; }

        [DbField(false)]
        public DicCategory Category { get; set; }

        [DbField(false)]
        public IEnumerable<Dic> children
        {
            get { return DicDal.Instance.GetListBy(CategoryId,KeyId); }
        }

        [DbField(false)]
        public bool selected
        {
            get { return IsDefault == 1; }
        }


    }
}
