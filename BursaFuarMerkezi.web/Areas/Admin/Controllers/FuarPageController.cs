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
        public async Task<IActionResult> Upsert(FuarPageVM pageVM)
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

            if (ModelState.IsValid)
            {
                try
                {
                    // Process featured image if uploaded
                    if (pageVM.FeaturedImage != null)
                    {
                        pageVM.FuarPage.FeaturedImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.FeaturedImage, 
                            pageVM.FuarPage.FeaturedImageUrl);
                    }

                    string successMessage;
                    if (pageVM.FuarPage.Id == 0)
                    {
                        // Create new page
                        _unitOfWork.FuarPage.Add(pageVM.FuarPage);
                        successMessage = "Fuar page created successfully";
                    }
                    else
                    {
                        // Update existing page
                        _unitOfWork.FuarPage.Update(pageVM.FuarPage);
                        successMessage = "Fuar page updated successfully";
                    }

                    _unitOfWork.Save();

                    TempData["success"] = successMessage;

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