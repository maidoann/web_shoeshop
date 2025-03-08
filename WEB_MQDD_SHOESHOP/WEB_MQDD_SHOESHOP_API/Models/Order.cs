using System;
using System.Collections.Generic;

namespace WEB_MQDD_SHOESHOP_API.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; } = null!;
}
