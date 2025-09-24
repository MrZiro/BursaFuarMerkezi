using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class StandReqController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StandReqController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var requests = _unitOfWork.StandRequest.GetAll().OrderByDescending(s => s.CreatedAt);
            return View(requests);
        }

        public IActionResult Details(int id)
        {
            var request = _unitOfWork.StandRequest.Get(u => u.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var request = _unitOfWork.StandRequest.Get(u => u.Id == id);
            if (request != null)
            {
                _unitOfWork.StandRequest.Remove(request);
                _unitOfWork.Save();
                TempData["success"] = "Talep başarıyla silindi.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
