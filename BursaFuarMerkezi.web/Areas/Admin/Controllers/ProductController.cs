using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Models.ViewModels;
using BursaFuarMerkezi.Utility;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BursaFuarMerkezi.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // --- Inject required services (IUnitOfWork, IFileHelper) ---
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;

        // Constructor to receive injected services
        public ProductController(IUnitOfWork unitOfWork, IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper; // Assign the injected FileHelper
        }

        // GET action for the Product List page
        // URL example: /Admin/Product/Index
        public IActionResult Index()
        {
            // Get all products from the database, including the related Category for display
            // Assumes GetAll method in IRepository supports includeProperties
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            // Note: HTML sanitization/stripping for display in the list is omitted as requested.
            // If you decide to add it back, process product.Description here before returning the list.

            return View(productList);
        }

        // GET action for the Create/Edit form (Upsert)
        // URL example: /Admin/Product/Upsert or /Admin/Product/Upsert/5
        public IActionResult Upsert(int? id)
        {
            // Create and populate the ViewModel with a new Product and the Category list
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            // If id is null or 0, it's a Create operation
            if (id == null || id == 0)
            {
                // Return the view with the empty Product and populated CategoryList
                return View(productVM);
            }
            else
            {
                // Case: Update Product
                // Fetch the existing product from the database by Id
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);

                // If product not found, return 404
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                // CategoryList is already populated above
            }

            // Return the view with the Product data and populated CategoryList
            return View(productVM);
        }


        // POST action for Create/Edit form submission
        // Handles both creation and updating based on Product.Id
        // URL example: POST to /Admin/Product/Upsert
        [HttpPost]
        [ValidateAntiForgeryToken] // Crucial for security when accepting POST requests
        // Accept the ViewModel and the uploaded file
        public async Task<IActionResult> Upsert(ProductVM productVM, IFormFile? file) // Made IFormFile nullable as image upload is optional
        {
            // --- Re-populate CategoryList if ModelState is invalid ---
            // This must be done BEFORE returning the view in case of validation errors
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            // ------------------------------------------------------

            // Check if the model state is valid based on data annotations on ProductVM.Product
            if (ModelState.IsValid)
            {
                // --- File Handling using the Injected FileHelper ---
                // The SaveFileAsync method handles saving the new file and deleting the old one
                // It returns the new ImageUrl or the existing one if no new file is uploaded
                try
                {
                    productVM.Product.ImageUrl = await _fileHelper.SaveFileAsync(file, productVM.Product.ImageUrl);
                }
                catch (InvalidOperationException ex) // Catch exceptions thrown by FileHelper
                {
                    // Handle file saving error (e.g., add error to ModelState, log the exception)
                    ModelState.AddModelError("file", "Error saving file: " + ex.Message);
                    // Log the exception details here if you have logging configured
                    Console.Error.WriteLine($"File save error in Upsert: {ex}");
                    // Return the view with errors and the re-populated CategoryList
                    return View(productVM);
                }
                // -----------------------------------------------

                // Note: HTML Sanitization for productVM.Product.Description is omitted as requested.
                // If you decide to add it back, the code block should go here before adding/updating the product in DB.
                // Example: productVM.Product.Description = yourSanitizer.Sanitize(productVM.Product.Description);


                // --- Database Add or Update ---
                string successMessage;
                // If Product.Id is 0, it's a new product (Add)
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    successMessage = "Product created successfully";
                }
                else // If Product.Id is not 0, it's an existing product (Update)
                {
                    // Update the product entity in the DbContext's change tracker
                    // Assuming your Repository/UnitOfWork handles updates correctly,
                    // often just calling SaveChanges is enough if the entity was tracked or attached.
                    // If not using a tracked entity, you might need _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Product.Update(productVM.Product); // Added explicit Update call for clarity
                    successMessage = "Product updated successfully";
                }

                // Save all changes in the Unit of Work to the database
                _unitOfWork.Save();

                // Set the success message in TempData to be displayed on the Index page
                TempData["success"] = successMessage;

                // Redirect to the Product Index page
                return RedirectToAction("Index");
            }
            else // If ModelState is NOT valid (validation errors occurred on the form)
            {
                // CategoryList was already re-populated at the beginning of the action
                // Return the view so validation errors are displayed by tag helpers
                return View(productVM);
            }
        }

        // POST action for Deleting a Product (intended to be called via AJAX from Index page)
        // URL example: POST to /Admin/Product/Delete/5
        [HttpDelete]
        //[ValidateAntiForgeryToken] // Crucial for security
        // Accept the ID of the product to delete
        public async Task<IActionResult> Delete(int? id) // Made id nullable for safety
        {
            // Check if the provided ID is valid
            if (id == null || id == 0)
            {
                // Return a JSON response indicating failure
                return Json(new { success = false, message = "Invalid ID." });
            }

            // Get the product to delete from the database
            Product? productToDelete = _unitOfWork.Product.Get(u => u.Id == id);

            // If the product was not found
            if (productToDelete == null)
            {
                // Return a JSON response indicating failure (Product not found)
                return Json(new { success = false, message = "Error: Product not found." });
            }

            try
            {
                // --- Delete the associated image file using the Injected FileHelper ---
                if (!string.IsNullOrEmpty(productToDelete.ImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(productToDelete.ImageUrl);
                }
                // ---------------------------------------------------

                // Remove the product entity from the database context
                _unitOfWork.Product.Remove(productToDelete);
                // Save changes to the database
                _unitOfWork.Save();

                // Return a success JSON response
                return Json(new { success = true, message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                // Handle deletion error (e.g., database error, file deletion error)
                // Log the error details
                Console.Error.WriteLine($"Error deleting product {id}: {ex}");
                // Return an error JSON response
                return Json(new { success = false, message = "An error occurred while deleting the product." });
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = productList });
        }
        #endregion


    }
}
