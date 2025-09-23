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
        public Task<List<Blog>> GetLatestBlogsAsync(int count);
        public Task<List<object>> GetLatestBlogsWithFieldsAsync(int count, string language);
    }
}
