using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var contacts = _unitOfWork.Contact.GetAll().OrderByDescending(c => c.CreatedAt);
            return View(contacts);
        }

        public IActionResult Details(int id)
        {
            var contact = _unitOfWork.Contact.Get(u => u.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contact = _unitOfWork.Contact.Get(u => u.Id == id);
            if (contact != null)
            {
                _unitOfWork.Contact.Remove(contact);
                _unitOfWork.Save();
                TempData["success"] = "Mesaj başarıyla silindi.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
