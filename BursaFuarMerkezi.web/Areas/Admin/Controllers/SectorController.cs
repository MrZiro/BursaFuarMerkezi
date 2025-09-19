using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SectorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SectorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var sectors = _unitOfWork.Sector.GetAll().ToList();
            return View(sectors);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Sector());
            }
            var sector = _unitOfWork.Sector.Get(u => u.Id == id);
            if (sector == null) return NotFound();
            return View(sector);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Sector sector)
        {
            if (!ModelState.IsValid)
            {
                return View(sector);
            }
            if (sector.Id == 0)
            {
                _unitOfWork.Sector.Add(sector);
                TempData["success"] = "Sector created successfully";
            }
            else
            {
                _unitOfWork.Sector.Update(sector);
                TempData["success"] = "Sector updated successfully";
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var sectors = _unitOfWork.Sector.GetAll().ToList();
            return Json(new { data = sectors });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }
            var sector = _unitOfWork.Sector.Get(u => u.Id == id);
            if (sector == null)
            {
                return Json(new { success = false, message = "Error: Sector not found" });
            }
            try
            {
                _unitOfWork.Sector.Remove(sector);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Sector deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the sector: " + ex.Message });
            }
        }
    }
}


