using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IFuarTestRepository FuarTests { get; }
        IFuarPageRepository FuarPages { get; }
        IContentTypeRepository ContentType { get; }
        IBlogRepository Blog { get; }

        void Save();
    }
}
