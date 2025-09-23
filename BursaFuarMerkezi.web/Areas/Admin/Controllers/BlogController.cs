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
                    if (pageVM.CardImage != null)
                    {
                        string oldImageUrl = pageVM.Blog.CardImageUrl;
                        pageVM.Blog.CardImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.CardImage, pageVM.Blog.CardImageUrl, "Blogs");
                        if (pageVM.Blog.CardImageUrl == null)
                        {
                            // If image saving failed but validation passed, something went wrong
                            ModelState.AddModelError("CardImage", "Failed to upload image. Please try again.");
                            return View(pageVM);
                        }
                    }
                    else if (isNewRecord)
                    {
                        // This is a safety check - shouldn't happen due to validation above
                        ModelState.AddModelError("CardImage", "Please select an image.");
                        return View(pageVM);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                    return View(pageVM);
                }

                if (isNewRecord)
                {
                    _unitOfWork.Blog.Add(pageVM.Blog);
                    TempData["success"] = "Page created successfully.";
                }
                else
                {
                    _unitOfWork.Blog.Update(pageVM.Blog);
                    TempData["success"] = "Page updated successfully.";
                }
                _unitOfWork.Save();

                // Save gallery images after we have Blog.Id
                if (pageVM.GalleryImages != null && pageVM.GalleryImages.Count > 0)
                {
                    int nextOrder = _unitOfWork.BlogImage.GetAll()
                        .Where(x => x.BlogId == pageVM.Blog.Id)
                        .Select(x => x.DisplayOrder)
                        .DefaultIfEmpty(0)
                        .Max();

                    foreach (var file in pageVM.GalleryImages)
                    {
                        var imageUrl = await _fileHelper.SaveFileAsync(file, null, "BlogGallery");
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            nextOrder++;
                            _unitOfWork.BlogImage.Add(new BlogImage
                            {
                                BlogId = pageVM.Blog.Id,
                                ImageUrl = imageUrl,
                                DisplayOrder = nextOrder
                            });
                        }
                    }
                    _unitOfWork.Save();
                }
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

