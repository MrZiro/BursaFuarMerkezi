using BursaFuarMerkezi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IFuarPageRepository : IRepository<FuarPage>
    {
        void Update(FuarPage obj);
        Task<bool> IsSlugUniqueAsync(string slug, string language, int? id = null);
        Task<FuarPage> GetBySlugAsync(string slug, string language);
        void UpdateSectors(FuarPage obj, IEnumerable<int> sectorIds);

    }
}
