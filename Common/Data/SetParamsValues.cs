using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Omu.ValueInjecter;

namespace Xiucai.Common.Data
{
    public class SetParamsValues : KnownTargetValueInjection<SqlCommand>
    {
        private IEnumerable<string> ignoredFields = new string[] { };
        private string prefix = string.Empty;

        public SetParamsValues Prefix(string p)
        {
            prefix = p;
            return this;
        }

        public SetParamsValues IgnoreFields(params string[] fields)
        {
            ignoredFields = fields.AsEnumerable();
            return this;
        }

        protected override void Inject(object source, ref SqlCommand cmd)
        {
            if (source == null) return;
            var sourceProps = source.GetInfos().ToList();

            foreach (var prop in sourceProps)
            {
                if (prop.GetCustomAttributes(true).Length > 0)
                {
                    int k = prop.GetCustomAttributes(true).OfType<DbFieldAttribute>()
                                                .Count(dbFieldAttribute => !dbFieldAttribute.IsDbField);

                    if (k > 0)
                        continue;
                }

                if (ignoredFields.Contains(prop.Name.ToLower())) continue;

                var value = prop.GetValue(source,null) ?? DBNull.Value;

                if ((value is DateTime) && value.ToString().IndexOf("0001") > -1) //日期为空时的处理
                    value = DBNull.Value;

                cmd.Parameters.AddWithValue("@" + prefix + prop.Name, value);
            }
        }
    }
}
