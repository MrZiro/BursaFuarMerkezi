using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IContactRepository : IRepository<Contact>
    {
        void Update(Contact obj);
    }
}
