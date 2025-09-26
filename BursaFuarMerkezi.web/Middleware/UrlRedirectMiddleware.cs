using BursaFuarMerkezi.web.Services;

namespace BursaFuarMerkezi.web.Middleware
{
    /// <summary>
    /// Middleware to redirect incorrect URLs to their proper localized versions
    /// </summary>
    public class UrlRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UrlRedirectMiddleware> _logger;

        public UrlRedirectMiddleware(RequestDelegate next, ILogger<UrlRedirectMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            
            if (!string.IsNullOrEmpty(path) && ShouldRedirect(path, out var correctUrl))
            {
                _logger.LogInformation($"Redirecting from {path} to {correctUrl}");
                context.Response.Redirect(correctUrl, permanent: true);
                return;
            }

            await _next(context);
        }

        private bool ShouldRedirect(string path, out string correctUrl)
        {
            correctUrl = string.Empty;

            // Extract language and route from path
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            if (segments.Length < 2)
                return false;

            var lang = segments[0];
            var route = segments[1];

            // Only handle tr/en languages
            if (lang != "tr" && lang != "en")
                return false;

            // Check if the route is using the wrong language version
            var correctRoute = GetCorrectRoute(route, lang);
            
            if (!string.IsNullOrEmpty(correctRoute) && correctRoute != route)
            {
                // Rebuild the URL with correct route
                var pathSegments = new List<string> { lang, correctRoute };
                
                // Add any additional segments (like slugs)
                if (segments.Length > 2)
                {
                    pathSegments.AddRange(segments.Skip(2));
                }
                
                correctUrl = "/" + string.Join("/", pathSegments);
                return true;
            }

            return false;
        }

        private string? GetCorrectRoute(string route, string lang)
        {
            // Map of incorrect routes to correct ones
            var routeCorrections = new Dictionary<string, Dictionary<string, string>>
            {
                // Pages routes that might be accessed with wrong language
                ["about-us"] = new() { ["tr"] = "hakkimizda", ["en"] = "about-us" },
                ["hakkimizda"] = new() { ["tr"] = "hakkimizda", ["en"] = "about-us" },
                
                ["accommodation"] = new() { ["tr"] = "konaklama", ["en"] = "accommodation" },
                ["konaklama"] = new() { ["tr"] = "konaklama", ["en"] = "accommodation" },
                
                ["contact"] = new() { ["tr"] = "iletisim", ["en"] = "contact" },
                ["iletisim"] = new() { ["tr"] = "iletisim", ["en"] = "contact" },
                
                ["fair-calendar"] = new() { ["tr"] = "fuar-takvimi", ["en"] = "fair-calendar" },
                ["fuar-takvimi"] = new() { ["tr"] = "fuar-takvimi", ["en"] = "fair-calendar" },
                
                ["all-news"] = new() { ["tr"] = "tum-haberler", ["en"] = "all-news" },
                ["tum-haberler"] = new() { ["tr"] = "tum-haberler", ["en"] = "all-news" },
                
                ["blog-detail"] = new() { ["tr"] = "blog-detay", ["en"] = "blog-detail" },
                ["blog-detay"] = new() { ["tr"] = "blog-detay", ["en"] = "blog-detail" },
                
                ["fair-detail"] = new() { ["tr"] = "fuar-detay", ["en"] = "fair-detail" },
                ["fuar-detay"] = new() { ["tr"] = "fuar-detay", ["en"] = "fair-detail" },
                
                ["stand-request"] = new() { ["tr"] = "stand-talebi", ["en"] = "stand-request" },
                ["stand-talebi"] = new() { ["tr"] = "stand-talebi", ["en"] = "stand-request" },
                
                ["services"] = new() { ["tr"] = "hizmetler", ["en"] = "services" },
                ["hizmetler"] = new() { ["tr"] = "hizmetler", ["en"] = "services" },
                
                ["transportation"] = new() { ["tr"] = "ulasim", ["en"] = "transportation" },
                ["ulasim"] = new() { ["tr"] = "ulasim", ["en"] = "transportation" },
                
                ["restaurant-and-cafes"] = new() { ["tr"] = "restoran-ve-kafeler", ["en"] = "restaurant-and-cafes" },
                ["restoran-ve-kafeler"] = new() { ["tr"] = "restoran-ve-kafeler", ["en"] = "restaurant-and-cafes" },
                
                ["meeting-rooms"] = new() { ["tr"] = "toplanti-odalari", ["en"] = "meeting-rooms" },
                ["toplanti-odalari"] = new() { ["tr"] = "toplanti-odalari", ["en"] = "meeting-rooms" },
                
                ["parking"] = new() { ["tr"] = "otopark", ["en"] = "parking" },
                ["otopark"] = new() { ["tr"] = "otopark", ["en"] = "parking" },
                
                ["plans"] = new() { ["tr"] = "planlar", ["en"] = "plans" },
                ["planlar"] = new() { ["tr"] = "planlar", ["en"] = "plans" }
            };

            if (routeCorrections.TryGetValue(route, out var languageRoutes))
            {
                return languageRoutes.TryGetValue(lang, out var correctRoute) ? correctRoute : null;
            }

            return null;
        }
    }
}
