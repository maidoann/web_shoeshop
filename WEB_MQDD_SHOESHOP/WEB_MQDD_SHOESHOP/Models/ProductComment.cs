using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP.Models;

public partial class ProductComment
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Messages { get; set; }

    public decimal Rating { get; set; }

    public virtual Product? Product { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
