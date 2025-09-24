using BursaFuarMerkezi.DataAccess.Pagination;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Mvc;
using BursaFuarMerkezi.web.Services;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang}")]

    public class BlogsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlogsController> _logger;
        private readonly IUrlLocalizationService _urlService;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public BlogsController(IUnitOfWork unitOfWork, ILogger<BlogsController> logger, IUrlLocalizationService urlService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _urlService = urlService;
        }

        // GET: Blogs/Index
        [HttpGet("all-news")]
        [HttpGet("tum-haberler")]
        public IActionResult Index()
        {
            //// Get all published blogs with their content types
            //var blogs = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
            //    .Where(b => b.IsPublished)
            //    .OrderByDescending(b => b.CreatedAt)
            //    .ToList();
            var contentTypes = _unitOfWork.ContentType.GetAll().ToList();
            ViewBag.CanonicalUrl = _urlService.GetCanonicalUrl("Blogs", "Index", Lang);
            ViewBag.AlternateUrls = _urlService.GetAlternateLanguageUrls("Blogs", "Index", Lang);
            return View(contentTypes);
        }

        // GET: Blogs/slug
        [HttpGet]
        [Route("blog-detail/{slug}")]
        [Route("blog-detay/{slug}")]
        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // Find blog by slug (TR/EN) with ContentType and BlogImages included
            var blog = _unitOfWork.Blog.GetAll(includeProperties: "ContentType,BlogImages")
                .FirstOrDefault(b => (Lang == "en" ? b.SlugEn : b.SlugTr) == slug && b.IsPublished);

            if (blog == null)
            {
                _logger.LogWarning($"Blog with slug '{slug}' not found or not published");
                return NotFound();
            }

        return View();
    }

    // GET: /Blogs/GetBlogDetails/{slug}
    [HttpGet]
    [Route("blog/details/{slug}")]
    public JsonResult GetBlogDetails(string slug)
    {
        try
        {
            if (string.IsNullOrEmpty(slug))
            {
                return Json(new { success = false, message = "Invalid slug" });
            }

            var blog = _unitOfWork.Blog.GetAll(includeProperties: "ContentType,BlogImages")
                .FirstOrDefault(b => (Lang == "en" ? b.SlugEn : b.SlugTr) == slug && b.IsPublished);

            if (blog == null)
            {
                return Json(new { success = false, message = "Blog not found" });
            }

            var galleryImages = blog.BlogImages?.OrderBy(i => i.DisplayOrder).Select(img => new
            {
                imageUrl = img.ImageUrl,
                displayOrder = img.DisplayOrder
            }).ToList();

            var result = new
            {
                success = true,
                data = new
                {
                    title = Lang == "en" ? blog.TitleEn : blog.TitleTr,
                    content = Lang == "en" ? blog.ContentEn : blog.ContentTr,
                    cardImageUrl = blog.CardImageUrl,
                    createdAt = blog.CreatedAt.ToString("dd MMMM yyyy"),
                    category = blog.ContentType != null ? (Lang == "en" ? blog.ContentType.NameEn : blog.ContentType.NameTr) : "",
                    galleryImages = galleryImages
                }
            };

            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching blog details for slug: {slug}", slug);
            return Json(new { success = false, message = "Error loading blog" });
        }
    }

    // GET: /Blogs/GetBlogs
        [HttpGet]
        [Route("blog/getblogs")]
        public JsonResult GetBlogs([FromQuery] BlogFilterParams filterParams)
        {
            try
            {
                // Get all published blogs with their content types
                var blogsQuery = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                    .Where(b => b.IsPublished);

                // Debug: Log total published blogs
                var totalPublishedBlogs = blogsQuery.Count();
                _logger.LogInformation($"Total published blogs: {totalPublishedBlogs}");

                // Debug: Log available categories
                var availableCategories = _unitOfWork.ContentType.GetAll()
                    .Select(c => new { NameTr = c.NameTr, NameEn = c.NameEn })
                    .ToList();
                _logger.LogInformation($"Available categories: {string.Join(", ", availableCategories.Select(c => $"TR:{c.NameTr}, EN:{c.NameEn}"))}");
                _logger.LogInformation($"Requested category: {filterParams.Category}, Language: {Lang}");

                // Filter by category if specified
                if (!string.IsNullOrEmpty(filterParams.Category) && filterParams.Category != "all")
                {
                    blogsQuery = blogsQuery.Where(b => b.ContentType != null && 
                        (Lang == "en" ? b.ContentType.NameEn.ToLower() : b.ContentType.NameTr.ToLower()) == filterParams.Category.ToLower());
                    
                    // Debug: Log filtered count
                    var filteredCount = blogsQuery.Count();
                    _logger.LogInformation($"Blogs after category filter: {filteredCount}");
                }

                // Apply pagination
                var totalBlogs = blogsQuery.Count();
                var blogs = blogsQuery
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                    .Take(filterParams.PageSize)
                    .Select(b => new
                    {
                        id = b.Id,
                        title = Lang == "en" ? b.TitleEn : b.TitleTr,
                        shortDescription = Lang == "en" ? b.ShortDescriptionEn : b.ShortDescriptionTr,
                        slug = Lang == "en" ? b.SlugEn : b.SlugTr,
                        cardImageUrl = b.CardImageUrl,
                        createdAt = b.CreatedAt.ToString("dd.MM.yyyy"),
                        category = b.ContentType != null ? (Lang == "en" ? b.ContentType.NameEn : b.ContentType.NameTr) : "",
                        detailUrl = Lang == "en" ? $"/en/blog-detail/{b.SlugEn}" : $"/tr/blog-detay/{b.SlugTr}"
                    })
                    .ToList();

                return Json(new
                {
                    success = true,
                    data = blogs,
                    totalCount = totalBlogs,
                    currentPage = filterParams.PageNumber,
                    pageSize = filterParams.PageSize,
                    totalPages = (int)Math.Ceiling((double)totalBlogs / filterParams.PageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching blogs");
                return Json(new
                {
                    success = false,
                    message = "Error occurred while fetching blogs"
                });
            }
    }
}
}