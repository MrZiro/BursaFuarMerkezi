using BursaFuarMerkezi.Utility;

namespace BursaFuarMerkezi.web.Services
{
    /// <summary>
    /// Simple SEO helper for generating canonical URLs and alternate language URLs
    /// </summary>
    public static class SeoHelper
    {
        /// <summary>
        /// Route mappings for each action across languages
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, string>> RouteMap = new()
        {
            // Home Controller Routes
            ["Home_Index"] = new() { ["tr"] = "", ["en"] = "" }, // Root for both languages
            
            // Pages Controller Routes
            ["Pages_AboutUs"] = new() { ["tr"] = "hakkimizda", ["en"] = "about-us" },
            ["Pages_Accommodation"] = new() { ["tr"] = "konaklama", ["en"] = "accommodation" },
            ["Pages_Atms"] = new() { ["tr"] = "atm", ["en"] = "atms" },
            ["Pages_ExtraServices"] = new() { ["tr"] = "ekstra-hizmetler", ["en"] = "extra-services" },
            ["Pages_Florist"] = new() { ["tr"] = "cicekci", ["en"] = "florist" },
            ["Pages_FoodPointEn"] = new() { ["tr"] = "yiyecek-noktalari", ["en"] = "food-point" },
            ["Pages_Infirmary"] = new() { ["tr"] = "revir", ["en"] = "infirmary" },
            ["Pages_InformationSocietyServices"] = new() { ["tr"] = "bilgi-toplumu-hizmetleri", ["en"] = "information-society-services" },
            ["Pages_KvkkDocuments"] = new() { ["tr"] = "kvkk-dokumanlari", ["en"] = "kvkk-documents" },
            ["Pages_LostItem"] = new() { ["tr"] = "kayip-esya", ["en"] = "lost-item" },
            ["Pages_MeetingRooms"] = new() { ["tr"] = "toplanti-salonlari", ["en"] = "meeting-rooms" },
            ["Pages_Organizer"] = new() { ["tr"] = "organizator", ["en"] = "organizer" },
            ["Pages_OtherServices"] = new() { ["tr"] = "diger-hizmetler", ["en"] = "other-services" },
            ["Pages_OurTeam"] = new() { ["tr"] = "ekibimiz", ["en"] = "our-team" },
            ["Pages_Parking"] = new() { ["tr"] = "otopark", ["en"] = "parking" },
            ["Pages_Plans"] = new() { ["tr"] = "planlar", ["en"] = "plans" },
            ["Pages_PresidentMessage"] = new() { ["tr"] = "baskan-mesaji", ["en"] = "president-message" },
            ["Pages_QualityPolicy"] = new() { ["tr"] = "kalite-politikamiz", ["en"] = "quality-policy" },
            ["Pages_Reports"] = new() { ["tr"] = "raporlar", ["en"] = "reports" },
            ["Pages_RestaurantAndCafes"] = new() { ["tr"] = "restoran-ve-kafeler", ["en"] = "restaurant-and-cafes" },
            ["Pages_Services"] = new() { ["tr"] = "hizmetler", ["en"] = "services" },
            ["Pages_Transportation"] = new() { ["tr"] = "ulasim", ["en"] = "transportation" },
            
            // Blogs Controller Routes
            ["Blogs_Index"] = new() { ["tr"] = "tum-haberler", ["en"] = "all-news" },
            ["Blogs_Details"] = new() { ["tr"] = "blog-detay", ["en"] = "blog-detail" },
            
            // Fuar Controller Routes
            ["Fuar_FuarDetail"] = new() { ["tr"] = "fuar-detay", ["en"] = "fair-detail" },
            
            // Contact Controller Routes
            ["Contact_Index"] = new() { ["tr"] = "iletisim", ["en"] = "contact" },
            
            // FairCalendar Controller Routes
            ["FairCalendar_FairCalendar"] = new() { ["tr"] = "fuar-takvimi", ["en"] = "fair-calendar" },
            
            // StandReq Controller Routes
            ["StandReq_Index"] = new() { ["tr"] = "stand-talebi", ["en"] = "stand-request" },
            
            // Error Controller Routes
            ["Error_NotFound"] = new() { ["tr"] = "hata-404", ["en"] = "error-404" },
            ["Error_Error"] = new() { ["tr"] = "hata", ["en"] = "error" }
        };

        /// <summary>
        /// Gets the canonical URL for a specific controller action and language
        /// </summary>
        public static string GetCanonicalUrl(string controller, string action, string language)
        {
            var key = $"{controller}_{action}";
            
            if (RouteMap.TryGetValue(key, out var routes) && routes.TryGetValue(language, out var route))
            {
                // Handle home page routes (empty route)
                if (string.IsNullOrEmpty(route))
                {
                    return language == "tr" ? SD.siteUrl : $"{SD.siteUrl}/{language}";
                }
                
                return $"{SD.siteUrl}/{language}/{route}";
            }

            // Fallback to simple route
            return $"{SD.siteUrl}/{language}/{action.ToLower()}";
        }

        /// <summary>
        /// Gets alternate language URLs for language switcher
        /// </summary>
        public static Dictionary<string, string> GetAlternateLanguageUrls(string controller, string action, string currentLanguage)
        {
            var alternateUrls = new Dictionary<string, string>();
            var key = $"{controller}_{action}";

            if (RouteMap.TryGetValue(key, out var routes))
            {
                foreach (var kvp in routes)
                {
                    var lang = kvp.Key;
                    var route = kvp.Value;
                    
                    if (lang != currentLanguage.ToLower())
                    {
                        alternateUrls[lang] = $"{SD.siteUrl}/{lang}/{route}";
                    }
                }
            }

            return alternateUrls;
        }

        /// <summary>
        /// Gets the alternate language URL for a specific slug-based page (like blog details)
        /// </summary>
        public static Dictionary<string, string> GetAlternateLanguageUrlsWithSlug(string controller, string action, string currentLanguage, string currentSlug, string? alternateSlug)
        {
            var alternateUrls = new Dictionary<string, string>();
            var key = $"{controller}_{action}";

            if (RouteMap.TryGetValue(key, out var routes) && !string.IsNullOrEmpty(alternateSlug))
            {
                var targetLang = currentLanguage.ToLower() == "tr" ? "en" : "tr";
                
                if (routes.TryGetValue(targetLang, out var route))
                {
                    alternateUrls[targetLang] = $"{SD.siteUrl}/{targetLang}/{route}/{alternateSlug}";
                }
            }

            return alternateUrls;
        }

        /// <summary>
        /// Gets the language switch URL for the current page
        /// </summary>
        public static string GetLanguageSwitchUrl(string controller, string action, string currentLang, string targetLang, object? routeValues = null)
        {
            var key = $"{controller}_{action}";
            
            if (RouteMap.TryGetValue(key, out var routes) && routes.TryGetValue(targetLang, out var route))
            {
                // Handle home page routes (empty route)
                if (string.IsNullOrEmpty(route))
                {
                    return targetLang == "tr" ? SD.siteUrl : $"{SD.siteUrl}/{targetLang}";
                }
                
                var baseUrl = $"{SD.siteUrl}/{targetLang}/{route}";
                
                // Handle routes with slugs (like blog details)
                if (routeValues != null && routeValues.GetType().GetProperty("slug") != null)
                {
                    var slug = routeValues.GetType().GetProperty("slug")?.GetValue(routeValues)?.ToString();
                    if (!string.IsNullOrEmpty(slug))
                    {
                        baseUrl += $"/{slug}";
                    }
                }
                
                return baseUrl;
            }

            // Fallback
            return $"{SD.siteUrl}/{targetLang}/{action.ToLower()}";
        }
    }
}
