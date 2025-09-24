using BursaFuarMerkezi.DataAccess.Pagination;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BursaFuarMerkezi.web.Services;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang}")]

    public class FuarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FuarController> _logger;
        private readonly IUrlLocalizationService _urlService;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public FuarController(IUnitOfWork unitOfWork, ILogger<FuarController> logger, IUrlLocalizationService urlService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _urlService = urlService;
        }

        [HttpGet("fair-detail/{slug}")]
        [HttpGet("fuar-detay/{slug}")]
        public IActionResult FuarDetail(string slug)
        {
            var fuarPage = _unitOfWork.FuarPages.Get(
                filter: Lang == "tr" ? x => x.SlugTr == slug : x => x.SlugEn == slug,
                includeProperties: "Sectors"
            );

            if (fuarPage == null)
            {
                return NotFound();
            }

            // Get alternate slug for language switcher
            string? alternateSlug = Lang == "tr" ? fuarPage.SlugEn : fuarPage.SlugTr;
            if (!string.IsNullOrEmpty(alternateSlug))
            {
                ViewData["alternateSlug"] = alternateSlug;
            }

            var vm = new FuarPageVM
            {
                FuarPage = fuarPage
            };

            return View(vm);
        }
    }
}