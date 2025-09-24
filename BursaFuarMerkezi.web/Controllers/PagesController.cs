using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang}")]
    public class PagesController : Controller
    {
        private readonly IUrlLocalizationService _urlService;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public PagesController(IUrlLocalizationService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("about-us")]
        [HttpGet("hakkimizda")]
        public IActionResult AboutUs()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "AboutUs", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "AboutUs", Lang);
            return View();
        }

        [HttpGet("accommodation")]
        [HttpGet("konaklama")]
        public IActionResult Accommodation()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Accommodation", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Accommodation", Lang);
            return View();
        }

        [HttpGet("atms")]
        [HttpGet("atm-ler")]
        public IActionResult Atms()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Atms", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Atms", Lang);
            return View();
        }

        [HttpGet("extra-services")]
        [HttpGet("ekstra-hizmetler")]
        public IActionResult ExtraServices()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "ExtraServices", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "ExtraServices", Lang);
            return View();
        }

        [HttpGet("fair-calendar")]
        [HttpGet("fuar-takvimi")]
        public IActionResult FairCalendar()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "FairCalendar", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "FairCalendar", Lang);
            return View();
        }

        [HttpGet("florist")]
        [HttpGet("cicekci")]
        public IActionResult Florist()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Florist", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Florist", Lang);
            return View();
        }

        [HttpGet("food-point")]
        [HttpGet("yiyecek-noktalari")]
        public IActionResult FoodPointEn()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "FoodPointEn", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "FoodPointEn", Lang);
            return View();
        }

        [HttpGet("infirmary")]
        [HttpGet("revir")]
        public IActionResult Infirmary()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Infirmary", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Infirmary", Lang);
            return View();
        }

        [HttpGet("information-society-services")]
        [HttpGet("bilgi-toplumu-hizmetleri")]
        public IActionResult InformationSocietyServices()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "InformationSocietyServices", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "InformationSocietyServices", Lang);
            return View();
        }

        [HttpGet("kvkk-documents")]
        [HttpGet("kvkk-belgeleri")]
        public IActionResult KvkkDocuments()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "KvkkDocuments", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "KvkkDocuments", Lang);
            return View();
        }

        [HttpGet("lost-item")]
        [HttpGet("kayip-esya")]
        public IActionResult LostItem()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "LostItem", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "LostItem", Lang);
            return View();
        }

        [HttpGet("meeting-rooms")]
        [HttpGet("toplanti-odalari")]
        public IActionResult MeetingRooms()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "MeetingRooms", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "MeetingRooms", Lang);
            return View();
        }

        [HttpGet("organizer")]
        [HttpGet("organizator")]
        public IActionResult Organizer()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Organizer", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Organizer", Lang);
            return View();
        }

        [HttpGet("other-services")]
        [HttpGet("diger-hizmetler")]
        public IActionResult OtherServices()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "OtherServices", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "OtherServices", Lang);
            return View();
        }

        [HttpGet("our-team")]
        [HttpGet("ekibimiz")]
        public IActionResult OurTeam()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "OurTeam", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "OurTeam", Lang);
            return View();
        }

        [HttpGet("parking")]
        [HttpGet("otopark")]
        public IActionResult Parking()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Parking", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Parking", Lang);
            return View();
        }

        [HttpGet("plans")]
        [HttpGet("planlar")]
        public IActionResult Plans()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Plans", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Plans", Lang);
            return View();
        }

        [HttpGet("president-message")]
        [HttpGet("baskan-mesaji")]
        public IActionResult PresidentMessage()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "PresidentMessage", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "PresidentMessage", Lang);
            return View();
        }

        [HttpGet("quality-policy")]
        [HttpGet("kalite-politikasi")]
        public IActionResult QualityPolicy()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "QualityPolicy", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "QualityPolicy", Lang);
            return View();
        }

        [HttpGet("reports")]
        [HttpGet("raporlar")]
        public IActionResult Reports()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Reports", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Reports", Lang);
            return View();
        }

        [HttpGet("restaurant-and-cafes")]
        [HttpGet("restoran-ve-kafeler")]
        public IActionResult RestaurantAndCafes()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "RestaurantAndCafes", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "RestaurantAndCafes", Lang);
            return View();
        }

        [HttpGet("services")]
        [HttpGet("hizmetler")]
        public IActionResult Services()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Services", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Services", Lang);
            return View();
        }


        [HttpGet("transportation")]
        [HttpGet("ulasim")]
        public IActionResult Transportation()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Pages", "Transportation", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Pages", "Transportation", Lang);
            return View();
        }
    }
}
