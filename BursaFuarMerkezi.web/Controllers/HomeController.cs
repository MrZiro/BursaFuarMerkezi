using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BursaFuarMerkezi.Models;
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

    public IActionResult Index()
    {
        // get all sliders
        // get the latest 9 blogs
        // get all events
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // get blogs
    [HttpGet("{lang}/blogs")]
    public async Task<JsonResult> GetBlogs()
    {
        var lang = RouteData.Values["lang"]?.ToString() ?? "tr";
        var blogs = await _unitOfWork.Blog.GetLatestBlogsWithFieldsAsync(9, lang);
        return Json(new { blogs = blogs });
    }

    // get Durular

    // get Haberler

}
