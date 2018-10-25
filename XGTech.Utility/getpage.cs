using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Utility
{
        public class PagedList<T> : List<T>
        {
            #region 分页属性

            public int PageIndex { get; private set; }

            public int PageSize { get; private set; }

            public int TotalCount { get; private set; }

            public int TotalPages { get; private set; }

            public bool HasPreviousPage
            {
                get { return (PageIndex > 0); }
            }
            public bool HasNextPage
            {
                get { return (PageIndex + 1 < TotalPages); }
            }

            #endregion

            #region 构造方法

            public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
            {
                if (source == null || source.Count() < 1)
                    throw new System.ArgumentNullException("source");

                int total = source.Count();
                this.TotalCount = total;
                this.TotalPages = total / pageSize;

                if (total % pageSize > 0)
                    TotalPages++;

                this.PageSize = pageSize;
                this.PageIndex = pageIndex;
                this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
            }

            public PagedList(IList<T> source, int pageIndex, int pageSize)
            {
                if (source == null || source.Count() < 1)
                    throw new System.ArgumentNullException("source");

                TotalCount = source.Count();
                TotalPages = TotalCount / pageSize;

                if (TotalCount % pageSize > 0)
                    TotalPages++;

                this.PageSize = pageSize;
                this.PageIndex = pageIndex;
                this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
            }

            public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
            {
                if (source == null || source.Count() < 1)
                    throw new System.ArgumentNullException("source");

                TotalCount = totalCount;
                TotalPages = TotalCount / pageSize;

                if (TotalCount % pageSize > 0)
                    TotalPages++;

                this.PageSize = pageSize;
                this.PageIndex = pageIndex;
                this.AddRange(source);
            }

            #endregion
        }
    
}
