using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;

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
            var fromDb = _db.Sliders.FirstOrDefault(s => s.Id == obj.Id);
            if (fromDb == null) return;
            fromDb.DisplayOrder = obj.DisplayOrder;
            fromDb.IsActive = obj.IsActive;
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                fromDb.ImageUrl = obj.ImageUrl;
            }
        }
    }
}


