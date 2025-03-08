using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP_API.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductDetailId { get; set; }

    public int Qty { get; set; }

    public decimal Total { get; set; }

    public virtual Order? Order { get; set; } = null!;

    public virtual ProductDetail? ProductDetail { get; set; } = null!;
}
