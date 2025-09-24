using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class StandRequestRepository : Repository<StandRequest>, IStandRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public StandRequestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(StandRequest obj)
        {
            _db.StandRequests.Update(obj);
        }
    }
}
