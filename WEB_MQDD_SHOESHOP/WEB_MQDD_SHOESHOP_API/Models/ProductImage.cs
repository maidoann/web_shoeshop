using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP_API.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? Path { get; set; }

    public virtual Product? Product { get; set; } = null!;
}
