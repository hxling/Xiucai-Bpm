using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xiucai.Common;
using Xiucai.Common.Data;
using Xiucai.Common.Data.SqlServer;
using Xiucai.Common.Provider;
using Xiucai.BPM.Core.Model;
namespace Xiucai.BPM.Core.Dal
{
    public class UserDal : BaseRepository<User>
    {
        public  static UserDal Instance
        {
            get { return SingletonProvider<UserDal>.Instance; }
        }

        public int UpdateUserConfig(int userId,string configJson)
        {
            string sql = "update sys_users set configJson=@configJson where keyid=@keyid";
            return DbUtils.ExecuteNonQuery(sql, new { ConfigJson = configJson, keyid = userId });
        }

        public override int Update(User o)
        {
            return DbUtils.Update(o, new[] {"keyid", "password", "passsalt","configjson"});
        }

        public int UpdatePassword(int userId,string password)
        {
            string sql = "update sys_users set password=@password where keyid=@keyid";
            return DbUtils.ExecuteNonQuery(sql, new {Password = password, keyid = userId});
        }

        public IEnumerable<Role> GetRolesBy(int userId)
        {
            string s = "select roleid from Sys_UserRoles where userid=@userid";
            DataTable dt = SqlEasy.ExecuteDataTable(s, new SqlParameter("@userid", userId));

            var list = from n in RoleDal.Instance.GetAll()
                       where dt.AsEnumerable().Select(r => PublicMethod.GetInt(r[0])).ToArray<int>().Contains(n.KeyId)
                       select n;
            return list;
        }

        public User GetUserBy(string userName)
        {
            return base.GetAll().ToList().Find(n => n.UserName == userName);
        }

        /// <summary>
        /// 为指定的用户分配角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID</param>
        /// <returns></returns>
        public int AddUserTo(int userId,params int[] roleIds)
        {
            string sql = "insert into Sys_UserRoles (userid,roleid) values({0},{1})";
            StringBuilder sb = new StringBuilder();
            foreach (var rid in roleIds)
            {
                sb.AppendFormat(sql, userId, rid);
                sb.AppendLine();
            }

            if(!string.IsNullOrEmpty(sb.ToString()))
            {
                return SqlEasy.ExecuteNonQuery(sb.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 删除用户相关的数据，角色、权限、所在的用户部门对应记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public int DeleteRolesBy(int userId)
        {
            DbUtils.ExecuteNonQuery("delete Sys_UserNavBtns where userid=@userid;delete Sys_Users_Departments where userid=@userid;", new { userid = userId });
            return DbUtils.ExecuteNonQuery("delete sys_userroles where userid=@userid", new {userid = userId});
        }

        /// <summary>
        /// 获取指定用户的菜单及按钮
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public DataTable GetNavBtnsBy(int userId)
        {
            string sql =
                "select a.*,b.ButtonTag from Sys_UserNavBtns a join sys_buttons b on a.btnid=b.keyid where a.userid=@userid";
            return SqlEasy.ExecuteDataTable(sql, new SqlParameter("@userid", userId));
        }


        #region 为用户设置数据访问权限

        public int ClearDepartmentsBy(int userid)
        {
            string sql = "delete Sys_Users_Departments where userid=@UserID";
            return DbUtils.ExecuteNonQuery(sql, new { UserID = userid });
        }

        public int SetDepartments(int userid, string deps)
        {
            if (string.IsNullOrEmpty(deps))
                return 0;

            string[] arrDep = deps.Split(',');

            string sql = "insert into Sys_Users_Departments (userid,depid) values({0},{1}) ";
            StringBuilder sb = new StringBuilder();
            foreach (string depid in arrDep)
            {
                sb.AppendFormat(sql, userid, depid);
                sb.AppendLine();
            }

            return sb.Length > 0 ? SqlEasy.ExecuteNonQuery(sb.ToString()) : 0;
        }


        public List<int> GetDepIDs(int userid)
        {
            List<int> list = new List<int>();
            var dr = SqlEasy.ExecuteDataReader("select depid from Sys_Users_Departments where userid=@UserID", new SqlParameter("@UserID", userid));
            while (dr.Read())
            {
                list.Add(PublicMethod.GetInt(dr[0]));
            }
            return list;
        }


        #endregion

        public List<User> GetUsers(int roleid)
        {
            string sql = "select * from Sys_UserRoles where roleid=@Roleid";
            return DbUtils.ExecuteReader<User>(sql, new {Roleid = roleid}).ToList();
        }

        public int GetUsersCountByDepartment(int depid)
        {
            string sql = "select count(*) from Sys_Users_Departments where depid=@depid";
            return PublicMethod.GetInt(SqlEasy.ExecuteScalar(sql, new SqlParameter("@depid", depid)));
        }
    }
}
