using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP_API.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Deleted { get; set; } = false;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
