using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Cache
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class CacheBase : ICache
    {
        private TimeSpan maxDuration = TimeSpan.FromDays(15);

        /// <summary>
        /// 最长持续时间
        /// </summary>
        public TimeSpan MaxDuration
        {
            get
            {
                return this.maxDuration;
            }
            set
            {
                this.maxDuration = value;
            }
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get;
            set;
        }

        public bool Add<T>(string key, T value)
        {
            return this.Add<T>(key, value, TimeSpan.FromHours(1));
        }

        public abstract bool Add<T>(string key, T value, TimeSpan duration);

        public abstract void Clear();

        public abstract T Get<T>(string key);

        /// <summary>
        ///  获取全名
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>全名</returns>
        public virtual string GetFullName(string key)
        {
            string result = key;
            if (!string.IsNullOrWhiteSpace(this.Prefix))
            {
                result = string.Format("{0}.{1}", this.Prefix, key);
            }

            return result;
        }

        public abstract IDictionary<string, object> MultiGet(IList<string> keys);

        public abstract void Remove(string key);

        public bool Set<T>(string key, T value)
        {
            return this.Set<T>(key, value, TimeSpan.FromHours(1));
        }

        public abstract bool Set<T>(string key, T value, TimeSpan duration);
    }
}
