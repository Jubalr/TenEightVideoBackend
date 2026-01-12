using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public class ResultSet<T> : IResultSet<T>
    {
        public ResultSet(IEnumerable<T> items)
        {
            Items = items;
            Pagination = new PagingInfo(1, items.Count(), items.Count());
        }

        private ResultSet(IEnumerable<T> items, PagingInfo pagination)
        {
            Items = items;
            Pagination = pagination;
        }

        public ResultSet(IEnumerable<T> items, int pageNumber, int pageSize, int totalRecordCount)
        {
            Items = items;
            Pagination = new PagingInfo(pageNumber, pageSize, totalRecordCount);
        }

        public PagingInfo Pagination { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public IResultSet<TargetType> ToType<TargetType>(Func<T, TargetType> mapper)
        {
            List<TargetType> mappedItems = new List<TargetType>();
            foreach (var item in Items)
            {
                mappedItems.Add(mapper(item));
            }
            var result = new ResultSet<TargetType>(mappedItems, Pagination);
            return result;
        }
    }
}
