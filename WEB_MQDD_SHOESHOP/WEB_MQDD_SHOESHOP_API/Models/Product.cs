using System;
using System.Collections.Generic;
namespace WEB_MQDD_SHOESHOP_API.Models;

public partial class Product
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Tag { get; set; }

    public bool Deleted { get; set; } = false;

    public virtual Brand? Brand { get; set; } = null!;

    public virtual ProductCategory? ProductCategory { get; set; } = null!;

    public virtual ICollection<ProductComment> ProductComments { get; set; } = new List<ProductComment>();

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
