using System;
using System.Collections.Generic;

namespace z_workshop_server.Models;

public partial class PurchaseDetail
{
    public string PurchaseId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
