using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.Utility
{
    public static class SD
    {
        public const string AllowedImageExtensions = ".jpg,.jpeg,.png,.gif,.webp";
        public const int MaxImageSizeInBytes = 5 * 1024 * 1024; // 5MB
        public const int MaxImageSizeInMB = 5;

        public const string Role_Admin = "Admin";
        public const string Role_Editor = "Editor";

        // Constants for TempFileCleanupService
        public const int TempFileCleanupIntervalMinutes = 1440; // Run cleanup every day
        public const int TempFileMaxAgeMinutes = 120;         // Delete files older than 2 hours

        public const string siteUrl = "http://localhost:5106";
        public const string AdminEmail = "test@test.com";
    }
}
