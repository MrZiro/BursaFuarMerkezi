using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;

        public ProductController(IUnitOfWork unitOfWork, IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();


            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);

                if (productVM.Product == null)
                {
                    return NotFound();
                }
            }

            return View(productVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM, IFormFile? file)
        {
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            if (ModelState.IsValid)
            {
                try
                {
                    productVM.Product.ImageUrl = await _fileHelper.SaveFileAsync(file, productVM.Product.ImageUrl);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("file", "Error saving file: " + ex.Message);
                    Console.Error.WriteLine($"File save error in Upsert: {ex}");
                    return View(productVM);
                }



                string successMessage;
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    successMessage = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    successMessage = "Product updated successfully";
                }

                _unitOfWork.Save();

                TempData["success"] = successMessage;

                return RedirectToAction("Index");
            }
            else
            {
                return View(productVM);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Invalid ID." });
            }

            Product? productToDelete = _unitOfWork.Product.Get(u => u.Id == id);

            if (productToDelete == null)
            {
                return Json(new { success = false, message = "Error: Product not found." });
            }

            try
            {
                if (!string.IsNullOrEmpty(productToDelete.ImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(productToDelete.ImageUrl);
                }

                _unitOfWork.Product.Remove(productToDelete);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting product {id}: {ex}");
                return Json(new { success = false, message = "An error occurred while deleting the product." });
            }
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // --- DataTables parameters ---
                var draw = Request.Query["draw"].FirstOrDefault() ?? "1";
                int start = int.TryParse(Request.Query["start"].FirstOrDefault(), out var s) ? s : 0;
                int length = int.TryParse(Request.Query["length"].FirstOrDefault(), out var l) ? l : 5;
                string searchValue = Request.Query["search[value]"].FirstOrDefault() ?? string.Empty;



                // --- Sorting ---
                var sortColumnIndex = Request.Query["order[0][column]"].FirstOrDefault();
                string sortColumn = !string.IsNullOrEmpty(sortColumnIndex)
                    ? Request.Query[$"columns[{sortColumnIndex}][data]"].FirstOrDefault()
                    : "Id";
                
                // Get sort direction from request, default to "asc" if not provided
                string sortDirection = Request.Query["order[0][dir]"].FirstOrDefault()?.ToLower();

                // Only set default if sort column or direction is missing
                if (string.IsNullOrWhiteSpace(sortColumn))
                    sortColumn = "Id";
                
                // Only default the direction if it's empty or invalid
                if (string.IsNullOrWhiteSpace(sortDirection) || (sortDirection != "asc" && sortDirection != "desc"))
                    sortDirection = "desc";

                // --- Get paged, filtered, sorted data from repository ---
                var (data, filteredTotal, total) = await _unitOfWork.Product.GetPagedAsync(
                    start, length, sortColumn, sortDirection, searchValue, "Category"
                );

                // --- Return in DataTables format ---
                return Json(new
                {
                    draw = draw,
                    recordsTotal = total,
                    recordsFiltered = filteredTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                // Log the error (replace with your logger if available)
                Console.Error.WriteLine($"[ProductController.GetAll] Error: {ex}");

                // Return DataTables error format
                return Json(new
                {
                    draw = 0,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = "An error occurred while processing your request."
                });
            }
        }
        #endregion


    }
}
