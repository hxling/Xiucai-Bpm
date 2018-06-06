using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Newtonsoft.Json;
using System.Web;
using Newtonsoft.Json.Converters;
using Xiucai.Common;
namespace Xiucai.BPM.Core
{
    public class RequestParamModel<T> where T:class
    {
        private HttpContext _context;
        public  RequestParamModel(){} 
        public RequestParamModel(HttpContext context)
        {
            this._context = context;
        }

        public HttpContext CurrentContext
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        [DefaultValue(0)]
        public int KeyId { get; set; }

        /// <summary>
        /// 批量处理多个ID，格式：1,2,3,4,5......
        /// </summary>
        public string KeyIds { get; set; }


        /// <summary>
        /// 实体JSON
        /// </summary>
        public string JsonEntity
        {
            get;
            set;
        }
        /// <summary>
        /// 实体类
        /// </summary>
        public T Entity {
            get
            {
                var errors = new List<string>();
                return string.IsNullOrEmpty(JsonEntity) ? null : JsonConvert.DeserializeObject<T>(JsonEntity, new JsonSerializerSettings
                {
                    Error = delegate(object obj, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    },
                    Converters = { new IsoDateTimeConverter() }

                });
            }
        }

        public string Request(string key)
        {
            return _context.Request[key];
        }

        /// <summary>
        /// 页索引
        /// </summary>
        public int Pageindex
        {
            get
            {
                int pageindex;
                int.TryParse(_context.Request["page"], out pageindex);
                if (pageindex == 0)
                    pageindex = 1;
                return pageindex;
            }
        }

        /// <summary>
        /// grid 排序字段
        /// </summary>
        public string Sort
        {
            get { return _context.Request["sort"]; }
        }

        /// <summary>
        /// grid 排序方式 asc || desc
        /// </summary>
        public string Order
        {
            get { return _context.Request["order"]; }
        }


        /// <summary>
        /// 页尺寸
        /// </summary>
        public int Pagesize
        {
            get
            {
                int pagesize;
                int.TryParse(_context.Request["rows"], out pagesize);
                if (pagesize == 0)
                    pagesize = 20;
                return pagesize;
            }
        }

        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Filter
        {
            get
            {
                return PublicMethod.GetString(_context.Request["filter"]);
            }
        }

    }
}
