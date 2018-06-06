using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Common;
using Xiucai.Common.Data;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Data.Filter;
using Xiucai.Common.Provider;
using System.Data;
using System.Data.SqlClient;
using Xiucai.Common.Data.SqlServer;

namespace Xiucai.BPM.Core.Dal
{
    public class RoleDal : BaseRepository<Role>
    {

        public static RoleDal Instance
        {
            get { return SingletonProvider<RoleDal>.Instance; }
        }

        public DataTable GetNavBtnsBy(params int[] roleid)
        {
            var roleids = roleid.Aggregate("", (current, i) => current + (i.ToString() + ",")).TrimEnd(',');
            string sql = "select a.*,b.ButtonTag from sys_roleNavBtns a join sys_buttons b on a.btnid=b.keyid where a.roleid in ("+roleids+")";
            return SqlEasy.ExecuteDataTable(sql);
        }


        public int SetDefaultRole(int roleid)
        {
            DbUtils.ExecuteNonQuery("update sys_roles set isdefault=0", null);
            return DbUtils.ExecuteNonQuery("update sys_roles set isdefault=1 where keyid=@keyid",new {keyid=roleid} );
        }

        public string JsonDataForEasyUIdataGrid(int pageindex, int pagesize, string filterJson, string sort = "keyid", string order = "asc")
        {
            string sortorder = sort + " " + order;

            var pcp = new ProcCustomPage("sys_roles")
            {
                PageIndex = pageindex,
                PageSize = pagesize,
                OrderFields = sortorder,
                WhereString = FilterTranslator.ToSql(filterJson)
            };
            int recordCount;
            DataTable dt = base.GetPageWithSp(pcp, out recordCount);
            dt.Columns.Add(new DataColumn("Departments")); //可以访问的部门数据

            var rolelist = RoleDal.Instance.GetAll();

            foreach (DataRow row in dt.Rows)
            {
                row["Departments"] = rolelist.First(n => n.KeyId == PublicMethod.GetInt(row["KeyId"])).Departments;
            }


            return JSONhelper.FormatJSONForEasyuiDataGrid(recordCount, dt);

        }


        #region 为角色设置数据访问权限
        
        public int ClearDepartmentsBy(int roleid)
        {
            string sql = "delete Sys_Roles_Departments where roleid=@RoleId";
            return DbUtils.ExecuteNonQuery(sql, new {RoleId = roleid});
        }
        
        public int SetDepartments(int roleid, string deps)
        {
            if (string.IsNullOrEmpty(deps))
                return 0;

            string[] arrDep = deps.Split(',');

            string sql = "insert into Sys_Roles_Departments (roleid,depid) values({0},{1}) ";
            StringBuilder sb = new StringBuilder();
            foreach (string depid in arrDep)
            {
                sb.AppendFormat(sql, roleid, depid);
                sb.AppendLine();
            }

            return sb.Length > 0 ? SqlEasy.ExecuteNonQuery(sb.ToString()) : 0;
        }

        public List<Department> GetDepsBy(int roleid)
        {
            string sql =
                "select a.* from Sys_Departments a join Sys_Roles_Departments b on a.depid=b.keyid where b.roleid=@roleid";

            return DbUtils.GetList<Department>(sql,new {RoleID = roleid}).ToList();
        }

        public string GetDepIDs(int roleid)
        {
            string temp = "";
            var dr = SqlEasy.ExecuteDataReader("select depid from Sys_Roles_Departments where roleid=@RoleID",new SqlParameter("@RoleID",roleid));
            while (dr.Read())
            {
                temp += dr.GetInt32(0) + ",";
            }
            return temp.TrimEnd(',');
        }
                                             

        #endregion

    }
}
