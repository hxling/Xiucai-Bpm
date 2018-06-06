using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xiucai.Common.Provider;
using Xiucai.BPM.Core.Dal;
using Xiucai.Common;

using Newtonsoft.Json.Linq;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Data.SqlServer;
using System.Data;

namespace Xiucai.BPM.Core.Bll
{
    public class RoleBll
    {
        public static RoleBll Instance
        {
            get { return SingletonProvider<RoleBll>.Instance; }
        }

        public bool HasRoleName(string roleName,int roleId = 0)
        {
            var allRoles = from n in RoleDal.Instance.GetAll()
                           where n.RoleName == roleName && n.KeyId != roleId
                           select n;
            return allRoles.Any();
        }

        public int SetDefaultRole(int roleid)
        {
            return RoleDal.Instance.SetDefaultRole(roleid);
        }


        public string Add(Role r)
        {
            string msg = "添加失败";
            if (HasRoleName(r.RoleName))
            {
                msg = "角色名称已存在。";
            }
            int k = RoleDal.Instance.Insert(r);
            if(k > 0 )
            {
                msg = "添加成功";
                LogBll<Role> log = new LogBll<Role>();
                r.KeyId = k;
                log.AddLog(r);
            }
            if (r.IsDefault == 1)
                SetDefaultRole(r.KeyId);
            return new JsonMessage {Success = true, Data = k.ToString(), Message = msg}.ToString();
        }

        public string Update(Role r)
        {
            string msg = "编辑失败";
            if (HasRoleName(r.RoleName, r.KeyId))
                msg = "角色名称已存在。";
            Role old = RoleDal.Instance.Get(r.KeyId);
            int k = RoleDal.Instance.Update(r);
            if(k > 0)
            {
                msg = "编辑成功";
                LogBll<Role> log = new LogBll<Role>();
                log.UpdateLog(old, r);
            }

            if (r.IsDefault == 1)
                SetDefaultRole(r.KeyId);

            return new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString();
        }

        private bool HasUsers(int roleid)
        {
            List<User> users = UserDal.Instance.GetUsers(roleid);
            return users.Count > 0;
        }

        public string Delete(int roleid)
        {
            string msg = "删除失败。";
            //判断是否有用户在使用该角色
            if (HasUsers(roleid))
            {
                msg = "该角色使用中，不能删除！";
                return new JsonMessage { Success = true, Data = "0", Message = msg }.ToString();
            }

            var r = RoleDal.Instance.Get(roleid);

            //先删除角色中分配的权限
            SqlEasy.ExecuteNonQuery("delete Sys_RoleNavBtns where roleid=@roleid", new SqlParameter("@roleid", roleid));
            int k = RoleDal.Instance.Delete(roleid);
            if(k > 0 )
            {
                msg = "删除成功。";
                LogBll<Role> log = new LogBll<Role>();
                log.DeleteLog(r);
            }
            return new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString();
        }

        /// <summary>
        /// 创建treegrid的所有按钮列
        /// </summary>
        /// <returns></returns>
        public string BuildNavBtnsColumns()
        {
            var list = ButtonDal.Instance.GetAll();
            var json = from n in list
                       orderby n.Sortnum ascending
                       select new {title = n.ButtonText, field = n.ButtonTag, width = 60,align="center",
                                   editor = new { type = "checkbox", options = new { @on = "√", off = "x" } }
                       };
            return JSONhelper.ToJson(json);
        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="navJsonData">导航菜单、按钮数据</param>
        /// <returns></returns>
        public int RoleAuthorize(string navJsonData)
        {
            JObject jobj = JObject.Parse(navJsonData);
            var buttons = ButtonDal.Instance.GetAll().ToList();
            var roleid = jobj["roleId"];
            var menus = jobj["menus"];
            var navs = menus.Select(menu => new{
                                        navid = menu["navid"],
                                        btns = buttons.Where(n =>
                                                menu["buttons"].Select(m => (string) m).Contains<string>(n.ButtonTag)
                                                ).Select(k => k)
                        });
            const string sql = "insert into Sys_RoleNavBtns(roleid,navid,btnid) values ({0},{1},{2})";
            var sb = new StringBuilder();

            foreach (var nav in navs )
            {
                foreach (var btn in nav.btns)
                {
                    sb.AppendFormat(sql, roleid, nav.navid, btn.KeyId);
                    sb.AppendLine();
                }
            }

            SqlEasy.ExecuteNonQuery("delete sys_roleNavBtns where roleid=@roleid", 
                                                        new SqlParameter("@roleid", (int)roleid));

            return !string.IsNullOrEmpty(sb.ToString()) ? SqlEasy.ExecuteNonQuery( sb.ToString()) : 0;
        }


        public JArray GetRoleNavBtns(IEnumerable<DataRow> btns, int parentId)
        {
            var navList = NavigationDal.Instance.GetList(parentId);

            var dataRows = btns as DataRow[] ?? btns.ToArray();
            var navigations = navList as Navigation[] ?? navList.ToArray();
            
            JArray jArr = new JArray();
            foreach (var n in navigations)
            {
                var jobj = new JObject(new JProperty("KeyId", n.KeyId),
                                       new JProperty("NavTitle", n.NavTitle),
                                       new JProperty("iconCls", n.iconCls),
                                       new JProperty("Buttons", new JArray(from b in n.Buttons
                                                                           select new JValue(b.ButtonTag))),
                                       new JProperty("children", GetRoleNavBtns(dataRows, n.KeyId)));

                var n1 = n;

                var navbtns = dataRows.Where(b => (int) b["navid"] == n1.KeyId).Select(c=>c["ButtonTag"]).ToArray();

                foreach (var button in ButtonDal.Instance.GetAll())
                    jobj.Add(new JProperty(button.ButtonTag, navbtns.Contains(button.ButtonTag) ? "√" : "x"));

                jArr.Add(jobj);
            }
            return jArr;
        }


        public  string GetRoleNavBtns(int roleid)
        {
            DataTable dt = RoleDal.Instance.GetNavBtnsBy(roleid);
            return GetRoleNavBtns(dt.AsEnumerable(), 0).ToString().Replace("icon ","");
        }

        /// <summary>
        /// 设置数据访问权限
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="deps">部门IDs</param>
        /// <returns></returns>
        public int SetDepartments(int roleid, string deps)
        {
            RoleDal.Instance.ClearDepartmentsBy(roleid);
            if (string.IsNullOrEmpty(deps))
                return 1; //取消数据访问权限
            return RoleDal.Instance.SetDepartments(roleid, deps);
        }
    }
}
