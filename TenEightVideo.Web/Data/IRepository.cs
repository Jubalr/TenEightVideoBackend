using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public interface IRepository<T> : IRepository where T : class
    {
        T? GetFirstOrDefault(Func<T, bool> selector);
        void Add(T obj);
        void Update(T obj);
        void Delete(ref T? obj);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll<TSort>(Func<T, TSort> orderBy, bool ascending);
        IResultSet<T> GetAll<TSort>(Func<T, TSort> orderBy, bool ascending, int pageNumber, int pageSize);
        IEnumerable<T> GetAll(Func<T, bool> selector);
        IResultSet<T> GetAll<TSort>(Func<T, bool> selector, Func<T, TSort> orderBy, bool ascending, int pageNumber, int pageSize);
        IEnumerable<T> GetAll<TSort>(Func<T, bool> selector, Func<T, TSort> orderBy, bool ascending);
        int Count(Func<T, bool> selector);
    }

    public interface IRepository
    {

    }
}
