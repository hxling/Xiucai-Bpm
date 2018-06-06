using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;

namespace Xiucai.Common.Data
{
    public class FieldsBy : KnownTargetValueInjection<string>
    {
        private IEnumerable<string> _ignoredFields = new string[] { };
        private string _format = "{0}";
        private string _nullFormat;
        private string _glue = ",";

        public FieldsBy SetGlue(string g)
        {
            _glue = " " + g + " ";
            return this;
        }

        public FieldsBy IgnoreFields(params string[] fields)
        {
            _ignoredFields = fields;
            return this;
        }

        public FieldsBy SetFormat(string f)
        {
            _format = f;
            return this;
        }

        public FieldsBy SetNullFormat(string f)
        {
            _nullFormat = f;
            return this;
        }

        protected override void Inject(object source, ref string target)
        {
            var sourceProps = source.GetInfos().ToList();
            var s = string.Empty;
            for (var i = 0; i < sourceProps.Count(); i++)
            {
                var prop = sourceProps[i];
                
                if (prop.GetCustomAttributes(true).Length > 0)
                {
                    int k = prop.GetCustomAttributes(true).OfType<DbFieldAttribute>().Count(atr => !(atr).IsDbField);

                    if (k > 0)
                        continue;
                }

                if (_ignoredFields.Contains(prop.Name.ToLower())) continue;
                    
                    
                if (prop.GetValue(source,null) == DBNull.Value && _nullFormat != null)
                    s += string.Format(_nullFormat, prop.Name);
                else
                    s += string.Format(_format, prop.Name) + _glue;
            }
            s = s.RemoveSuffix(_glue);
            target += s;
        }
    }
}
