using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ContentTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContentTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<ContentType> contentTypeList = _unitOfWork.ContentType.GetAll().ToList();
            return View(contentTypeList);
        }

        // GET
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                // Create new content type
                return View(new ContentType());
            }
            
            // Edit existing content type
            ContentType contentType = _unitOfWork.ContentType.Get(u => u.Id == id);
            if (contentType == null)
            {
                return NotFound();
            }
            
            return View(contentType);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ContentType contentType)
        {
            if (ModelState.IsValid)
            {
                if (contentType.Id == 0)
                {
                    _unitOfWork.ContentType.Add(contentType);
                    TempData["success"] = "Content type created successfully";
                }
                else
                {
                    _unitOfWork.ContentType.Update(contentType);
                    TempData["success"] = "Content type updated successfully";
                }
                
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            
            return View(contentType);
        }

        // API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ContentType> contentTypeList = _unitOfWork.ContentType.GetAll().ToList();
            return Json(new { data = contentTypeList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }

            ContentType contentTypeToDelete = _unitOfWork.ContentType.Get(u => u.Id == id);
            
            if (contentTypeToDelete == null)
            {
                return Json(new { success = false, message = "Error: Content type not found" });
            }

            try
            {
                _unitOfWork.ContentType.Remove(contentTypeToDelete);
                _unitOfWork.Save();
                
                return Json(new { success = true, message = "Content type deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the content type: " + ex.Message });
            }
        }
    }
} 