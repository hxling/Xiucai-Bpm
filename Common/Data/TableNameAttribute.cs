using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Data
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute()
        {
        }

        public TableNameAttribute(string name)
        {
            _name = name;
        }
        private string _name; public virtual string Name { get { return _name; } set { _name = value; } }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        public DbFieldAttribute()
        {
        }
        public DbFieldAttribute(bool isDbField)
        {
            _isDbField = isDbField;
        }
        private bool _isDbField; 
        public virtual bool IsDbField { get { return _isDbField; } set { _isDbField = value; } }
    }
}
