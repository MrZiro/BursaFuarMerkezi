using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
