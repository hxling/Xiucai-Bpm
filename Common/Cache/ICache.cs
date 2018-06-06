using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        bool Add<T>(string key, T value);

        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="duration">持续时间</param>
        /// <returns>结果</returns>
        bool Add<T>(string key, T value, TimeSpan duration);

        /// <summary>
        /// 清除
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 多线程获取
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns>值集合</returns>
        IDictionary<string, object> MultiGet(IList<string> keys);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        bool Set<T>(string key, T value);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="duration">持续时间</param>
        /// <returns>结果</returns>
        bool Set<T>(string key, T value, TimeSpan duration);
    }
}
