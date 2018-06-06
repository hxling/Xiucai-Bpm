using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Xiucai.Common
{
    public class CookieHelper
    {
        /// <summary>
        /// 写cookie值，不在客户端创建文件，存放在服务器内存中。
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 写cookie值，在客户端创建文件。存放cookie值（单一项值）。
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="cookieDomain">域</param>
        /// <param name="expires">cookie 保存时长 单位分种</param>
        public static void WriteCookie(string strName, string strValue, string cookieDomain, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName] ?? new HttpCookie(strName);
            cookie.Value = strValue;
            //cookie.Expires = DateTime.Now.AddMinutes(expires);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            cookie.Domain = cookieDomain;
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 写cookie值（存放数组形式参数，用于第一次写cookie值，并指定有效时间）
        /// </summary>
        /// <param name="cookieName">cookies名字</param>
        /// <param name="strValuesName">cookie项的名称，cookie[strValuesName][]</param>
        /// <param name="strValue">cookie项的值，cookie[strValuesName][strValue]</param>
        /// <param name="cookieDomain">cookie域属性</param>
        /// <param name="expiresDays">cookies 有效时间 单位天</param>
        public static void WriteCookie(string cookieName, string strValuesName, string strValue, string cookieDomain, int expiresDays)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Values[strValuesName] = HttpUtility.UrlEncode(strValue);
            }

            cookie.Values[strValuesName] = HttpUtility.UrlEncode(strValue);
            cookie.Values["expires"] = expiresDays.ToString();
            cookie.Expires = DateTime.Now.AddDays(expiresDays);


            if (cookieDomain != string.Empty 
                && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain) > -1 
                && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }


        /// <summary>
        /// 写cookie值（操作已经存在的cookie，存放数组形式参数）
        /// </summary>
        /// <param name="cookieName">cookies名字</param>
        /// <param name="strValuesName">cookie项的名称，cookie[strValuesName][]</param>
        /// <param name="strValue">cookie项的值，cookie[strValuesName][strValue]</param>
        /// <param name="cookieDomain">cookie域属性</param>
        public static void WriteCookie(string cookieName, string strValuesName, string strValue, string cookieDomain)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Values[strValuesName] = HttpUtility.UrlEncode(strValue);
            }
            else
            {

                cookie.Values[strValuesName] = HttpUtility.UrlEncode(strValue);

                var httpCookie = HttpContext.Current.Request.Cookies[cookieName];
                if (httpCookie != null && httpCookie["expires"] != null)
                {
                    int intExpires = PublicMethod.GetInt(httpCookie["expires"], 0);
                    if (intExpires > 0)
                    {
                        cookie.Values["expires"] = intExpires.ToString();
                        cookie.Expires = DateTime.Now.AddMinutes(intExpires);
                    }
                }

            }

            if (cookieDomain != string.Empty && 
                HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain, System.StringComparison.Ordinal) > -1 && 
                IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 清除登录用户的cookie
        /// </summary>
        public static void ClearUserCookie(string cookieDomain, string cookiesName)
        {
            HttpCookie cookie = new HttpCookie(cookiesName);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddYears(-1);
            //string cookieDomain = ConfigFactory.GetConfig().CookieDomain.Trim();
            if (cookieDomain != string.Empty && 
                HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain, System.StringComparison.Ordinal) > -1 
                && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = cookieDomain;
            HttpContext.Current.Response.AppendCookie(cookie);

        }


        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[strName];
            return httpCookie != null ? httpCookie.Value : "";
        }

        /// <summary>
        /// 获得cookie值
        /// </summary>
        /// <param name="cookiesName">Cookie 名称</param>
        /// <param name="strName">项</param>
        /// <returns>值</returns>
        public static string GetCookie(string cookiesName, string strName)
        {
            var httpCookie = HttpContext.Current.Request.Cookies[cookiesName];
            if (httpCookie != null && httpCookie[strName] != null)
            {
                return HttpUtility.UrlDecode(httpCookie.Values[strName]);
            }

            return "";
        }

        /// <summary>
        /// 是否为有效域
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string host)
        {
            Regex r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return !r.IsMatch(host.Replace(".", string.Empty));
        }
    }
}
