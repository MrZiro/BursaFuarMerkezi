using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IFuarPageRepository FuarPages { get; private set; }
        public IContentTypeRepository ContentType { get; private set; }
        public ISectorRepository Sector { get; private set; }
        public ISliderRepository Slider { get; private set; }
        public IBlogRepository Blog { get; private set; }
        public IBlogImageRepository BlogImage { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            FuarPages = new FuarPageRepository(_db);
            ContentType = new ContentTypeRepository(_db);
            Sector = new SectorRepository(_db);
            Slider = new SliderRepository(_db);
            Blog = new BlogRepository(_db);
            BlogImage = new BlogImageRepository(_db);
        }

        public void Save()
        {
             _db.SaveChanges();
        }
    }
}
