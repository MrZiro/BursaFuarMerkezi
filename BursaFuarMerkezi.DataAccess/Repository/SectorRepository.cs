using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class SectorRepository : Repository<Sector>, ISectorRepository
    {
        private readonly ApplicationDbContext _db;
        public SectorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Sector obj)
        {
            _db.Sectors.Update(obj);
        }
    }
}


