using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Xiucai.Common.Data;
using Xiucai.Common;
using Xiucai.BPM.Core.Dal;
using System.Collections;
namespace Xiucai.BPM.Core.Model
{
    [TableName("sys_users")]
    [Description("系统用户")]
    public class User
    {
        public int KeyId { get; set; }
        [Description("用户名")]
        public string UserName { get; set; }
        [Description("密码")]
        public string Password { get; set; }
        [Description("真实姓名")]
        public string TrueName { get; set; }
        [Description("密码佐料")]
        public string PassSalt { get; set; }
        
        public string Email { get; set; }
        [Description("是否超管")]
        public bool IsAdmin { get; set; }
        [Description("是否禁用")]
        public bool IsDisabled { get; set; }
        [Description("部门Id")]
        public int DepartmentId { get; set; }
        [Description("手机")]
        public string Mobile { get; set; }
        
        public string QQ { get; set; }
        [Description("备注")]
        public string Remark { get; set; }
        [Description("个性化设置")]
        public string ConfigJson { get; set; }

        [DbField(false)]
        public Department Department
        {
            get { return DepartmentDal.Instance.Get(DepartmentId); }
        }

        [DbField(false)]
        public List<Navigation> Navigations { get; set; }

        [DbField(false)]
        public IEnumerable<Role> Roles
        {
            get { return UserDal.Instance.GetRolesBy(KeyId); }
        }

        /// <summary>
        /// 用户可以访问的部门列表
        /// </summary>
        [DbField(false)]
        public string Departments
        {
            get { return string.Join(",", UserDal.Instance.GetDepIDs(KeyId)); }
        } 

       
        public override string ToString()
        {
            return JSONhelper.ToJson(this);
        }
    }
}
