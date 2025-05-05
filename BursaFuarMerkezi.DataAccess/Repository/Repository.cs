using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db) {
            _db = db;
            this.dbSet = db.Set<T>();
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            } 
            return query.ToList();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }


        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public async Task<(IEnumerable<T> data, int filteredTotal, int total)> GetPagedAsync(
            int start, int length, string orderColumn, string orderDirection, 
            string searchValue, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            
            // Include properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            
            // Count total before any filtering
            int total = await query.CountAsync();
            
            // Apply filtering logic (to be overridden in specific repositories)
            int filteredTotal = total;
            
            // Apply pagination
            var data = await query.Skip(start)
                                .Take(length > 0 ? length : total)
                                .ToListAsync();
            
            return (data, filteredTotal, total);
        }
    }
}
