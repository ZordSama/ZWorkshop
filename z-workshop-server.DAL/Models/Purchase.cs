using System;
using System.Collections.Generic;

namespace z_workshop_server.DAL.Models;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public decimal TotalValue { get; set; }

    public DateTime CloseAt { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } =
        new List<PurchaseDetail>();
}
