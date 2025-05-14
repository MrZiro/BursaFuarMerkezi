using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class ContentTypeRepository : Repository<ContentType>, IContentTypeRepository
    {
        private readonly ApplicationDbContext _db;
        
        public ContentTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(ContentType obj)
        {
            _db.ContentTypes.Update(obj);
        }
    }
} 