using BursaFuarMerkezi.DataAccess.Pagination;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlogsController> _logger;

        public BlogsController(IUnitOfWork unitOfWork, ILogger<BlogsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Blogs/Index
        public IActionResult Index()
        {
            //// Get all published blogs with their content types
            //var blogs = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
            //    .Where(b => b.IsPublished)
            //    .OrderByDescending(b => b.CreatedAt)
            //    .ToList();
            var contentTypes = _unitOfWork.ContentType.GetAll().ToList();
            return View(contentTypes);
        }

        // GET: Blogs/slug
        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // Find blog by slug with ContentType included
            var blog = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                .FirstOrDefault(b => b.Slug == slug && b.IsPublished);

            if (blog == null)
            {
                _logger.LogWarning($"Blog with slug '{slug}' not found or not published");
                return NotFound();
            }

            return View(blog);
        }

        // GET: /Blogs/GetBlogs
        [HttpGet]
        [Route("blog/getblogs")]
        public JsonResult GetBlogs([FromQuery] BlogFilterParams filterParams)
        {
            try
            {
                // Start with base query
                var query = _unitOfWork.Blog.GetAll(includeProperties: "ContentType").AsQueryable();

                // Only include published blogs
                query = query.Where(b => b.IsPublished);

                // Apply category filter if provided
                if (!string.IsNullOrEmpty(filterParams.Category) && filterParams.Category != "all")
                {
                    query = query.Where(b => b.ContentType != null && b.ContentType.Name == filterParams.Category);
                }

                // Order by creation date (newest first)
                query = query.OrderByDescending(b => b.CreatedAt);

                // Create paginated list
                var paginatedBlogs = PaginatedList<Blog>.Create(
                    query,
                    filterParams.PageNumber,
                    filterParams.PageSize
                );

                // Map to DTOs
                var mappedItems = paginatedBlogs.Items.Select(b => new
                {
                    id = b.Id,
                    title = b.Title,
                    category = b.ContentType?.Name ?? "Uncategorized",
                    date = b.CreatedAt.Day.ToString("D2"),
                    month = b.CreatedAt.ToString("MMM"),
                    author = b.Author,
                    image = b.CardImageUrl,
                    slug = b.Slug,
                    url = $"/blogs/{b.Slug}"
                }).ToList();

                return Json(new
                {
                    data = mappedItems,
                    totalPages = paginatedBlogs.TotalPages,
                    currentPage = paginatedBlogs.PageNumber,
                    totalCount = paginatedBlogs.TotalCount,
                    hasNext = paginatedBlogs.HasNextPage,
                    hasPrevious = paginatedBlogs.HasPreviousPage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBlogs");
                return Json(new { error = ex.Message });
            }
        }

        // Helper class for blog filtering and pagination
        public class BlogFilterParams
        {
            public string Category { get; set; } = "all";
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 9;
        }
    }
}