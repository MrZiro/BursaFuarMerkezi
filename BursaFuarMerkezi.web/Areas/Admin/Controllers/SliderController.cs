using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var sliders = _unitOfWork.Slider.GetAll().OrderBy(s => s.DisplayOrder).ToList();
            return View(sliders);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new SliderVM { Slider = new Slider() });
            }
            var slider = _unitOfWork.Slider.Get(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(new SliderVM { Slider = slider });
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(SliderVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.Image != null)
            {
                vm.Slider.ImageUrl = await _fileHelper.SaveFileAsync(vm.Image, vm.Slider.ImageUrl, "Slider");
                if (vm.Slider.ImageUrl == null)
                {
                    ModelState.AddModelError("", "Failed to upload image.");
                    return View(vm);
                }
            }

            if (vm.Slider.Id == 0)
            {
                _unitOfWork.Slider.Add(vm.Slider);
                TempData["success"] = "Slider created successfully.";
            }
            else
            {
                _unitOfWork.Slider.Update(vm.Slider);
                TempData["success"] = "Slider updated successfully.";
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return Json(new { success = false, message = "Invalid ID" });
            var slider = _unitOfWork.Slider.Get(s => s.Id == id);
            if (slider == null) return Json(new { success = false, message = "Slider not found" });
            try
            {
                if (!string.IsNullOrEmpty(slider.ImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(slider.ImageUrl);
                }
                _unitOfWork.Slider.Remove(slider);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Slider deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting slider: " + ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var sliders = _unitOfWork.Slider.GetAll().ToList();
            return Json(new { data = sliders });
        }
    }
}


