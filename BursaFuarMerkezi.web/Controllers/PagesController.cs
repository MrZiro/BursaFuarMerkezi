using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Controllers
{
    [Route("{lang}")]
    public class PagesController : Controller
    {
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public PagesController()
        {
        }

        [HttpGet("about-us")]
        [HttpGet("hakkimizda")]
        public IActionResult AboutUs()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "AboutUs", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "AboutUs", Lang);
            return View();
        }

        [HttpGet("accommodation")]
        [HttpGet("konaklama")]
        public IActionResult Accommodation()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Accommodation", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Accommodation", Lang);
            return View();
        }

        [HttpGet("atms")]
        [HttpGet("atm")]
        public IActionResult Atms()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Atms", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Atms", Lang);
            return View();
        }

        [HttpGet("extra-services")]
        [HttpGet("ekstra-hizmetler")]
        public IActionResult ExtraServices()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "ExtraServices", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "ExtraServices", Lang);
            return View();
        }


        [HttpGet("florist")]
        [HttpGet("cicekci")]
        public IActionResult Florist()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Florist", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Florist", Lang);
            return View();
        }

        [HttpGet("food-point")]
        [HttpGet("yiyecek-noktalari")]
        public IActionResult FoodPointEn()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "FoodPointEn", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "FoodPointEn", Lang);
            return View();
        }

        [HttpGet("infirmary")]
        [HttpGet("revir")]
        public IActionResult Infirmary()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Infirmary", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Infirmary", Lang);
            return View();
        }

        [HttpGet("information-society-services")]
        [HttpGet("bilgi-toplumu-hizmetleri")]
        public IActionResult InformationSocietyServices()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "InformationSocietyServices", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "InformationSocietyServices", Lang);
            return View();
        }

        [HttpGet("kvkk-documents")]
        [HttpGet("kvkk-dokumanlari")]
        public IActionResult KvkkDocuments()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "KvkkDocuments", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "KvkkDocuments", Lang);
            return View();
        }

        [HttpGet("lost-item")]
        [HttpGet("kayip-esya")]
        public IActionResult LostItem()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "LostItem", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "LostItem", Lang);
            return View();
        }

        [HttpGet("meeting-rooms")]
        [HttpGet("toplanti-salonlari")]
        public IActionResult MeetingRooms()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "MeetingRooms", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "MeetingRooms", Lang);
            return View();
        }

        [HttpGet("organizer")]
        [HttpGet("organizator")]
        public IActionResult Organizer()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Organizer", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Organizer", Lang);
            return View();
        }

        [HttpGet("other-services")]
        [HttpGet("diger-hizmetler")]
        public IActionResult OtherServices()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "OtherServices", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "OtherServices", Lang);
            return View();
        }

        [HttpGet("our-team")]
        [HttpGet("ekibimiz")]
        public IActionResult OurTeam()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "OurTeam", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "OurTeam", Lang);
            return View();
        }

        [HttpGet("parking")]
        [HttpGet("otopark")]
        public IActionResult Parking()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Parking", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Parking", Lang);
            return View();
        }

        [HttpGet("plans")]
        [HttpGet("planlar")]
        public IActionResult Plans()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Plans", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Plans", Lang);
            return View();
        }

        [HttpGet("president-message")]
        [HttpGet("baskan-mesaji")]
        public IActionResult PresidentMessage()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "PresidentMessage", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "PresidentMessage", Lang);
            return View();
        }

        [HttpGet("quality-policy")]
        [HttpGet("kalite-politikamiz")]
        public IActionResult QualityPolicy()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "QualityPolicy", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "QualityPolicy", Lang);
            return View();
        }

        [HttpGet("reports")]
        [HttpGet("raporlar")]
        public IActionResult Reports()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Reports", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Reports", Lang);
            return View();
        }

        [HttpGet("restaurant-and-cafes")]
        [HttpGet("restoran-ve-kafeler")]
        public IActionResult RestaurantAndCafes()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "RestaurantAndCafes", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "RestaurantAndCafes", Lang);
            return View();
        }

        [HttpGet("services")]
        [HttpGet("hizmetler")]
        public IActionResult Services()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Services", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Services", Lang);
            return View();
        }


        [HttpGet("transportation")]
        [HttpGet("ulasim")]
        public IActionResult Transportation()
        {
            ViewData["CanonicalUrl"] = SeoHelper.GetCanonicalUrl("Pages", "Transportation", Lang);
            ViewData["AlternateUrls"] = SeoHelper.GetAlternateLanguageUrls("Pages", "Transportation", Lang);
            return View();
        }
    }
}
