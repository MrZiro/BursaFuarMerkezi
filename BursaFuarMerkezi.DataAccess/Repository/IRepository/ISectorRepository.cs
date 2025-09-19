namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    using BursaFuarMerkezi.Models;

    public interface ISectorRepository : IRepository<Sector>
    {
        void Update(Sector obj);
    }
}
