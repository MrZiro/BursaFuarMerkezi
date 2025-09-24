using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IFuarPageRepository FuarPages { get; }
        IContentTypeRepository ContentType { get; }
        ISectorRepository Sector { get; }
        ISliderRepository Slider { get; }
        IBlogRepository Blog { get; }
        IBlogImageRepository BlogImage { get; }
        IStandRequestRepository StandRequest { get; }
        IContactRepository Contact { get; }

        void Save();
    }
}
