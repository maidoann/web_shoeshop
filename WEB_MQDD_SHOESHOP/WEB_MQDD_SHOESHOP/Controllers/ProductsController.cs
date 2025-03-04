using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_MQDD_SHOESHOP.Models;

namespace MQDD_SHOESHOP_V1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShoeShopDbContext _context;

        public ProductsController(ShoeShopDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            if (page < 1) page = 1; // Đảm bảo page không âm
            if (pageSize < 1) pageSize = 5; // Đảm bảo pageSize không âm

            // Tổng số bản ghi
            var totalItems = await _context.Products.CountAsync();

            // Lấy danh sách sản phẩm theo trang, bao gồm các mối quan hệ
            var products = await _context.Products
                        .Where(p => !p.Deleted) // Chỉ lấy các bản ghi có Deleted = false
                        .Include(p => p.Brand)
                        .Include(p => p.ProductCategory)
                        .Include(p => p.ProductImages)
                        .Include(p => p.ProductDetails)
                        .OrderBy(p => p.Id) // Sắp xếp theo Id (hoặc bạn có thể thay bằng trường khác như Name)
                        .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                        .Take(pageSize) // Lấy số bản ghi cho trang hiện tại
                        .ToListAsync();

            // Tạo model phân trang
            var model = new PaginatedList<Product>(products, totalItems, page, pageSize);

            // Chỉ định rõ đường dẫn đến View
            return View("~/Views/Admin/Products/Index.cshtml", model);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Products/Detail.cshtml",product);
        }

        public IActionResult Create(int page = 1)
        {
            ViewBag.CurrentPage = page;
            var brandList = _context.Brands.OrderBy(b => b.Name).ToList();
            ViewBag.Brand = new SelectList(brandList, "Id", "Name");

            var categoryList = _context.ProductCategories.OrderBy(b => b.Name).ToList();
            ViewBag.Category = new SelectList(categoryList, "Id", "Name");

            return View("~/Views/Admin/Products/Create.cshtml", new Product());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, decimal Price, int StockQuantity, List<string> Sizes, List<string> Colors, IFormFile ImageUrl, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                // Lưu Product
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Lưu ProductDetail
                var productDetail = new ProductDetail
                {
                    ProductId = product.Id,
                    Color = string.Join(",", Colors ?? new List<string>()), // Ghép Colors thành chuỗi
                    Size = string.Join(",", Sizes ?? new List<string>()),   // Ghép Sizes thành chuỗi
                    Price = Price,
                    Qty = StockQuantity
                };
                _context.ProductDetails.Add(productDetail);

                // Lưu ProductImage
                if (ImageUrl != null)
                {
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageUrl.FileName);
                    var filePath = Path.Combine(imagesFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }

                    var productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        Path = "/images/" + uniqueFileName
                    };
                    _context.ProductImages.Add(productImage);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { page });
            }

            // Nếu ModelState không hợp lệ, tải lại danh sách Brand và Category
            ViewBag.CurrentPage = page;
            var brandList = _context.Brands.OrderBy(b => b.Name).ToList();
            ViewBag.Brand = new SelectList(brandList, "Id", "Name");

            var categoryList = _context.ProductCategories.OrderBy(b => b.Name).ToList();
            ViewBag.Category = new SelectList(categoryList, "Id", "Name");

            return View("~/Views/Admin/Products/Create.cshtml", product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CurrentPage = page;
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductDetails)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var brandList = _context.Brands.OrderBy(b => b.Name).ToList();
            ViewBag.Brand = new SelectList(brandList, "Id", "Name", product.BrandId);

            var categoryList = _context.ProductCategories.OrderBy(b => b.Name).ToList();
            ViewBag.Category = new SelectList(categoryList, "Id", "Name", product.ProductCategoryId);

            return View("~/Views/Admin/Products/Edit.cshtml", product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int page, int id, Product product, [BindRequired] decimal Price, [BindRequired] int StockQuantity, [BindRequired] List<string> Sizes, [BindRequired] List<string> Colors, List<IFormFile> ImageUrls = null, List<int> RemoveImageIds = null, bool RemoveAllImages = false)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                // Tải lại danh sách Brand và Category
                ViewBag.CurrentPage = page;
                var brandList = _context.Brands.OrderBy(b => b.Name).ToList();
                ViewBag.Brand = new SelectList(brandList, "Id", "Name", product.BrandId);
                var categoryList = _context.ProductCategories.OrderBy(b => b.Name).ToList();
                ViewBag.Category = new SelectList(categoryList, "Id", "Name", product.ProductCategoryId);
                return View("~/Views/Admin/Products/Edit.cshtml", product);
            }

            var existingProduct = await _context.Products
                .Include(p => p.ProductDetails)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Cập nhật Product
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Content = product.Content;
            existingProduct.Tag = product.Tag;
            existingProduct.BrandId = product.BrandId;
            existingProduct.ProductCategoryId = product.ProductCategoryId;

            // Cập nhật ProductDetail
            var productDetail = existingProduct.ProductDetails.FirstOrDefault() ?? new ProductDetail { ProductId = id };
            productDetail.Color = string.Join(",", Colors);
            productDetail.Size = string.Join(",", Sizes); // Ghép Sizes thành chuỗi
            productDetail.Price = Price;
            productDetail.Qty = StockQuantity;
            if (existingProduct.ProductDetails.Count == 0)
            {
                _context.ProductDetails.Add(productDetail);
            }

            // Xử lý ảnh
            if (RemoveAllImages)
            {
                foreach (var image in existingProduct.ProductImages.ToList())
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Path.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    _context.ProductImages.Remove(image);
                }
            }
            else if (RemoveImageIds != null && RemoveImageIds.Any())
            {
                foreach (var imageId in RemoveImageIds)
                {
                    var imageToRemove = existingProduct.ProductImages.FirstOrDefault(i => i.Id == imageId);
                    if (imageToRemove != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageToRemove.Path.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        _context.ProductImages.Remove(imageToRemove);
                    }
                }
            }

            if (ImageUrls != null)
            {
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                foreach (var imageUrl in ImageUrls)
                {
                    if (imageUrl != null && imageUrl.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageUrl.FileName);
                        var filePath = Path.Combine(imagesFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageUrl.CopyToAsync(stream);
                        }

                        var productImage = new ProductImage
                        {
                            ProductId = id,
                            Path = "/images/" + uniqueFileName
                        };
                        _context.ProductImages.Add(productImage);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { page });
        }


        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Brand)
        //        .Include(p => p.ProductCategory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // POST: Products/Delete/5
        // POST: Products/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int page)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                // Thay vì xóa, chỉ cập nhật Deleted thành true
                product.Deleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new {page});
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
