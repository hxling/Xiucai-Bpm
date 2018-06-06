using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Dal;
using Xiucai.Common;
using Xiucai.Common.Data;

namespace Xiucai.BPM.Core.Model
{
    [TableName("sys_logs")]
    public class LogModel
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int KeyId { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int OperationType { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
        /// <summary>
        /// 操作表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 操作表中文名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 主键值
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 执行的SQL语句
        /// </summary>
        public string SqlText { get; set; }

        /// <summary>
        /// 操作Ip
        /// </summary>
        public string OperationIp { get; set; }


        [DbField(false)]
        public IEnumerable<LogDetailModel> Details 
        { 
            get
            {
                return LogDetailDal.Instance.GetBy(KeyId);
            }
        }

        public override string ToString()
        {
            return JSONhelper.ToJson(this);
        }
    }


    
}
