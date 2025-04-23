namespace z_workshop_server.DAL.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Role { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime HiredDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    public int Status { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User User { get; set; } = null!;
}
