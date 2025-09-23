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
            var objFromDb = _db.ContentTypes.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.NameTr = obj.NameTr;
                objFromDb.NameEn = obj.NameEn;
            }
        }

        public string GetNameByLanguage(ContentType contentType, string language)
        {
            if (contentType == null) return string.Empty;
            
            return language?.ToLower() switch
            {
                "en" => contentType.NameEn ?? contentType.NameTr,
                "tr" or _ => contentType.NameTr ?? contentType.NameEn
            };
        }
    }
} 