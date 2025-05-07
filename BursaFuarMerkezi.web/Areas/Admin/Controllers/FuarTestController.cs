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
    public class FuarTestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;

        public FuarTestController(IUnitOfWork unitOfWork, IFileHelper fileHelper)
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
            FuarTestVM pageVM = new FuarTestVM()
            {
                FuarTest = new FuarTest()
            };

            if (id == null || id == 0)
            {
                // Create new page
                return View(pageVM);
            }
                // Edit existing page
            pageVM.FuarTest = _unitOfWork.FuarTests.Get(u => u.Id == id);
            if (pageVM.FuarTest == null)
            {
                return NotFound();
            }
            return View(pageVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Upsert(FuarTestVM pageVM)
        {
            if (ModelState.IsValid)
            {
                if (pageVM.FuarTest.Id == 0)
                {
                    _unitOfWork.FuarTests.Add(pageVM.FuarTest);
                    TempData["success"] = "Page created successfully.";
                }
                else
                {
                    _unitOfWork.FuarTests.Update(pageVM.FuarTest);
                    TempData["success"] = "Page updated successfully.";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(pageVM);
        }



        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.FuarTests.GetAll();
            return Json(new { data = allObj });
        }
    }
}
