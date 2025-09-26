
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var dashboardStats = new DashboardStatsVM
            {
                TotalBlogs = _unitOfWork.Blog.GetAll().Count(),
                TotalFuars = _unitOfWork.FuarPages.GetAll().Count(),
                TotalContacts = _unitOfWork.Contact.GetAll().Count(),
                TotalStandRequests = _unitOfWork.StandRequest.GetAll().Count(),
                TotalSectors = _unitOfWork.Sector.GetAll().Count(),
                RecentBlogs = _unitOfWork.Blog.GetAll(orderBy: q => q.OrderByDescending(b => b.CreatedAt)).Take(5).ToList(),
                RecentContacts = _unitOfWork.Contact.GetAll(orderBy: q => q.OrderByDescending(c => c.CreatedAt)).Take(5).ToList(),
                RecentStandRequests = _unitOfWork.StandRequest.GetAll(orderBy: q => q.OrderByDescending(s => s.CreatedAt)).Take(5).ToList()
            };

            return View(dashboardStats);
        }

        public IActionResult Pages() {
        
            return View();
        }

        public IActionResult Test()
        {

            return View();
        }

    }
}
