using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using System.Linq;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class BlogImageRepository : Repository<BlogImage>, IBlogImageRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(BlogImage obj)
        {
            var fromDb = _db.BlogImages.FirstOrDefault(x => x.Id == obj.Id);
            if (fromDb != null)
            {
                fromDb.ImageUrl = obj.ImageUrl;
                fromDb.Caption = obj.Caption;
                fromDb.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}




