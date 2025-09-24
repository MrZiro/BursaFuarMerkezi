using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;

        public BlogController(IUnitOfWork unitOfWork, IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            BlogVM pageVM = new BlogVM()
            {
                Blog = new Blog(),
                ContentTypeList = _unitOfWork.ContentType.GetAll().Select(c => new SelectListItem
                {
                    Text = c.NameTr,
                    Value = c.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                // Create new page
                return View(pageVM);
            }
            // Edit existing page
            pageVM.Blog = _unitOfWork.Blog.Get(u => u.Id == id, includeProperties: "ContentType");
            pageVM.ExistingImages = _unitOfWork.BlogImage.GetAll()
                .Where(x => x.BlogId == id)
                .OrderBy(x => x.DisplayOrder);
            if (pageVM.Blog == null)
            {
                return NotFound();
            }
            return View(pageVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BlogVM pageVM)
        {
            pageVM.ContentTypeList = _unitOfWork.ContentType.GetAll().Select(c => new SelectListItem
            {
                Text = c.NameTr,
                Value = c.Id.ToString()
            });

            bool isNewRecord = pageVM.Blog.Id == 0;
            
            // Validation for featured image
            if (isNewRecord && pageVM.CardImage == null)
            {
                ModelState.AddModelError("CardImage", "Please select a card image.");
            } 
            else if (pageVM.CardImage != null)
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
                try 
                {
                    // 1. Handle featured image
                    if (pageVM.CardImage != null)
                    {
                        var savedImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.CardImage, pageVM.Blog.CardImageUrl, "Blogs");
                        if (string.IsNullOrEmpty(savedImageUrl))
                        {
                            ModelState.AddModelError("CardImage", "Failed to upload image. Please try again.");
                            return View(pageVM);
                        }
                        pageVM.Blog.CardImageUrl = savedImageUrl;
                    }

                    // 2. Save/update blog
                    if (isNewRecord)
                    {
                        _unitOfWork.Blog.Add(pageVM.Blog);
                        TempData["success"] = "Blog created successfully.";
                    }
                    else
                    {
                        _unitOfWork.Blog.Update(pageVM.Blog);
                        TempData["success"] = "Blog updated successfully.";
                    }
                    _unitOfWork.Save();

                    // 3. Handle gallery deletions
                    if (pageVM.DeleteImageIds != null && pageVM.DeleteImageIds.Any())
                    {
                        await DeleteGalleryImages(pageVM.DeleteImageIds);
                    }

                    // 4. Handle new gallery images
                    if (pageVM.GalleryImages != null && pageVM.GalleryImages.Any())
                    {
                        await AddGalleryImages(pageVM.Blog.Id, pageVM.GalleryImages);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                    return View(pageVM);
                }
            }
            return View(pageVM);
        }

        // Helper method to delete gallery images
        private async Task DeleteGalleryImages(List<int> imageIds)
        {
            var imagesToDelete = _unitOfWork.BlogImage.GetAll()
                .Where(x => imageIds.Contains(x.Id)).ToList();
                
            foreach (var img in imagesToDelete)
            {
                if (!string.IsNullOrEmpty(img.ImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(img.ImageUrl);
                }
                _unitOfWork.BlogImage.Remove(img);
            }
            _unitOfWork.Save();
        }

        // Helper method to add new gallery images
        private async Task AddGalleryImages(int blogId, List<IFormFile> images)
        {
            int nextOrder = _unitOfWork.BlogImage.GetAll()
                .Where(x => x.BlogId == blogId)
                .Select(x => x.DisplayOrder)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var file in images)
            {
                var imageUrl = await _fileHelper.SaveFileAsync(file, null, "BlogGallery");
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    nextOrder++;
                    _unitOfWork.BlogImage.Add(new BlogImage
                    {
                        BlogId = blogId,
                        ImageUrl = imageUrl,
                        DisplayOrder = nextOrder
                    });
                }
            }
            _unitOfWork.Save();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid ID." });
            }

            Blog pageToDelete = _unitOfWork.Blog.Get(u => u.Id == id);

            if (pageToDelete == null)
            {
                return Json(new { success = false, message = "Error: Page not found." });
            }

            try
            {
                if (!string.IsNullOrEmpty(pageToDelete.CardImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(pageToDelete.CardImageUrl);
                }
                
                // Delete gallery images files
                var gallery = _unitOfWork.BlogImage.GetAll().Where(x => x.BlogId == pageToDelete.Id).ToList();
                foreach (var img in gallery)
                {
                    if (!string.IsNullOrEmpty(img.ImageUrl))
                    {
                        await _fileHelper.DeleteFileAsync(img.ImageUrl);
                    }
                }
                _unitOfWork.BlogImage.RemoveRange(gallery);

                _unitOfWork.Blog.Remove(pageToDelete);
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
            var allObj = _unitOfWork.Blog.GetAll(includeProperties: "ContentType").OrderByDescending(u => u.Id);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var img = _unitOfWork.BlogImage.Get(x => x.Id == id);
            if (img == null)
            {
                return Json(new { success = false, message = "Image not found." });
            }

            if (!string.IsNullOrEmpty(img.ImageUrl))
            {
                await _fileHelper.DeleteFileAsync(img.ImageUrl);
            }
            _unitOfWork.BlogImage.Remove(img);
            _unitOfWork.Save();
            return Json(new { success = true });
        }
    }
}

