using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WEB_MQDD_SHOESHOP.Models;

namespace WEB_MQDD_SHOESHOP.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ShoeShopDbContext _context;

        public BrandsController(ShoeShopDbContext context)
        {
            _context = context;
        }

        // GET: Brands/Index
        public async Task<IActionResult> Index(int page = 1 , int pageSize = 5)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 5;

            // Tổng số bản ghi chưa bị xóa
            var totalItems = await _context.Brands.CountAsync(p => !p.Deleted);

            // Lấy danh sách thương hiệu chưa bị xóa
            var brands = await _context.Brands
                .Where(p => !p.Deleted) // Chỉ lấy các bản ghi có Deleted = false
                .OrderBy(p => p.Id) // Sắp xếp theo Id
                .Skip((page - 1) * pageSize) // Bỏ qua bản ghi của các trang trước
                .Take(pageSize) // Lấy số bản ghi cho trang hiện tại
                .ToListAsync();

            var model = new PaginatedList<Brand>(brands, totalItems, page, pageSize);
            return View("~/Views/Admin/Brands/Index.cshtml", model);
        }

        // GET: Brands/Create
        [HttpGet]
        public IActionResult Create(int page)
        {
            ViewBag.CurrentPage = page;
            return View("~/Views/Admin/Brands/Create.cshtml",new Brand());
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Brand brand,int page)
        {
            Console.Write("da vao create");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var fieldName = entry.Key; // Tên trường (field name)
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Trường '{fieldName}' bị lỗi: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                // Không cần gán Deleted vì mặc định là false trong model
                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {page});
            }

            return View("~/Views/Admin/Brands/Create.cshtml", brand);
        }

        // GET: Brands/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int page)
        {
            if (id == null) return NotFound();

            var brand = await _context.Brands
                .Where(p => !p.Deleted) // Chỉ lấy thương hiệu chưa bị xóa
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.CurrentPage = page;

            if (brand == null) return NotFound();

            return View("~/Views/Admin/Brands/Edit.cshtml", brand);
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Brand brand,int page)
        {
            if (id != brand.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View("~/Views/Admin/Brands/Edit.cshtml", brand);
            }

            var existingBrand = await _context.Brands
                .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted);

            if (existingBrand == null) return NotFound();

            if (ModelState.IsValid)
            {
                    // Cập nhật chỉ trường Name
                    existingBrand.Name = brand.Name;
                    // Không sửa Deleted vì không cần thay đổi trạng thái
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {page});
            }

            return View("~/Views/Admin/Brands/Edit.cshtml", brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id, int page)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            brand.Deleted = true;
            _context.Update(brand);
            await _context.SaveChangesAsync();

            // Sửa cú pháp RedirectToAction để truyền page
            return RedirectToAction(nameof(Index), new { page });
        }
        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id && !e.Deleted);
        }
    }
}