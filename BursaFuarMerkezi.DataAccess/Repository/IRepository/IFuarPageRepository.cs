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
        Task<FuarPage> GetBySlugAsync(string slug);
        Task<List<FuarPage>> GetPublishedPagesAsync();
        Task<bool> IsSlugUniqueAsync(string slug, int? id = null);
    }
} 