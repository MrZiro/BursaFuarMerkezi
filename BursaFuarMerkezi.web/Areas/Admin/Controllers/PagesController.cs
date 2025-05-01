using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
