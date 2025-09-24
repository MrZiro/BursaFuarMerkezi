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
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;
        
        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Slider obj)
        {
            var objFromDb = _db.Sliders.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                // Multilingual fields
                objFromDb.TitleTr = obj.TitleTr;
                objFromDb.TitleEn = obj.TitleEn;
                objFromDb.DescriptionTr = obj.DescriptionTr;
                objFromDb.DescriptionEn = obj.DescriptionEn;
                objFromDb.ButtonTextTr = obj.ButtonTextTr;
                objFromDb.ButtonTextEn = obj.ButtonTextEn;
                
                // Image fields
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
                if (!string.IsNullOrEmpty(obj.MobileImageUrl))
                {
                    objFromDb.MobileImageUrl = obj.MobileImageUrl;
                }
                objFromDb.AltText = obj.AltText;
                
                // Link and behavior
                objFromDb.LinkUrl = obj.LinkUrl;
                objFromDb.OpenInNewTab = obj.OpenInNewTab;
                
                // Display settings
                objFromDb.DisplayOrder = obj.DisplayOrder;
                objFromDb.IsActive = obj.IsActive;
                
                // Audit fields
                objFromDb.UpdatedAt = DateTime.Now;
                objFromDb.CreatedAt = obj.CreatedAt;
            }
        }

        public async Task<List<Slider>> GetActiveSlidersByOrderAsync()
        {
            return await _db.Sliders
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Slider?> GetSliderByIdAsync(int id)
        {
            return await _db.Sliders
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}