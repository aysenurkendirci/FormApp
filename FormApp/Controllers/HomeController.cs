using FormsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FormsApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string? searchString, string? category)
        {
            var products = Repository.Products;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                ViewBag.SearchString = searchString;

                products = products
                    .Where(p => p.Name != null &&
                                p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(category) && category != "0")
            {
                if (int.TryParse(category, out int selectedCategory))
                {
                    products = products.Where(p => p.CategoryId == selectedCategory).ToList();
                }
            }

            var model = new ProductViewModel
            {
                Products = products,
                Categories = Repository.Category,
                SelectedCategory = category
            };

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = Repository.Category;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile? imageFile)
        {
            string? extension = null;

            // Dosya seçildiyse uzantı kontrolü
            if (imageFile != null)
            {
                var allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
                extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz!");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = Repository.Category;
                return View(model);
            }

            // Dosya varsa kaydet
            if (imageFile != null)
            {
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                model.Image = randomFileName;
            }

            model.ProductId = Repository.Products.Count + 1;
            Repository.CreateProduct(model);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null) return NotFound();

            ViewBag.Categories = Repository.Category;
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
        {
            if (id != model.ProductId) return NotFound();

            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                model.Image = randomFileName;
            }

            Repository.EditProduct(model);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null) return NotFound();

            return View("DeleteConfirm", entity);
        }

        [HttpPost]
        public IActionResult Delete(int id, int ProductId)
        {
            if (id != ProductId) return NotFound();

            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == ProductId);
            if (entity == null) return NotFound();

            Repository.DeleteProduct(entity);
            return RedirectToAction("Index");
        }

        public IActionResult EditProducts(List<Product> Products)
        {
            foreach (var product in Products)
            {
                Repository.EditIsActive(product);
            }
            return RedirectToAction("Index");
        }
    }
}

