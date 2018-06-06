using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Dal;
using Xiucai.Common;
using Xiucai.Common.Data.SqlServer;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Data;
using Xiucai.Common.Provider;

namespace Xiucai.BPM.Core.Bll
{
    public class NavigationBll
    {
        
        public static NavigationBll Instance
        {
            get { return SingletonProvider<NavigationBll>.Instance; }
        }

        public Navigation GetBy(int navId)
        {
            return NavigationDal.Instance.Get(navId);
        }

        /// <summary>
        /// 构建EasyUI treegrid、tree 所需要的JSON
        /// </summary>
        /// <param name="parentid">上级节点ID</param>
        /// <returns></returns>
        public string BuildNavTreeJSON(int parentid=0)
        {
            return JSONhelper.ToJson(NavigationDal.Instance.GetList(parentid).ToList())
                .Replace("icon ","");
        }

        /// <summary>
        /// 获取所有操作按钮
        /// </summary>
        /// <returns></returns>
        public string GetAllButtons()
        {
            var btns = from n in ButtonDal.Instance.GetAll()
                       orderby n.Sortnum
                       select n;
            return JSONhelper.ToJson(btns.ToList());
        }

        /// <summary>
        /// 是否存在同名的导航菜单
        /// </summary>
        /// <param name="title">菜单名称</param>
        /// <param name="navid">菜单ID。为0表现为新增，否则为编辑菜单</param>
        /// <returns></returns>
        public bool HasNav(string title,int navid=0)
        {
            var list = from n in NavigationDal.Instance.GetAll()
                       where n.NavTitle == title && n.KeyId != navid
                       select n;
            return list.Any();
        }

        public string AddNewNav(Navigation nav)
        {
            string msg = "添加失败.";
            if (HasNav(nav.NavTitle, nav.KeyId))
            {
                msg = "菜单名称已存在.";
            }
            var k = NavigationDal.Instance.Insert(nav);
            //if (k > 0)
            //    SetNavButtons(k, "18");  //添加菜单添加默认权限

            if (k > 0)
            {
                msg = "添加成功。";
                LogBll<Navigation> log = new LogBll<Navigation>();
                nav.KeyId = k;
                log.AddLog(nav);
            }

            return new JsonMessage {Data = k.ToString(), Message = msg, Success = k > 0}.ToString();
        }

        public string EditNav(Navigation nav)
        {
            string msg = "编辑失败。";
            Navigation oldNav = GetBy(nav.KeyId);
            if(HasNav(nav.NavTitle, nav.KeyId))
                msg = "菜单名称已存在。";

            int k = NavigationDal.Instance.Update(nav);
            if (k > 0)
            {
                msg = "编辑成功。";
                LogBll<Navigation> log = new LogBll<Navigation>();
                log.UpdateLog(oldNav,nav);
            }
            return new JsonMessage { Data = k.ToString(), Message = msg, Success = k > 0 }.ToString();
        }

        public string DeleteNav(string navids)
        {
            var oldNavList = from n in NavigationDal.Instance.GetAll().ToList()
                             where navids.Split(',').Contains(n.KeyId.ToString())
                             select n;
            string msg = "删除失败。";
            int k = NavigationDal.Instance.Delete(navids);

            if (k > 0)
            {
                // 删除与导航菜单分配的按钮列表 2013-07-05
                DeleteNavButtons(navids);

                msg = "删除成功。";
                var log = new LogBll<Navigation>();
                foreach (var n in oldNavList)
                {
                    log.DeleteLog(n);    
                }
                
            }

            return new JsonMessage { Data = k.ToString(), Message = msg, Success = k > 0 }.ToString();
        }

        /// <summary>
        /// 设置菜单按钮
        /// </summary>
        /// <param name="navid">菜单ID</param>
        /// <param name="permissions">按钮</param>
        /// <returns></returns>
        public int SetNavButtons(int navid,string permissions)
        {
            const string sql = "insert into Sys_NavButtons (navid,buttonid,sortnum) values({0},{1},{2})";
            if(permissions !="" && navid >0)
            {
                var sb = new StringBuilder();
                var arr = permissions.Split(',');
                int k = 0;
                foreach (var s in arr)
                {
                    sb.AppendFormat(sql, navid, s,k);
                    sb.AppendLine();
                    k++;
                }
                if(string.IsNullOrEmpty(sb.ToString()))
                    throw new Exception("按钮数量为0或菜单ID未找到。");
                
                DeleteNavButtons(navid);
                return SqlEasy.ExecuteNonQuery(sb.ToString());
            }
            return 0;
        }

        private void DeleteNavButtons(int navid)
        {
            const string deleteSql = "delete Sys_NavButtons where navid=@Navid";
            DbUtils.ExecuteNonQuery(deleteSql, new { Navid = navid });
            
        }
        private void DeleteNavButtons(string navids)
        {
            string deleteSql = "delete Sys_NavButtons where navid in ("+navids+")";
            DbUtils.ExecuteNonQuery(deleteSql,null);
        }
    }
}
