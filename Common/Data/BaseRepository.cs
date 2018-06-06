using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xiucai.Common.Data.Filter;

namespace Xiucai.Common.Data
{
    public class BaseRepository<T> :IRepository<T> where T:new ()
    {

        public T Get(int id)
        {
            return DbUtils.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbUtils.GetAll<T>();
        }



        /// <summary>
        /// 执行SQL语句并返回指定类型的列表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="whereParam">条件参数</param>
        /// <returns></returns>
        public  IEnumerable<T> GetList(string sql,object whereParam)
        {
            return DbUtils.GetList<T>(sql, whereParam);
        }

        public virtual int Insert(T o)
        {
            return DbUtils.Insert(o);
        }

        public virtual int Update(T o)
        {
            return DbUtils.Update(o);
        }

        public virtual int UpdateWhatWhere(object what, object where)
        {
            return DbUtils.UpdateWhatWhere<T>(what, where);
        }

        public virtual int InsertNoIdentity(T o)
        {
            return DbUtils.InsertNoIdentity(o);
        }

        public  IEnumerable<T> GetPage(int page, int pageSize,string sort="keyid desc")
        {
            return DbUtils.GetPage<T>(page, pageSize,sort);
        }

        public IEnumerable<T> GetPage(int page, int pageSize,string sort,object where)
        {
            return DbUtils.GetPage<T>(page, pageSize,sort,where);
        }

        public int Count()
        {
            return DbUtils.Count<T>();
        }

        public IPageable<T> GetPageable(int page, int pageSize)
        {
            return new Pageable<T>
            {
                Rows = GetPage(page, pageSize),
                PageCount = DbUtils.GetPageCount(pageSize, Count()),
                PageIndex = page,
            };
        }

        public IEnumerable<T> GetWhere(object where)
        {
            return DbUtils.GetWhere<T>(where);
        }

        public int Delete(int id)
        {
            return DbUtils.Delete<T>(id);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">格式如： 1,2,4,6,8....</param>
        /// <returns></returns>
        public int Delete(string ids)
        {
            return DbUtils.Delete<T>(ids);
        }

        public int Delete()
        {
            return DbUtils.Delete<T>();
        }

        public int CountWhere(object where)
        {
            return DbUtils.CountWhere<T>(where);
        }

        public DataTable GetPageWithSp(ProcCustomPage pcp,out int recordCount)
        {
            return DbUtils.GetPageWithSp(pcp, out recordCount);
        }


        public virtual string JsonDataForEasyUIdataGrid(int pageindex, int pagesize)
        {
            IEnumerable<T> list = this.GetPage(pageindex,pagesize);
            
            int recordcount = this.Count();

            return JSONhelper.FormatJSONForEasyuiDataGrid(recordcount, list);
            
        }

        public string JsonDataForEasyUIdataGrid(string tablename, int pageindex, int pagesize, string filterJson, string sort = "keyid", string order = "asc")
        {
            string sortorder = sort + " " + order;

            var pcp = new ProcCustomPage(tablename)
            {
                PageIndex = pageindex,
                PageSize = pagesize,
                OrderFields = sortorder,
                WhereString = FilterTranslator.ToSql(filterJson)
            };
            int recordCount;
            DataTable dt = GetPageWithSp(pcp, out recordCount);
            return JSONhelper.FormatJSONForEasyuiDataGrid(recordCount, dt);

        }


        public virtual string JsonDataForjQgrid(int pageindex, int pagesize)
        {
            IPageable<T> page = this.GetPageable(pageindex, pagesize);
            int recordcount = this.Count();
            return JSONhelper.FormatJSONForJQgrid(page.PageCount, pageindex, recordcount, page.Rows);
        }
    }
}
