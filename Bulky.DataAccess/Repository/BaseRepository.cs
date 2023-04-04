using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        public DbSet<T> _dbset;
        public BaseRepository(ApplicationDBContext db)
        {
            _db = db;
            _dbset = _db.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string[] Includes = null)
        {
            IQueryable<T> query = _dbset.Where(filter);
            if (Includes != null)
            {
                foreach (var item in Includes)
                {
                    query = query.Include(item);
                }
            }
            
            return query.FirstOrDefault();
        }


        public IEnumerable<T> GetAll(string[] Includes = null)
        {
            IQueryable<T> query = _dbset;
            if (Includes != null)
            {
                foreach (var item in Includes)
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }


        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
        }
    }
}
