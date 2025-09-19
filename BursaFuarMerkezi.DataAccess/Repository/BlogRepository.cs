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
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {

        private readonly ApplicationDbContext _db;
        public BlogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Blog obj)
        {
            var objFromDb = _db.Blogs.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Slug = obj.Slug;
                objFromDb.Content = obj.Content;
                objFromDb.UpdatedAt = DateTime.Now;
                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.IsPublished = obj.IsPublished;
                if (obj.CardImageUrl != null)
                {
                    objFromDb.CardImageUrl = obj.CardImageUrl;
                }
                objFromDb.MetaDescription = obj.MetaDescription;
                objFromDb.MetaKeywords = obj.MetaKeywords;
                objFromDb.Author = obj.Author;
                objFromDb.ContentTypeId = obj.ContentTypeId;

            }
        }

        public async Task<List<Blog>> GetLatestBlogsAsync(int count)
        {
            return await _db.Blogs
                .OrderByDescending(b => b.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<object>> GetLatestBlogsWithFieldsAsync(int count, string language)
        {
            return await _db.Blogs
                .Include(b => b.ContentType)
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .Take(count)
                .Select(b => new
                {
                    slug = b.Slug,
                    contentType = b.ContentType != null ? b.ContentType.Name : null,
                    createdAt = b.CreatedAt,
                    cardImage = b.CardImageUrl,
                    title = b.Title,
                    language = language
                })
                .Cast<object>()
                .ToListAsync();
        }
    }
}
