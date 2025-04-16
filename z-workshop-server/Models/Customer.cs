using System;
using System.Collections.Generic;

namespace z_workshop_server.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Payments { get; set; }

    public DateOnly Dob { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Status { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductsNavigation { get; set; } = new List<Product>();
}
