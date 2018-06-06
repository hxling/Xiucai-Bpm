using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data;

namespace Xiucai.Common.Data.SQLite
{
    public class SQLiteEasy
    {
        private static string connString;
        public static string ConnectionString
        {
            get { return connString; }
            set { connString = value; }
        }


        public static DataTable ExecuteDataTable(string commandText)
        {
            DataSet ds = SQLiteHelper.ExecuteDataSet(connString, commandText, null);
            return ds.Tables[0];
        }

        public static DataTable ExecuteDataTable(string commandText, params object[] paras)
        {
            DataSet ds = SQLiteHelper.ExecuteDataSet(connString, commandText, paras);
            return ds.Tables[0];
        }

        public static DataSet ExecuteDataSet(string commandText)
        {
            return SQLiteHelper.ExecuteDataSet(connString, commandText, null);
        }

        public static DataSet ExecuteDataSet(string commandText, params object[] paras)
        {
            return SQLiteHelper.ExecuteDataSet(connString, commandText, paras);
        }

        public static int ExecuteNonQuery(string commandText)
        {
            return SQLiteHelper.ExecuteNonQuery(connString, commandText, null);
        }

        public static int ExecuteNonQuery(string commandText, params object[] paras)
        {
            return SQLiteHelper.ExecuteNonQuery(connString, commandText, paras);
        }

        public static object ExecuteScalar(string commandText)
        {
            return SQLiteHelper.ExecuteScalar(connString, commandText, null);
        }

        public static object ExecuteScalar(string commandText, params object[] paras)
        {
            return SQLiteHelper.ExecuteScalar(connString, commandText, paras);
        }

        public static IDataReader ExecuteReader(string commandText)
        {
            return SQLiteHelper.ExecuteReader(connString, commandText);
        }

        public static IDataReader ExecuteReader(string commandText, params object[] paras)
        {
            return SQLiteHelper.ExecuteReader(connString, commandText, paras);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="SelectList">字段列表</param>
        /// <param name="tablename">表名或视图名</param>
        /// <param name="where">筛选条件</param>
        /// <param name="OrderExpression">排序字段。格式：id desc</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string SelectList, string tablename, string where, string OrderExpression, int pageindex, int pagesize)
        {
            if (pageindex == 0)
                pageindex = 1;
            string sql = "select {0} from {1} {2} order by {3} limit {4} offset {5}";
            if (where != "")
                where = " where " + where;
            sql = string.Format(sql, SelectList, tablename, where, OrderExpression, pagesize, (pageindex - 1) * pagesize);

            return ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="name">表或视图名</param>
        /// <param name="where">筛选条件</param>
        /// <returns></returns>
        public static int GetRecordCount(string name, string where)
        {
            string sqlStr = "select count(*) from " + name;
            if (!string.IsNullOrEmpty(where))
                sqlStr += " where " + where;

            return Convert.ToInt32(ExecuteScalar(sqlStr));
        }

        public static DataRow ExecuteDataRow(string commandText)
        {
            DataTable dt = ExecuteDataTable(commandText);
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

        public static DataRow ExecuteDataRow(string commandText, params object[] paras)
        {
            DataTable dt = ExecuteDataTable(commandText, paras);
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }
    }
}
