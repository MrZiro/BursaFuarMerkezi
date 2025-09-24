using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang}")]
    public class FairCalendarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();
        public FairCalendarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("fuar-takvimi")]
        [Route("fair-calendar")]
        public IActionResult FairCalendar()
        {
            var fuarlar = _unitOfWork.FuarPages.GetAll(filter: x => x.IsPublished == true, includeProperties: "Sectors", orderBy: o => o.OrderBy(x => x.StartDate));

            var viewModel = new FairCalendarViewModel
            {
                Fairs = fuarlar.ToList()
            };

            return View(viewModel);
        }
    }
}
