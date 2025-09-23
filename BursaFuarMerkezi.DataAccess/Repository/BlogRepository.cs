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
                // Multilingual fields
                objFromDb.TitleTr = obj.TitleTr;
                objFromDb.TitleEn = obj.TitleEn;
                objFromDb.SlugTr = obj.SlugTr;
                objFromDb.SlugEn = obj.SlugEn;
                objFromDb.ContentTr = obj.ContentTr;
                objFromDb.ContentEn = obj.ContentEn;
                objFromDb.UpdatedAt = DateTime.Now;
                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.IsPublished = obj.IsPublished;
                if (obj.CardImageUrl != null)
                {
                    objFromDb.CardImageUrl = obj.CardImageUrl;
                }
                // SEO
                objFromDb.MetaDescriptionTr = obj.MetaDescriptionTr;
                objFromDb.MetaDescriptionEn = obj.MetaDescriptionEn;
                objFromDb.MetaKeywordsTr = obj.MetaKeywordsTr;
                objFromDb.MetaKeywordsEn = obj.MetaKeywordsEn;
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
            var lang = (language ?? string.Empty).Trim().ToLowerInvariant();
            var useTr = lang == "tr";

            return await _db.Blogs
                .Include(b => b.ContentType)
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .Take(count)
                .Select(b => new
                {
                    slug = useTr ? b.SlugTr : b.SlugEn,
                    contentType = b.ContentType != null ? (useTr ? b.ContentType.NameTr : b.ContentType.NameEn) : null,
                    createdAt = b.CreatedAt,
                    cardImage = b.CardImageUrl,
                    title = useTr ? b.TitleTr : b.TitleEn,
                    language = useTr ? "tr" : "en"
                })
                .Cast<object>()
                .ToListAsync();
        }

    }
}
