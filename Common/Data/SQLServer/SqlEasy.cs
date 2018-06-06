using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Xiucai;
using System.Reflection;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.Caching;


namespace Xiucai.Common.Data.SqlServer
{
    public static class SqlEasy
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string connString
        {
            get
            {
                //Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                string connStr = ConfigurationManager.ConnectionStrings["Xiucai.DbConnection"].ConnectionString;
                bool useEncrypt = ConfigHelper.GetValue("useEncrypt").ToLower() == "true";
                if (useEncrypt)
                    return StringHelper.UnBase64(connStr);
                else
                    return connStr;
            }
        }
        // public static string IP_connectionString = StringHelper.DecryptDES(ConfigurationManager.ConnectionStrings["IP_ConnectionString"].ConnectionString, "j7e5q1y%");


        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(string sql)
        {
            return SqlHelper.ExecuteDataset(connString, CommandType.Text, sql).Tables[0];
        }
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteDataset(connString, CommandType.Text, sql, para).Tables[0];
        }

        public static DataTable ExecuteDataTable(string connectionString, string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, para).Tables[0];
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string sql)
        {
            return SqlHelper.ExecuteNonQuery(connString, CommandType.Text, sql);
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteNonQuery(connString, CommandType.Text, sql, para);
        }

        public static int ExecuteNonQuery(string connectionString, string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, para);
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(string sql)
        {
            return SqlHelper.ExecuteScalar(connString, CommandType.Text, sql);
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteScalar(connString, CommandType.Text, sql, para);
        }

        public static object ExecuteScalar(string connectionString, string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql, para);
        }
        #endregion

        #region ExecuteDataReader
        public static SqlDataReader ExecuteDataReader(string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteReader(connString, CommandType.Text, sql, para);
        }

        public static SqlDataReader ExecuteDataReader(string sql)
        {
            return SqlHelper.ExecuteReader(connString, CommandType.Text, sql);
        }

        public static SqlDataReader ExecuteDataReader(string connectionString, string sql)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
        }

        public static SqlDataReader ExecuteDataReader(string connectionString, string sql, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, para);
        }
        #endregion

        #region ExecuteProcedure
        public static DataTable ExecuteProcedure(string procedureName, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteDataset(connString, CommandType.StoredProcedure, procedureName, para).Tables[0];
        }

        public static string ExecuteString(string procedureName, int index, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteString(connString, CommandType.StoredProcedure, procedureName, index, para);
        }

        public static string ExecuteString(string procedureName, string paramenterName, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteString(connString, CommandType.StoredProcedure, procedureName, paramenterName, para);
        }

        public static DataTable ExecuteProcedure(string connectionString, string procedureName, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, procedureName, para).Tables[0];
        }

        public static string ExecuteString(string connectionString, string procedureName, int index, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteString(connectionString, CommandType.StoredProcedure, procedureName, index, para);
        }

        public static string ExecuteString(string connectionString, string procedureName, string paramenterName, params SqlParameter[] para)
        {
            return SqlHelper.ExecuteString(connectionString, CommandType.StoredProcedure, procedureName, paramenterName, para);
        }
        #endregion

        #region ExecuteTran
        public static int ExecuteTran(string commandText)
        {
            return SqlHelper.ExcuteTran(connString, CommandType.Text, commandText);
        }

        public static int ExecuteTran(string commandText, params SqlParameter[] para)
        {
            return SqlHelper.ExcuteTran(connString, CommandType.Text, commandText, para);
        }

        public static int ExecuteTran(string connectionString, string commandText)
        {
            return SqlHelper.ExcuteTran(connectionString, CommandType.Text, commandText);
        }

        public static int ExecuteTran(string connectionString, string commandText, params SqlParameter[] para)
        {
            return SqlHelper.ExcuteTran(connectionString, CommandType.Text, commandText, para);
        }
        #endregion

        #region ExecuteRow
        public static DataRow ExecuteDataRow(string sql)
        {
            DataRow row = null;
            DataTable dt = ExecuteDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                dt.Dispose();
            }
            return row;
        }

        public static DataRow ExecuteDataRow(string sql, params SqlParameter[] para)
        {
            DataRow row = null;
            DataTable dt = ExecuteDataTable(sql, para);
            if (dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                dt.Dispose();
            }
            return row;
        }

        public static DataRow ExecuteDataRow(string connectionString, string sql)
        {
            DataRow row = null;
            DataTable dt = ExecuteDataTable(connectionString, sql);
            if (dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                dt.Dispose();
            }
            return row;
        }

        public static DataRow ExecuteDataRow(string connectionString, string sql, params SqlParameter[] para)
        {
            DataRow row = null;
            DataTable dt = ExecuteDataTable(connectionString, sql, para);
            if (dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                dt.Dispose();
            }
            return row;
        }
        #endregion

        #region ExecuteObject
        public static string ExecuteObject(string sql)
        {
            object obj = ExecuteScalar(sql);
            if (obj != null)
                return obj.ToString();
            return "";
        }

        public static string ExecuteObject(string sql, params SqlParameter[] para)
        {
            object obj = ExecuteScalar(sql, para);
            if (obj != null)
                return obj.ToString();
            return "";
        }

        public static string ExecuteObject(string connectionString, string sql)
        {
            object obj = ExecuteScalar(connectionString, sql);
            if (obj != null)
                return obj.ToString();
            return "";
        }

        public static string ExecuteObject(string connectionString, string sql, params SqlParameter[] para)
        {
            object obj = ExecuteScalar(connectionString, sql, para);
            if (obj != null)
                return obj.ToString();
            return "";
        }
        #endregion

        #region 分页获取

        /// <summary>
        /// 分页获取数据列表 适用于SQL2000
        /// </summary>
        /// <param name="fieldlist">查找的字段</param>
        /// <param name="tablename">表名</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderfield">排序字段 如 id asc,name desc</param>
        /// <param name="key">主键</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2000(string fieldlist, string tablename, string where, string orderfield, string key, int pageindex, int pagesize, out int recordcount)
        {
            string cmd = "ProcCustomPage";
            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@tbname", tablename);
            para[1] = new SqlParameter("@FieldKey", key);
            para[2] = new SqlParameter("@WhereString", where);
            para[3] = new SqlParameter("@PageSize", pagesize);
            para[4] = new SqlParameter("@PageCurrent", pageindex);
            para[5] = new SqlParameter("@FieldOrder", orderfield);
            para[6] = new SqlParameter("@FieldShow", fieldlist);
            para[7] = new SqlParameter("@RecordCount", SqlDbType.Int);

            para[7].Direction = ParameterDirection.Output;

            DataTable dt = SqlHelper.ExecuteDataset(connString, CommandType.StoredProcedure, cmd, para).Tables[0];
            recordcount = PublicMethod.GetInt(para[7].Value);
            return dt;
        }

        /// <summary>
        /// 分页获取数据列表 适用于SQL2000
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="fieldlist">查找的字段</param>
        /// <param name="tablename">表名</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderfield">排序字段</param>
        /// <param name="key">主键</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="ordertype">排序方式 0=ASC 1=DESC</param>
        /// <param name="recordcount">总记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2000(string connectionString, string fieldlist, string tablename, string where, string orderfield, string key, int pageindex, int pagesize)
        {
            string cmd = "ProcCustomPage";
            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@tbname", tablename);
            para[1] = new SqlParameter("@FieldKey", key);
            para[2] = new SqlParameter("@WhereString", where);
            para[3] = new SqlParameter("@PageSize", pagesize);
            para[4] = new SqlParameter("@PageCurrent", pageindex);
            para[5] = new SqlParameter("@FieldOrder", orderfield);
            para[6] = new SqlParameter("@FieldShow", fieldlist);
            para[7] = new SqlParameter("@RecordCount", SqlDbType.Int);

            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, cmd, para).Tables[0];

        }

        /// <summary>
        /// 分页获取数据列表 适用于SQL2005
        /// </summary>
        /// <param name="SelectList">选取字段列表</param>
        /// <param name="tablename">数据源名称表名或视图名称</param>
        /// <param name="where">筛选条件</param>
        /// <param name="OrderExpression">排序 必须指定一个排序字段</param>
        /// <param name="pageindex">页索引 从0开始</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2005(string SelectList, string tablename, string where, string OrderExpression, int pageindex, int pagesize)
        {
            string cmd = "GetRecordFromPage";
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@SelectList", SelectList);
            para[1] = new SqlParameter("@TableSource", tablename);
            para[2] = new SqlParameter("@SearchCondition", where);
            para[3] = new SqlParameter("@OrderExpression", OrderExpression);
            para[4] = new SqlParameter("@pageindex", pageindex);
            para[5] = new SqlParameter("@pagesize", pagesize);

            return SqlHelper.ExecuteDataset(connString, cmd, para).Tables[0];
        }


        #endregion

        #region 获取某表中的总记录数

        /// <summary>
        /// 获取某表中的总记录数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static int GetRecordCount(string tablename)
        {
            string s = "select count(*) from {0}";
            s = string.Format(s, tablename);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connString, CommandType.Text, s));
        }

        public static int GetRecordCount(string tablename, string where)
        {
            string s = "select count(*) from {0} ";
            s = string.Format(s, tablename);
            if (!string.IsNullOrEmpty(where))
                s += " where " + where;
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connString, CommandType.Text, s));
        }

        public static int GetRecordCount(string connectionString, string tablename, string where)
        {
            string s = "select count(*) from {0} ";
            s = string.Format(s, tablename);
            if (!string.IsNullOrEmpty(where))
                s += " where " + where;
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, CommandType.Text, s));
        }


        #endregion

        #region 分页存储过程

        #region  sql 2000 分页存储过程
        /*
     * 
     * CREATE PROCEDURE [dbo].[ProcCustomPage]
	(
@tbname     nvarchar(100),               --要分页显示的表名
@FieldKey   nvarchar(1000),      --用于定位记录的主键(惟一键)字段,可以是逗号分隔的多个字段
@PageCurrent int=1,               --要显示的页码
@PageSize   int=10,                --每页的大小(记录数)
@FieldShow nvarchar(1000)='',      --以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段
@FieldOrder nvarchar(1000)='',      --以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC
@WhereString    nvarchar(1000)='',     --查询条件
@RecordCount int OUTPUT             --总页数
)
AS
SET NOCOUNT ON
--检查对象是否有效
--IF OBJECT_ID(convert(sysname,@tbname)) IS NULL
--BEGIN
--    RAISERROR(N'对象"%s"不存在',1,16,@tbname)
--    RETURN
--END
--IF OBJECTPROPERTY(OBJECT_ID(@tbname),N'IsTable')=0
--    AND OBJECTPROPERTY(OBJECT_ID(@tbname),N'IsView')=0
--    AND OBJECTPROPERTY(OBJECT_ID(@tbname),N'IsTableFunction')=0
--BEGIN
--    RAISERROR(N'"%s"不是表、视图或者表值函数',1,16,@tbname)
--    RETURN
--END

--分页字段检查
IF ISNULL(@FieldKey,N'')=''
BEGIN
    RAISERROR(N'分页处理需要主键（或者惟一键）',1,16)
    RETURN
END

--其他参数检查及规范
IF ISNULL(@PageCurrent,0)<1 SET @PageCurrent=1
IF ISNULL(@PageSize,0)<1 SET @PageSize=10
IF ISNULL(@FieldShow,N'')=N'' SET @FieldShow=N'*'
IF ISNULL(@FieldOrder,N'')=N''
    SET @FieldOrder=N''
ELSE
    SET @FieldOrder=N'ORDER BY '+LTRIM(@FieldOrder)
IF ISNULL(@WhereString,N'')=N''
    SET @WhereString=N''
ELSE
    SET @WhereString=N'WHERE ('+@WhereString+N')'

--如果@RecordCount为NULL值,则计算总页数(这样设计可以只在第一次计算总页数,以后调用时,把总页数传回给存储过程,避免再次计算总页数,对于不想计算总页数的处理而言,可以给@RecordCount赋值)
IF @RecordCount IS NULL
BEGIN
    DECLARE @sql nvarchar(4000)
    SET @sql=N'SELECT @RecordCount=COUNT(*)'
        +N' FROM '+@tbname
        +N' '+@WhereString
    EXEC sp_executesql @sql,N'@RecordCount int OUTPUT',@RecordCount OUTPUT
END

--计算分页显示的TOPN值
DECLARE @TopN varchar(20),@TopN1 varchar(20)
SELECT @TopN=@PageSize,
    @TopN1=(@PageCurrent-1)*@PageSize

--第一页直接显示
IF @PageCurrent=1
    EXEC(N'SELECT TOP '+@TopN
        +N' '+@FieldShow
        +N' FROM '+@tbname
        +N' '+@WhereString
        +N' '+@FieldOrder)
ELSE
BEGIN
    --处理别名
    IF @FieldShow=N'*'
        SET @FieldShow=N'a.*'

    --生成主键(惟一键)处理条件
    DECLARE @Where1 nvarchar(4000),@Where2 nvarchar(4000),
        @s nvarchar(1000),@Field sysname
    SELECT @Where1=N'',@Where2=N'',@s=@FieldKey
    WHILE CHARINDEX(N',',@s)>0
        SELECT @Field=LEFT(@s,CHARINDEX(N',',@s)-1),
            @s=STUFF(@s,1,CHARINDEX(N',',@s),N''),
            @Where1=@Where1+N' AND a.'+@Field+N'=b.'+@Field,
            @Where2=@Where2+N' AND b.'+@Field+N' IS NULL',
            @WhereString=REPLACE(@WhereString,@Field,N'a.'+@Field),
            @FieldOrder=REPLACE(@FieldOrder,@Field,N'a.'+@Field),
            @FieldShow=REPLACE(@FieldShow,@Field,N'a.'+@Field)
    SELECT @WhereString=REPLACE(@WhereString,@s,N'a.'+@s),
        @FieldOrder=REPLACE(@FieldOrder,@s,N'a.'+@s),
        @FieldShow=REPLACE(@FieldShow,@s,N'a.'+@s),
        @Where1=STUFF(@Where1+N' AND a.'+@s+N'=b.'+@s,1,5,N''),    
        @Where2=CASE
            WHEN @WhereString='' THEN N'WHERE ('
            ELSE @WhereString+N' AND ('
            END+N'b.'+@s+N' IS NULL'+@Where2+N')'

    --执行查询
    EXEC(N'SELECT TOP '+@TopN
        +N' '+@FieldShow
        +N' FROM '+@tbname
        +N' a LEFT JOIN(SELECT TOP '+@TopN1
        +N' '+@FieldKey
        +N' FROM '+@tbname
        +N' a '+@WhereString
        +N' '+@FieldOrder
        +N')b ON '+@Where1
        +N' '+@Where2
        +N' '+@FieldOrder)
END
GO


     * */


        #endregion

        #region SQL2005 分页存储过程
        /*
     * 
   CREATE PROCEDURE [dbo].[GetRecordFromPage] 
    @SelectList            VARCHAR(2000),    --欲选择字段列表
    @TableSource        VARCHAR(100),    --表名或视图表 
    @SearchCondition    VARCHAR(2000),    --查询条件
    @OrderExpression    VARCHAR(1000),    --排序表达式
    @PageIndex            INT = 1,        --页号,从0开始
    @PageSize            INT = 10        --页尺寸
AS 
BEGIN
    IF @SelectList IS NULL OR LTRIM(RTRIM(@SelectList)) = ''
    BEGIN
        SET @SelectList = '*'
    END
    PRINT @SelectList
    
    SET @SearchCondition = ISNULL(@SearchCondition,'')
    SET @SearchCondition = LTRIM(RTRIM(@SearchCondition))
    IF @SearchCondition <> ''
    BEGIN
        IF UPPER(SUBSTRING(@SearchCondition,1,5)) <> 'WHERE'
        BEGIN
            SET @SearchCondition = 'WHERE ' + @SearchCondition
        END
    END
    PRINT @SearchCondition

    SET @OrderExpression = ISNULL(@OrderExpression,'')
    SET @OrderExpression = LTRIM(RTRIM(@OrderExpression))
    IF @OrderExpression <> ''
    BEGIN
        IF UPPER(SUBSTRING(@OrderExpression,1,5)) <> 'WHERE'
        BEGIN
            SET @OrderExpression = 'ORDER BY ' + @OrderExpression
        END
    END
    PRINT @OrderExpression

    IF @PageIndex IS NULL OR @PageIndex < 1
    BEGIN
        SET @PageIndex = 1
    END
    PRINT @PageIndex
    IF @PageSize IS NULL OR @PageSize < 1
    BEGIN
        SET @PageSize = 10
    END
    PRINT  @PageSize

    DECLARE @SqlQuery VARCHAR(4000)

    SET @SqlQuery='SELECT '+@SelectList+',RowNumber 
    FROM 
        (SELECT ' + @SelectList + ',ROW_NUMBER() OVER( '+ @OrderExpression +') AS RowNumber 
          FROM '+@TableSource+' '+ @SearchCondition +') AS RowNumberTableSource 
    WHERE RowNumber BETWEEN ' + CAST(((@PageIndex - 1)* @PageSize+1) AS VARCHAR) 
    + ' AND ' + 
    CAST((@PageIndex * @PageSize) AS VARCHAR) 
--    ORDER BY ' + @OrderExpression
    PRINT @SqlQuery
    SET NOCOUNT ON
    EXECUTE(@SqlQuery)
    SET NOCOUNT OFF
 
    RETURN @@RowCount
END
     **/

        #endregion

        #endregion

        #region 获取指定表中指定字段的最大值
        /// <summary>
        /// 获取指定表中指定字段的最大值
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="field">字段</param>
        /// <returns>Return Type:Int</returns>
        public static int GetMaxID(string tableName, string field)
        {
            string s = "select Max({0}) from {1}";
            s = string.Format(s, field, tableName);
            int i = Convert.ToInt32(SqlHelper.ExecuteScalar(connString, CommandType.Text, s) == DBNull.Value ? "0" : SqlHelper.ExecuteScalar(connString, CommandType.Text, s));
            return i;
        }

        /// <summary>
        /// 获取指定表中指定字段的最大值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="tableName">表名称</param>
        /// <param name="field">字段</param>
        /// <returns>Return Type:Int</returns>
        public static int GetMaxID(string connectionString, string tableName, string field)
        {
            string s = "select Max({0}) from {1}";
            s = string.Format(s, field, tableName);
            int i = Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, CommandType.Text, s) == DBNull.Value ? "0" : SqlHelper.ExecuteScalar(connectionString, CommandType.Text, s));
            return i;
        }
        #endregion

        #region 获取总页数

        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="recordcount">总记录数</param>
        /// <returns></returns>
        public static int GetDataPages(int pagesize, int recordcount)
        {
            int iTotalpages = 0;
            if (recordcount > 0)
            {
                int iR = recordcount % pagesize;
                if (iR == 0)
                    iTotalpages = recordcount / pagesize;
                else
                {
                    iR = recordcount / pagesize;
                    iTotalpages = iR + 1;
                }
            }
            else
            {
                iTotalpages = 0;
            }

            return iTotalpages;
        }

        #endregion

    }
}
