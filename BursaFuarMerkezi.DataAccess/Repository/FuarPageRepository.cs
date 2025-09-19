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
                // Assign multilingual fields
                objFromDb.TitleTr = obj.TitleTr;
                objFromDb.TitleEn = obj.TitleEn;
                objFromDb.SubTitleTr = obj.SubTitleTr;
                objFromDb.SubTitleEn = obj.SubTitleEn;
                objFromDb.SlugTr = obj.SlugTr;
                objFromDb.SlugEn = obj.SlugEn;
                objFromDb.ContentTr = obj.ContentTr;
                objFromDb.ContentEn = obj.ContentEn;
                objFromDb.UpdatedAt = DateTime.Now;
                objFromDb.StartDate = obj.StartDate;
                objFromDb.EndDate = obj.EndDate;
                objFromDb.FairHall = obj.FairHall;
                objFromDb.OrganizerTr = obj.OrganizerTr;
                objFromDb.OrganizerEn = obj.OrganizerEn;
                objFromDb.VisitingHours = obj.VisitingHours;
                objFromDb.FairLocationTr = obj.FairLocationTr;
                objFromDb.FairLocationEn = obj.FairLocationEn;
                objFromDb.WebsiteUrl = obj.WebsiteUrl;
                objFromDb.IsPublished = obj.IsPublished;
                if (obj.FeaturedImageUrl != null)
                {
                    objFromDb.FeaturedImageUrl = obj.FeaturedImageUrl;
                }
                if (obj.CardImageUrl != null)
                {
                    objFromDb.CardImageUrl = obj.CardImageUrl;
                }
                objFromDb.MetaDescription = obj.MetaDescription;
                objFromDb.MetaKeywords = obj.MetaKeywords;

            }
        }
        // legacy slug methods removed

        public async Task<bool> IsSlugUniqueAsync(string slug, string language, int? id = null)
        {
            if (string.Equals(language, "en", StringComparison.OrdinalIgnoreCase))
            {
                return id.HasValue
                    ? await _db.FuarPages.FirstOrDefaultAsync(p => p.SlugEn == slug && p.Id != id) == null
                    : await _db.FuarPages.FirstOrDefaultAsync(p => p.SlugEn == slug) == null;
            }
            // default tr
            return id.HasValue
                ? await _db.FuarPages.FirstOrDefaultAsync(p => p.SlugTr == slug && p.Id != id) == null
                : await _db.FuarPages.FirstOrDefaultAsync(p => p.SlugTr == slug) == null;
        }

        public async Task<FuarPage> GetBySlugAsync(string slug, string language)
        {
            if (string.Equals(language, "en", StringComparison.OrdinalIgnoreCase))
            {
                return await _db.FuarPages.FirstOrDefaultAsync(p => p.SlugEn == slug);
            }
            return await _db.FuarPages.FirstOrDefaultAsync(p => p.SlugTr == slug);
        }

        public void UpdateSectors(FuarPage obj, IEnumerable<int> sectorIds)
        {
            var page = _db.FuarPages
                .Include(p => p.Sectors)
                .FirstOrDefault(p => p.Id == obj.Id);
            if (page == null) return;

            page.Sectors.Clear();
            var sectors = _db.Sectors.Where(s => sectorIds.Contains(s.Id)).ToList();
            foreach (var sector in sectors)
            {
                page.Sectors.Add(sector);
            }
        }
    }
}
