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
    public class FuarPageRepository : Repository<FuarPage>, IFuarPageRepository
    {
        private readonly ApplicationDbContext _db;

        public FuarPageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(FuarPage obj)
        {
            var objFromDb = _db.FuarPages.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Slug = obj.Slug;
                objFromDb.Content = obj.Content;
                objFromDb.IsPublished = obj.IsPublished;
                objFromDb.MetaDescription = obj.MetaDescription;
                objFromDb.MetaKeywords = obj.MetaKeywords;
                objFromDb.PageType = obj.PageType;
                objFromDb.UpdatedAt = DateTime.Now;
                
                if (obj.FeaturedImageUrl != null)
                {
                    objFromDb.FeaturedImageUrl = obj.FeaturedImageUrl;
                }
            }
        }

        public async Task<FuarPage> GetBySlugAsync(string slug)
        {
            return await _db.FuarPages.FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<List<FuarPage>> GetPublishedPagesAsync()
        {
            return await _db.FuarPages
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, int? id = null)
        {
            if (id.HasValue)
            {
                // Check if slug exists but not for this page (for updates)
                return await _db.FuarPages.FirstOrDefaultAsync(p => p.Slug == slug && p.Id != id) == null;
            }
            else
            {
                // Check if slug exists at all (for new pages)
                return await _db.FuarPages.FirstOrDefaultAsync(p => p.Slug == slug) == null;
            }
        }
    }
} 