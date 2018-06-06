using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Common;
using Xiucai.Common.Data;
namespace Xiucai.BPM.Core.Model
{
    [TableName("Sys_LogDetails")]
    public class LogDetailModel
    {
        public int KeyId { get; set; }
        /// <summary>
        /// LOG id
        /// </summary>
        public int LogId { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string FieldText { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }
        /// <summary>
        /// 其他信息
        /// </summary>
        public string Remark { get; set; }

        public override string ToString()
        {
            return JSONhelper.ToJson(this);
        }
    }
}
