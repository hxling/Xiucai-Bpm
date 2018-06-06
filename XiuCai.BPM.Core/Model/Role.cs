using System.Collections.Generic;
using Xiucai.Common.Data;
using System.ComponentModel;
using Xiucai.BPM.Core.Dal;
namespace Xiucai.BPM.Core.Model
{
    [TableName("sys_roles")]
    [Description("角色管理")]
    public class Role
    {
        [DefaultValue(0)]
        public int KeyId { get; set; }

        [Description("角色名称")]
        public string RoleName { get; set; }

        [DefaultValue(0)]
        [Description("排序")]
        public int Sortnum { get; set; }
        [Description("描述")]
        public string Remark { get; set; }

        [Description("是否为默认角色")]
        public int IsDefault { get; set; }

        [DbField(false)]
        public IEnumerable<Navigation> Navigations { get; set; }

        [DbField(false)]
        public IEnumerable<User> Users { get; set; }

        
        /// <summary>
        /// 角色可以访问的部门列表
        /// </summary>
        [DbField(false)]
        public string Departments
        {
            get { return RoleDal.Instance.GetDepIDs(KeyId); }
        } 
    }
}
