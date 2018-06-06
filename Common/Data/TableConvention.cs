using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Data
{
    public static class TableConvention
    {

        public static string Resolve(Type t)
        {
            string _tablename = "";
            TableNameAttribute tableName;
            var name = t.Name;
            foreach(Attribute  attr in t.GetCustomAttributes(true))
            {
                tableName = attr as TableNameAttribute;
                if(tableName!=null)
                    _tablename = tableName.Name;
            }

            if (string.IsNullOrEmpty(_tablename))
            {
                if (name.EndsWith("s"))
                    _tablename = t.Name + "es";
                _tablename = t.Name + "s";
            }

            return _tablename;
        }

        public static string Resolve(object o)
        {
            return Resolve(o.GetType());
        }
    }
}
