using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IStandRequestRepository : IRepository<StandRequest>
    {
        void Update(StandRequest obj);
    }
}
