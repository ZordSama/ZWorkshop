namespace z_workshop_server.DTOs;

#region User related DTOs
public class UserDTO
{
    public string? UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Avatar { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdate { get; set; }
}

public class UserAuthDTO : UserDTO
{
    public string Password { get; set; } = null!;
}

public class PersonDTO
{
    public string Fullname { get; set; } = null!;
    public DateOnly Dob { get; set; }
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int Status { get; set; } = 0;
    public string UserId { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdate { get; set; }
}

public class CustomerDTO : PersonDTO
{
    public string CustomerId { get; set; } = null!;
    public string? Payments { get; set; }
}

public class EmployeeDTO : PersonDTO
{
    public string EmployeeId { get; set; } = null!;
}

public class ProductToPurchaseDTO
{
    public required string ProductId { get; set; }
    public int Quantity { get; set; }
}
#endregion

#region Product related DTOs
public class ProductDTO
{
    public string ProductId { get; set; } = null!;
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Type { get; set; }
    public required string Genre { get; set; }
    public required string? Desc { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string ApprovedBy { get; set; } = null!;
    public string PublisherId { get; set; } = null!;
}

#endregion
