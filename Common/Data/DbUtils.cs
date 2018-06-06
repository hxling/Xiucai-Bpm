using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;
using System.Configuration;
using Xiucai.Common.Data.SqlServer;


namespace Xiucai.Common.Data
{
    public static class DbUtils
    {
        static string cs = SqlEasy.connString; //数据库连接字符串
        public static IEnumerable<T> GetWhere<T>(object where) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "select * from " + TableConvention.Resolve(typeof(T)) + " where "
                        .InjectFrom(new FieldsBy()
                        .SetFormat("{0}=@{0}")
                        .SetNullFormat("{0} is null")
                        .SetGlue("and"),
                        where);
                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                        dr.Close();
                    }
                }
            }
        }

        public static int CountWhere<T>(object where) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select count(*) from " + TableConvention.Resolve(typeof(T)) + " where "
                        .InjectFrom(new FieldsBy()
                        .SetFormat("{0}=@{0}")
                        .SetNullFormat("{0} is null")
                        .SetGlue("and"),
                        where);
                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();

                    var regval = (int)cmd.ExecuteScalar();
                    conn.Close();
                    return regval;
                }
            }
        }

        /// <summary>
        /// 清空表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int Delete<T>()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "TRUNCATE TABLE  " + TableConvention.Resolve(typeof (T));
                    conn.Open();
                    var retval = cmd.ExecuteNonQuery();
                    conn.Close();
                    return retval;
                }
            }
        }


        public static int Delete<T>(int id)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from " + TableConvention.Resolve(typeof (T)) + " where KeyID=@KeyID";

                    cmd.InjectFrom<SetParamsValues>(new {KeyID = id});
                    conn.Open();
                    var retval = cmd.ExecuteNonQuery();
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int DeleteWhere<T>(object where)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from " + TableConvention.Resolve(typeof (T)) + " where "
                        .InjectFrom(new FieldsBy()
                            .SetFormat("{0}=@{0}")
                            .SetNullFormat("{0} is null")
                            .SetGlue("and"),
                            where);

                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();
                    var retval = cmd.ExecuteNonQuery();
                    conn.Close();
                    return retval;
                }
            }
        }


        public static int Delete<T>(string ids)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from " + TableConvention.Resolve(typeof(T)) + " where charindex(',' + cast(keyid AS varchar(50)) + ',',','  + @KeyID + ',') > 0";

                    cmd.InjectFrom<SetParamsValues>(new { KeyID = ids });
                    conn.Open();
                    var retval = cmd.ExecuteNonQuery();
                    conn.Close();
                    return retval;
                }
            }
        }

        ///<returns> the id of the inserted object </returns>
        public static int Insert(object o)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert " + TableConvention.Resolve(o) + " ("
                        .InjectFrom(new FieldsBy().IgnoreFields("keyid"), o) + ") values("
                            .InjectFrom(new FieldsBy().IgnoreFields("keyid").SetFormat("@{0}"), o)
                                      + ") select @@identity";

                    cmd.InjectFrom(new SetParamsValues().IgnoreFields("keyid"), o);

                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int Insert(object o, string IgnoreFields)
        {
            string[] strarr = { };
            if (!string.IsNullOrEmpty(IgnoreFields))
                strarr = IgnoreFields.Split(',');
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert " + TableConvention.Resolve(o) + " ("
                        .InjectFrom(new FieldsBy().IgnoreFields(strarr), o) + ") values("
                            .InjectFrom(new FieldsBy().IgnoreFields(strarr).SetFormat("@{0}"), o)
                                      + ") ";

                    cmd.InjectFrom(new SetParamsValues().IgnoreFields(strarr), o);

                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteNonQuery());
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int Update(object o)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update " + TableConvention.Resolve(o) + " set "
                        .InjectFrom(new FieldsBy().IgnoreFields("keyid").SetFormat("{0}=@{0}"), o)
                                      + " where KeyID = @KeyID";

                    cmd.InjectFrom<SetParamsValues>(o);

                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteNonQuery());
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int Update(object o, params string[] fields)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update " + TableConvention.Resolve(o) + " set "
                        .InjectFrom(new FieldsBy().IgnoreFields(fields).SetFormat("{0}=@{0}"), o)
                                      + " where KeyID = @KeyID";

                    cmd.InjectFrom<SetParamsValues>(o);

                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteNonQuery());
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int UpdateWhatWhere<T>(object what, object where)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update " + TableConvention.Resolve(typeof (T)) + " set "
                        .InjectFrom(new FieldsBy().SetFormat("{0}=@{0}"), what)
                                      + " where "
                                          .InjectFrom(new FieldsBy()
                                              .SetFormat("{0}=@wp{0}")
                                              .SetNullFormat("{0} is null")
                                              .SetGlue("and"),
                                              where);

                    cmd.InjectFrom<SetParamsValues>(what);
                    cmd.InjectFrom(new SetParamsValues().Prefix("wp"), where);

                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteNonQuery());
                    conn.Close();
                    return retval;
                }
            }
        }


        public static int InsertNoIdentity(object o)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert " + TableConvention.Resolve(o) + " ("
                        .InjectFrom(new FieldsBy().IgnoreFields("keyid"), o) + ") values("
                            .InjectFrom(new FieldsBy().IgnoreFields("keyid").SetFormat("@{0}"), o) + ")";

                    cmd.InjectFrom<SetParamsValues>(o);

                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteNonQuery());
                    conn.Close();
                    return retval;
                }
            }
        }

        /// <returns>rows affected</returns>
        public static int ExecuteNonQuerySp(string sp, object parameters)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    var retval = Convert.ToInt32(cmd.ExecuteNonQuery());
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int ExecuteNonQuery(string commendText, object parameters)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = commendText;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    var retval = cmd.ExecuteNonQuery();
                    conn.Close();
                    return retval;
                }
            }
        }

        public static IEnumerable<T> ExecuteReader<T>(string sql, object parameters) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                        dr.Close();
                    }
                }
            }
        }


        public static IEnumerable<T> ExecuteReaderSp<T>(string sp, object parameters) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                        dr.Close();
                    }
                }
            }
        }

        public static IEnumerable<T> ExecuteReaderSpValueType<T>(string sp, object parameters)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            yield return (T) dr.GetValue(0);
                        }
                        dr.Close();
                    }
                }
            }
        }

        public static int Count<T>()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select count(*) from " + TableConvention.Resolve(typeof(T));
                    conn.Open();

                    var retval = (int)cmd.ExecuteScalar();
                    conn.Close();
                    return retval;
                }
            }
        }

        public static int GetPageCount(int pageSize, int count)
        {
            var pages = count / pageSize;
            if (count % pageSize > 0) pages++;
            return pages;
        }

        public static IEnumerable<T> GetAll<T>() where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from " + TableConvention.Resolve(typeof(T));
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                        dr.Close();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        public static IEnumerable<T> GetList<T>(string sql, object parameters) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }

                        dr.Close();
                        conn.Close();
                        conn.Dispose();
                    }


                }
            }
        }

        public static DataTable GetPageWithSp(ProcCustomPage pcp, out int recordCount)
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = pcp.Sp_PagerName;
                    cmd.InjectFrom(new SetParamsValues().IgnoreFields("sp_pagername"), pcp);

                    SqlParameter outputPara = new SqlParameter("@RecordCount", SqlDbType.Int);
                    outputPara.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputPara);

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        recordCount = PublicMethod.GetInt(outputPara.Value);
                        conn.Close();
                        conn.Dispose();

                        return ds.Tables[0];
                    }
                }
            }
        }

        /// <summary>
        /// 根据条件获取记录
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="sort">排序 如：keyid desc </param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPage<T>(int page, int pageSize, string sort, object where) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    var name = TableConvention.Resolve(typeof(T));

                    if (string.IsNullOrEmpty(sort))
                        sort = "keyid desc";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "with result as(select *, ROW_NUMBER() over(order by {3}) nr from {0} where "
                    .InjectFrom(new FieldsBy()
                        .SetFormat("{0}=@{0}")
                        .SetNullFormat("{0} is null")
                        .SetGlue("and"),
                        where) + @" )
                    select  * 
                    from    result
                    where   nr  between (({1} - 1) * {2} + 1)
                            and ({1} * {2}) ";

                    cmd.CommandText = string.Format(cmd.CommandText, name, page, pageSize, sort);
                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                        dr.Close();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }



        }

        /// <summary>
        /// 默认为KEYID 倒序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPage<T>(int page, int pageSize, string sort = "keyid desc") where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    var name = TableConvention.Resolve(typeof(T));

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format(@"with result as(select *, ROW_NUMBER() over(order by {3}) nr
                            from {0}
                    )
                    select  * 
                    from    result
                    where   nr  between (({1} - 1) * {2} + 1)
                            and ({1} * {2}) ", name, page, pageSize, sort);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                        dr.Close();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        public static T Get<T>(long keyid) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from " + TableConvention.Resolve(typeof (T)) + " where keyid = " + keyid;
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            return o;
                        }
                        dr.Close();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return default(T);
        }

        public static T Get<T>(object where) where T : new()
        {
            using (var conn = new SqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from " + TableConvention.Resolve(typeof (T)) + " where "
                        .InjectFrom(new FieldsBy()
                            .SetFormat("{0}=@{0}")
                            .SetNullFormat("{0} is null")
                            .SetGlue("and"),
                            where);
                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            return o;
                        }
                        dr.Close();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return default(T);
        }
    }
}
