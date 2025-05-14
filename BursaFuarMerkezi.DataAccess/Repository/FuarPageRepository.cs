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
                objFromDb.UpdatedAt = DateTime.Now;
                objFromDb.StartDate = obj.StartDate;
                objFromDb.EndDate = obj.EndDate;
                objFromDb.FairHall = obj.FairHall;
                objFromDb.Organizer = obj.Organizer;
                objFromDb.VisitingHours = obj.VisitingHours;
                objFromDb.FairLocation = obj.FairLocation;
                objFromDb.WebsiteUrl = obj.WebsiteUrl;
                objFromDb.IsPublished = obj.IsPublished;
                if (obj.FeaturedImageUrl != null)
                {
                    objFromDb.FeaturedImageUrl = obj.FeaturedImageUrl;
                }
                objFromDb.MetaDescription = obj.MetaDescription;
                objFromDb.MetaKeywords = obj.MetaKeywords;

            }
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
        public async Task<FuarPage> GetBySlugAsync(string slug)
        {
            return await _db.FuarPages.FirstOrDefaultAsync(p => p.Slug == slug);
        }
    }
}
