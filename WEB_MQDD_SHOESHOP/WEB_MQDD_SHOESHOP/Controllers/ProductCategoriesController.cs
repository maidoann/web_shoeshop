using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_MQDD_SHOESHOP.Models;

namespace WEB_MQDD_SHOESHOP.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly ShoeShopDbContext _context;

        public ProductCategoriesController(ShoeShopDbContext context)
        {
            _context = context;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 5;

            // Tổng số bản ghi chưa bị xóa
            var totalItems = await _context.ProductCategories.CountAsync(p => !p.Deleted);

            // Lấy danh sách thương hiệu chưa bị xóa
            var productCategories = await _context.ProductCategories
                .Where(p => !p.Deleted) // Chỉ lấy các bản ghi có Deleted = false
                .OrderBy(p => p.Id) // Sắp xếp theo Id
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi cho trang hiện tại
                .ToListAsync();

            var model = new PaginatedList<ProductCategory>(productCategories, totalItems, page, pageSize);
            return View("~/Views/Admin/ProductCategories/Index.cshtml", model);
        }

        // GET: ProductCategories/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var productCategory = await _context.ProductCategories
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (productCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productCategory);
        //}

        // GET: ProductCategories/Create
        public IActionResult Create(int page)
        {
            ViewBag.CurrentPage = page;
            return View("~/Views/Admin/ProductCategories/Create.cshtml", new ProductCategory());
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Deleted")] ProductCategory productCategory, int page)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {page});
            }
            return View("~/Views/Admin/ProductCategories/Index.cshtml", page);
        }

        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id,int page)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CurrentPage = page;

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/ProductCategories/Edit.cshtml", productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Deleted")] ProductCategory productCategory, int page)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { page });
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var productCategory = await _context.ProductCategories
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (productCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productCategory);
        //}

        // POST: ProductCategories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            // Thay vì xóa, cập nhật Deleted thành true
            productCategory.Deleted = true;
            _context.Update(productCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new {page});
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }
    }
}
