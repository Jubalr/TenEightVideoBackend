using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.Web.Data
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public EFRepository(DbContextOptions<TenEightVideoDbContext> options)
        {
            Options = options;
        }
        public DbContextOptions<TenEightVideoDbContext> Options { get; private set; }

        public T? GetFirstOrDefault(Func<T, bool> selector)
        {
            using (var context = GetDataContext())
            {
                var obj = context.Set<T>().FirstOrDefault(selector);
                return obj;
            }
        }

        public TenEightVideoDbContext GetDataContext()
        {
            return new TenEightVideoDbContext(Options);
        }

        public void Add(T obj)
        {
            using (var context = GetDataContext())
            {
                context.Add(obj);
                context.SaveChanges();
            }
        }

        public virtual void Delete(ref T? obj)
        {
            using (var context = GetDataContext())
            {
                context.Update(obj!);
                context.SaveChanges();
                obj = null;
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var context = GetDataContext())
            {
                var objects = context.Set<T>().ToArray();
                return objects;
            }
        }

        public IResultSet<T> GetAll<TSort>(Func<T, TSort> orderBy, bool ascending, int pageNumber, int pageSize)
        {
            using (var context = GetDataContext())
            {
                IEnumerable<T> query;
                if (ascending)
                    query = context.Set<T>().OrderBy(orderBy);
                else
                    query = context.Set<T>().OrderByDescending(orderBy);
                var skip = (pageNumber - 1) * pageSize;
                if (skip > 0)
                    query = query.Skip(skip);
                var result = query.Take(pageSize).ToArray();
                int totalRecordCount = context.Set<T>().Count();
                return new ResultSet<T>(result, pageNumber, pageSize, totalRecordCount);
            }
        }

        public IResultSet<T> GetAll<TSort>(Func<T, bool> selector, Func<T, TSort> orderBy, bool ascending, int pageNumber, int pageSize)
        {
            using (var context = GetDataContext())
            {
                IEnumerable<T> query;
                if (ascending)
                    query = context.Set<T>().Where(selector).OrderBy(orderBy);
                else
                    query = context.Set<T>().Where(selector).OrderByDescending(orderBy);
                var skip = (pageNumber - 1) * pageSize;
                if (skip > 0)
                    query = query.Skip(skip);
                var result = query.Take(pageSize).ToArray();
                int totalRecordCount = context.Set<T>().Count();
                return new ResultSet<T>(result, pageNumber, pageSize, totalRecordCount);
            }
        }

        public IEnumerable<T> GetAll(Func<T, bool> selector)
        {
            using (var context = GetDataContext())
            {
                return context.Set<T>().Where(selector).ToArray();
            }
        }

        public IEnumerable<T> GetAll<TSort>(Func<T, TSort> orderBy, bool ascending)
        {
            using (var context = GetDataContext())
            {
                if (ascending)
                    return context.Set<T>().OrderBy(orderBy).ToArray();
                else
                    return context.Set<T>().OrderByDescending(orderBy).ToArray();
            }
        }

        public IEnumerable<T> GetAll<TSort>(Func<T, bool> selector, Func<T, TSort> orderBy, bool ascending)
        {
            using (var context = GetDataContext())
            {
                if (ascending)
                    return context.Set<T>().Where(selector).OrderBy(orderBy).ToArray();
                else
                    return context.Set<T>().Where(selector).OrderByDescending(orderBy).ToArray();
            }
        }

        public int Count(Func<T, bool> selector)
        {
            using (var context = GetDataContext())
            {
                var count = context.Set<T>().Count(selector);
                return count;
            }
        }


        public void Update(T obj)
        {
            using (var context = GetDataContext())
            {
                context.Update(obj);
                context.SaveChanges();
            }
        }
    }
}
