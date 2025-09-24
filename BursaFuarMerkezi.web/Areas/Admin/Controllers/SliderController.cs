using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SliderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;

        public SliderController(IUnitOfWork unitOfWork, IFileHelper fileHelper)
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
            SliderVM sliderVM = new SliderVM()
            {
                Slider = new Slider()
            };

            if (id == null || id == 0)
            {
                // Create new slider
                return View(sliderVM);
            }
            
            // Edit existing slider
            sliderVM.Slider = _unitOfWork.Slider.Get(u => u.Id == id);
            if (sliderVM.Slider == null)
            {
                return NotFound();
            }
            return View(sliderVM);
        }

        [HttpPost]
        public IActionResult Upsert(SliderVM sliderVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderVM);
            }

            try
            {
                // Handle desktop image upload
                if (sliderVM.DesktopImage != null)
                {
                    var desktopImageUrl = _fileHelper.UploadFile(sliderVM.DesktopImage, "sliders");
                    if (!string.IsNullOrEmpty(desktopImageUrl))
                    {
                        // Delete old desktop image if updating
                        if (sliderVM.Slider.Id != 0 && !string.IsNullOrEmpty(sliderVM.Slider.ImageUrl))
                        {
                            _fileHelper.DeleteFile(sliderVM.Slider.ImageUrl);
                        }
                        sliderVM.Slider.ImageUrl = desktopImageUrl;
                    }
                }

                // Handle mobile image upload
                if (sliderVM.MobileImage != null)
                {
                    var mobileImageUrl = _fileHelper.UploadFile(sliderVM.MobileImage, "sliders");
                    if (!string.IsNullOrEmpty(mobileImageUrl))
                    {
                        // Delete old mobile image if updating
                        if (sliderVM.Slider.Id != 0 && !string.IsNullOrEmpty(sliderVM.Slider.MobileImageUrl))
                        {
                            _fileHelper.DeleteFile(sliderVM.Slider.MobileImageUrl);
                        }
                        sliderVM.Slider.MobileImageUrl = mobileImageUrl;
                    }
                }

                if (sliderVM.Slider.Id == 0)
                {
                    // Create new slider
                    sliderVM.Slider.CreatedAt = DateTime.Now;
                    sliderVM.Slider.UpdatedAt = DateTime.Now;
                    _unitOfWork.Slider.Add(sliderVM.Slider);
                    TempData["success"] = "Slider created successfully";
                }
                else
                {
                    // Update existing slider
                    sliderVM.Slider.UpdatedAt = DateTime.Now;
                    _unitOfWork.Slider.Update(sliderVM.Slider);
                    TempData["success"] = "Slider updated successfully";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error saving slider: {ex.Message}";
                return View(sliderVM);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Slider? sliderFromDb = _unitOfWork.Slider.Get(u => u.Id == id);
            if (sliderFromDb == null)
            {
                return NotFound();
            }
            return View(sliderFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Slider? obj = _unitOfWork.Slider.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            try
            {
                // Delete associated images
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    _fileHelper.DeleteFile(obj.ImageUrl);
                }
                if (!string.IsNullOrEmpty(obj.MobileImageUrl))
                {
                    _fileHelper.DeleteFile(obj.MobileImageUrl);
                }

                _unitOfWork.Slider.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Slider deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error deleting slider: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Slider> objSliderList = _unitOfWork.Slider.GetAll().OrderBy(s => s.DisplayOrder).ToList();
            return Json(new { data = objSliderList });
        }

        [HttpPost]
        public IActionResult ToggleActive(int id)
        {
            var slider = _unitOfWork.Slider.Get(u => u.Id == id);
            if (slider == null)
            {
                return Json(new { success = false, message = "Slider not found" });
            }

            slider.IsActive = !slider.IsActive;
            slider.UpdatedAt = DateTime.Now;
            _unitOfWork.Slider.Update(slider);
            _unitOfWork.Save();

            return Json(new { 
                success = true, 
                message = $"Slider {(slider.IsActive ? "activated" : "deactivated")} successfully",
                isActive = slider.IsActive 
            });
        }

        #endregion
    }
}