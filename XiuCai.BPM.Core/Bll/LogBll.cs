using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Omu.ValueInjecter;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Xiucai.Common;
using Xiucai.Common.Data;


namespace Xiucai.BPM.Core.Bll
{
    public enum OperationType
    {
        Add = 1,
        Update = 2,
        Delete = 3,
        Select = 4,
        Login = 5,
        LoginOut = 6,
        Other = 7
    }


    public class LogBll<T> where T:new ()
    {
        private IEnumerable<string> ignoredFields = new string[] { };
        public LogBll<T> IgnoreFields(params string[] fields)
        {
            ignoredFields = fields.AsEnumerable();
            return this;
        }
       
        public int AddLog(string busingessName,string opContent)
        {
            LogModel log = new LogModel();
            log.BusinessName = busingessName ;
            log.OperationIp = PublicMethod.GetClientIP();
            log.OperationTime = DateTime.Now;
            log.OperationType = (int)OperationType.Other;
            log.UserId = SysVisitor.Instance.UserId;
            log.SqlText = "";

            int logId = LogDal.Instance.Insert(log);
            if (logId > 0)
            {
                //添加日志详细信息
                StringBuilder sb = new StringBuilder();

                var sqlTemp =
                    "insert into Sys_LogDetails (logid,fieldName,fieldtext,oldvalue,newvalue,remark) values('{0}','{1}','{2}','{3}','{4}','{5}')";
                sb.AppendFormat(sqlTemp, logId, "", "", "", "",opContent);

                if (sb.Length > 0)
                {
                    return DbUtils.ExecuteNonQuery(sb.ToString(), null);
                }
            }
            return 0;
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int AddLog(T t,string busingessName="")
        {
            Type objTye = typeof (T);
            PropertyInfo property = objTye.GetProperty("KeyId");
            
            LogModel log = new LogModel();
            log.BusinessName = busingessName?? GetBusingessName();
            log.OperationIp = PublicMethod.GetClientIP();
            log.OperationTime = DateTime.Now;
            log.OperationType = (int)OperationType.Add;
            if (property != null) 
                log.PrimaryKey =  property.GetValue(t,null).ToString();
            log.TableName = TableConvention.Resolve(t);
            log.UserId = SysVisitor.Instance.UserId;
            log.SqlText = "";
            

            int logId = LogDal.Instance.Insert(log);
            if(logId >0 )
            {
                //添加日志详细信息
                StringBuilder sb = new StringBuilder();
                
                var sqlTemp =
                    "insert into Sys_LogDetails (logid,fieldName,fieldtext,oldvalue,newvalue,remark) values('{0}','{1}','{2}','{3}','{4}','')";
                foreach(PropertyInfo pi in objTye.GetProperties())
                {

                    if (!string.Equals(pi.Name, "keyid",StringComparison.CurrentCultureIgnoreCase) &&
                       pi.GetCustomAttributes(true).OfType<DbFieldAttribute>().Count(dbFieldAttribute => !dbFieldAttribute.IsDbField) ==0 &&
                        !ignoredFields.Contains(pi.Name,StringComparer.InvariantCultureIgnoreCase))
                    {
                        sb.AppendFormat(sqlTemp, logId, pi.Name, GetFieldText(pi), "",pi.GetValue(t, null));
                        sb.AppendLine();
                    }
                }

                if(sb.Length > 0)
                {
                    return DbUtils.ExecuteNonQuery(sb.ToString(), null);
                }
            }
            return 0;
        }

        public T Clone(T obj)
        {
            var model = new T();
            return (T)model.InjectFrom(obj);
        }

        /// <summary>
        /// 添加更新操作日志
        /// </summary>
        /// <param name="oldObj">旧实体对象</param>
        /// <param name="newObj">新实体对象</param>
        /// <returns></returns>
        public int UpdateLog(T oldObj,T newObj)
        {
            Type objTye = typeof (T);

            PropertyInfo property = objTye.GetProperty("KeyId");

            LogModel log = new LogModel();
            log.BusinessName = GetBusingessName();
            log.OperationIp = PublicMethod.GetClientIP();
            log.OperationTime = DateTime.Now;
            log.OperationType = (int)OperationType.Update;
            log.PrimaryKey = property.GetValue(newObj,null).ToString();
            log.TableName = TableConvention.Resolve(newObj);
            log.UserId = SysVisitor.Instance.UserId;
            log.SqlText = "";


            //添加日志详细信息
            StringBuilder sb = new StringBuilder();

            var sqlTemp =
                "insert into Sys_LogDetails (logid,fieldName,fieldtext,oldvalue,newvalue,remark) values('{0}','{1}','{2}','{3}','{4}','')";

            int logId = LogDal.Instance.Insert(log);
            if (logId > 0)
            {
                var propertyList = objTye.GetInfos();
                foreach (PropertyInfo p in propertyList)
                {
                    object oldVal = p.GetValue(oldObj, null);
                    object newVal = p.GetValue(newObj, null);

                    if (!string.Equals(p.Name, "keyid", StringComparison.CurrentCultureIgnoreCase) &&
                            p.GetCustomAttributes(true).OfType<DbFieldAttribute>().Count(dbFieldAttribute => !dbFieldAttribute.IsDbField) == 0 &&
                            !ignoredFields.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
                    {
                        if(!IsEqual(p.PropertyType,oldVal,newVal))
                        {
                            sb.AppendFormat(sqlTemp, logId, p.Name, GetFieldText(p), oldVal, newVal, "");
                            sb.AppendLine();
                        }
                    }
                }

                if(sb.Length > 0)
                    return DbUtils.ExecuteNonQuery(sb.ToString(), null);
                return LogDal.Instance.Delete(logId);
            }
            return 0;
        }

        /// <summary>
        /// 执行删除的操作日志
        /// </summary>
        /// <param name="t">业务实体</param>
        /// <returns></returns>
        public int DeleteLog(T t)
        {
             Type objTye = typeof (T);

             PropertyInfo property = objTye.GetProperty("KeyId");

            LogModel log = new LogModel();
            log.BusinessName = GetBusingessName();
            log.OperationIp = PublicMethod.GetClientIP();
            log.OperationTime = DateTime.Now;
            log.OperationType = (int)OperationType.Delete;
            log.PrimaryKey = property.GetValue(t,null).ToString();
            log.TableName = TableConvention.Resolve(t);
            log.UserId = SysVisitor.Instance.UserId;
            log.SqlText = "";


            //添加日志详细信息
            StringBuilder sb = new StringBuilder();

            var sqlTemp =
                "insert into Sys_LogDetails (logid,fieldName,fieldtext,oldvalue,newvalue,remark) values('{0}','{1}','{2}','{3}','{4}','')";

            int logId = LogDal.Instance.Insert(log);
            if (logId > 0)
            {
                foreach (PropertyInfo pi in objTye.GetProperties())
                {

                    if (!string.Equals(pi.Name, "keyid", StringComparison.CurrentCultureIgnoreCase) &&
                        pi.GetCustomAttributes(true).OfType<DbFieldAttribute>().Count(dbFieldAttribute => !dbFieldAttribute.IsDbField) == 0)
                    {
                        sb.AppendFormat(sqlTemp, logId, pi.Name, GetFieldText(pi), pi.GetValue(t, null), "");
                        sb.AppendLine();
                    }
                }

                if (sb.Length > 0)
                {
                    return DbUtils.ExecuteNonQuery(sb.ToString(), null);
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取业务名称
        /// </summary>
        /// <param name="obj">业务实体</param>
        /// <returns></returns>
        private string GetBusingessName()
        {
            Type objTye = typeof(T);
            string busingessName = "";
            var busingessNames = objTye.GetCustomAttributes(true).OfType<DescriptionAttribute>();
            var descriptionAttributes = busingessNames as DescriptionAttribute[] ?? busingessNames.ToArray();
            if (descriptionAttributes.Any())
                busingessName = descriptionAttributes.ToList()[0].Description;
            else
            {
                busingessName = objTye.Name;
            }
            return busingessName;
        }

        /// <summary>
        /// 获取字段中文名称
        /// </summary>
        /// <param name="pi">字段属性信息</param>
        /// <returns></returns>
        private string GetFieldText(PropertyInfo pi)
        {
            DescriptionAttribute descAttr;
            string txt = "";
            var descAttrs = pi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (descAttrs.Any())
            {
                descAttr = descAttrs[0] as DescriptionAttribute;
                txt = descAttr.Description;
            }
            else
            {
                txt = pi.Name;
            }
            return txt;
        }

        private bool IsEqual(Type dataType, object oldObj, object newObj)
        {
            if (oldObj == null && newObj == null)
                return true;

            if (dataType == typeof(int))
            {
                return (int)oldObj == (int)newObj;
            }
            else if (dataType == typeof(decimal))
            {
                return (decimal)oldObj == (decimal)newObj;
            }
            else if (dataType == typeof(double))
            {
                return (double)oldObj == (double)newObj;
            }
            else if (dataType == typeof(Guid))
            {
                return (Guid)oldObj == (Guid)newObj;
            }
            else if (dataType == typeof(DateTime))
            {
                return (DateTime)oldObj == (DateTime)newObj;
            }
            else
                return oldObj.Equals(newObj);

        }

        /// <summary>
        /// 清除操作日志
        /// </summary>
        /// <param name="days">保留日志的天数</param>
        /// <returns></returns>
        public string ClearLog(int days)
        {
            if(days == 0)
            {
                LogDal.Instance.RemoveAll();
                return new JsonMessage { Data = "1", Message = "日志清除成功！", Success = true }.ToString();
            }

            List<LogModel> logs = LogDal.Instance.GetList(days).ToList();
            if(logs.Count >0)
            {
                string logIds = logs.Aggregate("", (current, log) => current + (log.KeyId.ToString() + ",")).TrimEnd(',');
                int k = LogDal.Instance.Delete(logIds);
                int n = LogDetailDal.Instance.DeleteBy(logIds);

                return new JsonMessage {Data = k.ToString(), Message = "日志清除成功！", Success = true}.ToString();
            }
            else
            {
                return new JsonMessage { Data = "0", Message = "没有符合条件日志可以清除！", Success = false }.ToString();
            }
        }
    }
}
