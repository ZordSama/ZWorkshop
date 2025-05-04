namespace z_workshop_server.BLL.DTOs;

#region User related DTOs
public class UserDTO
{
    public string UserId { get; set; } = null!;
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

public class CustomerWithUserDTO
{
    public CustomerDTO CustomerDto { get; set; } = null!;
    public UserDTO UserDto { get; set; } = null!;
}

public class EmployeeDTO : PersonDTO
{
    public string EmployeeId { get; set; } = null!;
    public DateTime HiredDate { get; set; }
}

public class EmployeeWithUserDTO
{
    public EmployeeDTO EmployeeDto { get; set; } = null!;
    public UserDTO UserDto { get; set; } = null!;
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
    public string PublisherName { get; set; } = null!;
}

public class ProductDescJsonDTO
{
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
}

public class PublisherDTO
{
    public string PublisherId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Avt { get; set; } = null!;
    public int Status { get; set; }
    public string Email { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdate { get; set; }
}

public class PurchaseDTO
{
    public string PurchaseId { get; set; } = null!;
    public string CustomerId { get; set; } = null!;
    public string CustomerFullname { get; set; } = null!;
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public DateTime? CloseAt { get; set; }
    public decimal UnitPrice { get; set; } = 0!;
}
#endregion
