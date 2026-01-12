using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public interface IResultSet<out T>
    {
        PagingInfo Pagination { get; }
        IEnumerable<T> Items { get; }

        IResultSet<TargetType> ToType<TargetType>(Func<T, TargetType> mapper);
    }
}
