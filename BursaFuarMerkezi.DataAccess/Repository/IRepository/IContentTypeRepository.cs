using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IContentTypeRepository : IRepository<ContentType>
    {
        void Update(ContentType obj);
    }
} 