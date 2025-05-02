using Microsoft.AspNetCore.Http; // Required for IFormFile

namespace BursaFuarMerkezi.web.Services
{
    public interface IFileHelper
    {
        // Add methods required for file handling
        // You might adjust return types (e.g., return a Result object with success/error messages)
        // depending on how you want to handle errors in the controller.
        // This example uses async as the file operations are async.

        // Saves a file, deletes the old one if specified, returns the new URL relative to wwwroot
        Task<string?> SaveFileAsync(IFormFile file, string? existingImageUrl);

        // Deletes a file based on its URL relative to wwwroot
        Task DeleteFileAsync(string imageUrl);

        // Optional: Add validation method if not doing it in controller
        // bool IsValidFile(IFormFile file);
    }
}