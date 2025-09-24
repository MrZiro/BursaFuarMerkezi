using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
namespace BursaFuarMerkezi.web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;


    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    [HttpGet("")]
    [HttpGet("{lang:regex(^tr|en$)}")]
    [HttpGet("{lang}/home")]
    [HttpGet("{lang}/anasayfa")]

    public async Task<IActionResult> Index()
    {
        var vm = new HomeVM
        {
            // Sliders only for now; other sections can be filled later
            Sliders = await _unitOfWork.Slider.GetActiveSlidersByOrderAsync()
        };

        return View(vm);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    // Get upcoming fairs (next 4 months)
    [HttpGet("{lang}/home/upcoming-fairs")]
    public async Task<JsonResult> GetUpcomingFairs()
    {
        try
        {
            var lang = RouteData.Values["lang"]?.ToString() ?? "tr";
            var useTr = lang == "tr";
            var today = DateTime.Today;
            var fourMonthsLater = today.AddMonths(4);

            var fairs = _unitOfWork.FuarPages.GetAll()
                .Where(f => f.IsPublished && f.StartDate >= today && f.StartDate <= fourMonthsLater)
                .OrderBy(f => f.StartDate)
                .Select(f => new
                {
                    id = f.Id,
                    title = useTr ? f.TitleTr : f.TitleEn,
                    subtitle = useTr ? f.SubTitleTr : f.SubTitleEn,
                    cardImageUrl = f.CardImageUrl ?? "/images/default-fair.png",
                    startDate = f.StartDate.ToString("dd MMMM yyyy"),
                    endDate = f.EndDate.ToString("dd MMMM yyyy"),
                    fairHall = f.FairHall,
                    organizer = useTr ? f.OrganizerTr : f.OrganizerEn,
                    detailUrl = useTr ? $"/tr/fuar-detay/{f.SlugTr}" : $"/en/fair-detail/{f.SlugEn}"
                })
                .ToList();

            return Json(new { success = true, data = fairs });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching upcoming fairs");
            return Json(new { success = false, message = "Error loading upcoming fairs" });
        }
    }

    // Get all fairs with year filter
    [HttpGet("{lang}/home/all-fairs")]
    public async Task<JsonResult> GetAllFairs(int? year = null)
    {
        try
        {
            var lang = RouteData.Values["lang"]?.ToString() ?? "tr";
            var useTr = lang == "tr";

            var query = _unitOfWork.FuarPages.GetAll()
                .Where(f => f.IsPublished);

            // Filter by year if provided
            if (year.HasValue)
            {
                query = query.Where(f => f.StartDate.Year == year.Value);
            }

            var fairs = query
                .OrderBy(f => f.StartDate)
                .Select(f => new
                {
                    id = f.Id,
                    title = useTr ? f.TitleTr : f.TitleEn,
                    subtitle = useTr ? f.SubTitleTr : f.SubTitleEn,
                    cardImageUrl = f.CardImageUrl ?? "/images/default-fair.png",
                    startDate = f.StartDate.ToString("dd MMMM yyyy"),
                    endDate = f.EndDate.ToString("dd MMMM yyyy"),
                    fairHall = f.FairHall,
                    organizer = useTr ? f.OrganizerTr : f.OrganizerEn,
                    detailUrl = useTr ? $"/tr/fuar-detay/{f.SlugTr}" : $"/en/fair-detail/{f.SlugEn}"
                })
                .ToList();

            return Json(new { success = true, data = fairs });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all fairs");
            return Json(new { success = false, message = "Error loading fairs" });
        }
    }

    // Get available years for year filter
    [HttpGet("{lang}/home/fair-years")]
    public async Task<JsonResult> GetFairYears()
    {
        try
        {
            var years = _unitOfWork.FuarPages.GetAll()
                .Where(f => f.IsPublished)
                .Select(f => f.StartDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            return Json(new { success = true, data = years });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching fair years");
            return Json(new { success = false, message = "Error loading years" });
        }
    }

    // Get content types for category tabs
    [HttpGet("{lang}/home/categories")]
    public async Task<JsonResult> GetCategories()
    {
        try
        {
            var lang = RouteData.Values["lang"]?.ToString() ?? "tr";
            var useTr = lang == "tr";

            var categories = _unitOfWork.ContentType.GetAll()
                .Select(c => new
                {
                    id = c.Id,
                    name = useTr ? c.NameTr : c.NameEn,
                    key = useTr ? c.NameTr.ToLower() : c.NameEn.ToLower()
                })
                .ToList();

            return Json(new { success = true, data = categories });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories");
            return Json(new { success = false, message = "Error loading categories" });
        }
    }

    // Get months from fairs for filter dropdown
    [HttpGet("{lang}/home/fair-months")]
    public async Task<JsonResult> GetFairMonths()
    {
        try
        {
            var lang = RouteData.Values["lang"]?.ToString() ?? "tr";

            var months = _unitOfWork.FuarPages.GetAll()
                .Where(f => f.IsPublished)
                .Select(f => f.StartDate.Month)
                .Distinct()
                .OrderBy(m => m)
                .Select(m => new
                {
                    value = m,
                    name = new DateTime(2024, m, 1).ToString("MMMM", 
                        lang == "tr" ? new System.Globalization.CultureInfo("tr-TR") : 
                                      new System.Globalization.CultureInfo("en-US"))
                })
                .ToList();

            return Json(new { success = true, data = months });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching fair months");
            return Json(new { success = false, message = "Error loading months" });
        }
    }

    // Get sectors from fairs for filter dropdown
    [HttpGet("{lang}/home/fair-sectors")]
    public async Task<JsonResult> GetFairSectors()
    {
        try
        {
            var lang = RouteData.Values["lang"]?.ToString() ?? "tr";
            var useTr = lang == "tr";

            var sectors = _unitOfWork.Sector.GetAll()
                .Select(s => new
                {
                    id = s.Id,
                    name = useTr ? s.NameTr : s.NameEn
                })
                .ToList();

            return Json(new { success = true, data = sectors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching fair sectors");
            return Json(new { success = false, message = "Error loading sectors" });
        }
    }

    // Get cities from fairs for filter dropdown
    [HttpGet("{lang}/home/fair-cities")]
    public async Task<JsonResult> GetFairCities()
    {
        try
        {
            var cities = _unitOfWork.FuarPages.GetAll()
                .Where(f => f.IsPublished && !string.IsNullOrEmpty(f.City))
                .Select(f => f.City)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Json(new { success = true, data = cities });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching fair cities");
            return Json(new { success = false, message = "Error loading cities" });
        }
    }

}
