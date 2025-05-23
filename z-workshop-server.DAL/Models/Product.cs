﻿using System;
using System.Collections.Generic;

namespace z_workshop_server.DAL.Models;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Type { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public string? Desc { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string ApprovedBy { get; set; } = null!;

    public string PublisherId { get; set; } = null!;

    public virtual Employee ApprovedByNavigation { get; set; } = null!;

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } =
        new List<PurchaseDetail>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
