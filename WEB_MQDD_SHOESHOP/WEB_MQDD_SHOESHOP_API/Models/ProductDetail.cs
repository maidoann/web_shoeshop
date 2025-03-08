using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP_API.Models;

public partial class ProductDetail
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Color { get; set; } = null!;

    public string Size { get; set; } = null!;

    public decimal Price { get; set; }

    public int? Qty { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product? Product { get; set; } = null!;
}
