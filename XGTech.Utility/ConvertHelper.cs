using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XGTech.Utility
{
    /// <summary>
    /// 基本类型转换辅助类
    /// </summary>
    public static partial class ConvertHelper
    {
        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>string</returns>
        public static string ToString(string value, string defaultValue = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                defaultValue = value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>string</returns>
        public static string ToString(object value, string defaultValue = "")
        {
            if (value != null)
            {
                defaultValue = value.ToString();
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>string</returns>
        public static bool ToBoolean(object value, bool defaultValue = false)
        {
            if (value != null)
            {
                if (value.ToString() == "True" || value.ToString() == "true" || value.ToString() == "是")
                {
                    defaultValue = true;
                }
                else
                {
                    //TODO:Test
                    bool temp;
                    if (Boolean.TryParse(value.ToString(), out temp))
                    {
                        defaultValue = temp;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>int</returns>
        public static int ToInt32(string value, int defaultValue = 0)
        {
            if (!String.IsNullOrEmpty(value))
            {
                int temp;
                if (Int32.TryParse(value.ToString(), out temp))
                {
                    defaultValue = temp;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>int</returns>
        public static int ToInt32(object value, int defaultValue = 0)
        {
            if (value != null)
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(value.ToString());
                }
                catch
                {
                    temp = defaultValue;
                }

                defaultValue = temp;
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>int</returns>
        public static long ToInt64(string value, long defaultValue = 0)
        {
            if (!String.IsNullOrEmpty(value))
            {
                long temp;
                if (Int64.TryParse(value.ToString(), out temp))
                {
                    defaultValue = temp;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>int</returns>
        public static long ToInt64(object value, long defaultValue = 0)
        {
            if (value != null)
            {
                long temp;
                try
                {
                    temp = Convert.ToInt64(value.ToString());
                }
                catch
                {
                    temp = defaultValue;
                }

                defaultValue = temp;
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(string value, decimal defaultValue = 0)
        {
            if (!String.IsNullOrEmpty(value))
            {
                decimal temp;
                if (Decimal.TryParse(value, out temp))
                {
                    defaultValue = temp;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(object value, decimal defaultValue = 0)
        {
            if (value != null)
            {
                decimal temp;
                try
                {
                    temp = Convert.ToDecimal(value);
                }
                catch
                {
                    temp = defaultValue;
                }

                defaultValue = temp;
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="num">保留几位小数</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>decimal</returns>
        public static decimal ToDouble(string value, int num, decimal defaultValue = 0)
        {
            if (!String.IsNullOrEmpty(value))
            {
                decimal temp;
                if (Decimal.TryParse(value, out temp))
                {
                    defaultValue = temp;
                    defaultValue = decimal.Round(decimal.Parse(defaultValue.ToString()), num);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">参数名</param>
        /// <param name="num">保留几位小数</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>decimal</returns>
        public static decimal ToDouble(object value, int num, decimal defaultValue = 0)
        {
            if (value != null)
            {
                decimal temp;
                try
                {
                    temp = Convert.ToDecimal(value);
                    temp = decimal.Round(decimal.Parse(temp.ToString()), num);
                }
                catch
                {
                    temp = defaultValue;
                }

                defaultValue = temp;
            }

            return defaultValue;
        }

        /// <summary>
        /// 截取指定开始,结束位置字符 如果不存在返回空
        /// </summary>
        /// <param name="htmlCode">源码</param>
        /// <param name="startHtmlCode">其始代码</param>
        /// <param name="endHtmlCode">结束代码</param>
        /// <returns></returns>
        public static string GetStartEndCode(string htmlCode, string startHtmlCode, string endHtmlCode)
        {
            if (startHtmlCode == "" || startHtmlCode == string.Empty || endHtmlCode == "" || endHtmlCode == string.Empty) { return ""; }
            int startCount = htmlCode.IndexOf(startHtmlCode);
            if (startCount == -1) { return ""; }
            int endCount = htmlCode.IndexOf(endHtmlCode, startCount + startHtmlCode.Length);
            if (endCount == -1)
            {
                return "";
            }
            try
            {
                return htmlCode.Substring(startCount + startHtmlCode.Length, endCount - startCount - startHtmlCode.Length);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index">第几位数</param>
        /// <returns></returns>
        public static string GetSubstringIndex(string content, int index)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            if (content.Length < index)
            {
                return content;
            }
            return content.Substring(0, index);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index">第几位数</param>
        /// <returns></returns>
        public static string GetSubstringIndex(string content, int index, string endString)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            content = System.Text.RegularExpressions.Regex.Replace(content, @"<[^>]*>", "");
            if (content.Length < index)
            {
                return content;
            }
            return content.Substring(0, index) + endString;
        }

        /// <summary>
        /// 转换为时间格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            DateTime defaultValue = DateTime.Parse("1900-01-01".ToString());
            if (value != null)
            {
                DateTime temp;
                try
                {
                    temp = DateTime.Parse(value.ToString());
                }
                catch
                {
                    temp = defaultValue;
                }

                defaultValue = temp;
            }

            return defaultValue;
        }

        /// <summary>
        /// 转换为byte格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(string value, byte defaultValue = 0)
        {
            if (!String.IsNullOrEmpty(value))
            {
                byte temp;
                if (Byte.TryParse(value.ToString(), out temp))
                {
                    defaultValue = temp;
                }
            }

            return defaultValue;
        }



        public static bool ValidateMobilePhone(this object value)
        {
            if (value == null)
            {
                return false;
            }
            Regex regex = new Regex(@"^(0|86|17951)?(13[0-9]|15[0-9]|17[0-9]|18[0-9]|14[57])[0-9]{8}$");
            if (!regex.IsMatch(value.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static string ToSafeString(this object value, string defaultValue = "")
        {
            if (value != null)
            {
                defaultValue = value.ToString();
            }

            return defaultValue;
        }
    }
}
