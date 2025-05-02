using System.ComponentModel.DataAnnotations;

namespace z_workshop_server.BLL.DTOs;

#region Accounts related requests DTOs

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}

public class BaseUserForm
{
    [Required(ErrorMessage = "Username is required")]
    [Length(3, 20, ErrorMessage = "Username must be between 3 and 20 characters")]
    [RegularExpression(
        @"^[a-zA-Z0-9_]+$",
        ErrorMessage = "Username can only contain letters and numbers or underscore"
    )]
    public required string Username { get; set; }
}

public class UserFormData : BaseUserForm
{
    [Required(ErrorMessage = "Password is required")]
    [Length(8, 20, ErrorMessage = "Password must be between 8 and 20 characters")]
    public required string Password { get; set; }

    public string? Role { get; set; }
}

public class BasePersonFormData
{
    [Required(ErrorMessage = "Full Name is required")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Dob is required")]
    public required DateOnly Dob { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
    public required string Phone { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public required string Email { get; set; } = "";

    [Required(ErrorMessage = "Address is required")]
    public required string Address { get; set; }
}

public class CustomerFormData : BasePersonFormData
{
    public int Status { get; set; } = 0;
}

public class EmployeeFormData : CustomerFormData
{
    [Required(ErrorMessage = "Role is required")]
    public required string Role { get; set; }

    [Required(ErrorMessage = "Hired Date is required")]
    public required DateTime HiredDate { get; set; }
}

public class CustomerRegisterRequest
{
    public required UserFormData UserFormData { get; set; }
    public required CustomerFormData CustomerFormData { get; set; }
}

public class CustomerUpdateFormData : BasePersonFormData
{
    public required string CustomerId { get; set; }
}

public class EmployeeUpdateFormData : BasePersonFormData
{
    public required string EmployeeId { get; set; }
}

public class EmployeeIssueRequest
{
    public required UserFormData UserFormData { get; set; }
    public required EmployeeFormData EmployeeFormData { get; set; }
}

public class ChangePasswordRequest
{
    public required string UserId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
#endregion

#region Products related requests DTOs

public class ProductBaseFormData
{
    [Required(ErrorMessage = "Product name is required")]
    [MaxLength(50, ErrorMessage = "Product name must be less than 50 characters")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public required decimal Price { get; set; }
    public required string Type { get; set; }
    public required string Genre { get; set; }
    public string? Desc { get; set; }
    public IFormFile? Thumbnail { get; set; }
}

public class ProductFormData : ProductBaseFormData
{
    public required string PublisherId { get; set; }
}

public class ProductUpdateFormData : ProductBaseFormData
{
    public required string ProductId { get; set; }
}

public class PublisherFormData
{
    public string Name { get; set; } = null!;
    public string Avt { get; set; } = null!;
    public int Status { get; set; }
    public string Email { get; set; } = null!;
}

public class PublisherUpdateFormData : ProductBaseFormData
{
    public required string PublisherId { get; set; }
}
#endregion

#region Orders, User interacts related requests DTOs
public class PurchaseRequest
{
    public required List<ProductToPurchaseDTO> ProductToPurchase { get; set; }
}

public class UserCommentRequest
{
    public bool Type { get; set; } = false;
    public required string ResponseOf { get; set; }
    public required string Content { get; set; }
    public required string ProductId { get; set; }
}
#endregion
