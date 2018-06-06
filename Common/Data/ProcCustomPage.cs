using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Data
{
    public class ProcCustomPage
    {


        public ProcCustomPage() 
        {
            ShowFields = "*";
            KeyFields = "keyid";
            OrderFields = "keyid desc";
            PageIndex = 1;
            PageSize = 20;
            WhereString = "";
            Sp_PagerName = "ProcCustomPage";
        }

        public string Sp_PagerName
        {
            get;
            set;
        }

        public ProcCustomPage(string tablename) :this()
        {
            TableName = tablename;
        }
        
        /// <summary>
        /// 表名或视图名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 查询字段
        /// </summary>
        public string ShowFields { get; set; }

        /// <summary>
        /// 主键或标识字段
        /// </summary>
        public string KeyFields { get; set; }

        /// <summary>
        /// 排序字段 如：keyid desc,name asc
        /// </summary>
        public string OrderFields { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string WhereString { get; set; }
    }
}
