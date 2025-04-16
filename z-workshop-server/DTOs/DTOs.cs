namespace z_workshop_server.DTOs;

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

// public class ZActionResult
// {
//     public bool Success { get; set; } = false;
//     public string Message { get; set; } = null!;

//     public ZActionResult(bool success, string message)
//     {
//         Success = success;
//         Message = message;
//     }
// }
