using BursaFuarMerkezi.DataAccess.Pagination;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            // Get current date
            DateTime today = DateTime.Today;
            
            // Calculate date 2 months from now
            DateTime twoMonthsFromNow = today.AddMonths(2);
            
            // Get upcoming fuar pages (starting today or in the future, but not beyond 2 months)
            var upcomingFuars = _unitOfWork.FuarTests.GetAll()
                .Where(p => p.IsPublished && p.StartDate >= today && p.StartDate <= twoMonthsFromNow)
                .OrderBy(p => p.StartDate) // Order by nearest date first
                .ToList();
                
            // Pass upcoming fuars to the view
            return View(upcomingFuars);
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
        public JsonResult GetFuars([FromQuery] FuarFilterParams filterParams)
        {
            try {
                // Start with the base query - forcing IQueryable with AsQueryable()
                var query = _unitOfWork.FuarTests.GetAll().AsQueryable();
                
                // Apply filters if provided
                if (!string.IsNullOrEmpty(filterParams.Year))
                    query = query.Where(f => f.EndDate.Year.ToString() == filterParams.Year);
                    
                if (!string.IsNullOrEmpty(filterParams.Month))
                    query = query.Where(f => f.EndDate.ToString("MMMM") == filterParams.Month);
                    
                if (!string.IsNullOrEmpty(filterParams.Sector))
                    query = query.Where(f => f.Sector == filterParams.Sector);
                    
                if (!string.IsNullOrEmpty(filterParams.Location))
                    query = query.Where(f => f.City == filterParams.Location);
                
                // Get paged data with synchronous Create method
                var paginatedFuars = PaginatedList<FuarTest>.Create(
                    query, 
                    filterParams.PageNumber, 
                    filterParams.PageSize
                );
                
                // Map to DTOs
                var mappedItems = paginatedFuars.Items.Select(f => new
                {
                    id = f.Id,
                    title = f.Title,
                    date = $"{f.StartDate:dd} - {f.EndDate:dd} {f.EndDate:MMMM yyyy}",
                    year = f.EndDate.Year.ToString(),
                    month = f.EndDate.ToString("MMMM"),
                    sector = f.Sector ?? "Turizm",
                    location = f.City ?? "Bursa",
                    organizer = f.Organizer,
                    url = $"FuarTest/{f.Slug}",
                }).ToList();
                
                return Json(new
                {
                    data = mappedItems,
                    totalPages = paginatedFuars.TotalPages,
                    currentPage = paginatedFuars.PageNumber,
                    totalCount = paginatedFuars.TotalCount,
                    hasNext = paginatedFuars.HasNextPage,
                    hasPrevious = paginatedFuars.HasPreviousPage
                });
            }
            catch (Exception ex) {
                return Json(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        [Route("api/test-json")]
        public JsonResult TestJson()
        {
            return Json(new { message = "Test successful", timestamp = DateTime.Now });
        }
    }
}