using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Dal;
using Xiucai.Common.Provider;
using Xiucai.Common;
using Xiucai.BPM.Core.Model;
namespace Xiucai.BPM.Core.Bll
{
    public class DepartmentBll
    {
        public  static  DepartmentBll Instance
        {
            get { return SingletonProvider<DepartmentBll>.Instance; }
        }

        private IEnumerable<object> GetDepartmentTreeNodes(int parentid = 0)
        {
            var nodes = DepartmentDal.Instance.GetChildren(parentid);
            var treeNodes = from n in nodes
                            orderby n.Sortnum ascending
                            select
                                new {id = n.KeyId, text = n.DepartmentName, children = GetDepartmentTreeNodes(n.KeyId)};
            return treeNodes;
        }

        /// <summary>
        /// 获取部门数据
        /// </summary>
        /// <returns></returns>
        public string GetDepartmentTreeJson()
        {
            var nodes = GetDepartmentTreeNodes();
            return JSONhelper.ToJson(nodes);
        }

        public string GetDepartmentTreegridData()
        {
            return JSONhelper.ToJson(DepartmentDal.Instance.GetChildren());
        }

        public bool HasDepartmentBy(string departmentName,int depid=0)
        {
            var departments = DepartmentDal.Instance.GetAll().ToList();
            return departments.Any(n => n.DepartmentName == departmentName && n.KeyId!=depid);
        }

        public string AddNewDepartment(Department dep)
        {
            int k = 0;
            string msg = "添加失败！";
            if (HasDepartmentBy(dep.DepartmentName))
                msg = "部门名称已存在！";
            else
            {
                k = DepartmentDal.Instance.Insert(dep);
                if(k>0)
                {
                    msg = "添加成功。";
                    LogBll<Department> log = new LogBll<Department>();
                    dep.KeyId = k;
                    log.AddLog(dep);
                }
            }

            return new JsonMessage {Data = k.ToString(), Message = msg, Success = k > 0}.ToString();
        }

        public string EditDepartment(Department dep)
        {
            string msg = "修改失败。";
            int k = 0;
            var oldDep = DepartmentDal.Instance.Get(dep.KeyId);
            if(HasDepartmentBy(dep.DepartmentName,dep.KeyId))
                msg = "部门名称已存在。";
            else
            {
                k = DepartmentDal.Instance.Update(dep);
                if(k>0)
                {
                    msg = "修改成功。";
                    LogBll<Department> log = new LogBll<Department>();
                    log.UpdateLog(oldDep,dep);
                }
            }

            return new JsonMessage {Data = k.ToString(), Message = msg, Success = k > 0}.ToString();
        }

        public string DeleteDepartment(int depid)
        {
            int k = 0;
            string msg = "删除失败";
            var dep = DepartmentDal.Instance.Get(depid);

            if (UserDal.Instance.GetUsersCountByDepartment(depid) > 0)
            {
                msg = "部门中有员工数据不能删除！";
            }
            else if (dep.children.Any())
                msg = "有下级部门数据，不能删除。";
            else
            {
                k = DepartmentDal.Instance.Delete(depid);
                if (k > 0)
                {
                    msg = "删除成功。";
                    LogBll<Department> log = new LogBll<Department>();
                    log.DeleteLog(dep);
                }
            }

            return new JsonMessage { Data = k.ToString(), Message = msg, Success = k > 0 }.ToString();
        }


       
    }
}
