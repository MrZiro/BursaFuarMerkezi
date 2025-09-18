using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Utility
{
    public static class SlugUtility
    {
        public static string GenerateSlug(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var normalized = text.ToLowerInvariant();

            // Replace Turkish characters
            normalized = normalized
                .Replace("ı", "i").Replace("İ", "i")
                .Replace("ö", "o").Replace("Ö", "o")
                .Replace("ş", "s").Replace("Ş", "s")
                .Replace("ğ", "g").Replace("Ğ", "g")
                .Replace("ü", "u").Replace("Ü", "u")
                .Replace("ç", "c").Replace("Ç", "c");

            // Remove diacritics
            var formD = normalized.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var ch in formD)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            var cleaned = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);

            // Remove invalid chars
            var arr = cleaned.Select(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-' ? c : '-').ToArray();
            var interim = new string(arr);

            // Collapse spaces and dashes to single dash
            var collapsed = System.Text.RegularExpressions.Regex.Replace(interim, @"[\n\r\t_]+", " ");
            collapsed = System.Text.RegularExpressions.Regex.Replace(collapsed, @"\s+", "-");
            collapsed = System.Text.RegularExpressions.Regex.Replace(collapsed, "-+", "-");

            // Trim dashes
            return collapsed.Trim('-');
        }
    }
}
