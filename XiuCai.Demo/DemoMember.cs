using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xiucai.Common.Data;

namespace XiuCai.Demo
{
    //测试数据
    [TableName("demo_users")]
    [Description("会员信息")]
    public class DemoMember
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int KeyID { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 会员单位名称
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 隶属用户
        /// </summary>
        public int Ownner { get; set; }
        /// <summary>
        /// 隶属部门
        /// </summary>
        public int DepID { get; set; }

        public override string ToString()
        {
            return Xiucai.Common.JSONhelper.ToJson(this);
        }
        

    }
}
