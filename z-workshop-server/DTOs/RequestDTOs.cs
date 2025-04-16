using System.ComponentModel.DataAnnotations;

namespace z_workshop_server.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}

public class UserFormData
{
    [Required(ErrorMessage = "Username is required")]
    [Length(3, 20, ErrorMessage = "Username must be between 3 and 20 characters")]
    [RegularExpression(
        @"^[a-zA-Z0-9_]+$",
        ErrorMessage = "Username can only contain letters and numbers or underscore"
    )]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [Length(8, 20, ErrorMessage = "Password must be between 8 and 20 characters")]
    public required string Password { get; set; }

    public string? Role { get; set; }
}

public class CustomerFormData
{
    [Required(ErrorMessage = "Full Name is required")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Dob is required")]
    public required DateTime Dob { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
    public required string Phone { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public required string Email { get; set; } = "";

    [Required(ErrorMessage = "Address is required")]
    public required string Address { get; set; }
    public int Status { get; set; } = 0;
}

public class EmployeeFormData : CustomerFormData
{
    [Required(ErrorMessage = "Role is required")]
    public required string Role { get; set; }
}

public class CustomerRegisterRequest
{
    public required UserFormData UserFormData { get; set; }
    public required CustomerFormData CustomerFormData { get; set; }
}

public class EmployeeIssueRequest
{
    public required UserFormData UserFormData { get; set; }
    public required EmployeeFormData EmployeeFormData { get; set; }
}
