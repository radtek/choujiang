using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XGTech.Utility;

namespace XGTech.Web.Common
{
    public static class CookiesHelper
    {
        #region  cookie 操作
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="year">过期时长，单位：年</param>      
        public static void SetCookies(string key, string value, int year = 30)
        {
            XGTech.Utility.HttpContext.Current.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(year)
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public static void DeleteCookies(string key)
        {
            XGTech.Utility.HttpContext.Current.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public static string GetCookies(string key)
        {
            XGTech.Utility.HttpContext.Current.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        #endregion
    }
}
