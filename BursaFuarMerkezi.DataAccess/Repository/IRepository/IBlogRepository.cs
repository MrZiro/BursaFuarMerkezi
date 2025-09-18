using BursaFuarMerkezi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        public void Update(Blog obj);
        // No changes needed now; gallery handled via BlogImage repository
    }
}
