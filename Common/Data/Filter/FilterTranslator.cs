using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Data.Filter
{
    public class FilterTranslator
    {
       
        /// <summary>
        /// 将操作符代码转换为SQL的操作符号
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private static string GetOperatorQueryText(string op)
        {
            switch (op.ToLower())
            {
                case "eq": return " = ";
                case "gt": return " > ";
                case "ge": return " >= ";
                case "nu": return " IS NULL ";
                case "nn": return " IS NOT NULL ";
                case "lt": return " < ";
                case "le": return " <= ";
                case "cn": return " like ";
                case "bw": return " like ";
                case "ew": return " like ";
                case "ne": return " <> ";
                case "in": return " IN ";
                case "ni": return " NOT IN ";
                default: return " = ";
            }
        }

        public static string ToSql(string jsonFilter)
        {
            if (string.IsNullOrEmpty(jsonFilter))
                return " 1=1 ";
            FilterGroup fg = Common.JSONhelper.ConvertToObject<FilterGroup>(jsonFilter);
            return ToSql(fg);
        }

        private static string ToSql(FilterGroup fg)
        {
            StringBuilder sb = new StringBuilder();
            
            if (fg == null)
                return " 1=1 ";

            sb.Append("(");
            bool flag = false;
            if (fg.Rules != null)
            {
                foreach (var rule in fg.Rules)
                {
                    if (flag)
                        sb.Append(" "+fg.groupOp.ToString()+" ");
                    sb.Append(TranslateRule(rule));
                    flag = true;
                }
            }

            if (fg.Groups != null)
            {
                foreach (var subgroup in fg.Groups)
                {
                    if (flag)
                        sb.Append(" " + fg.groupOp.ToString()+" ");
                    sb.Append(ToSql(subgroup));
                    flag = true;
                }
            }

            sb.Append(")");
            return sb.ToString();
        }

        private static string TranslateRule(FilterRule rule)
        {
            StringBuilder sb = new StringBuilder();
            if (rule == null) return " 1=1 ";
            
			if( !string.IsNullOrEmpty(rule.op) ) {
                string _op = GetOperatorQueryText(rule.op);
				switch (rule.op)
				{
					case "bw":
						sb.Append(rule.field+_op+"'"+rule.data+"%'");
						break;
					case "ew":
						sb.Append(rule.field+_op+"'%"+rule.data+"'");
						break;
					case "cn":
					case "nc":
						sb.Append(rule.field+_op+"'%"+rule.data+"%'");
						break;
					case "in":
					case "ni":
						sb.Append(rule.field+_op+"("+rule.data+")");
						break;
					case "nu":
					case "nn":
						sb.Append(rule.field+_op);
						break;
					default :
                        sb.Append(rule.field+ _op +"'"+ rule.data +"'");
						break;
				}
			}
            return sb.ToString();
        }
    }
}
