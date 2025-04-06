using System;
using System.Collections.Generic;

namespace z_workshop_server.Models;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Type { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public string? Desc { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string ApprovedBy { get; set; } = null!;

    public string PublisherId { get; set; } = null!;

    public virtual Employee ApprovedByNavigation { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomersNavigation { get; set; } = new List<Customer>();
}
