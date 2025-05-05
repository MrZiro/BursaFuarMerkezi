using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BursaFuarMerkezi.web.Controllers
{
    public class FuarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FuarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Get all published fuar pages
            var fuarPages = _unitOfWork.FuarPage.GetAll()
                .Where(p => p.IsPublished && p.PageType == "fuar")
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
            var page = await _unitOfWork.FuarPage.GetBySlugAsync(slug);

            if (page == null || !page.IsPublished)
            {
                return NotFound();
            }

            return View(page);
        }
    }
} 