using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGTech.Utility
{
    public class Paging<TEntity> where TEntity : class
    {
        public static dynamic GetPageList(int curPage, int pageSize, IEnumerable<TEntity> list)
        {
            int curIndex = curPage <= 0 ? 1 : curPage / pageSize + 1;

            decimal rsCount = list.Count();
            int totalCount = (int)Math.Ceiling(rsCount / pageSize);

            if (curIndex > totalCount) curIndex = totalCount;

            if (totalCount > 0)
            {
                list = list.Skip((curIndex - 1) * pageSize).Take(pageSize);
            }
            return new { rows = list.ToList(), total = rsCount, pages = totalCount };
        }

        public static List<TEntity> PageByList(int curPage, int pageSize, IEnumerable<TEntity> list, out decimal total, out int totalPage)
        {
            int curIndex = curPage <= 0 ? 1 : curPage / pageSize + 1;

            decimal rsCount = list.Count();
            int totalCount = (int)Math.Ceiling(rsCount / pageSize);

            if (curIndex > totalCount) curIndex = totalCount;

            if (totalCount > 0)
            {
                list = list.Skip((curIndex - 1) * pageSize).Take(pageSize);
            }
            total = rsCount;
            totalPage = totalCount;
            return list.ToList();
        }

    }
    public static class PagingHelper
    {


        /// <summary>
        /// 根据查询字段的数据类型进行基本过滤
        /// 1、字符串：过滤空字符串
        /// 2、整型：过滤小于1的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParas"></param>
        /// <param name="queryStringFieldName"></param>
        /// <param name="action"></param>
        /// <param name="queryParaMethod"></param>
        public static void AddQueryStringParas<T>(this RouteValueDictionary queryParas, string queryStringFieldName,
            Action<T> action, Func<string, string> queryParaMethod)
        {
            //AddQueryParas
            var type = typeof(T);

            string typeName = type.ToString();

            switch (typeName)
            {
                case "System.String":
                    string stringValue = ConvertHelper.ToString(queryParaMethod(queryStringFieldName));

                    if (stringValue != "")
                    {
                        queryParas.Add(queryStringFieldName, stringValue);
                        action((T)(object)stringValue);
                    }

                    break;
                case "System.Int32":

                    int int32Value = ConvertHelper.ToInt32(queryParaMethod(queryStringFieldName), -1);
                    if (int32Value > -1)
                    {
                        queryParas.Add(queryStringFieldName, int32Value);
                        action((T)(object)int32Value);
                    }
                    break;
            }
        }

        public static string CreatePagingSql(int _recordCount, int _pageSize, int _pageIndex, string _safeSql, string _orderField)
        {
            //计算总页数
            _pageSize = _pageSize == 0 ? _recordCount : _pageSize;
            int pageCount = (_recordCount + _pageSize - 1) / _pageSize;

            //检查当前页数
            if (_pageIndex < 1)
            {
                _pageIndex = 1;
            }
            else if (_pageIndex > pageCount)
            {
                _pageIndex = pageCount;
            }
            //拼接SQL字符串，加上ROW_NUMBER函数进行分页
            StringBuilder newSafeSql = new StringBuilder();
            newSafeSql.AppendFormat("SELECT ROW_NUMBER() OVER(ORDER BY {0}) as row_number,", _orderField);
            newSafeSql.Append(_safeSql.Substring(_safeSql.ToUpper().IndexOf("SELECT") + 6));

            //拼接成最终的SQL语句
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM (");
            sbSql.Append(newSafeSql.ToString());
            sbSql.Append(") AS T");
            sbSql.AppendFormat(" WHERE row_number between {0} and {1}", ((_pageIndex - 1) * _pageSize) + 1, _pageIndex * _pageSize);

            return sbSql.ToString();
        }

        /// <summary>
        /// 获取记录总数SQL语句
        /// </summary>
        /// <param name="_safeSql">SQL查询语句</param>
        /// <returns>记录总数SQL语句</returns>
        public static string CreateCountingSql(string _safeSql)
        {
            return string.Format(" SELECT COUNT(1) AS RecordCount FROM ({0}) AS T ", _safeSql);
        }
    }
}
