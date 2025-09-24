using BursaFuarMerkezi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface ISliderRepository : IRepository<Slider>
    {
        public void Update(Slider obj);
        public Task<List<Slider>> GetActiveSlidersByOrderAsync();
        public Task<Slider?> GetSliderByIdAsync(int id);
    }
}