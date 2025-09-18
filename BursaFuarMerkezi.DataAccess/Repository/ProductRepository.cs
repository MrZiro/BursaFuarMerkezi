using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
        public async Task<(IEnumerable<Product> data, int filteredTotal, int total)> GetPagedAsync(
        int start, int length, string orderColumn, string orderDirection, 
        string searchValue, string? includeProperties = null)
        {
            IQueryable<Product> query = dbSet;
            
            // Include properties
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }
            
            // Count total before any filtering
            int total = await query.CountAsync();
            
            // Apply search
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                string search = searchValue.ToLower();
                query = query.Where(p =>
                    (p.Title != null && p.Title.ToLower().Contains(search)) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    (p.Author != null && p.Author.ToLower().Contains(search)) ||
                    (p.ISBN != null && p.ISBN.ToLower().Contains(search)) ||
                    (p.Category != null && p.Category.Name != null && p.Category.Name.ToLower().Contains(search))
                );
            }
            
            // Get filtered count
            int filteredTotal = await query.CountAsync();
            
            // Apply sorting
            bool isDescending = orderDirection?.ToLower() == "desc";
            orderColumn = orderColumn?.ToLower() ?? "id";
            query = orderColumn switch
            {
                "title" => isDescending ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "author" => isDescending ? query.OrderByDescending(p => p.Author) : query.OrderBy(p => p.Author),
                "isbn" => isDescending ? query.OrderByDescending(p => p.ISBN) : query.OrderBy(p => p.ISBN),
                "category.name" or "category" => isDescending
                    ? query.OrderByDescending(p => p.Category.Name)
                    : query.OrderBy(p => p.Category.Name),
                _ => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            };
            
            // Apply pagination
            var data = await query.Skip(start)
                                .Take(length > 0 ? length : filteredTotal)
                                .ToListAsync();
            
            return (data, filteredTotal, total);
        }
    }
}
