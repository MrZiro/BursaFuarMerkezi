using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BursaFuarMerkezi.web.Controllers
{
    public class FuarTestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FuarTestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Get all published fuar pages
            var fuarPages = _unitOfWork.FuarTests.GetAll()
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(fuarPages);
        }



        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // Get the page by slug
            var page = await _unitOfWork.FuarTests.GetBySlugAsync(slug);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpGet]
        [Route("fuartest/getfuars")]
        public JsonResult GetFuars()
        {
            var fuarList = _unitOfWork.FuarTests.GetAll()
                .Select(f => new
                {
                    id = f.Id,
                    title = f.Title,
                    date = $"{f.StartDate:dd} - {f.EndDate:dd} {f.EndDate:MMMM yyyy}",
                    year = f.EndDate.Year.ToString(),
                    month = f.EndDate.ToString("MMMM"),
                    sector = "Turizim",
                    location = f.City == "" ? "Bursa" : f.City,
                    organizer = f.Organizer,
                    url = $"FuarTest/{f.Slug}",
                })
                .ToList();
            return Json(new { data = fuarList });
        }

        [HttpGet]
        [Route("api/test-json")]
        public JsonResult TestJson()
        {
            return Json(new { message = "Test successful", timestamp = DateTime.Now });
        }
    }
}