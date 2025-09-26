using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang:regex(^tr|en$)}")]
    public class ErrorController : Controller
    {
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        [HttpGet("hata-404")]
        [HttpGet("error-404")]
        public IActionResult NotFound()
        {
            // Add SEO data
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Error", "NotFound", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Error", "NotFound", Lang);
            
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet("hata")]
        [HttpGet("error")]
        public IActionResult Error()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
