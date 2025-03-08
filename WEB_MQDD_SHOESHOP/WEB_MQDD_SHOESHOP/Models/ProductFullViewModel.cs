
using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP.Models;

public partial class ProductFullViewModel
{
    // Từ Product
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? Tag { get; set; }
    public bool? Deleted { get; set; } // Có thể null nếu không cần kiểm tra trạng thái

    // Từ Brand
    public int? BrandId { get; set; }
    public string? BrandName { get; set; }

    // Từ ProductCategory
    public int? ProductCategoryId { get; set; }
    public string? CategoryName { get; set; }

    // Từ ProductDetail (lấy thông tin của một ProductDetail đầu tiên hoặc đại diện)
    public int? ProductDetailId { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal? Price { get; set; }
    public int? Qty { get; set; }

    // Từ ProductImage (lấy thông tin của một ProductImage đầu tiên hoặc đại diện)
    public int? ProductImageId { get; set; }
    public string? ImagePath { get; set; }

    // Nếu cần danh sách (tuỳ chọn)
    public List<string>? ImagePaths { get; set; } // Danh sách tất cả ảnh nếu cần
    public List<ProductDetailInfo>? ProductDetails { get; set; } // Danh sách chi tiết nếu cần
    public class ProductDetailInfo
    {
        public int Id { get; set; }
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
        public int? Qty { get; set; }
    }
}
