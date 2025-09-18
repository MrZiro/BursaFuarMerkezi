using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // Using shared SlugUtility

        public IActionResult Index()
        {
            return View();
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
                // Edit existing page
            pageVM.FuarPage = _unitOfWork.FuarPages.Get(u => u.Id == id);
            if (pageVM.FuarPage == null)
            {
                return NotFound();
            }
            return View(pageVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(FuarPageVM pageVM)
        {
            bool isNewRecord = pageVM.FuarPage.Id == 0;

            // Only require featured image for new records
            if (isNewRecord && pageVM.FeaturedImage == null)
            {
                ModelState.AddModelError("FeaturedImage", "Please select an image.");
            }
            else if (pageVM.FeaturedImage != null)
            {
                // Validate image if provided
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

            if (isNewRecord && pageVM.CardImage == null)
            {
                ModelState.AddModelError("CardImage", "Please select a card image.");
            } else if (pageVM.CardImage != null)
            {
                // Check file size (5MB max)
                if (pageVM.CardImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("CardImage", "Card image size cannot exceed 5MB.");
                }
                // Check file format
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(pageVM.CardImage.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CardImage", "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                try {
                    // Generate TR/EN slugs if empty
                    if (string.IsNullOrWhiteSpace(pageVM.FuarPage.SlugTr) && !string.IsNullOrWhiteSpace(pageVM.FuarPage.TitleTr))
                    {
                        pageVM.FuarPage.SlugTr = SlugUtility.GenerateSlug(pageVM.FuarPage.TitleTr);
                    }
                    if (string.IsNullOrWhiteSpace(pageVM.FuarPage.SlugEn) && !string.IsNullOrWhiteSpace(pageVM.FuarPage.TitleEn))
                    {
                        pageVM.FuarPage.SlugEn = SlugUtility.GenerateSlug(pageVM.FuarPage.TitleEn);
                    }

                    // Validate slug uniqueness (TR/EN)
                    if (!string.IsNullOrWhiteSpace(pageVM.FuarPage.SlugTr))
                    {
                        var uniqueTr = await _unitOfWork.FuarPages.IsSlugUniqueAsync(pageVM.FuarPage.SlugTr, "tr", isNewRecord ? null : pageVM.FuarPage.Id);
                        if (!uniqueTr)
                        {
                            ModelState.AddModelError("FuarPage.SlugTr", "TR slug already exists.");
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(pageVM.FuarPage.SlugEn))
                    {
                        var uniqueEn = await _unitOfWork.FuarPages.IsSlugUniqueAsync(pageVM.FuarPage.SlugEn, "en", isNewRecord ? null : pageVM.FuarPage.Id);
                        if (!uniqueEn)
                        {
                            ModelState.AddModelError("FuarPage.SlugEn", "EN slug already exists.");
                        }
                    }
                    if (!ModelState.IsValid)
                    {
                        return View(pageVM);
                    }
                    // Only process the featured image if a new one is provided
                    if (pageVM.FeaturedImage != null)
                    {
                        string oldImageUrl = pageVM.FuarPage.FeaturedImageUrl;
                        pageVM.FuarPage.FeaturedImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.FeaturedImage, pageVM.FuarPage.FeaturedImageUrl, "FuarPage");
                        if (pageVM.FuarPage.FeaturedImageUrl == null)
                        {
                            // If image saving failed but validation passed, something went wrong
                            ModelState.AddModelError("FeaturedImage", "Failed to upload image. Please try again.");
                            return View(pageVM);
                        }
                    }
                    else if (isNewRecord)
                    {
                        // This is a safety check - shouldn't happen due to validation above
                        ModelState.AddModelError("FeaturedImage", "Please select an image.");
                        return View(pageVM);
                    }

                    // Process card image if provided
                    if (pageVM.CardImage != null)
                    {
                        string oldCardImageUrl = pageVM.FuarPage.CardImageUrl;
                        pageVM.FuarPage.CardImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.CardImage, pageVM.FuarPage.CardImageUrl, "FuarPageCard");
                        if (pageVM.FuarPage.CardImageUrl == null)
                        {
                            ModelState.AddModelError("CardImage", "Failed to upload card image. Please try again.");
                            return View(pageVM);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                    return View(pageVM);
                }

                if (isNewRecord)
                {
                    _unitOfWork.FuarPages.Add(pageVM.FuarPage);
                    TempData["success"] = "Page created successfully.";
                }
                else
                {
                    _unitOfWork.FuarPages.Update(pageVM.FuarPage);
                    TempData["success"] = "Page updated successfully.";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(pageVM);
        }



         [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid ID." });
            }

            FuarPage pageToDelete = _unitOfWork.FuarPages.Get(u => u.Id == id);

            if (pageToDelete == null)
            {
                return Json(new { success = false, message = "Error: Page not found." });
            }

            try
            {
                if (!string.IsNullOrEmpty(pageToDelete.FeaturedImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(pageToDelete.FeaturedImageUrl);
                }
                
                if (!string.IsNullOrEmpty(pageToDelete.CardImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(pageToDelete.CardImageUrl);
                }
                
                _unitOfWork.FuarPages.Remove(pageToDelete);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Page deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the page: " + ex.Message });
            }
        }


        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.FuarPages.GetAll().OrderByDescending(u => u.Id);
            return Json(new { data = allObj });
        }
    }
}

