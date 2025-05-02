using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Utility
{
    public static class HelperExtensions
    {
        public static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Get the inner text first (removes tags)
            string plainText = doc.DocumentNode.InnerText;

            // --- Add this line to decode HTML entities ---
            // This converts &nbsp; to a space, &lt; to <, etc.
            plainText = WebUtility.HtmlDecode(plainText);
            // ------------------------------------------

            // Optional: Add trimming or other text cleaning here
            // plainText = plainText.Trim();

            return plainText;
        }



    }
}
