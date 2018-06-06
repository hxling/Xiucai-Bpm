using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Data.Filter
{
    public class FilterRule
    {
        public FilterRule()
        {
        }

        public FilterRule(string field, object data, string op)
        {
            this.field = field;
            this.data = data;
            this.op = op;
        }

        /// <summary>
        /// 字段
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 比较符号
        /// </summary>
        public string op { get; set; }
    }
}
