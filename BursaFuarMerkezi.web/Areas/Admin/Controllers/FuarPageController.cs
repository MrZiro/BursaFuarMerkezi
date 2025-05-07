using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class FuarPageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;

        public FuarPageController(IUnitOfWork unitOfWork, IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            List<FuarPage> pages = _unitOfWork.FuarPage.GetAll().ToList();
            return View(pages);
        }

        public IActionResult Upsert(int? id)
        {
            FuarPageVM pageVM = new FuarPageVM()
            {
                FuarPage = new FuarPage()
            };

            if (id == null || id == 0)
            {
                // Create new page
                return View(pageVM);
            }
            else
            {
                // Edit existing page
                pageVM.FuarPage = _unitOfWork.FuarPage.Get(u => u.Id == id);

                if (pageVM.FuarPage == null)
                {
                    return NotFound();
                }
            }

            return View(pageVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(FuarPageVM pageVM, bool removeImage = false)
        {
            // Generate slug if empty
            if (string.IsNullOrEmpty(pageVM.FuarPage.Slug))
            {
                pageVM.FuarPage.Slug = GenerateSlug(pageVM.FuarPage.Title);
            }
            else
            {
                pageVM.FuarPage.Slug = GenerateSlug(pageVM.FuarPage.Slug);
            }

            // Check for unique slug
            bool isSlugUnique = await _unitOfWork.FuarPage.IsSlugUniqueAsync(
                pageVM.FuarPage.Slug, 
                pageVM.FuarPage.Id == 0 ? null : pageVM.FuarPage.Id);

            if (!isSlugUnique)
            {
                ModelState.AddModelError("FuarPage.Slug", "This URL is already taken. Please modify it.");
            }

            // Validate image if provided
            if (pageVM.FeaturedImage != null)
            {
                // Check file size (5MB max)
                if (pageVM.FeaturedImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("FeaturedImage", "Image size cannot exceed 5MB.");
                }

                // Check file format
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(pageVM.FeaturedImage.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("FeaturedImage", "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image operations
                    string oldImageUrl = pageVM.FuarPage.FeaturedImageUrl;
                    
                    // Process featured image if uploaded
                    if (pageVM.FeaturedImage != null)
                    {
                        pageVM.FuarPage.FeaturedImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.FeaturedImage, 
                            pageVM.FuarPage.FeaturedImageUrl);
                        
                        if (pageVM.FuarPage.FeaturedImageUrl == null)
                        {
                            // If image saving failed but validation passed, something went wrong
                            ModelState.AddModelError("FeaturedImage", "Failed to upload image. Please try again.");
                            return View(pageVM);
                        }
                    }
                    // Remove image if requested (and no new one uploaded)
                    else if (removeImage && !string.IsNullOrEmpty(oldImageUrl))
                    {
                        await _fileHelper.DeleteFileAsync(oldImageUrl);
                        pageVM.FuarPage.FeaturedImageUrl = null;
                    }

                    // Handle update vs insert
                    if (pageVM.FuarPage.Id == 0)
                    {
                        // Creating new record
                        pageVM.FuarPage.CreatedAt = DateTime.Now;
                        _unitOfWork.FuarPage.Add(pageVM.FuarPage);
                        TempData["success"] = "Fuar page created successfully";
                    }
                    else
                    {
                        // Updating existing record
                        var existingPage = _unitOfWork.FuarPage.Get(u => u.Id == pageVM.FuarPage.Id);
                        if (existingPage == null)
                        {
                            return NotFound();
                        }

                        // Update main properties, preserving CreatedAt
                        existingPage.Title = pageVM.FuarPage.Title;
                        existingPage.Slug = pageVM.FuarPage.Slug;
                        existingPage.Content = pageVM.FuarPage.Content;
                        existingPage.IsPublished = pageVM.FuarPage.IsPublished;
                        existingPage.MetaDescription = pageVM.FuarPage.MetaDescription;
                        existingPage.MetaKeywords = pageVM.FuarPage.MetaKeywords;
                        existingPage.PageType = pageVM.FuarPage.PageType;
                        existingPage.UpdatedAt = DateTime.Now;
                        
                        // Update FeaturedImageUrl if changed
                        existingPage.FeaturedImageUrl = pageVM.FuarPage.FeaturedImageUrl;

                        // Update fair-specific fields
                        existingPage.StartDate = pageVM.FuarPage.StartDate;
                        existingPage.EndDate = pageVM.FuarPage.EndDate;
                        existingPage.FairHall = pageVM.FuarPage.FairHall;
                        existingPage.Organizer = pageVM.FuarPage.Organizer;
                        existingPage.VisitingHours = pageVM.FuarPage.VisitingHours;
                        existingPage.FairLocation = pageVM.FuarPage.FairLocation;
                        existingPage.WebsiteUrl = pageVM.FuarPage.WebsiteUrl;

                        _unitOfWork.FuarPage.Update(existingPage);
                        TempData["success"] = "Fuar page updated successfully";
                    }

                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving page: " + ex.Message);
                }
            }

            return View(pageVM);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid ID." });
            }

            FuarPage pageToDelete = _unitOfWork.FuarPage.Get(u => u.Id == id);

            if (pageToDelete == null)
            {
                return Json(new { success = false, message = "Error: Page not found." });
            }

            try
            {
                // Delete featured image if exists
                if (!string.IsNullOrEmpty(pageToDelete.FeaturedImageUrl))
                {
                    _fileHelper.DeleteFileAsync(pageToDelete.FeaturedImageUrl).Wait();
                }

                _unitOfWork.FuarPage.Remove(pageToDelete);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Page deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the page: " + ex.Message });
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var pages = _unitOfWork.FuarPage.GetAll()
                .Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    slug = p.Slug,
                    isPublished = p.IsPublished ? "Published" : "Draft",
                    createdAt = p.CreatedAt,
                    pageType = p.PageType
                });

            return Json(new { data = pages });
        }

        #endregion

        // Helper method to convert title to URL-friendly slug
        private static string GenerateSlug(string title)
        {
            // Convert to lowercase and remove accents
            string slug = title.ToLowerInvariant();
            
            // Remove invalid characters
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            
            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");
            
            // Remove multiple hyphens
            slug = Regex.Replace(slug, @"-+", "-");
            
            // Trim hyphens from beginning and end
            slug = slug.Trim('-');
            
            return slug;
        }
    }
} 