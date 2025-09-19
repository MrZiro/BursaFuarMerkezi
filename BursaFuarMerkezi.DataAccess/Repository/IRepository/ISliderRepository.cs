namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    using BursaFuarMerkezi.Models;

    public interface ISliderRepository : IRepository<Slider>
    {
        void Update(Slider obj);
    }
}


