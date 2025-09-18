using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IBlogImageRepository : IRepository<BlogImage>
    {
        void Update(BlogImage obj);
    }
}




