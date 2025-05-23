﻿using System;
using System.Collections.Generic;

namespace z_workshop_server.DAL.Models;

public partial class Publisher
{
    public string PublisherId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Avt { get; set; }

    public int Status { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
