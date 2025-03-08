using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_MQDD_SHOESHOP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB_MQDD_SHOESHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        ShoeShopDbContext _context;
        public ProductsApiController(ShoeShopDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("/Home/GetProductsHome")]
        public async Task<ActionResult> GetProductsHome()
        {
                var products = await _context.Products
            .Where(p => !p.Deleted)
            .Include(p => p.Brand) // Include Brand explicitly
            .Select(p => new
            {
                p.Id,
                p.Name,
                ImagePath = p.ProductImages.FirstOrDefault() != null
                    ? p.ProductImages.FirstOrDefault().Path
                    : null,
                Price = p.ProductDetails.FirstOrDefault() != null
                    ? p.ProductDetails.FirstOrDefault().Price
                    : 0m,
                BrandName = p.Brand != null
                    ? p.Brand.Name
                    : null
            })
            .Take(4)
            .ToListAsync();


            return Ok(new { data = products });
        }

        [HttpGet]
        [Route("/Home/GetProductsShop")]
        public async Task<ActionResult> GetProductsShop() {
            var products = await _context.Products
            .Where(p => !p.Deleted)
            .Include(p => p.Brand) // Include Brand explicitly
            .Include(p => p.ProductDetails)
            .Include(p => p.ProductCategory)
            .Include(p => p.ProductImages)
            .Select(p => new
            {
                p.Id,
                p.Name,
                ImagePath = p.ProductImages.FirstOrDefault() != null
                    ? p.ProductImages.FirstOrDefault().Path
                    : null,
                Price = p.ProductDetails.FirstOrDefault() != null
                    ? p.ProductDetails.FirstOrDefault().Price
                    : 0m,
                BrandName = p.Brand != null
                    ? p.Brand.Name
                    : null,
                BrandId =  p.BrandId,
                ProductCategoryId = p.ProductCategoryId ,
                CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : null 

            })
            .ToListAsync();
            return Ok(new { data = products }); 
        }
        [HttpGet("/Home/GetProduct/{id}")]

        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Where(p => !p.Deleted && p.Id == id)
                .Include(p => p.Brand)
                .Include(p => p.ProductDetails)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductImages)
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name,
                    BrandName = p.Brand != null ? p.Brand.Name : null,
                    CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : null,
                    Price = p.ProductDetails.FirstOrDefault() != null ? p.ProductDetails.FirstOrDefault().Price : 0m,
                    Description = p.Description,
                    Content = p.Content,
                    Qty = p.ProductDetails.FirstOrDefault() != null ? p.ProductDetails.FirstOrDefault().Qty : null ,
                    Color = p.ProductDetails.FirstOrDefault() != null ? p.ProductDetails.FirstOrDefault().Color : null,
                    Size = p.ProductDetails.FirstOrDefault() != null ? p.ProductDetails.FirstOrDefault().Size : null,
                    ImagePaths = p.ProductImages.Select(pi => pi.Path).ToList()
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //public ActionResult GetProducts()
        //{
        //    var products = _context.Products
        //     .Where(p => !p.Deleted)
        //     .Include(p => p.ProductDetails)
        //     .Include(p => p.ProductImages)
        //     .OrderByDescending(p => p.Id)
        //     .Take(4)
        //     .ToListAsync();
        //    return Ok(new { data = products });
        //}



    }
}
