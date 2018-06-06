using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Xiucai.Common.Cache
{
    /// <summary>
    /// Aspnet缓存
    /// </summary>
    public class AspnetCache : CacheBase
    {
        private System.Web.Caching.Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AspnetCache()
            : this("Common.Cache")
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prefix">前缀</param>
        public AspnetCache(string prefix)
        {
            this.Prefix = prefix;
        }

        public override bool Add<T>(string key, T value, TimeSpan duration)
        {
            bool result = false;
            if (value != null)
            {
                if (duration <= TimeSpan.Zero)
                {
                    duration = this.MaxDuration;
                }
                result = this.cache.Add(this.GetFullName(key), value, null, DateTime.Now.Add(duration), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null) == null;
            }

            return result;
        }

        public override void Clear()
        {
            //	获取键集合
            IList<string> keys = new List<string>();
            IDictionaryEnumerator caches = this.cache.GetEnumerator();
            while (caches.MoveNext())
            {
                string key = caches.Key.ToString();
                if (key.StartsWith(this.Prefix))
                {
                    keys.Add(key);
                }
            }
            //	移除全部
            foreach (string key in keys)
            {
                this.cache.Remove(key);
            }
        }

        public override T Get<T>(string key)
        {
            T result = default(T);
            object value = this.cache.Get(this.GetFullName(key));
            if (value is T)
            {
                result = (T)value;
            }

            return result;
        }

        public override IDictionary<string, object> MultiGet(IList<string> keys)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in keys)
            {
                result.Add(key, this.Get<object>(key));
            }

            return result;
        }

        public override void Remove(string key)
        {
            this.cache.Remove(this.GetFullName(key));
        }

        public override bool Set<T>(string key, T value, TimeSpan duration)
        {
            bool result = false;
            if (value != null)
            {
                if (duration <= TimeSpan.Zero)
                {
                    duration = this.MaxDuration;
                }
                this.cache.Insert(this.GetFullName(key), value, null, DateTime.Now.Add(duration), System.Web.Caching.Cache.NoSlidingExpiration);
                result = true;
            }

            return result;
        }

        
    }
}
